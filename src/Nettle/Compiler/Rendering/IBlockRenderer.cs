namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a contract for a code block renderer
    /// </summary>
    internal interface IBlockRenderer
    {
        /// <summary>
        /// Determines if the renderer can render the code block specified
        /// </summary>
        /// <param name="block">The code block</param>
        /// <returns>True, if it can be rendered; otherwise false</returns>
        bool CanRender(CodeBlock block);

        /// <summary>
        /// Asynchronously renders the code block specified into a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="block">The code block to render</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The rendered block</returns>
        Task<string> Render(TemplateContext context, CodeBlock block, CancellationToken cancellationToken);
    }
}
