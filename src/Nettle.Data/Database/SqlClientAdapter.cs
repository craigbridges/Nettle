namespace Nettle.Data.Database;

using Nettle.Common.Serialization.Grid;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Represents a database adapter for SQL Server
/// </summary>
public class SqlClientAdapter : IDbAdapter
{
    /// <summary>
    /// Executes an SQL query against the database
    /// </summary>
    /// <param name="connectionString">The connection string</param>
    /// <param name="sql">The query to execute</param>
    /// <returns>The data returned by the query</returns>
    public IDataGrid ExecuteQuery(string connectionString, string sql)
    {
        Validate.IsNotEmpty(connectionString);
        Validate.IsNotEmpty(sql);

        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    return ReadToGrid(reader);
                }
            }
        }
    }

    /// <summary>
    /// Executes a stored procedure against the database
    /// </summary>
    /// <param name="connectionString">The connection string</param>
    /// <param name="procedureName">The procedure name</param>
    /// <param name="parameters">The parameters</param>
    /// <returns>The data returned by the store procedure</returns>
    public IDataGrid ExecuteStoredProcedure(string connectionString, string procedureName, Dictionary<string, object?> parameters)
    {
        Validate.IsNotEmpty(connectionString);
        Validate.IsNotEmpty(procedureName);
        Validate.IsNotNull(parameters);

        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(new(parameter.Key, parameter.Value));
                }
                
                using (var reader = command.ExecuteReader())
                {
                    return ReadToGrid(reader);
                }
            }
        }
    }

    /// <summary>
    /// Reads an SQL data reader into a new data grid
    /// </summary>
    /// <param name="reader">The data reader</param>
    /// <returns>The data grid generated</returns>
    private static IDataGrid ReadToGrid(SqlDataReader reader)
    {
        var grid = new DataGrid("QueryResults");

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                var rowValues = new Dictionary<string, object?>();
                
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var fieldName = reader.GetName(i);
                    var fieldValue = reader.GetValue(i);

                    rowValues.Add(fieldName, fieldValue);
                }

                grid.AddRow(rowValues.ToArray());
            }
        }

        return grid;
    }
}
