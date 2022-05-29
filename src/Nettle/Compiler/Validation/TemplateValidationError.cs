namespace Nettle.Compiler.Validation
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a template validation error
    /// </summary>
    /// <param name="Block">The invalid code block</param>
    /// <param name="Message">The error message</param>
    internal record class TemplateValidationError(CodeBlock Block, string Message);
}
