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
        public ReadTextFunction()
        {
            DefineRequiredParameter("FilePath", "The text file path", typeof(string));
        }

        public override string Description => "Reads a text file into a string.";

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
            var content = File.ReadAllText(filePath ?? String.Empty);

            return content;
        }
    }
}
