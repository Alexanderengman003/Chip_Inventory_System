using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Chip_Inventory_System
{
    public partial class ButtonClickForm : Form
    {
        private readonly ReadWriteData readWriteData;
        private Form1 parentForm;

        public string BatchValue { get; set; }
        public string WaferValue { get; set; }
        public string Chip { get; set; }
        public string NeedleGeneration { get; set; }

        private bool rightArrowKeyPressed = false; // Track the state of the right arrow key
        private bool leftArrowKeyPressed = false;  // Track the state of the left arrow key
        private bool enterKeyPressed = false; // Track the state of the right arrow key
        private bool escKeyPressed = false;  // Track the state of the left arrow key
        private bool isLoadingImage = false;

        public ButtonClickForm(string batchValue, string waferValue, string chip, string needleGeneration, Form1 parentForm)
        {
            InitializeComponent();
            InitializeAdvancedDataGridView();
            adgvChipData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            adgvChipData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.parentForm = parentForm;
            BatchValue = batchValue;
            WaferValue = waferValue;
            NeedleGeneration = needleGeneration;
            Chip = chip;
            readWriteData = new ReadWriteData();

            // Set KeyPreview to true to ensure the form receives key events
            this.KeyPreview = true;

            // Subscribe to the KeyDown event
            this.KeyDown += ButtonClickForm_KeyDown;

            // Subscribe to the KeyUp event
            this.KeyUp += ButtonClickForm_KeyUp;

            adgvChipData.CellValueChanged += adgvChipData_CellValueChanged; // Subscribe to the event
        }

        private void InitializeAdvancedDataGridView()
        {
            this.TopMost = true; // Set to true to keep the form always on top.
            adgvChipData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            adgvChipData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            adgvChipData.AllowUserToResizeRows = false;
            adgvChipData.AllowUserToResizeColumns = false;

            // Set the Dock property to Fill to make it fill the cell in the TableLayoutPanel
            adgvChipData.Dock = DockStyle.Fill;
        }

        //public void ShowImageForChip(string chipID)
        //{
        //    try
        //    {
        //        string filePathImages = $@"Z:\Shared\Ascilion\Engineering\MEASUREMENTS\_Incoming Inspection ASNE\{NeedleGeneration}\{BatchValue}\{BatchValue} {WaferValue}\AOI\NI\02_Image_processing";

        //        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        //        // Construct the expected file name pattern
        //        string fileNamePattern = $"{BatchValue}_{WaferValue}_{chipID}_NI.tif";

        //        // Search for the file in the specified directory (filePath)
        //        string[] matchingFiles = Directory.GetFiles(filePathImages, fileNamePattern);

        //        // Assuming there is only one matching file, load and display it
        //        string imagePath = matchingFiles[0];

        //        // Load the image into pictureBox1 with SizeMode set to Zoom
        //        pictureBox1.Image = Image.FromFile(imagePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred while loading the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        pictureBox1.Image = Properties.Resources.error; // Set a default image
        //    }
        //}

        public async void ShowImageForChip(string chipID)
        {
            try
            {
                this.KeyPreview = false; // Disable key input while loading

                string filePathImages = $@"Z:\Shared\Ascilion\Engineering\MEASUREMENTS\_Incoming Inspection ASNE\{NeedleGeneration}\{BatchValue}\{BatchValue} {WaferValue}\AOI\NI\02_Image_processing";

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                // Construct the expected file name pattern
                string fileNamePattern = $"{BatchValue}_{WaferValue}_{chipID}_NI.tif";

                // Search for the file in the specified directory (filePath)
                string[] matchingFiles = Directory.GetFiles(filePathImages, fileNamePattern);

                // Assuming there is only one matching file
                string imagePath = matchingFiles[0];

                // Reset the ProgressBar to zero
                progressBar1.Value = 0;

                // Create a CancellationTokenSource to cancel the operation if needed
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    // Subscribe to the cancellation event
                    cts.Token.Register(() =>
                    {
                    });

                    // Load the image asynchronously in the background with progress reporting
                    Image image = await LoadImageAsync(imagePath, cts.Token, progressBar1);

                    // Dispose of the previous image, if any
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                    }

                    // Set the loaded image to the PictureBox
                    pictureBox1.Image = image;

                }
                this.KeyPreview = true; // Re-enable key input after loading
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictureBox1.Image = Properties.Resources.error; // Set a default image
                this.KeyPreview = true; // Re-enable key input in case of an error
            }
        }

        private async Task<Image> LoadImageAsync(string imagePath, CancellationToken cancellationToken, System.Windows.Forms.ProgressBar progressBar)
        {
            try
            {
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] imageData = new byte[fileStream.Length];
                    int bytesRead = 0;
                    int totalBytesRead = 0;

                    while (totalBytesRead < imageData.Length &&
                        (bytesRead = await fileStream.ReadAsync(imageData, totalBytesRead, imageData.Length - totalBytesRead, cancellationToken)) > 0)
                    {
                        totalBytesRead += bytesRead;

                        // Calculate the progress percentage and update the ProgressBar
                        int progressPercentage = (int)((double)totalBytesRead / imageData.Length * 100);
                        progressBar.Value = progressPercentage;
                    }

                    // Load the image directly from the byte array
                    return Image.FromStream(new MemoryStream(imageData));
                }
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception to handle it at a higher level if needed
            }
        }
        private void ButtonClickForm_Load(object sender, EventArgs e)
        {
            ShowImageForChip(Chip);

            // Read data from SQL and filter for the matching row
            DataTable dataTable = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryChip);

            // Create a new DataTable with the same schema as the original DataTable
            DataTable filteredDataTable = dataTable.Clone();

            DataRow[] matchingRows = dataTable.Select($"Batch = '{BatchValue}' AND Wafer = '{WaferValue}' AND Chip = '{Chip}'");

            if (matchingRows.Length > 0)
            {
                // Add the matching row to the filtered DataTable
                DataRow matchingRow = matchingRows[0];
                filteredDataTable.ImportRow(matchingRow);
            }
            else
            {
                // Handle the case where no matching row was found
                MessageBox.Show("No matching data found for the specified batch, wafer, and chip.");
            }

            // Set the DataGridView's DataSource to the filtered DataTable
            adgvChipData.DataSource = filteredDataTable;
        }

        private void UpdateChipData(string status)
        {
            // Iterate through the rows
            foreach (DataGridViewRow row in adgvChipData.Rows)
            {
                if (!row.IsNewRow)
                {
                    // Update the specified columns for each row
                    row.Cells["Manual inspection"].Value = status;
                    row.Cells["Manual inspection date"].Value = DateTime.Now;
                    row.Cells["Manual inspection by"].Value = parentForm.userName;
                    string firstCharacterToLower = char.ToLower(status[0]) + status.Substring(1);
                    row.Cells["Comment"].Value = $"{parentForm.userName} {firstCharacterToLower}ed chip";
                    row.Cells["Last edited"].Value = DateTime.Now;
                }
            }

            // Refresh the DataGridView to reflect the changes
            adgvChipData.Refresh();
        }

        private void ButtonClickForm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && !enterKeyPressed)
            {
                enterKeyPressed = true; // Mark the right arrow key as pressed
                // Display a message box to confirm passing the chip
                DialogResult result = MessageBox.Show("Do you want to pass this chip?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // Update adgvChipData with "Pass" status
                    UpdateChipData("Pass");
                }
            }
            else if (e.KeyCode == Keys.Escape && !escKeyPressed)
            {
                escKeyPressed = true; // Mark the right arrow key as pressed
                // Display a message box to confirm failing the chip
                DialogResult result = MessageBox.Show("Do you want to fail this chip?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // Update adgvChipData with "Fail" status
                    UpdateChipData("Fail");
                }
            }

            if (e.KeyCode == Keys.Right && !rightArrowKeyPressed)
            {
                rightArrowKeyPressed = true; // Mark the right arrow key as pressed
                FindAndShowNextChip(e);
            }
            else if (e.KeyCode == Keys.Left && !leftArrowKeyPressed)
            {
                leftArrowKeyPressed = true; // Mark the left arrow key as pressed
                FindAndShowNextChip(e);
            }
        }

        private void ButtonClickForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                rightArrowKeyPressed = false; // Mark the right arrow key as released
            }
            else if (e.KeyCode == Keys.Left)
            {
                leftArrowKeyPressed = false; // Mark the left arrow key as released
            }
            else if (e.KeyCode == Keys.Enter)
            {
                enterKeyPressed = false; // Mark the left arrow key as released
            }
            else if (e.KeyCode == Keys.Escape)
            {
                escKeyPressed = false; // Mark the left arrow key as released
            }
        }

        private void FindAndShowNextChip(KeyEventArgs e)
        {
            // Read data from SQL and filter for the matching row
            DataTable dataTable = readWriteData.ReadFromSql(parentForm.connectionString, parentForm.queryChip);

            // Create a new DataTable with the same schema as the original
            DataTable filteredDataTable = dataTable.Clone();
            foreach (DataRow row in dataTable.Rows)
            {
                // Check if the current row meets the filter conditions
                if (row["Generation"].ToString() == NeedleGeneration &&
                    row["Batch"].ToString() == BatchValue &&
                    row["Wafer"].ToString() == WaferValue)
                {
                    // Import the row into the filteredDataTable
                    filteredDataTable.ImportRow(row);
                }
            }

            // Find the row that contains the value of 'Chip' in the 'Chip' column
            DataRow chipRow = filteredDataTable.AsEnumerable()
                .FirstOrDefault(r => r.Field<string>("Chip") == $"{Chip}");

            // Get the value of 'ID' from the found row
            string ID = chipRow.Field<string>("ID");

            if (e.KeyCode == Keys.Right)
            {
                int currentID = int.Parse(ID);

                // Check if the current ID does not belong to the last row of filteredDataTable
                if (chipRow != filteredDataTable.AsEnumerable().LastOrDefault())
                {
                    currentID++;
                    ID = currentID.ToString();
                }
                else
                {
                    return;
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                int currentID = int.Parse(ID);

                // Check if the current ID does not belong to the first row of filteredDataTable
                if (chipRow != filteredDataTable.AsEnumerable().FirstOrDefault())
                {
                    currentID--;
                    ID = currentID.ToString();

                    // Reset chipRow to the new row
                    chipRow = filteredDataTable.AsEnumerable()
                        .FirstOrDefault(r => r.Field<string>("ID") == ID);
                }
                else
                {
                    return;
                }
            }


            DataRow IDRow = filteredDataTable.AsEnumerable()
                .FirstOrDefault(r => r.Field<string>("ID") == ID);

            if (IDRow != null)
            {
                // Get the Chip value from the IDRow
                Chip = IDRow.Field<string>("Chip");
                ShowImageForChip(Chip);
                // Create a new DataTable with the same schema as the original
                DataTable singleRowTable = IDRow.Table.Clone();

                // Import the matching row into the new DataTable
                singleRowTable.ImportRow(IDRow);

                // Set adgvChipData's data source to the new DataTable containing only the matching row
                adgvChipData.DataSource = singleRowTable;
                this.Text = "Information for Batch: " + BatchValue + ", Wafer: " + WaferValue + ", Chip: " + Chip;
            }
            else
            {
                Console.WriteLine("Row not found with the specified ID.");
            }

            // Set the focus back to the form
            this.Focus();
        }

        private void adgvChipData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Assuming newValue is the new value entered by the user
                object newValue = adgvChipData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string columnName = adgvChipData.Columns[e.ColumnIndex].DataPropertyName;

                int rowID = Convert.ToInt32(adgvChipData.Rows[e.RowIndex].Cells["ID"].Value);

                readWriteData.WriteToSqlReplace(parentForm.destinationTableChip, rowID, columnName, newValue);
            }
        }
    }
}
