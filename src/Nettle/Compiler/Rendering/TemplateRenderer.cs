namespace Nettle.Compiler.Rendering;

using System.Threading.Tasks;

internal class TemplateRenderer : NettleRendererBase, ITemplateRenderer
{
    private readonly BlockCollectionRenderer _collectionRenderer;

    public TemplateRenderer(IFunctionRepository functionRepository, IRegisteredTemplateRepository templateRepository)
        : base(functionRepository)
    {
        Validate.IsNotNull(templateRepository);

        _collectionRenderer = new BlockCollectionRenderer(functionRepository, templateRepository);
    }

    public async Task<string> Render(Template template, object model, CancellationToken cancellationToken)
    {
        Validate.IsNotNull(template);

        var watch = Stopwatch.StartNew();
        var debugMode = template.IsFlagSet(TemplateFlag.DebugMode);
        var flags = template.Flags;

        var context = new TemplateContext(model, flags);
        var blocks = template.Blocks;

        var output = _collectionRenderer.Render(ref context, blocks, flags);

        if (debugMode)
        {
            watch.Stop();

            var debugInfo = context.GenerateDebugInfo(watch.Elapsed);

            output += $"\r\n\r\n{debugInfo}";
        }

        return output;
    }
}
