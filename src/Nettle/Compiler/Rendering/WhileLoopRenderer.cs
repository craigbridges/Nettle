namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;
using System.Threading.Tasks;

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
        return block.GetType() == typeof(WhileLoop);
    }

    public async Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
    {
        var loop = (WhileLoop)block;
        var result = await _expressionEvaluator.Evaluate(context, loop.ConditionExpression, cancellationToken);

        var builder = new StringBuilder();
        
        while (result)
        {
            var renderedBody = await _collectionRenderer.Render(context, loop.Blocks, cancellationToken);

            builder.Append(renderedBody);

            result = await _expressionEvaluator.Evaluate(context, loop.ConditionExpression, cancellationToken);
        }

        return builder.ToString();
    }
}
