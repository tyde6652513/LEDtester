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
using MPI.Tester.DeviceCommon;
using System.Xml;
using System.Xml.Linq;

namespace MPI.Tester.Report.User.EPISKY
{
	class Report : ReportBase
	{
		private Dictionary<string, ElectSettingData[]> _dicItemForceValue = new Dictionary<string, ElectSettingData[]>();
		private Dictionary<string, string> _dicItemNullValue = new Dictionary<string, string>();

		private List<int> _zapTable = new List<int>();
		private List<double> _listMIRJ = new List<double>();
		private List<string> _resultItemInReport = new List<string>();

		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
          
		}

		private void ProductInfoExport(string path, string state)
		{
			List<string[]> data = new List<string[]>();

			if (!File.Exists(path))
			{
				string[] newdata = { "機台號", "WaferID", "Start/End", "Strat Time/EndTime", "Test Count" };

				data.Add(newdata);

				CSVUtil.WriteCSV(path, data);
			}

			data.Clear();

			data = CSVUtil.ReadCSV(path);

			if (state == "Start")
			{
				string[] dataLine = { this.UISetting.MachineName, this.UISetting.WaferNumber, state, this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"), "" };

				data.Add(dataLine);
			}
			else if (state == "End")
			{
				string[] dataLine = { this.UISetting.MachineName, this.UISetting.WaferNumber, state, this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm"), ReportProcess.TestCount.ToString() };

				data.Add(dataLine);
			}

			CSVUtil.WriteCSV(path, data);
		}

		private string GetItemForceValue(string keyName)
		{
			double forceValue;

			ElectSettingData[] electSettingData = new ElectSettingData[this._dicItemForceValue.Count];

			if (!this._dicItemForceValue.TryGetValue(keyName, out electSettingData))
			{
				return "";
			}
			else
			{
				forceValue = electSettingData[0].ForceValue;

				return forceValue.ToString();
			}

		}

		private string GetItemForceTime(string keyName)
		{
			double forceTime;
			ElectSettingData[] electSettingData = new ElectSettingData[this._dicItemForceValue.Count];

			if (!this._dicItemForceValue.TryGetValue(keyName, out electSettingData))
			{
				return "";
			}
			else
			{
				forceTime = electSettingData[0].ForceTime;

				return forceTime.ToString();
			}

		}

		private void ImportUserXML()
		{
			string xmlPath = Path.Combine(Constants.Paths.USER_DIR, "User6016.xml");

			XmlDocument XmlDoc = new XmlDocument();
			XmlDoc.Load(xmlPath);
			XmlNodeList NodeList = XmlDoc.SelectNodes("UserDefine/Formats/Format/MsrtDisplayItem/C");

			this._dicItemNullValue.Clear();

			foreach (XmlElement node in NodeList)
			{
				this._dicItemNullValue.Add(node.InnerText, node.GetAttribute("nullValue"));
			}
		}

		private string SetForceValueTable()
		{
			string lineForceValueTable = string.Empty;

			lineForceValueTable += "@" + this.GetItemForceValue("IF_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IF_2") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IF_3") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IF_4") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFMA_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFMC_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFMB_1") + "*";

			lineForceValueTable += this.GetItemForceTime("IFMB_1") + ",,,";

			lineForceValueTable += "@" + this.GetItemForceValue("IZ_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IZ_2") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("VR_1") + ",";

			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_3") + ",";

			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",,,,,,";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",";

			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",,,,,,";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",";

			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_3") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_3") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_3") + ",,,,,,";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_3") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_3") + ",";
			lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_3") + ",,";

			return lineForceValueTable;

		}

		#region >>> Protected Override Method <<<

		protected override void SetResultTitle()
		{
			this.ImportUserXML();

			// Insert result header//
			this.ResultTitleInfo.Clear();

			this.ResultTitleInfo.AddResultData("TEST", "TEST");

			this.ResultTitleInfo.AddResultData("BIN", "BIN");

			this.ResultTitleInfo.AddResultData("CONTA", "CONTA");

			this.ResultTitleInfo.AddResultData("CONTC", "CONTC");

			this.ResultTitleInfo.AddResultData("POLAR", "POLAR");

			this.ResultTitleInfo.AddResultData("MVF_1", "VF1");

			this.ResultTitleInfo.AddResultData("MVF_2", "VF2");

			this.ResultTitleInfo.AddResultData("MVF_3", "VF3");

			this.ResultTitleInfo.AddResultData("MVF_4", "VF4");

			this.ResultTitleInfo.AddResultData("MVFMA_1", "VFM1");

			this.ResultTitleInfo.AddResultData("MVFMB_1", "VFM2");

			this.ResultTitleInfo.AddResultData("MVFMD_1", "DVF");

			this.ResultTitleInfo.AddResultData("MTHYVP_1", "VF");

			this.ResultTitleInfo.AddResultData("MTHYVD_1", "VFD");

			this.ResultTitleInfo.AddResultData("MVZ_1", "VZ1");

			this.ResultTitleInfo.AddResultData("MIR_2", "IR2");

			this.ResultTitleInfo.AddResultData("MIR_1", "IR");

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				this.ResultTitleInfo.AddResultData("LOP_1", "LOP1");

				this.ResultTitleInfo.AddResultData("LOP_2", "LOP2");

				this.ResultTitleInfo.AddResultData("LOP_3", "LOP3");
			}
			else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
			{
				this.ResultTitleInfo.AddResultData("LM_1", "Lm1");

				this.ResultTitleInfo.AddResultData("LM_2", "Lm2");

				this.ResultTitleInfo.AddResultData("LM_3", "Lm3");
			}
			else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
			{
				this.ResultTitleInfo.AddResultData("WATT_1", "PO1");

				this.ResultTitleInfo.AddResultData("WATT_2", "PO2");

				this.ResultTitleInfo.AddResultData("WATT_3", "PO3");
			}
			else
			{
				this.ResultTitleInfo.AddResultData("LOP_1", "LOP1");

				this.ResultTitleInfo.AddResultData("LOP_2", "LOP2");

				this.ResultTitleInfo.AddResultData("LOP_3", "LOP3");
			}

			this.ResultTitleInfo.AddResultData("WLP_1", "WLP1");

			this.ResultTitleInfo.AddResultData("WLD_1", "WLD1");

			this.ResultTitleInfo.AddResultData("WLC_1", "WLC1");

			this.ResultTitleInfo.AddResultData("HW_1", "HW1");

			this.ResultTitleInfo.AddResultData("PURITY_1", "PURITY1");

			this.ResultTitleInfo.AddResultData("CIEx_1", "X1");

			this.ResultTitleInfo.AddResultData("CIEy_1", "Y1");

			this.ResultTitleInfo.AddResultData("CIEz_1", "Z1");

            this.ResultTitleInfo.AddResultData("CCT_1", "CCT1");

			this.ResultTitleInfo.AddResultData("ST_1", "ST1");

			this.ResultTitleInfo.AddResultData("INT_1", "INT1");

			this.ResultTitleInfo.AddResultData("WLP_2", "WLP2");

			this.ResultTitleInfo.AddResultData("WLD_2", "WLD2");

			this.ResultTitleInfo.AddResultData("WLC_2", "WLC2");

			this.ResultTitleInfo.AddResultData("HW_2", "HW2");

			this.ResultTitleInfo.AddResultData("PURITY_2", "PURITY2");

			this.ResultTitleInfo.AddResultData("CIEx_2", "X2");

			this.ResultTitleInfo.AddResultData("CIEy_2", "Y2");

			this.ResultTitleInfo.AddResultData("CIEz_2", "Z2");

            this.ResultTitleInfo.AddResultData("CCT_2", "CCT2");

			this.ResultTitleInfo.AddResultData("ST_2", "ST2");

			this.ResultTitleInfo.AddResultData("INT_2", "INT2");

			this.ResultTitleInfo.AddResultData("WLP_3", "WLP3");

			this.ResultTitleInfo.AddResultData("WLD_3", "WLD3");

			this.ResultTitleInfo.AddResultData("WLC_3", "WLC3");

			this.ResultTitleInfo.AddResultData("HW_3", "HW3");

			this.ResultTitleInfo.AddResultData("PURITY_3", "PURITY3");

			this.ResultTitleInfo.AddResultData("CIEx_3", "X3");

			this.ResultTitleInfo.AddResultData("CIEy_3", "Y3");

			this.ResultTitleInfo.AddResultData("CIEz_3", "Z3");

            this.ResultTitleInfo.AddResultData("CCT_3", "CCT3");

			this.ResultTitleInfo.AddResultData("ST_3", "ST3");

			this.ResultTitleInfo.AddResultData("INT_3", "INT3");

			this.ResultTitleInfo.AddResultData("COL", "PosX");

			this.ResultTitleInfo.AddResultData("ROW", "PosY");

			this.ResultTitleInfo.AddResultData("CHANNEL", "Channel");

			this._dicItemForceValue.Clear();
			this._resultItemInReport.Clear();
			this._resultItemInReport.Add("TEST");
			this._resultItemInReport.Add("BIN");
			this._resultItemInReport.Add("CONTA");
			this._resultItemInReport.Add("CONTC");
			this._resultItemInReport.Add("POLAR");
			this._resultItemInReport.Add("COL");
			this._resultItemInReport.Add("ROW");
			this._resultItemInReport.Add("CHANNEL");

			if (this.Product.TestCondition != null &&
				this.Product.TestCondition.TestItemArray != null &&
				this.Product.TestCondition.TestItemArray.Length > 0)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{
					if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable || testItem.ElecSetting == null)
					{
						continue;
					}

					this._dicItemForceValue.Add(testItem.KeyName, testItem.ElecSetting);

					foreach (var resultItem in testItem.MsrtResult)
					{
						if (!resultItem.IsEnable || !resultItem.IsVision)
						{
							continue;
						}

						this._resultItemInReport.Add(resultItem.KeyName);
					}
				}
			}


            Console.WriteLine("[Report], SetResultTitle(), OK");
		}

		protected override EErrorCode WriteReportHeadByUser()
		{
			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////

			this.WriteLine("FileID,,MPI Data File,id," + this.UISetting.UserID + ",version,1.0.1");

			this.WriteLine("FileName,," + Path.Combine(this.TestResultFileNameWithoutExt(), this.UISetting.TestResultFileExt));

			this.WriteLine("TestStartTime,," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

			this.WriteLine("TesterModel,," + this.MachineInfo.TesterModel);

			this.WriteLine("CommPort,," + "COM:1");

			this.WriteLine("TesterNumber,," + this.UISetting.MachineName);

			this.WriteLine("Specification,," + "_LINE_NO.");

			this.WriteLine("SampleLevel,," + "STANDARD");

			this.WriteLine("SampleStandard,," + "WEIMIN");

			this.WriteLine("TotalTest,,");

			this.WriteLine("Samples,,");

			this.WriteLine("Remark1,," + this.UISetting.TestResultPath01);

			this.WriteLine("Remark2,," + this.UISetting.TestResultPath02);

			this.WriteLine("CustomerNote1,," + "\"" + this.UISetting.WeiminUIData.CustomerNote01 + "\"");

			this.WriteLine("CustomerRemark,," + "\"" + this.UISetting.WeiminUIData.CustomerRemark01 + "\"");

			this.WriteLine("LotNumber,," + this.UISetting.TestResultFileName);

			this.WriteLine("Operator,," + this.UISetting.OperatorName);

			this.WriteLine("MaximumBin,," + "31");

			string lineResultItem = string.Empty;

			lineResultItem += "BIN,";

			lineResultItem += "POLAR,";

			lineResultItem += "ISALLPASS,";

			lineResultItem += "ROW,";

			lineResultItem += "COL,";			

			if (this.Product.TestCondition != null &&
				this.Product.TestCondition.TestItemArray != null &&
				this.Product.TestCondition.TestItemArray.Length > 0)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{
					if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable || testItem.ElecSetting == null)
					{
						continue;
					}

					foreach (var resultItem in testItem.MsrtResult)
					{
						if (!resultItem.IsEnable || !resultItem.IsVision)
						{
							continue;
						}

						if (resultItem.KeyName.Contains("LOP_") || resultItem.KeyName.Contains("LM_") || resultItem.KeyName.Contains("WATT_") || resultItem.KeyName.Contains("PURITY_1") ||
							resultItem.KeyName.Contains("CIEx") || resultItem.KeyName.Contains("CIEy") || resultItem.KeyName.Contains("CIExz") || resultItem.KeyName.Contains("CCT_") ||
							resultItem.KeyName.Contains("ST_") || resultItem.KeyName.Contains("INT_"))
						{
							lineResultItem += resultItem.Name;

							lineResultItem += ",";
						}
						else
						{
							lineResultItem += resultItem.Name;

							lineResultItem += "(" + resultItem.Unit + ")";

							lineResultItem += ",";
						}
					}

				}
			}

			lineResultItem = lineResultItem.TrimEnd(',');

			this.WriteLine("ItemName,," + "\"" + lineResultItem + "\"");

			this.WriteLine("DataFormat,," + "\"" + "0,0,0,,,0.000,0.000,,0.0,0.0,0.0,0.0,0.00,0.0000,0.0000,0.0000,0,0,0,0.000" + "\"");

			this.WriteLine("TestCondition,," + "\"" + "BIN,,,,IF1(mA),IF2(mA),IF3(mA),IF4(mA),IFM1(mA),IFM2(mA),IFP*T,,,IZ1(uA),IZ2(uA),VR(V),IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),,,,,,IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),,,,,,IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),,,,,,IF(mA),IF(mA),IF(mA),," + "\"");

			this.WriteLine("At,," + "\"" + "ALL,,,," + this.SetForceValueTable() + "\"");

			for (int i = 1; i <= 30; i++)
			{
				this.WriteLine("BinAt" + i.ToString() + ",," + "\"" + i.ToString() + ",,,," + this.SetForceValueTable() + "\"");
			}

			this.WriteLine("Target,," + "\"" + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,," + "\"");

			this.WriteLine("LowLimit,," + "\"" + ",,,,0,0,0,0,0,0,0,0,,0,0,0,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,," + "\"");

			this.WriteLine("HighLimit,," + "\"" + ",0,0,,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,," + "\"");

			this.WriteLine("");

			this.WriteLine(this.ResultTitleInfo.TitleStr);

            Console.WriteLine("[Report], WriteReportHeadByUser(), OK");

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string testCount = "TotalTest,," + this.TotalTestCount.ToString();

			replaceData.Add("TotalTest,,", testCount);

			this.ReplaceReport(replaceData);

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
		{
			string line = string.Empty;
			bool isEsdEnable = false;
			int column = 0;
			int ESDJudePass = -1;

			this._zapTable.Clear();
			this._listMIRJ.Clear();

			if (this.Product.TestCondition != null &&
				this.Product.TestCondition.TestItemArray != null &&
				this.Product.TestCondition.TestItemArray.Length > 0)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{
					if (!testItem.IsEnable)
					{
						continue;
					}

					if (testItem is ESDTestItem)
					{
						if ((testItem as ESDTestItem).EsdSetting == null)
						{
							isEsdEnable = false;

							break;
						}

						isEsdEnable = true;

						//foreach (int zapTable in (testItem as ESDTestItem).EsdSetting.TableValue)
						//{
						//    this._zapTable.Add(zapTable);
						//}

						foreach (var ESDItem in (testItem as ESDTestItem).MsrtResult)
						{
							if (ESDItem.KeyName == "JUDGEPASS_1")
							{
								ESDJudePass = (int)data["JUDGEPASS_1"];
							}

							if (ESDItem.KeyName.Contains("MIRJ"))
							{
								this._listMIRJ.Add(ESDItem.Value);
							}
						}
					}
				}
			}

			foreach (var resultItem in this.ResultTitleInfo)
			{
				if (isEsdEnable)
				{
					if (resultItem.Key == "FESDF_1")
					{
						line += this._zapTable[0].ToString();
					}

					if (resultItem.Key == "FESDF_2")
					{
						line += this._zapTable[1].ToString();
					}

					if (resultItem.Key == "MIR_ESD1")
					{
						line += this._listMIRJ[0].ToString();
					}

					if (resultItem.Key == "MIR_ESD2")
					{
						line += this._listMIRJ[1].ToString();
					}

					if (resultItem.Key == "MESDF_1")
					{
						if (ESDJudePass == 0)
						{
							line += "0";
						}
						else
						{
							line += "1";
						}
					}

					if (resultItem.Key == "MESDF_2")
					{
						if (ESDJudePass == 99)
						{
							line += "1";
						}
						else
						{
							line += "0";
						}
					}
				}
				else
				{
					if (resultItem.Key == "FESDF_1" || resultItem.Key == "FESDF_2" ||
						resultItem.Key == "MIR_ESD1" || resultItem.Key == "MIR_ESD2" ||
						resultItem.Key == "MESDF_1" || resultItem.Key == "MESDF_2")
					{
						line += "";
					}
				}

				if (data.ContainsKey(resultItem.Key) && this._resultItemInReport.Contains(resultItem.Key))
				{
					string format = string.Empty;

					if (this.ResultData.ContainsKey(resultItem.Key))
					{
						format = this.ResultData[resultItem.Key].Formate;
					}

					line += data[resultItem.Key].ToString(format);
				}
				else
				{
					if (this._dicItemNullValue.ContainsKey(resultItem.Key))
					{
						line += this._dicItemNullValue[resultItem.Key];
					}
				}

				column++;

				if (column != this.ResultTitleInfo.ResultCount)
				{
					line += ",";
				}
			}

			this.WriteLine(line);

			return EErrorCode.NONE;
		}

		#endregion

		#region >>> Public Override Method <<<

		public override string TestResultFileNameWithoutExt()
		{
			//機台號碼_工單號碼_生產日期_LOT NUMBER_IN BIN良率_SIDE BIN良率_NG BIN良率
			string fileNameWithoutExt = this.UISetting.Barcode;

			//fileNameWithoutExt += "_" + this.UISetting.Barcode;

			//fileNameWithoutExt += "_#" + this.TesterSetting.EndTestTime.ToString("yyyyMMddHHmmss") + "#";

			//fileNameWithoutExt += "_" + this.UISetting.LotNumber;

			//fileNameWithoutExt += "_" + this.SmartBinning.InBinRate.ToString("0.00");

			//fileNameWithoutExt += "_" + this.SmartBinning.SideBinRate.ToString("0.00");

			//fileNameWithoutExt += "_" + this.SmartBinning.NGBinRate.ToString("0.00");

			return fileNameWithoutExt;
		}

		#endregion
	}
}
