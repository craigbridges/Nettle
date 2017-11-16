namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Compiler.Parsing.Conditions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an 'if' statement code block parser
    /// </summary>
    internal sealed class IfStatementParser : NestedBlockParser
    {
        private BooleanExpressionParser _expressionParser;

        /// <summary>
        /// Constructs the parser with a blockifier
        /// </summary>
        /// <param name="blockifier">The blockifier</param>
        public IfStatementParser
            (
                IBlockifier blockifier
            )

            : base(blockifier)
        {
            _expressionParser = new BooleanExpressionParser();
        }
        
        /// <summary>
        /// Gets the tag name
        /// </summary>
        protected override string TagName
        {
            get
            {
                return "if";
            }
        }

        /// <summary>
        /// Provides the else statement tag names
        /// </summary>
        protected override string[] BodyPartitionTagNames
        {
            get
            {
                return new string[]
                {
                    "else if ",
                    "else"
                };
            }
        }

        /// <summary>
        /// Parses the 'if' statement signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The 'if' signature</param>
        /// <returns>The parsed if statement</returns>
        public override CodeBlock Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var signatureBody = UnwrapSignatureBody
            (
                signature
            );

            var conditionSignature = signatureBody.RightOf
            (
                "{0} ".With
                (
                    this.TagName
                )
            );

            if (String.IsNullOrWhiteSpace(conditionSignature))
            {
                throw new NettleParseException
                (
                    "The if statements condition must be specified.",
                    positionOffSet
                );
            }

            var expression = _expressionParser.Parse
            (
                conditionSignature
            );

            var ifContent = ExtractNestedBody
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            var ifSignature = ifContent.Signature;
            var elseIfBlocks = new List<ElseIfStatement>();
            var elseContent = default(NestableCodeBlock);
            var elseFindCount = 0;

            var elseFound = templateContent.StartsWith
            (
                @"{{else"
            );

            while (elseFound)
            {
                if (templateContent.StartsWith(@"{{else if "))
                {
                    var elseSignature = String.Empty;
                    var closureFound = false;

                    // Scan until the tag ending }} brackets are reached
                    foreach (var c in templateContent)
                    {
                        elseSignature += c;

                        if (elseSignature.EndsWith(@"}}"))
                        {
                            closureFound = true;
                            break;
                        }
                    }

                    if (false == closureFound)
                    {
                        var message = "The else if tag '{0}' is invalid.";
                        
                        throw new NettleParseException
                        (
                            message.With(elseSignature),
                            positionOffSet
                        );
                    }

                    // Unwrap the 'else if' signature body
                    var elseSignatureBody = UnwrapSignatureBody
                    (
                        elseSignature
                    );

                    // Extract the boolean expression and parse
                    var elseConditionSignature = elseSignatureBody.RightOf
                    (
                        "else if "
                    );

                    var elseExpression = _expressionParser.Parse
                    (
                        elseConditionSignature
                    );

                    // Extract the 'else if' body content
                    var elseIfContent = ExtractNestedBody
                    (
                        ref templateContent,
                        ref positionOffSet,
                        elseSignature
                    );

                    var elseIfBlock = new ElseIfStatement()
                    {
                        ConditionExpression = elseExpression,
                        Body = elseIfContent.Body,
                        Blocks = elseIfContent.Blocks,
                        Signature = elseIfContent.Signature,
                        StartPosition = elseIfContent.StartPosition,
                        EndPosition = elseIfContent.EndPosition
                    };

                    elseIfBlocks.Add(elseIfBlock);

                    ifSignature += elseIfContent.Signature;
                }
                else
                {
                    // Ensure only one else block is defined
                    if (elseFindCount > 0)
                    {
                        throw new NettleParseException
                        (
                            "Only one else tag is allowed.",
                            positionOffSet
                        );
                    }

                    // Ensure it's a valid else block
                    if (false == templateContent.StartsWith(@"{{else}}"))
                    {
                        throw new NettleParseException
                        (
                            "The else tag is invalid.",
                            positionOffSet
                        );
                    }

                    elseContent = ExtractNestedBody
                    (
                        ref templateContent,
                        ref positionOffSet,
                        @"{{else}}"
                    );

                    elseContent.EndPosition = 
                    (
                        elseContent.StartPosition + elseContent.Signature.Length
                    );

                    elseFindCount++;
                    ifSignature += elseContent.Signature;
                }

                elseFound = templateContent.StartsWith
                (
                    @"{{else"
                );
            }

            var ifEndPosition = 
            (
                ifContent.StartPosition + ifSignature.Length
            );

            return new IfStatement()
            {
                Signature = ifSignature,
                StartPosition = ifContent.StartPosition,
                EndPosition = ifEndPosition,
                ConditionExpression = expression,
                Body = ifContent.Body,
                Blocks = ifContent.Blocks,
                ElseIfConditions = elseIfBlocks,
                ElseContent = elseContent
            };
        }
    }
}
