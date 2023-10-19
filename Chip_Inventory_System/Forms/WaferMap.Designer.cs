namespace Chip_Inventory_System
{
    partial class WaferMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaferMap));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.adgvWaferData = new Chip_Inventory_System.DoubleBufferedAdvancedDataGridView();
            this.adgvWaferMap = new Chip_Inventory_System.DoubleBufferedAdvancedDataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPDF = new System.Windows.Forms.Button();
            this.buttonCSV = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adgvWaferData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adgvWaferMap)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.adgvWaferData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.adgvWaferMap, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.672823F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.89077F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.45951F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1155, 1594);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // adgvWaferData
            // 
            this.adgvWaferData.AllowUserToAddRows = false;
            this.adgvWaferData.AllowUserToDeleteRows = false;
            this.adgvWaferData.AllowUserToResizeColumns = false;
            this.adgvWaferData.AllowUserToResizeRows = false;
            this.adgvWaferData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvWaferData.FilterAndSortEnabled = true;
            this.adgvWaferData.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvWaferData.Location = new System.Drawing.Point(3, 93);
            this.adgvWaferData.Name = "adgvWaferData";
            this.adgvWaferData.ReadOnly = true;
            this.adgvWaferData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.adgvWaferData.RowHeadersWidth = 62;
            this.adgvWaferData.Size = new System.Drawing.Size(1149, 271);
            this.adgvWaferData.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvWaferData.TabIndex = 0;
            this.adgvWaferData.SelectionChanged += new System.EventHandler(this.adgvWaferData_SelectionChanged);
            // 
            // adgvWaferMap
            // 
            this.adgvWaferMap.AllowUserToAddRows = false;
            this.adgvWaferMap.AllowUserToDeleteRows = false;
            this.adgvWaferMap.AllowUserToResizeColumns = false;
            this.adgvWaferMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvWaferMap.ColumnHeadersVisible = false;
            this.adgvWaferMap.FilterAndSortEnabled = true;
            this.adgvWaferMap.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvWaferMap.Location = new System.Drawing.Point(3, 378);
            this.adgvWaferMap.Name = "adgvWaferMap";
            this.adgvWaferMap.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.adgvWaferMap.RowHeadersVisible = false;
            this.adgvWaferMap.RowHeadersWidth = 62;
            this.adgvWaferMap.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.adgvWaferMap.Size = new System.Drawing.Size(1149, 1213);
            this.adgvWaferMap.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvWaferMap.TabIndex = 1;
            this.adgvWaferMap.TabStop = false;
            this.adgvWaferMap.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.adgvWaferMap_CellContentClick);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.buttonPDF, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonCSV, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.button3, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1149, 83);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // buttonPDF
            // 
            this.buttonPDF.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonPDF.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPDF.Location = new System.Drawing.Point(3, 11);
            this.buttonPDF.Name = "buttonPDF";
            this.buttonPDF.Size = new System.Drawing.Size(120, 60);
            this.buttonPDF.TabIndex = 0;
            this.buttonPDF.Text = "Save to PDF";
            this.buttonPDF.UseVisualStyleBackColor = true;
            this.buttonPDF.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCSV
            // 
            this.buttonCSV.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCSV.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCSV.Location = new System.Drawing.Point(129, 11);
            this.buttonCSV.Name = "buttonCSV";
            this.buttonCSV.Size = new System.Drawing.Size(120, 60);
            this.buttonCSV.TabIndex = 1;
            this.buttonCSV.Text = "Save to CSV";
            this.buttonCSV.UseVisualStyleBackColor = true;
            this.buttonCSV.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(255, 11);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 60);
            this.button3.TabIndex = 2;
            this.button3.Text = "Save map";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // WaferMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 1618);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WaferMap";
            this.Text = "Wafer map";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WaferMap_FormClosed_1);
            this.Load += new System.EventHandler(this.WaferMap_Load_1);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.adgvWaferData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adgvWaferMap)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DoubleBufferedAdvancedDataGridView adgvWaferData;
        public DoubleBufferedAdvancedDataGridView adgvWaferMap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonPDF;
        private System.Windows.Forms.Button buttonCSV;
        private System.Windows.Forms.Button button3;
    }
}