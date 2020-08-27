using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Gui.Extension
{
    public static class UIComponentConverter
    {

        public static MPI.Tester.GuiComponent.PathInfo TD2UIPath(MPI.Tester.Data.PathInfo pInfo)
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

        public static MPI.Tester.Data.PathInfo UI2TDPath(MPI.Tester.GuiComponent.PathInfo uiPInfo)
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
