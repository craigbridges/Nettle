namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents a segment of a Nettle path
    /// </summary>
    internal sealed class NettlePathSegment
    {
        public NettlePathSegment(int index, string signature)
        {
            PopulateSegmentDetails(index, signature);
        }

        /// <summary>
        /// Gets the segments index number
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the segment signature (including the indexer)
        /// </summary>
        public string Signature { get; private set; } = default!;

        /// <summary>
        /// Gets the segment name (excluding the indexer)
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets a flag indicating if the segment is a model pointer
        /// </summary>
        /// <remarks>
        /// A model pointer is denoted using the dollar sign
        /// </remarks>
        public bool IsModelPointer { get; private set; }

        /// <summary>
        /// Gets the segments indexer information
        /// </summary>
        public Indexer IndexerInfo { get; private set; } = default!;

        /// <summary>
        /// Populates the segments details
        /// </summary>
        /// <param name="index">The segment index</param>
        /// <param name="signature">The segment signature</param>
        private void PopulateSegmentDetails(int index, string signature)
        {
            var isValid = IsValidSegment(signature);

            if (false == isValid)
            {
                throw new ArgumentException($"The path segment '{signature}' is invalid.");
            }

            var indexerInfo = new Indexer(signature);

            Index = index;
            Signature = signature;
            Name = indexerInfo.PathWithoutIndexer;
            IndexerInfo = indexerInfo;

            IsModelPointer = TemplateContext.IsModelReference(indexerInfo.PathWithoutIndexer);
        }

        /// <summary>
        /// Determines if the segment signature is valid
        /// </summary>
        /// <param name="signature">The segment signature</param>
        /// <returns>True, if it is valid; otherwise false</returns>
        /// <remarks>
        /// The segment signature is valid if it only contains 
        /// letters, numbers and optionally ends with an indexer.
        /// </remarks>
        public static bool IsValidSegment(string signature)
        {
            if (String.IsNullOrWhiteSpace(signature))
            {
                return false;
            }

            if (signature == @"$")
            {
                return true;
            }

            var indexerInfo = new Indexer(signature);

            signature = indexerInfo.PathWithoutIndexer;

            var containsValidChars = signature.All(c => Char.IsLetter(c) || Char.IsNumber(c));

            if (false == containsValidChars)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Provides a custom description of the path segment info
        /// </summary>
        /// <returns>The segment signature</returns>
        public override string ToString() => Signature;
    }
}
