namespace Nettle.Data.Database;

using Nettle.Common.Serialization.Grid;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Defines a contract for a database adapter
/// </summary>
public interface IDbAdapter
{
    /// <summary>
    /// Asynchronously executes an SQL query against the database
    /// </summary>
    /// <param name="connectionString">The connection string</param>
    /// <param name="sql">The query to execute</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The data returned by the query</returns>
    Task<IDataGrid> ExecuteQuery(string connectionString, string sql, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously executes a stored procedure against the database
    /// </summary>
    /// <param name="connectionString">The connection string</param>
    /// <param name="procedureName">The procedure name</param>
    /// <param name="parameters">The parameters</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The data returned by the store procedure</returns>
    Task<IDataGrid> ExecuteStoredProcedure(string connectionString, string procedureName, Dictionary<string, object?> parameters, CancellationToken cancellationToken);
}
