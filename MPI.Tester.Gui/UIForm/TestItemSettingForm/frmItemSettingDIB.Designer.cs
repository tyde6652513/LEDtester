namespace MPI.Tester.Gui
{
    partial class frmItemSettingDIB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemSettingDIB));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpApplySetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pnlScanCount = new System.Windows.Forms.Panel();
            this.lblScanCount = new DevComponents.DotNetBar.LabelX();
            this.dinScanCount = new DevComponents.Editors.DoubleInput();
            this.lblScanCountUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlFilterSpec = new System.Windows.Forms.Panel();
            this.lblFilterSpec = new DevComponents.DotNetBar.LabelX();
            this.dinFilterSpec = new DevComponents.Editors.DoubleInput();
            this.lblFilterSpecUnit = new DevComponents.DotNetBar.LabelX();
            this.chkIsOnlyJuadgeSerious = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.pnlTargetItem = new System.Windows.Forms.Panel();
            this.lblTargetItem = new DevComponents.DotNetBar.LabelX();
            this.cmbTargetItemA = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.pnlFilterBase = new System.Windows.Forms.Panel();
            this.lblFilterBase = new DevComponents.DotNetBar.LabelX();
            this.dinFilterBase = new DevComponents.Editors.DoubleInput();
            this.lblFilterBaseUnit = new DevComponents.DotNetBar.LabelX();
            this.grpMsrtSetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pnlDeltaVolt = new System.Windows.Forms.Panel();
            this.lblDeltaVolt = new DevComponents.DotNetBar.LabelX();
            this.dinMsrtClamp = new DevComponents.Editors.DoubleInput();
            this.lblDeltaVoltUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlBaseLine = new System.Windows.Forms.Panel();
            this.lblBaseLine = new DevComponents.DotNetBar.LabelX();
            this.dinBaseLine = new DevComponents.Editors.DoubleInput();
            this.lblBaseLineUnit = new DevComponents.DotNetBar.LabelX();
            this.grpItemCondition = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpApplySetting.SuspendLayout();
            this.pnlScanCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinScanCount)).BeginInit();
            this.pnlFilterSpec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinFilterSpec)).BeginInit();
            this.pnlTargetItem.SuspendLayout();
            this.pnlFilterBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinFilterBase)).BeginInit();
            this.grpMsrtSetting.SuspendLayout();
            this.pnlDeltaVolt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).BeginInit();
            this.pnlBaseLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinBaseLine)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.grpMsrtSetting);
            // 
            // grpApplySetting
            // 
            this.grpApplySetting.BackColor = System.Drawing.Color.Transparent;
            this.grpApplySetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpApplySetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpApplySetting.Controls.Add(this.pnlScanCount);
            this.grpApplySetting.Controls.Add(this.pnlFilterSpec);
            this.grpApplySetting.Controls.Add(this.chkIsOnlyJuadgeSerious);
            this.grpApplySetting.Controls.Add(this.pnlTargetItem);
            this.grpApplySetting.Controls.Add(this.pnlFilterBase);
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
            // pnlScanCount
            // 
            this.pnlScanCount.Controls.Add(this.lblScanCount);
            this.pnlScanCount.Controls.Add(this.dinScanCount);
            this.pnlScanCount.Controls.Add(this.lblScanCountUnit);
            resources.ApplyResources(this.pnlScanCount, "pnlScanCount");
            this.pnlScanCount.Name = "pnlScanCount";
            // 
            // lblScanCount
            // 
            resources.ApplyResources(this.lblScanCount, "lblScanCount");
            this.lblScanCount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblScanCount.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblScanCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblScanCount.ForeColor = System.Drawing.Color.Black;
            this.lblScanCount.Name = "lblScanCount";
            // 
            // dinScanCount
            // 
            // 
            // 
            // 
            this.dinScanCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinScanCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinScanCount.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinScanCount.DisplayFormat = "0";
            resources.ApplyResources(this.dinScanCount, "dinScanCount");
            this.dinScanCount.Increment = 1D;
            this.dinScanCount.MaxValue = 1000D;
            this.dinScanCount.MinValue = 0D;
            this.dinScanCount.Name = "dinScanCount";
            this.dinScanCount.ShowUpDown = true;
            this.dinScanCount.Value = 100D;
            // 
            // lblScanCountUnit
            // 
            resources.ApplyResources(this.lblScanCountUnit, "lblScanCountUnit");
            this.lblScanCountUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblScanCountUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblScanCountUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblScanCountUnit.ForeColor = System.Drawing.Color.Black;
            this.lblScanCountUnit.Name = "lblScanCountUnit";
            // 
            // pnlFilterSpec
            // 
            this.pnlFilterSpec.Controls.Add(this.lblFilterSpec);
            this.pnlFilterSpec.Controls.Add(this.dinFilterSpec);
            this.pnlFilterSpec.Controls.Add(this.lblFilterSpecUnit);
            resources.ApplyResources(this.pnlFilterSpec, "pnlFilterSpec");
            this.pnlFilterSpec.Name = "pnlFilterSpec";
            // 
            // lblFilterSpec
            // 
            resources.ApplyResources(this.lblFilterSpec, "lblFilterSpec");
            this.lblFilterSpec.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblFilterSpec.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblFilterSpec.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFilterSpec.ForeColor = System.Drawing.Color.Black;
            this.lblFilterSpec.Name = "lblFilterSpec";
            // 
            // dinFilterSpec
            // 
            // 
            // 
            // 
            this.dinFilterSpec.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinFilterSpec.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinFilterSpec.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinFilterSpec.DisplayFormat = "0.000";
            resources.ApplyResources(this.dinFilterSpec, "dinFilterSpec");
            this.dinFilterSpec.Increment = 0.01D;
            this.dinFilterSpec.MaxValue = 0.2D;
            this.dinFilterSpec.MinValue = 0.01D;
            this.dinFilterSpec.Name = "dinFilterSpec";
            this.dinFilterSpec.ShowUpDown = true;
            this.dinFilterSpec.Value = 0.05D;
            // 
            // lblFilterSpecUnit
            // 
            resources.ApplyResources(this.lblFilterSpecUnit, "lblFilterSpecUnit");
            this.lblFilterSpecUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblFilterSpecUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblFilterSpecUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFilterSpecUnit.ForeColor = System.Drawing.Color.Black;
            this.lblFilterSpecUnit.Name = "lblFilterSpecUnit";
            // 
            // chkIsOnlyJuadgeSerious
            // 
            // 
            // 
            // 
            this.chkIsOnlyJuadgeSerious.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsOnlyJuadgeSerious.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsOnlyJuadgeSerious.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsOnlyJuadgeSerious, "chkIsOnlyJuadgeSerious");
            this.chkIsOnlyJuadgeSerious.Name = "chkIsOnlyJuadgeSerious";
            this.chkIsOnlyJuadgeSerious.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkIsOnlyJuadgeSerious.TextColor = System.Drawing.Color.DarkBlue;
            // 
            // pnlTargetItem
            // 
            this.pnlTargetItem.Controls.Add(this.lblTargetItem);
            this.pnlTargetItem.Controls.Add(this.cmbTargetItemA);
            resources.ApplyResources(this.pnlTargetItem, "pnlTargetItem");
            this.pnlTargetItem.Name = "pnlTargetItem";
            // 
            // lblTargetItem
            // 
            this.lblTargetItem.BackColor = System.Drawing.Color.LightBlue;
            // 
            // 
            // 
            this.lblTargetItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblTargetItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblTargetItem, "lblTargetItem");
            this.lblTargetItem.ForeColor = System.Drawing.Color.Black;
            this.lblTargetItem.Name = "lblTargetItem";
            this.lblTargetItem.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // cmbTargetItemA
            // 
            this.cmbTargetItemA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetItemA.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbTargetItemA, "cmbTargetItemA");
            this.cmbTargetItemA.Name = "cmbTargetItemA";
            this.cmbTargetItemA.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // pnlFilterBase
            // 
            this.pnlFilterBase.Controls.Add(this.lblFilterBase);
            this.pnlFilterBase.Controls.Add(this.dinFilterBase);
            this.pnlFilterBase.Controls.Add(this.lblFilterBaseUnit);
            resources.ApplyResources(this.pnlFilterBase, "pnlFilterBase");
            this.pnlFilterBase.Name = "pnlFilterBase";
            // 
            // lblFilterBase
            // 
            resources.ApplyResources(this.lblFilterBase, "lblFilterBase");
            this.lblFilterBase.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblFilterBase.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblFilterBase.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFilterBase.ForeColor = System.Drawing.Color.Black;
            this.lblFilterBase.Name = "lblFilterBase";
            // 
            // dinFilterBase
            // 
            // 
            // 
            // 
            this.dinFilterBase.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinFilterBase.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinFilterBase.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinFilterBase.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinFilterBase, "dinFilterBase");
            this.dinFilterBase.Increment = 0.01D;
            this.dinFilterBase.MaxValue = 10D;
            this.dinFilterBase.MinValue = 0D;
            this.dinFilterBase.Name = "dinFilterBase";
            this.dinFilterBase.ShowUpDown = true;
            this.dinFilterBase.Value = 1.9D;
            // 
            // lblFilterBaseUnit
            // 
            resources.ApplyResources(this.lblFilterBaseUnit, "lblFilterBaseUnit");
            this.lblFilterBaseUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblFilterBaseUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblFilterBaseUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFilterBaseUnit.ForeColor = System.Drawing.Color.Black;
            this.lblFilterBaseUnit.Name = "lblFilterBaseUnit";
            // 
            // grpMsrtSetting
            // 
            this.grpMsrtSetting.BackColor = System.Drawing.Color.Transparent;
            this.grpMsrtSetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpMsrtSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpMsrtSetting.Controls.Add(this.pnlDeltaVolt);
            this.grpMsrtSetting.Controls.Add(this.pnlBaseLine);
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
            // pnlDeltaVolt
            // 
            this.pnlDeltaVolt.Controls.Add(this.lblDeltaVolt);
            this.pnlDeltaVolt.Controls.Add(this.dinMsrtClamp);
            this.pnlDeltaVolt.Controls.Add(this.lblDeltaVoltUnit);
            resources.ApplyResources(this.pnlDeltaVolt, "pnlDeltaVolt");
            this.pnlDeltaVolt.Name = "pnlDeltaVolt";
            // 
            // lblDeltaVolt
            // 
            resources.ApplyResources(this.lblDeltaVolt, "lblDeltaVolt");
            this.lblDeltaVolt.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDeltaVolt.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDeltaVolt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDeltaVolt.ForeColor = System.Drawing.Color.Black;
            this.lblDeltaVolt.Name = "lblDeltaVolt";
            // 
            // dinMsrtClamp
            // 
            // 
            // 
            // 
            this.dinMsrtClamp.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinMsrtClamp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinMsrtClamp.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinMsrtClamp.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinMsrtClamp, "dinMsrtClamp");
            this.dinMsrtClamp.Increment = 1D;
            this.dinMsrtClamp.MaxValue = 0.1D;
            this.dinMsrtClamp.MinValue = 0.02D;
            this.dinMsrtClamp.Name = "dinMsrtClamp";
            this.dinMsrtClamp.ShowUpDown = true;
            this.dinMsrtClamp.Value = 0.05D;
            // 
            // lblDeltaVoltUnit
            // 
            resources.ApplyResources(this.lblDeltaVoltUnit, "lblDeltaVoltUnit");
            this.lblDeltaVoltUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDeltaVoltUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDeltaVoltUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDeltaVoltUnit.ForeColor = System.Drawing.Color.Black;
            this.lblDeltaVoltUnit.Name = "lblDeltaVoltUnit";
            // 
            // pnlBaseLine
            // 
            this.pnlBaseLine.Controls.Add(this.lblBaseLine);
            this.pnlBaseLine.Controls.Add(this.dinBaseLine);
            this.pnlBaseLine.Controls.Add(this.lblBaseLineUnit);
            resources.ApplyResources(this.pnlBaseLine, "pnlBaseLine");
            this.pnlBaseLine.Name = "pnlBaseLine";
            // 
            // lblBaseLine
            // 
            resources.ApplyResources(this.lblBaseLine, "lblBaseLine");
            this.lblBaseLine.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblBaseLine.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblBaseLine.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblBaseLine.ForeColor = System.Drawing.Color.Black;
            this.lblBaseLine.Name = "lblBaseLine";
            // 
            // dinBaseLine
            // 
            // 
            // 
            // 
            this.dinBaseLine.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinBaseLine.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinBaseLine.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinBaseLine.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinBaseLine, "dinBaseLine");
            this.dinBaseLine.Increment = 1D;
            this.dinBaseLine.MaxValue = 10D;
            this.dinBaseLine.MinValue = 1D;
            this.dinBaseLine.Name = "dinBaseLine";
            this.dinBaseLine.ShowUpDown = true;
            this.dinBaseLine.Value = 1.9D;
            // 
            // lblBaseLineUnit
            // 
            resources.ApplyResources(this.lblBaseLineUnit, "lblBaseLineUnit");
            this.lblBaseLineUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblBaseLineUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblBaseLineUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblBaseLineUnit.ForeColor = System.Drawing.Color.Black;
            this.lblBaseLineUnit.Name = "lblBaseLineUnit";
            // 
            // grpItemCondition
            // 
            this.grpItemCondition.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.grpItemCondition, "grpItemCondition");
            this.grpItemCondition.ForeColor = System.Drawing.Color.DimGray;
            this.grpItemCondition.Name = "grpItemCondition";
            this.grpItemCondition.TabStop = false;
            // 
            // frmItemSettingDIB
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grpItemCondition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmItemSettingDIB";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpApplySetting.ResumeLayout(false);
            this.pnlScanCount.ResumeLayout(false);
            this.pnlScanCount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinScanCount)).EndInit();
            this.pnlFilterSpec.ResumeLayout(false);
            this.pnlFilterSpec.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinFilterSpec)).EndInit();
            this.pnlTargetItem.ResumeLayout(false);
            this.pnlFilterBase.ResumeLayout(false);
            this.pnlFilterBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinFilterBase)).EndInit();
            this.grpMsrtSetting.ResumeLayout(false);
            this.pnlDeltaVolt.ResumeLayout(false);
            this.pnlDeltaVolt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).EndInit();
            this.pnlBaseLine.ResumeLayout(false);
            this.pnlBaseLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinBaseLine)).EndInit();
            this.grpItemCondition.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpItemCondition;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.Controls.GroupPanel grpApplySetting;
        private System.Windows.Forms.Panel pnlFilterBase;
        private DevComponents.DotNetBar.LabelX lblFilterBase;
        private DevComponents.Editors.DoubleInput dinFilterBase;
        private DevComponents.DotNetBar.LabelX lblFilterBaseUnit;
        private DevComponents.DotNetBar.Controls.GroupPanel grpMsrtSetting;
        private System.Windows.Forms.Panel pnlDeltaVolt;
        private DevComponents.DotNetBar.LabelX lblDeltaVolt;
        private DevComponents.Editors.DoubleInput dinMsrtClamp;
        private DevComponents.DotNetBar.LabelX lblDeltaVoltUnit;
        private System.Windows.Forms.Panel pnlBaseLine;
        private DevComponents.DotNetBar.LabelX lblBaseLine;
        private DevComponents.Editors.DoubleInput dinBaseLine;
        private DevComponents.DotNetBar.LabelX lblBaseLineUnit;
        private System.Windows.Forms.Panel pnlTargetItem;
        private DevComponents.DotNetBar.LabelX lblTargetItem;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTargetItemA;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsOnlyJuadgeSerious;
        private System.Windows.Forms.Panel pnlScanCount;
        private DevComponents.DotNetBar.LabelX lblScanCount;
        private DevComponents.Editors.DoubleInput dinScanCount;
        private DevComponents.DotNetBar.LabelX lblScanCountUnit;
        private System.Windows.Forms.Panel pnlFilterSpec;
        private DevComponents.DotNetBar.LabelX lblFilterSpec;
        private DevComponents.Editors.DoubleInput dinFilterSpec;
        private DevComponents.DotNetBar.LabelX lblFilterSpecUnit;

    }
}