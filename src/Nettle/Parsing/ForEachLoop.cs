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

        /// <summary>
        /// Gets or sets a flag indicating if the collection is a model binding
        /// </summary>
        public bool IsModelBinding { get; set; }
    }
}
