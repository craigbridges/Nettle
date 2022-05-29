namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents a Nettle value type resolver
    /// </summary>
    internal sealed class NettleValueResolver
    {
        /// <summary>
        /// Resolves the value type from its string representation
        /// </summary>
        /// <param name="value">The raw value</param>
        /// <returns>The value type resolved</returns>
        public static NettleValueType ResolveType(string value)
        {
            NettleValueType type;

            if (String.IsNullOrWhiteSpace(value))
            {
                type = NettleValueType.String;
            }
            else if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                type = NettleValueType.String;
            }
            else if (value.StartsWith(@"{{") && value.EndsWith(@"}}"))
            {
                type = NettleValueType.ModelBinding;
            }
            else if (value.StartsWith(@"$"))
            {
                type = NettleValueType.ModelBinding;
            }
            else if (value.StartsWith("@"))
            {
                type = NettleValueType.Function;
            }
            else if (value.StartsWith("(") && value.EndsWith(")"))
            {
                type = NettleValueType.BooleanExpression;
            }
            else if (value.StartsWith("<") && value.EndsWith(">"))
            {
                type = NettleValueType.KeyValuePair;
            }
            else if (value.StartsWith("[") && value.EndsWith("]"))
            {
                type = NettleValueType.AnonymousType;
            }
            else
            {
                // Decide if the value looks like a number or variable
                if (value.IsNumeric())
                {
                    type = NettleValueType.Number;
                }
                else if (value.Equals("true", StringComparison.OrdinalIgnoreCase) || value.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    type = NettleValueType.Boolean;
                }
                else
                {
                    type = NettleValueType.Variable;
                }
            }

            return type;
        }
    }
}
