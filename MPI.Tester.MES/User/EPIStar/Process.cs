using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;

namespace MPI.Tester.MES.User.EPIStar
{
    class MESProcess : ProcessBase
    {
        private bool _isRunDailyCheckMode;

        public MESProcess()
            : base()
        {

        }

        private bool IsDailyCheckReady(string dailyCheckFileName, string recipeName, List<int> dailyCheckTime)
        {
            //Check TimeList Format
            if (dailyCheckTime == null || dailyCheckTime.Count < 1)
            {
                return false;
            }

            //Copy File to Local and Read File
            string localFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Path.GetFileName(dailyCheckFileName));

            if (!MPI.Tester.MPIFile.CopyFile(dailyCheckFileName, localFile))
            {
                return false;
            }

            List<string[]> checkFile = CSVUtil.ReadCSV(localFile);

            if (checkFile == null || checkFile.Count == 0)
            {
                return false;
            }

            List<string> lastLine = new List<string>(checkFile[checkFile.Count - 1]);

            if (lastLine.Count < 4)
            {
                return false;
            }

            //Get File Info(CalcTime, fileRecipe, fileIsOK), and check PASS/NG
            DateTime fileTime;

            string fileRecipe = lastLine[1];

            string fileIsOK = lastLine[3];

            if (fileIsOK != "True")
            {
                return false;
            }

            if (fileRecipe != recipeName)
            {
                if (!lastLine.Contains(recipeName))
                return false;
            }

            if (!DateTime.TryParseExact(lastLine[0], "yyyy-MM-dd-HH-mm-ss",
                CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out fileTime))
            {
                return false;
            }

            DateTime nowTime = DateTime.Now;

            if ((fileTime - nowTime).TotalSeconds > 0)
            {
                return false;
            }

            DateTime checkTime = new DateTime();

            int checkHour = -1;

            dailyCheckTime.Sort();

            for (int i = 0; i < dailyCheckTime.Count - 1; i++)
            {
                if (nowTime.Hour >= dailyCheckTime[i] && nowTime.Hour < dailyCheckTime[i + 1])
                {
                    checkHour = dailyCheckTime[i];

                    break;
                }
            }

            if (checkHour > 0)
            {
                checkTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, checkHour, 0, 0);
            }
            else
            {
                checkTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, dailyCheckTime[dailyCheckTime.Count - 1], 0, 0);
            }

            TimeSpan ts = fileTime - checkTime;

            if (ts.TotalSeconds >= 0)
            {
                return true;
            }

            return false;
        }


        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
        {
            EErrorCode rtn = EErrorCode.NONE;

            if (uiSetting.WaferNumber.ToUpper().Contains("TO") || uiSetting.WaferNumber.ToUpper().Contains("FIX") || uiSetting.WaferNumber.ToUpper().Contains("GLD"))
            {
                uiSetting.IsRunDailyCheckMode = true;
            }
            else
            {
                uiSetting.IsRunDailyCheckMode = false;

                if (uiSetting.IsEnableCheckDailyDataIsValid)
                {
                    if (this.IsDailyCheckReady(uiSetting.DailyCheckLogFileName, uiSetting.TaskSheetFileName, uiSetting.DailyCheckTime))
                    {
                        // GlobalFlag.IsDailyCheckFail = true;
                    }
                    else
                    {
                        //   GlobalFlag.IsDailyCheckFail = false;
                        Console.WriteLine("[MESProcess], OpenFileAndParse " + EErrorCode.MES_DailyCheckIsNotReady.ToString());
                        return EErrorCode.MES_DailyCheckIsNotReady;
                    }
                }
            }



      

            //string serverConditonFullPath = Path.Combine(uiSetting.MESPath, COND_FILE_NANE);
            //string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, COND_FILE_NANE);

            //DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

            //if (!driveInfo.IsReady)
            //{
            //    return EErrorCode.MES_ServerConnectFail;
            //}

            //if (File.Exists(serverConditonFullPath))
            //{
            //    if (File.Exists(loaclConditonFullPath))
            //    {
            //        File.Delete(loaclConditonFullPath);
            //    }
            //    File.Copy(serverConditonFullPath, loaclConditonFullPath);
            //}
            //else
            //{
            //    return EErrorCode.MES_CondDataNotExist;
            //}

            //if (uiSetting.WaferNumber.Length < 5)
            //    return EErrorCode.MES_BarcodeError;

            ////Ex: QC2FC1202-885_RP-H222904 -> Get "FC"
            //// string fileKey = DataCenter._uiSetting.Barcode.Substring(0, 2);

            //// string fileKey = DataCenter._uiSetting.Barcode.Remove(3);


            //List<string[]> FileNames = Tool.ToolBox.ReadCSV(loaclConditonFullPath);

            //if (FileNames == null)
            //{
            //    return EErrorCode.MES_OpenFileError;
            //}

            //this._testerRecipeFileName = String.Empty;

            //foreach (string[] key in FileNames)
            //{
            //    if (key[0] == "SpecID" || uiSetting.WaferNumber.Length < key[0].Length)
            //        continue;

            //    string fileKey = uiSetting.WaferNumber.Remove(key[0].Length);
            //    char[] subfileKey = uiSetting.WaferNumber.ToCharArray();
            //    int indexKeyChar = key[0].Length;

            //    if (fileKey == key[0] && subfileKey[indexKeyChar] == 'F')
            //    {
            //        this._testerRecipeFileName = key[1];
            //        break;
            //    }
            //}

            //if (this._testerRecipeFileName == String.Empty)
            //{
            //    if (uiSetting.WaferNumber.Contains("GLD") || uiSetting.WaferNumber.Contains("Golden"))
            //    {
            //        this._isRunDailyCheckMode = true;
            //        rtn = EErrorCode.NONE;
            //    }
            //    else
            //    {
            //        this._isRunDailyCheckMode = false;
            //        rtn = EErrorCode.MES_NotMatchRecipe;
            //    }
            //}
            //else
            //{
            //    rtn = EErrorCode.NONE;
            //}

            //if (!File.Exists(Path.Combine(uiSetting.ProductPath, this._testerRecipeFileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
            //{
            //    return EErrorCode.MES_TargetRecipeNoExist;
            //}

            //switch (rtn)
            //{
            //    case EErrorCode.NONE:
            //        uiSetting.TaskSheetFileName = this._testerRecipeFileName;
            //        uiSetting.IsRunDailyCheckMode = this._isRunDailyCheckMode;
            //        break;
            //    default:
            //        break;
            //}

            return rtn;
        }

        protected override EErrorCode ConverterToMPIFormat()
        {
            return EErrorCode.NONE;
        }

        protected override EErrorCode SaveRecipeToFile()
        {
            return EErrorCode.NONE;
        }
    }
}
