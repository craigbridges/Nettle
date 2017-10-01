namespace Nettle.Common.Serialization.Grid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents an implementation of the IDataGrid contract
    /// </summary>
    public class DataGrid : IDataGrid
    {
        private string[] _columnNames;
        private List<IDataGridRow> _rows;

        /// <summary>
        /// Constructs a new data grid with an empty collection of columns and rows
        /// </summary>
        /// <param name="name">The name of the grid</param>
        public DataGrid(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException
                (
                    "A name value is required to create a data grid."
                );
            }

            this.Name = name;

            _rows = new List<IDataGridRow>();
        }

        /// <summary>
        /// Gets the data grids name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets an array of strings that represent all of the column names in the data grid
        /// </summary>
        /// <returns>An array of column names for the data grid</returns>
        public string[] GetColumnNames()
        {
            return _columnNames;
        }

        /// <summary>
        /// Determines if the data grid contains a column with the name specified
        /// </summary>
        /// <param name="columnName">The name of the column to check for</param>
        /// <returns>True, if the grid contains a matching column; otherwise false</returns>
        public bool HasColumn
            (
                string columnName
            )
        {
            if (_columnNames == null || _columnNames.Length == 0)
            {
                return false;
            }
            else
            {
                return _columnNames.Any
                (
                    m => m == columnName
                );
            }
        }

        /// <summary>
        /// Determines if the data grid contains all the columns with the names specified
        /// </summary>
        /// <param name="columnNames">The names of the columns to check for</param>
        /// <returns>True, if the grid contains matching columns; otherwise false</returns>
        public bool HasColumns
            (
                params string[] columnNames
            )
        {
            if (columnNames == null || columnNames.Length == 0)
            {
                throw new ArgumentException
                (
                    "At least one column name must be specified."
                );
            }

            return columnNames.All
            (
                m => HasColumn(m)
            );
        }

        /// <summary>
        /// Determines if the data grid contains any data (including any columns)
        /// </summary>
        /// <returns>True, if the data grid has data; otherwise false</returns>
        public bool HasData()
        {
            if (_columnNames == null || _columnNames.Length == 0)
            {
                return false;
            }
            else
            {
                return _rows.Any();
            }
        }

        /// <summary>
        /// Adds a new row of values to the data grids collection of rows
        /// </summary>
        /// <param name="values">A collection of key value pairs that represent the data in the row</param>
        public void AddRow
            (
                params KeyValuePair<string, object>[] values
            )
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException
                (
                    "The row must contain at least one value."
                );
            }

            // Extract a collection of column names
            var columnNames = values.Select(m => m.Key).Distinct();

            // Ensure there are no duplicate column names in the row values that were supplied
            if (columnNames.Count() != values.Length)
            {
                throw new InvalidOperationException
                (
                    "There are one or more duplicate column names. Column names must be unique."
                );
            }

            // Add the column names array if this is the row we are adding
            if (_rows.Count == 0)
            {
                _columnNames = columnNames.ToArray();
            }
            else
            {
                var columnsMatch = _columnNames.ToList().SequenceEqual
                (
                    columnNames.ToList()
                );

                // Make sure all the columns in the row values match the grids column name sequence
                if (false == columnsMatch)
                {
                    var newColumnList = _columnNames.ToList();

                    foreach (var name in columnNames)
                    {
                        if (false == newColumnList.Contains(name))
                        {
                            newColumnList.Add(name);
                        }
                    }

                    _columnNames = newColumnList.ToArray();
                }
            }

            var row = new DataGridRow
            (
                this,
                values
            );

            _rows.Add(row);
        }

        /// <summary>
        /// Adds a new row of values to the data grids collection of rows
        /// </summary>
        /// <param name="values">A collection of key value pairs that represent the data in the row</param>
        public void AddRow
            (
                params KeyValuePair<string, string>[] values
            )
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException
                (
                    "The row must contain at least one value."
                );
            }

            var convertedValues = new List<KeyValuePair<string, object>>();

            foreach (var item in values)
            {
                convertedValues.Add
                (
                    new KeyValuePair<string, object>
                    (
                        item.Key,
                        item.Value
                    )
                );
            }

            AddRow
            (
                convertedValues.ToArray()
            );
        }

        /// <summary>
        /// Removes an item from the row collection at the index specified
        /// </summary>
        /// <param name="index">The row index (zero based)</param>
        public void RemoveAt
            (
                int index
            )
        {
            if (index < 0 || index >= _rows.Count)
            {
                throw new ArgumentOutOfRangeException
                (
                    "The row index {0} is invalid.".With
                    (
                        index
                    )
                );
            }

            _rows.RemoveAt(index);
        }

        /// <summary>
        /// Gets a collection of the values in the data grid for the row index specified
        /// </summary>
        /// <param name="index">The row index (zero based)</param>
        /// <returns>A IDataGridRow row containing the column values</returns>
        public IDataGridRow GetRow
            (
                int index
            )
        {
            if (index < 0 || index >= _rows.Count)
            {
                throw new ArgumentOutOfRangeException
                (
                    "The row index {0} is invalid.".With
                    (
                        index
                    )
                );
            }

            return _rows.ElementAt(index);
        }

        /// <summary>
        /// Gets the data grid row at the index specified
        /// </summary>
        /// <param name="index">The row index (zero based)</param>
        /// <returns>A IDataGridRow row containing the column values</returns>
        public IDataGridRow this[int index]
        {
            get
            {
                return GetRow(index);
            }
        }

        /// <summary>
        /// Gets a single data grid cell value for the row and column indexes specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnIndex">The row column (zero based)</param>
        /// <returns>The cell value that matches the co-ordinates specified</returns>
        public object GetValue
            (
                int rowIndex,
                int columnIndex
            )
        {
            if (columnIndex < 0 || columnIndex >= _columnNames.Length)
            {
                throw new ArgumentOutOfRangeException
                (
                    "The column index {0} is invalid.".With
                    (
                        columnIndex
                    )
                );
            }

            var row = GetRow(rowIndex);

            return row[columnIndex];
        }

        /// <summary>
        /// Gets a single data grid cell value for the row and column indexes specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnIndex">The row column (zero based)</param>
        /// <returns>The cell value that matches the co-ordinates specified</returns>
        public object this[int rowIndex, int columnIndex]
        {
            get
            {
                return GetValue(rowIndex, columnIndex);
            }
            set
            {
                SetValue(rowIndex, columnIndex, value);
            }
        }

        /// <summary>
        /// Gets a single data grid cell value for the row index and column name values specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnName">The column name</param>
        /// <returns>The cell value that matches the row and column specified</returns>
        public object GetValue
            (
                int rowIndex,
                string columnName
            )
        {
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName");
            }

            if (false == _columnNames.Contains(columnName))
            {
                throw new KeyNotFoundException
                (
                    "No column was found with the name '{0}'.".With
                    (
                        columnName
                    )
                );
            }

            var row = GetRow(rowIndex);

            return row[columnName];
        }

        /// <summary>
        /// Gets a single data grid cell value for the row index and column name values specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnName">The column name</param>
        /// <returns>The cell value that matches the row and column specified</returns>
        public object this[int rowIndex, string columnName]
        {
            get
            {
                return GetValue(rowIndex, columnName);
            }
            set
            {
                SetValue(rowIndex, columnName, value);
            }
        }

        /// <summary>
        /// Sets the value of a row cell in the data grid to the new value specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <param name="value">The new value</param>
        public void SetValue
            (
                int rowIndex,
                int columnIndex,
                object value
            )
        {
            if (columnIndex < 0 || columnIndex >= _columnNames.Length)
            {
                throw new ArgumentOutOfRangeException
                (
                    "The column index {0} is invalid.".With
                    (
                        columnIndex
                    )
                );
            }

            GetRow(rowIndex)[columnIndex] = value;
        }

        /// <summary>
        /// Sets the value of a row cell in the data grid to the new value specified
        /// </summary>
        /// <param name="rowIndex">The row index (zero based)</param>
        /// <param name="columnName">The column name</param>
        /// <param name="value">The new value</param>
        public void SetValue
            (
                int rowIndex,
                string columnName,
                object value
            )
        {
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName");
            }

            if (false == _columnNames.Contains(columnName))
            {
                throw new KeyNotFoundException
                (
                    "No column was found with the name '{0}'.".With
                    (
                        columnName
                    )
                );
            }

            GetRow(rowIndex)[columnName] = value;
        }

        /// <summary>
        /// Merges the data grid specified into the current data grid instance
        /// </summary>
        /// <param name="grid">The data grid to merge</param>
        public void Merge
            (
                IDataGrid grid
            )
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            foreach (var row in grid)
            {
                var rowData = new Dictionary<string, object>();

                foreach (var item in row)
                {
                    rowData.Add(item.Key, item.Value);
                }

                AddRow
                (
                    rowData.ToArray()
                );
            }
        }

        /// <summary>
        /// Gets an enumerator for the collection of rows managed by the data grid
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<IDataGridRow> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        /// <summary>
        /// Gets a generic enumerator for the collection of rows managed by the data grid
        /// </summary>
        /// <returns>The generic enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Generates a tabbed representation of the data grid
        /// </summary>
        /// <returns>The data grids data, separated by line breaks and tabs</returns>
        public override string ToString()
        {
            if (false == this.HasData())
            {
                return String.Empty;
            }
            else
            {
                var builder = new StringBuilder();
                var columnNames = GetColumnNames();
                var columnCount = columnNames.Count();
                var columnNumber = 1;

                foreach (var column in columnNames)
                {
                    builder.Append(column);

                    if (columnNumber < columnCount)
                    {
                        builder.Append("\t");
                    }

                    columnNumber++;
                }
                
                foreach (var row in this)
                {
                    builder.Append("\r\n");

                    columnNumber = 1;

                    foreach (var column in row)
                    {
                        builder.Append(column.Value);

                        if (columnNumber < columnCount)
                        {
                            builder.Append("\t");
                        }

                        columnNumber++;
                    }
                }

                return builder.ToString();
            }
        }
    }
}
