using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester.TestKernel;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Report
{
    public abstract partial class ReportBase
    {
        #region >>>Sweep 01 ,For Liv Sweep, generally<<<

        protected virtual EErrorCode RunCommandByUserS01(EServerQueryCmd cmd)
        {
            switch (cmd)
            {
                case EServerQueryCmd.CMD_TESTER_START:
                    {
                        if (this._isAppend)
                        {
                            return EErrorCode.NONE;
                        }
                        else
                        {
                            return this.WriteReportHeadByUserS01(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUserS01();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUseS01(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUserS01();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUseS01(cmd);
                        }
                        else
                        {
                            return EErrorCode.NONE;
                        }
                    }
                default:
                    {
                        return EErrorCode.NONE;
                    }
            }
        }

        protected virtual EErrorCode WriteReportHeadByUserS01()
        {
            //LIV
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserS01(Dictionary<string, double> data,bool isMS)
        {

            //if (!this.UISetting.IsEnableSaveLIVData || this.AcquireData.LIVDataSet.Count == 0)
            //{
            //    return EErrorCode.NONE;
            //}

            //List<string[]> outData = DefaultPushLIV_PIV();


            //string fileName = string.Empty;
            //string fileNameWithoutExt = this.TestResultFileNameWithoutExt();
            //fileName = fileNameWithoutExt + "_LIV_PIV_" + "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")" + ".csv";

            //string fileAndPath = Path.Combine(this.UISetting.LIVDataSavePath, fileName);

            ////transpose
            //List<string[]> csvdata = TransposeCSV(outData);


            //if (!CSVUtil.WriteCSV(fileAndPath, csvdata))
            //{
            //    Console.WriteLine("[OutputBigData], PushDataByUserS01(), Write title fail.");
            //}
            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";
            string path = GetFullPathWithFolder(this.UISetting.LIVDataSavePath, this.UISetting.LIVCreatFolderType);

            if (this.UISetting.IsEnableSaveLIVData )
            {
                if (this.AcquireData.LIVDataSet.Count != 0)
                {
                    LIVDataSet livSet = this.AcquireData.LIVDataSet;
                    if (isMS)
                    {
                        //livSet = 
                    }
                    foreach (LIVData livData in livSet)
                    {
                        if (EErrorCode.NONE != DefaultPushLIV(livData, pos, path))
                        {
                            return EErrorCode.SaveFileFail;
                        }
                    }
                }

                if (this.AcquireData.PIVDataSet.Count != 0)
                {
                    PIVDataSet pivSet = this.AcquireData.PIVDataSet;
                    if (isMS)
                    {
                        //livSet = 
                    }
                    foreach (PIVData PivD in pivSet)
                    {
                        if (EErrorCode.NONE != DefaultPushPIV(PivD, pos, path))
                        {
                            return EErrorCode.SaveFileFail;
                        }
                    }
                }
            }

            return EErrorCode.NONE;
        }
        
        protected virtual EErrorCode RewriteReportByUserS01()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseS01(EServerQueryCmd cmd)
        {        
            return EErrorCode.NONE;
        }
        
        #endregion


        #region >>>Sweep 02 ,For Liv Sweep, generally<<<
        protected virtual EErrorCode RunCommandByUserS02(EServerQueryCmd cmd)
        {
        //    switch (cmd)
        //    {
        //        case EServerQueryCmd.CMD_TESTER_START:
        //            {
        //                if (this._isAppend)
        //                {
        //                    return EErrorCode.NONE;
        //                }
        //                else
        //                {
        //                    return this.WriteReportHeadByUserS02(); // *.Temp
        //                }
        //            }
        //        case EServerQueryCmd.CMD_TESTER_END:
        //            {
        //                EErrorCode code = this.RewriteReportByUserS02();

        //                if (code != EErrorCode.NONE)
        //                {
        //                    return code;
        //                }

        //                return this.MoveFileToTargetByUseS02(cmd);
        //            }
        //        case EServerQueryCmd.CMD_TESTER_ABORT:
        //            {
        //                if (this.UISetting.IsAbortSaveFile)
        //                {
        //                    EErrorCode code = this.RewriteReportByUserS02();

        //                    if (code != EErrorCode.NONE)
        //                    {
        //                        return code;
        //                    }

        //                    return this.MoveFileToTargetByUseS02(cmd);
        //                }
        //                else
        //                {
        //                    return EErrorCode.NONE;
        //                }
        //            }
        //        default:
        //            {
        //                return EErrorCode.NONE;
        //            }
        //    }
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode WriteReportHeadByUserS02()
        {
            //LIV
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////


            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserS02(Dictionary<string, double> data, bool isMS)
        {

            //if (!this.UISetting.IsEnableSaveLIVDataPath02 || this.AcquireData.LIVDataSet.Count == 0)
            //{
            //    return EErrorCode.NONE;
            //}

            //List<string[]> outData = DefaultPushLIV_PIV();


            //string fileName = string.Empty;
            //string fileNameWithoutExt = this.TestResultFileNameWithoutExt();
            //fileName = fileNameWithoutExt + "_LIV_PIV_" + "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")" + ".csv";

            //string fileAndPath = Path.Combine(this.UISetting.LIVDataSavePath02, fileName);

            ////transpose
            //List<string[]> csvdata = TransposeCSV(outData);


            //if (!CSVUtil.WriteCSV(fileAndPath, csvdata))
            //{
            //    Console.WriteLine("[OutputBigData], PushDataByUserS02(), Write title fail.");
            //}
            string path = GetFullPathWithFolder(this.UISetting.LIVDataSavePath02, this.UISetting.LIVCreatFolderType02);
            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";

            if (this.UISetting.IsEnableSaveLIVDataPath02)
            {
                LIVDataSet livSet = this.AcquireData.LIVDataSet;
                if (isMS)
                {
                    //livSet = 
                }
                foreach (LIVData livData in livSet)
                {
                    if (EErrorCode.NONE != DefaultPushLIV(livData, pos, path))
                    {
                        return EErrorCode.SaveFileFail;
                    }
                }


                PIVDataSet pivSet = this.AcquireData.PIVDataSet;
                if (isMS)
                {
                    //livSet = 
                }
                foreach (PIVData PivD in pivSet)
                {
                    if (EErrorCode.NONE != DefaultPushPIV(PivD, pos, path))
                    {
                        return EErrorCode.SaveFileFail;
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserS02()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseS02(EServerQueryCmd cmd)
        {
            return EErrorCode.NONE;
        }
        #endregion

        //保留部分F版LIV輸出所需的物件，後續若有需要可參考這段來實作
        #region >>>Sweep 03 ,For PIV Sweep, generally<<<

        public string SweepTitle3 = "";

        protected Dictionary<string, string> _sweepItemDic03;

        private StreamWriter _swS03;

        protected int _maxSweepCount03 = 0;

        private string _fileFullNameTmpS03 = "";

        private string _fileFullNameRepS03 = "";


        protected virtual EErrorCode RunCommandByUserS03(EServerQueryCmd cmd)
        {
            //switch (cmd)
            //{
            //    case EServerQueryCmd.CMD_TESTER_START:
            //        {
            //            if (this._isAppend)
            //            {
            //                return EErrorCode.NONE;
            //            }
            //            else
            //            {
            //                return this.WriteReportHeadByUserS03(); // *.Temp
            //            }
            //        }
            //    case EServerQueryCmd.CMD_TESTER_END:
            //        {
            //            EErrorCode code = this.RewriteReportByUserS03();

            //            if (code != EErrorCode.NONE)
            //            {
            //                return code;
            //            }

            //            return this.MoveFileToTargetByUseS03(cmd);
            //        }
            //    case EServerQueryCmd.CMD_TESTER_ABORT:
            //        {
            //            if (this.UISetting.IsAbortSaveFile)
            //            {
            //                EErrorCode code = this.RewriteReportByUserS03();

            //                if (code != EErrorCode.NONE)
            //                {
            //                    return code;
            //                }

            //                return this.MoveFileToTargetByUseS03(cmd);
            //            }
            //            else
            //            {
            //                return EErrorCode.NONE;
            //            }
            //        }
            //    default:
            //        {
            //            return EErrorCode.NONE;
            //        }
            //}

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode WriteReportHeadByUserS03()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserS03(Dictionary<string, double> data, bool isMS)
        {
            string path = GetFullPathWithFolder(this.UISetting.LIVDataSavePath03, this.UISetting.LIVCreatFolderType03);
            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";

            if (this.UISetting.IsEnableSaveLIVDataPath03)
            {
                LIVDataSet livSet = this.AcquireData.LIVDataSet;
                if (isMS)
                {
                    //livSet = 
                }
                foreach (LIVData livData in livSet)
                {
                    if (EErrorCode.NONE != DefaultPushLIV(livData, pos, path))
                    {
                        return EErrorCode.SaveFileFail;
                    }
                }

                PIVDataSet pivSet = this.AcquireData.PIVDataSet;
                if (isMS)
                {
                    //livSet = 
                }
                foreach (PIVData PivD in pivSet)
                {
                    if (EErrorCode.NONE != DefaultPushPIV(PivD, pos, path))
                    {
                        return EErrorCode.SaveFileFail;
                    }
                }

            }
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserS03()
        {

            return EErrorCode.NONE;
        }


        protected void WriteLineS03(string line)
        {
            if (this._swS03 != null)
            {
                this._swS03.WriteLine(line);

                this._swS03.Flush();
            }
        }

        protected void ReplaceReportS03(Dictionary<string, string> replaceData)
        {
            if (File.Exists(this._fileFullNameTmpS03))
            {
                this.ReplaceReportData(replaceData, this._fileFullNameTmpS03, this._fileFullNameRepS03);
            }
        }

        protected string FileFullNameRepS03
        {
            get { return this._fileFullNameRepS03; }
            set { this._fileFullNameRepS03 = value; }
        }

        protected string FileFullNameTmpS03
        {
            get { return this._fileFullNameTmpS03; }
            set { this._fileFullNameTmpS03 = value; }
        }

        protected void SetResultSweepTitle03ByDefault()
        {
            SweepTitle3 = "";

            GetMaxPivSwCount();

            ResetLivSwResultDic03();

            int counter = 0;

            string titleStr = string.Empty;//"TEST,PosX,PosY,BIN,POLAR";

            foreach (KeyValuePair<string, string> knPair in _sweepItemDic03)
            {
                titleStr += knPair.Value;

                ++counter;

                if (counter != _sweepItemDic03.Count)
                {
                    titleStr += ",";
                }
            }

            SweepTitle3 = titleStr.TrimEnd(new char[] { ',' });

        }

        #endregion

        #region >>>Sweep 04 ,For IV/VI Sweep, generally<<<

        public string SweepTitle4 = "";

        protected Dictionary<string, string> _sweepItemDic04;

        private StreamWriter _swS04;

        protected int _maxSweepCount04 = 0;

        private string _fileFullNameTmpS04 = "";

        private string _fileFullNameRepS04 = "";


        protected virtual EErrorCode RunCommandByUserS04(EServerQueryCmd cmd)
        {
            switch (cmd)
            {
                case EServerQueryCmd.CMD_TESTER_START:
                    {
                        if (this._isAppend)
                        {
                            return EErrorCode.NONE;
                        }
                        else
                        {
                            return this.WriteReportHeadByUserS04(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUserS04();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUseS04(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUserS04();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUseS04(cmd);
                        }
                        else
                        {
                            return EErrorCode.NONE;
                        }
                    }
                default:
                    {
                        return EErrorCode.NONE;
                    }
            }
        }

        protected virtual EErrorCode WriteReportHeadByUserS04()
        {

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserS04(Dictionary<string, double> data,bool isMS)
        {
            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";

            string path = GetPathWithFolder(UISetting.SweepPathInfo01);
            //string path = GetFullPathWithFolder(this.UISetting.SweepOutputPath, this.UISetting.SweepOutputPathType);

            if (this.UISetting.IsEnableSweepPath)
            {
                if (this.AcquireData.ElecSweepDataSet.Count != 0)
                {
                    
                    ElecSweepDataSet sweepSet = this.AcquireData.ElecSweepDataSet;
                    if (isMS)
                    {
                        //livSet = 
                    }
                    foreach (ElecSweepData sData in sweepSet)
                    {
                        if (sData.Channel == 0 )
                        {
                            if (EErrorCode.NONE != DefaultPushVI_IVSweep(sData, pos, path))
                            {
                                return EErrorCode.SaveFileFail;
                            }
                        }
                        else if (sData.SweepData.Length > 1)
                        {
                            string m_pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")_CH(" + (sData.Channel + 1).ToString() + ")";

                            if (EErrorCode.NONE != DefaultPushVI_IVSweep(sData, m_pos, path))
                            {
                                return EErrorCode.SaveFileFail;
                            }
                        }
                        
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserS04()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseS04(EServerQueryCmd cmd)
        {
            return EErrorCode.NONE;
        }

        protected void WriteLineS04(string line)
        {
            if (this._swS04 != null)
            {
                this._swS04.WriteLine(line);

                this._swS04.Flush();
            }
        }

        protected void ReplaceReportS04(Dictionary<string, string> replaceData)
        {
            //if (File.Exists(this._fileFullNameTmpS04))
            //{
            //    this.ReplaceReportData(replaceData, this._fileFullNameTmpS04, this._fileFullNameRepS04);
            //}
        }

        protected string FileFullNameRepS04
        {
            get { return this._fileFullNameRepS04; }
            set { this._fileFullNameRepS04 = value; }
        }

        protected string FileFullNameTmpS04
        {
            get { return this._fileFullNameTmpS04; }
            set { this._fileFullNameTmpS04 = value; }
        }

        protected void SetResultSweepTitle04ByDefault()
        {
            SweepTitle4 = "";

            GetMaxPivSwCount();

            //ResetLivSwResultDic04();

            int counter = 0;

            string titleStr = string.Empty;//"TEST,PosX,PosY,BIN,POLAR";

            foreach (KeyValuePair<string, string> knPair in _sweepItemDic04)
            {
                titleStr += knPair.Value;

                ++counter;

                if (counter != _sweepItemDic04.Count)
                {
                    titleStr += ",";
                }
            }

            SweepTitle4 = titleStr.TrimEnd(new char[] { ',' });

        }

        #endregion

        #region>>> protected methods<<<

        //protected virtual List<string> GetLcrCaliList()
        //{
        //    List<string> outStrList = new List<string>();

        //    //LCRCaliData cData = SysCali.SystemCaliData.LCRCaliData;

        //    int dataNum = cData.NowDataNum - 1;


        //    //string type = "CaliType:," + cData.TestType.ToString();

        //    //outStrList.Add(type);
        //    string caliVolt = "Cali Level:," + cData.Level.ToString() + "V";

        //    outStrList.Add(caliVolt);

        //    string CableLength = "Cable Length:," + cData.CableLength.ToString();

        //    outStrList.Add(CableLength);

        //    string caliBias = "Cali Bias:," + cData.Bias.ToString() + "V";

        //    outStrList.Add(caliBias);


        //    string caliItems = "Type,Enable,Msrt Type,Raw A,Raw B,Set A,SetB,Meter A,Meter B";

        //    outStrList.Add(caliItems);

        //    string openCali = "OPEN:," + (cData.EnableOpen ? "ON" : "OFF");

        //    if (cData.EnableOpen)
        //    {
        //        CaliRowData crd = cData.LoadingList[dataNum].OpenRaw;
        //        openCali +=
        //            "," + crd.CaliLCRTestType.ToString() +
        //            "," + crd.ValA.ToString("E3") + " " + crd.UnitA +
        //            "," + crd.ValB.ToString("E3") + " " + crd.UnitB +
        //            ",," +
        //            "," + crd.MeterRowValA.ToString("E3") + " " + crd.MeterUnitA +
        //            "," + crd.MeterRowValB.ToString("E3") + " " + crd.MeterUnitB;
        //    }

        //    outStrList.Add(openCali);

        //    string shortCali = "SHORT:," + (cData.EnableShort ? "ON" : "OFF");

        //    if (cData.EnableShort)
        //    {
        //        CaliRowData crd = cData.LoadingList[dataNum].ShortRaw;
        //        shortCali +=
        //            "," + crd.CaliLCRTestType.ToString() +
        //            "," + crd.ValA.ToString("E3") + " " + crd.UnitA +
        //            "," + crd.ValB.ToString("E3") + " " + crd.UnitB +
        //            ",," +
        //            "," + crd.MeterRowValA.ToString("E3") + " " + crd.MeterUnitA +
        //            "," + crd.MeterRowValB.ToString("E3") + " " + crd.MeterUnitB;
        //    }

        //    outStrList.Add(shortCali);

        //    string loadCali = "Load:," + (cData.EnableLoad ? "ON" : "OFF");

        //    if (cData.EnableLoad)
        //    {
        //        CaliRowData crd = cData.LoadingList[dataNum].LoadRaw;
        //        loadCali +=
        //            "," + crd.CaliLCRTestType.ToString() +
        //            "," + crd.ValA.ToString("E3") + " " + crd.UnitA +
        //            "," + crd.ValB.ToString("E3") + " " + crd.UnitB +
        //            "," + cData.LoadingList[dataNum].RefA.ToString("E3") + " " + crd.UnitA +
        //            "," + cData.LoadingList[dataNum].RefB.ToString("E3") + " " + crd.UnitB +
        //            "," + crd.MeterRowValA.ToString("E3") + " " + crd.MeterUnitA +
        //            "," + crd.MeterRowValB.ToString("E3") + " " + crd.MeterUnitB;
        //    }


        //    outStrList.Add(loadCali);

        //    return outStrList;
        //    //outStrList 

        //}

        protected virtual void ResetLivSwResultDic03()
        {
            this._sweepItemDic03 = new Dictionary<string, string>();

            Dictionary<string, string> MD = UISetting.UserDefinedData.MsrtDisplayItemDic;
            this._sweepItemDic03.Add("TEST", MD["TEST"]);
            this._sweepItemDic03.Add("COL", MD["COL"]);
            this._sweepItemDic03.Add("ROW", MD["ROW"]);
            this._sweepItemDic03.Add("BIN", MD["BIN"]);
            this._sweepItemDic03.Add("POLAR", MD["POLAR"]);


            if (this.Product.TestCondition != null &&
                this.Product.TestCondition.TestItemArray != null &&
                this.Product.TestCondition.TestItemArray.Length > 0)
            {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                {
                    if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable)
                    {
                        continue;
                    }
                    if (this.AcquireData.PIVDataSet != null)
                    {
                        int count = 1;
                        foreach (PIVData ls in this.AcquireData.PIVDataSet)
                        {
                            this._sweepItemDic03.Add("Curr_" + count.ToString(), "Curr_" + count.ToString());
                            this._sweepItemDic03.Add("Volt_" + count.ToString(), "Volt_" + count.ToString());
                            this._sweepItemDic03.Add("Se_" + count.ToString(), "Se_" + count.ToString());
                            this._sweepItemDic03.Add("Rs_" + count.ToString(), "Rs_" + count.ToString());
                            this._sweepItemDic03.Add("Pce_" + count.ToString(), "Pce_" + count.ToString());
                        }
                        count++;
                    }

                }
            }
        }

        protected virtual bool IsLCRListItem(string keyName)
        {
            //if (this.AcquireData.LCRDataSet != null)
            //{
            //    foreach (LCRData ls in this.AcquireData.LCRDataSet)
            //    {
            //        foreach (var va in ls)
            //        {
            //            if (va.KeyName == keyName)
            //            {
            //                return true;
            //            }
            //        }
            //    }
            //}
            return false;

        }

        protected virtual bool IsLIVListItem(string keyName)
        {
            if (this.AcquireData.LIVDataSet != null)
            {
                foreach (LIVData ls in this.AcquireData.LIVDataSet)
                {
                    foreach (var va in ls)
                    {
                        if (va.KeyName == keyName)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }

        protected virtual bool GetResultData(string keyName, out TestResultData result)
        {
            result = null;
            foreach (TestItemData ti in this.Product.TestCondition.TestItemArray)
            {
                foreach (TestResultData tr in ti.MsrtResult)
                {
                    if (tr.KeyName == keyName)
                    {
                        result = tr;
                        return true;
                    }
                }
            }

            return false;
        }

        protected virtual int GetMsrtIndex(TestItemData testItem, string keyName)
        {
            int resultIndex = 0;
            foreach (TestResultData trd in testItem.MsrtResult)
            {
                if (trd.KeyName == keyName)
                {
                    return resultIndex;
                }
                resultIndex++;
            }
            return 0;
        }

        protected virtual string FindMsrtUnit(string keyName)
        {
            string result = string.Empty;
            if (this.UISetting.UserDefinedData[keyName] != null)
            {
                result = this.UISetting.UserDefinedData[keyName].Unit;
            }

            return result;
        }

        protected virtual void GetBinNum(int binSN, out string binCode, out int binNumber, out int binGrade)
        {
            //int binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

            binGrade = 0;

            binNumber = 0;

            binCode = string.Empty;

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
        }

        protected virtual string GetBinResult(Dictionary<string, double> data, int binGrade, int binNumber, string binCode)
        {
            string binResult = data["BIN"].ToString("0");

            foreach (var item in this.ResultTitleInfo)
            {
                if (item.Key == "BIN_CODE")
                {
                    binResult = binCode;
                    break;
                }
                else if (item.Key == "BIN_NUMBER")
                {
                    binResult = binNumber.ToString();
                    break;
                }
                else if (item.Key == "BIN_GRADE")
                {
                    binResult = binGrade.ToString();
                    break;
                }
            }
            return binResult;
        }

        //protected virtual void GetMaxLcrSwCount()
        //{
        //    _maxSweepCount01 = 0;

        //    if (this.AcquireData.LCRDataSet != null)
        //    {
        //        foreach (LCRData LcrList in this.AcquireData.LCRDataSet)
        //        {
        //            if (!LcrList.IsEnable)
        //            {
        //                continue;
        //            }

        //            foreach (BaseResultData LcrData in LcrList)
        //            {
        //                if (this._maxSweepCount01 < LcrData.DataArray.Length)
        //                {
        //                    this._maxSweepCount01 = LcrData.DataArray.Length;
        //                }
        //            }
        //        }
        //    }

        //}


        protected virtual void GetMaxPivSwCount()
        {
            _maxSweepCount03 = 0;
            if (this.AcquireData.PIVDataSet != null)
            {
                foreach (PIVData Pivd in this.AcquireData.PIVDataSet)
                {
                    if (!Pivd.IsEnable)
                    {
                        continue;
                    }


                    if (this._maxSweepCount03 < Pivd.CurrentData.Length)
                    {
                        this._maxSweepCount03 = Pivd.CurrentData.Length;
                    }

                }
            }
        }
        #endregion

        protected List<string[]> TransposeCSV(List<string[]> outData)
        {
            List<string[]> csvdata = new List<string[]>();
            if (outData.Count > 0)
            {
                int maxlength = outData.Max(x => x.Length);
                
                for (int i = 0; i < maxlength; i++)
                {
                    string[] rowdata = new string[outData.Count];

                    for (int j = 0; j < outData.Count; j++)
                    {
                        if (outData[j].Length > i)
                        {
                            rowdata[j] = outData[j][i].ToString();
                        }
                    }
                    csvdata.Add(rowdata);
                }
            }
            return csvdata;
        }

        protected List<string[]> DefaultPushLIV_PIV()
        {
            List<string[]> outData = new List<string[]>();
            foreach (LIVData livData in this.AcquireData.LIVDataSet)
            {
                if (!livData.IsEnable)
                {
                    continue;
                }

                foreach (var resultItem in livData)
                {
                    if (!resultItem.IsEnable)
                    {
                        continue;
                    }

                    string[] result = new string[resultItem.DataArray.Length + 1];

                    result[0] = resultItem.Name;

                    for (int j = 0; j < resultItem.DataArray.Length; j++)
                    {
                        result[j + 1] = resultItem.DataArray[j].ToString(resultItem.Formate);
                    }

                    outData.Add(result);
                }
            }

            foreach (PIVData PivD in this.AcquireData.PIVDataSet)
            {
                if (!PivD.IsEnable && PivD.CurrentData.Length > 0)
                {
                    continue;
                }
                List<string[]> pivOutData = new List<string[]>();

                int resultCount = PivD.CurrentData.Length;

                for (int i = 0; i < 3; ++i)
                {
                    pivOutData.Add(new string[resultCount + 1]);
                    switch (i)
                    {
                        case 0:
                            pivOutData[i][0] = PivD.Name + "_Apply(A)";
                            break;
                        case 1:
                            pivOutData[i][0] = PivD.Name + "_Msrt(V)";
                            break;
                        case 2:
                            pivOutData[i][0] = PivD.Name + "_Pow(mW)";
                            break;
                    }

                    for (int j = 1; j < resultCount + 1; ++j)
                    {
                        //PIVData[i][j] = 
                        switch (i)
                        {
                            case 0:
                                pivOutData[i][j] = PivD.CurrentData[j -1].ToString("0.####");
                                break;
                            case 1:
                                pivOutData[i][j] = PivD.VoltageData[j -1].ToString("0.####");
                                break;
                            case 2:
                                pivOutData[i][j] = PivD.PowerData[j -1].ToString("0.####");
                                break;
                        }
                    }
                }
                outData.AddRange(pivOutData.ToArray());
            }
            
            return outData;
        }

        protected EErrorCode DefaultPushPIV(PIVData PivD, string pos, string outputPath)
        {
            List<string[]> outData = new List<string[]>();

            if (!PivD.IsEnable || PivD.CurrentData.Length <= 0)
            {
                return EErrorCode.NONE;
            }
           // List<string[]> pivOutData = new List<string[]>();
            List<string> pivOutData = new List<string>();
            int resultCount = PivD.CurrentData.Length;

            for (int i = 0; i < 3; ++i)
            {
                pivOutData.Clear();

                switch (i)
                {
                    case 0:
                        pivOutData.Add(PivD.Name + "_Apply(A)");
                        break;
                    case 1:
                        pivOutData.Add(PivD.Name + "_Msrt(V)");
                        break;
                    case 2:
                        pivOutData.Add(PivD.Name + "_Pow(mW)");
                        break;
                }

                for (int j = 1; j < resultCount + 1; ++j)
                {
                    //PIVData[i][j] = 
                    switch (i)
                    {
                        case 0:
                            pivOutData.Add(PivD.CurrentData[j -1].ToString("0.####"));
                            break;
                        case 1:
                            pivOutData.Add(PivD.VoltageData[j -1].ToString("0.####"));
                            break;
                        case 2:
                            pivOutData.Add(PivD.PowerData[j -1].ToString("0.####"));
                            break;
                    }
                }

                outData.Add(pivOutData.ToArray());
            }

            string fileName = string.Empty;
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();
            fileName = PivD.Name + pos + ".csv";

            //string fileAndPath = Path.Combine(this.UISetting.LIVDataSavePath, fileName);
            string fileAndPath = Path.Combine(outputPath, fileName);
            //transpose
            List<string[]> csvdata = TransposeCSV(outData);


            if (!CSVUtil.WriteCSV(fileAndPath, csvdata))
            {
                Console.WriteLine("[OutputBigData], DefaultPushPIV(),outputPath = " + outputPath + " Write title fail.");

                return EErrorCode.SaveFileFail;
            }

            return EErrorCode.NONE;
        }

        protected EErrorCode DefaultPushLIV(LIVData livData, string pos, string outputPath)
        {
            List<string[]> outData = new List<string[]>();

            if (!livData.IsEnable || livData.Count <= 0)
            {
                return EErrorCode.NONE; ;
            }

            foreach (var resultItem in livData)
            {
                if (!resultItem.IsEnable)
                {
                    continue;
                }

                string[] result = new string[resultItem.DataArray.Length + 1];

                result[0] = resultItem.Name;

                for (int j = 0; j < resultItem.DataArray.Length; j++)
                {
                    result[j + 1] = resultItem.DataArray[j].ToString(resultItem.Formate);
                }

                outData.Add(result);
            }

            string fileName = string.Empty;
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();
            fileName = livData.Name + pos + ".csv";

            //string fileAndPath = Path.Combine(this.UISetting.LIVDataSavePath, fileName);
            string fileAndPath = Path.Combine(outputPath, fileName);
            //transpose
            List<string[]> csvdata = TransposeCSV(outData);


            if (!CSVUtil.WriteCSV(fileAndPath, csvdata))
            {
                Console.WriteLine("[OutputBigData], DefaultPushLIV(),outputPath = " + outputPath + "Write title fail.");

                return EErrorCode.SaveFileFail;
            }

            return EErrorCode.NONE;
        }

        protected List<string[]> DefaultPushVI_IVSweep()
        {
            List<string[]> outData = new List<string[]>();

            if (this.AcquireData.ElecSweepDataSet != null && this.AcquireData.ElecSweepDataSet.Count > 0)
            {
                foreach (ElecSweepData sData in this.AcquireData.ElecSweepDataSet)
                {
                    if (!sData.IsEnable)
                    {
                        continue;
                    }
                    int length = this.AcquireData.ElecSweepDataSet.Count();

                    string[][] title;

                    if (sData.KeyName.Contains("VISCAN"))
                    {
                        title = new string[][] { new string[] { "VISCAN "},
                                                 new string[] { "Time(ms)", "Apply(V)", "Msrt(A)" } };
                    }
                    else if (sData.KeyName.Contains("VISWEEP"))
                    {
                        title = new string[][] { new string[] { "VISweep "},
                                                 new string[] { "Time(ms)", "Apply(V)", "Msrt(A)" } };
                    }
                    else
                    {
                        title = new string[][] { new string[] { "IVSweep "},
                                                 new string[] { "Time(ms)", "Apply(A)", "Msrt(V)" } };
                    }

                    string[][] data = new string[3][];

                    for (int i = 0; i < 3; ++i)
                    {
                        data[i] = new string[2 + sData.TimeChain.Length];
                        switch (i)
                        {
                            case 0:
                                data[0][0] = title[0][0];
                                data[0][1] = title[1][0];
                                break;
                            case 1:
                                data[1][0] = "";
                                data[1][1] = title[1][1];
                                break;
                            case 2:
                                data[2][0] = "";
                                data[2][1] = title[1][2];
                                break;
                        }
                        for (int j = 0; j < sData.TimeChain.Length; ++j)
                        {
                            switch (i)
                            {
                                case 0:
                                    data[i][j + 2] = sData.TimeChain[j].ToString();
                                    break;
                                case 1:
                                    data[i][j + 2] = sData.ApplyData[j].ToString();
                                    break;
                                case 2:
                                    data[i][j + 2] = sData.SweepData[j].ToString();
                                    break;
                            }

                        }

                    }
                    outData.AddRange(data.ToList());
                }
                
            }
            return outData;
        }

        protected EErrorCode DefaultPushVI_IVSweep(ElecSweepData sData, string pos, string outputPath)
        {
            if (!this.UISetting.IsEnableSweepPath || this.AcquireData.ElecSweepDataSet.Count == 0)
            {
                return EErrorCode.NONE;
            }

            List<string[]> outData = new List<string[]>();


            if (!sData.IsEnable || sData.TimeChain.Length <= 0)
            {
                return EErrorCode.NONE;
            }

            string[][] title;

            if (sData.KeyName.Contains("VISCAN"))
            {
                title = new string[][] { new string[] { "VISCAN "},
                                                 new string[] { "Time(ms)", "Apply(V)", "Msrt(A)" } };
            }
            else if (sData.KeyName.Contains("VISWEEP"))
            {
                title = new string[][] { new string[] { "VISweep "},
                                                 new string[] { "Time(ms)", "Apply(V)", "Msrt(A)" } };
            }
            else
            {
                title = new string[][] { new string[] { "IVSweep "},
                                                 new string[] { "Time(ms)", "Apply(A)", "Msrt(V)" } };
            }

            string[][] data = new string[3][];

            for (int i = 0; i < 3; ++i)
            {
                data[i] = new string[2 + sData.TimeChain.Length];
                switch (i)
                {
                    case 0:
                        data[0][0] = title[0][0];
                        data[0][1] = title[1][0];
                        break;
                    case 1:
                        data[1][0] = "";
                        data[1][1] = title[1][1];
                        break;
                    case 2:
                        data[2][0] = "";
                        data[2][1] = title[1][2];
                        break;
                }
                for (int j = 0; j < sData.TimeChain.Length; ++j)
                {
                    switch (i)
                    {
                        case 0:
                            data[i][j + 2] = sData.TimeChain[j].ToString();
                            break;
                        case 1:
                            data[i][j + 2] = sData.ApplyData[j].ToString();
                            break;
                        case 2:
                            data[i][j + 2] = sData.SweepData[j].ToString();
                            break;
                    }
                }
            }
            outData.AddRange(data.ToList());
            




            string fileName = string.Empty;
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();
            fileName = sData.Name + pos + ".csv";

            //string fileAndPath = Path.Combine(this.UISetting.LIVDataSavePath, fileName);
            string fileAndPath = Path.Combine(outputPath, fileName);
            //transpose
            List<string[]> csvdata = TransposeCSV(outData);


            if (!CSVUtil.WriteCSV(fileAndPath, csvdata))
            {
                Console.WriteLine("[OutputBigData], DefaultPushVI_IVSweep(),outputPath = " + outputPath + "Write title fail.");

                return EErrorCode.SaveFileFail;
            }


            return EErrorCode.NONE;
            
        }

      


    }
}
