namespace Nettle.Compiler
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Compiler.Validation;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a parsed Nettle template
    /// </summary>
    internal class Template
    {
        /// <summary>
        /// Constructs the template with the details
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
            
            if (blocks == null)
            {
                this.Blocks = new CodeBlock[] { };
            }
            else
            {
                this.Blocks = blocks;
            }
            
            this.Flags = FindAllFlags();
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
        /// Gets an array of template flags
        /// </summary>
        public TemplateFlag[] Flags { get; private set; }

        /// <summary>
        /// Determines if a specific flag has been set against the template
        /// </summary>
        /// <param name="flag">The flag</param>
        /// <returns>True, if the flag has been set; otherwise false</returns>
        public bool IsFlagSet
            (
                TemplateFlag flag
            )
        {
            return this.Flags.Contains(flag);
        }

        /// <summary>
        /// Finds all flags in the templates code blocks
        /// </summary>
        /// <returns>An array of template flags</returns>
        private TemplateFlag[] FindAllFlags()
        {
            var flagBlocks = FindBlocks<FlagDeclaration>();
            var flagsFound = new List<TemplateFlag>();

            foreach (var block in flagBlocks)
            {
                var enumFound = Enum.TryParse
                (
                    block.FlagName,
                    out TemplateFlag flag
                );

                if (false == enumFound)
                {
                    throw new NettleValidationException
                    (
                        "The flag {0} does not exist.".With
                        (
                            block.FlagName
                        )
                    );
                }
                else if (flagsFound.Contains(flag))
                {
                    throw new NettleValidationException
                    (
                        "The {0} flag was declared more than once.".With
                        (
                            block.FlagName
                        )
                    );
                }

                flagsFound.Add(flag);
            }

            return flagsFound.ToArray();
        }

        /// <summary>
        /// Finds all blocks of the code block type specified
        /// </summary>
        /// <typeparam name="T">The block type</typeparam>
        /// <returns>An array of matching code blocks</returns>
        public T[] FindBlocks<T>() 
            where T : CodeBlock
        {
            if (this.Blocks == null || this.Blocks.Length == 0)
            {
                return new T[] {};
            }
            else
            {
                return FindBlocks<T>
                (
                    this.Blocks
                );
            }
        }
        
        /// <summary>
        /// Finds all blocks of the code block type specified
        /// </summary>
        /// <typeparam name="T">The block type</typeparam>
        /// <returns>An array of matching code blocks</returns>
        private T[] FindBlocks<T>
            (
                CodeBlock[] blocks
            )

            where T : CodeBlock
        {
            Validate.IsNotNull(blocks);

            var matchingBlocks = new List<CodeBlock>();

            var blockFilterResults = blocks.Where
            (
                block => block.GetType() == typeof(T)
            )
            .Select
            (
                block => block as T
            );

            foreach (var block in blockFilterResults)
            {
                matchingBlocks.Add(block);

                var isNested = typeof(NestableCodeBlock).IsAssignableFrom
                (
                    typeof(T)
                );

                if (isNested)
                {
                    var nestedBlock = block as NestableCodeBlock;

                    if (nestedBlock.Blocks != null)
                    {
                        var matchingNestedBlocks = FindBlocks<T>
                        (
                            nestedBlock.Blocks
                        );

                        if (matchingNestedBlocks.Any())
                        {
                            matchingBlocks.AddRange
                            (
                                matchingNestedBlocks
                            );
                        }
                    }
                }
            }

            return blockFilterResults.ToArray();
        }
    }
}
