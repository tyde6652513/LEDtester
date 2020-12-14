using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.Gui.UIForm.UIComponent;

namespace MPI.Tester.Gui
{
	public partial class frmSetUISetting : System.Windows.Forms.Form
	{

        Form customerFrm;

        frmSysPathSet sysPathFrm;
		private List<string> _userList = new List<string>();

		public frmSetUISetting()
		{
			Console.WriteLine("[frmSetUISetting], frmSetUISetting()");

			InitializeComponent();

			this.InitParamAndCompData();
		}

		#region >>> Private Method <<<

		private void InitParamAndCompData()
		{
			//---------------------------------------
			// UI component initialization
			//---------------------------------------
			foreach( string userID in DataCenter._uiSetting.UserIDList )
			{
				int userIDInt;

				if (Int32.TryParse(userID, out userIDInt) == false)
					continue;

				if ( Enum.IsDefined( typeof(EUserID), userIDInt) )
				{
#if  ( DebugVer )
					this._userList.Add(Enum.GetName(typeof(EUserID), userIDInt));
#else
					this._userList.Add( userID );
       
#endif
				}
			}
			
			this.cmbUserID.Items.AddRange(this._userList.ToArray() );

			if (DataCenter._uiSetting.UserDefinedData.FormatNames == null)
			{
				this.cmbFormat.Items.Add("NONE");
			}
			else
			{
				this.cmbFormat.Items.AddRange(DataCenter._uiSetting.UserDefinedData.FormatNames);
			}

			this.cmbFileNamePresent.Items.AddRange(Enum.GetNames(typeof(EOutputFileNamePresent)));

            this.cmbFileNamePresent.SelectedIndex = 0;
			this.cmbMultiLanguage.Items.AddRange(Enum.GetNames(typeof(EMultiLanguage)));
			this.cmbUIDisplaySelect.Items.AddRange(Enum.GetNames(typeof(EUIDisplayType)));

			this.cmbTestResultPathCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
			this.cmbTestResultPathCreatFolderType02.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
			this.cmbTestResultPathCreatFolderType03.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
			this.cmbManualOutputPathCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
			this.cmbManualOutputPathCreatFolderType02.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
			this.cmbManualOutputPathCreatFolderType03.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));

            this.cmbLIV_PIVPathCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbLIV_PIVPathCreatFolderType02.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbLIV_PIVPathCreatFolderType03.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));

            this.cmbSweepPathCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbLCRSweepPathCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));

            this.cmbRelSptCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbAbsSptCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            
            //----------------------------------------------------------
            // Alec, 20131212
            // WAF 和 Statistic 檔案輸出頁面UI檔案名稱Type選項
            //----------------------------------------------------------
            this.cmbWAFTestResultPathCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbWAFTestResultPathCreatFolderType02.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbWAFTestResultPathCreatFolderType03.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbSTATTestResultPathCreatFolderType01.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbSTATTestResultPathCreatFolderType02.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            this.cmbSTATTestResultPathCreatFolderType03.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));


            if (DataCenter._rdFunc.RDFuncData.TesterConfigType == ETesterConfigType.PDTester &&
                DataCenter._machineConfig.SpectrometerModel == DeviceCommon.ESpectrometerModel.NONE)
            {
                tabiSpectrumRawData.Visible = false;
            }
            else
            {
                tabiSpectrumRawData.Visible = true;
            }

            SetCustomizeUI();

            SetSysPathUI();
            
            //this.cmbSweepTestResultPathCreatFolderType.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            //this.cmbSptTestResultPathCreatFolderType.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
		}

        private void SetCustomizeUI()
        {
            List<EUserID> enableIdList = new List<EUserID>();
            enableIdList.Add(EUserID.WAVETEK00);
            enableIdList.Add(EUserID.DOWA);
            enableIdList.Add(EUserID.OptoTech);
            enableIdList.Add(EUserID.Accelink);
            if (enableIdList.Contains(DataCenter._uiSetting.UserID))
            {
                tabiCustomer.Visible = true;
                if (customerFrm != null)
                {
                    customerFrm.Dispose();
                    customerFrm = null;
                }
            }

            switch (DataCenter._uiSetting.UserID)
            {
                case EUserID.WAVETEK00:
                    #region
                    {
                        if (customerFrm == null)
                        {
                            customerFrm = new MPI.Tester.Gui.UIForm.UserForm.UISetting.frmWAVETEK00Path(DataCenter._uiSetting.PathInfoArr);
                        }
                    }
                    #endregion
                    break;
                case EUserID.DOWA:
                    #region
                    {
                        if (customerFrm == null)
                        {
                            //customerFrm = new MPI.Tester.Gui.UIForm.UserForm.UISetting.frmDowaPath(DataCenter._uiSetting.UIMapPathInfo);

                            customerFrm = new MPI.Tester.Gui.UIForm.UserForm.UISetting.frmDowaPath();
                        }
                    }
                     #endregion
                    break;
                case EUserID.OptoTech:
                    {
                        #region
                        if (customerFrm == null)
                        {
                            //customerFrm = new MPI.Tester.Gui.UIForm.UserForm.UISetting.frmOptoTechPath(DataCenter._uiSetting.MergeFilePath);
                            customerFrm = new MPI.Tester.Gui.UIForm.UserForm.UISetting.frmOptoTechPath();
                        }
                        #endregion
                    }
                    break;
                case EUserID.Accelink:
                    #region
                    if (customerFrm == null)
                    {
                        //customerFrm = new MPI.Tester.Gui.UIForm.UserForm.UISetting.frmOptoTechPath(DataCenter._uiSetting.MergeFilePath);
                        customerFrm = new MPI.Tester.Gui.UIForm.UserForm.UISetting.frmAccelinkPath();
                    }
                    #endregion


                    break;
                default:
                    tabiCustomer.Visible = false;
                    break;
            }

            if (enableIdList.Contains(DataCenter._uiSetting.UserID))
            {
                customerFrm.TopLevel = false;

                tabControlPanel6.Controls.Add(customerFrm);

                customerFrm.Dock = DockStyle.Fill;

                customerFrm.Size = tabControlPanel6.Size;

                customerFrm.Show();
            }
        }

        private void SetSysPathUI()
        {
            //int t = 0;
            sysPathFrm = new frmSysPathSet(DataCenter._uiSetting);

            sysPathFrm.TopLevel = false;

            pnlSysPath.Controls.Add(sysPathFrm);

            sysPathFrm.Dock = DockStyle.Top;

            //pFrm.Size = pnlMapPath.Size;

            sysPathFrm.Width = pnlSysPath.Width;

            sysPathFrm.Show();

        }

		private void ChangeAuthority()
		{
			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
                    this.btnSave.Enabled = false;
                    this.tabpBasic.Enabled = false;

                    this.tabpMES.Enabled = false;
              
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer:
                    this.btnSave.Enabled = true;
                    this.tabpBasic.Enabled = true;
           
                    this.tabpMES.Enabled = true;
                
                    break;
				case EAuthority.Admin:
				case EAuthority.Super:
                    this.btnSave.Enabled = true;
                    this.tabpBasic.Enabled = true;

                    this.tabpMES.Enabled = true;

					break;
				//-------------------------------------------------------------------
				default:
                    this.btnSave.Enabled = false;
                    this.tabpBasic.Enabled = false;
                    this.tabpMES.Enabled = false;
                   
					break;
			}
		}

		private void UpdateDataToControls()
		{
#if  ( DebugVer )
			this.cmbUserID.SelectedItem = DataCenter._uiSetting.UserID.ToString();
#else
			this.cmbUserID.SelectedItem = ((int)DataCenter._uiSetting.UserID).ToString("0000");
#endif
			this.txtUserDataVersion.Text = DataCenter._uiSetting.UserDefinedData.Version;

			if (DataCenter._uiSetting.FormatIndex < this.cmbFormat.Items.Count)
			{
				this.cmbFormat.SelectedIndex = (int)DataCenter._uiSetting.FormatIndex;
			}
			else
			{
				this.cmbFormat.SelectedIndex = this.cmbFormat.Items.Count - 1;
			}
            
			bool isMatchFormat = false;
			for (int i = 0; i < this.cmbFormat.Items.Count; i++)
			{
				if (DataCenter._uiSetting.FormatName == this.cmbFormat.Items[i].ToString())
				{
					this.cmbFormat.SelectedIndex = i;
					isMatchFormat = true;
					break;
				}
			}
			if (isMatchFormat == false)
			{
				DataCenter._uiSetting.FormatName = this.cmbFormat.Items[0].ToString();
			}

			this.cmbMultiLanguage.SelectedIndex = DataCenter._uiSetting.MultiLanguage;							 // 0-base

			this.txtMachineName.Text = DataCenter._uiSetting.MachineName;

			this.txtTestResultPath01.Text = DataCenter._uiSetting.TestResultPath01;
			this.txtTestResultPath02.Text = DataCenter._uiSetting.TestResultPath02;
            this.txtTestResultPath03.Text = DataCenter._uiSetting.TestResultPath03;
			this.txtManualOutputPath01.Text = DataCenter._uiSetting.ManualOutputPath01;
			this.txtManualOutputPath02.Text = DataCenter._uiSetting.ManualOutputPath02;
			this.txtManualOutputPath03.Text = DataCenter._uiSetting.ManualOutputPath03;
			this.txtTestResultExt.Text = DataCenter._uiSetting.TestResultFileExt;
			this.chkIsEnablePath02.Checked = DataCenter._uiSetting.IsEnablePath02;
            this.chkIsEnablePath03.Checked = DataCenter._uiSetting.IsEnablePath03;
			this.chkIsEnableManualPath02.Checked = DataCenter._uiSetting.IsEnableManualPath02;
			this.chkIsEnableManualPath03.Checked = DataCenter._uiSetting.IsEnableManualPath03;
			this.cmbTestResultPathCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.TesterResultCreatFolderType01;
			this.cmbTestResultPathCreatFolderType02.SelectedIndex = (int)DataCenter._uiSetting.TesterResultCreatFolderType02;
			this.cmbTestResultPathCreatFolderType03.SelectedIndex = (int)DataCenter._uiSetting.TesterResultCreatFolderType03;
			this.cmbManualOutputPathCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.ManualOutputPathType01;
			this.cmbManualOutputPathCreatFolderType02.SelectedIndex = (int)DataCenter._uiSetting.ManualOutputPathType02;
			this.cmbManualOutputPathCreatFolderType03.SelectedIndex = (int)DataCenter._uiSetting.ManualOutputPathType03;

            //------------------------------------------
            // WAF Setting Update To UI
            //------------------------------------------

            string userID = ((int)DataCenter._uiSetting.UserID).ToString("0000");

            string wafFormat = "Format" + userID + DataCenter._uiSetting.FormatName.Replace("Format", "") + "-Sorter";

            string wafFormatPath = System.IO.Path.Combine(Constants.Paths.USER_DIR, wafFormat + ".xslt");

            //Format6074-A-Sorter.xslt

            if (System.IO.File.Exists(wafFormatPath))
            {
                this.tabcFormatB.Visible = true;
            }
            else
            {
                this.tabcFormatB.Visible = false;
            }

            this.txtWAFTestResultPath01.Text = DataCenter._uiSetting.WAFOutputPath01;
            this.txtWAFTestResultPath02.Text = DataCenter._uiSetting.WAFOutputPath02;
            this.txtWAFTestResultPath03.Text = DataCenter._uiSetting.WAFOutputPath03;

            this.chkIsEnableWAFPath01.Checked = DataCenter._uiSetting.IsEnableWAFPath01;
            this.chkIsEnableWAFPath02.Checked = DataCenter._uiSetting.IsEnableWAFPath02;
            this.chkIsEnableWAFPath03.Checked = DataCenter._uiSetting.IsEnableWAFPath03;

            this.txtWAFTestResultExt.Text = DataCenter._uiSetting.WAFTestResultFileExt;

            this.cmbWAFTestResultPathCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.WAFTesterResultCreatFolderType01;
            this.cmbWAFTestResultPathCreatFolderType02.SelectedIndex = (int)DataCenter._uiSetting.WAFTesterResultCreatFolderType02;
            this.cmbWAFTestResultPathCreatFolderType03.SelectedIndex = (int)DataCenter._uiSetting.WAFTesterResultCreatFolderType03;

            //------------------------------------------

            //------------------------------------------
            // STAT Setting Update To UI
            //------------------------------------------

            string statisticFormat = "Format-" + userID;

            string statisticFormatPath = System.IO.Path.Combine(Constants.Paths.USER_DIR, statisticFormat + ".txt");

            //Format6074-A.txt

            if (System.IO.File.Exists(statisticFormatPath))
            {
                this.tabFormatStatistic.Visible = true;
            }
            else
            {
                this.tabFormatStatistic.Visible = false;
            }

            if (DataCenter._sysSetting.SpecCtrl.IsSupportedLIVItem ||
    DataCenter._sysSetting.SpecCtrl.IsSupportedPIVItem ||
    DataCenter._sysSetting.SpecCtrl.IsSupportedTransistorItem ||
    DataCenter._sysSetting.SpecCtrl.IsSupportedSweepItem)
            {
                this.tabiSweepRawData.Visible = true;

                pnlLIV_PIV.Visible = false;
                pnlIV_VISweep.Visible = false;
                pnlLCRSweep.Visible = false;

                if (DataCenter._sysSetting.SpecCtrl.IsSupportedLIVItem ||
                DataCenter._sysSetting.SpecCtrl.IsSupportedPIVItem)
                {
                    pnlLIV_PIV.Visible = true;
                }
                else
                {
                    DataCenter._uiSetting.IsEnableSaveLIVData = false;
                    DataCenter._uiSetting.IsEnableSaveLIVDataPath02 = false;
                    DataCenter._uiSetting.IsEnableSaveLIVDataPath03 = false;
                }

                if (DataCenter._sysSetting.SpecCtrl.IsSupportedSweepItem)
                {
                    pnlIV_VISweep.Visible = true;
                }
                else
                {
                    DataCenter._uiSetting.IsEnableSweepPath = false;
                }
                if (DataCenter._sysSetting.SpecCtrl.IsSupportedLCRSweepItem)
                {
                    pnlLCRSweep.Visible = true;
                }
                else
                {
                    DataCenter._uiSetting.IsEnableSaveRelativeSpectrum = false;
                    DataCenter._uiSetting.IsEnableSaveAbsoluteSpectrum = false;
                }
            }

            this.txtSTATTestResultPath01.Text = DataCenter._uiSetting.STATOutputPath01;
            this.txtSTATTestResultPath02.Text = DataCenter._uiSetting.STATOutputPath02;
            this.txtSTATTestResultPath03.Text = DataCenter._uiSetting.STATOutputPath03;

            this.chkIsEnableSTATPath01.Checked = DataCenter._uiSetting.IsEnableSTATPath01;
            this.chkIsEnableSTATPath02.Checked = DataCenter._uiSetting.IsEnableSTATPath02;
            this.chkIsEnableSTATPath03.Checked = DataCenter._uiSetting.IsEnableSTATPath03;

            this.txtSTATTestResultExt.Text = DataCenter._uiSetting.STATTestResultFileExt;

            this.cmbSTATTestResultPathCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.STATTesterResultCreatFolderType01;
            this.cmbSTATTestResultPathCreatFolderType02.SelectedIndex = (int)DataCenter._uiSetting.STATTesterResultCreatFolderType02;
            this.cmbSTATTestResultPathCreatFolderType03.SelectedIndex = (int)DataCenter._uiSetting.STATTesterResultCreatFolderType03;
            //------------------------------------------

            //this.cmbSweepTestResultPathCreatFolderType.SelectedIndex = (int)DataCenter._uiSetting.SweepTesterResultCreatFolderType;
            
            // Update PIV / LIV Output Path Setting
            this.chkIsSaveLIVRawData.Checked = DataCenter._uiSetting.IsEnableSaveLIVData;
            this.txtSaveLIVRawDataPath.Text = DataCenter._uiSetting.LIVDataSavePath;
            this.cmbLIV_PIVPathCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.LIVCreatFolderType;

            this.chkIsSaveLIVRawDataPath02.Checked = DataCenter._uiSetting.IsEnableSaveLIVDataPath02;
            this.txtSaveLIVRawDataPath02.Text = DataCenter._uiSetting.LIVDataSavePath02;
            this.cmbLIV_PIVPathCreatFolderType02.SelectedIndex = (int)DataCenter._uiSetting.LIVCreatFolderType02;

            this.chkIsSaveLIVRawDataPath03.Checked = DataCenter._uiSetting.IsEnableSaveLIVDataPath03;
            this.txtSaveLIVRawDataPath03.Text = DataCenter._uiSetting.LIVDataSavePath03;
            this.cmbLIV_PIVPathCreatFolderType03.SelectedIndex = (int)DataCenter._uiSetting.LIVCreatFolderType03;
            // Update VI_IV Output Path Setting-------------------------------------------------------------------
            this.chkIsSaveIV_VISweepData.Checked = DataCenter._uiSetting.IsEnableSweepPath;
            this.txtSaveIV_VISweepDataPath.Text = DataCenter._uiSetting.SweepOutputPath;
            this.cmbSweepPathCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.SweepOutputPathType;

            //Spectrum----------------------------------------------------------------------------------------------------------
            //this.cmbSptTestResultPathCreatFolderType.SelectedIndex = (int)DataCenter._uiSetting.SpectrumOutputPathType;

            this.chkIsSaveRelativeSpectrum.Checked = DataCenter._uiSetting.IsEnableSaveRelativeSpectrum;
            this.txtSaveRelativeSpectrumPath.Text = DataCenter._uiSetting.RelativeSpectrumPath;

            this.cmbRelSptCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.SptRelCreatFolderType;


            this.chkIsSaveAbsoluteSpectrum.Checked = DataCenter._uiSetting.IsEnableSaveAbsoluteSpectrum;
            this.txtSaveAbsoluteSpectrumPath.Text = DataCenter._uiSetting.AbsoluteSpectrumPath;

            //this.cmbAbsSptCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.SptAbsCreatFolderType;

            this.cmbAbsSptCreatFolderType01.SelectedIndex = (int)DataCenter._uiSetting.SptAbsCreatFolderType;

            this.numSpectrumCount.Value = (int)DataCenter._uiSetting.SaveSpectrumMaxCount;

			//this.cmbFileNamePresent.SelectedIndex = (int)DataCenter._uiSetting.FileNameFormatPresent;

           // this.cmbFileNamePresent.SelectedItem = DataCenter._uiSetting.FileNameFormatPresent;

            try {

                this.cmbFileNamePresent.SelectedItem =( (EOutputFileNamePresent)DataCenter._uiSetting.FileNameFormatPresent).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("[frmSetUISetting], ChangeAuthority(),EOutputFileNamePresent parse fail Err:"+e.Message);
            }
	
			this.cmbUIDisplaySelect.SelectedIndex = DataCenter._uiSetting.UIDisplayType;    
            //NFP/FFP-----------------------------------------------------------------------------------------------------
            //this.chkIsSaveNFP.Checked = DataCenter._uiSetting.IsEnableSaveNFPDataPath;
            //this.txtSaveNFPRawDataPath.Text = DataCenter._uiSetting.NFPDataSavePath;

            //this.chkIsSaveFFP.Checked = DataCenter._uiSetting.IsEnableSaveFFPDataPath;
            //this.txtSaveFFPRawDataPath.Text = DataCenter._uiSetting.FFPDataSavePath;
      

            //this.chkIsSaveNFPImg.Checked = DataCenter._uiSetting.IsEnableSaveNFPImgPath;
            //this.txtSaveNFPRawDataPath.Text = DataCenter._uiSetting.NFPImgSavePath;

            //this.chkIsSaveFFPImg.Checked = DataCenter._uiSetting.IsEnableSaveFFPImgPath;
            //this.txtSaveFFPRawDataPath.Text = DataCenter._uiSetting.FFPImgSavePath;
            //-----------------------------------------------------------------------------------------------------

            this.txtMESPath.Text = DataCenter._uiSetting.MESPath;

            this.txtMESTestConditionPath.Text = DataCenter._uiSetting.MESPath2;

			this.txtBackupPath.Text = DataCenter._uiSetting.MESBackupPath;

            this.chkEnableMES.Checked = DataCenter._uiSetting.IsEnableRunMesSystem;

            this.chkEnableTransferProberRecipe.Checked = DataCenter._uiSetting.IsDeliverProberRecipe;

            this.chkPopFormTopMost.Checked = DataCenter._uiSetting.IsEnableMapFormTopMost;

            this.chkEnanleEndTestClearMap.Checked = DataCenter._uiSetting.IsEnableAutoClearMapAndCIEChart;

			this.chkClearHistoryWhenStartTest.Checked = DataCenter._uiSetting.IsEnableClearHistoryWhenStartTest;


            this.chkEnableOutputPathByRecipe.Checked = DataCenter._uiSetting.IsTestResultPathByTaskSheet;
            this.txtStandRecipePath.Text = DataCenter._uiSetting.StandRecipePath;

            this.chkIsEnableCheckStandRecipe.Checked = DataCenter._uiSetting.IsCheckStandardRecipe;

            this.chkEnableReCoverStdRecipe.Checked = DataCenter._uiSetting.IsEnableDuplicateStdRecipe;

            this.chkIsCheckDailyChecking.Checked = DataCenter._uiSetting.IsCheckDailyVerifyResult;

            this.chkIsCheckDailyChecking.Checked = DataCenter._uiSetting.IsEnableJudgeRunDailyCheck;

            this.txtDailyCheckingKeyWord.Text = DataCenter._uiSetting.RunDailyCheckingKeyWord;

            this.chkIsEnableCheckDailyCheckingOverDue.Checked=DataCenter._uiSetting.IsCheckDailyCheckingOverDue;

            this.txtMesMoitorPath.Text = DataCenter._uiSetting.DailyCheckingMonitorPath;

            this.numDailyCheckOverDueHours.Value = DataCenter._uiSetting.DailyCheckingOverDueHours;

            this.chkEnableMonitorPreSampingData.Checked = DataCenter._uiSetting.IsEnableMonitorPreSamplingData;

            this.txtSampingDataPath.Text = DataCenter._uiSetting.PreSamplingDataPath;

            //this.chkLoadGroupMap.Checked = DataCenter._uiSetting.IsEnableLaodGroupData;

            this.txtGroupMapPath.Text = DataCenter._uiSetting.GroupDataPath;

            this.chkIsEnableCheckRecipe.Checked = DataCenter._uiSetting.IsCheckStandardRecipe;

            this.chkEnableAutoPopFourMapForm.Checked = DataCenter._sysSetting.IsAutoPopFourMapForm;

            this.chkIsEanbleConFormEnCheckBox.Checked = DataCenter._uiSetting.IsEnableConditionFormEnCheckBox;

            this.chkIsEnableMergeAOIFile.Checked = DataCenter._uiSetting.IsEnableMergeAOIFile ;

            this.chkIsEnableDeletePBAOIFile.Checked = DataCenter._uiSetting.IsDeletePBAOISourceFile;

            this.chkShowReportComments.Checked = DataCenter._uiSetting.IsShowReportCommentsUI;

			this.btnManualRunMES.Visible = DataCenter._uiSetting.IsEnableRunMesSystem;

            UpdataCustomerPath();

            UpdataSysPath();
            
            switch (DataCenter._uiSetting.UIOperateMode)
            {
                case (int)EUIOperateMode.Idle:
					this.ChangeAuthority();
                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
                    this.btnSave.Enabled = false;
                    this.tabpBasic.Enabled = false;
                    this.tabpMES.Enabled = false;
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
        private void UpdataCustomerPath()
        {
            if (customerFrm == null)
            {
                return;
            }
            switch(DataCenter._uiSetting.UserID)
            {
                case(  EUserID.WAVETEK00):
                    {                        
                        (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmWAVETEK00Path).SetData(DataCenter._uiSetting.PathInfoArr.Clone() as PathInfo[]);
                    }
                    break;
                case (EUserID.DOWA):
                    {
                        //(customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmDowaPath).SetData(DataCenter._uiSetting.UIMapPathInfo.Clone() as PathInfo);
                        (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmDowaPath).LoadDataFromDataCenter();
                    }
                    break;
                case (EUserID.OptoTech):
                    {
                        (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmOptoTechPath).LoadDataFromDataCenter();
                    }
                    break;
                case (EUserID.Accelink):
                    {
                        (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmAccelinkPath).LoadDataFromDataCenter();
                    }
                    break;

                    
                default:
                    break;            
            }
        }
        private void UpdataSysPath()
        {
            if (sysPathFrm != null)
            {
                sysPathFrm.Load(DataCenter._uiSetting);
            }
        }

		private string SelectPath(string title)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

			folderBrowserDialog.Description = title;
			folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
			folderBrowserDialog.SelectedPath = Constants.Paths.ROOT;

			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				return folderBrowserDialog.SelectedPath;
			}
			else
			{
				return string.Empty;
			}
		}

		private void SaveDataToFile()
		{
            int i = 0;
			EUserID newUserID = (EUserID)Enum.Parse(typeof(EUserID), this.cmbUserID.SelectedItem.ToString(), true);

			if (DataCenter._uiSetting.UserID != newUserID)
			{
				//-----------------------------------------------------------------
				// Case 1 , new user ID and new format file name
				//-----------------------------------------------------------------
				DataCenter._uiSetting.UserID = newUserID;

				DataCenter.LoadUserData(newUserID); 

				DataCenter._uiSetting.FormatName = this.cmbFormat.SelectedItem.ToString();
				//======================================================
				// Check the LopSaveItem. If it do not match, set to the default value [0]
				//======================================================
				for (i = 0; i < DataCenter._uiSetting.UserDefinedData.LOPItemSelectList.Length; i++)
                {
					if (DataCenter._product.LOPSaveItem.ToString() == DataCenter._uiSetting.UserDefinedData.LOPItemSelectList[i])
                        break;
                }

				if (i == DataCenter._uiSetting.UserDefinedData.LOPItemSelectList.Length)
                {
                    Host.SetErrorCode(EErrorCode.LOPSaveItemNotMatch);

                    DataCenter._product.LOPSaveItem = (ELOPSaveItem)Enum.Parse(typeof(ELOPSaveItem), DataCenter._uiSetting.UserDefinedData.LOPItemSelectList[0], true);
                }
			}			
			else if (DataCenter._uiSetting.FormatName != this.cmbFormat.SelectedItem.ToString())
			{
				//-----------------------------------------------------------------
				// Case 2 ,  new defined format file name 
				//-----------------------------------------------------------------
				DataCenter._uiSetting.FormatName = this.cmbFormat.SelectedItem.ToString();
			}

			// (a) First, Change setting from UserDefind
			DataCenter.ChangeItemNameFromUserDefine();
			// (b) Second, Change setting from UI Setting
			DataCenter._conditionCtrl.ResetLOPVisionProperty(DataCenter._product.LOPSaveItem);

			Host.UpdateDataToAllUIForm();

			DataCenter._uiSetting.MachineName = this.txtMachineName.Text;

			DataCenter._uiSetting.TestResultPath01 = this.txtTestResultPath01.Text;

			DataCenter._uiSetting.TestResultPath02 = this.txtTestResultPath02.Text;

            DataCenter._uiSetting.TestResultPath03 = this.txtTestResultPath03.Text;

			DataCenter._uiSetting.ManualOutputPath01 = this.txtManualOutputPath01.Text;

			DataCenter._uiSetting.ManualOutputPath02 = this.txtManualOutputPath02.Text;

			DataCenter._uiSetting.ManualOutputPath03 = this.txtManualOutputPath03.Text;

			DataCenter._uiSetting.IsEnablePath02 = this.chkIsEnablePath02.Checked;

            DataCenter._uiSetting.IsEnablePath03 = this.chkIsEnablePath03.Checked;

			DataCenter._uiSetting.IsEnableManualPath02 = this.chkIsEnableManualPath02.Checked;

			DataCenter._uiSetting.IsEnableManualPath03 = this.chkIsEnableManualPath03.Checked;

			DataCenter._uiSetting.TesterResultCreatFolderType01 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbTestResultPathCreatFolderType01.SelectedItem.ToString(), true);

			DataCenter._uiSetting.TesterResultCreatFolderType02 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbTestResultPathCreatFolderType02.SelectedItem.ToString(), true);

			DataCenter._uiSetting.TesterResultCreatFolderType03 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbTestResultPathCreatFolderType03.SelectedItem.ToString(), true);

			DataCenter._uiSetting.ManualOutputPathType01 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbManualOutputPathCreatFolderType01.SelectedItem.ToString(), true);

			DataCenter._uiSetting.ManualOutputPathType02 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbManualOutputPathCreatFolderType02.SelectedItem.ToString(), true);

			DataCenter._uiSetting.ManualOutputPathType03 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbManualOutputPathCreatFolderType03.SelectedItem.ToString(), true);

			DataCenter._uiSetting.TestResultFileExt = this.txtTestResultExt.Text;

            // WAF PATH
            DataCenter._uiSetting.WAFOutputPath01 = this.txtWAFTestResultPath01.Text;

            DataCenter._uiSetting.WAFOutputPath02 = this.txtWAFTestResultPath02.Text;

            DataCenter._uiSetting.WAFOutputPath03 = this.txtWAFTestResultPath03.Text;

            DataCenter._uiSetting.IsEnableWAFPath01 = this.chkIsEnableWAFPath01.Checked;

            DataCenter._uiSetting.IsEnableWAFPath02 = this.chkIsEnableWAFPath02.Checked;

            DataCenter._uiSetting.IsEnableWAFPath03 = this.chkIsEnableWAFPath03.Checked;

            DataCenter._uiSetting.WAFTesterResultCreatFolderType01 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbWAFTestResultPathCreatFolderType01.SelectedItem.ToString(), true);

            DataCenter._uiSetting.WAFTesterResultCreatFolderType02 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbWAFTestResultPathCreatFolderType02.SelectedItem.ToString(), true);

            DataCenter._uiSetting.WAFTesterResultCreatFolderType03 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbWAFTestResultPathCreatFolderType03.SelectedItem.ToString(), true);

            DataCenter._uiSetting.WAFTestResultFileExt = this.txtWAFTestResultExt.Text;

            // PIV / LIV PATH
            DataCenter._uiSetting.IsEnableSaveLIVData = this.chkIsSaveLIVRawData.Checked;

            DataCenter._uiSetting.LIVDataSavePath = this.txtSaveLIVRawDataPath.Text;

            DataCenter._uiSetting.LIVCreatFolderType = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbLIV_PIVPathCreatFolderType01.SelectedIndex.ToString(), true);

            DataCenter._uiSetting.IsEnableSaveLIVDataPath02 = this.chkIsSaveLIVRawDataPath02.Checked;

            DataCenter._uiSetting.LIVDataSavePath02 = this.txtSaveLIVRawDataPath02.Text;

            DataCenter._uiSetting.LIVCreatFolderType02 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbLIV_PIVPathCreatFolderType02.SelectedIndex.ToString(), true);

            DataCenter._uiSetting.IsEnableSaveLIVDataPath03 = this.chkIsSaveLIVRawDataPath03.Checked;

            DataCenter._uiSetting.LIVDataSavePath03 = this.txtSaveLIVRawDataPath03.Text;

            DataCenter._uiSetting.LIVCreatFolderType03 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbLIV_PIVPathCreatFolderType03.SelectedIndex.ToString(), true);

            //DataCenter._uiSetting.SweepTesterResultCreatFolderType = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbSweepTestResultPathCreatFolderType.SelectedItem.ToString(), true);

            // VI IV PATH
            DataCenter._uiSetting.IsEnableSweepPath = this.chkIsSaveIV_VISweepData.Checked;

            DataCenter._uiSetting.SweepOutputPath = this.txtSaveIV_VISweepDataPath.Text;

            DataCenter._uiSetting.SweepOutputPathType = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbSweepPathCreatFolderType01.SelectedIndex.ToString(), true);

            // Statistic PATH
            DataCenter._uiSetting.STATOutputPath01 = this.txtSTATTestResultPath01.Text;

            DataCenter._uiSetting.STATOutputPath02 = this.txtSTATTestResultPath02.Text;

            DataCenter._uiSetting.STATOutputPath03 = this.txtSTATTestResultPath03.Text;

            DataCenter._uiSetting.IsEnableSTATPath01 = this.chkIsEnableSTATPath01.Checked;

            DataCenter._uiSetting.IsEnableSTATPath02 = this.chkIsEnableSTATPath02.Checked;

            DataCenter._uiSetting.IsEnableSTATPath03 = this.chkIsEnableSTATPath03.Checked;

            DataCenter._uiSetting.STATTesterResultCreatFolderType01 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbSTATTestResultPathCreatFolderType01.SelectedItem.ToString(), true);

            DataCenter._uiSetting.STATTesterResultCreatFolderType02 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbSTATTestResultPathCreatFolderType02.SelectedItem.ToString(), true);

            DataCenter._uiSetting.STATTesterResultCreatFolderType03 = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbSTATTestResultPathCreatFolderType03.SelectedItem.ToString(), true);

            DataCenter._uiSetting.STATTestResultFileExt = this.txtSTATTestResultExt.Text;

            // Spectrum PATH
            //DataCenter._uiSetting.SpectrumOutputPathType = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbSptTestResultPathCreatFolderType.SelectedItem.ToString(), true);

            DataCenter._uiSetting.IsEnableSaveRelativeSpectrum = this.chkIsSaveRelativeSpectrum.Checked;
            DataCenter._uiSetting.IsEnableSaveAbsoluteSpectrum = this.chkIsSaveAbsoluteSpectrum.Checked;
            DataCenter._uiSetting.SptRelCreatFolderType = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbRelSptCreatFolderType01.SelectedIndex.ToString(), true);
            
            DataCenter._uiSetting.RelativeSpectrumPath = this.txtSaveRelativeSpectrumPath.Text;
            DataCenter._uiSetting.AbsoluteSpectrumPath = this.txtSaveAbsoluteSpectrumPath.Text;
            DataCenter._uiSetting.SptAbsCreatFolderType = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbAbsSptCreatFolderType01.SelectedIndex.ToString(), true);

            DataCenter._uiSetting.SaveSpectrumMaxCount = (uint)this.numSpectrumCount.Value;

            //NFP/FFP----------
            //DataCenter._uiSetting.IsEnableSaveNFPDataPath = this.chkIsSaveNFP.Checked;
            //DataCenter._uiSetting.NFPDataSavePath = this.txtSaveNFPRawDataPath.Text;

            //DataCenter._uiSetting.IsEnableSaveFFPDataPath = this.chkIsSaveFFP.Checked;
            //DataCenter._uiSetting.FFPDataSavePath = this.txtSaveFFPRawDataPath.Text;

            //DataCenter._uiSetting.IsEnableSaveNFPImgPath = this.chkIsSaveNFPImg.Checked;
            //DataCenter._uiSetting.NFPImgSavePath = this.txtSaveNFPImgPath.Text;

            //DataCenter._uiSetting.IsEnableSaveFFPImgPath = this.chkIsSaveFFPImg.Checked;
            //DataCenter._uiSetting.FFPImgSavePath = this.txtSaveFFPImgPath.Text;

            //----------------------------------------------------------
            // Alec, 20131212
            // CSV WAF STAT副檔名一樣時會有檔案附帶風險，在此進行檢查並警告
            //----------------------------------------------------------
				if(DataCenter._uiSetting.UserID != EUserID.HCSemiTek)
				{
					bool checkNameRepeat = false;

					if (DataCenter._uiSetting.TestResultFileExt == DataCenter._uiSetting.WAFTestResultFileExt
						 || DataCenter._uiSetting.TestResultFileExt == DataCenter._uiSetting.STATTestResultFileExt
						 || DataCenter._uiSetting.WAFTestResultFileExt == DataCenter._uiSetting.STATTestResultFileExt)
					{
						 checkNameRepeat = true;
					}

					if (checkNameRepeat)
					{
						 Host.SetErrorCode(EErrorCode.OutputExtFilenameRepeat);
					}
				}

			DataCenter._uiSetting.UIDisplayType = (int)this.cmbUIDisplaySelect.SelectedIndex;			


            DataCenter._uiSetting.MESPath = this.txtMESPath.Text;

            DataCenter._uiSetting.MESPath2 = this.txtMESTestConditionPath.Text;

			DataCenter._uiSetting.MESBackupPath = this.txtBackupPath.Text;

            DataCenter._uiSetting.IsEnableRunMesSystem = this.chkEnableMES.Checked;

            DataCenter._uiSetting.IsEnableMapFormTopMost = this.chkPopFormTopMost.Checked;

            DataCenter._uiSetting.IsEnableAutoClearMapAndCIEChart = this.chkEnanleEndTestClearMap.Checked;

			DataCenter._uiSetting.IsEnableClearHistoryWhenStartTest = this.chkClearHistoryWhenStartTest.Checked;

            DataCenter._uiSetting.IsTestResultPathByTaskSheet = this.chkEnableOutputPathByRecipe.Checked;

            DataCenter._uiSetting.StandRecipePath = this.txtStandRecipePath.Text;

            DataCenter._uiSetting.IsCheckStandardRecipe = this.chkIsEnableCheckStandRecipe.Checked;

            DataCenter._uiSetting.IsEnableDuplicateStdRecipe = this.chkEnableReCoverStdRecipe.Checked;

            DataCenter._uiSetting.IsCheckDailyVerifyResult = this.chkIsCheckDailyChecking.Checked;

            DataCenter._uiSetting.IsEnableJudgeRunDailyCheck = this.chkIsCheckDailyChecking.Checked;

            DataCenter._uiSetting.RunDailyCheckingKeyWord = this.txtDailyCheckingKeyWord.Text;

            DataCenter._uiSetting.IsCheckDailyCheckingOverDue = this.chkIsEnableCheckDailyCheckingOverDue.Checked;

            DataCenter._uiSetting.DailyCheckingMonitorPath = this.txtMesMoitorPath.Text;

            DataCenter._uiSetting.DailyCheckingOverDueHours = this.numDailyCheckOverDueHours.Value;


            DataCenter._uiSetting.IsEnableMonitorPreSamplingData = this.chkEnableMonitorPreSampingData.Checked;

            DataCenter._uiSetting.PreSamplingDataPath=this.txtSampingDataPath.Text;

            DataCenter._uiSetting.IsCheckStandardRecipe = this.chkIsEnableCheckRecipe.Checked ;

            //DataCenter._uiSetting.IsEnableLaodGroupData = this.chkLoadGroupMap.Checked;

            DataCenter._uiSetting.GroupDataPath = this.txtGroupMapPath.Text  ;

			if (!DataCenter._uiSetting.IsEnableRunMesSystem)
			{
				DataCenter._uiSetting.IsConverterTasksheet = false;
			}

            if (DataCenter._uiSetting.UserID == EUserID.HCSemiTek)
            {
                if (DataCenter._uiSetting.MESPath != string.Empty)
                {
                    DataCenter._uiSetting.IsEnableRunMesSystem = true;
                }
            }


            DataCenter._uiSetting.IsDeliverProberRecipe = this.chkEnableTransferProberRecipe.Checked;

			//DataCenter._uiSetting.FileNameFormatPresent = (int)this.cmbFileNamePresent.SelectedIndex;
            DataCenter._uiSetting.FileNameFormatPresent = (int)Enum.Parse(typeof(EOutputFileNamePresent), this.cmbFileNamePresent.SelectedItem.ToString(), true);


            DataCenter._sysSetting.IsAutoPopFourMapForm = this.chkEnableAutoPopFourMapForm.Checked;

            DataCenter._uiSetting.IsEnableConditionFormEnCheckBox = this.chkIsEanbleConFormEnCheckBox.Checked;

            DataCenter._uiSetting.IsEnableMergeAOIFile = this.chkIsEnableMergeAOIFile.Checked;

            DataCenter._uiSetting.IsDeletePBAOISourceFile = this.chkIsEnableDeletePBAOIFile.Checked;

            DataCenter._uiSetting.IsShowReportCommentsUI = this.chkShowReportComments.Checked;

            SaveCustomerPath();

            SaveSysPath();

			Host._MPIStorage.GenerateOutputFileName();

			if (DataCenter._uiSetting.MultiLanguage != this.cmbMultiLanguage.SelectedIndex)
			{
				DataCenter._uiSetting.MultiLanguage = this.cmbMultiLanguage.SelectedIndex;     // 0-base

				FormAgent.MultiLanguage(DataCenter._uiSetting.MultiLanguage);

				FormAgent.AddFormToList();

				FormAgent.BuildFormTabs();
			}

			AppSystem.SetDataToSystem();

			DataCenter.Save();

			this.UpdateDataToControls();

			this.Refresh();

			FormAgent.RecipeForm.CreatUserIDForm();
		}


        private void SaveCustomerPath()
        {
            switch (DataCenter._uiSetting.UserID)
            {
                case (EUserID.WAVETEK00):
                    {
                        if (customerFrm != null)
                        {
                            PathInfo[] pArr = (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmWAVETEK00Path).GetPathArr();
                            DataCenter._uiSetting.PathInfoArr = pArr;
                        }
                    }
                    break;
                case (EUserID.DOWA):
                    {
                        if (customerFrm != null)
                        {
                            //PathInfo pInfo = (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmDowaPath).GetData();                            

                            //DataCenter._uiSetting.UIMapPathInfo = pInfo;

                            (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmDowaPath).SaveDataToDataCenter();
                        }
                    }
                    break;
                case (EUserID.OptoTech):
                    {
                        if (customerFrm != null)
                        {
                            (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmOptoTechPath).SaveDataToDataCenter();
                        }
                    }
                    break;
                case (EUserID.Accelink):
                    {
                        if (customerFrm != null)
                        {
                            (customerFrm as MPI.Tester.Gui.UIForm.UserForm.UISetting.frmAccelinkPath).SaveDataToDataCenter();
                        }
                    }
                    break;


                default:
                    {
                        DataCenter._uiSetting.UIMapPathInfo.EnablePath = false;//強制取消，避免不同user互相複製時UI不顯示又無法關閉
                    }
                    break;
            }
        }

        private void SaveSysPath()
        {
            if (sysPathFrm != null)
            {
                sysPathFrm.Save();
            }
        }
		#endregion

		#region >>> UI Event Handler <<<

		private void frmSetUISetting_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				this.UpdateDataToControls();
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
            UILog.Log(this, sender, "btnSave_Click");

			this.SaveDataToFile();
            Host.UpdateDataToAllUIForm();
		}

		private void cmbUserID_SelectedIndexChanged(object sender, EventArgs e)
		{
			//----------------------------------------------------------------------------
			// (1) Load the selected "UserID" and get the format list
			//		Update formate file name lists for display
			//----------------------------------------------------------------------------
			EUserID newUserID = (EUserID)Enum.Parse(typeof(EUserID), this.cmbUserID.SelectedItem.ToString(), true);
			if (DataCenter.LoadUserData(newUserID))
			{
				this.cmbFormat.Items.Clear();
				this.cmbFormat.Items.AddRange(DataCenter._uiSetting.UserDefinedData.FormatNames);
				this.cmbFormat.SelectedIndex = 0;
			}
			else
			{
				Host.SetErrorCode(EErrorCode.LoadUserDataFail);
				this.cmbUserID.SelectedItem = DataCenter._uiSetting.UserID.ToString();
			}

			this.txtUserDataVersion.Text = DataCenter._uiSetting.UserDefinedData.Version;

			//----------------------------------------------------------------------------
			// (2) Load the original "UserID" 
			//----------------------------------------------------------------------------
			DataCenter.LoadUserData(DataCenter._uiSetting.UserID);

		}
		private void btnTestResultSelect01_Click(object sender, EventArgs e)
		{
			string path = this.SelectPath("Test Output Path");
			if (path != string.Empty)
			{
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtTestResultPath01.Text = path;
                }
			}
		}

		private void btnTestResultSelect02_Click(object sender, EventArgs e)
		{
			string path = this.SelectPath("Test Output Path");
			if (path != string.Empty)
			{
				if ( (path.Length >= Constants.Paths.ROOT.Length) && 
				     (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT) )
				{
					Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
				}
				else
				{
					this.txtTestResultPath02.Text = path;
				}
			}
		}

        private void btnTestResultSelect03_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Test Output Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                     (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtTestResultPath03.Text = path;
                }
            }
        }

		private void btnManualOutputPath01_Click(object sender, EventArgs e)
		{
			string path = this.SelectPath("Test Output Path");
			if (path != string.Empty)
			{
				if ((path.Length >= Constants.Paths.ROOT.Length) &&
					(path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
				{
					Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
				}
				else
				{
					this.txtManualOutputPath01.Text = path;
				}
			}
		}

		private void btnManualOutputPath02_Click(object sender, EventArgs e)
		{
			string path = this.SelectPath("Test Output Path");
			if (path != string.Empty)
			{
				if ((path.Length >= Constants.Paths.ROOT.Length) &&
					(path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
				{
					Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
				}
				else
				{
					this.txtManualOutputPath02.Text = path;
				}
			}
		}

		private void btnManualOutputPath03_Click(object sender, EventArgs e)
		{
			string path = this.SelectPath("Test Output Path");
			if (path != string.Empty)
			{
				if ((path.Length >= Constants.Paths.ROOT.Length) &&
					(path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
				{
					Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
				}
				else
				{
					this.txtManualOutputPath03.Text = path;
				}
			}
		}
       
        private void btnMESDataPathSelect_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("MES");
            if (path != string.Empty)
            {
                this.txtMESPath.Text = path;
            }
        }


		private void btnWaferMapSetting_Click(object sender, EventArgs e)
		{
			frmWaferMapSetting frmWaferMapSetting = new frmWaferMapSetting();

			frmWaferMapSetting.ShowDialog();

			frmWaferMapSetting.Dispose();

			frmWaferMapSetting.Close();

			Host.UpdateDataToAllUIForm();
		}

		#endregion

        private void btnMESStandRecipePathSelect_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("MES");
            if (path != string.Empty)
            {
                this.txtStandRecipePath.Text = path;
            }
        }

        private void btnMesMonitorPath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("MES");
            if (path != string.Empty)
            {
                this.txtMesMoitorPath.Text = path;
            }
        }

        private void btnWAFTestResultSelect01_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("WAF Test Output Path");

            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtWAFTestResultPath01.Text = path;
                }
            }
        }

        private void btnWAFTestResultSelect02_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("WAF Test Output Path");

            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtWAFTestResultPath02.Text = path;
                }
            }
        }

        private void btnWAFTestResultSelect03_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("WAF Test Output Path");

            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
					Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtWAFTestResultPath03.Text = path;
                }
            }
        }

        private void btnSTATTestResultSelect01_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("STAT Test Output Path");

            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSTATTestResultPath01.Text = path;
                }
            }
        }

        private void btnSTATTestResultSelect02_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("STAT Test Output Path");

            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSTATTestResultPath02.Text = path;
                }
            }
        }

        private void btnSTATTestResultSelect03_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("STAT Test Output Path");

            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSTATTestResultPath03.Text = path;
                }
            }
        }

        private void btnMESTestCondition_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("MES");

            if (path != string.Empty)
            {
                this.txtMESTestConditionPath.Text = path;
            }
        }

        private void btnSetSampingDataPath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Sampling Data");
            if (path != string.Empty)
            {
                this.txtSampingDataPath.Text = path;
            }
        }

		private void buttonX1_Click(object sender, EventArgs e)
		{
			string path = this.SelectPath("Backup");
			if (path != string.Empty)
			{
				this.txtBackupPath.Text = path;
			}
		}

        private void chkEnableMES_CheckedChanged(object sender, EventArgs e)
        {
            UILog.Log(this, sender, "chkEnableMES_CheckedChanged");
        }

		private void btnManualRunMES_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnManualRunMES_Click");

			EErrorCode errorCode = MESCtrl.LoadRecipe();

			//Host.SetErrorCode(errorCode);
		}

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            frmUploadRecipe frm = new frmUploadRecipe();

            frm.ShowDialog();

            frm.Close();

            frm.Dispose();
        }

        private void frmSetUISetting_Load(object sender, EventArgs e)
        {

        }

        private void btnLIVRawDataSelect_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Test Output Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSaveLIVRawDataPath.Text = path;
                }
            }
        }
        private void btnLIVRawDataSelect02_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Test Output Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSaveLIVRawDataPath02.Text = path;
                }
            }
        }
        private void btnLIVRawDataSelect03_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Test Output Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSaveLIVRawDataPath03.Text = path;
                }
            }
        }

        private void btnIV_VISweepSelect_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Test Output Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSaveIV_VISweepDataPath.Text = path;
                }
            }
        }

        private void btntRelativeSpectrumSelect_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Test Output Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSaveRelativeSpectrumPath.Text = path;
                }
            }
            
        }

        private void btneAbsoluteSpectrumSelect_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Test Output Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtSaveAbsoluteSpectrumPath.Text = path;
                }
            }
        }



    }
}
