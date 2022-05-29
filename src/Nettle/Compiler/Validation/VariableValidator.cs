namespace Nettle.Compiler.Validation;

using Nettle.Compiler.Parsing;
using Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a template variable code block validator
/// </summary>
internal sealed class VariableValidator : IBlockValidator
{
    /// <summary>
    /// Validates the templates variable usage
    /// </summary>
    /// <param name="template">The template</param>
    /// <returns>An array of errors</returns>
    /// <remarks>
    /// Duplicate variable declarations are checked for first
    /// </remarks>
    public TemplateValidationError[] ValidateTemplate(Template template)
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
            var foundInDuplicates = duplicates.Contains(variableName);

            if (false == foundInDuplicates)
            {
                var matches = allVariables.Where(x => x.VariableName == variableName);
                var isDuplicate = matches.Count() > 1;

                if (isDuplicate)
                {
                    errors.Add
                    (
                        new TemplateValidationError
                        (
                            matches.Last(),
                            $"A variable with the name '{variableName}' has already been defined."
                        )
                    );

                    duplicates.Add(variableName);
                }
            }
        }

        ValidateVariables(ref errors, ref declaredVariables, template.Blocks);

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
                    var variableName = variable.AssignedValueSignature ?? String.Empty;

                    RunVariableCheck(ref errors, declaredVariables, variable, variableName);
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
                        var variableName = parameterValue.ValueSignature;

                        RunVariableCheck(ref errors, declaredVariables, call, variableName);
                    }
                }
            }
            else if (blockType == typeof(ForEachLoop))
            {
                var loop = (ForEachLoop)block;

                if (loop.CollectionType == NettleValueType.Variable)
                {
                    var variableName = loop.CollectionSignature;

                    RunVariableCheck(ref errors, declaredVariables, loop, variableName);
                }

                if (loop.Blocks != null)
                {
                    ValidateVariables(ref errors, ref declaredVariables, loop.Blocks);
                }
            }
            else if (blockType == typeof(IfStatement))
            {
                var statement = (IfStatement)block;
                var expression = statement.ConditionExpression;

                foreach (var condition in expression.Conditions)
                {
                    if (condition.LeftValue.ValueType == NettleValueType.Variable)
                    {
                        var variableName = condition.LeftValue.Signature;

                        RunVariableCheck(ref errors, declaredVariables, statement, variableName);
                    }

                    if (condition.RightValue != null)
                    {
                        if (condition.RightValue.ValueType == NettleValueType.Variable)
                        {
                            var variableName = condition.RightValue.Signature;

                            RunVariableCheck(ref errors, declaredVariables, statement, variableName);
                        }
                    }
                }

                if (statement.Blocks != null)
                {
                    ValidateVariables(ref errors, ref declaredVariables, statement.Blocks);
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
    private static void RunVariableCheck
        (
            ref List<TemplateValidationError> errors,
            List<VariableDeclaration> declaredVariables,
            CodeBlock block,
            string variableName
        )
    {
        var pathInfo = new NettlePath(variableName);

        variableName = pathInfo[0].Name;

        var variableFound = declaredVariables.Any(x => x.VariableName == variableName);

        if (false == variableFound)
        {
            errors.Add
            (
                new TemplateValidationError
                (
                    block,
                    $"No variable was found with the name '{variableName}'."
                )
            );
        }
    }
}
