using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using MPI.Tester.Data;

using MPI.Tester.TestServer;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Report.User.MPI_LCR
{
	class Report : ReportBase
	{
        //Dictionary<string, string> _sweepItemDic;

        ////private List<string> _colRow;

        //private List<string> _itemKeys;

        //private int _maxSweepCount = 0;

		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
		}

        protected override void SetResultTitle()
		{

            this.SetResultTitleByDefault();

            this.SetResultSweepTitle01ByDefault();
            //_maxSweepCount01 = 0;

            //GetMaxLcrSwCount();

            //ResetLcrSwResultDic01();

		}

        protected override EErrorCode WriteReportHeadByUser()
        {
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("FileName,\"" + Path.Combine(this.TestResultFileNameWithoutExt(), this.UISetting.TestResultFileExt) + "\"");

            this.WriteLine("UserID,\"" + this.UISetting.UserID.ToString() + "_" + this.UISetting.FormatName + "\"");

            this.WriteLine("TestTime,\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

            this.WriteLine("EndTime,\"" + "\"");

            this.WriteLine("TesterModel,\"" + this.MachineInfo.TesterModel + "/" + this.MachineInfo.TesterSN + "\"");

            this.WriteLine("MachineName,\"" + this.UISetting.MachineName + "\"");

            this.WriteLine("LotNumber,\"" + this.UISetting.LotNumber + "\"");

            this.WriteLine("Substrate,\"" + this.UISetting.Substrate + "\"");

            this.WriteLine("TaskFile,\"" + this.UISetting.TaskSheetFileName + "\"");

            this.WriteLine("Recipe,\"" + this.UISetting.ProductFileName + "\"");

            this.WriteLine("ConditionFileName,\"" + this.UISetting.ConditionFileName + "\"");

            this.WriteLine("BinFileName,\"" + this.UISetting.BinDataFileName + "\"");

            this.WriteLine("Filter Wheel,\"" + this.Product.ProductFilterWheelPos.ToString() + "\"");

            this.WriteLine("LOPSaveItem,\"" + this.Product.LOPSaveItem.ToString() + "\"");

            this.WriteLine("Operator,\"" + this.UISetting.OperatorName + "\"");

            this.WriteLine("Samples,\"\"");

            this.WriteLine("Unit,\"9999\"");

            this.WriteLine("");

            //Write LCR Cali Info
            List<string> caliList = GetLcrCaliList();
            foreach (string str in caliList)
            {
                this.WriteLine(str);
            }

            ////////////////////////////////////////////
            //Write Test Item Info
            ////////////////////////////////////////////
            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

            Dictionary<string, GainOffsetData> gainOffsetData = new Dictionary<string, GainOffsetData>();

            //Dictionary<string, double> maxData = new Dictionary<string, double>();

            //Dictionary<string, double> minData = new Dictionary<string, double>();

            #region

            //List<string> titleItemList = this.ResultTitleInfo.TitleStr.Split(new char[] { ',' }).ToList();

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

                        if (!data.IsEnable || !data.IsVision || testItem.MsrtResult[GetMsrtIndex(testItem, data.KeyName)].IsEnable == false)
                        {
                            continue;
                        }

                        gainOffsetData.Add(data.KeyName, data);

                        //maxData.Add(data.KeyName, -999999);

                        //minData.Add(data.KeyName, 999999);
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

                        line += "," + testItem.ElecSetting[0].ForceValue.ToString();

                        line += "," + testItem.ElecSetting[0].ForceUnit;

                        line += "," + testItem.ElecSetting[0].ForceTime.ToString();

                        line += "," + testItem.ElecSetting[0].MsrtProtection.ToString();

                        //line += "," + testItem.ElecSetting[0].MsrtUnit;
                        line += "," + FindMsrtUnit(msrtItem.KeyName);

                        line += "," + msrtItem.MinLimitValue;

                        line += "," + msrtItem.MaxLimitValue;

                        line += "," + sqr.ToString();

                        line += "," + gain.ToString();

                        line += "," + offset.ToString();

                        //line += "," + testItem.ElecSetting[0].MsrtUnit;
                        line += "," + FindMsrtUnit(msrtItem.KeyName);

                        this.WriteLine(line);
                    }
                }
            }
            #endregion

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Gain Offset Info
            ////////////////////////////////////////////
            string lineItem = "Result Item";

            string lineType = "Type";

            string lineSquare = "Square";

            string lineGain = "Gain";

            string lineOffset = "Offset";

            string lineGain2 = "Gain2";

            string lineOffset2 = "Offset2";

            string lineMin = "Min";

            string lineMax = "Max";

            foreach (var item in gainOffsetData)
            {
                lineItem += "," + item.Value.Name;

                lineType += "," + item.Value.Type;

                lineSquare += "," + item.Value.Square;

                lineGain += "," + item.Value.Gain;

                lineOffset += "," + item.Value.Offset;

                lineGain2 += "," + item.Value.Gain2;

                lineOffset2 += "," + item.Value.Offset2;

                TestResultData result;
                if (GetResultData(item.Value.KeyName, out  result))
                {
                    lineMin += "," + result.MinLimitValue;

                    lineMax += "," + result.MaxLimitValue;

                }
                else
                {
                    lineMin += ",";

                    lineMax += ",";
                }

            }

            this.WriteLine(lineItem);

            this.WriteLine(lineType);

            this.WriteLine(lineSquare);

            this.WriteLine(lineGain);

            this.WriteLine(lineOffset);

            this.WriteLine(lineGain2);

            this.WriteLine(lineOffset2);

            this.WriteLine(lineMin);

            this.WriteLine(lineMax);

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////
            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
        {
            int binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

            int binGrade = 0;

            int binNumber = 0;

            string binCode = string.Empty;

            GetBinNum(binSN, out  binCode, out  binNumber, out  binGrade);

            string line = string.Empty;

            int index = 0;

            foreach (var item in this.ResultTitleInfo)
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

                if (index != this.ResultTitleInfo.ResultCount)
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

            string endTime = "EndTime,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm") + "\"";

            replaceData.Add("EndTime,\"" + "\"", endTime);

            string testCount = "Samples,\"" + this.TotalTestCount.ToString() + "\"";

            replaceData.Add("Samples,\"" + "\"", testCount);


            //string maxVal = "Max" + getMaxValList();

            //replaceData.Add("Max", maxVal);

            //string minVal = "Min" + getMinValList();

            //replaceData.Add("Min", minVal);

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
        }




        private Dictionary<string, string> _sweepItemDic01;

        private string SweepTitle1 = "";


        private int _maxSweepCount01 = 0;

        private void ResetLcrSwResultDic01()
        {
            this._sweepItemDic01 = new Dictionary<string, string>();

            Dictionary<string, string> MD = UISetting.UserDefinedData.MsrtDisplayItemDic;
            this._sweepItemDic01.Add("TEST", MD["TEST"]);
            this._sweepItemDic01.Add("COL", MD["COL"]);
            this._sweepItemDic01.Add("ROW", MD["ROW"]);
            this._sweepItemDic01.Add("BIN", MD["BIN"]);
            this._sweepItemDic01.Add("POLAR", MD["POLAR"]);


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

                    foreach (var msrtItem in testItem.MsrtResult)
                    {
                        if (!msrtItem.IsEnable || !msrtItem.IsVision)
                        {
                            continue;
                        }

                        if (IsLCRListItem(msrtItem.KeyName))
                        {
                            this._sweepItemDic01.Add(msrtItem.KeyName, msrtItem.Name);
                        }
                      
                    }
                }
            }
        }

        private void SetResultSweepTitle01ByDefault()
        {
            SweepTitle1 = "";

            GetMaxLcrSwCount();

            ResetLcrSwResultDic01();

            int counter = 0;

            string titleStr = string.Empty;//"TEST,PosX,PosY,BIN,POLAR";

            foreach (KeyValuePair<string, string> knPair in _sweepItemDic01)
            {
                titleStr += knPair.Value;

                ++counter;

                if (counter != _sweepItemDic01.Count)
                {
                    titleStr += ",";
                }
            }

            SweepTitle1 = titleStr.TrimEnd(new char[] { ',' });

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

            string shortCali = "SHORT:," + (cData.EnableShort ? "ON" : "OFF");

            if (cData.EnableShort)
            {
                CaliRowData crd = cData.LoadingList[dataNum].ShortRaw;
                shortCali +=
                    "," + crd.CaliLCRTestType.ToString() +
                    "," + crd.ValA.ToString("E3") + " " + crd.UnitA +
                    "," + crd.ValB.ToString("E3") + " " + crd.UnitB ;//+
                    //",," +
                    //"," + crd.MeterRowValA.ToString("E3") + " " + crd.MeterUnitA +
                    //"," + crd.MeterRowValB.ToString("E3") + " " + crd.MeterUnitB;
            }

            outStrList.Add(shortCali);

            string loadCali = "Load:," + (cData.EnableLoad ? "ON" : "OFF");

            if (cData.EnableLoad)
            {
                CaliRowData crd = cData.LoadingList[dataNum].LoadRaw;
                loadCali +=
                    "," + crd.CaliLCRTestType.ToString() +
                    "," + crd.ValA.ToString("E3") + " " + crd.UnitA +
                    "," + crd.ValB.ToString("E3") + " " + crd.UnitB +
                    "," + cData.LoadingList[dataNum].RefA.ToString("E3") + " " + crd.UnitA +
                    "," + cData.LoadingList[dataNum].RefB.ToString("E3") + " " + crd.UnitB +
                    "," + crd.MeterRowValA.ToString("E3") + " " + crd.MeterUnitA +
                    "," + crd.MeterRowValB.ToString("E3") + " " + crd.MeterUnitB;
            }


            outStrList.Add(loadCali);

            return outStrList;
            //outStrList 

        }
        private void GetMaxLcrSwCount()
        {
            _maxSweepCount01 = 0;

            if (this.AcquireData.LCRDataSet != null)
            {
                foreach (LCRData LcrList in this.AcquireData.LCRDataSet)
                {
                    if (!LcrList.IsEnable)
                    {
                        continue;
                    }

                    foreach (BaseResultData LcrData in LcrList)
                    {
                        if (this._maxSweepCount01 < LcrData.DataArray.Length)
                        {
                            this._maxSweepCount01 = LcrData.DataArray.Length;
                        }
                    }
                }
            }

        }

	}
}
