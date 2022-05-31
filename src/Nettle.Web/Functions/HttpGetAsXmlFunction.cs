namespace Nettle.Data.Functions
{
    using System.Xml;

    /// <summary>
    /// Represents function for getting a single HTTP resource as JSON
    /// </summary>
    public class HttpGetAsXmlFunction : HttpGetFunction
    {
        public HttpGetAsXmlFunction()
            : base()
        { }

        public override string Description => "Gets a single HTTP resource as an XML document.";

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
