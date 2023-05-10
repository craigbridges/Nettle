namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a flag renderer
    /// </summary>
    internal class FlagRenderer : NettleRendererBase, IBlockRenderer
    {
        public FlagRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        public bool CanRender(CodeBlock block)
        {
            return block.GetType() == typeof(FlagDeclaration);
        }

        public Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
        {
            return Task.FromResult(String.Empty);
        }
    }
}
