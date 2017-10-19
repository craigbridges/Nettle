namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            var variableIncrementerRenderer = new VariableIncrementerRenderer
            (
                functionRepository
            );

            var variableDecrementerRenderer = new VariableDecrementerRenderer
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
                variableIncrementerRenderer,
                variableDecrementerRenderer,
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
        /// <param name="flags">The template flags</param>
        /// <returns>The rendered code blocks</returns>
        public string Render
            (
                ref TemplateContext context,
                CodeBlock[] blocks,
                params TemplateFlag[] flags
            )
        {
            var builder = new StringBuilder();
            
            var autoFormat = flags.Contains
            (
                TemplateFlag.AutoFormat
            );

            var previousBlockType = default(Type);
            var previousRawOutput = String.Empty;

            foreach (var block in blocks)
            {
                var blockOutput = RenderBlock
                (
                    ref context,
                    block,
                    flags
                );

                var formattedOutput = blockOutput;
                var blockType = block.GetType();

                if (autoFormat)
                {
                    var previousHadLineBreak = previousRawOutput.EndsWithAny
                    (
                        "\n",
                        "\r",
                        "\r\n",
                        "\n\t",
                        "\r\t",
                        "\r\n\t"
                    );

                    if (blockType == typeof(ContentBlock))
                    {
                        if (previousBlockType == null || previousBlockType != typeof(ContentBlock))
                        {
                            var startsWithLineBreak = formattedOutput.StartsWithAny
                            (
                                "\n",
                                "\r",
                                "\r\n"
                            );

                            // Determine if content is only tabs or line breaks
                            var isWhitespace = formattedOutput.IsMadeUpOf
                            (
                                '\n',
                                '\r',
                                '\t'
                            );

                            formattedOutput = RemoveExtraPadding
                            (
                                formattedOutput
                            );

                            if (startsWithLineBreak && false == isWhitespace && false == previousHadLineBreak)
                            {
                                formattedOutput = "\r\n" + formattedOutput;
                            }
                        }
                    }
                    else
                    {
                        // Determine if previous output was only tabs or line breaks
                        var wasWhitespace = previousRawOutput.IsMadeUpOf
                        (
                            '\n',
                            '\r',
                            '\t'
                        );

                        if (previousHadLineBreak && false == wasWhitespace)
                        {
                            formattedOutput = "\r\n" + formattedOutput;
                        }
                    }
                }
                
                builder.Append(formattedOutput);

                previousBlockType = block.GetType();
                previousRawOutput = blockOutput;
            }

            // Check if we should minify the output (this overrides auto format)
            if (flags.Contains(TemplateFlag.Minify))
            {
                builder.Replace("\t", String.Empty);
                builder.Replace("    ", String.Empty);
                builder.Replace("\r\n", String.Empty);
                builder.Replace("\n", String.Empty);
                builder.Replace("\r", String.Empty);
            }

            var output = builder.ToString();

            if (autoFormat)
            {
                if (context.IsRoot())
                {
                    // Remove leading and trailing padding from template output
                    output = RemoveExtraPadding(output);
                }
                else
                {
                    var endsWithLineBreak = output.EndsWithAny
                    (
                        "\n",
                        "\r",
                        "\r\n"
                    );

                    // Ensure nested blocks end with line breaks
                    if (false == endsWithLineBreak)
                    {
                        output = output + "\r\n";
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Removes extra padding around a string text
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>The updated text</returns>
        /// <remarks>
        /// Extra padding are tabs or line breaks at the start or end of the text
        /// </remarks>
        private string RemoveExtraPadding
            (
                string text
            )
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }
            else
            {
                text = text.Trim
                (
                    "\t",
                    "\n\t",
                    "\r\t",
                    "\r\n\t",
                    "\n",
                    "\r",
                    "\r\n"
                );
            }
            
            return text;
        }

        /// <summary>
        /// Renders a single code block into a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="block">The code block to render</param>
        /// <param name="flags">The template flags</param>
        /// <returns>The rendered block</returns>
        private string RenderBlock
            (
                ref TemplateContext context,
                CodeBlock block,
                params TemplateFlag[] flags
            )
        {
            var ignoreErrors = flags.Contains
            (
                TemplateFlag.IgnoreErrors
            );

            var renderer = FindRenderer(block);
            var blockOutput = String.Empty;

            if (ignoreErrors)
            {
                try
                {
                    blockOutput = renderer.Render
                    (
                        ref context,
                        block,
                        flags
                    );
                }
                catch
                {
                    // NOTE: Purposefully ignore all errors
                }
            }
            else
            {
                blockOutput = renderer.Render
                (
                    ref context,
                    block,
                    flags
                );
            }

            return blockOutput;
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
