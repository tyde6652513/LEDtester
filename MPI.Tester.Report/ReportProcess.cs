using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester.TestKernel;
using MPI.Tester.Tools;
using MPI.Tester.Data.ChannelCoordTable;

namespace MPI.Tester.Report
{
	public class ReportProcess
	{
		#region >>> Private Static Property <<<

		private static bool _isImplement = false;
		private static ReportBase _report;

		#endregion

		#region >>> Public Static Method <<<

		public static void SetData(List<object> objs)
		{
			ReportProcess._isImplement = true;

			ReportProjectData reportData = new ReportProjectData();

			objs.Add(reportData);

			////////////////////////////////////////////////////////////
			//Get Info
			////////////////////////////////////////////////////////////
			UISetting uiSetting = null;

			ProductData productData = null;

			foreach (var item in objs)
			{
				if (item is UISetting)
				{
					uiSetting = item as UISetting;
				}
				else if (item is ProductData)
				{
					productData = item as ProductData;
				}
			}

            if (_report != null)
            {
                _report.CloseFile();
            }

			////////////////////////////////////////////////////////////
			//Create Report
			////////////////////////////////////////////////////////////
			switch (uiSetting.UserID)
			{
				case EUserID.MPI_COB:
					{
						_report = new MPI.Tester.Report.User.MPI.Report(objs, false);

						break;
					}
				//-----------------------------------------------------------------
				case EUserID.TSB:
					{
						_report = new MPI.Tester.Report.User.TSB.Report(objs, false);

						break;
					}
				//-----------------------------------------------------------------
				case EUserID.HP000:
					{
						_report = new MPI.Tester.Report.User.HP000.Report(objs, false);

						break;
					}
				//-----------------------------------------------------------------
				case EUserID.EPISKY:
					{
						_report = new MPI.Tester.Report.User.EPISKY.Report(objs, false);

						break;
					}
				//-----------------------------------------------------------------
				case EUserID.Lumitek:
					{
						_report = new MPI.Tester.Report.User.Lumitek.Report(objs, false);

						break;
					}
				//-----------------------------------------------------------------
				case EUserID.GPI:
					{
						_report = new MPI.Tester.Report.User.GPI.Report(objs, false);

						break;
					}
				//-----------------------------------------------------------------
				case EUserID.ChangeLight:
					{
						_report = new MPI.Tester.Report.User.ChangeLight.Report(objs, false);

						break;
					}
                //-----------------------------------------------------------------
                case EUserID.UOC:  // LD
                    {
                        _report = new MPI.Tester.Report.User.UOC.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.TSMC01: // MOSFET
                    {
                        _report = new MPI.Tester.Report.User.TSMC01.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.Heptagon:  // LD
                    {
                        _report = new MPI.Tester.Report.User.Heptagon.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.Picometrix:  // PD
                    {
                        _report = new MPI.Tester.Report.User.Picometrix.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.ATV:  // IR LED
                    {
                        _report = new MPI.Tester.Report.User.ATV.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.Eti:  // NTLM & Flash
                    {
                        _report = new MPI.Tester.Report.User.Eti.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.InfiniLED:  // Micro LED
                    {
                        _report = new MPI.Tester.Report.User.InfiniLED.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.Luxnet:  // LD VCSEL
                    {
                        _report = new MPI.Tester.Report.User.Luxnet.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.MPI_LD:  // MPI LD
                    {
                        _report = new MPI.Tester.Report.User.MPI_LD.Report(objs, false);

                        break;
                    }
  				//-----------------------------------------------------------------
                case EUserID.Sanan_Xiamen:
                    {
                        _report = new MPI.Tester.Report.User.Sanan_Xiamen.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.CHT:  // 
                    {
                        _report = new MPI.Tester.Report.User.CHT.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.MPI_LCR:  // 
                    {
                        _report = new MPI.Tester.Report.User.MPI_LCR.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.AOI:  // 
                    {
                        _report = new MPI.Tester.Report.User.AOI.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.MPI_DEMO:
                    {
                        _report = new MPI.Tester.Report.User.MPI_DEMO.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.MPI_Python:
                    {
                        _report = new MPI.Tester.Report.User.MPI_Python.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.TYNTE:
                    {
                        _report = new MPI.Tester.Report.User.TYNTE.Report(objs, false);

                        break;
                    }
                //----------------------------------------
                case EUserID.Emcore:
                    {
                        _report = new MPI.Tester.Report.User.Emcore.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.WAVETEK00:
                    {
                        _report = new MPI.Tester.Report.User.WAVETEK00.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.EpiStar:
                    {
                        _report = new MPI.Tester.Report.User.EpiStar.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------\
                case EUserID.DOWA:
                    {
                        _report = new MPI.Tester.Report.User.DOWA.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.OptoTech:
                    {
                        _report = new MPI.Tester.Report.User.OptoTech.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.Accelink:
                    {
                        _report = new MPI.Tester.Report.User.Accelink.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------
                case EUserID.ASE:
                    {
                        _report = new MPI.Tester.Report.User.ASE.Report(objs, false);

                        break;
                    }
                //-----------------------------------------------------------------

				default:
					{
						ReportProcess._isImplement = false;

						_report = null;

						break;
					}

			}

			ReportBase.ResetStatisticData(productData);

			if (_report != null)
			{
				uiSetting.TestResultFileName = _report.TestResultFileNameWithoutExt();
			}
		}

		public static EErrorCode RunCommand(EServerQueryCmd cmd)
		{
			if (_report == null)
			{
				return EErrorCode.NONE;
			}

			return _report.RunCommand(cmd);
		}

		public static EErrorCode Push(Dictionary<string, double> data,bool IsMultoiStage = false)
		{
			if(GlobalFlag.IsStopPushData)
			{
				return EErrorCode.NONE;
			}

			ReportBase.PushStatisticData(data);

			if (_report == null)
			{
				return EErrorCode.NONE;
			}
			else
			{
				return _report.Push(data,IsMultoiStage);
			}
		}

		public static Statistic StatisticData(string keyName, EStatisticType type)
		{
			return ReportBase.StatisticData(keyName, type);
		}

		public static Statistic StatisticData(ESysResultItem keyName, EStatisticType type)
		{
			return ReportBase.StatisticData(keyName, type);
		}

		public static double GoodRate01(string keyName)
		{
			return ReportBase.GoodRateS01(keyName);
		}

		public static double GoodRate02(string keyName)
		{
			return ReportBase.GoodRateS02(keyName);
		}

		public static bool IsCanAppendFile(out int lastCol, out int lastRow)
		{
			lastCol = int.MinValue;

			lastRow = int.MinValue;

			if (_report == null)
			{
				return false;
			}

			return _report.IsCanAppendFile(out lastCol, out lastRow);
		}

		public static bool IsOutputReportExist(bool isDeleteExistFile = false)
		{
			if (_report == null)
			{
				return false;
			}

			return _report.IsOutputReportExist();
		}

		public static bool OpenReport(string fileFullName)
		{
			return ReportProcess._report.OpenReport(fileFullName);
		}

		public static Dictionary<string, float> ReadLine()
		{
			return ReportProcess._report.ReadLine();
		}

		public static ColRow GetChannel1ColRow(int col, int row)
		{
            if (ReportProcess._report == null)
            {
                return null;
            }

			return ReportProcess._report.GetChannel1ColRow(col, row);
		}

		public static EErrorCode CombineAndSimulatorReportInterpolation(string optiReport, string elecReport)
		{
            if (ReportProcess._report == null)
            {
                return EErrorCode.SaveFileFail;
            }

			return ReportProcess._report.CombineSimulatorReportInterpolation(optiReport, elecReport);
		}

        public static EErrorCode SimulatorReportInterpolation(string Report)
        {
            if (ReportProcess._report == null)
            {
                return EErrorCode.SaveFileFail;
            }

            return ReportProcess._report.SimulatorReportInterpolation(Report);
        }
        
        public static EErrorCode MergeFile(string outputPath,List<string> fileList = null)
        {
            return ReportProcess._report.MergeFile(outputPath,fileList);
        }

        public static string  GetFilePath(int fileNum = 1)
        {
            return ReportProcess._report.GetFilePath(fileNum);
        }

        public static EErrorCode CopyDataToTemp()
        {
            if (ReportProcess._report != null)
            {
                return ReportProcess._report.CloneFileToTemp();
            }
            else
            {
                Console.WriteLine("[ReportProcess],CloneFileToTemp(), _report is null");
                return EErrorCode.REPORT_File_To_Temp_Fail;
            }
        }

        public static EErrorCode CreateMapFromTemp()
        {
            if (ReportProcess._report != null)
            {
                return ReportProcess._report.CreateMapFromTemp();
            }
            else
            {
                Console.WriteLine("[ReportProcess],CreateMapFromTemp(), _report is null");
                return EErrorCode.REPORT_File_To_Temp_Fail;
            }
        }

        public static string GetOutputFileName()
        {
            if (ReportProcess._report != null)
            {
                return ReportProcess._report.GetOutputFileName();
            }
            else
            {
                Console.WriteLine("[ReportProcess],GetOutputFileName(), _report is null");
                return "";
            }
 
        }

        public static void PushRemarkLog(string str)
        {
            if (ReportProcess._report != null)
            {
                ReportProcess._report.PushRemarkInfo(str);
            }
            
        }
		#endregion

		#region >>> Public Static Property <<<

		public static bool IsImplement
		{
			get { return ReportProcess._isImplement; }
		}

		public static bool IsImplementLIVReport
		{
			get 
			{
				if (_report == null)
				{
					return false;
				}

				return _report.IsImplementLIVReport; 
			}
		}

		public static bool IsImplementSpectrumReport
		{
			get 
			{
				if (_report == null)
				{
					return false;
				}

				return _report.IsImplementSpectrumReport; 
			}
		}

		public static bool IsImplementSweepDataReport
		{
			get 
			{
				if (_report == null)
				{
					return false;
				}

				return _report.IsImplementSweepDataReport; 
			}
		}

		public static bool IsImplementPIVDataReport
		{
			get 
			{
				if (_report == null)
				{
					return false;
				}

				return _report.IsImplementPIVDataReport; 
			}
		}

		public static bool IsTempFileExist
		{
			get
			{
				if (_report == null)
				{
					return false;
				}

				return _report.IsTempFileExist;
			}
		}

		public static int TestCount
		{
			get { return ReportBase.TestCount; }
		}

		public static string TestResultFileName
		{
			get
			{
				if (_report == null)
				{
					return string.Empty;
				}

				return _report.TestResultFileName;
			}
		}

        public static CoordTransferTool CustomizeCoordTransTool
        {
            get
            {
                if (_report == null)
                {
                    return null;
                }

                return _report.CustomizeCoordTransTool;
            }

            set 
            {
                if (_report != null)
                {
                    _report.CustomizeCoordTransTool = value;
                }
            }
        }

        public static CoordTransferTool CoordTransferTool
        {
            get
            {
                if (_report == null)
                {
                    return null;
                }

                return _report.CoordTransTool;
            }

            set 
            {
                if (_report != null)
                {
                    _report.CoordTransTool = value;
                }
            }
        }

        public static ChannelPosShiftTable<int> Channel2PosTable
        {
            get
            {
                if (_report == null)
                {
                    return null;
                }

                return _report.Channel2PosTable;
            }

            set
            {
                if (_report != null)
                {
                    _report.Channel2PosTable = value;
                }
            }
        }

        
		#endregion
	}
}
