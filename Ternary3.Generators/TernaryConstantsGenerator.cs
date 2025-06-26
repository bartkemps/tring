using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ternary3.Generators
{
    using System.Diagnostics;

    [Generator]
    public class TernaryConstantsGenerator : ISourceGenerator
    {
        private class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> PartialClasses { get; } = new List<ClassDeclarationSyntax>();
            
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
            var hasAttribute = context.Compilation
                .Assembly
                .GetAttributes()
                .Any(attr => attr.AttributeClass?.Name == "GenerateTernaryConstantsAttribute");
            if (!hasAttribute)
                return;

            // Get the syntax receiver with the collected partial classes
            var syntaxReceiver = (SyntaxReceiver)context.SyntaxReceiver;
            if (syntaxReceiver == null)
                return;

            var regex = new System.Text.RegularExpressions.Regex("^ter[01T_]+$");
            
            foreach (var cls in syntaxReceiver.PartialClasses)
            {
                // Try to get the semantic model - this might fail if there are compilation errors
                SemanticModel semanticModel;
                INamedTypeSymbol classSymbol;
                
                try
                {
                    semanticModel = context.Compilation.GetSemanticModel(cls.SyntaxTree);
                    classSymbol = semanticModel.GetDeclaredSymbol(cls) as INamedTypeSymbol;
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
                        .Where(name => regex.IsMatch(name))
                        .Distinct();
                    
                    if (!identifiers.Any()) continue;
                    
                    // Generate constants based on syntax analysis only
                    GenerateConstants(context, className, namespaceName, identifiers);
                    continue;
                }

                // If we got here, we have a valid semantic model
                // Find all variable references matching the regex
                var identifiersFromSemantics = cls.DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Select(id => id.Identifier.Text)
                    .Where(name => regex.IsMatch(name))
                    .Distinct();

                // Find existing members
                var existingMembers = new HashSet<string>(classSymbol.GetMembers().Select(m => m.Name));
                var missing = identifiersFromSemantics.Where(name => !existingMembers.Contains(name));

                if (!missing.Any()) continue;

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
                sb.AppendLine($"namespace {namespaceName}");
                sb.AppendLine("{");
            }
            
            sb.AppendLine($"    partial class {className}");
            sb.AppendLine("    {");
            foreach (var name in constants)
            {
                sb.AppendLine($"        public const int {name} = 0; // TODO: Set correct value");
            }
            sb.AppendLine("    }");
            
            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine("}");
            }

            context.AddSource($"{className}_TernaryConstants.g.cs", sb.ToString());
        }
        
        private static string GetNamespaceFrom(ClassDeclarationSyntax cls)
        {
            // Walk up the syntax tree to find namespace declaration
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
