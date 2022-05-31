namespace Nettle.Data.Functions
{
    using Newtonsoft.Json.Linq;
    
    /// <summary>
    /// Represents function for getting a single HTTP resource as JSON
    /// </summary>
    public class HttpGetAsJsonFunction : HttpGetFunction
    {
        public HttpGetAsJsonFunction()
            : base()
        { }

        public override string Description => "Gets a single HTTP resource as a JSON object.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var content = (string)(base.GenerateOutput(context, parameterValues) ?? String.Empty);

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
}
