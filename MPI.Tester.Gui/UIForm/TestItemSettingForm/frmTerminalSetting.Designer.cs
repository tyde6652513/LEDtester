namespace MPI.Tester.Gui
{
    partial class frmTerminalSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTerminalSetting));
            this.grpTitle = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpApplySetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.tabpBias = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.pnlBias = new System.Windows.Forms.Panel();
            this.lblForceValue = new DevComponents.DotNetBar.LabelX();
            this.dinForceValue = new DevComponents.Editors.DoubleInput();
            this.lblForceValueUnit = new DevComponents.DotNetBar.LabelX();
            this.tabiBias = new DevComponents.DotNetBar.SuperTabItem();
            this.tabpSweep = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.pnlSweep = new System.Windows.Forms.Panel();
            this.lblCustomList = new DevComponents.DotNetBar.LabelX();
            this.dgvCustomList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colForceValue = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.txtDisplayStepValue = new DevComponents.DotNetBar.LabelX();
            this.lblStepValue = new DevComponents.DotNetBar.LabelX();
            this.lblStepValueUnit = new DevComponents.DotNetBar.LabelX();
            this.lblEndValue = new DevComponents.DotNetBar.LabelX();
            this.dinEndValue = new DevComponents.Editors.DoubleInput();
            this.lblEndValueUnit = new DevComponents.DotNetBar.LabelX();
            this.lblStartValue = new DevComponents.DotNetBar.LabelX();
            this.dinStartValue = new DevComponents.Editors.DoubleInput();
            this.lblStartValueUnit = new DevComponents.DotNetBar.LabelX();
            this.lblSweepMode = new DevComponents.DotNetBar.LabelX();
            this.cmbSweepMode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.tabiSweep = new DevComponents.DotNetBar.SuperTabItem();
            this.grpMsrtSetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblMsrtClamp = new DevComponents.DotNetBar.LabelX();
            this.dinMsrtClamp = new DevComponents.Editors.DoubleInput();
            this.lblMsrtClampUnit = new DevComponents.DotNetBar.LabelX();
            this.cmbTestSelected = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblMsrtType = new DevComponents.DotNetBar.LabelX();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.grpTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpApplySetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.tabpBias.SuspendLayout();
            this.pnlBias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).BeginInit();
            this.tabpSweep.SuspendLayout();
            this.pnlSweep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinEndValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinStartValue)).BeginInit();
            this.grpMsrtSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).BeginInit();
            this.SuspendLayout();
            // 
            // grpTitle
            // 
            this.grpTitle.BackColor = System.Drawing.SystemColors.Control;
            this.grpTitle.Controls.Add(this.splitContainer1);
            this.grpTitle.Controls.Add(this.cmbTestSelected);
            this.grpTitle.Controls.Add(this.lblMsrtType);
            this.grpTitle.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold);
            this.grpTitle.ForeColor = System.Drawing.Color.DimGray;
            this.grpTitle.Location = new System.Drawing.Point(8, 12);
            this.grpTitle.Name = "grpTitle";
            this.grpTitle.Size = new System.Drawing.Size(451, 498);
            this.grpTitle.TabIndex = 1;
            this.grpTitle.TabStop = false;
            this.grpTitle.Text = "Title";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(6, 76);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpApplySetting);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpMsrtSetting);
            this.splitContainer1.Size = new System.Drawing.Size(437, 413);
            this.splitContainer1.SplitterDistance = 337;
            this.splitContainer1.TabIndex = 95;
            // 
            // grpApplySetting
            // 
            this.grpApplySetting.BackColor = System.Drawing.Color.Transparent;
            this.grpApplySetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpApplySetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpApplySetting.Controls.Add(this.superTabControl1);
            this.grpApplySetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpApplySetting.DrawTitleBox = false;
            this.grpApplySetting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grpApplySetting.Location = new System.Drawing.Point(0, 0);
            this.grpApplySetting.Margin = new System.Windows.Forms.Padding(0);
            this.grpApplySetting.Name = "grpApplySetting";
            this.grpApplySetting.Size = new System.Drawing.Size(437, 337);
            // 
            // 
            // 
            this.grpApplySetting.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpApplySetting.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpApplySetting.Style.BackColorGradientAngle = 90;
            this.grpApplySetting.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpApplySetting.Style.BorderBottomWidth = 1;
            this.grpApplySetting.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpApplySetting.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpApplySetting.Style.BorderLeftWidth = 1;
            this.grpApplySetting.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpApplySetting.Style.BorderRightWidth = 1;
            this.grpApplySetting.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpApplySetting.Style.BorderTopWidth = 1;
            this.grpApplySetting.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.grpApplySetting.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpApplySetting.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpApplySetting.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.grpApplySetting.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpApplySetting.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.grpApplySetting.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpApplySetting.TabIndex = 95;
            this.grpApplySetting.Text = "APPLY SETTING";
            // 
            // superTabControl1
            // 
            this.superTabControl1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.superTabControl1.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.Controls.Add(this.tabpSweep);
            this.superTabControl1.Controls.Add(this.tabpBias);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.superTabControl1.ForeColor = System.Drawing.Color.Black;
            this.superTabControl1.Location = new System.Drawing.Point(0, 0);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.superTabControl1.SelectedTabIndex = 1;
            this.superTabControl1.Size = new System.Drawing.Size(427, 305);
            this.superTabControl1.TabFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.superTabControl1.TabIndex = 0;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabiBias,
            this.tabiSweep});
            this.superTabControl1.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.VisualStudio2008Dock;
            this.superTabControl1.TabsVisible = false;
            this.superTabControl1.Text = "superTabControl1";
            // 
            // tabpBias
            // 
            this.tabpBias.Controls.Add(this.pnlBias);
            this.tabpBias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabpBias.Location = new System.Drawing.Point(0, 29);
            this.tabpBias.Name = "tabpBias";
            this.tabpBias.Size = new System.Drawing.Size(427, 276);
            this.tabpBias.TabIndex = 1;
            this.tabpBias.TabItem = this.tabiBias;
            // 
            // pnlBias
            // 
            this.pnlBias.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBias.Controls.Add(this.lblForceValue);
            this.pnlBias.Controls.Add(this.dinForceValue);
            this.pnlBias.Controls.Add(this.lblForceValueUnit);
            this.pnlBias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBias.Location = new System.Drawing.Point(0, 0);
            this.pnlBias.Name = "pnlBias";
            this.pnlBias.Size = new System.Drawing.Size(427, 276);
            this.pnlBias.TabIndex = 0;
            // 
            // lblForceValue
            // 
            this.lblForceValue.AutoSize = true;
            this.lblForceValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForceValue.ForeColor = System.Drawing.Color.Black;
            this.lblForceValue.Location = new System.Drawing.Point(10, 20);
            this.lblForceValue.Name = "lblForceValue";
            this.lblForceValue.Size = new System.Drawing.Size(83, 19);
            this.lblForceValue.TabIndex = 108;
            this.lblForceValue.Text = "Force Value";
            // 
            // dinForceValue
            // 
            // 
            // 
            // 
            this.dinForceValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceValue.DisplayFormat = "0.000";
            this.dinForceValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dinForceValue.Increment = 1D;
            this.dinForceValue.Location = new System.Drawing.Point(100, 20);
            this.dinForceValue.MaxValue = 10000D;
            this.dinForceValue.MinValue = 0D;
            this.dinForceValue.Name = "dinForceValue";
            this.dinForceValue.ShowUpDown = true;
            this.dinForceValue.Size = new System.Drawing.Size(88, 22);
            this.dinForceValue.TabIndex = 109;
            this.dinForceValue.Value = 1D;
            // 
            // lblForceValueUnit
            // 
            this.lblForceValueUnit.AutoSize = true;
            this.lblForceValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblForceValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceValueUnit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForceValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblForceValueUnit.Location = new System.Drawing.Point(194, 20);
            this.lblForceValueUnit.Name = "lblForceValueUnit";
            this.lblForceValueUnit.Size = new System.Drawing.Size(28, 19);
            this.lblForceValueUnit.TabIndex = 110;
            this.lblForceValueUnit.Text = "mA";
            // 
            // tabiBias
            // 
            this.tabiBias.AttachedControl = this.tabpBias;
            this.tabiBias.GlobalItem = false;
            this.tabiBias.Name = "tabiBias";
            this.tabiBias.Text = "Bias";
            // 
            // tabpSweep
            // 
            this.tabpSweep.Controls.Add(this.pnlSweep);
            this.tabpSweep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabpSweep.Location = new System.Drawing.Point(0, 29);
            this.tabpSweep.Name = "tabpSweep";
            this.tabpSweep.Size = new System.Drawing.Size(427, 276);
            this.tabpSweep.TabIndex = 0;
            this.tabpSweep.TabItem = this.tabiSweep;
            // 
            // pnlSweep
            // 
            this.pnlSweep.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlSweep.Controls.Add(this.lblCustomList);
            this.pnlSweep.Controls.Add(this.dgvCustomList);
            this.pnlSweep.Controls.Add(this.txtDisplayStepValue);
            this.pnlSweep.Controls.Add(this.lblStepValue);
            this.pnlSweep.Controls.Add(this.lblStepValueUnit);
            this.pnlSweep.Controls.Add(this.lblEndValue);
            this.pnlSweep.Controls.Add(this.dinEndValue);
            this.pnlSweep.Controls.Add(this.lblEndValueUnit);
            this.pnlSweep.Controls.Add(this.lblStartValue);
            this.pnlSweep.Controls.Add(this.dinStartValue);
            this.pnlSweep.Controls.Add(this.lblStartValueUnit);
            this.pnlSweep.Controls.Add(this.lblSweepMode);
            this.pnlSweep.Controls.Add(this.cmbSweepMode);
            this.pnlSweep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSweep.Location = new System.Drawing.Point(0, 0);
            this.pnlSweep.Name = "pnlSweep";
            this.pnlSweep.Size = new System.Drawing.Size(427, 276);
            this.pnlSweep.TabIndex = 1;
            // 
            // lblCustomList
            // 
            this.lblCustomList.AutoSize = true;
            this.lblCustomList.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCustomList.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblCustomList.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCustomList.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomList.ForeColor = System.Drawing.Color.Black;
            this.lblCustomList.Location = new System.Drawing.Point(238, 14);
            this.lblCustomList.Name = "lblCustomList";
            this.lblCustomList.Size = new System.Drawing.Size(84, 19);
            this.lblCustomList.TabIndex = 121;
            this.lblCustomList.Text = "Custom List";
            // 
            // dgvCustomList
            // 
            this.dgvCustomList.AllowUserToAddRows = false;
            this.dgvCustomList.AllowUserToDeleteRows = false;
            this.dgvCustomList.AllowUserToResizeColumns = false;
            this.dgvCustomList.AllowUserToResizeRows = false;
            this.dgvCustomList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colForceValue});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCustomList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCustomList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvCustomList.Location = new System.Drawing.Point(238, 34);
            this.dgvCustomList.MultiSelect = false;
            this.dgvCustomList.Name = "dgvCustomList";
            this.dgvCustomList.RowHeadersVisible = false;
            this.dgvCustomList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvCustomList.RowTemplate.Height = 24;
            this.dgvCustomList.Size = new System.Drawing.Size(180, 236);
            this.dgvCustomList.TabIndex = 120;
            // 
            // colNo
            // 
            this.colNo.HeaderText = "No";
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNo.Width = 30;
            // 
            // colForceValue
            // 
            // 
            // 
            // 
            this.colForceValue.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colForceValue.HeaderText = "Force Value";
            this.colForceValue.Increment = 1D;
            this.colForceValue.MaxValue = 2000D;
            this.colForceValue.MinValue = -2000D;
            this.colForceValue.Name = "colForceValue";
            this.colForceValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colForceValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colForceValue.Width = 130;
            // 
            // txtDisplayStepValue
            // 
            this.txtDisplayStepValue.BackColor = System.Drawing.Color.Gainsboro;
            // 
            // 
            // 
            this.txtDisplayStepValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.txtDisplayStepValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtDisplayStepValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDisplayStepValue.ForeColor = System.Drawing.Color.Black;
            this.txtDisplayStepValue.Location = new System.Drawing.Point(100, 125);
            this.txtDisplayStepValue.Name = "txtDisplayStepValue";
            this.txtDisplayStepValue.Size = new System.Drawing.Size(88, 25);
            this.txtDisplayStepValue.TabIndex = 119;
            this.txtDisplayStepValue.Text = "Step Value";
            this.txtDisplayStepValue.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblStepValue
            // 
            this.lblStepValue.AutoSize = true;
            this.lblStepValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStepValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblStepValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStepValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepValue.ForeColor = System.Drawing.Color.Black;
            this.lblStepValue.Location = new System.Drawing.Point(10, 125);
            this.lblStepValue.Name = "lblStepValue";
            this.lblStepValue.Size = new System.Drawing.Size(75, 19);
            this.lblStepValue.TabIndex = 117;
            this.lblStepValue.Text = "Step Value";
            // 
            // lblStepValueUnit
            // 
            this.lblStepValueUnit.AutoSize = true;
            this.lblStepValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStepValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblStepValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStepValueUnit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblStepValueUnit.Location = new System.Drawing.Point(194, 125);
            this.lblStepValueUnit.Name = "lblStepValueUnit";
            this.lblStepValueUnit.Size = new System.Drawing.Size(28, 19);
            this.lblStepValueUnit.TabIndex = 118;
            this.lblStepValueUnit.Text = "mA";
            // 
            // lblEndValue
            // 
            this.lblEndValue.AutoSize = true;
            this.lblEndValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblEndValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblEndValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblEndValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndValue.ForeColor = System.Drawing.Color.Black;
            this.lblEndValue.Location = new System.Drawing.Point(10, 90);
            this.lblEndValue.Name = "lblEndValue";
            this.lblEndValue.Size = new System.Drawing.Size(72, 19);
            this.lblEndValue.TabIndex = 111;
            this.lblEndValue.Text = "End Value";
            // 
            // dinEndValue
            // 
            // 
            // 
            // 
            this.dinEndValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinEndValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinEndValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinEndValue.DisplayFormat = "0.000";
            this.dinEndValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dinEndValue.Increment = 1D;
            this.dinEndValue.Location = new System.Drawing.Point(100, 90);
            this.dinEndValue.MaxValue = 10000D;
            this.dinEndValue.MinValue = 0D;
            this.dinEndValue.Name = "dinEndValue";
            this.dinEndValue.ShowUpDown = true;
            this.dinEndValue.Size = new System.Drawing.Size(88, 22);
            this.dinEndValue.TabIndex = 112;
            this.dinEndValue.Value = 1D;
            // 
            // lblEndValueUnit
            // 
            this.lblEndValueUnit.AutoSize = true;
            this.lblEndValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblEndValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblEndValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblEndValueUnit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblEndValueUnit.Location = new System.Drawing.Point(194, 90);
            this.lblEndValueUnit.Name = "lblEndValueUnit";
            this.lblEndValueUnit.Size = new System.Drawing.Size(28, 19);
            this.lblEndValueUnit.TabIndex = 113;
            this.lblEndValueUnit.Text = "mA";
            // 
            // lblStartValue
            // 
            this.lblStartValue.AutoSize = true;
            this.lblStartValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStartValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblStartValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStartValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartValue.ForeColor = System.Drawing.Color.Black;
            this.lblStartValue.Location = new System.Drawing.Point(10, 55);
            this.lblStartValue.Name = "lblStartValue";
            this.lblStartValue.Size = new System.Drawing.Size(77, 19);
            this.lblStartValue.TabIndex = 108;
            this.lblStartValue.Text = "Start Value";
            // 
            // dinStartValue
            // 
            // 
            // 
            // 
            this.dinStartValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinStartValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinStartValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinStartValue.DisplayFormat = "0.000";
            this.dinStartValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dinStartValue.Increment = 1D;
            this.dinStartValue.Location = new System.Drawing.Point(100, 55);
            this.dinStartValue.MaxValue = 10000D;
            this.dinStartValue.MinValue = 0D;
            this.dinStartValue.Name = "dinStartValue";
            this.dinStartValue.ShowUpDown = true;
            this.dinStartValue.Size = new System.Drawing.Size(88, 22);
            this.dinStartValue.TabIndex = 109;
            this.dinStartValue.Value = 1D;
            // 
            // lblStartValueUnit
            // 
            this.lblStartValueUnit.AutoSize = true;
            this.lblStartValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStartValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblStartValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStartValueUnit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblStartValueUnit.Location = new System.Drawing.Point(194, 55);
            this.lblStartValueUnit.Name = "lblStartValueUnit";
            this.lblStartValueUnit.Size = new System.Drawing.Size(28, 19);
            this.lblStartValueUnit.TabIndex = 110;
            this.lblStartValueUnit.Text = "mA";
            // 
            // lblSweepMode
            // 
            this.lblSweepMode.AutoSize = true;
            this.lblSweepMode.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblSweepMode.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblSweepMode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSweepMode.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSweepMode.ForeColor = System.Drawing.Color.Black;
            this.lblSweepMode.Location = new System.Drawing.Point(10, 20);
            this.lblSweepMode.Name = "lblSweepMode";
            this.lblSweepMode.Size = new System.Drawing.Size(88, 19);
            this.lblSweepMode.TabIndex = 97;
            this.lblSweepMode.Text = "Sweep Mode";
            // 
            // cmbSweepMode
            // 
            this.cmbSweepMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSweepMode.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbSweepMode.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSweepMode.Location = new System.Drawing.Point(100, 20);
            this.cmbSweepMode.Name = "cmbSweepMode";
            this.cmbSweepMode.Size = new System.Drawing.Size(88, 24);
            this.cmbSweepMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.cmbSweepMode.TabIndex = 98;
            // 
            // tabiSweep
            // 
            this.tabiSweep.AttachedControl = this.tabpSweep;
            this.tabiSweep.GlobalItem = false;
            this.tabiSweep.Name = "tabiSweep";
            this.tabiSweep.Text = "Sweep";
            // 
            // grpMsrtSetting
            // 
            this.grpMsrtSetting.BackColor = System.Drawing.Color.Transparent;
            this.grpMsrtSetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpMsrtSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpMsrtSetting.Controls.Add(this.lblMsrtClamp);
            this.grpMsrtSetting.Controls.Add(this.dinMsrtClamp);
            this.grpMsrtSetting.Controls.Add(this.lblMsrtClampUnit);
            this.grpMsrtSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMsrtSetting.DrawTitleBox = false;
            this.grpMsrtSetting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grpMsrtSetting.Location = new System.Drawing.Point(0, 0);
            this.grpMsrtSetting.Margin = new System.Windows.Forms.Padding(0);
            this.grpMsrtSetting.Name = "grpMsrtSetting";
            this.grpMsrtSetting.Size = new System.Drawing.Size(437, 72);
            // 
            // 
            // 
            this.grpMsrtSetting.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpMsrtSetting.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpMsrtSetting.Style.BackColorGradientAngle = 90;
            this.grpMsrtSetting.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpMsrtSetting.Style.BorderBottomWidth = 1;
            this.grpMsrtSetting.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpMsrtSetting.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpMsrtSetting.Style.BorderLeftWidth = 1;
            this.grpMsrtSetting.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpMsrtSetting.Style.BorderRightWidth = 1;
            this.grpMsrtSetting.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpMsrtSetting.Style.BorderTopWidth = 1;
            this.grpMsrtSetting.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.grpMsrtSetting.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpMsrtSetting.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpMsrtSetting.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.grpMsrtSetting.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpMsrtSetting.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.grpMsrtSetting.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpMsrtSetting.TabIndex = 96;
            this.grpMsrtSetting.Text = "MEASUREMENT SETTING";
            // 
            // lblMsrtClamp
            // 
            this.lblMsrtClamp.AutoSize = true;
            this.lblMsrtClamp.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtClamp.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblMsrtClamp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtClamp.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsrtClamp.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtClamp.Location = new System.Drawing.Point(14, 10);
            this.lblMsrtClamp.Name = "lblMsrtClamp";
            this.lblMsrtClamp.Size = new System.Drawing.Size(80, 19);
            this.lblMsrtClamp.TabIndex = 165;
            this.lblMsrtClamp.Text = "Msrt Clamp";
            // 
            // dinMsrtClamp
            // 
            // 
            // 
            // 
            this.dinMsrtClamp.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinMsrtClamp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinMsrtClamp.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinMsrtClamp.DisplayFormat = "0.0";
            this.dinMsrtClamp.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dinMsrtClamp.ForeColor = System.Drawing.Color.Black;
            this.dinMsrtClamp.Increment = 1D;
            this.dinMsrtClamp.Location = new System.Drawing.Point(100, 10);
            this.dinMsrtClamp.MaxValue = 300D;
            this.dinMsrtClamp.MinValue = 1D;
            this.dinMsrtClamp.Name = "dinMsrtClamp";
            this.dinMsrtClamp.ShowUpDown = true;
            this.dinMsrtClamp.Size = new System.Drawing.Size(92, 22);
            this.dinMsrtClamp.TabIndex = 166;
            this.dinMsrtClamp.Value = 8D;
            // 
            // lblMsrtClampUnit
            // 
            this.lblMsrtClampUnit.AutoSize = true;
            this.lblMsrtClampUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtClampUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblMsrtClampUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtClampUnit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsrtClampUnit.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtClampUnit.Location = new System.Drawing.Point(198, 10);
            this.lblMsrtClampUnit.Name = "lblMsrtClampUnit";
            this.lblMsrtClampUnit.Size = new System.Drawing.Size(16, 19);
            this.lblMsrtClampUnit.TabIndex = 167;
            this.lblMsrtClampUnit.Text = "V";
            // 
            // cmbTestSelected
            // 
            this.cmbTestSelected.DisplayMember = "Text";
            this.cmbTestSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTestSelected.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbTestSelected.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTestSelected.FormattingEnabled = true;
            this.cmbTestSelected.ItemHeight = 16;
            this.cmbTestSelected.Location = new System.Drawing.Point(160, 43);
            this.cmbTestSelected.Name = "cmbTestSelected";
            this.cmbTestSelected.Size = new System.Drawing.Size(130, 24);
            this.cmbTestSelected.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.cmbTestSelected.TabIndex = 94;
            this.cmbTestSelected.SelectedIndexChanged += new System.EventHandler(this.cmbTestSelected_SelectedIndexChanged);
            // 
            // lblMsrtType
            // 
            this.lblMsrtType.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtType.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblMsrtType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtType.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsrtType.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtType.Location = new System.Drawing.Point(17, 40);
            this.lblMsrtType.Name = "lblMsrtType";
            this.lblMsrtType.Size = new System.Drawing.Size(137, 27);
            this.lblMsrtType.TabIndex = 93;
            this.lblMsrtType.Text = "Function Type";
            this.lblMsrtType.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfirm.AntiAlias = true;
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FocusCuesEnabled = false;
            this.btnConfirm.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.Image = ((System.Drawing.Image)(resources.GetObject("btnConfirm.Image")));
            this.btnConfirm.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnConfirm.Location = new System.Drawing.Point(223, 516);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnConfirm.ShowSubItems = false;
            this.btnConfirm.Size = new System.Drawing.Size(115, 40);
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnConfirm.TabIndex = 93;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.AntiAlias = true;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FocusCuesEnabled = false;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnCancel.Location = new System.Drawing.Point(344, 516);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnCancel.ShowSubItems = false;
            this.btnCancel.Size = new System.Drawing.Size(115, 40);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCancel.TabIndex = 92;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmTerminalSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.ClientSize = new System.Drawing.Size(467, 565);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTerminalSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Terminal Setting";
            this.TopMost = true;
            this.grpTitle.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpApplySetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.tabpBias.ResumeLayout(false);
            this.pnlBias.ResumeLayout(false);
            this.pnlBias.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).EndInit();
            this.tabpSweep.ResumeLayout(false);
            this.pnlSweep.ResumeLayout(false);
            this.pnlSweep.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinEndValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinStartValue)).EndInit();
            this.grpMsrtSetting.ResumeLayout(false);
            this.grpMsrtSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTitle;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTestSelected;
        private DevComponents.DotNetBar.LabelX lblMsrtType;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.Controls.GroupPanel grpApplySetting;
        private DevComponents.DotNetBar.Controls.GroupPanel grpMsrtSetting;
        private DevComponents.DotNetBar.LabelX lblMsrtClamp;
        private DevComponents.Editors.DoubleInput dinMsrtClamp;
        private DevComponents.DotNetBar.LabelX lblMsrtClampUnit;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel tabpBias;
        private DevComponents.DotNetBar.SuperTabItem tabiBias;
        private DevComponents.DotNetBar.SuperTabControlPanel tabpSweep;
        private DevComponents.DotNetBar.SuperTabItem tabiSweep;
        private System.Windows.Forms.Panel pnlSweep;
        private System.Windows.Forms.Panel pnlBias;
        private DevComponents.DotNetBar.LabelX lblForceValue;
        private DevComponents.Editors.DoubleInput dinForceValue;
        private DevComponents.DotNetBar.LabelX lblForceValueUnit;
        private DevComponents.DotNetBar.LabelX lblSweepMode;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSweepMode;
        private DevComponents.DotNetBar.LabelX lblStartValue;
        private DevComponents.Editors.DoubleInput dinStartValue;
        private DevComponents.DotNetBar.LabelX lblStartValueUnit;
        private DevComponents.DotNetBar.LabelX lblEndValue;
        private DevComponents.Editors.DoubleInput dinEndValue;
        private DevComponents.DotNetBar.LabelX lblEndValueUnit;
        private DevComponents.DotNetBar.LabelX txtDisplayStepValue;
        private DevComponents.DotNetBar.LabelX lblStepValue;
        private DevComponents.DotNetBar.LabelX lblStepValueUnit;
        private DevComponents.DotNetBar.LabelX lblCustomList;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvCustomList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colForceValue;

    }
}