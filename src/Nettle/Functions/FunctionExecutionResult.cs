namespace Nettle.Functions
{
    /// <summary>
    /// Represents the result of a function execution
    /// </summary>
    public sealed class FunctionExecutionResult
    {
        public FunctionExecutionResult(IFunction function, object? output, object?[] parameterValues)
        {
            Validate.IsNotNull(function);

            Function = function;
            Output = output;
            ParameterValues = parameterValues;

            GenerateCallSignature();
        }

        /// <summary>
        /// Generates the functions call signature
        /// </summary>
        private void GenerateCallSignature()
        {
            var parameterBuilder = new StringBuilder();

            if (ParameterValues != null)
            {
                foreach (var value in ParameterValues)
                {
                    var stringValue = "null";

                    if (value != null)
                    {
                        stringValue = value.ToString();
                    }

                    if (parameterBuilder.Length > 0)
                    {
                        parameterBuilder.Append(", ");
                    }

                    parameterBuilder.Append(stringValue);
                }
            }

            CallSignature = $"@{Function.Name}({parameterBuilder})";
        }

        /// <summary>
        /// Gets the function that was executed
        /// </summary>
        public IFunction Function { get; private set; }

        /// <summary>
        /// Gets the functions output
        /// </summary>
        public object? Output { get; private set; }

        /// <summary>
        /// Gets an array of parameter values that were supplied to the function
        /// </summary>
        public object?[] ParameterValues { get; private set; }

        /// <summary>
        /// Gets the functions execution call signature
        /// </summary>
        public string CallSignature { get; private set; } = default!;

        /// <summary>
        /// Provides a customer description of the result
        /// </summary>
        /// <returns>A description of the result</returns>
        public override string ToString() => CallSignature;
    }
}
