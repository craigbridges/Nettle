namespace Nettle.Data.Functions;

using Nettle.Data.Common.Serialization.Csv;
using Nettle.Functions;
using System.Threading.Tasks;

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
    /// Asynchronously reads the CSV file into a data grid
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="parameterValues">The parameter values</param>
    /// <returns>A new data grid containing the CSV data</returns>
    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var filePath = GetParameterValue<string>("FilePath", request);
        var grid = await CsvToGridSerializer.ReadCsvFile(filePath ?? String.Empty);

        return grid;
    }
}
