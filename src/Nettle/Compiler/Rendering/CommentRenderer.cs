namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a comment renderer
    /// </summary>
    internal class CommentRenderer : NettleRendererBase, IBlockRenderer
    {
        public CommentRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        public bool CanRender(CodeBlock block)
        {
            Validate.IsNotNull(block);

            return block.GetType() == typeof(Comment);
        }

        public Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
        {
            return Task.FromResult(String.Empty);
        }
    }
}
