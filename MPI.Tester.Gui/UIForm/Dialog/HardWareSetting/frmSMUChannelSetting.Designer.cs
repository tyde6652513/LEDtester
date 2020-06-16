namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    partial class frmSMUChannelSetting 
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pncColRow = new System.Windows.Forms.Panel();
            this.intRow = new DevComponents.Editors.IntegerInput();
            this.intCol = new DevComponents.Editors.IntegerInput();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.pnlMode = new System.Windows.Forms.Panel();
            this.cmbSequential = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.pnlCh = new System.Windows.Forms.Panel();
            this.dgvChannelConfigTable = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colDutCh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colDevSrcCh = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colIP = new DevComponents.DotNetBar.Controls.DataGridViewIpAddressInputColumn();
            this.colComPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pncColRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intCol)).BeginInit();
            this.pnlMode.SuspendLayout();
            this.pnlCh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannelConfigTable)).BeginInit();
            this.SuspendLayout();
            // 
            // pncColRow
            // 
            this.pncColRow.Controls.Add(this.intRow);
            this.pncColRow.Controls.Add(this.intCol);
            this.pncColRow.Controls.Add(this.labelX2);
            this.pncColRow.Controls.Add(this.labelX1);
            this.pncColRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pncColRow.Location = new System.Drawing.Point(0, 0);
            this.pncColRow.Name = "pncColRow";
            this.pncColRow.Size = new System.Drawing.Size(375, 33);
            this.pncColRow.TabIndex = 0;
            // 
            // intRow
            // 
            // 
            // 
            // 
            this.intRow.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intRow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intRow.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intRow.Location = new System.Drawing.Point(150, 3);
            this.intRow.MaxValue = 64;
            this.intRow.MinValue = 1;
            this.intRow.Name = "intRow";
            this.intRow.ShowUpDown = true;
            this.intRow.Size = new System.Drawing.Size(80, 22);
            this.intRow.TabIndex = 10;
            this.intRow.Value = 1;
            this.intRow.ValueChanged += new System.EventHandler(this.intRow_ValueChanged);

            // 
            // intCol
            // 
            // 
            // 
            // 
            this.intCol.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intCol.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intCol.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intCol.Location = new System.Drawing.Point(30, 3);
            this.intCol.MaxValue = 64;
            this.intCol.MinValue = 1;
            this.intCol.Name = "intCol";
            this.intCol.ShowUpDown = true;
            this.intCol.Size = new System.Drawing.Size(80, 22);
            this.intCol.TabIndex = 8;
            this.intCol.Value = 1;
            this.intCol.ValueChanged += new System.EventHandler(this.intCol_ValueChanged);

            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX2.Location = new System.Drawing.Point(119, 3);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 9;
            this.labelX2.Text = "Row";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX1.Location = new System.Drawing.Point(5, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "Col";
            // 
            // pnlMode
            // 
            this.pnlMode.Controls.Add(this.cmbSequential);
            this.pnlMode.Controls.Add(this.labelX3);
            this.pnlMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMode.Location = new System.Drawing.Point(0, 33);
            this.pnlMode.Name = "pnlMode";
            this.pnlMode.Size = new System.Drawing.Size(375, 38);
            this.pnlMode.TabIndex = 2;
            // 
            // cmbSequential
            // 
            this.cmbSequential.DisplayMember = "Text";
            this.cmbSequential.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSequential.FormattingEnabled = true;
            this.cmbSequential.ItemHeight = 16;
            this.cmbSequential.Location = new System.Drawing.Point(69, 5);
            this.cmbSequential.Name = "cmbSequential";
            this.cmbSequential.Size = new System.Drawing.Size(88, 22);
            this.cmbSequential.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSequential.TabIndex = 8;
            this.cmbSequential.SelectedIndexChanged += new System.EventHandler(this.cmbSequential_SelectedIndexChanged);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX3.Location = new System.Drawing.Point(5, 5);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(58, 23);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "Sequential";
            // 
            // pnlCh
            // 
            this.pnlCh.Controls.Add(this.dgvChannelConfigTable);
            this.pnlCh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCh.Location = new System.Drawing.Point(0, 71);
            this.pnlCh.Name = "pnlCh";
            this.pnlCh.Size = new System.Drawing.Size(375, 397);
            this.pnlCh.TabIndex = 3;
            // 
            // dgvChannelConfigTable
            // 
            this.dgvChannelConfigTable.AllowUserToAddRows = false;
            this.dgvChannelConfigTable.AllowUserToDeleteRows = false;
            this.dgvChannelConfigTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChannelConfigTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDutCh,
            this.colModel,
            this.colDevSrcCh,
            this.colIP,
            this.colComPort});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChannelConfigTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvChannelConfigTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChannelConfigTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvChannelConfigTable.Location = new System.Drawing.Point(0, 0);
            this.dgvChannelConfigTable.Name = "dgvChannelConfigTable";
            this.dgvChannelConfigTable.RowHeadersVisible = false;
            this.dgvChannelConfigTable.RowTemplate.Height = 24;
            this.dgvChannelConfigTable.Size = new System.Drawing.Size(375, 397);
            this.dgvChannelConfigTable.TabIndex = 0;
            // 
            // colDutCh
            // 
            this.colDutCh.FillWeight = 50F;
            this.colDutCh.HeaderText = "CH";
            this.colDutCh.Name = "colDutCh";
            this.colDutCh.Width = 50;
            // 
            // colModel
            // 
            this.colModel.FillWeight = 80F;
            this.colModel.HeaderText = "Model";
            this.colModel.Name = "colModel";
            this.colModel.Width = 80;
            // 
            // colDevSrcCh
            // 
            this.colDevSrcCh.FillWeight = 60F;
            this.colDevSrcCh.HeaderText = "SrcCh";
            this.colDevSrcCh.Name = "colDevSrcCh";
            this.colDevSrcCh.Width = 60;
            // 
            // colIP
            // 
            this.colIP.AutoOverwrite = true;
            this.colIP.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.colIP.BackgroundStyle.Class = "DataGridViewIpAddressBorder";
            this.colIP.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colIP.FillWeight = 150F;
            this.colIP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.colIP.HeaderText = "IP";
            this.colIP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colIP.Name = "colIP";
            this.colIP.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colIP.Text = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.colIP.Width = 150;
            // 
            // colComPort
            // 
            this.colComPort.FillWeight = 80F;
            this.colComPort.HeaderText = "ComPort";
            this.colComPort.Name = "colComPort";
            this.colComPort.Width = 80;
            // 
            // frmSMUChannelSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(375, 468);
            this.Controls.Add(this.pnlCh);
            this.Controls.Add(this.pnlMode);
            this.Controls.Add(this.pncColRow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSMUChannelSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmSMUChannelSetting";
            this.pncColRow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.intRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intCol)).EndInit();
            this.pnlMode.ResumeLayout(false);
            this.pnlCh.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannelConfigTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pncColRow;
        private DevComponents.Editors.IntegerInput intRow;
        private DevComponents.Editors.IntegerInput intCol;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.Panel pnlMode;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSequential;
        private DevComponents.DotNetBar.LabelX labelX3;
        private System.Windows.Forms.Panel pnlCh;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvChannelConfigTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDutCh;
        private System.Windows.Forms.DataGridViewComboBoxColumn colModel;
        private System.Windows.Forms.DataGridViewComboBoxColumn colDevSrcCh;
        private DevComponents.DotNetBar.Controls.DataGridViewIpAddressInputColumn colIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComPort;




    }
}