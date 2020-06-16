#define PD200

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;

using MPI.RemoteControl.Tester;
using MPI.RemoteControl.Tester.Command;
//using MPI.RemoteControl.MPIDS7600Command;
//using MPI.Comm.TSECommand;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;




namespace MPI.Tester.TestServer
{
    public class TCPTestServer
    {
        private readonly object _lockObj;

        private Thread _thrdTCPIP;

        private static EventWaitHandle _signal;
        private ClientRole _myClient;

        private ETCPTestSeverState _lastState;
        private bool _isConnected;

        private static byte[] _buffer;

        private TesterKernelBase _testerSys;
        private TesterCommandAgent _cmdAgent;

        private bool _isAutoRunStart;
        private string[] _binGradeNames;

        private bool _isBusy;
        private ArrayList _txtLog;

        private MPI.PerformanceTimer _pt1;

        public event EventHandler<ServerQueryEventArg> ServerQueryEvent;
        public ClientRole.StateChangeHandler TCPIPStateChangeEvent;

        private bool _isTCPIPSendEnableResultItem;

        private string _ipAddress;
        private int _port;

        public TCPTestServer()
        {
            this._lockObj = new object();

            this._lastState = ETCPTestSeverState.NOT_READY;
            this._isBusy = false;
            this._isAutoRunStart = false;


            this._thrdTCPIP = new Thread(new ThreadStart(this.ThreadProc));
            _signal = new EventWaitHandle(false, EventResetMode.ManualReset);

            List<int> cmdIDList = new List<int>();
            foreach (int id in Enum.GetValues(typeof(ETSECommand)))//取得所有命令的ID
            {
                cmdIDList.Add(id);
            }
            this._myClient = new ClientRole(cmdIDList);
            this._isConnected = false;
            this._txtLog = new ArrayList();


            this._pt1 = new PerformanceTimer();

            _myClient.TCPClientReceiveEvent += new EventHandler<TCPClientReceiveEventArgs>(TCPClientReceiveEventHandler);

            _myClient.StateChangeEvent += new ClientRole.StateChangeHandler(this.OnTCPIPStateChange);

            this._isTCPIPSendEnableResultItem = false;
        }

        public TCPTestServer(TesterKernelBase kernel, bool IsTCPIPSendEnableResultItem)
            : this()
        {
            this._testerSys = kernel;

            this._cmdAgent = new TesterCommandAgent();

            this._isTCPIPSendEnableResultItem = IsTCPIPSendEnableResultItem;
        }

        #region >>> Public Proberty <<<

        public bool IsAutoRunStart
        {
            get { return this._isAutoRunStart; }
            set { lock (this._lockObj) { this._isAutoRunStart = value; } }
        }

        public bool IsBusy
        {
            get { return this._isBusy; }
        }

        public ArrayList TxtLog
        {
            get { lock (this._txtLog.SyncRoot) return _txtLog; }
        }

        #endregion

        #region >>> Private Methods <<<

        private void TCPClientReceiveEventHandler(object sender, TCPClientReceiveEventArgs e)
        {
            _buffer = e.Data;
            _signal.Set();
        }

        private void Fire_ServerQueryEvent(EServerQueryCmd cmd)
        {
            Fire_ServerQueryEvent(cmd, null, null);
        }

        private void Fire_ServerQueryEvent(EServerQueryCmd cmd, double[] buffer, string[] strData)
        {
            EventHandler<ServerQueryEventArg> handlerInstance = ServerQueryEvent;
            if (handlerInstance != null)
            {
                ServerQueryEventArg theArg = new ServerQueryEventArg();
                theArg.CmdID = (int)cmd;
                theArg.StrData = strData;
                theArg.BufferData = buffer;
                handlerInstance(this, theArg);
            }
        }

        private int CommandProc()
        {
            bool rtn = false;
            AcquireData acquireData;
            string errStr = "";
            if (CheckIfPackageCorrect(_buffer, out errStr))//若_buffer傳回錯誤訊息
            {
                ShowErrorMsg((int)EErrorCode.TCPIP_CheckPacket_Err, errStr);

                CmdError echoTSECmd = new CmdError();
                (echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_UNKNOW_ERROR;
                this._myClient.SendMessage(echoTSECmd.Packet.Serialize());

                return -1;
            }

            MPIDS7600Packet mpiPacket = new MPIDS7600Packet(_buffer);

            if (mpiPacket.CheckPacket())
            {
                MPIDS7600Command echoTSECmd = null;

                MPIDS7600Command cmd = this._cmdAgent.CommandFactory(mpiPacket);

                if (cmd == null)
                {
                    ShowErrorMsg((int)EErrorCode.TCPIP_CommandFactory_Err, "CommandFactory() fail");

                    return 1;
                }

                cmd.Deserialize(mpiPacket.Serialize());

                switch (cmd.CommandID)
                {
                    case (int)ETSECommand.ID_BARCODE_INSERT:
                        {
                            #region
                            Console.WriteLine("[TCPTestServer], ID_BARCODE_INSERT");

                            string testItemFileName = new string((cmd as CmdBarcodeInsert).TestItemName);
                            string binFileName = new string((cmd as CmdBarcodeInsert).BinFileName);
                            string[] strData = new string[2];

                            testItemFileName = testItemFileName.TrimEnd('\0');
                            binFileName = binFileName.Trim('\0');

                            strData[0] = testItemFileName;
                            strData[1] = binFileName;

                            double[] buffer = new double[1];
                            buffer[0] = 0;//IsPresampling

                            // Fire evnet
                            Fire_ServerQueryEvent(EServerQueryCmd.CMD_LOAD_ITEM_FILE, new double[1], strData);

                            if (GlobalFlag.IsSuccessLoadProduct && GlobalFlag.IsSuccessLoadBin)
                            {
                                echoTSECmd = new CmdBarcodeInsert();
                                (echoTSECmd as CmdBarcodeInsert).TestItemName = testItemFileName.ToCharArray();
                                (echoTSECmd as CmdBarcodeInsert).BinFileName = binFileName.ToCharArray();
                            }
                            else if (GlobalFlag.IsSuccessLoadProduct == false)
                            {
                                echoTSECmd = new CmdErrorNoTestItemFile();
                                Console.WriteLine("[TCPTestServer], ID_BARCODE_INSERT, LoadProduct Fail!");
                            }
                            else if (GlobalFlag.IsSuccessLoadBin == false)
                            {
                                echoTSECmd = new CmdErrorNoBinFile();
                                Console.WriteLine("[TCPTestServer], ID_BARCODE_INSERT, LoadBin Fail!");
                            }
                            #endregion
                        }
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_BEGIN:
                        #region
                        {
                            Console.WriteLine("[TCPTestServer], ID_WAFER_BEGIN");
                            string wafferID_Begin = new string((cmd as CmdWaferBegin).WaferNo);
                            wafferID_Begin = wafferID_Begin.TrimEnd('\0');
                            char[] chArr = wafferID_Begin.ToCharArray();

                            if (chArr.Length == 0)
                            {
                                chArr = new char[1] { 'a' };
                                Console.WriteLine("[TCPTestServer], ID_WAFER_BEGIN, WaferID = Empty");
                            }
                            else
                            {
                                chArr[0] = 'a';
                            }
                            string[] strData = new string[1];
                            strData[0] = wafferID_Begin;

                            Fire_ServerQueryEvent(EServerQueryCmd.CMD_WAFER_BEGIN, new double[1], strData);


                            echoTSECmd = new CmdWaferBegin();
                            (echoTSECmd as CmdWaferBegin).WaferNo = chArr;
                            Console.WriteLine("[TCPTestServer], ID_WAFER_BEGIN WaferID:" + wafferID_Begin);
                        }

                        #endregion
                        break;

                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_QUERY_ABLE_MODE: //20180608 David
                        {
                            #region
                            Console.WriteLine("[TCPTestServer], ID_QUERY_ABLE_MODE");

                            string testItemFileName = new string((cmd as CmdQueryAbleMode).TestItemName);
                            string binFileName = new string((cmd as CmdQueryAbleMode).BinFileName);
                            string[] strData = new string[2];

                            testItemFileName = testItemFileName.TrimEnd('\0');
                            binFileName = binFileName.Trim('\0');

                            strData[0] = testItemFileName;
                            strData[1] = binFileName;

                            double[] buffer = new double[1];
                            buffer[0] = 0;//IsPresampling

                            Console.WriteLine("[TCPTestServer], ID_QUERY_ABLE_MODE,change recipe");
                            Fire_ServerQueryEvent(EServerQueryCmd.CMD_LOAD_ITEM_FILE, new double[1], strData);

                            if (GlobalFlag.IsSuccessLoadProduct && GlobalFlag.IsSuccessLoadBin)
                            {
                                echoTSECmd = new CmdBarcodeInsert();
                                (echoTSECmd as CmdBarcodeInsert).TestItemName = testItemFileName.ToCharArray();
                                (echoTSECmd as CmdBarcodeInsert).BinFileName = binFileName.ToCharArray();
                            }
                            else if (GlobalFlag.IsSuccessLoadProduct == false)
                            {
                                echoTSECmd = new CmdErrorNoTestItemFile();
                                Console.WriteLine("[TCPTestServer], ID_BARCODE_INSERT, LoadProduct Fail!");
                                break;
                            }
                            else if (GlobalFlag.IsSuccessLoadBin == false)
                            {
                                echoTSECmd = new CmdErrorNoBinFile();
                                Console.WriteLine("[TCPTestServer], ID_BARCODE_INSERT, LoadBin Fail!");
                                break;
                            }
                            #endregion
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            #region
                            string lotNumStr = new string((cmd as CmdQueryAbleMode).LotNo);
                            string waferNumStr = new string((cmd as CmdQueryAbleMode).WaferNo);
                            string opName = new string((cmd as CmdQueryAbleMode).OperatorName);
                            string pRecipe = new string((cmd as CmdQueryAbleMode).ProberMainRecipe);


                            strData = new string[11];
                            lotNumStr = lotNumStr.TrimEnd('\0');
                            waferNumStr = waferNumStr.TrimEnd('\0');
                            opName = opName.TrimEnd('\0');

                            strData[0] = "";				// barcode number
                            strData[1] = lotNumStr;
                            strData[2] = "";				// cassette number
                            strData[3] = waferNumStr;
                            strData[4] = opName;
                            strData[5] = pRecipe;			// ProberRecipe
                            strData[6] = "";				// ProductType
                            strData[7] = "";				// Substrate
                            strData[8] = "";				// CassettleID
                            strData[9] = "";				// SlotID
                            strData[10] = "";				// CustomerKeyNumber


                            // Fire evnet
                            Fire_ServerQueryEvent(EServerQueryCmd.CMD_CHECK_AVALIABLE_MODE, new double[1], strData);

                            echoTSECmd = new CmdQueryAbleMode();


                            switch (GlobalFlag.OutputReportState)
                            {
                                case EOutputReportState.CanAppend:
                                    {
                                        //Fire_ServerQueryEvent(EServerQueryCmd.CMD_CREATE_MAP_FROM_TEMP, null, null);//create map from temp
                                        (echoTSECmd as CmdQueryAbleMode).FileAppendable = true;
                                        (echoTSECmd as CmdQueryAbleMode).FileNameRepeat = false;
                                    }
                                    break;
                                //case EOutputReportState.CanAppendAndRetest:
                                //    {
                                //        Fire_ServerQueryEvent(EServerQueryCmd.CMD_CREATE_MAP_FROM_TEMP, null, null);//create map from temp
                                //        (echoTSECmd as CmdQueryAbleMode).FileAppendable = true;
                                //        (echoTSECmd as CmdQueryAbleMode).FileNameRepeat = true;
                                //    }
                                //    break;
                                case EOutputReportState.CanOverwrite:
                                case EOutputReportState.CanRetest:
                                    {
                                        (echoTSECmd as CmdQueryAbleMode).FileAppendable = false;
                                        (echoTSECmd as CmdQueryAbleMode).FileNameRepeat = true;
                                    }
                                    break;
                                case EOutputReportState.FileNameIsEmpty:
                                    {
                                        (echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_WAFER_ID_FAIL;
                                    }
                                    break;
                                case EOutputReportState.CanNotAppend:
                                case EOutputReportState.None:
                                    {
                                        (echoTSECmd as CmdQueryAbleMode).FileAppendable = false;
                                        (echoTSECmd as CmdQueryAbleMode).FileNameRepeat = false;
                                    }
                                    break;
                            }

                            (echoTSECmd as CmdQueryAbleMode).TesterCoord = _testerSys.TesterCoord;


                            #endregion
                        }
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_SET_TEST_MODE: //20180608 David
                        #region
                        {
                            Console.WriteLine("[TCPTestServer], ID_START_TEST_MODE");

                            CmdSetTestMode.ETestMode mode = ((cmd as CmdSetTestMode).TestMode);

                            string[] strData = new string[11];

                            echoTSECmd = new CmdSetTestMode();//後面特例時改為CmdError

                            switch (mode)
                            {
                                case CmdSetTestMode.ETestMode.CONTINOUS:
                                    {
                                        Console.WriteLine("[TCPTestServer], ID_START_TEST_MODE,CONTINOUS");
                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_APPEND_TESTER_OUTPUT_FILE, null, null);
                                        //Fire_ServerQueryEvent(EServerQueryCmd.CMD_CREATE_MAP_FROM_TEMP, null, null);//create map from temp

                                        GlobalFlag.OutputReportState = EOutputReportState.CanAppend;
                                    }
                                    break;
                                //case CmdSetTestMode.ETestMode.NG_CONTINOUS:
                                //    {
                                //        Console.WriteLine("[TCPTestServer], ID_START_TEST_MODE,OVERWRITE");
                                //        Fire_ServerQueryEvent(EServerQueryCmd.CMD_OVERWRITE_TESTER_OUTPUT_FILE, new double[1], strData);
                                //        Fire_ServerQueryEvent(EServerQueryCmd.CMD_CREATE_MAP_FROM_TEMP, null, null);//create map from temp
                                //        GlobalFlag.OutputReportState = EOutputReportState.None;
                                //    }
                                //    break;
                                case CmdSetTestMode.ETestMode.NG_RETEST:
                                    {
                                        Console.WriteLine("[TCPTestServer], ID_START_TEST_MODE,NG_Retest");

                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_LOAD_FILE_TO_TEMP, null, null);//clone data to temp                                        
                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_APPEND_TESTER_OUTPUT_FILE, null, null);
                                        GlobalFlag.OutputReportState = EOutputReportState.CanAppend;
                                    }
                                    break;
                                case CmdSetTestMode.ETestMode.OVERWRITE:
                                    {
                                        Console.WriteLine("[TCPTestServer], ID_START_TEST_MODE,OVERWRITE");
                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_OVERWRITE_TESTER_OUTPUT_FILE, new double[1], strData);
                                        GlobalFlag.OutputReportState = EOutputReportState.None;
                                    }
                                    break;

                                default:
                                case CmdSetTestMode.ETestMode.NORMAL:
                                case CmdSetTestMode.ETestMode.NG_SKIP:
                                    {
                                        Console.WriteLine("[TCPTestServer], ID_START_TEST_MODE,NORMAL");
                                        GlobalFlag.OutputReportState = EOutputReportState.None;
                                        //do nothing
                                    }
                                    break;
                                case CmdSetTestMode.ETestMode.UNDEFINED:
                                    Console.WriteLine("[TCPTestServer], ID_START_TEST_MODE,UNDEFINED");
                                    echoTSECmd = new CmdError();
                                    break;
                            }


                            //string lotNumStr = new string((cmd as CmdQueryAbleMode).LotNo);
                            //string waferNumStr = new string((cmd as CmdQueryAbleMode).WaferNo);
                            //string opName = new string((cmd as CmdQueryAbleMode).OperatorName);
                            //string pRecipe = new string((cmd as CmdQueryAbleMode).ProberMainRecipe);

                        }

                        #endregion
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_IN:
                        {
                            #region
                            Console.WriteLine("[TCPTestServer], ID_WAFER_IN");

                            StringBuilder sb = new StringBuilder();
                            string lotNumStr = new string((cmd as CmdWaferIn).LotNo);
                            string waferNumStr = new string((cmd as CmdWaferIn).WaferNo);
                            string opName = new string((cmd as CmdWaferIn).OperatorName);
                            string[] strData = new string[11];

                            lotNumStr = lotNumStr.TrimEnd('\0');
                            waferNumStr = waferNumStr.TrimEnd('\0');
                            opName = opName.TrimEnd('\0');

                            strData[0] = "";				// barcode number
                            strData[1] = lotNumStr;
                            strData[2] = "";				// cassette number
                            strData[3] = waferNumStr;
                            strData[4] = opName;
                            strData[5] = "";				// ProberRecipe
                            strData[6] = "";				// ProductType
                            strData[7] = "";				// Substrate
                            strData[8] = "";				// CassettleID
                            strData[9] = "";				// SlotID
                            strData[10] = "";				// CustomerKeyNumber
                            // Fire evnet
                            Fire_ServerQueryEvent(EServerQueryCmd.CMD_OPEN_OUTPUT_FILE, new double[1], strData);

                            echoTSECmd = new CmdTestItem();
                            // MES Run
                            if (GlobalFlag.IsSuccessLoadMESData == false)
                            {
                                echoTSECmd = new CmdError();

                                (echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_BARCODE_INSERT_FAIL;
                            }
                            else
                            {
                                if (GlobalFlag.OutputReportState == EOutputReportState.None ||
                                    GlobalFlag.OutputReportState == EOutputReportState.CanAppend)
                                {
                                    if (this._isTCPIPSendEnableResultItem)
                                    {
                                        char[] name = new Char[MPIDS7600Command.ConstDefinition.MAX_ITEM_NAME];

                                        acquireData = this._testerSys.Data;

                                        int enableItemcount = 0;

                                        for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                                        {
                                            if (enableItemcount >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                                            {
                                                break;
                                            }

                                            if (acquireData.EnableTestResult.Contains(acquireData.OutputTestResult[i].KeyName))
                                            {
                                                enableItemcount++;
                                            }
                                        }

                                        (echoTSECmd as CmdTestItem).ItemCount = enableItemcount;

                                        int count = 0;

                                        for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                                        {
                                            if (count >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                                            {
                                                break;
                                            }

                                            if (acquireData.EnableTestResult.Contains(acquireData.OutputTestResult[i].KeyName))
                                            {
                                                name = acquireData.OutputTestResult[i].Name.Replace("λ", "Lambda").ToCharArray();

                                                (echoTSECmd as CmdTestItem).SetItemName(count, name);

                                                count++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        char[] name = new Char[MPIDS7600Command.ConstDefinition.MAX_ITEM_NAME];

                                        acquireData = this._testerSys.Data;

                                        (echoTSECmd as CmdTestItem).ItemCount = acquireData.OutputTestResult.Count;
                                        
                                        for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                                        {
                                            if (i >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                                            {
                                                break;
                                            }

                                            name = acquireData.OutputTestResult[i].Name.Replace("λ", "Lambda").ToCharArray();

                                            (echoTSECmd as CmdTestItem).SetItemName(i, name);
                                        }
                                    }
                                }                                
                                else
                                {
                                    echoTSECmd = new CmdError();

                                    (echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_TESTER_OUTPUT_FILENAME_EXIST;
                                }

                            break;
                            }
                            #endregion
                        }
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_SCAN_END:
                        {
                            #region
                            Console.WriteLine("[TCPTestServer], ID_WAFER_SCAN_END");

                            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.EndTest;
                            rtn = this._testerSys.RunCommand((int)ETesterKernelCmd.EndTest);
							string[] strArr = new string[ 1 ];
							strArr[ 0 ] = "";
                            double[] buffer = new double[45];

							for ( int i = 0; i < 45; ++i )
							{
								buffer[ i ] = 0;
							}

                            buffer[0] = (double)(cmd as CmdWaferScanEnd).XMin; //ColX min
                            buffer[1] = (double)(cmd as CmdWaferScanEnd).YMin; //RowY min
                            buffer[2] = (double)(cmd as CmdWaferScanEnd).XMax; //ColX max
                            buffer[3] = (double)(cmd as CmdWaferScanEnd).YMax; //RowY max
                            //buffer[4] = //TotoalSacnCounts
                            //buffer[5] = //ChipXPictch
                            //buffer[6] = //ChipYPictch
                            buffer[7] = (double)(cmd as CmdWaferScanEnd).ChipCount; //TotalProbingCounts
                            buffer[8] = (double)(cmd as CmdWaferScanEnd).ProbeMoveDirection; //MoveMainAxis
                            //buffer[9] = //SamplingMode
                            buffer[10] = (double)(cmd as CmdWaferScanEnd).ProbeXInitDirection; //XInitDirection
                            buffer[11] = (double)(cmd as CmdWaferScanEnd).ProbeYInitDirection; //YinitDirection
                            Console.WriteLine("[TCPTestServer], ID_WAFER_SCAN_END,XMin:" + (cmd as CmdWaferScanEnd).XMin.ToString());
                            Console.WriteLine("[TCPTestServer], ID_WAFER_SCAN_END,YMin:" + (cmd as CmdWaferScanEnd).YMin.ToString());
                            Console.WriteLine("[TCPTestServer], ID_WAFER_SCAN_END,XMax:" + (cmd as CmdWaferScanEnd).XMax.ToString());
                            Console.WriteLine("[TCPTestServer], ID_WAFER_SCAN_END,YMax:" + (cmd as CmdWaferScanEnd).YMax.ToString());

                            buffer[16] = 0.0d; // HighSpeedMode Disable

							Fire_ServerQueryEvent( EServerQueryCmd.CMD_TESTER_START, buffer, strArr );
                            echoTSECmd = cmd;//new CmdWaferScanEnd();

                            #endregion
                        }
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_SOT:
                        {
                            #region

                            this._pt1.Start();
                            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.RunTest;
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.COL] = Convert.ToDouble((cmd as CmdSOT).WaferPositionX);
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ROW] = Convert.ToDouble((cmd as CmdSOT).WaferPositionY);
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.PROBE_INDEX] = Convert.ToDouble((cmd as CmdSOT).ProbeIndex);
                            this._testerSys.CmdData.DoubleData[2] = (cmd as CmdSOT).ChipIndex;//SOT時沒有用到DoubleData[2]，先拿來用一下

                            rtn = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);

                            this._testerSys.GetTestedDataFromDevice();
                            this._testerSys.ResetTesterCond();

                            string strData = "";
                            char[] resultData = new Char[MPIDS7600Command.ConstDefinition.MAX_RESULT_DATA];
                            acquireData = (this._testerSys as HS_TesterKernel).Data;

                            echoTSECmd = new CmdEOT();

                            int enableItemcount = 0;

                            for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                            {
                                if (enableItemcount >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                                {
                                    break;
                                }

                                if (acquireData.EnableTestResult.Contains(acquireData.OutputTestResult[i].KeyName))
                                {
                                    enableItemcount++;
                                }
                            }

                            int count = 0;

                            if (this._isTCPIPSendEnableResultItem)
                            {
                                (echoTSECmd as CmdEOT).ResultDataCount = enableItemcount;

                                for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                                {
                                    if (count >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                                    {
                                        break;
                                    }

                                    if (acquireData.EnableTestResult.Contains(acquireData.OutputTestResult[i].KeyName))
                                    {
                                        strData = acquireData.OutputTestResult[i].Value.ToString(acquireData.OutputTestResult[i].Formate);

                                        resultData = strData.ToCharArray();

                                        (echoTSECmd as CmdEOT).SetResultData(count, resultData);

                                        count++;
                                    }
                                }
                            }
                            else
                            {
                                (echoTSECmd as CmdEOT).ResultDataCount = acquireData.OutputTestResult.Count;

                                for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                                {
                                    if (count >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                                        break;

                                    strData = acquireData.OutputTestResult[i].Value.ToString(acquireData.OutputTestResult[i].Formate);
                                    resultData = strData.ToCharArray();
                                    (echoTSECmd as CmdEOT).SetResultData(i, resultData);
                                    count++;
                                }
                            }

                            (echoTSECmd as CmdEOT).Bin = acquireData.ChipInfo.BinGrade;

                            if (acquireData.ChipInfo.IsPass == true)
                            {
                                (echoTSECmd as CmdEOT).GoodNG = 1;	// TSE Protocal 1: Good , " minValue < value <maxValue "
                            }
                            else
                            {
                                (echoTSECmd as CmdEOT).GoodNG = 2;	// TSE Protocal 2: NG
                            }
                            //-----------------------------------------------------------
                            // (2) Transfer UI Operation State
                            //----------------------------------------------------------
                            //this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_CALC, null, null);

                            if (GlobalFlag.IsSourceMeterDisconnect)
                            {
                                Console.WriteLine("[TCPTestServer], SourceMeter Disconnect");
                                echoTSECmd = new CmdError();

                                (echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_TESTER_SOURCEMETER_DISCONNECT;
                            }

 
                            #endregion
                        }
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_MUTIL_DIE_SOT:
                        #region >>multi DIE SOT<<
                        {
                            this._pt1.Start();

                            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.RunTest;

                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.COL] = Convert.ToDouble((cmd as CmdMutiDieSOT).Col);

                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ROW] = Convert.ToDouble((cmd as CmdMutiDieSOT).Row);

                            char[] channelStatus = (cmd as CmdMutiDieSOT).ChannelStatus;

                            this._testerSys.CmdData.StringData[0] = new string(channelStatus).TrimEnd('\0');

                            this._testerSys.CmdData.StringData[1] = new string((cmd as CmdMutiDieSOT).SubBin).TrimEnd('\0');

                            this._testerSys.CmdData.StringData[2] = new string(channelStatus).TrimEnd('\0');

                            rtn = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);

                            this._testerSys.GetTestedDataFromDevice();

                            this._testerSys.ResetTesterCond();

                            acquireData = (this._testerSys as MultiDie_TesterKernel).Data;

                            echoTSECmd = new CmdMutiDieEOT();


                            #region >>enable item<<

                            List<string> enableItem = new List<string>();                            

                            for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                            {
                                //if (acquireData.OutputTestResult[i].IsEnable && acquireData.OutputTestResult[i].eDataName != string.Empty)
                                if (this._isTCPIPSendEnableResultItem)
                                {
                                    if ( acquireData.OutputTestResult[i].Name != string.Empty  &&
                                        acquireData.EnableTestResult.Contains(acquireData.OutputTestResult[i].KeyName))
                                    //if (acquireData.OutputTestResult[i].IsEnable && acquireData.OutputTestResult[i].Name != string.Empty)
                                    {
                                        enableItem.Add(acquireData.OutputTestResult[i].Name);
                                    }
                                }
                                else
                                {
                                    enableItem.Add(acquireData.OutputTestResult[i].Name);                                    
                                }
                            }
                            #endregion


                            MUTIL_DIE_DATA_LIST dieDataList = new MUTIL_DIE_DATA_LIST();

                            dieDataList.Init();

                            for (int channel = 0; channel < acquireData.ChannelResultDataSet.Count; channel++)
                            {

                                ChannelResultData channelResultData = acquireData.ChannelResultDataSet[(uint)channel];

                                dieDataList.DieDataArray[channel].DataCount = enableItem.Count;

                                for (int i = 0; i < enableItem.Count; i++)
                                {
                                    for (int k = 0; k < channelResultData.Count; k++)
                                    {
                                        if (enableItem[i] == channelResultData[k].Name)
                                        {
                                            if (double.IsNaN(channelResultData[i].Value) ||
                                                double.IsInfinity(channelResultData[i].Value))
                                            {
                                                dieDataList.DieDataArray[channel].TestResults[i] = 0.0d;
                                            }
                                            else
                                            {
                                                dieDataList.DieDataArray[channel].TestResults[i] = channelResultData[k].Value;
                                            }
                                        }
                                    }
                                }

                                dieDataList.DieDataArray[channel].Col = channelResultData.Col; ;

                                dieDataList.DieDataArray[channel].Row = channelResultData.Row;

                                dieDataList.DieDataArray[channel].BinCode = channelResultData.BinGradeName;

                                dieDataList.DieDataArray[channel].Bin = channelResultData.BinGrade;

                                if (channelResultData.IsPass)
                                {
                                    dieDataList.DieDataArray[channel].Pass = 1;
                                }
                                else
                                {
                                    dieDataList.DieDataArray[channel].Pass = 0;
                                }

                            }

                            dieDataList.Count = acquireData.ChannelResultDataSet.Count;

                            (echoTSECmd as CmdMutiDieEOT).SetMutilDieData(dieDataList);


                            //-----------------------------------------------------------
                            // (2) Transfer UI Operation State
                            //----------------------------------------------------------
                            //this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_CALC, null, null);

                            if (GlobalFlag.IsSourceMeterDisconnect)
                            {
                                Console.WriteLine("[TCPIP], SourceMeter Disconnect");

                                echoTSECmd = new CmdError();

                                (echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_TESTER_SOURCEMETER_DISCONNECT;
                            }

                        }
                        #endregion
                        break;
                    //------------------------------------------------------------------------------------------------
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_END:

                        Console.WriteLine("[TCPTestServer], ID_WAFER_END");
                        double[] buffer2 = new double[9];

                        this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.EndTest;
                        this._testerSys.RunCommand((int)ETesterKernelCmd.EndTest);

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_TESTER_END, buffer2, null);
                        echoTSECmd = new CmdWaferEnd();

                        if (GlobalFlag.IsEnableEndTest == false)
                        {
                            echoTSECmd = new CmdError();
                            (echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_TESTER_COMMAND_LINE_WAITTING;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_LOT_END:
                        echoTSECmd = new CmdLotEnd();
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_LOT_IN:
                        echoTSECmd = new CmdLotIn();
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_AUTOCAL_START:
                        echoTSECmd = new CmdAutoCalibrationStart();
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_AUTOCAL_SOT:
                        rtn = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);
                        this._testerSys.ResetTesterCond();
                        echoTSECmd = new CmdAutoCalibrationEOT();
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_AUTOCAL_END:
                        //Fire_ServerQueryEvent(EServerQueryCmd.CMD_AUTOCAL_END);

                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_BIN_GRADE:
                        #region

                        Console.WriteLine("[TCPTestServer], ID_BIN_GRADE");
                        echoTSECmd = new CmdBinGrade();

                        this._binGradeNames = (this._testerSys as HS_TesterKernel).GetBinGradeNames();

                        (echoTSECmd as CmdBinGrade).BinGradeCount = (this._binGradeNames.Length + 1);

                        for (int i = 0; i < this._binGradeNames.Length; i++)
                        {
                            (echoTSECmd as CmdBinGrade).SetBinGradeName(i, this._binGradeNames[i]);
                        }
                        //Fire_ServerQueryEvent(EServerQueryCmd.CMD_AUTOCAL_END);
                        (echoTSECmd as CmdBinGrade).SetBinGradeName(this._binGradeNames.Length, "Fail Bin");

                        #endregion
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_BARCODE_PRINT:

                        #region
                        Console.WriteLine("[TCPTestServer], ID_BARCODE_PRINT");

                        echoTSECmd = new CmdBarcodePrint();

                        GlobalFlag.IsSuccessLoadBin = false;
                        string[] outData = new string[6];
                        outData[0] = (cmd as CmdBarcodePrint).TubeNumber.ToString();
                        outData[1] = (cmd as CmdBarcodePrint).TubeBin.ToString();
                        outData[2] = (cmd as CmdBarcodePrint).TubeCount.ToString();
                        outData[3] = new string((cmd as CmdBarcodePrint).TubeID);
                        outData[4] = new string((cmd as CmdBarcodePrint).PartID);
                        outData[5] = (cmd as CmdBarcodePrint).TubePullNumber.ToString();

                        outData[3] = outData[3].TrimEnd('\0');
                        outData[4] = outData[4].TrimEnd('\0');

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_PRINT_BARCODE, new double[1], outData);

                        if (GlobalFlag.IsFinishPrintBarcode)
                        {
                            Console.WriteLine("[TCPTestServer], ID_BARCODE_PRINT.SUCCESS");
                        }
                        else
                        {
                            Console.WriteLine("[TCPTestServer], ID_BARCODE_PRINT.DS_ERROR_PRINT_BARCODE_FAIL");
                            echoTSECmd = new CmdError();
                            (echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_PRINT_BARCODE_FAIL;
                        }
                        #endregion
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_OVERRIDE_TESTER_REPORT:
                        {
                            echoTSECmd = new CmdOverrideTesterReport();

                            Console.WriteLine("[TCPTestServer], ID_OVERRIDE_TESTER_REPORT");

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_OVERWRITE_TESTER_OUTPUT_FILE, null, null);

                            Console.WriteLine("[TCPTestServer], ID_OVERRIDE_TESTER_REPORT.SUCCESS");
                        }

                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_FINISH:
                        {
                            string wafferID_Fin = new string((cmd as CmdWaferFinish).WaferNo);
                            wafferID_Fin = wafferID_Fin.TrimEnd('\0');

                            string[] strData = new string[1];
                            strData[0] = wafferID_Fin;

                            Fire_ServerQueryEvent(EServerQueryCmd.CMD_WAFER_FINISH, new double[1], strData);

                            echoTSECmd = new CmdWaferFinish();
                            (echoTSECmd as CmdWaferFinish).WaferNo = wafferID_Fin.ToCharArray();
                            Console.WriteLine("[TCPTestServer], ID_WAFER_FINISH WaferID:" + wafferID_Fin);
                        }
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_TEST_ABORT:
                        {
                            Console.WriteLine("[EMPITestServerCmd], ID_TEST_ABORT");
                            double[] buffer = new double[1];

                            //buffer[0] = (cmd as CmdTestAbort).ABORT_SAVE_FILE; //0: Abort, 1: SaveFile
                            buffer[0] = 1;//先強迫寫死，後續等Prober開放此設定的UI後再改回來 20190619

                            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.StopTest;
                            this._testerSys.RunCommand((int)ETesterKernelCmd.StopTest);

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_TESTER_ABORT, buffer, null);

                            echoTSECmd = new CmdTestAbort();

                        }
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_QUERY_INFORMATION:
                        {
                            #region
							TransferableCommonObjectBase baseObj = new TransferableCommonObjectBase();
							( cmd as CmdQueryInformation ).GetTransferableItem( baseObj );

                            echoTSECmd = new CmdQueryInformation();

							int objctType = baseObj.GetHashCode();

                            switch (objctType)
                            {
                                case ((int)ETransferableCommonObject.MC300Data):
                                    {
#if MC300
                                        MC300Data mc300 = new MC300Data();

                                        if ((this._testerSys as HS_TesterKernel).StartSendInformation())
                                        {
                                            mc300.TestSpecTable = table;
                                        }
                                        else
                                        {
                                            mc300.TestSpecTable = null;
                                        }

                                        (echoTSECmd as CmdQueryInformation).SetTransferableItem(mc300);
#endif
                                    }
                                    break;
                                case ((int)ETransferableCommonObject.ProcessInformation):
                                    {
                                        #region >>> ProcessInformation <<<

                                        ProcessInformation processInfo = new ProcessInformation();

                                        (cmd as CmdQueryInformation).GetTransferableItem(processInfo);

                                        string[] strData = new string[10];
                                        double[] buffer = new double[1];

                                        strData[0] = "";				                    // barcode number
                                        strData[1] = processInfo.LotID;
                                        strData[2] = processInfo.CassetteID;                  // cassette number
                                        strData[3] = processInfo.WaferID;
                                        strData[4] = processInfo.Operator;
                                        strData[5] = processInfo.ProberRecipe;                // ProberRecipe
                                        strData[6] = "";				                    // ProductType
                                        strData[7] = "";				                    // Substrate
                                        strData[8] = processInfo.CassetteID;                  // CassettleID
                                        strData[9] = "";				                    // SlotID

                                        buffer[0] = processInfo.TestTemperature;
                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_QUERY_PROCESS_INFO, buffer, strData);

                                        (echoTSECmd as CmdQueryInformation).SetTransferableItem(processInfo);
                                        #endregion
                                    }
                                    break;
                                case ((int)ETransferableCommonObject.PreWaferINInformation):
                                    {
                                        #region >>> PreWaferINInformation <<<

                                        PreWaferINInformation preWaferIn = new PreWaferINInformation();

                                        (cmd as CmdQueryInformation).GetTransferableItem(preWaferIn);

                                        string[] strData = new string[2];
                                        double[] buffer = new double[2];

                                        strData[0] = preWaferIn.ProductType;
                                        strData[1] = preWaferIn.ProberRecipe;

                                        buffer[0] = preWaferIn.TestStageCount;
                                        buffer[1] = preWaferIn.Temperature;
                                        // Fire evnet
                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_QUERY_PRE_WAFER_IN_INFO, buffer, strData);

                                        (echoTSECmd as CmdQueryInformation).SetTransferableItem(preWaferIn);

                                        #endregion
                                    }
                                    break;
                                case ((int)ETransferableCommonObject.PreSOTInformationForProber):
                                    {
#if LD200
                                        #region >>> PreSOTInformationForProber <<<
                                        PreSOTInformationForProber preProberSOT = new PreSOTInformationForProber();
                                        (cmd as CmdQueryInformation).GetTransferableItem(preProberSOT);

                                        this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ReticleX] = Convert.ToDouble(preProberSOT.ReticleX);
                                        this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ReticleY] = Convert.ToDouble(preProberSOT.ReticleY);
                                        this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.DutNumber] = Convert.ToDouble(preProberSOT.DUTNumber);
                                        this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.DutOffset] = preProberSOT.DUTOffset;
                                        this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.DZPosition] = preProberSOT.DZPosition;
                                        this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.PZPosition] = preProberSOT.PZPosition;

                                        this._testerSys.CmdData.StringData[(uint)EProberStrDataIndex.DutID] = preProberSOT.DUTID;

                                        (echoTSECmd as CmdQueryInformation).SetTransferableItem(preProberSOT);
                                        
                                        #endregion
#endif
                                    }
                                    break;

                                case ((int)ETransferableCommonObject.PreOverloadTestInfo):
                                    {
                                        #region >>> PreOverloadTestInfo <<<
                                        Console.WriteLine("[EMPITestServerCmd], ID_QUERY_INFORMATION,PreOverloadTestInfo");
#if PD200

                                        PreOverloadTestInfo preOverload = new PreOverloadTestInfo();
                                        (cmd as CmdQueryInformation).GetTransferableItem(preOverload);

                                        string[] strData = new string[20];
                                        double[] buffer = new double[2];

                                        buffer[0] = (int)preOverload.TestMode;
                                        int refDataCoubt = 0;
                                        int p2trefDataCoubt = 0;
                                        try
                                        {
                                            p2trefDataCoubt = preOverload.Prober2TesterRefTable.Table.Count;
											refDataCoubt = preOverload.RefTable.Table.Count;
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("[EMPITestServerCmd], PreOverloadTestInfo, refTable is count fail");
                                        }

                                        string pointInfo = "";


                                        for (int i = 0; i < 10; ++i)//max get 10 refPoint
                                        {
                                            if (i < p2trefDataCoubt)
                                            {
                                                pointInfo = preOverload.Prober2TesterRefTable.Table[i].BaseX.ToString() + "," +
                                                    preOverload.Prober2TesterRefTable.Table[i].BaseY.ToString() + "," +
                                                     preOverload.Prober2TesterRefTable.Table[i].NewX.ToString() + "," +
                                                      preOverload.Prober2TesterRefTable.Table[i].NewY.ToString() + "," +
                                                       preOverload.Prober2TesterRefTable.Table[i].ChipName.ToString() + "," +
                                                        preOverload.Prober2TesterRefTable.Table[i].Remark.ToString();
                                                Console.WriteLine("[EMPITestServerCmd], PreOverloadTestInfo, Prober2TesterRefTable Info:" + pointInfo);
                                            }
                                            else
                                            {
                                                pointInfo = "";
                                            }

                                            strData[i ] = pointInfo;
                                        }

                                        for (int i = 0; i < 10; ++i)//max get 10 refPoint
                                        {
                                            if (i < refDataCoubt)
                                            {
                                                pointInfo = preOverload.RefTable.Table[i].BaseX.ToString() + "," +
                                                    preOverload.RefTable.Table[i].BaseY.ToString() + "," +
                                                     preOverload.RefTable.Table[i].NewX.ToString() + "," +
                                                      preOverload.RefTable.Table[i].NewY.ToString() + "," +
                                                       preOverload.RefTable.Table[i].ChipName.ToString() + "," +
                                                        preOverload.RefTable.Table[i].Remark.ToString();
                                                Console.WriteLine("[EMPITestServerCmd], PreOverloadTestInfo, RefPoint Info:" + pointInfo);
                                            }
                                            else
                                            {
                                                pointInfo = "";
                                            }

                                            strData[i + 10] = pointInfo;
                                        }

                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_QUERY_PRE_OVERLOAD_TEST_INFO, buffer, strData);                                        

                                        (echoTSECmd as CmdQueryInformation).SetTransferableItem(preOverload); 
#endif
                                        #endregion
                                    }
                                    break;
                                case((int )ETransferableCommonObject.ChuckTemperatureInfo):
                                    {
                                        #region >>> PreOverloadTestInfo <<<
                                        Console.WriteLine("[EMPITestServerCmd], ID_QUERY_INFORMATION,ChuckTemperatureInfo");

                                        
                                        ChuckTemperatureInfo cTempInfo = new ChuckTemperatureInfo();
                                        (cmd as CmdQueryInformation).GetTransferableItem(cTempInfo);

                                        string[] strData = new string[1];
                                        double[] buffer = new double[1];

                                        buffer[0] = cTempInfo.ChuckTemperature;


                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_QUERY_PRE_CHUCK_TEMP_INFO, buffer, strData);

                                        (echoTSECmd as CmdQueryInformation).SetTransferableItem(cTempInfo); 

                                        #endregion
                                    }
                                    break;
                                case ((int)ETransferableCommonObject.CheckLaserPower):
                                    {
                                        #region >>> CheckLaserPower <<<
                                        Console.WriteLine("[EMPITestServerCmd], ID_QUERY_INFORMATION,CheckLaserPower");


                                        CheckLaserPower cTempInfo = new CheckLaserPower();
                                        (cmd as CmdQueryInformation).GetTransferableItem(cTempInfo);

                                        string[] strData = new string[1];
                                        double[] buffer = new double[1];

                                        buffer[0] = cTempInfo.AutoSetLaserPower?1:0;
                                        if (cTempInfo.Remark != null)
                                        { strData[0] = cTempInfo.Remark; }
                                        else { strData[0] = ""; }

                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_QUERY_CHECK_LASER_POWER_INFO, buffer, strData);

                                        cTempInfo.IsPowerCheckPass =  buffer[0] == 100?true:false;

                                        (echoTSECmd as CmdQueryInformation).SetTransferableItem(cTempInfo); 
                                        #endregion
                                    }
                                    break;
                                case -1:
                                    {
                                        ShowErrorMsg((int)EErrorCode.TCPIP2_TransferableItem_Err, "ID_QUERY_INFORMATION ,hash =-1");

                                        echoTSECmd = new CmdQueryInformation();
                                    }
                                    break;
                            }

                            LaserBarProberInfo lbpInfo = new LaserBarProberInfo();
                            SubRecipeInformation srInfo = new SubRecipeInformation();


                            if ((cmd as CmdQueryInformation).GetTransferableItem(lbpInfo))
                            {
                                double[] dData = new double[10];

                                double temperature = lbpInfo.ProbingTemperature;

                                dData[0] = temperature;

                                Console.WriteLine("[TCPTestServer], ID_QUERY_INFORMATION,CMD_LASER_BAR_INFO, ProbingTemperature = " + temperature.ToString("0.00"));

                                this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_LASER_BAR_INFO, dData, null);
                            }
                            else if ((cmd as CmdQueryInformation).GetTransferableItem(srInfo))
                            {
                                double[] dData = new double[3];

                                string[] sData = new string[2];

                                sData[0] = srInfo.MainRecipeName;
                                sData[1] = srInfo.SubRecipeName;

                                dData[0] = srInfo.SlotNumber;
                                dData[1] = srInfo.SubRecipeIndex;
                                dData[2] = srInfo.Temperature;

                                Console.WriteLine("[TCPTestServer], ID_QUERY_INFORMATION,CMD_LASER_BAR_INFO, SubRecipeInformation = " + srInfo.SubRecipeName);

                                this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_SUB_RECIPE_INFO, dData, sData);
                            }


                            #endregion
                            break;
                        }
                    //------------------------------------------------------------------------------------------------
                    default:
                        break;
                }

                if (cmd != null && echoTSECmd != null)
                {
                    this._myClient.SendMessage(echoTSECmd.Packet.Serialize());

                    if (echoTSECmd.CommandID != (int)ETSECommand.ID_EOT)
                    {
                        Console.WriteLine("[TCPTestServer], Echo CMD_ID:" + ((ETSECommand)echoTSECmd.CommandID).ToString());
                    }
                    this._txtLog.Add(string.Format("[RECV] {0}\r\n", cmd.CommandID.ToString()));
                    this._txtLog.Add(string.Format("[SEND] {0}\r\n", echoTSECmd.CommandID.ToString() + "\r\n"));
                    return 1;
                }
            }

            return -1;
        }
        protected virtual bool CheckIfPackageCorrect(byte[] packet, out string errStr)
        {
            errStr = "";
            bool result = _myClient.CheckIfErrBuffer(packet, out errStr);
            return result;
        }

        protected virtual void ShowErrorMsg(double errID, string errMsg)
        {
            double[] dData = new double[] { errID }; ;

            string[] sData = new string[] { errMsg };

            Console.WriteLine("[TCPTestServer], CommandProc()," + errMsg);

            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_SHOW_ERROR, dData, sData);
        }

        private void ThreadProc()
        {
            while (true)
            {
                this._isBusy = true;
                _signal.WaitOne();

                this.CommandProc();
                _buffer = null;

                _signal.Reset();
                this._isBusy = false;
            }
        }

        private void OnTCPIPStateChange(ETCPClientState state)
        {
            if (this.TCPIPStateChangeEvent != null)
            {
                this.TCPIPStateChangeEvent(state);
            }
        }

        #endregion

        #region >>> Public Methods <<<

        public void Open(string serverName, string IPAddress, int port)
        {
            //System.Threading.ThreadPriority.Highest

            this._thrdTCPIP.Name = "TCP_IP Server Thread";

            if (this._thrdTCPIP.ThreadState == ThreadState.Unstarted)
            {
                this._thrdTCPIP.Start();
            }

            this._lastState = ETCPTestSeverState.DISCONNECT;

            this.Connect(IPAddress, port);
        }

        public void Connect(string IPAddress, int port)
        {
            if (this._ipAddress != IPAddress || this._port != port)
            {
                this._ipAddress = IPAddress;

                this._port = port;

                this._myClient.Disconnect();
            }

            if (this._myClient.LastState == ETCPClientState.NONE || this._myClient.LastState == ETCPClientState.ERROR)
            {
                this._myClient.BeginConnect(IPAddress, port);
            }
        }

        public void Close()
        {
            this._myClient.Dispose();
            this._thrdTCPIP.Abort();
        }

        #endregion
    }
}
