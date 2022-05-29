namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable decrementer code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="VariableName">The variable name</param>
internal record class VariableDecrementer(string Signature, string VariableName) : VariableAdjuster(Signature, VariableName);
