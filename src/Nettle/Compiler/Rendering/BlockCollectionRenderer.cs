namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Blocks;

internal sealed class BlockCollectionRenderer
{
    private readonly IBlockRenderer[] _renderers;

    public BlockCollectionRenderer(IFunctionRepository functionRepository, IRegisteredTemplateRepository templateRepository)
    {
        Validate.IsNotNull(functionRepository);
        Validate.IsNotNull(templateRepository);

        var expressionEvaluator = new BooleanExpressionEvaluator(functionRepository);

        _renderers = new IBlockRenderer[]
        {
            new CommentRenderer(functionRepository),
            new ContentRenderer(functionRepository),
            new ModelBindingRenderer(functionRepository),
            new ConditionalBindingRenderer(functionRepository, expressionEvaluator),
            new VariableRenderer(functionRepository),
            new VariableReassignmentRenderer(functionRepository),
            new VariableIncrementerRenderer(functionRepository),
            new VariableDecrementerRenderer(functionRepository),
            new FlagRenderer(functionRepository),
            new FunctionRenderer(functionRepository),
            new ForEachLoopRenderer(functionRepository, this),
            new WhileLoopRenderer(functionRepository, expressionEvaluator, this),
            new IfStatementRenderer(functionRepository, expressionEvaluator, this),
            new PartialRenderer(functionRepository, templateRepository, this)
        };
    }

    /// <summary>
    /// Renders an array of code blocks to a string
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="blocks">An array of blocks to render</param>
    /// <param name="flags">The template flags</param>
    /// <returns>The rendered code blocks</returns>
    public string Render(ref TemplateContext context, CodeBlock[] blocks, params TemplateFlag[] flags)
    {
        var builder = new StringBuilder();

        var autoFormat = flags.Contains(TemplateFlag.AutoFormat);

        var previousBlockType = default(Type);
        var previousRawOutput = String.Empty;

        foreach (var block in blocks)
        {
            var blockOutput = RenderBlock(ref context, block, flags);

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
                        var startsWithLineBreak = formattedOutput.StartsWithAny("\n", "\r", "\r\n");

                        // Determine if content is only tabs or line breaks
                        var isWhitespace = formattedOutput.IsMadeUpOf('\n', '\r', '\t');

                        formattedOutput = RemoveExtraPadding(formattedOutput);

                        if (startsWithLineBreak && false == isWhitespace && false == previousHadLineBreak)
                        {
                            formattedOutput = "\r\n" + formattedOutput;
                        }
                    }
                }
                else
                {
                    // Determine if previous output was only tabs or line breaks
                    var wasWhitespace = previousRawOutput.IsMadeUpOf('\n', '\r', '\t');

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
                var endsWithLineBreak = output.EndsWithAny("\n", "\r", "\r\n");

                // Ensure nested blocks end with line breaks
                if (false == endsWithLineBreak)
                {
                    output += "\r\n";
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
    private static string RemoveExtraPadding(string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            return text;
        }
        else
        {
            text = text.Trim("\t", "\n\t", "\r\t", "\r\n\t", "\n", "\r", "\r\n");
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
    private string RenderBlock(ref TemplateContext context, CodeBlock block, params TemplateFlag[] flags)
    {
        var ignoreErrors = flags.Contains(TemplateFlag.IgnoreErrors);

        var renderer = FindRenderer(block);
        var blockOutput = String.Empty;

        try
        {
            blockOutput = renderer.Render(ref context, block, flags);
        }
        catch (Exception ex)
        {
            if (false == ignoreErrors)
            {
                throw new NettleRenderException
                (
                    $"Exception raised during rendering:\r\n\r\n{ex.Message}\r\n\r\n{block}",
                    ex
                );
            }
        }

        return blockOutput;
    }

    /// <summary>
    /// Finds a renderer for the code block specified
    /// </summary>
    /// <param name="block">The code block</param>
    /// <returns>The matching renderer</returns>
    private IBlockRenderer FindRenderer(CodeBlock block)
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

        throw new NettleRenderException($"No renderer could be found for '{block}'.");
    }
}
