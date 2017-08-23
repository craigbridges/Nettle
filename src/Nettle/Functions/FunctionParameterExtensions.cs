namespace Nettle.Functions
{
    using Nettle.Compiler.Parsing;
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents various extension methods for function parameters
    /// </summary>
    public static class FunctionParameterExtensions
    {
        /// <summary>
        /// Determines if the parameter accepts the value specified
        /// </summary>
        /// <param name="parameter">The function parameter</param>
        /// <param name="parameterValue">The parameter value</param>
        /// <returns>True, if it accepts the value; otherwise false</returns>
        internal static bool Accepts
            (
                this FunctionParameter parameter,
                FunctionCallParameter parameterValue
            )
        {
            Validate.IsNotNull(parameter);
            Validate.IsNotNull(parameterValue);

            switch (parameterValue.Type)
            {
                case NettleValueType.ModelBinding:
                case NettleValueType.Variable:

                    // We assume the value is valid because we can't resolve the 
                    // values until the template is executed with a model
                    return true;
            }

            if (parameter.DataType == typeof(string))
            {
                return 
                (
                    parameterValue.Type == NettleValueType.String
                );
            }
            else if (parameter.DataType.IsNumeric())
            {
                return
                (
                    parameterValue.Type == NettleValueType.Number
                );
            }
            else if (parameter.DataType == typeof(bool))
            {
                return
                (
                    parameterValue.Type == NettleValueType.Boolean
                );
            }
            else
            {
                // NOTE:
                // If we can't resolve the type then assume valid
                return true;
            }
        }
    }
}
