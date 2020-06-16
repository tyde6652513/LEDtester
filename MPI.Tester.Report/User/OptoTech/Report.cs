using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;

namespace MPI.Tester.Report.User.OptoTech
{
    partial class Report : ReportBase
    {
        Dictionary<string, bool> _keyIsTest = new Dictionary<string, bool>();

        private StreamWriter _lsw;
        public Report(List<object> objs, bool isReStatistic)
            : base(objs, isReStatistic)
        {
        }

        protected override void SetResultTitle()
        {
            Dictionary<string, string> keyNameDic = new Dictionary<string, string>();

            if (this.UISetting.UserDefinedData.ResultItemNameDic != null)
            {
                foreach (var p in this.UISetting.UserDefinedData.ResultItemNameDic)
                {
                    string key = p.Key;
                    string val = p.Value;
                    bool isTest = true;

                    if (this.Product.TestCondition != null &&
                        this.Product.TestCondition.TestItemArray != null)
                    {
                        foreach (var testItem in this.Product.TestCondition.TestItemArray)
                        {
                            foreach (var rData in testItem.MsrtResult)
                            {
                                if (rData.KeyName == key)
                                {
                                   val = rData.Name;
                                    isTest = rData.IsThisItemTested || rData.IsSystemItem;
                                }
                            }
                        }
                    }

                    _keyIsTest.Add(key, isTest);

                    keyNameDic.Add(key, val);
                }
            }

            this.ResultTitleInfo.SetResultData(keyNameDic);
        }

        protected override EErrorCode WriteReportHeadByUser()
        {
            Dictionary<string, string> keyInfoDic = FindInfoFromKeyInData(UISetting.WaferNumber);
            //663C058D1011

            this.WriteLine("FileID,,\"WEI MIN Data File\"");

            this.WriteLine("FileName,,\"" + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt + "\"");

            this.WriteLine("TestTime,,\"" + "\"");


            this.WriteLine("TesterModel,,\"LED617WH\"");

            this.WriteLine("CommPort,,\"" + ReturnValueOrEmpty(keyInfoDic,"CommPort") + "\"");//此片第幾次測試

            this.WriteLine("TesterNumber,,\"" + this.UISetting.MachineName + "\"");//點測機代碼：點測機機台編號

            this.WriteLine("ProductName,,\"" + ReturnValueOrEmpty(keyInfoDic, "ProductName") + "\"");//Sort作業識別碼

            this.WriteLine("DeviceNumber,,\"" + ReturnValueOrEmpty(keyInfoDic, "DeviceNumber") + "\"");//毛片代碼

            this.WriteLine("Specification,,\"\"");

            this.WriteLine("SpecificationRemark,,\"\"");

            this.WriteLine("SampleBins,,\"ALL\"");

            this.WriteLine("SampleStandard,,\"WEIMIN\"");

            this.WriteLine("SampleLevel,,\"STANDARD\"");

            this.WriteLine("TotalTested,,\"\"");

            this.WriteLine("Samples,,\"\"");

            this.WriteLine("Remark1,,\"\"");
            this.WriteLine("Remark2,,\"" + GetAttSetStr() +","+ GetLcrCaliSet() + "\"");
            this.WriteLine("Remark3,,\"" + ReturnValueOrEmpty(keyInfoDic, "Remark3") + "\"");//點測條件設定檔 (產品檔名稱)


            

            this.WriteLine("CustomerNotel,,\"\"");
            this.WriteLine("CustomerRemark,,\"\"");


            this.WriteLine("LotNumber,,\"" + this.UISetting.WaferNumber + "\"");

            this.WriteLine("Operator,,\"" + this.UISetting.OperatorName + "\"");

            this.WriteLine("CodeNumber,,\"" + ReturnValueOrEmpty(keyInfoDic, "CodeNumber") + "\"");//校正晶粒編號

            this.WriteLine("SerialNumber,,\"" + ReturnValueOrEmpty(keyInfoDic, "SerialNumber") + "\"");//校正晶片編號

            this.WriteLine("MaximumBin,,\"31\"");

            this.WriteLine("ItemName,,\"" + this.ItemNameStr() + "\"");

            this.WriteLine("DataFormat,,\"" + this.DataFormatStr() + "\"");

            this.WriteLine("TestCondition,,\"" + this.TestConditionStr() + "\"");

            this.WriteLine("At,,\"ALL," + this.BinStr() + "\"");

            for (int i = 1; i <= 30; i++)
            {
                this.WriteLine("BinAt" + i + ",,\"" + i + "," + this.BinStr() + "\"");
            }

            this.WriteLine("Target,,\"" + this.targetStr() + "\"");

            this.WriteLine("LowLimit,,\"" + LimitStr(false) + "\"");

            this.WriteLine("HighLimit,,\"" + LimitStr(true) + "\"");

            this.WriteLine("");

            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string testTime = "TestTime,,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm:ss") + "\"";

            replaceData.Add("TestTime,,\"" + "\"", testTime);
            string rematk1 = "Remark1,,\"\"";
            string newRemark1 = "Remark1,,\""+GoodRateA01.ToString("0.00")+"%\"";
            replaceData.Add(rematk1, newRemark1);
            /*
            if(!(this.UISetting.LaserPowerLogPath != null &&
                this.UISetting.LaserPowerLogPath.EnablePath) ||
                _lsw == null)//沒開的話才丟到報表上
            {
                if (RemarkList != null && RemarkList.Count > 0)
                {
                    string pMonitorStr = "Remark2,,\"\"";
                    foreach (string str in RemarkList)
                        pMonitorStr += str + ",";
                    replaceData.Add("Remark2,,\"\"", pMonitorStr);
                }
            }
              */

            this.ReplaceReport(replaceData);

            CloseLaserLog();


            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
        {
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
                    long tick = (long)data[item.Key];
                    DateTime dt = new DateTime(tick);
                    line += dt.ToString("HH:mm:ss.fff");
                }
                else if (data.ContainsKey(item.Key) && _keyIsTest.ContainsKey(item.Key) &&
                    _keyIsTest[item.Key] == true)
                {
                    string format = string.Empty;

                    if (this.UISetting.UserDefinedData[item.Key] != null)
                    {
                        format = this.UISetting.UserDefinedData[item.Key].Formate;
                    }

                    string outStr = (0.0).ToString(format);
                    
                    //else if (item.Key.Contains("MCALC"))

                    if (item.Key == "ISALLPASS")
                    {
                        outStr = data[item.Key] == 1 ? "1" : "0";//原來的pass=1/NG=2
                    }
                    else
                    {
                        if (data.ContainsKey(item.Key))
                        {
                            var rData = GetMsrtDataFromKey(item.Key);
                            if (rData == null)//system value
                            { 
                                outStr = data[item.Key].ToString(format);
                            }
                            else if (double.IsNaN(data[item.Key]))
                            {
                                outStr = "----";
                            }
                            else if (!rData.IsTested)
                            {
                                outStr = "----";
                            }
                            else
                            {
                                outStr = data[item.Key].ToString(format);
                            }
                        }
                        else
                        {
                            outStr = "";
                        }
                        
                    }

                    line += outStr;
                }
                

                index++;

                if (index != this._resultTitleInfo.ResultCount)
                {
                    line += this.SpiltChar;
                }
            }

            this.WriteLine(line);

            return EErrorCode.NONE;
        }

        protected override EErrorCode ProcessAfterWaferFinished()//在WaferFinished後啟動，目前是設計來啟動光磊合檔用
        {
            EErrorCode err = EErrorCode.NONE;

            if (!this.UISetting.IsManualRunMode)
            {
                string srcPath01 = this.UISetting.TestResultPath01;

                ETesterResultCreatFolderType type01 = this.UISetting.TesterResultCreatFolderType01;

                srcPath01 = GetPathWithFolder(srcPath01, type01);

                string mergeOutPath01 = GetPathWithFolder(this.UISetting.MergeFilePath);

                string fileNameWithExt = this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt;

                if (UISetting.MergeFilePath.EnablePath &&
                    UISetting.FileInProcessList != null && UISetting.FileInProcessList.Count > 1)
                {
                    if (Directory.Exists(srcPath01))
                    {
                        string tarFolder = GetPathWithFolder(UISetting.MergeFilePath);
                        string tarPath = Path.Combine(tarFolder, UISetting.FileInProcessList[0]);

                        List<string> strList = new List<string>();
                        foreach (string str in UISetting.FileInProcessList)
                        {
                            string pStr = Path.Combine(srcPath01, str);
                            strList.Add(pStr);
                        }
                        err = MergeFile(tarPath, strList);
                    }
                    else
                    {
                        err = EErrorCode.REPORT_Merge_FilePathError;
                    }
                }
            }

            return err;
        }

        public override EErrorCode MergeFile(string outputPath, List<string> fileList = null)
        {
            Console.WriteLine("[Report],MergeFile()");
            List<string> oldFirstRow = new List<string>();
            oldFirstRow.Add("FileName");
            oldFirstRow.Add("TestTime");
            //oldFirstRow.Add("Remark1");

            List<string> longFirstRow = new List<string>();         
            List<string> infoAppendRow = new List<string>();
            infoAppendRow.Add("Remark2");
            //List<string> headerText = new List<string>();
            List<string[]> headerArrList = new List<string[]>();
            Dictionary<int, string> colNameDic = new Dictionary<int, string>();
            Dictionary<string, string[]> posStrArrDic = new Dictionary<string, string[]>();

            

            if (fileList != null && fileList.Count > 1)
            {
                for (int fCnt = 0; fCnt < fileList.Count && File.Exists(fileList[fCnt]); ++fCnt)
                {
                    #region
                    using (StreamReader sr = new StreamReader(fileList[fCnt]))
                    {
                        bool isRawData = false;

                        string tarStr = this.UISetting.UserDefinedData.ResultItemNameDic[EProberDataIndex.COL.ToString()] + "," +
                            this.UISetting.UserDefinedData.ResultItemNameDic[EProberDataIndex.ROW.ToString()];
                        HeaderFinder_ByEndStr hf = new HeaderFinder_ByEndStr(tarStr, TitleStrShift);//光磊的測試結果名稱可修改，因此需使用最後兩欄的Col,Row來識別
                        int nowRow = 0;
                        while (sr.Peek() >= 0)
                        {
                            string line = sr.ReadLine();
                            if (isRawData)
                            {
                                string[] rawData = line.Split(this.SpiltChar);

                                string colrowKey = ColRowKeyMaker(rawData);

                                if (!posStrArrDic.ContainsKey(colrowKey))
                                {
                                    posStrArrDic.Add(colrowKey, rawData);
                                }
                                else
                                {
                                    for (int i = 0; i < rawData.Length; ++i)
                                    {                                        
                                        if (rawData[i] != null && rawData[i] != "")
                                        {                                            
                                            if (this.ResultTitleInfo.IsAllPassIndex == i)
                                            {
                                                posStrArrDic[colrowKey][i] = (rawData[i] == "0" ? "0" : posStrArrDic[colrowKey][i]);
                                            }
                                            else
                                            {
                                                posStrArrDic[colrowKey][i] = rawData[i];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                #region >>header<<
                                if (hf.CheckIfRowData(line))
                                {
                                    Console.WriteLine("[Report],MergeFile(),found data header of" + fileList[fCnt]);
                                    isRawData = true;
                                    if (fCnt == 0)
                                    {
                                        string[] strArr = line.Split(this.SpiltChar);
                                        {
                                            for (int i = 0; i < strArr.Length; ++i)
                                            {
                                                colNameDic.Add(i, strArr[i]);
                                            }
                                        }
                                    }
                                }

                                if (fCnt == 0)
                                {
                                    //headerArrList
                                    if (line != "")
                                    {
                                        string[] strArr = line.Split(this.SpiltChar);
                                        headerArrList.Add(strArr);
                                    }
                                    else
                                    {
                                        headerArrList.Add(null);
                                    }
                                }
                                else
                                {
                                    #region >>header merge<<
                                    if (line != "" && headerArrList[nowRow] != null)
                                    {
                                        string[] strArr = line.Split(this.SpiltChar);
                                        string[] oldStrArr = headerArrList[nowRow];


                                        if (IsStartInRefList(line, infoAppendRow))
                                        {
                                            if (strArr != null && oldStrArr != null &&
                                                strArr.Length > 2 && oldStrArr.Length > 2)
                                            {
                                                string key = strArr[0];
                                                string mergeStr = "";
                                                for (int i = 1; i < oldStrArr.Length; ++i)
                                                {
                                                    if (oldStrArr[i] == "" || oldStrArr[i] == "\"")
                                                    {
                                                        continue;
                                                    }
                                                    string clearStr = oldStrArr[i].Replace("\"", "");
                                                    mergeStr += clearStr + ","; 
                                                }
                                                for (int i = 1; i < strArr.Length; ++i)
                                                {
                                                    if (strArr[i] == "" || strArr[i] == "\"")
                                                    {
                                                        continue;
                                                    }
                                                    string clearStr = strArr[i].Replace("\"", "");
                                                    mergeStr += clearStr + ",";
                                                }
                                                headerArrList[nowRow] = new string[] { key, "", ("\"" + mergeStr + "\"") }; 
                                            }                                            
                                        }
                                        else if (IsStartInRefList(line, longFirstRow))
                                        {
                                            if (CheckLongerFirst(strArr, oldStrArr))
                                            {
                                                headerArrList[nowRow] = strArr;
                                            }                                        
                                        }
                                        else if (!IsStartInRefList(line, oldFirstRow))
                                        {
                                            
                                            if (strArr.Length == oldStrArr.Length)
                                            {
                                                if (isRawData)//檔頭-測試項目name
                                                {
                                                    string[] oriTitleArr = (from p in UISetting.UserDefinedData.ResultItemNameDic
                                                                            select p.Value).ToArray();//default title  
                                                    if (oriTitleArr.Length == strArr.Length)
                                                    {
                                                        for (int i = 0; i < strArr.Length; ++i)
                                                        {
                                                            if (strArr[i] == "" || strArr[i] == oriTitleArr[i])
                                                            {
                                                                strArr[i] = oldStrArr[i];
                                                            }
                                                        }
                                                        headerArrList[nowRow] = strArr;
                                                    }
                                                }
                                                else
                                                {
                                                    for (int i = 0; i < strArr.Length; ++i)
                                                    {
                                                        if (strArr[i] == "" && oldStrArr[i] != "")
                                                        {
                                                            strArr[i] = oldStrArr[i];
                                                        }
                                                    }
                                                    headerArrList[nowRow] = strArr;
                                                }
                                            }
                                        }
                                    }                                    
                                    #endregion
                                }
                                #endregion
                            }
                            nowRow++;
                        }
                    }
                    #endregion
                }

                #region >>calc yield rate<<
                Statistic statistic = new Statistic();
                double yieldRate = 1;
                foreach (var p in posStrArrDic.Values)
                {
                    if (p != null && p.Length > this.ResultTitleInfo.IsAllPassIndex)
                    {
                        int passVal = p[this.ResultTitleInfo.IsAllPassIndex] == "1"? 1:0;
                        statistic.Push(passVal);
                    } 
                }
                yieldRate = statistic.Mean;
                //if (this.ResultTitleInfo.IsAllPassIndex == i)
                
                #endregion
                Console.WriteLine("[Report],MergeFile(),write out data");
                string mergeTmpPath = @"C:\MPI\Temp2\mergeTemp.csv";
                using (StreamWriter sw = new StreamWriter(mergeTmpPath))
                {
                    foreach (string[] strArr in headerArrList)
                    {
                        string str = StrArrToString(strArr, this.SpiltChar.ToString());
                        if (str.StartsWith("Remark1"))
                        {

                            str = "Remark1,,\"" + (yieldRate * 100).ToString("0.00") + "%\"";
                        }
                        sw.WriteLine(str);
                    }
                    foreach (var p in posStrArrDic.Values)
                    {
                        string outStr = "";
                        int legnth = p.Length;
                        int cnt = 1;
                        foreach (string str in p)
                        {
                            outStr += str;
                            if (cnt < legnth) { outStr += this.SpiltChar.ToString(); }
                            ++cnt;
                        }
                        sw.WriteLine(outStr);
                    }
                }
                if (File.Exists(mergeTmpPath))
                {
                    string fileName = Path.GetFileName(mergeTmpPath);
                    string backupFullName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName);
                    MPIFile.CopyFile(mergeTmpPath, outputPath);
                }
                MPIFile.CopyFile(mergeTmpPath, outputPath);
            }

            return EErrorCode.NONE;
        }

        private bool CheckLongerFirst( string[] strArr, string[] oldStrArr)
        {
            if (strArr != null && oldStrArr != null &&
                strArr.Length > oldStrArr.Length)
            {
                return true;                
            }
            return false;
        }


        #region >>>private method
        private string GetLcrCaliSet()
        {
            string lcrInfo = "";
            try {
                bool useLCR = false;
                for (int i = 0; i < this.Product.TestCondition.TestItemArray.Length; ++i)
                {
                    TestItemData item = this.Product.TestCondition.TestItemArray[i];
                    if (item.IsEnable && item.Type == ETestType.LCR)
                    {
                        useLCR = true;
                        break;
                    }
                }
                if (useLCR && this.SysCali.SystemCaliData.LCRCaliData != null)
                {
                    lcrInfo = this.SysCali.SystemCaliData.LCRCaliData.ToString();
                    lcrInfo = ","+lcrInfo.Replace('\n', ',');
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Report.OptoTech],GetLcrCaliSet(),Excpetion :" + e.Message);
                return "";
            }
            return lcrInfo;

        }
        private string GetAttSetStr()
        {
            string attSet = "";

            if (this.Product.TestCondition != null &&
this.Product.TestCondition.TestItemArray != null)
            {
                bool useAtt = false;
                
                //foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                for (int i = 0; i < this.Product.TestCondition.TestItemArray.Length; ++i)
                {
                    TestItemData item = this.Product.TestCondition.TestItemArray[i];
                    if (item.IsEnable && item.Type == ETestType.LaserSource)
                    {
                        if (!useAtt)
                        { attSet = ",AttSet:";
                        useAtt = true;}
                        LaserSourceTestItem lItem = (item as LaserSourceTestItem);
                        if (lItem.LaserSourceSet != null && lItem.LaserSourceSet.AttenuatorData != null)
                        {
                            attSet += lItem.Name + ":" + lItem.LaserSourceSet.AttenuatorData.Attenuate.Set.ToString("0.###") + ",";
                        }
                    }
                }
            }

            return attSet;
            
        }

        private string ItemNameStr()
        {
            string itemNameStr = "BIN,";

            foreach (var data in ResultTitleInfo)
            {
                string key = data.Key;

                if (key != "TEST" && key != "BIN")
                {
                    bool found = false;

                    foreach (var rData in AcquireData.OutputTestResult)
                    {
                        if (key == rData.KeyName)
                        {
                            found = true;
                            itemNameStr += data.Value;

                            if (rData.Unit != "")
                            {
                                itemNameStr += "(" + rData.Unit + ")";
                            }
                            break;
                        }
                    }

                    if (!found)
                    {
                        var item = this.UISetting.UserDefinedData[key];
                        if (item != null)
                        {
                            string name1 = item.Name;
                            string unit = item.Unit;
                            itemNameStr += name1;
                            if (unit != "")
                            { itemNameStr += "(" + unit + ")"; }
                        }
                    }
                    itemNameStr += ",";
                }
                
            }
            if (itemNameStr.Length > 0 && itemNameStr[itemNameStr.Length - 1] == ',')
            {
                itemNameStr = itemNameStr.Remove(itemNameStr.Length - 1);
            }


            return itemNameStr;
        }

        private string DataFormatStr()
        {
            string dataFormatStr = "0,";

            foreach (var data in ResultTitleInfo)
            {
                string key = data.Key;
                bool found = false;
                if (key != "TEST" && key != "BIN")
                {
                    if (AcquireData.OutputTestResult.FindIndex(x => x.KeyName == key) < 0)
                    {
                        foreach (var rData in AcquireData.OutputTestResult)
                        {
                            if (key == rData.KeyName)
                            {
                                dataFormatStr += rData.Formate;
                            }
                        }
                    }
                    if (!found)
                    {
                        var item = this.UISetting.UserDefinedData[key];
                        if (item != null)
                        {
                            string format = item.Formate;
                            dataFormatStr += format;
                        }
                    }
                    dataFormatStr += ",";
                }
            }
            if (dataFormatStr.Length > 0 && dataFormatStr[dataFormatStr.Length - 1] == ',')
            {
                dataFormatStr = dataFormatStr.Remove(dataFormatStr.Length - 1);
            }

            return dataFormatStr;
        }

        private string TestConditionStr()
        {
            string testConditionStr = "BIN,";

            foreach (var data in ResultTitleInfo)
            {
                string key = data.Key;

                if (key != "TEST" && key != "BIN")
                {
                    bool isFound = false;
                    foreach (var tData in Product.TestCondition.TestItemArray)
                    {
                        foreach (var rData in tData.MsrtResult)
                        {
                            if (rData.KeyName == key)
                            {
                                switch (key)
                                {                                 

                                    default:
                                        {
                                            if (tData.ElecSetting != null && tData.ElecSetting.Length > 0)
                                            {
                                                testConditionStr += tData.ElecSetting[0].Name;

                                                if (tData.ElecSetting[0].MsrtUnit != "")
                                                {
                                                    testConditionStr += "(" + tData.ElecSetting[0].ForceUnit + ")";
                                                }

                                            }
                                            isFound = true;
                                        }
                                        break;
                                }
                            }
                        }

                        if (isFound)
                        {
                            break;
                        }
                    }
                    testConditionStr += ",";
                }
            }


            if (testConditionStr.Length > 0 && testConditionStr[testConditionStr.Length - 1] == ',')
            {
                testConditionStr = testConditionStr.Remove(testConditionStr.Length - 1);
            }

            return testConditionStr;
        }

        private string BinStr()
        {
            string binStr = string.Empty;
            
            foreach (var data in ResultTitleInfo)
            {
                string key = data.Key;

                if (key != "TEST" && key != "BIN")
                {
                    bool isFound = false;
                    foreach (var tData in Product.TestCondition.TestItemArray)
                    {
                        foreach (var rData in tData.MsrtResult)
                        {
                            if (rData.KeyName == key)
                            {
                                if (tData.ElecSetting != null && tData.ElecSetting.Length > 0)
                                {
                                    binStr += "@" + tData.ElecSetting[0].ForceValue;
                                }
                                isFound = true;
                            }
                        }

                        if (isFound)
                        {
                            break;
                        }
                    }
                    
                }

                if (key != "TEST" && key != "BIN")
                {
                    binStr += ",";
                }
            }

            if (binStr.Length > 0 && binStr[binStr.Length - 1] == ',')
            {
                binStr = binStr.Remove(binStr.Length - 1);
            }

            return binStr;
        }

        private string targetStr()
        {
            DateTime dt = this.TesterSetting.StartTestTime;
            DateTime dtBase = new DateTime(2012, 2, 21, 0, 0, 0);

            //Calculate Target
            TimeSpan ts1 = new TimeSpan(dt.Ticks);
            TimeSpan ts2 = new TimeSpan(dtBase.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            int day = ts.Days % 256;

            if (dt.Hour >= 12)
            {
                day++;
            }

            string targetStr = ((double)day * 52.34158).ToString();

            targetStr = "";

            foreach (var data in ResultTitleInfo)
            {
                string key = data.Key;

                if (key != "TEST" && key != "BIN")
                {
                    targetStr += ",";
                }
            }
            if (targetStr.Length > 0 && targetStr[targetStr.Length - 1] == ',')
            {
                targetStr = targetStr.Remove(targetStr.Length - 1);
            }

            return targetStr;
        }

        private string LimitStr(bool isUpperSide)
        {
            string LimStr = string.Empty;


            foreach (var data in ResultTitleInfo)
            {
                string key = data.Key;

                if (key != "TEST")
                {
                    bool isFound = false;
                    foreach (var tData in Product.TestCondition.TestItemArray)
                    {

                        foreach (var rData in tData.MsrtResult)
                        {
                            if (key == rData.KeyName &&
                                rData.IsEnable && rData.IsVision && tData.IsEnable)
                            {
                                if (isUpperSide)
                                {
                                    LimStr += rData.MaxLimitValue.ToString(rData.Formate);
                                }
                                else
                                {
                                    LimStr += rData.MinLimitValue.ToString(rData.Formate);
                                }
                                isFound = true;
                                break;
                            }
                            if (isFound)
                                break;
                        }
                    }

                    LimStr += ",";
                }
            }

            if (LimStr.Length > 0 && LimStr[LimStr.Length - 1] == ',')
            {
                LimStr = LimStr.Remove(LimStr.Length - 1);
            }

            return LimStr;
        }

        private bool IsStartInRefList(string strIn,List<string> strList)
        {
            if (strList != null)
            {
                foreach (string str in strList)
                {
                    if (strIn.StartsWith(str))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string StrArrToString(string[] strArr,string splitStr)
        {
            string str = "";
            if (strArr != null)
            {
                int leng_min1 = strArr.Length - 1;
                for (int i = 0; i < strArr.Length; ++i)
                {
                    str += strArr[i];
                    if (i != leng_min1)
                    {
                        str += splitStr;
                    }
                }
            }
            return str;
        }

        private Dictionary<string, string> FindInfoFromKeyInData(string waferID)
        {
            Dictionary<string, string> keyInfoDic = new Dictionary<string, string>();

            try
            {
                if (UISetting.OptoTechKeyInDataPath != null && UISetting.OptoTechKeyInDataPath != "" &&
                    File.Exists(UISetting.OptoTechKeyInDataPath))
                {
                    using (StreamReader sr = new StreamReader(UISetting.OptoTechKeyInDataPath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string line = sr.ReadLine();
                            string[] strArr = line.Split(',');
                            if (strArr != null && strArr.Length == 7)
                            {
                                if (strArr[0] == waferID)
                                {
                                    keyInfoDic.Add("DeviceNumber", strArr[1]);
                                    keyInfoDic.Add("Remark3", strArr[2]);
                                    keyInfoDic.Add("ProductName", strArr[3]);
                                    keyInfoDic.Add("CodeNumber", strArr[4]);
                                    keyInfoDic.Add("SerialNumber", strArr[5]);
                                    keyInfoDic.Add("CommPort", strArr[6]);

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Report.OptoTech],FindInfoFromKeyInData(),Excpetion :" + e.Message);
            }
            return keyInfoDic;
        }

        private string ReturnValueOrEmpty(Dictionary<string, string>dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            return "";
 
        }

        private TestResultData GetMsrtDataFromKey(string key)
        {
            TestResultData mData = null;
            if (Product.TestCondition.TestItemArray != null)
            {
                foreach (var tData in Product.TestCondition.TestItemArray)
                {
                    foreach (var rData in tData.MsrtResult)
                    {
                        if (rData.KeyName == key)
                        {
                            return rData;
                        }
                    }
                }
            }
            return mData;
 
        }
        #endregion
        
        #region

        protected class HeaderFinder_ByEndStr : HeaderFinder
        {
            bool startCount = false;

            public HeaderFinder_ByEndStr(string tarStr, int shift)
                : base(tarStr, shift)
            {
                TarStr = tarStr;
                ShidftRow = shift;
            }

            public override bool CheckIfRowData(string str)
            {
                if (str.EndsWith(TarStr))
                {
                    startCount = true;
                }

                if (startCount)
                {
                    ShidftRow--;
                    if (ShidftRow <= 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion
    }
}
