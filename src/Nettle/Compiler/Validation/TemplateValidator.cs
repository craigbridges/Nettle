namespace Nettle.Compiler.Validation
{
    internal sealed class TemplateValidator : ITemplateValidator
    {
        private readonly VariableValidator _variableValidator;
        private readonly ForLoopValidator _forLoopValidator;
        private readonly FunctionValidator _functionValidator;

        public TemplateValidator(IFunctionRepository functionRepository)
        {
            Validate.IsNotNull(functionRepository);
            
            _variableValidator = new VariableValidator();
            _forLoopValidator = new ForLoopValidator();
            _functionValidator = new FunctionValidator(functionRepository);
        }

        public TemplateValidationResult ValidateTemplate(Template template)
        {
            Validate.IsNotNull(template);

            var ignoreErrors = template.IsFlagSet(TemplateFlag.IgnoreErrors);

            if (ignoreErrors)
            {
                return new TemplateValidationResult(template, true);
            }
            else
            {
                var isValid = true;
                var allErrors = new List<TemplateValidationError>();

                var variableErrors = _variableValidator.ValidateTemplate(template);
                var functionErrors = _functionValidator.ValidateTemplate(template);
                var loopErrors = _forLoopValidator.ValidateTemplate(template);

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

                return new TemplateValidationResult(template, isValid)
                {
                    Errors = allErrors.ToArray()
                };
            }
        }
    }
}
