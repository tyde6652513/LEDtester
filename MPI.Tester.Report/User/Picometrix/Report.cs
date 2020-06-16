using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using System.Diagnostics;
using MPI.Tester.Data;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester;
using MPI.Tester.TestKernel;
using MPI.Tester.Data;

namespace MPI.Tester.Report.User.Picometrix
{
	public class Report : ReportBase
	{
        public Report(List<object> objs, bool isReStatistic): base(objs, isReStatistic)
		{
         
        }

        #region >>> Private Method <<<


        #endregion

        #region >>> Protected Override Method <<<

        protected override void SetResultTitle()
		{
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
        } 

		protected override EErrorCode WriteReportHeadByUser()
		{
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("FileName," + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt.ToUpper());

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

            this.WriteLine("Operator,\"" + this.UISetting.OperatorName + "\"");

            this.WriteLine("Samples,\"\"");

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Test Item Info
            ////////////////////////////////////////////
            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Gain,Offset,RltUnit");

            Dictionary<string, GainOffsetData> gainOffsetData = new Dictionary<string, GainOffsetData>();

            List<int> lstViSweepPoint = new List<int>();

            if (this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null && this.Product.TestCondition.TestItemArray.Length > 0)
            {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                {
                    double gain = 1.0d;
                    double offset = 0.0d;

                    string strForceValue = string.Empty;

                    if (testItem is VISweepTestItem)
                    {
                        if (!testItem.IsEnable)
                        {
                            lstViSweepPoint.Add(-1);
                            continue;
                        }
                    
                        
                        strForceValue = string.Format("{0};{1};{2}", testItem.ElecSetting[0].SweepStart.ToString("0.0"),
                                                                     testItem.ElecSetting[0].SweepStep.ToString("0.00"),
                                                                     testItem.ElecSetting[0].SweepStop.ToString("0.0"));


                        string line = string.Empty;

                        line += testItem.Name;

                        line += "," + strForceValue;

                        line += "," + testItem.ElecSetting[0].ForceUnit;

                        line += "," + testItem.ElecSetting[0].ForceTime.ToString();

                        line += "," + testItem.ElecSetting[0].MsrtProtection.ToString();

                        line += "," + testItem.ElecSetting[0].MsrtUnit;

                        line += ",0,0,1,0,A";

                        this.WriteLine(line);
                        lstViSweepPoint.Add((int)testItem.ElecSetting[0].SweepRiseCount);
                    }
                    else
                    {
                        if (!testItem.IsEnable)
                        {
                            continue;
                        }

                        strForceValue = testItem.ElecSetting[0].ForceValue.ToString();

                        if (testItem is VRTestItem)
                        {
                            VRTestItem vr = (testItem as VRTestItem);

                            if (vr.IsUseVzAsForceValue)
                            {
                                strForceValue = "F";
                                gain = vr.Factor;
                                offset = vr.Offset * -1.0d;
                            }
                        }

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
                            if (!msrtItem.IsEnable || !msrtItem.IsVision)
                            {
                                continue;
                            }

                            string line = string.Empty;

                            line += msrtItem.Name;

                            line += "," + strForceValue;

                            line += "," + testItem.ElecSetting[0].ForceUnit;

                            line += "," + testItem.ElecSetting[0].ForceTime.ToString();

                            line += "," + testItem.ElecSetting[0].MsrtProtection.ToString();

                            line += "," + testItem.ElecSetting[0].MsrtUnit;

                            line += "," + msrtItem.MinLimitValue;

                            line += "," + msrtItem.MaxLimitValue;

                            line += "," + gain.ToString();

                            line += "," + offset.ToString();

                            line += "," + testItem.ElecSetting[0].MsrtUnit;

                            this.WriteLine(line);
                        }
                    }
                }
            }

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

            foreach (var item in gainOffsetData)
            {
                lineItem += "," + item.Value.Name;

                lineType += "," + item.Value.Type;

                lineSquare += "," + item.Value.Square;

                lineGain += "," + item.Value.Gain;

                lineOffset += "," + item.Value.Offset;

                lineGain2 += "," + item.Value.Gain2;

                lineOffset2 += "," + item.Value.Offset2;
            }

            this.WriteLine(lineItem);

            this.WriteLine(lineType);

            this.WriteLine(lineSquare);

            this.WriteLine(lineGain);

            this.WriteLine(lineOffset);

            this.WriteLine(lineGain2);

            this.WriteLine(lineOffset2);

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////
            string titleStr = this.ResultTitleInfo.TitleStr;

            for (int itemIdx = 0; itemIdx < lstViSweepPoint.Count; itemIdx++)
            {
                int point = lstViSweepPoint[itemIdx];
              
                if (point > 0)
                {
                    for (int i = 0; i < point; i++)
                    {
                        titleStr += string.Format(",V{0}_{1}", (itemIdx + 1).ToString(), (i + 1).ToString());
                        titleStr += string.Format(",I{0}_{1}", (itemIdx + 1).ToString(), (i + 1).ToString());
                    }
                }
            }

            this.WriteLine(titleStr);

            return EErrorCode.NONE;
		}

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
        {
            string line = string.Empty;
            int column = 0;
          
            foreach (var resultItem in this.ResultTitleInfo)
            {
                if (data.ContainsKey(resultItem.Key))
                {
                    string format = string.Empty;

                    if (this.ResultData.ContainsKey(resultItem.Key))
                    {
                        format = this.ResultData[resultItem.Key].Formate;
                    }

                    line += data[resultItem.Key].ToString(format);
                }

                column++;

                if (column != this.ResultTitleInfo.ResultCount)
                {
                    line += ",";
                }
            }

            if (this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null && this.Product.TestCondition.TestItemArray.Length > 0)
            {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                {
                    if (testItem is VISweepTestItem)
                    {
                        if (!testItem.IsEnable)
                        {
                            continue;
                        }

                        string keyName = testItem.KeyName;

                        ElecSweepData viData = this.AcquireData.ElecSweepDataSet[0, keyName];

                        if (viData != null)
                        {
                            for (int i = 0; i < viData.SweepData.Length; i++)
                            {
                                line += "," + viData.ApplyData[i].ToString();
                                line += "," + viData.SweepData[i].ToString();
                            }
                        }
                    }
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

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
		}

		#endregion
	}
}
