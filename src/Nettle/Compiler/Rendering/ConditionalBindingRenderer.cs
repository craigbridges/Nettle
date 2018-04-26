namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;

    /// <summary>
    /// Represents a conditional binding renderer
    /// </summary>
    internal class ConditionalBindingRenderer : NettleRendererBase, IBlockRenderer
    {
        private BooleanExpressionEvaluator _expressionEvaluator;
        
        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="expressionEvaluator">The expression evaluator</param>
        public ConditionalBindingRenderer
            (
                IFunctionRepository functionRepository,
                BooleanExpressionEvaluator expressionEvaluator
            )

            : base(functionRepository)
        {
            Validate.IsNotNull(expressionEvaluator);
            
            _expressionEvaluator = expressionEvaluator;
        }

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
                blockType == typeof(ConditionalBinding)
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

            var binding = (ConditionalBinding)block;
            var value = default(object);

            var result = _expressionEvaluator.Evaluate
            (
                ref context,
                binding.ConditionExpression
            );

            if (result)
            {
                value = ResolveValue
                (
                    ref context,
                    binding.TrueValue,
                    binding.TrueValueType
                );
            }
            else
            {
                value = ResolveValue
                (
                    ref context,
                    binding.FalseValue,
                    binding.FalseValueType
                );
            }

            return ToString(value, flags);
        }
    }
}
