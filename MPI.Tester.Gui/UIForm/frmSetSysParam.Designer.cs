namespace MPI.Tester.Gui
{
    partial class frmSetSysParam
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetSysParam));
            this.lblBoxCarTitle = new DevComponents.DotNetBar.LabelX();
            this.numBoxCar = new DevComponents.Editors.IntegerInput();
            this.numMinCatchCount = new DevComponents.Editors.IntegerInput();
            this.lblMinCatchCountTitle = new DevComponents.DotNetBar.LabelX();
            this.chkIsSaveDarkSpectrum = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsNonlinearityCorrect = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsCorrectBackLight = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblDarkCorrectTitle = new DevComponents.DotNetBar.LabelX();
            this.cmbDarkCorrectMode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbSptMeterOpMode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSptMeterFilterModeTitle = new DevComponents.DotNetBar.LabelX();
            this.lblSptMeterOpModeTitle = new DevComponents.DotNetBar.LabelX();
            this.cmbSptMeterFilterMode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.numEndWave = new DevComponents.Editors.IntegerInput();
            this.numStartWave = new DevComponents.Editors.IntegerInput();
            this.dinNoiseBase = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.numRepeatCount = new DevComponents.Editors.IntegerInput();
            this.numRepeatDelay = new DevComponents.Editors.IntegerInput();
            this.chkSyncSpecAndBin = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmbGroupBinSortingRule = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.lblGroupBinSorting = new DevComponents.DotNetBar.LabelX();
            this.pnlCalSetting = new System.Windows.Forms.Panel();
            this.lblCalibarationSetting = new DevComponents.DotNetBar.LabelX();
            this.plSptCustomizing = new System.Windows.Forms.Panel();
            this.chkCalcBigFactorBeforeSmall = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsUseNDFilterRatio = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.pnlLimitModeSetting = new System.Windows.Forms.Panel();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.lblLimitStartTimeFactor = new DevComponents.DotNetBar.LabelX();
            this.dinLimitStartTimeFactor = new DevComponents.Editors.DoubleInput();
            this.labelX17 = new DevComponents.DotNetBar.LabelX();
            this.lblLimit02MinSTTime = new DevComponents.DotNetBar.LabelX();
            this.dinLimit02TurnOffTime = new DevComponents.Editors.IntegerInput();
            this.intLimit02MinSTTime = new DevComponents.Editors.IntegerInput();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.dinLimit02PeakPercent = new DevComponents.Editors.IntegerInput();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.pnlSptDevParamSetting = new System.Windows.Forms.Panel();
            this.lblSpectrometerGroup = new DevComponents.DotNetBar.LabelX();
            this.lblMaxWaveTitle = new DevComponents.DotNetBar.LabelX();
            this.lblMinWaveTitle = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.chkIsCalcCCTAndCRI = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsCalcCCTByCaliCIExy = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmbCCTcalcType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblCCTCalcMode = new DevComponents.DotNetBar.LabelX();
            this.lblCalcSpecialWLPPlaceUnit = new DevComponents.DotNetBar.LabelX();
            this.dinCalcSpecialWLPPlace = new DevComponents.Editors.DoubleInput();
            this.chkIsCalcSpecailWLP = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.dinBaseNosie = new DevComponents.Editors.DoubleInput();
            this.lblNosieBase = new DevComponents.DotNetBar.LabelX();
            this.pnlSDCMSetting = new System.Windows.Forms.Panel();
            this.lblSDCMSetting = new DevComponents.DotNetBar.LabelX();
            this.chkIsCalcANSIAAndGB = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmbANSICalcCCT = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblANSICalcCCT = new DevComponents.DotNetBar.LabelX();
            this.cmbGBCalcCCT = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblGBCalcCCT = new DevComponents.DotNetBar.LabelX();
            this.pnlPdDetectorConfig = new System.Windows.Forms.Panel();
            this.lblPDDarkCorrectTitle = new DevComponents.DotNetBar.LabelX();
            this.cmbPDDarkCorrectMode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblPDDetectorConfig = new DevComponents.DotNetBar.LabelX();
            this.chkIsEnableFloatReport = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.numDefaultBinGrade = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.chkEnableSettingDefaultBin = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.pnlSptMeterAdvSetting = new System.Windows.Forms.Panel();
            this.chkIsAdjacentError = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkBinSortingIncludeMinMax = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblBinSortingRule = new DevComponents.DotNetBar.LabelX();
            this.cmbBinSortingRule = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.lblBinSorting = new DevComponents.DotNetBar.LabelX();
            this.lblSingleTestSetting = new DevComponents.DotNetBar.LabelX();
            this.lblCriterionSetting = new DevComponents.DotNetBar.LabelX();
            this.chkIsCountPassFaiByBinGrade = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsCheckSpec2 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsCheckRowCol = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.plSptAdvDeviceSetting = new System.Windows.Forms.Panel();
            this.chkEnableSptModifyXCoeff = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEnableSptModifyYweigth = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEnableSptModifyYCoeff = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsFullyAutomatic = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblLimitStartTimeUnit = new DevComponents.DotNetBar.LabelX();
            this.lblSpectrumOpModeTitle = new DevComponents.DotNetBar.LabelX();
            this.lblLimitStartTimeTitle = new DevComponents.DotNetBar.LabelX();
            this.lblSpectrometerSetting = new DevComponents.DotNetBar.LabelX();
            this.lblCoordinateGroup = new DevComponents.DotNetBar.LabelX();
            this.lblTesterCoordSetUnit = new DevComponents.DotNetBar.LabelX();
            this.lblProberCoordSetUnit = new DevComponents.DotNetBar.LabelX();
            this.lblProberCoordSetTitle = new DevComponents.DotNetBar.LabelX();
            this.lblTesterCoordSetTitle = new DevComponents.DotNetBar.LabelX();
            this.numProberCoordSet = new DevComponents.Editors.IntegerInput();
            this.numTesterCoordSet = new DevComponents.Editors.IntegerInput();
            this.lblBasicSysParamSetting = new DevComponents.DotNetBar.LabelX();
            this.lblRepeatDelayTitle = new DevComponents.DotNetBar.LabelX();
            this.lblRepeatCountTitle = new DevComponents.DotNetBar.LabelX();
            this.lblRepeatDelayUnit = new DevComponents.DotNetBar.LabelX();
            this.lblRepeatCountUnit = new DevComponents.DotNetBar.LabelX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnFAEFunc = new DevComponents.DotNetBar.ButtonX();
            this.btnRDFunc = new DevComponents.DotNetBar.ButtonX();
            this.btnESDSysParam = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.numBoxCar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinCatchCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndWave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartWave)).BeginInit();
            this.dinNoiseBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRepeatCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRepeatDelay)).BeginInit();
            this.pnlCalSetting.SuspendLayout();
            this.plSptCustomizing.SuspendLayout();
            this.pnlLimitModeSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinLimitStartTimeFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinLimit02TurnOffTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intLimit02MinSTTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinLimit02PeakPercent)).BeginInit();
            this.pnlSptDevParamSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinCalcSpecialWLPPlace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinBaseNosie)).BeginInit();
            this.pnlSDCMSetting.SuspendLayout();
            this.pnlPdDetectorConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultBinGrade)).BeginInit();
            this.pnlSptMeterAdvSetting.SuspendLayout();
            this.plSptAdvDeviceSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProberCoordSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTesterCoordSet)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBoxCarTitle
            // 
            this.lblBoxCarTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblBoxCarTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblBoxCarTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblBoxCarTitle, "lblBoxCarTitle");
            this.lblBoxCarTitle.ForeColor = System.Drawing.Color.Black;
            this.lblBoxCarTitle.Name = "lblBoxCarTitle";
            this.lblBoxCarTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // numBoxCar
            // 
            this.numBoxCar.AntiAlias = true;
            // 
            // 
            // 
            this.numBoxCar.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numBoxCar.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numBoxCar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numBoxCar, "numBoxCar");
            this.numBoxCar.MaxValue = 15;
            this.numBoxCar.MinValue = 0;
            this.numBoxCar.Name = "numBoxCar";
            this.numBoxCar.ShowUpDown = true;
            this.numBoxCar.Value = 6;
            // 
            // numMinCatchCount
            // 
            this.numMinCatchCount.AntiAlias = true;
            // 
            // 
            // 
            this.numMinCatchCount.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numMinCatchCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numMinCatchCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numMinCatchCount, "numMinCatchCount");
            this.numMinCatchCount.MaxValue = 10000;
            this.numMinCatchCount.MinValue = 700;
            this.numMinCatchCount.Name = "numMinCatchCount";
            this.numMinCatchCount.ShowUpDown = true;
            this.numMinCatchCount.Value = 5000;
            // 
            // lblMinCatchCountTitle
            // 
            this.lblMinCatchCountTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblMinCatchCountTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblMinCatchCountTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMinCatchCountTitle, "lblMinCatchCountTitle");
            this.lblMinCatchCountTitle.ForeColor = System.Drawing.Color.Black;
            this.lblMinCatchCountTitle.Name = "lblMinCatchCountTitle";
            this.lblMinCatchCountTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // chkIsSaveDarkSpectrum
            // 
            this.chkIsSaveDarkSpectrum.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsSaveDarkSpectrum.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsSaveDarkSpectrum.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsSaveDarkSpectrum.CheckSignSize = new System.Drawing.Size(17, 17);
            resources.ApplyResources(this.chkIsSaveDarkSpectrum, "chkIsSaveDarkSpectrum");
            this.chkIsSaveDarkSpectrum.Name = "chkIsSaveDarkSpectrum";
            this.chkIsSaveDarkSpectrum.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsSaveDarkSpectrum.TabStop = false;
            // 
            // chkIsNonlinearityCorrect
            // 
            this.chkIsNonlinearityCorrect.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsNonlinearityCorrect.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsNonlinearityCorrect.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsNonlinearityCorrect.Checked = true;
            this.chkIsNonlinearityCorrect.CheckSignSize = new System.Drawing.Size(16, 16);
            this.chkIsNonlinearityCorrect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsNonlinearityCorrect.CheckValue = "Y";
            resources.ApplyResources(this.chkIsNonlinearityCorrect, "chkIsNonlinearityCorrect");
            this.chkIsNonlinearityCorrect.Name = "chkIsNonlinearityCorrect";
            this.chkIsNonlinearityCorrect.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsNonlinearityCorrect.TabStop = false;
            // 
            // chkIsCorrectBackLight
            // 
            this.chkIsCorrectBackLight.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsCorrectBackLight.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsCorrectBackLight.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsCorrectBackLight.CheckSignSize = new System.Drawing.Size(16, 16);
            resources.ApplyResources(this.chkIsCorrectBackLight, "chkIsCorrectBackLight");
            this.chkIsCorrectBackLight.Name = "chkIsCorrectBackLight";
            this.chkIsCorrectBackLight.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsCorrectBackLight.TabStop = false;
            // 
            // lblDarkCorrectTitle
            // 
            this.lblDarkCorrectTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblDarkCorrectTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblDarkCorrectTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblDarkCorrectTitle, "lblDarkCorrectTitle");
            this.lblDarkCorrectTitle.ForeColor = System.Drawing.Color.Black;
            this.lblDarkCorrectTitle.Name = "lblDarkCorrectTitle";
            this.lblDarkCorrectTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // cmbDarkCorrectMode
            // 
            this.cmbDarkCorrectMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDarkCorrectMode.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbDarkCorrectMode, "cmbDarkCorrectMode");
            this.cmbDarkCorrectMode.ForeColor = System.Drawing.Color.Black;
            this.cmbDarkCorrectMode.Name = "cmbDarkCorrectMode";
            // 
            // cmbSptMeterOpMode
            // 
            this.cmbSptMeterOpMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSptMeterOpMode.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbSptMeterOpMode, "cmbSptMeterOpMode");
            this.cmbSptMeterOpMode.ForeColor = System.Drawing.Color.Black;
            this.cmbSptMeterOpMode.Name = "cmbSptMeterOpMode";
            // 
            // lblSptMeterFilterModeTitle
            // 
            this.lblSptMeterFilterModeTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblSptMeterFilterModeTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblSptMeterFilterModeTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblSptMeterFilterModeTitle, "lblSptMeterFilterModeTitle");
            this.lblSptMeterFilterModeTitle.ForeColor = System.Drawing.Color.Black;
            this.lblSptMeterFilterModeTitle.Name = "lblSptMeterFilterModeTitle";
            this.lblSptMeterFilterModeTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblSptMeterOpModeTitle
            // 
            this.lblSptMeterOpModeTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblSptMeterOpModeTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblSptMeterOpModeTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblSptMeterOpModeTitle, "lblSptMeterOpModeTitle");
            this.lblSptMeterOpModeTitle.ForeColor = System.Drawing.Color.Black;
            this.lblSptMeterOpModeTitle.Name = "lblSptMeterOpModeTitle";
            this.lblSptMeterOpModeTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // cmbSptMeterFilterMode
            // 
            this.cmbSptMeterFilterMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSptMeterFilterMode.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbSptMeterFilterMode, "cmbSptMeterFilterMode");
            this.cmbSptMeterFilterMode.ForeColor = System.Drawing.Color.Black;
            this.cmbSptMeterFilterMode.Name = "cmbSptMeterFilterMode";
            // 
            // numEndWave
            // 
            this.numEndWave.AntiAlias = true;
            // 
            // 
            // 
            this.numEndWave.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numEndWave.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numEndWave.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numEndWave, "numEndWave");
            this.numEndWave.MaxValue = 3000;
            this.numEndWave.MinValue = 1;
            this.numEndWave.Name = "numEndWave";
            this.numEndWave.ShowUpDown = true;
            this.numEndWave.Value = 780;
            // 
            // numStartWave
            // 
            this.numStartWave.AntiAlias = true;
            // 
            // 
            // 
            this.numStartWave.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numStartWave.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numStartWave.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numStartWave, "numStartWave");
            this.numStartWave.MaxValue = 3000;
            this.numStartWave.MinValue = 1;
            this.numStartWave.Name = "numStartWave";
            this.numStartWave.ShowUpDown = true;
            this.numStartWave.Value = 380;
            // 
            // dinNoiseBase
            // 
            this.dinNoiseBase.CanvasColor = System.Drawing.SystemColors.Control;
            this.dinNoiseBase.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.dinNoiseBase.Controls.Add(this.numRepeatCount);
            this.dinNoiseBase.Controls.Add(this.numRepeatDelay);
            this.dinNoiseBase.Controls.Add(this.chkSyncSpecAndBin);
            this.dinNoiseBase.Controls.Add(this.cmbGroupBinSortingRule);
            this.dinNoiseBase.Controls.Add(this.lblGroupBinSorting);
            this.dinNoiseBase.Controls.Add(this.pnlCalSetting);
            this.dinNoiseBase.Controls.Add(this.pnlLimitModeSetting);
            this.dinNoiseBase.Controls.Add(this.pnlSptDevParamSetting);
            this.dinNoiseBase.Controls.Add(this.pnlSDCMSetting);
            this.dinNoiseBase.Controls.Add(this.pnlPdDetectorConfig);
            this.dinNoiseBase.Controls.Add(this.chkIsEnableFloatReport);
            this.dinNoiseBase.Controls.Add(this.numDefaultBinGrade);
            this.dinNoiseBase.Controls.Add(this.labelX1);
            this.dinNoiseBase.Controls.Add(this.chkEnableSettingDefaultBin);
            this.dinNoiseBase.Controls.Add(this.pnlSptMeterAdvSetting);
            this.dinNoiseBase.Controls.Add(this.chkIsAdjacentError);
            this.dinNoiseBase.Controls.Add(this.chkBinSortingIncludeMinMax);
            this.dinNoiseBase.Controls.Add(this.lblBinSortingRule);
            this.dinNoiseBase.Controls.Add(this.cmbBinSortingRule);
            this.dinNoiseBase.Controls.Add(this.lblBinSorting);
            this.dinNoiseBase.Controls.Add(this.lblSingleTestSetting);
            this.dinNoiseBase.Controls.Add(this.lblCriterionSetting);
            this.dinNoiseBase.Controls.Add(this.chkIsCountPassFaiByBinGrade);
            this.dinNoiseBase.Controls.Add(this.chkIsCheckSpec2);
            this.dinNoiseBase.Controls.Add(this.chkIsCheckRowCol);
            this.dinNoiseBase.Controls.Add(this.plSptAdvDeviceSetting);
            this.dinNoiseBase.Controls.Add(this.lblLimitStartTimeUnit);
            this.dinNoiseBase.Controls.Add(this.lblSpectrumOpModeTitle);
            this.dinNoiseBase.Controls.Add(this.lblLimitStartTimeTitle);
            this.dinNoiseBase.Controls.Add(this.lblSpectrometerSetting);
            this.dinNoiseBase.Controls.Add(this.lblCoordinateGroup);
            this.dinNoiseBase.Controls.Add(this.lblTesterCoordSetUnit);
            this.dinNoiseBase.Controls.Add(this.lblProberCoordSetUnit);
            this.dinNoiseBase.Controls.Add(this.lblProberCoordSetTitle);
            this.dinNoiseBase.Controls.Add(this.lblTesterCoordSetTitle);
            this.dinNoiseBase.Controls.Add(this.numProberCoordSet);
            this.dinNoiseBase.Controls.Add(this.numTesterCoordSet);
            this.dinNoiseBase.Controls.Add(this.lblBasicSysParamSetting);
            this.dinNoiseBase.Controls.Add(this.lblRepeatDelayTitle);
            this.dinNoiseBase.Controls.Add(this.lblRepeatCountTitle);
            this.dinNoiseBase.Controls.Add(this.lblRepeatDelayUnit);
            this.dinNoiseBase.Controls.Add(this.lblRepeatCountUnit);
            this.dinNoiseBase.DrawTitleBox = false;
            resources.ApplyResources(this.dinNoiseBase, "dinNoiseBase");
            this.dinNoiseBase.Name = "dinNoiseBase";
            // 
            // 
            // 
            this.dinNoiseBase.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dinNoiseBase.Style.BackColor2 = System.Drawing.Color.GhostWhite;
            this.dinNoiseBase.Style.BackColorGradientAngle = 90;
            this.dinNoiseBase.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dinNoiseBase.Style.BorderBottomWidth = 1;
            this.dinNoiseBase.Style.BorderColor = System.Drawing.Color.Gray;
            this.dinNoiseBase.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dinNoiseBase.Style.BorderLeftWidth = 1;
            this.dinNoiseBase.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dinNoiseBase.Style.BorderRightWidth = 1;
            this.dinNoiseBase.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dinNoiseBase.Style.BorderTopWidth = 1;
            this.dinNoiseBase.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.dinNoiseBase.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.dinNoiseBase.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.dinNoiseBase.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            // 
            // 
            // 
            this.dinNoiseBase.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.dinNoiseBase.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.dinNoiseBase.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.dinNoiseBase.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinNoiseBase.TitleImagePosition = DevComponents.DotNetBar.eTitleImagePosition.Center;
            // 
            // numRepeatCount
            // 
            this.numRepeatCount.AntiAlias = true;
            // 
            // 
            // 
            this.numRepeatCount.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numRepeatCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numRepeatCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numRepeatCount, "numRepeatCount");
            this.numRepeatCount.MaxValue = 999999;
            this.numRepeatCount.MinValue = 1;
            this.numRepeatCount.Name = "numRepeatCount";
            this.numRepeatCount.ShowUpDown = true;
            this.numRepeatCount.Value = 1000;
            // 
            // numRepeatDelay
            // 
            this.numRepeatDelay.AntiAlias = true;
            // 
            // 
            // 
            this.numRepeatDelay.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numRepeatDelay.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numRepeatDelay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numRepeatDelay, "numRepeatDelay");
            this.numRepeatDelay.MaxValue = 10000000;
            this.numRepeatDelay.MinValue = 1;
            this.numRepeatDelay.Name = "numRepeatDelay";
            this.numRepeatDelay.ShowUpDown = true;
            this.numRepeatDelay.Value = 50;
            // 
            // chkSyncSpecAndBin
            // 
            this.chkSyncSpecAndBin.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkSyncSpecAndBin.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkSyncSpecAndBin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkSyncSpecAndBin.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkSyncSpecAndBin, "chkSyncSpecAndBin");
            this.chkSyncSpecAndBin.Name = "chkSyncSpecAndBin";
            this.chkSyncSpecAndBin.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkSyncSpecAndBin.TabStop = false;
            // 
            // cmbGroupBinSortingRule
            // 
            this.cmbGroupBinSortingRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupBinSortingRule.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbGroupBinSortingRule, "cmbGroupBinSortingRule");
            this.cmbGroupBinSortingRule.ForeColor = System.Drawing.Color.Black;
            this.cmbGroupBinSortingRule.Items.AddRange(new object[] {
            this.comboItem4,
            this.comboItem5,
            this.comboItem6});
            this.cmbGroupBinSortingRule.Name = "cmbGroupBinSortingRule";
            // 
            // comboItem4
            // 
            resources.ApplyResources(this.comboItem4, "comboItem4");
            // 
            // comboItem5
            // 
            resources.ApplyResources(this.comboItem5, "comboItem5");
            // 
            // comboItem6
            // 
            resources.ApplyResources(this.comboItem6, "comboItem6");
            // 
            // lblGroupBinSorting
            // 
            this.lblGroupBinSorting.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblGroupBinSorting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblGroupBinSorting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblGroupBinSorting, "lblGroupBinSorting");
            this.lblGroupBinSorting.ForeColor = System.Drawing.Color.Black;
            this.lblGroupBinSorting.Name = "lblGroupBinSorting";
            this.lblGroupBinSorting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // pnlCalSetting
            // 
            this.pnlCalSetting.Controls.Add(this.lblCalibarationSetting);
            this.pnlCalSetting.Controls.Add(this.plSptCustomizing);
            this.pnlCalSetting.Controls.Add(this.chkCalcBigFactorBeforeSmall);
            this.pnlCalSetting.Controls.Add(this.chkIsUseNDFilterRatio);
            resources.ApplyResources(this.pnlCalSetting, "pnlCalSetting");
            this.pnlCalSetting.Name = "pnlCalSetting";
            // 
            // lblCalibarationSetting
            // 
            this.lblCalibarationSetting.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblCalibarationSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblCalibarationSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCalibarationSetting, "lblCalibarationSetting");
            this.lblCalibarationSetting.ForeColor = System.Drawing.Color.White;
            this.lblCalibarationSetting.Name = "lblCalibarationSetting";
            this.lblCalibarationSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblCalibarationSetting.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // plSptCustomizing
            // 
            this.plSptCustomizing.Controls.Add(this.lblDarkCorrectTitle);
            this.plSptCustomizing.Controls.Add(this.cmbDarkCorrectMode);
            resources.ApplyResources(this.plSptCustomizing, "plSptCustomizing");
            this.plSptCustomizing.Name = "plSptCustomizing";
            // 
            // chkCalcBigFactorBeforeSmall
            // 
            this.chkCalcBigFactorBeforeSmall.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkCalcBigFactorBeforeSmall.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkCalcBigFactorBeforeSmall.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCalcBigFactorBeforeSmall.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkCalcBigFactorBeforeSmall, "chkCalcBigFactorBeforeSmall");
            this.chkCalcBigFactorBeforeSmall.Name = "chkCalcBigFactorBeforeSmall";
            this.chkCalcBigFactorBeforeSmall.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkCalcBigFactorBeforeSmall.TabStop = false;
            // 
            // chkIsUseNDFilterRatio
            // 
            this.chkIsUseNDFilterRatio.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsUseNDFilterRatio.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsUseNDFilterRatio.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsUseNDFilterRatio.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsUseNDFilterRatio, "chkIsUseNDFilterRatio");
            this.chkIsUseNDFilterRatio.Name = "chkIsUseNDFilterRatio";
            this.chkIsUseNDFilterRatio.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsUseNDFilterRatio.TabStop = false;
            // 
            // pnlLimitModeSetting
            // 
            this.pnlLimitModeSetting.Controls.Add(this.labelX7);
            this.pnlLimitModeSetting.Controls.Add(this.lblMinCatchCountTitle);
            this.pnlLimitModeSetting.Controls.Add(this.numMinCatchCount);
            this.pnlLimitModeSetting.Controls.Add(this.labelX13);
            this.pnlLimitModeSetting.Controls.Add(this.lblLimitStartTimeFactor);
            this.pnlLimitModeSetting.Controls.Add(this.dinLimitStartTimeFactor);
            this.pnlLimitModeSetting.Controls.Add(this.labelX17);
            this.pnlLimitModeSetting.Controls.Add(this.lblLimit02MinSTTime);
            this.pnlLimitModeSetting.Controls.Add(this.dinLimit02TurnOffTime);
            this.pnlLimitModeSetting.Controls.Add(this.intLimit02MinSTTime);
            this.pnlLimitModeSetting.Controls.Add(this.labelX8);
            this.pnlLimitModeSetting.Controls.Add(this.labelX16);
            this.pnlLimitModeSetting.Controls.Add(this.labelX10);
            this.pnlLimitModeSetting.Controls.Add(this.dinLimit02PeakPercent);
            this.pnlLimitModeSetting.Controls.Add(this.labelX14);
            resources.ApplyResources(this.pnlLimitModeSetting, "pnlLimitModeSetting");
            this.pnlLimitModeSetting.Name = "pnlLimitModeSetting";
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX7, "labelX7");
            this.labelX7.ForeColor = System.Drawing.Color.White;
            this.labelX7.Name = "labelX7";
            this.labelX7.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX7.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX13
            // 
            this.labelX13.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX13, "labelX13");
            this.labelX13.ForeColor = System.Drawing.Color.Black;
            this.labelX13.Name = "labelX13";
            this.labelX13.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblLimitStartTimeFactor
            // 
            this.lblLimitStartTimeFactor.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblLimitStartTimeFactor.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblLimitStartTimeFactor.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblLimitStartTimeFactor, "lblLimitStartTimeFactor");
            this.lblLimitStartTimeFactor.ForeColor = System.Drawing.Color.Black;
            this.lblLimitStartTimeFactor.Name = "lblLimitStartTimeFactor";
            this.lblLimitStartTimeFactor.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
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
            // labelX17
            // 
            this.labelX17.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX17.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX17.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX17, "labelX17");
            this.labelX17.ForeColor = System.Drawing.Color.Black;
            this.labelX17.Name = "labelX17";
            this.labelX17.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX17.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblLimit02MinSTTime
            // 
            this.lblLimit02MinSTTime.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblLimit02MinSTTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblLimit02MinSTTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblLimit02MinSTTime, "lblLimit02MinSTTime");
            this.lblLimit02MinSTTime.ForeColor = System.Drawing.Color.Black;
            this.lblLimit02MinSTTime.Name = "lblLimit02MinSTTime";
            this.lblLimit02MinSTTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // dinLimit02TurnOffTime
            // 
            this.dinLimit02TurnOffTime.AntiAlias = true;
            // 
            // 
            // 
            this.dinLimit02TurnOffTime.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.dinLimit02TurnOffTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLimit02TurnOffTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.dinLimit02TurnOffTime, "dinLimit02TurnOffTime");
            this.dinLimit02TurnOffTime.MaxValue = 500;
            this.dinLimit02TurnOffTime.MinValue = 20;
            this.dinLimit02TurnOffTime.Name = "dinLimit02TurnOffTime";
            this.dinLimit02TurnOffTime.ShowUpDown = true;
            this.dinLimit02TurnOffTime.Value = 20;
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
            this.intLimit02MinSTTime.Value = 5;
            // 
            // labelX8
            // 
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX8, "labelX8");
            this.labelX8.ForeColor = System.Drawing.Color.Black;
            this.labelX8.Name = "labelX8";
            this.labelX8.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX8.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX16
            // 
            this.labelX16.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.labelX16.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX16.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX16, "labelX16");
            this.labelX16.ForeColor = System.Drawing.Color.Black;
            this.labelX16.Name = "labelX16";
            this.labelX16.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // labelX10
            // 
            this.labelX10.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX10, "labelX10");
            this.labelX10.ForeColor = System.Drawing.Color.Black;
            this.labelX10.Name = "labelX10";
            this.labelX10.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // dinLimit02PeakPercent
            // 
            this.dinLimit02PeakPercent.AntiAlias = true;
            // 
            // 
            // 
            this.dinLimit02PeakPercent.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.dinLimit02PeakPercent.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLimit02PeakPercent.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.dinLimit02PeakPercent, "dinLimit02PeakPercent");
            this.dinLimit02PeakPercent.MaxValue = 90;
            this.dinLimit02PeakPercent.MinValue = 20;
            this.dinLimit02PeakPercent.Name = "dinLimit02PeakPercent";
            this.dinLimit02PeakPercent.ShowUpDown = true;
            this.dinLimit02PeakPercent.Value = 20;
            // 
            // labelX14
            // 
            this.labelX14.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX14, "labelX14");
            this.labelX14.ForeColor = System.Drawing.Color.Black;
            this.labelX14.Name = "labelX14";
            this.labelX14.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX14.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // pnlSptDevParamSetting
            // 
            this.pnlSptDevParamSetting.BackColor = System.Drawing.Color.Transparent;
            this.pnlSptDevParamSetting.Controls.Add(this.lblSpectrometerGroup);
            this.pnlSptDevParamSetting.Controls.Add(this.lblMaxWaveTitle);
            this.pnlSptDevParamSetting.Controls.Add(this.lblMinWaveTitle);
            this.pnlSptDevParamSetting.Controls.Add(this.numStartWave);
            this.pnlSptDevParamSetting.Controls.Add(this.numEndWave);
            this.pnlSptDevParamSetting.Controls.Add(this.labelX11);
            this.pnlSptDevParamSetting.Controls.Add(this.labelX12);
            this.pnlSptDevParamSetting.Controls.Add(this.cmbSptMeterFilterMode);
            this.pnlSptDevParamSetting.Controls.Add(this.chkIsCalcCCTAndCRI);
            this.pnlSptDevParamSetting.Controls.Add(this.chkIsCalcCCTByCaliCIExy);
            this.pnlSptDevParamSetting.Controls.Add(this.cmbCCTcalcType);
            this.pnlSptDevParamSetting.Controls.Add(this.lblSptMeterFilterModeTitle);
            this.pnlSptDevParamSetting.Controls.Add(this.lblCCTCalcMode);
            this.pnlSptDevParamSetting.Controls.Add(this.lblCalcSpecialWLPPlaceUnit);
            this.pnlSptDevParamSetting.Controls.Add(this.numBoxCar);
            this.pnlSptDevParamSetting.Controls.Add(this.dinCalcSpecialWLPPlace);
            this.pnlSptDevParamSetting.Controls.Add(this.lblBoxCarTitle);
            this.pnlSptDevParamSetting.Controls.Add(this.chkIsCalcSpecailWLP);
            this.pnlSptDevParamSetting.Controls.Add(this.dinBaseNosie);
            this.pnlSptDevParamSetting.Controls.Add(this.lblNosieBase);
            resources.ApplyResources(this.pnlSptDevParamSetting, "pnlSptDevParamSetting");
            this.pnlSptDevParamSetting.Name = "pnlSptDevParamSetting";
            // 
            // lblSpectrometerGroup
            // 
            this.lblSpectrometerGroup.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblSpectrometerGroup.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblSpectrometerGroup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblSpectrometerGroup, "lblSpectrometerGroup");
            this.lblSpectrometerGroup.ForeColor = System.Drawing.Color.White;
            this.lblSpectrometerGroup.Name = "lblSpectrometerGroup";
            this.lblSpectrometerGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblSpectrometerGroup.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblMaxWaveTitle
            // 
            this.lblMaxWaveTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblMaxWaveTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblMaxWaveTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMaxWaveTitle, "lblMaxWaveTitle");
            this.lblMaxWaveTitle.ForeColor = System.Drawing.Color.Black;
            this.lblMaxWaveTitle.Name = "lblMaxWaveTitle";
            this.lblMaxWaveTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblMinWaveTitle
            // 
            this.lblMinWaveTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblMinWaveTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblMinWaveTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMinWaveTitle, "lblMinWaveTitle");
            this.lblMinWaveTitle.ForeColor = System.Drawing.Color.Black;
            this.lblMinWaveTitle.Name = "lblMinWaveTitle";
            this.lblMinWaveTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // labelX11
            // 
            this.labelX11.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX11, "labelX11");
            this.labelX11.ForeColor = System.Drawing.Color.Black;
            this.labelX11.Name = "labelX11";
            this.labelX11.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX12
            // 
            this.labelX12.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX12, "labelX12");
            this.labelX12.ForeColor = System.Drawing.Color.Black;
            this.labelX12.Name = "labelX12";
            this.labelX12.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // chkIsCalcCCTAndCRI
            // 
            this.chkIsCalcCCTAndCRI.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsCalcCCTAndCRI.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsCalcCCTAndCRI.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsCalcCCTAndCRI.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsCalcCCTAndCRI, "chkIsCalcCCTAndCRI");
            this.chkIsCalcCCTAndCRI.Name = "chkIsCalcCCTAndCRI";
            this.chkIsCalcCCTAndCRI.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsCalcCCTAndCRI.TabStop = false;
            // 
            // chkIsCalcCCTByCaliCIExy
            // 
            this.chkIsCalcCCTByCaliCIExy.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsCalcCCTByCaliCIExy.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsCalcCCTByCaliCIExy.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsCalcCCTByCaliCIExy.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsCalcCCTByCaliCIExy, "chkIsCalcCCTByCaliCIExy");
            this.chkIsCalcCCTByCaliCIExy.Name = "chkIsCalcCCTByCaliCIExy";
            this.chkIsCalcCCTByCaliCIExy.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsCalcCCTByCaliCIExy.TabStop = false;
            // 
            // cmbCCTcalcType
            // 
            this.cmbCCTcalcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCCTcalcType.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbCCTcalcType, "cmbCCTcalcType");
            this.cmbCCTcalcType.ForeColor = System.Drawing.Color.Black;
            this.cmbCCTcalcType.Name = "cmbCCTcalcType";
            // 
            // lblCCTCalcMode
            // 
            this.lblCCTCalcMode.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblCCTCalcMode.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblCCTCalcMode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCCTCalcMode, "lblCCTCalcMode");
            this.lblCCTCalcMode.ForeColor = System.Drawing.Color.Black;
            this.lblCCTCalcMode.Name = "lblCCTCalcMode";
            this.lblCCTCalcMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblCalcSpecialWLPPlaceUnit
            // 
            this.lblCalcSpecialWLPPlaceUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCalcSpecialWLPPlaceUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblCalcSpecialWLPPlaceUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCalcSpecialWLPPlaceUnit, "lblCalcSpecialWLPPlaceUnit");
            this.lblCalcSpecialWLPPlaceUnit.ForeColor = System.Drawing.Color.Black;
            this.lblCalcSpecialWLPPlaceUnit.Name = "lblCalcSpecialWLPPlaceUnit";
            this.lblCalcSpecialWLPPlaceUnit.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblCalcSpecialWLPPlaceUnit.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // dinCalcSpecialWLPPlace
            // 
            // 
            // 
            // 
            this.dinCalcSpecialWLPPlace.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinCalcSpecialWLPPlace.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinCalcSpecialWLPPlace.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinCalcSpecialWLPPlace.DisplayFormat = "0";
            resources.ApplyResources(this.dinCalcSpecialWLPPlace, "dinCalcSpecialWLPPlace");
            this.dinCalcSpecialWLPPlace.Increment = 1D;
            this.dinCalcSpecialWLPPlace.MaxValue = 100D;
            this.dinCalcSpecialWLPPlace.MinValue = 50D;
            this.dinCalcSpecialWLPPlace.Name = "dinCalcSpecialWLPPlace";
            this.dinCalcSpecialWLPPlace.ShowUpDown = true;
            this.dinCalcSpecialWLPPlace.Value = 90D;
            // 
            // chkIsCalcSpecailWLP
            // 
            this.chkIsCalcSpecailWLP.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsCalcSpecailWLP.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsCalcSpecailWLP.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsCalcSpecailWLP.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsCalcSpecailWLP, "chkIsCalcSpecailWLP");
            this.chkIsCalcSpecailWLP.Name = "chkIsCalcSpecailWLP";
            this.chkIsCalcSpecailWLP.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsCalcSpecailWLP.TabStop = false;
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
            this.lblNosieBase.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblNosieBase.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblNosieBase.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblNosieBase, "lblNosieBase");
            this.lblNosieBase.ForeColor = System.Drawing.Color.Black;
            this.lblNosieBase.Name = "lblNosieBase";
            this.lblNosieBase.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // pnlSDCMSetting
            // 
            this.pnlSDCMSetting.BackColor = System.Drawing.Color.Transparent;
            this.pnlSDCMSetting.Controls.Add(this.lblSDCMSetting);
            this.pnlSDCMSetting.Controls.Add(this.chkIsCalcANSIAAndGB);
            this.pnlSDCMSetting.Controls.Add(this.cmbANSICalcCCT);
            this.pnlSDCMSetting.Controls.Add(this.lblANSICalcCCT);
            this.pnlSDCMSetting.Controls.Add(this.cmbGBCalcCCT);
            this.pnlSDCMSetting.Controls.Add(this.lblGBCalcCCT);
            resources.ApplyResources(this.pnlSDCMSetting, "pnlSDCMSetting");
            this.pnlSDCMSetting.Name = "pnlSDCMSetting";
            // 
            // lblSDCMSetting
            // 
            this.lblSDCMSetting.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblSDCMSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblSDCMSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblSDCMSetting, "lblSDCMSetting");
            this.lblSDCMSetting.ForeColor = System.Drawing.Color.White;
            this.lblSDCMSetting.Name = "lblSDCMSetting";
            this.lblSDCMSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblSDCMSetting.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // chkIsCalcANSIAAndGB
            // 
            this.chkIsCalcANSIAAndGB.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsCalcANSIAAndGB.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsCalcANSIAAndGB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsCalcANSIAAndGB.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsCalcANSIAAndGB, "chkIsCalcANSIAAndGB");
            this.chkIsCalcANSIAAndGB.Name = "chkIsCalcANSIAAndGB";
            this.chkIsCalcANSIAAndGB.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsCalcANSIAAndGB.TabStop = false;
            // 
            // cmbANSICalcCCT
            // 
            this.cmbANSICalcCCT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbANSICalcCCT.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbANSICalcCCT, "cmbANSICalcCCT");
            this.cmbANSICalcCCT.ForeColor = System.Drawing.Color.Black;
            this.cmbANSICalcCCT.Name = "cmbANSICalcCCT";
            // 
            // lblANSICalcCCT
            // 
            this.lblANSICalcCCT.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblANSICalcCCT.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblANSICalcCCT.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblANSICalcCCT, "lblANSICalcCCT");
            this.lblANSICalcCCT.ForeColor = System.Drawing.Color.Black;
            this.lblANSICalcCCT.Name = "lblANSICalcCCT";
            this.lblANSICalcCCT.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // cmbGBCalcCCT
            // 
            this.cmbGBCalcCCT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGBCalcCCT.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbGBCalcCCT, "cmbGBCalcCCT");
            this.cmbGBCalcCCT.ForeColor = System.Drawing.Color.Black;
            this.cmbGBCalcCCT.Name = "cmbGBCalcCCT";
            // 
            // lblGBCalcCCT
            // 
            this.lblGBCalcCCT.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblGBCalcCCT.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblGBCalcCCT.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblGBCalcCCT, "lblGBCalcCCT");
            this.lblGBCalcCCT.ForeColor = System.Drawing.Color.Black;
            this.lblGBCalcCCT.Name = "lblGBCalcCCT";
            this.lblGBCalcCCT.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // pnlPdDetectorConfig
            // 
            this.pnlPdDetectorConfig.Controls.Add(this.lblPDDarkCorrectTitle);
            this.pnlPdDetectorConfig.Controls.Add(this.cmbPDDarkCorrectMode);
            this.pnlPdDetectorConfig.Controls.Add(this.lblPDDetectorConfig);
            resources.ApplyResources(this.pnlPdDetectorConfig, "pnlPdDetectorConfig");
            this.pnlPdDetectorConfig.Name = "pnlPdDetectorConfig";
            // 
            // lblPDDarkCorrectTitle
            // 
            this.lblPDDarkCorrectTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblPDDarkCorrectTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblPDDarkCorrectTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblPDDarkCorrectTitle, "lblPDDarkCorrectTitle");
            this.lblPDDarkCorrectTitle.ForeColor = System.Drawing.Color.Black;
            this.lblPDDarkCorrectTitle.Name = "lblPDDarkCorrectTitle";
            this.lblPDDarkCorrectTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // cmbPDDarkCorrectMode
            // 
            this.cmbPDDarkCorrectMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPDDarkCorrectMode.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbPDDarkCorrectMode, "cmbPDDarkCorrectMode");
            this.cmbPDDarkCorrectMode.ForeColor = System.Drawing.Color.Black;
            this.cmbPDDarkCorrectMode.Name = "cmbPDDarkCorrectMode";
            // 
            // lblPDDetectorConfig
            // 
            this.lblPDDetectorConfig.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblPDDetectorConfig.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblPDDetectorConfig.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblPDDetectorConfig, "lblPDDetectorConfig");
            this.lblPDDetectorConfig.ForeColor = System.Drawing.Color.White;
            this.lblPDDetectorConfig.Name = "lblPDDetectorConfig";
            this.lblPDDetectorConfig.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblPDDetectorConfig.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // chkIsEnableFloatReport
            // 
            this.chkIsEnableFloatReport.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsEnableFloatReport.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsEnableFloatReport.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableFloatReport.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsEnableFloatReport, "chkIsEnableFloatReport");
            this.chkIsEnableFloatReport.Name = "chkIsEnableFloatReport";
            this.chkIsEnableFloatReport.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableFloatReport.TabStop = false;
            // 
            // numDefaultBinGrade
            // 
            this.numDefaultBinGrade.AntiAlias = true;
            // 
            // 
            // 
            this.numDefaultBinGrade.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numDefaultBinGrade.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numDefaultBinGrade.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numDefaultBinGrade, "numDefaultBinGrade");
            this.numDefaultBinGrade.MaxValue = 1000;
            this.numDefaultBinGrade.MinValue = -1;
            this.numDefaultBinGrade.Name = "numDefaultBinGrade";
            this.numDefaultBinGrade.ShowUpDown = true;
            this.numDefaultBinGrade.Value = 4;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX1, "labelX1");
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Name = "labelX1";
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // chkEnableSettingDefaultBin
            // 
            this.chkEnableSettingDefaultBin.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkEnableSettingDefaultBin.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkEnableSettingDefaultBin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableSettingDefaultBin.CheckSignSize = new System.Drawing.Size(16, 16);
            resources.ApplyResources(this.chkEnableSettingDefaultBin, "chkEnableSettingDefaultBin");
            this.chkEnableSettingDefaultBin.Name = "chkEnableSettingDefaultBin";
            this.chkEnableSettingDefaultBin.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableSettingDefaultBin.TabStop = false;
            // 
            // pnlSptMeterAdvSetting
            // 
            this.pnlSptMeterAdvSetting.BackColor = System.Drawing.Color.Transparent;
            this.pnlSptMeterAdvSetting.Controls.Add(this.chkIsCorrectBackLight);
            this.pnlSptMeterAdvSetting.Controls.Add(this.chkIsNonlinearityCorrect);
            this.pnlSptMeterAdvSetting.Controls.Add(this.chkIsSaveDarkSpectrum);
            this.pnlSptMeterAdvSetting.Controls.Add(this.lblSptMeterOpModeTitle);
            this.pnlSptMeterAdvSetting.Controls.Add(this.cmbSptMeterOpMode);
            resources.ApplyResources(this.pnlSptMeterAdvSetting, "pnlSptMeterAdvSetting");
            this.pnlSptMeterAdvSetting.Name = "pnlSptMeterAdvSetting";
            // 
            // chkIsAdjacentError
            // 
            this.chkIsAdjacentError.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsAdjacentError.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsAdjacentError.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsAdjacentError.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsAdjacentError, "chkIsAdjacentError");
            this.chkIsAdjacentError.Name = "chkIsAdjacentError";
            this.chkIsAdjacentError.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsAdjacentError.TabStop = false;
            // 
            // chkBinSortingIncludeMinMax
            // 
            this.chkBinSortingIncludeMinMax.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkBinSortingIncludeMinMax.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkBinSortingIncludeMinMax.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkBinSortingIncludeMinMax.CheckSignSize = new System.Drawing.Size(16, 16);
            resources.ApplyResources(this.chkBinSortingIncludeMinMax, "chkBinSortingIncludeMinMax");
            this.chkBinSortingIncludeMinMax.Name = "chkBinSortingIncludeMinMax";
            this.chkBinSortingIncludeMinMax.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkBinSortingIncludeMinMax.TabStop = false;
            // 
            // lblBinSortingRule
            // 
            this.lblBinSortingRule.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblBinSortingRule.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblBinSortingRule.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblBinSortingRule, "lblBinSortingRule");
            this.lblBinSortingRule.ForeColor = System.Drawing.Color.White;
            this.lblBinSortingRule.Name = "lblBinSortingRule";
            this.lblBinSortingRule.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblBinSortingRule.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // cmbBinSortingRule
            // 
            this.cmbBinSortingRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBinSortingRule.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbBinSortingRule, "cmbBinSortingRule");
            this.cmbBinSortingRule.ForeColor = System.Drawing.Color.Black;
            this.cmbBinSortingRule.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3,
            this.comboItem7,
            this.comboItem8});
            this.cmbBinSortingRule.Name = "cmbBinSortingRule";
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
            // comboItem7
            // 
            resources.ApplyResources(this.comboItem7, "comboItem7");
            // 
            // comboItem8
            // 
            resources.ApplyResources(this.comboItem8, "comboItem8");
            // 
            // lblBinSorting
            // 
            this.lblBinSorting.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblBinSorting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblBinSorting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblBinSorting, "lblBinSorting");
            this.lblBinSorting.ForeColor = System.Drawing.Color.Black;
            this.lblBinSorting.Name = "lblBinSorting";
            this.lblBinSorting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblSingleTestSetting
            // 
            this.lblSingleTestSetting.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblSingleTestSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblSingleTestSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblSingleTestSetting, "lblSingleTestSetting");
            this.lblSingleTestSetting.ForeColor = System.Drawing.Color.Black;
            this.lblSingleTestSetting.Name = "lblSingleTestSetting";
            this.lblSingleTestSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblCriterionSetting
            // 
            this.lblCriterionSetting.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblCriterionSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblCriterionSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCriterionSetting, "lblCriterionSetting");
            this.lblCriterionSetting.ForeColor = System.Drawing.Color.White;
            this.lblCriterionSetting.Name = "lblCriterionSetting";
            this.lblCriterionSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblCriterionSetting.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // chkIsCountPassFaiByBinGrade
            // 
            this.chkIsCountPassFaiByBinGrade.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsCountPassFaiByBinGrade.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsCountPassFaiByBinGrade.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsCountPassFaiByBinGrade.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsCountPassFaiByBinGrade, "chkIsCountPassFaiByBinGrade");
            this.chkIsCountPassFaiByBinGrade.Name = "chkIsCountPassFaiByBinGrade";
            this.chkIsCountPassFaiByBinGrade.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsCountPassFaiByBinGrade.TabStop = false;
            // 
            // chkIsCheckSpec2
            // 
            this.chkIsCheckSpec2.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsCheckSpec2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsCheckSpec2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsCheckSpec2.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsCheckSpec2, "chkIsCheckSpec2");
            this.chkIsCheckSpec2.Name = "chkIsCheckSpec2";
            this.chkIsCheckSpec2.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsCheckSpec2.TabStop = false;
            // 
            // chkIsCheckRowCol
            // 
            this.chkIsCheckRowCol.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsCheckRowCol.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsCheckRowCol.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsCheckRowCol.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkIsCheckRowCol, "chkIsCheckRowCol");
            this.chkIsCheckRowCol.Name = "chkIsCheckRowCol";
            this.chkIsCheckRowCol.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsCheckRowCol.TabStop = false;
            // 
            // plSptAdvDeviceSetting
            // 
            this.plSptAdvDeviceSetting.BackColor = System.Drawing.Color.Transparent;
            this.plSptAdvDeviceSetting.Controls.Add(this.chkEnableSptModifyXCoeff);
            this.plSptAdvDeviceSetting.Controls.Add(this.chkEnableSptModifyYweigth);
            this.plSptAdvDeviceSetting.Controls.Add(this.chkEnableSptModifyYCoeff);
            this.plSptAdvDeviceSetting.Controls.Add(this.chkIsFullyAutomatic);
            resources.ApplyResources(this.plSptAdvDeviceSetting, "plSptAdvDeviceSetting");
            this.plSptAdvDeviceSetting.Name = "plSptAdvDeviceSetting";
            // 
            // chkEnableSptModifyXCoeff
            // 
            this.chkEnableSptModifyXCoeff.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkEnableSptModifyXCoeff.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkEnableSptModifyXCoeff.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableSptModifyXCoeff.CheckSignSize = new System.Drawing.Size(16, 16);
            resources.ApplyResources(this.chkEnableSptModifyXCoeff, "chkEnableSptModifyXCoeff");
            this.chkEnableSptModifyXCoeff.Name = "chkEnableSptModifyXCoeff";
            this.chkEnableSptModifyXCoeff.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableSptModifyXCoeff.TabStop = false;
            // 
            // chkEnableSptModifyYweigth
            // 
            this.chkEnableSptModifyYweigth.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkEnableSptModifyYweigth.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkEnableSptModifyYweigth.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableSptModifyYweigth.CheckSignSize = new System.Drawing.Size(16, 16);
            resources.ApplyResources(this.chkEnableSptModifyYweigth, "chkEnableSptModifyYweigth");
            this.chkEnableSptModifyYweigth.Name = "chkEnableSptModifyYweigth";
            this.chkEnableSptModifyYweigth.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableSptModifyYweigth.TabStop = false;
            // 
            // chkEnableSptModifyYCoeff
            // 
            this.chkEnableSptModifyYCoeff.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkEnableSptModifyYCoeff.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkEnableSptModifyYCoeff.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnableSptModifyYCoeff.CheckSignSize = new System.Drawing.Size(16, 16);
            resources.ApplyResources(this.chkEnableSptModifyYCoeff, "chkEnableSptModifyYCoeff");
            this.chkEnableSptModifyYCoeff.Name = "chkEnableSptModifyYCoeff";
            this.chkEnableSptModifyYCoeff.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnableSptModifyYCoeff.TabStop = false;
            // 
            // chkIsFullyAutomatic
            // 
            this.chkIsFullyAutomatic.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.chkIsFullyAutomatic.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkIsFullyAutomatic.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsFullyAutomatic.CheckSignSize = new System.Drawing.Size(16, 16);
            resources.ApplyResources(this.chkIsFullyAutomatic, "chkIsFullyAutomatic");
            this.chkIsFullyAutomatic.Name = "chkIsFullyAutomatic";
            this.chkIsFullyAutomatic.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsFullyAutomatic.TabStop = false;
            // 
            // lblLimitStartTimeUnit
            // 
            this.lblLimitStartTimeUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblLimitStartTimeUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblLimitStartTimeUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLimitStartTimeUnit.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.lblLimitStartTimeUnit, "lblLimitStartTimeUnit");
            this.lblLimitStartTimeUnit.Name = "lblLimitStartTimeUnit";
            // 
            // lblSpectrumOpModeTitle
            // 
            this.lblSpectrumOpModeTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblSpectrumOpModeTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblSpectrumOpModeTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSpectrumOpModeTitle.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.lblSpectrumOpModeTitle, "lblSpectrumOpModeTitle");
            this.lblSpectrumOpModeTitle.Name = "lblSpectrumOpModeTitle";
            this.lblSpectrumOpModeTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblLimitStartTimeTitle
            // 
            this.lblLimitStartTimeTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblLimitStartTimeTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblLimitStartTimeTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLimitStartTimeTitle.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.lblLimitStartTimeTitle, "lblLimitStartTimeTitle");
            this.lblLimitStartTimeTitle.Name = "lblLimitStartTimeTitle";
            this.lblLimitStartTimeTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblSpectrometerSetting
            // 
            this.lblSpectrometerSetting.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblSpectrometerSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblSpectrometerSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSpectrometerSetting.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.lblSpectrometerSetting, "lblSpectrometerSetting");
            this.lblSpectrometerSetting.Name = "lblSpectrometerSetting";
            this.lblSpectrometerSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblSpectrometerSetting.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblCoordinateGroup
            // 
            this.lblCoordinateGroup.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblCoordinateGroup.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblCoordinateGroup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblCoordinateGroup, "lblCoordinateGroup");
            this.lblCoordinateGroup.ForeColor = System.Drawing.Color.White;
            this.lblCoordinateGroup.Name = "lblCoordinateGroup";
            this.lblCoordinateGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblCoordinateGroup.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblTesterCoordSetUnit
            // 
            this.lblTesterCoordSetUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblTesterCoordSetUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblTesterCoordSetUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblTesterCoordSetUnit, "lblTesterCoordSetUnit");
            this.lblTesterCoordSetUnit.ForeColor = System.Drawing.Color.Black;
            this.lblTesterCoordSetUnit.Name = "lblTesterCoordSetUnit";
            this.lblTesterCoordSetUnit.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblProberCoordSetUnit
            // 
            this.lblProberCoordSetUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblProberCoordSetUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblProberCoordSetUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblProberCoordSetUnit, "lblProberCoordSetUnit");
            this.lblProberCoordSetUnit.ForeColor = System.Drawing.Color.Black;
            this.lblProberCoordSetUnit.Name = "lblProberCoordSetUnit";
            this.lblProberCoordSetUnit.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblProberCoordSetTitle
            // 
            this.lblProberCoordSetTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblProberCoordSetTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblProberCoordSetTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblProberCoordSetTitle, "lblProberCoordSetTitle");
            this.lblProberCoordSetTitle.ForeColor = System.Drawing.Color.Black;
            this.lblProberCoordSetTitle.Name = "lblProberCoordSetTitle";
            this.lblProberCoordSetTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblTesterCoordSetTitle
            // 
            this.lblTesterCoordSetTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblTesterCoordSetTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblTesterCoordSetTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblTesterCoordSetTitle, "lblTesterCoordSetTitle");
            this.lblTesterCoordSetTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTesterCoordSetTitle.Name = "lblTesterCoordSetTitle";
            this.lblTesterCoordSetTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // numProberCoordSet
            // 
            this.numProberCoordSet.AntiAlias = true;
            // 
            // 
            // 
            this.numProberCoordSet.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numProberCoordSet.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numProberCoordSet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numProberCoordSet, "numProberCoordSet");
            this.numProberCoordSet.MaxValue = 4;
            this.numProberCoordSet.MinValue = 1;
            this.numProberCoordSet.Name = "numProberCoordSet";
            this.numProberCoordSet.ShowUpDown = true;
            this.numProberCoordSet.Value = 3;
            // 
            // numTesterCoordSet
            // 
            this.numTesterCoordSet.AntiAlias = true;
            // 
            // 
            // 
            this.numTesterCoordSet.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numTesterCoordSet.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numTesterCoordSet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numTesterCoordSet, "numTesterCoordSet");
            this.numTesterCoordSet.MaxValue = 4;
            this.numTesterCoordSet.MinValue = 1;
            this.numTesterCoordSet.Name = "numTesterCoordSet";
            this.numTesterCoordSet.ShowUpDown = true;
            this.numTesterCoordSet.Value = 4;
            // 
            // lblBasicSysParamSetting
            // 
            this.lblBasicSysParamSetting.BackColor = System.Drawing.Color.RoyalBlue;
            // 
            // 
            // 
            this.lblBasicSysParamSetting.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblBasicSysParamSetting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblBasicSysParamSetting, "lblBasicSysParamSetting");
            this.lblBasicSysParamSetting.ForeColor = System.Drawing.Color.White;
            this.lblBasicSysParamSetting.Name = "lblBasicSysParamSetting";
            this.lblBasicSysParamSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblBasicSysParamSetting.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblRepeatDelayTitle
            // 
            this.lblRepeatDelayTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblRepeatDelayTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblRepeatDelayTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblRepeatDelayTitle, "lblRepeatDelayTitle");
            this.lblRepeatDelayTitle.ForeColor = System.Drawing.Color.Black;
            this.lblRepeatDelayTitle.Name = "lblRepeatDelayTitle";
            this.lblRepeatDelayTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblRepeatCountTitle
            // 
            this.lblRepeatCountTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblRepeatCountTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblRepeatCountTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblRepeatCountTitle, "lblRepeatCountTitle");
            this.lblRepeatCountTitle.ForeColor = System.Drawing.Color.Black;
            this.lblRepeatCountTitle.Name = "lblRepeatCountTitle";
            this.lblRepeatCountTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblRepeatDelayUnit
            // 
            this.lblRepeatDelayUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblRepeatDelayUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblRepeatDelayUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblRepeatDelayUnit, "lblRepeatDelayUnit");
            this.lblRepeatDelayUnit.ForeColor = System.Drawing.Color.Black;
            this.lblRepeatDelayUnit.Name = "lblRepeatDelayUnit";
            this.lblRepeatDelayUnit.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblRepeatCountUnit
            // 
            this.lblRepeatCountUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblRepeatCountUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblRepeatCountUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblRepeatCountUnit, "lblRepeatCountUnit");
            this.lblRepeatCountUnit.ForeColor = System.Drawing.Color.Black;
            this.lblRepeatCountUnit.Name = "lblRepeatCountUnit";
            this.lblRepeatCountUnit.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
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
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFAEFunc
            // 
            this.btnFAEFunc.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnFAEFunc.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnFAEFunc, "btnFAEFunc");
            this.btnFAEFunc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFAEFunc.Image = global::MPI.Tester.Gui.Properties.Resources.btnPowerTool_32;
            this.btnFAEFunc.Name = "btnFAEFunc";
            this.btnFAEFunc.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnFAEFunc.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnFAEFunc.Click += new System.EventHandler(this.btnFAEFunc_Click);
            // 
            // btnRDFunc
            // 
            this.btnRDFunc.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRDFunc.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnRDFunc, "btnRDFunc");
            this.btnRDFunc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRDFunc.Image = global::MPI.Tester.Gui.Properties.Resources.btnPowerTool_32;
            this.btnRDFunc.Name = "btnRDFunc";
            this.btnRDFunc.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnRDFunc.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnRDFunc.Click += new System.EventHandler(this.btnRDFunc_Click);
            // 
            // btnESDSysParam
            // 
            this.btnESDSysParam.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnESDSysParam.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnESDSysParam, "btnESDSysParam");
            this.btnESDSysParam.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnESDSysParam.Image = global::MPI.Tester.Gui.Properties.Resources.btnPowerTool_32;
            this.btnESDSysParam.Name = "btnESDSysParam";
            this.btnESDSysParam.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnESDSysParam.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnESDSysParam.Click += new System.EventHandler(this.btnESDSysParam_Click);
            // 
            // frmSetSysParam
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnESDSysParam);
            this.Controls.Add(this.btnRDFunc);
            this.Controls.Add(this.btnFAEFunc);
            this.Controls.Add(this.dinNoiseBase);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetSysParam";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSetSysParam_Load);
            this.VisibleChanged += new System.EventHandler(this.frmSetSysParam_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numBoxCar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinCatchCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndWave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartWave)).EndInit();
            this.dinNoiseBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRepeatCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRepeatDelay)).EndInit();
            this.pnlCalSetting.ResumeLayout(false);
            this.plSptCustomizing.ResumeLayout(false);
            this.pnlLimitModeSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinLimitStartTimeFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinLimit02TurnOffTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intLimit02MinSTTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinLimit02PeakPercent)).EndInit();
            this.pnlSptDevParamSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinCalcSpecialWLPPlace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinBaseNosie)).EndInit();
            this.pnlSDCMSetting.ResumeLayout(false);
            this.pnlPdDetectorConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultBinGrade)).EndInit();
            this.pnlSptMeterAdvSetting.ResumeLayout(false);
            this.plSptAdvDeviceSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numProberCoordSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTesterCoordSet)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevComponents.Editors.IntegerInput numMinCatchCount;
		private DevComponents.DotNetBar.LabelX lblMinCatchCountTitle;
		private DevComponents.Editors.IntegerInput numEndWave;
		private DevComponents.Editors.IntegerInput numStartWave;
		private DevComponents.DotNetBar.Controls.GroupPanel dinNoiseBase;
		private DevComponents.DotNetBar.LabelX lblMinWaveTitle;
		private DevComponents.DotNetBar.LabelX lblMaxWaveTitle;
		private DevComponents.DotNetBar.LabelX lblSptMeterOpModeTitle;
        private DevComponents.DotNetBar.LabelX lblSpectrometerGroup;
		private DevComponents.DotNetBar.LabelX lblLimitStartTimeUnit;
		private DevComponents.DotNetBar.LabelX lblSpectrumOpModeTitle;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSptMeterFilterMode;
		private DevComponents.DotNetBar.LabelX lblLimitStartTimeTitle;
		private DevComponents.DotNetBar.LabelX lblSpectrometerSetting;
		private DevComponents.DotNetBar.LabelX lblCoordinateGroup;
		private DevComponents.DotNetBar.LabelX lblTesterCoordSetUnit;
		private DevComponents.DotNetBar.LabelX lblProberCoordSetUnit;
		private DevComponents.DotNetBar.LabelX lblProberCoordSetTitle;
		private DevComponents.DotNetBar.LabelX lblTesterCoordSetTitle;
		private DevComponents.Editors.IntegerInput numProberCoordSet;
		private DevComponents.Editors.IntegerInput numTesterCoordSet;
		private DevComponents.DotNetBar.LabelX lblBasicSysParamSetting;
		private DevComponents.DotNetBar.LabelX lblRepeatDelayTitle;
		private DevComponents.DotNetBar.LabelX lblRepeatCountTitle;
		private DevComponents.DotNetBar.LabelX lblRepeatDelayUnit;
		private DevComponents.DotNetBar.LabelX lblRepeatCountUnit;
		private DevComponents.Editors.IntegerInput numRepeatCount;
		private DevComponents.Editors.IntegerInput numRepeatDelay;
		private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsFullyAutomatic;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsCorrectBackLight;
		private DevComponents.DotNetBar.LabelX labelX12;
		private DevComponents.DotNetBar.LabelX labelX11;
		private DevComponents.DotNetBar.LabelX lblBoxCarTitle;
        private DevComponents.Editors.IntegerInput numBoxCar;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSptMeterOpMode;
		private DevComponents.DotNetBar.LabelX lblSptMeterFilterModeTitle;
		private DevComponents.DotNetBar.LabelX lblDarkCorrectTitle;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbDarkCorrectMode;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsCalcCCTAndCRI;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableSptModifyXCoeff;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableSptModifyYCoeff;
        private System.Windows.Forms.Panel plSptAdvDeviceSetting;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableSptModifyYweigth;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsNonlinearityCorrect;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsCheckRowCol;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsCheckSpec2;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsSaveDarkSpectrum;
        private DevComponents.DotNetBar.LabelX lblCriterionSetting;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsCountPassFaiByBinGrade;
        private DevComponents.DotNetBar.LabelX lblSingleTestSetting;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkBinSortingIncludeMinMax;
        private DevComponents.DotNetBar.LabelX lblBinSortingRule;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbBinSortingRule;
        private DevComponents.DotNetBar.LabelX lblBinSorting;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsAdjacentError;
        private System.Windows.Forms.Panel pnlSptMeterAdvSetting;
		private DevComponents.DotNetBar.LabelX labelX13;
        private System.Windows.Forms.Panel plSptCustomizing;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsCalcCCTByCaliCIExy;
		private DevComponents.DotNetBar.ButtonX btnFAEFunc;
        private DevComponents.Editors.IntegerInput numDefaultBinGrade;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnableSettingDefaultBin;
        private DevComponents.DotNetBar.ButtonX btnRDFunc;
        private DevComponents.Editors.DoubleInput dinLimitStartTimeFactor;
        private DevComponents.DotNetBar.LabelX lblLimitStartTimeFactor;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsUseNDFilterRatio;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkCalcBigFactorBeforeSmall;
        private DevComponents.DotNetBar.LabelX lblCCTCalcMode;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCCTcalcType;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.Editors.IntegerInput intLimit02MinSTTime;
        private DevComponents.DotNetBar.LabelX lblLimit02MinSTTime;
		  private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX lblCalibarationSetting;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsCalcSpecailWLP;
        private DevComponents.Editors.DoubleInput dinCalcSpecialWLPPlace;
        private DevComponents.DotNetBar.LabelX lblCalcSpecialWLPPlaceUnit;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.Editors.IntegerInput dinLimit02PeakPercent;
		private DevComponents.DotNetBar.LabelX labelX10;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsCalcANSIAAndGB;
		private DevComponents.DotNetBar.LabelX lblSDCMSetting;
		private DevComponents.DotNetBar.LabelX lblGBCalcCCT;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbGBCalcCCT;
		private DevComponents.DotNetBar.LabelX lblANSICalcCCT;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbANSICalcCCT;
        private DevComponents.Editors.DoubleInput dinBaseNosie;
        private DevComponents.DotNetBar.LabelX lblNosieBase;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableFloatReport;
        private DevComponents.DotNetBar.LabelX labelX17;
        private DevComponents.Editors.IntegerInput dinLimit02TurnOffTime;
        private DevComponents.DotNetBar.LabelX labelX16;
        private DevComponents.DotNetBar.ButtonX btnESDSysParam;
		private System.Windows.Forms.Panel pnlPdDetectorConfig;
		private DevComponents.DotNetBar.LabelX lblPDDarkCorrectTitle;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbPDDarkCorrectMode;
		private DevComponents.DotNetBar.LabelX lblPDDetectorConfig;
        private System.Windows.Forms.Panel pnlSDCMSetting;
        private System.Windows.Forms.Panel pnlSptDevParamSetting;
        private System.Windows.Forms.Panel pnlLimitModeSetting;
        private System.Windows.Forms.Panel pnlCalSetting;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbGroupBinSortingRule;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.Editors.ComboItem comboItem6;
        private DevComponents.DotNetBar.LabelX lblGroupBinSorting;
        private DevComponents.Editors.ComboItem comboItem7;
        private DevComponents.Editors.ComboItem comboItem8;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSyncSpecAndBin;

	}
}