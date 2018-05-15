namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents an unresolved anonymous type property code block
    /// </summary>
    internal class UnresolvedAnonymousTypeProperty : CodeBlock
    {
        /// <summary>
        /// Gets or sets the property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value type
        /// </summary>
        public NettleValueType ValueType { get; set; }

        /// <summary>
        /// Gets or sets the raw value
        /// </summary>
        public object RawValue { get; set; }
    }
}
