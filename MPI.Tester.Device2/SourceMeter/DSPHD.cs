using System;
using System.Collections.Generic;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SourceMeter
{
	public class DSPHD : ISourceMeter
	{
		private const int MAX_SETTING_MEMORY = 16;
		private float MAX_VOLTAGE = 300.0f;
		private float MAX_CURRENT = 2.5f;
		private bool _isEnableRegister = false;

		private string _serialNumber;
		private string _softwareVersion;
		private string _hardwareVersion;
		private double[][] _applyData;
		private double[][] _msrtResult;
		private IConnect _conn;
		private EDevErrorNumber _errorNumber;
		private ElecDevSetting _elcDevSetting;
		private ElectSettingData[] _elcSetting;

        private SourceMeterSpec _spec;

		public DSPHD()
		{
			this._serialNumber = "SN NONE";
			this._softwareVersion = "SW NONE";
			this._hardwareVersion = "HW NONE";
			this._errorNumber = EDevErrorNumber.Device_NO_Error;
			this._elcDevSetting = new ElecDevSetting();

            this._spec = new SourceMeterSpec();
		}

		public DSPHD(ElecDevSetting Setting)
			: this()
		{
			this._elcDevSetting = Setting;
		}

		#region >>> Private Method <<<

		private bool IsHardwareError()
		{
			this._errorNumber = EDevErrorNumber.Device_NO_Error;

			this.SendCommand("SYSTem:ERRor?");

			string ErrorMsg;

			this._conn.WaitAndGetData(out ErrorMsg);

			string[] Msg = ErrorMsg.Split(',');

			int ErrorCode = -1;

			if (!int.TryParse(Msg[0].Remove(1), out ErrorCode))
			{
				this._errorNumber = EDevErrorNumber.SourceMeterDevice_HW_Err;

				Console.WriteLine("[DSPHD], Error:" + ErrorMsg);

				return true;
			}

			if (ErrorCode != 0)
			{
				this._errorNumber = EDevErrorNumber.SourceMeterDevice_HW_Err;

				Console.WriteLine("[DSPHD], Error:" + ErrorMsg);

				this.SendCommand("SYSTem:REMOte");

				this.SendCommand("*CLS");

				this.SendCommand("*RST");

				this.SendCommand("OUTPut OFF");

				this.SendCommand("SOURce:VOLTage:PROTection:LEVel " + MAX_VOLTAGE.ToString());

				this.SendCommand("SOURce:CURRent:PROTection:LEVel " + MAX_CURRENT.ToString());

				return true;
			}

			return false;
		}

		private void SetDeviceSpec(string deviceName)
		{
			// deviceName: DSP-300-02.5HD

			string str = string.Empty;
			string voltage = string.Empty;
			string current = string.Empty;

			// str: 300-02.5HD

			if (deviceName.Contains("DSP-"))
			{
				str = deviceName.Replace("DSP-", "");
			}
			else if (deviceName.Contains("DSP_"))
			{
				str = deviceName.Replace("DSP_", "");
			}
            else if (deviceName.Contains("DSP"))
			{
				str = deviceName.Replace("DSP", "");
			}

			//voltage: 300
			if (str.Contains("-"))
			{
				voltage = str.Substring(0, str.IndexOf("-"));
			}
			else if(str.Contains("_"))
			{
				voltage = str.Substring(0, str.IndexOf("_"));
			}

			//str: -02.5HD
			if (str.Contains(voltage))
			{
				str = str.Replace(voltage, "");
			}

			//str: 02.5HD
			if (str.Contains("-"))
			{
				str = str.Replace("-", "");
			}
			else if (str.Contains("_"))
			{
				str = str.Replace("_", "");
			}

			//current: 02.5
			if (str.Contains("-HD"))
			{
				current = str.Replace("-HD", "");
			}
			else if (str.Contains("-HDB"))
			{
				current = str.Replace("-HDB", "");
			}
			else if (str.Contains("-HR"))
			{
				current = str.Replace("-HR", "");
			}
			if (str.Contains("_HD"))
			{
				current = str.Replace("_HD", "");
			}
			else if (str.Contains("_HDB"))
			{
				current = str.Replace("_HDB", "");
			}
			else if (str.Contains("_HR"))
			{
				current = str.Replace("_HR", "");
			}
            else if (str.Contains("HD"))
            {
                current = str.Replace("HD", "");
            }
            else if (str.Contains("HDB"))
            {
                current = str.Replace("HDB", "");
            }
            else if (str.Contains("HR"))
            {
                current = str.Replace("HR", "");
            }

			if(!float.TryParse(voltage, out this.MAX_VOLTAGE) || !float.TryParse(current, out this.MAX_CURRENT))
			{
				this.MAX_VOLTAGE = 0.0f;

				this.MAX_CURRENT = 0.0f;
			}
		 }

		private void SendCommand(string comm)
		{
			if (this._elcDevSetting.HWConnectorType == EHWConnectType.GPIB)
			{
				this._conn.SendCommand(comm);
			}
			else if (this._elcDevSetting.HWConnectorType == EHWConnectType.RS232)
			{
				this._conn.SendCommand("A001" + comm);
			}
		}

		#endregion

		#region >>> Public Proberty <<<

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
				Console.WriteLine("[DSPHD], Init()");

				string Address = String.Empty;

				switch (this._elcDevSetting.HWConnectorType)
				{
					case EHWConnectType.GPIB:
						this._conn = new GPIBConnect();
						break;
					case EHWConnectType.RS232:
						RS232SettingData rs232Setting = new RS232SettingData();
						rs232Setting.ComPortName = sourceMeterSN;
						rs232Setting.BaudRate = 115200;
						rs232Setting.DataBits = 8;
						rs232Setting.Parity = "None";
						rs232Setting.StopBits = 1;
						rs232Setting.TimeOut = 500;
						rs232Setting.Terminator = "\x0d";
						this._conn = new RS232Connect(rs232Setting);
						break;
					default:
						this._conn = new RS232Connect();
						break;
				}

				string info;

				this._conn.Open(out info);

				this.Reset();

				//Get Device Infomation
				this.SendCommand("*IDN?");

				this._conn.WaitAndGetData(out info);

				Console.WriteLine("[DSPHD], *IDN?:" + info);

				string[] deviceInfo = info.Split(',');

				this.SetDeviceSpec(deviceInfo[1]);

				this._serialNumber = deviceInfo[1] + "-" + deviceInfo[2];

				this._softwareVersion = deviceInfo[3];


				if (this.IsHardwareError())
				{
					return false;
				}

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("[DSP], Init():" + e.ToString());

				this._errorNumber = EDevErrorNumber.SourceMeterDevice_Init_Err;

				return false;
			}
		}

		public void Close()
		{
			Console.WriteLine("[DSP], Close()");

			this.Reset();

			this._conn.Close();
		}

		public void Reset()
		{
			Console.WriteLine("[DSP], Reset()");

			this.SendCommand("SYSTem:REMOte");
			
			this.SendCommand("*CLS");

			this.SendCommand("*RST");

			this.SendCommand("OUTPut OFF");

			//this.SendCommand("SYS:BEEP OFF");

			this.SendCommand("SOURce:VOLTage:PROTection:LEVel " + MAX_VOLTAGE.ToString());

			this.SendCommand("SOURce:CURRent:PROTection:LEVel " + MAX_CURRENT.ToString());

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
			Console.WriteLine("[DSPHD], SetParamToMeter()");

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

			//Set Parameter To Power Supply
			if (this._isEnableRegister)
			{
				for (int cnt = 0; cnt < paramSetting.Length; cnt++)
				{
					switch (paramSetting[cnt].MsrtType)
					{
						case EMsrtType.FVMI:
							{
								this.SendCommand("SOURce:MEMory:VOLTage:" + cnt.ToString() + " " + Math.Abs(paramSetting[cnt].ForceValue).ToString());
								
								this.SendCommand("SOURce:MEMory:CURRent:" + cnt.ToString() + " " + Math.Abs(paramSetting[cnt].MsrtProtection).ToString());
								
								break;
							}
						case EMsrtType.FIMV:
							{
								this.SendCommand("SOURce:MEMory:VOLTage:" + (cnt + 1).ToString() + " " + Math.Abs(paramSetting[cnt].MsrtProtection).ToString());
								
								this.SendCommand("SOURce:MEMory:CURRent:" + (cnt + 1).ToString() + " " + Math.Abs(paramSetting[cnt].ForceValue).ToString());
								
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

			System.Threading.Thread.Sleep((int)this._elcSetting[settingIndex].ForceDelayTime);
			//this.SendCommand("SOURce:VOLTage 0.001");
			//this.SendCommand("SOURce:CURRent 0.5");
			//this.SendCommand("OUTPut ON");
			//System.Threading.Thread.Sleep(0);

			if (this._isEnableRegister)
			{
				// MEM start from 0 to 16
				this.SendCommand("SOURce:MEMory:RECall " + (settingIndex + 1).ToString());
			}
			else
			{

				if (this._elcSetting[settingIndex].MsrtType == EMsrtType.FVMI)
				{
					this.SendCommand("SOURce:VOLTage " + Math.Abs(this._elcSetting[settingIndex].ForceValue).ToString());

					this.SendCommand("SOURce:CURRent " + Math.Abs(this._elcSetting[settingIndex].MsrtProtection).ToString());
				}
				else if (this._elcSetting[settingIndex].MsrtType == EMsrtType.FIMV)
				{
					this.SendCommand("SOURce:VOLTage " + Math.Abs(this._elcSetting[settingIndex].MsrtProtection).ToString());

					this.SendCommand("SOURce:CURRent " + Math.Abs(this._elcSetting[settingIndex].ForceValue).ToString());
				}
				else
				{
					Console.WriteLine("[DSP], MeterOutput(), EMsrtType Err, EMsrtType:" + this._elcSetting[settingIndex].MsrtType.ToString());

					this._errorNumber = EDevErrorNumber.SourceMeterIndexSetting_Err;

					return false;
				}
			}

			this.SendCommand("OUTPut ON");

			System.Threading.Thread.Sleep((int)this._elcSetting[settingIndex].ForceTime);

			string voltage = "";

			string current = "";

			string data;

			this.SendCommand("FETCh?");

			this._conn.WaitAndGetData(out data);

			voltage = data.Split(',')[0];

			current = data.Split(',')[1];

			if (this._elcSetting[settingIndex].MsrtType == EMsrtType.FVMI)
			{
				double.TryParse(current, out this._msrtResult[settingIndex][0]);
			}
			else if (this._elcSetting[settingIndex].MsrtType == EMsrtType.FIMV)
			{
				double.TryParse(voltage, out this._msrtResult[settingIndex][0]);
			}


			if (this._elcSetting[settingIndex].IsAutoTurnOff)
			{
				this.SendCommand("OUTPut OFF");
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

        public double[] GetApplyDataFromMeter(uint channel, uint settingIndex)
        {
			if (this._elcSetting == null || settingIndex > this._elcSetting.Length)
			{
				double[] data = new double[0];

				this._errorNumber = EDevErrorNumber.SourceMeterIndexSetting_Err;

				return data;
			}

			return this._applyData[settingIndex];
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
			this.SendCommand("OUTPut OFF");
		}

		public void TurnOff(double delay, bool isOpenRelay)
		{
			this.SendCommand("OUTPut OFF");
		}

		public void Output(uint point, bool active)
		{
			throw new NotImplementedException();
		}

		public byte InputB(uint point)
		{
			throw new NotImplementedException();
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
