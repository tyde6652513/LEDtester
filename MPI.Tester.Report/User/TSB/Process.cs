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

namespace MPI.Tester.Report.User.Lextar_DP
{
	class Report : ReportBase
	{
		public Report(List<object> objs)
			: base(objs)
		{			
		}

		#region >>> Protected Override Method <<<

		protected override EErrorCode WriteReportHeadByUser()
		{
			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////
			this.WriteLine("FileID,,WEI MIN Data File");

			this.WriteLine("TestTime,,");

			this.WriteLine("WO,," + this.UISetting.WO);

			this.WriteLine("Operator,," + this.UISetting.LoginID);

			this.WriteLine("MaximumBin,,255");

			this.WriteLine("Specification,," + this.UISetting.TaskSheetFileName);

			this.WriteLine("SpecRemark,,");

			this.WriteLine("FileName,,");

			this.WriteLine("ProductName,,");

			this.WriteLine("DeviceNumber,," + this.UISetting.WeiminUIData.DeviceNumber);

			this.WriteLine("LOT_ID,," + this.UISetting.LotNumber);

			this.WriteLine("Customer,,");

			this.WriteLine("Class,,");

			this.WriteLine("OrderNumber,,");

			this.WriteLine("Temperature,,");

			this.WriteLine("Humidity,,");

			this.WriteLine("Remark1,,");

			this.WriteLine("Remark2,,");

			this.WriteLine("Remark3,," + FileVersionInfo.GetVersionInfo(@"C:\MPI\LEDTester\LEDTester.exe").FileVersion.ToString());

			this.WriteLine("Remark4,," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

			this.WriteLine("EQP_ID,," + this.UISetting.MachineName);

			this.WriteLine("Recipe_Name,," + this.UISetting.TaskSheetFileName);

			this.WriteLine("QTY,,");

			this.WriteLine("SideBin,,");

			this.WriteLine("NGBin,,");

			this.WriteLine("ResortBin,,");

			this.WriteLine("TryLot,,No");

			this.WriteLine("CORREL1,,");

			this.WriteLine("CORREL2,,");

			this.WriteLine("");

			////////////////////////////////////////////
			//Write Result Item Title
			////////////////////////////////////////////
			string title = string.Empty;

			if (this.UISetting.IsEnableFloatReport)
			{
				this.ResultTitleInfo.Clear();

				this.ResultTitleInfo.AddResultData("COL", "PosX");

				this.ResultTitleInfo.AddResultData("ROW", "PosY");

				this.ResultTitleInfo.AddResultData("BIN", "Bin");

				this.ResultTitleInfo.AddResultData("TEST", "TEST");

				this.ResultTitleInfo.AddResultData("BIN_CODE", "BIN_CODE");

				this.ResultTitleInfo.AddResultData("BIN_GRADE", "BIN_GRADE");

				this.ResultTitleInfo.AddResultData("POLAR", "TPOLAR");

				this.ResultTitleInfo.AppendResultData(this.ResultData);
			}
			else
			{
				this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
			}

			this.WriteLine(this.ResultTitleInfo.TitleStr);

			this.WriteLine("<<<" + this.UISetting.LotNumber + ">>>");

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
			////////////////////////////////////////////
			//Rerite Report Head
			////////////////////////////////////////////
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string testTime = "TestTime,," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm");

			string fileName = "FileName,," + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt;

			string qty = "QTY,," + ReportProcess.TestCount.ToString();

			string sideBin = "SideBin,," + this.SmartBinning.SideBin.ChipCount.ToString();

			string ngBin = "NGBin,," + this.SmartBinning.NGBin.ChipCount.ToString();

			////////////////////////////////////////////
			//Insert statistic Data
			////////////////////////////////////////////
			string correl2 = "CORREL2,," + Environment.NewLine;

			string statisticTitle = "SUMITEM,Bin,BIN_CODE,BIN_GRADE,TPOLAR";

			foreach (var item in this.ResultData)
			{
				statisticTitle += ",";

				statisticTitle += item.Value.Name;
			}

			correl2 += statisticTitle + Environment.NewLine;

			string maxLine = "MAX," + this.SmartBinning.StatisticBin.Max + ",0,0," + ReportProcess.StatisticData(ESysResultItem.POLAR, EStatisticType.All01).Max.ToString();

			string minLine = "MIN," + this.SmartBinning.StatisticBin.Min + ",0,0," + ReportProcess.StatisticData(ESysResultItem.POLAR, EStatisticType.All01).Min.ToString();

			string avgLine = "AVG," + this.SmartBinning.StatisticBin.Mean + ",0,0," + ReportProcess.StatisticData(ESysResultItem.POLAR, EStatisticType.All01).Mean.ToString();

			string stdLine = "STD," + this.SmartBinning.StatisticBin.STDEV + ",0,0," + ReportProcess.StatisticData(ESysResultItem.POLAR, EStatisticType.All01).STDEV.ToString();

			string ngLine = "NG,0,0,0,0";

			string sideLine = "Cond_NG,0,0,0,0";

			foreach (var item in this.ResultData)
			{
				string format = item.Value.Formate;

				maxLine += "," + ReportProcess.StatisticData(item.Key, EStatisticType.Single01).Max.ToString(format);

				minLine += "," + ReportProcess.StatisticData(item.Key, EStatisticType.Single01).Min.ToString(format);

				avgLine += "," + ReportProcess.StatisticData(item.Key, EStatisticType.Single01).Mean.ToString(format);

				stdLine += "," + ReportProcess.StatisticData(item.Key, EStatisticType.Single01).STDEV.ToString(format);

				uint ngCount = 0;

				uint sideCount = 0;

				if (this.SmartBinning.NGBin.ContainsKey(item.Key))
				{
					ngCount = this.SmartBinning.NGBin[item.Key].ChipCount;
				}

				if (this.SmartBinning.SideBin.ContainsKey(item.Key))
				{
					sideCount = this.SmartBinning.SideBin[item.Key].ChipCount;
				}

				ngLine += "," + ngCount.ToString();

				sideLine += "," + sideCount.ToString();
			}

			correl2 += maxLine + Environment.NewLine;

			correl2 += minLine + Environment.NewLine;

			correl2 += avgLine + Environment.NewLine;

			correl2 += stdLine + Environment.NewLine;

			correl2 += ngLine + Environment.NewLine;

			correl2 += sideLine + Environment.NewLine;

			replaceData.Add("TestTime,,", testTime);

			replaceData.Add("FileName,,", fileName);

			replaceData.Add("QTY,,", qty);

			replaceData.Add("SideBin,,", sideBin);

			replaceData.Add("NGBin,,", ngBin);

			replaceData.Add("CORREL2,,", correl2);

			this.UISetting.TestResultFileName = this.TestResultFileNameWithoutExt();

			this.FileFullNameCsv = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, this.TestResultFileNameWithoutExt() + ".csv");

			this.ReplaceReport(replaceData);

			if (this.TesterSetting.IsCheckRowCol)
			{
				this.CheckRowCol();
			}

			return EErrorCode.NONE;
		}

		#endregion

		#region >>> Public Override Method <<<

		public override string TestResultFileNameWithoutExt()
		{
			//機台號碼_工單號碼_生產日期_LOT NUMBER_IN BIN良率_SIDE BIN良率_NG BIN良率
			string fileNameWithoutExt = this.UISetting.MachineName;

			fileNameWithoutExt += "_" + this.UISetting.WO;

			fileNameWithoutExt += "_#" + this.TesterSetting.EndTestTime.ToString("yyyyMMddHHmmss") + "#";

			fileNameWithoutExt += "_" + this.UISetting.LotNumber;

			fileNameWithoutExt += "_" + this.SmartBinning.InBinRate.ToString("0.00");

			fileNameWithoutExt += "_" + this.SmartBinning.SideBinRate.ToString("0.00");

			fileNameWithoutExt += "_" + this.SmartBinning.NGBinRate.ToString("0.00");

			return fileNameWithoutExt;
		}

		#endregion
	}
}
