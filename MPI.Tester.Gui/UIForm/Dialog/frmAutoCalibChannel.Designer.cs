namespace MPI.Tester.Gui
{
    partial class frmAutoCalibChannel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAutoCalibChannel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblRowCol = new DevComponents.DotNetBar.LabelX();
            this.pnlDutChDisplay = new System.Windows.Forms.Panel();
            this.dgvByChannelCoefTable = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colByChannelCoefNO = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.colByChannelCoefName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colByChannelCoefChannel = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.colByChannelCoefType = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.colByChannelCoefSquare = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colByChannelCoefGain = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colByChannelCoefOffset = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colByChannelCoefKeyName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.Column3 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.Column1 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.Column2 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.btnCombine = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlDutChDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvByChannelCoefTable)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRowCol
            // 
            this.lblRowCol.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.lblRowCol.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblRowCol.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblRowCol, "lblRowCol");
            this.lblRowCol.Name = "lblRowCol";
            this.lblRowCol.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // pnlDutChDisplay
            // 
            resources.ApplyResources(this.pnlDutChDisplay, "pnlDutChDisplay");
            this.pnlDutChDisplay.BackColor = System.Drawing.Color.Transparent;
            this.pnlDutChDisplay.Controls.Add(this.dgvByChannelCoefTable);
            this.pnlDutChDisplay.Controls.Add(this.btnCombine);
            this.pnlDutChDisplay.Controls.Add(this.lblRowCol);
            this.pnlDutChDisplay.Name = "pnlDutChDisplay";
            // 
            // dgvByChannelCoefTable
            // 
            this.dgvByChannelCoefTable.AllowUserToAddRows = false;
            this.dgvByChannelCoefTable.AllowUserToDeleteRows = false;
            this.dgvByChannelCoefTable.AllowUserToResizeRows = false;
            this.dgvByChannelCoefTable.BackgroundColor = System.Drawing.Color.White;
            this.dgvByChannelCoefTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvByChannelCoefTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvByChannelCoefTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvByChannelCoefTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colByChannelCoefNO,
            this.colByChannelCoefName,
            this.colByChannelCoefChannel,
            this.colByChannelCoefType,
            this.colByChannelCoefSquare,
            this.colByChannelCoefGain,
            this.colByChannelCoefOffset,
            this.colByChannelCoefKeyName,
            this.Column3,
            this.Column1,
            this.Column2});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvByChannelCoefTable.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvByChannelCoefTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            resources.ApplyResources(this.dgvByChannelCoefTable, "dgvByChannelCoefTable");
            this.dgvByChannelCoefTable.MultiSelect = false;
            this.dgvByChannelCoefTable.Name = "dgvByChannelCoefTable";
            this.dgvByChannelCoefTable.RowHeadersVisible = false;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvByChannelCoefTable.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvByChannelCoefTable.RowTemplate.Height = 24;
            this.dgvByChannelCoefTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvByChannelCoefTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvByChannelCoefTable_CellContentClick);
            // 
            // colByChannelCoefNO
            // 
            this.colByChannelCoefNO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colByChannelCoefNO.FillWeight = 20F;
            resources.ApplyResources(this.colByChannelCoefNO, "colByChannelCoefNO");
            this.colByChannelCoefNO.Name = "colByChannelCoefNO";
            this.colByChannelCoefNO.ReadOnly = true;
            this.colByChannelCoefNO.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colByChannelCoefNO.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // colByChannelCoefName
            // 
            this.colByChannelCoefName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colByChannelCoefName.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.colByChannelCoefName, "colByChannelCoefName");
            this.colByChannelCoefName.Name = "colByChannelCoefName";
            this.colByChannelCoefName.ReadOnly = true;
            this.colByChannelCoefName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colByChannelCoefName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colByChannelCoefChannel
            // 
            this.colByChannelCoefChannel.FillWeight = 30F;
            resources.ApplyResources(this.colByChannelCoefChannel, "colByChannelCoefChannel");
            this.colByChannelCoefChannel.Name = "colByChannelCoefChannel";
            this.colByChannelCoefChannel.ReadOnly = true;
            // 
            // colByChannelCoefType
            // 
            this.colByChannelCoefType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colByChannelCoefType.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colByChannelCoefType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colByChannelCoefType.DefaultCellStyle = dataGridViewCellStyle3;
            this.colByChannelCoefType.DisplayFormat = "0";
            this.colByChannelCoefType.FillWeight = 50F;
            resources.ApplyResources(this.colByChannelCoefType, "colByChannelCoefType");
            this.colByChannelCoefType.MaxValue = 3;
            this.colByChannelCoefType.MinValue = 0;
            this.colByChannelCoefType.Name = "colByChannelCoefType";
            this.colByChannelCoefType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colByChannelCoefSquare
            // 
            this.colByChannelCoefSquare.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colByChannelCoefSquare.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colByChannelCoefSquare.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colByChannelCoefSquare.DefaultCellStyle = dataGridViewCellStyle4;
            this.colByChannelCoefSquare.DisplayFormat = "0.0000";
            this.colByChannelCoefSquare.FillWeight = 50F;
            resources.ApplyResources(this.colByChannelCoefSquare, "colByChannelCoefSquare");
            this.colByChannelCoefSquare.Increment = 1D;
            this.colByChannelCoefSquare.Name = "colByChannelCoefSquare";
            this.colByChannelCoefSquare.ReadOnly = true;
            this.colByChannelCoefSquare.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colByChannelCoefSquare.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colByChannelCoefGain
            // 
            this.colByChannelCoefGain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colByChannelCoefGain.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colByChannelCoefGain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colByChannelCoefGain.DefaultCellStyle = dataGridViewCellStyle5;
            this.colByChannelCoefGain.DisplayFormat = "0.0000";
            this.colByChannelCoefGain.FillWeight = 50F;
            resources.ApplyResources(this.colByChannelCoefGain, "colByChannelCoefGain");
            this.colByChannelCoefGain.Increment = 1D;
            this.colByChannelCoefGain.Name = "colByChannelCoefGain";
            this.colByChannelCoefGain.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colByChannelCoefGain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colByChannelCoefOffset
            // 
            this.colByChannelCoefOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colByChannelCoefOffset.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colByChannelCoefOffset.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colByChannelCoefOffset.DefaultCellStyle = dataGridViewCellStyle6;
            this.colByChannelCoefOffset.DisplayFormat = "0.0000";
            this.colByChannelCoefOffset.FillWeight = 50F;
            resources.ApplyResources(this.colByChannelCoefOffset, "colByChannelCoefOffset");
            this.colByChannelCoefOffset.Increment = 1D;
            this.colByChannelCoefOffset.Name = "colByChannelCoefOffset";
            this.colByChannelCoefOffset.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colByChannelCoefOffset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colByChannelCoefKeyName
            // 
            resources.ApplyResources(this.colByChannelCoefKeyName, "colByChannelCoefKeyName");
            this.colByChannelCoefKeyName.Name = "colByChannelCoefKeyName";
            this.colByChannelCoefKeyName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column3
            // 
            // 
            // 
            // 
            this.Column3.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column3.DisplayFormat = "0.000";
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Increment = 1D;
            this.Column3.Name = "Column3";
            // 
            // Column1
            // 
            // 
            // 
            // 
            this.Column1.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column1.DisplayFormat = "0.0000";
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Increment = 1D;
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            // 
            // 
            // 
            this.Column2.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Increment = 1D;
            this.Column2.Name = "Column2";
            // 
            // btnCombine
            // 
            this.btnCombine.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCombine.AntiAlias = true;
            this.btnCombine.BackColor = System.Drawing.Color.Transparent;
            this.btnCombine.EnableMarkup = false;
            this.btnCombine.FocusCuesEnabled = false;
            resources.ApplyResources(this.btnCombine, "btnCombine");
            this.btnCombine.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCombine.Image = global::MPI.Tester.Gui.Properties.Resources.btnUpdateItem;
            this.btnCombine.ImageFixedSize = new System.Drawing.Size(22, 22);
            this.btnCombine.Name = "btnCombine";
            this.btnCombine.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(3);
            this.btnCombine.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCombine.Click += new System.EventHandler(this.btnCombine_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.AntiAlias = true;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.EnableMarkup = false;
            this.btnCancel.FocusCuesEnabled = false;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Image = global::MPI.Tester.Gui.Properties.Resources.btnDataCancel;
            this.btnCancel.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(3);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfirm.AntiAlias = true;
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.EnableMarkup = false;
            this.btnConfirm.FocusCuesEnabled = false;
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnConfirm.Image = global::MPI.Tester.Gui.Properties.Resources.btnDataOK;
            this.btnConfirm.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(3);
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // frmAutoCalibChannel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pnlDutChDisplay);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAutoCalibChannel";
            this.Load += new System.EventHandler(this.frmAutoCalibChannel_Load);
            this.pnlDutChDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvByChannelCoefTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lblRowCol;
        private System.Windows.Forms.Panel pnlDutChDisplay;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvByChannelCoefTable;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.ButtonX btnCombine;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn colByChannelCoefNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colByChannelCoefName;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn colByChannelCoefChannel;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn colByChannelCoefType;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colByChannelCoefSquare;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colByChannelCoefGain;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colByChannelCoefOffset;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn colByChannelCoefKeyName;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn Column3;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn Column1;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn Column2;
    }
}