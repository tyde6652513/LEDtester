	using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.ESD
{
	static class PLCConstants
	{
		public const int			DELAY80MS = 80;                  // ideal time for sent command and receive acknowledge
		public const int			DELAY20MS = 20;                  // ideal time for sent command and receive acknowledge

		public const string S_AUTO_RUN_OFF		= "WCSR00000";
		public const string S_AUTO_RUN_ON		= "WCSR00001";

		public const string S_ONE_ZAP					= "WCSR00011";					// no used

		public const string S_REMOTE_OFF			= "WCSR00800";			// no used
		public const string S_REMOTE_ON				= "WCSR00801";			// no used

		public const string S_POLARITY_P				= "WCSR00810";
		public const string S_POLARITY_N			= "WCSR00811";

		public const string S_MODEL_HBM			= "WCSR00820";
		public const string S_MODEL_MM				= "WCSR00821";

		public const string S_TESTER_LOOP			= "WCSR00830";
		public const string S_ESD_LOOP					= "WCSR00831";

		public const string S_RESET_ON = "WCSR01011";				// no used
		public const string S_RESET_OFF = "WCSR01010";				// no used

		public const string S_INFINITE_LOOP = "WCSR01021";		// no used
		public const string S_FINITE_LOOP = "WCSR01020";

		public const string S_SINGLE_VOLT_ON = "WCSR01031";
		public const string S_SINGLE_VOLT_OFF = "WCSR01030";

		public const string S_TABLE_ON = "WCSR01041";
		public const string S_TABLE_OFF = "WCSR01040";

		public const string S_SWEEP_ON = "WCSR01051";
		public const string S_SWEEP_OFF = "WCSR01050";

        public const string S_PRECHARGE_ON = "WCSR01061";
        public const string S_PRECHARGE_OFF = "WCSR01060";
        
		public const string S_ZAP_START = "WCSR0013";
			

        public const string S_START00_ON = "WCSR00131";     //item 00
        public const string S_START00_OFF = "WCSR00130";      



		public const int			S_REG_ZAP_START					= 13;

		public const int			DT_REG_START_POS				= 500;
		public const int			DT_REG_DATA_LENGTH			= 100;
		public const int			DT_REG_HBMTABLE_POS		= 520;
		public const int			DT_REG_MMTABLE_POS		= 560;
		public const int			MAX_TABLE_COUNT				= 20;
		public const string			PLC_CHECK_CODE				= "**";
	}

	public enum EESDType : int 
	{
		HBM2K_MM200		= 0,
		HBM4K_MM400		= 1,
		HBM8K_MM800		= 2,
	}

	public class CommandRepliedEventArgs : EventArgs
	{
		private string _data;

		public CommandRepliedEventArgs(string data)
		{
			this._data = data;
		}

		public string Data
		{
			get { return this._data; }
		}
	}


    public class ESDCtrl : IESDDevice
    {
		private const int			DAC_SCALE					= 5;
		private  const int		HBM_START_VOLT		= 250;
        private  const int		HBM_END_VOLT			= 2000;
        private  const int		MM_START_VOLT		= 50;
        private  const int		MM_END_VOLT				= 400;

		private const string _ESD2K_SOFT_VER = "1.1";

        private object _lockObj;
        
		private EESDType _type;
        private SerialPort _port;
        private ESDSettingData[] _setting;
		private ESDDevSetting _cfg;

        private string _command = null;
        private string _comPortName = null;

		public event EventHandler<CommandRepliedEventArgs> CommandReplied;
		public event EventHandler<EventArgs> FinishTestEvent;

		private int _minHBM;			// unit = V
		private int _maxHBM;			// unit = V
		private int _minMM;				// unit = V
		private int _maxMM;				// unit = V
		private int _minCount;			// unit = cnt
		private int _maxCount;			// unit = cnt
		private int _minInterval;			// unit = ms
		private int _maxInterval;			// unit = ms
		private int _minStep;				// unit = V

        public string _sentCmd; //for a while; timporality; by angus

		private EDevErrorNumber _errorNum;
		private string _plcFirmwareVer;
		private string _plcHardwareVer;
		private string _plcSN;

		private bool _isWorkingBusy;
		private string _portData;

        public ESDCtrl()
        {
            this._lockObj = new object();

			this.ResetBoundry(EESDType.HBM2K_MM200);
            this._comPortName = "COM1";
            this._port = new SerialPort(this._comPortName, 19200, System.IO.Ports.Parity.Odd, 8, System.IO.Ports.StopBits.One);
            
            // (1) Open the COM port first
            this._port.Open();
            // (2) Add the Event Handler sencond
			this._port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort_DataReceived);

            this._setting = null;
			this._cfg = null;
            this._command = null;
			this._type = EESDType.HBM2K_MM200;
			this._errorNum = EDevErrorNumber.Device_NO_Error;
			this._plcFirmwareVer = "plc_ver";
			this._plcHardwareVer = "plc_hw_ver";
			this._plcSN = "plc_SN";
			this._isWorkingBusy = false;
			this._portData = null;
        }

		public ESDCtrl(EESDType type) : this()
		{
			this._type = type;
			this.ResetBoundry(  type );
		}
		
        public ESDCtrl(string comPortName, EESDType type) : this( type )
        {
            this._comPortName = comPortName;
            if (this._port.IsOpen)
            {
                if (this._port.PortName == comPortName)
                {
                    ;
                }
                else
                {
                    this._port.Close();
                }
            }
            else
            {
                this._port = new SerialPort( comPortName, 19200, System.IO.Ports.Parity.Odd, 8, System.IO.Ports.StopBits.One);
                // (1) Open the COM port first
                this._port.Open();
                // (2) Add the Event Handler sencond
                this._port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort_DataReceived);
            }
        }
		
        #region >>> Public Property <<<

        public int SetLength
        {
            get { return 0; }
        }

        public int[] ZapList
        {
            get { return null; }
        }

		public SerialPort Port
		{
			get { return this._port; }
		}

        public int SettingLength
        {
            get { return 0; }
        }

		public int MaxHBM
		{
			get { return this._maxHBM; }
		}

		public int MinHBM
		{
			get { return this._minHBM; }
		}

		public int MaxMM
		{
			get { return this._maxMM; }
		}

		public int MinMM
		{
			get { return this._minMM; }
		}

		public int MaxCount
		{
			get { return this._maxCount; }
		}

		public int MinCount
		{
			get { return this._minCount; }
		}

		public int MaxInterval
		{
			get { return this._maxInterval; }
		}

		public int MinInterval
		{
			get { return this._minInterval; }
		}

		public int MinStep
		{
			get { return this._minStep; }
		}

		public string SerialNumber
		{ 
			get { return this._plcSN; }
		}

		public string SoftwareVersion 
		{
			get { return ( _ESD2K_SOFT_VER + "_" + this._plcFirmwareVer); }
		}

		public string HardwareVersion
		{
			get { return  this._plcHardwareVer; }
		}

		public EDevErrorNumber ErrorNumber
		{
			get { return this._errorNum; }
		}

		public bool  IsWorkingBusy
		{
			get { return this._isWorkingBusy; }
		}

        public ESDHardwareInfo HardwareInfo
        {
            get { return new ESDHardwareInfo(); }
        }

        #endregion

        #region >>> Private Method <<<

		private string ShifHexStr(string str)
		{
			char padChar = '0';

			if (str.Length  < 4)
			{
				str.PadLeft(4, padChar );
			}

			string tempStr = str.Substring(2, 2);
			tempStr += str.Substring(0, 2);

			return tempStr;
		}

		private short ConvertToInt16(int data)
		{
			short rtnValue = 0;

			if (data > short.MaxValue)
			{
				rtnValue = short.MaxValue;
			}
			else if (data < short.MinValue)
			{
				rtnValue = short.MinValue;
			}
			else
			{
				rtnValue = (short)data;
			}

			return rtnValue;
		}

		private void ResetBoundry(EESDType type)
		{
			switch (type)
			{
				case EESDType.HBM2K_MM200:
					this._minHBM = 20;
					this._maxHBM = 2000;
					this._minMM = 5;
					this._maxMM = 200;
					this._minCount = 1;
					this._maxCount = 999;
					this._minInterval = 10;
					this._maxInterval = 1000;
					this._minStep = 10;
					break;
				//----------------------------------------------------------------------------
				case EESDType.HBM4K_MM400:
					this._minHBM = 20;				
					this._maxHBM = 4000;			
					this._minMM = 5;					
					this._maxMM = 400;				
					this._minCount = 1;				
					this._maxCount  = 999;			
					this._minInterval = 10;			
					this._maxInterval = 1000;		
					this._minStep = 10;
					break;
				//----------------------------------------------------------------------------
				case EESDType.HBM8K_MM800:
					this._minHBM = 20;				
					this._maxHBM = 8000;			
					this._minMM = 5;					
					this._maxMM = 800;				
					this._minCount = 1;				
					this._maxCount  = 999;			
					this._minInterval = 10;			
					this._maxInterval = 1000;		
					this._minStep = 10;
					break;
				//----------------------------------------------------------------------------
				default:
					break;
			}
		}

		private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
		{
			string bufferData = (sender as SerialPort).ReadExisting();

			this._portData = bufferData.Replace("\r","");

			if ( this._portData == "OK" )
			{
				// System.Threading.Thread.Sleep(5);
				this._isWorkingBusy = false;
			}
					 

			this.FireFinishTestEvent();			
		}

		private void FireFinishTestEvent()
		{
			if (this.FinishTestEvent != null)
			{
				this.FinishTestEvent(new object(), new EventArgs());
			}
		}

        private void SetDataToPLC( EESDOperation opMode, int dataIndex)
        {
			StringBuilder sb = new StringBuilder();
			int regStart = 0;
            int regEnd = 0;
            int sweepCount = 1;
            int sweepInterval = 1500;
            int IntervalforDelay = 1500;
            
            bool interval_flag = false;

			switch (opMode)
			{ 
				case EESDOperation.FixVolt:

                    //-----check if need to delay interval time for zap or not ----------------
                    if(dataIndex != 0)
                        if (this._setting[dataIndex - 1].OpMode == EESDOperation.FixVolt) // last opMode = ESD_Pulse
                        {
                            if (this._setting[dataIndex].SingleVolt != this._setting[dataIndex - 1].SingleVolt)
                                interval_flag = true;
                        }
                        else if (this._setting[dataIndex - 1].OpMode == EESDOperation.Sweep) // last opMode = ESD_Sweep
                        {
                            if (this._setting[dataIndex].SingleVolt != this._setting[dataIndex - 1].StartVolt)
                                interval_flag = true;
                        }

                    regStart = PLCConstants.DT_REG_START_POS + PLCConstants.DT_REG_DATA_LENGTH * dataIndex;
					regEnd = regStart + 9;
					sb.Append("%01#");
					sb.Append("WDD");
					sb.Append(regStart.ToString("D5"));
					sb.Append(regEnd.ToString("D5"));
					sb.Append(this.ShifHexStr(this.ConvertToInt16( (int) this._setting[dataIndex].Mode).ToString("X4")));			// DT500 , Registor_start = 500
					sb.Append(this.ShifHexStr(this.ConvertToInt16((int)this._setting[dataIndex].OpMode).ToString("X4")));		// DT501
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].SingleVolt/5).ToString("X4")));			// DT502
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._cfg.ChargeTime).ToString("X4")));			// DT503
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._cfg.DischargeTime).ToString("X4")));	// DT504
                    if (interval_flag == true )
                        sb.Append(this.ShifHexStr(this.ConvertToInt16(IntervalforDelay).ToString("X4")));		// DT505, delay Zap
                    else
					    sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].IntervalTime).ToString("X4")));		// DT505
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].Count).ToString("X4")));					// DT506
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].StartVolt/5).ToString("X4")));				// DT507
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].EndVolt/5).ToString("X4")));				// DT508
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].StepVolt/5).ToString("X4")));				// DT509
					sb.Append(PLCConstants.PLC_CHECK_CODE);
					sb.Append("\r");
					this._port.Write(sb.ToString());

                    interval_flag = false;

					break;
				//---------------------------------------------------------------------------------------------------------------------------
				case EESDOperation.Sweep:
					regStart = PLCConstants.DT_REG_START_POS + PLCConstants.DT_REG_DATA_LENGTH * dataIndex;
					regEnd = regStart + 9;
					sb.Append("%01#");
					sb.Append("WDD");
					sb.Append(regStart.ToString("D5"));
					sb.Append(regEnd.ToString("D5"));
					sb.Append(this.ShifHexStr(this.ConvertToInt16( (int) this._setting[dataIndex].Mode).ToString("X4")));			// DT500 , Registor_start = 500
					sb.Append(this.ShifHexStr(this.ConvertToInt16((int)this._setting[dataIndex].OpMode).ToString("X4")));		// DT501
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].SingleVolt/5).ToString("X4")));			// DT502
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._cfg.ChargeTime).ToString("X4")));			// DT503
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._cfg.DischargeTime).ToString("X4")));	// DT504
					//sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].IntervalTime).ToString("X4")));		// DT505
					sb.Append(this.ShifHexStr(this.ConvertToInt16(sweepInterval).ToString("X4")));		// DT505 , sweep interval always = 1500ms
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].Count).ToString("X4")));					// DT506
                    sb.Append(this.ShifHexStr(this.ConvertToInt16(sweepCount).ToString("X4")));					// DT506 , sweep's count always = 1
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].StartVolt/5).ToString("X4")));				// DT507
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].EndVolt/5).ToString("X4")));				// DT508
					sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].StepVolt/5).ToString("X4")));				// DT509
					sb.Append(PLCConstants.PLC_CHECK_CODE);
					sb.Append("\r");
					this._port.Write(sb.ToString());
					break;
				//---------------------------------------------------------------------------------------------------------------------------
				case EESDOperation.Table:
                    //regStart = PLCConstants.DT_REG_START_POS + PLCConstants.DT_REG_DATA_LENGTH * dataIndex;
                    //regEnd = regStart + 9;
                    //sb.Append("%01#");
                    //sb.Append("WDD");
                    //sb.Append(regStart.ToString("D5"));
                    //sb.Append(regEnd.ToString("D5"));
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16( (int) this._setting[dataIndex].Mode).ToString("X4")));			// DT500 , Registor_start = 500
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16((int)this._setting[dataIndex].OpMode).ToString("X4")));		// DT501
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].SingleVolt/5).ToString("X4")));			// DT502
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._cfg.ChargeTime).ToString("X4")));			// DT503
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._cfg.DischargeTime).ToString("X4")));	// DT504
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].IntervalTime).ToString("X4")));		// DT505
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].Count).ToString("X4")));					// DT506
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].StartVolt/5).ToString("X4")));				// DT507
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].EndVolt/5).ToString("X4")));				// DT508
                    //sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].StepVolt/5).ToString("X4")));				// DT509
                    //sb.Append(PLCConstants.PLC_CHECK_CODE);
                    //sb.Append("\r");
                    //this._port.Write(sb.ToString());

                    //// Send HBM Table Data to PLC, DT520 , Registor_start  for HBM Table= 520
                    //regStart = PLCConstants.DT_REG_HBMTABLE_POS;
                    //regEnd = regStart + PLCConstants.MAX_TABLE_COUNT - 1;
                    //sb.Clear();
                    //sb.Append("%01#");
                    //sb.Append("WDD");
                    //sb.Append(regStart.ToString("D5"));
                    //sb.Append(regEnd.ToString("D5"));
                    //for (int i = 0; i < this._setting[dataIndex].TableHBM.Length && i < PLCConstants.MAX_TABLE_COUNT; i++)
                    //{
                    //    sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].TableHBM[i]/5).ToString("X4")));	
                    //}
                    //sb.Append(PLCConstants.PLC_CHECK_CODE);
                    //sb.Append("\r");
                    //this._port.Write(sb.ToString());

                    //// Send MMTable Data to PLC, DT560 , Registor_start  for MM Table= 560
                    //regStart = PLCConstants.DT_REG_HBMTABLE_POS + PLCConstants.DT_REG_DATA_LENGTH * dataIndex;
                    //regEnd = regStart + PLCConstants.MAX_TABLE_COUNT - 1;
                    //sb.Clear();
                    //sb.Append("%01#");
                    //sb.Append("WDD");
                    //sb.Append(regStart.ToString("D5"));
                    //sb.Append(regEnd.ToString("D5"));
                    //for (int i = 0; i < this._setting[dataIndex].TableMM.Length && i < PLCConstants.MAX_TABLE_COUNT; i++)
                    //{
                    //    sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting[dataIndex].TableMM[i]/5).ToString("X4")));
                    //}
                    //sb.Append(PLCConstants.PLC_CHECK_CODE);
                    //sb.Append("\r");
                    //this._port.Write(sb.ToString());
					break;
				//---------------------------------------------------------------------------------------------------------------------------
				default:
					break;
			}
        }     
        
		#endregion

        #region >>> Public Method <<<

        bool IESDDevice.Open()
        {
            this._port.Open();
			return true;
        }
		
		void IESDDevice.Close()
        {
            this._port.Close();
        }

		bool IESDDevice.PreCharge(int index)
		{
			return true;
		}

		bool IESDDevice.ResetToSafeStatus()
		{
			return true;
		}

        public void CheckOpen()
        {
            if (this._port.IsOpen)
            {
                if (this._port.PortName == this._comPortName)
                {
                    ;
                }
                else
                {
                    this._port.Close();
                }
            }
            else
            {
                this._port = new SerialPort(this._comPortName, 9600, System.IO.Ports.Parity.Odd, 8, System.IO.Ports.StopBits.One);
                this._port.Open();
            }
        }
		
		public bool SetConfigToMeter(ESDDevSetting cfg)
		{
			StringBuilder sb = new StringBuilder();
			this._cfg = cfg;

			sb.Append("%01#" + "WDD0030000301");
			sb.Append(this.ShifHexStr(this.ConvertToInt16(this._cfg.WaitTime).ToString("X4")));				// DT00300
			sb.Append(this.ShifHexStr(this.ConvertToInt16(this._cfg.SafeTime).ToString("X4")));				// DT00301
			sb.Append(PLCConstants.PLC_CHECK_CODE);
			sb.Append("\r");

			this._port.Write(sb.ToString());
			System.Threading.Thread.Sleep(PLCConstants.DELAY80MS);     // delay for PLC read command

			return true;
		}

		private int ChenckUpDownBound(int value, int lowValue, int highValue)
		{
			if ( value <= lowValue )
				return lowValue;

			if ( value > highValue )
				return highValue;

			 return value;
		}
		
		public bool SetParamToMeter(ESDSettingData[] setting)
        {
			if ( setting == null )
				return true;
			
			this._setting = setting;

			for ( int i = 0 ; i < this._setting.Length; i++ )
			{
				//---------------------------------------------------
				// MM mode, settin data check 
				//---------------------------------------------------
				if ( this._setting[i].Mode == EESDMode.MM )
				{
					// Single Volt 
					if ( this._setting[i].SingleVolt > 0 )
					{
						this._setting[i].SingleVolt = this.ChenckUpDownBound( Math.Abs(this._setting[i].SingleVolt) , MM_START_VOLT , MM_END_VOLT);							
					}
					else
					{
						this._setting[i].SingleVolt = (-1) *this.ChenckUpDownBound( Math.Abs(this._setting[i].SingleVolt) , MM_START_VOLT , MM_END_VOLT);
					}

					// Start Volt
					if ( this._setting[i].StartVolt > 0 )
					{
						this._setting[i].StartVolt = this.ChenckUpDownBound( Math.Abs(this._setting[i].StartVolt) , MM_START_VOLT , MM_END_VOLT);							
					}
					else
					{
						this._setting[i].StartVolt = (-1) *this.ChenckUpDownBound( Math.Abs(this._setting[i].StartVolt) , MM_START_VOLT , MM_END_VOLT);
					}

					// End Volt 
					if ( this._setting[i].EndVolt > 0 )
					{
						this._setting[i].EndVolt = this.ChenckUpDownBound( Math.Abs(this._setting[i].EndVolt) , MM_START_VOLT , MM_END_VOLT);							
					}
					else
					{
						this._setting[i].EndVolt = (-1) *this.ChenckUpDownBound( Math.Abs(this._setting[i].EndVolt) , MM_START_VOLT , MM_END_VOLT);
					}			
				}

				//---------------------------------------------------
				// HBM mode, settin data check 
				//---------------------------------------------------
				if ( this._setting[i].Mode == EESDMode.HBM )
				{
						// Single Volt 
					if ( this._setting[i].SingleVolt > 0 )
					{
						this._setting[i].SingleVolt = this.ChenckUpDownBound( Math.Abs(this._setting[i].SingleVolt) , HBM_START_VOLT  , HBM_END_VOLT);							
					}
					else
					{
						this._setting[i].SingleVolt = (-1) *this.ChenckUpDownBound( Math.Abs(this._setting[i].SingleVolt) , HBM_START_VOLT , HBM_END_VOLT);
					}

					// Start Volt
					if ( this._setting[i].StartVolt > 0 )
					{
						this._setting[i].StartVolt = this.ChenckUpDownBound( Math.Abs(this._setting[i].StartVolt) , HBM_START_VOLT , HBM_END_VOLT);							
					}
					else
					{
						this._setting[i].StartVolt = (-1) *this.ChenckUpDownBound( Math.Abs(this._setting[i].StartVolt) , HBM_START_VOLT , HBM_END_VOLT);
					}

					// End Volt 
					if ( this._setting[i].EndVolt > 0 )
					{
						this._setting[i].EndVolt = this.ChenckUpDownBound( Math.Abs(this._setting[i].EndVolt) , HBM_START_VOLT , HBM_END_VOLT);							
					}
					else
					{
						this._setting[i].EndVolt = (-1) *this.ChenckUpDownBound( Math.Abs(this._setting[i].EndVolt) , HBM_START_VOLT , HBM_END_VOLT);
					}			
				}

				if (this._setting[0].Polarity == EESDPolarity.P)
				{
					this.RunCommand(EESDCommand.PolarityPositive);
				}
				else
				{
					this.RunCommand(EESDCommand.PolarityNegative);
				}

			}
            //--------------sent total item number -1 to PLC ---------------------------------------
            StringBuilder sb = new StringBuilder();
            sb.Append("%01#WDD0002000021");			// DT20 & DT21
            sb.Append(this.ShifHexStr(this.ConvertToInt16(this._setting.Length-1).ToString("X4"))); // sent total item number - 1
            sb.Append("0000");  // for DT21 = 0
            sb.Append(PLCConstants.PLC_CHECK_CODE);
            sb.Append("\r");
            this._port.Write(sb.ToString());
            System.Threading.Thread.Sleep(PLCConstants.DELAY80MS);     // delay for PLC read command
            //-----------------------------------------------------------------------------------------

			for (int index = 0; index < this._setting.Length; index++)
			{
				this.SetDataToPLC(this._setting[index].OpMode, index);
				System.Threading.Thread.Sleep(PLCConstants.DELAY80MS * 3);		// delay for PLC read command
			}

			this.RunCommand(EESDCommand.PrechargeOn);
			return true;
        }

		public void RunCommand(EESDCommand Cmd)
		{
			this.RunCommand(Cmd, 0);
		}

        //public void RunCommand( EESDCommand Cmd, int index)
		public bool RunCommand( EESDCommand Cmd, int index) // angus0626
        {
			bool isEnableDelay = true;

            if ( this._cfg == null || this._setting == null ) 
                //return;
                return false;// angus0626

            bool isZapOK = false;// angus0626

            switch (Cmd)
            {
                case EESDCommand.AutoRun:
                    this._command = "%01#" + PLCConstants.S_AUTO_RUN_ON + "**\r";
                    this._port.Write(this._command);                    
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.OneZap:
                    this._command = "%01#" + PLCConstants.S_ONE_ZAP + "**\r";
                    this._port.Write(this._command);                    
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.Stop:
                    this._command = "%01#" + PLCConstants.S_AUTO_RUN_OFF + "**\r";
                    this._port.Write(this._command);    
                break;
				//-----------------------------------------------------------------
                case EESDCommand.ESDLoop:
                    this._command = "%01#" + PLCConstants.S_ESD_LOOP + "**\r";
                    this._port.Write(this._command);        
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.TesterLoop:
                    this._command = "%01#" + PLCConstants.S_TESTER_LOOP + "**\r";
                    this._port.Write(this._command);       
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.ModelHBM:
                    this._command = "%01#" + PLCConstants.S_MODEL_HBM + "**\r";
                    this._port.Write(this._command);        
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.ModelMM:
                    this._command = "%01#" + PLCConstants.S_MODEL_MM + "**\r";
                    this._port.Write(this._command);    
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.PolarityPositive:
                    this._command = "%01#" + PLCConstants.S_POLARITY_P + "**\r";
                    this._port.Write(this._command);      
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.PolarityNegative:
					this._command = "%01#" + PLCConstants.S_POLARITY_N + "**\r";
                    this._port.Write(this._command);     
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.SingleVoltOn:
                    this._command = "%01#" + PLCConstants.S_SINGLE_VOLT_ON + "**\r";
                    this._port.Write(this._command);     
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.SingleVoltOff:
                    this._command = "%01#" + PLCConstants.S_SINGLE_VOLT_OFF + "**\r";
                    this._port.Write(this._command);     
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.ESDSet:
					//this.ESDSetDataToPLC();
					//isEnableDelay = false;
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.SweepOn:
                    this._command = "%01#" + PLCConstants.S_SWEEP_ON + "**\r";
                    this._port.Write(this._command);       
                    break;
				//-----------------------------------------------------------------
                case EESDCommand.SweepOff:
                    this._command = "%01#" + PLCConstants.S_SWEEP_OFF + "**\r";
                    this._port.Write(this._command);        
                    break;
                //-----------------------------------------------------------------
                case EESDCommand.PrechargeOn:
                    this._command = "%01#" + PLCConstants.S_PRECHARGE_ON + "**\r";
                    this._port.Write(this._command);
                    break;
                //-----------------------------------------------------------------
                case EESDCommand.PrechargeOff:
                    this._command = "%01#" + PLCConstants.S_PRECHARGE_OFF + "**\r";
                    this._port.Write(this._command);
                    break;
                //-----------------------------------------------------------------
                case EESDCommand.GetSN:
                    //this._command = "%01#" + this._sentCmd + "**\r";
                    this._command = "%01#" + "RDD3271032714" + "**\r"; //DT32710 ~ DT32714
                    this._port.Write(this._command);
                    break;
                //-----------------------------------------------------------------
				//case EESDCommand.GetCount:
                //    //this._command = "%01#" + this._sentCmd + "**\r";
                //    this._command = "%01#" + "RDD3271032714" + "**\r"; //DT32710 ~ DT32714
                //    this._port.Write(this._command);
                //    break;
                //-----------------------------------------------------------------	
				case EESDCommand.ZapON:
                        if (index >= 0 && index <= 5)
                        {
                            this._command = "%01#WCSR" + ((PLCConstants.S_REG_ZAP_START + index) * 10 + 1).ToString("D5");
                            this._command += PLCConstants.PLC_CHECK_CODE;
                            this._command += "\r";
                            this._port.Write(this._command);
                        }
					break;
				//-----------------------------------------------------------------
				case EESDCommand.ZapOff:
					if (index >= 0 && index <= 5)
					{
						this._command = "%01#WCSR" + ((PLCConstants.S_REG_ZAP_START + index) * 10 ).ToString("D5");
						this._command += PLCConstants.PLC_CHECK_CODE;
						this._command += "\r";
						this._port.Write(this._command);
					}
					break;				
                case EESDCommand.None:
                    break;
				//-----------------------------------------------------------------
                default:
                    break;
            }

			if ( isEnableDelay )
			{
				System.Threading.Thread.Sleep(PLCConstants.DELAY20MS);         // ms, delay 20ms for PLC read command
			}

            if (isZapOK == true)
            {
                return true; //current cmd is zapping
            }
            else
            {
                return false; //Zap is busy
            }
        }

		public bool Zap(int index) // angus0626
		{
			this._isWorkingBusy = true;
			return this.RunCommand(EESDCommand.ZapON, index);
		}

		public long GetESDRelayCount()
		{
			return 100;
		}


        #endregion
		      
    }
}
