namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;

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

    public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
    {
        Validate.IsNotNull(block);

        var binding = (ConditionalBinding)block;

        var result = _expressionEvaluator.Evaluate(ref context, binding.ConditionExpression);

        object? value;

        if (result)
        {
            value = ResolveValue(ref context, binding.TrueValue, binding.TrueValueType);
        }
        else
        {
            value = ResolveValue(ref context, binding.FalseValue, binding.FalseValueType);
        }

        return ToString(value, flags);
    }
}
