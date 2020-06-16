using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.Maths;
using System.IO;

namespace MPI.Tester.Report
{
	public class RIMathsA : IRICalcMath
	{
		private bool IsOutputDetail = true;		

		public bool ClacProcess(UISetting uiSetting, TesterSetting testerSetting, ProductData productData, string inputFileFullName, string outputFileFullName)
		{
			//-----------------------------------------------------------------
			// Check Enable Spec Item
			//-----------------------------------------------------------------
            Dictionary<string, int> needData = new Dictionary<string, int>();

			Dictionary<string, int> riItemIndex = new Dictionary<string, int>();

			foreach (var item in productData.RISpec)
			{
				if (item.IsEnableReportInterpolation)
				{
					riItemIndex.Add(item.KeyName, -1);
				}

                if (item.IsEnableReportInterpolation || item.IsEnableVerify)
                {
                    needData.Add(item.KeyName, -1);
                }
			}

			if (riItemIndex.Count == 0)
			{
                Console.WriteLine("[RIMathsA], ClacProcess(), riItemIndex.Count = 0");

				return false;
			}	

			//-----------------------------------------------------------------
			// Get Col Row Index And titleStr 
			//-----------------------------------------------------------------
			int colIndex = -1;

			int rowIndex = -1;

			int groupIndex = -1;

			int ciex3Index = -1;

			int ciey3Index = -1;

			int ciez3Index = -1;

			List<string> keyNameList = new List<string>();

			List<string> nameList = new List<string>();

			int index = 0;

			string titleStr = string.Empty;

			foreach (var item in uiSetting.UserDefinedData.ResultItemNameDic)
			{
				if (item.Key.Contains("LOP"))
				{
					if (!productData.LOPSaveItem.ToString().Contains("mcd"))
					{
						continue;
					}
				}

				if (item.Key.Contains("WATT"))
				{
					if (!productData.LOPSaveItem.ToString().Contains("watt"))
					{
						continue;
					}
				}

				if (item.Key.Contains("LM"))
				{
					if (!productData.LOPSaveItem.ToString().Contains("lm"))
					{
						continue;
					}
				}

				if (item.Key == EProberDataIndex.COL.ToString())
				{
					colIndex = index;
				}

				if (item.Key == EProberDataIndex.ROW.ToString())
				{
					rowIndex = index;
				}

				if (item.Key == EProberDataIndex.TestChipGroup.ToString())
				{
					groupIndex = index;
				}

				if (riItemIndex.ContainsKey(item.Key))
				{
					riItemIndex[item.Key] = index;
				}

                if (needData.ContainsKey(item.Key))
                {
                    needData[item.Key] = index;
                }

				if (item.Key == "CIEx_3")
				{
					ciex3Index = index;
				}

				if (item.Key == "CIEy_3")
				{
					ciey3Index = index;
				}

				if (item.Key == "CIEz_3")
				{
					ciez3Index = index;
				}

				titleStr += item.Value + ",";

				keyNameList.Add(item.Key);

				nameList.Add(item.Value);

				index++;
			}

			if (titleStr.Length > 0)
			{
				titleStr = titleStr.Remove(titleStr.Length - 1, 1);
			}

			if (titleStr == string.Empty || colIndex < 0 || rowIndex < 0 || groupIndex < 0)
			{
				Console.WriteLine("[RIMathsA], ClacProcess(), colIndex:" + colIndex + ", rowIndex:" + rowIndex + ", groupIndex:" + groupIndex);

				return false;
			}

			//-----------------------------------------------------------------
			// Load Raw Data
			//-----------------------------------------------------------------
			Dictionary<string, RIChip> dataSet = new Dictionary<string, RIChip>();

			Dictionary<string, Statistic> statisticSet = new Dictionary<string, Statistic>();

			using (StreamReader sr = new StreamReader(inputFileFullName, Encoding.Default))
			{
				bool isRawData = false;

				while (sr.Peek() >= 0)
				{
					string line = sr.ReadLine();

					if (isRawData)
					{
						string[] items = line.Split(',');

						int col = int.Parse(items[colIndex]);

						int row = int.Parse(items[rowIndex]);

						int group = int.Parse(items[groupIndex]);

						string key = "X" + items[colIndex] + "_Y" + items[rowIndex];

						// dataSet
						RIChip chip = new RIChip(col, row);

						if (group == 1)
						{
							chip.Mark = ERIChipMark.SimpleTest;
						}

                        foreach (var data in needData)
						{
							float value = 0.0f;

                            if (float.TryParse(items[data.Value], out value))
							{
								// statisticSet
                                if (statisticSet.ContainsKey(data.Key))
								{
                                    statisticSet[data.Key].Push(value);
								}
								else
								{
									Statistic st = new Statistic();

									st.Push(value);

                                    statisticSet.Add(data.Key, st);
								}
							}

                            chip.RawData.Add(data.Key, value);
						}

						if (dataSet.ContainsKey(key))
						{
							dataSet[key] = chip;
						}
						else
						{
							dataSet.Add(key, chip);
						}
					}
					else if (line == titleStr)
					{
						isRawData = true;
					}
				}
			}

            if (dataSet.Count == 0 || statisticSet.Count == 0)
            {
                Console.WriteLine("[RIMathsA], ClacProcess(), dataSet.Count:" + dataSet.Count + ", statisticSet.Count:" + statisticSet.Count);

                return false;
            }

			//-----------------------------------------------------------------
			// Mark Sample and WaitInterpolation Chip
			//-----------------------------------------------------------------
			foreach (var chip in dataSet)
			{
				if (chip.Value.Mark == ERIChipMark.None)
				{
					chip.Value.Mark = ERIChipMark.WaitInterpolation;
				}

				foreach (var spic in productData.RISpec)
				{
					if (!spic.IsEnableVerify)
					{
						continue;
					}

					if (chip.Value.Mark == ERIChipMark.WaitInterpolation && spic.IsOptItem)
					{
						continue;
					}

					float value = chip.Value.RawData[spic.KeyName];

					if (spic.SpecMode == ERISpecMode.Range)
					{
						if (value < spic.Min || value > spic.Max)
						{
							if (chip.Value.Mark == ERIChipMark.SimpleTest)
							{
								chip.Value.Mark = ERIChipMark.WaitReCalSimpleTest;
							}
							else
							{
								chip.Value.Mark = ERIChipMark.WaitInterpolationAndElecFail;
							}

							break;
						}
					}
					else
					{
						float std = (float)statisticSet[spic.KeyName].STDEV;

						float avg = (float)statisticSet[spic.KeyName].Mean;

						float spec = spic.STD;

						float min = avg - spec * std;

						float max = avg + spec * std;

						if (value < min || value > max)
						{
							if (chip.Value.Mark == ERIChipMark.SimpleTest)
							{
								chip.Value.Mark = ERIChipMark.WaitReCalSimpleTest;
							}
							else
							{
								chip.Value.Mark = ERIChipMark.WaitInterpolationAndElecFail;
							}

							break;
						}
					}
				}
			}

			//-----------------------------------------------------------------
			// Mark Skip All Test Chip
			//-----------------------------------------------------------------



			//-----------------------------------------------------------------
			// ReCal Sample Test Chip Value
			//-----------------------------------------------------------------
			foreach (var chip in dataSet)
			{
				if (chip.Value.Mark == ERIChipMark.WaitReCalSimpleTest)
				{
					List<string> keys = new List<string>();

                    for (int col = 0; col < productData.XLineSubBinSampleCH * 3; col++)
                    {
						for (int row = 0; row < productData.XLineSubBinSampleCH * 3; row++)
                        {
                            keys.Add("X" + (chip.Value.Col + col - 1) + "_Y" + (chip.Value.Row + row - 1));
                        }
                    }

					Dictionary<string, List<float>> reSampleDataDic = new Dictionary<string, List<float>>();

					foreach (var key in keys)
					{
						if (dataSet.ContainsKey(key) && (dataSet[key].Mark == ERIChipMark.SimpleTest ||
                                                         dataSet[key].Mark == ERIChipMark.ReCalSimpleTestFail ||
														 dataSet[key].Mark == ERIChipMark.ReCalSimpleTestFinish))
						{
							foreach (var item in riItemIndex)
							{
								if (!reSampleDataDic.ContainsKey(item.Key))
								{
									reSampleDataDic.Add(item.Key, new List<float>());
								}

								reSampleDataDic[item.Key].Add(dataSet[key].RawData[item.Key]);
							}
						}
					}

                    if (reSampleDataDic.Count != 0)
                    {
                        foreach (var item in reSampleDataDic)
                        {
                            float avg = reSampleDataDic[item.Key].Average();

                            float mean = reSampleDataDic[item.Key][reSampleDataDic[item.Key].Count / 2];

                            chip.Value.RawData[item.Key] = avg > mean ? avg : mean;

                            chip.Value.Mark = ERIChipMark.ReCalSimpleTestFinish;
                        }
                    }
                    else
                    {
                        //chip.Value.RawData[item.Key] = 0;

                        chip.Value.Mark = ERIChipMark.ReCalSimpleTestFail;
                    }
				}
				else if (chip.Value.Mark == ERIChipMark.SimpleTest)
				{
                    //check
					List<string> keys = new List<string>();

					keys.Add("X" + chip.Value.Col + "_Y" + (chip.Value.Row - productData.YLineSubBinSampleCH));

					keys.Add("X" + chip.Value.Col + "_Y" + (chip.Value.Row + productData.YLineSubBinSampleCH));

					Dictionary<string, List<float>> reSampleDataDic = new Dictionary<string, List<float>>();

					foreach (var key in keys)
					{
						if (dataSet.ContainsKey(key) && (dataSet[key].Mark == ERIChipMark.SimpleTest ||
                                                         dataSet[key].Mark == ERIChipMark.ReCalSimpleTestFail ||
														 dataSet[key].Mark == ERIChipMark.ReCalSimpleTestFinish))
						{
							foreach (var item in riItemIndex)
							{
								if (!reSampleDataDic.ContainsKey(item.Key))
								{
									reSampleDataDic.Add(item.Key, new List<float>());
								}

								reSampleDataDic[item.Key].Add(dataSet[key].RawData[item.Key]);
							}
						}
					}

					foreach (var item in reSampleDataDic)
					{
						if (item.Value.Count != 2)
						{
							continue;
						}

						float errSpec = 0.030f;

						float err1 = (item.Value[0] / chip.Value.RawData[item.Key] - 1);

						float err2 = (item.Value[1] / chip.Value.RawData[item.Key] - 1);

						bool reCal = false;

						reCal |= err1 > 0 & err2 > 0 & err1 > errSpec & err2 > errSpec;

						reCal |= err1 < 0 & err2 < 0 & Math.Abs(err1) > errSpec & Math.Abs(err2) > errSpec;

						if (reCal)
						{
							for (int col = 0; col < productData.XLineSubBinSampleCH * 3; col++)
                            {
								for (int row = 0; row < productData.YLineSubBinSampleCH * 3; row++)
                                {
                                    string key = "X" + (chip.Value.Col + col - 1) + "_Y" + (chip.Value.Row + row - 1);

                                    if (!keys.Contains(key))
                                    {
                                        keys.Add(key);
                                    }
                                }
                            }

							foreach (var key in keys)
							{
                                if (dataSet.ContainsKey(key) && (dataSet[key].Mark == ERIChipMark.SimpleTest ||
																 dataSet[key].Mark == ERIChipMark.ReCalSimpleTestFail ||
																 dataSet[key].Mark == ERIChipMark.ReCalSimpleTestFinish))
								{
									reSampleDataDic[item.Key].Add(dataSet[key].RawData[item.Key]);
								}
							}

							if (testerSetting.RIReCalcMode == ERIReCalcMode.Average)
							{
								chip.Value.RawData[item.Key] = item.Value.Average();
							}
							else if (testerSetting.RIReCalcMode == ERIReCalcMode.Median)
							{
								chip.Value.RawData[item.Key] = item.Value[item.Value.Count / 2];
							}

							chip.Value.Mark = ERIChipMark.ReCalSimpleTestFinish;
						}
					}
				}
			}

			//-----------------------------------------------------------------
			// Interpolation
			//-----------------------------------------------------------------
			foreach (var chip in dataSet)
			{
				if (chip.Value.Mark != ERIChipMark.WaitInterpolation && chip.Value.Mark != ERIChipMark.WaitInterpolationAndElecFail)
				{
					continue;
				}

				if (chip.Value.Row % productData.YLineSubBinSampleCH != 0)
				{
					continue;
				}

				int chip1Col1 = (chip.Value.Col / productData.XLineSubBinSampleCH) * productData.XLineSubBinSampleCH;

				int chip1Col2 = chip.Value.Col > 0 ? chip1Col1 + productData.XLineSubBinSampleCH : chip1Col1 - productData.XLineSubBinSampleCH;

				string key1 = "X" + chip1Col1 + "_Y" + chip.Value.Row;

				string key2 = "X" + chip1Col2 + "_Y" + chip.Value.Row;

				RIChip chip1 = null;

				RIChip chip2 = null;

				if (dataSet.ContainsKey(key1))
				{
					chip1 = dataSet[key1];

					//if (chip1.Mark == EChipMark.AllTestAndChipFail)
					//{
					//    chip1 = null;
					//}
				}

				if (dataSet.ContainsKey(key2))
				{
					chip2 = dataSet[key2];

					//if (chip2.Mark == EChipMark.AllTestAndChipFail)
					//{
					//    chip2 = null;
					//}
				}

				foreach (var item in riItemIndex)
				{
					if (chip1 == null && chip2 == null)
					{
						chip.Value.RawData[item.Key] = 0.0f;
					}
					else if (chip1 != null && chip2 == null)
					{
						chip.Value.RawData[item.Key] = chip1.RawData[item.Key];
					}
					else if (chip1 == null && chip2 != null)
					{
						chip.Value.RawData[item.Key] = chip2.RawData[item.Key];
					}
					else
					{
						chip.Value.RawData[item.Key] = Maths.LinearInterpolation.Push(chip.Value.Col, chip1.Col, chip2.Col, chip1.RawData[item.Key], chip2.RawData[item.Key]);
					}
				}

				if (chip.Value.Mark == ERIChipMark.WaitInterpolation)
				{
					chip.Value.Mark = ERIChipMark.InterpolationFinish;
				}
				else
				{
					chip.Value.Mark = ERIChipMark.InterpolationFinishAndElecFail;
				}
			}

			foreach (var chip in dataSet)
			{
				if (chip.Value.Mark != ERIChipMark.WaitInterpolation && chip.Value.Mark != ERIChipMark.WaitInterpolationAndElecFail)
				{
					continue;
				}

				int chip1Row1 = (chip.Value.Row / productData.YLineSubBinSampleCH) * productData.YLineSubBinSampleCH;

				int chip1Row2 = chip.Value.Row > 0 ? chip1Row1 + productData.YLineSubBinSampleCH : chip1Row1 - productData.YLineSubBinSampleCH;

				string key1 = "X" + chip.Value.Col + "_Y" + chip1Row1;

				string key2 = "X" + chip.Value.Col + "_Y" + chip1Row2;

				RIChip chip1 = null;

				RIChip chip2 = null;

				if (dataSet.ContainsKey(key1))
				{
					chip1 = dataSet[key1];

					//if (chip1.Mark == EChipMark.AllTestAndChipFail)
					//{
					//    chip1 = null;
					//}
				}

				if (dataSet.ContainsKey(key2))
				{
					chip2 = dataSet[key2];

					//if (chip2.Mark == EChipMark.AllTestAndChipFail)
					//{
					//    chip2 = null;
					//}
				}

				foreach (var item in riItemIndex)
				{
					if (chip1 == null && chip2 == null)
					{
						chip.Value.RawData[item.Key] = 0.0f;
					}
					else if (chip1 != null && chip2 == null)
					{
						chip.Value.RawData[item.Key] = chip1.RawData[item.Key];
					}
					else if (chip1 == null && chip2 != null)
					{
						chip.Value.RawData[item.Key] = chip2.RawData[item.Key];
					}
					else
					{
						chip.Value.RawData[item.Key] = Maths.LinearInterpolation.Push(chip.Value.Row, chip1.Row, chip2.Row, chip1.RawData[item.Key], chip2.RawData[item.Key]);
					}
				}

				if (chip.Value.Mark == ERIChipMark.WaitInterpolation)
				{
					chip.Value.Mark = ERIChipMark.InterpolationFinish;
				}
				else
				{
					chip.Value.Mark = ERIChipMark.InterpolationFinishAndElecFail;
				}
			}

			//-----------------------------------------------------------------
			// Write Interpolation Report
			//-----------------------------------------------------------------
			Dictionary<string, string> formatDic = new Dictionary<string, string>();

			foreach (var item in riItemIndex)
			{
				string format = "0";

				if (uiSetting.UserDefinedData[item.Key] != null)
				{
					format = uiSetting.UserDefinedData[item.Key].Formate;
				}

				formatDic.Add(item.Key, format);
			}

			using (StreamReader sr = new StreamReader(inputFileFullName, Encoding.Default))
			{
                using (StreamWriter sw = new StreamWriter(outputFileFullName, false, Encoding.Default))
				{
					bool isRawData = false;

					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();

						if (isRawData)
						{
							List<string> items = line.Split(',').ToList();

                            //int col = int.Parse(items[colIndex]);

                            //int row = int.Parse(items[rowIndex]);

							string key = "X" + items[colIndex] + "_Y" + items[rowIndex];

							RIChip chip = dataSet[key];

                            //if (IsOutputDetail && riItemIndex.ContainsKey("WATT_1"))
                            //{
                            //    items[ciez3Index] = ((int)chip.Mark).ToString();

                            //    line = string.Empty;

                            //    for (int i = 0; i < items.Count; i++)
                            //    {
                            //        line += items[i];

                            //        if (i != items.Count - 1)
                            //        {
                            //            line += ",";
                            //        }
                            //    }
                            //}

							if (chip.Mark == ERIChipMark.InterpolationFinish ||
								chip.Mark == ERIChipMark.InterpolationFinishAndElecFail)
							{
								foreach (var item in riItemIndex)
								{
									line = string.Empty;

									if (chip.Mark == ERIChipMark.InterpolationFinish)
									{
										items[item.Value] = chip.RawData[item.Key].ToString(formatDic[item.Key]);
									}

									for (int i = 0; i < items.Count; i++)
									{
										line += items[i];

										if (i != items.Count - 1)
										{
											line += ",";
										}
									}
								}
							}
						}
						else if (line == titleStr)
						{
							isRawData = true;
						}

						sw.WriteLine(line);
					}
				}
			}

            return true;
		}
	}
}
