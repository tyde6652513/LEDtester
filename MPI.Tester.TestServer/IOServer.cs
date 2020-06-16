using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Device.IOCard;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;

namespace MPI.Tester.TestServer
{
    public class IOServer
    {
        public event EventHandler<ServerQueryEventArg> ServerQueryEvent;

        private object _lockObj;
        private bool _isTested;
        private bool _isAutoRunStart;
        private TesterKernelBase _testerSys;
        private AcquireData _acquireData;
        private IIOCard _ioCard;
        private IOCommand _ioCommand;
        private bool _activeState;
        private EEqModel _eqModle;

        public IOServer()
        {
            this._lockObj = new object();

            this._isTested = false;

            this._isAutoRunStart = false;

            this._ioCommand = new IOCommand();

            this._ioCommand.StartOfTest += this.OnStartOfTest;

            //Active High: true, Active Low: false 
            this._activeState = true;

            this._eqModle = EEqModel.Prober;
        }

        public IOServer(TesterKernelBase kernel, EEqModel eqModel, EActiveState activeState)
            : this()
        {
            this._testerSys = kernel;

            this._acquireData = (this._testerSys as HS_TesterKernel).Data;

            this._eqModle = eqModel;

            if (activeState == EActiveState.ActiveHigh)
            {
                this._activeState = true;
            }
            else
            {
                this._activeState = false;
            }
        }

        #region >>> Public Proterties <<<

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
            }
        }

        #region >>> CommandProc <<<

        private int MPIProbeCommandProc(EMPIIOTestServerCmd cmd)
        {
            switch (cmd)
            {
                case EMPIIOTestServerCmd.TS_CMD_NONE:
                    {
                        break;
                    }
                case EMPIIOTestServerCmd.TS_CMD_SOT:
                    {
                        if (this._isAutoRunStart)
                        {
                            //////////////////////////////////////////////
                            // Receive SOT, Sent EOT、REOT and Start Test 
                            //////////////////////////////////////////////
                            this._ioCard.DO[(int)EIOPin.EOT] = this._activeState;

                            this._ioCard.DO[(int)EIOPin.REOT] = this._activeState;

                            this._isTested = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);

                            //////////////////////////////////////////////
                            // Start Test Success, Clear REOT
                            //////////////////////////////////////////////
                            this._ioCard.DO[(int)EIOPin.REOT] = !this._activeState;

                            this._testerSys.GetTestedDataFromDevice();

                            this._testerSys.ResetTesterCond();

                            //////////////////////////////////////////////
							// Check Source Is Connect
							//////////////////////////////////////////////
							if (GlobalFlag.IsSourceMeterDisconnect)
							{
								return 0;
							}

                            //////////////////////////////////////////////
                            // Calculate Success, Send Pass / Fail
                            //////////////////////////////////////////////
                            //	IsPass:  " minValue < value < maxValue "
                            if (this._acquireData.ChipInfo.IsPass)
                            {
                                this._ioCard.DO[(int)EIOPin.Pass] = this._activeState;

                                this._ioCard.DO[(int)EIOPin.Fail] = !this._activeState;
                            }
                            else
                            {
                                this._ioCard.DO[(int)EIOPin.Pass] = !this._activeState;

                                this._ioCard.DO[(int)EIOPin.Fail] = this._activeState;
                            }

                            //////////////////////////////////////////////
                            // Send BIN Pass / Fail, Clear SOT
                            //////////////////////////////////////////////
                            this._ioCard.DO[(int)EIOPin.EOT] = !this._activeState;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return 1;
        }

        private int MPIHandlerCommandProc(EMPIIOTestServerCmd cmd)
        {
            switch (cmd)
            {
                case EMPIIOTestServerCmd.TS_CMD_NONE:
                    {
                        break;
                    }
                case EMPIIOTestServerCmd.TS_CMD_SOT:
                    {
                        //if (this._isAutoRunStart)
                        {
                            ///////////////////////////////////////////////////////////
                            // Receive SOT, Start Test and Calculate
                            ///////////////////////////////////////////////////////////
                            this._isTested = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);

                            this._testerSys.GetTestedDataFromDevice();

                            this._testerSys.ResetTesterCond();

							//////////////////////////////////////////////
							// Check Source Is Connect
							//////////////////////////////////////////////
							if (GlobalFlag.IsSourceMeterDisconnect)
							{
								return 0;
							}

                            ///////////////////////////////////////////////////////////
                            // Calculate Success, Send BIN and dlay 3ms
                            ///////////////////////////////////////////////////////////
                            List<int> pins = new List<int>();

                            for (int i = (int)EIOPin.BIN0; i < (int)EIOPin.BIN7; i++)
                            {
                                pins.Add(i);
                            }

                            this._ioCard.WriteDO(pins.ToArray(), false);

                            pins.Clear();

                            int BinGrade = this._testerSys.Data.ChipInfo.BinGrade;

                            if (BinGrade > 0 && BinGrade < 255)
                            {                               
                                string str = Convert.ToString(BinGrade, 2);

                                pins = new List<int>();

                                for (int i = 0; i < str.Length; i++)
                                {
                                    int pin = int.Parse(str[i].ToString());

                                    if (pin == 0)
                                    {
                                        continue;
                                    }

                                    // 0 base: BinGrade = BinGrade - 1
                                    pins.Add(str.Length - i + (int)EIOPin.BIN0 - 1);
                                }

                                this._ioCard.WriteDO(pins.ToArray(), true);
                            }

                            System.Threading.Thread.Sleep(3);

                            ///////////////////////////////////////////////////////////
                            // Send BIN Success, Send EOT & delay 12ms then clear EOT
                            ///////////////////////////////////////////////////////////
                            this._ioCard.DO[(int)EIOPin.EOT] = this._activeState;

                            System.Threading.Thread.Sleep(5);

                            this._ioCard.DO[(int)EIOPin.EOT] = !this._activeState;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return 1;
        }

        private int MPITapingCommandProc(EMPIIOTestServerCmd cmd)
        {
            switch (cmd)
            {
                case EMPIIOTestServerCmd.TS_CMD_NONE:
                    {
                        break;
                    }
                case EMPIIOTestServerCmd.TS_CMD_SOT:
                    {
                        //if (this._isAutoRunStart)
                        {
                            ///////////////////////////////////////////////////////////
                            // Receive SOT, Start Test and Calculate
                            ///////////////////////////////////////////////////////////
                            this._isTested = this._testerSys.RunCommand((int)ETesterKernelCmd.RunTest);

                            this._testerSys.GetTestedDataFromDevice();

                            this._testerSys.ResetTesterCond();

                            //////////////////////////////////////////////
							// Check Source Is Connect
							//////////////////////////////////////////////
							if (GlobalFlag.IsSourceMeterDisconnect)
							{
								return 0;
							}

                            //////////////////////////////////////////////
                            // Calculate Success, Send Pass / Fail and Polar
                            //////////////////////////////////////////////

                            //	1 : Good  " minValue < value < maxValue "
                            if (this._acquireData.ChipInfo.IsPass)
                            {
                                this._ioCard.DO[(int)EIOPin.Pass] = this._activeState;

                                this._ioCard.DO[(int)EIOPin.Fail] = !this._activeState;
                            }
                            else
                            {
                                this._ioCard.DO[(int)EIOPin.Pass] = !this._activeState;

                                this._ioCard.DO[(int)EIOPin.Fail] = this._activeState;
                            }

                            if (this._acquireData.ChipInfo.Polarity == EPolarity.Anode_P)
                            {
                                this._ioCard.DO[(int)EIOPin.Anode_P] = this._activeState;

                                this._ioCard.DO[(int)EIOPin.Cathode_N] = !this._activeState;
                            }
                            else
                            {
                                this._ioCard.DO[(int)EIOPin.Anode_P] = !this._activeState;

                                this._ioCard.DO[(int)EIOPin.Cathode_N] = this._activeState;
                            }

                            System.Threading.Thread.Sleep(3);

                            ///////////////////////////////////////////////////////////
                            // Send BIN Success, Send EOT & delay 12ms then clear EOT
                            ///////////////////////////////////////////////////////////
                            this._ioCard.DO[(int)EIOPin.EOT] = this._activeState;

                            System.Threading.Thread.Sleep(12);

                            this._ioCard.DO[(int)EIOPin.EOT] = !this._activeState;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return 1;
        }

        #endregion

        #endregion

        #region >>> Public Methods <<<

		public void OnStartOfTest()
		{
			switch (this._eqModle)
			{
                case EEqModel.Prober:
					{
						this.MPIProbeCommandProc(EMPIIOTestServerCmd.TS_CMD_SOT);

						break;
					}
                case EEqModel.Handler:
					{
						this.MPIHandlerCommandProc(EMPIIOTestServerCmd.TS_CMD_SOT);

						break;
					}
                case EEqModel.Taping:
					{
						this.MPITapingCommandProc(EMPIIOTestServerCmd.TS_CMD_SOT);

						break;
					}
				default:
					break;
			}


		}

        public bool Open(EIOCardModel ioCardModel)
        {
            switch (ioCardModel)
            {
                case EIOCardModel.NONE:
                    {
                        this._ioCard = null;

                        break;
                    }
                case EIOCardModel.PCI1756:
                    {
                        this._ioCard = new PCI1756Wrapper(this._ioCommand, this._activeState);

                        if (this._ioCard.Init())
                        {
                            List<int> pins = new List<int>();

                            pins.AddRange((int[])Enum.GetValues(typeof(EIOPin)));

                            this._ioCard.WriteDO(pins.ToArray(), !this._activeState);
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    }
                default:
                    {
                        this._ioCard = null;

                        break;
                    }
            }

            return true;
        }

        public void Close()
        {
            this._ioCommand.StartOfTest -= this.OnStartOfTest;

            this._ioCard.Close();
        }

        #endregion
    }
}
