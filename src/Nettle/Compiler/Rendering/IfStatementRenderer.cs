namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents an if statement renderer
    /// </summary>
    internal class IfStatementRenderer : NettleRenderer, IBlockRenderer
    {
        private BlockCollectionRenderer _collectionRenderer;

        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="collectionRenderer">The block collection renderer</param>
        public IfStatementRenderer
            (
                IFunctionRepository functionRepository,
                BlockCollectionRenderer collectionRenderer
            )

            : base(functionRepository)
        {
            Validate.IsNotNull(collectionRenderer);

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
                blockType == typeof(IfStatement)
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

            var statement = (IfStatement)block;

            var condition = ResolveValue
            (
                ref context,
                statement.ConditionValue,
                statement.ConditionType
            );

            var result = ToBool(condition);

            if (false == result)
            {
                return String.Empty;
            }
            else
            {
                var renderedBody = _collectionRenderer.Render
                (
                    ref context,
                    statement.Blocks
                );

                return renderedBody;
            }
        }

        /// <summary>
        /// Converts an object into a boolean representation
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The boolean representation</returns>
        private bool ToBool
            (
                object value
            )
        {
            var result = default(bool);

            if (value != null)
            {
                if (value is bool)
                {
                    result = (bool)value;
                }
                else if (value.GetType().IsNumeric())
                {
                    result = (double)value > 0;
                }
                else
                {
                    result = value.ToString().Length > 0;
                }
            }

            return result;
        }
    }
}
