using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;



namespace MPI.Tester.Report.User.HpoLED
{
    class Report : ReportBase
    {
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
            //
            this.WriteLine("FileName" + this.SpiltChar.ToString() + "\"" + Path.Combine(this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt) + "\"");

            this.WriteLine("TestTime" + this.SpiltChar.ToString() + "\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\"");

            this.WriteLine("TesterModel" + this.SpiltChar.ToString() + "\"" + this.MachineInfo.TesterModel + "/" + this.MachineInfo.TesterSN + "\"");

            this.WriteLine("TesterNumber" + this.SpiltChar.ToString() + "\"" + this.UISetting.MachineName + "\"");

            this.WriteLine("DeviceNumber" + this.SpiltChar.ToString() + "\"" + this.UISetting.WeiminUIData.DeviceNumber + "\"");

            this.WriteLine("Specification" + this.SpiltChar.ToString() + "\"" + this.UISetting.WeiminUIData.Specification + "\"");

            this.WriteLine("SpecRemark" + this.SpiltChar.ToString() + "\"" + this.UISetting.WeiminUIData.SpecificationRemark + "\"");

            this.WriteLine("LotNumber" + this.SpiltChar.ToString() + "\"" + this.UISetting.LotNumber + "\"");

            this.WriteLine("Operator" + this.SpiltChar.ToString() + "\"" + this.UISetting.OperatorName + "\"");

            this.WriteLine("EquipmentNo" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("Rework" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("K Value" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("LOP Ratio" + this.SpiltChar.ToString() + "\"" + "0, 0, 0, 0, 0, ToolFactor, 1, 0, 0, 0, 0, 0, 0" + "\"");

            this.WriteLine("Dark Value" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("Epi" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("Sqin" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("Type" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("MFNO" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("Remark1" + this.SpiltChar.ToString() + "\"" + this.UISetting.TestResultPath01 + "\"");

            this.WriteLine("BinGrade" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("Sort" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("Bin" + this.SpiltChar.ToString() + "\"" + "Unknown" + "\"");

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Test Item Info
            ////////////////////////////////////////////
            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

            Dictionary<string, GainOffsetData> gainOffsetData = new Dictionary<string, GainOffsetData>();

            bool useLcr  =false;
            #region
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

                        if (testItem.Type == ETestType.LCR)
                        {
                            useLcr = true;
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

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceValue.ToString();

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceUnit;

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceTime.ToString();

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtProtection.ToString();

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtUnit;

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

            if (useLcr)
            {
                List<string> strList = GetLcrCaliList();
                foreach (string str in strList)
                {
                    WriteLine(str);
                }
            }
            #endregion

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Gain Offset Info
            ////////////////////////////////////////////
            #region
            string lineItem = "Result Item";

            string lineType = "Type";

            string lineSquare = "Square";

            string lineGain = "Gain";

            string lineOffset = "Offset";

            string lineGain2 = "Gain2";

            string lineOffset2 = "Offset2";

            foreach (var item in gainOffsetData)
            {
                lineItem += this.SpiltChar.ToString() + item.Value.Name;

                lineType += this.SpiltChar.ToString() + item.Value.Type;

                lineSquare += this.SpiltChar.ToString() + item.Value.Square;

                lineGain += this.SpiltChar.ToString() + item.Value.Gain;

                lineOffset += this.SpiltChar.ToString() + item.Value.Offset;

                lineGain2 += this.SpiltChar.ToString() + item.Value.Gain2;

                lineOffset2 += this.SpiltChar.ToString() + item.Value.Offset2;
            }

            this.WriteLine(lineItem);

            this.WriteLine(lineType);

            this.WriteLine(lineSquare);

            this.WriteLine(lineGain);

            this.WriteLine(lineOffset);

            this.WriteLine(lineGain2);

            this.WriteLine(lineOffset2);
            #endregion
            this.WriteLine("WDOffset1" + this.SpiltChar.ToString() + 
                 "WDOffset2" + this.SpiltChar.ToString() + 
                 "WDOffset3" + this.SpiltChar.ToString() + 
                 "WDOffset4" + this.SpiltChar.ToString() + 
                 "MLOP1" + this.SpiltChar.ToString() + 
                 "MLOP2" + this.SpiltChar.ToString() + 
                 "MLOP3" + this.SpiltChar.ToString() + 
                 "MLOP4" + this.SpiltChar.ToString() + 
                 "MWLD1" + this.SpiltChar.ToString() + 
                 "MWLD2" + this.SpiltChar.ToString() + 
                 "MWLD3" + this.SpiltChar.ToString() + "\"" +
                 "MWLD4" + "\"");
            this.WriteLine("0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() + 
                 "0" + this.SpiltChar.ToString() +
                 "0" + this.SpiltChar.ToString() + "\"" +
                 "0" +  "\"");
            this.WriteLine("");
            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////
            this.WriteLine(this._resultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser()
        {
            return this.RewriteReportByDefault();
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
                    string format = "HH:mm:ss.fff";

                    if (this.UISetting.UserDefinedData[item.Key] != null)
                    {
                        format = this.UISetting.UserDefinedData[item.Key].Formate;
                    }

                    long tick = (long)data[item.Key];
                    DateTime dt = new DateTime(tick);
                    line += dt.ToString(format);
                }
                //else if (item.Key == "TimeSpan")
                //{
                //    string tFormat = "";
                //    if (this.UISetting.UserDefinedData["TimeSpan"] != null)
                //    {
                //        tFormat = UISetting.UserDefinedData["TimeSpan"].Formate;
                //    }
                //    line += AcquireData.ChipInfo.TimeSpan.Milliseconds.ToString();
                //}
                //else if (item.Key == "TESTENDTIME")
                //{
                //    string tFormat = "";
                //    if (this.UISetting.UserDefinedData["TESTENDTIME"] != null)
                //    {
                //        tFormat = UISetting.UserDefinedData["TESTENDTIME"].Formate;
                //    }
                //    line += AcquireData.ChipInfo.EndTime.ToString();
                //}
                //else if (item.Key == "TESTSTARTTIME")
                //{
                //    line += AcquireData.ChipInfo.StartTime.ToString("HH:mm:ss.fff");
                //}



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

            this.WriteLine(line);

            return EErrorCode.NONE;
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


    }
}
