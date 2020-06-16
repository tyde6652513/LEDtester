using System;
using System.IO;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
	public partial class frmSetSysParam : System.Windows.Forms.Form
	{
		public frmSetSysParam()
		{
			Console.WriteLine("[frmSetSysParam], frmSetSysParam()");

			InitializeComponent();

			this.InitParamAndCompData();
		}

		#region >>> Private Method <<<

		private void InitParamAndCompData()
		{
			//---------------------------------------
			// UI component initialization
			//---------------------------------------
			this.cmbSptMeterOpMode.Items.Clear();
			this.cmbSptMeterOpMode.Items.AddRange(Enum.GetNames(typeof(ESpectrometerOpMode)));
			this.cmbSptMeterFilterMode.Items.Clear();
			this.cmbSptMeterFilterMode.Items.AddRange(Enum.GetNames(typeof(ESptFilterMode)));
			this.cmbDarkCorrectMode.Items.Clear();
			this.cmbDarkCorrectMode.Items.AddRange(Enum.GetNames(typeof(EDarkCorrectMode)));
            this.cmbPDDarkCorrectMode.Items.Clear();
            this.cmbPDDarkCorrectMode.Items.AddRange(Enum.GetNames(typeof(EPDDarkCorrectMode)));
            this.cmbGroupBinSortingRule.Items.Clear();
            this.cmbGroupBinSortingRule.Items.AddRange(Enum.GetNames(typeof(EGroupBinRule)));
			
#if  ( DebugVer )
			//this.pnlSptMeterAdvanceSetting.Visible = true;
			this.pnlTemp.Visible = true;
			this.chkIsCorrectBackLight.Visible = true;
#else

			this.chkIsCorrectBackLight.Visible = false;
            this.chkIsSaveDarkSpectrum.Visible = false;
            this.chkIsSaveDarkSpectrum.Checked = false;
#endif

		}

		private void ChangeAuthority()
		{
            //this.plSptAdvDeviceSetting.Visible = false;
			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
                    this.btnSave.Enabled = false;
                    this.dinNoiseBase.Enabled = false;
					this.btnFAEFunc.Visible = false;
                    this.btnRDFunc.Visible = false;
					break;
				case EAuthority.Engineer:
					this.btnSave.Enabled = true;
					this.dinNoiseBase.Enabled = true;
					this.btnFAEFunc.Visible = false;
                    this.btnRDFunc.Visible = false;
					break;
                case EAuthority.Admin:
                   this.btnSave.Enabled = true;
                   this.dinNoiseBase.Enabled = true;
				   this.btnFAEFunc.Visible = true;
                   this.btnRDFunc.Visible = false;
                   this.btnESDSysParam.Visible = true;
                   break;
				case EAuthority.Super:
                    this.btnSave.Enabled = true;
                    this.dinNoiseBase.Enabled = true;
                    this.plSptAdvDeviceSetting.Visible = true;
                    this.plSptCustomizing.Visible = true;
					this.btnFAEFunc.Visible = true;
                    this.btnRDFunc.Visible = true;
                    this.btnESDSysParam.Visible = true;
                    break;
				default:
                    this.btnSave.Enabled = false;
                    this.dinNoiseBase.Enabled = false;
					this.btnFAEFunc.Visible = false;
                    this.btnRDFunc.Visible = false;
					break;
			}
		}

		private void UpdateDataToControls()
		{
			if (DataCenter._uiSetting.UIOperateMode == (int)EUIOperateMode.ManulRun)
			{
				this.chkIsFullyAutomatic.Checked = true;
			}
			else
			{
				this.chkIsFullyAutomatic.Checked = false;
			}

            if (DataCenter._uiSetting.AuthorityLevel == EAuthority.Super)
            {
                this.pnlSDCMSetting.Visible = true;
                this.chkIsCalcCCTAndCRI.Visible = true;
                this.chkIsCalcCCTByCaliCIExy.Visible = true;
                this.lblCCTCalcMode.Visible = true;
                this.cmbCCTcalcType.Visible = true;
                this.chkIsCalcSpecailWLP.Visible = true;
                this.dinCalcSpecialWLPPlace.Visible = true;
                this.lblCalcSpecialWLPPlaceUnit.Visible = true;
                this.pnlSptDevParamSetting.Visible = true;
                this.pnlPdDetectorConfig.Visible = true;
      
                this.pnlLimitModeSetting.Visible = true;
                this.pnlCalSetting.Visible = true;
            }
            else
            {
                switch (DataCenter._rdFunc.RDFuncData.TesterConfigType)
                {
                    case ETesterConfigType.LDTester:
                        {
                            this.pnlSDCMSetting.Visible = false;
                            this.chkIsCalcCCTAndCRI.Visible = false;
                            this.chkIsCalcCCTByCaliCIExy.Visible = false;
                            this.lblCCTCalcMode.Visible = false;
                            this.cmbCCTcalcType.Visible = false;

                            this.chkIsCalcSpecailWLP.Visible = false;
                            this.dinCalcSpecialWLPPlace.Visible = false;
                            this.lblCalcSpecialWLPPlaceUnit.Visible = false;
                            this.pnlCalSetting.Visible = false;
                            break;
                        }
                    case ETesterConfigType.PDTester:
                        {
                            this.pnlSDCMSetting.Visible = false;
                            this.pnlSptDevParamSetting.Visible = false;
                            this.pnlPdDetectorConfig.Visible = false;
							this.pnlCalSetting.Visible = false;
                            this.pnlLimitModeSetting.Visible = false;
                            this.pnlCalSetting.Visible = false;
                            break;
                        }
                }
            }


			this.chkIsCheckRowCol.Checked = DataCenter._sysSetting.IsCheckRowCol;
			this.numRepeatCount.Value = (int)DataCenter._sysSetting.DieRepeatTestCount;
			this.numRepeatDelay.Value = (int)DataCenter._sysSetting.DieRepeatTestDelay;

			this.numTesterCoordSet.Value = (int)DataCenter._sysSetting.TesterCoord;
			this.numProberCoordSet.Value = (int)DataCenter._sysSetting.ProberCoord;

			// Spectromter parameters setting
            this.chkIsCorrectBackLight.Checked = DataCenter._sysSetting.OptiDevSetting.IsEnableCorrectForDark;
			this.cmbSptMeterOpMode.SelectedIndex = (int)DataCenter._sysSetting.OptiDevSetting.OperationMode;
			this.cmbSptMeterFilterMode.SelectedIndex = (int)DataCenter._sysSetting.OptiDevSetting.SptFilterMode;
			this.cmbDarkCorrectMode.SelectedIndex = (int)DataCenter._sysSetting.OptiDevSetting.DarkCorrectMode;
            this.cmbPDDarkCorrectMode.SelectedIndex = (int)DataCenter._sysSetting.PDDarkCorrectMode;
			this.numStartWave.Value = (int)DataCenter._sysSetting.OptiDevSetting.StartWavelength;
			this.numEndWave.Value = (int)DataCenter._sysSetting.OptiDevSetting.EndWavelength;
			this.numBoxCar.Value = (int)DataCenter._sysSetting.OptiDevSetting.BoxCar;
			this.numMinCatchCount.Value = (int)DataCenter._sysSetting.OptiDevSetting.MinCatchPeakCount;
            this.chkEnableSptModifyXCoeff.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseProductXaxisCoeff;
            this.chkEnableSptModifyYCoeff.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseProductYaxisCalib;
			this.chkIsCalcCCTAndCRI.Checked = DataCenter._sysSetting.OptiDevSetting.IsCalcCRIData;
            this.cmbGroupBinSortingRule.SelectedIndex = (int)DataCenter._sysSetting.GroupBinRule;

            this.chkEnableSettingDefaultBin.Checked = DataCenter._sysSetting.IsEnableSettingDefaultBinGrade;

            this.numDefaultBinGrade.Value = DataCenter._sysSetting.DefaultBinGrade;

            this.dinLimit02PeakPercent.Value = DataCenter._sysSetting.OptiDevSetting.Limit02PeakPercent;

            if(DataCenter._uiSetting.UserDefinedData.IsEnableCorrectSpectometerParam)
            {
                this.plSptAdvDeviceSetting.Visible = true;
            }
            else
            {
                this.plSptAdvDeviceSetting.Visible = false;
            }


            if (DataCenter._uiSetting.UserDefinedData.IsEnableSptAdvSetting)
            {
                this.plSptCustomizing.Visible = true;
            }
            else
            {
                 this.plSptCustomizing.Visible = false;
            }

            this.chkEnableSptModifyXCoeff.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseProductXaxisCoeff;
            this.chkEnableSptModifyYCoeff.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseProductYaxisCalib;
            this.chkEnableSptModifyYweigth.Checked = DataCenter._sysSetting.OptiDevSetting.isUseProductYaxisWeight;                        
			this.chkIsCalcCCTByCaliCIExy.Checked = DataCenter._sysSetting.OptiDevSetting.IsCalcCCTByCaliCIExy;
            
            this.chkIsSaveDarkSpectrum.Checked = DataCenter._uiSetting.IsEnableSaveDarkSpectrum;     
            // BIN grade setting
            this.chkSyncSpecAndBin.Checked = DataCenter._sysSetting.IsSpecBinTableSync;
            
            this.chkIsCountPassFaiByBinGrade.Checked=DataCenter._sysSetting.IsCountPassFailByBinGrade;
            this.chkBinSortingIncludeMinMax.Checked = DataCenter._sysSetting.IsBinSortingIncludeMinMax;
            this.cmbBinSortingRule.SelectedIndex = (int)DataCenter._sysSetting.BinSortingRule;
            this.chkIsAdjacentError.Checked = DataCenter._sysSetting.IsEnableAdjacentError;
            this.chkIsCheckSpec2.Checked = DataCenter._sysSetting.IsCheckSpec2;

			this.chkIsCalcANSIAAndGB.Checked = DataCenter._sysSetting.IsCalcANSIAndGB;

            //=====================
            //  Controls Loaction in FAE UI Orignally 
            //=====================

			this.chkIsEnableFloatReport.Checked = DataCenter._uiSetting.IsEnableFloatReport;

            this.chkCalcBigFactorBeforeSmall.Checked = DataCenter._sysSetting.IsEnalbeCalcBigFactorBeforeSmall;

            this.chkIsUseNDFilterRatio.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseNDFilterRatio;

            this.cmbCCTcalcType.Items.Clear();

            this.cmbCCTcalcType.Items.AddRange(Enum.GetNames(typeof(ECCTCaculationType)));

            this.cmbCCTcalcType.SelectedItem = DataCenter._sysSetting.CCTcaculationType.ToString();

			this.cmbANSICalcCCT.Items.Clear();

			this.cmbANSICalcCCT.Items.AddRange(Enum.GetNames(typeof(EANSI376)));

			this.cmbANSICalcCCT.SelectedItem = DataCenter._sysSetting.ANSI376.ToString();

			this.cmbGBCalcCCT.Items.Clear();

			this.cmbGBCalcCCT.Items.AddRange(Enum.GetNames(typeof(EGB10682)));

			this.cmbGBCalcCCT.SelectedItem = DataCenter._sysSetting.GB10682.ToString();

            this.intLimit02MinSTTime.Value = DataCenter._sysSetting.OptiDevSetting.Limit02MinSTTime;

            this.dinLimitStartTimeFactor.Value = DataCenter._sysSetting.OptiDevSetting.LimitStartTime / 6.0d;

            // WLP的90%計算方法
            this.chkIsCalcSpecailWLP.Checked = DataCenter._sysSetting.OptiDevSetting.IsCalcSpecialWLP;

            this.dinCalcSpecialWLPPlace.Value = DataCenter._sysSetting.OptiDevSetting.CalcSpecialWLPPlace;

            this.dinBaseNosie.Value = DataCenter._sysSetting.OptiDevSetting.BaseNoise;

            this.dinLimit02TurnOffTime.Value = (int)DataCenter._sysSetting.OptiDevSetting.Limit02TurnOffTime;

            if (DataCenter._machineConfig.ESDModel == EESDModel.ESD_PCA)
            {
                this.btnESDSysParam.Visible = true;
            }
            else 
            {
                this.btnESDSysParam.Visible = false;
            }


            switch (DataCenter._uiSetting.UIOperateMode)
            {
                case (int)EUIOperateMode.Idle:
					this.ChangeAuthority();
                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
                    this.btnSave.Enabled = false;
                    this.dinNoiseBase.Enabled = false;
                    break;
                //-----------------------------------------------------------------------------
                //case (int)EUIOperateMode.ManulRun:
                //    this.ChangeAuthority();
                //    break;
                //-----------------------------------------------------------------------------
                default:
					this.ChangeAuthority();
                    break;
            }
		}

		private void SaveDataToFile()
		{
			DataCenter._sysSetting.IsCheckRowCol = this.chkIsCheckRowCol.Checked;
			DataCenter._sysSetting.DieRepeatTestCount = (uint)this.numRepeatCount.Value;
			DataCenter._sysSetting.DieRepeatTestDelay = (uint)this.numRepeatDelay.Value;

			DataCenter._sysSetting.TesterCoord = this.numTesterCoordSet.Value;
			DataCenter._sysSetting.ProberCoord = this.numProberCoordSet.Value;

            DataCenter._uiSetting.TesterCoord = this.numTesterCoordSet.Value;

			// Spectromter parameters setting
            DataCenter._sysSetting.OptiDevSetting.IsEnableCorrectForDark = this.chkIsCorrectBackLight.Checked;
			DataCenter._sysSetting.OptiDevSetting.StartWavelength = (uint)numStartWave.Value;
			DataCenter._sysSetting.OptiDevSetting.EndWavelength = (uint)numEndWave.Value;
			DataCenter._sysSetting.OptiDevSetting.BoxCar = (int)numBoxCar.Value;			

            DataCenter._sysSetting.IsCheckSpec2 = this.chkIsCheckSpec2.Checked;
			DataCenter._sysSetting.OptiDevSetting.OperationMode = (ESpectrometerOpMode)Enum.Parse(typeof(ESpectrometerOpMode), this.cmbSptMeterOpMode.SelectedItem.ToString(), true);
			DataCenter._sysSetting.OptiDevSetting.SptFilterMode = (ESptFilterMode)Enum.Parse(typeof(ESptFilterMode), this.cmbSptMeterFilterMode.SelectedItem.ToString(), true);
			DataCenter._sysSetting.OptiDevSetting.DarkCorrectMode = (EDarkCorrectMode)Enum.Parse(typeof(EDarkCorrectMode), this.cmbDarkCorrectMode.SelectedItem.ToString(), true);
            DataCenter._sysSetting.PDDarkCorrectMode = (EPDDarkCorrectMode)Enum.Parse(typeof(EPDDarkCorrectMode), this.cmbPDDarkCorrectMode.SelectedItem.ToString(), true);
            DataCenter._sysSetting.OptiDevSetting.IsCorrectForNonlinearity = this.chkIsNonlinearityCorrect.Checked;
            DataCenter._sysSetting.OptiDevSetting.IsCalcCRIData = this.chkIsCalcCCTAndCRI.Checked;
            DataCenter._sysSetting.OptiDevSetting.IsUseProductXaxisCoeff = this.chkEnableSptModifyXCoeff.Checked;
            DataCenter._sysSetting.OptiDevSetting.IsUseProductYaxisCalib = this.chkEnableSptModifyYCoeff.Checked;
            // Save Spectrum Setting
            // Bin Grade Setting
            DataCenter._sysSetting.IsSpecBinTableSync = this.chkSyncSpecAndBin.Checked;
            DataCenter._sysSetting.IsCountPassFailByBinGrade = this.chkIsCountPassFaiByBinGrade.Checked;
            DataCenter._sysSetting.IsBinSortingIncludeMinMax = this.chkBinSortingIncludeMinMax.Checked;
            DataCenter._sysSetting.BinSortingRule = (EBinBoundaryRule)this.cmbBinSortingRule.SelectedIndex;

            if (DataCenter._sysSetting.BinSortingRule != EBinBoundaryRule.Various)
            {
                TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

                foreach (TestItemData item in testItems)
                {
                    if (item.MsrtResult != null)
                    {
                        for (int i = 0; i < item.MsrtResult.Length; i++)
                        {
                            item.MsrtResult[i].BoundaryRule = DataCenter._sysSetting.BinSortingRule;
                        }
                    }
                }
            }
            DataCenter._sysSetting.IsEnableAdjacentError = this.chkIsAdjacentError.Checked;

            DataCenter._sysSetting.GroupBinRule = (EGroupBinRule)Enum.Parse(typeof(EGroupBinRule), this.cmbGroupBinSortingRule.SelectedIndex.ToString(), true);  

			DataCenter._sysSetting.OptiDevSetting.IsCorrectForNonlinearity = true;
			DataCenter._sysSetting.OptiDevSetting.IsUseSphere = true;
			DataCenter._sysSetting.OptiDevSetting.SurfaceAreaCmSquared = 1;
			DataCenter._sysSetting.OptiDevSetting.IsAutoGetDark = true;

            DataCenter._sysSetting.OptiDevSetting.IsUseProductXaxisCoeff = this.chkEnableSptModifyXCoeff.Checked;
            DataCenter._sysSetting.OptiDevSetting.IsUseProductYaxisCalib = this.chkEnableSptModifyYCoeff.Checked;
            DataCenter._sysSetting.OptiDevSetting.isUseProductYaxisWeight = this.chkEnableSptModifyYweigth.Checked;                        
			DataCenter._sysSetting.OptiDevSetting.IsCalcCCTByCaliCIExy = this.chkIsCalcCCTByCaliCIExy.Checked;

            DataCenter._sysSetting.IsEnableSettingDefaultBinGrade = this.chkEnableSettingDefaultBin.Enabled;

			DataCenter._sysSetting.IsCalcANSIAndGB = this.chkIsCalcANSIAAndGB.Checked;

			DataCenter._sysSetting.ANSI376 = (EANSI376)Enum.Parse(typeof(EANSI376), this.cmbANSICalcCCT.SelectedItem.ToString(), true);

			DataCenter._sysSetting.GB10682 = (EGB10682)Enum.Parse(typeof(EGB10682), this.cmbGBCalcCCT.SelectedItem.ToString(), true);

            // 

            DataCenter._sysSetting.DefaultBinGrade = (int)this.numDefaultBinGrade.Value;

            DataCenter._sysSetting.OptiDevSetting.LimitStartTime = this.dinLimitStartTimeFactor.Value * 6.0d;

			DataCenter._uiSetting.IsEnableFloatReport = this.chkIsEnableFloatReport.Checked;

            DataCenter._sysSetting.IsEnalbeCalcBigFactorBeforeSmall = this.chkCalcBigFactorBeforeSmall.Checked;

            DataCenter._sysSetting.OptiDevSetting.IsUseNDFilterRatio = this.chkIsUseNDFilterRatio.Checked;

            DataCenter._sysSetting.CCTcaculationType = (ECCTCaculationType)Enum.Parse(typeof(ECCTCaculationType), this.cmbCCTcalcType.SelectedItem.ToString(), true);

            DataCenter._sysSetting.OptiDevSetting.Limit02MinSTTime = this.intLimit02MinSTTime.Value;

            DataCenter._sysSetting.OptiDevSetting.IsCalcSpecialWLP = this.chkIsCalcSpecailWLP.Checked;

            DataCenter._sysSetting.OptiDevSetting.CalcSpecialWLPPlace = this.dinCalcSpecialWLPPlace.Value;

            DataCenter._sysSetting.OptiDevSetting.Limit02PeakPercent = (int)this.dinLimit02PeakPercent.Value;

            DataCenter._sysSetting.OptiDevSetting.BaseNoise = this.dinBaseNosie.Value;

            DataCenter._sysSetting.OptiDevSetting.Limit02TurnOffTime = (uint)this.dinLimit02TurnOffTime.Value;

            DataCenter._sysSetting.OptiDevSetting.MinCatchPeakCount = this.numMinCatchCount.Value;

			AppSystem.SetDataToSystem();

			DataCenter.Save();

			DataCenter.ModifyCoefTableRange();
		}

        private void ShowBigDataSaveSettingPanel()
        {

        }

		#endregion

		#region >>> UI Event Handler <<<

		private void frmSetSysParam_Load(object sender, EventArgs e)
		{
            this.ShowBigDataSaveSettingPanel();
            
            this.UpdateDataToControls();
		}

		private void frmSetSysParam_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
			   this.UpdateDataToControls();
			}
		}

		private void btnSave_Click( object sender, EventArgs e )
		{
			this.SaveDataToFile();
		}

		

		private void btnFAEFunc_Click(object sender, EventArgs e)
		{
			frmSetFAEFunc frm = new frmSetFAEFunc();

			frm.ShowDialog();

			frm.Close();

			frm.Dispose();
		}

        private void btnRDFunc_Click(object sender, EventArgs e)
        {
            frmSetRDFunc frm = new frmSetRDFunc();

            frm.ShowDialog();

            frm.Close();

            frm.Dispose();
        }

        private void btnESDSysParam_Click(object sender, EventArgs e)
        {
            frmSetESDSysParam frm = new frmSetESDSysParam();

            frm.ShowDialog();

            frm.Close();

            frm.Dispose();
        }

        

        

		#endregion
	}
}