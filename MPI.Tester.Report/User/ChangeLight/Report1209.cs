using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using MPI.Tester.TestServer;
using System.IO;

namespace MPI.Tester.Report.User.ChangeLight
{
	class Report : ReportBase
	{
		private Dictionary<string, string> _calData;
		private Dictionary<string, EESDMode> _esdResultKeyNameAndMode;
		private Dictionary<string, int> _esdResultKeyNameAndZapValue;
        private Dictionary<string, bool> _isTestItemEnableResultKeyName;
        private Dictionary<string, bool> _isTested;

		private Dictionary<string, string> _esdTestItem;
		private Dictionary<string, double> _esdResultKeyNameAndPassDie;
        private Dictionary<string, double> _esdResultKeyNameAndTestCount;

		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
            this._isTested = new Dictionary<string, bool>();

			this._esdResultKeyNameAndMode = new Dictionary<string, EESDMode>();

			this._esdResultKeyNameAndZapValue = new Dictionary<string, int>();

            this._isTestItemEnableResultKeyName = new Dictionary<string, bool>();

			this._esdTestItem = new Dictionary<string, string>();

			for (int i = 0; i < 32; i++)
			{
				this._esdTestItem.Add("ESD_" + (i + 1).ToString(), "ESD" + (i + 1).ToString());
			}
		}

		private void SetData()
		{
			this._calData = new Dictionary<string, string>();

			if (this.Product != null && this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null)
			{
				string testCondition = string.Empty;

				for (int i = 0; i < this.Product.TestCondition.TestItemArray.Length; i++)
				{
					TestItemData item = this.Product.TestCondition.TestItemArray[i];

					if (item is VRTestItem || item is IZTestItem || item is IFTestItem)
					{
						string forceValue = "0.0";

						string forceTime = "0";

						string minLimitValue = "0.0";

						string maxLimitValue = "0.0";

						if (Math.Abs(item.ElecSetting[0].ForceValue) != 0)
						{
							forceValue = Math.Abs(item.ElecSetting[0].ForceValue).ToString();
						}

						if (item.ElecSetting[0].ForceTime != 0)
						{
							forceTime = item.ElecSetting[0].ForceTime.ToString();
						}

						if (item.MsrtResult[0].MinLimitValue != 0)
						{
							minLimitValue = item.MsrtResult[0].MinLimitValue.ToString();
						}

						if (item.MsrtResult[0].MaxLimitValue != 0)
						{
							maxLimitValue = item.MsrtResult[0].MaxLimitValue.ToString();
						}

						testCondition += item.Name + "@" + forceValue + "," + forceTime + "," + minLimitValue + "," + maxLimitValue;

						if (i != this.Product.TestCondition.TestItemArray.Length - 1)
						{
							testCondition += ";";
						}

						this._calData.Add(item.MsrtResult[0].KeyName + "_G", item.GainOffsetSetting[0].Gain.ToString());

						this._calData.Add(item.MsrtResult[0].KeyName + "_O", item.GainOffsetSetting[0].Offset.ToString());
					}
					else if (item is LOPWLTestItem)
					{
						foreach (var cal in item.GainOffsetSetting)
						{
							this._calData.Add(cal.KeyName + "_G", cal.Gain.ToString());

							this._calData.Add(cal.KeyName + "_O", cal.Offset.ToString());
						}
					}
				}

				this._calData.Add("testCondition", testCondition);
			}
		}

		private string GetData(string key)
		{
			if (!this._calData.ContainsKey(key))
			{
				if (key.Contains("_G"))
				{
					return "1.0";
				}
				else if (key.Contains("_O"))
				{
					return "0.0";
				}

				return string.Empty;
			}

			return this._calData[key];
		}

		protected override void SetResultTitle()
		{
            this._esdResultKeyNameAndPassDie = new Dictionary<string, double>();

            this._esdResultKeyNameAndTestCount = new Dictionary<string, double>();

			this.ResultTitleInfo.Clear();

			this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
		}

		protected override EErrorCode WriteReportHeadByUser()
		{
			this.SetData();

			string calData = string.Empty;

			calData += "AmpGain:0.0;";

			calData += "LOPGain:" + this.GetData("WATT_1_G") + ";";
			calData += "LopOffset:" + this.GetData("WATT_1_O") + ";";
			calData += "Lop2Gain:" + this.GetData("WATT_2_G") + ";";
			calData += "Lop2Offset:" + this.GetData("WATT_2_O") + ";";
			calData += "Lop5Gain:" + this.GetData("WATT_5_G") + ";";
			calData += "Lop5Offset:" + this.GetData("WATT_5_O") + ";";

			calData += "VF1Offset:" + this.GetData("MVF_1_O") + ";";
			calData += "VF2Offset:" + this.GetData("MVF_2_O") + ";";
			calData += "VF3Offset:" + this.GetData("MVF_3_O") + ";";
			calData += "VF4Offset:" + this.GetData("MVF_4_O") + ";";
			calData += "VF5Offset:" + this.GetData("MVF_5_O") + ";";
			calData += "IROffset:" + this.GetData("MIR_1_O") + ";";

			calData += "WpOffset:" + this.GetData("WLP_1_O") + ";";
			calData += "WdOffset:" + this.GetData("WLD_1_O") + ";";
			calData += "WcOffset:" + this.GetData("WLC_1_O") + ";";
			calData += "Wp2Offset:" + this.GetData("WLP_2_O") + ";";
			calData += "Wd2Offset:" + this.GetData("WLD_2_O") + ";";
			calData += "Wp5Offset:" + this.GetData("WLP_5_O") + ";";
			calData += "Wd5Offset:" + this.GetData("WLD_5_O");

			this.WriteLine("FileName," + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt);

			this.WriteLine("TestStartTime," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

			this.WriteLine("TestEndTime," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm"));

			this.WriteLine("TesterNumber," + this.UISetting.MachineName);

			this.WriteLine("TesterModel,T200");

			this.WriteLine("Specification," + this.UISetting.TaskSheetFileName);

			this.WriteLine("TotalScan," + this.UISetting.TotalSacnCounts);

			this.WriteLine("TotalTested,");

            this.WriteLine("LotNumber,");

			this.WriteLine("Operator," + this.UISetting.OperatorName);

			this.WriteLine("Test Condition,\"" + this.GetData("testCondition") + "\"");

			this.WriteLine("CalData,\"" + calData + "\"");

			this.WriteLine("");

			this.WriteLine("");

			this.WriteLine("");

			this.WriteLine("");

			this.WriteLine("");

			this.WriteLine("");

			this.WriteLine("");

			this.WriteLine(this.ResultTitleInfo.TitleStr);           

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string endTestTime = "TestEndTime," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm");

			replaceData.Add("TestEndTime,", endTestTime);

			string testCount = "TotalTested," + this.TotalTestCount.ToString();

			replaceData.Add("TotalTested,", testCount);

			this.ReplaceReport(replaceData);

			return EErrorCode.NONE;
		}

        protected override EErrorCode PushDataByUser(Dictionary<string, float> data)
        {
			//======================================================
			// Get TestItem Info
			//======================================================
            this._isTestItemEnableResultKeyName.Clear();

            this._isTested.Clear();

			this._esdResultKeyNameAndMode.Clear();

			this._esdResultKeyNameAndZapValue.Clear();

			bool isEnableESD = false;

			if (this.Product != null && this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null)
			{
				foreach (var item in this.Product.TestCondition.TestItemArray)
				{
					foreach (var msrt in item.MsrtResult)
					{
                        this._isTestItemEnableResultKeyName.Add(msrt.KeyName, item.IsEnable);

                        this._isTested.Add(msrt.KeyName, msrt.IsTested);
					}

					if (item is ESDTestItem)
					{
						this._esdResultKeyNameAndMode.Add(item.MsrtResult[0].KeyName, (item as ESDTestItem).EsdSetting.Mode);

                        this._esdResultKeyNameAndZapValue.Add(item.MsrtResult[0].KeyName, (item as ESDTestItem).EsdSetting.ZapVoltage);

						if ((item as ESDTestItem).EsdSetting.Polarity == EESDPolarity.N)
						{
							this._esdResultKeyNameAndZapValue[item.MsrtResult[0].KeyName] *= -1;
						}

						isEnableESD = item.IsEnable;
					}
				}
			}

			//======================================================
			// Get AcquireData Info
			//======================================================
			bool isHBMIsTester = false;

			bool isMMIsTester = false;

			string lastPassHBMZapValue = string.Empty;

            string lastPassMMZapValue = string.Empty;

            foreach (var item in this.AcquireData.OutputTestResult)
            {
                if (!item.KeyName.Contains("MIRJ") || !item.IsTested)
                {
                    continue;
                }

                if (this._esdResultKeyNameAndMode[item.KeyName] == EESDMode.HBM)
                {
                    isHBMIsTester = true;

                    if (!this._esdResultKeyNameAndTestCount.ContainsKey(item.KeyName))
                    {
                        this._esdResultKeyNameAndTestCount.Add(item.KeyName, 0);
                    }

                    this._esdResultKeyNameAndTestCount[item.KeyName]++;

                    if (!this._esdResultKeyNameAndPassDie.ContainsKey(item.KeyName))
                    {
                        this._esdResultKeyNameAndPassDie.Add(item.KeyName, 0);
                    }

                    if (item.IsPass)
                    {
                        lastPassHBMZapValue = this._esdResultKeyNameAndZapValue[item.KeyName].ToString();

                        this._esdResultKeyNameAndPassDie[item.KeyName]++;
                    }
                }
                else
                {
                    isMMIsTester = true;

                    if (!this._esdResultKeyNameAndTestCount.ContainsKey(item.KeyName))
                    {
                        this._esdResultKeyNameAndTestCount.Add(item.KeyName, 0);
                    }

                    if (!this._esdResultKeyNameAndPassDie.ContainsKey(item.KeyName))
                    {
                        this._esdResultKeyNameAndPassDie.Add(item.KeyName, 0);
                    }

                    this._esdResultKeyNameAndTestCount[item.KeyName]++;

                    if (item.IsPass)
                    {
                        lastPassMMZapValue = this._esdResultKeyNameAndZapValue[item.KeyName].ToString();

                        this._esdResultKeyNameAndPassDie[item.KeyName]++;
                    }
                }
            }

			//======================================================
			// Push Data
			//======================================================
            int index = 0;

            string line = string.Empty;

            foreach (var item in this.ResultTitleInfo)
            {
                if (this._isTested.ContainsKey(item.Key) && !this._isTested[item.Key])
                { 
                
                }
				else if (item.Key == "MESDHBM_1" &&  isHBMIsTester)
				{
					line += lastPassHBMZapValue.ToString();
				}
				else if (item.Key == "MESDMM_1" && isMMIsTester)
				{
					line += lastPassMMZapValue.ToString();
				}
                else if (data.ContainsKey(item.Key))
                {
                    string format = string.Empty;

                    if (this.ResultData.ContainsKey(item.Key))
                    {
                        format = this.ResultData[item.Key].Formate;
                    }

                    line += data[item.Key].ToString(format);
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

		protected override EErrorCode WriteReportHeadByUser2()
		{
			string calData = string.Empty;

			calData += "AmpGain:0.0;";

			calData += "LOPGain:" + this.GetData("WATT_1_G") + ";";
			calData += "LopOffset:" + this.GetData("WATT_1_O") + ";";
			calData += "Lop2Gain:" + this.GetData("WATT_2_G") + ";";
			calData += "Lop2Offset:" + this.GetData("WATT_2_O") + ";";
			calData += "Lop5Gain:" + this.GetData("WATT_5_G") + ";";
			calData += "Lop5Offset:" + this.GetData("WATT_5_O") + ";";

			calData += "VF1Offset:" + this.GetData("MVF_1_O") + ";";
			calData += "VF2Offset:" + this.GetData("MVF_2_O") + ";";
			calData += "VF3Offset:" + this.GetData("MVF_3_O") + ";";
			calData += "VF4Offset:" + this.GetData("MVF_4_O") + ";";
			calData += "VF5Offset:" + this.GetData("MVF_5_O") + ";";
			calData += "IROffset:" + this.GetData("MIR_1_O") + ";";

			calData += "WpOffset:" + this.GetData("WLP_1_O") + ";";
			calData += "WdOffset:" + this.GetData("WLD_1_O") + ";";
			calData += "WcOffset:" + this.GetData("WLC_1_O") + ";";
			calData += "Wp2Offset:" + this.GetData("WLP_2_O") + ";";
			calData += "Wd2Offset:" + this.GetData("WLD_2_O") + ";";
			calData += "Wp5Offset:" + this.GetData("WLP_5_O") + ";";
			calData += "Wd5Offset:" + this.GetData("WLD_5_O");

			this.WriteLine2("FileName," + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt);

			this.WriteLine2("TestStartTime," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

			this.WriteLine2("TestEndTime," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm"));

			this.WriteLine2("TesterNumber," + this.UISetting.MachineName);

			this.WriteLine2("TesterModel,T200");

			this.WriteLine2("Specification," + this.UISetting.TaskSheetFileName);

			this.WriteLine2("TotalScan," + this.UISetting.TotalSacnCounts);

			this.WriteLine2("TotalTested,");

			this.WriteLine2("LotNumber,");

			this.WriteLine2("Operator," + this.UISetting.OperatorName);

			this.WriteLine2("Test Condition,\"" + this.GetData("testCondition") + "\"");

			this.WriteLine2("CalData,\"" + calData + "\"");

			this.WriteLine2("");

			this.WriteLine2("");

			this.WriteLine2("");

			string esdInfo = string.Empty;

			foreach (var item in this._esdTestItem)
			{
				esdInfo += "," + item.Value;
			}

			this.WriteLine2(esdInfo);

			string mode = "Mode";

			string zapValue = "Voltage";

			if (this.Product != null && this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null)
			{
				foreach (var esdItem in this._esdTestItem)
				{
                    mode += ",";

                    zapValue += ",";

					foreach (var item in this.Product.TestCondition.TestItemArray)
					{
						if (esdItem.Key == item.KeyName)
						{
							mode += (item as ESDTestItem).EsdSetting.Mode.ToString();

							int value = (item as ESDTestItem).EsdSetting.ZapVoltage;

                            if ((item as ESDTestItem).EsdSetting.Polarity == EESDPolarity.N)
                            {
                                value *= -1;
                            }

                            zapValue += value.ToString();
						}
					}
				}
			}

			this.WriteLine2(mode);

			this.WriteLine2(zapValue);

			this.WriteLine2("YieldLine1");

			this.WriteLine2("");

			this.WriteLine2("");

			this.WriteLine2("");

			string title = "TEST,X,Y,IR0";

			foreach (var item in this._esdTestItem)
			{
				string num = item.Value.Replace("ESD", "");

				title += "," + item.Value + ",IR" + num.ToString() + ",PASS" + num.ToString();
			}

			this.WriteLine2(title);

			this.WriteLine2("YieldLine2");

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser2()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string endTestTime = "TestEndTime," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm");

			replaceData.Add("TestEndTime,", endTestTime);

			string testCount = "TotalTested," + this.TotalTestCount.ToString();

			replaceData.Add("TotalTested,", testCount);

			string yieldLine1 = "Yield";			

			foreach (var item in this._esdTestItem)
			{
				string resultKeyName = item.Key.Replace("ESD", "MIRJ");

				yieldLine1 += ",";

				if (this._esdResultKeyNameAndPassDie.ContainsKey(resultKeyName))
				{
					double yield = this._esdResultKeyNameAndPassDie[resultKeyName] / this._esdResultKeyNameAndTestCount[resultKeyName] * 100.0d;

					yieldLine1 += yield.ToString("0.00") + "%";
				}
			}

			replaceData.Add("YieldLine1", yieldLine1);

			string yieldLine2 = "0,,,";

			if (this.Product != null && this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null)
			{
				foreach (var esdItem in this._esdTestItem)
				{
					bool isHasTestItem = false;

                    string mode = string.Empty;

					foreach (var item in this.Product.TestCondition.TestItemArray)
					{
                        if (item is ESDTestItem)
                        {
                            string resultKeyName = (item as ESDTestItem).MsrtResult[0].KeyName;

                            if (esdItem.Key == item.KeyName && item.IsEnable)
                            {
                                mode = (item as ESDTestItem).EsdSetting.Mode.ToString();

                                if (this._esdResultKeyNameAndPassDie.ContainsKey(resultKeyName))
                                {
                                    isHasTestItem = true;

                                    double yield = this._esdResultKeyNameAndPassDie[resultKeyName] / this._esdResultKeyNameAndTestCount[resultKeyName] * 100.0d;

                                    yieldLine2 += "," + mode + ",," + yield.ToString("0.00") + "%";;
                                }
                            }
                        }
					}

					if (!isHasTestItem)
					{
                        yieldLine2 += "," + mode + ",,";
					}
				}
			}

			replaceData.Add("YieldLine2", yieldLine2);

            this.ReplaceReport2(replaceData);

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser2(Dictionary<string, float> data)
		{
			string line = data["TEST"].ToString();

			line += "," + data["COL"].ToString();

			line += "," + data["ROW"].ToString();

			line += ",";

			if (data.ContainsKey("MIR_1"))
			{
				string format = "";

				if (this.ResultData.ContainsKey("MIR_1"))
				{
					format = this.ResultData["MIR_1"].Formate;
				}

                if (this._isTested.ContainsKey("MIR_1") && this._isTested["MIR_1"])
                {
                    line += data["MIR_1"].ToString(format);
                }
			}

			foreach (var item in this._esdTestItem)
			{
				string key = item.Key.Replace("ESD", "MIRJ");

				bool isTested = false;

                bool isShowZapValue = false; 

                string zapValue = string.Empty;

                foreach (var result in this.AcquireData.OutputTestResult)
				{
                    if (this._esdResultKeyNameAndZapValue.ContainsKey(key) && key == result.KeyName)
					{
						zapValue = this._esdResultKeyNameAndZapValue[key].ToString();

						string judgeIR = data[key].ToString();

						string isPass = result.IsPass ? "1" : "0";

                        if (result.IsTested)
                        {
                            isTested = true;

                            line += "," + zapValue + "," + judgeIR + "," + isPass;
                        }

                        if (this._isTestItemEnableResultKeyName.ContainsKey(result.KeyName))
                        {
                            if (this._isTestItemEnableResultKeyName[result.KeyName])
                            {
                                isShowZapValue = !result.IsGroupSkip;
                            }
                        }

                        break;
					}
				}

                if (!isTested)
				{
                    if (isShowZapValue)
                    {
                        line += "," + zapValue + ",,";
                    }
                    else
                    {
                        line += ",,,";
                    }
				}
			}

			this.WriteLine2(line);

			return EErrorCode.NONE;
		}

        protected override EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
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
                isOutputPath02 = this.UISetting.IsEnableManualPath02;

                isOutputPath03 = this.UISetting.IsEnableManualPath03;

                outPath01 = this.UISetting.ManualOutputPath01;

                outPath02 = this.UISetting.ManualOutputPath02;

                outPath03 = this.UISetting.ManualOutputPath03;

                type01 = this.UISetting.ManualOutputPathType01;

                type02 = this.UISetting.ManualOutputPathType02;

                type03 = this.UISetting.ManualOutputPathType03;
            }
            else
            {
                isOutputPath02 = this.UISetting.IsEnablePath02;

                isOutputPath03 = this.UISetting.IsEnablePath03;

                outPath01 = this.UISetting.TestResultPath01;

                outPath02 = this.UISetting.TestResultPath02;

                outPath03 = this.UISetting.TestResultPath03;

                type01 = this.UISetting.TesterResultCreatFolderType01;

                type02 = this.UISetting.TesterResultCreatFolderType02;

                type03 = this.UISetting.TesterResultCreatFolderType03;
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
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt() + "_ESD";

            string fileNameWithExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.TestResultFileExt;

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

        public override string TestResultFileNameWithoutExt()
        {
            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            foreach (var chr in invalidFileChars)
            {
                if (this.UISetting.WeiminUIData.OutputFileName.Contains(chr))
                {
                    this.UISetting.WeiminUIData.OutputFileName = this.UISetting.WeiminUIData.OutputFileName.Replace(chr.ToString(), "");
                }
            }

            if (this.UISetting.WeiminUIData.OutputFileName == "")
            {
                return DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileName = string.Empty;

            if (this.UISetting.WeiminUIData.WMTestMode == 0)
            {
                fileName = this.UISetting.WeiminUIData.OutputFileName + "_COT";
            }
			else if (this.UISetting.WeiminUIData.WMTestMode == 1)
            {
                fileName = this.UISetting.WeiminUIData.OutputFileName + "_COW";
            }
            else
            {
                string startTestTime = this.TesterSetting.StartTestTime.ToString("yyddHHmm");

                byte[] byteMonth = BitConverter.GetBytes(this.TesterSetting.StartTestTime.Month);

                startTestTime = startTestTime.Insert(2, byteMonth[0].ToString("X"));

                fileName = this.UISetting.WeiminUIData.OutputFileName + "#" + startTestTime;
            }

            return fileName;
        }

        public override bool IsOutputReportExist()
        {
            bool exist = false;

            string outPath01 = string.Empty;

            string outPath02 = string.Empty;

            string outPath03 = string.Empty;

            string fileNameWithExt = this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt;

            if (this.UISetting.IsManualRunMode)
            {
                outPath01 = this.UISetting.ManualOutputPath01;

                outPath02 = this.UISetting.ManualOutputPath02;

                outPath03 = this.UISetting.ManualOutputPath03;

                if (this.UISetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath01 = Path.Combine(outPath01, this.UISetting.LotNumber);
                }
                else if (this.UISetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath01 = Path.Combine(outPath01, this.UISetting.MachineName);
                }
                else if (this.UISetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (this.UISetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath02 = Path.Combine(outPath02, this.UISetting.LotNumber);
                }
                else if (this.UISetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath02 = Path.Combine(outPath02, this.UISetting.MachineName);
                }
                else if (this.UISetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (this.UISetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath03 = Path.Combine(outPath03, this.UISetting.LotNumber);
                }
                else if (this.UISetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath03 = Path.Combine(outPath03, this.UISetting.MachineName);
                }
                else if (this.UISetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                outPath01 = Path.Combine(outPath01, fileNameWithExt);

                outPath02 = Path.Combine(outPath02, fileNameWithExt);

                outPath03 = Path.Combine(outPath03, fileNameWithExt);

                if (File.Exists(outPath01) && this.UISetting.IsEnableManualPath01)
                {
                    exist = true;
                }

                if (this.UISetting.IsRunDailyCheckMode)
                {

                    exist = false;
                }
            }
            else
            {
                outPath01 = this.UISetting.TestResultPath01;

                outPath02 = this.UISetting.TestResultPath02;

                outPath03 = this.UISetting.TestResultPath03;

                if (this.UISetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath01 = Path.Combine(outPath01, this.UISetting.LotNumber);
                }
                else if (this.UISetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath01 = Path.Combine(outPath01, this.UISetting.MachineName);
                }
                else if (this.UISetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (this.UISetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath02 = Path.Combine(outPath02, this.UISetting.LotNumber);
                }
                else if (this.UISetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath02 = Path.Combine(outPath02, this.UISetting.MachineName);
                }
                else if (this.UISetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (this.UISetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath03 = Path.Combine(outPath03, this.UISetting.LotNumber);
                }
                else if (this.UISetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath03 = Path.Combine(outPath03, this.UISetting.MachineName);
                }
                else if (this.UISetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                outPath01 = Path.Combine(outPath01, fileNameWithExt);

                outPath02 = Path.Combine(outPath02, fileNameWithExt);

                outPath03 = Path.Combine(outPath03, fileNameWithExt);

                if (File.Exists(outPath01) && this.UISetting.IsEnablePath01)
                {
                    exist = true;
                }
                else if (File.Exists(outPath02) && this.UISetting.IsEnablePath02)
                {
                    exist = true;
                }
                else if (File.Exists(outPath03) && this.UISetting.IsEnablePath03)
                {
                    exist = true;
                }
            }

            return exist;
        }
	}
}
