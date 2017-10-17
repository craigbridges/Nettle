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
    }
}
