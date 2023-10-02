using Chip_Inventory_System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

internal class StoreToPDF
{
    private Document document;
    private PdfWriter writer;
    public string connectionString = "Server=Ascilion006;Database=Chip_Inventory_Database;Integrated Security=True";
    public string queryWafer = "SELECT * FROM Wafer_inventory_list";
    private readonly ReadWriteData readWriteData; // Declare the field

    public StoreToPDF(Document document, Stream stream)
    {
        this.document = document;
        writer = PdfWriter.GetInstance(document, stream);
        document.Open();
    }

    public void AddText(string text)
    {
        Paragraph paragraph = new Paragraph(text);
        document.Add(paragraph);
    }

    public void Close()
    {
        document.Close();
        writer.Close();
    }

    public static StoreToPDF GetInstance(Document document, Stream stream)
    {
        return new StoreToPDF(document, stream);
    }

    private static System.Drawing.Color ConvertToSystemDrawingColor(BaseColor baseColor)
    {
        return System.Drawing.Color.FromArgb(baseColor.R, baseColor.G, baseColor.B);
    }
    public static void ExportDataTableToPDF(DataTable dataTable, string filePath, string batch, string wafer, string generation, DateTime saveDate, string selectedLot)
    {
        try
        {
            Document doc = new Document(PageSize.A4); // Use PageSize.A4 or customize as needed
            string connectionString = "Server=Ascilion006;Database=Chip_Inventory_Database;Integrated Security=True";
            string queryWafer = "SELECT * FROM Wafer_inventory_list";
            ReadWriteData readWriteData = new ReadWriteData();
            DataTable dbDataTable = readWriteData.ReadFromSql(connectionString, queryWafer);

            // Initialize the PDF writer
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            // Set the page event handler to add the header and footer
            writer.PageEvent = new PageEventHandler(batch, wafer, generation, saveDate);

            // Open the document for writing
            doc.Open();

            // Calculate the width of the page and the width of the grid (90% of the page width)
            float pageWidth = doc.PageSize.Width;
            float gridWidth = pageWidth * 0.9f;

            // Calculate cell size to make them equal height and width (squares)
            float cellSize = gridWidth / 17; // Assuming 17 columns

            // Create a table with 17 columns
            PdfPTable pdfTable = new PdfPTable(17);
            pdfTable.DefaultCell.Padding = 0;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;

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

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Populate the grid with button labels and apply conditions
            for (int row = 0; row < 17; row++) // Assuming 17 rows
            {
                for (int col = 0; col < 17; col++) // Assuming 17 columns
                {
                    string buttonLabel = $"{(row + 1):D2}{(col + 1):D2}";

                    PdfPCell pdfCell;
                    if (buttonNumbersToLabel.Contains(buttonLabel))
                    {
                        // Button should have a label
                        pdfCell = new PdfPCell();

                        // Look up the corresponding chipID in the DataTable
                        string chipID = "C" + buttonLabel;
                        DataRow[] matchingRows = dataTable.Select($"Batch = '{batch}' AND Wafer = '{wafer}' AND Chip = '{chipID}'");

                        if (matchingRows.Length > 0)
                        {
                            DataRow matchingRow = matchingRows[0]; // Assuming there's only one matching row

                            // Get the Capillary value
                            string capillaryValue = matchingRow["Capillary"].ToString();

                            // Determine the letter to add based on the Capillary value
                            string letterToAdd = " ";
                            if (capillaryValue == "Medusa")
                            {
                                letterToAdd = "M";
                            }
                            else if (capillaryValue == "Snowflake")
                            {
                                letterToAdd = "S";
                            }

                            // Create the text to add to the cell with a newline character
                            string cellText = buttonLabel + "\n" + letterToAdd;

                            // Set the text in the cell and center-align it both horizontally and vertically
                            pdfCell.Phrase = new Phrase(cellText, font);
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            string statusValue = string.Empty; // Initialize with an empty string
                            string reservedValue = matchingRow["Lot"].ToString();

                            // Check if the column "Status" exists in the DataTable
                            if (dataTable.Columns.Contains("Status"))
                            {
                                statusValue = matchingRow["Status"].ToString();
                            }
                            else
                            {
                                statusValue = "";
                                string lotValue = matchingRow["Lot"].ToString();

                                Console.WriteLine($"Selected Lot: {selectedLot}");

                                if (!string.IsNullOrEmpty(selectedLot))
                                {
                                    if (reservedValue == selectedLot)
                                    {
                                        reservedValue = matchingRow["Lot"].ToString();
                                    }
                                    else
                                    {
                                        reservedValue = "";
                                    }
                                }
                            }

                            string pickedValue = matchingRow["Picked"].ToString();

                            // Apply the specified conditions
                            BaseColor cellBackgroundColor = BaseColor.WHITE; // Default background color

                            if (string.IsNullOrEmpty(reservedValue))
                            {
                                if (!string.IsNullOrEmpty(statusValue))
                                {
                                    if (statusValue == "Good")
                                    {
                                        cellBackgroundColor = new BaseColor(Color.Green);
                                    }
                                    else if (statusValue == "Fail")
                                    {
                                        cellBackgroundColor = new BaseColor(Color.Red);
                                    }
                                }
                            }
                            else
                            {
                                cellBackgroundColor = new BaseColor(Color.Orange);
                            }

                            if (!string.IsNullOrEmpty(pickedValue))
                            {
                                cellBackgroundColor = new BaseColor(Color.Gray);
                            }

                            pdfCell.BackgroundColor = cellBackgroundColor; // Set the background color
                        }
                    }
                    else
                    {
                        // Button should be an empty cell
                        pdfCell = new PdfPCell();

                        // Set border width to 0 to remove grid lines from empty cells
                        pdfCell.BorderWidth = 0;
                    }

                    pdfCell.FixedHeight = cellSize; // Make the cells square
                    pdfTable.AddCell(pdfCell);

                }
            }

            // Add the PDF table to the document
            doc.Add(pdfTable);

            // Iterate through the DataTable to find the first row for each unique combination
            foreach (DataRow row in dbDataTable.Rows)
            {
                string dbBatch = row["Batch"].ToString();
                string dbWafer = row["Wafer"].ToString();
                string dbGeneration = row["Generation"].ToString();
                string dbCustomer = row["Customer"].ToString();
                string dbProject = row["Project"].ToString();
                string dbBackside = row["Backside"].ToString();
                string dbGlass = row["Glass"].ToString();
                string dbAccepted = row["Accepted"].ToString();
                string dbRejected = row["Rejected"].ToString();
                string dbReserved = row["Reserved"].ToString();
                string dbPicked = row["Picked"].ToString();

                // Check if the current database row matches the batch, wafer, and generation of the page
                if (dbBatch == batch && dbWafer == wafer && dbGeneration == generation)
                {
                    // Create a paragraph for the additional text
                    Paragraph additionalText = new Paragraph();
                    additionalText.Alignment = Element.ALIGN_LEFT; // Adjust alignment as needed

                    // Add the information from the database for the current combination
                    additionalText.Add($"Batch: {dbBatch}\n");
                    additionalText.Add($"Wafer: {dbWafer}\n");
                    additionalText.Add($"Generation: {dbGeneration}\n");
                    additionalText.Add($"Glass design: {dbGlass}\n");
                    additionalText.Add($"Backside: {dbBackside}\n");
                    additionalText.Add($"Customer: {dbCustomer}\n");
                    additionalText.Add($"Project: {dbProject}\n");
                    additionalText.Add($"Accepted: {dbAccepted}\n");
                    additionalText.Add($"Rejected: {dbRejected}\n");
                    additionalText.Add($"Reserved: {dbReserved}\n");
                    additionalText.Add($"Picked: {dbPicked}\n\n");

                    // Add the additional text to the document
                    doc.Add(additionalText);

                }
            }

            // Close the document and writer
            doc.Close();
            writer.Close();
            MessageBox.Show("Wafer map exported successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating PDF: {ex.Message}");
        }
    }

    public static void GeneratePdf(string fileName, DataTable dataTable, List<string> includedColumns, string selectedLot)
    {
        Document doc = new Document(PageSize.A4.Rotate()); // Landscape orientation

        try
        {
            // Create a PdfWriter instance with the custom PdfPageEvent
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
            writer.PageEvent = new CustomPdfPageEvent(fileName, selectedLot, dataTable);

            // Open the document for writing
            doc.Open();

            // Define Arial font for the entire table
            iTextSharp.text.Font font = FontFactory.GetFont("c:\\windows\\fonts\\arial.ttf", BaseFont.CP1252, BaseFont.EMBEDDED, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            iTextSharp.text.Font fontWhite = FontFactory.GetFont("c:\\windows\\fonts\\arial.ttf", BaseFont.CP1252, BaseFont.EMBEDDED, 12, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
            // Create a PdfPTable for your data with the specified included columns
            PdfPTable table = new PdfPTable(includedColumns.Count);
            table.WidthPercentage = 100; // Table should take up the entire page width
            table.HorizontalAlignment = Element.ALIGN_CENTER; // Center-align the table

            // Debug: Print included column names
            Console.WriteLine("Included Columns:");
            foreach (string columnName in includedColumns)
            {
                Console.WriteLine(columnName);
            }

            // Add table headers with custom font and background color for included columns
            PdfPCell headerCell = new PdfPCell();
            headerCell.BackgroundColor = new BaseColor(189, 0, 38); // Red background for the entire header row
            foreach (string columnName in includedColumns)
            {
                if (dataTable.Columns.Contains(columnName))
                {
                    DataColumn column = dataTable.Columns[columnName];
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, fontWhite));

                    // Set the text color to white
                    cell.BackgroundColor = new BaseColor(189, 0, 38); // Red background for the entire header row
                    cell.HorizontalAlignment = Element.ALIGN_CENTER; // Center-align header cells
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE; // Center-align vertically
                    table.AddCell(cell);
                }
            }
            table.HeaderRows = 1; // Ensure that the header row is repeated on each page

            // Iterate through rows in the DataTable
            foreach (DataRow row in dataTable.Rows)
            {
                // Add data cells for included columns to the table with custom font
                foreach (string columnName in includedColumns)
                {
                    if (dataTable.Columns.Contains(columnName))
                    {
                        PdfPCell pdfCell = new PdfPCell(new Phrase(row[columnName]?.ToString()));
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER; // Center-align data cells
                        pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE; // Center-align vertically
                        table.AddCell(pdfCell);
                    }
                }
            }

            // Add the table to the PDF document
            doc.Add(table);
            MessageBox.Show("PDF created successfully!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during PDF generation
            Console.WriteLine($"Error creating PDF: {ex.Message}");
            throw new Exception($"Error creating PDF: {ex.Message}");
        }
        finally
        {
            // Close the document and writer
            doc.Close();
        }
    }

    public class CustomPdfPageEvent : PdfPageEventHelper
    {
        private string documentName;
        private string lotName;
        private DateTime creationTime;
        private int totalPages;
        private DataTable dataTable; // Added to store the DataTable

        public CustomPdfPageEvent(string documentName, string lotName, DataTable dataTable)
        {
            this.documentName = documentName;
            this.lotName = lotName;
            this.creationTime = DateTime.Now;
            this.totalPages = 0;
            this.dataTable = dataTable; // Store the DataTable
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            totalPages++;

            // Define the header table with one column and no border
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.DefaultCell.Border = PdfPCell.NO_BORDER; // Remove border

            // Determine the header text based on conditions
            string headerText = string.Empty;
            if (string.IsNullOrEmpty(lotName))
            {
                headerText = dataTable.Columns.Contains("Chip") ? "Chip list" : "Wafer list";
            }
            else
            {
                headerText = $"Pick list for Lot {lotName}";
            }

            // Add centered text as the header
            PdfPCell headerCell = new PdfPCell(new Phrase(headerText));
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.VerticalAlignment = Element.ALIGN_TOP;
            headerCell.Border = PdfPCell.NO_BORDER; // Remove border from the cell
            headerTable.AddCell(headerCell);

            // Position the header table at the top of the page
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + 30f, writer.DirectContent);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // Define the footer table with two columns and no border
            PdfPTable footerTable = new PdfPTable(2);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.DefaultCell.Border = PdfPCell.NO_BORDER; // Remove border

            // Create a cell for the date
            PdfPCell dateCell = new PdfPCell(new Phrase(DateTime.Now.ToString()));
            dateCell.HorizontalAlignment = Element.ALIGN_LEFT;
            dateCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            dateCell.Border = PdfPCell.NO_BORDER; // Remove border from the cell
            footerTable.AddCell(dateCell);

            // Create a cell for the page number and total number of pages
            PdfPCell pageNumberCell = new PdfPCell(new Phrase($"{writer.PageNumber} ({totalPages})"));
            pageNumberCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pageNumberCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            pageNumberCell.Border = PdfPCell.NO_BORDER; // Remove border from the cell
            footerTable.AddCell(pageNumberCell);

            // Position the footer table at the bottom of the page
            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
        }
    }

    public class PageEventHandler : PdfPageEventHelper
    {
        private string batch;
        private string wafer;
        private string generation;
        private DateTime saveDate;

        public PageEventHandler(string batch, string wafer, string generation, DateTime saveDate)
        {
            this.batch = batch;
            this.generation = generation;
            this.wafer = wafer;
            this.saveDate = saveDate;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Create a PdfContentByte instance
            PdfContentByte cb = writer.DirectContent;
            cb.BeginText();

            // Set the font and size for the header
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            cb.SetFontAndSize(bf, 12);

            // Header text
            string headerText = $"Wafer map for {generation}, batch {batch}, wafer {wafer}";

            // Calculate the width of the header text
            float textWidth = bf.GetWidthPoint(headerText, 12);

            // Center the header text horizontally
            float x = (document.PageSize.Width - textWidth) / 2;

            // Calculate the Y-coordinate for the header text (adjust as needed)
            float headerY = document.PageSize.Top - 20;

            // Add the header text
            cb.SetTextMatrix(x, headerY);
            cb.ShowText(headerText);
            cb.EndText();

            // Date text
            string dateText = "Created: " + saveDate.ToString("yyyy-MM-dd");

            // Calculate the width of the date text
            float dateWidth = bf.GetWidthPoint(dateText, 12);

            // Center the date text horizontally
            float dateX = (document.PageSize.Width - dateWidth) / 2;

            // Calculate the Y-coordinate for the date text (bottom of the page)
            float dateY = document.PageSize.Bottom + 20;

            // Add the date text
            cb.BeginText();
            cb.SetTextMatrix(dateX, dateY);
            cb.ShowText(dateText);
            cb.EndText();
        }
    }
}

