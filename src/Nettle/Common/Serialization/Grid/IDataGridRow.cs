namespace Nettle.Common.Serialization.Grid
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a contract for representing a row that manages a collection of data grid cells
    /// </summary>
    public interface IDataGridRow : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Gets a reference to the container data grid instance
        /// </summary>
        IDataGrid Grid { get; }

        /// <summary>
        /// Gets the value at the column index specified
        /// </summary>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <returns>The value, if found; otherwise null</returns>
        object GetValue
        (
            int columnIndex
        );

        /// <summary>
        /// Gets the value at the column index specified
        /// </summary>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <returns>The column value</returns>
        object this[int columnIndex] { get; set; }

        /// <summary>
        /// Gets the value that matches the column name specified
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <returns>The value, if found; otherwise null</returns>
        object GetValue
        (
            string columnName
        );

        /// <summary>
        /// Gets the value that matches the column name specified
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <returns>The column value</returns>
        object this[string columnName] { get; set; }

        /// <summary>
        /// Sets the value of the column at the index specified to the new value
        /// </summary>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <param name="value">The value to set</param>
        void SetValue
        (
            int columnIndex,
            object value
        );

        /// <summary>
        /// Sets the value of the column matching the name specified to the new value
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="value">The value to set</param>
        void SetValue
        (
            string columnName,
            object value
        );
    }
}
