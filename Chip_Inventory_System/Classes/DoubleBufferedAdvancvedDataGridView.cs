using System;
using System.Windows.Forms;
using Zuby.ADGV;

namespace Chip_Inventory_System
{
    public class DoubleBufferedAdvancedDataGridView : AdvancedDataGridView
    {
        public DoubleBufferedAdvancedDataGridView()
        {
            DoubleBuffered = true; // Enable double buffering
        }
    }
}
