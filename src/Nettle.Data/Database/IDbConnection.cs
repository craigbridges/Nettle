namespace Nettle.Data.Database
{
    /// <summary>
    /// Defines a contract for a database connection
    /// </summary>
    public interface IDbConnection
    {
        /// <summary>
        /// Gets the database adapter
        /// </summary>
        IDbAdapter Adapter { get; }

        /// <summary>
        /// Gets the name of the database connection
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        string ConnectionString { get; }
    }
}
