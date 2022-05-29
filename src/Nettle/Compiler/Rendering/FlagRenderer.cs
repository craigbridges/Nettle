namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;

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
            Validate.IsNotNull(block);

            return block.GetType() == typeof(FlagDeclaration);
        }

        public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
        {
            return String.Empty;
        }
    }
}
