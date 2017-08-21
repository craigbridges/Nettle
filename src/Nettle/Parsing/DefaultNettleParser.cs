namespace Nettle.Parsing
{
    using System;
    using System.Collections.Generic;

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
        /// Parses the content specified into code blocks
        /// </summary>
        /// <param name="content">The content to parse</param>
        /// <returns>An array of code blocks</returns>
        private CodeBlock[] Blockify
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
                "{{"
            );

            // Check if there is any content preceding the next code block
            // Return the content as the next block, if any was found
            if (nextCodeBlockIndex > 0)
            {
                var blockContent = templateContent.Substring
                (
                    0,
                    nextCodeBlockIndex + 1
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
                        if (signature.EndsWith("{{"))
                        {
                            openTagCount++;
                        }
                        else if (signature.EndsWith("}}"))
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
                var startPosition = positionOffSet;
                var endPosition = startPosition + (signature.Length - 1);

                // Ensure the code blocks body is valid
                if (false == IsValidSignatureBody(signatureBody))
                {
                    throw new NettleParseException
                    (
                        "Invalid block signature '{0}'.".With
                        (
                            signatureBody
                        ),
                        startPosition
                    );
                }

                // Function call
                if (signatureBody.StartsWith("@"))
                {
                    return ExtractFunction
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // Variable declaration
                else if (signatureBody.StartsWith("var"))
                {
                    return ExtractVariable
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // For each loop
                else if (signatureBody.StartsWith("foreach"))
                {
                    return ExtractForEachLoop
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // If statement
                else if (signatureBody.StartsWith("if"))
                {
                    return ExtractIfStatement
                    (
                        ref templateContent,
                        ref positionOffSet,
                        signature
                    );
                }
                // Model binding
                else
                {
                    return new ModelBinding()
                    {
                        Signature = signature,
                        StartPosition = startPosition,
                        EndPosition = endPosition
                    };
                }
            }
        }

        /// <summary>
        /// Determines if a code blocks signature body is valid
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <returns>True, if the body is valid; otherwise false</returns>
        private bool IsValidSignatureBody
            (
                string signatureBody
            )
        {
            if (String.IsNullOrWhiteSpace(signatureBody))
            {
                return false;
            }
            else if (signatureBody.Contains("{"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Unwraps the body from a code block signature
        /// </summary>
        /// <param name="signature">The signature</param>
        /// <returns>The signatures body</returns>
        private string UnwrapSignatureBody
            (
                string signature
            )
        {
            var body = String.Empty;

            if (signature.StartsWith("{{") && signature.EndsWith("}}"))
            {
                body = signature.Substring
                (
                    2,
                    signature.Length - 2
                );
            }

            return body.Trim();
        }

        /// <summary>
        /// Resolves the value type from its string representation
        /// </summary>
        /// <param name="value">The raw value</param>
        /// <returns>The value type resolved</returns>
        private NettleValueType ResolveType
            (
                string value
            )
        {
            var type = default(NettleValueType);

            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                type = NettleValueType.String;
            }
            else if (value.StartsWith("{{") && value.EndsWith("}}"))
            {
                type = NettleValueType.ModelBinding;
            }
            else if (value.StartsWith("@"))
            {
                type = NettleValueType.Function;
            }
            else
            {
                // Decide if the value looks like a number or variable
                if (value.IsNumeric())
                {
                    type = NettleValueType.Number;
                }
                else
                {
                    type = NettleValueType.Variable;
                }
            }

            return type;
        }

        /// <summary>
        /// Trims the signature from the template content specified
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The signature</param>
        private void TrimTemplate
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var endPosition = signature.Length - 1;

            positionOffSet += endPosition;

            templateContent = templateContent.Crop
            (
                endPosition
            );
        }

        /// <summary>
        /// Extracts the function signature into a function call object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The function signature</param>
        /// <returns>The parsed function</returns>
        private FunctionCall ExtractFunction
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var body = UnwrapSignatureBody(signature);

            body = body.RightOf("@");

            if (false == body.EndsWith(")"))
            {
                var errorPosition = 
                (
                    positionOffSet + (signature.Length - 1)
                );

                throw new NettleParseException
                (
                    "The functions closing bracket it missing.",
                    errorPosition
                );
            }

            var name = body.LeftOf("(");
            var parameterSegment = signature.RightOf("(").Replace(" ", "");
            var parameterValues = parameterSegment.Split(',');
            var parameters = new List<FunctionCallParameter>();

            // Parse each parameter and check what type it is
            foreach (var value in parameterValues)
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new NettleParseException
                    (
                        "Parameter values cannot be empty.",
                        positionOffSet
                    );
                }

                var type = ResolveType(value);

                parameters.Add
                (
                    new FunctionCallParameter()
                    {
                        Value = value,
                        Type = type
                    }
                );
            }

            var startPosition = positionOffSet;
            var endPosition = signature.Length - 1;

            TrimTemplate
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new FunctionCall()
            {
                Signature = signature,
                StartPosition = startPosition,
                EndPosition = endPosition,
                FunctionName = name,
                ParameterValues = parameters.ToArray()
            };
        }

        /// <summary>
        /// Extracts the variable signature into a variable declaration object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The variable signature</param>
        /// <returns>The parsed variable declaration</returns>
        private VariableDeclaration ExtractVariable
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var body = UnwrapSignatureBody(signature);

            body = body.RightOf("var ");

            var parts = body.Split('=');

            if (parts.Length != 2)
            {
                throw new NettleParseException
                (
                    "The variable declaration '{0}' has invalid syntax.".With
                    (
                        signature
                    ),
                    positionOffSet
                );
            }

            var name = parts[0].Trim();
            var assignedValue = parts[1].Trim();
            var type = ResolveType(assignedValue);

            if (false == IsValidVariableName(name))
            {
                throw new NettleParseException
                (
                    "The variable name '{0}' is invalid.".With
                    (
                        name
                    ),
                    positionOffSet
                );
            }

            var startPosition = positionOffSet;
            var endPosition = signature.Length - 1;

            TrimTemplate
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            var declaration = new VariableDeclaration()
            {
                Signature = signature,
                StartPosition = startPosition,
                EndPosition = endPosition,
                VariableName = name,
                AssignedValueSignature = assignedValue
            };

            if (type == NettleValueType.Function)
            {
                var functionTemplate = String.Copy(assignedValue);
                var assignmentStartIndex = default(int);

                declaration.FunctionCall = ExtractFunction
                (
                    ref functionTemplate,
                    ref assignmentStartIndex,
                    assignedValue
                );
            }

            return declaration;
        }

        /// <summary>
        /// Determines if a variable name is valid
        /// </summary>
        /// <param name="name">The variable name</param>
        /// <returns>True, if the variable name is valid; otherwise false</returns>
        private bool IsValidVariableName
            (
                string name
            )
        {
            if (String.IsNullOrWhiteSpace(name) || name.Contains(" "))
            {
                return false;
            }
            else if (name.ContainsNonAscii())
            {
                return false;
            }
            else if (name.IsNumeric())
            {
                return false;
            }
            else if (false == name.IsAlphaNumeric())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Extracts the 'for each' signature into a variable declaration object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The variable signature</param>
        /// <returns>The parsed variable declaration</returns>
        private ForEachLoop ExtractForEachLoop
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var forLoop = UnwrapSignatureBody(signature);

            var collectionName = forLoop.RightOf
            (
                "foreach "
            );

            var collectionType = ResolveType
            (
                collectionName
            );

            var isModelBinding = false;

            switch (collectionType)
            {
                case NettleValueType.ModelBinding:
                    isModelBinding = true;
                    break;

                case NettleValueType.Variable:
                    isModelBinding = false;
                    break;

                default:
                    throw new NettleParseException
                    (
                        "'{0}' is not a valid collection",
                        positionOffSet
                    );
            }

            var nestedBody = ExtractNestedBody
            (
                ref templateContent,
                ref positionOffSet,
                signature,
                "foreach",
                "endfor"
            );

            return new ForEachLoop()
            {
                Signature = nestedBody.Signature,
                StartPosition = nestedBody.StartPosition,
                EndPosition = nestedBody.EndPosition,
                CollectionName = collectionName,
                IsModelBinding = isModelBinding,
                Body = nestedBody.Body,
                Blocks = nestedBody.Blocks
            };
        }

        /// <summary>
        /// Extracts the 'if statement' signature into a variable declaration object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The variable signature</param>
        /// <returns>The parsed variable declaration</returns>
        private IfStatement ExtractIfStatement
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var ifStatement = UnwrapSignatureBody(signature);

            var conditionName = ifStatement.RightOf
            (
                "if "
            );

            var conditionType = ResolveType
            (
                conditionName
            );

            var isModelBinding = false;

            switch (conditionType)
            {
                case NettleValueType.ModelBinding:
                    isModelBinding = true;
                    break;

                case NettleValueType.Variable:
                    isModelBinding = false;
                    break;

                default:
                    throw new NettleParseException
                    (
                        "'{0}' is not a valid condition",
                        positionOffSet
                    );
            }

            var nestedBody = ExtractNestedBody
            (
                ref templateContent,
                ref positionOffSet,
                signature,
                "foreach",
                "endfor"
            );

            return new IfStatement()
            {
                Signature = nestedBody.Signature,
                StartPosition = nestedBody.StartPosition,
                EndPosition = nestedBody.EndPosition,
                ConditionName = conditionName,
                IsModelBinding = isModelBinding,
                Body = nestedBody.Body,
                Blocks = nestedBody.Blocks
            };
        }

        /// <summary>
        /// Extracts the body of a nested code block
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The variable signature</param>
        /// <param name="openTagName">The open tag name</param>
        /// <param name="closeTagName">The end tag name</param>
        /// <returns>The extracted block</returns>
        private NestableCodeBlock ExtractNestedBody
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature,
                string openTagName,
                string closeTagName
            )
        {
            var startIndex = signature.Length;
            var templateLength = templateContent.Length;
            var body = String.Empty;
            var openTagCount = 0;
            var closeTagCount = 0;
            var endFound = false;

            for (int currentIndex = startIndex; currentIndex < templateLength; currentIndex++)
            {
                body += templateContent[currentIndex];

                if (body.Length > 1)
                {
                    if (body.EndsWith("{{" + openTagName))
                    {
                        openTagCount++;
                    }
                    else if (body.EndsWith("{{" + closeTagName + "}}"))
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
                    "No '{{{0}}}' tag was found.".With
                    (
                        closeTagName
                    ),
                    templateLength
                );
            }

            signature += body + "{{{0}}}".With(closeTagName);

            var blocks = Blockify(body);
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
