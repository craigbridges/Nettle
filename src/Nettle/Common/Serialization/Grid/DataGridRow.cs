namespace Nettle.Common.Serialization.Grid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents an implementation of the IDataGridRow contract
    /// </summary>
    public class DataGridRow : IDataGridRow
    {
        private Dictionary<string, object> _columnValues;

        /// <summary>
        /// Constructs a new data grid row with a new key-value collection
        /// </summary>
        /// <param name="grid">The data grid reference</param>
        /// <param name="values">The values to add to the row</param>
        protected internal DataGridRow
            (
                IDataGrid grid,
                params KeyValuePair<string, object>[] values
            )
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length == 0)
            {
                throw new ArgumentException
                (
                    "At least one value is required to create a data grid row."
                );
            }

            this.Grid = grid;

            _columnValues = new Dictionary<string, object>();

            // Add the column values to the row but also pad the row with blanks where values are missing
            foreach (var column in grid.GetColumnNames())
            {
                if (values.Any(m => m.Key == column))
                {
                    var matchingItem = values.First
                    (
                        m => m.Key.ToLower() == column.ToLower()
                    );

                    _columnValues.Add
                    (
                        column,
                        matchingItem.Value
                    );
                }
                else
                {
                    _columnValues.Add
                    (
                        column,
                        null
                    );
                }
            }
        }

        /// <summary>
        /// Gets a reference to the container data grid instance
        /// </summary>
        public IDataGrid Grid { get; private set; }

        /// <summary>
        /// Gets the value at the column index specified
        /// </summary>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <returns>The value, if found; otherwise null</returns>
        public object GetValue
            (
                int columnIndex
            )
        {
            if (columnIndex < 0 || columnIndex >= _columnValues.Count())
            {
                var message = "The column index {0} is invalid.";

                throw new ArgumentOutOfRangeException
                (
                    message.With(columnIndex)
                );
            }

            var element = _columnValues.ElementAt
            (
                columnIndex
            );

            return element.Value;
        }

        /// <summary>
        /// Gets the value at the column index specified
        /// </summary>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <returns>The column value</returns>
        public object this[int columnIndex]
        {
            get
            {
                return GetValue(columnIndex);
            }
            set
            {
                SetValue(columnIndex, value);
            }
        }

        /// <summary>
        /// Gets the value that matches the column name specified
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <returns>The value, if found; otherwise null</returns>
        public object GetValue
            (
                string columnName
            )
        {
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException
                (
                    "columnName"
                );
            }

            var columnExists = _columnValues.Any
            (
                m => m.Key.ToLower() == columnName.ToLower()
            );

            if (false == columnExists)
            {
                var message = "No column was found with the name '{0}'.";

                throw new KeyNotFoundException
                (
                    message.With(columnName)
                );
            }

            var entry = _columnValues.FirstOrDefault
            (
                m => m.Key.ToLower() == columnName.ToLower()
            );

            return entry.Value;
        }

        /// <summary>
        /// Gets the value that matches the column name specified
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <returns>The column value</returns>
        public object this[string columnName]
        {
            get
            {
                return GetValue(columnName);
            }
            set
            {
                SetValue(columnName, value);
            }
        }

        /// <summary>
        /// Sets the value of the column at the index specified to the new value
        /// </summary>
        /// <param name="columnIndex">The column index (zero based)</param>
        /// <param name="value">The value to set</param>
        public void SetValue
            (
                int columnIndex,
                object value
            )
        {
            if (columnIndex < 0 || columnIndex >= _columnValues.Count())
            {
                var message = "The column index {0} is invalid.";

                throw new ArgumentOutOfRangeException
                (
                    message.With(columnIndex)
                );
            }

            var element = _columnValues.ElementAt(columnIndex);
            var columnName = element.Key;

            SetValue(columnName, value);
        }

        /// <summary>
        /// Sets the value of the column matching the name specified to the new value
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="value">The value to set</param>
        public void SetValue
            (
                string columnName,
                object value
            )
        {
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName");
            }

            var columnNames = _columnValues.Select
            (
                m => m.Key
            );

            if (false == columnNames.Contains(columnName))
            {
                var message = "No column was found with the name '{0}'.";

                throw new KeyNotFoundException
                (
                    message.With(columnName)
                );
            }

            _columnValues[columnName] = new KeyValuePair<string, object>
            (
                columnName,
                value
            );
        }

        /// <summary>
        /// Gets an enumerator for the collection of rows managed by the data grid
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _columnValues.GetEnumerator();
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
            var builder = new StringBuilder();
            var columnNames = this.Grid.GetColumnNames();
            var columnCount = columnNames.Count();
            var columnNumber = 1;

            foreach (var column in this)
            {
                builder.Append(column.Value);

                if (columnNumber < columnCount)
                {
                    builder.Append("\t");
                }

                columnNumber++;
            }

            return builder.ToString();
        }
    }
}
