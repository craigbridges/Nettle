namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Defines a contract for a code block parser
    /// </summary>
    internal interface IBlockParser
    {
        /// <summary>
        /// Determines if a signature matches the block type of the parser
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <returns>True, if it matches; otherwise false</returns>
        bool Matches
        (
            string signatureBody
        );

        /// <summary>
        /// Parses the code block signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        CodeBlock Parse
        (
            ref string templateContent,
            ref int positionOffSet,
            string signature
        );
    }
}
