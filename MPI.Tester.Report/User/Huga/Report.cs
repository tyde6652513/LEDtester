using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.Report.User.Huga
{
	class Report : ReportBase
	{
        private int _sn;
        private double _area;

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
            this._sn = 0;

            this._area = 1;

            string Vf0 = "Vf0=,Vf0 DT=,";

            string Vf1 = "Vf1=,Vf1 DT=,";

            string Vf3 = "Vf3=,Vf3 DT=,";

            string Vf4 = "Vf4=,Vf4 DT=,";

            string IR = "IR0=,IR0 DT=,";

            string VR = "VR0=,VR0 DT=,";

            string If2 = "If2=,If2=,Step=,Delay Time=,Pause Time=,WL Mode=,WL DT=";

			if(this.Product.TestCondition.TestItemArray != null)
			{
				foreach (var item in this.Product.TestCondition.TestItemArray)
				{
                    if (item.ElecSetting == null || item.ElecSetting.Length == 0 || !item.IsEnable)
					{
						continue;
					}

                    if (item.KeyName == "IF_1")
                    {
                        Vf0 = "Vf0=" + Math.Abs(item.ElecSetting[0].ForceValue) + item.ElecSetting[0].ForceUnit + ",Vf0 DT=" + item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeUnit + ",";
                    }
                    else if(item.KeyName == "IF_2")
                    {
                        Vf1 = "Vf1=" + Math.Abs(item.ElecSetting[0].ForceValue) + item.ElecSetting[0].ForceUnit + ",Vf1 DT=" + item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeUnit + ",";
                    }
                    else if (item.KeyName == "IF_3")
                    {
                        Vf3 = "Vf3=" + Math.Abs(item.ElecSetting[0].ForceValue) + item.ElecSetting[0].ForceUnit + ",Vf3 DT=" + item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeUnit + ",";
                    }
                    else if (item.KeyName == "IF_4")
                    {
                        Vf4 = "Vf4=" + Math.Abs(item.ElecSetting[0].ForceValue) + item.ElecSetting[0].ForceUnit + ",Vf4 DT=" + item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeUnit + ",";
                    }
                    else if (item.KeyName == "IZ_1")
                    {
                        IR = "VR=" + Math.Abs(item.ElecSetting[0].ForceValue) + item.ElecSetting[0].ForceUnit + ",VR DT=" + item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeUnit + ",";
                    }
                    else if (item.KeyName == "VR_1")
                    {
                        VR = "IR=" + Math.Abs(item.ElecSetting[0].ForceValue) + item.ElecSetting[0].ForceUnit + ",IR DT=" + item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeUnit + ",";
                    }
                    else if (item.KeyName == "LIV_1")
                    {
                        LIVTestItem livTestItem = item as LIVTestItem;

                        If2 = livTestItem.Name + "=" + Math.Abs(livTestItem.LIVStartValue).ToString() + livTestItem.LIVForceUnit + ",";

                        If2 += livTestItem.Name + "=" + Math.Abs(livTestItem.LIVStopValue).ToString() + livTestItem.LIVForceUnit + ",";

                        If2 += "Step=" + livTestItem.LIVStepValue + livTestItem.LIVForceUnit + ",";

                        If2 += "Delay Time=" + livTestItem.LIVForceTime + livTestItem.LIVForceTimeUnit + ",";

                        If2 += "Pause Time=" + livTestItem.LIVTurnOffTime + livTestItem.LIVForceTimeUnit + ",";

                        If2 += "WL Mode=" + livTestItem.LIVSensingMode.ToString() + ",";

                        If2 += "WL DT=" + livTestItem.LIVForceTime + livTestItem.LIVForceTimeUnit;
                    }
				}
			}

            double.TryParse(this.UISetting.WeiminUIData.Remark01, out this._area);

            string testCondition = ",Area(um2),";

            testCondition += this.UISetting.WeiminUIData.Remark01 + ",";

            testCondition += Vf0 + Vf1 + Vf3 + Vf4 + IR + VR + If2;

			this.WriteLine("LotNo=" + this.UISetting.WaferNumber);

            this.WriteLine("StartTestDate=" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss"));

			this.WriteLine("SpecName=" + this.UISetting.TaskSheetFileName);

			this.WriteLine("Test Condition=" + testCondition);

			this.WriteLine("EquipmentID=" + this.UISetting.MachineName);

			this.WriteLine("OperatorID=" + this.UISetting.OperatorName);

            this.WriteLine(this.ResultTitleInfo.TitleStr);

            this.WriteLine(this.ResultTitleInfo.TitleStr);

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{

            MPIFile.CopyFile(this.FileFullNameTmp, this.FileFullNameRep);

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
		{
			

			int index = 0;

            LIVData livData1 = this.AcquireData.LIVData["LIV_1"];

            LIVData livData2 = this.AcquireData.LIVData["LIV_2"];

            int rowCount = 0;

            bool liv1Enable = livData1 != null && livData1["LIVSETCURRENT_1"] != null && livData1.IsEnable;

            bool liv2Enable = livData2 != null && livData2["LIVSETCURRENT_2"] != null && livData2.IsEnable;

            if(liv1Enable && liv2Enable)
            {
                if (livData1["LIVSETCURRENT_1"].Length > livData2["LIVSETCURRENT_2"].Length)
                {
                    rowCount = livData1["LIVSETCURRENT_1"].Length;
                }
                else
                {
                    rowCount = livData2["LIVSETCURRENT_2"].Length;
                }
            }
            else if (liv1Enable && !liv2Enable)
            {
                rowCount = livData1["LIVSETCURRENT_1"].Length;
            }
            else if (!liv1Enable && liv2Enable)
            {
                rowCount = livData2["LIVSETCURRENT_2"].Length;
            }
            else
            {
                rowCount = 1;
            }

            for (int i = 0; i < rowCount; i++)
            {
                this._sn++;

                string line = string.Empty;

                foreach (var item in this.ResultTitleInfo)
                {
                    string format = this.UISetting.UserDefinedData[item.Key].Formate;

                    if (item.Key == "SN")
                    {
                        line += this._sn.ToString(format);
                    }
                    else if (item.Key == "CD" && liv1Enable && livData1["LIVSETCURRENT_1"].Length > i)
                    {
                        line += ((double)livData1["LIVSETCURRENT_1"][i] * (double)10e4 / this._area).ToString(format);
                    }
                    else if (livData1[item.Key] != null && livData1[item.Key].Length > i)
                    {
                        line += livData1[item.Key][i].ToString(format);
                    }
                    else if (livData2[item.Key] != null && livData2[item.Key].Length > i)
                    {
                        line += livData2[item.Key][i].ToString(format);
                    }
                    else if (data.ContainsKey(item.Key) && 
                        (this.ResultData.ContainsKey(item.Key) || item.Key == "TEST" || item.Key == "COL" || item.Key == "ROW"))
                    {
                        line += data[item.Key].ToString(format);
                    }
                    else
                    {
                        line += 0;
                    }

                    index++;

                    if (index != this.ResultTitleInfo.ResultCount)
                    {
                        line += ",";
                    }
                }

                this.WriteLine(line);
            }

			return EErrorCode.NONE;
		}
	}
}
