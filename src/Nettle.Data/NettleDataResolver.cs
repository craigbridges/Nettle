namespace Nettle.Data
{
    using Nettle.Compiler;
    using Nettle.Data.Database;
    using Nettle.Data.Functions;
    using Nettle.Functions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a Nettle data resolver
    /// </summary>
    public class NettleDataResolver : DefaultNettleResolver
    {
        public NettleDataResolver()
        {
            this.ConnectionRepository = new DbConnectionRepository();
        }

        /// <summary>
        /// Generates a custom collection of data functions
        /// </summary>
        /// <returns>A collection of functions</returns>
        public override IEnumerable<IFunction> ResolveFunctions()
        {
            var functions = base.ResolveFunctions().ToList();

            var queryFunction = new ExecuteQueryFunction
            (
                this.ConnectionRepository
            );

            var procedureFunction = new ExecuteStoredProcedureFunction
            (
                this.ConnectionRepository
            );

            functions.Add(queryFunction);
            functions.Add(procedureFunction);

            return functions;
        }

        /// <summary>
        /// Gets the database connections repository
        /// </summary>
        public IDbConnectionRepository ConnectionRepository
        {
            get;
            private set;
        }
    }
}
