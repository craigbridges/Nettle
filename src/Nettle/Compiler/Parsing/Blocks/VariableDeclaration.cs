namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable declaration code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="VariableName">The variable name</param>
internal record class VariableDeclaration(string Signature, string VariableName) : CodeBlock(Signature)
{
    /// <summary>
    /// Gets or sets the signature of the assigned value
    /// </summary>
    public string? AssignedValueSignature { get; init; }

    /// <summary>
    /// Gets or sets the assigned value
    /// </summary>
    public object? AssignedValue { get; init; }

    /// <summary>
    /// Gets or sets the variables assignment type
    /// </summary>
    public NettleValueType ValueType { get; init; }
}
