namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Functions;
    using System;
    using System.Net;

    /// <summary>
    /// Represents function for posting to a single HTTP resource
    /// </summary>
    public class HttpPostFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public HttpPostFunction()
        {
            DefineRequiredParameter
            (
                "URL",
                "The URL to post to.",
                typeof(string)
            );

            DefineOptionalParameter
            (
                "Body",
                "The body content.",
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
                return "Posts to a single HTTP resource.";
            }
        }

        /// <summary>
        /// Gets the response from a HTTP POST
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

            var body = GetParameterValue<string>
            (
                "Body",
                parameterValues
            );

            if (body == null)
            {
                body = String.Empty;
            }

            var headerValues = ExtractKeyValuePairs<string, object>
            (
                parameterValues,
                2
            );

            using (var client = new WebClient())
            {
                foreach (var header in headerValues)
                {
                    var value = String.Empty;

                    if (header.Value != null)
                    {
                        value = header.Value.ToString();
                    }

                    client.Headers.Add
                    (
                        header.Key,
                        value
                    );
                }
                
                return client.UploadString
                (
                    url,
                    body
                );
            }
        }
    }
}
