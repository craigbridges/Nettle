namespace Nettle.Compiler.Parsing
{
    using System;

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
        public NettleValueType ResolveType
            (
                string value
            )
        {
            var type = default(NettleValueType);

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
            else
            {
                // Decide if the value looks like a number or variable
                if (value.IsNumeric())
                {
                    type = NettleValueType.Number;
                }
                else if (value.ToLower() == "true" || value.ToLower() == "false")
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
