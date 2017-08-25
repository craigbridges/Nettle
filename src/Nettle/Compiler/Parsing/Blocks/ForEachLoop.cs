namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a 'for each' loop code block
    /// </summary>
    internal class ForEachLoop : NestableCodeBlock
    {
        /// <summary>
        /// Gets or sets the loops collection signature
        /// </summary>
        public string CollectionSignature { get; set; }

        /// <summary>
        /// Gets or sets the collections value type
        /// </summary>
        public NettleValueType CollectionType { get; set; }

        /// <summary>
        /// Gets or sets the collections value
        /// </summary>
        public object CollectionValue { get; set; }
    }
}
