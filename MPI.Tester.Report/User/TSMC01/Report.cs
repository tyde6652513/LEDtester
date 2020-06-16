using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;
using MPI.Tester.TestServer;

namespace MPI.Tester.Report.User.TSMC01
{
	public class Report : ReportBase
	{
		private int _maxLIVCount;

		private List<string> _trEnableResuleKeyName;

		private List<string> _colRow;

		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
			this._colRow = new List<string>();

			this.SetLIVResultTitle();

			this.GetMaxLIVCount();

			this._isImplementLIVReport = true;

			this._isImplementSpectrumReport = true;
		}

		private void SetLIVResultTitle()
		{
			this._trEnableResuleKeyName = new List<string>();

			for (int i = 1; i < 11; i++)
			{
				//this._livEnableResuleKeyName.Add("LIVTIMEB_" + i.ToString());

				//this._livEnableResuleKeyName.Add("LIVSETVALUE_" + i.ToString());

				//Elec
				this._trEnableResuleKeyName.Add("TRMsrtDrainV_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRMsrtDrainI_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRMsrtSourceV_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRMsrtSourceI_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRMsrtGateV_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRMsrtGateI_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRMsrtBlukV_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRMsrtBlukI_" + i.ToString());

				//LOPWL
				this._trEnableResuleKeyName.Add("TRWATT_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRWLP_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRWLD_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRWLC_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRST_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRINT_" + i.ToString());

				//PD
				this._trEnableResuleKeyName.Add("TRPDWATT_" + i.ToString());

				this._trEnableResuleKeyName.Add("TRPDCURRENT_" + i.ToString());

				//this._livEnableResuleKeyName.Add("LIVPDWATT_" + i.ToString());
			}
		}

		private void GetMaxLIVCount()
		{
			foreach (var livNum in this.AcquireData.LIVDataSet)
			{
				if (!livNum.IsEnable)
				{
					continue;
				}

				foreach (var livData in livNum)
				{
					if (this._maxLIVCount < livData.DataArray.Length)
					{
						this._maxLIVCount = livData.DataArray.Length;
					}
				}
			}
		}

		private void ClearTempFile()
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo(Constants.Paths.MPI_TEMP_DIR2);

				List<FileInfo> fiList = new List<FileInfo>();

				fiList.AddRange(di.GetFiles("*.tmp"));

				fiList.AddRange(di.GetFiles("*.tmp2"));

				fiList.AddRange(di.GetFiles("*.tmp3"));

				fiList.AddRange(di.GetFiles("*.rep"));

				fiList.AddRange(di.GetFiles("*.rep2"));

				fiList.AddRange(di.GetFiles("*.rep3"));

				foreach (var fi in fiList)
				{
					MPIFile.DeleteFile(fi.FullName);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("[Report TSMC01], ClearTempFile(), " + e.ToString());
			}
		}

		protected override void SetResultTitle()
		{
			this.ClearTempFile();

			//this.SetResultTitleByDefault();
		}

		protected override EErrorCode WriteReportHeadByUser()
		{
			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////
			this.WriteLine("FileName,\"" + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt + "\"");

			this.WriteLine("UserID,\"" + (int)this.UISetting.UserID + "_" + this.UISetting.FormatName + "\"");

			this.WriteLine("TestTime,\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss") + "\"");

			this.WriteLine("EndTime,\"\"");

			this.WriteLine("TesterModel,\"" + this.MachineInfo.TesterModel + "/" + this.MachineInfo.TesterSN + "\"");

			this.WriteLine("MachineName,\"" + this.UISetting.MachineName + "\"");

			this.WriteLine("LotNumber,\"" + this.UISetting.LotNumber + "\"");

			this.WriteLine("TaskFile,\"" + this.UISetting.TaskSheetFileName + "\"");

			//this.WriteLine("Filter Wheel,\"" + this.Product.ProductFilterWheelPos.ToString() + "\"");

			this.WriteLine("Operator,\"" + this.UISetting.OperatorName + "\"");

			this.WriteLine("Samples,\"\"");

			this.WriteLine("");

			string title = "TEST,COL,ROW";

			foreach (var resultKey in _trEnableResuleKeyName)
			{
				foreach (var livNum in this.AcquireData.LIVDataSet)
				{
					if (!livNum.IsEnable)
					{
						continue;
					}

					foreach (var livData in livNum)
					{
						if (livData.KeyName != resultKey)
						{
							continue;
						}

						if (livData.Unit == "")
						{
							title += "," + livData.Name;
						}
						else
						{
							title += "," + livData.Name + "(" + livData.Unit + ")";
						}
					}
				}
			}

			this.WriteLine(title);

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
			if (!File.Exists(this.FileFullNameTmp))
			{
				return EErrorCode.NONE;
			}

			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string endTime = "EndTime,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm:ss") + "\"";

			string samples = "Samples,\"" + this._colRow.Count + "\"";

			replaceData.Add("EndTime,\"\"", endTime);

			replaceData.Add("Samples,\"\"", samples);

			using (StreamReader sr = new StreamReader(this.FileFullNameTmp, Encoding.Default))
			{
				using (StreamWriter sw = new StreamWriter(this.FileFullNameRep))
				{
					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();

						if (replaceData.ContainsKey(line))
						{
							line = replaceData[line];
						}

						sw.WriteLine(line);
					}
				}
			}

			return EErrorCode.NONE;
		}

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
		{
			string test = data["TEST"].ToString();

			string col = data["COL"].ToString();

			string row = data["ROW"].ToString();

			string key = "X" + col + "Y" + row;

			if (!this._colRow.Contains(key))
			{
				this._colRow.Add(key);
			}

			if (this._maxLIVCount == 0)
			{
				return EErrorCode.NONE;
			}

			for (int resultCount = 0; resultCount < this._maxLIVCount; resultCount++)
			{
				string line = test + "," + col + "," + row;

				foreach (var resultKey in _trEnableResuleKeyName)
				{
					foreach (var livNum in this.AcquireData.LIVDataSet)
					{
						if (!livNum.IsEnable)
						{
							continue;
						}

						foreach (var livData in livNum)
						{
							if (livData.KeyName != resultKey)
							{
								continue;
							}

							line += ",";

							if (livData.DataArray.Length > resultCount)
							{
								line += livData.DataArray[resultCount].ToString(livData.Formate);
							}
						}
					}
				}

				this.WriteLine(line);
			}

			return EErrorCode.NONE;
		}

		private Dictionary<string, List<string>> _drainForceValue;
		private Dictionary<string, List<string>> _sourceForceValue;
		private Dictionary<string, List<string>> _gateForceValue;
		private Dictionary<string, List<string>> _blukForceValue;

		private double[] _wavelength;

		private void GetTRForceValue()
		{
			this._drainForceValue = new Dictionary<string, List<string>>();

			this._sourceForceValue = new Dictionary<string, List<string>>();

			this._gateForceValue = new Dictionary<string, List<string>>();

			this._blukForceValue = new Dictionary<string, List<string>>();

			if (this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{
					if (!(testItem is TransistorTestItem) || !testItem.IsEnable)
					{
						continue;
					}

					TransistorTestItem trItem = testItem as TransistorTestItem;

					List<string> drainFouceValue = new List<string>();

					List<string> sourceFouceValue = new List<string>();

					List<string> gateFouceValue = new List<string>();

					List<string> blukFouceValue = new List<string>();

					for (int i = 0; i < trItem.ElecSetting.Length; i++)
					{
						string drainUnit = trItem.ElecSetting[i].ElecTerminalSetting[0].ForceUnit.ToString();

						string sourceUnit = trItem.ElecSetting[i].ElecTerminalSetting[1].ForceUnit.ToString();

						string gateUnit = trItem.ElecSetting[i].ElecTerminalSetting[2].ForceUnit.ToString();

						string blukUnit = trItem.ElecSetting[i].ElecTerminalSetting[3].ForceUnit.ToString();

						if (trItem.ElecSetting[i].ElecTerminalSetting[0].SMU == DeviceCommon.ESMU.None)
						{
							drainFouceValue.Add("None");
						}
						else
						{
							drainFouceValue.Add(trItem.ElecSetting[i].ElecTerminalSetting[0].ForceValue + "(" + drainUnit + ")");
						}

						if (trItem.ElecSetting[i].ElecTerminalSetting[1].SMU == DeviceCommon.ESMU.None)
						{
							sourceFouceValue.Add("None");
						}
						else
						{
							sourceFouceValue.Add(trItem.ElecSetting[i].ElecTerminalSetting[1].ForceValue + "(" + sourceUnit + ")");
						}

						if (trItem.ElecSetting[i].ElecTerminalSetting[2].SMU == DeviceCommon.ESMU.None)
						{
							gateFouceValue.Add("None");
						}
						else
						{
							gateFouceValue.Add(trItem.ElecSetting[i].ElecTerminalSetting[2].ForceValue + "(" + gateUnit + ")");
						}

						if (trItem.ElecSetting[i].ElecTerminalSetting[3].SMU == DeviceCommon.ESMU.None)
						{
							blukFouceValue.Add("None");
						}
						else
						{
							blukFouceValue.Add(trItem.ElecSetting[i].ElecTerminalSetting[3].ForceValue + "(" + blukUnit + ")");
						}

					}

					this._drainForceValue.Add(testItem.KeyName, drainFouceValue);

					this._sourceForceValue.Add(testItem.KeyName, sourceFouceValue);

					this._gateForceValue.Add(testItem.KeyName, gateFouceValue);

					this._blukForceValue.Add(testItem.KeyName, blukFouceValue);
				}
			}
		}

		protected override EErrorCode WriteReportHeadByUser2()
		{
			if (!this.UISetting.IsEnableSaveAbsoluteSpectrum)
			{
				return EErrorCode.NONE;
			}

			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////
			this.WriteLine2("FileName,\"" + this.TestResultFileNameWithoutExt() + "_Spt." + this.UISetting.TestResultFileExt + "\"");

			this.WriteLine2("UserID,\"" + (int)this.UISetting.UserID + "_" + this.UISetting.FormatName + "\"");

			this.WriteLine2("TestTime,\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss") + "\"");

			this.WriteLine2("EndTime,\"\"");

			this.WriteLine2("TesterModel,\"" + this.MachineInfo.TesterModel + "/" + this.MachineInfo.TesterSN + "\"");

			this.WriteLine2("MachineName,\"" + this.UISetting.MachineName + "\"");

			this.WriteLine2("LotNumber,\"" + this.UISetting.LotNumber + "\"");

			this.WriteLine2("TaskFile,\"" + this.UISetting.TaskSheetFileName + "\"");

			this.WriteLine2("Operator,\"" + this.UISetting.OperatorName + "\"");

			this.WriteLine2("Samples,\"\"");

			this.WriteLine2("");

			this.WriteLine2("Test Count,COL,ROW,Name,Drain,Source,Gate,Bluk,Wave Length");

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser2()
		{
			if (!this.UISetting.IsEnableSaveAbsoluteSpectrum)
			{
				return EErrorCode.NONE;
			}

			if (!File.Exists(this.FileFullNameTmp))
			{
				return EErrorCode.NONE;
			}

			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string endTime = "EndTime,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm:ss") + "\"";

			string samples = "Samples,\"" + this._colRow.Count + "\"";

			string title = "Test Count,COL,ROW,Name,Drain,Source,Gate,Bluk,Wave Length";

			if (this._wavelength != null)
			{
				foreach (var item in this._wavelength)
				{
					title += "," + item.ToString();
				}
			}

			replaceData.Add("EndTime,\"\"", endTime);

			replaceData.Add("Samples,\"\"", samples);

			replaceData.Add("Test Count,COL,ROW,Name,Drain,Source,Gate,Bluk,Wave Length", title);

			using (StreamReader sr = new StreamReader(this.FileFullNameTmp2, Encoding.Default))
			{
				using (StreamWriter sw = new StreamWriter(this.FileFullNameRep2))
				{
					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();

						if (replaceData.ContainsKey(line))
						{
							line = replaceData[line];
						}

						sw.WriteLine(line);
					}
				}
			}

			return EErrorCode.NONE;
		}

        protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
		{
			if (!this.UISetting.IsEnableSaveAbsoluteSpectrum)
			{
				return EErrorCode.NONE;
			}

			if (this._colRow.Count > this.UISetting.SaveSpectrumMaxCount)
			{
				return EErrorCode.NONE;
			}

			this.GetTRForceValue();

			string test = data["TEST"].ToString();

			string col = data["COL"].ToString();

			string row = data["ROW"].ToString();

			string testCountStr = test + "," + col + "," + row;

			foreach (var livNum in this.AcquireData.LIVDataSet)
			{
				if (!livNum.IsEnable)
				{
					continue;
				}

				string nameStr = "," + livNum.Name;

				string keyNameStr = "," + livNum.KeyName;

				List<string> drainFouceValue = this._drainForceValue[livNum.KeyName];

				List<string> sourceFouceValue = this._sourceForceValue[livNum.KeyName];

				List<string> gateFouceValue = this._gateForceValue[livNum.KeyName];

				List<string> blukFouceValue = this._blukForceValue[livNum.KeyName];

				for (int i = 0; i < drainFouceValue.Count; i++)
				{
					string forceValue = "," + drainFouceValue[i] + "," + sourceFouceValue[i] + "," + gateFouceValue[i] + "," + blukFouceValue[i];

					string absLine = testCountStr + nameStr + forceValue + ",Absoluate";

					this._wavelength = livNum.SpectrumDataData[i].Wavelength;

					for (int j = 0; j < livNum.SpectrumDataData[i].Wavelength.Length; j++)
					{
						absLine += "," + livNum.SpectrumDataData[i].Absoluate[j].ToString();
					}

					this.WriteLine2(absLine);
				}
			}

			return EErrorCode.NONE;
		}

		protected override EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
		{
			if (!this.UISetting.IsEnableSaveAbsoluteSpectrum)
			{
				return EErrorCode.NONE;
			}

			string outPath = this.UISetting.AbsoluteSpectrumPath;

			string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

			//Abort
			if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
			{
				fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
			}

			string fileNameWithExt = fileNameWithoutExt + ".abs";

			string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

			MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile);

			return EErrorCode.NONE;
		}

		protected override EErrorCode WriteReportHeadByUser3()
		{
			if (!this.UISetting.IsEnableSaveRelativeSpectrum)
			{
				return EErrorCode.NONE;
			}

			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////
			this.WriteLine3("FileName,\"" + this.TestResultFileNameWithoutExt() + "_Spt." + this.UISetting.TestResultFileExt + "\"");

			this.WriteLine3("UserID,\"" + (int)this.UISetting.UserID + "_" + this.UISetting.FormatName + "\"");

			this.WriteLine3("TestTime,\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss") + "\"");

			this.WriteLine3("EndTime,\"\"");

			this.WriteLine3("TesterModel,\"" + this.MachineInfo.TesterModel + "/" + this.MachineInfo.TesterSN + "\"");

			this.WriteLine3("MachineName,\"" + this.UISetting.MachineName + "\"");

			this.WriteLine3("LotNumber,\"" + this.UISetting.LotNumber + "\"");

			this.WriteLine3("TaskFile,\"" + this.UISetting.TaskSheetFileName + "\"");

			this.WriteLine3("Operator,\"" + this.UISetting.OperatorName + "\"");

			this.WriteLine3("Samples,\"\"");

			this.WriteLine3("");

			this.WriteLine3("Test Count,COL,ROW,Name,Drain,Source,Gate,Bluk,Wave Length");

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser3()
		{
			if (!this.UISetting.IsEnableSaveRelativeSpectrum)
			{
				return EErrorCode.NONE;
			}

			if (!File.Exists(this.FileFullNameTmp))
			{
				return EErrorCode.NONE;
			}

			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string endTime = "EndTime,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm:ss") + "\"";

			string samples = "Samples,\"" + this._colRow.Count + "\"";

			string title = "Test Count,COL,ROW,Name,Drain,Source,Gate,Bluk,Wave Length";

			if (this._wavelength != null)
			{
				foreach (var item in this._wavelength)
				{
					title += "," + item.ToString();
				}
			}

			replaceData.Add("EndTime,\"\"", endTime);

			replaceData.Add("Samples,\"\"", samples);

			replaceData.Add("Test Count,COL,ROW,Name,Drain,Source,Gate,Bluk,Wave Length", title);

			using (StreamReader sr = new StreamReader(this.FileFullNameTmp3, Encoding.Default))
			{
				using (StreamWriter sw = new StreamWriter(this.FileFullNameRep3))
				{
					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();

						if (replaceData.ContainsKey(line))
						{
							line = replaceData[line];
						}

						sw.WriteLine(line);
					}
				}
			}

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser3(Dictionary<string, double> data)
		{
			if (!this.UISetting.IsEnableSaveRelativeSpectrum)
			{
				return EErrorCode.NONE;
			}

			if (this._colRow.Count > this.UISetting.SaveSpectrumMaxCount)
			{
				return EErrorCode.NONE;
			}

			this.GetTRForceValue();

			string test = data["TEST"].ToString();

			string col = data["COL"].ToString();

			string row = data["ROW"].ToString();

			string testCountStr = test + "," + col + "," + row;

			foreach (var livNum in this.AcquireData.LIVDataSet)
			{
				if (!livNum.IsEnable)
				{
					continue;
				}

				string nameStr = "," + livNum.Name;

				string keyNameStr = "," + livNum.KeyName;

				List<string> drainFouceValue = this._drainForceValue[livNum.KeyName];

				List<string> sourceFouceValue = this._sourceForceValue[livNum.KeyName];

				List<string> gateFouceValue = this._gateForceValue[livNum.KeyName];

				List<string> blukFouceValue = this._blukForceValue[livNum.KeyName];

				for (int i = 0; i < drainFouceValue.Count; i++)
				{
					string forceValue = "," + drainFouceValue[i] + "," + sourceFouceValue[i] + "," + gateFouceValue[i] + "," + blukFouceValue[i];

					string IntLine = testCountStr + nameStr + forceValue + ",Intensity";

					for (int j = 0; j < livNum.SpectrumDataData[i].Wavelength.Length; j++)
					{
						IntLine += "," + livNum.SpectrumDataData[i].Intensity[j].ToString();
					}

					this.WriteLine3(IntLine);
				}
			}

			return EErrorCode.NONE;
		}

		protected override EErrorCode MoveFileToTargetByUser3(EServerQueryCmd cmd)
		{
			if (!this.UISetting.IsEnableSaveRelativeSpectrum)
			{
				return EErrorCode.NONE;
			}

			string outPath = this.UISetting.RelativeSpectrumPath;

			string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

			//Abort
			if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
			{
				fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
			}

			string fileNameWithExt = fileNameWithoutExt + ".rel";

			string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

			MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile);

			return EErrorCode.NONE;
		}
	}

}
