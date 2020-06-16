using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public partial class frmEpiLEDs : System.Windows.Forms.Form
	{
		public frmEpiLEDs()
		{
			InitializeComponent();
		}

		private void frmEpiLEDs_Load(object sender, EventArgs e)
		{
			this.LoadData(null, null);

			FormAgent.RecipeForm.OpRecipeSaveEvent += new EventHandler<EventArgs>(this.SaveData);

			FormAgent.RecipeForm.UpDateUserIDFormEvent += new EventHandler<EventArgs>(this.LoadData);
		}

		private void LoadData(object sender, EventArgs e)
		{
			this.txtTestResultPath.Text = DataCenter._uiSetting.TestResultPath01;
			this.txtOutputFileName.Text = DataCenter._uiSetting.TestResultFileName;
			this.txtProductType.Text = DataCenter._uiSetting.ProductType;
			this.txtPcs.Text = DataCenter._uiSetting.WaferPcs.ToString("00");
			this.txtLotNumber.Text = DataCenter._uiSetting.LotNumber;
			this.txtWaferNumber.Text = DataCenter._uiSetting.WaferNumber;
			this.txtOperatorName.Text = DataCenter._uiSetting.OperatorName;

			this.lblAutoRm2Path.Text = DataCenter._uiSetting.AutoRm2Path;
			this.lblRm2FileName.Text = DataCenter._uiSetting.RM2FileName;
			this.lblImportCalibrateFileNames.Text = DataCenter._uiSetting.ImportCalibrateFileName;
			this.lblImportBinFileNames.Text = DataCenter._uiSetting.MapGradeFileName;
			this.lblSptCalibFilePath.Text = DataCenter._uiSetting.ImportCalibSptDataFileName;
			this.txtEquipmentID.Text = DataCenter._uiSetting.MachineName;

			this.plCMFormat.Visible = true;
			this.lblSptCalibFilePath.Text = DataCenter._product.SptCalibPathAndFile;
			this.lblImportCalibrateFileNames.Text = DataCenter._uiSetting.ImportCalibrateFileName;
			//   this.lblImportBinFileNames.Text = DataCenter._uiSetting.ImportBinFileName;
			this.lblAutoRm2Path.Text = DataCenter._uiSetting.AutoRm2Path;
			this.lblRm2FileName.Text = DataCenter._uiSetting.RM2FileName;
			this.txtProductFileName.Text = DataCenter._uiSetting.ProductFileName;
		}

		private void SaveData(object o, EventArgs e)
		{
			DataCenter._uiSetting.TestResultPath01 = this.txtTestResultPath.Text;

			DataCenter._uiSetting.TestResultFileName = this.txtOutputFileName.Text;

			DataCenter._uiSetting.ProductType = this.txtProductType.Text;

			//DataCenter._uiSetting.WaferPcs = int.Parse(this.txtPcs.Text); //ReadOnly

			DataCenter._uiSetting.LotNumber = this.txtLotNumber.Text;

			DataCenter._uiSetting.WaferNumber = this.txtWaferNumber.Text;

			DataCenter._uiSetting.OperatorName = this.txtOperatorName.Text;
		}

		private void btnUpdateTaskSheetFile_Click(object sender, EventArgs e)
		{
			DataCenter._uiSetting.ControlTaskSettingUI = EControlTaskSetting.UPDATE;

            frmOpRecipe._frmSetTaskSheet.ShowDialog();

			if (DataCenter._uiSetting.SendTaskFileName == "")
				return;

			WMOperate.WM_ReadCalibrateParamFromSetting();
			// Import Calibration // BIN
			DataCenter.CreateTaskSheet(DataCenter._uiSetting.SendTaskFileName);

			if (DataCenter._uiSetting.ImportCalibrateFileName != "")
			{
				DataCenter.ImportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
			}
			//if (DataCenter._uiSetting.ImportBinFileName != "")
			//{
			//    DataCenter.ImportBinTable(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportBinFileName + ".sr2");
			//}
			DataCenter.Save();
			Host.UpdateDataToAllUIForm();
		}

		private void btnManualRunMES_Click(object sender, EventArgs e)
		{
			if (DataCenter._uiSetting.IsEnableRunMesSystem)
			{
				DataCenter._uiSetting.RM2FileName = "";
				//EErrorCode errorCode = MESControl.MESRun();
                string message = string.Empty;

                EErrorCode errorCode = MES.MESProcess.LoadRecipe(DataCenter._uiSetting, DataCenter._machineConfig,  out message);

				Host.SetErrorCode(errorCode);

                Host._alarmDescribe.Append(message);

				DataCenter.Save();

				Host.UpdateDataToAllUIForm();

                this.plCMFormat.Visible = true;
                this.lblSptCalibFilePath.Text = DataCenter._product.SptCalibPathAndFile;
                this.lblImportCalibrateFileNames.Text = DataCenter._uiSetting.ImportCalibrateFileName;
                this.lblImportBinFileNames.Text = DataCenter._uiSetting.ImportBinFileName;
                this.lblAutoRm2Path.Text = DataCenter._uiSetting.AutoRm2Path;
                this.lblRm2FileName.Text = DataCenter._uiSetting.RM2FileName;
                this.txtProductType.Text = DataCenter._uiSetting.ProductFileName;
                this.txtProductFileName.Text = DataCenter._uiSetting.ProductFileName;
			}
		}

		private void btnAutoRm2PathSelect_Click(object sender, EventArgs e)
		{
			string path = this.SelectPath("Auto Rm2 Path");
			if (path != string.Empty)
			{
				this.lblAutoRm2Path.Text = path;
                DataCenter._uiSetting.AutoRm2Path = path;
                DataCenter.SaveUISettingToFile();
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

        private void btnSaveCalibrationFile_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export CoefTable Data to File";
            saveFileDialog.Filter = "CSV files (*.cal)|*.cal";
            saveFileDialog.FilterIndex = 1;    // default value = 1
            saveFileDialog.InitialDirectory = DataCenter._uiSetting.ProductPath02;
            saveFileDialog.FileName = DataCenter._uiSetting.TaskSheetFileName;

            if (DataCenter._uiSetting.ImportCalibrateFileName != "")
            {
                saveFileDialog.FileName = DataCenter._uiSetting.ImportCalibrateFileName;
            }

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            DataCenter.ExportCalibrateData(Path.GetDirectoryName(saveFileDialog.FileName), Path.GetFileName(saveFileDialog.FileName));
        }


	}
}
