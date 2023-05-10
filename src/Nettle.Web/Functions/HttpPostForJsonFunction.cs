namespace Nettle.Data.Functions;

using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

/// <summary>
/// Represents function for posting a single HTTP resource for JSON
/// </summary>
public class HttpPostForJsonFunction : HttpPostFunction
{
    public HttpPostForJsonFunction()
        : base()
    { }

    public override string Description => "Posts to a HTTP resource for a JSON object.";

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
