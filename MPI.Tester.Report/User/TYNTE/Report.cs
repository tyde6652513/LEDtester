using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.Report.User.TYNTE
{
    class Report : ReportBase
    {
        private Dictionary<string, double> _gain = new Dictionary<string, double>();

        private Dictionary<string, double> _offset = new Dictionary<string, double>();


        //private string _IFMB_1_ForceTime;


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
            this.WriteLine("FileID,,\"WEI MIN Data File\"");

            this.WriteLine("FileName,,\"" + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt + "\"");

            this.WriteLine("TestTime,,\"" + "\"");

            this.WriteLine("TesterModel,,\"LED617WH\"");

            this.WriteLine("CommPort,,\"COM1\"");

            this.WriteLine("TesterNumber,,\"" + this.UISetting.MachineName + "\"");


            this.WriteLine("DeviceNumber,,\"T140168\"");

            this.WriteLine("Specification,,\"" + this.UISetting.ProductFileName + "\"");


            this.WriteLine("SpecificationRemark,,\"\"");

            this.WriteLine("SampleBins,,\"ALL\"");

            this.WriteLine("SampleStandard,,\"WEIMIN\"");

            this.WriteLine("SampleLevel,,\"STANDARD\"");

            this.WriteLine("TotalTested,,\"" + "\"");

            this.WriteLine("Samples,,\"" + "\"");

            this.WriteLine("LotNumber,,\"" + this.UISetting.LotNumber + "\"");            

            this.WriteLine("Operator,,\"" + this.UISetting.LoginID + "-" + this.UISetting.OperatorName + "\"");

            this.WriteLine("CodeNumber,,\"1\"");

            this.WriteLine("SerialNumber,,\"T140168\"");

            this.WriteLine("MaximumBin,,\"31\"");
                        

            this.WriteLine("ItemName,,\"" + this.ItemNameStr() + "\"");

            this.WriteLine("DataFormat,,\"" + this.DataFormatStr() + "\"");

            this.WriteLine("TestCondition,,\"" + this.TestConditionStr() + "\"");

            this.WriteLine("At,,\",,,,ALL,,,," + this.BinStr() + "\"");

            for (int i = 1; i < 30; i++)
            {
                this.WriteLine("BinAt" + i + ",,\",,,," + i + ",,,," + this.BinStr() + "\"");
            }

            this.WriteLine("Target,,\"" + this.targetStr() + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,\"");

            this.WriteLine("LowLimit,,\"" + LimitStr (false)+ "\"");

            this.WriteLine("HighLimit,,\"" + LimitStr(true) + "\"");

            this.WriteLine("");

            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string testTime = "TestTime,,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm") + "\"";

            replaceData.Add("TestTime,,\"" + "\"", testTime);

            string testCount = "TotalTested,,\"" + this._checkColRowKey.Count.ToString() + "\"";

            replaceData.Add("TotalTested,,\"" + "\"", testCount);

            string sampleCount = "Samples,,\"" + this._checkColRowKey.Count.ToString() + "\"";

            replaceData.Add("Samples,,\"" + "\"", sampleCount);

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
        }



        #region >>>private method
        private string ItemNameStr()
        {
            string itemNameStr = string.Empty;

            foreach (var data in ResultTitleInfo)
            {
                string key = data.Key;


                if (key != "TEST")
                {
                    foreach (var rData in AcquireData.OutputTestResult)
                    {
                        if (key == rData.KeyName)
                        {
                            itemNameStr += data.Value;

                            if (rData.Unit != "")
                            {
                                itemNameStr += "(" + rData.Unit + ")";
                            }

                            
                            break;
                        }
                    }
                }
                itemNameStr += ",";
            }
            

            if (itemNameStr.Length > 0 && itemNameStr[itemNameStr.Length - 1] == ',')
            {
                itemNameStr = itemNameStr.Remove(itemNameStr.Length - 1);
            }

            return itemNameStr;
        }

        private string DataFormatStr()
        {
            string dataFormatStr = string.Empty;

            foreach (var data in ResultTitleInfo)
            {
                string key = data.Key;

                if (key != "TEST")
                {

                    foreach (var rData in AcquireData.OutputTestResult)
                    {
                        if (key == rData.KeyName)
                        {
                            dataFormatStr += rData.Formate;
                            
                        }
                    }
                }
                dataFormatStr += ",";
            }

            if (dataFormatStr.Length > 0 && dataFormatStr[dataFormatStr.Length - 1] == ',')
            {
                dataFormatStr = dataFormatStr.Remove(dataFormatStr.Length - 1);
            }

            return dataFormatStr;
        }

        private string TestConditionStr()
        {
            string testConditionStr = string.Empty;


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
                            if (rData.KeyName == key)
                            {
                                switch(key)
                                {
                                    case "MVFMB_1":
                                    {
                                        testConditionStr += tData.ElecSetting[1].Name;
                                        testConditionStr += "(" + tData.ElecSetting[1].MsrtUnit + ")";
                                        isFound = true;
                                    }
                                    break;

                                    case "MVFMC_1":
                                    {
                                        testConditionStr += "IFP * T";
                                        isFound = true;
                                    }
                                    break;

                                    default:
                                    {
                                        if (tData.ElecSetting != null)
                                        {
                                            testConditionStr += tData.ElecSetting[0].Name;

                                            if (tData.ElecSetting[0].MsrtUnit != "")
                                            {
                                                testConditionStr += "(" + tData.ElecSetting[0].MsrtUnit + ")";
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

                if (key != "TEST")
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
                                    case "MVFMB_1":
                                        {
                                            binStr += tData.ElecSetting[1].ForceValue;
                                            isFound = true;
                                        }
                                        break;

                                    case "MVFMC_1":
                                        {
                                            binStr += "";
                                            isFound = true;
                                        }
                                        break;

                                    default:
                                        {
                                            if (tData.ElecSetting != null)
                                            {
                                                binStr += tData.ElecSetting[0].ForceValue;
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

            return ((double)day * 52.34158).ToString();
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

        #endregion
    }
}
