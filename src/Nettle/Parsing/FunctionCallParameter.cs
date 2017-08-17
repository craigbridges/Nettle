namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a function call parameter
    /// </summary>
    internal class FunctionCallParameter
    {
        /// <summary>
        /// Constructs the parameter with a value and type
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="type">The parameter type</param>
        public FunctionCallParameter
            (
                string value,
                FunctionCallParameterType type
            )
        {
            this.Value = value;
            this.Type = type;
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets the parameter type
        /// </summary>
        public FunctionCallParameterType Type { get; private set; }

        /// <summary>
        /// Provides a string representation of the parameter value
        /// </summary>
        /// <returns>The value as a string</returns>
        public override string ToString()
        {
            return this.Value;
        }
    }
}
