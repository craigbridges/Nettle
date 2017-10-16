namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a model binding code block parser
    /// </summary>
    internal sealed class ModelBindingParser : NettleParser, IBlockParser
    {
        /// <summary>
        /// Determines if a signature matches the block type of the parser
        /// </summary>
        /// <param name="signatureBody">The signature body</param>
        /// <returns>True, if it matches; otherwise false</returns>
        /// <remarks>
        /// The rules for matching a model binding are as follows:
        /// 
        /// - The trimmed signature body must not contain spaces
        /// - The first character must be either a letter, underscore or dollar sign
        /// - The signature can end with [*] indexers
        /// - Subsequent characters may be letters, underscore, dots, or numbers
        /// </remarks>
        public bool Matches
            (
                string signatureBody
            )
        {
            signatureBody = signatureBody.Trim();

            // Rule: must not contain spaces
            if (signatureBody.Contains(" "))
            {
                return false;
            }

            var firstChar = signatureBody.First();

            var isValidChar = 
            (
                Char.IsLetter(firstChar) 
                    || firstChar == '_'
                    || firstChar == '$'
            );

            // Rule: must start with letter, underscore or dollar sign
            if (false == isValidChar)
            {
                return false;
            }

            var remainingBody = signatureBody.Substring(1);
            var hasIndexer = HasIndexer(signatureBody);

            // Rule: the signature can end with an indexer
            if (hasIndexer)
            {
                var indexerSignature = ExtractIndexerSignature
                (
                    signatureBody
                );

                remainingBody = remainingBody.TrimEnd
                (
                    indexerSignature.ToArray()
                );
            }

            // Rule: remaining characters must be letter, underscore, dots or numbers
            var containsValidChars = remainingBody.All
            (
                c => Char.IsLetter(c) 
                    || Char.IsNumber(c) 
                    || c == '_' 
                    || c == '.'
            );

            if (false == containsValidChars)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if the binding path has an indexer
        /// </summary>
        /// <param name="bindingPath">The binding path</param>
        /// <returns>True, if the path has an indexer; otherwise false</returns>
        private bool HasIndexer
            (
                string bindingPath
            )
        {
            var indexer = ExtractIndexerSignature
            (
                bindingPath
            );

            return 
            (
                false == String.IsNullOrEmpty(indexer)
            );
        }

        /// <summary>
        /// Extracts the indexer signature from the binding path
        /// </summary>
        /// <param name="bindingPath">The binding path</param>
        /// <returns>The indexer</returns>
        private string ExtractIndexerSignature
            (
                string bindingPath
            )
        {
            if (false == bindingPath.EndsWith("]"))
            {
                return String.Empty;
            }
            else
            {
                var signature = String.Empty;

                foreach (var c in bindingPath.Reverse())
                {
                    signature  = signature.Insert
                    (
                        0,
                        c.ToString()
                    );

                    if (c == '[')
                    {
                        break;
                    }
                }

                return signature;
            }
        }

        /// <summary>
        /// Parses the code block signature into a code block object
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public CodeBlock Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var bindingValue = NettleValueType.ModelBinding.ParseValue
            (
                signature
            );

            var bindingPath = bindingValue.ToString();
            var hasIndexer = HasIndexer(bindingPath);
            var index = default(int);

            if (hasIndexer)
            {
                var indexerSignature = ExtractIndexerSignature
                (
                    bindingPath
                );

                var numberString = String.Empty;

                if (indexerSignature.Length > 2)
                {
                    numberString = indexerSignature.Crop
                    (
                        1,
                        indexerSignature.Length - 2
                    );
                }

                if (false == numberString.IsNumeric())
                {
                    var message = "The indexer for '{0}' must contain a number.".With
                    (
                        bindingPath
                    );

                    throw new NettleParseException
                    (
                        message,
                        positionOffSet
                    );
                }
                else
                {
                    index = Int32.Parse(numberString);
                }

                bindingPath = bindingPath.TrimEnd
                (
                    indexerSignature.ToArray()
                );
            }

            var startPosition = positionOffSet;
            var endPosition = startPosition + (signature.Length - 1);

            TrimTemplate
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new ModelBinding()
            {
                Signature = signature,
                StartPosition = startPosition,
                EndPosition = endPosition,
                BindingPath = bindingPath,
                HasIndexer = hasIndexer,
                Index = index
            };
        }
    }
}
