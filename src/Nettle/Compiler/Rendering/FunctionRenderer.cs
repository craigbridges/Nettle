namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;
using System.Threading.Tasks;

/// <summary>
/// Represents a function block renderer
/// </summary>
internal class FunctionRenderer : NettleRendererBase, IBlockRenderer
{
    public FunctionRenderer(IFunctionRepository functionRepository)
        : base(functionRepository)
    { }

    public bool CanRender(CodeBlock block)
    {
        return block.GetType() == typeof(FunctionCall);
    }

    public async Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
    {
        var call = (FunctionCall)block;
        var result = await ExecuteFunction(context, call, cancellationToken);

        return ToString(result.Output, context.Flags);
    }
}
