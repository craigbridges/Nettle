namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Functions;
    using Newtonsoft.Json.Linq;
    using System.IO;

    /// <summary>
    /// Represents function for reading an XML file into a dynamic object
    /// </summary>
    public class ReadJsonFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ReadJsonFunction()
        {
            DefineRequiredParameter
            (
                "FilePath",
                "The JSON file path",
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
                return "Reads a JSON file into a dynamic object.";
            }
        }

        /// <summary>
        /// Reads the JSON file into a dynamic object
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The object generated</returns>
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

            var fileContents = File.ReadAllText
            (
                filePath
            );

            var json = JObject.Parse(fileContents);

            return json;
        }
    }
}
