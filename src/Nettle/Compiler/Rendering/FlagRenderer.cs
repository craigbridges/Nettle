namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents a flag renderer
    /// </summary>
    internal class FlagRenderer : NettleRenderer, IBlockRenderer
    {
        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public FlagRenderer
            (
                IFunctionRepository functionRepository
            )

            : base(functionRepository)
        { }

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
                blockType == typeof(FlagDeclaration)
            );
        }

        /// <summary>
        /// Renders the code block specified into a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="block">The code block to render</param>
        /// <returns>The rendered block</returns>
        public string Render
            (
                ref TemplateContext context,
                CodeBlock block
            )
        {
            Validate.IsNotNull(block);

            var flag = (FlagDeclaration)block;

            SetFlag
            (
                ref context,
                flag
            );

            return String.Empty;
        }

        /// <summary>
        /// Sets a flag in the template context by initialising it
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="flag">The flag code block</param>
        private void SetFlag
            (
                ref TemplateContext context,
                FlagDeclaration flag
            )
        {
            Validate.IsNotNull(flag);

            // TODO: set the flag in the template context
        }
    }
}
