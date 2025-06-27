using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ternary3.Generators
{
    using System.Text.RegularExpressions;


    [Generator]
    public class TernaryConstantsGenerator : ISourceGenerator
    {
        private static readonly Regex ternaryConstant = new("^ter(_*[01T]){1,40}$", RegexOptions.Compiled);

        private class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> PartialClasses { get; } = [];

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                // Add partial classes to our list
                if (syntaxNode is ClassDeclarationSyntax classSyntax &&
                    classSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                {
                    PartialClasses.Add(classSyntax);
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // Check for [assembly: GenerateTernaryConstants]
            var assemblyAttributes = context.Compilation.Assembly.GetAttributes();
            var assemblyAttr = assemblyAttributes.FirstOrDefault(attr => attr.AttributeClass?.Name == "GenerateTernaryConstantsAttribute");
            var assemblyEnabled = true;
            if (assemblyAttr != null)
            {
                if (assemblyAttr.ConstructorArguments.Length == 1 &&
                    assemblyAttr.ConstructorArguments[0].Kind == TypedConstantKind.Primitive &&
                    assemblyAttr.ConstructorArguments[0].Value is bool)
                {
                    assemblyEnabled = (bool)assemblyAttr.ConstructorArguments[0].Value;
                }
            }
            var syntaxReceiver = (SyntaxReceiver)context.SyntaxReceiver;
            if (syntaxReceiver == null) return;

            foreach (var cls in syntaxReceiver.PartialClasses)
            {
                // Check for class-level attribute
                var classHasAttribute = false;
                var classEnabled = false;
                foreach (var attrList in cls.AttributeLists)
                {
                    foreach (var attr in attrList.Attributes)
                    {
                        var name = attr.Name.ToString();
                        if (name.EndsWith("GenerateTernaryConstants") || name.EndsWith("GenerateTernaryConstantsAttribute"))
                        {
                            classHasAttribute = true;
                            if (attr.ArgumentList == null || attr.ArgumentList.Arguments.Count == 0)
                            {
                                classEnabled = true;
                            }
                            else if (attr.ArgumentList.Arguments[0].Expression is LiteralExpressionSyntax literal && literal.IsKind(SyntaxKind.TrueLiteralExpression))
                            {
                                classEnabled = true;
                            }
                            else if (attr.ArgumentList.Arguments[0].Expression is LiteralExpressionSyntax literal2 && literal2.IsKind(SyntaxKind.FalseLiteralExpression))
                            {
                                classEnabled = false;
                            }
                        }
                    }
                }

                var shouldProcess = false;
                if (assemblyAttr == null || assemblyEnabled)
                {
                    // Default or assembly enabled: process all unless class disables
                    shouldProcess = !classHasAttribute || (classHasAttribute && classEnabled);
                }
                else
                {
                    // Assembly not enabled: only process if class enables
                    shouldProcess = classHasAttribute && classEnabled;
                }
                if (!shouldProcess) continue;

                // Try to get the semantic model - this might fail if there are compilation errors
                INamedTypeSymbol classSymbol;

                try
                {
                    var semanticModel = context.Compilation.GetSemanticModel(cls.SyntaxTree);
                    classSymbol = semanticModel.GetDeclaredSymbol(cls);
                    if (classSymbol == null) continue;
                }
                catch
                {
                    // If we can't get the semantic model, try a more resilient approach
                    // using syntax analysis only
                    var className = cls.Identifier.Text;
                    var namespaceName = GetNamespaceFrom(cls);

                    // Find all identifiers that match our pattern from the syntax tree
                    var identifiers = cls.DescendantNodes()
                        .OfType<IdentifierNameSyntax>()
                        .Select(id => id.Identifier.Text)
                        .Where(name => ternaryConstant.IsMatch(name))
                        .Distinct()
                        .ToArray();

                    if (identifiers.Length == 0) continue;

                    // Generate constants based on syntax analysis only
                    GenerateConstants(context, className, namespaceName, identifiers);
                    continue;
                }

                // If we got here, we have a valid semantic model
                // Find all variable references matching the regex
                var identifiersFromSemantics = cls.DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Select(id => id.Identifier.Text)
                    .Where(name => ternaryConstant.IsMatch(name))
                    .Distinct();

                // Find existing members
                var existingMembers = new HashSet<string>(classSymbol.GetMembers().Select(m => m.Name));
                var missing = identifiersFromSemantics.Where(name => !existingMembers.Contains(name)).ToArray();

                if (missing.Length == 0) continue;

                // Generate constants with semantic model
                var namespaceName2 = classSymbol.ContainingNamespace.ToDisplayString();
                GenerateConstants(context, cls.Identifier.Text, namespaceName2, missing);
            }
        }

        private static void GenerateConstants(
            GeneratorExecutionContext context,
            string className,
            string namespaceName,
            IEnumerable<string> constants)
        {
            var sb = new StringBuilder();

            // Include the namespace
            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine($"namespace {namespaceName};");
            }

            sb.AppendLine($"partial class {className}");
            sb.AppendLine("{");
            foreach (var name in constants)
            {
                var value = Conversion.ToLong(name);
                var type = value is < int.MinValue or > int.MaxValue ? "long" : "int";
                sb.AppendLine($"    /// <summary>");
                sb.AppendLine($"    /// Constant with value {value}.");
                sb.AppendLine($"    /// </summary>");
                sb.AppendLine($"    private const {type} {name} = {value};");
            }
            sb.AppendLine("}");
            context.AddSource($"{className}_TernaryConstants.g.cs", sb.ToString());
        }

        private static string GetNamespaceFrom(ClassDeclarationSyntax cls)
        {
            // Walk up the syntax tree to find a namespace declaration
            SyntaxNode current = cls;
            while (current != null &&
                   current is not NamespaceDeclarationSyntax &&
                   current is not FileScopedNamespaceDeclarationSyntax)
            {
                current = current.Parent;
            }

            if (current is NamespaceDeclarationSyntax namespaceDeclaration)
            {
                return namespaceDeclaration.Name.ToString();
            }

            if (current is FileScopedNamespaceDeclarationSyntax fileScopedNamespace)
            {
                return fileScopedNamespace.Name.ToString();
            }

            return string.Empty;
        }
    }
}