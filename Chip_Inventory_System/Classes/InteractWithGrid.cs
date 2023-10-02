using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chip_Inventory_System
{
    internal class InteractWithGrid
    {

        public void ReadAndOutputSql(string connectionString, string sqlQuery, DataGridView dataGridView, List<string> columnsToExclude)
        {
            try
            {
                ReadWriteData readWriteData = new ReadWriteData();

                // Read data from the database
                DataTable dataTable = readWriteData.ReadFromSql(connectionString, sqlQuery);

                // Add datatable to grid
                dataGridView.DataSource = dataTable;

                // Remove columns based on the columnsToExclude list
                if (columnsToExclude != null)
                {
                    foreach (string columnName in columnsToExclude)
                    {
                        if (dataGridView.Columns.Contains(columnName))
                        {
                            dataGridView.Columns.Remove(columnName);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("An error occurred while reading and outputting data: " + ex.Message);
            }
        }

        public void ReadAndInputSql(string connectionString, string sqlQuery, string tableName)
        {
            try
            {
                // Create an instance of the ReadWriteData class
                ReadWriteData readWriteData = new ReadWriteData();

                // Read data from the database into a DataTable
                DataTable dataTable = readWriteData.ReadFromSql(connectionString, sqlQuery);

                // Write the DataTable to SQL using the WriteToSql method
                //readWriteData.WriteToSql(dataTable, tableName);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("An error occurred while reading and inputting data: " + ex.Message);
            }
        }

    }
}
