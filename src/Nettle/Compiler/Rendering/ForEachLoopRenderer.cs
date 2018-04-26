namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System.Collections;
    using System.Text;

    /// <summary>
    /// Represents a for each loop renderer
    /// </summary>
    internal class ForEachLoopRenderer : NettleRendererBase, IBlockRenderer
    {
        private BlockCollectionRenderer _collectionRenderer;

        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="collectionRenderer">The block collection renderer</param>
        public ForEachLoopRenderer
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
                blockType == typeof(ForEachLoop)
            );
        }

        /// <summary>
        /// Renders the code block specified into a string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="block">The code block to render</param>
        /// <param name="flags">The template flags</param>
        /// <returns>The rendered block</returns>
        public string Render
            (
                ref TemplateContext context,
                CodeBlock block,
                params TemplateFlag[] flags
            )
        {
            Validate.IsNotNull(block);

            var loop = (ForEachLoop)block;

            var collection = ResolveValue
            (
                ref context,
                loop.CollectionValue,
                loop.CollectionType
            );

            if (collection == null)
            {
                throw new NettleRenderException
                (
                    "A null collection was invoked at index {0}.".With
                    (
                        loop.StartPosition
                    )
                );
            }
            else if (false == collection.GetType().IsEnumerable())
            {
                throw new NettleRenderException
                (
                    "The type {0} is not a valid collection.".With
                    (
                        collection.GetType().Name
                    )
                );
            }

            var builder = new StringBuilder();

            foreach (var item in collection as IEnumerable)
            {
                var nestedContext = context.CreateNestedContext
                (
                    item
                );

                var renderedContent = _collectionRenderer.Render
                (
                    ref nestedContext,
                    loop.Blocks,
                    flags
                );

                builder.Append(renderedContent);
            }

            return builder.ToString();
        }
    }
}
