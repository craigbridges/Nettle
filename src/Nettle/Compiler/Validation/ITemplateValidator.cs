namespace Nettle.Compiler.Validation
{
    /// <summary>
    /// Defines a contract for a template validator
    /// </summary>
    /// <remarks>
    /// The template validator is responsible for checking the semantics
    /// of each code block as well as the validity of function calls.
    /// </remarks>
    internal interface ITemplateValidator
    {
        /// <summary>
        /// Validates the template specified
        /// </summary>
        /// <param name="template">The template</param>
        /// <returns>The validation result</returns>
        TemplateValidationResult ValidateTemplate(Template template);
    }
}
