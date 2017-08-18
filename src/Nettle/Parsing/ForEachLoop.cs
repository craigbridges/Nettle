namespace Nettle.Parsing
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
    }
}
