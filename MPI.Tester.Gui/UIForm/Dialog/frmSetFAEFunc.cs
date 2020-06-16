using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
	public partial class frmSetFAEFunc : System.Windows.Forms.Form
	{
		public frmSetFAEFunc()
		{
			InitializeComponent();

			this.InitUIComponent();
		}

		private void InitUIComponent()
		{
			///////////////////////////////////////////////////////////////////////////////////////////
			// Parameter Setting
			///////////////////////////////////////////////////////////////////////////////////////////
			this.cmbCalibrationUIMode.Items.Clear();

			this.cmbCalibrationUIMode.Items.AddRange(Enum.GetNames(typeof(ECalibrationUIMode)));

            this.cmbCalibrationUIMode.SelectedItem = DataCenter._uiSetting.CalibrationUIMode.ToString();

            this.cmbSrcTurnOffType.Items.Clear();

            this.cmbSrcTurnOffType.Items.AddRange(Enum.GetNames(typeof(ESrcTurnOffType)));

			this.cmbSrcTurnOffType.SelectedItem = ((ESrcTurnOffType)DataCenter._rdFunc.RDFuncData.SrcTurnOffType).ToString();

			this.chkIsEnableErrStateReTest.Checked = DataCenter._sysSetting.IsEnableErrStateReTest;

			this.chkIsEnableSaveDetailLog.Checked = DataCenter._sysSetting.IsEnableSaveDetailLog;

			this.numCoefStartWL.Value = (int)DataCenter._sysSetting.CoefTableStartWL;

			this.numCoefEndWL.Value = (int)DataCenter._sysSetting.CoefTableEndWL;

			this.numCoefResolution.Value = (int)DataCenter._sysSetting.CoefTableResolution;

			this.chkIsReturnPolar.Checked = !DataCenter._machineConfig.IsFastPolar;

			this.chkIsExtendResultItem.Checked = DataCenter._uiSetting.IsExtResultItem;

			this.chkIsEnableSaveErrMsrt.Checked = DataCenter._uiSetting.IsEnableSaveErrMsrt;

            this.chkEnableOutputPathByRecipe.Checked = DataCenter._uiSetting.IsTestResultPathByTaskSheet;

			this.chkIsWriteStatisticsDataToXmlHead.Checked = DataCenter._uiSetting.IsWriteStatisticsDataToXmlHead;

			this.chkTCPIPSendEnableResultItem.Checked = DataCenter._uiSetting.IsTCPIPSendEnableResultItem;

			this.chkIsShowNPLCAndSGFilterSetting.Checked = DataCenter._uiSetting.IsShowNPLCAndSGFilterSetting;		

            this.chkIsWriteReportDirectly.Checked = DataCenter._uiSetting.IsWriteReportDirectly;

            this.cmbSrcSensingMode.Items.Clear();

            this.cmbSrcSensingMode.Items.AddRange(Enum.GetNames(typeof(ESrcSensingMode)));

            this.cmbSrcSensingMode.SelectedItem = ((ESrcSensingMode)DataCenter._machineConfig.SrcSensingMode).ToString();

			///////////////////////////////////////////////////////////////////////////////////////////
			// Spectrometer Setting
			///////////////////////////////////////////////////////////////////////////////////////////
			this.dinBaseNosie.Value = DataCenter._sysSetting.OptiDevSetting.BaseNoise;

			this.cmbCieObserver.Items.Clear();

			this.cmbCieObserver.Items.AddRange(Enum.GetNames(typeof(ECIEObserver)));

			this.cmbCieObserver.SelectedIndex = (int)DataCenter._sysSetting.OptiDevSetting.CieObserver;

			this.cmbCieIlluminant.Items.Clear();

			this.cmbCieIlluminant.Items.AddRange(Enum.GetNames(typeof(ECIEilluminant)));

			this.cmbCieIlluminant.SelectedIndex = (int)DataCenter._sysSetting.OptiDevSetting.CieIlluminant;

			this.numLimitLowCount.Value = (int)DataCenter._sysSetting.OptiDevSetting.LimitLowCount;

			this.numLimitHighCount.Value = (int)DataCenter._sysSetting.OptiDevSetting.LimitHighCount;

			this.numScanAverage.Value = (int)DataCenter._sysSetting.OptiDevSetting.ScanAverage;

			this.dinVlamdaType.Value = DataCenter._sysSetting.OptiDevSetting.VLamdaType;			

			this.chkIsEnableAbsCorrect.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseAbsCorrection;

            // Contact Check

            this.chkEnableContactCheck.Checked = DataCenter._sysSetting.contactCheckCFG._isEnableContactCheck;

            this.dinContactApplyValue.Value = DataCenter._sysSetting.contactCheckCFG._contactApplyCurrentValue;

            this.dinContactApplyTime.Value = DataCenter._sysSetting.contactCheckCFG._contactApplyForceTime;

            this.dinContactHighLimit.Value = DataCenter._sysSetting.contactCheckCFG._contactSpecMax;

            this.dinContactLowLimit.Value = DataCenter._sysSetting.contactCheckCFG._contactSpecMin;

            this.chkDisableCheckAtPosXZero.Checked = DataCenter._sysSetting.contactCheckCFG._isDisableCheckAtPosX;

            this.chkEableVzRandomValue.Checked = DataCenter._sysSetting.contactCheckCFG._isEnableVzFillRandomValue;

            // Move to Sysyem Setting Func

            //this.chkIsEnableDataRecovery.Checked = DataCenter._uiSetting.IsEnableDataRecovery;

            //this.chkCalcBigFactorBeforeSmall.Checked = DataCenter._sysSetting.IsEnalbeCalcBigFactorBeforeSmall;

            //this.chkIsUseNDFilterRatio.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseNDFilterRatio;

            //this.cmbCCTcalcType.Items.Clear();

            //this.cmbCCTcalcType.Items.AddRange(Enum.GetNames(typeof(ECCTCaculationType)));

            //this.cmbCCTcalcType.SelectedItem = DataCenter._sysSetting.CCTcaculationType.ToString();

            //this.intLimit02MinSTTime.Value = DataCenter._sysSetting.OptiDevSetting.Limit02MinSTTime;

           // this.dinLimitStartTimeFactor.Value = DataCenter._sysSetting.OptiDevSetting.LimitStartTime / 6.0d;

            this.chkIsUseMPISpam2.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseMPISpam2;

            this.chkEnableSrcMeterFirmwareCalcTHY.Checked = DataCenter._machineConfig.Enable.IsEnableSrcFirmwareCalcTHY;

            this.chkEnableForceMode.Checked = DataCenter._rdFunc.RDFuncData.IsEnableForceMode;

            this.chkEnableTakeFirstItemAsOSCheck.Checked = DataCenter._sysSetting.IsTakeFirstItemAsOpenShort;

			this.chkEnableK26ReturnDefaultRange.Checked = DataCenter._rdFunc.RDFuncData.IsTurnOffRangeIBackToDefault;

			this.chkIsEnableSettingReverseRange.Checked = DataCenter._rdFunc.RDFuncData.IsSettingReverseCurrentRange;

			this.dinForceReverseCurrentRange.Value = DataCenter._rdFunc.RDFuncData.ReverseCurrentApplyRange;

            this.chkIsShowTesterChannelConfig.Checked = DataCenter._rdFunc.RDFuncData.IsShowTesterChannelConfig;

            this.chkIsShowContinousProbing.Checked = DataCenter._rdFunc.RDFuncData.IsShowContinousProbing;

            this.chkIsEnableMsrtForceValue.Checked = DataCenter._sysSetting.IsEnableSrcMeterMsrtForceValue;

            this.chkIsEnableReTestMode.Checked = DataCenter._sysSetting.IsEnableRetestTestItem;

            this.chkIsEnableSaveOsaRawData.Checked = DataCenter._sysSetting.OsaDevSetting.IsSaveRawData;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.CheckIsReStartSystem, "System Setting , Please Restart the application？", "Close", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
			
			if (result != DialogResult.OK)
			{
				return;
			}

			///////////////////////////////////////////////////////////////////////////////////////////
			// Parameter Setting
			///////////////////////////////////////////////////////////////////////////////////////////
            DataCenter._uiSetting.CalibrationUIMode = (ECalibrationUIMode)Enum.Parse(typeof(ECalibrationUIMode), this.cmbCalibrationUIMode.SelectedItem.ToString(), true);

			DataCenter._rdFunc.RDFuncData.SrcTurnOffType = (int)Enum.Parse(typeof(ESrcTurnOffType), this.cmbSrcTurnOffType.SelectedItem.ToString(), true);
			
			DataCenter._sysSetting.IsEnableErrStateReTest = this.chkIsEnableErrStateReTest.Checked;

			DataCenter._sysSetting.IsEnableSaveDetailLog = this.chkIsEnableSaveDetailLog.Checked;

			DataCenter._sysSetting.CoefTableStartWL = this.numCoefStartWL.Value;

			DataCenter._sysSetting.CoefTableEndWL = this.numCoefEndWL.Value;

			DataCenter._sysSetting.CoefTableResolution = this.numCoefResolution.Value;

			DataCenter._machineConfig.IsFastPolar = !this.chkIsReturnPolar.Checked;

			DataCenter._uiSetting.IsExtResultItem = this.chkIsExtendResultItem.Checked;

			DataCenter._uiSetting.IsEnableSaveErrMsrt = this.chkIsEnableSaveErrMsrt.Checked;

			DataCenter._uiSetting.IsTestResultPathByTaskSheet = this.chkEnableOutputPathByRecipe.Checked;

			DataCenter._uiSetting.IsWriteStatisticsDataToXmlHead = this.chkIsWriteStatisticsDataToXmlHead.Checked;

			DataCenter._uiSetting.IsTCPIPSendEnableResultItem = this.chkTCPIPSendEnableResultItem.Checked;

			DataCenter._uiSetting.IsShowNPLCAndSGFilterSetting = this.chkIsShowNPLCAndSGFilterSetting.Checked;

            DataCenter._uiSetting.IsWriteReportDirectly = this.chkIsWriteReportDirectly.Checked;

			///////////////////////////////////////////////////////////////////////////////////////////
			// Spectrometer Setting
			///////////////////////////////////////////////////////////////////////////////////////////
			DataCenter._sysSetting.OptiDevSetting.CieObserver = (ECIEObserver)Enum.Parse(typeof(ECIEObserver), this.cmbCieObserver.SelectedItem.ToString(), true);

			DataCenter._sysSetting.OptiDevSetting.CieIlluminant = (ECIEilluminant)Enum.Parse(typeof(ECIEilluminant), this.cmbCieIlluminant.SelectedItem.ToString(), true);

			DataCenter._sysSetting.OptiDevSetting.LimitLowCount = (int)this.numLimitLowCount.Value;

			DataCenter._sysSetting.OptiDevSetting.LimitHighCount = (int)this.numLimitHighCount.Value;

			DataCenter._sysSetting.OptiDevSetting.ScanAverage = (int)numScanAverage.Value;

            DataCenter._sysSetting.OptiDevSetting.VLamdaType = (int)this.dinVlamdaType.Value;

            // Contact Check

            DataCenter._sysSetting.contactCheckCFG._isEnableContactCheck = this.chkEnableContactCheck.Checked;

            DataCenter._sysSetting.contactCheckCFG._contactApplyCurrentValue = this.dinContactApplyValue.Value;

            DataCenter._sysSetting.contactCheckCFG._contactApplyForceTime = this.dinContactApplyTime.Value;

            DataCenter._sysSetting.contactCheckCFG._contactSpecMax = this.dinContactHighLimit.Value;

            DataCenter._sysSetting.contactCheckCFG._contactSpecMin = this.dinContactLowLimit.Value;

            DataCenter._sysSetting.contactCheckCFG._isDisableCheckAtPosX = this.chkDisableCheckAtPosXZero.Checked;

            DataCenter._sysSetting.contactCheckCFG._isEnableVzFillRandomValue = this.chkEableVzRandomValue.Checked;

            // Move to System Setting ; let user to setting

            //DataCenter._sysSetting.OptiDevSetting.LimitStartTime = this.dinLimitStartTimeFactor.Value * 6.0d;

            //DataCenter._uiSetting.IsEnableDataRecovery = this.chkIsEnableDataRecovery.Checked;

            //DataCenter._sysSetting.IsEnalbeCalcBigFactorBeforeSmall = this.chkCalcBigFactorBeforeSmall.Checked;

            //DataCenter._sysSetting.OptiDevSetting.IsUseNDFilterRatio = this.chkIsUseNDFilterRatio.Checked;

            //DataCenter._sysSetting.CCTcaculationType = (ECCTCaculationType)Enum.Parse(typeof(ECCTCaculationType), this.cmbCCTcalcType.SelectedItem.ToString(), true);

            //DataCenter._sysSetting.OptiDevSetting.Limit02MinSTTime = this.intLimit02MinSTTime.Value;

            DataCenter._sysSetting.OptiDevSetting.IsUseMPISpam2 = this.chkIsUseMPISpam2.Checked;

            DataCenter._sysSetting.IsTakeFirstItemAsOpenShort = this.chkEnableTakeFirstItemAsOSCheck.Checked;

			//DataCenter._sysSetting.OptiDevSetting.IsUseAbsCorrection = this.chkIsEnableAbsCorrect.Checked;

            DataCenter._machineConfig.Enable.IsEnableSrcFirmwareCalcTHY = this.chkEnableSrcMeterFirmwareCalcTHY.Checked;

            DataCenter._machineConfig.SrcSensingMode = (ESrcSensingMode)Enum.Parse(typeof(ESrcSensingMode), this.cmbSrcSensingMode.SelectedItem.ToString(), true);

            DataCenter._rdFunc.RDFuncData.IsEnableForceMode = this.chkEnableForceMode.Checked;

			DataCenter._rdFunc.RDFuncData.IsTurnOffRangeIBackToDefault = this.chkEnableK26ReturnDefaultRange.Checked;

			DataCenter._rdFunc.RDFuncData.IsSettingReverseCurrentRange = this.chkIsEnableSettingReverseRange.Checked;

			DataCenter._rdFunc.RDFuncData.ReverseCurrentApplyRange = this.dinForceReverseCurrentRange.Value;

            DataCenter._rdFunc.RDFuncData.IsShowTesterChannelConfig = this.chkIsShowTesterChannelConfig.Checked;

            DataCenter._rdFunc.RDFuncData.IsShowContinousProbing = this.chkIsShowContinousProbing.Checked;

            DataCenter._sysSetting.IsEnableSrcMeterMsrtForceValue = this.chkIsEnableMsrtForceValue.Checked;

            DataCenter._sysSetting.IsEnableRetestTestItem = this.chkIsEnableReTestMode.Checked;

            DataCenter._sysSetting.OsaDevSetting.IsSaveRawData = this.chkIsEnableSaveOsaRawData.Checked;

			DataCenter.Save();

            FormAgent.MainForm.IsClose = true;

            FormAgent.MainForm.Close();
		}

		private void btnReload_Click(object sender, EventArgs e)
		{
			this.InitUIComponent();
		}

        private void frmSetFAEFunc_Load(object sender, EventArgs e)
        {

        }
	}
}
