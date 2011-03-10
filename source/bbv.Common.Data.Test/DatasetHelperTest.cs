//-------------------------------------------------------------------------------
// <copyright file="DatasetHelperTest.cs" company="bbv Software Services AG">
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
    using System;
    using System.Data;

    using NUnit.Framework;

    [TestFixture]
    public class DatasetHelperTest
    {
        [Test]
        public void ChangeColumns()
        {
            DataSet ds = new DataSet();
            DataTable t = ds.Tables.Add("Table");
            t.Columns.Add("int", typeof(int));
            t.Columns.Add("string", typeof(string));

            t.Rows.Add(new object[] { 1, DBNull.Value });
            t.Rows.Add(new object[] { 2, "zwei" });
            t.Rows.Add(new object[] { 3, "drei" });
            ds.AcceptChanges();
            t.Rows[1]["string"] = "modified";
            t.Rows[2].Delete();
            t.Rows.Add(new object[] { 4, "added" });
            
            Assert.AreEqual(0, DatasetHelper.GetChangedColumns(t.Rows[0]).Length, "no column should be changed.");
            Assert.AreEqual("string", DatasetHelper.GetChangedColumns(t.Rows[1])[0].ColumnName, "string column sould be changed.");
            Assert.AreEqual(2, DatasetHelper.GetChangedColumns(t.Rows[2]).Length, "all columns should be changed for a deleted row.");
            Assert.AreEqual(2, DatasetHelper.GetChangedColumns(t.Rows[3]).Length, "all columns should be changed for an added row.");
        }

        [Test]
        public void HasColumnChanged()
        {
            const string TableName = "Test";
            const string ColumnInt = "Int";
            const string ColumnString = "String";
            const string ColumnDateTime = "DateTime";
            DataSet ds = new DataSet();
            ds.Tables.Add(TableName);
            DataColumn column1 = ds.Tables[TableName].Columns.Add(ColumnInt, typeof(int));
            DataColumn column2 = ds.Tables[TableName].Columns.Add(ColumnString, typeof(string));
            DataColumn column3 = ds.Tables[TableName].Columns.Add(ColumnDateTime, typeof(DateTime));

            // add an empty row
            DataRow row = ds.Tables[TableName].Rows.Add(new object[] { 1, "Test", DateTime.Now });
            Assert.AreEqual(false, DatasetHelper.HasColumnChanged(new DataColumn[] { }, ds.Tables[TableName].Rows[0]));
            Assert.AreEqual(
                true, DatasetHelper.HasColumnChanged(new DataColumn[] { column1, column2 }, ds.Tables[TableName].Rows[0]));

            // AcceptChanges
            row.AcceptChanges();
            Assert.AreEqual(false, DatasetHelper.HasColumnChanged(new DataColumn[] { }, ds.Tables[TableName].Rows[0]));
            Assert.AreEqual(
                false, DatasetHelper.HasColumnChanged(new DataColumn[] { column1, column2 }, ds.Tables[TableName].Rows[0]));

            // Modify 
            row[column2] = "Hallo";
            row[column3] = new DateTime(2000, 1, 1);
            Assert.AreEqual(false, DatasetHelper.HasColumnChanged(new DataColumn[] { }, ds.Tables[TableName].Rows[0]));
            Assert.AreEqual(false, DatasetHelper.HasColumnChanged(new DataColumn[] { column1 }, ds.Tables[TableName].Rows[0]));
            Assert.AreEqual(
                true, DatasetHelper.HasColumnChanged(new DataColumn[] { column1, column2 }, ds.Tables[TableName].Rows[0]));

            // Delete 
            row.Delete();
            Assert.AreEqual(false, DatasetHelper.HasColumnChanged(new DataColumn[] { }, ds.Tables[TableName].Rows[0]));
            Assert.AreEqual(true, DatasetHelper.HasColumnChanged(new DataColumn[] { column1 }, ds.Tables[TableName].Rows[0]));
        }

        [Test]
        public void UpdateColumns()
        {
            DataSet ds = new DataSet();
            DataTable t = ds.Tables.Add("Table");
            t.Columns.Add("int", typeof(int));
            t.Columns.Add("string", typeof(string));

            t.Rows.Add(new object[] { 1, DBNull.Value });
            t.Rows.Add(new object[] { 1, "zwei" });
            DatasetHelper.UpdateColumnsIfDifferent(t.Rows[0], t.Rows[1], new string[] { "int", "string" });
            Assert.AreEqual(DBNull.Value, t.Rows[1]["string"]);
        }
    }
}
