namespace Nettle.Compiler.Validation
{
    using Nettle.Functions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the default implementation of a template validator
    /// </summary>
    internal sealed class TemplateValidator : ITemplateValidator
    {
        private VariableValidator _variableValidator;
        private ForLoopValidator _forLoopValidator;
        private FunctionValidator _functionValidator;

        /// <summary>
        /// Constructs the template validator with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public TemplateValidator
            (
                IFunctionRepository functionRepository
            )
        {
            Validate.IsNotNull(functionRepository);
            
            _variableValidator = new VariableValidator();
            _forLoopValidator = new ForLoopValidator();

            _functionValidator = new FunctionValidator
            (
                functionRepository
            );
        }

        /// <summary>
        /// Validates the template specified
        /// </summary>
        /// <param name="template">The template</param>
        /// <returns>The validation result</returns>
        public TemplateValidationResult ValidateTemplate
            (
                Template template
            )
        {
            Validate.IsNotNull(template);

            var isValid = true;
            var allErrors = new List<TemplateValidationError>();

            var variableErrors = _variableValidator.ValidateTemplate
            (
                template
            );

            var functionErrors = _functionValidator.ValidateTemplate
            (
                template
            );

            var loopErrors = _forLoopValidator.ValidateTemplate
            (
                template
            );

            if (variableErrors.Any())
            {
                allErrors.AddRange(variableErrors);
                isValid = false;
            }

            if (functionErrors.Any())
            {
                allErrors.AddRange(functionErrors);
                isValid = false;
            }

            if (loopErrors.Any())
            {
                allErrors.AddRange(loopErrors);
                isValid = false;
            }
            
            return new TemplateValidationResult
            (
                template,
                isValid,
                allErrors.ToArray()
            );
        }
    }
}
