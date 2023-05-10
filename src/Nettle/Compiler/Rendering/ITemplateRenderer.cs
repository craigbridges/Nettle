namespace Nettle.Compiler.Rendering
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a contract for a template renderer
    /// </summary>
    internal interface ITemplateRenderer
    {
        /// <summary>
        /// Asynchronously renders a Nettle template with the model data specified
        /// </summary>
        /// <param name="template">The template</param>
        /// <param name="model">The model data</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The rendered template</returns>
        Task<string> Render(Template template, object model, CancellationToken cancellationToken);
    }
}
