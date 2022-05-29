namespace Nettle.Compiler.Parsing;

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
    protected static bool IsValidSignatureBody(string signatureBody)
    {
        if (String.IsNullOrWhiteSpace(signatureBody))
        {
            return false;
        }
        else if (signatureBody.StartsWith(' '))
        {
            return false;
        }
        else if (signatureBody.StartsWith('}') || signatureBody.EndsWith('{'))
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
    protected static string UnwrapSignatureBody(string signature)
    {
        string? body;

        if (signature.StartsWith(@"{{") && signature.EndsWith(@"}}"))
        {
            body = signature.Crop(2, signature.Length - 3);
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
    protected NettleValueType ResolveType(string value) => NettleValueResolver.ResolveType(value);

    /// <summary>
    /// Trims the signature from the template content specified
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The signature</param>
    protected static void TrimTemplate(ref string templateContent, ref int positionOffSet, string signature)
    {
        var endPosition = signature.Length;

        positionOffSet += endPosition;
        templateContent = templateContent.Crop(endPosition);
    }
}
