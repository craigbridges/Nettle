namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents an enum value parser
    /// </summary>
    internal sealed class EnumParser : NettleParser
    {
        /// <summary>
        /// Parses the code block signature into an unresolved enum value
        /// </summary>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public static object Parse(string signature)
        {
            var signatureBody = UnwrapSignatureBody(signature);
            var parseErrorMessage = $"'{signature}' is not a valid enum signature.";

            if (false == signatureBody.StartsWith("Enum."))
            {
                throw new NettleParseException(parseErrorMessage);
            }

            signatureBody = signatureBody.RightOf("Enum.");

            var tokenizer = new Tokenizer('.');
            var tokens = tokenizer.Tokenize(signatureBody);

            if (tokens.Length < 2)
            {
                throw new NettleParseException(parseErrorMessage);
            }

            // Parse the enum type name and value name parts
            var typeSignature = String.Join('.', tokens[..^1]);
            var valueSignature = tokens[^1].Trim();

            var enumType = GetEnumType(typeSignature);

            if (enumType == null)
            {
                return new NettleParseException($"{typeSignature} is not a recognized enum type.");
            }

            var success = Enum.TryParse(enumType, valueSignature, true, out object? enumValue);

            if (false == success)
            {
                return new NettleParseException($"{valueSignature} is not a recognized enum value.");
            }

            return enumValue!;
        }

        /// <summary>
        /// Gets an enum type from an enum type name (this can contain the namespace)
        /// </summary>
        /// <param name="enumTypeName">The enum type name</param>
        /// <returns>The enum type, if a match is found; otherwise false</returns>
        private static Type? GetEnumType(string enumTypeName)
        {
            if (String.IsNullOrEmpty(enumTypeName))
            {
                return default;
            }

            var containsNamespace = enumTypeName.Contains('.');
            var comparison = StringComparison.OrdinalIgnoreCase;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type? type;

                if (containsNamespace)
                {
                    type = assembly.GetType(enumTypeName);
                }
                else
                {
                    type = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(enumTypeName, comparison));
                }

                if (type?.IsEnum ?? false)
                {
                    return type;
                }
            }

            return null;
        }
    }
}
