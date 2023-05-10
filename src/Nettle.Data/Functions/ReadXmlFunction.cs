namespace Nettle.Data.Functions;

using Nettle.Functions;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

/// <summary>
/// Represents function for reading an XML file into an XmlDocument
/// </summary>
public class ReadXmlFunction : FunctionBase
{
    public ReadXmlFunction()
    {
        DefineOptionalParameter("FilePath", "The XML file path", typeof(string));
    }

    public override string Description => "Reads an XML file into an XmlDocument.";

    /// <summary>
    /// Reads the XML file into an XmlDocument
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="parameterValues">The parameter values</param>
    /// <returns>The XML document</returns>
    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var filePath = GetParameterValue<string>("FilePath", request) ?? String.Empty;

        using var reader = new StreamReader(filePath);
        var document = await XDocument.LoadAsync(reader, LoadOptions.None, cancellationToken);

        return document;
    }
}
