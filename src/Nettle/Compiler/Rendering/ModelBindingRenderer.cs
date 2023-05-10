namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a model binding renderer
    /// </summary>
    internal class ModelBindingRenderer : NettleRendererBase, IBlockRenderer
    {
        public ModelBindingRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        public bool CanRender(CodeBlock block)
        {
            return block.GetType() == typeof(ModelBinding);
        }

        public Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken)
        {
            var binding = (ModelBinding)block;
            var value = ResolveBindingValue(context, binding.BindingPath);

            return Task.FromResult(ToString(value, context.Flags));
        }
    }
}
