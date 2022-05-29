namespace Nettle.Compiler.Parsing.Blocks;

using Nettle.Compiler.Parsing.Conditions;

/// <summary>
/// Represents an 'else if' statement code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="Body">The blocks raw body content (this could be empty, but should never be null)</param>
/// <param name="ConditionExpression">The conditions expression</param>
internal record class ElseIfStatement(string Signature, string Body, BooleanExpression ConditionExpression) : NestableCodeBlock(Signature, Body);
