namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a 'for each' loop code block
    /// </summary>
    internal class ForEachLoop : CodeBlock
    {
        /// <summary>
        /// Gets the loops collection name
        /// </summary>
        public string CollectionName { get; protected set; }

        /// <summary>
        /// Gets the loops body
        /// </summary>
        public Template Body { get; protected set; }
    }
}
