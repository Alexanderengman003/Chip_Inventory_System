using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using Zuby.ADGV;

namespace Chip_Inventory_System
{
    public partial class Form1 : Form
    {
        y
        private readonly InteractWithGrid interactWithGrid; // Declare the field
        private readonly ReadWriteData readWriteData; // Declare the field
        private readonly InteractWithDatabase interactWithDatabase;
        private readonly AddToLot addToLot;
        public string connectionString = "Server=Ascilion006;Database=Chip_Inventory_Database;Integrated Security=True";
        public string queryWafer = "SELECT * FROM Wafer_inventory_list";
        public string queryChip = "SELECT * FROM Chip_inventory_list";
        public string queryLotList = "SELECT * FROM Lot_list";
        public string userName = "Alexander Engman";
        string destinationTableWafer = "Wafer_inventory_list";
        public string destinationTableChip = "Chip_inventory_list";
        private string filePath = @"Z:\Shared\Ascilion\General\4_Storage (STO)\Wafers\Dev\";
        private string fileNameChip = "chip_inventory_list.csv";
        private string fileNameWafer = "wafer_inventory_list.csv";
        private string fileNameLot = "lot_list.csv";
        private string buttonColumn = "ButtonColumn";
        private UserInputLot userInputForm;
        private Timer CsvBackupTimer;

        public List<string> includedColumnsWafer = new List<string>
            {
                "Generation",
                "Batch",
                "Wafer",
                "Customer",
                "Backside",
                "Glass",
                "Received"
            };

        public List<string> includedColumnsChip = new List<string>
            {
                "Generation",
                "Batch",
                "Wafer",
                "Chip",
                "Customer",
                "Backside",
                "Glass",
                "Chip",
                "Status",
                "Lot"
            };

        public List<string> includedColumnsLot = new List<string>
            {
                // Add the names of columns you want to include in the PDF
                "Generation",
                "Batch",
                "Wafer",
                "Chip",
                "Glass",
                "Backside",
                "Capillary",
                "Status"
                // Add more columns as needed
            };

        DataTable lotList = new DataTable();

        private void InitializeBackupTimer()
        {
            // Initialize the timer
            CsvBackupTimer = new Timer();

            // Set the timer interval to 5 minutes (300,000 milliseconds)
            CsvBackupTimer.Interval = 300000;

            // Hook up the timer tick event to your method
            CsvBackupTimer.Tick += CsvWriteTimer_Tick;

            // Start the timer
            CsvBackupTimer.Start();
        }

        private void InitializeAdvancedDataGridView()
        {
            adgvWafer.CellClick += adgvWafer_CellClick;
            adgvChip.CellClick += adgvChip_CellClick;

            adgvWafer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            adgvWafer.AllowUserToResizeRows = false;
            adgvWafer.AllowUserToResizeColumns = false;
            adgvWafer.Dock = DockStyle.Fill;

            adgvChip.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            adgvChip.AllowUserToResizeRows = false;
            adgvChip.AllowUserToResizeColumns = false;
            adgvChip.Dock = DockStyle.Fill;

            adgvLot.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            adgvLot.AllowUserToResizeRows = false;
            adgvLot.AllowUserToResizeColumns = false;
            adgvLot.Dock = DockStyle.Fill;

            // Create a button column
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Wafer map";
            buttonColumn.Text = "Open";
            buttonColumn.Name = "ButtonColumn";
            buttonColumn.CellTemplate = new DataGridViewButtonCell { UseColumnTextForButtonValue = true };
            adgvWafer.Columns.Add(buttonColumn);
        }


    public Form1()
        {
            InitializeComponent();
            InitializeAdvancedDataGridView();
            toggleButtonsWafer(false);
            toggleButtonsChip(false);
            interactWithGrid = new InteractWithGrid(); // Initialize the field
            readWriteData = new ReadWriteData(); // Initialize the field

            interactWithDatabase = new InteractWithDatabase(); // Initialize the field
            addToLot = new AddToLot(this);

        }

        private void toggleButtonsWafer(bool enabled)
        {
            buttonDeleteWafer.Enabled = enabled;
            buttonPDFWafer.Enabled = enabled;
            buttonCSVWafer.Enabled = enabled;
        }

        private void toggleButtonsChip(bool enabled)
        {
            buttonPDFChip.Enabled = enabled;
            buttonCSVChip.Enabled = enabled;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            CustomFlagColumn flagColumn = new CustomFlagColumn();
            flagColumn.Name = "Flag";
            flagColumn.Width = 20;
            flagColumn.HeaderText = "";

            adgvChip.Columns.Add(flagColumn);

            DataTable dataTableWafer = readWriteData.ReadFromSql(connectionString, queryWafer);
            adgvWafer.DataSource = dataTableWafer;
            DataTable dataTableChip = readWriteData.ReadFromSql(connectionString, queryChip);
            adgvChip.DataSource = dataTableChip;
            InitializeBackupTimer();
        }


        public void UpdateCellValueAndSql(string destinationTable, DataGridViewRow row, string columnName, object newValue)
        {
            row.Cells[columnName].Value = newValue;
            int rowID = Convert.ToInt32(row.Cells["ID"].Value);

            // Write the edited value to the SQL database
            readWriteData.WriteToSqlReplace(destinationTable, rowID, columnName, newValue);

            // Check if the changed cell is in the "Picked," "Lot," or "Lot status" columns
            if (columnName == "Picked" || columnName == "Lot" || columnName == "Lot status")
            {
                // Update the flag column immediately based on the cell values
                string pickedValue = row.Cells["Picked"].Value?.ToString();
                string lotValue = row.Cells["Lot"].Value?.ToString();
                string lotStatusValue = row.Cells["Lot status"].Value?.ToString();

                // Access the custom flag cell in the "Flag" column and set its value
                CustomFlagCell flagCell = row.Cells["Flag"] as CustomFlagCell;
                if (flagCell != null)
                {
                    // Use the LoadFlagImage method to set the value of the "Flag" cell
                    row.Cells["Flag"].Value = flagCell.LoadFlagImage(flagCell.GetFlagKey(pickedValue, lotValue, lotStatusValue));
                }
            }
        }

        private void CsvWriteTimer_Tick(object sender, EventArgs e)
        {
            // This method will be called every 5 minutes
            readWriteData.ExportToCsv(connectionString, queryChip, filePath + fileNameChip);
            readWriteData.ExportToCsv(connectionString, queryWafer, filePath + fileNameWafer);
            readWriteData.ExportToCsv(connectionString, queryLotList, filePath + fileNameLot);
        }
        private void adgvWafer_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Assuming newValue is the new value entered by the user
                object newValue = adgvWafer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string columnName = adgvWafer.Columns[e.ColumnIndex].DataPropertyName;

                int rowID = Convert.ToInt32(adgvWafer.Rows[e.RowIndex].Cells["ID"].Value);

                readWriteData.WriteToSqlReplace(destinationTableWafer, rowID, columnName, newValue);
            }
        }

        private void buttonPDFWafer_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                saveFileDialog.FileName = $"Wafer_list_{DateTime.Now:yyyyMMdd_HHmmss}";

                // Get the selected "Generation," "Batch," and "Wafer" values from adgvWafer
                List<string> selectedValues = new List<string>();

                foreach (DataGridViewRow row in adgvWafer.SelectedRows)
                {
                    string generation = row.Cells["Generation"].Value.ToString();
                    string batch = row.Cells["Batch"].Value.ToString();
                    string wafer = row.Cells["Wafer"].Value.ToString();
                    string combinedValue = $"{generation}_{batch}_{wafer}";

                    selectedValues.Add(combinedValue);
                }

                // Read data from the database
                DataTable dataTable = readWriteData.ReadFromSql(connectionString, queryWafer);

                // Filter the dataTable to include only rows with the selected values
                DataTable filteredDataTable = dataTable.Clone();

                foreach (DataRow row in dataTable.Rows)
                {
                    string generation = row["Generation"].ToString();
                    string batch = row["Batch"].ToString();
                    string wafer = row["Wafer"].ToString();
                    string combinedValue = $"{generation}_{batch}_{wafer}";

                    if (selectedValues.Contains(combinedValue))
                    {
                        filteredDataTable.ImportRow(row);
                    }
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;

                    StoreToPDF.GeneratePdf(fileName, filteredDataTable, includedColumnsWafer, "");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonCSVWafer_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files|*.csv";
                saveFileDialog.Title = "Save CSV File";
                saveFileDialog.FileName = $"Wafer_list_{DateTime.Now:yyyyMMdd_HHmmss}";

                // Get the selected "Generation," "Batch," and "Wafer" values from adgvWafer
                List<string> selectedValues = new List<string>();

                foreach (DataGridViewRow row in adgvWafer.SelectedRows)
                {
                    string generation = row.Cells["Generation"].Value.ToString();
                    string batch = row.Cells["Batch"].Value.ToString();
                    string wafer = row.Cells["Wafer"].Value.ToString();
                    string combinedValue = $"{generation}_{batch}_{wafer}";

                    selectedValues.Add(combinedValue);
                }

                // Read data from the database
                DataTable dataTable = readWriteData.ReadFromSql(connectionString, queryWafer);

                // Filter the dataTable to include only rows with the selected values
                DataTable filteredDataTable = dataTable.Clone();

                foreach (DataRow row in dataTable.Rows)
                {
                    string generation = row["Generation"].ToString();
                    string batch = row["Batch"].ToString();
                    string wafer = row["Wafer"].ToString();
                    string combinedValue = $"{generation}_{batch}_{wafer}";

                    if (selectedValues.Contains(combinedValue))
                    {
                        filteredDataTable.ImportRow(row);
                    }
                }

                // Debug: Display the contents of filteredDataTable
                foreach (DataRow row in filteredDataTable.Rows)
                {
                    foreach (DataColumn column in filteredDataTable.Columns)
                    {
                        Console.WriteLine($"{column.ColumnName}: {row[column]}");
                    }
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    StoreToCSV.ExportDataTableToCsv(filteredDataTable, includedColumnsWafer, fileName);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonPDFChip_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                saveFileDialog.FileName = $"Chip_list_{DateTime.Now:yyyyMMdd_HHmmss}";

                // Get the selected "Generation," "Batch," and "Wafer" values from adgvWafer
                List<string> selectedValues = new List<string>();

                foreach (DataGridViewRow row in adgvChip.SelectedRows)
                {
                    string generation = row.Cells["Generation"].Value.ToString();
                    string batch = row.Cells["Batch"].Value.ToString();
                    string wafer = row.Cells["Wafer"].Value.ToString();
                    string chip = row.Cells["Chip"].Value.ToString();
                    string combinedValue = $"{generation}_{batch}_{wafer}_{chip}";
                    selectedValues.Add(combinedValue);
                }

                // Read data from the database
                DataTable dataTable = readWriteData.ReadFromSql(connectionString, queryChip);

                // Filter the dataTable to include only rows with the selected values
                DataTable filteredDataTable = dataTable.Clone();

                foreach (DataRow row in dataTable.Rows)
                {
                    string generation = row["Generation"].ToString();
                    string batch = row["Batch"].ToString();
                    string wafer = row["Wafer"].ToString();
                    string chip = row["Chip"].ToString();
                    string combinedValue = $"{generation}_{batch}_{wafer}_{chip}";

                    if (selectedValues.Contains(combinedValue))
                    {
                        filteredDataTable.ImportRow(row);
                    }
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;

                    StoreToPDF.GeneratePdf(fileName, filteredDataTable, includedColumnsChip, "");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCSVChip_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files|*.csv";
                saveFileDialog.Title = "Save CSV File";
                saveFileDialog.FileName = $"Chip_list_{DateTime.Now:yyyyMMdd_HHmmss}";

                // Get the selected "Generation," "Batch," and "Wafer" values from adgvWafer
                List<string> selectedValues = new List<string>();

                foreach (DataGridViewRow row in adgvChip.SelectedRows)
                {
                    string generation = row.Cells["Generation"].Value.ToString();
                    string batch = row.Cells["Batch"].Value.ToString();
                    string wafer = row.Cells["Wafer"].Value.ToString();
                    string chip = row.Cells["Chip"].Value.ToString();
                    string combinedValue = $"{generation}_{batch}_{wafer}_{chip}";
                    selectedValues.Add(combinedValue);
                }

                // Read data from the database
                DataTable dataTable = readWriteData.ReadFromSql(connectionString, queryChip);

                // Filter the dataTable to include only rows with the selected values
                DataTable filteredDataTable = dataTable.Clone();

                foreach (DataRow row in dataTable.Rows)
                {
                    string generation = row["Generation"].ToString();
                    string batch = row["Batch"].ToString();
                    string wafer = row["Wafer"].ToString();
                    string chip = row["chip"].ToString();
                    string combinedValue = $"{generation}_{batch}_{wafer}_{chip}";

                    if (selectedValues.Contains(combinedValue))
                    {
                        filteredDataTable.ImportRow(row);
                    }
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    StoreToCSV.ExportDataTableToCsv(filteredDataTable, includedColumnsChip, fileName);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowUserInputForm()
        {
            // Create an instance of the UserInputLot form
            userInputForm = new UserInputLot();
            // Show the UserInputLot form as a dialog
            DialogResult result = userInputForm.ShowDialog();
        }

        private void buttonLot_Click(object sender, EventArgs e)
        {
            // Get the selected rows as a DataTable
            DataTable selectedChipData = GetSelectedRowsAsDataTable(adgvChip);

            // Create an instance of the AddToLot form and pass the DataTable as a parameter
            AddToLot addToLot = new AddToLot(this);

            // Set the TopMost property to true to keep the form on top
            addToLot.TopMost = true;

            // Show the AddToLot form
            addToLot.Show();
        }

        private void adgvWafer_SelectionChanged(object sender, EventArgs e)
        {
            bool anyItemsSelected = adgvWafer.SelectedRows.Count > 0;

            // Call the method to toggle buttons based on the selection
            toggleButtonsWafer(anyItemsSelected);
        }

        private void adgvChip_SelectionChanged(object sender, EventArgs e)
        {
            bool anyItemsSelected = adgvChip.SelectedRows.Count > 0;

            // Call the method to toggle buttons based on the selection
            toggleButtonsChip(anyItemsSelected);
        }

        private DataTable GetSelectedRowsAsDataTable(DataGridView dataGridView)
        {
            DataTable dataTable = new DataTable();

            // Create columns based on the DataGridView columns
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.Name, typeof(object)); // Use typeof(object) for flexibility
            }

            // Add selected rows to the DataTable
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                DataRow dataRow = dataTable.NewRow();

                // Populate the DataRow with cell values from the selected row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dataRow[cell.OwningColumn.Name] = cell.Value;
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void adgvLot_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void adgvWafer_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the selected rows as a DataTable
            DataTable selectedChipData = GetSelectedRowsAsDataTable(adgvChip);

            // Create an instance of the AddToLot form and pass the DataTable as a parameter
            AddToLot addToLot = new AddToLot(this);

            // Show the AddToLot form
            addToLot.ShowDialog();
        }

        private void buttonRemovePick_Click(object sender, EventArgs e)
        {
            // Get the selected row
            DataGridViewRow selectedRow = adgvChip.SelectedRows[0];

            // Get the value from the "Lot" column of the selected row

            foreach (DataGridViewRow row in adgvChip.SelectedRows)
            {
                row.Cells["Picked"].Value = "";
                row.Cells["Picked by"].Value = "";
                row.Cells["Last edited"].Value = DateTime.Now;
                row.Cells["Comment"].Value = userName + " removed pick";
            }

        }

        private void buttonPick_Click(object sender, EventArgs e)
        {
            // Get the selected row
            DataGridViewRow selectedRow = adgvChip.SelectedRows[0];

            // Get the value from the "Lot" column of the selected row
            foreach (DataGridViewRow row in adgvChip.SelectedRows)
            {
                row.Cells["Picked"].Value = DateTime.Now;
                row.Cells["Picked by"].Value = userName;
                row.Cells["Last edited"].Value = DateTime.Now;
                row.Cells["Comment"].Value = userName + " picked chip";
            }
        }

        private bool CombinationExistsInSQL(string batch, string wafer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create a SQL query to check if the combination exists in Chip_inventory_list
                string query = "SELECT COUNT(*) FROM Chip_inventory_list WHERE Batch = @Batch AND Wafer = @Wafer";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Batch", batch);
                    command.Parameters.AddWithValue("@Wafer", wafer);

                    // Execute the query and check if there are any matching rows
                    int rowCount = (int)command.ExecuteScalar();

                    return rowCount > 0;
                }
            }
        }


        //private void adgvWafer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    try
        //    {
        //        if (e.ColumnIndex == adgvWafer.Columns["ButtonColumn"].Index && e.RowIndex >= 0)
        //        {
        //            string batchValue = adgvWafer.Rows[e.RowIndex].Cells["Batch"].Value.ToString();
        //            string waferValue = adgvWafer.Rows[e.RowIndex].Cells["Wafer"].Value.ToString();
        //            string needleGeneration = adgvWafer.Rows[e.RowIndex].Cells["Generation"].Value.ToString();

        //            if (CombinationExistsInSQL(batchValue, waferValue))
        //            {
        //                    // Create and show the WaferMap form
        //                    WaferMap waferMap = new WaferMap(batchValue, waferValue);
        //                    waferMap.BatchValue = batchValue;
        //                    waferMap.WaferValue = waferValue;
        //                waferMap.Generation = needleGeneration;
        //                waferMap.Text = "Information for Batch: " + batchValue + ", Wafer: " + waferValue;
        //                waferMap.Show();
        //            }
        //            else
        //            {
        //                // Show a message indicating that the combination doesn't exist
        //                MessageBox.Show("The combination of Batch and Wafer does not exist in the database.", "Combination Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred: " + ex.Message);
        //    }
        //}

        private Dictionary<int, bool> disabledButtons = new Dictionary<int, bool>();

        private void adgvWafer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == adgvWafer.Columns[buttonColumn].Index)
            {
                // Check if the button cell for this row is already disabled
                if (adgvWafer.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
                {
                    // Button cell is disabled, do nothing
                    return;
                }

                // Disable the button cell for this row
                adgvWafer.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;

                // Handle opening the form here (you can add your code for opening the form)

                string batchValue = adgvWafer.Rows[e.RowIndex].Cells["Batch"].Value.ToString();
                string waferValue = adgvWafer.Rows[e.RowIndex].Cells["Wafer"].Value.ToString();
                string needleGeneration = adgvWafer.Rows[e.RowIndex].Cells["Generation"].Value.ToString();

                if (CombinationExistsInSQL(batchValue, waferValue))
                {
                    // Create and show the WaferMap form
                    WaferMap waferMap = new WaferMap(batchValue, waferValue, this);
                    waferMap.BatchValue = batchValue;
                    waferMap.WaferValue = waferValue;
                    waferMap.Generation = needleGeneration;
                    waferMap.Text = "Information for Batch: " + batchValue + ", Wafer: " + waferValue;
                    waferMap.FormClosed += (s, args) =>
                    {
                        // Re-enable the button cell for this row when the form is closed
                        adgvWafer.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                    };
                    waferMap.Show();
                }
                else
                {
                    // Show a message indicating that the combination doesn't exist
                    MessageBox.Show("The combination of Batch and Wafer does not exist in the database.", "Combination Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void adgvChip_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && e.ColumnIndex == adgvChip.Columns[buttonColumn].Index)
            //{
            //    // Check if the button is already disabled for this row
            //    if (buttonStates.ContainsKey(e.RowIndex) && !buttonStates[e.RowIndex])
            //    {
            //        // Button is disabled, do nothing
            //        return;
            //    }

            //    // Disable the button
            //    DisableButton(e.RowIndex);

            //    // Handle opening the form here (you can add your code for opening the form)

            //    // For example, open the form and subscribe to its FormClosed event
            //    YourForm yourForm = new YourForm();
            //    yourForm.FormClosed += (s, args) =>
            //    {
            //        // Enable the button when the form is closed
            //        EnableButton(e.RowIndex);
            //    };
            //    yourForm.Show();
            //}
        }
    }
}
