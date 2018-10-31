namespace Nettle.Data.Common.Serialization.Csv
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using Nettle.Common.Serialization.Grid;
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents a class for serializing a data grid into a CSV file
    /// </summary>
    public sealed class GridToCsvSerializer
    {
        /// <summary>
        /// Converts the data grid specified to a CSV new data string
        /// </summary>
        /// <param name="grid">The data grid binder to convert</param>
        /// <returns>A string that represents the data grid in CSV format</returns>
        public string Serialize
            (
                IDataGrid grid
            )
        {
            if (grid == null || grid.Count() == 0)
            {
                return String.Empty;
            }

            var configuration = new Configuration()
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                Quote = '"'
            };

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, configuration))
            {
                // Create the CSV headings
                grid.GetColumnNames().ToList().ForEach
                (
                    m => csv.WriteField(m)
                );

                csv.NextRecord();

                // Create a CSV record for every row in the data grid
                foreach (var row in grid)
                {
                    foreach (var cell in row)
                    {
                        // Get a string representation of the cells value
                        var value = 
                        (
                            cell.Value == null ? String.Empty : cell.Value.ToString()
                        );

                        csv.WriteField(value);
                    }

                    csv.NextRecord();
                }

                // Reset the memory stream and writers
                writer.Flush();
                stream.Position = 0;
                
                return new StreamReader(stream).ReadToEnd();
            }
        }
    }
}
