namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable reassignment code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="VariableName">The variable name</param>
internal record class VariableReassignment(string Signature, string VariableName) : VariableDeclaration(Signature, VariableName);
