using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;

namespace MPI.Tester.Device.LCRMeter
{
    public class WK4100 : LCRBase 
	{
		private IConnect _device;
		private LCRDevSetting _devSetting;
		private LCRSettingData[] _lcrSetting;
		private EDevErrorNumber _errorCode;
		private double[][] _msrtResult;
		private string _serialNumber;
		private string _softwareVersion;
		private string _hardwareVersion;
		private LCRMeterSpec _spec;
        private LCRCaliData _caliData;
		private CmdData _cmdList;

		public WK4100()
		{
			this._devSetting = new LCRDevSetting();

			this._lcrSetting = null;

			this._msrtResult = null;

			this._errorCode = EDevErrorNumber.Device_NO_Error;

			this._serialNumber = string.Empty;

			this._softwareVersion = string.Empty;

			this._hardwareVersion = string.Empty;

			this._cmdList = new CmdData();
		}

		public WK4100(LCRDevSetting Setting)
			: this()
		{
			this._devSetting = Setting;
		}

		public string SerialNumber
		{
			get { return this._serialNumber; }
		}

		public string SoftwareVersion
		{
			get { return this._softwareVersion; }
		}

		public string HardwareVersion
		{
			get { return this._hardwareVersion; }
		}

		public LCRSettingData[] LCRSetting
		{
			get { return this._lcrSetting; }
		}

		public EDevErrorNumber ErrorNumber
		{
			get { return this._errorCode; }
		}

		public LCRMeterSpec Spec
		{
			get { return this._spec; }
		}
        public LCRCaliData CaliData
        {
            get { return this._caliData; }
        }
        public bool SetCaliData(LCRCaliData caliData)
        {
            return true;
        }
        public bool LCRCali(ELCRCaliMode caliMode)
        {
            return true;
        }

		private void SetDeviceSpec(string sn)
		{
			this._spec = new LCRMeterSpec();

			this._spec.IsProvideSignalLevelV = true;

			this._spec.IsProvideSignalLevelI = false;

			this._spec.IsProvideDCBiasV = true;

			this._spec.IsProvideDCBiasI = false;

			this._spec.SignalLevelVMin = 0.01;

			this._spec.SignalLevelVMax = 2;

			this._spec.SignalLevelIMin = 0;

			this._spec.SignalLevelIMax = 0;

			this._spec.FrequencyMin = 20;

			this._spec.FrequencyMax = 1e6;

			this._spec.DCBiasVMin = 0;

			this._spec.DCBiasVMax = 2;

			this._spec.DCBiasIMin = 0;

			this._spec.DCBiasIMax = 0;

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Long);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Medium);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Short);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Max);

			if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Ext_Master ||
				this._devSetting.LCRDCBiasType == ELCRDCBiasType.Other)
			{
				this._spec.DCBiasVMax = 40;
			}

			foreach (var item in Enum.GetNames(typeof(ELCRTestType)))
			{
				ELCRTestType type = (ELCRTestType)Enum.Parse(typeof(ELCRTestType), item);

				if (type == ELCRTestType.VDID ||
					type == ELCRTestType.YTR ||
					type == ELCRTestType.ZTR)
				{
					continue;
				}

				this._spec.TestTypeList.Add(type);
			}
		}

		private bool ParseMsrtData(string msg, uint index)
		{
			string[] data = msg.Split(',');

			double valueA = 0.0d;

			double valueB = 0.0d;

			for (int i = 0; i < this._msrtResult[index].Length; i++)
			{
				this._msrtResult[index][i] = 0.0d;
			}

			if (double.TryParse(data[0], out valueA) && double.TryParse(data[1], out valueB))
			{
                switch (this._lcrSetting[index].LCRMsrtType)
				{
					case ELCRTestType.CPD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
						break;
					case ELCRTestType.CPG:
						this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRG] = valueB;
						break;
					case ELCRTestType.CPQ:
						this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
						break;
					case ELCRTestType.CPRP:
						this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRP] = valueB;
						break;
					case ELCRTestType.CSD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRCS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
						break;
					case ELCRTestType.CSQ:
						this._msrtResult[index][(int)ELCRMsrtType.LCRCS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
						break;
					case ELCRTestType.CSRS:
						this._msrtResult[index][(int)ELCRMsrtType.LCRCS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRS] = valueB;
						break;
					case ELCRTestType.LPD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
						break;
					case ELCRTestType.LPG:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRG] = valueB;
						break;
					case ELCRTestType.LPQ:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
						break;
					case ELCRTestType.LPRD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRDC] = valueB;
						break;
					case ELCRTestType.LPRP:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRP] = valueB;
						break;
					case ELCRTestType.LSD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
						break;
					case ELCRTestType.LSQ:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
						break;
					case ELCRTestType.LSRD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRDC] = valueB;
						break;
					case ELCRTestType.LSRS:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRS] = valueB;
						break;
					case ELCRTestType.GB:
						this._msrtResult[index][(int)ELCRMsrtType.LCRG] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRB] = valueB;
						break;
					case ELCRTestType.RX:
						this._msrtResult[index][(int)ELCRMsrtType.LCRR] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRX] = valueB;
						break;
					case ELCRTestType.VDID:
						this._msrtResult[index][(int)ELCRMsrtType.LCRVDC] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRIDC] = valueB;
						break;
					case ELCRTestType.YTD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRY] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRTD] = valueB;
						break;
					case ELCRTestType.YTR:
						this._msrtResult[index][(int)ELCRMsrtType.LCRY] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRTR] = valueB;
						break;
					case ELCRTestType.ZTD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRZ] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRTD] = valueB;
						break;
					case ELCRTestType.ZTR:
						this._msrtResult[index][(int)ELCRMsrtType.LCRZ] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRTR] = valueB;
						break;
					default:
						break;
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		public bool Init(int deviceNum, string sourceMeterSN)
		{
			this.SetDeviceSpec(this._serialNumber);

			// check is ip address 
			bool isOnlyIPAddressNum = false;

			string[] ip = sourceMeterSN.Split('.');

			if (ip.Length == 4)
			{
				isOnlyIPAddressNum = true;

				foreach (var item in ip)
				{
					int num = 0;

					isOnlyIPAddressNum &= int.TryParse(item, out num);
				}
			}

			if (isOnlyIPAddressNum)
			{
				LANSettingData lanData = new LANSettingData();

				lanData.IPAddress = sourceMeterSN;

                lanData.Port = 9760;

				this._device = new LANConnect(lanData);
			}
			else
			{
				//"TCPIP0::" + sourceMeterSN + "::inst0::INSTR";

				//"USB0::0x0B6A::0x5346::0944459::INSTR"

				this._device = new IVIConnect(sourceMeterSN);
			}

			string msg = string.Empty;

            if (!this._device.Open(out msg))
            {
                this._errorCode = EDevErrorNumber.LCRInitFail;

                return false;
            }

			this._device.SendCommand("*IDN?");

			this._device.WaitAndGetData(out msg);

			string[] data = msg.Replace("\n", "").Split(',');

			if (data.Length != 4) 
			{
				this._errorCode = EDevErrorNumber.LCRInitFail;

				return false;
			}

			this._serialNumber = data[2];

			this._softwareVersion = "NA";

			this._hardwareVersion = data[3];

			//this._device.SendCommand("*RST;*CLS");

			if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
			{
				this._device.SendCommand(":MEAS:BIAS VINT");

				this._device.SendCommand(":MEAS:BIAS ON");
			}
			else if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Ext_Master ||
						this._devSetting.LCRDCBiasType == ELCRDCBiasType.Ext_Other)
			{
				this._device.SendCommand(":MEAS:BIAS VEXT");

				this._device.SendCommand(":MEAS:BIAS ON");
			}
			else
			{
				this._device.SendCommand(":MEAS:BIAS OFF");
			}

			return true;
		}

		public void TurnOff()
		{
            this._device.SendCommand(":MEAS:LEV 0.01");

            this._cmdList.SingleLevel = ":MEAS:LEV 0.01";
		}

		public bool SetConfigToMeter(LCRDevSetting devSetting)
		{
			this._devSetting = devSetting;

			return true;
		}

		public bool SetParamToMeter(LCRSettingData[] paramSetting)
		{
			this._errorCode = EDevErrorNumber.Device_NO_Error;

			if (paramSetting == null || paramSetting.Length == 0)
			{
				this._lcrSetting = null;

				this._msrtResult = null;

				return false;
			}

			int msrtLen = Enum.GetNames(typeof(ELCRMsrtType)).Length;

			this._msrtResult = new double[paramSetting.Length][];

			for (int i = 0; i < paramSetting.Length; i++)
			{
				LCRSettingData item = paramSetting[i];

				if (item.SignalMode == ELCRSignalMode.Voltage)
				{
					if (item.SignalLevelV < this._spec.SignalLevelVMin || item.SignalLevelV > this._spec.SignalLevelVMax)
					{
						this._lcrSetting = null;

						this._msrtResult = null;

						this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

						return false;
					}
				}
				else
				{
					if (item.SignalLevelI < this._spec.SignalLevelIMin || item.SignalLevelI > this._spec.SignalLevelIMax)
					{
						this._lcrSetting = null;

						this._msrtResult = null;

						this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

						return false;
					}
				}

				if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
				{
					if (item.DCBiasMode == ELCRDCBiasMode.Voltage)
					{
						if (item.DCBiasV < this._spec.DCBiasVMin || item.DCBiasV > this._spec.DCBiasVMax)
						{
							this._lcrSetting = null;

							this._msrtResult = null;

							this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

							return false;
						}
					}
					else
					{
						if (item.DCBiasI < this._spec.DCBiasIMin || item.DCBiasI > this._spec.DCBiasIMax)
						{
							this._lcrSetting = null;

							this._msrtResult = null;

							this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

							return false;
						}
					}
				}

				if (item.Frequency < this._spec.FrequencyMin || item.Frequency > this._spec.FrequencyMax)
				{
					this._lcrSetting = null;

					this._msrtResult = null;

					this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

					return false;
				}

				this._msrtResult[i] = new double[msrtLen];
			}

			this._lcrSetting = paramSetting;

			return true;
		}

        public bool PreSettingParamToMeter(uint settingIndex)
        {
            if (this._lcrSetting == null || this._lcrSetting.Length == 0)
            {
				return true;
            }

            LCRSettingData data = this._lcrSetting[settingIndex];

			System.Threading.Thread.Sleep((int)data.MsrtDelayTime);

            string frequency = ":MEAS:FREQ " + data.Frequency;

            string singleLevel = ":MEAS:LEV " + data.SignalLevelV;

            string func1 = string.Empty;

            string func2 = string.Empty;

            string testType = string.Empty;

            string speed = string.Empty;

            string range = string.Empty;

            switch (data.LCRMsrtType)
			{
				case ELCRTestType.CPD:
					func1 = ":MEAS:FUNC1 C";
					func2 = ":MEAS:FUNC2 D";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.CPG:
					func1 = ":MEAS:FUNC1 C";
					func2 = ":MEAS:FUNC2 G";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.CPQ:
					func1 = ":MEAS:FUNC1 C";
					func2 = ":MEAS:FUNC2 Q";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.CPRP:
					func1 = ":MEAS:FUNC1 C";
					func2 = ":MEAS:FUNC2 R";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.CSD:
					func1 = ":MEAS:FUNC1 C";
					func2 = ":MEAS:FUNC2 D";
					testType = ":MEAS:EQU-CCT SER";
					break;
				case ELCRTestType.CSQ:
					func1 = ":MEAS:FUNC1 C";
					func2 = ":MEAS:FUNC2 Q";
					testType = ":MEAS:EQU-CCT SER";
					break;
				case ELCRTestType.CSRS:
					func1 = ":MEAS:FUNC1 C";
					func2 = ":MEAS:FUNC2 R";
					testType = ":MEAS:EQU-CCT SER";
					break;
				case ELCRTestType.LPD:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 D";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.LPG:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 G";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.LPQ:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 Q";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.LPRD:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 RDC";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.LPRP:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 R";
					testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.LSD:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 D";
					testType = ":MEAS:EQU-CCT SER";
					break;
				case ELCRTestType.LSQ:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 Q";
					testType = ":MEAS:EQU-CCT SER";
					break;
				case ELCRTestType.LSRD:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 RDC";
					testType = ":MEAS:EQU-CCT SER";
					break;
				case ELCRTestType.LSRS:
					func1 = ":MEAS:FUNC1 L";
					func2 = ":MEAS:FUNC2 R";
					testType = ":MEAS:EQU-CCT SER";
					break;
				case ELCRTestType.GB:
					func1 = ":MEAS:FUNC1 G";
					func2 = ":MEAS:FUNC2 B";
					//testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.RX:
					func1 = ":MEAS:FUNC1 R";
					func2 = ":MEAS:FUNC2 X";
					//testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.YTD:
					func1 = ":MEAS:FUNC1 Y";
					func2 = ":MEAS:FUNC2 A";
					//testType = ":MEAS:EQU-CCT PAR";
					break;
				case ELCRTestType.ZTD:
					func1 = ":MEAS:FUNC1 Z";
					func2 = ":MEAS:FUNC2 A";
					//testType = ":MEAS:EQU-CCT PAR";
					break;
				default:
					return false;
			}

            switch (data.MsrtSpeed)
            {
                case ELCRMsrtSpeed.Long:
                    {
                        speed = ":MEAS:SPEED SLOW";

                        break;
                    }
                case ELCRMsrtSpeed.Medium:
                    {
                        speed = ":MEAS:SPEED MED";

                        break;
                    }
                case ELCRMsrtSpeed.Short:
                    {
                        speed = ":MEAS:SPEED FAST";

                        break;
                    }
				case ELCRMsrtSpeed.Max:
					{
						speed = ":MEAS:SPEED MAX";

						break;
					}
                default:
                    {
						speed = ":MEAS:SPEED SLOW";

                        break;
                    }
            }

			if (data.Range == 0)
			{
				range = ":MEAS:RANGE AUTO";
			}
			else if (data.Range < 7.99)
			{
				range = ":MEAS:RANGE 1";
			}
			else if (data.Range < 80)
			{
				range = ":MEAS:RANGE 2";
			}
			else if (data.Range < 692)
			{
				range = ":MEAS:RANGE 3";
			}
			else if (data.Range < 6920)
			{
				range = ":MEAS:RANGE 4";
			}
			else if (data.Range < 69200)
			{
				range = ":MEAS:RANGE 5";
			}
			else if (data.Range < 692000)
			{
				range = ":MEAS:RANGE 6";
			}
			else
			{
				range = ":MEAS:RANGE 7";
			}

            bool isChange = false;

            //this._device.SendCommand(":DISP-OFF");

            //System.Threading.Thread.Sleep(500);

            if (this._cmdList.Frequency != frequency)
            {
                isChange = true;

                this._cmdList.Frequency = frequency;

                this._device.SendCommand(frequency);
            }

            if (this._cmdList.Func1 != func1)
            {
                isChange = true;

                this._cmdList.Func1 = func1;

                this._device.SendCommand(func1);
            }

            if (this._cmdList.Func2 != func2)
            {
                isChange = true;

                this._cmdList.Func2 = func2;

                this._device.SendCommand(func2);
            }

            if (this._cmdList.TestType != testType)
            {
                isChange = true;

                this._cmdList.TestType = testType;

                this._device.SendCommand(testType);
            }

            if (this._cmdList.Speed != speed)
            {
                isChange = true;

                this._cmdList.Speed = speed;

                this._device.SendCommand(speed);
            }

            if (this._cmdList.Range != range)
            {
                isChange = true;

                this._cmdList.Range = range;

                this._device.SendCommand(range);
            }

            if (this._cmdList.SingleLevel != singleLevel)
            {
                isChange = true;

                this._cmdList.SingleLevel = singleLevel;

                this._device.SendCommand(singleLevel);
            }

            if (isChange)
            {
                //this._device.SendCommand(":DISP-OFF");

                //System.Threading.Thread.Sleep(100);

                //System.Threading.Thread.Sleep(500);
            }

            //this._device.SendCommand(":MEAS:BIAS ON");

            return true;
        }

		public bool MeterOutput(uint[] activateChannels, uint settingIndex)
		{
            LCRSettingData data = this._lcrSetting[settingIndex];

			System.Threading.Thread.Sleep((int)data.MsrtDelayTime);

			this._device.SendCommand(":MEAS:TRIG");

			string msg = string.Empty;

			this._device.WaitAndGetData(out msg);

			this.ParseMsrtData(msg, settingIndex);

			return true;
		}

		public double[] GetDataFromMeter(uint channel, uint settingIndex)
		{
			return this._msrtResult[settingIndex];
		}

		public void Close()
		{
			if (this._device != null)
			{
				this.TurnOff();

				if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal ||
					this._devSetting.LCRDCBiasType == ELCRDCBiasType.Ext_Master ||
					this._devSetting.LCRDCBiasType == ELCRDCBiasType.Ext_Other)
				{
					this._device.SendCommand(":MEAS:BIAS OFF");
				}

				this._device.Close();
			}
		}

		private class CmdData
		{
			public string Frequency { get; set; }
			public string SingleLevel { get; set; }
			public string Func1 { get; set; }
			public string Func2 { get; set; }
			public string TestType { get; set; }
			public string Speed { get; set; }
			public string Range { get; set; }
			public string DCBias { get; set; }
		}
	}
}
