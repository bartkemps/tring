namespace ClassLibrary1;

using System.Reflection;
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
        var path = Path.Combine("../../../..", OutputFilePath);
        await using var stream = File.Open(path, FileMode.Create, FileAccess.Write);
        await using var writer = new StreamWriter(stream);
        await generator.GenerateDocumentation(writer, "template.md");
    }
}