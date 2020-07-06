using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;

using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;

namespace MPI.Tester.Report.User.DOWA
{
    partial class Report
    {
        private EErrorCode WriteReportHeadByUser_FormatA()
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

            this.WriteLine("");

            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,RltUnit");

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

                    foreach (var msrtItem in testItem.MsrtResult)
                    {
                        string line = string.Empty;
                        if (!msrtItem.IsEnable || !msrtItem.IsVision || testItem.ElecSetting == null)
                        {
                            if (testItem.Type == ETestType.CALC)
                            {
                                line += msrtItem.Name;

                                line += this.SpiltChar.ToString() + this.SpiltChar.ToString() + this.SpiltChar.ToString() + this.SpiltChar.ToString() + this.SpiltChar.ToString();

                                line += this.SpiltChar.ToString() + msrtItem.MinLimitValue;

                                line += this.SpiltChar.ToString() + msrtItem.MaxLimitValue;

                                line += this.SpiltChar.ToString() + msrtItem.Unit;

                                this.WriteLine(line);
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

                        this.WriteLine(line);
                    }
                }
            }

            this.WriteLine("");

            this.WriteLine(this._resultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        private EErrorCode PushDataByUser_FormatA(Dictionary<string, double> data)
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
                if (item.Key == "BIN_CODE")
                {
                    line += binCode;
                }
                else if (item.Key == "BIN_NUMBER")
                {
                    line += binNumber.ToString();
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
                    line += this.SpiltChar.ToString(); ;
                }
            }

            this.WriteLine(line);

            return EErrorCode.NONE;
        }

        private EErrorCode RewriteReportByUser_FormatA()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string endTime = "EndTime" + this.SpiltChar.ToString() + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm");

            replaceData.Add("EndTime", endTime);

            string testCount = "Samples" + this.SpiltChar.ToString() + this._checkColRowKey.Count.ToString();

            replaceData.Add("Samples", testCount);

            if (File.Exists(this.FileFullNameTmp))
            {
                base.ReplaceReportData(replaceData, this.FileFullNameTmp, this.FileFullNameRep, false);
            }

            return EErrorCode.NONE;
        }

    }
}
