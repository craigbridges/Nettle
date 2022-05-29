﻿namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a comment code block parser
    /// </summary>
    internal sealed class CommentParser : NettleParser, IBlockParser
    {
        /// <summary>
        /// Determines if a signature matches the block type of the parser
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <returns>True, if it matches; otherwise false</returns>
        public bool Matches(string signatureBody) => signatureBody.StartsWith(@"!");

        /// <summary>
        /// Parses the code block signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
        {
            var signatureBody = UnwrapSignatureBody(signature);
            var text = signatureBody.RightOf(@"!").Trim();
            var startPosition = positionOffSet;
            var endPosition = (startPosition + signature.Length);

            TrimTemplate(ref templateContent, ref positionOffSet, signature);

            return new Comment(signature)
            {
                StartPosition = startPosition,
                EndPosition = endPosition,
                Text = text
            };
        }
    }
}
