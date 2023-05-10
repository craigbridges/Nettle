namespace Nettle.Data.Functions;

using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

/// <summary>
/// Represents function for getting a single HTTP resource as JSON
/// </summary>
public class HttpGetAsJsonFunction : HttpGetFunction
{
    public HttpGetAsJsonFunction()
        : base()
    { }

    public override string Description => "Gets a single HTTP resource as a JSON object.";

    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var content = (string)(await base.GenerateOutput(request, cancellationToken) ?? String.Empty);

        if (content.StartsWith("[") && content.EndsWith("]"))
        {
            return JArray.Parse(content);
        }
        else
        {
            return JObject.Parse(content);
        }
    }
}
