namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the default implementation of a blockifier
    /// </summary>
    internal sealed class Blockifier : NettleParser, IBlockifier
    {
        private List<IBlockParser> _parsers;

        /// <summary>
        /// Constructs the blockifier by initialising block parsers
        /// </summary>
        public Blockifier()
        {
            var commentParser = new CommentParser();
            var bindingParser = new ModelBindingParser();
            var conditionalBindingParser = new ConditionalBindingParser();
            var functionParser = new FunctionParser();
            var variableParser = new VariableParser();
            var variableReassignmentParser = new VariableReassignmentParser();
            var variableIncrementerParser = new VariableIncrementerParser();
            var variableDecrementerParser = new VariableDecrementerParser();
            var flagParser = new FlagParser();
            var eachLoopParser = new ForEachLoopParser(this);
            var whileLoopParser = new WhileLoopParser(this);
            var ifParser = new IfStatementParser(this);
            var partialParser = new RenderPartialParser();

            _parsers = new List<IBlockParser>()
            {
                commentParser,
                bindingParser,
                conditionalBindingParser,
                functionParser,
                variableParser,
                variableReassignmentParser,
                variableIncrementerParser,
                variableDecrementerParser,
                flagParser,
                eachLoopParser,
                whileLoopParser,
                ifParser,
                partialParser
            };
        }

        /// <summary>
        /// Parses the content specified into code blocks
        /// </summary>
        /// <param name="content">The content to parse</param>
        /// <returns>An array of code blocks</returns>
        public CodeBlock[] Blockify
            (
                string content
            )
        {
            var templateContent = String.Copy(content);
            var blocksFound = new List<CodeBlock>();
            var positionOffSet = new int();

            while (templateContent.Length > 0)
            {
                var nextBlock = ExtractNextBlock
                (
                    ref templateContent,
                    ref positionOffSet
                );

                blocksFound.Add(nextBlock);
            }

            return blocksFound.ToArray();
        }

        /// <summary>
        /// Extracts the next code block from the template content specified
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset</param>
        /// <returns>The code block found</returns>
        private CodeBlock ExtractNextBlock
            (
                ref string templateContent,
                ref int positionOffSet
            )
        {
            var nextCodeBlockIndex = templateContent.IndexOf
            (
                @"{{"
            );

            // Check if there is any content preceding the next code block
            // Return the content as the next block, if any was found
            if (nextCodeBlockIndex > 0)
            {
                var blockContent = templateContent.Substring
                (
                    0,
                    nextCodeBlockIndex
                );

                var startPosition = positionOffSet;
                var endPosition = (startPosition + blockContent.Length);

                templateContent = templateContent.Crop
                (
                    nextCodeBlockIndex
                );

                positionOffSet += nextCodeBlockIndex;

                return new ContentBlock()
                {
                    Signature = blockContent,
                    StartPosition = startPosition,
                    EndPosition = endPosition
                };
            }
            // When no other code blocks are found, return everything as a content block
            else if (nextCodeBlockIndex == -1)
            {
                var blockContent = String.Copy(templateContent);
                var startPosition = positionOffSet;
                var endPosition = (startPosition + templateContent.Length);

                positionOffSet = endPosition;
                templateContent = String.Empty;

                return new ContentBlock()
                {
                    Signature = blockContent,
                    StartPosition = startPosition,
                    EndPosition = endPosition
                };
            }
            else
            {
                var templateLength = templateContent.Length;
                var signature = String.Empty;
                var openTagCount = 0;
                var closeTagCount = 0;
                var endFound = false;

                for (int currentIndex = 0; currentIndex < templateLength; currentIndex++)
                {
                    signature += templateContent[currentIndex];

                    if (signature.Length > 1)
                    {
                        if (signature.EndsWith(@"{{"))
                        {
                            openTagCount++;
                        }
                        else if (signature.EndsWith(@"}}"))
                        {
                            closeTagCount++;
                        }
                    }

                    if (openTagCount > 0 && openTagCount == closeTagCount)
                    {
                        // The final closing tag was found
                        endFound = true;
                        break;
                    }
                }
                
                if (false == endFound)
                {
                    throw new NettleParseException
                    (
                        "No end block tag was found.",
                        templateLength
                    );
                }

                var signatureBody = UnwrapSignatureBody(signature);

                // Ensure the code blocks body is valid
                if (false == IsValidSignatureBody(signatureBody))
                {
                    throw new NettleParseException
                    (
                        "Invalid block signature '{0}'.".With
                        (
                            signatureBody
                        ),
                        positionOffSet
                    );
                }

                var parser = FindParser
                (
                    signatureBody,
                    positionOffSet
                );

                return parser.Parse
                (
                    ref templateContent,
                    ref positionOffSet,
                    signature
                );
            }
        }

        /// <summary>
        /// Finds a code block parser for the signature body specified
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <param name="positionOffSet">The position offset</param>
        /// <returns>The parser found</returns>
        private IBlockParser FindParser
            (
                string signatureBody,
                int positionOffSet
            )
        {
            foreach (var parser in _parsers)
            {
                var matches = parser.Matches(signatureBody);

                if (matches)
                {
                    return parser;
                }
            }

            var message = "No parser could be found for the signature '{0}'.";

            throw new NettleParseException
            (
                message.With(signatureBody),
                positionOffSet
            );
        }
    }
}
