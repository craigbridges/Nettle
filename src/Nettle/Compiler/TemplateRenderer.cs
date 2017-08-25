namespace Nettle.Compiler
{
    using Nettle.Compiler.Parsing;
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents the default implementation of a template renderer
    /// </summary>
    internal class TemplateRenderer : ITemplateRenderer
    {
        private IFunctionRepository _functionRepository;

        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public TemplateRenderer
            (
                IFunctionRepository functionRepository
            )
        {
            Validate.IsNotNull(functionRepository);

            _functionRepository = functionRepository;
        }

        /// <summary>
        /// Renders a Nettle template with the model data specified
        /// </summary>
        /// <param name="template">The template</param>
        /// <param name="model">The model data</param>
        /// <returns>The rendered template</returns>
        public string Render
            (
                Template template,
                object model
            )
        {
            Validate.IsNotNull(template);

            var context = new TemplateContext(model);
            var blocks = template.Blocks;

            return RenderBlocks
            (
                ref context,
                blocks
            );
        }

        /// <summary>
        /// Renders an array of code blocks to a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="blocks">An array of blocks to render</param>
        /// <returns>The rendered code blocks</returns>
        private string RenderBlocks
            (
                ref TemplateContext context,
                CodeBlock[] blocks
            )
        {
            var builder = new StringBuilder();

            foreach (var block in blocks)
            {
                var blockType = block.GetType();
                var blockOutput = String.Empty;

                if (blockType == typeof(ContentBlock))
                {
                    blockOutput = block.Signature;
                }
                else if (blockType == typeof(ModelBinding))
                {
                    blockOutput = RenderBinding
                    (
                        ref context,
                        (ModelBinding)block
                    );
                }
                else if (blockType == typeof(FunctionCall))
                {
                    blockOutput = RenderFunction
                    (
                        ref context,
                        (FunctionCall)block
                    );
                }
                else if (blockType == typeof(VariableDeclaration))
                {
                    DefineVariable
                    (
                        ref context,
                        (VariableDeclaration)block
                    );
                }
                else if (blockType == typeof(ForEachLoop))
                {
                    blockOutput = RenderForLoop
                    (
                        ref context,
                        (ForEachLoop)block
                    );
                }
                else if (blockType == typeof(IfStatement))
                {
                    blockOutput = RenderIfStatement
                    (
                        ref context,
                        (IfStatement)block
                    );
                }

                builder.Append(blockOutput);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Resolves a value by converting it to the Nettle value type specified
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="rawValue">The raw value</param>
        /// <param name="type">The value type</param>
        /// <param name="parsedFunction">The parsed function (optional)</param>
        /// <returns>The resolved value</returns>
        private object ResolveValue
            (
                ref TemplateContext context,
                object rawValue,
                NettleValueType type
            )
        {
            if (rawValue == null)
            {
                return null;
            }

            var resolvedValue = default(object);

            switch (type)
            {
                case NettleValueType.ModelBinding:

                    var bindingName = rawValue.ToString();

                    resolvedValue = ResolveBindingValue
                    (
                        ref context,
                        bindingName
                    );

                    break;

                case NettleValueType.Function:

                    if (rawValue != null && rawValue is FunctionCall)
                    {
                        var parsedFunction = (FunctionCall)rawValue;

                        var result = ExecuteFunction
                        (
                            ref context,
                            parsedFunction
                        );

                        resolvedValue = result.Output;
                        break;
                    }
                    else
                    {
                        throw new NettleRenderException
                        (
                            "The function call is invalid."
                        );
                    }

                case NettleValueType.Variable:

                    resolvedValue = context.ResolveVariableValue
                    (
                        rawValue.ToString()
                    );

                    break;

                default:

                    resolvedValue = rawValue;
                    break;
            }

            return resolvedValue;
        }

        /// <summary>
        /// Defines a variable in the template context by initialising it
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="variable">The variable code block</param>
        private void DefineVariable
            (
                ref TemplateContext context,
                VariableDeclaration variable
            )
        {
            Validate.IsNotNull(variable);

            var value = ResolveValue
            (
                ref context,
                variable.AssignedValue,
                variable.ValueType
            );

            var variableName = variable.VariableName;

            context.DefineVariable
            (
                variableName,
                value
            );
        }

        /// <summary>
        /// Renders a binding code block into a string using the template context
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="binding">The binding code block</param>
        /// <returns>The rendered block</returns>
        private string RenderBinding
            (
                ref TemplateContext context,
                ModelBinding binding
            )
        {
            Validate.IsNotNull(binding);

            var value = ResolveBindingValue
            (
                ref context,
                binding.BindingPath
            );

            return ToString(value);
        }

        /// <summary>
        /// Resolves a model binding value from the context and binding path
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="bindingPath">The bindings path</param>
        /// <returns>The model bindings value</returns>
        /// <remarks>
        /// A check is made to see if the binding path refers to a variable.
        /// If not variable is found then it is assumed to be a property.
        /// </remarks>
        private object ResolveBindingValue
            (
                ref TemplateContext context,
                string bindingPath
            )
        {
            Validate.IsNotEmpty(bindingPath);

            var name = bindingPath;

            var isNested = TemplateContext.IsNested
            (
                bindingPath
            );

            // Extract the name of the root variable or property
            if (isNested)
            {
                var pathCopy = String.Copy(bindingPath);

                name = TemplateContext.ExtractNextSegment
                (
                    ref pathCopy
                );
            }

            var variableFound = context.Variables.ContainsKey
            (
                name
            );

            // Check if it's a variable or property
            if (variableFound)
            {
                return context.ResolveVariableValue
                (
                    bindingPath
                );
            }
            else
            {
                return context.ResolvePropertyValue
                (
                    bindingPath
                );
            }
        }

        /// <summary>
        /// Renders a function code block into a string using the template context
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="call">The function call code block</param>
        /// <returns>The rendered block</returns>
        private string RenderFunction
            (
                ref TemplateContext context,
                FunctionCall call
            )
        {
            Validate.IsNotNull(call);

            var result = ExecuteFunction
            (
                ref context,
                call
            );

            return ToString
            (
                result.Output
            );
        }

        /// <summary>
        /// Executes a function call using the template context specified
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="call">The function call code block</param>
        /// <returns>The function execution result</returns>
        private FunctionExecutionResult ExecuteFunction
            (
                ref TemplateContext context,
                FunctionCall call
            )
        {
            Validate.IsNotNull(call);

            var function = _functionRepository.GetFunction
            (
                call.FunctionName
            );

            var parameterValues = ResolveParameterValues
            (
                ref context,
                call
            );

            var result = function.Execute
            (
                context,
                parameterValues
            );

            return result;
        }

        /// <summary>
        /// Resolves the parameter values for a function call
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="call">The function call</param>
        /// <returns>An array of resolved parameter values</returns>
        private object[] ResolveParameterValues
            (
                ref TemplateContext context,
                FunctionCall call
            )
        {
            Validate.IsNotNull(call);

            var parameterValues = new List<object>();

            foreach (var parameter in call.ParameterValues)
            {
                var value = ResolveValue
                (
                    ref context,
                    parameter.Value,
                    parameter.Type
                );

                parameterValues.Add(value);
            }

            return parameterValues.ToArray();
        }

        /// <summary>
        /// Renders a for loop code block into a string using the template context
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="loop">The for loop code block</param>
        /// <returns>The rendered block</returns>
        private string RenderForLoop
            (
                ref TemplateContext context,
                ForEachLoop loop
            )
        {
            Validate.IsNotNull(loop);

            var collection = ResolveValue
            (
                ref context,
                loop.CollectionValue,
                loop.CollectionType
            );

            if (collection == null)
            {
                throw new NettleRenderException
                (
                    "A null collection was invoked at index {0}.".With
                    (
                        loop.StartPosition
                    )
                );
            }
            else if (false == collection.GetType().IsEnumerable())
            {
                throw new NettleRenderException
                (
                    "The type {0} is not a valid collection.".With
                    (
                        collection.GetType().Name
                    )
                );
            }

            var builder = new StringBuilder();

            foreach (var item in collection as IEnumerable)
            {
                var nestedContext = context.CreateNestedContext
                (
                    item
                );

                var renderedContent = RenderBlocks
                (
                    ref nestedContext,
                    loop.Blocks
                );

                builder.Append(renderedContent);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Renders an if statement code block into a string using the template context
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="statement">The if statement code block</param>
        /// <returns>The rendered block</returns>
        private string RenderIfStatement
            (
                ref TemplateContext context,
                IfStatement statement
            )
        {
            Validate.IsNotNull(statement);

            var condition = ResolveValue
            (
                ref context,
                statement.ConditionValue,
                statement.ConditionType
            );

            var result = ToBool(condition);

            if (false == result)
            {
                return String.Empty;
            }
            else
            {
                var renderedBody = RenderBlocks
                (
                    ref context,
                    statement.Blocks
                );

                return renderedBody;
            }
        }

        /// <summary>
        /// Converts an object into a boolean representation
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The boolean representation</returns>
        private bool ToBool
            (
                object value
            )
        {
            var result = default(bool);

            if (value != null)
            {
                if (value is bool)
                {
                    result = (bool)value;
                }
                else if (value.GetType().IsNumeric())
                {
                    result = (double)value > 0;
                }
                else
                {
                    result = value.ToString().Length > 0;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a generic object to a string representation
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>A string representation of the value</returns>
        private string ToString
            (
                object value
            )
        {
            if (value == null)
            {
                return String.Empty;
            }
            else
            {
                var valueType = value.GetType();

                if (valueType == typeof(string))
                {
                    return (string)value;
                }
                else if (valueType == typeof(decimal))
                {
                    var roundedValue = Math.Round
                    (
                        (decimal)value,
                        2
                    );

                    return roundedValue.ToString();
                }
                else if (valueType == typeof(double))
                {
                    var roundedValue = Math.Round
                    (
                        (double)value,
                        2
                    );

                    return roundedValue.ToString();
                }
                else
                {
                    return value.ToString();
                }
            }
        }
    }
}
