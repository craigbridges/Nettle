namespace Nettle.Compiler.Parsing.Conditions;

/// <summary>
/// Represents a boolean condition value
/// </summary>
/// <param name="Signature">The statements conditions signature</param>
/// <param name="ValueType">The conditions value type</param>
/// <param name="Value">The conditions parsed value</param>
internal record class BooleanConditionValue(string Signature, NettleValueType ValueType, object? Value);
