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

namespace MPI.Tester.Report.User.TSB
{
	class Report : ReportBase
	{
		private Dictionary<string, string> _dicmsrtResultItemTitleName;

		private Dictionary<string, string> _dicAllResultItemTitleName;

		private Dictionary<string, ElectSettingData> _dicAllResultItemElecSettingData;

		private Dictionary<string, string> _dicCALCItems;

		private static string[] _msrtResultStatus = new string[3] { "P", "F(U)", "F(L)" };

		private static string[] SYS_RESULT_ITEM_NAME = new string[13] 
        {  
            "Device Number","X (Chip)","Y (Chip)","X (Optical)","Y (Optical)","Theta (Optical)","X (AOI)","Y (AOI)","P/F (Optical)","Bin (Optical)","Cat (Optical)","Mark(AOI)","Cat(AOI)"
        };

		private static string[] SYS_RESULT_ITEM_KEYNAME = new string[13] 
        {   
            "TEST",
              "COL",
              "ROW",
             "CHUCKX",
             "CHUCKY",
             "CHUCKZ",
             "COL2",
             "ROW2", 
             "PFRESULT",
             "BINPB",
             "CATPB",
             "MARKAOI",
             "CATAOI"
        };

		private double time = 0;

		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{

		}

		#region >>> Private  Method <<<

		private void ResetResultData()
		{
			this.ResultData = new Dictionary<string, TestResultData>();

			_dicAllResultItemTitleName = new Dictionary<string, string>();

			_dicmsrtResultItemTitleName = new Dictionary<string, string>();

			_dicAllResultItemElecSettingData = new Dictionary<string, ElectSettingData>();

			_dicCALCItems = new Dictionary<string, string>();

			for (int i = 0; i < SYS_RESULT_ITEM_KEYNAME.Length; i++)
			{
				_dicAllResultItemTitleName.Add(SYS_RESULT_ITEM_KEYNAME[i], CsvQuoteMark.PlusMark(SYS_RESULT_ITEM_NAME[i]));
			}

			if (this.Product.TestCondition != null &&
				this.Product.TestCondition.TestItemArray != null &&
				this.Product.TestCondition.TestItemArray.Length > 0)
			{
				foreach (var testItem in this.Product.TestCondition.TestItemArray)
				{
					if (testItem is CALCTestItem)
					{
						foreach (var msrtItem in testItem.MsrtResult)
						{
							string a = (testItem as CALCTestItem).ItemNameA.Replace("VF", "");

							string b = (testItem as CALCTestItem).ItemNameB.Replace("VF", "");

							string output = string.Empty;

							if ((testItem as CALCTestItem).CalcType == ECalcType.Subtract)
							{
								output = a + ",-," + b;
							}
							else
							{
								output = a + ",+," + b;
							}

							_dicCALCItems.Add(msrtItem.KeyName, output);

						}
					}

					if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable)
					{
						continue;
					}

					foreach (var msrtItem in testItem.MsrtResult)
					{
						if (!msrtItem.IsEnable || !msrtItem.IsVision)
						{
							continue;
						}

						if (msrtItem.Index > 0)
						{
							string name = "[" + msrtItem.Index.ToString() + "]" + msrtItem.Name;

							this.ResultData.Add(msrtItem.KeyName, msrtItem);

							_dicmsrtResultItemTitleName.Add(msrtItem.KeyName, name);

							_dicAllResultItemTitleName.Add(msrtItem.KeyName, name);

							if (testItem.ElecSetting != null)
							{
								_dicAllResultItemElecSettingData.Add(msrtItem.KeyName, testItem.ElecSetting[0]);
							}

						}

					}
				}
			}

			this.ResultTitleInfo.SetResultData(_dicAllResultItemTitleName);
		}

		private string GetPassFailStatus(Dictionary<string, double> testData)
		{
			string rtn = "P";

			int failCount = 0;

			foreach (TestItemData item in this.Product.TestCondition.TestItemArray)
			{
				if (item.MsrtResult != null)
				{
					foreach (TestResultData result in item.MsrtResult)
					{
						if (testData.ContainsKey(result.KeyName))
						{
							if (result.IsVerify)
							{
								// 增加TempValue,卡位數
								double tempValue = Convert.ToDouble(testData[result.KeyName].ToString(result.Formate));

								if (tempValue >= result.MinLimitValue && tempValue < result.MaxLimitValue)
								{
									// return true;
								}
								else
								{
									failCount++;
								}

								//// OutSpec
								//if (tempValue > result.MaxLimitValue ||
								//    tempValue < result.MinLimitValue)
								//{
								//    failCount++;
								//    //rtn = "F(" + result.Index + ")";
								//    //return rtn;
								//}
							}
						}
					}

				}
			}

			if (failCount != 0)
			{
				rtn = "F(" + failCount.ToString() + ")";
			}

			return rtn;
		}

		private string GetTestItemSettingData(string msrtItemKey)
		{
			//Read Info=1,0,0,1.000,10,1,0,1.700,2.200,-1

			string describ = string.Empty;

			describ += "Read Info=";

			foreach (TestItemData item in this.Product.TestCondition.TestItemArray)
			{
				if (item.MsrtResult != null)
				{
					foreach (TestResultData data in item.MsrtResult)
					{
						if (data.KeyName == msrtItemKey)
						{

						}
					}

				}
			}

			return describ;
		}

		private string GetBinData(string mark, string cat, string pf, string pbBin)
		{
			if (mark == "100")
			{
				return "-10";
			}

			if (cat != "0")
			{
				return "-5";
			}

			if (pbBin == "-8")
			{
				return "-6";
			}

			if (pf == "P")
			{
				return "0";
			}

			return "-1";

		}

		private string GenerateAOIFailData(int reportDataIndex, string[] AOIData, int rawDataCount)
		{
			string line = string.Empty;

			for (int i = 0; i < rawDataCount; i++)
			{
				switch (i)
				{
					case 0:
						string indexStr = reportDataIndex.ToString("00000000");
						line += CsvQuoteMark.PlusMark(indexStr);
						break;
					//=============================
					case 1:
						line += CsvQuoteMark.PlusMark(AOIData[0]);
						break;
					case 2:
						line += CsvQuoteMark.PlusMark(AOIData[1]);
						break;
					//=============================
					case 3:
						double x = 0;
						double.TryParse(AOIData[2], out x);
						x = 1000 * x;
						int intx = (int)x;

						line += CsvQuoteMark.PlusMark(intx.ToString());
						break;
					//=============================
					case 4:
						double y = 0;
						double.TryParse(AOIData[2], out y);
						y = y * 1000;
						int inty = (int)y;

						line += CsvQuoteMark.PlusMark(inty.ToString());
						break;
					//=============================
					case 8:
						//if (AOIData[6] == "100" || AOIData[7] != "0")
						//{
						//    line += CsvQuoteMark.PlusMark("F(A)");
						//}
						//else
						//{
						//    line += CsvQuoteMark.PlusMark(string.Empty);
						//}
						line += CsvQuoteMark.PlusMark("F(A)");
						break;
					//=============================
					case 6:
						line += CsvQuoteMark.PlusMark(AOIData[0]);
						break;
					//=============================
					case 7:
						line += CsvQuoteMark.PlusMark(AOIData[1]);
						break;
					//=============================
					case 11:
						line += CsvQuoteMark.PlusMark(AOIData[6]);
						break;
					//=============================
					case 12:
						line += CsvQuoteMark.PlusMark(AOIData[7]);
						break;
					//=============================
					case 9:
					case 10:
						line += CsvQuoteMark.PlusMark(GetBinData(AOIData[6], AOIData[7], "F(A)", AOIData[8]));
						break;
					default:
						if (i != rawDataCount - 1)
						{
							line += CsvQuoteMark.PlusMark(string.Empty);
						}
						break;
				}

				if (i != rawDataCount - 1)
				{
					line += ",";
				}
				//else if (i == rawDataCount-1)
				//{
				//    line += "NULL";
				//}
			}

			line += "NULL";

			return line;

		}

		private void CombineReoprtFileAndAOIFile(string inputFile, string AOIMapFile, string outputFile)
		{
			if (!File.Exists(inputFile))
			{
				return;
			}

			if (this.ResultTitleInfo.TitleStr == string.Empty)
			{
				return;
			}

			System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
			timer.Restart();
			///////////////////////////////////////////////////////
			//Replace Data And Check Row Col
			///////////////////////////////////////////////////////
			StreamWriter sw = new StreamWriter(outputFile, false, Encoding.Default);

			StreamReader sr = new StreamReader(inputFile, Encoding.Default);

			Dictionary<string, string> dicTestData = new Dictionary<string, string>();

			string[] tempRawData = null;

			bool isGetTempRawData = false;

			bool isRawData = false;

			int testCount = 0;

			while (sr.Peek() >= 0)
			{
				string line = sr.ReadLine();

				if (isRawData)
				{
					string[] rawData = line.Split(',');

					if (!isGetTempRawData)
						tempRawData = rawData;

					string colrowKey = rawData[this.ResultTitleInfo.ColIndex].ToString() + "_" + rawData[this.ResultTitleInfo.RowIndex].ToString();

					//=============================
					// 2014.11.12 
					// 修正Row/Col 重複讀檔案造成錯誤，在此只讀最後一筆Row/Col資料。
					//=============================
					if (dicTestData.ContainsKey(colrowKey))
					{
						dicTestData[colrowKey] = line;
					}
					else
					{
						dicTestData.Add(colrowKey, line);
					}
				}
				else
				{
					if (line == this.ResultTitleInfo.TitleStr)
					{
						isRawData = true;
					}
					else
					{

					}

					sw.WriteLine(line);
				}
			}

			if (tempRawData == null)
			{
				Console.WriteLine("[TSB Report], CombineReoprtFileAndAOIFile(), TempRawData is null");
			}


			if (File.Exists(AOIMapFile) && this.UISetting.IsEnableMergeAOIFile)
			{
				bool isFindAOIDFileTitle = false;

				StreamReader srAOI = new StreamReader(AOIMapFile, Encoding.Default);

				while (srAOI.Peek() >= 0)
				{
					string line = srAOI.ReadLine();

					string[] AOIData = line.Split(',');

					if (!isFindAOIDFileTitle)
					{
						if (line == "X,Y,mPosX,mPosY,AOI_X,AOI_Y,AOIMark,AOIBin,ProberBin")
						{
							isFindAOIDFileTitle = true;
						}
						continue;
					}

					testCount++;

					//0	1	2	        3	        4	        5	        6	            7
					//X	Y	mPosX	mPosY	AOI_X	AOI_Y	AOIMark	AOIBin

					//  string AOIKey = AOIData[0] + "_" + AOIData[1];

					int posX = -999;

					int posY = -999;

					int.TryParse(AOIData[0], out posX);

					int.TryParse(AOIData[1], out posY);

					switch (this.TesterSetting.TesterCoord)
					{
						case 1:
							break;
						case 2:
							posX = posX * -1;
							break;
						case 3:
							posX = posX * -1;
							posY = posY * -1;
							break;
						case 4:
							posY = posY * -1;
							break;
					}

					string AOIKey = posX.ToString() + "_" + posY.ToString();

					//AOIData[0] = posX.ToString();

					//AOIData[1] = posY.ToString();

					if (dicTestData.ContainsKey(AOIKey))
					{
						string[] rawData = dicTestData[AOIKey].Split(',');

						if (this.ResultTitleInfo.TestIndex >= 0)
						{
							rawData[this.ResultTitleInfo.TestIndex] = testCount.ToString();

							//5	        6	        7	                8	                9	                10            11
							//X (AOI)	Y (AOI)	P/F (Optical)	Bin (Optical)	Cat (Optical)	Mark(AOI)	Cat(AOI)

							line = string.Empty;

							int endIndex = rawData.Length - 1;

							for (int i = 0; i < rawData.Length; i++)
							{
								switch (i)
								{
									case 0:
										string indexStr = testCount.ToString("00000000");
										line += CsvQuoteMark.PlusMark(indexStr);
										break;
									//=============================
									case 1:
										line += CsvQuoteMark.PlusMark(rawData[1]);
										break;
									case 2:
										line += CsvQuoteMark.PlusMark(rawData[2]);
										break;
									//=============================
									case 3:
										double x = 0;
										double.TryParse(AOIData[2], out x);
										x = 1000 * x;
										int intx = (int)x;
										line += CsvQuoteMark.PlusMark(intx.ToString());
										break;
									//=============================
									case 6:
										line += CsvQuoteMark.PlusMark(AOIData[0]);
										break;
									//=============================
									case 7:
										line += CsvQuoteMark.PlusMark(AOIData[1]);
										break;
									//=============================
									case 4:
										double y = 0;
										double.TryParse(AOIData[2], out y);

										y = 1000 * y;

										int inty = (int)y;
										line += CsvQuoteMark.PlusMark(inty.ToString());
										break;
									//=============================
									case 11:
										line += CsvQuoteMark.PlusMark(AOIData[6]);
										break;
									//=============================
									case 12:
										line += CsvQuoteMark.PlusMark(AOIData[7]);
										break;
									//=============================
									case 9:
									case 10:
										line += CsvQuoteMark.PlusMark(GetBinData(AOIData[6], AOIData[7], rawData[8], string.Empty));
										break;
									default:
										if (i != endIndex)
										{
											line += CsvQuoteMark.PlusMark(rawData[i]);
										}
										else
										{
											line += rawData[i];
										}
										break;
								}

								if (i != rawData.Length - 1)
								{
									line += ",";
								}
							}
						}

						sw.WriteLine(line);
					}
					else
					{
						AOIData[0] = posX.ToString();

						AOIData[1] = posY.ToString();

						string aoiFailStr = string.Empty;

						if (tempRawData != null)
						{
							aoiFailStr = GenerateAOIFailData(testCount, AOIData, tempRawData.Length);

							sw.WriteLine(aoiFailStr);
						}
					}
				}

				srAOI.Close();

				srAOI.Dispose();

				if (this.UISetting.IsDeletePBAOISourceFile)
				{
					MPIFile.DeleteFile(AOIMapFile);
				}

				// MPIFile.DeleteFile(AOIMapFile);
			}
			else
			{
				Console.WriteLine("[TSB Report], CombineReoprtFileAndAOIFile(), NO AOI ReferenceData");

				foreach (var data in dicTestData)
				{
					testCount++;

					string[] rawData = data.Value.Split(',');

					string line = string.Empty;

					int endIndex = rawData.Length - 1;

					if (this.ResultTitleInfo.TestIndex >= 0)
					{
						//5	        6	        7	                8	                9	                10            11
						//X (AOI)	Y (AOI)	P/F (Optical)	Bin (Optical)	Cat (Optical)	Mark(AOI)	Cat(AOI)

						for (int i = 0; i < rawData.Length; i++)
						{
							if (i == 0)
							{
								string indexStr = testCount.ToString("00000000");
								line += CsvQuoteMark.PlusMark(indexStr);
							}
							else if (i < SYS_RESULT_ITEM_KEYNAME.Length)
							{
								line += CsvQuoteMark.PlusMark(rawData[i]);
							}
							else
							{
								if (i != endIndex)
								{
									line += CsvQuoteMark.PlusMark(rawData[i]);
								}
								else
								{
									line += rawData[i];
								}
							}

							if (i != rawData.Length - 1)
							{
								line += ",";
							}
						}
					}

					sw.WriteLine(line);
				}
			}

			dicTestData.Clear();

			sr.Close();

			sr.Dispose();

			sw.Close();

			sw.Dispose();

			timer.Stop();

			time = timer.ElapsedMilliseconds;

			MPIFile.DeleteFile(inputFile);

			// MPIFile.DeleteFile(AOIMapFile);
		}

		private string GetReadInfo(string name, string min, string max)
		{
			string readInfo = "Read Info=";

			string DisplayName = string.Empty;

			if (this.ResultData.ContainsKey(name))
			{
				string showName = this.ResultData[name].Name;

				if (showName.Contains("VFD"))
				{
					readInfo += "101,0,";
				}
				else if (showName.Contains("VF"))
				{
					readInfo += "1,0,";
				}
				else if (showName.Contains("IR"))
				{
					readInfo += "4,0,";
				}
				else if (showName.Contains("VR"))
				{
					readInfo += "3,0,";
				}
				else if (showName.Contains("IF"))
				{
					readInfo += "2,0,";
				}
				else if (showName.Contains("WP"))
				{
					readInfo += "7,3,";
				}
				else if (showName.Contains("WD"))
				{
					readInfo += "10,4,";
				}
				else if (showName.Contains("PO(mW)"))
				{
					readInfo += "21,2,";
				}
				else if (showName.Contains("PO(V)"))
				{
					readInfo += "5,1,";
				}
				else
				{
					readInfo += "0,0,";
				}
			}

			// #1 , #2 Name & Item

			if (_dicAllResultItemElecSettingData.ContainsKey(name))
			{
				ElectSettingData esd = this._dicAllResultItemElecSettingData[name];

				double applydata = esd.ForceValue;

				//#3 Range & #4 Bias
				readInfo += getForceRangeIndex(applydata); //1

				readInfo += ",";

				//#5 ForceTime

				if (esd.ForceTime >= 1)
				{
					readInfo += esd.ForceTime.ToString(); //3

					readInfo += ",";

					readInfo += "1";
				}
				else
				{
					readInfo += (1000 * esd.ForceTime).ToString(); //3

					readInfo += ",";

					readInfo += "0";
				}

				//readInfo += esd.ForceTime.ToString(); //3

				//readInfo += ",";

				//#7 Space
				readInfo += ",0,"; //4
			}
			else if (this._dicCALCItems.ContainsKey(name))
			{
				readInfo += _dicCALCItems[name];

				readInfo += ",0,0,";
			}
			else
			{
				readInfo += ",0,0,";
			}

			readInfo += min + "," + max + ",-1";

			return readInfo;
		}

		private string getForceRangeIndex(double input)
		{
			//#3 Range

			//#4 Bias

			//#5 ForceTime

			//#6 Unit

			if (input < 0)
			{
				return "0," + Math.Abs(input).ToString("0.000");
			}

			string str = string.Empty;

			input = input * 1000;


			if (input >= 100000) // above 100mA
			{
				str = "5," + (input / 1000).ToString("000.0");
			}
			else if (input >= 10000) //above 10mA
			{
				str = "4," + (input / 1000).ToString("00.00");
			}
			else if (input >= 1000) //above 1mA
			{
				str = "3," + (input / 1000).ToString("0.000");
			}
			else if (input >= 100) //above 100uA
			{
				str = "2," + input.ToString("000.0");
			}
			else if (input >= 10)  //above 10uA
			{
				str = "1," + input.ToString("00.00");
			}
			else  //above 1uA
			{
				str = "0," + input.ToString("0.000");
			}
			return str;
		}

		#endregion

		#region >>> Protected Override Method <<<

		protected override void SetResultTitle()
		{
			List<string> msrtItemName = new List<string>();

			foreach (string data in SYS_RESULT_ITEM_NAME)
			{
				msrtItemName.Add(CsvQuoteMark.PlusMark(data) + ",");
			}

			foreach (var data in _dicmsrtResultItemTitleName)
			{
				string str = CsvQuoteMark.PlusMark(data.Value) + ",";

				str += CsvQuoteMark.PlusMark("P/F") + ",";

				msrtItemName.Add(str);
			}

			string tiltstr = string.Join("", msrtItemName.ToArray()) + "CH,NULL";

			this.ResultTitleInfo.SetTitleStr(tiltstr);
		}

		protected override EErrorCode WriteReportHeadByUser()
		{
			//  this.ResultTitleInfo.AddTitleKey(@"""Device Number"",");

			ResetResultData();

			this.WriteLine("[PRODUCT]");
			this.WriteLine("Product Name=" + this.UISetting.ProductType);
			this.WriteLine(string.Empty);
			//[PRODUCT]
			//Product Name=TS1FH1B

			this.WriteLine("[LOT]");
			this.WriteLine("Lot Number=" + this.UISetting.LotNumber);
			this.WriteLine("Key Number=" + this.UISetting.KeyNumber);
			this.WriteLine("Number of Wafer=" + this.UISetting.WaferPcs.ToString());
			this.WriteLine(string.Empty);

			//[LOT]
			//Lot Number=HH0338-AA
			//Key Number=TS1FH1B   H07520HH0338-AA
			//Number of Wafer=16

			string waferNumberStr = string.Empty;

			string ShapeofWaferStr = string.Empty;

			if (this.UISetting.WaferNumber != string.Empty)
			{
				char[] waferNameChar = this.UISetting.WaferNumber.ToArray();

				if (waferNameChar.Length >= 3)
				{
					char[] a = new char[2] { waferNameChar[0], waferNameChar[1] };

					char[] b = new char[1] { waferNameChar[2] };

					waferNumberStr = new string(a);

					ShapeofWaferStr = new string(b);
				}
			}


			this.WriteLine("[WAFER]");
			this.WriteLine("Wafer Laser Mark=" + this.UISetting.LotNumber);
			this.WriteLine("Wafer Number=" + waferNumberStr);
			this.WriteLine("Suffix Number=" + this.UISetting.WaferPcs.ToString());
			this.WriteLine("Wafer Size=8");
			this.WriteLine("Shape of Wafer=" + ShapeofWaferStr);
			this.WriteLine(string.Empty);

			//[WAFER]
			//Wafer Laser Mark=
			//Wafer Number=01
			//Suffix Number=
			//Wafer Size=8
			//Shape of Wafer=1

			this.WriteLine("[RING]");
			this.WriteLine("Ring Number=" + this.UISetting.WaferNumber);
			this.WriteLine(string.Empty);
			//[RING]
			//Ring Number=

			this.WriteLine("[OPERATION]");
			this.WriteLine("Work Code=S20");
			this.WriteLine("Work Name=" + this.UISetting.OperatorName);
			this.WriteLine("Tester Number=" + this.UISetting.MachineName);
			this.WriteLine("Cassette Number=" + this.UISetting.CassetteNumber);
			this.WriteLine("Slot Number=" + this.UISetting.SlotNumber);
			this.WriteLine("Tester Recipe File Name=" + this.UISetting.TaskSheetFileName);
			this.WriteLine("Prober Recipe File Name=" + this.UISetting.TaskSheetFileName);
			this.WriteLine("Starting Point Position=CC");
			this.WriteLine("Direction of increment X=Right");
			this.WriteLine("Direction of increment Y=Down");
			this.WriteLine("Device Rows=");
			this.WriteLine("Device Columns=");
			this.WriteLine("Pass Bin Code=" + CsvQuoteMark.PlusMark("0"));

			string failBinDescrib = CsvQuoteMark.PlusMark("-1") + "," + CsvQuoteMark.PlusMark("-2") + "," + CsvQuoteMark.PlusMark("-3") + "," + CsvQuoteMark.PlusMark("-4") + "," +
												   CsvQuoteMark.PlusMark("-5") + "," + CsvQuoteMark.PlusMark("-6") + "," + CsvQuoteMark.PlusMark("-10");
			this.WriteLine("Fail Bin Code=" + failBinDescrib);
			this.WriteLine("Pass Cat Code=" + CsvQuoteMark.PlusMark("0"));
			this.WriteLine("Fail Cat Code=" + failBinDescrib);
			this.WriteLine("Start Date Time=" + this.TesterSetting.StartTestTime.ToString("yyyy-MM-dd HH:mm:ss"));
			this.WriteLine("End Date Time=");
			this.WriteLine(string.Empty);

			//[OPERATION]
			//Work Code=S20
			//Work Name=杺菾枌
			//Tester Number=SH05
			//Cassette Number=01
			//Slot Number=01
			//Tester Recipe File Name=SS1FH1-C02.CND
			//Prober Recipe File Name=SS1FH1-C02.XML
			//Starting Point Position=CC
			//Direction of increment X=Right
			//Direction of increment Y=Down
			//Device Rows=70
			//Device Columns=71
			//Pass Bin Code="0"
			//Fail Bin Code="-1","-2","-3","-4","-5","-6","-10"
			//Pass Cat Code="0"
			//Fail Cat Code="-1","-2","-3","-4","-5","-6","-10"
			//Start Date Time=2014-09-10 11:57:44
			//End Date Time=2014-09-10 12:20:59

			this.WriteLine("[SPEC]");

			if (_dicmsrtResultItemTitleName != null)
			{
				// this.WriteLine("Test Item Count=" + CsvQuoteMark.PlusMark(this.Product.TestCondition.TestItemArray.Length.ToString()));
				this.WriteLine("Test Item Count=" + _dicmsrtResultItemTitleName.Count.ToString());
			}
			else
			{
				this.WriteLine("Test Item Count=0");
			}

			int i = 0;

			foreach (var data in this.ResultData)
			{
				string indexDescrib = (i + 1).ToString("000") + " ";

				this.WriteLine(indexDescrib + "Test Number=" + (i + 1).ToString());

				this.WriteLine(indexDescrib + "Test Item Name=" + data.Value.Name);

				string min = string.Empty;

				string max = string.Empty;

				if (data.Value.IsVerify)
				{
					min = data.Value.MinLimitValue.ToString(data.Value.Formate);

					max = data.Value.MaxLimitValue.ToString(data.Value.Formate);
				}
				else
				{
					min = 0.ToString(data.Value.Formate);

					max = 0.ToString(data.Value.Formate);
				}



				this.WriteLine(indexDescrib + "Lower=" + min);

				this.WriteLine(indexDescrib + "Upper=" + max);

				this.WriteLine(indexDescrib + "Unit=" + data.Value.Unit);

				this.WriteLine(indexDescrib + "Fail Bin Number=-1");

				string readInfo = string.Empty;

				readInfo = GetReadInfo(data.Key, min, max);



				this.WriteLine(indexDescrib + readInfo);

				i++;
			}

			this.WriteLine(string.Empty);

			this.WriteLine("[PASS BIN]");
			this.WriteLine("Pass Bin Item Count=0");
			this.WriteLine(string.Empty);

			this.WriteLine("[SUMMARY]");
			this.WriteLine("Gross=");
			this.WriteLine("Net=");
			this.WriteLine(string.Empty);

			//001 Test Number=1
			//001 Test Item Name=VF1
			//001 Lower=1.700
			//001 Upper=2.200
			//001 Unit=V
			//001 Fail Bin Number=-1
			//001 Read Info=1,0,0,1.000,10,1,0,1.700,2.200,-1

			//[PASS BIN]
			//Pass Bin Item Count=0

			//[SUMMARY]
			//Gross=3847
			//Net=2650

			//[RESULT]			

			this.WriteLine("[RESULT]");

			this.WriteLine(this.ResultTitleInfo.TitleStr);

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
			////////////////////////////////////////////
			//Rerite Report Head
			////////////////////////////////////////////
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string EndtestTime = "End Date Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			replaceData.Add("End Date Time=", EndtestTime);

			replaceData.Add("Gross=", "Gross=" + ReportProcess.TestCount.ToString());

			//  replaceData.Add("Gross=", "Gross=" + ReportProcess.StatisticData("TEST", EStatisticType.System).Count.ToString());


			string msrtName = string.Empty;

			foreach (var item in this.ResultData)
			{
				msrtName = item.Key;
				break;
			}

			if (msrtName != string.Empty)
			{
				replaceData.Add("Net=", "Net=" + ReportProcess.StatisticData("TEST", EStatisticType.All01).Count.ToString());
			}
			else
			{
				replaceData.Add("Net=", "Net=0");
			}

			this.UISetting.TestResultFileName = this.TestResultFileNameWithoutExt();

            this.FileFullNameRep = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, this.TestResultFileNameWithoutExt() + ".OutputTemp");

			string actualFullNameCsv = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, this.TestResultFileNameWithoutExt() + ".csv");

			this.ReplaceReport(replaceData);

			// AOI FileName,  "C:\MPI\Share\LotName\WaferID.csv的路徑下讀取檔案
			// 由Tester決定是否要砍檔案。

			string AOIFileName = Path.Combine(this.TestResultFileNameWithoutExt() + ".csv");

			// string AOIFileName = Path.Combine(this.UISetting.ProductType,this.UISetting.LotNumber, this.TestResultFileNameWithoutExt() + ".csv");

			string AOIMapFile = Path.Combine(Constants.Paths.MPI_SHARE_DIR, AOIFileName);

            CombineReoprtFileAndAOIFile(this.FileFullNameRep, AOIMapFile, actualFullNameCsv);

            this.FileFullNameRep = actualFullNameCsv;

			return EErrorCode.NONE;
		}

		protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
		{
			int binSN = (int)data["BINSN"];

			SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

			int binGrade = 0;

			string binCode = string.Empty;

			if (bin != null)
			{
				binCode = bin.BinCode;

				if (bin.BinningType == EBinningType.IN_BIN)
				{
					binGrade = 1;
				}
				else if (bin.BinningType == EBinningType.SIDE_BIN)
				{
					binGrade = 2;
				}
				else if (bin.BinningType == EBinningType.NG_BIN)
				{
					binGrade = 3;
				}
			}

			string line = string.Empty;

			int index = 0;

			string systemData = string.Empty;

			if (data.ContainsKey("TEST"))
			{
				//  line += "\"" + data["TEST"].ToString("0000000") + "\"";

				line += data["TEST"].ToString();
			}
			else
			{
				//  line += "\"" + string.Empty + "\"";
			}

			line += ",";

			for (int i = 1; i < SYS_RESULT_ITEM_KEYNAME.Length; i++)
			{
				if (SYS_RESULT_ITEM_KEYNAME[i] == "PFRESULT")
				{
					systemData = GetPassFailStatus(data);
				}
				else if (data.ContainsKey(SYS_RESULT_ITEM_KEYNAME[i]))
				{
					systemData = data[SYS_RESULT_ITEM_KEYNAME[i]].ToString();
				}
				else
				{
					systemData = string.Empty;
				}

				//    line += AddDoubleQuote(systemData);

				line += systemData;
				line += ",";
			}

			foreach (var item in this.ResultData)
			{
				if (SYS_RESULT_ITEM_KEYNAME.Contains(item.Key))
				{
					continue;
				}

				if (item.Key == "BIN_CODE")
				{
					line += binCode;
				}
				else if (item.Key == "BIN_GRADE")
				{
					line += binGrade.ToString();
				}
				else if (data.ContainsKey(item.Key))
				{
					string format = string.Empty;

					string PFResult = string.Empty;

					if (this.ResultData.ContainsKey(item.Key))
					{
						format = this.ResultData[item.Key].Formate;

						if (this.ResultData[item.Key].IsVerify)
						{
							double tempValue = Convert.ToDouble(data[item.Key].ToString(format));

							if (tempValue >= this.ResultData[item.Key].MinLimitValue && tempValue < this.ResultData[item.Key].MaxLimitValue)
							{
								PFResult = _msrtResultStatus[0]; // P  // 2.800 <=value < 3.200
							}
							else if (tempValue >= this.ResultData[item.Key].MaxLimitValue)
							{
								PFResult = _msrtResultStatus[1]; // F(U) // 3.200
							}
							else
							{
								PFResult = _msrtResultStatus[2];  // F(L)   < 2.800
							}

							//if (tempValue > this.ResultData[item.Key].MaxLimitValue)
							//{
							//    PFResult = _msrtResultStatus[1];  // F(U)  // 3.2
							//}
							//else if (tempValue < this.ResultData[item.Key].MinLimitValue)
							//{
							//    PFResult = _msrtResultStatus[2];  // F(L)   //2.8
							//}
							//else
							//{
							//    PFResult = _msrtResultStatus[0]; // P
							//}
						}
						else
						{
							PFResult = _msrtResultStatus[0];  // P
						}
					}

					line += data[item.Key].ToString(format);

					line += ",";

					line += PFResult;

					line += ",";

				}

				index++;
			}


			line += data["CHANNEL"];

			line += ",";

			line += "NULL";

			this.WriteLine(line);

			return EErrorCode.NONE;
		}

		protected override EErrorCode MoveFileToTargetByUser(EServerQueryCmd cmd)
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
			string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

			string fileNameWithExt = this.TestResultFileNameWithoutExt();

			//Abort
			fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

			fileNameWithExt = "OI_" + fileNameWithoutExt + "." + this.UISetting.TestResultFileExt;

			fileNameWithExt = Path.Combine(this.UISetting.ProductType, this.UISetting.LotNumber, fileNameWithExt);

			string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

			string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

			string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

			MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile01);

			if (isOutputPath02)
			{
                MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile02);
			}

			if (isOutputPath03)
			{
                MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile03);
			}

			DriveInfo driveInfo = new DriveInfo(outputPathAndFile02);

			if (!driveInfo.IsReady)
			{
				if (driveInfo.DriveType == DriveType.Network)
				{
					if (isOutputPath03)
					{
                        MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile03);
					}

				}
			}

			return EErrorCode.NONE;
		}

		#endregion
	}
}
