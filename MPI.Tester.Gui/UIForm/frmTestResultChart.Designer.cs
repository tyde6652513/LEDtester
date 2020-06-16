namespace MPI.Tester.Gui
{
    partial class frmTestResultChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTestResultChart));
            this.plResult = new System.Windows.Forms.Panel();
            this.tsMain = new DevComponents.DotNetBar.SuperTabStrip();
            this.btnColorMapSetting = new DevComponents.DotNetBar.ButtonX();
            this.btnAutoArrangeMultiMap = new DevComponents.DotNetBar.ButtonX();
            this.chkIsEnableShowData = new System.Windows.Forms.CheckBox();
            this.chkEnableMultiMap = new System.Windows.Forms.CheckBox();
            this.chkIsEnableShowSpectrum = new System.Windows.Forms.CheckBox();
            this.controlContainerItem1 = new DevComponents.DotNetBar.ControlContainerItem();
            this.controlContainerItem6 = new DevComponents.DotNetBar.ControlContainerItem();
            this.controlContainerItem4 = new DevComponents.DotNetBar.ControlContainerItem();
            this.controlContainerItem2 = new DevComponents.DotNetBar.ControlContainerItem();
            this.controlContainerItem3 = new DevComponents.DotNetBar.ControlContainerItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.lblBin = new DevComponents.DotNetBar.LabelX();
            this.lblFailCount = new DevComponents.DotNetBar.LabelX();
            this.lblPassCount = new DevComponents.DotNetBar.LabelX();
            this.lblRowY = new DevComponents.DotNetBar.LabelX();
            this.lblColX = new DevComponents.DotNetBar.LabelX();
            this.lblPassRate = new DevComponents.DotNetBar.LabelX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.lblBaseTaskSheet = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblTask = new System.Windows.Forms.Label();
            this.lblHWFilterSelect = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSWNDFilter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label4 = new System.Windows.Forms.Label();
            this.lblBaseAuthorityTitle = new System.Windows.Forms.Label();
            this.txtWaferID = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLotID = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblBaseTaskSheet2 = new System.Windows.Forms.Label();
            this.lblTestTime = new DevComponents.DotNetBar.LabelX();
            this.lblTestCount = new DevComponents.DotNetBar.LabelX();
            this.superTabStrip2 = new DevComponents.DotNetBar.SuperTabStrip();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.btnTestItem = new DevComponents.DotNetBar.ButtonItem();
            this.btnSwitchResultForm = new DevComponents.DotNetBar.ButtonItem();
            this.btnUISetting = new DevComponents.DotNetBar.ButtonItem();
            this.btnCycleRun = new DevComponents.DotNetBar.ButtonItem();
            this.btnExit = new DevComponents.DotNetBar.ButtonItem();
            this.btnEndAndSave = new DevComponents.DotNetBar.ButtonItem();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.lblRowYData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblColXData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblTestTimeData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblSysStatus = new DevComponents.DotNetBar.LabelItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.labelItem3 = new DevComponents.DotNetBar.LabelItem();
            this.labelItem4 = new DevComponents.DotNetBar.LabelItem();
            this.stsStatus = new DevComponents.DotNetBar.Bar();
            this.lblStatus = new DevComponents.DotNetBar.LabelItem();
            this.lblAuthority = new DevComponents.DotNetBar.LabelItem();
            this.lblUser = new DevComponents.DotNetBar.LabelItem();
            this.lblDateTime = new DevComponents.DotNetBar.LabelItem();
            this.lblTestCountData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblFailCountData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblPassCountData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblPassRateData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblBinData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tsMain)).BeginInit();
            this.tsMain.SuspendLayout();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabStrip2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stsStatus)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plResult
            // 
            this.plResult.BackColor = System.Drawing.Color.WhiteSmoke;
            this.plResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.plResult.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.plResult.Location = new System.Drawing.Point(0, 163);
            this.plResult.Name = "plResult";
            this.plResult.Size = new System.Drawing.Size(1252, 744);
            this.plResult.TabIndex = 5;
            // 
            // tsMain
            // 
            this.tsMain.AutoSelectAttachedControl = false;
            this.tsMain.BackColor = System.Drawing.Color.White;
            this.tsMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            // 
            // 
            // 
            this.tsMain.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.tsMain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tsMain.CloseButtonOnTabsAlwaysDisplayed = false;
            this.tsMain.CloseButtonOnTabsVisible = true;
            this.tsMain.ContainerControlProcessDialogKey = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tsMain.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            // 
            // 
            // 
            this.tsMain.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.tsMain.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.tsMain.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tsMain.ControlBox.MenuBox,
            this.tsMain.ControlBox.CloseBox});
            this.tsMain.Controls.Add(this.btnColorMapSetting);
            this.tsMain.Controls.Add(this.btnAutoArrangeMultiMap);
            this.tsMain.Controls.Add(this.chkIsEnableShowData);
            this.tsMain.Controls.Add(this.chkEnableMultiMap);
            this.tsMain.Controls.Add(this.chkIsEnableShowSpectrum);
            this.tsMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tsMain.Font = new System.Drawing.Font("Arial", 9F);
            this.tsMain.Location = new System.Drawing.Point(0, 111);
            this.tsMain.Name = "tsMain";
            this.tsMain.ReorderTabsEnabled = true;
            this.tsMain.SelectedTabFont = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.tsMain.SelectedTabIndex = -1;
            this.tsMain.Size = new System.Drawing.Size(1252, 52);
            this.tsMain.TabCloseButtonHot = null;
            this.tsMain.TabFont = new System.Drawing.Font("Arial", 11.25F);
            this.tsMain.TabHorizontalSpacing = 12;
            this.tsMain.TabIndex = 419;
            this.tsMain.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.controlContainerItem1,
            this.controlContainerItem6,
            this.controlContainerItem4,
            this.controlContainerItem2,
            this.controlContainerItem3,
            this.buttonItem1});
            this.tsMain.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.WinMediaPlayer12;
            this.tsMain.Text = "superTabStrip1";
            // 
            // btnColorMapSetting
            // 
            this.btnColorMapSetting.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnColorMapSetting.Enabled = false;
            this.btnColorMapSetting.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnColorMapSetting.Image = global::MPI.Tester.Gui.Properties.Resources.btnArrangeWindow;
            this.btnColorMapSetting.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.btnColorMapSetting.Location = new System.Drawing.Point(110, 15);
            this.btnColorMapSetting.Name = "btnColorMapSetting";
            this.btnColorMapSetting.Size = new System.Drawing.Size(110, 20);
            this.btnColorMapSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.btnColorMapSetting.TabIndex = 9;
            this.btnColorMapSetting.TabStop = false;
            this.btnColorMapSetting.Text = "Color Setting";
            this.btnColorMapSetting.Visible = false;
            this.btnColorMapSetting.Click += new System.EventHandler(this.btnColorMapSetting_Click);
            // 
            // btnAutoArrangeMultiMap
            // 
            this.btnAutoArrangeMultiMap.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAutoArrangeMultiMap.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAutoArrangeMultiMap.Image = global::MPI.Tester.Gui.Properties.Resources.btnArrangeWindow;
            this.btnAutoArrangeMultiMap.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.btnAutoArrangeMultiMap.Location = new System.Drawing.Point(2, 15);
            this.btnAutoArrangeMultiMap.Name = "btnAutoArrangeMultiMap";
            this.btnAutoArrangeMultiMap.Size = new System.Drawing.Size(104, 20);
            this.btnAutoArrangeMultiMap.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.btnAutoArrangeMultiMap.TabIndex = 8;
            this.btnAutoArrangeMultiMap.TabStop = false;
            this.btnAutoArrangeMultiMap.Text = "map arrange";
            this.btnAutoArrangeMultiMap.Visible = false;
            this.btnAutoArrangeMultiMap.Click += new System.EventHandler(this.btnAutoArrangeMultiMap_Click);
            // 
            // chkIsEnableShowData
            // 
            this.chkIsEnableShowData.AutoSize = true;
            this.chkIsEnableShowData.BackColor = System.Drawing.Color.Transparent;
            this.chkIsEnableShowData.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkIsEnableShowData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkIsEnableShowData.Location = new System.Drawing.Point(224, 16);
            this.chkIsEnableShowData.Name = "chkIsEnableShowData";
            this.chkIsEnableShowData.Size = new System.Drawing.Size(83, 18);
            this.chkIsEnableShowData.TabIndex = 11;
            this.chkIsEnableShowData.Text = "Show Data";
            this.chkIsEnableShowData.UseVisualStyleBackColor = false;
            this.chkIsEnableShowData.CheckedChanged += new System.EventHandler(this.chkIsEnableShowData_CheckedChanged);
            // 
            // chkEnableMultiMap
            // 
            this.chkEnableMultiMap.AutoSize = true;
            this.chkEnableMultiMap.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableMultiMap.Checked = true;
            this.chkEnableMultiMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableMultiMap.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkEnableMultiMap.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkEnableMultiMap.Location = new System.Drawing.Point(311, 16);
            this.chkEnableMultiMap.Name = "chkEnableMultiMap";
            this.chkEnableMultiMap.Size = new System.Drawing.Size(111, 18);
            this.chkEnableMultiMap.TabIndex = 10;
            this.chkEnableMultiMap.Text = "Multi WaferMap";
            this.chkEnableMultiMap.UseVisualStyleBackColor = false;
            this.chkEnableMultiMap.CheckedChanged += new System.EventHandler(this.chkEnableMultiMap_CheckedChanged);
            // 
            // chkIsEnableShowSpectrum
            // 
            this.chkIsEnableShowSpectrum.AutoSize = true;
            this.chkIsEnableShowSpectrum.BackColor = System.Drawing.Color.Transparent;
            this.chkIsEnableShowSpectrum.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkIsEnableShowSpectrum.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkIsEnableShowSpectrum.Location = new System.Drawing.Point(426, 16);
            this.chkIsEnableShowSpectrum.Name = "chkIsEnableShowSpectrum";
            this.chkIsEnableShowSpectrum.Size = new System.Drawing.Size(80, 18);
            this.chkIsEnableShowSpectrum.TabIndex = 7;
            this.chkIsEnableShowSpectrum.Text = "Spectrum";
            this.chkIsEnableShowSpectrum.UseVisualStyleBackColor = false;
            this.chkIsEnableShowSpectrum.CheckedChanged += new System.EventHandler(this.chkIsEnableShowSpectrum_CheckedChanged);
            // 
            // controlContainerItem1
            // 
            this.controlContainerItem1.AllowItemResize = false;
            this.controlContainerItem1.Control = this.btnAutoArrangeMultiMap;
            this.controlContainerItem1.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem1.Name = "controlContainerItem1";
            // 
            // controlContainerItem6
            // 
            this.controlContainerItem6.AllowItemResize = false;
            this.controlContainerItem6.Control = this.btnColorMapSetting;
            this.controlContainerItem6.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem6.Name = "controlContainerItem6";
            // 
            // controlContainerItem4
            // 
            this.controlContainerItem4.AllowItemResize = false;
            this.controlContainerItem4.Control = this.chkIsEnableShowData;
            this.controlContainerItem4.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem4.Name = "controlContainerItem4";
            // 
            // controlContainerItem2
            // 
            this.controlContainerItem2.AllowItemResize = false;
            this.controlContainerItem2.Control = this.chkEnableMultiMap;
            this.controlContainerItem2.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem2.Name = "controlContainerItem2";
            // 
            // controlContainerItem3
            // 
            this.controlContainerItem3.AllowItemResize = false;
            this.controlContainerItem3.Control = this.chkIsEnableShowSpectrum;
            this.controlContainerItem3.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem3.Name = "controlContainerItem3";
            // 
            // buttonItem1
            // 
            this.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem1.FixedSize = new System.Drawing.Size(68, 50);
            this.buttonItem1.Image = global::MPI.Tester.Gui.Properties.Resources.btnCycleRun_32;
            this.buttonItem1.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.buttonItem1.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItemsExpandWidth = 10;
            this.buttonItem1.Text = "Cycle Test";
            this.buttonItem1.Visible = false;
            // 
            // lblBin
            // 
            this.lblBin.BackColor = System.Drawing.Color.PowderBlue;
            // 
            // 
            // 
            this.lblBin.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblBin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblBin.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblBin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblBin.Location = new System.Drawing.Point(383, 30);
            this.lblBin.Name = "lblBin";
            this.lblBin.Size = new System.Drawing.Size(96, 44);
            this.lblBin.TabIndex = 430;
            this.lblBin.Text = "Bin";
            this.lblBin.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblFailCount
            // 
            this.lblFailCount.BackColor = System.Drawing.Color.PowderBlue;
            // 
            // 
            // 
            this.lblFailCount.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblFailCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFailCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFailCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblFailCount.Location = new System.Drawing.Point(192, 55);
            this.lblFailCount.Name = "lblFailCount";
            this.lblFailCount.Size = new System.Drawing.Size(96, 20);
            this.lblFailCount.TabIndex = 425;
            this.lblFailCount.Text = "NG";
            this.lblFailCount.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblPassCount
            // 
            this.lblPassCount.BackColor = System.Drawing.Color.PowderBlue;
            // 
            // 
            // 
            this.lblPassCount.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblPassCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPassCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblPassCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblPassCount.Location = new System.Drawing.Point(192, 31);
            this.lblPassCount.Name = "lblPassCount";
            this.lblPassCount.Size = new System.Drawing.Size(96, 20);
            this.lblPassCount.TabIndex = 424;
            this.lblPassCount.Text = "Pass";
            this.lblPassCount.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblRowY
            // 
            this.lblRowY.BackColor = System.Drawing.Color.PowderBlue;
            // 
            // 
            // 
            this.lblRowY.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblRowY.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblRowY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRowY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblRowY.Location = new System.Drawing.Point(4, 54);
            this.lblRowY.Name = "lblRowY";
            this.lblRowY.Size = new System.Drawing.Size(95, 20);
            this.lblRowY.TabIndex = 423;
            this.lblRowY.Text = "Row ( Y )";
            this.lblRowY.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblColX
            // 
            this.lblColX.BackColor = System.Drawing.Color.PowderBlue;
            // 
            // 
            // 
            this.lblColX.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblColX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblColX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblColX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblColX.Location = new System.Drawing.Point(4, 30);
            this.lblColX.Name = "lblColX";
            this.lblColX.Size = new System.Drawing.Size(95, 20);
            this.lblColX.TabIndex = 422;
            this.lblColX.Text = "Col ( X )";
            this.lblColX.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblPassRate
            // 
            this.lblPassRate.BackColor = System.Drawing.Color.PowderBlue;
            // 
            // 
            // 
            this.lblPassRate.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblPassRate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPassRate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblPassRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblPassRate.Location = new System.Drawing.Point(383, 7);
            this.lblPassRate.Name = "lblPassRate";
            this.lblPassRate.Size = new System.Drawing.Size(96, 20);
            this.lblPassRate.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblPassRate.TabIndex = 420;
            this.lblPassRate.Text = "Yield";
            this.lblPassRate.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.lblBaseTaskSheet);
            this.panelEx1.Controls.Add(this.lblTask);
            this.panelEx1.Controls.Add(this.lblHWFilterSelect);
            this.panelEx1.Controls.Add(this.txtSWNDFilter);
            this.panelEx1.Controls.Add(this.label4);
            this.panelEx1.Controls.Add(this.lblBaseAuthorityTitle);
            this.panelEx1.Controls.Add(this.txtWaferID);
            this.panelEx1.Controls.Add(this.label3);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.txtLotID);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 52);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1252, 59);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.WhiteSmoke;
            this.panelEx1.Style.BackColor2.Color = System.Drawing.Color.LightBlue;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 17;
            // 
            // lblBaseTaskSheet
            // 
            this.lblBaseTaskSheet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            // 
            // 
            // 
            this.lblBaseTaskSheet.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblBaseTaskSheet.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblBaseTaskSheet.Border.BorderBottomWidth = 1;
            this.lblBaseTaskSheet.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblBaseTaskSheet.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblBaseTaskSheet.Border.BorderLeftWidth = 1;
            this.lblBaseTaskSheet.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblBaseTaskSheet.Border.BorderRightWidth = 1;
            this.lblBaseTaskSheet.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblBaseTaskSheet.Border.BorderTopWidth = 1;
            this.lblBaseTaskSheet.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblBaseTaskSheet.Border.CornerDiameter = 4;
            this.lblBaseTaskSheet.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblBaseTaskSheet.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaseTaskSheet.ForeColor = System.Drawing.Color.DarkRed;
            this.lblBaseTaskSheet.Location = new System.Drawing.Point(411, 8);
            this.lblBaseTaskSheet.Name = "lblBaseTaskSheet";
            this.lblBaseTaskSheet.ReadOnly = true;
            this.lblBaseTaskSheet.Size = new System.Drawing.Size(229, 20);
            this.lblBaseTaskSheet.TabIndex = 443;
            this.lblBaseTaskSheet.Tag = "8";
            this.lblBaseTaskSheet.Text = "Disable";
            this.lblBaseTaskSheet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTask
            // 
            this.lblTask.BackColor = System.Drawing.Color.Transparent;
            this.lblTask.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTask.ForeColor = System.Drawing.Color.Black;
            this.lblTask.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTask.Location = new System.Drawing.Point(344, 8);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(79, 22);
            this.lblTask.TabIndex = 426;
            this.lblTask.Text = "Recipe.";
            this.lblTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHWFilterSelect
            // 
            this.lblHWFilterSelect.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.lblHWFilterSelect.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblHWFilterSelect.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblHWFilterSelect.Border.BorderBottomWidth = 1;
            this.lblHWFilterSelect.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblHWFilterSelect.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblHWFilterSelect.Border.BorderLeftWidth = 1;
            this.lblHWFilterSelect.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblHWFilterSelect.Border.BorderRightWidth = 1;
            this.lblHWFilterSelect.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblHWFilterSelect.Border.BorderTopWidth = 1;
            this.lblHWFilterSelect.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblHWFilterSelect.Border.CornerDiameter = 4;
            this.lblHWFilterSelect.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblHWFilterSelect.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblHWFilterSelect.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblHWFilterSelect.Location = new System.Drawing.Point(518, 32);
            this.lblHWFilterSelect.Name = "lblHWFilterSelect";
            this.lblHWFilterSelect.ReadOnly = true;
            this.lblHWFilterSelect.Size = new System.Drawing.Size(74, 20);
            this.lblHWFilterSelect.TabIndex = 425;
            this.lblHWFilterSelect.Tag = "8";
            this.lblHWFilterSelect.Text = "Disable";
            this.lblHWFilterSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSWNDFilter
            // 
            this.txtSWNDFilter.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtSWNDFilter.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtSWNDFilter.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtSWNDFilter.Border.BorderBottomWidth = 1;
            this.txtSWNDFilter.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.txtSWNDFilter.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtSWNDFilter.Border.BorderLeftWidth = 1;
            this.txtSWNDFilter.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtSWNDFilter.Border.BorderRightWidth = 1;
            this.txtSWNDFilter.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtSWNDFilter.Border.BorderTopWidth = 1;
            this.txtSWNDFilter.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.txtSWNDFilter.Border.CornerDiameter = 4;
            this.txtSWNDFilter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSWNDFilter.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtSWNDFilter.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSWNDFilter.Location = new System.Drawing.Point(411, 32);
            this.txtSWNDFilter.Name = "txtSWNDFilter";
            this.txtSWNDFilter.ReadOnly = true;
            this.txtSWNDFilter.Size = new System.Drawing.Size(50, 20);
            this.txtSWNDFilter.TabIndex = 424;
            this.txtSWNDFilter.Tag = "8";
            this.txtSWNDFilter.Text = "-1";
            this.txtSWNDFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(467, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 22);
            this.label4.TabIndex = 422;
            this.label4.Text = "HW :  ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBaseAuthorityTitle
            // 
            this.lblBaseAuthorityTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblBaseAuthorityTitle.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblBaseAuthorityTitle.ForeColor = System.Drawing.Color.Black;
            this.lblBaseAuthorityTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblBaseAuthorityTitle.Location = new System.Drawing.Point(345, 31);
            this.lblBaseAuthorityTitle.Name = "lblBaseAuthorityTitle";
            this.lblBaseAuthorityTitle.Size = new System.Drawing.Size(73, 22);
            this.lblBaseAuthorityTitle.TabIndex = 423;
            this.lblBaseAuthorityTitle.Text = "ND Filter";
            this.lblBaseAuthorityTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWaferID
            // 
            this.txtWaferID.BackColor = System.Drawing.Color.WhiteSmoke;
            // 
            // 
            // 
            this.txtWaferID.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtWaferID.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtWaferID.Border.BorderBottomWidth = 1;
            this.txtWaferID.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.txtWaferID.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtWaferID.Border.BorderLeftWidth = 1;
            this.txtWaferID.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtWaferID.Border.BorderRightWidth = 1;
            this.txtWaferID.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtWaferID.Border.BorderTopWidth = 1;
            this.txtWaferID.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.txtWaferID.Border.CornerDiameter = 4;
            this.txtWaferID.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtWaferID.Font = new System.Drawing.Font("Arial", 11.25F);
            this.txtWaferID.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWaferID.Location = new System.Drawing.Point(76, 33);
            this.txtWaferID.Name = "txtWaferID";
            this.txtWaferID.ReadOnly = true;
            this.txtWaferID.Size = new System.Drawing.Size(263, 20);
            this.txtWaferID.TabIndex = 179;
            this.txtWaferID.Tag = "8";
            this.txtWaferID.Text = "123456789abcdefghijklno";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(11, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 22);
            this.label3.TabIndex = 177;
            this.label3.Text = "Lot ID  : ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(2, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 22);
            this.label1.TabIndex = 175;
            this.label1.Text = "Wafer ID :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtLotID
            // 
            this.txtLotID.BackColor = System.Drawing.Color.WhiteSmoke;
            // 
            // 
            // 
            this.txtLotID.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtLotID.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtLotID.Border.BorderBottomWidth = 1;
            this.txtLotID.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.txtLotID.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtLotID.Border.BorderLeftWidth = 1;
            this.txtLotID.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtLotID.Border.BorderRightWidth = 1;
            this.txtLotID.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtLotID.Border.BorderTopWidth = 1;
            this.txtLotID.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.txtLotID.Border.CornerDiameter = 4;
            this.txtLotID.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtLotID.Font = new System.Drawing.Font("Arial", 11.25F);
            this.txtLotID.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLotID.Location = new System.Drawing.Point(77, 9);
            this.txtLotID.Name = "txtLotID";
            this.txtLotID.ReadOnly = true;
            this.txtLotID.Size = new System.Drawing.Size(263, 20);
            this.txtLotID.TabIndex = 178;
            this.txtLotID.Tag = "8";
            this.txtLotID.Text = "123456789abcdefghijklno";
            // 
            // lblBaseTaskSheet2
            // 
            this.lblBaseTaskSheet2.BackColor = System.Drawing.Color.Transparent;
            this.lblBaseTaskSheet2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblBaseTaskSheet2.ForeColor = System.Drawing.Color.Black;
            this.lblBaseTaskSheet2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblBaseTaskSheet2.Location = new System.Drawing.Point(521, 14);
            this.lblBaseTaskSheet2.Name = "lblBaseTaskSheet2";
            this.lblBaseTaskSheet2.Size = new System.Drawing.Size(76, 22);
            this.lblBaseTaskSheet2.TabIndex = 0;
            this.lblBaseTaskSheet2.Text = "Recipe :";
            this.lblBaseTaskSheet2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTestTime
            // 
            this.lblTestTime.BackColor = System.Drawing.Color.PowderBlue;
            // 
            // 
            // 
            this.lblTestTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblTestTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTestTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTestTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblTestTime.Location = new System.Drawing.Point(4, 6);
            this.lblTestTime.Name = "lblTestTime";
            this.lblTestTime.Size = new System.Drawing.Size(95, 20);
            this.lblTestTime.TabIndex = 433;
            this.lblTestTime.Text = "Cycle Time";
            this.lblTestTime.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblTestCount
            // 
            this.lblTestCount.BackColor = System.Drawing.Color.PowderBlue;
            // 
            // 
            // 
            this.lblTestCount.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblTestCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTestCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTestCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblTestCount.Location = new System.Drawing.Point(192, 6);
            this.lblTestCount.Name = "lblTestCount";
            this.lblTestCount.Size = new System.Drawing.Size(96, 20);
            this.lblTestCount.TabIndex = 435;
            this.lblTestCount.Text = "Total Count";
            this.lblTestCount.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // superTabStrip2
            // 
            this.superTabStrip2.AutoSelectAttachedControl = false;
            this.superTabStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            // 
            // 
            // 
            this.superTabStrip2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.superTabStrip2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.superTabStrip2.CloseButtonOnTabsAlwaysDisplayed = false;
            this.superTabStrip2.CloseButtonOnTabsVisible = true;
            this.superTabStrip2.ContainerControlProcessDialogKey = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabStrip2.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            // 
            // 
            // 
            this.superTabStrip2.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.superTabStrip2.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.superTabStrip2.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabStrip2.ControlBox.MenuBox,
            this.superTabStrip2.ControlBox.CloseBox});
            this.superTabStrip2.Dock = System.Windows.Forms.DockStyle.Top;
            this.superTabStrip2.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold);
            this.superTabStrip2.Location = new System.Drawing.Point(0, 0);
            this.superTabStrip2.Name = "superTabStrip2";
            this.superTabStrip2.ReorderTabsEnabled = true;
            this.superTabStrip2.SelectedTabFont = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold);
            this.superTabStrip2.SelectedTabIndex = -1;
            this.superTabStrip2.Size = new System.Drawing.Size(1252, 52);
            this.superTabStrip2.TabCloseButtonHot = null;
            this.superTabStrip2.TabFont = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold);
            this.superTabStrip2.TabHorizontalSpacing = 12;
            this.superTabStrip2.TabIndex = 395;
            this.superTabStrip2.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.btnTestItem,
            this.btnSwitchResultForm,
            this.btnUISetting,
            this.btnCycleRun,
            this.btnExit});
            this.superTabStrip2.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.WinMediaPlayer12;
            this.superTabStrip2.Text = "superTabStrip2";
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            // 
            // btnTestItem
            // 
            this.btnTestItem.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnTestItem.FixedSize = new System.Drawing.Size(68, 50);
            this.btnTestItem.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestItem_32;
            this.btnTestItem.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.btnTestItem.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnTestItem.Name = "btnTestItem";
            this.btnTestItem.Text = "Test Cond.";
            this.btnTestItem.Click += new System.EventHandler(this.btnTestItem_Click);
            // 
            // btnSwitchResultForm
            // 
            this.btnSwitchResultForm.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnSwitchResultForm.FixedSize = new System.Drawing.Size(68, 50);
            this.btnSwitchResultForm.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestResult_32;
            this.btnSwitchResultForm.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.btnSwitchResultForm.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnSwitchResultForm.Name = "btnSwitchResultForm";
            this.btnSwitchResultForm.Text = "Test Result";
            this.btnSwitchResultForm.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnUISetting
            // 
            this.btnUISetting.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnUISetting.FixedSize = new System.Drawing.Size(68, 50);
            this.btnUISetting.Image = global::MPI.Tester.Gui.Properties.Resources.btnSetting_32;
            this.btnUISetting.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.btnUISetting.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnUISetting.Name = "btnUISetting";
            this.btnUISetting.Text = "Setting";
            this.btnUISetting.Click += new System.EventHandler(this.btnUISetting_Click);
            // 
            // btnCycleRun
            // 
            this.btnCycleRun.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnCycleRun.FixedSize = new System.Drawing.Size(68, 50);
            this.btnCycleRun.Image = global::MPI.Tester.Gui.Properties.Resources.btnCycleRun_32;
            this.btnCycleRun.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.btnCycleRun.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnCycleRun.Name = "btnCycleRun";
            this.btnCycleRun.SubItemsExpandWidth = 10;
            this.btnCycleRun.Text = "Single Test";
            this.btnCycleRun.Click += new System.EventHandler(this.btnCycleRun_Click);
            // 
            // btnExit
            // 
            this.btnExit.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnExit.FixedSize = new System.Drawing.Size(68, 50);
            this.btnExit.Image = global::MPI.Tester.Gui.Properties.Resources.btnExitApp_32;
            this.btnExit.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.btnExit.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnExit.Name = "btnExit";
            this.btnExit.SubItemsExpandWidth = 10;
            this.btnExit.Text = "Exit";
            this.btnExit.Visible = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnEndAndSave
            // 
            this.btnEndAndSave.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnEndAndSave.FixedSize = new System.Drawing.Size(68, 50);
            this.btnEndAndSave.Image = global::MPI.Tester.Gui.Properties.Resources.btnBlueStop_32;
            this.btnEndAndSave.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.btnEndAndSave.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnEndAndSave.Name = "btnEndAndSave";
            this.btnEndAndSave.Text = "End";
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 300;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // lblRowYData
            // 
            this.lblRowYData.BackColor = System.Drawing.Color.AliceBlue;
            // 
            // 
            // 
            this.lblRowYData.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblRowYData.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblRowYData.Border.BorderBottomWidth = 1;
            this.lblRowYData.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblRowYData.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblRowYData.Border.BorderLeftWidth = 1;
            this.lblRowYData.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblRowYData.Border.BorderRightWidth = 1;
            this.lblRowYData.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblRowYData.Border.BorderTopWidth = 1;
            this.lblRowYData.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblRowYData.Border.CornerDiameter = 4;
            this.lblRowYData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblRowYData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRowYData.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblRowYData.Location = new System.Drawing.Point(103, 54);
            this.lblRowYData.Name = "lblRowYData";
            this.lblRowYData.ReadOnly = true;
            this.lblRowYData.Size = new System.Drawing.Size(83, 21);
            this.lblRowYData.TabIndex = 436;
            this.lblRowYData.Tag = "8";
            this.lblRowYData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblColXData
            // 
            this.lblColXData.BackColor = System.Drawing.Color.AliceBlue;
            // 
            // 
            // 
            this.lblColXData.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblColXData.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblColXData.Border.BorderBottomWidth = 1;
            this.lblColXData.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblColXData.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblColXData.Border.BorderLeftWidth = 1;
            this.lblColXData.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblColXData.Border.BorderRightWidth = 1;
            this.lblColXData.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblColXData.Border.BorderTopWidth = 1;
            this.lblColXData.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblColXData.Border.CornerDiameter = 4;
            this.lblColXData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblColXData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColXData.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblColXData.Location = new System.Drawing.Point(103, 30);
            this.lblColXData.Name = "lblColXData";
            this.lblColXData.ReadOnly = true;
            this.lblColXData.Size = new System.Drawing.Size(83, 21);
            this.lblColXData.TabIndex = 437;
            this.lblColXData.Tag = "8";
            this.lblColXData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTestTimeData
            // 
            this.lblTestTimeData.BackColor = System.Drawing.Color.AliceBlue;
            // 
            // 
            // 
            this.lblTestTimeData.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblTestTimeData.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblTestTimeData.Border.BorderBottomWidth = 1;
            this.lblTestTimeData.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblTestTimeData.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblTestTimeData.Border.BorderLeftWidth = 1;
            this.lblTestTimeData.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblTestTimeData.Border.BorderRightWidth = 1;
            this.lblTestTimeData.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblTestTimeData.Border.BorderTopWidth = 1;
            this.lblTestTimeData.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblTestTimeData.Border.CornerDiameter = 4;
            this.lblTestTimeData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTestTimeData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestTimeData.ForeColor = System.Drawing.Color.Red;
            this.lblTestTimeData.Location = new System.Drawing.Point(103, 6);
            this.lblTestTimeData.Name = "lblTestTimeData";
            this.lblTestTimeData.ReadOnly = true;
            this.lblTestTimeData.Size = new System.Drawing.Size(83, 21);
            this.lblTestTimeData.TabIndex = 438;
            this.lblTestTimeData.Tag = "8";
            this.lblTestTimeData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSysStatus
            // 
            this.lblSysStatus.AutoCollapseOnClick = false;
            this.lblSysStatus.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblSysStatus.EnableMarkup = false;
            this.lblSysStatus.Name = "lblSysStatus";
            this.lblSysStatus.ShowSubItems = false;
            this.lblSysStatus.SingleLineColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblSysStatus.Text = "Status";
            this.lblSysStatus.Width = 120;
            // 
            // labelItem2
            // 
            this.labelItem2.AutoCollapseOnClick = false;
            this.labelItem2.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.labelItem2.EnableMarkup = false;
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.ShowSubItems = false;
            this.labelItem2.SingleLineColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelItem2.Text = "Status";
            this.labelItem2.Width = 120;
            // 
            // labelItem3
            // 
            this.labelItem3.AutoCollapseOnClick = false;
            this.labelItem3.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.labelItem3.EnableMarkup = false;
            this.labelItem3.Name = "labelItem3";
            this.labelItem3.ShowSubItems = false;
            this.labelItem3.SingleLineColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelItem3.Text = "Status";
            this.labelItem3.Width = 120;
            // 
            // labelItem4
            // 
            this.labelItem4.AutoCollapseOnClick = false;
            this.labelItem4.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.labelItem4.EnableMarkup = false;
            this.labelItem4.Name = "labelItem4";
            this.labelItem4.ShowSubItems = false;
            this.labelItem4.SingleLineColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelItem4.Text = "Status";
            this.labelItem4.Width = 120;
            // 
            // stsStatus
            // 
            this.stsStatus.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.stsStatus.AntiAlias = true;
            this.stsStatus.AutoCreateCaptionMenu = false;
            this.stsStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.stsStatus.BarType = DevComponents.DotNetBar.eBarType.StatusBar;
            this.stsStatus.CanAutoHide = false;
            this.stsStatus.CanDockBottom = false;
            this.stsStatus.CanDockLeft = false;
            this.stsStatus.CanDockRight = false;
            this.stsStatus.CanDockTab = false;
            this.stsStatus.CanDockTop = false;
            this.stsStatus.CanReorderTabs = false;
            this.stsStatus.CanUndock = false;
            this.stsStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stsStatus.DockedBorderStyle = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.stsStatus.FadeEffect = true;
            this.stsStatus.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.stsStatus.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.lblStatus,
            this.lblAuthority,
            this.lblUser,
            this.lblDateTime});
            this.stsStatus.ItemSpacing = 2;
            this.stsStatus.Location = new System.Drawing.Point(0, 934);
            this.stsStatus.Margin = new System.Windows.Forms.Padding(0);
            this.stsStatus.Name = "stsStatus";
            this.stsStatus.PaddingLeft = 5;
            this.stsStatus.PaddingRight = 0;
            this.stsStatus.PaddingTop = 0;
            this.stsStatus.RoundCorners = false;
            this.stsStatus.SaveLayoutChanges = false;
            this.stsStatus.SingleLineColor = System.Drawing.SystemColors.ControlDarkDark;
            this.stsStatus.Size = new System.Drawing.Size(1252, 27);
            this.stsStatus.Stretch = true;
            this.stsStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.stsStatus.TabIndex = 439;
            this.stsStatus.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoCollapseOnClick = false;
            this.lblStatus.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblStatus.EnableMarkup = false;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.ShowSubItems = false;
            this.lblStatus.SingleLineColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblStatus.Text = "Status";
            this.lblStatus.Width = 120;
            // 
            // lblAuthority
            // 
            this.lblAuthority.AutoCollapseOnClick = false;
            this.lblAuthority.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblAuthority.EnableMarkup = false;
            this.lblAuthority.ForeColor = System.Drawing.Color.Black;
            this.lblAuthority.Name = "lblAuthority";
            this.lblAuthority.ShowSubItems = false;
            this.lblAuthority.SingleLineColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblAuthority.Text = "Authority";
            this.lblAuthority.Width = 120;
            // 
            // lblUser
            // 
            this.lblUser.AutoCollapseOnClick = false;
            this.lblUser.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblUser.EnableMarkup = false;
            this.lblUser.Name = "lblUser";
            this.lblUser.ShowSubItems = false;
            this.lblUser.Text = "User";
            this.lblUser.Width = 120;
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoCollapseOnClick = false;
            this.lblDateTime.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblDateTime.EnableMarkup = false;
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.ShowSubItems = false;
            this.lblDateTime.Text = "DateTime";
            this.lblDateTime.Width = 250;
            // 
            // lblTestCountData
            // 
            this.lblTestCountData.BackColor = System.Drawing.Color.AliceBlue;
            // 
            // 
            // 
            this.lblTestCountData.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblTestCountData.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblTestCountData.Border.BorderBottomWidth = 1;
            this.lblTestCountData.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblTestCountData.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblTestCountData.Border.BorderLeftWidth = 1;
            this.lblTestCountData.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblTestCountData.Border.BorderRightWidth = 1;
            this.lblTestCountData.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblTestCountData.Border.BorderTopWidth = 1;
            this.lblTestCountData.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblTestCountData.Border.CornerDiameter = 4;
            this.lblTestCountData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTestCountData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestCountData.ForeColor = System.Drawing.Color.Black;
            this.lblTestCountData.Location = new System.Drawing.Point(294, 6);
            this.lblTestCountData.Name = "lblTestCountData";
            this.lblTestCountData.ReadOnly = true;
            this.lblTestCountData.Size = new System.Drawing.Size(83, 21);
            this.lblTestCountData.TabIndex = 440;
            this.lblTestCountData.Tag = "8";
            this.lblTestCountData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFailCountData
            // 
            this.lblFailCountData.BackColor = System.Drawing.Color.AliceBlue;
            // 
            // 
            // 
            this.lblFailCountData.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblFailCountData.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblFailCountData.Border.BorderBottomWidth = 1;
            this.lblFailCountData.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblFailCountData.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblFailCountData.Border.BorderLeftWidth = 1;
            this.lblFailCountData.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblFailCountData.Border.BorderRightWidth = 1;
            this.lblFailCountData.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblFailCountData.Border.BorderTopWidth = 1;
            this.lblFailCountData.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblFailCountData.Border.CornerDiameter = 4;
            this.lblFailCountData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFailCountData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFailCountData.ForeColor = System.Drawing.Color.Red;
            this.lblFailCountData.Location = new System.Drawing.Point(294, 55);
            this.lblFailCountData.Name = "lblFailCountData";
            this.lblFailCountData.ReadOnly = true;
            this.lblFailCountData.Size = new System.Drawing.Size(83, 21);
            this.lblFailCountData.TabIndex = 441;
            this.lblFailCountData.Tag = "8";
            this.lblFailCountData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPassCountData
            // 
            this.lblPassCountData.BackColor = System.Drawing.Color.AliceBlue;
            // 
            // 
            // 
            this.lblPassCountData.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblPassCountData.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPassCountData.Border.BorderBottomWidth = 1;
            this.lblPassCountData.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblPassCountData.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPassCountData.Border.BorderLeftWidth = 1;
            this.lblPassCountData.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPassCountData.Border.BorderRightWidth = 1;
            this.lblPassCountData.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPassCountData.Border.BorderTopWidth = 1;
            this.lblPassCountData.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblPassCountData.Border.CornerDiameter = 4;
            this.lblPassCountData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPassCountData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassCountData.ForeColor = System.Drawing.Color.Green;
            this.lblPassCountData.Location = new System.Drawing.Point(294, 30);
            this.lblPassCountData.Name = "lblPassCountData";
            this.lblPassCountData.ReadOnly = true;
            this.lblPassCountData.Size = new System.Drawing.Size(83, 21);
            this.lblPassCountData.TabIndex = 442;
            this.lblPassCountData.Tag = "8";
            this.lblPassCountData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPassRateData
            // 
            this.lblPassRateData.BackColor = System.Drawing.Color.AliceBlue;
            // 
            // 
            // 
            this.lblPassRateData.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblPassRateData.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPassRateData.Border.BorderBottomWidth = 1;
            this.lblPassRateData.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblPassRateData.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPassRateData.Border.BorderLeftWidth = 1;
            this.lblPassRateData.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPassRateData.Border.BorderRightWidth = 1;
            this.lblPassRateData.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPassRateData.Border.BorderTopWidth = 1;
            this.lblPassRateData.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblPassRateData.Border.CornerDiameter = 4;
            this.lblPassRateData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPassRateData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassRateData.ForeColor = System.Drawing.Color.Green;
            this.lblPassRateData.Location = new System.Drawing.Point(482, 7);
            this.lblPassRateData.Name = "lblPassRateData";
            this.lblPassRateData.ReadOnly = true;
            this.lblPassRateData.Size = new System.Drawing.Size(106, 21);
            this.lblPassRateData.TabIndex = 443;
            this.lblPassRateData.Tag = "8";
            this.lblPassRateData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblBinData
            // 
            this.lblBinData.BackColor = System.Drawing.Color.AliceBlue;
            // 
            // 
            // 
            this.lblBinData.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblBinData.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblBinData.Border.BorderBottomWidth = 1;
            this.lblBinData.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.lblBinData.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblBinData.Border.BorderLeftWidth = 1;
            this.lblBinData.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblBinData.Border.BorderRightWidth = 1;
            this.lblBinData.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblBinData.Border.BorderTopWidth = 1;
            this.lblBinData.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblBinData.Border.CornerDiameter = 4;
            this.lblBinData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblBinData.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBinData.ForeColor = System.Drawing.Color.Orange;
            this.lblBinData.Location = new System.Drawing.Point(482, 31);
            this.lblBinData.Name = "lblBinData";
            this.lblBinData.ReadOnly = true;
            this.lblBinData.Size = new System.Drawing.Size(106, 44);
            this.lblBinData.TabIndex = 444;
            this.lblBinData.Tag = "8";
            this.lblBinData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTestTime);
            this.panel1.Controls.Add(this.lblPassRate);
            this.panel1.Controls.Add(this.lblColX);
            this.panel1.Controls.Add(this.lblRowY);
            this.panel1.Controls.Add(this.lblBinData);
            this.panel1.Controls.Add(this.lblPassCount);
            this.panel1.Controls.Add(this.lblPassRateData);
            this.panel1.Controls.Add(this.lblFailCount);
            this.panel1.Controls.Add(this.lblPassCountData);
            this.panel1.Controls.Add(this.lblBin);
            this.panel1.Controls.Add(this.lblFailCountData);
            this.panel1.Controls.Add(this.lblRowYData);
            this.panel1.Controls.Add(this.lblTestCountData);
            this.panel1.Controls.Add(this.lblColXData);
            this.panel1.Controls.Add(this.lblTestTimeData);
            this.panel1.Controls.Add(this.lblTestCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 856);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1252, 78);
            this.panel1.TabIndex = 445;
            // 
            // frmTestResultChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1252, 961);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.stsStatus);
            this.Controls.Add(this.plResult);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.superTabStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmTestResultChart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LEDTester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPopResult_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPopResult_FormClosed);
            this.Load += new System.EventHandler(this.frmPopResult_Load);
            this.SizeChanged += new System.EventHandler(this.frmTestResultChart_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.frmTestresultChart_VisbkeChange);
            ((System.ComponentModel.ISupportInitialize)(this.tsMain)).EndInit();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabStrip2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stsStatus)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plResult;
        private DevComponents.DotNetBar.SuperTabStrip tsMain;
        private DevComponents.DotNetBar.ButtonX btnAutoArrangeMultiMap;
        private System.Windows.Forms.CheckBox chkIsEnableShowSpectrum;
        private System.Windows.Forms.CheckBox chkEnableMultiMap;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem1;
        private System.Windows.Forms.CheckBox chkIsEnableShowData;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem4;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem2;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem3;
        private DevComponents.DotNetBar.ButtonX btnColorMapSetting;
		  private DevComponents.DotNetBar.ControlContainerItem controlContainerItem6;
        private DevComponents.DotNetBar.LabelX lblBin;
        private DevComponents.DotNetBar.LabelX lblFailCount;
        private DevComponents.DotNetBar.LabelX lblPassCount;
        private DevComponents.DotNetBar.LabelX lblRowY;
		  private DevComponents.DotNetBar.LabelX lblColX;
        private DevComponents.DotNetBar.LabelX lblPassRate;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.Label lblBaseTaskSheet2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtWaferID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLotID;
        private DevComponents.DotNetBar.LabelX lblTestTime;
        private DevComponents.DotNetBar.LabelX lblTestCount;
        private DevComponents.DotNetBar.SuperTabStrip superTabStrip2;
        private DevComponents.DotNetBar.ButtonItem btnCycleRun;
        private DevComponents.DotNetBar.ButtonItem btnSwitchResultForm;
        private DevComponents.DotNetBar.ButtonItem btnEndAndSave;
        private System.Windows.Forms.Timer tmrUpdate;
        private DevComponents.DotNetBar.Controls.TextBoxX lblRowYData;
        private DevComponents.DotNetBar.Controls.TextBoxX lblColXData;
        private DevComponents.DotNetBar.Controls.TextBoxX lblTestTimeData;
        private DevComponents.DotNetBar.ButtonItem btnTestItem;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.Controls.TextBoxX lblHWFilterSelect;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSWNDFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblBaseAuthorityTitle;
        private DevComponents.DotNetBar.ButtonItem btnUISetting;
        private DevComponents.DotNetBar.LabelItem lblSysStatus;
        private DevComponents.DotNetBar.LabelItem labelItem2;
		  private DevComponents.DotNetBar.LabelItem labelItem3;
		  private DevComponents.DotNetBar.LabelItem labelItem4;
		  private DevComponents.DotNetBar.Bar stsStatus;
		  private DevComponents.DotNetBar.LabelItem lblStatus;
		  private DevComponents.DotNetBar.LabelItem lblAuthority;
		  private DevComponents.DotNetBar.LabelItem lblUser;
		  private DevComponents.DotNetBar.LabelItem lblDateTime;
          private DevComponents.DotNetBar.Controls.TextBoxX lblTestCountData;
          private DevComponents.DotNetBar.Controls.TextBoxX lblFailCountData;
          private DevComponents.DotNetBar.Controls.TextBoxX lblPassCountData;
          private System.Windows.Forms.Label lblTask;
          private DevComponents.DotNetBar.Controls.TextBoxX lblBaseTaskSheet;
          private DevComponents.DotNetBar.ButtonItem btnExit;
			 private DevComponents.DotNetBar.Controls.TextBoxX lblPassRateData;
			 private DevComponents.DotNetBar.Controls.TextBoxX lblBinData;
             private DevComponents.DotNetBar.ButtonItem buttonItem1;
             private System.Windows.Forms.Panel panel1;

    }
}