namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a while loop renderer
    /// </summary>
    internal class WhileLoopRenderer : NettleRendererBase, IBlockRenderer
    {
        private readonly BooleanExpressionEvaluator _expressionEvaluator;
        private readonly BlockCollectionRenderer _collectionRenderer;

        public WhileLoopRenderer
            (
                IFunctionRepository functionRepository,
                BooleanExpressionEvaluator expressionEvaluator,
                BlockCollectionRenderer collectionRenderer
            )
            : base(functionRepository)
        {
            Validate.IsNotNull(expressionEvaluator);
            Validate.IsNotNull(collectionRenderer);

            _expressionEvaluator = expressionEvaluator;
            _collectionRenderer = collectionRenderer;
        }

        public bool CanRender(CodeBlock block)
        {
            Validate.IsNotNull(block);

            return block.GetType() == typeof(WhileLoop);
        }

        public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
        {
            Validate.IsNotNull(block);

            var loop = (WhileLoop)block;
            var result = _expressionEvaluator.Evaluate(ref context, loop.ConditionExpression);

            var builder = new StringBuilder();
            
            while (result)
            {
                var renderedBody = _collectionRenderer.Render(ref context, loop.Blocks, flags);

                builder.Append(renderedBody);

                result = _expressionEvaluator.Evaluate(ref context, loop.ConditionExpression);
            }

            return builder.ToString();
        }
    }
}
