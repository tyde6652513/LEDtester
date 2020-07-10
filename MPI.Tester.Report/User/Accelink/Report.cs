using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Data;

using System.IO;

namespace MPI.Tester.Report.User.Accelink
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
            return this.RewriteReportByDefault();
        }
    }
}
