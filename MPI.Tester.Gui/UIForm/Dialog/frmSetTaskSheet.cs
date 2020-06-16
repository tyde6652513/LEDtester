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
    public partial class frmSetTaskSheet : Form
    {
        public frmSetTaskSheet()
        {
            InitializeComponent();
        }

        

        private void frmSetTaskSheet_Load(object sender, EventArgs e)
        {
            this.cmbImportCalibrateFileNames.Items.Clear();
            this.cmbImportBinFileNames.Items.Clear();
            this.txtNewProductFileName.Enabled = true;
            this.grpTaskName.Text = string.Empty;

            switch (DataCenter._uiSetting.ControlTaskSettingUI)
            {
                case EControlTaskSetting.NONE:
                case EControlTaskSetting.NEW:
                    this.cmbImportCalibrateFileNames.Text = "";
                    this.cmbImportBinFileNames.Text = "";
                    this.plCMFormat.Visible = false;
                    this.grpTaskName.Text = "NEW";

					break;
                case EControlTaskSetting.SAVEAS:     
      					this.cmbImportCalibrateFileNames.Visible = true;
						this.labelX28.Visible = true;
                        this.cmbImportCalibrateFileNames.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.PRODUCT_FILE02, "cal"));       
                        this.cmbImportBinFileNames.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.PRODUCT_FILE02, "sr2"));
                        this.cmbImportCalibrateFileNames.Text = DataCenter._uiSetting.ImportCalibrateFileName;
                        this.cmbImportBinFileNames.Text = DataCenter._uiSetting.ImportBinFileName;
                        this.plCMFormat.Visible = false;
                        this.grpTaskName.Text = "SAVE AS";
                        break;
                case EControlTaskSetting.UPDATE:
					    this.cmbImportCalibrateFileNames.Visible = true;
						this.labelX28.Visible = true;
						this.cmbImportCalibrateFileNames.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.PRODUCT_FILE02, "cal"));       
                        this.cmbImportBinFileNames.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.PRODUCT_FILE02, "sr2"));
                        this.cmbImportCalibrateFileNames.Text = DataCenter._uiSetting.ImportCalibrateFileName;
                        this.cmbImportBinFileNames.Text = DataCenter._uiSetting.ImportBinFileName;
                        this.txtNewProductFileName.Text = DataCenter._uiSetting.TaskSheetFileName;
                        this.txtNewProductFileName.Enabled = false;

                        if (DataCenter._uiSetting.UserDefinedData.IsEnableShowCalibrateFileLink) //1 :Emable //0 Disable
                        {
                            this.plCMFormat.Visible = true;
                        }
                        else
                        {
                            this.plCMFormat.Visible = false;
                        }

                        break;
                case EControlTaskSetting.TSMC:
						this.cmbImportCalibrateFileNames.Visible = false;
						this.labelX28.Visible = false;
						this.cmbImportBinFileNames.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.PRODUCT_FILE02, "bin"));

                        if (DataCenter._uiSetting.UserDefinedData.IsEnableShowCalibrateFileLink ) //1 :Emable //0 Disable
                        {
                            this.plCMFormat.Visible = true;
                        }
                        else
                        {
                            this.plCMFormat.Visible = false;
                        }
						break;
                default:
                    break;
            }

            this.TopMost = true;
          
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //UILog.Log(this, sender, "btnConfirm_Click", "TS:" + this.txtNewProductFileName.Text);

            if (DataCenter._uiSetting.ControlTaskSettingUI == EControlTaskSetting.TSMC)
            {
                if (this.cmbImportBinFileNames.Text == "")
                {
                    MessageBox.Show("File Name Can't be Empty!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    return;
                }

                if (File.Exists(Path.Combine(DataCenter._uiSetting.ProductPath, this.txtNewProductFileName.Text + "." + Constants.Files.TASK_SHEET_EXTENSION)))
                {
                    MessageBox.Show("File Already Exists!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    return;
                }
            }

            DataCenter._uiSetting.SendTaskFileName = this.txtNewProductFileName.Text;
            DataCenter._uiSetting.ImportCalibrateFileName = this.cmbImportCalibrateFileNames.Text;

            if (DataCenter._uiSetting.ControlTaskSettingUI == EControlTaskSetting.TSMC)
            {
                DataCenter._uiSetting.ProductFileName = this.txtNewProductFileName.Text;
                DataCenter._uiSetting.BinDataFileName = this.cmbImportBinFileNames.Text;
            }
            else
            {
                DataCenter._uiSetting.ImportBinFileName = this.cmbImportBinFileNames.Text;
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
			UILog.Log(this, sender, "btnCancel_Click");

            this.txtNewProductFileName.Text = "";

            DataCenter._uiSetting.SendTaskFileName = "";

            this.Close();
        }

    }
}
