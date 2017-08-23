namespace Nettle.Compiler.Parsing
{
    using System;

    /// <summary>
    /// Represents the default implementation of a template parser
    /// </summary>
    internal sealed class TemplateParser : ITemplateParser
    {
        private IBlockifier _blockifier;

        /// <summary>
        /// Constructs the template parser with required dependencies
        /// </summary>
        /// <param name="blockifier">The blockifier</param>
        public TemplateParser
            (
                IBlockifier blockifier
            )
        {
            Validate.IsNotNull(blockifier);

            _blockifier = blockifier;
        }

        /// <summary>
        /// Parses the content specified into a template
        /// </summary>
        /// <param name="content">The content to parse</param>
        /// <returns>The template</returns>
        public Template Parse
            (
                string content
            )
        {
            if (String.IsNullOrWhiteSpace(content))
            {
                return new Template
                (
                    content
                );
            }
            else
            {
                var blocks = _blockifier.Blockify
                (
                    content
                );

                return new Template
                (
                    content,
                    blocks
                );
            }
        }
    }
}
