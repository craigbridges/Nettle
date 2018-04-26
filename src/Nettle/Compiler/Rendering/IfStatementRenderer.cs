namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;
    using System;
    using System.Linq;

    /// <summary>
    /// Represents an if statement renderer
    /// </summary>
    internal class IfStatementRenderer : NettleRendererBase, IBlockRenderer
    {
        private BooleanExpressionEvaluator _expressionEvaluator;
        private BlockCollectionRenderer _collectionRenderer;

        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="expressionEvaluator">The expression evaluator</param>
        /// <param name="collectionRenderer">The block collection renderer</param>
        public IfStatementRenderer
            (
                IFunctionRepository functionRepository,
                BooleanExpressionEvaluator expressionEvaluator,
                BlockCollectionRenderer collectionRenderer
            )

            : base(functionRepository)
        {
            Validate.IsNotNull(expressionEvaluator);
            Validate.IsNotNull(collectionRenderer);

            _expressionEvaluator = expressionEvaluator;
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

            var statement = (IfStatement)block;
            var renderedBody = String.Empty;
            
            var result = _expressionEvaluator.Evaluate
            (
                ref context,
                statement.ConditionExpression
            );

            if (result)
            {
                renderedBody = _collectionRenderer.Render
                (
                    ref context,
                    statement.Blocks,
                    flags
                );
            }
            else if (statement.ElseIfConditions.Any())
            {
                foreach (var elseCondition in statement.ElseIfConditions)
                {
                    result = _expressionEvaluator.Evaluate
                    (
                        ref context,
                        elseCondition.ConditionExpression
                    );

                    if (result)
                    {
                        renderedBody = _collectionRenderer.Render
                        (
                            ref context,
                            elseCondition.Blocks,
                            flags
                        );

                        break;
                    }
                }
            }

            if (false == result && statement.ElseContent != null)
            {
                renderedBody = _collectionRenderer.Render
                (
                    ref context,
                    statement.ElseContent.Blocks,
                    flags
                );
            }

            return renderedBody;
        }
    }
}
