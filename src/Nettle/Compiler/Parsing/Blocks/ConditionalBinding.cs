namespace Nettle.Compiler.Parsing.Blocks;

using Nettle.Compiler.Parsing.Conditions;

/// <summary>
/// Represents a conditional binding code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="ConditionExpression">The conditions parsed expression</param>
internal record class ConditionalBinding(string Signature, BooleanExpression ConditionExpression) : CodeBlock(Signature)
{
    /// <summary>
    /// Gets or sets the signature of the true value
    /// </summary>
    public string TrueValueSignature { get; init; } = default!;

    /// <summary>
    /// Gets or sets the true value
    /// </summary>
    public object? TrueValue { get; init; }

    /// <summary>
    /// Gets or sets the true value type
    /// </summary>
    public NettleValueType TrueValueType { get; init; }

    /// <summary>
    /// Gets or sets the signature of the false value
    /// </summary>
    public string FalseValueSignature { get; init; } = default!;

    /// <summary>
    /// Gets or sets the false value
    /// </summary>
    public object? FalseValue { get; init; }

    /// <summary>
    /// Gets or sets the false value type
    /// </summary>
    public NettleValueType FalseValueType { get; init; }
}
