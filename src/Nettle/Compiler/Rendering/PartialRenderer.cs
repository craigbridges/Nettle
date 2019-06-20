namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;

    /// <summary>
    /// Represents a partial renderer
    /// </summary>
    internal class PartialRenderer : NettleRendererBase, IBlockRenderer
    {
        private IRegisteredTemplateRepository _templateRepository;
        private BlockCollectionRenderer _collectionRenderer;

        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="templateRepository">The template repository</param>
        /// <param name="collectionRenderer">The block collection renderer</param>
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
        /// <param name="flags">The template flags</param>
        /// <returns>The rendered block</returns>
        public string Render
            (
                ref TemplateContext context,
                CodeBlock block,
                params TemplateFlag[] flags
            )
        {
            Validate.IsNotNull(block);

            var partial = (RenderPartial)block;

            CheckForCircularReference
            (
                ref context,
                partial
            );

            var template = _templateRepository.Get
            (
                partial.TemplateName
            );

            var partialContent = template.ParsedTemplate.Blocks;
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

            var newContext = context.CreateNestedContext
            (
                model
            );

            newContext.Variables.Clear();

            newContext.PartialCallStack.Add
            (
                partial.TemplateName
            );

            return _collectionRenderer.Render
            (
                ref newContext,
                partialContent,
                flags
            );
        }

        /// <summary>
        /// Checks for circular reference calls to a partial that has already been called
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="partial">The partial code block</param>
        private void CheckForCircularReference
            (
                ref TemplateContext context,
                RenderPartial partial
            )
        {
            Validate.IsNotNull(partial);

            var templateName = partial.TemplateName;

            var previousCallFound = context.PartialCallStack.Contains
            (
                templateName
            );

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
