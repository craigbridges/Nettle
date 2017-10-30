namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents function for posting a single HTTP resource for JSON
    /// </summary>
    public class HttpPostForJsonFunction : HttpPostFunction
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public HttpPostForJsonFunction()
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Posts to a HTTP resource for a JSON object.";
            }
        }

        /// <summary>
        /// Generates a JSON object from the post results
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
