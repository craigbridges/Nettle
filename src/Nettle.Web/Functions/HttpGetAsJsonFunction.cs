namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents function for getting a single HTTP resource as JSON
    /// </summary>
    public class HttpGetAsJsonFunction : HttpGetFunction
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public HttpGetAsJsonFunction()
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Gets a single HTTP resource as a JSON object.";
            }
        }

        /// <summary>
        /// Gets the response of a HTTP GET as JSON
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The truncated text</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var content = (string)base.GenerateOutput
            (
                context,
                parameterValues
            );

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
