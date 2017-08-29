namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;

    /// <summary>
    /// Represents a variable declaration code block parser
    /// </summary>
    internal sealed class VariableParser : NettleParser, IBlockParser
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
            return signatureBody.StartsWith("var ");
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

            body = body.RightOf("var ");

            var parts = body.Split('=');

            if (parts.Length != 2)
            {
                throw new NettleParseException
                (
                    "The variable declaration '{0}' has invalid syntax.".With
                    (
                        signature
                    ),
                    positionOffSet
                );
            }

            var name = parts[0].Trim();
            var valueSignature = parts[1].Trim();
            var type = ResolveType(valueSignature);
            var value = type.ParseValue(valueSignature);

            if (false == IsValidVariableName(name))
            {
                throw new NettleParseException
                (
                    "The variable name '{0}' is invalid.".With
                    (
                        name
                    ),
                    positionOffSet
                );
            }

            var startPosition = positionOffSet;
            var endPosition = signature.Length - 1;

            TrimTemplate
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            var declaration = new VariableDeclaration()
            {
                Signature = signature,
                StartPosition = startPosition,
                EndPosition = endPosition,
                VariableName = name,
                AssignedValueSignature = valueSignature,
                AssignedValue = value,
                ValueType = type
            };

            return declaration;
        }

        /// <summary>
        /// Determines if a variable name is valid
        /// </summary>
        /// <param name="name">The variable name</param>
        /// <returns>True, if the variable name is valid; otherwise false</returns>
        private bool IsValidVariableName
            (
                string name
            )
        {
            if (String.IsNullOrWhiteSpace(name) || name.Contains(" "))
            {
                return false;
            }
            else if (name.ContainsNonAscii())
            {
                return false;
            }
            else if (name.IsNumeric())
            {
                return false;
            }
            else if (false == name.IsAlphaNumeric())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
