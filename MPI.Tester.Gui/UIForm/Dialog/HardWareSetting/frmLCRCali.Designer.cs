namespace MPI.Tester.Gui
{
    partial class frmLCRCali
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
            this.btnload = new DevComponents.DotNetBar.ButtonX();
            this.grpEnable = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chkLoad = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkShort = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkOpen = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnShort = new DevComponents.DotNetBar.ButtonX();
            this.btnOpen = new DevComponents.DotNetBar.ButtonX();
            this.grpCaliData = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.dinFrequency = new DevComponents.Editors.DoubleInput();
            this.btnSet = new DevComponents.DotNetBar.ButtonX();
            this.dinRefB = new DevComponents.Editors.DoubleInput();
            this.dinRefA = new DevComponents.Editors.DoubleInput();
            this.cmbRefAUnit = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.chkEnableData = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cmbDataNum = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.grpGeneral = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dinLevel = new DevComponents.Editors.DoubleInput();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.dinBias = new DevComponents.Editors.DoubleInput();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.cmbCable = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbFunc = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.grpTestCtrl = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.grpEnable.SuspendLayout();
            this.grpCaliData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinRefB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinRefA)).BeginInit();
            this.grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinBias)).BeginInit();
            this.grpTestCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnload
            // 
            this.btnload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnload.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnload.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnload.Location = new System.Drawing.Point(16, 140);
            this.btnload.Name = "btnload";
            this.btnload.Size = new System.Drawing.Size(130, 42);
            this.btnload.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnload.TabIndex = 15;
            this.btnload.Text = "Test Load";
            this.btnload.Click += new System.EventHandler(this.btnload_Click);
            // 
            // grpEnable
            // 
            this.grpEnable.BackColor = System.Drawing.SystemColors.Control;
            this.grpEnable.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpEnable.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpEnable.Controls.Add(this.chkLoad);
            this.grpEnable.Controls.Add(this.chkShort);
            this.grpEnable.Controls.Add(this.chkOpen);
            this.grpEnable.DrawTitleBox = false;
            this.grpEnable.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpEnable.Location = new System.Drawing.Point(3, 249);
            this.grpEnable.Name = "grpEnable";
            this.grpEnable.Size = new System.Drawing.Size(160, 180);
            // 
            // 
            // 
            this.grpEnable.Style.BackColor = System.Drawing.SystemColors.Control;
            this.grpEnable.Style.BackColor2 = System.Drawing.SystemColors.Control;
            this.grpEnable.Style.BackColorGradientAngle = 90;
            this.grpEnable.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpEnable.Style.BorderBottomWidth = 1;
            this.grpEnable.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpEnable.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Dash;
            this.grpEnable.Style.BorderLeftWidth = 1;
            this.grpEnable.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpEnable.Style.BorderRightWidth = 1;
            this.grpEnable.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpEnable.Style.BorderTopWidth = 1;
            this.grpEnable.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpEnable.Style.CornerDiameter = 4;
            this.grpEnable.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpEnable.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpEnable.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpEnable.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpEnable.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpEnable.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpEnable.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpEnable.TabIndex = 14;
            this.grpEnable.Text = "Compensate";
            // 
            // chkLoad
            // 
            // 
            // 
            // 
            this.chkLoad.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkLoad.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkLoad.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.chkLoad.Location = new System.Drawing.Point(10, 85);
            this.chkLoad.Name = "chkLoad";
            this.chkLoad.Size = new System.Drawing.Size(111, 39);
            this.chkLoad.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkLoad.TabIndex = 7;
            this.chkLoad.Text = "Load";
            this.chkLoad.TextColor = System.Drawing.Color.Black;
            // 
            // chkShort
            // 
            // 
            // 
            // 
            this.chkShort.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkShort.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkShort.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.chkShort.Location = new System.Drawing.Point(10, 43);
            this.chkShort.Name = "chkShort";
            this.chkShort.Size = new System.Drawing.Size(111, 39);
            this.chkShort.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkShort.TabIndex = 6;
            this.chkShort.Text = "Short";
            this.chkShort.TextColor = System.Drawing.Color.Black;
            // 
            // chkOpen
            // 
            // 
            // 
            // 
            this.chkOpen.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkOpen.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkOpen.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.chkOpen.Location = new System.Drawing.Point(10, 3);
            this.chkOpen.Name = "chkOpen";
            this.chkOpen.Size = new System.Drawing.Size(111, 39);
            this.chkOpen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkOpen.TabIndex = 5;
            this.chkOpen.Text = "Open";
            this.chkOpen.TextColor = System.Drawing.Color.Black;
            // 
            // btnShort
            // 
            this.btnShort.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnShort.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnShort.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnShort.Location = new System.Drawing.Point(16, 78);
            this.btnShort.Name = "btnShort";
            this.btnShort.Size = new System.Drawing.Size(130, 42);
            this.btnShort.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnShort.TabIndex = 14;
            this.btnShort.Text = "Test Short";
            this.btnShort.Click += new System.EventHandler(this.btnShort_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpen.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOpen.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnOpen.Location = new System.Drawing.Point(16, 17);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(130, 42);
            this.btnOpen.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnOpen.TabIndex = 13;
            this.btnOpen.Text = "Test Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // grpCaliData
            // 
            this.grpCaliData.BackColor = System.Drawing.SystemColors.Control;
            this.grpCaliData.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpCaliData.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpCaliData.Controls.Add(this.labelX7);
            this.grpCaliData.Controls.Add(this.dinFrequency);
            this.grpCaliData.Controls.Add(this.btnSet);
            this.grpCaliData.Controls.Add(this.dinRefB);
            this.grpCaliData.Controls.Add(this.dinRefA);
            this.grpCaliData.Controls.Add(this.cmbRefAUnit);
            this.grpCaliData.Controls.Add(this.labelX6);
            this.grpCaliData.Controls.Add(this.labelX5);
            this.grpCaliData.Controls.Add(this.chkEnableData);
            this.grpCaliData.Controls.Add(this.labelX3);
            this.grpCaliData.Controls.Add(this.cmbDataNum);
            this.grpCaliData.Controls.Add(this.labelX4);
            this.grpCaliData.DrawTitleBox = false;
            this.grpCaliData.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.grpCaliData.Location = new System.Drawing.Point(12, 144);
            this.grpCaliData.Name = "grpCaliData";
            this.grpCaliData.Size = new System.Drawing.Size(420, 324);
            // 
            // 
            // 
            this.grpCaliData.Style.BackColor = System.Drawing.SystemColors.Control;
            this.grpCaliData.Style.BackColor2 = System.Drawing.SystemColors.Control;
            this.grpCaliData.Style.BackColorGradientAngle = 90;
            this.grpCaliData.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCaliData.Style.BorderBottomWidth = 1;
            this.grpCaliData.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpCaliData.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Dash;
            this.grpCaliData.Style.BorderLeftWidth = 1;
            this.grpCaliData.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCaliData.Style.BorderRightWidth = 1;
            this.grpCaliData.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCaliData.Style.BorderTopWidth = 1;
            this.grpCaliData.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpCaliData.Style.CornerDiameter = 4;
            this.grpCaliData.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpCaliData.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpCaliData.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpCaliData.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpCaliData.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpCaliData.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpCaliData.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpCaliData.TabIndex = 2;
            this.grpCaliData.Text = "Calibration Data";
            // 
            // labelX7
            // 
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX7.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX7.Location = new System.Drawing.Point(297, 93);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(114, 23);
            this.labelX7.TabIndex = 14;
            this.labelX7.Text = "Hz";
            // 
            // dinFrequency
            // 
            // 
            // 
            // 
            this.dinFrequency.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinFrequency.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinFrequency.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinFrequency.DisplayFormat = "0,0";
            this.dinFrequency.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinFrequency.Increment = 1D;
            this.dinFrequency.Location = new System.Drawing.Point(167, 93);
            this.dinFrequency.MaxValue = 1000000D;
            this.dinFrequency.MinValue = 1D;
            this.dinFrequency.Name = "dinFrequency";
            this.dinFrequency.ShowUpDown = true;
            this.dinFrequency.Size = new System.Drawing.Size(121, 25);
            this.dinFrequency.TabIndex = 13;
            this.dinFrequency.Value = 1D;
            // 
            // btnSet
            // 
            this.btnSet.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSet.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSet.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnSet.Location = new System.Drawing.Point(298, 236);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(102, 42);
            this.btnSet.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSet.TabIndex = 12;
            this.btnSet.Text = "Set Data";
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // dinRefB
            // 
            // 
            // 
            // 
            this.dinRefB.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRefB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRefB.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRefB.DisplayFormat = "0.00000";
            this.dinRefB.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinRefB.Increment = 1D;
            this.dinRefB.Location = new System.Drawing.Point(58, 218);
            this.dinRefB.MaxValue = 10D;
            this.dinRefB.MinValue = -10D;
            this.dinRefB.Name = "dinRefB";
            this.dinRefB.ShowUpDown = true;
            this.dinRefB.Size = new System.Drawing.Size(145, 25);
            this.dinRefB.TabIndex = 11;
            // 
            // dinRefA
            // 
            // 
            // 
            // 
            this.dinRefA.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRefA.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRefA.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRefA.DisplayFormat = "0.000";
            this.dinRefA.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinRefA.Increment = 1D;
            this.dinRefA.Location = new System.Drawing.Point(58, 161);
            this.dinRefA.MaxValue = 9999D;
            this.dinRefA.MinValue = 1D;
            this.dinRefA.Name = "dinRefA";
            this.dinRefA.ShowUpDown = true;
            this.dinRefA.Size = new System.Drawing.Size(145, 25);
            this.dinRefA.TabIndex = 10;
            this.dinRefA.Value = 1D;
            // 
            // cmbRefAUnit
            // 
            this.cmbRefAUnit.DisplayMember = "Text";
            this.cmbRefAUnit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRefAUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRefAUnit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.cmbRefAUnit.FormattingEnabled = true;
            this.cmbRefAUnit.ItemHeight = 19;
            this.cmbRefAUnit.Location = new System.Drawing.Point(209, 161);
            this.cmbRefAUnit.Name = "cmbRefAUnit";
            this.cmbRefAUnit.Size = new System.Drawing.Size(69, 25);
            this.cmbRefAUnit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbRefAUnit.TabIndex = 9;
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX6.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX6.Location = new System.Drawing.Point(13, 192);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(114, 23);
            this.labelX6.TabIndex = 6;
            this.labelX6.Text = "Ref B";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX5.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX5.Location = new System.Drawing.Point(13, 132);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(114, 23);
            this.labelX5.TabIndex = 5;
            this.labelX5.Text = "Ref A";
            // 
            // chkEnableData
            // 
            // 
            // 
            // 
            this.chkEnableData.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkEnableData.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableData.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.chkEnableData.Location = new System.Drawing.Point(6, 3);
            this.chkEnableData.Name = "chkEnableData";
            this.chkEnableData.Size = new System.Drawing.Size(215, 39);
            this.chkEnableData.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkEnableData.TabIndex = 4;
            this.chkEnableData.Text = "Enable this Data";
            this.chkEnableData.TextColor = System.Drawing.Color.Black;
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX3.Location = new System.Drawing.Point(13, 48);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(143, 23);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "Loading Data Num";
            // 
            // cmbDataNum
            // 
            this.cmbDataNum.DisplayMember = "Text";
            this.cmbDataNum.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDataNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataNum.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.cmbDataNum.FormattingEnabled = true;
            this.cmbDataNum.ItemHeight = 19;
            this.cmbDataNum.Location = new System.Drawing.Point(167, 48);
            this.cmbDataNum.Name = "cmbDataNum";
            this.cmbDataNum.Size = new System.Drawing.Size(121, 25);
            this.cmbDataNum.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbDataNum.TabIndex = 1;
            this.cmbDataNum.SelectedIndexChanged += new System.EventHandler(this.cmbDataNum_SelectedIndexChanged);
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX4.Location = new System.Drawing.Point(13, 93);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(114, 23);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "Fequency";
            // 
            // grpGeneral
            // 
            this.grpGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.grpGeneral.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpGeneral.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpGeneral.Controls.Add(this.dinLevel);
            this.grpGeneral.Controls.Add(this.labelX11);
            this.grpGeneral.Controls.Add(this.labelX10);
            this.grpGeneral.Controls.Add(this.dinBias);
            this.grpGeneral.Controls.Add(this.labelX9);
            this.grpGeneral.Controls.Add(this.labelX8);
            this.grpGeneral.Controls.Add(this.cmbCable);
            this.grpGeneral.Controls.Add(this.labelX2);
            this.grpGeneral.Controls.Add(this.cmbFunc);
            this.grpGeneral.Controls.Add(this.labelX1);
            this.grpGeneral.DrawTitleBox = false;
            this.grpGeneral.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpGeneral.Location = new System.Drawing.Point(12, 14);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(420, 124);
            // 
            // 
            // 
            this.grpGeneral.Style.BackColor = System.Drawing.SystemColors.Control;
            this.grpGeneral.Style.BackColor2 = System.Drawing.SystemColors.Control;
            this.grpGeneral.Style.BackColorGradientAngle = 90;
            this.grpGeneral.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpGeneral.Style.BorderBottomWidth = 1;
            this.grpGeneral.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpGeneral.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Dash;
            this.grpGeneral.Style.BorderLeftWidth = 1;
            this.grpGeneral.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpGeneral.Style.BorderRightWidth = 1;
            this.grpGeneral.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpGeneral.Style.BorderTopWidth = 1;
            this.grpGeneral.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpGeneral.Style.CornerDiameter = 4;
            this.grpGeneral.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpGeneral.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpGeneral.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpGeneral.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpGeneral.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpGeneral.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpGeneral.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpGeneral.TabIndex = 1;
            this.grpGeneral.Text = "General";
            // 
            // dinLevel
            // 
            // 
            // 
            // 
            this.dinLevel.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLevel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinLevel.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinLevel.DisplayFormat = "0.00000";
            this.dinLevel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinLevel.Increment = 1D;
            this.dinLevel.Location = new System.Drawing.Point(290, 9);
            this.dinLevel.MaxValue = 10D;
            this.dinLevel.MinValue = -10D;
            this.dinLevel.Name = "dinLevel";
            this.dinLevel.ShowUpDown = true;
            this.dinLevel.Size = new System.Drawing.Size(100, 25);
            this.dinLevel.TabIndex = 12;
            // 
            // labelX11
            // 
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX11.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX11.Location = new System.Drawing.Point(215, 10);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(73, 25);
            this.labelX11.TabIndex = 17;
            this.labelX11.Text = "AC Level";
            // 
            // labelX10
            // 
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX10.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX10.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX10.Location = new System.Drawing.Point(395, 55);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(22, 23);
            this.labelX10.TabIndex = 16;
            this.labelX10.Text = "V";
            // 
            // dinBias
            // 
            // 
            // 
            // 
            this.dinBias.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinBias.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinBias.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinBias.DisplayFormat = "0.00000";
            this.dinBias.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinBias.Increment = 1D;
            this.dinBias.Location = new System.Drawing.Point(290, 53);
            this.dinBias.MaxValue = 10D;
            this.dinBias.MinValue = -10D;
            this.dinBias.Name = "dinBias";
            this.dinBias.ShowUpDown = true;
            this.dinBias.Size = new System.Drawing.Size(100, 25);
            this.dinBias.TabIndex = 13;
            // 
            // labelX9
            // 
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX9.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX9.Location = new System.Drawing.Point(215, 53);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(73, 25);
            this.labelX9.TabIndex = 15;
            this.labelX9.Text = "DC Bias";
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX8.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX8.Location = new System.Drawing.Point(395, 10);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(28, 23);
            this.labelX8.TabIndex = 14;
            this.labelX8.Text = "V";
            // 
            // cmbCable
            // 
            this.cmbCable.DisplayMember = "Text";
            this.cmbCable.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCable.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.cmbCable.FormattingEnabled = true;
            this.cmbCable.ItemHeight = 19;
            this.cmbCable.Location = new System.Drawing.Point(109, 53);
            this.cmbCable.Name = "cmbCable";
            this.cmbCable.Size = new System.Drawing.Size(100, 25);
            this.cmbCable.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbCable.TabIndex = 3;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX2.Location = new System.Drawing.Point(3, 53);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(103, 25);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "Cable Length";
            // 
            // cmbFunc
            // 
            this.cmbFunc.DisplayMember = "Text";
            this.cmbFunc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFunc.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.cmbFunc.FormattingEnabled = true;
            this.cmbFunc.ItemHeight = 19;
            this.cmbFunc.Location = new System.Drawing.Point(109, 9);
            this.cmbFunc.Name = "cmbFunc";
            this.cmbFunc.Size = new System.Drawing.Size(100, 25);
            this.cmbFunc.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbFunc.TabIndex = 1;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelX1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelX1.Location = new System.Drawing.Point(6, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 25);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Msrt type";
            // 
            // grpTestCtrl
            // 
            this.grpTestCtrl.BackColor = System.Drawing.SystemColors.Control;
            this.grpTestCtrl.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpTestCtrl.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpTestCtrl.Controls.Add(this.btnload);
            this.grpTestCtrl.Controls.Add(this.btnOpen);
            this.grpTestCtrl.Controls.Add(this.grpEnable);
            this.grpTestCtrl.Controls.Add(this.btnShort);
            this.grpTestCtrl.DrawTitleBox = false;
            this.grpTestCtrl.Font = new System.Drawing.Font("PMingLiU", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grpTestCtrl.Location = new System.Drawing.Point(438, 30);
            this.grpTestCtrl.Name = "grpTestCtrl";
            this.grpTestCtrl.Size = new System.Drawing.Size(172, 438);
            // 
            // 
            // 
            this.grpTestCtrl.Style.BackColor = System.Drawing.SystemColors.Control;
            this.grpTestCtrl.Style.BackColor2 = System.Drawing.SystemColors.Control;
            this.grpTestCtrl.Style.BackColorGradientAngle = 90;
            this.grpTestCtrl.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpTestCtrl.Style.BorderBottomWidth = 1;
            this.grpTestCtrl.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpTestCtrl.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Dash;
            this.grpTestCtrl.Style.BorderLeftWidth = 1;
            this.grpTestCtrl.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpTestCtrl.Style.BorderRightWidth = 1;
            this.grpTestCtrl.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpTestCtrl.Style.BorderTopWidth = 1;
            this.grpTestCtrl.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpTestCtrl.Style.CornerDiameter = 4;
            this.grpTestCtrl.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpTestCtrl.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpTestCtrl.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpTestCtrl.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpTestCtrl.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpTestCtrl.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.grpTestCtrl.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpTestCtrl.TabIndex = 3;
            // 
            // frmLCRCali
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.ClientSize = new System.Drawing.Size(618, 478);
            this.Controls.Add(this.grpTestCtrl);
            this.Controls.Add(this.grpGeneral);
            this.Controls.Add(this.grpCaliData);
            this.Name = "frmLCRCali";
            this.Text = "LCR Calibration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLCRCali_FormClosing);
            this.grpEnable.ResumeLayout(false);
            this.grpCaliData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinRefB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinRefA)).EndInit();
            this.grpGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinBias)).EndInit();
            this.grpTestCtrl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel grpCaliData;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableData;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbDataNum;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.GroupPanel grpGeneral;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCable;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFunc;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.Editors.DoubleInput dinRefA;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRefAUnit;
        private DevComponents.DotNetBar.ButtonX btnSet;
        private DevComponents.Editors.DoubleInput dinRefB;
        private DevComponents.DotNetBar.ButtonX btnOpen;
        private DevComponents.Editors.DoubleInput dinFrequency;
        private DevComponents.DotNetBar.Controls.GroupPanel grpEnable;
        private DevComponents.DotNetBar.ButtonX btnShort;
        private DevComponents.DotNetBar.ButtonX btnload;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkOpen;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkLoad;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkShort;
        private DevComponents.Editors.DoubleInput dinLevel;
        private DevComponents.Editors.DoubleInput dinBias;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.Controls.GroupPanel grpTestCtrl;
        private DevComponents.DotNetBar.LabelX labelX7;


    }
}