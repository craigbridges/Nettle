namespace Nettle.Compiler.Validation
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a template validation error
    /// </summary>
    internal class TemplateValidationError
    {
        /// <summary>
        /// Constructs the error with the block and message
        /// </summary>
        /// <param name="block">The code block</param>
        /// <param name="message">The error message</param>
        public TemplateValidationError
            (
                CodeBlock block,
                string message
            )
        {
            Validate.IsNotNull(block);

            this.Block = block;
            this.Message = message;
        }

        /// <summary>
        /// Gets the code block
        /// </summary>
        public CodeBlock Block { get; private set; }

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Message { get; private set; }
    }
}
