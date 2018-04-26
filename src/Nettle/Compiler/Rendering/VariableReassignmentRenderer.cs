namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents a variable reassignment renderer
    /// </summary>
    internal class VariableReassignmentRenderer : NettleRendererBase, IBlockRenderer
    {
        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public VariableReassignmentRenderer
            (
                IFunctionRepository functionRepository
            )

            : base(functionRepository)
        { }

        /// <summary>
        /// Determines if the renderer can render the code block specified
        /// </summary>
        /// <param name="block">The code block</param>
        /// <returns>True, if it can be rendered; otherwise false</returns>
        public bool CanRender
            (
                CodeBlock block
            )
        {
            Validate.IsNotNull(block);

            var blockType = block.GetType();

            return
            (
                blockType == typeof(VariableReassignment)
            );
        }

        /// <summary>
        /// Renders the code block specified into a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="block">The code block to render</param>
        /// <param name="flags">The template flags</param>
        /// <returns>The rendered block</returns>
        public string Render
            (
                ref TemplateContext context,
                CodeBlock block,
                params TemplateFlag[] flags
            )
        {
            Validate.IsNotNull(block);

            var variable = (VariableReassignment)block;

            ReassignVariable
            (
                ref context,
                variable
            );

            return String.Empty;
        }

        /// <summary>
        /// Defines a variable in the template context by initialising it
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="variable">The variable code block</param>
        private void ReassignVariable
            (
                ref TemplateContext context,
                VariableDeclaration variable
            )
        {
            Validate.IsNotNull(variable);

            var variableName = variable.VariableName;

            var value = ResolveValue
            (
                ref context,
                variable.AssignedValue,
                variable.ValueType
            );

            context.ReassignVariable
            (
                variableName,
                value
            );
        }
    }
}
