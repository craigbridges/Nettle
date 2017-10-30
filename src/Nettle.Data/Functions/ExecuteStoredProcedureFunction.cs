namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Data.Database;
    using Nettle.Functions;

    /// <summary>
    /// Represents function for executing a stored procedure
    /// </summary>
    public class ExecuteStoredProcedureFunction : FunctionBase
    {
        private IDbConnectionRepository _connectionRepository;

        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        /// <param name="connectionRepository">The connection repository</param>
        public ExecuteStoredProcedureFunction
            (
                IDbConnectionRepository connectionRepository
            )
        {
            Validate.IsNotNull(connectionRepository);

            DefineRequiredParameter
            (
                "ConnectionName",
                "The database connection name.",
                typeof(string)
            );

            DefineRequiredParameter
            (
                "ProcedureName",
                "The name of the stored procedure to execute.",
                typeof(string)
            );

            _connectionRepository = connectionRepository;
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Executes a stored procedure and reads the results into a data grid.";
            }
        }

        /// <summary>
        /// Executes a stored procedure and reads the results into a data grid
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The data grid</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var connectionName = GetParameterValue<string>
            (
                "ConnectionName",
                parameterValues
            );

            var procedureName = GetParameterValue<string>
            (
                "ProcedureName",
                parameterValues
            );

            var procedureParameters = ExtractKeyValuePairs<string, object>
            (
                parameterValues,
                2
            );
            
            var connection = _connectionRepository.GetConnection
            (
                connectionName
            );

            var grid = connection.Adapter.ExecuteStoredProcedure
            (
                connection.ConnectionString,
                procedureName,
                procedureParameters
            );

            return grid;
        }
    }
}
