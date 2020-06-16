using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;

namespace MPI.Tester.Device.SourceMeter.LDT3ALib
{
    public class LDT3A_Lib
    {
        private bool _isLog = false;
        
        public string DLL_VER = "V1.0.0.0";

        public string DEV_SN = string.Empty;
        public string CAL_DATE = string.Empty;
        public string HW_VER = string.Empty;
        public string PCB_SN = string.Empty;
        public string QC_SW_VER = string.Empty;

        HH_Lib hwh = new HH_Lib();
        
        public SerialPort LDT3A200USB = null;

		//public byte[] Parameter = new byte[40];       
		public string Message = "";

		
		public UInt32 SerialNum = 0xFFFFFFFF, SerialNum1 = 0xFFFFFFFF;
		//----------------------------------------------
		// Private Variables 
		//----------------------------------------------
		private UInt32 serialNum2 = 0xFFFFFFFF;
		private const byte SOF = 0xFF;
		private const byte EOF = 0xFF;

		private byte[] _parameter = new byte[40]; 
		private int[] _nMemory = new int[LDevConst.MAX_MEMORY_LENGTH];
		private double[] _rtnValue = new double[10];

		private string[] _sCurveData = new string[ LDevConst.MAX_CURVE_POINTS + 1 ];
		private UInt16[][] _thy4ChRawData = new UInt16[4][];
		private double[][] _thy4ChData = new double [4][];

		private string[] _sMeasureIV = new string[LDevConst.MAX_SEQUENCE_LENGTH * 8];

		private uint _seqIndex = 0;
		private byte[] _seqHWRange = new byte[LDevConst.MAX_SEQUENCE_LENGTH];
		private byte[] _seqMasterGL = new byte[LDevConst.MAX_SEQUENCE_LENGTH];
		private int[] _seqCMP = new int[LDevConst.MAX_SEQUENCE_LENGTH];

		private byte _bCMPA			= 0x19;
		private byte _bCMPAHR		= 0x80;
		private byte _bHW_DevRange	= 0x1E;
		private byte _bNPLC_Order	= 0; 
		private byte _bTimeBase		= 100;
		private byte _bTimeCount	= 0x01;
		private byte _bExecMode		= 0x65;
		private byte _bStatus		= 0x06;
        
        private byte _bLimitCurrLow;
		private byte _bLimitCurrHigh;		

        private byte[] _byCMP_ex = new byte[2];
        private byte[] _byUSBRx;
        private byte[] _byUSBTx = new byte[99];
        private byte[] serial_num_temp = new byte[4];

		private float _voltOffset6V_1 = 0.0f;
        private float _voltOffset6V_2 = 0.0f;

        private float _voltOffset20V_1 = 0.0f;
        private float _voltOffset20V_2 = 0.0f;

        private float _voltOffset200V_1 = 0.0f;
        private float _voltOffset200V_2 = 0.0f;

		private float[] _curveAD1 = new float[LDevConst.MAX_CURVE_POINTS];
		private float[] _curveAD2 = new float[LDevConst.MAX_CURVE_POINTS];

		private int USBbytes = 0;

		private uint _curvePoints = 0;
        private int _nClampI = 0;		
        private float _refVolt_CMP_ex = 3.0f;

        private string[] _sRtnIF = new string[4];
        private string[] _sRtnVS = new string[8];
        private string[] _sVersion = new string[2];
        private string _sIRange = "1uA";

        private bool _isOverTime = false;
        private bool _isMultiDriveState = false;
        private bool _isMultiCurveState = false;
        private System.Threading.Timer loopTimer;

		private E_VRange _vRangeSet,_execVRange;
		private E_IRange _iRangeSet,_execIRange;
		
		private int[] _execData = new int[11];

		private bool _isCalcCMP_ex;
		private bool _isRun4WireVDB;

        private bool _isCompensateV;

        private MPI.PerformanceTimer _pt = new PerformanceTimer();

		public LDT3A_Lib()
		{
			for (int k = 0; k < 4; k++)
			{
				this._thy4ChRawData[k] = new UInt16[LDevConst.MAX_CURVE_POINTS];
				this._thy4ChData[k] = new double[LDevConst.MAX_CURVE_POINTS];
			}

			this._bExecMode = 0x65;			// isAutoTurnOff = true,
			
			this._bCMPA = (byte)(LDevConst.CMP_CENTER >> 8);
			this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER);
			this._bTimeBase = 100;

			this._isCalcCMP_ex = true;
			this._isRun4WireVDB = true;
            this._isCompensateV = false;
          
            this._isLog = false;
		}

		#region >>> Public Property <<<

		public E_VRange VRangeSet
		{
			get { return this._vRangeSet; }
		}

		public E_IRange IRangeSet
		{
			get { return this._iRangeSet; }
		}

		public byte Status
		{
			get { return this._bStatus; }
		}

		public string[] MeasureIV
		{
			get { return this._sMeasureIV; }
		}

		public E_VRange ExecVRange
		{
			get { return this._execVRange; }
		}

		public E_IRange ExecIRange
		{
			get { return this._execIRange; }
		}

		public int[] ExecData
		{
			get { return this._execData; }
		}

		public double[] RtnValue
		{
			get 
            {
                return this._rtnValue.Clone() as double[]; 
            } 
		}

		public double[][] Thy4ChData
		{
			get { return this._thy4ChData; }
		}

		public UInt16[][] Thy4ChRawData
		{
			get { return this._thy4ChRawData; }
		}

		public int[] FlashMemData
		{
			get { return this._nMemory; }
		}

		public byte[] Parameter
		{
			get { return this._parameter; }
			set
			{ 
				this._parameter = value;

				this.SetParameterToDev();
			}
		}

		public uint NPLC_Order
		{
			get { return (uint)this._bNPLC_Order; }
			set
			{
				if (this._bNPLC_Order == (byte)value)
					return;

                //if (value != 13)
                //{
                //    this._bNPLC_Order = 3;
                //}
                //else
                //{
                //    this._bNPLC_Order = (byte)value;
                //}

                this._bNPLC_Order = (byte)value;

				if (this._bNPLC_Order <= 1)
				{
					this._bNPLC_Order = 1;
				}

				if (this._bNPLC_Order >= 0xFE)
				{
					this._bNPLC_Order = 0xFE;
				}

				this.SetNPLCToDev();
			}			
		}

		public bool IsCalcCMP_ex
		{
			get { return this._isCalcCMP_ex; }
			set { this._isCalcCMP_ex = value; }
		}

		public bool IsRun4WireVDB
		{
			get { return this._isRun4WireVDB; }
			set { this._isRun4WireVDB = value; }
		}

        // for debugging, Roy 20160831 
        public byte CMPA
        {
            get { return this._bCMPA; }
        }

        // for debugging, Roy 20160831 
        public byte CMPAHR
        {
            get { return this._bCMPAHR; }
        }

        public bool IsCompensateV
        {
            get { return this._isCompensateV; }
			set { this._isCompensateV = value; }
        }

        public bool IsLog
        {
            get { return this._isLog; }
            set { this._isLog = value; }
        }

		#endregion

		#region >>> Private Methods 01 <<<

		private void TimerTask(object stateObj)
        {
            this._isOverTime = true;
        }

		private void SetTimeBaseToDev()
		{
			this.Message = "";
			try
			{
				if (this._isMultiDriveState == true)
				{
					this.Multi_Getdata();
				}
				if (this._isMultiCurveState == true)
				{
					this.Multi_GetCurveData();
				}
				if (this._isMultiDriveState == false && this._isMultiCurveState == false)
				{
					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetTimeBase;               // 0x54
					_byUSBTx[4] = this._bTimeBase;
					_byUSBTx[5] = 0xFF;
					
					this.LDT3A200USB.Write(_byUSBTx, 0, 6);
					int USBbytes = this.LDT3A200USB.BytesToRead;
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					while (USBbytes < 3)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);

						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();
					
					byte[] _byUSBRx = new byte[USBbytes];
					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x54)
					{
						this.Message = this.Message + "communication error\n";
					}
				}
			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}
		}

		private void SetStatusToDev()
		{
			this.Message = "";
			try
			{
				if (this._isMultiDriveState == true)
				{
					this.Multi_Getdata();
				}
				if (this._isMultiCurveState == true)
				{
					this.Multi_GetCurveData();
				}

				if (this._isMultiDriveState == false && this._isMultiCurveState == false)
				{
					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
					_byUSBTx[3] = (byte)ERegCmd.SetStatus;              //0x73
					_byUSBTx[4] = this._bStatus;
					_byUSBTx[5] = 0xFF;

					this.LDT3A200USB.Write(_byUSBTx, 0, 6);

					int USBbytes = this.LDT3A200USB.BytesToRead;
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x73 status(1byte) = 4 bytes
					//-------------------------------------------------------------------------------------------------
					while (USBbytes < 4)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();

					byte[] _byUSBRx = new byte[USBbytes];
					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					this._bStatus = _byUSBRx[3];
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x73)
					{
						this.Message = this.Message + "communication error\n";
					}
				}
			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}

		}

		private void SetParameterToDev()
		{
			this.Message = "";
			try
			{
				if (this._isMultiDriveState == true)
				{
					this.Multi_Getdata();
				}
				if (this._isMultiCurveState == true)
				{
					this.Multi_GetCurveData();
				}
				if (this._isMultiDriveState == false && this._isMultiCurveState == false)
				{
					for (int p = 0; p < this._parameter.Length; p++)
					{
						if (this._parameter[p] == 0xFF)
						{
							this._parameter[p] = 0xFE;
						}
					}

					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;

					_byUSBTx[2] = SOF;
					_byUSBTx[3] = (byte)ERegCmd.SetParameter;                  //0x70

					//_byUSBTx[4] = paramData[0];
					//_byUSBTx[43] = paramData[39];

					for (int p = 4; p <= 43; p++)
					{
						if (this._parameter[p - 4] != null)
						{
							_byUSBTx[p] = this._parameter[p - 4];		// from _byUSBTx[4] to from _byUSBTx[43], paramData.Length = 40 
						}
					}

					_byUSBTx[44] = 0xFF;

					this.LDT3A200USB.Write(_byUSBTx, 0, 45);
					int USBbytes = this.LDT3A200USB.BytesToRead;
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					while (USBbytes < 3)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();

					byte[] _byUSBRx = new byte[USBbytes];
					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x70)
					{
						this.Message = this.Message + "communication error\n";
					}
				}
			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}
		}

		private void SetNPLCToDev()
		{
			this.Message = "";

			if (this._isMultiDriveState == true)
			{
				this.Multi_Getdata();
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
			}

			
			if (this._isMultiDriveState == false && this._isMultiCurveState == false)
			{

				try
				{
					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
					_byUSBTx[3] = (byte)ERegCmd.SetNPLC;            //0x4E
					_byUSBTx[4] = this._bNPLC_Order;
					_byUSBTx[5] = 0xFF;

					this.LDT3A200USB.Write(_byUSBTx, 0, 6);
					int USBbytes = this.LDT3A200USB.BytesToRead;
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					while (USBbytes < 3)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();

					byte[] _byUSBRx = new byte[USBbytes];
					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x4E)
					{
						this.Message = this.Message + "communication error\n";
					}
				}
				catch (Exception e1)
				{
					this.Message = e1.ToString();
				}
			}
		}

		private void SetForceTime(uint sequemceIndex,double msTims)
		{
            if (msTims <= 0)
			{
				msTims = 0;
			}

			this._bTimeCount = (byte)(this._bTimeBase * msTims / 100);

			if (this._bTimeCount <= 0x01) { this._bTimeCount = 0x01; }
			if (this._bTimeCount >= 0xFE) { this._bTimeCount = 0xFE; }
		}

        private void SetForceTime(double msTims)
        {
            byte timeBase = 10;
            double resolution = 0.1d;  // ms

            // F/W Time Base Setting
            for (int i = 10; i < (int)Byte.MaxValue; i++)
            {
                timeBase = (byte)i;

                resolution = Math.Round(timeBase * LDevConst.FIRMWARE_TIMER_RESOLUTION, 3, MidpointRounding.AwayFromZero);

                double timeMaxLimit = (int)Byte.MaxValue * resolution;

                if (msTims >= resolution && msTims <= timeMaxLimit)
                {
                    break;
                }
            }

            this._bTimeCount = (byte)(msTims / LDevConst.FIRMWARE_TIMER_RESOLUTION / timeBase - 1);

            this.SetTimeBase(timeBase);
        }

		private void cmd_converter(float V, float I, string I_unit)
		{
			int addr = 0, temp = 0;
			float temp1, temp2;
			int _byCMP_ex_temp = 0;

			#region >> (01) 由輸入I, 調整數據與單位，對應內部定義的檔位

			if (I < 0f) I = 0f;

			if (I_unit == "uA")
			{
				if (I < 1f)
				{
					this._sIRange = "1uA";
				}
				else if (I <= 10f)
				{
					this._sIRange = "10uA";
				}
				//else if (I <= 100f)
				//{
				//    this._sIRange = "100uA";
				//}
				else if (I <= 1000f)
				{
					this._sIRange = "1mA";
				}
				else
				{
					I = 1000f;
					this._sIRange = "1mA";
				}
			}
			else if (I_unit == "mA")
			{
				if (I <= 1f)
				{
					I = (float)I * 1000f;
					if (I < 1f)
					{
						this._sIRange = "1uA";
					}
					else if (I <= 10f)
					{
						this._sIRange = "10uA";
					}
					//else if (I <= 100)
					//{
					//    this._sIRange = "100uA";
					//}
					else if (I <= 1000f)
					{
						this._sIRange = "1mA";
					}
				}
				//else if (I <= 10f)
				//{
				//    this._sIRange = "10mA";
				//}
				else if (I <= 100f)
				{
					this._sIRange = "100mA";
				}
				else if (I <= 400f)
				{
					this._sIRange = "800mA";
				}
				else if (I <= 1000f)
				{
					this._sIRange = "2A";
				}
				else if (I <= 3000f)
				{
					this._sIRange = "3A";
				}
				else
				{
					I = 3000f;
					this._sIRange = "3A";
				}
			}
			else if (I_unit == "A")
			{
				I = (float)I * 1000f;
				if (I <= 1f)
				{
					I = (float)I * 1000f;
					if (I < 1f)
					{
						this._sIRange = "1uA";
					}
					else if (I <= 10f)
					{
						this._sIRange = "10uA";
					}
					//else if (I <= 100f)
					//{
					//    this._sIRange = "100uA";
					//}
					else if (I <= 1000f)
					{
						this._sIRange = "1mA";
					}
				}
				//else if (I <= 10f)
				//{
				//    this._sIRange = "10mA";
				//}
				else if (I <= 100f)
				{
					this._sIRange = "100mA";
				}
				else if (I <= 400f)
				{
					this._sIRange = "800mA";
				}
				else if (I <= 1000f)
				{
					this._sIRange = "2A";
				}
				else if (I <= 3000f)
				{
					this._sIRange = "3A";
				}
				else
				{
					I = 3000f;
					this._sIRange = "3A";
				}
			}

			#endregion

			#region >> (02) 由輸入I, 查表內插設定 limit_I 的 HIGH byte & LOW byte 並計算設定 iRange (含masterGainLoop)
			//------------------------------------------------------------------------------------------------------
			// _nMemory[i] = 0x0000 ~ 0xFFFF, 存放的是 2 bytes 的資料。
			// _nClampI 是用 0x0000 ~ 0x0FFF, 的 12bit 資料的表達，直接對應到 ADC 的紀錄能力。
			// _bLimitCurrLow 與 _bLimitCurrHigh 為一個 byte 的資料，用於 USB 的傳送，所以需要避開 0xFF。
			//------------------------------------------------------------------------------------------------------
			if (this._sIRange == "1uA")
			{
				if (V >= 0f)
				{
					if (I <= 0.0005f) _nClampI = 0;
					else if (I <= 0.001f) _nClampI = Convert.ToUInt16((I / 0.001f * (float)(_nMemory[385] - _nMemory[384]) + (float)_nMemory[384]) / 16f);
					else if (I <= 0.01f) _nClampI = Convert.ToUInt16(((I - 0.001f) / 0.009f * (float)(_nMemory[386] - _nMemory[385]) + (float)_nMemory[385]) / 16f);
					else if (I <= 0.1f) _nClampI = Convert.ToUInt16(((I - 0.01f) / 0.09f * (float)(_nMemory[387] - _nMemory[386]) + (float)_nMemory[386]) / 16f);
					else if (I <= 0.5f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.4f * (float)(_nMemory[388] - _nMemory[387]) + (float)_nMemory[387]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 0.5f) / 0.5f * (float)(_nMemory[389] - _nMemory[388]) + (float)_nMemory[388]) / 16f);
				}
				else
				{
					if (I <= 0.0005f) _nClampI = 0;
					else if (I <= 0.001f) _nClampI = Convert.ToUInt16((I / 0.001f * (float)(_nMemory[393] - _nMemory[392]) + (float)_nMemory[392]) / 16f);
					else if (I <= 0.01f) _nClampI = Convert.ToUInt16(((I - 0.001f) / 0.009f * (float)(_nMemory[394] - _nMemory[393]) + (float)_nMemory[393]) / 16f);
					else if (I <= 0.1f) _nClampI = Convert.ToUInt16(((I - 0.01f) / 0.09f * (float)(_nMemory[395] - _nMemory[394]) + (float)_nMemory[394]) / 16f);
					else if (I <= 0.5f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.4f * (float)(_nMemory[396] - _nMemory[395]) + (float)_nMemory[395]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 0.5f) / 0.5f * (float)(_nMemory[397] - _nMemory[396]) + (float)_nMemory[396]) / 16f);
				}

				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte(((_nClampI * 2) / 256) + 0x80);
                this._bHW_DevRange = (byte)ERegRange._1uA;
			}
			else if (this._sIRange == "10uA")
			{
				if (V >= 0f)
				{
					if (I <= 0.01f) _nClampI = Convert.ToUInt16((I / 0.01f * (float)(_nMemory[401] - _nMemory[400]) + (float)_nMemory[400]) / 16f);
					else if (I <= 0.1f) _nClampI = Convert.ToUInt16(((I - 0.01f) / 0.09f * (float)(_nMemory[402] - _nMemory[401]) + (float)_nMemory[401]) / 16f);
					else if (I <= 1f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.9f * (float)(_nMemory[403] - _nMemory[402]) + (float)_nMemory[402]) / 16f);
					else if (I <= 5f) _nClampI = Convert.ToUInt16(((I - 1f) / 4f * (float)(_nMemory[404] - _nMemory[403]) + (float)_nMemory[403]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 5f) / 5f * (float)(_nMemory[405] - _nMemory[404]) + (float)_nMemory[404]) / 16f);
				}
				else
				{
					if (I <= 0.01f) _nClampI = Convert.ToUInt16((I / 0.01f * (float)(_nMemory[409] - _nMemory[408]) + (float)_nMemory[408]) / 16f);
					else if (I <= 0.1f) _nClampI = Convert.ToUInt16(((I - 0.01f) / 0.09f * (float)(_nMemory[410] - _nMemory[409]) + (float)_nMemory[409]) / 16f);
					else if (I <= 1f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.9f * (float)(_nMemory[411] - _nMemory[410]) + (float)_nMemory[410]) / 16f);
					else if (I <= 5f) _nClampI = Convert.ToUInt16(((I - 1f) / 4f * (float)(_nMemory[412] - _nMemory[411]) + (float)_nMemory[411]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 5f) / 5f * (float)(_nMemory[413] - _nMemory[412]) + (float)_nMemory[412]) / 16f);
				}
				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte((_nClampI * 2) / 256);
				this._bHW_DevRange = (byte)ERegRange._10uA;
			}
			else if (this._sIRange == "100uA")
			{
				if (V >= 0f)
				{
					if (I <= 0.1f) _nClampI = Convert.ToUInt16((I / 0.1f * (float)(_nMemory[417] - _nMemory[416]) + (float)_nMemory[416]) / 16f);
					else if (I <= 1f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.9f * (float)(_nMemory[418] - _nMemory[417]) + (float)_nMemory[417]) / 16f);
					else if (I <= 10f) _nClampI = Convert.ToUInt16(((I - 1f) / 9f * (float)(_nMemory[419] - _nMemory[418]) + (float)_nMemory[418]) / 16f);
					else if (I <= 50f) _nClampI = Convert.ToUInt16(((I - 10f) / 40f * (float)(_nMemory[420] - _nMemory[419]) + (float)_nMemory[419]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 50f) / 50f * (float)(_nMemory[421] - _nMemory[420]) + (float)_nMemory[420]) / 16f);
				}
				else
				{
					if (I <= 0.1f) _nClampI = Convert.ToUInt16((I / 0.1f * (float)(_nMemory[425] - _nMemory[424]) + (float)_nMemory[424]) / 16f);
					else if (I <= 1f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.9f * (float)(_nMemory[426] - _nMemory[425]) + (float)_nMemory[425]) / 16f);
					else if (I <= 10f) _nClampI = Convert.ToUInt16(((I - 1f) / 9f * (float)(_nMemory[427] - _nMemory[426]) + (float)_nMemory[426]) / 16f);
					else if (I <= 50f) _nClampI = Convert.ToUInt16(((I - 10f) / 40f * (float)(_nMemory[428] - _nMemory[427]) + (float)_nMemory[427]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 50f) / 50f * (float)(_nMemory[429] - _nMemory[428]) + (float)_nMemory[428]) / 16f);
				}
				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte(((_nClampI * 2) / 256) + 0x80);
				this._bHW_DevRange = (byte)ERegRange._100uA;
			}
			else if (this._sIRange == "1mA")
			{
				if (V >= 0f)
				{
					if (I <= 1f) _nClampI = Convert.ToUInt16((I / 1f * (float)(_nMemory[433] - _nMemory[432]) + (float)_nMemory[432]) / 16f);
					else if (I <= 10f) _nClampI = Convert.ToUInt16(((I - 1f) / 9f * (float)(_nMemory[434] - _nMemory[433]) + (float)_nMemory[433]) / 16f);
					else if (I <= 100f) _nClampI = Convert.ToUInt16(((I - 10f) / 90f * (float)(_nMemory[435] - _nMemory[434]) + (float)_nMemory[434]) / 16f);
					else if (I <= 500f) _nClampI = Convert.ToUInt16(((I - 100f) / 400f * (float)(_nMemory[436] - _nMemory[435]) + (float)_nMemory[435]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 500f) / 500f * (float)(_nMemory[437] - _nMemory[436]) + (float)_nMemory[436]) / 16f);
				}
				else
				{
					if (I <= 1f) _nClampI = Convert.ToUInt16((I / 1f * (float)(_nMemory[441] - _nMemory[440]) + (float)_nMemory[440]) / 16f);
					else if (I <= 10f) _nClampI = Convert.ToUInt16(((I - 1f) / 9f * (float)(_nMemory[442] - _nMemory[441]) + (float)_nMemory[441]) / 16f);
					else if (I <= 100f) _nClampI = Convert.ToUInt16(((I - 10f) / 90f * (float)(_nMemory[443] - _nMemory[442]) + (float)_nMemory[442]) / 16f);
					else if (I <= 500f) _nClampI = Convert.ToUInt16(((I - 100f) / 400f * (float)(_nMemory[444] - _nMemory[443]) + (float)_nMemory[443]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 500f) / 500f * (float)(_nMemory[445] - _nMemory[444]) + (float)_nMemory[444]) / 16f);
				}
				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte((_nClampI * 2) / 256);
				this._bHW_DevRange = (byte)ERegRange._1mA;
			}
			else if (this._sIRange == "10mA")
			{
				if (V >= 0f)
				{
					if (I <= 0.01f) _nClampI = Convert.ToUInt16((I / 0.01f * (float)(_nMemory[449] - _nMemory[448]) + (float)_nMemory[448]) / 16f);
					else if (I <= 0.1f) _nClampI = Convert.ToUInt16(((I - 0.01f) / 0.09f * (float)(_nMemory[450] - _nMemory[449]) + (float)_nMemory[449]) / 16f);
					else if (I <= 1f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.9f * (float)(_nMemory[451] - _nMemory[450]) + (float)_nMemory[450]) / 16f);
					else if (I <= 5f) _nClampI = Convert.ToUInt16(((I - 1f) / 4f * (float)(_nMemory[452] - _nMemory[451]) + (float)_nMemory[451]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 5f) / 5f * (float)(_nMemory[453] - _nMemory[452]) + (float)_nMemory[452]) / 16f);
				}
				else
				{
					if (I <= 0.01f) _nClampI = Convert.ToUInt16((I / 0.01f * (float)(_nMemory[457] - _nMemory[456]) + (float)_nMemory[456]) / 16f);
					else if (I <= 0.1f) _nClampI = Convert.ToUInt16(((I - 0.01f) / 0.09f * (float)(_nMemory[458] - _nMemory[457]) + (float)_nMemory[457]) / 16f);
					else if (I <= 1f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.9f * (float)(_nMemory[459] - _nMemory[458]) + (float)_nMemory[458]) / 16f);
					else if (I <= 5f) _nClampI = Convert.ToUInt16(((I - 1f) / 4f * (float)(_nMemory[460] - _nMemory[459]) + (float)_nMemory[459]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 5f) / 5f * (float)(_nMemory[461] - _nMemory[460]) + (float)_nMemory[460]) / 16f);
				}
				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte(((_nClampI * 2) / 256) + 0x80);
				this._bHW_DevRange = (byte)ERegRange._10mA;
			}
			else if (this._sIRange == "100mA")
			{
				if (V >= 0f)
				{
					if (I <= 0.1f) _nClampI = Convert.ToUInt16((I / 0.1f * (float)(_nMemory[465] - _nMemory[464]) + (float)_nMemory[464]) / 16f);
					else if (I <= 1f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.9f * (float)(_nMemory[466] - _nMemory[465]) + (float)_nMemory[465]) / 16f);
					else if (I <= 10f) _nClampI = Convert.ToUInt16(((I - 1f) / 9f * (float)(_nMemory[467] - _nMemory[466]) + (float)_nMemory[466]) / 16f);
					else if (I <= 50f) _nClampI = Convert.ToUInt16(((I - 10f) / 40f * (float)(_nMemory[468] - _nMemory[467]) + (float)_nMemory[467]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 50f) / 50f * (float)(_nMemory[469] - _nMemory[468]) + (float)_nMemory[468]) / 16f);
				}
				else
				{
					if (I <= 0.1f) _nClampI = Convert.ToUInt16((I / 0.1f * (float)(_nMemory[473] - _nMemory[472]) + (float)_nMemory[472]) / 16f);
					else if (I <= 1f) _nClampI = Convert.ToUInt16(((I - 0.1f) / 0.9f * (float)(_nMemory[474] - _nMemory[473]) + (float)_nMemory[473]) / 16f);
					else if (I <= 10f) _nClampI = Convert.ToUInt16(((I - 1f) / 9f * (float)(_nMemory[475] - _nMemory[474]) + (float)_nMemory[474]) / 16f);
					else if (I <= 50f) _nClampI = Convert.ToUInt16(((I - 10f) / 40f * (float)(_nMemory[476] - _nMemory[475]) + (float)_nMemory[475]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 50f) / 50f * (float)(_nMemory[477] - _nMemory[476]) + (float)_nMemory[476]) / 16f);
				}
				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte((_nClampI * 2) / 256);
				this._bHW_DevRange = (byte)ERegRange._100mA;
			}
			else if (this._sIRange == "800mA")
			{
				if (V >= 0f)
				{
					if (I <= 0.8f) _nClampI = Convert.ToUInt16((I / 0.8f * (float)(_nMemory[481] - _nMemory[480]) + (float)_nMemory[480]) / 16f);
					else if (I <= 8f) _nClampI = Convert.ToUInt16(((I - 0.8f) / 7.2f * (float)(_nMemory[482] - _nMemory[481]) + (float)_nMemory[481]) / 16f);
					else if (I <= 80f) _nClampI = Convert.ToUInt16(((I - 8f) / 72f * (float)(_nMemory[483] - _nMemory[482]) + (float)_nMemory[482]) / 16f);
					else if (I <= 400f) _nClampI = Convert.ToUInt16(((I - 80f) / 320f * (float)(_nMemory[484] - _nMemory[483]) + (float)_nMemory[483]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 400f) / 400f * (float)(_nMemory[485] - _nMemory[484]) + (float)_nMemory[484]) / 16f);
				}
				else
				{
					if (I <= 0.8f) _nClampI = Convert.ToUInt16((I / 0.8f * (float)(_nMemory[489] - _nMemory[488]) + (float)_nMemory[488]) / 16f);
					else if (I <= 8f) _nClampI = Convert.ToUInt16(((I - 0.8f) / 7.2f * (float)(_nMemory[490] - _nMemory[489]) + (float)_nMemory[489]) / 16f);
					else if (I <= 80f) _nClampI = Convert.ToUInt16(((I - 8f) / 72f * (float)(_nMemory[491] - _nMemory[490]) + (float)_nMemory[490]) / 16f);
					else if (I <= 400f) _nClampI = Convert.ToUInt16(((I - 80f) / 320f * (float)(_nMemory[492] - _nMemory[491]) + (float)_nMemory[491]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 400f) / 400f * (float)(_nMemory[493] - _nMemory[492]) + (float)_nMemory[492]) / 16f);
				}
				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte(((_nClampI * 2) / 256) + 0x80);
				this._bHW_DevRange = (byte)ERegRange._800mA;
			}
			else if (this._sIRange == "2A")
			{
				if (V >= 0f)
				{
					if (I <= 2f) _nClampI = Convert.ToUInt16((I / 2f * (float)(_nMemory[497] - _nMemory[496]) + (float)_nMemory[496]) / 16f);
					else if (I <= 20f) _nClampI = Convert.ToUInt16(((I - 2f) / 18f * (float)(_nMemory[498] - _nMemory[497]) + (float)_nMemory[497]) / 16f);
					else if (I <= 200f) _nClampI = Convert.ToUInt16(((I - 20f) / 180f * (float)(_nMemory[499] - _nMemory[498]) + (float)_nMemory[498]) / 16f);
					else if (I <= 1000f) _nClampI = Convert.ToUInt16(((I - 200f) / 800f * (float)(_nMemory[500] - _nMemory[499]) + (float)_nMemory[499]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 1000f) / 1000f * (float)(_nMemory[501] - _nMemory[500]) + (float)_nMemory[500]) / 16f);
				}
				else
				{
					if (I <= 2f) _nClampI = Convert.ToUInt16((I / 2f * (float)(_nMemory[505] - _nMemory[504]) + (float)_nMemory[504]) / 16f);
					else if (I <= 20f) _nClampI = Convert.ToUInt16(((I - 2f) / 18f * (float)(_nMemory[506] - _nMemory[505]) + (float)_nMemory[505]) / 16f);
					else if (I <= 200f) _nClampI = Convert.ToUInt16(((I - 20f) / 180f * (float)(_nMemory[507] - _nMemory[506]) + (float)_nMemory[506]) / 16f);
					else if (I <= 1000f) _nClampI = Convert.ToUInt16(((I - 200f) / 800f * (float)(_nMemory[508] - _nMemory[507]) + (float)_nMemory[507]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 1000f) / 1000f * (float)(_nMemory[509] - _nMemory[508]) + (float)_nMemory[508]) / 16f);
				}
				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte(((_nClampI * 2) / 256) + 0x80);
				this._bHW_DevRange = (byte)ERegRange._2A;
			}
			else if (this._sIRange == "3A")
			{
				if (V >= 0f)
				{
					if (I <= 3f) _nClampI = Convert.ToUInt16((I / 3f * (float)(_nMemory[513] - _nMemory[512]) + (float)_nMemory[512]) / 16f);
					else if (I <= 30f) _nClampI = Convert.ToUInt16(((I - 3f) / 27f * (float)(_nMemory[514] - _nMemory[513]) + (float)_nMemory[513]) / 16f);
					else if (I <= 300f) _nClampI = Convert.ToUInt16(((I - 30f) / 270f * (float)(_nMemory[515] - _nMemory[514]) + (float)_nMemory[514]) / 16f);
					else if (I <= 1500f) _nClampI = Convert.ToUInt16(((I - 300f) / 1200f * (float)(_nMemory[516] - _nMemory[515]) + (float)_nMemory[515]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 1500f) / 1500f * (float)(_nMemory[517] - _nMemory[516]) + (float)_nMemory[516]) / 16f);
				}
				else
				{
					if (I <= 3f) _nClampI = Convert.ToUInt16((I / 3f * (float)(_nMemory[521] - _nMemory[520]) + (float)_nMemory[520]) / 16f);
					else if (I <= 30f) _nClampI = Convert.ToUInt16(((I - 3f) / 27f * (float)(_nMemory[522] - _nMemory[521]) + (float)_nMemory[521]) / 16f);
					else if (I <= 300f) _nClampI = Convert.ToUInt16(((I - 30f) / 270f * (float)(_nMemory[523] - _nMemory[522]) + (float)_nMemory[522]) / 16f);
					else if (I <= 1500f) _nClampI = Convert.ToUInt16(((I - 300f) / 1200f * (float)(_nMemory[524] - _nMemory[523]) + (float)_nMemory[523]) / 16f);
					else _nClampI = Convert.ToUInt16(((I - 1500f) / 1500f * (float)(_nMemory[525] - _nMemory[524]) + (float)_nMemory[524]) / 16f);
				}
				_bLimitCurrLow = Convert.ToByte((_nClampI * 2) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte(((_nClampI * 2) / 256) + 0x80);
				this._bHW_DevRange = (byte)ERegRange._3A;
			}
			else
			{ 
				this.Message = this.Message + "_sIRange error!!\n"; 
			}

			#endregion

			#region >> (03) 由輸入V, 查表內插設定 _bCMPA & _bCMPAHR & _byCMP_ex[0] & _byCMP_ex[1] & _byCMP_ex_temp 並計算設定 vRange

			if (V > 205f)
			{
				V = 205f;
			}
			if (V < -205f)
			{
				V = -205f;
			}

			if (V >= 0f)
			{
				this._refVolt_CMP_ex = V + 3f;					// Gilbert ?

				if (V <= 6.001f)
				{
					this._bHW_DevRange = Convert.ToByte(_bHW_DevRange + 0x40);
					
					addr = 0;							// 對 +6V Range 用 VF1 ~ VF24, addr = 0 ~ 23, 內插算出 CMPA 與 CMPAHR
					while (addr < 24)
					{
						temp1 = (float)_nMemory[addr] * LDevConst.OP6VRangeOut / 32767f;
						if (V <= temp1)
						{
							if (addr == 0)				// 靠近0V _bCMPA = 1980
							{
								_bCMPA = 0x19;
								_bCMPAHR = 0x80;
							}
							else
							{
								temp2 = (float)_nMemory[addr - 1] * LDevConst.OP6VRangeOut / 32767f;
								if ((temp1 - temp2) == 0)
								{
									temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
								}
								else
								{
									temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);    // Terrell_96修改成97區間
								}

								if (temp < 49)														// 小於中間值 使用前一個_nMemory區間 48改成49
								{
									_bCMPA = Convert.ToByte(addr + 0x18);
									_bCMPAHR = Convert.ToByte(temp + 0x80);
								}
								else
								{
									_bCMPA = Convert.ToByte(addr + 0x19);
									_bCMPAHR = Convert.ToByte(0x4F + temp - 48);					// 47改成48
								}
							}
							addr = 24;
						}
						else
						{
							addr = addr + 1;
						}
					}

					addr = 1;
					while (addr < 24)
					{
						temp1 = (float)_nMemory[addr] * LDevConst.OP6VRangeOut / 32767f;
						if (this._refVolt_CMP_ex <= temp1)
						{
							temp2 = (float)_nMemory[addr - 1] * LDevConst.OP6VRangeOut / 32767f;

							if ((temp1 - temp2) == 0)
							{
								temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
							}
							else
							{
								temp = Convert.ToUInt16((this._refVolt_CMP_ex - temp2) / (temp1 - temp2) * 97);				// Terrell_需要修改97
							}

							if (temp < 49)																			// 48改成49
							{
								_byCMP_ex_temp = (addr + 0x18 - _bCMPA) * 0x61 + (temp + 0x80) - _bCMPAHR;			// 0x61 97-->  _bCMPA * 97 與 _bCMPAHR 相加   _byCMP_ex_temp ----> 3V CMP
							}
							else
							{
								_byCMP_ex_temp = (addr + 0x19 - _bCMPA) * 0x61 + (0x4F + temp - 48) - _bCMPAHR;	// 47改成48
							}
							_byCMP_ex_temp = _byCMP_ex_temp * 32767 / _nMemory[339];								// 3V 比例
							_byCMP_ex[0] = Convert.ToByte(_byCMP_ex_temp & 0xFE);									// _byCMP_ex low byte  FE = 1111 1110
							_byCMP_ex[1] = Convert.ToByte((_byCMP_ex_temp / 256) & 0x7F);							// _byCMP_ex high byte 7F = 0111 1111
							addr = 24;
						}
						else
						{
							addr = addr + 1;
						}
					}
				}
				else if (V <= 20.001f)						
				{
					this._bHW_DevRange = Convert.ToByte(this._bHW_DevRange + 0x60);

					addr = 94;									// 對 +20V Range 用 VF48 ~ VF91, addr = 94 ~ 117, 內插算出 CMPA 與 CMPAHR
					while (addr < 118)
					{
						temp1 = (float)_nMemory[addr] * LDevConst.OP20VRangeOut / 32767f;
						if (V <= temp1)
						{
							if (addr == 94)
							{
								_bCMPA = 0x19;
								_bCMPAHR = 0x80;
							}
							else
							{
								temp2 = (float)_nMemory[addr - 1] * LDevConst.OP20VRangeOut / 32767f;
								if ((temp1 - temp2) == 0)
								{
									temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
								}
								else
								{
									temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);				// Terrell_需要修改97
								}

								if (temp < 49) //48改成49
								{
									_bCMPA = Convert.ToByte(addr - 94 + 0x18);
									_bCMPAHR = Convert.ToByte(temp + 0x80);
								}
								else
								{
									_bCMPA = Convert.ToByte(addr - 94 + 0x19);
									_bCMPAHR = Convert.ToByte(0x4F + temp - 48);							// 47改成48
								}
							}
							addr = 118;
						}
						else
						{
							addr = addr + 1;
						}
					}

					addr = 95;
					while (addr < 118)
					{
						temp1 = (float)_nMemory[addr] * LDevConst.OP20VRangeOut / 32767f;
						if (this._refVolt_CMP_ex <= temp1)
						{
							temp2 = (float)_nMemory[addr - 1] * LDevConst.OP20VRangeOut / 32767f;
							if ((temp1 - temp2) == 0)
							{
								temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
							}
							else
							{
								temp = Convert.ToUInt16((this._refVolt_CMP_ex - temp2) / (temp1 - temp2) * 97);							// Terrell_需要修改97
							}

							if (temp < 49) //48改成49
							{
								_byCMP_ex_temp = (addr - 94 + 0x18 - _bCMPA) * 0x61 + (temp + 0x80) - _bCMPAHR;
							}
							else
							{
								_byCMP_ex_temp = (addr - 94 + 0x19 - _bCMPA) * 0x61 + (0x4F + temp - 48) - _bCMPAHR;			// 47改成48
							}
							_byCMP_ex_temp = _byCMP_ex_temp * 32767 / _nMemory[339];
							_byCMP_ex[0] = Convert.ToByte(_byCMP_ex_temp & 0xFE);
							_byCMP_ex[1] = Convert.ToByte((_byCMP_ex_temp / 256) & 0x7F);
							addr = 118;
						}
						else
						{
							addr = addr + 1;
						}
					}
				}
				else
				{
					this._bHW_DevRange = Convert.ToByte(this._bHW_DevRange + 0xA0);

					addr = 188;							// 對 +200V Range 用 VF95 ~ VF118, addr = 188 ~ 211, 內插算出 CMPA 與 CMPAHR
					while (addr < 212)
					{
						temp1 = (float)_nMemory[addr] * LDevConst.OP200VRangeOut / 32767f;
						if (V <= temp1)
						{
							if (addr == 188)
							{
								_bCMPA = 0x19;
								_bCMPAHR = 0x80;
							}
							else
							{
								temp2 = (float)_nMemory[addr - 1] * LDevConst.OP200VRangeOut / 32767f;
								if ((temp1 - temp2) == 0)
								{
									temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
								}
								else
								{
									temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);			// Terrell_20160613
								}

								if (temp < 49)															// Terrell_20160613
								{
									_bCMPA = Convert.ToByte(addr - 188 + 0x18);
									_bCMPAHR = Convert.ToByte(temp + 0x80);
								}
								else
								{
									_bCMPA = Convert.ToByte(addr - 188 + 0x19);
									_bCMPAHR = Convert.ToByte(0x4F + temp - 48);						// Terrell_20160613
								}
							}
							addr = 212;
						}
						else
						{
							addr = addr + 1;
							if (addr == 212)
							{
								_bCMPA = 0x30;
								_bCMPAHR = 0x80;
							}
						}
					}

					addr = 189;
					while (addr < 212)
					{
						temp1 = (float)_nMemory[addr] * LDevConst.OP200VRangeOut / 32767f;
						if (this._refVolt_CMP_ex <= temp1)
						{
							temp2 = (float)_nMemory[addr - 1] * LDevConst.OP200VRangeOut / 32767f;
							if ((temp1 - temp2) == 0)
							{
								temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
							}
							else
							{
								temp = Convert.ToUInt16((this._refVolt_CMP_ex - temp2) / (temp1 - temp2) * 97);							// Terrell_20160613\
							}
							if (temp < 49) //Terrell_20160613
							{
								_byCMP_ex_temp = (addr - 188 + 0x18 - _bCMPA) * 0x61 + (temp + 0x80) - _bCMPAHR;
							}
							else
							{
								_byCMP_ex_temp = (addr - 188 + 0x19 - _bCMPA) * 0x61 + (0x4F + temp - 48) - _bCMPAHR;			// Terrell_20160613
							}
							_byCMP_ex_temp = _byCMP_ex_temp * 32767 / _nMemory[339];
							_byCMP_ex[0] = Convert.ToByte(_byCMP_ex_temp & 0xFE);
							_byCMP_ex[1] = Convert.ToByte((_byCMP_ex_temp / 256) & 0x7F);
							addr = 212;
						}
						else
						{
							addr = addr + 1;
						}
					}
				}
			}
			else
			{
				this._refVolt_CMP_ex = V - 3f;			// Gilbert ??
				if (V >= -6.001f)
				{
					this._bHW_DevRange = Convert.ToByte(this._bHW_DevRange + 0x40);

					addr = 24;					// 對 -6V Range 用 VF25 ~ VF47, addr = 24 ~ 46, 內插算出 CMPA 與 CMPAHR
					while (addr < 47)
					{
						temp1 = -(float)_nMemory[addr] * LDevConst.OP6VRangeOut / 32767f;
						if (V >= temp1)
						{
							if (addr == 24)
							{
								temp2 = (float)_nMemory[0] * LDevConst.OP6VRangeOut / 32767f;
								if ((temp1 - temp2) == 0)
								{
									temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
								}
								else
								{
									temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);				// Terrell_20160613
								}

								if (temp <= 49) //Terrell_20160613
								{
									_bCMPA = Convert.ToByte(24 - addr + 0x19);
									_bCMPAHR = Convert.ToByte(0x80 - temp);
								}
								else
								{
									_bCMPA = Convert.ToByte(24 - addr + 0x18);
									_bCMPAHR = Convert.ToByte(0x80 + 97 - temp);							// Terrell_20160613 96-->97
								}
							}
							else
							{
								temp2 = -(float)_nMemory[addr - 1] * LDevConst.OP6VRangeOut / 32767f;
								if ((temp1 - temp2) == 0)
								{
									temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
								}
								else
								{
									temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);					// Terrell_20160613
								}

								if (temp <= 49)   //Terrell_20160613
								{
									_bCMPA = Convert.ToByte(24 - addr + 0x19);
									_bCMPAHR = Convert.ToByte(0x80 - temp);
								}
								else
								{
									_bCMPA = Convert.ToByte(24 - addr + 0x18);
									_bCMPAHR = Convert.ToByte(0x80 + 97 - temp);							// Terrell_20160613 96-->97
								}
							}
							addr = 47;
						}
						else
						{
							addr = addr + 1;
						}
					}

					addr = 25;
					while (addr < 47)
					{
						temp1 = -(float)_nMemory[addr] * LDevConst.OP6VRangeOut / 32767f;
						if (this._refVolt_CMP_ex >= temp1)
						{
							temp2 = -(float)_nMemory[addr - 1] * LDevConst.OP6VRangeOut / 32767f;
							if ((temp1 - temp2) == 0)
							{
								temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
							}
							else
							{
								temp = Convert.ToUInt16((this._refVolt_CMP_ex - temp2) / (temp1 - temp2) * 97);							// Terrell_20160613 96-->97
							}

							if (temp < 49) //Terrell_20160613 48-->49
							{
								_byCMP_ex_temp = (_bCMPA - (24 - addr + 0x19)) * 0x61 + _bCMPAHR - (0x80 - temp);
							}
							else
							{
								_byCMP_ex_temp = (_bCMPA - (24 - addr + 0x18)) * 0x61 + _bCMPAHR - (0x80 + 97 - temp);			// Terrell_20160613 96-->97
							}
							_byCMP_ex_temp = _byCMP_ex_temp * 32767 / _nMemory[343];
							_byCMP_ex[0] = Convert.ToByte(_byCMP_ex_temp & 0xFE);
							_byCMP_ex[1] = Convert.ToByte((_byCMP_ex_temp / 256) & 0x7F);
							addr = 47;
						}
						else
						{
							addr = addr + 1;
						}
					}
				}
				else if (V >= -20.001f)
				{
					this._bHW_DevRange = Convert.ToByte(this._bHW_DevRange + 0x60);

					addr = 118;						// 對 -20V Range 用 VF72 ~ VF94, addr = 118 ~ 140, 內插算出 CMPA 與 CMPAHR
					while (addr < 141)
					{
						temp1 = -(float)_nMemory[addr] * LDevConst.OP20VRangeOut / 32767f;
						if (V >= temp1)
						{
							if (addr == 118)
							{
								temp2 = (float)_nMemory[94] * LDevConst.OP20VRangeOut / 32767f;
								if ((temp1 - temp2) == 0)
								{
									temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
								}
								else
								{
									temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);			// Terrell_20160613 96-->97
								}

								if (temp <= 49)  //Terrell_20160613 48-->49
								{
									_bCMPA = Convert.ToByte(118 - addr + 0x19);
									_bCMPAHR = Convert.ToByte(0x80 - temp);
								}
								else
								{
									_bCMPA = Convert.ToByte(118 - addr + 0x18);
									_bCMPAHR = Convert.ToByte(0x80 + 97 - temp);						// Terrell_20160613 96-->97
								}
							}
							else
							{
								temp2 = -(float)_nMemory[addr - 1] * LDevConst.OP20VRangeOut / 32767f;
								if ((temp1 - temp2) == 0)
								{
									temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
								}
								else
								{
									temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);			// Terrell_20160613 96-->97
								}

								if (temp <= 49) //Terrell_20160613 48-->49
								{
									_bCMPA = Convert.ToByte(118 - addr + 0x19);
									_bCMPAHR = Convert.ToByte(0x80 - temp);
								}
								else
								{
									_bCMPA = Convert.ToByte(118 - addr + 0x18);
									_bCMPAHR = Convert.ToByte(0x80 + 97 - temp);						// Terrell_20160613 96-->97
								}
							}
							addr = 141;
						}
						else
						{
							addr = addr + 1;
						}
					}

					addr = 119;
					while (addr < 141)
					{
						temp1 = -(float)_nMemory[addr] * LDevConst.OP20VRangeOut / 32767f;
						if ( this._refVolt_CMP_ex >= temp1)
						{
							temp2 = -(float)_nMemory[addr - 1] * LDevConst.OP20VRangeOut / 32767f;
							if ((temp1 - temp2) == 0)
							{
								temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
							}
							else
							{
								temp = Convert.ToUInt16(( this._refVolt_CMP_ex - temp2) / (temp1 - temp2) * 97);						// Terrell_20160613 96-->97
							}

							if (temp < 49)  //Terrell_20160613 48-->49
							{
								_byCMP_ex_temp = (_bCMPA - (118 - addr + 0x19)) * 0x61 + _bCMPAHR - (0x80 - temp);
							}
							else
							{
								_byCMP_ex_temp = (_bCMPA - (118 - addr + 0x18)) * 0x61 + _bCMPAHR - (0x80 + 97 - temp);	// Terrell_20160613 96-->97
							}
							_byCMP_ex_temp = _byCMP_ex_temp * 32767 / _nMemory[343];
							_byCMP_ex[0] = Convert.ToByte(_byCMP_ex_temp & 0xFE);
							_byCMP_ex[1] = Convert.ToByte((_byCMP_ex_temp / 256) & 0x7F);
							addr = 141;
						}
						else
						{
							addr = addr + 1;
						}
					}
				}
				else
				{
					this._bHW_DevRange = Convert.ToByte(this._bHW_DevRange + 0xA0);

					addr = 212;						// 對 -200V Range 用 VF119 ~ VF141, addr = 212 ~ 234, 內插算出 CMPA 與 CMPAHR
					while (addr < 235)
					{
						temp1 = -(float)_nMemory[addr] * LDevConst.OP200VRangeOut / 32767f;
						if (V >= temp1)
						{
							if (addr == 212)
							{
								temp2 = (float)_nMemory[188] * LDevConst.OP200VRangeOut / 32767f;
								if ((temp1 - temp2) == 0)
								{
									temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
								}
								else
								{
									temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);			// Terrell_20160613 96-->97
								}

								if (temp <= 49)  //Terrell_20160613 48-->49
								{
									_bCMPA = Convert.ToByte(212 - addr + 0x19);
									_bCMPAHR = Convert.ToByte(0x80 - temp);
								}
								else
								{
									_bCMPA = Convert.ToByte(212 - addr + 0x18);
									_bCMPAHR = Convert.ToByte(0x80 + 97 - temp);					// Terrell_20160613 96-->97
								}
							}
							else
							{
								temp2 = -(float)_nMemory[addr - 1] * LDevConst.OP200VRangeOut / 32767f;
								temp = Convert.ToUInt16((V - temp2) / (temp1 - temp2) * 97);			// Terrell_20160613 96-->97
								if (temp <= 49)  //Terrell_20160613 48-->49
								{
									_bCMPA = Convert.ToByte(212 - addr + 0x19);
									_bCMPAHR = Convert.ToByte(0x80 - temp);
								}
								else
								{
									_bCMPA = Convert.ToByte(212 - addr + 0x18);
									_bCMPAHR = Convert.ToByte(0x80 + 97 - temp);					// Terrell_20160613 96-->97
								}
							}
							addr = 235;
						}
						else
						{
							addr = addr + 1;
							if (addr == 235)
							{
								_bCMPA = 2;
								_bCMPAHR = 0x80;
							}
						}
					}

					addr = 212;
					while (addr < 235)
					{
						temp1 = -(float)_nMemory[addr] * LDevConst.OP200VRangeOut / 32767f;
						if (this._refVolt_CMP_ex <= temp1)
						{
							if (addr == 212) temp2 = 0;
							else temp2 = -(float)_nMemory[addr - 1] * LDevConst.OP200VRangeOut / 32767f;  
							if ((temp1 - temp2) == 0)
							{
								temp = 1;				// Gilbert  這是很嚴重的錯誤 , 該變成甚麼 ?? temp = ???
							}
							else
							{
								temp = Convert.ToUInt16((this._refVolt_CMP_ex - temp2) / (temp1 - temp2) * 97);								// Terrell_20160613 96-->97
							}

							if (temp < 49)  //Terrell_20160613 48-->49
							{
								_byCMP_ex_temp = (_bCMPA - (212 - addr + 0x19)) * 0x61 + _bCMPAHR - (0x80 - temp);
							}
							else
							{
								_byCMP_ex_temp = (_bCMPA - (212 - addr + 0x18)) * 0x61 + _bCMPAHR - (0x80 + 97 - temp);		// Terrell_20160613 96-->97
							}
							_byCMP_ex_temp = _byCMP_ex_temp * 32767 / _nMemory[343];
							_byCMP_ex[0] = Convert.ToByte(_byCMP_ex_temp & 0xFE);
							_byCMP_ex[1] = Convert.ToByte((_byCMP_ex_temp / 256) & 0x7F);
							addr = 235;
						}
						else
						{
							addr = addr + 1;
						}
					}
				}
			}

			#endregion
		}

		private void calc_3ARange_IF(int CMP, UInt16 AD0)
		{
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[352] == 0xFFFF)				// Memory[352] = Close Loop Voffs offset @ +6V/+3A, 0xFFFF = 尚未執行
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[352] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[352] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[512])					// 0.0 ~ 0.1nA @ +6V/+3A range
				{
					temp1 = 0;								// 0A
					_voltOffset6V_1 = 0;
				}
				else if (temp <= _nMemory[513])				// 0.1nA ~ 3mA @ +6V/+3A range
				{
					temp1 = (float)(temp - _nMemory[512]) / (float)(_nMemory[513] - _nMemory[512]) * 0.003f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
				}
				else if (temp <= _nMemory[514])				// 3mA ~ 30mA @ +6V/+3A range
				{
					temp1 = (float)(temp - _nMemory[513]) / (float)(_nMemory[514] - _nMemory[513]) * 0.027f + 0.003f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
				}
				else if (temp <= _nMemory[515])				// 30mA ~ 300mA @ +6V/+3A range
				{
					temp1 = (float)(temp - _nMemory[514]) / (float)(_nMemory[515] - _nMemory[514]) * 0.27f + 0.03f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
				}
				else if (temp <= _nMemory[516])				// 300mA ~1.5A @ +6V/+3A range
				{
					temp1 = (float)(temp - _nMemory[515]) / (float)(_nMemory[516] - _nMemory[515]) * 1.2f + 0.3f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
				}
				else if (temp <= _nMemory[517])				// 1.5A ~ 3A @ +6V/+3A range
				{
					temp1 = (float)(temp - _nMemory[516]) / (float)(_nMemory[517] - _nMemory[516]) * 1.5f + 1.5f;

					if (temp1 <= 2) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 2f) + _voltOffset6V_1;
				}
				else if (temp >= _nMemory[517])				// 3A ~ Max @ +6V/+3A range
				{
					temp1 = (float)temp / (float)_nMemory[517] * 3.0f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 3;
				}

				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "A";
			}
			else
			{
				if (_nMemory[361] == 0xFFFF)				//  Memory[361] = Close Loop Voffs offset @ -6V/-3A, 0xFFFF = 尚未執行
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[361] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[361] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[520])					// 0.0 ~ 0.1nA @ -6V/-3A range
				{
					temp1 = 0;								// 0A
					_voltOffset6V_1 = 0;
				}
				else if (temp <= _nMemory[521])				// -0.1nA ~ -3mA @ -6V/-3A range
				{
					temp1 = (float)(temp - _nMemory[520]) / (float)(_nMemory[521] - _nMemory[520]) * 0.003f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
				}
				else if (temp <= _nMemory[522])				//-3mA ~ -30mA @ -6V/-3A range
				{
					temp1 = (float)(temp - _nMemory[521]) / (float)(_nMemory[522] - _nMemory[521]) * 0.027f + 0.003f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
				}
				else if (temp <= _nMemory[523])				// -30mA ~ -300mA @ -6/V-3A range
				{
					temp1 = (float)(temp - _nMemory[522]) / (float)(_nMemory[523] - _nMemory[522]) * 0.27f + 0.03f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
				}
				else if (temp <= _nMemory[524])				// -300mA ~ -1.5A @ -6V/-3A range
				{
					temp1 = (float)(temp - _nMemory[523]) / (float)(_nMemory[524] - _nMemory[523]) * 1.2f + 0.3f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
				}
				else if (temp <= _nMemory[525])				// -1.5A ~ -3A @ -6V/-3A range
				{
					temp1 = (float)(temp - _nMemory[524]) / (float)(_nMemory[525] - _nMemory[524]) * 1.5f + 1.5f;

					if (temp1 <= 2) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 2;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 2f) + _voltOffset6V_1;
				}
				else if (temp >= _nMemory[525])				// -3A ~ Max @ -6V/-3A range
				{
					temp1 = (float)temp / (float)_nMemory[525] * 3.0f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 3;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "A";
			}
		}

		private void calc_2ARange_IF(int CMP, UInt16 AD0)
		{
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[351] == 0xFFFF)				// Memory[351] = Close Loop Voffs offset @ +6V/+2A, 0xFFFF = 尚未執行
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[351] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[351] & 0xFF00) / 256 - 127;
				}


				if (temp <= _nMemory[496])					// 0.0 ~ 0.1nA@ +6V/+2A range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
				}
				else if (temp <= _nMemory[497])				// 0.1nA ~ 2mA @ +6V/+2A range
				{
					temp1 = (float)(temp - _nMemory[496]) / (float)(_nMemory[497] - _nMemory[496]) * 0.002f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
				}
				else if (temp <= _nMemory[498])				// 2mA ~ 20mA @ +6V/+2A range
				{
					temp1 = (float)(temp - _nMemory[497]) / (float)(_nMemory[498] - _nMemory[497]) * 0.018f + 0.002f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
				}
				else if (temp <= _nMemory[499])				// 20mA ~ 200mA @ +6V/+2A range
				{
					temp1 = (float)(temp - _nMemory[498]) / (float)(_nMemory[499] - _nMemory[498]) * 0.18f + 0.02f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
				}
				else if (temp <= _nMemory[500])				// 200mA ~ 1A @ +6V/+2A range
				{
					temp1 = (float)(temp - _nMemory[499]) / (float)(_nMemory[500] - _nMemory[499]) * 0.8f + 0.2f;

					if (temp1 <= 0.8) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 0.8f) / 1.2f + _voltOffset6V_1;
				}
				else if (temp <= _nMemory[501])				// 1A ~ 2A @ +6V/+2A range
				{
					temp1 = (float)(temp - _nMemory[500]) / (float)(_nMemory[501] - _nMemory[500]) * 1.0f + 1.0f;

					if (temp1 <= 0.8) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 0.8f) / 1.2f + _voltOffset6V_1;
				}
				else if (temp >= _nMemory[501])				// 2A ~ Max @ +6V/+2A range
				{
					temp1 = (float)temp / (float)_nMemory[501] * 2.0f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 2f;
				}

				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "A";
			}
			else
			{
				if (_nMemory[360] == 0xFFFF)				// Memory[360] = Close Loop Voffs offset @ -6V/-2A, 0xFFFF = 尚未執行
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[360] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[360] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[504])					// 0.0 ~ 0.1nA@ -6V/-2A range
				{
					temp1 = 0;								// 0A
					_voltOffset6V_1 = 0;
				}
				else if (temp <= _nMemory[505])				// -0.1nA ~ -2mA @ -6V/-2A range
				{
					temp1 = (float)(temp - _nMemory[504]) / (float)(_nMemory[505] - _nMemory[504]) * 0.002f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
				}
				else if (temp <= _nMemory[506])				// -2mA ~ -20mA @ -6V/-2A range
				{
					temp1 = (float)(temp - _nMemory[505]) / (float)(_nMemory[506] - _nMemory[505]) * 0.018f + 0.002f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
				}
				else if (temp <= _nMemory[507])				// -20mA ~ -200mA @ -6V/-2A range
				{
					temp1 = (float)(temp - _nMemory[506]) / (float)(_nMemory[507] - _nMemory[506]) * 0.18f + 0.02f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
				}
				else if (temp <= _nMemory[508])				// -200mA ~ -1A@ -6V/-2A range
				{
					temp1 = (float)(temp - _nMemory[507]) / (float)(_nMemory[508] - _nMemory[507]) * 0.8f + 0.2f;

					if (temp1 <= 0.8) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 0.8f) / 1.2f + _voltOffset6V_1;
				}
				else if (temp <= _nMemory[509])				// -1A ~ -2A @ -6V/-2A range
				{
					temp1 = (float)(temp - _nMemory[508]) / (float)(_nMemory[509] - _nMemory[508]) * 1.0f + 1.0f;

					if (temp1 <= 0.8) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.8f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 0.8f) / 1.2f + _voltOffset6V_1;
				}
				else if (temp >= _nMemory[509])				// -2A ~ Max @ -6V/-2A range
				{
					temp1 = (float)temp / (float)_nMemory[509] * 2.0f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 2f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "A";
			}
		}

		private void calc_800mARange_IF(int CMP, UInt16 AD0)
		{
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[350] == 0xFFFF)					// Memory[350] = Close Loop Voffs offset @ +6V/+800mA, 0xFFFF = 尚未執行
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[350] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[350] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[368] == 0xFFFF)					// Memory[368] = Close Loop Voffs offset @ +20V/+800mA, 0xFFFF = 尚未執行		
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[368] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[368] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[480])						// 0.0 ~ 0.1nA @ +6V/+800mA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
				}
				else if (temp <= _nMemory[481])					// 0.1nA ~ 0.8mA @ +6V/+800mA range
				{
					temp1 = (float)(temp - _nMemory[480]) / (float)(_nMemory[481] - _nMemory[480]) * 0.8f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
				}
				else if (temp <= _nMemory[482])					// 0.8mA ~ 8mA @ +6V/+800mA range
				{
					temp1 = (float)(temp - _nMemory[481]) / (float)(_nMemory[482] - _nMemory[481]) * 7.2f + 0.8f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
				}
				else if (temp <= _nMemory[483])					// 8mA ~ 80mA @ +6V/+800mA range
				{
					temp1 = (float)(temp - _nMemory[482]) / (float)(_nMemory[483] - _nMemory[482]) * 72f + 8f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
				}
				else if (temp <= _nMemory[484])					// 80mA ~ 400mA @ +6V/+800mA range
				{
					temp1 = (float)(temp - _nMemory[483]) / (float)(_nMemory[484] - _nMemory[483]) * 320f + 80f;

					if (temp1 <= 100) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 100f) / 700f + _voltOffset6V_1;

					if (temp1 <= 100) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 100f) / 700f + _voltOffset20V_1;
				}
				else if (temp <= _nMemory[485])					// 400mA ~ 800mA @ +6V/+800mA range
				{
					temp1 = (float)(temp - _nMemory[484]) / (float)(_nMemory[485] - _nMemory[484]) * 400f + 400f;

					if (temp1 <= 100) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 100f) / 700f + _voltOffset6V_1;

					if (temp1 <= 100) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 100f) / 700f + _voltOffset20V_1;
				}
				else if (temp >= _nMemory[485])					// 800mA ~ Max @ +6V/+800mA range
				{
					temp1 = (float)temp / (float)_nMemory[485] * 800f;

					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 800f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 800f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "mA";
			}
			else
			{
				if (_nMemory[359] == 0xFFFF)					// Memory[359] = Close Loop Voffs offset @ -6V/-800mA, 0xFFFF = 尚未執行
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[359] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[359] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[375] == 0xFFFF)					// Memory[375] = Close Loop Voffs offset @ -20V/-800mA, 0xFFFF = 尚未執行
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[375] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[375] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[488])						// 0.0 ~ 0.1nA @ -6V/-800mA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
				}
				else if (temp <= _nMemory[489])					// 0.1nA ~ -0.8mA @ -6V/-800mA range
				{
					temp1 = (float)(temp - _nMemory[488]) / (float)(_nMemory[489] - _nMemory[488]) * 0.8f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
				}
				else if (temp <= _nMemory[490])					// -0.8mA ~ -8mA @ -6V/-800mA range
				{
					temp1 = (float)(temp - _nMemory[489]) / (float)(_nMemory[490] - _nMemory[489]) * 7.2f + 0.8f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
				}
				else if (temp <= _nMemory[491])					// -8mA ~ -80mA @ -6V/-800mA range
				{
					temp1 = (float)(temp - _nMemory[490]) / (float)(_nMemory[491] - _nMemory[490]) * 72f + 8f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
				}
				else if (temp <= _nMemory[492])					// -80mA ~ -400mA @ -6V/-800mA range
				{
					temp1 = (float)(temp - _nMemory[491]) / (float)(_nMemory[492] - _nMemory[491]) * 320f + 80f;

					if (temp1 <= 100) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 100f) / 700f + _voltOffset6V_1;

					if (temp1 <= 100) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 100f) / 700f + _voltOffset20V_1;
				}
				else if (temp <= _nMemory[493])					// -400mA ~ -800mA @ -6V/-800mA range
				{
					temp1 = (float)(temp - _nMemory[492]) / (float)(_nMemory[493] - _nMemory[492]) * 400f + 400f;

					if (temp1 <= 100) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 100f) / 700f + _voltOffset6V_1;

					if (temp1 <= 100) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 100f) / 700f + _voltOffset20V_1;
				}
				else if (temp >= _nMemory[493])					// -800mA ~ Max @ -6V/-800mA range
				{
					temp1 = (float)temp / (float)_nMemory[493] * 800f;

					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 800f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 800f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "mA";
			}
		}

		private void calc_100mARange_IF(int CMP, UInt16 AD0)
		{
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[349] == 0xFFFF)				// Memory[349] = Close Loop Voffs offset @ +6V/+100mA, 0xFFFF = 尚未執行
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[349] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[349] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[367] == 0xFFFF)				// Memory[367] = Close Loop Voffs offset @ +20V/+100mA, 0xFFFF = 尚未執行
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[367] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[367] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[301] == 0xFFFF)				// Memory[301] = Close Loop Voffs offset @ +200V/+100mA, 0xFFFF = 尚未執行
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[301] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[301] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[464])					// 0.0 ~ 0.1nA @ +6V/+100mA range
				{
					temp1 = 0;								// 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[465])				// 0.1nA ~ 0.1mA @ +6V/+100mA range
				{
					temp1 = (float)(temp - _nMemory[464]) / (float)(_nMemory[465] - _nMemory[464]) * 0.1f;

					if (_nMemory[348] == 0xFFFF)			// Memory[348] = Close Loop Voffs offset @ +6V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[348] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[348] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;

					if (_nMemory[366] == 0xFFFF)			// Memory[366] = Close Loop Voffs offset @ +20V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[366] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[366] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;

					if (_nMemory[300] == 0xFFFF)			// Memory[300] = Close Loop Voffs offset @ +200V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[300] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[300] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[466])				// 0.1mA ~ 1mA @ +6V/+100mA range
				{
					temp1 = (float)(temp - _nMemory[465]) / (float)(_nMemory[466] - _nMemory[465]) * 0.9f + 0.1f;

					if (_nMemory[348] == 0xFFFF)			// Memory[348] = Close Loop Voffs offset @ +6V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[348] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[348] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;

					if (_nMemory[366] == 0xFFFF)			// Memory[366] = Close Loop Voffs offset @ +20V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[366] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[366] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;

					if (_nMemory[300] == 0xFFFF)			// Memory[300] = Close Loop Voffs offset @ +200V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[300] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[300] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[467])				// 1mA ~ 10mA @ +6V/+100mA range
				{
					temp1 = (float)(temp - _nMemory[466]) / (float)(_nMemory[467] - _nMemory[466]) * 9f + 1.0f;

					if (_nMemory[348] == 0xFFFF)			// Memory[348] = Close Loop Voffs offset @ +6V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[348] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[348] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;

					if (_nMemory[366] == 0xFFFF)			// Memory[366] = Close Loop Voffs offset @ +20V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[366] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[366] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;

					if (_nMemory[300] == 0xFFFF)			// Memory[300] = Close Loop Voffs offset @ +200V/+10mA, 0xFFFF = 尚未執行
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[300] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[300] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[468])				// 10mA ~ 50mA @ +6V/+100mA range
				{
					temp1 = (float)(temp - _nMemory[467]) / (float)(_nMemory[468] - _nMemory[467]) * 40f + 10f;

					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[469])				// 50mA ~ 100mA @ +6V/+100mA range
				{
					temp1 = (float)(temp - _nMemory[468]) / (float)(_nMemory[469] - _nMemory[468]) * 50f + 50f;

					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[469])				// 100mA ~ Max @ +6V/+100mA range
				{
					temp1 = (float)temp / (float)_nMemory[469] * 100f;

					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 100f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 100f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "mA";
			}
			else
			{
				if (_nMemory[358] == 0xFFFF)				// Memory[358] = Close Loop Voffs offset @ -6V/-100mA, 0xFFFF = 尚未執行
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[358] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[358] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[374] == 0xFFFF)				// Memory[374] = Close Loop Voffs offset @ -20V/-100mA, 0xFFFF = 尚未執行
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[374] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[374] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[308] == 0xFFFF)				// Memory[308] = Close Loop Voffs offset @ -200V/-100mA, 0xFFFF = 尚未執行
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[308] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[308] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[472])					// 0.0 ~ -0.1nA @ -6V/-100mA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[473])				// -0.1nA ~ -0.1mA @ -6V/-100mA range
				{
					temp1 = (float)(temp - _nMemory[472]) / (float)(_nMemory[473] - _nMemory[472]) * 0.1f;

					if (_nMemory[357] == 0xFFFF)			// Memory[357] = Close Loop Voffs offset @ -6V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[357] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[357] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;

					if (_nMemory[373] == 0xFFFF)			// Memory[373] = Close Loop Voffs offset @ -20V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[373] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[373] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;

					if (_nMemory[307] == 0xFFFF)			// Memory[307] = Close Loop Voffs offset @ -200V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[307] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[307] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[474])				// -0.1mA ~ -1mA @ -6V/-100mA range
				{
					temp1 = (float)(temp - _nMemory[473]) / (float)(_nMemory[474] - _nMemory[473]) * 0.9f + 0.1f;

					if (_nMemory[357] == 0xFFFF)			// Memory[357] = Close Loop Voffs offset @ -6V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[357] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[357] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;

					if (_nMemory[373] == 0xFFFF)			// Memory[373] = Close Loop Voffs offset @ -20V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[373] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[373] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;

					if (_nMemory[307] == 0xFFFF)			// Memory[307] = Close Loop Voffs offset @ -200V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[307] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[307] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[475])				// -1mA ~ -10mA @ -6V/-100mA range
				{
					temp1 = (float)(temp - _nMemory[474]) / (float)(_nMemory[475] - _nMemory[474]) * 9f + 1.0f;

					if (_nMemory[357] == 0xFFFF)			// Memory[357] = Close Loop Voffs offset @ -6V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[357] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[357] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;

					if (_nMemory[373] == 0xFFFF)			// Memory[373] = Close Loop Voffs offset @ -20V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[373] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[373] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;

					if (_nMemory[307] == 0xFFFF)			// Memory[307] = Close Loop Voffs offset @ -200V/-10mA, 0xFFFF = 尚未執行
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[307] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[307] & 0xFF00) / 256 - 127;
					}

					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[476])				// -10mA ~ -50mA @ -6V/-100mA range
				{
					temp1 = (float)(temp - _nMemory[475]) / (float)(_nMemory[476] - _nMemory[475]) * 40f + 10f;

					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[477])				// -50mA ~ -100mA @ -6V/-100mA range
				{
					temp1 = (float)(temp - _nMemory[476]) / (float)(_nMemory[477] - _nMemory[476]) * 50f + 50f;

					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[477])				// -100mA ~ Max @ -6V/-100mA range
				{
					temp1 = (float)temp / (float)_nMemory[477] * 100f;

					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 100f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 100f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "mA";
			}
		}

		private void calc_10mARange_IF(int CMP, UInt16 AD0)
		{
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[348] == 0xFFFF) // offset @ 10mA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[348] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[348] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[366] == 0xFFFF) // offset @ 10mA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[366] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[366] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[300] == 0xFFFF) // offset @ 10mA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[300] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[300] & 0xFF00) / 256 - 127;
				}
				if (temp <= _nMemory[448]) // 0A@10mA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[449]) // 0~0.01mA@10mA range
				{
					temp1 = (float)(temp - _nMemory[448]) / (float)(_nMemory[449] - _nMemory[448]) * 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[450]) // 0.01m~0.1mA@10mA range
				{
					temp1 = (float)(temp - _nMemory[449]) / (float)(_nMemory[450] - _nMemory[449]) * 0.09f + 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[451]) // 0.1m~1mA@10mA range
				{
					temp1 = (float)(temp - _nMemory[450]) / (float)(_nMemory[451] - _nMemory[450]) * 0.9f + 0.1f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[452]) // 1m~5mA@10mA range
				{
					temp1 = (float)(temp - _nMemory[451]) / (float)(_nMemory[452] - _nMemory[451]) * 4f + 1f;
					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[453]) // 5m~10mA@10mA range
				{
					temp1 = (float)(temp - _nMemory[452]) / (float)(_nMemory[453] - _nMemory[452]) * 5f + 5f;
					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[453]) // 10mA@10mA range
				{
					temp1 = (float)temp / (float)_nMemory[453] * 10f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 10f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "mA";
			}
			else
			{
				if (_nMemory[357] == 0xFFFF) // offset @ -10mA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[357] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[357] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[373] == 0xFFFF) // offset @ -10mA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[373] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[373] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[307] == 0xFFFF) // offset @ -10mA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[307] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[307] & 0xFF00) / 256 - 127;
				}
				if (temp <= _nMemory[456]) // 0A@-10mA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[457]) // 0~-0.01mA@-10mA range
				{
					temp1 = (float)(temp - _nMemory[456]) / (float)(_nMemory[457] - _nMemory[456]) * 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[458]) // -0.01m~-0.1mA@-10mA range
				{
					temp1 = (float)(temp - _nMemory[457]) / (float)(_nMemory[458] - _nMemory[457]) * 0.09f + 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[459]) // -0.1m~-1mA@-10mA range
				{
					temp1 = (float)(temp - _nMemory[458]) / (float)(_nMemory[459] - _nMemory[458]) * 0.9f + 0.1f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[460]) // -1m~-5mA@-10mA range
				{
					temp1 = (float)(temp - _nMemory[459]) / (float)(_nMemory[460] - _nMemory[459]) * 4f + 1f;
					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[461]) // -5m~-10mA@-10mA range
				{
					temp1 = (float)(temp - _nMemory[460]) / (float)(_nMemory[461] - _nMemory[460]) * 5f + 5f;
					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[461]) // -10mA@-10mA range
				{
					temp1 = (float)temp / (float)_nMemory[461] * 10f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 10f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "mA";
			}
		}

		private void calc_1mARange_IF(int CMP, UInt16 AD0)
		{
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[347] == 0xFFFF) // offset @ 1mA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[347] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[347] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[365] == 0xFFFF) // offset @ 1mA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[365] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[365] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[299] == 0xFFFF) // offset @ 1mA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[299] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[299] & 0xFF00) / 256 - 127;
				}

				//---------------------------------------------------------------------------------
				if (temp <= _nMemory[432]) // 0A@1mA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[433]) // 0~1uA@1mA range
				{
					temp1 = (float)(temp - _nMemory[432]) / (float)(_nMemory[433] - _nMemory[432]) * 1.0f;
					if (_nMemory[346] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[346] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[346] & 0xFF00) / 256 - 127;
					}
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					if (_nMemory[364] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[364] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[364] & 0xFF00) / 256 - 127;
					}
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					if (_nMemory[298] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[298] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[298] & 0xFF00) / 256 - 127;
					}
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
				}
				else if (temp <= _nMemory[434]) // 1u~10uA@1mA range
				{
					temp1 = (float)(temp - _nMemory[433]) / (float)(_nMemory[434] - _nMemory[433]) * 9f + 1.0f;
					if (_nMemory[346] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[346] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[346] & 0xFF00) / 256 - 127;
					}
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					if (_nMemory[364] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[364] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[364] & 0xFF00) / 256 - 127;
					}
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					if (_nMemory[298] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[298] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[298] & 0xFF00) / 256 - 127;
					}
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
				}
				else if (temp <= _nMemory[435]) // 10u~100uA@1mA range
				{
					temp1 = (float)(temp - _nMemory[434]) / (float)(_nMemory[435] - _nMemory[434]) * 90f + 10f;
					if (_nMemory[346] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[346] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[346] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (_nMemory[364] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[364] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[364] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (_nMemory[298] == 0xFFFF) // offset @ 100uA
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[298] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[298] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[436]) // 100u~500uA@1mA range
				{
					temp1 = (float)(temp - _nMemory[435]) / (float)(_nMemory[436] - _nMemory[435]) * 400f + 100f;
					if (temp1 <= 100) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 100f) / 900f + _voltOffset6V_1;
					if (temp1 <= 100) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 100f) / 900f + _voltOffset20V_1;
					if (temp1 <= 100) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 100f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 100f) / 900f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[437]) // 500u~1mA@1mA range
				{
					temp1 = (float)(temp - _nMemory[436]) / (float)(_nMemory[437] - _nMemory[436]) * 500f + 500f;
					if (temp1 <= 100) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 100f) / 900f + _voltOffset6V_1;
					if (temp1 <= 100) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 100f) / 900f + _voltOffset20V_1;
					if (temp1 <= 100) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 100f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 100f) / 900f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[437]) // 1mA@1mA range
				{
					temp1 = (float)temp / (float)_nMemory[437] * 1000f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 1000f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 1000f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 1000f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "uA";
			}
			else
			{
				if (_nMemory[356] == 0xFFFF) // offset @ -1mA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[356] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[356] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[372] == 0xFFFF) // offset @ -1mA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[372] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[372] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[306] == 0xFFFF) // offset @ -1mA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[306] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[306] & 0xFF00) / 256 - 127;
				}
				if (temp <= _nMemory[440]) // 0A@-1mA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[441]) // 0~-1uA@-1mA range
				{
					temp1 = (float)(temp - _nMemory[440]) / (float)(_nMemory[441] - _nMemory[440]) * 1.0f;
					if (_nMemory[355] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[355] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[355] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (_nMemory[371] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[371] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[371] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (_nMemory[305] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[305] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[305] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[442]) // -1u~-10uA@-1mA range
				{
					temp1 = (float)(temp - _nMemory[441]) / (float)(_nMemory[442] - _nMemory[441]) * 9f + 1.0f;
					if (_nMemory[355] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[355] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[355] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (_nMemory[371] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[371] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[371] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (_nMemory[305] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[305] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[305] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[443]) // -10u~-100uA@-1mA range
				{
					temp1 = (float)(temp - _nMemory[442]) / (float)(_nMemory[443] - _nMemory[442]) * 90f + 10f;
					if (_nMemory[355] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset6V_1 = 0;
						_voltOffset6V_2 = 0;
					}
					else
					{
						_voltOffset6V_1 = ((UInt16)_nMemory[355] & 0xFF) - 127;
						_voltOffset6V_2 = ((UInt16)_nMemory[355] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (_nMemory[371] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset20V_1 = 0;
						_voltOffset20V_2 = 0;
					}
					else
					{
						_voltOffset20V_1 = ((UInt16)_nMemory[371] & 0xFF) - 127;
						_voltOffset20V_2 = ((UInt16)_nMemory[371] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (_nMemory[305] == 0xFFFF) // offset @ -100uA
					{
						_voltOffset200V_1 = 0;
						_voltOffset200V_2 = 0;
					}
					else
					{
						_voltOffset200V_1 = ((UInt16)_nMemory[305] & 0xFF) - 127;
						_voltOffset200V_2 = ((UInt16)_nMemory[305] & 0xFF00) / 256 - 127;
					}
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[444]) // -100u~-500uA@-1mA range
				{
					temp1 = (float)(temp - _nMemory[443]) / (float)(_nMemory[444] - _nMemory[443]) * 400f + 100f;
					if (temp1 <= 100) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 100f) / 900f + _voltOffset6V_1;
					if (temp1 <= 100) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 100f) / 900f + _voltOffset20V_1;
					if (temp1 <= 100) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 100f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 100f) / 900f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[445]) // -500u~-1mA@-1mA range
				{
					temp1 = (float)(temp - _nMemory[444]) / (float)(_nMemory[445] - _nMemory[444]) * 500f + 500f;
					if (temp1 <= 100) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 100f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 100f) / 900f + _voltOffset6V_1;
					if (temp1 <= 100) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 100f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 100f) / 900f + _voltOffset20V_1;
					if (temp1 <= 100) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 100f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 100f) / 900f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[445]) // -1mA@-1mA range
				{
					temp1 = (float)temp / (float)_nMemory[445] * 1000f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 1000f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 1000f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 1000f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "uA";
			}
		}

		private void calc_100uARange_IF(int CMP, UInt16 AD0)
		{ 
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[346] == 0xFFFF) // offset @ 100uA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[346] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[346] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[364] == 0xFFFF) // offset @ 100uA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[364] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[364] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[298] == 0xFFFF) // offset @ 100uA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[298] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[298] & 0xFF00) / 256 - 127;
				}
				if (temp <= _nMemory[416]) // 0A@100uA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[417]) // 0~0.1uA@100uA range
				{
					temp1 = (float)(temp - _nMemory[416]) / (float)(_nMemory[417] - _nMemory[416]) * 0.1f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
				}
				else if (temp <= _nMemory[418]) // 0.1u~1uA@100uA range
				{
					temp1 = (float)(temp - _nMemory[417]) / (float)(_nMemory[418] - _nMemory[417]) * 0.9f + 0.1f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
				}
				else if (temp <= _nMemory[419]) // 1u~10uA@100uA range
				{
					temp1 = (float)(temp - _nMemory[418]) / (float)(_nMemory[419] - _nMemory[418]) * 9f + 1.0f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
				}
				else if (temp <= _nMemory[420]) // 10u~50uA@100uA range
				{
					temp1 = (float)(temp - _nMemory[419]) / (float)(_nMemory[420] - _nMemory[419]) * 40f + 10f;
					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[421]) // 50u~100uA@100uA range
				{
					temp1 = (float)(temp - _nMemory[420]) / (float)(_nMemory[421] - _nMemory[420]) * 50f + 50f;
					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[421]) // 100uA@100uA range
				{
					temp1 = (float)temp / (float)_nMemory[421] * 100f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 100f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 100f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "uA";
			}
			else
			{
				if (_nMemory[355] == 0xFFFF) // offset @ -100uA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[355] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[355] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[371] == 0xFFFF) // offset @ -100uA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[371] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[371] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[305] == 0xFFFF) // offset @ -100uA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[305] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[305] & 0xFF00) / 256 - 127;
				}
				if (temp <= _nMemory[424]) // 0A@-100uA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[425]) // 0~-0.1uA@-100uA range
				{
					temp1 = (float)(temp - _nMemory[424]) / (float)(_nMemory[425] - _nMemory[424]) * 0.1f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
				}
				else if (temp <= _nMemory[426]) // -0.1u~-1uA@-100uA range
				{
					temp1 = (float)(temp - _nMemory[425]) / (float)(_nMemory[426] - _nMemory[425]) * 0.9f + 0.1f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
				}
				else if (temp <= _nMemory[427]) // -1u~-10uA@-100uA range
				{
					temp1 = (float)(temp - _nMemory[426]) / (float)(_nMemory[427] - _nMemory[426]) * 9f + 1.0f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
				}
				else if (temp <= _nMemory[428]) // -10u~-50uA@-100uA range
				{
					temp1 = (float)(temp - _nMemory[427]) / (float)(_nMemory[428] - _nMemory[427]) * 40f + 10f;
					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[429]) // -50u~-100uA@-100uA range
				{
					temp1 = (float)(temp - _nMemory[428]) / (float)(_nMemory[429] - _nMemory[428]) * 50f + 50f;
					if (temp1 <= 10) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 10f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 10f) / 90f + _voltOffset6V_1;
					if (temp1 <= 10) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 10f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 10f) / 90f + _voltOffset20V_1;
					if (temp1 <= 10) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 10f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 10f) / 90f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[429]) // -100uA@-100uA range
				{
					temp1 = (float)temp / (float)_nMemory[429] * 100f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 100f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 100f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 100f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "uA";
			}

		}

	    private void calc_10uARange_IF(int CMP, UInt16 AD0)
		{
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[345] == 0xFFFF) // offset @ 10uA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[345] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[345] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[363] == 0xFFFF) // offset @ 10uA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[363] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[363] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[297] == 0xFFFF) // offset @ 10uA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[297] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[297] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[400]) // 0A@10uA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[401]) // 0~0.01uA@10uA range
				{
					temp1 = (float)(temp - _nMemory[400]) / (float)(_nMemory[401] - _nMemory[400]) * 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[402]) // 0.01u~0.1uA@10uA range
				{
					temp1 = (float)(temp - _nMemory[401]) / (float)(_nMemory[402] - _nMemory[401]) * 0.09f + 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[403]) // 0.1u~1uA@10uA range
				{
					temp1 = (float)(temp - _nMemory[402]) / (float)(_nMemory[403] - _nMemory[402]) * 0.9f + 0.1f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[404]) // 1u~5uA@10uA range
				{
					temp1 = (float)(temp - _nMemory[403]) / (float)(_nMemory[404] - _nMemory[403]) * 4f + 1.0f;

					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[405]) // 5u~10uA@10uA range
				{
					temp1 = (float)(temp - _nMemory[404]) / (float)(_nMemory[405] - _nMemory[404]) * 5f + 5f;

					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[405]) // 10uA@10uA range
				{
					temp1 = (float)temp / (float)_nMemory[405] * 10f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 10f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "uA";
			}
			else
			{
				if (_nMemory[354] == 0xFFFF) // offset @ -10uA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[354] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[354] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[370] == 0xFFFF) // offset @ -10uA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[370] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[370] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[304] == 0xFFFF) // offset @ -10uA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[304] & 0xFF) - 127;				// Gilbert, Addr 錯了，改回 Index = 304
					_voltOffset200V_2 = ((UInt16)_nMemory[304] & 0xFF00) / 256 - 127;		// Gilbert, Addr 錯了，改回 Index = 304
				}

				if (temp <= _nMemory[408]) // 0A@-10uA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[409]) // 0~-0.01uA@-10uA range
				{
					temp1 = (float)(temp - _nMemory[408]) / (float)(_nMemory[409] - _nMemory[408]) * 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[410]) // -0.01u~-0.1uA@-10uA range
				{
					temp1 = (float)(temp - _nMemory[409]) / (float)(_nMemory[410] - _nMemory[409]) * 0.09f + 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[411]) // -0.1u~-1uA@-10uA range
				{
					temp1 = (float)(temp - _nMemory[410]) / (float)(_nMemory[411] - _nMemory[410]) * 0.9f + 0.1f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1;
				}
				else if (temp <= _nMemory[412]) // -1u~-5uA@-10uA range
				{
					temp1 = (float)(temp - _nMemory[411]) / (float)(_nMemory[412] - _nMemory[411]) * 4f + 1.0f;
					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[413]) // -5u~-10uA@-10uA range
				{
					temp1 = (float)(temp - _nMemory[412]) / (float)(_nMemory[413] - _nMemory[412]) * 5f + 5f;
					if (temp1 <= 1) _voltOffset6V_1 = _voltOffset6V_1 * temp1;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 1f) / 9f + _voltOffset6V_1;
					if (temp1 <= 1) _voltOffset20V_1 = _voltOffset20V_1 * temp1;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 1f) / 9f + _voltOffset20V_1;
					if (temp1 <= 1) _voltOffset200V_1 = _voltOffset200V_1 * temp1;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 1f) / 9f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[413]) // -10uA@-10uA range
				{
					temp1 = (float)temp / (float)_nMemory[413] * 10f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1 / 10f;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1 / 10f;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1 / 10f;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "uA";
			}
		}

		private void calc_1uARange_IF(int CMP, UInt16 AD0)
		{
			float temp1 = 0;
			UInt16 temp = AD0;

			if (CMP >= 0x1980)
			{
				if (_nMemory[344] == 0xFFFF) // offset @ 1uA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[344] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[344] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[362] == 0xFFFF) // offset @ 1uA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[362] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[362] & 0xFF00) / 256 - 127;
				}

				if (_nMemory[296] == 0xFFFF) // offset @ 1uA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[296] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[296] & 0xFF00) / 256 - 127;
				}

				if (temp <= _nMemory[384]) // 0A@1uA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[385]) // 0~0.001uA@1uA range
				{
					temp1 = (float)(temp - _nMemory[384]) / (float)(_nMemory[385] - _nMemory[384]) * 0.001f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
				}
				else if (temp <= _nMemory[386]) // 0.001u~0.01uA@1uA range
				{
					temp1 = (float)(temp - _nMemory[385]) / (float)(_nMemory[386] - _nMemory[385]) * 0.009f + 0.001f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
				}
				else if (temp <= _nMemory[387]) // 0.01u~0.1uA@1uA range
				{
					temp1 = (float)(temp - _nMemory[386]) / (float)(_nMemory[387] - _nMemory[386]) * 0.09f + 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
				}
				else if (temp <= _nMemory[388]) // 0.1u~0.5uA@1uA range
				{
					temp1 = (float)(temp - _nMemory[387]) / (float)(_nMemory[388] - _nMemory[387]) * 0.4f + 0.1f;
					if (temp1 <= 0.1) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset6V_1;
					if (temp1 <= 0.1) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset20V_1;
					if (temp1 <= 0.1) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[389]) // 0.5u~1uA@1uA range
				{
					temp1 = (float)(temp - _nMemory[388]) / (float)(_nMemory[389] - _nMemory[388]) * 0.5f + 0.5f;
					if (temp1 <= 0.1) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset6V_1;
					if (temp1 <= 0.1) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset20V_1;
					if (temp1 <= 0.1) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[389]) // 1uA@1uA range
				{
					temp1 = (float)temp / (float)_nMemory[389] * 1.0f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "uA";
			}
			else
			{
				if (_nMemory[353] == 0xFFFF) // offset @ -1uA
				{
					_voltOffset6V_1 = 0;
					_voltOffset6V_2 = 0;
				}
				else
				{
					_voltOffset6V_1 = ((UInt16)_nMemory[353] & 0xFF) - 127;
					_voltOffset6V_2 = ((UInt16)_nMemory[353] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[369] == 0xFFFF) // offset @ -1uA
				{
					_voltOffset20V_1 = 0;
					_voltOffset20V_2 = 0;
				}
				else
				{
					_voltOffset20V_1 = ((UInt16)_nMemory[369] & 0xFF) - 127;
					_voltOffset20V_2 = ((UInt16)_nMemory[369] & 0xFF00) / 256 - 127;
				}
				if (_nMemory[303] == 0xFFFF) // offset @ -1uA
				{
					_voltOffset200V_1 = 0;
					_voltOffset200V_2 = 0;
				}
				else
				{
					_voltOffset200V_1 = ((UInt16)_nMemory[303] & 0xFF) - 127;
					_voltOffset200V_2 = ((UInt16)_nMemory[303] & 0xFF00) / 256 - 127;
				}
				if (temp <= _nMemory[392]) // 0A@-1uA range
				{
					temp1 = 0; // 0A
					_voltOffset6V_1 = 0;
					_voltOffset20V_1 = 0;
					_voltOffset200V_1 = 0;
				}
				else if (temp <= _nMemory[393]) // 0~-0.001uA@-1uA range
				{
					temp1 = (float)(temp - _nMemory[392]) / (float)(_nMemory[393] - _nMemory[392]) * 0.001f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
				}
				else if (temp <= _nMemory[394]) // -0.001u~-0.01uA@-1uA range
				{
					temp1 = (float)(temp - _nMemory[393]) / (float)(_nMemory[394] - _nMemory[393]) * 0.009f + 0.001f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
				}
				else if (temp <= _nMemory[395]) // -0.01u~-0.1uA@-1uA range
				{
					temp1 = (float)(temp - _nMemory[394]) / (float)(_nMemory[395] - _nMemory[394]) * 0.09f + 0.01f;
					_voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					_voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					_voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
				}
				else if (temp <= _nMemory[396]) // -0.1u~-0.5uA@-1uA range
				{
					temp1 = (float)(temp - _nMemory[395]) / (float)(_nMemory[396] - _nMemory[395]) * 0.4f + 0.1f;
					if (temp1 <= 0.1) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset6V_1;
					if (temp1 <= 0.1) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset20V_1;
					if (temp1 <= 0.1) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset200V_1;
				}
				else if (temp <= _nMemory[397]) // -0.5u~-1uA@-1uA range
				{
					temp1 = (float)(temp - _nMemory[396]) / (float)(_nMemory[397] - _nMemory[396]) * 0.5f + 0.5f;
					if (temp1 <= 0.1) _voltOffset6V_1 = _voltOffset6V_1 * temp1 / 0.1f;
					else _voltOffset6V_1 = (_voltOffset6V_2 - _voltOffset6V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset6V_1;
					if (temp1 <= 0.1) _voltOffset20V_1 = _voltOffset20V_1 * temp1 / 0.1f;
					else _voltOffset20V_1 = (_voltOffset20V_2 - _voltOffset20V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset20V_1;
					if (temp1 <= 0.1) _voltOffset200V_1 = _voltOffset200V_1 * temp1 / 0.1f;
					else _voltOffset200V_1 = (_voltOffset200V_2 - _voltOffset200V_1) * (temp1 - 0.1f) / 0.9f + _voltOffset200V_1;
				}
				else if (temp >= _nMemory[397]) // -1uA@-1uA range
				{
					temp1 = (float)temp / (float)_nMemory[397] * 1.0f;
					_voltOffset6V_1 = _voltOffset6V_2 * temp1;
					_voltOffset20V_1 = _voltOffset20V_2 * temp1;
					_voltOffset200V_1 = _voltOffset200V_2 * temp1;
				}
				_sRtnIF[0] = Convert.ToString(temp1);
				_sRtnIF[1] = "uA";
			}
		}

		private string[] cal_IF(byte bNowHWRange, int CMP, byte Limit_I1, UInt16 AD0)
		{
			//float temp1 = 0;
			//UInt16 temp = AD0;

			if ((bNowHWRange & 0x10) == 0)					// I = 2.2A range ON, bHW=0x10 control
			{
				if ((bNowHWRange & 0x08) == 0)				// I = 3A range ( 2.2A Range & 0.8A Range ON )
				{
					calc_3ARange_IF(CMP, AD0);
				}
				else										
				{
					calc_2ARange_IF(CMP, AD0);				// I = 2A range
				}	
			}
			else if ((bNowHWRange & 0x08) == 0)				// I = 800mA range, bHW=0x80 control
			{
				calc_800mARange_IF(CMP, AD0);
			}
			else if ((bNowHWRange & 0x04) == 0)				// I = 100mA & 10mA range, bHW=0x04 control
			{
				if ((Limit_I1 & 0x80) == 0)					// 100mA range , MasterGainLoop = 0
				{
					calc_100mARange_IF(CMP, AD0);
				}
				else // 10mA range
				{
					calc_10mARange_IF(CMP, AD0);
				}
			}
			else if ((bNowHWRange & 0x02) == 0)				// I = 1mA & 100uA range, bHW=0x02control
			{
				if ((Limit_I1 & 0x80) == 0)					// 1mA range , MasterGainLoop = 0
				{
					calc_1mARange_IF(CMP, AD0);
				}
				else // 100uA range
				{
					calc_100uARange_IF(CMP, AD0);
				}
			}
			else											// I = 10uA & 1uA range, bHW=0x?? control , // Gilbert 邏輯不夠嚴謹
			{
				if ((Limit_I1 & 0x80) == 0)					// 10uA range, MasterGainLoop = 0
				{
					calc_10uARange_IF(CMP, AD0);
				}
				else										// 1uA range
				{
					calc_1uARange_IF(CMP, AD0);
				}
			}

			if ( CMP < 0x1980 )
			{
				_sRtnIF[0] = Convert.ToString((0.0f - Convert.ToSingle(_sRtnIF[0])));
			}

			if ( _sRtnIF[1] =="A" ) { this._rtnValue[0] = Convert.ToSingle(_sRtnIF[0]); }
			if ( _sRtnIF[1] =="mA" ) { this._rtnValue[0] = Convert.ToSingle(_sRtnIF[0]) / 1000.0f; }
			if (_sRtnIF[1] == "uA") { this._rtnValue[0] = Convert.ToSingle(_sRtnIF[0]) / 1000000.0f; }

			return _sRtnIF.Clone() as string[];
		}

		private string[] cal_IF2(byte bNowHWRange, int CMP, byte Limit_I1, UInt16 AD0)
		{
			byte iRangeCtrl = bNowHWRange;

			iRangeCtrl = Convert.ToByte(iRangeCtrl & 0x1E);
			
			switch ( iRangeCtrl )
			{
				case 0x06 :										// xxx0-0110
					calc_3ARange_IF(CMP, AD0);					// I = 3A range ( 2.2A Range & 0.8A Range ON )
					break;
				//--------------------------------------
				case 0x0E :										// xxx0-1110
					calc_2ARange_IF(CMP, AD0);					// I = 2A range 
					break;
				//--------------------------------------
				case 0x16 :										// xxx1-0110
					calc_800mARange_IF(CMP, AD0);				// I =800mA range 
					break;
				//--------------------------------------
				case 0x1A :										// xxx1-1010
					if ((Limit_I1 & 0x80) == 0)					// I = 100mA range , MasterGainLoop = 0
					{
						calc_100mARange_IF(CMP, AD0);
					}
					else										// I = 10mA range , MasterGainLoop = 1
					{
						calc_10mARange_IF(CMP, AD0);
					}
					break;
				//--------------------------------------
				case 0x1C :										// xxx1-1100
					if ((Limit_I1 & 0x80) == 0)					// I = 1mA range , MasterGainLoop = 0
					{
						calc_1mARange_IF(CMP, AD0);
					}
					else										// I = 100uA range , MasterGainLoop = 1
					{
						calc_100uARange_IF(CMP, AD0);
					}
					break;
				//--------------------------------------
				case 0x1E :										// xxx1-1110
					if ((Limit_I1 & 0x80) == 0)					// 10uA range, MasterGainLoop = 0
					{
						calc_10uARange_IF(CMP, AD0);
					}
					else										// 1uA range , MasterGainLoop = 1
					{
						calc_1uARange_IF(CMP, AD0);
					}
					break;
				//--------------------------------------
				default :										// Others Status, DO NOT Anything
					break;
			}

			return _sRtnIF.Clone() as string[];
		}

		private E_IRange GetIRange(byte bNowHWRange, byte Limit_I_High)
		{
			//==============================================================================================================================
			// HWRange	=	b7	b6	b5	b4	-	b3	b2	b1	b0	,	b0 = 0, 閃避 0xFF 的 SOF/EOF 定義
			// IRange	=	x	x	x	v	-	v	v	v	0	, 0x1E
			// VRange	=	v	v	v	x	-	x	x	x	0	, 0xE0
			// 0x1980  <= CMP <= 0x30AF : Positive Value	, 0x024F  <= CMP <0x1980 : Negitive Value
			// 
			// Limit_I	= 0x0000 ~ 0x1FFE, 表達 12bit 的資料， b0 = 0, 閃避 0xFF 的 SOF/EOF 定義
			// Limit_I_HighByte 剩下的3 bits用來表達其他事項,
			// Limit_I_HighByte =	b7	b6	b5	b4	-	b3,	b2,	b1,	b0
			//	(masterGL)			0	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x00,	10uA,	1mA,	100mA
			//						1	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x80,	1uA,	100uA,	10mA	
			//==============================================================================================================================

			int masterGainLoop = Limit_I_High & 0x80;
			E_IRange iRangeMatch = E_IRange._1uA;

			bNowHWRange = (byte)(bNowHWRange & 0x1E);

			if ((bNowHWRange == 0x1E) && (masterGainLoop == 0x80)) { iRangeMatch = E_IRange._1uA; }
			else if ((bNowHWRange == 0x1E) && (masterGainLoop == 0x00)) { iRangeMatch = E_IRange._10uA; }
			else if ((bNowHWRange == 0x1C) && (masterGainLoop == 0x80)) { iRangeMatch = E_IRange._100uA; }
			else if ((bNowHWRange == 0x1C) && (masterGainLoop == 0x00)) { iRangeMatch = E_IRange._1mA; }
			else if ((bNowHWRange == 0x1A) && (masterGainLoop == 0x80)) { iRangeMatch = E_IRange._10mA; }
			else if ((bNowHWRange == 0x1A) && (masterGainLoop == 0x00)) { iRangeMatch = E_IRange._100mA; }
			else if (bNowHWRange == 0x16) { iRangeMatch = E_IRange._800mA; }
			else if (bNowHWRange == 0x0E) { iRangeMatch = E_IRange._2A; }
			else if (bNowHWRange == 0x06) { iRangeMatch = E_IRange._3A; }
			else
			{
				iRangeMatch = E_IRange._1uA;
			}

			return iRangeMatch;

		}

		private byte SetIVRange2HWRange(E_IRange iRange, E_VRange vRange)
		{
			//==============================================================================================================================
			// HWRange	=	b7	b6	b5	b4	-	b3	b2	b1	b0	,	b0 = 0, 閃避 0xFF 的 SOF/EOF 定義
			// IRange	=	x	x	x	v	-	v	v	v	0	, 0x1E
			// VRange	=	v	v	v	x	-	x	x	x	0	, 0xE0
			// 0x1980  <= CMP <= 0x30AF : Positive Value	, 0x024F  <= CMP <0x1980 : Negitive Value
			// 
			// Limit_I	= 0x0000 ~ 0x1FFE, 表達 12bit 的資料， b0 = 0, 閃避 0xFF 的 SOF/EOF 定義
			// Limit_I_HighByte 剩下的3 bits用來表達其他事項,
			// Limit_I_HighByte =	b7	b6	b5	b4	-	b3,	b2,	b1,	b0
			//	(masterGL)			0	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x00,	10uA,	1mA,	100mA
			//						1	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x80,	1uA,	100uA,	10mA	
			//==============================================================================================================================

			this._bHW_DevRange = (byte)((((int)iRange) & 0x00FF) + vRange);

			return this._bHW_DevRange;
		}

		private string[] calcIFN_VDB_ByAD0(byte bNowHWRange, int CMP, byte Limit_I_HighByte, UInt16 AD0)
		{
			//==============================================================================================================================
			// HWRange	=	b7	b6	b5	b4	-	b3	b2	b1	b0	,	b0 = 0, 閃避 0xFF 的 SOF/EOF 定義
			// IRange	=	x	x	x	v	-	v	v	v	0	, 0x1E
			// VRange	=	v	v	v	x	-	x	x	x	0	, 0xE0
			// 0x1980  <= CMP <= 0x30AF : Positive Value	, 0x024F  <= CMP <0x1980 : Negitive Value
			// 
			// Limit_I	= 0x0000 ~ 0x1FFE, 表達 12bit 的資料， b0 = 0, 閃避 0xFF 的 SOF/EOF 定義
			// Limit_I_HighByte 剩下的3 bits用來表達其他事項,
			// Limit_I_HighByte =	b7	b6	b5	b4	-	b3,	b2,	b1,	b0
			//	(masterGL)			0	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x00,	10uA,	1mA,	100mA
			//						1	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x80,	1uA,	100uA,	10mA	
			//==============================================================================================================================
			int memVDStartIndex = -1, memIFStartIndex = -1;
			float iRangeMax = 1.0f;
			byte masterGainLoop = 0;
			float calcIFN = 0.0f;
			double voltOffset = 0.0;
			E_IRange iRangeSetting;
			E_VRange vRangeSetting;

			//V6 = V20 = V200 = 0.0f;

			iRangeSetting = this.GetIRange(bNowHWRange, Limit_I_HighByte);
			vRangeSetting = (E_VRange)(bNowHWRange & 0xE0);

			#region >> (01) 確認 IRange , VRange , 取得各檔位記憶中的起始點位置

			switch (vRangeSetting)
			{
				case E_VRange._6V:
					if (LDevConst.CMP_CENTER <= CMP) { memVDStartIndex = LDevConst.memTableVDB[0]; }		// +6V_VRange
					else { memVDStartIndex = LDevConst.memTableVDB[2]; }		// -6V_VRange
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_VRange._20V:
					if (LDevConst.CMP_CENTER <= CMP) { memVDStartIndex = LDevConst.memTableVDB[4]; }		// +20V_VRange
					else { memVDStartIndex = LDevConst.memTableVDB[6]; }		// -20V_VRange
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_VRange._200V:
					if (LDevConst.CMP_CENTER <= CMP) { memVDStartIndex = LDevConst.memTableVDB[8]; }		// +200V_VRange
					else { memVDStartIndex = LDevConst.memTableVDB[10]; }	// -200V_VRange
					break;
				//----------------------------------------------------------------------------------------------------------------
				default:
					memVDStartIndex = -1;
					break;
			}

			switch (iRangeSetting)
			{
				case E_IRange._1uA:
					{
						iRangeMax = 1.0f;
						_sRtnIF[1] = "uA";
						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[0]; }		// +1uA_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[2]; }		// -1uA_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_IRange._10uA:
					{
						iRangeMax = 10.0f;
						_sRtnIF[1] = "uA";
						memVDStartIndex = memVDStartIndex + 1;
						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[4]; }		// +10uA_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[6]; }		// -10uA_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_IRange._100uA:
					{
						iRangeMax = 100.0f;
						_sRtnIF[1] = "uA";
						memVDStartIndex = memVDStartIndex + 2;
						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[8]; }		// +100uA_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[10]; }		// -100uA_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_IRange._1mA:
					{
						iRangeMax = 1000.0f;	// 1mA = 1000uA
						_sRtnIF[1] = "uA";
						memVDStartIndex = memVDStartIndex + 3;
						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[12]; }		// +1mA_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[14]; }		// -1mA_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_IRange._10mA:
					{
						iRangeMax = 10.0f;
						_sRtnIF[1] = "mA";
						memVDStartIndex = memVDStartIndex + 4;
						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[16]; }		// +10mA_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[18]; }		// -10mA_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_IRange._100mA:
					{
						iRangeMax = 100.0f;
						_sRtnIF[1] = "mA";
						memVDStartIndex = memVDStartIndex + 5;
						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[20]; }		// +100mA_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[22]; }		// -100mA_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_IRange._800mA:
					{
						iRangeMax = 800.0f;
						_sRtnIF[1] = "mA";
						if (((bNowHWRange & 0xE0) == (byte)E_VRange._6V) || ((bNowHWRange & 0xE0) == (byte)E_VRange._20V))
						{
							memVDStartIndex = memVDStartIndex + 6;
						}
						else
						{
							memVDStartIndex = -1;
						}

						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[24]; }		// +800mA_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[26]; }		// -800mA_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_IRange._2A:
					{
						iRangeMax = 2.0f;
						_sRtnIF[1] = "A";
						if ((bNowHWRange & 0xE0) == (byte)E_VRange._6V)
						{
							memVDStartIndex = memVDStartIndex + 7;
						}
						else
						{
							memVDStartIndex = -1;
						}

						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[28]; }		// +2A_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[30]; }		// -2A_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				case E_IRange._3A:
					{
						iRangeMax = 3.0f;
						_sRtnIF[1] = "A";
						if ((bNowHWRange & 0xE0) == (byte)E_VRange._6V)
						{
							memVDStartIndex = memVDStartIndex + 8;
						}
						else
						{
							memVDStartIndex = -1;
						}

						if (LDevConst.CMP_CENTER <= CMP) { memIFStartIndex = LDevConst.memTableIFN[32]; }		// +3A_IRange
						else { memIFStartIndex = LDevConst.memTableIFN[34]; }		// -3A_IRange
					}
					break;
				//----------------------------------------------------------------------------------------------------------------
				default:
					memVDStartIndex = -1;
					break;
			}

			if ((memIFStartIndex == -1) || (memVDStartIndex == -1))
			{
				calcIFN = 0.0f;
				voltOffset = 0.0f;

				//V6 = 0f;
				//V20 = 0f;
				//V200 = 0f;

				_sRtnIF[0] = calcIFN.ToString();
				_sRtnIF[1] = "A";

				return _sRtnIF;
			}

			#endregion

			float segCurrMin = 0.0f, segCurrMax = 0.0f;
			UInt16 segBitMin = 0, segBitMax = 0;
			float[] segScale = new float[] { 0.0f, 0.001f, 0.01f, 0.1f, 0.5f, 1.0f };		// 第一個 Segment 為 0 點起算，最小值就是 0, 沒有負數

			#region >> (02) 計算電流 IFN, 由ADC 讀入的 AD0, 查表IFN_Table 內插, 求出補償後的值

			for (int p = 0; p < (segScale.Length - 1); p++)
			{
				segCurrMin = segScale[p] * iRangeMax;
				segCurrMax = segScale[p + 1] * iRangeMax;
				segBitMin = (UInt16)_nMemory[memIFStartIndex + p];
				segBitMax = (UInt16)_nMemory[memIFStartIndex + p + 1];

				if ((p == 0) && (AD0 <= segBitMin))
				{
					calcIFN = 0.0f;
					break;
				}

				if ((segBitMin < AD0) && (AD0 <= segBitMax))
				{
					if (segBitMin != segBitMax)
					{
						calcIFN = ((float)(AD0 - segBitMin)) / ((float)(segBitMax - segBitMin)) * (segCurrMax - segCurrMin) + segCurrMin;
					}
					else
					{
						calcIFN = segCurrMin;
					}
					break;
				}

				if ((p == (segScale.Length - 2)) && (segBitMax < AD0))
				{
					if (segBitMax != 0x00)
					{
						calcIFN = ((float)AD0) / ((float)segBitMax) * iRangeMax;
					}
					else
					{
						calcIFN = float.MaxValue;
					}
					break;
				}
			}

			#endregion

			#region >> (03) 計算補償電壓 Voffset, 經由算出的IFN 電流值, 查表 Voffset_Table 內插,求出要補償的值
			//------------------------------------------------------------------------
			// Read the Voffset data from Memory, transfer the to real value,
			// It will have positive and negtive value, when it mine 127.
			//------------------------------------------------------------------------

			float calVD_IFmin = 0.0f, calVD_IFmin02 = 0.0f;
			double volt01 = 0, volt02 = 0, volt03 = 0, volt04 = 0;

			if (_nMemory[memVDStartIndex] == 0xFFFF)		// offset @ 1uA
			{
				volt01 = 0;
				volt02 = 0;
			}
			else
			{
				volt01 = ((UInt16)_nMemory[memVDStartIndex] & 0xFF) - 127;
				volt02 = ((UInt16)(_nMemory[memVDStartIndex] & 0xFF00) >> 8) - 127;
			}

			//-----------------------------------------------------------------------
			// 除了 1uA_IRange 在表中沒有前一個區段外，其他都可以在往下查下一個區段,
			// 只是，使否採用，在看實務狀況，
			//-----------------------------------------------------------------------
			if (((bNowHWRange & 0x1E) == (byte)E_IRange._1uA) && (masterGainLoop == 0x80))		// +1uA_IRange, -1uA_IRange
			{
				volt03 = 0.0;
				volt04 = 0.0;
			}
			else
			{
				if (_nMemory[memVDStartIndex - 1] == 0xFFFF)		// offset @ 1uA
				{
					volt03 = 0;
					volt04 = 0;
				}
				else
				{
					volt03 = ((UInt16)_nMemory[memVDStartIndex - 1] & 0xFF) - 127;
					volt04 = ((UInt16)(_nMemory[memVDStartIndex - 1] & 0xFF00) >> 8) - 127;
				}
			}

			calVD_IFmin = segScale[3] * iRangeMax;				// iRangeMax * 1/10,	查表的這一個區段
			calVD_IFmin02 = segScale[2] * iRangeMax;			// iRangeMax * 1/100,	查表對應的下一個區段

			if (iRangeSetting == E_IRange._800mA)	// 特定檔位，在掃描 Voffset 使用的電流，並非做 IFN 掃描的點位, 無法由 segScale 推算
			{
				calVD_IFmin = 100.0f;
				calVD_IFmin02 = 10.0f;
			}

			if (iRangeSetting == E_IRange._2A)
			{
				calVD_IFmin = 0.8f;
				calVD_IFmin02 = 100.0f;
			}

			if (iRangeSetting == E_IRange._3A)
			{
				calVD_IFmin = 2.0f;
				calVD_IFmin02 = 0.8f;
			}

			//-------------------------------------------------------------------------------------------------
			// 目前，對應 1uA_10uA_100uA_10mA_800mA_2A_3A_IRange 這幾個檔位，都只有對應當下的 Voffset_Table 的一個區段，
			// 來進行查表，例如 10uA_IRange, 只有用  1uA ~ 10uA, 這個 Voffset 掃描的區段，
			// 其他的  IRange 會使用當下這一個區段，與下一個區段，形成更佳的查表機制，
			// 例如 100mA_IRange, 會使用 100mA ~ 10mA 與 10mA ~ 1mA 這二個 Voffset 掃描的區段，進行查表，
			// Gilbert // 實務上，在確認
			//-------------------------------------------------------------------------------------------------

			if (	(iRangeSetting == E_IRange._1uA) || (iRangeSetting == E_IRange._10uA) || (iRangeSetting == E_IRange._100uA) ||
					(iRangeSetting == E_IRange._10mA) || (iRangeSetting == E_IRange._800mA) ||
					(iRangeSetting == E_IRange._2A) || (iRangeSetting == E_IRange._3A))
			{
				if (calcIFN <= calVD_IFmin)
				{
					if (Math.Abs(calVD_IFmin) > float.Epsilon)
					{
						voltOffset = volt01 * calcIFN / calVD_IFmin;
					}
					else
					{
						voltOffset = float.MaxValue;
					}
				}
				else if ((calVD_IFmin < calcIFN) && (calcIFN <= iRangeMax))  //  if (AD0 <= segBitMax)
				{
					if (Math.Abs((iRangeMax - calVD_IFmin)) > float.Epsilon)
					{
						voltOffset = (volt02 - volt01) * (calcIFN - calVD_IFmin) / (iRangeMax - calVD_IFmin) + volt01;
					}
					else
					{
						voltOffset = float.MaxValue;
					}
				}
				else  // if (AD0 > segBitMax)
				{
					if (Math.Abs(iRangeMax) > float.Epsilon)
					{
						voltOffset = volt02 * calcIFN / iRangeMax;
					}
					else
					{
						voltOffset = float.MaxValue;
					}
				}
			}
			else
			{
				if (calcIFN <= calVD_IFmin02)
				{
					if (Math.Abs(calVD_IFmin02) > float.Epsilon)
					{
						voltOffset = volt03 * calcIFN / calVD_IFmin02;
					}
					else
					{
						voltOffset = float.MaxValue;
					}
				}
				else if ((calVD_IFmin02 < calcIFN) && (calcIFN <= calVD_IFmin))
				{
					if (Math.Abs(calVD_IFmin - calVD_IFmin02) > float.Epsilon)
					{
						voltOffset = (volt04 - volt03) * (calcIFN - calVD_IFmin02) / (calVD_IFmin - calVD_IFmin02) + volt03;
					}
					else
					{
						voltOffset = float.MaxValue;
					}
				}
				else if ((calVD_IFmin < calcIFN) && (calcIFN <= iRangeMax))  // (AD0 <= segBitMax)
				{
					if (Math.Abs(iRangeMax - calVD_IFmin) > float.Epsilon)
					{
						voltOffset = (volt02 - volt01) * (calcIFN - calVD_IFmin) / (iRangeMax - calVD_IFmin) + volt01;
					}
					else
					{
						voltOffset = float.MaxValue;
					}

				}
				else   //if (AD0 > segBitMax)
				{
					if (Math.Abs(iRangeMax) > float.Epsilon)
					{
						voltOffset = volt02 * calcIFN / iRangeMax;
					}
					else
					{
						voltOffset = float.MaxValue;
					}

				}
			}

			_sRtnIF[0] = Convert.ToString(calcIFN);

			//if (_sRtnIF[1] == "uA") { _rtnValue[0] = (double)calcIFN / 1E6; }
			//if (_sRtnIF[1] == "mA") { _rtnValue[0] = (double)calcIFN / 1E3; }

			if (vRangeSetting == E_VRange._6V)	{ voltOffset = voltOffset * 1E-3; }		// Voff, Using mV as one bit
			if (vRangeSetting == E_VRange._20V) { voltOffset = voltOffset * 1E-3; }		// Voff, Using mV as one bit
			if (vRangeSetting == E_VRange._200V) { voltOffset = voltOffset * 1E-2; }		// Voff, Using 10mV as one bit

			#endregion

			_sRtnIF[2] = voltOffset.ToString();
			_sRtnIF[3] = "V";

			return _sRtnIF.Clone() as string[];
		}

		private double calcIFN_ByAD0(byte bNowHWRange, int CMP, byte masterGL, UInt16 AD0)
		{
			//==============================================================================================================================
			// Limit_I_HighByte =	b7	b6	b5	b4	-	b3,	b2,	b1,	b0
			//	(masterGL)			0	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x00,	10uA,	1mA,	100mA
			//						1	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x80,	1uA,	100uA,	10mA	
			//==============================================================================================================================
			int memIFStartIndex = -1, index_IRange;
			double iRangeMax = 1.0;
			double calcIFN = 0.0;
			double segCurrMin = 0.0, segCurrMax = 0.0;
			UInt16 segBitMin = 0, segBitMax = 0;
			E_IRange iRangeSetting;

			iRangeSetting = this.GetIRange(bNowHWRange, masterGL);

			//----------------------------------------------------------------------------------------------
			// (01) 確認 IRange , 取得各檔位記憶中的起始點位置
			//----------------------------------------------------------------------------------------------
			index_IRange = Array.IndexOf(Enum.GetValues(typeof(E_IRange)), iRangeSetting);
			iRangeMax = LDevConst.IRANGE_SegValue[index_IRange];

			_sRtnIF[1] = "uA";
			if ((4 <= index_IRange) && (index_IRange <= 6))
			{
				iRangeMax = iRangeMax * 1E-3;	// change to mA unit, for 10mA, 100mA, 800mA
				_sRtnIF[1] = "mA";
			}

			if (7 <= index_IRange)
			{
				iRangeMax = iRangeMax * 1E-6;	// chage to A unit
				_sRtnIF[1] = "A";
			}

			if (LDevConst.CMP_CENTER <= CMP)
			{
				memIFStartIndex = LDevConst.memTableIFN[index_IRange << 2];			// index = 0, 4, 8,		=>> +1uA, +10uA, +100uA, ...
			}
			else
			{
				memIFStartIndex = LDevConst.memTableIFN[ (index_IRange << 2) + 2];		// index = 2, 6, 10		=>> -1uA, -10uA, -100uA, ...
			}

			//----------------------------------------------------------------------------------------------
			// (02) 計算電流 IFN, 由ADC 讀入的 AD0, 查表IFN_Table 內插, 求出補償後的值
			//----------------------------------------------------------------------------------------------			
			for (int p = 0; p < (LDevConst.IFN_SegScale.Length - 1); p++)
			{
				segCurrMin = LDevConst.IFN_SegScale[p] * iRangeMax;
				segCurrMax = LDevConst.IFN_SegScale[p + 1] * iRangeMax;
				segBitMin = (UInt16)_nMemory[memIFStartIndex + p];
				segBitMax = (UInt16)_nMemory[memIFStartIndex + p + 1];

				if ((p == 0) && (AD0 <= segBitMin))
				{
					calcIFN = 0.0;
					break;
				}

				if ((segBitMin < AD0) && (AD0 <= segBitMax))
				{
					if (segBitMin != segBitMax)
					{
						calcIFN = ((double)(AD0 - segBitMin)) / ((double)(segBitMax - segBitMin)) * (segCurrMax - segCurrMin) + segCurrMin;
					}
					else
					{
						calcIFN = segCurrMin;
					}
					break;
				}

				if ((p == (LDevConst.IFN_SegScale.Length - 2)) && (segBitMax < AD0))
				{
					if (segBitMax != 0x00)
					{
						calcIFN = ((double)AD0) / ((double)segBitMax) * iRangeMax;
					}
					else
					{
						calcIFN = double.MaxValue;
					}
					break;
				}
			}

			

			//if (_sRtnIF[1] == "uA") { _rtnValue[0] = calcIFN * 1E-6; }
			//if (_sRtnIF[1] == "mA") { _rtnValue[0] = calcIFN * 1E-3; }

			if (_sRtnIF[1] == "uA") { calcIFN = calcIFN * 1E-6; }
			if (_sRtnIF[1] == "mA") { calcIFN = calcIFN * 1E-3; }

			if ( CMP < LDevConst.CMP_CENTER )
			{
				calcIFN = 0.0 - calcIFN;
			}

			this._sRtnIF[0] = Convert.ToString(calcIFN);
			this._sRtnIF[1] = "A";

			return calcIFN;
		}

		private double calcVDB_ByIFN(byte bNowHWRange, int CMP, byte masterGL, double calcIFN)
		{
			//==============================================================================================================================
			// Limit_I_HighByte =	b7	b6	b5	b4	-	b3,	b2,	b1,	b0
			//	(masterGL)			0	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x00,	10uA,	1mA,	100mA
			//						1	x	x	x	-	x	x	x	x	==>	 MasterGainLoop = 0x80,	1uA,	100uA,	10mA	
			//==============================================================================================================================
			int memVDStartIndex = -1, index_IRange = -1, index_VRange = -1;
			double iRangeMax = 0.0;
			double calVD_IFmin = 0.0, calVD_IFmin02 = 0.0;
			double volt01 = 0, volt02 = 0, volt03 = 0, volt04 = 0, voltOffset = 0.0; ;
			E_IRange iRangeSetting;
			E_VRange vRangeSeting;

			//V6 = V20 = V200 = 0.0f;

			iRangeSetting = this.GetIRange(bNowHWRange, masterGL);
			vRangeSeting = (E_VRange)(bNowHWRange & 0xE0);
			calcIFN = Math.Abs(calcIFN) * 1E6;							// 在函式內計算，查表過程，都是一正值計算，對應校正資料，以 uA 為計算單位

			//----------------------------------------------------------------------------------------------		
			// (01) 確認 VRange , 取得各檔位記憶中的起始點位置
			//----------------------------------------------------------------------------------------------		
			index_IRange = Array.IndexOf(Enum.GetValues(typeof(E_IRange)), iRangeSetting);
			iRangeMax = LDevConst.IRANGE_SegValue[index_IRange];

			if ((4 <= index_IRange) && (index_IRange <= 6))			
			{
				iRangeMax = iRangeMax * 1E-3;	// change "uA" to "mA" unit, for 10mA, 100mA, 800mA
				calcIFN = calcIFN * 1E-3;
			}

			if (7 <= index_IRange)
			{
				iRangeMax = iRangeMax * 1E-6;	// chage "uA" to "A" unit for 2A, 3A
				calcIFN = calcIFN * 1E-6;
			}

			index_VRange = Array.IndexOf(Enum.GetValues(typeof(E_VRange)), vRangeSeting);

			if (LDevConst.CMP_CENTER <= CMP)
			{
				memVDStartIndex = LDevConst.memTableVDB[index_VRange << 2]; 			// index = 0, 4, 8		==> +6V, +20V, +200V
			}
			else
			{
				memVDStartIndex = LDevConst.memTableVDB[ (index_VRange << 2) + 2];		// index = 2, 6, 10		==> -6V, -20V, -200V
			}

			if (vRangeSeting == E_VRange._6V)
			{
				memVDStartIndex = memVDStartIndex + index_IRange;
			}
			else if ((vRangeSeting == E_VRange._20V) && (index_IRange <= 6))			// 20V_VRange with 1uA, 10uA, 100uA, 1mA, 10mA, 100mA, 800mA : index 0 ~ 6
			{
				memVDStartIndex = memVDStartIndex + index_IRange;
			}
			else if ((vRangeSeting == E_VRange._200V) && (index_IRange <= 5))			// 200V_VRange with 1uA, 10uA, 100uA, 1mA, 10mA, 100mA : index 0 ~ 5
			{
				memVDStartIndex = memVDStartIndex + index_IRange;
			}
			else
			{
				memVDStartIndex = -1;
			}

			if (memVDStartIndex == -1)
			{
				voltOffset = 0.0;
				return voltOffset;
			}
			//----------------------------------------------------------------------------------------------	
			// (02) 計算補償電壓 Voffset, 經由算出的IFN 電流值, 查表 Voffset_Table 內插,求出要補償的值
			//----------------------------------------------------------------------------------------------	

			if (_nMemory[memVDStartIndex] == 0xFFFF)		// offset @ 1uA
			{
				volt01 = 0;
				volt02 = 0;
			}
			else
			{
				volt01 = ((UInt16)_nMemory[memVDStartIndex] & 0xFF) - 127;
				volt02 = ((UInt16)(_nMemory[memVDStartIndex] & 0xFF00) >> 8) - 127;
			}

			//-----------------------------------------------------------------------
			// 除了 1uA_IRange 在表中沒有前一個區段外，其他都可以在往下查下一個區段,
			// 只是，使否採用，在看實務狀況，
			//-----------------------------------------------------------------------
			if (iRangeSetting == E_IRange._1uA)		// +1uA_IRange, -1uA_IRange
			{
				volt03 = 0.0;
				volt04 = 0.0;
			}
			else
			{
				if (_nMemory[memVDStartIndex - 1] == 0xFFFF)		// offset @ 1uA
				{
					volt03 = 0;
					volt04 = 0;
				}
				else
				{
					volt03 = ((UInt16)_nMemory[memVDStartIndex - 1] & 0xFF) - 127;
					volt04 = ((UInt16)(_nMemory[memVDStartIndex - 1] & 0xFF00) >> 8) - 127;
				}
			}

			calVD_IFmin = LDevConst.IFN_SegScale[3] * iRangeMax;			// iRangeMax * 1/10,	查表的這一個區段
			calVD_IFmin02 = LDevConst.IFN_SegScale[2] * iRangeMax;			// iRangeMax * 1/100,	查表對應的下一個區段

			if (iRangeSetting == E_IRange._800mA)	// 特定檔位，在掃描 Voffset 使用的電流，並非做 IFN 掃描的點位, 無法由 segScale 推算
			{
				calVD_IFmin = 100.0;
				calVD_IFmin02 = 10.0;
			}

			if (iRangeSetting == E_IRange._2A)
			{
				calVD_IFmin = 0.8;
				calVD_IFmin02 = 100.0;
			}

			if (iRangeSetting == E_IRange._3A)
			{
				calVD_IFmin = 2.0;
				calVD_IFmin02 = 0.8;
			}

			//-------------------------------------------------------------------------------------------------
			// 目前，對應 1uA_10uA_100uA_10mA_800mA_2A_3A_IRange 這幾個檔位，都只有對應當下的 Voffset_Table 的一個區段，
			// 來進行查表，例如 10uA_IRange, 只有用  1uA ~ 10uA, 這個 Voffset 掃描的區段，
			// 其他的  IRange 會使用當下這一個區段，與下一個區段，形成更佳的查表機制，
			// 例如 100mA_IRange, 會使用 100mA ~ 10mA 與 10mA ~ 1mA 這二個 Voffset 掃描的區段，進行查表，
			// Gilbert // 實務上，在確認
			//-------------------------------------------------------------------------------------------------

			if (	(iRangeSetting == E_IRange._1uA) || (iRangeSetting == E_IRange._10uA) || (iRangeSetting == E_IRange._100uA) ||
					(iRangeSetting == E_IRange._10mA) || (iRangeSetting == E_IRange._800mA) ||
					(iRangeSetting == E_IRange._2A) || (iRangeSetting == E_IRange._3A))
			{
				if (calcIFN <= calVD_IFmin)
				{
					if (Math.Abs(calVD_IFmin) > double.Epsilon)
					{
						voltOffset = volt01 * calcIFN / calVD_IFmin;
					}
					else
					{
						voltOffset = double.MaxValue;
					}
				}
				else if ((calVD_IFmin < calcIFN) && (calcIFN <= iRangeMax))  //  if (AD0 <= segBitMax)
				{
					if (Math.Abs((iRangeMax - calVD_IFmin)) > double.Epsilon)
					{
						voltOffset = (volt02 - volt01) * (calcIFN - calVD_IFmin) / (iRangeMax - calVD_IFmin) + volt01;
					}
					else
					{
						voltOffset = double.MaxValue;
					}
				}
				else  // if (AD0 > segBitMax)
				{
					if (Math.Abs(iRangeMax) > double.Epsilon)
					{
						voltOffset = volt02 * calcIFN / iRangeMax;
					}
					else
					{
						voltOffset = double.MaxValue;
					}
				}
			}
			else
			{
				if (calcIFN <= calVD_IFmin02)
				{
					if (Math.Abs(calVD_IFmin02) > double.Epsilon)
					{
						voltOffset = volt03 * calcIFN / calVD_IFmin02;
					}
					else
					{
						voltOffset = double.MaxValue;
					}
				}
				else if ((calVD_IFmin02 < calcIFN) && (calcIFN <= calVD_IFmin))
				{
					if (Math.Abs(calVD_IFmin - calVD_IFmin02) > double.Epsilon)
					{
						voltOffset = (volt04 - volt03) * (calcIFN - calVD_IFmin02) / (calVD_IFmin - calVD_IFmin02) + volt03;
					}
					else
					{
						voltOffset = double.MaxValue;
					}
				}
				else if ((calVD_IFmin < calcIFN) && (calcIFN <= iRangeMax))  // (AD0 <= segBitMax)
				{
					if (Math.Abs(iRangeMax - calVD_IFmin) > double.Epsilon)
					{
						voltOffset = (volt02 - volt01) * (calcIFN - calVD_IFmin) / (iRangeMax - calVD_IFmin) + volt01;
					}
					else
					{
						voltOffset = double.MaxValue;
					}

				}
				else   //if (AD0 > segBitMax)
				{
					if (Math.Abs(iRangeMax) > double.Epsilon)
					{
						voltOffset = volt02 * calcIFN / iRangeMax;
					}
					else
					{
						voltOffset = double.MaxValue;
					}

				}
			}

			if (vRangeSeting == E_VRange._6V)	{ voltOffset = voltOffset / 1E3; }		// Voff, Using mV as one bit
			if (vRangeSeting == E_VRange._20V)	{ voltOffset = voltOffset / 1E3; }		// Voff, Using mV as one bit
			if (vRangeSeting == E_VRange._200V) { voltOffset = voltOffset / 1E2; }		// Voff, Using 10mV as one bit


			_sRtnIF[2] = voltOffset.ToString();
			_sRtnIF[3] = "V";

			return voltOffset;
		}

		private double calcVFP_ByCMP(byte bNowHWRange, int CMP, UInt16 readBackCMP)
		{
			double volt = 0.0d;
			double OP_Volt = 0.0d, ratio = 0.0d;
			int memStart = 0;
			int segLeftMax, segRightMin;
			int p = 0;
			int cmpLeft = LDevConst.CMP_CENTER;
			int cmpRight = cmpLeft + LDevConst.A_REG_STEP;
			E_VRange vRangeSet;

			//---------------------------------------------------------------
			// HW Range =>		6V	: b-0100-0000
			//					20V	: b-0110-0000
			//					200V: b-1010-0000
			// bit7, bit6, bit5 control the range definition, ( & 0xE0 )
			//---------------------------------------------------------------

			vRangeSet = (E_VRange)(bNowHWRange & 0xE0);

			switch (vRangeSet)
			{
				case E_VRange._6V:
					if (readBackCMP >= LDevConst.CMP_CENTER)
					{
						memStart = LDevConst.memTableVFP[0];
					}
					else
					{
						memStart = LDevConst.memTableVFP[2];
					}
					OP_Volt = LDevConst.OP6VRangeOut;
					break;
				//---------------------------------------------------------
				case E_VRange._20V:
					if (readBackCMP >= LDevConst.CMP_CENTER)
					{
						memStart = LDevConst.memTableVFP[4];
					}
					else
					{
						memStart = LDevConst.memTableVFP[6];
					}
					OP_Volt = LDevConst.OP20VRangeOut;
					break;
				//---------------------------------------------------------
				case E_VRange._200V:
					if (readBackCMP >= LDevConst.CMP_CENTER)
					{
						memStart = LDevConst.memTableVFP[8];
					}
					else
					{
						memStart = LDevConst.memTableVFP[10];
					}
					OP_Volt = LDevConst.OP200VRangeOut;
					break;
				//---------------------------------------------------------
				default:
					return 0.0d;
					break;
			}

			//------------------------------------------------------------------------
			// memory index = 0, from 0x1980<= data <0x1A80 compensation point,					
			//------------------------------------------------------------------------
			p = 0;
			if (readBackCMP >= LDevConst.CMP_CENTER)
			{
				cmpLeft = LDevConst.CMP_CENTER;
				for (p = 0; cmpLeft < LDevConst.CMP_MAX; p++)											// (0x1980,0x1A80) ==>  0x30AF
				{
					cmpLeft = LDevConst.CMP_CENTER + LDevConst.A_REG_STEP * p;
					cmpRight = cmpLeft + LDevConst.A_REG_STEP;

					if ((cmpLeft <= readBackCMP) && (readBackCMP < cmpRight))											// 第一個區間 0x1980<= ~ <0x1A80, 最後一個 0x2980<= ~ <0x3080
					{
						segLeftMax = cmpLeft - LDevConst.HR_REG_CENTER + LDevConst.HR_REG_MAX;			// 左半的最大值 = 0x19AF = 0x1980 - 0x0080 + 0x00AF
						segRightMin = cmpRight - LDevConst.HR_REG_CENTER + LDevConst.HR_REG_MIN;			// 右半的最小值 = 0x1A4F = 0x1A80 - 0x0080 + 0x004F

						// if (readBackCMP <= segLeftMax)						// 0x1980<= ~ <=0x19AF, Left Segment
						if ((readBackCMP & 0xFF00) == (cmpLeft & 0xFF00))		// 0x1900 都還在左半面, 假設輸入的 AD1 遠超過表達的範圍，有循環重疊一小區段
						{
							ratio = (double)(readBackCMP - cmpLeft);
							volt = (double)_nMemory[memStart + p] + ((double)(_nMemory[memStart + p + 1] - _nMemory[memStart + p])) * ratio / ((double)LDevConst.HR_REG_LENGTH);
							volt = volt * OP_Volt / 32767.0;
						}
						else									// 0x1A4F<= ~ <0x1A80, Rigth Segment
						{
							//ratio = (double)(AD1 - segRightMin + LDevConst.HR_REG_MAX - LDevConst.HR_REG_CENTER + 2);	// AD1 - 0x1A4F + 1 + 0x00AF - 0x0080 + 1
							ratio = (double)(readBackCMP - LDevConst.HR_REG_DIFF - cmpLeft + 1);										// AD1 - 0x09F 變成用左半的數值表達, // Gilbert 多減的 1
							volt = (double)_nMemory[memStart + p] + ((double)(_nMemory[memStart + p + 1] - _nMemory[memStart + p])) * ratio / ((double)LDevConst.HR_REG_LENGTH);
							volt = volt * OP_Volt / 32767.0;
						}
						break;
					}
				}

				p = p - 1;	// using the final memmory segment;
				cmpLeft = LDevConst.CMP_MAX - LDevConst.HR_REG_MAX + LDevConst.HR_REG_CENTER;
				cmpRight = cmpLeft + LDevConst.A_REG_STEP;										// 虛擬最後一個 Segment ( 0x3080, 0x3180 )
				if ((cmpLeft <= readBackCMP) && (readBackCMP < cmpRight))										// 0x3080<= ~ <=030AF, Left Segment
				{
					if ((readBackCMP & 0xFF00) == (cmpLeft & 0xFF00))		// 0x1900 都還在左半面, 假設輸入的 AD1 遠超過表達的範圍，有循環重疊一小區段
					{
						ratio = (double)(readBackCMP - cmpLeft);
						volt = (double)_nMemory[memStart + p + 1] + ((double)(_nMemory[memStart + p + 1] - _nMemory[memStart + p])) * ratio / ((double)LDevConst.HR_REG_LENGTH);
						volt = volt * OP_Volt / 32767.0;
					}
					else									// 0x1A4F<= ~ <0x1A80, Rigth Segment
					{
						ratio = (double)(readBackCMP - LDevConst.HR_REG_DIFF - cmpLeft + 1);										// AD1 - 0x09F 變成用左半的數值表達, // Gilbert 多減的 1
						volt = (double)_nMemory[memStart + p + 1] + ((double)(_nMemory[memStart + p + 1] - _nMemory[memStart + p])) * ratio / ((double)LDevConst.HR_REG_LENGTH);
						volt = volt * OP_Volt / 32767.0;
					}

				}

				//if (AD1 <= 0x30B0) temp1 = ((double)_nMemory[23] * 10f / 32767f) + ((double)(_nMemory[23] - _nMemory[22]) * (double)(AD1 - 0x3080) / 97f * 10f / 32767f);
				//else temp1 = ((double)_nMemory[23] * 10f / 32767f) + ((double)(_nMemory[23] - _nMemory[22]) * (double)(AD1 - 0x314F + 49) / 97f * 10f / 32767f);

			}
			else		//  < 0x1980
			{
				//----------------------------------------------------------------------------------------------------------------
				// memory index = 0, from 0x1880<= data <0x1980 compensation point,					
				// memory 對應正向區間，有從最小值的 0x1980 開始校正起，形成每一個區間 cmpLeft ~ cmpRight 都有點，可以校正。
				// 但是，負向區間，卻是從 0x1880 開始，並沒有對 0x197F 校正，
				// 所以負向第一個區間，基準點直接假設為 0V // Gilbert 怪怪的
				//----------------------------------------------------------------------------------------------------------------	
				p = 0;
				cmpLeft = LDevConst.CMP_CENTER - LDevConst.A_REG_STEP;
				cmpRight = cmpLeft + LDevConst.A_REG_STEP;

				if (((LDevConst.CMP_CENTER - LDevConst.A_REG_STEP) <= readBackCMP) && p == 0)					// 負向第一個區間， 0x1880<= ~ <0x1980 ( 0x1880<= ~ <=0x197F )
				{
					segLeftMax = cmpLeft - LDevConst.HR_REG_CENTER + LDevConst.HR_REG_MAX;						// 0x18AF = 0x1880 - 0x0080 + 0x00AF
					segRightMin = cmpRight - LDevConst.HR_REG_CENTER + LDevConst.HR_REG_MIN;						// 0x194F = 0x1980 - 0x0080 + 0x004F

					//if (readBackCMP >= segRightMin)				// 0x194F<= ~ <0x1980 (<=0x197F), Right Segment
					if ((readBackCMP & 0xFF00) == (LDevConst.CMP_CENTER & 0xFF00))	// 0x1900 都還在左半面, 假設輸入的 AD1 遠超過表達的範圍，有循環重疊一小區段
					{
						ratio = (double)(LDevConst.CMP_CENTER - LDevConst.HR_REG_STEP - readBackCMP);			// 0x1980-0x01 - AD1= 0x197F - AD1
						volt = (double)_nMemory[memStart + p] * ratio / ((double)LDevConst.HR_REG_LENGTH);
						volt = volt * OP_Volt / 32767.0;
					}
					else											// 0x1880<= ~ <=0x18B0, Left Segment
					{
						//ratio = (double)(segLeftMax - AD1 + LDevConst.HR_REG_CENTER - LDevConst.HR_REG_MIN + 1);	// 0x18AF - AD1 + 1 + 0x80 - 0x01 - 0x4F + 1 = 0x18B0 - AD1 + 49
						ratio = (double)(LDevConst.CMP_CENTER - LDevConst.HR_REG_STEP - (readBackCMP + LDevConst.HR_REG_DIFF) + 1);  // Gilbert 多加的 1
						volt = (double)_nMemory[memStart + p] * ratio / ((double)LDevConst.HR_REG_LENGTH);
						volt = volt * OP_Volt / 32767.0;
					}
				}
				//------------------------------------------------------------------------
				// memory index = 1, from 0x1780<= data <0x1880 compensation point,					
				//------------------------------------------------------------------------
				p = 1;

				for (p = 1; cmpLeft > LDevConst.CMP_MIN; p++)													// (0x1780,0x1880) ==> 0x024F
				{
					cmpLeft = LDevConst.CMP_CENTER - LDevConst.A_REG_STEP * (p + 1);
					cmpRight = cmpLeft + LDevConst.A_REG_STEP;

					if ((cmpLeft <= readBackCMP) && (readBackCMP < cmpRight))									// 0x1780<= ~ <0x1880, 0x1680<= ~ <0x1780, ...
					{
						segLeftMax = cmpLeft - LDevConst.HR_REG_CENTER + LDevConst.HR_REG_MAX;					// 0x17AF = 0x1780 - 0x0080 + 0x00AF
						segRightMin = cmpRight - LDevConst.HR_REG_CENTER + LDevConst.HR_REG_MIN;					// 0x184F = 0x1880 - 0x0080 + 0x004F

						//if (readBackCMP >= segRightMin)						// 0x184F<= ~ <0x1880 (<=0x187F), Right Segment		
						if ((readBackCMP & 0xFF00) == (cmpRight & 0xFF00))		// 0x1800 都還在 Right Segment, 假設輸入的 AD1 遠超過表達的範圍，有循環重疊一小區段
						{
							ratio = (double)(cmpRight - LDevConst.HR_REG_STEP - readBackCMP);
							volt = (double)_nMemory[memStart + p - 1] + ((double)(_nMemory[memStart + p] - _nMemory[memStart + p - 1])) * ratio / ((double)LDevConst.HR_REG_LENGTH);
							volt = volt * OP_Volt / 32767.0;
						}
						else											// 0x1780<= ~ <=0x17B0, Left Segment
						{
							//ratio = (double)(segLeftMax - AD1 + LDevConst.HR_REG_CENTER - LDevConst.HR_REG_MIN + 1);				// 0x17AF - AD1 + 1 + 0x80 - 1 - 0x4F + 1 = 0x18B0 - AD1 + 49
							ratio = (double)(cmpRight - (readBackCMP + LDevConst.HR_REG_DIFF));
							volt = (double)_nMemory[memStart + p - 1] + ((double)(_nMemory[memStart + p] - _nMemory[memStart + p - 1])) * ratio / ((double)LDevConst.HR_REG_LENGTH);
							volt = volt * OP_Volt / 32767.0;
						}
						break;
					}


				}

				p = p - 1;	// using the final memmory segment;

				cmpRight = LDevConst.CMP_MIN - LDevConst.HR_REG_MIN + LDevConst.HR_REG_CENTER;
				cmpLeft = LDevConst.CMP_MIN - LDevConst.A_REG_STEP;									// 虛擬最後一個 Segment (0x0180,0x0280)
				if ((cmpLeft <= readBackCMP) && (readBackCMP < cmpRight))											// 0x024F<= ~ <0x0280, Right Segment
				{
					if ((readBackCMP & 0xFF00) == (cmpRight & 0xFF00))		// 0x1800 都還在 Right Segment, 假設輸入的 AD1 遠超過表達的範圍，有循環重疊一小區段
					{
						ratio = (double)(cmpRight - LDevConst.HR_REG_STEP - readBackCMP);
						volt = (double)_nMemory[memStart + p] + ((double)(_nMemory[memStart + p] - _nMemory[memStart + p - 1])) * ratio / ((double)LDevConst.HR_REG_LENGTH);
						volt = volt * OP_Volt / 32767.0;
					}
					else											// 0x1780<= ~ <=0x17B0, Left Segment
					{
						ratio = (double)(cmpRight - (readBackCMP + LDevConst.HR_REG_DIFF));
						volt = (double)_nMemory[memStart + p] + ((double)(_nMemory[memStart + p] - _nMemory[memStart + p - 1])) * ratio / ((double)LDevConst.HR_REG_LENGTH);
						volt = volt * OP_Volt / 32767.0;
					}
				}

				//if (AD1 >= 0x024F) temp1 = ((double)_nMemory[46] * 10f / 32767f) + ((double)(_nMemory[46] - _nMemory[45]) * (double)(0x027F - AD1) / 97f * 10f / 32767f);
				//else temp1 = ((double)_nMemory[46] * 10 / 32767f) + ((double)(_nMemory[46] - _nMemory[45]) * (double)(0x01B0 - AD1 + 49) / 97f * 10f / 32767f);

			}
			
			//----------------------------------------------------------------------------------------------
			// 以輸入期望的命令值當成最後正負的判斷，
			// 表示，計算過程的數值都是以無符號的運算為主，由 0x0000 ~ 0xFFFF, 表達一個正數數值的值。
			//----------------------------------------------------------------------------------------------

			if (CMP >= LDevConst.CMP_CENTER)
			{
				return volt;
			}
			else
			{
				return (0.0d - volt);
			}
		}

		private double calcVFP_ByCMP_Old(byte hwRange,int CMP ,UInt16 AD1)
		{
			double temp1;

			//------------------------------------------------------------
			// bHW=0x60 ,  V = 20V Range
			//------------------------------------------------------------
			if ((hwRange & 0x60) == 0x60)			// 20V Range
			{
				if (AD1 >= 0x1980)
				{
					#region +20V range

					if (AD1 < 0x1A80)
					{
						if (AD1 <= 0x19B0)
							temp1 = ((double)_nMemory[94] * 24f / 32767f) + ((double)(_nMemory[95] - _nMemory[94]) * (double)(AD1 - 0x1980) / 97f * 24f / 32767f); // 20160526_Terrell_初值加上比例；
						else
							temp1 = ((double)_nMemory[94] * 24f / 32767f) + ((double)(_nMemory[95] - _nMemory[94]) * (double)(AD1 - 0x1A4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x1B80)
					{
						if (AD1 <= 0x1AB0)
							temp1 = ((double)_nMemory[95] * 24f / 32767f) + ((double)(_nMemory[96] - _nMemory[95]) * (double)(AD1 - 0x1A80) / 97f * 24f / 32767f);
						else
							temp1 = ((double)_nMemory[95] * 24f / 32767f) + ((double)(_nMemory[96] - _nMemory[95]) * (double)(AD1 - 0x1B4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x1C80)
					{
						if (AD1 <= 0x1BB0) temp1 = ((double)_nMemory[96] * 24f / 32767f) + ((double)(_nMemory[97] - _nMemory[96]) * (double)(AD1 - 0x1B80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[96] * 24f / 32767f) + ((double)(_nMemory[97] - _nMemory[96]) * (double)(AD1 - 0x1C4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x1D80)
					{
						if (AD1 <= 0x1CB0) temp1 = ((double)_nMemory[97] * 24f / 32767f) + ((double)(_nMemory[98] - _nMemory[97]) * (double)(AD1 - 0x1C80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[97] * 24f / 32767f) + ((double)(_nMemory[98] - _nMemory[97]) * (double)(AD1 - 0x1D4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x1E80)
					{
						if (AD1 <= 0x1DB0) temp1 = ((double)_nMemory[98] * 24f / 32767f) + ((double)(_nMemory[99] - _nMemory[98]) * (double)(AD1 - 0x1D80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[98] * 24f / 32767f) + ((double)(_nMemory[99] - _nMemory[98]) * (double)(AD1 - 0x1E4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x1F80)
					{
						if (AD1 <= 0x1EB0) temp1 = ((double)_nMemory[99] * 24f / 32767f) + ((double)(_nMemory[100] - _nMemory[99]) * (double)(AD1 - 0x1E80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[99] * 24f / 32767f) + ((double)(_nMemory[100] - _nMemory[99]) * (double)(AD1 - 0x1F4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2080)
					{
						if (AD1 <= 0x1FB0) temp1 = ((double)_nMemory[100] * 24f / 32767f) + ((double)(_nMemory[101] - _nMemory[100]) * (double)(AD1 - 0x1F80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[100] * 24f / 32767f) + ((double)(_nMemory[101] - _nMemory[100]) * (double)(AD1 - 0x204F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2180)
					{
						if (AD1 <= 0x20B0) temp1 = ((double)_nMemory[101] * 24f / 32767f) + ((double)(_nMemory[102] - _nMemory[101]) * (double)(AD1 - 0x2080) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[101] * 24f / 32767f) + ((double)(_nMemory[102] - _nMemory[101]) * (double)(AD1 - 0x214F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2280)
					{
						if (AD1 <= 0x21B0) temp1 = ((double)_nMemory[102] * 24f / 32767f) + ((double)(_nMemory[103] - _nMemory[102]) * (double)(AD1 - 0x2180) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[102] * 24f / 32767f) + ((double)(_nMemory[103] - _nMemory[102]) * (double)(AD1 - 0x224F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2380)
					{
						if (AD1 <= 0x22B0) temp1 = ((double)_nMemory[103] * 24f / 32767f) + ((double)(_nMemory[104] - _nMemory[103]) * (double)(AD1 - 0x2280) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[103] * 24f / 32767f) + ((double)(_nMemory[104] - _nMemory[103]) * (double)(AD1 - 0x234F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2480)
					{
						if (AD1 <= 0x23B0) temp1 = ((double)_nMemory[104] * 24f / 32767f) + ((double)(_nMemory[105] - _nMemory[104]) * (double)(AD1 - 0x2380) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[104] * 24f / 32767f) + ((double)(_nMemory[105] - _nMemory[104]) * (double)(AD1 - 0x244F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2580)
					{
						if (AD1 <= 0x24B0) temp1 = ((double)_nMemory[105] * 24f / 32767f) + ((double)(_nMemory[106] - _nMemory[105]) * (double)(AD1 - 0x2480) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[105] * 24f / 32767f) + ((double)(_nMemory[106] - _nMemory[105]) * (double)(AD1 - 0x254F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2680)
					{
						if (AD1 <= 0x25B0) temp1 = ((double)_nMemory[106] * 24f / 32767f) + ((double)(_nMemory[107] - _nMemory[106]) * (double)(AD1 - 0x2580) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[106] * 24f / 32767f) + ((double)(_nMemory[107] - _nMemory[106]) * (double)(AD1 - 0x264F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2780)
					{
						if (AD1 <= 0x26B0) temp1 = ((double)_nMemory[107] * 24f / 32767f) + ((double)(_nMemory[108] - _nMemory[107]) * (double)(AD1 - 0x2680) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[107] * 24f / 32767f) + ((double)(_nMemory[108] - _nMemory[107]) * (double)(AD1 - 0x274F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2880)
					{
						if (AD1 <= 0x27B0) temp1 = ((double)_nMemory[108] * 24f / 32767f) + ((double)(_nMemory[109] - _nMemory[108]) * (double)(AD1 - 0x2780) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[108] * 24f / 32767f) + ((double)(_nMemory[109] - _nMemory[108]) * (double)(AD1 - 0x284F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2980)
					{
						if (AD1 <= 0x28B0) temp1 = ((double)_nMemory[109] * 24f / 32767f) + ((double)(_nMemory[110] - _nMemory[109]) * (double)(AD1 - 0x2880) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[109] * 24f / 32767f) + ((double)(_nMemory[110] - _nMemory[109]) * (double)(AD1 - 0x294F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2A80)
					{
						if (AD1 <= 0x29B0) temp1 = ((double)_nMemory[110] * 24f / 32767f) + ((double)(_nMemory[111] - _nMemory[110]) * (double)(AD1 - 0x2980) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[110] * 24f / 32767f) + ((double)(_nMemory[111] - _nMemory[110]) * (double)(AD1 - 0x2A4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2B80)
					{
						if (AD1 <= 0x2AB0) temp1 = ((double)_nMemory[111] * 24f / 32767f) + ((double)(_nMemory[112] - _nMemory[111]) * (double)(AD1 - 0x2A80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[111] * 24f / 32767f) + ((double)(_nMemory[112] - _nMemory[111]) * (double)(AD1 - 0x2B4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2C80)
					{
						if (AD1 <= 0x2BB0) temp1 = ((double)_nMemory[112] * 24f / 32767f) + ((double)(_nMemory[113] - _nMemory[112]) * (double)(AD1 - 0x2B80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[112] * 24f / 32767f) + ((double)(_nMemory[113] - _nMemory[112]) * (double)(AD1 - 0x2C4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2D80)
					{
						if (AD1 <= 0x2CB0) temp1 = ((double)_nMemory[113] * 24f / 32767f) + ((double)(_nMemory[114] - _nMemory[113]) * (double)(AD1 - 0x2C80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[113] * 24f / 32767f) + ((double)(_nMemory[114] - _nMemory[113]) * (double)(AD1 - 0x2D4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2E80)
					{
						if (AD1 <= 0x2DB0) temp1 = ((double)_nMemory[114] * 24f / 32767f) + ((double)(_nMemory[115] - _nMemory[114]) * (double)(AD1 - 0x2D80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[114] * 24f / 32767f) + ((double)(_nMemory[115] - _nMemory[114]) * (double)(AD1 - 0x2E4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 < 0x2F80)
					{
						if (AD1 <= 0x2EB0) temp1 = ((double)_nMemory[115] * 24f / 32767f) + ((double)(_nMemory[116] - _nMemory[115]) * (double)(AD1 - 0x2E80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[115] * 24f / 32767f) + ((double)(_nMemory[116] - _nMemory[115]) * (double)(AD1 - 0x2F4F + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 <= 0x3080)
					{
						if (AD1 <= 0x2FB0) temp1 = ((double)_nMemory[116] * 24f / 32767f) + ((double)(_nMemory[117] - _nMemory[116]) * (double)(AD1 - 0x2F80) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[116] * 24f / 32767f) + ((double)(_nMemory[117] - _nMemory[116]) * (double)(AD1 - 0x304F + 49) / 97f * 24f / 32767f);
					}
					else
					{
						if (AD1 <= 0x30B0) temp1 = ((double)_nMemory[117] * 24f / 32767f) + ((double)(_nMemory[117] - _nMemory[116]) * (double)(AD1 - 0x3080) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[117] * 24f / 32767f) + ((double)(_nMemory[117] - _nMemory[116]) * (double)(AD1 - 0x314F + 49) / 97f * 24f / 32767f);
					}
					#endregion +20V range
				}
				else
				{
					#region -20V range

					if (AD1 >= 0x1880) // 1880~197F
					{
						if (AD1 >= 0x194F)             //  20160526_Terrell_修改194F
							temp1 = (double)_nMemory[118] * (double)(0x197F - AD1) / 97f * 24f / 32767f;  // 20160526_Terrell_初值加上比例；區間改97
						else
							temp1 = (double)_nMemory[118] * (double)(0x18B0 - AD1 + 49) / 98f * 24f / 32767f;
					}
					else if (AD1 >= 0x1780) // 1780~187F 
					{
						if (AD1 >= 0x184F)
							temp1 = ((double)_nMemory[118] * 24f / 32767f) + ((double)(_nMemory[119] - _nMemory[118]) * (double)(0x187F - AD1) / 97f * 24f / 32767f);
						else
							temp1 = ((double)_nMemory[118] * 24f / 32767f) + ((double)(_nMemory[119] - _nMemory[118]) * (double)(0x17B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x168F)
					{
						if (AD1 >= 0x1740) temp1 = ((double)_nMemory[119] * 24f / 32767f) + ((double)(_nMemory[120] - _nMemory[119]) * (double)(0x177F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[119] * 24f / 32767f) + ((double)(_nMemory[120] - _nMemory[119]) * (double)(0x16B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x158F)
					{
						if (AD1 >= 0x1640) temp1 = ((double)_nMemory[120] * 24f / 32767f) + ((double)(_nMemory[121] - _nMemory[120]) * (double)(0x167F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[120] * 24f / 32767f) + ((double)(_nMemory[121] - _nMemory[120]) * (double)(0x15B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x148F)
					{
						if (AD1 >= 0x1540) temp1 = ((double)_nMemory[121] * 24f / 32767f) + ((double)(_nMemory[122] - _nMemory[121]) * (double)(0x157F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[121] * 24f / 32767f) + ((double)(_nMemory[122] - _nMemory[121]) * (double)(0x14B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x138F)
					{
						if (AD1 >= 0x1440) temp1 = ((double)_nMemory[122] * 24f / 32767f) + ((double)(_nMemory[123] - _nMemory[122]) * (double)(0x147F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[122] * 24f / 32767f) + ((double)(_nMemory[123] - _nMemory[122]) * (double)(0x13B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x128F)
					{
						if (AD1 >= 0x1340) temp1 = ((double)_nMemory[123] * 24f / 32767f) + ((double)(_nMemory[124] - _nMemory[123]) * (double)(0x137F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[123] * 24f / 32767f) + ((double)(_nMemory[124] - _nMemory[123]) * (double)(0x12B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x118F)
					{
						if (AD1 >= 0x1240) temp1 = ((double)_nMemory[124] * 24f / 32767f) + ((double)(_nMemory[125] - _nMemory[124]) * (double)(0x127F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[124] * 24f / 32767f) + ((double)(_nMemory[125] - _nMemory[124]) * (double)(0x11B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x108F)
					{
						if (AD1 >= 0x1140) temp1 = ((double)_nMemory[125] * 24f / 32767f) + ((double)(_nMemory[126] - _nMemory[125]) * (double)(0x117F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[125] * 24f / 32767f) + ((double)(_nMemory[126] - _nMemory[125]) * (double)(0x10B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x0F8F)
					{
						if (AD1 >= 0x1040) temp1 = ((double)_nMemory[126] * 24f / 32767f) + ((double)(_nMemory[127] - _nMemory[126]) * (double)(0x107F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[126] * 24f / 32767f) + ((double)(_nMemory[127] - _nMemory[126]) * (double)(0x0FB0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x0E8F)
					{
						if (AD1 >= 0x0F40) temp1 = ((double)_nMemory[127] * 24f / 32767f) + ((double)(_nMemory[128] - _nMemory[127]) * (double)(0x0F7F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[127] * 24f / 32767f) + ((double)(_nMemory[128] - _nMemory[127]) * (double)(0x0EB0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x0D8F)
					{
						if (AD1 >= 0x0E40) temp1 = ((double)_nMemory[128] * 24f / 32767f) + ((double)(_nMemory[129] - _nMemory[128]) * (double)(0x0E7F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[128] * 24f / 32767f) + ((double)(_nMemory[129] - _nMemory[128]) * (double)(0x0DB0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x0C8F)
					{
						if (AD1 >= 0x0D40) temp1 = ((double)_nMemory[129] * 24f / 32767f) + ((double)(_nMemory[130] - _nMemory[129]) * (double)(0x0D7F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[129] * 24f / 32767f) + ((double)(_nMemory[130] - _nMemory[129]) * (double)(0x0CB0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x0B8F)
					{
						if (AD1 >= 0x0C40) temp1 = ((double)_nMemory[130] * 24f / 32767f) + ((double)(_nMemory[131] - _nMemory[130]) * (double)(0x0C7F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[130] * 24f / 32767f) + ((double)(_nMemory[131] - _nMemory[130]) * (double)(0x0BB0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x0A8F)
					{
						if (AD1 >= 0x0B40) temp1 = ((double)_nMemory[131] * 24f / 32767f) + ((double)(_nMemory[132] - _nMemory[131]) * (double)(0x0B7F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[131] * 24f / 32767f) + ((double)(_nMemory[132] - _nMemory[131]) * (double)(0x0AB0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x098F)
					{
						if (AD1 >= 0x0A40) temp1 = ((double)_nMemory[132] * 24f / 32767f) + ((double)(_nMemory[133] - _nMemory[132]) * (double)(0x0A7F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[132] * 24f / 32767f) + ((double)(_nMemory[133] - _nMemory[132]) * (double)(0x09B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x088F)
					{
						if (AD1 >= 0x0940) temp1 = ((double)_nMemory[133] * 24f / 32767f) + ((double)(_nMemory[134] - _nMemory[133]) * (double)(0x097F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[133] * 24f / 32767f) + ((double)(_nMemory[134] - _nMemory[133]) * (double)(0x08B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x078F)
					{
						if (AD1 >= 0x0840) temp1 = ((double)_nMemory[134] * 24f / 32767f) + ((double)(_nMemory[135] - _nMemory[134]) * (double)(0x087F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[134] * 24f / 32767f) + ((double)(_nMemory[135] - _nMemory[134]) * (double)(0x07B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x068F)
					{
						if (AD1 >= 0x0740) temp1 = ((double)_nMemory[135] * 24f / 32767f) + ((double)(_nMemory[136] - _nMemory[135]) * (double)(0x077F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[135] * 24f / 32767f) + ((double)(_nMemory[136] - _nMemory[135]) * (double)(0x06B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x058F)
					{
						if (AD1 >= 0x0640) temp1 = ((double)_nMemory[136] * 24f / 32767f) + ((double)(_nMemory[137] - _nMemory[136]) * (double)(0x067F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[136] * 24f / 32767f) + ((double)(_nMemory[137] - _nMemory[136]) * (double)(0x05B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x048F)
					{
						if (AD1 >= 0x0540) temp1 = ((double)_nMemory[137] * 24f / 32767f) + ((double)(_nMemory[138] - _nMemory[137]) * (double)(0x057F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[137] * 24f / 32767f) + ((double)(_nMemory[138] - _nMemory[137]) * (double)(0x04B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x038F)
					{
						if (AD1 >= 0x0440) temp1 = ((double)_nMemory[138] * 24f / 32767f) + ((double)(_nMemory[139] - _nMemory[138]) * (double)(0x047F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[138] * 24f / 32767f) + ((double)(_nMemory[139] - _nMemory[138]) * (double)(0x03B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else if (AD1 >= 0x028F)
					{
						if (AD1 >= 0x0340) temp1 = ((double)_nMemory[139] * 24f / 32767f) + ((double)(_nMemory[140] - _nMemory[139]) * (double)(0x037F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[139] * 24f / 32767f) + ((double)(_nMemory[140] - _nMemory[139]) * (double)(0x02B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					else
					{
						if (AD1 >= 0x024F) temp1 = ((double)_nMemory[140] * 24f / 32767f) + ((double)(_nMemory[140] - _nMemory[139]) * (double)(0x027F - AD1) / 97f * 24f / 32767f);
						else temp1 = ((double)_nMemory[140] * 24f / 32767f) + ((double)(_nMemory[140] - _nMemory[139]) * (double)(0x01B0 - AD1 + 49) / 97f * 24f / 32767f);
					}
					#endregion -20V range
				}
			}
			//------------------------------------------------------------
			// bHW=0x40 ,  V = 6V Range
			//------------------------------------------------------------
			else if ((hwRange & 0x40) == 0x40)	// 6V Range
			{
				if (AD1 >= 0x1980)
				{
					#region +6V range

					if (AD1 < 0x1A80)
					{
						if (AD1 <= 0x19B0) temp1 = ((double)_nMemory[0] * 10f / 32767f) + ((double)(_nMemory[1] - _nMemory[0]) * (double)(AD1 - 0x1980) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[0] * 10f / 32767f) + ((double)(_nMemory[1] - _nMemory[0]) * (double)(AD1 - 0x1A4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x1B80)
					{
						if (AD1 <= 0x1AB0) temp1 = ((double)_nMemory[1] * 10f / 32767f) + ((double)(_nMemory[2] - _nMemory[1]) * (double)(AD1 - 0x1A80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[1] * 10f / 32767f) + ((double)(_nMemory[2] - _nMemory[1]) * (double)(AD1 - 0x1B4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x1C80)
					{
						if (AD1 <= 0x1BB0) temp1 = ((double)_nMemory[2] * 10f / 32767f) + ((double)(_nMemory[3] - _nMemory[2]) * (double)(AD1 - 0x1B80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[2] * 10f / 32767f) + ((double)(_nMemory[3] - _nMemory[2]) * (double)(AD1 - 0x1C4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x1D80)
					{
						if (AD1 <= 0x1CB0) temp1 = ((double)_nMemory[3] * 10f / 32767f) + ((double)(_nMemory[4] - _nMemory[3]) * (double)(AD1 - 0x1C80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[3] * 10f / 32767f) + ((double)(_nMemory[4] - _nMemory[3]) * (double)(AD1 - 0x1D4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x1E80)
					{
						if (AD1 <= 0x1DB0) temp1 = ((double)_nMemory[4] * 10f / 32767f) + ((double)(_nMemory[5] - _nMemory[4]) * (double)(AD1 - 0x1D80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[4] * 10f / 32767f) + ((double)(_nMemory[5] - _nMemory[4]) * (double)(AD1 - 0x1E4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x1F80)
					{
						if (AD1 <= 0x1EB0) temp1 = ((double)_nMemory[5] * 10f / 32767f) + ((double)(_nMemory[6] - _nMemory[5]) * (double)(AD1 - 0x1E80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[5] * 10f / 32767f) + ((double)(_nMemory[6] - _nMemory[5]) * (double)(AD1 - 0x1F4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2080)
					{
						if (AD1 <= 0x1FB0) temp1 = ((double)_nMemory[6] * 10f / 32767f) + ((double)(_nMemory[7] - _nMemory[6]) * (double)(AD1 - 0x1F80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[6] * 10f / 32767f) + ((double)(_nMemory[7] - _nMemory[6]) * (double)(AD1 - 0x204F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2180)
					{
						if (AD1 <= 0x20B0) temp1 = ((double)_nMemory[7] * 10f / 32767f) + ((double)(_nMemory[8] - _nMemory[7]) * (double)(AD1 - 0x2080) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[7] * 10f / 32767f) + ((double)(_nMemory[8] - _nMemory[7]) * (double)(AD1 - 0x214F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2280)
					{
						if (AD1 <= 0x21B0) temp1 = ((double)_nMemory[8] * 10f / 32767f) + ((double)(_nMemory[9] - _nMemory[8]) * (double)(AD1 - 0x2180) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[8] * 10f / 32767f) + ((double)(_nMemory[9] - _nMemory[8]) * (double)(AD1 - 0x224F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2380)
					{
						if (AD1 <= 0x22B0) temp1 = ((double)_nMemory[9] * 10f / 32767f) + ((double)(_nMemory[10] - _nMemory[9]) * (double)(AD1 - 0x2280) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[9] * 10f / 32767f) + ((double)(_nMemory[10] - _nMemory[9]) * (double)(AD1 - 0x234F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2480)
					{
						if (AD1 <= 0x23B0) temp1 = ((double)_nMemory[10] * 10f / 32767f) + ((double)(_nMemory[11] - _nMemory[10]) * (double)(AD1 - 0x2380) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[10] * 10f / 32767f) + ((double)(_nMemory[11] - _nMemory[10]) * (double)(AD1 - 0x244F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2580)
					{
						if (AD1 <= 0x24B0) temp1 = ((double)_nMemory[11] * 10f / 32767f) + ((double)(_nMemory[12] - _nMemory[11]) * (double)(AD1 - 0x2480) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[11] * 10f / 32767f) + ((double)(_nMemory[12] - _nMemory[11]) * (double)(AD1 - 0x254F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2680)
					{
						if (AD1 <= 0x25B0) temp1 = ((double)_nMemory[12] * 10f / 32767f) + ((double)(_nMemory[13] - _nMemory[12]) * (double)(AD1 - 0x2580) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[12] * 10f / 32767f) + ((double)(_nMemory[13] - _nMemory[12]) * (double)(AD1 - 0x264F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2780)
					{
						if (AD1 <= 0x26B0) temp1 = ((double)_nMemory[13] * 10f / 32767f) + ((double)(_nMemory[14] - _nMemory[13]) * (double)(AD1 - 0x2680) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[13] * 10f / 32767f) + ((double)(_nMemory[14] - _nMemory[13]) * (double)(AD1 - 0x274F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2880)
					{
						if (AD1 <= 0x27B0) temp1 = ((double)_nMemory[14] * 10f / 32767f) + ((double)(_nMemory[15] - _nMemory[14]) * (double)(AD1 - 0x2780) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[14] * 10f / 32767f) + ((double)(_nMemory[15] - _nMemory[14]) * (double)(AD1 - 0x284F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2980)
					{
						if (AD1 <= 0x28B0) temp1 = ((double)_nMemory[15] * 10f / 32767f) + ((double)(_nMemory[16] - _nMemory[15]) * (double)(AD1 - 0x2880) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[15] * 10f / 32767f) + ((double)(_nMemory[16] - _nMemory[15]) * (double)(AD1 - 0x294F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2A80)
					{
						if (AD1 <= 0x29B0) temp1 = ((double)_nMemory[16] * 10f / 32767f) + ((double)(_nMemory[17] - _nMemory[16]) * (double)(AD1 - 0x2980) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[16] * 10f / 32767f) + ((double)(_nMemory[17] - _nMemory[16]) * (double)(AD1 - 0x2A4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2B80)
					{
						if (AD1 <= 0x2AB0) temp1 = ((double)_nMemory[17] * 10f / 32767f) + ((double)(_nMemory[18] - _nMemory[17]) * (double)(AD1 - 0x2A80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[17] * 10f / 32767f) + ((double)(_nMemory[18] - _nMemory[17]) * (double)(AD1 - 0x2B4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2C80)
					{
						if (AD1 <= 0x2BB0) temp1 = ((double)_nMemory[18] * 10f / 32767f) + ((double)(_nMemory[19] - _nMemory[18]) * (double)(AD1 - 0x2B80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[18] * 10f / 32767f) + ((double)(_nMemory[19] - _nMemory[18]) * (double)(AD1 - 0x2C4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2D80)
					{
						if (AD1 <= 0x2CB0) temp1 = ((double)_nMemory[19] * 10f / 32767f) + ((double)(_nMemory[20] - _nMemory[19]) * (double)(AD1 - 0x2C80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[19] * 10f / 32767f) + ((double)(_nMemory[20] - _nMemory[19]) * (double)(AD1 - 0x2D4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2E80)
					{
						if (AD1 <= 0x2DB0) temp1 = ((double)_nMemory[20] * 10f / 32767f) + ((double)(_nMemory[21] - _nMemory[20]) * (double)(AD1 - 0x2D80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[20] * 10f / 32767f) + ((double)(_nMemory[21] - _nMemory[20]) * (double)(AD1 - 0x2E4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 < 0x2F80)
					{
						if (AD1 <= 0x2EB0) temp1 = ((double)_nMemory[21] * 10f / 32767f) + ((double)(_nMemory[22] - _nMemory[21]) * (double)(AD1 - 0x2E80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[21] * 10f / 32767f) + ((double)(_nMemory[22] - _nMemory[21]) * (double)(AD1 - 0x2F4F + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 <= 0x3080)
					{
						if (AD1 <= 0x2FB0) temp1 = ((double)_nMemory[22] * 10f / 32767f) + ((double)(_nMemory[23] - _nMemory[22]) * (double)(AD1 - 0x2F80) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[22] * 10f / 32767f) + ((double)(_nMemory[23] - _nMemory[22]) * (double)(AD1 - 0x304F + 49) / 97f * 10f / 32767f);
					}
					else
					{
						if (AD1 <= 0x30B0) temp1 = ((double)_nMemory[23] * 10f / 32767f) + ((double)(_nMemory[23] - _nMemory[22]) * (double)(AD1 - 0x3080) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[23] * 10f / 32767f) + ((double)(_nMemory[23] - _nMemory[22]) * (double)(AD1 - 0x314F + 49) / 97f * 10f / 32767f);
					}
					#endregion +6V range
				}
				else
				{
					#region -6V range
					if (AD1 >= 0x1880) // 1880~197F
					{
						if (AD1 >= 0x194F) temp1 = (double)_nMemory[24] * (double)(0x197F - AD1) / 97f * 10f / 32767f; //20160526_Terrell_修改4F； +1；98區間
						else temp1 = (double)_nMemory[24] * (double)(0x18B0 - AD1 + 49) / 97f * 10f / 32767f;
					}
					else if (AD1 >= 0x1780) // 1780~187F 
					{
						if (AD1 >= 0x184F) temp1 = ((double)_nMemory[24] * 10f / 32767f) + ((double)(_nMemory[25] - _nMemory[24]) * (double)(0x187F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[24] * 10f / 32767f) + ((double)(_nMemory[25] - _nMemory[24]) * (double)(0x17B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x1680)
					{
						if (AD1 >= 0x174F) temp1 = ((double)_nMemory[25] * 10f / 32767f) + ((double)(_nMemory[26] - _nMemory[25]) * (double)(0x177F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[25] * 10f / 32767f) + ((double)(_nMemory[26] - _nMemory[25]) * (double)(0x16B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x1580)
					{
						if (AD1 >= 0x164F) temp1 = ((double)_nMemory[26] * 10f / 32767f) + ((double)(_nMemory[27] - _nMemory[26]) * (double)(0x167F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[26] * 10f / 32767f) + ((double)(_nMemory[27] - _nMemory[26]) * (double)(0x15B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x1480)
					{
						if (AD1 >= 0x154F) temp1 = ((double)_nMemory[27] * 10f / 32767f) + ((double)(_nMemory[28] - _nMemory[27]) * (double)(0x157F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[27] * 10f / 32767f) + ((double)(_nMemory[28] - _nMemory[27]) * (double)(0x14B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x1380)
					{
						if (AD1 >= 0x144F) temp1 = ((double)_nMemory[28] * 10f / 32767f) + ((double)(_nMemory[29] - _nMemory[28]) * (double)(0x147F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[28] * 10f / 32767f) + ((double)(_nMemory[29] - _nMemory[28]) * (double)(0x13B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x1280)
					{
						if (AD1 >= 0x134F) temp1 = ((double)_nMemory[29] * 10f / 32767f) + ((double)(_nMemory[30] - _nMemory[29]) * (double)(0x137F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[29] * 10f / 32767f) + ((double)(_nMemory[30] - _nMemory[29]) * (double)(0x12B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x1180)
					{
						if (AD1 >= 0x124F) temp1 = ((double)_nMemory[30] * 10f / 32767f) + ((double)(_nMemory[31] - _nMemory[30]) * (double)(0x127F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[30] * 10f / 32767f) + ((double)(_nMemory[31] - _nMemory[30]) * (double)(0x11B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x1080)
					{
						if (AD1 >= 0x114F) temp1 = ((double)_nMemory[31] * 10f / 32767f) + ((double)(_nMemory[32] - _nMemory[31]) * (double)(0x117F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[31] * 10f / 32767f) + ((double)(_nMemory[32] - _nMemory[31]) * (double)(0x10B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0F80)
					{
						if (AD1 >= 0x104F) temp1 = ((double)_nMemory[32] * 10f / 32767f) + ((double)(_nMemory[33] - _nMemory[32]) * (double)(0x107F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[32] * 10f / 32767f) + ((double)(_nMemory[33] - _nMemory[32]) * (double)(0x0FB0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0E80)
					{
						if (AD1 >= 0x0F4F) temp1 = ((double)_nMemory[33] * 10f / 32767f) + ((double)(_nMemory[34] - _nMemory[33]) * (double)(0x0F7F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[33] * 10f / 32767f) + ((double)(_nMemory[34] - _nMemory[33]) * (double)(0x0EB0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0D80)
					{
						if (AD1 >= 0x0E4F) temp1 = ((double)_nMemory[34] * 10f / 32767f) + ((double)(_nMemory[35] - _nMemory[34]) * (double)(0x0E7F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[34] * 10f / 32767f) + ((double)(_nMemory[35] - _nMemory[34]) * (double)(0x0DB0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0C80)
					{
						if (AD1 >= 0x0D4F) temp1 = ((double)_nMemory[35] * 10f / 32767f) + ((double)(_nMemory[36] - _nMemory[35]) * (double)(0x0D7F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[35] * 10f / 32767f) + ((double)(_nMemory[36] - _nMemory[35]) * (double)(0x0CB0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0B80)
					{
						if (AD1 >= 0x0C4F) temp1 = ((double)_nMemory[36] * 10f / 32767f) + ((double)(_nMemory[37] - _nMemory[36]) * (double)(0x0C7F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[36] * 10f / 32767f) + ((double)(_nMemory[37] - _nMemory[36]) * (double)(0x0BB0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0A80)
					{
						if (AD1 >= 0x0B4F) temp1 = ((double)_nMemory[37] * 10f / 32767f) + ((double)(_nMemory[38] - _nMemory[37]) * (double)(0x0B7F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[37] * 10f / 32767f) + ((double)(_nMemory[38] - _nMemory[37]) * (double)(0x0AB0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0980)
					{
						if (AD1 >= 0x0A4F) temp1 = ((double)_nMemory[38] * 10f / 32767f) + ((double)(_nMemory[39] - _nMemory[38]) * (double)(0x0A7F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[38] * 10f / 32767f) + ((double)(_nMemory[39] - _nMemory[38]) * (double)(0x09B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0880)
					{
						if (AD1 >= 0x094F) temp1 = ((double)_nMemory[39] * 10f / 32767f) + ((double)(_nMemory[40] - _nMemory[39]) * (double)(0x097F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[39] * 10f / 32767f) + ((double)(_nMemory[40] - _nMemory[39]) * (double)(0x08B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0780)
					{
						if (AD1 >= 0x084F) temp1 = ((double)_nMemory[40] * 10f / 32767f) + ((double)(_nMemory[41] - _nMemory[40]) * (double)(0x087F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[40] * 10f / 32767f) + ((double)(_nMemory[41] - _nMemory[40]) * (double)(0x07B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0680)
					{
						if (AD1 >= 0x074F) temp1 = ((double)_nMemory[41] * 10f / 32767f) + ((double)(_nMemory[42] - _nMemory[41]) * (double)(0x077F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[41] * 10f / 32767f) + ((double)(_nMemory[42] - _nMemory[41]) * (double)(0x06B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0580)
					{
						if (AD1 >= 0x064F) temp1 = ((double)_nMemory[42] * 10f / 32767f) + ((double)(_nMemory[43] - _nMemory[42]) * (double)(0x067F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[42] * 10f / 32767f) + ((double)(_nMemory[43] - _nMemory[42]) * (double)(0x05B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0480)
					{
						if (AD1 >= 0x054F) temp1 = ((double)_nMemory[43] * 10f / 32767f) + ((double)(_nMemory[44] - _nMemory[43]) * (double)(0x057F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[43] * 10f / 32767f) + ((double)(_nMemory[44] - _nMemory[43]) * (double)(0x04B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0380)
					{
						if (AD1 >= 0x044F) temp1 = ((double)_nMemory[44] * 10f / 32767f) + ((double)(_nMemory[45] - _nMemory[44]) * (double)(0x047F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[44] * 10f / 32767f) + ((double)(_nMemory[45] - _nMemory[44]) * (double)(0x03B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else if (AD1 >= 0x0280)
					{
						if (AD1 >= 0x034F) temp1 = ((double)_nMemory[45] * 10f / 32767f) + ((double)(_nMemory[46] - _nMemory[45]) * (double)(0x037F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[45] * 10f / 32767f) + ((double)(_nMemory[46] - _nMemory[45]) * (double)(0x02B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					else
					{
						if (AD1 >= 0x024F) temp1 = ((double)_nMemory[46] * 10f / 32767f) + ((double)(_nMemory[46] - _nMemory[45]) * (double)(0x027F - AD1) / 97f * 10f / 32767f);
						else temp1 = ((double)_nMemory[46] * 10 / 32767f) + ((double)(_nMemory[46] - _nMemory[45]) * (double)(0x01B0 - AD1 + 49) / 97f * 10f / 32767f);
					}
					#endregion -6V range
				}
			}
			//------------------------------------------------------------
			// bHW=0xA0 ,  V = 200V Range		// Gilbert , What = bits
			//------------------------------------------------------------
			else if ((hwRange & 0xA0) == 0xA0)	// 200V Range
			{
				if (AD1 >= 0x1980)
				{
					#region +200V range

					if (AD1 < 0x1A80)
					{
						if (AD1 <= 0x19B0) temp1 = ((double)_nMemory[188] * 216f / 32767f) + ((double)(_nMemory[189] - _nMemory[188]) * (double)(AD1 - 0x1980) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[188] * 216f / 32767f) + ((double)(_nMemory[189] - _nMemory[188]) * (double)(AD1 - 0x1A4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x1B80)
					{
						if (AD1 <= 0x1AB0) temp1 = ((double)_nMemory[189] * 216f / 32767f) + ((double)(_nMemory[190] - _nMemory[189]) * (double)(AD1 - 0x1A80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[189] * 216f / 32767f) + ((double)(_nMemory[190] - _nMemory[189]) * (double)(AD1 - 0x1B4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x1C80)
					{
						if (AD1 <= 0x1BB0) temp1 = ((double)_nMemory[190] * 216f / 32767f) + ((double)(_nMemory[191] - _nMemory[190]) * (double)(AD1 - 0x1B80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[190] * 216f / 32767f) + ((double)(_nMemory[191] - _nMemory[190]) * (double)(AD1 - 0x1C4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x1D80)
					{
						if (AD1 <= 0x1CB0) temp1 = ((double)_nMemory[191] * 216f / 32767f) + ((double)(_nMemory[192] - _nMemory[191]) * (double)(AD1 - 0x1C80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[191] * 216f / 32767f) + ((double)(_nMemory[192] - _nMemory[191]) * (double)(AD1 - 0x1D4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x1E80)
					{
						if (AD1 <= 0x1DB0) temp1 = ((double)_nMemory[192] * 216f / 32767f) + ((double)(_nMemory[193] - _nMemory[192]) * (double)(AD1 - 0x1D80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[192] * 216f / 32767f) + ((double)(_nMemory[193] - _nMemory[192]) * (double)(AD1 - 0x1E4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x1F80)
					{
						if (AD1 <= 0x1EB0) temp1 = ((double)_nMemory[193] * 216f / 32767f) + ((double)(_nMemory[194] - _nMemory[193]) * (double)(AD1 - 0x1E80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[193] * 216f / 32767f) + ((double)(_nMemory[194] - _nMemory[193]) * (double)(AD1 - 0x1F4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2080)
					{
						if (AD1 <= 0x1FB0) temp1 = ((double)_nMemory[194] * 216f / 32767f) + ((double)(_nMemory[195] - _nMemory[194]) * (double)(AD1 - 0x1F80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[194] * 216f / 32767f) + ((double)(_nMemory[195] - _nMemory[194]) * (double)(AD1 - 0x204F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2180)
					{
						if (AD1 <= 0x20B0) temp1 = ((double)_nMemory[195] * 216f / 32767f) + ((double)(_nMemory[196] - _nMemory[195]) * (double)(AD1 - 0x2080) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[195] * 216f / 32767f) + ((double)(_nMemory[196] - _nMemory[195]) * (double)(AD1 - 0x214F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2280)
					{
						if (AD1 <= 0x21B0) temp1 = ((double)_nMemory[196] * 216f / 32767f) + ((double)(_nMemory[197] - _nMemory[196]) * (double)(AD1 - 0x2180) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[196] * 216f / 32767f) + ((double)(_nMemory[197] - _nMemory[196]) * (double)(AD1 - 0x224F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2380)
					{
						if (AD1 <= 0x22B0) temp1 = ((double)_nMemory[197] * 216f / 32767f) + ((double)(_nMemory[198] - _nMemory[197]) * (double)(AD1 - 0x2280) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[197] * 216f / 32767f) + ((double)(_nMemory[198] - _nMemory[197]) * (double)(AD1 - 0x234F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2480)
					{
						if (AD1 <= 0x23B0) temp1 = ((double)_nMemory[198] * 216f / 32767f) + ((double)(_nMemory[199] - _nMemory[197]) * (double)(AD1 - 0x2380) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[198] * 216f / 32767f) + ((double)(_nMemory[199] - _nMemory[198]) * (double)(AD1 - 0x244F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2580)
					{
						if (AD1 <= 0x24B0) temp1 = ((double)_nMemory[199] * 216f / 32767f) + ((double)(_nMemory[200] - _nMemory[199]) * (double)(AD1 - 0x2480) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[199] * 216f / 32767f) + ((double)(_nMemory[200] - _nMemory[199]) * (double)(AD1 - 0x254F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2680)
					{
						if (AD1 <= 0x25B0) temp1 = ((double)_nMemory[200] * 216f / 32767f) + ((double)(_nMemory[201] - _nMemory[200]) * (double)(AD1 - 0x2580) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[200] * 216f / 32767f) + ((double)(_nMemory[201] - _nMemory[200]) * (double)(AD1 - 0x264F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2780)
					{
						if (AD1 <= 0x26B0) temp1 = ((double)_nMemory[201] * 216f / 32767f) + ((double)(_nMemory[202] - _nMemory[201]) * (double)(AD1 - 0x2680) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[201] * 216f / 32767f) + ((double)(_nMemory[202] - _nMemory[201]) * (double)(AD1 - 0x274F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2880)
					{
						if (AD1 <= 0x27B0) temp1 = ((double)_nMemory[202] * 216f / 32767f) + ((double)(_nMemory[203] - _nMemory[202]) * (double)(AD1 - 0x2780) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[202] * 216f / 32767f) + ((double)(_nMemory[203] - _nMemory[202]) * (double)(AD1 - 0x284F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2980)
					{
						if (AD1 <= 0x28B0) temp1 = ((double)_nMemory[203] * 216f / 32767f) + ((double)(_nMemory[204] - _nMemory[203]) * (double)(AD1 - 0x2880) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[203] * 216f / 32767f) + ((double)(_nMemory[204] - _nMemory[203]) * (double)(AD1 - 0x294F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2A80)
					{
						if (AD1 <= 0x29B0) temp1 = ((double)_nMemory[204] * 216f / 32767f) + ((double)(_nMemory[205] - _nMemory[204]) * (double)(AD1 - 0x2980) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[204] * 216f / 32767f) + ((double)(_nMemory[205] - _nMemory[204]) * (double)(AD1 - 0x2A4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2B80)
					{
						if (AD1 <= 0x2AB0) temp1 = ((double)_nMemory[205] * 216f / 32767f) + ((double)(_nMemory[206] - _nMemory[205]) * (double)(AD1 - 0x2A80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[205] * 216f / 32767f) + ((double)(_nMemory[206] - _nMemory[205]) * (double)(AD1 - 0x2B4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2C80)
					{
						if (AD1 <= 0x2BB0) temp1 = ((double)_nMemory[206] * 216f / 32767f) + ((double)(_nMemory[207] - _nMemory[206]) * (double)(AD1 - 0x2B80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[206] * 216f / 32767f) + ((double)(_nMemory[207] - _nMemory[206]) * (double)(AD1 - 0x2C4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2D80)
					{
						if (AD1 <= 0x2CB0) temp1 = ((double)_nMemory[207] * 216f / 32767f) + ((double)(_nMemory[208] - _nMemory[207]) * (double)(AD1 - 0x2C80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[207] * 216f / 32767f) + ((double)(_nMemory[208] - _nMemory[207]) * (double)(AD1 - 0x2D4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2E80)
					{
						if (AD1 <= 0x2DB0) temp1 = ((double)_nMemory[208] * 216f / 32767f) + ((double)(_nMemory[209] - _nMemory[208]) * (double)(AD1 - 0x2D80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[208] * 216f / 32767f) + ((double)(_nMemory[209] - _nMemory[208]) * (double)(AD1 - 0x2E4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 < 0x2F80)
					{
						if (AD1 <= 0x2EB0) temp1 = ((double)_nMemory[209] * 216f / 32767f) + ((double)(_nMemory[210] - _nMemory[209]) * (double)(AD1 - 0x2E80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[209] * 216f / 32767f) + ((double)(_nMemory[210] - _nMemory[209]) * (double)(AD1 - 0x2F4F + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 <= 0x3080)
					{
						if (AD1 <= 0x2FB0) temp1 = ((double)_nMemory[210] * 216f / 32767f) + ((double)(_nMemory[211] - _nMemory[210]) * (double)(AD1 - 0x2F80) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[210] * 216f / 32767f) + ((double)(_nMemory[211] - _nMemory[210]) * (double)(AD1 - 0x304F + 49) / 97f * 216f / 32767f);
					}
					else
					{
						if (AD1 <= 0x30B0) temp1 = ((double)_nMemory[211] * 216f / 32767f) + ((double)(_nMemory[211] - _nMemory[210]) * (double)(AD1 - 0x3080) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[211] * 216f / 32767f) + ((double)(_nMemory[211] - _nMemory[210]) * (double)(AD1 - 0x314F + 49) / 97f * 216f / 32767f);
					}
					#endregion +200V range
				}
				else
				{
					#region -200V range

					if (AD1 >= 0x1880) // 1880~197F
					{
						if (AD1 >= 0x194F) temp1 = (double)_nMemory[212] * (double)(0x197F - AD1) / 97f * 216f / 32767f;
						else temp1 = (double)_nMemory[212] * (double)(0x18B0 - AD1 + 49) / 97f * 216f / 32767f;
					}
					else if (AD1 >= 0x1780) // 1780~187F 
					{
						if (AD1 >= 0x184F) temp1 = ((double)_nMemory[212] * 216f / 32767f) + ((double)(_nMemory[213] - _nMemory[212]) * (double)(0x187F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[212] * 216f / 32767f) + ((double)(_nMemory[213] - _nMemory[212]) * (double)(0x17B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x1680)
					{
						if (AD1 >= 0x174F) temp1 = ((double)_nMemory[213] * 216f / 32767f) + ((double)(_nMemory[214] - _nMemory[213]) * (double)(0x177F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[213] * 216f / 32767f) + ((double)(_nMemory[214] - _nMemory[213]) * (double)(0x16B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x1580)
					{
						if (AD1 >= 0x164F) temp1 = ((double)_nMemory[214] * 216f / 32767f) + ((double)(_nMemory[215] - _nMemory[214]) * (double)(0x167F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[214] * 216f / 32767f) + ((double)(_nMemory[215] - _nMemory[214]) * (double)(0x15B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x1480)
					{
						if (AD1 >= 0x154F) temp1 = ((double)_nMemory[215] * 216f / 32767f) + ((double)(_nMemory[216] - _nMemory[215]) * (double)(0x157F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[215] * 216f / 32767f) + ((double)(_nMemory[216] - _nMemory[215]) * (double)(0x14B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x1380)
					{
						if (AD1 >= 0x144F) temp1 = ((double)_nMemory[216] * 216f / 32767f) + ((double)(_nMemory[217] - _nMemory[216]) * (double)(0x147F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[216] * 216f / 32767f) + ((double)(_nMemory[217] - _nMemory[216]) * (double)(0x13B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x1280)
					{
						if (AD1 >= 0x134F) temp1 = ((double)_nMemory[217] * 216f / 32767f) + ((double)(_nMemory[218] - _nMemory[217]) * (double)(0x137F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[217] * 216f / 32767f) + ((double)(_nMemory[218] - _nMemory[217]) * (double)(0x12B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x1180)
					{
						if (AD1 >= 0x124F) temp1 = ((double)_nMemory[218] * 216f / 32767f) + ((double)(_nMemory[219] - _nMemory[218]) * (double)(0x127F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[218] * 216f / 32767f) + ((double)(_nMemory[219] - _nMemory[218]) * (double)(0x11B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x1080)
					{
						if (AD1 >= 0x114F) temp1 = ((double)_nMemory[219] * 216f / 32767f) + ((double)(_nMemory[220] - _nMemory[219]) * (double)(0x117F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[219] * 216f / 32767f) + ((double)(_nMemory[220] - _nMemory[219]) * (double)(0x10B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0F80)
					{
						if (AD1 >= 0x104F) temp1 = ((double)_nMemory[220] * 216f / 32767f) + ((double)(_nMemory[221] - _nMemory[220]) * (double)(0x107F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[220] * 216f / 32767f) + ((double)(_nMemory[221] - _nMemory[220]) * (double)(0x0FB0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0E80)
					{
						if (AD1 >= 0x0F4F) temp1 = ((double)_nMemory[221] * 216f / 32767f) + ((double)(_nMemory[222] - _nMemory[221]) * (double)(0x0F7F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[221] * 216f / 32767f) + ((double)(_nMemory[222] - _nMemory[221]) * (double)(0x0EB0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0D80)
					{
						if (AD1 >= 0x0E4F) temp1 = ((double)_nMemory[222] * 216f / 32767f) + ((double)(_nMemory[223] - _nMemory[222]) * (double)(0x0E7F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[222] * 216f / 32767f) + ((double)(_nMemory[223] - _nMemory[222]) * (double)(0x0DB0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0C80)
					{
						if (AD1 >= 0x0D4F) temp1 = ((double)_nMemory[223] * 216f / 32767f) + ((double)(_nMemory[224] - _nMemory[223]) * (double)(0x0D7F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[223] * 216f / 32767f) + ((double)(_nMemory[224] - _nMemory[223]) * (double)(0x0CB0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0B80)
					{
						if (AD1 >= 0x0C4F) temp1 = ((double)_nMemory[224] * 216f / 32767f) + ((double)(_nMemory[225] - _nMemory[224]) * (double)(0x0C7F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[224] * 216f / 32767f) + ((double)(_nMemory[225] - _nMemory[224]) * (double)(0x0BB0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0A80)
					{
						if (AD1 >= 0x0B4F) temp1 = ((double)_nMemory[225] * 216f / 32767f) + ((double)(_nMemory[226] - _nMemory[225]) * (double)(0x0B7F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[225] * 216f / 32767f) + ((double)(_nMemory[226] - _nMemory[225]) * (double)(0x0AB0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0980)
					{
						if (AD1 >= 0x0A4F) temp1 = ((double)_nMemory[226] * 216f / 32767f) + ((double)(_nMemory[227] - _nMemory[226]) * (double)(0x0A7F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[226] * 216f / 32767f) + ((double)(_nMemory[227] - _nMemory[226]) * (double)(0x09B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0880)
					{
						if (AD1 >= 0x094F) temp1 = ((double)_nMemory[227] * 216f / 32767f) + ((double)(_nMemory[228] - _nMemory[227]) * (double)(0x097F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[227] * 216f / 32767f) + ((double)(_nMemory[228] - _nMemory[227]) * (double)(0x08B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0780)
					{
						if (AD1 >= 0x084F) temp1 = ((double)_nMemory[228] * 216f / 32767f) + ((double)(_nMemory[229] - _nMemory[228]) * (double)(0x087F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[228] * 216f / 32767f) + ((double)(_nMemory[229] - _nMemory[228]) * (double)(0x07B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0680)
					{
						if (AD1 >= 0x074F) temp1 = ((double)_nMemory[229] * 216f / 32767f) + ((double)(_nMemory[230] - _nMemory[229]) * (double)(0x077F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[229] * 216f / 32767f) + ((double)(_nMemory[230] - _nMemory[229]) * (double)(0x06B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0580)
					{
						if (AD1 >= 0x064F) temp1 = ((double)_nMemory[230] * 216f / 32767f) + ((double)(_nMemory[231] - _nMemory[230]) * (double)(0x067F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[230] * 216f / 32767f) + ((double)(_nMemory[231] - _nMemory[230]) * (double)(0x05B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0480)
					{
						if (AD1 >= 0x054F) temp1 = ((double)_nMemory[231] * 216f / 32767f) + ((double)(_nMemory[232] - _nMemory[231]) * (double)(0x057F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[231] * 216f / 32767f) + ((double)(_nMemory[232] - _nMemory[231]) * (double)(0x04B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0380)
					{
						if (AD1 >= 0x044F) temp1 = ((double)_nMemory[232] * 216f / 32767f) + ((double)(_nMemory[233] - _nMemory[232]) * (double)(0x047F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[232] * 216f / 32767f) + ((double)(_nMemory[233] - _nMemory[232]) * (double)(0x03B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else if (AD1 >= 0x0280)
					{
						if (AD1 >= 0x034F) temp1 = ((double)_nMemory[233] * 216f / 32767f) + ((double)(_nMemory[234] - _nMemory[233]) * (double)(0x037F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[233] * 216f / 32767f) + ((double)(_nMemory[234] - _nMemory[233]) * (double)(0x02B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					else
					{
						if (AD1 >= 0x024F) temp1 = ((double)_nMemory[234] * 216f / 32767f) + ((double)(_nMemory[234] - _nMemory[233]) * (double)(0x027F - AD1) / 97f * 216f / 32767f);
						else temp1 = ((double)_nMemory[234] * 216f / 32767f) + ((double)(_nMemory[234] - _nMemory[233]) * (double)(0x01B0 - AD1 + 49) / 97f * 216f / 32767f);
					}
					#endregion -200V range ===========================
				}
			}
			else
			{
				temp1 = 0.0d;
			}

			if (CMP >= 0x1980)
			{
				return temp1;
			}
			else
			{
				return (0.0 - temp1);
			}
			
		}

		//private double calcVFP_ByAD1_2Wire(byte hwRange, int CMP, UInt16 AD1) // Gilbert 不應該存在
		//{
		//    double volt = 0.0d;
		//    double V1 = 0.0d, V2 = 0.0d, Vqua = 10.0d;
		//    int VF_StartIndex = 0,VF_EndIndex = 1 ,VS_StartIndex = 0 , VS_EndIndex = 1;

		//    hwRange = (byte)(hwRange & 0xE0);

		//    if ((hwRange & 0x40) == 0x40)		// -6V ~ +6V Range
		//    {
		//        Vqua = LDevConst.OP6VRangeOut;
		//        if (CMP >= 0x1980)				// +6VRanage, 23 point
		//        {
		//            VF_StartIndex = LDevConst.memTableVFP[0];
		//            VF_EndIndex = LDevConst.memTableVFP[1];
		//            VS_StartIndex = LDevConst.memTableVSP[0];
		//            VS_EndIndex = LDevConst.memTableVSP[1];
		//        }
		//        else							// -6VRange, 22 point
		//        {
		//            VF_StartIndex = LDevConst.memTableVFP[2];
		//            VF_EndIndex = LDevConst.memTableVFP[3];
		//            VS_StartIndex = LDevConst.memTableVSP[2];
		//            VS_EndIndex = LDevConst.memTableVSP[3];

		//        }
		//    }
		//    else if ((hwRange & 0x60) == 0x60)	// -20V ~ +20V Range
		//    {
		//        Vqua = LDevConst.OP20VRangeOut;
		//        if (CMP >= 0x1980)				// +20VRange, 23 point
		//        {
		//            VF_StartIndex = LDevConst.memTableVFP[4];
		//            VF_EndIndex = LDevConst.memTableVFP[5];
		//            VS_StartIndex = LDevConst.memTableVSP[4];
		//            VS_EndIndex = LDevConst.memTableVSP[5];
		//        }
		//        else							// -20VRange, 22 point
		//        {
		//            VF_StartIndex = LDevConst.memTableVFP[6];
		//            VF_EndIndex = LDevConst.memTableVFP[7];
		//            VS_StartIndex = LDevConst.memTableVSP[6];
		//            VS_EndIndex = LDevConst.memTableVSP[7];	
		//        }
		//    }
		//    else if ((hwRange & 0xA0) == 0xA0)	// -200V ~ +200V Range
		//    {
		//        Vqua = LDevConst.OP200VRangeOut;
		//        if (CMP >= 0x1980)
		//        {
		//            VF_StartIndex = LDevConst.memTableVFP[8];
		//            VF_EndIndex = LDevConst.memTableVFP[9];
		//            VS_StartIndex = LDevConst.memTableVSP[8];
		//            VS_EndIndex = LDevConst.memTableVSP[9];	
		//        }
		//        else
		//        {
		//            VF_StartIndex = LDevConst.memTableVFP[10];
		//            VF_EndIndex = LDevConst.memTableVFP[11];
		//            VS_StartIndex = LDevConst.memTableVSP[10];
		//            VS_EndIndex = LDevConst.memTableVSP[11];	
		//        }
		//    }
		//    else
		//    {
		//        VF_StartIndex = -1;
		//    }

		//    //-------------------------------------------------------------------------------------------------
		//    // AD1 = VS_Anode, 所以，用 memory 中的 VS 紀錄，確認計算的區間，在計算電壓值。
		//    // 校正的過程，memory 中的 VS 紀錄是 AD1 量化後的數位編碼紀錄。不是真實值。
		//    // 校正的過程，對應該區間的 memory VF 紀錄則是 Keithley 的真實值，
		//    // 原始，對應 Keithley 的真實值，是用 OP 操作電壓來對應16 bits 的量化單位，所以可由此反推真正的電壓值，
		//    // 再用比例的內插方式，算出 AD1 真的電壓數值是多少 Voltage.
		//    //-------------------------------------------------------------------------------------------------
		//    for (int k = 0; k < (VS_EndIndex - VS_StartIndex); k++)
		//    { 
		//        if ( (this._nMemory[k + VS_StartIndex] <=AD1) && (AD1< this._nMemory[k + VS_StartIndex + 1]))
		//        {
		//            V1 = this._nMemory[k + VF_StartIndex] ;
		//            V2 = this._nMemory[k + VF_StartIndex + 1 ];

		//            if (this._nMemory[k + 1] != this._nMemory[k])
		//            {
		//                volt = ((AD1 - this._nMemory[k]) / (this._nMemory[k + 1] - this._nMemory[k]) * (V2 - V1) + V1) * Vqua / 32767.0d;
		//            }
		//            else
		//            {
		//                volt = double.MaxValue;
		//            }
					
		//            break;
		//        }

		//        if (AD1 == this._nMemory[k])
		//        {
		//            volt = V2;
		//        }
		//    }

		//    if (CMP >= 0x1980)
		//    {
		//        return volt;
		//    }
		//    else
		//    {
		//        return (0.0 - volt);
		//    }

		//}

		private double calcVSN_ByAD2(int CMP, UInt16 AD2)
		{
			double volt = 0.0d;
			UInt16 start = 0, end = 1;

			if (CMP >= LDevConst.CMP_CENTER)
			{
				start = LDevConst.memTableVSN[0];
				end = LDevConst.memTableVSN[1];
			}
			else
			{
				start = LDevConst.memTableVSN[2];
				end = LDevConst.memTableVSN[3];
			}

			if ((end - start) != (LDevConst.CalVN_Value.Length - 1))
				return Double.MaxValue;

			if ((AD2 <= this._nMemory[start]) && this._nMemory[start] != 0)
			{
				volt = (double)AD2 / (double)this._nMemory[start] * LDevConst.CalVN_Value[0];
			}
			else if ((AD2 >= this._nMemory[end]) && this._nMemory[end] != 0)
			{
				volt = (double)AD2 / (double)_nMemory[end] * LDevConst.CalVN_Value[end - start];
			}
			else
			{
				for (int k = 0; k < (end - start); k++)
				{
					if ((this._nMemory[k + start] <= AD2) && (AD2 <= this._nMemory[k + start + 1]))
					{
						if (this._nMemory[k + start + 1] != this._nMemory[k + start])
						{
							volt = (double)(AD2 - this._nMemory[k + start]) / (double)(this._nMemory[k + start + 1] - this._nMemory[k + start]);
							volt = volt * (LDevConst.CalVN_Value[k + 1] - LDevConst.CalVN_Value[k]) + LDevConst.CalVN_Value[k];
						}
						else
						{
							volt = double.MaxValue;
						}
						break;
					}
				}
			}

			if (CMP >= LDevConst.CMP_CENTER)
			{
				return volt;
			}
			else
			{
				return (0.0 - volt);
			}

		}

		private double calcVSN_ByAD2_Old(int CMP, UInt16 AD2)
		{
			double volt = 0.0d;

			if (CMP >= 0x1980)
			{
				if (AD2 <= _nMemory[336])
				{
					volt = (double)AD2 / (double)_nMemory[336] * 0.003d;
				}
				else if (AD2 >= _nMemory[339])
				{
					volt = (double)AD2 / (double)_nMemory[339] * 3d;
				}
				else if (AD2 >= _nMemory[336] && AD2 <= _nMemory[337])
				{
					volt = (double)(AD2 - _nMemory[336]) / (double)(_nMemory[337] - _nMemory[336]) * 0.297d + 0.003d;
				}
				else if (AD2 >= _nMemory[337] && AD2 <= _nMemory[338])
				{
					volt = (double)(AD2 - _nMemory[337]) / (double)(_nMemory[338] - _nMemory[337]) * 1.2d + 0.3d;
				}
				else if (AD2 >= _nMemory[338] && AD2 <= _nMemory[339])
				{
					volt = (double)(AD2 - _nMemory[338]) / (double)(_nMemory[339] - _nMemory[338]) * 1.5d + 1.5d;
				}
			}
			else
			{
				if (AD2 <= _nMemory[340])
				{
					volt = (float)AD2 / (float)_nMemory[340] * 0.003d;
				}
				else if (AD2 >= _nMemory[343])
				{
					volt = (float)AD2 / (float)_nMemory[343] * 3d;
				}
				else if (AD2 >= _nMemory[340] && AD2 <= _nMemory[341])
				{
					volt = (float)(AD2 - _nMemory[340]) / (float)(_nMemory[341] - _nMemory[340]) * 0.297d + 0.003d;
				}
				else if (AD2 >= _nMemory[341] && AD2 <= _nMemory[342])
				{
					volt = (float)(AD2 - _nMemory[341]) / (float)(_nMemory[342] - _nMemory[341]) * 1.2d + 0.3d;
				}
				else if (AD2 >= _nMemory[342] && AD2 <= _nMemory[343])
				{
					volt = (float)(AD2 - _nMemory[342]) / (float)(_nMemory[343] - _nMemory[342]) * 1.5d + 1.5d;
				}

				volt = 0.0d - volt;
			}
			return volt;
		}

		private double calcVSP_ByAD1(byte bNowHWRange,int CMPcmd,UInt16 AD1)
		{
			//==============================================================================================================================
			// HWRange	=	b7	b6	b5	b4	-	b3	b2	b1	b0	,	b0 = 0, 閃避 0xFF 的 SOF/EOF 定義
			// IRange	=	x	x	x	v	-	v	v	v	0	, 0x1E
			// VRange	=	v	v	v	x	-	x	x	x	0	, 0xE0
			// 0x1980  <= CMP <= 0x30AF : Positive Value	, 0x024F  <= CMP <0x1980 : Negitive Value
			// 
			// Limit_I	= 0x0000 ~ 0x1FFE, 表達 12bit 的資料， b0 = 0, 閃避 0xFF 的 SOF/EOF 定義
			// Limit_I_HighByte 剩下的3 bits用來表達其他事項,
			// Limit_I_HighByte =	b7	b6	b5	b4	-	b3,	b2,	b1,	b0
			//						0	x	x	x	-	x	x	x	x	: MasterGainLoop = 0,	10uA,	1mA,	100mA
			//						1	x	x	x	-	x	x	x	x	: MasterGainLoop = 1,	1uA,	100uA,	10mA	
			//==============================================================================================================================

			int memVSP_StartIndex = -1, memVSP_EndIndex = -1,memVFP_StartIndex = -1, memVZeroIndex = -1;
			float Vqua = 0.0f;
			double volt = 0;
			int segBitMin = 0, segBitMax= 0, zeroBit = 0;
			double segVF_Min =0, segVF_Max =0;
			E_VRange vRangeSetting;

			vRangeSetting = (E_VRange)(((int)bNowHWRange) & 0xE0);

			if (LDevConst.CMP_CENTER <= CMPcmd)
			{
				memVZeroIndex = LDevConst.memTableVZero[0];
				if (vRangeSetting == E_VRange._6V)
				{
					memVSP_StartIndex = LDevConst.memTableVSP[0];
					memVSP_EndIndex = LDevConst.memTableVSP[1];

					memVFP_StartIndex = LDevConst.memTableVFP[0];
					Vqua = LDevConst.OP6VRangeOut;
				}
				else if (vRangeSetting == E_VRange._20V)
				{
					memVSP_StartIndex = LDevConst.memTableVSP[4];
					memVSP_EndIndex = LDevConst.memTableVSP[5];

					memVFP_StartIndex = LDevConst.memTableVFP[4];
					Vqua = LDevConst.OP20VRangeOut;
				}
				else if (vRangeSetting == E_VRange._200V)
				{
					memVSP_StartIndex = LDevConst.memTableVSP[8];
					memVSP_EndIndex = LDevConst.memTableVSP[9];

					memVFP_StartIndex = LDevConst.memTableVFP[8];
					Vqua = LDevConst.OP200VRangeOut;
				}
				else
				{
					memVSP_StartIndex = -1;
				}
			}
			else
			{
				memVZeroIndex = LDevConst.memTableVZero[1];
				if (vRangeSetting == E_VRange._6V)
				{
					memVSP_StartIndex = LDevConst.memTableVSP[2];
					memVSP_EndIndex = LDevConst.memTableVSP[3];
					
					memVFP_StartIndex = LDevConst.memTableVFP[2];
					Vqua = LDevConst.OP6VRangeOut;
				}
				else if (vRangeSetting == E_VRange._20V)
				{
					memVSP_StartIndex = LDevConst.memTableVSP[6];
					memVSP_EndIndex = LDevConst.memTableVSP[7];

					memVFP_StartIndex = LDevConst.memTableVFP[6];
					Vqua = LDevConst.OP20VRangeOut;
				}
				else if (vRangeSetting == E_VRange._200V)
				{
					memVSP_StartIndex = LDevConst.memTableVSP[10];
					memVSP_EndIndex = LDevConst.memTableVSP[11];

					memVFP_StartIndex = LDevConst.memTableVFP[10];
					Vqua = LDevConst.OP200VRangeOut;
				}
				else
				{
					memVSP_StartIndex = -1;
					memVFP_StartIndex = -1;
				}
			}

			if (( memVSP_StartIndex == -1) || (memVFP_StartIndex == -1) )
			{
				return 0.0;
			}

			//-------------------------------------------------------------------------------------------------
			// AD1 = VS_Anode, 所以，用 memory 中的 VSP 紀錄，確認計算的區間，在計算電壓值。
			// 校正的過程，memory 中的 VSP 紀錄是 AD1 量化後的數位編碼紀錄。不是真實值。
			// 校正的過程，對應該區間的 memory VFP 紀錄則是 Keithley 的真實值，
			// 原始，對應 Keithley 的真實值，是用 OP 操作電壓來對應16 bits 的量化單位，所以可由此反推真正的電壓值，
			// 再用比例的內插方式，算出 AD1 真的電壓數值是多少 Voltage.
			// zeroBit, 當成是最小 bit 的跳動點，以下，都視為是 VSP = 0V,
			//-------------------------------------------------------------------------------------------------


			zeroBit= _nMemory[ memVZeroIndex ];
			for (int p = 0; p < (memVSP_EndIndex - memVSP_StartIndex); p++)
			{
				segBitMin = _nMemory[ memVSP_StartIndex + p];
				segBitMax = _nMemory[ memVSP_StartIndex + p + 1];
				segVF_Min = _nMemory[ memVFP_StartIndex + p];			// * (Vqua / 32767.0d )
				segVF_Max = _nMemory[ memVFP_StartIndex + p + 1];		// * (Vqua / 32767.0d )

				if ( ( p ==0 ) && AD1 <= segBitMin )
				{
					if ( AD1 <= zeroBit  )
					{
						volt = 0.0;
					}
					else
					{
						if ( segVF_Min != zeroBit )
						{
							volt = ((double)(AD1-zeroBit)) / ((double)(segBitMin - zeroBit)) * segVF_Min * Vqua / 32767.0d;
						}
						else
						{
							volt = double.MaxValue;
						}
					}
					break;
				}

				if ( ( segBitMin < AD1 ) && (AD1 <= segBitMax ))
				{
					if ( segBitMin != segVF_Max )
					{
						volt = (((double)(AD1 - segBitMin )) / ((double)( segBitMax - segBitMin)) * ((double)(segVF_Max - segVF_Min)) + (double)segVF_Min ) * Vqua / 32767.0d;
					}
					else
					{
						volt= double.MaxValue;
					}
					break;
				}

				if ((p == (memVSP_EndIndex - memVSP_StartIndex -1)) && (segBitMin < AD1))
				{
					if ( segBitMax != 0 )
					{
						volt = ((double)(AD1))/((double)(segBitMax)) * (double)(segVF_Max) * Vqua / 32767.0d;
					}
					else
					{
						volt = double.MaxValue;
					}
					break;
				}
			}

			//return volt;	// 所有 bit 數運算最小就是 0

			if (LDevConst.CMP_CENTER <= CMPcmd)
			{
				return volt;
			}
			else
			{
				return (0.0 - volt);
			}
			
		}

		private double calcVSP_ByAD1_Old(byte hwRange, int CMPcmd, UInt16 AD1)
		{
			int addr = 0;
			double volt = 0;
			UInt16 temp = AD1;
			//txtAD1.Text = Convert.ToString(temp);
			//--------------------------------------------------------------
			// 20V Range
			//--------------------------------------------------------------
			if ((hwRange & 0x60) == 0x60)					// 20VRange
			{
				if (CMPcmd >= 0x1980)
				{
					addr = 141;								// Start address of VS141 for +20VRange
					if (temp <= _nMemory[141])				// Mem[141] = VS48, +20VRange 第一個點。
					{
						if (temp > _nMemory[288])			// Mem[288], 0V offset@p0V , CMP = 0x1980
						{
							volt = (float)(temp - _nMemory[288]) / (float)(_nMemory[141] - _nMemory[288]) * (float)_nMemory[94] * LDevConst.OP20VRangeOut / 32767f;
						}
						else
						{
							volt = 0;
						}
					}
					else if (temp >= _nMemory[164])			// Mem[164] = VS71, +20VRange 最後的點。
					{
						volt = (float)temp / (float)_nMemory[164] * (float)_nMemory[117] * LDevConst.OP20VRangeOut / 32767f;
					}

					while (addr < 164)						// End address for VS164 +20VRange
					{
						if (temp >= _nMemory[addr] && temp <= _nMemory[addr + 1])
						{
							volt = ((float)(temp - _nMemory[addr]) / (float)(_nMemory[addr + 1] - _nMemory[addr]) * (float)(_nMemory[addr - 46] - _nMemory[addr - 47]) + (float)_nMemory[addr - 47]) * LDevConst.OP20VRangeOut / 32767f;
							addr = 164;
						}
						else
						{
							addr = addr + 1;
						}
					}
					volt = volt - (_voltOffset20V_1 / 1000);
				}
				else
				{
					addr = 165;								// Start address of VS165 for -20VRange
					if (temp <= _nMemory[165])				// Mem[165] = VS72, -20VRange 第一個點。
					{
						if (temp > _nMemory[289])			// Mem[289] = 0V offset@n0V  CMP = 0x197E
						{
							volt = (float)(temp - _nMemory[289]) / (float)(_nMemory[165] - _nMemory[289]) * (float)_nMemory[118] * LDevConst.OP20VRangeOut / 32767f;
						}
						else
						{
							volt = 0;
						}
					}
					else if (temp >= _nMemory[187])			// Mem[187] = VS94, -20VRange 最後的點。
					{
						volt = (float)temp / (float)_nMemory[187] * (float)_nMemory[140] * LDevConst.OP20VRangeOut / 32767f;
					}

					while (addr < 187)						// End address of VS187 for -20VRange
					{
						if (temp >= _nMemory[addr] && temp <= _nMemory[addr + 1])
						{
							volt = ((float)(temp - _nMemory[addr]) / (float)(_nMemory[addr + 1] - _nMemory[addr]) * (float)(_nMemory[addr - 46] - _nMemory[addr - 47]) + (float)_nMemory[addr - 47]) * LDevConst.OP20VRangeOut / 32767f;
							addr = 187;
						}
						else
						{
							addr = addr + 1;
						}
					}
					volt = volt + (_voltOffset20V_1 / 1000);
				}
			}
			//--------------------------------------------------------------
			// 6V Range
			//--------------------------------------------------------------
			else if ((hwRange & 0x40) == 0x40)			// 6V Range
			{
				if (CMPcmd >= 0x1980)
				{
					addr = 47;								// Start address of VS1 for +6VRang
					if (temp <= _nMemory[47])				// Mem[46] = VS1, +6VRange 第一個點。
					{
						if (temp > _nMemory[288])			// Mem[288], 0V offset@p0V , CMP = 0x1980
						{
							volt = (float)(temp - _nMemory[288]) / (float)(_nMemory[47] - _nMemory[288]) * (float)_nMemory[0] * LDevConst.OP6VRangeOut / 32767f;
						}
						else
						{
							volt = 0;
						}
					}
					else if (temp >= _nMemory[70])			// Mem[70] = VS24, +6VRange 最後的點。
					{
						volt = (float)temp / (float)_nMemory[70] * (float)_nMemory[23] * LDevConst.OP6VRangeOut / 32767f;
					}

					while (addr < 70)						// End address of VS24 for +6VRange
					{
						if (temp >= _nMemory[addr] && temp <= _nMemory[addr + 1])
						{
							volt = ((float)(temp - _nMemory[addr]) / (float)(_nMemory[addr + 1] - _nMemory[addr]) * (float)(_nMemory[addr - 46] - _nMemory[addr - 47]) + (float)_nMemory[addr - 47]) * LDevConst.OP6VRangeOut / 32767f;
							addr = 70;
						}
						else addr = addr + 1;
					}
					volt = volt - (_voltOffset6V_1 / 1000);
				}
				else
				{
					addr = 71;								// Start address of VS25 for -6VRang					
					if (temp <= _nMemory[71])				// Mem[71] = VS25, -6VRange 第一個點。
					{
						if (temp > _nMemory[289])			// Mem[289] = 0V offset@n0V  CMP = 0x197E
						{
							volt = (float)(temp - _nMemory[289]) / (float)(_nMemory[71] - _nMemory[289]) * (float)_nMemory[24] * LDevConst.OP6VRangeOut / 32767f;
						}
						else
						{
							volt = 0;
						}
					}
					else if (temp >= _nMemory[93])			// Mem[93] = VS47, -6VRange 最後的點。
					{
						volt = (float)temp / (float)_nMemory[93] * (float)_nMemory[46] * LDevConst.OP6VRangeOut / 32767f;
					}

					while (addr < 93)						// End address of VS47 for -6VRange
					{
						if (temp >= _nMemory[addr] && temp <= _nMemory[addr + 1])
						{
							volt = ((float)(temp - _nMemory[addr]) / (float)(_nMemory[addr + 1] - _nMemory[addr]) * (float)(_nMemory[addr - 46] - _nMemory[addr - 47]) + (float)_nMemory[addr - 47]) * LDevConst.OP6VRangeOut / 32767f;
							addr = 93;
						}
						else
						{
							addr = addr + 1;
						}
					}
					volt = volt + (_voltOffset6V_1 / 1000);
				}
			}
			//--------------------------------------------------------------
			// 200V Range
			//--------------------------------------------------------------
			else		// 200V Range
			{
				if (CMPcmd >= 0x1980)
				{
					addr = 235;
					if (temp <= _nMemory[235])
					{
						if (temp > _nMemory[288])
						{
							volt = (float)(temp - _nMemory[288]) / (float)(_nMemory[235] - _nMemory[288]) * (float)_nMemory[188] * LDevConst.OP200VRangeOut / 32767f;
						}
						else
						{
							volt = 0;
						}
					}
					else if (temp >= _nMemory[258])
					{
						volt = (float)temp / (float)_nMemory[258] * (float)_nMemory[211] * LDevConst.OP200VRangeOut / 32767f;
					}

					while (addr < 258)
					{
						if (temp >= _nMemory[addr] && temp <= _nMemory[addr + 1])
						{
							volt = ((float)(temp - _nMemory[addr]) / (float)(_nMemory[addr + 1] - _nMemory[addr]) * (float)(_nMemory[addr - 46] - _nMemory[addr - 47]) + (float)_nMemory[addr - 47]) * LDevConst.OP200VRangeOut / 32767f;
							addr = 258;
						}
						else
						{
							addr = addr + 1;
						}
					}
					volt = volt - (_voltOffset200V_1 / 100);
				}
				else
				{
					addr = 259;
					if (temp <= _nMemory[259])
					{
						if (temp > _nMemory[289])
						{
							volt = (float)(temp - _nMemory[289]) / (float)(_nMemory[259] - _nMemory[289]) * (float)_nMemory[212] * LDevConst.OP200VRangeOut / 32767f;
						}
						else
						{
							volt = 0;
						}
					}
					else if (temp >= _nMemory[281])
					{
						volt = (float)temp / (float)_nMemory[281] * (float)_nMemory[234] * LDevConst.OP200VRangeOut / 32767f;
					}
					while (addr < 281)
					{
						if (temp >= _nMemory[addr] && temp <= _nMemory[addr + 1])
						{
							volt = ((float)(temp - _nMemory[addr]) / (float)(_nMemory[addr + 1] - _nMemory[addr]) * (float)(_nMemory[addr - 46] - _nMemory[addr - 47]) + (float)_nMemory[addr - 47]) * LDevConst.OP200VRangeOut / 32767f;
							addr = 281;
						}
						else
						{
							addr = addr + 1;
						}
					}
					volt = volt + (_voltOffset200V_1 / 100);
				}
			}

			if (CMPcmd >= 0x1980)
			{
				return volt;
			}
			else
			{
				return (0.0d - volt);
			}

		}

		private string[] cal_2Wire(byte hwRange, int CMPcmd, UInt16 readBackCMP, UInt16 AD2)
		{
			//------------------------------------------------------------------------------------------------------------------------
			// cal_2Wire() is for 2-wire applicaion, OPEN Loop calibration 
			// readBackCMP : 指的應該是動態改變的驅動值 AD1C = CMP = CMPA_CMPAHR, ==> look up VFP_Table to calculate the voltage
			//------------------------------------------------------------------------------------------------------------------------
			double VSP = 0, VSN = 0, VSD = 0;
			double currIFN = 0;

			//currIFN = this.calcIFN_ByAD0(hwRange,CMPcmd,

			//---------------------------------------------------------------------------
			// VS+ Positive, AD1, VS_Anode of 2 wire structure
			//---------------------------------------------------------------------------
			VSP = calcVFP_ByCMP(hwRange, CMPcmd, readBackCMP);
			this._sRtnVS[0] = Convert.ToString(VSP);
			this._sRtnVS[1] = "V";
			this._rtnValue[1] = VSP;

			//---------------------------------------------------------------------------
			// VS- Negative, AD2, VS_Cathode
			//---------------------------------------------------------------------------
			VSN = calcVSN_ByAD2(CMPcmd, AD2);
			this._sRtnVS[2] = Convert.ToString(VSN);
			this._sRtnVS[3] = "V";
			this._rtnValue[2] = VSN;

			//---------------------------------------------------------------------------
			// Vdd = (VS+ - VS-) = ( VS_Anode - VS_Cathode ), 
			//---------------------------------------------------------------------------
			VSN = Math.Abs(VSN);		// Gilbert 下面運算只看正值, Why ?

			if ((VSP - VSN) >= 0)
			{
				VSD = VSP - VSN;
			}
			else
			{
				VSD = 0;
			}

			if (CMPcmd >= LDevConst.CMP_CENTER)							// if (readBackCMP >= LDevConst.CMP_CENTER) // Gilbert 怪怪的應該看起 4Wire 的作法
			{
				this._sRtnVS[4] = Convert.ToString(VSD);
				this._rtnValue[3] = VSD;
			}
			else
			{
				this._sRtnVS[4] = Convert.ToString(-VSD);
				this._rtnValue[3] = 0.0 - VSD;
			}
			this._sRtnVS[5] = "V";

			return _sRtnVS.Clone() as string[];
		}

		private string[] cal_4Wire(byte hwRange, int CMPcmd, UInt16 AD1, UInt16 AD2)
		{
			//------------------------------------------------------------------------------------------------------------------------
			// cal_4Wire() is for 4-wire applicaion, Close Loop calibration 
			// the AD1 = ADC transfer value, 0x0000 ~ 0x0FFF, ==> look up VF & VS table to calculate the voltage
			//------------------------------------------------------------------------------------------------------------------------

			double VSP = 0, VSN = 0, VSD = 0;

			//---------------------------------------------------------------------------
			// VS+ Positive, AD1, VS_Anode of 4 wire structure
			//---------------------------------------------------------------------------

			VSP = calcVSP_ByAD1_Old(hwRange, CMPcmd, AD1);
			if ((hwRange & 0xE0) == 0x40) { this._rtnValue[4] = _voltOffset6V_1 / 1000; }	// mV to V
			if ((hwRange & 0xE0) == 0x60) { this._rtnValue[4] = _voltOffset20V_1 / 1000; }	// mV to V
			if ((hwRange & 0xE0) == 0xA0) { this._rtnValue[4] = _voltOffset200V_1 / 100; }	// 10mV to V

			this._sRtnVS[0] = Convert.ToString(VSP);
			this._rtnValue[1] = VSP;

			this._voltOffset6V_1 = 0;
			this._voltOffset6V_2 = 0;
			this._voltOffset20V_1 = 0;
			this._voltOffset20V_2 = 0;
			this._voltOffset200V_1 = 0;
			this._voltOffset200V_2 = 0;
			this._sRtnVS[1] = "V";
			
			//---------------------------------------------------------------------------
			// VS- Negative, AD2, VS_Cathode
			//---------------------------------------------------------------------------
			VSN = calcVSN_ByAD2(CMPcmd, AD2);
			this._sRtnVS[2] = Convert.ToString(VSN);
			this._sRtnVS[3] = "V";
			this._rtnValue[2] = VSN;			

			//---------------------------------------------------------------------------
			// Vdd = (VS+ - VS-) = ( VS_Anode - VS_Cathode ), 
			//---------------------------------------------------------------------------

			if ((VSP - VSN) >= 0)
			{
				VSD = VSP - VSN;
			}
			else
			{
				VSD = VSN - VSP;
			}

			if (CMPcmd >= LDevConst.CMP_CENTER)
			{
				this._sRtnVS[4] = Convert.ToString(VSD);
				this._rtnValue[3] = VSD;
			}
			else
			{
				this._sRtnVS[4] = Convert.ToString(-VSD);
				this._rtnValue[3] = 0.0d -VSD;
			}
			this._sRtnVS[5] = "V";



			return _sRtnVS;
		}

		public void runSingleSequence(float V, float I, string I_unit, byte time, byte mode, byte continuous)
		{
			this.Message = "";

			if (this._isMultiDriveState == true)
			{
				this.Multi_Getdata();
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
			}
			if (this._isMultiDriveState == false && this._isMultiCurveState == false)
			{
				try
				{
					_seqIndex = 0;
					//if (time > 0xFE) time = 0xFE;
					
					//this._bTimeCount = (byte)(this._bTimeBase * time / 100);     //計算時間
					//if (this._bTimeCount <= 0x01)	{   this._bTimeCount = 0x01;	}
					//if (this._bTimeCount >= 0xFE)	{   this._bTimeCount = 0xFE;	}

					this.SetForceTime(0, (double)time);
					this.cmd_converter(V, I, I_unit);                   //設定電壓電流檔位； // 計算 bHW_Range, _bCMPA, _bCMPAHR, _byCMP_ex[0], _byCMP_ex[1], _byCMP_ex_temp

					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetSequence; //0x50
					_byUSBTx[4] = 0;
					_byUSBTx[5] = this._bHW_DevRange;
					_byUSBTx[6] = this._bCMPA;
					_byUSBTx[7] = this._bCMPAHR;
					_byUSBTx[8] = this._bLimitCurrLow;                               // 0xFE;   low byte
					_byUSBTx[9] = this._bLimitCurrHigh;                              // 0x1F;   high byte
					_byUSBTx[10] = this._bTimeCount;
					if (mode == 1)
					{
						_byCMP_ex[0] = 0;
						_byCMP_ex[1] = 0;
					}

					_byUSBTx[11] = _byCMP_ex[0];
					_byUSBTx[12] = _byCMP_ex[1];
					_byUSBTx[13] = 0xFF;
                    _byUSBTx[14] = (byte)ERegCmd.SetSequence; //0x50
					_byUSBTx[15] = 1;
					_byUSBTx[16] = continuous;
					_byUSBTx[17] = 0xFF;

					this.LDT3A200USB.Write(_byUSBTx, 0, 18);

					_seqHWRange[0] = this._bHW_DevRange;				// bHW_Range
					_seqCMP[0] = (_bCMPA << 8) + this._bCMPAHR;			// CMP
					_seqMasterGL[0] = _bLimitCurrHigh;					// Limit high byte = MasterGainLoop

					int USBbytes = this.LDT3A200USB.BytesToRead;
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, this._bTimeCount + 5000, Timeout.Infinite);

					while (USBbytes < 16)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();

					byte[] _byUSBRx = new byte[16];

					this.LDT3A200USB.Read(_byUSBRx, 0, 16);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
					{
						this.Message = this.Message + "communication error\n";
					}

					UInt16 AD0 = 0, AD1 = 0, AD2 = 0;

					AD0 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[10]) + (Convert.ToUInt16(_byUSBRx[11]) * 256));             // F/W 值 AD0, IF, F-, Current
					AD1 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[12]) + (Convert.ToUInt16(_byUSBRx[13]) * 256));             // F/W 值 AD1, VS_Anode, S+
					AD2 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[14]) + (Convert.ToUInt16(_byUSBRx[15]) * 256));             // F/W 值 AD2, VS_Cathode, VN, S-
					//----------------------------------------------------------------------------------------------------------------------------------------
					// 由設定的 hwRange, CMP, Limit_I 等參數，與讀取回來的 AD0, AD1, AD2, AD1C (真正驅動的 CMPA值) 等數據
					// 透過校正時建立的資料表，對應回查反算真正的數據，得到 IF, VSP (VS_Anode), VSN (VS_Cathode), VSD = VSP - VSN
					// 注意 : (01) cal_IF 是由 AD0 反算 IF 值
					//		  (02) cal_VS 是針對二線式，所以是 CMP (F+) 算 VSP, 以及 AD2 算 VSN 和 VSD, // Gilbert 都二線了，哪來的 VSN ???
					//		  (03) cal_4Wire 是針對四線式。所以是 AD1 算VSP, AD2 算 VSN，再算 VSD
					//
					//  ** 但是，因為  cal_4Wire 的計算過程會再補償一個電流流過的的電壓值，
					// 所以執行 cal_4Wire() 要跑 cal_IF(), 同時，在  cal_4Wire() 內會將計算過的 offset 變數，再設回 0.  // Gilbert 用法邏輯不佳
					//--------------------------------------------------------------------------------------------------------------------------------------------

					this._execIRange = this.GetIRange(_seqHWRange[0], _seqMasterGL[0]);
					this._execVRange = (E_VRange)(_seqHWRange[0] & 0xE0);

					this._execData[0] = _seqHWRange[0];
					this._execData[1] = _seqCMP[0];
					this._execData[2] = (_byCMP_ex[1] << 8) + _byCMP_ex[0];
					this._execData[3] = ((this._bLimitCurrHigh << 8) + this._bLimitCurrLow);
					this._execData[4] = _seqMasterGL[0] & 0x80;
					this._execData[5] = AD0;
					this._execData[6] = AD1;
					this._execData[7] = AD2;
					this._execData[8] = this._nClampI;

					_sRtnIF = cal_IF(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0);
					
					if ((this._bStatus & 0x04) == 0x04)		// 設定在四線式架構下，啟動 F+ / F- / S+ / S- 的架構，可作閉迴路
					{
						_sRtnVS = this.cal_4Wire(_seqHWRange[0], _seqCMP[0], AD1, AD2);
					}
					else
					{
						if (((_seqHWRange[0] & 0x60) == 0x60) || ((_seqHWRange[0] & 0x40) == 0x40))						// 0x40 = vRange = 6V ; 0x60 = vRange = 20V
						{
							_sRtnVS = this.cal_2Wire(_seqHWRange[0], _seqCMP[0], AD1, AD2);		// 用2線式的 cal_2Wire() 計算， 並沒有 AD1 的數據 ? // Gilbert 不對吧 !!	
						}
						else
						{
							_sRtnVS = this.cal_4Wire(_seqHWRange[0], _seqCMP[0], AD1, AD2);
						}
					}

					this._sMeasureIV[0] = _sRtnIF[0];
					this._sMeasureIV[1] = _sRtnIF[1];
					this._sMeasureIV[2] = _sRtnVS[0];
					this._sMeasureIV[3] = _sRtnVS[1];
					this._sMeasureIV[4] = _sRtnVS[2];
					this._sMeasureIV[5] = _sRtnVS[3];
					this._sMeasureIV[6] = _sRtnVS[4];   //  S/W   LDT3A_measure
					this._sMeasureIV[7] = _sRtnVS[5];
					this._sMeasureIV[8] = Message;

				}
				catch (Exception e1)
				{
					this.Message = e1.ToString();
				}
			}

		}

		private void Fore_Wire(bool isEnable)
		{
			if (isEnable)
			{
				this._bStatus = Convert.ToByte(this._bStatus | 0x04);
				this.SetStatusToDev();
			}
			else
			{
				this._bStatus = Convert.ToByte(this._bStatus & 0xFB);
				this.SetStatusToDev();
			}
		}

		private void Internal_IO_Direction(byte direction1, byte direction2, byte direction3)
		{
			this.Message = "";
			try
			{
				if (this._isMultiDriveState == true)
				{
					this.Multi_Getdata();
				}
				if (this._isMultiCurveState == true)
				{
					this.Multi_GetCurveData();
				}
				if (this._isMultiDriveState == false && this._isMultiCurveState == false)
				{
					direction1 = Convert.ToByte(direction1 & 0x7F);
					direction2 = Convert.ToByte(direction2 & 0x7F);
					direction3 = Convert.ToByte(direction3 & 0x7F);
					
					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetInternalIO;                                          //0x44
					_byUSBTx[4] = direction1;
					_byUSBTx[5] = direction2;
					_byUSBTx[6] = direction3;
					_byUSBTx[7] = 0xFF;

					this.LDT3A200USB.Write(_byUSBTx, 0, 8);
					int USBbytes = this.LDT3A200USB.BytesToRead;
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
					
					while (USBbytes < 3)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();
					
					byte[] _byUSBRx = new byte[USBbytes];
					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x44)
					{
						this.Message = this.Message + "communication error\n";
					}
				}
			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}
		}

		private byte[] Internal_IO(byte setting1, byte setting2, byte setting3)
		{
			this.Message = "";
			byte[] reading = new byte[3];
			try
			{
				if (this._isMultiDriveState == true)
				{
					this.Multi_Getdata();
				}
				if (this._isMultiCurveState == true)
				{
					this.Multi_GetCurveData();
				}
				if (this._isMultiDriveState == false && this._isMultiCurveState == false)
				{
					setting1 = Convert.ToByte(setting1 & 0x7F);
					setting2 = Convert.ToByte(setting2 & 0x7F);
					setting3 = Convert.ToByte(setting3 & 0x7F);
					
					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteInternalIO;             //0x45
					_byUSBTx[4] = setting1;
					_byUSBTx[5] = setting2;
					_byUSBTx[6] = setting3;
					_byUSBTx[7] = 0xFF;
					
					this.LDT3A200USB.Write(_byUSBTx, 0, 8);
					int USBbytes = this.LDT3A200USB.BytesToRead;
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
					
					while (USBbytes < 6)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();
					
					byte[] _byUSBRx = new byte[USBbytes];
					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x45)
					{
						this.Message = this.Message + "communication error\n";
					}

					reading[0] = _byUSBRx[3];
					reading[1] = _byUSBRx[4];
					reading[2] = _byUSBRx[5];
				}
			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}
			return reading;
		}

		

		#endregion

		#region >>> Private Methods New <<<

		private void Cmd_I_Convert(double V, double I, string I_unit, bool isAutoIRange)
		{
			int index_IRange = 0;
			int memIFStartIndex = -1;
			double iRangeMax = 0.0;
			double uA_Value = 0.0d;
			string[] iRangeStrArr;

			this._nClampI = 0;
			this._sIRange = string.Empty;

			#region >> (01) 由輸入I, 調整數據與單位，對應內部定義的 IRange 檔位

			//---------------------------------------------------------------------------
			// 全部轉成 uA unit 表達，以 double 變數，來運算，自動找出適合的 IRange，
			// 以正數計算 0x0000 ~ 0xFFFF
			//---------------------------------------------------------------------------

			I = Math.Abs(I);

			if (I_unit == "mA")
			{
				uA_Value = ((double)I) * 1E3;
			}
			else if (I_unit == "A")
			{
				uA_Value = ((double)I) * 1E6;
			}
			else
			{
				uA_Value = (double)I;
			}

			if (uA_Value > 3E6)
			{
				uA_Value = 3E6;
			}

			iRangeStrArr = Enum.GetNames(typeof(E_IRange));
			for (int i = 0; i < iRangeStrArr.Length; i++)
			{
				iRangeStrArr[i] = iRangeStrArr[i].Remove(0, 1);
			}

			for (int p = 0; p < (LDevConst.IRANGE_AutoValue.Length - 1); p++)
			{
				if ((p == 0) && (uA_Value <= LDevConst.IRANGE_AutoValue[0]))
				{
					this._sIRange = iRangeStrArr[0];
					index_IRange = 0;
					break;
				}

				if ((LDevConst.IRANGE_AutoValue[p] < uA_Value) && (uA_Value <= LDevConst.IRANGE_AutoValue[p + 1]))
				{
					this._sIRange = iRangeStrArr[p + 1];
					index_IRange = p + 1;
					break;
				}
			}

			//-----------------------------------------------------------------------
			// Gilbert, 對於 1uA 這一個測量值，該用哪一個 IRange 有特殊作法，當下是
			//   1uA<= input <=10uA, 也就是用 10uAIRange
			//-----------------------------------------------------------------------
			if (this._sIRange == "1uA" && Math.Abs(uA_Value - 1) < double.Epsilon)
			{
				this._sIRange = "10uA";				// uA_Value = 1uA, using the 10uARange,
				index_IRange = index_IRange + 1;
			}

			// Gilbert, 目前有 2 個 IRange 先閃避, 都往上提升一個 IRange 檔位
			if (this._sIRange == "100uA")
			{
				this._sIRange = "1mA";
				index_IRange = index_IRange + 1;
			}

			if (this._sIRange == "10mA")
			{
				this._sIRange = "100mA";
				index_IRange = index_IRange + 1;
			}

			if (isAutoIRange == true)
			{
				this._iRangeSet = (E_IRange)(Enum.GetValues(typeof(E_IRange)).GetValue(index_IRange));
			}
			else
			{
				index_IRange = Array.IndexOf(Enum.GetValues(typeof(E_IRange)), this._iRangeSet);
				if ( Math.Abs(I) > LDevConst.IRANGE_SegValue[index_IRange])
				{
					I = LDevConst.IRANGE_SegValue[index_IRange];
				}
				//uA_Value = I;	// No Auto-Tuning, Base unit = uA
			}

			this._bHW_DevRange = SetIVRange2HWRange( this._iRangeSet, this._vRangeSet);		// 依據設定 IRange，或是自動找到的 IRange 重新設定 HW_Range

			if (index_IRange <= 3)		// 1uA, 10uA, 100uA, 1mA		==> using uA as unit
			{
				iRangeMax = LDevConst.IRANGE_SegValue[index_IRange];
				I = uA_Value;
			}
			else						// 10mA, 100mA, 800mA, 2A, 3A	==> using mA as uint
			{
				iRangeMax = (LDevConst.IRANGE_SegValue[index_IRange] * 1E-3);
				I = (uA_Value * 1E-3);
			}

			#endregion

			#region >> (02) 由輸入I, 查表內插設定 limit_I 的 HIGH byte & LOW byte (含masterGainLoop 設定)
			//------------------------------------------------------------------------------------------------------
			// _nMemory[i] = 0x0000 ~ 0xFFFF, 存放的是 2 bytes 的資料。
			// _nClampI 是用 0x0000 ~ 0x0FFF, 的 12bit 資料的表達，直接對應到 ADC 的紀錄能力。
			// _bLimitCurrLow 與 _bLimitCurrHigh 為一個 byte 的資料，用於 USB 的傳送，所以需要避開 0xFF。
			//------------------------------------------------------------------------------------------------------
			double calcCurrent = 0.0;
			double segMin = 0.0;
			double segMax = 0.0;

			if (V >= 0.0d)
			{
				memIFStartIndex = LDevConst.memTableIFN[index_IRange << 2];			// index = 0, 4, 8, ...		=>> +1uA, +10uA, +100uA, ...
			}
			else
			{
				memIFStartIndex = LDevConst.memTableIFN[(index_IRange << 2) + 2];	// index = 2, 6, 10, ...	=>> -1uA, -10uA, -100uA, ...
			}

			for (int p = 0; p < (LDevConst.IFN_SegScale.Length - 1); p++)
			{
				segMin = LDevConst.IFN_SegScale[p] * iRangeMax;
				segMax = LDevConst.IFN_SegScale[p + 1] * iRangeMax;			// SegMin != SegMax
				if ((segMin < I) && (I <= segMax))
				{
					if ((segMax - segMin) < double.Epsilon)
					{
						this._nClampI = 0x0FFF;
					}
					else
					{
						calcCurrent = (I - segMin) / (segMax - segMin) * ((double)(_nMemory[memIFStartIndex + p + 1] - _nMemory[memIFStartIndex + p])) + (double)_nMemory[memIFStartIndex + p];
						this._nClampI = (Convert.ToUInt16(calcCurrent) >> 4);			// 記憶存的是 DSP Moving Average 的資料，用的是 16 bit, ClampI 的命令輸入，
					}																// 對應原始 AD0 輸入的 12 bits, 在 DPS 內直接運算.
                          
              
				}
			}

			// Special case for the 1uA Range
			if ((index_IRange == 0) && (Math.Abs(I) <= 0.0005d))		// 1uA_IRange
			{
				this._nClampI = 0;
			}

			//if (_sIRange == "10uA" || _sIRange == "1mA" || _sIRange == "100mA")		// MasterGainLoop = 0
			if ((index_IRange == 1) || (index_IRange == 3) || (index_IRange == 5))
			{
				this._bLimitCurrLow = Convert.ToByte((this._nClampI << 1) & 0xFF);
				this._bLimitCurrHigh = Convert.ToByte((this._nClampI << 1) >> 8);
			}
			else																		// MasterGainLoop = 1
			{
				_bLimitCurrLow = Convert.ToByte((this._nClampI << 1) & 0xFF);
				_bLimitCurrHigh = Convert.ToByte(((this._nClampI << 1) >> 8) + 0x80);
			}

			#endregion

		}

        private void Cmd_V_Convert(double V, bool isAutoVRange)
		{
			int memVFStartIndex = -1, memFirtRight = -1;
			int cmpIndex = 0, CMP_ex_Index;
			double volt_Add_CMP_ex = 3.0d;
			int totalMEP_temp = 0;
			int totalMEP = 0;
			
			double segMin = 0.0;
			double segMax = 0.0;

			int startA_Reg = (LDevConst.CMP_CENTER >> 8);
			byte bCMP_ex_A;
			byte bCMP_ex_HR;
			int segCount = 0;
			double vRangeMax = 0.0;
			int cmpLeft = LDevConst.CMP_CENTER;
			int cmpRight = cmpLeft + LDevConst.A_REG_STEP;
			int index_VRange = -1;

			this._bCMPA = (byte)startA_Reg;
			this._bCMPAHR = LDevConst.HR_REG_CENTER;

			#region >> (01) 由輸入V, 調整數據與單位，對應內部定義的 VRange 檔位

			if (V > 205d)	{ V = 205d;		}	// 涵蓋了 200V + CMP_ex_volt = 3V = 203V < 205V
			if (V < -205d)	{ V = -205d;		}

			if (isAutoVRange == true)
			{
				if (Math.Abs(V) <= 6.001d)
				{
					this._vRangeSet = E_VRange._6V;
				}
				else if ((6.001d < Math.Abs(V)) && (Math.Abs(V) <= 20.001d))
				{
					this._vRangeSet = E_VRange._20V;
				}
				else
				{
					this._vRangeSet = E_VRange._200V;
				}
			}

			index_VRange = Array.IndexOf(Enum.GetValues(typeof(E_VRange)), this._vRangeSet);

			this._bHW_DevRange = SetIVRange2HWRange(this._iRangeSet, this._vRangeSet);							// 依據設定的 VRange 或是自動找到的 IRange 檔位, 設定 HW_Range

			if (this._vRangeSet == E_VRange._6V)
			{
				vRangeMax = LDevConst.OP6VRangeOut;
			}
			else if (this._vRangeSet == E_VRange._20V)
			{
				vRangeMax = LDevConst.OP20VRangeOut;
			}
			else if (this._vRangeSet == E_VRange._200V)
			{
				vRangeMax = LDevConst.OP200VRangeOut;
			}
			else
			{
				vRangeMax = 0.0d;
			}

			if ((Math.Abs(V) >= vRangeMax) && (V >= 0))
			{
				V = vRangeMax;
			}

			if ((Math.Abs(V) > vRangeMax) && (V < 0))
			{
				V = 0.0 - vRangeMax;
			}

			#endregion

			#region >> (02) 由輸入V, 查表內插設定 _bCMPA & _bCMPAHR & _bCMP_ex[0] & _bCMP_ex[1] & titalMEP

			if (V >= 0.0d)
			{
				memVFStartIndex = LDevConst.memTableVFP[index_VRange << 2];			// index = 0, 4, 8, 	=>> +6V, +20V, +200V,
				segCount = LDevConst.memTableVFP[1] - LDevConst.memTableVFP[0];
				volt_Add_CMP_ex = V + (double)LDevConst.CMP_ex_RefVolt;				// 對應輸入的 VRange <6V, <20V, <200V 實際 OP output volt 量化的過程是 realVolt < 10V, <24V <216V
																					// 可滿足， 3V 的額外變動電壓。
			}
			else
			{
				memVFStartIndex = LDevConst.memTableVFP[(index_VRange << 2) + 2];		// index = 2, 6, 10,	=>> -6A, -20V, -200V,
				memFirtRight = LDevConst.memTableVFP[index_VRange << 2];			// index = 0, 4, 8,
				segCount = LDevConst.memTableVFP[3] - LDevConst.memTableVFP[2];
				volt_Add_CMP_ex = V - (double)LDevConst.CMP_ex_RefVolt; 
			}
			//---------------------------------------------------------------------------------------------------
			// 當輸入是正電壓時 ( V>=0 )，用校正表的 VF_Table 的正值電壓區段,使用CMP的範圍為 0x1980 ~ 0x30AF , 
			// 計算出驅動 ePWM 的 CMPA 與 CMPAHR 的值,與補償的 CMP_ex 比例值
			//---------------------------------------------------------------------------------------------------
			if (V >= 0)
			{
				startA_Reg = (LDevConst.CMP_CENTER >> 8);	// start from 0x19
				for (int p = 0; p < segCount; p++)			// start from 0x1980, 0x1A80, ... ,0x3080, First Segemnt (0x1980,0x1A80) has the smallest positive value,
				{
					segMin = (double)_nMemory[memVFStartIndex + p] * vRangeMax / 32767.0;
					segMax = (double)_nMemory[memVFStartIndex + p + 1] * vRangeMax / 32767.0;

					if ((p == 0) && (V <= segMin))
					{
						this._bCMPA = (byte)startA_Reg;
						this._bCMPAHR = (byte)LDevConst.HR_REG_CENTER;
					}
					else if ((segMin < V) && (V <= segMax))
					{
						if (segMin != segMax)
						{
							cmpIndex = Convert.ToUInt16((V - segMin) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);
						}
						else
						{
							cmpIndex = 0;
						}

						if (cmpIndex <= (LDevConst.HR_REG_MAX - LDevConst.HR_REG_CENTER + 1))				// Left Segment, <=B0 (index=48),  重疊點 0xB1, index = 49
						{
							this._bCMPA = (byte)(startA_Reg + p);
							this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER + cmpIndex);
						}
						else																				// Right Segment, index = 49 ==> 4F
						{
							this._bCMPA = (byte)(startA_Reg + p + 1);
							//this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER - (LDevConst.HR_REG_LENGTH - cmpIndex + 1 ));		// Gilbert
							this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER - (LDevConst.HR_REG_LENGTH - cmpIndex));				// Jack
						}

						//if ( cmpIndex ==LDevConst.HR_REG_LENGTH )
						//{
						//    this.this._bCMPA = (byte)(startA_Reg + p + 1);	
						//    this._bCMPAHR = (byte)LDevConst.HR_REG_CENTER;
						//}
						break;
					}

				}

				//--------------------------------------------------------------------------------------
				// Using ( volt_Add_CMP_exV > 0 ) to find the ex_CMPA and _exCMPAHR
				// Calculate the total MEP betwee the V and volt_Add_CMP_exV
				//--------------------------------------------------------------------------------------	
				for (int m = 0; m < segCount; m++)			// start from 0x1980, 0x1A80, ... ,0x3080, First Segemnt (0x1980,0x1A80) has the smallest positive value,
				{
					segMin = (double)_nMemory[memVFStartIndex + m] * vRangeMax / 32767.0;
					segMax = (double)_nMemory[memVFStartIndex + m + 1] * vRangeMax / 32767.0;

					if ((segMin < volt_Add_CMP_ex) && (volt_Add_CMP_ex <= segMax))					// 因為 volt_Add_CMP_ex = V + 3f 所以一定是後續才被查到，一定先有 _byCMPA, _byCMPAHR 
					{

						if (segMin != segMax)
						{
							CMP_ex_Index = Convert.ToUInt16((volt_Add_CMP_ex - segMin) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);
						}
						else
						{
							CMP_ex_Index = 0;
						}

						if (CMP_ex_Index <= (LDevConst.HR_REG_MAX - LDevConst.HR_REG_CENTER + 1))						// Left Segment, <=B0 (index=48),  重疊點 0xB1, index = 49
						{
							bCMP_ex_A = (byte)(startA_Reg + m);
							bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER + CMP_ex_Index);
						}
						else																							// Right Segment, index = 49 ==> 4F
						{
							bCMP_ex_A = (byte)(startA_Reg + m + 1);
							//bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER - (LDevConst.HR_REG_LENGTH -CMP_ex_Index + 1 )) ;		// Gilbert
							bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER - (LDevConst.HR_REG_LENGTH - CMP_ex_Index));				// Jack

						}

						//if ( CMP_ex_Index == LDevConst.HR_REG_LENGTH )
						//{
						//    bCMP_ex_A = (byte)(startA_Reg + m + 1);
						//    bCMP_ex_HR = (byte)LDevConst.HR_REG_CENTER;
						//}


						totalMEP = (bCMP_ex_A - this._bCMPA) * LDevConst.HR_REG_LENGTH + (bCMP_ex_HR - this._bCMPAHR);		// ( _byCMPA < bCMP_ex_A ), calculate the total MEP steps
						totalMEP_temp = totalMEP;

						totalMEP_temp = totalMEP_temp * 32767 / _nMemory[LDevConst.memTableVSN[1]];			// 3V 比例與 CMP 比例，用Q15計算，bit0 = 0，閃避 0xFF，忽略小一個bit的誤差。

						this._byCMP_ex[0] = Convert.ToByte(totalMEP_temp & 0xFE);									// _byCMP_ex low byte  FE = 1111 1110

                        this._byCMP_ex[1] = Convert.ToByte((totalMEP_temp >> 8) & 0x7F);							// _byCMP_ex high byte 7F = 0111 1111

						break;
					}
				}
			}
			//---------------------------------------------------------------------------------------------------
			// 當輸入是負電壓時 ( V<0 )，用校正表的 VF_Table 的負值電壓區段,使用CMP的範圍為 0x1880 ~ 0x024F , 
			// 計算出驅動 ePWM 的 CMPA 與 CMPAHR 的值,與補償的 CMP_ex 比例值
			// ** 注意 : 負值的第一個區間 0x1880 ~ 0x197F 的特殊性
			//---------------------------------------------------------------------------------------------------
			else
			{
				startA_Reg = ((LDevConst.CMP_CENTER - LDevConst.A_REG_STEP) >> 8);			// start from 0x18
				for (int p = -1; p < segCount; p++)											// start from 0x1980, 0x1A80, ... ,0x3080, First Segemnt (0x1980,0x1A80) has the smallest positive value,
				{
					if (p == -1)		// for 0x1880 ~ 0x197F, 少校正一個點。 用 p =-1 表達。
					{
						segMin = 0.0 - (double)_nMemory[memVFStartIndex] * vRangeMax / 32767.0;
						segMax = (double)_nMemory[memFirtRight] * vRangeMax / 32767.0;
					}
					else
					{
						segMin = 0.0 - (double)_nMemory[memVFStartIndex + p + 1] * vRangeMax / 32767.0;
						segMax = 0.0 - (double)_nMemory[memVFStartIndex + p] * vRangeMax / 32767.0;
					}

					if ((segMin <= V) && (V < segMax))
					{
						if (segMin != segMax)
						{
							// cmpIndex = Convert.ToUInt16((segMax - V) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);			//	Gilbert
							cmpIndex = Convert.ToUInt16((V - segMax) / (segMin - segMax) * LDevConst.HR_REG_LENGTH);
						}
						else
						{
							cmpIndex = 0;
						}


						if (cmpIndex <= (LDevConst.HR_REG_CENTER - LDevConst.HR_REG_MIN))		// Rigth Segment, // <=49, 由前一個 Segment 的 0x1880 起算， index=49 => 0x194F
						{
							if (p == -1)									// for 0x1880 ~ 0x1980, 0x1980 的校正點在另一個區段
							{
								this._bCMPA = (byte)(startA_Reg + 1);		// 0x18 + 1 ;				
							}
							else
							{
								this._bCMPA = (byte)(startA_Reg - p);
							}
							this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER - cmpIndex);
						}
						else																		// Left Segment,
						{
							if (p == -1)
							{
								this._bCMPA = (byte)(startA_Reg);
							}
							else
							{
								this._bCMPA = (byte)(startA_Reg - p - 1);		// 0x1980 => 0x19 - p - 1
							}

							//this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER +  LDevConst.HR_REG_LENGTH - cmpIndex + 1  );	// Gilbert			
							this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER + LDevConst.HR_REG_LENGTH - cmpIndex);			// Jack
						}

						break;
					}
				}

				//--------------------------------------------------------------------------------------
				// Using ( volt_Add_CMP_exV < 0 ) to find the ex_CMPA and _exCMPAHR
				// Calculate the total MEP betwee the V and volt_Add_CMP_exV
				//--------------------------------------------------------------------------------------	
				for (int m = -1; m < segCount; m++)										// start from 0x1980, 0x1A80, ... ,0x3080, First Segemnt (0x1980,0x1A80) has the smallest positive value,
				{
					if (m == -1)		// for 0x1880 ~ 0x197F, 少校正一個點。 用 p =-1 表達。
					{
						segMin = 0.0 - (double)_nMemory[memVFStartIndex] * vRangeMax / 32767.0;
						segMax = (double)_nMemory[memFirtRight] * vRangeMax / 32767.0;
					}
					else
					{
						segMin = 0.0 - (double)_nMemory[memVFStartIndex + m + 1] * vRangeMax / 32767.0;
						segMax = 0.0 - (double)_nMemory[memVFStartIndex + m] * vRangeMax / 32767.0;
					}


					if ((segMin <= volt_Add_CMP_ex) && (volt_Add_CMP_ex < segMax))				// 因為 volt_Add_CMP_ex = V - 3f 所以一定是後續才被查到，一定先有 _byCMPA, _byCMPAHR 
					{

						if (segMin != segMax)
						{
							CMP_ex_Index = Convert.ToUInt16((segMax - volt_Add_CMP_ex) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);
						}
						else
						{
							CMP_ex_Index = 0;
						}


						if (CMP_ex_Index <= (LDevConst.HR_REG_CENTER - LDevConst.HR_REG_MIN))		// Rigth Segment, // <=49, 由前一個 Segment 的 0x1880 起算， index=49 => 0x194F
						{
							if (m == -1)
							{
								bCMP_ex_A = (byte)(startA_Reg + 1);
							}
							else
							{
								bCMP_ex_A = (byte)(startA_Reg - m);
							}
							bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER - CMP_ex_Index);
						}
						else																			// Left Segment,
						{
							if (m == -1)
							{
								bCMP_ex_A = (byte)(startA_Reg);
							}
							else
							{
								bCMP_ex_A = (byte)(startA_Reg - m - 1);
							}
							//bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER + LDevConst.HR_REG_LENGTH - CMP_ex_Index + 1);		// Gilbert
							bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER + LDevConst.HR_REG_LENGTH - CMP_ex_Index);			// Jacke
						}

						totalMEP = (this._bCMPA - bCMP_ex_A) * LDevConst.HR_REG_LENGTH + (this._bCMPAHR - bCMP_ex_HR);				// ( bCMP_ex_A < _byCPMA) // All CMPA transfer to CMPAHR ( total MEP )
						totalMEP_temp = totalMEP;

						totalMEP_temp = totalMEP_temp * 32767 / _nMemory[LDevConst.memTableVSN[3]];			// 3V 比例與 CMP 比例，用Q15計算，bit0 = 0，閃避 0xFF，忽略小一個bit的誤差。

						this._byCMP_ex[0] = Convert.ToByte(totalMEP_temp & 0xFE);									// _byCMP_ex low byte  FE = 1111 1110

						this._byCMP_ex[1] = Convert.ToByte((totalMEP_temp / 256) & 0x7F);							// _byCMP_ex high byte 7F = 0111 1111

						break;
					}
				}
			}

			#endregion

			////================================
			//bCMP_ex_LOW = _byCMP_ex[0];
			//bCMP_ex_HIGH = _byCMP_ex[1];

		}

        private void Cmd_V_Convert02(double V, bool isAutoVRange)
        {
            int memVFStartIndex = -1, memFirtRight = -1;
            int cmpIndex = 0, CMP_ex_Index;
            double volt_Add_CMP_ex = 3.0d;
            int totalMEP_temp = 0;
            int totalMEP = 0;

            double segMin = 0.0;
            double segMax = 0.0;

            int startA_Reg = (LDevConst.CMP_CENTER >> 8);  // 0x19
            byte bCMP_ex_A;
            byte bCMP_ex_HR;
            int segCount = 0;
            double vRangeMax = 0.0;
            int cmpLeft = LDevConst.CMP_CENTER;              // 0x1980
            int cmpRight = cmpLeft + LDevConst.A_REG_STEP;   // 0x1A80
            int index_VRange = -1;

            this._bCMPA = (byte)startA_Reg;
            this._bCMPAHR = LDevConst.HR_REG_CENTER;         // 0x80

            #region >> (01) 由輸入V, 調整數據與單位，對應內部定義的 VRange 檔位

            if (V > 205d) { V = 205d; }	// 涵蓋了 200V + CMP_ex_volt = 3V = 203V < 205V
            if (V < -205d) { V = -205d; }

            if (isAutoVRange == true)
            {
                if (Math.Abs(V) <= 6.001d)
                {
                    this._vRangeSet = E_VRange._6V;
                }
                else if ((6.001d < Math.Abs(V)) && (Math.Abs(V) <= 20.001d))
                {
                    this._vRangeSet = E_VRange._20V;
                }
                else
                {
                    this._vRangeSet = E_VRange._200V;
                }
            }

            index_VRange = Array.IndexOf(Enum.GetValues(typeof(E_VRange)), this._vRangeSet);

            this._bHW_DevRange = SetIVRange2HWRange(this._iRangeSet, this._vRangeSet);							// 依據設定的 VRange 或是自動找到的 IRange 檔位, 設定 HW_Range

            if (this._vRangeSet == E_VRange._6V)
            {
                vRangeMax = LDevConst.OP6VRangeOut;
            }
            else if (this._vRangeSet == E_VRange._20V)
            {
                vRangeMax = LDevConst.OP20VRangeOut;
            }
            else if (this._vRangeSet == E_VRange._200V)
            {
                vRangeMax = LDevConst.OP200VRangeOut;
            }
            else
            {
                vRangeMax = 0.0d;
            }

            if ((Math.Abs(V) >= vRangeMax) && (V >= 0))
            {
                V = vRangeMax;
            }

            if ((Math.Abs(V) > vRangeMax) && (V < 0))
            {
                V = 0.0 - vRangeMax;
            }

            #endregion

            #region >> (02) 由輸入V, 查表內插設定 _bCMPA & _bCMPAHR & _bCMP_ex[0] & _bCMP_ex[1] & titalMEP

            if (V >= 0.0d)
            {
                memVFStartIndex = LDevConst.memTableVFP2[index_VRange << 2];			// index = 0, 4, 8, 	=>> +6V, +20V, +200V,
                segCount = LDevConst.memTableVFP2[1] - LDevConst.memTableVFP2[0];
                volt_Add_CMP_ex = V + (double)LDevConst.CMP_ex_RefVolt;				// 對應輸入的 VRange <6V, <20V, <200V 實際 OP output volt 量化的過程是 realVolt < 10V, <24V <216V
                // 可滿足， 3V 的額外變動電壓。
            }
            else
            {
                memVFStartIndex = LDevConst.memTableVFP2[(index_VRange << 2) + 2];		// index = 2, 6, 10,	=>> -6A, -20V, -200V,
                memFirtRight = LDevConst.memTableVFP[index_VRange << 2];			// index = 0, 4, 8,
                segCount = LDevConst.memTableVFP2[3] - LDevConst.memTableVFP2[2];
                volt_Add_CMP_ex = V - (double)LDevConst.CMP_ex_RefVolt;
            }


            //---------------------------------------------------------------------------------------------------
            // 當輸入是正電壓時 ( V>=0 )，用校正表的 VF_Table 的正值電壓區段,使用CMP的範圍為 0x1980 ~ 0x30AF , 
            // 計算出驅動 ePWM 的 CMPA 與 CMPAHR 的值,與補償的 CMP_ex 比例值
            //---------------------------------------------------------------------------------------------------
            if (V >= 0)
            {
                startA_Reg = (LDevConst.CMP_CENTER >> 8) + 1;	// start from 0x19

                for (int p = 1; p < segCount; p+=2)			// start from 0x1980, 0x1A80, ... ,0x3080, First Segemnt (0x1980,0x1A80) has the smallest positive value, [Roy, it should be  p <=  segCount]
                {
                    segMin = (double)_nMemory[memVFStartIndex + p] * vRangeMax / 32767.0;   // 4F
                    segMax = (double)_nMemory[memVFStartIndex + p + 1] * vRangeMax / 32767.0;  //AF

                    if ((p == 0) && (V <= (segMin + segMax) / 2))
                    {
                        this._bCMPA = (byte)startA_Reg;
                        this._bCMPAHR = (byte)LDevConst.HR_REG_CENTER;
                        break;
                    }
                    else if ((segMin < V) && (V <= segMax))
                    {
                        if (segMin != segMax)
                        {
                            cmpIndex = Convert.ToUInt16((V - segMin) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);                  
                        }
                        else
                        {
                            cmpIndex = 0;
                        }

                        this._bCMPA = (byte)(startA_Reg);

                        this._bCMPAHR = (byte)(LDevConst.HR_REG_MIN + cmpIndex);

                        break;
                    }

                    startA_Reg++;

                }

                //--------------------------------------------------------------------------------------
                // Using ( volt_Add_CMP_exV > 0 ) to find the ex_CMPA and _exCMPAHR
                // Calculate the total MEP betwee the V and volt_Add_CMP_exV
                //--------------------------------------------------------------------------------------	
                #region >>> CMP_EX <<<

                startA_Reg = (LDevConst.CMP_CENTER >> 8) + 1;	// start from 0x19

                for (int m = 1; m < segCount; m+=2)			// start from 0x1980, 0x1A80, ... ,0x3080, First Segemnt (0x1980,0x1A80) has the smallest positive value,
                {
                    segMin = (double)_nMemory[memVFStartIndex + m] * vRangeMax / 32767.0;
                    segMax = (double)_nMemory[memVFStartIndex + m + 1] * vRangeMax / 32767.0;

                    if ((segMin < volt_Add_CMP_ex) && (volt_Add_CMP_ex <= segMax))					// 因為 volt_Add_CMP_ex = V + 3f 所以一定是後續才被查到，一定先有 _byCMPA, _byCMPAHR 
                    {

                        if (segMin != segMax)
                        {
                            CMP_ex_Index = Convert.ToUInt16((volt_Add_CMP_ex - segMin) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);
                        }
                        else
                        {
                            CMP_ex_Index = 0;
                        }

                        bCMP_ex_A = (byte)(startA_Reg);
                        bCMP_ex_HR = (byte)(LDevConst.HR_REG_MIN + CMP_ex_Index);

                        //if (CMP_ex_Index <= (LDevConst.HR_REG_MAX - LDevConst.HR_REG_CENTER + 1))						// Left Segment, <=B0 (index=48),  重疊點 0xB1, index = 49
                        //{
                        //    bCMP_ex_A = (byte)(startA_Reg);
                        //    bCMP_ex_HR = (byte)(LDevConst.HR_REG_MIN + CMP_ex_Index);
                        //}
                        //else																							// Right Segment, index = 49 ==> 4F
                        //{
                        //    bCMP_ex_A = (byte)(startA_Reg + 1);
                        //    //bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER - (LDevConst.HR_REG_LENGTH -CMP_ex_Index + 1 )) ;		// Gilbert
                        //    bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER - (LDevConst.HR_REG_LENGTH - CMP_ex_Index));				// Jack

                        //}

                        //if ( CMP_ex_Index == LDevConst.HR_REG_LENGTH )
                        //{
                        //    bCMP_ex_A = (byte)(startA_Reg + m + 1);
                        //    bCMP_ex_HR = (byte)LDevConst.HR_REG_CENTER;
                        //}


                        totalMEP = (bCMP_ex_A - this._bCMPA) * LDevConst.HR_REG_LENGTH + (bCMP_ex_HR - this._bCMPAHR);		// ( _byCMPA < bCMP_ex_A ), calculate the total MEP steps
                   
                        totalMEP_temp = totalMEP;

                        int count = _nMemory[LDevConst.memTableVSN[1]];

                        totalMEP_temp = totalMEP * 32767 / count;			// 3V 比例與 CMP 比例，用Q15計算，bit0 = 0，閃避 0xFF，忽略小一個bit的誤差。

                        this._byCMP_ex[0] = Convert.ToByte(totalMEP_temp & 0xFE);									// _byCMP_ex low byte  FE = 1111 1110

                        this._byCMP_ex[1] = Convert.ToByte((totalMEP_temp >> 8) & 0x7F);							// _byCMP_ex high byte 7F = 0111 1111

                        break;
                    }

                    startA_Reg++;

                }
                #endregion
            }
            //---------------------------------------------------------------------------------------------------
            // 當輸入是負電壓時 ( V<0 )，用校正表的 VF_Table 的負值電壓區段,使用CMP的範圍為 0x1880 ~ 0x024F , 
            // 計算出驅動 ePWM 的 CMPA 與 CMPAHR 的值,與補償的 CMP_ex 比例值
            // ** 注意 : 負值的第一個區間 0x1880 ~ 0x197F 的特殊性
            //---------------------------------------------------------------------------------------------------
            else
            {
                startA_Reg = ((LDevConst.CMP_CENTER - LDevConst.A_REG_STEP) >> 8);			// start from 0x18

                for (int p = 1; p < segCount; p += 2)											// start from 0x1980, 0x1A80, ... ,0x3080, First Segemnt (0x1980,0x1A80) has the smallest positive value,
                {
                    segMin = 0.0 - (double)_nMemory[memVFStartIndex + p + 1] * vRangeMax / 32767.0;
                    segMax = 0.0 - (double)_nMemory[memVFStartIndex + p] * vRangeMax / 32767.0;

                    if ((segMin <= V) && (V < segMax))
                    {
                        if (segMin != segMax)
                        {
                            //cmpIndex = Convert.ToUInt16((V - segMin) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);
                            cmpIndex = Convert.ToUInt16((segMax - V) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);	
                        }
                        else
                        {
                            cmpIndex = 0;
                        }

                        this._bCMPA = (byte)(startA_Reg);

                        this._bCMPAHR = (byte)(LDevConst.HR_REG_MAX - cmpIndex);

                        break;

                        //if (segMin != segMax)
                        //{
                        //    // cmpIndex = Convert.ToUInt16((segMax - V) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);			//	Gilbert
                        //    cmpIndex = Convert.ToUInt16((V - segMax) / (segMin - segMax) * LDevConst.HR_REG_LENGTH);
                        //}
                        //else
                        //{
                        //    cmpIndex = 0;
                        //}


                        //if (cmpIndex <= (LDevConst.HR_REG_CENTER - LDevConst.HR_REG_MIN))		// Rigth Segment, // <=49, 由前一個 Segment 的 0x1880 起算， index=49 => 0x194F
                        //{
                        //    if (p == -1)									// for 0x1880 ~ 0x1980, 0x1980 的校正點在另一個區段
                        //    {
                        //        this._bCMPA = (byte)(startA_Reg + 1);		// 0x18 + 1 ;				
                        //    }
                        //    else
                        //    {
                        //        this._bCMPA = (byte)(startA_Reg - p);
                        //    }
                        //    this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER - cmpIndex);
                        //}
                        //else																		// Left Segment,
                        //{
                        //    this._bCMPA = (byte)(startA_Reg);

                        //    //this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER +  LDevConst.HR_REG_LENGTH - cmpIndex + 1  );	// Gilbert			
                        //    this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER + LDevConst.HR_REG_LENGTH - cmpIndex);			// Jack
                        //}

                        //break;
                    }

                    startA_Reg--;
                }

                //--------------------------------------------------------------------------------------
                // Using ( volt_Add_CMP_exV < 0 ) to find the ex_CMPA and _exCMPAHR
                // Calculate the total MEP betwee the V and volt_Add_CMP_exV
                //--------------------------------------------------------------------------------------	
                #region >>> CMP_EX <<<

                startA_Reg = ((LDevConst.CMP_CENTER - LDevConst.A_REG_STEP) >> 8);

                for (int m = 1; m < segCount; m += 2)										// start from 0x1980, 0x1A80, ... ,0x3080, First Segemnt (0x1980,0x1A80) has the smallest positive value,
                {
                    segMin = 0.0 - (double)_nMemory[memVFStartIndex + m + 1] * vRangeMax / 32767.0;
                    segMax = 0.0 - (double)_nMemory[memVFStartIndex + m] * vRangeMax / 32767.0;

                    if ((segMin <= volt_Add_CMP_ex) && (volt_Add_CMP_ex < segMax))				// 因為 volt_Add_CMP_ex = V - 3f 所以一定是後續才被查到，一定先有 _byCMPA, _byCMPAHR 
                    {
                        if (segMin != segMax)
                        {
                            CMP_ex_Index = Convert.ToUInt16((segMax - volt_Add_CMP_ex) / (segMax - segMin) * LDevConst.HR_REG_LENGTH);
                
                        }
                        else
                        {
                            CMP_ex_Index = 0;
                        }

                        bCMP_ex_A = (byte)(startA_Reg);
                        bCMP_ex_HR = (byte)(LDevConst.HR_REG_MAX- CMP_ex_Index);

                        //if (CMP_ex_Index <= (LDevConst.HR_REG_CENTER - LDevConst.HR_REG_MIN))		// Rigth Segment, // <=49, 由前一個 Segment 的 0x1880 起算， index=49 => 0x194F
                        //{
                        //    if (m == -1)
                        //    {
                        //        bCMP_ex_A = (byte)(startA_Reg + 1);
                        //    }
                        //    else
                        //    {
                        //        bCMP_ex_A = (byte)(startA_Reg - m);
                        //    }
                        //    bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER - CMP_ex_Index);
                        //}
                        //else																			// Left Segment,
                        //{
                        //    if (m == -1)
                        //    {
                        //        bCMP_ex_A = (byte)(startA_Reg);
                        //    }
                        //    else
                        //    {
                        //        bCMP_ex_A = (byte)(startA_Reg - m - 1);
                        //    }
                        //    //bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER + LDevConst.HR_REG_LENGTH - CMP_ex_Index + 1);		// Gilbert
                        //    bCMP_ex_A = (byte)(startA_Reg);
                        //    bCMP_ex_HR = (byte)(LDevConst.HR_REG_CENTER + LDevConst.HR_REG_LENGTH - CMP_ex_Index);			// Jacke
                        //}

                        totalMEP = (this._bCMPA - bCMP_ex_A) * LDevConst.HR_REG_LENGTH + (this._bCMPAHR - bCMP_ex_HR);				// ( bCMP_ex_A < _byCPMA) // All CMPA transfer to CMPAHR ( total MEP )
                        totalMEP_temp = totalMEP;

                        totalMEP_temp = totalMEP_temp * 32767 / _nMemory[LDevConst.memTableVSN[3]];			// 3V 比例與 CMP 比例，用Q15計算，bit0 = 0，閃避 0xFF，忽略小一個bit的誤差。

                        this._byCMP_ex[0] = Convert.ToByte(totalMEP_temp & 0xFE);									// _byCMP_ex low byte  FE = 1111 1110

                        this._byCMP_ex[1] = Convert.ToByte((totalMEP_temp / 256) & 0x7F);							// _byCMP_ex high byte 7F = 0111 1111

                        break;
                    }

                    startA_Reg--;
                }

                #endregion
            }

            #endregion

            ////================================
            //bCMP_ex_LOW = _byCMP_ex[0];
            //bCMP_ex_HIGH = _byCMP_ex[1];

        }				

		private void cal_2Wire_New(byte bNowHWRange, int CMPcmd, byte masterGL,UInt16 AD0, UInt16 readBackCMP, UInt16 AD2)
		{
			//------------------------------------------------------------------------------------------------------------------------
			// cal_2Wire() is for 2-wire applicaion, OPEN Loop calibration 
			// readBackCMP : 指的應該是動態改變的驅動值 AD1C = CMP = CMPA_CMPAHR, ==> look up VFP_Table to calculate the voltage
			//------------------------------------------------------------------------------------------------------------------------
			double VSP = 0, VSN = 0, VSD = 0;
			double IFN = 0;

			IFN = this.calcIFN_ByAD0(bNowHWRange, CMPcmd, masterGL, AD0);
			this._rtnValue[0] = IFN;	// 單位已經轉成 "A"

			//---------------------------------------------------------------------------
			// VS+ Positive, AD1, VS_Anode of 2 wire structure
			//---------------------------------------------------------------------------
			VSP = calcVFP_ByCMP(bNowHWRange, CMPcmd, readBackCMP);
			this._sRtnVS[0] = Convert.ToString(VSP);
			this._sRtnVS[1] = "V";
			this._rtnValue[1] = VSP;

			//---------------------------------------------------------------------------
			// VS- Negative, AD2, VS_Cathode
			//---------------------------------------------------------------------------
			VSN = calcVSN_ByAD2(CMPcmd, AD2);
			this._sRtnVS[2] = Convert.ToString(VSN);
			this._sRtnVS[3] = "V";
			this._rtnValue[2] = VSN;

			//---------------------------------------------------------------------------
			// Vdd = (VS+ - VS-) = ( VS_Anode - VS_Cathode ), 
			//---------------------------------------------------------------------------
			VSN = Math.Abs(VSN);		// Gilbert 下面運算只看正值, Why ?

			if ((VSP - VSN) >= 0)
			{
				VSD = VSP - VSN;
			}
			else
			{
				VSD = 0;
			}

			if (CMPcmd >= LDevConst.CMP_CENTER)							// if (readBackCMP >= LDevConst.CMP_CENTER) // Gilbert 怪怪的應該看起 4Wire 的作法
			{
				this._sRtnVS[4] = Convert.ToString(VSD);
				this._rtnValue[3] = VSD;
			}
			else
			{
				this._sRtnVS[4] = Convert.ToString(-VSD);
				this._rtnValue[3] = 0.0 - VSD;
			}
			this._sRtnVS[5] = "V";

			this._rtnValue[4] = -1.0;  // VDB for 4 Wire
			
		}

		private void cal_4Wire_New(byte bNowHWRange, int CMPcmd, byte masterGL, UInt16 AD0, UInt16 AD1, UInt16 AD2)
		{
			//------------------------------------------------------------------------------------------------------------------------
			// cal_4Wire() is for 4-wire applicaion, Close Loop calibration 
			// the AD1 = ADC transfer value, 0x0000 ~ 0x0FFF, ==> look up VF & VS table to calculate the voltage
			//------------------------------------------------------------------------------------------------------------------------

			double VSP = 0, VSN = 0, VDD_DUT = 0;
			double IFN = 0, VDB = 0;

			IFN = this.calcIFN_ByAD0(bNowHWRange, CMPcmd, masterGL, AD0);
			if (this._isRun4WireVDB == true)
			{
				VDB = this.calcVDB_ByIFN(bNowHWRange, CMPcmd, masterGL, IFN);
			}
			this._rtnValue[0] = IFN;	// 單位已經轉成 "A"
			this._rtnValue[4] = VDB;	// VDB for 4 Wire

			//---------------------------------------------------------------------------
			// VS+ Positive, AD1, VS_Anode of 4 wire structure
			// 已經是根據 CMPcmd 正負，回傳 VSP 的正負值
			//---------------------------------------------------------------------------

			VSP = this.calcVSP_ByAD1(bNowHWRange, CMPcmd, AD1);
			this._sRtnVS[0] = Convert.ToString(VSP);
			this._rtnValue[1] = VSP;
			this._sRtnVS[1] = "V";

			//---------------------------------------------------------------------------
			// VS- Negative, AD2, VS_Cathode
			// 已經是根據 CMPcmd 正負，回傳 VSN 的正負值
			//---------------------------------------------------------------------------
			VSN = calcVSN_ByAD2(CMPcmd, AD2);
			this._sRtnVS[2] = Convert.ToString(VSN);
			this._sRtnVS[3] = "V";
			this._rtnValue[2] = VSN;

			//---------------------------------------------------------------------------
			// Vdd = (VS+ - VS-) = ( VS_Anode - VS_Cathode ), 

			//---------------------------------------------------------------------------
			//VSP = VSP - VDB;
			//VDD_DUT = VSP - VSN;

			VDD_DUT = VSP - VSN - VDB;		// 都是已經帶有正負號的數字

			//if ((VSP - VSN) >= 0)
			//{
			//    VSD = VSP - VSN;
			//}
			//else
			//{
			//    VSD = VSN - VSP;
			//}

			if ( (CMPcmd >= LDevConst.CMP_CENTER) && ( VDD_DUT < 0 ) )	// 補過頭了  // Gilbert 要不要留下一些跳動，可以再思考
			{
				VDD_DUT = 0;
			}
			
			if ( (CMPcmd < LDevConst.CMP_CENTER) && (VDD_DUT > 0 ))
			{
				VDD_DUT = 0;
			}


			this._sRtnVS[4] = Convert.ToString(VDD_DUT);
			this._sRtnVS[5] = "V";
			this._rtnValue[3] = VDD_DUT;		

		}

		private void runSingleSequence_New()
		{
			this.Message = "";

			if ((this._isMultiDriveState == true) || (this._isMultiCurveState == true))
				return;

			try
			{
				_seqIndex = 0;

				_byUSBTx[0] = 0;
				_byUSBTx[1] = 0;
				_byUSBTx[2] = SOF;
				_byUSBTx[3] = (byte)ERegCmd.SetSequence; //0x50
				_byUSBTx[4] = 0;
				_byUSBTx[5] = this._bHW_DevRange;
				_byUSBTx[6] = this._bCMPA;
				_byUSBTx[7] = this._bCMPAHR;
				_byUSBTx[8] = this._bLimitCurrLow;                               // 0xFE;   low byte
				_byUSBTx[9] = this._bLimitCurrHigh;                              // 0x1F;   high byte
				_byUSBTx[10] = this._bTimeCount;

				if ( this._isCalcCMP_ex == false)
				{
					_byCMP_ex[0] = 0;
					_byCMP_ex[1] = 0;
				}

				_byUSBTx[11] = _byCMP_ex[0];
				_byUSBTx[12] = _byCMP_ex[1];
				_byUSBTx[13] = 0xFF;
				_byUSBTx[14] = (byte)ERegCmd.SetSequence; //0x50
				_byUSBTx[15] = 1;
				_byUSBTx[16] = this._bExecMode;
				_byUSBTx[17] = 0xFF;

                if (this.IsLog)
                {
                    Console.WriteLine(string.Format("[LDT3A DLL], Cmd SD {0}, vRng = {1}, iRng = {2}, Rng = {3}, CMPA = {4}, HR = {5}, I_L = {6}, I_H = {7}, Time = {8}, Ex_L = {9}, Ex_H = {10}",
                                                          LDT3A200USB.PortName, _vRangeSet.ToString(), _iRangeSet.ToString(), this._bHW_DevRange, _bCMPA, _bCMPAHR, _bLimitCurrLow, _bLimitCurrHigh, _bTimeCount, _byCMP_ex[0], _byCMP_ex[1]));

                }

				this.LDT3A200USB.Write(_byUSBTx, 0, 18);

				_seqHWRange[0] = this._bHW_DevRange;					// bHW_Range
				_seqCMP[0] = (this._bCMPA << 8) + this._bCMPAHR;		// CMP
				_seqMasterGL[0] = this._bLimitCurrHigh;					// Limit high byte = MasterGainLoop

				int USBbytes = this.LDT3A200USB.BytesToRead;
				System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
				this.loopTimer = new System.Threading.Timer(TimerDelegate, null, this._bTimeCount + 5000, Timeout.Infinite);

				while (USBbytes < 16)
				{
					USBbytes = this.LDT3A200USB.BytesToRead;
					Thread.Sleep(0);
					if (this._isOverTime == true)
					{
						this.Message = this.Message + "timeout\n";
						break;
					}
				}
				this._isOverTime = false;
				this.loopTimer.Dispose();

				byte[] _byUSBRx = new byte[16];

				this.LDT3A200USB.Read(_byUSBRx, 0, 16);
				if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
				{
					this.Message = this.Message + "communication error\n";
				}

				UInt16 AD0 = 0, AD1 = 0, AD2 = 0;

				AD0 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[10]) + (Convert.ToUInt16(_byUSBRx[11]) << 8 ));             // F/W 值 AD0, IF, F-, Current
				AD1 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[12]) + (Convert.ToUInt16(_byUSBRx[13]) << 8 ));             // F/W 值 AD1, VS_Anode, S+
				AD2 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[14]) + (Convert.ToUInt16(_byUSBRx[15]) << 8 ));             // F/W 值 AD2, VS_Cathode, VN, S-
				//----------------------------------------------------------------------------------------------------------------------------------------
				// 由設定的 hwRange, CMP, Limit_I 等參數，與讀取回來的 AD0, AD1, AD2, AD1C (真正驅動的 CMPA值) 等數據
				// 透過校正時建立的資料表，對應回查反算真正的數據，得到 IF, VSP (VS_Anode), VSN (VS_Cathode), VSD = VSP - VSN
				// 注意 : (01) cal_IF 是由 AD0 反算 IF 值
				//		  (02) cal_VS 是針對二線式，所以是 CMP (F+) 算 VSP, 以及 AD2 算 VSN 和 VSD, // Gilbert 都二線了，哪來的 VSN ???
				//		  (03) cal_4Wire 是針對四線式。所以是 AD1 算VSP, AD2 算 VSN，再算 VSD
				//
				//  ** 但是，因為  cal_4Wire 的計算過程會再補償一個電流流過的的電壓值，
				// 所以執行 cal_4Wire() 要跑 cal_IF(), 同時，在  cal_4Wire() 內會將計算過的 offset 變數，再設回 0.  // Gilbert 用法邏輯不佳
				//--------------------------------------------------------------------------------------------------------------------------------------------

				//_sRtnIF = cal_IF(sequence_Range[0], sequence_CMP[0], sequence_Limit_I1[0], AD0);

				this._execIRange = this.GetIRange(_seqHWRange[0], _seqMasterGL[0]);
				this._execVRange = (E_VRange)(_seqHWRange[0] & 0xE0);

				this._execData[0] = _seqHWRange[0];
				this._execData[1] = _seqCMP[0];
				this._execData[2] = (_byCMP_ex[1] << 8 ) + _byCMP_ex[0];
				this._execData[3] = ((this._bLimitCurrHigh << 8 ) + this._bLimitCurrLow);
				this._execData[4] = _seqMasterGL[0] & 0x80;
				this._execData[5] = AD0;
				this._execData[6] = AD1;
				this._execData[7] = AD2;
				this._execData[8] = this._nClampI;
                this._execData[9] = this._bCMPA;
                this._execData[10] = this._bCMPAHR;


				if ((this._bStatus & 0x04) == 0x04)		// 設定在四線式架構下，啟動 F+ / F- / S+ / S- 的架構，可作閉迴路
				{
					this.cal_4Wire_New(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0, AD1, AD2);
				}
				else
				{
					if (((_seqHWRange[0] & 0x60) == 0x60) || ((_seqHWRange[0] & 0x40) == 0x40))						// 0x40 = vRange = 6V ; 0x60 = vRange = 20V
					{
						// _sRtnVS = this.cal_2Wire(sequence_Range[0], sequence_CMP[0], AD1, AD2);
						this.cal_2Wire_New(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0, 0, AD2);		// 用2線式的 cal_2Wire() 計算， 並沒有 AD1 的數據 ? // Gilbert 不對吧 !!	
					}
					else
					{
						this.cal_4Wire_New(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0, AD1, AD2);
					}
				}

				this._sMeasureIV[0] = _sRtnIF[0];
				this._sMeasureIV[1] = _sRtnIF[1];
				this._sMeasureIV[2] = _sRtnVS[0];
				this._sMeasureIV[3] = _sRtnVS[1];
				this._sMeasureIV[4] = _sRtnVS[2];
				this._sMeasureIV[5] = _sRtnVS[3];
				this._sMeasureIV[6] = _sRtnVS[4];   //  S/W   LDT3A_measure
				this._sMeasureIV[7] = _sRtnVS[5];
				this._sMeasureIV[8] = Message;

			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}		

		}

        private void TriggerSingleSequence()
        {
 			this.Message = "";

			if ((this._isMultiDriveState == true) || (this._isMultiCurveState == true))
				return;

            try
            {
                _seqIndex = 0;

                _byUSBTx[0] = 0;
                _byUSBTx[1] = 0;
                _byUSBTx[2] = SOF;
                _byUSBTx[3] = (byte)ERegCmd.SetSequence; //0x50
                _byUSBTx[4] = 0;
                _byUSBTx[5] = this._bHW_DevRange;
                _byUSBTx[6] = this._bCMPA;
                _byUSBTx[7] = this._bCMPAHR;
                _byUSBTx[8] = this._bLimitCurrLow;                               // 0xFE;   low byte
                _byUSBTx[9] = this._bLimitCurrHigh;                              // 0x1F;   high byte
                _byUSBTx[10] = this._bTimeCount;

                if (this._isCalcCMP_ex == false)
                {
                    _byCMP_ex[0] = 0;
                    _byCMP_ex[1] = 0;
                }

                _byUSBTx[11] = _byCMP_ex[0];
                _byUSBTx[12] = _byCMP_ex[1];
                _byUSBTx[13] = 0xFF;
                _byUSBTx[14] = (byte)ERegCmd.SetSequence; //0x50
                _byUSBTx[15] = 1;
                _byUSBTx[16] = this._bExecMode;
                _byUSBTx[17] = 0xFF;

                if (this.IsLog)
                {
                    Console.WriteLine(string.Format("[LDT3A DLL], Cmd {0}, vRng = {1}, iRng = {2}, Rng = {3}, CMPA = {4}, HR = {5}, I_L = {6}, I_H = {7}, Time = {8}, Ex_L = {9}, Ex_H = {10}",
                                                          LDT3A200USB.PortName, _vRangeSet.ToString(), _iRangeSet.ToString(), this._bHW_DevRange, _bCMPA, _bCMPAHR, _bLimitCurrLow, _bLimitCurrHigh, _bTimeCount, _byCMP_ex[0], _byCMP_ex[1]));

                }

                this.LDT3A200USB.Write(_byUSBTx, 0, 18);

                _seqHWRange[0] = this._bHW_DevRange;					// bHW_Range
                _seqCMP[0] = (this._bCMPA << 8) + this._bCMPAHR;		// CMP
                _seqMasterGL[0] = this._bLimitCurrHigh;					// Limit high byte = MasterGainLoop
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }	
        }

        private void AcquireSingleSequenceResult()
        {
            int USBbytes = this.LDT3A200USB.BytesToRead;
            System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
            this.loopTimer = new System.Threading.Timer(TimerDelegate, null, this._bTimeCount + 5000, Timeout.Infinite);

            while (USBbytes < 16)
            {
                USBbytes = this.LDT3A200USB.BytesToRead;
                Thread.Sleep(0);
                if (this._isOverTime == true)
                {
                    this.Message = this.Message + "timeout\n";
                    break;
                }
            }
            this._isOverTime = false;
            this.loopTimer.Dispose();

            byte[] _byUSBRx = new byte[16];

            this.LDT3A200USB.Read(_byUSBRx, 0, 16);
            if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
            {
                this.Message = this.Message + "communication error\n";
            }

            UInt16 AD0 = 0, AD1 = 0, AD2 = 0;

            AD0 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[10]) + (Convert.ToUInt16(_byUSBRx[11]) << 8));             // F/W 值 AD0, IF, F-, Current
            AD1 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[12]) + (Convert.ToUInt16(_byUSBRx[13]) << 8));             // F/W 值 AD1, VS_Anode, S+
            AD2 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[14]) + (Convert.ToUInt16(_byUSBRx[15]) << 8));             // F/W 值 AD2, VS_Cathode, VN, S-
            //----------------------------------------------------------------------------------------------------------------------------------------
            // 由設定的 hwRange, CMP, Limit_I 等參數，與讀取回來的 AD0, AD1, AD2, AD1C (真正驅動的 CMPA值) 等數據
            // 透過校正時建立的資料表，對應回查反算真正的數據，得到 IF, VSP (VS_Anode), VSN (VS_Cathode), VSD = VSP - VSN
            // 注意 : (01) cal_IF 是由 AD0 反算 IF 值
            //		  (02) cal_VS 是針對二線式，所以是 CMP (F+) 算 VSP, 以及 AD2 算 VSN 和 VSD, // Gilbert 都二線了，哪來的 VSN ???
            //		  (03) cal_4Wire 是針對四線式。所以是 AD1 算VSP, AD2 算 VSN，再算 VSD
            //
            //  ** 但是，因為  cal_4Wire 的計算過程會再補償一個電流流過的的電壓值，
            // 所以執行 cal_4Wire() 要跑 cal_IF(), 同時，在  cal_4Wire() 內會將計算過的 offset 變數，再設回 0.  // Gilbert 用法邏輯不佳
            //--------------------------------------------------------------------------------------------------------------------------------------------

            //_sRtnIF = cal_IF(sequence_Range[0], sequence_CMP[0], sequence_Limit_I1[0], AD0);

            this._execIRange = this.GetIRange(_seqHWRange[0], _seqMasterGL[0]);
            this._execVRange = (E_VRange)(_seqHWRange[0] & 0xE0);

            this._execData[0] = _seqHWRange[0];
            this._execData[1] = _seqCMP[0];
            this._execData[2] = (_byCMP_ex[1] << 8) + _byCMP_ex[0];
            this._execData[3] = ((this._bLimitCurrHigh << 8) + this._bLimitCurrLow);
            this._execData[4] = _seqMasterGL[0] & 0x80;
            this._execData[5] = AD0;
            this._execData[6] = AD1;
            this._execData[7] = AD2;
            this._execData[8] = this._nClampI;
            this._execData[9] = this._bCMPA;
            this._execData[10] = this._bCMPAHR;


            if ((this._bStatus & 0x04) == 0x04)		// 設定在四線式架構下，啟動 F+ / F- / S+ / S- 的架構，可作閉迴路
            {
                this.cal_4Wire_New(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0, AD1, AD2);
            }
            else
            {
                if (((_seqHWRange[0] & 0x60) == 0x60) || ((_seqHWRange[0] & 0x40) == 0x40))						// 0x40 = vRange = 6V ; 0x60 = vRange = 20V
                {
                    // _sRtnVS = this.cal_2Wire(sequence_Range[0], sequence_CMP[0], AD1, AD2);
                    this.cal_2Wire_New(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0, 0, AD2);		// 用2線式的 cal_2Wire() 計算， 並沒有 AD1 的數據 ? // Gilbert 不對吧 !!	
                }
                else
                {
                    this.cal_4Wire_New(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0, AD1, AD2);
                }
            }

            this._sMeasureIV[0] = _sRtnIF[0];
            this._sMeasureIV[1] = _sRtnIF[1];
            this._sMeasureIV[2] = _sRtnVS[0];
            this._sMeasureIV[3] = _sRtnVS[1];
            this._sMeasureIV[4] = _sRtnVS[2];
            this._sMeasureIV[5] = _sRtnVS[3];
            this._sMeasureIV[6] = _sRtnVS[4];   //  S/W   LDT3A_measure
            this._sMeasureIV[7] = _sRtnVS[5];
            this._sMeasureIV[8] = Message;

            if (this.IsLog)
            {
                Console.WriteLine(string.Format("[LDT3A DLL], Rtn {0}, AD0 = {1}, AD1 = {2}, AD2 = {3}, VSP = {4}, VSN = {5}, VDB = {6}, VDD = {7}, IFN = {8}",
                                                 LDT3A200USB.PortName, 
                                                 AD0.ToString(), AD1.ToString(), AD2.ToString(),
                                                 this._rtnValue[1].ToString(),  // VSP
                                                 this._rtnValue[2].ToString(),  // VSN
                                                 this._rtnValue[4].ToString(),  // VDB
                                                 this._rtnValue[3].ToString(),  // VDD
                                                 this._rtnValue[0].ToString()   // IFN
                                                 ));

            }
        }

		private void ReadCurveFromDev(uint channel, uint point)
		{
			this.Message = "";
			try
			{
				//if (this._isMultiDriveState == true)
				//{
				//    this.Multi_Getdata();
				//}
				//if (this._isMultiCurveState == true)
				//{
				//    this.multi_GetCurveData();
				//}

				//if (this._isMultiDriveState == false && this._isMultiCurveState == false)
				//{

				if (channel > 0xFE) { channel = 0xFE; }
				if (point < 1)		{ point = 1; }
				if (point > LDevConst.MAX_CURVE_POINTS) { point = LDevConst.MAX_CURVE_POINTS; }

				_byUSBTx[0] = 0;
				_byUSBTx[1] = 0;
				_byUSBTx[2] = SOF;
				_byUSBTx[3] = (byte)ERegCmd.ReadCurve;                     //0x43
				_byUSBTx[4] = (byte) channel;
				_byUSBTx[5] = (byte) (point & 0xFF);

				if (_byUSBTx[5] > 0)
				{
					_byUSBTx[5] = Convert.ToByte(_byUSBTx[5] - 1);		// Min point = 1, LowByte = 0x01 ~ 0xFF, ==> 0x00 ~ 0xFE
				}

				_byUSBTx[6] = (byte)((point >> 8 ) & 0xFF);				// MAX point= 2000 = 0x07D0
				_byUSBTx[7] = 0xFF;

				this.LDT3A200USB.Write(_byUSBTx, 0, 8);
                //---------------------------------------------------------------------------------------------------------------------------

				point = (uint)((_byUSBTx[6] * 256) + _byUSBTx[5] + 1) * 2 + 4;

				int USBbytes = this.LDT3A200USB.BytesToRead;
				System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
				this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

				while (USBbytes < point)
				{
					USBbytes = this.LDT3A200USB.BytesToRead;
					Thread.Sleep(0);
					if (this._isOverTime == true)
					{
						this.Message = this.Message + "timeout\n";
						break;
					}
				}
				this._isOverTime = false;
				this.loopTimer.Dispose();
				byte[] _byUSBRx = new byte[USBbytes];
				this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

				if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x43)
				{
					this.Message = this.Message + "communication error\n";
				}
				UInt16 temp = 0;

				point = (uint)((_byUSBTx[6] * 256) + _byUSBTx[5] + 1);
				temp = Convert.ToUInt16(_byUSBRx[4] + _byUSBRx[5] * 256);

				for (int index = 0; index < point; index++)
				{
					temp  = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);

					this._thy4ChRawData[channel][index] = temp;

					if (channel == 0)			// AD0 -> IFN
					{
						this._thy4ChData[channel][index] = this.calcIFN_ByAD0(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], _seqMasterGL[_seqIndex], temp);
					}
					else if (channel == 1)		// AD1 -> VSP
					{
						this._thy4ChData[channel][index] = this.calcVSP_ByAD1(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], temp);
					}
					else if (channel == 2)
					{
						this._thy4ChData[channel][index] = this.calcVSN_ByAD2(_seqCMP[_seqIndex], temp);
					}
					else if (channel == 3 )
					{
						this._thy4ChData[channel][index] = this.calcVFP_ByCMP(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], temp);
					}
					else
					{
						this._thy4ChData[channel][index] = 0;
					}				
				}				
			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}
		}

		#endregion

		#region >>> Public Methods 01 <<<

		public void Connect(string portName, out string outMessage)
        {
            string[] ports = SerialPort.GetPortNames();

            // string  s =string.Empty;

            //for (int i = 0; i < ports.Length; i++)
            //{
            //     s += ports[i]+",";                
            //}

            //Console.WriteLine("[LDT3ALib], Connect()" + s);

            Message = "";
            outMessage = "";
			
            foreach (string thePort in ports)
            {
                if (thePort == portName)
                {
                    this.LDT3A200USB = new SerialPort(thePort);

                    if (!LDT3A200USB.IsOpen)
                    {
                        try
                        {
                            this.LDT3A200USB.Open();
                            this.LDT3A200USB.ReadTimeout = 1;
                            this.LDT3A200USB.ReadExisting();
                            int USBbytes = LDT3A200USB.BytesToRead;
                            byte[] _byUSBRx = new byte[USBbytes];
                            this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

                            this.GetVersion();

                            if (this._sVersion[0] != "3")
                            {
                                this.Disconnect();
                                outMessage = "firmware _sVersion error";
                                break;
                            }

                            this.GetDeviceInfo();

                            this._bNPLC_Order = 3;

							this.SetNPLCToDev();

                            this._bTimeBase = 100;

                            this.SetTimeBaseToDev();

							this._parameter[0] = 248;
							this._parameter[1] = 200;
							this._parameter[2] = 30;
							this._parameter[3] = 254;
							this._parameter[4] = 30;
							//this._parameter[4] = 10;	// 20140707, thy
							this._parameter[5] = 128;
							this._parameter[6] = 254;	// 3;
							this._parameter[7] = 27;
							this._parameter[8] = 254;
							this._parameter[9] = 10;
							this._parameter[10] = 10;
							this._parameter[11] = 10;
							this._parameter[12] = 20;
							this._parameter[13] = 20;
							this._parameter[14] = 20;
							this._parameter[15] = 20;
							//this._parameter[9] = 254;
							//this._parameter[10] = 8;
							//this._parameter[11] = 254;
							//this._parameter[12] = 254;
							//this._parameter[13] = 93;
							//this._parameter[14] = 43;
							//this._parameter[15] = 16;
							this._parameter[16] = 18;
							this._parameter[17] = 32;
							this._parameter[18] = 0;		// 50;
							this._parameter[19] = 0;		// 25;
							this._parameter[20] = 150;
							this._parameter[21] = 150;
							this._parameter[22] = 100;
							this._parameter[23] = 254;
							this._parameter[24] = 70;
							this._parameter[25] = 150;
							this._parameter[26] = 254;
							this._parameter[27] = 254;
							this._parameter[28] = 254;
							this._parameter[29] = 0;
							this._parameter[30] = 8;
							this._parameter[31] = 254;
							this._parameter[32] = 94;
							this._parameter[33] = 93;
							this._parameter[34] = 30;
							this._parameter[35] = 16;
							this._parameter[36] = 32;
							this._parameter[37] = 32;
							this._parameter[38] = 50;
							this._parameter[39] = 25;

                            this.SetParameterToDev();
                            this._bStatus = Convert.ToByte(this._bStatus | 0x06);
                            this.SetStatusToDev();
                        }
                        catch (Exception LDT3A200USB_e)
                        {
                            outMessage = LDT3A200USB_e.ToString();
                            this.LDT3A200USB = null;
                        }
                    }
                }
            }
            this.Message = outMessage;
        }

        public string Disconnect()
        {
            string[] HardwareList = hwh.GetAll();
            string[] MPI_SUB = new string[1];
            this.Message = "";
            MPI_SUB[0] = "MPI USB serial port";

            try
            {
                this.SetStatus(0);
                this.LDT3A200USB.Close();
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
            
			try
            {
                this.LDT3A200USB.Dispose();
            }
            catch (Exception e1)
            {
                this.Message = this.Message + "\n" + e1.ToString();
            }
            
            //if (HardwareList.Contains("MPI USB serial port"))
            //{
            //    hwh.SetDeviceState(MPI_SUB, false);
            //    hwh.SetDeviceState(MPI_SUB, true);
            //}
            //else
            //{
            //    this.Message = this.Message + "\n" + "Can't find MPI_USB";
            //}

            this.LDT3A200USB = null;
            return this.Message;
        }

        public void ResetCom()
        {
            int USBbytes = this.LDT3A200USB.BytesToRead;
            byte[] _byUSBRx = new byte[USBbytes];

            try
            {
                this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
        }

		public string ReadMemFromFile(string pathAndName)
		{
			this.Message = "";
			int addr = 0;
			try
			{
				using (StreamReader reader = new StreamReader(pathAndName, true))
				{
					for (addr = 0; addr < 808; addr++)   // 原本為 526, 
					{
						this._nMemory[addr] = Convert.ToUInt16(reader.ReadLine());
					}
				}
			}
			catch (Exception e1)
			{
				this.Message = "Can't Read Data from File";
			}
			return this.Message;
		}

		public string TurnOn()
        {
            this.Message = "";
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
				//------------------------------------------------------------
				// 以最小檔位，送出最小值，設定 & 執行一次
				//------------------------------------------------------------
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetSequence;           //0x50
                    _byUSBTx[4] = 0;
                    _byUSBTx[5] = 0x5E;
                    _byUSBTx[6] = 0x19;
                    _byUSBTx[7] = 0x80;
                    _byUSBTx[8] = 0xFE;
                    _byUSBTx[9] = 0x1F;
                    _byUSBTx[10] = 0;
                    _byUSBTx[11] = 0;
                    _byUSBTx[12] = 0;
                    _byUSBTx[13] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 14);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 3)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
					{
						this.Message = this.Message + "communication error\n";
					}
                    
					_byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetSequence;           //0x50
                    _byUSBTx[4] = 1;
                    _byUSBTx[5] = 0x65;
                    _byUSBTx[6] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 7);
                    USBbytes = this.LDT3A200USB.BytesToRead;
                    TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 13)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					_byUSBRx = new byte[13];
                    this.LDT3A200USB.Read(_byUSBRx, 0, 13);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
					{
						this.Message = this.Message + "communication error\n";
					}
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return this.Message;
        }

        public string TurnOff()
        {
            this.Message = "";
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
				//------------------------------------------------------------
				// 以最小檔位，送出最小值，設定 & 執行一次
				//------------------------------------------------------------
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetSequence;           //0x50
                    _byUSBTx[4] = 0;
                    _byUSBTx[5] = 0x1E;
                    _byUSBTx[6] = 0x19;
                    _byUSBTx[7] = 0x80;
                    _byUSBTx[8] = 0xFE;
                    _byUSBTx[9] = 0x1F;
                    _byUSBTx[10] = 0;
                    _byUSBTx[11] = 0;
                    _byUSBTx[12] = 0;
                    _byUSBTx[13] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 14);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 3)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();

                    byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
					{
						this.Message = this.Message + "communication error\n";
					}
                    
					_byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetSequence;           //0x50
                    _byUSBTx[4] = 1;
                    _byUSBTx[5] = 0x65;
                    _byUSBTx[6] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 7);
                    USBbytes = this.LDT3A200USB.BytesToRead;
                    TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 13)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					_byUSBRx = new byte[13];
                    this.LDT3A200USB.Read(_byUSBRx, 0, 13);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
					{
						this.Message = this.Message + "communication error\n";
					}
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return this.Message;
        }

        public UInt16 AD0MA()
        {
            this.Message = "";
            UInt16 temp = 0;
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadAD0MA;              // 0x30
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x30 AD0MA_LOW_Byte  AD0MA_HIGH_Byte = 5 bytes
					//-------------------------------------------------------------------------------------------------
					while (USBbytes < 5)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x30)
					{
						this.Message = this.Message + "communication error\n";
					}
					temp = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[3]) + (Convert.ToUInt16(_byUSBRx[4]) * 256));
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return temp;
        }

        public UInt16 AD1MA()
        {
            this.Message = "";
            UInt16 temp = 0;
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadAD1MA;                                             //0x31
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x31 AD1MA_LOW_Byte  AD1MA_HIGH_Byte = 5 bytes
					//-------------------------------------------------------------------------------------------------
					while (USBbytes < 5)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					byte[] _byUSBRx = new byte[USBbytes];
					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x31)
					{
						this.Message = this.Message + "communication error\n";
					}
                    temp = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[3]) + (Convert.ToUInt16(_byUSBRx[4]) * 256));
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return temp;
        }

        public UInt16 AD2MA()
        {
            this.Message = "";
            UInt16 temp = 0;

            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadAD2MA;                                                     //0x32
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);


					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x32 AD2MA_LOW_Byte  AD2MA_HIGH_Byte = 5 bytes
					//-------------------------------------------------------------------------------------------------
					while (USBbytes < 5)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();

                    byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x32)
					{
						this.Message = this.Message + "communication error\n";
					}
                    temp = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[3]) + (Convert.ToUInt16(_byUSBRx[4]) * 256));
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return temp;
        }

        public UInt16 AD0()
        {
            this.Message = "";
            UInt16 temp = 0;
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadAD0;                                                         //0x33
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 5)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x33)
					{
						this.Message = this.Message + "communication error\n";
					}
                    temp = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[3]) + (Convert.ToUInt16(_byUSBRx[4]) * 256));
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return temp;
        }

        public UInt16 AD1()
        {
            this.Message = "";
            UInt16 temp = 0;
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadAD1;                                                                     //0x34
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

                    while (USBbytes < 5)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();

                    byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x34)
					{
						this.Message = this.Message + "communication error\n";
					}
                    temp = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[3]) + (Convert.ToUInt16(_byUSBRx[4]) * 256));
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return temp;
        }

        public UInt16 AD2()
        {
            this.Message = "";
            UInt16 temp = 0;
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadAD2;                                             //0x35
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 5)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x35)
					{
						this.Message = this.Message + "communication error\n";
					}
                    temp = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[3]) + (Convert.ToUInt16(_byUSBRx[4]) * 256));
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return temp;
        }

        public UInt16[] ADMA()
        {
            this.Message = "";
            UInt16[] temp = new UInt16[4];
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadADMA;                      //0x36
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

                    while (USBbytes < 11)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();

                    byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x36)
					{
						this.Message = this.Message + "communication error\n";
					}

                    temp[0] = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[3]) + (Convert.ToUInt16(_byUSBRx[4]) * 256));
                    temp[1] = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[5]) + (Convert.ToUInt16(_byUSBRx[6]) * 256));
                    temp[2] = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[7]) + (Convert.ToUInt16(_byUSBRx[8]) * 256));
                    temp[3] = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[9]) + (Convert.ToUInt16(_byUSBRx[10]) * 256));
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return temp;
        }

        public UInt16[] AD()
        {
            this.Message = "";
            UInt16[] temp = new UInt16[4];
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadAD;            //0x37
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

                    while (USBbytes < 11)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();

                    byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x37)
					{
						this.Message = this.Message + "communication error\n";
					}

                    temp[0] = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[3]) + (Convert.ToUInt16(_byUSBRx[4]) * 256));
                    temp[1] = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[5]) + (Convert.ToUInt16(_byUSBRx[6]) * 256));
                    temp[2] = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[7]) + (Convert.ToUInt16(_byUSBRx[8]) * 256));
                    temp[3] = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[9]) + (Convert.ToUInt16(_byUSBRx[10]) * 256));
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return temp;
        }

        public byte[] Flash_Memory(byte opCode, UInt32 address, byte data1, byte data2, byte data3, byte data4)
        {
            byte addr1, addr2, addr3;
            byte[] data = new byte[4];
            this.Message = "";
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    addr1 = Convert.ToByte(((address * 4) & 0xFF0000) / 65536);
                    addr3 = Convert.ToByte((address * 2) & 0xFF);
                    addr2 = Convert.ToByte((((address & 0xFFFF00) * 4) + ((address & 0xFF) * 2) & 0xFF00) / 256);
                    
					#region > Opcode == 0x06	; Write Enable

                    if (opCode == 0x06)			
                    {
                        _byUSBTx[0] = 0;
                        _byUSBTx[1] = 0;
                        _byUSBTx[2] = SOF;
                        _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteFlash_nMemory;                   //0x46
                        _byUSBTx[4] = 0x06;
                        _byUSBTx[5] = 0xFF;

                        this.LDT3A200USB.Write(_byUSBTx, 0, 6);
                        int USBbytes = this.LDT3A200USB.BytesToRead;

                        System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                        this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                        
						while (USBbytes < 3)
                        {
                            USBbytes = this.LDT3A200USB.BytesToRead;
                            Thread.Sleep(0);
                            if (this._isOverTime == true)
                            {
                                this.Message = this.Message + "timeout\n";
                                break;
                            }
                        }
                        this._isOverTime = false;
                        this.loopTimer.Dispose();

                        byte[] _byUSBRx = new byte[USBbytes];
                        
						this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
						if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x46)
						{
							this.Message = this.Message + "communication error\n";
						}
                    }
                    #endregion (Opcode == 6)	// Write Enable

                    #region > Opcode == 0x39	; Unprotect Sector

                    else if (opCode == 0x39)	
                    {
                        _byUSBTx[0] = 0;
                        _byUSBTx[1] = 0;
                        _byUSBTx[2] = SOF;
                        _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteFlash_nMemory;                   //0x46
                        _byUSBTx[4] = 0x39;
                        _byUSBTx[5] = addr1;
                        _byUSBTx[6] = addr2;
                        _byUSBTx[7] = addr3;
                        _byUSBTx[8] = 0xFF;

                        this.LDT3A200USB.Write(_byUSBTx, 0, 9);
                        int USBbytes = this.LDT3A200USB.BytesToRead;
                        
						System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                        this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                        
						while (USBbytes < 3)
                        {
                            USBbytes = this.LDT3A200USB.BytesToRead;
                            Thread.Sleep(0);
                            if (this._isOverTime == true)
                            {
                                this.Message = this.Message + "timeout\n";
                                break;
                            }
                        }
                        this._isOverTime = false;
                        this.loopTimer.Dispose();
                        byte[] _byUSBRx = new byte[USBbytes];
                        this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
                        if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x46) this.Message = this.Message + "communication error\n";
                    }
                    #endregion (Opcode == 0x39)		// Unprotect Sector

                    #region > Opcode == 0x20	; Block Erase (4 Kbytes)

                    else if (opCode == 0x20)	
                    {
                        _byUSBTx[0] = 0;
                        _byUSBTx[1] = 0;
                        _byUSBTx[2] = SOF;
                        _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteFlash_nMemory;                   //0x46
                        _byUSBTx[4] = 0x20;
                        _byUSBTx[5] = addr1;
                        _byUSBTx[6] = addr2;
                        _byUSBTx[7] = addr3;
                        _byUSBTx[8] = 0xFF;

                        this.LDT3A200USB.Write(_byUSBTx, 0, 9);
                        int USBbytes = this.LDT3A200USB.BytesToRead;
                        System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                        
						this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                        
						while (USBbytes < 3)
                        {
                            USBbytes = this.LDT3A200USB.BytesToRead;
                            Thread.Sleep(0);
                            if (this._isOverTime == true)
                            {
                                this.Message = this.Message + "timeout\n";
                                break;
                            }
                        }
                        this._isOverTime = false;
                        this.loopTimer.Dispose();
                        byte[] _byUSBRx = new byte[USBbytes];
                        this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
                        if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x46) this.Message = this.Message + "communication error\n";
                    }
                    #endregion (Opcode == 0x20)		// Block Erase (4 Kbytes)

                    #region > Opcode == 0x02	; Byte / Page Program (1 to 256 Bytes)

                    else if (opCode == 0x02)			
                    {
                        _byUSBTx[0] = 0;
                        _byUSBTx[1] = 0;
                        _byUSBTx[2] = SOF;
                        _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteFlash_nMemory;                   //0x46
                        _byUSBTx[4] = 2;
                        _byUSBTx[5] = addr1;
                        _byUSBTx[6] = addr2;
                        _byUSBTx[7] = addr3;
                        if (data1 == 0xFF)
                        {
                            _byUSBTx[8] = 0xFE;
                            _byUSBTx[9] = 1;
                        }
                        else
                        {
                            _byUSBTx[8] = data1;
                            _byUSBTx[9] = 0;
                        }
                        if (data2 == 0xFF)
                        {
                            _byUSBTx[10] = 0xFE;
                            _byUSBTx[11] = 1;
                        }
                        else
                        {
                            _byUSBTx[10] = data2;
                            _byUSBTx[11] = 0;
                        }
                        if (data3 == 0xFF)
                        {
                            _byUSBTx[12] = 0xFE;
                            _byUSBTx[13] = 1;
                        }
                        else
                        {
                            _byUSBTx[12] = data3;
                            _byUSBTx[13] = 0;
                        }
                        if (data4 == 0xFF)
                        {
                            _byUSBTx[14] = 0xFE;
                            _byUSBTx[15] = 1;
                        }
                        else
                        {
                            _byUSBTx[14] = data4;
                            _byUSBTx[15] = 0;
                        }
                        _byUSBTx[16] = 0xFF;

                        this.LDT3A200USB.Write(_byUSBTx, 0, 17);
                        int USBbytes = this.LDT3A200USB.BytesToRead;

                        System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                        this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                        
						while (USBbytes < 3)
                        {
                            USBbytes = this.LDT3A200USB.BytesToRead;
                            Thread.Sleep(0);
                            if (this._isOverTime == true)
                            {
                                this.Message = this.Message + "timeout\n";
                                break;
                            }
                        }
                        this._isOverTime = false;
                        this.loopTimer.Dispose();
                        
						byte[] _byUSBRx = new byte[USBbytes];
                        this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

						if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x46)
						{
							this.Message = this.Message + "communication error\n";
						}
                    }
                    #endregion (Opcode == 2)		// Byte/Page Program (1 to 256 Bytes)
                    
					#region > Opcode == 0x03	; Read Array
                    
					else if (opCode == 0x03)	
                    {
                        _byUSBTx[0] = 0;
                        _byUSBTx[1] = 0;
                        _byUSBTx[2] = SOF;
                        _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteFlash_nMemory;                   //0x46
                        _byUSBTx[4] = 3;
                        _byUSBTx[5] = addr1;
                        _byUSBTx[6] = addr2;
                        _byUSBTx[7] = addr3;
                        _byUSBTx[8] = 0xFF;

                        this.LDT3A200USB.Write(_byUSBTx, 0, 9);
                        int USBbytes = this.LDT3A200USB.BytesToRead;
                        
						System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                        this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

						//-------------------------------------------------------------------------------------------------
						// DSP Side Return : 0xFF 0xFF data01(2byte) data02(2byte) data03(2byte) = 9 bytes
						//-------------------------------------------------------------------------------------------------
						while (USBbytes < 9)
                        {
                            USBbytes = this.LDT3A200USB.BytesToRead;
                            Thread.Sleep(0);
                            if (this._isOverTime == true)
                            {
                                this.Message = this.Message + "timeout\n";
                                break;
                            }
                        }
                        this._isOverTime = false;
                        this.loopTimer.Dispose();
                        
						byte[] _byUSBRx = new byte[USBbytes];
                        this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

						if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x46)
						{
							this.Message = this.Message + "communication error\n";
						}
                        
						data[0] = _byUSBRx[5];
                        data[1] = _byUSBRx[6];
                        data[2] = _byUSBRx[7];
                        data[3] = _byUSBRx[8];
                    }
                    #endregion (Opcode == 3)		// Read Array

                    #region > Opcode == 0x05	; Read Status Register

                    else if (opCode == 0x05)
                    {
                        _byUSBTx[0] = 0;
                        _byUSBTx[1] = 0;
                        _byUSBTx[2] = SOF;
                        _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteFlash_nMemory;                   //0x46
                        _byUSBTx[4] = 5;
                        _byUSBTx[5] = 0xFF;

                        this.LDT3A200USB.Write(_byUSBTx, 0, 6);
                        int USBbytes = this.LDT3A200USB.BytesToRead;
                        
						System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                        this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

						//-------------------------------------------------------------------------------------------------
						// DSP Side Return : 0xFF 0xFF 0x46 data01(2byte) data02(2byte) data03(2byte) = 9 bytes
						//-------------------------------------------------------------------------------------------------
						while (USBbytes < 9)
                        {
                            USBbytes = this.LDT3A200USB.BytesToRead;
                            Thread.Sleep(0);
                            if (this._isOverTime == true)
                            {
                                this.Message = this.Message + "timeout\n";
                                break;
                            }
                        }
                        this._isOverTime = false;
                        this.loopTimer.Dispose();
                        
						byte[] _byUSBRx = new byte[USBbytes];
                        this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

						if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x46)
						{
							this.Message = this.Message + "communication error\n";
						}
                        data[0] = _byUSBRx[5];
                        data[1] = _byUSBRx[6];
                        data[2] = _byUSBRx[7];
                        data[3] = _byUSBRx[8];
                    }

                    #endregion (Opcode == 5)		// Read Status Register

                    #region > Opcode == 0x9F	; Read Manufacturer and Device ID

                    else if (opCode == 0x9F)	
                    {
                        _byUSBTx[0] = 0;
                        _byUSBTx[1] = 0;
                        _byUSBTx[2] = SOF;
                        _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteFlash_nMemory;                   //0x46
                        _byUSBTx[4] = 0x9F;
                        _byUSBTx[5] = 0xFF;

                        this.LDT3A200USB.Write(_byUSBTx, 0, 6);
                        
						int USBbytes = this.LDT3A200USB.BytesToRead;
                        System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                        this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

						//-------------------------------------------------------------------------------------------------
						// DSP Side Return : 0xFF 0xFF 0x46 data01(2byte) data02(2byte) data03(2byte) = 9 bytes
						//-------------------------------------------------------------------------------------------------
						while (USBbytes < 9)
                        {
                            USBbytes = this.LDT3A200USB.BytesToRead;
                            Thread.Sleep(0);
                            if (this._isOverTime == true)
                            {
                                this.Message = this.Message + "timeout\n";
                                break;
                            }
                        }
                        this._isOverTime = false;
                        this.loopTimer.Dispose();
                        byte[] _byUSBRx = new byte[USBbytes];
                        this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

						if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x46)
						{
							this.Message = this.Message + "communication error\n";
						}
                        data[0] = _byUSBRx[5];
                        data[1] = _byUSBRx[6];
                        data[2] = _byUSBRx[7];
                        data[3] = _byUSBRx[8];
                    }
                    #endregion (Opcode == 0x9F)			// Read Manufacturer and Device ID

                    #region > Opcode == 0xAB	; Resume from Deep Power-down

                    if (opCode == 0xAB)				
                    {
                        _byUSBTx[0] = 0;
                        _byUSBTx[1] = 0;
                        _byUSBTx[2] = SOF;
                        _byUSBTx[3] = (byte)ERegCmd.ReadAndWriteFlash_nMemory;                   //0x46
                        _byUSBTx[4] = 0xAB;
                        _byUSBTx[5] = 0xFF;

                        this.LDT3A200USB.Write(_byUSBTx, 0, 6);
                        
						int USBbytes = this.LDT3A200USB.BytesToRead;
                        System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                        this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                        
						while (USBbytes < 3)
                        {
                            USBbytes = this.LDT3A200USB.BytesToRead;
                            Thread.Sleep(0);
                            if (this._isOverTime == true)
                            {
                                this.Message = this.Message + "timeout\n";
                                break;
                            }
                        }
                        this._isOverTime = false;
                        this.loopTimer.Dispose();

                        byte[] _byUSBRx = new byte[USBbytes];
                        this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
						if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x46)
						{
							this.Message = this.Message + "communication error\n";
						}
                    }
                    #endregion (Opcode == 0xAB) //Resume from Deep Power-down
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return data;
        }

        public byte[] Read_Flash(UInt32 address)
        {
            byte[] data = new byte[4];
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                data = this.Flash_Memory(0x03, address, 0, 0, 0, 0);
            }
            return data;
        }

        public void Write_Flash(UInt32 address, byte data1, byte data2, byte data3, byte data4)
        {
            byte[] data = new byte[4];
            this.Message = "";
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                data = Flash_Memory(0x03, address, 0, 0, 0, 0);

                if (((data[0] == 0xFF) || (data[0] == data1)) && 
					((data[1] == 0xFF) || (data[1] == data2)) && 
					((data[2] == 0xFF) || (data[2] == data3)) && 
					((data[3] == 0xFF) || (data[3] == data4)) )
                {
					this.Flash_Memory(0x06,		address, 0, 0, 0, 0);   //
					this.Flash_Memory(0x39,		address, 0, 0, 0, 0);
					this.Flash_Memory(0x06,		address, 0, 0, 0, 0);
					this.Flash_Memory(0x39,		address + 3, 0, 0, 0, 0);
					this.Flash_Memory(0x06,		address, 0, 0, 0, 0);
					this.Flash_Memory(0x02,		address, data1, data2, data3, data4);
                }
                else
                {
                    byte[] temp = new byte[4096];
                    byte[] temp1 = new byte[4];
                    UInt32 addr1 = (address & 0xFF000);
                    for (UInt32 i = 0; i < 4096; i += 4)
                    {
						temp1 = this.Flash_Memory(0x03, addr1 + i, 0, 0, 0, 0);
                        temp[i] = temp1[0];
                        temp[i + 1] = temp1[1];
                        temp[i + 2] = temp1[2];
                        temp[i + 3] = temp1[3];
                    }
                    addr1 = (address & 0xFFF);
                    temp[addr1] = data1;
                    temp[addr1 + 1] = data2;
                    temp[addr1 + 2] = data3;
                    temp[addr1 + 3] = data4;
                    addr1 = (address & 0xFF000);
                    
					this.Flash_Memory(0x06,	address, 0, 0, 0, 0);
                    this.Flash_Memory(0x39, address, 0, 0, 0, 0);
                    this.Flash_Memory(0x06,	address, 0, 0, 0, 0);
                    this.Flash_Memory(0x20, address, 0, 0, 0, 0);

					for (int i = 0; i < 100; i++)
					{
						this.Flash_Memory(0x9F, address, 0, 0, 0, 0);
					}
                    for (UInt32 i = 0; i < 4096; i += 4)
                    {
						this.Flash_Memory(0x06, address, 0, 0, 0, 0);
						this.Flash_Memory(0x02, addr1 + i, temp[i], temp[i + 1], temp[i + 2], temp[i + 3]);
                    }
                }
                data = Flash_Memory(0x03, address, 0, 0, 0, 0);
				if ((data[0] != data1) || (data[1] != data2) || (data[2] != data3) || (data[3] != data4))
				{
					this.Message = "Flash error!!!";
				}
            }
        }

        public string SetGain(byte order)
        {
            this.Message = "";
            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }
            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                try
                {
                    if (order > 0xFE) order = 0xFE;
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetCH2Gain;                   //0x47;
                    _byUSBTx[4] = order;
                    _byUSBTx[5] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 6);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 3)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x47)
					{
						this.Message = this.Message + "communication error\n";
					}
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
            return this.Message;
        }        

        public byte[] Read_DI()
        {
            this.Message = "";
            byte[] temp = new byte[3];
            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }
                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadDI;                //0x38
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x38 IO_Data01(byte) IO_Data02(1byte) IO_Data03(1byte) = 6 bytes
					//-------------------------------------------------------------------------------------------------
                    while (USBbytes < 6)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();

                    byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x38)
					{
						this.Message = this.Message + "communication error\n";
					}
                    temp[0] = _byUSBRx[3];
                    temp[1] = _byUSBRx[4];
                    temp[2] = _byUSBRx[5];
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
            return temp;
        }

        public string Write_DO(byte DO1, byte DO2, byte DO3)
        {
            this.Message = "";
            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }
                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    if (DO1 == 0xFF) DO1 = 0;
                    if (DO2 == 0xFF) DO2 = 0;
                    if (DO3 == 0xFF) DO3 = 0;
                    
					_byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.WriteDO;                        //0x4F
                    _byUSBTx[4] = DO1;
                    _byUSBTx[5] = DO2;
                    _byUSBTx[6] = DO3;
                    _byUSBTx[7] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 8);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 3)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();

                    byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x4F)
					{
						this.Message = this.Message + "communication error\n";
					}
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
            return this.Message;
        }		

        public void Stop()
        {
            this.Message = "";
            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }
                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.StopSequence;           //0x53;
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
					while (USBbytes < 3)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x53)
					{
						this.Message = this.Message + "communication error\n";
					}
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
        }       

        public void SetTimeBase(byte timeBase)
        {
            this.Message = "";

			if (this._bTimeBase == timeBase)
				return;


			this._bTimeBase = timeBase;  
			this.SetTimeBaseToDev();
        }

        public void GetDeviceInfo()
        {
            DEV_SN = "None";
            CAL_DATE = "None";
            HW_VER = "None";
            PCB_SN = "None";
            QC_SW_VER = "None";

            if (this._isMultiDriveState == true)
            {
                this.Multi_Getdata();
            }

            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }

            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                byte[] data = new byte[4];
                byte[] data02 = new byte[4];

                try
                {
                    // Get SN
                    data = this.Read_Flash(0x41C);

                    uint sn = (uint)(data[0] + (data[1] << 8) + (data[2] << 16) + (data[3] << 32));

                    this.DEV_SN = string.Format("T3A{0}", sn.ToString("00000000"));

                    // Get Cal Date
                    data = this.Read_Flash(0x420);
                    uint year = (uint)((data[2]) + (data[3] << 8));
                    uint date = (uint)((data[0]) + (data[1] << 8));

                    this.CAL_DATE = string.Format("{0} {1}", year, date);

                    // Get HW Ver
                    data = this.Read_Flash(0x424);
                    this.HW_VER = string.Format("V{0}.{1}{2}", ((uint)data[0]).ToString(), ((uint)data[1]).ToString("00"), ((uint)data[2]).ToString("00"));

                    // Get PCB Ver & QC S/W Ver
                    data = this.Read_Flash(0x427);
                    data02 = this.Read_Flash(0x42B);
                    uint pcbsn = (uint)(data[1] + (data[2] << 8) + (data[3] << 16) + (data02[0] << 32));

                    this.PCB_SN = string.Format("ZO{0} {1}", ((uint)data[0]).ToString(), pcbsn);

                    this.QC_SW_VER = string.Format("V{0}.{1}.{2}", ((uint)data02[1]).ToString(), ((uint)data02[2]).ToString(), ((uint)data02[3]).ToString());
                }
                catch (Exception e1)
                {
                    this.Message = e1.ToString();
                }
            }
        }

        public string[] GetVersion()
        {
            this.Message = "";
            this._sVersion[0] = "0"; //  new _sVersion
            this._sVersion[1] = "0.1.1.3"; // multi-channel _sVersion, fast thy+100points, _sVersion check
            this.SerialNum = 0xFFFFFFFF;
            this.SerialNum1 = 0xFFFFFFFF;
            this.serialNum2 = 0xFFFFFFFF;

            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }
                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.Read_sVersion;                                 //0x56
                    _byUSBTx[4] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 5);
                    int USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x56 ver01(1byte) ver02(1byte) ver03(1byte) ver04(1byte) = 7 bytes
					//-------------------------------------------------------------------------------------------------
					while (USBbytes < 7)
                    {
                        USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    
					byte[] _byUSBRx = new byte[USBbytes];
                    this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x56)
					{
						this.Message = this.Message + "communication error\n";
					}
                    
					this._sVersion[0] = Convert.ToString(_byUSBRx[3] + _byUSBRx[4] * 256);
                    this.serial_num_temp = this.Read_Flash(1052);
                    this.SerialNum1 = Convert.ToUInt32(this.serial_num_temp[0] + this.serial_num_temp[1] * 256 + this.serial_num_temp[2] * 256 * 256 + this.serial_num_temp[3] * 256 * 256 * 256);
                    this.serial_num_temp = this.Read_Flash(1056);
                    this.serialNum2 = Convert.ToUInt32(this.serial_num_temp[0] + this.serial_num_temp[1] + this.serial_num_temp[2] + this.serial_num_temp[3]);
					
					if (this.serialNum2 >= 1000)
					{
						this.serialNum2 = 0;
					}
                    this.SerialNum = this.SerialNum1 * 1000 + this.serialNum2;
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
            return this._sVersion.Clone() as string[];
        }        

		public byte SetStatus(EStatus status)
		{
			this._bStatus = (byte)(status);

			this.SetStatusToDev();
			
			return this._bStatus;
		}
        
		public float[] ContactCheck(byte time)
		{
			float[] contact_R = new float[2] { 1000000000, 1000000000 };
			float V1, V2, V3, I;
			try
			{
				if (this._isMultiDriveState == true)
				{
					this.Multi_Getdata();
				}
				if (this._isMultiCurveState == true)
				{
					this.Multi_GetCurveData();
				}
				if (this._isMultiDriveState == false && this._isMultiCurveState == false)
				{
					//if ( _nMemory[46] == 0) this.ReadMemToFile("Memory.csv");		// Gilbert 這一行太奇怪了，_nMemory[46] 這是 -6V 校正的最後一點，通常是最大值，
																					// 先不管，Disable 掉， ReadMemToFile() 這應該是外部功能，不該在此 Class.

					//-----------------------------------------------------------------------
					// (01) 執行IO切換, (02)輸出驅動，抓取資料，  (03)將IO切回到原來
					//-----------------------------------------------------------------------
					this.Internal_IO(0, 0, 2);
					this.runSingleSequence(1, 10, "mA", time, 1, 0x65);
					this.Internal_IO(0, 0, 0);

					V1 = Convert.ToSingle(this._sRtnVS[0]);
					V2 = Convert.ToSingle(this._sRtnVS[2]);
					I = Convert.ToSingle(this._sRtnIF[0]);

					V3 = I / 1000f * 30f;
					if (I <= 0.01)
					{
						contact_R[0] = 1000000000;
						contact_R[1] = 1000000000;
					}
					else
					{
						if (V1 > V2) contact_R[0] = (V1 - V2) / I * 1000f;
						else contact_R[0] = 0;
						if (V2 > V3)
						{
							contact_R[1] = (V2 - V3) / I * 1000f;
							if (contact_R[1] >= 1000) contact_R[1] = 1000000000;
							else contact_R[1] = contact_R[1] * 1000f / (1000f - contact_R[1]);
						}
						else contact_R[1] = 0;
					}
				}
			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}
			return contact_R;
		}		

        public int[] Dump_Flash_Calibration()
        {
            byte[] data = new byte[4];

            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }
                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                   // 1052
                    uint shitfIndex = 0;

                    for (uint i = 0; i < LDevConst.MAX_MEMORY_LENGTH * 2; i += 4)   // Read 263 times
                    {
                        shitfIndex = 0;
                        
                        if (i >= 1052)
                        {
                            shitfIndex = 20;
                        }

                        data = this.Read_Flash(i + shitfIndex);
                        this._nMemory[i / 2] = data[0] + data[1] * 256;
                        this._nMemory[i / 2 + 1] = data[2] + data[3] * 256;
                    }
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
            return this._nMemory.Clone() as int[];
        }

        public void Dump_Flash(UInt32 address, out int[] flash_nMemory)
        {
            flash_nMemory = new int[address / 2];

            byte[] data = new byte[4];

            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }

                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    for (uint i = 0; i < address; i += 4)
                    {
                        data = this.Read_Flash(i);
                        flash_nMemory[i / 2] = data[0] + data[1] * 256;
                        flash_nMemory[i / 2 + 1] = data[2] + data[3] * 256;
                    }
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
        }

        public void Multi_Drive(float V, float I, string currUint, double msTime,  bool isAutoTurnOff)
        {
            this.Message = "";
            try
            {
                if (this._isMultiDriveState == true)  //狀態檢查
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)  //狀態檢查
                {
                    this.Multi_GetCurveData();
                }
                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    _seqIndex = 0;

                    
					//this._bTimeCount = (byte)(this._bTimeBase * time / 100);  // 時間設定
					//if (this._bTimeCount <= 0x01) { this._bTimeCount = 0x01; }
					//if (this._bTimeCount >= 0xFE) { this._bTimeCount = 0xFE; }

					//this.SetForceTime(0, (double)time);
					//this.cmd_converter(V, I, I_unit);

                    this._bExecMode = (isAutoTurnOff == true) ? (byte)0x65 : (byte)0x67;

                    if ((I < 0.0) && (V > 0.0))
                    {
                        V = 0f - V;
                    }

                    this.Cmd_V_Convert(V, true);

                    this.Cmd_I_Convert(V, I, currUint, true);

                    this.SetForceTime(0, msTime);

                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.SetSequence;           //0x50
                    _byUSBTx[4] = 0;
                    _byUSBTx[5] = this._bHW_DevRange;				// 0x5E;
                    _byUSBTx[6] = this._bCMPA;					// 0x19;
                    _byUSBTx[7] = this._bCMPAHR;					// 0x80;
                    _byUSBTx[8] = this._bLimitCurrLow;			// 0xFE;
                    _byUSBTx[9] = this._bLimitCurrHigh;			// 0x1F;
                    _byUSBTx[10] = this._bTimeCount;
                    _byUSBTx[11] = _byCMP_ex[0];
                    _byUSBTx[12] = _byCMP_ex[1];
                    _byUSBTx[13] = 0xFF;								// Gilbert 怪怪的用法，出現 0xFF 在 Frame 上,
                    _byUSBTx[14] = (byte)ERegCmd.SetSequence;           // 直接用0xFF 斷開，立刻執行 cmd = 0x50, executeMode =0x65 or 0x67 嗎?
                    _byUSBTx[15] = 1;
                    _byUSBTx[16] = this._bExecMode;
                    _byUSBTx[17] = 0xFF;

                    if (this.IsLog)
                    {
                        Console.WriteLine(string.Format("[LDT3A DLL], Cmd {0}, vRng = {1}, iRng = {2}, Rng = {3}, CMPA = {4}, HR = {5}, I_L = {6}, I_H = {7}, Time = {8}, Ex_L = {9}, Ex_H = {10}",
                                                              LDT3A200USB.PortName, _vRangeSet.ToString(), _iRangeSet.ToString(), this._bHW_DevRange, _bCMPA, _bCMPAHR, _bLimitCurrLow, _bLimitCurrHigh, _bTimeCount, _byCMP_ex[0], _byCMP_ex[1]));

                    }

					this.LDT3A200USB.Write(_byUSBTx, 0, 18);
					_seqHWRange[0] = this._bHW_DevRange;
					_seqCMP[0] = _bCMPA * 256 + _bCMPAHR;
					_seqMasterGL[0] = _bLimitCurrHigh;
                    this._isMultiDriveState = true;
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
        }

        public string[] Multi_Getdata()
        {

			//------------------------------------------------------------------------
			// Gilbert, 沒有對應的 cmd , 直接就對 USB Port 讀取資料 ???
			//------------------------------------------------------------------------
            this.Message = "";
            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.USBbytes = this.LDT3A200USB.BytesToRead;
                    if (this.USBbytes >= 16)
                    {
                        this._byUSBRx = new byte[this.USBbytes];
                        this.LDT3A200USB.Read(this._byUSBRx, 0, this.USBbytes);
						if (this._byUSBRx[0] != 0xFF || this._byUSBRx[1] != 0xFF || this._byUSBRx[2] != 0x50)
						{
							this.Message = this.Message + "communication error\n";
						}
                        
						UInt16 AD0 = 0, AD1 = 0, AD2 = 0;
                        AD0 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[10]) + (Convert.ToUInt16(_byUSBRx[11]) * 256));                        
                        AD1 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[12]) + (Convert.ToUInt16(_byUSBRx[13]) * 256));
                        AD2 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[14]) + (Convert.ToUInt16(_byUSBRx[15]) * 256));
                        

						//--------------------------------------------------------------------------------------
						// cal_4Wire() 執行前，一定要先執行 cal_IF, 算電流流過的一個電壓補償值。
						//--------------------------------------------------------------------------------------
						this._sRtnIF = cal_IF(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0);

						if ((this._bStatus & 0x04) == 0x04)
                        {
							this._sRtnVS = this.cal_4Wire(_seqHWRange[0], _seqCMP[0], AD1, AD2);
                        }
                        else
                        {
							if (((_seqHWRange[0] & 0x60) == 0x60) || ((_seqHWRange[0] & 0x40) == 0x40))
							{
								this._sRtnVS = this.cal_2Wire(_seqHWRange[0], _seqCMP[0], AD1, AD2);		// 用2線式的 cal_2Wire() 計算， 並沒有 AD1 的數據 ? // Gilbert 不對吧 !!	
							}
							else
							{
								this._sRtnVS = this.cal_4Wire(_seqHWRange[0], _seqCMP[0], AD1, AD2);
							}
                        }
						this._sMeasureIV[0] = this._sRtnIF[0];
						this._sMeasureIV[1] = this._sRtnIF[1];
						this._sMeasureIV[2] = this._sRtnVS[0];
						this._sMeasureIV[3] = this._sRtnVS[1];
						this._sMeasureIV[4] = this._sRtnVS[2];
						this._sMeasureIV[5] = this._sRtnVS[3];
						this._sMeasureIV[6] = this._sRtnVS[4];
						this._sMeasureIV[7] = this._sRtnVS[5];
						this._sMeasureIV[8] = Message;
                        this._isMultiDriveState = false;
                    }
                    else
                    {
						this._sMeasureIV[0] = null;
						this._sMeasureIV[1] = null;
						this._sMeasureIV[2] = null;
						this._sMeasureIV[3] = null;
						this._sMeasureIV[4] = null;
						this._sMeasureIV[5] = null;
						this._sMeasureIV[6] = null;
						this._sMeasureIV[7] = null;
						this._sMeasureIV[8] = "busy";
                    }
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
			return this._sMeasureIV.Clone() as string[];
        }

        public string[] Multi_GetThyData()
        {
			//----------------------------------------------------------------------
			// Gilbert, 這一個指令怪怪的，沒有 points 設定，改看 Multi_2Thy_Getdata(int point)
			//----------------------------------------------------------------------
            this.Message = "";
            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }

                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.GetThyData;                     //0x74
                    _byUSBTx[4] = 0;
                    _byUSBTx[5] = 0;                    
					_byUSBTx[6] = 0xFF;
                    
					this.LDT3A200USB.Write(_byUSBTx, 0, 7);
                    this.USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x74 + 6 x 2 byte data = 15 bytes
					// Vpeak_VS_Anode,	Vpeak_VS_Cathode,	Vstable_VS_Anode,	Vstable_VS_Cathode,	Vpoint_VS_Anode,	Vpoint_VS_Cathode
					//-------------------------------------------------------------------------------------------------
					while (this.USBbytes < 15)
                    {
                        this.USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    this._byUSBRx = new byte[this.USBbytes];
                    this.LDT3A200USB.Read(this._byUSBRx, 0, this.USBbytes);

					if (this._byUSBRx[0] != 0xFF || this._byUSBRx[1] != 0xFF || this._byUSBRx[2] != 0x74)
					{
						this.Message = this.Message + "communication error\n";
					}
                    
					UInt16 AD1p = 0, AD2p = 0, AD1s = 0, AD2s = 0;
                    AD1p = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[3]) + (Convert.ToUInt16(this._byUSBRx[4]) * 256));
                    AD2p = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[5]) + (Convert.ToUInt16(this._byUSBRx[6]) * 256));
                    AD1s = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[7]) + (Convert.ToUInt16(this._byUSBRx[8]) * 256));
                    AD2s = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[9]) + (Convert.ToUInt16(this._byUSBRx[10]) * 256));
                    
					//-------------------------------------------------------------------- 
					// Gilbert 怪怪，沒有事先做 cal_IF, 且邏輯上，要再確認。
					//--------------------------------------------------------------------

					float Vpp,Vss,Vdd;

					_sRtnVS = cal_4Wire(_seqHWRange[0], _seqCMP[0], AD1p, AD2p);
                    Vpp = Math.Abs(Convert.ToSingle(_sRtnVS[4]));

					_sRtnVS = cal_4Wire(_seqHWRange[0], _seqCMP[0], AD1s, AD2s);
                    Vss = Math.Abs(Convert.ToSingle(_sRtnVS[4]));		// VDD_DUT, 4 Wire 真正量到的 DUT 電壓差
                    
					if (Vpp < Vss) Vpp = Vss;
                    
					//0.0.4.0
					this._sCurveData = this.ReadCurve(1, 100);		// AD1, VSP, VS_Anode
                    for (int i = 0; i < 100; i++)
					{
						this._curveAD1[i] = Math.Abs(Convert.ToSingle(this._sCurveData[i + 1]));
					}

					this._sCurveData = this.ReadCurve(2, 100);		// AD2, VSN, VS_Cathod
					for (int i = 0; i < 100; i++)
					{
						this._curveAD2[i] = Math.Abs(Convert.ToSingle(this._sCurveData[i + 1]));
					}

					for (int i = 0; i < 100; i++)
					{
						if ((this._curveAD1[i] - this._curveAD2[i]) > Vpp)
						{
							Vpp = this._curveAD1[i] - this._curveAD2[i];
						}
					}
                    //0.0.4.0
                    Vdd = Vpp - Vss;
					this._sMeasureIV[0] = Convert.ToString(Vpp);
					this._sMeasureIV[1] = Convert.ToString(Vss);
					this._sMeasureIV[2] = Convert.ToString(Math.Abs(Vdd));
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
			this._sMeasureIV[3] = Message;
			return this._sMeasureIV.Clone() as string[];
        }

        //0.0.4.0
        public string[] Multi_2Thy_Getdata(int point)
        {
            this.Message = "";
            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Getdata();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }
                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    point = point - 1;
                    if (point < 0) point = 0;
                    if (point > 1999) point = 1999;

                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.GetThyData;                     //0x74
                    _byUSBTx[4] = Convert.ToByte(point & 0xFF);
                    
					if (_byUSBTx[4] == 0xFF) _byUSBTx[4] = 0xFE;
                    _byUSBTx[5] = Convert.ToByte((point / 256) & 0xFF);
                    _byUSBTx[6] = 0xFF;

                    this.LDT3A200USB.Write(_byUSBTx, 0, 7);
                    this.USBbytes = this.LDT3A200USB.BytesToRead;
                    
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x74 + 6 x 2 byte data = 15 bytes
					// Vpeak_VS_Anode,	Vpeak_VS_Cathode,	Vstable_VS_Anode,	Vstable_VS_Cathode,	Vpoint_VS_Anode,	Vpoint_VS_Cathode
					//-------------------------------------------------------------------------------------------------
					while (this.USBbytes < 15)
                    {
                        this.USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    this._byUSBRx = new byte[this.USBbytes];
                    this.LDT3A200USB.Read(this._byUSBRx, 0, this.USBbytes);

					if (this._byUSBRx[0] != 0xFF || this._byUSBRx[1] != 0xFF || this._byUSBRx[2] != 0x74)
					{
						this.Message = this.Message + "communication error\n";
					}

                    UInt16 AD1s = 0, AD2s = 0;
                    AD1s = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[11]) + (Convert.ToUInt16(this._byUSBRx[12]) * 256));
                    AD2s = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[13]) + (Convert.ToUInt16(this._byUSBRx[14]) * 256));


					_sRtnVS = cal_4Wire(_seqHWRange[0], _seqCMP[0], AD1s, AD2s);	// Gilbert, 沒有先執行 cal_IF 算補償值

					//this.Vss = Math.Abs(Convert.ToSingle(_sRtnVS[4]));
					//this._sMeasureIV[0] = Convert.ToString(this.Vss);

					this._sMeasureIV[0] = Convert.ToString(Math.Abs(Convert.ToSingle(_sRtnVS[4])));		// _sRtnVS[4] = VDD_DUT, 真正 DUT 的電壓差值
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
			this._sMeasureIV[1] = Message;
			return this._sMeasureIV.Clone() as string[];
        }

		//public string[] Old_Multi_2Thy_Getdata(int point)
		//{
		//    this.Message = "";
		//    try
		//    {
		//        if (this._isMultiDriveState == true)
		//        {
		//            this.Multi_Getdata();
		//        }
		//        if (this._isMultiCurveState == true)
		//        {
		//            this.multi_GetCurveData();
		//        }
		//        if (this._isMultiDriveState == false && this._isMultiCurveState == false)
		//        {
		//            if (point < 100) point = 100;
		//            if (point > 2000) point = 2000;

		//            _byUSBTx[0] = 0;
		//            _byUSBTx[1] = 0;
		//            _byUSBTx[2] = SOF;
		//            _byUSBTx[3] = 0x74;
		//            _byUSBTx[4] = 0xFF;
                    
		//            this.LDT3A200USB.Write(_byUSBTx, 0, 5);
		//            this.USBbytes = this.LDT3A200USB.BytesToRead;
                    
		//            System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
		//            this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);
                    
		//            while (this.USBbytes < 11)
		//            {
		//                this.USBbytes = this.LDT3A200USB.BytesToRead;
		//                Thread.Sleep(0);
		//                if (this._isOverTime == true)
		//                {
		//                    this.Message = this.Message + "timeout\n";
		//                    break;
		//                }
		//            }
		//            this._isOverTime = false;
		//            this.loopTimer.Dispose();

		//            this._byUSBRx = new byte[this.USBbytes];
		//            this.LDT3A200USB.Read(this._byUSBRx, 0, this.USBbytes);

		//            if (this._byUSBRx[0] != 0xFF || this._byUSBRx[1] != 0xFF || this._byUSBRx[2] != 0x74)
		//            {
		//                this.Message = this.Message + "communication error\n";
		//            }
                    
		//            UInt16 AD1p = 0, AD2p = 0, AD1s = 0, AD2s = 0;
                    
		//            AD1p = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[3]) + (Convert.ToUInt16(this._byUSBRx[4]) * 256));
		//            AD2p = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[5]) + (Convert.ToUInt16(this._byUSBRx[6]) * 256));
		//            AD1s = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[7]) + (Convert.ToUInt16(this._byUSBRx[8]) * 256));
		//            AD2s = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[9]) + (Convert.ToUInt16(this._byUSBRx[10]) * 256));
                    
		//            _sRtnVS = cal_4Wire(sequence_Range[0], sequence_CMP[0], AD1p, AD2p);
		//            this.Vpp = Math.Abs(Convert.ToSingle(_sRtnVS[4]));
                    
		//            _sRtnVS = cal_4Wire(sequence_Range[0], sequence_CMP[0], AD1s, AD2s);
		//            this.Vss = Math.Abs(Convert.ToSingle(_sRtnVS[4]));
                    
		//            if (Vpp < Vss) Vpp = Vss;
		//            //0.0.4.0
		//            this._sCurveData = this.ReadCurve(1, point);
		//            for (int i = 0; i < point; i++)
		//            {
		//                this._curveAD1[i] = Math.Abs(Convert.ToSingle(this._sCurveData[i + 1]));
		//            }
		//            this._sCurveData = this.ReadCurve(2, point);
		//            for (int i = 0; i < point; i++)
		//            {
		//                this._curveAD2[i] = Math.Abs(Convert.ToSingle(this._sCurveData[i + 1]));
		//            }
		//            for (int i = 0; i < point; i++)
		//            {
		//                if ((this._curveAD1[i] - this._curveAD2[i]) > this.Vpp)
		//                {
		//                    this.Vpp = this._curveAD1[i] - this._curveAD2[i];
		//                }
		//            }
		//            //0.0.4.0
		//            Vdd = Vpp - Vss;
		//            this.measure_IV[0] = Convert.ToString(this.Vpp);
		//            this.measure_IV[1] = Convert.ToString(this.Vss);
		//            this.measure_IV[2] = Convert.ToString(Math.Abs(Vdd));
		//            this.Vss2 = 0;

		//            for (int i = 1; i <= 20; i++)
		//            {
		//                this.Vss2 = this.Vss2 + (this._curveAD1[point - i] - this._curveAD2[point - i]);
		//            }
                    
		//            this.Vss2 = this.Vss2 / 20;
		//            Vdd2 = Vpp - Vss2;
		//            this.measure_IV[3] = Convert.ToString(this.Vss2);
		//            this.measure_IV[4] = Convert.ToString(Math.Abs(Vdd2));
		//        }
		//    }
		//    catch (Exception e1)
		//    {
		//        this.Message = e1.ToString();
		//    }
		//    this.measure_IV[5] = Message;
		//    return this.measure_IV.Clone() as string[];
		//}
		//0.0.4.0

		#endregion

		#region >>> Public Methods 02 <<<

		public string Set_Sequence(byte sequence, float V, float I, string I_unit, byte time, byte mode)
		{
			this.Message = "";
			if (this._isMultiDriveState == true)
			{
				this.Multi_Getdata();
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
			}
			if (this._isMultiDriveState == false && this._isMultiCurveState == false)
			{
				try
				{
					if (sequence > 0xFE) sequence = 0xFE;

					//this._bTimeCount = (byte)(this._bTimeBase * time / 100);
					//if (this._bTimeCount <= 0x01) { this._bTimeCount = 0x01; }
					//if (this._bTimeCount >= 0xFE) { this._bTimeCount = 0xFE; }

					if (( I < 0.0f ) && ( V>0.0f ) )
					{
						V = 0.0f - V;
					}
					I = Math.Abs(I);

					this.SetForceTime(0, (double)time);
					this.cmd_converter(V, I, I_unit);

					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
					_byUSBTx[3] = (byte)ERegCmd.SetSequence;           //0x50
					_byUSBTx[4] = sequence;
					_byUSBTx[5] = this._bHW_DevRange;				//0x5E;
					_byUSBTx[6] = this._bCMPA;					//0x19;
					_byUSBTx[7] = this._bCMPAHR;					//0x80;
					_byUSBTx[8] = this._bLimitCurrLow;			// 0xFE;
					_byUSBTx[9] = this._bLimitCurrHigh;			// 0x1F;
					_byUSBTx[10] = this._bTimeCount;

					if (mode == 1)
					{
						_byCMP_ex[0] = 0;
						_byCMP_ex[1] = 0;
					}
					_byUSBTx[11] = _byCMP_ex[0];
					_byUSBTx[12] = _byCMP_ex[1];
					_byUSBTx[13] = 0xFF;

					_seqHWRange[sequence] = this._bHW_DevRange;
					_seqCMP[sequence] = (byte)(this._bCMPA << 8) + this._bCMPAHR;
					_seqMasterGL[sequence] = this._bLimitCurrHigh;

					this.LDT3A200USB.Write(_byUSBTx, 0, 14);

					int USBbytes = this.LDT3A200USB.BytesToRead;

					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					while (USBbytes < 3)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();

					byte[] _byUSBRx = new byte[USBbytes];

					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
					{
						this.Message = this.Message + "communication error\n";
					}
				}
				catch (Exception e1)
				{
					this.Message = e1.ToString();
				}
			}
			return this.Message;
		}

		public string[] Start_Sequence(uint sequence, bool isAutoTurnOff)
		{
			this.Message = "";
			if (this._isMultiDriveState == true)
			{
				this.Multi_Getdata();
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
			}

			this._bExecMode = (isAutoTurnOff == true) ? (byte)0x65 : (byte)0x67;

			if (this._isMultiDriveState == false && this._isMultiCurveState == false)
			{
				try
				{
					//if (executeMode > 0xFE) executeMode = 0xFE;
					if (sequence > 0xFE) { sequence = 0xFE; }
					if (sequence > 0)
					{
						_seqIndex = sequence - 1;
					}
					else
					{
						_seqIndex = 0;
					}

					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
					_byUSBTx[3] = (byte)ERegCmd.SetSequence;           //0x50
					_byUSBTx[4] = (byte)sequence;
					_byUSBTx[5] = this._bExecMode;
					_byUSBTx[6] = 0xFF;

					this.LDT3A200USB.Write(_byUSBTx, 0, 7);
					int USBbytes = this.LDT3A200USB.BytesToRead;

					while (USBbytes < 3)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
					}

					byte[] _byUSBRx = new byte[3];
					this.LDT3A200USB.Read(_byUSBRx, 0, 3);
					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
					{
						this.Message = this.Message + "communication error\n";
					}

					USBbytes = this.LDT3A200USB.BytesToRead;
					//-------------------------------------------------------------------------------------------------
					// DSP Side Return : 0xFF 0xFF 0x50 Num AD0MA (2byte) AD1MA(2byte) AD2MA(2byte) = 10 bytes
					//-------------------------------------------------------------------------------------------------
					if (sequence > 0)
					{
						while (USBbytes < (10 * sequence))
						{
							USBbytes = this.LDT3A200USB.BytesToRead;
							Thread.Sleep(0);
						}
						_byUSBRx = new byte[USBbytes];
						this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

						if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x50)
						{
							this.Message = "communication error\n";
						}

						UInt16 AD0 = 0, AD1 = 0, AD2 = 0;
						for (int i = 0; i < sequence; i++)
						{
							AD0 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[(10 * i) + 4]) + (Convert.ToUInt16(_byUSBRx[(10 * i) + 5]) * 256));
							AD1 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[(10 * i) + 6]) + (Convert.ToUInt16(_byUSBRx[(10 * i) + 7]) * 256));
							AD2 = Convert.ToUInt16(Convert.ToUInt16(_byUSBRx[(10 * i) + 8]) + (Convert.ToUInt16(_byUSBRx[(10 * i) + 9]) * 256));


							//--------------------------------------------------------------------------------------
							// cal_4Wire() 執行前，一定要先執行 cal_IF, 算電流流過的一個電壓補償值。
							//--------------------------------------------------------------------------------------
							this._sRtnIF = cal_IF(_seqHWRange[i], _seqCMP[i], _seqMasterGL[i], AD0);

							if ((this._bStatus & 0x04) == 0x04)
							{
								this._sRtnVS = this.cal_4Wire(_seqHWRange[i], _seqCMP[i], AD1, AD2);
							}
							else
							{
								if (((_seqHWRange[i] & 0x60) == 0x60) || ((_seqHWRange[i] & 0x40) == 0x40))
								{
									this._sRtnVS = this.cal_2Wire(_seqHWRange[i], _seqCMP[i], AD1, AD2);		// 用2線式的 cal_2Wire() 計算， 並沒有 AD1 的數據 ? // Gilbert 不對吧 !!	
								}
								else
								{
									this._sRtnVS = this.cal_4Wire(_seqHWRange[i], _seqCMP[i], AD1, AD2);
								}
							}
							this._sMeasureIV[i * 8] = this._sRtnIF[0];
							this._sMeasureIV[(i * 8) + 1] = this._sRtnIF[1];
							this._sMeasureIV[(i * 8) + 2] = this._sRtnVS[0];
							this._sMeasureIV[(i * 8) + 3] = this._sRtnVS[1];
							this._sMeasureIV[(i * 8) + 4] = this._sRtnVS[2];
							this._sMeasureIV[(i * 8) + 5] = this._sRtnVS[3];
							this._sMeasureIV[(i * 8) + 6] = this._sRtnVS[4];
							this._sMeasureIV[(i * 8) + 7] = this._sRtnVS[5];
						}
					}
				}
				catch (Exception e1)
				{
					this.Message = e1.ToString();
				}
			}
			return this._sMeasureIV.Clone() as string[];
		}

		public string[] Single_Sequence(float V, float I, string I_unit, byte time, byte calcMode, byte executeMode)
		{
			if (this._isMultiDriveState == true)
			{
				this.Multi_Getdata();
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
			}
			if (this._isMultiDriveState == false && this._isMultiCurveState == false)
			{
				if ((I < 0.0f) && (V > 0.0f))
				{
					V = 0.0f - V;
				}

				I = Math.Abs(I);

				if (V == 8)
				{
					this.runSingleSequence(6, I, I_unit, time, calcMode, executeMode);

					if (Convert.ToSingle(this._sMeasureIV[6]) >= 5.9)
					{
						this.runSingleSequence(8, I, I_unit, time, calcMode, executeMode);
					}
				}
				else
				{
					this.runSingleSequence(V, I, I_unit, time, calcMode, executeMode);
				}
			}
			return this._sMeasureIV;
		}

		public string[] ReadCurve(byte channel, int point)
		{
			this.Message = "";
			try
			{
				if (this._isMultiDriveState == true)
				{
					this.Multi_Getdata();
				}
				if (this._isMultiCurveState == true)
				{
					this.Multi_GetCurveData();
				}

				if (this._isMultiDriveState == false && this._isMultiCurveState == false)
				{
					if (channel > 0xFE) channel = 0xFE;
					if (point < 1) point = 1;
					if (point > 2000) point = 2000;

					_byUSBTx[0] = 0;
					_byUSBTx[1] = 0;
					_byUSBTx[2] = SOF;
					_byUSBTx[3] = (byte)ERegCmd.ReadCurve;                     //0x43
					_byUSBTx[4] = channel;
					_byUSBTx[5] = Convert.ToByte(point & 0xFF);

					if (_byUSBTx[5] > 0) _byUSBTx[5] = Convert.ToByte(_byUSBTx[5] - 1);

					_byUSBTx[6] = Convert.ToByte((point / 256) & 0xFF);
					_byUSBTx[7] = 0xFF;
					this.LDT3A200USB.Write(_byUSBTx, 0, 8);

					point = ((_byUSBTx[6] * 256) + _byUSBTx[5] + 1) * 2 + 4;

					int USBbytes = this.LDT3A200USB.BytesToRead;
					System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
					this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

					while (USBbytes < point)
					{
						USBbytes = this.LDT3A200USB.BytesToRead;
						Thread.Sleep(0);
						if (this._isOverTime == true)
						{
							this.Message = this.Message + "timeout\n";
							break;
						}
					}
					this._isOverTime = false;
					this.loopTimer.Dispose();
					byte[] _byUSBRx = new byte[USBbytes];
					this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

					if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x43)
					{
						this.Message = this.Message + "communication error\n";
					}
					UInt16 temp = 0;

					point = ((_byUSBTx[6] * 256) + _byUSBTx[5] + 1);
					temp = Convert.ToUInt16(_byUSBRx[4] + _byUSBRx[5] * 256);

					if (channel == 0)			// AD0, IF, F-, Current Measurement,用 cal_IF() 計算
					{
						string[] rtnIF = cal_IF(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], _seqMasterGL[_seqIndex], temp);
						this._sCurveData[0] = rtnIF[1];
						for (int index = 0; index < point; index++)
						{
							temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);
							rtnIF = cal_IF(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], _seqMasterGL[_seqIndex], temp);
							this._sCurveData[index + 1] = rtnIF[0];
						}
					}
					else if (channel == 1)		// AD1, VS_Anode, S+, Forward Voltage,用4線式的 cal_4Wire() 計算，將 AD2 = 0, 作為計算基準點
					{
						string[] rtnVSmeasure = cal_4Wire(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], temp, 0);
						this._sCurveData[0] = "V";
						for (int index = 0; index < point; index++)
						{
							temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);
							rtnVSmeasure = cal_4Wire(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], temp, 0);
							this._sCurveData[index + 1] = rtnVSmeasure[0];
						}
					}
					else if (channel == 2)		// AD2, VS_Cathode, S-, Zero Voltage,用2線式的 cal_2Wire() 計算， 將 AD1 = 0 作為基準點
					{
						string[] rtnVS_Cathode = cal_2Wire(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], 0, temp);
						this._sCurveData[0] = "V";
						for (int index = 0; index < point; index++)
						{
							temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);
							rtnVS_Cathode = cal_2Wire(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], 0, temp);
							this._sCurveData[index + 1] = rtnVS_Cathode[2];
						}
					}
					else						// channel =3, AD1C, CMPA & CMPAHR, F+, driving Voltage,用2線式的 cal_2Wire() 計算， 將 AD2 = 0 作為基準點 
					{							// (_byUSBRx[3] == 3), channel = 3 = AD1C, right now,  temp = feedbackCMP
						string[] rtnVS_Cathode = cal_2Wire(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], temp, 0);
						this._sCurveData[0] = "V";
						for (int index = 0; index < point; index++)
						{
							temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);
							rtnVS_Cathode = cal_2Wire(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], temp, 0);
							this._sCurveData[index + 1] = rtnVS_Cathode[0];
						}
					}
				}
			}
			catch (Exception e1)
			{
				this.Message = e1.ToString();
			}

			return this._sCurveData.Clone() as string[];
		}

		//public void ReadCurveToFile(byte channel, int point, string fileName)
		//{
		//    Message = "";
		//    try
		//    {
		//        if (this._isMultiDriveState == true)
		//        {
		//            this.Multi_Getdata();
		//        }
		//        if (this._isMultiCurveState == true)
		//        {
		//            this.multi_GetCurveData();
		//        }
		//        if (this._isMultiDriveState == false && this._isMultiCurveState == false)
		//        {
		//            if (channel > 0xFE) channel = 0xFE;
		//            if (point < 1) point = 1;
		//            if (point > 2000) point = 2000;

		//            _byUSBTx[0] = 0;
		//            _byUSBTx[1] = 0;
		//            _byUSBTx[2] = SOF;
		//            _byUSBTx[3] = (byte)ERegCmd.ReadCurve;                          //0x43
		//            _byUSBTx[4] = channel;
		//            _byUSBTx[5] = Convert.ToByte(point & 0xFF);

		//            if (_byUSBTx[5] > 0)
		//            {
		//                _byUSBTx[5] = Convert.ToByte(_byUSBTx[5] - 1);
		//            }

		//            _byUSBTx[6] = Convert.ToByte((point / 256) & 0xFF);
		//            _byUSBTx[7] = 0xFF;

		//            this.LDT3A200USB.Write(_byUSBTx, 0, 8);
		//            point = ((_byUSBTx[6] * 256) + _byUSBTx[5] + 1) * 2 + 4;

		//            int USBbytes = this.LDT3A200USB.BytesToRead;
		//            System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
		//            this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

		//            while (USBbytes < point)
		//            {
		//                USBbytes = this.LDT3A200USB.BytesToRead;
		//                Thread.Sleep(0);
		//                if (this._isOverTime == true)
		//                {
		//                    this.Message = this.Message + "timeout\n";
		//                    break;
		//                }
		//            }
		//            this._isOverTime = false;
		//            this.loopTimer.Dispose();

		//            byte[] _byUSBRx = new byte[USBbytes];
		//            this.LDT3A200USB.Read(_byUSBRx, 0, USBbytes);

		//            if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x43)
		//            {
		//                this.Message = this.Message + "communication error\n";
		//            }
		//            UInt16 temp = 0;
		//            point = ((_byUSBTx[6] * 256) + _byUSBTx[5] + 1);

		//            using (StreamWriter writer = new StreamWriter(fileName, true))
		//            {
		//                temp = Convert.ToUInt16(_byUSBRx[4] + _byUSBRx[5] * 256);

		//                if (channel == 0)
		//                {
		//                    string[] rtnIF = cal_IF(sequence_Range[_seqIndex], sequence_CMP[_seqIndex], sequence_Limit_I1[_seqIndex], temp);
		//                    writer.WriteLine(rtnIF[1]);
		//                    for (int index = 0; index < point; index++)
		//                    {
		//                        temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);
		//                        rtnIF = cal_IF(sequence_Range[_seqIndex], sequence_CMP[_seqIndex], sequence_Limit_I1[_seqIndex], temp);
		//                        writer.WriteLine(rtnIF[0]);
		//                    }
		//                }
		//                else if (channel == 1)
		//                {
		//                    string[] rtnVSmeasure = cal_4Wire(sequence_Range[_seqIndex], sequence_CMP[_seqIndex], temp, 0);
		//                    for (int index = 0; index < point; index++)
		//                    {
		//                        temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);
		//                        rtnVSmeasure = cal_4Wire(sequence_Range[_seqIndex], sequence_CMP[_seqIndex], temp, 0);
		//                        string VS = rtnVSmeasure[0] + "," + rtnVSmeasure[1];
		//                        writer.WriteLine(VS);
		//                    }
		//                }
		//                else if (channel == 2)
		//                {
		//                    string[] rtnVS_Cathode = cal_2Wire(sequence_Range[_seqIndex], sequence_CMP[_seqIndex], 0, temp);
		//                    for (int index = 0; index < point; index++)
		//                    {
		//                        temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);
		//                        rtnVS_Cathode = cal_2Wire(sequence_Range[_seqIndex], sequence_CMP[_seqIndex], 0, temp);
		//                        string VS = rtnVS_Cathode[2] + "," + rtnVS_Cathode[3];
		//                        writer.WriteLine(VS);
		//                        //writer.WriteLine(temp);
		//                    }
		//                }
		//                else
		//                {
		//                    string[] rtnVS_Cathode = cal_2Wire(sequence_Range[_seqIndex], sequence_CMP[_seqIndex], temp, 0);
		//                    for (int index = 0; index < point; index++)
		//                    {
		//                        temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);
		//                        //writer.WriteLine(temp);
		//                        rtnVS_Cathode = cal_2Wire(sequence_Range[_seqIndex], sequence_CMP[_seqIndex], temp, 0);
		//                        string VS = rtnVS_Cathode[0] + "," + rtnVS_Cathode[1];
		//                        writer.WriteLine(VS);
		//                    }
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception e1)
		//    {
		//        this.Message = e1.ToString();
		//    }
		//}

		#endregion

		#region >>> Public Methods New <<<

		public double[] Single_Sequence_New(double V, E_VRange vRange, double I, E_IRange iRange, double msTime, bool isAutoTurnOff)
		{
			if (this._isMultiDriveState == true)
			{
                this.Multi_Drive_GetData();
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
			}

			this._vRangeSet = vRange;
			this._iRangeSet = iRange;
			this._bExecMode = (isAutoTurnOff == true) ? (byte)0x65 : (byte)0x67;
			//this._currSetUnit = iRange.ToString().Remove(0, 1);

			if ((I < 0.0) && (V > 0.0))
			{
				V = 0.0 - V;
			}

			I = Math.Abs(I);

            if (this._isCompensateV)
            {
                this.Cmd_V_Convert02(V, false);
            }
            else
            {
                this.Cmd_V_Convert(V, false);
            }
         
			this.Cmd_I_Convert(V, I, "uA", false);

			this.SetForceTime(msTime);

			if (this._isMultiDriveState == false && this._isMultiCurveState == false)
			{
				this.runSingleSequence_New();
			}

			return this._rtnValue;
		}

		public double[] Single_Sequence_New(double V,double I, string currUint, double msTime, bool isAutoTurnOff)
		{
			if (this._isMultiDriveState == true)
			{
                this.Multi_Drive_GetData();
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
			}

			this._bExecMode = (isAutoTurnOff == true) ? (byte)0x65 : (byte)0x67;

			if ((I < 0.0) && (V > 0.0))
			{
				V = 0.0 - V;
			}

			//this.Cmd_V_Convert(V, true);

            if (this._isCompensateV)
            {
                this.Cmd_V_Convert02(V, true);
            }
            else
            {
                this.Cmd_V_Convert(V, true);
            }

			this.Cmd_I_Convert(V, I, currUint, true);

			this.SetForceTime(msTime);

			if (this._isMultiDriveState == false && this._isMultiCurveState == false)
			{
				//this.runSingleSequence_New();
                this.TriggerSingleSequence();


                this.AcquireSingleSequenceResult();
				//if (V == 8)
				//{
				//    this.runSingleSequence( 6, (float)I, this._currUnit, time, calcMode, execMode);
				//    if (Convert.ToSingle(this.measure_IV[6]) >= 5.9)
				//    {
				//        this.runSingleSequence( 8, (float)I, this._currUnit, time, calcMode, execMode);
				//    }
				//}
				//else
				//{
				//    this.runSingleSequence( (float) V, (float)I, this._currUnit, time, calcMode, execMode);
				//}
			}

			return this._rtnValue;
		}

        public void Multi_Drive_New(double V, E_VRange vRange, double I, E_IRange iRange, double msTime, bool isAutoTurnOff)
        {
            if (this._isMultiDriveState == true)
            {
                this.Multi_Drive_GetData();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }

            this._vRangeSet = vRange;
            this._iRangeSet = iRange;
            this._bExecMode = (isAutoTurnOff == true) ? (byte)0x65 : (byte)0x67;

            if ((I < 0.0) && (V > 0.0))
            {
                V = 0.0 - V;
            }

            if (this._isCompensateV)
            {
                this.Cmd_V_Convert02(V, false);
            }
            else
            {
                this.Cmd_V_Convert(V, false);
            }

            this.Cmd_I_Convert(V, I, "A", false);

            this.SetForceTime(msTime);

            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                this.TriggerSingleSequence();

                this._isMultiDriveState = true;
            }
        }

        public void Multi_Drive_New(double V, double I, string currUint, double msTime, bool isAutoTurnOff)
        {
            if (this._isMultiDriveState == true)
            {
                this.Multi_Drive_GetData();
            }
            if (this._isMultiCurveState == true)
            {
                this.Multi_GetCurveData();
            }

            this._bExecMode = (isAutoTurnOff == true) ? (byte)0x65 : (byte)0x67;

            if ((I < 0.0) && (V > 0.0))
            {
                V = 0.0 - V;
            }

            //this.Cmd_V_Convert(V, true);

            if (this._isCompensateV)
            {
                this.Cmd_V_Convert02(V, true);
            }
            else
            {
                this.Cmd_V_Convert(V, true);
            }

            this.Cmd_I_Convert(V, I, currUint, true);

            this.SetForceTime(msTime);

            if (this._isMultiDriveState == false && this._isMultiCurveState == false)
            {
                //this.runSingleSequence_New();
                this.TriggerSingleSequence();

                this._isMultiDriveState = true;
                //this.AcquireSingleSequenceResult();
                //if (V == 8)
                //{
                //    this.runSingleSequence( 6, (float)I, this._currUnit, time, calcMode, execMode);
                //    if (Convert.ToSingle(this.measure_IV[6]) >= 5.9)
                //    {
                //        this.runSingleSequence( 8, (float)I, this._currUnit, time, calcMode, execMode);
                //    }
                //}
                //else
                //{
                //    this.runSingleSequence( (float) V, (float)I, this._currUnit, time, calcMode, execMode);
                //}
            }
        }

        public double[] Multi_Drive_GetData()
        {
            this.AcquireSingleSequenceResult();

            this._isMultiDriveState = false;

            return this._rtnValue;
        }

		public double[] Single_Sequence_New(uint ePWM_CMP, E_VRange vRange, uint clampI, E_IRange iRange, double msTime, bool isAutoTurnOff)
		{
			if (this._isMultiDriveState == true)
			{
				this.Multi_Getdata();
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
			}
			
			this._vRangeSet = vRange;
			this._iRangeSet = iRange;
            this._bHW_DevRange = Convert.ToByte((byte)vRange + (byte)iRange);  // Roy
			this._bExecMode = (isAutoTurnOff == true) ? (byte)0x65 : (byte)0x67;
			this._bCMPA = (byte)((ePWM_CMP & 0xFF00) >> 8);
			this._bCMPAHR = (byte)(ePWM_CMP & 0x00FF);

			this.SetForceTime(msTime);

            clampI = clampI & 0x7FFF;

			if (( this._iRangeSet == E_IRange._10uA ) || ( this._iRangeSet == E_IRange._1mA) || this._iRangeSet == E_IRange._100mA )			
            {
                
				this._bLimitCurrLow = Convert.ToByte((clampI << 1) & 0xFF);
				this._bLimitCurrHigh = Convert.ToByte((clampI << 1) >> 8);
			}
			else
			{
				this._bLimitCurrLow = Convert.ToByte((this._nClampI << 1) & 0xFF);
				this._bLimitCurrHigh = Convert.ToByte(((this._nClampI << 1) >> 8) + 0x80);
			}

			////if (_sIRange == "10uA" || _sIRange == "1mA" || _sIRange == "100mA")		// MasterGainLoop = 0
			//{
			//    this._bLimitCurrLow = Convert.ToByte((this._nClampI << 1) & 0xFF);
			//    this._bLimitCurrHigh = Convert.ToByte((this._nClampI << 1) >> 8);
			//}
			//else																		// MasterGainLoop = 1
			//{
			//    this._bLimitCurrLow = Convert.ToByte((this._nClampI << 1) & 0xFF);
			//    this._bLimitCurrHigh = Convert.ToByte(((this._nClampI << 1) >> 8) + 0x80);
			//}

			if (this._isMultiDriveState == false && this._isMultiCurveState == false)
			{
                this.runSingleSequence_New();	//bCMP_A, bCMP_AHR, bLimit_I);   // bLimit_I, it seems it's not need to set this parameter

				//if (V == 8)
				//{
				//    this.runSingleSequence( 6, (float)I, this._currUnit, time, calcMode, execMode);
				//    if (Convert.ToSingle(this.measure_IV[6]) >= 5.9)
				//    {
				//        this.runSingleSequence( 8, (float)I, this._currUnit, time, calcMode, execMode);
				//    }
				//}
				//else
				//{
				//    this.runSingleSequence( (float) V, (float)I, this._currUnit, time, calcMode, execMode);
				//}
			}

			return this._rtnValue;
		}

		public double[] ReadCurve_New(uint channel, uint point)
		{ 
			this.Message = "";

			if (this._isMultiDriveState == true)
			{
                this.Multi_Drive_GetData();
				return new double[] {0.0};
			}
			if (this._isMultiCurveState == true)
			{
				this.Multi_GetCurveData();
				return new double[] {0.0};
			}

			if (channel <= 3)
			{
				this.ReadCurveFromDev(channel, point);
				return  this._thy4ChData[channel];
			}
			else
			{
				for (uint p = 0; p <= 3; p++)
				{
					this.ReadCurveFromDev(p, point);
				}
				return  this._thy4ChData[0];
			}		
		}

        public void Multi_BeginReadCurve(uint channel, uint point)
        {
            this.Message = "";
            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Drive_GetData();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }
                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    if (channel > 0xFE) channel = 0xFE;
                    if (point < 1) point = 1;
                    if (point > 2000) point = 2000;

                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.ReadCurve;                         //0x43  
                    _byUSBTx[4] = (byte)channel;
                    _byUSBTx[5] = Convert.ToByte(point & 0xFF);

                    if (_byUSBTx[5] > 0) _byUSBTx[5] = Convert.ToByte(_byUSBTx[5] - 1);

                    _byUSBTx[6] = Convert.ToByte((point / 256) & 0xFF);
                    _byUSBTx[7] = 0xFF;

                    this.LDT3A200USB.Write(_byUSBTx, 0, 8);
                    this._curvePoints = (uint)((_byUSBTx[6] * 256) + _byUSBTx[5] + 1) * 2 + 4;
                    this._isMultiCurveState = true;
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }
        }

        public double[] Multi_GetCurveData()
        {
            ushort channel = 0;
            this.Message = "";
            try
            {
                if (this._isMultiCurveState == true)
                {
                    this.USBbytes = this.LDT3A200USB.BytesToRead;

                    if (this.USBbytes >= this._curvePoints)
                    {
                        this._byUSBRx = new byte[this.USBbytes];

                        this.LDT3A200USB.Read(_byUSBRx, 0, this.USBbytes);

                        if (_byUSBRx[0] != 0xFF || _byUSBRx[1] != 0xFF || _byUSBRx[2] != 0x43)
                        {
                            this.Message = this.Message + "communication error\n";
                        }

                        UInt16 temp = 0;

                        channel = Convert.ToUInt16(_byUSBRx[3]);

                        this._curvePoints = (this._curvePoints - 4) / 2;

                        temp = Convert.ToUInt16(_byUSBRx[4] + _byUSBRx[5] * 256);

                        for (int index = 0; index < this._curvePoints; index++)
                        {
                            temp = Convert.ToUInt16(_byUSBRx[index * 2 + 4] + _byUSBRx[index * 2 + 5] * 256);

                            this._thy4ChRawData[channel][index] = temp;

                            if (channel == 0)			// AD0 -> IFN
                            {
                                this._thy4ChData[channel][index] = this.calcIFN_ByAD0(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], _seqMasterGL[_seqIndex], temp);
                            }
                            else if (channel == 1)		// AD1 -> VSP
                            {
                                this._thy4ChData[channel][index] = this.calcVSP_ByAD1(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], temp);
                            }
                            else if (channel == 2)
                            {
                                this._thy4ChData[channel][index] = this.calcVSN_ByAD2(_seqCMP[_seqIndex], temp);
                            }
                            else if (channel == 3)
                            {
                                this._thy4ChData[channel][index] = this.calcVFP_ByCMP(_seqHWRange[_seqIndex], _seqCMP[_seqIndex], temp);
                            }
                            else
                            {
                                this._thy4ChData[channel][index] = 0;
                            }
                        }				

                        this._isMultiCurveState = false;
                    }
                    else
                    {
                        this._sCurveData[0] = "busy";
                        return null;
                    }
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();
            }

            return this._thy4ChData[channel];
        }

        public bool Multi_GetFwCalcThyResult()
        {
            this.Message = "";

            try
            {
                if (this._isMultiDriveState == true)
                {
                    this.Multi_Drive_GetData();
                }
                if (this._isMultiCurveState == true)
                {
                    this.Multi_GetCurveData();
                }

                if (this._isMultiDriveState == false && this._isMultiCurveState == false)
                {
                    _byUSBTx[0] = 0;
                    _byUSBTx[1] = 0;
                    _byUSBTx[2] = SOF;
                    _byUSBTx[3] = (byte)ERegCmd.GetThyData;                     //0x74
                    _byUSBTx[4] = 0;
                    _byUSBTx[5] = 0;
                    _byUSBTx[6] = 0xFF;

                    this.LDT3A200USB.Write(_byUSBTx, 0, 7);
                    this.USBbytes = this.LDT3A200USB.BytesToRead;
                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(this.TimerTask);
                    this.loopTimer = new System.Threading.Timer(TimerDelegate, null, 5000, Timeout.Infinite);

                    //-------------------------------------------------------------------------------------------------
                    // DSP Side Return : 0xFF 0xFF 0x74 + 6 x 2 byte data = 15 bytes
                    // Vpeak_VS_Anode,	Vpeak_VS_Cathode,	Vstable_VS_Anode,	Vstable_VS_Cathode,	Vpoint_VS_Anode,	Vpoint_VS_Cathode
                    //-------------------------------------------------------------------------------------------------
                    while (this.USBbytes < 15)
                    {
                        this.USBbytes = this.LDT3A200USB.BytesToRead;
                        Thread.Sleep(0);
                        if (this._isOverTime == true)
                        {
                            this.Message = this.Message + "timeout\n";
                            break;
                        }
                    }
                    this._isOverTime = false;
                    this.loopTimer.Dispose();
                    this._byUSBRx = new byte[this.USBbytes];
                    this.LDT3A200USB.Read(this._byUSBRx, 0, this.USBbytes);

                    if (this._byUSBRx[0] != 0xFF || this._byUSBRx[1] != 0xFF || this._byUSBRx[2] != 0x74)
                    {
                        this.Message = this.Message + "communication error\n";
                    }

                    UInt16 AD0 = 0;
                    UInt16 AD1p = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[3]) + (Convert.ToUInt16(this._byUSBRx[4]) * 256));
                    UInt16 AD2p = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[5]) + (Convert.ToUInt16(this._byUSBRx[6]) * 256));
                    UInt16 AD1s = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[7]) + (Convert.ToUInt16(this._byUSBRx[8]) * 256));
                    UInt16 AD2s = Convert.ToUInt16(Convert.ToUInt16(this._byUSBRx[9]) + (Convert.ToUInt16(this._byUSBRx[10]) * 256));

                    this._execIRange = this.GetIRange(_seqHWRange[0], _seqMasterGL[0]);
                    this._execVRange = (E_VRange)(_seqHWRange[0] & 0xE0);

                    //------------------------------------------------------------------------------------------------------------------------
                    double tempVpp = 0.0d;
                    double tempVss = 0.0d;
                    double tempVdd = 0.0d;

                    this.cal_4Wire_New(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0, AD1p, AD2p);

                    tempVpp = this._rtnValue[(int)ERtnValue.VDD];


                    this.cal_4Wire_New(_seqHWRange[0], _seqCMP[0], _seqMasterGL[0], AD0, AD1s, AD2s);

                    tempVss = this._rtnValue[(int)ERtnValue.VDD];

                    if (tempVpp < tempVss)
                    {
                        tempVpp = tempVss;
                    }

                    //this._pt.Reset();

                    //this._pt.Start();

                    double[] arrayAD1 = this.ReadCurve_New((uint)ERegADChannel.VSP, 100);

                    double[] arrayAD2 = this.ReadCurve_New((uint)ERegADChannel.VSN, 100);

                    //this._pt.Stop();

                    //double timeSpan = this._pt.GetTimeSpan(MPI.ETimeSpanUnit.MilliSecond);

                    //string time = timeSpan.ToString();

                    for (int i = 0; i < 100; i++)
                    {
                        if ((arrayAD1[i] - arrayAD2[i]) > tempVpp)
                        {
                            tempVpp = arrayAD1[i] - arrayAD2[i];
                        }
                    }
                    //0.0.4.0
                    tempVdd = tempVpp - tempVss;

                    // Clear rtnValueArray
                    for (int index = 0; index < this._rtnValue.Length; index++)
                    {
                        this._rtnValue[index] = 0.0d;
                    }

                    this._rtnValue[(int)ERtnFwCalcThyValue.VDD_Peak] = tempVpp;
                    this._rtnValue[(int)ERtnFwCalcThyValue.VDD_Stable] = tempVss;
                    this._rtnValue[(int)ERtnFwCalcThyValue.VDD_Diff] = Math.Abs(tempVdd);
                }
            }
            catch (Exception e1)
            {
                this.Message = e1.ToString();

                return false;
            }

            return true;
        }

		#endregion

	}


}
