using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ternary3.Generators;

using System.Text.RegularExpressions;

[Generator]
public class TernaryConstantsGenerator : ISourceGenerator
{
    private static readonly Regex TernaryConstant = new("^ter(_*[01T]){1,40}$", RegexOptions.Compiled);

    private class SyntaxReceiver : ISyntaxReceiver
    {
        public HashSet<string> TernaryConstants { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Look for identifier names that match our ternary constant pattern
            if (syntaxNode is not IdentifierNameSyntax identifier) return;
            var name = identifier.Identifier.Text;
            if (TernaryConstant.IsMatch(name))
            {
                TernaryConstants.Add(name);
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
        var syntaxReceiver = (SyntaxReceiver)context.SyntaxReceiver;
        if (syntaxReceiver == null || !syntaxReceiver.TernaryConstants.Any())
            return;

        // Generate a single Literals class with all constants
        GenerateLiteralsClass(context, syntaxReceiver.TernaryConstants);
    }

    private static void GenerateLiteralsClass(
        GeneratorExecutionContext context,
        IEnumerable<string> constants)
    {
        var sb = new StringBuilder();

        sb.AppendLine("global using static Ternary3.Literals;");
        sb.AppendLine();
        sb.AppendLine("namespace Ternary3;");
        sb.AppendLine();
        sb.AppendLine("using System.ComponentModel;");
        sb.AppendLine("using System.Diagnostics;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Contains generated ternary literal constants.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("[EditorBrowsable(EditorBrowsableState.Never)]");
        sb.AppendLine("internal static class Literals");
        sb.AppendLine("{");

        foreach (var name in constants.OrderBy(x => x))
        {
            var value = Conversion.ToLong(name);
            var type = value is < int.MinValue or > int.MaxValue ? "long" : "int";
            sb.AppendLine($"    public const {type} {name} = {value};");
        }

        sb.AppendLine("}");

        context.AddSource("Literals.g.cs", sb.ToString());
    }
}