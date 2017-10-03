namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Functions;
    using System.IO;

    /// <summary>
    /// Represents function for reading a text file into a string
    /// </summary>
    public class ReadTextFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ReadTextFunction()
        {
            DefineRequiredParameter
            (
                "FilePath",
                "The text file path",
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
                return "Reads a text file into a string.";
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

            var content = File.ReadAllText
            (
                filePath
            );

            return content;
        }
    }
}
