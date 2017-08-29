namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;

    /// <summary>
    /// Represents a partial renderer
    /// </summary>
    internal class PartialRenderer : NettleRenderer, IBlockRenderer
    {
        private IRegisteredTemplateRepository _templateRepository;

        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="templateRepository">The template repository</param>
        public PartialRenderer
            (
                IFunctionRepository functionRepository,
                IRegisteredTemplateRepository templateRepository
            )

            : base(functionRepository)
        {
            _templateRepository = templateRepository;
        }

        /// <summary>
        /// Determines if the renderer can render the code block specified
        /// </summary>
        /// <param name="block">The code block</param>
        /// <returns>True, if it can be rendered; otherwise false</returns>
        public bool CanRender
            (
                CodeBlock block
            )
        {
            Validate.IsNotNull(block);

            var blockType = block.GetType();

            return
            (
                blockType == typeof(RenderPartial)
            );
        }

        /// <summary>
        /// Renders the code block specified into a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="block">The code block to render</param>
        /// <returns>The rendered block</returns>
        public string Render
            (
                ref TemplateContext context,
                CodeBlock block
            )
        {
            Validate.IsNotNull(block);

            var partial = (RenderPartial)block;

            var registeredTemplate = _templateRepository.Get
            (
                partial.TemplateName
            );

            var template = registeredTemplate.Template;
            var model = context.Model;

            if (partial.ModelType.HasValue && partial.ModelValue != null)
            {
                model = ResolveValue
                (
                    ref context,
                    partial.ModelValue,
                    partial.ModelType.Value
                );
            }

            return template(model);
        }
    }
}
