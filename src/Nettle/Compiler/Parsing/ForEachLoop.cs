namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents a 'for each' loop code block
    /// </summary>
    internal class ForEachLoop : NestableCodeBlock
    {
        /// <summary>
        /// Gets or sets the loops collection name
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the collections value type
        /// </summary>
        public NettleValueType CollectionType { get; set; }
    }
}
