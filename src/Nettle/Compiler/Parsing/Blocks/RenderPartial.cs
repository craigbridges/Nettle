namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents a render partial code block
    /// </summary>
    internal class RenderPartial : CodeBlock
    {
        /// <summary>
        /// Gets or sets the template name
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets the model signature
        /// </summary>
        public string ModelSignature { get; set; }

        /// <summary>
        /// Gets or sets the models value type
        /// </summary>
        public NettleValueType? ModelType { get; set; }

        /// <summary>
        /// Gets or sets the models value
        /// </summary>
        public object ModelValue { get; set; }
    }
}
