namespace Nettle.Data.Functions;

using Nettle.Compiler;
using Nettle.Data.Database;
using Nettle.Functions;

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
    /// <returns>The data grid</returns>
    protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
    {
        Validate.IsNotNull(context);

        var connectionName = GetParameterValue<string>("ConnectionName", parameterValues);
        var sql = GetParameterValue<string>("SQL", parameterValues);

        var connection = _connectionRepository.GetConnection(connectionName ?? String.Empty);
        var grid = connection.Adapter.ExecuteQuery(connection.ConnectionString, sql ?? String.Empty);

        return grid;
    }
}
