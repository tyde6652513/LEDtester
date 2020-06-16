namespace MPI.Tester.Gui
{
	partial class frmSetFAEFunc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetFAEFunc));
            this.gplParameter = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chkIsShowContinousProbing = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsShowTesterChannelConfig = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsWriteReportDirectly = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkTCPIPSendEnableResultItem = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsWriteStatisticsDataToXmlHead = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEnableOutputPathByRecipe = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsEnableSaveErrMsrt = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsExtendResultItem = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsReturnPolar = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblCalibrationUIMode = new DevComponents.DotNetBar.LabelX();
            this.cmbCalibrationUIMode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.chkIsEnableSaveDetailLog = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsEnableErrStateReTest = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmbSrcSensingMode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.chkEnableSrcMeterFirmwareCalcTHY = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.gbContinousProbing = new System.Windows.Forms.GroupBox();
            this.chkEnableTakeFirstItemAsOSCheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEableVzRandomValue = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkDisableCheckAtPosXZero = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEnableContactCheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.dinContactApplyValue = new DevComponents.Editors.DoubleInput();
            this.lbllowLimit = new DevComponents.DotNetBar.LabelX();
            this.dinContactHighLimit = new DevComponents.Editors.DoubleInput();
            this.dinContactApplyTime = new DevComponents.Editors.DoubleInput();
            this.dinContactLowLimit = new DevComponents.Editors.DoubleInput();
            this.lblApplyTime = new DevComponents.DotNetBar.LabelX();
            this.lblApplyCurrent = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbSrcTurnOffType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.chkIsShowNPLCAndSGFilterSetting = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.plCoeffTable = new System.Windows.Forms.Panel();
            this.lblCoeffTableSetting = new DevComponents.DotNetBar.LabelX();
            this.lblCoefStartWL = new DevComponents.DotNetBar.LabelX();
            this.numCoefStartWL = new DevComponents.Editors.IntegerInput();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.lblCoefEndWL = new DevComponents.DotNetBar.LabelX();
            this.numCoefEndWL = new DevComponents.Editors.IntegerInput();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.lblCoefResolution = new DevComponents.DotNetBar.LabelX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.numCoefResolution = new DevComponents.Editors.IntegerInput();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnReload = new DevComponents.DotNetBar.ButtonX();
            this.gplSpectrometerGroup = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.intLimit02MinSTTime = new DevComponents.Editors.IntegerInput();
            this.lblLimit02MinSTTime = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmbCCTcalcType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.chkIsUseMPISpam2 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsEnableAbsCorrect = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.dinVlamdaType = new DevComponents.Editors.IntegerInput();
            this.lblVlamdaType = new DevComponents.DotNetBar.LabelX();
            this.lblScanAverageTitle = new DevComponents.DotNetBar.LabelX();
            this.numScanAverage = new DevComponents.Editors.IntegerInput();
            this.numLimitHighCount = new DevComponents.Editors.IntegerInput();
            this.numLimitLowCount = new DevComponents.Editors.IntegerInput();
            this.lblLimitHighCountTitle = new DevComponents.DotNetBar.LabelX();
            this.lblLimitLowCountTitle = new DevComponents.DotNetBar.LabelX();
            this.cmbCieIlluminant = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblCieIlluminantTitle = new DevComponents.DotNetBar.LabelX();
            this.cmbCieObserver = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblCieObserverTitle = new DevComponents.DotNetBar.LabelX();
            this.dinLimitStartTimeFactor = new DevComponents.Editors.DoubleInput();
            this.lblLimitStartTimeFactor = new DevComponents.DotNetBar.LabelX();
            this.dinBaseNosie = new DevComponents.Editors.DoubleInput();
            this.lblNosieBase = new DevComponents.DotNetBar.LabelX();
            this.chkEnableOnlySkipIZ = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.dinForceReverseCurrentRange = new DevComponents.Editors.DoubleInput();
            this.dinSafetyClamp = new DevComponents.Editors.DoubleInput();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.chkIsEnableSettingReverseRange = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEnableK26ReturnDefaultRange = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEnableForceMode = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chkIsEnableReTestMode = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsEnableMsrtForceValue = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.gplOsaGroup = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chkIsEnableSaveOsaRawData = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.gplParameter.SuspendLayout();
            this.gbContinousProbing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinContactApplyValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinContactHighLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinContactApplyTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinContactLowLimit)).BeginInit();
            this.plCoeffTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCoefStartWL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoefEndWL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoefResolution)).BeginInit();
            this.gplSpectrometerGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intLimit02MinSTTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinVlamdaType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScanAverage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitHighCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitLowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinLimitStartTimeFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinBaseNosie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceReverseCurrentRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinSafetyClamp)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.gplOsaGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // gplParameter
            // 
            this.gplParameter.BackColor = System.Drawing.Color.Transparent;
            this.gplParameter.CanvasColor = System.Drawing.Color.Transparent;
            this.gplParameter.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.gplParameter.Controls.Add(this.chkIsShowContinousProbing);
            this.gplParameter.Controls.Add(this.chkIsShowTesterChannelConfig);
            this.gplParameter.Controls.Add(this.chkIsWriteReportDirectly);
            this.gplParameter.Controls.Add(this.chkTCPIPSendEnableResultItem);
            this.gplParameter.Controls.Add(this.chkIsWriteStatisticsDataToXmlHead);
            this.gplParameter.Controls.Add(this.chkEnableOutputPathByRecipe);
            this.gplParameter.Controls.Add(this.chkIsEnableSaveErrMsrt);
            this.gplParameter.Controls.Add(this.chkIsExtendResultItem);
            this.gplParameter.Controls.Add(this.chkIsReturnPolar);
            this.gplParameter.Controls.Add(this.lblCalibrationUIMode);
            this.gplParameter.Controls.Add(this.cmbCalibrationUIMode);
            this.gplParameter.Controls.Add(this.chkIsEnableSaveDetailLog);
            this.gplParameter.Controls.Add(this.chkIsEnableErrStateReTest);
            this.gplParameter.DrawTitleBox = false;
            resources.ApplyResources(this.gplParameter, "gplParameter");
            this.gplParameter.Name = "gplParameter";
            // 
            // 
            // 
            this.gplParameter.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gplParameter.Style.BackColor2 = System.Drawing.Color.Lavender;
            this.gplParameter.Style.BackColorGradientAngle = 90;
            this.gplParameter.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplParameter.Style.BorderBottomWidth = 1;
            this.gplParameter.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gplParameter.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplParameter.Style.BorderLeftWidth = 1;
            this.gplParameter.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplParameter.Style.BorderRightWidth = 1;
            this.gplParameter.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplParameter.Style.BorderTopWidth = 1;
            this.gplParameter.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplParameter.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gplParameter.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gplParameter.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            // 
            // 
            // 
            this.gplParameter.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplParameter.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gplParameter.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplParameter.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // chkIsShowContinousProbing
            // 
            this.chkIsShowContinousProbing.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsShowContinousProbing.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsShowContinousProbing.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsShowContinousProbing.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsShowContinousProbing, "chkIsShowContinousProbing");
            this.chkIsShowContinousProbing.Name = "chkIsShowContinousProbing";
            this.chkIsShowContinousProbing.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsShowContinousProbing.TabStop = false;
            // 
            // chkIsShowTesterChannelConfig
            // 
            this.chkIsShowTesterChannelConfig.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsShowTesterChannelConfig.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsShowTesterChannelConfig.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsShowTesterChannelConfig.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsShowTesterChannelConfig, "chkIsShowTesterChannelConfig");
            this.chkIsShowTesterChannelConfig.Name = "chkIsShowTesterChannelConfig";
            this.chkIsShowTesterChannelConfig.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsShowTesterChannelConfig.TabStop = false;
            // 
            // chkIsWriteReportDirectly
            // 
            this.chkIsWriteReportDirectly.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsWriteReportDirectly.BackgroundStyle.BackColor = System.Drawing.Color.Transparent;
            this.chkIsWriteReportDirectly.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsWriteReportDirectly.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsWriteReportDirectly.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsWriteReportDirectly, "chkIsWriteReportDirectly");
            this.chkIsWriteReportDirectly.Name = "chkIsWriteReportDirectly";
            this.chkIsWriteReportDirectly.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            // 
            // chkTCPIPSendEnableResultItem
            // 
            this.chkTCPIPSendEnableResultItem.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkTCPIPSendEnableResultItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkTCPIPSendEnableResultItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkTCPIPSendEnableResultItem.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkTCPIPSendEnableResultItem, "chkTCPIPSendEnableResultItem");
            this.chkTCPIPSendEnableResultItem.Name = "chkTCPIPSendEnableResultItem";
            this.chkTCPIPSendEnableResultItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkTCPIPSendEnableResultItem.TabStop = false;
            // 
            // chkIsWriteStatisticsDataToXmlHead
            // 
            this.chkIsWriteStatisticsDataToXmlHead.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsWriteStatisticsDataToXmlHead.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsWriteStatisticsDataToXmlHead.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsWriteStatisticsDataToXmlHead.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsWriteStatisticsDataToXmlHead, "chkIsWriteStatisticsDataToXmlHead");
            this.chkIsWriteStatisticsDataToXmlHead.Name = "chkIsWriteStatisticsDataToXmlHead";
            this.chkIsWriteStatisticsDataToXmlHead.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsWriteStatisticsDataToXmlHead.TabStop = false;
            // 
            // chkEnableOutputPathByRecipe
            // 
            this.chkEnableOutputPathByRecipe.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkEnableOutputPathByRecipe.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEnableOutputPathByRecipe.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableOutputPathByRecipe.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkEnableOutputPathByRecipe, "chkEnableOutputPathByRecipe");
            this.chkEnableOutputPathByRecipe.Name = "chkEnableOutputPathByRecipe";
            this.chkEnableOutputPathByRecipe.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableOutputPathByRecipe.TabStop = false;
            // 
            // chkIsEnableSaveErrMsrt
            // 
            this.chkIsEnableSaveErrMsrt.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableSaveErrMsrt.BackgroundStyle.BackColor = System.Drawing.Color.Transparent;
            this.chkIsEnableSaveErrMsrt.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableSaveErrMsrt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableSaveErrMsrt.Checked = true;
            this.chkIsEnableSaveErrMsrt.CheckSignSize = new System.Drawing.Size(15, 15);
            this.chkIsEnableSaveErrMsrt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsEnableSaveErrMsrt.CheckValue = "Y";
            resources.ApplyResources(this.chkIsEnableSaveErrMsrt, "chkIsEnableSaveErrMsrt");
            this.chkIsEnableSaveErrMsrt.Name = "chkIsEnableSaveErrMsrt";
            this.chkIsEnableSaveErrMsrt.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            // 
            // chkIsExtendResultItem
            // 
            this.chkIsExtendResultItem.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsExtendResultItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsExtendResultItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsExtendResultItem.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsExtendResultItem, "chkIsExtendResultItem");
            this.chkIsExtendResultItem.Name = "chkIsExtendResultItem";
            this.chkIsExtendResultItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsExtendResultItem.TabStop = false;
            // 
            // chkIsReturnPolar
            // 
            this.chkIsReturnPolar.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsReturnPolar.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsReturnPolar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsReturnPolar.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsReturnPolar, "chkIsReturnPolar");
            this.chkIsReturnPolar.Name = "chkIsReturnPolar";
            this.chkIsReturnPolar.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsReturnPolar.TabStop = false;
            // 
            // lblCalibrationUIMode
            // 
            this.lblCalibrationUIMode.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCalibrationUIMode.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCalibrationUIMode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCalibrationUIMode, "lblCalibrationUIMode");
            this.lblCalibrationUIMode.Name = "lblCalibrationUIMode";
            // 
            // cmbCalibrationUIMode
            // 
            this.cmbCalibrationUIMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbCalibrationUIMode, "cmbCalibrationUIMode");
            this.cmbCalibrationUIMode.ForeColor = System.Drawing.Color.Black;
            this.cmbCalibrationUIMode.FormattingEnabled = true;
            this.cmbCalibrationUIMode.Name = "cmbCalibrationUIMode";
            this.cmbCalibrationUIMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // chkIsEnableSaveDetailLog
            // 
            this.chkIsEnableSaveDetailLog.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableSaveDetailLog.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableSaveDetailLog.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableSaveDetailLog.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableSaveDetailLog, "chkIsEnableSaveDetailLog");
            this.chkIsEnableSaveDetailLog.Name = "chkIsEnableSaveDetailLog";
            this.chkIsEnableSaveDetailLog.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableSaveDetailLog.TabStop = false;
            // 
            // chkIsEnableErrStateReTest
            // 
            this.chkIsEnableErrStateReTest.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableErrStateReTest.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableErrStateReTest.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableErrStateReTest.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableErrStateReTest, "chkIsEnableErrStateReTest");
            this.chkIsEnableErrStateReTest.Name = "chkIsEnableErrStateReTest";
            this.chkIsEnableErrStateReTest.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableErrStateReTest.TabStop = false;
            // 
            // cmbSrcSensingMode
            // 
            this.cmbSrcSensingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbSrcSensingMode, "cmbSrcSensingMode");
            this.cmbSrcSensingMode.ForeColor = System.Drawing.Color.Black;
            this.cmbSrcSensingMode.FormattingEnabled = true;
            this.cmbSrcSensingMode.Name = "cmbSrcSensingMode";
            this.cmbSrcSensingMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // labelX4
            // 
            resources.ApplyResources(this.labelX4, "labelX4");
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Name = "labelX4";
            // 
            // chkEnableSrcMeterFirmwareCalcTHY
            // 
            this.chkEnableSrcMeterFirmwareCalcTHY.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkEnableSrcMeterFirmwareCalcTHY.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEnableSrcMeterFirmwareCalcTHY.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableSrcMeterFirmwareCalcTHY.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkEnableSrcMeterFirmwareCalcTHY, "chkEnableSrcMeterFirmwareCalcTHY");
            this.chkEnableSrcMeterFirmwareCalcTHY.Name = "chkEnableSrcMeterFirmwareCalcTHY";
            this.chkEnableSrcMeterFirmwareCalcTHY.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableSrcMeterFirmwareCalcTHY.TabStop = false;
            // 
            // gbContinousProbing
            // 
            this.gbContinousProbing.BackColor = System.Drawing.Color.Transparent;
            this.gbContinousProbing.Controls.Add(this.chkEnableTakeFirstItemAsOSCheck);
            this.gbContinousProbing.Controls.Add(this.chkEableVzRandomValue);
            this.gbContinousProbing.Controls.Add(this.chkDisableCheckAtPosXZero);
            this.gbContinousProbing.Controls.Add(this.chkEnableContactCheck);
            this.gbContinousProbing.Controls.Add(this.labelX3);
            this.gbContinousProbing.Controls.Add(this.dinContactApplyValue);
            this.gbContinousProbing.Controls.Add(this.lbllowLimit);
            this.gbContinousProbing.Controls.Add(this.dinContactHighLimit);
            this.gbContinousProbing.Controls.Add(this.dinContactApplyTime);
            this.gbContinousProbing.Controls.Add(this.dinContactLowLimit);
            this.gbContinousProbing.Controls.Add(this.lblApplyTime);
            this.gbContinousProbing.Controls.Add(this.lblApplyCurrent);
            resources.ApplyResources(this.gbContinousProbing, "gbContinousProbing");
            this.gbContinousProbing.Name = "gbContinousProbing";
            this.gbContinousProbing.TabStop = false;
            // 
            // chkEnableTakeFirstItemAsOSCheck
            // 
            this.chkEnableTakeFirstItemAsOSCheck.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkEnableTakeFirstItemAsOSCheck.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEnableTakeFirstItemAsOSCheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableTakeFirstItemAsOSCheck.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkEnableTakeFirstItemAsOSCheck, "chkEnableTakeFirstItemAsOSCheck");
            this.chkEnableTakeFirstItemAsOSCheck.Name = "chkEnableTakeFirstItemAsOSCheck";
            this.chkEnableTakeFirstItemAsOSCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableTakeFirstItemAsOSCheck.TabStop = false;
            // 
            // chkEableVzRandomValue
            // 
            this.chkEableVzRandomValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkEableVzRandomValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEableVzRandomValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEableVzRandomValue.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkEableVzRandomValue, "chkEableVzRandomValue");
            this.chkEableVzRandomValue.Name = "chkEableVzRandomValue";
            this.chkEableVzRandomValue.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEableVzRandomValue.TabStop = false;
            // 
            // chkDisableCheckAtPosXZero
            // 
            this.chkDisableCheckAtPosXZero.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkDisableCheckAtPosXZero.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkDisableCheckAtPosXZero.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkDisableCheckAtPosXZero.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkDisableCheckAtPosXZero, "chkDisableCheckAtPosXZero");
            this.chkDisableCheckAtPosXZero.Name = "chkDisableCheckAtPosXZero";
            this.chkDisableCheckAtPosXZero.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkDisableCheckAtPosXZero.TabStop = false;
            // 
            // chkEnableContactCheck
            // 
            this.chkEnableContactCheck.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkEnableContactCheck.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEnableContactCheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableContactCheck.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkEnableContactCheck, "chkEnableContactCheck");
            this.chkEnableContactCheck.Name = "chkEnableContactCheck";
            this.chkEnableContactCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableContactCheck.TabStop = false;
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX3, "labelX3");
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Name = "labelX3";
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // dinContactApplyValue
            // 
            // 
            // 
            // 
            this.dinContactApplyValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinContactApplyValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinContactApplyValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinContactApplyValue.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinContactApplyValue, "dinContactApplyValue");
            this.dinContactApplyValue.Increment = 0.1D;
            this.dinContactApplyValue.MaxValue = 10D;
            this.dinContactApplyValue.MinValue = 0D;
            this.dinContactApplyValue.Name = "dinContactApplyValue";
            this.dinContactApplyValue.ShowUpDown = true;
            // 
            // lbllowLimit
            // 
            this.lbllowLimit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbllowLimit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lbllowLimit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lbllowLimit, "lbllowLimit");
            this.lbllowLimit.ForeColor = System.Drawing.Color.Black;
            this.lbllowLimit.Name = "lbllowLimit";
            this.lbllowLimit.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // dinContactHighLimit
            // 
            // 
            // 
            // 
            this.dinContactHighLimit.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinContactHighLimit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinContactHighLimit.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinContactHighLimit.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinContactHighLimit, "dinContactHighLimit");
            this.dinContactHighLimit.Increment = 0.5D;
            this.dinContactHighLimit.MaxValue = 10D;
            this.dinContactHighLimit.MinValue = 0D;
            this.dinContactHighLimit.Name = "dinContactHighLimit";
            this.dinContactHighLimit.ShowUpDown = true;
            // 
            // dinContactApplyTime
            // 
            // 
            // 
            // 
            this.dinContactApplyTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinContactApplyTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinContactApplyTime.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinContactApplyTime.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinContactApplyTime, "dinContactApplyTime");
            this.dinContactApplyTime.Increment = 0.5D;
            this.dinContactApplyTime.MaxValue = 10D;
            this.dinContactApplyTime.MinValue = 0.5D;
            this.dinContactApplyTime.Name = "dinContactApplyTime";
            this.dinContactApplyTime.ShowUpDown = true;
            this.dinContactApplyTime.Value = 1D;
            // 
            // dinContactLowLimit
            // 
            // 
            // 
            // 
            this.dinContactLowLimit.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinContactLowLimit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinContactLowLimit.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinContactLowLimit.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinContactLowLimit, "dinContactLowLimit");
            this.dinContactLowLimit.Increment = 0.5D;
            this.dinContactLowLimit.MaxValue = 10D;
            this.dinContactLowLimit.MinValue = 0D;
            this.dinContactLowLimit.Name = "dinContactLowLimit";
            this.dinContactLowLimit.ShowUpDown = true;
            // 
            // lblApplyTime
            // 
            this.lblApplyTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblApplyTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblApplyTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblApplyTime, "lblApplyTime");
            this.lblApplyTime.ForeColor = System.Drawing.Color.Black;
            this.lblApplyTime.Name = "lblApplyTime";
            this.lblApplyTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblApplyCurrent
            // 
            this.lblApplyCurrent.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblApplyCurrent.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblApplyCurrent.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblApplyCurrent, "lblApplyCurrent");
            this.lblApplyCurrent.ForeColor = System.Drawing.Color.Black;
            this.lblApplyCurrent.Name = "lblApplyCurrent";
            this.lblApplyCurrent.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // labelX2
            // 
            resources.ApplyResources(this.labelX2, "labelX2");
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Name = "labelX2";
            // 
            // cmbSrcTurnOffType
            // 
            this.cmbSrcTurnOffType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbSrcTurnOffType, "cmbSrcTurnOffType");
            this.cmbSrcTurnOffType.ForeColor = System.Drawing.Color.Black;
            this.cmbSrcTurnOffType.FormattingEnabled = true;
            this.cmbSrcTurnOffType.Name = "cmbSrcTurnOffType";
            this.cmbSrcTurnOffType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // chkIsShowNPLCAndSGFilterSetting
            // 
            this.chkIsShowNPLCAndSGFilterSetting.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsShowNPLCAndSGFilterSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsShowNPLCAndSGFilterSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsShowNPLCAndSGFilterSetting.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsShowNPLCAndSGFilterSetting, "chkIsShowNPLCAndSGFilterSetting");
            this.chkIsShowNPLCAndSGFilterSetting.Name = "chkIsShowNPLCAndSGFilterSetting";
            this.chkIsShowNPLCAndSGFilterSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsShowNPLCAndSGFilterSetting.TabStop = false;
            // 
            // plCoeffTable
            // 
            this.plCoeffTable.BackColor = System.Drawing.Color.Transparent;
            this.plCoeffTable.Controls.Add(this.lblCoeffTableSetting);
            this.plCoeffTable.Controls.Add(this.lblCoefStartWL);
            this.plCoeffTable.Controls.Add(this.numCoefStartWL);
            this.plCoeffTable.Controls.Add(this.labelX6);
            this.plCoeffTable.Controls.Add(this.lblCoefEndWL);
            this.plCoeffTable.Controls.Add(this.numCoefEndWL);
            this.plCoeffTable.Controls.Add(this.labelX8);
            this.plCoeffTable.Controls.Add(this.lblCoefResolution);
            this.plCoeffTable.Controls.Add(this.labelX10);
            this.plCoeffTable.Controls.Add(this.numCoefResolution);
            resources.ApplyResources(this.plCoeffTable, "plCoeffTable");
            this.plCoeffTable.Name = "plCoeffTable";
            // 
            // lblCoeffTableSetting
            // 
            this.lblCoeffTableSetting.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblCoeffTableSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCoeffTableSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCoeffTableSetting, "lblCoeffTableSetting");
            this.lblCoeffTableSetting.ForeColor = System.Drawing.Color.White;
            this.lblCoeffTableSetting.Name = "lblCoeffTableSetting";
            this.lblCoeffTableSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblCoeffTableSetting.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblCoefStartWL
            // 
            this.lblCoefStartWL.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCoefStartWL.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCoefStartWL.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCoefStartWL, "lblCoefStartWL");
            this.lblCoefStartWL.ForeColor = System.Drawing.Color.Black;
            this.lblCoefStartWL.Name = "lblCoefStartWL";
            this.lblCoefStartWL.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // numCoefStartWL
            // 
            this.numCoefStartWL.AntiAlias = true;
            // 
            // 
            // 
            this.numCoefStartWL.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numCoefStartWL.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numCoefStartWL.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numCoefStartWL, "numCoefStartWL");
            this.numCoefStartWL.MaxValue = 10000;
            this.numCoefStartWL.MinValue = 0;
            this.numCoefStartWL.Name = "numCoefStartWL";
            this.numCoefStartWL.ShowUpDown = true;
            this.numCoefStartWL.Value = 350;
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX6, "labelX6");
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Name = "labelX6";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblCoefEndWL
            // 
            this.lblCoefEndWL.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCoefEndWL.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCoefEndWL.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCoefEndWL, "lblCoefEndWL");
            this.lblCoefEndWL.ForeColor = System.Drawing.Color.Black;
            this.lblCoefEndWL.Name = "lblCoefEndWL";
            this.lblCoefEndWL.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // numCoefEndWL
            // 
            this.numCoefEndWL.AntiAlias = true;
            // 
            // 
            // 
            this.numCoefEndWL.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numCoefEndWL.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numCoefEndWL.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numCoefEndWL, "numCoefEndWL");
            this.numCoefEndWL.MaxValue = 10000;
            this.numCoefEndWL.MinValue = 0;
            this.numCoefEndWL.Name = "numCoefEndWL";
            this.numCoefEndWL.ShowUpDown = true;
            this.numCoefEndWL.Value = 850;
            // 
            // labelX8
            // 
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX8, "labelX8");
            this.labelX8.ForeColor = System.Drawing.Color.Black;
            this.labelX8.Name = "labelX8";
            this.labelX8.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblCoefResolution
            // 
            this.lblCoefResolution.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCoefResolution.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCoefResolution.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCoefResolution, "lblCoefResolution");
            this.lblCoefResolution.ForeColor = System.Drawing.Color.Black;
            this.lblCoefResolution.Name = "lblCoefResolution";
            this.lblCoefResolution.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // labelX10
            // 
            this.labelX10.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX10, "labelX10");
            this.labelX10.ForeColor = System.Drawing.Color.Black;
            this.labelX10.Name = "labelX10";
            this.labelX10.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // numCoefResolution
            // 
            this.numCoefResolution.AntiAlias = true;
            // 
            // 
            // 
            this.numCoefResolution.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numCoefResolution.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numCoefResolution.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numCoefResolution, "numCoefResolution");
            this.numCoefResolution.MaxValue = 100;
            this.numCoefResolution.MinValue = 0;
            this.numCoefResolution.Name = "numCoefResolution";
            this.numCoefResolution.ShowUpDown = true;
            this.numCoefResolution.Value = 1;
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Image = global::MPI.Tester.Gui.Properties.Resources.btnSaveFile_B;
            this.btnSave.Name = "btnSave";
            this.btnSave.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSave.Tag = "40";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReload
            // 
            this.btnReload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReload.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnReload, "btnReload");
            this.btnReload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReload.Image = global::MPI.Tester.Gui.Properties.Resources.btnUndo;
            this.btnReload.Name = "btnReload";
            this.btnReload.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnReload.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnReload.Tag = "40";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // gplSpectrometerGroup
            // 
            this.gplSpectrometerGroup.BackColor = System.Drawing.Color.Transparent;
            this.gplSpectrometerGroup.CanvasColor = System.Drawing.Color.Transparent;
            this.gplSpectrometerGroup.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.gplSpectrometerGroup.Controls.Add(this.intLimit02MinSTTime);
            this.gplSpectrometerGroup.Controls.Add(this.lblLimit02MinSTTime);
            this.gplSpectrometerGroup.Controls.Add(this.labelX1);
            this.gplSpectrometerGroup.Controls.Add(this.cmbCCTcalcType);
            this.gplSpectrometerGroup.Controls.Add(this.chkIsUseMPISpam2);
            this.gplSpectrometerGroup.Controls.Add(this.chkIsEnableAbsCorrect);
            this.gplSpectrometerGroup.Controls.Add(this.dinVlamdaType);
            this.gplSpectrometerGroup.Controls.Add(this.lblVlamdaType);
            this.gplSpectrometerGroup.Controls.Add(this.lblScanAverageTitle);
            this.gplSpectrometerGroup.Controls.Add(this.numScanAverage);
            this.gplSpectrometerGroup.Controls.Add(this.numLimitHighCount);
            this.gplSpectrometerGroup.Controls.Add(this.numLimitLowCount);
            this.gplSpectrometerGroup.Controls.Add(this.plCoeffTable);
            this.gplSpectrometerGroup.Controls.Add(this.lblLimitHighCountTitle);
            this.gplSpectrometerGroup.Controls.Add(this.lblLimitLowCountTitle);
            this.gplSpectrometerGroup.Controls.Add(this.cmbCieIlluminant);
            this.gplSpectrometerGroup.Controls.Add(this.lblCieIlluminantTitle);
            this.gplSpectrometerGroup.Controls.Add(this.cmbCieObserver);
            this.gplSpectrometerGroup.Controls.Add(this.lblCieObserverTitle);
            this.gplSpectrometerGroup.Controls.Add(this.dinLimitStartTimeFactor);
            this.gplSpectrometerGroup.Controls.Add(this.lblLimitStartTimeFactor);
            this.gplSpectrometerGroup.Controls.Add(this.dinBaseNosie);
            this.gplSpectrometerGroup.Controls.Add(this.lblNosieBase);
            this.gplSpectrometerGroup.DrawTitleBox = false;
            resources.ApplyResources(this.gplSpectrometerGroup, "gplSpectrometerGroup");
            this.gplSpectrometerGroup.Name = "gplSpectrometerGroup";
            // 
            // 
            // 
            this.gplSpectrometerGroup.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gplSpectrometerGroup.Style.BackColor2 = System.Drawing.Color.Lavender;
            this.gplSpectrometerGroup.Style.BackColorGradientAngle = 90;
            this.gplSpectrometerGroup.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplSpectrometerGroup.Style.BorderBottomWidth = 1;
            this.gplSpectrometerGroup.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gplSpectrometerGroup.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplSpectrometerGroup.Style.BorderLeftWidth = 1;
            this.gplSpectrometerGroup.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplSpectrometerGroup.Style.BorderRightWidth = 1;
            this.gplSpectrometerGroup.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplSpectrometerGroup.Style.BorderTopWidth = 1;
            this.gplSpectrometerGroup.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplSpectrometerGroup.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gplSpectrometerGroup.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gplSpectrometerGroup.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gplSpectrometerGroup.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gplSpectrometerGroup.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplSpectrometerGroup.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gplSpectrometerGroup.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplSpectrometerGroup.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // intLimit02MinSTTime
            // 
            this.intLimit02MinSTTime.AntiAlias = true;
            // 
            // 
            // 
            this.intLimit02MinSTTime.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.intLimit02MinSTTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intLimit02MinSTTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.intLimit02MinSTTime, "intLimit02MinSTTime");
            this.intLimit02MinSTTime.MaxValue = 10;
            this.intLimit02MinSTTime.MinValue = 1;
            this.intLimit02MinSTTime.Name = "intLimit02MinSTTime";
            this.intLimit02MinSTTime.ShowUpDown = true;
            this.intLimit02MinSTTime.Value = 10;
            // 
            // lblLimit02MinSTTime
            // 
            this.lblLimit02MinSTTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblLimit02MinSTTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblLimit02MinSTTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblLimit02MinSTTime, "lblLimit02MinSTTime");
            this.lblLimit02MinSTTime.ForeColor = System.Drawing.Color.Black;
            this.lblLimit02MinSTTime.Name = "lblLimit02MinSTTime";
            this.lblLimit02MinSTTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX1, "labelX1");
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Name = "labelX1";
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // cmbCCTcalcType
            // 
            this.cmbCCTcalcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCCTcalcType.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbCCTcalcType, "cmbCCTcalcType");
            this.cmbCCTcalcType.ForeColor = System.Drawing.Color.Black;
            this.cmbCCTcalcType.Name = "cmbCCTcalcType";
            // 
            // chkIsUseMPISpam2
            // 
            this.chkIsUseMPISpam2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsUseMPISpam2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsUseMPISpam2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsUseMPISpam2.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsUseMPISpam2, "chkIsUseMPISpam2");
            this.chkIsUseMPISpam2.Name = "chkIsUseMPISpam2";
            this.chkIsUseMPISpam2.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsUseMPISpam2.TabStop = false;
            // 
            // chkIsEnableAbsCorrect
            // 
            this.chkIsEnableAbsCorrect.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableAbsCorrect.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableAbsCorrect.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableAbsCorrect.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableAbsCorrect, "chkIsEnableAbsCorrect");
            this.chkIsEnableAbsCorrect.Name = "chkIsEnableAbsCorrect";
            this.chkIsEnableAbsCorrect.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableAbsCorrect.TabStop = false;
            // 
            // dinVlamdaType
            // 
            this.dinVlamdaType.AntiAlias = true;
            // 
            // 
            // 
            this.dinVlamdaType.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.dinVlamdaType.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinVlamdaType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.dinVlamdaType, "dinVlamdaType");
            this.dinVlamdaType.MaxValue = 1;
            this.dinVlamdaType.MinValue = 0;
            this.dinVlamdaType.Name = "dinVlamdaType";
            this.dinVlamdaType.ShowUpDown = true;
            // 
            // lblVlamdaType
            // 
            this.lblVlamdaType.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblVlamdaType.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblVlamdaType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblVlamdaType, "lblVlamdaType");
            this.lblVlamdaType.ForeColor = System.Drawing.Color.Black;
            this.lblVlamdaType.Name = "lblVlamdaType";
            this.lblVlamdaType.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblScanAverageTitle
            // 
            this.lblScanAverageTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblScanAverageTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblScanAverageTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblScanAverageTitle, "lblScanAverageTitle");
            this.lblScanAverageTitle.ForeColor = System.Drawing.Color.Black;
            this.lblScanAverageTitle.Name = "lblScanAverageTitle";
            this.lblScanAverageTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // numScanAverage
            // 
            this.numScanAverage.AntiAlias = true;
            // 
            // 
            // 
            this.numScanAverage.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numScanAverage.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numScanAverage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numScanAverage, "numScanAverage");
            this.numScanAverage.MaxValue = 100;
            this.numScanAverage.MinValue = 1;
            this.numScanAverage.Name = "numScanAverage";
            this.numScanAverage.ShowUpDown = true;
            this.numScanAverage.Value = 1;
            // 
            // numLimitHighCount
            // 
            this.numLimitHighCount.AntiAlias = true;
            // 
            // 
            // 
            this.numLimitHighCount.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numLimitHighCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numLimitHighCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numLimitHighCount, "numLimitHighCount");
            this.numLimitHighCount.MaxValue = 100000;
            this.numLimitHighCount.MinValue = 1;
            this.numLimitHighCount.Name = "numLimitHighCount";
            this.numLimitHighCount.ShowUpDown = true;
            this.numLimitHighCount.Value = 60000;
            // 
            // numLimitLowCount
            // 
            this.numLimitLowCount.AntiAlias = true;
            // 
            // 
            // 
            this.numLimitLowCount.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numLimitLowCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numLimitLowCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numLimitLowCount, "numLimitLowCount");
            this.numLimitLowCount.MaxValue = 100000;
            this.numLimitLowCount.MinValue = 5000;
            this.numLimitLowCount.Name = "numLimitLowCount";
            this.numLimitLowCount.ShowUpDown = true;
            this.numLimitLowCount.Value = 20000;
            // 
            // lblLimitHighCountTitle
            // 
            this.lblLimitHighCountTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblLimitHighCountTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblLimitHighCountTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblLimitHighCountTitle, "lblLimitHighCountTitle");
            this.lblLimitHighCountTitle.ForeColor = System.Drawing.Color.Black;
            this.lblLimitHighCountTitle.Name = "lblLimitHighCountTitle";
            this.lblLimitHighCountTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblLimitLowCountTitle
            // 
            this.lblLimitLowCountTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblLimitLowCountTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblLimitLowCountTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblLimitLowCountTitle, "lblLimitLowCountTitle");
            this.lblLimitLowCountTitle.ForeColor = System.Drawing.Color.Black;
            this.lblLimitLowCountTitle.Name = "lblLimitLowCountTitle";
            this.lblLimitLowCountTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // cmbCieIlluminant
            // 
            this.cmbCieIlluminant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCieIlluminant.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbCieIlluminant, "cmbCieIlluminant");
            this.cmbCieIlluminant.ForeColor = System.Drawing.Color.Black;
            this.cmbCieIlluminant.Name = "cmbCieIlluminant";
            // 
            // lblCieIlluminantTitle
            // 
            this.lblCieIlluminantTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCieIlluminantTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCieIlluminantTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCieIlluminantTitle, "lblCieIlluminantTitle");
            this.lblCieIlluminantTitle.ForeColor = System.Drawing.Color.Black;
            this.lblCieIlluminantTitle.Name = "lblCieIlluminantTitle";
            this.lblCieIlluminantTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // cmbCieObserver
            // 
            this.cmbCieObserver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCieObserver.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbCieObserver, "cmbCieObserver");
            this.cmbCieObserver.ForeColor = System.Drawing.Color.Black;
            this.cmbCieObserver.Name = "cmbCieObserver";
            // 
            // lblCieObserverTitle
            // 
            this.lblCieObserverTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCieObserverTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblCieObserverTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCieObserverTitle, "lblCieObserverTitle");
            this.lblCieObserverTitle.ForeColor = System.Drawing.Color.Black;
            this.lblCieObserverTitle.Name = "lblCieObserverTitle";
            this.lblCieObserverTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // dinLimitStartTimeFactor
            // 
            // 
            // 
            // 
            this.dinLimitStartTimeFactor.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLimitStartTimeFactor.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinLimitStartTimeFactor.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinLimitStartTimeFactor.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinLimitStartTimeFactor, "dinLimitStartTimeFactor");
            this.dinLimitStartTimeFactor.Increment = 0.5D;
            this.dinLimitStartTimeFactor.MaxValue = 10D;
            this.dinLimitStartTimeFactor.MinValue = 0.5D;
            this.dinLimitStartTimeFactor.Name = "dinLimitStartTimeFactor";
            this.dinLimitStartTimeFactor.ShowUpDown = true;
            this.dinLimitStartTimeFactor.Value = 1D;
            // 
            // lblLimitStartTimeFactor
            // 
            this.lblLimitStartTimeFactor.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblLimitStartTimeFactor.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblLimitStartTimeFactor.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblLimitStartTimeFactor, "lblLimitStartTimeFactor");
            this.lblLimitStartTimeFactor.ForeColor = System.Drawing.Color.Black;
            this.lblLimitStartTimeFactor.Name = "lblLimitStartTimeFactor";
            this.lblLimitStartTimeFactor.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // dinBaseNosie
            // 
            // 
            // 
            // 
            this.dinBaseNosie.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinBaseNosie.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinBaseNosie.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinBaseNosie.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinBaseNosie, "dinBaseNosie");
            this.dinBaseNosie.Increment = 0.1D;
            this.dinBaseNosie.MaxValue = 10D;
            this.dinBaseNosie.MinValue = 0D;
            this.dinBaseNosie.Name = "dinBaseNosie";
            this.dinBaseNosie.ShowUpDown = true;
            // 
            // lblNosieBase
            // 
            this.lblNosieBase.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblNosieBase.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblNosieBase.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblNosieBase, "lblNosieBase");
            this.lblNosieBase.ForeColor = System.Drawing.Color.Black;
            this.lblNosieBase.Name = "lblNosieBase";
            this.lblNosieBase.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // chkEnableOnlySkipIZ
            // 
            this.chkEnableOnlySkipIZ.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkEnableOnlySkipIZ.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEnableOnlySkipIZ.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableOnlySkipIZ.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkEnableOnlySkipIZ, "chkEnableOnlySkipIZ");
            this.chkEnableOnlySkipIZ.Name = "chkEnableOnlySkipIZ";
            this.chkEnableOnlySkipIZ.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableOnlySkipIZ.TabStop = false;
            // 
            // dinForceReverseCurrentRange
            // 
            // 
            // 
            // 
            this.dinForceReverseCurrentRange.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceReverseCurrentRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceReverseCurrentRange.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceReverseCurrentRange.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinForceReverseCurrentRange, "dinForceReverseCurrentRange");
            this.dinForceReverseCurrentRange.Increment = 0D;
            this.dinForceReverseCurrentRange.MaxValue = 10D;
            this.dinForceReverseCurrentRange.MinValue = 0.01D;
            this.dinForceReverseCurrentRange.Name = "dinForceReverseCurrentRange";
            this.dinForceReverseCurrentRange.ShowUpDown = true;
            this.dinForceReverseCurrentRange.Value = 0.1D;
            // 
            // dinSafetyClamp
            // 
            // 
            // 
            // 
            this.dinSafetyClamp.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSafetyClamp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSafetyClamp.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSafetyClamp.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinSafetyClamp, "dinSafetyClamp");
            this.dinSafetyClamp.Increment = 0D;
            this.dinSafetyClamp.MaxValue = 200D;
            this.dinSafetyClamp.MinValue = 0D;
            this.dinSafetyClamp.Name = "dinSafetyClamp";
            this.dinSafetyClamp.ShowUpDown = true;
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX5, "labelX5");
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Name = "labelX5";
            this.labelX5.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // chkIsEnableSettingReverseRange
            // 
            this.chkIsEnableSettingReverseRange.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableSettingReverseRange.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableSettingReverseRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableSettingReverseRange.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableSettingReverseRange, "chkIsEnableSettingReverseRange");
            this.chkIsEnableSettingReverseRange.Name = "chkIsEnableSettingReverseRange";
            this.chkIsEnableSettingReverseRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableSettingReverseRange.TabStop = false;
            // 
            // chkEnableK26ReturnDefaultRange
            // 
            this.chkEnableK26ReturnDefaultRange.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkEnableK26ReturnDefaultRange.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEnableK26ReturnDefaultRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableK26ReturnDefaultRange.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkEnableK26ReturnDefaultRange, "chkEnableK26ReturnDefaultRange");
            this.chkEnableK26ReturnDefaultRange.Name = "chkEnableK26ReturnDefaultRange";
            this.chkEnableK26ReturnDefaultRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableK26ReturnDefaultRange.TabStop = false;
            // 
            // chkEnableForceMode
            // 
            this.chkEnableForceMode.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkEnableForceMode.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkEnableForceMode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableForceMode.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkEnableForceMode, "chkEnableForceMode");
            this.chkEnableForceMode.Name = "chkEnableForceMode";
            this.chkEnableForceMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableForceMode.TabStop = false;
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.Color.Transparent;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.groupPanel1.Controls.Add(this.chkIsEnableReTestMode);
            this.groupPanel1.Controls.Add(this.chkIsEnableMsrtForceValue);
            this.groupPanel1.Controls.Add(this.chkEnableOnlySkipIZ);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.dinForceReverseCurrentRange);
            this.groupPanel1.Controls.Add(this.cmbSrcTurnOffType);
            this.groupPanel1.Controls.Add(this.dinSafetyClamp);
            this.groupPanel1.Controls.Add(this.gbContinousProbing);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Controls.Add(this.cmbSrcSensingMode);
            this.groupPanel1.Controls.Add(this.chkIsEnableSettingReverseRange);
            this.groupPanel1.Controls.Add(this.chkIsShowNPLCAndSGFilterSetting);
            this.groupPanel1.Controls.Add(this.chkEnableK26ReturnDefaultRange);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.chkEnableForceMode);
            this.groupPanel1.Controls.Add(this.chkEnableSrcMeterFirmwareCalcTHY);
            this.groupPanel1.DrawTitleBox = false;
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupPanel1.Style.BackColor2 = System.Drawing.Color.Lavender;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // chkIsEnableReTestMode
            // 
            this.chkIsEnableReTestMode.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableReTestMode.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableReTestMode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableReTestMode.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableReTestMode, "chkIsEnableReTestMode");
            this.chkIsEnableReTestMode.Name = "chkIsEnableReTestMode";
            this.chkIsEnableReTestMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableReTestMode.TabStop = false;
            // 
            // chkIsEnableMsrtForceValue
            // 
            this.chkIsEnableMsrtForceValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableMsrtForceValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableMsrtForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableMsrtForceValue.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableMsrtForceValue, "chkIsEnableMsrtForceValue");
            this.chkIsEnableMsrtForceValue.Name = "chkIsEnableMsrtForceValue";
            this.chkIsEnableMsrtForceValue.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableMsrtForceValue.TabStop = false;
            // 
            // gplOsaGroup
            // 
            this.gplOsaGroup.BackColor = System.Drawing.Color.Transparent;
            this.gplOsaGroup.CanvasColor = System.Drawing.Color.Transparent;
            this.gplOsaGroup.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.gplOsaGroup.Controls.Add(this.chkIsEnableSaveOsaRawData);
            this.gplOsaGroup.DrawTitleBox = false;
            resources.ApplyResources(this.gplOsaGroup, "gplOsaGroup");
            this.gplOsaGroup.Name = "gplOsaGroup";
            // 
            // 
            // 
            this.gplOsaGroup.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gplOsaGroup.Style.BackColor2 = System.Drawing.Color.Lavender;
            this.gplOsaGroup.Style.BackColorGradientAngle = 90;
            this.gplOsaGroup.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplOsaGroup.Style.BorderBottomWidth = 1;
            this.gplOsaGroup.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gplOsaGroup.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplOsaGroup.Style.BorderLeftWidth = 1;
            this.gplOsaGroup.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplOsaGroup.Style.BorderRightWidth = 1;
            this.gplOsaGroup.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gplOsaGroup.Style.BorderTopWidth = 1;
            this.gplOsaGroup.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplOsaGroup.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gplOsaGroup.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gplOsaGroup.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            // 
            // 
            // 
            this.gplOsaGroup.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplOsaGroup.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gplOsaGroup.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gplOsaGroup.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // chkIsEnableSaveOsaRawData
            // 
            this.chkIsEnableSaveOsaRawData.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableSaveOsaRawData.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableSaveOsaRawData.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableSaveOsaRawData.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableSaveOsaRawData, "chkIsEnableSaveOsaRawData");
            this.chkIsEnableSaveOsaRawData.Name = "chkIsEnableSaveOsaRawData";
            this.chkIsEnableSaveOsaRawData.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableSaveOsaRawData.TabStop = false;
            // 
            // frmSetFAEFunc
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.Controls.Add(this.gplOsaGroup);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.gplSpectrometerGroup);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gplParameter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetFAEFunc";
            this.Load += new System.EventHandler(this.frmSetFAEFunc_Load);
            this.gplParameter.ResumeLayout(false);
            this.gbContinousProbing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinContactApplyValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinContactHighLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinContactApplyTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinContactLowLimit)).EndInit();
            this.plCoeffTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numCoefStartWL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoefEndWL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoefResolution)).EndInit();
            this.gplSpectrometerGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.intLimit02MinSTTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinVlamdaType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScanAverage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitHighCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitLowCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinLimitStartTimeFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinBaseNosie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceReverseCurrentRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinSafetyClamp)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.gplOsaGroup.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private DevComponents.DotNetBar.Controls.GroupPanel gplParameter;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCalibrationUIMode;
		private DevComponents.DotNetBar.LabelX lblCalibrationUIMode;
		private DevComponents.DotNetBar.ButtonX btnSave;
		private DevComponents.DotNetBar.ButtonX btnReload;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableErrStateReTest;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableSaveDetailLog;
		private DevComponents.DotNetBar.Controls.GroupPanel gplSpectrometerGroup;
		private DevComponents.Editors.DoubleInput dinBaseNosie;
        private DevComponents.DotNetBar.LabelX lblNosieBase;
		private DevComponents.Editors.DoubleInput dinLimitStartTimeFactor;
		private DevComponents.DotNetBar.LabelX lblLimitStartTimeFactor;
		private DevComponents.DotNetBar.LabelX lblScanAverageTitle;
		private DevComponents.Editors.IntegerInput numScanAverage;
		private DevComponents.Editors.IntegerInput numLimitHighCount;
		private DevComponents.Editors.IntegerInput numLimitLowCount;
		private DevComponents.DotNetBar.LabelX lblLimitHighCountTitle;
		private DevComponents.DotNetBar.LabelX lblLimitLowCountTitle;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCieIlluminant;
		private DevComponents.DotNetBar.LabelX lblCieIlluminantTitle;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCieObserver;
		private DevComponents.DotNetBar.LabelX lblCieObserverTitle;
		private DevComponents.Editors.IntegerInput dinVlamdaType;
		private DevComponents.DotNetBar.LabelX lblVlamdaType;
		private System.Windows.Forms.Panel plCoeffTable;
		private DevComponents.DotNetBar.LabelX lblCoeffTableSetting;
		private DevComponents.DotNetBar.LabelX lblCoefStartWL;
		private DevComponents.Editors.IntegerInput numCoefStartWL;
		private DevComponents.DotNetBar.LabelX labelX6;
		private DevComponents.DotNetBar.LabelX lblCoefEndWL;
		private DevComponents.Editors.IntegerInput numCoefEndWL;
		private DevComponents.DotNetBar.LabelX labelX8;
		private DevComponents.DotNetBar.LabelX lblCoefResolution;
		private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.Editors.IntegerInput numCoefResolution;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsReturnPolar;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableSaveErrMsrt;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsExtendResultItem;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableAbsCorrect;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableOutputPathByRecipe;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsWriteStatisticsDataToXmlHead;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkTCPIPSendEnableResultItem;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsShowNPLCAndSGFilterSetting;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCCTcalcType;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsUseMPISpam2;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsWriteReportDirectly;
		private DevComponents.DotNetBar.LabelX labelX2;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSrcTurnOffType;
		private DevComponents.Editors.IntegerInput intLimit02MinSTTime;
		private DevComponents.DotNetBar.LabelX lblLimit02MinSTTime;
        private DevComponents.Editors.DoubleInput dinContactLowLimit;
        private DevComponents.Editors.DoubleInput dinContactHighLimit;
        private DevComponents.Editors.DoubleInput dinContactApplyValue;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableContactCheck;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX lbllowLimit;
        private DevComponents.Editors.DoubleInput dinContactApplyTime;
        private DevComponents.DotNetBar.LabelX lblApplyTime;
        private DevComponents.DotNetBar.LabelX lblApplyCurrent;
        private System.Windows.Forms.GroupBox gbContinousProbing;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkDisableCheckAtPosXZero;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEableVzRandomValue;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableSrcMeterFirmwareCalcTHY;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSrcSensingMode;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableForceMode;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableTakeFirstItemAsOSCheck;
		  private DevComponents.Editors.DoubleInput dinForceReverseCurrentRange;
		  private DevComponents.Editors.DoubleInput dinSafetyClamp;
		  private DevComponents.DotNetBar.LabelX labelX5;
		  private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableSettingReverseRange;
		  private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableK26ReturnDefaultRange;
		  private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableOnlySkipIZ;
          private DevComponents.DotNetBar.Controls.CheckBoxX chkIsShowTesterChannelConfig;
          private DevComponents.DotNetBar.Controls.CheckBoxX chkIsShowContinousProbing;
          private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
          private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableMsrtForceValue;
          private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableReTestMode;
          private DevComponents.DotNetBar.Controls.GroupPanel gplOsaGroup;
          private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableSaveOsaRawData;
	}
}