namespace MPI.Tester.GuiComponent.TestItemComponent
{
    partial class CustomizeListComponent
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMode = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.colStart = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colEnd = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colCnt = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colFT = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colOT = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colNPLC = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colAutoRange = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colClamp = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.btnDelet = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewX1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnDelet);
            this.splitContainer1.Panel2.Controls.Add(this.btnAdd);
            this.splitContainer1.Size = new System.Drawing.Size(1048, 380);
            this.splitContainer1.SplitterDistance = 781;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colMode,
            this.colStart,
            this.colEnd,
            this.colCnt,
            this.colFT,
            this.colOT,
            this.colNPLC,
            this.colAutoRange,
            this.colClamp});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowHeadersVisible = false;
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.Size = new System.Drawing.Size(781, 380);
            this.dataGridViewX1.TabIndex = 0;
            // 
            // colID
            // 
            this.colID.FillWeight = 40F;
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Width = 40;
            // 
            // colMode
            // 
            this.colMode.DropDownHeight = 106;
            this.colMode.DropDownWidth = 121;
            this.colMode.FillWeight = 80F;
            this.colMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colMode.HeaderText = "Mode";
            this.colMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colMode.IntegralHeight = false;
            this.colMode.ItemHeight = 17;
            this.colMode.Name = "colMode";
            this.colMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colMode.Width = 80;
            // 
            // colStart
            // 
            // 
            // 
            // 
            this.colStart.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.colStart.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colStart.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colStart.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
            this.colStart.DisplayFormat = "0.0#####";
            this.colStart.HeaderText = "Start Value";
            this.colStart.Increment = 1D;
            this.colStart.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.colStart.MaxValue = 2000D;
            this.colStart.MinValue = -2000D;
            this.colStart.Name = "colStart";
            // 
            // colEnd
            // 
            // 
            // 
            // 
            this.colEnd.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colEnd.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colEnd.DisplayFormat = "0.0#####";
            this.colEnd.HeaderText = "End Value";
            this.colEnd.Increment = 1D;
            this.colEnd.MaxValue = 2000D;
            this.colEnd.MinValue = -2000D;
            this.colEnd.Name = "colEnd";
            // 
            // colCnt
            // 
            // 
            // 
            // 
            this.colCnt.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colCnt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colCnt.DisplayFormat = "0";
            this.colCnt.HeaderText = "Cnt";
            this.colCnt.Increment = 1D;
            this.colCnt.MaxValue = 40D;
            this.colCnt.MinValue = 1D;
            this.colCnt.Name = "colCnt";
            this.colCnt.Width = 40;
            // 
            // colFT
            // 
            // 
            // 
            // 
            this.colFT.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colFT.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colFT.DisplayFormat = "0";
            this.colFT.FillWeight = 90F;
            this.colFT.HeaderText = "Force Time";
            this.colFT.Increment = 1D;
            this.colFT.MaxValue = 2000D;
            this.colFT.MinValue = 1D;
            this.colFT.Name = "colFT";
            this.colFT.Width = 90;
            // 
            // colOT
            // 
            // 
            // 
            // 
            this.colOT.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colOT.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colOT.DisplayFormat = "0";
            this.colOT.FillWeight = 90F;
            this.colOT.HeaderText = "Off Time";
            this.colOT.Increment = 1D;
            this.colOT.MaxValue = 2000D;
            this.colOT.MinValue = 0D;
            this.colOT.Name = "colOT";
            this.colOT.Width = 90;
            // 
            // colNPLC
            // 
            // 
            // 
            // 
            this.colNPLC.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colNPLC.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colNPLC.DisplayFormat = "0.00";
            this.colNPLC.FillWeight = 50F;
            this.colNPLC.HeaderText = "NPLC";
            this.colNPLC.Increment = 0.01D;
            this.colNPLC.MaxValue = 25D;
            this.colNPLC.MinValue = 0D;
            this.colNPLC.Name = "colNPLC";
            this.colNPLC.Width = 50;
            // 
            // colAutoRange
            // 
            this.colAutoRange.FillWeight = 70F;
            this.colAutoRange.HeaderText = "Auto Range";
            this.colAutoRange.Name = "colAutoRange";
            this.colAutoRange.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAutoRange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colAutoRange.Width = 70;
            // 
            // colClamp
            // 
            // 
            // 
            // 
            this.colClamp.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colClamp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colClamp.DisplayFormat = "0.######";
            this.colClamp.HeaderText = "Clamp";
            this.colClamp.Increment = 1D;
            this.colClamp.MaxValue = 100000D;
            this.colClamp.MinValue = 1E-06D;
            this.colClamp.Name = "colClamp";
            // 
            // btnDelet
            // 
            this.btnDelet.Font = new System.Drawing.Font("PMingLiU", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnDelet.Location = new System.Drawing.Point(24, 104);
            this.btnDelet.Name = "btnDelet";
            this.btnDelet.Size = new System.Drawing.Size(90, 33);
            this.btnDelet.TabIndex = 1;
            this.btnDelet.Text = "Delet";
            this.btnDelet.UseVisualStyleBackColor = true;
            this.btnDelet.Click += new System.EventHandler(this.btnDelet_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("PMingLiU", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnAdd.Location = new System.Drawing.Point(24, 33);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 32);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // CustomizeListComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CustomizeListComponent";
            this.Size = new System.Drawing.Size(1048, 380);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.Button btnDelet;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colMode;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colStart;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colEnd;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colCnt;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colFT;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colOT;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colNPLC;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colAutoRange;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colClamp;
    }
}
