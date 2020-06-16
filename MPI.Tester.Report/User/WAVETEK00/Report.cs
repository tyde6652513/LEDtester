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
        bool IsCvTest = false;
        Dictionary<string, MsrtInfo> MsrtkeyInfoDic;

        Dictionary<int,string> IndexMsrtKeyDic;

        protected DateTime _refSearchTime = DateTime.Now;

        public Report(List<object> objs, bool isReStatistic)
            : base(objs, isReStatistic)
        {
            _saveMapAtAbort = false;
        }

        protected override void SetResultTitle()
        {
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);


            this._refSearchTime = UISetting.WaferBeginTime;

            //this._titleStrKey = 

            MsrtkeyInfoDic = new Dictionary<string, MsrtInfo>();

            IndexMsrtKeyDic = new Dictionary<int, string>();
            if (this.Product.TestCondition != null &&
this.Product.TestCondition.TestItemArray != null &&
this.Product.TestCondition.TestItemArray.Length > 0)
            {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                {
                    if (testItem is LCRTestItem)
                    {
                        IsCvTest = true;
                    }

                    if (testItem.IsEnable)
                    {
                        foreach (var rData in testItem.MsrtResult)
                        {
                            if (rData.IsEnable && rData.IsVision)
                            {
                                string msrtKey = rData.KeyName;
                                int colIndex = 0;

                                foreach(var pData in this.UISetting.UserDefinedData.ResultItemNameDic)
                                {
                                    if (pData.Key == msrtKey)
                                    {
                                        MsrtInfo mData = new MsrtInfo(rData, testItem, colIndex);

                                        MsrtkeyInfoDic.Add(msrtKey, mData);
                                        IndexMsrtKeyDic.Add(colIndex, msrtKey);
                                        break;
                                    }
                                    colIndex++;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override EErrorCode WriteReportHeadByUser()
        {

            

            this.WriteLine("LotNumber," + UISetting.LotNumber );

            this.WriteLine("OP," + UISetting.OperatorName);

            this.WriteLine("SlotNum," + UISetting.WaferNumber);

            this.WriteLine("Stage,");//站別(CP1 = elec,CP2 = Cp)

            this.WriteLine("sub_wafer_Num,"); //sub_wafer_Num(Q00為全片,Q01為破片1Q02為破片2)

            this.WriteLine("Test_Times,"); //Test Times(測試次數，地一次Retest為Time2)

            this.WriteLine("Product_Type," + UISetting.ProductName);

            this.WriteLine("Recipe," + this.Product.ProductName);

            this.WriteLine("MachineName," + UISetting.MachineName);


            string starttime = this._refSearchTime.ToString("yyyy/MM/dd HH:mm:ss");

            if (this.UISetting.IsManualRunMode)
            {
                starttime = this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss");
            }

            this.WriteLine("TestStartTime," + starttime);


            this.WriteLine("Samples,");

            this.WriteLine("TestEndTime,");

            this.WriteLine("EdgeSensorName,"  + this.UISetting.EdgeSensorName);

            int chNum = 1;
            if (MachineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                chNum = MachineConfig.ChannelConfig.ChannelCount;
            }

            this.WriteLine("ChannelQty," + chNum);

            this.WriteLine(ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
        {
            int binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);
            //SmartBinDataBase bin = this.GetBinNum.GetBinFromSN(binSN);

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

            foreach (var item in ResultTitleInfo)
            {

                //TESTSTARTTIME\
                string format = string.Empty;
                switch (item.Key)
                {
                    case "BIN_CODE":
                        line += binCode;
                        break;
                    case "BIN_NUMBER":
                        line += binNumber.ToString();
                        break;
                    case "BIN_GRADE":
                        line += binGrade.ToString();
                        break;

                    case "TESTSTARTTIME":
                        if (this.UISetting.UserDefinedData[item.Key] != null)
                        {
                            format = UISetting.UserDefinedData[item.Key].Formate;
                        }
                        line += AcquireData.ChipInfo.StartTime.ToString(format);
                        break;

                    case "TESTENDTIME":
                        if (this.UISetting.UserDefinedData[item.Key] != null)
                        {
                            format = UISetting.UserDefinedData[item.Key].Formate;
                        }
                        line += AcquireData.ChipInfo.EndTime.ToString(format);
                        break;
                    default:
                        if (data.ContainsKey(item.Key))
                        {
                            if (this.UISetting.UserDefinedData[item.Key] != null)
                            {
                                format = UISetting.UserDefinedData[item.Key].Formate;
                            }

                            line += data[item.Key].ToString(format);
                        }
                        break;

                }
                index++;

                if (index != ResultTitleInfo.ResultCount)
                {
                    line += ",";
                }
            }

            this.WriteLine(line);

            return EErrorCode.NONE;
        }


        protected override EErrorCode RewriteReportByUser()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string endTime = "TestEndTime," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm:ss.fff");

            replaceData.Add("TestEndTime,", endTime);

            string testCount = "Samples," + this._checkColRowKey.Count.ToString() ;

            replaceData.Add("Samples,", testCount);

            string starttime = this._refSearchTime.ToString("yyyy/MM/dd HH:mm:ss");

            if (this.UISetting.IsManualRunMode)
            {
                starttime = this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss");
            }

            replaceData.Add("TestStartTime,", "TestStartTime," + starttime);

            this.ReplaceReportData4WTK(replaceData,this.TempFullFileName,this.FileFullNameRep);
            return EErrorCode.NONE;
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

            if (this.UISetting.IsManualRunMode)
            {
                isOutputPath02 = this.UISetting.IsEnableManualPath02;

                isOutputPath03 = this.UISetting.IsEnableManualPath03;

                outPath01 = this.UISetting.ManualOutputPath01;

                outPath02 = this.UISetting.ManualOutputPath02;

                outPath03 = this.UISetting.ManualOutputPath03;

                type01 = this.UISetting.ManualOutputPathType01;

                type02 = this.UISetting.ManualOutputPathType02;

                type03 = this.UISetting.ManualOutputPathType03;
            }
            else
            {
                isOutputPath02 = this.UISetting.IsEnablePath02;

                isOutputPath03 = this.UISetting.IsEnablePath03;

                outPath01 = this.UISetting.TestResultPath01;

                outPath02 = this.UISetting.TestResultPath02;

                outPath03 = this.UISetting.TestResultPath03;

                type01 = this.UISetting.TesterResultCreatFolderType01;

                type02 = this.UISetting.TesterResultCreatFolderType02;

                type03 = this.UISetting.TesterResultCreatFolderType03;
            }

            if (type01 == ETesterResultCreatFolderType.ByLotNumber)
            {
                outPath01 = Path.Combine(outPath01, this.UISetting.LotNumber);
            }
            else if (type01 == ETesterResultCreatFolderType.ByMachineName)
            {
                outPath01 = Path.Combine(outPath01, this.UISetting.MachineName);
            }
            else if (type01 == ETesterResultCreatFolderType.ByDataTime)
            {
                outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            }

            if (type02 == ETesterResultCreatFolderType.ByLotNumber)
            {
                outPath02 = Path.Combine(outPath02, this.UISetting.LotNumber);
            }
            else if (type02 == ETesterResultCreatFolderType.ByMachineName)
            {
                outPath02 = Path.Combine(outPath02, this.UISetting.MachineName);
            }
            else if (type02 == ETesterResultCreatFolderType.ByDataTime)
            {
                outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            }

            if (type03 == ETesterResultCreatFolderType.ByLotNumber)
            {
                outPath03 = Path.Combine(outPath03, this.UISetting.LotNumber);
            }
            else if (type03 == ETesterResultCreatFolderType.ByMachineName)
            {
                outPath03 = Path.Combine(outPath03, this.UISetting.MachineName);
            }
            else if (type03 == ETesterResultCreatFolderType.ByDataTime)
            {
                outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            }

            //---------------------------------------------------------------------------------
            // Copy Report file to taget path
            //---------------------------------------------------------------------------------
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            string fileNameWithExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)//Software #33246 提出的邏輯 = > 是六份不出,Csv要出
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.TestResultFileExt;

            string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

            string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

            string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

            CheckIfFileExist(outputPathAndFile01);

            MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile01);

            if (isOutputPath02)
            {
                CheckIfFileExist(outputPathAndFile02);
                MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile02);
            }

            if (isOutputPath03)
            {
                CheckIfFileExist(outputPathAndFile03);
                MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile03);
            }

            EErrorCode err = EErrorCode.NONE;
            if (cmd != EServerQueryCmd.CMD_TESTER_ABORT)
            {
                 err = PostProcess(this.FileFullNameRep);
            }
            return err;
        }

        private static void CheckIfFileExist(string outputPath)
        {
            if (File.Exists(outputPath))
            {
                DateTime dt = File.GetLastWriteTime(outputPath);
                string folder = Path.GetDirectoryName(outputPath);
                string fileNameNoExt = Path.GetFileNameWithoutExtension(outputPath);
                string ext = Path.GetExtension(outputPath);
                MPIFile.CopyFile(outputPath, Path.Combine(folder, fileNameNoExt + "_" + dt.ToString("yyMMdd_HHmmss") + ext));
            }
        }


        private void ReplaceReportData4WTK(Dictionary<string, string> replaceData, string inputFile, string outputFile)
        {
            ///////////////////////////////////////////////////////
            //Set Statistic Data
            ///////////////////////////////////////////////////////
            Dictionary<string, double> data = new Dictionary<string, double>();

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
            int timeCol = GetIndexOfKey();
            HeaderFinder hf = new HeaderFinder(this.TitleStrKey, TitleStrShift);
            // 開始比對ColRowKey並寫檔
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();

                string[] rawData  =null;
                if (line != null && line != string.Empty && line != "")
                {
                    rawData = line.Split(this.SpiltChar);
                }

                if (isRawData && rawData != null)
                {
                    if (this.TesterSetting.IsCheckRowCol)
                    {
                        rawLineCount++;

                        string colrowKey = ColRowKeyMaker(rawData);

                        // 把 row.col 和 checkRowCol "raw line count " 相同時, 才會push資料,解決當點重測row,col的問題
                        if (!(this._checkColRowKey.ContainsKey(colrowKey) && this._checkColRowKey[colrowKey] == rawLineCount))
                        {
                            continue;

                            //continue;
                        }
                        else
                        {
                            DateTime dt;
                            if (this.UISetting.IsRetest && DateTime.TryParse(rawData[timeCol], out dt))//retest 模式才進行判斷
                            {
                                if (this._refSearchTime > dt)//上一次上片資料
                                {
                                    continue;
                                }
                            }
                        }

                    }

                    //DateTime dt;
                    //if (DateTime.TryParse(rawData[timeCol], out dt))
                    //{
                    //    if (UISetting.WaferBeginTime > dt)//上一次上片資料
                    //    {
                    //        continue;
                    //    }
                    //}

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
                    if (hf.CheckIfRowData(line))
                    {
                        isRawData = true;
                    }
                    else
                    {
                        //這樣寫abort時Start time會錯
                        //if (replaceData.ContainsKey(line))
                        //{
                        //    line = replaceData[line];
                        //}

                        foreach (var p in replaceData)
                        {
                            if (line.StartsWith(p.Key))
                            {

                                if (this.UISetting.IsAppendForWaferBegine && line.StartsWith("TestStartTime,"))
                                {
                                    string[] strArr = line.Split(',');
                                    if (strArr != null && strArr.Length > 1 && DateTime.TryParse(strArr[1], out this._refSearchTime))
                                    {

                                    }
                                }
                                else
                                {
                                    line = replaceData[p.Key];
                                }
                            }
                        }
                    }
                }

                sw.WriteLine(line);
            }

            sr.Close();

            sr.Dispose();

            sw.Close();

            sw.Dispose();
        }

        private int GetIndexOfKey()
        {
            int col = -1;
            int counter = 0;
            foreach (var item in _resultTitleInfo)
            {
                if (item.Key == "TESTSTARTTIME")
                {
                    col = counter;
                }
                counter++;
            }
            return col;
        }

    }
}
