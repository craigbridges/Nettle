namespace Nettle.Data.Database
{
    /// <summary>
    /// Represents a database connection for SQL Server
    /// </summary>
    public class SqlClientConnection : IDbConnection
    {
        /// <summary>
        /// Constructs the connection with a name and connection string
        /// </summary>
        /// <param name="name">The connection name</param>
        /// <param name="connectionString">The connection string</param>
        public SqlClientConnection(string name, string connectionString)
        {
            Validate.IsNotEmpty(name);
            Validate.IsNotEmpty(connectionString);

            Adapter = new SqlClientAdapter();
            Name = name;
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets the database adapter
        /// </summary>
        public IDbAdapter Adapter { get; private set; }

        /// <summary>
        /// Gets the name of the database connection
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        public string ConnectionString { get; private set; }
    }
}
