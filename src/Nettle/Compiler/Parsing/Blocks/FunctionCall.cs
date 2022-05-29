namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a model binding code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="FunctionName">The name of the function being called</param>
internal record class FunctionCall(string Signature, string FunctionName) : CodeBlock(Signature)
{
    /// <summary>
    /// Gets or sets an array of the parameters supplied
    /// </summary>
    public FunctionCallParameter[] ParameterValues { get; init; } = Array.Empty<FunctionCallParameter>();
}
