namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a comment code block
    /// </summary>
    internal class Comment : CodeBlock
    {
        /// <summary>
        /// Gets or sets the comments text
        /// </summary>
        public string Text { get; set; }
    }
}
