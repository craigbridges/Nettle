namespace Nettle.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the default implementation of a Nettle parser
    /// </summary>
    internal class DefaultNettleParser : INettleParser
    {
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
                var blocks = Blockify(content);

                return new Template
                (
                    content,
                    blocks
                );
            }
        }

        /// <summary>
        /// Parses the content into code blocks
        /// </summary>
        /// <param name="templateContent">The content to parse</param>
        /// <returns>An array of code blocks</returns>
        private CodeBlock[] Blockify
            (
                string templateContent
            )
        {

            // TODO:
            // scan ahead until a block start signature is found
            // if valid then scan ahead until end of block is found (or end of content is reached)
            // determine what it is and if it's valid
            // if not valid, throw exception
            // if valid, then break into code parts and initial code block object, then add to collection


            var blockStartIndexes = templateContent.IndexesOf
            (
                "{{"
            )
            .ToArray();

            // Check if there are any code blocks to process
            if (blockStartIndexes.Length == 0)
            {
                return new CodeBlock[]
                {
                    new ContentBlock()
                    {
                        Signature = templateContent,
                        StartPosition = 0,
                        EndPosition = templateContent.Length - 1
                    }
                };
            }
            
            var blocksFound = new List<CodeBlock>();
            var currentPosition = blockStartIndexes[0] - 1;
            var templateLength = templateContent.Length;

            // Grab all the content before the first code block
            var initialContent = templateContent.Substring
            (
                0,
                currentPosition
            );

            blocksFound.Add
            (
                new ContentBlock()
                {
                    Signature = initialContent,
                    StartPosition = 0,
                    EndPosition = currentPosition
                }
            );

            // Skip to the start of each block found in the template
            foreach (var blockIndex in blockStartIndexes)
            {
                currentPosition = blockIndex + 2;

                var signature = "{{";
                var endFound = false;

                // Scan through the block to find its end
                while (false == endFound && currentPosition < templateLength)
                {
                    signature += templateContent[currentPosition];

                    if (signature.EndsWith("}}"))
                    {
                        endFound = true;
                    }

                    currentPosition++;
                }

                if (false == endFound)
                {
                    throw new NettleParseException
                    (
                        "No end block was found.",
                        currentPosition
                    );
                }

                var signatureBody = signature.Substring
                (
                    2,
                    signature.Length - 2
                );

                var startPosition = blockIndex;
                var endPosition = currentPosition - 1;

                // Ensure the blocks body is valid
                if (String.IsNullOrWhiteSpace(signatureBody))
                {
                    throw new NettleParseException
                    (
                        "Invalid block signature.",
                        currentPosition
                    );
                }

                // Function call
                if (signatureBody.StartsWith("@"))
                {
                    var function = ParseFunction
                    (
                        signature,
                        startPosition,
                        endPosition
                    );

                    blocksFound.Add(function);
                }
                // Variable declaration
                else if (signatureBody.StartsWith("var"))
                {
                    var variable = ParseVariable
                    (
                        signature,
                        startPosition,
                        endPosition
                    );

                    blocksFound.Add(variable);
                }
                // For each loop
                else if (signatureBody.StartsWith("foreach"))
                {
                    // TODO: validate syntax   
                    // TODO: extract collection name
                    // TODO: extract loop body
                    // TODO: call Blockify again to parse body
                    // TODO: find end block (i.e. {{endfor}}
                }
                // If statement
                else if (signatureBody.StartsWith("if"))
                {
                    // TODO: validate syntax   
                    // TODO: extract condition name
                    // TODO: extract if body
                    // TODO: call Blockify again to parse body
                    // TODO: find end block (i.e. {{endif}}
                }
                // Model binding
                else
                {
                    if (signatureBody.Contains(" "))
                    {
                        throw new NettleParseException
                        (
                            "Model bindings cannot contain spaces.",
                            currentPosition
                        );
                    }

                    var binding = new ModelBinding()
                    {
                        Signature = signature,
                        StartPosition = startPosition,
                        EndPosition = endPosition
                    };

                    blocksFound.Add(binding);
                }

                // TODO: check that the next block index is less than the final current position
            }
            
            return blocksFound.ToArray();
        }

        /// <summary>
        /// Parses a function signature into a function call object
        /// </summary>
        /// <param name="signature">The function signature</param>
        /// <param name="startPosition">The start position</param>
        /// <param name="endPosition">The end position</param>
        /// <returns>The parsed function</returns>
        private FunctionCall ParseFunction
            (
                string signature,
                int startPosition,
                int endPosition
            )
        {
            // TODO: validate syntax
            // TODO: extract function name
            // TODO: extract parameters
            // TODO: determine what each parameter type is

            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a variable signature into a variable declaration object
        /// </summary>
        /// <param name="signature">The variable signature</param>
        /// <param name="startPosition">The start position</param>
        /// <param name="endPosition">The end position</param>
        /// <returns>The parsed variable declaration</returns>
        private VariableDeclaration ParseVariable
            (
                string signature,
                int startPosition,
                int endPosition
            )
        {
            // TODO: validate syntax
            // TODO: extract variable name
            // TODO: extract assigned value
            // TODO: determine what the assigned value type is
            // TODO: if it is a function, parse the function

            throw new NotImplementedException();
        }
    }
}
