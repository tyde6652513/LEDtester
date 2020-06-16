namespace MPI.Tester.Gui
{
    partial class frmSetESDSysParam
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnReload = new DevComponents.DotNetBar.ButtonX();
            this.btnRestESDGain = new DevComponents.DotNetBar.ButtonX();
            this.gplParameter = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dgvESDMM = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewIntegerInputColumn7 = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.dataGridViewIntegerInputColumn8 = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.dataGridViewIntegerInputColumn9 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.dgvESDHBM = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colHBMIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHBMLowerRange = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.colHBMUpperRange = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.colHBMGain = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.gplParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvESDMM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvESDHBM)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Image = global::MPI.Tester.Gui.Properties.Resources.btnSaveFile_B;
            this.btnSave.Location = new System.Drawing.Point(17, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnSave.Size = new System.Drawing.Size(120, 50);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSave.TabIndex = 220;
            this.btnSave.Tag = "40";
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReload
            // 
            this.btnReload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReload.BackColor = System.Drawing.Color.Transparent;
            this.btnReload.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnReload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReload.Image = global::MPI.Tester.Gui.Properties.Resources.btnUndo;
            this.btnReload.Location = new System.Drawing.Point(154, 12);
            this.btnReload.Name = "btnReload";
            this.btnReload.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnReload.Size = new System.Drawing.Size(120, 50);
            this.btnReload.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnReload.TabIndex = 221;
            this.btnReload.Tag = "40";
            this.btnReload.Text = "Reload";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnRestESDGain
            // 
            this.btnRestESDGain.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRestESDGain.AntiAlias = true;
            this.btnRestESDGain.BackColor = System.Drawing.Color.Transparent;
            this.btnRestESDGain.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnRestESDGain.Location = new System.Drawing.Point(321, 3);
            this.btnRestESDGain.Name = "btnRestESDGain";
            this.btnRestESDGain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRestESDGain.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnRestESDGain.Size = new System.Drawing.Size(142, 39);
            this.btnRestESDGain.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnRestESDGain.TabIndex = 222;
            this.btnRestESDGain.Text = "Reset ESD Gain";
            this.btnRestESDGain.Click += new System.EventHandler(this.btnRestESDGain_Click);
            // 
            // gplParameter
            // 
            this.gplParameter.CanvasColor = System.Drawing.SystemColors.ActiveCaption;
            this.gplParameter.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.gplParameter.Controls.Add(this.dgvESDMM);
            this.gplParameter.Controls.Add(this.labelX4);
            this.gplParameter.Controls.Add(this.labelX3);
            this.gplParameter.Controls.Add(this.dgvESDHBM);
            this.gplParameter.Controls.Add(this.btnRestESDGain);
            this.gplParameter.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.gplParameter.Location = new System.Drawing.Point(17, 83);
            this.gplParameter.Name = "gplParameter";
            this.gplParameter.Size = new System.Drawing.Size(492, 713);
            // 
            // 
            // 
            this.gplParameter.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gplParameter.Style.BackColor2 = System.Drawing.Color.Lavender;
            this.gplParameter.Style.BackColorGradientAngle = 90;
            this.gplParameter.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplParameter.Style.BorderBottomWidth = 1;
            this.gplParameter.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gplParameter.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplParameter.Style.BorderLeftWidth = 1;
            this.gplParameter.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplParameter.Style.BorderRightWidth = 1;
            this.gplParameter.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplParameter.Style.BorderTopWidth = 1;
            this.gplParameter.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplParameter.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gplParameter.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gplParameter.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gplParameter.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gplParameter.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplParameter.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gplParameter.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplParameter.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gplParameter.TabIndex = 223;
            // 
            // dgvESDMM
            // 
            this.dgvESDMM.AllowUserToAddRows = false;
            this.dgvESDMM.AllowUserToDeleteRows = false;
            this.dgvESDMM.AllowUserToResizeColumns = false;
            this.dgvESDMM.AllowUserToResizeRows = false;
            this.dgvESDMM.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvESDMM.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvESDMM.ColumnHeadersHeight = 30;
            this.dgvESDMM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvESDMM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewIntegerInputColumn7,
            this.dataGridViewIntegerInputColumn8,
            this.dataGridViewIntegerInputColumn9});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvESDMM.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvESDMM.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvESDMM.Location = new System.Drawing.Point(22, 434);
            this.dgvESDMM.MultiSelect = false;
            this.dgvESDMM.Name = "dgvESDMM";
            this.dgvESDMM.RowHeadersVisible = false;
            this.dgvESDMM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvESDMM.RowTemplate.Height = 24;
            this.dgvESDMM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvESDMM.Size = new System.Drawing.Size(441, 255);
            this.dgvESDMM.TabIndex = 226;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn5.HeaderText = "No";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Width = 50;
            // 
            // dataGridViewIntegerInputColumn7
            // 
            // 
            // 
            // 
            this.dataGridViewIntegerInputColumn7.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dataGridViewIntegerInputColumn7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewIntegerInputColumn7.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewIntegerInputColumn7.HeaderText = "Lower Boundary";
            this.dataGridViewIntegerInputColumn7.MaxValue = 9000;
            this.dataGridViewIntegerInputColumn7.MinValue = 0;
            this.dataGridViewIntegerInputColumn7.Name = "dataGridViewIntegerInputColumn7";
            this.dataGridViewIntegerInputColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewIntegerInputColumn7.Width = 145;
            // 
            // dataGridViewIntegerInputColumn8
            // 
            // 
            // 
            // 
            this.dataGridViewIntegerInputColumn8.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dataGridViewIntegerInputColumn8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewIntegerInputColumn8.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewIntegerInputColumn8.HeaderText = "Upper Boundary";
            this.dataGridViewIntegerInputColumn8.MaxValue = 9000;
            this.dataGridViewIntegerInputColumn8.MinValue = 0;
            this.dataGridViewIntegerInputColumn8.Name = "dataGridViewIntegerInputColumn8";
            this.dataGridViewIntegerInputColumn8.ReadOnly = true;
            this.dataGridViewIntegerInputColumn8.Width = 145;
            // 
            // dataGridViewIntegerInputColumn9
            // 
            // 
            // 
            // 
            this.dataGridViewIntegerInputColumn9.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dataGridViewIntegerInputColumn9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewIntegerInputColumn9.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewIntegerInputColumn9.HeaderText = "Gain";
            this.dataGridViewIntegerInputColumn9.Increment = 1D;
            this.dataGridViewIntegerInputColumn9.MaxValue = 2D;
            this.dataGridViewIntegerInputColumn9.MinValue = 0D;
            this.dataGridViewIntegerInputColumn9.Name = "dataGridViewIntegerInputColumn9";
            this.dataGridViewIntegerInputColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIntegerInputColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewIntegerInputColumn9.Width = 80;
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX4.Location = new System.Drawing.Point(22, 403);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(146, 25);
            this.labelX4.TabIndex = 225;
            this.labelX4.Text = "MM System Gain";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX3.Location = new System.Drawing.Point(22, 47);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(146, 25);
            this.labelX3.TabIndex = 224;
            this.labelX3.Text = "HBM System Gain";
            // 
            // dgvESDHBM
            // 
            this.dgvESDHBM.AllowUserToAddRows = false;
            this.dgvESDHBM.AllowUserToDeleteRows = false;
            this.dgvESDHBM.AllowUserToResizeColumns = false;
            this.dgvESDHBM.AllowUserToResizeRows = false;
            this.dgvESDHBM.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvESDHBM.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvESDHBM.ColumnHeadersHeight = 30;
            this.dgvESDHBM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvESDHBM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHBMIndex,
            this.colHBMLowerRange,
            this.colHBMUpperRange,
            this.colHBMGain});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvESDHBM.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvESDHBM.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvESDHBM.Location = new System.Drawing.Point(22, 78);
            this.dgvESDHBM.MultiSelect = false;
            this.dgvESDHBM.Name = "dgvESDHBM";
            this.dgvESDHBM.RowHeadersVisible = false;
            this.dgvESDHBM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvESDHBM.RowTemplate.Height = 24;
            this.dgvESDHBM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvESDHBM.Size = new System.Drawing.Size(441, 300);
            this.dgvESDHBM.TabIndex = 223;
            // 
            // colHBMIndex
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colHBMIndex.DefaultCellStyle = dataGridViewCellStyle8;
            this.colHBMIndex.HeaderText = "No";
            this.colHBMIndex.Name = "colHBMIndex";
            this.colHBMIndex.ReadOnly = true;
            this.colHBMIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colHBMIndex.Width = 50;
            // 
            // colHBMLowerRange
            // 
            // 
            // 
            // 
            this.colHBMLowerRange.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colHBMLowerRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colHBMLowerRange.DefaultCellStyle = dataGridViewCellStyle9;
            this.colHBMLowerRange.HeaderText = "Lower Boundary";
            this.colHBMLowerRange.MaxValue = 9000;
            this.colHBMLowerRange.MinValue = 0;
            this.colHBMLowerRange.Name = "colHBMLowerRange";
            this.colHBMLowerRange.ReadOnly = true;
            this.colHBMLowerRange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colHBMLowerRange.Width = 145;
            // 
            // colHBMUpperRange
            // 
            // 
            // 
            // 
            this.colHBMUpperRange.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colHBMUpperRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colHBMUpperRange.DefaultCellStyle = dataGridViewCellStyle10;
            this.colHBMUpperRange.HeaderText = "Upper Boundary";
            this.colHBMUpperRange.MaxValue = 9000;
            this.colHBMUpperRange.MinValue = 0;
            this.colHBMUpperRange.Name = "colHBMUpperRange";
            this.colHBMUpperRange.ReadOnly = true;
            this.colHBMUpperRange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colHBMUpperRange.Width = 145;
            // 
            // colHBMGain
            // 
            // 
            // 
            // 
            this.colHBMGain.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colHBMGain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colHBMGain.DefaultCellStyle = dataGridViewCellStyle11;
            this.colHBMGain.HeaderText = "Gain";
            this.colHBMGain.Increment = 1D;
            this.colHBMGain.MaxValue = 2D;
            this.colHBMGain.MinValue = 0D;
            this.colHBMGain.Name = "colHBMGain";
            this.colHBMGain.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colHBMGain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colHBMGain.Width = 80;
            // 
            // frmSetESDSysParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.ClientSize = new System.Drawing.Size(526, 803);
            this.Controls.Add(this.gplParameter);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetESDSysParam";
            this.Text = "ESD System Parameter";
            this.gplParameter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvESDMM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvESDHBM)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnReload;
        private DevComponents.DotNetBar.ButtonX btnRestESDGain;
        private DevComponents.DotNetBar.Controls.GroupPanel gplParameter;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvESDHBM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHBMIndex;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn colHBMLowerRange;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn colHBMUpperRange;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colHBMGain;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvESDMM;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn dataGridViewIntegerInputColumn7;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn dataGridViewIntegerInputColumn8;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn dataGridViewIntegerInputColumn9;
    }
}