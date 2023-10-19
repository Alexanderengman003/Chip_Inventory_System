using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;

namespace Chip_Inventory_System
{
    internal class ReadWriteData
    {
        private string connectionString = "Server=Ascilion006;Database=Chip_Inventory_Database;Integrated Security=True";

        public void ExportToCsv(string connectionString, string query, string filePath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the data
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Export all data to a CSV file
                        using (StreamWriter writer = new StreamWriter(filePath))
                        {
                            // Write the header row
                            string header = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName));
                            writer.WriteLine(header);

                            // Write the data rows
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                                writer.WriteLine(string.Join(",", fields));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void CreateNewRowSql(string tableName, DataTable dataTable)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Create a parameterized SQL INSERT command for each row
                        string insertCommand = $"INSERT INTO {tableName} ({string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(col => $"[{col.ColumnName}]"))}) " +
                                               $"VALUES ({string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(col => "@" + col.ColumnName.Replace(" ", "")))})";

                        using (SqlCommand sqlCommand = new SqlCommand(insertCommand, connection))
                        {
                            // Add parameters for each column value
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                // Remove spaces from parameter names, if needed.
                                sqlCommand.Parameters.AddWithValue("@" + column.ColumnName.Replace(" ", ""), row[column]);
                            }

                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void UpdateSqlTableWithMatchingIDs(string tableName, DataTable sourceTable)
        {
            if (!sourceTable.Columns.Contains("ID"))
            {
                Console.WriteLine("The source DataTable must have an 'ID' column for matching.");
                return;
            }

            try
            {
                Console.WriteLine($"Updating rows in table '{tableName}'...");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataRow sourceRow in sourceTable.Rows)
                    {
                        int id;
                        if (int.TryParse(sourceRow["ID"].ToString(), out id))
                        {
                            Console.WriteLine($"Updating row with ID: {id}");

                            // Generate an UPDATE statement for the matching ID
                            string updateCommand = $"UPDATE {tableName} SET ";

                            List<SqlParameter> parameters = new List<SqlParameter>();

                            foreach (DataColumn column in sourceTable.Columns)
                            {
                                if (column.ColumnName != "ID")
                                {
                                    // Remove spaces and special characters from column names
                                    string paramName = column.ColumnName.Replace(" ", "").Replace(".", "");
                                    updateCommand += $"[{column.ColumnName}] = @{paramName}, ";
                                    parameters.Add(new SqlParameter($"@{paramName}", sourceRow[column]));
                                }
                            }

                            // Remove the trailing comma and space
                            updateCommand = updateCommand.TrimEnd(' ', ',');

                            updateCommand += $" WHERE [ID] = {id}";

                            Console.WriteLine($"Update command: {updateCommand}");

                            using (SqlCommand sqlCommand = new SqlCommand(updateCommand, connection))
                            {
                                // Add the SqlParameter objects to the command
                                sqlCommand.Parameters.AddRange(parameters.ToArray());

                                int rowsAffected = sqlCommand.ExecuteNonQuery();
                                Console.WriteLine($"Rows affected: {rowsAffected}");
                            }
                        }
                        else
                        {
                            // Handle cases where the "ID" value cannot be converted to an int
                            Console.WriteLine($"Invalid 'ID' value: {sourceRow["ID"]}");
                        }
                    }
                }

                Console.WriteLine($"Update complete for table '{tableName}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the rows: " + ex.Message);
            }
        }

        public void WriteToSqlReplaceMultipleRows(string tableName, List<int> rowIDs, List<string> columnNames, List<object> newValues)
        {
            if (columnNames.Count != newValues.Count)
            {
                Console.WriteLine("The count of column names and new values should be equal.");
                return;
            }

            try
            {
                Console.WriteLine($"Updating rows in table '{tableName}'...");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    for (int i = 0; i < columnNames.Count; i++)
                    {
                        string columnName = columnNames[i];
                        object newValue = newValues[i];

                        string updateCommand = $"UPDATE {tableName} SET [{columnName}] = @NewValue WHERE [ID] IN ({string.Join(",", rowIDs)})";

                        using (SqlCommand sqlCommand = new SqlCommand(updateCommand, connection))
                        {
                            sqlCommand.Parameters.AddWithValue("@NewValue", newValue);

                            int rowsAffected = sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the rows: " + ex.Message);
            }
        }


        public void WriteToSqlReplace(string tableName, int rowID, string columnName, object newValue)
        {
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Create a SQL update command to set the new value
                        string updateCommand = $"UPDATE {tableName} SET [{columnName}] = @NewValue WHERE [ID] = @RowID"; // Assuming "ID" is the primary key column name

                        using (SqlCommand sqlCommand = new SqlCommand(updateCommand, connection))
                        {
                            sqlCommand.Parameters.AddWithValue("@NewValue", newValue);
                            sqlCommand.Parameters.AddWithValue("@RowID", rowID);

                            int rowsAffected = sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while updating the cell: " + ex.Message);
                }
            }
        }

        public DataTable ReadFromSql(string connectionString, string sqlQuery)
        {
            DataTable dataTable = new DataTable();
            try
            {
                // Create an instance of the DataAccess class
                var dataAccess = new DataAccess(connectionString);

                // Execute the query and retrieve results
                IEnumerable<dynamic> results = dataAccess.Query<dynamic>(sqlQuery);

                // If there are no results, return an empty DataTable
                if (results == null || !results.Any())
                    return dataTable;

                // Create columns in the DataTable based on the properties of the first result
                var firstResult = results.First();
                foreach (var property in ((IDictionary<string, object>)firstResult).Keys)
                {
                    dataTable.Columns.Add(property);
                }
                // Loop through the results and add rows to the DataTable
                foreach (var result in results)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var property in ((IDictionary<string, object>)result))
                    {
                        row[property.Key] = property.Value;
                    }
                    dataTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("An error occurred while reading the data: " + ex.Message);
            }

            return dataTable;
        }

        public DataTable CountDataInSql(string table1Query, string table2Query, string columnName, List<string> valuesToSearch)
        {
            // Read table1 data into a DataTable
            DataTable table1 = ReadFromSql(connectionString, table1Query);

            // Initialize a DataTable to store the results
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("Batch", typeof(string));
            resultTable.Columns.Add("Wafer", typeof(string));

            foreach (string valueToSearch in valuesToSearch)
            {
                resultTable.Columns.Add($"{valueToSearch} Count", typeof(int));
            }

            // Iterate through table1 to get unique Batch/Wafer combinations
            foreach (DataRow row in table1.Rows)
            {
                string batch = row["Batch"].ToString();
                string wafer = row["Wafer"].ToString();
                string batchAndWafer = $"{batch}/{wafer}";

                // Check if the combination is not already in the DataTable
                DataRow existingRow = resultTable.AsEnumerable()
                    .FirstOrDefault(r => r.Field<string>("Batch") == batch && r.Field<string>("Wafer") == wafer);

                if (existingRow == null)
                {
                    // Create a new row for the combination
                    DataRow newRow = resultTable.NewRow();
                    newRow["Batch"] = batch;
                    newRow["Wafer"] = wafer;

                    // Initialize counts for each value in valuesToSearch
                    foreach (string valueToSearch in valuesToSearch)
                    {
                        newRow[$"{valueToSearch} Count"] = 0;
                    }

                    resultTable.Rows.Add(newRow);
                }
            }

            // Iterate through table2 and count occurrences of valuesToSearch
            DataTable table2 = ReadFromSql(connectionString, table2Query);

            foreach (DataRow innerRow in table2.Rows)
            {
                string innerBatch = innerRow["Batch"].ToString();
                string innerWafer = innerRow["Wafer"].ToString();
                string innerValue = innerRow[columnName]?.ToString();

                DataRow existingRow = resultTable.AsEnumerable()
                    .FirstOrDefault(r => r.Field<string>("Batch") == innerBatch && r.Field<string>("Wafer") == innerWafer);

                if (existingRow != null && valuesToSearch.Contains(innerValue))
                {
                    int currentCount = existingRow[$"{innerValue} Count"] is int currentCountInt ? currentCountInt : 0;
                    existingRow[$"{innerValue} Count"] = currentCount + 1;
                }
            }
            return resultTable;
        }


        public DataTable ReadFromCsv(string filePath)
        {
            DataTable dataTable = new DataTable();

            try
            {
                if (File.Exists(filePath))
                {
                    // Create the DataTable with columns based on headers
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        if (!reader.EndOfStream)
                        {
                            string headerLine = reader.ReadLine();
                            string[] headers = headerLine.Split(',');

                            foreach (string header in headers)
                            {
                                dataTable.Columns.Add(header);
                            }
                        }

                        // Read data into the DataTable
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string[] values = line.Split(',');

                            // Create a new row and add it to the DataTable
                            DataRow dataRow = dataTable.NewRow();
                            for (int i = 0; i < values.Length; i++)
                            {
                                dataRow[i] = values[i];
                            }

                            dataTable.Rows.Add(dataRow);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("File not found: " + filePath);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("An error occurred while reading the CSV data: " + ex.Message);
            }
            return dataTable;
        }
    }

}

