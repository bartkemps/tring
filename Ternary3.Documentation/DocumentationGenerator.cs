namespace ClassLibrary1;

using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

public class DocumentationGenerator(XDocument xmlDocument, Assembly assembly)
{
    const string BaseNamespace = "Ternary3"; // the namespace of the assembly to document

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
        var markdown = "\n\n## Reference\n\n";
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
        if (type.IsEnum)
        {
            return GenerateMarkdownDescribingEnum(type);
        }

        var info = type.GetTypeInfo();
        var fullName = GetFullTypeName(info);
        var element = FindElement("T", info);
        var comment = CommentsAsMarkdown(element);
        var markdown = $"## {fullName}\n\n{comment}\n\n";
        markdown += GenerateMarkdownDescribingConstructors(true, info.DeclaredConstructors);
        markdown += GenerateMarkdownDescribingMethods(true, info.DeclaredMethods);
        markdown += GenerateMarkdowDescribingProperties(true, info.DeclaredProperties);
        markdown += GenerateMarkdowDescribingFields(true, info.DeclaredFields);
        markdown += GenerateMarkdownDescribingConstructors(false, info.DeclaredConstructors);
        markdown += GenerateMarkdownDescribingMethods(false, info.DeclaredMethods);
        markdown += GenerateMarkdowDescribingProperties(false, info.DeclaredProperties);
        markdown += GenerateMarkdowDescribingFields(false, info.DeclaredFields);
        markdown += GenerateMarkdowDescribingOperators(info.DeclaredMembers);
        return markdown;
    }

    private string GenerateMarkdownDescribingEnum(Type type)
    {
        var info = type.GetTypeInfo();
        var fullName = GetFullTypeName(info);
        var element = FindElement("T", info);
        var comment = CommentsAsMarkdown(element);

        var markdown = $"## {fullName}\n\n{comment}\n\n";
        markdown += "### Enum Values\n\n";

        var fields = info.DeclaredFields
            .Where(f => f.IsPublic && f.IsStatic)
            .ToList();

        foreach (var field in fields)
        {
            var fieldElement = FindElement("F", field);
            var fieldComment = CommentsAsMarkdown(fieldElement);
            var value = Convert.ChangeType(field.GetRawConstantValue(), Enum.GetUnderlyingType(type));
            markdown += $"- **{field.Name}** = `{value}`\n{fieldComment}\n";
        }

        return markdown + "\n";
    }

    private string GenerateMarkdownDescribingConstructors(bool isStatic, IEnumerable<ConstructorInfo> c)
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

    private string GenerateMarkdownDescribingMethods(bool isStatic, IEnumerable<MethodInfo> methods)
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

            // Format property accessors
            string accessors = "";
            if (property.CanRead && property.CanWrite)
                accessors = " { get; set; }";
            else if (property.CanRead)
                accessors = " { get; }";
            else if (property.CanWrite)
                accessors = " { set; }";

            // Handle indexers specially
            string propertyDisplay;
            if (property.Name == "Item" && property.GetIndexParameters().Length > 0)
            {
                var parameters = property.GetIndexParameters();
                var paramList = string.Join(", ", parameters.Select(p => $"{GetFullTypeLink(p.ParameterType)} {p.Name}"));
                propertyDisplay = $"**this[{paramList}]**{accessors}";
            }
            else
            {
                propertyDisplay = $"**{property.Name}**{accessors}";
            }

            markdown += $"#### <code>{GetFullTypeLink(property.PropertyType)} {propertyDisplay}</code>\n\n{comments}\n\n";
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

    private string GenerateMarkdowDescribingOperators(IEnumerable<MemberInfo> members)
    {
        var operatorMethods = members
            .OfType<MethodInfo>()
            .Where(m => m.IsSpecialName && m.Name.StartsWith("op_") && m.IsPublic)
            .ToList();

        if (operatorMethods.Count == 0) return "";

        var markdown = "### Operators\n\n";

        foreach (var method in operatorMethods)
        {
            var element = FindElement("M", method);
            var comments = CommentsAsMarkdown(element);

            // Get a friendly operator name
            string operatorSymbol = GetOperatorSymbol(method.Name);
            string operatorName = operatorSymbol != null ? $"operator {operatorSymbol}" : method.Name;

            // Format parameters
            string parameters = FormatParameters(method);

            markdown += $"#### <code>{GetFullTypeLink(method.ReturnType)} **{operatorName}**{parameters}</code>\n\n{comments}\n\n";
        }

        return markdown + "\n";
    }

    private string GetOperatorSymbol(string methodName)
    {
        return methodName switch
        {
            "op_Addition" => "+",
            "op_Subtraction" => "-",
            "op_Multiply" => "&ast;",
            "op_Division" => "/",
            "op_Modulus" => "%",
            "op_BitwiseAnd" => "&amp;",
            "op_BitwiseOr" => "|",
            "op_ExclusiveOr" => "^",
            "op_LeftShift" => "&lt;&lt;",
            "op_RightShift" => "&gt;&gt;",
            "op_UnsignedRightShift" => "&gt;&gt;&gt;",
            "op_Equality" => "==",
            "op_Inequality" => "!=",
            "op_LessThan" => "&lt;",
            "op_LessThanOrEqual" => "&lt;=",
            "op_GreaterThan" => "&gt;",
            "op_GreaterThanOrEqual" => "&gt;=",
            "op_UnaryNegation" => "-",
            "op_UnaryPlus" => "+",
            "op_OnesComplement" => "~",
            "op_Increment" => "++",
            "op_Decrement" => "--",
            "op_Implicit" => "implicit",
            "op_Explicit" => "explicit",
            "op_True" => "true",
            "op_False" => "false",
            "op_LogicalNot" => "!",
            _ => throw new NotSupportedException($"Operator '{methodName}' is not supported.")
        };
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
            sb.AppendLine(UnIndent(summary));
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
                var crefAttribute = exception.Attribute("cref")?.Value;
                string exceptionTypeName = crefAttribute ?? "Exception";

                // Convert from T:System.ArgumentException format to just the type name
                if (exceptionTypeName.Contains(':'))
                {
                    exceptionTypeName = exceptionTypeName.Split(':')[1];
                }

                var exceptionDescription = XmlElementTextToMarkdown(exception);
                sb.AppendLine($"- `{exceptionTypeName}`: {exceptionDescription}");
            }

            sb.AppendLine();
        }

        // Add remarks
        var remarks = xml.Element("remarks");
        if (remarks != null)
        {
            sb.AppendLine("\n**Remarks:**\n");
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
                    var langwordValue = elem.Attribute("langword")?.Value;

                    if (langwordValue != null)
                    {
                        // Handle language keywords like true, false, null
                        return $"`{langwordValue}`";
                    }
                    else if (crefValue == null)
                    {
                        // Handle <see> tags without cref but with text content
                        var elemValue = elem.Value.Trim();
                        if (!string.IsNullOrEmpty(elemValue))
                            return elemValue;
                        else
                            return "[reference]";
                    }
                    else
                    {
                        // Handle cref references to types
                        var isSystemType = crefValue.StartsWith("T:System.");
                        var displayName = crefValue.Split('.').Last();
                        var linkTarget = crefValue.Replace("T:", "").ToLower();
                        return isSystemType ? $"`{displayName}`" : $"[{displayName}](#{linkTarget})";
                    }
                case "paramref":
                    // Handle parameter references
                    var nameValue = elem.Attribute("name")?.Value;
                    return nameValue != null ? $"`{nameValue}`" : "[parameter]";
                case "code":
                    return $"\n```csharp\n{UnIndentCode(elem.Value)}\n```\n";
                case "para":
                    return $"{XmlElementTextToMarkdown(elem)}\n";
                default:
                    return XmlElementTextToMarkdown(elem);
            }
        }
    }

    
    
    private string UnIndentCode(string text)
    {
        var lines = text.Trim('\n').TrimEnd('\n', ' ').Split('\n');
        var minIndent = lines.Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.TakeWhile(char.IsWhiteSpace).Count())
            .DefaultIfEmpty(0)
            .Min();
        return string.Join("\n", lines.Select(line => line.Substring(minIndent).TrimEnd()));
    }
    
    private string UnIndent(string text)
    {
        var lines = text.Trim('\n').TrimEnd('\n', ' ','\t').Split('\n');
        return string.Join("\n", lines.Select(line => line.Trim(' ', '\t')));
    }


    /// <summary>
    /// Finds the corresponding XML element for a given member in the XML document.
    /// If the member uses inheritdoc, it will search for the base type's or interfaces' element.
    /// </summary>
    private XElement? FindElement(string memberType, MemberInfo member)
    {
        // Generate the XML documentation key for this member
        var declaringType = member.DeclaringType?.FullName?.Replace("+", ".") ?? (member as Type)?.Namespace;

        // Handle constructors differently - they have special naming in XML (#ctor)
        string name;
        if (member is ConstructorInfo)
        {
            name = $"{memberType}:{declaringType}.#ctor";

            // Add parameter information for constructors with parameters
            if (member is ConstructorInfo constructorInfo && constructorInfo.GetParameters().Length > 0)
            {
                name += "(" + string.Join(",", constructorInfo.GetParameters().Select(p => p.ParameterType.FullName ?? p.ParameterType.Name)) + ")";
            }
        }
        else
        {
            name = $"{memberType}:{declaringType}.{member.Name.Replace('.', '#')}";

            // Add parameter information for methods
            if (member is MethodBase methodBase && !(member is ConstructorInfo))
            {
                var parameters = methodBase.GetParameters();
                if (parameters.Length > 0)
                {
                    name += "(" + string.Join(",", parameters.Select(p => p.ParameterType.FullName ?? p.ParameterType.Name)) + ")";
                }
                else
                {
                    name += "()";
                }
            }
        }

        // Find the element in the XML document
        var element = xmlDocument.Descendants("member").FirstOrDefault(e => e.Attribute("name")?.Value == name);

        // If we didn't find a match and this is a method or constructor with parameters,
        // try without parameters as some XML doc generators handle this differently
        if (element == null && member is MethodBase)
        {
            string baseNameWithoutParams = member is ConstructorInfo
                ? $"{memberType}:{declaringType}.#ctor"
                : $"{memberType}:{declaringType}.{member.Name.Replace('.', '#')}";

            element = xmlDocument.Descendants("member")
                .FirstOrDefault(e => e.Attribute("name")?.Value?.StartsWith(baseNameWithoutParams) == true);
        }

        // Handle inheritdoc tag
        if (element?.Element("inheritdoc") != null)
        {
            // First, try to find documentation in the base type
            if (member.DeclaringType?.BaseType != null && member.DeclaringType.BaseType.FullName?.StartsWith(BaseNamespace) == true)
            {
                var baseType = member.DeclaringType.BaseType;
                MemberInfo? baseMember = null;

                // Find the equivalent member in the base type
                if (member is PropertyInfo property)
                {
                    var parameters = property.GetIndexParameters();
                    if (parameters.Length > 0)
                    {
                        // Handle indexers with parameters
                        baseMember = baseType.GetProperties()
                            .FirstOrDefault(p => p.Name == property.Name &&
                                                 p.GetIndexParameters().Length == parameters.Length &&
                                                 p.PropertyType == property.PropertyType);
                    }
                    else
                    {
                        // Handle regular properties
                        baseMember = baseType.GetProperty(property.Name);
                    }
                }
                else if (member is MethodInfo method)
                {
                    // Handle methods
                    baseMember = baseType.GetMethod(method.Name,
                        method.GetParameters().Select(p => p.ParameterType).ToArray());
                }
                else if (member is ConstructorInfo)
                {
                    // Handle constructors
                    var constructorParams = ((ConstructorInfo)member).GetParameters();
                    baseMember = baseType.GetConstructor(constructorParams.Select(p => p.ParameterType).ToArray());
                }

                // If we found the member in the base type, try to get its documentation
                if (baseMember != null)
                {
                    var baseElement = FindElement(memberType, baseMember);
                    if (baseElement != null)
                    {
                        return baseElement;
                    }
                }
            }

            // If base type doesn't have documentation, search interfaces
            if (member.DeclaringType != null)
            {
                foreach (var interfaceType in member.DeclaringType.GetInterfaces().Where(i => i.FullName?.StartsWith(BaseNamespace) == true))
                {
                    MemberInfo? interfaceMember = null;

                    // Find the equivalent member in the interface
                    if (member is PropertyInfo property)
                    {
                        var parameters = property.GetIndexParameters();
                        if (parameters.Length > 0)
                        {
                            // Handle indexers with parameters
                            interfaceMember = interfaceType.GetProperties()
                                .FirstOrDefault(p => p.Name == property.Name &&
                                                     p.GetIndexParameters().Length == parameters.Length &&
                                                     p.PropertyType == property.PropertyType);
                        }
                        else
                        {
                            // Handle regular properties
                            interfaceMember = interfaceType.GetProperty(property.Name);
                        }
                    }
                    else if (member is MethodInfo method)
                    {
                        // Handle methods
                        interfaceMember = interfaceType.GetMethod(method.Name,
                            method.GetParameters().Select(p => p.ParameterType).ToArray());
                    }

                    // If we found the member in the interface, try to get its documentation
                    if (interfaceMember != null)
                    {
                        var interfaceElement = FindElement(memberType, interfaceMember);
                        if (interfaceElement != null && interfaceElement.Element("inheritdoc") == null)
                        {
                            return interfaceElement;
                        }
                    }
                }
            }
        }

        // If we couldn't find documentation through inheritance or the element doesn't have inheritdoc,
        // return whatever we found originally (might be null)
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
        if (name.StartsWith(BaseNamespace))
        {
            return $"[{name}](#{Regex.Replace(target.ToLower().Replace(' ', '-'), @"[^\w-_]", "")})";
        }

        var parts = name.ToLower().Split('<', ',');
        var link = parts.Length > 1 ? $"{parts[0]}-{parts.Length - 1}" : parts[0];
        return $"[{name}](https://learn.microsoft.com/en-us/dotnet/api/{link})";
    }
}