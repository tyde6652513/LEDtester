using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;

using System.Reflection;
using MPI.Tester.Data;

namespace MPI.Tester.Gui.UIForm.UserForm.UISetting
{
    public partial class frmDowaPath : Form
    {
        const int PATH_QTY = 12;
        //PathArr _pArr;
        public frmDowaPath()
        {
            InitializeComponent();

            this.cmbFileNamePresent.Items.AddRange(Enum.GetNames(typeof(EOutputFileNamePresent)));
            this.cmbFileNamePresent.Items.Remove(EOutputFileNamePresent.WaferNum_Stage.ToString());
            this.cmbFileNamePresent.SelectedIndex = 1;

        }

        //public frmDowaPath(PathInfo pInfo)
        //    : this()
        //{
        //    //SetData(pInfo);
        //    //SetData(pArr);

        //    //SetLabelColumnWidth(pgdPath, 10);
        //}

        public bool LoadDataFromDataCenter()
        {
            pathUIComponent1.PathInfomation = TD2UIPath(DataCenter._uiSetting.UIMapPathInfo.Clone() as MPI.Tester.Data.PathInfo);
            pathUIComponent1.PathInfomation.PathName = "Binマップ保存位置";
            pucMergeFilePath.PathInfomation = TD2UIPath(DataCenter._uiSetting.MergeFilePath.Clone() as MPI.Tester.Data.PathInfo);
            pucLaserPower.PathInfomation = TD2UIPath(DataCenter._uiSetting.LaserPowerLogPath.Clone() as MPI.Tester.Data.PathInfo);
            this.cmbFileNamePresent.SelectedItem = DataCenter._uiSetting.EMergeFileNameFormatPresent.ToString();
            return true;
        }
        public bool SaveDataToDataCenter()
        {
            DataCenter._uiSetting.UIMapPathInfo = UI2TDPath(pathUIComponent1.PathInfomation);
            DataCenter._uiSetting.MergeFilePath = UI2TDPath(pucMergeFilePath.PathInfomation);
            DataCenter._uiSetting.LaserPowerLogPath = UI2TDPath(pucLaserPower.PathInfomation);
            //DataCenter._uiSetting.EMergeFileNameFormatPresent = (enum)this.cmbFileNamePresent.SelectedItem.ToString();

            DataCenter._uiSetting.EMergeFileNameFormatPresent = (EOutputFileNamePresent)Enum.Parse(typeof(EOutputFileNamePresent), this.cmbFileNamePresent.SelectedItem.ToString(), true);
            return true;
        }

        private static MPI.Tester.GuiComponent.PathInfo TD2UIPath(MPI.Tester.Data.PathInfo pInfo)
        {
            MPI.Tester.GuiComponent.PathInfo uiPInfo = new GuiComponent.PathInfo();

            uiPInfo.EnablePath = pInfo.EnablePath;
            uiPInfo.FileExt = pInfo.FileExt;
            string enumName = pInfo.FolderType.ToString();
            MPI.Tester.GuiComponent.ETesterResultCreatFolderType foo = (MPI.Tester.GuiComponent.ETesterResultCreatFolderType)Enum.Parse(typeof(MPI.Tester.GuiComponent.ETesterResultCreatFolderType), enumName);
            uiPInfo.FolderType = foo;
            uiPInfo.PathName = "Binマップ保存位置";
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
                                //string folderName = DataCenter._uiSetting.GetPathWithFolder(UI2TDPath(pathUIComponent1.PathInfomation));
                                //string mapFileName = DataCenter._uiSetting.TestResultFileName;
                                //string file = Path.Combine(folderName, mapFileName) + "." + DataCenter._uiSetting.TestResultFileExt;

                                List<string> sList = new List<string>();
                                sList.AddRange(od.FileNames);
                                AppSystem.MergeFile(saveFilePAth, sList);
                            }
                        }
                    }
                    //AppSystem.MergeFile()
                }
            }

        }



    }

}
