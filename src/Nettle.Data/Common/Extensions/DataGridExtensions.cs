namespace Nettle.Common.Serialization.Grid
{
    using Nettle.Data.Common.Serialization.Csv;
    using Nettle.Data.Common.Serialization.Json;
    using Nettle.Data.Common.Serialization.Xml;
    using System.Xml;

    public static class DataGridExtensions
    {
        /// <summary>
        /// Converts a data grid to a CSV string using the default CSV export options
        /// </summary>
        /// <param name="grid">The data grid to serialize</param>
        /// <returns>The CSV content generated</returns>
        public static string ToCsv(this IDataGrid grid) => GridToCsvSerializer.Serialize(grid);

        /// <summary>
        /// Converts a data grid to an XML document representation
        /// </summary>
        /// <param name="grid">The data grid to serialize</param>
        /// <returns>The XML document generated</returns>
        public static XmlDocument ToXml(this IDataGrid grid) => GridToXmlSerializer.Serialize(grid);

        /// <summary>
        /// Converts a data grid to a JSON string representation
        /// </summary>
        /// <param name="grid">The data grid to serialize</param>
        /// <returns>The JSON content generated</returns>
        public static string ToJson(this IDataGrid grid) => GridToJsonSerializer.Serialize(grid);
    }
}
