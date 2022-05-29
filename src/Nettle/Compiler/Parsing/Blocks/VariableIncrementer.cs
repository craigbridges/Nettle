namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable incrementer code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="VariableName">The variable name</param>
internal record class VariableIncrementer(string Signature, string VariableName) : VariableAdjuster(Signature, VariableName);
