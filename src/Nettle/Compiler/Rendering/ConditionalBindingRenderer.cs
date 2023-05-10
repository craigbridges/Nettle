namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;
using System.Threading.Tasks;

/// <summary>
/// Represents a conditional binding renderer
/// </summary>
internal class ConditionalBindingRenderer : NettleRendererBase, IBlockRenderer
{
    private readonly BooleanExpressionEvaluator _expressionEvaluator;

    public ConditionalBindingRenderer(IFunctionRepository functionRepository, BooleanExpressionEvaluator expressionEvaluator)
        : base(functionRepository)
    {
        Validate.IsNotNull(expressionEvaluator);

        _expressionEvaluator = expressionEvaluator;
    }

    public bool CanRender(CodeBlock block)
    {
        Validate.IsNotNull(block);

        return block.GetType() == typeof(ConditionalBinding);
    }

    public async Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
    {
        var binding = (ConditionalBinding)block;

        var result = await _expressionEvaluator.Evaluate(context, binding.ConditionExpression, cancellationToken);

        object? value;

        if (result)
        {
            value = await ResolveValue(context, binding.TrueValue, binding.TrueValueType, cancellationToken);
        }
        else
        {
            value = await ResolveValue(context, binding.FalseValue, binding.FalseValueType, cancellationToken);
        }

        return ToString(value, context.Flags);
    }
}
