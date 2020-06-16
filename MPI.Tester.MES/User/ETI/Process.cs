using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml.Serialization;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;


namespace MPI.Tester.MES.User.ETI
{
    class MESProcess : ProcessBase
    {
        private string FILE_EXTEND = ".csv";

        private bool _isRunDailyCheckMode;

        private ProductData _currentProduct;

        private ProductData _standardProduct;

        private CalibrationCtrl _calibrationCtrl;

        public MESProcess()
            : base()
        {
            _isRunDailyCheckMode = false;
        }

        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
        {
            EErrorCode rtn = EErrorCode.NONE;

            string serverConditonFullPath = Path.Combine(uiSetting.MESPath, uiSetting.WaferNumber+ FILE_EXTEND);

            string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, uiSetting.WaferNumber + FILE_EXTEND);

            DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

            if (!driveInfo.IsReady)
            {
                return EErrorCode.MES_ServerConnectFail;
            }

            if (File.Exists(serverConditonFullPath))
            {

                if (File.Exists(loaclConditonFullPath))
                {
                    File.Delete(loaclConditonFullPath);
                }

                File.Copy(serverConditonFullPath, loaclConditonFullPath);
            }
            else
            {
                this._describe.AppendLine(serverConditonFullPath + " is not Existence");

                return EErrorCode.MES_CondDataNotExist;
            }

            List<string[]> contents = CSVUtil.ReadCSV(loaclConditonFullPath);

            if (contents == null)
            {
                this._describe.AppendLine("Open " + loaclConditonFullPath + " File Error");

                return EErrorCode.MES_OpenFileError;
            }

            uiSetting.IsRunDailyCheckMode = false;

            this._testerRecipeFileName = String.Empty;

            string waferID=string.Empty;

            string recipe = string.Empty;

            string pbRecipe = string.Empty;

            if (contents.Count > 1)
            {
                if (contents[0][0] == "WaferID")
                {
                    waferID = contents[1][0].Replace(" ","");
                }

                if (contents[0][2] == "Recipe")
                {
                    recipe = contents[1][2].Replace(" ", "");

                    pbRecipe = contents[1][1].Replace(" ", "");
                }
            }

            if (File.Exists(loaclConditonFullPath))
            {
                File.Delete(loaclConditonFullPath);
            }

            //-------------------------------------------------
            //  is Deliver Prober Recipe => s
            //-------------------------------------------------

            if (uiSetting.IsDeliverProberRecipe)
            {
                string rceipePath = Path.Combine(uiSetting.ProductPath, recipe + ".pd");

                if (!File.Exists(rceipePath))
                {
                    this._describe.AppendLine(rceipePath);

                    this._describe.AppendLine("Is Not Existance");

                    return EErrorCode.MES_TargetRecipeNoExist;
                }
                    
                uiSetting.TaskSheetFileName = recipe;

                uiSetting.ProberRecipeName = pbRecipe;

                uiSetting.TestResultFileName = waferID;

                GlobalData.ProberRecipeName = pbRecipe;

                Console.WriteLine("[MESProcess], TesterRecipeFileName, " + recipe);

                Console.WriteLine("[MESProcess], ProberRecipeName, " + pbRecipe);
            }
            else
            {
                if (waferID.ToUpper() != uiSetting.WaferNumber.ToUpper() || recipe != uiSetting.TaskSheetFileName)
                {
                    Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_NotMatchRecipe.ToString());

                    rtn = EErrorCode.MES_NotMatchRecipe;
                }
            }

            if (uiSetting.WaferNumber.ToUpper().Contains("STD") || uiSetting.WaferNumber.ToUpper().Contains("CK"))
            {
                Console.WriteLine("[MESProcess], RunDailyCheck Mode => wafer ID : "+uiSetting.WaferNumber.ToString());

                uiSetting.IsRunDailyCheckMode = true;
            }
            else
            {
                uiSetting.IsRunDailyCheckMode = false;

                //  Check Daily Checking is OverDue

                string machineNameFilePath= Path.Combine(uiSetting.DailyCheckingMonitorPath,uiSetting.MachineName+"_DailyCheckLog.csv");

                if (uiSetting.IsCheckDailyVerifyResult)
                {
                    if (this.IsDailyCheckReady(uiSetting.IsCheckDailyCheckingOverDue, machineNameFilePath, uiSetting.TaskSheetFileName, uiSetting.DailyCheckingOverDueHours))
                    {
                        // GlobalFlag.IsDailyCheckFail = true;
                    }
                    else
                    {
                        Console.WriteLine("[MESProcess], OpenFileAndParse " + EErrorCode.MES_DailyCheckIsNotReady.ToString());

                        return EErrorCode.MES_DailyCheckIsNotReady;
                    }
                }
            }

            rtn = CheckIsMatchStandardRecipe(uiSetting.IsCheckStandardRecipe, uiSetting.StandRecipePath, uiSetting.TaskSheetFileName, uiSetting.IsEnableDuplicateStdRecipe);

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

        private EErrorCode CheckIsMatchStandardRecipe(bool isCheckStandRecipe, string standardPath, string recipe ,bool isCopyRecipe)
        {
            if (!isCheckStandRecipe)
            {
                return EErrorCode.NONE;
            }

            recipe = recipe +"."+Constants.Files.PRODUCT_FILE_EXTENSION;

            string serveoStandardRecipeFullPath = Path.Combine(standardPath, recipe);

            string loaclStandardRecipeFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, recipe);

            string currentRecipeFullPath= Path.Combine(Constants.Paths.PRODUCT_FILE, recipe);

            DriveInfo driveInfo = new DriveInfo(serveoStandardRecipeFullPath);

            if (!driveInfo.IsReady)
            {
                return EErrorCode.MES_ServerConnectFail;
            }


            if (!File.Exists(serveoStandardRecipeFullPath))
            {
                this._describe.Append(" Server Standard Recipe is not Existance");

                return EErrorCode.MES_TargetRecipeNoExist;
            }


            if (!File.Exists(currentRecipeFullPath))
            {
                this._describe.Append(" Local Recipe is not Existance");

                return EErrorCode.MES_TargetRecipeNoExist;
            }

            MPIFile.CopyFile(serveoStandardRecipeFullPath,loaclStandardRecipeFullPath);


            this._standardProduct = this.DeserializeProduct(loaclStandardRecipeFullPath);

            this._currentProduct = this.DeserializeProduct(currentRecipeFullPath);

            if (this._standardProduct.LOPSaveItem != this._currentProduct.LOPSaveItem)
            {
                this._describe.Append(" LOPSaveItem is not Equal");

                return EErrorCode.MES_RecipeContentNotMatch;
            }

            if (this._standardProduct.TestCondition.CalByWave != this._currentProduct.TestCondition.CalByWave)
            {
                this._describe.Append(" CalByWave is not Equal");

                return EErrorCode.MES_RecipeContentNotMatch;
            }

            Dictionary<string, TestItemData> standardtItem = new Dictionary<string, TestItemData>();

            Dictionary<string, TestItemData> currentItem = new Dictionary<string, TestItemData>();

            Dictionary<string, TestResultData> standardResult = new Dictionary<string, TestResultData>();

            Dictionary<string, TestResultData> currentResult = new Dictionary<string, TestResultData>();

            // Save Current Gain Offset & Table Save

            if (isCopyRecipe)
            {
                  // 建立新的CalibrateCtrl 

                  this._calibrationCtrl = new CalibrationCtrl();

                  this._calibrationCtrl.DicGainOffset = new Dictionary<string, GainOffsetData>();

                  this._calibrationCtrl.DicLOPWLParameter = new Dictionary<string, LOPWLParameter>();

                // 裡用Calibration Ctrl

                  string calibFile = recipe + ".cal";  // Recipe File Name

                  this._calibrationCtrl.ExtractCalibrateDataAndSave(this._currentProduct,standardPath, calibFile);

                  string serveoStandardCalibFullPath = Path.Combine(standardPath, calibFile);

                  this._calibrationCtrl.LoadFormCALFile(standardPath, calibFile,ESerializeFormatter.XML);

                // 把標準檔案的Recipe複製下來 當作local 

                this._currentProduct = this._standardProduct.Clone() as ProductData;

                // 打 CAL FILE 裡面的 DATA 放入PD內

                foreach (TestItemData item in this._currentProduct.TestCondition.TestItemArray)
                {
                    if (item.Type == ETestType.LOPWL)
                    {
                        if (_calibrationCtrl.DicLOPWLParameter.ContainsKey(item.KeyName))
                        {
                            (item as LOPWLTestItem).CoefTable = _calibrationCtrl.DicLOPWLParameter[item.KeyName].CoefTable;

                            (item as LOPWLTestItem).CoefWLResolution = _calibrationCtrl.DicLOPWLParameter[item.KeyName].CoefWLResolution;

                            (item as LOPWLTestItem).CoefStartWL = _calibrationCtrl.DicLOPWLParameter[item.KeyName].CoefStartWL;

                            (item as LOPWLTestItem).CoefEndWL = _calibrationCtrl.DicLOPWLParameter[item.KeyName].CoefEndWL;
                        }
                    }

                    if (item.GainOffsetSetting == null)
                    {
                        continue;
                    }

                    for (int i = 0; i < item.GainOffsetSetting.Length; i++)
                    {
                        if (_calibrationCtrl.DicGainOffset.ContainsKey(item.GainOffsetSetting[i].KeyName))
                        {
                            item.GainOffsetSetting[i] = _calibrationCtrl.DicGainOffset[item.GainOffsetSetting[i].KeyName];
                        }
                    }

                }

                // Item 相同 , 把校正係數塞進來後 ，序列化機台端的產品檔案 

                MPI.Xml.XmlFileSerializer.Serialize(this._currentProduct, currentRecipeFullPath);

                // 讀取新的recipe 確認數值是否相同

                this._currentProduct = this.DeserializeProduct(currentRecipeFullPath);
            }

            // 把Server上面，標準的 Test Condition 
            // 把Server上面，標準的 Test Condition By Result Item 複製下來

            foreach (TestItemData item in this._standardProduct.TestCondition.TestItemArray)
            {
                standardtItem.Add(item.KeyName, item);

                if (item.MsrtResult == null)
                {
                    continue;
                }

                foreach (TestResultData result in item.MsrtResult)
                {
                    if (result.IsEnable)
                    {
                        standardResult.Add(result.KeyName, result);
                    }
                }
            }

            // 把Local端的 Test Condition 
            // 把Local端的 Test Condition By Result Item 複製下來

            foreach (TestItemData item in this._currentProduct.TestCondition.TestItemArray)
            {
                currentItem.Add(item.KeyName, item);

                if (item.MsrtResult == null)
                {
                    continue;
                }

                foreach (TestResultData result in item.MsrtResult)
                {
                    if (result.IsEnable)
                    {
                        currentResult.Add(result.KeyName, result);
                    }
                }
            }

            // 確認content是否正確。Serial Byte[] 在比較其內容是否相同。

            bool isContentEqual = true;

            foreach (var content in standardtItem)
            {
                if (currentItem.ContainsKey(content.Key))
                {
                    if (!EqualTestContentData(content.Value, currentItem[content.Key]))
                    {
                        isContentEqual = false;
                    }                    
                }
                else
                {
                    isContentEqual = false;             
                }
            }


            if (isContentEqual == false)
            {
                return EErrorCode.MES_RecipeContentNotMatch;
            }

            // 比較TestResult 的 Adjacent & Msrt Setting 是否正確。

            foreach (var std in standardResult)
            {
                if (currentResult.ContainsKey(std.Key))
                {
                    if (std.Value.EnableAdjacent != currentResult[std.Key].EnableAdjacent
                        || std.Value.AdjacentType != currentResult[std.Key].AdjacentType
                        || std.Value.AdjacentRange != currentResult[std.Key].AdjacentRange)
                    {
                        this._describe.Append(std.Value.Name + " Adjacent Setting (Enable/Type/Range) is not Equal");
                        return EErrorCode.MES_RecipeContentNotMatch;
                    }

                    //if (std.Value.IsEnable != currentResult[std.Key].IsEnable
                    //      || std.Value.IsVerify != currentResult[std.Key].IsVerify
                    //      || std.Value.IsSkip != currentResult[std.Key].IsSkip)
                    //{
                    //    this._describe.Append(std.Value.Name+ " Msrt Setting (Enable/Pass/Skip) is not Equal");
                    //    return EErrorCode.MES_RecipeContentNotMatch;
                    //}
                }
                else
                {
                    this._describe.Append(std.Value.Name + " Msrt Setting (Enable/Pass/Skip) is not Have");
                    return EErrorCode.MES_RecipeContentNotMatch;
                }
            }

            return EErrorCode.NONE;

        }

        private bool EqualTestContentData(TestItemData expectItem, TestItemData currnetItem)
        {
            bool rtn = true;

            if (expectItem.IsEnable != currnetItem.IsEnable)
            {
                this._describe.AppendLine("Item : " + expectItem.Name + " Setting is not Enable");
                rtn= false;
            }

            object stdElect = expectItem.ElecSetting;

            object currentElect = currnetItem.ElecSetting;

            if (expectItem.ElecSetting[0].ForceValue != currnetItem.ElecSetting[0].ForceValue
                || expectItem.ElecSetting[0].ForceTime != currnetItem.ElecSetting[0].ForceTime)
            {
                this._describe.AppendLine("Item : " + expectItem.Name + " Electrical Content Setting is not Equal");
                rtn = false;
            }

            if (expectItem.Order != currnetItem.Order)
            {
                this._describe.AppendLine("Item : " + expectItem.Name + " Electrical  Order is not Equal");

                rtn = false;
            }
            return rtn;
        }

        private ProductData DeserializeProduct(string recipeFileAndPath)
        {
            ProductData pd = null;

            if (!System.IO.File.Exists(recipeFileAndPath))
                return pd;

            try
            {
                pd = MPI.Xml.XmlFileSerializer.Deserialize(typeof(ProductData), recipeFileAndPath) as ProductData;

                if (pd == null)
                {
                    return pd;
                }

                return pd;
            }
            catch
            {
                return pd;
            }
        }

        private bool IsDailyCheckReady(bool isCheckOverDue, string monitorFilePath, string recipeName, int OverDueHours)
        {
            //Copy File to Local and Read File  //@"C:\MPI\LEDTester\Temp2"
            string localFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR2, Path.GetFileName(monitorFilePath));

            if(!File.Exists(monitorFilePath))
            {
                this._describe.AppendLine("日校正監控檔案不存在 請確認檔案是否在指定路徑下");
                this._describe.AppendLine(monitorFilePath);
                return false;
            }


            if (monitorFilePath != localFile)
            {
                if (!MPI.Tester.MPIFile.CopyFile(monitorFilePath, localFile))
                {
                    return false;
                }
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

            string fileRecipe = lastLine[4];
            string machineName = lastLine[3];
            string fileIsOK = lastLine[7];

            if (fileIsOK.ToLower() != "true")
            {
                Console.WriteLine("[MESProcess], Daily Checking Result is Fail , System Stop");

                this._describe.Append("Daily Checking Result is Fail (日校正結果超出規範 ), System Stop");

                return false;
               
            }

            if (fileRecipe != recipeName)
            {
                Console.WriteLine("[MESProcess], Daily Checking Recipe is Not Match , System Stop");

                this._describe.AppendLine("Daily Checking Recipe is Not Match , System Stop");

                //if (!lastLine.Contains(recipeName))
                    return false;
            }

            if (isCheckOverDue)
            {
                if (!DateTime.TryParseExact(lastLine[0], "yyyy-MM-dd-HH-mm-ss",
                   CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out fileTime))
                {
                    Console.WriteLine("[MESProcess], DateTime File Format Is not Match , System Stop");
                    return false;
                }

                DateTime nowTime = DateTime.Now;

                if ((nowTime - fileTime).TotalHours > OverDueHours)
                {
                    Console.WriteLine("[MESProcess], Daily Checking Time is OverDue , System Stop");

                    this._describe.AppendLine("Daily Checking Time is OverDue (日校正結果超出規範時間 ), System Stop");

                    this._describe.AppendLine("OverDue Hours = "+ ((nowTime - fileTime).TotalHours).ToString("0.0"));

                    return false;
                }
            }


            //if (isCheckOverDue)
            //{
            //    if (!DateTime.TryParseExact(lastLine[0], "yyyy-MM-dd-HH-mm-ss",
            //        CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out fileTime))
            //    {
            //        return false;
            //    }

            //    DateTime nowTime = DateTime.Now;

            //    if ((fileTime - nowTime).TotalSeconds > 0)
            //    {
            //        return false;
            //    }

            //    DateTime checkTime = new DateTime();

            //    int checkHour = -1;

            //    dailyCheckTime.Sort();

            //    for (int i = 0; i < dailyCheckTime.Count - 1; i++)
            //    {
            //        if (nowTime.Hour >= dailyCheckTime[i] && nowTime.Hour < dailyCheckTime[i + 1])
            //        {
            //            checkHour = dailyCheckTime[i];

            //            break;
            //        }
            //    }

            //    if (checkHour > 0)
            //    {
            //        checkTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, checkHour, 0, 0);
            //    }
            //    else
            //    {
            //        checkTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, dailyCheckTime[dailyCheckTime.Count - 1], 0, 0);
            //    }

            //    TimeSpan ts = fileTime - checkTime;

            //    if (ts.TotalSeconds >= 0)
            //    {
            //        return true;
            //    }
            //}

            //Console.WriteLine("[MESProcess], Daily Checking Time is OverDue , System Stop");
            //return false;

            return true;
        }
    }
}
