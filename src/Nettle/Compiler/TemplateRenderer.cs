namespace Nettle.Compiler
{
    using Nettle.Compiler.Parsing;
    using Nettle.Functions;
    using System;
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
        /// Defines a variable in the template context by initialising it
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="binding">The variable code block</param>
        private void DefineVariable
            (
                ref TemplateContext context,
                VariableDeclaration variable
            )
        {
            Validate.IsNotNull(variable);

            var value = default(object);
            var assignedSignature = variable.AssignedValueSignature;

            switch (variable.ValueType)
            {
                case NettleValueType.String:

                    // TODO: remove start and end " chars

                    value = assignedSignature;
                    break;

                case NettleValueType.Number:

                    value = Double.Parse(assignedSignature);
                    break;

                case NettleValueType.Boolean:

                    value = Boolean.Parse(assignedSignature);
                    break;

                case NettleValueType.ModelBinding:

                    // TODO: extract the binding name from the signature
                    var bindingName = assignedSignature;

                    value = ResolveBindingValue
                    (
                        ref context,
                        bindingName
                    );

                    break;

                case NettleValueType.Function:

                    var result = ExecuteFunction
                    (
                        ref context,
                        variable.FunctionCall
                    );

                    value = result.Output;
                    break;

                case NettleValueType.Variable:

                    value = context.Variables[variable.VariableName];
                    break;
            }

            context.DefineVariable
            (
                variable.VariableName,
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
                binding.ItemName
            );

            return ToString(value);
        }

        /// <summary>
        /// Resolves a model binding value from the context and binding name
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="itemName">The binding item name</param>
        /// <returns>The model bindings value</returns>
        private object ResolveBindingValue
            (
                ref TemplateContext context,
                string itemName
            )
        {
            Validate.IsNotEmpty(itemName);

            // TODO: resolve the binding (determine if it's a variable or property)
            // TODO: get the bindings value (including nested properties and variables)

            throw new NotImplementedException();
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

            // TODO: resolve each parameter value based on what value type it is

            throw new NotImplementedException();
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

            // TODO: resolve the collection instance
            // TODO: for each item in the collection, render blocks in body

            throw new NotImplementedException();
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

            // TODO: resolve the condition value (to true or false)
            // TODO: if false, return empty string
            // TODO: if true, render body blocks and return result

            throw new NotImplementedException();
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
                // TODO: format select types such as date and time, numbers etc

                return value.ToString();
            }
        }
    }
}
