namespace MPI.Tester.Gui
{
    partial class frmItemSettingCALC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemSettingCALC));
            this.grpItemCondition = new System.Windows.Forms.GroupBox();
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.grpApplySetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.gBxGain = new System.Windows.Forms.GroupBox();
            this.dinGain = new DevComponents.Editors.DoubleInput();
            this.lblGain = new DevComponents.DotNetBar.LabelX();
            this.lblMulti = new DevComponents.DotNetBar.LabelX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbUseValB = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.dinValB = new DevComponents.Editors.DoubleInput();
            this.rdDeltaR = new System.Windows.Forms.RadioButton();
            this.rdSubtract = new System.Windows.Forms.RadioButton();
            this.lblCalcType = new DevComponents.DotNetBar.LabelX();
            this.lblAItem = new DevComponents.DotNetBar.LabelX();
            this.cmbCalcAddItemB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.rdAdd = new System.Windows.Forms.RadioButton();
            this.lblBItem = new DevComponents.DotNetBar.LabelX();
            this.rdDivide = new System.Windows.Forms.RadioButton();
            this.cmbCalcAddItemA = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.rdMultiple = new System.Windows.Forms.RadioButton();
            this.pnlCalcTitle = new System.Windows.Forms.Panel();
            this.lblCalcFunc = new DevComponents.DotNetBar.LabelX();
            this.lblResultName = new DevComponents.DotNetBar.LabelX();
            this.tbiNormal = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel3 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.rtbInput = new System.Windows.Forms.RichTextBox();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTestRun = new DevComponents.DotNetBar.ButtonX();
            this.btnCompile = new DevComponents.DotNetBar.ButtonX();
            this.tbiAdv = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControl2 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.chbUseValA = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.dinValA = new DevComponents.Editors.DoubleInput();
            this.grpItemCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
            this.grpApplySetting.SuspendLayout();
            this.gBxGain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinGain)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinValB)).BeginInit();
            this.pnlCalcTitle.SuspendLayout();
            this.superTabControlPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinValA)).BeginInit();
            this.SuspendLayout();
            // 
            // grpItemCondition
            // 
            this.grpItemCondition.Controls.Add(this.superTabControl1);
            resources.ApplyResources(this.grpItemCondition, "grpItemCondition");
            this.grpItemCondition.ForeColor = System.Drawing.Color.DimGray;
            this.grpItemCondition.Name = "grpItemCondition";
            this.grpItemCondition.TabStop = false;
            // 
            // superTabControl1
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.superTabControl1.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Controls.Add(this.superTabControlPanel3);
            this.superTabControl1.Controls.Add(this.superTabControl2);
            resources.ApplyResources(this.superTabControl1, "superTabControl1");
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tbiNormal,
            this.tbiAdv});
            this.superTabControl1.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.VisualStudio2008Dock;
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.grpApplySetting);
            resources.ApplyResources(this.superTabControlPanel1, "superTabControlPanel1");
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.TabItem = this.tbiNormal;
            // 
            // grpApplySetting
            // 
            this.grpApplySetting.BackColor = System.Drawing.Color.Transparent;
            this.grpApplySetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpApplySetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpApplySetting.Controls.Add(this.gBxGain);
            this.grpApplySetting.Controls.Add(this.groupBox1);
            this.grpApplySetting.Controls.Add(this.pnlCalcTitle);
            resources.ApplyResources(this.grpApplySetting, "grpApplySetting");
            this.grpApplySetting.DrawTitleBox = false;
            this.grpApplySetting.Name = "grpApplySetting";
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
            this.grpApplySetting.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpApplySetting.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpApplySetting.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpApplySetting.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpApplySetting.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpApplySetting.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpApplySetting.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // gBxGain
            // 
            this.gBxGain.Controls.Add(this.dinGain);
            this.gBxGain.Controls.Add(this.lblGain);
            this.gBxGain.Controls.Add(this.lblMulti);
            resources.ApplyResources(this.gBxGain, "gBxGain");
            this.gBxGain.Name = "gBxGain";
            this.gBxGain.TabStop = false;
            // 
            // dinGain
            // 
            // 
            // 
            // 
            this.dinGain.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinGain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinGain.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinGain.DisplayFormat = "0.#####";
            this.dinGain.ForeColor = System.Drawing.Color.Black;
            this.dinGain.Increment = 1D;
            resources.ApplyResources(this.dinGain, "dinGain");
            this.dinGain.Name = "dinGain";
            this.dinGain.ShowUpDown = true;
            this.dinGain.Value = 1D;
            // 
            // lblGain
            // 
            this.lblGain.BackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // 
            // 
            this.lblGain.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblGain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblGain, "lblGain");
            this.lblGain.ForeColor = System.Drawing.Color.Black;
            this.lblGain.Name = "lblGain";
            this.lblGain.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblMulti
            // 
            this.lblMulti.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMulti.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblMulti.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMulti, "lblMulti");
            this.lblMulti.ForeColor = System.Drawing.Color.Black;
            this.lblMulti.Name = "lblMulti";
            this.lblMulti.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbUseValA);
            this.groupBox1.Controls.Add(this.dinValA);
            this.groupBox1.Controls.Add(this.chbUseValB);
            this.groupBox1.Controls.Add(this.dinValB);
            this.groupBox1.Controls.Add(this.rdDeltaR);
            this.groupBox1.Controls.Add(this.rdSubtract);
            this.groupBox1.Controls.Add(this.lblCalcType);
            this.groupBox1.Controls.Add(this.lblAItem);
            this.groupBox1.Controls.Add(this.cmbCalcAddItemB);
            this.groupBox1.Controls.Add(this.rdAdd);
            this.groupBox1.Controls.Add(this.lblBItem);
            this.groupBox1.Controls.Add(this.rdDivide);
            this.groupBox1.Controls.Add(this.cmbCalcAddItemA);
            this.groupBox1.Controls.Add(this.rdMultiple);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chbUseValB
            // 
            // 
            // 
            // 
            this.chbUseValB.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chbUseValB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.chbUseValB, "chbUseValB");
            this.chbUseValB.Name = "chbUseValB";
            this.chbUseValB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // dinValB
            // 
            // 
            // 
            // 
            this.dinValB.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinValB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinValB.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinValB.DisplayFormat = "E3";
            this.dinValB.ForeColor = System.Drawing.Color.Black;
            this.dinValB.Increment = 1D;
            resources.ApplyResources(this.dinValB, "dinValB");
            this.dinValB.Name = "dinValB";
            this.dinValB.ShowUpDown = true;
            // 
            // rdDeltaR
            // 
            resources.ApplyResources(this.rdDeltaR, "rdDeltaR");
            this.rdDeltaR.Name = "rdDeltaR";
            this.rdDeltaR.UseVisualStyleBackColor = true;
            // 
            // rdSubtract
            // 
            resources.ApplyResources(this.rdSubtract, "rdSubtract");
            this.rdSubtract.Checked = true;
            this.rdSubtract.Name = "rdSubtract";
            this.rdSubtract.TabStop = true;
            this.rdSubtract.UseVisualStyleBackColor = true;
            // 
            // lblCalcType
            // 
            this.lblCalcType.BackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // 
            // 
            this.lblCalcType.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCalcType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCalcType, "lblCalcType");
            this.lblCalcType.ForeColor = System.Drawing.Color.Black;
            this.lblCalcType.Name = "lblCalcType";
            this.lblCalcType.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblAItem
            // 
            this.lblAItem.BackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // 
            // 
            this.lblAItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblAItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblAItem, "lblAItem");
            this.lblAItem.ForeColor = System.Drawing.Color.Black;
            this.lblAItem.Name = "lblAItem";
            this.lblAItem.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // cmbCalcAddItemB
            // 
            this.cmbCalcAddItemB.DisplayMember = "Text";
            this.cmbCalcAddItemB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCalcAddItemB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCalcAddItemB.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCalcAddItemB, "cmbCalcAddItemB");
            this.cmbCalcAddItemB.Name = "cmbCalcAddItemB";
            this.cmbCalcAddItemB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // rdAdd
            // 
            resources.ApplyResources(this.rdAdd, "rdAdd");
            this.rdAdd.Name = "rdAdd";
            this.rdAdd.UseVisualStyleBackColor = true;
            // 
            // lblBItem
            // 
            this.lblBItem.BackColor = System.Drawing.Color.LightSkyBlue;
            // 
            // 
            // 
            this.lblBItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblBItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblBItem, "lblBItem");
            this.lblBItem.ForeColor = System.Drawing.Color.Black;
            this.lblBItem.Name = "lblBItem";
            this.lblBItem.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // rdDivide
            // 
            resources.ApplyResources(this.rdDivide, "rdDivide");
            this.rdDivide.Name = "rdDivide";
            this.rdDivide.UseVisualStyleBackColor = true;
            // 
            // cmbCalcAddItemA
            // 
            this.cmbCalcAddItemA.DisplayMember = "Text";
            this.cmbCalcAddItemA.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCalcAddItemA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCalcAddItemA.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCalcAddItemA, "cmbCalcAddItemA");
            this.cmbCalcAddItemA.Name = "cmbCalcAddItemA";
            this.cmbCalcAddItemA.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // rdMultiple
            // 
            resources.ApplyResources(this.rdMultiple, "rdMultiple");
            this.rdMultiple.Name = "rdMultiple";
            this.rdMultiple.UseVisualStyleBackColor = true;
            // 
            // pnlCalcTitle
            // 
            this.pnlCalcTitle.Controls.Add(this.lblCalcFunc);
            this.pnlCalcTitle.Controls.Add(this.lblResultName);
            resources.ApplyResources(this.pnlCalcTitle, "pnlCalcTitle");
            this.pnlCalcTitle.Name = "pnlCalcTitle";
            // 
            // lblCalcFunc
            // 
            this.lblCalcFunc.BackColor = System.Drawing.Color.LightBlue;
            // 
            // 
            // 
            this.lblCalcFunc.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCalcFunc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCalcFunc, "lblCalcFunc");
            this.lblCalcFunc.ForeColor = System.Drawing.Color.Black;
            this.lblCalcFunc.Name = "lblCalcFunc";
            this.lblCalcFunc.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblResultName
            // 
            this.lblResultName.BackColor = System.Drawing.Color.SkyBlue;
            // 
            // 
            // 
            this.lblResultName.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblResultName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblResultName, "lblResultName");
            this.lblResultName.ForeColor = System.Drawing.Color.Black;
            this.lblResultName.Name = "lblResultName";
            this.lblResultName.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // tbiNormal
            // 
            this.tbiNormal.AttachedControl = this.superTabControlPanel1;
            this.tbiNormal.GlobalItem = false;
            this.tbiNormal.Name = "tbiNormal";
            resources.ApplyResources(this.tbiNormal, "tbiNormal");
            // 
            // superTabControlPanel3
            // 
            this.superTabControlPanel3.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.superTabControlPanel3, "superTabControlPanel3");
            this.superTabControlPanel3.Name = "superTabControlPanel3";
            this.superTabControlPanel3.TabItem = this.tbiAdv;
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.rtbInput);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.rtbOutput);
            // 
            // rtbInput
            // 
            resources.ApplyResources(this.rtbInput, "rtbInput");
            this.rtbInput.Name = "rtbInput";
            // 
            // rtbOutput
            // 
            resources.ApplyResources(this.rtbOutput, "rtbOutput");
            this.rtbOutput.Name = "rtbOutput";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTestRun);
            this.panel1.Controls.Add(this.btnCompile);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnTestRun
            // 
            this.btnTestRun.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTestRun.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnTestRun, "btnTestRun");
            this.btnTestRun.ForeColor = System.Drawing.Color.Black;
            this.btnTestRun.Name = "btnTestRun";
            this.btnTestRun.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnTestRun.Click += new System.EventHandler(this.btnTestRun_Click);
            // 
            // btnCompile
            // 
            this.btnCompile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCompile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnCompile, "btnCompile");
            this.btnCompile.ForeColor = System.Drawing.Color.Black;
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // tbiAdv
            // 
            this.tbiAdv.AttachedControl = this.superTabControlPanel3;
            this.tbiAdv.GlobalItem = false;
            this.tbiAdv.Name = "tbiAdv";
            resources.ApplyResources(this.tbiAdv, "tbiAdv");
            // 
            // superTabControl2
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl2.ControlBox.CloseBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            // 
            // 
            // 
            this.superTabControl2.ControlBox.MenuBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.superTabControl2.ControlBox.Name = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.superTabControl2.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl2.ControlBox.MenuBox,
            this.superTabControl2.ControlBox.CloseBox});
            resources.ApplyResources(this.superTabControl2, "superTabControl2");
            this.superTabControl2.Name = "superTabControl2";
            this.superTabControl2.ReorderTabsEnabled = true;
            this.superTabControl2.SelectedTabIndex = -1;
            // 
            // superTabItem1
            // 
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.Name = "superTabItem1";
            // 
            // superTabControlPanel2
            // 
            resources.ApplyResources(this.superTabControlPanel2, "superTabControlPanel2");
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.TabItem = this.superTabItem1;
            // 
            // chbUseValA
            // 
            // 
            // 
            // 
            this.chbUseValA.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chbUseValA.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.chbUseValA, "chbUseValA");
            this.chbUseValA.Name = "chbUseValA";
            this.chbUseValA.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // dinValA
            // 
            // 
            // 
            // 
            this.dinValA.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinValA.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinValA.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinValA.DisplayFormat = "E3";
            this.dinValA.ForeColor = System.Drawing.Color.Black;
            this.dinValA.Increment = 1D;
            resources.ApplyResources(this.dinValA, "dinValA");
            this.dinValA.Name = "dinValA";
            this.dinValA.ShowUpDown = true;
            // 
            // frmItemSettingCALC
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grpItemCondition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmItemSettingCALC";
            this.grpItemCondition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            this.grpApplySetting.ResumeLayout(false);
            this.gBxGain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinGain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinValB)).EndInit();
            this.pnlCalcTitle.ResumeLayout(false);
            this.superTabControlPanel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinValA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpItemCondition;
        private DevComponents.DotNetBar.Controls.GroupPanel grpApplySetting;
        private System.Windows.Forms.Panel pnlCalcTitle;
        private DevComponents.DotNetBar.LabelX lblCalcFunc;
        private DevComponents.DotNetBar.LabelX lblResultName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdDeltaR;
        private System.Windows.Forms.RadioButton rdSubtract;
        private DevComponents.DotNetBar.LabelX lblCalcType;
        private DevComponents.DotNetBar.LabelX lblAItem;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCalcAddItemB;
        private System.Windows.Forms.RadioButton rdAdd;
        private DevComponents.DotNetBar.LabelX lblBItem;
        private System.Windows.Forms.RadioButton rdDivide;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCalcAddItemA;
        private System.Windows.Forms.RadioButton rdMultiple;
        private DevComponents.Editors.DoubleInput dinGain;
        private DevComponents.DotNetBar.LabelX lblGain;
        private DevComponents.DotNetBar.LabelX lblMulti;
        private System.Windows.Forms.GroupBox gBxGain;
        private DevComponents.Editors.DoubleInput dinValB;
        private DevComponents.DotNetBar.Controls.CheckBoxX chbUseValB;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox rtbInput;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.SuperTabItem tbiAdv;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem tbiNormal;
        private DevComponents.DotNetBar.SuperTabControl superTabControl2;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.ButtonX btnTestRun;
        private DevComponents.DotNetBar.ButtonX btnCompile;
        private DevComponents.DotNetBar.Controls.CheckBoxX chbUseValA;
        private DevComponents.Editors.DoubleInput dinValA;

    }
}