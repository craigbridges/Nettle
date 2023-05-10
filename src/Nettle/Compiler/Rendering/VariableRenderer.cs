namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;
using System.Threading.Tasks;

/// <summary>
/// Represents a variable renderer
/// </summary>
internal class VariableRenderer : NettleRendererBase, IBlockRenderer
{
    public VariableRenderer(IFunctionRepository functionRepository)
        : base(functionRepository)
    { }

    public bool CanRender(CodeBlock block)
    {
        Validate.IsNotNull(block);

        return block.GetType() == typeof(VariableDeclaration);
    }

    public async Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
    {
        await DefineVariable(context, (VariableDeclaration)block, cancellationToken);

        return String.Empty;
    }

    /// <summary>
    /// Asynchronously defines a variable in the template context by initialising it
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="variable">The variable code block</param>
    /// <param name="cancellationToken">The cancellation token</param>
    private async Task DefineVariable(TemplateContext context, VariableDeclaration variable, CancellationToken cancellationToken)
    {
        var variableName = variable.VariableName;
        var value = await ResolveValue(context, variable.AssignedValue, variable.ValueType, cancellationToken);

        context.DefineVariable(variableName, value);
    }
}
