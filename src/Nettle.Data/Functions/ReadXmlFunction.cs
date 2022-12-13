namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Functions;
    using System.Xml;

    /// <summary>
    /// Represents function for reading an XML file into an XmlDocument
    /// </summary>
    public class ReadXmlFunction : FunctionBase
    {
        public ReadXmlFunction()
        {
            DefineOptionalParameter("FilePath", "The XML file path", typeof(string));
        }

        public override string Description => "Reads an XML file into an XmlDocument.";

        /// <summary>
        /// Reads the XML file into an XmlDocument
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The XML document</returns>
        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var filePath = GetParameterValue<string>("FilePath", parameterValues);
            var document = new XmlDocument();

            document.Load(filePath ?? String.Empty);

            return document;
        }
    }
}
