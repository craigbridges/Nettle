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
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ReadXmlFunction()
        {
            DefineOptionalParameter
            (
                "FilePath",
                "The XML file path",
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
                return "Reads an XML file into an XmlDocument.";
            }
        }

        /// <summary>
        /// Reads the XML file into an XmlDocument
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The XML document</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var filePath = GetParameterValue<string>
            (
                "FilePath",
                parameterValues
            );

            var document = new XmlDocument();

            document.Load(filePath);

            return document;
        }
    }
}
