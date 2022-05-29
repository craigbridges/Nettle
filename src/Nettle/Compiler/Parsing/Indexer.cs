namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents an aggregation of indexer information
    /// </summary>
    internal sealed class Indexer
    {
        public Indexer(string path)
        {
            PopulateIndexerDetails(path);
        }

        private Indexer(string path, int signatureIndex)
        {
            PopulateIndexerDetails(path, signatureIndex);
        }

        /// <summary>
        /// Gets the full path set against the indexer info
        /// </summary>
        public string FullPath { get; private set; } = default!;

        /// <summary>
        /// Gets the path without the indexer segment
        /// </summary>
        public string PathWithoutIndexer { get; private set; } = default!;

        /// <summary>
        /// Gets a flag indicating if the path has an indexer
        /// </summary>
        public bool HasIndexer { get; private set; }

        /// <summary>
        /// Gets the indexer signature
        /// </summary>
        public string? IndexerSignature { get; private set; }

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
        public Indexer? NextIndexer { get; private set; }

        /// <summary>
        /// Populates the indexer details based on the binding path
        /// </summary>
        /// <param name="path">The binding path</param>
        /// <param name="signatureIndex">THe signature index</param>
        private void PopulateIndexerDetails(string path, int signatureIndex = 0)
        {
            if (path.StartsWith(@".") && path.Length > 1)
            {
                path = path.Crop(1);
            }

            var extractedSignatures = ExtractIndexerSignatures(path);

            if (extractedSignatures.Length == 0)
            {
                HasIndexer = false;
                ResolvedIndex = -1;
                PathWithoutIndexer = path;
                FullPath = path;
            }
            else
            {
                var signature = extractedSignatures[signatureIndex];

                if (signature.Length > 2)
                {
                    signature = signature.Crop(1, signature.Length - 2);
                }

                var valueType = NettleValueResolver.ResolveType(signature);

                switch (valueType)
                {
                    case NettleValueType.Number:
                    {
                        ResolvedIndex = Int32.Parse(signature);
                        break;
                    }
                    case NettleValueType.Variable:
                    {
                        ResolvedIndex = -1;
                        break;
                    }
                    default:
                    {
                        var position = (path.Length - signature.Length);

                        throw new NettleParseException
                        (
                            $"The indexer '{signature}' for '{path}' is invalid.",
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

                HasIndexer = true;
                IndexerValueType = valueType;
                IndexerSignature = signature;

                PathWithoutIndexer = path[..^indexerSequence.Length];
                FullPath = path[..^trimSequence.Length];

                if (signatureIndex < extractedSignatures.Length - 1)
                {
                    NextIndexer = new Indexer(path, signatureIndex + 1);
                }
            }
        }
        
        /// <summary>
        /// Extracts the indexer signatures from the path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>An array of indexer signatures</returns>
        private static string[] ExtractIndexerSignatures(string path)
        {
            if (false == path.EndsWith(']'))
            {
                return Array.Empty<string>();
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

                    nextSignature = nextSignature.Insert(0, c.ToString());

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
