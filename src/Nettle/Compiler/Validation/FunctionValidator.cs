namespace Nettle.Compiler.Validation
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a function code block validator
    /// </summary>
    internal sealed class FunctionValidator : IBlockValidator
    {
        private IFunctionRepository _functionRepository;

        /// <summary>
        /// Constructs the template validator with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public FunctionValidator
            (
                IFunctionRepository functionRepository
            )
        {
            Validate.IsNotNull(functionRepository);

            _functionRepository = functionRepository;
        }

        /// <summary>
        /// Validates the templates function calls
        /// </summary>
        /// <param name="template">The template</param>
        /// <returns>An array of errors</returns>
        /// <remarks>
        /// There are two steps to function validation:
        /// 
        /// 1) Check all function names are valid (i.e. matching function is found)
        /// 2) Check the correct parameter values are supplied
        /// </remarks>
        public TemplateValidationError[] ValidateTemplate
            (
                Template template
            )
        {
            Validate.IsNotNull(template);

            var functionCalls = template.FindBlocks<FunctionCall>();

            if (false == functionCalls.Any())
            {
                return new TemplateValidationError[] { };
            }
            else
            {
                var errors = new List<TemplateValidationError>();

                foreach (var call in functionCalls)
                {
                    var exists = _functionRepository.FunctionExists
                    (
                        call.FunctionName
                    );

                    if (false == exists)
                    {
                        errors.Add
                        (
                            new TemplateValidationError
                            (
                                call,
                                "No function was found with the name '{0}'.".With
                                (
                                    call.FunctionName
                                )
                            )
                        );
                    }
                    else
                    {
                        var function = _functionRepository.GetFunction
                        (
                            call.FunctionName
                        );

                        var parameters = function.GetAllParameters();
                        var values = call.ParameterValues;

                        if (parameters.Count() != values.Count())
                        {
                            var requiredParameters = function.GetRequiredParameters();

                            if (requiredParameters.Count() > values.Count())
                            {
                                errors.Add
                                (
                                    new TemplateValidationError
                                    (
                                        call,
                                        "One or more parameter values are missing for '{0}'.".With
                                        (
                                            call.FunctionName
                                        )
                                    )
                                );
                            }
                        }

                        var counter = 0;

                        if (parameters.Any())
                        {
                            foreach (var value in values)
                            {
                                if (counter < parameters.Count())
                                {
                                    var matchingParameter = parameters.ElementAt
                                    (
                                        counter
                                    );

                                    var acceptsValue = matchingParameter.Accepts
                                    (
                                        value
                                    );

                                    if (false == acceptsValue)
                                    {
                                        errors.Add
                                        (
                                            new TemplateValidationError
                                            (
                                                call,
                                                "The parameter value '{0}' is not valid.".With
                                                (
                                                    call.FunctionName
                                                )
                                            )
                                        );
                                    }
                                }

                                counter++;
                            }
                        }
                    }
                }

                return errors.ToArray();
            }
        }
    }
}
