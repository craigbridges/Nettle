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
            var previousOutput = String.Empty;
            var removeNextLineBreak = false;

            foreach (var block in blocks)
            {
                var blockOutput = RenderBlock
                (
                    ref context,
                    block,
                    flags
                );

                if (autoFormat)
                {
                    if (removeNextLineBreak)
                    {
                        var startsWithLineBreak = blockOutput.StartsWithAny
                        (
                            "\n",
                            "\r",
                            "\r\n"
                        );

                        if (startsWithLineBreak)
                        {
                            if (blockOutput.StartsWith("\n"))
                            {
                                blockOutput = blockOutput.RemoveFirst("\n");
                            }
                            else if (blockOutput.StartsWith("\r"))
                            {
                                blockOutput = blockOutput.RemoveFirst("\r");
                            }
                            else if (blockOutput.StartsWith("\r\n"))
                            {
                                blockOutput = blockOutput.RemoveFirst("\r\n");
                            }

                            removeNextLineBreak = false;
                        }
                    }

                    if (block.GetType() == typeof(ContentBlock))
                    {
                        if (previousBlockType == null || previousBlockType != typeof(ContentBlock))
                        {
                            blockOutput = RemoveExtraPadding(blockOutput);
                        }
                    }
                    else if (previousBlockType == typeof(ContentBlock))
                    {
                        var endedWithLineBreak = previousOutput.EndsWithAny
                        (
                            "\n",
                            "\r",
                            "\r\n"
                        );

                        if (endedWithLineBreak)
                        {
                            removeNextLineBreak = true;
                        }
                    }
                }

                builder.Append(blockOutput);

                previousBlockType = block.GetType();
                previousOutput = blockOutput;
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

            return builder.ToString();
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
            else if (text.StartsWith("\r\n\r\n"))
            {
                text = text.ReplaceFirst
                (
                    "\r\n\r\n",
                    "\r\n"
                );
            }
            else
            {
                text = RemoveFirstAndLastOccurances
                (
                    text,
                    "\t",
                    "\n\t",
                    "\r\t",
                    "\r\n\t"
                );
            }
            
            return text;
        }

        /// <summary>
        /// Removes the first and last occurrences of a value within a string
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="phases">The search phases</param>
        /// <returns>The updated text</returns>
        private string RemoveFirstAndLastOccurances
            (
                string text,
                params string[] phases
            )
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            foreach (var phrase in phases)
            {
                if (text.StartsWith(phrase))
                {
                    text = text.RemoveFirst(phrase);
                }

                if (text.EndsWith(phrase))
                {
                    text = text.RemoveLast(phrase);
                }
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
