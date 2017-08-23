namespace Nettle.Compiler.Parsing
{
    using System;

    /// <summary>
    /// Represents the base class for all Nettle parsers
    /// </summary>
    internal abstract class NettleParser
    {
        /// <summary>
        /// Determines if a code blocks signature body is valid
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <returns>True, if the body is valid; otherwise false</returns>
        protected bool IsValidSignatureBody
            (
                string signatureBody
            )
        {
            if (String.IsNullOrWhiteSpace(signatureBody))
            {
                return false;
            }
            else if (signatureBody.StartsWith(@"}") || signatureBody.EndsWith(@"{"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Unwraps the body from a code block signature
        /// </summary>
        /// <param name="signature">The signature</param>
        /// <returns>The signatures body</returns>
        protected string UnwrapSignatureBody
            (
                string signature
            )
        {
            var body = String.Empty;

            if (signature.StartsWith(@"{{") && signature.EndsWith(@"}}"))
            {
                body = signature.Substring
                (
                    2,
                    signature.Length - 2
                );
            }
            else
            {
                body = signature;
            }

            return body.Trim();
        }

        /// <summary>
        /// Resolves the value type from its string representation
        /// </summary>
        /// <param name="value">The raw value</param>
        /// <returns>The value type resolved</returns>
        protected NettleValueType ResolveType
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
            else if (value.StartsWith("@"))
            {
                type = NettleValueType.Function;
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

        /// <summary>
        /// Converts the value signature to the value type specified
        /// </summary>
        /// <param name="signature">The value signature</param>
        /// <param name="type">The value type</param>
        /// <returns>The converted value</returns>
        protected object ConvertValue
            (
                string signature,
                NettleValueType type
            )
        {
            var convertedValue = default(object);

            switch (type)
            {
                case NettleValueType.String:

                    if (signature.StartsWith("\"") && signature.EndsWith("\""))
                    {
                        convertedValue = signature.Crop
                        (
                            1,
                            signature.Length - 2
                        );
                    }
                    else
                    {
                        convertedValue = signature;
                    }

                    break;

                case NettleValueType.Number:

                    convertedValue = Double.Parse(signature);
                    break;

                case NettleValueType.Boolean:

                    convertedValue = Boolean.Parse(signature);
                    break;

                case NettleValueType.ModelBinding:

                    if (signature.StartsWith(@"{{") && signature.EndsWith(@"}}"))
                    {
                        convertedValue = signature.Crop
                        (
                            2,
                            signature.Length - 3
                        );
                    }
                    else
                    {
                        convertedValue = signature;
                    }

                    break;

                case NettleValueType.Variable:

                    // NOTE: this isn't resolvable until runtime
                    break;

                case NettleValueType.Function:

                    // TODO: find a clean way of parsing the function
                    break;
            }

            return convertedValue;
        }

        /// <summary>
        /// Trims the signature from the template content specified
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The signature</param>
        protected void TrimTemplate
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var endPosition = signature.Length - 1;

            positionOffSet += endPosition;

            templateContent = templateContent.Crop
            (
                endPosition
            );
        }
    }
}
