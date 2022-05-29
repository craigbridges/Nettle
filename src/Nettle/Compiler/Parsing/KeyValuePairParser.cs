namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a key value pair parser
    /// </summary>
    internal sealed class KeyValuePairParser : NettleParser
    {
        /// <summary>
        /// Determines if a signature matches the block type of the parser
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <returns>True, if it matches; otherwise false</returns>
        public static bool Matches(string signatureBody)
        {
            var validStart = signatureBody.StartsWith('<');
            var validEnd = signatureBody.EndsWith('>');

            return validStart && validEnd;
        }

        /// <summary>
        /// Parses the code block signature into a code block object
        /// </summary>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public UnresolvedKeyValuePair Parse(string signature)
        {
            var signatureBody = UnwrapSignatureBody(signature);

            // Remove wrapping '<' and '>' characters and split by comma
            signatureBody = signatureBody.Crop(1, signatureBody.Length - 2);

            var tokenizer = new Tokenizer(',');
            var tokens = tokenizer.Tokenize(signatureBody);

            if (tokens.Length != 2)
            {
                throw new NettleParseException
                (
                    $"'{signature}' is not a valid key value pair signature."
                );
            }

            // Parse the key and value parts
            var keySignature = tokens[0].Trim();
            var valueSignature = tokens[1].Trim();

            var keyType = ResolveType(keySignature);
            var valueType = ResolveType(valueSignature);

            var parsedKey = keyType.ParseValue(keySignature);
            var parsedValue = valueType.ParseValue(valueSignature);

            return new UnresolvedKeyValuePair(signature)
            {
                ParsedKey = parsedKey,
                KeyType = keyType,
                ParsedValue = parsedValue,
                ValueType = valueType
            };
        }
    }
}
