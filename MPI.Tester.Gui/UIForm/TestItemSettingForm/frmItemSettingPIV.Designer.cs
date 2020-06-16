namespace MPI.Tester.Gui
{
    partial class frmItemSettingPIV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemSettingPIV));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpChannel = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmbSelectedChannel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSelectedChannel = new DevComponents.DotNetBar.LabelX();
            this.grpApplySetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pnlStepPulseCnt = new System.Windows.Forms.Panel();
            this.lblStepPulseCnt = new DevComponents.DotNetBar.LabelX();
            this.numStepPulseCnt = new DevComponents.Editors.IntegerInput();
            this.pnlSourceFunc = new System.Windows.Forms.Panel();
            this.cmbSourceFunc = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSourceFunc = new DevComponents.DotNetBar.LabelX();
            this.pnlDutyCycle = new System.Windows.Forms.Panel();
            this.lblDutyCycle = new DevComponents.DotNetBar.LabelX();
            this.dinDutyCycle = new DevComponents.Editors.DoubleInput();
            this.lblDutyCycleUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlTurnOffTime = new System.Windows.Forms.Panel();
            this.lblTurnOffTime = new DevComponents.DotNetBar.LabelX();
            this.dinTurnOffTime = new DevComponents.Editors.DoubleInput();
            this.lblTurnOffTimeUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlStepValue = new System.Windows.Forms.Panel();
            this.dinStepValue = new DevComponents.Editors.DoubleInput();
            this.lblStepValue = new DevComponents.DotNetBar.LabelX();
            this.lblStepValueUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlPoints = new System.Windows.Forms.Panel();
            this.txtDisplayPointsValue = new DevComponents.DotNetBar.LabelX();
            this.lblPoints = new DevComponents.DotNetBar.LabelX();
            this.lblPointsUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlEndValue = new System.Windows.Forms.Panel();
            this.lblEndValue = new DevComponents.DotNetBar.LabelX();
            this.dinEndValue = new DevComponents.Editors.DoubleInput();
            this.lblEndValueUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlForceTime = new System.Windows.Forms.Panel();
            this.lblPulseWidth = new DevComponents.DotNetBar.LabelX();
            this.lblForceTime = new DevComponents.DotNetBar.LabelX();
            this.dinForceTime = new DevComponents.Editors.DoubleInput();
            this.lblForceTimeUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlStartValue = new System.Windows.Forms.Panel();
            this.lblStartValue = new DevComponents.DotNetBar.LabelX();
            this.dinStartValue = new DevComponents.Editors.DoubleInput();
            this.lblStartValueUnit = new DevComponents.DotNetBar.LabelX();
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
            this.pnlQcwFilterCount = new System.Windows.Forms.Panel();
            this.lblQcwFilterCount = new DevComponents.DotNetBar.LabelX();
            this.numQcwFilterCount = new DevComponents.Editors.IntegerInput();
            this.pnlMsrtClamp = new System.Windows.Forms.Panel();
            this.lblMsrtClamp = new DevComponents.DotNetBar.LabelX();
            this.dinMsrtClamp = new DevComponents.Editors.DoubleInput();
            this.lblMsrtClampUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlMsrtRange = new System.Windows.Forms.Panel();
            this.lblMsrtRange = new DevComponents.DotNetBar.LabelX();
            this.dinMsrtRange = new DevComponents.Editors.DoubleInput();
            this.lblMsrtRangeUnit = new DevComponents.DotNetBar.LabelX();
            this.pnlNPLC = new System.Windows.Forms.Panel();
            this.lblNPLC = new DevComponents.DotNetBar.LabelX();
            this.dinNPLC = new DevComponents.Editors.DoubleInput();
            this.lblNplcAuto = new DevComponents.DotNetBar.LabelX();
            this.pnlFilterCount = new System.Windows.Forms.Panel();
            this.lblMsrtFilterCount = new DevComponents.DotNetBar.LabelX();
            this.numMsrtFilterCount = new DevComponents.Editors.IntegerInput();
            this.grpPDDetector = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pnlDetectorBiasVoltage = new System.Windows.Forms.Panel();
            this.lblDetectorBiasVoltageUnit = new DevComponents.DotNetBar.LabelX();
            this.lblDetectorBiasVoltage = new DevComponents.DotNetBar.LabelX();
            this.dinDetectorBiasVoltage = new DevComponents.Editors.DoubleInput();
            this.pnlDetectorNPLC = new System.Windows.Forms.Panel();
            this.lblDetectorNPLC = new DevComponents.DotNetBar.LabelX();
            this.dinDetectorNPLC = new DevComponents.Editors.DoubleInput();
            this.lblDetectorNplcAuto1 = new DevComponents.DotNetBar.LabelX();
            this.pnlDetectorMsrtRange = new System.Windows.Forms.Panel();
            this.lblDetectorMsrtRangeUnit = new DevComponents.DotNetBar.LabelX();
            this.lblDetectorMsrtRange = new DevComponents.DotNetBar.LabelX();
            this.dinDetectorMsrtRange = new DevComponents.Editors.DoubleInput();
            this.grpCalcSetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.grpLinearitySetting = new System.Windows.Forms.GroupBox();
            this.pnlLnSearchCurr2 = new System.Windows.Forms.Panel();
            this.lblLnSearchCurr2 = new System.Windows.Forms.Label();
            this.dinLnSearchCurr2 = new DevComponents.Editors.DoubleInput();
            this.lblLnSearchCurr2Unit = new System.Windows.Forms.Label();
            this.rdbLnSearchByCurr = new System.Windows.Forms.RadioButton();
            this.rdbLnSearchByPow = new System.Windows.Forms.RadioButton();
            this.pnlLnSearchPow2 = new System.Windows.Forms.Panel();
            this.lblLnSearchPow2 = new System.Windows.Forms.Label();
            this.dinLnSearchPow2 = new DevComponents.Editors.DoubleInput();
            this.lblLnSearchPow2Unit = new System.Windows.Forms.Label();
            this.pnlLnSearchCurr1 = new System.Windows.Forms.Panel();
            this.lblLnSearchCurr1 = new System.Windows.Forms.Label();
            this.dinLnSearchCurr1 = new DevComponents.Editors.DoubleInput();
            this.lblLnSearchCurr1Unit = new System.Windows.Forms.Label();
            this.pnlLnSearchPow1 = new System.Windows.Forms.Panel();
            this.lblLnSearchPow1 = new System.Windows.Forms.Label();
            this.dinLnSearchPow1 = new DevComponents.Editors.DoubleInput();
            this.lblLnSearchPow1Unit = new System.Windows.Forms.Label();
            this.grpSe2Setting = new System.Windows.Forms.GroupBox();
            this.pnlSe2SearchCurr2 = new System.Windows.Forms.Panel();
            this.lblSe2SearchCurr2 = new System.Windows.Forms.Label();
            this.dinSe2SearchCurr2 = new DevComponents.Editors.DoubleInput();
            this.lblSe2SearchCurr2Unit = new System.Windows.Forms.Label();
            this.rdbSe2SearchByCurr = new System.Windows.Forms.RadioButton();
            this.rdbSe2SearchByPow = new System.Windows.Forms.RadioButton();
            this.pnlSe2SearchPow2 = new System.Windows.Forms.Panel();
            this.lblSe2SearchPow2 = new System.Windows.Forms.Label();
            this.dinSe2SearchPow2 = new DevComponents.Editors.DoubleInput();
            this.lblSe2SearchPow2Unit = new System.Windows.Forms.Label();
            this.pnlSe2SearchCurr1 = new System.Windows.Forms.Panel();
            this.lblSe2SearchCurr1 = new System.Windows.Forms.Label();
            this.dinSe2SearchCurr1 = new DevComponents.Editors.DoubleInput();
            this.lblSe2SearchCurr1Unit = new System.Windows.Forms.Label();
            this.pnlSe2SearchPow1 = new System.Windows.Forms.Panel();
            this.lblSe2SearchPow1 = new System.Windows.Forms.Label();
            this.dinSe2SearchPow1 = new DevComponents.Editors.DoubleInput();
            this.lblSe2SearchPow1Unit = new System.Windows.Forms.Label();
            this.grpThresholdPoints = new System.Windows.Forms.GroupBox();
            this.pnlThresholdSearchValue2 = new System.Windows.Forms.Panel();
            this.lblThresholdSearchValue2 = new System.Windows.Forms.Label();
            this.dinThresholdSearchValue2 = new DevComponents.Editors.DoubleInput();
            this.lblThresholdSearchValueUnit2 = new System.Windows.Forms.Label();
            this.pnlThresholdSearchValue = new System.Windows.Forms.Panel();
            this.lblThresholdSearchValue = new System.Windows.Forms.Label();
            this.dinThresholdSearchValue = new DevComponents.Editors.DoubleInput();
            this.lblThresholdSearchValueUnit = new System.Windows.Forms.Label();
            this.pnlThresholdSearchCurr2 = new System.Windows.Forms.Panel();
            this.lblThresholdSearchCurr2 = new System.Windows.Forms.Label();
            this.dinThresholdSearchCurr2 = new DevComponents.Editors.DoubleInput();
            this.lblThresholdSearchCurr2Unit = new System.Windows.Forms.Label();
            this.rdbThresholdSearchByCurr = new System.Windows.Forms.RadioButton();
            this.rdbThresholdSearchByPow = new System.Windows.Forms.RadioButton();
            this.pnlThresholdSearchPow2 = new System.Windows.Forms.Panel();
            this.lblThresholdSearchPow2 = new System.Windows.Forms.Label();
            this.dinThresholdSearchPow2 = new DevComponents.Editors.DoubleInput();
            this.lblThresholdSearchPow2Unit = new System.Windows.Forms.Label();
            this.pnlThresholdSearchPow1 = new System.Windows.Forms.Panel();
            this.lblThresholdSearchPow1 = new System.Windows.Forms.Label();
            this.dinThresholdSearchPow1 = new DevComponents.Editors.DoubleInput();
            this.lblThresholdSearchPow1Unit = new System.Windows.Forms.Label();
            this.pnlThresholdSearchCurr1 = new System.Windows.Forms.Panel();
            this.lblThresholdSearchCurr1 = new System.Windows.Forms.Label();
            this.dinThresholdSearchCurr1 = new DevComponents.Editors.DoubleInput();
            this.lblThresholdSearchCurr1Unit = new System.Windows.Forms.Label();
            this.grpSpecificPoint = new System.Windows.Forms.GroupBox();
            this.pnlSpecificCurr3 = new System.Windows.Forms.Panel();
            this.lblSpecificCurr3 = new System.Windows.Forms.Label();
            this.dinSpecificCurr3 = new DevComponents.Editors.DoubleInput();
            this.lblSpecificCurrUnit3 = new System.Windows.Forms.Label();
            this.pnlSpecificCurr2 = new System.Windows.Forms.Panel();
            this.lblSpecificCurr2 = new System.Windows.Forms.Label();
            this.dinSpecificCurr2 = new DevComponents.Editors.DoubleInput();
            this.lblSpecificCurrUnit2 = new System.Windows.Forms.Label();
            this.pnlSpecificCurr1 = new System.Windows.Forms.Panel();
            this.lblSpecificCurr1 = new System.Windows.Forms.Label();
            this.dinSpecificCurr1 = new DevComponents.Editors.DoubleInput();
            this.lblSpecificCurrUnit1 = new System.Windows.Forms.Label();
            this.grpRsSetting = new System.Windows.Forms.GroupBox();
            this.pnlRsSearchCurr2 = new System.Windows.Forms.Panel();
            this.lblRsSearchCurr2 = new System.Windows.Forms.Label();
            this.dinRsSearchCurr2 = new DevComponents.Editors.DoubleInput();
            this.lblRsSearchCurr2Unit = new System.Windows.Forms.Label();
            this.rdbRsSearchByCurr = new System.Windows.Forms.RadioButton();
            this.rdbRsSearchByPow = new System.Windows.Forms.RadioButton();
            this.pnlRsSearchPow2 = new System.Windows.Forms.Panel();
            this.lblRsSearchPow2 = new System.Windows.Forms.Label();
            this.dinRsSearchPow2 = new DevComponents.Editors.DoubleInput();
            this.lblRsSearchPow2Unit = new System.Windows.Forms.Label();
            this.pnlRsSearchCurr1 = new System.Windows.Forms.Panel();
            this.lblRsSearchCurr1 = new System.Windows.Forms.Label();
            this.dinRsSearchCurr1 = new DevComponents.Editors.DoubleInput();
            this.lblRsSearchCurr1Unit = new System.Windows.Forms.Label();
            this.pnlRsSearchPow1 = new System.Windows.Forms.Panel();
            this.lblRsSearchPow1 = new System.Windows.Forms.Label();
            this.dinRsSearchPow1 = new DevComponents.Editors.DoubleInput();
            this.lblRsSearchPow1Unit = new System.Windows.Forms.Label();
            this.grpOperationPoints = new System.Windows.Forms.GroupBox();
            this.PnlPop = new System.Windows.Forms.Panel();
            this.lblPop = new System.Windows.Forms.Label();
            this.dinPop = new DevComponents.Editors.DoubleInput();
            this.lblPopUnit = new System.Windows.Forms.Label();
            this.grpKinkSetting = new System.Windows.Forms.GroupBox();
            this.pnlKinkRatio = new System.Windows.Forms.Panel();
            this.lblKinkRatio = new System.Windows.Forms.Label();
            this.dinKinkRatio = new DevComponents.Editors.DoubleInput();
            this.lblKinkRatioUnit = new System.Windows.Forms.Label();
            this.pnlKinkSearchCurr2 = new System.Windows.Forms.Panel();
            this.lblKinkSearchCurr2 = new System.Windows.Forms.Label();
            this.dinKinkSearchCurr2 = new DevComponents.Editors.DoubleInput();
            this.lblKinkSearchCurr2Unit = new System.Windows.Forms.Label();
            this.pnlKinkSearchCurr1 = new System.Windows.Forms.Panel();
            this.lblKinkSearchCurr1 = new System.Windows.Forms.Label();
            this.dinKinkSearchCurr1 = new DevComponents.Editors.DoubleInput();
            this.lblKinkSearchCurr1Unit = new System.Windows.Forms.Label();
            this.grpAdvancedSettings = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdbThresholdCalcPo = new System.Windows.Forms.RadioButton();
            this.rdbThresholdCalcThr = new System.Windows.Forms.RadioButton();
            this.rdbThresholdCalc2Point = new System.Windows.Forms.RadioButton();
            this.lblThresholdCalcSelection = new System.Windows.Forms.Label();
            this.rdbThresholdCalcLR = new System.Windows.Forms.RadioButton();
            this.pnlKinkCalcSelection = new System.Windows.Forms.Panel();
            this.rdbKinkCalcSecondOrder = new System.Windows.Forms.RadioButton();
            this.rdbKinkCalcFittingLine = new System.Windows.Forms.RadioButton();
            this.rdbKinkCalcRefCurve = new System.Windows.Forms.RadioButton();
            this.rdbKinkCalcDeltaPow = new System.Windows.Forms.RadioButton();
            this.rdbKinkCalcSEk = new System.Windows.Forms.RadioButton();
            this.lblKinkCalcSelection = new System.Windows.Forms.Label();
            this.pnlIVopSelectMode = new System.Windows.Forms.Panel();
            this.rdbOpInterpolation = new System.Windows.Forms.RadioButton();
            this.lblIVopSelection = new System.Windows.Forms.Label();
            this.rdbOpClosestpoint = new System.Windows.Forms.RadioButton();
            this.pnlSeCalcSelection = new System.Windows.Forms.Panel();
            this.rdbSeCalcAvg = new System.Windows.Forms.RadioButton();
            this.rdbSeCalc2Point = new System.Windows.Forms.RadioButton();
            this.lblSeCalcSelection = new System.Windows.Forms.Label();
            this.rdbSeCalcLR = new System.Windows.Forms.RadioButton();
            this.pnlRsCalcSelection = new System.Windows.Forms.Panel();
            this.rdbRsCalcAvg = new System.Windows.Forms.RadioButton();
            this.rdbRsCalc2Point = new System.Windows.Forms.RadioButton();
            this.lblRsCalcSelection = new System.Windows.Forms.Label();
            this.rdbRsCalcLR = new System.Windows.Forms.RadioButton();
            this.grpSeSetting = new System.Windows.Forms.GroupBox();
            this.pnlSeSearchCurr2 = new System.Windows.Forms.Panel();
            this.lblSeSearchCurr2 = new System.Windows.Forms.Label();
            this.dinSeSearchCurr2 = new DevComponents.Editors.DoubleInput();
            this.lblSeSearchCurr2Unit = new System.Windows.Forms.Label();
            this.rdbSeSearchByCurr = new System.Windows.Forms.RadioButton();
            this.rdbSeSearchByPow = new System.Windows.Forms.RadioButton();
            this.pnlSeSearchPow2 = new System.Windows.Forms.Panel();
            this.lblSeSearchPow2 = new System.Windows.Forms.Label();
            this.dinSeSearchPow2 = new DevComponents.Editors.DoubleInput();
            this.lblSeSearchPow2Unit = new System.Windows.Forms.Label();
            this.pnlSeSearchCurr1 = new System.Windows.Forms.Panel();
            this.lblSeSearchCurr1 = new System.Windows.Forms.Label();
            this.dinSeSearchCurr1 = new DevComponents.Editors.DoubleInput();
            this.lblSeSearchCurr1Unit = new System.Windows.Forms.Label();
            this.pnlSeSearchPow1 = new System.Windows.Forms.Panel();
            this.lblSeSearchPow1 = new System.Windows.Forms.Label();
            this.dinSeSearchPow1 = new DevComponents.Editors.DoubleInput();
            this.lblSeSearchPow1Unit = new System.Windows.Forms.Label();
            this.grpMovingAverage = new System.Windows.Forms.GroupBox();
            this.pnlIPMovingAvg = new System.Windows.Forms.Panel();
            this.lblIPMovingAvg = new System.Windows.Forms.Label();
            this.dinIPMovingAvg = new DevComponents.Editors.DoubleInput();
            this.lblIPMovingAvgUnit = new System.Windows.Forms.Label();
            this.pnlIVMovingAvg = new System.Windows.Forms.Panel();
            this.lblIVMovingAvg = new System.Windows.Forms.Label();
            this.dinIVMovingAvg = new DevComponents.Editors.DoubleInput();
            this.lblIVMovingAvgUnit = new System.Windows.Forms.Label();
            this.grpRollOver = new System.Windows.Forms.GroupBox();
            this.pnlRollOver = new System.Windows.Forms.Panel();
            this.lblRollOver = new System.Windows.Forms.Label();
            this.dinRollOver = new DevComponents.Editors.DoubleInput();
            this.lblRollOverUnit = new System.Windows.Forms.Label();
            this.grpItemCondition = new System.Windows.Forms.GroupBox();
            this.rdbOpOnFittingLine = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpChannel.SuspendLayout();
            this.grpApplySetting.SuspendLayout();
            this.pnlStepPulseCnt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStepPulseCnt)).BeginInit();
            this.pnlSourceFunc.SuspendLayout();
            this.pnlDutyCycle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinDutyCycle)).BeginInit();
            this.pnlTurnOffTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinTurnOffTime)).BeginInit();
            this.pnlStepValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinStepValue)).BeginInit();
            this.pnlPoints.SuspendLayout();
            this.pnlEndValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinEndValue)).BeginInit();
            this.pnlForceTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceTime)).BeginInit();
            this.pnlStartValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinStartValue)).BeginInit();
            this.pnlWaitTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDelay)).BeginInit();
            this.pnlForceRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.grpMsrtSetting.SuspendLayout();
            this.pnlQcwFilterCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQcwFilterCount)).BeginInit();
            this.pnlMsrtClamp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).BeginInit();
            this.pnlMsrtRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtRange)).BeginInit();
            this.pnlNPLC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinNPLC)).BeginInit();
            this.pnlFilterCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMsrtFilterCount)).BeginInit();
            this.grpPDDetector.SuspendLayout();
            this.pnlDetectorBiasVoltage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinDetectorBiasVoltage)).BeginInit();
            this.pnlDetectorNPLC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinDetectorNPLC)).BeginInit();
            this.pnlDetectorMsrtRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinDetectorMsrtRange)).BeginInit();
            this.grpCalcSetting.SuspendLayout();
            this.grpLinearitySetting.SuspendLayout();
            this.pnlLnSearchCurr2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLnSearchCurr2)).BeginInit();
            this.pnlLnSearchPow2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLnSearchPow2)).BeginInit();
            this.pnlLnSearchCurr1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLnSearchCurr1)).BeginInit();
            this.pnlLnSearchPow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLnSearchPow1)).BeginInit();
            this.grpSe2Setting.SuspendLayout();
            this.pnlSe2SearchCurr2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSe2SearchCurr2)).BeginInit();
            this.pnlSe2SearchPow2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSe2SearchPow2)).BeginInit();
            this.pnlSe2SearchCurr1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSe2SearchCurr1)).BeginInit();
            this.pnlSe2SearchPow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSe2SearchPow1)).BeginInit();
            this.grpThresholdPoints.SuspendLayout();
            this.pnlThresholdSearchValue2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchValue2)).BeginInit();
            this.pnlThresholdSearchValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchValue)).BeginInit();
            this.pnlThresholdSearchCurr2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchCurr2)).BeginInit();
            this.pnlThresholdSearchPow2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchPow2)).BeginInit();
            this.pnlThresholdSearchPow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchPow1)).BeginInit();
            this.pnlThresholdSearchCurr1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchCurr1)).BeginInit();
            this.grpSpecificPoint.SuspendLayout();
            this.pnlSpecificCurr3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSpecificCurr3)).BeginInit();
            this.pnlSpecificCurr2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSpecificCurr2)).BeginInit();
            this.pnlSpecificCurr1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSpecificCurr1)).BeginInit();
            this.grpRsSetting.SuspendLayout();
            this.pnlRsSearchCurr2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRsSearchCurr2)).BeginInit();
            this.pnlRsSearchPow2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRsSearchPow2)).BeginInit();
            this.pnlRsSearchCurr1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRsSearchCurr1)).BeginInit();
            this.pnlRsSearchPow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRsSearchPow1)).BeginInit();
            this.grpOperationPoints.SuspendLayout();
            this.PnlPop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinPop)).BeginInit();
            this.grpKinkSetting.SuspendLayout();
            this.pnlKinkRatio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinKinkRatio)).BeginInit();
            this.pnlKinkSearchCurr2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinKinkSearchCurr2)).BeginInit();
            this.pnlKinkSearchCurr1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinKinkSearchCurr1)).BeginInit();
            this.grpAdvancedSettings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlKinkCalcSelection.SuspendLayout();
            this.pnlIVopSelectMode.SuspendLayout();
            this.pnlSeCalcSelection.SuspendLayout();
            this.pnlRsCalcSelection.SuspendLayout();
            this.grpSeSetting.SuspendLayout();
            this.pnlSeSearchCurr2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSeSearchCurr2)).BeginInit();
            this.pnlSeSearchPow2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSeSearchPow2)).BeginInit();
            this.pnlSeSearchCurr1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSeSearchCurr1)).BeginInit();
            this.pnlSeSearchPow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSeSearchPow1)).BeginInit();
            this.grpMovingAverage.SuspendLayout();
            this.pnlIPMovingAvg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinIPMovingAvg)).BeginInit();
            this.pnlIVMovingAvg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinIVMovingAvg)).BeginInit();
            this.grpRollOver.SuspendLayout();
            this.pnlRollOver.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRollOver)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
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
            this.grpApplySetting.Controls.Add(this.pnlStepPulseCnt);
            this.grpApplySetting.Controls.Add(this.pnlSourceFunc);
            this.grpApplySetting.Controls.Add(this.pnlDutyCycle);
            this.grpApplySetting.Controls.Add(this.pnlTurnOffTime);
            this.grpApplySetting.Controls.Add(this.pnlStepValue);
            this.grpApplySetting.Controls.Add(this.pnlPoints);
            this.grpApplySetting.Controls.Add(this.pnlEndValue);
            this.grpApplySetting.Controls.Add(this.pnlForceTime);
            this.grpApplySetting.Controls.Add(this.pnlStartValue);
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
            // pnlStepPulseCnt
            // 
            this.pnlStepPulseCnt.BackColor = System.Drawing.Color.Transparent;
            this.pnlStepPulseCnt.Controls.Add(this.lblStepPulseCnt);
            this.pnlStepPulseCnt.Controls.Add(this.numStepPulseCnt);
            resources.ApplyResources(this.pnlStepPulseCnt, "pnlStepPulseCnt");
            this.pnlStepPulseCnt.Name = "pnlStepPulseCnt";
            // 
            // lblStepPulseCnt
            // 
            resources.ApplyResources(this.lblStepPulseCnt, "lblStepPulseCnt");
            this.lblStepPulseCnt.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStepPulseCnt.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblStepPulseCnt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStepPulseCnt.ForeColor = System.Drawing.Color.Black;
            this.lblStepPulseCnt.Name = "lblStepPulseCnt";
            // 
            // numStepPulseCnt
            // 
            // 
            // 
            // 
            this.numStepPulseCnt.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numStepPulseCnt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numStepPulseCnt.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numStepPulseCnt.DisplayFormat = "0";
            resources.ApplyResources(this.numStepPulseCnt, "numStepPulseCnt");
            this.numStepPulseCnt.MaxValue = 200;
            this.numStepPulseCnt.MinValue = 2;
            this.numStepPulseCnt.Name = "numStepPulseCnt";
            this.numStepPulseCnt.ShowUpDown = true;
            this.numStepPulseCnt.Value = 2;
            // 
            // pnlSourceFunc
            // 
            this.pnlSourceFunc.Controls.Add(this.cmbSourceFunc);
            this.pnlSourceFunc.Controls.Add(this.lblSourceFunc);
            resources.ApplyResources(this.pnlSourceFunc, "pnlSourceFunc");
            this.pnlSourceFunc.Name = "pnlSourceFunc";
            // 
            // cmbSourceFunc
            // 
            this.cmbSourceFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceFunc.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbSourceFunc, "cmbSourceFunc");
            this.cmbSourceFunc.Name = "cmbSourceFunc";
            this.cmbSourceFunc.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.cmbSourceFunc.SelectedIndexChanged += new System.EventHandler(this.cmbSourceFunc_SelectedIndexChanged);
            // 
            // lblSourceFunc
            // 
            resources.ApplyResources(this.lblSourceFunc, "lblSourceFunc");
            this.lblSourceFunc.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblSourceFunc.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblSourceFunc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSourceFunc.ForeColor = System.Drawing.Color.Black;
            this.lblSourceFunc.Name = "lblSourceFunc";
            // 
            // pnlDutyCycle
            // 
            this.pnlDutyCycle.Controls.Add(this.lblDutyCycle);
            this.pnlDutyCycle.Controls.Add(this.dinDutyCycle);
            this.pnlDutyCycle.Controls.Add(this.lblDutyCycleUnit);
            resources.ApplyResources(this.pnlDutyCycle, "pnlDutyCycle");
            this.pnlDutyCycle.Name = "pnlDutyCycle";
            // 
            // lblDutyCycle
            // 
            resources.ApplyResources(this.lblDutyCycle, "lblDutyCycle");
            this.lblDutyCycle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDutyCycle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDutyCycle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDutyCycle.ForeColor = System.Drawing.Color.Black;
            this.lblDutyCycle.Name = "lblDutyCycle";
            // 
            // dinDutyCycle
            // 
            // 
            // 
            // 
            this.dinDutyCycle.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinDutyCycle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinDutyCycle.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinDutyCycle.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinDutyCycle, "dinDutyCycle");
            this.dinDutyCycle.Increment = 0.5D;
            this.dinDutyCycle.MaxValue = 99.9D;
            this.dinDutyCycle.MinValue = 0.1D;
            this.dinDutyCycle.Name = "dinDutyCycle";
            this.dinDutyCycle.ShowUpDown = true;
            this.dinDutyCycle.Value = 1D;
            // 
            // lblDutyCycleUnit
            // 
            resources.ApplyResources(this.lblDutyCycleUnit, "lblDutyCycleUnit");
            this.lblDutyCycleUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDutyCycleUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDutyCycleUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDutyCycleUnit.ForeColor = System.Drawing.Color.Black;
            this.lblDutyCycleUnit.Name = "lblDutyCycleUnit";
            // 
            // pnlTurnOffTime
            // 
            this.pnlTurnOffTime.Controls.Add(this.lblTurnOffTime);
            this.pnlTurnOffTime.Controls.Add(this.dinTurnOffTime);
            this.pnlTurnOffTime.Controls.Add(this.lblTurnOffTimeUnit);
            resources.ApplyResources(this.pnlTurnOffTime, "pnlTurnOffTime");
            this.pnlTurnOffTime.Name = "pnlTurnOffTime";
            // 
            // lblTurnOffTime
            // 
            resources.ApplyResources(this.lblTurnOffTime, "lblTurnOffTime");
            this.lblTurnOffTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblTurnOffTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblTurnOffTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTurnOffTime.ForeColor = System.Drawing.Color.Black;
            this.lblTurnOffTime.Name = "lblTurnOffTime";
            // 
            // dinTurnOffTime
            // 
            // 
            // 
            // 
            this.dinTurnOffTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinTurnOffTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinTurnOffTime.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinTurnOffTime.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinTurnOffTime, "dinTurnOffTime");
            this.dinTurnOffTime.Increment = 0.5D;
            this.dinTurnOffTime.MaxValue = 1000000D;
            this.dinTurnOffTime.MinValue = 0D;
            this.dinTurnOffTime.Name = "dinTurnOffTime";
            this.dinTurnOffTime.ShowUpDown = true;
            // 
            // lblTurnOffTimeUnit
            // 
            resources.ApplyResources(this.lblTurnOffTimeUnit, "lblTurnOffTimeUnit");
            this.lblTurnOffTimeUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblTurnOffTimeUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblTurnOffTimeUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTurnOffTimeUnit.ForeColor = System.Drawing.Color.Black;
            this.lblTurnOffTimeUnit.Name = "lblTurnOffTimeUnit";
            // 
            // pnlStepValue
            // 
            this.pnlStepValue.Controls.Add(this.dinStepValue);
            this.pnlStepValue.Controls.Add(this.lblStepValue);
            this.pnlStepValue.Controls.Add(this.lblStepValueUnit);
            resources.ApplyResources(this.pnlStepValue, "pnlStepValue");
            this.pnlStepValue.Name = "pnlStepValue";
            // 
            // dinStepValue
            // 
            // 
            // 
            // 
            this.dinStepValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinStepValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinStepValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinStepValue.DisplayFormat = "0.0000";
            resources.ApplyResources(this.dinStepValue, "dinStepValue");
            this.dinStepValue.Increment = 1D;
            this.dinStepValue.MaxValue = 10000D;
            this.dinStepValue.MinValue = 0D;
            this.dinStepValue.Name = "dinStepValue";
            this.dinStepValue.ShowUpDown = true;
            this.dinStepValue.Value = 0.01D;
            // 
            // lblStepValue
            // 
            resources.ApplyResources(this.lblStepValue, "lblStepValue");
            this.lblStepValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStepValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblStepValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStepValue.ForeColor = System.Drawing.Color.Black;
            this.lblStepValue.Name = "lblStepValue";
            // 
            // lblStepValueUnit
            // 
            resources.ApplyResources(this.lblStepValueUnit, "lblStepValueUnit");
            this.lblStepValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStepValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblStepValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStepValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblStepValueUnit.Name = "lblStepValueUnit";
            // 
            // pnlPoints
            // 
            this.pnlPoints.Controls.Add(this.txtDisplayPointsValue);
            this.pnlPoints.Controls.Add(this.lblPoints);
            this.pnlPoints.Controls.Add(this.lblPointsUnit);
            resources.ApplyResources(this.pnlPoints, "pnlPoints");
            this.pnlPoints.Name = "pnlPoints";
            // 
            // txtDisplayPointsValue
            // 
            this.txtDisplayPointsValue.BackColor = System.Drawing.Color.Gainsboro;
            // 
            // 
            // 
            this.txtDisplayPointsValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.txtDisplayPointsValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.txtDisplayPointsValue, "txtDisplayPointsValue");
            this.txtDisplayPointsValue.ForeColor = System.Drawing.Color.Black;
            this.txtDisplayPointsValue.Name = "txtDisplayPointsValue";
            this.txtDisplayPointsValue.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblPoints
            // 
            resources.ApplyResources(this.lblPoints, "lblPoints");
            this.lblPoints.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPoints.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblPoints.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPoints.ForeColor = System.Drawing.Color.Black;
            this.lblPoints.Name = "lblPoints";
            // 
            // lblPointsUnit
            // 
            resources.ApplyResources(this.lblPointsUnit, "lblPointsUnit");
            this.lblPointsUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPointsUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblPointsUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPointsUnit.ForeColor = System.Drawing.Color.Black;
            this.lblPointsUnit.Name = "lblPointsUnit";
            // 
            // pnlEndValue
            // 
            this.pnlEndValue.Controls.Add(this.lblEndValue);
            this.pnlEndValue.Controls.Add(this.dinEndValue);
            this.pnlEndValue.Controls.Add(this.lblEndValueUnit);
            resources.ApplyResources(this.pnlEndValue, "pnlEndValue");
            this.pnlEndValue.Name = "pnlEndValue";
            // 
            // lblEndValue
            // 
            resources.ApplyResources(this.lblEndValue, "lblEndValue");
            this.lblEndValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblEndValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblEndValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblEndValue.ForeColor = System.Drawing.Color.Black;
            this.lblEndValue.Name = "lblEndValue";
            // 
            // dinEndValue
            // 
            // 
            // 
            // 
            this.dinEndValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinEndValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinEndValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinEndValue.DisplayFormat = "0.0000";
            resources.ApplyResources(this.dinEndValue, "dinEndValue");
            this.dinEndValue.Increment = 1D;
            this.dinEndValue.MaxValue = 10000D;
            this.dinEndValue.MinValue = 0D;
            this.dinEndValue.Name = "dinEndValue";
            this.dinEndValue.ShowUpDown = true;
            this.dinEndValue.Value = 0.01D;
            // 
            // lblEndValueUnit
            // 
            resources.ApplyResources(this.lblEndValueUnit, "lblEndValueUnit");
            this.lblEndValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblEndValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblEndValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblEndValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblEndValueUnit.Name = "lblEndValueUnit";
            // 
            // pnlForceTime
            // 
            this.pnlForceTime.Controls.Add(this.lblPulseWidth);
            this.pnlForceTime.Controls.Add(this.lblForceTime);
            this.pnlForceTime.Controls.Add(this.dinForceTime);
            this.pnlForceTime.Controls.Add(this.lblForceTimeUnit);
            resources.ApplyResources(this.pnlForceTime, "pnlForceTime");
            this.pnlForceTime.Name = "pnlForceTime";
            // 
            // lblPulseWidth
            // 
            resources.ApplyResources(this.lblPulseWidth, "lblPulseWidth");
            this.lblPulseWidth.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPulseWidth.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblPulseWidth.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPulseWidth.ForeColor = System.Drawing.Color.Black;
            this.lblPulseWidth.Name = "lblPulseWidth";
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
            // pnlStartValue
            // 
            this.pnlStartValue.Controls.Add(this.lblStartValue);
            this.pnlStartValue.Controls.Add(this.dinStartValue);
            this.pnlStartValue.Controls.Add(this.lblStartValueUnit);
            resources.ApplyResources(this.pnlStartValue, "pnlStartValue");
            this.pnlStartValue.Name = "pnlStartValue";
            // 
            // lblStartValue
            // 
            resources.ApplyResources(this.lblStartValue, "lblStartValue");
            this.lblStartValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStartValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblStartValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStartValue.ForeColor = System.Drawing.Color.Black;
            this.lblStartValue.Name = "lblStartValue";
            // 
            // dinStartValue
            // 
            // 
            // 
            // 
            this.dinStartValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinStartValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinStartValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinStartValue.DisplayFormat = "0.0000";
            resources.ApplyResources(this.dinStartValue, "dinStartValue");
            this.dinStartValue.Increment = 1D;
            this.dinStartValue.MaxValue = 10000D;
            this.dinStartValue.MinValue = 0D;
            this.dinStartValue.Name = "dinStartValue";
            this.dinStartValue.ShowUpDown = true;
            this.dinStartValue.Value = 0.01D;
            // 
            // lblStartValueUnit
            // 
            resources.ApplyResources(this.lblStartValueUnit, "lblStartValueUnit");
            this.lblStartValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblStartValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblStartValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblStartValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblStartValueUnit.Name = "lblStartValueUnit";
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
            this.splitContainer2.Panel1.Controls.Add(this.grpPDDetector);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grpCalcSetting);
            // 
            // grpMsrtSetting
            // 
            this.grpMsrtSetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpMsrtSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpMsrtSetting.Controls.Add(this.pnlQcwFilterCount);
            this.grpMsrtSetting.Controls.Add(this.pnlMsrtClamp);
            this.grpMsrtSetting.Controls.Add(this.pnlMsrtRange);
            this.grpMsrtSetting.Controls.Add(this.pnlNPLC);
            this.grpMsrtSetting.Controls.Add(this.pnlFilterCount);
            resources.ApplyResources(this.grpMsrtSetting, "grpMsrtSetting");
            this.grpMsrtSetting.DrawTitleBox = false;
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
            this.grpMsrtSetting.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
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
            // pnlQcwFilterCount
            // 
            this.pnlQcwFilterCount.BackColor = System.Drawing.Color.Transparent;
            this.pnlQcwFilterCount.Controls.Add(this.lblQcwFilterCount);
            this.pnlQcwFilterCount.Controls.Add(this.numQcwFilterCount);
            resources.ApplyResources(this.pnlQcwFilterCount, "pnlQcwFilterCount");
            this.pnlQcwFilterCount.Name = "pnlQcwFilterCount";
            // 
            // lblQcwFilterCount
            // 
            resources.ApplyResources(this.lblQcwFilterCount, "lblQcwFilterCount");
            this.lblQcwFilterCount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblQcwFilterCount.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblQcwFilterCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblQcwFilterCount.ForeColor = System.Drawing.Color.Black;
            this.lblQcwFilterCount.Name = "lblQcwFilterCount";
            // 
            // numQcwFilterCount
            // 
            // 
            // 
            // 
            this.numQcwFilterCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numQcwFilterCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numQcwFilterCount.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numQcwFilterCount.DisplayFormat = "0";
            resources.ApplyResources(this.numQcwFilterCount, "numQcwFilterCount");
            this.numQcwFilterCount.MaxValue = 200;
            this.numQcwFilterCount.MinValue = 1;
            this.numQcwFilterCount.Name = "numQcwFilterCount";
            this.numQcwFilterCount.ShowUpDown = true;
            this.numQcwFilterCount.Value = 1;
            // 
            // pnlMsrtClamp
            // 
            this.pnlMsrtClamp.BackColor = System.Drawing.Color.Transparent;
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
            this.pnlMsrtRange.BackColor = System.Drawing.Color.Transparent;
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
            // pnlNPLC
            // 
            this.pnlNPLC.BackColor = System.Drawing.Color.Transparent;
            this.pnlNPLC.Controls.Add(this.lblNPLC);
            this.pnlNPLC.Controls.Add(this.dinNPLC);
            this.pnlNPLC.Controls.Add(this.lblNplcAuto);
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
            // lblNplcAuto
            // 
            resources.ApplyResources(this.lblNplcAuto, "lblNplcAuto");
            this.lblNplcAuto.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblNplcAuto.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblNplcAuto.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblNplcAuto.ForeColor = System.Drawing.Color.DimGray;
            this.lblNplcAuto.Name = "lblNplcAuto";
            this.lblNplcAuto.SingleLineColor = System.Drawing.Color.DarkSlateGray;
            // 
            // pnlFilterCount
            // 
            this.pnlFilterCount.BackColor = System.Drawing.Color.Transparent;
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
            // grpPDDetector
            // 
            this.grpPDDetector.CanvasColor = System.Drawing.Color.Empty;
            this.grpPDDetector.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpPDDetector.Controls.Add(this.pnlDetectorBiasVoltage);
            this.grpPDDetector.Controls.Add(this.pnlDetectorNPLC);
            this.grpPDDetector.Controls.Add(this.pnlDetectorMsrtRange);
            resources.ApplyResources(this.grpPDDetector, "grpPDDetector");
            this.grpPDDetector.DrawTitleBox = false;
            this.grpPDDetector.Name = "grpPDDetector";
            // 
            // 
            // 
            this.grpPDDetector.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpPDDetector.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpPDDetector.Style.BackColorGradientAngle = 90;
            this.grpPDDetector.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPDDetector.Style.BorderBottomWidth = 1;
            this.grpPDDetector.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpPDDetector.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPDDetector.Style.BorderLeftWidth = 1;
            this.grpPDDetector.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPDDetector.Style.BorderRightWidth = 1;
            this.grpPDDetector.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPDDetector.Style.BorderTopWidth = 1;
            this.grpPDDetector.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpPDDetector.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpPDDetector.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpPDDetector.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpPDDetector.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpPDDetector.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpPDDetector.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpPDDetector.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // pnlDetectorBiasVoltage
            // 
            this.pnlDetectorBiasVoltage.BackColor = System.Drawing.Color.Transparent;
            this.pnlDetectorBiasVoltage.Controls.Add(this.lblDetectorBiasVoltageUnit);
            this.pnlDetectorBiasVoltage.Controls.Add(this.lblDetectorBiasVoltage);
            this.pnlDetectorBiasVoltage.Controls.Add(this.dinDetectorBiasVoltage);
            resources.ApplyResources(this.pnlDetectorBiasVoltage, "pnlDetectorBiasVoltage");
            this.pnlDetectorBiasVoltage.Name = "pnlDetectorBiasVoltage";
            // 
            // lblDetectorBiasVoltageUnit
            // 
            resources.ApplyResources(this.lblDetectorBiasVoltageUnit, "lblDetectorBiasVoltageUnit");
            this.lblDetectorBiasVoltageUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDetectorBiasVoltageUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDetectorBiasVoltageUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDetectorBiasVoltageUnit.ForeColor = System.Drawing.Color.Black;
            this.lblDetectorBiasVoltageUnit.Name = "lblDetectorBiasVoltageUnit";
            // 
            // lblDetectorBiasVoltage
            // 
            this.lblDetectorBiasVoltage.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDetectorBiasVoltage.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDetectorBiasVoltage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblDetectorBiasVoltage, "lblDetectorBiasVoltage");
            this.lblDetectorBiasVoltage.ForeColor = System.Drawing.Color.Green;
            this.lblDetectorBiasVoltage.Name = "lblDetectorBiasVoltage";
            // 
            // dinDetectorBiasVoltage
            // 
            // 
            // 
            // 
            this.dinDetectorBiasVoltage.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinDetectorBiasVoltage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinDetectorBiasVoltage.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinDetectorBiasVoltage.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinDetectorBiasVoltage, "dinDetectorBiasVoltage");
            this.dinDetectorBiasVoltage.Increment = 1D;
            this.dinDetectorBiasVoltage.MaxValue = 20D;
            this.dinDetectorBiasVoltage.MinValue = -20D;
            this.dinDetectorBiasVoltage.Name = "dinDetectorBiasVoltage";
            this.dinDetectorBiasVoltage.ShowUpDown = true;
            // 
            // pnlDetectorNPLC
            // 
            this.pnlDetectorNPLC.BackColor = System.Drawing.Color.Transparent;
            this.pnlDetectorNPLC.Controls.Add(this.lblDetectorNPLC);
            this.pnlDetectorNPLC.Controls.Add(this.dinDetectorNPLC);
            this.pnlDetectorNPLC.Controls.Add(this.lblDetectorNplcAuto1);
            resources.ApplyResources(this.pnlDetectorNPLC, "pnlDetectorNPLC");
            this.pnlDetectorNPLC.Name = "pnlDetectorNPLC";
            // 
            // lblDetectorNPLC
            // 
            resources.ApplyResources(this.lblDetectorNPLC, "lblDetectorNPLC");
            this.lblDetectorNPLC.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDetectorNPLC.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDetectorNPLC.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDetectorNPLC.ForeColor = System.Drawing.Color.Green;
            this.lblDetectorNPLC.Name = "lblDetectorNPLC";
            // 
            // dinDetectorNPLC
            // 
            // 
            // 
            // 
            this.dinDetectorNPLC.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinDetectorNPLC.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinDetectorNPLC.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinDetectorNPLC.DisplayFormat = "0.000";
            resources.ApplyResources(this.dinDetectorNPLC, "dinDetectorNPLC");
            this.dinDetectorNPLC.Increment = 0.01D;
            this.dinDetectorNPLC.MaxValue = 1D;
            this.dinDetectorNPLC.MinValue = 0.001D;
            this.dinDetectorNPLC.Name = "dinDetectorNPLC";
            this.dinDetectorNPLC.ShowUpDown = true;
            this.dinDetectorNPLC.Value = 0.01D;
            // 
            // lblDetectorNplcAuto1
            // 
            resources.ApplyResources(this.lblDetectorNplcAuto1, "lblDetectorNplcAuto1");
            this.lblDetectorNplcAuto1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDetectorNplcAuto1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDetectorNplcAuto1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDetectorNplcAuto1.ForeColor = System.Drawing.Color.DimGray;
            this.lblDetectorNplcAuto1.Name = "lblDetectorNplcAuto1";
            this.lblDetectorNplcAuto1.SingleLineColor = System.Drawing.Color.DarkSlateGray;
            // 
            // pnlDetectorMsrtRange
            // 
            this.pnlDetectorMsrtRange.BackColor = System.Drawing.Color.Transparent;
            this.pnlDetectorMsrtRange.Controls.Add(this.lblDetectorMsrtRangeUnit);
            this.pnlDetectorMsrtRange.Controls.Add(this.lblDetectorMsrtRange);
            this.pnlDetectorMsrtRange.Controls.Add(this.dinDetectorMsrtRange);
            resources.ApplyResources(this.pnlDetectorMsrtRange, "pnlDetectorMsrtRange");
            this.pnlDetectorMsrtRange.Name = "pnlDetectorMsrtRange";
            // 
            // lblDetectorMsrtRangeUnit
            // 
            resources.ApplyResources(this.lblDetectorMsrtRangeUnit, "lblDetectorMsrtRangeUnit");
            this.lblDetectorMsrtRangeUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDetectorMsrtRangeUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDetectorMsrtRangeUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDetectorMsrtRangeUnit.ForeColor = System.Drawing.Color.Black;
            this.lblDetectorMsrtRangeUnit.Name = "lblDetectorMsrtRangeUnit";
            // 
            // lblDetectorMsrtRange
            // 
            this.lblDetectorMsrtRange.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDetectorMsrtRange.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblDetectorMsrtRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblDetectorMsrtRange, "lblDetectorMsrtRange");
            this.lblDetectorMsrtRange.ForeColor = System.Drawing.Color.Green;
            this.lblDetectorMsrtRange.Name = "lblDetectorMsrtRange";
            // 
            // dinDetectorMsrtRange
            // 
            // 
            // 
            // 
            this.dinDetectorMsrtRange.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinDetectorMsrtRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinDetectorMsrtRange.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinDetectorMsrtRange.DisplayFormat = "0.000";
            resources.ApplyResources(this.dinDetectorMsrtRange, "dinDetectorMsrtRange");
            this.dinDetectorMsrtRange.Increment = 1D;
            this.dinDetectorMsrtRange.MaxValue = 150D;
            this.dinDetectorMsrtRange.MinValue = 0.001D;
            this.dinDetectorMsrtRange.Name = "dinDetectorMsrtRange";
            this.dinDetectorMsrtRange.ShowUpDown = true;
            this.dinDetectorMsrtRange.Value = 1D;
            // 
            // grpCalcSetting
            // 
            this.grpCalcSetting.BackColor = System.Drawing.Color.Transparent;
            this.grpCalcSetting.CanvasColor = System.Drawing.Color.Empty;
            this.grpCalcSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpCalcSetting.Controls.Add(this.grpLinearitySetting);
            this.grpCalcSetting.Controls.Add(this.grpSe2Setting);
            this.grpCalcSetting.Controls.Add(this.grpThresholdPoints);
            this.grpCalcSetting.Controls.Add(this.grpSpecificPoint);
            this.grpCalcSetting.Controls.Add(this.grpRsSetting);
            this.grpCalcSetting.Controls.Add(this.grpOperationPoints);
            this.grpCalcSetting.Controls.Add(this.grpKinkSetting);
            this.grpCalcSetting.Controls.Add(this.grpAdvancedSettings);
            this.grpCalcSetting.Controls.Add(this.grpSeSetting);
            this.grpCalcSetting.Controls.Add(this.grpMovingAverage);
            this.grpCalcSetting.Controls.Add(this.grpRollOver);
            resources.ApplyResources(this.grpCalcSetting, "grpCalcSetting");
            this.grpCalcSetting.DrawTitleBox = false;
            this.grpCalcSetting.Name = "grpCalcSetting";
            // 
            // 
            // 
            this.grpCalcSetting.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpCalcSetting.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpCalcSetting.Style.BackColorGradientAngle = 90;
            this.grpCalcSetting.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCalcSetting.Style.BorderBottomWidth = 1;
            this.grpCalcSetting.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpCalcSetting.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCalcSetting.Style.BorderLeftWidth = 1;
            this.grpCalcSetting.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCalcSetting.Style.BorderRightWidth = 1;
            this.grpCalcSetting.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCalcSetting.Style.BorderTopWidth = 1;
            this.grpCalcSetting.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpCalcSetting.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpCalcSetting.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpCalcSetting.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpCalcSetting.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpCalcSetting.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpCalcSetting.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpCalcSetting.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // grpLinearitySetting
            // 
            this.grpLinearitySetting.Controls.Add(this.pnlLnSearchCurr2);
            this.grpLinearitySetting.Controls.Add(this.rdbLnSearchByCurr);
            this.grpLinearitySetting.Controls.Add(this.rdbLnSearchByPow);
            this.grpLinearitySetting.Controls.Add(this.pnlLnSearchPow2);
            this.grpLinearitySetting.Controls.Add(this.pnlLnSearchCurr1);
            this.grpLinearitySetting.Controls.Add(this.pnlLnSearchPow1);
            resources.ApplyResources(this.grpLinearitySetting, "grpLinearitySetting");
            this.grpLinearitySetting.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpLinearitySetting.Name = "grpLinearitySetting";
            this.grpLinearitySetting.TabStop = false;
            // 
            // pnlLnSearchCurr2
            // 
            this.pnlLnSearchCurr2.Controls.Add(this.lblLnSearchCurr2);
            this.pnlLnSearchCurr2.Controls.Add(this.dinLnSearchCurr2);
            this.pnlLnSearchCurr2.Controls.Add(this.lblLnSearchCurr2Unit);
            resources.ApplyResources(this.pnlLnSearchCurr2, "pnlLnSearchCurr2");
            this.pnlLnSearchCurr2.Name = "pnlLnSearchCurr2";
            // 
            // lblLnSearchCurr2
            // 
            resources.ApplyResources(this.lblLnSearchCurr2, "lblLnSearchCurr2");
            this.lblLnSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.lblLnSearchCurr2.Name = "lblLnSearchCurr2";
            // 
            // dinLnSearchCurr2
            // 
            // 
            // 
            // 
            this.dinLnSearchCurr2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLnSearchCurr2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinLnSearchCurr2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinLnSearchCurr2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinLnSearchCurr2, "dinLnSearchCurr2");
            this.dinLnSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.dinLnSearchCurr2.Increment = 1D;
            this.dinLnSearchCurr2.MaxValue = 10000D;
            this.dinLnSearchCurr2.MinValue = 0D;
            this.dinLnSearchCurr2.Name = "dinLnSearchCurr2";
            this.dinLnSearchCurr2.ShowUpDown = true;
            this.dinLnSearchCurr2.Value = 8D;
            // 
            // lblLnSearchCurr2Unit
            // 
            resources.ApplyResources(this.lblLnSearchCurr2Unit, "lblLnSearchCurr2Unit");
            this.lblLnSearchCurr2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblLnSearchCurr2Unit.Name = "lblLnSearchCurr2Unit";
            // 
            // rdbLnSearchByCurr
            // 
            resources.ApplyResources(this.rdbLnSearchByCurr, "rdbLnSearchByCurr");
            this.rdbLnSearchByCurr.ForeColor = System.Drawing.Color.Black;
            this.rdbLnSearchByCurr.Name = "rdbLnSearchByCurr";
            this.rdbLnSearchByCurr.TabStop = true;
            this.rdbLnSearchByCurr.UseVisualStyleBackColor = true;
            // 
            // rdbLnSearchByPow
            // 
            resources.ApplyResources(this.rdbLnSearchByPow, "rdbLnSearchByPow");
            this.rdbLnSearchByPow.ForeColor = System.Drawing.Color.Black;
            this.rdbLnSearchByPow.Name = "rdbLnSearchByPow";
            this.rdbLnSearchByPow.TabStop = true;
            this.rdbLnSearchByPow.UseVisualStyleBackColor = true;
            // 
            // pnlLnSearchPow2
            // 
            this.pnlLnSearchPow2.Controls.Add(this.lblLnSearchPow2);
            this.pnlLnSearchPow2.Controls.Add(this.dinLnSearchPow2);
            this.pnlLnSearchPow2.Controls.Add(this.lblLnSearchPow2Unit);
            resources.ApplyResources(this.pnlLnSearchPow2, "pnlLnSearchPow2");
            this.pnlLnSearchPow2.Name = "pnlLnSearchPow2";
            // 
            // lblLnSearchPow2
            // 
            resources.ApplyResources(this.lblLnSearchPow2, "lblLnSearchPow2");
            this.lblLnSearchPow2.ForeColor = System.Drawing.Color.Black;
            this.lblLnSearchPow2.Name = "lblLnSearchPow2";
            // 
            // dinLnSearchPow2
            // 
            // 
            // 
            // 
            this.dinLnSearchPow2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLnSearchPow2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinLnSearchPow2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinLnSearchPow2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinLnSearchPow2, "dinLnSearchPow2");
            this.dinLnSearchPow2.ForeColor = System.Drawing.Color.Black;
            this.dinLnSearchPow2.Increment = 0.1D;
            this.dinLnSearchPow2.MaxValue = 10000D;
            this.dinLnSearchPow2.MinValue = 0D;
            this.dinLnSearchPow2.Name = "dinLnSearchPow2";
            this.dinLnSearchPow2.ShowUpDown = true;
            this.dinLnSearchPow2.Value = 1.5D;
            // 
            // lblLnSearchPow2Unit
            // 
            resources.ApplyResources(this.lblLnSearchPow2Unit, "lblLnSearchPow2Unit");
            this.lblLnSearchPow2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblLnSearchPow2Unit.Name = "lblLnSearchPow2Unit";
            // 
            // pnlLnSearchCurr1
            // 
            this.pnlLnSearchCurr1.Controls.Add(this.lblLnSearchCurr1);
            this.pnlLnSearchCurr1.Controls.Add(this.dinLnSearchCurr1);
            this.pnlLnSearchCurr1.Controls.Add(this.lblLnSearchCurr1Unit);
            resources.ApplyResources(this.pnlLnSearchCurr1, "pnlLnSearchCurr1");
            this.pnlLnSearchCurr1.Name = "pnlLnSearchCurr1";
            // 
            // lblLnSearchCurr1
            // 
            resources.ApplyResources(this.lblLnSearchCurr1, "lblLnSearchCurr1");
            this.lblLnSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.lblLnSearchCurr1.Name = "lblLnSearchCurr1";
            // 
            // dinLnSearchCurr1
            // 
            // 
            // 
            // 
            this.dinLnSearchCurr1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLnSearchCurr1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinLnSearchCurr1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinLnSearchCurr1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinLnSearchCurr1, "dinLnSearchCurr1");
            this.dinLnSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.dinLnSearchCurr1.Increment = 1D;
            this.dinLnSearchCurr1.MaxValue = 10000D;
            this.dinLnSearchCurr1.MinValue = 0D;
            this.dinLnSearchCurr1.Name = "dinLnSearchCurr1";
            this.dinLnSearchCurr1.ShowUpDown = true;
            this.dinLnSearchCurr1.Value = 4D;
            // 
            // lblLnSearchCurr1Unit
            // 
            resources.ApplyResources(this.lblLnSearchCurr1Unit, "lblLnSearchCurr1Unit");
            this.lblLnSearchCurr1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblLnSearchCurr1Unit.Name = "lblLnSearchCurr1Unit";
            // 
            // pnlLnSearchPow1
            // 
            this.pnlLnSearchPow1.Controls.Add(this.lblLnSearchPow1);
            this.pnlLnSearchPow1.Controls.Add(this.dinLnSearchPow1);
            this.pnlLnSearchPow1.Controls.Add(this.lblLnSearchPow1Unit);
            resources.ApplyResources(this.pnlLnSearchPow1, "pnlLnSearchPow1");
            this.pnlLnSearchPow1.Name = "pnlLnSearchPow1";
            // 
            // lblLnSearchPow1
            // 
            resources.ApplyResources(this.lblLnSearchPow1, "lblLnSearchPow1");
            this.lblLnSearchPow1.ForeColor = System.Drawing.Color.Black;
            this.lblLnSearchPow1.Name = "lblLnSearchPow1";
            // 
            // dinLnSearchPow1
            // 
            // 
            // 
            // 
            this.dinLnSearchPow1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLnSearchPow1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinLnSearchPow1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinLnSearchPow1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinLnSearchPow1, "dinLnSearchPow1");
            this.dinLnSearchPow1.ForeColor = System.Drawing.Color.Black;
            this.dinLnSearchPow1.Increment = 0.1D;
            this.dinLnSearchPow1.MaxValue = 10000D;
            this.dinLnSearchPow1.MinValue = 0D;
            this.dinLnSearchPow1.Name = "dinLnSearchPow1";
            this.dinLnSearchPow1.ShowUpDown = true;
            this.dinLnSearchPow1.Value = 0.5D;
            // 
            // lblLnSearchPow1Unit
            // 
            resources.ApplyResources(this.lblLnSearchPow1Unit, "lblLnSearchPow1Unit");
            this.lblLnSearchPow1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblLnSearchPow1Unit.Name = "lblLnSearchPow1Unit";
            // 
            // grpSe2Setting
            // 
            this.grpSe2Setting.Controls.Add(this.pnlSe2SearchCurr2);
            this.grpSe2Setting.Controls.Add(this.rdbSe2SearchByCurr);
            this.grpSe2Setting.Controls.Add(this.rdbSe2SearchByPow);
            this.grpSe2Setting.Controls.Add(this.pnlSe2SearchPow2);
            this.grpSe2Setting.Controls.Add(this.pnlSe2SearchCurr1);
            this.grpSe2Setting.Controls.Add(this.pnlSe2SearchPow1);
            resources.ApplyResources(this.grpSe2Setting, "grpSe2Setting");
            this.grpSe2Setting.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpSe2Setting.Name = "grpSe2Setting";
            this.grpSe2Setting.TabStop = false;
            // 
            // pnlSe2SearchCurr2
            // 
            this.pnlSe2SearchCurr2.Controls.Add(this.lblSe2SearchCurr2);
            this.pnlSe2SearchCurr2.Controls.Add(this.dinSe2SearchCurr2);
            this.pnlSe2SearchCurr2.Controls.Add(this.lblSe2SearchCurr2Unit);
            resources.ApplyResources(this.pnlSe2SearchCurr2, "pnlSe2SearchCurr2");
            this.pnlSe2SearchCurr2.Name = "pnlSe2SearchCurr2";
            // 
            // lblSe2SearchCurr2
            // 
            resources.ApplyResources(this.lblSe2SearchCurr2, "lblSe2SearchCurr2");
            this.lblSe2SearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.lblSe2SearchCurr2.Name = "lblSe2SearchCurr2";
            // 
            // dinSe2SearchCurr2
            // 
            // 
            // 
            // 
            this.dinSe2SearchCurr2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSe2SearchCurr2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSe2SearchCurr2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSe2SearchCurr2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSe2SearchCurr2, "dinSe2SearchCurr2");
            this.dinSe2SearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.dinSe2SearchCurr2.Increment = 1D;
            this.dinSe2SearchCurr2.MaxValue = 10000D;
            this.dinSe2SearchCurr2.MinValue = 0D;
            this.dinSe2SearchCurr2.Name = "dinSe2SearchCurr2";
            this.dinSe2SearchCurr2.ShowUpDown = true;
            this.dinSe2SearchCurr2.Value = 8D;
            // 
            // lblSe2SearchCurr2Unit
            // 
            resources.ApplyResources(this.lblSe2SearchCurr2Unit, "lblSe2SearchCurr2Unit");
            this.lblSe2SearchCurr2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblSe2SearchCurr2Unit.Name = "lblSe2SearchCurr2Unit";
            // 
            // rdbSe2SearchByCurr
            // 
            resources.ApplyResources(this.rdbSe2SearchByCurr, "rdbSe2SearchByCurr");
            this.rdbSe2SearchByCurr.ForeColor = System.Drawing.Color.Black;
            this.rdbSe2SearchByCurr.Name = "rdbSe2SearchByCurr";
            this.rdbSe2SearchByCurr.TabStop = true;
            this.rdbSe2SearchByCurr.UseVisualStyleBackColor = true;
            // 
            // rdbSe2SearchByPow
            // 
            resources.ApplyResources(this.rdbSe2SearchByPow, "rdbSe2SearchByPow");
            this.rdbSe2SearchByPow.ForeColor = System.Drawing.Color.Black;
            this.rdbSe2SearchByPow.Name = "rdbSe2SearchByPow";
            this.rdbSe2SearchByPow.TabStop = true;
            this.rdbSe2SearchByPow.UseVisualStyleBackColor = true;
            // 
            // pnlSe2SearchPow2
            // 
            this.pnlSe2SearchPow2.Controls.Add(this.lblSe2SearchPow2);
            this.pnlSe2SearchPow2.Controls.Add(this.dinSe2SearchPow2);
            this.pnlSe2SearchPow2.Controls.Add(this.lblSe2SearchPow2Unit);
            resources.ApplyResources(this.pnlSe2SearchPow2, "pnlSe2SearchPow2");
            this.pnlSe2SearchPow2.Name = "pnlSe2SearchPow2";
            // 
            // lblSe2SearchPow2
            // 
            resources.ApplyResources(this.lblSe2SearchPow2, "lblSe2SearchPow2");
            this.lblSe2SearchPow2.ForeColor = System.Drawing.Color.Black;
            this.lblSe2SearchPow2.Name = "lblSe2SearchPow2";
            // 
            // dinSe2SearchPow2
            // 
            // 
            // 
            // 
            this.dinSe2SearchPow2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSe2SearchPow2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSe2SearchPow2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSe2SearchPow2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSe2SearchPow2, "dinSe2SearchPow2");
            this.dinSe2SearchPow2.ForeColor = System.Drawing.Color.Black;
            this.dinSe2SearchPow2.Increment = 0.1D;
            this.dinSe2SearchPow2.MaxValue = 10000D;
            this.dinSe2SearchPow2.MinValue = 0D;
            this.dinSe2SearchPow2.Name = "dinSe2SearchPow2";
            this.dinSe2SearchPow2.ShowUpDown = true;
            this.dinSe2SearchPow2.Value = 1.5D;
            // 
            // lblSe2SearchPow2Unit
            // 
            resources.ApplyResources(this.lblSe2SearchPow2Unit, "lblSe2SearchPow2Unit");
            this.lblSe2SearchPow2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblSe2SearchPow2Unit.Name = "lblSe2SearchPow2Unit";
            // 
            // pnlSe2SearchCurr1
            // 
            this.pnlSe2SearchCurr1.Controls.Add(this.lblSe2SearchCurr1);
            this.pnlSe2SearchCurr1.Controls.Add(this.dinSe2SearchCurr1);
            this.pnlSe2SearchCurr1.Controls.Add(this.lblSe2SearchCurr1Unit);
            resources.ApplyResources(this.pnlSe2SearchCurr1, "pnlSe2SearchCurr1");
            this.pnlSe2SearchCurr1.Name = "pnlSe2SearchCurr1";
            // 
            // lblSe2SearchCurr1
            // 
            resources.ApplyResources(this.lblSe2SearchCurr1, "lblSe2SearchCurr1");
            this.lblSe2SearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.lblSe2SearchCurr1.Name = "lblSe2SearchCurr1";
            // 
            // dinSe2SearchCurr1
            // 
            // 
            // 
            // 
            this.dinSe2SearchCurr1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSe2SearchCurr1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSe2SearchCurr1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSe2SearchCurr1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSe2SearchCurr1, "dinSe2SearchCurr1");
            this.dinSe2SearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.dinSe2SearchCurr1.Increment = 1D;
            this.dinSe2SearchCurr1.MaxValue = 10000D;
            this.dinSe2SearchCurr1.MinValue = 0D;
            this.dinSe2SearchCurr1.Name = "dinSe2SearchCurr1";
            this.dinSe2SearchCurr1.ShowUpDown = true;
            this.dinSe2SearchCurr1.Value = 4D;
            // 
            // lblSe2SearchCurr1Unit
            // 
            resources.ApplyResources(this.lblSe2SearchCurr1Unit, "lblSe2SearchCurr1Unit");
            this.lblSe2SearchCurr1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblSe2SearchCurr1Unit.Name = "lblSe2SearchCurr1Unit";
            // 
            // pnlSe2SearchPow1
            // 
            this.pnlSe2SearchPow1.Controls.Add(this.lblSe2SearchPow1);
            this.pnlSe2SearchPow1.Controls.Add(this.dinSe2SearchPow1);
            this.pnlSe2SearchPow1.Controls.Add(this.lblSe2SearchPow1Unit);
            resources.ApplyResources(this.pnlSe2SearchPow1, "pnlSe2SearchPow1");
            this.pnlSe2SearchPow1.Name = "pnlSe2SearchPow1";
            // 
            // lblSe2SearchPow1
            // 
            resources.ApplyResources(this.lblSe2SearchPow1, "lblSe2SearchPow1");
            this.lblSe2SearchPow1.ForeColor = System.Drawing.Color.Black;
            this.lblSe2SearchPow1.Name = "lblSe2SearchPow1";
            // 
            // dinSe2SearchPow1
            // 
            // 
            // 
            // 
            this.dinSe2SearchPow1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSe2SearchPow1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSe2SearchPow1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSe2SearchPow1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSe2SearchPow1, "dinSe2SearchPow1");
            this.dinSe2SearchPow1.ForeColor = System.Drawing.Color.Black;
            this.dinSe2SearchPow1.Increment = 0.1D;
            this.dinSe2SearchPow1.MaxValue = 10000D;
            this.dinSe2SearchPow1.MinValue = 0D;
            this.dinSe2SearchPow1.Name = "dinSe2SearchPow1";
            this.dinSe2SearchPow1.ShowUpDown = true;
            this.dinSe2SearchPow1.Value = 0.5D;
            // 
            // lblSe2SearchPow1Unit
            // 
            resources.ApplyResources(this.lblSe2SearchPow1Unit, "lblSe2SearchPow1Unit");
            this.lblSe2SearchPow1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblSe2SearchPow1Unit.Name = "lblSe2SearchPow1Unit";
            // 
            // grpThresholdPoints
            // 
            this.grpThresholdPoints.Controls.Add(this.pnlThresholdSearchValue2);
            this.grpThresholdPoints.Controls.Add(this.pnlThresholdSearchValue);
            this.grpThresholdPoints.Controls.Add(this.pnlThresholdSearchCurr2);
            this.grpThresholdPoints.Controls.Add(this.rdbThresholdSearchByCurr);
            this.grpThresholdPoints.Controls.Add(this.rdbThresholdSearchByPow);
            this.grpThresholdPoints.Controls.Add(this.pnlThresholdSearchPow2);
            this.grpThresholdPoints.Controls.Add(this.pnlThresholdSearchPow1);
            this.grpThresholdPoints.Controls.Add(this.pnlThresholdSearchCurr1);
            resources.ApplyResources(this.grpThresholdPoints, "grpThresholdPoints");
            this.grpThresholdPoints.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpThresholdPoints.Name = "grpThresholdPoints";
            this.grpThresholdPoints.TabStop = false;
            // 
            // pnlThresholdSearchValue2
            // 
            this.pnlThresholdSearchValue2.Controls.Add(this.lblThresholdSearchValue2);
            this.pnlThresholdSearchValue2.Controls.Add(this.dinThresholdSearchValue2);
            this.pnlThresholdSearchValue2.Controls.Add(this.lblThresholdSearchValueUnit2);
            resources.ApplyResources(this.pnlThresholdSearchValue2, "pnlThresholdSearchValue2");
            this.pnlThresholdSearchValue2.Name = "pnlThresholdSearchValue2";
            // 
            // lblThresholdSearchValue2
            // 
            resources.ApplyResources(this.lblThresholdSearchValue2, "lblThresholdSearchValue2");
            this.lblThresholdSearchValue2.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchValue2.Name = "lblThresholdSearchValue2";
            // 
            // dinThresholdSearchValue2
            // 
            // 
            // 
            // 
            this.dinThresholdSearchValue2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinThresholdSearchValue2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinThresholdSearchValue2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinThresholdSearchValue2.DisplayFormat = "0.000";
            resources.ApplyResources(this.dinThresholdSearchValue2, "dinThresholdSearchValue2");
            this.dinThresholdSearchValue2.ForeColor = System.Drawing.Color.Black;
            this.dinThresholdSearchValue2.Increment = 1D;
            this.dinThresholdSearchValue2.MaxValue = 100D;
            this.dinThresholdSearchValue2.MinValue = 0.01D;
            this.dinThresholdSearchValue2.Name = "dinThresholdSearchValue2";
            this.dinThresholdSearchValue2.ShowUpDown = true;
            this.dinThresholdSearchValue2.Value = 0.03D;
            // 
            // lblThresholdSearchValueUnit2
            // 
            resources.ApplyResources(this.lblThresholdSearchValueUnit2, "lblThresholdSearchValueUnit2");
            this.lblThresholdSearchValueUnit2.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchValueUnit2.Name = "lblThresholdSearchValueUnit2";
            // 
            // pnlThresholdSearchValue
            // 
            this.pnlThresholdSearchValue.Controls.Add(this.lblThresholdSearchValue);
            this.pnlThresholdSearchValue.Controls.Add(this.dinThresholdSearchValue);
            this.pnlThresholdSearchValue.Controls.Add(this.lblThresholdSearchValueUnit);
            resources.ApplyResources(this.pnlThresholdSearchValue, "pnlThresholdSearchValue");
            this.pnlThresholdSearchValue.Name = "pnlThresholdSearchValue";
            // 
            // lblThresholdSearchValue
            // 
            resources.ApplyResources(this.lblThresholdSearchValue, "lblThresholdSearchValue");
            this.lblThresholdSearchValue.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchValue.Name = "lblThresholdSearchValue";
            // 
            // dinThresholdSearchValue
            // 
            // 
            // 
            // 
            this.dinThresholdSearchValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinThresholdSearchValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinThresholdSearchValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinThresholdSearchValue.DisplayFormat = "0.000";
            resources.ApplyResources(this.dinThresholdSearchValue, "dinThresholdSearchValue");
            this.dinThresholdSearchValue.ForeColor = System.Drawing.Color.Black;
            this.dinThresholdSearchValue.Increment = 1D;
            this.dinThresholdSearchValue.MaxValue = 100D;
            this.dinThresholdSearchValue.MinValue = 0.01D;
            this.dinThresholdSearchValue.Name = "dinThresholdSearchValue";
            this.dinThresholdSearchValue.ShowUpDown = true;
            this.dinThresholdSearchValue.Value = 0.03D;
            // 
            // lblThresholdSearchValueUnit
            // 
            resources.ApplyResources(this.lblThresholdSearchValueUnit, "lblThresholdSearchValueUnit");
            this.lblThresholdSearchValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchValueUnit.Name = "lblThresholdSearchValueUnit";
            // 
            // pnlThresholdSearchCurr2
            // 
            this.pnlThresholdSearchCurr2.Controls.Add(this.lblThresholdSearchCurr2);
            this.pnlThresholdSearchCurr2.Controls.Add(this.dinThresholdSearchCurr2);
            this.pnlThresholdSearchCurr2.Controls.Add(this.lblThresholdSearchCurr2Unit);
            resources.ApplyResources(this.pnlThresholdSearchCurr2, "pnlThresholdSearchCurr2");
            this.pnlThresholdSearchCurr2.Name = "pnlThresholdSearchCurr2";
            // 
            // lblThresholdSearchCurr2
            // 
            resources.ApplyResources(this.lblThresholdSearchCurr2, "lblThresholdSearchCurr2");
            this.lblThresholdSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchCurr2.Name = "lblThresholdSearchCurr2";
            // 
            // dinThresholdSearchCurr2
            // 
            // 
            // 
            // 
            this.dinThresholdSearchCurr2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinThresholdSearchCurr2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinThresholdSearchCurr2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinThresholdSearchCurr2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinThresholdSearchCurr2, "dinThresholdSearchCurr2");
            this.dinThresholdSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.dinThresholdSearchCurr2.Increment = 1D;
            this.dinThresholdSearchCurr2.MaxValue = 10000D;
            this.dinThresholdSearchCurr2.MinValue = 0D;
            this.dinThresholdSearchCurr2.Name = "dinThresholdSearchCurr2";
            this.dinThresholdSearchCurr2.ShowUpDown = true;
            this.dinThresholdSearchCurr2.Value = 8D;
            // 
            // lblThresholdSearchCurr2Unit
            // 
            resources.ApplyResources(this.lblThresholdSearchCurr2Unit, "lblThresholdSearchCurr2Unit");
            this.lblThresholdSearchCurr2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchCurr2Unit.Name = "lblThresholdSearchCurr2Unit";
            // 
            // rdbThresholdSearchByCurr
            // 
            resources.ApplyResources(this.rdbThresholdSearchByCurr, "rdbThresholdSearchByCurr");
            this.rdbThresholdSearchByCurr.ForeColor = System.Drawing.Color.Black;
            this.rdbThresholdSearchByCurr.Name = "rdbThresholdSearchByCurr";
            this.rdbThresholdSearchByCurr.TabStop = true;
            this.rdbThresholdSearchByCurr.UseVisualStyleBackColor = true;
            // 
            // rdbThresholdSearchByPow
            // 
            resources.ApplyResources(this.rdbThresholdSearchByPow, "rdbThresholdSearchByPow");
            this.rdbThresholdSearchByPow.ForeColor = System.Drawing.Color.Black;
            this.rdbThresholdSearchByPow.Name = "rdbThresholdSearchByPow";
            this.rdbThresholdSearchByPow.TabStop = true;
            this.rdbThresholdSearchByPow.UseVisualStyleBackColor = true;
            // 
            // pnlThresholdSearchPow2
            // 
            this.pnlThresholdSearchPow2.Controls.Add(this.lblThresholdSearchPow2);
            this.pnlThresholdSearchPow2.Controls.Add(this.dinThresholdSearchPow2);
            this.pnlThresholdSearchPow2.Controls.Add(this.lblThresholdSearchPow2Unit);
            resources.ApplyResources(this.pnlThresholdSearchPow2, "pnlThresholdSearchPow2");
            this.pnlThresholdSearchPow2.Name = "pnlThresholdSearchPow2";
            // 
            // lblThresholdSearchPow2
            // 
            resources.ApplyResources(this.lblThresholdSearchPow2, "lblThresholdSearchPow2");
            this.lblThresholdSearchPow2.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchPow2.Name = "lblThresholdSearchPow2";
            // 
            // dinThresholdSearchPow2
            // 
            // 
            // 
            // 
            this.dinThresholdSearchPow2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinThresholdSearchPow2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinThresholdSearchPow2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinThresholdSearchPow2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinThresholdSearchPow2, "dinThresholdSearchPow2");
            this.dinThresholdSearchPow2.ForeColor = System.Drawing.Color.Black;
            this.dinThresholdSearchPow2.Increment = 0.1D;
            this.dinThresholdSearchPow2.MaxValue = 10000D;
            this.dinThresholdSearchPow2.MinValue = 0D;
            this.dinThresholdSearchPow2.Name = "dinThresholdSearchPow2";
            this.dinThresholdSearchPow2.ShowUpDown = true;
            this.dinThresholdSearchPow2.Value = 1.5D;
            // 
            // lblThresholdSearchPow2Unit
            // 
            resources.ApplyResources(this.lblThresholdSearchPow2Unit, "lblThresholdSearchPow2Unit");
            this.lblThresholdSearchPow2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchPow2Unit.Name = "lblThresholdSearchPow2Unit";
            // 
            // pnlThresholdSearchPow1
            // 
            this.pnlThresholdSearchPow1.Controls.Add(this.lblThresholdSearchPow1);
            this.pnlThresholdSearchPow1.Controls.Add(this.dinThresholdSearchPow1);
            this.pnlThresholdSearchPow1.Controls.Add(this.lblThresholdSearchPow1Unit);
            resources.ApplyResources(this.pnlThresholdSearchPow1, "pnlThresholdSearchPow1");
            this.pnlThresholdSearchPow1.Name = "pnlThresholdSearchPow1";
            // 
            // lblThresholdSearchPow1
            // 
            resources.ApplyResources(this.lblThresholdSearchPow1, "lblThresholdSearchPow1");
            this.lblThresholdSearchPow1.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchPow1.Name = "lblThresholdSearchPow1";
            // 
            // dinThresholdSearchPow1
            // 
            // 
            // 
            // 
            this.dinThresholdSearchPow1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinThresholdSearchPow1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinThresholdSearchPow1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinThresholdSearchPow1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinThresholdSearchPow1, "dinThresholdSearchPow1");
            this.dinThresholdSearchPow1.ForeColor = System.Drawing.Color.Black;
            this.dinThresholdSearchPow1.Increment = 0.1D;
            this.dinThresholdSearchPow1.MaxValue = 10000D;
            this.dinThresholdSearchPow1.MinValue = 0D;
            this.dinThresholdSearchPow1.Name = "dinThresholdSearchPow1";
            this.dinThresholdSearchPow1.ShowUpDown = true;
            this.dinThresholdSearchPow1.Value = 0.5D;
            // 
            // lblThresholdSearchPow1Unit
            // 
            resources.ApplyResources(this.lblThresholdSearchPow1Unit, "lblThresholdSearchPow1Unit");
            this.lblThresholdSearchPow1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchPow1Unit.Name = "lblThresholdSearchPow1Unit";
            // 
            // pnlThresholdSearchCurr1
            // 
            this.pnlThresholdSearchCurr1.Controls.Add(this.lblThresholdSearchCurr1);
            this.pnlThresholdSearchCurr1.Controls.Add(this.dinThresholdSearchCurr1);
            this.pnlThresholdSearchCurr1.Controls.Add(this.lblThresholdSearchCurr1Unit);
            resources.ApplyResources(this.pnlThresholdSearchCurr1, "pnlThresholdSearchCurr1");
            this.pnlThresholdSearchCurr1.Name = "pnlThresholdSearchCurr1";
            // 
            // lblThresholdSearchCurr1
            // 
            resources.ApplyResources(this.lblThresholdSearchCurr1, "lblThresholdSearchCurr1");
            this.lblThresholdSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchCurr1.Name = "lblThresholdSearchCurr1";
            // 
            // dinThresholdSearchCurr1
            // 
            // 
            // 
            // 
            this.dinThresholdSearchCurr1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinThresholdSearchCurr1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinThresholdSearchCurr1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinThresholdSearchCurr1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinThresholdSearchCurr1, "dinThresholdSearchCurr1");
            this.dinThresholdSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.dinThresholdSearchCurr1.Increment = 1D;
            this.dinThresholdSearchCurr1.MaxValue = 10000D;
            this.dinThresholdSearchCurr1.MinValue = 0D;
            this.dinThresholdSearchCurr1.Name = "dinThresholdSearchCurr1";
            this.dinThresholdSearchCurr1.ShowUpDown = true;
            this.dinThresholdSearchCurr1.Value = 4D;
            // 
            // lblThresholdSearchCurr1Unit
            // 
            resources.ApplyResources(this.lblThresholdSearchCurr1Unit, "lblThresholdSearchCurr1Unit");
            this.lblThresholdSearchCurr1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdSearchCurr1Unit.Name = "lblThresholdSearchCurr1Unit";
            // 
            // grpSpecificPoint
            // 
            this.grpSpecificPoint.Controls.Add(this.pnlSpecificCurr3);
            this.grpSpecificPoint.Controls.Add(this.pnlSpecificCurr2);
            this.grpSpecificPoint.Controls.Add(this.pnlSpecificCurr1);
            resources.ApplyResources(this.grpSpecificPoint, "grpSpecificPoint");
            this.grpSpecificPoint.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpSpecificPoint.Name = "grpSpecificPoint";
            this.grpSpecificPoint.TabStop = false;
            // 
            // pnlSpecificCurr3
            // 
            this.pnlSpecificCurr3.Controls.Add(this.lblSpecificCurr3);
            this.pnlSpecificCurr3.Controls.Add(this.dinSpecificCurr3);
            this.pnlSpecificCurr3.Controls.Add(this.lblSpecificCurrUnit3);
            resources.ApplyResources(this.pnlSpecificCurr3, "pnlSpecificCurr3");
            this.pnlSpecificCurr3.Name = "pnlSpecificCurr3";
            // 
            // lblSpecificCurr3
            // 
            resources.ApplyResources(this.lblSpecificCurr3, "lblSpecificCurr3");
            this.lblSpecificCurr3.ForeColor = System.Drawing.Color.Black;
            this.lblSpecificCurr3.Name = "lblSpecificCurr3";
            // 
            // dinSpecificCurr3
            // 
            // 
            // 
            // 
            this.dinSpecificCurr3.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSpecificCurr3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSpecificCurr3.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSpecificCurr3.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSpecificCurr3, "dinSpecificCurr3");
            this.dinSpecificCurr3.ForeColor = System.Drawing.Color.Black;
            this.dinSpecificCurr3.Increment = 1D;
            this.dinSpecificCurr3.MaxValue = 10000D;
            this.dinSpecificCurr3.MinValue = 0D;
            this.dinSpecificCurr3.Name = "dinSpecificCurr3";
            this.dinSpecificCurr3.ShowUpDown = true;
            this.dinSpecificCurr3.Value = 10D;
            // 
            // lblSpecificCurrUnit3
            // 
            resources.ApplyResources(this.lblSpecificCurrUnit3, "lblSpecificCurrUnit3");
            this.lblSpecificCurrUnit3.ForeColor = System.Drawing.Color.Black;
            this.lblSpecificCurrUnit3.Name = "lblSpecificCurrUnit3";
            // 
            // pnlSpecificCurr2
            // 
            this.pnlSpecificCurr2.Controls.Add(this.lblSpecificCurr2);
            this.pnlSpecificCurr2.Controls.Add(this.dinSpecificCurr2);
            this.pnlSpecificCurr2.Controls.Add(this.lblSpecificCurrUnit2);
            resources.ApplyResources(this.pnlSpecificCurr2, "pnlSpecificCurr2");
            this.pnlSpecificCurr2.Name = "pnlSpecificCurr2";
            // 
            // lblSpecificCurr2
            // 
            resources.ApplyResources(this.lblSpecificCurr2, "lblSpecificCurr2");
            this.lblSpecificCurr2.ForeColor = System.Drawing.Color.Black;
            this.lblSpecificCurr2.Name = "lblSpecificCurr2";
            // 
            // dinSpecificCurr2
            // 
            // 
            // 
            // 
            this.dinSpecificCurr2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSpecificCurr2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSpecificCurr2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSpecificCurr2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSpecificCurr2, "dinSpecificCurr2");
            this.dinSpecificCurr2.ForeColor = System.Drawing.Color.Black;
            this.dinSpecificCurr2.Increment = 1D;
            this.dinSpecificCurr2.MaxValue = 10000D;
            this.dinSpecificCurr2.MinValue = 0D;
            this.dinSpecificCurr2.Name = "dinSpecificCurr2";
            this.dinSpecificCurr2.ShowUpDown = true;
            this.dinSpecificCurr2.Value = 5D;
            // 
            // lblSpecificCurrUnit2
            // 
            resources.ApplyResources(this.lblSpecificCurrUnit2, "lblSpecificCurrUnit2");
            this.lblSpecificCurrUnit2.ForeColor = System.Drawing.Color.Black;
            this.lblSpecificCurrUnit2.Name = "lblSpecificCurrUnit2";
            // 
            // pnlSpecificCurr1
            // 
            this.pnlSpecificCurr1.Controls.Add(this.lblSpecificCurr1);
            this.pnlSpecificCurr1.Controls.Add(this.dinSpecificCurr1);
            this.pnlSpecificCurr1.Controls.Add(this.lblSpecificCurrUnit1);
            resources.ApplyResources(this.pnlSpecificCurr1, "pnlSpecificCurr1");
            this.pnlSpecificCurr1.Name = "pnlSpecificCurr1";
            // 
            // lblSpecificCurr1
            // 
            resources.ApplyResources(this.lblSpecificCurr1, "lblSpecificCurr1");
            this.lblSpecificCurr1.ForeColor = System.Drawing.Color.Black;
            this.lblSpecificCurr1.Name = "lblSpecificCurr1";
            // 
            // dinSpecificCurr1
            // 
            // 
            // 
            // 
            this.dinSpecificCurr1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSpecificCurr1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSpecificCurr1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSpecificCurr1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSpecificCurr1, "dinSpecificCurr1");
            this.dinSpecificCurr1.ForeColor = System.Drawing.Color.Black;
            this.dinSpecificCurr1.Increment = 1D;
            this.dinSpecificCurr1.MaxValue = 10000D;
            this.dinSpecificCurr1.MinValue = 0D;
            this.dinSpecificCurr1.Name = "dinSpecificCurr1";
            this.dinSpecificCurr1.ShowUpDown = true;
            this.dinSpecificCurr1.Value = 2D;
            // 
            // lblSpecificCurrUnit1
            // 
            resources.ApplyResources(this.lblSpecificCurrUnit1, "lblSpecificCurrUnit1");
            this.lblSpecificCurrUnit1.ForeColor = System.Drawing.Color.Black;
            this.lblSpecificCurrUnit1.Name = "lblSpecificCurrUnit1";
            // 
            // grpRsSetting
            // 
            this.grpRsSetting.Controls.Add(this.pnlRsSearchCurr2);
            this.grpRsSetting.Controls.Add(this.rdbRsSearchByCurr);
            this.grpRsSetting.Controls.Add(this.rdbRsSearchByPow);
            this.grpRsSetting.Controls.Add(this.pnlRsSearchPow2);
            this.grpRsSetting.Controls.Add(this.pnlRsSearchCurr1);
            this.grpRsSetting.Controls.Add(this.pnlRsSearchPow1);
            resources.ApplyResources(this.grpRsSetting, "grpRsSetting");
            this.grpRsSetting.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpRsSetting.Name = "grpRsSetting";
            this.grpRsSetting.TabStop = false;
            // 
            // pnlRsSearchCurr2
            // 
            this.pnlRsSearchCurr2.Controls.Add(this.lblRsSearchCurr2);
            this.pnlRsSearchCurr2.Controls.Add(this.dinRsSearchCurr2);
            this.pnlRsSearchCurr2.Controls.Add(this.lblRsSearchCurr2Unit);
            resources.ApplyResources(this.pnlRsSearchCurr2, "pnlRsSearchCurr2");
            this.pnlRsSearchCurr2.Name = "pnlRsSearchCurr2";
            // 
            // lblRsSearchCurr2
            // 
            resources.ApplyResources(this.lblRsSearchCurr2, "lblRsSearchCurr2");
            this.lblRsSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.lblRsSearchCurr2.Name = "lblRsSearchCurr2";
            // 
            // dinRsSearchCurr2
            // 
            // 
            // 
            // 
            this.dinRsSearchCurr2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRsSearchCurr2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRsSearchCurr2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRsSearchCurr2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinRsSearchCurr2, "dinRsSearchCurr2");
            this.dinRsSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.dinRsSearchCurr2.Increment = 1D;
            this.dinRsSearchCurr2.MaxValue = 10000D;
            this.dinRsSearchCurr2.MinValue = 0D;
            this.dinRsSearchCurr2.Name = "dinRsSearchCurr2";
            this.dinRsSearchCurr2.ShowUpDown = true;
            this.dinRsSearchCurr2.Value = 8D;
            // 
            // lblRsSearchCurr2Unit
            // 
            resources.ApplyResources(this.lblRsSearchCurr2Unit, "lblRsSearchCurr2Unit");
            this.lblRsSearchCurr2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblRsSearchCurr2Unit.Name = "lblRsSearchCurr2Unit";
            // 
            // rdbRsSearchByCurr
            // 
            resources.ApplyResources(this.rdbRsSearchByCurr, "rdbRsSearchByCurr");
            this.rdbRsSearchByCurr.ForeColor = System.Drawing.Color.Black;
            this.rdbRsSearchByCurr.Name = "rdbRsSearchByCurr";
            this.rdbRsSearchByCurr.TabStop = true;
            this.rdbRsSearchByCurr.UseVisualStyleBackColor = true;
            // 
            // rdbRsSearchByPow
            // 
            resources.ApplyResources(this.rdbRsSearchByPow, "rdbRsSearchByPow");
            this.rdbRsSearchByPow.ForeColor = System.Drawing.Color.Black;
            this.rdbRsSearchByPow.Name = "rdbRsSearchByPow";
            this.rdbRsSearchByPow.TabStop = true;
            this.rdbRsSearchByPow.UseVisualStyleBackColor = true;
            // 
            // pnlRsSearchPow2
            // 
            this.pnlRsSearchPow2.Controls.Add(this.lblRsSearchPow2);
            this.pnlRsSearchPow2.Controls.Add(this.dinRsSearchPow2);
            this.pnlRsSearchPow2.Controls.Add(this.lblRsSearchPow2Unit);
            resources.ApplyResources(this.pnlRsSearchPow2, "pnlRsSearchPow2");
            this.pnlRsSearchPow2.Name = "pnlRsSearchPow2";
            // 
            // lblRsSearchPow2
            // 
            resources.ApplyResources(this.lblRsSearchPow2, "lblRsSearchPow2");
            this.lblRsSearchPow2.ForeColor = System.Drawing.Color.Black;
            this.lblRsSearchPow2.Name = "lblRsSearchPow2";
            // 
            // dinRsSearchPow2
            // 
            // 
            // 
            // 
            this.dinRsSearchPow2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRsSearchPow2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRsSearchPow2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRsSearchPow2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinRsSearchPow2, "dinRsSearchPow2");
            this.dinRsSearchPow2.ForeColor = System.Drawing.Color.Black;
            this.dinRsSearchPow2.Increment = 0.1D;
            this.dinRsSearchPow2.MaxValue = 10000D;
            this.dinRsSearchPow2.MinValue = 0D;
            this.dinRsSearchPow2.Name = "dinRsSearchPow2";
            this.dinRsSearchPow2.ShowUpDown = true;
            this.dinRsSearchPow2.Value = 1.5D;
            // 
            // lblRsSearchPow2Unit
            // 
            resources.ApplyResources(this.lblRsSearchPow2Unit, "lblRsSearchPow2Unit");
            this.lblRsSearchPow2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblRsSearchPow2Unit.Name = "lblRsSearchPow2Unit";
            // 
            // pnlRsSearchCurr1
            // 
            this.pnlRsSearchCurr1.Controls.Add(this.lblRsSearchCurr1);
            this.pnlRsSearchCurr1.Controls.Add(this.dinRsSearchCurr1);
            this.pnlRsSearchCurr1.Controls.Add(this.lblRsSearchCurr1Unit);
            resources.ApplyResources(this.pnlRsSearchCurr1, "pnlRsSearchCurr1");
            this.pnlRsSearchCurr1.Name = "pnlRsSearchCurr1";
            // 
            // lblRsSearchCurr1
            // 
            resources.ApplyResources(this.lblRsSearchCurr1, "lblRsSearchCurr1");
            this.lblRsSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.lblRsSearchCurr1.Name = "lblRsSearchCurr1";
            // 
            // dinRsSearchCurr1
            // 
            // 
            // 
            // 
            this.dinRsSearchCurr1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRsSearchCurr1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRsSearchCurr1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRsSearchCurr1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinRsSearchCurr1, "dinRsSearchCurr1");
            this.dinRsSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.dinRsSearchCurr1.Increment = 1D;
            this.dinRsSearchCurr1.MaxValue = 10000D;
            this.dinRsSearchCurr1.MinValue = 0D;
            this.dinRsSearchCurr1.Name = "dinRsSearchCurr1";
            this.dinRsSearchCurr1.ShowUpDown = true;
            this.dinRsSearchCurr1.Value = 4D;
            // 
            // lblRsSearchCurr1Unit
            // 
            resources.ApplyResources(this.lblRsSearchCurr1Unit, "lblRsSearchCurr1Unit");
            this.lblRsSearchCurr1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblRsSearchCurr1Unit.Name = "lblRsSearchCurr1Unit";
            // 
            // pnlRsSearchPow1
            // 
            this.pnlRsSearchPow1.Controls.Add(this.lblRsSearchPow1);
            this.pnlRsSearchPow1.Controls.Add(this.dinRsSearchPow1);
            this.pnlRsSearchPow1.Controls.Add(this.lblRsSearchPow1Unit);
            resources.ApplyResources(this.pnlRsSearchPow1, "pnlRsSearchPow1");
            this.pnlRsSearchPow1.Name = "pnlRsSearchPow1";
            // 
            // lblRsSearchPow1
            // 
            resources.ApplyResources(this.lblRsSearchPow1, "lblRsSearchPow1");
            this.lblRsSearchPow1.ForeColor = System.Drawing.Color.Black;
            this.lblRsSearchPow1.Name = "lblRsSearchPow1";
            // 
            // dinRsSearchPow1
            // 
            // 
            // 
            // 
            this.dinRsSearchPow1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRsSearchPow1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRsSearchPow1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRsSearchPow1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinRsSearchPow1, "dinRsSearchPow1");
            this.dinRsSearchPow1.ForeColor = System.Drawing.Color.Black;
            this.dinRsSearchPow1.Increment = 0.1D;
            this.dinRsSearchPow1.MaxValue = 10000D;
            this.dinRsSearchPow1.MinValue = 0D;
            this.dinRsSearchPow1.Name = "dinRsSearchPow1";
            this.dinRsSearchPow1.ShowUpDown = true;
            this.dinRsSearchPow1.Value = 0.5D;
            // 
            // lblRsSearchPow1Unit
            // 
            resources.ApplyResources(this.lblRsSearchPow1Unit, "lblRsSearchPow1Unit");
            this.lblRsSearchPow1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblRsSearchPow1Unit.Name = "lblRsSearchPow1Unit";
            // 
            // grpOperationPoints
            // 
            this.grpOperationPoints.Controls.Add(this.PnlPop);
            resources.ApplyResources(this.grpOperationPoints, "grpOperationPoints");
            this.grpOperationPoints.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpOperationPoints.Name = "grpOperationPoints";
            this.grpOperationPoints.TabStop = false;
            // 
            // PnlPop
            // 
            this.PnlPop.Controls.Add(this.lblPop);
            this.PnlPop.Controls.Add(this.dinPop);
            this.PnlPop.Controls.Add(this.lblPopUnit);
            resources.ApplyResources(this.PnlPop, "PnlPop");
            this.PnlPop.Name = "PnlPop";
            // 
            // lblPop
            // 
            resources.ApplyResources(this.lblPop, "lblPop");
            this.lblPop.ForeColor = System.Drawing.Color.Black;
            this.lblPop.Name = "lblPop";
            // 
            // dinPop
            // 
            // 
            // 
            // 
            this.dinPop.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinPop.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinPop.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinPop.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinPop, "dinPop");
            this.dinPop.ForeColor = System.Drawing.Color.Black;
            this.dinPop.Increment = 0.1D;
            this.dinPop.MaxValue = 10000D;
            this.dinPop.MinValue = 0D;
            this.dinPop.Name = "dinPop";
            this.dinPop.ShowUpDown = true;
            this.dinPop.Value = 0.7D;
            // 
            // lblPopUnit
            // 
            resources.ApplyResources(this.lblPopUnit, "lblPopUnit");
            this.lblPopUnit.ForeColor = System.Drawing.Color.Black;
            this.lblPopUnit.Name = "lblPopUnit";
            // 
            // grpKinkSetting
            // 
            this.grpKinkSetting.Controls.Add(this.pnlKinkRatio);
            this.grpKinkSetting.Controls.Add(this.pnlKinkSearchCurr2);
            this.grpKinkSetting.Controls.Add(this.pnlKinkSearchCurr1);
            resources.ApplyResources(this.grpKinkSetting, "grpKinkSetting");
            this.grpKinkSetting.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpKinkSetting.Name = "grpKinkSetting";
            this.grpKinkSetting.TabStop = false;
            // 
            // pnlKinkRatio
            // 
            this.pnlKinkRatio.Controls.Add(this.lblKinkRatio);
            this.pnlKinkRatio.Controls.Add(this.dinKinkRatio);
            this.pnlKinkRatio.Controls.Add(this.lblKinkRatioUnit);
            resources.ApplyResources(this.pnlKinkRatio, "pnlKinkRatio");
            this.pnlKinkRatio.Name = "pnlKinkRatio";
            // 
            // lblKinkRatio
            // 
            resources.ApplyResources(this.lblKinkRatio, "lblKinkRatio");
            this.lblKinkRatio.ForeColor = System.Drawing.Color.Black;
            this.lblKinkRatio.Name = "lblKinkRatio";
            // 
            // dinKinkRatio
            // 
            // 
            // 
            // 
            this.dinKinkRatio.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinKinkRatio.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinKinkRatio.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinKinkRatio.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinKinkRatio, "dinKinkRatio");
            this.dinKinkRatio.ForeColor = System.Drawing.Color.Black;
            this.dinKinkRatio.Increment = 1D;
            this.dinKinkRatio.MaxValue = 100D;
            this.dinKinkRatio.MinValue = 0.1D;
            this.dinKinkRatio.Name = "dinKinkRatio";
            this.dinKinkRatio.ShowUpDown = true;
            this.dinKinkRatio.Value = 1D;
            // 
            // lblKinkRatioUnit
            // 
            resources.ApplyResources(this.lblKinkRatioUnit, "lblKinkRatioUnit");
            this.lblKinkRatioUnit.ForeColor = System.Drawing.Color.Black;
            this.lblKinkRatioUnit.Name = "lblKinkRatioUnit";
            // 
            // pnlKinkSearchCurr2
            // 
            this.pnlKinkSearchCurr2.Controls.Add(this.lblKinkSearchCurr2);
            this.pnlKinkSearchCurr2.Controls.Add(this.dinKinkSearchCurr2);
            this.pnlKinkSearchCurr2.Controls.Add(this.lblKinkSearchCurr2Unit);
            resources.ApplyResources(this.pnlKinkSearchCurr2, "pnlKinkSearchCurr2");
            this.pnlKinkSearchCurr2.Name = "pnlKinkSearchCurr2";
            // 
            // lblKinkSearchCurr2
            // 
            resources.ApplyResources(this.lblKinkSearchCurr2, "lblKinkSearchCurr2");
            this.lblKinkSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.lblKinkSearchCurr2.Name = "lblKinkSearchCurr2";
            // 
            // dinKinkSearchCurr2
            // 
            // 
            // 
            // 
            this.dinKinkSearchCurr2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinKinkSearchCurr2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinKinkSearchCurr2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinKinkSearchCurr2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinKinkSearchCurr2, "dinKinkSearchCurr2");
            this.dinKinkSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.dinKinkSearchCurr2.Increment = 1D;
            this.dinKinkSearchCurr2.MaxValue = 10000D;
            this.dinKinkSearchCurr2.MinValue = 0D;
            this.dinKinkSearchCurr2.Name = "dinKinkSearchCurr2";
            this.dinKinkSearchCurr2.ShowUpDown = true;
            this.dinKinkSearchCurr2.Value = 8D;
            // 
            // lblKinkSearchCurr2Unit
            // 
            resources.ApplyResources(this.lblKinkSearchCurr2Unit, "lblKinkSearchCurr2Unit");
            this.lblKinkSearchCurr2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblKinkSearchCurr2Unit.Name = "lblKinkSearchCurr2Unit";
            // 
            // pnlKinkSearchCurr1
            // 
            this.pnlKinkSearchCurr1.Controls.Add(this.lblKinkSearchCurr1);
            this.pnlKinkSearchCurr1.Controls.Add(this.dinKinkSearchCurr1);
            this.pnlKinkSearchCurr1.Controls.Add(this.lblKinkSearchCurr1Unit);
            resources.ApplyResources(this.pnlKinkSearchCurr1, "pnlKinkSearchCurr1");
            this.pnlKinkSearchCurr1.Name = "pnlKinkSearchCurr1";
            // 
            // lblKinkSearchCurr1
            // 
            resources.ApplyResources(this.lblKinkSearchCurr1, "lblKinkSearchCurr1");
            this.lblKinkSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.lblKinkSearchCurr1.Name = "lblKinkSearchCurr1";
            // 
            // dinKinkSearchCurr1
            // 
            // 
            // 
            // 
            this.dinKinkSearchCurr1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinKinkSearchCurr1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinKinkSearchCurr1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinKinkSearchCurr1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinKinkSearchCurr1, "dinKinkSearchCurr1");
            this.dinKinkSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.dinKinkSearchCurr1.Increment = 1D;
            this.dinKinkSearchCurr1.MaxValue = 10000D;
            this.dinKinkSearchCurr1.MinValue = 0D;
            this.dinKinkSearchCurr1.Name = "dinKinkSearchCurr1";
            this.dinKinkSearchCurr1.ShowUpDown = true;
            this.dinKinkSearchCurr1.Value = 1D;
            // 
            // lblKinkSearchCurr1Unit
            // 
            resources.ApplyResources(this.lblKinkSearchCurr1Unit, "lblKinkSearchCurr1Unit");
            this.lblKinkSearchCurr1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblKinkSearchCurr1Unit.Name = "lblKinkSearchCurr1Unit";
            // 
            // grpAdvancedSettings
            // 
            this.grpAdvancedSettings.Controls.Add(this.panel1);
            this.grpAdvancedSettings.Controls.Add(this.pnlKinkCalcSelection);
            this.grpAdvancedSettings.Controls.Add(this.pnlIVopSelectMode);
            this.grpAdvancedSettings.Controls.Add(this.pnlSeCalcSelection);
            this.grpAdvancedSettings.Controls.Add(this.pnlRsCalcSelection);
            resources.ApplyResources(this.grpAdvancedSettings, "grpAdvancedSettings");
            this.grpAdvancedSettings.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpAdvancedSettings.Name = "grpAdvancedSettings";
            this.grpAdvancedSettings.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbThresholdCalcPo);
            this.panel1.Controls.Add(this.rdbThresholdCalcThr);
            this.panel1.Controls.Add(this.rdbThresholdCalc2Point);
            this.panel1.Controls.Add(this.lblThresholdCalcSelection);
            this.panel1.Controls.Add(this.rdbThresholdCalcLR);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // rdbThresholdCalcPo
            // 
            resources.ApplyResources(this.rdbThresholdCalcPo, "rdbThresholdCalcPo");
            this.rdbThresholdCalcPo.ForeColor = System.Drawing.Color.Black;
            this.rdbThresholdCalcPo.Name = "rdbThresholdCalcPo";
            this.rdbThresholdCalcPo.TabStop = true;
            this.rdbThresholdCalcPo.UseVisualStyleBackColor = true;
            // 
            // rdbThresholdCalcThr
            // 
            resources.ApplyResources(this.rdbThresholdCalcThr, "rdbThresholdCalcThr");
            this.rdbThresholdCalcThr.ForeColor = System.Drawing.Color.Black;
            this.rdbThresholdCalcThr.Name = "rdbThresholdCalcThr";
            this.rdbThresholdCalcThr.TabStop = true;
            this.rdbThresholdCalcThr.UseVisualStyleBackColor = true;
            // 
            // rdbThresholdCalc2Point
            // 
            resources.ApplyResources(this.rdbThresholdCalc2Point, "rdbThresholdCalc2Point");
            this.rdbThresholdCalc2Point.ForeColor = System.Drawing.Color.Black;
            this.rdbThresholdCalc2Point.Name = "rdbThresholdCalc2Point";
            this.rdbThresholdCalc2Point.TabStop = true;
            this.rdbThresholdCalc2Point.UseVisualStyleBackColor = true;
            // 
            // lblThresholdCalcSelection
            // 
            resources.ApplyResources(this.lblThresholdCalcSelection, "lblThresholdCalcSelection");
            this.lblThresholdCalcSelection.ForeColor = System.Drawing.Color.Black;
            this.lblThresholdCalcSelection.Name = "lblThresholdCalcSelection";
            // 
            // rdbThresholdCalcLR
            // 
            resources.ApplyResources(this.rdbThresholdCalcLR, "rdbThresholdCalcLR");
            this.rdbThresholdCalcLR.ForeColor = System.Drawing.Color.Black;
            this.rdbThresholdCalcLR.Name = "rdbThresholdCalcLR";
            this.rdbThresholdCalcLR.TabStop = true;
            this.rdbThresholdCalcLR.UseVisualStyleBackColor = true;
            // 
            // pnlKinkCalcSelection
            // 
            this.pnlKinkCalcSelection.Controls.Add(this.rdbKinkCalcSecondOrder);
            this.pnlKinkCalcSelection.Controls.Add(this.rdbKinkCalcFittingLine);
            this.pnlKinkCalcSelection.Controls.Add(this.rdbKinkCalcRefCurve);
            this.pnlKinkCalcSelection.Controls.Add(this.rdbKinkCalcDeltaPow);
            this.pnlKinkCalcSelection.Controls.Add(this.rdbKinkCalcSEk);
            this.pnlKinkCalcSelection.Controls.Add(this.lblKinkCalcSelection);
            resources.ApplyResources(this.pnlKinkCalcSelection, "pnlKinkCalcSelection");
            this.pnlKinkCalcSelection.Name = "pnlKinkCalcSelection";
            // 
            // rdbKinkCalcSecondOrder
            // 
            resources.ApplyResources(this.rdbKinkCalcSecondOrder, "rdbKinkCalcSecondOrder");
            this.rdbKinkCalcSecondOrder.ForeColor = System.Drawing.Color.Black;
            this.rdbKinkCalcSecondOrder.Name = "rdbKinkCalcSecondOrder";
            this.rdbKinkCalcSecondOrder.TabStop = true;
            this.rdbKinkCalcSecondOrder.UseVisualStyleBackColor = true;
            // 
            // rdbKinkCalcFittingLine
            // 
            resources.ApplyResources(this.rdbKinkCalcFittingLine, "rdbKinkCalcFittingLine");
            this.rdbKinkCalcFittingLine.ForeColor = System.Drawing.Color.Black;
            this.rdbKinkCalcFittingLine.Name = "rdbKinkCalcFittingLine";
            this.rdbKinkCalcFittingLine.TabStop = true;
            this.rdbKinkCalcFittingLine.UseVisualStyleBackColor = true;
            // 
            // rdbKinkCalcRefCurve
            // 
            resources.ApplyResources(this.rdbKinkCalcRefCurve, "rdbKinkCalcRefCurve");
            this.rdbKinkCalcRefCurve.ForeColor = System.Drawing.Color.Black;
            this.rdbKinkCalcRefCurve.Name = "rdbKinkCalcRefCurve";
            this.rdbKinkCalcRefCurve.TabStop = true;
            this.rdbKinkCalcRefCurve.UseVisualStyleBackColor = true;
            // 
            // rdbKinkCalcDeltaPow
            // 
            resources.ApplyResources(this.rdbKinkCalcDeltaPow, "rdbKinkCalcDeltaPow");
            this.rdbKinkCalcDeltaPow.ForeColor = System.Drawing.Color.Black;
            this.rdbKinkCalcDeltaPow.Name = "rdbKinkCalcDeltaPow";
            this.rdbKinkCalcDeltaPow.TabStop = true;
            this.rdbKinkCalcDeltaPow.UseVisualStyleBackColor = true;
            // 
            // rdbKinkCalcSEk
            // 
            resources.ApplyResources(this.rdbKinkCalcSEk, "rdbKinkCalcSEk");
            this.rdbKinkCalcSEk.ForeColor = System.Drawing.Color.Black;
            this.rdbKinkCalcSEk.Name = "rdbKinkCalcSEk";
            this.rdbKinkCalcSEk.TabStop = true;
            this.rdbKinkCalcSEk.UseVisualStyleBackColor = true;
            // 
            // lblKinkCalcSelection
            // 
            resources.ApplyResources(this.lblKinkCalcSelection, "lblKinkCalcSelection");
            this.lblKinkCalcSelection.ForeColor = System.Drawing.Color.Black;
            this.lblKinkCalcSelection.Name = "lblKinkCalcSelection";
            // 
            // pnlIVopSelectMode
            // 
            this.pnlIVopSelectMode.Controls.Add(this.rdbOpOnFittingLine);
            this.pnlIVopSelectMode.Controls.Add(this.rdbOpInterpolation);
            this.pnlIVopSelectMode.Controls.Add(this.lblIVopSelection);
            this.pnlIVopSelectMode.Controls.Add(this.rdbOpClosestpoint);
            this.pnlIVopSelectMode.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.pnlIVopSelectMode, "pnlIVopSelectMode");
            this.pnlIVopSelectMode.Name = "pnlIVopSelectMode";
            // 
            // rdbOpInterpolation
            // 
            resources.ApplyResources(this.rdbOpInterpolation, "rdbOpInterpolation");
            this.rdbOpInterpolation.ForeColor = System.Drawing.Color.Black;
            this.rdbOpInterpolation.Name = "rdbOpInterpolation";
            this.rdbOpInterpolation.TabStop = true;
            this.rdbOpInterpolation.UseVisualStyleBackColor = true;
            // 
            // lblIVopSelection
            // 
            resources.ApplyResources(this.lblIVopSelection, "lblIVopSelection");
            this.lblIVopSelection.ForeColor = System.Drawing.Color.Black;
            this.lblIVopSelection.Name = "lblIVopSelection";
            // 
            // rdbOpClosestpoint
            // 
            resources.ApplyResources(this.rdbOpClosestpoint, "rdbOpClosestpoint");
            this.rdbOpClosestpoint.ForeColor = System.Drawing.Color.Black;
            this.rdbOpClosestpoint.Name = "rdbOpClosestpoint";
            this.rdbOpClosestpoint.TabStop = true;
            this.rdbOpClosestpoint.UseVisualStyleBackColor = true;
            // 
            // pnlSeCalcSelection
            // 
            this.pnlSeCalcSelection.Controls.Add(this.rdbSeCalcAvg);
            this.pnlSeCalcSelection.Controls.Add(this.rdbSeCalc2Point);
            this.pnlSeCalcSelection.Controls.Add(this.lblSeCalcSelection);
            this.pnlSeCalcSelection.Controls.Add(this.rdbSeCalcLR);
            resources.ApplyResources(this.pnlSeCalcSelection, "pnlSeCalcSelection");
            this.pnlSeCalcSelection.Name = "pnlSeCalcSelection";
            // 
            // rdbSeCalcAvg
            // 
            resources.ApplyResources(this.rdbSeCalcAvg, "rdbSeCalcAvg");
            this.rdbSeCalcAvg.ForeColor = System.Drawing.Color.Black;
            this.rdbSeCalcAvg.Name = "rdbSeCalcAvg";
            this.rdbSeCalcAvg.TabStop = true;
            this.rdbSeCalcAvg.UseVisualStyleBackColor = true;
            // 
            // rdbSeCalc2Point
            // 
            resources.ApplyResources(this.rdbSeCalc2Point, "rdbSeCalc2Point");
            this.rdbSeCalc2Point.ForeColor = System.Drawing.Color.Black;
            this.rdbSeCalc2Point.Name = "rdbSeCalc2Point";
            this.rdbSeCalc2Point.TabStop = true;
            this.rdbSeCalc2Point.UseVisualStyleBackColor = true;
            // 
            // lblSeCalcSelection
            // 
            resources.ApplyResources(this.lblSeCalcSelection, "lblSeCalcSelection");
            this.lblSeCalcSelection.ForeColor = System.Drawing.Color.Black;
            this.lblSeCalcSelection.Name = "lblSeCalcSelection";
            // 
            // rdbSeCalcLR
            // 
            resources.ApplyResources(this.rdbSeCalcLR, "rdbSeCalcLR");
            this.rdbSeCalcLR.ForeColor = System.Drawing.Color.Black;
            this.rdbSeCalcLR.Name = "rdbSeCalcLR";
            this.rdbSeCalcLR.TabStop = true;
            this.rdbSeCalcLR.UseVisualStyleBackColor = true;
            // 
            // pnlRsCalcSelection
            // 
            this.pnlRsCalcSelection.Controls.Add(this.rdbRsCalcAvg);
            this.pnlRsCalcSelection.Controls.Add(this.rdbRsCalc2Point);
            this.pnlRsCalcSelection.Controls.Add(this.lblRsCalcSelection);
            this.pnlRsCalcSelection.Controls.Add(this.rdbRsCalcLR);
            resources.ApplyResources(this.pnlRsCalcSelection, "pnlRsCalcSelection");
            this.pnlRsCalcSelection.Name = "pnlRsCalcSelection";
            // 
            // rdbRsCalcAvg
            // 
            resources.ApplyResources(this.rdbRsCalcAvg, "rdbRsCalcAvg");
            this.rdbRsCalcAvg.ForeColor = System.Drawing.Color.Black;
            this.rdbRsCalcAvg.Name = "rdbRsCalcAvg";
            this.rdbRsCalcAvg.TabStop = true;
            this.rdbRsCalcAvg.UseVisualStyleBackColor = true;
            // 
            // rdbRsCalc2Point
            // 
            resources.ApplyResources(this.rdbRsCalc2Point, "rdbRsCalc2Point");
            this.rdbRsCalc2Point.ForeColor = System.Drawing.Color.Black;
            this.rdbRsCalc2Point.Name = "rdbRsCalc2Point";
            this.rdbRsCalc2Point.TabStop = true;
            this.rdbRsCalc2Point.UseVisualStyleBackColor = true;
            // 
            // lblRsCalcSelection
            // 
            resources.ApplyResources(this.lblRsCalcSelection, "lblRsCalcSelection");
            this.lblRsCalcSelection.ForeColor = System.Drawing.Color.Black;
            this.lblRsCalcSelection.Name = "lblRsCalcSelection";
            // 
            // rdbRsCalcLR
            // 
            resources.ApplyResources(this.rdbRsCalcLR, "rdbRsCalcLR");
            this.rdbRsCalcLR.ForeColor = System.Drawing.Color.Black;
            this.rdbRsCalcLR.Name = "rdbRsCalcLR";
            this.rdbRsCalcLR.TabStop = true;
            this.rdbRsCalcLR.UseVisualStyleBackColor = true;
            // 
            // grpSeSetting
            // 
            this.grpSeSetting.Controls.Add(this.pnlSeSearchCurr2);
            this.grpSeSetting.Controls.Add(this.rdbSeSearchByCurr);
            this.grpSeSetting.Controls.Add(this.rdbSeSearchByPow);
            this.grpSeSetting.Controls.Add(this.pnlSeSearchPow2);
            this.grpSeSetting.Controls.Add(this.pnlSeSearchCurr1);
            this.grpSeSetting.Controls.Add(this.pnlSeSearchPow1);
            resources.ApplyResources(this.grpSeSetting, "grpSeSetting");
            this.grpSeSetting.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpSeSetting.Name = "grpSeSetting";
            this.grpSeSetting.TabStop = false;
            // 
            // pnlSeSearchCurr2
            // 
            this.pnlSeSearchCurr2.Controls.Add(this.lblSeSearchCurr2);
            this.pnlSeSearchCurr2.Controls.Add(this.dinSeSearchCurr2);
            this.pnlSeSearchCurr2.Controls.Add(this.lblSeSearchCurr2Unit);
            resources.ApplyResources(this.pnlSeSearchCurr2, "pnlSeSearchCurr2");
            this.pnlSeSearchCurr2.Name = "pnlSeSearchCurr2";
            // 
            // lblSeSearchCurr2
            // 
            resources.ApplyResources(this.lblSeSearchCurr2, "lblSeSearchCurr2");
            this.lblSeSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.lblSeSearchCurr2.Name = "lblSeSearchCurr2";
            // 
            // dinSeSearchCurr2
            // 
            // 
            // 
            // 
            this.dinSeSearchCurr2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSeSearchCurr2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSeSearchCurr2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSeSearchCurr2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSeSearchCurr2, "dinSeSearchCurr2");
            this.dinSeSearchCurr2.ForeColor = System.Drawing.Color.Black;
            this.dinSeSearchCurr2.Increment = 1D;
            this.dinSeSearchCurr2.MaxValue = 10000D;
            this.dinSeSearchCurr2.MinValue = 0D;
            this.dinSeSearchCurr2.Name = "dinSeSearchCurr2";
            this.dinSeSearchCurr2.ShowUpDown = true;
            this.dinSeSearchCurr2.Value = 8D;
            // 
            // lblSeSearchCurr2Unit
            // 
            resources.ApplyResources(this.lblSeSearchCurr2Unit, "lblSeSearchCurr2Unit");
            this.lblSeSearchCurr2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblSeSearchCurr2Unit.Name = "lblSeSearchCurr2Unit";
            // 
            // rdbSeSearchByCurr
            // 
            resources.ApplyResources(this.rdbSeSearchByCurr, "rdbSeSearchByCurr");
            this.rdbSeSearchByCurr.ForeColor = System.Drawing.Color.Black;
            this.rdbSeSearchByCurr.Name = "rdbSeSearchByCurr";
            this.rdbSeSearchByCurr.TabStop = true;
            this.rdbSeSearchByCurr.UseVisualStyleBackColor = true;
            // 
            // rdbSeSearchByPow
            // 
            resources.ApplyResources(this.rdbSeSearchByPow, "rdbSeSearchByPow");
            this.rdbSeSearchByPow.ForeColor = System.Drawing.Color.Black;
            this.rdbSeSearchByPow.Name = "rdbSeSearchByPow";
            this.rdbSeSearchByPow.TabStop = true;
            this.rdbSeSearchByPow.UseVisualStyleBackColor = true;
            // 
            // pnlSeSearchPow2
            // 
            this.pnlSeSearchPow2.Controls.Add(this.lblSeSearchPow2);
            this.pnlSeSearchPow2.Controls.Add(this.dinSeSearchPow2);
            this.pnlSeSearchPow2.Controls.Add(this.lblSeSearchPow2Unit);
            resources.ApplyResources(this.pnlSeSearchPow2, "pnlSeSearchPow2");
            this.pnlSeSearchPow2.Name = "pnlSeSearchPow2";
            // 
            // lblSeSearchPow2
            // 
            resources.ApplyResources(this.lblSeSearchPow2, "lblSeSearchPow2");
            this.lblSeSearchPow2.ForeColor = System.Drawing.Color.Black;
            this.lblSeSearchPow2.Name = "lblSeSearchPow2";
            // 
            // dinSeSearchPow2
            // 
            // 
            // 
            // 
            this.dinSeSearchPow2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSeSearchPow2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSeSearchPow2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSeSearchPow2.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSeSearchPow2, "dinSeSearchPow2");
            this.dinSeSearchPow2.ForeColor = System.Drawing.Color.Black;
            this.dinSeSearchPow2.Increment = 0.1D;
            this.dinSeSearchPow2.MaxValue = 10000D;
            this.dinSeSearchPow2.MinValue = 0D;
            this.dinSeSearchPow2.Name = "dinSeSearchPow2";
            this.dinSeSearchPow2.ShowUpDown = true;
            this.dinSeSearchPow2.Value = 1.5D;
            // 
            // lblSeSearchPow2Unit
            // 
            resources.ApplyResources(this.lblSeSearchPow2Unit, "lblSeSearchPow2Unit");
            this.lblSeSearchPow2Unit.ForeColor = System.Drawing.Color.Black;
            this.lblSeSearchPow2Unit.Name = "lblSeSearchPow2Unit";
            // 
            // pnlSeSearchCurr1
            // 
            this.pnlSeSearchCurr1.Controls.Add(this.lblSeSearchCurr1);
            this.pnlSeSearchCurr1.Controls.Add(this.dinSeSearchCurr1);
            this.pnlSeSearchCurr1.Controls.Add(this.lblSeSearchCurr1Unit);
            resources.ApplyResources(this.pnlSeSearchCurr1, "pnlSeSearchCurr1");
            this.pnlSeSearchCurr1.Name = "pnlSeSearchCurr1";
            // 
            // lblSeSearchCurr1
            // 
            resources.ApplyResources(this.lblSeSearchCurr1, "lblSeSearchCurr1");
            this.lblSeSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.lblSeSearchCurr1.Name = "lblSeSearchCurr1";
            // 
            // dinSeSearchCurr1
            // 
            // 
            // 
            // 
            this.dinSeSearchCurr1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSeSearchCurr1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSeSearchCurr1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSeSearchCurr1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSeSearchCurr1, "dinSeSearchCurr1");
            this.dinSeSearchCurr1.ForeColor = System.Drawing.Color.Black;
            this.dinSeSearchCurr1.Increment = 1D;
            this.dinSeSearchCurr1.MaxValue = 10000D;
            this.dinSeSearchCurr1.MinValue = 0D;
            this.dinSeSearchCurr1.Name = "dinSeSearchCurr1";
            this.dinSeSearchCurr1.ShowUpDown = true;
            this.dinSeSearchCurr1.Value = 4D;
            // 
            // lblSeSearchCurr1Unit
            // 
            resources.ApplyResources(this.lblSeSearchCurr1Unit, "lblSeSearchCurr1Unit");
            this.lblSeSearchCurr1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblSeSearchCurr1Unit.Name = "lblSeSearchCurr1Unit";
            // 
            // pnlSeSearchPow1
            // 
            this.pnlSeSearchPow1.Controls.Add(this.lblSeSearchPow1);
            this.pnlSeSearchPow1.Controls.Add(this.dinSeSearchPow1);
            this.pnlSeSearchPow1.Controls.Add(this.lblSeSearchPow1Unit);
            resources.ApplyResources(this.pnlSeSearchPow1, "pnlSeSearchPow1");
            this.pnlSeSearchPow1.Name = "pnlSeSearchPow1";
            // 
            // lblSeSearchPow1
            // 
            resources.ApplyResources(this.lblSeSearchPow1, "lblSeSearchPow1");
            this.lblSeSearchPow1.ForeColor = System.Drawing.Color.Black;
            this.lblSeSearchPow1.Name = "lblSeSearchPow1";
            // 
            // dinSeSearchPow1
            // 
            // 
            // 
            // 
            this.dinSeSearchPow1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSeSearchPow1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSeSearchPow1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSeSearchPow1.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinSeSearchPow1, "dinSeSearchPow1");
            this.dinSeSearchPow1.ForeColor = System.Drawing.Color.Black;
            this.dinSeSearchPow1.Increment = 0.1D;
            this.dinSeSearchPow1.MaxValue = 10000D;
            this.dinSeSearchPow1.MinValue = 0D;
            this.dinSeSearchPow1.Name = "dinSeSearchPow1";
            this.dinSeSearchPow1.ShowUpDown = true;
            this.dinSeSearchPow1.Value = 0.5D;
            // 
            // lblSeSearchPow1Unit
            // 
            resources.ApplyResources(this.lblSeSearchPow1Unit, "lblSeSearchPow1Unit");
            this.lblSeSearchPow1Unit.ForeColor = System.Drawing.Color.Black;
            this.lblSeSearchPow1Unit.Name = "lblSeSearchPow1Unit";
            // 
            // grpMovingAverage
            // 
            this.grpMovingAverage.Controls.Add(this.pnlIPMovingAvg);
            this.grpMovingAverage.Controls.Add(this.pnlIVMovingAvg);
            resources.ApplyResources(this.grpMovingAverage, "grpMovingAverage");
            this.grpMovingAverage.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpMovingAverage.Name = "grpMovingAverage";
            this.grpMovingAverage.TabStop = false;
            // 
            // pnlIPMovingAvg
            // 
            this.pnlIPMovingAvg.Controls.Add(this.lblIPMovingAvg);
            this.pnlIPMovingAvg.Controls.Add(this.dinIPMovingAvg);
            this.pnlIPMovingAvg.Controls.Add(this.lblIPMovingAvgUnit);
            this.pnlIPMovingAvg.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.pnlIPMovingAvg, "pnlIPMovingAvg");
            this.pnlIPMovingAvg.Name = "pnlIPMovingAvg";
            // 
            // lblIPMovingAvg
            // 
            resources.ApplyResources(this.lblIPMovingAvg, "lblIPMovingAvg");
            this.lblIPMovingAvg.Name = "lblIPMovingAvg";
            // 
            // dinIPMovingAvg
            // 
            // 
            // 
            // 
            this.dinIPMovingAvg.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinIPMovingAvg.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinIPMovingAvg.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinIPMovingAvg.DisplayFormat = "0";
            resources.ApplyResources(this.dinIPMovingAvg, "dinIPMovingAvg");
            this.dinIPMovingAvg.Increment = 1D;
            this.dinIPMovingAvg.MaxValue = 20D;
            this.dinIPMovingAvg.MinValue = 0D;
            this.dinIPMovingAvg.Name = "dinIPMovingAvg";
            this.dinIPMovingAvg.ShowUpDown = true;
            this.dinIPMovingAvg.Value = 3D;
            // 
            // lblIPMovingAvgUnit
            // 
            resources.ApplyResources(this.lblIPMovingAvgUnit, "lblIPMovingAvgUnit");
            this.lblIPMovingAvgUnit.Name = "lblIPMovingAvgUnit";
            // 
            // pnlIVMovingAvg
            // 
            this.pnlIVMovingAvg.Controls.Add(this.lblIVMovingAvg);
            this.pnlIVMovingAvg.Controls.Add(this.dinIVMovingAvg);
            this.pnlIVMovingAvg.Controls.Add(this.lblIVMovingAvgUnit);
            this.pnlIVMovingAvg.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.pnlIVMovingAvg, "pnlIVMovingAvg");
            this.pnlIVMovingAvg.Name = "pnlIVMovingAvg";
            // 
            // lblIVMovingAvg
            // 
            resources.ApplyResources(this.lblIVMovingAvg, "lblIVMovingAvg");
            this.lblIVMovingAvg.Name = "lblIVMovingAvg";
            // 
            // dinIVMovingAvg
            // 
            // 
            // 
            // 
            this.dinIVMovingAvg.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinIVMovingAvg.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinIVMovingAvg.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinIVMovingAvg.DisplayFormat = "0";
            resources.ApplyResources(this.dinIVMovingAvg, "dinIVMovingAvg");
            this.dinIVMovingAvg.Increment = 1D;
            this.dinIVMovingAvg.MaxValue = 20D;
            this.dinIVMovingAvg.MinValue = 0D;
            this.dinIVMovingAvg.Name = "dinIVMovingAvg";
            this.dinIVMovingAvg.ShowUpDown = true;
            // 
            // lblIVMovingAvgUnit
            // 
            resources.ApplyResources(this.lblIVMovingAvgUnit, "lblIVMovingAvgUnit");
            this.lblIVMovingAvgUnit.Name = "lblIVMovingAvgUnit";
            // 
            // grpRollOver
            // 
            this.grpRollOver.Controls.Add(this.pnlRollOver);
            resources.ApplyResources(this.grpRollOver, "grpRollOver");
            this.grpRollOver.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpRollOver.Name = "grpRollOver";
            this.grpRollOver.TabStop = false;
            // 
            // pnlRollOver
            // 
            this.pnlRollOver.Controls.Add(this.lblRollOver);
            this.pnlRollOver.Controls.Add(this.dinRollOver);
            this.pnlRollOver.Controls.Add(this.lblRollOverUnit);
            resources.ApplyResources(this.pnlRollOver, "pnlRollOver");
            this.pnlRollOver.Name = "pnlRollOver";
            // 
            // lblRollOver
            // 
            resources.ApplyResources(this.lblRollOver, "lblRollOver");
            this.lblRollOver.ForeColor = System.Drawing.Color.Black;
            this.lblRollOver.Name = "lblRollOver";
            // 
            // dinRollOver
            // 
            // 
            // 
            // 
            this.dinRollOver.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRollOver.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRollOver.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRollOver.DisplayFormat = "0.00";
            resources.ApplyResources(this.dinRollOver, "dinRollOver");
            this.dinRollOver.ForeColor = System.Drawing.Color.Black;
            this.dinRollOver.Increment = 1D;
            this.dinRollOver.MaxValue = 1000D;
            this.dinRollOver.MinValue = 0D;
            this.dinRollOver.Name = "dinRollOver";
            this.dinRollOver.ShowUpDown = true;
            this.dinRollOver.Value = 15D;
            // 
            // lblRollOverUnit
            // 
            resources.ApplyResources(this.lblRollOverUnit, "lblRollOverUnit");
            this.lblRollOverUnit.ForeColor = System.Drawing.Color.Black;
            this.lblRollOverUnit.Name = "lblRollOverUnit";
            // 
            // grpItemCondition
            // 
            this.grpItemCondition.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.grpItemCondition, "grpItemCondition");
            this.grpItemCondition.ForeColor = System.Drawing.Color.DimGray;
            this.grpItemCondition.Name = "grpItemCondition";
            this.grpItemCondition.TabStop = false;
            // 
            // rdbOpOnFittingLine
            // 
            resources.ApplyResources(this.rdbOpOnFittingLine, "rdbOpOnFittingLine");
            this.rdbOpOnFittingLine.ForeColor = System.Drawing.Color.Black;
            this.rdbOpOnFittingLine.Name = "rdbOpOnFittingLine";
            this.rdbOpOnFittingLine.TabStop = true;
            this.rdbOpOnFittingLine.UseVisualStyleBackColor = true;
            // 
            // frmItemSettingPIV
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grpItemCondition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmItemSettingPIV";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpChannel.ResumeLayout(false);
            this.grpChannel.PerformLayout();
            this.grpApplySetting.ResumeLayout(false);
            this.pnlStepPulseCnt.ResumeLayout(false);
            this.pnlStepPulseCnt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStepPulseCnt)).EndInit();
            this.pnlSourceFunc.ResumeLayout(false);
            this.pnlSourceFunc.PerformLayout();
            this.pnlDutyCycle.ResumeLayout(false);
            this.pnlDutyCycle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinDutyCycle)).EndInit();
            this.pnlTurnOffTime.ResumeLayout(false);
            this.pnlTurnOffTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinTurnOffTime)).EndInit();
            this.pnlStepValue.ResumeLayout(false);
            this.pnlStepValue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinStepValue)).EndInit();
            this.pnlPoints.ResumeLayout(false);
            this.pnlPoints.PerformLayout();
            this.pnlEndValue.ResumeLayout(false);
            this.pnlEndValue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinEndValue)).EndInit();
            this.pnlForceTime.ResumeLayout(false);
            this.pnlForceTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceTime)).EndInit();
            this.pnlStartValue.ResumeLayout(false);
            this.pnlStartValue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinStartValue)).EndInit();
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
            this.pnlQcwFilterCount.ResumeLayout(false);
            this.pnlQcwFilterCount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQcwFilterCount)).EndInit();
            this.pnlMsrtClamp.ResumeLayout(false);
            this.pnlMsrtClamp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).EndInit();
            this.pnlMsrtRange.ResumeLayout(false);
            this.pnlMsrtRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtRange)).EndInit();
            this.pnlNPLC.ResumeLayout(false);
            this.pnlNPLC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinNPLC)).EndInit();
            this.pnlFilterCount.ResumeLayout(false);
            this.pnlFilterCount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMsrtFilterCount)).EndInit();
            this.grpPDDetector.ResumeLayout(false);
            this.pnlDetectorBiasVoltage.ResumeLayout(false);
            this.pnlDetectorBiasVoltage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinDetectorBiasVoltage)).EndInit();
            this.pnlDetectorNPLC.ResumeLayout(false);
            this.pnlDetectorNPLC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinDetectorNPLC)).EndInit();
            this.pnlDetectorMsrtRange.ResumeLayout(false);
            this.pnlDetectorMsrtRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinDetectorMsrtRange)).EndInit();
            this.grpCalcSetting.ResumeLayout(false);
            this.grpLinearitySetting.ResumeLayout(false);
            this.grpLinearitySetting.PerformLayout();
            this.pnlLnSearchCurr2.ResumeLayout(false);
            this.pnlLnSearchCurr2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLnSearchCurr2)).EndInit();
            this.pnlLnSearchPow2.ResumeLayout(false);
            this.pnlLnSearchPow2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLnSearchPow2)).EndInit();
            this.pnlLnSearchCurr1.ResumeLayout(false);
            this.pnlLnSearchCurr1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLnSearchCurr1)).EndInit();
            this.pnlLnSearchPow1.ResumeLayout(false);
            this.pnlLnSearchPow1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLnSearchPow1)).EndInit();
            this.grpSe2Setting.ResumeLayout(false);
            this.grpSe2Setting.PerformLayout();
            this.pnlSe2SearchCurr2.ResumeLayout(false);
            this.pnlSe2SearchCurr2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSe2SearchCurr2)).EndInit();
            this.pnlSe2SearchPow2.ResumeLayout(false);
            this.pnlSe2SearchPow2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSe2SearchPow2)).EndInit();
            this.pnlSe2SearchCurr1.ResumeLayout(false);
            this.pnlSe2SearchCurr1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSe2SearchCurr1)).EndInit();
            this.pnlSe2SearchPow1.ResumeLayout(false);
            this.pnlSe2SearchPow1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSe2SearchPow1)).EndInit();
            this.grpThresholdPoints.ResumeLayout(false);
            this.grpThresholdPoints.PerformLayout();
            this.pnlThresholdSearchValue2.ResumeLayout(false);
            this.pnlThresholdSearchValue2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchValue2)).EndInit();
            this.pnlThresholdSearchValue.ResumeLayout(false);
            this.pnlThresholdSearchValue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchValue)).EndInit();
            this.pnlThresholdSearchCurr2.ResumeLayout(false);
            this.pnlThresholdSearchCurr2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchCurr2)).EndInit();
            this.pnlThresholdSearchPow2.ResumeLayout(false);
            this.pnlThresholdSearchPow2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchPow2)).EndInit();
            this.pnlThresholdSearchPow1.ResumeLayout(false);
            this.pnlThresholdSearchPow1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchPow1)).EndInit();
            this.pnlThresholdSearchCurr1.ResumeLayout(false);
            this.pnlThresholdSearchCurr1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinThresholdSearchCurr1)).EndInit();
            this.grpSpecificPoint.ResumeLayout(false);
            this.pnlSpecificCurr3.ResumeLayout(false);
            this.pnlSpecificCurr3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSpecificCurr3)).EndInit();
            this.pnlSpecificCurr2.ResumeLayout(false);
            this.pnlSpecificCurr2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSpecificCurr2)).EndInit();
            this.pnlSpecificCurr1.ResumeLayout(false);
            this.pnlSpecificCurr1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSpecificCurr1)).EndInit();
            this.grpRsSetting.ResumeLayout(false);
            this.grpRsSetting.PerformLayout();
            this.pnlRsSearchCurr2.ResumeLayout(false);
            this.pnlRsSearchCurr2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRsSearchCurr2)).EndInit();
            this.pnlRsSearchPow2.ResumeLayout(false);
            this.pnlRsSearchPow2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRsSearchPow2)).EndInit();
            this.pnlRsSearchCurr1.ResumeLayout(false);
            this.pnlRsSearchCurr1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRsSearchCurr1)).EndInit();
            this.pnlRsSearchPow1.ResumeLayout(false);
            this.pnlRsSearchPow1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRsSearchPow1)).EndInit();
            this.grpOperationPoints.ResumeLayout(false);
            this.PnlPop.ResumeLayout(false);
            this.PnlPop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinPop)).EndInit();
            this.grpKinkSetting.ResumeLayout(false);
            this.pnlKinkRatio.ResumeLayout(false);
            this.pnlKinkRatio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinKinkRatio)).EndInit();
            this.pnlKinkSearchCurr2.ResumeLayout(false);
            this.pnlKinkSearchCurr2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinKinkSearchCurr2)).EndInit();
            this.pnlKinkSearchCurr1.ResumeLayout(false);
            this.pnlKinkSearchCurr1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinKinkSearchCurr1)).EndInit();
            this.grpAdvancedSettings.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlKinkCalcSelection.ResumeLayout(false);
            this.pnlKinkCalcSelection.PerformLayout();
            this.pnlIVopSelectMode.ResumeLayout(false);
            this.pnlIVopSelectMode.PerformLayout();
            this.pnlSeCalcSelection.ResumeLayout(false);
            this.pnlSeCalcSelection.PerformLayout();
            this.pnlRsCalcSelection.ResumeLayout(false);
            this.pnlRsCalcSelection.PerformLayout();
            this.grpSeSetting.ResumeLayout(false);
            this.grpSeSetting.PerformLayout();
            this.pnlSeSearchCurr2.ResumeLayout(false);
            this.pnlSeSearchCurr2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSeSearchCurr2)).EndInit();
            this.pnlSeSearchPow2.ResumeLayout(false);
            this.pnlSeSearchPow2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSeSearchPow2)).EndInit();
            this.pnlSeSearchCurr1.ResumeLayout(false);
            this.pnlSeSearchCurr1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSeSearchCurr1)).EndInit();
            this.pnlSeSearchPow1.ResumeLayout(false);
            this.pnlSeSearchPow1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSeSearchPow1)).EndInit();
            this.grpMovingAverage.ResumeLayout(false);
            this.pnlIPMovingAvg.ResumeLayout(false);
            this.pnlIPMovingAvg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinIPMovingAvg)).EndInit();
            this.pnlIVMovingAvg.ResumeLayout(false);
            this.pnlIVMovingAvg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinIVMovingAvg)).EndInit();
            this.grpRollOver.ResumeLayout(false);
            this.pnlRollOver.ResumeLayout(false);
            this.pnlRollOver.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinRollOver)).EndInit();
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
        private System.Windows.Forms.Panel pnlStartValue;
        private DevComponents.DotNetBar.LabelX lblStartValue;
        private DevComponents.Editors.DoubleInput dinStartValue;
        private DevComponents.DotNetBar.LabelX lblStartValueUnit;
        private System.Windows.Forms.Panel pnlWaitTime;
        private DevComponents.DotNetBar.LabelX lblWaitTime;
        private DevComponents.Editors.DoubleInput dinForceDelay;
        private DevComponents.DotNetBar.LabelX lblWaitTimeUnit;
        private System.Windows.Forms.Panel pnlForceRange;
        private DevComponents.DotNetBar.LabelX lblForceRange;
        private DevComponents.DotNetBar.LabelX lblAutoForceRange;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbForceRange;
        private System.Windows.Forms.Panel pnlEndValue;
        private DevComponents.DotNetBar.LabelX lblEndValue;
        private DevComponents.Editors.DoubleInput dinEndValue;
        private DevComponents.DotNetBar.LabelX lblEndValueUnit;
        private System.Windows.Forms.Panel pnlPoints;
        private DevComponents.DotNetBar.LabelX lblPoints;
        private DevComponents.DotNetBar.LabelX lblPointsUnit;
        private System.Windows.Forms.Panel pnlStepValue;
        private DevComponents.DotNetBar.LabelX lblStepValue;
        private DevComponents.DotNetBar.LabelX lblStepValueUnit;
        private DevComponents.DotNetBar.LabelX txtDisplayPointsValue;
        private System.Windows.Forms.Panel pnlTurnOffTime;
        private DevComponents.DotNetBar.LabelX lblTurnOffTime;
        private DevComponents.Editors.DoubleInput dinTurnOffTime;
        private DevComponents.DotNetBar.LabelX lblTurnOffTimeUnit;
        private System.Windows.Forms.SplitContainer splitContainer2;
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
        private System.Windows.Forms.GroupBox grpOperationPoints;
        private DevComponents.Editors.DoubleInput dinPop;
        private System.Windows.Forms.Label lblPopUnit;
        private System.Windows.Forms.Label lblPop;
        private System.Windows.Forms.Panel PnlPop;
        private System.Windows.Forms.Panel pnlIPMovingAvg;
        private System.Windows.Forms.Label lblIPMovingAvg;
        private DevComponents.Editors.DoubleInput dinIPMovingAvg;
        private System.Windows.Forms.Label lblIPMovingAvgUnit;
        private System.Windows.Forms.Panel pnlIVMovingAvg;
        private System.Windows.Forms.Label lblIVMovingAvg;
        private DevComponents.Editors.DoubleInput dinIVMovingAvg;
        private System.Windows.Forms.Label lblIVMovingAvgUnit;
        private System.Windows.Forms.GroupBox grpSeSetting;
        private System.Windows.Forms.GroupBox grpMovingAverage;
        private System.Windows.Forms.Panel pnlIVopSelectMode;
        private System.Windows.Forms.Label lblIVopSelection;
        private System.Windows.Forms.RadioButton rdbOpInterpolation;
        private System.Windows.Forms.RadioButton rdbOpClosestpoint;
        private System.Windows.Forms.Panel pnlSeCalcSelection;
        private System.Windows.Forms.RadioButton rdbSeCalc2Point;
        private System.Windows.Forms.Label lblSeCalcSelection;
        private System.Windows.Forms.RadioButton rdbSeCalcLR;
        private System.Windows.Forms.RadioButton rdbSeSearchByCurr;
        private System.Windows.Forms.RadioButton rdbSeSearchByPow;
        private System.Windows.Forms.Panel pnlSeSearchCurr2;
        private System.Windows.Forms.Label lblSeSearchCurr2;
        private DevComponents.Editors.DoubleInput dinSeSearchCurr2;
        private System.Windows.Forms.Label lblSeSearchCurr2Unit;
        private System.Windows.Forms.Panel pnlSeSearchCurr1;
        private System.Windows.Forms.Label lblSeSearchCurr1;
        private DevComponents.Editors.DoubleInput dinSeSearchCurr1;
        private System.Windows.Forms.Label lblSeSearchCurr1Unit;
        private System.Windows.Forms.Panel pnlSeSearchPow2;
        private System.Windows.Forms.Label lblSeSearchPow2;
        private DevComponents.Editors.DoubleInput dinSeSearchPow2;
        private System.Windows.Forms.Label lblSeSearchPow2Unit;
        private System.Windows.Forms.Panel pnlSeSearchPow1;
        private System.Windows.Forms.Label lblSeSearchPow1;
        private DevComponents.Editors.DoubleInput dinSeSearchPow1;
        private System.Windows.Forms.Label lblSeSearchPow1Unit;
        private System.Windows.Forms.Panel pnlDetectorNPLC;
        private DevComponents.DotNetBar.LabelX lblDetectorNPLC;
        private DevComponents.Editors.DoubleInput dinDetectorNPLC;
        private System.Windows.Forms.Panel pnlDetectorMsrtRange;
        private DevComponents.DotNetBar.LabelX lblDetectorMsrtRange;
        private DevComponents.Editors.DoubleInput dinDetectorMsrtRange;
        private System.Windows.Forms.GroupBox grpRsSetting;
        private System.Windows.Forms.Panel pnlRsSearchCurr2;
        private System.Windows.Forms.Label lblRsSearchCurr2;
        private DevComponents.Editors.DoubleInput dinRsSearchCurr2;
        private System.Windows.Forms.Label lblRsSearchCurr2Unit;
        private System.Windows.Forms.RadioButton rdbRsSearchByCurr;
        private System.Windows.Forms.Panel pnlRsSearchCurr1;
        private System.Windows.Forms.Label lblRsSearchCurr1;
        private DevComponents.Editors.DoubleInput dinRsSearchCurr1;
        private System.Windows.Forms.Label lblRsSearchCurr1Unit;
        private System.Windows.Forms.RadioButton rdbRsSearchByPow;
        private System.Windows.Forms.Panel pnlRsSearchPow2;
        private System.Windows.Forms.Label lblRsSearchPow2;
        private DevComponents.Editors.DoubleInput dinRsSearchPow2;
        private System.Windows.Forms.Label lblRsSearchPow2Unit;
        private System.Windows.Forms.Panel pnlRsCalcSelection;
        private System.Windows.Forms.RadioButton rdbRsCalc2Point;
        private System.Windows.Forms.Label lblRsCalcSelection;
        private System.Windows.Forms.RadioButton rdbRsCalcLR;
        private System.Windows.Forms.Panel pnlRsSearchPow1;
        private System.Windows.Forms.Label lblRsSearchPow1;
        private DevComponents.Editors.DoubleInput dinRsSearchPow1;
        private System.Windows.Forms.Label lblRsSearchPow1Unit;
        private System.Windows.Forms.GroupBox grpRollOver;
        private System.Windows.Forms.Panel pnlRollOver;
        private System.Windows.Forms.Label lblRollOver;
        private DevComponents.Editors.DoubleInput dinRollOver;
        private System.Windows.Forms.Label lblRollOverUnit;
        private DevComponents.Editors.DoubleInput dinStepValue;
        private DevComponents.DotNetBar.LabelX lblDetectorMsrtRangeUnit;
        private System.Windows.Forms.GroupBox grpAdvancedSettings;
        private System.Windows.Forms.GroupBox grpKinkSetting;
        private System.Windows.Forms.Panel pnlKinkSearchCurr2;
        private System.Windows.Forms.Label lblKinkSearchCurr2;
        private DevComponents.Editors.DoubleInput dinKinkSearchCurr2;
        private System.Windows.Forms.Label lblKinkSearchCurr2Unit;
        private System.Windows.Forms.Panel pnlKinkSearchCurr1;
        private System.Windows.Forms.Label lblKinkSearchCurr1;
        private DevComponents.Editors.DoubleInput dinKinkSearchCurr1;
        private System.Windows.Forms.Label lblKinkSearchCurr1Unit;
        private System.Windows.Forms.Panel pnlDetectorBiasVoltage;
        private DevComponents.DotNetBar.LabelX lblDetectorBiasVoltageUnit;
        private DevComponents.DotNetBar.LabelX lblDetectorBiasVoltage;
        private DevComponents.Editors.DoubleInput dinDetectorBiasVoltage;
        private DevComponents.DotNetBar.Controls.GroupPanel grpMsrtSetting;
        private DevComponents.DotNetBar.Controls.GroupPanel grpPDDetector;
        private DevComponents.DotNetBar.Controls.GroupPanel grpCalcSetting;
        private System.Windows.Forms.Panel pnlKinkCalcSelection;
        private System.Windows.Forms.Label lblKinkCalcSelection;
        private System.Windows.Forms.Panel pnlDutyCycle;
        private DevComponents.DotNetBar.LabelX lblDutyCycle;
        private DevComponents.Editors.DoubleInput dinDutyCycle;
        private DevComponents.DotNetBar.LabelX lblDutyCycleUnit;
        private System.Windows.Forms.GroupBox grpSpecificPoint;
        private System.Windows.Forms.Panel pnlSpecificCurr1;
        private System.Windows.Forms.Label lblSpecificCurr1;
        private DevComponents.Editors.DoubleInput dinSpecificCurr1;
        private System.Windows.Forms.Label lblSpecificCurrUnit1;
        private System.Windows.Forms.Panel pnlSourceFunc;
        private DevComponents.DotNetBar.LabelX lblSourceFunc;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSourceFunc;
        private System.Windows.Forms.Panel pnlStepPulseCnt;
        private DevComponents.DotNetBar.LabelX lblStepPulseCnt;
        private DevComponents.Editors.IntegerInput numStepPulseCnt;
        private System.Windows.Forms.Panel pnlQcwFilterCount;
        private DevComponents.DotNetBar.LabelX lblQcwFilterCount;
        private DevComponents.Editors.IntegerInput numQcwFilterCount;
        private DevComponents.DotNetBar.LabelX lblNplcAuto;
        private DevComponents.DotNetBar.LabelX lblDetectorNplcAuto1;
        private System.Windows.Forms.Panel pnlSpecificCurr3;
        private System.Windows.Forms.Label lblSpecificCurr3;
        private DevComponents.Editors.DoubleInput dinSpecificCurr3;
        private System.Windows.Forms.Label lblSpecificCurrUnit3;
        private System.Windows.Forms.Panel pnlSpecificCurr2;
        private System.Windows.Forms.Label lblSpecificCurr2;
        private DevComponents.Editors.DoubleInput dinSpecificCurr2;
        private System.Windows.Forms.Label lblSpecificCurrUnit2;
        private System.Windows.Forms.GroupBox grpSe2Setting;
        private System.Windows.Forms.Panel pnlSe2SearchCurr2;
        private System.Windows.Forms.Label lblSe2SearchCurr2;
        private DevComponents.Editors.DoubleInput dinSe2SearchCurr2;
        private System.Windows.Forms.Label lblSe2SearchCurr2Unit;
        private System.Windows.Forms.RadioButton rdbSe2SearchByCurr;
        private System.Windows.Forms.Panel pnlSe2SearchCurr1;
        private System.Windows.Forms.Label lblSe2SearchCurr1;
        private DevComponents.Editors.DoubleInput dinSe2SearchCurr1;
        private System.Windows.Forms.Label lblSe2SearchCurr1Unit;
        private System.Windows.Forms.RadioButton rdbSe2SearchByPow;
        private System.Windows.Forms.Panel pnlSe2SearchPow2;
        private System.Windows.Forms.Label lblSe2SearchPow2;
        private DevComponents.Editors.DoubleInput dinSe2SearchPow2;
        private System.Windows.Forms.Label lblSe2SearchPow2Unit;
        private System.Windows.Forms.Panel pnlSe2SearchPow1;
        private System.Windows.Forms.Label lblSe2SearchPow1;
        private DevComponents.Editors.DoubleInput dinSe2SearchPow1;
        private System.Windows.Forms.Label lblSe2SearchPow1Unit;
        private System.Windows.Forms.GroupBox grpThresholdPoints;
        private System.Windows.Forms.Panel pnlThresholdSearchPow2;
        private System.Windows.Forms.Label lblThresholdSearchPow2;
        private DevComponents.Editors.DoubleInput dinThresholdSearchPow2;
        private System.Windows.Forms.Label lblThresholdSearchPow2Unit;
        private System.Windows.Forms.Panel pnlThresholdSearchPow1;
        private System.Windows.Forms.Label lblThresholdSearchPow1;
        private DevComponents.Editors.DoubleInput dinThresholdSearchPow1;
        private System.Windows.Forms.Label lblThresholdSearchPow1Unit;
        private System.Windows.Forms.GroupBox grpLinearitySetting;
        private System.Windows.Forms.Panel pnlLnSearchCurr2;
        private System.Windows.Forms.Label lblLnSearchCurr2;
        private DevComponents.Editors.DoubleInput dinLnSearchCurr2;
        private System.Windows.Forms.Label lblLnSearchCurr2Unit;
        private System.Windows.Forms.RadioButton rdbLnSearchByCurr;
        private System.Windows.Forms.Panel pnlLnSearchCurr1;
        private System.Windows.Forms.Label lblLnSearchCurr1;
        private DevComponents.Editors.DoubleInput dinLnSearchCurr1;
        private System.Windows.Forms.Label lblLnSearchCurr1Unit;
        private System.Windows.Forms.RadioButton rdbLnSearchByPow;
        private System.Windows.Forms.Panel pnlLnSearchPow2;
        private System.Windows.Forms.Label lblLnSearchPow2;
        private DevComponents.Editors.DoubleInput dinLnSearchPow2;
        private System.Windows.Forms.Label lblLnSearchPow2Unit;
        private System.Windows.Forms.Panel pnlLnSearchPow1;
        private System.Windows.Forms.Label lblLnSearchPow1;
        private DevComponents.Editors.DoubleInput dinLnSearchPow1;
        private System.Windows.Forms.Label lblLnSearchPow1Unit;
        private DevComponents.Editors.DoubleInput dinThresholdSearchCurr2;
        private DevComponents.Editors.DoubleInput dinThresholdSearchCurr1;
        private System.Windows.Forms.Panel pnlThresholdSearchCurr2;
        private System.Windows.Forms.Label lblThresholdSearchCurr2;
        private System.Windows.Forms.Label lblThresholdSearchCurr2Unit;
        private System.Windows.Forms.Panel pnlThresholdSearchCurr1;
        private System.Windows.Forms.Label lblThresholdSearchCurr1;
        private System.Windows.Forms.Label lblThresholdSearchCurr1Unit;
        private System.Windows.Forms.RadioButton rdbThresholdSearchByCurr;
        private System.Windows.Forms.RadioButton rdbThresholdSearchByPow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdbThresholdCalc2Point;
        private System.Windows.Forms.Label lblThresholdCalcSelection;
        private System.Windows.Forms.RadioButton rdbThresholdCalcLR;
        private System.Windows.Forms.Panel pnlKinkRatio;
        private System.Windows.Forms.Label lblKinkRatio;
        private DevComponents.Editors.DoubleInput dinKinkRatio;
        private System.Windows.Forms.Label lblKinkRatioUnit;
        private System.Windows.Forms.Panel pnlThresholdSearchValue;
        private System.Windows.Forms.Label lblThresholdSearchValue;
        private DevComponents.Editors.DoubleInput dinThresholdSearchValue;
        private System.Windows.Forms.Label lblThresholdSearchValueUnit;
        private System.Windows.Forms.RadioButton rdbThresholdCalcThr;
        private System.Windows.Forms.RadioButton rdbSeCalcAvg;
        private System.Windows.Forms.RadioButton rdbRsCalcAvg;
        private System.Windows.Forms.RadioButton rdbThresholdCalcPo;
        private System.Windows.Forms.Panel pnlThresholdSearchValue2;
        private System.Windows.Forms.Label lblThresholdSearchValue2;
        private DevComponents.Editors.DoubleInput dinThresholdSearchValue2;
        private System.Windows.Forms.Label lblThresholdSearchValueUnit2;
        private System.Windows.Forms.RadioButton rdbKinkCalcFittingLine;
        private System.Windows.Forms.RadioButton rdbKinkCalcRefCurve;
        private System.Windows.Forms.RadioButton rdbKinkCalcDeltaPow;
        private System.Windows.Forms.RadioButton rdbKinkCalcSEk;
        private System.Windows.Forms.RadioButton rdbKinkCalcSecondOrder;
        private DevComponents.DotNetBar.Controls.GroupPanel grpChannel;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSelectedChannel;
        private DevComponents.DotNetBar.LabelX lblSelectedChannel;
        private DevComponents.DotNetBar.LabelX lblPulseWidth;
        private System.Windows.Forms.RadioButton rdbOpOnFittingLine;

    }
}