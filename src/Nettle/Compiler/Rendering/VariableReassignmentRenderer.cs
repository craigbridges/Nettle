namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a variable reassignment renderer
    /// </summary>
    internal class VariableReassignmentRenderer : NettleRendererBase, IBlockRenderer
    {
        public VariableReassignmentRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        public bool CanRender(CodeBlock block)
        {
            Validate.IsNotNull(block);

            return block.GetType() == typeof(VariableReassignment);
        }

        public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
        {
            Validate.IsNotNull(block);

            var variable = (VariableReassignment)block;

            ReassignVariable(ref context, variable);

            return String.Empty;
        }

        /// <summary>
        /// Defines a variable in the template context by initialising it
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="variable">The variable code block</param>
        private void ReassignVariable(ref TemplateContext context, VariableDeclaration variable)
        {
            Validate.IsNotNull(variable);

            var variableName = variable.VariableName;
            var value = ResolveValue(ref context, variable.AssignedValue, variable.ValueType);

            context.ReassignVariable(variableName, value);
        }
    }
}
