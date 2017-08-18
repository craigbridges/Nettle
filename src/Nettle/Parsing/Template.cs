namespace Nettle.Parsing
{
    using System.Linq;

    /// <summary>
    /// Represents a parsed template
    /// </summary>
    internal class Template
    {
        /// <summary>
        /// Constructs the template with the raw text and blocks
        /// </summary>
        /// <param name="rawText">The raw text</param>
        /// <param name="blocks">The blocks that make up the template</param>
        public Template
            (
                string rawText,
                params CodeBlock[] blocks
            )
        {
            this.RawText = rawText;
            this.Blocks = blocks;
        }

        /// <summary>
        /// Gets the templates raw text
        /// </summary>
        public string RawText { get; private set; }

        /// <summary>
        /// Gets an array of code blocks that make up the template
        /// </summary>
        public CodeBlock[] Blocks { get; private set; }

        /// <summary>
        /// Finds all blocks of the code block type specified
        /// </summary>
        /// <typeparam name="T">The block type</typeparam>
        /// <returns>An array of matching code blocks</returns>
        public T[] FindBlocks<T>() 
            where T : CodeBlock
        {
            if (this.Blocks == null)
            {
                return new T[] {};
            }
            else
            {
                var matchingBlocks = this.Blocks.Where
                (
                    block => block.GetType() == typeof(T)
                )
                .Select
                (
                    block => block as T
                );

                return matchingBlocks.ToArray();
            }
        }
    }
}
