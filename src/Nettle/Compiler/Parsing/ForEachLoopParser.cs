namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;

    /// <summary>
    /// Represents a for each loop code block parser
    /// </summary>
    internal sealed class ForEachLoopParser : NestedBlockParser
    {
        /// <summary>
        /// Constructs the parser with a blockifier
        /// </summary>
        /// <param name="blockifier">The blockifier</param>
        public ForEachLoopParser
            (
                IBlockifier blockifier
            )

            : base(blockifier)
        { }

        /// <summary>
        /// Gets the open tag name
        /// </summary>
        protected override string OpenTagName
        {
            get
            {
                return "foreach";
            }
        }

        /// <summary>
        /// Gets the close tag name
        /// </summary>
        protected override string CloseTagName
        {
            get
            {
                return "endfor";
            }
        }

        /// <summary>
        /// Extracts the 'for each' signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The variable signature</param>
        /// <returns>The parsed for each code block</returns>
        public override CodeBlock Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var forLoop = UnwrapSignatureBody
            (
                signature
            );

            var collectionSignature = forLoop.RightOf
            (
                "foreach "
            );

            if (String.IsNullOrWhiteSpace(collectionSignature))
            {
                throw new NettleParseException
                (
                    "The loops collection name must be specified.",
                    positionOffSet
                );
            }

            var collectionType = ResolveType
            (
                collectionSignature
            );

            var collectionValue = collectionType.ParseValue
            (
                collectionSignature
            );

            var nestedBody = ExtractNestedBody
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new ForEachLoop()
            {
                Signature = nestedBody.Signature,
                StartPosition = nestedBody.StartPosition,
                EndPosition = nestedBody.EndPosition,
                CollectionSignature = collectionSignature,
                CollectionType = collectionType,
                CollectionValue = collectionValue,
                Body = nestedBody.Body,
                Blocks = nestedBody.Blocks
            };
        }
    }
}
