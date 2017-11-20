namespace Nettle.Compiler.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an aggregation of indexer information
    /// </summary>
    internal sealed class IndexerInfo
    {
        /// <summary>
        /// Constructs the indexer info with a path
        /// </summary>
        /// <param name="path">The binding path</param>
        public IndexerInfo
            (
                string path
            )
        {
            PopulateIndexerDetails(path);
        }

        /// <summary>
        /// Constructs the indexer info with a path and signature index
        /// </summary>
        /// <param name="path">The binding path</param>
        /// <param name="signatureIndex">The signature index</param>
        private IndexerInfo
            (
                string path,
                int signatureIndex
            )
        {
            PopulateIndexerDetails
            (
                path,
                signatureIndex
            );
        }

        /// <summary>
        /// Gets the full path set against the indexer info
        /// </summary>
        public string FullPath { get; private set; }

        /// <summary>
        /// Gets the path without the indexer segment
        /// </summary>
        public string PathWithoutIndexer { get; private set; }

        /// <summary>
        /// Gets a flag indicating if the path has an indexer
        /// </summary>
        public bool HasIndexer { get; private set; }

        /// <summary>
        /// Gets the indexer signature
        /// </summary>
        public string IndexerSignature { get; private set; }

        /// <summary>
        /// Gets the indexer value type
        /// </summary>
        public NettleValueType IndexerValueType { get; private set; }

        /// <summary>
        /// Gets the indexers resolved index pointer
        /// </summary>
        public int ResolvedIndex { get; private set; }

        /// <summary>
        /// Gets the next indexer chained to the current
        /// </summary>
        public IndexerInfo NextIndexer { get; private set; }

        /// <summary>
        /// Populates the indexer details based on the binding path
        /// </summary>
        /// <param name="path">The binding path</param>
        /// <param name="signatureIndex">THe signature index</param>
        private void PopulateIndexerDetails
            (
                string path,
                int signatureIndex = 0
            )
        {
            if (path.StartsWith(@".") && path.Length > 1)
            {
                path = path.Crop(1);
            }

            var extractedSignatures = ExtractIndexerSignatures
            (
                path
            );

            if (extractedSignatures.Length == 0)
            {
                this.HasIndexer = false;
                this.ResolvedIndex = -1;
                this.PathWithoutIndexer = path;
                this.FullPath = path;
            }
            else
            {
                var signature = extractedSignatures[signatureIndex];

                if (signature.Length > 2)
                {
                    signature = signature.Crop
                    (
                        1,
                        signature.Length - 2
                    );
                }

                var valueResolver = new NettleValueResolver();

                var valueType = valueResolver.ResolveType
                (
                    signature
                );

                switch (valueType)
                {
                    case NettleValueType.Number:
                    {
                        this.ResolvedIndex = Int32.Parse(signature);
                        break;
                    }
                    case NettleValueType.Variable:
                    {
                        this.ResolvedIndex = -1;
                        break;
                    }
                    default:
                    {
                        var message = "The indexer '{0}' for '{1}' is invalid.";
                        var position = (path.Length - signature.Length);

                        throw new NettleParseException
                        (
                            message.With(signature, path),
                            position
                        );
                    }
                }
                
                var indexerSequence = String.Empty;
                var trimSequence = String.Empty;

                for (var i = 0; i < extractedSignatures.Length; i++)
                {
                    indexerSequence += extractedSignatures[i];

                    if (i > signatureIndex)
                    {
                        trimSequence += extractedSignatures[i];
                    }
                }

                this.HasIndexer = true;
                this.IndexerValueType = valueType;
                this.IndexerSignature = signature;

                this.PathWithoutIndexer = path.Substring
                (
                    0,
                    path.Length - indexerSequence.Length
                );

                this.FullPath = path.Substring
                (
                    0,
                    path.Length - trimSequence.Length
                );

                if (signatureIndex < extractedSignatures.Length - 1)
                {
                    this.NextIndexer = new IndexerInfo
                    (
                        path,
                        signatureIndex + 1
                    );
                }
            }
        }
        
        /// <summary>
        /// Extracts the indexer signatures from the path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>An array of indexer signatures</returns>
        private string[] ExtractIndexerSignatures
            (
                string path
            )
        {
            if (false == path.EndsWith("]"))
            {
                return new string[] { };
            }
            else
            {
                var signatureList = new List<string>();
                var nextSignature = String.Empty;

                foreach (var c in path.Reverse())
                {
                    if (String.IsNullOrEmpty(nextSignature))
                    {
                        if (c != ']')
                        {
                            break;
                        }
                    }

                    nextSignature = nextSignature.Insert
                    (
                        0,
                        c.ToString()
                    );

                    if (c == '[')
                    {
                        signatureList.Add(nextSignature);
                        nextSignature = String.Empty;
                    }
                }

                signatureList.Reverse();

                return signatureList.ToArray();
            }
        }
    }
}
