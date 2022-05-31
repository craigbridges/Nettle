namespace Nettle.Data
{
    using Nettle.Data.Functions;

    public class NettleDataResolver : DefaultNettleResolver
    {
        public NettleDataResolver()
        {
            ConnectionRepository = new DbConnectionRepository();
        }

        /// <summary>
        /// Generates a custom collection of data functions
        /// </summary>
        /// <returns>A collection of functions</returns>
        public override IEnumerable<IFunction> ResolveFunctions()
        {
            var functions = base.ResolveFunctions().ToList();

            var queryFunction = new ExecuteQueryFunction(ConnectionRepository);
            var procedureFunction = new ExecuteStoredProcedureFunction(ConnectionRepository);

            functions.Add(queryFunction);
            functions.Add(procedureFunction);

            return functions;
        }

        /// <summary>
        /// Gets the database connections repository
        /// </summary>
        public IDbConnectionRepository ConnectionRepository { get; private set; }
    }
}
