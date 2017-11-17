namespace Nettle.Compiler.Parsing
{
    using System;
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
        /// Populates the indexer details based on the binding path
        /// </summary>
        /// <param name="path">The binding path</param>
        private void PopulateIndexerDetails
            (
                string path
            )
        {
            if (path.StartsWith(@".") && path.Length > 1)
            {
                path = path.Crop(1);
            }

            var signature = ExtractIndexerSignature
            (
                path
            );

            if (String.IsNullOrEmpty(signature))
            {
                this.HasIndexer = false;
                this.ResolvedIndex = -1;
                this.PathWithoutIndexer = path;
            }
            else
            {
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
                
                var wrappedSignature = "[{0}]".With
                (
                    signature
                );

                this.HasIndexer = true;
                this.IndexerValueType = valueType;

                this.PathWithoutIndexer = path.Substring
                (
                    0,
                    path.Length - wrappedSignature.Length
                );
            }

            this.FullPath = path;
            this.IndexerSignature = signature;
        }
        
        /// <summary>
        /// Extracts the indexer signature from the path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The indexer</returns>
        private string ExtractIndexerSignature
            (
                string path
            )
        {
            if (false == path.EndsWith("]"))
            {
                return String.Empty;
            }
            else
            {
                var signature = String.Empty;

                foreach (var c in path.Reverse())
                {
                    signature = signature.Insert
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
    }
}
