namespace Nettle.Data.Functions;

using System.Threading.Tasks;
using System.Xml.Linq;

/// <summary>
/// Represents function for getting a single HTTP resource as JSON
/// </summary>
public class HttpGetAsXmlFunction : HttpGetFunction
{
    public HttpGetAsXmlFunction()
        : base()
    { }

    public override string Description => "Gets a single HTTP resource as an XML document.";

    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var content = (string)(await base.GenerateOutput(request, cancellationToken) ?? String.Empty);
        var document = XDocument.Parse(content);

        return document;
    }
}
