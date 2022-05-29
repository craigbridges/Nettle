namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a parsed block of template code
    /// </summary>
    /// <param name="Signature">The blocks signature (i.e. the content that was parsed)</param>
    internal abstract record class CodeBlock(string Signature)
    {
        /// <summary>
        /// Gets or sets blocks the start position
        /// </summary>
        public int StartPosition { get; init; }

        /// <summary>
        /// Gets or sets blocks the end position
        /// </summary>
        public int EndPosition { get; init; }

        /// <summary>
        /// Gets the number of characters in the code block
        /// </summary>
        public int Length => (EndPosition - StartPosition);

        /// <summary>
        /// Provides a custom string representation of the code block
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString() => $"[{StartPosition}, {EndPosition}] \r\n{Signature}";
    }
}
