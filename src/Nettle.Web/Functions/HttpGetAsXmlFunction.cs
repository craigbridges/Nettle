namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using System.Xml;

    /// <summary>
    /// Represents function for getting a single HTTP resource as JSON
    /// </summary>
    public class HttpGetAsXmlFunction : HttpGetFunction
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public HttpGetAsXmlFunction()
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Gets a single HTTP resource as an XML document.";
            }
        }

        /// <summary>
        /// Gets the response of a HTTP GET as XML
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
