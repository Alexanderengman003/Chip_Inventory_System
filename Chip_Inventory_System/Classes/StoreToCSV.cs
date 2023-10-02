using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public static class StoreToCSV
{
    public static void ExportDataTableToCsv(DataTable dataTable, List<string> columnsToInclude, string filePath)
    {
        try
        {
            StringBuilder sb = new StringBuilder();

            // Filter columns to include based on the columnsToInclude list
            IEnumerable<DataColumn> selectedColumns = dataTable.Columns.Cast<DataColumn>()
                .Where(column => columnsToInclude.Contains(column.ColumnName));

            // Write column headers to CSV
            sb.AppendLine(string.Join(";", selectedColumns.Select(column => column.ColumnName)));

            foreach (DataRow row in dataTable.Rows)
            {
                // Get values for selected columns and write to CSV
                IEnumerable<string> fields = selectedColumns.Select(column => row[column].ToString());
                sb.AppendLine(string.Join(";", fields));
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating CSV file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}