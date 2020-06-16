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

namespace MPI.Tester.Report.User.HP000
{
	class Report : ReportBase
	{
		private List<string[]> _WLDTable = new List<string[]>();

		private Dictionary<int, double[]> _WL1Table = new Dictionary<int, double[]>();
		private Dictionary<int, double[]> _WL2Table = new Dictionary<int, double[]>();
		private Dictionary<int, double[]> _WL3Table = new Dictionary<int, double[]>();
		private Dictionary<int, double[]> _WL4Table = new Dictionary<int, double[]>();

		private List<int> _zapTable = new List<int>();
		private List<double> _listMIRJ = new List<double>();



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

		#region >>> Protected Override Method <<<

		protected override void SetResultTitle()
		{
			// Insert result header//

			this.ResultTitleInfo.Clear();

			this.ResultTitleInfo.AddResultData("TEST", "TEST");

			this.ResultTitleInfo.AddResultData("BIN", "BIN");

			this.ResultTitleInfo.AddResultData("MVF_1", "VF1");

			this.ResultTitleInfo.AddResultData("MVF_2", "VF2");

			this.ResultTitleInfo.AddResultData("MVF_3", "VF3");

			this.ResultTitleInfo.AddResultData("MVF_4", "VF4");

			this.ResultTitleInfo.AddResultData("MVFMA_1", "VFM1");

			this.ResultTitleInfo.AddResultData("MVFMC_1", "VFM2");

			this.ResultTitleInfo.AddResultData("MVFMD_1", "DVF");

			this.ResultTitleInfo.AddResultData("MTHYVP_1", "VF");

			this.ResultTitleInfo.AddResultData("MTHYVD_1", "VFD");

			this.ResultTitleInfo.AddResultData("MVZ_1", "VZ1");

			this.ResultTitleInfo.AddResultData("MVZ_2", "VZ_2");

			this.ResultTitleInfo.AddResultData("MIR_1", "IR");


			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				this.ResultTitleInfo.AddResultData("LOP_1", "LOP1");

				this.ResultTitleInfo.AddResultData("LOP_2", "LOP2");

				this.ResultTitleInfo.AddResultData("LOP_3", "LOP3");
			}
			else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
			{
				this.ResultTitleInfo.AddResultData("LM_1", "LOP1");

				this.ResultTitleInfo.AddResultData("LM_2", "LOP2");

				this.ResultTitleInfo.AddResultData("LM_3", "LOP3");
			}
			else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
			{
				this.ResultTitleInfo.AddResultData("WATT_1", "LOP1");

				this.ResultTitleInfo.AddResultData("WATT_2", "LOP2");

				this.ResultTitleInfo.AddResultData("WATT_3", "LOP3");
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

			this.ResultTitleInfo.AddResultData("HW_1", "HW");

			this.ResultTitleInfo.AddResultData("PURITY_1", "PURITY1");

			this.ResultTitleInfo.AddResultData("CIEx_1", "X1");

			this.ResultTitleInfo.AddResultData("CIEy_1", "Y1");

			this.ResultTitleInfo.AddResultData("CIEz_1", "Z1");

			this.ResultTitleInfo.AddResultData("ST_1", "ST1");

			this.ResultTitleInfo.AddResultData("INT_1", "INT1");

			this.ResultTitleInfo.AddResultData("WLP_2", "WLP2");

			this.ResultTitleInfo.AddResultData("WLD_2", "WLD2");

			this.ResultTitleInfo.AddResultData("WLC_2", "WLC2");

			this.ResultTitleInfo.AddResultData("HW_2", "HW");

			this.ResultTitleInfo.AddResultData("PURITY_2", "PURITY2");

			this.ResultTitleInfo.AddResultData("CIEx_2", "X2");

			this.ResultTitleInfo.AddResultData("CIEy_2", "Y2");

			this.ResultTitleInfo.AddResultData("CIEz_2", "Z2");

			this.ResultTitleInfo.AddResultData("ST_2", "ST2");

			this.ResultTitleInfo.AddResultData("INT_2", "INT2");

			this.ResultTitleInfo.AddResultData("WLP_3", "WLP3");

			this.ResultTitleInfo.AddResultData("WLD_3", "WLD3");

			this.ResultTitleInfo.AddResultData("WLC_3", "WLC3");

			this.ResultTitleInfo.AddResultData("HW_3", "HW");

			this.ResultTitleInfo.AddResultData("PURITY_3", "PURITY3");

			this.ResultTitleInfo.AddResultData("CIEx_3", "X3");

			this.ResultTitleInfo.AddResultData("CIEy_3", "Y3");

			this.ResultTitleInfo.AddResultData("CIEz_3", "Z3");

			this.ResultTitleInfo.AddResultData("ST_3", "ST3");

			this.ResultTitleInfo.AddResultData("INT_3", "INT3");

			this.ResultTitleInfo.AddResultData("MVF_6", "IV");

			this.ResultTitleInfo.AddResultData("FESDF_1", "ESD1");

			this.ResultTitleInfo.AddResultData("FESDF_2", "ESD2");

			this.ResultTitleInfo.AddResultData("MIRJ_1", "IR1");

			this.ResultTitleInfo.AddResultData("MIRJ_2", "IR2");

			this.ResultTitleInfo.AddResultData("MESDF_1", "ESD1PASS");

			this.ResultTitleInfo.AddResultData("MESDF_2", "ESD2PASS");

			this.ResultTitleInfo.AddResultData("COL", "PosX");

			this.ResultTitleInfo.AddResultData("ROW", "PosY");

			// Format 2  Title 

			this.ResultTitleInfo2.Clear();

			this.ResultTitleInfo2.AddResultData("COL", "PosX");

			this.ResultTitleInfo2.AddResultData("ROW", "PosY");

			this.ResultTitleInfo2.AddResultData("BIN", "BIN");

			this.ResultTitleInfo2.AddResultData("MIR_1", "IR1");

			this.ResultTitleInfo2.AddResultData("MVZ_1", "VZ1");

			this.ResultTitleInfo2.AddResultData("MVF_2", "VF2");

			this.ResultTitleInfo2.AddResultData("MVF_1", "VF1");

			if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
			{
				this.ResultTitleInfo2.AddResultData("LOP_1", "LOP1");
			}
			else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
			{
				this.ResultTitleInfo2.AddResultData("LM_1", "LOP1");
			}
			else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
			{
				this.ResultTitleInfo2.AddResultData("WATT_1", "LOP1");
			}
			else
			{
				this.ResultTitleInfo2.AddResultData("LOP_1", "LOP1");
			}

			this.ResultTitleInfo2.AddResultData("WLD_1", "WLD1");

			this.ResultTitleInfo2.AddResultData("WLP_1", "WLP1");

			this.ResultTitleInfo2.AddResultData("HW_1", "HW");

			this.ResultTitleInfo2.AddResultData("CIEx_1", "X1");

			this.ResultTitleInfo2.AddResultData("CIEy_1", "Y1");

			this.ResultTitleInfo2.AddResultData("PURITY_1", "PURITY1");

			this.ResultTitleInfo2.AddResultData("INT_1", "INTENS");

		}

		protected override EErrorCode WriteReportHeadByUser()
		{
			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////

			this.WriteLine("FileName,," + Path.Combine(this.TestResultFileNameWithoutExt(), this.UISetting.TestResultFileExt));

			this.WriteLine("LotNumber,," + this.UISetting.LotNumber);

			this.WriteLine("Model,," + this.MachineInfo.TesterModel);

			this.WriteLine("TestStartTime,," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

			this.WriteLine("TestTime,," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

			this.WriteLine("TesterModel,," + this.MachineInfo.TesterModel);

			this.WriteLine("TesterNumber,," + this.UISetting.MachineName);

			string lineLOPGain = "";
			string lineWLDOffset = "";
			string lineVF1Offset = "";

			if (this.Product.TestCondition != null &&
				this.Product.TestCondition.TestItemArray != null &&
				this.Product.TestCondition.TestItemArray.Length > 0)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{
					if (testItem.KeyName == "LOPWL_1")
					{
						for (int i = 0; i < testItem.GainOffsetSetting.Length; i++)
						{
							if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
							{
								if (testItem.GainOffsetSetting[i].KeyName == "LOP_1")
								{
									lineLOPGain = testItem.GainOffsetSetting[i].Gain.ToString();
								}
							}
							else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
							{
								if (testItem.GainOffsetSetting[i].KeyName == "LM_1")
								{
									lineLOPGain = testItem.GainOffsetSetting[i].Gain.ToString();
								}
							}
							else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
							{
								if (testItem.GainOffsetSetting[i].KeyName == "WATT_1")
								{
									lineLOPGain = testItem.GainOffsetSetting[i].Gain.ToString();
								}
							}

							if (testItem.GainOffsetSetting[i].KeyName == "WLD_1")
							{
								lineWLDOffset = testItem.GainOffsetSetting[i].Offset.ToString();
							}
						}
					}

					if (testItem.KeyName == "IF_1")
					{
						lineVF1Offset = testItem.GainOffsetSetting[0].Offset.ToString();
					}

				}
			}

			this.WriteLine("LOPGain,," + lineLOPGain);

			this.WriteLine("WLDOffset,," + lineWLDOffset);

			this.WriteLine("VF1Offset,," + lineVF1Offset);

			this.WriteLine("TotalTest,,");

			this.WriteLine("Operator,," + this.UISetting.OperatorName);

			this.WriteLine("MaximumBin,,");

			this.WriteLine("");

			////////////////////////////////////////////
			//Write Result Item Title
			////////////////////////////////////////////

			this.WriteLine(this.ResultTitleInfo.TitleStr);

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string testCount = "TotalTest,," + ReportProcess.TestCount.ToString();

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

				if (data.ContainsKey(resultItem.Key))
				{
					string format = string.Empty;

					if (this.ResultData.ContainsKey(resultItem.Key))
					{
						format = this.ResultData[resultItem.Key].Formate;

						//line += data[resultItem.Key].ToString(format);
					}

					line += data[resultItem.Key].ToString(format);
				}

				column++;

				if (column != this.ResultTitleInfo3.ResultCount)
				{
					line += ",";
				}
			}

			this.WriteLine(line);

			return EErrorCode.NONE;
		}

		protected override EErrorCode WriteReportHeadByUser2()
		{
			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////

			this.WriteLine2("FileName,," + Path.Combine(this.TestResultFileNameWithoutExt(), this.UISetting.TestResultFileExt));

			this.WriteLine2("TestTime,," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

			this.WriteLine2("TesterModel,," + this.MachineInfo.TesterModel);

			this.WriteLine2("TesterNumber,," + this.UISetting.MachineName);

			this.WriteLine2("DeviceNumber,," + this.UISetting.WeiminUIData.DeviceNumber);

			this.WriteLine2("Specification,," + this.UISetting.WeiminUIData.Specification);

			this.WriteLine2("SpecRemark,," + this.UISetting.WeiminUIData.SpecificationRemark);

			this.WriteLine2("LotNumber,," + this.UISetting.LotNumber);

			this.WriteLine2("Operator,," + this.UISetting.OperatorName);

			this.WriteLine2("EquipmentNo,,");

			this.WriteLine2("Rework,,");

			this.WriteLine2("K Value,,Use,,WLD Individual,,Pass,,1,0,1,0");

			this.WriteLine2("LOP Ratio,,0,0,0,0,0,ToolFactor,1,0,0,0,0,0,0");

			this.WriteLine2("Dark Value,,");

			this.WriteLine2("Remark1,,");

			this.WriteLine2("ToolFactor,,");

			this.WriteLine2("BinGrade,,");

			this.WriteLine2("Sort,,");

			this.WriteLine2("Bin,,");

			this.WriteLine2("");

			this.WriteLine2("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

			string lineTestItemCondition = string.Empty;

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

					lineTestItemCondition = string.Empty;

					lineTestItemCondition += testItem.Name + ",";

					lineTestItemCondition += testItem.ElecSetting[0].ForceValue + ",";

					lineTestItemCondition += testItem.ElecSetting[0].ForceUnit + ",";

					lineTestItemCondition += testItem.ElecSetting[0].ForceTime + ",";

					lineTestItemCondition += testItem.ElecSetting[0].ForceRange + ",";

					lineTestItemCondition += testItem.MsrtResult[0].Unit + ",";

					lineTestItemCondition += ",";

					lineTestItemCondition += ",";

					lineTestItemCondition += ",";

					lineTestItemCondition += testItem.GainOffsetSetting[0].Gain + ",";

					lineTestItemCondition += testItem.GainOffsetSetting[0].Offset + ",";

					lineTestItemCondition += testItem.MsrtResult[0].Unit + ",";

					this.WriteLine2(lineTestItemCondition);

				}
			}

			int keyWL1;
			int keyWL2;
			int keyWL3;
			int keyWL4;

			this.WriteLine2("WDOffset1,WDOffset2,WDOffset3,WDOffset4,MLOP1,MLOP2,MLOP3,MLOP4,MWLD1,MWLD2,MWLD3,MWLD4");

			this.WriteLine2("0,0,0,0,0,0,0,0,0,0,0,0");

			this.WriteLine2("KValue");

			this.WriteLine2("WLD,LOP1,LOP2,LOP3,LOP4,WLD1,WLD2,WLD3,WLD4,WLP1,WLP2,WLP3,WLP4,HW1,HW2,HW3,HW4");

			this._WLDTable.Clear();
			this._WL1Table.Clear();
			this._WL2Table.Clear();
			this._WL3Table.Clear();
			this._WL4Table.Clear();

			if (this.Product.TestCondition != null &&
				this.Product.TestCondition.TestItemArray != null &&
				this.Product.TestCondition.TestItemArray.Length > 0)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{
					if (testItem.KeyName == "LOPWL_1")
					{
						for (int i = 0; i < (testItem as LOPWLTestItem).CoefTable.Length; i++)
						{
							keyWL1 = (int)(testItem as LOPWLTestItem).CoefTable[i][0];		// pm for wavelength	                 
							this._WL1Table.Add(keyWL1, (testItem as LOPWLTestItem).CoefTable[i]);
						}
					}
					else if (testItem.KeyName == "LOPWL_2")
					{
						for (int i = 0; i < (testItem as LOPWLTestItem).CoefTable.Length; i++)
						{
							keyWL2 = (int)(testItem as LOPWLTestItem).CoefTable[i][0];		// pm for wavelength	                 
							this._WL2Table.Add(keyWL2, (testItem as LOPWLTestItem).CoefTable[i]);
						}
					}
					else if (testItem.KeyName == "LOPWL_3")
					{
						for (int i = 0; i < (testItem as LOPWLTestItem).CoefTable.Length; i++)
						{
							keyWL3 = (int)(testItem as LOPWLTestItem).CoefTable[i][0];		// pm for wavelength	                 
							this._WL1Table.Add(keyWL3, (testItem as LOPWLTestItem).CoefTable[i]);
						}
					}
					else if (testItem.KeyName == "LOPWL_4")
					{
						for (int i = 0; i < (testItem as LOPWLTestItem).CoefTable.Length; i++)
						{
							keyWL4 = (int)(testItem as LOPWLTestItem).CoefTable[i][0];		// pm for wavelength	                 
							this._WL1Table.Add(keyWL4, (testItem as LOPWLTestItem).CoefTable[i]);
						}
					}
				}
			}

			double[] coefTable1;
			double[] coefTable2;
			double[] coefTable3;
			double[] coefTable4;

			for (int i = 430; i <= 470; i++)
			{
				string[] lineWLDTable = new string[17];

				lineWLDTable[0] = i.ToString();

				coefTable1 = null;
				coefTable2 = null;
				coefTable3 = null;
				coefTable4 = null;

				if (this._WL1Table.TryGetValue(i, out coefTable1))
				{
					if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
					{
						lineWLDTable[1] = coefTable1[6].ToString();
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
					{
						lineWLDTable[1] = coefTable1[4].ToString();
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
					{
						lineWLDTable[1] = coefTable1[5].ToString();
					}
					else
					{
						lineWLDTable[1] = coefTable1[4].ToString();
					}

					lineWLDTable[5] = coefTable1[2].ToString();

					lineWLDTable[9] = coefTable1[1].ToString();

					lineWLDTable[13] = coefTable1[7].ToString();
				}
				else
				{
					lineWLDTable[1] = "0";

					lineWLDTable[5] = "0";

					lineWLDTable[9] = "0";

					lineWLDTable[13] = "0";
				}

				if (this._WL2Table.TryGetValue(i, out coefTable2))
				{
					if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
					{
						lineWLDTable[2] = coefTable2[6].ToString();
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
					{
						lineWLDTable[2] = coefTable2[4].ToString();
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
					{
						lineWLDTable[2] = coefTable2[5].ToString();
					}
					else
					{
						lineWLDTable[2] = coefTable2[4].ToString();
					}

					lineWLDTable[6] = coefTable2[2].ToString();

					lineWLDTable[10] = coefTable2[1].ToString();

					lineWLDTable[14] = coefTable2[7].ToString();
				}
				else
				{
					lineWLDTable[2] = "0";

					lineWLDTable[6] = "0";

					lineWLDTable[10] = "0";

					lineWLDTable[14] = "0";
				}

				if (this._WL3Table.TryGetValue(i, out coefTable3))
				{
					if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
					{
						lineWLDTable[3] = coefTable3[6].ToString();
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
					{
						lineWLDTable[3] = coefTable3[4].ToString();
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
					{
						lineWLDTable[3] = coefTable3[5].ToString();
					}
					else
					{
						lineWLDTable[3] = coefTable3[4].ToString();
					}

					lineWLDTable[7] = coefTable3[2].ToString();

					lineWLDTable[11] = coefTable3[1].ToString();

					lineWLDTable[15] = coefTable3[7].ToString();
				}
				else
				{
					lineWLDTable[3] = "0";

					lineWLDTable[7] = "0";

					lineWLDTable[11] = "0";

					lineWLDTable[15] = "0";
				}

				if (this._WL4Table.TryGetValue(i, out coefTable4))
				{
					if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
					{
						lineWLDTable[4] = coefTable4[6].ToString();
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
					{
						lineWLDTable[4] = coefTable4[4].ToString();
					}
					else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
					{
						lineWLDTable[4] = coefTable4[5].ToString();
					}
					else
					{
						lineWLDTable[4] = coefTable4[4].ToString();
					}

					lineWLDTable[8] = coefTable4[2].ToString();

					lineWLDTable[12] = coefTable4[1].ToString();

					lineWLDTable[16] = coefTable4[7].ToString();

				}
				else
				{
					lineWLDTable[4] = "0";

					lineWLDTable[8] = "0";

					lineWLDTable[12] = "0";

					lineWLDTable[16] = "0";
				}

				this._WLDTable.Add(lineWLDTable);

			}

			string[] lineTable;

			string lineTableWrite;

			for (int i = 0; i < this._WLDTable.Count; i++)
			{
				lineTable = this._WLDTable[i];

				lineTableWrite = string.Empty;

				for (int j = 0; j < lineTable.Length; j++)
				{
					lineTableWrite += lineTable[j];

					if (j < lineTable.Length - 1)
					{
						lineTableWrite += ",";
					}
				}

				this.WriteLine2(lineTableWrite);
			}

			this.WriteLine2("");

			////////////////////////////////////////////
			//Write Result Item Title
			////////////////////////////////////////////



			this.WriteLine2(this.ResultTitleInfo2.TitleStr);

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser2()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string testCount = "TotalTested,," + ReportProcess.TestCount.ToString();

			replaceData.Add("TotalTested,,", testCount);

			this.ReplaceReport2(replaceData);

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
		{
			string line = string.Empty;

			int column = 0;

			foreach (var resultItem in this.ResultTitleInfo2)
			{
				if (data.ContainsKey(resultItem.Key))
				{
					string format = string.Empty;

					if (this.ResultData.ContainsKey(resultItem.Key))
					{
						format = this.ResultData[resultItem.Key].Formate;

						//line += data[resultItem.Key].ToString(format);
					}

					line += data[resultItem.Key].ToString(format);
				}

				column++;

				if (column != this.ResultTitleInfo2.ResultCount)
				{
					line += ",";
				}
			}

			this.WriteLine2(line);

			return EErrorCode.NONE;
		}

		protected override EErrorCode RunCommandByUser2(EServerQueryCmd cmd)
		{
			switch (cmd)
			{
				case EServerQueryCmd.CMD_TESTER_START:
					{
						//this.ProductInfoExport(path, "Start");

						return this.WriteReportHeadByUser2();
					}
				case EServerQueryCmd.CMD_TESTER_END:
				case EServerQueryCmd.CMD_TESTER_ABORT:
					{
						EErrorCode code = this.RewriteReportByUser2();

						//this.ProductInfoExport(path, "End");

						if (code != EErrorCode.NONE)
						{
							return code;
						}

						return this.MoveFileToTargetByUser2(cmd);
					}
				default:
					{
						return EErrorCode.NONE;
					}
			}
		}

		protected override EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
		{
			///////////////////////////////////////////
			// Copy Report To Target
			///////////////////////////////////////////
			bool isWAFOutputPath01 = false;

			string outWAFPath01 = string.Empty;


			ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

			isWAFOutputPath01 = this.UISetting.IsEnableWAFPath01;

			outWAFPath01 = this.UISetting.WAFOutputPath01;

			type01 = this.UISetting.WAFTesterResultCreatFolderType01;

			if (type01 == ETesterResultCreatFolderType.ByLotNumber)
			{
				outWAFPath01 = Path.Combine(outWAFPath01, this.UISetting.LotNumber);
			}
			else if (type01 == ETesterResultCreatFolderType.ByMachineName)
			{
				outWAFPath01 = Path.Combine(outWAFPath01, this.UISetting.MachineName);
			}
			else if (type01 == ETesterResultCreatFolderType.ByDataTime)
			{
				outWAFPath01 = Path.Combine(outWAFPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
			}

			//---------------------------------------------------------------------------------
			// Copy Report file to taget path
			//---------------------------------------------------------------------------------
			string fileNameWithoutExt = this.UISetting.TestResultFileName;

			string fileNameWithExt = this.UISetting.TestResultFileName;

			if (!this.UISetting.IsManualRunMode)
			{
				fileNameWithoutExt = this.TestResultFileNameWithoutExt();

				fileNameWithExt = this.TestResultFileNameWithoutExt();
			}

			//Abort
			if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
			{
				fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
			}

			fileNameWithExt = fileNameWithoutExt + "_Ft" + "." + this.UISetting.TestResultFileExt;

			string outputWAFPathAndFile01 = Path.Combine(outWAFPath01, fileNameWithExt);

			DriveInfo driveInfo = new DriveInfo(outputWAFPathAndFile01);

			if (isWAFOutputPath01)
			{
				if (driveInfo.IsReady)
				{
					if (driveInfo.DriveType == DriveType.Fixed)
					{
						if (File.Exists(outputWAFPathAndFile01))
						{
							int number = 1;

							do
							{
								outputWAFPathAndFile01 = Path.Combine(outWAFPath01, fileNameWithoutExt + "_Ft" + "_" + number.ToString() + "." + this.UISetting.TestResultFileExt);

								number++;
							}
							while (File.Exists(outputWAFPathAndFile01));
						}

                        MPIFile.CopyFile(this.FileFullNameRep2, outputWAFPathAndFile01);
					}
					else
					{
						Console.WriteLine("[HP000 Report], MoveFileToTargetByUser(), drive type not fixed:" + driveInfo.DriveType.ToString() + "," + outputWAFPathAndFile01);
					}
				}
				else
				{
					Console.WriteLine("[HP000 Report], MoveFileToTargetByUser(), drive no ready:" + outputWAFPathAndFile01);
				}
			}
			else
			{
				Console.WriteLine("[HP000 Report], MoveFileToTargetByUser(), isOutputPath02:" + isWAFOutputPath01.ToString());
			}


			return EErrorCode.NONE;
		}

		//protected override EErrorCode WriteReportHeadByUser3()
		//{
		//    ////////////////////////////////////////////
		//    //Write Report Head
		//    ////////////////////////////////////////////

		//    this.WriteLine3("FileName,," + Path.Combine(this.TestResultFileNameWithoutExt(), this.UISetting.TestResultFileExt));

		//    this.WriteLine3("LotNumber,," + this.UISetting.LotNumber);

		//    this.WriteLine3("Model,," + this.MachineInfo.TesterModel);

		//    this.WriteLine3("TestStartTime,," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

		//    this.WriteLine3("TestTime,," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

		//    this.WriteLine3("TesterModel,," + this.MachineInfo.TesterModel);

		//    this.WriteLine3("TesterNumber,," + this.UISetting.MachineName);

		//    string lineLOPGain = "";
		//    string lineWLDOffset = "";
		//    string lineVF1Offset = "";

		//    if (this.Product.TestCondition != null &&
		//        this.Product.TestCondition.TestItemArray != null &&
		//        this.Product.TestCondition.TestItemArray.Length > 0)
		//    {
		//        foreach (var testItem in this.Product.TestCondition.TestItemArray)
		//        {
		//            if (testItem.KeyName == "LOPWL_1")
		//            {
		//                for (int i = 0; i < testItem.GainOffsetSetting.Length; i++)
		//                {
		//                    if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
		//                    {
		//                        if (testItem.GainOffsetSetting[i].KeyName == "LOP_1")
		//                        {
		//                            lineLOPGain = testItem.GainOffsetSetting[i].Gain.ToString();
		//                        }
		//                    }
		//                    else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
		//                    {
		//                        if (testItem.GainOffsetSetting[i].KeyName == "LM_1")
		//                        {
		//                            lineLOPGain = testItem.GainOffsetSetting[i].Gain.ToString();
		//                        }
		//                    }
		//                    else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
		//                    {
		//                        if (testItem.GainOffsetSetting[i].KeyName == "WATT_1")
		//                        {
		//                            lineLOPGain = testItem.GainOffsetSetting[i].Gain.ToString();
		//                        }
		//                    }

		//                    if (testItem.GainOffsetSetting[i].KeyName == "WLD_1")
		//                    {
		//                        lineWLDOffset = testItem.GainOffsetSetting[0].Offset.ToString();
		//                    }                                                        
		//                }
		//            }

		//            if (testItem.KeyName == "VF_1")
		//            {
		//                lineVF1Offset = testItem.GainOffsetSetting[0].Offset.ToString();
		//            }

		//        }
		//    }

		//    this.WriteLine3("LOPGain,," + lineLOPGain);

		//    this.WriteLine3("WLDOffset,," + lineWLDOffset);

		//    this.WriteLine3("VF1Offset,," + lineVF1Offset);

		//    this.WriteLine3("TotalTest,,");                        

		//    this.WriteLine3("Operator,," + this.UISetting.OperatorName);

		//    this.WriteLine3("MaximumBin,,");

		//    this.WriteLine3("");

		//    ////////////////////////////////////////////
		//    //Write Result Item Title
		//    ////////////////////////////////////////////

		//    this.ResultTitleInfo3.Clear();

		//    this.ResultTitleInfo3.AddResultData("TEST", "TEST");

		//    this.ResultTitleInfo3.AddResultData("BIN", "BIN");

		//    this.ResultTitleInfo3.AddResultData("MVF_1", "VF1");

		//    this.ResultTitleInfo3.AddResultData("MVF_2", "VF2");

		//    this.ResultTitleInfo3.AddResultData("MVF_3", "VF3");

		//    this.ResultTitleInfo3.AddResultData("MVF_4", "VF4");

		//    this.ResultTitleInfo3.AddResultData("MVFMA_1", "VFM1");

		//    this.ResultTitleInfo3.AddResultData("MVFMC_1", "VFM2");

		//    this.ResultTitleInfo3.AddResultData("MVFMD_1", "DVF");

		//    this.ResultTitleInfo3.AddResultData("MTHYVP_1", "VF");

		//    this.ResultTitleInfo3.AddResultData("MTHYVD_1", "VFD");

		//    this.ResultTitleInfo3.AddResultData("MVZ_1", "VZ1");

		//    this.ResultTitleInfo3.AddResultData("MVZ_2", "VZ_2");

		//    this.ResultTitleInfo3.AddResultData("MIR_1", "IR");


		//    if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
		//    {
		//        this.ResultTitleInfo3.AddResultData("LOP_1", "LOP1");

		//        this.ResultTitleInfo3.AddResultData("LOP_2", "LOP2");

		//        this.ResultTitleInfo3.AddResultData("LOP_3", "LOP3");
		//    }
		//    else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
		//    {
		//        this.ResultTitleInfo3.AddResultData("LM_1", "LOP1");

		//        this.ResultTitleInfo3.AddResultData("LM_2", "LOP2");

		//        this.ResultTitleInfo3.AddResultData("LM_3", "LOP3");
		//    }
		//    else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
		//    {
		//        this.ResultTitleInfo3.AddResultData("WATT_1", "LOP1");

		//        this.ResultTitleInfo3.AddResultData("WATT_2", "LOP2");

		//        this.ResultTitleInfo3.AddResultData("WATT_3", "LOP3");
		//    }
		//    else
		//    {
		//        this.ResultTitleInfo3.AddResultData("LOP_1", "LOP1");

		//        this.ResultTitleInfo3.AddResultData("LOP_2", "LOP2");

		//        this.ResultTitleInfo3.AddResultData("LOP_3", "LOP3");
		//    }

		//    this.ResultTitleInfo3.AddResultData("WLP_1", "WLP1");

		//    this.ResultTitleInfo3.AddResultData("WLD_1", "WLD1");

		//    this.ResultTitleInfo3.AddResultData("WLC_1", "WLC1");

		//    this.ResultTitleInfo3.AddResultData("HW_1", "HW");

		//    this.ResultTitleInfo3.AddResultData("PURITY_1", "PURITY1");

		//    this.ResultTitleInfo3.AddResultData("CIEx_1", "X1");

		//    this.ResultTitleInfo3.AddResultData("CIEy_1", "Y1");

		//    this.ResultTitleInfo3.AddResultData("CIEz_1", "Z1");

		//    this.ResultTitleInfo3.AddResultData("ST_1", "ST1");

		//    this.ResultTitleInfo3.AddResultData("INT_1", "INT1");

		//    this.ResultTitleInfo3.AddResultData("WLP_2", "WLP2");

		//    this.ResultTitleInfo3.AddResultData("WLD_2", "WLD2");

		//    this.ResultTitleInfo3.AddResultData("WLC_2", "WLC2");

		//    this.ResultTitleInfo3.AddResultData("HW_2", "HW");

		//    this.ResultTitleInfo3.AddResultData("PURITY_2", "PURITY2");

		//    this.ResultTitleInfo3.AddResultData("CIEx_2", "X2");

		//    this.ResultTitleInfo3.AddResultData("CIEy_2", "Y2");

		//    this.ResultTitleInfo3.AddResultData("CIEz_2", "Z2");

		//    this.ResultTitleInfo3.AddResultData("ST_2", "ST2");

		//    this.ResultTitleInfo3.AddResultData("INT_2", "INT2");

		//    this.ResultTitleInfo3.AddResultData("WLP_3", "WLP3");

		//    this.ResultTitleInfo3.AddResultData("WLD_3", "WLD3");

		//    this.ResultTitleInfo3.AddResultData("WLC_3", "WLC3");

		//    this.ResultTitleInfo3.AddResultData("HW_3", "HW");

		//    this.ResultTitleInfo3.AddResultData("PURITY_3", "PURITY3");

		//    this.ResultTitleInfo3.AddResultData("CIEx_3", "X3");

		//    this.ResultTitleInfo3.AddResultData("CIEy_3", "Y3");

		//    this.ResultTitleInfo3.AddResultData("CIEz_3", "Z3");

		//    this.ResultTitleInfo3.AddResultData("ST_3", "ST3");

		//    this.ResultTitleInfo3.AddResultData("INT_3", "INT3");

		//    this.ResultTitleInfo3.AddResultData("MVF_6", "IV");

		//    this.ResultTitleInfo3.AddResultData("FESDF_1", "ESD1");

		//    this.ResultTitleInfo3.AddResultData("FESDF_2", "ESD2");

		//    this.ResultTitleInfo3.AddResultData("MIRJ_1", "IR1");

		//    this.ResultTitleInfo3.AddResultData("MIRJ_2", "IR2");

		//    this.ResultTitleInfo3.AddResultData("MESDF_1", "ESD1PASS");

		//    this.ResultTitleInfo3.AddResultData("MESDF_2", "ESD2PASS");

		//    this.ResultTitleInfo3.AddResultData("COL", "PosX");

		//    this.ResultTitleInfo3.AddResultData("ROW", "PosY");

		//    this.WriteLine3(this.ResultTitleInfo3.TitleStr);

		//    return EErrorCode.NONE;
		//}

		//protected override EErrorCode RewriteReportByUser3()
		//{
		//    Dictionary<string, string> replaceData = new Dictionary<string, string>();

		//    string testCount = "TotalTest,," + ReportProcess.TestCount.ToString();

		//    replaceData.Add("TotalTest,,", testCount);

		//    this.ReplaceReport3(replaceData);

		//    return EErrorCode.NONE;
		//}

		//protected override EErrorCode PushDataByUser3(Dictionary<string, double> data)
		//{            
		//    string line = string.Empty;
		//    bool isEsdEnable = false;
		//    int column = 0;
		//    int ESDJudePass = -1;

		//    this._zapTable.Clear();
		//    this._listMIRJ.Clear();

		//    if (this.Product.TestCondition != null &&
		//        this.Product.TestCondition.TestItemArray != null &&
		//        this.Product.TestCondition.TestItemArray.Length > 0)
		//    {
		//        foreach (var testItem in this.Product.TestCondition.TestItemArray)
		//        {
		//            if (!testItem.IsEnable)
		//            {
		//                continue;
		//            }         

		//            if (testItem is ESDTestItem)
		//            {
		//                if ((testItem as ESDTestItem).EsdSetting == null)
		//                {
		//                    isEsdEnable = false;

		//                    break;
		//                }

		//                isEsdEnable = true;

		//                foreach (int zapTable in (testItem as ESDTestItem).EsdSetting.TableValue)
		//                {
		//                    this._zapTable.Add(zapTable);
		//                }

		//                foreach (var ESDItem in (testItem as ESDTestItem).MsrtResult)
		//                {
		//                    if (ESDItem.KeyName == "JUDGEPASS_1")
		//                    {
		//                        ESDJudePass = (int)data["JUDGEPASS_1"];
		//                    }

		//                    if (ESDItem.KeyName.Contains("MIRJ"))
		//                    {
		//                        this._listMIRJ.Add(ESDItem.Value);
		//                    }
		//                }                        
		//            }                    
		//        }                
		//    }

		//    foreach (var resultItem in this.ResultTitleInfo3)
		//    {
		//        if (isEsdEnable)
		//        {
		//            if (resultItem.Key == "FESDF_1")
		//            {
		//                line += this._zapTable[0].ToString();
		//            }

		//            if (resultItem.Key == "FESDF_2")
		//            {
		//                line += this._zapTable[1].ToString();
		//            }

		//            if (resultItem.Key == "MIR_ESD1")
		//            {
		//                line += this._listMIRJ[0].ToString();
		//            }

		//            if (resultItem.Key == "MIR_ESD2")
		//            {
		//                line += this._listMIRJ[1].ToString();
		//            }

		//            if (resultItem.Key == "MESDF_1")
		//            {
		//                if (ESDJudePass == 0)
		//                {
		//                    line += "0";
		//                }
		//                else
		//                {
		//                    line += "1";
		//                }
		//            }

		//            if (resultItem.Key == "MESDF_2")
		//            {
		//                if (ESDJudePass == 99)
		//                {
		//                    line += "1";
		//                }
		//                else
		//                {
		//                    line += "0";
		//                }
		//            }
		//        }
		//        else
		//        {
		//            if (resultItem.Key == "FESDF_1" || resultItem.Key == "FESDF_2" ||
		//                resultItem.Key == "MIR_ESD1" || resultItem.Key == "MIR_ESD2" ||
		//                resultItem.Key == "MESDF_1" || resultItem.Key == "MESDF_2")
		//            {
		//                line += "";
		//            }
		//        }

		//        if (data.ContainsKey(resultItem.Key))
		//        {
		//            string format = string.Empty;

		//            if (this.ResultData.ContainsKey(resultItem.Key))
		//            {
		//                format = this.ResultData[resultItem.Key].Formate;

		//                //line += data[resultItem.Key].ToString(format);
		//            }

		//            line += data[resultItem.Key].ToString(format);
		//        }

		//        column++;

		//        if (column != this.ResultTitleInfo3.ResultCount)
		//        {
		//            line += ",";
		//        }
		//    }

		//    this.WriteLine3(line);

		//    return EErrorCode.NONE;
		//}

		//protected override EErrorCode RunCommandByUser3(EServerQueryCmd cmd)
		//{
		//    switch (cmd)
		//    {
		//        case EServerQueryCmd.CMD_TESTER_START:
		//            {
		//                //this.ProductInfoExport(path, "Start");

		//                return this.WriteReportHeadByUser3();
		//            }
		//        case EServerQueryCmd.CMD_TESTER_END:
		//        case EServerQueryCmd.CMD_TESTER_ABORT:
		//            {
		//                EErrorCode code = this.RewriteReportByUser3();

		//                //this.ProductInfoExport(path, "End");

		//                if (code != EErrorCode.NONE)
		//                {
		//                    return code;
		//                }

		//                return this.MoveFileToTargetByUser3(cmd);
		//            }
		//        default:
		//            {
		//                return EErrorCode.NONE;
		//            }
		//    }
		//}

		//protected override EErrorCode MoveFileToTargetByUser3(EServerQueryCmd cmd)
		//{
		//    ///////////////////////////////////////////
		//    // Copy Report To Target
		//    ///////////////////////////////////////////
		//    bool isSTATTOutputPath01 = false;                       

		//    string outSTATTPath01 = string.Empty;

		//    ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

		//    isSTATTOutputPath01 = this.UISetting.IsEnableSTATPath01;

		//    outSTATTPath01 = this.UISetting.STATOutputPath01;

		//    type01 = this.UISetting.STATTesterResultCreatFolderType01;

		//    if (type01 == ETesterResultCreatFolderType.ByLotNumber)
		//    {
		//        outSTATTPath01 = Path.Combine(outSTATTPath01, this.UISetting.LotNumber);
		//    }
		//    else if (type01 == ETesterResultCreatFolderType.ByMachineName)
		//    {
		//        outSTATTPath01 = Path.Combine(outSTATTPath01, this.UISetting.MachineName);
		//    }
		//    else if (type01 == ETesterResultCreatFolderType.ByDataTime)
		//    {
		//        outSTATTPath01 = Path.Combine(outSTATTPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
		//    }

		//    //---------------------------------------------------------------------------------
		//    // Copy Report file to taget path
		//    //---------------------------------------------------------------------------------
		//    string fileNameWithoutExt = this.UISetting.TestResultFileName;

		//    string fileNameWithExt = this.UISetting.TestResultFileName;

		//    if (!this.UISetting.IsManualRunMode)
		//    {
		//        fileNameWithoutExt = this.TestResultFileNameWithoutExt();

		//        fileNameWithExt = this.TestResultFileNameWithoutExt();
		//    }

		//    //Abort
		//    if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
		//    {
		//        fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
		//    }

		//    fileNameWithExt = fileNameWithoutExt + "_WM" + "." + this.UISetting.TestResultFileExt;

		//    string outputSTATTPathAndFile01 = Path.Combine(outSTATTPath01, fileNameWithExt);

		//    DriveInfo driveInfo = new DriveInfo(outputSTATTPathAndFile01);

		//    if (isSTATTOutputPath01)
		//    {
		//        if (driveInfo.IsReady)
		//        {
		//            if (driveInfo.DriveType == DriveType.Fixed)
		//            {
		//                if (File.Exists(outputSTATTPathAndFile01))
		//                {
		//                    int number = 1;

		//                    do
		//                    {
		//                        outputSTATTPathAndFile01 = Path.Combine(outSTATTPath01, fileNameWithoutExt + "_WM" + "_" + number.ToString() + "." + this.UISetting.TestResultFileExt);

		//                        number++;
		//                    }
		//                    while (File.Exists(outputSTATTPathAndFile01));
		//                }

		//                MPIFile.CopyFile(this.FileFullNameCsv3, outputSTATTPathAndFile01);
		//            }
		//            else
		//            {
		//                Console.WriteLine("[HP000 Report], MoveFileToTargetByUser(), drive type not fixed:" + driveInfo.DriveType.ToString() + "," + outputSTATTPathAndFile01);
		//            }
		//        }
		//        else
		//        {
		//            Console.WriteLine("[HP000 Report], MoveFileToTargetByUser(), drive no ready:" + outputSTATTPathAndFile01);
		//        }
		//    }
		//    else
		//    {
		//        Console.WriteLine("[HP000 Report], MoveFileToTargetByUser(), isOutputPath03:" + isSTATTOutputPath01.ToString());
		//    }

		//    return EErrorCode.NONE;
		//}

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
