namespace Nettle.Data.Functions
{
    using System.Xml;

    /// <summary>
    /// Represents function for posting a single HTTP resource for XML
    /// </summary>
    public class HttpPostForXmlFunction : HttpPostFunction
    {
        public HttpPostForXmlFunction()
            : base()
        { }

        public override string Description => "Posts to a HTTP resource for an XML document.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var content = (string)(base.GenerateOutput(context, parameterValues) ?? String.Empty);
            var document = new XmlDocument();

            document.LoadXml(content);

            return document;
        }
    }
}
