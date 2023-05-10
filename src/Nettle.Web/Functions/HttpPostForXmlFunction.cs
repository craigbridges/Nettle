namespace Nettle.Data.Functions;

using System.Threading.Tasks;
using System.Xml.Linq;

/// <summary>
/// Represents function for posting a single HTTP resource for XML
/// </summary>
public class HttpPostForXmlFunction : HttpPostFunction
{
    public HttpPostForXmlFunction()
        : base()
    { }

    public override string Description => "Posts to a HTTP resource for an XML document.";

    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var content = (string)(await base.GenerateOutput(request, cancellationToken) ?? String.Empty);
        var document = XDocument.Parse(content);

        return document;
    }
}
