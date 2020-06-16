namespace MPI.Tester.Gui
{
    partial class frmItemSettingPOLAR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemSettingPOLAR));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpApplySetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pnlForceTime = new System.Windows.Forms.Panel();
            this.lblForceTime = new DevComponents.DotNetBar.LabelX();
            this.dinForceTime = new DevComponents.Editors.DoubleInput();
            this.lblForceTimeUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlForceValue = new System.Windows.Forms.Panel();
            this.lblForceValue = new DevComponents.DotNetBar.LabelX();
            this.dinForceValue = new DevComponents.Editors.DoubleInput();
            this.lblForceValueUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlWaitTime = new System.Windows.Forms.Panel();
            this.lblWaitTime = new DevComponents.DotNetBar.LabelX();
            this.dinForceDelay = new DevComponents.Editors.DoubleInput();
            this.lblWaitTimeUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlForceRange = new System.Windows.Forms.Panel();
            this.lblForceRange = new DevComponents.DotNetBar.LabelX();
            this.lblAutoForceRange = new DevComponents.DotNetBar.LabelX();
            this.cmbForceRange = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.grpMsrtSetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pnlMsrtClamp = new System.Windows.Forms.Panel();
            this.lblMsrtClamp = new DevComponents.DotNetBar.LabelX();
            this.dinMsrtClamp = new DevComponents.Editors.DoubleInput();
            this.lblMsrtClampUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlMsrtRange = new System.Windows.Forms.Panel();
            this.lblMsrtRange = new DevComponents.DotNetBar.LabelX();
            this.dinMsrtRange = new DevComponents.Editors.DoubleInput();
            this.lblMsrtRangeUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlFilterCount = new System.Windows.Forms.Panel();
            this.lblMsrtFilterCount = new DevComponents.DotNetBar.LabelX();
            this.numMsrtFilterCount = new DevComponents.Editors.IntegerInput();
            this.pnlNPLC = new System.Windows.Forms.Panel();
            this.lblNPLC = new DevComponents.DotNetBar.LabelX();
            this.dinNPLC = new DevComponents.Editors.DoubleInput();
            this.grpThreshold = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblThreshold = new DevComponents.DotNetBar.LabelX();
            this.dinThreshold = new DevComponents.Editors.DoubleInput();
            this.lblThresholdUnit = new DevComponents.DotNetBar.LabelX();
            this.grpItemCondition = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpApplySetting.SuspendLayout();
            this.pnlForceTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceTime)).BeginInit();
            this.pnlForceValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).BeginInit();
            this.pnlWaitTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDelay)).BeginInit();
            this.pnlForceRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.grpMsrtSetting.SuspendLayout();
            this.pnlMsrtClamp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).BeginInit();
            this.pnlMsrtRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtRange)).BeginInit();
            this.pnlFilterCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMsrtFilterCount)).BeginInit();
            this.pnlNPLC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinNPLC)).BeginInit();
            this.grpThreshold.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThreshold)).BeginInit();
            this.grpItemCondition.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpApplySetting);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // grpApplySetting
            // 
            this.grpApplySetting.BackColor = System.Drawing.Color.Transparent;
            this.grpApplySetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpApplySetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpApplySetting.Controls.Add(this.pnlForceTime);
            this.grpApplySetting.Controls.Add(this.pnlForceValue);
            this.grpApplySetting.Controls.Add(this.pnlWaitTime);
            this.grpApplySetting.Controls.Add(this.pnlForceRange);
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
            // pnlForceTime
            // 
            this.pnlForceTime.Controls.Add(this.lblForceTime);
            this.pnlForceTime.Controls.Add(this.dinForceTime);
            this.pnlForceTime.Controls.Add(this.lblForceTimeUnit);
            resources.ApplyResources(this.pnlForceTime, "pnlForceTime");
            this.pnlForceTime.Name = "pnlForceTime";
            // 
            // lblForceTime
            // 
            resources.ApplyResources(this.lblForceTime, "lblForceTime");
            this.lblForceTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblForceTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceTime.ForeColor = System.Drawing.Color.Black;
            this.lblForceTime.Name = "lblForceTime";
            // 
            // dinForceTime
            // 
            // 
            // 
            // 
            this.dinForceTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceTime.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceTime.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinForceTime, "dinForceTime");
            this.dinForceTime.Increment = 0.5D;
            this.dinForceTime.MaxValue = 1000000D;
            this.dinForceTime.MinValue = 0D;
            this.dinForceTime.Name = "dinForceTime";
            this.dinForceTime.ShowUpDown = true;
            this.dinForceTime.Value = 1D;
            // 
            // lblForceTimeUnit
            // 
            resources.ApplyResources(this.lblForceTimeUnit, "lblForceTimeUnit");
            this.lblForceTimeUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceTimeUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblForceTimeUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceTimeUnit.ForeColor = System.Drawing.Color.Black;
            this.lblForceTimeUnit.Name = "lblForceTimeUnit";
            // 
            // pnlForceValue
            // 
            this.pnlForceValue.Controls.Add(this.lblForceValue);
            this.pnlForceValue.Controls.Add(this.dinForceValue);
            this.pnlForceValue.Controls.Add(this.lblForceValueUnit);
            resources.ApplyResources(this.pnlForceValue, "pnlForceValue");
            this.pnlForceValue.Name = "pnlForceValue";
            // 
            // lblForceValue
            // 
            resources.ApplyResources(this.lblForceValue, "lblForceValue");
            this.lblForceValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceValue.ForeColor = System.Drawing.Color.Black;
            this.lblForceValue.Name = "lblForceValue";
            // 
            // dinForceValue
            // 
            // 
            // 
            // 
            this.dinForceValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceValue.DisplayFormat = "0.0000";
            resources.ApplyResources(this.dinForceValue, "dinForceValue");
            this.dinForceValue.Increment = 1D;
            this.dinForceValue.MaxValue = 10000D;
            this.dinForceValue.MinValue = 0D;
            this.dinForceValue.Name = "dinForceValue";
            this.dinForceValue.ShowUpDown = true;
            this.dinForceValue.Value = 0.01D;
            // 
            // lblForceValueUnit
            // 
            resources.ApplyResources(this.lblForceValueUnit, "lblForceValueUnit");
            this.lblForceValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblForceValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblForceValueUnit.Name = "lblForceValueUnit";
            // 
            // pnlWaitTime
            // 
            this.pnlWaitTime.Controls.Add(this.lblWaitTime);
            this.pnlWaitTime.Controls.Add(this.dinForceDelay);
            this.pnlWaitTime.Controls.Add(this.lblWaitTimeUnit);
            resources.ApplyResources(this.pnlWaitTime, "pnlWaitTime");
            this.pnlWaitTime.Name = "pnlWaitTime";
            // 
            // lblWaitTime
            // 
            resources.ApplyResources(this.lblWaitTime, "lblWaitTime");
            this.lblWaitTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblWaitTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblWaitTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblWaitTime.ForeColor = System.Drawing.Color.Black;
            this.lblWaitTime.Name = "lblWaitTime";
            // 
            // dinForceDelay
            // 
            this.dinForceDelay.AutoResolveFreeTextEntries = false;
            // 
            // 
            // 
            this.dinForceDelay.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceDelay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceDelay.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceDelay.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinForceDelay, "dinForceDelay");
            this.dinForceDelay.Increment = 0.5D;
            this.dinForceDelay.MaxValue = 90000D;
            this.dinForceDelay.MinValue = 0D;
            this.dinForceDelay.Name = "dinForceDelay";
            this.dinForceDelay.ShowUpDown = true;
            // 
            // lblWaitTimeUnit
            // 
            resources.ApplyResources(this.lblWaitTimeUnit, "lblWaitTimeUnit");
            this.lblWaitTimeUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblWaitTimeUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblWaitTimeUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblWaitTimeUnit.ForeColor = System.Drawing.Color.Black;
            this.lblWaitTimeUnit.Name = "lblWaitTimeUnit";
            // 
            // pnlForceRange
            // 
            this.pnlForceRange.Controls.Add(this.lblForceRange);
            this.pnlForceRange.Controls.Add(this.lblAutoForceRange);
            this.pnlForceRange.Controls.Add(this.cmbForceRange);
            resources.ApplyResources(this.pnlForceRange, "pnlForceRange");
            this.pnlForceRange.Name = "pnlForceRange";
            // 
            // lblForceRange
            // 
            resources.ApplyResources(this.lblForceRange, "lblForceRange");
            this.lblForceRange.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceRange.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblForceRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceRange.ForeColor = System.Drawing.Color.Black;
            this.lblForceRange.Name = "lblForceRange";
            // 
            // lblAutoForceRange
            // 
            resources.ApplyResources(this.lblAutoForceRange, "lblAutoForceRange");
            this.lblAutoForceRange.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblAutoForceRange.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblAutoForceRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblAutoForceRange.ForeColor = System.Drawing.Color.DimGray;
            this.lblAutoForceRange.Name = "lblAutoForceRange";
            this.lblAutoForceRange.SingleLineColor = System.Drawing.Color.DarkSlateGray;
            // 
            // cmbForceRange
            // 
            this.cmbForceRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbForceRange.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbForceRange, "cmbForceRange");
            this.cmbForceRange.Name = "cmbForceRange";
            this.cmbForceRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.grpMsrtSetting);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grpThreshold);
            // 
            // grpMsrtSetting
            // 
            this.grpMsrtSetting.BackColor = System.Drawing.Color.Transparent;
            this.grpMsrtSetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpMsrtSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpMsrtSetting.Controls.Add(this.pnlMsrtClamp);
            this.grpMsrtSetting.Controls.Add(this.pnlMsrtRange);
            this.grpMsrtSetting.Controls.Add(this.pnlFilterCount);
            this.grpMsrtSetting.Controls.Add(this.pnlNPLC);
            resources.ApplyResources(this.grpMsrtSetting, "grpMsrtSetting");
            this.grpMsrtSetting.DrawTitleBox = false;
            this.grpMsrtSetting.IsShadowEnabled = true;
            this.grpMsrtSetting.Name = "grpMsrtSetting";
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
            this.grpMsrtSetting.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpMsrtSetting.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpMsrtSetting.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpMsrtSetting.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpMsrtSetting.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpMsrtSetting.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpMsrtSetting.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // pnlMsrtClamp
            // 
            this.pnlMsrtClamp.Controls.Add(this.lblMsrtClamp);
            this.pnlMsrtClamp.Controls.Add(this.dinMsrtClamp);
            this.pnlMsrtClamp.Controls.Add(this.lblMsrtClampUnit);
            resources.ApplyResources(this.pnlMsrtClamp, "pnlMsrtClamp");
            this.pnlMsrtClamp.Name = "pnlMsrtClamp";
            // 
            // lblMsrtClamp
            // 
            resources.ApplyResources(this.lblMsrtClamp, "lblMsrtClamp");
            this.lblMsrtClamp.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtClamp.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblMsrtClamp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtClamp.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtClamp.Name = "lblMsrtClamp";
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
            resources.ApplyResources(this.dinMsrtClamp, "dinMsrtClamp");
            this.dinMsrtClamp.Increment = 1D;
            this.dinMsrtClamp.MaxValue = 285D;
            this.dinMsrtClamp.MinValue = 1D;
            this.dinMsrtClamp.Name = "dinMsrtClamp";
            this.dinMsrtClamp.ShowUpDown = true;
            this.dinMsrtClamp.Value = 8D;
            // 
            // lblMsrtClampUnit
            // 
            resources.ApplyResources(this.lblMsrtClampUnit, "lblMsrtClampUnit");
            this.lblMsrtClampUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtClampUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblMsrtClampUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtClampUnit.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtClampUnit.Name = "lblMsrtClampUnit";
            // 
            // pnlMsrtRange
            // 
            this.pnlMsrtRange.Controls.Add(this.lblMsrtRange);
            this.pnlMsrtRange.Controls.Add(this.dinMsrtRange);
            this.pnlMsrtRange.Controls.Add(this.lblMsrtRangeUnit);
            resources.ApplyResources(this.pnlMsrtRange, "pnlMsrtRange");
            this.pnlMsrtRange.Name = "pnlMsrtRange";
            // 
            // lblMsrtRange
            // 
            resources.ApplyResources(this.lblMsrtRange, "lblMsrtRange");
            this.lblMsrtRange.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtRange.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblMsrtRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtRange.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtRange.Name = "lblMsrtRange";
            // 
            // dinMsrtRange
            // 
            // 
            // 
            // 
            this.dinMsrtRange.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinMsrtRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinMsrtRange.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinMsrtRange.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinMsrtRange, "dinMsrtRange");
            this.dinMsrtRange.Increment = 1D;
            this.dinMsrtRange.MaxValue = 285D;
            this.dinMsrtRange.MinValue = 1D;
            this.dinMsrtRange.Name = "dinMsrtRange";
            this.dinMsrtRange.ShowUpDown = true;
            this.dinMsrtRange.Value = 8D;
            // 
            // lblMsrtRangeUnit
            // 
            resources.ApplyResources(this.lblMsrtRangeUnit, "lblMsrtRangeUnit");
            this.lblMsrtRangeUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtRangeUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblMsrtRangeUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtRangeUnit.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtRangeUnit.Name = "lblMsrtRangeUnit";
            // 
            // pnlFilterCount
            // 
            this.pnlFilterCount.Controls.Add(this.lblMsrtFilterCount);
            this.pnlFilterCount.Controls.Add(this.numMsrtFilterCount);
            resources.ApplyResources(this.pnlFilterCount, "pnlFilterCount");
            this.pnlFilterCount.Name = "pnlFilterCount";
            // 
            // lblMsrtFilterCount
            // 
            resources.ApplyResources(this.lblMsrtFilterCount, "lblMsrtFilterCount");
            this.lblMsrtFilterCount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtFilterCount.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblMsrtFilterCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtFilterCount.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtFilterCount.Name = "lblMsrtFilterCount";
            // 
            // numMsrtFilterCount
            // 
            // 
            // 
            // 
            this.numMsrtFilterCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numMsrtFilterCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numMsrtFilterCount.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numMsrtFilterCount.DisplayFormat = "0";
            resources.ApplyResources(this.numMsrtFilterCount, "numMsrtFilterCount");
            this.numMsrtFilterCount.MaxValue = 200;
            this.numMsrtFilterCount.MinValue = 1;
            this.numMsrtFilterCount.Name = "numMsrtFilterCount";
            this.numMsrtFilterCount.ShowUpDown = true;
            this.numMsrtFilterCount.Value = 1;
            // 
            // pnlNPLC
            // 
            this.pnlNPLC.Controls.Add(this.lblNPLC);
            this.pnlNPLC.Controls.Add(this.dinNPLC);
            resources.ApplyResources(this.pnlNPLC, "pnlNPLC");
            this.pnlNPLC.Name = "pnlNPLC";
            // 
            // lblNPLC
            // 
            resources.ApplyResources(this.lblNPLC, "lblNPLC");
            this.lblNPLC.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblNPLC.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblNPLC.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblNPLC.ForeColor = System.Drawing.Color.Black;
            this.lblNPLC.Name = "lblNPLC";
            // 
            // dinNPLC
            // 
            // 
            // 
            // 
            this.dinNPLC.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinNPLC.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinNPLC.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinNPLC.DisplayFormat = "0.000";
            resources.ApplyResources(this.dinNPLC, "dinNPLC");
            this.dinNPLC.Increment = 0.01D;
            this.dinNPLC.MaxValue = 1D;
            this.dinNPLC.MinValue = 0.001D;
            this.dinNPLC.Name = "dinNPLC";
            this.dinNPLC.ShowUpDown = true;
            this.dinNPLC.Value = 0.01D;
            // 
            // grpThreshold
            // 
            this.grpThreshold.BackColor = System.Drawing.Color.Transparent;
            this.grpThreshold.CanvasColor = System.Drawing.Color.Empty;
            this.grpThreshold.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpThreshold.Controls.Add(this.panel1);
            resources.ApplyResources(this.grpThreshold, "grpThreshold");
            this.grpThreshold.DrawTitleBox = false;
            this.grpThreshold.IsShadowEnabled = true;
            this.grpThreshold.Name = "grpThreshold";
            // 
            // 
            // 
            this.grpThreshold.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpThreshold.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpThreshold.Style.BackColorGradientAngle = 90;
            this.grpThreshold.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpThreshold.Style.BorderBottomWidth = 1;
            this.grpThreshold.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpThreshold.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpThreshold.Style.BorderLeftWidth = 1;
            this.grpThreshold.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpThreshold.Style.BorderRightWidth = 1;
            this.grpThreshold.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpThreshold.Style.BorderTopWidth = 1;
            this.grpThreshold.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpThreshold.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpThreshold.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpThreshold.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpThreshold.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpThreshold.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpThreshold.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblThreshold);
            this.panel1.Controls.Add(this.dinThreshold);
            this.panel1.Controls.Add(this.lblThresholdUnit);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // lblThreshold
            // 
            resources.ApplyResources(this.lblThreshold, "lblThreshold");
            this.lblThreshold.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblThreshold.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblThreshold.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblThreshold.ForeColor = System.Drawing.Color.Black;
            this.lblThreshold.Name = "lblThreshold";
            // 
            // dinThreshold
            // 
            // 
            // 
            // 
            this.dinThreshold.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinThreshold.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinThreshold.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinThreshold.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinThreshold, "dinThreshold");
            this.dinThreshold.Increment = 1D;
            this.dinThreshold.MaxValue = 285D;
            this.dinThreshold.MinValue = 1D;
            this.dinThreshold.Name = "dinThreshold";
            this.dinThreshold.ShowUpDown = true;
            this.dinThreshold.Value = 8D;
            // 
            // lblThresholdUnit
            // 
            resources.ApplyResources(this.lblThresholdUnit, "lblThresholdUnit");
            this.lblThresholdUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblThresholdUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblThresholdUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblThresholdUnit.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdUnit.Name = "lblThresholdUnit";
            // 
            // grpItemCondition
            // 
            this.grpItemCondition.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.grpItemCondition, "grpItemCondition");
            this.grpItemCondition.ForeColor = System.Drawing.Color.DimGray;
            this.grpItemCondition.Name = "grpItemCondition";
            this.grpItemCondition.TabStop = false;
            // 
            // frmItemSettingPOLAR
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grpItemCondition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmItemSettingPOLAR";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpApplySetting.ResumeLayout(false);
            this.pnlForceTime.ResumeLayout(false);
            this.pnlForceTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceTime)).EndInit();
            this.pnlForceValue.ResumeLayout(false);
            this.pnlForceValue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).EndInit();
            this.pnlWaitTime.ResumeLayout(false);
            this.pnlWaitTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDelay)).EndInit();
            this.pnlForceRange.ResumeLayout(false);
            this.pnlForceRange.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.grpMsrtSetting.ResumeLayout(false);
            this.pnlMsrtClamp.ResumeLayout(false);
            this.pnlMsrtClamp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).EndInit();
            this.pnlMsrtRange.ResumeLayout(false);
            this.pnlMsrtRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtRange)).EndInit();
            this.pnlFilterCount.ResumeLayout(false);
            this.pnlFilterCount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMsrtFilterCount)).EndInit();
            this.pnlNPLC.ResumeLayout(false);
            this.pnlNPLC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinNPLC)).EndInit();
            this.grpThreshold.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThreshold)).EndInit();
            this.grpItemCondition.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpItemCondition;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.Controls.GroupPanel grpApplySetting;
        private System.Windows.Forms.Panel pnlForceTime;
        private DevComponents.DotNetBar.LabelX lblForceTime;
        private DevComponents.Editors.DoubleInput dinForceTime;
        private DevComponents.DotNetBar.LabelX lblForceTimeUnit;
        private System.Windows.Forms.Panel pnlForceValue;
        private DevComponents.DotNetBar.LabelX lblForceValue;
        private DevComponents.Editors.DoubleInput dinForceValue;
        private DevComponents.DotNetBar.LabelX lblForceValueUnit;
        private System.Windows.Forms.Panel pnlWaitTime;
        private DevComponents.DotNetBar.LabelX lblWaitTime;
        private DevComponents.Editors.DoubleInput dinForceDelay;
        private DevComponents.DotNetBar.LabelX lblWaitTimeUnit;
        private System.Windows.Forms.Panel pnlForceRange;
        private DevComponents.DotNetBar.LabelX lblForceRange;
        private DevComponents.DotNetBar.LabelX lblAutoForceRange;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbForceRange;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevComponents.DotNetBar.Controls.GroupPanel grpMsrtSetting;
        private System.Windows.Forms.Panel pnlMsrtClamp;
        private DevComponents.DotNetBar.LabelX lblMsrtClamp;
        private DevComponents.Editors.DoubleInput dinMsrtClamp;
        private DevComponents.DotNetBar.LabelX lblMsrtClampUnit;
        private System.Windows.Forms.Panel pnlMsrtRange;
        private DevComponents.DotNetBar.LabelX lblMsrtRange;
        private DevComponents.Editors.DoubleInput dinMsrtRange;
        private DevComponents.DotNetBar.LabelX lblMsrtRangeUnit;
        private System.Windows.Forms.Panel pnlFilterCount;
        private DevComponents.DotNetBar.LabelX lblMsrtFilterCount;
        private DevComponents.Editors.IntegerInput numMsrtFilterCount;
        private System.Windows.Forms.Panel pnlNPLC;
        private DevComponents.DotNetBar.LabelX lblNPLC;
        private DevComponents.Editors.DoubleInput dinNPLC;
        private DevComponents.DotNetBar.Controls.GroupPanel grpThreshold;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.LabelX lblThreshold;
        private DevComponents.Editors.DoubleInput dinThreshold;
        private DevComponents.DotNetBar.LabelX lblThresholdUnit;

    }
}