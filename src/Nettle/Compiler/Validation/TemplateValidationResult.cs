namespace Nettle.Compiler.Validation
{
    /// <summary>
    /// Represents a template validation result
    /// </summary>
    /// <param name="Template">The template that was validated</param>
    /// <param name="IsValid">is the template valid?</param>
    internal record class TemplateValidationResult(Template Template, bool IsValid)
    {
        /// <summary>
        /// Gets an array of errors that were found
        /// </summary>
        public TemplateValidationError[] Errors { get; init; } = Array.Empty<TemplateValidationError>();
    }
}
