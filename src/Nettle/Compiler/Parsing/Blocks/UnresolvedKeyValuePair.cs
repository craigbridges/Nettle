namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents an unresolved key value pair code block
    /// </summary>
    /// <param name="Signature">The blocks signature</param>
    internal record class UnresolvedKeyValuePair(string Signature) : CodeBlock(Signature)
    {
        /// <summary>
        /// Gets or sets the unresolved parsed key
        /// </summary>
        public object? ParsedKey { get; set; }

        /// <summary>
        /// Gets or sets the unresolved key value type
        /// </summary>
        public NettleValueType KeyType { get; set; }

        /// <summary>
        /// Gets or sets the unresolved parsed value
        /// </summary>
        public object? ParsedValue { get; set; }

        /// <summary>
        /// Gets or sets the unresolved value type
        /// </summary>
        public NettleValueType ValueType { get; set; }
    }
}
