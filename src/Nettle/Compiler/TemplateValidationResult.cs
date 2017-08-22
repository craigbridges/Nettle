namespace Nettle.Compiler
{
    using Nettle.Compiler.Parsing;

    /// <summary>
    /// Represents a template validation result
    /// </summary>
    internal class TemplateValidationResult
    {
        /// <summary>
        /// Constructs the validation result with dependencies
        /// </summary>
        /// <param name="template">The template</param>
        /// <param name="isValid">A flag indicating if the template is valid</param>
        /// <param name="errors">The validation errors</param>
        public TemplateValidationResult
            (
                Template template,
                bool isValid,
                params TemplateValidationError[] errors
            )
        {
            Validate.IsNotNull(template);

            this.Template = template;
            this.IsValid = isValid;

            if (errors == null)
            {
                this.Errors = new TemplateValidationError[] { };
            }
            else
            {
                this.Errors = errors;
            }
        }

        /// <summary>
        /// Gets the template that was validated
        /// </summary>
        public Template Template { get; private set; }

        /// <summary>
        /// Gets a flag indicating if the template is valid
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets an array of errors
        /// </summary>
        public TemplateValidationError[] Errors { get; private set; }
    }
}
