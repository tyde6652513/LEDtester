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
        EErrorCode OutPutFile2()
        {
            string tempFileName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, WAVETEK_Tmp);

            string tarFileName = GetFile2Name();


            wtSw = new StreamWriter(tempFileName, false, this._reportData.Encoding);

            CreateFile2Head();

            PushDataToFile2();

            wtSw.Close();

            wtSw.Dispose();

            wtSw = null;

            bool result = true;

            if (UISetting.PathInfoArr[2].EnablePath)
            {
                string fullTarName1 = Path.Combine(UISetting.PathInfoArr[2].TestResultPath, tarFileName);
                if (!CopyFileOrBackUp(tempFileName, tarFileName, fullTarName1))
                    result = false;
            }

            if (UISetting.PathInfoArr[3].EnablePath)
            {
                string fullTarName2 = Path.Combine(UISetting.PathInfoArr[3].TestResultPath, tarFileName);
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

        private string GetFile2Name()
        {
            string fileName = UISetting.WaferNumber;
            if (UISetting.PathInfoArr != null )
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

                fileName += "_GLXY";
                fileName += ".csv";

            }

            return fileName;
        }

        private EErrorCode CreateFile2Head()
        {
           
            wtSw.WriteLine("# Semiconductor Yield Analysis is easy with Galaxy!");
            wtSw.WriteLine("# Check latest news: www.galaxysemi.com");
            wtSw.WriteLine("# Created by: Examinator - V7.1.01	");
            wtSw.WriteLine("# Examinator Data File: Edit/Add/Remove any data you want!");
            wtSw.WriteLine("");
            wtSw.WriteLine("--- Csv version:");
            wtSw.WriteLine("Major,2");
            wtSw.WriteLine("Minor,1");
            wtSw.WriteLine("--- Global Info:");

            DateTime time;
            string startTimeStr = "", endTimeStr = "";
            //if (DateTime.TryParse(SetDataInfo["TestStartTime"], out time))
            //{
            //    startTimeStr = time.ToString("yyyy_MM_dd HH:mm:ss");
            //}
            startTimeStr = this._refSearchTime.ToString("yyyy_MM_dd HH:mm:ss");
            if (DateTime.TryParse(SetDataInfo["TestEndTime"], out time))
            {
                endTimeStr = time.ToString("yyyy_MM_dd HH:mm:ss");
            }
            wtSw.WriteLine("Date," + startTimeStr);
            wtSw.WriteLine("SetupTime," + startTimeStr);
            wtSw.WriteLine("StartTime," + startTimeStr);
            wtSw.WriteLine("FinishTime," + endTimeStr);

            wtSw.WriteLine("ProgramName," + UISetting.TaskSheetFileName);
            wtSw.WriteLine("Lot," + UISetting.LotNumber);           

            wtSw.WriteLine("Wafer," + UISetting.WaferNumber);
            wtSw.WriteLine("Wafer_Center_X,0");
            wtSw.WriteLine("Wafer_Center_Y,0");
            wtSw.WriteLine("Wafer_Pos_X," );
            wtSw.WriteLine("Wafer_Pos_Y," );
            wtSw.WriteLine("TesterName," + UISetting.MachineName);
            wtSw.WriteLine("TesterType,PD200");
            wtSw.WriteLine("Product," + Product.ProductName);
            wtSw.WriteLine("Operator," + UISetting.OperatorName);
            wtSw.WriteLine("ExecType,RFIC");
            wtSw.WriteLine("ExecRevision,A.01.07");
            wtSw.WriteLine("ModeCode,P");
            wtSw.WriteLine("BurnTime,0");
            wtSw.WriteLine("Facility,WTK");

            wtSw.WriteLine("Process," + Product.ProductName);
            string esName ="";
            if (SetDataInfo.ContainsKey("EdgeSensorName"))
            {
                esName = SetDataInfo["EdgeSensorName"];
            }
            wtSw.WriteLine("SetupID," + esName);
            wtSw.WriteLine("--- Site details:,Head #1");


            string chNum = "1";
            if (SetDataInfo.ContainsKey("ChannelQty"))
            {
                chNum = SetDataInfo["ChannelQty"];
            }
            //ChannelQty

            wtSw.WriteLine("Site group,0");
            wtSw.WriteLine("Testing sites," + chNum);
            wtSw.WriteLine("Probe card ID," + esName);
            wtSw.WriteLine("DIB board type,-1");
            wtSw.WriteLine("DIB board ID,2.68E+08");
            wtSw.WriteLine("--- Options:");
            wtSw.WriteLine("UnitsMode,scaling_factor");
            wtSw.WriteLine("");


            string ParaStr = "Parameter,SBIN,HBIN,DIE_X,DIE_Y,SITE,TIME,TOTAL_TESTS,LOT_ID,WAFER_ID";

            string itemStr = "Tests#,,,,,,,,,", unitStr = "Unit,,,,,,sec.,,,";
            string HLStr = "HighL,,,,,,,,,", LLStr = "LowL,,,,,,,,,";

            int counter = 1;
            foreach (var mInfo in MsrtkeyInfoDic)
            {
                ParaStr += "," + mInfo.Value.GetCustomerName_1base(2);
                itemStr += "," + counter;
                unitStr += "," + mInfo.Value.MsrtData.Unit;

                string format = mInfo.Value.MsrtData.Formate;
                double minVal = Math.Round(mInfo.Value.MsrtData.MinLimitValue, 9, MidpointRounding.AwayFromZero);
                double maxVal = Math.Round(mInfo.Value.MsrtData.MaxLimitValue, 9, MidpointRounding.AwayFromZero);

                string minstr = minVal.ToString("0.######");
                string maxstr = maxVal.ToString("0.######");
                HLStr += "," + maxstr;
                LLStr += "," + minstr;
                counter++;
            }

            wtSw.WriteLine(ParaStr);
            wtSw.WriteLine(itemStr);
            wtSw.WriteLine("Patterns");
            wtSw.WriteLine(unitStr);
            wtSw.WriteLine(HLStr);
            wtSw.WriteLine(LLStr);


            return EErrorCode.NONE;
        }

        private EErrorCode PushDataToFile2()
        {
            int count = 1;
            int tesitemCount = MsrtkeyInfoDic.Count;
            string lotNum = UISetting.LotNumber;
            string slotNum = UISetting.WaferNumber;
            foreach (var dieData in dieLogDic)
            {

                string str = "PID-" + count.ToString() +
                    "," + dieData.Value.Bin.ToString() +
                    "," + dieData.Value.Bin.ToString() +
                    "," + dieData.Value.X.ToString() +
                    "," + dieData.Value.Y.ToString() +
                    "," + dieData.Value.Ch.ToString();

                str += "," + (dieData.Value.TestTime / 1000).ToString("0.000");
                str += "," + tesitemCount.ToString();
                str += "," + lotNum.ToString();
                str += "," + slotNum.ToString();

                foreach (var mInfo in MsrtkeyInfoDic)
                {
                    int index = mInfo.Value.ColIndex;
                    str += "," + dieData.Value.RawData[index];
                }
                wtSw.WriteLine(str);
                count++;
            }
            return EErrorCode.NONE;
        }
    }
}
