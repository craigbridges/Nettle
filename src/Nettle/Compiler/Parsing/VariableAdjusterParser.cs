namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable adjuster block parser
/// </summary>
internal abstract class VariableAdjusterParser : NettleParser, IBlockParser
{
    /// <summary>
    /// Gets the adjuster operand signature
    /// </summary>
    protected abstract string AdjusterSignature { get; }

    /// <summary>
    /// Determines if a signature matches the block type of the parser
    /// </summary>
    /// <param name="signatureBody">The signature body</param>
    /// <returns>True, if it matches; otherwise false</returns>
    /// <remarks>
    /// The rules for matching a variable adjuster are as follows:
    /// 
    /// - The trimmed signature body must not contain spaces
    /// - The variable name must be valid
    /// - Must end with the adjuster operand signature
    /// </remarks>
    public virtual bool Matches(string signatureBody)
    {
        // Rule: must not contain spaces
        if (signatureBody.Contains(' '))
        {
            return false;
        }

        // Rule: the variable name must be valid
        var variableName = ExtractVariableName(signatureBody);
        var isValidName = VariableParser.IsValidVariableName(variableName);

        if (false == isValidName)
        {
            return false;
        }

        // Rule: must end with the adjuster signature
        var hasAdjuster = signatureBody.EndsWith(AdjusterSignature);

        if (false == hasAdjuster)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Parses the code block signature into a code block object
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The block signature</param>
    /// <returns>The parsed code block</returns>
    public virtual CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
    {
        var signatureBody = UnwrapSignatureBody(signature);
        var startPosition = positionOffSet;
        var endPosition = (startPosition + signature.Length);

        var variableName = ExtractVariableName(signatureBody);

        TrimTemplate(ref templateContent, ref positionOffSet, signature);

        return new VariableAdjuster(signature, variableName)
        {
            StartPosition = startPosition,
            EndPosition = endPosition
        };
    }

    /// <summary>
    /// Extracts the variable name from the signatures body
    /// </summary>
    /// <param name="signatureBody">The signature body</param>
    /// <returns>The variable name</returns>
    protected string ExtractVariableName(string signatureBody) => signatureBody.LeftOf(AdjusterSignature);
}
