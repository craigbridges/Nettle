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
        private CommentParser _commentParser;
        private ModelBindingParser _bindingParser;
        private FunctionParser _functionParser;
        private VariableParser _variableParser;
        private ForEachLoopParser _loopParser;
        private IfStatementParser _ifParser;

        /// <summary>
        /// Constructs the blockifier by initialising block parsers
        /// </summary>
        public Blockifier()
        {
            _commentParser = new CommentParser();
            _bindingParser = new ModelBindingParser();
            _functionParser = new FunctionParser();
            _variableParser = new VariableParser();
            _loopParser = new ForEachLoopParser(this);
            _ifParser = new IfStatementParser(this);
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
                var endPosition = startPosition + (blockContent.Length - 1);

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
                var endPosition = startPosition + (templateContent.Length - 1);

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
                        //The final closing tag was found
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

                // Comment
                if (signatureBody.StartsWith("!"))
                {
                    return _commentParser.Parse
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // Function call
                else if (signatureBody.StartsWith("@"))
                {
                    return _functionParser.Parse
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // Variable declaration
                else if (signatureBody.StartsWith("var"))
                {
                    return _variableParser.Parse
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // For each loop
                else if (signatureBody.StartsWith("foreach"))
                {
                    return _loopParser.Parse
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // If statement
                else if (signatureBody.StartsWith("if"))
                {
                    return _ifParser.Parse
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // Model binding
                else
                {
                    return _bindingParser.Parse
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
            }
        }
    }
}
