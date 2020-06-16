using System;
using System.Collections.Generic;

using MPI.Tester.DeviceCommon;
using Ivi.Visa.Interop;

namespace MPI.Tester.Device.SourceMeter
{
	public class N5751A : ISourceMeter
	{
		private const int MAX_SETTING_MEMORY = 16;
		private const float MAX_VOLTAGE = 285.0f;
		private const float MAX_CURRENT = 2.5f;
		private bool _isEnableRegister = false;

		private string _serialNumber;
		private string _softwareVersion;
		private string _hardwareVersion;
		private double[][] _applyData;
		private double[][] _msrtResult;
		private EDevErrorNumber _errorNumber;
		private ElecDevSetting _elcDevSetting;
		private ElectSettingData[] _elcSetting;
		private IConnect _conn;

        private SourceMeterSpec _spec;

		public N5751A()
		{
			this._serialNumber = "SN NONE";
			this._softwareVersion = "SW NONE";
			this._hardwareVersion = "HW NONE";
			this._errorNumber = EDevErrorNumber.Device_NO_Error;
			this._elcDevSetting = new ElecDevSetting();

            this._spec = new SourceMeterSpec();
		}

		public N5751A(ElecDevSetting Setting)
			: this()
		{
			this._elcDevSetting = Setting;
		}

		#region >>> Private Method <<<

		private static string GetGPIBAddress(string GPIBChannel)
		{
			//GPIB0::GPIBChannel
			return "GPIB0::" + GPIBChannel;
		}

		private static string GetUSBAddress(string serialNumber)
		{
			//USB0::VendorID::ProductID::serialNumber
			return "USB0::0x0957::0x0807::" + serialNumber;
		}

		private static string GetTCPIPAddress(string IPAddress)
		{
			//TCPIP0::IP
			return "TCPIP0::" + IPAddress;
		}

		private bool IsHardwareError()
		{
            this._errorNumber = EDevErrorNumber.Device_NO_Error;

			//for H/W Error
			this._conn.SendCommand("SYSTem:ERRor?");

			string str = string.Empty;

			this._conn.WaitAndGetData(out str);

			string[] Msg = str.Split(',');

			int ErrorCode = 0;

			int.TryParse(Msg[0].Replace("+", ""), out ErrorCode);

			if (ErrorCode != 0)
			{
				Console.WriteLine("[N5700], IsHardwareError():" + str);

				this._errorNumber = EDevErrorNumber.SourceMeterDevice_HW_Err;

				return true;
			}

			//for OVP or OCP..
			this._conn.SendCommand("STATus:QUEStionable?");

			string state = string.Empty;

			this._conn.WaitAndGetData(out state);

			state.Replace("+", "").Replace("\n", "");

			int.TryParse(state, out ErrorCode);

			if (ErrorCode > 0 )
			{
				Console.WriteLine("[N5700], OVP or OCP, reset");

				this._errorNumber = EDevErrorNumber.SourceMeterDevice_HW_Err;

				this._conn.SendCommand("*CLS");

				this._conn.SendCommand("*RST");

				this._conn.SendCommand("OUTPut OFF");

				this._conn.SendCommand("VOLTage:PROTection:LEVel " + MAX_VOLTAGE.ToString());

				return true;
			}

			return false;
		}

		#endregion

		# region >>> Public Proberty <<<

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

		public EDevErrorNumber ErrorNumber
		{
			get { return this._errorNumber; }
		}

        public ElectSettingData[] ElecSetting
        {
            get
            {
                if (this._elcSetting == null)
                {
                    return null;
                }

                ElectSettingData[] data = new ElectSettingData[this._elcSetting.Length];

                for (int i = 0; i < this._elcSetting.Length; i++)
                {
                    data[i] = this._elcSetting[i].Clone() as ElectSettingData;
                }

                return data;
            }
        }

        public SourceMeterSpec Spec
        {
            get { return this._spec; }
        }

		#endregion

		#region >>> Public Method <<<

		public bool Init(int devNum, string sourceMeterSN)
		{
			try
			{
				Console.WriteLine("[N5700], Init()");

				string Address = String.Empty;

				switch (this._elcDevSetting.HWConnectorType)
				{
					case EHWConnectType.GPIB:
						//GPIB Channel
						Address = N5751A.GetGPIBAddress(sourceMeterSN);//"1");
						break;
					case EHWConnectType.USB:
						//Serial Number
						Address = N5751A.GetUSBAddress(sourceMeterSN);
						break;
					case EHWConnectType.TCPIP:
						//IP Address
						Address = N5751A.GetTCPIPAddress(sourceMeterSN);//"169.254.113.203");
						break;
					default:
						Address = N5751A.GetGPIBAddress(sourceMeterSN);//"1");
						break;
				}

				this._serialNumber = sourceMeterSN;

				this._conn = new IVIConnect(Address);

				string msg = string.Empty;

				this._conn.Open(out msg);

				this.Reset();

				this._conn.SendCommand("*IDN?");

				string sn = string.Empty;
					
				this._conn.WaitAndGetData(out sn);

				Console.WriteLine("[N5700], *IDN?:" + sn);

				string[] deviceInfo = sn.Split(',');

				this._serialNumber = deviceInfo[1] + "-" + deviceInfo[2];
				
				this._softwareVersion = deviceInfo[3];

				if (this.IsHardwareError())
				{
					return false;
				}

				return true;
			}
			catch(Exception e)
			{
				Console.WriteLine("[N5700], Init():" + e.ToString());

				this._errorNumber = EDevErrorNumber.SourceMeterDevice_Init_Err;

				return false;
			}
		}

		public void Close()
		{
			Console.WriteLine("[N5700], Close():");

			this.Reset();

			this._conn.Close();
		}

		public void Reset()
		{
			Console.WriteLine("[N5700], Reset():");

			this._conn.SendCommand("*CLS");

			this._conn.SendCommand("*RST");

			this._conn.SendCommand("OUTPut OFF");

			this._conn.SendCommand("VOLTage:PROTection:LEVel " + MAX_VOLTAGE.ToString());

			this._elcSetting = null;

			this._msrtResult = new double[MAX_SETTING_MEMORY][];

			for (int i = 0; i < this._msrtResult.Length; i++)
			{
				this._msrtResult[i] = new double[1];
			}
		}

		public bool SetConfigToMeter(ElecDevSetting devSetting)
		{
			return true;
		}

		public bool SetParamToMeter(ElectSettingData[] paramSetting)
		{
			Console.WriteLine("[N5700], SetParamToMeter():");

			this._errorNumber = EDevErrorNumber.Device_NO_Error;

			if (paramSetting.Length == 0)
			{
				this._errorNumber = EDevErrorNumber.NoSourceMeterParamSettingData;

				return false;
			}

			if (this._isEnableRegister && paramSetting.Length > MAX_SETTING_MEMORY)
			{
				return false;
			}

			//Check Setting value is out off Spec !?
			this._elcSetting = null;

			this._applyData = new double[paramSetting.Length][];

			for (int cnt = 0; cnt < paramSetting.Length; cnt++)
			{
				this._applyData[cnt] = new double[1] { paramSetting[cnt].ForceValue };

				paramSetting[cnt].MsrtRange = paramSetting[cnt].MsrtProtection;

				switch (paramSetting[cnt].MsrtType)
				{
					case EMsrtType.FVMI:
						{
							if (paramSetting[cnt].ForceValue > MAX_VOLTAGE || paramSetting[cnt].MsrtProtection > MAX_CURRENT)
							{
								this._errorNumber = EDevErrorNumber.ParameterSetting_Err;

								return false;
							}

							if (paramSetting[cnt].ForceValue < 0 || paramSetting[cnt].MsrtProtection < 0)
							{
								this._errorNumber = EDevErrorNumber.ParameterSetting_Err;

								return false;
							}

							break;
						}
					case EMsrtType.FIMV:
						{
							if (paramSetting[cnt].ForceValue > MAX_CURRENT || paramSetting[cnt].MsrtProtection > MAX_VOLTAGE)
							{
								this._errorNumber = EDevErrorNumber.ParameterSetting_Err;

								return false;
							}

							if (paramSetting[cnt].ForceValue < 0 || paramSetting[cnt].MsrtProtection < 0)
							{
								this._errorNumber = EDevErrorNumber.ParameterSetting_Err;

								return false;
							}

							break;
						}
					default:
						{
							break;
						}

				}
			}

			this.TurnOff();
	
			//Set Parameter To N5751A 
			if (this._isEnableRegister)
			{
				for (int cnt = 0; cnt < paramSetting.Length; cnt++)
				{

					switch (paramSetting[cnt].MsrtType)
					{
						case EMsrtType.FVMI:
							{
								this._conn.SendCommand("VOLTage " + Math.Abs(paramSetting[cnt].ForceValue).ToString());

								this._conn.SendCommand("CURRent " + Math.Abs(paramSetting[cnt].MsrtProtection).ToString());

								this._conn.SendCommand("*SAV " + cnt.ToString());
								break;
							}
						case EMsrtType.FIMV:
							{
								this._conn.SendCommand("VOLTage " + Math.Abs(paramSetting[cnt].MsrtProtection).ToString());

								this._conn.SendCommand("CURRent " + Math.Abs(paramSetting[cnt].ForceValue).ToString());

								this._conn.SendCommand("*SAV " + cnt.ToString());
								break;
							}
						default:
							{
								break;
							}
					}

					if (this.IsHardwareError())
					{
						return false;
					}
				}
			}

			this._elcSetting = paramSetting;

			return true;
		}

        public bool MeterOutput(uint[] activateChannels, uint settingIndex)
		{
			if (this._elcSetting == null)
			{
				this._errorNumber = EDevErrorNumber.NoSourceMeterParamSettingData;

				return false;
			}

			if (this._elcSetting.Length == 0)
			{
				this._errorNumber = EDevErrorNumber.NoSourceMeterParamSettingData;

				return false;
			}

			if (settingIndex > this._elcSetting.Length)
			{
				this._errorNumber = EDevErrorNumber.SourceMeterIndexSetting_Err;

				return false;
			}


			if (this._isEnableRegister)
			{
				this._conn.SendCommand("*RCL " + settingIndex);    // MEM start from 0 to 16
			}
			else
			{
				if (this._elcSetting[settingIndex].MsrtType == EMsrtType.FVMI)
				{
					this._conn.SendCommand("VOLTage " + Math.Abs(this._elcSetting[settingIndex].ForceValue).ToString());

					this._conn.SendCommand("CURRent " + Math.Abs(this._elcSetting[settingIndex].MsrtProtection).ToString());
				}
				else if (this._elcSetting[settingIndex].MsrtType == EMsrtType.FIMV)
				{
					this._conn.SendCommand("VOLTage " + Math.Abs(this._elcSetting[settingIndex].MsrtProtection).ToString());

					this._conn.SendCommand("CURRent " + Math.Abs(this._elcSetting[settingIndex].ForceValue).ToString());
				}
				else
				{
					Console.WriteLine("[N5700], MeterOutput(), EMsrtType Err, EMsrtType:" + this._elcSetting[settingIndex].MsrtType.ToString());

					this._errorNumber = EDevErrorNumber.SourceMeterIndexSetting_Err;

					return false;
				}
			}

			System.Threading.Thread.Sleep((int)this._elcSetting[settingIndex].ForceDelayTime);

			this._conn.SendCommand("OUTPut ON");

			System.Threading.Thread.Sleep((int)this._elcSetting[settingIndex].ForceTime);

			//Query Measure data
			if (this._elcSetting[settingIndex].MsrtType == EMsrtType.FIMV)
			{
				this._conn.SendCommand("MEASure:VOLTage?");
			}
			else if (this._elcSetting[settingIndex].MsrtType == EMsrtType.FVMI)
			{
				this._conn.SendCommand("MEASure:CURRent?");
			}

			string str = string.Empty;

			this._conn.WaitAndGetData(out str);

			double Data = 0.0d;

			if (!double.TryParse(str, out Data))
			{ 
				return false;
			}

			this._msrtResult[settingIndex][0] = Data;

			if (this._elcSetting[settingIndex].IsAutoTurnOff)
			{
				this._conn.SendCommand("OUTPut OFF");
			}

			if (this.IsHardwareError())
			{
				return false;
			}

			return true;
		}

        public bool MeterOutput(uint[] activateChannels, uint itemIndex, double applyValue)
        {
            return true;
        }

        public double[] GetDataFromMeter(uint channel, uint settingIndex)
		{
			return this._msrtResult[settingIndex];
		}

        public double[] GetApplyDataFromMeter(uint channel, uint itemIndex)
        {
			if (this._elcSetting == null || itemIndex > this._elcSetting.Length)
			{
				double[] data = new double[0];

				this._errorNumber = EDevErrorNumber.SourceMeterIndexSetting_Err;

				return data;
			}

			return this._applyData[itemIndex];
        }

        public double[] GetSweepPointFromMeter(uint channel, uint settingIndex)
		{
			throw new NotImplementedException();
		}

        public double[] GetSweepResultFromMeter(uint channel, uint itemIndex)
		{
			throw new NotImplementedException();
		}

        public double[] GetTimeChainFromMeter(uint channel, uint settingIndex)
		{
			throw new NotImplementedException();
		}

		public void TurnOff()
		{
			this._conn.SendCommand("OUTPut OFF");
		}

        public void TurnOff(double delay, bool isOpenRelay)
		{
			this._conn.SendCommand("OUTPut OFF");
		}

		public void Output(uint point, bool active)
		{
			throw new NotImplementedException();
		}

		public byte InputB(uint point)
		{
			return 0;
		}

		public byte Input(uint point)
		{
			throw new NotImplementedException();
		}

        public double GetPDDarkSample(int count)
        {
            return 0.0d;
        }

        public bool CheckInterLock()
        {
            return true;
        }

		#endregion

	}

}
