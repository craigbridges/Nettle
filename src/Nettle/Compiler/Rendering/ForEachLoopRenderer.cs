namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;

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
        Validate.IsNotNull(block);

        return block.GetType() == typeof(ForEachLoop);
    }

    public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
    {
        Validate.IsNotNull(block);

        var loop = (ForEachLoop)block;
        var collection = ResolveValue(ref context, loop.CollectionValue, loop.CollectionType);

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

        foreach (var item in (IEnumerable)collection)
        {
            var nestedContext = context.CreateNestedContext(item);
            var renderedContent = _collectionRenderer.Render(ref nestedContext, loop.Blocks, flags);

            builder.Append(renderedContent);
        }

        return builder.ToString();
    }
}
