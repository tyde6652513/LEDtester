using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using MPI.UCF.ControlServer;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;

namespace MPI.Tester.TestServer
{
    public class TestServer : MPI.UCF.ControlServer.ControlServerBase
    {
        private object _lockObj;

        private bool _isAutoRunStart;
        private bool _isTested;

        private const string ServerName = "TEST_SERVER";
        private const int DataArrayLen = 1024;
        private TesterKernelBase _testerSys;
        private bool _isTestServerOpen;
        private AcquireData _acquireData;

        public event EventHandler<ServerQueryEventArg> ServerQueryEvent;

        private bool _enableProberResetSepctrometer = false;

        /// <summary>
        ///  Constructer, assgin one tester system to tester server
        /// </summary>
        /// <param name="kernel"></param>
        public TestServer()
            : base()
        {
            this._lockObj = new object();

            this._isAutoRunStart = false;
            this._isTested = false;
            //this._isPass = false;
        }

        public TestServer(TesterKernelBase kernel)
            : this()
        {
            this._testerSys = kernel;
            this._isTestServerOpen = false;
        }

        # region >>> Public Proterties <<<

        public bool IsAutoRunStart
        {
            get { return this._isAutoRunStart; }
            set { lock (this._lockObj) { this._isAutoRunStart = value; } }
        }

        #endregion

        #region >>> Private Method <<<

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
                //  ServerQueryEvent(this, theArg); //why do not use this
            }
        }

        private void Push_ChartStrToMem(string str, int doubleArrayIndex, int addressLength = 16)
        {
            byte[] asciiBytes;
            byte[] doubleBytes;

            asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(str));
            doubleBytes = new byte[addressLength * 8];
            if (asciiBytes.Length < doubleBytes.Length)
            {
                Array.Copy(asciiBytes, doubleBytes, asciiBytes.Length);
            }
            else
            {
                Array.Copy(asciiBytes, doubleBytes, doubleBytes.Length);
            }

            for (int i = 0; i < addressLength; i++)
            {
                this._dataMemArray[doubleArrayIndex + i] = BitConverter.ToDouble(doubleBytes, i * 8);
            }

        }

        #endregion

        #region >>> Protected Methods <<<

        protected override int CommandProc(int command, object o)
        {
            if (!this._isTestServerOpen)
                return command;

            int rtnCode = command;
            int dataStart = 0;

            this._acquireData = this._testerSys.Data;

            //if (this._testerSys.Status.State == EKernelState.Not_Ready || this._testerSys.Status.State == EKernelState.Error )
            //{
            //    return (int)EMPITestServerCmdResult.Err_CommandUndefined;
            //}

            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
            this._dataMemArray[(int)EMPITestServerDataIndex.REOT] = (int)EMPIDefine.Deactive;
            this._dataMemArray[(int)EMPITestServerDataIndex.EOT] = (int)EMPIDefine.Deactive;
            // paul remove_0721
            // this._dataMemArray[(int)EMPITestServerDataIndex.PF0] = (int)EMPIDefine.Fail;

            if (this._testerSys == null)
            {
                return (int)EMPITestServerCmdResult.Err_CommandUndefined;
            }

            switch (command)
            {
                case (int)EMPITestServerCmd.TS_CMD_TEST_START:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_TEST_START");

                        double[] buffer = new double[46];
                        buffer[0] = this._dataMemArray[(int)EMPITestServerDataIndex.ColMinimum]; //ColX min
                        buffer[1] = this._dataMemArray[(int)EMPITestServerDataIndex.RowMinimum]; //RowY min
                        buffer[2] = this._dataMemArray[(int)EMPITestServerDataIndex.ColMaximum]; //ColX max
                        buffer[3] = this._dataMemArray[(int)EMPITestServerDataIndex.RowMaximum]; //RowY max
                        buffer[4] = this._dataMemArray[(int)EMPITestServerDataIndex.TotalSacnCounts];
                        buffer[5] = this._dataMemArray[(int)EMPITestServerDataIndex.ChipXPictch];
                        buffer[6] = this._dataMemArray[(int)EMPITestServerDataIndex.ChipYPictch];
                        buffer[7] = this._dataMemArray[(int)EMPITestServerDataIndex.TotalProbingCounts];
                        buffer[8] = this._dataMemArray[(int)EMPITestServerDataIndex.MoveMainAxis];
                        buffer[9] = this._dataMemArray[(int)EMPITestServerDataIndex.SamplingMode];
                        buffer[10] = this._dataMemArray[(int)EMPITestServerDataIndex.XInitDirection];
                        buffer[11] = this._dataMemArray[(int)EMPITestServerDataIndex.YInitDirection];
                        buffer[12] = this._dataMemArray[(int)EMPITestServerDataIndex.ProbingCount1];
                        buffer[13] = this._dataMemArray[(int)EMPITestServerDataIndex.ProbingCount2];
                        buffer[14] = this._dataMemArray[(int)EMPITestServerDataIndex.ProbingCount3];
                        buffer[15] = this._dataMemArray[(int)EMPITestServerDataIndex.ProbingCount4];
                        buffer[16] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_ColX];   // Roy, Multi-Die Testing
                        buffer[17] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_RowY];
                        buffer[18] = this._dataMemArray[(int)EMPITestServerDataIndex.RefColumn];
                        buffer[19] = this._dataMemArray[(int)EMPITestServerDataIndex.RefRow];
                        buffer[20] = this._dataMemArray[(int)EMPITestServerDataIndex.StartTemp];
                        buffer[21] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_Rotation];
                        buffer[22] = this._dataMemArray[(int)EMPITestServerDataIndex.XPitch1];
                        buffer[23] = this._dataMemArray[(int)EMPITestServerDataIndex.YPitch1];
                        buffer[24] = this._dataMemArray[(int)EMPITestServerDataIndex.XPitch2];
                        buffer[25] = this._dataMemArray[(int)EMPITestServerDataIndex.YPitch2];
                        buffer[26] = this._dataMemArray[(int)EMPITestServerDataIndex.XPitch3];
                        buffer[27] = this._dataMemArray[(int)EMPITestServerDataIndex.YPitch3];
                        buffer[28] = this._dataMemArray[(int)EMPITestServerDataIndex.XPitch4];
                        buffer[29] = this._dataMemArray[(int)EMPITestServerDataIndex.YPitch4];
                        buffer[30] = this._dataMemArray[(int)EMPITestServerDataIndex.XPitch5];
                        buffer[31] = this._dataMemArray[(int)EMPITestServerDataIndex.YPitch5];
                        buffer[32] = this._dataMemArray[(int)EMPITestServerDataIndex.SamplingDiePitchCol];
                        buffer[33] = this._dataMemArray[(int)EMPITestServerDataIndex.SamplingDiePitchRow];
                        buffer[34] = this._dataMemArray[(int)EMPITestServerDataIndex.XLineSubBinSampleCH];
                        buffer[35] = this._dataMemArray[(int)EMPITestServerDataIndex.YLineSubBinSampleCH];

                        buffer[36] = this._dataMemArray[(int)EMPITestServerDataIndex.ES01_START_COUNT];
                        buffer[37] = this._dataMemArray[(int)EMPITestServerDataIndex.ES02_START_COUNT];
                        buffer[38] = this._dataMemArray[(int)EMPITestServerDataIndex.ES03_START_COUNT];
                        buffer[39] = this._dataMemArray[(int)EMPITestServerDataIndex.ES04_START_COUNT];
                        buffer[40] = this._dataMemArray[(int)EMPITestServerDataIndex.ES05_START_COUNT];
                        buffer[41] = this._dataMemArray[(int)EMPITestServerDataIndex.ES06_START_COUNT];
                        buffer[42] = this._dataMemArray[(int)EMPITestServerDataIndex.ES07_START_COUNT];
                        buffer[43] = this._dataMemArray[(int)EMPITestServerDataIndex.ES08_START_COUNT];

                        buffer[44] = this._dataMemArray[(int)EMPITestServerDataIndex.IsSingleProbingInMultiDie];//AOI//////////////////////////////////////////////

                        StringBuilder sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.ProberRecipeFileName;
                        byte[] dataByte = null;
                        string[] strData = new string[1];
                        for (int index = dataStart; index < (dataStart + 16); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[0] = sb.ToString().Trim('\0');
                        Console.WriteLine("[EMPITestServerCmd], ProbRecipeName = {0}", strData[0]);


                        Console.WriteLine("[EMPITestServerCmd], ProbeChannel_ColX = {0}, ProbeChannel_RowY = {1}", buffer[16], buffer[17]);

                        GlobalFlag.IsContinueMode = false;
                        GlobalFlag.IsSuccessCheckFilterWheel = true;

                        //GlobalFlag.IsSuccessCheckChannelConfig = false;  // Roy, AppSystem has already reseted this flag

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_TESTER_START, buffer, strData);

                        this._isAutoRunStart = true;
                        // Gilbert
                        // Running status start
                        if (this._testerSys.Status.State == EKernelState.Not_Ready || this._testerSys.Status.State == EKernelState.Error)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_TESTER_IS_NOT_READY;

                            if (GlobalFlag.IsSuccessCheckFilterWheel == false)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR");
                            }

                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_TESTER_IS_NOT_READY");
                        }
                        else if (GlobalFlag.IsSuccessCheckFilterWheel == false)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_TESTER_IS_NOT_READY;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR");
                        }
                        else if (buffer[16] * buffer[17] > 1 && GlobalFlag.IsSuccessCheckChannelConfig == false)  // Prober Total Channel > 1 且 CheckChannelConfig == false
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_MULTIDIE_SETTING_NOT_READY;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_MULTIDIE_SETTING_NOT_READY");
                        }
                        else
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                        }
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_TEST_END:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_TEST_END");

                        double[] buffer2 = new double[9];
                        buffer2[0] = this._dataMemArray[(int)EMPITestServerDataIndex.EndTemp]; //ColX min
                        buffer2[1] = this._dataMemArray[(int)EMPITestServerDataIndex.ES01_END_COUNT];
                        buffer2[2] = this._dataMemArray[(int)EMPITestServerDataIndex.ES02_END_COUNT];
                        buffer2[3] = this._dataMemArray[(int)EMPITestServerDataIndex.ES03_END_COUNT];
                        buffer2[4] = this._dataMemArray[(int)EMPITestServerDataIndex.ES04_END_COUNT];
                        buffer2[5] = this._dataMemArray[(int)EMPITestServerDataIndex.ES05_END_COUNT];
                        buffer2[6] = this._dataMemArray[(int)EMPITestServerDataIndex.ES06_END_COUNT];
                        buffer2[7] = this._dataMemArray[(int)EMPITestServerDataIndex.ES07_END_COUNT];
                        buffer2[8] = this._dataMemArray[(int)EMPITestServerDataIndex.ES08_END_COUNT];

                        this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.EndTest;
                        this._testerSys.RunCommand((int)ETesterKernelCmd.EndTest);



                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_TESTER_END, buffer2, null);
                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                        Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_TEST_ABORT:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_TEST_ABORT");

                        double[] buffer = new double[1];

                        buffer[0] = this._dataMemArray[(int)EMPITestServerDataIndex.AbortIsSaveFile]; //0: Abort, 1: SaveFile

                        this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.StopTest;
                        this._testerSys.RunCommand((int)ETesterKernelCmd.StopTest);

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_TESTER_ABORT, buffer, null);

                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                        Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_GET_ITEM_NAME:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_GET_ITEM_NAME");
                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;

                        //if ( this._acquireData != null && this._acquireData.OutputTestResult != null )
                        //{
                        //    int index = 0;
                        //    for (int i = (int)EMPITestServerDataIndex.ResultItemName; i < 100; i++)
                        //    {
                        //        this._dataMemArray[i] = 0.0d;
                        //    }

                        //    for (int k = 6; k < this._acquireData.OutputTestResult.Length && k < 50; k++ )
                        //    {
                        //        if (this._acquireData.OutputTestResult[k].IsEnable)
                        //        {
                        //            this.Push_16ChartStrToMem(this._acquireData.OutputTestResult[k].Name, (int)EMPITestServerDataIndex.ResultItemName + index * 2);
                        //            index++;
                        //        }
                        //    }
                        //    this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                        //}
                        //else
                        //{
                        //    this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_TESTER_GET_ITEM_NAME_ERROR;
                        //}
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_SOT:
                    {
                        if (this._isAutoRunStart)
                        {
                            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.RunTest;
                            this._testerSys.CmdData.IntData[0] = 1;
                            this._testerSys.CmdData.IntData[1] = 0;
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.COL] = this._dataMemArray[(int)EMPITestServerDataIndex.Address_Col];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ROW] = this._dataMemArray[(int)EMPITestServerDataIndex.Address_Row];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.CHUCKX] = this._dataMemArray[(int)EMPITestServerDataIndex.ChuckX];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.CHUCKY] = this._dataMemArray[(int)EMPITestServerDataIndex.ChuckY];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.CHUCKZ] = this._dataMemArray[(int)EMPITestServerDataIndex.ChuckZ];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ES01] = this._dataMemArray[(int)EMPITestServerDataIndex.ES01];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ES02] = this._dataMemArray[(int)EMPITestServerDataIndex.ES02];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ES03] = this._dataMemArray[(int)EMPITestServerDataIndex.ES03];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ES04] = this._dataMemArray[(int)EMPITestServerDataIndex.ES04];

                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.MLIResult] = this._dataMemArray[(int)EMPITestServerDataIndex.MLIResult];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.TestChipGroup] = this._dataMemArray[(int)EMPITestServerDataIndex.TestChipGroup];
                            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.PROBE_INDEX] = 1.0d;

                            //this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.IsSingleProbingInMultiDie]  = this._dataMemArray[(int)EMPITestServerDataIndex.IsSingleProbingInMultiDie];
                            //this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.TestGroupIndex] = this._dataMemArray[(int)EMPITestServerDataIndex.TestGroupIndex];//AOI
                            //this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.TestGroupStr] = this._dataMemArray[(int)EMPITestServerDataIndex.TestGroupStr];//AOI
                            //TestGroupIndex

                            int index = 0;

                            byte[] dataByte = null;
                            string strData = "";

                            if (GlobalFlag.IsSuccessCheckChannelConfig)
                            {
                                // 取得 Prober 資訊, Channel 上有無 Die
                                int dataLength = 16;
                                dataStart = (int)EMPITestServerDataIndex.ProberChannelStatus;
                                index = 0;
                                dataByte = null;
                                strData = "";

                                for (index = dataStart; index < (dataStart + dataLength); index++)
                                {
                                    dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                                    strData += Encoding.ASCII.GetString(dataByte);
                                }

                                this._testerSys.CmdData.StringData[0] = strData;

                                // 取得 Prober 資訊, Channel 上 SubBin
                                dataLength = 24;
                                dataStart = (int)EMPITestServerDataIndex.SubBin;
                                strData = "";

                                for (index = dataStart; index < (dataStart + dataLength); index++)
                                {
                                    dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                                    strData += Encoding.ASCII.GetString(dataByte);
                                }

                                int endPoint = strData.IndexOf('\0');

                                if (endPoint > 0)
                                {
                                    strData = strData.Remove(endPoint);
                                }

                                this._testerSys.CmdData.StringData[1] = strData;

                                // 取得 Prober 資訊, Channel 上 ProbeBin
                                dataLength = 24;
                                dataStart = (int)EMPITestServerDataIndex.ProberBin;
                                strData = "";

                                for (index = dataStart; index < (dataStart + dataLength); index++)
                                {
                                    dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                                    strData += Encoding.ASCII.GetString(dataByte);
                                }

                                endPoint = strData.IndexOf('\0');

                                if (endPoint > 0)
                                {
                                    strData = strData.Remove(endPoint);
                                }

                                this._testerSys.CmdData.StringData[2] = strData;
                            }

                            //this._isTested = this._testerSys.RunTestSequence();
                            this._isTested = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);

                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                            this._dataMemArray[(int)EMPITestServerDataIndex.REOT] = (int)EMPIDefine.Active;
                            this._dataMemArray[(int)EMPITestServerDataIndex.EOT] = (int)EMPIDefine.Deactive;
                            GlobalFlag.SeqStep = 1014;
                        }
                        else
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPIDefine.Deactive;
                        }
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_CALC:
                    {
                        if (this._isTested)
                        {
                            GlobalFlag.IsDataRecoveryPushSuccess = true;
                            GlobalFlag.IsPassRateCheckSuccess = true;
                            //-----------------------------------------------------------
                            // (1) Transfer UI Operation State
                            //----------------------------------------------------------
                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_CALC, null, null);

                            //----------------------------------------------------------
                            // (2) Get Data from Tester
                            //----------------------------------------------------------
                            this._testerSys.ResetTesterCond();								// First Step, for K2400
                            this._testerSys.GetTestedDataFromDevice();

                            //----------------------------------------------------------
                            // Gilbert, 20121223
                            // DieTestState 不為 0, 表示有異常狀況,在點測第二次測試
                            //----------------------------------------------------------
                            //if (this._testerSys.Data.IR[(int)EDataIR.DieTestState] != 0)
                            //{
                            //    System.Threading.Thread.Sleep(50);
                            //    this._isTested = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);
                            //    this._testerSys.GetTestedDataFromDevice();
                            //}

                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                            this._dataMemArray[(int)EMPITestServerDataIndex.REOT] = (int)EMPIDefine.Active;
                            this._dataMemArray[(int)EMPITestServerDataIndex.EOT] = (int)EMPIDefine.Active;

                            if (GlobalFlag.IsSuccessCheckChannelConfig)
                            {
                                // Multi-Die-Die Testing, Return P/F for each Channel
                                string strPF = string.Empty;

                                for (uint channel = 0; channel < this._acquireData.ChannelResultDataSet.Count; channel++)
                                {
                                    if (this._acquireData.ChannelResultDataSet[channel].IsPass)
                                    {
                                        strPF += "1";

                                        if (channel == 0)
                                        {
                                            // for 五點測高
                                            this._dataMemArray[(int)EMPITestServerDataIndex.PF0] = (int)EMPIDefine.Pass;
                                        }
                                    }
                                    else
                                    {
                                        strPF += "0";

                                        if (channel == 0)
                                        {
                                            // for 五點測高
                                            this._dataMemArray[(int)EMPITestServerDataIndex.PF0] = (int)EMPIDefine.Fail;
                                        }
                                    }
                                }

                                //  Console.WriteLine("[EMPITestServerCmd], TS_CMD_CALC" + strPF);

                                this.Push_ChartStrToMem(strPF, (int)EMPITestServerDataIndex.ChannelResultPF);
                            }
                            else
                            {
                                // Single-Die Testing, Return P/F
                                if (this._acquireData.ChipInfo.IsPass)
                                {
                                    //	1 : Good  " minValue < value < maxValue "
                                    this._dataMemArray[(int)EMPITestServerDataIndex.PF0] = (int)EMPIDefine.Pass;
                                }
                                else
                                {
                                    //	2 : Fail 
                                    this._dataMemArray[(int)EMPITestServerDataIndex.PF0] = (int)EMPIDefine.Fail;
                                }
                            }


                            // Run Y axis Adjacent Error Check
                            if (this._testerSys.Data.ChipInfo.AdjacentResult == EAdjacentResult.RETEST)		//	2 : FAIL 
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_EOT_ADJACENT_ERROR;
                                this._dataMemArray[(int)EMPITestServerDataIndex.ReTestColX] = this._testerSys.Data.ChipInfo.ReTestColX;
                                this._dataMemArray[(int)EMPITestServerDataIndex.ReTestRowY] = this._testerSys.Data.ChipInfo.ReTestRowY;
                                Console.WriteLine("[EMPITestServerCmd], EOT_ERROR, Adjacent_Error Prober Run needle Clean");
                                Console.WriteLine("[EMPITestServerCmd], EOT_ERROR, ReTest ColX/RowY : "
                                                                 + this._testerSys.Data.ChipInfo.ReTestColX.ToString() + "," +
                                                                 this._testerSys.Data.ChipInfo.ReTestRowY.ToString());
                            }
                            else if (this._testerSys.Data.ChipInfo.AdjacentResult == EAdjacentResult.ERROR)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_EOT_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EOT_ERROR, Adjacent_Error ReTest Error Prober Stop");
                            }
                            else if (GlobalFlag.IsSourceMeterDisconnect)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_EOT_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EOT_ERROR, Source Meter Disconnectr Prober Stop");
                            }
                            else if (!GlobalFlag.IsPassRateCheckSuccess)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_EOT_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EOT_ERROR, Pass Rate Check Fail Prober Stop");
                            }

                            if (!GlobalFlag.IsPreSamplingCheckSuccess)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_EOT_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EOT_ERROR, Pre Sampling Check Fail, Prober Stip");
                            }

                            //if (!GlobalFlag.IsDataRecoveryPushSuccess)
                            //{
                            //    this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_EOT_ERROR;
                            //    Console.WriteLine("[EMPITestServerCmd], EOT_ERROR, Data Recovery Push Data Fail");
                            //}

                            GlobalFlag.SeqStep = 1017;
                        }
                        else
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_EOT_ERROR;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_EOT_ERROR");
                        }

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_SEND_BARCODE:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_SEND_BARCODE");

                        int dataLength = 16;
                        dataStart = (int)EMPITestServerDataIndex.BarCodeFileName;
                        int index = 0;
                        StringBuilder sb;

                        byte[] dataByte = null;
                        string[] strData = new string[11];

                        //Send Base Row Col
                        this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.TransCOL] = this._dataMemArray[(int)EMPITestServerDataIndex.CoordTransColX];
                        this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.TransROW] = this._dataMemArray[(int)EMPITestServerDataIndex.CoordTransRowY];

                        double[] buffer = new double[2];//20180413 David
                        buffer[0] = this._dataMemArray[(int)EMPITestServerDataIndex.IsPreSampling];
                        buffer[1] = this._dataMemArray[(int)EMPITestServerDataIndex.TestTemperature];

                        //buffer[1] = this._dataMemArray[(int)EMPITestServerDataIndex.IsSingleProbingInMultiDie];//AOI//////////////////////////////////////////////

                        // Get "Barcode" information from Prober 
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.BarCodeFileName;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[0] = sb.ToString().Trim('\0');

                        // Get "Lot number" from Prober
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.LotNumber;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[1] = sb.ToString().Trim('\0');

                        // Get "Cassette number" from Prober
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.CassetteNumer;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[2] = sb.ToString().Trim('\0');

                        // Get "Wafer number" from Prober
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.WaferNumber;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[3] = sb.ToString().Trim('\0');

                        // Get "Operator name" from Prober
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.OperatorName;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[4] = sb.ToString().Trim('\0');

                        // Get "ProberRecipe" Name // Paul add
                        //sb = new StringBuilder();
                        //dataStart = (int)EMPITestServerDataIndex.ProberRecipeFileName;
                        //for (index = dataStart; index < (dataStart + dataLength); index++)
                        //{
                        //    dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                        //    sb.Append(Encoding.ASCII.GetString(dataByte));
                        //}
                        //strData[5] = sb.ToString().Trim('\0');

                        // Get "ProductType"
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.ProductType;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[6] = sb.ToString().Trim('\0');

                        // Get "Substrate"
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.Substrate;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[7] = sb.ToString().Trim('\0');


                        // Get "CassetteID"
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.CassetteID;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[8] = sb.ToString().Trim('\0');

                        // Get "SoltNumber"
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.SoltNumber;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[9] = sb.ToString().Trim('\0');

                        // Get "CustomerKeyNumber"
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.CustomerKeyNumber;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[10] = sb.ToString().Trim('\0');


                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_OPEN_OUTPUT_FILE, buffer, strData);

                        if (GlobalFlag.IsSuccessLoadMESData == false)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_AUTO_LOAD_RECIPE_ERROR;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_REMOTE_LOAD_RECIPE_ERROR");
                        }
                        else if (GlobalFlag.IsDailyCheckFail == true)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_NO_RUN_DAILY_CHECK;

                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_NO_RUN_DAILY_CHECK");
                        }
                        else if (GlobalFlag.OutputReportState == EOutputReportState.FileNameIsEmpty)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_OUTPUT_FILENAME_EMPTY;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_OUTPUT_FILENAME_EMPTY");
                        }
                        else if (GlobalFlag.OutputReportState == EOutputReportState.CanOverwrite)
                        {
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_OUTPUT_FILENAME_EXIST_FORMAT_ERR");

                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_OUTPUT_FILENAME_EXIST_FORMAT_ERR;
                        }
                        else if (GlobalFlag.OutputReportState == EOutputReportState.CanAppend)
                        {
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_OUTPUT_FILENAME_EXIST");

                            this._dataMemArray[(int)EMPITestServerDataIndex.ReTestColX] = GlobalData.ContinueModeCol;

                            this._dataMemArray[(int)EMPITestServerDataIndex.ReTestRowY] = GlobalData.ContinueModeRow;

                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_OUTPUT_FILENAME_EXIST;
                        }
                        else if (GlobalFlag.IsOperatorEmpty)
                        {
                            Console.WriteLine("[EMPITestServerCmd], Operator Is Empty");

                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_TESTER_IS_NOT_READY;
                        }
                        else
                        {
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");

                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                        }

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_SEND_TEST_FILE_NAMES:
                    {
                        {
                            Console.WriteLine("[EMPITestServerCmd], TS_CMD_SEND_TEST_FILE_NAMES");
                            int dataLength = 16;
                            dataStart = (int)EMPITestServerDataIndex.CondFileName;
                            int index = 0;
                            StringBuilder sb;

                            byte[] dataByte;
                            string[] strData = new string[2];

                            GlobalFlag.IsSuccessCheckFilterWheel = true;
                            GlobalFlag.IsSuccessLoadProduct = false;
                            GlobalFlag.IsSuccessLoadBin = false;

                            // Get "Condition file name " from Prober 
                            sb = new StringBuilder();
                            dataStart = (int)EMPITestServerDataIndex.CondFileName;
                            for (index = dataStart; index < (dataStart + dataLength); index++)
                            {
                                dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                                sb.Append(Encoding.ASCII.GetString(dataByte));
                            }
                            strData[0] = sb.ToString().Trim('\0');

                            // Get "Bin file name " from Prober 
                            sb = new StringBuilder();
                            dataStart = (int)EMPITestServerDataIndex.BinFileName;
                            for (index = dataStart; index < (dataStart + dataLength); index++)
                            {
                                dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                                sb.Append(Encoding.ASCII.GetString(dataByte));
                            }
                            strData[1] = sb.ToString().Trim('\0');

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_LOAD_ITEM_FILE, null, strData);

                            if (GlobalFlag.IsSuccessCheckFilterWheel == false)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR");
                            }
                            else if (GlobalFlag.IsSuccessLoadProduct == true && GlobalFlag.IsSuccessLoadBin == true)
                            {
                                //this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPIDefine.Active;
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                                Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                            }
                            else
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_LOAD_RECIPE_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_LOAD_RECIPE_ERROR");
                            }

                        }
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_REWRITE_TESTER_OUTPUT_FILE:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_REWRITE_TESTER_OUTPUT_FILE");

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_OVERWRITE_TESTER_OUTPUT_FILE, null, null);

                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;

                        Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_APPEND_TESTER_OUTPUT_FILE:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_APPEND_TESTER_OUTPUT_FILE");

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_APPEND_TESTER_OUTPUT_FILE, null, null);

                        if (GlobalFlag.OutputReportState == EOutputReportState.None)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;

                            this._dataMemArray[(int)EMPITestServerDataIndex.ReTestColX] = GlobalData.ContinueModeCol;

                            this._dataMemArray[(int)EMPITestServerDataIndex.ReTestRowY] = GlobalData.ContinueModeRow;

                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                        }
                        else
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_APPEND_OUTPUT_FILENAME_FAIL;

                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_APPEND_OUTPUT_FILENAME_FAIL");
                        }
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_SHORT_TEST:
                    {
                        this._testerSys.RunCommand((int)ETesterKernelCmd.ShortTestIF);
                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = Convert.ToInt32(this._testerSys.Data.ChipInfo.IsSortTestOK);
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_OPEN_TEST:
                    {
                        this._testerSys.RunCommand((int)ETesterKernelCmd.OpenTestIF);
                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = Convert.ToInt32(this._testerSys.Data.ChipInfo.IsOpenTestOK);
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_ABORT_OPEN_SHORT_TEST:
                    {
                        this._testerSys.RunCommand((int)ETesterKernelCmd.AbortOpenShortTestIF);
                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPIDefine.Active;
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_SHOW_TESTERUI:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_SHOW_TESTERUI");
                        double[] UIParameter = new double[1];
                        UIParameter[0] = this._dataMemArray[(int)EMPITestServerDataIndex.UI_Display_ID];

                        int dataLength1 = 16;
                        int index1 = 0;
                        StringBuilder sb1 = new StringBuilder();

                        byte[] dataByte1 = null;
                        string[] strData1 = new string[1];

                        dataStart = (int)EMPITestServerDataIndex.ProberRecipeFileName;
                        for (index1 = dataStart; index1 < (dataStart + dataLength1); index1++)
                        {
                            dataByte1 = BitConverter.GetBytes(this._dataMemArray[index1]);
                            sb1.Append(Encoding.ASCII.GetString(dataByte1));
                        }
                        strData1[0] = sb1.ToString().Trim('\0');
                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_UPDATE_UI_FORM, UIParameter, strData1);
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_GET_BIN_RESULT:
                    {
                        this._dataMemArray[(int)EMPITestServerDataIndex.BIN_NUM] = this._testerSys.Data.ChipInfo.BinGrade;
                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_PRINT_BARCODE:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_PRINT_BARCODE");

                        GlobalFlag.IsSuccessLoadBin = false;
                        string[] outTubeData = new string[6];
                        outTubeData[0] = this._dataMemArray[(int)EMPITestServerDataIndex.OUT_TUBE_NUM].ToString();
                        outTubeData[1] = this._dataMemArray[(int)EMPITestServerDataIndex.OUT_TUBE_BIN_NUM].ToString();
                        outTubeData[2] = this._dataMemArray[(int)EMPITestServerDataIndex.OUT_TUBE_COUNT].ToString();
                        outTubeData[3] = "";
                        outTubeData[4] = "";
                        outTubeData[5] = this._dataMemArray[(int)EMPITestServerDataIndex.OUT_TUBE_PULL_NUM].ToString();

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_PRINT_BARCODE, null, outTubeData);

                        if (GlobalFlag.IsFinishPrintBarcode)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                        }
                        else
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_REWRITE_OUTPUT_FILENAME_FAIL;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_REWRITE_OUTPUT_FILENAME_FAIL");
                        }

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_REQUEST_RECIPE:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_REQUEST_RECIPE");
                        int dataLength = 16;
                        dataStart = (int)EMPITestServerDataIndex.CondFileName;
                        int index = 0;
                        StringBuilder sb;

                        byte[] dataByte;
                        string[] strData = new string[3];

                        GlobalFlag.IsSuccessCheckFilterWheel = true;
                        GlobalFlag.IsSuccessLoadProduct = false;
                        GlobalFlag.IsSuccessLoadBin = false;
                        GlobalData.ProberRecipeName = "";

                        // Get "OperatorName" information from Prober 
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.OperatorName;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[0] = sb.ToString().Trim('\0');

                        // Get "Barcode" information from Prober 
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.BarCodeFileName;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[1] = sb.ToString().Trim('\0');

                        // Get "Lot number" from Prober
                        sb = new StringBuilder();
                        dataStart = (int)EMPITestServerDataIndex.LotNumber;
                        for (index = dataStart; index < (dataStart + dataLength); index++)
                        {
                            dataByte = BitConverter.GetBytes(this._dataMemArray[index]);
                            sb.Append(Encoding.ASCII.GetString(dataByte));
                        }
                        strData[2] = sb.ToString().Trim('\0');

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_LOAD_RECIPE_FROM_PROBER, null, strData);

                        if (GlobalFlag.IsSuccessCheckFilterWheel == false)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR");
                        }
                        else if (GlobalFlag.IsSuccessLoadProduct == true && GlobalFlag.IsSuccessLoadBin == true && GlobalData.ProberRecipeName != "")
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                            this.Push_ChartStrToMem(GlobalData.ProberRecipeName, (int)EMPITestServerDataIndex.ProberRecipeFileName);
                            Console.WriteLine("[EMPITestServerCmd], RETURN PROBER RECIPE : " + GlobalData.ProberRecipeName);
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                        }
                        else
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_LOAD_RECIPE_ERROR;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_LOAD_RECIPE_ERROR");
                        }
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_REQUEST_PRODUCTION_INFO:
                    {
                        Console.WriteLine("[EMPITestServerCmd], TS_CMD_REQUEST_PRODUCTION_INFO");

                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_SEND_BARCODE_TO_PROBER);

                        if (GlobalFlag.IsSuccessCheckFilterWheel == false)
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_FILTER_WHEEL_ERROR");
                        }
                        else if (GlobalFlag.IsSuccessLoadProduct == true && GlobalFlag.IsSuccessLoadBin == true && GlobalData.ProberRecipeName != "")
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;

                            if (GlobalFlag.IsEnableSendBarcodeToProbe)
                            {
                                this.Push_ChartStrToMem(GlobalData.ProberRecipeName, (int)EMPITestServerDataIndex.ProberRecipeFileName);
                                this.Push_ChartStrToMem(GlobalData.ToProbeWaferNumber, (int)EMPITestServerDataIndex.WaferNumber);
                                this.Push_ChartStrToMem(GlobalData.ToProbeLotNumber, (int)EMPITestServerDataIndex.LotNumber);
                                this.Push_ChartStrToMem(GlobalData.ToProbeBarcode, (int)EMPITestServerDataIndex.BarCodeFileName);
                                this.Push_ChartStrToMem(GlobalData.ToProbeOperator, (int)EMPITestServerDataIndex.OperatorName);
                            }
                            Console.WriteLine("[EMPITestServerCmd], IsEnableSendBarcodeToProbe : " + GlobalFlag.IsEnableSendBarcodeToProbe);
                            Console.WriteLine("[EMPITestServerCmd], RETURN PROBER RECIPE : " + GlobalData.ProberRecipeName);
                            Console.WriteLine("[EMPITestServerCmd], WAFER ID : " + GlobalData.ToProbeWaferNumber);
                            Console.WriteLine("[EMPITestServerCmd], LOT ID : " + GlobalData.ToProbeLotNumber);
                            Console.WriteLine("[EMPITestServerCmd], BARCODE : " + GlobalData.ToProbeBarcode);
                            Console.WriteLine("[EMPITestServerCmd], OPERATOR : " + GlobalData.ToProbeOperator);
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                        }
                        else
                        {
                            this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_LOAD_RECIPE_ERROR;
                            Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_LOAD_RECIPE_ERROR");
                        }
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_NONE:
                    {
                        break;      // Prober time out error,
                    }
                //-------------------------------------------------------------------------------------------------------------
                case (int)EMPITestServerCmd.TS_CMD_CHECK_RCONTACT_STATE_START:
                case (int)EMPITestServerCmd.TS_CMD_CHECK_RCONTACT_STATE_END:
                    {
                        {
                            Console.WriteLine("[EMPITestServerCmd], TS_CMD_CHECK_RCONTACT_STATE");

                            GlobalFlag.IsSuccessLoadProduct = false;

                            GlobalFlag.IsSuccessLoadBin = false;

                            double[] dData = new double[2];

                            string[] strData = new string[3] { "RContact", "RContact", command.ToString() };

                            dData[0] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_ColX];   // Roy, Multi-Die Testing
                            dData[1] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_RowY];

                            _isAutoRunStart = true;

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_CHECK_RCONTACT_STATE, dData, strData);

                            if (GlobalFlag.IsSuccessLoadProduct == true && GlobalFlag.IsSuccessLoadBin == true)
                            {
                                //this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPIDefine.Active;
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                                Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.SUCCESS");
                            }
                            else if (GlobalFlag.IsSuccessCheckChannelConfig == false)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_RCONTACT_RECIPE_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.IsSuccessCheckChannelConfig FAIL");
                            }
                            else
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_RCONTACT_RECIPE_ERROR;
                                Console.WriteLine("[EMPITestServerCmd], EMPITestServerErrCode.PB_ERROR_LOAD_RECIPE_ERROR");
                            }

                            Console.WriteLine("[EMPITestServerCmd], TS_CMD_CHECK_RCONTACT_STATE.SUCCESS");
                        }
                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------   
                case (int)EMPITestServerCmd.TS_CMD_AUTO_CH_CALIB_START:
                    {
                        {
                            _isAutoRunStart = true;

                            double[] dData = new double[2];

                            dData[0] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_ColX];   // Roy, Multi-Die Testing
                            dData[1] = this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_RowY];

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_AUTO_CH_CALIB_START, dData, null);

                            if (GlobalFlag.IsSuccessCheckFilterWheel == false)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_CH_CALIB_START_ERROR;
                            }
                            else if (this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_ColX] * this._dataMemArray[(int)EMPITestServerDataIndex.ProberChannel_RowY] > 1
                                && GlobalFlag.IsSuccessCheckChannelConfig == false)
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.PB_ERROR_CH_CALIB_START_ERROR;
                            }
                            else
                            {
                                this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                            }
                        }
                        break;      // Prober time out error,
                    }
                //-------------------------------------------------------------------------------------------------------------   
                case (int)EMPITestServerCmd.TS_CMD_AUTO_CH_CALIB_END:
                    {
                        this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_AUTO_CH_CALIB_END, null, null);

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------   
                case (int)EMPITestServerCmd.TS_CMD_CHECK_SPECTROMETER_STATE:
                    {
                        if (_enableProberResetSepctrometer)
                        {
                            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.ResetMachineHW;
                            this._testerSys.RunCommand((int)ETesterKernelCmd.ResetMachineHW);
                        }
                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPITestServerErrCode.SUCCESS;
                        break;      // Prober time out error,
                    }
                //-------------------------------------------------------------------------------------------------------------
                default:
                    {
                        this._dataMemArray[(int)EMPITestServerDataIndex.RESULT] = (int)EMPIDefine.Deactive;
                        break;
                    }
            }

            return rtnCode;
        }

        #endregion

        #region >>> Public Methods <<<

        public bool Open()
        {
            if (this._isTestServerOpen == false)
            {
                try
                {
                    base.Open(ServerName, System.Threading.ThreadPriority.Highest, DataArrayLen);
                }
                catch
                {
                    return false;
                }
            }

            this._isTestServerOpen = true;

            return true;

        }

        public override void Close()
        {
            if (this._isTestServerOpen)
            {
                base.Close();
            }

            this._isAutoRunStart = false;

            this._isTestServerOpen = false;
        }

        #endregion

    }

}
