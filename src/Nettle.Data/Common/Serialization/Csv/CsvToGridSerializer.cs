﻿namespace Nettle.Data.Common.Serialization.Csv
{
    using CsvHelper;
    using Nettle.Common.Serialization.Grid;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a class for serializing a CSV file into a data grid
    /// </summary>
    public sealed class CsvToGridSerializer
    {
        /// <summary>
        /// Reads the contents of a CSV file into a data grid
        /// </summary>
        /// <param name="filePath">The CSV file path</param>
        /// <returns>A data grid containing the CSV data</returns>
        public static async Task<IDataGrid> ReadCsvFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);

            var grid = new DataGrid(filePath);

            while (await csv.ReadAsync())
            {
                var rowValues = new Dictionary<string, object?>();

                foreach (var header in csv.Context.Reader.HeaderRecord ?? Array.Empty<string>())
                {
                    rowValues[header] = csv.GetField(header);
                }

                grid.AddRow(rowValues.ToArray());
            }

            return grid;
        }
    }
}
