namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents an unresolved anonymous type code block
/// </summary>
internal record class UnresolvedAnonymousType(string Signature) : CodeBlock(Signature)
{
    /// <summary>
    /// Gets an array of properties contained in the type
    /// </summary>
    public UnresolvedAnonymousTypeProperty[] Properties { get; init; } = Array.Empty<UnresolvedAnonymousTypeProperty>();
}
