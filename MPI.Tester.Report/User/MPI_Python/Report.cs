using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics;
using MPI.Tester.TestServer;

using MPI.Tester.Report.BaseMethod.InfoToJSON;
using MPI.Tester.Report.BaseMethod.HeaderFinder;


namespace MPI.Tester.Report.User.MPI_Python
{
    class Report : ReportBase
    {
        private bool isHeaderCreated = false;

        public Report(List<object> objs, bool isReStatistic)
            : base(objs, isReStatistic)
        {
        }

        protected override void SetResultTitle()
        {
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
            this.ResultTitleInfo.AddResultData("CONDITION", this.UISetting.PrefixStr + "Condition");//強迫塞入識別資訊，以免客戶所需的輸出報表未包含PASS,Channel等資訊

            string[] nArr = new string[] { "TEST", "COL", "ROW", "BIN", "POLAR", "CHANNEL", "ISALLPASS", "SEQUENCETIME", "TEST_START_TIME" };
            UISetting.ConditionKeyNames = new List<string>(nArr);
        }

        protected override EErrorCode WriteReportHeadByUser()
        {

            Dictionary<string,object> keyInfoDic = new Dictionary<string,object>();
            UISettingInfoConverter uiObj = new UISettingInfoConverter(UISetting, TesterSetting);

            keyInfoDic.Add("UISetting", uiObj.GetInfoDic());

            ProductDataInfoConverter pObj = new ProductDataInfoConverter(Product);

            keyInfoDic.Add("Product", pObj.GetInfoDic());

            BinDataInfo bObj = new BinDataInfo(SmartBinning);
            keyInfoDic.Add("Bin", bObj.GetInfoDic());

            MachineDataInfo mObj = new MachineDataInfo(MachineConfig,MachineInfo);
            keyInfoDic.Add("MachineSetting", mObj.GetInfoDic());

            string jStr = JsonConvert.SerializeObject(keyInfoDic, Formatting.Indented);

            this.WriteLine("--BeforeTest--");
            this.WriteLine(jStr);
            this.WriteLine("--End--");  
            this.WriteLine(this._resultTitleInfo.TitleStr);

            isHeaderCreated = true;

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
        }

        protected override void ReLoadTmpFile()
        {
            ///////////////////////////////////////////////////////
            //Replace Data And Check Row Col
            ///////////////////////////////////////////////////////
            if (this.FileFullNameTmp == null || this.FileFullNameTmp == "")
            {
                string fileName = this.TestResultFileNameWithoutExt();


                this.FileFullNameTmp = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMP1);

            }

            if (!File.Exists(this.FileFullNameTmp))
            {
                string fileName = this.TestResultFileNameWithoutExt();

                return;
            }

            try
            {
                using (StreamReader srCheckRowCol = new StreamReader(this.FileFullNameTmp, this._reportData.Encoding))
                {
                    bool isRawData = false;

                    int rawLineCount = 0;

                    this._checkColRowKey.Clear();

                    HeaderFinder hf = new HeaderFinder(this.TitleStrKey, TitleStrShift);

                    //重繞tmp檔取得 ColRowkey 對應的第幾筆數據
                    while (srCheckRowCol.Peek() >= 0)
                    {
                        string line = srCheckRowCol.ReadLine();

                        if (isRawData && line.Contains("@") && !line.Contains("@Condition"))
                        {
                            rawLineCount++;

                            string[] rawData = line.Split(this.SpiltChar);

                            string colrowKey = ColRowKeyMaker(rawData);

                            if (this._checkColRowKey.ContainsKey(colrowKey))
                            {
                                this._checkColRowKey[colrowKey] = rawLineCount;
                            }
                            else
                            {
                                this._checkColRowKey.Add(colrowKey, rawLineCount);
                            }
                        }
                        else if(!isRawData)
                        {
                            //if (line == this.ResultTitleInfo.TitleStr.Replace(",", this.SpiltChar.ToString()))
                            if (hf.CheckIfRowData(line))
                            {
                                isRawData = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.Delete(this.FileFullNameTmp);

                string fileName = this.TestResultFileNameWithoutExt();

                this._checkColRowKey.Clear();

                return;
            }
        }        

        protected override EErrorCode MoveFileToTargetByUser(EServerQueryCmd cmd)
        {

            bool isOutputPath02 = false;

            bool isOutputPath03 = false;

            string outPath01 = string.Empty;

            string outPath02 = string.Empty;

            string outPath03 = string.Empty;

            ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

            ETesterResultCreatFolderType type02 = ETesterResultCreatFolderType.None;

            ETesterResultCreatFolderType type03 = ETesterResultCreatFolderType.None;

            
            SetAutoMode(ref isOutputPath02, ref isOutputPath03, ref outPath01, ref outPath02, ref outPath03, ref type01, ref type02, ref type03);

            //outPath01 = GetPathWithFolder(outPath01, type01);

            outPath01 = GetPathWithFolder(outPath01, type01);

            //---------------------------------------------------------------------------------
            // Copy Report file to taget path
            //---------------------------------------------------------------------------------
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            string fileNameWithExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.TestResultFileExt;

            string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile01);

            //RunPythonScript("ReportConverter.py");

            //run_cmd("‪D:\\temp\\test\\t2.py", "-u ");
            


            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
        {
            if (!isHeaderCreated)
            {
                this.WriteLine(this._resultTitleInfo.TitleStr);
                isHeaderCreated = true;
            }
            int binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

            int binGrade = 0;

            int binNumber = 0;

            string binCode = string.Empty;

            if (bin != null)
            {
                binCode = bin.BinCode;

                binNumber = bin.BinNumber;

                if (bin.BinningType == EBinningType.IN_BIN)
                {
                    binGrade = 1;
                }
                else if (bin.BinningType == EBinningType.SIDE_BIN)
                {
                    binGrade = 2;
                }
                else if (bin.BinningType == EBinningType.NG_BIN)
                {
                    binGrade = 3;
                }
            }

            string line = string.Empty;

            int index = 0;

            foreach (var item in this._resultTitleInfo)
            {
                if (item.Key == "BIN_CODE")
                {
                    line += binCode;
                }
                else if (item.Key == "BIN_NUMBER")
                {
                    line += binNumber.ToString();
                }
                else if (item.Key == "BIN_GRADE")
                {
                    line += binGrade.ToString();
                }

                else if (item.Key == "TEST_START_TIME")
                {
                    string format = "yyyy/MM/DD HH:mm:ss.fff";

                    if (this.UISetting.UserDefinedData[item.Key] != null)
                    {
                        format = this.UISetting.UserDefinedData[item.Key].Formate;
                    }
                    line += TickToString(data[item.Key],format);
                }
                else if (data.ContainsKey(item.Key))
                {
                    string format = string.Empty;

                    if (this.UISetting.UserDefinedData[item.Key] != null)
                    {
                        format = this.UISetting.UserDefinedData[item.Key].Formate;
                    }

                    line += data[item.Key].ToString(format);
                }

                index++;

                if (index != this._resultTitleInfo.ResultCount)
                {
                    line += this.SpiltChar;
                }
            }
            line += GetTestConditionStr(data);
            this.WriteLine(line);

            return EErrorCode.NONE;
        }

        protected override void ReplaceReportData(Dictionary<string, string> replaceData, string inputFile, string outputFile, bool isSkipWritingTestCount = false)
        {
            ///////////////////////////////////////////////////////
            //Set Statistic Data
            ///////////////////////////////////////////////////////
            Dictionary<string, double> data = new Dictionary<string, double>();


            ///////////////////////////////////////////////////////
            //Replace Data And Check Row Col
            ///////////////////////////////////////////////////////
            if (outputFile == string.Empty)
            {
                return;
            }

            StreamWriter sw = new StreamWriter(outputFile, false, this._reportData.Encoding);

            StreamReader sr = new StreamReader(inputFile, this._reportData.Encoding);

            bool isRawData = false;

            int rawLineCount = 0;

            int testCount = this.UISetting.UserDefinedData.TestStartIndex;

            int shiftCount = TitleStrShift;

            HeaderFinder hf = new HeaderFinder(this.TitleStrKey, TitleStrShift);
            // 開始比對ColRowKey並寫檔
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();

                if (isRawData)
                {
                    if (this.TesterSetting.IsCheckRowCol)
                    {
                        rawLineCount++;

                        string[] rawData = line.Split(this.SpiltChar);

                        string colrowKey = ColRowKeyMaker(rawData);

                        // 把 row.col 和 checkRowCol "raw line count " 相同時, 才會push資料,解決當點重測row,col的問題
                        if (this._checkColRowKey.ContainsKey(colrowKey) && this._checkColRowKey[colrowKey] == rawLineCount)
                        {
                            // Check Col Row And ReStatist

                            //Rewrite TEST
                            if (this._resultTitleInfo.TestIndex >= 0)
                            {
                                rawData[this._resultTitleInfo.TestIndex] = testCount.ToString();

                                line = string.Empty;

                                for (int i = 0; i < rawData.Length; i++)
                                {
                                    line += rawData[i];

                                    if (i != rawData.Length - 1)
                                    {
                                        line += this.SpiltChar;
                                    }
                                }
                            }

                            testCount++;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
                else
                {
                    if (hf.CheckIfRowData(line))
                    {
                        isRawData = true;
                    }
                    else
                    {
                        if (replaceData.ContainsKey(line))
                        {
                            line = replaceData[line];
                        }
                    }
                }

                sw.WriteLine(line);
            }


            string endStr = GetTestEndInfo();

            sw.WriteLine("--AfterTest--");
            sw.WriteLine(endStr);
            sw.WriteLine("--End--");


            sr.Close();

            sr.Dispose();

            sw.Close();

            sw.Dispose();
        }


        #region


        protected void RunPythonScript(string sArgName, string args = "", params string[] teps)
        {
            Process p = new Process();
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + sArgName;// 獲得python檔案的絕對路徑（將檔案放在c#的debug資料夾中可以這樣操作）
            // path = @"C:\Users\user\Desktop\test\" + sArgName;//(因為我沒放debug下，所以直接寫的絕對路徑,替換掉上面的路徑了)

            path = @"D:\SoftwareDevelopment\01.LedTester\0051_MPI_DEMO_Python\ReportConverter_Git\" + sArgName;//ReportConverter.py
            p.StartInfo.FileName = @"D:\Programe\Python\python.exe";//沒有配環境變數的話，可以像我這樣寫python.exe的絕對路徑。如果配了，直接寫"python.exe"即可
            string str = File.Exists(p.StartInfo.FileName).ToString();
            string s2 = str;
            string sArguments = path;
            foreach (string sigstr in teps)
            {
                sArguments += " " + sigstr;//傳遞引數
            }

            sArguments += " " + args;

            p.StartInfo.Arguments = sArguments;

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardError = true;

            p.StartInfo.CreateNoWindow = true;

            p.Start();
            //p.BeginOutputReadLine();
            //p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            //Console.ReadLine();
            p.WaitForExit();
        }


        private List<string> GetLcrCaliList()
        {
            List<string> outStrList = new List<string>();

            LCRCaliData cData = SysCali.SystemCaliData.LCRCaliData;

            int dataNum = cData.NowDataNum - 1;


            //string type = "CaliType:," + cData.TestType.ToString();

            //outStrList.Add(type);
            string caliVolt = "Cali Level:," + cData.Level.ToString() + "V";

            outStrList.Add(caliVolt);

            string CableLength = "Cable Length:," + cData.CableLength.ToString();

            outStrList.Add(CableLength);

            string caliBias = "Cali Bias:," + cData.Bias.ToString() + "V";

            outStrList.Add(caliBias);


            string caliItems = "Type,Enable,Msrt Type,Raw A,Raw B,Set A,SetB,Meter A,Meter B";

            outStrList.Add(caliItems);

            string openCali = "OPEN:," + (cData.EnableOpen ? "ON" : "OFF");

            if (cData.EnableOpen)
            {
                CaliRowData crd = cData.LoadingList[dataNum].OpenRaw;
                openCali +=
                    "," + crd.CaliLCRTestType.ToString() +
                    "," + crd.ValA.ToString("E3") + " " + crd.UnitA +
                    "," + crd.ValB.ToString("E3") + " " + crd.UnitB +
                    ",," +
                    "," + crd.MeterRowValA.ToString("E3") + " " + crd.MeterUnitA +
                    "," + crd.MeterRowValB.ToString("E3") + " " + crd.MeterUnitB;
            }

            outStrList.Add(openCali);


            return outStrList;
            //outStrList 

        }

        private string GetTestConditionStr(Dictionary<string, double> data)
        {
            string outStr = this.UISetting.PrefixStr;

            if (UISetting.ConditionKeyNames != null)
            {
                int length = UISetting.ConditionKeyNames.Count;
                for (int i = 0; i < length; ++i)
                {

                    string key = UISetting.ConditionKeyNames[i];
                    if (data.ContainsKey(key))
                    {

                        string format = "";
                        if (this.UISetting.UserDefinedData[key] != null)
                        {
                            format = this.UISetting.UserDefinedData[key].Formate;
                        }
                        string toString = data[key].ToString(format);

                        if(key == "TEST_START_TIME")
                        {
                            toString = TickToString(data[key]);
                        }
                        outStr += toString;
                    }
                    if (i < length - 1)
                    {
                        outStr += ",";
                    }
                }
            }
            return outStr;

        }

        private string GetTestEndInfo()
        {
            Dictionary<string, object> keyInfoDic = new Dictionary<string, object>();

            string tsStr = this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
            string teStr = this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
            string tbStr = this.UISetting.WaferBeginTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
            keyInfoDic.Add("StartTestTime", tsStr);
            keyInfoDic.Add("EndTestTime", teStr);
            keyInfoDic.Add("WaferBeginTime", teStr);
            string jStr = JsonConvert.SerializeObject(keyInfoDic, Formatting.Indented);
            return jStr;
        }

        private string TickToString( double tick, string format = "yyyy/MM/dd HH:mm:ss.fff")
        {
            long tickl = (long)tick;
            DateTime dt = new DateTime(tickl);
            string outStr =  dt.ToString(format);
            return outStr;
        }
        #endregion

    }
}
