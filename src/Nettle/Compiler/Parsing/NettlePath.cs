namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents a full Nettle path
    /// </summary>
    internal sealed class NettlePath
    {
        public NettlePath(string path)
        {
            PopulatePathDetails(path);
        }

        /// <summary>
        /// Gets the full path set against the path info
        /// </summary>
        public string FullPath { get; private set; } = default!;

        /// <summary>
        /// Gets a flag indicating if the path is nested
        /// </summary>
        /// <remarks>
        /// The path is nested if it has more than one segment.
        /// </remarks>
        public bool IsNested { get; private set; }

        /// <summary>
        /// Gets an array of segments in the path
        /// </summary>
        public NettlePathSegment[] Segments { get; private set; } = Array.Empty<NettlePathSegment>();

        /// <summary>
        /// Gets the segment at the index specified
        /// </summary>
        /// <param name="index">The segment index</param>
        /// <returns>The segment info</returns>
        public NettlePathSegment this[int index] => Segments[index];

        /// <summary>
        /// Removes a segment at the index specified
        /// </summary>
        /// <param name="index">The index</param>
        public void RemoveSegment(int index)
        {
            var segments = Segments.ToList();

            segments.RemoveAt(index);

            var pathBuilder = new StringBuilder();

            // Rebuild the path from the new segments list
            foreach (var segment in segments)
            {
                if (pathBuilder.Length > 0)
                {
                    pathBuilder.Append('.');
                }

                pathBuilder.Append(segment.Signature);
            }

            PopulatePathDetails(pathBuilder.ToString());
        }

        /// <summary>
        /// Populates the path details based on the binding path
        /// </summary>
        /// <param name="path">The binding path</param>
        private void PopulatePathDetails(string path)
        {
            var fullPath = path;
            var isValid = IsValidPath(path);

            if (false == isValid)
            {
                throw new ArgumentException($"The path '{path}' is invalid.");
            }

            path = TrimPath(path);

            // Split the path into segments
            var segments = path.Split('.');
            var segmentInfos = new List<NettlePathSegment>();
            var index = 0;

            foreach (var segment in segments)
            {
                segmentInfos.Add(new NettlePathSegment(index, segment));
            }

            FullPath = fullPath;
            Segments = segmentInfos.ToArray();
            IsNested = segmentInfos.Count > 1;
        }

        /// <summary>
        /// Trims the path by removing leading dollar, dot and whitespace
        /// </summary>
        /// <param name="path">The path to trim</param>
        /// <returns>The trimmed path</returns>
        private static string TrimPath(string path)
        {
            path = path.Trim();

            // Strip leading dollar or dot characters
            if (path.StartsWith(@"$."))
            {
                return path;
            }
            else if (path.Length > 1 && path.First() == '$')
            {
                return path[1..];
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// Determines if a binding path is valid
        /// </summary>
        /// <param name="path">The binding path</param>
        /// <returns>True, if the path is valid; otherwise false</returns>
        /// <remarks>
        /// The rules for a valid binding path are as follows:
        /// 
        /// - The trimmed path must not contain whitespace
        /// - The first character must be either a letter or dollar sign
        /// - The path segments can end with [*] indexers
        /// - Subsequent characters may be letters, dots, square brackets or numbers
        /// - Sequential dots are not allowed (e.g. "..")
        /// </remarks>
        public static bool IsValidPath(string path)
        {
            // Rule: the path must not contain spaces
            if (String.IsNullOrWhiteSpace(path) || path.Contains(' '))
            {
                return false;
            }

            // Rule: must start with letter or dollar sign
            var firstChar = path.First();
            var isValidChar = Char.IsLetter(firstChar) || firstChar == '$';

            if (false == isValidChar)
            {
                return false;
            }

            // Rule: sequential dots are not allowed
            if (path.Contains(".."))
            {
                return false;
            }

            path = TrimPath(path);

            // Rule: the path segments can end with an indexer
            var segments = path.Split('.');

            foreach (var segment in segments)
            {
                var isValid = NettlePathSegment.IsValidSegment(segment);

                if (false == isValid)
                {
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// Provides a custom description of the path info
        /// </summary>
        /// <returns>The full path</returns>
        public override string ToString() => FullPath;
    }
}
