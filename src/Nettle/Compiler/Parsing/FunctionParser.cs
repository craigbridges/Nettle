namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

            body = body.RightOf("@");

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

            var name = body.LeftOf("(");
            var parameters = new List<FunctionCallParameter>();

            var parameterStart = body.IndexOf('(');
            var parameterEnd = body.LastIndexOf(')');
            var parameterLength = (parameterEnd - parameterStart) - 1;

            var parameterSegment = body.Substring
            (
                parameterStart + 1,
                parameterLength
            );

            if (false == String.IsNullOrEmpty(parameterSegment))
            {
                var parameterValues = parameterSegment.Split(',').Select
                (
                    s => s.Trim()
                );

                // Parse each parameter signature and check what type it is
                foreach (var valueSignature in parameterValues)
                {
                    if (String.IsNullOrWhiteSpace(valueSignature))
                    {
                        throw new NettleParseException
                        (
                            "Parameter values cannot be empty.",
                            positionOffSet
                        );
                    }

                    var type = ResolveType(valueSignature);
                    var value = type.ParseValue(valueSignature);

                    parameters.Add
                    (
                        new FunctionCallParameter()
                        {
                            ValueSignature = valueSignature,
                            Value = value,
                            Type = type
                        }
                    );
                }
            }

            var startPosition = positionOffSet;
            var endPosition = signature.Length - 1;

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
