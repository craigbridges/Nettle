namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Compiler.Parsing.Conditions;
    using System;

    /// <summary>
    /// Represents an 'while' loop code block parser
    /// </summary>
    internal sealed class WhileLoopParser : NestedBlockParser
    {
        private BooleanExpressionParser _expressionParser;

        /// <summary>
        /// Constructs the parser with a blockifier
        /// </summary>
        /// <param name="blockifier">The blockifier</param>
        public WhileLoopParser
            (
                IBlockifier blockifier
            )

            : base(blockifier)
        {
            _expressionParser = new BooleanExpressionParser();
        }
        
        /// <summary>
        /// Gets the tag name
        /// </summary>
        protected override string TagName
        {
            get
            {
                return "while";
            }
        }

        /// <summary>
        /// Parses the 'while' loop signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The loop signature</param>
        /// <returns>The parsed while loop</returns>
        public override CodeBlock Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var signatureBody = UnwrapSignatureBody
            (
                signature
            );

            var conditionSignature = signatureBody.RightOf
            (
                "{0} ".With
                (
                    this.TagName
                )
            );

            if (String.IsNullOrWhiteSpace(conditionSignature))
            {
                throw new NettleParseException
                (
                    "The while loops condition must be specified.",
                    positionOffSet
                );
            }

            var expression = _expressionParser.Parse
            (
                conditionSignature
            );
            
            var nestedBody = ExtractNestedBody
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new WhileLoop()
            {
                Signature = nestedBody.Signature,
                StartPosition = nestedBody.StartPosition,
                EndPosition = nestedBody.EndPosition,
                ConditionExpression = expression,
                Body = nestedBody.Body,
                Blocks = nestedBody.Blocks
            };
        }
    }
}
