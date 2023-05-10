namespace Nettle.Data.Functions;

using System.Net.Http;
using System.Threading.Tasks;

/// <summary>
/// Represents function for getting a single HTTP resource
/// </summary>
public class HttpGetFunction : FunctionBase
{
    public HttpGetFunction()
    {
        DefineRequiredParameter("URL", "The URL to get.", typeof(string));
    }

    public override string Description => "Gets a single HTTP resource as a string.";

    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var url = GetParameterValue<string>("URL", request);
        var headerValues = ExtractKeyValuePairs<string, object>(request.ParameterValues, 1);

        using (var client = new HttpClient())
        {
            foreach (var header in headerValues)
            {
                var value = String.Empty;

                if (header.Value != null)
                {
                    value = header.Value.ToString();
                }

                client.DefaultRequestHeaders.Add(header.Key, value);
            }

            return await client.GetStringAsync(url, cancellationToken);
        }
    }
}
