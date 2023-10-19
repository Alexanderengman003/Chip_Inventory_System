namespace Chip_Inventory_System
{
    partial class AddToLot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddToLot));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonWaferMap = new System.Windows.Forms.Button();
            this.buttonPickList = new System.Windows.Forms.Button();
            this.buttonCloseLot = new System.Windows.Forms.Button();
            this.buttonCreateLot = new System.Windows.Forms.Button();
            this.buttonPick = new System.Windows.Forms.Button();
            this.buttonRemovePick = new System.Windows.Forms.Button();
            this.buttonRemoveLot = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.adgvLotList = new Zuby.ADGV.AdvancedDataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adgvLotList)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.adgvLotList, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(947, 1336);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.buttonWaferMap, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonPickList, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonCloseLot, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonCreateLot, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonPick, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonRemovePick, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonRemoveLot, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonAdd, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(622, 92);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // buttonWaferMap
            // 
            this.buttonWaferMap.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonWaferMap.Location = new System.Drawing.Point(468, 49);
            this.buttonWaferMap.Name = "buttonWaferMap";
            this.buttonWaferMap.Size = new System.Drawing.Size(151, 40);
            this.buttonWaferMap.TabIndex = 1;
            this.buttonWaferMap.Text = "Save map";
            this.buttonWaferMap.UseVisualStyleBackColor = true;
            this.buttonWaferMap.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // buttonPickList
            // 
            this.buttonPickList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPickList.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.buttonPickList.Location = new System.Drawing.Point(469, 5);
            this.buttonPickList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonPickList.Name = "buttonPickList";
            this.buttonPickList.Size = new System.Drawing.Size(149, 36);
            this.buttonPickList.TabIndex = 1;
            this.buttonPickList.Text = "Print pick list";
            this.buttonPickList.UseVisualStyleBackColor = true;
            this.buttonPickList.Click += new System.EventHandler(this.buttonPickList_Click);
            // 
            // buttonCloseLot
            // 
            this.buttonCloseLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCloseLot.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCloseLot.Location = new System.Drawing.Point(3, 49);
            this.buttonCloseLot.Name = "buttonCloseLot";
            this.buttonCloseLot.Size = new System.Drawing.Size(149, 40);
            this.buttonCloseLot.TabIndex = 2;
            this.buttonCloseLot.Text = "Close lot";
            this.buttonCloseLot.UseVisualStyleBackColor = true;
            this.buttonCloseLot.Click += new System.EventHandler(this.buttonCloseLot_Click);
            // 
            // buttonCreateLot
            // 
            this.buttonCreateLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateLot.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCreateLot.Location = new System.Drawing.Point(3, 3);
            this.buttonCreateLot.Name = "buttonCreateLot";
            this.buttonCreateLot.Size = new System.Drawing.Size(149, 40);
            this.buttonCreateLot.TabIndex = 6;
            this.buttonCreateLot.Text = "Create lot";
            this.buttonCreateLot.UseVisualStyleBackColor = true;
            this.buttonCreateLot.Click += new System.EventHandler(this.buttonCreateLot_Click_1);
            // 
            // buttonPick
            // 
            this.buttonPick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPick.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPick.Location = new System.Drawing.Point(313, 3);
            this.buttonPick.Name = "buttonPick";
            this.buttonPick.Size = new System.Drawing.Size(149, 40);
            this.buttonPick.TabIndex = 3;
            this.buttonPick.Text = "Pick";
            this.buttonPick.UseVisualStyleBackColor = true;
            this.buttonPick.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonRemovePick
            // 
            this.buttonRemovePick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemovePick.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemovePick.Location = new System.Drawing.Point(313, 49);
            this.buttonRemovePick.Name = "buttonRemovePick";
            this.buttonRemovePick.Size = new System.Drawing.Size(149, 40);
            this.buttonRemovePick.TabIndex = 4;
            this.buttonRemovePick.Text = "Unpick";
            this.buttonRemovePick.UseVisualStyleBackColor = true;
            this.buttonRemovePick.Click += new System.EventHandler(this.buttonRemovePick_Click);
            // 
            // buttonRemoveLot
            // 
            this.buttonRemoveLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemoveLot.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveLot.Location = new System.Drawing.Point(158, 49);
            this.buttonRemoveLot.Name = "buttonRemoveLot";
            this.buttonRemoveLot.Size = new System.Drawing.Size(149, 40);
            this.buttonRemoveLot.TabIndex = 5;
            this.buttonRemoveLot.Text = "Remove from lot";
            this.buttonRemoveLot.UseVisualStyleBackColor = true;
            this.buttonRemoveLot.Click += new System.EventHandler(this.buttonRemoveLot_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAdd.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(158, 3);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(149, 40);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add to lot";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // adgvLotList
            // 
            this.adgvLotList.AllowUserToAddRows = false;
            this.adgvLotList.AllowUserToDeleteRows = false;
            this.adgvLotList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvLotList.FilterAndSortEnabled = true;
            this.adgvLotList.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvLotList.Location = new System.Drawing.Point(3, 101);
            this.adgvLotList.MultiSelect = false;
            this.adgvLotList.Name = "adgvLotList";
            this.adgvLotList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.adgvLotList.RowHeadersWidth = 62;
            this.adgvLotList.RowTemplate.Height = 28;
            this.adgvLotList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.adgvLotList.Size = new System.Drawing.Size(941, 1232);
            this.adgvLotList.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvLotList.TabIndex = 2;
            this.adgvLotList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.adgvLotList_CellValueChanged);
            this.adgvLotList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.adgvLotList_DataBindingComplete);
            this.adgvLotList.SelectionChanged += new System.EventHandler(this.adgvLotList_SelectionChanged);
            // 
            // AddToLot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 1362);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddToLot";
            this.Text = "Chip manager";
            this.Load += new System.EventHandler(this.AddToLot_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.adgvLotList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonCloseLot;
        private System.Windows.Forms.Button buttonPick;
        private System.Windows.Forms.Button buttonRemovePick;
        private System.Windows.Forms.Button buttonRemoveLot;
        private System.Windows.Forms.Button buttonCreateLot;
        public Zuby.ADGV.AdvancedDataGridView adgvLotList;
        private System.Windows.Forms.Button buttonPickList;
        public System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonWaferMap;
    }
}