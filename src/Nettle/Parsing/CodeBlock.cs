namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a parsed block of template code
    /// </summary>
    internal abstract class CodeBlock
    {
        public CodeBlock()
        {

        }

        /// <summary>
        /// Gets a key value assigned to the code block
        /// </summary>
        public string Key { get; protected set; }

        /// <summary>
        /// Gets the blocks raw text
        /// </summary>
        public string RawText { get; protected set; }

        /// <summary>
        /// Gets blocks the start position
        /// </summary>
        public long StartPosition { get; protected set; }

        /// <summary>
        /// Gets blocks the end position
        /// </summary>
        public long EndPosition { get; protected set; }
    }
}
