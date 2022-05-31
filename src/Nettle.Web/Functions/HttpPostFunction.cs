namespace Nettle.Data.Functions
{
    using Nito.AsyncEx.Synchronous;
    using System.Net.Http;

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

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var url = GetParameterValue<string>("URL", parameterValues);
            var body = GetParameterValue<string>("Body", parameterValues);

            if (body == null)
            {
                body = String.Empty;
            }

            var headerValues = ExtractKeyValuePairs<string, object>(parameterValues, 2);

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

                // TODO: make the function async
                return client.PostAsync(url, content).WaitAndUnwrapException().Content.ReadAsStringAsync().WaitAndUnwrapException();
            }
        }
    }
}
