namespace ClassLibrary1;

using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

public class Program
{
    const string XmlFilePath = "Ternary3.xml"; // relative to executing assembly = entry assembly
    const string OutputFilePath = "README.md"; // relative to solution root

    public static async Task Main(string[] args)
    {
        var generator = new DocumentationGenerator(
            XDocument.Load(XmlFilePath),
            Assembly.Load("Ternary3")
        );
        await using var stream = File.Open(OutputFilePath, FileMode.Create, FileAccess.Write);
        await using var writer = new StreamWriter(stream);
        await generator.GenerateDocumentation(writer, "template.md");
    }
}

public class DocumentationGenerator(XDocument xmlDocument, Assembly assembly)
{
    /// <summary>
    /// Concats all filesToInclude markdown files into a single markdown file,
    /// then concats the documnentation
    /// </summary>
    public async Task GenerateDocumentation(StreamWriter file, params string[] filesToInclude)
    {
        foreach (var f in filesToInclude)
        {
            var path = Path.Combine("../../..", f);
            if (!File.Exists(path)) throw new FileNotFoundException($"The file '{file}' does not exist.");
            var md = await File.ReadAllTextAsync(path);
            await file.WriteAsync(md);
        }

        var assemblyDocumentation = OutputAssembly();
        await file.WriteAsync(assemblyDocumentation);
    }

    /// <summary>
    /// Generates a markdown string containing assembly documentation.
    /// For all public types (classes, interfaces, structs, and enums) in the assembly.
    /// </summary>
    private string OutputAssembly()
    {
        var markdown = "## Assembly Documentation\n\n";
        var types = assembly.GetExportedTypes();
        foreach (var type in types.GroupBy(t => t.Namespace))
        {
            markdown += $"\n### Namespace: {type.Key}\n";
            foreach (var t in type)
            {
                markdown += $"- {GetFullTypeLink(t)}\n";
            }
        }

        return markdown + string.Concat(types.Select(t => OutputType(t.GetTypeInfo())));
    }
    


    /// <summary>
    /// Converts a class and the corresponding XML element to a markdown string.
    /// Outputs a class description and all its public and protected members.
    /// Searches for the corresponding XML element when calling other methods
    /// </summary>
    private string OutputType(Type type)
    {
        var info = type.GetTypeInfo();
        var fullName = GetFullTypeName(info);
        var element = FindElement(info);
        var comment = CommentsAsMarkdown(element);
        var markdown = $"### `{fullName}`\n\n{comment.md}\n\n";
        markdown += OutputConstructors(true, info.DeclaredConstructors);
        markdown += OutputMethods(true, info.DeclaredMethods);
        markdown += OutputProperties(true, info.DeclaredProperties);
        markdown += OutputFields(true, info.DeclaredFields);
        markdown += OutputConstructors(false, info.DeclaredConstructors);
        markdown += OutputMethods(false, info.DeclaredMethods);
        markdown += OutputProperties(false, info.DeclaredProperties);
        markdown += OutputFields(false, info.DeclaredFields);
        markdown += OutputOperators(info.DeclaredMembers);
        return markdown;
    }


    


    private string OutputOperators(IEnumerable<MemberInfo> infoDeclaredMembers)
    {
        return "\n\n**OutputOperators**\n\n";
    }

    private string OutputConstructors(bool isStatic, IEnumerable<ConstructorInfo> infoDeclaredConstructors)
    {
        return "\n\n**OutputConstructors**\n\n";
    }
    

    


    private string OutputMethods(bool isStatic, IEnumerable<MethodInfo> infoDeclaredMethods)
    {
        return "\n\n**OutputMethods**\n\n";
    }

    private string OutputProperties(bool isStatic, IEnumerable<PropertyInfo> infoDeclaredProperties)
    {
        return "\n\n**OutputProperties**\n\n";
    }

    private string OutputFields(bool isStatic, IEnumerable<FieldInfo> f)
    {
        var fields = f.Where(f => isStatic == f.IsStatic && (f.IsPublic || f.IsFamily)).ToList();
        if (fields.Count == 0) return "";
        var markdown = isStatic ? "#### Static Fields\n\n" : "#### Fields\n\n";
        foreach (var field in fields)
        {
            var element = FindElement(field);
            var comments = CommentsAsMarkdown(element);
            markdown += comments.multiline
                ? $"- **{field.Name}**\n\n{comments.md}\n\n"
                : $"- **{field.Name}** - {comments.md}\n\n";
        }

        return markdown + "\n";
    }

    /// <summary>
    /// Creates a nice markdown block with:
    /// - a summary
    /// - (if any parameters are present) a list of parameters with their descriptions
    /// - (if any exceptions are present) a list of exceptions with their descriptions
    /// - (if any returns are present) a description of the return value
    /// - (if any remarks are present) a remarks section
    /// - (if any examples are present) an examples section
    /// </summary>
    private (string md, bool multiline) CommentsAsMarkdown(XElement member)
    {
        if (member == null)
        {
            return ("No documentation available.", false);
        }

        var markdownBuilder = new StringBuilder();
        var multiline = false;

        // Add summary
        var summary = ToMarkdown(member.Element("summary"));
        if (!string.IsNullOrWhiteSpace(summary))
        {
            markdownBuilder.AppendLine(summary);
            // if summary contains multiple lines, we will treat it as multiline
            multiline = summary.Contains('\n');
        }

        // Add parameters
        var parameters = member.Elements("param");
        if (parameters.Any())
        {
            markdownBuilder.AppendLine("**Parameters:**");
            foreach (var param in parameters)
            {
                var paramName = param.Attribute("name")?.Value;
                var paramDescription = ToMarkdown(param);
                markdownBuilder.AppendLine($"- `{paramName}`: {paramDescription}");
            }

            markdownBuilder.AppendLine();
            multiline = true;
        }

        // Add returns
        var returns = member.Element("returns");
        if (returns != null)
        {
            markdownBuilder.AppendLine("**Returns:**");
            markdownBuilder.AppendLine(ToMarkdown(returns));
            markdownBuilder.AppendLine();
            multiline = true;
        }

        // Add exceptions
        var exceptions = member.Elements("exception");
        if (exceptions.Any())
        {
            markdownBuilder.AppendLine("**Exceptions:**");
            foreach (var exception in exceptions)
            {
                var exceptionType = exception.Attribute("cref")?.Value;
                var exceptionDescription = ToMarkdown(exception);
                markdownBuilder.AppendLine($"- `{exceptionType}`: {exceptionDescription}");
            }

            markdownBuilder.AppendLine();
            multiline = true;
        }

        // Add remarks
        var remarks = member.Element("remarks");
        if (remarks != null)
        {
            markdownBuilder.AppendLine("**Remarks:**");
            markdownBuilder.AppendLine(ToMarkdown(remarks));
            markdownBuilder.AppendLine();
            multiline = true;
        }

        // Add examples
        var examples = member.Element("example");
        if (examples != null)
        {
            markdownBuilder.AppendLine("**Examples:**");
            markdownBuilder.AppendLine(ToMarkdown(examples));
            markdownBuilder.AppendLine();
            multiline = true;
        }

        var markdown = markdownBuilder.ToString().Trim();
        return (string.IsNullOrWhiteSpace(markdown) ? "No documentation available." : markdown, multiline);
    }


    /// <summary>
    /// Handles the conversion of a text to Markdown format:
    /// replaces see and cref with correct links.
    /// replaces code tags with backticks.
    /// </summary>
    private string ToMarkdown(XElement element)
    {
        if (element == null)
            return string.Empty;

        return string.Join(" ", element.Nodes().Select(node =>
        {
            return node.NodeType switch
            {
                XmlNodeType.Text => node.ToString().Trim(),
                XmlNodeType.Element => ProcessElement((XElement)node),
                _ => string.Empty
            };
        }));

        string ProcessElement(XElement elem)
        {
            switch (elem.Name.LocalName)
            {
                case "see":
                    var crefValue = elem.Attribute("cref")?.Value;
                    if (crefValue == null) return $"[{elem.Value}]";
                    var isSystemType = crefValue.StartsWith("T:System.");
                    var displayName = crefValue.Split('.').Last();
                    var linkTarget = crefValue.Replace("T:", "").ToLower();
                    return isSystemType ? $"`{displayName}`" : $"[{displayName}](#{linkTarget})";
                case "code":
                    return $"\n```\n{elem.Value}\n```\n";
                case "para":
                    return $"{ToMarkdown(elem)}\n";
                default:
                    return ToMarkdown(elem);
            }
        }
    }


    /// <summary>
    /// Finds the corresponding XML element for a given member in the XML document.
    /// If the member uses inheritdoc, it will search for the base type's or interfaces' element.
    /// </summary>
    public XElement FindElement(MemberInfo member)
    {
        var memberType = member switch
        {
            Type _ => "T:",
            MethodInfo _ => "M:",
            PropertyInfo _ => "P:",
            FieldInfo _ => "F:",
            EventInfo _ => "E:",
            _ => throw new ArgumentException("Unsupported member type", nameof(member))
        };

        var declaringType = member.DeclaringType?.FullName?.Replace("+", ".") ?? (member as Type)?.Namespace;
        var element = xmlDocument.Descendants("member").FirstOrDefault(e => e.Attribute("name")?.Value == $"{memberType}{declaringType}.{member.Name}");

        while (element != null && element.Element("inheritdoc") != null)
        {
            var baseMember = member.DeclaringType?.BaseType?.GetMember(member.Name).FirstOrDefault();
            member = baseMember;

            if (member == null)
            {
                // Search interfaces if base type has no documentation for the member
                foreach (var iface in member.DeclaringType?.GetInterfaces() ?? Array.Empty<Type>())
                {
                    baseMember = iface.GetMember(member.Name).FirstOrDefault();
                    if (baseMember != null)
                    {
                        declaringType = baseMember.DeclaringType?.FullName?.Replace("+", ".");
                        element = xmlDocument.Descendants("member")
                            .FirstOrDefault(e => e.Attribute("name")?.Value == $"M:{declaringType}.{baseMember.Name}");
                        if (element != null) break;
                    }
                }

                break;
            }

            declaringType = member?.DeclaringType?.FullName?.Replace("+", ".");
            element = xmlDocument.Descendants("member").FirstOrDefault(e => e.Attribute("name")?.Value == $"{memberType}:{declaringType}.{member.Name}");
        }

        return element;
    }
    
    private string GetFullTypeLink(Type type)
    {
        var typeName = GetFullTypeName(type);
        return ToMarkdownLink(typeName, typeName);
    }
    
    private string GetFullTypeName(Type info)
    {
        var typeName = info.FullName!.Replace('+', '.').Split('`')[0]; // Remove any generic type parameters
        if (info.IsGenericType)
        {
            var genericArgs = string.Join(", ", info.GenericTypeArguments.Select(t => t.Name));
            typeName += $"<{genericArgs}>";
        }
        return typeName;
    }
    
    private string ToMarkdownLink(string name, string target)
    {
        return $"[{name}](#{Regex.Replace(target.ToLower().Replace(' ', '-'), @"[^\w-_]", "")})";
    }
}