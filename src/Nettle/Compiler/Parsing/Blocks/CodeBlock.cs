namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a parsed block of template code
    /// </summary>
    internal abstract class CodeBlock
    {
        /// <summary>
        /// Gets or sets the blocks signature
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets blocks the start position
        /// </summary>
        public int StartPosition { get; set; }

        /// <summary>
        /// Gets or sets blocks the end position
        /// </summary>
        public int EndPosition { get; set; }

        /// <summary>
        /// Provides a custom string representation of the code block
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return this.Signature;
        }
    }
}
