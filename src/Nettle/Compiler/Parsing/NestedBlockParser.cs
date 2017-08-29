namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;

    /// <summary>
    /// Represents a function code block parser
    /// </summary>
    internal abstract class NestedBlockParser : NettleParser, IBlockParser
    {
        /// <summary>
        /// Constructs the parser with a blockifier
        /// </summary>
        /// <param name="blockifier">The blockifier</param>
        protected NestedBlockParser
            (
                IBlockifier blockifier
            )
        {
            Validate.IsNotNull(blockifier);

            this.Blockifier = blockifier;
        }

        /// <summary>
        /// Gets the blockifier
        /// </summary>
        protected IBlockifier Blockifier { get; private set; }

        /// <summary>
        /// Gets the open tag name
        /// </summary>
        protected abstract string OpenTagName { get;  }

        /// <summary>
        /// Gets the close tag name
        /// </summary>
        protected abstract string CloseTagName { get; }

        /// <summary>
        /// Determines if a signature matches the block type of the parser
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <returns>True, if it matches; otherwise false</returns>
        public bool Matches
            (
                string signatureBody
            )
        {
            return signatureBody.StartsWith
            (
                this.OpenTagName
            );
        }

        /// <summary>
        /// Parses the signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public abstract CodeBlock Parse
        (
            ref string templateContent,
            ref int positionOffSet,
            string signature
        );

        /// <summary>
        /// Extracts the body of a nested code block
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The variable signature</param>
        /// <returns>The extracted block</returns>
        protected NestableCodeBlock ExtractNestedBody
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var startIndex = signature.Length;
            var templateLength = templateContent.Length;
            var body = String.Empty;
            var openTagSyntax = @"{{" + this.OpenTagName + " ";
            var closeTagSyntax = @"{{" + this.CloseTagName + @"}}";
            var openTagCount = 1;
            var closeTagCount = 0;
            var endFound = false;

            for (int currentIndex = startIndex; currentIndex < templateLength; currentIndex++)
            {
                body += templateContent[currentIndex];

                if (body.Length > 1)
                {
                    if (body.EndsWith(openTagSyntax))
                    {
                        openTagCount++;
                    }
                    else if (body.EndsWith(closeTagSyntax))
                    {
                        closeTagCount++;
                    }
                }

                if (openTagCount == closeTagCount)
                {
                    //The final closing tag was found
                    endFound = true;
                    break;
                }
            }

            if (false == endFound)
            {
                throw new NettleParseException
                (
                    "No '{0}' tag was found.".With
                    (
                        closeTagSyntax
                    ),
                    templateLength
                );
            }

            signature += body;

            body = body.Substring
            (
                0,
                body.Length - closeTagSyntax.Length
            );

            var blocks = this.Blockifier.Blockify(body);
            var startPosition = positionOffSet;
            var endPosition = signature.Length - 1;

            TrimTemplate
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new NestableCodeBlock()
            {
                Signature = signature,
                StartPosition = startPosition,
                EndPosition = endPosition,
                Body = body,
                Blocks = blocks
            };
        }
    }
}
