namespace Nettle.Data.Functions;

using System.Net.Http;
using System.Threading.Tasks;

/// <summary>
/// Represents function for posting to a single HTTP resource
/// </summary>
public class HttpPostFunction : FunctionBase
{
    public HttpPostFunction()
    {
        DefineRequiredParameter("URL", "The URL to post to.", typeof(string));
        DefineOptionalParameter("Body", "The body content.", typeof(string));
    }

    public override string Description => "Posts to a single HTTP resource.";

    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var url = GetParameterValue<string>("URL", request);
        var body = GetParameterValue<string>("Body", request);

        body ??= String.Empty;

        var headerValues = ExtractKeyValuePairs<string, object>(request.ParameterValues, 2);

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

            var content = new StringContent(body);

            var message = await client.PostAsync(url, content, cancellationToken);
            var response = await message.Content.ReadAsStringAsync(cancellationToken);

            return response;
        }
    }
}
