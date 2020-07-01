using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Linq;

using MPI.Tester.TestServer;
using MPI.Tester.TestKernel;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using MPI.Tester.MES;
using MPI.Tester.Maths;
using MPI.Tester.Report;
using MPI.Tester.Tools;

namespace MPI.Tester.Gui
{
    public class ShowMapDataEventArgs : EventArgs
    {
        public readonly Dictionary<string, float> Values;
        public readonly int Row, Col;

        public ShowMapDataEventArgs(int row, int col, Dictionary<string, float> result)
        {
            this.Row = row;
            this.Col = col;
            this.Values = result;
        }
    }

    public class SwitchUIArgs : EventArgs
    {
        public readonly int Index;

        public SwitchUIArgs(int index)
        {
            this.Index = index;
        }
    }

    public class SaveBinMapArgs : EventArgs
    {
        public readonly string _fileName;

        public SaveBinMapArgs(string fileName)
        {
            this._fileName = fileName;
        }
    }

    public class AppSystem
    {
        private object _lockObj;

        private static TestServer.TestServer _MPITestServer = null;
        private static TestServer.TCPTestServer _MPITCPTestServer = null;
        private static TestServer.IOServer _MPIIOTestServer = null;
        public static TesterKernelBase _MPITesterKernel = null;

        public static event EventHandler OnAppSystemRun;
        public static event EventHandler<ShowMapDataEventArgs> ShowMapDataEvent;
        public static event EventHandler<SwitchUIArgs> SwitchUIEvent;

        public static event EventHandler<SwitchUIArgs> PopDialogEvnet;

        public static event EventHandler<SaveBinMapArgs> SaveBinMapEvnet;

        //------------------------------------------------------------------------------------------------
        // Alec, 20130531
        // 以 UserData 紀錄的量測輸出項目為主軸的 bodyDataList
        // _bodyDataDic.Keys 紀錄 UserData 的 ResultItem、ExtResultItem，以及程式內部的 ESD keyNames
        // 並記錄每個 key 分別對應到 OutputTestResult 和 _bodyDataList 的資訊 Index 加入 bodyDataDic02
        //------------------------------------------------------------------------------------------------
        public static int _bodyDataCount;
        public static Dictionary<string, int> _bodyDataHeadDic;
        public static List<float[]> _bodyDataList;
        public static OutputBigData _outputBigData = new OutputBigData();

        public static Dictionary<string, int[]> _bodyDataDic;
        public static List<Dictionary<string, List<float>>> _bodyDataList03;
        private static List<string> _bodyRowColIndex;

        // Gilbert, 20121223,  紀錄的量測項目中的 RawDataArray
        public static List<string[]> _rawDataHead;
        public static List<double[]> _rawDataList;
        // Gilbert, 20121223,  紀錄的有異常的光譜資料

        public static string[][] _mapItemNames = new string[2][];

        private static List<double> _rawData;
        private static List<Dictionary<string, float>> _consecutiveRecordData = new List<Dictionary<string, float>>(3);

        private static PreSamplingCheck _preSamplingCheck = new PreSamplingCheck();

        public static AutoCalibChannelGain _autoCalibChannelGain = new AutoCalibChannelGain();

        public static ETCPClientState _tcpipClientState = ETCPClientState.NONE;

        private static string _lastProdcutRecipe = string.Empty;

        private static CoordTransferTool _customizeCoordTransTool = new CoordTransferTool();

        private static CoordTransferTool _p2TcoordTransTool = new CoordTransferTool();

        private static Dictionary<string, int> _oriboundaryDic;

        private static int _laserCheckCnt = 0;

        public AppSystem()
        {
            this._lockObj = new object();

            ResetDataList();

            _customizeCoordTransTool = new CoordTransferTool();
            _p2TcoordTransTool = new CoordTransferTool();
            _oriboundaryDic = new Dictionary<string, int>();
            _laserCheckCnt = 0;

        }

        #region >>> Private Static Methods <<<

        private static void ServerQueryEventHandler(object sender, ServerQueryEventArg e)
        {

            //if (GlobalFlag.TestMode == ETesterTestMode.Overload)//for Emcore Overload Test
            //{
            //    if (e.CmdID == (int)EServerQueryCmd.CMD_LOAD_ITEM_FILE ||
            //       e.CmdID == (int)EServerQueryCmd.CMD_APPEND_TESTER_OUTPUT_FILE ||
            //       e.CmdID == (int)EServerQueryCmd.CMD_LOAD_FILE_TO_TEMP||
            //        e.CmdID == (int)EServerQueryCmd.CMD_TESTER_START||
            //        e.CmdID == (int)EServerQueryCmd.CMD_TESTER_END)
            //    {

            //        Console.WriteLine("[ServerQueryEventHandler], ServerQueryEventHandler ,skip since Overload, cmd is: " + e.CmdID);
            //        return;
            //    }
            //}
            switch (e.CmdID)
            {
                case (int)EServerQueryCmd.CMD_LOAD_ITEM_FILE:
                    #region
                    {
                        Console.WriteLine("[ServerQueryEventHandler], CMD_LOAD_ITEM_FILE");
                        
                        //if (GlobalFlag.TestMode == ETesterTestMode.Normal)
                        {

                            if (DataCenter._uiSetting.IsEnableRunMesSystem && DataCenter._uiSetting.UserID != EUserID.Eti)
                            {
                                Console.WriteLine("[ServerQueryEventHandler], Run MES Mode TaskSheetFileName:" + e.StrData[0]);

                                DataCenter._uiSetting.TaskSheetFileName = e.StrData[0];
                                GlobalFlag.IsSuccessLoadProduct = true;
                                GlobalFlag.IsSuccessLoadBin = true;
                            }
                            else
                            {
                                Console.WriteLine("[ServerQueryEventHandler], CMD_LOAD_ITEM_FILE, TaskSheetFileName:" + e.StrData[0]);

                                // 當收到空字串時，傳回錯誤數值
                                if (e.StrData[0] == string.Empty)
                                {
                                    GlobalFlag.IsSuccessLoadBin = false;
                                    GlobalFlag.IsSuccessLoadProduct = false;
                                    Console.WriteLine("[ServerQueryEventHandler], CMD_LOAD_ITEM_FILE, Recipe Name equal to Empty");
                                    break;
                                }

                                if (DataCenter.CheckTaskSheetIsExist(e.StrData[0]))
                                {
                                    if (DataCenter.LoadTaskSheet(e.StrData[0],true))
                                    {
                                        AppSystem.SetDataToSystem();

                                        AppSystem.CheckMachineHW();

                                        Host.UpdateDataToAllUIForm();

                                        GlobalFlag.IsSuccessLoadProduct = true;
                                        GlobalFlag.IsSuccessLoadBin = true;

                                    }
                                    else
                                    {
                                        GlobalFlag.IsSuccessLoadBin = false;
                                        GlobalFlag.IsSuccessLoadProduct = false;
                                    }
                                }
                                else
                                {
                                    GlobalFlag.IsSuccessLoadBin = false;
                                    GlobalFlag.IsSuccessLoadProduct = false;
                                }
                            }
                            if (DataCenter._uiSetting.IsShowReportCommentsUI)
                            {
                                GlobalFlag.IsEnableEndTest = false;
                            }
                        }
                        //else
                        //{
                        //    Console.WriteLine("[ServerQueryEventHandler],CMD_LOAD_ITEM_FILE, Overload Test Mode");
                        //}
                        break;
                    }
                    #endregion
                //----------------------------------------------------------------------------------------------------------------------
 				case (int)EServerQueryCmd.CMD_CHECK_AVALIABLE_MODE: //20180607 David
                    {
                        #region
                        //////////////////////////////////////////////////////////////////////
                        // Log  Data
                        //////////////////////////////////////////////////////////////////////
                        Console.WriteLine("[ServerQueryEventHandler], CMD_CHECK_AVALIABLE_MODE");
                        Console.WriteLine("Barcode:" + e.StrData[0]);
                        Console.WriteLine("LotNumber:" + e.StrData[1]);
                        Console.WriteLine("CassetteID:" + e.StrData[2]);
                        Console.WriteLine("WaferNumber:" + e.StrData[3]);
                        Console.WriteLine("OPID:" + e.StrData[4]);
                        Console.WriteLine("ProductType:" + e.StrData[6]);
                        Console.WriteLine("Substrate:" + e.StrData[7]);
                        Console.WriteLine("CassetteNumber:" + e.StrData[8]);
                        Console.WriteLine("SoltNumber:" + e.StrData[9]);
                        Console.WriteLine("CustomerKeyNumber:" + e.StrData[10]);

                        //////////////////////////////////////////////////////////////////////
                        // Set  Data
                        //////////////////////////////////////////////////////////////////////
                        #region
                        if (e.StrData[0].Equals("") || !e.StrData[0][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.Barcode = e.StrData[0];
                            DataCenter._uiSetting.WeiminUIData.KeyInFileName = e.StrData[0];
                        }

                        if (e.StrData[1].Equals("") || !e.StrData[1][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.LotNumber = e.StrData[1];
                            DataCenter._uiSetting.WeiminUIData.LotNumber = e.StrData[1];
                        }

                        if (e.StrData[2].Equals("") || !e.StrData[2][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.CassetteNumber = e.StrData[2];
                        }

                        if (e.StrData[3].Equals("") || !e.StrData[3][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.WaferNumber = e.StrData[3];
                        }

                        if (e.StrData[4].Equals("") || !e.StrData[4][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.OperatorName = e.StrData[4];
                        }

                        if (e.StrData[6].Equals("") || !e.StrData[6][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.ProductType = e.StrData[6];
                        }

                        if (e.StrData[7].Equals("") || !e.StrData[7][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.Substrate = e.StrData[7];
                        }

                        if (e.StrData[8].Equals("") || !e.StrData[8][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.CassetteNumber = e.StrData[8];
                        }

                        if (e.StrData[9].Equals("") || !e.StrData[9][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.SlotNumber = e.StrData[9];
                        }

                        if (e.StrData[10].Equals("") || !e.StrData[10][0].Equals((char)0x01))
                        {
                            DataCenter._uiSetting.KeyNumber = e.StrData[10];
                        }
                        #endregion
                       
                        //////////////////////////////////////////////////////////////////////
                        // Check Output State
                        //////////////////////////////////////////////////////////////////////
                        Host._MPIStorage.GenerateOutputFileName();

                        if (ReportProcess.IsImplement)
                        {
                            GlobalFlag.OutputReportState = EOutputReportState.None;

                            //Write Report by Report Project
                            if (ReportProcess.TestResultFileName == string.Empty)
                            {
                                GlobalFlag.OutputReportState = EOutputReportState.FileNameIsEmpty;
                            }
                            else if (DataCenter._sysSetting.IsPresampling)
                            {
                                //TempFile: not exist, Report: not exist
                                GlobalFlag.OutputReportState = EOutputReportState.None;
                            }
                            else if (ReportProcess.IsTempFileExist)
                            {
                                int lastCol = int.MinValue;

                                int lastRow = int.MinValue;

                                if (ReportProcess.IsCanAppendFile(out lastCol, out lastRow))
                                {
                                    //TempFile: exist, Format OK
                                    GlobalFlag.OutputReportState = EOutputReportState.CanAppend;

                                    DataCenter.ChangeRowColToProbe(ref lastCol, ref lastRow);

                                    GlobalData.ContinueModeCol = lastCol;

                                    GlobalData.ContinueModeRow = lastRow;



                                    ReportProcess.CreateMapFromTemp();
                                }
                                else
                                {
                                    //TempFile: exist, Format Error
                                    GlobalFlag.OutputReportState = EOutputReportState.CanNotAppend;
                                }
                            }

                            if (ReportProcess.IsOutputReportExist())
                            {
                                //TempFile: not exist, Report: exist 
                                if (GlobalFlag.OutputReportState == EOutputReportState.CanAppend)
                                {
                                    //GlobalFlag.OutputReportState = EOutputReportState.CanAppendAndRetest;
                                }
                                else
                                {
                                    GlobalFlag.OutputReportState = EOutputReportState.CanRetest;
                                }
                            }
                        }


                        //Host.UpdateDataToAllUIForm();

                        break;

                        #endregion
					}
				//----------------------------------------------------------------------------------------------------------------------
				                
                case (int)EServerQueryCmd.CMD_OPEN_OUTPUT_FILE:
                    #region
                    {
                        //////////////////////////////////////////////////////////////////////
                        // Log  Data
                        //////////////////////////////////////////////////////////////////////
                        Console.WriteLine("[ServerQueryEventHandler], CMD_OPEN_OUTPUT_FILE");
                        
                        //if (GlobalFlag.TestMode == ETesterTestMode.Normal)
                        {
                            Console.WriteLine("Barcode:" + e.StrData[0]);
                            Console.WriteLine("LotNumber:" + e.StrData[1]);
                            Console.WriteLine("CassetteID:" + e.StrData[2]);
                            Console.WriteLine("WaferNumber:" + e.StrData[3]);
                            Console.WriteLine("OPID:" + e.StrData[4]);
                            Console.WriteLine("ProberRecipe:" + e.StrData[5]);
                            Console.WriteLine("ProductType:" + e.StrData[6]);
                            Console.WriteLine("Substrate:" + e.StrData[7]);
                            Console.WriteLine("CassetteNumber:" + e.StrData[8]);
                            Console.WriteLine("SoltNumber:" + e.StrData[9]);
                            Console.WriteLine("CustomerKeyNumber:" + e.StrData[10]);
							if ( e.BufferData.Length > 1 )
							{
                            Console.WriteLine("TestTemperature:" + e.BufferData[1].ToString("0.##"));
							}
                            //////////////////////////////////////////////////////////////////////
                            // Set  Data
                            //////////////////////////////////////////////////////////////////////
                            if (e.StrData[0].Equals("") || !e.StrData[0][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.Barcode = e.StrData[0];
                                DataCenter._uiSetting.WeiminUIData.KeyInFileName = e.StrData[0];
                            }

                            if (e.StrData[1].Equals("") || !e.StrData[1][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.LotNumber = e.StrData[1];
                                DataCenter._uiSetting.WeiminUIData.LotNumber = e.StrData[1];
                            }

                            if (e.StrData[2].Equals("") || !e.StrData[2][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.CassetteNumber = e.StrData[2];
                            }

                            if (e.StrData[3].Equals("") || !e.StrData[3][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.WaferNumber = e.StrData[3];
                            }

                            if (e.StrData[4].Equals("") || !e.StrData[4][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.OperatorName = e.StrData[4];
                            }
                            if (e.StrData[5].Equals("") || !e.StrData[5][0].Equals((char)0x01))
                            {
                                if (!DataCenter._uiSetting.IsEnableFloatReport)//20181025 AOI後續若要修改，記得通知 IsEnableFloatReport要勾起來
                                {
                                    DataCenter._uiSetting.ProberRecipeName = e.StrData[5];
                                }
                            }

                            if (e.StrData[6].Equals("") || !e.StrData[6][0].Equals((char)0x01))
                            {
                                if (!DataCenter._uiSetting.IsEnableFloatReport)
                                {
                                    DataCenter._uiSetting.ProductType = e.StrData[6];
                                }
                            }

                            if (e.StrData[7].Equals("") || !e.StrData[7][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.Substrate = e.StrData[7];
                            }

                            if (e.StrData[8].Equals("") || !e.StrData[8][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.CassetteNumber = e.StrData[8];
                            }

                            if (e.StrData[9].Equals("") || !e.StrData[9][0].Equals((char)0x01))
                            {
                                if (!DataCenter._uiSetting.IsEnableFloatReport)
                                {
                                    DataCenter._uiSetting.SlotNumber = e.StrData[9];
                                }
                            }

                            if (e.StrData[10].Equals("") || !e.StrData[10][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.KeyNumber = e.StrData[10];
                            }

                            DataCenter._sysSetting.IsPresampling = (e.BufferData[0] == 1);

							if ( e.BufferData.Length > 1 )
							{
                            DataCenter._uiSetting.ChuckTemprature = e.BufferData[1];
							}
							else
							{
								DataCenter._uiSetting.ChuckTemprature = 25;
							}

                            DataCenter._uiSetting.TopDisplayUIForm = 1;

                            DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.AutoRun;

                            //////////////////////////////////////////////////////////////////////
                            // Check Operator Name is not Empty
                            //////////////////////////////////////////////////////////////////////
                            GlobalFlag.IsOperatorEmpty = DataCenter._uiSetting.OperatorName == string.Empty;

                            //////////////////////////////////////////////////////////////////////
                            // Run MES
                            //////////////////////////////////////////////////////////////////////
                            EErrorCode errorCode = MESCtrl.LoadRecipe();

                            if (errorCode == EErrorCode.NONE)
                            {
                                GlobalFlag.IsSuccessLoadMESData = true;
                                GlobalFlag.IsDailyCheckFail = false;
                            }
                            else if (errorCode == EErrorCode.MES_DailyCheckIsNotReady)
                            {
                                GlobalFlag.IsSuccessLoadMESData = true;
                                GlobalFlag.IsDailyCheckFail = true;
                                DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                            }
                            else
                            {
                                GlobalFlag.IsSuccessLoadMESData = false;
                                GlobalFlag.IsDailyCheckFail = false;
                                DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                            }

                            //////////////////////////////////////////////////////////////////////
                            // Daily Check
                            //////////////////////////////////////////////////////////////////////
                            DataCenter._uiSetting.IsShowDailyCheckUI = false;

                            if (DataCenter._uiSetting.IsRunDailyCheckMode)
                            {
                                DataCenter._uiSetting.IsManualRunMode = true;
                            }
                            else
                            {
                                DataCenter._uiSetting.IsManualRunMode = false;
                            }

                            //////////////////////////////////////////////////////////////////////
                            // Check Output State
                            //////////////////////////////////////////////////////////////////////
                            Host._MPIStorage.GenerateOutputFileName();

                            if (ReportProcess.IsImplement)
                            {
                                //Write Report by Report Project
                                if (ReportProcess.TestResultFileName == string.Empty)
                                {
                                    GlobalFlag.OutputReportState = EOutputReportState.FileNameIsEmpty;
                                }
                                else if (DataCenter._sysSetting.IsPresampling)
                                {
                                    //TempFile: not exist, Report: not exist
                                    GlobalFlag.OutputReportState = EOutputReportState.None;
                                }
                                else if (ReportProcess.IsTempFileExist)
                                {
                                    int lastCol = int.MinValue;

                                    int lastRow = int.MinValue;

                                    if (ReportProcess.IsCanAppendFile(out lastCol, out lastRow))
                                    {
                                        //TempFile: exist, Format OK
                                        GlobalFlag.OutputReportState = EOutputReportState.CanAppend;

                                        DataCenter.ChangeRowColToProbe(ref lastCol, ref lastRow);

                                        GlobalData.ContinueModeCol = lastCol;

                                        GlobalData.ContinueModeRow = lastRow;
                                    }
                                    else
                                    {
                                        //TempFile: exist, Format Error
                                        GlobalFlag.OutputReportState = EOutputReportState.CanOverwrite;
                                    }
                                }
                                else
                                {
                                    if (ReportProcess.IsOutputReportExist())
                                    {
                                        if (GlobalFlag.ProberAssignMode == EOutputReportState.None)
                                        {
                                            GlobalFlag.OutputReportState = EOutputReportState.FileNameExist;
                                        }
                                        else
                                        {
                                            GlobalFlag.OutputReportState = EOutputReportState.CanOverwrite;
                                        }
                                    }
                                    else
                                    {
                                        //TempFile: not exist, Report: not exist
                                        GlobalFlag.OutputReportState = EOutputReportState.None;
                                    }
                                }
                            }
                            else
                            {
                                //Write Report by xml
                                if (Host._MPIStorage.IsExistTestOutputFileName())
                                {
								GlobalFlag.OutputReportState = EOutputReportState.FileNameExist;
                                }
                                else
                                {
                                    GlobalFlag.OutputReportState = EOutputReportState.None;
                                }

                                if (DataCenter._uiSetting.TestResultFileName == string.Empty)
                                {
                                    GlobalFlag.IsTestResultFileEmpty = true;

                                    DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                                }
                            }

                            Host.UpdateDataToAllUIForm();

                        }
                        //else
                        //{
                        //    Console.WriteLine("[ServerQueryEventHandler],CMD_OPEN_OUTPUT_FILE, Overload Test Mode");
                        //}

                        break;
                    }
                    #endregion
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_OVERWRITE_TESTER_OUTPUT_FILE:
                    #region
                    {
                        Console.WriteLine("[ServerQueryEventHandler], CMD_OVERWRITE_TESTER_OUTPUT_FILE");

                        DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.AutoRun;

                        //if (!ReportProcess.IsImplement)//20190622
                        {
                            Host._MPIStorage.IsExistTestOutputFileName(true);
                        }

                        GlobalFlag.OutputReportState = EOutputReportState.None;

                        break;
                    }
                    #endregion
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_APPEND_TESTER_OUTPUT_FILE:
                    #region
                    {
                        Console.WriteLine("[ServerQueryEventHandler], CMD_APPEND_TESTER_OUTPUT_FILE");

                        if (ReportProcess.IsImplement)
                        {
                            DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.AutoRun;

                            int lastCol = 0;

                            int lastRow = 0;

                            if (ReportProcess.IsCanAppendFile(out lastCol, out lastRow))
                            {
                                DataCenter.ChangeRowColToProbe(ref lastCol, ref lastRow);

                                if (DataCenter._uiSetting.IsManualSettingContiousProbingRowCol)
                                {
                                    GlobalData.ContinueModeCol = DataCenter._uiSetting.ContiousProbingPosX;

                                    GlobalData.ContinueModeRow = DataCenter._uiSetting.ContiousProbingPosY;
                                }
                                else
                                {
                                    GlobalData.ContinueModeCol = lastCol;

                                    GlobalData.ContinueModeRow = lastRow;
                                }
                                if (!DataCenter._uiSetting.IsRetest)
                                {
                                    DataCenter._uiSetting.IsAppend = true;
                                    DataCenter._uiSetting.IsAppendForWaferBegine = true;
                                }

                                GlobalFlag.OutputReportState = EOutputReportState.None;
                            }
                            else
                            {
                                GlobalFlag.OutputReportState = EOutputReportState.CanNotAppend;
                            }
                        }
                        else
                        {
                            GlobalFlag.OutputReportState = EOutputReportState.CanNotAppend;
                        }
                        break;
                    }
                    #endregion
					//----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_LOAD_FILE_TO_TEMP:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_LOAD_FILE_TO_TEMP");

                        if (ReportProcess.IsImplement)
                        {
                            DataCenter._uiSetting.IsRetest = true;
                            ReportProcess.CopyDataToTemp();
                        }

                        GlobalFlag.OutputReportState = EOutputReportState.None;

                        break;
                        #endregion
                    }
				//----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_CREATE_MAP_FROM_TEMP:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_CREATE_MAP_FROM_TEMP");

                        if (!ReportProcess.IsImplement)
                        {
                            //ReportProcess.CopyDataToTemp();
                        }

                        //GlobalFlag.OutputReportState = EOutputReportState.None;

                        break;
                        #endregion
					}
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_TESTER_START:
                    #region
                    {
                        Console.WriteLine("[ServerQueryEventHandler], CMD_TESTER_START");
                        
                        //if (GlobalFlag.TestMode == ETesterTestMode.Normal)
                        {

                            //buffer[0]=this._dataMemArray[(int)EMPITestServerDataIndex.ColMinimum]; //ColX min
                            //buffer[1]=this._dataMemArray[(int)EMPITestServerDataIndex.RowMinimum]; //RowY min
                            //buffer[2]=this._dataMemArray[(int)EMPITestServerDataIndex.ColMaximum]; //ColX max
                            //buffer[3]=this._dataMemArray[(int)EMPITestServerDataIndex.RowMaximum]; //RowY max
                            //buffer[4] = this._dataMemArray[(int)EMPITestServerDataIndex.TotalSacnCounts];
                            //buffer[5] = this._dataMemArray[(int)EMPITestServerDataIndex.ChipXPictch];
                            //buffer[6] = this._dataMemArray[(int)EMPITestServerDataIndex.ChipYPictch];
                            //buffer[7] = this._dataMemArray[(int)EMPITestServerDataIndex.TotalProbingCounts];
                            //buffer[8] = this._dataMemArray[(int)EMPITestServerDataIndex.MoveMainAxis];
                            //buffer[9] = this._dataMemArray[(int)EMPITestServerDataIndex.SamplingMode];
                            //buffer[10] = this._dataMemArray[(int)EMPITestServerDataIndex.XInitDirection];
                            //buffer[11] = this._dataMemArray[(int)EMPITestServerDataIndex.YInitDirection];
                            //buffer[12] = this._dataMemArray[(int)EMPITestServerDataIndex.ProbingCount1];
                            //buffer[13] = this._dataMemArray[(int)EMPITestServerDataIndex.ProbingCount2];
                            //buffer[14] = this._dataMemArray[(int)EMPITestServerDataIndex.ProbingCount3];
                            //buffer[15] = this._dataMemArray[(int)EMPITestServerDataIndex.ProbingCount4];                  
                            //buffer[16] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_ColX];   // Roy, Multi-Die Testing
                            //buffer[17] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_RowY];
                            //buffer[18] = this._dataMemArray[(int)EMPITestServerDataIndex.StartTemp];

                            //if (e.BufferData != null && e.BufferData.Length >= 16)   //  ***確認40P 是否備註記

                            int mainAxis = (int)e.BufferData[8];
                            int xInitialDirection = (int)e.BufferData[10];
                            int yInitialDirection = (int)e.BufferData[11];

                            uint xProberChannel = (uint)e.BufferData[16];   // Roy, 20140331, prober colX channel count
                            uint yProberChannel = (uint)e.BufferData[17];   //                prober rowY channel count

                            int tProberChannel = (int)e.BufferData[21];    // Roy, 20150202 prber channel rotation

                            DataCenter._uiSetting.TotalSacnCounts = (uint)e.BufferData[4];
                            DataCenter._uiSetting.ProbingCount1 = (uint)e.BufferData[12];
                            DataCenter._uiSetting.ProbingCount2 = (uint)e.BufferData[13];
                            DataCenter._uiSetting.ProbingCount3 = (uint)e.BufferData[14];
                            DataCenter._uiSetting.ProbingCount4 = (uint)e.BufferData[15];
                            DataCenter._uiSetting.XPitch1 = e.BufferData[22];
                            DataCenter._uiSetting.YPitch1 = e.BufferData[23];
                            DataCenter._uiSetting.XPitch2 = e.BufferData[24];
                            DataCenter._uiSetting.YPitch2 = e.BufferData[25];
                            DataCenter._uiSetting.XPitch3 = e.BufferData[26];
                            DataCenter._uiSetting.YPitch3 = e.BufferData[27];
                            DataCenter._uiSetting.XPitch4 = e.BufferData[28];
                            DataCenter._uiSetting.YPitch4 = e.BufferData[29];
                            DataCenter._uiSetting.XPitch5 = e.BufferData[30];
                            DataCenter._uiSetting.YPitch5 = e.BufferData[31];
                            DataCenter._uiSetting.EndTemp = e.BufferData[20];

                            DataCenter._uiSetting.SamplingDiePitchCol = (int)e.BufferData[32];
                            DataCenter._uiSetting.SamplingDiePitchRow = (int)e.BufferData[33];

                            if (DataCenter._uiSetting.RILoadReportMode != ERILoadReportMode.None)
                            {
                                DataCenter._product.XLineSubBinSampleCH = (int)e.BufferData[34];
                                DataCenter._product.YLineSubBinSampleCH = (int)e.BufferData[35];
                            }

                            //////////////////////////////////////////////////////////////////////
                            // Check IF Use Group Map
                            //////////////////////////////////////////////////////////////////////
                            if (e.BufferData[44] >= 1)
                            {
                                DataCenter._uiSetting.IsEnableLaodGroupData = true;
                            }
                            else
                            {
                                DataCenter._uiSetting.IsEnableLaodGroupData = false;
                            }
                            if (e.StrData[0] != null && e.StrData[0] != "")
                            {
                                DataCenter._uiSetting.ProberRecipeName = e.StrData[0];
                                Console.WriteLine("[ServerQueryEventHandler], ProbRecipeName: " + e.StrData[0]);
                            }
                            Console.WriteLine("[ServerQueryEventHandler], e.StrData[0]: " + e.StrData[0]);
                            //////////////////////////////////////////////////////////////////////

                            DataCenter._sysSetting.TotalProbingCount = (uint)e.BufferData[7];

                            int ES01_START_COUNT = 36;

                            for (int i = 0; i < 8; i++)
                            {
                                DataCenter._uiSetting.StartCountOfEdgeSensor[i] = (int)e.BufferData[i + ES01_START_COUNT];
                            }

                            GlobalFlag.IsEnableShowMap = true;

                            if (mainAxis == (int)EProbderMoveDirection.Xaxis) // X
                            {
                                if (yInitialDirection == 0) // Y move up 
                                {
                                    DataCenter._uiSetting.WaferMapGrowthDirection = (int)EMapGrowthDirection.Upward;
                                }
                                else           //Y move dowun
                                {
                                    DataCenter._uiSetting.WaferMapGrowthDirection = (int)EMapGrowthDirection.Downward;
                                }
                            }
                            else
                            {
                                if (xInitialDirection == 0) //X move right
                                {
                                    DataCenter._uiSetting.WaferMapGrowthDirection = (int)EMapGrowthDirection.Rightward;
                                }
                                else
                                {
                                    DataCenter._uiSetting.WaferMapGrowthDirection = (int)EMapGrowthDirection.Leftward;
                                }
                            }

                            //---------------------------------------------------------------------------------------------------

                            _oriboundaryDic = new Dictionary<string, int>();
                            int refCol = (int)e.BufferData[18];
                            int refRow = (int)e.BufferData[19];

                            int colMin = (int)e.BufferData[0];
                            int rowMin = (int)e.BufferData[1];
                            int colMax = (int)e.BufferData[2];
                            int rowMax = (int)e.BufferData[3];

                            _oriboundaryDic.Add("tProberChannel", tProberChannel);
                            _oriboundaryDic.Add("refCol", refCol);
                            _oriboundaryDic.Add("refRow", refRow);
                            _oriboundaryDic.Add("colMin", colMin);
                            _oriboundaryDic.Add("rowMin", rowMin);
                            _oriboundaryDic.Add("colMax", colMax);
                            _oriboundaryDic.Add("rowMax", rowMax);


                            //if (_p2TcoordTransTool != null && _p2TcoordTransTool.IsCleared)
                            
                            DataCenter.ChangeMapRowCol(colMin, rowMin, colMax, rowMax, refCol, refRow, tProberChannel);


                            if (_p2TcoordTransTool != null )
                            {
                                if (_p2TcoordTransTool.IsDefaultTrans)
                                {
                                    _p2TcoordTransTool = SetCoordinatTool( colMin,  rowMin,  colMax,  rowMax);
                                }
                                if (_p2TcoordTransTool.Matrix != null)
                                    ChangeMapRowColByMatrix(_p2TcoordTransTool);
                            }


                            GlobalFlag.IsSuccessCheckChannelConfig = false;

                            DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.AutoRun;

                            Host._MPIStorage.GenerateOutputFileName();

                            AppSystem.SetDataToSystem();

                            AppSystem.CheckMachineHW();

                            if (GlobalFlag.IsSuccessCheckFilterWheel || !DataCenter._machineConfig.Enable.IsCheckFilterWheel)
                            {
                                Fire_SwitchUIEvent((int)EBaseFormDisplayUI.RunningForm);

                                switch (DataCenter._machineConfig.TesterFunctionType)
                                {
                                    case ETesterFunctionType.Multi_Die:
                                        {

                                            if (DataCenter._machineConfig.TesterCommMode == ETesterCommMode.TCPIP)
                                            {
                                                xProberChannel = (uint)DataCenter._machineConfig.ChannelConfig.ColXCount;
                                                yProberChannel = (uint)DataCenter._machineConfig.ChannelConfig.RowYCount;
                                            }
                                            if (AppSystem.CheckChannelConfig(xProberChannel, yProberChannel, tProberChannel))
                                            {
                                                AppSystem.Run();
                                            }
                                            else
                                            {
                                                DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                                            }

                                            break;
                                        }
                                    default:
                                        {
                                            AppSystem.Run();

                                            break;
                                        }
                                }
                            }
                            else
                            {
                                DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                            }

                            ProduceInfo.SaveProduceInfo(DataCenter._uiSetting, EServerQueryCmd.CMD_TESTER_START);

                            Host.UpdateDataToAllUIForm();
                           
                        }
                        //else
                        //{
                        //    Console.WriteLine("[ServerQueryEventHandler],CMD_TESTER_START, Overload Test Mode");
                        //}

                        break;
                    }
                    #endregion
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_CALC:
                    #region
                    {

                        bool isUpdateDataToUI = false;

                        if (DataCenter._uiSetting.UIOperateMode != (int)EUIOperateMode.AutoRun)
                        {
                            isUpdateDataToUI = true;
                        }

                        DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.AutoRun;

                        if (isUpdateDataToUI)
                        {
                            Host.UpdateDataToAllUIForm();
                        }
                    }
                    #endregion
                    break;
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_TESTER_END:
                    #region
                    {

                        Console.WriteLine("[ServerQueryEventHandler], CMD_TESTER_END");

                        //if (GlobalFlag.TestMode == ETesterTestMode.Normal)
                        {
                            if (DataCenter._uiSetting.IsSendBarcodeToProbe)
                            {
                                Console.WriteLine("[ServerQueryEventHandler], IsSendBarcodeToProbe = true, break");

                                break;
                            }

                            GlobalFlag.IsSuccessCheckChannelConfig = false;  // Reset the flag, 否則在點測結束後, singleRetest 會保持Prober給的 channel has die 作動

                            if (e.BufferData != null)
                            {
                                DataCenter._uiSetting.EndTemp = e.BufferData[0];

                                for (int i = 0; i < 8; i++)
                                {
                                    DataCenter._uiSetting.EndCountOfEdgeSensor[i] = (int)e.BufferData[i];
                                }
                            }

                            //Write Report By xml
                            if (!ReportProcess.IsImplement && Host._MPIStorage != null)
                            {
                                if (!Host._MPIStorage.CheckTestResultFolderIsReady())
                                {
                                    Host.SetErrorCode(EErrorCode.ResultFolderIsNotReady);
                                }

                                if (_bodyDataList != null && _bodyDataList.Count != 0 && _bodyDataCount != 0)
                                {
                                    Host._MPIStorage.SaveTestResultToFile(true);
                                }
                            }

                            Host._MPIStorage.SaveSweepRawData();

                            if (GlobalFlag.IsEnableEndTest == true)
                            {

                                _MPITesterKernel.Stop();

                                ResetDataList();

                                DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

                                if (DataCenter._uiSetting.UIDisplayType == (int)EUIDisplayType.WMStartUI)
                                {
                                    WMOperate.WM_EndTest();
                                }

                                DataCenter._uiSetting.IsShowDailyCheckUI = true;

                                _preSamplingCheck.End();

                                DataCenter._uiSetting.IsManualSettingContiousProbingRowCol = false;

                                ProduceInfo.SaveProduceInfo(DataCenter._uiSetting, EServerQueryCmd.CMD_TESTER_END);

                                _customizeCoordTransTool.Clear();

                                SaveBinMapImg();

                                Host.UpdateDataToAllUIForm();
                                
                            }
                        }
                        //else
                        //{
                        //    Console.WriteLine("[ServerQueryEventHandler],CMD_TESTER_END, Overload Test Mode");
                        //}
                    }
                    #endregion

                    break;
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_TESTER_ABORT:
                    #region
                    {

                        Console.WriteLine("[ServerQueryEventHandler], CMD_TESTER_ABORT");

                        if (e.BufferData != null)
                        {
                            Console.WriteLine("[ServerQueryEventHandler], save abort file e.BufferData[0] == 1");
                            if ((int)e.BufferData[0] == 1)
                            {
                                DataCenter._uiSetting.IsAbortSaveFile = true;

                                //Write Report By xml
                                if (!ReportProcess.IsImplement && Host._MPIStorage != null)
                                {
                                    if (!Host._MPIStorage.CheckTestResultFolderIsReady())
                                    {
                                        Host.SetErrorCode(EErrorCode.ResultFolderIsNotReady);
                                    }

                                    if (_bodyDataList != null && _bodyDataList.Count != 0 && _bodyDataCount != 0)
                                    {
                                        Host._MPIStorage.SaveTestResultToFile(false);
                                    }
                                }
                            }
                            else if ((int)e.BufferData[0] == 0)
                            {
                                Console.WriteLine("[ServerQueryEventHandler], not save abort file e.BufferData[0] == 0");
                                DataCenter._uiSetting.IsAbortSaveFile = false;
                            }
                            Console.WriteLine("[ServerQueryEventHandler], save abort file  強迫輸出");
                            DataCenter._uiSetting.IsAbortSaveFile = true;//先強迫輸出
                        }

                        _MPITesterKernel.Stop();

                        ProduceInfo.SaveProduceInfo(DataCenter._uiSetting, EServerQueryCmd.CMD_TESTER_ABORT);

                        DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

                        _preSamplingCheck.End();

                        _customizeCoordTransTool.Clear();
                        
                        DataCenter._uiSetting.IsManualSettingContiousProbingRowCol = false;

                        Host.UpdateDataToAllUIForm();
                    }
                    #endregion

                    break;
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_AUTOCAL_END:

                    Console.WriteLine("[ServerQueryEventHandler], CMD_AUTOCAL_END");

                    break;
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_UPDATE_UI_FORM:
                    {
                        #region
                        DataCenter._uiSetting.TopDisplayUIForm = (int)e.BufferData[0];

                        if (DataCenter._uiSetting.TopDisplayUIForm == (int)EBaseFormDisplayUI.ConditionForm)
                        {
                            if (DataCenter.CheckTaskSheetIsExist(e.StrData[0]))
                            {
                                if (DataCenter.LoadTaskSheet(e.StrData[0]))
                                {
                                    GlobalFlag.IsSuccessLoadProduct = true;
                                    GlobalFlag.IsSuccessLoadBin = true;
                                    Host.UpdateDataToAllUIForm();
                                }
                                else
                                {
                                    GlobalFlag.IsSuccessLoadBin = false;
                                    GlobalFlag.IsSuccessLoadProduct = false;
                                }
                            }
                            else
                            {
                                DataCenter.NewTaskSheet(e.StrData[0]);
                                GlobalFlag.IsSuccessLoadProduct = true;
                                GlobalFlag.IsSuccessLoadBin = true;
                                Host.UpdateDataToAllUIForm();
                            }
                        }
                        else
                        {
                            //Host.UpdateBaseFormTopDisplay();
                        }
                        //Host.UpdateBaseFormTopDisplay();
					//Fire_SwitchUIEvent((int)e.BufferData[0]);
                    #endregion
                    }
                    break;
                //----------------------------------------------------------------------------------------------------------------------                                 
                case (int)EServerQueryCmd.CMD_PRINT_BARCODE:
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_PRINT_BARCODE");
                        Console.WriteLine("TubeNum:" + e.StrData[0]);
                        Console.WriteLine("TubeBinNum:" + e.StrData[1]);
                        Console.WriteLine("TubeCount:" + e.StrData[2]);
                        Console.WriteLine("TubeID:" + e.StrData[3]);
                        Console.WriteLine("PartID:" + e.StrData[4]);
                        Console.WriteLine("TubePullNumber:" + e.StrData[5]);

                        GlobalFlag.IsFinishPrintBarcode = Host._MPIStorage.PrintLabel(e.StrData);
                        #endregion
                    break;
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_LOAD_RECIPE_FROM_PROBER:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_LOAD_RECIPE_FROM_PROBER");
                        Console.WriteLine("OperatorName:" + e.StrData[0]);
                        Console.WriteLine("Barcode:" + e.StrData[1]);
                        Console.WriteLine("Lot number" + e.StrData[2]);

                        DataCenter._uiSetting.OperatorName = e.StrData[0];
                        DataCenter._uiSetting.Barcode = e.StrData[1];
                        DataCenter._uiSetting.WaferNumber = e.StrData[1];
                        DataCenter._uiSetting.LotNumber = e.StrData[2];

                        // EErrorCode errorCode1 = MESProcess.LoadRecipe(DataCenter._uiSetting);
                        EErrorCode errorCode1 = MESCtrl.LoadRecipe();

                        if (errorCode1 == EErrorCode.NONE)
                        {
                            //DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.AutoRun;
                            GlobalFlag.IsSuccessLoadProduct = true;
                            GlobalFlag.IsSuccessLoadBin = true;
                        }
                        else
                        {
                            GlobalFlag.IsSuccessLoadBin = false;
                            GlobalFlag.IsSuccessLoadProduct = false;
                        }
                        DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

                        Host.UpdateDataToAllUIForm();
                    #endregion
					}
                    break;
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_CHECK_RCONTACT_STATE:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_CHECK_RCONTACT_STATE");

                        // 切換Tester RContact 或是 Sampling Recipe
                        // 記下切換前的Recipe，TS_CMD_CHECK_RCONTACT_STATE_END時，再呼叫出來。
                        if (e.StrData[2] == ((int)TestServer.EMPITestServerCmd.TS_CMD_CHECK_RCONTACT_STATE_START).ToString()) // TS_CMD_CHECK_RCONTACT_STATE
                        {
                            _lastProdcutRecipe = DataCenter._uiSetting.TaskSheetFileName;

                            if (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
                            {
                                if (!AppSystem.CheckChannelConfig((uint)e.BufferData[0], (uint)e.BufferData[1], 0))
                                {
                                    DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                                }
                            }
                        }
                        else if (e.StrData[2] == ((int)TestServer.EMPITestServerCmd.TS_CMD_CHECK_RCONTACT_STATE_END).ToString())
                        {
                            e.StrData[0] = _lastProdcutRecipe;

                            DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                        }


                        if (DataCenter.CheckTaskSheetIsExist(e.StrData[0]))
                        {
                            if (DataCenter.LoadTaskSheet(e.StrData[0]))
                            {
                                AppSystem.SetDataToSystem();
                                AppSystem.CheckMachineHW();

                                Host.UpdateDataToAllUIForm();
                                GlobalFlag.IsSuccessLoadProduct = true;
                                GlobalFlag.IsSuccessLoadBin = true;
                            }
                            else
                            {
                                GlobalFlag.IsSuccessLoadBin = false;
                                GlobalFlag.IsSuccessLoadProduct = false;
                            }
                        }
                        else
                        {
                            GlobalFlag.IsSuccessLoadBin = false;
                            GlobalFlag.IsSuccessLoadProduct = false;
                        }

                        break;
                        #endregion
                    }
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_SEND_BARCODE_TO_PROBER:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_SEND_BARCODE_TO_PROBER");

                        EErrorCode errorCode1 = MESCtrl.LoadRecipe();

                        GlobalFlag.IsEnableSendBarcodeToProbe = DataCenter._uiSetting.IsSendBarcodeToProbe;

                        if (errorCode1 == EErrorCode.NONE)
                        {
                            GlobalFlag.IsSuccessLoadProduct = true;

                            GlobalFlag.IsSuccessLoadBin = true;

                            if (GlobalFlag.IsEnableSendBarcodeToProbe)
                            {
                                GlobalData.ToProbeBarcode = DataCenter._uiSetting.Barcode;

                                GlobalData.ToProbeLotNumber = DataCenter._uiSetting.LotNumber;

                                GlobalData.ToProbeOperator = DataCenter._uiSetting.OperatorName;

                                GlobalData.ToProbeWaferNumber = DataCenter._uiSetting.WaferNumber;
                            }
                        }
                        else
                        {
                            GlobalFlag.IsSuccessLoadBin = false;

                            GlobalFlag.IsSuccessLoadProduct = false;
                        }

                        DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

                        Host.UpdateDataToAllUIForm();

                        break;
                        #endregion
                    }
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_AUTO_CH_CALIB_START:
                    {
                        #region
                        uint xProberChannel = (uint)e.BufferData[0];   // Roy, 20140331, prober colX channel count
                        uint yProberChannel = (uint)e.BufferData[1];   //                prober rowY channel count

                        GlobalFlag.IsSuccessCheckChannelConfig = false;

                        DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.AutoRun;

                        AppSystem.SetDataToSystem();

                        AppSystem.CheckMachineHW();

                        if (GlobalFlag.IsSuccessCheckFilterWheel || !DataCenter._machineConfig.Enable.IsCheckFilterWheel)
                        {
                            //Fire_SwitchUIEvent((int)EBaseFormDisplayUI.RunningForm);

                            switch (DataCenter._machineConfig.TesterFunctionType)
                            {
                                case ETesterFunctionType.Multi_Die:
                                    {
                                        if (AppSystem.CheckChannelConfig(xProberChannel, yProberChannel, 0))
                                        {
                                            // AppSystem.Run();
                                        }
                                        else
                                        {
                                            DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        AppSystem.Run();
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;
                        }

                        _autoCalibChannelGain.Start(xProberChannel * yProberChannel, DataCenter._product);

                        GlobalFlag.IsEnableShowMap = false;

                        GlobalFlag.IsStopPushData = true;

                        Console.WriteLine("[ServerQueryEventHandler], CMD_AUTOCAL_END");

                        break;
                        #endregion
                    }
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_AUTO_CH_CALIB_END:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_AUTO_CH_CALIB_END");

                        _autoCalibChannelGain.End();

                        AppSystem.Fire_PopUIEvent((int)EPopUpUIForm.AutoChannelCalibrationForm);

                        DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

                        GlobalFlag.IsEnableShowMap = true;

                        GlobalFlag.IsStopPushData = false;

                        Host.UpdateDataToAllUIForm();

                        break;
                        #endregion
                    }
				//----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_QUERY_PROCESS_INFO:
                    {
                        #region

                        Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_PROCESS_INFO");

                        if (e.StrData != null && e.StrData.Length == 11)//只有TCP/IP時才會進來
                        {
                            Console.WriteLine("Barcode:" + e.StrData[0]);
                            Console.WriteLine("LotNumber:" + e.StrData[1]);
                            Console.WriteLine("CassetteID:" + e.StrData[2]);
                            Console.WriteLine("WaferNumber:" + e.StrData[3]);
                            Console.WriteLine("OPID:" + e.StrData[4]);
                            Console.WriteLine("ProductType:" + e.StrData[6]);
                            Console.WriteLine("Substrate:" + e.StrData[7]);
                            Console.WriteLine("CassetteNumber:" + e.StrData[8]);
                            Console.WriteLine("SoltNumber:" + e.StrData[9]);
                            Console.WriteLine("CustomerKeyNumber:" + e.StrData[10]);
                            Console.WriteLine("Station:" + e.StrData[11]);

                            //////////////////////////////////////////////////////////////////////
                            // Set  Data
                            //////////////////////////////////////////////////////////////////////
                            if (e.StrData[0].Equals("") || !e.StrData[0][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.Barcode = e.StrData[0];
                                DataCenter._uiSetting.WeiminUIData.KeyInFileName = e.StrData[0];
                            }

                            if (e.StrData[1].Equals("") || !e.StrData[1][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.LotNumber = e.StrData[1];
                                DataCenter._uiSetting.WeiminUIData.LotNumber = e.StrData[1];
                            }

                            if (e.StrData[2].Equals("") || !e.StrData[2][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.CassetteNumber = e.StrData[2];
                            }

                            if (e.StrData[3].Equals("") || !e.StrData[3][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.WaferNumber = e.StrData[3];
                            }

                            if (e.StrData[4].Equals("") || !e.StrData[4][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.OperatorName = e.StrData[4];
                            }

                            if (e.StrData[6].Equals("") || !e.StrData[6][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.ProductType = e.StrData[6];
                            }

                            if (e.StrData[7].Equals("") || !e.StrData[7][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.Substrate = e.StrData[7];
                            }

                            if (e.StrData[8].Equals("") || !e.StrData[8][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.CassetteNumber = e.StrData[8];
                            }

                            if (e.StrData[9].Equals("") || !e.StrData[9][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.SlotNumber = e.StrData[9];
                            }

                            if (e.StrData[10].Equals("") || !e.StrData[10][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.KeyNumber = e.StrData[10];
                            }


                        }

                        double temperature = e.BufferData[0];

                        GlobalData.ProberTemperature = temperature;
                        Console.WriteLine("Temperature:" + temperature.ToString());
                        break;
                        #endregion
                    }
                //----------------------------------------------------------------------------------------------------------------------

                case (int)EServerQueryCmd.CMD_QUERY_PRE_OVERLOAD_TEST_INFO:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_PRE_OVERLOAD_TEST_INFO");


                        GlobalFlag.TestMode = (ETesterTestMode)e.BufferData[0];

                        Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_PRE_OVERLOAD_TEST_INFO,Normal");

                        //if (GlobalFlag.TestMode == ETesterTestMode.Normal)
                        //{
                        //    _MPITesterKernel.CmdData.DoubleData[(uint)EProberDataIndex.TestChipGroup] = 1;
                        //    Console.WriteLine( "[ServerQueryEventHandler], CMD_QUERY_PRE_OVERLOAD_TEST_INFO,Normal" );
                        //}
                        //else
                        //{
                        //    _MPITesterKernel.CmdData.DoubleData[(uint)EProberDataIndex.TestChipGroup] = 2;
                        //    Console.WriteLine( "[ServerQueryEventHandler], CMD_QUERY_PRE_OVERLOAD_TEST_INFO,OverloadTest" );
                        //}

                        if (e.StrData != null && e.StrData.Length == 20)
                        {
                            
                            _customizeCoordTransTool.Clear();
                            _p2TcoordTransTool.Clear();
                            for (int i = 0; i < 10; ++i)
                            {
                                SetCoordTool(e, i, ref _p2TcoordTransTool);
                            }

                            for (int i = 10; i < 20; ++i)
                            {
								SetCoordTool( e, i, ref _customizeCoordTransTool );
                            }

                            _p2TcoordTransTool.CalcConvertMatrix(true);

                            if (_p2TcoordTransTool.Matrix != null)
                            {
                                _MPITesterKernel.P2TcoordTransTool = _p2TcoordTransTool.Clone() as CoordTransferTool;

                                Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_PRE_OVERLOAD_TEST_INFO,_p2TcoordTransTool" + _p2TcoordTransTool.Matrix.ToString(format: "0.00"));
                            }

                            _customizeCoordTransTool.CalcConvertMatrix(true);

                            if (_customizeCoordTransTool.Matrix != null)
                            {
                                Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_PRE_OVERLOAD_TEST_INFO,_coordTransTool" + _customizeCoordTransTool.Matrix.ToString(format: "0.00"));
                            }

                        }

                        #endregion
                        break;
                    }
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_QUERY_PRE_CHUCK_TEMP_INFO:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_PRE_CHUCK_TEMP_INFO");

                        _MPITesterKernel.Data.ChipInfo.ChuckTemp = e.BufferData[0];
                        //_autoCalibChannelGain.End();

                        //AppSystem.Fire_PopUIEvent((int)EPopUpUIForm.AutoChannelCalibrationForm);

                        //DataCenter._uiSetting.UIOperateMode = (int)EUIOperateMode.Idle;

                        //GlobalFlag.IsEnableShowMap = true;

                        //GlobalFlag.IsStopPushData = false;

                        //Host.UpdateDataToAllUIForm();
                        #endregion
                        break;
                    }
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_LASER_BAR_INFO:
                    #region
                    {
                        Console.WriteLine("[ServerQueryEventHandler], CMD_LASER_BAR_INFO");

                        double temperature = e.BufferData[0];

                        GlobalData.ProberTemperature = temperature;

                        break;
                    }
                    #endregion
                //----------------------------------------------------------------------------------------------------------------------

                case (int)EServerQueryCmd.CMD_SUB_RECIPE_INFO:
                    {
                        Console.WriteLine("[ServerQueryEventHandler], CMD_SUB_RECIPE_INFO");

                        Console.WriteLine("ProberRecipeName:" + DataCenter._uiSetting.ProberRecipeName);
                        Console.WriteLine("ProberSubRecipe:" + DataCenter._uiSetting.ProberSubRecipe);
                        Console.WriteLine("SlotNumber:" + DataCenter._uiSetting.SlotNumber + ",Ori:" + e.BufferData[0]);

                        DataCenter._uiSetting.ProberRecipeName = e.StrData[0];
                        DataCenter._uiSetting.ProberSubRecipe = e.StrData[1];
                        DataCenter._uiSetting.SlotNumber = e.BufferData[0].ToString("0");

                        break;
                    }
                //----------------------------------------------------------------------------------------------------------------------

                case (int)EServerQueryCmd.CMD_WAFER_BEGIN:
                    {
                        Console.WriteLine("[ServerQueryEventHandler], CMD_WAFER_BEGIN");
                        DataCenter._uiSetting.WaferBeginTime = DateTime.Now;
                        if (e.StrData != null && e.StrData.Length > 0)
                        {DataCenter._uiSetting.SubPiece = e.StrData[0];}
                    }
                    break;
                //----------------------------------------------------------------------------------------------------------------------

                case (int)EServerQueryCmd.CMD_WAFER_FINISH:
                    {
                        Console.WriteLine("[ServerQueryEventHandler], CMD_WAFER_FINISH");
                        DataCenter._uiSetting.SubPiece = "Q00";//預設為聯穎全片的格式
                        DataCenter._uiSetting.IsAppendForWaferBegine = false;
                    }
                    break;

                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_QUERY_PRE_WAFER_IN_INFO:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_PRE_WAFER_IN_INFO");

                        if (e.StrData != null && e.StrData.Length == 2)//只有TCP/IP時才會進來
                        {
                            //strData[0] = preWaferIn.ProductType;
                            //strData[1] = preWaferIn.ProberRecipe;
                            //strData[2] = preWaferIn.Temperature;

                            //buffer[0] = preWaferIn.TestStageCount;
                            Console.WriteLine("ProductType:" + e.StrData[0]);
                            Console.WriteLine("ProberRecipe:" + e.StrData[1]);

                            Console.WriteLine("TestStageCount:" + e.BufferData[0]);
                            Console.WriteLine("Temperature:" + e.BufferData[1]);

                            //////////////////////////////////////////////////////////////////////
                            // Set  Data
                            //////////////////////////////////////////////////////////////////////
                            if (e.StrData[0].Equals("") || !e.StrData[0][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.ProductType = e.StrData[0];
                            }

                            if (e.StrData[1].Equals("") || !e.StrData[1][0].Equals((char)0x01))
                            {
                                DataCenter._uiSetting.ProberRecipeName = e.StrData[1];
                            }

                            GlobalData.ProberTemperature = e.BufferData[1];
                        }

                        break;
                        //----------------------------------------------------------------------------------------------------------------------

                        #endregion
                    }
                //----------------------------------------------------------------------------------------------------------------------
                case (int)EServerQueryCmd.CMD_QUERY_CHECK_LASER_POWER_INFO:
                    {
                        #region
                        Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_CHECK_LASER_POWER_INFO");

                        bool cmdIsCompensate = e.BufferData[0] == 1;
                        bool isCompensate = cmdIsCompensate;
                        bool isPass = true;// MonitorLaser(isCompensate);
                        if (e.StrData != null && e.StrData.Length > 0 &&
                            e.StrData[0] != null && e.StrData[0].Length > 0)
                        {
                            #region preheat

                            Console.WriteLine("[ServerQueryEventHandler], CMD_QUERY_CHECK_LASER_POWER_INFO,Msg:" + e.StrData[0]);
                            string msg = e.StrData[0];
                            string[] sArr = msg.Split(',');
                            int runTimeSec = 0;
                            int delaySec = 0;
                            for (int i = 0; i < sArr.Length; ++i)
                            {
                                string[] sArr1 = sArr[i].Split(':');
                                switch (sArr1[0].Trim().ToUpper())
                                {
                                    case "RUNTIME":
                                        {
                                            int.TryParse(sArr1[1].Trim(), out runTimeSec);
                                        }
                                        break;
                                    case "DELAY":
                                        {
                                            int.TryParse(sArr1[1].Trim(), out delaySec);
                                        }
                                        break;
                                }
                            }
                            DateTime st = DateTime.Now;
                            //_laserCheckCnt
                           
                            while ((DateTime.Now - st).TotalSeconds < runTimeSec - delaySec * 2 && isPass)
                            {
                                isPass = MonitorLaser(cmdIsCompensate,0);

                                if (isPass)//沒有成功就直接跳err並通知prober
                                {
                                    System.Threading.Thread.Sleep(delaySec * 1000);
                                }
                            }

                            if (isPass)//沒有成功就直接跳err並通知prober
                            {
                                isPass = MonitorLaser(isCompensate && isPass, _MPITesterKernel.Data.ChipInfo.TestCount + 1);
                            }
                            #endregion
                        }
                        else
                        {
                            if (cmdIsCompensate)
                                _laserCheckCnt++;
                            if (cmdIsCompensate && DataCenter._machineConfig.LaserSrcSysConfig.AutoAttPerCntCheck > 0 &&
                                (_laserCheckCnt % DataCenter._machineConfig.LaserSrcSysConfig.AutoAttPerCntCheck) == 0)
                            {
                                isCompensate = true;
                            }
                            else
                            {
                                isCompensate = false;
                            }
                            //AutoAttPerCntCheck
                            isPass = MonitorLaser(isCompensate && isPass, _MPITesterKernel.Data.ChipInfo.TestCount + 1);
                        }

                        
                        _MPITesterKernel.SetOpticalSwitchToDefault();
                        
                        

                        e.BufferData[0] = isPass ? 100 : 200;
                        break;
                        //----------------------------------------------------------------------------------------------------------------------

                        #endregion
                    }
                //
                default:
                    break;
            }



            if ((EServerQueryCmd)e.CmdID == EServerQueryCmd.CMD_TESTER_ABORT)//ABORT時強迫切回Normal
            {
                GlobalFlag.TestMode = ETesterTestMode.Normal;
            }

            //if (GlobalFlag.TestMode == ETesterTestMode.Normal)
            {
                if (e.CmdID == (int)EServerQueryCmd.CMD_TESTER_END && GlobalFlag.IsEnableEndTest == false)
                {
                    
                }
                else
                {
                    if (!DataCenter._uiSetting.IsManualRunMode && (EServerQueryCmd)e.CmdID == EServerQueryCmd.CMD_TESTER_END)//光磊
                    { DataCenter._uiSetting.FileInProcessList.Add(DataCenter._uiSetting.TestResultFileName + "."+DataCenter._uiSetting.TestResultFileExt); }
                    EErrorCode err = ReportProcess.RunCommand((EServerQueryCmd)e.CmdID);

                    if (err != EErrorCode.NONE)
                    {
                        Host.SetErrorCode(err);
                    }

                    if ((EServerQueryCmd)e.CmdID == EServerQueryCmd.CMD_TESTER_END ||
                        (EServerQueryCmd)e.CmdID == EServerQueryCmd.CMD_TESTER_ABORT)
                    {
                        
                        DataCenter._uiSetting.IsRetest = false;
                        DataCenter._uiSetting.IsAppend = false;
                        _p2TcoordTransTool.Clear();
                        _MPITesterKernel.P2TcoordTransTool = null;
                    }
                }
            }

            if ((EServerQueryCmd)e.CmdID == EServerQueryCmd.CMD_TESTER_START )//CMD_TESTER_START 後 CoordTransTool才存在
            {
                if (_customizeCoordTransTool != null)
                {
                    ReportProcess.CustomizeCoordTransTool = _customizeCoordTransTool.Clone() as CoordTransferTool;
                }

                if( _p2TcoordTransTool != null)
                {
                    ReportProcess.CoordTransferTool = _p2TcoordTransTool.Clone() as CoordTransferTool;
                }
            }

            if ((EServerQueryCmd)e.CmdID == EServerQueryCmd.CMD_WAFER_FINISH)
            {
                DataCenter._uiSetting.FileInProcessList.Clear();
            }

            
        }

        private static bool MonitorLaser(bool isCompensate,int testIndex = -1)
        {
            string attStr = _MPITesterKernel.GetAttMoniterInfo();
            bool isPass = true;
            string errMsg = "";
            string powStr = _MPITesterKernel.GetPowerMeterInfo(ref isPass, out errMsg);

            string compStr = GetNoCompStr(powStr);
            if (isCompensate)
            {
                string tCompStr = "";
                bool retule = _MPITesterKernel.RunAutoLaserCompensate(out tCompStr);

                if (tCompStr != "")
                {
                    compStr = tCompStr;
                }

                if ( !retule)
                {
                    
                   // Host.SetErrorCode(EErrorCode.LASER_AutoSetAttenuator_Fail_Err, tCompStr);

                    Console.WriteLine("[AppSystem], MonitorLaser(),LASER_AutoSetAttenuator_Fail_Err", tCompStr);

                    return false;
                }
            }
            
            bool isLog = false;
            if (testIndex < 0)
            {
                testIndex = _MPITesterKernel.Data.ChipInfo.TestCount + 1;
            }
            string logStr = "Time:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "," + "Test:" + testIndex.ToString() + ",";
            if (attStr != "") { logStr += attStr; isLog = true; }
            if (powStr != "") { logStr += powStr; isLog = true; }
            if (isLog)
            {
                logStr += compStr;
                ReportProcess.PushRemarkLog(logStr);
            }

            if (!isPass)
            {
                _MPITesterKernel.GetPowerMeterInfo(ref isPass, out errMsg);
                if (!isPass)
                {
                    Host.SetErrorCode(EErrorCode.LASER_PowerMeter_CheckPower_Fail_Err, errMsg);

                    Console.WriteLine("[AppSystem], MonitorLaser(),LASER_PowerMeter_CheckPower_Fail_Err");
                }
            }

            _MPITesterKernel.SetOpticalSwitchToDefault();

            //isPass = true;//20200408_David 讓tolerance機制回歸Auto Tune
            return isPass;
        }

        private static string GetNoCompStr(string powStr)
        {
            string compStr = _MPITesterKernel.GetAttSetInfo() + powStr.Replace(":", "_After:");

            string timeStr = "";
            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item.IsEnable && item.Type == ETestType.LaserSource)
                {
                    timeStr = item.Name + "_Loop:-1,";// +"," + item.Name + "_Span(ms):0,";
                    compStr += timeStr;
                }
            }
            

            return compStr;
        }

        private static void SaveBinMapImg()
        {
            if (DataCenter._uiSetting.UIMapPathInfo.EnablePath)
            {
                string folderName = DataCenter._uiSetting.GetPathWithFolder(DataCenter._uiSetting.UIMapPathInfo);

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                string mapFileName = DataCenter._uiSetting.TestResultFileName;
                string file = Path.Combine(folderName, mapFileName) + ".jpg";
                //FormAgent.frmTestResultChart.btnHide_Click(null, null);
                //Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);

                Fire_SaveBinMapImgEvent(file);
                //FormAgent.TestResultForm.SaveMapImg(file,System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private static void ChangeMapRowColByMatrix(CoordTransferTool ct)
        {
            //int tProberChannel = 0;

            //if (_MPITesterKernel.P2TcoordTransTool.Matrix[0, 0] < 0)//2,3
            //{
            //    tProberChannel += 2;
            //}
            //if (_MPITesterKernel.P2TcoordTransTool.Matrix[1, 1] < 0)//3,4
            //{
            //    tProberChannel += 1;
            //}


			int refCol = _oriboundaryDic[ "refCol" ] = 0;//(int)ct.Matrix[0, 1];
			int refRow = _oriboundaryDic[ "refRow" ] = 0;// ( int ) ct.Matrix[ 0, 0 ];

            int colMin = _oriboundaryDic["colMin"];
            int rowMin = _oriboundaryDic["rowMin"];
            int colMax = _oriboundaryDic["colMax"];
            int rowMax = _oriboundaryDic["rowMax"];

			var mMin = ct.TransCoord( colMin, rowMin );
			var mMax = ct.TransCoord( colMax, rowMax );

			colMin = ( int ) Math.Min( mMin[ 0, 0 ], mMax[ 0, 0 ] );//mMin[ 0, 0 ];
			rowMin = ( int ) Math.Min( mMin[ 1, 0 ], mMax[ 1, 0 ] ); //mMin[ 1, 0 ];


			colMax = ( int ) Math.Max( mMin[ 0, 0 ], mMax[ 0, 0 ] );
			rowMax = ( int ) Math.Max( mMin[ 1, 0 ], mMax[ 1, 0 ] );

			//refCol = ( rowMax + rowMin ) / 2;

			//DataCenter.ChangeMapRowCol( colMin, rowMax, colMax, rowMin, refCol, refRow, 0 );

            DataCenter.ChangeMapRowCol(colMin, rowMin, colMax, rowMax, refCol, refRow, 0);// make up side down, so map can show as coord 1

            Host.UpdateDataToAllUIForm();
        }

        private static void SetCoordTool( ServerQueryEventArg e, int i,ref CoordTransferTool ctt)
        {
            int x, y, nx, ny;
            string id, remark;
            List<string> strList = e.StrData[i].Split(',').ToList();
            id = "";
            remark = "";

            if (strList.Count >= 5)
            { id = strList[4]; }
            if (strList.Count >= 6)
            { remark = strList[5]; }

            if (strList != null && int.TryParse(strList[0], out x) &&
                int.TryParse(strList[1], out y) &&
                int.TryParse(strList[2], out nx) &&
                int.TryParse(strList[3], out ny) && strList.Count == 6)
            {
                ctt.PushData(x, y, nx, ny, id, remark);
            }
        }

        private static void FinishTestAndCalcEventHandler(object o, System.EventArgs e)
        {
            _MPITesterKernel.Data.Overwrite(ref DataCenter._acquireData);

            // Device Verify
            if (GlobalFlag.IsDeviceVerifyMode)
            {
                float[] testData = new float[DataCenter._acquireData.OutputTestResult.Count];

                for (int i = 0; i < DataCenter._acquireData.OutputTestResult.Count; i++)
                {
                    TestResultData data = DataCenter._acquireData.OutputTestResult[i];

                    testData[i] = (float)data.Value;
                }

                _bodyDataList.Add(testData);

                GlobalFlag.IsDeviceVerifyMode = false;

                _MPITesterKernel.RunCommand((int)ETesterKernelCmd.ConfirmDataReceived);

                return;
            }

            int row = -1;

            int col = -1;

            int countIndex = -1;

            string rowColKey = string.Empty;

            Dictionary<string, float> result = null;

            Dictionary<string, double> result2 = null;

            Dictionary<string, bool> isTested = null;

            switch (DataCenter._machineConfig.TesterFunctionType)
            {
                case ETesterFunctionType.Single_Die:
                case ETesterFunctionType.Multi_Terminal:
                    {
                        #region >>> Single Die <<<

                        float[] testData = new float[DataCenter._acquireData.OutputTestResult.Count];

                        result = new Dictionary<string, float>(DataCenter._acquireData.OutputTestResult.Count);

                        result2 = new Dictionary<string, double>(DataCenter._acquireData.OutputTestResult.Count);

                        isTested = new Dictionary<string, bool>(DataCenter._acquireData.OutputTestResult.Count);

                        col = DataCenter._acquireData.ChipInfo.ColX;

                        row = DataCenter._acquireData.ChipInfo.RowY;

                        for (int i = 0; i < DataCenter._acquireData.OutputTestResult.Count; i++)
                        {
                            TestResultData data = DataCenter._acquireData.OutputTestResult[i];

                            result.Add(data.KeyName, (float)data.Value);

                            result2.Add(data.KeyName, data.Value);

                            isTested.Add(data.KeyName, data.IsTested);

                            testData[i] = (float)data.Value;

                            if (data.KeyName == "TEST")
                            {
                                countIndex = i;
                            }
                        }

                        //----------------------------------------------------------------------------------
                        // Update tempCond for UI Using
                        //----------------------------------------------------------------------------------
                        UpdateDataToUIDisplay(result, isTested);

                        if (GlobalFlag.IsReSingleTestMode == true)
                        {
                            GlobalFlag.IsReSingleTestMode = false;

                            _MPITesterKernel.RunCommand((int)ETesterKernelCmd.ConfirmDataReceived);

                            return;
                        }

                        //----------------------------------------------------------------------------------
                        // Pre Sampling Check 
                        //----------------------------------------------------------------------------------
                        GlobalFlag.IsPreSamplingCheckSuccess = _preSamplingCheck.Push(col, row, result);

                        //----------------------------------------------------------------------------------
                        // Add result to BodyDataList / ReportProcess
                        //----------------------------------------------------------------------------------
                        UpdateDataToBodyDataList(col, row, countIndex, testData);

                        _outputBigData.AddBigData(col, row, 0, DataCenter._acquireData);

                        Report.ReportProcess.Push(result2);

                        //----------------------------------------------------------------------------------
                        // Show Map
                        //----------------------------------------------------------------------------------
                        if (GlobalFlag.IsEnableShowMap == true)
                        {
                            Fire_ShowMapDataEvent(row, col, result);
                        }

                        #endregion

                        break;
                    }
                case ETesterFunctionType.Multi_Die:
                    {
                        #region >>> Multi Die <<<

                        for (uint channel = 0; channel < DataCenter._machineConfig.ChannelConfig.ChannelCount; channel++)
                        {
                            if (!DataCenter._acquireData.ChannelResultDataSet[channel].IsTested && DataCenter._userManag.CurrentUserName != "simulator")
                            {
                                continue;
                            }

                            float[] testData = new float[DataCenter._acquireData.OutputTestResult.Count];

                            result = new Dictionary<string, float>(DataCenter._acquireData.OutputTestResult.Count);

                            result2 = new Dictionary<string, double>(DataCenter._acquireData.OutputTestResult.Count);

                            isTested = new Dictionary<string, bool>(DataCenter._acquireData.OutputTestResult.Count);

                            col = DataCenter._acquireData.ChannelResultDataSet[channel].Col;

                            row = DataCenter._acquireData.ChannelResultDataSet[channel].Row;

                            for (int i = 0; i < DataCenter._acquireData.OutputTestResult.Count; i++)
                            {
                                TestResultData data = DataCenter._acquireData.ChannelResultDataSet[channel][i];

                                result.Add(data.KeyName, (float)data.Value);

                                result2.Add(data.KeyName, data.Value);

                                isTested.Add(data.KeyName, data.IsTested);

                                testData[i] = (float)data.Value;

                                if (data.KeyName == "TEST")
                                {
                                    countIndex = i;
                                }
                            }

                            //----------------------------------------------------------------------------------
                            // Update IsTester form channel data to product
                            //----------------------------------------------------------------------------------
                            if (DataCenter._product != null && DataCenter._product.TestCondition != null && DataCenter._product.TestCondition.TestItemArray != null)
                            {
                                foreach (var testItem in DataCenter._product.TestCondition.TestItemArray)
                                {
                                    if (testItem.MsrtResult == null)
                                    {
                                        continue;
                                    }

                                    foreach (var resultItem in testItem.MsrtResult)
                                    {
                                        if (isTested.ContainsKey(resultItem.KeyName))
                                        {
                                            resultItem.IsTested = isTested[resultItem.KeyName];
                                        }
                                    }
                                }
                            }

                            //----------------------------------------------------------------------------------
                            // Update tempCond for UI Using
                            //----------------------------------------------------------------------------------
                            UpdateDataToUIDisplay(result, isTested, true, channel);

                            if (GlobalFlag.IsReSingleTestMode == true)
                            {
                                continue;
                            }

                            //----------------------------------------------------------------------------------
                            // Pre Sampling Check 
                            //----------------------------------------------------------------------------------
                            GlobalFlag.IsPreSamplingCheckSuccess = _preSamplingCheck.Push(col, row, result);

                            //----------------------------------------------------------------------------------
                            // Auto Calibration Channel Gain 
                            //----------------------------------------------------------------------------------
                            _autoCalibChannelGain.Push(col, row, channel, result);

                            //----------------------------------------------------------------------------------
                            // Add result to BodyDataList / ReportProcess
                            //----------------------------------------------------------------------------------
                            _outputBigData.AddBigData(col, row, channel, DataCenter._acquireData);

                            UpdateDataToBodyDataList(col, row, countIndex, testData);

                            Report.ReportProcess.Push(result2);

                            //----------------------------------------------------------------------------------
                            // Show Map
                            //----------------------------------------------------------------------------------
                            if (GlobalFlag.IsEnableShowMap == true)
                            {
                                Fire_ShowMapDataEvent(row, col, result);
                            }
                        }

                        #endregion

                        break;
                    }
                case ETesterFunctionType.Multi_Pad:
                    {
                        #region >>> Multi Pad <<<

                        float[] testData = new float[DataCenter._acquireData.OutputTestResult.Count];

                        result = new Dictionary<string, float>(DataCenter._acquireData.OutputTestResult.Count);

                        result2 = new Dictionary<string, double>(DataCenter._acquireData.OutputTestResult.Count);

                        isTested = new Dictionary<string, bool>(DataCenter._acquireData.OutputTestResult.Count);

                        col = DataCenter._acquireData.ChipInfo.ColX;

                        row = DataCenter._acquireData.ChipInfo.RowY;

                        //-------------------------------------------------------------------------------------
                        // (1) 先由 OutputTestResult, 繞出 System Result Data
                        //-------------------------------------------------------------------------------------
                        for (int i = 0; i < DataCenter._acquireData.OutputTestResult.Count; i++)
                        {
                            TestResultData data = DataCenter._acquireData.OutputTestResult[i];

                            result.Add(data.KeyName, (float)data.Value);

                            result2.Add(data.KeyName, data.Value);

                            isTested.Add(data.KeyName, data.IsTested);

                            testData[i] = (float)data.Value;

                            if (data.KeyName == "TEST")
                            {
                                countIndex = i;
                            }
                        }


                        //----------------------------------------------------------------------------------
                        // Update tempCond for UI Using
                        //----------------------------------------------------------------------------------
                        UpdateDataToUIDisplay(result, isTested);

                        if (GlobalFlag.IsReSingleTestMode == true)
                        {
                            GlobalFlag.IsReSingleTestMode = false;

                            _MPITesterKernel.RunCommand((int)ETesterKernelCmd.ConfirmDataReceived);

                            return;
                        }

                        //----------------------------------------------------------------------------------
                        // Pre Sampling Check 
                        //----------------------------------------------------------------------------------
                        GlobalFlag.IsPreSamplingCheckSuccess = _preSamplingCheck.Push(col, row, result);

                        //----------------------------------------------------------------------------------
                        // Add result to BodyDataList / ReportProcess
                        //----------------------------------------------------------------------------------
                        for (uint channel = 0; channel < DataCenter._acquireData.ChannelResultDataSet.Count; channel++)
                        {
                            _outputBigData.AddBigData(col, row, channel, DataCenter._acquireData);
                        }

                        UpdateDataToBodyDataList(col, row, countIndex, testData);

                        Report.ReportProcess.Push(result2);

                        //----------------------------------------------------------------------------------
                        // Show Map
                        //----------------------------------------------------------------------------------
                        if (GlobalFlag.IsEnableShowMap == true)
                        {
                            Fire_ShowMapDataEvent(row, col, result);
                        }

                        #endregion

                        break;
                    }
            }

            GlobalFlag.IsReSingleTestMode = false;

            _MPITesterKernel.RunCommand((int)ETesterKernelCmd.ConfirmDataReceived);
        }

        private static void TCPIPStateChangeHandker(ETCPClientState state)
        {
            _tcpipClientState = state;
        }

        private static void UpdateDataToUIDisplay(Dictionary<string, float> result, Dictionary<string, bool> isTested,
            bool isUpdateToIndicateChannel = false, uint channel = 0)
        {
            //----------------------------------------------------------------------------------
            // Update tempCond for UI Using
            //----------------------------------------------------------------------------------
            foreach (TestItemData tid in DataCenter._tempCond.TestItemArray)
            {
                if (tid.MsrtResult == null)
                {
                    continue;
                }

                foreach (TestResultData data in tid.MsrtResult)
                {
                    if (result.ContainsKey(data.KeyName))
                    {
                        data.Value = result[data.KeyName];
                    }

                    if (isTested.ContainsKey(data.KeyName))
                    {
                        data.IsTested = isTested[data.KeyName];
                    }
                }
            }

            if (DataCenter._product.TestCondition.ChannelConditionTable.IsEnable)
            {
                if (isUpdateToIndicateChannel)
                {
                    foreach (TestItemData tid in DataCenter._tempCond.ChannelConditionTable.Channels[channel].Conditions)
                    {
                        if (tid.MsrtResult == null)
                        {
                            continue;
                        }

                        foreach (TestResultData data in tid.MsrtResult)
                        {
                            if (result.ContainsKey(data.KeyName))
                            {
                                data.Value = result[data.KeyName];
                            }
                        }
                    }
                }
                else
                {
                    for (int count = 0; count < DataCenter._tempCond.ChannelConditionTable.Count; count++)
                    {
                        foreach (TestItemData tid in DataCenter._tempCond.ChannelConditionTable.Channels[count].Conditions)
                        {
                            if (tid.MsrtResult == null)
                            {
                                continue;
                            }

                            foreach (TestResultData data in tid.MsrtResult)
                            {
                                if (result.ContainsKey(data.KeyName))
                                {
                                    data.Value = result[data.KeyName];
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void UpdateDataToBodyDataList(int col, int row, int countIndex, float[] testData)
        {
            if (ReportProcess.IsImplement)
            {
                return;
            }

            string rowColKey = "X" + col.ToString() + "Y" + row.ToString();

            //----------------------------------------------------------------------------------
            // Body Data List
            //----------------------------------------------------------------------------------
            if (DataCenter._uiSetting.UIOperateMode == (int)EUIOperateMode.ManulRun ||
                DataCenter._uiSetting.UIOperateMode == (int)EUIOperateMode.AutoRun)
            {
                if (GlobalFlag.IsReSingleTestMode == false)
                {
                    if (_bodyRowColIndex.Contains(rowColKey) && DataCenter._sysSetting.IsCheckRowCol)
                    {
                        int index = _bodyRowColIndex.IndexOf(rowColKey);

                        testData[countIndex] = index + 1;

                        _bodyDataList.RemoveAt(index);

                        _bodyDataList.Insert(index, testData);
                    }
                    else
                    {
                        _bodyDataCount++;

                        testData[countIndex] = _bodyDataCount;

                        _bodyDataList.Add(testData);

                        if (DataCenter._sysSetting.IsCheckRowCol)
                        {
                            _bodyRowColIndex.Add(rowColKey);
                        }
                    }
                }
            }
        }

        private static void LogTestResultSpec()
        {
            if (DataCenter._product.TestCondition == null)
            {
                return;
            }

            if (DataCenter._product.TestCondition.TestItemArray == null)
            {
                return;
            }

            if (DataCenter._product.TestCondition.TestItemArray.Length == 0)
            {
                return;
            }

            foreach (var testItem in DataCenter._product.TestCondition.TestItemArray)
            {
                Console.WriteLine("[AppSystem], LogTestResultSpec(), " + testItem.KeyName);

                Console.WriteLine("[AppSystem], LogTestResultSpec(), IsEnable," + testItem.IsEnable);

                if (testItem.MsrtResult == null)
                {
                    continue;
                }

                foreach (var result in testItem.MsrtResult)
                {
                    Console.WriteLine("[AppSystem], LogTestResultSpec(), " + result.KeyName);

                    Console.WriteLine("[AppSystem], LogTestResultSpec(), IsEnable," + result.IsEnable);

                    Console.WriteLine("[AppSystem], LogTestResultSpec(), IsVerify," + result.IsVerify);

                    Console.WriteLine("[AppSystem], LogTestResultSpec(), IsVision," + result.IsVision);

                    Console.WriteLine("[AppSystem], LogTestResultSpec(), " + result.MinLimitValue + "," + result.MaxLimitValue);
                }
            }
        }

        #endregion

        public static bool Initialize()
        {
            bool rtn = true;

            Console.WriteLine("[AppSystem], Initialize() ");

            if (DataCenter._machineConfig.Enable.IsSimulator == true)
            {
                DataCenter._machineConfig.TesterCommMode = ETesterCommMode.TCPIP;

                DataCenter._machineConfig.SourceMeterModel = ESourceMeterModel.LDT1A;
            }

            //---------------------------------------------------------------------------------------------------------------
            // (1) create system kernal 
            //---------------------------------------------------------------------------------------------------------------
            switch (DataCenter._machineConfig.TesterFunctionType)
            {
                case ETesterFunctionType.Single_Die:
                case ETesterFunctionType.Multi_Terminal:
                    _MPITesterKernel = new HS_TesterKernel();
                    break;
                //---------------------------------------------------------------------------
                case ETesterFunctionType.Multi_Die:
                case ETesterFunctionType.Multi_Pad:
                    _MPITesterKernel = new MultiDie_TesterKernel();
                    break;
                //---------------------------------------------------------------------------
                default:
                    _MPITesterKernel = new HS_TesterKernel();
                    break;
            }

            FormAgent.MainForm.SetKernelErrorCodeEvent();

            _MPITesterKernel.Init("", DataCenter._rdFunc);

            //---------------------------------------------------------------------------------------------------------------
            // (2) create tester server 
            //---------------------------------------------------------------------------------------------------------------
            switch (DataCenter._machineConfig.TesterCommMode)
            {
                case ETesterCommMode.TCPIP:

                    _MPITCPTestServer = new TCPTestServer(_MPITesterKernel, DataCenter._uiSetting.IsTCPIPSendEnableResultItem);

                    _MPITCPTestServer.TCPIPStateChangeEvent += new ClientRole.StateChangeHandler(TCPIPStateChangeHandker);

                    _MPITCPTestServer.Open("TCP_TEST_SERVER", DataCenter._machineConfig.IPAddr01, DataCenter._machineConfig.NetPort01);

                    _MPITCPTestServer.ServerQueryEvent += new EventHandler<ServerQueryEventArg>(ServerQueryEventHandler);

                    break;
                //---------------------------------------------------------------------------
                case ETesterCommMode.BySoftware:

                    _MPITestServer = new TestServer.TestServer(_MPITesterKernel);

                    if (!_MPITestServer.Open())
                    {
                        Host.SetErrorCode(EErrorCode.CommunicationModeError);
                    }

                    _MPITestServer.ServerQueryEvent += new EventHandler<ServerQueryEventArg>(ServerQueryEventHandler);

                    break;
                //---------------------------------------------------------------------------
                case ETesterCommMode.IO:
                    _MPIIOTestServer = new IOServer(_MPITesterKernel, DataCenter._machineConfig.EQModle, DataCenter._machineConfig.ActiveState);

                    if (!_MPIIOTestServer.Open(EIOCardModel.PCI1756))
                    {
                        Host.SetErrorCode(EErrorCode.MachineCfgSettingErr);
                    }

                    _MPIIOTestServer.ServerQueryEvent += new EventHandler<ServerQueryEventArg>(ServerQueryEventHandler);

                    break;
                //---------------------------------------------------------------------------
                //TCPIP_MPI
                case ETesterCommMode.TCPIP_MPI:
                    {
                        _MPITCPTestServer = new TCPTestServer2(_MPITesterKernel, DataCenter._uiSetting.IsTCPIPSendEnableResultItem);

                        _MPITCPTestServer.TCPIPStateChangeEvent += new ClientRole.StateChangeHandler(TCPIPStateChangeHandker);

                        _MPITCPTestServer.Open("TCP_TEST_SERVER", DataCenter._machineConfig.IPAddr01, DataCenter._machineConfig.NetPort01);

                        _MPITCPTestServer.ServerQueryEvent += new EventHandler<ServerQueryEventArg>(ServerQueryEventHandler);
                    }
                    break;
                //---------------------------------------------------------------------------
                default:

                    _MPITCPTestServer = null;

                    _MPITestServer = null;

                    _MPIIOTestServer = null;

                    rtn &= false;

                    break;
            }

            DataCenter._machineInfo = _MPITesterKernel.MachineInfo;

            //---------------------------------------------------------------------------------------------------------------
            // (5) SysSetting TesterSpecCtrl 
            //---------------------------------------------------------------------------------------------------------------
            DataCenter._sysSetting.SpecCtrl.SetSpecBoundary(DataCenter._machineInfo, DataCenter._rdFunc.RDFuncData.SpecDataDefinition);

            // DataCenter._sysSetting.SpecCtrl.SetSpecDescription(DataCenter._machineConfig, DataCenter._machineInfo, DataCenter._rdFunc.RDFuncData.SpecDataDefinition);

            if (rtn == false)
            {
                Host.SetErrorCode(EErrorCode.MachineCfgSettingErr);

                return rtn;
            }
            //---------------------------------------------------------------------------------------------------------------
            // (6) Initialize variable and events 
            //---------------------------------------------------------------------------------------------------------------
            _MPITesterKernel.FinishTestAndCalcEvent += new EventHandler<EventArgs>(AppSystem.FinishTestAndCalcEventHandler);
            _MPITesterKernel.FinishTestAndCalcEvent += new EventHandler<EventArgs>(FormAgent.TestResultAnalyzeForm.UpdateChartDataToUIForm);
            _MPITesterKernel.FinishTestAndCalcEvent += new EventHandler<EventArgs>(FormAgent.TestResultSpectrum.UpdateChartDataToUIForm);
            _MPITesterKernel.FinishTestAndCalcEvent += new EventHandler<EventArgs>(FormAgent.TestResultAnalyzeForm2.UpdateChartDataToUIForm);
            _MPITesterKernel.FinishTestAndCalcEvent += new EventHandler<EventArgs>(FormAgent.TestResultCurveAnalyzeForm.UpdateChartDataToUIForm);

            ResetDataList();

            //DataCenter._acquireData = (_MPITesterKernel as HS_TesterKernel).Data.Clone() as AcquireData;

            //---------------------------------------------------------------------------------------------------------------
            // (7) DataCenter has loaded and MPITestKernel has initialized, then set Data to kernel first
            //---------------------------------------------------------------------------------------------------------------
            SetDataToSystem();

            DataCenter._acquireData = _MPITesterKernel.Data.Clone() as AcquireData;

            CheckMachineHW();

            return true;
        }

        public static void Fire_ShowMapDataEvent(int row, int col, Dictionary<string, float> val)
        {
            if (DataCenter._tempCond.TestItemArray == null || DataCenter._tempCond.TestItemArray.Length == 0)
            {
                return;
            }

            if (ShowMapDataEvent != null)
            {
                ShowMapDataEventArgs map_data = new ShowMapDataEventArgs(row, col, new Dictionary<string, float>(val));

                FormAgent.BaseForm.Invoke((EventHandler<ShowMapDataEventArgs>)ShowMapDataEvent, null, map_data);
            }
        }

        public static void Fire_SaveBinMapImgEvent(string fileName)
        {
            if (SaveBinMapEvnet != null)
            {
                SaveBinMapArgs binMap_data = new SaveBinMapArgs(fileName);

                FormAgent.BaseForm.Invoke((EventHandler<SaveBinMapArgs>)SaveBinMapEvnet, null, binMap_data);
            }
        }

        public static void Fire_SwitchUIEvent(int index)
        {
            if (!FormAgent.BaseForm.IsHandleCreated)
                return;

            if (SwitchUIEvent != null)
            {
                SwitchUIArgs UIdata = new SwitchUIArgs(index);
                FormAgent.BaseForm.Invoke((EventHandler<SwitchUIArgs>)SwitchUIEvent, null, UIdata);
                //  SwitchUIEvent.Invoke(null, map_data);
            }
        }

        public static void Fire_PopUIEvent(int index)
        {
            if (!FormAgent.BaseForm.IsHandleCreated)
                return;

            if (PopDialogEvnet != null)
            {
                SwitchUIArgs args = new SwitchUIArgs(index);

                FormAgent.BaseForm.Invoke((EventHandler<SwitchUIArgs>)PopDialogEvnet, null, args);
            }
        }

        public static void CheckMachineHW()
        {
            switch (DataCenter._machineConfig.TesterFunctionType)
            {
                case ETesterFunctionType.Single_Die:
                case ETesterFunctionType.Multi_Terminal:
                    (_MPITesterKernel as HS_TesterKernel).CheckMachineHW();
                    break;
                //---------------------------------------------------------------------------
                case ETesterFunctionType.Multi_Die:
                    (_MPITesterKernel as MultiDie_TesterKernel).CheckMachineHW();
                    break;
                //---------------------------------------------------------------------------
                default:
                    break;
            }
        }

        public static bool CheckChannelConfig(uint x, uint y, int theta)
        {
            switch (DataCenter._machineConfig.TesterFunctionType)
            {
                //---------------------------------------------------------------------------
                case ETesterFunctionType.Multi_Die:
                    
                    return (_MPITesterKernel as MultiDie_TesterKernel).CheckChannelConfig(x, y, theta);
                //---------------------------------------------------------------------------
                default:
                    return true;
            }
        }

        public static void SetDataToSystem(bool isClearBodyDataList = true)
        {
            Console.WriteLine("[AppSystem], SetDataToSystem() ");

            if (isClearBodyDataList)
            {
                ResetDataList();
            }

            DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

            DataCenter._smartBinning.ResetStatistic();

            DataCenter._sysSetting.IsEnableLDT1ASoftwareClamp = DataCenter._uiSetting.UserDefinedData.IsLDT1ASoftwareClamp;

            //-----------------------------------------------------------------------------------
            //  20131205 Roy
            //  ESD High Speed Mode & ESD High Speed Delay Time Setting
            //-----------------------------------------------------------------------------------
            DataCenter._sysSetting.EsdDevSetting.IsHighSpeedMode = DataCenter._rdFunc.RDFuncData.IsEnableESDHighSpeedMode;

            DataCenter._sysSetting.EsdDevSetting.HighSpeedDelayTime = DataCenter._rdFunc.RDFuncData.ESDHighSpeedDelayTime;

            _MPITesterKernel.SetSysData(DataCenter._sysSetting, DataCenter._sysCali);

            _MPITesterKernel.SetCondionData(DataCenter._conditionCtrl, DataCenter._product, DataCenter._smartBinning);

            //string laserInfo = _MPITesterKernel.GetAttMoniterInfo();

            //DataCenter._uiSetting.AttenuatorInfo = laserInfo;

            _MPITesterKernel.SetOpticalSwitchToDefault();

            DataCenter._tempCond = DataCenter._product.TestCondition.Clone() as ConditionData;

            DataCenter._acquireData = _MPITesterKernel.Data.Clone() as AcquireData;

            List<string> names = new List<string>();

            List<string> keyNames = new List<string>();

            names.Clear();

            keyNames.Clear();

            string[] strOtherKeyNames = Enum.GetNames(typeof(EProberDataIndex));

            int headIndex = 0;

            _bodyDataHeadDic = new Dictionary<string, int>(20000);

            foreach (TestResultData data in DataCenter._acquireData.OutputTestResult)
            {
                _bodyDataHeadDic.Add(data.KeyName, headIndex);

                headIndex++;

                if (data.KeyName == "TEST")
                    continue;

                bool isSameKeyName = false;

                for (int k = 0; k < strOtherKeyNames.Length; k++)
                {
                    if (data.KeyName == strOtherKeyNames[k])
                    {
                        isSameKeyName = true;
                    }
                }

                if (isSameKeyName)
                    continue;

                keyNames.Add(data.KeyName);

                names.Add(data.Name);
            }

            _mapItemNames[0] = keyNames.ToArray();

            _mapItemNames[1] = names.ToArray();

            //-------------------------------------------------------------------------------
            // Gilbert, 20121223
            // 每次 SetDataToSystem(),重新抓出有在 UserData 紀錄的 TestResult 之 KeyName,
            // 並紀錄對應在 outputTestResult 中的 Index
            //-------------------------------------------------------------------------------
            _bodyDataDic = new Dictionary<string, int[]>(DataCenter._acquireData.OutputTestResult.Count);

            if (isClearBodyDataList)
            {
                _bodyDataList.Clear();
            }

            _rawDataHead = new List<string[]>(50);

            _rawDataHead.Clear();

            bool isFindEnableKeyName = false;

            int bodyDataIndex = 0;

            //------------------------------------------------------------------------------------------------
            // Alec, 20130530
            // 根據輸出報表ResultItem需求抓出 key 對應到 OutputTestResult 的資訊 Index 加入bodyDataDic02
            //------------------------------------------------------------------------------------------------
            foreach (string keyName in DataCenter._uiSetting.UserDefinedData.ResultItemNameDic.Keys)
            {
                isFindEnableKeyName = false;

                for (int i = 0; i < DataCenter._acquireData.OutputTestResult.Count; i++)
                {
                    if (keyName == DataCenter._acquireData.OutputTestResult[i].KeyName &&
                        DataCenter._acquireData.OutputTestResult[i].IsEnable == true)
                    {
                        int[] temp = new int[] { i, bodyDataIndex };

                        _bodyDataDic.Add(keyName, temp);

                        bodyDataIndex++;

                        isFindEnableKeyName = true;

                        break;
                    }
                }

                if (isFindEnableKeyName == false)
                {
                    int[] temp = new int[] { -1, -1 };

                    _bodyDataDic.Add(keyName, temp);
                }
            }

            //------------------------------------------------------------------------------------------------
            // Alec, 20130530
            // 當報表開啟 ExtResultItem 功能時
            // 根據輸出報表 ExtResultItem 需求抓出 Key 對應到 OutputTestResult 的資訊 Index 加入bodyDataDic02
            //------------------------------------------------------------------------------------------------
            if (DataCenter._uiSetting.IsExtResultItem)
            {
                foreach (string keyName in DataCenter._uiSetting.UserDefinedData.ResultItemExtNameDic.Keys)
                {
                    if (_bodyDataDic.ContainsKey(keyName))
                    {
                        continue;
                    }

                    isFindEnableKeyName = false;

                    for (int i = 0; i < DataCenter._acquireData.OutputTestResult.Count; i++)
                    {
                        if (keyName == DataCenter._acquireData.OutputTestResult[i].KeyName &&
                            DataCenter._acquireData.OutputTestResult[i].IsEnable == true)
                        {
                            int[] temp = new int[] { i, bodyDataIndex };

                            _bodyDataDic.Add(keyName, temp);

                            bodyDataIndex++;

                            isFindEnableKeyName = true;

                            break;
                        }
                    }

                    if (isFindEnableKeyName == false)
                    {
                        int[] temp = new int[] { -1, -1 };

                        _bodyDataDic.Add(keyName, temp);
                    }
                }
            }

            //------------------------------------------------------------------------------------------------
            // Alec, 20130530
            // 根據工單分析是否含有 ESDTestItem 
            // 抓出量測數據的 Key 對應到 OutputTestResult 的資訊 Index 加入bodyDataDic02
            //------------------------------------------------------------------------------------------------
            ESDTestItem esdTestItem = null;

            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                foreach (TestItemData data in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (data is ESDTestItem)
                    {
                        esdTestItem = (data as ESDTestItem);
                    }
                }
            }

            // 若是 TestItem = Disable, 則移出這所相對應的 MsrtResult 的 Index 值 ( 設為負值 )
            if (DataCenter._tempCond.TestItemArray != null)
            {
                foreach (TestItemData item in DataCenter._tempCond.TestItemArray)
                {
                    if (item.IsEnable == false && item.MsrtResult != null)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            if (_bodyDataDic.ContainsKey(data.KeyName))
                            {
                                _bodyDataDic[data.KeyName][0] = -2;
                            }
                        }
                    }
                }
            }

            //------------------------------------------------------------------------------------------------
            // Alec, 2013823
            // 強制加入系統需求資訊
            //------------------------------------------------------------------------------------------------
            string[] systemData = Enum.GetNames(typeof(ESysResultItem));

            for (int i = 0; i < DataCenter._acquireData.OutputTestResult.Count; i++)
            {
                TestResultData data = DataCenter._acquireData.OutputTestResult[i];

                if (systemData.Contains(data.KeyName))
                {
                    if (_bodyDataDic.ContainsKey(data.KeyName))
                    {
                        if (_bodyDataDic[data.KeyName][0] < 0)
                        {
                            _bodyDataDic[data.KeyName][0] = i;

                            _bodyDataDic[data.KeyName][1] = bodyDataIndex;

                            bodyDataIndex++;
                        }
                    }
                    else
                    {
                        int[] temp = new int[] { i, bodyDataIndex };

                        _bodyDataDic.Add(data.KeyName, temp);

                        bodyDataIndex++;
                    }
                }
            }

            //-------------------------------------------------------------------------------
            // Gilbert, 20121223
            // Create the rawDataHead, 量測項目中有 RawDataArray 值的，將相關資訊記錄下來
            //-------------------------------------------------------------------------------
            int index = 0;
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb.Clear();
            sb2.Clear();
            foreach (KeyValuePair<string, int[]> kvp in _bodyDataDic)
            {
                index = kvp.Value[0];
                if (index >= 0)
                {
                    sb.Clear();
                    if (DataCenter._acquireData.OutputTestResult[index].RawValueArray != null)
                    {
                        sb.Append(DataCenter._acquireData.OutputTestResult[index].KeyName);
                        sb.Append(",");
                        sb.Append(DataCenter._acquireData.OutputTestResult[index].Name);
                        sb.Append(",");
                        sb.Append(DataCenter._acquireData.OutputTestResult[index].RawValueArray.Length);

                        _rawDataHead.Add(sb.ToString().Split(','));

                        sb2.Append(DataCenter._acquireData.OutputTestResult[index].Name);
                        sb2.Append("_Avg,");
                        for (int m = 1; m < DataCenter._acquireData.OutputTestResult[index].RawValueArray.Length; m++)
                        {
                            sb2.Append(DataCenter._acquireData.OutputTestResult[index].Name);
                            sb2.Append(",");
                        }
                    }
                }

            }

            if (sb2.Length > 1)
            {
                sb2.Remove(sb2.Length - 1, 1);
            }

            _laserCheckCnt = 0;

            _rawDataHead.Add(new string[] { "RawData" });

            _rawDataHead.Add(sb2.ToString().Split(','));

            _consecutiveRecordData.Clear();

            if (isClearBodyDataList)
            {
                AppSystem.SetDataToReport();

                // Report.ReportProcess.RunCommand(TestServer.EServerQueryCmd.CMD_TESTER_START);
            }
        }

        public static void ResetDataList()
        {
            Console.WriteLine("[AppSystem], ResetDataList() ");

            //----------------------------------------------
            // reset body data content
            //----------------------------------------------
            _bodyDataCount = 0;

            if (_bodyDataList == null)
            {
                _bodyDataList = new List<float[]>(20000);
            }
            else
            {
                _bodyDataList.Clear();
            }

            if (_bodyDataList03 == null)
            {
                _bodyDataList03 = new List<Dictionary<string, List<float>>>(20000);
            }
            else
            {
                _bodyDataList03.Clear();
            }

            if (_bodyRowColIndex == null)
            {
                _bodyRowColIndex = new List<string>(20000);
            }
            else
            {
                _bodyRowColIndex.Clear();
            }

            if (_rawDataList == null)
            {
                _rawDataList = new List<double[]>(20000);
            }
            else
            {
                _rawDataList.Clear();
            }

            if (_rawData == null)
            {
                _rawData = new List<double>(100);
            }
            else
            {
                _rawData.Clear();
            }

            //----------------------------------------------
            // reset sweep data
            //----------------------------------------------
            _outputBigData.Clear();

            _outputBigData.IsCheckRowCol = DataCenter._sysSetting.IsCheckRowCol;

            _outputBigData.IsEnableSaveAbsoluteSpectrum = DataCenter._uiSetting.IsEnableSaveAbsoluteSpectrum;

            _outputBigData.IsEnableSaveRelativeSpectrum = DataCenter._uiSetting.IsEnableSaveRelativeSpectrum;

            _outputBigData.IsEnableSaveDarkSpectrum = DataCenter._uiSetting.IsEnableSaveDarkSpectrum;

            _outputBigData.SaveSpectrumMaxCount = DataCenter._uiSetting.SaveSpectrumMaxCount;

            _outputBigData.IsEnableSaveAllSweepData = DataCenter._uiSetting.IsEnableSweepPath & DataCenter._sysSetting.SpecCtrl.IsSupportedSweepItem;

            _outputBigData.IsEnableSaveLIVData = DataCenter._uiSetting.IsEnableSaveLIVData & (DataCenter._sysSetting.SpecCtrl.IsSupportedLIVItem
                || DataCenter._sysSetting.SpecCtrl.IsSupportedTransistorItem || DataCenter._sysSetting.SpecCtrl.IsSupportedLCRSweepItem);

            _outputBigData.IsEnableSavePIVData = DataCenter._uiSetting.IsEnableSaveLIVData & DataCenter._sysSetting.SpecCtrl.IsSupportedPIVItem;

        }

        public static void Run()
        {
            // Set AutoRunStart
            if (_MPITCPTestServer != null)
            {
                _MPITCPTestServer.IsAutoRunStart = true;
            }

            if (_MPITestServer != null)
            {
                _MPITestServer.IsAutoRunStart = true;
            }

            if (_MPIIOTestServer != null)
            {
                _MPIIOTestServer.IsAutoRunStart = true;
            }

            AppSystem.ClearMapAndCIEChart();

            if (_preSamplingCheck.Start(DataCenter._uiSetting, DataCenter._product) == false)
            {
                TopMessageBox.Show("Sampling Data Load Fail");
            }
        }

        public static void RunSingleRetest()
        {
            Console.WriteLine("[AppSystem], RunSingleRetest()");
            
            // AppSystem.ClearMapAndCIEChart();
            //  CheckMachineHW();
            _MPITesterKernel.CmdData.CmdID = ((int)ETesterKernelCmd.RunSingleRetest);

            if (_MPITesterKernel.RunCommand((int)ETesterKernelCmd.RunSingleRetest) == false)
            {
                _MPITesterKernel.Stop();
                return;
            }

        }

        public static void ManualRun(uint cycleCount, uint RepeatCount, uint RepeatDelay)
        {
            DataCenter._uiSetting.WaferMapLeft = 0;
            DataCenter._uiSetting.WaferMapTop = 0;

            DataCenter._uiSetting.WaferMapRight = (int)Math.Floor(Math.Sqrt(RepeatCount * DataCenter._machineConfig.ChannelConfig.ChannelCount));
            DataCenter._uiSetting.WaferMapBottom = (int)Math.Floor(Math.Sqrt(RepeatCount * DataCenter._machineConfig.ChannelConfig.ChannelCount));

            AppSystem.ClearMapAndCIEChart();

            GlobalFlag.IsSuccessCheckChannelConfig = false;

            DataCenter._uiSetting.WaferBeginTime = DateTime.Now;
            //-------------------------------------------
            // (1) Run simulator 
            //-------------------------------------------
            if (DataCenter._uiSetting.LoginID == "simulator")
            {
                _MPITesterKernel.CmdData.CmdID = ((int)ETesterKernelCmd.SimulatorRun);
                _MPITesterKernel.CmdData.IntData[0] = (int)cycleCount;
                _MPITesterKernel.CmdData.IntData[1] = (int)RepeatCount;
                _MPITesterKernel.CmdData.IntData[2] = (int)RepeatDelay;

                if (_MPITesterKernel.RunCommand((int)ETesterKernelCmd.SimulatorRun) == false)
                {
                    _MPITesterKernel.Stop();
                    return;
                }
            }
            else
            {
                Console.WriteLine("[AppSystem], ManualRun(), Start, Repeat={0} Delay={1}", RepeatCount, RepeatDelay);

                SetCoordTransToolForManualTest(cycleCount, RepeatCount);
                //--------------------------------------------------------------------
                // 20131205 ESD High Speed 為了電容充電 需在執行 SetDataToSystem()
                //--------------------------------------------------------------------            
                bool isClearBodyDataList = false;

                SetDataToSystem(isClearBodyDataList);

                //  Report.ReportProcess.RunCommand(TestServer.EServerQueryCmd.CMD_TESTER_START);

                CheckMachineHW();
                _MPITesterKernel.CmdData.CmdID = ((int)ETesterKernelCmd.ManualRun);
                _MPITesterKernel.CmdData.IntData[0] = (int)cycleCount;
                _MPITesterKernel.CmdData.IntData[1] = (int)RepeatCount;
                _MPITesterKernel.CmdData.IntData[2] = (int)RepeatDelay;

                if (_p2TcoordTransTool != null)
                {
                    ReportProcess.CoordTransferTool = _p2TcoordTransTool.Clone() as CoordTransferTool;
                }

                //ProduceInfo.SaveProduceInfo(DataCenter._uiSetting, EServerQueryCmd.CMD_TESTER_START);  // For Dubug

                if (_MPITesterKernel.RunCommand((int)ETesterKernelCmd.ManualRun) == false)
                {
                    SaveBinMapImg();
                    _MPITesterKernel.Stop();
                    Console.WriteLine("[AppSystem], ManualRun(), End");
                    //ProduceInfo.SaveProduceInfo(DataCenter._uiSetting, EServerQueryCmd.CMD_TESTER_END);  // For Dubug
                    return;
                }

            }
            
            _MPITesterKernel.Stop();
        }

        private static CoordTransferTool SetCoordinatTool(int colMin, int rowMin, int colMax, int rowMax)
        {
            int refcolMin = colMin;
            int refrowMin = rowMin;
            int refcolMax = colMax;
            int refrowMax = rowMax;

            DataCenter.ChangeRowColToProbe(ref  refcolMin, ref  refrowMin);
            DataCenter.ChangeRowColToProbe(ref  refcolMax, ref  refrowMax);

            System.Drawing.Rectangle pRect = new System.Drawing.Rectangle(colMin, rowMin, (colMax - colMin), (rowMax - rowMin));
            System.Drawing.Rectangle tRect = new System.Drawing.Rectangle(refcolMin, refrowMin, (refcolMax - refcolMin), (refrowMax - refrowMin));//map因UI座標系關係Y會多轉一次

            return  new CoordTransferTool(pRect, tRect);
        }

        private static void SetCoordTransToolForManualTest(uint cycleCount, uint RepeatCount)
        {
            _oriboundaryDic = new Dictionary<string, int>();
            _oriboundaryDic.Add("tProberChannel", DataCenter._machineConfig.ChannelConfig.ChannelCount);
            _oriboundaryDic.Add("refCol", 0);
            _oriboundaryDic.Add("refRow", 0);
            _oriboundaryDic.Add("colMin", 0);
            _oriboundaryDic.Add("rowMin", 0);

            int colMax = 0, rowMax = 0, colMin = 0, rowMin = 0;
            if (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                int shiftColX = DataCenter._machineConfig.ChannelConfig.ColXCount;

                int shiftRowY = DataCenter._machineConfig.ChannelConfig.RowYCount;

                colMax = (int)cycleCount * shiftColX;
                rowMax = (int)RepeatCount * shiftRowY + 1;

                _oriboundaryDic.Add("colMax", colMax);
                _oriboundaryDic.Add("rowMax", rowMax);
            }
            else
            {
                int len = (int)Math.Floor(Math.Sqrt(RepeatCount));
                colMax = (int)Math.Ceiling(1 + len * 1.5);
                rowMax = (int)Math.Ceiling(1 + len * 1.5);
                _oriboundaryDic.Add("colMax", colMax);
                _oriboundaryDic.Add("rowMax", rowMax);
            }


            if (_p2TcoordTransTool != null )
            {
                _p2TcoordTransTool = SetCoordinatTool(colMin, rowMin, colMax, rowMax);
                if (_p2TcoordTransTool.Matrix != null)
                {
                    ChangeMapRowColByMatrix(_p2TcoordTransTool);
                }
            }
        }

        public static void RunCommand(ETesterKernelCmd cmd)
        {
            _MPITesterKernel.RunCommand((int)cmd);
        }

        public static void StopTest()
        {
            if (_MPITCPTestServer != null)
            {
                _MPITCPTestServer.IsAutoRunStart = false;
            }

            if (_MPITestServer != null)
            {
                _MPITestServer.IsAutoRunStart = false;
            }

            if (_MPIIOTestServer != null)
            {
                _MPIIOTestServer.IsAutoRunStart = false;
            }
        }

        public static void ClearMapAndCIEChart()
        {
            if (AppSystem.OnAppSystemRun != null && GlobalFlag.IsEnableShowMap)
            {
                FormAgent.MainForm.Invoke((EventHandler)OnAppSystemRun, null, null);
            }
        }

        public static void Close()
        {
            if (_MPITestServer != null)
            {
                _MPITestServer.Close();
            }

            if (_MPITCPTestServer != null)
            {
                _MPITCPTestServer.Close();
            }

            if (_MPIIOTestServer != null)
            {
                _MPIIOTestServer.Close();
            }

            if (_MPITesterKernel != null)
            {
                _MPITesterKernel.Close();
            }
        }

        public static void ReConnectTCPIP()
        {
            _MPITCPTestServer.Connect(DataCenter._machineConfig.IPAddr01, DataCenter._machineConfig.NetPort01);
        }

        public static void SetDataToReport()
        {
            List<object> objs = new List<object>();

            objs.Add(DataCenter._uiSetting);

            objs.Add(DataCenter._sysSetting);

            objs.Add(DataCenter._product);

            objs.Add(DataCenter._smartBinning);

            objs.Add(DataCenter._machineInfo);

            objs.Add(DataCenter._machineConfig);

            objs.Add(AppSystem._MPITesterKernel.Data);

            objs.Add(DataCenter._sysCali);

            //objs.Add(DataCenter._conditionCtrl);

            Report.ReportProcess.SetData(objs);
        }

        public static bool RunDeviceVerify(uint channel, uint RepeatCount, uint RepeatDelay)
        {
            _MPITesterKernel.CmdData.CmdID = ((int)ETesterKernelCmd.RunDeviceVerify);
            _MPITesterKernel.CmdData.IntData[0] = (int)channel;
            _MPITesterKernel.CmdData.IntData[1] = (int)RepeatCount;
            _MPITesterKernel.CmdData.IntData[2] = (int)RepeatDelay;

            if (_MPITesterKernel.RunCommand((int)ETesterKernelCmd.RunDeviceVerify) == false)
            {
                _MPITesterKernel.Stop();
                return false;
            }

            _MPITesterKernel.Stop();

            return true;
        }

        public static bool RunLCRCalibration(ELCRCaliMode mode)
        {
            _MPITesterKernel.CmdData.CmdID = ((int)ETesterKernelCmd.RunLcrCalibration);
            _MPITesterKernel.CmdData.IntData[0] = (int)mode;

            if (_MPITesterKernel.RunCommand((int)ETesterKernelCmd.RunLcrCalibration) == false)
            {
                _MPITesterKernel.Stop();
                return false;
            }

            _MPITesterKernel.Stop();

            return true;
        }

        public static bool RunSmuOutput(double srcI, double MsrtV, bool isOutput)
        {
            _MPITesterKernel.CmdData.CmdID = ((int)ETesterKernelCmd.RunSrcOutput);

            _MPITesterKernel.CmdData.IntData[0] = isOutput ? 1 : 0;
            _MPITesterKernel.CmdData.DoubleData[0] = srcI;
            _MPITesterKernel.CmdData.DoubleData[1] = MsrtV;

            if (_MPITesterKernel.RunCommand((int)ETesterKernelCmd.RunSrcOutput) == false)
            {
                _MPITesterKernel.Stop();
                return false;
            }

            _MPITesterKernel.Stop();

            return true;
        }

        public static bool RunOsaCoupling(bool isOutput)
        {
            _MPITesterKernel.CmdData.CmdID = ((int)ETesterKernelCmd.RunOsaCoupling);

            _MPITesterKernel.CmdData.IntData[0] = isOutput ? 1 : 0;

            if (_MPITesterKernel.RunCommand((int)ETesterKernelCmd.RunOsaCoupling) == false)
            {
                _MPITesterKernel.Stop();
                return false;
            }

            _MPITesterKernel.Stop();

            return true;
        }

        public static bool RunAttenuator(ELaserSourceSysAction action ,Dictionary<string,double> paraValDic = null)//1 to get data
        {
            switch (action)
            {
                case(ELaserSourceSysAction.ATTENUATOR_MSRT):
                    {
                       
                    }
                    break;
            }
            _MPITesterKernel.CmdData.CmdID = ((int)ETesterKernelCmd.RunAttenuator);
            _MPITesterKernel.CmdData.IntData[0] = (int)action;

            if (_MPITesterKernel.RunCommand((int)ETesterKernelCmd.RunAttenuator) == false)
            {
                _MPITesterKernel.Stop();
                return false;
            }

            _MPITesterKernel.Stop();

            return true;
        }

        public static double RunPowerMeter( PowerMeterSettingData pmSet)
        {
            double val =0;

            val = _MPITesterKernel.RunPowerMeter(pmSet);

            return val;
        }

        public static double RunAttMoniter(AttenuatorSettingData attSet)
        {
            double val = 0;

            val = _MPITesterKernel.RunAttMoniter(attSet);

            return val;
        }

        public static bool Switch2OpticalCh(int sysCh)
        {
            return _MPITesterKernel.Switch2OpticalCh(sysCh);
        }

        public static bool Switch2D4OpticalCh()
        {
            return _MPITesterKernel.SetOpticalSwitchToDefault();
        }

        public static MinMaxValuePair<double> GetAttPowRangeIndBm(int sysCh)
        {
            return _MPITesterKernel.GetAttPowRangeIndBm(sysCh);
        }

        public static LaserSourceTestItem RunAutoLaserCompensate(LaserSourceTestItem item)
        {
            string pStr, aStr,tStr;
            return _MPITesterKernel.RunAutoLaserCompensate(item, out aStr, out pStr, out tStr);
        }

        public static bool MergeFile(string outputPath, List<string> fileList = null)
        {
            if (fileList != null)
            {
                ReportProcess.MergeFile(outputPath, fileList);
            }
            return true;
        }

        public static Dictionary<string, object> LoadDevRelayInfo( )
        {
            return _MPITesterKernel.LoadDeviceRelayCnt();
        }

        public static void SaveDevRelayInfo(Dictionary<string, object> devLog)
        {
            _MPITesterKernel.ForceSetDeviceRelayCnt(devLog);
        }
    }
}
