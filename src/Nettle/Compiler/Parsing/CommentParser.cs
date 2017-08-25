namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a comment code block parser
    /// </summary>
    internal sealed class CommentParser : NettleParser, IBlockParser<Comment>
    {
        /// <summary>
        /// Parses the code block signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public Comment Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var signatureBody = UnwrapSignatureBody(signature);
            var text = signatureBody.RightOf(@"!").Trim();
            var startPosition = positionOffSet;
            var endPosition = startPosition + (signature.Length - 1);

            TrimTemplate
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new Comment()
            {
                Signature = signature,
                StartPosition = startPosition,
                EndPosition = endPosition,
                Text = text
            };
        }
    }
}
