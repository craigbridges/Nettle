namespace Nettle.Common.Serialization.Grid
{
    using Nettle.Data.Common.Serialization.Csv;
    using Nettle.Data.Common.Serialization.Xml;
    using System.Xml;

    /// <summary>
    /// Contains various extension methods for the DataGrid class
    /// </summary>
    public static class DataGridExtensions
    {
        /// <summary>
        /// Converts a data grid to a CSV string using the default CSV export options
        /// </summary>
        /// <param name="grid">The data grid to serialize</param>
        /// <returns>The CSV content generated</returns>
        public static string ToCsv
            (
                this IDataGrid grid
            )
        {
            return new GridToCsvSerializer().Serialize
            (
                grid
            );
        }

        /// <summary>
        /// Converts a data grid to an XML document representation
        /// </summary>
        /// <param name="grid">The data grid to serialize</param>
        /// <returns>The XML document generated</returns>
        public static XmlDocument ToXml
            (
                this IDataGrid grid
            )
        {
            return new GridToXmlSerializer().Serialize
            (
                grid
            );
        }

        ///// <summary>
        ///// Converts a data grid to a JSON string representation
        ///// </summary>
        ///// <param name="grid">The data grid to serialize</param>
        ///// <returns>The JSON content generated</returns>
        //public static string ToJson
        //    (
        //        this IDataGrid grid
        //    )
        //{
        //    return new GridToJsonSerializer().Serialize
        //    (
        //        grid
        //    );
        //}
    }
}
