namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a function call parameter
    /// </summary>
    internal class FunctionCallParameter
    {
        /// <summary>
        /// Gets or sets the value signature
        /// </summary>
        public string ValueSignature { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the parameter value type
        /// </summary>
        public NettleValueType Type { get; set; }
    }
}
