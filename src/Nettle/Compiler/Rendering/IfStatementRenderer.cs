namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;

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
        Validate.IsNotNull(block);

        return block.GetType() == typeof(IfStatement);
    }

    public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
    {
        Validate.IsNotNull(block);

        var statement = (IfStatement)block;
        var renderedBody = String.Empty;

        var result = _expressionEvaluator.Evaluate(ref context, statement.ConditionExpression);

        if (result)
        {
            renderedBody = _collectionRenderer.Render(ref context, statement.Blocks, flags);
        }
        else if (statement.ElseIfConditions.Any())
        {
            foreach (var elseCondition in statement.ElseIfConditions)
            {
                result = _expressionEvaluator.Evaluate(ref context, elseCondition.ConditionExpression);

                if (result)
                {
                    renderedBody = _collectionRenderer.Render(ref context, elseCondition.Blocks, flags);
                    break;
                }
            }
        }

        if (false == result && statement.ElseContent != null)
        {
            renderedBody = _collectionRenderer.Render(ref context, statement.ElseContent.Blocks, flags);
        }

        return renderedBody;
    }
}
