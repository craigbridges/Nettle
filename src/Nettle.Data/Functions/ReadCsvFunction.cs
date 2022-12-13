namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Data.Common.Serialization.Csv;
    using Nettle.Functions;

    /// <summary>
    /// Represents function for reading a CSV file into a data grid
    /// </summary>
    public class ReadCsvFunction : FunctionBase
    {
        public ReadCsvFunction()
        {
            DefineRequiredParameter("FilePath", "The CSV file path", typeof(string));
        }

        public override string Description => "Reads a CSV file into a data grid.";

        /// <summary>
        /// Reads the CSV file into a data grid
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The data grid</returns>
        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var filePath = GetParameterValue<string>("FilePath", parameterValues);
            var grid = CsvToGridSerializer.ReadCsvFile(filePath ?? String.Empty);

            return grid;
        }
    }
}
