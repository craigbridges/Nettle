namespace Nettle.Data.Common.Serialization.Json
{
    using Nettle.Common.Serialization.Grid;
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a class for serializing a data grid into a JSON document
    /// </summary>
    public sealed class GridToJsonSerializer
    {
        /// <summary>
        /// Serializes a data grid to a JSON document
        /// </summary>
        /// <param name="grid">The data grid</param>
        /// <returns>A JSON string containing the grids data</returns>
        public string Serialize
            (
                IDataGrid grid
            )
        {
            if (grid == null || grid.Count() == 0)
            {
                return String.Empty;
            }

            var jsonBuilder = new StringBuilder();
            var rowCount = grid.Count();
            var columnCount = grid.GetColumnNames().Length;
            var rowNumber = 1;
            
            jsonBuilder.Append("{\n");

            jsonBuilder.Append
            (
                "\t\"{0}\": [\n".With
                (
                    grid.Name
                )
            );

            foreach (var row in grid)
            {
                var columnNumber = 1;

                jsonBuilder.Append("\t{\n");

                foreach (var cell in row)
                {
                    // Get a string representation of the cells value
                    var value = 
                    (
                        cell.Value == null ? String.Empty : cell.Value.ToString()
                    );

                    // Create a JSON property template
                    var template = 
                    (
                        (columnNumber < columnCount) 
                            ? "\t\t\"{0}\": \"{1}\",\n" 
                            : "\t\t\"{0}\": \"{1}\"\n"
                    );

                    // Add the column name and value to the JSON items properties
                    jsonBuilder.Append
                    (
                        template.With(cell.Key, value)
                    );

                    columnNumber++;
                }

                jsonBuilder.Append
                (
                    (rowNumber < rowCount) 
                        ? "\t},\n" 
                        : "\t}\n"
                );

                rowNumber++;
            }
            
            jsonBuilder.Append("]}");

            return jsonBuilder.ToString();
        }
    }
}
