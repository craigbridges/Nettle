namespace Nettle
{
    using System;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Represents various extension methods for the XmlDocument class
    /// </summary>
    public static class XmlDocumentExtensions
    {
        /// <summary>
        /// Converts an XML document into a string representation
        /// </summary>
        /// <param name="document">The XML document</param>
        /// <returns>The stringified version</returns>
        public static string Stringify
            (
                this XmlDocument document
            )
        {
            if (document == null)
            {
                return String.Empty;
            }
            else
            {
                var sb = new StringBuilder();

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using (var writer = XmlWriter.Create(sb, settings))
                {
                    document.Save(writer);
                }

                return sb.ToString();
            }
        }
    }
}
