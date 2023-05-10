namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;
using System.Threading.Tasks;

/// <summary>
/// Represents an if statement renderer
/// </summary>
internal class IfStatementRenderer : NettleRendererBase, IBlockRenderer
{
    private readonly BooleanExpressionEvaluator _expressionEvaluator;
    private readonly BlockCollectionRenderer _collectionRenderer;

    public IfStatementRenderer
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
        return block.GetType() == typeof(IfStatement);
    }

    public async Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
    {
        var statement = (IfStatement)block;
        var renderedBody = String.Empty;

        var result = await _expressionEvaluator.Evaluate(context, statement.ConditionExpression, cancellationToken);

        if (result)
        {
            renderedBody = await _collectionRenderer.Render(context, statement.Blocks, cancellationToken);
        }
        else if (statement.ElseIfConditions.Any())
        {
            foreach (var elseCondition in statement.ElseIfConditions)
            {
                result = await _expressionEvaluator.Evaluate(context, elseCondition.ConditionExpression, cancellationToken);

                if (result)
                {
                    renderedBody = await _collectionRenderer.Render(context, elseCondition.Blocks, cancellationToken);
                    break;
                }
            }
        }

        if (false == result && statement.ElseContent != null)
        {
            renderedBody = await _collectionRenderer.Render(context, statement.ElseContent.Blocks, cancellationToken);
        }

        return renderedBody;
    }
}
