namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a 'for each' loop code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="Body">The blocks raw body content (this could be empty, but should never be null)</param>
/// <param name="CollectionSignature">The loops collection signature</param>
internal record class ForEachLoop(string Signature, string Body, string CollectionSignature) : NestableCodeBlock(Signature, Body)
{
    /// <summary>
    /// Gets or sets the collections value type
    /// </summary>
    public NettleValueType CollectionType { get; init; }

    /// <summary>
    /// Gets or sets the collections value
    /// </summary>
    public object? CollectionValue { get; init; }
}
