namespace Nettle.Data.Functions;

using Nettle.Functions;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Represents function for reading a text file into a string
/// </summary>
public class ReadTextFunction : FunctionBase
{
    public ReadTextFunction()
    {
        DefineRequiredParameter("FilePath", "The text file path", typeof(string));
    }

    public override string Description => "Reads a text file into a string.";

    /// <summary>
    /// Reads the XML file into an XmlDocument
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="parameterValues">The parameter values</param>
    /// <returns>The XML document</returns>
    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var filePath = GetParameterValue<string>("FilePath", request);
        var content = await File.ReadAllTextAsync(filePath ?? String.Empty, cancellationToken);

        return content;
    }
}
