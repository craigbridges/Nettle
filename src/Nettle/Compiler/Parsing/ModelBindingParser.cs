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
        /// - The first character must be either a letter or dollar sign
        /// - The signature can end with [*] indexers
        /// - Subsequent characters may be letters, dots, or numbers
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

            // Rule: must start with letter or dollar sign
            var firstChar = signatureBody.First();

            var isValidChar = 
            (
                Char.IsLetter(firstChar) 
                    || firstChar == '$'
            );

            if (false == isValidChar)
            {
                return false;
            }

            // Rule: the signature can end with an indexer
            var remainingBody = signatureBody.Substring(1);
            var indexerInfo = new IndexerInfo(remainingBody);
            
            if (indexerInfo.HasIndexer)
            {
                remainingBody = indexerInfo.PathWithoutIndexer;
            }

            // Rule: remaining characters must be letters, dots or numbers
            var containsValidChars = remainingBody.All
            (
                c => Char.IsLetter(c) 
                    || Char.IsNumber(c) 
                    || c == '.'
            );

            if (false == containsValidChars)
            {
                return false;
            }

            return true;
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
                BindingPath = bindingPath
            };
        }
    }
}
