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
    public partial class frmAccelinkPath : Form
    {
        public frmAccelinkPath()
        {
            InitializeComponent();
        }


        #region>>public method<<
        
        public bool SaveDataToDataCenter()
        {
            DataCenter._uiSetting.MergeFilePath = UI2TDPath(pathUIComponent1.PathInfomation);
            DataCenter._uiSetting.UIMapPathInfo = UI2TDPath(pucWaferMap.PathInfomation);
            return true;
        }
        public bool LoadDataFromDataCenter()
        {
            
            pathUIComponent1.PathInfomation = TD2UIPath(DataCenter._uiSetting.MergeFilePath.Clone() as MPI.Tester.Data.PathInfo);
            pucWaferMap.PathInfomation = TD2UIPath(DataCenter._uiSetting.UIMapPathInfo.Clone() as MPI.Tester.Data.PathInfo);
            return true;
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
            using (OpenFileDialog od = new OpenFileDialog())
            {
                od.Multiselect = true;
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (SaveFileDialog sd = new SaveFileDialog())
                    {
                        sd.Filter = DataCenter._uiSetting.TestResultFileExt.ToUpper() +
                            " (*." + DataCenter._uiSetting.TestResultFileExt + ")|*." + DataCenter._uiSetting.TestResultFileExt;
                        sd.DefaultExt = DataCenter._uiSetting.TestResultFileExt;
                        sd.AddExtension = true;

                        if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            string saveFilePAth = sd.FileName;
                            List<string> fileList = new List<string>();
                            if (od.FileNames != null && od.FileNames.Length > 1)
                            {

                                List<string> sList = new List<string>();
                                sList.AddRange(od.FileNames);
                                AppSystem.MergeFile(saveFilePAth, sList);
                            }
                        }
                    }
                }
            }
        }

       
    }
}
