using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zuby.ADGV;

namespace Chip_Inventory_System
{
    public partial class WaferMap : Form
    {
        private string filePathWafer;
        private string filePathWaferList;
        public string connectionString = "Server=Ascilion006;Database=Chip_Inventory_Database;Integrated Security=True";
        public string queryWafer = "SELECT * FROM Wafer_inventory_list";
        public string queryChip = "SELECT * FROM Chip_inventory_list";
        private DataTable dataTable;
        private Form1 parentForm;
        private readonly ReadWriteData readWriteData; // Declare the field

        public string BatchValue { get; set; }
        public string WaferValue { get; set; }
        public string Generation { get; set; }

        public WaferMap(string batchValue, string waferValue, Form1 parentForm)
        {
            BatchValue = batchValue;
            WaferValue = waferValue;
            Generation = Generation;
            readWriteData = new ReadWriteData(); // Initialize the field
            this.parentForm = parentForm;
            InitializeComponent();
            InitializeAdvancedDataGridView();
            adgvWaferData.SelectionChanged += adgvWaferData_SelectionChanged;
        }

        private void InitializeAdvancedDataGridView()
        {
            // Configure DataGridView properties
            adgvWaferMap.AllowUserToResizeRows = false;
            adgvWaferMap.AllowUserToResizeColumns = false;
            adgvWaferMap.Dock = DockStyle.Fill;
            adgvWaferMap.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            adgvWaferMap.RowsDefaultCellStyle = null;
            adgvWaferMap.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            adgvWaferMap.DefaultCellStyle.Padding = new Padding(0); // Remove any padding
            adgvWaferData.AllowUserToResizeRows = false;
            adgvWaferData.AllowUserToResizeColumns = false;
            this.TopMost = true; // Set to true to keep the form always on top.

            DataTable dataTable = readWriteData.ReadFromSql(connectionString, queryChip);
            string filterExpression = $"Batch = '{BatchValue}' AND Wafer = '{WaferValue}'";
            DataRow[] filteredRows = dataTable.Select(filterExpression);

            // Create a new DataTable for filtered data
            DataTable filteredTable = dataTable.Clone();

            // Import filtered rows to the new DataTable
            foreach (DataRow row in filteredRows)
            {
                filteredTable.ImportRow(row);
            }

            // Bind the filtered DataTable to adgvWaferData
            adgvWaferData.DataSource = filteredTable;

            // Set AutoSizeMode to AllCells for each column
            foreach (DataGridViewColumn column in adgvWaferData.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void WaferMap_Load_1(object sender, EventArgs e)
        {
            int columnCount = 17;
            int rowCount = 17;

            // Define the list of button numbers to label
            List<string> buttonNumbersToLabel = new List<string>
    {
            "0107", "0108", "0109", "0110", "0111", "0205", "0206", "0207", "0208", "0209",
            "0210", "0211", "0212", "0213", "0303", "0304", "0305", "0306", "0307", "0308",
            "0309", "0310", "0311", "0312", "0313", "0314", "0315", "0403", "0404", "0405",
            "0406", "0407", "0408", "0409", "0410", "0411", "0412", "0413", "0414", "0415",
            "0502", "0503", "0504", "0505", "0506", "0507", "0508", "0509", "0510", "0511",
            "0512", "0513", "0514", "0515", "0516", "0602", "0603", "0604", "0605", "0606",
            "0607", "0608", "0609", "0610", "0611", "0612", "0613", "0614", "0615", "0616",
            "0701", "0702", "0703", "0704", "0705", "0706", "0707", "0708", "0709", "0710",
            "0711", "0712", "0713", "0714", "0715", "0716", "0717", "0801", "0802", "0803",
            "0804", "0805", "0806", "0807", "0808", "0809", "0810", "0811", "0812", "0813",
            "0814", "0815", "0816", "0817", "0901", "0902", "AL1", "AL2", "0905", "0906",
            "0907", "0908", "0909", "0910", "0911", "0912", "0913", "AL3", "AL4", "0916",
            "0917", "1001", "1002", "1003", "1004", "1005", "1006", "1007", "1008", "1009",
            "1010", "1011", "1012", "1013", "1014", "1015", "1016", "1017", "1101", "1102",
            "1103", "1104", "1105", "1106", "1107", "1108", "1109", "1110", "1111", "1112",
            "1113", "1114", "1115", "1116", "1117", "1202", "1203", "1204", "1205", "1206",
            "1207", "1208", "1209", "1210", "1211", "1212", "1213", "1214", "1215", "1216",
            "1302", "1303", "1304", "1305", "1306", "1307", "1308", "1309", "1310", "1311",
            "1312", "1313", "1314", "1315", "1316", "1403", "1404", "1405", "1406", "1407",
            "1408", "1409", "1410", "1411", "1412", "1413", "1414", "1415", "1503", "1504",
            "1505", "1506", "1507", "1508", "1509", "1510", "1511", "1512", "1513", "1514",
            "1515", "1605", "1606", "1607", "1608", "1609", "1610", "1611", "1612", "1613",
            "1707", "1708", "1709", "1710", "1711"
    };

            DataTable dataTable = readWriteData.ReadFromSql(connectionString, queryChip);
            // Set up DataGridView properties
            adgvWaferMap.ColumnCount = columnCount;
            adgvWaferMap.RowCount = rowCount;

            // Set row height and column width for square cells
            int cellSize = 45;

            foreach (DataGridViewRow row in adgvWaferMap.Rows)
            {
                row.Height = cellSize; // Set row height to make cells square
            }

            foreach (DataGridViewColumn column in adgvWaferMap.Columns)
            {
                column.Width = cellSize; // Set column width to make cells square
            }

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    string buttonLabel = $"{(row + 1):D2}{(col + 1):D2}";

                    if (buttonNumbersToLabel.Contains(buttonLabel))
                    {
                        // Button should have a label
                        DataGridViewButtonCell buttonCell = new DataGridViewButtonCell();
                        buttonCell.Value = $"{buttonLabel}";
                        buttonCell.FlatStyle = FlatStyle.Flat;
                        adgvWaferMap[col, row] = buttonCell;
                    }
                    else
                    {
                        // Button should be a disabled textbox
                        DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                        adgvWaferMap[col, row] = cell;
                        cell.ReadOnly = true;
                    }
                }
            }
            RepaintWaferMap("Status", "Lot", "Picked", dataTable);
        }

        public void RepaintWaferMap(string statusColumn, string reservedColumn, string pickedColumn, DataTable dataTable)
        {
            for (int rowIndex = 0; rowIndex < adgvWaferMap.Rows.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < adgvWaferMap.ColumnCount; columnIndex++)
                {
                    DataGridViewCell cell = adgvWaferMap[columnIndex, rowIndex];

                    // Check if the cell is a button cell and not disabled
                    if (cell is DataGridViewButtonCell buttonCell
                        && !string.IsNullOrEmpty(buttonCell.Value?.ToString()))
                    {
                        // Extract the chip ID from the button's text
                        string chip = "C" + buttonCell.Value.ToString();
                        // Find the row in the DataTable where Batch, Wafer, and Chip match
                        DataRow[] matchingRows = dataTable.Select($"Batch = '{BatchValue}' AND Wafer = '{WaferValue}' AND Chip = '{chip}'");

                        if (matchingRows.Length > 0)
                        {
                            DataRow matchingRow = matchingRows[0]; // Assuming there's only one matching row

                            string statusValue = matchingRow[statusColumn].ToString();
                            string reservedValue = matchingRow[reservedColumn].ToString();
                            string pickedValue = matchingRow[pickedColumn].ToString();
                            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                            if (string.IsNullOrEmpty(reservedValue))
                            {
                                // If reservedColumn is empty
                                if (statusValue == "Fail")
                                {
                                    cellStyle.BackColor = Color.Red;
                                }
                                else if (statusValue == "Good")
                                {
                                    cellStyle.BackColor = Color.Green;
                                }
                            }
                            else
                            {
                                // If reservedColumn is not empty
                                cellStyle.BackColor = Color.Orange;
                            }
                            if (!string.IsNullOrEmpty(pickedValue))
                            {
                                // If pickedColumn is not empty
                                cellStyle.BackColor = Color.Gray;
                            }
                            adgvWaferMap.Rows[rowIndex].Cells[columnIndex].Style = cellStyle;
                        }
                        else
                        {
                            // No matching row found for this chip, handle accordingly
                            // For example, you can set a default background color
                            cell.Style.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dataTable = readWriteData.ReadFromSql(connectionString, queryChip);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files|*.pdf";
            saveFileDialog.FileName = Generation + "_" + BatchValue + "_" + WaferValue;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                DateTime saveDate = DateTime.Now;
                StoreToPDF.ExportDataTableToPDF(dataTable, fileName, BatchValue, WaferValue, Generation, saveDate, "");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                saveFileDialog.FileName = $"Chip_list_{DateTime.Now:yyyyMMdd_HHmmss}";

                // Get the selected "Generation," "Batch," and "Wafer" values from adgvWafer
                List<string> selectedValues = new List<string>();

                foreach (DataGridViewRow row in adgvWaferData.SelectedRows)
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

                    StoreToPDF.GeneratePdf(fileName, filteredDataTable, parentForm.includedColumnsChip, "");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files|*.csv";
                saveFileDialog.Title = "Save CSV File";
                saveFileDialog.FileName = $"Chip_list_{DateTime.Now:yyyyMMdd_HHmmss}";

                // Get the selected "Generation," "Batch," and "Wafer" values from adgvWafer
                List<string> selectedValues = new List<string>();

                foreach (DataGridViewRow row in adgvWaferData.SelectedRows)
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
                    StoreToCSV.ExportDataTableToCsv(filteredDataTable, parentForm.includedColumnsChip, fileName);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void adgvWaferData_SelectionChanged(object sender, EventArgs e)
        {
            bool rowsSelected = adgvWaferData.SelectedRows.Count > 0;

            // Enable or disable the buttons based on whether rows are selected
            buttonPDF.Enabled = rowsSelected;
            buttonCSV.Enabled = rowsSelected;
        }

        private List<DataGridViewCell> clickedCells = new List<DataGridViewCell>();
        private Dictionary<DataGridViewCell, ButtonClickForm> openedForms = new Dictionary<DataGridViewCell, ButtonClickForm>();

        private void adgvWaferMap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is a button cell and if it's already been clicked
            DataGridViewButtonCell buttonCell = adgvWaferMap.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;

            if (buttonCell != null && !IsCellClicked(buttonCell))
            {
                // Mark the cell as clicked
                MarkCellAsClicked(buttonCell);

                string Chip = "C" + buttonCell.Value.ToString(); // Get the value of the clicked cell
                Console.WriteLine(Chip);
                ButtonClickForm buttonClickForm = new ButtonClickForm(BatchValue, WaferValue, Chip, Generation, parentForm);
                buttonClickForm.BatchValue = BatchValue;
                buttonClickForm.WaferValue = WaferValue;
                buttonClickForm.NeedleGeneration = Generation;

                buttonClickForm.Text = "Information for Batch: " + BatchValue + ", Wafer: " + WaferValue + ", Chip: " + Chip;
                buttonClickForm.BringToFront(); // Ensure ButtonClickForm is in front

                // Handle the form closed event to remove the cell from the clicked list and close the form
                buttonClickForm.FormClosed += (s, args) =>
                {
                    UnmarkCellAsClicked(buttonCell);
                    // Remove the form from the dictionary when it's closed
                    if (openedForms.ContainsKey(buttonCell))
                    {
                        openedForms.Remove(buttonCell);
                    }
                };

                // Store the form in the dictionary
                openedForms[buttonCell] = buttonClickForm;

                buttonClickForm.Show();
            }
        }

        // Check if a cell has been clicked
        private bool IsCellClicked(DataGridViewCell cell)
        {
            return clickedCells.Contains(cell);
        }

        // Mark a cell as clicked
        private void MarkCellAsClicked(DataGridViewCell cell)
        {
            clickedCells.Add(cell);
        }

        // Unmark a cell as clicked (when the form is closed)
        private void UnmarkCellAsClicked(DataGridViewCell cell)
        {
            clickedCells.Remove(cell);
        }

        private void WaferMap_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            List<ButtonClickForm> formsToClose = new List<ButtonClickForm>();
            foreach (var kvp in openedForms)
            {
                var form = kvp.Value;
                if (!form.IsDisposed)
                {
                    formsToClose.Add(form);
                }
            }
            foreach (var form in formsToClose)
            {
                form.Close();
            }
            openedForms.Clear();
        }
    }
}
