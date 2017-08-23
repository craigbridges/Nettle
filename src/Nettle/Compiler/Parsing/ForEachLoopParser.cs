namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;

    /// <summary>
    /// Represents a for each loop code block parser
    /// </summary>
    internal sealed class ForEachLoopParser : NestedBlockParser<ForEachLoop>
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
        /// Extracts the 'for each' signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The variable signature</param>
        /// <returns>The parsed for each code block</returns>
        public override ForEachLoop Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var forLoop = UnwrapSignatureBody(signature);

            var collectionName = forLoop.RightOf
            (
                "foreach "
            );

            if (String.IsNullOrWhiteSpace(collectionName))
            {
                throw new NettleParseException
                (
                    "The loops collection name must be specified.",
                    positionOffSet
                );
            }

            var collectionType = ResolveType
            (
                collectionName
            );

            var nestedBody = ExtractNestedBody
            (
                ref templateContent,
                ref positionOffSet,
                signature,
                "foreach",
                "endfor"
            );

            return new ForEachLoop()
            {
                Signature = nestedBody.Signature,
                StartPosition = nestedBody.StartPosition,
                EndPosition = nestedBody.EndPosition,
                CollectionName = collectionName,
                CollectionType = collectionType,
                Body = nestedBody.Body,
                Blocks = nestedBody.Blocks
            };
        }
    }
}
