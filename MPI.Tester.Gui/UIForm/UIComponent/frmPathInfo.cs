using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MPI.Tester.Data;

namespace MPI.Tester.Gui.UIForm.UIComponent
{
    public partial class frmPathInfo : Form
    {
        PathInfo _pInfo;
        public frmPathInfo()
        {
            InitializeComponent();

            cmbFolderType.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));
            cmbFolderType.SelectedIndex = 0;
        }

        public frmPathInfo(string pathName, PathInfo pInfo,bool isShowEnable =true,bool isShowFolderType = false,bool IsShowExt = false):this()
        {
            lblPathName.Text = pathName;
            _pInfo = pInfo;
            RefreshUI(isShowEnable,isShowFolderType, IsShowExt);
            Load(_pInfo);

        }

        public void RefreshUI(bool isShowEnable,bool isShowFolderType,bool IsShowExt)
        {

            if (!isShowEnable)
            {
                chkEnablePath.Checked = true;

                chkEnablePath.Visible = false;
            }


            if (isShowFolderType && !IsShowExt )
            {

                cmbFolderType.Width = 200;
                txtExt.Visible = false;
            }
            else if (!isShowFolderType && IsShowExt)
            {
                cmbFolderType.Visible = false;
            }
            else if (!isShowFolderType && !IsShowExt)
            {
                txtPath.Width = 460;
                cmbFolderType.Visible = false;
                txtExt.Visible = false;
            }


        }

        public void Save()
        {
            _pInfo.EnablePath = chkEnablePath.Checked;
            _pInfo.FileExt = txtExt.Text;
            _pInfo.FolderType = (ETesterResultCreatFolderType)Enum.Parse(typeof(ETesterResultCreatFolderType),cmbFolderType.Text.ToString());
            _pInfo.TestResultPath = txtPath.Text;
        }
        public void Load(PathInfo pInfo)
        {
            _pInfo = pInfo;

            chkEnablePath.Checked = _pInfo.EnablePath ;
            txtExt.Text = _pInfo.FileExt;
            txtPath.Text = _pInfo.TestResultPath;
            cmbFolderType.SelectedIndex = (int)_pInfo.FolderType;
            
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath(lblPathName.Text);
            if (path != string.Empty)
            {
                if ((path.Length >= Constants.Paths.ROOT.Length) &&
                    (path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
                {
                    Host.SetErrorCode(EErrorCode.SaveFilePathSetingFail);
                }
                else
                {
                    this.txtPath.Text = path;
                }
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
    }
}
