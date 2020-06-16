namespace MPI.Tester.Gui
{
	partial class frmWaferMapSetting
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaferMapSetting));
			this.gpbTopGroup = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.tabmMain = new DevComponents.DotNetBar.SuperTabControl();
			this.tabpEnableItem = new DevComponents.DotNetBar.SuperTabControlPanel();
			this.btnEnableItemClose = new System.Windows.Forms.Button();
			this.btnEnableItemSave = new System.Windows.Forms.Button();
			this.btnDelete = new DevComponents.DotNetBar.ButtonX();
			this.btnAdd = new DevComponents.DotNetBar.ButtonX();
			this.lblEnableItem = new DevComponents.DotNetBar.LabelX();
			this.lblMsrtItem = new DevComponents.DotNetBar.LabelX();
			this.lstEnableItem = new System.Windows.Forms.ListBox();
			this.lstMsrtItem = new System.Windows.Forms.ListBox();
			this.TabiEnableItem = new DevComponents.DotNetBar.SuperTabItem();
			this.tabpColorSetting = new DevComponents.DotNetBar.SuperTabControlPanel();
			this.btnColorSettingClose = new System.Windows.Forms.Button();
			this.gplDivideSetting = new System.Windows.Forms.GroupBox();
			this.lblMin = new System.Windows.Forms.Label();
			this.txtMinVal = new System.Windows.Forms.TextBox();
			this.lblMax = new System.Windows.Forms.Label();
			this.lblInterval = new System.Windows.Forms.Label();
			this.txtMaxVal = new System.Windows.Forms.TextBox();
			this.txtStepVal = new System.Windows.Forms.TextBox();
			this.btnDivide = new System.Windows.Forms.Button();
			this.gplOtherSetting = new System.Windows.Forms.GroupBox();
			this.txtMaxColor = new System.Windows.Forms.TextBox();
			this.chkBoxOutOfRange = new System.Windows.Forms.CheckBox();
			this.lblMaxColor = new System.Windows.Forms.Label();
			this.labMapBackColor = new System.Windows.Forms.Label();
			this.txtMinColor = new System.Windows.Forms.TextBox();
			this.txtMapBackColor = new System.Windows.Forms.TextBox();
			this.lblMinColor = new System.Windows.Forms.Label();
			this.gplColorSetting = new System.Windows.Forms.GroupBox();
			this.lblAdvSettingColor = new System.Windows.Forms.Label();
			this.lblBValue = new System.Windows.Forms.Label();
			this.lblGValue = new System.Windows.Forms.Label();
			this.lblRValue = new System.Windows.Forms.Label();
			this.btnSettingColor = new System.Windows.Forms.Button();
			this.sliderB = new DevComponents.DotNetBar.Controls.Slider();
			this.sliderG = new DevComponents.DotNetBar.Controls.Slider();
			this.sliderR = new DevComponents.DotNetBar.Controls.Slider();
			this.lblSelectedColor = new System.Windows.Forms.Label();
			this.lblCurrentColor = new DevComponents.DotNetBar.LabelX();
			this.cpbAdvSettingColor = new DevComponents.DotNetBar.ColorPickerButton();
			this.btnColorSettingSave = new System.Windows.Forms.Button();
			this.gplSelectedItemColors = new System.Windows.Forms.GroupBox();
			this.btnDefaultColor = new System.Windows.Forms.Button();
			this.gplMertItem = new System.Windows.Forms.GroupBox();
			this.lstColorSetMsrtItem = new System.Windows.Forms.ListBox();
			this.TabiColorSetting = new DevComponents.DotNetBar.SuperTabItem();
			this.gpbTopGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabmMain)).BeginInit();
			this.tabmMain.SuspendLayout();
			this.tabpEnableItem.SuspendLayout();
			this.tabpColorSetting.SuspendLayout();
			this.gplDivideSetting.SuspendLayout();
			this.gplOtherSetting.SuspendLayout();
			this.gplColorSetting.SuspendLayout();
			this.gplSelectedItemColors.SuspendLayout();
			this.gplMertItem.SuspendLayout();
			this.SuspendLayout();
			// 
			// gpbTopGroup
			// 
			this.gpbTopGroup.BackColor = System.Drawing.Color.Transparent;
			this.gpbTopGroup.CanvasColor = System.Drawing.Color.Empty;
			this.gpbTopGroup.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.gpbTopGroup.Controls.Add(this.tabmMain);
			resources.ApplyResources(this.gpbTopGroup, "gpbTopGroup");
			this.gpbTopGroup.DrawTitleBox = false;
			this.gpbTopGroup.IsShadowEnabled = true;
			this.gpbTopGroup.Name = "gpbTopGroup";
			// 
			// 
			// 
			this.gpbTopGroup.Style.BackColor = System.Drawing.Color.LightBlue;
			this.gpbTopGroup.Style.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.gpbTopGroup.Style.BackColorGradientAngle = 90;
			this.gpbTopGroup.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.gpbTopGroup.Style.BorderBottomWidth = 1;
			this.gpbTopGroup.Style.BorderColor = System.Drawing.Color.Gray;
			this.gpbTopGroup.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.gpbTopGroup.Style.BorderLeftWidth = 1;
			this.gpbTopGroup.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.gpbTopGroup.Style.BorderRightWidth = 1;
			this.gpbTopGroup.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.gpbTopGroup.Style.BorderTopWidth = 1;
			this.gpbTopGroup.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.gpbTopGroup.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			this.gpbTopGroup.Style.TextColor = System.Drawing.Color.DarkOrange;
			// 
			// 
			// 
			this.gpbTopGroup.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.gpbTopGroup.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.gpbTopGroup.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.gpbTopGroup.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// tabmMain
			// 
			this.tabmMain.AutoCloseTabs = false;
			this.tabmMain.BackColor = System.Drawing.Color.WhiteSmoke;
			resources.ApplyResources(this.tabmMain, "tabmMain");
			// 
			// 
			// 
			// 
			// 
			// 
			this.tabmMain.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			// 
			// 
			// 
			this.tabmMain.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.tabmMain.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.tabmMain.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabmMain.ControlBox.MenuBox,
            this.tabmMain.ControlBox.CloseBox});
			this.tabmMain.Controls.Add(this.tabpEnableItem);
			this.tabmMain.Controls.Add(this.tabpColorSetting);
			this.tabmMain.Name = "tabmMain";
			this.tabmMain.ReorderTabsEnabled = false;
			this.tabmMain.SelectedTabIndex = 0;
			this.tabmMain.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Left;
			this.tabmMain.TabHorizontalSpacing = 2;
			this.tabmMain.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.TabiEnableItem,
            this.TabiColorSetting});
			this.tabmMain.TabStop = false;
			this.tabmMain.TabStripTabStop = false;
			this.tabmMain.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.WinMediaPlayer12;
			this.tabmMain.TabVerticalSpacing = 12;
			this.tabmMain.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
			// 
			// tabpEnableItem
			// 
			this.tabpEnableItem.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
			this.tabpEnableItem.Controls.Add(this.btnEnableItemClose);
			this.tabpEnableItem.Controls.Add(this.btnEnableItemSave);
			this.tabpEnableItem.Controls.Add(this.btnDelete);
			this.tabpEnableItem.Controls.Add(this.btnAdd);
			this.tabpEnableItem.Controls.Add(this.lblEnableItem);
			this.tabpEnableItem.Controls.Add(this.lblMsrtItem);
			this.tabpEnableItem.Controls.Add(this.lstEnableItem);
			this.tabpEnableItem.Controls.Add(this.lstMsrtItem);
			resources.ApplyResources(this.tabpEnableItem, "tabpEnableItem");
			this.tabpEnableItem.Name = "tabpEnableItem";
			this.tabpEnableItem.TabItem = this.TabiEnableItem;
			// 
			// btnEnableItemClose
			// 
			this.btnEnableItemClose.BackColor = System.Drawing.Color.Chocolate;
			resources.ApplyResources(this.btnEnableItemClose, "btnEnableItemClose");
			this.btnEnableItemClose.Name = "btnEnableItemClose";
			this.btnEnableItemClose.UseVisualStyleBackColor = false;
			this.btnEnableItemClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnEnableItemSave
			// 
			this.btnEnableItemSave.BackColor = System.Drawing.Color.YellowGreen;
			resources.ApplyResources(this.btnEnableItemSave, "btnEnableItemSave");
			this.btnEnableItemSave.Name = "btnEnableItemSave";
			this.btnEnableItemSave.UseVisualStyleBackColor = false;
			this.btnEnableItemSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnDelete.Image = global::MPI.Tester.Gui.Properties.Resources.arrow_left;
			this.btnDelete.ImageFixedSize = new System.Drawing.Size(15, 30);
			resources.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnDelete.TabStop = false;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnAdd.Image = global::MPI.Tester.Gui.Properties.Resources.arrow_right;
			this.btnAdd.ImageFixedSize = new System.Drawing.Size(15, 30);
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnAdd.TabStop = false;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// lblEnableItem
			// 
			this.lblEnableItem.BackColor = System.Drawing.Color.PapayaWhip;
			// 
			// 
			// 
			this.lblEnableItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.lblEnableItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			resources.ApplyResources(this.lblEnableItem, "lblEnableItem");
			this.lblEnableItem.ForeColor = System.Drawing.Color.Black;
			this.lblEnableItem.Name = "lblEnableItem";
			this.lblEnableItem.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// lblMsrtItem
			// 
			this.lblMsrtItem.BackColor = System.Drawing.Color.PapayaWhip;
			// 
			// 
			// 
			this.lblMsrtItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.lblMsrtItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			resources.ApplyResources(this.lblMsrtItem, "lblMsrtItem");
			this.lblMsrtItem.ForeColor = System.Drawing.Color.Black;
			this.lblMsrtItem.Name = "lblMsrtItem";
			this.lblMsrtItem.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// lstEnableItem
			// 
			this.lstEnableItem.BackColor = System.Drawing.Color.WhiteSmoke;
			resources.ApplyResources(this.lstEnableItem, "lstEnableItem");
			this.lstEnableItem.FormattingEnabled = true;
			this.lstEnableItem.Name = "lstEnableItem";
			this.lstEnableItem.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.lstEnableItem.TabStop = false;
			// 
			// lstMsrtItem
			// 
			this.lstMsrtItem.BackColor = System.Drawing.Color.WhiteSmoke;
			resources.ApplyResources(this.lstMsrtItem, "lstMsrtItem");
			this.lstMsrtItem.FormattingEnabled = true;
			this.lstMsrtItem.Name = "lstMsrtItem";
			this.lstMsrtItem.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.lstMsrtItem.TabStop = false;
			this.lstMsrtItem.SelectedIndexChanged += new System.EventHandler(this.lstColorSetMsrtItem_SelectedIndexChanged);
			// 
			// TabiEnableItem
			// 
			this.TabiEnableItem.AttachedControl = this.tabpEnableItem;
			this.TabiEnableItem.GlobalItem = false;
			this.TabiEnableItem.Name = "TabiEnableItem";
			resources.ApplyResources(this.TabiEnableItem, "TabiEnableItem");
			// 
			// tabpColorSetting
			// 
			this.tabpColorSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
			this.tabpColorSetting.Controls.Add(this.btnColorSettingClose);
			this.tabpColorSetting.Controls.Add(this.gplDivideSetting);
			this.tabpColorSetting.Controls.Add(this.gplOtherSetting);
			this.tabpColorSetting.Controls.Add(this.gplColorSetting);
			this.tabpColorSetting.Controls.Add(this.btnColorSettingSave);
			this.tabpColorSetting.Controls.Add(this.gplSelectedItemColors);
			this.tabpColorSetting.Controls.Add(this.gplMertItem);
			resources.ApplyResources(this.tabpColorSetting, "tabpColorSetting");
			this.tabpColorSetting.Name = "tabpColorSetting";
			this.tabpColorSetting.TabItem = this.TabiColorSetting;
			// 
			// btnColorSettingClose
			// 
			this.btnColorSettingClose.BackColor = System.Drawing.Color.Chocolate;
			resources.ApplyResources(this.btnColorSettingClose, "btnColorSettingClose");
			this.btnColorSettingClose.Name = "btnColorSettingClose";
			this.btnColorSettingClose.UseVisualStyleBackColor = false;
			this.btnColorSettingClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// gplDivideSetting
			// 
			this.gplDivideSetting.BackColor = System.Drawing.Color.Transparent;
			this.gplDivideSetting.Controls.Add(this.lblMin);
			this.gplDivideSetting.Controls.Add(this.txtMinVal);
			this.gplDivideSetting.Controls.Add(this.lblMax);
			this.gplDivideSetting.Controls.Add(this.lblInterval);
			this.gplDivideSetting.Controls.Add(this.txtMaxVal);
			this.gplDivideSetting.Controls.Add(this.txtStepVal);
			this.gplDivideSetting.Controls.Add(this.btnDivide);
			resources.ApplyResources(this.gplDivideSetting, "gplDivideSetting");
			this.gplDivideSetting.ForeColor = System.Drawing.SystemColors.ControlText;
			this.gplDivideSetting.Name = "gplDivideSetting";
			this.gplDivideSetting.TabStop = false;
			// 
			// lblMin
			// 
			resources.ApplyResources(this.lblMin, "lblMin");
			this.lblMin.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblMin.Name = "lblMin";
			// 
			// txtMinVal
			// 
			resources.ApplyResources(this.txtMinVal, "txtMinVal");
			this.txtMinVal.Name = "txtMinVal";
			// 
			// lblMax
			// 
			resources.ApplyResources(this.lblMax, "lblMax");
			this.lblMax.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblMax.Name = "lblMax";
			// 
			// lblInterval
			// 
			resources.ApplyResources(this.lblInterval, "lblInterval");
			this.lblInterval.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblInterval.Name = "lblInterval";
			// 
			// txtMaxVal
			// 
			resources.ApplyResources(this.txtMaxVal, "txtMaxVal");
			this.txtMaxVal.Name = "txtMaxVal";
			// 
			// txtStepVal
			// 
			resources.ApplyResources(this.txtStepVal, "txtStepVal");
			this.txtStepVal.Name = "txtStepVal";
			// 
			// btnDivide
			// 
			this.btnDivide.BackColor = System.Drawing.Color.LightBlue;
			resources.ApplyResources(this.btnDivide, "btnDivide");
			this.btnDivide.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnDivide.Name = "btnDivide";
			this.btnDivide.UseVisualStyleBackColor = false;
			this.btnDivide.Click += new System.EventHandler(this.btnDivide_Click);
			// 
			// gplOtherSetting
			// 
			this.gplOtherSetting.BackColor = System.Drawing.Color.Transparent;
			this.gplOtherSetting.Controls.Add(this.txtMaxColor);
			this.gplOtherSetting.Controls.Add(this.chkBoxOutOfRange);
			this.gplOtherSetting.Controls.Add(this.lblMaxColor);
			this.gplOtherSetting.Controls.Add(this.labMapBackColor);
			this.gplOtherSetting.Controls.Add(this.txtMinColor);
			this.gplOtherSetting.Controls.Add(this.txtMapBackColor);
			this.gplOtherSetting.Controls.Add(this.lblMinColor);
			resources.ApplyResources(this.gplOtherSetting, "gplOtherSetting");
			this.gplOtherSetting.ForeColor = System.Drawing.SystemColors.ControlText;
			this.gplOtherSetting.Name = "gplOtherSetting";
			this.gplOtherSetting.TabStop = false;
			// 
			// txtMaxColor
			// 
			this.txtMaxColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.txtMaxColor, "txtMaxColor");
			this.txtMaxColor.Name = "txtMaxColor";
			this.txtMaxColor.ReadOnly = true;
			this.txtMaxColor.Click += new System.EventHandler(this.OnClickChangeColor);
			// 
			// chkBoxOutOfRange
			// 
			resources.ApplyResources(this.chkBoxOutOfRange, "chkBoxOutOfRange");
			this.chkBoxOutOfRange.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkBoxOutOfRange.Name = "chkBoxOutOfRange";
			this.chkBoxOutOfRange.UseVisualStyleBackColor = true;
			this.chkBoxOutOfRange.CheckedChanged += new System.EventHandler(this.chkBoxOutOfRange_CheckedChanged);
			// 
			// lblMaxColor
			// 
			resources.ApplyResources(this.lblMaxColor, "lblMaxColor");
			this.lblMaxColor.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblMaxColor.Name = "lblMaxColor";
			// 
			// labMapBackColor
			// 
			resources.ApplyResources(this.labMapBackColor, "labMapBackColor");
			this.labMapBackColor.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labMapBackColor.Name = "labMapBackColor";
			// 
			// txtMinColor
			// 
			this.txtMinColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.txtMinColor, "txtMinColor");
			this.txtMinColor.Name = "txtMinColor";
			this.txtMinColor.ReadOnly = true;
			this.txtMinColor.Click += new System.EventHandler(this.OnClickChangeColor);
			// 
			// txtMapBackColor
			// 
			this.txtMapBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.txtMapBackColor, "txtMapBackColor");
			this.txtMapBackColor.Name = "txtMapBackColor";
			this.txtMapBackColor.ReadOnly = true;
			this.txtMapBackColor.Click += new System.EventHandler(this.OnClickChangeColor);
			// 
			// lblMinColor
			// 
			resources.ApplyResources(this.lblMinColor, "lblMinColor");
			this.lblMinColor.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblMinColor.Name = "lblMinColor";
			// 
			// gplColorSetting
			// 
			this.gplColorSetting.BackColor = System.Drawing.Color.Transparent;
			this.gplColorSetting.Controls.Add(this.lblAdvSettingColor);
			this.gplColorSetting.Controls.Add(this.lblBValue);
			this.gplColorSetting.Controls.Add(this.lblGValue);
			this.gplColorSetting.Controls.Add(this.lblRValue);
			this.gplColorSetting.Controls.Add(this.btnSettingColor);
			this.gplColorSetting.Controls.Add(this.sliderB);
			this.gplColorSetting.Controls.Add(this.sliderG);
			this.gplColorSetting.Controls.Add(this.sliderR);
			this.gplColorSetting.Controls.Add(this.lblSelectedColor);
			this.gplColorSetting.Controls.Add(this.lblCurrentColor);
			this.gplColorSetting.Controls.Add(this.cpbAdvSettingColor);
			resources.ApplyResources(this.gplColorSetting, "gplColorSetting");
			this.gplColorSetting.ForeColor = System.Drawing.SystemColors.ControlText;
			this.gplColorSetting.Name = "gplColorSetting";
			this.gplColorSetting.TabStop = false;
			// 
			// lblAdvSettingColor
			// 
			resources.ApplyResources(this.lblAdvSettingColor, "lblAdvSettingColor");
			this.lblAdvSettingColor.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblAdvSettingColor.Name = "lblAdvSettingColor";
			// 
			// lblBValue
			// 
			resources.ApplyResources(this.lblBValue, "lblBValue");
			this.lblBValue.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblBValue.Name = "lblBValue";
			// 
			// lblGValue
			// 
			resources.ApplyResources(this.lblGValue, "lblGValue");
			this.lblGValue.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblGValue.Name = "lblGValue";
			// 
			// lblRValue
			// 
			resources.ApplyResources(this.lblRValue, "lblRValue");
			this.lblRValue.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblRValue.Name = "lblRValue";
			// 
			// btnSettingColor
			// 
			this.btnSettingColor.BackColor = System.Drawing.Color.LightBlue;
			resources.ApplyResources(this.btnSettingColor, "btnSettingColor");
			this.btnSettingColor.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnSettingColor.Name = "btnSettingColor";
			this.btnSettingColor.UseVisualStyleBackColor = false;
			this.btnSettingColor.Click += new System.EventHandler(this.btnSettingColor_Click);
			// 
			// sliderB
			// 
			// 
			// 
			// 
			this.sliderB.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.sliderB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.sliderB.LabelWidth = 20;
			resources.ApplyResources(this.sliderB, "sliderB");
			this.sliderB.Maximum = 255;
			this.sliderB.Name = "sliderB";
			this.sliderB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.sliderB.TextColor = System.Drawing.Color.Blue;
			this.sliderB.Value = 0;
			this.sliderB.Click += new System.EventHandler(this.slider_Click);
			// 
			// sliderG
			// 
			// 
			// 
			// 
			this.sliderG.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.sliderG.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.sliderG.LabelWidth = 20;
			resources.ApplyResources(this.sliderG, "sliderG");
			this.sliderG.Maximum = 255;
			this.sliderG.Name = "sliderG";
			this.sliderG.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.sliderG.TextColor = System.Drawing.Color.Green;
			this.sliderG.Value = 0;
			this.sliderG.Click += new System.EventHandler(this.slider_Click);
			// 
			// sliderR
			// 
			// 
			// 
			// 
			this.sliderR.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.sliderR.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.sliderR.LabelWidth = 20;
			resources.ApplyResources(this.sliderR, "sliderR");
			this.sliderR.Maximum = 255;
			this.sliderR.Name = "sliderR";
			this.sliderR.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.sliderR.TextColor = System.Drawing.Color.Red;
			this.sliderR.Value = 0;
			this.sliderR.Click += new System.EventHandler(this.slider_Click);
			// 
			// lblSelectedColor
			// 
			resources.ApplyResources(this.lblSelectedColor, "lblSelectedColor");
			this.lblSelectedColor.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblSelectedColor.Name = "lblSelectedColor";
			// 
			// lblCurrentColor
			// 
			this.lblCurrentColor.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.lblCurrentColor.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
			this.lblCurrentColor.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lblCurrentColor, "lblCurrentColor");
			this.lblCurrentColor.Name = "lblCurrentColor";
			this.lblCurrentColor.BackColorChanged += new System.EventHandler(this.lblCurrentColor_BackColorChanged);
			// 
			// cpbAdvSettingColor
			// 
			this.cpbAdvSettingColor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.cpbAdvSettingColor.Checked = true;
			this.cpbAdvSettingColor.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.cpbAdvSettingColor.Image = ((System.Drawing.Image)(resources.GetObject("cpbAdvSettingColor.Image")));
			resources.ApplyResources(this.cpbAdvSettingColor, "cpbAdvSettingColor");
			this.cpbAdvSettingColor.Name = "cpbAdvSettingColor";
			this.cpbAdvSettingColor.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
			this.cpbAdvSettingColor.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.cpbAdvSettingColor.SelectedColorChanged += new System.EventHandler(this.cpbAdvSettingColor_SelectedColorChanged);
			// 
			// btnColorSettingSave
			// 
			this.btnColorSettingSave.BackColor = System.Drawing.Color.YellowGreen;
			resources.ApplyResources(this.btnColorSettingSave, "btnColorSettingSave");
			this.btnColorSettingSave.Name = "btnColorSettingSave";
			this.btnColorSettingSave.UseVisualStyleBackColor = false;
			this.btnColorSettingSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// gplSelectedItemColors
			// 
			this.gplSelectedItemColors.BackColor = System.Drawing.Color.Transparent;
			this.gplSelectedItemColors.Controls.Add(this.btnDefaultColor);
			resources.ApplyResources(this.gplSelectedItemColors, "gplSelectedItemColors");
			this.gplSelectedItemColors.ForeColor = System.Drawing.SystemColors.ControlText;
			this.gplSelectedItemColors.Name = "gplSelectedItemColors";
			this.gplSelectedItemColors.TabStop = false;
			// 
			// btnDefaultColor
			// 
			this.btnDefaultColor.BackColor = System.Drawing.Color.LightBlue;
			resources.ApplyResources(this.btnDefaultColor, "btnDefaultColor");
			this.btnDefaultColor.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnDefaultColor.Name = "btnDefaultColor";
			this.btnDefaultColor.UseVisualStyleBackColor = false;
			this.btnDefaultColor.Click += new System.EventHandler(this.btnDefaultColor_Click);
			// 
			// gplMertItem
			// 
			this.gplMertItem.BackColor = System.Drawing.Color.Transparent;
			this.gplMertItem.Controls.Add(this.lstColorSetMsrtItem);
			resources.ApplyResources(this.gplMertItem, "gplMertItem");
			this.gplMertItem.ForeColor = System.Drawing.SystemColors.ControlText;
			this.gplMertItem.Name = "gplMertItem";
			this.gplMertItem.TabStop = false;
			// 
			// lstColorSetMsrtItem
			// 
			resources.ApplyResources(this.lstColorSetMsrtItem, "lstColorSetMsrtItem");
			this.lstColorSetMsrtItem.FormattingEnabled = true;
			this.lstColorSetMsrtItem.Name = "lstColorSetMsrtItem";
			this.lstColorSetMsrtItem.SelectedIndexChanged += new System.EventHandler(this.lstColorSetMsrtItem_SelectedIndexChanged);
			// 
			// TabiColorSetting
			// 
			this.TabiColorSetting.AttachedControl = this.tabpColorSetting;
			this.TabiColorSetting.GlobalItem = false;
			this.TabiColorSetting.Name = "TabiColorSetting";
			resources.ApplyResources(this.TabiColorSetting, "TabiColorSetting");
			// 
			// frmWaferMapSetting
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Controls.Add(this.gpbTopGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmWaferMapSetting";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWaferMapSetting_FormClosing);
			this.Load += new System.EventHandler(this.frmWaferMapSetting_Load);
			this.gpbTopGroup.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tabmMain)).EndInit();
			this.tabmMain.ResumeLayout(false);
			this.tabpEnableItem.ResumeLayout(false);
			this.tabpColorSetting.ResumeLayout(false);
			this.gplDivideSetting.ResumeLayout(false);
			this.gplDivideSetting.PerformLayout();
			this.gplOtherSetting.ResumeLayout(false);
			this.gplOtherSetting.PerformLayout();
			this.gplColorSetting.ResumeLayout(false);
			this.gplColorSetting.PerformLayout();
			this.gplSelectedItemColors.ResumeLayout(false);
			this.gplMertItem.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevComponents.DotNetBar.Controls.GroupPanel gpbTopGroup;
		private DevComponents.DotNetBar.SuperTabControl tabmMain;
		private DevComponents.DotNetBar.SuperTabControlPanel tabpEnableItem;
		private DevComponents.DotNetBar.SuperTabItem TabiEnableItem;
		private System.Windows.Forms.ListBox lstEnableItem;
		private System.Windows.Forms.ListBox lstMsrtItem;
		private DevComponents.DotNetBar.LabelX lblEnableItem;
		private DevComponents.DotNetBar.LabelX lblMsrtItem;
		private DevComponents.DotNetBar.ButtonX btnDelete;
		private DevComponents.DotNetBar.ButtonX btnAdd;
		private DevComponents.DotNetBar.SuperTabControlPanel tabpColorSetting;
		private DevComponents.DotNetBar.SuperTabItem TabiColorSetting;
		private System.Windows.Forms.Button btnEnableItemSave;
		private System.Windows.Forms.GroupBox gplColorSetting;
		private System.Windows.Forms.Label labMapBackColor;
		private System.Windows.Forms.TextBox txtMapBackColor;
		private System.Windows.Forms.Label lblAdvSettingColor;
		private System.Windows.Forms.Label lblBValue;
		private System.Windows.Forms.Label lblGValue;
		private System.Windows.Forms.Label lblRValue;
		private System.Windows.Forms.Button btnSettingColor;
		private DevComponents.DotNetBar.Controls.Slider sliderB;
		private DevComponents.DotNetBar.Controls.Slider sliderG;
		private DevComponents.DotNetBar.Controls.Slider sliderR;
		private System.Windows.Forms.Label lblSelectedColor;
		private DevComponents.DotNetBar.LabelX lblCurrentColor;
		private DevComponents.DotNetBar.ColorPickerButton cpbAdvSettingColor;
		private System.Windows.Forms.Button btnColorSettingSave;
		private System.Windows.Forms.TextBox txtMaxColor;
		private System.Windows.Forms.Label lblMaxColor;
		private System.Windows.Forms.TextBox txtMinColor;
		private System.Windows.Forms.Label lblMinColor;
		private System.Windows.Forms.GroupBox gplSelectedItemColors;
		private System.Windows.Forms.Button btnDefaultColor;
		private System.Windows.Forms.GroupBox gplMertItem;
		private System.Windows.Forms.CheckBox chkBoxOutOfRange;
		private System.Windows.Forms.ListBox lstColorSetMsrtItem;
		private System.Windows.Forms.TextBox txtMinVal;
		private System.Windows.Forms.Label lblInterval;
		private System.Windows.Forms.TextBox txtStepVal;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Button btnDivide;
		private System.Windows.Forms.TextBox txtMaxVal;
		private System.Windows.Forms.Label lblMax;
		private System.Windows.Forms.GroupBox gplDivideSetting;
		private System.Windows.Forms.GroupBox gplOtherSetting;
		private System.Windows.Forms.Button btnColorSettingClose;
		private System.Windows.Forms.Button btnEnableItemClose;
	}
}