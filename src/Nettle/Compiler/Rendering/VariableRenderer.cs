namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a variable renderer
    /// </summary>
    internal class VariableRenderer : NettleRendererBase, IBlockRenderer
    {
        public VariableRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        public bool CanRender(CodeBlock block)
        {
            Validate.IsNotNull(block);

            return block.GetType() == typeof(VariableDeclaration);
        }

        public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
        {
            Validate.IsNotNull(block);

            DefineVariable(ref context, (VariableDeclaration)block);

            return String.Empty;
        }

        /// <summary>
        /// Defines a variable in the template context by initialising it
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="variable">The variable code block</param>
        private void DefineVariable(ref TemplateContext context, VariableDeclaration variable)
        {
            Validate.IsNotNull(variable);

            var variableName = variable.VariableName;
            var value = ResolveValue(ref context, variable.AssignedValue, variable.ValueType);

            context.DefineVariable(variableName, value);
        }
    }
}
