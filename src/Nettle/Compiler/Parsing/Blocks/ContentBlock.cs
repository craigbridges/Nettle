namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a content code block
    /// </summary>
    internal record class ContentBlock(string Signature) : CodeBlock(Signature)
    {

    }
}
