using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;

namespace MPI.Tester.Report.User.GPI
{
	public class Report : ReportBase
	{
		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{

		}

		protected override void SetResultTitle()
		{
			if (this.UISetting.UserDefinedData.MsrtDisplayItemDic.Count == 0)
			{
				return;
			}

			Dictionary<string, string> msrtItem = new Dictionary<string, string>();

			foreach(var item in this.UISetting.UserDefinedData.ResultItemNameDic)
			{
				if (item.Key.Contains(EOptiMsrtType.LOP.ToString()) && this.Product.LOPSaveItem != ELOPSaveItem.mcd)
				{
					continue;
				}

				if (item.Key.Contains(EOptiMsrtType.WATT.ToString()) && this.Product.LOPSaveItem != ELOPSaveItem.watt)
				{
					continue;
				}
				
				msrtItem.Add(item.Key, item.Value);
			}

			this.ResultTitleInfo.Clear();

			this.ResultTitleInfo.SetResultData(msrtItem);

			//this.ResultTitleInfo.AddResultData("TEST", msrtItem["TEST"]);

			//this.ResultTitleInfo.AddResultData("MVF_1", msrtItem["MVF_1"]);

			//this.ResultTitleInfo.AddResultData("MVFLA_1", msrtItem["MVFLA_1"]);

			//this.ResultTitleInfo.AddResultData("MVZ_1", msrtItem["MVZ_1"]);

			//this.ResultTitleInfo.AddResultData("MIR_1", msrtItem["MIR_1"]);

			//this.ResultTitleInfo.AddResultData("Rs", "Rs");

			//if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			//{
			//    this.ResultTitleInfo.AddResultData("LOP_1", msrtItem["LOP_1"]);
			//}
			//else
			//{
			//    this.ResultTitleInfo.AddResultData("WATT_1", msrtItem["WATT_1"]);
			//}

			//this.ResultTitleInfo.AddResultData("WLD_1", msrtItem["WLD_1"]);

			//this.ResultTitleInfo.AddResultData("WLP_1", msrtItem["WLP_1"]);

			//this.ResultTitleInfo.AddResultData("HW_1", msrtItem["HW_1"]);

			//this.ResultTitleInfo.AddResultData("MVF_3", msrtItem["MVF_3"]);

			//if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			//{
			//    this.ResultTitleInfo.AddResultData("LOP_2", msrtItem["LOP_2"]);
			//}
			//else
			//{
			//    this.ResultTitleInfo.AddResultData("WATT_2", msrtItem["WATT_2"]);
			//}

			//this.ResultTitleInfo.AddResultData("WLD_2", msrtItem["WLD_2"]);

			//this.ResultTitleInfo.AddResultData("CIEx_1", msrtItem["CIEx_1"]);

			//this.ResultTitleInfo.AddResultData("CIEy_1", msrtItem["CIEy_1"]);

			//this.ResultTitleInfo.AddResultData("MTHYVD_1", msrtItem["MTHYVD_1"]);

			//this.ResultTitleInfo.AddResultData("MVF_4", msrtItem["MVF_4"]);

			//this.ResultTitleInfo.AddResultData("MVF_5", msrtItem["MVF_5"]);

			//this.ResultTitleInfo.AddResultData("INT_1", msrtItem["INT_1"]);

			//this.ResultTitleInfo.AddResultData("INT_2", msrtItem["INT_2"]);

			//this.ResultTitleInfo.AddResultData("MVFMD_1", msrtItem["MVFMD_1"]);

			//this.ResultTitleInfo.AddResultData("BIN", msrtItem["BIN"]);

			//this.ResultTitleInfo.AddResultData("DWDWP_1", msrtItem["DWDWP_1"]);

			//if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			//{
			//    this.ResultTitleInfo.AddResultData("LOP_3", msrtItem["LOP_3"]);
			//}
			//else
			//{
			//    this.ResultTitleInfo.AddResultData("WATT_3", msrtItem["WATT_3"]);
			//}

			//this.ResultTitleInfo.AddResultData("WLD_3", msrtItem["WLD_3"]);

			//this.ResultTitleInfo.AddResultData("COL", msrtItem["COL"]);

			//this.ResultTitleInfo.AddResultData("ROW", msrtItem["ROW"]);
		}

		protected override Data.EErrorCode WriteReportHeadByUser()
		{
			this.WriteLine("LotNo=" + this.UISetting.WaferNumber);

			this.WriteLine("Date=");

			this.WriteLine("Time=");

			this.WriteLine("EquipmentID=" + this.UISetting.MachineName);

			this.WriteLine("OperatorID=" + this.UISetting.OperatorName);

			this.WriteLine("CalibrationFile=" + this.UISetting.ImportCalibrateFileName + ".cal");

			this.WriteLine("EsdFile=" + this.UISetting.WeiminUIData.Remark03);

			this.WriteLine("MapSetupFile=" + this.UISetting.ProductFileName + ".mcg");

			this.WriteLine("TestCond,Ifin,If2,Ir,Vr,If3,Ifd,If4,If5,Iv-Gain,Wd-Offset,ESD-ON/OFF,ESD-Mode,Temperature");

			string TestCond = "At", IF_1 = "0.0", IF_2 = "0.0", IZ_1 = "0.0", VR_1 = "0.0", IF_3 = "0.0", THY_1 = "0.0", IF_4 = "0.0", IF_5 = "0.0", Iv_Gain = "1.0", Wd_Offset = "0.0", ESD_ON_OFF = "0", ESD_Mode = this.UISetting.WeiminUIData.Remark03, Temperature = "0";

			if (this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{ 
					if(testItem.KeyName == "IF_1")
					{
						IF_1 = testItem.ElecSetting[0].ForceValue.ToString("0.0");
					}
					else if (testItem.KeyName == "LOPWL_1")
					{
						IF_2 = testItem.ElecSetting[0].ForceValue.ToString("0.0");
					}
					else if (testItem.KeyName == "IZ_1")
					{
						IZ_1 = Math.Abs(testItem.ElecSetting[0].ForceValue).ToString("0.0");
					}
					else if (testItem.KeyName == "VR_1")
					{
						VR_1 = Math.Abs(testItem.ElecSetting[0].ForceValue).ToString("0.0");
					}
					else if (testItem.KeyName == "IF_2")
					{
						IF_3 = testItem.ElecSetting[0].ForceValue.ToString("0.0");
					}
					else if (testItem.KeyName == "THY_1")
					{
						THY_1 = testItem.ElecSetting[0].ForceValue.ToString("0.0");
					}
					else if (testItem.KeyName == "IF_3")
					{
						IF_4 = testItem.ElecSetting[0].ForceValue.ToString("0.0");
					}
					else if (testItem.KeyName == "IF_4")
					{
						IF_5 = testItem.ElecSetting[0].ForceValue.ToString("0.0");
					}
					else if (testItem.KeyName == "LOPWL_1")
					{
						if (this.Product.LOPSaveItem.ToString().Contains(ELOPSaveItem.mcd.ToString()))
						{
							Iv_Gain = testItem.GainOffsetSetting[(int)EOptiMsrtType.LOP].Gain.ToString();
						}
						else
						{
							Iv_Gain = testItem.GainOffsetSetting[(int)EOptiMsrtType.WATT].Gain.ToString();
						}

						if (Iv_Gain.Length == 1)
						{
							Iv_Gain += ".0";
						}

						Wd_Offset = testItem.GainOffsetSetting[(int)EOptiMsrtType.WLD].Offset.ToString();

						if (Wd_Offset.Length == 1)
						{
							Wd_Offset += ".0";
						}
					}
					else if (testItem.KeyName.Contains("ESD") && testItem.IsEnable)
					{
						ESD_ON_OFF = "1";
					}
				}
			}

			this.WriteLine(TestCond + "," + IF_1 + "," + IF_2 + "," + IZ_1 + "," + VR_1 + "," + IF_3 + "," + THY_1 + "," + IF_4 + "," + IF_5 + "," + Iv_Gain + "," + Wd_Offset + "," + ESD_ON_OFF + "," + ESD_Mode + "," + Temperature);

			this.WriteLine("");

			this.WriteLine(this.ResultTitleInfo.TitleStr);

			return EErrorCode.NONE;
		}

		protected override Data.EErrorCode RewriteReportByUser()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string date = "Date=" + this.TesterSetting.EndTestTime.ToString("yyyy/M/d");

			string time = "Time=" + this.TesterSetting.EndTestTime.ToString("H:m:s");

			replaceData.Add("Date=", date);

			replaceData.Add("Time=", time);

			this.ReplaceReport(replaceData);

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
		{
			string line = string.Empty;

			int index = 0;

			foreach (var item in this.ResultTitleInfo)
			{
				string format = string.Empty;

				if (data.ContainsKey(item.Key))
				{
					if (this.ResultData.ContainsKey(item.Key))
					{
						format = this.ResultData[item.Key].Formate;
					}

					line += data[item.Key].ToString(format);
				}
				else
				{ 
					if(this.UISetting.UserDefinedData[item.Key] != null)
					{
						format = this.UISetting.UserDefinedData[item.Key].Formate;
					}

					line += (0).ToString(format);
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

		protected override Data.EErrorCode WriteReportHeadByUser2()
		{
			this.WriteLine2("Date,");

			this.WriteLine2("LotNumber," + this.UISetting.WaferNumber);

			this.WriteLine2("EquipmentID," + this.UISetting.MachineName);

			this.WriteLine2("Parameter," + this.UISetting.ProductFileName + ".mcg");

			this.WriteLine2("ESD_Result," + this.UISetting.WeiminUIData.Remark03);

			this.WriteLine2("Temperature,0");

			this.WriteLine2("");

			//全部統計， Item名稱不能重複，否則在Replace時會發生問題
			this.WriteLine2(",Min.,Avg.,Max.,Std.");

			this.WriteLine2("Vfin(V),A");

			this.WriteLine2("Vf(V),A");

			this.WriteLine2("Vr(V),A");

			this.WriteLine2("Ir(uA)A,");

			this.WriteLine2("Rs(ohm),A");

			this.WriteLine2("Iv(mcd),A");

			this.WriteLine2("λd(nm),A");

			this.WriteLine2("λp(nm),A");

			this.WriteLine2("△λ(nm),A");

			this.WriteLine2("CIE-x,A");

			this.WriteLine2("CIE-y,A");

			this.WriteLine2("Vf3(V),A");

			this.WriteLine2("Iv3(mcd),A");

			this.WriteLine2("λd3(nm),A");

			this.WriteLine2("Vf4(V),A");

			this.WriteLine2("Vf5(V),A");

			this.WriteLine2("VfD(V),A");

			this.WriteLine2("Wd-Wp(nm),A");

			this.WriteLine2("Iv5(mcd),A");

			this.WriteLine2("λd5(nm),A");
			
			this.WriteLine2("");

			this.WriteLine2(",Good,Fail,Total");

			this.WriteLine2("Quantity,");

			this.WriteLine2("%,");

			this.WriteLine2("");

			//單獨統計
			this.WriteLine2(",Min.,Avg.,Max.,Std.,%");

			this.WriteLine2("Vfin(V),S");

			this.WriteLine2("Vf(V),S");

			this.WriteLine2("Vr(V),S");

			this.WriteLine2("Ir(uA),S");

			this.WriteLine2("Rs(ohm),S");

			this.WriteLine2("Iv(mcd),S");

			this.WriteLine2("λd(nm),S");

			this.WriteLine2("λp(nm),S");

			this.WriteLine2("△λ(nm),S");

			this.WriteLine2("CIE-x,S");

			this.WriteLine2("CIE-y,S");

			this.WriteLine2("Vf3(V),S");

			this.WriteLine2("Iv3(mcd),S");

			this.WriteLine2("λd3(nm),S");

			this.WriteLine2("Vf4(V),S");

			this.WriteLine2("Vf5(V),S");

			this.WriteLine2("VfD(V),S");

			this.WriteLine2("Wd-Wp(nm),S");

			this.WriteLine2("Iv5(mcd),S");

			this.WriteLine2("λd5(nm),S");

			this.WriteLine2("");

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser2()
		{
			//////////////////////////////////////////////////////////
			// 全部統計
			//////////////////////////////////////////////////////////
			
			// Vfin
			string Vfin_A_Min = ReportBase.StatisticData("MVF_1", EStatisticType.All01).Min.ToString("0.0000");

			string Vfin_A_Avg = ReportBase.StatisticData("MVF_1", EStatisticType.All01).Mean.ToString("0.0000");

			string Vfin_A_Max = ReportBase.StatisticData("MVF_1", EStatisticType.All01).Max.ToString("0.0000");

			string Vfin_A_Std = ReportBase.StatisticData("MVF_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// Vf
			string Vf_A_Min = ReportBase.StatisticData("MVF_2", EStatisticType.All01).Min.ToString("0.0000");

			string Vf_A_Avg = ReportBase.StatisticData("MVF_2", EStatisticType.All01).Mean.ToString("0.0000");

			string Vf_A_Max = ReportBase.StatisticData("MVF_2", EStatisticType.All01).Max.ToString("0.0000");

			string Vf_A_Std = ReportBase.StatisticData("MVF_2", EStatisticType.All01).STDEV.ToString("0.0000");

			// Vr
			string Vr_A_Min = ReportBase.StatisticData("MVZ_1", EStatisticType.All01).Min.ToString("0.0000");

			string Vr_A_Avg = ReportBase.StatisticData("MVZ_1", EStatisticType.All01).Mean.ToString("0.0000");

			string Vr_A_Max = ReportBase.StatisticData("MVZ_1", EStatisticType.All01).Max.ToString("0.0000");

			string Vr_A_Std = ReportBase.StatisticData("MVZ_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// Ir
			string Ir_A_Min = ReportBase.StatisticData("MIR_1", EStatisticType.All01).Min.ToString("0.0000");

			string Ir_A_Avg = ReportBase.StatisticData("MIR_1", EStatisticType.All01).Mean.ToString("0.0000");

			string Ir_A_Max = ReportBase.StatisticData("MIR_1", EStatisticType.All01).Max.ToString("0.0000");

			string Ir_A_Std = ReportBase.StatisticData("MIR_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// Rs
			string Rs_A_Min = "0.0000";

			string Rs_A_Avg = "0.0000";

			string Rs_A_Max = "0.0000";

			string Rs_A_Std = "0.0000";

			// Iv
			string Iv_A_Min = "0.0000";

			string Iv_A_Avg = "0.0000";

			string Iv_A_Max = "0.0000";

			string Iv_A_Std = "0.0000";

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				Iv_A_Min = ReportBase.StatisticData("LOP_1", EStatisticType.All01).Min.ToString("0.0000");

				Iv_A_Avg = ReportBase.StatisticData("LOP_1", EStatisticType.All01).Mean.ToString("0.0000");

				Iv_A_Max = ReportBase.StatisticData("LOP_1", EStatisticType.All01).Max.ToString("0.0000");

				Iv_A_Std = ReportBase.StatisticData("LOP_1", EStatisticType.All01).STDEV.ToString("0.0000");
			}
			else
			{
				Iv_A_Min = ReportBase.StatisticData("WATT_1", EStatisticType.All01).Min.ToString("0.0000");

				Iv_A_Avg = ReportBase.StatisticData("WATT_1", EStatisticType.All01).Mean.ToString("0.0000");

				Iv_A_Max = ReportBase.StatisticData("WATT_1", EStatisticType.All01).Max.ToString("0.0000");

				Iv_A_Std = ReportBase.StatisticData("WATT_1", EStatisticType.All01).STDEV.ToString("0.0000");
			}

			// λd
			string Wld_A_Min = ReportBase.StatisticData("WLD_1", EStatisticType.All01).Min.ToString("0.0000");

			string Wld_A_Avg = ReportBase.StatisticData("WLD_1", EStatisticType.All01).Mean.ToString("0.0000");

			string Wld_A_Max = ReportBase.StatisticData("WLD_1", EStatisticType.All01).Max.ToString("0.0000");

			string Wld_A_Std = ReportBase.StatisticData("WLD_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// λp
			string Wlp_A_Min = ReportBase.StatisticData("WLP_1", EStatisticType.All01).Min.ToString("0.0000");

			string Wlp_A_Avg = ReportBase.StatisticData("WLP_1", EStatisticType.All01).Mean.ToString("0.0000");

			string Wlp_A_Max = ReportBase.StatisticData("WLP_1", EStatisticType.All01).Max.ToString("0.0000");

			string Wlp_A_Std = ReportBase.StatisticData("WLP_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// △λ
			string Hw_A_Min = ReportBase.StatisticData("HW_1", EStatisticType.All01).Min.ToString("0.0000");

			string Hw_A_Avg = ReportBase.StatisticData("HW_1", EStatisticType.All01).Mean.ToString("0.0000");

			string Hw_A_Max = ReportBase.StatisticData("HW_1", EStatisticType.All01).Max.ToString("0.0000");

			string Hw_A_Std = ReportBase.StatisticData("HW_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// CIE-x
			string CIEx_A_Min = ReportBase.StatisticData("CIEx_1", EStatisticType.All01).Min.ToString("0.0000");

			string CIEx_A_Avg = ReportBase.StatisticData("CIEx_1", EStatisticType.All01).Mean.ToString("0.0000");

			string CIEx_A_Max = ReportBase.StatisticData("CIEx_1", EStatisticType.All01).Max.ToString("0.0000");

			string CIEx_A_Std = ReportBase.StatisticData("CIEx_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// CIE-y
			string CIEy_A_Min = ReportBase.StatisticData("CIEy_1", EStatisticType.All01).Min.ToString("0.0000");

			string CIEy_A_Avg = ReportBase.StatisticData("CIEy_1", EStatisticType.All01).Mean.ToString("0.0000");

			string CIEy_A_Max = ReportBase.StatisticData("CIEy_1", EStatisticType.All01).Max.ToString("0.0000");

			string CIEy_A_Std = ReportBase.StatisticData("CIEy_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// Vf3
			string Vf3_A_Min = ReportBase.StatisticData("MVF_3", EStatisticType.All01).Min.ToString("0.0000");

			string Vf3_A_Avg = ReportBase.StatisticData("MVF_3", EStatisticType.All01).Mean.ToString("0.0000");

			string Vf3_A_Max = ReportBase.StatisticData("MVF_3", EStatisticType.All01).Max.ToString("0.0000");

			string Vf3_A_Std = ReportBase.StatisticData("MVF_3", EStatisticType.All01).STDEV.ToString("0.0000");

			// Iv3
			string Iv3_A_Min = "0.0000";

			string Iv3_A_Avg = "0.0000";

			string Iv3_A_Max = "0.0000";

			string Iv3_A_Std = "0.0000";

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				Iv3_A_Min = ReportBase.StatisticData("LOP_2", EStatisticType.All01).Min.ToString("0.0000");

				Iv3_A_Avg = ReportBase.StatisticData("LOP_2", EStatisticType.All01).Mean.ToString("0.0000");

				Iv3_A_Max = ReportBase.StatisticData("LOP_2", EStatisticType.All01).Max.ToString("0.0000");

				Iv3_A_Std = ReportBase.StatisticData("LOP_2", EStatisticType.All01).STDEV.ToString("0.0000");
			}
			else
			{
				Iv3_A_Min = ReportBase.StatisticData("WATT_2", EStatisticType.All01).Min.ToString("0.0000");

				Iv3_A_Avg = ReportBase.StatisticData("WATT_2", EStatisticType.All01).Mean.ToString("0.0000");

				Iv3_A_Max = ReportBase.StatisticData("WATT_2", EStatisticType.All01).Max.ToString("0.0000");

				Iv3_A_Std = ReportBase.StatisticData("WATT_2", EStatisticType.All01).STDEV.ToString("0.0000");
			}

			// λd3
			string Wld3_A_Min = ReportBase.StatisticData("WLD_2", EStatisticType.All01).Min.ToString("0.0000");

			string Wld3_A_Avg = ReportBase.StatisticData("WLD_2", EStatisticType.All01).Mean.ToString("0.0000");

			string Wld3_A_Max = ReportBase.StatisticData("WLD_2", EStatisticType.All01).Max.ToString("0.0000");

			string Wld3_A_Std = ReportBase.StatisticData("WLD_2", EStatisticType.All01).STDEV.ToString("0.0000");

			// Vf4
			string Vf4_A_Min = ReportBase.StatisticData("MVF_4", EStatisticType.All01).Min.ToString("0.0000");

			string Vf4_A_Avg = ReportBase.StatisticData("MVF_4", EStatisticType.All01).Mean.ToString("0.0000");

			string Vf4_A_Max = ReportBase.StatisticData("MVF_4", EStatisticType.All01).Max.ToString("0.0000");

			string Vf4_A_Std = ReportBase.StatisticData("MVF_4", EStatisticType.All01).STDEV.ToString("0.0000");

			// Vf5
			string Vf5_A_Min = ReportBase.StatisticData("MVF_5", EStatisticType.All01).Min.ToString("0.0000");

			string Vf5_A_Avg = ReportBase.StatisticData("MVF_5", EStatisticType.All01).Mean.ToString("0.0000");

			string Vf5_A_Max = ReportBase.StatisticData("MVF_5", EStatisticType.All01).Max.ToString("0.0000");

			string Vf5_A_Std = ReportBase.StatisticData("MVF_5", EStatisticType.All01).STDEV.ToString("0.0000");

			// Vfd
			string Vfd_A_Min = ReportBase.StatisticData("MTHYVD_1", EStatisticType.All01).Min.ToString("0.0000");

			string Vfd_A_Avg = ReportBase.StatisticData("MTHYVD_1", EStatisticType.All01).Mean.ToString("0.0000");

			string Vfd_A_Max = ReportBase.StatisticData("MTHYVD_1", EStatisticType.All01).Max.ToString("0.0000");

			string Vfd_A_Std = ReportBase.StatisticData("MTHYVD_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// Wd-Wp
			string Wdd_A_Min = ReportBase.StatisticData("DWDWP_1", EStatisticType.All01).Min.ToString("0.0000");

			string Wdd_A_Avg = ReportBase.StatisticData("DWDWP_1", EStatisticType.All01).Mean.ToString("0.0000");

			string Wdd_A_Max = ReportBase.StatisticData("DWDWP_1", EStatisticType.All01).Max.ToString("0.0000");

			string Wdd_A_Std = ReportBase.StatisticData("DWDWP_1", EStatisticType.All01).STDEV.ToString("0.0000");

			// Iv5
			string Iv5_A_Min = "0.0000";

			string Iv5_A_Avg = "0.0000";

			string Iv5_A_Max = "0.0000";

			string Iv5_A_Std = "0.0000";

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				Iv5_A_Min = ReportBase.StatisticData("LOP_3", EStatisticType.All01).Min.ToString("0.0000");

				Iv5_A_Avg = ReportBase.StatisticData("LOP_3", EStatisticType.All01).Mean.ToString("0.0000");

				Iv5_A_Max = ReportBase.StatisticData("LOP_3", EStatisticType.All01).Max.ToString("0.0000");

				Iv5_A_Std = ReportBase.StatisticData("LOP_3", EStatisticType.All01).STDEV.ToString("0.0000");
			}
			else
			{
				Iv5_A_Min = ReportBase.StatisticData("WATT_3", EStatisticType.All01).Min.ToString("0.0000");

				Iv5_A_Avg = ReportBase.StatisticData("WATT_3", EStatisticType.All01).Mean.ToString("0.0000");

				Iv5_A_Max = ReportBase.StatisticData("WATT_3", EStatisticType.All01).Max.ToString("0.0000");

				Iv5_A_Std = ReportBase.StatisticData("WATT_3", EStatisticType.All01).STDEV.ToString("0.0000");
			}

			// λd5
			string Wld5_A_Min = ReportBase.StatisticData("WLD_3", EStatisticType.All01).Min.ToString("0.0000");

			string Wld5_A_Avg = ReportBase.StatisticData("WLD_3", EStatisticType.All01).Mean.ToString("0.0000");

			string Wld5_A_Max = ReportBase.StatisticData("WLD_3", EStatisticType.All01).Max.ToString("0.0000");

			string Wld5_A_Std = ReportBase.StatisticData("WLD_3", EStatisticType.All01).STDEV.ToString("0.0000");

			//////////////////////////////////////////////////////////
			// 良率統計
			//////////////////////////////////////////////////////////

			//Good
			string Good = this.AcquireData.ChipInfo.GoodDieCount.ToString("0.0000");

			//Fail
			string Fail = this.AcquireData.ChipInfo.FailDieCount.ToString("0.0000");

			//Total
			string Total = this.AcquireData.ChipInfo.TestCount.ToString("0.0000");

			//Good
			string GoodRate = this.AcquireData.ChipInfo.GoodRate.ToString("0.0000");

			//Fail
			string FailRate = (100.0d - this.AcquireData.ChipInfo.GoodRate).ToString("0.0000");

			//Total
			string TotalRate = "100.0000";


			//////////////////////////////////////////////////////////
			// 單獨統計
			//////////////////////////////////////////////////////////

			// Vfin
			string Vfin_S_Min = ReportBase.StatisticData("MVF_1", EStatisticType.Single01).Min.ToString("0.0000");

			string Vfin_S_Avg = ReportBase.StatisticData("MVF_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string Vfin_S_Max = ReportBase.StatisticData("MVF_1", EStatisticType.Single01).Max.ToString("0.0000");

			string Vfin_S_Std = ReportBase.StatisticData("MVF_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Vfin_S_Rate = ReportBase.GoodRateS01("MVF_1").ToString("0.0000");

			// Vf
			string Vf_S_Min = ReportBase.StatisticData("MVF_2", EStatisticType.Single01).Min.ToString("0.0000");

			string Vf_S_Avg = ReportBase.StatisticData("MVF_2", EStatisticType.Single01).Mean.ToString("0.0000");

			string Vf_S_Max = ReportBase.StatisticData("MVF_2", EStatisticType.Single01).Max.ToString("0.0000");

			string Vf_S_Std = ReportBase.StatisticData("MVF_2", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Vf_S_Rate = ReportBase.GoodRateS01("MVF_2").ToString("0.0000");

			// Vr
			string Vr_S_Min = ReportBase.StatisticData("MVZ_1", EStatisticType.Single01).Min.ToString("0.0000");

			string Vr_S_Avg = ReportBase.StatisticData("MVZ_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string Vr_S_Max = ReportBase.StatisticData("MVZ_1", EStatisticType.Single01).Max.ToString("0.0000");

			string Vr_S_Std = ReportBase.StatisticData("MVZ_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Vr_S_Rate = ReportBase.GoodRateS01("MVZ_1").ToString("0.0000");

			// Ir
			string Ir_S_Min = ReportBase.StatisticData("MIR_1", EStatisticType.Single01).Min.ToString("0.0000");

			string Ir_S_Avg = ReportBase.StatisticData("MIR_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string Ir_S_Max = ReportBase.StatisticData("MIR_1", EStatisticType.Single01).Max.ToString("0.0000");

			string Ir_S_Std = ReportBase.StatisticData("MIR_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Ir_S_Rate = ReportBase.GoodRateS01("MIR_1").ToString("0.0000");

			// Rs
			string Rs_S_Min = "0.0000";

			string Rs_S_Avg = "0.0000";

			string Rs_S_Max = "0.0000";

			string Rs_S_Std = "0.0000";

			string Rs_S_Rate = "0.0000";

			// Iv
			string Iv_S_Min = "0.0000";

			string Iv_S_Avg = "0.0000";

			string Iv_S_Max = "0.0000";

			string Iv_S_Std = "0.0000";

			string Iv_S_Rate = "0.0000";

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				Iv_S_Min = ReportBase.StatisticData("LOP_1", EStatisticType.Single01).Min.ToString("0.0000");

				Iv_S_Avg = ReportBase.StatisticData("LOP_1", EStatisticType.Single01).Mean.ToString("0.0000");

				Iv_S_Max = ReportBase.StatisticData("LOP_1", EStatisticType.Single01).Max.ToString("0.0000");

				Iv_S_Std = ReportBase.StatisticData("LOP_1", EStatisticType.Single01).STDEV.ToString("0.0000");

				Iv_S_Rate = ReportBase.GoodRateS01("LOP_1").ToString("0.0000");
			}
			else
			{
				Iv_S_Min = ReportBase.StatisticData("WATT_1", EStatisticType.Single01).Min.ToString("0.0000");

				Iv_S_Avg = ReportBase.StatisticData("WATT_1", EStatisticType.Single01).Mean.ToString("0.0000");

				Iv_S_Max = ReportBase.StatisticData("WATT_1", EStatisticType.Single01).Max.ToString("0.0000");

				Iv_S_Std = ReportBase.StatisticData("WATT_1", EStatisticType.Single01).STDEV.ToString("0.0000");

				Iv_S_Rate = ReportBase.GoodRateS01("WATT_1").ToString("0.0000");
			}

			// λd
			string Wld_S_Min = ReportBase.StatisticData("WLD_1", EStatisticType.Single01).Min.ToString("0.0000");

			string Wld_S_Avg = ReportBase.StatisticData("WLD_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string Wld_S_Max = ReportBase.StatisticData("WLD_1", EStatisticType.Single01).Max.ToString("0.0000");

			string Wld_S_Std = ReportBase.StatisticData("WLD_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Wld_S_Rate = ReportBase.GoodRateS01("WLD_1").ToString("0.0000");

			// λp
			string Wlp_S_Min = ReportBase.StatisticData("WLP_1", EStatisticType.Single01).Min.ToString("0.0000");

			string Wlp_S_Avg = ReportBase.StatisticData("WLP_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string Wlp_S_Max = ReportBase.StatisticData("WLP_1", EStatisticType.Single01).Max.ToString("0.0000");

			string Wlp_S_Std = ReportBase.StatisticData("WLP_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Wlp_S_Rate = ReportBase.GoodRateS01("WLP_1").ToString("0.0000");

			// △λ
			string Hw_S_Min = ReportBase.StatisticData("HW_1", EStatisticType.Single01).Min.ToString("0.0000");

			string Hw_S_Avg = ReportBase.StatisticData("HW_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string Hw_S_Max = ReportBase.StatisticData("HW_1", EStatisticType.Single01).Max.ToString("0.0000");

			string Hw_S_Std = ReportBase.StatisticData("HW_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Hw_S_Rate = ReportBase.GoodRateS01("HW_1").ToString("0.0000");

			// CIE-x
			string CIEx_S_Min = ReportBase.StatisticData("CIEx_1", EStatisticType.Single01).Min.ToString("0.0000");

			string CIEx_S_Avg = ReportBase.StatisticData("CIEx_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string CIEx_S_Max = ReportBase.StatisticData("CIEx_1", EStatisticType.Single01).Max.ToString("0.0000");

			string CIEx_S_Std = ReportBase.StatisticData("CIEx_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string CIEx_S_Rate = ReportBase.GoodRateS01("CIEx_1").ToString("0.0000");

			// CIE-y
			string CIEy_S_Min = ReportBase.StatisticData("CIEy_1", EStatisticType.Single01).Min.ToString("0.0000");

			string CIEy_S_Avg = ReportBase.StatisticData("CIEy_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string CIEy_S_Max = ReportBase.StatisticData("CIEy_1", EStatisticType.Single01).Max.ToString("0.0000");

			string CIEy_S_Std = ReportBase.StatisticData("CIEy_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string CIEy_S_Rate = ReportBase.GoodRateS01("CIEy_1").ToString("0.0000");

			// Vf3
			string Vf3_S_Min = ReportBase.StatisticData("MVF_3", EStatisticType.Single01).Min.ToString("0.0000");

			string Vf3_S_Avg = ReportBase.StatisticData("MVF_3", EStatisticType.Single01).Mean.ToString("0.0000");

			string Vf3_S_Max = ReportBase.StatisticData("MVF_3", EStatisticType.Single01).Max.ToString("0.0000");

			string Vf3_S_Std = ReportBase.StatisticData("MVF_3", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Vf3_S_Rate = ReportBase.GoodRateS01("MVF_3").ToString("0.0000");

			// Iv3
			string Iv3_S_Min = "0.0000";

			string Iv3_S_Avg = "0.0000";

			string Iv3_S_Max = "0.0000";

			string Iv3_S_Std = "0.0000";

			string Iv3_S_Rate = "0.0000";

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				Iv3_S_Min = ReportBase.StatisticData("LOP_2", EStatisticType.Single01).Min.ToString("0.0000");

				Iv3_S_Avg = ReportBase.StatisticData("LOP_2", EStatisticType.Single01).Mean.ToString("0.0000");

				Iv3_S_Max = ReportBase.StatisticData("LOP_2", EStatisticType.Single01).Max.ToString("0.0000");

				Iv3_S_Std = ReportBase.StatisticData("LOP_2", EStatisticType.Single01).STDEV.ToString("0.0000");

				Iv3_S_Rate = ReportBase.GoodRateS01("LOP_2").ToString("0.0000");
			}
			else
			{
				Iv3_S_Min = ReportBase.StatisticData("WATT_2", EStatisticType.Single01).Min.ToString("0.0000");

				Iv3_S_Avg = ReportBase.StatisticData("WATT_2", EStatisticType.Single01).Mean.ToString("0.0000");

				Iv3_S_Max = ReportBase.StatisticData("WATT_2", EStatisticType.Single01).Max.ToString("0.0000");

				Iv3_S_Std = ReportBase.StatisticData("WATT_2", EStatisticType.Single01).STDEV.ToString("0.0000");

				Iv3_S_Rate = ReportBase.GoodRateS01("WATT_2").ToString("0.0000");
			}

			// λd3
			string Wld3_S_Min = ReportBase.StatisticData("WLD_2", EStatisticType.Single01).Min.ToString("0.0000");

			string Wld3_S_Avg = ReportBase.StatisticData("WLD_2", EStatisticType.Single01).Mean.ToString("0.0000");

			string Wld3_S_Max = ReportBase.StatisticData("WLD_2", EStatisticType.Single01).Max.ToString("0.0000");

			string Wld3_S_Std = ReportBase.StatisticData("WLD_2", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Wld3_S_Rate = ReportBase.GoodRateS01("WLD_2").ToString("0.0000");

			// Vf4
			string Vf4_S_Min = ReportBase.StatisticData("MVF_4", EStatisticType.Single01).Min.ToString("0.0000");

			string Vf4_S_Avg = ReportBase.StatisticData("MVF_4", EStatisticType.Single01).Mean.ToString("0.0000");

			string Vf4_S_Max = ReportBase.StatisticData("MVF_4", EStatisticType.Single01).Max.ToString("0.0000");

			string Vf4_S_Std = ReportBase.StatisticData("MVF_4", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Vf4_S_Rate = ReportBase.GoodRateS01("MVF_4").ToString("0.0000");

			// Vf5
			string Vf5_S_Min = ReportBase.StatisticData("MVF_5", EStatisticType.Single01).Min.ToString("0.0000");

			string Vf5_S_Avg = ReportBase.StatisticData("MVF_5", EStatisticType.Single01).Mean.ToString("0.0000");

			string Vf5_S_Max = ReportBase.StatisticData("MVF_5", EStatisticType.Single01).Max.ToString("0.0000");

			string Vf5_S_Std = ReportBase.StatisticData("MVF_5", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Vf5_S_Rate = ReportBase.GoodRateS01("MVF_5").ToString("0.0000");

			// Vfd
			string Vfd_S_Min = ReportBase.StatisticData("MTHYVD_1", EStatisticType.Single01).Min.ToString("0.0000");

			string Vfd_S_Avg = ReportBase.StatisticData("MTHYVD_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string Vfd_S_Max = ReportBase.StatisticData("MTHYVD_1", EStatisticType.Single01).Max.ToString("0.0000");

			string Vfd_S_Std = ReportBase.StatisticData("MTHYVD_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Vfd_S_Rate = ReportBase.GoodRateS01("MTHYVD_1").ToString("0.0000");

			// Wd-Wp
			string Wdd_S_Min = ReportBase.StatisticData("DWDWP_1", EStatisticType.Single01).Min.ToString("0.0000");

			string Wdd_S_Avg = ReportBase.StatisticData("DWDWP_1", EStatisticType.Single01).Mean.ToString("0.0000");

			string Wdd_S_Max = ReportBase.StatisticData("DWDWP_1", EStatisticType.Single01).Max.ToString("0.0000");

			string Wdd_S_Std = ReportBase.StatisticData("DWDWP_1", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Wdd_S_Rate = ReportBase.GoodRateS01("DWDWP_1").ToString("0.0000");

			// Iv5
			string Iv5_S_Min = "0.0000";

			string Iv5_S_Avg = "0.0000";

			string Iv5_S_Max = "0.0000";

			string Iv5_S_Std = "0.0000";

			string Iv5_S_Rate = "0.0000";

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				Iv5_S_Min = ReportBase.StatisticData("LOP_3", EStatisticType.Single01).Min.ToString("0.0000");

				Iv5_S_Avg = ReportBase.StatisticData("LOP_3", EStatisticType.Single01).Mean.ToString("0.0000");

				Iv5_S_Max = ReportBase.StatisticData("LOP_3", EStatisticType.Single01).Max.ToString("0.0000");

				Iv5_S_Std = ReportBase.StatisticData("LOP_3", EStatisticType.Single01).STDEV.ToString("0.0000");

				Iv5_S_Rate = ReportBase.GoodRateS01("LOP_3").ToString("0.0000");
			}
			else
			{
				Iv5_S_Min = ReportBase.StatisticData("WATT_3", EStatisticType.Single01).Min.ToString("0.0000");

				Iv5_S_Avg = ReportBase.StatisticData("WATT_3", EStatisticType.Single01).Mean.ToString("0.0000");

				Iv5_S_Max = ReportBase.StatisticData("WATT_3", EStatisticType.Single01).Max.ToString("0.0000");

				Iv5_S_Std = ReportBase.StatisticData("WATT_3", EStatisticType.Single01).STDEV.ToString("0.0000");

				Iv5_S_Rate = ReportBase.GoodRateS01("WATT_3").ToString("0.0000");
			}

			// λd5
			string Wld5_S_Min = ReportBase.StatisticData("WLD_3", EStatisticType.Single01).Min.ToString("0.0000");

			string Wld5_S_Avg = ReportBase.StatisticData("WLD_3", EStatisticType.Single01).Mean.ToString("0.0000");

			string Wld5_S_Max = ReportBase.StatisticData("WLD_3", EStatisticType.Single01).Max.ToString("0.0000");

			string Wld5_S_Std = ReportBase.StatisticData("WLD_3", EStatisticType.Single01).STDEV.ToString("0.0000");

			string Wld5_S_Rate = ReportBase.GoodRateS01("WLD_3").ToString("0.0000");

			//////////////////////////////////////////////////////////
			// Replace Data
			//////////////////////////////////////////////////////////
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string date = "Date," + this.TesterSetting.EndTestTime.ToString("yyyy/M/d");

			replaceData.Add("Date,", date);

			// 全部統計
			replaceData.Add("Vfin(V),A", "Vfin(V)," + Vfin_A_Min + "," + Vfin_A_Avg + "," + Vfin_A_Max + "," + Vfin_A_Std);

			replaceData.Add("Vf(V),A", "Vf(V)," + Vf_A_Min + "," + Vf_A_Avg + "," + Vf_A_Max + "," + Vf_A_Std);

			replaceData.Add("Vr(V),A", "Vr(V)," + Vr_A_Min + "," + Vr_A_Avg + "," + Vr_A_Max + "," + Vr_A_Std);

			replaceData.Add("Ir(uA)A,", "Ir(uA)," + Ir_A_Min + "," + Ir_A_Avg + "," + Ir_A_Max + "," + Ir_A_Std);

			replaceData.Add("Rs(ohm),A", "Rs(ohm)," + Rs_A_Min + "," + Rs_A_Avg + "," + Rs_A_Max + "," + Rs_A_Std);

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				replaceData.Add("Iv(mcd),A", "Iv(mcd)," + Iv_A_Min + "," + Iv_A_Avg + "," + Iv_A_Max + "," + Iv_A_Std);
			}
			else
			{
				replaceData.Add("Iv(mcd),A", "Iv(mwatt)," + Iv_A_Min + "," + Iv_A_Avg + "," + Iv_A_Max + "," + Iv_A_Std);
			}

			replaceData.Add("λd(nm),A", "λd(nm)," + Wld_A_Min + "," + Wld_A_Avg + "," + Wld_A_Max + "," + Wld_A_Std);

			replaceData.Add("λp(nm),A", "λp(nm)," + Wlp_A_Min + "," + Wlp_A_Avg + "," + Wlp_A_Max + "," + Wlp_A_Std);

			replaceData.Add("△λ(nm),A", "△λ(nm)," + Hw_A_Min + "," + Hw_A_Avg + "," + Hw_A_Max + "," + Hw_A_Std);

			replaceData.Add("CIE-x,A", "CIE-x," + CIEx_A_Min + "," + CIEx_A_Avg + "," + CIEx_A_Max + "," + CIEx_A_Std);

			replaceData.Add("CIE-y,A", "CIE-y," + CIEy_A_Min + "," + CIEy_A_Avg + "," + CIEy_A_Max + "," + CIEy_A_Std);

			replaceData.Add("Vf3(V),A", "Vf3(V)," + Vf3_A_Min + "," + Vf3_A_Avg + "," + Vf3_A_Max + "," + Vf3_A_Std);

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				replaceData.Add("Iv3(mcd),A", "Iv3(mcd)," + Iv3_A_Min + "," + Iv3_A_Avg + "," + Iv3_A_Max + "," + Iv3_A_Std);
			}
			else
			{
				replaceData.Add("Iv3(mcd),A", "Iv3(mwatt)," + Iv3_A_Min + "," + Iv3_A_Avg + "," + Iv3_A_Max + "," + Iv3_A_Std);
			}

			replaceData.Add("λd3(nm),A", "λd3(nm)," + Wld3_A_Min + "," + Wld3_A_Avg + "," + Wld3_A_Max + "," + Wld3_A_Std);

			replaceData.Add("Vf4(V),A", "Vf4(V)," + Vf4_A_Min + "," + Vf4_A_Avg + "," + Vf4_A_Max + "," + Vf4_A_Std);

			replaceData.Add("Vf5(V),A", "Vf5(V)," + Vf5_A_Min + "," + Vf5_A_Avg + "," + Vf5_A_Max + "," + Vf5_A_Std);

			replaceData.Add("VfD(V),A", "VfD(V)," + Vfd_A_Min + "," + Vfd_A_Avg + "," + Vfd_A_Max + "," + Vfd_A_Std);

			replaceData.Add("Wd-Wp(nm),A", "Wd-Wp(nm)," + Wdd_A_Min + "," + Wdd_A_Avg + "," + Wdd_A_Max + "," + Wdd_A_Std);

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				replaceData.Add("Iv5(mcd),A", "Iv5(mcd)," + Iv5_A_Min + "," + Iv5_A_Avg + "," + Iv5_A_Max + "," + Iv5_A_Std);
			}
			else
			{
				replaceData.Add("Iv5(mcd),A", "Iv5(mwatt)," + Iv5_A_Min + "," + Iv5_A_Avg + "," + Iv5_A_Max + "," + Iv5_A_Std);
			}

			replaceData.Add("λd5(nm),A", "λd5(nm)," + Wld5_A_Min + "," + Wld5_A_Avg + "," + Wld5_A_Max + "," + Wld5_A_Std);

			// 良率統計
			replaceData.Add("Quantity,", "Quantity," + Good + "," + Fail + "," + Total);

			replaceData.Add("%,", "%," + GoodRate + "," + FailRate + "," + TotalRate);			

			//單獨統計
			replaceData.Add("Vfin(V),S", "Vfin(V)," + Vfin_S_Min + "," + Vfin_S_Avg + "," + Vfin_S_Max + "," + Vfin_S_Std +  "," + Vfin_S_Rate);

			replaceData.Add("Vf(V),S", "Vf(V)," + Vf_S_Min + "," + Vf_S_Avg + "," + Vf_S_Max + "," + Vf_S_Std + "," + Vf_S_Rate);

			replaceData.Add("Vr(V),S", "Vr(V)," + Vr_S_Min + "," + Vr_S_Avg + "," + Vr_S_Max + "," + Vr_S_Std + "," + Vr_S_Rate);

			replaceData.Add("Ir(uA),S", "Ir(uA)," + Ir_S_Min + "," + Ir_S_Avg + "," + Ir_S_Max + "," + Ir_S_Std + "," + Ir_S_Rate);

			replaceData.Add("Rs(ohm),S", "Rs(ohm)," + Rs_S_Min + "," + Rs_S_Avg + "," + Rs_S_Max + "," + Rs_S_Std + "," + Rs_S_Rate);

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				replaceData.Add("Iv(mcd),S", "Iv(mcd)," + Iv_S_Min + "," + Iv_S_Avg + "," + Iv_S_Max + "," + Iv_S_Std + "," + Iv_S_Rate);
			}
			else
			{
				replaceData.Add("Iv(mcd),S", "Iv(mwatt)," + Iv_S_Min + "," + Iv_S_Avg + "," + Iv_S_Max + "," + Iv_S_Std + "," + Iv_S_Rate);
			}

			replaceData.Add("λd(nm),S", "λd(nm)," + Wld_S_Min + "," + Wld_S_Avg + "," + Wld_S_Max + "," + Wld_S_Std + "," + Wld_S_Rate);

			replaceData.Add("λp(nm),S", "λp(nm)," + Wlp_S_Min + "," + Wlp_S_Avg + "," + Wlp_S_Max + "," + Wlp_S_Std + "," + Wlp_S_Rate);

			replaceData.Add("△λ(nm),S", "△λ(nm)," + Hw_S_Min + "," + Hw_S_Avg + "," + Hw_S_Max + "," + Hw_S_Std + "," + Hw_S_Rate);

			replaceData.Add("CIE-x,S", "CIE-x," + CIEx_S_Min + "," + CIEx_S_Avg + "," + CIEx_S_Max + "," + CIEx_S_Std + "," + CIEx_S_Rate);

			replaceData.Add("CIE-y,S", "CIE-y," + CIEy_S_Min + "," + CIEy_S_Avg + "," + CIEy_S_Max + "," + CIEy_S_Std + "," + CIEy_S_Rate);

			replaceData.Add("Vf3(V),S", "Vf3(V)," + Vf3_S_Min + "," + Vf3_S_Avg + "," + Vf3_S_Max + "," + Vf3_S_Std + "," + Vf3_S_Rate);

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				replaceData.Add("Iv3(mcd),S", "Iv3(mcd)," + Iv3_S_Min + "," + Iv3_S_Avg + "," + Iv3_S_Max + "," + Iv3_S_Std + "," + Iv3_S_Rate);
			}
			else
			{
				replaceData.Add("Iv3(mcd),S", "Iv3(mwatt)," + Iv3_S_Min + "," + Iv3_S_Avg + "," + Iv3_S_Max + "," + Iv3_S_Std + "," + Iv3_S_Rate);
			}

			replaceData.Add("λd3(nm),S", "λd3(nm)," + Wld3_S_Min + "," + Wld3_S_Avg + "," + Wld3_S_Max + "," + Wld3_S_Std + "," + Wld3_S_Rate);

			replaceData.Add("Vf4(V),S", "Vf4(V)," + Vf4_S_Min + "," + Vf4_S_Avg + "," + Vf4_S_Max + "," + Vf4_S_Std + "," + Vf4_S_Rate);

			replaceData.Add("Vf5(V),S", "Vf5(V)," + Vf5_S_Min + "," + Vf5_S_Avg + "," + Vf5_S_Max + "," + Vf5_S_Std + "," + Vf5_S_Rate);

			replaceData.Add("VfD(V),S", "VfD(V)," + Vfd_S_Min + "," + Vfd_S_Avg + "," + Vfd_S_Max + "," + Vfd_S_Std + "," + Vfd_S_Rate);

			replaceData.Add("Wd-Wp(nm),S", "Wd-Wp(nm)," + Wdd_S_Min + "," + Wdd_S_Avg + "," + Wdd_S_Max + "," + Wdd_S_Std + "," + Wdd_S_Rate);

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				replaceData.Add("Iv5(mcd),S", "Iv5(mcd)," + Iv5_S_Min + "," + Iv5_S_Avg + "," + Iv5_S_Max + "," + Iv5_S_Std + "," + Iv5_S_Rate);
			}
			else
			{
				replaceData.Add("Iv5(mcd),S", "Iv5(mwatt)," + Iv5_S_Min + "," + Iv5_S_Avg + "," + Iv5_S_Max + "," + Iv5_S_Std + "," + Iv5_S_Rate);
			}

			replaceData.Add("λd5(nm),S", "λd5(nm)," + Wld5_S_Min + "," + Wld5_S_Avg + "," + Wld5_S_Max + "," + Wld5_S_Std + "," + Wld5_S_Rate);

			this.ReplaceReport2(replaceData);

			return EErrorCode.NONE;
		}

		protected override EErrorCode MoveFileToTargetByUser2(TestServer.EServerQueryCmd cmd)
		{
			bool isOutputPath02 = false;

			bool isOutputPath03 = false;

			string outPath01 = string.Empty;

			string outPath02 = string.Empty;

			string outPath03 = string.Empty;

			ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

			ETesterResultCreatFolderType type02 = ETesterResultCreatFolderType.None;

			ETesterResultCreatFolderType type03 = ETesterResultCreatFolderType.None;

			if (this.UISetting.IsManualRunMode)
			{
				isOutputPath02 = this.UISetting.IsEnableSTATPath02;

				isOutputPath03 = this.UISetting.IsEnableSTATPath03;

				outPath01 = this.UISetting.ManualOutputPath01;

				outPath02 = this.UISetting.ManualOutputPath02;

				outPath03 = this.UISetting.ManualOutputPath03;

				type01 = this.UISetting.ManualOutputPathType01;

				type02 = this.UISetting.ManualOutputPathType02;

				type03 = this.UISetting.ManualOutputPathType03;
			}
			else
			{
				isOutputPath02 = this.UISetting.IsEnableSTATPath02;

				isOutputPath03 = this.UISetting.IsEnableSTATPath03;

				outPath01 = this.UISetting.STATOutputPath01;

				outPath02 = this.UISetting.STATOutputPath02;

				outPath03 = this.UISetting.STATOutputPath03;

				type01 = this.UISetting.STATTesterResultCreatFolderType01;

				type02 = this.UISetting.STATTesterResultCreatFolderType02;

				type03 = this.UISetting.STATTesterResultCreatFolderType03;
			}

			if (type01 == ETesterResultCreatFolderType.ByLotNumber)
			{
				outPath01 = Path.Combine(outPath01, this.UISetting.LotNumber);
			}
			else if (type01 == ETesterResultCreatFolderType.ByMachineName)
			{
				outPath01 = Path.Combine(outPath01, this.UISetting.MachineName);
			}
			else if (type01 == ETesterResultCreatFolderType.ByDataTime)
			{
				outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
			}

			if (type02 == ETesterResultCreatFolderType.ByLotNumber)
			{
				outPath02 = Path.Combine(outPath02, this.UISetting.LotNumber);
			}
			else if (type02 == ETesterResultCreatFolderType.ByMachineName)
			{
				outPath02 = Path.Combine(outPath02, this.UISetting.MachineName);
			}
			else if (type02 == ETesterResultCreatFolderType.ByDataTime)
			{
				outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
			}

			if (type03 == ETesterResultCreatFolderType.ByLotNumber)
			{
				outPath03 = Path.Combine(outPath03, this.UISetting.LotNumber);
			}
			else if (type03 == ETesterResultCreatFolderType.ByMachineName)
			{
				outPath03 = Path.Combine(outPath03, this.UISetting.MachineName);
			}
			else if (type03 == ETesterResultCreatFolderType.ByDataTime)
			{
				outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
			}

			//---------------------------------------------------------------------------------
			// Copy Report file to taget path
			//---------------------------------------------------------------------------------
			string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

			string fileNameWithExt = this.TestResultFileNameWithoutExt();

			//Abort
			if (cmd == TestServer.EServerQueryCmd.CMD_TESTER_ABORT)
			{
				fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
			}

			fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.STATTestResultFileExt;

			string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

			string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

			string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile01);

			if (isOutputPath02)
			{
                MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile02);
			}

			if (isOutputPath03)
			{
                MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile03);
			}

			return EErrorCode.NONE;
		}

		protected override Data.EErrorCode WriteReportHeadByUser3()
		{
			this.WriteLine3("PROBE_TYPE,WAFER_NO,JOB_OUT_DATETIME,JOB_OUT_USER,MACHINE_NO,MPI_PRD,ESD_Result");

			this.WriteLine3("Data");

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser3()
		{
			//PROBE_TYPE:		Remark01
			//JOB_IN_DATETIME : Remark02
			//ESD_Result:		Remark03

			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			//PROBE_TYPE
			string data = this.UISetting.WeiminUIData.Remark01 + ",";

			//WAFER_NO
			data += this.UISetting.WaferNumber + ",";

			//JOB_OUT_DATETIME
			data += this.TesterSetting.EndTestTime.ToString("yyyy/M/d H:m:s") + ",";

			//JOB_OUT_USER
			data += this.UISetting.OperatorName + ",";

			//MACHINE_NO
			data += this.UISetting.MachineName + ",";

			//MPI_PRD
			data += this.UISetting.ProberRecipeName + ",";

			//ESD_Result
			data += this.UISetting.WeiminUIData.Remark03;

			replaceData.Add("Data", data);

			this.ReplaceReport3(replaceData);

			return EErrorCode.NONE;
		}

		protected override EErrorCode MoveFileToTargetByUser3(TestServer.EServerQueryCmd cmd)
		{
			bool isOutputPath02 = false;

			bool isOutputPath03 = false;

			string outPath01 = string.Empty;

			string outPath02 = string.Empty;

			string outPath03 = string.Empty;

			ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

			ETesterResultCreatFolderType type02 = ETesterResultCreatFolderType.None;

			ETesterResultCreatFolderType type03 = ETesterResultCreatFolderType.None;

			if (this.UISetting.IsManualRunMode)
			{
				isOutputPath02 = this.UISetting.IsEnableWAFPath02;

				isOutputPath03 = this.UISetting.IsEnableWAFPath03;

				outPath01 = this.UISetting.ManualOutputPath01;

				outPath02 = this.UISetting.ManualOutputPath02;

				outPath03 = this.UISetting.ManualOutputPath03;

				type01 = this.UISetting.ManualOutputPathType01;

				type02 = this.UISetting.ManualOutputPathType02;

				type03 = this.UISetting.ManualOutputPathType03;
			}
			else
			{
				isOutputPath02 = this.UISetting.IsEnableWAFPath02;

				isOutputPath03 = this.UISetting.IsEnableWAFPath03;

				outPath01 = this.UISetting.WAFOutputPath01;

				outPath02 = this.UISetting.WAFOutputPath02;

				outPath03 = this.UISetting.WAFOutputPath03;

				type01 = this.UISetting.WAFTesterResultCreatFolderType01;

				type02 = this.UISetting.WAFTesterResultCreatFolderType02;

				type03 = this.UISetting.WAFTesterResultCreatFolderType03;
			}

			if (type01 == ETesterResultCreatFolderType.ByLotNumber)
			{
				outPath01 = Path.Combine(outPath01, this.UISetting.LotNumber);
			}
			else if (type01 == ETesterResultCreatFolderType.ByMachineName)
			{
				outPath01 = Path.Combine(outPath01, this.UISetting.MachineName);
			}
			else if (type01 == ETesterResultCreatFolderType.ByDataTime)
			{
				outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
			}

			if (type02 == ETesterResultCreatFolderType.ByLotNumber)
			{
				outPath02 = Path.Combine(outPath02, this.UISetting.LotNumber);
			}
			else if (type02 == ETesterResultCreatFolderType.ByMachineName)
			{
				outPath02 = Path.Combine(outPath02, this.UISetting.MachineName);
			}
			else if (type02 == ETesterResultCreatFolderType.ByDataTime)
			{
				outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
			}

			if (type03 == ETesterResultCreatFolderType.ByLotNumber)
			{
				outPath03 = Path.Combine(outPath03, this.UISetting.LotNumber);
			}
			else if (type03 == ETesterResultCreatFolderType.ByMachineName)
			{
				outPath03 = Path.Combine(outPath03, this.UISetting.MachineName);
			}
			else if (type03 == ETesterResultCreatFolderType.ByDataTime)
			{
				outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
			}

			//---------------------------------------------------------------------------------
			// Copy Report file to taget path
			//---------------------------------------------------------------------------------
			string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

			string fileNameWithExt = this.TestResultFileNameWithoutExt();

			//Abort
			if (cmd == TestServer.EServerQueryCmd.CMD_TESTER_ABORT)
			{
				fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
			}

			fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.WAFTestResultFileExt;

			string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

			string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

			string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

			MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile01);

			if (isOutputPath02)
			{
                MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile02);
			}

			if (isOutputPath03)
			{
                MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile03);
			}

			return EErrorCode.NONE;
		}
	}
}
