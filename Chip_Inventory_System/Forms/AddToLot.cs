using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Chip_Inventory_System
{
    public partial class AddToLot : Form
    {
        private readonly InteractWithGrid interactWithGrid;
        string destinationTableLot = "Lot_list";
        private Form1 parentForm;
        private readonly ReadWriteData readWriteData; // Declare the field
        private DataTable changedRowsDataTable;
        private Stopwatch stopwatch;

        // Constructor that takes a DataTable as an argument
        public AddToLot(Form1 parentForm)
        {
            InitializeComponent();
            InitializeAdvancedDataGridView();
            this.parentForm = parentForm;
            readWriteData = new ReadWriteData(); // Initialize the field
            interactWithGrid = new InteractWithGrid(); // Initialize the field
            stopwatch = new Stopwatch();
        }

        private void InitializeAdvancedDataGridView()
        {
            adgvLotList.AllowUserToResizeRows = false;
            adgvLotList.AllowUserToResizeColumns = false;
            adgvLotList.AutoGenerateColumns = true; // Automatically generate columns
            adgvLotList.Dock = DockStyle.Fill;
            adgvLotList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void updateGrid()
        {
            try
            {
                // Read data tables from SQL
                DataTable chipList = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryChip);
                DataTable waferList = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryWafer);
                DataTable lotList = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryLotList);

                // Initialize dictionaries to store counts
                Dictionary<string, int> countGood = new Dictionary<string, int>();
                Dictionary<string, int> countFail = new Dictionary<string, int>();
                Dictionary<string, int> countPicked = new Dictionary<string, int>();
                Dictionary<string, int> countLot = new Dictionary<string, int>();

                List<string> valuesToSearchLotList = lotList.AsEnumerable()
                    .Select(row => row.Field<string>("Lot"))
                    .Distinct()
                    .ToList();

                string columnNameLot = "Lot";

                // Create a new DataTable to store rows with changed 'Current qty'
                DataTable changedRowsDataTable = lotList.Clone();

                // Create a dictionary to store the initial sum of 'Current qty' for each 'Lot' value
                Dictionary<string, int> initialSumByLot = new Dictionary<string, int>();

                // Calculate the initial sum of 'Current qty' for each 'Lot' value
                foreach (string valueToCount in valuesToSearchLotList)
                {
                    int initialSum = lotList.AsEnumerable()
                        .Where(row => row.Field<string>(columnNameLot) == valueToCount)
                        .Sum(row => int.Parse(row["Current qty"].ToString()));

                    initialSumByLot[valueToCount] = initialSum;
                }

                foreach (string valueToCount in valuesToSearchLotList)
                {
                    int count = chipList.AsEnumerable()
                        .Count(row => row.Field<string>(columnNameLot) == valueToCount);

                    countLot[valueToCount] = count; // Store the count in the dictionary

                    // Update the "Current qty" column in lotListDataTable
                    DataRow[] matchingRows = lotList.Select($"Lot = '{valueToCount}'");
                    foreach (DataRow row in matchingRows)
                    {
                        int originalQty = int.Parse(row["Current qty"].ToString());
                        int updatedQty = count;

                        // If 'Current qty' changed or decreased, add the row to the new DataTable
                        if (originalQty != updatedQty || updatedQty < initialSumByLot[valueToCount])
                        {
                            DataRow newRow = changedRowsDataTable.NewRow();
                            newRow.ItemArray = row.ItemArray; // Copy the entire row

                            // Update the 'Current qty' value in the new row
                            newRow["Current qty"] = updatedQty;

                            // Add the changed row to the new DataTable
                            changedRowsDataTable.Rows.Add(newRow);
                        }

                        // Update the 'Current qty' in the original DataTable (lotList)
                        row["Current qty"] = updatedQty;
                    }
                }

                // Update the SQL table with matching IDs
                readWriteData.UpdateSqlTableWithMatchingIDs(destinationTableLot, changedRowsDataTable);

                // Read the updated data from SQL into a DataTable
                DataTable updatedLotList = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryLotList);

                // Set the updated DataTable as the DataSource for adgvLotList
                adgvLotList.DataSource = updatedLotList;

                adgvLotList.Sort(adgvLotList.Columns["ID"], ListSortDirection.Descending);

                // Create a DataTable to store rows with updated values
                DataTable updatedRowsDataTable = waferList.Clone();

                foreach (DataRow row in chipList.Rows)
                {
                    string batch = row.Field<string>("Batch");
                    string waferValue = row.Field<string>("Wafer");
                    string status = row.Field<string>("Status");
                    string picked = row.Field<string>("Picked");
                    string lot = row.Field<string>("Lot");

                    // Create a key for the batch-wafer combination
                    string key = $"{batch}-{waferValue}";

                    // Create a DataTable with columns "Lot" and "Qty"
                    DataTable lotCountTable = new DataTable();
                    lotCountTable.Columns.Add("Lot", typeof(string));
                    lotCountTable.Columns.Add("Qty", typeof(int));

                    // Count "Good" and "Fail" values separately in the Status column
                    if (!countGood.ContainsKey(key))
                    {
                        countGood[key] = 0;
                    }
                    if (status == "Good")
                    {
                        countGood[key]++;
                    }
                    else if (status == "Fail")
                    {
                        // Use a separate count for "Fail" status
                        if (!countFail.ContainsKey(key))
                        {
                            countFail[key] = 0;
                        }
                        countFail[key]++;
                    }

                    // Count non-empty cells in the "Picked" column
                    if (!countPicked.ContainsKey(key))
                    {
                        countPicked[key] = 0;
                    }

                    if (!string.IsNullOrEmpty(picked))
                    {
                        countPicked[key]++;
                    }

                    // Count non-empty cells in the "Lot" column
                    if (!countLot.ContainsKey(key))
                    {
                        countLot[key] = 0;
                    }

                    if (!string.IsNullOrEmpty(lot))
                    {
                        countLot[key]++;
                    }
                }

                // Update the queryWafer DataTable based on the counts
                foreach (DataRow waferRow in waferList.Rows)
                {
                    string batch = waferRow.Field<string>("Batch");
                    string waferValue = waferRow.Field<string>("Wafer");
                    string key = $"{batch}-{waferValue}";

                    if (countGood.ContainsKey(key) || countFail.ContainsKey(key) || countPicked.ContainsKey(key) || countLot.ContainsKey(key))
                    {
                        // Get the original counts from the row
                        int originalAccepted = 0;
                        int originalRejected = 0;
                        int originalPicked = 0;
                        int originalReserved = 0;

                        // Use Try-Catch to catch the "Specified cast is not valid" error
                        try
                        {
                            originalAccepted = waferRow.Field<int>("Accepted");
                            originalRejected = waferRow.Field<int>("Rejected");
                            originalPicked = waferRow.Field<int>("Picked");
                            originalReserved = waferRow.Field<int>("Reserved");
                        }
                        catch (InvalidCastException ex)
                        {
                            Console.WriteLine($"Error casting row data: {ex.Message}");
                        }

                        // Get the updated counts from the dictionaries (use 0 if not found)
                        int updatedAccepted = countGood.ContainsKey(key) ? countGood[key] : 0;
                        int updatedRejected = countFail.ContainsKey(key) ? countFail[key] : 0;
                        int updatedPicked = countPicked.ContainsKey(key) ? countPicked[key] : 0;
                        int updatedReserved = countLot.ContainsKey(key) ? countLot[key] : 0;

                        // Check if any counts have changed
                        if (originalAccepted != updatedAccepted ||
                            originalRejected != updatedRejected ||
                            originalPicked != updatedPicked ||
                            originalReserved != updatedReserved)
                        {
                            // Create a new row in the updatedRowsDataTable and copy the data from the original row
                            DataRow newRow = updatedRowsDataTable.NewRow();
                            newRow.ItemArray = waferRow.ItemArray; // Copy the entire row

                            // Update the counts in the new row
                            newRow["Accepted"] = updatedAccepted;
                            newRow["Rejected"] = updatedRejected;
                            newRow["Picked"] = updatedPicked;
                            newRow["Reserved"] = updatedReserved;

                            // Add the row to the DataTable of updated rows
                            updatedRowsDataTable.Rows.Add(newRow);
                        }
                    }
                }

                // Update the SQL table with matching IDs
                readWriteData.UpdateSqlTableWithMatchingIDs(parentForm.destinationTableWafer, updatedRowsDataTable);

                // Read the updated data from SQL into a DataTable
                DataTable updatedWaferList = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryWafer);

                // Set the updated DataTable as the DataSource for adgvWafer
                parentForm.adgvWafer.DataSource = updatedWaferList;

                // Sort adgvWafer
                parentForm.adgvWafer.Sort(parentForm.adgvWafer.Columns["ID"], ListSortDirection.Descending);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void adgvLotList_SelectionChanged(object sender, EventArgs e)
        {
            // Check if there is a selected row in adgvLotList
            if (adgvLotList.SelectedRows.Count > 0)
            {
                // Get the selected row from adgvLotList
                DataGridViewRow selectedRow = adgvLotList.SelectedRows[0];

                // Get the value from the "Lot status" column of the selected row
                string lotStatus = selectedRow.Cells["Lot status"].Value.ToString();
                lotStatus = lotStatus.Trim();

                if (lotStatus == "Closed")
                {
                    toggleLotButtons(false);
                    buttonPickList.Enabled = true;
                    buttonWaferMap.Enabled = true;
                }
                else
                {
                    toggleLotButtons(true);
                }
            }
            else
            {
                // No row selected, disable all buttons
                toggleLotButtons(false);
            }

            if (adgvLotList.SelectedRows.Count > 1)
            {
                // If more than one row is selected, deselect all but the first selected row
                for (int i = 1; i < adgvLotList.SelectedRows.Count; i++)
                {
                    adgvLotList.SelectedRows[i].Selected = false;
                }
            }
        }

        private void toggleLotButtons(bool enabled)
        {
            buttonCloseLot.Enabled = enabled;
            buttonAdd.Enabled = enabled;
            buttonPickList.Enabled = enabled;
            buttonWaferMap.Enabled = enabled;
        }

        private void AddToLot_Load(object sender, EventArgs e)
        {
            DataTable dataTableLot = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryLotList);
            if (dataTableLot.Columns["ID"].DataType == typeof(string))
            {
                // Add a temporary column
                dataTableLot.Columns.Add("TempID", typeof(int));

                // Copy the data from the original ID column to TempID and convert to int
                foreach (DataRow row in dataTableLot.Rows)
                {
                    row["TempID"] = Convert.ToInt32(row["ID"]);
                }

                // Remove the original ID column and rename the TempID column to ID
                dataTableLot.Columns.Remove("ID");
                dataTableLot.Columns["TempID"].ColumnName = "ID";
            }
            updateGrid();
            adgvLotList.DataSource = dataTableLot;
            adgvLotList.Sort(adgvLotList.Columns["ID"], ListSortDirection.Descending);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> columnNames = new List<string> { "Picked", "Picked by", "Last edited", "Comment" };
            List<object> newValues = new List<object> { DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), parentForm.userName, DateTime.Now, $"{parentForm.userName} picked chip" };

            UpdateAndReselectRows(columnNames, newValues);
            updateGrid();
        }

        private void buttonRemovePick_Click(object sender, EventArgs e)
        {
            List<string> columnNames = new List<string> { "Picked", "Picked by", "Last edited", "Comment" };
            List<object> newValues = new List<object> { "", "", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), $"{parentForm.userName} unpicked chip" };

            UpdateAndReselectRows(columnNames, newValues);
            updateGrid();
        }

        private void buttonRemoveLot_Click(object sender, EventArgs e)
        {
            List<string> columnNames = new List<string> { "Lot", "Lot date", "Last edited", "Lot by", "Lot status", "Comment" };
            List<object> newValues = new List<object> { "", "", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "", "", $"{parentForm.userName} removed from lot" };


            UpdateAndReselectRows(columnNames, newValues);
            updateGrid();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = adgvLotList.SelectedRows[0];
            string selectedLotName = selectedRow.Cells["Lot"].Value.ToString();
            string lotStatus = selectedRow.Cells["Lot status"].Value.ToString();

            List<string> columnNames = new List<string> { "Lot", "Lot date", "Last edited", "Lot by", "Lot status", "Comment" };
            List<object> newValues = new List<object> { selectedLotName, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), parentForm.userName, lotStatus, $"{parentForm.userName} added to lot {selectedLotName}" };

            UpdateAndReselectRows(columnNames, newValues);
            updateGrid();
        }

        private void buttonCloseLot_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if there is a selected row in adgvLotList
                if (adgvLotList.SelectedRows.Count == 0)
                {
                    throw new Exception("Please select a row in adgvLotList.");
                }

                // Get the selected row from adgvLotList
                DataGridViewRow selectedRow = adgvLotList.SelectedRows[0];

                // Get the value from the "Lot" column of the selected row
                string selectedLotName = selectedRow.Cells["Lot"].Value.ToString();

                selectedRow.Cells["Lot status"].Value = "Closed";

                // Set up column names and new values for updating rows
                List<string> columnNames = new List<string> { "Lot status" };
                List<object> newValues = new List<object> { "Closed" };

                // Mark rows with matching "Lot" for update
                foreach (DataGridViewRow row in parentForm.adgvChip.Rows)
                {
                    if (row.Cells["Lot"].Value.ToString() == selectedLotName)
                    {
                        row.Selected = true;
                    }
                }

                // Use the UpdateAndReselectRows method to update selected rows
                UpdateAndReselectRows(columnNames, newValues);

                toggleLotButtons(false);
                List<string> includedColumnsLot = parentForm.includedColumnsLot;
                printListOfChips(includedColumnsLot);
            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateAndReselectRows(List<string> columnNames, List<object> newValues)
        {
            try
            {
                if (parentForm.adgvChip.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select at least one row in chip list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (columnNames.Count != newValues.Count)
                {
                    MessageBox.Show("Mismatch between column names and values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show("Do you want to update the selected rows?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    List<int> selectedIndices = new List<int>();
                    foreach (DataGridViewRow row in parentForm.adgvChip.SelectedRows)
                    {
                        selectedIndices.Add(row.Index);
                    }

                    List<int> selectedRowIDs = new List<int>();
                    foreach (DataGridViewRow selectedRow in parentForm.adgvChip.SelectedRows)
                    {
                        int rowID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                        selectedRowIDs.Add(rowID);
                    }

                    readWriteData.WriteToSqlReplaceMultipleRows(parentForm.destinationTableChip, selectedRowIDs, columnNames, newValues);

                    DataTable dataTableChip = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryChip);
                    parentForm.adgvChip.DataSource = dataTableChip;

                    // Re-select the rows.
                    parentForm.adgvChip.ClearSelection();
                    foreach (int index in selectedIndices)
                    {
                        if (index < parentForm.adgvChip.Rows.Count)
                        {
                            parentForm.adgvChip.Rows[index].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void adgvLotList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Assuming newValue is the new value entered by the user
                object newValue = adgvLotList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string columnName = adgvLotList.Columns[e.ColumnIndex].DataPropertyName;

                int rowID = Convert.ToInt32(adgvLotList.Rows[e.RowIndex].Cells["ID"].Value);

                readWriteData.WriteToSqlReplace(destinationTableLot, rowID, columnName, newValue);
            }
        }

        private string GenerateUniqueLotName(DataGridView dgv)
        {
            // Get today's date in the format "yyMMdd" (e.g., "230920" for 2023-09-20)
            string today = DateTime.Now.ToString("yyMMdd");

            List<string> existingLotNames = new List<string>();

            // Loop through the existing lot names in the "Lot" column of the DataGridView
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["Lot"].Value != null)
                {
                    string lotName = row.Cells["Lot"].Value.ToString();
                    existingLotNames.Add(lotName);
                }
            }

            int newLotNumber = 1;

            // Find the highest lot number in existing lot names with the same date stamp
            foreach (string lotName in existingLotNames)
            {
                if (lotName.StartsWith(today))
                {
                    string lastTwoDigits = lotName.Substring(today.Length);
                    int number;
                    if (int.TryParse(lastTwoDigits, out number))
                    {
                        if (number >= newLotNumber)
                        {
                            newLotNumber = number + 1;
                        }
                    }
                }
            }

            // Create the new unique lot name
            string newLotName = today + newLotNumber.ToString("D2"); // D2 ensures two digits (01, 02, etc.)

            return newLotName;
        }

        private void buttonCreateLot_Click_1(object sender, EventArgs e)
        {
            // Generate a unique Lot name
            string generatedLotName = GenerateUniqueLotName(adgvLotList);

            // Show a confirmation message with the generated "Lot" value
            DialogResult result = MessageBox.Show("Do you want to create Lot " + generatedLotName + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataTable dataTable = new DataTable();

                // Define columns for the DataTable based on your cell values
                dataTable.Columns.Add("Lot", typeof(string));
                dataTable.Columns.Add("Created by", typeof(string));
                dataTable.Columns.Add("Creation date", typeof(DateTime));
                dataTable.Columns.Add("Lot status", typeof(string));
                dataTable.Columns.Add("Current qty", typeof(string));

                // Add a new row to the DataTable
                DataRow newRow = dataTable.NewRow();
                newRow["Lot"] = generatedLotName;
                newRow["Created by"] = parentForm.userName;
                newRow["Creation date"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                newRow["Lot status"] = "Open";
                newRow["Current qty"] = "0";

                // Add the new row to the DataTable
                dataTable.Rows.Add(newRow);
                readWriteData.CreateNewRowSql(destinationTableLot, dataTable);
                DataTable dataTableSource = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryLotList);
                if (dataTableSource.Columns["ID"].DataType == typeof(string))
                {
                    // Add a temporary column
                    dataTableSource.Columns.Add("TempID", typeof(int));

                    // Copy the data from the original ID column to TempID and convert to int
                    foreach (DataRow row in dataTableSource.Rows)
                    {
                        row["TempID"] = Convert.ToInt32(row["ID"]);
                    }

                    // Remove the original ID column and rename the TempID column to ID
                    dataTableSource.Columns.Remove("ID");
                    dataTableSource.Columns["TempID"].ColumnName = "ID";
                }
                adgvLotList.DataSource = dataTableSource;
                adgvLotList.Sort(adgvLotList.Columns["ID"], ListSortDirection.Descending);
                Console.WriteLine(dataTableSource.Columns["ID"].DataType);
            }
        }

        private void printListOfChips(List<string> includedColumns)
        {
            string selectedLot = adgvLotList.SelectedRows[0].Cells["Lot"].Value.ToString();
            // Create a DataTable to hold the selected data
            DataTable selectedDataTable = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryChip);
            DataTable filteredDataTable = selectedDataTable.Clone();
            // Iterate through rows in the selectedDataTable and copy matching rows to the filteredDataTable
            foreach (DataRow row in selectedDataTable.Rows)
            {
                if (row["Lot"] != null && row["Lot"].ToString() == selectedLot)
                {
                    // Create a new row in the filteredDataTable and copy data
                    DataRow newRow = filteredDataTable.NewRow();

                    foreach (string columnName in includedColumns)
                    {
                        if (filteredDataTable.Columns.Contains(columnName))
                        {
                            newRow[columnName] = row[columnName];
                        }
                    }

                    // Add the row to the filteredDataTable
                    filteredDataTable.Rows.Add(newRow);
                }
            }

            // Check if there are matching rows
            if (filteredDataTable.Rows.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                // Prepopulate the filename with the selectedLot
                saveFileDialog.FileName = $"{selectedLot}_pick_list";
                // Check if originalDataTable is empty
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    // Call the GeneratePdf method to create the PDF from the filteredDataTable
                    StoreToPDF.GeneratePdf(fileName, filteredDataTable, includedColumns, selectedLot);
                }
            }
            else
            {
                // Display a message to inform the user
                MessageBox.Show("No data found for the selected Lot.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Exit the method
            }
        }


        private void buttonPickList_Click(object sender, EventArgs e)
        {
            List<string> includedColumnsLot = parentForm.includedColumnsLot;
            printListOfChips(includedColumnsLot);
        }

        private void adgvLotList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Sort the DataGridView in descending order based on the "Lot" column
            adgvLotList.Sort(adgvLotList.Columns["ID"], ListSortDirection.Descending);

            // Unsubscribe from the event to avoid sorting multiple times
            adgvLotList.DataBindingComplete -= adgvLotList_DataBindingComplete;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Check if there is a selected row in adgvLotList
                if (adgvLotList.SelectedRows.Count > 0)
                {
                    // Get the "Lot" value from the selected row in adgvLotList
                    string selectedLot = adgvLotList.SelectedRows[0].Cells["Lot"].Value.ToString();

                    Console.WriteLine($"Selected Lot: {selectedLot}");

                    // Query the Chip_inventory_list SQL table to get rows with the same "Lot" value
                    DataTable originalDataTable = new DataTable();
                    // Replace 'yourConnectionString' and 'yourQuery' with your actual database connection string and SQL query.
                    string query = $"SELECT * FROM Chip_inventory_list WHERE Lot = '{selectedLot}'";
                    originalDataTable = readWriteData.ReadFromSql(parentForm.connectionString, query);

                    // Check if originalDataTable is empty
                    if (originalDataTable.Rows.Count == 0)
                    {
                        // Display a message to inform the user
                        MessageBox.Show("No data found for the selected Lot.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Exit the method
                    }

                    // Create a list to store unique batch-wafer-generation combinations
                    List<string> uniqueBatchWaferGenerationCombinations = new List<string>();

                    // Populate the list with unique combinations from the queried data
                    foreach (DataRow row in originalDataTable.Rows)
                    {
                        string batch = row["Batch"].ToString();
                        string wafer = row["Wafer"].ToString();
                        string generation = row["Generation"].ToString();

                        // Create a unique combination string
                        string combination = $"{batch}_{wafer}_{generation}";

                        if (!uniqueBatchWaferGenerationCombinations.Contains(combination))
                        {
                            uniqueBatchWaferGenerationCombinations.Add(combination);
                        }
                    }

                    foreach (string combination in uniqueBatchWaferGenerationCombinations)
                    {
                        // Split the combination into batch, wafer, and generation
                        string[] parts = combination.Split('_');
                        string batch = parts[0];
                        string wafer = parts[1];
                        string generation = parts[2];

                        // Query the Chip_inventory_list to get rows with the same combination
                        DataTable combinationDataTable = new DataTable();
                        string combinationQuery = $"SELECT * FROM Chip_inventory_list WHERE Batch = '{batch}' AND Wafer = '{wafer}' AND Generation = '{generation}'";
                        combinationDataTable = readWriteData.ReadFromSql(parentForm.connectionString, combinationQuery);

                        // Create a new combinedDataTable for each combination
                        DataTable combinedDataTable = new DataTable();
                        combinedDataTable.Columns.Add("Batch");
                        combinedDataTable.Columns.Add("Wafer");
                        combinedDataTable.Columns.Add("Generation");
                        combinedDataTable.Columns.Add("Chip");
                        combinedDataTable.Columns.Add("Picked");
                        combinedDataTable.Columns.Add("Capillary");
                        combinedDataTable.Columns.Add("Lot");

                        foreach (DataRow row in combinationDataTable.Rows)
                        {
                            // Add the row data to the combinedDataTable
                            combinedDataTable.Rows.Add(row["Batch"], row["Wafer"], row["Generation"], row["Chip"], row["Picked"], row["Capillary"], row["Lot"]);
                        }

                        // Create a SaveFileDialog to choose the file path for the PDF
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "PDF Files|*.pdf";
                        saveFileDialog.Title = "Save PDF File";
                        saveFileDialog.FileName = $"{generation}_{batch}_{wafer}_{selectedLot}";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string fileName = saveFileDialog.FileName;
                            DateTime saveDate = DateTime.Now;

                            // Call the method to export data to PDF
                            StoreToPDF.ExportDataTableToPDF(combinedDataTable, fileName, batch, wafer, generation, saveDate, selectedLot);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row in adgvLotList.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
