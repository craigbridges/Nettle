namespace Nettle.Compiler.Rendering
{
    /// <summary>
    /// Defines a contract for a template renderer
    /// </summary>
    internal interface ITemplateRenderer
    {
        /// <summary>
        /// Renders a Nettle template with the model data specified
        /// </summary>
        /// <param name="template">The template</param>
        /// <param name="model">The model data</param>
        /// <returns>The rendered template</returns>
        string Render
        (
            Template template,
            object model
        );
    }
}
