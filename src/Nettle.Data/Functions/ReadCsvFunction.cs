namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Data.Common.Serialization.Csv;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents function for reading a CSV file into a data grid
    /// </summary>
    public class ReadCsvFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ReadCsvFunction()
        {
            DefineOptionalParameter
            (
                "FilePath",
                "The CSV file path",
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
                return "Reads a CSV file into a data grid.";
            }
        }

        /// <summary>
        /// Reads the CSV file into a data grid
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The data grid</returns>
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

            var serializer = new CsvToGridSerializer();
            var grid = serializer.ReadCsvFile(filePath);

            return grid;
        }
    }
}
