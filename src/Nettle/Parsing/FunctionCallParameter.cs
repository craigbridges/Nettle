namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a function call parameter
    /// </summary>
    internal class FunctionCallParameter
    {
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the parameter type
        /// </summary>
        public FunctionCallParameterType Type { get; set; }
    }
}
