using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Reflection;
using MPI.Tester.Data;
using MPI.Tester.Gui;

namespace MPI.Tester.Gui.UIForm.UserForm.UISetting
{
    public partial class frmOptoTechPath : Form
    {
        public frmOptoTechPath()
        {
            InitializeComponent();
        }

        // public frmOptoTechPath(PathInfo pInfo)
        //    : this()
        //{
        //    SetData(pInfo);
        //}

        #region>>public method<<
        //public void SetData(PathInfo pInfo)
        //{
        //    pathUIComponent1.PathInfomation = TD2UIPath(pInfo);
        //}

        //public PathInfo GetData()
        //{
        //    PathInfo pInfo = UI2TDPath(pathUIComponent1.PathInfomation);

        //    return pInfo;
        //}

        public void SetKeyInDataPath(string path)
        {
            txtKeyInDataPath.Text = path;
        }
        public bool LoadDataFromDataCenter()
        {
            pathUIComponent1.PathInfomation = TD2UIPath(DataCenter._uiSetting.MergeFilePath.Clone() as MPI.Tester.Data.PathInfo);

            txtKeyInDataPath.Text = DataCenter._uiSetting.OptoTechKeyInDataPath;

            pucLaserPower.PathInfomation = TD2UIPath(DataCenter._uiSetting.LaserPowerLogPath.Clone() as MPI.Tester.Data.PathInfo);
            return true;
        }
        public bool SaveDataToDataCenter()
        {
            DataCenter._uiSetting.MergeFilePath = UI2TDPath(pathUIComponent1.PathInfomation);
            DataCenter._uiSetting.LaserPowerLogPath = UI2TDPath(pucLaserPower.PathInfomation);
            DataCenter._uiSetting.OptoTechKeyInDataPath = GetKeyInDataPath();

            return true;
        }
        
        public string GetKeyInDataPath()
        {
            return txtKeyInDataPath.Text;
        }
        #endregion
        #region >>private method<<
        private static MPI.Tester.GuiComponent.PathInfo TD2UIPath(MPI.Tester.Data.PathInfo pInfo)
        {
            MPI.Tester.GuiComponent.PathInfo uiPInfo = new GuiComponent.PathInfo();

            uiPInfo.EnablePath = pInfo.EnablePath;
            uiPInfo.FileExt = pInfo.FileExt;
            string enumName = pInfo.FolderType.ToString();
            MPI.Tester.GuiComponent.ETesterResultCreatFolderType foo = (MPI.Tester.GuiComponent.ETesterResultCreatFolderType)Enum.Parse(typeof(MPI.Tester.GuiComponent.ETesterResultCreatFolderType), enumName);
            uiPInfo.FolderType = foo;
            uiPInfo.PathName = pInfo.PathName;
            uiPInfo.TestResultPath = pInfo.TestResultPath;

            return uiPInfo;
        }

        private static MPI.Tester.Data.PathInfo UI2TDPath(MPI.Tester.GuiComponent.PathInfo uiPInfo)
        {

            MPI.Tester.Data.PathInfo pInfo = new MPI.Tester.Data.PathInfo();

            pInfo.EnablePath = uiPInfo.EnablePath;
            pInfo.FileExt = uiPInfo.FileExt;
            string enumName = uiPInfo.FolderType.ToString();
            MPI.Tester.Data.ETesterResultCreatFolderType foo = (MPI.Tester.Data.ETesterResultCreatFolderType)Enum.Parse(typeof(MPI.Tester.Data.ETesterResultCreatFolderType), enumName);
            pInfo.FolderType = foo;
            pInfo.PathName = uiPInfo.PathName;
            pInfo.TestResultPath = uiPInfo.TestResultPath;

            return pInfo;
        }
        #endregion

        private void btnMerge_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog od  = new OpenFileDialog())
            {
                od.Multiselect = true;
                if(od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    List<string> fileList = new List<string>();
                    if (od.FileNames != null && od.FileNames.Length > 1)
                    {

                        string folderName = DataCenter._uiSetting.GetPathWithFolder(UI2TDPath(pathUIComponent1.PathInfomation));

                        string mapFileName = DataCenter._uiSetting.TestResultFileName;
                        string file = Path.Combine(folderName, mapFileName) + "." + DataCenter._uiSetting.TestResultFileExt;

                        List<string> sList = new List<string>();
                        sList.AddRange(od.FileNames );
                        AppSystem.MergeFile(file, sList);
                    }
                    //AppSystem.MergeFile()
                }
            }

            
            //ReportProcess.
        }

        private void btnKeyInData_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog od = new OpenFileDialog())
            {
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = od.FileName;
                    txtKeyInDataPath.Text = path;
                    
                }
            }
        }
    }
}
