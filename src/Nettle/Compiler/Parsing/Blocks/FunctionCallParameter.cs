namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a function call parameter
/// </summary>
/// <param name="ValueSignature">The parameter value signature</param>
/// <param name="Value">The parameter value supplied</param>
/// <param name="Type">The parameter value type</param>
internal record class FunctionCallParameter(string ValueSignature, object? Value, NettleValueType Type);
