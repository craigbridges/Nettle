namespace Nettle.Data.Database;

using Nettle.Common.Serialization.Grid;
using System.Collections.Generic;

/// <summary>
/// Defines a contract for a database adapter
/// </summary>
public interface IDbAdapter
{
    /// <summary>
    /// Executes an SQL query against the database
    /// </summary>
    /// <param name="connectionString">The connection string</param>
    /// <param name="sql">The query to execute</param>
    /// <returns>The data returned by the query</returns>
    IDataGrid ExecuteQuery(string connectionString, string sql);

    /// <summary>
    /// Executes a stored procedure against the database
    /// </summary>
    /// <param name="connectionString">The connection string</param>
    /// <param name="procedureName">The procedure name</param>
    /// <param name="parameters">The parameters</param>
    /// <returns>The data returned by the store procedure</returns>
    IDataGrid ExecuteStoredProcedure(string connectionString, string procedureName, Dictionary<string, object?> parameters);
}
