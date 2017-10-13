namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents the default implementation of a template renderer
    /// </summary>
    internal sealed class BlockCollectionRenderer
    {
        private List<IBlockRenderer> _renderers;

        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="templateRepository">The template repository</param>
        public BlockCollectionRenderer
            (
                IFunctionRepository functionRepository,
                IRegisteredTemplateRepository templateRepository
            )
        {
            Validate.IsNotNull(functionRepository);
            Validate.IsNotNull(templateRepository);

            var commentRenderer = new CommentRenderer
            (
                functionRepository
            );

            var contentRenderer = new ContentRenderer
            (
                functionRepository
            );

            var bindingRenderer = new ModelBindingRenderer
            (
                functionRepository
            );

            var variableRenderer = new VariableRenderer
            (
                functionRepository
            );

            var variableReassignmentRenderer = new VariableReassignmentRenderer
            (
                functionRepository
            );

            var flagRenderer = new FlagRenderer
            (
                functionRepository
            );

            var functionRenderer = new FunctionRenderer
            (
                functionRepository
            );

            var loopRenderer = new ForEachLoopRenderer
            (
                functionRepository,
                this
            );

            var ifStatementRenderer = new IfStatementRenderer
            (
                functionRepository,
                this
            );

            var partialRenderer = new PartialRenderer
            (
                functionRepository,
                templateRepository,
                this
            );

            _renderers = new List<IBlockRenderer>()
            {
                commentRenderer,
                contentRenderer,
                bindingRenderer,
                variableRenderer,
                variableReassignmentRenderer,
                flagRenderer,
                functionRenderer,
                loopRenderer,
                ifStatementRenderer,
                partialRenderer
            };
        }
        
        /// <summary>
        /// Renders an array of code blocks to a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="blocks">An array of blocks to render</param>
        /// <returns>The rendered code blocks</returns>
        public string Render
            (
                ref TemplateContext context,
                CodeBlock[] blocks
            )
        {
            var builder = new StringBuilder();

            foreach (var block in blocks)
            {
                var renderer = FindRenderer(block);

                var blockOutput = renderer.Render
                (
                    ref context,
                    block
                );

                builder.Append(blockOutput);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Finds a renderer for the code block specified
        /// </summary>
        /// <param name="block">The code block</param>
        /// <returns>The matching renderer</returns>
        private IBlockRenderer FindRenderer
            (
                CodeBlock block
            )
        {
            Validate.IsNotNull(block);

            foreach (var renderer in _renderers)
            {
                var canRender = renderer.CanRender(block);

                if (canRender)
                {
                    return renderer;
                }
            }

            throw new NettleRenderException
            (
                "No renderer could be found for '{0}'.".With
                (
                    block.ToString()
                )
            );
        }
    }
}
