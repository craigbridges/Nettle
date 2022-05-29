namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a for each loop code block parser
/// </summary>
internal sealed class ForEachLoopParser : NestedBlockParser
{
    public ForEachLoopParser(IBlockifier blockifier)
        : base(blockifier)
    { }

    /// <summary>
    /// Gets the open tag name
    /// </summary>
    protected override string TagName => "each";

    /// <summary>
    /// Extracts the 'for each' signature into a code block object
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The variable signature</param>
    /// <returns>The parsed for each code block</returns>
    public override CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
    {
        var forLoop = UnwrapSignatureBody(signature);
        var collectionSignature = forLoop.RightOf($"{TagName} ");

        if (String.IsNullOrWhiteSpace(collectionSignature))
        {
            throw new NettleParseException
            (
                "The loops collection name must be specified.",
                positionOffSet
            );
        }

        var collectionType = ResolveType(collectionSignature);
        var collectionValue = collectionType.ParseValue(collectionSignature);

        var nestedBody = ExtractNestedBody(ref templateContent, ref positionOffSet, signature);

        return new ForEachLoop(nestedBody.Signature, nestedBody.Body, collectionSignature)
        {
            StartPosition = nestedBody.StartPosition,
            EndPosition = nestedBody.EndPosition,
            CollectionType = collectionType,
            CollectionValue = collectionValue,
            Blocks = nestedBody.Blocks
        };
    }
}
