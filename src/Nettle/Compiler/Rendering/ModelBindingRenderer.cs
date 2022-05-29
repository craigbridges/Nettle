namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;

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
            Validate.IsNotNull(block);

            return block.GetType() == typeof(ModelBinding);
        }

        public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
        {
            Validate.IsNotNull(block);

            var binding = (ModelBinding)block;
            var value = ResolveBindingValue(ref context, binding.BindingPath);

            return ToString(value, flags);
        }
    }
}
