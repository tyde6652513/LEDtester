using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using MPI.Tester.Data;
using System.Collections.Generic;

namespace MPI.Tester.Gui
{
	public partial class frmOpRecipe : System.Windows.Forms.Form
	{
		private delegate void UpdateDataHandler();
        public static frmSetTaskSheet _frmSetTaskSheet = new frmSetTaskSheet();
       // public frmDailyVerify _frmDailyWath = new frmDailyVerify();
		private Form _frmUserID;
      //  public frmDailyCheckSetting _frmDailyCheckSetting;
		public event EventHandler<EventArgs> OpRecipeSaveEvent;
		public event EventHandler<EventArgs> UpDateUserIDFormEvent;

		public frmOpRecipe()
		{
			Console.WriteLine("[frmOpRecipe], frmOpRecipe()");
			
			InitializeComponent();
		}

		#region >>> Private Method <<<

		private void ChangeAuthority()
		{
           // this.btnManualRunMES.Enabled = false;

			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
					this.btnSave.Enabled = true;
					this.btnNewTaskSheetFile.Enabled = false;
					this.btnOpenTaskSheetFile.Enabled = true;
					this.btnSaveAsTaskSheetFile.Enabled = false;
					this.btnDelTaskSheetFile.Enabled = false;
					this.btnWMNewTaskSheetFile.Enabled = false;
					this.btnWMSaveTaskSheetFile.Enabled = false;
					this.btnWMDelTaskSheetFile.Enabled = false;
					this.gpbWMTestMode.Enabled = false;
					this.btnWMOpenTaskSheetFile.Enabled = true;
					this.gplOutputPathByRecipe.Enabled = false;
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer:
				case EAuthority.Admin:
				case EAuthority.Super:
					this.btnSave.Enabled = true;
					this.btnOpenTaskSheetFile.Enabled = true;
					this.btnNewTaskSheetFile.Enabled = true;
					this.btnSaveAsTaskSheetFile.Enabled = true;
					this.btnDelTaskSheetFile.Enabled = true;
					this.btnWMNewTaskSheetFile.Enabled = true;
					this.btnWMSaveTaskSheetFile.Enabled = true;
					this.btnWMDelTaskSheetFile.Enabled = true;
					this.gpbWMTestMode.Enabled = true;
					this.btnWMOpenTaskSheetFile.Enabled = true;
					this.gplOutputPathByRecipe.Enabled = true;
					break;
				//-------------------------------------------------------------------
				default:
					this.btnSave.Enabled = true;
					this.btnNewTaskSheetFile.Enabled = false;
					this.btnOpenTaskSheetFile.Enabled = true;
					this.btnSaveAsTaskSheetFile.Enabled = false;
					this.btnDelTaskSheetFile.Enabled = false;
					this.btnWMNewTaskSheetFile.Enabled = false;
					this.btnWMSaveTaskSheetFile.Enabled = false;
					this.btnWMDelTaskSheetFile.Enabled = false;
					this.gpbWMTestMode.Enabled = false;
					this.btnWMOpenTaskSheetFile.Enabled = true;
					this.gplOutputPathByRecipe.Enabled = true;
					break;
			}
		}

		private void UpdateDataToControls()
		{
			if (DataCenter._uiSetting.UIDisplayType == 0)
			{
				this.tabcOpRecipe.SelectedPanel = this.tabpMPI;
			}
			else
			{
				this.tabcOpRecipe.SelectedPanel = this.tabpWeiminUI;
			}

            if (DataCenter._uiSetting.IsEnableRunMesSystem == true)
            {
				if (this.UpDateUserIDFormEvent != null)
				{
					this.UpDateUserIDFormEvent(null, null);
				}

                this.lblEMESEnable.Visible = true;
                this.btnManualRunMES.Enabled = true;
                this.btnManualRunMES.Visible = true;

                if (DataCenter._uiSetting.IsDeliverProberRecipe)
                {
                    string path = Path.Combine(@"C:\FAE\Product", DataCenter._uiSetting.ProberRecipeName + ".pat");

                    if (File.Exists(path))
                    {

                         string tempPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, DataCenter._uiSetting.ProberRecipeName + ".pat");

                        if(!File.Exists(tempPath))
                        {
                            File.Copy(path, tempPath);
                        }

                        this.picProberRecipe.Image = new Bitmap(tempPath);
                    }

                    this.txtProberRecipeName.Text = DataCenter._uiSetting.ProberRecipeName;

                    this.gplProberInformation.Visible = true;
                }
                else
                {
                    this.gplProberInformation.Visible = false;
                }
            }
            else
            {
                this.btnManualRunMES.Enabled = false;
                this.btnManualRunMES.Visible = false;
                this.lblEMESEnable.Visible = false;
                this.gplProberInformation.Visible = false;
            }
			
			if ( DataCenter._uiSetting.UIDisplayType == (int) EUIDisplayType.MPIStartUI )		
            {
                this.cmbTaskSheet.Items.Clear();

                if (DataCenter._uiSetting.IsConverterTasksheet)
                {
                    this.cmbTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.MES_FILE_PATH, Constants.Files.TASK_SHEET_EXTENSION));
                    this.txtProductPath.Text = Constants.Paths.MES_FILE_PATH;
                }
                else
                {
                    this.cmbTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._uiSetting.ProductPath, Constants.Files.TASK_SHEET_EXTENSION));
                    this.txtProductPath.Text = DataCenter._uiSetting.ProductPath;
                }

                this.cmbTaskSheet.SelectedItem = DataCenter._uiSetting.TaskSheetFileName;
				this.txtProductFileName.Text = DataCenter._uiSetting.ProductFileName;
				this.txtBinDataFileName.Text = DataCenter._uiSetting.BinDataFileName;

                // Update By Recipe Setting 

                this.cmbByRecipeOutputPathCreatFolderType.Items.Clear();

                this.cmbByRecipeOutputPathCreatFolderType.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));

                this.cmbByRecipeOutputPathCreatFolderType.SelectedIndex = (int)DataCenter._uiSetting.ByRecipeResultCreateFolderType;

				if (this.UpDateUserIDFormEvent != null)
				{
					this.UpDateUserIDFormEvent(null, null);
				}

                this.txtTestResultPathByTaskSheet.Text = DataCenter._product.TestResultPathByTaskSheet;
			}
			else if ( DataCenter._uiSetting.UIDisplayType == (int) EUIDisplayType.WMStartUI )
			{

                this.cmbTaskSheet.Items.Clear();

                if (DataCenter._uiSetting.IsConverterTasksheet)
                {
                    this.cmbTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.MES_FILE_PATH, Constants.Files.TASK_SHEET_EXTENSION));
                    this.txtProductPath.Text = Constants.Paths.MES_FILE_PATH;
                }
                else
                {
                    this.cmbTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._uiSetting.ProductPath, Constants.Files.TASK_SHEET_EXTENSION));
                    this.txtProductPath.Text = DataCenter._uiSetting.ProductPath;
                }

                this.cmbTaskSheet.SelectedItem = DataCenter._uiSetting.TaskSheetFileName;
                this.txtProductFileName.Text = DataCenter._uiSetting.ProductFileName;
                this.txtBinDataFileName.Text = DataCenter._uiSetting.BinDataFileName;

                // Update By Recipe Setting 

                this.cmbByRecipeOutputPathCreatFolderType.Items.Clear();

                this.cmbByRecipeOutputPathCreatFolderType.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));

                this.cmbByRecipeOutputPathCreatFolderType.SelectedIndex = (int)DataCenter._uiSetting.ByRecipeResultCreateFolderType;

                if (this.UpDateUserIDFormEvent != null)
                {
                    this.UpDateUserIDFormEvent(null, null);
                }

                this.txtTestResultPathByTaskSheet.Text = DataCenter._product.TestResultPathByTaskSheet;

                /// combine




				this.cmbWMTaskSheet.Items.Clear();
				this.cmbWMTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.PRODUCT_FILE,Constants.Files.TASK_SHEET_EXTENSION));
				this.cmbWMTaskSheet.SelectedItem = DataCenter._uiSetting.TaskSheetFileName;
				
                this.txtWMOutputFileName.Text = DataCenter._uiSetting.WeiminUIData.KeyInFileName;

				this.txtWMProductFile.Text = DataCenter._uiSetting.ProductFileName;

				this.txtWMBinDataFile.Text = DataCenter._uiSetting.BinDataFileName;

				//------------------------------------------------------------------------
				// Weimin's Items
				//------------------------------------------------------------------------      
				this.txtWMSpecRemark.Text = DataCenter._uiSetting.WeiminUIData.SpecificationRemark;
				this.txtWMProductName.Text = DataCenter._uiSetting.WeiminUIData.ProductName;
				this.txtWMDeviceNumber.Text = DataCenter._uiSetting.WeiminUIData.DeviceNumber;
				this.txtWMLotNumber.Text = DataCenter._uiSetting.WeiminUIData.LotNumber;
				this.txtWMClassNumber.Text = DataCenter._uiSetting.WeiminUIData.ClassNumber;
				this.txtWMCodeNumber.Text = DataCenter._uiSetting.WeiminUIData.CodeNumber;
				this.txtWMSerialNumber.Text = DataCenter._uiSetting.WeiminUIData.SerialNumber;
				this.txtWMRemark01.Text = DataCenter._uiSetting.WeiminUIData.Remark01;
				this.txtWMRemark02.Text = DataCenter._uiSetting.WeiminUIData.Remark02;
				this.txtWMRemark03.Text = DataCenter._uiSetting.WeiminUIData.Remark03;
				this.txtWMRemark04.Text = DataCenter._uiSetting.WeiminUIData.Remark04;

				this.txtWMCustomerID.Text = DataCenter._uiSetting.WeiminUIData.CustomerID;
				this.txtWMCustomer.Text = DataCenter._uiSetting.WeiminUIData.Customer;
				this.txtWMCustomerNote01.Text = DataCenter._uiSetting.WeiminUIData.CustomerNote01;
				this.txtWMCustomerNote02.Text = DataCenter._uiSetting.WeiminUIData.CustomerNote02;
				this.txtWMCustomerNote03.Text = DataCenter._uiSetting.WeiminUIData.CustomerNote03;
				this.txtWMCustomerRemark01.Text = DataCenter._uiSetting.WeiminUIData.CustomerRemark01;

				this.txtWMSampleBins.Text = DataCenter._uiSetting.WeiminUIData.SampleBins;
				this.txtWMSampleStandard.Text = DataCenter._uiSetting.WeiminUIData.SampleStandard;
				this.txtWMSampleLevel.Text = DataCenter._uiSetting.WeiminUIData.SampleLevel;
				this.txtWMTotalTested.Text = DataCenter._uiSetting.WeiminUIData.TotalTested;
				this.txtWMSamples.Text = DataCenter._uiSetting.WeiminUIData.Samples;

				this.txtWMOperator.Text = DataCenter._uiSetting.OperatorName;

                this.txtBarcode.Text = DataCenter._uiSetting.Barcode;

                this.txtLotNumber.Text = DataCenter._uiSetting.LotNumber;

                this.txtWaferNumber.Text = DataCenter._uiSetting.WaferNumber;

                this.chkEnableClearWMData.Checked = DataCenter._uiSetting.WeiminUIData.IsAutoResetData;

                //switch (DataCenter._uiSetting.WeiminUIData.WMTestMode)
                //{ 
                //    case (int) EWMTestMode.FullyTest :
                //        this.rdbWMFullyTest.Checked = true;
                //        break;
                //    case (int) EWMTestMode.SampleTest :
                //        this.rdbWMSampleTest.Checked = true;
                //        break;
                //    case (int) EWMTestMode.ESDTest :
                //        this.rdbWMESDTest.Checked = true;
                //        break;
                //    case ( int) EWMTestMode.EngineerTest :
                //        this.rdbWMEngineerTest.Checked = true;
                //        break;
                //}
			}

            //ChangeUIControlsBySysState
            switch (DataCenter._uiSetting.UIOperateMode)
            {
                case (int)EUIOperateMode.Idle:
					this.ChangeAuthority();
                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
                    this.btnSave.Enabled = false;
                    this.btnNewTaskSheetFile.Enabled = false;
					this.btnOpenTaskSheetFile.Enabled = false;
                    this.btnSaveAsTaskSheetFile.Enabled = false;
                    this.btnDelTaskSheetFile.Enabled = false;
                    this.btnWMNewTaskSheetFile.Enabled = false;
                    this.btnWMSaveTaskSheetFile.Enabled = false;
                    this.btnWMDelTaskSheetFile.Enabled = false;
					this.gpbWMTestMode.Enabled = false;
					this.btnWMOpenTaskSheetFile.Enabled = false;
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

            if (DataCenter._uiSetting.IsConverterTasksheet)
            {
                this.btnNewTaskSheetFile.Enabled = false;
                this.btnSaveAsTaskSheetFile.Enabled = false;
                this.btnDelTaskSheetFile.Enabled = false;
            }

			if (DataCenter._uiSetting.IsTestResultPathByTaskSheet)
			{
               // UpdateUserFormat();//新版UI已使用其他方式處理
				this.gplOutputPathByRecipe.Visible = true;
			}
			else
			{
				this.gplOutputPathByRecipe.Visible = false;
			}
		}

        private void UpdateUserFormat()
        {
            this.cmbFormat.Items.Clear();

            if (DataCenter._uiSetting.UserDefinedData.FormatNames == null)
            {
                this.cmbFormat.Items.Add("NONE");
            }
            else
            {
                this.cmbFormat.Items.AddRange(DataCenter._uiSetting.UserDefinedData.FormatNames);
            }

            this.cmbFormat.SelectedItem = DataCenter._product.OutputFileFormat;

            if (!this.cmbFormat.Items.Contains(DataCenter._product.OutputFileFormat))
            {
                Host.SetErrorCode(EErrorCode.ByRecipeOutputFileFormatError);
            }
        }

        public void NewTaskSheetFile(string fileName)
		{
			if (File.Exists(Path.Combine(DataCenter._uiSetting.ProductPath, fileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
			{
				Host.SetErrorCode(EErrorCode.TaskSheetFileExisted);
				return;
			}

            ELOPSaveItem lopSaveItem = DataCenter._product.LOPSaveItem;
            DataCenter._uiSetting.TaskSheetFileName = fileName;
			DataCenter.CreateTaskSheet(fileName);
			DataCenter.CreateBinDataFile(fileName);
			DataCenter.CreateProductFile(fileName);
            DataCenter.CreateMapDataFile(fileName);
            DataCenter._product.LOPSaveItem = lopSaveItem;
			DataCenter.Save();

			Host.UpdateDataToAllUIForm();
		}
		
		public void SaveTaskSheetFile(string fileName)
		{
			if (File.Exists(Path.Combine(DataCenter._uiSetting.ProductPath, fileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
			{
				Host.SetErrorCode(EErrorCode.TaskSheetFileExisted);
				return ;
			}

			if ( fileName != DataCenter._uiSetting.TaskSheetFileName)
			{
				DataCenter.CreateTaskSheet(fileName);
				DataCenter._uiSetting.ProductFileName = fileName;
				DataCenter._uiSetting.BinDataFileName = fileName;
				DataCenter._uiSetting.MapDataFileName = fileName;

                if (DataCenter._uiSetting.ImportCalibrateFileName != "")
                {
                    DataCenter.ImportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
                }

				DataCenter.Save();
				Host.UpdateDataToAllUIForm();
			}
		}

        public void DelTaskSheetFile(string fileName)
		{
			string fileNameWithExt = fileName + "." + Constants.Files.TASK_SHEET_EXTENSION;
			MPIFile.DeleteFile(Path.Combine(DataCenter._uiSetting.ProductPath, fileNameWithExt));

			fileNameWithExt = fileName + "." + Constants.Files.PRODUCT_FILE_EXTENSION;
			MPIFile.DeleteFile(Path.Combine(DataCenter._uiSetting.ProductPath, fileNameWithExt));

			fileNameWithExt = fileName + "." + Constants.Files.BIN_FILE_EXTENSION;
			MPIFile.DeleteFile(Path.Combine(DataCenter._uiSetting.ProductPath, fileNameWithExt));

            fileNameWithExt = fileName + "." + Constants.Files.MAPDATA_FILE_EXTENSION;
            MPIFile.DeleteFile(Path.Combine(DataCenter._uiSetting.ProductPath, fileNameWithExt));
		}

		#endregion

		#region >>> Public Method <<<

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

		public void SaveWMData()
		{ 		
			DataCenter._uiSetting.Barcode = this.txtWMOutputFileName.Text;

			DataCenter._uiSetting.WeiminUIData.KeyInFileName = this.txtWMOutputFileName.Text;
			//------------------------------------------------------------------------
			// Weimin's Items
			//------------------------------------------------------------------------
			DataCenter._uiSetting.WeiminUIData.Specification = DataCenter._uiSetting.TaskSheetFileName;

			DataCenter._uiSetting.WeiminUIData.ProductName = this.txtWMProductName.Text;
			DataCenter._uiSetting.WeiminUIData.SpecificationRemark = this.txtWMSpecRemark.Text;
			DataCenter._uiSetting.WeiminUIData.DeviceNumber = this.txtWMDeviceNumber.Text;
			DataCenter._uiSetting.WeiminUIData.LotNumber = this.txtWMLotNumber.Text;
			DataCenter._uiSetting.WeiminUIData.ClassNumber = this.txtWMClassNumber.Text;
			DataCenter._uiSetting.WeiminUIData.CodeNumber = this.txtWMCodeNumber.Text;
			DataCenter._uiSetting.WeiminUIData.SerialNumber = this.txtWMSerialNumber.Text;
			DataCenter._uiSetting.WeiminUIData.Remark01 = this.txtWMRemark01.Text;
			DataCenter._uiSetting.WeiminUIData.Remark02 = this.txtWMRemark02.Text;
			DataCenter._uiSetting.WeiminUIData.Remark03 = this.txtWMRemark03.Text;
			DataCenter._uiSetting.WeiminUIData.Remark04 = this.txtWMRemark04.Text;

			DataCenter._uiSetting.WeiminUIData.CustomerID = this.txtWMCustomerID.Text;
			DataCenter._uiSetting.WeiminUIData.Customer = this.txtWMCustomer.Text;
			//DataCenter._uiSetting.WeiminUIData.CustomerNote01 = this.txtWMCustomerNote01.Text;
			DataCenter._uiSetting.WeiminUIData.CustomerNote02 = this.txtWMCustomerNote02.Text;
			DataCenter._uiSetting.WeiminUIData.CustomerNote03 = this.txtWMCustomerNote03.Text;
			//DataCenter._uiSetting.WeiminUIData.CustomerRemark01 = this.txtWMCustomerRemark01.Text;

			DataCenter._uiSetting.WeiminUIData.SampleBins = this.txtWMSampleBins.Text;
			DataCenter._uiSetting.WeiminUIData.SampleStandard = this.txtWMSampleStandard.Text;
			DataCenter._uiSetting.WeiminUIData.SampleLevel = this.txtWMSampleLevel.Text;
			DataCenter._uiSetting.WeiminUIData.TotalTested = this.txtWMTotalTested.Text;
			DataCenter._uiSetting.WeiminUIData.Samples = this.txtWMSamples.Text;

            DataCenter._uiSetting.WeiminUIData.IsAutoResetData = this.chkEnableClearWMData.Checked;

            //if (rdbWMFullyTest.Checked == true)
            //{
            //    DataCenter._uiSetting.WeiminUIData.WMTestMode = (int)EWMTestMode.FullyTest;
            //    //DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.BarCode;
            //    //DataCenter._uiSetting.IsEnablePath01 = true;
            //    //DataCenter._uiSetting.IsEnablePath02 = true;
            //}
            //else if (rdbWMSampleTest.Checked == true)
            //{
            //    DataCenter._uiSetting.WeiminUIData.WMTestMode = (int)EWMTestMode.SampleTest;
            //    //DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.Customer01;
            //    //DataCenter._uiSetting.IsEnablePath01 = true;
            //    //DataCenter._uiSetting.IsEnablePath02 = true;
            //}
            //else if (rdbWMESDTest.Checked == true)
            //{
            //    DataCenter._uiSetting.WeiminUIData.WMTestMode = (int)EWMTestMode.ESDTest;
            //    //DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.Customer02;
            //    //DataCenter._uiSetting.IsEnablePath01 = true;
            //    //DataCenter._uiSetting.IsEnablePath02 = true;
            //}
            //else if (rdbWMEngineerTest.Checked == true)
            //{
            //    DataCenter._uiSetting.WeiminUIData.WMTestMode = (int)EWMTestMode.EngineerTest;
            //    //DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.BarCode;
            //    //DataCenter._uiSetting.IsEnablePath01 = true;
            //    //DataCenter._uiSetting.IsEnablePath02 = false;
            //}	

			DataCenter.SaveUISettingToFile();
		}

		public void CreatUserIDForm()
		{
			if (this._frmUserID != null)
			{
				this._frmUserID.Close();
				this._frmUserID.Dispose();
			}

			switch (DataCenter._uiSetting.UserID)
			{
				case EUserID.HelioOpto:
					this._frmUserID = new frmHelio();
					break;
				case EUserID.Epileds:
					this._frmUserID = new frmEpiLEDs();
					break;
				case EUserID.LPC00:
					this._frmUserID = new frmLPC00();
					break;
				case EUserID.KAISTAR:
					this._frmUserID = new frmKAISTAR();
					break;
				case EUserID.TSMC00:
					this._frmUserID = new frmTSMC00();
					break;
				//-------------------------------------------------------------------
				default:
					this._frmUserID = new frmDefault();
					break;
			}

			this._frmUserID.TopLevel = false;
			this._frmUserID.Parent = this.tabpMPI;
			this._frmUserID.Dock = DockStyle.None;
			this._frmUserID.Location = new Point(5, 10);
			this._frmUserID.Show();
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmSetRecipe_Load(object sender, EventArgs e)
		{
			this.UpdateDataToControls();

			this.CreatUserIDForm();
		}

		private void frmSetRecipe_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				this.UpdateDataToControls();
			}
		}		
		
		private void btnOpenTaskSheetFile_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnOpenTaskSheetFile_Click", this.cmbTaskSheet.Text);
			
			GlobalFlag.IsSuccessCheckFilterWheel = true;

            DataCenter.LoadTaskSheet(this.cmbTaskSheet.Text);

			WMOperate.WM_ReadCalibrateParamFromSetting();

            AppSystem.SetDataToSystem();

            AppSystem.CheckMachineHW();

			Host.UpdateDataToAllUIForm();
		}

		private void btnNewTaskSheetFile_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnNewTaskSheetFile_Click");

            DataCenter._uiSetting.ControlTaskSettingUI = EControlTaskSetting.NEW;

            _frmSetTaskSheet.ShowDialog();

			if (DataCenter._uiSetting.SendTaskFileName == "")
				return;
            //
            this.NewTaskSheetFile(DataCenter._uiSetting.SendTaskFileName);

            if (!Host._MPIStorage.SaveTestCoefficientToFile())
            {
                Host.SetErrorCode(EErrorCode.SaveWatchCoefficientFileFail);
            }


            WMOperate.WM_ReadCalibrateParamFromSetting();
			//Host.UpdateDataToAllUIForm();
		}

		private void btnSaveAsTaskSheetFile_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnSaveAsTaskSheetFile_Click");

            DataCenter._uiSetting.ControlTaskSettingUI = EControlTaskSetting.SAVEAS;

            _frmSetTaskSheet.ShowDialog();

            if (DataCenter._uiSetting.SendTaskFileName == "")
                return;

            this.SaveTaskSheetFile(DataCenter._uiSetting.SendTaskFileName);

            if (!Host._MPIStorage.SaveTestCoefficientToFile())
            {
                Host.SetErrorCode(EErrorCode.SaveWatchCoefficientFileFail);
            }

            WMOperate.WM_ReadCalibrateParamFromSetting();
			//Host.UpdateDataToAllUIForm();	
		}

		private void btnDelTaskSheetFile_Click(object sender, EventArgs e)
		{
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.DeleteTaskSheet, "Would you delete Selceted Recipe? ", "Qusetion ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            //DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Delete Selceted Recipe ? ", "Qusetion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            if (this.cmbWMTaskSheet.Items.Count == 1)
            {
                return;
            }
            else if (this.cmbWMTaskSheet.Items.Count > 1)
            {
				UILog.Log(this, sender, "btnDelTaskSheetFile_Click", this.cmbTaskSheet.Text);

                this.DelTaskSheetFile(this.cmbTaskSheet.Text);
                this.cmbTaskSheet.Items.Remove(this.cmbTaskSheet.Text);
                this.cmbTaskSheet.SelectedIndex = 0;
                DataCenter.LoadTaskSheet(this.cmbTaskSheet.Text);
            }

			Host.UpdateDataToAllUIForm();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnSave_Click");

			if (DataCenter._uiSetting.IsTestResultPathByTaskSheet)
			{
                DataCenter._product.TestResultPathByTaskSheet = this.txtTestResultPathByTaskSheet.Text;

                DataCenter.SaveProductFile();
			}

			if (DataCenter._uiSetting.UIDisplayType == (int)EUIDisplayType.MPIStartUI)
			{
				this.SaveMPIData(true);
			}
			else if (DataCenter._uiSetting.UIDisplayType == (int)EUIDisplayType.WMStartUI)
			{
				this.SaveWMData();
			}

            Host.UpdateDataToAllUIForm();

			GlobalData.ProberRecipeName = DataCenter._uiSetting.ProberRecipeName;

            Host._MPIStorage.SaveReportHeadToFile();

			if (DataCenter._uiSetting.UserID == EUserID.KAISTAR)
			{
				UILog.Log(this, sender, "btnManualRunMES_Click");

				EErrorCode errorCode = MESCtrl.LoadRecipe();

				Host.SetErrorCode(errorCode);
			}

			if (DataCenter._uiSetting.UserID == EUserID.GPI)
			{
				UILog.Log(this, sender, "btnManualRunMES_Click");

				EErrorCode errorCode = MESCtrl.LoadRecipe();

				Host.SetErrorCode(errorCode);
			}
		}

		private void btnWMOpenTaskSheetFile_Click(object sender, EventArgs e)
		{
            DataCenter.LoadTaskSheet(this.cmbWMTaskSheet.Text);

            WMOperate.WM_ReadCalibrateParamFromSetting();

            AppSystem.SetDataToSystem();

            AppSystem.CheckMachineHW();

            Host.UpdateDataToAllUIForm();	
		}

		private void btnWMNewTaskSheetFile_Click(object sender, EventArgs e)
		{
			this.NewTaskSheetFile( this.cmbWMTaskSheet.Text);
			WMOperate.WM_ReadCalibrateParamFromSetting();
			Host.UpdateDataToAllUIForm();	
		}

		private void btnWMSaveTaskSheetFile_Click(object sender, EventArgs e)
		{
			this.SaveTaskSheetFile(this.cmbWMTaskSheet.Text);
			WMOperate.WM_ReadCalibrateParamFromSetting();
			Host.UpdateDataToAllUIForm();
		}

		private void btnWMDelTaskSheetFile_Click(object sender, EventArgs e)
		{
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.DeleteTaskSheet, "Would you delete Selceted Recipe? ", "Qusetion ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            //DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Delete Selceted Recipe ? ", "Qusetion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

			if ( this.cmbWMTaskSheet.Items.Count == 1 )
			{
				return;
			}
			else if (this.cmbWMTaskSheet.Items.Count > 1)
			{
				this.DelTaskSheetFile(this.cmbWMTaskSheet.Text);
				this.cmbWMTaskSheet.Items.Remove(this.cmbWMTaskSheet.Text);
				this.cmbWMTaskSheet.SelectedIndex = 0;
				DataCenter.LoadTaskSheet(this.cmbWMTaskSheet.Text);
			}
			else
			{
				this.DelTaskSheetFile(this.cmbWMTaskSheet.Text);
				DataCenter.LoadTaskSheet(Constants.Files.DEFAULT_FILENAME);
			}
			Host.UpdateDataToAllUIForm();
		}

        private void btnResetState_Click(object sender, EventArgs e)
        {
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Do you want to reset system state ? ", "Warn", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

			UILog.Log(this, sender, "btnResetState_Click");

            DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.ManulRun;
            Host.UpdateDataToAllUIForm();
        }

		public void SaveMPIData(bool GenerateOutputFileName)
        {
			if (this.OpRecipeSaveEvent != null)
			{
				this.OpRecipeSaveEvent(null, null);
			}

			if (GenerateOutputFileName)
			{
				Host._MPIStorage.GenerateOutputFileName();
			}

            DataCenter.SaveUISettingToFile();
        }

        private void btnManualRunMES_Click(object sender, EventArgs e)
        {
			UILog.Log(this, sender, "btnManualRunMES_Click");

            //EErrorCode errorCode = MESControl.MESRun();
            EErrorCode errorCode = MESCtrl.LoadRecipe();
            Host.SetErrorCode(errorCode);
            //DataCenter.Save();
           // AppSystem.SetDataToSystem();
          //   Host.UpdateDataToAllUIForm();
        }
              
		private void btnTestResultSelect_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

			folderBrowserDialog.Description = "Test Output Path";

			folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;

			folderBrowserDialog.SelectedPath = Constants.Paths.ROOT;

			if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
				return;

			string path = folderBrowserDialog.SelectedPath;

			if (path != string.Empty)
			{
				if ((path.Length >= Constants.Paths.ROOT.Length) &&
					(path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
				{
					Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
				}
				else
				{
					this.txtTestResultPathByTaskSheet.Text = path;
				}
			}

		}

        #endregion

        private void cmbByRecipeOutputPathCreatFolderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataCenter._uiSetting.ByRecipeResultCreateFolderType = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType), this.cmbByRecipeOutputPathCreatFolderType.SelectedItem.ToString(), true);

            DataCenter.SaveUISettingToFile();          

        }

        private void cmbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataCenter._product.OutputFileFormat = this.cmbFormat.SelectedItem.ToString();

            DataCenter.SaveProductFile();
        }
        
        private void btnParseSingleDieRecipeToMultiDie_Click(object sender, EventArgs e)
        {
            DataCenter._product.TestCondition.ChannelConditionTable.ColXCount= DataCenter._machineConfig.ChannelConfig.ColXCount;

            DataCenter._product.TestCondition.ChannelConditionTable.RowYCount= DataCenter._machineConfig.ChannelConfig.RowYCount;

            DataCenter.SaveProductFile();

            DataCenter.LoadTaskSheet(this.cmbTaskSheet.Text);

            WMOperate.WM_ReadCalibrateParamFromSetting();

            AppSystem.SetDataToSystem();

            AppSystem.CheckMachineHW();

            Host.UpdateDataToAllUIForm();

        }


		#region >>> Public Property <<<

        //public Form frmSetTaskSheet
        //{
        //    get { return this._frmSetTaskSheet; }
        //}

		#endregion
	}
}