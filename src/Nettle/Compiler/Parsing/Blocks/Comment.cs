namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a comment code block
    /// </summary>
    internal record class Comment(string Signature) : CodeBlock(Signature)
    {
        /// <summary>
        /// Gets or sets the comments text
        /// </summary>
        public string? Text { get; init; }
    }
}
