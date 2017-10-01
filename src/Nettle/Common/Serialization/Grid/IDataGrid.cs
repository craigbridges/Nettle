namespace Nettle.Common.Serialization.Grid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a contract for representing a collection of data in a grid structure
    /// </summary>
    public interface IDataGrid : IEnumerable<IDataGridRow>
    {
        /// <summary>
        /// Gets the data grids name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets an array of strings that represent all of the column names in the data grid
        /// </summary>
        /// <returns>An array of column names for the data grid</returns>
        string[] GetColumnNames();

        /// <summary>
        /// Determines if the data grid contains a column with the name specified
        /// </summary>
        /// <param name="columnName">The name of the column to check for</param>
        /// <returns>True, if the grid contains a matching column; otherwise false</returns>
        bool HasColumn
        (
            string columnName
        );

        /// <summary>
        /// Determines if the data grid contains all the columns with the names specified
        /// </summary>
        /// <param name="columnNames">The names of the columns to check for</param>
        /// <returns>True, if the grid contains matching columns; otherwise false</returns>
        bool HasColumns
        (
            params string[] columnNames
        );

        /// <summary>
        /// Determines if the data grid contains any data (including any columns)
        /// </summary>
        /// <returns>True, if the data grid has data; otherwise false</returns>
        bool HasData();

        /// <summary>
        /// Adds a new row of values to the data grids collection of rows
        /// </summary>
        /// <param name="values">A collection of key value pairs that represent the data in the row</param>
        void AddRow
        (
            params KeyValuePair<string, object>[] values
        );

        /// <summary>
        /// Removes an item from the row collection at the index specified
        /// </summary>
        /// <param name="index">The row index (zero based)</param>
        void RemoveAt
        (
            int index
        );

        /// <summary>
        /// Gets a collection of the values in the data grid for the row index specified
        /// </summary>
        /// <param name="index">The row index (zero based)</param>
        /// <returns>A IDataGridRow row containing the column values</returns>
        IDataGridRow GetRow
        (
            int index
        );

        /// <summary>
        /// Gets the data grid row at the index specified
        /// </summary>
        /// <param name="index">The row index (zero based)</param>
        /// <returns>A IDataGridRow row containing the column values</returns>
        IDataGridRow this[int index] { get; }

        /// <summary>
        /// Gets a single data grid cell value for the row and column indexes specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnIndex">The row column (zero based)</param>
        /// <returns>The cell value that matches the co-ordinates specified</returns>
        object GetValue
        (
            int rowIndex,
            int columnIndex
        );

        /// <summary>
        /// Gets a single data grid cell value for the row and column indexes specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <returns>The cell value that matches the co-ordinates specified</returns>
        object this[int rowIndex, int columnIndex] { get; set; }

        /// <summary>
        /// Gets a single data grid cell value for the row index and column name values specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnName">The column name</param>
        /// <returns>The cell value that matches the row and column specified</returns>
        object GetValue
        (
            int rowIndex,
            string columnName
        );

        /// <summary>
        /// Gets a single data grid cell value for the row index and column name values specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnName">The column name</param>
        /// <returns>The cell value that matches the row and column specified</returns>
        object this[int rowIndex, string columnName] { get; set; }

        /// <summary>
        /// Sets the value of a row cell in the data grid to the new value specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <param name="value">The new value</param>
        void SetValue
        (
            int rowIndex,
            int columnIndex,
            object value
        );

        /// <summary>
        /// Sets the value of a row cell in the data grid to the new value specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnName">The column name</param>
        /// <param name="value">The new value</param>
        void SetValue
        (
            int rowIndex,
            string columnName,
            object value
        );

        /// <summary>
        /// Merges the data grid specified into the current data grid instance
        /// </summary>
        /// <param name="grid">The data grid to merge</param>
        void Merge
        (
            IDataGrid grid
        );
    }
}
