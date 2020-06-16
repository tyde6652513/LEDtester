namespace MPI.Tester.Gui
{
	partial class frmBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBase));
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.stsStatus = new DevComponents.DotNetBar.Bar();
            this.lblSysStatus = new DevComponents.DotNetBar.LabelItem();
            this.lblAuthority = new DevComponents.DotNetBar.LabelItem();
            this.lblUser = new DevComponents.DotNetBar.LabelItem();
            this.lblTaskSheet = new DevComponents.DotNetBar.LabelItem();
            this.lblCode = new DevComponents.DotNetBar.LabelItem();
            this.lblDateTime = new DevComponents.DotNetBar.LabelItem();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.tabcMenu = new DevComponents.DotNetBar.SuperTabControl();
            this.tabpCondition = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.tabsSubMenu03 = new DevComponents.DotNetBar.TabStrip();
            this.tabItem3 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabItem4 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabCondition = new DevComponents.DotNetBar.SuperTabItem();
            this.tabpSetting = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.tabsSubMenu02 = new DevComponents.DotNetBar.TabStrip();
            this.tabSetting = new DevComponents.DotNetBar.SuperTabItem();
            this.tabpResult = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.tabsSubMenu05 = new DevComponents.DotNetBar.TabStrip();
            this.tabResult = new DevComponents.DotNetBar.SuperTabItem();
            this.pnlLogo = new DevComponents.DotNetBar.PanelEx();
            this.lblTesterMode = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.plRecipeCtrl = new System.Windows.Forms.Panel();
            this.plCondShow = new System.Windows.Forms.Panel();
            this.btnCreateTask = new DevComponents.DotNetBar.ButtonX();
            this.btnDelete = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveAs = new DevComponents.DotNetBar.ButtonX();
            this.lblRecipeFileName = new DevComponents.DotNetBar.LabelX();
            this.lblFilterPosition = new System.Windows.Forms.Label();
            this.lblHWFilterSelect = new System.Windows.Forms.Label();
            this.cmbTaskSheet = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.btnLoadTask = new DevComponents.DotNetBar.ButtonX();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.btnArrangeWindow = new DevComponents.DotNetBar.ButtonX();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.lblHWFilterWheel = new System.Windows.Forms.Label();
            this.lblSWFilterWheel = new System.Windows.Forms.Label();
            this.barLeft = new DevComponents.DotNetBar.Bar();
            this.btnCloseApp = new DevComponents.DotNetBar.ButtonX();
            this.btnExitApp = new DevComponents.DotNetBar.ButtonX();
            this.btnAlarmLog = new DevComponents.DotNetBar.ButtonX();
            this.btnContinousProbing = new DevComponents.DotNetBar.ButtonX();
            this.btnResetState = new DevComponents.DotNetBar.ButtonX();
            this.btnUserMgr = new DevComponents.DotNetBar.ButtonX();
            this.btnReSingleTest = new DevComponents.DotNetBar.ButtonX();
            this.btnGoProberUI = new DevComponents.DotNetBar.ButtonX();
            this.btnLogin = new DevComponents.DotNetBar.ButtonX();
            this.btnEndAndSave = new DevComponents.DotNetBar.ButtonX();
            this.btnCycleRun = new DevComponents.DotNetBar.ButtonX();
            this.btnStartAndOpen = new DevComponents.DotNetBar.ButtonX();
            this.btnCIEMap = new DevComponents.DotNetBar.ButtonX();
            this.btnWaferMap = new DevComponents.DotNetBar.ButtonX();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.controlContainerItem1 = new DevComponents.DotNetBar.ControlContainerItem();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.stsStatus)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabcMenu)).BeginInit();
            this.tabcMenu.SuspendLayout();
            this.tabpCondition.SuspendLayout();
            this.tabpSetting.SuspendLayout();
            this.tabpResult.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.plRecipeCtrl.SuspendLayout();
            this.plCondShow.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barLeft)).BeginInit();
            this.barLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 500;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // stsStatus
            // 
            this.stsStatus.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.stsStatus.AntiAlias = true;
            this.stsStatus.AutoCreateCaptionMenu = false;
            this.stsStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(237)))), ((int)(((byte)(223)))));
            this.stsStatus.BarType = DevComponents.DotNetBar.eBarType.StatusBar;
            this.stsStatus.CanAutoHide = false;
            this.stsStatus.CanDockBottom = false;
            this.stsStatus.CanDockLeft = false;
            this.stsStatus.CanDockRight = false;
            this.stsStatus.CanDockTab = false;
            this.stsStatus.CanDockTop = false;
            this.stsStatus.CanReorderTabs = false;
            this.stsStatus.CanUndock = false;
            resources.ApplyResources(this.stsStatus, "stsStatus");
            this.stsStatus.DockedBorderStyle = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.stsStatus.FadeEffect = true;
            this.stsStatus.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.lblSysStatus,
            this.lblAuthority,
            this.lblUser,
            this.lblTaskSheet,
            this.lblCode,
            this.lblDateTime});
            this.stsStatus.ItemSpacing = 2;
            this.stsStatus.Name = "stsStatus";
            this.stsStatus.PaddingLeft = 5;
            this.stsStatus.PaddingRight = 0;
            this.stsStatus.PaddingTop = 0;
            this.stsStatus.RoundCorners = false;
            this.stsStatus.SaveLayoutChanges = false;
            this.stsStatus.SingleLineColor = System.Drawing.SystemColors.ControlDarkDark;
            this.stsStatus.Stretch = true;
            this.stsStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.stsStatus.TabStop = false;
            // 
            // lblSysStatus
            // 
            this.lblSysStatus.AutoCollapseOnClick = false;
            this.lblSysStatus.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblSysStatus.EnableMarkup = false;
            this.lblSysStatus.Name = "lblSysStatus";
            this.lblSysStatus.ShowSubItems = false;
            this.lblSysStatus.SingleLineColor = System.Drawing.SystemColors.ControlDarkDark;
            resources.ApplyResources(this.lblSysStatus, "lblSysStatus");
            this.lblSysStatus.Width = 120;
            // 
            // lblAuthority
            // 
            this.lblAuthority.AutoCollapseOnClick = false;
            this.lblAuthority.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblAuthority.EnableMarkup = false;
            this.lblAuthority.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblAuthority.Name = "lblAuthority";
            this.lblAuthority.ShowSubItems = false;
            this.lblAuthority.SingleLineColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblAuthority, "lblAuthority");
            this.lblAuthority.Width = 150;
            // 
            // lblUser
            // 
            this.lblUser.AutoCollapseOnClick = false;
            this.lblUser.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblUser.EnableMarkup = false;
            this.lblUser.Name = "lblUser";
            this.lblUser.ShowSubItems = false;
            resources.ApplyResources(this.lblUser, "lblUser");
            this.lblUser.Width = 150;
            // 
            // lblTaskSheet
            // 
            this.lblTaskSheet.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblTaskSheet.EnableMarkup = false;
            this.lblTaskSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaskSheet.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblTaskSheet.Name = "lblTaskSheet";
            this.lblTaskSheet.ShowSubItems = false;
            resources.ApplyResources(this.lblTaskSheet, "lblTaskSheet");
            this.lblTaskSheet.Width = 400;
            // 
            // lblCode
            // 
            this.lblCode.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblCode.Name = "lblCode";
            this.lblCode.ShowSubItems = false;
            resources.ApplyResources(this.lblCode, "lblCode");
            this.lblCode.Width = 150;
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoCollapseOnClick = false;
            this.lblDateTime.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.lblDateTime.EnableMarkup = false;
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.ShowSubItems = false;
            resources.ApplyResources(this.lblDateTime, "lblDateTime");
            this.lblDateTime.Width = 150;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.tabcMenu);
            this.pnlTop.Controls.Add(this.pnlLogo);
            resources.ApplyResources(this.pnlTop, "pnlTop");
            this.pnlTop.Name = "pnlTop";
            // 
            // tabcMenu
            // 
            this.tabcMenu.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tabcMenu.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            // 
            // 
            // 
            this.tabcMenu.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.tabcMenu.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.tabcMenu.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabcMenu.ControlBox.MenuBox,
            this.tabcMenu.ControlBox.CloseBox});
            this.tabcMenu.Controls.Add(this.tabpCondition);
            this.tabcMenu.Controls.Add(this.tabpResult);
            this.tabcMenu.Controls.Add(this.tabpSetting);
            resources.ApplyResources(this.tabcMenu, "tabcMenu");
            this.tabcMenu.FixedTabSize = new System.Drawing.Size(128, 30);
            this.tabcMenu.Name = "tabcMenu";
            this.tabcMenu.ReorderTabsEnabled = false;
            this.tabcMenu.SelectedTabIndex = 0;
            this.tabcMenu.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabCondition,
            this.tabResult,
            this.tabSetting});
            this.tabcMenu.SelectedTabChanged += new System.EventHandler<DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs>(this.tabcMenu_SelectedTabChanged);
            // 
            // tabpCondition
            // 
            this.tabpCondition.Controls.Add(this.tabsSubMenu03);
            resources.ApplyResources(this.tabpCondition, "tabpCondition");
            this.tabpCondition.Name = "tabpCondition";
            this.tabpCondition.TabItem = this.tabCondition;
            // 
            // tabsSubMenu03
            // 
            this.tabsSubMenu03.AutoSelectAttachedControl = true;
            this.tabsSubMenu03.CanReorderTabs = false;
            this.tabsSubMenu03.CloseButtonVisible = false;
            this.tabsSubMenu03.ColorScheme.TabBackground = System.Drawing.Color.AliceBlue;
            this.tabsSubMenu03.ColorScheme.TabBackground2 = System.Drawing.Color.LightBlue;
            this.tabsSubMenu03.ColorScheme.TabBackgroundGradientAngle = 180;
            this.tabsSubMenu03.ColorScheme.TabItemBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(230)))), ((int)(((byte)(249))))), 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(220)))), ((int)(((byte)(248))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(208)))), ((int)(((byte)(245))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(229)))), ((int)(((byte)(247))))), 1F)});
            this.tabsSubMenu03.ColorScheme.TabItemBackgroundGradientAngle = 180;
            this.tabsSubMenu03.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(235))))), 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(236)))), ((int)(((byte)(168))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(218)))), ((int)(((byte)(89))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(230)))), ((int)(((byte)(141))))), 1F)});
            this.tabsSubMenu03.ColorScheme.TabItemSelectedBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabsSubMenu03.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254))))), 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254))))), 1F)});
            this.tabsSubMenu03.ColorScheme.TabPanelBackground = System.Drawing.Color.LightBlue;
            this.tabsSubMenu03.ColorScheme.TabPanelBackground2 = System.Drawing.Color.LightBlue;
            this.tabsSubMenu03.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tabsSubMenu03, "tabsSubMenu03");
            this.tabsSubMenu03.FixedTabSize = new System.Drawing.Size(128, 0);
            this.tabsSubMenu03.Name = "tabsSubMenu03";
            this.tabsSubMenu03.SelectedTab = this.tabItem3;
            this.tabsSubMenu03.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabsSubMenu03.Tabs.Add(this.tabItem3);
            this.tabsSubMenu03.Tabs.Add(this.tabItem4);
            // 
            // tabItem3
            // 
            this.tabItem3.Name = "tabItem3";
            resources.ApplyResources(this.tabItem3, "tabItem3");
            // 
            // tabItem4
            // 
            this.tabItem4.Name = "tabItem4";
            resources.ApplyResources(this.tabItem4, "tabItem4");
            // 
            // tabCondition
            // 
            this.tabCondition.AttachedControl = this.tabpCondition;
            this.tabCondition.GlobalItem = false;
            this.tabCondition.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestItem_16;
            this.tabCondition.Name = "tabCondition";
            resources.ApplyResources(this.tabCondition, "tabCondition");
            // 
            // tabpSetting
            // 
            this.tabpSetting.Controls.Add(this.tabsSubMenu02);
            resources.ApplyResources(this.tabpSetting, "tabpSetting");
            this.tabpSetting.Name = "tabpSetting";
            this.tabpSetting.TabItem = this.tabSetting;
            // 
            // tabsSubMenu02
            // 
            this.tabsSubMenu02.AutoSelectAttachedControl = true;
            this.tabsSubMenu02.CanReorderTabs = false;
            this.tabsSubMenu02.CloseButtonVisible = true;
            this.tabsSubMenu02.ColorScheme.TabBackground = System.Drawing.Color.AliceBlue;
            this.tabsSubMenu02.ColorScheme.TabBackground2 = System.Drawing.Color.LightBlue;
            this.tabsSubMenu02.ColorScheme.TabBackgroundGradientAngle = 180;
            this.tabsSubMenu02.ColorScheme.TabItemSelectedBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabsSubMenu02.ColorScheme.TabPanelBackground = System.Drawing.Color.LightBlue;
            this.tabsSubMenu02.ColorScheme.TabPanelBackground2 = System.Drawing.Color.LightBlue;
            this.tabsSubMenu02.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tabsSubMenu02, "tabsSubMenu02");
            this.tabsSubMenu02.FixedTabSize = new System.Drawing.Size(128, 0);
            this.tabsSubMenu02.Name = "tabsSubMenu02";
            this.tabsSubMenu02.SelectedTab = null;
            // 
            // tabSetting
            // 
            this.tabSetting.AttachedControl = this.tabpSetting;
            this.tabSetting.GlobalItem = false;
            this.tabSetting.Image = global::MPI.Tester.Gui.Properties.Resources.btnSetting_16;
            this.tabSetting.Name = "tabSetting";
            resources.ApplyResources(this.tabSetting, "tabSetting");
            // 
            // tabpResult
            // 
            this.tabpResult.Controls.Add(this.tabsSubMenu05);
            resources.ApplyResources(this.tabpResult, "tabpResult");
            this.tabpResult.Name = "tabpResult";
            this.tabpResult.TabItem = this.tabResult;
            // 
            // tabsSubMenu05
            // 
            this.tabsSubMenu05.AutoSelectAttachedControl = true;
            this.tabsSubMenu05.CanReorderTabs = false;
            this.tabsSubMenu05.CloseButtonVisible = true;
            this.tabsSubMenu05.ColorScheme.TabBackground = System.Drawing.Color.AliceBlue;
            this.tabsSubMenu05.ColorScheme.TabBackground2 = System.Drawing.Color.LightBlue;
            this.tabsSubMenu05.ColorScheme.TabBackgroundGradientAngle = 180;
            this.tabsSubMenu05.ColorScheme.TabItemSelectedBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabsSubMenu05.ColorScheme.TabPanelBackground = System.Drawing.Color.LightBlue;
            this.tabsSubMenu05.ColorScheme.TabPanelBackground2 = System.Drawing.Color.LightBlue;
            this.tabsSubMenu05.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tabsSubMenu05, "tabsSubMenu05");
            this.tabsSubMenu05.FixedTabSize = new System.Drawing.Size(128, 0);
            this.tabsSubMenu05.Name = "tabsSubMenu05";
            this.tabsSubMenu05.SelectedTab = null;
            // 
            // tabResult
            // 
            this.tabResult.AttachedControl = this.tabpResult;
            this.tabResult.GlobalItem = false;
            this.tabResult.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestResult_16;
            this.tabResult.Name = "tabResult";
            resources.ApplyResources(this.tabResult, "tabResult");
            // 
            // pnlLogo
            // 
            this.pnlLogo.CanvasColor = System.Drawing.Color.Empty;
            this.pnlLogo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.pnlLogo.Controls.Add(this.lblTesterMode);
            this.pnlLogo.Controls.Add(this.pictureBox1);
            resources.ApplyResources(this.pnlLogo, "pnlLogo");
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.pnlLogo.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(245)))));
            this.pnlLogo.Style.BackColor2.Color = System.Drawing.Color.WhiteSmoke;
            this.pnlLogo.Style.BorderColor.Color = System.Drawing.Color.DarkSlateGray;
            this.pnlLogo.Style.ForeColor.Color = System.Drawing.Color.DarkOrange;
            this.pnlLogo.Style.GradientAngle = 90;
            // 
            // lblTesterMode
            // 
            resources.ApplyResources(this.lblTesterMode, "lblTesterMode");
            this.lblTesterMode.Name = "lblTesterMode";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::MPI.Tester.Gui.Properties.Resources.C101;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // plRecipeCtrl
            // 
            this.plRecipeCtrl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.plRecipeCtrl.Controls.Add(this.plCondShow);
            this.plRecipeCtrl.Controls.Add(this.lblRecipeFileName);
            this.plRecipeCtrl.Controls.Add(this.lblFilterPosition);
            this.plRecipeCtrl.Controls.Add(this.lblHWFilterSelect);
            this.plRecipeCtrl.Controls.Add(this.cmbTaskSheet);
            this.plRecipeCtrl.Controls.Add(this.btnLoadTask);
            resources.ApplyResources(this.plRecipeCtrl, "plRecipeCtrl");
            this.plRecipeCtrl.Name = "plRecipeCtrl";
            // 
            // plCondShow
            // 
            this.plCondShow.Controls.Add(this.btnCreateTask);
            this.plCondShow.Controls.Add(this.btnDelete);
            this.plCondShow.Controls.Add(this.btnSaveAs);
            resources.ApplyResources(this.plCondShow, "plCondShow");
            this.plCondShow.Name = "plCondShow";
            // 
            // btnCreateTask
            // 
            this.btnCreateTask.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCreateTask.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnCreateTask, "btnCreateTask");
            this.btnCreateTask.ForeColor = System.Drawing.Color.Black;
            this.btnCreateTask.Image = global::MPI.Tester.Gui.Properties.Resources.btnNewAdd;
            this.btnCreateTask.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.btnCreateTask.Name = "btnCreateTask";
            this.btnCreateTask.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCreateTask.Click += new System.EventHandler(this.btnCreateTask_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Image = global::MPI.Tester.Gui.Properties.Resources.btnCancel;
            this.btnDelete.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveAs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnSaveAs, "btnSaveAs");
            this.btnSaveAs.ForeColor = System.Drawing.Color.Black;
            this.btnSaveAs.Image = global::MPI.Tester.Gui.Properties.Resources.btnDivide;
            this.btnSaveAs.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // lblRecipeFileName
            // 
            // 
            // 
            // 
            this.lblRecipeFileName.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblRecipeFileName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblRecipeFileName, "lblRecipeFileName");
            this.lblRecipeFileName.ForeColor = System.Drawing.Color.Black;
            this.lblRecipeFileName.Name = "lblRecipeFileName";
            // 
            // lblFilterPosition
            // 
            this.lblFilterPosition.BackColor = System.Drawing.SystemColors.Control;
            this.lblFilterPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblFilterPosition, "lblFilterPosition");
            this.lblFilterPosition.ForeColor = System.Drawing.Color.Navy;
            this.lblFilterPosition.Name = "lblFilterPosition";
            // 
            // lblHWFilterSelect
            // 
            this.lblHWFilterSelect.BackColor = System.Drawing.SystemColors.Control;
            this.lblHWFilterSelect.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblHWFilterSelect, "lblHWFilterSelect");
            this.lblHWFilterSelect.ForeColor = System.Drawing.Color.Red;
            this.lblHWFilterSelect.Name = "lblHWFilterSelect";
            // 
            // cmbTaskSheet
            // 
            this.cmbTaskSheet.DisplayMember = "Text";
            this.cmbTaskSheet.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbTaskSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTaskSheet.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbTaskSheet.FocusHighlightColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.cmbTaskSheet, "cmbTaskSheet");
            this.cmbTaskSheet.FormattingEnabled = true;
            this.cmbTaskSheet.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3});
            this.cmbTaskSheet.Name = "cmbTaskSheet";
            this.cmbTaskSheet.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.cmbTaskSheet.WatermarkEnabled = false;
            this.cmbTaskSheet.SelectionChangeCommitted += new System.EventHandler(this.cmbTaskSheet_SelectionChangeCommitted);
            // 
            // comboItem1
            // 
            resources.ApplyResources(this.comboItem1, "comboItem1");
            // 
            // comboItem2
            // 
            resources.ApplyResources(this.comboItem2, "comboItem2");
            // 
            // comboItem3
            // 
            resources.ApplyResources(this.comboItem3, "comboItem3");
            // 
            // btnLoadTask
            // 
            this.btnLoadTask.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadTask.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnLoadTask, "btnLoadTask");
            this.btnLoadTask.ForeColor = System.Drawing.Color.Black;
            this.btnLoadTask.Image = global::MPI.Tester.Gui.Properties.Resources.btnSelectSource;
            this.btnLoadTask.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.btnLoadTask.Name = "btnLoadTask";
            this.btnLoadTask.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnLoadTask.Click += new System.EventHandler(this.btnLoadTask_Click);
            // 
            // pnlCenter
            // 
            this.pnlCenter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlCenter.Controls.Add(this.plRecipeCtrl);
            this.pnlCenter.Controls.Add(this.btnArrangeWindow);
            this.pnlCenter.Controls.Add(this.pnlContainer);
            this.pnlCenter.Controls.Add(this.barLeft);
            resources.ApplyResources(this.pnlCenter, "pnlCenter");
            this.pnlCenter.Name = "pnlCenter";
            // 
            // btnArrangeWindow
            // 
            this.btnArrangeWindow.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            resources.ApplyResources(this.btnArrangeWindow, "btnArrangeWindow");
            this.btnArrangeWindow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnArrangeWindow.Image = global::MPI.Tester.Gui.Properties.Resources.btnWaferMap_32;
            this.btnArrangeWindow.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnArrangeWindow.Name = "btnArrangeWindow";
            this.btnArrangeWindow.ShowSubItems = false;
            this.btnArrangeWindow.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.btnArrangeWindow.TabStop = false;
            this.btnArrangeWindow.Click += new System.EventHandler(this.btnArrangeWindow_Click);
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.Transparent;
            this.pnlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlContainer.Controls.Add(this.lblHWFilterWheel);
            this.pnlContainer.Controls.Add(this.lblSWFilterWheel);
            resources.ApplyResources(this.pnlContainer, "pnlContainer");
            this.pnlContainer.Name = "pnlContainer";
            // 
            // lblHWFilterWheel
            // 
            this.lblHWFilterWheel.BackColor = System.Drawing.Color.Lavender;
            this.lblHWFilterWheel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblHWFilterWheel, "lblHWFilterWheel");
            this.lblHWFilterWheel.ForeColor = System.Drawing.Color.Black;
            this.lblHWFilterWheel.Name = "lblHWFilterWheel";
            // 
            // lblSWFilterWheel
            // 
            this.lblSWFilterWheel.BackColor = System.Drawing.Color.Lavender;
            this.lblSWFilterWheel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblSWFilterWheel, "lblSWFilterWheel");
            this.lblSWFilterWheel.ForeColor = System.Drawing.Color.Black;
            this.lblSWFilterWheel.Name = "lblSWFilterWheel";
            // 
            // barLeft
            // 
            resources.ApplyResources(this.barLeft, "barLeft");
            this.barLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barLeft.AntiAlias = true;
            this.barLeft.BackColor = System.Drawing.Color.WhiteSmoke;
            this.barLeft.BarType = DevComponents.DotNetBar.eBarType.DockWindow;
            this.barLeft.CanDockBottom = false;
            this.barLeft.CanDockRight = false;
            this.barLeft.CanDockTab = false;
            this.barLeft.CanDockTop = false;
            this.barLeft.CanUndock = false;
            this.barLeft.Controls.Add(this.btnCloseApp);
            this.barLeft.Controls.Add(this.btnExitApp);
            this.barLeft.Controls.Add(this.btnAlarmLog);
            this.barLeft.Controls.Add(this.btnContinousProbing);
            this.barLeft.Controls.Add(this.btnResetState);
            this.barLeft.Controls.Add(this.btnUserMgr);
            this.barLeft.Controls.Add(this.btnReSingleTest);
            this.barLeft.Controls.Add(this.btnGoProberUI);
            this.barLeft.Controls.Add(this.btnLogin);
            this.barLeft.Controls.Add(this.btnEndAndSave);
            this.barLeft.Controls.Add(this.btnCycleRun);
            this.barLeft.Controls.Add(this.btnStartAndOpen);
            this.barLeft.Controls.Add(this.btnCIEMap);
            this.barLeft.Controls.Add(this.btnWaferMap);
            this.barLeft.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.barLeft.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.barLeft.LayoutType = DevComponents.DotNetBar.eLayoutType.TaskList;
            this.barLeft.Name = "barLeft";
            this.barLeft.RoundCorners = false;
            this.barLeft.SaveLayoutChanges = false;
            this.barLeft.SingleLineColor = System.Drawing.Color.Silver;
            this.barLeft.Stretch = true;
            this.barLeft.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.barLeft.TabStop = false;
            // 
            // btnCloseApp
            // 
            this.btnCloseApp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCloseApp.AntiAlias = true;
            this.btnCloseApp.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnCloseApp, "btnCloseApp");
            this.btnCloseApp.Image = global::MPI.Tester.Gui.Properties.Resources.btnExitApp;
            this.btnCloseApp.ImageFixedSize = new System.Drawing.Size(40, 40);
            this.btnCloseApp.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnCloseApp.Name = "btnCloseApp";
            this.btnCloseApp.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(4);
            this.btnCloseApp.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCloseApp.Tag = "2";
            this.btnCloseApp.Click += new System.EventHandler(this.btnCloseApp_Click);
            // 
            // btnExitApp
            // 
            this.btnExitApp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExitApp.AntiAlias = true;
            this.btnExitApp.BackColor = System.Drawing.Color.Transparent;
            this.btnExitApp.Image = global::MPI.Tester.Gui.Properties.Resources.btnExitApp;
            this.btnExitApp.ImageFixedSize = new System.Drawing.Size(40, 40);
            this.btnExitApp.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            resources.ApplyResources(this.btnExitApp, "btnExitApp");
            this.btnExitApp.Name = "btnExitApp";
            this.btnExitApp.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(4);
            this.btnExitApp.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnExitApp.Tag = "2";
            this.btnExitApp.Click += new System.EventHandler(this.btnExitApp_Click);
            // 
            // btnAlarmLog
            // 
            this.btnAlarmLog.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAlarmLog.AntiAlias = true;
            this.btnAlarmLog.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnAlarmLog, "btnAlarmLog");
            this.btnAlarmLog.Image = global::MPI.Tester.Gui.Properties.Resources.btnAlramList;
            this.btnAlarmLog.ImageFixedSize = new System.Drawing.Size(40, 40);
            this.btnAlarmLog.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnAlarmLog.Name = "btnAlarmLog";
            this.btnAlarmLog.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(4);
            this.btnAlarmLog.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnAlarmLog.Tag = "2";
            this.btnAlarmLog.Click += new System.EventHandler(this.btnAlarmLog_Click);
            // 
            // btnContinousProbing
            // 
            this.btnContinousProbing.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnContinousProbing.AntiAlias = true;
            this.btnContinousProbing.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnContinousProbing.EnableMarkup = false;
            this.btnContinousProbing.ImageFixedSize = new System.Drawing.Size(38, 38);
            this.btnContinousProbing.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnContinousProbing.ImageTextSpacing = -5;
            resources.ApplyResources(this.btnContinousProbing, "btnContinousProbing");
            this.btnContinousProbing.Name = "btnContinousProbing";
            this.btnContinousProbing.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnContinousProbing.ShowSubItems = false;
            this.btnContinousProbing.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnContinousProbing.TabStop = false;
            this.btnContinousProbing.Click += new System.EventHandler(this.btnContinousProbing_Click);
            // 
            // btnResetState
            // 
            this.btnResetState.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnResetState.AntiAlias = true;
            this.btnResetState.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnResetState.EnableMarkup = false;
            resources.ApplyResources(this.btnResetState, "btnResetState");
            this.btnResetState.Image = global::MPI.Tester.Gui.Properties.Resources.btnRepeatTest;
            this.btnResetState.ImageFixedSize = new System.Drawing.Size(45, 45);
            this.btnResetState.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnResetState.Name = "btnResetState";
            this.btnResetState.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnResetState.ShowSubItems = false;
            this.btnResetState.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnResetState.TabStop = false;
            this.btnResetState.Tag = "Reset State";
            this.btnResetState.Click += new System.EventHandler(this.btnResetState_Click);
            // 
            // btnUserMgr
            // 
            this.btnUserMgr.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUserMgr.AntiAlias = true;
            this.btnUserMgr.BackColor = System.Drawing.Color.MistyRose;
            this.btnUserMgr.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnUserMgr.EnableMarkup = false;
            resources.ApplyResources(this.btnUserMgr, "btnUserMgr");
            this.btnUserMgr.Image = global::MPI.Tester.Gui.Properties.Resources.btnUserManager_32;
            this.btnUserMgr.ImageFixedSize = new System.Drawing.Size(44, 44);
            this.btnUserMgr.Name = "btnUserMgr";
            this.btnUserMgr.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnUserMgr.ShowSubItems = false;
            this.btnUserMgr.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnUserMgr.TabStop = false;
            this.btnUserMgr.Click += new System.EventHandler(this.btnUserMgr_Click);
            // 
            // btnReSingleTest
            // 
            this.btnReSingleTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReSingleTest.AntiAlias = true;
            this.btnReSingleTest.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnReSingleTest.EnableMarkup = false;
            resources.ApplyResources(this.btnReSingleTest, "btnReSingleTest");
            this.btnReSingleTest.Image = global::MPI.Tester.Gui.Properties.Resources.btnOperation_32;
            this.btnReSingleTest.ImageFixedSize = new System.Drawing.Size(36, 36);
            this.btnReSingleTest.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnReSingleTest.Name = "btnReSingleTest";
            this.btnReSingleTest.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnReSingleTest.ShowSubItems = false;
            this.btnReSingleTest.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnReSingleTest.TabStop = false;
            this.btnReSingleTest.Tag = "Re Single Test";
            this.btnReSingleTest.Click += new System.EventHandler(this.btnReSingleTest_Click);
            // 
            // btnGoProberUI
            // 
            this.btnGoProberUI.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGoProberUI.AntiAlias = true;
            this.btnGoProberUI.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnGoProberUI.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnGoProberUI.EnableMarkup = false;
            resources.ApplyResources(this.btnGoProberUI, "btnGoProberUI");
            this.btnGoProberUI.Image = global::MPI.Tester.Gui.Properties.Resources.btnSwitch;
            this.btnGoProberUI.ImageFixedSize = new System.Drawing.Size(44, 44);
            this.btnGoProberUI.Name = "btnGoProberUI";
            this.btnGoProberUI.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnGoProberUI.ShowSubItems = false;
            this.btnGoProberUI.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnGoProberUI.TabStop = false;
            this.btnGoProberUI.Click += new System.EventHandler(this.btnGoProberUI_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLogin.AntiAlias = true;
            this.btnLogin.BackColor = System.Drawing.Color.MistyRose;
            this.btnLogin.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnLogin.EnableMarkup = false;
            resources.ApplyResources(this.btnLogin, "btnLogin");
            this.btnLogin.Image = global::MPI.Tester.Gui.Properties.Resources.other_social;
            this.btnLogin.ImageFixedSize = new System.Drawing.Size(36, 36);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnLogin.ShowSubItems = false;
            this.btnLogin.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnLogin.TabStop = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnEndAndSave
            // 
            this.btnEndAndSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEndAndSave.AntiAlias = true;
            this.btnEndAndSave.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnEndAndSave.EnableMarkup = false;
            resources.ApplyResources(this.btnEndAndSave, "btnEndAndSave");
            this.btnEndAndSave.Image = global::MPI.Tester.Gui.Properties.Resources.btnBlueStop_32;
            this.btnEndAndSave.ImageFixedSize = new System.Drawing.Size(45, 45);
            this.btnEndAndSave.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnEndAndSave.Name = "btnEndAndSave";
            this.btnEndAndSave.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnEndAndSave.ShowSubItems = false;
            this.btnEndAndSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnEndAndSave.TabStop = false;
            this.btnEndAndSave.Click += new System.EventHandler(this.btnEndAndSave_Click);
            // 
            // btnCycleRun
            // 
            this.btnCycleRun.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCycleRun.AntiAlias = true;
            this.btnCycleRun.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnCycleRun.EnableMarkup = false;
            resources.ApplyResources(this.btnCycleRun, "btnCycleRun");
            this.btnCycleRun.Image = global::MPI.Tester.Gui.Properties.Resources.btnCycleRun_32;
            this.btnCycleRun.ImageFixedSize = new System.Drawing.Size(45, 45);
            this.btnCycleRun.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnCycleRun.Name = "btnCycleRun";
            this.btnCycleRun.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnCycleRun.ShowSubItems = false;
            this.btnCycleRun.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCycleRun.TabStop = false;
            this.btnCycleRun.Click += new System.EventHandler(this.btnCycleRun_Click);
            // 
            // btnStartAndOpen
            // 
            this.btnStartAndOpen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStartAndOpen.AntiAlias = true;
            this.btnStartAndOpen.AutoCheckOnClick = true;
            this.btnStartAndOpen.EnableMarkup = false;
            resources.ApplyResources(this.btnStartAndOpen, "btnStartAndOpen");
            this.btnStartAndOpen.Image = global::MPI.Tester.Gui.Properties.Resources.btnBlueStart_32;
            this.btnStartAndOpen.ImageFixedSize = new System.Drawing.Size(45, 45);
            this.btnStartAndOpen.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnStartAndOpen.Name = "btnStartAndOpen";
            this.btnStartAndOpen.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnStartAndOpen.ShowSubItems = false;
            this.btnStartAndOpen.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnStartAndOpen.TabStop = false;
            this.btnStartAndOpen.Click += new System.EventHandler(this.btnStartAndOpen_Click);
            // 
            // btnCIEMap
            // 
            this.btnCIEMap.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCIEMap.AntiAlias = true;
            this.btnCIEMap.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnCIEMap.EnableMarkup = false;
            this.btnCIEMap.Image = global::MPI.Tester.Gui.Properties.Resources.btnCIEMap_32;
            this.btnCIEMap.ImageFixedSize = new System.Drawing.Size(45, 45);
            this.btnCIEMap.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            resources.ApplyResources(this.btnCIEMap, "btnCIEMap");
            this.btnCIEMap.Name = "btnCIEMap";
            this.btnCIEMap.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnCIEMap.ShowSubItems = false;
            this.btnCIEMap.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCIEMap.TabStop = false;
            this.btnCIEMap.Click += new System.EventHandler(this.btnCIEMap_Click);
            // 
            // btnWaferMap
            // 
            this.btnWaferMap.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnWaferMap.AntiAlias = true;
            this.btnWaferMap.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnWaferMap.EnableMarkup = false;
            this.btnWaferMap.Image = global::MPI.Tester.Gui.Properties.Resources.btnWaferMap_32;
            this.btnWaferMap.ImageFixedSize = new System.Drawing.Size(45, 45);
            resources.ApplyResources(this.btnWaferMap, "btnWaferMap");
            this.btnWaferMap.Name = "btnWaferMap";
            this.btnWaferMap.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnWaferMap.ShowSubItems = false;
            this.btnWaferMap.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnWaferMap.TabStop = false;
            this.btnWaferMap.Click += new System.EventHandler(this.btnWaferMap_Click);
            // 
            // labelItem1
            // 
            this.labelItem1.BorderType = DevComponents.DotNetBar.eBorderType.Raised;
            this.labelItem1.EnableMarkup = false;
            this.labelItem1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelItem1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.ShowSubItems = false;
            resources.ApplyResources(this.labelItem1, "labelItem1");
            this.labelItem1.Width = 400;
            // 
            // controlContainerItem1
            // 
            this.controlContainerItem1.AllowItemResize = false;
            this.controlContainerItem1.Control = this.buttonX2;
            this.controlContainerItem1.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem1.Name = "controlContainerItem1";
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.AntiAlias = true;
            this.buttonX2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.buttonX2, "buttonX2");
            this.buttonX2.Image = global::MPI.Tester.Gui.Properties.Resources.btnExitApp;
            this.buttonX2.ImageFixedSize = new System.Drawing.Size(40, 40);
            this.buttonX2.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(4);
            this.buttonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.buttonX2.Tag = "2";
            // 
            // frmBase
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.stsStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmBase";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBase_FormClosing);
            this.Load += new System.EventHandler(this.frmBase_Load);
            this.VisibleChanged += new System.EventHandler(this.frmBase_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.stsStatus)).EndInit();
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabcMenu)).EndInit();
            this.tabcMenu.ResumeLayout(false);
            this.tabpCondition.ResumeLayout(false);
            this.tabpSetting.ResumeLayout(false);
            this.tabpResult.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.plRecipeCtrl.ResumeLayout(false);
            this.plCondShow.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.pnlContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barLeft)).EndInit();
            this.barLeft.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer tmrUpdate;
		private DevComponents.DotNetBar.Bar stsStatus;
		private DevComponents.DotNetBar.LabelItem lblTaskSheet;
		private DevComponents.DotNetBar.LabelItem lblSysStatus;
		private DevComponents.DotNetBar.LabelItem lblUser;
		private DevComponents.DotNetBar.LabelItem lblAuthority;
        private DevComponents.DotNetBar.LabelItem lblDateTime;
		private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlCenter;
		private DevComponents.DotNetBar.PanelEx pnlLogo;
        private DevComponents.DotNetBar.SuperTabControl tabcMenu;
		private DevComponents.DotNetBar.SuperTabControlPanel tabpSetting;
        private DevComponents.DotNetBar.SuperTabItem tabSetting;
		private DevComponents.DotNetBar.SuperTabControlPanel tabpCondition;
		private DevComponents.DotNetBar.SuperTabItem tabCondition;
		private DevComponents.DotNetBar.SuperTabControlPanel tabpResult;
        private DevComponents.DotNetBar.SuperTabItem tabResult;
		private DevComponents.DotNetBar.TabStrip tabsSubMenu03;
		private DevComponents.DotNetBar.TabItem tabItem3;
        private DevComponents.DotNetBar.TabItem tabItem4;
		private DevComponents.DotNetBar.TabStrip tabsSubMenu02;
        private DevComponents.DotNetBar.TabStrip tabsSubMenu05;
        private DevComponents.DotNetBar.LabelItem lblCode;
        private DevComponents.DotNetBar.LabelX lblRecipeFileName;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.ButtonX btnLoadTask;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Label lblFilterPosition;
        private System.Windows.Forms.Label lblHWFilterSelect;
        private System.Windows.Forms.Label lblHWFilterWheel;
        private System.Windows.Forms.Label lblSWFilterWheel;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTaskSheet;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.DotNetBar.Bar barLeft;
        private DevComponents.DotNetBar.ButtonX btnUserMgr;
        private DevComponents.DotNetBar.ButtonX btnReSingleTest;
        private DevComponents.DotNetBar.ButtonX btnGoProberUI;
        private DevComponents.DotNetBar.ButtonX btnLogin;
        private DevComponents.DotNetBar.ButtonX btnEndAndSave;
        private DevComponents.DotNetBar.ButtonX btnCycleRun;
        private DevComponents.DotNetBar.ButtonX btnStartAndOpen;
        private DevComponents.DotNetBar.ButtonX btnCIEMap;
        private DevComponents.DotNetBar.ButtonX btnWaferMap;
        private DevComponents.DotNetBar.ButtonX btnResetState;
        private DevComponents.DotNetBar.ButtonX btnArrangeWindow;
        private DevComponents.DotNetBar.ButtonX btnContinousProbing;
        private DevComponents.DotNetBar.ButtonX btnAlarmLog;
        private DevComponents.DotNetBar.ButtonX btnCreateTask;
        private DevComponents.DotNetBar.ButtonX btnDelete;
        private DevComponents.DotNetBar.ButtonX btnSaveAs;
        private System.Windows.Forms.Panel plRecipeCtrl;
        private DevComponents.DotNetBar.ButtonX btnExitApp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTesterMode;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem1;
        private DevComponents.DotNetBar.ButtonX btnCloseApp;
        private System.Windows.Forms.Panel plCondShow;

	}
}