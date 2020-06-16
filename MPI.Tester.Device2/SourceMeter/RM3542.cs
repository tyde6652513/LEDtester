using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SourceMeter
{
	public class RM3542 : ISourceMeter
	{
		private IConnect _conn;
		private int _maxSyncSmuCount;

		private string _serialNumber;
		private string _softwareVersion;
		private string _hardwareVersion;
		private SourceMeterSpec _spec;
		private EDevErrorNumber _errorNumber;
		private ElectSettingData[] _elcSetting;

		private List<double[][]> _applyData;
		private List<double[][]> _acquireData;


		public RM3542()
		{
			this._maxSyncSmuCount = 1;

			this._serialNumber = string.Empty;

			this._softwareVersion = string.Empty;

			this._hardwareVersion = string.Empty;

			this._errorNumber = EDevErrorNumber.Device_NO_Error;

			this._acquireData = new List<double[][]>();

			this._applyData = new List<double[][]>();
		}

		public RM3542(ElecDevSetting Setting)
			: this()
		{

		}

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

		public bool Init(int deviceNum, string sourceMeterSN)
		{
			RS232SettingData data = new RS232SettingData();

			data.ComPortName = sourceMeterSN;

			data.BaudRate = 38400;

			data.DataBits = 8;

			data.Parity = "None";

			data.StopBits = 1;

			data.TimeOut = 500;

			data.Terminator = "\x0d";

			string msg = "";

			this._conn = new RS232Connect(data);

			if (!this._conn.Open(out msg))
			{
				this._errorNumber = EDevErrorNumber.SourceMeterDevice_Init_Err;

				return false;
			}

			this._conn.SendCommand("*IDN?");

			this._conn.WaitAndGetData(out msg);

			string[] info = msg.Split(',');

			if (info.Length < 4)
			{
				this._errorNumber = EDevErrorNumber.SourceMeterDevice_Init_Err;

				return false;
			}

			this._serialNumber = "N/A";

			this._hardwareVersion = info[1];

			this._softwareVersion = info[3];

			this.Reset();

			return true;
		}

		public bool SetConfigToMeter(ElecDevSetting devSetting)
		{
			return true;
		}

		public bool SetParamToMeter(ElectSettingData[] paramSetting)
		{
			this._errorNumber = EDevErrorNumber.Device_NO_Error;

			this._applyData.Clear();

			this._acquireData.Clear();

			if (paramSetting.Length == 0)
			{
				return false;
			}

			this._elcSetting = paramSetting;

			for (uint deviceID = 0; deviceID < this._maxSyncSmuCount; deviceID++)
			{
				this._applyData.Add(new double[paramSetting.Length][]);

				this._acquireData.Add(new double[paramSetting.Length][]);
			}

			double MAX_RANGE = 100e6;

			double MIN_RANGE = 100e-3;

			for (uint deviceID = 0; deviceID < this._maxSyncSmuCount; deviceID++)
			{
				for (int index = 0; index < paramSetting.Length; index++)
				{
					this._applyData[(int)deviceID][index] = new double[1] { this._elcSetting[index].MsrtRTestItemRange };

					switch (paramSetting[index].MsrtType)
					{
						case EMsrtType.R:
							{
								this._acquireData[(int)deviceID][index] = new double[1];

								if (paramSetting[index].MsrtRTestItemRange > MAX_RANGE || paramSetting[index].MsrtRTestItemRange < MIN_RANGE)
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
			}

			return true;
		}

		public bool MeterOutput(uint[] activateChannels, uint settingIndex)
		{
			ElectSettingData item = this._elcSetting[settingIndex];

			this._conn.SendCommand(":RESISTANCE:RANGe " + item.MsrtRTestItemRange);

			if (item.MsrtRTestItemSpeed == ERTestItemMsrtSpeed.FAST)
			{
				this._conn.SendCommand(":SPEEd FAST");
			}
			else if (item.MsrtRTestItemSpeed == ERTestItemMsrtSpeed.SLOW)
			{
				this._conn.SendCommand(":SPEEd SLOW");
			}
			else
			{
				this._conn.SendCommand(":SPEEd MEDIUM");
			}

			this._conn.SendCommand(":READ?");

			// 1: CE Lo, 2: CE Hi, 4: CURR, 8: VOLT
			//this._conn.SendCommand(":ESR1?");

			return true;
		}

        public bool MeterOutput(uint[] activateChannels, uint itemIndex, double applyValue)
        {
            return true;
        }

		public double[] GetDataFromMeter(uint channel, uint settingIndex)
		{
			if (settingIndex > this._elcSetting.Length - 1 || this._elcSetting == null)
			{
				return null;
			}

			string data = string.Empty;

			if (this._conn.WaitAndGetData(out data))
			{
				Double.TryParse(data, out this._acquireData[(int)channel][settingIndex][0]);
			}
			else
			{
				this._errorNumber = EDevErrorNumber.SourceMeterAcquireDataTimeout_Err;
			}

			return this._acquireData[(int)channel][settingIndex];
		}

		public double[] GetApplyDataFromMeter(uint channel, uint settingIndex)
		{
			if (this._elcSetting == null || settingIndex > this._elcSetting.Length)
			{
				double[] data = new double[0];

				this._errorNumber = EDevErrorNumber.SourceMeterIndexSetting_Err;

				return data;
			}

			return this._applyData[(int)channel][settingIndex];
		}

		public double[] GetSweepPointFromMeter(uint channel, uint index)
		{
			if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
			{
				return null;
			}

			return this._elcSetting[index].SweepCustomValue;
		}

		public double[] GetSweepResultFromMeter(uint channel, uint index)
		{
			return null;
		}

		public double[] GetTimeChainFromMeter(uint channel, uint index)
		{
			return null;
		}

		public double GetPDDarkSample(int count)
		{
			return 0;
		}

		public void TurnOff(double delay, bool isOpenRelay)
		{
			return;
		}

		public void TurnOff()
		{
			return;
		}

		public void Reset()
		{
			this._conn.SendCommand("*RST");

			foreach (var range in Enum.GetNames(typeof(ERM2542Range)))
			{
				this._conn.SendCommand(":RESistance:CIMProve " + range.ToString() + ",OFF");

				this._conn.SendCommand(":RESistance:CONTactcheck " + range.ToString() + ",ON");
			}

			this._conn.SendCommand(":INITIATE:CONTINUOUS OFF");

			this._conn.SendCommand(":TRIGGER:SOURCE IMM");
		}

		public void Close()
		{
			this._conn.Close();
		}

		public byte Input(uint point)
		{
			return 0;
		}

		public byte InputB(uint point)
		{
			return 0;
		}

		public void Output(uint point, bool active)
		{
			return;
		}

        public bool CheckInterLock()
        {
            return true;
        }

		private enum ERM2542Range
		{
			RNG100MIL,
			RNG1000MIL,
			RNG10,
			RNG100,
			RNG1000,
			RNG10K,
			RNG100K,
			RNG1000K,
			RNG10MEG,
			RNG100MEG,
		}
	}
}
