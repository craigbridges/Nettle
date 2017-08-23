namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Defines a contract for a code block parser
    /// </summary>
    /// <typeparam name="T">The code block type</typeparam>
    internal interface IBlockParser<T>
        where T : CodeBlock
    {
        /// <summary>
        /// Parses the code block signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        T Parse
        (
            ref string templateContent,
            ref int positionOffSet,
            string signature
        );
    }
}
