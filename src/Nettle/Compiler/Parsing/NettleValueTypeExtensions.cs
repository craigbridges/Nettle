namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;
using Nettle.Compiler.Parsing.Conditions;

internal static class NettleValueTypeExtensions
{
    /// <summary>
    /// Parses the value signature into the value type specified
    /// </summary>
    /// <param name="type">The value type</param>
    /// <param name="signature">The value signature</param>
    /// <returns>The parsed value</returns>
    public static object? ParseValue(this NettleValueType type, string signature)
    {
        if (String.IsNullOrEmpty(signature))
        {
            return default;
        }
        else
        {
            return type switch
            {
                NettleValueType.String => ParseString(),
                NettleValueType.Number => Double.Parse(signature),
                NettleValueType.Enum => EnumParser.Parse(signature),
                NettleValueType.Boolean => Boolean.Parse(signature),
                NettleValueType.ModelBinding => ParseModelBinding(),
                NettleValueType.Function => ParseFunction(),
                NettleValueType.BooleanExpression => ParseBooleanExpression(),
                NettleValueType.KeyValuePair => KeyValuePairParser.Parse(signature),
                NettleValueType.AnonymousType => AnonymousTypeParser.Parse(signature),
                _ => signature
            };
        }

        string ParseString()
        {
            if (signature.Equals("\"\""))
            {
                return String.Empty;
            }
            else if (signature.StartsWith("\"") && signature.EndsWith("\""))
            {
                return signature.Crop(1, signature.Length - 2);
            }
            else
            {
                return signature;
            }
        }

        string ParseModelBinding()
        {
            var bindingPath = signature;

            if (bindingPath.StartsWith(@"{{") && bindingPath.EndsWith(@"}}"))
            {
                bindingPath = bindingPath.Crop(2, bindingPath.Length - 3);
            }

            if (bindingPath.StartsWith(@"$") && bindingPath.Length > 1)
            {
                // Ensure a property reference isn't being made on the model
                if (false == bindingPath.StartsWith(@"$."))
                {
                    bindingPath = bindingPath.Crop(1);
                }
            }

            return bindingPath;
        }

        CodeBlock ParseFunction()
        {
            var templateContent = new string(signature);
            var positionOffset = default(int);

            return new FunctionParser().Parse(ref templateContent, ref positionOffset, signature);
        }

        BooleanExpression ParseBooleanExpression()
        {
            var expression = signature;

            if (expression.StartsWith(@"(") && expression.EndsWith(@")"))
            {
                expression = expression.Crop(1, expression.Length - 2);
            }

            return new BooleanExpressionParser().Parse(expression);
        }
    }
}
