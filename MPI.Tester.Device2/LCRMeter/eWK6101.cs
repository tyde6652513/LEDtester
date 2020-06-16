using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;

namespace MPI.Tester.Device.LCRMeter
{
    public class eWK6101 : LCRBase 
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

		public eWK6101()
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


		public eWK6101(LCRDevSetting Setting)
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

			this._spec.IsProvideDCBiasV = true;

			this._spec.IsProvideDCBiasI = true;

			this._spec.SignalLevelVMin = 0.005;

			this._spec.SignalLevelVMax = 10;

			this._spec.SignalLevelIMin = 0.00005;

			this._spec.SignalLevelIMax = 0.1;

			this._spec.FrequencyMin = 20;

			this._spec.FrequencyMax = 1e6;

			this._spec.DCBiasVMin = -10;

			this._spec.DCBiasVMax = 10;

			this._spec.DCBiasIMin = -0.1;

			this._spec.DCBiasIMax = 0.1;

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Long);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Medium);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Short);

			foreach (var item in Enum.GetNames(typeof(ELCRTestType)))
			{
				ELCRTestType type = (ELCRTestType)Enum.Parse(typeof(ELCRTestType), item);

				if (type == ELCRTestType.LPRD ||
					type == ELCRTestType.LSRD ||
					type == ELCRTestType.VDID)
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

				this._device = new LANConnect(lanData);
			}
			else
			{
				//"TCPIP0::" + sourceMeterSN + "::inst0::INSTR";

				//"USB0::0x0471::0x0827::6&235f1307&0&2::0::INSTR"

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

			this._device.SendCommand("*RST;*CLS");

			this._device.SendCommand("SYST:KLOCK ON");

			this._device.SendCommand(":TRIGger:SOURce BUS");			

			this._device.SendCommand("VOLT 0.01");

			this._device.SendCommand(":BIAS:VOLTage 0");

			return true;
		}

		public void TurnOff()
		{
			if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
			{
				this._device.SendCommand(":BIAS:VOLTage 0");

				this._cmdData.Bias = ":BIAS:VOLTage 0";
			}

			this._device.SendCommand("VOLT 0.01");

			this._cmdData.SingleLevel = "VOLT 0.01";
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

			string singleLevel = "VOLT " + data.SignalLevelV;

			string bias = ":BIAS:VOLTage " + data.DCBiasV;

			string msrtType = "FUNC:IMP " + data.MsrtType.ToString();

			string msrtSpeed = string.Empty;

			switch (data.MsrtSpeed)
			{
				case ELCRMsrtSpeed.Long:
				case ELCRMsrtSpeed.Medium:
				case ELCRMsrtSpeed.Short:
					{
						msrtSpeed = "APER " + data.MsrtSpeed.ToString();

						break;
					}
				default:
					{
						msrtSpeed = "APER Long";

						break;
					}
			}

			string msrtRange = "FUNC:IMP:RANG " + data.Range.ToString();

			if (data.Range == 0)
			{
				msrtRange = "FUNC:IMP:RANG:AUTO ON";
			}

			if (this._cmdData.Frequency != frequency)
			{
				this._cmdData.Frequency = frequency;

				this._device.SendCommand(frequency);
			}

			if (this._cmdData.SingleLevel != singleLevel)
			{
				this._cmdData.SingleLevel = singleLevel;

				this._device.SendCommand(singleLevel);
			}

			if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal && this._cmdData.Bias != bias)
			{
				this._cmdData.Bias = bias;

				this._device.SendCommand(bias);
			}

			if (this._cmdData.TestType != msrtType)
			{
				this._cmdData.TestType = msrtType;

				this._device.SendCommand(msrtType);
			}

			if (this._cmdData.Speed != msrtSpeed)
			{
				this._cmdData.Speed = msrtSpeed;

				this._device.SendCommand(msrtSpeed);
			}

			if (this._cmdData.Range != msrtRange)
			{
				this._cmdData.Range = msrtRange;

				this._device.SendCommand(msrtRange);
			}

			if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
			{
				this._device.SendCommand(":BIAS:STATe ON");
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

		public bool MeterOutput(uint[] activateChannels, uint settingIndex)
		{
			LCRSettingData data = this._lcrSetting[settingIndex];

			System.Threading.Thread.Sleep((int)data.MsrtDelayTime);

			this._device.SendCommand(":TRIGger");

			System.Threading.Thread.Sleep(150);

			this._device.SendCommand("FETCH?");

			string msg = string.Empty;

			this._device.WaitAndGetData(out msg);

			this.ParseMsrtData(msg, settingIndex);

			if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
			{
				this._device.SendCommand(":BIAS:STATe OFF");
			}

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
					this._device.SendCommand(":BIAS:STATe OFF");
				}

				this._device.Close();
			}
		}

		private class CmdData
		{
			public string Frequency { get; set; }
			public string SingleLevel { get; set; }
			public string Bias { get; set; }
			public string TestType { get; set; }
			public string Speed { get; set; }
			public string Range { get; set; }
		}
	}
}
