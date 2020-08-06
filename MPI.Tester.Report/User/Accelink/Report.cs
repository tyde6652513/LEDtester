using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Data;

using System.IO;
using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;
using MPI.Tester.Report.BaseMethod.MapReader;

namespace MPI.Tester.Report.User.Accelink
{
    partial class Report : ReportBase
    {
        ETestStage _stg = ETestStage.IV;
        Dictionary<string, AOI_OCR_SignItem> _posAOIDic = new Dictionary<string, AOI_OCR_SignItem>();
        List<string> _testedPosList = new List<string>(); 

        public Report(List<object> objs, bool isReStatistic)
            : base(objs, isReStatistic)
        {
        }

        protected override void SetResultTitle()
        {
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
        }

        protected override EErrorCode WriteReportHeadByUser()
        {
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("FileName" + this.SpiltChar.ToString() + "\"" + Path.Combine(this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt) + "\"");

            this.WriteLine("UserID" + this.SpiltChar.ToString() + "\"" + this.UISetting.UserID.ToString() + "_" + this.UISetting.FormatName + "\"");

            this.WriteLine("TestTime" + this.SpiltChar.ToString() + "\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\"");

            this.WriteLine("EndTime" + this.SpiltChar.ToString() + "\"" + "\"");

            this.WriteLine("TesterModel" + this.SpiltChar.ToString() + "\"" + this.MachineConfig.TesterModel + "/" + this.MachineConfig.TesterSN + "\"");

            this.WriteLine("MachineName" + this.SpiltChar.ToString() + "\"" + this.UISetting.MachineName + "\"");

            this.WriteLine("Operator" + this.SpiltChar.ToString() + "\"" + this.UISetting.OperatorName + "\"");

            this.WriteLine("LotNumber" + this.SpiltChar.ToString() + "\"" + this.UISetting.LotNumber + "\"");

            this.WriteLine("Substrate" + this.SpiltChar.ToString() + "\"" + this.UISetting.Substrate + "\"");

            this.WriteLine("TaskFile" + this.SpiltChar.ToString() + "\"" + this.UISetting.TaskSheetFileName + "\"");

            this.WriteLine("Recipe" + this.SpiltChar.ToString() + "\"" + this.UISetting.ProductFileName + "\"");

            this.WriteLine("ConditionFileName" + this.SpiltChar.ToString() + "\"" + this.UISetting.ConditionFileName + "\"");

            this.WriteLine("BinFileName" + this.SpiltChar.ToString() + "\"" + this.UISetting.BinDataFileName + "\"");
            
            this.WriteLine("Samples" + this.SpiltChar.ToString() + "\"\"");

            this.WriteLine("Stage" + this.SpiltChar.ToString() + _stg.ToString());

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Test Item Info
            ////////////////////////////////////////////
            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

            Dictionary<string, GainOffsetData> gainOffsetData = new Dictionary<string, GainOffsetData>();

            if (this.Product.TestCondition != null &&
                this.Product.TestCondition.TestItemArray != null &&
                this.Product.TestCondition.TestItemArray.Length > 0)
            {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                {
                    if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || testItem.GainOffsetSetting == null)
                    {
                        continue;
                    }

                    foreach (var data in testItem.GainOffsetSetting)
                    {
                        if (!data.IsEnable || !data.IsVision)
                        {
                            continue;
                        }

                        gainOffsetData.Add(data.KeyName, data);
                    }

                    foreach (var msrtItem in testItem.MsrtResult)
                    {
                        if (!msrtItem.IsEnable || !msrtItem.IsVision || testItem.ElecSetting == null)
                        {
                            continue;
                        }

                        double sqr = 0.0d;

                        double gain = 1.0d;

                        double offset = 0.0d;

                        if (gainOffsetData.ContainsKey(msrtItem.KeyName))
                        {
                            sqr = gainOffsetData[msrtItem.KeyName].Square;

                            gain = gainOffsetData[msrtItem.KeyName].Gain;

                            offset = gainOffsetData[msrtItem.KeyName].Offset;
                        }

                        string line = string.Empty;

                        line += msrtItem.Name;

                        if (testItem.ElecSetting.Length < 1)
                        {
                            line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceValue.ToString();

                            line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceUnit;

                            line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceTime.ToString();

                            line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtProtection.ToString();

                        }
                        else
                        {
                            for (int i = 0; i < 4; ++i)
                            {
                                line += this.SpiltChar.ToString();
                            }
                        }

                        line += this.SpiltChar.ToString() + msrtItem.Unit;

                        line += this.SpiltChar.ToString() + msrtItem.MinLimitValue;

                        line += this.SpiltChar.ToString() + msrtItem.MaxLimitValue;

                        line += this.SpiltChar.ToString() + sqr.ToString();

                        line += this.SpiltChar.ToString() + gain.ToString();

                        line += this.SpiltChar.ToString() + offset.ToString();

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtUnit;

                        this.WriteLine(line);
                    }
                }
            }

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////
            this.WriteLine(this._resultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser()
        {
            string outputFile = UISetting.MapPath;

            string fileName = UISetting.WaferNumber + ".tmap";
            string mapFolder = "";
            ETesterResultCreatFolderType ftype = UISetting.MapPathInfo.FolderType;
            mapFolder = GetPathWithFolder(outputFile, ftype, "tmap");

            GetRefDieData(mapFolder, fileName);

            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string endTime = "EndTime" + this.SpiltChar.ToString() + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm");

            replaceData.Add("EndTime", endTime);

            string testCount = "Samples" + this.SpiltChar.ToString() + this._checkColRowKey.Count.ToString();

            replaceData.Add("Samples", testCount);

            if (File.Exists(this.FileFullNameTmp))
            {
                this.ReplaceReportData_AcceLink(replaceData, this.FileFullNameTmp, this.FileFullNameRep, false);
            }

            return EErrorCode.NONE;
        }

        #region
        private void ReplaceReportData_AcceLink(Dictionary<string, string> replaceData, string inputFile, string outputFile, bool isSkipWritingTestCount = false)
        {
            ///////////////////////////////////////////////////////
            //Set Statistic Data
            ///////////////////////////////////////////////////////
            Dictionary<string, double> data = new Dictionary<string, double>();

            if (this._isReStatistic)
            {

                foreach (string keyName in Enum.GetNames(typeof(ESysResultItem)))
                {
                    data.Add(keyName, 0);
                }

                for (int i = 0; i < this._resultTitleInfo.ResultCount; i++)
                {
                    string keyName = string.Empty;

                    int index = 0;

                    foreach (var item in this._resultTitleInfo)
                    {
                        if (index == i)
                        {
                            keyName = item.Key;
                        }

                        index++;
                    }

                    if (!data.ContainsKey(keyName))
                    {
                        data.Add(keyName, 0);
                    }
                }
            }

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

            int testCount = 0;

            int shiftCount = TitleStrShift;

            int colAOI_SIGN = _resultTitleInfo.GetIndexOfKey("AOISIGN");

            int colOCR_SIGN = _resultTitleInfo.GetIndexOfKey("OCR");

            HeaderFinderBase hf = new HeaderFinderBase(this.TitleStrKey, TitleStrShift);
            // 開始比對ColRowKey並寫檔
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();

                if (isRawData)
                {
                    //if (this.TesterSetting.IsCheckRowCol)
                    {
                        rawLineCount++;

                        string[] rawData = line.Split(this.SpiltChar);

                        string colrowKey = ColRowKeyMaker(rawData);

                        // 把 row.col 和 checkRowCol "raw line count " 相同時, 才會push資料,解決當點重測row,col的問題
                        if (((this._checkColRowKey.ContainsKey(colrowKey) && this._checkColRowKey[colrowKey] == rawLineCount) && this.TesterSetting.IsCheckRowCol)
                            || !this.TesterSetting.IsCheckRowCol)
                        {
                            //Rewrite TEST
                            if (this._resultTitleInfo.TestIndex >= 0)
                            {
                                rawData[this._resultTitleInfo.TestIndex] = testCount.ToString();

                                line = string.Empty;

                                for (int i = 0; i < rawData.Length; i++)
                                {
                                    if (isSkipWritingTestCount)
                                    {
                                        if (this._resultTitleInfo.TestIndex != i)
                                        {
                                            line += rawData[i];
                                        }
                                    }
                                    else
                                    {                                       
                                        if (i == colAOI_SIGN)
                                        {
                                            line += GetAOISign(colrowKey);
                                        }
                                        else if (i == colOCR_SIGN)
                                        {
                                            line += GetOCRSign( colrowKey);
                                        }
                                        else
                                        {
                                            line += rawData[i];
                                        }
                                    }

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

            testCount = WriteRestAOI_SIGN(sw, testCount, colAOI_SIGN,colOCR_SIGN);



            sr.Close();

            sr.Dispose();

            sw.Close();

            sw.Dispose();
        }

        private int WriteRestAOI_SIGN(StreamWriter sw, int testCount, int colAOI_SIGN, int colOCR_SIGN)
        {
            if (_posAOIDic != null)
            {
                foreach (string pos in _testedPosList)
                {
                    if (_posAOIDic.ContainsKey(pos))
                    {
                        _posAOIDic.Remove(pos);
                    }
                }
                if (_posAOIDic.Count > 0)
                {                    

                    int colCnt = this._resultTitleInfo.TestIndex;
                    int colX = this._resultTitleInfo.ColIndex;
                    int colY = this._resultTitleInfo.RowIndex;
                    int colLength = _resultTitleInfo.ResultCount;
                    foreach (var p in _posAOIDic)
                    {
                        string outStr = "";
                        for (int i = 0; i < colLength; ++i)
                        {
                            if (i == colCnt)//變數不給switch....
                            { outStr += testCount.ToString(); }
                            else if (i == colX)
                            { outStr += p.Value.X.ToString(); }
                            else if (i == colY)
                            { outStr += p.Value.Y.ToString(); }
                            else if (i == colAOI_SIGN)
                            { outStr += p.Value.SIGN; }
                            else if (i == colOCR_SIGN)
                            { outStr += p.Value.OCR; }

                            if (i != colLength - 1)
                            {
                                outStr += this.SpiltChar.ToString();
                            }

                        }
                        sw.WriteLine(outStr);
                        testCount++;
                    }
                }

            }
            return testCount;
        }

        private EErrorCode GetRefDieData(string refFileFolder, string refFileNameWithNoExt)
        {
            EErrorCode err = Data.EErrorCode.NONE;
            Console.WriteLine("[AcceLinkReport], GetRefDieData");
            if (!MPIFile.IsAccessableIP(refFileFolder))
            {
                Console.WriteLine("[AcceLinkReport], GetRefDieData  IsAccessableIP fail.");
                return EErrorCode.REPORT_ReplaceDataFail;
            }

            string waferIdKey = UISetting.WaferNumber;

            List<string> strExt = new List<string>();
            strExt.Add("");

            switch (Product.TestCondition.TestStage)
            {
                case ETestStage.IV:

                    strExt.Add("_IV");
                    strExt.Add("_LIV");
                    break;
                case ETestStage.Sampling:
                    strExt.Add("_Sampling");
                    strExt.Add("_S");
                    break;

            }

            string tarFileName = "";
            bool isFound = false;
            foreach (string str in strExt)
            {
                tarFileName = Path.Combine(refFileFolder, waferIdKey + str + ".tmap");
                if (File.Exists(tarFileName))
                {
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                Console.WriteLine("[AcceLinkReport], GetRefDieData,dobule check inputFile :" + tarFileName + ": not Exist");
                return EErrorCode.NONE;
            }


            string proberTmap = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "proberTmap.csv");
            Console.WriteLine("[AcceLinkReport], GetRefDieData, clone to proberTmap.csv");
            if (MPIFile.CopyFile(tarFileName, proberTmap))
            {

                HeaderFinder_ByStartStr hf = new HeaderFinder_ByStartStr("Col,Row,ProberBin,SubBin", 0);
                List<int> cList = new List<int>();
                cList.Add(0);
                cList.Add(1);
                PosKeyMakerBase pMaker = new PosKeyMakerBase(0, 1, this.CoordTransTool);
                List<string> sList = new List<string>();
                sList.Add("Col");
                sList.Add("Row");
                MapDieReader<AOI_OCR_SignItem> mReader = new MapDieReader<AOI_OCR_SignItem>(hf, pMaker, sList);

                Console.WriteLine("[AcceLinkReport], GetRefDieData, ReadMapFromFile:" + proberTmap);
                _posAOIDic = mReader.ReadMapFromFile(proberTmap);
            }

            return err;
        }

        private string GetAOISign(string colrowKey)
        {
            if (_posAOIDic != null && _posAOIDic.ContainsKey(colrowKey))
            {
                string aoiSign = _posAOIDic[colrowKey].SIGN;
                _testedPosList.Add(colrowKey);
                //_posAOIDic.Remove(colrowKey);
                return aoiSign;

            }
            return "";
        }

        private string GetOCRSign(string colrowKey)
        {
            if (_posAOIDic != null && _posAOIDic.ContainsKey(colrowKey))
            {
                string aoiSign = _posAOIDic[colrowKey].OCR;
                //_posAOIDic.Remove(colrowKey);
                return aoiSign;

            }
            return "";
        }
        #endregion

        internal class AOI_OCR_SignItem : IMapItem//最基本的紀錄型別
        {
            public string SIGN = "";
            public int X = 0;
            public int Y = 0;
            public string OCR = "";

            public AOI_OCR_SignItem()
            {
            }

            public bool SetRowData(string str, List<string> refColList)
            {
                int index = refColList.IndexOf("AOIResult");
                int indexOcr = refColList.IndexOf("OCR");
                int indexX = refColList.IndexOf("Col");
                int indexY = refColList.IndexOf("Row");
                string[] strArr = str.Split(',');
                if (index >= 0 && indexX >= 0 && indexY >= 0 && indexOcr >=0)
                {
                    if (strArr != null && strArr.Length > index)
                    {
                        SIGN = strArr[index];
                        OCR = strArr[indexOcr];
                        int.TryParse(strArr[indexX], out X);
                        int.TryParse(strArr[indexY], out Y);
                    }

                }
                return true;
            }

        }

    }
}
