namespace Nettle.Data.Functions
{
    using Newtonsoft.Json.Linq;
    
    /// <summary>
    /// Represents function for posting a single HTTP resource for JSON
    /// </summary>
    public class HttpPostForJsonFunction : HttpPostFunction
    {
        public HttpPostForJsonFunction()
            : base()
        { }

        public override string Description => "Posts to a HTTP resource for a JSON object.";

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
