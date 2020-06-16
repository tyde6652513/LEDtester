using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SourceMeter
{
	public class IT7321 : ISourceMeter
	{
		private const int MAX_SETTING_MEMORY = 72;
		private float MAX_VOLTAGE = 300.0f;
		private float MIN_FREQUENCY = 45.0f;
		private float MAX_FREQUENCY = 500.0f;
		private bool _isEnableRegister = false;

		private string _serialNumber;
		private string _softwareVersion;
		private string _hardwareVersion;
		private double[][] _applyData;
		private List<double[]> _msrtResult;
		private IConnect _conn;
		private EDevErrorNumber _errorNumber;
		private ElecDevSetting _elcDevSetting;
		private ElectSettingData[] _elcSetting;

        private SourceMeterSpec _spec;

		public IT7321()
		{
			this._serialNumber = "SN NONE";

			this._softwareVersion = "SW NONE";

			this._hardwareVersion = "HW NONE";

			this._errorNumber = EDevErrorNumber.Device_NO_Error;

			this._elcDevSetting = new ElecDevSetting();

			this._msrtResult = new List<double[]>();

            this._spec = new SourceMeterSpec();
		}

		public IT7321(ElecDevSetting Setting)
			: this()
		{
			this._elcDevSetting = Setting;
		}

		#region >>> Private Method <<<

		private bool IsHardwareError()
		{
			this._errorNumber = EDevErrorNumber.Device_NO_Error;

			this._conn.SendCommand("SYSTem:ERRor?");

			string ErrorMsg;

			this._conn.WaitAndGetData(out ErrorMsg);

			string[] Msg = ErrorMsg.Split(',');

			int ErrorCode = -1;

			if (!int.TryParse(Msg[0], out ErrorCode))
			{
				this._errorNumber = EDevErrorNumber.SourceMeterDevice_HW_Err;

                Console.WriteLine("[IT7321], Error:" + Msg[0]);

				return true;
			}

			if (ErrorCode != 0)
			{
				this._errorNumber = EDevErrorNumber.SourceMeterDevice_HW_Err;

                Console.WriteLine("[IT7321], Error:" + ErrorCode);

				return true;
			}

  			return false;
		}

        private bool IsStateError()
        {
            /////////////////////////////////////////////
            // Query State
            /////////////////////////////////////////////
            System.Threading.Thread.Sleep(50);

            this._conn.SendCommand("STATus:QUEStionable:CONDition?");

            string state = string.Empty;

            this._conn.WaitAndGetData(out state);

            int ErrorCode = -1;

            if (!int.TryParse(state[0].ToString(), out ErrorCode))
            {
                this._errorNumber = EDevErrorNumber.SourceMeterDevice_HW_Err;

                Console.WriteLine("[IT7321], Error:" + state);

                return true;
            }

            if (ErrorCode != 0)
            {
                //PEAK OC: 1
                //RMS OC : 2
                //OV     : 3
                //OP     : 4
                //OT     : 5

                this._errorNumber = EDevErrorNumber.SourceMeterDevice_HW_Err;

                Console.WriteLine("[IT7321], Error:" + ErrorCode);

                this._conn.SendCommand("OUTPut ON");

                return true;
            }

            return false;
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
				Console.WriteLine("[IT7321], Init()");

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
					case EHWConnectType.USB:
						//USB0::0xFFFF::0x7300::0123456789AF::0::INSTR
						string sn = "USB0::0xFFFF::0x7300::" + sourceMeterSN + "::0::INSTR";
						this._conn = new IVIConnect(sn);
						break;
					default:
						this._conn = new RS232Connect();
						break;
				}

				string info;

				this._conn.Open(out info);

				this.Reset();

				this._conn.SendCommand("*IDN?");

				this._conn.WaitAndGetData(out info);

				Console.WriteLine("[IT7321], *IDN?:" + info);

				string[] deviceInfo = info.Split(',');

				this._serialNumber = deviceInfo[1] + "-" + deviceInfo[2];

				this._softwareVersion = deviceInfo[3];

				if (this.IsHardwareError())
				{
					return false;
				}

				return true;
			}
			catch
			{
				this._errorNumber = EDevErrorNumber.SourceMeterDevice_Init_Err;

				return false;
			}
		}

		public void Close()
		{
			this.Reset();

			this._conn.Close();
		}

		public void Reset()
		{
			this._conn.SendCommand("*CLS");

			this._conn.SendCommand("*RST");

			this._conn.SendCommand("SYSTem:BEEPer OFF");

			this._conn.SendCommand("SYSTem:REMote");

			this._conn.SendCommand("SOURce:RANGe AUTO");

			this._conn.SendCommand("OUTPut OFF");

            //IMMediate: 200mS~300sM Protect ON
            //DELay    : 1000mS      Protect ON
            this._conn.SendCommand("CONF:PROT:CURR:RMS:MODe IMMediate");

			this._elcSetting = null;

			this._msrtResult.Clear();
		}

		public bool SetConfigToMeter(ElecDevSetting devSetting)
		{
			return true;
		}

		public bool SetParamToMeter(ElectSettingData[] paramSetting)
		{
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
							if (paramSetting[cnt].ForceValue > MAX_VOLTAGE)
							{
								this._errorNumber = EDevErrorNumber.ParameterSetting_Err;

								return false;
							}

							if (paramSetting[cnt].ForceValueFrequency > MAX_FREQUENCY || paramSetting[cnt].ForceValueFrequency < MIN_FREQUENCY)
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
					this._conn.SendCommand("SOURce:VOLTage " + paramSetting[cnt].ForceValue.ToString());

					this._conn.SendCommand("SOURce:FREQuency " + paramSetting[cnt].ForceValueFrequency.ToString());

					this._conn.SendCommand("SOURce:PHASe:STARt 0");

					this._conn.SendCommand("*SAV " + (cnt + 1).ToString());

					if (this.IsHardwareError())
					{
						return false;
					}
				}
			}

			this._elcSetting = paramSetting;

			for (int i = 0; i < this._elcSetting.Length; i++)
			{
				// (1) Current
				// (2) Power
				// (3) Apparent
				// (4) Power Factor
				// (5) Frequency
				// (6) Current Peak 
				// (7) Current Peak Max
				// (8) Force Voltage
				this._msrtResult.Add(new double[8]);
			}

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

			if (this._isEnableRegister)
			{
				// MEM start from 0 to 16
				this._conn.SendCommand("*RCL " + (settingIndex + 1).ToString());
			}
			else
			{
				this._conn.SendCommand("SOURce:VOLTage " + this._elcSetting[settingIndex].ForceValue.ToString());

				this._conn.SendCommand("SOURce:FREQuency " + this._elcSetting[settingIndex].ForceValueFrequency.ToString());

                this._conn.SendCommand("CONF:PROT:CURR:RMS " + (this._elcSetting[settingIndex].MsrtProtection).ToString());

				this._conn.SendCommand("SOURce:PHASe:STARt 0");
			}

			this._conn.SendCommand("OUTPut ON");

			System.Threading.Thread.Sleep((int)this._elcSetting[settingIndex].ForceTime);

			if (this.IsStateError())
			{
				return false;
			}

			string data = string.Empty;

			// (1) Current
			this._conn.SendCommand("MEASure:SCALar:CURRent:AC?");

			this._conn.WaitAndGetData(out data);

			if (!double.TryParse(data, out this._msrtResult[(int)settingIndex][0]))
			{
				this._errorNumber = EDevErrorNumber.MeterOutput_Ctrl_Err;

				return false;
			}

			// (2) Power
			this._conn.SendCommand("MEASure:SCALar:POWer:AC:REAL?");

			this._conn.WaitAndGetData(out data);

			if (!double.TryParse(data, out this._msrtResult[(int)settingIndex][1]))
			{
				this._errorNumber = EDevErrorNumber.MeterOutput_Ctrl_Err;

				return false;
			}

			// (3) Apparent
			this._conn.SendCommand("MEASure:SCALar:POWer:AC:APParent?");

			this._conn.WaitAndGetData(out data);

			if (!double.TryParse(data, out this._msrtResult[(int)settingIndex][2]))
			{
				this._errorNumber = EDevErrorNumber.MeterOutput_Ctrl_Err;

				return false;
			}

			// (4) Power Factor
			this._conn.SendCommand("MEASure:SCALar:POWer:AC:PFACtor?");

			this._conn.WaitAndGetData(out data);

			if (!double.TryParse(data, out this._msrtResult[(int)settingIndex][3]))
			{
				this._errorNumber = EDevErrorNumber.MeterOutput_Ctrl_Err;

				return false;
			}

			// (5) Frequency
			this._conn.SendCommand("MEASure:SCALar:FREQuency?");

			this._conn.WaitAndGetData(out data);

			if (!double.TryParse(data, out this._msrtResult[(int)settingIndex][4]))
			{
				this._errorNumber = EDevErrorNumber.MeterOutput_Ctrl_Err;

				return false;
			}

            // (6) Current Peak 
            this._conn.SendCommand("MEASure:SCALar:CURRent:AC:PEAK?");

            this._conn.WaitAndGetData(out data);

            if (!double.TryParse(data, out this._msrtResult[(int)settingIndex][5]))
            {
                this._errorNumber = EDevErrorNumber.MeterOutput_Ctrl_Err;

                return false;
            }

            // (7) Current Peak Max
            this._conn.SendCommand("MEASure:SCALar:CURRent:AC:PEAK:MAXimum?");

            this._conn.WaitAndGetData(out data);

            if (!double.TryParse(data, out this._msrtResult[(int)settingIndex][6]))
            {
                this._errorNumber = EDevErrorNumber.MeterOutput_Ctrl_Err;

                return false;
            }

			// (8) Voltage
			this._conn.SendCommand("MEASure:SCALar:VOLTage:AC?");

			this._conn.WaitAndGetData(out data);

			if (!double.TryParse(data, out this._msrtResult[(int)settingIndex][7]))
			{
				this._errorNumber = EDevErrorNumber.MeterOutput_Ctrl_Err;

				return false;
			}

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
			return this._msrtResult[(int)settingIndex];
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
