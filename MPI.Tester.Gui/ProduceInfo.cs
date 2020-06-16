using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester.TestServer;
using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public class ProduceInfo
	{
		public static void SaveProduceInfo(UISetting uiSetting, EServerQueryCmd cmd)
		{
			try
			{
				switch (DataCenter._uiSetting.UserID)
				{
					case EUserID.Lumitek:
						ProduceInfo.Lumitek(uiSetting, cmd);
						break;
					//-----------------------------------------------------------------
					case EUserID.AquaLite:
						ProduceInfo.AquaLite(uiSetting, cmd);
						break;
					//-----------------------------------------------------------------
					case EUserID.GPI:
						ProduceInfo.GPI(uiSetting, cmd);
						break;
					//-----------------------------------------------------------------
					default:
						break;
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("[ProduceInfo], SaveProduceInfo()" + e.ToString());
			}
		}

		private static void Lumitek(UISetting uiSetting, EServerQueryCmd cmd)
		{
			Console.WriteLine("[ProduceInfo], Lumitek(), " + cmd.ToString());

			switch (cmd)
			{
				case EServerQueryCmd.CMD_TESTER_START:
					{
						DateTime dt = DataCenter._sysSetting.StartTestTime;

						string saveReportLocalPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "Date" + dt.Day.ToString("00") + ".CSV");

						string saveReportServerPath = Path.Combine(uiSetting.MESPath2, "Date" + dt.Day.ToString("00") + ".CSV");

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

						string[] data = new string[6] { dt.ToString("hh:mm:ss"), uiSetting.MachineName, uiSetting.WaferNumber, "OpenFile", uiSetting.TotalSacnCounts.ToString(), uiSetting.ProductFileName };

						file.Add(data);

						if (!CSVUtil.WriteCSV(saveReportLocalPath, file))
						{
							Console.WriteLine("[ProduceInfo], Lumitek(), Fail:" + cmd.ToString() + ",Path:" + saveReportLocalPath);
						}

						if (!MPIFile.CopyFile(saveReportLocalPath, saveReportServerPath))
						{
							Console.WriteLine("[ProduceInfo], Lumitek(), Fail:" + cmd.ToString() + ",Path:" + saveReportServerPath);
						}

						break;
					}
				case EServerQueryCmd.CMD_TESTER_END:
				case EServerQueryCmd.CMD_TESTER_ABORT:
					{
						DateTime dt = DateTime.Now;

						string saveReportLocalPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "Date" + dt.Day.ToString("00") + ".CSV");

						string saveReportServerPath = Path.Combine(uiSetting.MESPath2, "Date" + dt.Day.ToString("00") + ".CSV");

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

						string[] data = new string[6] { dt.ToString("hh:mm:ss"), uiSetting.MachineName, uiSetting.WaferNumber + ".CSV", "CloseFile", AppSystem._bodyDataCount.ToString(), uiSetting.ProductFileName };

						file.Add(data);

						if (!CSVUtil.WriteCSV(saveReportLocalPath, file))
						{
							Console.WriteLine("[ProduceInfo], Lumitek(), Fail:" + cmd.ToString() + ",Path:" + saveReportLocalPath);
						}

						if (!MPIFile.CopyFile(saveReportLocalPath, saveReportServerPath))
						{
							Console.WriteLine("[ProduceInfo], Lumitek(), Fail:" + cmd.ToString() + ",Path:" + saveReportServerPath);
						}

						break;
					}
				//-----------------------------------------------------------------
				default:
					break;
			}
		}

		private static void AquaLite(UISetting uiSetting, EServerQueryCmd cmd)
		{
			Console.WriteLine("[ProduceInfo], AquaLite(), " + cmd.ToString());

			switch (cmd)
			{
				case EServerQueryCmd.CMD_TESTER_START:
					{
						DateTime dt = DataCenter._sysSetting.StartTestTime;

						string fileName = string.Empty;

						if (DataCenter._uiSetting.TestResultFileName[0] == 'Q')
						{
							fileName = "TFULL_" + DataCenter._uiSetting.TestResultFileName + "_" + dt.ToString("yyyyMMddhhmm");
						}
						else if (DataCenter._uiSetting.TestResultFileName[0] == 'C')
						{
							fileName = "TCOW_" + DataCenter._uiSetting.TestResultFileName + "_" + dt.ToString("yyyyMMddhhmm");
						}
						else
						{
							fileName = DataCenter._uiSetting.TestResultFileName + "_" + dt.ToString("yyyyMMddhhmm");
						}

						string saveReportLocalPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, fileName + ".CSV");

						string saveReportServerPath = Path.Combine(uiSetting.MESPath2, fileName + ".CSV");

						List<string[]> file = new List<string[]>();

						file.Add(new string[] { "Wafer ID", DataCenter._uiSetting.WaferNumber });

						file.Add(new string[] { "Start Time", dt.ToString("yyyyMMdd hh:mm:ss") });

						file.Add(new string[] { "Machine ID", DataCenter._uiSetting.MachineName });

						file.Add(new string[] { "User ID", DataCenter._uiSetting.LoginID });

						if (!CSVUtil.WriteCSV(saveReportLocalPath, file))
						{
							Console.WriteLine("[ProduceInfo], Lumitek(), Fail:" + cmd.ToString() + ",Path:" + saveReportLocalPath);
						}

						if (!MPIFile.CopyFile(saveReportLocalPath, saveReportServerPath))
						{
							Console.WriteLine("[ProduceInfo], Lumitek(), Fail:" + cmd.ToString() + ",Path:" + saveReportServerPath);
						}

						break;
					}
				//-----------------------------------------------------------------
				default:
					break;
			}
		}

		private static void GPI(UISetting uiSetting, EServerQueryCmd cmd)
		{
			Console.WriteLine("[ProduceInfo], GPI(), " + cmd.ToString());

			switch (cmd)
			{
				case EServerQueryCmd.CMD_TESTER_END:
				case EServerQueryCmd.CMD_TESTER_ABORT:
					{
						DateTime dt = DateTime.Now;

						string fileName = uiSetting.WaferNumber + ".amo";

						string saveReportLocalPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, fileName);

						string saveReportServerPath = Path.Combine(uiSetting.MESPath2, fileName);

						List<string[]> file = new List<string[]>();

						string[] title = new string[7] { "PROBE_TYPE", "TAPE_NO", "JOB_IN_DATETIME", "CONFIGFILE_NAME1", "CONFIGFILE_NAME2", "JOB_IN_USER", "MACHINE_NO" };

						string[] body = new string[7] { uiSetting.WeiminUIData.Remark01 ,
														uiSetting.WaferNumber,
														uiSetting.WeiminUIData.Remark02,
														uiSetting.TaskSheetFileName,
														uiSetting.TaskSheetFileName,
														uiSetting.OperatorName,
														uiSetting.MachineName};

						file.Add(title);

						file.Add(body);

						if (!CSVUtil.WriteCSV(saveReportLocalPath, file))
						{
							Console.WriteLine("[ProduceInfo], GPI(), Fail:" + cmd.ToString() + ",Path:" + saveReportLocalPath);
						}

						if (!MPIFile.CopyFile(saveReportLocalPath, saveReportServerPath))
						{
							Console.WriteLine("[ProduceInfo], GPI(), Fail:" + cmd.ToString() + ",Path:" + saveReportServerPath);
						}

						break;
					}
				//-----------------------------------------------------------------
				default:
					break;
			}
		}
	}
}