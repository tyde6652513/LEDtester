using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;

namespace MPI.Tester.Device.LCRMeter
{
    public class WK6500 : LCRBase 
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
		private CmdData _cmdData;

		public WK6500()
		{
			this._devSetting = new LCRDevSetting();

			this._lcrSetting = null;

			this._msrtResult = null;

			this._errorCode = EDevErrorNumber.Device_NO_Error;

			this._serialNumber = string.Empty;

			this._softwareVersion = string.Empty;

			this._hardwareVersion = string.Empty;

			this._cmdData = new CmdData();
		}

		public WK6500(LCRDevSetting Setting)
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

			this._spec.IsProvideSignalLevelI = true;

			this._spec.IsProvideDCBiasV = false;

			this._spec.IsProvideDCBiasI = true;

			this._spec.SignalLevelVMin = 0.01;

			this._spec.SignalLevelVMax = 1;

			this._spec.SignalLevelIMin = 200e-6;

			this._spec.SignalLevelIMax = 20e-3;

			this._spec.FrequencyMin = 20;

			this._spec.FrequencyMax = 120e6;

			this._spec.DCBiasVMin = 0;

			this._spec.DCBiasVMax = 0;

			this._spec.DCBiasIMin = 0;

			this._spec.DCBiasIMax = 0.1;

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Long);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Medium);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Short);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Max);

			foreach (var item in Enum.GetNames(typeof(ELCRTestType)))
			{
				ELCRTestType type = (ELCRTestType)Enum.Parse(typeof(ELCRTestType), item);

				if (type == ELCRTestType.VDID ||
					type == ELCRTestType.YTR ||
					type == ELCRTestType.ZTR ||
					type == ELCRTestType.LPRD ||
					type == ELCRTestType.LSRD)
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

		private void SendCommand(string cmd)
		{
			if (this._device is LANConnect)
			{
				(this._device as LANConnect).SendCommand(cmd, false);
			}
			else
			{
				this._device.SendCommand(cmd);
			}

			System.Threading.Thread.Sleep(30);
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

				lanData.Port = 2000;

				lanData.IPAddress = sourceMeterSN;

				this._device = new LANConnect(lanData);
			}
			else
			{
				//string visaAddress = "TCPIP0::" + sourceMeterSN + "::inst0::INSTR";
				//string visaAddress = "GPIB0::" + sourceMeterSN + "::INSTR";
				//string visaAddress = "USB0::0x0B6A::0x5346::" + sourceMeterSN + "::INSTR";

				this._device = new IVIConnect(sourceMeterSN);
			}

			string msg = string.Empty;

			if (!this._device.Open(out msg))
			{
				this._errorCode = EDevErrorNumber.LCRInitFail;

				return false; 
			}

			this.SendCommand("*IDN?");

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
				this.SendCommand(":METER:BIAS 0");

				this.SendCommand(":METER:BIAS-TYPE CUR");

				this.SendCommand(":METER:BIAS-STAT ON");
			}			

			return true;
		}

		public void TurnOff()
		{
			if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
			{
				this.SendCommand(":METER:BIAS 0");

				this._cmdData.Bias = ":METER:BIAS 0";
			}

			this._cmdData.SingleLevel = ":METER:LEVEL 0";

			this.SendCommand(":METER:LEVEL 0");
		}

		public bool SetConfigToMeter(LCRDevSetting devSetting)
		{
			this._devSetting = devSetting;

			return true;
		}

		public bool PreSettingParamToMeter(uint settingIndex)
		{
			if (this._lcrSetting == null || this._lcrSetting.Length == 0)
			{
				return false;
			}

			LCRSettingData data = this._lcrSetting[settingIndex];

			System.Threading.Thread.Sleep((int)data.MsrtDelayTime);

			string frequency = "FREQ " + data.Frequency;

			string singleLevel = string.Empty;

			string func1 = string.Empty;

			string func2 = string.Empty;

			string testType = string.Empty;

			string speed = string.Empty;

			string bias = ":METER:BIAS " + data.DCBiasI;

			string range = string.Empty;

			if (data.SignalMode == ELCRSignalMode.Voltage)
			{
				singleLevel = ":METER:LEVEL " + data.SignalLevelV + "V";
			}
			else
			{
				singleLevel = ":METER:LEVEL " + data.SignalLevelI + "A";
			}

            switch (data.LCRMsrtType)
			{
				case ELCRTestType.CPD:
					func1 = ":METER:FUNC:1 C";
					func2 = ":METER:FUNC:2 D";
					testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.CPG:
					func1 = ":METER:FUNC:1 C";
					func2 = ":METER:FUNC:2 G";
					testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.CPQ:
					func1 = ":METER:FUNC:1 C";
					func2 = ":METER:FUNC:2 Q";
					testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.CPRP:
					func1 = ":METER:FUNC:1 C";
					func2 = ":METER:FUNC:2 R";
					testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.CSD:
					func1 = ":METER:FUNC:1 C";
					func2 = ":METER:FUNC:2 D";
					testType = ":METER:EQU-CCT SER";
					break;
				case ELCRTestType.CSQ:
					func1 = ":METER:FUNC:1 C";
					func2 = ":METER:FUNC:2 Q";
					testType = ":METER:EQU-CCT SER";
					break;
				case ELCRTestType.CSRS:
					func1 = ":METER:FUNC:1 C";
					func2 = ":METER:FUNC:2 R";
					testType = ":METER:EQU-CCT SER";
					break;
				case ELCRTestType.LPD:
					func1 = ":METER:FUNC:1 L";
					func2 = ":METER:FUNC:2 D";
					testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.LPG:
					func1 = ":METER:FUNC:1 L";
					func2 = ":METER:FUNC:2 G";
					testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.LPQ:
					func1 = ":METER:FUNC:1 L";
					func2 = ":METER:FUNC:2 Q";
					testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.LPRP:
					func1 = ":METER:FUNC:1 L";
					func2 = ":METER:FUNC:2 R";
					testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.LSD:
					func1 = ":METER:FUNC:1 L";
					func2 = ":METER:FUNC:2 D";
					testType = ":METER:EQU-CCT SER";
					break;
				case ELCRTestType.LSQ:
					func1 = ":METER:FUNC:1 L";
					func2 = ":METER:FUNC:2 Q";
					testType = ":METER:EQU-CCT SER";
					break;
				case ELCRTestType.LSRS:
					func1 = ":METER:FUNC:1 L";
					func2 = ":METER:FUNC:2 R";
					testType = ":METER:EQU-CCT SER";
					break;
				case ELCRTestType.GB:
					func1 = ":METER:FUNC:1 G";
					func2 = ":METER:FUNC:2 B";
					//testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.RX:
					func1 = ":METER:FUNC:1 R";
					func2 = ":METER:FUNC:2 X";
					//testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.YTD:
					func1 = ":METER:FUNC:1 Y";
					func2 = ":METER:FUNC:2 ANGLE";
					//testType = ":METER:EQU-CCT PAR";
					break;
				case ELCRTestType.ZTD:
					func1 = ":METER:FUNC:1 Z";
					func2 = ":METER:FUNC:2 ANGLE";
					//testType = ":METER:EQU-CCT PAR";
					break;
				default:
					return false;
			}

			switch (data.MsrtSpeed)
			{
				case ELCRMsrtSpeed.Long:
					{
						speed = ":METER:SPEED SLOW";

						break;
					}
				case ELCRMsrtSpeed.Medium:
					{
						speed = ":METER:SPEED MED";

						break;
					}
				case ELCRMsrtSpeed.Short:
					{
						speed = ":METER:SPEED FAST";

						break;
					}
				case ELCRMsrtSpeed.Max:
					{
						speed = ":METER:SPEED MAX";

						break;
					}
				default:
					{
						speed = ":METER:SPEED SLOW";

						break;
					}
			}

			if (data.Range == 0)
			{
				range = ":METER:RANGE AUTO";
			}
			else if (data.Range < 5)
			{
				range = ":METER:RANGE 1";
			}
			else if (data.Range < 50)
			{
				range = ":METER:RANGE 2";
			}
			else if (data.Range < 500)
			{
				range = ":METER:RANGE 3";
			}
			else if (data.Range < 5000 && data.Frequency >= 1000000)
			{
				range = ":METER:RANGE 4";
			}
			else if (data.Range < 50000 && data.Frequency >= 100000)
			{
				range = ":METER:RANGE 5";
			}
			else if (data.Range < 500000 && data.Frequency >= 10000)
			{
				range = ":METER:RANGE 6";
			}
			else
			{
				range = "RANGE AUTO";
			}

			if (this._cmdData.Frequency != frequency)
			{
				this._cmdData.Frequency = frequency;

				this.SendCommand(frequency);
			}

			if (this._cmdData.SingleLevel != singleLevel)
			{
				this._cmdData.SingleLevel = singleLevel;

				this.SendCommand(singleLevel);
			}

			if (this._cmdData.Func1 != func1)
			{
				this._cmdData.Func1 = func1;

				this.SendCommand(func1);
			}

			if (this._cmdData.Func2 != func2)
			{
				this._cmdData.Func2 = func2;

				this.SendCommand(func2);
			}

			if (this._cmdData.TestType != testType)
			{
				this._cmdData.TestType = testType;

				this.SendCommand(testType);
			}

			if (this._cmdData.Speed != speed)
			{
				this._cmdData.Speed = speed;

				this.SendCommand(speed);
			}

			if (this._cmdData.Range != range)
			{
				this._cmdData.Range = range;

				this.SendCommand(range);
			}

			if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal && this._cmdData.Bias != bias)
			{
				this._cmdData.Bias = bias;

				this.SendCommand(bias);
			}

			return true;
		}

		public bool SetParamToMeter(LCRSettingData[] paramSetting)
		{
			this._errorCode = EDevErrorNumber.Device_NO_Error;

			if (paramSetting == null || paramSetting.Length == 0)
			{
				return true;
			}

			int msrtLen = Enum.GetNames(typeof(ELCRMsrtType)).Length;

			this._msrtResult = new double[paramSetting.Length][];

			for (int i = 0; i < paramSetting.Length; i++)
			{
				LCRSettingData item = paramSetting[i];

				if (item.SignalMode == ELCRSignalMode.Voltage)
				{
					if (item.Frequency < 50e6)
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
						if (item.SignalLevelV < this._spec.SignalLevelVMin || item.SignalLevelV > 0.5)
						{
							this._lcrSetting = null;

							this._msrtResult = null;

							this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

							return false;
						}
					}
				}
				else
				{
					if (item.Frequency < 50e6)
					{
						if (item.SignalLevelI < this._spec.SignalLevelIMin || item.SignalLevelI > this._spec.SignalLevelIMax)
						{
							this._lcrSetting = null;

							this._msrtResult = null;

							this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

							return false;
						}
					}
					else
					{
						if (item.SignalLevelI < this._spec.SignalLevelIMin || item.SignalLevelI > 0.01)
						{
							this._lcrSetting = null;

							this._msrtResult = null;

							this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

							return false;
						}
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

		public bool MeterOutput(uint[] activateChannels, uint settingIndex)
		{
			LCRSettingData data = this._lcrSetting[settingIndex];

			System.Threading.Thread.Sleep((int)data.MsrtDelayTime);

			this.SendCommand(":METER:TRIG");

			string msg = string.Empty;

			if (!this._device.WaitAndGetData(out msg))
			{
				this._errorCode = EDevErrorNumber.LCRAcquireDataTimeout_Err;

				return false;
			}

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

				if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
				{
					this.SendCommand("BIAS-STATe OFF");
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
			public string Bias { get; set; }
		}
	}
}
