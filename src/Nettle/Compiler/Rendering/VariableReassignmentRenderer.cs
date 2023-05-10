namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;
using System.Threading.Tasks;

/// <summary>
/// Represents a variable reassignment renderer
/// </summary>
internal class VariableReassignmentRenderer : NettleRendererBase, IBlockRenderer
{
    public VariableReassignmentRenderer(IFunctionRepository functionRepository)
        : base(functionRepository)
    { }

    public bool CanRender(CodeBlock block)
    {
        return block.GetType() == typeof(VariableReassignment);
    }

    public async Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
    {
        var variable = (VariableReassignment)block;

        await ReassignVariable(context, variable, cancellationToken);

        return String.Empty;
    }

    /// <summary>
    /// Asynchronous defines a variable in the template context by initialising it
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="variable">The variable code block</param>
    /// <param name="cancellationToken">The cancellation</param>
    private async Task ReassignVariable(TemplateContext context, VariableDeclaration variable, CancellationToken cancellationToken)
    {
        var variableName = variable.VariableName;
        var value = await ResolveValue(context, variable.AssignedValue, variable.ValueType, cancellationToken);

        context.ReassignVariable(variableName, value);
    }
}
