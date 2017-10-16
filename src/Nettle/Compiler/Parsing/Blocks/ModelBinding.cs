namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a model binding code block
    /// </summary>
    internal class ModelBinding : CodeBlock
    {
        /// <summary>
        /// Gets or sets the bindings path
        /// </summary>
        /// <remarks>
        /// The binding path can contain nested properties and 
        /// these are donated by using a dot "." separator.
        /// </remarks>
        public string BindingPath { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the binding has an indexer
        /// </summary>
        /// <remarks>
        /// The indexer works in the same as the C# indexer
        /// </remarks>
        public bool HasIndexer { get; set; }

        /// <summary>
        /// Gets or sets the index number
        /// </summary>
        public int Index { get; set; }
    }
}
