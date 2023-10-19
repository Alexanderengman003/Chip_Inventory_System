using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Chip_Inventory_System
{
    public partial class StatisticsForm : Form
    {
        private Form1 parentForm;
        public StatisticsForm(Form1 parentForm)
        {
            this.parentForm = parentForm;
            InitializeComponent();
        }

        private void StatisticsForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            ReadAdgvWaferToDataTable(parentForm.adgvWafer);
        }

        public void ReadAdgvWaferToDataTable(DataGridView dataGridView)
        {
            DataTable dataTable = new DataTable();

            // Add columns to the DataTable with the same names as the DataGridView columns
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.HeaderText);
            }

            // Populate the DataTable with values from the DataGridView
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataRow dataRow = dataTable.NewRow();

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dataRow[cell.ColumnIndex] = cell.Value;
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            if (dataTable != null)
            {
                decimal sumAccepted = 0;
                decimal sumRejected = 0;
                decimal sumPicked = 0;
                decimal sumReserved = 0;
                Console.WriteLine(sumAccepted);

                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["Accepted"] != DBNull.Value && decimal.TryParse(row["Accepted"].ToString(), out decimal acceptedValue))
                    {
                        sumAccepted += acceptedValue;
                        Console.WriteLine(sumAccepted);
                    }

                    if (row["Rejected"] != DBNull.Value && decimal.TryParse(row["Rejected"].ToString(), out decimal rejectedValue))
                    {
                        sumRejected += rejectedValue;
                    }
                    if (row["Picked"] != DBNull.Value && decimal.TryParse(row["Picked"].ToString(), out decimal pickedValue))
                    {
                        sumPicked += pickedValue;
                    }
                    if (row["Reserved"] != DBNull.Value && decimal.TryParse(row["Reserved"].ToString(), out decimal reservedValue))
                    {
                        sumReserved += reservedValue;
                    }
                }

                decimal total = sumAccepted + sumRejected;

                if (total == 0) // Guard against divide by zero
                {
                    MessageBox.Show("Total count is zero. Cannot compute percentage.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // or handle in another suitable manner
                }

                decimal result = sumAccepted / total * 100; // Calculate as a percentage
                decimal resultPicked = sumPicked / total * 100; // Calculate as a percentage
                decimal resultReserved = sumReserved / total * 100; // Calculate as a percentage

                label1.Text = sumAccepted.ToString();
                label2.Text = result.ToString("0.00") + "%"; // Format as a percentage with two decimal places
                label3.Text = sumPicked.ToString();
                label5.Text = resultPicked.ToString("0.00") + "%"; // Format as a percentage with two decimal places
                label4.Text = sumReserved.ToString();
                label6.Text = resultReserved.ToString("0.00") + "%"; // Format as a percentage with two decimal places

                int percentage = (int)result; // Cast the decimal to int
                int percentagePicked = (int)resultPicked; // Cast the decimal to int
                int percentageReserved = (int)resultReserved; // Cast the decimal to int

                void FormatChart(System.Windows.Forms.DataVisualization.Charting.Chart chart)
                {
                    chart.Legends[0].Docking = Docking.Bottom;
                    chart.Legends[0].Alignment = StringAlignment.Center;
                    chart.Legends[0].IsDockedInsideChartArea = false;
                    chart.Legends[0].Position = new ElementPosition(10, 70, 80, 10);
                    chart.ChartAreas[0].Position = new ElementPosition(0, 0, 100, 80); // This sets the chart to fill its container but leaves some space at the bottom for the legend.
                    chart.Legends[0].Font = new Font("Arial", 9, FontStyle.Regular); // Adjust font size and style as needed
                    chart.Legends[0].TableStyle = LegendTableStyle.Wide;
                    chart.Series["Series1"].LabelFormat = "{0}%";
                }

                // Apply the format to all charts
                FormatChart(chart1);
                FormatChart(chart2);
                FormatChart(chart3);

                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                chart1.Series["Series1"].LabelFormat = "{0}%";  // Format the label to show value followed by a percentage sign
                chart1.Series["Series1"].IsValueShownAsLabel = true;
                chart1.Series["Series1"].Points.AddXY("Accepted", percentage);
                chart1.Series["Series1"].Points[0].Color = Color.Green; // LightGreen
                chart1.Series["Series1"].Points.AddXY("Rejected", 100 - percentage);
                chart1.Series["Series1"].Points[1].Color = Color.LightGray; // Change to your desired color

                foreach (var series in chart2.Series)
                {
                    series.Points.Clear();
                }
                chart2.Series["Series1"].IsValueShownAsLabel = true;
                chart2.Series["Series1"].Points.AddXY("Picked", percentagePicked);
                chart2.Series["Series1"].Points[0].Color = Color.Orange; // Change to your desired color
                chart2.Series["Series1"].Points.AddXY("Remaining", 100 - percentagePicked);
                chart2.Series["Series1"].Points[1].Color = Color.LightGray; // Change to your desired color

                foreach (var series in chart3.Series)
                {
                    series.Points.Clear();
                }
                chart3.Series["Series1"].IsValueShownAsLabel = true;
                chart3.Series["Series1"].Points.AddXY("Reserved", percentageReserved);
                chart3.Series["Series1"].Points[0].Color = Color.LightBlue; // Change to your desired color
                chart3.Series["Series1"].Points.AddXY("Available", 100 - percentageReserved);
                chart3.Series["Series1"].Points[1].Color = Color.LightGray; // Change to your desired color
            }
        }
    }
}
