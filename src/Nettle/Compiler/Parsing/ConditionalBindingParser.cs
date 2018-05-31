namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Compiler.Parsing.Conditions;

    /// <summary>
    /// Represents a conditional binding code block parser
    /// </summary>
    internal sealed class ConditionalBindingParser : NettleParser, IBlockParser
    {
        private BooleanExpressionParser _expressionParser;

        /// <summary>
        /// Constructs the parser with a new expression parser
        /// </summary>
        public ConditionalBindingParser()
        {
            _expressionParser = new BooleanExpressionParser();
        }

        /// <summary>
        /// Determines if a signature matches the block type of the parser
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <returns>True, if it matches; otherwise false</returns>
        public bool Matches
            (
                string signatureBody
            )
        {
            var expression = signatureBody.Trim();

            if (false == expression.StartsWith(@"="))
            {
                return false;
            }
            else if (false == expression.Contains(@"?") && expression.Contains(@":"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        /// <summary>
        /// Parses the code block signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public CodeBlock Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var body = UnwrapSignatureBody(signature);

            body = body.Substring(1).Trim();

            if (false == body.StartsWith("("))
            {
                throw new NettleParseException
                (
                    "The condition must be wrapped in brackets.",
                    positionOffSet
                );
            }
            
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(body);

            // There should be five tokens in total
            // Also, ensure tokens 1 and 3 are ? and : characters
            if (tokens.Length != 5 || tokens[1] != "?" || tokens[3] != ":")
            {
                var message = "The syntax for the condition '{0}' is invalid.";

                throw new NettleParseException
                (
                    message.With(body),
                    positionOffSet
                );
            }
            
            var startPosition = positionOffSet;
            var endPosition = (startPosition + signature.Length);

            // Get the condition and model binding expressions
            var conditionSignature = tokens[0];
            var trueSignature = tokens[2];
            var falseSignature = tokens[4];

            var conditionExpression = _expressionParser.Parse
            (
                conditionSignature
            );

            // Parse the true and false expressions
            var trueValueType = ResolveType(trueSignature);
            var trueValue = trueValueType.ParseValue(trueSignature);
            var falseValueType = ResolveType(falseSignature);
            var falseValue = falseValueType.ParseValue(falseSignature);
            
            // Remove the code block from the template
            TrimTemplate
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new ConditionalBinding()
            {
                Signature = signature,
                StartPosition = startPosition,
                EndPosition = endPosition,
                ConditionExpression = conditionExpression,
                TrueValueSignature = trueSignature,
                TrueValue = trueValue,
                TrueValueType = trueValueType,
                FalseValueSignature = falseSignature,
                FalseValue = falseValue,
                FalseValueType = falseValueType
            };
        }
    }
}
