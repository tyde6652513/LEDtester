namespace MPI.Tester.Gui
{
    partial class frmDailyWatch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.plSptXaxisCoeff = new System.Windows.Forms.Panel();
            this.dgvSptFactor = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colGainOffsetNo = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.colSquare = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colGain = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colOffset = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.lblThirdCoefficient = new System.Windows.Forms.TextBox();
            this.txtYCalibFilePath = new System.Windows.Forms.TextBox();
            this.lblOthersSetting = new DevComponents.DotNetBar.LabelX();
            this.lblSecondCoefficient = new System.Windows.Forms.TextBox();
            this.lblFirstCoefficient = new System.Windows.Forms.TextBox();
            this.lblIntecept = new System.Windows.Forms.TextBox();
            this.labelX28 = new DevComponents.DotNetBar.LabelX();
            this.labelX27 = new DevComponents.DotNetBar.LabelX();
            this.labelX25 = new DevComponents.DotNetBar.LabelX();
            this.labelX26 = new DevComponents.DotNetBar.LabelX();
            this.superTabStrip1 = new DevComponents.DotNetBar.SuperTabStrip();
            this.btnLoadStdFile = new DevComponents.DotNetBar.ButtonItem();
            this.btnGetDark = new DevComponents.DotNetBar.ButtonItem();
            this.btnLoadMsrtFile = new DevComponents.DotNetBar.ButtonItem();
            this.btnUnLack = new DevComponents.DotNetBar.ButtonItem();
            this.btnSaveCoeffToSystem = new DevComponents.DotNetBar.ButtonItem();
            this.btnReturn = new DevComponents.DotNetBar.ButtonItem();
            this.tabSptCalibration = new DevComponents.DotNetBar.SuperTabControl();
            this.tabPlCalibTools = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.lblMaxCount = new System.Windows.Forms.Label();
            this.lblMaxCountLabel = new DevComponents.DotNetBar.LabelX();
            this.lblIntegratingTime = new DevComponents.DotNetBar.LabelX();
            this.dinChipIndex = new DevComponents.Editors.DoubleInput();
            this.zedDailyWatch = new ZedGraph.ZedGraphControl();
            this.tabItemCalibTool = new DevComponents.DotNetBar.SuperTabItem();
            this.tabPlCalibDisplay = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.tabItemData = new DevComponents.DotNetBar.SuperTabItem();
            this.plSptXaxisCoeff.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSptFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabSptCalibration)).BeginInit();
            this.tabSptCalibration.SuspendLayout();
            this.tabPlCalibTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinChipIndex)).BeginInit();
            this.tabPlCalibDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.AntiAlias = true;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.EnableMarkup = false;
            this.btnClose.FocusCuesEnabled = false;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClose.Location = new System.Drawing.Point(641, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnAddBinItem_Click);
            // 
            // plSptXaxisCoeff
            // 
            this.plSptXaxisCoeff.BackColor = System.Drawing.Color.Transparent;
            this.plSptXaxisCoeff.Controls.Add(this.dgvSptFactor);
            this.plSptXaxisCoeff.Controls.Add(this.btnClose);
            this.plSptXaxisCoeff.Controls.Add(this.lblThirdCoefficient);
            this.plSptXaxisCoeff.Controls.Add(this.txtYCalibFilePath);
            this.plSptXaxisCoeff.Controls.Add(this.lblOthersSetting);
            this.plSptXaxisCoeff.Controls.Add(this.lblSecondCoefficient);
            this.plSptXaxisCoeff.Controls.Add(this.lblFirstCoefficient);
            this.plSptXaxisCoeff.Controls.Add(this.lblIntecept);
            this.plSptXaxisCoeff.Controls.Add(this.labelX28);
            this.plSptXaxisCoeff.Controls.Add(this.labelX27);
            this.plSptXaxisCoeff.Controls.Add(this.labelX25);
            this.plSptXaxisCoeff.Controls.Add(this.labelX26);
            this.plSptXaxisCoeff.Location = new System.Drawing.Point(3, 3);
            this.plSptXaxisCoeff.Name = "plSptXaxisCoeff";
            this.plSptXaxisCoeff.Size = new System.Drawing.Size(838, 512);
            this.plSptXaxisCoeff.TabIndex = 382;
            // 
            // dgvSptFactor
            // 
            this.dgvSptFactor.AllowUserToAddRows = false;
            this.dgvSptFactor.AllowUserToDeleteRows = false;
            this.dgvSptFactor.AllowUserToResizeRows = false;
            this.dgvSptFactor.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSptFactor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSptFactor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSptFactor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGainOffsetNo,
            this.colSquare,
            this.colGain,
            this.colOffset});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSptFactor.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvSptFactor.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvSptFactor.Location = new System.Drawing.Point(395, 110);
            this.dgvSptFactor.MultiSelect = false;
            this.dgvSptFactor.Name = "dgvSptFactor";
            this.dgvSptFactor.RowHeadersVisible = false;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSptFactor.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvSptFactor.RowTemplate.Height = 24;
            this.dgvSptFactor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvSptFactor.Size = new System.Drawing.Size(368, 373);
            this.dgvSptFactor.TabIndex = 392;
            // 
            // colGainOffsetNo
            // 
            this.colGainOffsetNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colGainOffsetNo.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colGainOffsetNo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colGainOffsetNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.colGainOffsetNo.FillWeight = 20F;
            this.colGainOffsetNo.HeaderText = "Pixel";
            this.colGainOffsetNo.Name = "colGainOffsetNo";
            this.colGainOffsetNo.ReadOnly = true;
            this.colGainOffsetNo.Width = 50;
            // 
            // colSquare
            // 
            this.colSquare.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colSquare.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colSquare.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSquare.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSquare.DisplayFormat = "0.00";
            this.colSquare.FillWeight = 50F;
            this.colSquare.HeaderText = "Wave";
            this.colSquare.Increment = 1D;
            this.colSquare.Name = "colSquare";
            this.colSquare.ReadOnly = true;
            this.colSquare.Width = 90;
            // 
            // colGain
            // 
            this.colGain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colGain.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colGain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGain.DefaultCellStyle = dataGridViewCellStyle4;
            this.colGain.DisplayFormat = "0.0000000";
            this.colGain.FillWeight = 50F;
            this.colGain.HeaderText = "IntCoeff";
            this.colGain.Increment = 1D;
            this.colGain.Name = "colGain";
            this.colGain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGain.Width = 90;
            // 
            // colOffset
            // 
            this.colOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colOffset.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colOffset.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colOffset.DefaultCellStyle = dataGridViewCellStyle5;
            this.colOffset.DisplayFormat = "0.000000";
            this.colOffset.FillWeight = 50F;
            this.colOffset.HeaderText = "WeightCoeff";
            this.colOffset.Increment = 1D;
            this.colOffset.Name = "colOffset";
            this.colOffset.Width = 90;
            // 
            // lblThirdCoefficient
            // 
            this.lblThirdCoefficient.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lblThirdCoefficient.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblThirdCoefficient.Location = new System.Drawing.Point(191, 195);
            this.lblThirdCoefficient.Name = "lblThirdCoefficient";
            this.lblThirdCoefficient.ReadOnly = true;
            this.lblThirdCoefficient.Size = new System.Drawing.Size(175, 24);
            this.lblThirdCoefficient.TabIndex = 389;
            // 
            // txtYCalibFilePath
            // 
            this.txtYCalibFilePath.BackColor = System.Drawing.Color.LightYellow;
            this.txtYCalibFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtYCalibFilePath.Location = new System.Drawing.Point(203, 61);
            this.txtYCalibFilePath.Name = "txtYCalibFilePath";
            this.txtYCalibFilePath.ReadOnly = true;
            this.txtYCalibFilePath.Size = new System.Drawing.Size(560, 24);
            this.txtYCalibFilePath.TabIndex = 391;
            // 
            // lblOthersSetting
            // 
            this.lblOthersSetting.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblOthersSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.lblOthersSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblOthersSetting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblOthersSetting.ForeColor = System.Drawing.Color.White;
            this.lblOthersSetting.Location = new System.Drawing.Point(18, 61);
            this.lblOthersSetting.Name = "lblOthersSetting";
            this.lblOthersSetting.Size = new System.Drawing.Size(179, 23);
            this.lblOthersSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblOthersSetting.TabIndex = 308;
            this.lblOthersSetting.Text = "Spt Calib. Data (.spt)";
            this.lblOthersSetting.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblSecondCoefficient
            // 
            this.lblSecondCoefficient.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lblSecondCoefficient.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblSecondCoefficient.Location = new System.Drawing.Point(191, 168);
            this.lblSecondCoefficient.Name = "lblSecondCoefficient";
            this.lblSecondCoefficient.ReadOnly = true;
            this.lblSecondCoefficient.Size = new System.Drawing.Size(175, 24);
            this.lblSecondCoefficient.TabIndex = 388;
            // 
            // lblFirstCoefficient
            // 
            this.lblFirstCoefficient.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lblFirstCoefficient.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblFirstCoefficient.Location = new System.Drawing.Point(191, 138);
            this.lblFirstCoefficient.Name = "lblFirstCoefficient";
            this.lblFirstCoefficient.ReadOnly = true;
            this.lblFirstCoefficient.Size = new System.Drawing.Size(175, 24);
            this.lblFirstCoefficient.TabIndex = 387;
            // 
            // lblIntecept
            // 
            this.lblIntecept.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lblIntecept.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblIntecept.Location = new System.Drawing.Point(191, 110);
            this.lblIntecept.Name = "lblIntecept";
            this.lblIntecept.ReadOnly = true;
            this.lblIntecept.Size = new System.Drawing.Size(175, 24);
            this.lblIntecept.TabIndex = 386;
            // 
            // labelX28
            // 
            this.labelX28.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX28.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX28.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX28.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX28.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelX28.Location = new System.Drawing.Point(18, 196);
            this.labelX28.Name = "labelX28";
            this.labelX28.Size = new System.Drawing.Size(167, 22);
            this.labelX28.TabIndex = 385;
            this.labelX28.Text = "Third Coefficient";
            // 
            // labelX27
            // 
            this.labelX27.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX27.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX27.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX27.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX27.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelX27.Location = new System.Drawing.Point(18, 168);
            this.labelX27.Name = "labelX27";
            this.labelX27.Size = new System.Drawing.Size(167, 22);
            this.labelX27.TabIndex = 384;
            this.labelX27.Text = "Second Coefficient";
            // 
            // labelX25
            // 
            this.labelX25.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX25.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX25.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX25.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX25.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelX25.Location = new System.Drawing.Point(18, 110);
            this.labelX25.Name = "labelX25";
            this.labelX25.Size = new System.Drawing.Size(167, 22);
            this.labelX25.TabIndex = 382;
            this.labelX25.Text = "Intercept";
            // 
            // labelX26
            // 
            this.labelX26.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX26.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX26.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX26.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX26.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelX26.Location = new System.Drawing.Point(18, 138);
            this.labelX26.Name = "labelX26";
            this.labelX26.Size = new System.Drawing.Size(167, 22);
            this.labelX26.TabIndex = 383;
            this.labelX26.Text = "First Coefficient";
            // 
            // superTabStrip1
            // 
            this.superTabStrip1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.superTabStrip1.AutoSelectAttachedControl = false;
            this.superTabStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            // 
            // 
            // 
            this.superTabStrip1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.superTabStrip1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.superTabStrip1.ContainerControlProcessDialogKey = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabStrip1.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            // 
            // 
            // 
            this.superTabStrip1.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.superTabStrip1.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.superTabStrip1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabStrip1.ControlBox.MenuBox,
            this.superTabStrip1.ControlBox.CloseBox});
            this.superTabStrip1.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold);
            this.superTabStrip1.Location = new System.Drawing.Point(3, 3);
            this.superTabStrip1.Name = "superTabStrip1";
            this.superTabStrip1.ReorderTabsEnabled = true;
            this.superTabStrip1.SelectedTabFont = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold);
            this.superTabStrip1.SelectedTabIndex = -1;
            this.superTabStrip1.Size = new System.Drawing.Size(102, 553);
            this.superTabStrip1.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Left;
            this.superTabStrip1.TabCloseButtonHot = null;
            this.superTabStrip1.TabFont = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold);
            this.superTabStrip1.TabIndex = 393;
            this.superTabStrip1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnLoadStdFile,
            this.btnGetDark,
            this.btnLoadMsrtFile,
            this.btnUnLack,
            this.btnSaveCoeffToSystem,
            this.btnReturn});
            this.superTabStrip1.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.Office2010BackstageBlue;
            this.superTabStrip1.Text = "superTabStrip1";
            // 
            // btnLoadStdFile
            // 
            this.btnLoadStdFile.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnLoadStdFile.FixedSize = new System.Drawing.Size(100, 70);
            this.btnLoadStdFile.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestItem_32;
            this.btnLoadStdFile.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.btnLoadStdFile.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnLoadStdFile.Name = "btnLoadStdFile";
            this.btnLoadStdFile.Text = "Load Std File";
            // 
            // btnGetDark
            // 
            this.btnGetDark.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnGetDark.FixedSize = new System.Drawing.Size(100, 70);
            this.btnGetDark.Image = global::MPI.Tester.Gui.Properties.Resources.btnSelectSource;
            this.btnGetDark.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.btnGetDark.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnGetDark.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.btnGetDark.Name = "btnGetDark";
            this.btnGetDark.Text = "Dark Noise";
            // 
            // btnLoadMsrtFile
            // 
            this.btnLoadMsrtFile.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnLoadMsrtFile.FixedSize = new System.Drawing.Size(100, 70);
            this.btnLoadMsrtFile.Image = global::MPI.Tester.Gui.Properties.Resources.btnOperation_32;
            this.btnLoadMsrtFile.ImageFixedSize = new System.Drawing.Size(26, 26);
            this.btnLoadMsrtFile.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnLoadMsrtFile.Name = "btnLoadMsrtFile";
            this.btnLoadMsrtFile.Text = "Get  Spectrum";
            // 
            // btnUnLack
            // 
            this.btnUnLack.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnUnLack.FixedSize = new System.Drawing.Size(100, 70);
            this.btnUnLack.Image = global::MPI.Tester.Gui.Properties.Resources.btnUserManager_32;
            this.btnUnLack.ImageFixedSize = new System.Drawing.Size(26, 26);
            this.btnUnLack.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnUnLack.Name = "btnUnLack";
            this.btnUnLack.Text = "Un Lack";
            // 
            // btnSaveCoeffToSystem
            // 
            this.btnSaveCoeffToSystem.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnSaveCoeffToSystem.FixedSize = new System.Drawing.Size(100, 70);
            this.btnSaveCoeffToSystem.Image = global::MPI.Tester.Gui.Properties.Resources.btnDataOK;
            this.btnSaveCoeffToSystem.ImageFixedSize = new System.Drawing.Size(26, 26);
            this.btnSaveCoeffToSystem.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnSaveCoeffToSystem.Name = "btnSaveCoeffToSystem";
            this.btnSaveCoeffToSystem.SubItemsExpandWidth = 10;
            this.btnSaveCoeffToSystem.Text = "Save Calib. Coeff";
            // 
            // btnReturn
            // 
            this.btnReturn.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnReturn.FixedSize = new System.Drawing.Size(100, 50);
            this.btnReturn.Image = global::MPI.Tester.Gui.Properties.Resources.btnDataCancel;
            this.btnReturn.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnReturn.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Text = "Close";
            // 
            // tabSptCalibration
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tabSptCalibration.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            // 
            // 
            // 
            this.tabSptCalibration.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.tabSptCalibration.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.tabSptCalibration.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabSptCalibration.ControlBox.MenuBox,
            this.tabSptCalibration.ControlBox.CloseBox});
            this.tabSptCalibration.Controls.Add(this.tabPlCalibDisplay);
            this.tabSptCalibration.Controls.Add(this.tabPlCalibTools);
            this.tabSptCalibration.Location = new System.Drawing.Point(-1, 1);
            this.tabSptCalibration.Name = "tabSptCalibration";
            this.tabSptCalibration.ReorderTabsEnabled = true;
            this.tabSptCalibration.SelectedTabFont = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold);
            this.tabSptCalibration.SelectedTabIndex = 0;
            this.tabSptCalibration.Size = new System.Drawing.Size(850, 603);
            this.tabSptCalibration.TabFont = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold);
            this.tabSptCalibration.TabIndex = 394;
            this.tabSptCalibration.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabItemData,
            this.tabItemCalibTool});
            this.tabSptCalibration.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.WinMediaPlayer12;
            this.tabSptCalibration.TabVerticalSpacing = 7;
            this.tabSptCalibration.Text = "superTabControl1";
            // 
            // tabPlCalibTools
            // 
            this.tabPlCalibTools.Controls.Add(this.labelX2);
            this.tabPlCalibTools.Controls.Add(this.labelX1);
            this.tabPlCalibTools.Controls.Add(this.lblMaxCount);
            this.tabPlCalibTools.Controls.Add(this.lblMaxCountLabel);
            this.tabPlCalibTools.Controls.Add(this.lblIntegratingTime);
            this.tabPlCalibTools.Controls.Add(this.dinChipIndex);
            this.tabPlCalibTools.Controls.Add(this.zedDailyWatch);
            this.tabPlCalibTools.Controls.Add(this.superTabStrip1);
            this.tabPlCalibTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPlCalibTools.Location = new System.Drawing.Point(0, 30);
            this.tabPlCalibTools.Name = "tabPlCalibTools";
            this.tabPlCalibTools.Size = new System.Drawing.Size(850, 573);
            this.tabPlCalibTools.TabIndex = 0;
            this.tabPlCalibTools.TabItem = this.tabItemCalibTool;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.labelX2.ForeColor = System.Drawing.Color.White;
            this.labelX2.Location = new System.Drawing.Point(120, 13);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(260, 23);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX2.TabIndex = 400;
            this.labelX2.Text = "Relative To Absolute";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelX1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelX1.Location = new System.Drawing.Point(600, 44);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(105, 22);
            this.labelX1.TabIndex = 399;
            this.labelX1.Text = "Rel. Max Count";
            // 
            // lblMaxCount
            // 
            this.lblMaxCount.BackColor = System.Drawing.Color.Lavender;
            this.lblMaxCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMaxCount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblMaxCount.ForeColor = System.Drawing.Color.Blue;
            this.lblMaxCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMaxCount.Location = new System.Drawing.Point(484, 44);
            this.lblMaxCount.Name = "lblMaxCount";
            this.lblMaxCount.Size = new System.Drawing.Size(97, 22);
            this.lblMaxCount.TabIndex = 398;
            this.lblMaxCount.Text = "0";
            this.lblMaxCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMaxCountLabel
            // 
            this.lblMaxCountLabel.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMaxCountLabel.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.lblMaxCountLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMaxCountLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblMaxCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblMaxCountLabel.Location = new System.Drawing.Point(373, 44);
            this.lblMaxCountLabel.Name = "lblMaxCountLabel";
            this.lblMaxCountLabel.Size = new System.Drawing.Size(105, 22);
            this.lblMaxCountLabel.TabIndex = 397;
            this.lblMaxCountLabel.Text = "Rel. Max Count";
            // 
            // lblIntegratingTime
            // 
            this.lblIntegratingTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblIntegratingTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.lblIntegratingTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblIntegratingTime.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblIntegratingTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblIntegratingTime.Location = new System.Drawing.Point(127, 44);
            this.lblIntegratingTime.Name = "lblIntegratingTime";
            this.lblIntegratingTime.Size = new System.Drawing.Size(105, 22);
            this.lblIntegratingTime.TabIndex = 396;
            this.lblIntegratingTime.Text = "IntegratingTime";
            // 
            // dinChipIndex
            // 
            // 
            // 
            // 
            this.dinChipIndex.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinChipIndex.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinChipIndex.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinChipIndex.DisplayFormat = "0";
            this.dinChipIndex.Increment = 1D;
            this.dinChipIndex.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Center;
            this.dinChipIndex.Location = new System.Drawing.Point(260, 44);
            this.dinChipIndex.Name = "dinChipIndex";
            this.dinChipIndex.ShowUpDown = true;
            this.dinChipIndex.Size = new System.Drawing.Size(81, 22);
            this.dinChipIndex.TabIndex = 395;
            this.dinChipIndex.Value = 3D;
            // 
            // zedDailyWatch
            // 
            this.zedDailyWatch.AutoScroll = true;
            this.zedDailyWatch.BackColor = System.Drawing.SystemColors.Window;
            this.zedDailyWatch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.zedDailyWatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.zedDailyWatch.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.zedDailyWatch.IsAutoScrollRange = true;
            this.zedDailyWatch.IsSynchronizeXAxes = true;
            this.zedDailyWatch.Location = new System.Drawing.Point(120, 86);
            this.zedDailyWatch.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.zedDailyWatch.Name = "zedDailyWatch";
            this.zedDailyWatch.ScrollGrace = 0D;
            this.zedDailyWatch.ScrollMaxX = 0D;
            this.zedDailyWatch.ScrollMaxY = 0D;
            this.zedDailyWatch.ScrollMaxY2 = 0D;
            this.zedDailyWatch.ScrollMinX = 0D;
            this.zedDailyWatch.ScrollMinY = 0D;
            this.zedDailyWatch.ScrollMinY2 = 0D;
            this.zedDailyWatch.Size = new System.Drawing.Size(679, 371);
            this.zedDailyWatch.TabIndex = 394;
            // 
            // tabItemCalibTool
            // 
            this.tabItemCalibTool.AttachedControl = this.tabPlCalibTools;
            this.tabItemCalibTool.GlobalItem = false;
            this.tabItemCalibTool.Name = "tabItemCalibTool";
            this.tabItemCalibTool.Text = "Calibration Tools";
            this.tabItemCalibTool.Visible = false;
            // 
            // tabPlCalibDisplay
            // 
            this.tabPlCalibDisplay.Controls.Add(this.plSptXaxisCoeff);
            this.tabPlCalibDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPlCalibDisplay.Location = new System.Drawing.Point(0, 30);
            this.tabPlCalibDisplay.Name = "tabPlCalibDisplay";
            this.tabPlCalibDisplay.Size = new System.Drawing.Size(850, 573);
            this.tabPlCalibDisplay.TabIndex = 1;
            this.tabPlCalibDisplay.TabItem = this.tabItemData;
            // 
            // tabItemData
            // 
            this.tabItemData.AttachedControl = this.tabPlCalibDisplay;
            this.tabItemData.GlobalItem = false;
            this.tabItemData.Name = "tabItemData";
            this.tabItemData.Text = "Calibration Data";
            // 
            // frmDailyWatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 597);
            this.Controls.Add(this.tabSptCalibration);
            this.Name = "frmDailyWatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spectrometer Calib Data";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSetTaskSheet_Load);
            this.plSptXaxisCoeff.ResumeLayout(false);
            this.plSptXaxisCoeff.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSptFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabSptCalibration)).EndInit();
            this.tabSptCalibration.ResumeLayout(false);
            this.tabPlCalibTools.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinChipIndex)).EndInit();
            this.tabPlCalibDisplay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnClose;
        private System.Windows.Forms.Panel plSptXaxisCoeff;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvSptFactor;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn colGainOffsetNo;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colSquare;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colGain;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colOffset;
        private System.Windows.Forms.TextBox txtYCalibFilePath;
        private DevComponents.DotNetBar.LabelX lblOthersSetting;
        private System.Windows.Forms.TextBox lblThirdCoefficient;
        private System.Windows.Forms.TextBox lblSecondCoefficient;
        private System.Windows.Forms.TextBox lblFirstCoefficient;
        private System.Windows.Forms.TextBox lblIntecept;
        private DevComponents.DotNetBar.LabelX labelX28;
        private DevComponents.DotNetBar.LabelX labelX27;
        private DevComponents.DotNetBar.LabelX labelX25;
        private DevComponents.DotNetBar.LabelX labelX26;
        private DevComponents.DotNetBar.SuperTabStrip superTabStrip1;
        private DevComponents.DotNetBar.ButtonItem btnLoadStdFile;
        private DevComponents.DotNetBar.ButtonItem btnLoadMsrtFile;
        private DevComponents.DotNetBar.ButtonItem btnGetDark;
        private DevComponents.DotNetBar.ButtonItem btnSaveCoeffToSystem;
        private DevComponents.DotNetBar.ButtonItem btnUnLack;
        private DevComponents.DotNetBar.ButtonItem btnReturn;
        private DevComponents.DotNetBar.SuperTabControl tabSptCalibration;
        private DevComponents.DotNetBar.SuperTabControlPanel tabPlCalibDisplay;
        private DevComponents.DotNetBar.SuperTabItem tabItemData;
        private DevComponents.DotNetBar.SuperTabControlPanel tabPlCalibTools;
        private DevComponents.DotNetBar.SuperTabItem tabItemCalibTool;
        private ZedGraph.ZedGraphControl zedDailyWatch;
        private DevComponents.Editors.DoubleInput dinChipIndex;
        private DevComponents.DotNetBar.LabelX lblMaxCountLabel;
        private DevComponents.DotNetBar.LabelX lblIntegratingTime;
        private System.Windows.Forms.Label lblMaxCount;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;

    }
}