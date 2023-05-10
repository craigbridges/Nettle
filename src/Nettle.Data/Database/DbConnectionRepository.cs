namespace Nettle.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class DbConnectionRepository : IDbConnectionRepository
{
    private readonly Dictionary<string, IDbConnection> _connections;

    public DbConnectionRepository()
    {
        _connections = new Dictionary<string, IDbConnection>();
    }

    public void AddConnection(IDbConnection connection)
    {
        Validate.IsNotNull(connection);

        var name = connection.Name;
        var nameConflict = _connections.ContainsKey(name);

        if (nameConflict)
        {
            throw new InvalidOperationException($"A database connection named '{connection.Name}' already exists.");
        }

        _connections.Add(name, connection);
    }

    public IDbConnection GetConnection(string name)
    {
        Validate.IsNotEmpty(name);

        var found = _connections.ContainsKey(name);

        if (false == found)
        {
            throw new KeyNotFoundException($"No database connection named '{name}' was found.");
        }

        return _connections[name];
    }

    public IEnumerable<IDbConnection> GetAllConnections()
    {
        return _connections.Select(item => item.Value);
    }
}
