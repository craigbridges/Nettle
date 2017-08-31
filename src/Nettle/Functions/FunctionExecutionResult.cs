namespace Nettle.Functions
{
    using System.Text;

    /// <summary>
    /// Represents the result of a function execution
    /// </summary>
    public sealed class FunctionExecutionResult
    {
        /// <summary>
        /// Constructs the function execution result with dependencies
        /// </summary>
        /// <param name="function">The function that was executed</param>
        /// <param name="output">The output generated</param>
        /// <param name="parameterValues">The parameter values supplied</param>
        public FunctionExecutionResult
            (
                IFunction function,
                object output,
                object[] parameterValues
            )
        {
            Validate.IsNotNull(function);

            this.Function = function;
            this.Output = output;
            this.ParameterValues = parameterValues;

            GenerateCallSignature();
        }

        /// <summary>
        /// Generates the functions call signature
        /// </summary>
        private void GenerateCallSignature()
        {
            var template = "@{0}({1})";
            var parameterBuilder = new StringBuilder();

            if (this.ParameterValues != null)
            {
                foreach (var value in this.ParameterValues)
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

            this.CallSignature = template.With
            (
                this.Function.Name,
                parameterBuilder.ToString()
            );
        }

        /// <summary>
        /// Gets the function that was executed
        /// </summary>
        public IFunction Function { get; private set; }

        /// <summary>
        /// Gets the functions output
        /// </summary>
        public object Output { get; private set; }

        /// <summary>
        /// Gets an array of parameter values that were supplied to the function
        /// </summary>
        public object[] ParameterValues { get; private set; }

        /// <summary>
        /// Gets the functions execution call signature
        /// </summary>
        public string CallSignature { get; private set; }

        /// <summary>
        /// Provides a customer description of the result
        /// </summary>
        /// <returns>A description of the result</returns>
        public override string ToString()
        {
            return this.CallSignature;
        }
    }
}
