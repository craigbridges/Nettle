namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a flag declaration code block parser
/// </summary>
internal class FlagParser : NettleParser, IBlockParser
{
    /// <summary>
    /// Gets the signature bodies prefix value
    /// </summary>
    protected virtual string Prefix => "#";

    /// <summary>
    /// Determines if a signature matches the block type of the parser
    /// </summary>
    /// <param name="signatureBody">The signature body</param>
    /// <returns>True, if it matches; otherwise false</returns>
    public virtual bool Matches(string signatureBody) => signatureBody.StartsWith(Prefix);

    /// <summary>
    /// Parses the code block signature into a code block object
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The block signature</param>
    /// <returns>The parsed code block</returns>
    public virtual CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
    {
        var body = UnwrapSignatureBody(signature);
        var nameIndex = this.Prefix.Length;
        var flagName = body.Crop(nameIndex).Trim();

        if (String.IsNullOrEmpty(flagName))
        {
            throw new NettleParseException
            (
                "The flag name must be specified.",
                positionOffSet
            );
        }

        var startPosition = positionOffSet;
        var endPosition = (startPosition + signature.Length);

        TrimTemplate(ref templateContent, ref positionOffSet, signature);

        var declaration = new FlagDeclaration(signature, flagName)
        {
            StartPosition = startPosition,
            EndPosition = endPosition
        };

        return declaration;
    }
}
