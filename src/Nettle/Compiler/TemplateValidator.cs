namespace Nettle.Compiler
{
    using Nettle.Compiler.Parsing;
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the default implementation of a template validator
    /// </summary>
    internal sealed class TemplateValidator : ITemplateValidator
    {
        private IFunctionRepository _functionRepository;

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

            _functionRepository = functionRepository;
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

            var variableErrors = ValidateVariables(template);
            var functionErrors = ValidateFunctions(template);
            var loopErrors = ValidateForLoops(template);

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
        
        /// <summary>
        /// Validates the templates variable usage
        /// </summary>
        /// <param name="template">The template</param>
        /// <returns>An array of errors</returns>
        /// <remarks>
        /// Duplicate variable declarations are checked for first
        /// </remarks>
        private TemplateValidationError[] ValidateVariables
            (
                Template template
            )
        {
            Validate.IsNotNull(template);

            var errors = new List<TemplateValidationError>();
            var declaredVariables = new List<VariableDeclaration>();
            var allVariables = template.FindBlocks<VariableDeclaration>();
            var duplicates = new List<string>();

            // Check for duplicate variable declarations
            foreach (var variable in allVariables)
            {
                var variableName = variable.VariableName;

                var foundInDuplicates = duplicates.Contains
                (
                    variableName
                );

                if (false == foundInDuplicates)
                {
                    var matches = allVariables.Where
                    (
                        v => v.VariableName == variableName
                    );

                    var isDuplicate = matches.Count() > 1;

                    if (isDuplicate)
                    {
                        errors.Add
                        (
                            new TemplateValidationError
                            (
                                matches.Last(),
                                "A variable with the name '{0}' has already been defined.".With
                                (
                                    variableName
                                )
                            )
                        );

                        duplicates.Add(variableName);
                    }
                }
            }

            ValidateVariables
            (
                ref errors,
                ref declaredVariables,
                template.Blocks
            );

            return errors.ToArray();
        }

        /// <summary>
        /// Validates the templates variable usage
        /// </summary>
        /// <param name="errors">The current error list</param>
        /// <param name="declaredVariables">The declared variables</param>
        /// <param name="blocks">An array of code blocks</param>
        /// <remarks>
        /// Each block is checked in sequence to find references to variables.
        /// 
        /// When a reference to a variable is found, a check is made to ensure
        /// the variable has previously been declared.
        /// 
        /// If the variable has not been declared, then it is invalid.
        /// </remarks>
        private void ValidateVariables
            (
                ref List<TemplateValidationError> errors,
                ref List<VariableDeclaration> declaredVariables,
                CodeBlock[] blocks
            )
        {
            foreach (var block in blocks)
            {
                var blockType = block.GetType();

                if (blockType == typeof(VariableDeclaration))
                {
                    var variable = (VariableDeclaration)block;

                    if (variable.ValueType == NettleValueType.Variable)
                    {
                        RunVariableCheck
                        (
                            ref errors,
                            declaredVariables,
                            variable,
                            variable.AssignedValueSignature
                        );
                    }

                    // Build a list of declared variables as we go so we can 
                    // check against that for anything referencing variable
                    declaredVariables.Add(variable);
                }
                else if (blockType == typeof(FunctionCall))
                {
                    var call = (FunctionCall)block;

                    foreach (var parameterValue in call.ParameterValues)
                    {
                        if (parameterValue.Type == NettleValueType.Variable)
                        {
                            RunVariableCheck
                            (
                                ref errors,
                                declaredVariables,
                                call,
                                parameterValue.ValueSignature
                            );
                        }
                    }
                }
                else if (blockType == typeof(ForEachLoop))
                {
                    var loop = (ForEachLoop)block;

                    if (loop.CollectionType == NettleValueType.Variable)
                    {
                        RunVariableCheck
                        (
                            ref errors,
                            declaredVariables,
                            loop,
                            loop.CollectionName
                        );
                    }

                    if (loop.Blocks != null)
                    {
                        ValidateVariables
                        (
                            ref errors,
                            ref declaredVariables,
                            loop.Blocks
                        );
                    }
                }
                else if (blockType == typeof(IfStatement))
                {
                    var statement = (IfStatement)block;

                    RunVariableCheck
                    (
                        ref errors,
                        declaredVariables,
                        statement,
                        statement.ConditionName
                    );

                    if (statement.Blocks != null)
                    {
                        ValidateVariables
                        (
                            ref errors,
                            ref declaredVariables,
                            statement.Blocks
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Runs an existential check to ensure a matching declared variable was found
        /// </summary>
        /// <param name="errors">The current error list</param>
        /// <param name="declaredVariables">The declared variables</param>
        /// <param name="block">The current code block</param>
        /// <param name="variableName">The variable name</param>
        private void RunVariableCheck
            (
                ref List<TemplateValidationError> errors,
                List<VariableDeclaration> declaredVariables,
                CodeBlock block,
                string variableName
            )
        {
            var variableFound = declaredVariables.Any
            (
                v => v.VariableName == variableName
            );

            if (false == variableFound)
            {
                errors.Add
                (
                    new TemplateValidationError
                    (
                        block,
                        "No variable was found with the name '{0}'.".With
                        (
                            variableName
                        )
                    )
                );
            }
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
        private TemplateValidationError[] ValidateFunctions
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

                        foreach (var value in values)
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

                            counter++;
                        }
                    }
                }

                return errors.ToArray();
            }
        }

        /// <summary>
        /// Validates the templates for each loops
        /// </summary>
        /// <param name="template">The template</param>
        /// <returns>An array of errors</returns>
        private TemplateValidationError[] ValidateForLoops
            (
                Template template
            )
        {
            Validate.IsNotNull(template);

            var loops = template.FindBlocks<ForEachLoop>();

            if (false == loops.Any())
            {
                return new TemplateValidationError[] { };
            }
            else
            {
                var errors = new List<TemplateValidationError>();

                foreach (var loop in loops)
                {
                    switch (loop.CollectionType)
                    {
                        case NettleValueType.Number:
                        case NettleValueType.Boolean:

                            errors.Add
                            (
                                new TemplateValidationError
                                (
                                    loop,
                                    "Invalid for each loop collection type."
                                )
                            );

                            break;
                    }
                }

                return errors.ToArray();
            }
        }
    }
}
