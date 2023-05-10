namespace Nettle.Data.Functions;

using Nettle.Data.Database;
using Nettle.Functions;
using System.Threading.Tasks;

/// <summary>
/// Represents function for executing a stored procedure
/// </summary>
public class ExecuteStoredProcedureFunction : FunctionBase
{
    private readonly IDbConnectionRepository _connectionRepository;

    public ExecuteStoredProcedureFunction(IDbConnectionRepository connectionRepository)
    {
        Validate.IsNotNull(connectionRepository);

        DefineRequiredParameter("ConnectionName", "The database connection name.", typeof(string));
        DefineRequiredParameter("ProcedureName", "The name of the stored procedure to execute.", typeof(string));

        _connectionRepository = connectionRepository;
    }

    public override string Description => "Executes a stored procedure and reads the results into a data grid.";

    /// <summary>
    /// Executes a stored procedure and reads the results into a data grid
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="parameterValues">The parameter values</param>
    /// <returns>A new data grid with the data returned from the stored procedure</returns>
    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var connectionName = GetParameterValue<string>("ConnectionName", request);
        var procedureName = GetParameterValue<string>("ProcedureName", request);
        var procedureParameters = ExtractKeyValuePairs<string, object>(request.ParameterValues, 2);
        
        var connection = _connectionRepository.GetConnection(connectionName ?? String.Empty);

        var grid = await connection.Adapter.ExecuteStoredProcedure
        (
            connection.ConnectionString,
            procedureName ?? String.Empty,
            procedureParameters,
            cancellationToken
        );

        return grid;
    }
}
