namespace Nettle.Data.Common.Serialization.Csv
{
    using Nettle.Common.Serialization.Grid;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CsvHelper;
    using System.IO;

    /// <summary>
    /// Represents a class responsible for reading a CSV file into a data grid
    /// </summary>
    public sealed class CsvToGridSerializer
    {
        /// <summary>
        /// Reads the contents of a CSV file into a data grid
        /// </summary>
        /// <param name="filePath">The CSV file path</param>
        /// <returns>A data grid containing the CSV data</returns>
        public IDataGrid ReadCsvFile
            (
                string filePath
            )
        {
            Validate.IsNotEmpty(filePath);

            using (var textReader = File.OpenText(filePath))
            {
                var csv = new CsvReader(textReader);
                var grid = new DataGrid(filePath);
                
                while (csv.Read())
                {
                    var rowValues = new Dictionary<string, object>();

                    foreach (var header in csv.FieldHeaders)
                    {
                        rowValues[header] = csv.GetField
                        (
                            header
                        );
                    }

                    grid.AddRow
                    (
                        rowValues.ToArray()
                    );
                }

                return grid;
            }
        }
    }
}
