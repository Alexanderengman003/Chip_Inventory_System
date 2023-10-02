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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Chip_Inventory_System
{
    public partial class AddToLot : Form
    {
        private readonly InteractWithDatabase interactWithDatabase;
        private readonly InteractWithGrid interactWithGrid;
        private readonly UserInputLot userInputLot;

        string destinationTableChip = "Chip_inventory_list";
        string destinationTableChipLog = "Chip_inventory_log";
        private string filePath = @"Z:\Shared\Ascilion\General\4_Storage (STO)\Wafers\Test\";
        private string fileNameChip = "chip_inventory_list.csv";
        private string fileNameChipLog = "chip_inventory_log.csv";
        string destinationTableLot = "Lot_list";
        private Form1 parentForm;
        private readonly ReadWriteData readWriteData; // Declare the field

        // Constructor that takes a DataTable as an argument
        public AddToLot(Form1 parentForm)
        {
            InitializeComponent();
            InitializeAdvancedDataGridView();
            this.parentForm = parentForm;
            interactWithDatabase = new InteractWithDatabase(); // Initialize the field
            readWriteData = new ReadWriteData(); // Initialize the field
            interactWithGrid = new InteractWithGrid(); // Initialize the field
        }

        private void InitializeAdvancedDataGridView()
        {
            adgvLotList.AllowUserToResizeRows = false;
            adgvLotList.AllowUserToResizeColumns = false;
            adgvLotList.AutoGenerateColumns = true; // Automatically generate columns
            adgvLotList.Dock = DockStyle.Fill;
            adgvLotList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            //buttonColumn.HeaderText = "Info";
            //buttonColumn.Text = "Open";
            //buttonColumn.Name = "ButtonColumn";
            //buttonColumn.CellTemplate = new DataGridViewButtonCell { UseColumnTextForButtonValue = true };
            //buttonColumn.SortMode = DataGridViewColumnSortMode.NotSortable; // Disable sorting
            //adgvLotList.Columns.Add(buttonColumn);

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if there are selected rows in parentForm.adgvChip
                if (parentForm.adgvChip.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select at least one row in chip list", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if there is a selected row in adgvLotList
                if (adgvLotList.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row in lot list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = adgvLotList.SelectedRows[0];
                string selectedLotName = selectedRow.Cells["Lot"].Value.ToString();

                DialogResult result = MessageBox.Show("Do you want to add the items to Lot " + selectedLotName + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Get the corresponding "Lot status" value from adgvLotList
                    string lotStatus = selectedRow.Cells["Lot status"].Value.ToString();

                    // Iterate through selected rows in parentForm.adgvChip
                    foreach (DataGridViewRow row in parentForm.adgvChip.SelectedRows)
                    {
                        int rowID = Convert.ToInt32(row.Cells["ID"].Value);
                        // Extract values from the selected row in adgvChip

                        string batchValue = row.Cells["Batch"].Value.ToString();
                        string waferValue = row.Cells["Wafer"].Value.ToString();

                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot", selectedLotName);
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot date", DateTime.Now);
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Last edited", DateTime.Now);
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot by", parentForm.userName);
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Comment", parentForm.userName + " added to lot " + selectedLotName);
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot status", lotStatus);
                    }
                    // Update the grid after making changes
                    updateGrid();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log the exception or show an error message.
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateGrid()
        {
            List<string> valuesToSearch = new List<string> { "Good", "Fail" };
            List<string> columnsToSearchStatus = new List<string> { "Status" };
            List<string> columnsToSearchEmpty = new List<string> { "Lot", "Picked" };
            List<string> columnsToSearchLotList = new List<string> { "Lot" };

            DataTable lotList = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryLotList);
            DataTable chipList = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryChip);

            // Extract all distinct Lot values from the lotList table
            List<string> valuesToSearchLotList = lotList.AsEnumerable()
                .Select(row => row.Field<string>("Lot"))
                .Distinct()
                .ToList();

            // Assuming you have a DataGridView named 'adgvLotList' for displaying the counts
            string columnNameLot = "Lot";

            foreach (string valueToCount in valuesToSearchLotList)
            {
                int countLot = chipList.AsEnumerable()
                    .Count(row => row.Field<string>(columnNameLot) == valueToCount);
                foreach (DataGridViewRow waferGridRow in adgvLotList.Rows)
                {
                    string adgvLot = waferGridRow.Cells["Lot"].Value?.ToString();

                    if (valueToCount == adgvLot)
                    {
                        waferGridRow.Cells["Current qty"].Value = countLot;
                    }
                }
            }

            string columnNameStatus = "Status";

            // Create a dictionary to store counts for each batch/wafer combination
            Dictionary<string, Dictionary<string, int>> batchWaferCounts = new Dictionary<string, Dictionary<string, int>>();

            foreach (DataRow row in chipList.Rows)
            {
                string batch = row.Field<string>("Batch");
                string wafer = row.Field<string>("Wafer");
                string valueToCount = row.Field<string>(columnNameStatus);

                // Create a unique key for the batch/wafer combination
                string key = $"{batch}-{wafer}";

                // Check if the key exists in the dictionary, if not, initialize the counts to 0
                if (!batchWaferCounts.ContainsKey(key))
                {
                    batchWaferCounts[key] = new Dictionary<string, int>
            {
                { "Good", 0 },
                { "Fail", 0 }
            };
                }

                // Increment the count for the specific combination and value
                if (valueToCount == "Good")
                {
                    batchWaferCounts[key]["Good"]++;
                }
                else if (valueToCount == "Fail")
                {
                    batchWaferCounts[key]["Fail"]++;
                }
            }

            // Print the counts for each batch/wafer combination
            foreach (var kvp in batchWaferCounts)
            {
                string[] parts = kvp.Key.Split('-');
                string batch = parts[0];
                string wafer = parts[1];
                int goodCount = kvp.Value["Good"];
                int failCount = kvp.Value["Fail"];
                foreach (DataGridViewRow waferGridRow in parentForm.adgvWafer.Rows)
                {
                    string adgvBatch = waferGridRow.Cells["Batch"].Value?.ToString();
                    string adgvWafer = waferGridRow.Cells["Wafer"].Value?.ToString();

                    if (batch == adgvBatch && wafer == adgvWafer)
                    {
                        waferGridRow.Cells["Accepted"].Value = goodCount;
                        waferGridRow.Cells["Rejected"].Value = failCount;
                    }
                }
                Console.WriteLine($"Batch: {batch}, Wafer: {wafer}, Good: {goodCount}, Fail: {failCount}");
            }

            Dictionary<string, Dictionary<string, int>> batchWaferCounts2 = new Dictionary<string, Dictionary<string, int>>();

            string columnNamePicked = "Picked";

            // Create a dictionary to store counts for each batch/wafer combination

            foreach (DataRow row in chipList.Rows)
            {
                string batch = row.Field<string>("Batch");
                string wafer = row.Field<string>("Wafer");
                string lotValue = row.Field<string>(columnNameLot);
                string pickedValue = row.Field<string>(columnNamePicked);

                // Create a unique key for the batch/wafer combination
                string key = $"{batch}-{wafer}";

                // Check if the key exists in the dictionary, if not, initialize the counts to 0
                if (!batchWaferCounts2.ContainsKey(key))
                {
                    batchWaferCounts2[key] = new Dictionary<string, int>
        {
            { "Lot", 0 },
            { "Picked", 0 }
        };
                }

                // Increment the count based on cell values
                if (!string.IsNullOrEmpty(lotValue))
                {
                    batchWaferCounts2[key]["Lot"]++;
                }

                if (!string.IsNullOrEmpty(pickedValue))
                {
                    batchWaferCounts2[key]["Picked"]++;
                }
            }

            // Print the counts for each batch/wafer combination
            foreach (var kvp in batchWaferCounts2)
            {
                string[] parts = kvp.Key.Split('-');
                string batch = parts[0];
                string wafer = parts[1];
                int lotCount = kvp.Value["Lot"];
                int pickedCount = kvp.Value["Picked"];

                foreach (DataGridViewRow waferGridRow in parentForm.adgvWafer.Rows)
                {
                    string adgvBatch = waferGridRow.Cells["Batch"].Value?.ToString();
                    string adgvWafer = waferGridRow.Cells["Wafer"].Value?.ToString();

                    if (batch == adgvBatch && wafer == adgvWafer)
                    {
                        waferGridRow.Cells["Reserved"].Value = lotCount;
                        waferGridRow.Cells["Picked"].Value = pickedCount;
                    }
                }
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
            adgvLotList.DataSource = dataTableLot;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if there is at least one selected row in adgvChip
                if (parentForm.adgvChip.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select at least one row in chip list", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Do you want to pick the chips?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Iterate through selected rows in parentForm.adgvChip
                    foreach (DataGridViewRow row in parentForm.adgvChip.SelectedRows)
                    {
                        int rowID = Convert.ToInt32(row.Cells["ID"].Value);

                        // Update the DataGridView with the edited values and call the method
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Picked", DateTime.Now);
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Last edited", DateTime.Now);
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Picked by", parentForm.userName);
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Comment", parentForm.userName + "picked chip");
                    }

                }
                updateGrid();
            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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

                // Update the "Lot status" of the selected lot in adgvLotList
                selectedRow.Cells["Lot_status"].Value = "Closed";

                // Update the "Lot status" of the corresponding rows in adgvChip
                foreach (DataGridViewRow row in parentForm.adgvChip.Rows)
                {
                    if (row.Cells["Lot"].Value.ToString() == selectedLotName)
                    {
                        row.Cells["Lot status"].Value = "Closed";
                        parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot status", "Closed");
                    }
                }

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

        private void buttonRemovePick_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any rows are selected
                if (parentForm.adgvChip.SelectedRows.Count == 0)
                {
                    MessageBox.Show("No rows selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Do you want to unpick the items?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in parentForm.adgvChip.SelectedRows)
                    {
                        // Get the "Lot" column value
                        string pickedValue = row.Cells["Picked"].Value?.ToString();

                        if (!string.IsNullOrWhiteSpace(pickedValue))
                        {
                            int rowID = Convert.ToInt32(row.Cells["ID"].Value);

                            // Update the DataGridView with the edited values and call the method
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Picked", "");
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Last edited", DateTime.Now);
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Picked by", "");
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Comment", parentForm.userName + " unpicked chip");
                        }
                    }
                }
                updateGrid();
            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRemoveLot_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any rows are selected
                if (parentForm.adgvChip.SelectedRows.Count == 0)
                {
                    MessageBox.Show("No rows selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Do you want to remove the items from the Lot?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in parentForm.adgvChip.SelectedRows)
                    {
                        // Get the "Lot" column value
                        string lotValue = row.Cells["Lot"].Value?.ToString();

                        if (!string.IsNullOrWhiteSpace(lotValue))
                        {
                            int rowID = Convert.ToInt32(row.Cells["ID"].Value);

                            // Update the DataGridView with the edited values and call the method
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot", "");
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot date", "");
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Last edited", DateTime.Now);
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot by", "");
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Comment", parentForm.userName + " removed from lot");
                            parentForm.UpdateCellValueAndSql(destinationTableChip, row, "Lot status", "");
                        }
                    }
                }
                updateGrid();
            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
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

                // Add a new row to the DataTable
                DataRow newRow = dataTable.NewRow();
                newRow["Lot"] = generatedLotName;
                newRow["Created by"] = parentForm.userName;
                newRow["Creation date"] = DateTime.Now;
                newRow["Lot status"] = "Open";

                // Add the new row to the DataTable
                dataTable.Rows.Add(newRow);

                readWriteData.CreateNewRowSql(destinationTableLot, dataTable);
                interactWithGrid.ReadAndOutputSql(parentForm.connectionString, parentForm.queryLotList, adgvLotList, null);
                adgvLotList.Sort(adgvLotList.Columns["ID"], ListSortDirection.Descending);
            }
        }


        // Define the columns to include in the PD

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
