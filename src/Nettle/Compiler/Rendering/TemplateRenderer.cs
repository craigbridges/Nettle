namespace Nettle.Compiler.Rendering
{
    using Nettle.Functions;

    /// <summary>
    /// Represents the default implementation of a template renderer
    /// </summary>
    internal class TemplateRenderer : NettleRenderer, ITemplateRenderer
    {
        private BlockCollectionRenderer _collectionRenderer;

        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="templateRepository">The template repository</param>
        public TemplateRenderer
            (
                IFunctionRepository functionRepository,
                IRegisteredTemplateRepository templateRepository
            )

            : base(functionRepository)
        {
            Validate.IsNotNull(templateRepository);

            _collectionRenderer = new BlockCollectionRenderer
            (
                functionRepository,
                templateRepository
            );
        }

        /// <summary>
        /// Renders a Nettle template with the model data specified
        /// </summary>
        /// <param name="template">The template</param>
        /// <param name="model">The model data</param>
        /// <returns>The rendered template</returns>
        public string Render
            (
                Template template,
                object model
            )
        {
            Validate.IsNotNull(template);

            var context = new TemplateContext(model);
            var blocks = template.Blocks;

            return _collectionRenderer.Render
            (
                ref context,
                blocks
            );
        }
    }
}
