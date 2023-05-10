namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;
using Nettle.Functions;
using System.Threading.Tasks;

/// <summary>
/// Represents a partial renderer
/// </summary>
internal class PartialRenderer : NettleRendererBase, IBlockRenderer
{
    private readonly IRegisteredTemplateRepository _templateRepository;
    private readonly BlockCollectionRenderer _collectionRenderer;

    public PartialRenderer
        (
            IFunctionRepository functionRepository,
            IRegisteredTemplateRepository templateRepository,
            BlockCollectionRenderer collectionRenderer
        )
        : base(functionRepository)
    {
        Validate.IsNotNull(templateRepository);
        Validate.IsNotNull(collectionRenderer);

        _templateRepository = templateRepository;
        _collectionRenderer = collectionRenderer;
    }

    public bool CanRender(CodeBlock block)
    {
        return block.GetType() == typeof(RenderPartial);
    }

    public async Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
    {
        var partial = (RenderPartial)block;

        CheckForCircularReference(context, partial);

        var template = _templateRepository.Get(partial.TemplateName);
        var partialContent = template.ParsedTemplate.Blocks;
        var model = context.Model;

        if (partial.ModelType.HasValue && partial.ModelValue != null)
        {
            model = await ResolveValue(context, partial.ModelValue, partial.ModelType.Value, cancellationToken);
        }

        var newContext = context.CreateNestedContext(model ?? context.Model);

        newContext.Variables.Clear();
        newContext.PartialCallStack.Add(partial.TemplateName);

        return await _collectionRenderer.Render(newContext, partialContent, cancellationToken);
    }

    /// <summary>
    /// Checks for circular reference calls to a partial that has already been called
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="partial">The partial code block</param>
    private static void CheckForCircularReference(TemplateContext context, RenderPartial partial)
    {
        var templateName = partial.TemplateName;
        var previousCallFound = context.PartialCallStack.Contains(templateName);

        if (previousCallFound)
        {
            throw new NettleRenderException
            (
                $"A circular reference to '{templateName}' was detected."
            );
        }
    }
}
