namespace Nettle.Functions.General
{
    using Nettle.Compiler;

    /// <summary>
    /// Represents an function that gets an environment variable
    /// </summary>
    public sealed class GetEnvironmentVariableFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public GetEnvironmentVariableFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Name",
                "The name of the variable to get.",
                typeof(string)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Retrieves the value of a Nettle environment variable.";
            }
        }

        /// <summary>
        /// Determines if the path is resolvable
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>True, if the path exists; otherwise false</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var name = GetParameterValue<string>
            (
                "Name",
                parameterValues
            );

            return NettleEnvironment.GetEnvironmentVariable
            (
                name
            );
        }
    }
}
