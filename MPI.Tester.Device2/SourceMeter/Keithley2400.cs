using System;
using System.Collections.Generic;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SourceMeter
{
    public class Keithley2400 : ISourceMeter
    {
        private object _lockObj;

        private const double MAX_LIM_VOLT       = 210.0d;       // unit = V
        private const double MAX_LIM_CURRENT    = 1.05d;        // unit = A
        private const double MAX_DELAY_TIME      = 9999000.0d;      // unit = ms
        private const double MIN_DELAY_TIME       = 0.01d;         // unit = ms

        private const int MEM_SWEEP_COUNT = 100;
        private const int MSRT_ELEMENT_COUNT = 5;
       
        private IConnect _conn;

        private ElectSettingData[] _elcSetting;
        private ElecDevSetting _devSetting;
        private EDevErrorNumber _errorNum;
     
        //private string _returnString;
        private bool _isTurnOff;
        private double[] _acquireData;
        private double[][] _msrtDataArray;
		private double[] _msrtResult;
  
        private string _hwVersion;
        private string _swVersion;
        private string _serialNum;

        private double[][] _voltRange;
        private double[][] _currRange;

        private SourceMeterSpec _spec;

        private static double[][] _k2400voltRange = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.21d,   2.1d,   21.0d },
                                                    new double[] { 210.0d },
												};

        private static double[][] _k2400currRange = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 0.00000105d,   0.0000105d,    0.000105d,    0.00105d,    0.0105d,    0.105d,    1.05d },
                                                    new double[] { 0.00000105d,   0.0000105d,    0.000105d,    0.00105d,    0.0105d,    0.105d },
												};


        private static double[][] _k2425voltRange = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.21d,   2.1d,   21.0d },
                                                    new double[] { 105.0d },
												};

        private static double[][] _k2425currRange = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 0.0000105d,    0.000105d,    0.00105d,    0.0105d,    0.105d,    1.05d,    3.15d },  
                                                    new double[] { 0.0000105d,    0.000105d,    0.00105d,    0.0105d,    0.105d,    1.05d },
												};

        private static double[][] _k2430voltRange = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.21d,   2.1d,   21.0d },
                                                    new double[] { 105.0d },
												};

        private static double[][] _k2430currRange = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 0.0000105d,    0.000105d,    0.00105d,    0.0105d,    0.105d,    1.05d,    3.15d },  
                                                    new double[] { 0.0000105d,    0.000105d,    0.00105d,    0.0105d,    0.105d,    1.05d },
												}; 

		public Keithley2400()
		{
			this._lockObj = new object();
         
            this._msrtResult = null;
			this._elcSetting = new ElectSettingData[MEM_SWEEP_COUNT];
			this._errorNum = EDevErrorNumber.Device_NO_Error;
			this._devSetting = new ElecDevSetting();		// default is GPIB communication

            this._msrtResult = new double[MEM_SWEEP_COUNT];
            this._isTurnOff = true;

            this._hwVersion = "HW NONE";
            this._swVersion = "SW NONE";
            this._serialNum = "SN NONE";

            this.ResetDevConfig();

            this._spec = new SourceMeterSpec();
		}

        public Keithley2400(ElecDevSetting setting) : this()
        {
			this._devSetting = setting;
			this.ResetDevConfig();
        }

        #region >>> Public Proberty <<<

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

        public ElectSettingData[] SettingData
        {
            get { return this._elcSetting; }
            set
            {
                lock (this._lockObj)
                {
                    if (value.Length < MEM_SWEEP_COUNT)
                    {
                        this._elcSetting = value;
                    }
                    else
                    {
                        for (int i = 0; i < value.Length; i++)
                        {
                            this._elcSetting[i] = value[i];
                        }

                    }
                }
            }

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

        private void ResetDevConfig()
        {
            //---------------------------------------------------------------
            //  Config connect type
            //---------------------------------------------------------------
            switch (this._devSetting.HWConnectorType)
            {
                case EHWConnectType.GPIB:
                    this._conn = new GPIBConnect();
                    break;
                //---------------------------------------------------
                case EHWConnectType.RS232:
                    this._conn = new GPIBConnect();
                    break;
                //---------------------------------------------------
                default:
                    this._conn = new GPIBConnect();
                    break;
            }
        }

        private double ResizeForceValue(double ForceValue, int roundoff)
        {
            double temp = 0.0d;
            temp = Math.Round(ForceValue, roundoff, MidpointRounding.AwayFromZero);
            return temp;
        }

        private bool AcquireMsrtData(uint itemIndex)
        {
            string tempStrResult = "";

            if (_conn.WaitAndGetData(out tempStrResult))
            {
                string[] rtnString = tempStrResult.Split(',');
                
                if (this._elcSetting[itemIndex].MsrtType == EMsrtType.FIMV || this._elcSetting[itemIndex].MsrtType == EMsrtType.POLAR)
                {
                    Double.TryParse(rtnString[0], out this._msrtResult[0]);
                }
                else
                {
                    Double.TryParse(rtnString[1], out this._msrtResult[0]);
                }             
            }
            else
            {
                return false;
            }
            return true;
        }
		
        private bool AcquireAllMsrtData()
        {
            //int elements = 2;           
            //this._returnString = "";

            //this._conn.SendCommand(":TRAC:FEED:CONT NEV");
            //this._conn.SendCommand(":TRAC:DATA?");

            //if (_conn.WaitAndGetData(out this._returnString))
            //{
            //    ParseString(elements, 0);
                
            //    this._conn.SendCommand(":TRAC:CLE");
            //    return true;
            //}
            //else
            //{
            //    this._conn.SendCommand(":TRAC:CLE");
            //    return false;
            //}
            return true;
        }

        private bool FindSrcAndMsrtRange(ElectSettingData[] settingData)
        {
            foreach (ElectSettingData setting in settingData)
            {
                switch (setting.MsrtType)
                {
                    case EMsrtType.FIMV:
                    case EMsrtType.FI:
                    case EMsrtType.POLAR:
                        // Force Range
                        if (setting.ForceValue <= this._currRange[0][this._currRange[0].Length - 1])
                        {
                            setting.ForceRange = setting.ForceValue;
                        }
                        else
                        {
                            setting.ForceRange = this._currRange[0][this._currRange[0].Length - 1];
                            return false;
                        }

                        // Measurement Range
                        if (setting.ForceValue > this._currRange[1][this._currRange[1].Length - 1] && setting.MsrtRange > this._voltRange[0][this._voltRange[0].Length - 1])
                        {
                            setting.MsrtRange = this._voltRange[0][this._voltRange[0].Length - 1];
                            setting.MsrtProtection = setting.MsrtRange;
                            return false;
                        }
                        else
                        {
                            if (setting.MsrtRange > this._voltRange[1][this._voltRange[1].Length - 1])
                            {
                                setting.MsrtRange = this._voltRange[0][this._voltRange[0].Length - 1];
                                setting.MsrtProtection = setting.MsrtRange;
                                return false;
                            }
                        }

                        break;
                    //-----------------------------------------------------------------------------//
                    case EMsrtType.FVMI:
                    case EMsrtType.FV:
                        // Force Range
                        if (setting.ForceValue <= this._voltRange[1][this._voltRange[1].Length - 1])
                        {
                            setting.ForceRange = setting.ForceValue;
                        }
                        else
                        {
                            setting.ForceRange = this._voltRange[1][this._voltRange[1].Length - 1];
                            return false;
                        }

                        // Measurement Range
                        if (setting.ForceValue > this._voltRange[0][this._voltRange[0].Length - 1] && setting.MsrtRange > this._currRange[1][this._currRange[1].Length - 1])
                        {
                            setting.MsrtRange = this._currRange[1][this._currRange[1].Length - 1];
                            setting.MsrtProtection = setting.MsrtRange;
                            return false;
                        }
                        else
                        {
                            if (setting.MsrtRange > this._currRange[0][this._currRange[0].Length - 1])
                            {
                                setting.MsrtRange = this._currRange[1][this._currRange[1].Length - 1];
                                setting.MsrtProtection = setting.MsrtRange;
                                return false;
                            }
                        }
                        break;
                }
            }
            return true;
        }

		private bool startMemSweep(uint startIndex, uint point)
		{
			string cmd;

            if (this._devSetting.IsGetAllData)
            {
                cmd = ":TRAC:FEED:CONT NEXT" + ";" +
                      ":ARM:COUN " + point.ToString() + ";" +
                      ":TRIG:COUN 1" + ";" +
                      ":SOUR:FUNC MEM" + ";" +
                      ":SOUR:MEM:POIN " + point.ToString() + ";" +
                      ":SOUR:MEM:STAR " + startIndex.ToString() + ";" +
                      ":SOUR:CLE:AUTO ON" + ";" +
                      ":SOUR:CLE:AUTO:MODE ALW" + ";" +
                      ":INIT";
            }
            else
            {
                cmd = ":ARM:COUN " + point.ToString() + ";" +
                      ":TRIG:COUN 1" + ";" +
                      ":SOUR:FUNC MEM" + ";" +
                      ":SOUR:MEM:POIN " + point.ToString() + ";" +
                      ":SOUR:MEM:STAR " + startIndex.ToString() + ";" +
                      ":SOUR:CLE:AUTO ON" + ";" +
                      ":SOUR:CLE:AUTO:MODE ALW" + ";" +
                      ":INIT";
            }
			this._conn.SendCommand(cmd);
			return true;

		}

        private bool startMemOutput(uint itemIndex)
        {
            uint memIdx = itemIndex + 1;
            string cmd = string.Empty;

            if (this._devSetting.IsGetAllData)
            {
                cmd = ":TRAC:FEED:CONT NEXT" + ";" +
                      ":SOUR:MEM:REC " + memIdx.ToString() + ";" +
                      ":READ?";
            }
            else
            {
                if (this._elcSetting[itemIndex].IsAutoTurnOff)
                {
                    cmd = ":SOUR:CLE:AUTO ON;" +
                          ":SOUR:MEM:REC " + memIdx.ToString() + ";" +
                          ":OUTP 1;" +
                          ":READ?";

                    if (!this._isTurnOff)
                    {
                        cmd = ":SOUR:CLE:AUTO ON;" +
                              ":READ?";
                        this._isTurnOff = true;
                    }
                }
                else
                {
                    cmd = ":SOUR:CLE:AUTO OFF;" +
                          ":SOUR:MEM:REC " + memIdx.ToString() + ";" +
                          ":OUTP 1;" +
                          ":READ?";
                    this._isTurnOff = false;
                }
            }

            this._conn.SendCommand(cmd);
            return true;
        }

		#endregion

        #region >>> Public Method <<<

        public bool Init(int devNum, string sourceMeterSN)       // InitDeviceSetting
        {
            string info = "";
            string deviceModel = "";

            if (_conn.Open(out info) && info != "")
            {
                string[] deviceInfo = info.Split(',');
                deviceModel = deviceInfo[1];
                this._serialNum = deviceInfo[2];
                this._swVersion = "Ver. 1.2";
                this._hwVersion = "Keithley " + deviceModel;
            }
            else
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                return false;
            }

            try
            {
                // K2400series initialize, K2400/K2425, and auto switching Device Spec

                switch (deviceModel)
                {
                    case "MODEL 2400":
                        this._voltRange = _k2400voltRange;
                        this._currRange = _k2400currRange;
                        break;
                    case "MODEL 2425":
                        this._voltRange = _k2425voltRange;
                        this._currRange = _k2425currRange;
                        break;
                    case "MODEL 2430":
                    case "MODEL 2430-C":
                        this._voltRange = _k2430voltRange;
                        this._currRange = _k2430currRange;
                        break;
                    default: 
                        this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;    
                        return false;
                }
                               
                this._conn.SendCommand("*RST");
                this._conn.SendCommand(":SYST:PRES");   
                this.Reset();

                this._conn.SendCommand(":SENS:CURR:NPLC 0.01");    // Measurement CURR speed  (Fast: 0.01, Medium: 0.1, Slow: 1 )
                this._conn.SendCommand(":SENS:VOLT:NPLC 0.01");    // Measurement VOLT speed
                this._conn.SendCommand(":SENS:RES:NPLC 0.01");     // Measurement RES speed

                this._conn.SendCommand(":SYST:BEEP:STAT OFF");     // Close Beeper          

                this._conn.SendCommand(":ROUT:TERM REAR");   // Output from Rear 
                this._conn.SendCommand(":SYST:RSEN ON");     // 4 wired Measurement

                // NPLC Caching / Auto Zero Disable
                this._conn.SendCommand(":SYST:AZERO OFF");
                this._conn.SendCommand(":SYST:AZERO:CACH OFF");
                this._conn.SendCommand(":SENS:FUNC:CONC OFF");

                this._conn.SendCommand(":DISP:DIG 5");
                this._conn.SendCommand(":DISP:ENAB OFF");

                this._conn.SendCommand(":SENS:AVER OFF");
                this._conn.SendCommand(":CALC2:NULL:STAT OFF");

                this._conn.SendCommand(":FORM:ELEM VOLT,CURR");                  	// return data formate  
                this._conn.SendCommand(":SOUR2:CLE:AUTO OFF");                   	// Set digital I/O auto clear
                this._conn.SendCommand(":SOUR2:CLE:AUTO:DEL 0.000");				// Auto Delay 

                return true;
            }
            catch
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                return false;
            }
        }

        public void Reset()
        {
            this._conn.SendCommand("*CLS");
            this._conn.SendCommand("*OPC");

           // this._conn.SendCommand(":SYST:MEM:INIT");
            this._conn.SendCommand(":ABOR");

            this._conn.SendCommand(":TRAC:FEED:CONT NEV");
            this._conn.SendCommand(":TRAC:CLE");
        }

        public bool SetParamToMeter(ElectSettingData[] settingData)    // Set Data into the device memory 
        {            
            ElectSettingData data;
            this._elcSetting = settingData;        
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            int memoryPoint = 0;
            
            if (settingData.Length == 0)
            {
                return true;
            }

            if (!this.FindSrcAndMsrtRange(settingData))
            {
                this._errorNum = EDevErrorNumber.NoMatchRangeIndex;
                return false;
            }

            for (int i = 0; i < settingData.Length; i++)
            {                          
                data = settingData[i];
                memoryPoint = i + 1;

               // data.ForceValue = this.ResizeForceValue(data.ForceValue, 6);  // resize force value
                double forceTime = Math.Round((data.ForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

                if (data.ForceTime > MAX_DELAY_TIME || data.ForceTime < MIN_DELAY_TIME)
                {
                    return false;
                }

                if (data.ForceDelayTime != 0)
                {
                    if (data.ForceDelayTime > MAX_DELAY_TIME || data.ForceDelayTime < MIN_DELAY_TIME)
                    {
                        return false;
                    }
                }

                switch (data.MsrtType)
                {
                    case EMsrtType.FV:    
                            this._conn.SendCommand(":SYST:AZERO OFF");

                            this._conn.SendCommand(":SOUR:FUNC VOLT");
                            this._conn.SendCommand(":SOUR:VOLT:RANG:AUTO OFF");
                            this._conn.SendCommand(":SOUR:VOLT:RANG " + data.ForceRange.ToString());

                            this._conn.SendCommand(":SOUR:DEL:AUTO OFF");
                            this._conn.SendCommand(":SOUR:DEL " + forceTime.ToString());                
                            this._conn.SendCommand(":SOUR:VOLT " + data.ForceValue.ToString());

                            this._conn.SendCommand(":TRIG:DEL 0.000");                                        
                            break;
                    //----------------------------------------------------------------------------------------------//
                    case EMsrtType.FI:
                            this._conn.SendCommand(":SYST:AZERO OFF");

                            this._conn.SendCommand(":SOUR:FUNC CURR");
                            this._conn.SendCommand(":SOUR:CURR:RANG:AUTO OFF");
                            this._conn.SendCommand(":SOUR:CURR:RANG " + data.ForceRange.ToString());

                            this._conn.SendCommand(":SOUR:DEL:AUTO OFF");
                            this._conn.SendCommand(":SOUR:DEL " + forceTime.ToString());                 
                            this._conn.SendCommand(":SOUR:CURR " + data.ForceValue.ToString());

                            this._conn.SendCommand(":TRIG:DEL 0.000");                                            
                            break;
                    //----------------------------------------------------------------------------------------------//
                    case EMsrtType.FVMI:                                                  
                            this._conn.SendCommand(":SYST:AZERO OFF");

                            this._conn.SendCommand(":SOUR:FUNC VOLT");  
                            this._conn.SendCommand(":SOUR:VOLT:RANG:AUTO OFF");
                            this._conn.SendCommand(":SOUR:DEL:AUTO OFF");

                            this._conn.SendCommand(":SOUR:VOLT:RANG " + data.ForceRange.ToString());
                            
                            this._conn.SendCommand(":SOUR:DEL " + forceTime.ToString());                  // Source Delay 5ms
                            this._conn.SendCommand(":SOUR:VOLT " + data.ForceValue.ToString());
             
                            this._conn.SendCommand(":SENS:FUNC 'CURR:DC'"); 
                            this._conn.SendCommand(":SENS:CURR:RANG:AUTO OFF");
                            this._conn.SendCommand(":SENS:AVER OFF");
                                 
                            this._conn.SendCommand(":SENS:CURR:PROT " + data.MsrtProtection.ToString());   // cmpl
                            this._conn.SendCommand(":SENS:CURR:RANG " + data.MsrtRange.ToString());
                                 
                            this._conn.SendCommand(":TRIG:DEL 0.000");                                         // Trigger Delay 0ms 
                            break;
                    //----------------------------------------------------------------------------------------------//
                    case EMsrtType.FIMV:
                    case EMsrtType.POLAR:
                            this._conn.SendCommand(":SYST:AZERO OFF");

                            this._conn.SendCommand(":SOUR:FUNC CURR");
                            this._conn.SendCommand(":SOUR:CURR:RANG:AUTO OFF");
                            this._conn.SendCommand(":SOUR:DEL:AUTO OFF");

                            this._conn.SendCommand(":SOUR:CURR:RANG " + data.ForceRange.ToString());
                            
                            this._conn.SendCommand(":SOUR:DEL " + forceTime.ToString());
                            this._conn.SendCommand(":SOUR:CURR " + data.ForceValue.ToString());

                            this._conn.SendCommand(":SENS:FUNC 'VOLT:DC'");
                            this._conn.SendCommand(":SENS:VOLT:RANG:AUTO OFF");
                            this._conn.SendCommand(":SENS:AVER OFF");

                            this._conn.SendCommand(":SENS:VOLT:PROT " + data.MsrtProtection.ToString());
                            this._conn.SendCommand(":SENS:VOLT:RANG " + data.MsrtRange.ToString());
                                                    
                            this._conn.SendCommand(":TRIG:DEL 0.000");
                            break;
                    //----------------------------------------------------------------------------------------------//
                    default:
                        break;                                         
                }
                this._conn.SendCommand(":SOUR:MEM:SAVE " + memoryPoint.ToString());
            }

            if (this._devSetting.IsGetAllData)
            {
                this._conn.SendCommand(":TRAC:FEED SENS");
            }

            //this._conn.SendCommand(":ARM:SOUR IMM");        // cmd INIT: for starting MemSweep        
            //this._conn.SendCommand(":ARM:DIR SOUR");

            //this._conn.SendCommand(":TRIG:CLE");   
            //this._conn.SendCommand(":TRIG:COUN 1");
            //this._conn.SendCommand(":ARM:COUN 1");

            //this._conn.SendCommand(":TRIG:DIR ACC");
            //this._conn.SendCommand(":TRIG:SOUR IMM");
            //this._conn.SendCommand(":TRIG:INP SOUR");
            return true;
        }

        public void Close()
        {
            this._conn.SendCommand(":SYST:LOC");
            this._conn.Close();
        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex)
        {
            if (this._elcSetting == null || this._elcSetting.Length == 0)
            {
                this._errorNum = EDevErrorNumber.NoSourceMeterParamSettingData;
                return false;
            }

            if (itemIndex > this._elcSetting.Length)
            {
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return false;
            }

            System.Threading.Thread.Sleep((int)this._elcSetting[itemIndex].ForceDelayTime);

            this.startMemOutput(itemIndex);

            this.AcquireMsrtData(itemIndex);

            return true;
        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex, double applyValue)
        {
            return true;
        }

        public double[] GetDataFromMeter(uint channel, uint itemIndex)
        {         
            if (this._elcSetting == null || itemIndex > this._elcSetting.Length)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }
           
			return this._msrtResult;
        }

        public double[] GetApplyDataFromMeter(uint channel, uint itemIndex)
        {
            double[] data = new double[1];
            return data;
        }

        public double[] GetSweepPointFromMeter(uint channel, uint settingIndex)
        {
            return null;
        }

        public double[] GetSweepResultFromMeter(uint channel, uint itemIndex)   // GetMeasurementData
        {
            return this._acquireData;
        }

        public double[] GetTimeChainFromMeter(uint channel, uint settingIndex)
        {
            throw new NotImplementedException();
        }

        public void TurnOff()
        {
            this._conn.SendCommand(":OUTP 0");
        }

        public void TurnOff(double delay, bool isOpenRelay)
        {
            this._conn.SendCommand(":OUTP 0");
            System.Threading.Thread.Sleep((int)delay);
        }
    
        public void Output(uint point, bool active)
        {
           
        }

        public byte InputB(uint point)
        {
            return 0;
        }

        public byte Input(uint point)
        {
            throw new NotImplementedException();
        }

        public bool SetConfigToMeter(ElecDevSetting devSetting)
        {
            return true;
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
