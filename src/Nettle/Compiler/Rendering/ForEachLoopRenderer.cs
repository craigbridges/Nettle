namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;
using System.Threading.Tasks;

/// <summary>
/// Represents a for each loop renderer
/// </summary>
internal class ForEachLoopRenderer : NettleRendererBase, IBlockRenderer
{
    private readonly BlockCollectionRenderer _collectionRenderer;

    public ForEachLoopRenderer(IFunctionRepository functionRepository, BlockCollectionRenderer collectionRenderer)
        : base(functionRepository)
    {
        Validate.IsNotNull(collectionRenderer);

        _collectionRenderer = collectionRenderer;
    }

    public bool CanRender(CodeBlock block)
    {
        return block.GetType() == typeof(ForEachLoop);
    }

    public async Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
    {
        var loop = (ForEachLoop)block;
        var collection = await ResolveValue(context, loop.CollectionValue, loop.CollectionType, cancellationToken);

        if (collection == null)
        {
            throw new NettleRenderException
            (
                $"A null collection was invoked at index {loop.StartPosition}."
            );
        }
        else if (false == collection.GetType().IsEnumerable())
        {
            throw new NettleRenderException
            (
                $"The type {collection.GetType().Name} is not a valid collection."
            );
        }

        var builder = new StringBuilder();
        var renderTasks = new List<Task<string>>();

        foreach (var item in (IEnumerable)collection)
        {
            var nestedContext = context.CreateNestedContext(item);
            var task = _collectionRenderer.Render(nestedContext, loop.Blocks, cancellationToken);

            renderTasks.Add(task);
        }

        var renderedContent = await Task.WhenAll(renderTasks);

        renderedContent.ToList().ForEach(content => builder.Append(content));

        return builder.ToString();
    }
}
