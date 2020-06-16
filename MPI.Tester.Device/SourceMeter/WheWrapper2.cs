using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SourceMeter
{
    public struct MeterStatus
    {
        public int PMU;
        public int ForceType;
        public double ForceValue;
        public int Polarity;
        public int I_Range;
        public int V_Range;
        public int RelayState;        

        public int MsrtType;
        public double MsrtAvgValue;
        public int MsrtCount;
        public int PulseCount;        
    }

	public class WheWrapper2
	{
        public const int MAX_MSRT_AVG_COUNT = 500;
        private enum EForceType : int
        {
            FV = 0,
            FI = 1,
        }

		// { VoltRange_Num , CurrentRange_Num }
        private const int PMU01_VOLT_RANGE_NUM = 2;
        private const int PMU02_CURRENT_RANGE_NUM = 7;

        private int[][] _pmuConfig = new int[][] { new int[] { PMU01_VOLT_RANGE_NUM, PMU02_CURRENT_RANGE_NUM }, new int[] { 1, 1 } }; 
		private int _lastPMU;
		private int _lastVoltRangeIndex;
		private int _lastCurrnetRangeIndex;
		private int _lastPolarity;          // POSITIVE = 0; NEGATIVE = 1

        private EForceType _forceType;
        private double[] _msrtRawData = new double[MAX_MSRT_AVG_COUNT];
        private MeterStatus _status;
        private List<double> _msrtDataList;

		#region >>> Import DLL <<<

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_INIT", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		private static extern int TESTER_INIT();

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_CLOSE", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TESTER_CLOSE();

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_RESET", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TESTER_RESET();

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "WAIT", CharSet = CharSet.Auto , CallingConvention = CallingConvention.Cdecl)]
        private static extern void WAIT(double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_ID", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TESTER_ID();

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_VERSION", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TESTER_VERSION();

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "HCFV", CharSet = CharSet.Auto , CallingConvention = CallingConvention.Cdecl)]  
        private static extern void HCFV(int polarity, double volt, int vrng, int irng, double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "HCFI", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HCFI(int polarity, double current, int vrng, int irng, double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "HVFV", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HVFV(int polarity, double volt, double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "HVFI", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HVFI(int polarity, double current, double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_MV", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern double PMU_MV(int type, double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_AMV", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern double PMU_AMV(int type, int cnt, double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_MI", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern double PMU_MI(int type, double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_AMI", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern double PMU_AMI(int type, int cnt, double delay);

        // Call this function when end of polling trig in ONLY
        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TRIG_RESET", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TRIG_RESET();

		// inTrigMode : one of TRIG_IN_FALLING or TRIG_IN_RISING
		// outTrigMode : one of TRIG_OUT_ACTION_LOW or TRIG_OUT_ACTION_HIGH
        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TRIG_MODE", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TRIG_MODE(int inTrigMode, int outTrigMode);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TRIG_OUT", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TRIG_OUT(int trig, bool flag);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TRIG_IN", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]        
        private static extern int TRIG_IN();

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_POWER", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool TESTER_POWER();

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "SW_VERSION", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SW_VERSION();

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "OUTPUT_SHORT", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int OUTPUT_SHORT(bool flag);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "DISCONNECT", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int DISCONNECT();

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_STATUS", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TESTER_STATUS(ref int pmu, ref int mode, ref int polarity, ref double  value, ref int vRng, ref int iRng, ref int relay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "GET_ROW_DATA", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GET_ROW_DATA(ref int type, ref int mode, ref int measCnt, ref Double pDouble);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_AMV02", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern double PMU_AMV02(int type,int cnt, int plusCnt, double delay);

        [DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_AMI02", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern double PMU_AMI02(int type, int cnt, int plusCnt, double delay);

		
        #endregion

		#region >>> Public Method <<<

		public int Init()
		{
            if (    System.IO.File.Exists(@"C:\MPI\LEDTester\Driver\TESTAPI.dll") == false ||
                     System.IO.File.Exists(@"C:\MPI\LEDTester\Driver\WHPCI.dll") == false)
                return -12345;

			this._lastPMU = 0;
			this._lastPolarity = 0;
            this._forceType = EForceType.FV;
            this._status = new MeterStatus();
            this._msrtDataList = new List<double>(MAX_MSRT_AVG_COUNT);

			return TESTER_INIT();
		}
		
		public int Close()
		{			
			 return TESTER_CLOSE();
		}
		
		public void Reset()
		{
			TESTER_RESET();
			TRIG_RESET();

			// TRIG_MODE ( inTrigMode , outTrigMode );
			// inTrigMode : one of TRIG_IN_FALLING or TRIG_IN_RISING
			// outTrigMode : one of TRIG_OUT_ACTION_LOW or TRIG_OUT_ACTION_HIGH
			TRIG_MODE(0, 1);
		}
		
		public void HWTimerWait(double delay)
		{
			WAIT(delay);
		}
		
		public int GetSerialNum()
		{
			return TESTER_ID();
		}
		
		public int GetHardwareVersion()
		{
			return TESTER_VERSION();
		}

		public int GetSoftwareVersion()
		{
			return SW_VERSION();
		}

		//public void ForceV(double volt, int voltRangeIndex, int currentRangeIndex, double currentClampHigh, double currentClampLow, double delay)
		//{
		//    PMU_FV(volt, voltRangeIndex, currentRangeIndex, currentClampHigh, currentClampLow, delay);
		//}

		public bool ForceV(int pmu, int polarity, double volt, int voltRangeIndex, int currentRangeIndex,double delay)
		{
            this._forceType = EForceType.FV;
			if (pmu < _pmuConfig.Length)
			{
				 this._lastPMU = pmu;
			}
			else
			{
				 return false;
			}

			this._lastPolarity = polarity;

            switch( pmu )
            {
	            case 0 :
		            if (voltRangeIndex < _pmuConfig[0][0] && currentRangeIndex < _pmuConfig[0][1])
		            {
		                this._lastVoltRangeIndex = voltRangeIndex;
		                this._lastCurrnetRangeIndex = currentRangeIndex;
		                HCFV(polarity, volt, voltRangeIndex, currentRangeIndex, delay);
		            }
		            else
		            {
			            return false;
		            }
		            break;
                //------------------------------------------------
                case 1 :
	                if (voltRangeIndex < _pmuConfig[1][0] && currentRangeIndex < _pmuConfig[1][1])
	                {
		                this._lastVoltRangeIndex = voltRangeIndex;
		                this._lastCurrnetRangeIndex = currentRangeIndex;
		                HVFV(polarity, volt, delay);
	                }
	                else
	                {
		                return false;
	                }
	                break;
                 //------------------------------------------------
				 default:
					  break;
			}

            //this.GetMeterStatus();
			return true;
		}

		//public void ForceI(double current, int voltRangeIndex, int currentRangeIndex, double voltClampHigh, double voltClampLow, double delay)
		//{          
		//    PMU_FI(current, voltRangeIndex, currentRangeIndex, voltClampHigh, voltClampLow, delay);
		//}

		public bool ForceI(int pmu, int polarity, double current, int voltRangeIndex, int currentRangeIndex, double delay)
		{
            this._forceType = EForceType.FI;
			if (pmu < _pmuConfig.Length)
			{
				 this._lastPMU = pmu;
			}
			else
			{
				 return false;
			}

			this._lastPolarity = polarity;

			switch (pmu)
			{
				 case 0:
					  if (voltRangeIndex < _pmuConfig[0][0] && currentRangeIndex < _pmuConfig[0][1])
					  {
							this._lastVoltRangeIndex = voltRangeIndex;
							this._lastCurrnetRangeIndex = currentRangeIndex;
							HCFI(polarity, current, voltRangeIndex, currentRangeIndex, delay);
					  }
					  else
					  {
							return false;
					  }
					  break;
                 //------------------------------------------------
				 case 1:
					  if (voltRangeIndex < _pmuConfig[1][0] && currentRangeIndex < _pmuConfig[1][1])
					  {
							this._lastVoltRangeIndex = voltRangeIndex;
							this._lastCurrnetRangeIndex = currentRangeIndex;
							HVFI(polarity, current , delay);
					  }
					  else
					  {
							return false;
					  }
					  break;
                 //------------------------------------------------
				 default:
					  break;
			}

            //this.GetMeterStatus();
			return true;
		}

		public double MeasureV(int PMU, double delay)
		{
            double readValue = 0.0d;
            if (PMU == this._lastPMU)
            {
                if (delay < 0.0d)
                {
                    delay = 0.0d;
                }
                switch (this._lastPolarity)         
                {
                    case 0:     // POSITIVE = 0
                        readValue = PMU_MV(PMU, delay);
                        break;
                    //-------------------------------------------------------------
                    case 1:     // NEGATIVE = 1                    
                        readValue = ( PMU_MV(PMU, delay) * (-1.0) );
                        break;
                    //-------------------------------------------------------------
                    default :
                        readValue = 0.0d;
                        break;
                }
            }

            this._status.MsrtAvgValue = readValue;
            this._status.PulseCount = 0;
            this._status.MsrtCount = 1;
            return readValue;
		}

		public double MeasureAvgV(int PMU, int count ,double delay)
		{
            double readValue = 0.0d;

            if (PMU == this._lastPMU)
            {
                if (delay < 0.0d)
                {
                    delay = 0.0d;
                }

                if (count < 1)
                {
                    count = 1;
                }

                if (count + 2 >= MAX_MSRT_AVG_COUNT)
                {
                    count = MAX_MSRT_AVG_COUNT - 2;
                }

                switch (this._lastPolarity)
                {
                    case 0:     // POSITIVE = 0
                        readValue = PMU_AMV(PMU, count, delay);
                        break;
                    //-------------------------------------------------------------
                    case 1:    // NEGATIVE = 1                 
                        readValue = (PMU_AMV(PMU, count, delay) * (-1.0));
                        break;
                    //-------------------------------------------------------------
                    default:
                        readValue = 0.0d;
                        break;
                }
            }
            else
            {
                readValue = 0.0d;
            }

            GET_ROW_DATA(ref this._status.PMU, ref  this._status.MsrtType, ref  this._status.MsrtCount, ref this._msrtRawData[0]);
            this._status.MsrtAvgValue = readValue;
            this._status.PulseCount = 1;    // Default Value
            return readValue;
		}

		public double MeasureAvgV02(int PMU, int count, int plusCount, double delay)
		{
            double readValue = 0.0d;

            if (PMU == this._lastPMU)
            {
                if (delay < 0.0d)
                {
                    delay = 0.0d;
                }

                if (count < 1)
                {
                    count = 1;
                }

                if (plusCount < 0)
                {
                    plusCount = 0;
                }

                if ((count + 20) >= MAX_MSRT_AVG_COUNT ||
                    (count + plusCount * 2) >= MAX_MSRT_AVG_COUNT)
                {
                    plusCount = 10;
                    count = MAX_MSRT_AVG_COUNT - 20;
                }
                switch (this._lastPolarity)
                {
                    case 0:     // POSITIVE = 0
                        readValue = PMU_AMV02(PMU, count, plusCount, delay);
                        break;
                    //-------------------------------------------------------------
                    case 1:    // NEGATIVE = 1                 
                        readValue = (PMU_AMV02(PMU, count, plusCount, delay) * (-1.0));
                        break;
                    //-------------------------------------------------------------
                    default:
                        readValue = 0.0d;
                        break;
                }
            }
            else
            {
                readValue = 0.0d;
            }

            GET_ROW_DATA(ref this._status.PMU, ref  this._status.MsrtType, ref  this._status.MsrtCount, ref this._msrtRawData[0]);
            this._status.MsrtAvgValue = readValue;
            this._status.PulseCount = plusCount;
            return readValue;
		}

		public double MeasureI(int PMU, double delay)
		{
            double readValue = 0.0d;

            if (PMU == this._lastPMU)
            {
                if (delay < 0.0d)
                {
                    delay = 0.0d;
                }
                switch (this._lastPolarity)
                {
                    case 0:     // POSITIVE = 0
                        readValue = PMU_MI(PMU, delay);
                        break;
                    //-------------------------------------------------------------
                    case 1:     // NEGATIVE = 1
                        readValue = (PMU_MI(PMU, delay) * (-1.0));
                        break;
                    //-------------------------------------------------------------
                    default:
                        readValue = 0.0d;
                        break;
                }

            }
            else
            {
                readValue = 0.0d;
            }

            this._status.MsrtAvgValue = readValue;
            this._status.PulseCount = 0;
            this._status.MsrtCount = 1;
			return readValue;
		}

		public double MeasureAvgI(int PMU, int count, double delay)
		{
            double readValue = 0.0d;

            if (PMU == this._lastPMU)
            {
                if (delay < 0.0d)
                {
                    delay = 0.0d;
                }

                if (count < 1)
                {
                    count = 1;
                }

                if (count + 2 >= MAX_MSRT_AVG_COUNT)
                {
                    count = MAX_MSRT_AVG_COUNT - 2;
                }
                switch (this._lastPolarity)
                {
                    case 0:     // POSITIVE = 0
                        readValue = PMU_AMI(PMU, count, delay);
                        break;
                    //-------------------------------------------------------------
                    case 1:     // NEGATIVE = 1
                        readValue = (PMU_AMI(PMU, count, delay) * (-1.0));
                        break;
                    //-------------------------------------------------------------
                    default:
                        readValue = 0.0d;
                        break;
                }

            }
            else
            {
                readValue = 0.0d;
            }

            GET_ROW_DATA(ref this._status.PMU, ref  this._status.MsrtType, ref  this._status.MsrtCount, ref this._msrtRawData[0]);
            this._status.MsrtAvgValue = readValue;
            this._status.PulseCount = 1;    // Default value
            return readValue;
		}

		public double MeasureAvgI02(int PMU, int count, int plusCount, double delay)
		{
            double readValue = 0.0d;

            if (PMU == this._lastPMU)
            {
                if (delay < 0.0d)
                {
                    delay = 0.0d;
                }

                if (count < 1)
                {
                    count = 1;
                }

                if (plusCount < 0)
                {
                    plusCount = 0;
                }
                if ((count + 20) >= MAX_MSRT_AVG_COUNT ||
                    (count + plusCount * 2) >= MAX_MSRT_AVG_COUNT)
                {
                    plusCount = 10;
                    count = MAX_MSRT_AVG_COUNT - 20;
                }

                switch (this._lastPolarity)
                {
                    case 0:     // POSITIVE = 0
                        readValue = PMU_AMI02(PMU, count, plusCount, delay);
                        break;
                    //-------------------------------------------------------------
                    case 1:     // NEGATIVE = 1
                        readValue = (PMU_AMI02(PMU, count, plusCount, delay) * (-1.0));
                        break;
                    //-------------------------------------------------------------
                    default:
                        readValue = 0.0d;
                        break;
                }

            }
            else
            {
                readValue = 0.0d;
            }

            GET_ROW_DATA(ref this._status.PMU, ref  this._status.MsrtType, ref  this._status.MsrtCount, ref this._msrtRawData[0]);
            this._status.MsrtAvgValue = readValue;
            this._status.PulseCount = plusCount;
            return readValue;
		}

        public List<double> GetMsrtRowData()
		{

            GET_ROW_DATA(ref this._status.PMU, ref  this._status.MsrtType, ref  this._status.MsrtCount, ref this._msrtRawData[0] );

            this._msrtDataList.Clear();
            this._msrtDataList.AddRange(this._msrtRawData);
            this._msrtDataList.RemoveRange(this._status.MsrtCount, this._msrtRawData.Length - this._status.MsrtCount);      

            return this._msrtDataList;
		}  

		public void TurnOff(double delay) // mike 1025
		{
			switch ( this._lastPMU )
			{ 
				 case 0 :    // PMU = HC 
					  //HCFV( this._lastPolarity, 0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, delay);
					  //HCFI( this._lastPolarity, 0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, delay );
					 
					  //HCFI( this._lastPolarity, 0, 0, 0, delay );
					  // driver by max rang setting 
                      //HCFV(this._lastPolarity, 0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, delay);

                    if (this._forceType == EForceType.FI) // PMU = HC, FI
                    {
                        if (this._lastCurrnetRangeIndex == (PMU02_CURRENT_RANGE_NUM - 1))       // Special Case : 最大電流範圍運作下的關閉要加長時間
                        {
                            if (delay < 10)
                            {
                                HCFI(this._lastPolarity, 0.0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, 5);            // FI 的狀態下關閉
                                HCFV(this._lastPolarity, 0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, 5);              // FV 的狀態下關閉
                            }
                            else
                            {
                                HCFI(this._lastPolarity, 0.0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, 5);            // FI 的狀態下關閉
                                HCFV(this._lastPolarity, 0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, delay - 5 );              // FV 的狀態下關閉
                            }
                            
                        }
                        else
                        {
                            HCFI(this._lastPolarity, 0.0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, delay * 0.25);  // FI 的狀態下關閉
                            HCFV(this._lastPolarity, 0.0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, delay * 0.75);  // FV 的狀態下關閉
                        }
                    }
                    else // PMU = HC , FV
                    {
                        HCFV(this._lastPolarity, 0, this._lastVoltRangeIndex, this._lastCurrnetRangeIndex, delay);
                    }

                   // HCFV(this._lastPolarity, 0, this._lastVoltRangeIndex, 0, 0.0);      // FV = 0V, 先切 IRange 到 0 的設定
                   // HCFV(this._lastPolarity, 0, 0, 0, 0.0);                             // FV = 0V, 再切 VRange 到 0 的設定

                    this._forceType = EForceType.FV;
                    this._lastVoltRangeIndex = 0;
                    this._lastCurrnetRangeIndex = 0;
                    break;
                //---------------------------------------------------
				 case 1 :
                    if (this._forceType == EForceType.FI)   // PMU = HV, FI
                    {
                        HVFI(this._lastPolarity, 0.0, delay * 0.25);  // FI 的狀態下關閉
                        HVFV(this._lastPolarity, 0.0, delay * 0.75);  // FV 的狀態下關閉      
                    }
                    else  // PMU = HV, FV
                    {
                        HVFV(this._lastPolarity, 0.0, delay);  // FV 的狀態下關閉    
                    }
					break;
                 //---------------------------------------------------
				 default :
                    break;
			}   
		}

		public void TriggerOut(uint point, bool flag)
		{
			point &= 0x07;
			TRIG_OUT((int) point, flag );
		}

		public uint TriggerIn()
		{
			return (uint)TRIG_IN();
		}

        public void Dissconnect()
        {
            DISCONNECT();
        }

        public MeterStatus GetMeterStatus()
        {
            TESTER_STATUS(  ref this._status.PMU, ref this._status.ForceType, ref  this._status.Polarity, ref  this._status.ForceValue,
                            ref  this._status.V_Range, ref  this._status.I_Range, ref  this._status.RelayState);


            // Gilbert 先幫 Charlie 加入

            if (this._status.RelayState == 0) // All Relay is open after call Disconnect()
            {
                this._status.MsrtCount = 0;
                this._status.PulseCount = 0;
                this._status.MsrtAvgValue = 0.0d;
            }
            return this._status;
        }

		#endregion     

	}
}
