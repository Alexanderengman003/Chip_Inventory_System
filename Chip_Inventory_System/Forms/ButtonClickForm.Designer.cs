namespace Chip_Inventory_System
{
    partial class ButtonClickForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonClickForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.adgvChipData = new Zuby.ADGV.AdvancedDataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adgvChipData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.adgvChipData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(806, 956);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // adgvChipData
            // 
            this.adgvChipData.AllowUserToAddRows = false;
            this.adgvChipData.AllowUserToDeleteRows = false;
            this.adgvChipData.AllowUserToResizeColumns = false;
            this.adgvChipData.AllowUserToResizeRows = false;
            this.adgvChipData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adgvChipData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adgvChipData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.adgvChipData.FilterAndSortEnabled = true;
            this.adgvChipData.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvChipData.Location = new System.Drawing.Point(3, 23);
            this.adgvChipData.Name = "adgvChipData";
            this.adgvChipData.ReadOnly = true;
            this.adgvChipData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.adgvChipData.RowHeadersWidth = 62;
            this.adgvChipData.RowTemplate.Height = 28;
            this.adgvChipData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.adgvChipData.Size = new System.Drawing.Size(800, 124);
            this.adgvChipData.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.adgvChipData.TabIndex = 0;
            this.adgvChipData.TabStop = false;
            this.adgvChipData.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.adgvChipData_CellValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 153);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 800);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(3, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(800, 14);
            this.progressBar1.TabIndex = 2;
            // 
            // ButtonClickForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 956);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1800, 2024);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "ButtonClickForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ButtonClickForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ButtonClickForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ButtonClickForm_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.adgvChipData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Zuby.ADGV.AdvancedDataGridView adgvChipData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
