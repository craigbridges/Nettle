namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing;
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a base class for all Nettle renderers
    /// </summary>
    internal abstract class NettleRenderer
    {
        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public NettleRenderer
            (
                IFunctionRepository functionRepository
            )
        {
            Validate.IsNotNull(functionRepository);

            this.FunctionRepository = functionRepository;
        }

        /// <summary>
        /// Gets the function repository
        /// </summary>
        protected IFunctionRepository FunctionRepository
        {
            get;
            private set;
        }

        /// <summary>
        /// Resolves a value by converting it to the Nettle value type specified
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="rawValue">The raw value</param>
        /// <param name="type">The value type</param>
        /// <param name="parsedFunction">The parsed function (optional)</param>
        /// <returns>The resolved value</returns>
        protected object ResolveValue
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
        /// Resolves a model binding value from the context and binding path
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="bindingPath">The bindings path</param>
        /// <returns>The model bindings value</returns>
        /// <remarks>
        /// A check is made to see if the binding path refers to a variable.
        /// If not variable is found then it is assumed to be a property.
        /// </remarks>
        protected object ResolveBindingValue
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
        /// Executes a function call using the template context specified
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="call">The function call code block</param>
        /// <returns>The function execution result</returns>
        protected FunctionExecutionResult ExecuteFunction
            (
                ref TemplateContext context,
                FunctionCall call
            )
        {
            Validate.IsNotNull(call);

            var function = this.FunctionRepository.GetFunction
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
        protected object[] ResolveParameterValues
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
        /// Converts a generic object to a string representation
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>A string representation of the value</returns>
        protected string ToString
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
