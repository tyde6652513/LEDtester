using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.Drawing;

using MPI.RemoteControl2.Tester;
using MPI.RemoteControl2.Tester.Mpi.Command;
using MPI.RemoteControl2.Tester.Mpi.Command.Base;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;

namespace MPI.Tester.TestServer
{
    public class TCPTestServer2 : TCPTestServer
    {
        private MPI.RemoteControl2.Tester.TesterCommandAgent _cmdAgent2;

        bool _isMultiDie = false;
        private readonly object _lockObj;

        List<string> enableKeyList = new List<string>();

        #region

        public TCPTestServer2(TesterKernelBase kernel ,bool IsTCPIPSendEnableResultItem, bool isEnableREOT = false)
            : base(kernel, IsTCPIPSendEnableResultItem, isEnableREOT)
        {
            List<int> cmdIDList = new List<int>();
            foreach (int id in Enum.GetValues(typeof(ETSECommand)))//取得所有命令的ID
            {
                cmdIDList.Add(id);
            }
            this._myClient = new ClientRole(cmdIDList);

            _myClient.TCPClientReceiveEvent +=
                new EventHandler<TCPClientReceiveEventArgs>(TCPClientReceiveEventHandler);

            _myClient.StateChangeEvent +=
                new ClientRole.StateChangeHandler(this.OnTCPIPStateChange);

            _cmdAgent2 = new MPI.RemoteControl2.Tester.TesterCommandAgent();

            _isMultiDie = (kernel is MultiDie_TesterKernel);
        }

        #endregion

        #region >>>protected method<<<

        protected override int CommandProc()
        {
            bool rtn = false;
            AcquireData acquireData;

            string errStr = "";

            if (CheckIfPackageCorrect(_buffer, out errStr))//若_buffer傳回錯誤訊息
            {
                ShowErrorMsg((int)EErrorCode.TCPIP2_CheckPacket_Err, errStr);

                CmdError echoTSECmd = new CmdError();
                (echoTSECmd as CmdError).ErrorCode = CmdError.EErrorCode.Unknown;
                this._myClient.SendMessage(echoTSECmd.Packet.Serialize());

                return -1;
            }

            MPIDS7600Packet mpiPacket = new MPIDS7600Packet(_buffer);

            if (mpiPacket.CheckPacket())
            {
                CmdMPIBased echoTSECmd = null;
                CmdMPIBased cmd = this._cmdAgent2.CommandFactory(mpiPacket);

                if (cmd == null)
                {
                    ShowErrorMsg((int)EErrorCode.TCPIP2_CommandFactory_Err, "CommandFactory() fail");

                    return -1;
                }

                Console.WriteLine("[TCPTestServer2], CommandID = " + cmd.CommandID.ToString());

                if (cmd.CommandID == (int)ETSECommand.ID_SOT2)
                {
                    (cmd as CmdPropertyBased).Deserialize(mpiPacket.Serialize());
                }
                else
                {
                    cmd.Deserialize(mpiPacket.Serialize());
                }

                switch (cmd.CommandID)
                {
                    case (int)ETSECommand.ID_BARCODE_INSERT:
                        #region >>ID_BARCODE_INSERT<<
                        {
                            Console.WriteLine("[TCPTestServer2], ID_BARCODE_INSERT");

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
                                Console.WriteLine("[TCPTestServer2], ID_BARCODE_INSERT, LoadProduct Fail!");
                            }
                            else if (GlobalFlag.IsSuccessLoadBin == false)
                            {
                                echoTSECmd = new CmdErrorNoBinFile();
                                Console.WriteLine("[TCPTestServer2], ID_BARCODE_INSERT, LoadBin Fail!");
                            }
                        }
                        #endregion
                        break;
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_BEGIN:
                        #region >>ID_WAFER_BEGIN<<
                        {
                            Console.WriteLine("[TCPTestServer2], ID_WAFER_BEGIN");

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_WAFER_BEGIN, null, null);

                            string wafferID_Begin = new string((cmd as CmdWaferBegin).WaferNo);
                            wafferID_Begin = wafferID_Begin.TrimEnd('\0');
                            char[] chArr = wafferID_Begin.ToCharArray();

                            if (chArr.Length == 0)
                            {
                                chArr = new char[1] { 'a' };
                                Console.WriteLine("[TCPTestServer2], ID_WAFER_BEGIN, WaferID = Empty");
                            }
                            else
                            {
                                chArr[0] = 'a';
                            }

                            echoTSECmd = new CmdWaferBegin();
                            (echoTSECmd as CmdWaferBegin).WaferNo = chArr;
                            Console.WriteLine("[TCPTestServer2], ID_WAFER_BEGIN WaferID:" + wafferID_Begin);


                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_IN:
                        #region >>ID_WAFER_IN<<
                        {
                            Console.WriteLine("[TCPTestServer2], ID_WAFER_IN");

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

                                (echoTSECmd as CmdError).ErrorCode = CmdError.EErrorCode.BarcodeInsertFailed;
                            }
                            else
                            {
                                if (GlobalFlag.OutputReportState == EOutputReportState.None)
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
                                            enableItemcount = acquireData.EnableTestResult.Count;
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

                                    (echoTSECmd as CmdError).ErrorCode = CmdError.EErrorCode.WaferIDExisted;
                                    //(echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_TESTER_OUTPUT_FILENAME_EXIST;
                                    //(echoTSECmd as CmdError).ErrorText = "Output file exist".ToCharArray();
                                }
                            }

                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_SCAN_END:
                        #region >>ID_WAFER_SCAN_END<<
                        {
                            Console.WriteLine("[TCPTestServer2], ID_WAFER_SCAN_END");

                            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.EndTest;
                            rtn = this._testerSys.RunCommand((int)ETesterKernelCmd.EndTest);
                            string[] strArr = new string[1];
                            strArr[0] = "";
                            double[] buffer = new double[45];
                            for (int i = 0; i < 45; ++i)
                            {
                                buffer[i] = 0;
                            }
                            buffer[0] = (double)(cmd as CmdWaferScanEnd).XMin; //ColX min
                            buffer[1] = (double)(cmd as CmdWaferScanEnd).YMin; //RowY min
                            buffer[2] = (double)(cmd as CmdWaferScanEnd).XMax; //ColX max
                            buffer[3] = (double)(cmd as CmdWaferScanEnd).YMax; //RowY max
                            buffer[7] = (double)(cmd as CmdWaferScanEnd).ChipCount; //TotalProbingCounts
                            buffer[8] = (double)(cmd as CmdWaferScanEnd).ProbeMoveDirection; //MoveMainAxis
                            buffer[10] = (double)(cmd as CmdWaferScanEnd).ProbeXInitDirection; //XInitDirection
                            buffer[11] = (double)(cmd as CmdWaferScanEnd).ProbeYInitDirection; //YinitDirection

                            buffer[16] = 0.0d; // HighSpeedMode Disable

                            //this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.SourceMeterPowerOn;
                            //this._testerSys.RunCommand((int)ETesterKernelCmd.SourceMeterPowerOn);

                            Fire_ServerQueryEvent(EServerQueryCmd.CMD_TESTER_START, buffer, strArr);
                            echoTSECmd = cmd;//new CmdWaferScanEnd();

                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_SOT:
                        #region >>ID_SOT<<
                        {
                            echoTSECmd = SOTProcess(cmd); // No Use

                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_SOT2:
                        #region >>ID_SOT2<<
                        {
                            Console.WriteLine("[SOT2Test],Start");
                            //echoTSECmd = Test1SOT2(cmd);
                            //echoTSECmd = Test2SOT2(cmd);
                            echoTSECmd = SOT2Process(cmd); // 20200302 Single & Multi 都使用SOT2
                            Console.WriteLine("[SOT2Test],End");

                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_END:
                        #region >>ID_WAFER_END<<
                        {
                            Console.WriteLine("[TCPTestServer2], ID_WAFER_END");
                            double[] buffer2 = new double[9];

                            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.EndTest;
                            this._testerSys.RunCommand((int)ETesterKernelCmd.EndTest);

                            //this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.SourceMeterPowerOn;
                            //this._testerSys.RunCommand((int)ETesterKernelCmd.SourceMeterPowerOn);

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_TESTER_END, buffer2, null);
                            echoTSECmd = new CmdWaferEnd();

                            if (GlobalFlag.IsEnableEndTest == false)
                            {
                                echoTSECmd = new CmdError();
                                (echoTSECmd as CmdError).ErrorCode = CmdError.EErrorCode.TesterCommandLineWaitting;
                                //(echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_TESTER_COMMAND_LINE_WAITTING;
                            }
                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_LOT_END:
                        #region
                        {
                            //this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_LOT_END, null, null);

                            echoTSECmd = new CmdLotEnd();

                            string lotNumStr = new string((cmd as CmdLotEnd).LotNo);

                            // this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_LOT_END, null, null);

                            echoTSECmd = new CmdLotEnd();

                            (echoTSECmd as CmdLotEnd).LotNo = lotNumStr.ToCharArray();

                            break;
                        }
                        #endregion
                    case (int)ETSECommand.ID_LOT_IN:
                        #region
                        {
                            StringBuilder sb = new StringBuilder();

                            string lotNumStr = new string((cmd as CmdLotIn).LotNo);

                            // this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_LOT_END, null, null);

                            echoTSECmd = new CmdLotIn();

                            (echoTSECmd as CmdLotIn).LotNo = lotNumStr.ToCharArray();

                            break;
                        }
                        #endregion
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
                    case (int)ETSECommand.ID_BIN_GRADE:
                        #region >>ID_BIN_GRADE<<
                        {
                            Console.WriteLine("[TCPTestServer2], ID_BIN_GRADE");
                            echoTSECmd = new CmdBinGrade();

                            this._binGradeNames = (this._testerSys as HS_TesterKernel).GetBinGradeNames();

                            (echoTSECmd as CmdBinGrade).BinGradeCount = (this._binGradeNames.Length + 1);

                            for (int i = 0; i < this._binGradeNames.Length; i++)
                            {
                                (echoTSECmd as CmdBinGrade).SetBinGradeName(i, this._binGradeNames[i]);
                            }
                            (echoTSECmd as CmdBinGrade).SetBinGradeName(this._binGradeNames.Length, "Fail Bin");
                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    //case (int)ETSECommand.ID_OVERWRITE_TESTER_REPORT:
                    case (int)ETSECommand.ID_OVERRIDE_TESTER_REPORT:
                        #region >>ID_OVERRIDE_TESTER_REPORT<<
                        {
                            echoTSECmd = new CmdOverrideTesterReport();

                            Console.WriteLine("[TCPTestServer2], ID_OVERWRITE_TESTER_REPORT");

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_OVERWRITE_TESTER_OUTPUT_FILE, null, null);

                            Console.WriteLine("[TCPTestServer2], ID_OVERRIDE_TESTER_REPORT.SUCCESS");

                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_WAFER_FINISH:
                        #region >>ID_WAFER_FINISH<<
                        {
                            Console.WriteLine("[TCPTestServer2], ID_WAFER_FINISH");

                            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_WAFER_FINISH, null, null);

                            string wafferID_Fin = new string((cmd as CmdWaferFinish).WaferNo);
                            wafferID_Fin = wafferID_Fin.TrimEnd('\0');
                            echoTSECmd = new CmdWaferFinish();
                            (echoTSECmd as CmdWaferFinish).WaferNo = wafferID_Fin.ToCharArray();
                            Console.WriteLine("[TCPTestServer2], ID_WAFER_FINISH WaferID:" + wafferID_Fin);
                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    case (int)ETSECommand.ID_QUERY_INFORMATION:
                        #region >>ID_QUERY_INFORMATION<<
                        {
                            Console.WriteLine("[TCPTestServer2], ID_QUERY_INFORMATION");
                            // echoTSECmd = new CmdQueryInformation();
                            TransferableCommonObjectBase tb = new TransferableCommonObjectBase();
                            (cmd as CmdQueryInformation).GetTransferableItem(tb);

                            if (!(cmd as CmdQueryInformation).GetTransferableItem(tb))
                            {
                                ShowErrorMsg((int)EErrorCode.TCPIP2_TransferableItem_Err, " ID_QUERY_INFORMATION ,GetTransferableItem() fail");
                            }
                            int id = tb.GetHashCode();
                            switch (id)
                            {
                                case (int)ETransferableCommonObject.TestingProperties:
                                    {
                                        TestingPropertyProcess(cmd);

                                        echoTSECmd = new CmdQueryInformation();
                                    }
                                    break;
                                case (int)ETransferableCommonObject.MappingTable:
                                    {
                                        MappingTableProcess(cmd);

                                        if (GlobalFlag.IsSuccessCheckChannelConfig == false)
                                        {
                                            Console.WriteLine("[TCPTestServer2], ID_QUERY_INFORMATION, CheckChannelConfig Fail!");

                                            echoTSECmd = new CmdError();

                                            (echoTSECmd as CmdError).ErrorCode = CmdError.EErrorCode.CheckChannelFail;
                                        }
                                        else
                                        {
                                            echoTSECmd = new CmdQueryInformation();
                                        }
                                    }
                                    break;
                                case ((int)ETransferableCommonObject.PreOverloadTestInfo):
                                    {
                                        #region >>> PreOverloadTestInfo <<<
                                        Console.WriteLine("[EMPITestServer2Cmd], ID_QUERY_INFORMATION,PreOverloadTestInfo");


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

                                        #endregion
                                    }
                                    break;
                                case ((int)ETransferableCommonObject.CheckLaserPower):
                                    {
                                        #region >>> CheckLaserPower <<<
                                        Console.WriteLine("[EMPITestServer2Cmd], ID_QUERY_INFORMATION,CheckLaserPower");


                                        CheckLaserPower cTempInfo = new CheckLaserPower();
                                        (cmd as CmdQueryInformation).GetTransferableItem(cTempInfo);

                                        string[] strData = new string[1];
                                        double[] buffer = new double[1];

                                        buffer[0] = cTempInfo.AutoSetLaserPower ? 1 : 0;
                                        if (cTempInfo.Remark != null)
                                        { strData[0] = cTempInfo.Remark; }
                                        else { strData[0] = ""; }

                                        Fire_ServerQueryEvent(EServerQueryCmd.CMD_QUERY_CHECK_LASER_POWER_INFO, buffer, strData);

                                        cTempInfo.IsPowerCheckPass = buffer[0] == 100 ? true : false;

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
                                default:
                                    {
                                        //因前期 command ID 定義時常衝突，因此使用暴力法比對
                                        #region
                                        bool matched = false;
                                        LaserBarProberInfo lbpInfo = new LaserBarProberInfo();
                                        SubRecipeInformation srInfo = new SubRecipeInformation();

                                        if ((cmd as CmdQueryInformation).GetTransferableItem(lbpInfo))
                                        {
                                            LaserBarProberInfoProcess(lbpInfo);
                                            matched = true;
                                        }
                                        else if ((cmd as CmdQueryInformation).GetTransferableItem(srInfo))
                                        {
                                            SubRecipeInformationProcess(srInfo);
                                            matched = true;
                                        }

                                        if (matched)
                                        {
                                            echoTSECmd = new CmdQueryInformation();
                                        }
                                        else
                                        {
                                            errStr = "ID_QUERY_INFORMATION.GetTransferableItem() Fail,ID is:" + id.ToString();
                                            ShowErrorMsg((int)EErrorCode.TCPIP2_CheckPacket_Err, errStr);

                                            echoTSECmd = new CmdError();
                                            (echoTSECmd as CmdError).ErrorCode = CmdError.EErrorCode.Undefined;
                                            this._myClient.SendMessage(echoTSECmd.Packet.Serialize());
                                        }



                                        #endregion
                                    }
                                    break;
                            }

                            break;
                        }
                        #endregion
                    //------------------------------------------------------------------------------------------------
                    default:
                        break;
                }

                if (cmd != null && echoTSECmd != null)
                {
                    byte[] bArr;
                    if (cmd.CommandID == (int)ETSECommand.ID_SOT2)
                    {
                        bArr = (echoTSECmd as CmdPropertyBased).Serialize();
                    }
                    else
                    {
                        bArr = echoTSECmd.Serialize();
                    }

                    this._myClient.SendMessage(bArr);
                    //this._myClient.SendMessage(echoTSECmd.Packet.Serialize());
                    ////this._txtLog.Add(string.Format("[RECV] {0}\r\n", cmd.CommandID.ToString()));
                    ////this._txtLog.Add(string.Format("[SEND] {0}\r\n", echoTSECmd.CommandID.ToString() + "\r\n"));
                    return 1;
                }
            }
            else
            {
                ShowErrorMsg((int)EErrorCode.TCPIP2_CheckPacket_Err, "CheckPacket() fail");
            }

            return -1;
        }

        protected override void ShowErrorMsg(double errID, string errMsg)
        {
            double[] dData = new double[] { errID }; ;

            string[] sData = new string[] { errMsg };

            Console.WriteLine("[TCPTestServer2], CommandProc()," + errMsg);

            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_SHOW_ERROR, dData, sData);
        }

        //個別Channel查詢
        private CmdMPIBased Test1SOT2(CmdMPIBased cmd)
        {
            CmdSOT2 cmd2 = (CmdSOT2)cmd;

            string aoiStr = "";

            for (int i = 0; i < 256; ++i)
            {
                string tempStr = "";
                string keyStr = "AOI_SIGN#"+i.ToString();
                tempStr = cmd2.GetStringProperty(keyStr);
                aoiStr += tempStr + ",";
            }
            aoiStr = "a";
            Console.WriteLine(aoiStr);


            CmdMPIBased echoTSECmd = new CmdEOT2();

            return echoTSECmd;
        }

        //單一字串解析
        private CmdMPIBased Test2SOT2(CmdMPIBased cmd)
        {
            CmdSOT2 cmd2 = (CmdSOT2)cmd;

            string cmdStr = cmd2.GetStringProperty("AOI_SIGN");
            string aoiStr = "";
            string[] strArr = cmdStr.Split(',');


            for (int i = 0; i < strArr.Length; ++i)
            {
                aoiStr += strArr[i] + ",";
            }
            aoiStr = "a";
            Console.WriteLine(aoiStr);


            CmdMPIBased echoTSECmd = new CmdEOT2();

            return echoTSECmd;
        }


        private CmdMPIBased SOT2Process(CmdMPIBased cmd)
        {
            AcquireData acquireData;
            CmdMPIBased echoTSECmd;
            //this._pt1.Start();
            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.RunTest;
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.COL] = Convert.ToDouble((cmd as CmdSOT2).WaferPositionX);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ROW] = Convert.ToDouble((cmd as CmdSOT2).WaferPositionY);
            string channelStatus = (cmd as CmdSOT2).NeedleMask;//有無
            string subBin = (cmd as CmdSOT2).ConditionMask;//group

            if ((cmd as CmdSOT2).TransientLayers.Layers > 2)//最上面的[col,row]以及 最底下的[subx,suby]以外層級的座標，目前還沒想到辦法傳進去
            {
                var lInfo = (cmd as CmdSOT2).TransientLayers.Infos;
            }

            if (_isMultiDie)
            {
                this._testerSys.CmdData.StringData[0] = channelStatus.TrimEnd('\0');
                this._testerSys.CmdData.StringData[1] = subBin.TrimEnd('\0');
                this._testerSys.CmdData.StringData[2] = channelStatus.TrimEnd('\0');
            }

            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.PROBE_INDEX] = Convert.ToDouble((cmd as CmdSOT2).ProbeIndex); // DP Chuck Index
            
            this._testerSys.CmdData.DoubleData[2] = (cmd as CmdSOT2).SubDieIndex;//SOT時沒有用到DoubleData[2]，先拿來用一下 // for Sony
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.SUBDIEINDEX] = Convert.ToDouble((cmd as CmdSOT2).ProbingChipIndex);

            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.GroupCOL] = Convert.ToDouble((cmd as CmdSOT2).GroupPositionX);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.GroupROW] = Convert.ToDouble((cmd as CmdSOT2).GroupPositionY);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.Temprature] = Convert.ToDouble((cmd as CmdSOT2).TestingTemperature) / 1000;//因CmdSOT2使用int傳遞，因此將溫度*1000，這邊記得除回來

            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.SubCOL] = Convert.ToDouble((cmd as CmdSOT2).SubDiePositionX);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.SubROW] = Convert.ToDouble((cmd as CmdSOT2).SubDiePositionY);

            // 20200206 Jeemo 新增DX/DY
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.DX_INDEX] = Convert.ToDouble((cmd as CmdSOT2).DXIndex);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.DY_INDEX] = Convert.ToDouble((cmd as CmdSOT2).DYIndex);

            bool rtn = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);

            if (this._isEnableREOT)
            {
                echoTSECmd = new CmdREOT();

                byte[] bArr = (echoTSECmd as CmdPropertyBased).Serialize();

                this._myClient.SendMessage(bArr);
            }

            this._testerSys.GetTestedDataFromDevice();
            this._testerSys.ResetTesterCond();

            char[] resultData = new Char[MPIDS7600Command.ConstDefinition.MAX_RESULT_DATA];
            acquireData = this._testerSys.Data;

            echoTSECmd = new CmdEOT2();

            int enableItemcount = 0;

            #region >>calc enable item<<

            List<string> enableItem = new List<string>();
            List<string> enableKeys = acquireData.EnableTestResult;
            for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
            {
                if (this._isTCPIPSendEnableResultItem)
                {
                    if (acquireData.OutputTestResult[i].Name != string.Empty && acquireData.OutputTestResult[i].IsThisItemTested)
                    {
                        enableItem.Add(acquireData.OutputTestResult[i].Name);
                    }
                }
                else
                {
                    enableItem.Add(acquireData.OutputTestResult[i].Name);
                }
            }

            enableItemcount = enableItem.Count;
            #endregion

            if (_isMultiDie)
            {
                echoTSECmd = EOT2MultiDieProcess(acquireData, enableItemcount);
            }
            else
            {
                echoTSECmd = EOT2SingleDieProcess(acquireData, enableItemcount);
            }
            //-----------------------------------------------------------
            // (2) Transfer UI Operation State
            //----------------------------------------------------------

            if (GlobalFlag.IsSourceMeterDisconnect)
            {
                Console.WriteLine("[TCPTestServer2], SourceMeter Disconnect");
                echoTSECmd = new CmdError();
                (echoTSECmd as CmdError).ErrorCode = CmdError.EErrorCode.SourceMeterDisconnect;
                //(echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_TESTER_SOURCEMETER_DISCONNECT;
            }
            return echoTSECmd;
        }

        private bool ParseSOT2Table(CmdSOT2 cmd)
        {
            bool result = true;

            return result;

        }

        private CmdEOT2 EOT2SingleDieProcess(AcquireData acquireData, int enableItemcount)
        {
            CmdEOT2 echoTSECmd = new CmdEOT2();
            string strData = "";
            int count = 0;
            echoTSECmd.TestDateCount = 1;

            TestData tData = echoTSECmd[0];
            tData.Bin = acquireData.ChipInfo.BinGrade;
            tData.TestResultCount = enableItemcount;
            tData.Column = acquireData.ChipInfo.ColX;
            tData.Row = acquireData.ChipInfo.RowY;
            tData.TestStatus = (acquireData.ChipInfo.IsPass) ? TestData.Status.Pass : TestData.Status.Fail;

            if (this._isTCPIPSendEnableResultItem)
            {
                for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                {
                    if (acquireData.OutputTestResult[i].IsThisItemTested)
                    {
                        strData = acquireData.OutputTestResult[i].Value.ToString(acquireData.OutputTestResult[i].Formate);

                        tData.SetTestResult(count, strData);

                        count++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
                {
                    if (count >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                        break;
                    strData = acquireData.OutputTestResult[i].Value.ToString(acquireData.OutputTestResult[i].Formate);
                    tData.SetTestResult(count, strData);
                    count++;
                }
            }
            return echoTSECmd;
        }

        private CmdEOT2 EOT2MultiDieProcess(AcquireData acquireData, int enableItemcount)
        {
            CmdEOT2 echoTSECmd = new CmdEOT2();
            int chQty = acquireData.ChannelResultDataSet.Count;
            echoTSECmd.TestDateCount = chQty;

            string strData = "";
            int count = 0;

            for (int ch = 0; ch < chQty; ++ch)
            {
                count = 0;

                TestData tData = echoTSECmd[ch];

                ChannelResultData channelResultData = acquireData.ChannelResultDataSet[(uint)ch];
                tData.Bin = channelResultData.BinGrade;
                tData.TestResultCount = enableItemcount;
                tData.Column = channelResultData.Col;
                tData.Row = channelResultData.Row;
                tData.TestStatus = (channelResultData.IsPass) ? TestData.Status.Pass : TestData.Status.Fail;

                if (this._isTCPIPSendEnableResultItem)
                {
                    for (int i = 0; i < channelResultData.Count; i++)
                    {
                        if (channelResultData[i].IsThisItemTested)
                        {
                            strData = channelResultData[i].Value.ToString(channelResultData[i].Formate);

                            tData.SetTestResult(count, strData);

                            count++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < channelResultData.Count; i++)
                    {
                        if (count >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                            break;
                        strData = channelResultData[i].Value.ToString(channelResultData[i].Formate);
                        tData.SetTestResult(count, strData);
                        count++;
                    }
                }
            }
            return echoTSECmd;
        }

        private CmdMPIBased SOTProcess(CmdMPIBased cmd)
        {
            AcquireData acquireData;
            CmdMPIBased echoTSECmd;
            //this._pt1.Start();
            this._testerSys.CmdData.CmdID = (int)ETesterKernelCmd.RunTest;
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.COL] = Convert.ToDouble((cmd as CmdSOT1).WaferPositionX);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.ROW] = Convert.ToDouble((cmd as CmdSOT1).WaferPositionY);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.PROBE_INDEX] = Convert.ToDouble((cmd as CmdSOT1).ProbeIndex);
            this._testerSys.CmdData.DoubleData[2] = (cmd as CmdSOT1).SubDieIndex;//SOT時沒有用到DoubleData[2]，先拿來用一下

            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.SUBDIEINDEX] = Convert.ToDouble((cmd as CmdSOT1).ProbingChipIndex);

            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.GroupCOL] = Convert.ToDouble((cmd as CmdSOT1).GroupPositionX);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.GroupROW] = Convert.ToDouble((cmd as CmdSOT1).GroupPositionY);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.Temprature] = Convert.ToDouble((cmd as CmdSOT1).TestingTemperature);

            // 20190613 Jeemo 新增SubDie Col/Row
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.SubCOL] = Convert.ToDouble((cmd as CmdSOT1).SubDiePositionX);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.SubROW] = Convert.ToDouble((cmd as CmdSOT1).SubDiePositionY);

            // 20200206 Jeemo 新增DX/DY
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.DX_INDEX] = Convert.ToDouble((cmd as CmdSOT1).DXIndex);
            this._testerSys.CmdData.DoubleData[(uint)EProberDataIndex.DY_INDEX] = Convert.ToDouble((cmd as CmdSOT1).DYIndex);

            bool rtn = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);

            if (this._isEnableREOT)
            {
                echoTSECmd = new CmdREOT();

                byte[] bArr = echoTSECmd.Serialize();

                this._myClient.SendMessage(bArr);
            }

            this._testerSys.GetTestedDataFromDevice();
            this._testerSys.ResetTesterCond();

            string strData = "";
            char[] resultData = new Char[MPIDS7600Command.ConstDefinition.MAX_RESULT_DATA];
            acquireData = this._testerSys.Data;

            echoTSECmd = new CmdEOT();

            int enableItemcount = 0;
            List<string> enableKeyList = acquireData.EnableTestResult;

            for (int i = 0; i < acquireData.OutputTestResult.Count; i++)
            {
                if (enableItemcount >= MPIDS7600Command.ConstDefinition.MAX_TEST_ITEM_LIST)
                {
                    break;
                }

                if (acquireData.OutputTestResult[i].IsThisItemTested)
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

                    if (acquireData.OutputTestResult[i].IsThisItemTested)
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
                Console.WriteLine("[TCPTestServer2], SourceMeter Disconnect");
                echoTSECmd = new CmdError();
                (echoTSECmd as CmdError).ErrorCode = CmdError.EErrorCode.SourceMeterDisconnect;
                //(echoTSECmd as CmdError).ErrorCode = (int)DS_ERROR_CODE.DS_ERROR_TESTER_SOURCEMETER_DISCONNECT;
            }
            return echoTSECmd;
        }

        private void SubRecipeInformationProcess(SubRecipeInformation srInfo)
        {
            double[] dData = new double[3];
            string[] sData = new string[3];

            sData[0] = srInfo.MainRecipeName;
            sData[1] = srInfo.SubRecipeName;
            sData[2] = srInfo.LaserID;

            dData[0] = srInfo.SlotNumber;
            dData[1] = srInfo.SubRecipeIndex;
            dData[2] = srInfo.Temperature;

            Console.WriteLine("[TCPTestServer2], ID_QUERY_INFORMATION,CMD_LASER_BAR_INFO, SubRecipeInformation = " + srInfo.SubRecipeName);

            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_SUB_RECIPE_INFO, dData, sData);
        }

        private void LaserBarProberInfoProcess(LaserBarProberInfo lbpInfo)
        {
            double[] dData = new double[10];

            double temperature = lbpInfo.Temperature;

            dData[0] = temperature;

            Console.WriteLine("[TCPTestServer2], ID_QUERY_INFORMATION,CMD_LASER_BAR_INFO, ProbingTemperature = " + temperature.ToString("0.00"));

            this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_LASER_BAR_INFO, dData, null);
        }

        private void TestingPropertyProcess(CmdMPIBased cmd)
        {
            TestingProperties tp = new TestingProperties();
            if ((cmd as CmdQueryInformation).GetTransferableItem(tp))
            {
                List<string> strList = new List<string>();
                var xDic = tp.Properties;

                Dictionary<string, string> keyStrDic = new Dictionary<string, string>();
                //Dictionary<string, double> keyValDic = new Dictionary<string, double>();

                foreach (var p in xDic)
                {
                    if (!keyStrDic.ContainsKey(p.Key))
                    {
                        if (p.Value.ValueType == "s")
                        {
                            keyStrDic.Add(p.Key, p.Value.Value);
                        }
                        else
                        {
                            double val = double.NaN;
                            switch (p.Value.ValueType)
                            {
                                case "B":
                                    {
                                        bool res = p.Value.Value;
                                        val = res ? 1 : 0;
                                    }
                                    break;
                                case "i3":
                                case "i1":
                                case "i6":
                                    {
                                        int res = p.Value.Value;
                                        val = (double)res;
                                    }
                                    break;
                                case "f":
                                    {
                                        float res = p.Value.Value;
                                        val = (double)res;
                                    }
                                    break;
                                case "d":
                                    {
                                        val = p.Value.Value;
                                    }
                                    break;
                            }
                            if (val != null)
                            { keyStrDic.Add(p.Key, val.ToString()); }


                        }
                    }
                }
                if (keyStrDic != null)
                {
                    foreach (var p in keyStrDic)
                    {
                        string str = p.Key + "," + p.Value.ToString().Trim();
                        Console.WriteLine(str);
                        strList.Add(str);
                    }
                }

                //keyValDic
                string[] sData = strList.ToArray();
                this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_TESTING_PROPERTY, null, sData);


            }
        }

        private void MappingTableProcess(CmdMPIBased cmd)
        {
            MappingTable tp = new MappingTable();
            if ((cmd as CmdQueryInformation).GetTransferableItem(tp))
            {
                List<string> strList = new List<string>();
                int chQty = tp.Count;
                int layer = -(tp.Layers - 1);//ChannelPosShiftTable 設計方式為 COL/ROW為第0層，subX/Y為第 (-N)層，因此在這邊先做轉換
                for (int ch = 0; ch < chQty * 2; ++ch)//保險起見，嘗試總數量2倍的channel數，以免未來出現channel不連續的狀況
                {
                    int col = 0;
                    int row = 0;
                    if (tp.Get(ch, out col, out row))
                    {
                        string str = ch.ToString() + "," + layer.ToString() + "," + col.ToString() + "," + row.ToString();
                        strList.Add(str);
                    }
                }
                string[] sData = strList.ToArray();
                this.Fire_ServerQueryEvent(EServerQueryCmd.CMD_MAPPING_TABLE, null, sData);

            }
        }
        #endregion
    }
}
