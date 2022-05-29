namespace Nettle.Compiler.Parsing.Blocks;

using Nettle.Compiler.Parsing.Conditions;

/// <summary>
/// Represents an 'if' statement code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="Body">The blocks raw body content (this could be empty, but should never be null)</param>
/// <param name="ConditionExpression">The conditions expression</param>
internal record class IfStatement(string Signature, string Body, BooleanExpression ConditionExpression) : NestableCodeBlock(Signature, Body)
{
    /// <summary>
    /// Gets or sets a list of else if conditions
    /// </summary>
    public ElseIfStatement[] ElseIfConditions { get; init; } = Array.Empty<ElseIfStatement>();

    /// <summary>
    /// Gets or sets the else content
    /// </summary>
    public NestableCodeBlock? ElseContent { get; init; }
}
