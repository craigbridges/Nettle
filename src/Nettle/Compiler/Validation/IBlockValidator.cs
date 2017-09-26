namespace Nettle.Compiler.Validation
{
    /// <summary>
    /// Defines a contract for a code block validator
    /// </summary>
    internal interface IBlockValidator
    {
        /// <summary>
        /// Validates the template specified
        /// </summary>
        /// <param name="template">The template to validate</param>
        /// <returns>The validation errors</returns>
        TemplateValidationError[] ValidateTemplate
        (
            Template template
        );
    }
}
