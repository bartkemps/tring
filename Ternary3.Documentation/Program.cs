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
        await generator.GenerateDocumentation(writer);
    }
}

public class DocumentationGenerator(XDocument xmlDocument, Assembly assembly)
{
    /// <summary>
    /// Concats all filesToInclude Markdown files into a single Markdown file,
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

        var assemblyDocumentation = GenerateMarkdownDescribingAssembly();
        await file.WriteAsync(assemblyDocumentation);
    }

    /// <summary>
    /// Generates a markdown string containing assembly documentation.
    /// For all public types (classes, interfaces, structs, and enums) in the assembly.
    /// </summary>
    private string GenerateMarkdownDescribingAssembly()
    {
        var markdown = "## Reference\n\n";
        var types = assembly.GetExportedTypes();
        foreach (var type in types.GroupBy(t => t.Namespace))
        {
            markdown += $"\n### <code>**{type.Key}**</code> namespace\n";
            foreach (var t in type)
            {
                markdown += $"- <code>{GetFullTypeLink(t)}</code>\n";
            }
        }

        return markdown + string.Concat(types.Select(t => GenerateMarkdownDescribingType(t.GetTypeInfo())));
    }
    


    /// <summary>
    /// Converts a class and the corresponding XML element to a markdown string.
    /// Outputs a class description and all its public and protected members.
    /// Searches for the corresponding XML element when calling other methods
    /// </summary>
    private string GenerateMarkdownDescribingType(Type type)
    {
        var info = type.GetTypeInfo();
        var fullName = GetFullTypeName(info);
        var element = FindElement("T", info);
        var comment = CommentsAsMarkdown(element);
        var markdown = $"## {fullName}\n\n{comment}\n\n";
        markdown += GenerateMarkdowDescribingConstructors(true, info.DeclaredConstructors);
        markdown += GenerateMarkdowDescribingMethods(true, info.DeclaredMethods);
        markdown += GenerateMarkdowDescribingProperties(true, info.DeclaredProperties);
        markdown += GenerateMarkdowDescribingFields(true, info.DeclaredFields);
        markdown += GenerateMarkdowDescribingConstructors(false, info.DeclaredConstructors);
        markdown += GenerateMarkdowDescribingMethods(false, info.DeclaredMethods);
        markdown += GenerateMarkdowDescribingProperties(false, info.DeclaredProperties);
        markdown += GenerateMarkdowDescribingFields(false, info.DeclaredFields);
        markdown += GenerateMarkdowDescribingOperators(info.DeclaredMembers);
        return markdown;
    }


    
    private string GenerateMarkdowDescribingConstructors(bool isStatic, IEnumerable<ConstructorInfo> c)
    {
        var constructors = c.Where(c => isStatic == c.IsStatic && (c.IsPublic || c.IsFamily)).ToList();
        if (constructors.Count == 0) return "";
        var markdown = isStatic ? "### Static Constructors\n\n" : "### Constructors\n\n";
        foreach (var constructor in constructors)
        {
            var element = FindElement("M", constructor);
            var comments = CommentsAsMarkdown(element);
            var name = $"**{constructor.DeclaringType?.Name}**{FormatParameters(constructor)}";
            markdown += $"#### <code>{name}</code>\n\n{comments}\n\n";
        }

        return markdown + "\n";
    }
    
    private string GenerateMarkdowDescribingMethods(bool isStatic, IEnumerable<MethodInfo> methods)
    {
        var filteredMethods = methods
            .Where(m => isStatic == m.IsStatic && (m.IsPublic || m.IsFamily) && !m.IsSpecialName && !m.IsConstructor)
            .ToList();
        if (filteredMethods.Count == 0) return "";
        var markdown = isStatic ? "### Static Methods\n\n" : "### Methods\n\n";
        foreach (var method in filteredMethods)
        {
            var element = FindElement("M", method);
            var comments = CommentsAsMarkdown(element);
            var name = $"{(method.IsGenericMethod ? $"**{method.Name}**<" + string.Join(", ", method.GetGenericArguments().Select(GetFullTypeLink)) + ">" : $"**{method.Name}**")}{FormatParameters(method)}";
            markdown += $"#### <code>{GetFullTypeLink(method.ReturnType)} {name}</code>\n\n{comments}\n\n";
        }
        return markdown + "\n";
    }

    private string FormatParameters(MethodBase constructor)
    {
        var sb = new StringBuilder();
        if (constructor.IsGenericMethod)
        {
            sb.Append("<");
            sb.Append(string.Join(",", constructor.GetGenericArguments().Select(GetFullTypeLink)));
            sb.Append(">");
        }
        sb.Append("(");
        sb.Append(string.Join(", ", constructor.GetParameters().Select(p => $"{GetFullTypeLink(p.ParameterType)} {p.Name}")));
        sb.Append(")");
        return sb.ToString();
    }
    
    private string GenerateMarkdowDescribingProperties(bool isStatic, IEnumerable<PropertyInfo> p)
    {
        var properties = p.Where(p => isStatic == p.GetAccessors(true)[0].IsStatic && (p.GetMethod?.IsPublic == true || p.GetMethod?.IsFamily == true || p.SetMethod?.IsPublic == true || p.SetMethod?.IsFamily == true)).ToList();
        if (properties.Count == 0) return "";
        var markdown = isStatic ? "### Static Properties\n\n" : "### Properties\n\n";
        foreach (var property in properties)
        {
            var element = FindElement("P", property);
            var comments = CommentsAsMarkdown(element);
            markdown += $"#### <code>{GetFullTypeLink(property.PropertyType)} **{property.Name}**</code>\n\n{comments}\n\n";
        }
    
        return markdown + "\n";
    }

    private string GenerateMarkdowDescribingFields(bool isStatic, IEnumerable<FieldInfo> f)
    {
        var fields = f.Where(f => isStatic == f.IsStatic && (f.IsPublic || f.IsFamily)).ToList();
        if (fields.Count == 0) return "";
        var markdown = isStatic ? "### Static Fields\n\n" : "### Fields\n\n";
        foreach (var field in fields)
        {
            var element = FindElement("F", field);
            var comments = CommentsAsMarkdown(element);
            markdown += $"#### <code>{GetFullTypeLink(field.FieldType)} **{field.Name}**</code>\n\n{comments}\n\n";
        }

        return markdown + "\n";
    }
    
    private string GenerateMarkdowDescribingOperators(IEnumerable<MemberInfo> infoDeclaredMembers)
    {
        return "\n\n**GenerateMarkdowDescribingOperators**\n\n";
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
    private string CommentsAsMarkdown(XElement? xml)
    {
        if (xml == null)
        {
            return "No documentation available.";
        }

        var sb = new StringBuilder();

        // Add summary
        var summary = XmlElementTextToMarkdown(xml.Element("summary"));
        if (!string.IsNullOrWhiteSpace(summary))
        {
            sb.AppendLine(summary);
            // if summary contains multiple lines, we will treat it as multiline
        }

        // Add parameters
        var parameters = xml.Elements("param").ToArray();
        if (parameters.Any())
        {
            sb.AppendLine("\n**Parameters:**");
            foreach (var param in parameters)
            {
                var paramName = param.Attribute("name")?.Value;
                var paramDescription = XmlElementTextToMarkdown(param);
                sb.AppendLine($"- `{paramName}`: {paramDescription}");
            }

            sb.AppendLine();
        }

        // Add returns
        var returns = xml.Element("returns");
        if (returns != null)
        {
            sb.AppendLine("\n**Returns:**");
            sb.AppendLine(XmlElementTextToMarkdown(returns));
            sb.AppendLine();
        }

        // Add exceptions
        var exceptions = xml.Elements("exception");
        if (exceptions.Any())
        {
            sb.AppendLine("\n**Exceptions:**");
            foreach (var exception in exceptions)
            {
                var exceptionType = exception.Attribute("cref")?.Value;
                var exceptionDescription = XmlElementTextToMarkdown(exception);
                sb.AppendLine($"- `{exceptionType}`: {exceptionDescription}");
            }

            sb.AppendLine();
        }

        // Add remarks
        var remarks = xml.Element("remarks");
        if (remarks != null)
        {
            sb.AppendLine("\n**Remarks:**");
            sb.AppendLine(XmlElementTextToMarkdown(remarks));
            sb.AppendLine();
        }

        // Add examples
        var examples = xml.Element("example");
        if (examples != null)
        {
            sb.AppendLine("\n**Examples:**");
            sb.AppendLine(XmlElementTextToMarkdown(examples));
            sb.AppendLine();
        }

        return ">" + sb.ToString().Trim().Replace("\n", "\n>");
    }


    /// <summary>
    /// Handles the conversion of a text to Markdown format:
    /// replaces see and cref with correct links.
    /// replaces code tags with backticks.
    /// </summary>
    private string XmlElementTextToMarkdown(XElement? element)
    {
        if (element == null) return "";
        return string.Join(" ", element.Nodes().Select(node =>
        {
            return node.NodeType switch
            {
                XmlNodeType.Text => node.ToString().Trim(),
                XmlNodeType.Element => NotTextElelementToMarkdown((XElement)node),
                _ => string.Empty
            };
        }));

        string NotTextElelementToMarkdown(XElement elem)
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
                    return $"{XmlElementTextToMarkdown(elem)}\n";
                default:
                    return XmlElementTextToMarkdown(elem);
            }
        }
    }


    /// <summary>
    /// Finds the corresponding XML element for a given member in the XML document.
    /// If the member uses inheritdoc, it will search for the base type's or interfaces' element.
    /// </summary>
    private XElement? FindElement(string memberType, MemberInfo member)
    {
        if (member.Name == "Parse")
        {
            
        }
        var declaringType = member.DeclaringType?.FullName?.Replace("+", ".") ?? (member as Type)?.Namespace;
        var name = $"{memberType}:{declaringType}.{member.Name.Replace('.', '#')}";
        if (member is MethodBase b)
        {
            name += "(" + string.Join(",", b.GetParameters().Select(t => t.ParameterType.FullName)) + ")";
        }
        var element = xmlDocument.Descendants("member").FirstOrDefault(e => e.Attribute("name")?.Value == name);
        return element;
    }
    
    private string GetFullTypeLink(Type type)
    {
        var typeName = GetFullTypeName(type);
        return ToMarkdownLink(typeName, typeName);
    }
    
    private string GetFullTypeName(Type type)
    {
        if (type.FullName == null)
        {
            return type.Name;
        }
        var sb = new StringBuilder();
        sb.Append(type.FullName!.Replace('+', '.').Split('`')[0]);
        if (type.IsGenericType)
        {
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments().Select(GetFullTypeName)));
            sb.Append(">");
        }
        return sb.ToString();
    }
    
    private string ToMarkdownLink(string name, string target)
    {
        return name.StartsWith("Ternary3.") ? $"[{name}](#{Regex.Replace(target.ToLower().Replace(' ', '-'), @"[^\w-_]", "")})" : name;
    }
}