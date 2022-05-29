namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;

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
            Validate.IsNotNull(block);

            return block.GetType() == typeof(RenderPartial);
        }

        public string Render(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
        {
            Validate.IsNotNull(block);

            var partial = (RenderPartial)block;

            CheckForCircularReference(ref context, partial);

            var template = _templateRepository.Get(partial.TemplateName);
            var partialContent = template.ParsedTemplate.Blocks;
            var model = context.Model;

            if (partial.ModelType.HasValue && partial.ModelValue != null)
            {
                model = ResolveValue(ref context, partial.ModelValue, partial.ModelType.Value);
            }

            var newContext = context.CreateNestedContext(model ?? context.Model);

            newContext.Variables.Clear();
            newContext.PartialCallStack.Add(partial.TemplateName);

            return _collectionRenderer.Render(ref newContext, partialContent, flags);
        }

        /// <summary>
        /// Checks for circular reference calls to a partial that has already been called
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="partial">The partial code block</param>
        private static void CheckForCircularReference(ref TemplateContext context, RenderPartial partial)
        {
            Validate.IsNotNull(partial);

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
}
