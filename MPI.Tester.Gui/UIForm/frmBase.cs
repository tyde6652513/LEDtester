using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.TestKernel;
using MPI.Tester.Data;
using MPI.AuthorityControl;
using System.IO;
using MPI.Tester.DeviceCommon;
using MPI.UCF;

namespace MPI.Tester.Gui
{
	public partial class frmBase : System.Windows.Forms.Form
	{
		private Form _activeForm;
		private DevComponents.DotNetBar.TabStrip _tabsForms = null;

        private delegate void UpdateTopDataHandler();
        private delegate void UpdateDataHandler();
		private uint _cycleCount;
        private frmSetTaskSheet _frmSetTaskSheet = new frmSetTaskSheet();

        private static frmTestResultChart frmTestResultChart = new Gui.frmTestResultChart();

        private Form _frmShowPopup;

        private static frmAutoCalibChannel _frmAutoCalibChannel = new frmAutoCalibChannel();

		public frmBase()
		{
			Console.WriteLine("[frmBase], frmBase()");

			InitializeComponent();
			this._cycleCount = 0;

			GlobalFlag.IsEnableShowMap = true;

            AppSystem.SwitchUIEvent += new EventHandler<SwitchUIArgs>(AppSystem_SwitchUIEvent);

            AppSystem.PopDialogEvnet += new EventHandler<SwitchUIArgs>(AppSystem_ShowDialog);
		}

		public Control ALogoPanel
		{
			get { return this.pnlLogo;	}
		}

        private const int CP_NOCLOSE_BUTTON = 0x200;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        } 

		private void ChangeAuthority(ref DevComponents.DotNetBar.SuperTabItem tab)
		{
			//switch (DataCenter._uiSetting.AuthorityLevel)
			//{
			//    case EAuthority.Operator:
			//    case EAuthority.QC:
					
			//        break;
			//    //-------------------------------------------------------------------
			//    case EAuthority.Super:
			//        this.btnSave.Enabled = true;
			//        this.btnNewTaskSheetFile.Enabled = true;
			//        this.btnSaveTaskSheetFile.Enabled = true;
			//        this.btnDelTaskSheetFile.Enabled = true;
			//        this.btnWMNewTaskSheetFile.Enabled = true;
			//        this.btnWMSaveTaskSheetFile.Enabled = true;
			//        this.btnWMDelTaskSheetFile.Enabled = true;
			//        break;
			//    //-------------------------------------------------------------------
			//    default:
			//        this.btnSave.Enabled = false;
			//        this.btnNewTaskSheetFile.Enabled = false;
			//        this.btnSaveTaskSheetFile.Enabled = false;
			//        this.btnDelTaskSheetFile.Enabled = false;
			//        this.btnWMNewTaskSheetFile.Enabled = false;
			//        this.btnWMSaveTaskSheetFile.Enabled = false;
			//        this.btnWMDelTaskSheetFile.Enabled = false;
			//        break;
			//}
		}

		#region >>> Public Method <<<

		public void BuildTabs(Dictionary<string, List<FormItem>> formTable)
		{
			foreach (Control aControl in this.tabcMenu.Controls)
			{
				if (aControl is DevComponents.DotNetBar.SuperTabControlPanel)
				{
					DevComponents.DotNetBar.SuperTabControlPanel panel = (aControl as DevComponents.DotNetBar.SuperTabControlPanel);
					
					string keyStr = panel.TabItem.Tag.ToString();

					if (  formTable.ContainsKey(keyStr) )
					{
						if ((panel.Controls[0] is DevComponents.DotNetBar.TabStrip))
						{
							_tabsForms = (panel.Controls[0] as DevComponents.DotNetBar.TabStrip);
						
							_tabsForms.Tabs.Clear();
							_tabsForms.SuspendLayout();
					
							foreach (FormItem fi in formTable[keyStr] )
							{
								DevComponents.DotNetBar.TabItem tabItem= new DevComponents.DotNetBar.TabItem();
								tabItem.BackColor = Color.Transparent;
								tabItem.BackColor2 = Color.Transparent;
								//tabItem.Text = fi.Title;
								tabItem.Name = "tab" + fi.Name;
								//ti.Image = fi.FormImage;
								tabItem.Tag = fi;
								tabItem.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Default;
								tabItem.Click += new System.EventHandler(this.tabEachItem_Click);

#if  ( DebugVer )
								if (fi.Title == "RDFunc")
								{
									tabItem.Visible = true;
								}
#else
								if (fi.Title == "RDFunc")
								{
									tabItem.Visible = false;
								}
#endif

								if (fi.FormType == null)
								{
									continue;
								}

								string formText = FormAgent.RetrieveForm(fi.FormType).Text;

								tabItem.Text = formText;

								_tabsForms.Tabs.Add(tabItem);

								// activate form
								if (fi.IsDefault)
								{
									_tabsForms.SelectedTab = tabItem;
								}
							}				
							_tabsForms.ResumeLayout();
						}
					}
				}
			}

            //paul Test
            //foreach (DevComponents.DotNetBar.TabItem tabItem in this._tabsForms.Tabs)
            //{
            //    string aa = tabItem.Name;
            //    if (aa == "tabCoef")
            //    {
            //        _tabsForms.SelectedTab = tabItem;
            //    }
            //}
            //_tabsForms.ResumeLayout();
 
		}
		       
		public void SwitchMenuTab(string tag)
		{
			foreach (DevComponents.DotNetBar.SuperTabItem tab in this.tabcMenu.Tabs)
			{				
				if ( tab.Tag.ToString() == tag )
				{
					this.tabcMenu.SelectedTab = tab;		
					break;
				}
			}			
		}

		public void SetAuthorityLevel( EAuthority level )
		{
			switch (level)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
					//this.tabOperate.Visible = true;
					this.tabCondition.Visible = true;
					this.tabResult.Visible = true;
					this.tabSetting.Visible = true;
                    this.btnResetState.Enabled = false;
					this.lblCode.Visible = false;
					this.btnUserMgr.Visible = false;
                    this.plCondShow.Enabled = false;
					break;
				//------------------------------------------------------------
				case EAuthority.Engineer:
				case EAuthority.Admin:
				//	this.tabOperate.Visible = true;
					this.tabCondition.Visible = true;
					this.tabResult.Visible = true;
					this.tabSetting.Visible = true;
                    this.btnResetState.Enabled = true;
					this.lblCode.Visible = false;
					this.btnUserMgr.Visible = true;
                    this.plCondShow.Enabled = true;
					break;
				//------------------------------------------------------------
				case EAuthority.Super:
					//this.tabOperate.Visible = true;
					this.tabCondition.Visible = true;
					this.tabResult.Visible = true;
					this.tabSetting.Visible = true;
                    this.btnResetState.Enabled = true;
					this.lblCode.Visible = true;
					this.btnUserMgr.Visible = true;
                    this.plCondShow.Enabled = true;
					break;
				//------------------------------------------------------------
				default:
					break;
			}

			tabcMenu.Refresh();
		}

		public void UpdateDataToUIForm()
		{
			if (this.InvokeRequired && this.IsHandleCreated)
			{
				this.BeginInvoke(new UpdateDataHandler(UpdateDataToControls), null);		// Run at other TestServer Thread
			}
			else if (this.IsHandleCreated)
			{
				this.UpdateDataToControls();			// Run at Main Thread
			}
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmBase_FormClosing( object sender, FormClosingEventArgs e )
		{
			if ( e.CloseReason == CloseReason.UserClosing )
			{
                e.Cancel = true;    // this cancels the close event.

                //this.Hide();
                //if (FormAgent.MainForm.IsDisposed != true)
                //{
                //    FormAgent.MainForm.Show();
                //    FormAgent.MainForm.TopMost = true;
                //}
                //e.Cancel = true;    // this cancels the close event.
			}
		}

		private void btnHide_Click( object sender, EventArgs e )
		{
			this.Hide();
		}

        private void UpdateDataToControls()
        {
            this.cmbTaskSheet.Items.Clear();

            if (DataCenter._uiSetting.IsConverterTasksheet)
            {
                this.cmbTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.MES_FILE_PATH, Constants.Files.TASK_SHEET_EXTENSION));
            }
            else
            {
                this.cmbTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._uiSetting.ProductPath, Constants.Files.TASK_SHEET_EXTENSION));
            }
            
            //if (DataCenter._uiSetting.IsConverterTasksheet)
            //{
            //    DataCenter._uiSetting.ProductPath = Constants.Paths.PRODUCT_FILE;
            //}

            //this.cmbTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._uiSetting.ProductPath, Constants.Files.TASK_SHEET_EXTENSION));
            this.cmbTaskSheet.SelectedItem = DataCenter._uiSetting.TaskSheetFileName;

            if (DataCenter._machineConfig.Enable.IsCheckFilterWheel)
            {

                if (GlobalData.HwFilterWheelPos == 7
                    || GlobalData.HwFilterWheelPos == 0
                    || GlobalData.HwFilterWheelPos == 6)
                {
                    lblHWFilterSelect.Text = "Error_" + GlobalData.HwFilterWheelPos;
                }
                else
                {
                    this.lblHWFilterSelect.Text = GlobalData.HwFilterWheelPos.ToString();
                }

            }
            else
            {
                this.lblHWFilterSelect.Text = "Disable";
            }

            this.lblFilterPosition.Text = (DataCenter._product.ProductFilterWheelPos+1).ToString();

            SetAuthorityLevel(DataCenter._userManag.CurrentAuthority);

            switch (DataCenter._uiSetting.UIOperateMode)
            {
                case (int)EUIOperateMode.Idle:
                    this.btnStartAndOpen.Enabled = true;
                    this.btnEndAndSave.Enabled = false;
                    this.btnCycleRun.Enabled = true;
                    this.cmbTaskSheet.Enabled = true;
                    this.plRecipeCtrl.Enabled = true;
                    this.btnResetState.Enabled = false;
                  //  this.btnReSingleTest.Enabled = true;
                    this.btnUserMgr.Enabled = true;
                    this.btnContinousProbing.Enabled = true;

	                 btnCycleRun.Enabled=false;

                     if (GlobalFlag.IsProductChannelConditionNotMatch)
                     {
                         this.btnReSingleTest.Enabled = false;
                         this.btnStartAndOpen.Enabled = false;
                     }
                     else
                     {
                         this.btnReSingleTest.Enabled = true;
                         this.btnStartAndOpen.Enabled = true;
                     }

                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.AutoRun:
                    this.btnStartAndOpen.Enabled = false;
                    this.btnEndAndSave.Enabled = false;
                    this.btnCycleRun.Enabled = false;
                    this.cmbTaskSheet.Enabled = false;
                    this.plRecipeCtrl.Enabled = false;
                    this.btnResetState.Enabled = true;
               //     this.btnReSingleTest.Enabled = false;
                    this.btnUserMgr.Enabled = false;
                    this.btnContinousProbing.Enabled = false;

                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.ManulRun:
                    this.btnStartAndOpen.Enabled = false;
                    this.btnEndAndSave.Enabled = true;
                    this.btnCycleRun.Enabled = true;
                    this.cmbTaskSheet.Enabled = false;
                    this.plRecipeCtrl.Enabled = false;
                    this.btnResetState.Enabled = true;
                    //     this.btnReSingleTest.Enabled = false;
                    this.btnUserMgr.Enabled = false;
                    this.btnContinousProbing.Enabled = false;
                    break;
                //case (int)EUIOperateMode.ManulRun:
                //    this.btnStartAndOpen.Enabled = false;
                //    this.btnEndAndSave.Enabled = true;
                //    this.btnCycleRun.Enabled = true;
                //    this.cmbTaskSheet.Enabled = false;
                //    this.plRecipeCtrl.Enabled = false;
                //    this.btnResetState.Enabled = false;
                ////    this.btnReSingleTest.Enabled = true;
                //    this.btnUserMgr.Enabled = true;
                //    this.btnContinousProbing.Enabled = false;

                //          btnCycleRun.Enabled = true;
                //    break;
                //-----------------------------------------------------------------------------
                default:
                    this.btnStartAndOpen.Enabled = true;
                    this.btnEndAndSave.Enabled = false;
                    this.btnCycleRun.Enabled = true;
                    this.cmbTaskSheet.Enabled = true;
                    this.plRecipeCtrl.Enabled = true;
                    this.btnResetState.Enabled = false;
               //     this.btnReSingleTest.Enabled = true;
                    this.btnUserMgr.Enabled = true;
                    this.btnContinousProbing.Enabled = false;
						  btnCycleRun.Enabled = false;

                    if (GlobalFlag.IsProductChannelConditionNotMatch)
                    {
                        this.btnReSingleTest.Enabled = false;
                        this.btnStartAndOpen.Enabled = false;
                    }
                    else
                    {
                        this.btnReSingleTest.Enabled = true;
                        this.btnStartAndOpen.Enabled = true;
                    }

                    break;
            }

            // TesterConfigType, Roy, 20160627
            switch (DataCenter._rdFunc.RDFuncData.TesterConfigType)
            {
                case ETesterConfigType.LDTester:
                    {
                        this.lblTesterMode.Text = "LDTester - LD200";
                        break;
                    }
                case ETesterConfigType.PDTester:
                    {
                        this.lblTesterMode.Text = "PDTester - PD200";
                        break;
                    }
                default:
                    {
                        this.lblTesterMode.Text = "LEDTester - T200";
                        break;
                    }
            }



            //if(AppSystem._MPITesterKernel.Status.State == EKernelState.Error || AppSystem._MPITesterKernel.Status.State == EKernelState.Not_Ready)
            //{
            //    this.btnStartAndOpen.Enabled = false;
            //    this.btnEndAndSave.Enabled = false;
            //    this.btnCycleRun.Enabled = false;
            //}
        }

        private void updateUI(object sender, EventArgs e)
        {
            if (_activeForm != null)
            {
                _activeForm.Hide();
            }

            FormItem fi = (FormItem)(sender as DevComponents.DotNetBar.TabItem).Tag;
            Form temp_form = FormAgent.RetrieveForm(fi.FormType);
            temp_form.TopLevel = false;
            temp_form.Parent = this.pnlContainer;
            temp_form.Dock = DockStyle.Fill;
            temp_form.Show();

            //if (temp_form.Equals(FormAgent.BinSettingForm))
            //{
            //    FormAgent.BinSettingForm.penalCIE.Controls.Add(FormAgent.BinSettingForm.penalCIEChartControl);
            //}

            //if (temp_form.Equals(FormAgent.TestResultForm))
            //{
            //    FormAgent.TestResultForm.PanelCIE.Controls.Add(FormAgent.BinSettingForm.penalCIEChartControl);
            //}

            _activeForm = temp_form;
        }

		private void frmBase_Load( object sender, EventArgs e )
		{ 
			string version = Host.GetProgramVersion();
			this.Text = "MPI TESTER" + "  [ " + version + " ]";

			AppSystem_SwitchUIEvent(null, new SwitchUIArgs((int)EBaseFormDisplayUI.UISettingForm));
			AppSystem_SwitchUIEvent(null, new SwitchUIArgs((int)EBaseFormDisplayUI.ConditionForm));
            AppSystem_SwitchUIEvent(null, new SwitchUIArgs((int)EBaseFormDisplayUI.ResultForm));
            AppSystem_SwitchUIEvent(null, new SwitchUIArgs((int)EBaseFormDisplayUI.BinSettingForm));
            AppSystem_SwitchUIEvent(null, new SwitchUIArgs((int)EBaseFormDisplayUI.OperatorForm));
      
			DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;

            int screenHeight = Screen.PrimaryScreen.Bounds.Height;


            this.Size = new Size(1280, 1000);
             
            //SetAuthorityLevel(DataCenter._userManag.CurrentAuthority);
            SetAuthorityLevel(DataCenter._userManag.CurrentAuthority);
            this.UpdateDataToControls();  
		}

		private void btnWaferMap_Click( object sender, EventArgs e )
		{
			if (DataCenter._sysSetting.IsAutoPopFourMapForm)
			{
				ShowRunningForm();
			}
			else
			{
				ShowRunningForm();

				FormAgent.TestResultForm.autoArrangeWindow();
			}

           // this.Hide();

           // UILog.Log(this, sender, "btnWaferMap_Click");
           //// FormAgent.TestResultForm.ShowSelectPopMap();
          //  FormAgent.TestResultForm.autoStartPopWaferMap();
			// FormAgent.PopupWaferMapForm( typeof( frmWaferMap ), null );			
		}

		private void ShowRunningForm()
		{
			if (!DataCenter._sysSetting.IsAutoPopFourMapForm)
			{
				return;
			}

			if (frmTestResultChart.Visible == true)
			{
                this.Hide();

				return;
			}


			if (this.Visible)
			{
				frmTestResultChart.Show();

				//frmTestResultChart.FormBorderStyle = FormBorderStyle.Sizable;

				frmTestResultChart.Location = new Point(100, 1);

				this.Hide();
			}
			else
			{
				FormAgent.MainForm.Hide();

				frmTestResultChart.Show();

				//frmTestResultChart.FormBorderStyle = FormBorderStyle.Sizable;

                frmTestResultChart.Location = new Point(100, 1);

				frmTestResultChart.TopMost = true;

				frmTestResultChart.TopMost = false;
			}
		}

		private void btnCIEMap_Click( object sender, EventArgs e )
		{
		}

		private void frmBase_VisibleChanged( object sender, EventArgs e )
		{
            //FormAgent.MainForm.Hide();

			if ( this.Visible == true )
			{
				tmrUpdate.Enabled = true;
			}
			else
			{
				tmrUpdate.Enabled = false;
			}
		}

        private void btnStartAndOpen_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnStartAndOpen_Click");

			DataCenter._uiSetting.IsManualRunMode = true;

            //--------------------------------------------------------------------
			// (1) Generate the File Name ( and record the StartTime )			
            //--------------------------------------------------------------------
			if (DataCenter._uiSetting.UIDisplayType == (int)EUIDisplayType.MPIStartUI)
			{
                FormAgent.RecipeForm.SaveMPIData(true);

                Host.FireSaveDataEvent();

				//Host._MPIStorage.GenerateOutputFileName();
			}
			else
			{
				FormAgent.RecipeForm.SaveWMData();

				if (WMOperate.WM_StartTest() == false)
					return;
			}

            //--------------------------------------------------------------------
			// (2) Open the TestResult File Name
            //--------------------------------------------------------------------
			string testResultFileName = string.Empty;

			if (Report.ReportProcess.IsImplement)
			{
				testResultFileName = Report.ReportProcess.TestResultFileName;
			}
			else
			{ 
				testResultFileName = DataCenter._uiSetting.TestResultFileName;
			}

			if (testResultFileName == string.Empty)
			{
				Host.SetErrorCode(EErrorCode.NoSaveFileName);
				return;
			}

			bool exist = false;

			if (Report.ReportProcess.IsTempFileExist)
			{
				exist = Report.ReportProcess.IsOutputReportExist();
			}
			else
			{ 
				exist = Host._MPIStorage.IsExistTestOutputFileName();
			}

			if (exist)
			{
                DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.CheckIsOverWriteTestOutputFile, "Test Result Output File Exist, Would you overwrite it? ", "MSG");

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.ManulRun;
				}
				else
				{
					Host.SetErrorCode(EErrorCode.TestResultFileExisted);
					DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

					return;
				}
			}
			else
			{
				DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.ManulRun;
			}
            //--------------------------------------------------------------------
            // (3) Reset Data
            //--------------------------------------------------------------------            
            AppSystem.SetDataToSystem();

			this._cycleCount = 0;

            Report.ReportProcess.RunCommand(TestServer.EServerQueryCmd.CMD_TESTER_START);

            Host.UpdateDataToAllUIForm();
		}

        private void btnEndAndSave_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnEndAndSave_Click");

			//AppSystem.StopTest();

            AppSystem.RunCommand(TestKernel.ETesterKernelCmd.GetPDDarkCurrent);

            if (DataCenter._uiSetting.UIDisplayType == (int)EUIDisplayType.MPIStartUI)
            {
                FormAgent.RecipeForm.SaveMPIData(false);
            }

			if (Host._MPIStorage.SaveTestResultToFile(true) == false)
			{
				Host.SetErrorCode(EErrorCode.SaveFileFail);
			}

            Host._MPIStorage.SaveSweepRawData();

			DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

			if (DataCenter._uiSetting.UIDisplayType == (int)EUIDisplayType.WMStartUI)
			{
				WMOperate.WM_EndTest();
				DataCenter._uiSetting.WeiminUIData.OutputFileName = DataCenter._uiSetting.WeiminUIData.KeyInFileName;
			}

            Report.ReportProcess.RunCommand(TestServer.EServerQueryCmd.CMD_TESTER_END);

            Host.UpdateDataToAllUIForm();

			DataCenter._uiSetting.IsManualRunMode = false;

		}

		private void btnCycleRun_Click( object sender, EventArgs e )
		{
			UILog.Log(this, sender, "btnCycleRun_Click");

            if (DataCenter._uiSetting.UIOperateMode == (int)EUIOperateMode.Idle)
            {
                AppSystem.ResetDataList();

                this._cycleCount = 0;
            }
            else
            {
                this._cycleCount++;
            }

			AppSystem.ManualRun( this._cycleCount, 
								DataCenter._sysSetting.DieRepeatTestCount,
								DataCenter._sysSetting.DieRepeatTestDelay);			


			//TopMessageBox.PopMessage(999);
		}

        private void AppSystem_SwitchUIEvent(object sender, SwitchUIArgs e)
        {
			Console.WriteLine("[frmBase], AppSystem_SwitchUIEvent(), e.Index = " + e.Index.ToString());

			if (this.IsHandleCreated == false)
			{
				return;
			}

            Form temp_form = null;

            switch (e.Index)
            {
                case (int)EBaseFormDisplayUI.HideAll:
                    //FormAgent.BaseForm.Hide();
                    //FormAgent.MainForm.Show();
                    return;
                case (int)EBaseFormDisplayUI.RunningForm:
                    this.ShowRunningForm();
                    return;
                case (int)EBaseFormDisplayUI.ResultForm:
                    temp_form = FormAgent.TestResultForm;
                    plRecipeCtrl.Visible = true;
                    break;
                case (int)EBaseFormDisplayUI.OperatorForm:
                    temp_form = FormAgent.RecipeForm;
                    break;
                case (int)EBaseFormDisplayUI.ConditionForm:
                    temp_form = FormAgent.ConditionForm;
                    plRecipeCtrl.Visible = true;
                    break;
                case (int)EBaseFormDisplayUI.UISettingForm:
                    temp_form = FormAgent.SetUISettingForm;
                    break;

                case (int)EBaseFormDisplayUI.SystemSettingForm:
                    temp_form = FormAgent.RetrieveForm(typeof(frmSetSysParam));
                    break;
                case (int)EBaseFormDisplayUI.CalibrationForm:
                    temp_form = FormAgent.RetrieveForm(typeof(frmConditionCoef));
                    plRecipeCtrl.Visible = true;
                    break;
                case (int)EBaseFormDisplayUI.BinSettingForm:
                    temp_form = FormAgent.RetrieveForm(typeof(frmBinSetting));
                    break;
                case (int)EBaseFormDisplayUI.MachineForm:
                    temp_form = FormAgent.RetrieveForm(typeof(frmSetMachine));
                    break;
                case (int)EBaseFormDisplayUI.SetProductForm:
                    temp_form = FormAgent.RetrieveForm(typeof(frmSetProduct));
                    plRecipeCtrl.Visible = true;
                    break;
                case (int)EBaseFormDisplayUI.SpectrumForm:
                    temp_form = FormAgent.RetrieveForm(typeof(frmTestResultSpectrum));
                    break;
                default:
					return;
            }

			if (_activeForm != null)
			{
				_activeForm.Hide();
			}

          //  FormAgent.MainForm.Hide();
            FormAgent.BaseForm.Show();

            temp_form.TopLevel = false;
            temp_form.Parent = this.pnlContainer;
            temp_form.Dock = DockStyle.Fill;
            temp_form.Show();

            _activeForm = temp_form;

            //UpdateDataToControls();
            FormAgent.BaseForm.TopMost = true;
            FormAgent.BaseForm.TopMost = false;

				//if (e.Index == (int)EBaseFormDisplayUI.ResultForm)
				//{
				//   FormAgent.BaseForm.Hide();
				//}


        }

		private void tmrUpdate_Tick( object sender, EventArgs e )
		{
			this.lblSysStatus.Text = AppSystem._MPITesterKernel.Status.State.ToString();
			this.lblAuthority.Text = DataCenter._uiSetting.AuthorityLevel.ToString();
			this.lblUser.Text = "User:" + DataCenter._uiSetting.OperatorName;
			this.lblTaskSheet.Text = "TS: " + DataCenter._uiSetting.TaskSheetFileName;
			this.lblDateTime.Text = System.DateTime.Now.ToShortTimeString();

            //if (  AppSystem._MPITesterKernel.Status.State == EKernelState.Running)
            //{
            //    this.btnStartAndOpen.Enabled = false;
            //    this.btnEndAndSave.Enabled = false;
            //    this.btnCycleRun.Enabled = false;

            //}
            //else if (	AppSystem._MPITesterKernel.Status.State == EKernelState.Not_Ready &&
            //            DataCenter._uiSetting.LoginID != "simulator")
            //{
            //    this.btnStartAndOpen.Enabled = false;
            //    this.btnEndAndSave.Enabled = false;
            //    this.btnCycleRun.Enabled = false;
            //}
            //else
            //{
            //    switch (DataCenter._uiSetting.UIOperateMode)
            //    {
            //        case (int)EUIOperateMode.Idle:
            //            this.btnStartAndOpen.Enabled = true;
            //            this.btnEndAndSave.Enabled = false;
            //            this.btnCycleRun.Enabled = true;
            //            break;
            //        //-----------------------------------------------------------------------------
            //        case (int)EUIOperateMode.AutoRun:
            //            this.btnStartAndOpen.Enabled = false;
            //            this.btnEndAndSave.Enabled = false;
            //            this.btnCycleRun.Enabled = false;
            //            break;
            //        //-----------------------------------------------------------------------------
            //        case (int)EUIOperateMode.ManulRun:
            //            this.btnStartAndOpen.Enabled = false;
            //            this.btnEndAndSave.Enabled = true;
            //            this.btnCycleRun.Enabled = true;
            //            break;
            //        //-----------------------------------------------------------------------------
            //        default:
            //            this.btnStartAndOpen.Enabled = true;
            //            this.btnEndAndSave.Enabled = false;
            //            this.btnCycleRun.Enabled = true;
            //            break;
            //    }
            //}

			//this.lblCode.Text = GlobalFlag.SeqStep.ToString("0000") + "_" + GlobalFlag.OptimumStatus.ToString("0000");

			System.Windows.Forms.Application.DoEvents();	
		}

		private void tabcMenu_SelectedTabChanged(object sender, DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs e)
		{
			Control oneControl = (sender as DevComponents.DotNetBar.SuperTabControl).SelectedPanel.Controls[0];
			if ( oneControl is DevComponents.DotNetBar.TabStrip)
			{
				if ((oneControl as DevComponents.DotNetBar.TabStrip).Tabs.Count > 0)
				{
					(oneControl as DevComponents.DotNetBar.TabStrip).Tabs[0].PerformClick();
                    (oneControl as DevComponents.DotNetBar.TabStrip).SelectedTabIndex = 0;
				}
			}
		}

		private void tabEachItem_Click(object sender, EventArgs e)
		{
			if (_activeForm != null)
			{
				_activeForm.Hide();
			}
			
			FormItem fi = ( FormItem) ( sender as DevComponents.DotNetBar.TabItem).Tag;
			Form temp_form = FormAgent.RetrieveForm(fi.FormType);
			temp_form.TopLevel = false;
			temp_form.Parent = this.pnlContainer;
			temp_form.Dock = DockStyle.Fill;
			temp_form.Show();

            if (temp_form.Equals(FormAgent.SetUISettingForm) ||
                temp_form.Equals(FormAgent.SetSysParamForm)||
                temp_form.Equals(FormAgent.SetMachineForm))
            {
                plRecipeCtrl.Visible = false;
            }
            else
            {
                plRecipeCtrl.Visible = true;
            }

            if (temp_form.Equals(FormAgent.TestResultForm) ||
                temp_form.Equals(FormAgent.TestResultSpectrum) ||
                temp_form.Equals(FormAgent.TestResultAnalyzeForm) ||
                temp_form.Equals(FormAgent.TestResultAnalyzeForm2) ||
                temp_form.Equals(FormAgent.TestResultCurveAnalyzeForm))
            {
                plCondShow.Visible = false;
            }
            else
            {
                plCondShow.Visible = true;
            }

            //if (temp_form.Equals(FormAgent.TestResultForm))
            //{
            //    FormAgent.TestResultForm.PanelCIE.Controls.Add(FormAgent.BinSettingForm.penalCIEChartControl);
            //}

			_activeForm = temp_form;
            this.UpdateDataToControls();
        }

		private void btnLogin_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnLogin_Click");
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.LogOut, " Log Out ? ", "Qusetion");
			//DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Log Out ? ", "Qusetion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (result != DialogResult.OK)
				return;

			this.Hide();

			Host._frmLogin.ShowDialog();
			FormAgent.HideAll();
			Host.UpdateDataToAllUIForm();
		}

		private void btnGoProberUI_Click(object sender, EventArgs e)
		{
            this.WindowState = FormWindowState.Minimized;
           // AppSystem_SwitchUIEvent(null, new SwitchUIArgs((int)EBaseFormDisplayUI.HideAll));
		}

		private void btnReSingleTest_Click(object sender, EventArgs e)
		{
            //UILog.Log(this, sender, "btnReSingleTest_Click");

            //DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.RunSingleReTest, "Test Result Output File Exist, Would you Run Test ? ", "  Run Single Retest");

            //if (result != DialogResult.OK)
            //    return;

			GlobalFlag.IsReSingleTestMode = true;
			AppSystem.RunSingleRetest();
		}

		private void btnLoadTask_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnLoadTask_Click", this.cmbTaskSheet.Text);

			GlobalFlag.IsSuccessCheckFilterWheel = true;

			DataCenter.LoadTaskSheet(this.cmbTaskSheet.Text);

			WMOperate.WM_ReadCalibrateParamFromSetting();

			AppSystem.SetDataToSystem();

			AppSystem.CheckMachineHW();

            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);

			Host.UpdateDataToAllUIForm();

           
		}

		private void btnUserMgr_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnUserMgr_Click");

			FormAgent.UserManagerForm.ShowDialog();
		}

        private void btnResetState_Click(object sender, EventArgs e)
        {
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.ResetSystemState, " Would you want to reset system state ? ", "Qusetion");
         //   DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Would you want to reset system state ? ", "Warn", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            UILog.Log(this, sender, "btnResetState_Click");

            DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.ManulRun;
            Host.UpdateDataToAllUIForm();
        }

        private void btnArrangeWindow_Click(object sender, EventArgs e)
        {
            AppSystem._autoCalibChannelGain.Start(4, DataCenter._product);
            //if (DataCenter._sysSetting.IsAutoPopFourMapForm)
            //{
            //    ShowRunningForm();
            //}
            //else
            //{
            //    FormAgent.TestResultForm.autoArrangeWindow();
            //}
        }

        private void btnContinousProbing_Click(object sender, EventArgs e)
        {

            FormAgent.MainForm.ExitApp();

            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.linkupProbing, " 確認接續點測 ? ", "Qusetion");
            //   DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Would you want to reset system state ? ", "Warn", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;
 
            FormAgent.MainForm.ExitApp();

        }

        private void btnAlarmLog_Click(object sender, EventArgs e)
        {
            TopMessageBox.ShowHistory();
        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            UILog.Log(this, sender, "btnNewTaskSheetFile_Click");

            DataCenter._uiSetting.ControlTaskSettingUI = EControlTaskSetting.NEW;

            _frmSetTaskSheet.ShowDialog();

            if (DataCenter._uiSetting.SendTaskFileName == "")
                return;
            //
            FormAgent.RecipeForm.NewTaskSheetFile(DataCenter._uiSetting.SendTaskFileName);

            if (!Host._MPIStorage.SaveTestCoefficientToFile())
            {
                Host.SetErrorCode(EErrorCode.SaveWatchCoefficientFileFail);
            }


            WMOperate.WM_ReadCalibrateParamFromSetting();
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            UILog.Log(this, sender, "btnSaveAsTaskSheetFile_Click");

            DataCenter._uiSetting.ControlTaskSettingUI = EControlTaskSetting.SAVEAS;

            _frmSetTaskSheet.ShowDialog();

            if (DataCenter._uiSetting.SendTaskFileName == "")
                return;

            FormAgent.RecipeForm.SaveTaskSheetFile(DataCenter._uiSetting.SendTaskFileName);

            if (!Host._MPIStorage.SaveTestCoefficientToFile())
            {
                Host.SetErrorCode(EErrorCode.SaveWatchCoefficientFileFail);
            }

            WMOperate.WM_ReadCalibrateParamFromSetting();
            //Host.UpdateDataToAllUIForm();	
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.DeleteTaskSheet, "Would you delete Selceted Recipe? ", "Qusetion ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            //DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Delete Selceted Recipe ? ", "Qusetion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            UILog.Log(this, sender, "btnDelTaskSheetFile_Click", this.cmbTaskSheet.Text);

            FormAgent.RecipeForm.DelTaskSheetFile(this.cmbTaskSheet.Text);
            this.cmbTaskSheet.Items.Remove(this.cmbTaskSheet.Text);
            this.cmbTaskSheet.SelectedIndex = 0;
            DataCenter.LoadTaskSheet(this.cmbTaskSheet.Text);

            Host.UpdateDataToAllUIForm();
        }

        private void btnBinSetting_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.BinSettingForm);
        }

        private void btnCondition_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);
        }

        private void btnTestResult_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ResultForm);
        }

        private void tsMain_ItemClick(object sender, EventArgs e)
        {

        }

        private void btnTool_Click(object sender, EventArgs e)
        {

        }

        private void btnSubCalib_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.CalibrationForm);
        }

        private void btnSubProduct_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.SetProductForm);
        }

        private void btnCond_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.UISettingForm);
        }

        private void btnExitApp_Click(object sender, EventArgs e)
        {
            FormAgent.MainForm.ExitApp();
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            FormAgent.MainForm.ExitApp();
        }

        private void AppSystem_ShowDialog(object sender, SwitchUIArgs e)
        {
            if (this._frmShowPopup != null)
            {
                _frmShowPopup.Close();

                _frmShowPopup.Dispose();
            }

            switch (e.Index)
            {
                case (int)EPopUpUIForm.AutoChannelCalibrationForm:

                    _frmShowPopup = new frmAutoCalibChannel();
                    _frmShowPopup.Show();
                    break;
                case (int)EPopUpUIForm.UserComments:

                    _frmShowPopup = new frmReportComments();
                    _frmShowPopup.ShowDialog();
                    break;
            }

        //    this._frmShowPopup.Close();

        //    this._frmShowPopup.Dispose();

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            AppSystem._autoCalibChannelGain.End();

            AppSystem.Fire_PopUIEvent((int)EPopUpUIForm.AutoChannelCalibrationForm);
        }

        private void buttonX1_Click()
        {

		}
        private void cmbTaskSheet_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated)
            {
                return;
            }

            UILog.Log(this, sender, "btnLoadTask_Click", this.cmbTaskSheet.Text);

            GlobalFlag.IsSuccessCheckFilterWheel = true;

            DataCenter.LoadTaskSheet(this.cmbTaskSheet.Text);
            
            WMOperate.WM_ReadCalibrateParamFromSetting();

            AppSystem.SetDataToSystem();

            AppSystem.CheckMachineHW();

            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);

            Host.UpdateDataToAllUIForm();
        }

		#endregion

        
	}
}			