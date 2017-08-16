namespace Nettle.Functions
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a contract for a Nettle function
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Gets the name of the function (this is the value used to call the function)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a description for the function (for documentation purposes)
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a collection of all parameters for the function
        /// </summary>
        IEnumerable<FunctionParameter> GetAllParameters();

        /// <summary>
        /// Gets a collection of parameters that are required
        /// </summary>
        /// <returns>A collection of matching function parameters</returns>
        IEnumerable<FunctionParameter> GetRequiredParameters();

        /// <summary>
        /// Gets a collection of parameters that are optional
        /// </summary>
        /// <returns>A collection of matching function parameters</returns>
        IEnumerable<FunctionParameter> GetOptionalParameters();

        /// <summary>
        /// Gets the function parameter by the name specified
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The matching parameter</returns>
        FunctionParameter GetParameter
        (
            string name
        );

        /// <summary>
        /// Executes the function against a template model and parameter values
        /// </summary>
        /// <param name="model">The template model</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The execution result</returns>
        FunctionExecutionResult Execute
        (
            TemplateModel model,
            params object[] parameterValues
        );
    }
}
