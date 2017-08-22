namespace Nettle.Compiler.Parsing
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
        /// Gets or sets the parameter value type
        /// </summary>
        public NettleValueType Type { get; set; }
    }
}
