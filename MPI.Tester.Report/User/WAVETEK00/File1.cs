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
        
        EErrorCode OutPutFile1()
        {
            string tempFileName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, WAVETEK_Tmp);

            string tarFileName = GetFile1Name();      

            wtSw = new StreamWriter(tempFileName, false, this._reportData.Encoding);

            CreateFile1Head();

            PushDataToFile1();

            wtSw.Close();

            wtSw.Dispose();

            wtSw = null;

            bool result = true;

            if (UISetting.PathInfoArr[0].EnablePath)
            {
                string fullTarName1 = Path.Combine(UISetting.PathInfoArr[0].TestResultPath, tarFileName);      
                if (!CopyFileOrBackUp(tempFileName, tarFileName, fullTarName1))
                    result = false;
            }

            if (UISetting.PathInfoArr[1].EnablePath)
            {
                string fullTarName2 = Path.Combine(UISetting.PathInfoArr[1].TestResultPath, tarFileName);
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

        private static bool CopyFileOrBackUp(string tempFileName, string tarFileName, string fullTarName)
        {
            try
            {
                //string tarFolder = Path.GetDirectoryName(fullTarName);
                //if (!Directory.Exists(tarFolder))
                //{
                //    Directory.CreateDirectory(tarFolder);
                //}

                if (!MPIFile.CopyFile(tempFileName, fullTarName))
                {
                    string backupPath = Path.Combine(Constants.Paths.MPI_BACKUP_DIR, tarFileName);
                    Console.WriteLine("[Report WTK],Copy fail: " + fullTarName);
                    if (MPIFile.CopyFile(tempFileName, backupPath))
                    {
                        Console.WriteLine("[Report WTK],Backup to: " + backupPath);
                    }
                    return false;

                }
                else
                {
                    Console.WriteLine("[Report WTK],Copy Pass : " + fullTarName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Report WTK],Exception: " + e.Message);
                string backupPath = Path.Combine(Constants.Paths.MPI_BACKUP_DIR, tarFileName);
                Console.WriteLine("[Report WTK],Copy fail: " + fullTarName);
                if (MPIFile.CopyFile(tempFileName, backupPath))
                {
                    Console.WriteLine("[Report WTK],Backup to: " + backupPath);
                }
                return false;
            }

            return true;
        }

        private string GetFile1Name()
        {
            string fileName = UISetting.WaferNumber;
            if (UISetting.PathInfoArr != null)
            {
                fileName = UISetting.LotNumber + "_" +
                    UISetting.WaferNumber + "_" +
                    UISetting.SubPiece + "_";
                string startTime = "";
                if (SetDataInfo.ContainsKey("TestStartTime"))
                {
                    DateTime dt;
                    DateTime.TryParse(SetDataInfo["TestStartTime"], out dt);
                    startTime = dt.ToString("yyMMdd_HHmmss");
                    fileName += startTime + "_";
                }
                fileName += UISetting.TaskSheetFileName + "_" +
                    UISetting.MachineName + "_";
                if (IsCvTest)
                {
                    fileName += "CP2";
                }
                else
                {
                    fileName += "CP1";
                }
                fileName += ".csv";

            }

            return fileName;
        }

        private EErrorCode CreateFile1Head()
        {
            wtSw.WriteLine("EQP_ID," + UISetting.MachineName);
            wtSw.WriteLine("WAFER_ID," +
                UISetting.LotNumber + "_" +
                    UISetting.WaferNumber + "_" +
                    UISetting.SubPiece);
            wtSw.WriteLine("Recipe_Name," + UISetting.TaskSheetFileName);
            wtSw.WriteLine("OPERATOR," + UISetting.OperatorName);
            DateTime time;
            string startTimeStr = "",endTimeStr = "";
            //if (DateTime.TryParse(SetDataInfo["TestStartTime"], out time))
            //{
            //    startTimeStr = time.ToString("yyyy/M/d HH:mm ");
            //}
            startTimeStr = this._refSearchTime.ToString("yyyy/M/d HH:mm ");

            if (DateTime.TryParse(SetDataInfo["TestEndTime"], out time))
            {
                endTimeStr = time.ToString("yyyy/M/d HH:mm ");
            }
            wtSw.WriteLine("StartTime," + startTimeStr);
            wtSw.WriteLine("EndTime," + endTimeStr);
            wtSw.WriteLine("ProbeCount," + dieLogDic.Count);
            wtSw.WriteLine("");

            string itemStr = "Item", unitStr = "Unit",forceStr = "Value",specStr = "Spec";

            foreach (var mInfo in MsrtkeyInfoDic)
            {
                itemStr += "," + mInfo.Value.GetCustomerName_1base(1);

                string format = mInfo.Value.MsrtData.Formate;
                double minVal = Math.Round(mInfo.Value.MsrtData.MinLimitValue, 9, MidpointRounding.AwayFromZero);
                double maxVal = Math.Round(mInfo.Value.MsrtData.MaxLimitValue, 9, MidpointRounding.AwayFromZero);

                string minstr = minVal.ToString("0.######");
                string maxstr = maxVal.ToString("0.######");
                switch(mInfo.Value.MsrtData.BoundaryRule)                
                {
                    default:
                    case EBinBoundaryRule.LeValL:
                        {
                            specStr += "," + minstr + "<=" + mInfo.Value.GetCustomerName_1base(1) + "<" + maxstr;//後面記得改成個別的判斷上下限條件
                        }
                        break;
                    case EBinBoundaryRule.LeValLe:
                        {
                            specStr += "," + minstr + "<=" + mInfo.Value.GetCustomerName_1base(1) + "<=" + maxstr;//後面記得改成個別的判斷上下限條件
                        }
                        break;
                    case EBinBoundaryRule.LValL:
                        {
                            specStr += "," + minstr + "<" + mInfo.Value.GetCustomerName_1base(1) + "<" + maxstr;//後面記得改成個別的判斷上下限條件
                        }
                        break;
                    case EBinBoundaryRule.LValLe:
                        {
                            specStr += "," + minstr + "<" + mInfo.Value.GetCustomerName_1base(1) + "<=" + maxstr;//後面記得改成個別的判斷上下限條件
                        }
                        break;

                }
                unitStr += "," + mInfo.Value.TestData.ElecSetting[0].ForceUnit;

                if (mInfo.Value.TestData is LCRTestItem)
                {
                    forceStr += "," + (mInfo.Value.TestData as LCRTestItem).LCRSetting.DCBiasV.ToString("0.###");
                }
                else
                {
                    forceStr += "," + mInfo.Value.TestData.ElecSetting[0].ForceValue.ToString("0.######");
                }
            }

            wtSw.WriteLine(itemStr);
            wtSw.WriteLine(unitStr);
            wtSw.WriteLine(forceStr);
            wtSw.WriteLine(specStr);


            string fileHead = "PosX,PosY,Item";
            foreach (var mInfo in MsrtkeyInfoDic)
            {
                string tmpStr = mInfo.Value.GetCustomerName_1base(1); 

                tmpStr += "(" + mInfo.Value.MsrtData.Unit + ")";

                fileHead +=  "," + tmpStr;
            }
            fileHead +=",CP BIN";
            wtSw.WriteLine(fileHead);


            return EErrorCode.NONE;
        }

        private EErrorCode PushDataToFile1()
        {
            /*

PosX	PosY	Item	VF(V)	IR(uA)	VR(V)	CP BIN
188	276	1	0.5540049	0.000173	42.55126	11
    */
            int count = 1;
            foreach (var dieData in dieLogDic)
            {

                string str = dieData.Value.X.ToString() + "," +
                        dieData.Value.Y.ToString() + "," + count.ToString();
                foreach (var mInfo in MsrtkeyInfoDic)
                {
                    int index = mInfo.Value.ColIndex;
                    str += "," + dieData.Value.RawData[index];
                }
                str += "," + dieData.Value.Bin.ToString();
                wtSw.WriteLine(str);
                count++;
            }
            return EErrorCode.NONE;
        }

        
    }
}
