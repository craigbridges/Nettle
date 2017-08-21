namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a nestable code block
    /// </summary>
    internal class NestableCodeBlock : CodeBlock
    {
        /// <summary>
        /// Gets or sets the blocks raw body content
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the nested code blocks
        /// </summary>
        public CodeBlock[] Blocks { get; set; }
    }
}
