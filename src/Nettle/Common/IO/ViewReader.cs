namespace Nettle
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Provides a mechanism for reading views into memory
    /// </summary>
    public static class ViewReader
    {
        /// <summary>
        /// Reads a view into memory
        /// </summary>
        /// <param name="filePath">The views file path</param>
        /// <returns>The view</returns>
        public static NettleView Read
            (
                string filePath
            )
        {
            Validate.IsNotEmpty(filePath);

            if (false == File.Exists(filePath))
            {
                throw new IOException
                (
                    "The file path {0} does not exist.".With
                    (
                        filePath
                    )
                );
            }

            var name = GetViewName(filePath);
            var content = File.ReadAllText(filePath);

            var view = new NettleView
            (
                name,
                content
            );

            return view;
        }

        /// <summary>
        /// Reads all views found in a directory into memory
        /// </summary>
        /// <param name="path">The directory path</param>
        /// <returns>A collection of matching views</returns>
        /// <remarks>
        /// Nested directories are also searched for matching files
        /// </remarks>
        public static IEnumerable<NettleView> ReadAll
            (
                string path
            )
        {
            Validate.IsNotEmpty(path);

            if (false == Directory.Exists(path))
            {
                throw new IOException
                (
                    "The directory {0} does not exist.".With
                    (
                        path
                    )
                );
            }

            var searchPattern = "*.{0}".With
            (
                NettleEngine.ViewFileExtension
            );

            var matchingFiles = Directory.GetFiles
            (
                path,
                searchPattern,
                SearchOption.AllDirectories
            );

            foreach (var filePath in matchingFiles)
            {
                yield return Read(filePath);
            }
        }

        /// <summary>
        /// Gets the view name from a file path
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>The view name</returns>
        private static string GetViewName
            (
                string filePath
            )
        {
            Validate.IsNotEmpty(filePath);

            return Path.GetFileNameWithoutExtension
            (
                filePath
            );
        }
    }
}
