using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.TestServer;

namespace MPI.Tester.Report.User.WAVETEK00
{
    partial class Report : ReportBase
    {
        EErrorCode OutPutFile5()
        {
            string tempFileName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, WAVETEK_Tmp);

            string tarFileName = GetFile5Name();

            wtSw = new StreamWriter(tempFileName, false, this._reportData.Encoding);

            CreateFile5Head();

            PushDataToFile5();

            wtSw.Close();

            wtSw.Dispose();

            wtSw = null;

            bool result = true;

            if (UISetting.PathInfoArr[8].EnablePath)
            {
                string fullTarName1 = Path.Combine(UISetting.PathInfoArr[8].TestResultPath, tarFileName);
                if (!CopyFileOrBackUp(tempFileName, tarFileName, fullTarName1))
                    result = false;
            }

            if (UISetting.PathInfoArr[9].EnablePath)
            {
                string fullTarName2 = Path.Combine(UISetting.PathInfoArr[9].TestResultPath, tarFileName);
                if (!CopyFileOrBackUp(tempFileName, tarFileName, fullTarName2))
                    result = false;
            }

            MPIFile.DeleteFile(tempFileName);

            if (result == false)
            {
                return EErrorCode.REPORT_Customize_File_Create_Fail;
            }
            return EErrorCode.NONE;
        }

        private string GetFile5Name()
        {
            string fileName = UISetting.WaferNumber;
            if (UISetting.PathInfoArr != null)
            {
                fileName = UISetting.LotNumber  +
                    UISetting.WaferNumber ;
                if (IsCvTest)
                {
                    fileName += "_LCR";
                }
            }

            fileName += ".WTK";
            return fileName;
        }

        private EErrorCode CreateFile5Head()
        { 
            wtSw.WriteLine("WTK");
            wtSw.WriteLine(UISetting.LotNumber);
            wtSw.WriteLine(UISetting.WaferNumber);
            wtSw.WriteLine(UISetting.LotNumber+UISetting.WaferNumber+".WTK");

            return EErrorCode.NONE;
        }

        private EErrorCode PushDataToFile5()
        {
            for (int y = 1; y <= top; y++)
            {
                string rowStr = "";
                for (int x = 1; x <= right; x++)
                {
                    string keystr = x.ToString() + "_" + y.ToString();
                    if (dieLogDic.ContainsKey(keystr))
                    {
                        rowStr += dieLogDic[keystr].IsPass ? "1" : "X";
                    }
                    else
                    {
                        rowStr += ".";
                    }

                }
                wtSw.WriteLine(rowStr);
            }

            return EErrorCode.NONE;
        }
    }
}
