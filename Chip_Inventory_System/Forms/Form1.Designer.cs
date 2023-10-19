namespace Chip_Inventory_System
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabContro = new System.Windows.Forms.TabControl();
            this.tabPageWafer = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.adgvWafer = new Zuby.ADGV.AdvancedDataGridView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPDFWafer = new System.Windows.Forms.Button();
            this.buttonCSVWafer = new System.Windows.Forms.Button();
            this.buttonDeleteWafer = new System.Windows.Forms.Button();
            this.buttonAddRowWafer = new System.Windows.Forms.Button();
            this.buttonStatistics = new System.Windows.Forms.Button();
            this.tabPageChip = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPDFChip = new System.Windows.Forms.Button();
            this.buttonCSVChip = new System.Windows.Forms.Button();
            this.buttonAddLot = new System.Windows.Forms.Button();
            this.tabPageStock = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.adgvLot = new Zuby.ADGV.AdvancedDataGridView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.adgvChip = new Chip_Inventory_System.DoubleBufferedAdvancedDataGridView();
            this.tabContro.SuspendLayout();
            this.tabPageWafer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adgvWafer)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tabPageChip.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPageStock.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adgvLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adgvChip)).BeginInit();
            this.SuspendLayout();
            // 
            // tabContro
            // 
            this.tabContro.Controls.Add(this.tabPageWafer);
            this.tabContro.Controls.Add(this.tabPageChip);
            this.tabContro.Controls.Add(this.tabPageStock);
            this.tabContro.Location = new System.Drawing.Point(0, 0);
            this.tabContro.Name = "tabContro";
            this.tabContro.SelectedIndex = 0;
            this.tabContro.Size = new System.Drawing.Size(3626, 1868);
            this.tabContro.TabIndex = 0;
            // 
            // tabPageWafer
            // 
            this.tabPageWafer.Controls.Add(this.tableLayoutPanel1);
            this.tabPageWafer.Location = new System.Drawing.Point(4, 29);
            this.tabPageWafer.Name = "tabPageWafer";
            this.tabPageWafer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWafer.Size = new System.Drawing.Size(3618, 1835);
            this.tabPageWafer.TabIndex = 0;
            this.tabPageWafer.Text = "Wafer view";
            this.tabPageWafer.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.adgvWafer, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(3604, 1822);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // adgvWafer
            // 
            this.adgvWafer.AllowUserToAddRows = false;
            this.adgvWafer.AllowUserToResizeColumns = false;
            this.adgvWafer.AllowUserToResizeRows = false;
            this.adgvWafer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvWafer.FilterAndSortEnabled = true;
            this.adgvWafer.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvWafer.Location = new System.Drawing.Point(3, 94);
            this.adgvWafer.Name = "adgvWafer";
            this.adgvWafer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.adgvWafer.RowHeadersWidth = 62;
            this.adgvWafer.RowTemplate.Height = 28;
            this.adgvWafer.Size = new System.Drawing.Size(3598, 1718);
            this.adgvWafer.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvWafer.TabIndex = 0;
            this.adgvWafer.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.adgvWafer_CellClick);
            this.adgvWafer.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.adgvWafer_CellValueChanged);
            this.adgvWafer.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.adgvWafer_DataBindingComplete);
            this.adgvWafer.SelectionChanged += new System.EventHandler(this.adgvWafer_SelectionChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 6;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.buttonPDFWafer, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonCSVWafer, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonDeleteWafer, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonAddRowWafer, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonStatistics, 4, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(3598, 85);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // buttonPDFWafer
            // 
            this.buttonPDFWafer.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPDFWafer.Location = new System.Drawing.Point(3, 3);
            this.buttonPDFWafer.Name = "buttonPDFWafer";
            this.buttonPDFWafer.Size = new System.Drawing.Size(180, 78);
            this.buttonPDFWafer.TabIndex = 1;
            this.buttonPDFWafer.Text = "Save to PDF";
            this.buttonPDFWafer.UseVisualStyleBackColor = true;
            this.buttonPDFWafer.Click += new System.EventHandler(this.buttonPDFWafer_Click);
            // 
            // buttonCSVWafer
            // 
            this.buttonCSVWafer.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCSVWafer.Location = new System.Drawing.Point(189, 3);
            this.buttonCSVWafer.Name = "buttonCSVWafer";
            this.buttonCSVWafer.Size = new System.Drawing.Size(180, 78);
            this.buttonCSVWafer.TabIndex = 2;
            this.buttonCSVWafer.Text = "Save to CSV";
            this.buttonCSVWafer.UseVisualStyleBackColor = true;
            this.buttonCSVWafer.Click += new System.EventHandler(this.buttonCSVWafer_Click);
            // 
            // buttonDeleteWafer
            // 
            this.buttonDeleteWafer.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeleteWafer.Location = new System.Drawing.Point(375, 3);
            this.buttonDeleteWafer.Name = "buttonDeleteWafer";
            this.buttonDeleteWafer.Size = new System.Drawing.Size(180, 78);
            this.buttonDeleteWafer.TabIndex = 3;
            this.buttonDeleteWafer.Text = "Delete row";
            this.buttonDeleteWafer.UseVisualStyleBackColor = true;
            // 
            // buttonAddRowWafer
            // 
            this.buttonAddRowWafer.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddRowWafer.Location = new System.Drawing.Point(561, 3);
            this.buttonAddRowWafer.Name = "buttonAddRowWafer";
            this.buttonAddRowWafer.Size = new System.Drawing.Size(180, 78);
            this.buttonAddRowWafer.TabIndex = 4;
            this.buttonAddRowWafer.Text = "Add row";
            this.buttonAddRowWafer.UseVisualStyleBackColor = true;
            // 
            // buttonStatistics
            // 
            this.buttonStatistics.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.buttonStatistics.Location = new System.Drawing.Point(747, 3);
            this.buttonStatistics.Name = "buttonStatistics";
            this.buttonStatistics.Size = new System.Drawing.Size(180, 78);
            this.buttonStatistics.TabIndex = 5;
            this.buttonStatistics.Text = "Statistics";
            this.buttonStatistics.UseVisualStyleBackColor = true;
            this.buttonStatistics.Click += new System.EventHandler(this.buttonStatistics_Click);
            // 
            // tabPageChip
            // 
            this.tabPageChip.Controls.Add(this.tableLayoutPanel2);
            this.tabPageChip.Location = new System.Drawing.Point(4, 29);
            this.tabPageChip.Name = "tabPageChip";
            this.tabPageChip.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChip.Size = new System.Drawing.Size(3618, 1835);
            this.tabPageChip.TabIndex = 1;
            this.tabPageChip.Text = "Chip view";
            this.tabPageChip.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.adgvChip, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(3608, 1823);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 9;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.buttonPDFChip, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonCSVChip, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonAddLot, 4, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(3602, 85);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // buttonPDFChip
            // 
            this.buttonPDFChip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonPDFChip.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPDFChip.Location = new System.Drawing.Point(3, 13);
            this.buttonPDFChip.Name = "buttonPDFChip";
            this.buttonPDFChip.Size = new System.Drawing.Size(120, 58);
            this.buttonPDFChip.TabIndex = 0;
            this.buttonPDFChip.Text = "Save to PDF";
            this.buttonPDFChip.UseVisualStyleBackColor = true;
            this.buttonPDFChip.Click += new System.EventHandler(this.buttonPDFChip_Click);
            // 
            // buttonCSVChip
            // 
            this.buttonCSVChip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCSVChip.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCSVChip.Location = new System.Drawing.Point(129, 13);
            this.buttonCSVChip.Name = "buttonCSVChip";
            this.buttonCSVChip.Size = new System.Drawing.Size(120, 58);
            this.buttonCSVChip.TabIndex = 1;
            this.buttonCSVChip.Text = "Save to CSV";
            this.buttonCSVChip.UseVisualStyleBackColor = true;
            this.buttonCSVChip.Click += new System.EventHandler(this.buttonCSVChip_Click);
            // 
            // buttonAddLot
            // 
            this.buttonAddLot.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonAddLot.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddLot.Location = new System.Drawing.Point(255, 13);
            this.buttonAddLot.Name = "buttonAddLot";
            this.buttonAddLot.Size = new System.Drawing.Size(120, 58);
            this.buttonAddLot.TabIndex = 2;
            this.buttonAddLot.Text = "Manage lots";
            this.buttonAddLot.UseVisualStyleBackColor = true;
            this.buttonAddLot.Click += new System.EventHandler(this.buttonLot_Click);
            // 
            // tabPageStock
            // 
            this.tabPageStock.Controls.Add(this.tableLayoutPanel5);
            this.tabPageStock.Location = new System.Drawing.Point(4, 29);
            this.tabPageStock.Name = "tabPageStock";
            this.tabPageStock.Size = new System.Drawing.Size(3618, 1835);
            this.tabPageStock.TabIndex = 2;
            this.tabPageStock.Text = "Stock";
            this.tabPageStock.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.adgvLot, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(3614, 1832);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(3608, 85);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // adgvLot
            // 
            this.adgvLot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.adgvLot.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvLot.FilterAndSortEnabled = true;
            this.adgvLot.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvLot.Location = new System.Drawing.Point(3, 94);
            this.adgvLot.Name = "adgvLot";
            this.adgvLot.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.adgvLot.RowHeadersWidth = 62;
            this.adgvLot.RowTemplate.Height = 28;
            this.adgvLot.Size = new System.Drawing.Size(3608, 1735);
            this.adgvLot.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvLot.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "blueFlag.ico");
            this.imageList1.Images.SetKeyName(1, "orangeFlag.ico");
            this.imageList1.Images.SetKeyName(2, "redFlag.ico");
            this.imageList1.Images.SetKeyName(3, "greenFlag.ico");
            // 
            // adgvChip
            // 
            this.adgvChip.AllowUserToAddRows = false;
            this.adgvChip.AllowUserToDeleteRows = false;
            this.adgvChip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvChip.FilterAndSortEnabled = true;
            this.adgvChip.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvChip.Location = new System.Drawing.Point(3, 94);
            this.adgvChip.Name = "adgvChip";
            this.adgvChip.ReadOnly = true;
            this.adgvChip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.adgvChip.RowHeadersWidth = 62;
            this.adgvChip.RowTemplate.Height = 28;
            this.adgvChip.Size = new System.Drawing.Size(3602, 1692);
            this.adgvChip.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvChip.TabIndex = 0;
            this.adgvChip.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.adgvChip_CellClick);
            this.adgvChip.SelectionChanged += new System.EventHandler(this.adgvChip_SelectionChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(3627, 1868);
            this.Controls.Add(this.tabContro);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chip Inventory System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabContro.ResumeLayout(false);
            this.tabPageWafer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.adgvWafer)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tabPageChip.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabPageStock.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.adgvLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adgvChip)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabContro;
        private System.Windows.Forms.TabPage tabPageWafer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tabPageChip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button buttonPDFChip;
        private System.Windows.Forms.Button buttonCSVChip;
        private System.Windows.Forms.Button buttonPDFWafer;
        private System.Windows.Forms.Button buttonCSVWafer;
        private System.Windows.Forms.Button buttonDeleteWafer;
        private System.Windows.Forms.TabPage tabPageStock;
        private System.Windows.Forms.Button buttonAddRowWafer;
        private System.Windows.Forms.Button buttonAddLot;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        public Zuby.ADGV.AdvancedDataGridView adgvLot;
        //public Zuby.ADGV.AdvancedDataGridView adgvChip;
        public Chip_Inventory_System.DoubleBufferedAdvancedDataGridView adgvChip;
        private System.Windows.Forms.ImageList imageList1;
        public Zuby.ADGV.AdvancedDataGridView adgvWafer;
        private System.Windows.Forms.Button buttonStatistics;
    }
}

