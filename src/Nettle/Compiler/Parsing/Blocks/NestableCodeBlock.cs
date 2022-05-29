namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a nestable code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="Body">The blocks raw body content (this could be empty, but should never be null)</param>
internal record class NestableCodeBlock(string Signature, string Body) : CodeBlock(Signature)
{
    /// <summary>
    /// Gets or sets the nested code blocks
    /// </summary>
    public CodeBlock[] Blocks { get; init; } = Array.Empty<CodeBlock>();
}
