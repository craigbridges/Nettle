namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;

    /// <summary>
    /// Represents an if statement code block parser
    /// </summary>
    internal sealed class IfStatementParser : NestedBlockParser
    {
        /// <summary>
        /// Constructs the parser with a blockifier
        /// </summary>
        /// <param name="blockifier">The blockifier</param>
        public IfStatementParser
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
                return "if";
            }
        }

        /// <summary>
        /// Gets the close tag name
        /// </summary>
        protected override string CloseTagName
        {
            get
            {
                return "endif";
            }
        }

        /// <summary>
        /// Parses the 'if statement' signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The variable signature</param>
        /// <returns>The parsed if statement</returns>
        public override CodeBlock Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var ifStatement = UnwrapSignatureBody
            (
                signature
            );

            var conditionSignature = ifStatement.RightOf
            (
                "if "
            );

            if (String.IsNullOrWhiteSpace(conditionSignature))
            {
                throw new NettleParseException
                (
                    "The if statements condition name must be specified.",
                    positionOffSet
                );
            }

            var conditionType = ResolveType
            (
                conditionSignature
            );

            var conditionValue = conditionType.ParseValue
            (
                conditionSignature
            );

            var nestedBody = ExtractNestedBody
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new IfStatement()
            {
                Signature = nestedBody.Signature,
                StartPosition = nestedBody.StartPosition,
                EndPosition = nestedBody.EndPosition,
                ConditionSignature = conditionSignature,
                ConditionType = conditionType,
                ConditionValue = conditionValue,
                Body = nestedBody.Body,
                Blocks = nestedBody.Blocks
            };
        }
    }
}
