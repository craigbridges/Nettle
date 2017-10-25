namespace Nettle.Data.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the default implementation for the connection repository
    /// </summary>
    public sealed class DbConnectionRepository : IDbConnectionRepository
    {
        private Dictionary<string, IDbConnection> _connections;

        public DbConnectionRepository()
        {
            _connections = new Dictionary<string, IDbConnection>();
        }

        /// <summary>
        /// Adds a new connection to the repository
        /// </summary>
        /// <param name="connection">The database connection</param>
        public void AddConnection
            (
                IDbConnection connection
            )
        {
            Validate.IsNotNull(connection);

            var name = connection.Name;
            var nameConflict = _connections.ContainsKey(name);

            if (nameConflict)
            {
                throw new InvalidOperationException
                (
                    "A database connection named '{0}' already exists.".With
                    (
                        connection.Name
                    )
                );
            }

            _connections.Add(name, connection);
        }

        /// <summary>
        /// Gets a single connection from the repository
        /// </summary>
        /// <param name="name">The connection name</param>
        /// <returns>The database connection</returns>
        public IDbConnection GetConnection
            (
                string name
            )
        {
            Validate.IsNotEmpty(name);

            var found = _connections.ContainsKey(name);

            if (false == found)
            {
                throw new KeyNotFoundException
                (
                    "No database connection named '{0}' was found.".With
                    (
                        name
                    )
                );
            }

            return _connections[name];
        }

        /// <summary>
        /// Gets all connections from the repository
        /// </summary>
        /// <returns>A collection of database connections</returns>
        public IEnumerable<IDbConnection> GetAllConnections()
        {
            return _connections.Select
            (
                item => item.Value
            );
        }
    }
}
