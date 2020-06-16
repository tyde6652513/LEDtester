using System;
using System.Collections.Generic;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SourceMeter
{
    public class DR2000 : ISourceMeter
    {
        private const int MAX_SETTING_MEMORY = 10;

        private float VOLT_LOW_RANGE  = 35;
        private float CURR_LOW_RANGE = 6;

        private float VOLT_HIGH_RANGE = 70;
        private float CURR_HIGH_RANGE = 3;

        private IConnect _conn;
        private ElectSettingData[] _elcSetting;
        private ElecDevSetting _devSetting;
        private EDevErrorNumber _errorNum;

        private string _returnString;
		private double[][] _applyData;
        private double[] _forceValue;
        private double[] _secForceRange;
        private double[][] _msrtResult;

        private string _hwVersion;
        private string _swVersion;
        private string _serialNum;

        private SourceMeterSpec _spec;

        public DR2000()
        {
            this._elcSetting = new ElectSettingData[MAX_SETTING_MEMORY];
            this._forceValue = new double[MAX_SETTING_MEMORY];
            this._secForceRange = new double[MAX_SETTING_MEMORY];
			this._applyData = new double[MAX_SETTING_MEMORY][];

            this._msrtResult = new double[MAX_SETTING_MEMORY][];

            for (int i = 0; i < this._msrtResult.Length; i++)
            {
                this._msrtResult[i] = new double[1];
            }

            this._hwVersion = "HW NONE";
            this._swVersion = "SW NONE";
            this._serialNum = "SN NONE";

            this._spec = new SourceMeterSpec();
        }

        public DR2000(ElecDevSetting setting) : this()
        {
			this._devSetting = setting;
        }

        # region >>> Public Proberty <<<

        public string SerialNumber
        {
            get { return (this._serialNum); }
        }

        public string SoftwareVersion
        {
            get { return this._swVersion; }
        }

        public string HardwareVersion
        {
            get { return this._hwVersion; }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return this._errorNum; }
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

        #region >>> Private Method <<<

        private bool AcquireMsrtData(uint itemIndex)
        {
            this._returnString = "";

            if (this._elcSetting == null || itemIndex > this._elcSetting.Length)
            {
                double[] data = new double[0];

                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;

                return false;
            }

            if (this._elcSetting[itemIndex].MsrtType == EMsrtType.FIMV)
            {
                this._conn.SendCommand("MEAS:VOLT?");
            }
			else if (this._elcSetting[itemIndex].MsrtType == EMsrtType.FVMI)
            {
                this._conn.SendCommand("MEAS:CURR?");
            }

            this._conn.WaitAndGetData(out this._returnString);

            this.ParseString(itemIndex);

            return true;
        }

        private bool FindForceRangIndex(ElectSettingData[] settingData)
        {
            foreach (ElectSettingData setting in settingData)
            {
                if (setting.MsrtType == EMsrtType.FIMV || setting.MsrtType == EMsrtType.FI)
                {
                    if (setting.ForceValue <= CURR_LOW_RANGE && setting.MsrtRange <= VOLT_LOW_RANGE)    // in RANGE 1
                    {
                        //setting.MsrtRange = VOLT_LOW_RANGE;
                    }
					else if (setting.ForceValue <= CURR_HIGH_RANGE && setting.MsrtRange > VOLT_LOW_RANGE && setting.MsrtRange <= VOLT_HIGH_RANGE) // in RANGE 2
					{
						//setting.MsrtRange = VOLT_HIGH_RANGE;
					}
					else
					{
						return false;
					}
                }
                else if (setting.MsrtType == EMsrtType.FVMI || setting.MsrtType == EMsrtType.FV)
                {
                    if (setting.ForceValue <= VOLT_LOW_RANGE && setting.MsrtRange <= CURR_LOW_RANGE)    // in RANGE 1
                    {
                        //setting.MsrtRange = CURR_LOW_RANGE;
                    }
					else if (setting.ForceValue > VOLT_LOW_RANGE && setting.ForceValue <= VOLT_HIGH_RANGE && setting.MsrtRange <= CURR_HIGH_RANGE) // in RANGE 2
					{
						//setting.MsrtRange = CURR_HIGH_RANGE;
					}
					else
					{
						return false;
					}
                }
            }

            return true;
        }

        private void SetRangeToMeter(uint itemIndex)
        {
			if (this._elcSetting[itemIndex].MsrtType == EMsrtType.FIMV)           // MV
			{
				this._conn.SendCommand("OUTPUT:MAX:VOLT " + this._elcSetting[itemIndex].MsrtRange.ToString());

				this._conn.SendCommand("OUTPUT:MAX:CURR " + this._elcSetting[itemIndex].ForceRange.ToString());
			}
			else                                                                  // MI
			{
				this._conn.SendCommand("OUTPUT:MAX:VOLT " + this._elcSetting[itemIndex].ForceRange.ToString());

				this._conn.SendCommand("OUTPUT:MAX:CURR " + this._elcSetting[itemIndex].MsrtRange.ToString());
			}
        }

        private void ParseString(uint itemIndex)
        {
            double tempResult;

            Double.TryParse(this._returnString, out tempResult);

            this._msrtResult[itemIndex][0] = tempResult;
        }

        private void SetDeviceSpec(string deviceName)
        {
            if (deviceName.Contains("DR 2003"))
            {
                this.VOLT_LOW_RANGE  = 35.0f;
                this.CURR_LOW_RANGE = 6.0f;

                this.VOLT_HIGH_RANGE = 70.0f;
                this.CURR_HIGH_RANGE = 3.0f;
            }
            else if (deviceName.Contains("DR 2004"))
            {
                this.VOLT_LOW_RANGE = 100.0f;
                this.CURR_LOW_RANGE = 2.0f;

                this.VOLT_HIGH_RANGE = 200.0f;
                this.CURR_HIGH_RANGE = 1.0f;

                this._conn.SendCommand("SYS:LOW:CURR 1");
            }
            else if (deviceName.Contains("DR 2005"))
            {
                this.VOLT_LOW_RANGE = 400.0f;
                this.CURR_LOW_RANGE = 0.5f;

                this.VOLT_HIGH_RANGE = 600.0f;
                this.CURR_HIGH_RANGE = 0.35f;

                this._conn.SendCommand("SYS:LOW:CURR 1");
            }
            else
            {
                this.VOLT_LOW_RANGE = 35;
                this.CURR_LOW_RANGE = 6;

                this.VOLT_HIGH_RANGE = 70;
                this.CURR_HIGH_RANGE = 3;
            }
        }

        #endregion

        #region >>> Public Method <<<

        public bool Init(int devNum, string sourceMeterSN)
        {
			Console.WriteLine("[DR2000], Init()");

            //---------------------------------------------------------------
            //  Config connect type
            //---------------------------------------------------------------
            switch (this._devSetting.HWConnectorType)
            {
                case EHWConnectType.GPIB:
                    this._conn = new GPIBConnect();
                    break;
                //---------------------------------------------------------------------------------------------
                case EHWConnectType.RS232:
                    RS232SettingData rs232Setting = new RS232SettingData();
                    rs232Setting.ComPortName = sourceMeterSN;
                    rs232Setting.BaudRate = 57600;
                    rs232Setting.DataBits = 8;
                    rs232Setting.Parity = "None";
                    rs232Setting.StopBits = 1;
                    rs232Setting.TimeOut = 500;
                    rs232Setting.Terminator = "\x0d";
                    this._conn = new RS232Connect(rs232Setting);
                    break;
                //---------------------------------------------------------------------------------------------
                default:
                    this._conn = new RS232Connect();
                    break;
            }

            string info = "";
            
            if (this._conn.Open(out info))
            {
                this._conn.SendCommand("*IDN?");

                this._conn.WaitAndGetData(out info);

				Console.WriteLine("[DR2000], *IDN?", info);

                string[] deviceInfo = info.Split(',');

                if (info != "")
                {
                    this._hwVersion = deviceInfo[1];

                    this._serialNum = deviceInfo[2];

                    this._swVersion = deviceInfo[3];
                }
                else
                {
					Console.WriteLine("[DR2000], Init() Err, info = empty");

                    this._conn.Close();

                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;

                    return false;
                }
            }
            else
            {
				Console.WriteLine("[DR2000], Init() Err, Open() Fail");

                this._conn.Close();

                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;

                return false;
            }

            try
            {
                this._conn.SendCommand("*CLS");            // clear status

                this._conn.SendCommand("*RST");            // resets the power supply to its power on state

				this._conn.SendCommand("SYS:LED 1");

                this._conn.SendCommand("SYS:BEEP 0");    // Beeper Off

                this.SetDeviceSpec(this._hwVersion);

                return true;
            }
            catch(Exception e)
            {
				Console.WriteLine("[DR2000]," + e.ToString());

                return false;
            }
            
        }

        public void Close()
        {
			Console.WriteLine("[DR2000], Close()");

            this._conn.SendCommand("*CLE");  
          
            this._conn.SendCommand("*RST");

            this._conn.Close();
        }

        public void Reset()
        {
			Console.WriteLine("[DR2000], Reset()");
        }

        public bool SetConfigToMeter(ElecDevSetting devSetting)
        {
            return true;
        }

        public bool SetParamToMeter(ElectSettingData[] settingData)
        {
			Console.WriteLine("[DR2000], SetParamToMeter()");

            ElectSettingData data;

            this._elcSetting = settingData;

            this._errorNum = EDevErrorNumber.Device_NO_Error;

            int memoryPoint = 0;

            if (settingData.Length == 0)
            {
                this._errorNum = EDevErrorNumber.NoSourceMeterParamSettingData;

                return false;
            }
            if (settingData.Length > MAX_SETTING_MEMORY)
            {
                return false;
            }

            if (!this.FindForceRangIndex(settingData))
            {
                this._errorNum = EDevErrorNumber.NoMatchRangeIndex;

                return false;
            }

            for (int i = 0; i < settingData.Length; i++)
            {
				data = settingData[i];

				this._applyData[i] = new double[1] { data.ForceValue };

                memoryPoint = i;
                
                switch (data.MsrtType)
                {
                    case EMsrtType.FVMI:
                        {
                            this._conn.SendCommand("MEM " + memoryPoint.ToString());

                            this._conn.SendCommand("MEM:VSET " + data.ForceValue.ToString());

                            this._conn.SendCommand("MEM:ISET " + data.MsrtRange.ToString());

                            this._conn.SendCommand("MEM:SAV");

                            break;
                        }
                    case EMsrtType.FIMV:
                        {
                            this._conn.SendCommand("MEM " + memoryPoint.ToString());;

                            this._conn.SendCommand("MEM:VSET " + data.MsrtRange.ToString());

                            this._conn.SendCommand("MEM:ISET " + data.ForceValue.ToString());

                            this._conn.SendCommand("MEM:SAV");

                            break;
                        }
					default:
						{
							break;
						}
                }
            }

            return true;
        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex)
        {
            if (this._elcSetting == null)
            {
                this._errorNum = EDevErrorNumber.NoSourceMeterParamSettingData;

                return false;
            }

            if (this._errorNum != EDevErrorNumber.Device_NO_Error)
            {
                return false;
            }

			//this.SetRangeToMeter(itemIndex);

            this._conn.SendCommand("*RCL " + itemIndex);    // MEM start from 0 to 9 
       
            System.Threading.Thread.Sleep((int)this._elcSetting[itemIndex].ForceDelayTime);// + 70

            this._conn.SendCommand("OUT 1");

            System.Threading.Thread.Sleep((int)this._elcSetting[itemIndex].ForceTime);// + 50

            this.AcquireMsrtData(itemIndex);

            if (this._elcSetting[itemIndex].IsAutoTurnOff)
            {
                this._conn.SendCommand("OUT 0");

            }
            return true;
        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex, double applyValue)
        {
            return true;
        }

        public double[] GetDataFromMeter(uint channel, uint itemIndex)
        {
            return this._msrtResult[itemIndex];
        }

        public double[] GetApplyDataFromMeter(uint channel, uint settingIndex)
        {
			if (this._elcSetting == null || settingIndex > this._elcSetting.Length)
			{
				double[] data = new double[0];

				this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;

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
            this._conn.SendCommand("OUT 0");
        }

        public void TurnOff(double delay, bool isOpenRelay)
        {
            this._conn.SendCommand("OUT 0");
            System.Threading.Thread.Sleep((int)delay);
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

        public enum DR2003ErrorCode : int
        {          
            ERR_NONE = 0,
            ERR_COMMAND = 1,
            ERR_EXECUTION = 2,
            ERR_QUERY = 3,
            ERR_INPUT_RANGE = 4,
        }

    }
}
