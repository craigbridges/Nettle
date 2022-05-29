namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a render partial code block
    /// </summary>
    /// <param name="Signature">The blocks signature</param>
    /// <param name="TemplateName">The template name</param>
    internal record class RenderPartial(string Signature, string TemplateName) : CodeBlock(Signature)
    {
        /// <summary>
        /// Gets or sets the model signature
        /// </summary>
        public string? ModelSignature { get; init; }

        /// <summary>
        /// Gets or sets the models value type
        /// </summary>
        public NettleValueType? ModelType { get; init; }

        /// <summary>
        /// Gets or sets the models value
        /// </summary>
        public object? ModelValue { get; init; }
    }
}
