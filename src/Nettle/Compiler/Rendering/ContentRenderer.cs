namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a content renderer
    /// </summary>
    internal class ContentRenderer : NettleRendererBase, IBlockRenderer
    {
        public ContentRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        public bool CanRender(CodeBlock block)
        {
            return block.GetType() == typeof(ContentBlock);
        }

        public Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
        {
            return Task.FromResult(block.Signature);
        }
    }
}
