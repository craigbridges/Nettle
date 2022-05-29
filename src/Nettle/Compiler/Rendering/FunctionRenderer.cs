namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a function renderer
    /// </summary>
    internal class FunctionRenderer : NettleRendererBase, IBlockRenderer
    {
        public FunctionRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        public bool CanRender(CodeBlock block)
        {
            Validate.IsNotNull(block);

            return block.GetType() == typeof(FunctionCall);
        }

        public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
        {
            Validate.IsNotNull(block);

            var call = (FunctionCall)block;
            var result = ExecuteFunction(ref context, call);

            return ToString(result.Output, flags);
        }
    }
}
