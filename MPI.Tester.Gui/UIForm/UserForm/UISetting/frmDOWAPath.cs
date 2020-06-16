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
        }

        public frmDowaPath(PathInfo pInfo)
            : this()
        {
            SetData(pInfo);
            //SetData(pArr);

            //SetLabelColumnWidth(pgdPath, 10);
        }

        public void SetData(PathInfo pInfo)
        {
            pathUIComponent1.PathInfomation = TD2UIPath(pInfo);
        }

        public PathInfo GetData()
        {
            PathInfo pInfo = UI2TDPath(pathUIComponent1.PathInfomation);
            return pInfo;
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



    }

}
