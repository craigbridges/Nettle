namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a variable adjuster renderer
    /// </summary>
    /// <typeparam name="T">The variable adjuster type</typeparam>
    internal abstract class VariableAdjusterRenderer<T> : NettleRendererBase, IBlockRenderer
        where T : VariableAdjuster
    {
        public VariableAdjusterRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        /// <summary>
        /// The amount to adjust the variable by
        /// </summary>
        protected abstract int Adjustment { get; }

        public virtual bool CanRender(CodeBlock block)
        {
            return block.GetType() == typeof(T);
        }

        public Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
        {
            var adjuster = (T)block;

            AdjustVariable(context, adjuster);

            return Task.FromResult(String.Empty);
        }

        /// <summary>
        /// Defines a adjuster in the template context by initialising it
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="adjuster">The adjuster code block</param>
        protected virtual void AdjustVariable(TemplateContext context, T adjuster)
        {
            var variableName = adjuster.VariableName;

            var value = context.ResolveVariableValue(variableName);

            if (value == null)
            {
                throw new NettleRenderException
                (
                    $"{variableName} cannot be adjusted because it is null."
                );
            }

            if (false == value.GetType().IsNumeric())
            {
                throw new NettleRenderException
                (
                    $"{variableName} is not a numeric type."
                );
            }

            value = (double)value + Adjustment;
            
            context.ReassignVariable(variableName, value);
        }
    }
}
