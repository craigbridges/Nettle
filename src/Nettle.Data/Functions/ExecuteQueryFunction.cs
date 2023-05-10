namespace Nettle.Data.Functions;

using Nettle.Data.Database;
using Nettle.Functions;
using System.Threading.Tasks;

/// <summary>
/// Represents function for executing an SQL query
/// </summary>
public class ExecuteQueryFunction : FunctionBase
{
    private readonly IDbConnectionRepository _connectionRepository;

    public ExecuteQueryFunction(IDbConnectionRepository connectionRepository)
    {
        Validate.IsNotNull(connectionRepository);

        DefineRequiredParameter("ConnectionName", "The database connection name.", typeof(string));
        DefineRequiredParameter("SQL", "The SQL query to execute.", typeof(string));

        _connectionRepository = connectionRepository;
    }

    public override string Description => "Executes an SQL query and reads the results into a data grid.";

    /// <summary>
    /// Executes an SQL query and reads the results into a data grid
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="parameterValues">The parameter values</param>
    /// <returns>A new data grid with the results of the query</returns>
    protected override async Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var connectionName = GetParameterValue<string>("ConnectionName", request);
        var sql = GetParameterValue<string>("SQL", request);

        var connection = _connectionRepository.GetConnection(connectionName ?? String.Empty);
        var grid = await connection.Adapter.ExecuteQuery(connection.ConnectionString, sql ?? String.Empty, cancellationToken);

        return Task.FromResult<object?>(grid);
    }
}
