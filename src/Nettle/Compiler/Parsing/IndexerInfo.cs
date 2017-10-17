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
        /// Constructs the inspector with a path
        /// </summary>
        /// <param name="path">The path</param>
        public IndexerInfo
            (
                string path
            )
        {
            Validate.IsNotEmpty(path);

            PopulateIndexerDetails(path);
        }

        /// <summary>
        /// Gets the full path set against the inspector
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
        /// Gets the indexers index pointer
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Populates the indexer details
        /// </summary>
        /// <param name="path">The path</param>
        private void PopulateIndexerDetails
            (
                string path
            )
        {
            var signature = ExtractIndexerSignature
            (
                path
            );

            if (String.IsNullOrEmpty(signature))
            {
                this.HasIndexer = false;
                this.Index = -1;
                this.PathWithoutIndexer = path;
            }
            else
            {
                var numberString = String.Empty;

                if (signature.Length > 2)
                {
                    numberString = signature.Crop
                    (
                        1,
                        signature.Length - 2
                    );
                }

                if (false == numberString.IsNumeric())
                {
                    var message = "The indexer for '{0}' must contain a number.".With
                    (
                        path
                    );

                    var position = (path.Length - signature.Length);

                    throw new NettleParseException
                    (
                        message,
                        position
                    );
                }

                this.HasIndexer = true;
                this.Index = Int32.Parse(numberString);

                this.PathWithoutIndexer = path.TrimEnd
                (
                    signature.ToArray()
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
