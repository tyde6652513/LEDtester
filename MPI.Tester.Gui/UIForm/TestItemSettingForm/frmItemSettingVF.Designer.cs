namespace MPI.Tester.Gui
{
    partial class frmItemSettingVF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemSettingVF));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpChannel = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmbSelectedChannel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSelectedChannel = new DevComponents.DotNetBar.LabelX();
            this.grpApplySetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pnlRetest = new System.Windows.Forms.Panel();
            this.lblRetestThresholdVUnit = new DevComponents.DotNetBar.LabelX();
            this.dinRetestThresholdV = new DevComponents.Editors.DoubleInput();
            this.lblRetestThresholdV = new DevComponents.DotNetBar.LabelX();
            this.numRetestCount = new DevComponents.Editors.IntegerInput();
            this.lblRetestCount = new DevComponents.DotNetBar.LabelX();
            this.chkEnableRetest = new DevComponents.DotNetBar.Controls.CheckBoxX();
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
            this.dinForceDealy = new DevComponents.Editors.DoubleInput();
            this.lblWaitTimeUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlForceRange = new System.Windows.Forms.Panel();
            this.lblForceRange = new DevComponents.DotNetBar.LabelX();
            this.lblAutoForceRange = new DevComponents.DotNetBar.LabelX();
            this.cmbForceRange = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.grpMsrtSetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pnlMsrtForceValue = new System.Windows.Forms.Panel();
            this.chkIsEnableMsrtForceValue = new DevComponents.DotNetBar.Controls.CheckBoxX();
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
            this.grpItemCondition = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpChannel.SuspendLayout();
            this.grpApplySetting.SuspendLayout();
            this.pnlRetest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRetestThresholdV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetestCount)).BeginInit();
            this.pnlForceTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceTime)).BeginInit();
            this.pnlForceValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).BeginInit();
            this.pnlWaitTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDealy)).BeginInit();
            this.pnlForceRange.SuspendLayout();
            this.grpMsrtSetting.SuspendLayout();
            this.pnlMsrtForceValue.SuspendLayout();
            this.pnlMsrtClamp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).BeginInit();
            this.pnlMsrtRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtRange)).BeginInit();
            this.pnlFilterCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMsrtFilterCount)).BeginInit();
            this.pnlNPLC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinNPLC)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.grpChannel);
            this.splitContainer1.Panel1.Controls.Add(this.grpApplySetting);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpMsrtSetting);
            // 
            // grpChannel
            // 
            this.grpChannel.BackColor = System.Drawing.Color.Transparent;
            this.grpChannel.CanvasColor = System.Drawing.Color.Empty;
            this.grpChannel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpChannel.Controls.Add(this.cmbSelectedChannel);
            this.grpChannel.Controls.Add(this.lblSelectedChannel);
            resources.ApplyResources(this.grpChannel, "grpChannel");
            this.grpChannel.DrawTitleBox = false;
            this.grpChannel.Name = "grpChannel";
            // 
            // 
            // 
            this.grpChannel.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpChannel.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpChannel.Style.BackColorGradientAngle = 90;
            this.grpChannel.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpChannel.Style.BorderBottomWidth = 1;
            this.grpChannel.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpChannel.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpChannel.Style.BorderLeftWidth = 1;
            this.grpChannel.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpChannel.Style.BorderRightWidth = 1;
            this.grpChannel.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpChannel.Style.BorderTopWidth = 1;
            this.grpChannel.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpChannel.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpChannel.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpChannel.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpChannel.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpChannel.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpChannel.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // cmbSelectedChannel
            // 
            this.cmbSelectedChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectedChannel.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbSelectedChannel, "cmbSelectedChannel");
            this.cmbSelectedChannel.Name = "cmbSelectedChannel";
            this.cmbSelectedChannel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblSelectedChannel
            // 
            resources.ApplyResources(this.lblSelectedChannel, "lblSelectedChannel");
            this.lblSelectedChannel.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblSelectedChannel.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblSelectedChannel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSelectedChannel.ForeColor = System.Drawing.Color.Black;
            this.lblSelectedChannel.Name = "lblSelectedChannel";
            // 
            // grpApplySetting
            // 
            this.grpApplySetting.BackColor = System.Drawing.Color.Transparent;
            this.grpApplySetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpApplySetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpApplySetting.Controls.Add(this.pnlRetest);
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
            // pnlRetest
            // 
            this.pnlRetest.Controls.Add(this.lblRetestThresholdVUnit);
            this.pnlRetest.Controls.Add(this.dinRetestThresholdV);
            this.pnlRetest.Controls.Add(this.lblRetestThresholdV);
            this.pnlRetest.Controls.Add(this.numRetestCount);
            this.pnlRetest.Controls.Add(this.lblRetestCount);
            this.pnlRetest.Controls.Add(this.chkEnableRetest);
            resources.ApplyResources(this.pnlRetest, "pnlRetest");
            this.pnlRetest.Name = "pnlRetest";
            // 
            // lblRetestThresholdVUnit
            // 
            resources.ApplyResources(this.lblRetestThresholdVUnit, "lblRetestThresholdVUnit");
            this.lblRetestThresholdVUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblRetestThresholdVUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblRetestThresholdVUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblRetestThresholdVUnit.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblRetestThresholdVUnit.Name = "lblRetestThresholdVUnit";
            // 
            // dinRetestThresholdV
            // 
            // 
            // 
            // 
            this.dinRetestThresholdV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRetestThresholdV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRetestThresholdV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRetestThresholdV.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinRetestThresholdV, "dinRetestThresholdV");
            this.dinRetestThresholdV.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.dinRetestThresholdV.Increment = 1D;
            this.dinRetestThresholdV.MaxValue = 200D;
            this.dinRetestThresholdV.MinValue = 1D;
            this.dinRetestThresholdV.Name = "dinRetestThresholdV";
            this.dinRetestThresholdV.ShowUpDown = true;
            this.dinRetestThresholdV.Value = 8D;
            // 
            // lblRetestThresholdV
            // 
            resources.ApplyResources(this.lblRetestThresholdV, "lblRetestThresholdV");
            this.lblRetestThresholdV.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblRetestThresholdV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblRetestThresholdV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblRetestThresholdV.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblRetestThresholdV.Name = "lblRetestThresholdV";
            // 
            // numRetestCount
            // 
            // 
            // 
            // 
            this.numRetestCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numRetestCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numRetestCount.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numRetestCount.DisplayFormat = "0";
            resources.ApplyResources(this.numRetestCount, "numRetestCount");
            this.numRetestCount.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.numRetestCount.MaxValue = 20;
            this.numRetestCount.MinValue = 1;
            this.numRetestCount.Name = "numRetestCount";
            this.numRetestCount.ShowUpDown = true;
            this.numRetestCount.Value = 1;
            // 
            // lblRetestCount
            // 
            resources.ApplyResources(this.lblRetestCount, "lblRetestCount");
            this.lblRetestCount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblRetestCount.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblRetestCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblRetestCount.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblRetestCount.Name = "lblRetestCount";
            // 
            // chkEnableRetest
            // 
            // 
            // 
            // 
            this.chkEnableRetest.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEnableRetest.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableRetest.CheckSignSize = new System.Drawing.Size(17, 17);
            resources.ApplyResources(this.chkEnableRetest, "chkEnableRetest");
            this.chkEnableRetest.Name = "chkEnableRetest";
            this.chkEnableRetest.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkEnableRetest.CheckedChanged += new System.EventHandler(this.chkEnableRetest_CheckedChanged);
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
            this.dinForceValue.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinForceValue, "dinForceValue");
            this.dinForceValue.Increment = 1D;
            this.dinForceValue.MaxValue = 100D;
            this.dinForceValue.MinValue = 0D;
            this.dinForceValue.Name = "dinForceValue";
            this.dinForceValue.ShowUpDown = true;
            this.dinForceValue.Value = 1D;
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
            this.pnlWaitTime.Controls.Add(this.dinForceDealy);
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
            // dinForceDealy
            // 
            this.dinForceDealy.AutoResolveFreeTextEntries = false;
            // 
            // 
            // 
            this.dinForceDealy.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceDealy.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceDealy.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceDealy.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinForceDealy, "dinForceDealy");
            this.dinForceDealy.Increment = 0.5D;
            this.dinForceDealy.MaxValue = 90000D;
            this.dinForceDealy.MinValue = 0D;
            this.dinForceDealy.Name = "dinForceDealy";
            this.dinForceDealy.ShowUpDown = true;
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
            // grpMsrtSetting
            // 
            this.grpMsrtSetting.BackColor = System.Drawing.Color.Transparent;
            this.grpMsrtSetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpMsrtSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpMsrtSetting.Controls.Add(this.pnlMsrtForceValue);
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
            // pnlMsrtForceValue
            // 
            this.pnlMsrtForceValue.Controls.Add(this.chkIsEnableMsrtForceValue);
            resources.ApplyResources(this.pnlMsrtForceValue, "pnlMsrtForceValue");
            this.pnlMsrtForceValue.Name = "pnlMsrtForceValue";
            // 
            // chkIsEnableMsrtForceValue
            // 
            // 
            // 
            // 
            this.chkIsEnableMsrtForceValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableMsrtForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableMsrtForceValue.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableMsrtForceValue, "chkIsEnableMsrtForceValue");
            this.chkIsEnableMsrtForceValue.Name = "chkIsEnableMsrtForceValue";
            this.chkIsEnableMsrtForceValue.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkIsEnableMsrtForceValue.CheckedChanged += new System.EventHandler(this.chkIsEnableMsrtForceValue_CheckedChanged);
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
            this.dinMsrtClamp.DisplayFormat = "0.000";
            resources.ApplyResources(this.dinMsrtClamp, "dinMsrtClamp");
            this.dinMsrtClamp.Increment = 1D;
            this.dinMsrtClamp.MaxValue = 1000D;
            this.dinMsrtClamp.MinValue = 0D;
            this.dinMsrtClamp.Name = "dinMsrtClamp";
            this.dinMsrtClamp.ShowUpDown = true;
            this.dinMsrtClamp.Value = 100D;
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
            this.dinMsrtRange.DisplayFormat = "0.000";
            resources.ApplyResources(this.dinMsrtRange, "dinMsrtRange");
            this.dinMsrtRange.Increment = 1D;
            this.dinMsrtRange.MaxValue = 1000D;
            this.dinMsrtRange.MinValue = 0D;
            this.dinMsrtRange.Name = "dinMsrtRange";
            this.dinMsrtRange.ShowUpDown = true;
            this.dinMsrtRange.Value = 100D;
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
            // grpItemCondition
            // 
            this.grpItemCondition.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.grpItemCondition, "grpItemCondition");
            this.grpItemCondition.ForeColor = System.Drawing.Color.DimGray;
            this.grpItemCondition.Name = "grpItemCondition";
            this.grpItemCondition.TabStop = false;
            // 
            // frmItemSettingVF
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grpItemCondition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmItemSettingVF";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpChannel.ResumeLayout(false);
            this.grpChannel.PerformLayout();
            this.grpApplySetting.ResumeLayout(false);
            this.pnlRetest.ResumeLayout(false);
            this.pnlRetest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRetestThresholdV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetestCount)).EndInit();
            this.pnlForceTime.ResumeLayout(false);
            this.pnlForceTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceTime)).EndInit();
            this.pnlForceValue.ResumeLayout(false);
            this.pnlForceValue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).EndInit();
            this.pnlWaitTime.ResumeLayout(false);
            this.pnlWaitTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDealy)).EndInit();
            this.pnlForceRange.ResumeLayout(false);
            this.pnlForceRange.PerformLayout();
            this.grpMsrtSetting.ResumeLayout(false);
            this.pnlMsrtForceValue.ResumeLayout(false);
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
        private DevComponents.Editors.DoubleInput dinForceDealy;
        private DevComponents.DotNetBar.LabelX lblWaitTimeUnit;
        private System.Windows.Forms.Panel pnlForceRange;
        private DevComponents.DotNetBar.LabelX lblForceRange;
        private DevComponents.DotNetBar.LabelX lblAutoForceRange;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbForceRange;
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
        private DevComponents.DotNetBar.Controls.GroupPanel grpChannel;
        private System.Windows.Forms.Panel pnlMsrtForceValue;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableMsrtForceValue;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSelectedChannel;
        private DevComponents.DotNetBar.LabelX lblSelectedChannel;
        private System.Windows.Forms.Panel pnlRetest;
        private DevComponents.DotNetBar.LabelX lblRetestThresholdV;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableRetest;
        private DevComponents.Editors.DoubleInput dinRetestThresholdV;
        private DevComponents.DotNetBar.LabelX lblRetestThresholdVUnit;
        private DevComponents.Editors.IntegerInput numRetestCount;
        private DevComponents.DotNetBar.LabelX lblRetestCount;

    }
}