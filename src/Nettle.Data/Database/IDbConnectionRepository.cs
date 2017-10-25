namespace Nettle.Data.Database
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a contract for a repository that manages database connections
    /// </summary>
    public interface IDbConnectionRepository
    {
        /// <summary>
        /// Adds a new connection to the repository
        /// </summary>
        /// <param name="connection">The database connection</param>
        void AddConnection
        (
            IDbConnection connection
        );

        /// <summary>
        /// Gets a single connection from the repository
        /// </summary>
        /// <param name="name">The connection name</param>
        /// <returns>The database connection</returns>
        IDbConnection GetConnection
        (
            string name
        );

        /// <summary>
        /// Gets all connections from the repository
        /// </summary>
        /// <returns>A collection of database connections</returns>
        IEnumerable<IDbConnection> GetAllConnections();
    }
}
