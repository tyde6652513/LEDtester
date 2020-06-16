using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public partial class frmUploadRecipe : System.Windows.Forms.Form
	{
		public frmUploadRecipe()
		{
			InitializeComponent();
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

        private void frmUploadRecipe_Load(object sender, EventArgs e)
        {
            this.txtUploadProductPath.Text = DataCenter._uiSetting.UploadProductFilePath;

            this.txtUploadMapPath.Text = DataCenter._uiSetting.UploadMapFilePath;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
				DataCenter._uiSetting.UploadProductFilePath = this.txtUploadProductPath.Text;

				DataCenter._uiSetting.UploadMapFilePath = this.txtUploadMapPath.Text;

                string localProduct = Path.Combine(DataCenter._uiSetting.ProductPath, DataCenter._uiSetting.ProductFileName + "." + Constants.Files.PRODUCT_FILE_EXTENSION);

                string localMap = Path.Combine(DataCenter._uiSetting.ProductPath, DataCenter._uiSetting.MapDataFileName + "." + Constants.Files.MAPDATA_FILE_EXTENSION);

                string serverProduct = Path.Combine(this.txtUploadProductPath.Text, DataCenter._uiSetting.ProductFileName + "." + Constants.Files.PRODUCT_FILE_EXTENSION);

                string serverMap = Path.Combine(this.txtUploadMapPath.Text, DataCenter._uiSetting.MapDataFileName + "." + Constants.Files.MAPDATA_FILE_EXTENSION);

                if (!Directory.Exists(this.txtUploadProductPath.Text))
                {
                    Directory.CreateDirectory(this.txtUploadProductPath.Text);
                }

                if (!Directory.Exists(this.txtUploadMapPath.Text))
                {
                    Directory.CreateDirectory(this.txtUploadMapPath.Text);
                }

                if (DataCenter._uiSetting.UserID == EUserID.GPI)
                {
                    serverProduct = Path.Combine(this.txtUploadProductPath.Text, DataCenter._uiSetting.ProductFileName + ".mcg");

                    serverMap = Path.Combine(this.txtUploadMapPath.Text, DataCenter._uiSetting.MapDataFileName + ".gcg");
                }

                if (this.chkIsEnableUploadProduct.Checked)
                {
                    MPIFile.CopyFile(localProduct, serverProduct);
                }

                if (this.chkIsEnableUploadMap.Checked)
                {
                    MPIFile.CopyFile(localMap, serverMap);
                }

				DataCenter.SaveUISettingToFile();

                MessageBox.Show("Upload Rceipe Finish");
            }
            catch
            {
                MessageBox.Show("Upload File Fail");
            }
        }

        private void btnTestResultSelect01_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Product Upload Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtUploadProductPath.Text = path;
                }
            }
        }

        private void btnTestResultSelect02_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Map Upload Path");
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtUploadMapPath.Text = path;
                }
            }
        }
	}
}
