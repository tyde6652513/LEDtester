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

namespace MPI.Tester.Report.User.Lumitek
{
	class Report : ReportBase
	{
		private List<string[]> _WLDTable = new List<string[]>();
		private Dictionary<int, double[]> _WL1Table = new Dictionary<int, double[]>();
		private Dictionary<int, double[]> _WL2Table = new Dictionary<int, double[]>();
		private Dictionary<int, double[]> _WL3Table = new Dictionary<int, double[]>();
		private Dictionary<int, double[]> _WL4Table = new Dictionary<int, double[]>();
		private Dictionary<string, ElectSettingData[]> _dicItemForceValue = new Dictionary<string, ElectSettingData[]>();
		private List<string> _sysResultKsyName = new List<string>();

		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
			this._sysResultKsyName.AddRange(Enum.GetNames(typeof(ESysResultItem)));

			this._sysResultKsyName.AddRange(Enum.GetNames(typeof(EProberDataIndex)));
		}

		private string GetItemForceValue(string keyName)
		{
			double forceValue;
			ElectSettingData[] electSettingData = new ElectSettingData[this._dicItemForceValue.Count];

			if (this._dicItemForceValue.TryGetValue(keyName, out electSettingData))
			{
				forceValue = Math.Abs(electSettingData[0].ForceValue);

				return forceValue.ToString();
			}
			else
			{
				foreach (var item in this._dicItemForceValue)
				{
					foreach (var elec in item.Value)
					{
						if (elec.KeyName == keyName)
						{
							forceValue = Math.Abs(elec.ForceValue);

							return forceValue.ToString();
						}
					}
				}

				return "0";
			}

		}

		private string GetItemForceTime(string keyName)
		{
			double forceTime;
			ElectSettingData[] electSettingData = new ElectSettingData[this._dicItemForceValue.Count];

			if (!this._dicItemForceValue.TryGetValue(keyName, out electSettingData))
			{
				return "0";
			}
			else
			{
				forceTime = electSettingData[0].ForceTime;

				return forceTime.ToString();
			}

		}

		private string SetForceValueTable()
		{
			string lineForceValueTable = string.Empty;

			if (this.UISetting.FormatName == "Format-A")
			{
				lineForceValueTable += "@" + this.GetItemForceValue("IF_1") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IF_2") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IF_3") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IF_4") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("THYIF_1") + ",@0,@0*0,,,";
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
			}
			else if (this.UISetting.FormatName == "Format-B")
			{
				lineForceValueTable += "@" + this.GetItemForceValue("IF_1") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IF_2") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IF_3") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IF_4") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IFMA_1") + ",@0,@0*0,,,";
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
			}
			else if (this.UISetting.FormatName == "Format-C")
			{
				lineForceValueTable += "@" + this.GetItemForceValue("IF_1") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IF_2") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("THYIF_1") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IFMB_1") + ",@0*0,,,";
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
			}
			else if (this.UISetting.FormatName == "Format-D")
			{
				lineForceValueTable += "@" + this.GetItemForceValue("IF_1") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IF_2") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_2") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IFWLA_1") + ",";
				lineForceValueTable += "@" + this.GetItemForceValue("IFMA_1") + ",@0,@0*0,,,";
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
			}

			return lineForceValueTable;
		}

		private void WriteProductInfoFile()
		{
			DateTime dt = this.TesterSetting.StartTestTime;

			string saveReportLocalPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "Date" + dt.Day.ToString("00") + ".CSV");

			string saveReportServerPath = Path.Combine(this.UISetting.MESPath2, "Date" + dt.Day.ToString("00") + ".CSV");

			List<string[]> file = new List<string[]>();

			bool isCreateNewFile = false;

			if (File.Exists(saveReportLocalPath))
			{
				file = CSVUtil.ReadCSV(saveReportLocalPath);

				if (file.Count > 1 && file[0].Length == 2)
				{
					if (file[0][1] != dt.ToString("yyyy/MM/dd"))
					{
						file.Clear();

						isCreateNewFile = true;
					}
				}
			}
			else
			{
				isCreateNewFile = true;
			}

			if (isCreateNewFile)
			{
				MPIFile.DeleteFile(saveReportLocalPath);

				string[] dateTitle = new string[2] { "Date", dt.ToString("yyyy/MM/dd") };

				string[] title = new string[6] { "Time", "Machine", "Wafer ID", "State", "Total Dies", "Tester Spec" };

				file.Add(dateTitle);

				file.Add(title);
			}

			string[] data = new string[6] { dt.ToString("hh:mm:ss"), this.UISetting.MachineName, this.UISetting.WaferNumber, "OpenFile", this.UISetting.TotalSacnCounts.ToString(), this.UISetting.ProductFileName };

			file.Add(data);

			if (!CSVUtil.WriteCSV(saveReportLocalPath, file))
			{
				Console.WriteLine("[ProduceInfo], Lumitek(), Fail, Path:" + saveReportLocalPath);
			}

			if (!MPIFile.CopyFile(saveReportLocalPath, saveReportServerPath))
			{
				Console.WriteLine("[ProduceInfo], Lumitek(), Fail, Path:" + saveReportServerPath);
			}
		}

		private void CloseProductInfoFile()
		{
			DateTime dt = DateTime.Now;

			string saveReportLocalPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "Date" + dt.Day.ToString("00") + ".CSV");

			string saveReportServerPath = Path.Combine(this.UISetting.MESPath2, "Date" + dt.Day.ToString("00") + ".CSV");

			List<string[]> file = new List<string[]>();

			bool isCreateNewFile = false;

			if (File.Exists(saveReportLocalPath))
			{
				file = CSVUtil.ReadCSV(saveReportLocalPath);

				if (file.Count > 1 && file[0].Length > 2)
				{
					if (file[0][2] != dt.ToString("yyyy/MM/dd"))
					{
						file.Clear();

						isCreateNewFile = true;
					}
				}
			}
			else
			{
				isCreateNewFile = true;
			}

			if (isCreateNewFile)
			{
				MPIFile.DeleteFile(saveReportLocalPath);

				string[] dateTitle = new string[2] { "Date", dt.ToString("yyyy/MM/dd") };

				string[] title = new string[6] { "Time", "Machine", "Wafer ID", "State", "Total Dies", "Tester Spec" };

				file.Add(dateTitle);

				file.Add(title);
			}

			string[] data = new string[6] { dt.ToString("hh:mm:ss"), this.UISetting.MachineName, this.UISetting.WaferNumber + ".CSV", "CloseFile", Report.TestCount.ToString(), this.UISetting.ProductFileName };

			file.Add(data);

			if (!CSVUtil.WriteCSV(saveReportLocalPath, file))
			{
				Console.WriteLine("[ProduceInfo], Lumitek(), Fail, Path:" + saveReportLocalPath);
			}

			if (!MPIFile.CopyFile(saveReportLocalPath, saveReportServerPath))
			{
				Console.WriteLine("[ProduceInfo], Lumitek(), Fail:, Path:" + saveReportServerPath);
			}
		}

		#region >>> Protected Override Method <<<

		protected override void SetResultTitle()
		{
			this.ResultTitleInfo.Clear();

			foreach (var resultItem in this.UISetting.UserDefinedData.ResultItemNameDic)
			{
				//if (resultItem.Key == "CHANNEL")
				//{
				//    continue;
				//}

				if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
				{
					if (resultItem.Key.Contains("WATT_") || resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
				{
					if (resultItem.Key.Contains("LOP_") || resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
				{
					if (resultItem.Key.Contains("LOP_") || resultItem.Key.Contains("WATT_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_lm)
				{
					if (resultItem.Key.Contains("WATT_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_watt)
				{
					if (resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.watt_lm)
				{
					if (resultItem.Key.Contains("LOP_"))
					{
						continue;
					}
				}

				this.ResultTitleInfo.AddResultData(resultItem.Key, resultItem.Value);
			}
		}

		protected override EErrorCode WriteReportHeadByUser()
		{
			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////                       

			this.WriteLine("FileID,,\"WEI MIN Data File\"");

			this.WriteLine("FileName,,\"" + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt + "\"");

			this.WriteLine("TestTime,,\"" + this.TesterSetting.StartTestTime.ToString("yy/MM/dd HH:mm:ss") + "\"");

			this.WriteLine("TesterModel,,\"" + this.MachineInfo.TesterModel + "/" + this.MachineInfo.TesterSN + "\"");

			this.WriteLine("Commport,,\"SOFTWARE-PORT\"");

			this.WriteLine("TesterNumber,,\"" + this.UISetting.MachineName + "\"");

			this.WriteLine("Sepecification,,\"_LINE_NO.\"");

			this.WriteLine("SampleBins,,\"ALL\"");

			this.WriteLine("SampleStandard,,\"MPI\"");

			this.WriteLine("SampleLevel,,\"STANDARD\"");

			this.WriteLine("TotalTested,,\"1000000\"");

			this.WriteLine("Samples,,\"\"");

			this.WriteLine("CustomerNote1,,\"SpecName:" + this.UISetting.ProductFileName + "/FixLopByWLSpec:1,2,63,"
							+ this.UISetting.ProductFileName + ",pitchx=357,353,358,362,357,pitchy=351,353,353,354,354" + "\"");

			string model = string.Empty;
			string votage = string.Empty;
			string times = string.Empty;
			string interval = string.Empty;
			string polarity = string.Empty;

			string strCustomerNote2 = string.Empty;

			bool isEsdEnable = false;

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

						polarity = (testItem as ESDTestItem).EsdSetting.Polarity.ToString();

                        votage = (testItem as ESDTestItem).EsdSetting.ZapVoltage.ToString();

						times = (testItem as ESDTestItem).EsdSetting.Count.ToString();

						interval = (testItem as ESDTestItem).EsdSetting.IntervalTime.ToString();

						model = (testItem as ESDTestItem).EsdSetting.Mode.ToString();

						break;
					}
				}
			}

			if (isEsdEnable)
			{
				if (polarity == "P")
				{
					strCustomerNote2 += "ESD :P->N(MODEL=";

					if (model == "HBM")
					{
						strCustomerNote2 += "1";
					}
					else if (model == "MM")
					{
						strCustomerNote2 += "0";
					}

					strCustomerNote2 += ",VOLTAGE=";
					strCustomerNote2 += votage;
					strCustomerNote2 += ",TIMES=";
					strCustomerNote2 += times;
					strCustomerNote2 += ",INTERVAL";
					strCustomerNote2 += interval;
					strCustomerNote2 += ") N->P(MODEL=0,VOLTAGE=0,TIMES=0,INTERVAL=0)";
				}
				else if (polarity == "N")
				{
					strCustomerNote2 += "ESD :P->N(MODEL=0,VOLTAGE=0,TIMES=0,INTERVAL=0)";

					if (model == "HBM")
					{
						strCustomerNote2 += "1";
					}
					else if (model == "MM")
					{
						strCustomerNote2 += "0";
					}

					strCustomerNote2 += ",VOLTAGE=";
					strCustomerNote2 += votage;
					strCustomerNote2 += ",TIMES=";
					strCustomerNote2 += times;
					strCustomerNote2 += ",INTERVAL";
					strCustomerNote2 += interval;
					strCustomerNote2 += ")";
				}
			}

			this.WriteLine("CustomerNote2,,\"" + strCustomerNote2 + "\"");

			string lineLOP1 = "0,0,0,";
			string lineLOP2 = "0,0,0,";
			string lineLOP3 = "0,0,0,";

			string lineWLD1Offset = "0,";
			string lineWLD2Offset = "0,";
			string lineWLD3Offset = "0,";

			string lineWLP1Offset = "0,";
			string lineWLP2Offset = "0,";
			string lineWLP3Offset = "0,";

			string lineWLC1Offset = "0,";
			string lineWLC2Offset = "0,";

			//WLC3 is the end, no ","
			string lineWLC3Offset = "0";

			string lineVF1Offset = "0,";
			string lineVF2Offset = "0,";
			string lineVF3Offset = "0,";
			string lineVF4Offset = "0,";

			string lineVZ1Offset = "0,";
			string lineVZ2Offset = "0,";

			string lineIR1Offset = "0,";

			string lineResultItem = "";
			string lineResultItemFormat = "";

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

					if (testItem.KeyName.Contains("LOPWL_"))
					{
						for (int i = 0; i < testItem.GainOffsetSetting.Length; i++)
						{
							if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
							{
								if (testItem.GainOffsetSetting[i].KeyName == "LOP_1")
								{
									lineLOP1 = testItem.GainOffsetSetting[i].Square.ToString("0.0000") + ",";

                                    lineLOP1 += testItem.GainOffsetSetting[i].Gain.ToString("0.0000") + ",";

                                    lineLOP1 += testItem.GainOffsetSetting[i].Offset.ToString("0.0000") + ",";
								}
								else if (testItem.GainOffsetSetting[i].KeyName == "LOP_2")
								{
                                    lineLOP2 = testItem.GainOffsetSetting[i].Square.ToString("0.0000") + ",";

                                    lineLOP2 += testItem.GainOffsetSetting[i].Gain.ToString("0.0000") + ",";

                                    lineLOP2 += testItem.GainOffsetSetting[i].Offset.ToString("0.0000") + ",";
								}
								else if (testItem.GainOffsetSetting[i].KeyName == "LOP_3")
								{
                                    lineLOP3 = testItem.GainOffsetSetting[i].Square.ToString("0.0000") + ",";

                                    lineLOP3 += testItem.GainOffsetSetting[i].Gain.ToString("0.0000") + ",";

                                    lineLOP3 += testItem.GainOffsetSetting[i].Offset.ToString("0.0000") + ",";
								}
							}
							else
							{
								if (testItem.GainOffsetSetting[i].KeyName == "WATT_1")
								{
                                    lineLOP1 = testItem.GainOffsetSetting[i].Square.ToString("0.0000") + ",";

                                    lineLOP1 += testItem.GainOffsetSetting[i].Gain.ToString("0.0000") + ",";

                                    lineLOP1 += testItem.GainOffsetSetting[i].Offset.ToString("0.0000") + ",";
								}
								else if (testItem.GainOffsetSetting[i].KeyName == "WATT_2")
								{
                                    lineLOP2 = testItem.GainOffsetSetting[i].Square.ToString("0.0000") + ",";

                                    lineLOP2 += testItem.GainOffsetSetting[i].Gain.ToString("0.0000") + ",";

                                    lineLOP2 += testItem.GainOffsetSetting[i].Offset.ToString("0.0000") + ",";
								}
								else if (testItem.GainOffsetSetting[i].KeyName == "WATT_3")
								{
                                    lineLOP3 = testItem.GainOffsetSetting[i].Square.ToString("0.0000") + ",";

                                    lineLOP3 += testItem.GainOffsetSetting[i].Gain.ToString("0.0000") + ",";

                                    lineLOP3 += testItem.GainOffsetSetting[i].Offset.ToString("0.0000") + ",";
								}

							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLD_1")
							{
								lineWLD1Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLP_1")
							{
								lineWLP1Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLP_2")
							{
								lineWLP2Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLP_3")
							{
								lineWLP3Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLC_1")
							{
								lineWLC1Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLC_2")
							{
								lineWLC2Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLD_1")
							{
								lineWLD1Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLD_2")
							{
								lineWLD2Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLD_3")
							{
								lineWLD3Offset = testItem.GainOffsetSetting[i].Offset.ToString() + ",";
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLC_3")
							{
								lineWLC3Offset = testItem.GainOffsetSetting[i].Offset.ToString();
							}
						}
					}

					if (testItem.GainOffsetSetting[0].KeyName == "MVF_1")
					{
						lineVF1Offset = testItem.GainOffsetSetting[0].Offset.ToString() + ",";
					}

					if (testItem.GainOffsetSetting[0].KeyName == "MVF_2")
					{
						lineVF2Offset = testItem.GainOffsetSetting[0].Offset.ToString() + ",";
					}

					if (testItem.GainOffsetSetting[0].KeyName == "MVF_3")
					{
						lineVF3Offset = testItem.GainOffsetSetting[0].Offset.ToString() + ",";
					}

					if (testItem.GainOffsetSetting[0].KeyName == "MVF_4")
					{
						lineVF4Offset = testItem.GainOffsetSetting[0].Offset.ToString() + ",";
					}

					if (testItem.GainOffsetSetting[0].KeyName == "MVZ_1")
					{
						lineVZ1Offset = testItem.GainOffsetSetting[0].Offset.ToString() + ",";
					}

					if (testItem.GainOffsetSetting[0].KeyName == "MVZ_2")
					{
						lineVZ2Offset = testItem.GainOffsetSetting[0].Offset.ToString() + ",";
					}

					if (testItem.GainOffsetSetting[0].KeyName == "MIR_1")
					{
						lineIR1Offset = testItem.GainOffsetSetting[0].Offset.ToString() + ",";
					}					
				}
			}

			if (this.UISetting.FormatName == "Format-C" || this.UISetting.FormatName == "Format-C"
			|| this.UISetting.FormatName == "Format-D" || this.UISetting.FormatName == "Format-D")
			{
				lineVF3Offset = "0,";

				lineVF4Offset = "0,";

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

						if (testItem.KeyName == "LOPWL_2")
						{
							lineVF3Offset = testItem.GainOffsetSetting[(int)EOptiMsrtType.MVFLA].Offset.ToString() + ",";
						}

						if (testItem.KeyName == "LOPWL_1")
						{
							lineVF4Offset = testItem.GainOffsetSetting[(int)EOptiMsrtType.MVFLA].Offset.ToString() + ",";
						}
					}
				}
			}

			foreach (var resultItem in this.ResultTitleInfo)
			{
				if (resultItem.Key == "TEST" || resultItem.Key == "CHANNEL")
				{
					continue;
				}

				if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
				{
					if (resultItem.Key.Contains("WATT_") || resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
				{
					if (resultItem.Key.Contains("LOP_") || resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
				{
					if (resultItem.Key.Contains("LOP_") || resultItem.Key.Contains("WATT_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_lm)
				{
					if (resultItem.Key.Contains("WATT_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_watt)
				{
					if (resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.watt_lm)
				{
					if (resultItem.Key.Contains("LOP_"))
					{
						continue;
					}
				}

				if (resultItem.Key.Contains("BIN") || resultItem.Key.Contains("CONTA") || resultItem.Key.Contains("CONTC") ||
					resultItem.Key.Contains("POLAR") || resultItem.Key.Contains("LOP_") || resultItem.Key.Contains("WATT_") ||
					resultItem.Key.Contains("LM_") || resultItem.Key.Contains("PURITY_") || resultItem.Key.Contains("CIEx_") ||
					resultItem.Key.Contains("CIEy_") || resultItem.Key.Contains("CIEz_") || resultItem.Key.Contains("ST_") ||
					resultItem.Key.Contains("CCT_") || resultItem.Key.Contains("INT_") || resultItem.Key.Contains("ROW") || resultItem.Key.Contains("COL"))
				{
					lineResultItem += resultItem.Value;

					lineResultItem += ",";
				}
				else
				{
					lineResultItem += resultItem.Value;

					lineResultItem += "(" + this.UISetting.UserDefinedData[resultItem.Key].Unit + ")";

					lineResultItem += ",";
				}
			}

			foreach (var resultItem in this.ResultTitleInfo)
			{
				if (resultItem.Key == "TEST" || resultItem.Key == "CHANNEL")
				{
					continue;
				}

				if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
				{
					if (resultItem.Key.Contains("WATT_") || resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
				{
					if (resultItem.Key.Contains("LOP_") || resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
				{
					if (resultItem.Key.Contains("LOP_") || resultItem.Key.Contains("WATT_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_lm)
				{
					if (resultItem.Key.Contains("WATT_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_watt)
				{
					if (resultItem.Key.Contains("LM_"))
					{
						continue;
					}
				}
				else if (this.Product.LOPSaveItem == ELOPSaveItem.watt_lm)
				{
					if (resultItem.Key.Contains("LOP_"))
					{
						continue;
					}
				}

				if (resultItem.Key == "IR_1" ||
					resultItem.Key == "LOP_1" || resultItem.Key == "LOP_2" || resultItem.Key == "LOP_3" ||
					resultItem.Key == "WATT_1" || resultItem.Key == "WATT_2" || resultItem.Key == "WATT_3" ||
					resultItem.Key == "LM_1" || resultItem.Key == "LM_2" || resultItem.Key == "LM_3" ||
					resultItem.Key == "ROW" || resultItem.Key == "COL")
				{
					lineResultItemFormat += ",";
				}
				else
				{
					lineResultItemFormat += this.UISetting.UserDefinedData[resultItem.Key].Formate;

					lineResultItemFormat += ",";
				}
			}

			if (lineResultItem[lineResultItem.Length - 1] == ',')
			{
				lineResultItem = lineResultItem.Remove(lineResultItem.Length - 1);
			}

			if (lineResultItemFormat[lineResultItemFormat.Length - 1] == ',')
			{
				lineResultItemFormat = lineResultItemFormat.Remove(lineResultItemFormat.Length - 1);
			}

			this.WriteLine("CustomerRemark,,\"" + lineLOP1 + lineLOP2 + lineLOP3 
												+ lineVF1Offset + lineVF2Offset + lineVF3Offset + lineVF4Offset 
												+ lineVZ1Offset + lineVZ2Offset + lineIR1Offset 
												+ lineWLP1Offset + lineWLD1Offset + lineWLC1Offset 
												+ lineWLP2Offset + lineWLD2Offset + lineWLC2Offset 
												+ lineWLP3Offset + lineWLD3Offset + lineWLC3Offset + ",0,0,0,0,0,0\"");

			this.WriteLine("Operator,,\"" + this.UISetting.OperatorName + "\"");

			this.WriteLine("MaximumBin,,\"31\"");

			this.WriteLine("ItemName,,\"" + lineResultItem + "\"");

			this.WriteLine("DataFormat,,\"" + lineResultItemFormat + "\"");

			this.WriteLine("TestCondition,,\"" 
							+ "BIN,,,,IF1(mA),IF2(mA),IF3(mA),IF4(mA),IFM1(mA),IFM2(mA),IFP*T,,,IZ1(uA),IZ2(uA),VR(V),"
							+ "IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),,,,,,"
							+ "IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),,,,,,"
							+ "IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),IF(mA),,,,,,IF(mA),IF(mA),IF(mA),,\"");

			this.WriteLine("At,,\"" + "ALL,,,," + this.SetForceValueTable() + "\"");

			for (int i = 1; i <= 30; i++)
			{
				this.WriteLine("BinAt" + i.ToString() + ",," + "\"" + i.ToString() + ",,,," + this.SetForceValueTable() + "\"");
			}

			this.WriteLine("Target,,\"5024.79168,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,\"");

			this.WriteLine("LowLimit,,\",,,,2.8,0,2.1,0,0,0,0,0,,15,0,0,30,0,0,440,440,440,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,,\"");

			this.WriteLine("HighLimit,,\",0,0,,3.6,0,2.5,0,0,0,0,0,0,100,0,0.2,200,0,0,480,480,480,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,,\"");

			this.WriteLine("");

			this.WriteLine(this.ResultTitleInfo.TitleStr);

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string testCount = "Samples,,\"" + ReportProcess.TestCount.ToString() + "\"";

			replaceData.Add("Samples,,\"\"", testCount);

			this.ReplaceReport(replaceData);

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
		{
			string line = string.Empty;
			int column = 0;

			foreach (var resultItem in this.ResultTitleInfo)
			{
				string format = this.UISetting.UserDefinedData[resultItem.Key].Formate;

				if (resultItem.Key == "CONTA")
				{
					line += data["CHANNEL"].ToString(format);
				}
				else if (resultItem.Key == "BIN")
				{
					line += "1";
				}
				else if (data.ContainsKey(resultItem.Key))
				{
					if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_watt && resultItem.Key == "CCT_1")
					{
						line += data["WATT_1"].ToString(format);
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_watt && resultItem.Key == "CCT_2")
					{
						line += data["WATT_2"].ToString(format);
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd_watt && resultItem.Key == "CCT_3")
					{
						line += data["WATT_3"].ToString(format);
					}
					else if (this._sysResultKsyName.Contains(resultItem.Key) || this.ResultData.ContainsKey(resultItem.Key))
					{
						line += data[resultItem.Key].ToString(format);
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

		protected override EErrorCode RunCommandByUser2(EServerQueryCmd cmd)
		{
			switch (cmd)
			{
				case EServerQueryCmd.CMD_TESTER_START:
					{
						this.WriteProductInfoFile();

						break;
					}
				case EServerQueryCmd.CMD_TESTER_END:
				case EServerQueryCmd.CMD_TESTER_ABORT:
					{
						this.CloseProductInfoFile();

						break;
					}
				default:
					{
						break;
					}
			}

			return EErrorCode.NONE;
		}

		#endregion

		#region >>> Public Override Method <<<

		public override string TestResultFileNameWithoutExt()
		{
			string fileNameWithoutExt = this.UISetting.Barcode;

			return fileNameWithoutExt;
		}

		#endregion
	}
}