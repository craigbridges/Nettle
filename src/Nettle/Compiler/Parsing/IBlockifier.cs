namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Defines a contract for a code blockifier
    /// </summary>
    internal interface IBlockifier
    {
        /// <summary>
        /// Parses the content specified into code blocks
        /// </summary>
        /// <param name="content">The content to parse</param>
        /// <returns>An array of code blocks</returns>
        CodeBlock[] Blockify
        (
            string content
        );
    }
}
