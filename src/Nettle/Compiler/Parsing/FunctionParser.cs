namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a function code block parser
    /// </summary>
    internal sealed class FunctionParser : NettleParser, IBlockParser
    {
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
            return signatureBody.StartsWith("@");
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

            // Remove the leading '@' character
            body = body.Substring(1);

            if (false == body.EndsWith(")"))
            {
                var errorPosition =
                (
                    positionOffSet + (signature.Length - 1)
                );

                throw new NettleParseException
                (
                    "The functions closing bracket it missing.",
                    errorPosition
                );
            }
            
            // Extract functions name (before the parameters)
            var name = body.Crop
            (
                0,
                body.IndexOf('(') - 1
            );

            var parameters = new List<FunctionCallParameter>();
            var parameterStart = body.IndexOf('(');
            var parameterEnd = body.LastIndexOf(')');
            var parameterLength = (parameterEnd - parameterStart) - 1;

            var parameterSegment = body.Substring
            (
                parameterStart + 1,
                parameterLength
            );

            // Tokenize the functions parameters so we can parse them
            if (false == String.IsNullOrEmpty(parameterSegment))
            {
                var tokenizer = new Tokenizer(',');

                var parameterValues = tokenizer.Tokenize
                (
                    parameterSegment
                );

                // Parse each parameter signature and check what type it is
                foreach (var token in parameterValues)
                {
                    if (String.IsNullOrWhiteSpace(token))
                    {
                        throw new NettleParseException
                        (
                            "Parameter values cannot be empty.",
                            positionOffSet
                        );
                    }

                    var type = ResolveType(token);
                    var value = type.ParseValue(token);

                    parameters.Add
                    (
                        new FunctionCallParameter()
                        {
                            ValueSignature = token,
                            Value = value,
                            Type = type
                        }
                    );
                }
            }

            var startPosition = positionOffSet;
            var endPosition = (startPosition + signature.Length);

            TrimTemplate
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new FunctionCall()
            {
                Signature = signature,
                StartPosition = startPosition,
                EndPosition = endPosition,
                FunctionName = name,
                ParameterValues = parameters.ToArray()
            };
        }
    }
}
