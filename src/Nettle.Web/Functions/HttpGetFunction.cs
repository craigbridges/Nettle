namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Functions;
    using System.Net;

    /// <summary>
    /// Represents function for generating an array of random numbers
    /// </summary>
    public class HttpGetFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public HttpGetFunction()
        {
            DefineRequiredParameter
            (
                "URL",
                "The URL to get",
                typeof(string)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Gets a HTTP resource as a string.";
            }
        }

        /// <summary>
        /// Generates an array of random integers
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

            var url = GetParameterValue<string>
            (
                "URL",
                parameterValues
            );

            using (var client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}
