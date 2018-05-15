namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an anonymous type parser
    /// </summary>
    internal sealed class AnonymousTypeParser : NettleParser
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
            var validStart = signatureBody.StartsWith(@"[");
            var validEnd = signatureBody.EndsWith(@"]");

            return validStart && validEnd;
        }

        /// <summary>
        /// Parses the code block signature into a code block object
        /// </summary>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public UnresolvedAnonymousType Parse
            (
                string signature
            )
        {
            var signatureBody = UnwrapSignatureBody(signature);

            if (signatureBody.Trim() == "[]")
            {
                return new UnresolvedAnonymousType()
                {
                    Signature = signature,
                    Properties = new UnresolvedAnonymousTypeProperty[] { }
                };
            }
            else
            {
                // Remove wrapping '[' and ']' characters and split by comma
                signatureBody = signatureBody.Crop
                (
                    1,
                    signatureBody.Length - 2
                );

                var tokenizer = new Tokenizer(',');
                var tokens = tokenizer.Tokenize(signatureBody);
                var properties = new List<UnresolvedAnonymousTypeProperty>();

                foreach (var token in tokens)
                {
                    var itemName = token.LeftOf("=").Trim();
                    var assignmentSignature = token.RightOf("=").Trim();
                    var isValidName = itemName.IsValidPropertyName();

                    // Ensure property name is valid
                    if (false == isValidName)
                    {
                        var message = "The property name '{0}' is invalid.";

                        throw new NettleParseException
                        (
                            message.With(itemName)
                        );
                    }

                    var assignmentType = ResolveType
                    (
                        assignmentSignature
                    );

                    var assignmentValue = assignmentType.ParseValue
                    (
                        assignmentSignature
                    );

                    properties.Add
                    (
                        new UnresolvedAnonymousTypeProperty()
                        {
                            Name = itemName,
                            ValueType = assignmentType,
                            RawValue = assignmentValue
                        }
                    );
                }

                return new UnresolvedAnonymousType()
                {
                    Signature = signature,
                    Properties = properties.ToArray()
                };
            }
        }
    }
}
