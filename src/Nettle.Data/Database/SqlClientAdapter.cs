namespace Nettle.Data.Database;

using Nettle.Common.Serialization.Grid;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

/// <summary>
/// Represents a database adapter for SQL Server
/// </summary>
public class SqlClientAdapter : IDbAdapter
{
    public async Task<IDataGrid> ExecuteQuery(string connectionString, string sql, CancellationToken cancellationToken)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;

                await connection.OpenAsync(cancellationToken);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    return await ReadToGrid(reader, cancellationToken);
                }
            }
        }
    }

    public async Task<IDataGrid> ExecuteStoredProcedure(string connectionString, string procedureName, Dictionary<string, object?> parameters, CancellationToken cancellationToken)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync(cancellationToken);
                
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(new(parameter.Key, parameter.Value));
                }
                
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    return await ReadToGrid(reader, cancellationToken);
                }
            }
        }
    }

    /// <summary>
    /// Reads an SQL data reader into a new data grid
    /// </summary>
    /// <param name="reader">The data reader</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The data grid generated</returns>
    private static async Task<IDataGrid> ReadToGrid(SqlDataReader reader, CancellationToken cancellationToken)
    {
        var grid = new DataGrid("QueryResults");

        if (reader.HasRows)
        {
            while (await reader.ReadAsync(cancellationToken))
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
