namespace Nettle.Compiler.Parsing.Blocks;

using Nettle.Compiler.Parsing.Conditions;

/// <summary>
/// Represents a 'while' loop code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="Body">The blocks raw body content (this could be empty, but should never be null)</param>
/// <param name="ConditionExpression">The loops condition expression</param>
internal record class WhileLoop(string Signature, string Body, BooleanExpression ConditionExpression) : NestableCodeBlock(Signature, Body);
