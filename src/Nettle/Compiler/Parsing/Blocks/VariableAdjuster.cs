namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable adjuster code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="VariableName">The variable name</param>
internal record class VariableAdjuster(string Signature, string VariableName) : CodeBlock(Signature);
