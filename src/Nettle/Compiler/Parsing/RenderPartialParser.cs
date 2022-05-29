namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a render partial code block parser
/// </summary>
internal sealed class RenderPartialParser : NettleParser, IBlockParser
{
    /// <summary>
    /// Determines if a signature matches the block type of the parser
    /// </summary>
    /// <param name="signatureBody">The signature body</param>
    /// <returns>True, if it matches; otherwise false</returns>
    public bool Matches(string signatureBody) => signatureBody.StartsWith(@">");

    /// <summary>
    /// Parses the code block signature into a code block object
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The block signature</param>
    /// <returns>The parsed code block</returns>
    public CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
    {
        var signatureBody = UnwrapSignatureBody(signature);

        signatureBody = signatureBody.RightOf(@">").Replace("  ", " ");

        if (String.IsNullOrWhiteSpace(signatureBody))
        {
            throw new NettleParseException("The template name must be specified.", positionOffSet);
        }

        var parts = signatureBody.Trim().Split(' ');
        var templateName = parts[0];
        var modelSignature = default(string);
        var modelType = default(NettleValueType?);
        var modelValue = default(object);

        if (parts.Length > 1)
        {
            modelSignature = parts[1];
            modelType = ResolveType(modelSignature);

            modelValue = modelType.Value.ParseValue(modelSignature);
        }

        var startPosition = positionOffSet;
        var endPosition = (startPosition + signature.Length);

        TrimTemplate(ref templateContent, ref positionOffSet, signature);

        return new RenderPartial(signature, templateName)
        {
            StartPosition = startPosition,
            EndPosition = endPosition,
            ModelSignature = modelSignature,
            ModelType = modelType,
            ModelValue = modelValue
        };
    }
}
