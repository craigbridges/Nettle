namespace Nettle.Data.Common.Serialization.Xml
{
    using Nettle.Common.Serialization.Grid;
    using System;
    using System.Xml;

    /// <summary>
    /// Represents a class for serializing a data grid into an XML document
    /// </summary>
    public sealed class GridToXmlSerializer
    {
        /// <summary>
        /// Serializes a data grid to an XML document
        /// </summary>
        /// <param name="grid">The data grid</param>
        /// <returns>An XmlDocument containing the grids data</returns>
        public XmlDocument Serialize
            (
                IDataGrid grid
            )
        {
            if (grid == null)
            {
                return new XmlDocument();
            }

            var document = new XmlDocument();

            // The XML declaration is recommended, but not mandatory
            var xmlDeclaration = document.CreateXmlDeclaration
            (
                "1.0",
                "UTF-8",
                null
            );
            
            var rootNode = document.CreateElement
            (
                "DataGrid"
            );

            document.InsertBefore
            (
                xmlDeclaration,
                document.DocumentElement
            );

            foreach (var row in grid)
            {
                var rowNode = document.CreateElement
                (
                    "Item"
                );

                foreach (var cell in row)
                {
                    // Get a string representation of the cells value
                    var value = 
                    (
                        cell.Value == null ? String.Empty : cell.Value.ToString()
                    );

                    // Create the cell node and append it to the row node
                    var cellNode = document.CreateElement
                    (
                        cell.Key
                    );

                    cellNode.InnerText = value;
                    rowNode.AppendChild(cellNode);
                }

                rootNode.AppendChild(rowNode);
            }

            document.AppendChild(rootNode);

            return document;
        }
    }
}
