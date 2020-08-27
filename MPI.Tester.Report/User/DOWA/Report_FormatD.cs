using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;

using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;
using MPI.Tester.Report.BaseMethod.MapReader;

namespace MPI.Tester.Report.User.DOWA
{
    partial class Report
    {
        private EErrorCode WriteReportHeadByUser_FormatD()
        {

            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("FileName" + this.SpiltChar.ToString() + Path.Combine(this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt));

            this.WriteLine("File Format" + this.SpiltChar.ToString() + this.UISetting.UserID.ToString() + "_" + this.UISetting.FormatName);

            this.WriteLine("TestTime" + this.SpiltChar.ToString() + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

            this.WriteLine("EndTime");

            this.WriteLine("TesterModel" + this.SpiltChar.ToString() + this.MachineInfo.TesterSN);

            this.WriteLine("MachineName" + this.SpiltChar.ToString() + this.UISetting.MachineName);

            this.WriteLine("WaferNumber" + this.SpiltChar.ToString() + this.UISetting.WaferNumber);

            this.WriteLine("LotNumber" + this.SpiltChar.ToString() + this.UISetting.LotNumber);

            this.WriteLine("Recipe" + this.SpiltChar.ToString() + this.UISetting.ProductFileName);

            this.WriteLine("Operator" + this.SpiltChar.ToString() + this.UISetting.OperatorName);

            this.WriteLine("Samples");

            this.WriteLine("Stage" + this.SpiltChar.ToString() + _stg.ToString());

            this.WriteLine("");

            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,RltUnit,Wavelengh");


            string lastWaveLength = "";
            if (this.Product.TestCondition != null &&
                this.Product.TestCondition.TestItemArray != null &&
                this.Product.TestCondition.TestItemArray.Length > 0)
            {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                {
                    if (testItem.Type == ETestType.LaserSource)
                    {
                        int ch = (testItem as LaserSourceTestItem).LaserSourceSet.SysChannel;
                        try
                        {
                            foreach (var lSet in this.MachineConfig.LaserSrcSysConfig.ChConfigList)
                            {
                                if (lSet.SysChannel == ch)
                                {
                                    lastWaveLength = lSet.ChannelName;
                                }
                            }
                        }
                        catch (Exception e)
                        { }
                    }
                    if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || testItem.GainOffsetSetting == null)
                    {
                        continue;
                    }

                    foreach (var msrtItem in testItem.MsrtResult)
                    {
                        string line = string.Empty;
                        if (!msrtItem.IsEnable || !msrtItem.IsVision || testItem.ElecSetting == null)
                        {
                            switch (testItem.Type)
                            {
                                case ETestType.CALC:
                                    {
                                        line += msrtItem.Name;

                                        line += this.SpiltChar.ToString() + this.SpiltChar.ToString() + this.SpiltChar.ToString() + this.SpiltChar.ToString() + this.SpiltChar.ToString();

                                        line += this.SpiltChar.ToString() + msrtItem.MinLimitValue;

                                        line += this.SpiltChar.ToString() + msrtItem.MaxLimitValue;

                                        line += this.SpiltChar.ToString() + msrtItem.Unit;

                                        line += this.SpiltChar.ToString() + "\"" + GetCalcItemDescription(testItem as CALCTestItem) + "\"";

                                        this.WriteLine(line);
                                    }
                                    break;
                            }

                            continue;
                        }

                        line += msrtItem.Name;

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceValue.ToString();

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceUnit;

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceTime.ToString();

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtProtection.ToString();

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtUnit;

                        line += this.SpiltChar.ToString() + msrtItem.MinLimitValue;

                        line += this.SpiltChar.ToString() + msrtItem.MaxLimitValue;

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtUnit;

                        line += this.SpiltChar.ToString() + lastWaveLength;

                        this.WriteLine(line);
                    }
                }
            }

            this.WriteLine("");

            this.WriteLine(this._resultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        private EErrorCode PushDataByUser_FormatD(Dictionary<string, double> data)
        {
            int binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

            int binNumber = 0;

            string binCode = string.Empty;

            if (bin != null)
            {
                binCode = bin.BinCode;

                binNumber = bin.BinNumber;
            }

            string line = string.Empty;

            int index = 0;

            foreach (var item in this._resultTitleInfo)
            {
                string format = string.Empty;

                if (this.UISetting.UserDefinedData[item.Key] != null)
                {
                    format = this.UISetting.UserDefinedData[item.Key].Formate;
                }

                switch (item.Key)
                {
                    case "TestTemp1":
                        {
                            if (_stg == ETestStage.IV)
                            {
                                line += this.AcquireData.ChipInfo.ChuckTemp.ToString(format);
                            }
                        }
                        break;
                    case "TestTemp2":
                        {
                            if (_stg == ETestStage.LCR)
                            {
                                line += this.AcquireData.ChipInfo.ChuckTemp.ToString(format);
                            }
                        }
                        break;
                    case "BINCV":
                        {
                            if (_stg == ETestStage.LCR)
                            {
                                line += data["BIN"].ToString(format);
                            }
                        }
                        break;
                    //case "AOISIGN":
                    //    {
                    //        line += this.AcquireData.ChipInfo.AOISign;
                    //    }
                    default:
                        {
                            if (data.ContainsKey(item.Key))
                            {
                                line += data[item.Key].ToString(format);
                            }
                        }
                        break;
                }


                index++;

                if (index != this._resultTitleInfo.ResultCount)
                {
                    line += this.SpiltChar.ToString(); ;
                }
            }

            this.WriteLine(line);

            return EErrorCode.NONE;
        }

        private EErrorCode RewriteReportByUser_FormatD()
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
                this.ReplaceReportData_FormatD(replaceData, this.FileFullNameTmp, this.FileFullNameRep, false);
            }

            return EErrorCode.NONE;
        }

        #region
        private void ReplaceReportData_FormatD(Dictionary<string, string> replaceData, string inputFile, string outputFile, bool isSkipWritingTestCount = false)
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

            int testCount = 1;

            int shiftCount = TitleStrShift;

            int colAOI_SIGN = _resultTitleInfo.GetIndexOfKey("AOISIGN");

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

            testCount = WriteRestAOI_SIGN(sw, testCount, colAOI_SIGN);



            sr.Close();

            sr.Dispose();

            sw.Close();

            sw.Dispose();
        }

        private int WriteRestAOI_SIGN(StreamWriter sw, int testCount, int colAOI_SIGN)
        {
            if (_posAOIDic != null && _posAOIDic.Count > 0)
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

                        if (i != colLength - 1)
                        {
                            outStr += this.SpiltChar.ToString();
                        }
                        
                    }
                    sw.WriteLine(outStr);
                    testCount++;
                }

            }
            return testCount;
        }

        private string GetCalcItemDescription(CALCTestItem cItem)
        {

            string cmd = cItem.UserCommand;
            if (!cItem.IsAdvanceMode)
            {
                double gain = cItem.Gain;
                cmd = gain == 1 ? "" : "(";
                string AStr = cItem.IsAConst ? cItem.ValA.ToString() : cItem.ItemNameA;
                string BStr = cItem.IsBConst ? cItem.ValB.ToString() : cItem.ItemNameB;

                switch (cItem.CalcType)
                {
                    case ECalcType.Add:
                        cmd += AStr + "+" + BStr;
                        break;
                    case ECalcType.Subtract:
                        cmd += AStr + "-" + BStr;
                        break;
                    case ECalcType.Multiple:
                        cmd += AStr + "*" + BStr;
                        break;
                    case ECalcType.DivideBy:
                        cmd += AStr + "/" + BStr;
                        break;
                    case ECalcType.DeltaR:
                        cmd += "Resistance (" + AStr + "," + BStr + ")";
                        break;
                }

                cmd = gain == 1 ? cmd : cmd + ")";
            }
            else
            {
                cmd = cmd.Replace("\n", " ");
            }
            return cmd;
        }

        private EErrorCode GetRefDieData(string refFileFolder, string refFileNameWithNoExt)
        {
            EErrorCode err = Data.EErrorCode.NONE;
            Console.WriteLine("[DOWAReport], GetRefDieData");
            if (!MPIFile.IsAccessableIP(refFileFolder))
            {
                Console.WriteLine("[DOWAReport], GetRefDieData  IsAccessableIP fail.");
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
                case ETestStage.LCR:
                    strExt.Add("_LCR");
                    strExt.Add("_CV");
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
                Console.WriteLine("[DOWAReport], GetRefDieData,dobule check inputFile :" + tarFileName + ": not Exist");
                return EErrorCode.NONE;
            }


            string proberTmap = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "proberTmap.csv");
            Console.WriteLine("[DOWAReport], GetRefDieData, clone to proberTmap.csv");
            if (MPIFile.CopyFile(tarFileName, proberTmap))
            {

                HeaderFinder_ByStartStr hf = new HeaderFinder_ByStartStr("X,Y,ProberBin,SubBin", 0);
                List<int> cList = new List<int>();
                cList.Add(0);
                cList.Add(1);
                PosKeyMakerBase pMaker = new PosKeyMakerBase(0, 1, this.CoordTransTool);
                List<string> sList = new List<string>();
                sList.Add("X");
                sList.Add("Y");
                MapReader_DOWA mReader = new MapReader_DOWA(hf, pMaker, sList);

                Console.WriteLine("[DOWAReport], GetRefDieData, ReadMapFromFile:" + proberTmap);
                _posAOIDic = mReader.ReadMapFromFile(proberTmap);
            }

            return err;
        }

        private string GetAOISign(string colrowKey)
        {
            if (_posAOIDic != null && _posAOIDic.ContainsKey(colrowKey))
            {
                string aoiSign = _posAOIDic[colrowKey].SIGN;
                _posAOIDic.Remove(colrowKey);
                return aoiSign;

            }
            return "";
        }

        #endregion

        #region

      

        #endregion
    }

    public class DOWA_AOISignItem : IMapItem//最基本的紀錄型別
    {
        public string SIGN = "";
        public int X = 0;
        public int Y = 0;

        public DOWA_AOISignItem()
        {
        }

        public bool SetRowData(string str, List<string> refColList)
        {
            int index = refColList.IndexOf("AOI_SIGN");
            int indexX = refColList.IndexOf("X");
            int indexY = refColList.IndexOf("Y");
            string[] strArr = str.Split(',');
            if (index >= 0 && indexX >= 0 && indexY >= 0)
            {
                if (strArr != null && strArr.Length > index)
                {
                    SIGN = strArr[index];
                    int.TryParse(strArr[indexX], out X);
                    int.TryParse(strArr[indexY], out Y);
                }

            }
            return true;
        }

    }
}
