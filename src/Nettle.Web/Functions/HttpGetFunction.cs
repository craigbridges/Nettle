namespace Nettle.Data.Functions
{
    using Nito.AsyncEx.Synchronous;
    using System.Net.Http;

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

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var url = GetParameterValue<string>("URL", parameterValues);
            var headerValues = ExtractKeyValuePairs<string, object>(parameterValues, 1);

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

                // TODO: make the function async
                return client.GetStringAsync(url).WaitAndUnwrapException();
            }
        }
    }
}
