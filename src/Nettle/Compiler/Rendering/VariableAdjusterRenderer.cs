namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents a variable adjuster renderer
    /// </summary>
    /// <typeparam name="T">The variable adjuster type</typeparam>
    internal abstract class VariableAdjusterRenderer<T> : NettleRendererBase, IBlockRenderer
        where T : VariableAdjuster
    {
        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public VariableAdjusterRenderer
            (
                IFunctionRepository functionRepository
            )

            : base(functionRepository)
        { }

        /// <summary>
        /// Gets the adjustment amount
        /// </summary>
        protected abstract int Adjustment { get; }

        /// <summary>
        /// Determines if the renderer can render the code block specified
        /// </summary>
        /// <param name="block">The code block</param>
        /// <returns>True, if it can be rendered; otherwise false</returns>
        public virtual bool CanRender
            (
                CodeBlock block
            )
        {
            Validate.IsNotNull(block);

            var blockType = block.GetType();

            return
            (
                blockType == typeof(T)
            );
        }

        /// <summary>
        /// Renders the code block specified into a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="block">The code block to render</param>
        /// <param name="flags">The template flags</param>
        /// <returns>The rendered block</returns>
        public virtual string Render
            (
                ref TemplateContext context,
                CodeBlock block,
                params TemplateFlag[] flags
            )
        {
            Validate.IsNotNull(block);

            var adjuster = (T)block;

            AdjustVariable
            (
                ref context,
                adjuster
            );

            return String.Empty;
        }

        /// <summary>
        /// Defines a adjuster in the template context by initialising it
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="adjuster">The adjuster code block</param>
        protected virtual void AdjustVariable
            (
                ref TemplateContext context,
                T adjuster
            )
        {
            Validate.IsNotNull(adjuster);

            var variableName = adjuster.VariableName;

            var value = context.ResolveVariableValue
            (
                variableName
            );

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

            value = (double)value + this.Adjustment;
            
            context.ReassignVariable
            (
                variableName,
                value
            );
        }
    }
}
