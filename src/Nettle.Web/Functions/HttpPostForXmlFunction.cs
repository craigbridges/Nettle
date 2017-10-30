namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using System.Xml;

    /// <summary>
    /// Represents function for posting a single HTTP resource for XML
    /// </summary>
    public class HttpPostForXmlFunction : HttpPostFunction
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public HttpPostForXmlFunction()
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Posts to a HTTP resource for an XML document.";
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

            var document = new XmlDocument();

            document.LoadXml(content);

            return document;
        }
    }
}
