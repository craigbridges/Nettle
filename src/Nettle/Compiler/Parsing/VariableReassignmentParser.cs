namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable reassignment code block parser
/// </summary>
internal sealed class VariableReassignmentParser : VariableParser
{
    /// <summary>
    /// Gets the signature bodies prefix value
    /// </summary>
    protected override string Prefix => "reassign ";

    /// <summary>
    /// Parses the code block signature into a code block object
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The block signature</param>
    /// <returns>The parsed code block</returns>
    public override CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
    {
        var declaration = (VariableDeclaration)base.Parse(ref templateContent, ref positionOffSet, signature);

        return new VariableReassignment(declaration.Signature, declaration.VariableName)
        {
            StartPosition = declaration.StartPosition,
            EndPosition = declaration.EndPosition,
            AssignedValueSignature = declaration.AssignedValueSignature,
            AssignedValue = declaration.AssignedValue,
            ValueType = declaration.ValueType
        };
    }
}
