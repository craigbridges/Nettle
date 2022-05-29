namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;
using Nettle.Compiler.Parsing.Conditions;

/// <summary>
/// Represents a 'while' loop code block parser
/// </summary>
internal sealed class WhileLoopParser : NestedBlockParser
{
    private readonly BooleanExpressionParser _expressionParser;

    public WhileLoopParser(IBlockifier blockifier) : base(blockifier)
    {
        _expressionParser = new BooleanExpressionParser();
    }

    /// <summary>
    /// Gets the tag name
    /// </summary>
    protected override string TagName => "while";

    /// <summary>
    /// Parses the 'while' loop signature into a code block object
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The loop signature</param>
    /// <returns>The parsed while loop</returns>
    public override CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
    {
        var signatureBody = UnwrapSignatureBody(signature);
        var conditionSignature = signatureBody.RightOf($"{TagName} ");

        if (String.IsNullOrWhiteSpace(conditionSignature))
        {
            throw new NettleParseException
            (
                "The while loops condition must be specified.",
                positionOffSet
            );
        }

        var expression = _expressionParser.Parse(conditionSignature);

        var nestedBody = ExtractNestedBody(ref templateContent, ref positionOffSet, signature);

        return new WhileLoop(nestedBody.Signature, nestedBody.Body, expression)
        {
            StartPosition = nestedBody.StartPosition,
            EndPosition = nestedBody.EndPosition,
            Blocks = nestedBody.Blocks
        };
    }
}
