namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a model binding code block
    /// </summary>
    internal class ModelBinding : CodeBlock
    {
        /// <summary>
        /// Gets the name of the property
        /// </summary>
        public string PropertyName { get; protected set; }
    }
}
