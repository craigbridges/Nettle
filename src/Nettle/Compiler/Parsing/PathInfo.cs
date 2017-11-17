namespace Nettle.Compiler.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents an aggregation of path information
    /// </summary>
    internal sealed class PathInfo
    {
        /// <summary>
        /// Constructs the path info with a path
        /// </summary>
        /// <param name="path">The binding path</param>
        public PathInfo
            (
                string path
            )
        {
            PopulatePathDetails(path);
        }

        /// <summary>
        /// Gets the full path set against the path info
        /// </summary>
        public string FullPath { get; private set; }

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
        public PathSegmentInfo[] Segments { get; private set; }

        /// <summary>
        /// Gets the segment at the index specified
        /// </summary>
        /// <param name="index">The segment index</param>
        /// <returns>The segment info</returns>
        public PathSegmentInfo this[int index]
        {
            get
            {
                return this.Segments[index];
            }
        }

        /// <summary>
        /// Removes a segment at the index specified
        /// </summary>
        /// <param name="index">The index</param>
        public void RemoveSegment
            (
                int index
            )
        {
            var segments = this.Segments.ToList();

            segments.RemoveAt(index);

            var pathBuilder = new StringBuilder();

            // Rebuild the path from the new segments list
            foreach (var segment in segments)
            {
                if (pathBuilder.Length > 0)
                {
                    pathBuilder.Append('.');
                }

                pathBuilder.Append
                (
                    segment.Signature
                );
            }

            PopulatePathDetails
            (
                pathBuilder.ToString()
            );
        }

        /// <summary>
        /// Populates the path details based on the binding path
        /// </summary>
        /// <param name="path">The binding path</param>
        private void PopulatePathDetails
            (
                string path
            )
        {
            var fullPath = path;
            var isValid = PathInfo.IsValidPath(path);

            if (false == isValid)
            {
                throw new ArgumentException
                (
                    "The path '{0}' is invalid.".With
                    (
                        path
                    )
                );
            }

            path = PathInfo.TrimPath(path);

            // Split the path into segments
            var segments = path.Split('.');
            var segmentInfos = new List<PathSegmentInfo>();
            var index = 0;

            foreach (var segment in segments)
            {
                segmentInfos.Add
                (
                    new PathSegmentInfo
                    (
                        index,
                        segment
                    )
                );
            }

            this.FullPath = fullPath;
            this.Segments = segmentInfos.ToArray();
            this.IsNested = segmentInfos.Count > 1;
        }

        /// <summary>
        /// Trims the path by removing leading dollar, dot and whitespace
        /// </summary>
        /// <param name="path">The path to trim</param>
        /// <returns>The trimmed path</returns>
        private static string TrimPath
            (
                string path
            )
        {
            path = path.Trim();

            // Strip leading dollar or dot characters
            if (path.StartsWith(@"$."))
            {
                return path;
            }
            else if (path.Length > 1 && path.First() == '$')
            {
                return path.Substring(1);
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
        public static bool IsValidPath
            (
                string path
            )
        {
            // Rule: the path must not contain spaces
            if (String.IsNullOrWhiteSpace(path) || path.Contains(" "))
            {
                return false;
            }

            // Rule: must start with letter or dollar sign
            var firstChar = path.First();

            var isValidChar =
            (
                Char.IsLetter(firstChar) || firstChar == '$'
            );

            if (false == isValidChar)
            {
                return false;
            }

            // Rule: sequential dots are not allowed
            if (path.Contains(".."))
            {
                return false;
            }

            path = PathInfo.TrimPath(path);

            // Rule: the path segments can end with an indexer
            var segments = path.Split('.');

            foreach (var segment in segments)
            {
                var isValid = PathSegmentInfo.IsValidSegment
                (
                    segment
                );

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
        public override string ToString()
        {
            return this.FullPath;
        }
    }
}
