//-------------------------------------------------------------------------------
// <copyright file="DatasetHelper.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace bbv.Common.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Contains static methods helping to work with Datasets
    /// </summary>
    public static class DatasetHelper
    {
        /// <summary>
        /// Creates a new DataTable from the rows currently active in the given view.
        /// </summary>
        /// <param name="dataView">DataView to create the table from.</param>
        /// <returns>
        /// Returns a new DataTable with the same schema as the table, 
        /// which the view is based on.
        /// </returns>
        public static DataTable CreateTableFromView(DataView dataView)
        {
            DataTable dataTable = dataView.Table.Clone();
            foreach (DataRowView rowView in dataView)
            {
                dataTable.ImportRow(rowView.Row);
            }

            return dataTable;
        }

        /// <summary>
        /// Creates a DataView showing the same entries as the original View
        /// </summary>
        /// <param name="originalView">the original view from which the new View is created</param>
        /// <returns>the created new View</returns>
        public static DataView CloneView(DataView originalView)
        {
            DataView newView = new DataView(originalView.Table)
                                   {
                                       Sort = originalView.Sort,
                                       RowFilter = originalView.RowFilter,
                                       RowStateFilter = originalView.RowStateFilter
                                   };
            return newView;
        }
        
        /// <summary>
        /// Returns the columns that were changed in the row.
        /// </summary>
        /// <param name="row">the row to be examined</param>
        /// <returns>the changed columns</returns>
        public static DataColumn[] GetChangedColumns(DataRow row)
        {
            List<DataColumn> changedColumns = new List<DataColumn>(row.Table.Columns.Count);

            switch (row.RowState)
            {
                case DataRowState.Unchanged:
                    break; // nothing has changed

                case DataRowState.Modified:
                    // find out what has changed
                    foreach (DataColumn column in row.Table.Columns)
                    {
                        if (!row[column, DataRowVersion.Original].Equals(row[column, DataRowVersion.Current]))
                        {
                            changedColumns.Add(column);
                        }
                    }

                    break;

                case DataRowState.Added:
                case DataRowState.Deleted:
                    // everything has changed
                    foreach (DataColumn column in row.Table.Columns)
                    {
                        changedColumns.Add(column);
                    }

                    break;
            }

            return changedColumns.ToArray();
        }

        /// <summary>
        /// Checks if certain Columns in a row have a changed value.
        /// </summary>
        /// <param name="columns">a list of the columns to be checked</param>
        /// <param name="row">the row to be analyzed</param>
        /// <returns>true if any of that columns in the row has changed</returns>
        public static bool HasColumnChanged(DataColumn[] columns, DataRow row)
        {
            ArrayList columnArray = new ArrayList(GetChangedColumns(row));
            
            foreach (DataColumn dataColumn in columns)
            {
                if (columnArray.Contains(dataColumn))
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Updates columns in <paramref name="columnsToUpdate"/> of <paramref name="destinationRow"/>
        /// that are different from <paramref name="sourceRow"/>. 
        /// </summary>
        /// <param name="sourceRow">Row to compare with</param>
        /// <param name="destinationRow">Ro to update</param>
        /// <param name="columnsToUpdate">Columns to verify</param>
        public static void UpdateColumnsIfDifferent(DataRow sourceRow, DataRow destinationRow, string[] columnsToUpdate)
        {
            List<string> columns = new List<string>(columnsToUpdate);
            
            foreach (DataColumn column in destinationRow.Table.Columns)
            {
                if (sourceRow.Table.Columns.Contains(column.ColumnName) && columns.Contains(column.ColumnName))
                {
                    if (destinationRow[column.ColumnName] != sourceRow[column.ColumnName])
                    {
                        destinationRow[column.ColumnName] = sourceRow[column.ColumnName];
                    }
                }
            }
        }
    }
}
