using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chip_Inventory_System
{
    public partial class UserInputLot : Form
    {
        public string userInputLot { get; private set; }
        string lotName;
        public AddToLot addToLot { get; set; }
        private readonly InteractWithDatabase interactWithDatabase;
        DataTable lotData = new DataTable();
        private readonly ReadWriteData readWriteData; // Declare the field
        string destinationTableLotList = "Lot_list";
        private string connectionString = "Server=Ascilion006;Database=Chip_Inventory_Database;Integrated Security=True";
        private Form1 parentForm;

        public UserInputLot()
        {
            InitializeComponent();
            readWriteData = new ReadWriteData(); // Initialize the field
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Get the lotName from textBoxLotInput
            string lotName = textBoxLotInput.Text;

            // Create a new row for adgvLotList
            DataGridViewRow newRow = new DataGridViewRow();

            newRow.Cells.Add(new DataGridViewTextBoxCell { Value = lotName });

            // Add the new row to adgvLotList
            addToLot.adgvLotList.Rows.Add(newRow);

            // Close the userInputLot form
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
