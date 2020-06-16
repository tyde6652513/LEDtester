using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.TestServer;
using System.IO;

namespace MPI.Tester.Report.User.Eti
{
	public class Report : ReportBase
	{
        private const string NTLM_FLASH_FORMAT_NAME = "Format-NTLM_FLASH";

        private Dictionary<string, double> _gain = new Dictionary<string, double>();

		private Dictionary<string, double> _offset = new Dictionary<string, double>();

		private Dictionary<string, double> _forceValue = new Dictionary<string, double>();

		private string _IFMB_1_ForceTime;

        private StreamWriter _sw1;
        private StreamWriter _sw2;
        private StreamWriter _sw3;
        private StreamWriter _sw4;

        private string sweepFileFullNameTmp1;
        private string sweepFileFullNameTmp2;
        private string sweepFileFullNameTmp3;
        private string sweepFileFullNameTmp4;

		private List<string> _report2Item = new List<string>();

		public Report(List<object> objs, bool isReStatistic) : base(objs, isReStatistic)
		{
            this._isImplementSweepDataReport = true;

            this.sweepFileFullNameTmp1 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "NTLM1.temp");
            this.sweepFileFullNameTmp2 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "NTLM2.temp");
            this.sweepFileFullNameTmp3 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "NTLM3.temp");
            this.sweepFileFullNameTmp4 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "NTLM4.temp");
		}

        #region >>> Private Method <<<

        private void GetInfo()
		{
			this._gain.Clear();

			this._offset.Clear();

			this._forceValue.Clear();

			this._IFMB_1_ForceTime = "0";

			if (this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{
					if (testItem.ElecSetting != null)
					{
						foreach (var item in testItem.ElecSetting)
						{
							if (item.KeyName == "IFMB_1")
							{
								this._IFMB_1_ForceTime = item.ForceTime.ToString();
							}

                            if (testItem.IsEnable)
                            {
                                this._forceValue.Add(item.KeyName, item.ForceValue);
                            }
                            else
                            {
                                this._forceValue.Add(item.KeyName, 0);
                            }
						}
					}

					if (testItem.MsrtResult == null)
					{
						continue;
					}

                    if (testItem.GainOffsetSetting != null)
                    {
                        foreach (var factor in testItem.GainOffsetSetting)
                        {
                            this._gain.Add(factor.KeyName, factor.Gain);

                            this._offset.Add(factor.KeyName, factor.Offset);
                        }
                    }
				}
			}

			this._report2Item.Clear();

			this._report2Item.Add("COL");

			this._report2Item.Add("ROW");

			this._report2Item.Add("BIN");

			this._report2Item.Add("MVF_1");

			this._report2Item.Add("MVF_2");

			this._report2Item.Add("MVF_3");

			this._report2Item.Add("MVF_4");

			this._report2Item.Add("MIR_1");

			this._report2Item.Add("MVZ_1");

			this._report2Item.Add("WLD_1");

			this._report2Item.Add("WLP_1");

			this._report2Item.Add("LOP_1");

			this._report2Item.Add("MTHYVD_1");

			this._report2Item.Add("WLD_2");

			this._report2Item.Add("WLP_2");

			this._report2Item.Add("WATT_1");
		}

		private string GainStr(string keyName)
		{
			if (this._gain.ContainsKey(keyName))
			{
				return this._gain[keyName].ToString();
			}
			else
			{
				return "";
			}
		}

		private string OffsetStr(string keyName)
		{
			if (this._offset.ContainsKey(keyName))
			{
				return this._offset[keyName].ToString();
			}
			else
			{
				return "";
			}
		}

		private string FactorStr()
		{
			string factorStr = string.Empty;

			factorStr += "VFD," + this.GainStr("MTHYVD_1") + "," + this.OffsetStr("MTHYVD_1") + ",";

			factorStr += "VF1," + this.GainStr("MVF_1") + "," + this.OffsetStr("MVF_1") + ",";

			factorStr += "VF2," + this.GainStr("MVF_2") + "," + this.OffsetStr("MVF_2") + ",";

			factorStr += "VF3," + this.GainStr("MVF_3") + "," + this.OffsetStr("MVF_3") + ",";

			factorStr += "VF4," + this.GainStr("MVF_4") + "," + this.OffsetStr("MVF_4") + ",";

			factorStr += "VZ," + this.GainStr("MVZ_1") + "," + this.OffsetStr("MVZ_1") + ",";

			factorStr += "IR," + this.GainStr("MIR_1") + "," + this.OffsetStr("MIR_1") + ",";

			factorStr += "WLD1," + this.GainStr("WLD_1") + "," + this.OffsetStr("WLD_1") + ",";

			factorStr += "LOP1(mw)," + this.GainStr("WATT_1") + "," + this.OffsetStr("WATT_1") + ",";

			factorStr += "LOP1(mcd)," + this.GainStr("LOP_1") + "," + this.OffsetStr("LOP_1") + ",";

			factorStr += "WLP1," + this.GainStr("WLP_1") + "," + this.OffsetStr("WLP_1") + ",";

			factorStr += "HW1," + this.GainStr("HW_1") + "," + this.OffsetStr("HW_1") + ",";

			factorStr += "WLD2," + this.GainStr("WLD_2") + "," + this.OffsetStr("WLD_2") + ",";

			factorStr += "LOP2(mw)," + this.GainStr("WATT_2") + "," + this.OffsetStr("WATT_2") + ",";

			factorStr += "LOP2(mcd)," + this.GainStr("LOP_2") + "," + this.OffsetStr("LOP_2") + ",";

			factorStr += "WLP2," + this.GainStr("WLP_2") + "," + this.OffsetStr("WLP_2") + ",";

			factorStr += "HW2," + this.GainStr("HW_2") + "," + this.OffsetStr("HW_2") + ",";

			factorStr += "WLD3," + this.GainStr("WLD_3") + "," + this.OffsetStr("WLD_3") + ",";

			factorStr += "LOP3(mw)," + this.GainStr("WATT_3") + "," + this.OffsetStr("WATT_3") + ",";

			factorStr += "LOP3(mcd)," + this.GainStr("LOP_3") + "," + this.OffsetStr("LOP_3") + ",";

			factorStr += "WLP3," + this.GainStr("WLP_3") + "," + this.OffsetStr("WLP_3") + ",";

			factorStr += "HW3," + this.GainStr("HW_3") + "," + this.OffsetStr("HW_3") + ",";

			factorStr += "ES1," + this.UISetting.StartCountOfEdgeSensor[0] + "," + this.UISetting.EndCountOfEdgeSensor[0] + ",";

			factorStr += "ES2," + this.UISetting.StartCountOfEdgeSensor[1] + "," + this.UISetting.EndCountOfEdgeSensor[1] + ",";

			factorStr += "ES3," + this.UISetting.StartCountOfEdgeSensor[2] + "," + this.UISetting.EndCountOfEdgeSensor[2] + ",";

			factorStr += "ES4," + this.UISetting.StartCountOfEdgeSensor[3] + "," + this.UISetting.EndCountOfEdgeSensor[3] + ",";

			return factorStr;
		}

		private string UintStr(string keyName)
		{
			TestResultData data =this.UISetting.UserDefinedData[keyName];

			if (data == null)
			{
				return "";
			}
			else
			{
				return data.Unit;
			}
		}

		private string FormatStr(string keyName)
		{
			TestResultData data = this.UISetting.UserDefinedData[keyName];

			if (data == null)
			{
				return "";
			}
			else
			{
				return data.Formate;
			}
		}

		private string ItemNameStr()
		{
			string itemNameStr = string.Empty;

			foreach (var resultItem in this.UISetting.UserDefinedData.ResultItemNameDic)
			{
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

				if (resultItem.Key != "TEST")
				{
					itemNameStr += resultItem.Value;

					if (resultItem.Key == "TEST" || resultItem.Key == "BIN" || resultItem.Key == "ISALLPASS" ||
						resultItem.Key == "CONTA" || resultItem.Key == "CONTC" || resultItem.Key == "POLAR" ||
						resultItem.Key == "LOP_1" || resultItem.Key == "LOP_2" || resultItem.Key == "LOP_3" ||
						resultItem.Key == "WATT_1" || resultItem.Key == "WATT_2" || resultItem.Key == "WATT_3" ||
						resultItem.Key == "LM_1" || resultItem.Key == "LM_2" || resultItem.Key == "LM_3" ||
						resultItem.Key == "PURITY_1" || resultItem.Key == "PURITY_2" || resultItem.Key == "PURITY_3" ||
						resultItem.Key == "CIEx_1" || resultItem.Key == "CIEx_2" || resultItem.Key == "CIEx_3" ||
						resultItem.Key == "CIEy_1" || resultItem.Key == "CIEy_2" || resultItem.Key == "CIEy_3" ||
						resultItem.Key == "CIEz_1" || resultItem.Key == "CIEz_2" || resultItem.Key == "CIEz_3" ||
						resultItem.Key == "CCT_1" || resultItem.Key == "CCT_2" || resultItem.Key == "CCT_3" ||
						resultItem.Key == "INT_1" || resultItem.Key == "INT_2" || resultItem.Key == "INT_3" ||
						resultItem.Key == "ROW" || resultItem.Key == "COL")
					{
						itemNameStr += ",";
					}
					else
					{
						itemNameStr += "(" + this.UintStr(resultItem.Key) + "),";
					}
				}
			}

			if (itemNameStr.Length > 0 && itemNameStr[itemNameStr.Length - 1] == ',')
			{
				itemNameStr = itemNameStr.Remove(itemNameStr.Length - 1);
			}

			return itemNameStr;
		}

		private string DataFormatStr()
		{
			string dataFormatStr = string.Empty;

			foreach (var resultItem in this.UISetting.UserDefinedData.ResultItemNameDic)
			{
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

				if (resultItem.Key == "TEST" || resultItem.Key == "MIR_1" ||
					resultItem.Key == "LOP_1" || resultItem.Key == "LOP_2" || resultItem.Key == "LOP_3" ||
					resultItem.Key == "WATT_1" || resultItem.Key == "WATT_2" || resultItem.Key == "WATT_3" ||
					resultItem.Key == "LM_1" || resultItem.Key == "LM_2" || resultItem.Key == "LM_3" ||
					resultItem.Key == "ROW" || resultItem.Key == "COL")
				{

				}
				else
				{
					dataFormatStr += this.FormatStr(resultItem.Key);
				}

				if (resultItem.Key != "TEST")
				{
					dataFormatStr += ",";
				}
			}

			if (dataFormatStr.Length > 0 && dataFormatStr[dataFormatStr.Length - 1] == ',')
			{
				dataFormatStr = dataFormatStr.Remove(dataFormatStr.Length - 1);
			}

			return dataFormatStr;
		}

		private string TestConditionStr()
		{
			string testConditionStr = string.Empty;

			testConditionStr += "FFINAL(V),RFINAL(V),FESD(V),RESD(V),BIN,,,,IF1(mA),IF2(mA),IF3(mA),IF4(mA),";

			testConditionStr += "IFM1(mA),IFM2(mA),IFP*T,,,IZ1(uA),IZ2(uA),VR(V),IF1(mA),IF2(mA),IF3(mA),IF1(mA),IF1(mA),";

			testConditionStr += "IF1(mA),,,,,,IF1(mA),IF1(mA),IF1(mA),IF2(mA),IF2(mA),IF2(mA),,,,,,IF2(mA),IF2(mA),";

			testConditionStr += "IF2(mA),IF3(mA),IF3(mA),IF3(mA),,,,,,IF3(mA),IF3(mA),IF3(mA),,";

			return testConditionStr;
		}

		private string ForceElecStr(string keyName)
		{
			if (this._forceValue.ContainsKey(keyName))
			{
				return Math.Abs(this._forceValue[keyName]).ToString();
			}
			else
			{
				return "0";
			}
		}

		private string BinStr()
		{
			string binStr = string.Empty;

			binStr += "@" + this.ForceElecStr("IF_1") + ",";
			binStr += "@" + this.ForceElecStr("IF_2") + ",";
			binStr += "@" + this.ForceElecStr("IF_3") + ",";
			binStr += "@" + this.ForceElecStr("IF_4") + ",";
			binStr += "@" + this.ForceElecStr("IFMA_1") + ",";
			binStr += "@" + this.ForceElecStr("IFMC_1") + ",";
			binStr += "@" + this.ForceElecStr("IFMB_1") + "*";
			binStr += this._IFMB_1_ForceTime + ",,,";
			binStr += "@" + this.ForceElecStr("IZ_1") + ",";
			binStr += "@" + this.ForceElecStr("IZ_2") + ",";
			binStr += "@" + this.ForceElecStr("VR_1") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_1") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_2") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_3") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_1") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_1") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_1") + ",,,,,,";
			binStr += "@" + this.ForceElecStr("IFWLA_1") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_1") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_1") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_2") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_2") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_2") + ",,,,,,";
			binStr += "@" + this.ForceElecStr("IFWLA_2") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_2") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_2") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_3") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_3") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_3") + ",,,,,,";
			binStr += "@" + this.ForceElecStr("IFWLA_3") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_3") + ",";
			binStr += "@" + this.ForceElecStr("IFWLA_3") + ",,";

			return binStr;
		}

		private string targetStr()
		{
			DateTime dt = this.TesterSetting.StartTestTime;
			DateTime dtBase = new DateTime(2012, 2, 21, 0, 0, 0);

			//Calculate Target
			TimeSpan ts1 = new TimeSpan(dt.Ticks);
			TimeSpan ts2 = new TimeSpan(dtBase.Ticks);
			TimeSpan ts = ts1.Subtract(ts2).Duration();

			int day = ts.Days % 256;

			if (dt.Hour >= 12)
			{
				day++;
			}

			return ((double)day * 52.34158).ToString();
		}

        private void OpenWriter()
        {
            this.CloseWriter();

            if (File.Exists(this.sweepFileFullNameTmp1))
            {
                MPIFile.DeleteFile(this.sweepFileFullNameTmp1);
            }

            if (File.Exists(this.sweepFileFullNameTmp2))
            {
                MPIFile.DeleteFile(this.sweepFileFullNameTmp2);
            }

            if (File.Exists(this.sweepFileFullNameTmp3))
            {
                MPIFile.DeleteFile(this.sweepFileFullNameTmp3);
            }

            if (File.Exists(this.sweepFileFullNameTmp4))
            {
                MPIFile.DeleteFile(this.sweepFileFullNameTmp4);
            }

            this._sw1 = new StreamWriter(this.sweepFileFullNameTmp1, true, Encoding.Default);
            this._sw2 = new StreamWriter(this.sweepFileFullNameTmp2, true, Encoding.Default);
            this._sw3 = new StreamWriter(this.sweepFileFullNameTmp3, true, Encoding.Default);
            this._sw4 = new StreamWriter(this.sweepFileFullNameTmp4, true, Encoding.Default);
        }

        private void CloseWriter()
        {
            if (this._sw1 != null)
            {
                this._sw1.Close();

                this._sw1.Dispose();

                this._sw1 = null;
            }

            if (this._sw2 != null)
            {
                this._sw2.Close();

                this._sw2.Dispose();

                this._sw2 = null;
            }

            if (this._sw3 != null)
            {
                this._sw3.Close();

                this._sw3.Dispose();

                this._sw3 = null;
            }

            if (this._sw4 != null)
            {
                this._sw4.Close();

                this._sw4.Dispose();

                this._sw4 = null;
            }
        }

        private void Writer1(string str)
        {
            if (this._sw1 != null)
            {
                this._sw1.WriteLine(str);

                this._sw1.Flush();
            }
        }

        private void Writer2(string str)
        {
            if (this._sw2 != null)
            {
                this._sw2.WriteLine(str);

                this._sw2.Flush();
            }
        }

        private void Writer3(string str)
        {
            if (this._sw3 != null)
            {
                this._sw3.WriteLine(str);

                this._sw3.Flush();
            }
        }

        private void Writer4(string str)
        {
            if (this._sw4 != null)
            {
                this._sw4.WriteLine(str);

                this._sw4.Flush();
            }
        }

        #endregion

        #region >>> Protected Override Method <<<

        protected override void SetResultTitle()
		{
			this.GetInfo();

			this.ResultTitleInfo.Clear();

            if (this.UISetting.FormatName == NTLM_FLASH_FORMAT_NAME)
            {
                //--------------------------------------------------------------------------------------------------
                // 特殊版的 Report Format-NTLM_Flash
                //--------------------------------------------------------------------------------------------------
                this.ResultTitleInfo.AddResultData("COL", "Die X");
                this.ResultTitleInfo.AddResultData("ROW", "Die Y");
                this.ResultTitleInfo.AddResultData("BIN", "Bin");
                this.ResultTitleInfo.AddResultData("TEST", "Die No.");
                this.ResultTitleInfo.AddResultData("POLAR", "POLAR");

                this.ResultTitleInfo.AddResultData("MIFSV_1", "BREAKP.Voltage");
                this.ResultTitleInfo.AddResultData("MIF_1", "BREAKP.Current");

                this.ResultTitleInfo.AddResultData("MVF_1", "VF1.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_1", "VF1.Current");

                this.ResultTitleInfo.AddResultData("MVF_2", "VF2.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_2", "VF2.Current");

                this.ResultTitleInfo.AddResultData("MVF_3", "VF3.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_3", "VF3.Current");

                this.ResultTitleInfo.AddResultData("MVF_4", "VF4.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_4", "VF4.Current");

                this.ResultTitleInfo.AddResultData("MVF_5", "VF5.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_5", "VF5.Current");

                this.ResultTitleInfo.AddResultData("MVF_6", "VF6.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_6", "VF6.Current");

                this.ResultTitleInfo.AddResultData("MVF_7", "VF7.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_7", "VF7.Current");

                this.ResultTitleInfo.AddResultData("MVF_8", "VF8.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_8", "VF8.Current");

                this.ResultTitleInfo.AddResultData("MVF_9", "VF9.Voltage");
                this.ResultTitleInfo.AddResultData("MVFSI_9", "VF9.Current");
            }
            else
            {
                //--------------------------------------------------------------------------------------------------
                // 正常版的 Report Format-FullESD
                //--------------------------------------------------------------------------------------------------
                foreach (var resultItem in this.UISetting.UserDefinedData.ResultItemNameDic)
                {
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
		}

		protected override EErrorCode WriteReportHeadByUser()
		{
			this.WriteLine("FileID,,\"WEI MIN Data File\"");

			this.WriteLine("FileName,,\"" + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt + "\"");

			this.WriteLine("TestTime");

			this.WriteLine("TesterNumber,,\"" + this.UISetting.MachineName + "\"");

			this.WriteLine("TesterModel,,\"LED617WH\"");

			this.WriteLine("CommPort,,\"1\"");

			this.WriteLine("Specification,,\"" + this.UISetting.ProductFileName + ".pd\"");

			this.WriteLine("SampleBins,,\"" + this.FactorStr() + "\"");

			this.WriteLine("SampleStandard,,\"WEIMIN\"");

			this.WriteLine("SampleLevel,,\"STANDARD\"");

			this.WriteLine("TotalTested");

			this.WriteLine("Samples");

			this.WriteLine("Operator,,\"" + this.UISetting.LoginID + "-" + this.UISetting.OperatorName + "\"");

			this.WriteLine("MaximumBin,,\"200\"");

			this.WriteLine("ItemName,,\"" + this.ItemNameStr() + "\"");

			this.WriteLine("DataFormat,,\"" + this.DataFormatStr() + "\"");

			this.WriteLine("TestCondition,,\"" + this.TestConditionStr() + "\"");

			this.WriteLine("At,,\",,,,ALL,,,," + this.BinStr() + "\"");

			for (int i = 1; i < 200; i++)
			{
				this.WriteLine("BinAt" + i + ",,\",,,," + i + ",,,," + this.BinStr() + "\"");
			}

			this.WriteLine("Target,,\"" + this.targetStr() + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,\"");

			this.WriteLine("LowLimit,,\",,,,,,,,0,0,0,0,0,0,0,0,,0,0,0,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,,\"");

			this.WriteLine("HighLimit,,\",,,,,0,0,,0,0,0,0,0,0,0,0,0,0,0,0.3,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,0,,,,0,0,0,,\"");

			this.WriteLine("");

			this.WriteLine(this.ResultTitleInfo.TitleStr);

			return EErrorCode.NONE;
		}

        //protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
        //{
        //    int binSN = (int)data["BINSN"];

        //    SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

        //    int binGrade = 0;

        //    int binNumber = 0;

        //    string binCode = string.Empty;

        //    if (bin != null)
        //    {
        //        binCode = bin.BinCode;

        //        binNumber = bin.BinNumber;

        //        if (bin.BinningType == EBinningType.IN_BIN)
        //        {
        //            binGrade = 1;
        //        }
        //        else if (bin.BinningType == EBinningType.SIDE_BIN)
        //        {
        //            binGrade = 2;
        //        }
        //        else if (bin.BinningType == EBinningType.NG_BIN)
        //        {
        //            binGrade = 3;
        //        }
        //    }

        //    string line = string.Empty;

        //    int index = 0;

        //    if (this.UISetting.FormatName == NTLM_FLASH_FORMAT_NAME)
        //    {
        //        //------------------------------------------------------------------------
        //        // Format-NTLN_FLASH
        //        //------------------------------------------------------------------------
        //        foreach (var item in this.ResultTitleInfo)
        //        {
        //            if (data.ContainsKey(item.Key))
        //            {
        //                string format = string.Empty;

        //                if (this.UISetting.UserDefinedData[item.Key] != null)
        //                {
        //                    format = this.UISetting.UserDefinedData[item.Key].Formate;
        //                }

        //                if (item.Key.Contains("MVF") || item.Key.Contains("MVZ"))
        //                {
        //                    // MsrtV + Msrt ForceI
        //                    line += string.Format("{0},{1}", data[item.Key].ToString(format), this.AcquireData[item.Key].ExtValue);
                          
        //                }
        //                else if (item.Key.Contains("MIF") || item.Key.Contains("MIR"))
        //                {
        //                    // Msrt ForceV + MsrtI
        //                    line += string.Format("{0},{1}", this.AcquireData[item.Key].ExtValue, data[item.Key].ToString(format));
        //                }
        //                else
        //                {
        //                    line += data[item.Key].ToString(format);
        //                }

        //                if (index != this.ResultTitleInfo.ResultCount - 2)
        //                {
        //                    line += ",";
        //                }
        //            }

        //            index++;
        //        }
        //    }
        //    else
        //    {
        //        //------------------------------------------------------------------------
        //        // Format-FullESD
        //        //------------------------------------------------------------------------
        //        foreach (var item in this.ResultTitleInfo)
        //        {
        //            if (item.Key == "BIN_CODE")
        //            {
        //                line += binCode;
        //            }
        //            else if (item.Key == "BIN_NUMBER")
        //            {
        //                line += binNumber.ToString();
        //            }
        //            else if (item.Key == "BIN_GRADE")
        //            {
        //                line += binGrade.ToString();
        //            }
        //            else if (data.ContainsKey(item.Key))
        //            {
        //                string format = string.Empty;

        //                if (this.UISetting.UserDefinedData[item.Key] != null)
        //                {
        //                    format = this.UISetting.UserDefinedData[item.Key].Formate;
        //                }

        //                line += data[item.Key].ToString(format);
        //            }

        //            index++;

        //            if (index != this.ResultTitleInfo.ResultCount)
        //            {
        //                line += ",";
        //            }
        //        }
        //    }

        //    this.WriteLine(line);

        //    return EErrorCode.NONE;
        //}

		protected override EErrorCode RewriteReportByUser()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string testTime = "TestTime,,\"" + this.TesterSetting.StartTestTime.ToString("yy/MM/dd HH:mm:ss") + "\"";

			string replaceTotalCnt = "TotalTested,,\"" + this.TotalTestCount.ToString() + "\"";

			string replaceSample = "Samples,,\"" + this.UISetting.TotalSacnCounts.ToString() + "\"";

			replaceData.Add("TestTime", testTime);

			replaceData.Add("TotalTested", replaceTotalCnt);

			replaceData.Add("Samples", replaceSample);

			this.ReplaceReport(replaceData);

			return EErrorCode.NONE;
		}

        // WAF Report
		protected override EErrorCode WriteReportHeadByUser2()
		{
			this.WriteLine2("MODEL," + this.UISetting.ProductFileName + ".pd");

			this.WriteLine2("WAFER NO.," + this.UISetting.WaferNumber);

			this.WriteLine2("TEST START TIME");

			this.WriteLine2("TEST END TIME");

			this.WriteLine2("MACHINE NO.," + this.UISetting.MachineName);

			this.WriteLine2("TEST MODEL,LED217WH");

			this.WriteLine2("LOGIN MODE," + this.UISetting.OperatorName);

			this.WriteLine2("TOTAL CHIP");

			this.WriteLine2("OK CHIP");

			this.WriteLine2("NG CHIP");

			this.WriteLine2("");

			this.WriteLine2("XADR,YADR,RANK,VF1,VF2,VF3,VF4,IR,VR,WD,WP,IV,VFD,WD2,WP2,IV2");

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser2()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string testStartTime = "TEST START TIME," + this.TesterSetting.StartTestTime.ToString("yyyyMMddHHmmss");

			string testEndTime = "TEST END TIME," + this.TesterSetting.EndTestTime.ToString("yyyyMMddHHmmss");

			string totalChip = "TOTAL CHIP," + this.TotalTestCount.ToString();

			string okChip = "OK CHIP," + ReportBase.GoodCount01;

			string ngChip = "NG CHIP," + ReportBase.FailCount01;

			replaceData.Add("TEST START TIME", testStartTime);

			replaceData.Add("TEST END TIME", testEndTime);

			replaceData.Add("TOTAL CHIP", totalChip);

			replaceData.Add("OK CHIP", okChip);

			replaceData.Add("NG CHIP", ngChip);

			this.ReplaceReport2(replaceData);

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
		{
			string line = string.Empty;
            
			for (int i = 0; i < this._report2Item.Count;i++ )
			{
				string keyName = this._report2Item[i];

				string format = string.Empty;

				TestResultData result = this.UISetting.UserDefinedData[keyName];

				if (result != null)
				{
					format = result.Formate;
				}

				if (data.ContainsKey(keyName))
				{
					line += data[keyName].ToString(format);
				}

				if (i < this._report2Item.Count - 1)
				{
					line += ",";
				}
			}

			this.WriteLine2(line);

			return EErrorCode.NONE;
		}

		protected override EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
		{
			return base.MoveWAFFileToTarget(cmd);
        }

        // NTLM (IVSweep)
        protected override EErrorCode WriteReportHeadByUser3()
        {
            if (this.UISetting.FormatName != NTLM_FLASH_FORMAT_NAME)
            {
                return EErrorCode.NONE;
            }
            
            if (!this.UISetting.IsEnableSweepPath)
            {
                return EErrorCode.NONE;
            }

            this.OpenWriter();

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser3(Dictionary<string, double> data)
        {
            if (this.UISetting.FormatName != NTLM_FLASH_FORMAT_NAME)
            {
                return EErrorCode.NONE;
            }

            if (!this.UISetting.IsEnableSweepPath)
            {
                return EErrorCode.NONE;
            }

            if (this._sw1 == null)
            {
                this._sw1 = new StreamWriter(this.sweepFileFullNameTmp1, true, Encoding.Default);
            }

            if (this._sw2 == null)
            {
                this._sw2 = new StreamWriter(this.sweepFileFullNameTmp2, true, Encoding.Default);
            }

            if (this._sw3 == null)
            {
                this._sw3 = new StreamWriter(this.sweepFileFullNameTmp3, true, Encoding.Default);
            }

            if (this._sw4 == null)
            {
                this._sw4 = new StreamWriter(this.sweepFileFullNameTmp4, true, Encoding.Default);
            }

            string test = data["TEST"].ToString();
            string col = data["COL"].ToString();
            string row = data["ROW"].ToString();

            foreach (var sweep in this.AcquireData.ElecSweepDataSet)
            {
                if (!sweep.IsEnable)
                {
                    continue;
                }

                string name = sweep.Name;

                switch (sweep.KeyName)
                {
                    case "IVSWEEP_1":
                        {
                            // Write Header Description
                            this.Writer1(string.Format("MeasureType,{0}", name));
                            this.Writer1(string.Format("Die X,{0}", col));
                            this.Writer1(string.Format("Die Y,{0}", row));
                            this.Writer1(string.Format("Die No.,{0}", test));
                            this.Writer1(string.Format("Bin,255"));
                            this.Writer1(string.Format("{0}.Voltage(V),{1}.Current(A)", name, name));

                            for (int i = 0; i < sweep.ApplyData.Length; i++)
                            {
                                this.Writer1(string.Format("{0},{1}", sweep.SweepData[i].ToString("0.0000"), sweep.ApplyData[i].ToString("0.0000")));
                            }

                            this.Writer1("");

                            break;
                        }
                    case "IVSWEEP_2":
                        {
                            // Write Header Description
                            this.Writer2(string.Format("MeasureType,{0}", name));
                            this.Writer2(string.Format("Die X,{0}", col));
                            this.Writer2(string.Format("Die Y,{0}", row));
                            this.Writer2(string.Format("Die No.,{0}", test));
                            this.Writer2(string.Format("Bin,255"));
                            this.Writer2(string.Format("{0}.Voltage(V),{1}.Current(A)", name, name));

                            for (int i = 0; i < sweep.ApplyData.Length; i++)
                            {
                                this.Writer2(string.Format("{0},{1}", sweep.SweepData[i].ToString("0.0000"), sweep.ApplyData[i].ToString("0.0000")));
                            }

                            this.Writer2("");

                            break;
                        }
                    case "IVSWEEP_3":
                        {
                            // Write Header Description
                            this.Writer3(string.Format("MeasureType,{0}", name));
                            this.Writer3(string.Format("Die X,{0}", col));
                            this.Writer3(string.Format("Die Y,{0}", row));
                            this.Writer3(string.Format("Die No.,{0}", test));
                            this.Writer3(string.Format("Bin,255"));
                            this.Writer3(string.Format("{0}.Voltage(V),{1}.Current(A)", name, name));

                            for (int i = 0; i < sweep.ApplyData.Length; i++)
                            {
                                this.Writer3(string.Format("{0},{1}", sweep.SweepData[i].ToString("0.0000"), sweep.ApplyData[i].ToString("0.0000")));
                            }

                            this.Writer3("");

                            break;
                        }
                    case "IVSWEEP_4":
                        {
                            // Write Header Description
                            this.Writer4(string.Format("MeasureType,{0}", name));
                            this.Writer4(string.Format("Die X,{0}", col));
                            this.Writer4(string.Format("Die Y,{0}", row));
                            this.Writer4(string.Format("Die No.,{0}", test));
                            this.Writer4(string.Format("Bin,255"));
                            this.Writer4(string.Format("{0}.Voltage(V),{1}.Current(A)", name, name));

                            for (int i = 0; i < sweep.ApplyData.Length; i++)
                            {
                                this.Writer4(string.Format("{0},{1}", sweep.SweepData[i].ToString("0.0000"), sweep.ApplyData[i].ToString("0.0000")));
                            }

                            this.Writer4("");

                            break;
                        }
                }
            }

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser3()
        {
            if (this.UISetting.FormatName != NTLM_FLASH_FORMAT_NAME)
            {
                return EErrorCode.NONE;
            }
            
            if (!this.UISetting.IsEnableSweepPath)
            {
                return EErrorCode.NONE;
            }

            return EErrorCode.NONE;
        }

        protected override EErrorCode MoveFileToTargetByUser3(EServerQueryCmd cmd)
        {
            if (this.UISetting.FormatName != NTLM_FLASH_FORMAT_NAME)
            {
                return EErrorCode.NONE;
            }
            
            if (!this.UISetting.IsEnableSweepPath)
            {
                return EErrorCode.NONE;
            }

            if (this.AcquireData.ElecSweepDataSet.Count == 0)
            {
                this.CloseWriter();

                return EErrorCode.NONE;
            }

            string outPath = this.UISetting.SweepOutputPath;

            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt1 = fileNameWithoutExt + "_NTLM1.SWP";
            string fileNameWithExt2 = fileNameWithoutExt + "_NTLM2.SWP";
            string fileNameWithExt3 = fileNameWithoutExt + "_NTLM3.SWP";
            string fileNameWithExt4 = fileNameWithoutExt + "_NTLM4.SWP";

            string outputPathAndFile1 = Path.Combine(outPath, fileNameWithExt1);
            string outputPathAndFile2 = Path.Combine(outPath, fileNameWithExt2);
            string outputPathAndFile3 = Path.Combine(outPath, fileNameWithExt3);
            string outputPathAndFile4 = Path.Combine(outPath, fileNameWithExt4);

            MPIFile.CopyFile(this.sweepFileFullNameTmp1, outputPathAndFile1);
            MPIFile.CopyFile(this.sweepFileFullNameTmp2, outputPathAndFile2);
            MPIFile.CopyFile(this.sweepFileFullNameTmp3, outputPathAndFile3);
            MPIFile.CopyFile(this.sweepFileFullNameTmp4, outputPathAndFile4);

            this.CloseWriter();

            return EErrorCode.NONE;
        }
        #endregion
    }
}
