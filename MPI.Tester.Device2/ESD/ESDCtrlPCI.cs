using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.ESD
{    

    public class ESDCtrlPCI : IESDDevice
	{
        #region >>> private Const Property <<<

        //-------------------------------------------------------------------------------------------//
        // ESD Common Setting
        //-------------------------------------------------------------------------------------------//
		private const string ESD_SOFT_VER = "SW Ver. PCI_001";
		private const string CALI_FILE_PATH = @"C:\MPI\LEDTester\ESD";

        private const int PRECHARGE_TIME_OUT = 2000;  // 2 seconds
		private const int MAX_ESD_SETTING_LENGTH = 20;
        private const double CAP_WAIT_DISCHARGE_TIME = 200.0d;

        //-------------------------------------------------------------------------------------------//
        // ESD 2K, 4K and 8K HBM / MM Spec
        //-------------------------------------------------------------------------------------------//
		private const int DAC_SCALE			= 5;
		private const int HBM_START_VOLT	= 0;
        private const int HBM_END_VOLT_2K   = 2000; 
        private const int HBM_END_VOLT_4K   = 4000;
        private const int HBM_END_VOLT_8K   = 8000;

        private const int MM_START_VOLT		= 50;
        private const int MM_END_VOLT_2K    = 400; 
        private const int MM_END_VOLT_4K    = 500; 
        private const int MM_END_VOLT_8K    = 600; 

        //-------------------------------------------------------------------------------------------//
        // PCI DIO Pin Assignment (2K / 4K / 8K)
        //-------------------------------------------------------------------------------------------//
        //------------------ 4KV --------------------------//
        private const int E4K_Y0_POSITIVE_LED   = 0;
        private const int E4K_Y1_NEGATIVE_LED   = 1;
        private const int E4K_Y2_POSITIVE       = 2;
        private const int E4K_Y3_NEGATIVE       = 3; 
        private const int E4K_Y4_CHARGE_ACTION  = 4;
        private const int E4K_Y5_MM_CAPACITOR   = 5; 
        private const int E4K_Y6_HBM_DISCHARGE  = 6;
        private const int E4K_Y7_MM_DISCHARGE   = 7; 
        private const int E4K_Y8_SAFTY          = 8;
        private const int E4K_Y9_TESTER         = 9;
        private const int E4K_Y10_WORK_LED      = 10;
        private const int E4K_Y11_ISOLATION     = 11;

        //------------------ 8KV --------------------------//
        private const int E8K_Y0_POSITIVE       = 0;
        private const int E8K_Y1_NEGATIVE       = 1;
        private const int E8K_Y2_POSITIVE       = 2;
        private const int E8K_Y3_NEGATIVE       = 3;

        private const int E8K_F1_Y4_CHARGE_HBM  = 4;  // FOR 8kv flow1
        private const int E8K_F2_Y4_CHARGE      = 4;  // For 8kv flow2
        private const int E8K_F3_Y4_CHARGE      = 4;  // For 8kv flow3

        private const int E8K_F1_Y5_CHARGE_MM         = 5;  // FOR 8kv flow1
        private const int E8K_F2_Y5_MM_CAP            = 5;  // For 8kv flow2
        private const int E8K_F3_Y5_MM_CAP_PATH       = 5;  // For 8kv flow3
        private const int E8K_F8_Y5_MM_DISCHARGE_PATH = 5;  // For 8kv flow8

        private const int E8K_F1_Y6_DISCHARGE_HBM    = 6;  // For 8KV flow1
        private const int E8K_F2_Y6_DISCHARGE_HBM_MM = 6;  // For 8KV flow2
        private const int E8K_F3_Y6_DISCHARGE_HBM    = 6;  // For 8KV flow3
        private const int E8K_Y6_DISCHARGE_HBM       = 6;  // For 8KV flow8

        private const int E8K_F1_Y7_DISCHARGE_MM     = 7;  // For 8KV flow1
        private const int E8K_F2_Y7_MM_PATH          = 7;  // For 8KV flow2
        private const int E8K_F3_Y7_DISCHARGE_MM     = 7;  // For 8KV flow3
        private const int E8K_Y7_DISCHARGE_MM        = 7;  // For 8KV flow8

        private const int E8K_Y8_SAFE                = 8;
        private const int E8K_Y9_TESTER              = 9;
        private const int E8K_Y10_WORK_LED           = 10;
        private const int E8K_Y11_ISOLATION          = 11;

        private const int E8K_Y12_TESTER             = 12;
        private const int E8K_Y12_TESTER_ISO         = 12; // For 8KV flow7,flow8

        private const int E8K_Y13_MM_CHARGE_PATH     = 13; // For 8KV flow8

        private const int E8K_F10_Y14_DISCHARGE_HBM_NEG = 14; // For 8KV flow10
        private const int E8K_F10_Y15_DISCHARGE_MM_NEG  = 15; // For 8KV flow10


        //-------------------------------------------------------------------------------------------//
        // ESD Cycle Time Setting - Sequence 01, FLOW 01
        //-------------------------------------------------------------------------------------------//
        private const int DELAY_RESET_RELAY_TIME = 4;   // ResetRelayToSafeStatusTime

        private const int ChargeTime = 12;              // ChargeTime;

        private const int DischargeHalfTime = 2;               	                 // original ChargeTime = 2;
        private const int SafeTime = 3;                                                 // original ChargeTime = 3;
        private const int WaitTime01_TesterToEsdRelayOpen = 2;     // original ChargeTime = 2;
        private const int WaitTime02_DischargeRelayOpen = 3;         // original ChargeTime = 3;
        private const int WaitTime03_ChargeRelayOpen = 2;             // original ChargeTime = 2;
        private const int WaitTime04_TesterToEseRelayClose = 2;    // original ChargeTime = 2;

        private const int ResetRelayToSafeStatusTime = 4;               // original ChargeTime = 4;

        private const int PolarityChangeTime = 2;                               // original ChargeTime = 2;

        private const int MiddleSafeTime = 3;                                       // original ChargeTime = 3;

        //----------- ESD Cycle Time Setting ---flow5 for zhongke test ------for ZapSequence 3( )-------------
        private const int Flow5step0 = 2;
        private const int Flow5step2 = 10;//original setting = 12;
        private const int Flow5step3 = 1;//original setting = 2;
        private const int Flow5step3_1 = 2;
        private const int Flow5step4 = 2;
        private const int Flow5step5 = 2;
        private const int Flow5step6 = 2;
        private const int Flow5step7 = 2;

        //----------- ESD Cycle Time Setting ---flow6-5 --------------for ZapSequence 4( )----------------------//

        //------------------ < A > -----------------------------------

        //private const double Flow6step0 = 1.5d;
        //private const double Flow6step2 = 10.0d;
        //private const double Flow6step3 = 1.0d;
        //private const double Flow6step3_1 = 1.5d;
        //private const double Flow6step4 = 1.5d;
        //private const double Flow6step5 = 0.0d;
        //private const double Flow6step6 = 1.5d;
        //private const double Flow6step7 = 1.5d;

        //------------------ < B > -----------------------------------

        //private const double Flow6step0 = 1.5d;
        //private const double Flow6step2 = 10.0d;
        //private const double Flow6step3 = 1.0d;
        //private const double Flow6step3_1 = 1.0d;
        //private const double Flow6step4 = 1.5d;
        //private const double Flow6step5 = 0.0d;
        //private const double Flow6step6 = 1.5d;
        //private const double Flow6step7 = 1.5d;

        //------------------ < C > -----------------------------------

        private const double Flow6step0 = 1.5d;
        private const double Flow6step2 = 9.5d;
        private const double Flow6step3 = 1.0d; //1.0d;
        private const double Flow6step3_1 = 1.5d;
        private const double Flow6step4 = 1.5d;
        private const double Flow6step5 = 0.0d;
        private const double Flow6step6 = 1.5d;//1.5
        private const double Flow6step7 = 1.5d;

        //------------------ < D > -----------------------------------

        //private const double Flow6step0 = 1.5d;
        //private const double Flow6step2 = 9.5d;
        //private const double Flow6step3 = 1.0d;
        //private const double Flow6step3_1 = 1.0d;
        //private const double Flow6step4 = 1.5d;
        //private const double Flow6step5 = 0.0d;
        //private const double Flow6step6 = 1.5d;
        //private const double Flow6step7 = 1.5d;

        //----------------------------------------------------------------------------------//

        //------------------ < E > -----------------------------------------------------------------------------------------------
        //20130815---------for 華燦----(ESD快速版)-------- for ZapSequence 5( )-------------
        //--------------charge-----------------
        private const double Flow7step0_L = 1d;    //1
        private const double Flow7step2 = 9.5d;     //9.5
        //Flow7Step2_1 套用至Flow8Step2_1需改用1d
        private const double Flow7step2_1 = 1d;     //1, // 20141031 : for Flow7 = 0.5d, for flow12 = 1d
        //--------------discharge-----------------
        private const double Flow7step0_H = 3d;//0.5
        private const double Flow7step3 = 1.0d;//1
        private const double Flow7step3_1 = 3d;//1.5 
        //===============================================
        // Flow7 Step 4 由1.5d 改成 3.0d
        // 解決ESD+THY測試時，電荷未放掉的問題
        //===============================================
        private const double Flow7step4 = 3.0d;//3
        private const double Flow7step4_2 = 3.0d;//3 
        private const double Flow7step5 = 0.0d;//0
        private const double Flow7step6 = 1.5d;//1.5
        private const double Flow7step7 = 1.5d;//1.5


        //------------------ < F > ---------------------------------- ------------------------------------------------------------
        //20141103---------for THY +ESD , 億力 華燦----(ESD快速版)-------- for ZapSequence 6( )-------------
        //--------------charge-----------------
        private const double Flow8step0_L = 1.0d; //沒用到..暫用flow7 step0_L
        private const double Flow8step2 = 9.5d;   //沒用到..暫用flow7 step2
        private const double Flow8step2_1 = 1d; //沒用到..暫用flow7 step2_1
        //--------------discharge-----------------
        private const double Flow8step0_H = 0.5d;//0.5   //reliability test = 1
        private const double Flow8step3 = 1d;//1
        private const double Flow8step3_1 = 2d; //2
        private const double Flow8step4 = 2d;//2
        private const double Flow8step5 = 1.5d;//1.5
        private const double Flow8step5_2 = 1.5d;//1.5 for test polar circuit discharge
        private const double Flow8step6 = 1.5d;//1.5       //reliability test = 2
        private const double Flow8step7 = 1.5d;//1.5 

        //20151110---------for THY +ESD , 億力 華燦----(ESD快速版)-------- for ZapSequence 12( )-------------
        //--------------charge-----------------
        private const double Flow12step0_L = 1.0d; //沒用到..暫用flow7 step0_L
        private const double Flow12step2 = 9.5d;   //沒用到..暫用flow7 step2
        private const double Flow12step2_1 = 1d; //沒用到..暫用flow7 step2_1
        //--------------discharge-----------------
        private const double Flow12step0_H = 1d;//0.5   //reliability test = 1
        private const double Flow12step3 = 1d;//1
        private const double Flow12step3_1 = 2d; //2
        private const double Flow12step4 = 7d;//2
        private const double Flow12step5 = 1.5d;//1.5
        private const double Flow12step5_2 = 1.5d;//1.5 for test polar circuit discharge
        private const double Flow12step6 = 2d;//1.5       //reliability test = 2
        private const double Flow12step7 = 1.5d;//1.5 


        //------------------ < G > ----------------------------------------------------------------------------------------------
        //20141103---------for ESD -----(ESD慢速版)------- for ZapSequence 7( )-------------
        private const double Flow9step0 = 2.5d;//2.5
        private const double Flow9step1 = 12d;//12
        private const double Flow9step2 = 2d;//2
        private const double Flow9step3 = 1d;//1
        private const double Flow9step4 = 3d;//3
        private const double Flow9step5 = 2d;//2
        private const double Flow9step6 = 2d;//2
        private const double Flow9step7 = 3d;//3
        private const double Flow9step8 = 3d;//3
        private const double Flow9step9 = 2.5d;//2.5

        //20151110---------for ESD ----(ESD慢速版)-------- for ZapSequence 13( )-------------
        private const double Flow13step0 = 2d;//1 //2
        private const double Flow13step0_2 = 1.5d;//1.5, for test
        private const double Flow13step1 = 9.5d;//9.5
        private const double Flow13step2 = 2d;//2
        private const double Flow13step3 = 1d;//1
        private const double Flow13step4 = 1d;//3
        private const double Flow13step5 = 2d;//2
        private const double Flow13step6 = 10d;//10.5
        private const double Flow13step7 = 1.5d;//1.5
        private const double Flow13step8 = 2.5d;//2.5
        private const double Flow13step9 = 1.5d;//1.5

        //----------- ESD Cycle Time Setting 8KV Flow11 ----------for ZapSequence_8KV_Flow11( ) -------
        private const int E8kvFlow11_Step0 = 2;
        private const int E8kvFlow11_Step1 = 2;
        private const int E8kvFlow11_Step2 = 10;
        private const int E8kvFlow11_Step3 = 2;
        private const int E8kvFlow11_Step3_2 = 2; //2
        private const int E8kvFlow11_Step4 = 5; //4 
        private const int E8kvFlow11_Step4_2 = 1;
        private const int E8kvFlow11_Step5 = 3;
        private const int E8kvFlow11_Step6 = 7; // 7;


        private const double HIGH_SPEED_ZAP_TIME = 7.5d;
        private const double HIGH_SPEED_CAP_CHARGE_TIME = 11.0d;
        private const double HIGH_SPEED_MAX_ZAP_DELAY_TIME = 15.0d;

		private const double InternalDischargeTime = 550d;
		private const double InternalDischargeRelayOFF = 3d;

		#endregion

		#region >>> private Property <<<
	
        private object _lockObj;

		private PCI7230Wrapper _ioCard;
        private PISODA2UWrapper _daCard;

        private bool _isThrdActivate;
        private bool _isThrdResetDAValue;
        private int _thrdPrechargeIndex;
        private Thread _thrdPrecharge;
        private static EventWaitHandle _prechargeStartEvent;
        private static EventWaitHandle _prechargeDoneEvent;

        private List<ESDSettingData> _paraSettingList;
        private ESDSettingData[] _rebuildSettingData;
		private ESDDevSetting _devConfig;
		private ESDHardwareInfo _hwConfig;
		private ESDGainTable _sysGainTable;

        private int[] _zapList;
        private double[] _calSingleVolt;
		private string _hwInfoFilePath;

		private int _minHBM;			// unit = V
		private int _maxHBM;			// unit = V
		private int _minMM;				// unit = V
		private int _maxMM;				// unit = V
		private int _minCount;			// unit = cnt
		private int _maxCount;			// unit = cnt
		private int _minInterval;		// unit = ms
		private int _maxInterval;		// unit = ms
		private int _minStep;			// unit = V

		private EDevErrorNumber _errorNum;

      private double _currentVolt;

		private bool _isWorkingBusy;

        private MPI.PerformanceTimer _pt;
        private MPI.PerformanceTimer _capWaitChargeTime;

      private double _estimateDelay;
        private double _highSpeedModeDelayTime;
        private double _preChargeDeltaV;
        private double _preChargeDeltaVDelayTime;
        private double _preChargeWaitTime;
		private double _InternalDischargeDeltaVolt;//Angus

      private int _pesdChannelCount;
      private double _psedExtraPrechargeTime;

		private Dictionary<int, int> _idxMapping;

		#endregion

		public ESDCtrlPCI()
        {
            this._lockObj = new object();

            this._rebuildSettingData = null;
			this._devConfig = null;
			this._errorNum = EDevErrorNumber.Device_NO_Error;
			this._isWorkingBusy = false;
            this._pt = new PerformanceTimer();
			this._hwConfig = new ESDHardwareInfo();
            this._paraSettingList = new List<ESDSettingData>();

            this._isThrdActivate = false;
            this._isThrdResetDAValue = false;

            _prechargeStartEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "PreChargeStart");
            _prechargeDoneEvent = new EventWaitHandle(true, EventResetMode.ManualReset, "PreChargeDone"); 

         this._currentVolt = 0.0d;

         this._zapList = new int[MAX_ESD_SETTING_LENGTH];
         this._calSingleVolt = new double[MAX_ESD_SETTING_LENGTH];

         this._estimateDelay = 0.0d;

            this._highSpeedModeDelayTime = 0.0d;

            this._preChargeDeltaV = 0.0d;

            this._preChargeDeltaVDelayTime = 0.0d;

            this._preChargeWaitTime = 0.0d;

			this._capWaitChargeTime = new PerformanceTimer();

			this._idxMapping = new Dictionary<int,int>();
        }

        public ESDCtrlPCI(ESDDevSetting config) : this()
        {
            this._devConfig = config;

            //this._highSpeedDelayTime = this._devConfig.HighSpeedDelayTime > HIGH_SPEED_MAX_ZAP_DELAY_TIME || this._devConfig.HighSpeedDelayTime < 0 ?
            //                                 HIGH_SPEED_MAX_ZAP_DELAY_TIME : (HIGH_SPEED_MAX_ZAP_DELAY_TIME - Math.Abs(this._devConfig.HighSpeedDelayTime));

            if (this._devConfig.HighSpeedDelayTime > HIGH_SPEED_MAX_ZAP_DELAY_TIME || this._devConfig.HighSpeedDelayTime < 0)
            {
                this._devConfig.HighSpeedDelayTime = HIGH_SPEED_MAX_ZAP_DELAY_TIME;
            }

            this._highSpeedModeDelayTime = this._devConfig.HighSpeedDelayTime;

            this._pesdChannelCount = config.ChannelAssignment.Count;

            this._psedExtraPrechargeTime = 0.0d;

            if (this._pesdChannelCount < 1)
            {
                this._pesdChannelCount = 1;
            }

            ////////////////////////////////////////////////////////////////////////////////////
            if (this._pesdChannelCount == 2) // PESD cap charge extra delay time
            {
				this._psedExtraPrechargeTime = 16.0d;//6.0d;
            }            
            else if (this._pesdChannelCount == 3)
            {
				this._psedExtraPrechargeTime = 23.0d;//13.0d;
            }
            else if (this._pesdChannelCount == 4)
            {
				this._psedExtraPrechargeTime = 30.0d;// 20.0d;
            }
            ////////////////////////////////////////////////////////////////////////////////////
        }

        #region >>> Public Property <<<

        public int SetLength
        {
            get 
            {
                if (this._rebuildSettingData == null)
                    return 0;
                else
                    return this._rebuildSettingData.Length;
            }
        }

        public int[] ZapList
        {
            get { return this._zapList; }
        }

        public double[] CalibratedZapList
        {
            get { return this._calSingleVolt; }
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
			get { return this._hwConfig.SerialNumber; }
		}

		public string SoftwareVersion 
		{
			get { return ESD_SOFT_VER; }
		}

        public string HardwareVersion
        {
			get { return this._hwConfig.HWVersion.ToString(); }
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
			//for QC Test Only
			get { return this._hwConfig; }
			set { lock (this._lockObj) { this._hwConfig = value; } }
		}
		
        #endregion

        #region >>> Private Method <<<

        private void ResetRelayToSafeStatus()
            {
            // Default要切回測試機迴路
			this._ioCard.BitOutput(E4K_Y9_TESTER, 1);

            //#region >>> ResetRelayToSafeStatus Cycle <<<
            ////------------------ResetRelayToSafeStatus-------------
            ////-----ResetRelayToSafeStatus Cycle Step 01------------------
            //this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
            //this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
            //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
            //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
            //this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
            //this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
            //this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
            //this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
            //this.DelayTime(ResetRelayToSafeStatusTime);  //ms

            ////-----ResetRelayToSafeStatus Cycle Step 02------------------
            //this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
            //this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
            //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
            //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
            //this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
            //this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
            //this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
            //this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
            //this.DelayTime(ResetRelayToSafeStatusTime);  //ms

            ////-----ResetRelayToSafeStatus Cycle Step 03------------------
            //this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);
            //this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
            //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
            //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
            //this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
            //this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
            //this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
            //this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
            //this.DelayTime(ResetRelayToSafeStatusTime);  //ms

            ////-----ResetRelayToSafeStatus Cycle Step 04------------------
            //this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
            //this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
            //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
            //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
            //this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
            //this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
            //this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
            //this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
            //this.DelayTime(ResetRelayToSafeStatusTime);  //ms

            ////-----ResetRelayToSafeStatus Cycle Step 05------------------
            //this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
            //this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
            //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
            //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
            //this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
            //this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
            //this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
            //this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
            //this.DelayTime(ResetRelayToSafeStatusTime);  //ms

            //#endregion
        }

        private void DelayTime(double delayTime, bool isThreadSleep = false)
        {
            if (delayTime > 0.0d)
            {
            if (delayTime >= 30.0d)
            {
                System.Threading.Thread.Sleep((int)delayTime);
            }
            else
            {
                this._pt.Start();
                do
                {
                        if (isThreadSleep)
                        {
                            System.Threading.Thread.Sleep(0);
                        }

                    if (this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond) >= delayTime)
                    {
                        this._pt.Stop();
                        this._pt.Reset();
                        return;
                    }
                    System.Threading.Thread.Sleep(0);
                } while (this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond) < delayTime);
                this._pt.Stop();
                this._pt.Reset();
            }
        }
        }

        private bool RebuildSettingData(ESDSettingData[] data)
                    {
            List<ESDSettingData> paraSettingList = new List<ESDSettingData>();
              
				this._idxMapping.Clear();

            for (int i = 0; i < data.Length; i++)
                    {
                if (data[i].IsEnable)
                        {
                        if (i == 0)
                        {
                        data[i].ChargeDelayTime = this._devConfig.EstimateDelayTime;
                        }
                        else
                        {
                        data[i].ChargeDelayTime = 0.0d;
								
								data[i].GainVolt = data[0].GainVolt;
								
								data[i].OffsetVolt = data[0].OffsetVolt;
                        }

						  this._idxMapping.Add(i, paraSettingList.Count);

                    paraSettingList.Add(data[i].Clone() as ESDSettingData);
                            }
                            }
                    
            this._rebuildSettingData = paraSettingList.ToArray();

            if (this._rebuildSettingData.Length == 0)
            {
                return true;
            }

            return true;
        }

        private int ChenckUpDownBound(int value, int lowValue, int highValue)
            {
            if (value <= lowValue)
                return lowValue;

            if (value > highValue)
                return highValue;

            return value;
        }

        private bool CalESDVolt(int index)
        {
            double outVolt = 0.0d;

            //----------------------------------------------------------------------------------------------------------------------------------
            // (1) By 儀器 Calibration
            //----------------------------------------------------------------------------------------------------------------------------------
			double devGain = 1.0d;
			double devOffset = 0.0d;
            
			if (this._hwConfig.CaliMode == EESDCalibrationMode.ByTable)
            {
				ESDGainData[] devTable = null;

                switch (this._rebuildSettingData[index].Mode)
                {
                    case EESDMode.HBM:
                        {
                            if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
                            {
								devTable = this._hwConfig.CaliAnode.HBM;
            }
            else
            {
								devTable = this._hwConfig.CaliCathode.HBM;
            }

                            break;
                        }
                    case EESDMode.MM:
            {
                            if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
                            {
								devTable = this._hwConfig.CaliAnode.MM;
                            }
                            else
                            {
								devTable = this._hwConfig.CaliCathode.MM;
                            }

                            break;
                        }
                }

                if (devTable == null)
                    return false;

				foreach (ESDGainData data in devTable)
                {
                    if (this._rebuildSettingData[index].ZapVoltage <= data.UpperBoundary && this._rebuildSettingData[index].ZapVoltage <= data.LowerBoundary)
                    {
						devGain = data.Gain;
                        break;
                    }
                }
            }
            else
            {
                switch (this._rebuildSettingData[index].Mode)
                {
                    case EESDMode.HBM:
                        {
							devGain = this._hwConfig.HBMCaliGain01;
							devOffset = this._hwConfig.HBMCaliOffSet01;
                            break;
                        }
                    case EESDMode.MM:
                        {
							devGain = this._hwConfig.MMCaliGain01;
							devOffset = this._hwConfig.MMCaliOffSet01;
                            break;
                        }
                }
            }

            outVolt = this._rebuildSettingData[index].ZapVoltage * devGain + devOffset;

            //----------------------------------------------------------------------------------------------------------------------------------
            // (2) By 系統補償
            //----------------------------------------------------------------------------------------------------------------------------------
			double sysGain = 1.0d;

			ESDGainData[] sysTable = null;

            switch (this._rebuildSettingData[index].Mode)
            {
                case EESDMode.HBM:
                    {
						sysTable = this._sysGainTable.HBM;
                        break;
                    }
                case EESDMode.MM:
                    {
						sysTable = this._sysGainTable.MM;
                        break;
                    }
            }

            if (sysTable == null)
                return false;

			foreach (ESDGainData data in sysTable)
            {
                if (this._rebuildSettingData[index].ZapVoltage <= data.UpperBoundary && this._rebuildSettingData[index].ZapVoltage <= data.LowerBoundary)
                {
					sysGain = data.Gain;
                    break;
            }
            }

			outVolt = outVolt * sysGain;

            //----------------------------------------------------------------------------------------------------------------------------------
            // (3) By 產品補償
            //----------------------------------------------------------------------------------------------------------------------------------
			outVolt = outVolt * this._rebuildSettingData[index].GainVolt + this._rebuildSettingData[index].OffsetVolt;

            //----------------------------------------------------------------------------------------------------------------------------------
            // (4) 檢查校正/補償後, 電壓值上下限
            //----------------------------------------------------------------------------------------------------------------------------------
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
				if (outVolt > this._hwConfig.HBMMaxVolt || outVolt < this._hwConfig.HBMMinVolt)
                {
                    this._currentVolt = this.ChenckUpDownBound((int)outVolt, (int)this._hwConfig.HBMMinVolt, (int)this._hwConfig.HBMMaxVolt);
                    return false;
                }
            }
            else
            {
				if (outVolt > this._hwConfig.MMMaxVolt || outVolt < this._hwConfig.MMMinVolt)
                {
                    this._currentVolt = this.ChenckUpDownBound((int)outVolt, (int)this._hwConfig.MMMinVolt, (int)this._hwConfig.MMMaxVolt);
                    return false;
                }
            }

            this._calSingleVolt[index] = outVolt;

            return true;
        }

		// 內部放電
		private void InternalDischarge()
        {
			#region >>> Internal Discharge Cycle <<<
			//-----Internal Discharge Cycle step00------------------
			this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
			this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
			this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
			this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);//MM Path
			this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
			this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
			this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
			this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
			this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
			this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
			this.DelayTime(InternalDischargeTime);  //ms

            //this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);

			//-----Internal Discharge Cycle step01------------------
			this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
			this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
			this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
			this.DelayTime(InternalDischargeRelayOFF);  //ms
			#endregion
        }

        #region >>> ESD H/W Config <<<

        private bool LoadSystemGainTable()
        {
			string esdSN = this._hwConfig.SerialNumber;

			string fileNameWithExt = "Sys" + esdSN + ".dat"; //"ESDSysParam.dat"

			string path = System.IO.Path.Combine(CALI_FILE_PATH, fileNameWithExt);

            this._sysGainTable = (ESDGainTable)ESDHardwareInfo.Deserialize<ESDGainTable>(path);

			if (this._sysGainTable == null)
            {
				this._sysGainTable = new ESDGainTable();

                Console.WriteLine("[ESDCtrl], LoadSystemGainTable(), Load Table Fail");

				return false;
            }

			return true;
        }

        private bool CheckHardwareConfig()
        {
			string SerialNumber = string.Empty;
			EESDMechineType esdMechineType = EESDMechineType.ESD_2000;
			EESDVersion esdVer = EESDVersion.Ver1;
			int esdNumber = 0;

			if (!this.GetSerialNumber(ref SerialNumber, ref esdMechineType, ref esdVer, ref esdNumber))
            {
                this._errorNum = EDevErrorNumber.EsdHWReadInfo_Fail;
                return false;
            }

			this._hwInfoFilePath = System.IO.Path.Combine(CALI_FILE_PATH, SerialNumber + ".dat");

			ESDHardwareInfo fileInfo = this.LoadHardwareInfo(this._hwInfoFilePath);

			if (fileInfo == null)
            {
                this._errorNum = EDevErrorNumber.EsdRead_CalibrationFile_Fail;
                return false;
            }

			if (SerialNumber != fileInfo.SerialNumber)
            {
                this._errorNum = EDevErrorNumber.EsdRead_CalibrationFile_SerialNumber_Fail;
                return false;
            }

			if (esdMechineType != fileInfo.HWMechineType)
            {
                this._errorNum = EDevErrorNumber.EsdRead_CalibrationFile_MechineType_Fail;
                return false;
        }

			if (esdVer != fileInfo.HWVersion)
        {                   
                this._errorNum = EDevErrorNumber.EsdRead_CalibrationFile_Ver_Fail;
                return false;
            }

			if (esdNumber != fileInfo.HWNumber)
            {
                this._errorNum = EDevErrorNumber.EsdRead_CalibrationFile_Number_Fail;
                return false;
            }

            this._hwConfig = fileInfo;

            return true;
        }

        private void SetConfigByMachineType()
        {
            string msg = string.Empty;
            this._preChargeDeltaV = 0.0d;
            this._preChargeDeltaVDelayTime = 0.0d;
            this._preChargeWaitTime = 0.0d;
            
			switch (this._hwConfig.HWMechineType)
            {
                case EESDMechineType.ESD_2000:
                    {
                        this._maxHBM = HBM_END_VOLT_2K;
                        this._minHBM = HBM_START_VOLT;
                        this._maxMM = MM_END_VOLT_2K;
                        this._minMM = MM_START_VOLT;
                    break;
                    }
                //----------------------------------------------------------
                case EESDMechineType.ESD_4000:
                    {
                        this._maxHBM = HBM_END_VOLT_4K;
                        this._minHBM = HBM_START_VOLT;
                        this._maxMM = MM_END_VOLT_4K;
                        this._minMM = MM_START_VOLT;

						if (this._hwConfig.HWVersion == EESDVersion.Ver4)  // ESD4000-CF
                    {
                            this._preChargeDeltaV = 3000.0d;
                            this._preChargeDeltaVDelayTime = 50.0d;

							this._InternalDischargeDeltaVolt = 800.0d; // Angus 

                            this._preChargeWaitTime = this._devConfig.PrechargeWaitTime;  // <-- RD Fun 開放設定
                    }

                        break;
                    }
                //----------------------------------------------------------
                case EESDMechineType.ESD_8000:
                    {
                        this._maxHBM = HBM_END_VOLT_8K;
                        this._minHBM = HBM_START_VOLT;
                        this._maxMM = MM_END_VOLT_8K;
                        this._minMM = MM_START_VOLT;

                        this._preChargeDeltaV = 6000.0d;
                        this._preChargeDeltaVDelayTime = 50.0d;
                        this._preChargeWaitTime = this._devConfig.PrechargeWaitTime;   // <-- RD Fun 開放設定

                        this._devConfig.IsHighSpeedMode = false;  // ESD 8KV 不支援 High Speed Mode (電容預充模式)

                        break;
                    }
            }
                    }

		private bool GetSerialNumber(ref string SerialNumber, ref EESDMechineType MechineType, ref EESDVersion Ver, ref int Number)
        {
            uint value = 0;

			if (!this._ioCard.BitInput(out value))
                return false;

            try
                    {
                //uint temp = (value & 6144) >> 11; //000 11 000 0000 0000
                //MechineType = (EESDMechineType)Enum.Parse(typeof(EESDMechineType), temp.ToString());
                //string MechineTypeFormat = String.Format("{0:D2}", int.Parse(Convert.ToString(temp, 2)));

                //temp = (value & 57344) >> 13; //111 00 000 0000 0000
                //Ver = (EESDVersion)Enum.Parse(typeof(EESDVersion), temp.ToString());

                //temp = (value & 2047);
                //Number = Convert.ToInt32(temp.ToString(), 10);
                //string esdNumberFormat = String.Format("{0:D11}", long.Parse(Convert.ToString(temp, 2)));

                //SerialNumber = "ESD" + MechineTypeFormat + esdNumberFormat;

                uint temp = (value & 0xF800) >> 11; //111 11 000 0000 0000 ---check VerFormat,MachineTypeFormat (if temp=0, ESD = PLC Version) 

                if (temp != 0)
                        {
                    temp = (value & 0x1800) >> 11; //0001 1000 0000 0000 ---check MechineType (2KV, 4KV or 8KV)

					MechineType = (EESDMechineType)Enum.Parse(typeof(EESDMechineType), temp.ToString());
					string MechineTypeFormat = "99";

                    if (temp == 0)
                        {
						MechineTypeFormat = "02";
                    }
                    else if (temp == 1)
                    {
						MechineTypeFormat = "04";
                    }
                    else if (temp == 2)
                    {
						MechineTypeFormat = "08";
                    }
                    else if (temp == 3)
                    {
						MechineTypeFormat = "99";
                    }

                    temp = (value & 0xE000) >> 13; //1110 0000 0000 0000
					Ver = (EESDVersion)Enum.Parse(typeof(EESDVersion), temp.ToString());
                    string VerFormat = temp.ToString("00");

                    temp = (value & 0x07FF);
					Number = Convert.ToInt32(temp.ToString(), 10);
                    string esdNumberFormat = temp.ToString("0000");

					SerialNumber = "ESD" + VerFormat + MechineTypeFormat + esdNumberFormat;

					this._hwConfig.SerialNumber = SerialNumber;
					this._hwConfig.HWMechineType = MechineType;
					this._hwConfig.HWVersion = Ver;
					this._hwConfig.HWNumber = Number;
                        }
                        else
                        {
					string MechineTypeFormat = "04";
					string VerFormat = "01";
					string esdNumberFormat = temp.ToString("0000");

					MechineType = EESDMechineType.ESD_4000;
					Ver = EESDVersion.Ver1;
					Number = 0;

					SerialNumber = "ESD" + VerFormat + MechineTypeFormat + esdNumberFormat;
					}

				return true;
            }
            catch
            {
                return false;
            }
        }

		private ESDHardwareInfo LoadHardwareInfo(string FileName)
				{
            return (ESDHardwareInfo)ESDHardwareInfo.Deserialize<ESDHardwareInfo>(FileName);
                    }

        private bool WriteHardwareInfo()
        {
            try
            {
				if (System.IO.File.Exists(this._hwInfoFilePath))
					{
                    ESDHardwareInfo.Serialize(this._hwInfoFilePath, this._hwConfig);
                }
            }
            catch
            {
                this._errorNum = EDevErrorNumber.EsdWriteHardwareInfoFile_Err;
                return false;
            }

            return true;
        }

        private void CapStartWaitTime()
        {
            this._capWaitChargeTime.Stop();

            this._capWaitChargeTime.Reset();

            this._capWaitChargeTime.Start();
        }

        private void CapStopWaitTime()
        {
            this._capWaitChargeTime.Stop();

            this._capWaitChargeTime.Reset();
        }

        private bool CapIsWaitOverTime()
        {
            bool rtn = false;

            //擊發前未充電
			rtn |= this._capWaitChargeTime.PeekTimeSpan(ETimeSpanUnit.MilliSecond) == 0;

            //開始充電後未超過200ms
            rtn |= this._capWaitChargeTime.PeekTimeSpan(ETimeSpanUnit.MilliSecond) >= CAP_WAIT_DISCHARGE_TIME;

            return rtn;
        }

        #endregion

        #region >>> ESD Sequence Flow <<<

        private void RunPreChargeRoutine()
        {
            while (true)
            {
                if (!this._isThrdActivate)
                    break;

                if (_prechargeStartEvent.WaitOne())
                    {
                    _prechargeStartEvent.Reset();

					double daOutValue = 0.0d;

                    if (!this._isThrdResetDAValue)
                        {
                        int currentIdx = 0;

                        if (this._thrdPrechargeIndex == 0)
						{
                            currentIdx = this._rebuildSettingData.Length - 1;
						}
                        else
						{
                            currentIdx = this._thrdPrechargeIndex - 1;
						}

						if (this._calSingleVolt[currentIdx] != this._calSingleVolt[this._thrdPrechargeIndex])
                            {
                     daOutValue = this._calSingleVolt[this._thrdPrechargeIndex] / this._hwConfig.DARatio;
							this._daCard.DAOutput(0, daOutValue);

							//---------------------------------------------------------------------------------------------------------------------
                     // Delay Time (1): 硬體升壓的時間，由ESD校正檔案而來。
							//---------------------------------------------------------------------------------------------------------------------
                     this.DelayTime(this._hwConfig.HVChargeDelayTime);

                            //---------------------------------------------------------------------------------------------------------------------
							// Delay Time (?): 從高電壓切換成低電壓時，電壓差異過大時，前置盒需內部放電
							// ESD 8KV 機型不執行此動作 !!
                            //---------------------------------------------------------------------------------------------------------------------
							//Angus 
                            // HBM->MM 做 InternalDischarge

                            if (this._rebuildSettingData[currentIdx].Mode == EESDMode.HBM && this._rebuildSettingData[_thrdPrechargeIndex].Mode == EESDMode.MM)
                            {
                     if ((this._calSingleVolt[currentIdx] - this._calSingleVolt[this._thrdPrechargeIndex]) > this._InternalDischargeDeltaVolt)
							{
								
								this.InternalDischarge();
								//Console.WriteLine("[ESDCtrlPCI] HighVolt to LowVolt Discharge");//Angus
							
                                }
							}

                            //---------------------------------------------------------------------------------------------------------------------
                            // Delay Time (2): 硬體高壓差下，升壓所需額外的時間
                            //---------------------------------------------------------------------------------------------------------------------
                     if (Math.Abs((this._calSingleVolt[currentIdx] - this._calSingleVolt[this._thrdPrechargeIndex])) > this._preChargeDeltaV)
                            {
                                this.DelayTime(_preChargeDeltaVDelayTime);
                            }
                            //---------------------------------------------------------------------------------------------------------------------
                            // Delay Time (3): 快速升壓模組，所放的等待時間，ESD4000-CF & ESD8000 有效 (Angus: Win Much Delay Time)
                            // 由外部速度控制檔案MACADRESS而來。
                            //---------------------------------------------------------------------------------------------------------------------
                            this.DelayTime(_preChargeWaitTime);
                            }
						else
						{
                     daOutValue = this._calSingleVolt[this._thrdPrechargeIndex] / this._hwConfig.DARatio;
							this._daCard.DAOutput(0, daOutValue);
						}

                        //----------------------------------------------------------------------------------------
                        // ESD 快速版 (預先升壓完，做 電容預充)
                        //----------------------------------------------------------------------------------------
                        if (this._devConfig.IsHighSpeedMode)
                        {
                            // Sleep
                            // this.DelayTime(this._rebuildSettingData[this._thrdPrechargeIndex].ChargeDelayTime);
                            // charge hw cap

                            //System.Threading.Thread.Sleep(16);
                            this.DelayTime(16, true);

                            this.SetPreChargeHWCap(-1);
                            // Console.WriteLine("[ESDCtrlPCI] HighSpeedMode, Precharge Cap");
                        }
                    }   
                    else
                    {
						this._daCard.DAOutput(0, daOutValue);
            }
      
                    this._isThrdResetDAValue = false;
                    _prechargeDoneEvent.Set();
                }
            }
        }

		private bool SetPreChargeHV(int nowIndex)   
        {
            if (_prechargeDoneEvent.WaitOne(PRECHARGE_TIME_OUT))
            {
                _prechargeDoneEvent.Reset();

				if (nowIndex >= 0)
                {
                    if (this._errorNum != EDevErrorNumber.Device_NO_Error)
					{
                        this._isThrdResetDAValue = true;
					}
                    else
					{
                        this._isThrdResetDAValue = false;
					}

                    if (nowIndex == this._rebuildSettingData.Length - 1)
					{
                        this._thrdPrechargeIndex = 0;
					}
                    else
					{
                        this._thrdPrechargeIndex = nowIndex + 1;
                }
                }
                else
                {
                    this._isThrdResetDAValue = true;
                }

                _prechargeStartEvent.Set();
                return true;
            }
            else
            {
                this._errorNum = EDevErrorNumber.EsdPreChargeTimeOut_Err;
                return false;
            }
        }

		private void SetPreChargeHWCap(int nowIndex)
        {
            int zapIndex = 0;

			if (nowIndex < 0)
            {
				// -1 : AutoPpreChargeHWCap
                zapIndex = this._thrdPrechargeIndex;
            }
            else
            {
                // PreChargeHWCap by index 
                zapIndex = nowIndex;
            }

            // Precharge Hardward circuit Capacitance

            //-----------check : change To MM Path or not---------------
            if (this._rebuildSettingData[zapIndex].Mode == EESDMode.HBM)
            {
                //----- HBM Cycle initinal step ----- step 0_L ----------------------------------------------------
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM path
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);

				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1

				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
                this.DelayTime(Flow7step0_L);

                //-----HBM Cycle step02------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
				this.DelayTime(Flow7step2 + this._psedExtraPrechargeTime);  //ms   cap charge timr

                //-----HBM Cycle step02-1------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow7step2_1);  //ms


            }
            else // MM_Mode
            {
                //----- MM Cycle initinal step ----- step 0_L ----------------------------------------------------
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);//MM path
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);

				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);

				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
                this.DelayTime(Flow7step0_L);

                //-----MM Cycle step02------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
				this.DelayTime(Flow7step2 + this._psedExtraPrechargeTime);  //ms  cap charge time
            
                //-----MM Cycle step02-1------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow7step2_1);  //ms
            }

            this.CapStartWaitTime();
            }

        #endregion
            
        #region >>> ESD Zap Sequence Flow <<<

		private bool ZapSequence(int index)
        {
            //----- HBM MM  Cycle initinal step ----- step 00 ----------------------------------------------------
            //-----------check : change To MM Path or not---------------
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
            }
            else // MM_Mode
            {
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);
            }

			this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
			this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);

			this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
            this.DelayTime(PolarityChangeTime);

            //----------------------------------------------------------------------------------
			if (this._hwConfig.HWNumber == 0)//ESD Old HwVersion 
            {
                if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
                {
                    #region >>> HBM Cycle <<<
                    //------------------HBM Cycle----------------------------------
                    //-----HBM Cycle step01------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(WaitTime01_TesterToEsdRelayOpen);  //ms

                    //-----HBM Cycle step02------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(ChargeTime);  //ms

                    //-----HBM Cycle step03------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(WaitTime03_ChargeRelayOpen);  //ms

                    //-----HBM Cycle step04------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(DischargeHalfTime);  //ms

                    //-----HBM Cycle step05------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(SafeTime);  //ms

                    //-----HBM Cycle step06------------------         
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                    this.DelayTime(WaitTime02_DischargeRelayOpen);  //ms

                    //-----HBM Cycle step07------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                    this.DelayTime(WaitTime04_TesterToEseRelayClose);  //ms

                    #endregion
                }
                else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
                {
                    #region >>> MM Cycle <<<
                    //------------------MM Cycle----------------------------------
                    //-----MM Cycle step01------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(WaitTime01_TesterToEsdRelayOpen);  //ms

                    //-----MM Cycle step02------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(ChargeTime);  //ms

                    //-----MM Cycle step03------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(WaitTime03_ChargeRelayOpen);  //ms

                    //-----MM Cycle step04------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(DischargeHalfTime);  //ms

                    //-----MM Cycle step05------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(SafeTime);  //ms

                    //-----MM Cycle step06------------------         
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                    this.DelayTime(WaitTime02_DischargeRelayOpen);  //ms

                    //-----MM Cycle step07------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                    this.DelayTime(WaitTime04_TesterToEseRelayClose);  //ms

                    #endregion
                }
                return true;
            }
            else//ESD New HwVersion
            {
                if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
                {
                    #region >>> HBM Cycle <<<
                    //------------------HBM Cycle----------------------------------
                    //-----HBM Cycle step01------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(WaitTime01_TesterToEsdRelayOpen);  //ms

                    //-----HBM Cycle step02------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(ChargeTime);  //ms

                    //-----HBM Cycle step03------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(WaitTime03_ChargeRelayOpen);  //ms

                    //-----HBM Cycle step03 -0 ------------------ 
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                    this.DelayTime(MiddleSafeTime);  //ms

                    //-----HBM Cycle step03 - 1   ------------------// change RL0~RL3 open time
                    if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
                    {
						this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
						this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
						this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
						this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                    }
                    else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                    {
						this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
						this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
						this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
						this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                    }
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                    this.DelayTime(PolarityChangeTime);  //ms

                    //-----HBM Cycle step04------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(DischargeHalfTime);  //ms

                    //-----HBM Cycle step05------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(SafeTime);  //ms

                    //-----HBM Cycle step06------------------         
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
					this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                    this.DelayTime(WaitTime02_DischargeRelayOpen);  //ms

                    //-----HBM Cycle step07------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                    this.DelayTime(WaitTime04_TesterToEseRelayClose);  //ms

                    #endregion
                }
                else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
                {
                    #region >>> MM Cycle <<<
                    //------------------MM Cycle----------------------------------
                    //-----MM Cycle step01------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(WaitTime01_TesterToEsdRelayOpen);  //ms

                    //-----MM Cycle step02------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(ChargeTime);  //ms

                    //-----MM Cycle step03------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(WaitTime03_ChargeRelayOpen);  //ms

                    //-----MM Cycle step03 -0 ------------------
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                    this.DelayTime(MiddleSafeTime);  //ms

                    //-----MM Cycle step03 - 1   ------------------// change RL0~RL3 open time
                    if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time
                    {
						this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
						this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
						this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
						this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                    }
                    else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                    {
						this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
						this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
						this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
						this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                    }
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                    this.DelayTime(PolarityChangeTime);  //ms

                    //-----MM Cycle step04------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(DischargeHalfTime);  //ms

                    //-----MM Cycle step05------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                    this.DelayTime(SafeTime);  //ms

                    //-----MM Cycle step06------------------         
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
					this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                    this.DelayTime(WaitTime02_DischargeRelayOpen);  //ms

                    //-----MM Cycle step07------------------
					this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
					this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
					this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
					this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
					this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
					this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                    this.DelayTime(WaitTime04_TesterToEseRelayClose);  //ms

                    #endregion
                }
                return true;
            }
        }  // 內含 電容充電

		private bool ZapSequence2(int index) //flow5 20130716 for 武漢華燦 test
        {
            //----- HBM MM  Cycle initinal step ----- step 00 ----------------------------------------------------
            //-----------check : change To MM Path or not---------------
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
            }
            else // MM_Mode
            {
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);
            }

			this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
			this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
            //this.DelayTime(PolarityChangeTime);

			this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
            this.DelayTime(Flow5step0);

            //----------------------------------------------------------------------------------
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //-----HBM Cycle step02------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step2);  //ms

                //-----HBM Cycle step03------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step3);  //ms

                //-----HBM Cycle step03 - 1   ------------------// change RL0~RL3 open time test
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
                this.DelayTime(Flow5step3_1);  //ms

                //-----HBM Cycle step04------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step4);  //ms

                //-----HBM Cycle step05------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step5);  //ms

                //-----HBM Cycle step06------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                this.DelayTime(Flow5step6);  //ms

                //-----HBM Cycle step07------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow5step7);  //ms

                #endregion
            }
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
            {
                #region >>> MM Cycle <<<
                //-----MM Cycle step02------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step2);  //ms

                //-----MM Cycle step03------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step3);  //ms

                //-----MM Cycle step03 - 1   ------------------// change RL0~RL3 open time test
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
                this.DelayTime(Flow5step3_1);  //ms

                //-----MM Cycle step04------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step4);  //ms

                //-----MM Cycle step05------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step5);  //ms

                //-----MM Cycle step06------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                this.DelayTime(Flow5step6);  //ms

                //-----MM Cycle step07------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow5step7);  //ms

                #endregion
            }
            return true;

        }

		private bool ZapSequence3(int index) //flow5-2 20130716 for 武漢華燦 test
        {
            //----- HBM MM  Cycle initinal step ----- step 00 ----------------------------------------------------
            //-----------check : change To MM Path or not---------------
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
            }
            else // MM_Mode
            {
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);
            }

			this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
			this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
            //this.DelayTime(PolarityChangeTime);

			this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);

			this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
            this.DelayTime(Flow5step0);

            //----------------------------------------------------------------------------------
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //-----HBM Cycle step02------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step2);  //ms

                //-----HBM Cycle step03------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step3);  //ms

                //-----HBM Cycle step03 - 1   ------------------// change RL0~RL3 open time test
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow5step3_1);  //ms

                //-----HBM Cycle step04------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step4);  //ms

                //-----HBM Cycle step05------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step5);  //ms

                //-----HBM Cycle step06------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                this.DelayTime(Flow5step6);  //ms

                //-----HBM Cycle step07------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow5step7);  //ms

                #endregion
            }
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
            {
                #region >>> MM Cycle <<<
                //-----MM Cycle step02------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step2);  //ms

                //-----MM Cycle step03------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step3);  //ms

                //-----MM Cycle step03 - 1   ------------------// change RL0~RL3 open time test
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow5step3_1);  //ms

                //-----MM Cycle step04------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step4);  //ms

                //-----MM Cycle step05------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow5step5);  //ms

                //-----MM Cycle step06------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                this.DelayTime(Flow5step6);  //ms

                //-----MM Cycle step07------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow5step7);  //ms

                #endregion
            }
            return true;

        }

		private bool ZapSequence4(int index) //20130814 for 武漢華燦 flow6
        {
            //----- HBM MM  Cycle initinal step ----- step 00 ----------------------------------------------------
            //-----------check : change To MM Path or not---------------
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
            }
            else // MM_Mode
            {
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);
            }

			this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);

			this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
			this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
            //this.DelayTime(PolarityChangeTime);

			this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);

			this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
            this.DelayTime(Flow6step0);

            //----------------------------------------------------------------------------------
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //-----HBM Cycle step02------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);  // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow6step2);  //ms

                //-----HBM Cycle step03------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);   // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);  // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow6step3);  //ms

                //-----HBM Cycle step03 - 1   ------------------// change RL0~RL3 open time test
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow6step3_1);  //ms

                //-----HBM Cycle step04------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);   // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow6step4);  //ms

                //-----HBM Cycle step05------------------
				//this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				//this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				//this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				//this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				//this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                //this.DelayTime(Flow6step5);  //ms

                //-----HBM Cycle step06------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                this.DelayTime(Flow6step6);  //ms

                //-----HBM Cycle step07------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);  // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow6step7);  //ms

                #endregion
            }
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
            {
                #region >>> MM Cycle <<<
                //-----MM Cycle step02------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);   // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow6step2);  //ms

                //-----MM Cycle step03------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);   // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow6step3);  //ms

                //-----MM Cycle step03 - 1   ------------------// change RL0~RL3 open time test
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow6step3_1);  //ms

                //-----MM Cycle step04------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);    // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow6step4);  //ms

                //-----MM Cycle step05------------------
				//this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				//this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				//this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				//this._ioCard.BitOutput(E4K_Y11_ISOLATION, 1);
				//this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                //this.DelayTime(Flow6step5);  //ms

                //-----MM Cycle step06------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                this.DelayTime(Flow6step6);  //ms

                //-----MM Cycle step07------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1); // change 0 -> 1
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow6step7);  //ms

                #endregion
            }
            return true;

        }

		private bool ZapSequence5(int index) //20130815 for 武漢華燦 flow7 電容充電 => Thread
        {
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //-----HBM Cycle step0_H------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow7step0_H);  //ms

                //-----HBM Cycle step03------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);   // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);  // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow7step3);  //ms

                //-----HBM Cycle step03 - 1   ------------------// change RL0~RL3 open time test
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow7step3_1);  //ms

                //-----HBM Cycle step04------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);   // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow7step4);  //ms

                //-----HBM Cycle step06------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);//0
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);
                this.DelayTime(Flow7step6);  //ms

                //-----HBM Cycle step07------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);  // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow7step7);  //ms

                #endregion
            }
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
            {
                #region >>> MM Cycle <<<
        
                //-----MM Cycle step0_H------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow7step0_H);  //ms

                //-----MM Cycle step03------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);   // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);  // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow7step3);  //ms

                //-----MM Cycle step03 - 1   ------------------// change RL0~RL3 open time test
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow7step3_1);  //ms

                //-----MM Cycle step04------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);   // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow7step4);  //ms

                //-----MM Cycle step06------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);
                this.DelayTime(Flow7step6);  //ms

                //-----MM Cycle step07------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);  // change 1 -> 0
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow7step7);  //ms

                #endregion
            }
            return true;
        }

		
		//=========================================================================
        // 20141103,Angus
		// ESD+thy test, flow 8 ,快速ESD  for 億力 & 華燦
		//=========================================================================
		private bool ZapSequence6(int index)
            {
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //-----HBM Cycle step0_H----------------------------------
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow8step0_H);  //ms

                //-----HBM Cycle step03----------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1   
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
            }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
                this.DelayTime(Flow8step3);  //ms

                //-----HBM Cycle step03-1--------------------------------
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow8step3_1);  //ms

                //-----HBM Cycle step04----------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow8step4);  //ms

                //-----HBM Cycle step05----------------------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1  
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow8step5);  //ms

                //-----HBM Cycle step06----------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1  
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this.DelayTime(Flow8step6);  //ms

                //-----HBM Cycle step07----------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow8step7);  //ms

                #endregion
            }
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
            {
                #region >>> MM Cycle <<<
                //-----MM Cycle step0_H----------------------------------
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);//MM Path
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow8step0_H);  //ms

                //-----MM Cycle step03----------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
            }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
            {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
                this.DelayTime(Flow8step3);  //ms

                //-----MM Cycle step03-1--------------------------------
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow8step3_1);  //ms

                //-----MM Cycle step04----------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow8step4);  //ms

                //-----MM Cycle step05----------------------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow8step5);  //ms

                //-----MM Cycle step06----------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this.DelayTime(Flow8step6);  //ms

                //-----MM Cycle step07----------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow8step7);  //ms

                #endregion
            }
            return true;

            }

        //=========================================================================
        // 20151110,Angus test
        // change from ZapSequence 12(), flowchart = 12, 快速ESD  for 億力 & 華燦
        //=========================================================================
        private bool ZapSequence12(int index)
        {
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //-----HBM Cycle step0_H----------------------------------
                this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow12step0_H);  //ms

                //-----HBM Cycle step03----------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);   
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);         
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
                    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
                    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
                    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
                    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
                    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
                this.DelayTime(Flow12step3);  //ms

                //-----HBM Cycle step03-1--------------------------------
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow12step3_1);  //ms

                //-----HBM Cycle step04----------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);   
                this.DelayTime(Flow12step4);  //ms

                //-----HBM Cycle step05----------------------------------         
                //this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1); // test
                //this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);     // test
                //this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1); // test
                //this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);     // test
                //this.DelayTime(Flow8step5_2);  //ms

                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);  
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0); 
                this.DelayTime(Flow12step5);  //ms

                ////-----HBM Cycle step05-2 test 20150327------ use polar circuit to clean the electric charge   
                //this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1); // test
                //this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);     // test
                //this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1); // test
                //this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);     // test

                //this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                ////this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                //this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                //this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                //this.DelayTime(Flow12step5_2);  //ms

                //-----HBM Cycle step06----------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);  
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this.DelayTime(Flow12step6);  //ms

                //-----HBM Cycle step07----------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
                this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow12step7);  //ms

                #endregion
            }
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
            {
                #region >>> MM Cycle <<<
                //-----MM Cycle step0_H----------------------------------
                this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);//MM Path
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow12step0_H);  //ms

                //-----MM Cycle step03----------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                {
                    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
                    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
                    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
                    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
                    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
                this.DelayTime(Flow12step3);  //ms

                //-----MM Cycle step03-1--------------------------------
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this.DelayTime(Flow8step3_1);  //ms

                //-----MM Cycle step04----------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow12step4);  //ms

                //-----MM Cycle step05----------------------------------         
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow12step5);  //ms

                //-----MM Cycle step06----------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this.DelayTime(Flow12step6);  //ms

                //-----MM Cycle step07----------------------------------
                this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
                this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow12step7);  //ms

                #endregion
            }
            return true;

        }


        //=========================================================================
        // 20141103,Angus
        // ESD+thy test,  flow 9 ,慢速ESD  for 億力 & 華燦
        //=========================================================================
		private bool ZapSequence7(int index)
            {
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //------------------HBM Cycle----------------------------------
                //-----HBM Cycle step00-------------------------------------------
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//0
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow9step0);  //ms

                //-----HBM Cycle step01-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step1);  //ms

                //-----HBM Cycle step02-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step2);  //ms

                //-----HBM Cycle step03-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step3);  //ms

                //-----HBM Cycle step04 ------------------------------------------- 
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
            {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
            }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this.DelayTime(Flow9step4);  //ms

                //-----HBM Cycle step05-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step5);  //ms

                //-----HBM Cycle step06-------------------------------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step6);  //ms

                //-----HBM Cycle step07-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step7);  //ms

                //-----HBM Cycle step08-------------------------------------------
				//this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this.DelayTime(Flow9step8);  //ms

                //-----HBM Cycle step09-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				//this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow9step9);  //ms

                #endregion
		}
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
        {
                #region >>> MM Cycle <<<
                //------------------MM Cycle----------------------------------
                //-----MM Cycle step00-------------------------------------------
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);//MM Path
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow9step0);  //ms

                //-----MM Cycle step01-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step1);  //ms

                //-----MM Cycle step02-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step2);  //ms

                //-----MM Cycle step03-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step3);  //ms

                //-----MM Cycle step04 -------------------------------------------
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
			{
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
					this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
					this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
					this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this.DelayTime(Flow9step4);  //ms

                //-----MM Cycle step05-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step5);  //ms

                //-----MM Cycle step06-------------------------------------------         
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step6);  //ms

                //-----MM Cycle step07-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow9step7);  //ms

                //-----MM Cycle step08-------------------------------------------
				this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
				this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
				this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this.DelayTime(Flow9step8);  //ms

                //-----MM Cycle step09-------------------------------------------
				this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
				//this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
				this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
				this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow9step9);  //ms
				
                #endregion
            }
            return true;

        }  // 內含 電容充電

        //=========================================================================
        // 20151110,Angus test
        // change from ZapSequence 13() ,flowchart = 13, 慢速ESD
        //=========================================================================
        private bool ZapSequence13(int index)
        {
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //------------------HBM Cycle----------------------------------
                //-----HBM Cycle step00-------------------------------------------
                this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
                this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow13step0);  //ms

                ////-----HBM Cycle step0_2 for test-------------------------------------------------------
                ////----------------------polar circuit relay on before Cap charging----------------------
                //if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)//for test
                //{
                //    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
                //    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
                //    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                //    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                //}
                //else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)//for test
                //{
                //    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                //    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                //    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
                //    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                //}
                //this.DelayTime(Flow13step0_2);  //ms

                //-----HBM Cycle step01-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step1);  //ms

                //-----HBM Cycle step02-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step2);  //ms

                //-----HBM Cycle step03-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step3);  //ms

                //-----HBM Cycle step04 ------------------------------------------- 
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
                {
                    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
                    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
                    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
                    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
                    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this.DelayTime(Flow13step4);  //ms

                //-----HBM Cycle step05-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step5);  //ms

                //-----HBM Cycle step06-------------------------------------------         
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step6);  //ms

                //-----HBM Cycle step07-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step7);  //ms

                //-----HBM Cycle step08-------------------------------------------
                //this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this.DelayTime(Flow13step8);  //ms

                //-----HBM Cycle step09-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                //this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);//1
                this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
                this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow13step9);  //ms

                #endregion
            }
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
            {
                #region >>> MM Cycle <<<
                //------------------MM Cycle----------------------------------
                //-----MM Cycle step00-------------------------------------------
                this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 1);//MM Path
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this._ioCard.BitOutput(E4K_Y11_ISOLATION, 0);
                this._ioCard.BitOutput(E4K_Y10_WORK_LED, 1);
                this.DelayTime(Flow13step0);  //ms

                ////-----MM Cycle step0_2 for test---------------------------------------------------------
                ////----------------------polar circuit relay on before Cap charging----------------------
                //if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)//for test
                //{
                //    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
                //    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
                //    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                //    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                //}
                //else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)//for test
                //{
                //    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                //    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                //    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
                //    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                //}
                //this.DelayTime(Flow13step0_2);  //ms

                //-----MM Cycle step01-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 1);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step1);  //ms

                //-----MM Cycle step02-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step2);  //ms

                //-----MM Cycle step03-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step3);  //ms

                //-----MM Cycle step04 -------------------------------------------
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
                {
                    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 1);
                    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 1);
                    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                {
                    this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                    this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                    this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 1);
                    this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 1);
                }
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this.DelayTime(Flow13step4);  //ms

                //-----MM Cycle step05-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step5);  //ms

                //-----MM Cycle step06-------------------------------------------         
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 0);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step6);  //ms

                //-----MM Cycle step07-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 1);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this.DelayTime(Flow13step7);  //ms

                //-----MM Cycle step08-------------------------------------------
                this._ioCard.BitOutput(E4K_Y5_MM_CAPACITOR, 0);//MM Path
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 0);
                this._ioCard.BitOutput(E4K_Y0_POSITIVE_LED, 0);
                this._ioCard.BitOutput(E4K_Y2_POSITIVE, 0);
                this._ioCard.BitOutput(E4K_Y1_NEGATIVE_LED, 0);
                this._ioCard.BitOutput(E4K_Y3_NEGATIVE, 0);
                this.DelayTime(Flow13step8);  //ms

                //-----MM Cycle step09-------------------------------------------
                this._ioCard.BitOutput(E4K_Y4_CHARGE_ACTION, 0);
                //this._ioCard.BitOutput(E4K_Y6_HBM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y7_MM_DISCHARGE, 0);
                this._ioCard.BitOutput(E4K_Y8_SAFTY, 1);
                this._ioCard.BitOutput(E4K_Y9_TESTER, 1);
                this._ioCard.BitOutput(E4K_Y10_WORK_LED, 0);
                this.DelayTime(Flow13step9);  //ms

                #endregion
            }
            return true;

        }  // 內含 電容充電

		private bool ZapSequence_8KV_Flow11(int index)
                {
            if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
            {
                #region >>> HBM Cycle <<<
                //------------------HBM Cycle----------------------------------
                //----- HBM Cycle initinal step ----- step 0 -------------
				this._ioCard.BitOutput(E8K_Y0_POSITIVE, 0);
				this._ioCard.BitOutput(E8K_Y1_NEGATIVE, 0);
				this._ioCard.BitOutput(E8K_Y2_POSITIVE, 1);
				this._ioCard.BitOutput(E8K_Y3_NEGATIVE, 1);
				this._ioCard.BitOutput(E8K_F8_Y5_MM_DISCHARGE_PATH, 0);
				this._ioCard.BitOutput(E8K_Y8_SAFE, 1);
				this._ioCard.BitOutput(E8K_Y6_DISCHARGE_HBM, 0);
				this._ioCard.BitOutput(E8K_Y7_DISCHARGE_MM, 0);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E8K_Y10_WORK_LED, 1);
				this._ioCard.BitOutput(E8K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E8K_Y12_TESTER_ISO, 0);
				this._ioCard.BitOutput(E8K_Y13_MM_CHARGE_PATH, 0);
                this.DelayTime(E8kvFlow11_Step0);

                //-----HBM Cycle step01------------------
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
                    {
					this._ioCard.BitOutput(E8K_Y0_POSITIVE, 1);
					this._ioCard.BitOutput(E8K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E8K_Y1_NEGATIVE, 0);
					this._ioCard.BitOutput(E8K_Y3_NEGATIVE, 0);
					this._ioCard.BitOutput(E8K_F10_Y14_DISCHARGE_HBM_NEG, 1);
                    }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
                    {
					this._ioCard.BitOutput(E8K_Y0_POSITIVE, 0);
					this._ioCard.BitOutput(E8K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E8K_Y1_NEGATIVE, 1);
					this._ioCard.BitOutput(E8K_Y3_NEGATIVE, 1);
					this._ioCard.BitOutput(E8K_Y6_DISCHARGE_HBM, 1);
                    }
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step1);  //ms

                //-----HBM Cycle step02------------------
				this._ioCard.BitOutput(E8K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 1);
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) //construct HBM Pos. Path
                    {
					this._ioCard.BitOutput(E8K_F10_Y14_DISCHARGE_HBM_NEG, 1);
                    }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N) //construct HBM Neg. Path
                    {
					this._ioCard.BitOutput(E8K_Y6_DISCHARGE_HBM, 1);
                    }
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step2);  //ms //test

                //-----HBM Cycle step03------------------
				this._ioCard.BitOutput(E8K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 0);
				this._ioCard.BitOutput(E8K_Y8_SAFE, 0);//0, test
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step3);  //ms

                //-----HBM Cycle step03_2------------------
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) //construct HBM Pos. Path
                {
					this._ioCard.BitOutput(E8K_F10_Y14_DISCHARGE_HBM_NEG, 1);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N) //construct HBM Neg. Path
                {
					this._ioCard.BitOutput(E8K_Y6_DISCHARGE_HBM, 1);
                }
				this._ioCard.BitOutput(E8K_Y12_TESTER_ISO, 1);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E8K_Y8_SAFE, 0); //0, test
                this.DelayTime(E8kvFlow11_Step3_2);  //ms

                //-----HBM Cycle step04------------------
				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 0);
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
                {
					this._ioCard.BitOutput(E8K_Y6_DISCHARGE_HBM, 1);
			}
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
			{
					this._ioCard.BitOutput(E8K_F10_Y14_DISCHARGE_HBM_NEG, 1);
			}
				this._ioCard.BitOutput(E8K_Y8_SAFE, 0); //0, test
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step4);  //ms

                //-----HBM Cycle step04_2 ------------------
				this._ioCard.BitOutput(E8K_Y8_SAFE, 1);
				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 0);
				this._ioCard.BitOutput(E8K_Y6_DISCHARGE_HBM, 1);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step4_2);  //ms

                //-----HBM Cycle step05------------------
				this._ioCard.BitOutput(E8K_Y0_POSITIVE, 0);
				this._ioCard.BitOutput(E8K_Y1_NEGATIVE, 0);
				this._ioCard.BitOutput(E8K_Y2_POSITIVE, 1); // For Safe Test
				this._ioCard.BitOutput(E8K_Y3_NEGATIVE, 1); // For Safe Test

				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 0);
				this._ioCard.BitOutput(E8K_Y6_DISCHARGE_HBM, 0);
				this._ioCard.BitOutput(E8K_F10_Y14_DISCHARGE_HBM_NEG, 0);
				this._ioCard.BitOutput(E8K_Y8_SAFE, 1);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E8K_Y12_TESTER_ISO, 0);
                this.DelayTime(E8kvFlow11_Step5);  //ms

                //-----HBM Cycle step06------------------
				this._ioCard.BitOutput(E8K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E8K_Y10_WORK_LED, 0);
                this.DelayTime(E8kvFlow11_Step6);  //ms

                #endregion
        }
            else if (this._rebuildSettingData[index].Mode == EESDMode.MM)
            {
                #region >>> MM Cycle <<<
                //------------------MM Cycle----------------------------------
                //----- MM Cycle initinal step ----- step 0 -------------
				this._ioCard.BitOutput(E8K_Y0_POSITIVE, 0);
				this._ioCard.BitOutput(E8K_Y1_NEGATIVE, 0);
				this._ioCard.BitOutput(E8K_Y2_POSITIVE, 1);
				this._ioCard.BitOutput(E8K_Y3_NEGATIVE, 1);
				this._ioCard.BitOutput(E8K_F8_Y5_MM_DISCHARGE_PATH, 0);
				this._ioCard.BitOutput(E8K_Y8_SAFE, 1);
				this._ioCard.BitOutput(E8K_Y6_DISCHARGE_HBM, 0);
				this._ioCard.BitOutput(E8K_Y7_DISCHARGE_MM, 0);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E8K_Y12_TESTER_ISO, 0);
				this._ioCard.BitOutput(E8K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E8K_Y10_WORK_LED, 1);
				this._ioCard.BitOutput(E8K_Y13_MM_CHARGE_PATH, 0);
                this.DelayTime(E8kvFlow11_Step0);

                //-----MM Cycle step01------------------
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) // change RL0~RL3 open time test
		{
					this._ioCard.BitOutput(E8K_Y0_POSITIVE, 1);
					this._ioCard.BitOutput(E8K_Y2_POSITIVE, 1);
					this._ioCard.BitOutput(E8K_Y1_NEGATIVE, 0);
					this._ioCard.BitOutput(E8K_Y3_NEGATIVE, 0);
		}
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
        {
					this._ioCard.BitOutput(E8K_Y0_POSITIVE, 0);
					this._ioCard.BitOutput(E8K_Y2_POSITIVE, 0);
					this._ioCard.BitOutput(E8K_Y1_NEGATIVE, 1);
					this._ioCard.BitOutput(E8K_Y3_NEGATIVE, 1);
                }
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step1);  //ms

                //-----MM Cycle step02------------------
				this._ioCard.BitOutput(E8K_Y11_ISOLATION, 1);
				this._ioCard.BitOutput(E8K_Y13_MM_CHARGE_PATH, 1);
				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 1);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step2);  //ms //test

                //-----MM Cycle step03------------------
				this._ioCard.BitOutput(E8K_Y11_ISOLATION, 0);
				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 0);
				this._ioCard.BitOutput(E8K_Y13_MM_CHARGE_PATH, 0);
				this._ioCard.BitOutput(E8K_Y8_SAFE, 0);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step3);  //ms

                //-----MM Cycle step03_2------------------
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P) //construct HBM Pos. Path
            {
					this._ioCard.BitOutput(E8K_F10_Y15_DISCHARGE_MM_NEG, 1);
                }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N) //construct HBM Neg. Path
                {
					this._ioCard.BitOutput(E8K_Y7_DISCHARGE_MM, 1);
                }
				this._ioCard.BitOutput(E8K_F8_Y5_MM_DISCHARGE_PATH, 1);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step3_2);  //ms

                //-----MM Cycle step04------------------
				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 0);
                if (this._rebuildSettingData[index].Polarity == EESDPolarity.P)
                {
					this._ioCard.BitOutput(E8K_Y7_DISCHARGE_MM, 1);
            }
                else if (this._rebuildSettingData[index].Polarity == EESDPolarity.N)
            {
					this._ioCard.BitOutput(E8K_F10_Y15_DISCHARGE_MM_NEG, 1);
            }
				this._ioCard.BitOutput(E8K_Y7_DISCHARGE_MM, 1);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step4);  //ms

                //-----MM Cycle step04_2 ------------------
				this._ioCard.BitOutput(E8K_Y8_SAFE, 1);
				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 0);
				this._ioCard.BitOutput(E8K_Y7_DISCHARGE_MM, 1);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
                this.DelayTime(E8kvFlow11_Step4_2);  //ms

                //-----MM Cycle step05------------------
				this._ioCard.BitOutput(E8K_Y0_POSITIVE, 0);
				this._ioCard.BitOutput(E8K_Y1_NEGATIVE, 0);
				this._ioCard.BitOutput(E8K_Y2_POSITIVE, 1); // For Safe Test
				this._ioCard.BitOutput(E8K_Y3_NEGATIVE, 1); // For Safe Test

				this._ioCard.BitOutput(E8K_F3_Y4_CHARGE, 0);
				this._ioCard.BitOutput(E8K_Y7_DISCHARGE_MM, 0);
				this._ioCard.BitOutput(E8K_F10_Y15_DISCHARGE_MM_NEG, 0);
				this._ioCard.BitOutput(E8K_Y8_SAFE, 1);
				this._ioCard.BitOutput(E8K_Y9_TESTER, 0);
				this._ioCard.BitOutput(E8K_F8_Y5_MM_DISCHARGE_PATH, 0);
                this.DelayTime(E8kvFlow11_Step5);  //ms

                //-----MM Cycle step06------------------
				this._ioCard.BitOutput(E8K_Y9_TESTER, 1);
				this._ioCard.BitOutput(E8K_Y10_WORK_LED, 0);
                this.DelayTime(E8kvFlow11_Step6);  //ms

                #endregion
            }
            return true;
        }

        #endregion

        #endregion

        #region >>> Public Method <<<

        public bool Open()
        {
            if (this._ioCard == null)
            {
                //this._ioCard = new PCI1756Wrapper();
                this._ioCard = new PCI7230Wrapper();
            }

            if (this._daCard == null)
            {
                this._daCard = new PISODA2UWrapper();
            }

			if (!this._ioCard.Init())
            {
                this._errorNum = EDevErrorNumber.EsdHWInitFail_IOCard_Err;
                return false;
            }

            if (!this._daCard.Init())
            {
                this._errorNum = EDevErrorNumber.EsdHWInitFail_DACard_Err;
                return false;
            }

            //Check Calibration File with Hardware Info.
            if (this.CheckHardwareConfig())
            {
                this.LoadSystemGainTable();
            
                this.SetConfigByMachineType();

                this.ResetRelayToSafeStatus();

                // Create PrecCharge Thread
                if (!this._isThrdActivate)
                {                   
                    this._isThrdActivate = true;

                    this._thrdPrecharge = new Thread(RunPreChargeRoutine);
                    this._thrdPrecharge.Name = "EsdPreCharge";
                    //this._thrdPrecharge.Priority = ThreadPriority.AboveNormal;
                    this._thrdPrecharge.Start();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Close()
        {          
            if (this._isThrdActivate)
            {
                this.ResetRelayToSafeStatus();
                this._isThrdActivate = false;              
                this.SetPreChargeHV(-1);
                System.Threading.Thread.Sleep(20);
            }

            //try
            //{
            //    if (System.IO.File.Exists(this._hwInfoFilePath))
            //    {
            //        ESDHardwareInfo HWInfo = ESDHardwareInfo.Deserialize<ESDHardwareInfo>(this._hwInfoFilePath);

            //        HWInfo.HBMRelayCount = this._hwConfig.HBMRelayCount;
            //        HWInfo.MMRelayCount = this._hwConfig.MMRelayCount;

            //        ESDHardwareInfo.Serialize(this._hwInfoFilePath, HWInfo);
            //    }
            //}
            //catch
            //{
            //    this._errorNum = EDevErrorNumber.EsdWriteHardwareInfoFile_Err;
            //}

            if (this._ioCard != null)
            {
                this._ioCard.Close();
            }

            if (this._daCard != null)
            {
                this._daCard.Close();
            }
        }

        public bool SetConfigToMeter(ESDDevSetting cfg)
		{
			return true;
		}

        public bool SetParamToMeter(ESDSettingData[] setting)
        {
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this.ResetRelayToSafeStatus();
            this.SetPreChargeHV(-1);

            if (setting == null || setting.Length == 0)
            {
                this._rebuildSettingData = null;
                return true;
            }

            if (!this.RebuildSettingData(setting))
                return false;

			for ( int i = 0 ; i < this._rebuildSettingData.Length; i++ )
			{
                // ESD-8000 暫時不開放 Zap Count > 2
				if (this._hwConfig.HWMechineType == EESDMechineType.ESD_8000)
                {
                    if (this._rebuildSettingData[i].Count > 2)
                    {
                        this._errorNum = EDevErrorNumber.EsdSetValue_Err;
                        return false;
                    }
                }
                
                //---------------------------------------------------
				// MM mode, settin data check 
				//---------------------------------------------------
				if ( this._rebuildSettingData[i].Mode == EESDMode.MM )
				{
                    // ESD-8000 暫時不開放 MM Mode
					if (this._hwConfig.HWMechineType == EESDMechineType.ESD_8000)
                    {
                        this._errorNum = EDevErrorNumber.EsdSetValue_Err;
                        return false;
                    }
                    
                    if (this._rebuildSettingData[i].ZapVoltage > 0)
					{
                        if (this._rebuildSettingData[i].ZapVoltage > this._maxMM || this._rebuildSettingData[i].ZapVoltage < this._minMM)
						{
                            this._rebuildSettingData[i].ZapVoltage = this.ChenckUpDownBound(Math.Abs(this._rebuildSettingData[i].ZapVoltage), this._minMM, this._maxMM);
                            this._currentVolt = this._maxMM;
                            this._errorNum = EDevErrorNumber.EsdSetValue_Err;
							return false;
						}

                        if (!this.CalESDVolt(i))
                        {
                            this._errorNum = EDevErrorNumber.EsdParameterCalibrated_Err;
                            return false;
                        }
					}
					else
					{
                        this._rebuildSettingData[i].ZapVoltage = (-1) * this.ChenckUpDownBound(Math.Abs(this._rebuildSettingData[i].ZapVoltage), this._minMM, this._maxMM);
					}		
				}

				//---------------------------------------------------
				// HBM mode, settin data check 
				//---------------------------------------------------
				if ( this._rebuildSettingData[i].Mode == EESDMode.HBM )
				{
                    if (this._rebuildSettingData[i].ZapVoltage > 0)
					{
                        if (this._rebuildSettingData[i].ZapVoltage > this._maxHBM || this._rebuildSettingData[i].ZapVoltage < this._minHBM)
                        {
                            this._rebuildSettingData[i].ZapVoltage = this.ChenckUpDownBound(Math.Abs(this._rebuildSettingData[i].ZapVoltage), this._minHBM, this._maxHBM);
                            this._currentVolt = this._maxHBM;
                            this._errorNum = EDevErrorNumber.EsdSetValue_Err;
                            return false;
                        }

                        if (!this.CalESDVolt(i))
                        {
                            this._errorNum = EDevErrorNumber.EsdParameterCalibrated_Err;
                            return false;
                        }
					}
					else
					{
                        this._rebuildSettingData[i].ZapVoltage = (-1) * this.ChenckUpDownBound(Math.Abs(this._rebuildSettingData[i].ZapVoltage), this._minHBM, this._maxHBM);
					}
				}
			}

            this.SetPreChargeHV(this._rebuildSettingData.Length - 1);

			//_prechargeDoneEvent.WaitOne(PRECHARGE_TIME_OUT);

            return true;
        }

        public bool Zap(uint[] activeCH, int index)
		{
            bool rtn = true;

            if (this._rebuildSettingData == null || this._rebuildSettingData.Length == 0)
                return false;

            if (this._ioCard == null || this._daCard == null)
            {
                this._errorNum = EDevErrorNumber.EsdHWInitFail;
                return false;
            }

            if (this._errorNum != EDevErrorNumber.Device_NO_Error)
            {
                return false;
            }

			index = this._idxMapping[index];

            this._isWorkingBusy = true;

            if (_prechargeDoneEvent.WaitOne(PRECHARGE_TIME_OUT))
            {
            for (int i = 0; i < this._rebuildSettingData[index].Count; i++)
            {
                    this.DelayTime(this._rebuildSettingData[index].IntervalTime);
                          
                    if (!this._devConfig.IsHighSpeedMode)
                    {
						switch (this._hwConfig.HWMechineType)
                        {
                            case EESDMechineType.ESD_2000:
                            case EESDMechineType.ESD_4000:
                        //--------------------------------------------------------------------
                                // ESD 正常版 (只做 預先升壓)  for 4KV
                        //--------------------------------------------------------------------
                                //rtn &= this.ZapSequence(index); //flowchat = 4, ESD cycle time = 33 ms
                                //rtn &= this.ZapSequence7(index); //flowchat = 9
                                rtn &= this.ZapSequence13(index); //flowchat = 13, 20151110
								//flowchat = 9, ESD cycle time = 33 ms for 億力 華燦 20141103
                       // Console.WriteLine("[ESDCtrlPCI] Normal Mode, Zap");
                                break;
                            case EESDMechineType.ESD_8000:
								rtn &= ZapSequence_8KV_Flow11(index);
                                break;
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            if (this.CapIsWaitOverTime())
                            {
                                //Stanley
                                this.SetPreChargeHWCap(-1);
                            }

                        //--------------------------------------------------------------------
                        // 武漢華燦 目前使用的 ESD 快速版 (預先升壓，電容預充) ESD cycle time = 7.5 ms  for 4KV
                        //--------------------------------------------------------------------
                            //rtn &= this.ZapSequence5(index);  //flowchat = 7
                            //rtn &= this.ZapSequence6(index);  //flowchat = 8  for 億力 華燦 20141103
                            rtn &= this.ZapSequence12(index);  //flowchat = 12  for 億力 華燦 20151110
                       // Console.WriteLine("[ESDCtrlPCI] High Speed Mode, Zap");

                        //--------------------------------------------------------------------
                        // Angus 試做版，先留著 暫不使用。
                        //--------------------------------------------------------------------                        
                        //rtn &= this.ZapSequence2(index); //flowchat = 5-1
                        //rtn &= this.ZapSequence3(index); //flowchat = 5-2
                        //rtn &= this.ZapSequence4(index); //flowchat = 6  

                            
                            // Zap 第一道 使用設定之 Delay Time
                            this.DelayTime(this._highSpeedModeDelayTime);

                            this.CapStopWaitTime();
                        }
                        else
                        {
                            // 電容充電
							this.SetPreChargeHWCap(index);

                            //rtn &= this.ZapSequence5(index);  //flowchat = 7
                            //rtn &= this.ZapSequence6(index);  //flowchat = 8  for 億力 華燦 20141103
                            rtn &= this.ZapSequence12(index);  //flowchat = 12  for 億力 華燦 20151110
                            
                            // Zap 第二道 為了與第一道時間等長, 估算 Delay Time
                            
                            double calcSecondDelayTime = (HIGH_SPEED_ZAP_TIME + this._highSpeedModeDelayTime) - (HIGH_SPEED_ZAP_TIME + HIGH_SPEED_CAP_CHARGE_TIME);

                            if (calcSecondDelayTime > 0)
                            {
                                this.DelayTime(calcSecondDelayTime);
                            }
                        }
                    }

                    foreach (var channel in activeCH)
                    {
                    if (this._rebuildSettingData[index].Mode == EESDMode.MM)
                    {
                            switch (channel)
                            {
                                case 0:
						this._hwConfig.MMRelayCount++;
                                    break;
                                case 1:
                                    this._hwConfig.MMRelayCount2++;
                                    break;
                                case 2:
                                    this._hwConfig.MMRelayCount3++;
                                    break;
                                case 3:
                                    this._hwConfig.MMRelayCount4++;
                                    break;
                            }
                    }
                    else if (this._rebuildSettingData[index].Mode == EESDMode.HBM)
                    {
                            switch (channel)
                            {
                                case 0:
						this._hwConfig.HBMRelayCount++;
                                    break;
                                case 1:
                                    this._hwConfig.HBMRelayCount2++;
                                    break;
                                case 2:
                                    this._hwConfig.HBMRelayCount3++;
                                    break;
                                case 3:
                                    this._hwConfig.HBMRelayCount4++;
                                    break;
                            }
                        }
                    }
                }

                    this.SetPreChargeHV(index);
                }   
                else
                {
                    rtn = false;
                    this._isWorkingBusy = false;
                    this._errorNum = EDevErrorNumber.EsdPreChargeTimeOut_Err;
                }
            this._isWorkingBusy = false;

            return rtn;

            ////-------------- BitOutput(bit, A) 
            //// -------------- A = 0 => DO = Hi => relay open
            //// -------------- A = 1 => DO = Low => relay close
		}

        public bool ResetToSafeStatus()
        {
            //if (this._ioCard == null || this._daCard == null)
            //    return false;

            //this.ResetRelayToSafeStatus();
            this.WriteHardwareInfo();

            return true;
        }

      public bool PreCharge(int index)
        {
            bool rtn = true;

            if (this._errorNum != EDevErrorNumber.Device_NO_Error)
				return false;

            if (this._ioCard == null || this._daCard == null)
				return false;

            if (this._rebuildSettingData == null || this._rebuildSettingData.Length == 0 || index < 0)
				return false;

				index = this._idxMapping[index];

            if (index == 0)
            {
                this.SetPreChargeHV(this._rebuildSettingData.Length - 1);
            }
            else
            {
                this.SetPreChargeHV(index - 1);
            }


            //this.PreSetRelayPath(0);
            //this.ChangeDAValue(0);
            //this._isPrecharge = rtn;

            return rtn;
        }

        #endregion
    }

}
