using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chip_Inventory_System
{
    internal class InteractWithDatabase
    {
        private readonly ReadWriteData readWriteData; // Declare the field
        private string connectionString = "Server=Ascilion006;Database=Chip_Inventory_Database;Integrated Security=True";
        private string queryChip = "SELECT * FROM Chip_inventory_list";
        public InteractWithDatabase()
        {
            readWriteData = new ReadWriteData(); // Initialize the field
        }

    }
}
