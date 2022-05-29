namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents an unresolved anonymous type property code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="Name">The property name</param>
internal record class UnresolvedAnonymousTypeProperty(string Signature, string Name) : CodeBlock(Signature)
{
    /// <summary>
    /// Gets or sets the value type
    /// </summary>
    public NettleValueType ValueType { get; init; }

    /// <summary>
    /// Gets or sets the raw value
    /// </summary>
    public object? RawValue { get; init; }
}
