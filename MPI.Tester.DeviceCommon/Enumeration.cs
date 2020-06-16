using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MPI.Tester.DeviceCommon
{
    #region >>> I/O Enum <<<

    public enum EHWConnectType : int
    {
		[XmlEnum(Name = "0")]
		NONE			= 0,					// NO HW connector setting

		[XmlEnum(Name = "1")]
		RS232			= 1,					// RS232

		[XmlEnum(Name = "2")]
        GPIB			= 2,

		[XmlEnum(Name = "3")]		
        TCPIP			= 3,

		[XmlEnum(Name = "4")]
        USB				= 4,
    } 

    public enum EIOCardModel : int
    {
        [XmlEnum(Name = "0")]
        NONE = 0,

        [XmlEnum(Name = "1")]
        PCI1756 = 1,
    }

    public enum EIOPin : int
    {
        Pass = 2,
        Fail = 3,
        Anode_P = 4,
        Cathode_N = 5,
        EOT = 6,
        REOT = 7,
        BIN0 = 8,
        BIN1 = 9,
        BIN2 = 10,
        BIN3 = 11,
        BIN4 = 12,
        BIN5 = 13,
        BIN6 = 14,
        BIN7 = 15,
    }

    #endregion

	#region >>> SourceMeter Enum <<<

	public enum ESourceMeterModel : int
	{
		[XmlEnum(Name = "0")]
		NONE			= 0,				// NO this Device

		[XmlEnum(Name = "1")]
		LDT1A			= 1,				// LDT1A

		[XmlEnum(Name = "2")]
        K2400           = 2,				// Keithley 2400 series

		[XmlEnum(Name = "3")]
        T2001L			= 3,				// TSE T2001L

        [XmlEnum(Name = "4")]
        DR2000          = 4,				// DR2000 serial

		[XmlEnum(Name = "5")]
		N5700			= 5,				// Agilent N5700 series

		[XmlEnum(Name = "6")]
		DSPHD			= 6,				// DSPHD serial 

        [XmlEnum(Name = "7")]
        K2600          = 7,				    // Keithley 2611A 

		[XmlEnum(Name = "8")]
		IT7321			= 8,				// AC Power Supply

        [XmlEnum(Name = "9")]
        LDT3A200        = 9,				// LDT3A200 (Type-C)

        [XmlEnum(Name = "10")]
        K2520           = 10,			    // K2520 (LD SMU)

		[XmlEnum(Name = "11")]
		RM3542          = 11,				        

        [XmlEnum(Name = "12")]
        B2900A          = 12,	

        [XmlEnum(Name = "13")]
        SS400           = 13,

        [XmlEnum(Name = "14")]
        Persona = 14,				            // Stand (simulation SMU)
	}

    public enum ESMUTriggerMode : int
    {
        Single   = 1,
        Multiple = 2,
        TSP_Link = 3,
        Dual_HHC = 4,  // Serial Join
        Dual_HHV = 5,  // Parallel Join
        PMDT = 6,
    }

    public enum ESMU : int
    {
		[XmlEnum(Name = "None")]
        None = -1,

		[XmlEnum(Name = "SMU1")]
        SMU1 = 0,

		[XmlEnum(Name = "SMU2")]
        SMU2 = 1,

		[XmlEnum(Name = "SMU3")]
        SMU3 = 2,

		[XmlEnum(Name = "SMU4")]
        SMU4 = 3,
    }

	public enum EMsrtType : int
    {
        [XmlEnum(Name = "FV")]
        FV				= 0,				// force voltage

        [XmlEnum(Name = "FI")]
        FI				= 1,				// force current

        [XmlEnum(Name = "FVMI")]
        FVMI			= 2,				// force voltage and measure current

        [XmlEnum(Name = "FIMV")]
        FIMV			= 3,				// force current and measure voltage

		[XmlEnum(Name = "THY")]
		THY				= 4,				// measure resistance

        [XmlEnum(Name = "FIMVSWEEP")]
        FIMVSWEEP		= 5,				// force current and measure voltage sweep

        [XmlEnum(Name = "FVMISWEEP")]
        FVMISWEEP		= 6,				// force current and measure voltage sweep

        [XmlEnum(Name = "MV")]
        MV				= 7,				// measure voltage 

        [XmlEnum(Name = "MI")]
        MI				= 8,				// measure current

        [XmlEnum(Name = "R")]
        R	            = 9,			    // measure resistance

        [XmlEnum(Name = "POLAR")]
        POLAR           = 10,				// measure voltage

		[XmlEnum(Name = "RTH")]
		RTH = 11,   

        [XmlEnum(Name = "LIV")]
        LIV             = 12,				// measure LIV, Current Source

        [XmlEnum(Name = "LVI")]
        LVI             = 13,				// measure LIV, Voltage Source

        [XmlEnum(Name = "VLR")]
        VLR             = 14,				// measure VLR

        [XmlEnum(Name = "FIMVLOP")]
        FIMVLOP          = 15,				// measure LOP, Current Source

        [XmlEnum(Name = "FVMILOP")]
        FVMILOP          = 16,				// measure LOP, Voltage Source

        [XmlEnum(Name = "FVMISCAN")]
        FVMISCAN         = 17,				// Scan Current, Voltage Source

        [XmlEnum(Name = "PIV")]
        PIV             = 18,				// PIV (Laser Diode)

        [XmlEnum(Name = "TRANSISTOR")]
        TRANSISTOR      = 19,				          

        [XmlEnum(Name = "ROHM")]
        ROHM            = 20,

		[XmlEnum(Name = "LCR")]
		LCR				= 21,

		[XmlEnum(Name = "CONTACTCHECK")]
		CONTACTCHECK = 22,

        [XmlEnum(Name = "LCRSWEEP")]
        LCRSWEEP = 23,

        [XmlEnum(Name = "IO")]
        IO = 24,

        [XmlEnum(Name = "PDFVMI")]
        PDFVMI = 25,				// force voltage and measure current
    }

    public enum ESweepMode : int
    {
		[XmlEnum(Name = "0")]
        Linear			= 0,

		[XmlEnum(Name = "1")]
        Log				= 1,

		[XmlEnum(Name = "2")]
        Custom			= 2,
    }

    public enum ESourceFunc : int
    {
        [XmlEnum(Name = "0")]
        CW = 0,

        [XmlEnum(Name = "1")]
        PULSE = 1,

        [XmlEnum(Name = "2")]
        QCW   = 2,
    }

	public enum ETHYResultItem : int
	{
		MinPeak			= 0,
		MaxPeak			= 1,
		StableValue		= 2,
		OverShoot		= 3,
		MaxToStable		= 4,

		VoltConvexPeak	= 5,
		VoltConvexLine	= 6,
		VoltConvexDiff	= 7,
		ConvexIndex		= 8,
		LineSlop		= 9,
		LineIntercept	= 10,

		MTHYVDA			= 11,
		MTHYVDB			= 12,
	}

    public enum EVoltUnit : int
    {
        [XmlEnum(Name = "kV")]
        kV				= 3,

        [XmlEnum(Name = "V")]
        V				= 0,

        [XmlEnum(Name = "mV")]
        mV				= -3,

        [XmlEnum(Name = "uV")]
        uV				= -6,

        [XmlEnum(Name = "nV")]
        nV				= -9,

        [XmlEnum(Name = "pV")]
        pV				= -12,
    }

	public enum EAmpUnit : int
    {
        [XmlEnum(Name = "kA")]
        kA				= 3,

        [XmlEnum(Name = "A")]
        A				= 0,

        [XmlEnum(Name = "mA")]
        mA				= -3,

        [XmlEnum(Name = "uA")]
        uA				= -6,

        [XmlEnum(Name = "nA")]
        nA				= -9,

        [XmlEnum(Name = "pA")]
        pA				= -12,
    }
	
    public enum EWattUnit : int
    {
        [XmlEnum(Name = "W")]
        W = 0,

        [XmlEnum(Name = "mW")]
        mW = -3,

        [XmlEnum(Name = "uW")]
        uW = -6,

        [XmlEnum(Name = "nW")]
        nW = -9,

        [XmlEnum(Name = "pW")]
        pW = -12,
    }

    public enum ETimeUnit : int
    {
        [XmlEnum(Name = "s")]
        s				= 0,

        [XmlEnum(Name = "ms")]
        ms				= -3,

        [XmlEnum(Name = "us")]
        us				= -6,
    }

	public enum ECapUnit : int
	{
		[XmlEnum(Name = "kF")]
		kF = 3,

		[XmlEnum(Name = "F")]
		F = 0,

		[XmlEnum(Name = "mF")]
		mF = -3,

		[XmlEnum(Name = "uF")]
		uF = -6,

		[XmlEnum(Name = "nF")]
		nF = -9,

		[XmlEnum(Name = "pF")]
		pF = -12,

        [XmlEnum(Name = "fF")]
        fF = -15,
	}

	public enum EIndUnit : int
	{
		[XmlEnum(Name = "kH")]
		kH = 3,

		[XmlEnum(Name = "H")]
		H = 0,

		[XmlEnum(Name = "mH")]
		mH = -3,

		[XmlEnum(Name = "uH")]
		uH = -6,

		[XmlEnum(Name = "nH")]
		nH = -9,

		[XmlEnum(Name = "pH")]
		pH = -12,
	}

	public enum EOhmUnit : int
	{
		[XmlEnum(Name = "GOhm")]
		GOhm = 6,

		[XmlEnum(Name = "MOhm")]
		MOhm = 6,

		[XmlEnum(Name = "kOhm")]
		kOhm = 3,

		[XmlEnum(Name = "Ohm")]
		Ohm = 0,

		[XmlEnum(Name = "mOhm")]
		mOhm = -3,

		[XmlEnum(Name = "uOhm")]
		uOhm = -6,

		[XmlEnum(Name = "nOhm")]
		nOhm = -9,

		[XmlEnum(Name = "pOhm")]
		pOhm = -12,
	}

	public enum ESieUnit : int
	{
		[XmlEnum(Name = "GS")]
		GS = 6,

		[XmlEnum(Name = "MS")]
		MS = 6,

		[XmlEnum(Name = "kS")]
		kS = 3,

		[XmlEnum(Name = "S")]
		S = 0,

		[XmlEnum(Name = "mS")]
		mS = -3,

		[XmlEnum(Name = "uS")]
		uS = -6,

		[XmlEnum(Name = "nS")]
		nS = -9,

		[XmlEnum(Name = "pS")]
		pS = -12,
	}

	public enum EFreqUnit : int
	{
		[XmlEnum(Name = "Hz")]
		Hz = 0,

		[XmlEnum(Name = "KHz")]
		KHz = 3,

		[XmlEnum(Name = "MHz")]
		MHz = 6,
	}

	public enum EContactCheckSpeed : int
	{
		[XmlEnum(Name = "FAST")]
		FAST = 0,

		[XmlEnum(Name = "MEDIUM")]
		MEDIUM = 1,

		[XmlEnum(Name = "SLOW")]
		SLOW = 2,
	}

	public enum ERTestItemMsrtSpeed : int
	{
		[XmlEnum(Name = "FAST")]
		FAST = 0,

		[XmlEnum(Name = "MEDIUM")]
		MEDIUM = 1,

		[XmlEnum(Name = "SLOW")]
		SLOW = 2,
	}

	#endregion

	#region >>> Spectrometer Enum <<<

	public enum ESpectrometerModel : int
	{
		[XmlEnum(Name = "0")]
		NONE			= 0,			// NO this Device

		[XmlEnum(Name = "1")]
		RS_OP			= 1,			// Optimum

		[XmlEnum(Name = "2")]
		USB2000P		= 2,			// USB2000P

        [XmlEnum(Name = "3")]
        CAS140          = 3,			// ISCAS140

        [XmlEnum(Name = "4")]
        LE5400          = 4,			// Photal LE5400

        [XmlEnum(Name = "5")]
        HR2000P         = 5,			// HR2000

        [XmlEnum(Name = "6")]
        HR4000          = 6,			// HR4000
	}

    public enum ESpectrometerInterfaceType : int
    {
        InterfaceISA = 0,
        InterfacePCI = 1,
        InterfaceTest = 3,
        InterfaceUSB = 5,
        InterfaceNVISCluster = 7
    }

	public enum ESensingMode : int
	{
		Limit			= 0,
		Fixed			= 1,
		Limit02			= 2,
	}

	public enum ESpectrometerOpMode : int
	{
		Normal			= 0,
		Filter			= 1,
	}

	public enum ESptFilterMode : int
	{
		[XmlEnum(Name = "0")]
		Normal			= 0,

		[XmlEnum(Name = "1")]
		Filter			= 1,
	}

	public enum EDarkCorrectMode : int
	{ 
		[XmlEnum(Name = "0")]
		Normal			= 0,

		[XmlEnum(Name = "1")]
        High = 1,

        [XmlEnum(Name = "2")]
        Low = 2,
	}

    public enum EPDDarkCorrectMode : int
    {
        [XmlEnum(Name = "0")]
        None = 0,

        [XmlEnum(Name = "1")]
        Normal = 1,

        [XmlEnum(Name = "2")]
        High = 2,

        [XmlEnum(Name = "3")]
        Low = 3,
    }

	public enum ECIEObserver : int
	{
		Ob_1931			= 0,		// 2-degree
		Ob_1964			= 1,		// 10-degree
	}

    public enum ECCTCaculationType : int
    {
        McCamy = 0,		// 2-degree
        Robertson = 1,		// 10-degree
    }

	public enum ECIEilluminant : int
	{
		A				= 0,
		B				= 1,
		C				= 2,
		D50				= 3,
		D55				= 4,
		D65				= 5,
		D75				= 6,
		E				= 7			// x=0.3333 , y=0.3333
	}

    public enum ESpectrometerCalibDataMode
    {
        [XmlEnum(Name = "0")]
        IntegratingSphere=0,
        [XmlEnum(Name = "1")]
        McdModule=1,
    }

    public enum ESrcSensingMode : int
    {
        _4wire = 0,
        _2wire = 1,
    }

    public enum ESrcTurnOffType : int
    {
        EOT = 0,
        TestEnd = 1,
        EachTestItem = 2,
    }

	#endregion

    #region >>> ESD Enum <<<

    public enum EESDMechineType : int
	{
		[XmlEnum(Name = "0")]
		ESD_2000 = 0,

		[XmlEnum(Name = "1")]
		ESD_4000 = 1,

		[XmlEnum(Name = "2")]
		ESD_8000 = 2,

		[XmlEnum(Name = "3")]
        ESD_MechineType_ERR = 3,
	}

	public enum EESDVersion : int
	{
		[XmlEnum(Name = "0")]
		Ver0 = 0,

		[XmlEnum(Name = "1")]
		Ver1 = 1,

		[XmlEnum(Name = "2")]
		Ver2 = 2,

		[XmlEnum(Name = "3")]
		Ver3 = 3,

		[XmlEnum(Name = "4")]
		Ver4 = 4,

		[XmlEnum(Name = "5")]
		Ver5 = 5,

		[XmlEnum(Name = "6")]
		Ver6 = 6,

		[XmlEnum(Name = "7")]
		Ver7 = 7,
	}

	public enum EESDModel : int
	{
		[XmlEnum(Name = "0")]
		NONE			= 0,		// NO this Device

		[XmlEnum(Name = "1")]
		ESD_PLC			= 1,		// ESD2K

		[XmlEnum(Name = "2")]
		ESD_PCA			= 2,		// ESD4K
	}

    public enum EESDMode : int
    {
        [XmlEnum(Name = "0")]
        HBM		        = 0,

        [XmlEnum(Name = "1")]
        MM			    = 1,
    }

    public enum EESDPolarity : int
    {
        [XmlEnum(Name = "1")]
        P = 1,

        [XmlEnum(Name = "-1")]
        N = -1,
    }

    public enum EESDCalibrationMode : int
    {
        [XmlEnum(Name = "0")]
        ByGain          = 0,

        [XmlEnum(Name = "1")]
        ByTable         = 1,
    }

	public enum EESDCommand : int
	{
		None				= -1,
		OneZap				= 0,
		AutoRun				= 1,
		Stop				= 2,
		ModelHBM			= 3,
		ModelMM				= 4,
		ESDLoop				= 5,
		TesterLoop			= 6,
		PolarityPositive	= 7,
		PolarityNegative	= 8,
		SingleVoltOn		= 9,
		SingleVoltOff		= 10,
		ESDSet				= 11,

		HBMTable			= 12,
		MMTable				= 13,
		SingleZap			= 14,

		SweepOn				= 15,
		SweepOff			= 16,

		PrechargeOn			= 17,
		PrechargeOff		= 18,

		GetSN				= 19,		//for a while; timporality; by angus

		ZapON				= 20,
		ZapOff				= 21,
	}

	#endregion

	#region >>> Switch System Enum <<<

	public enum ESwitchSystemModel : int
	{
		[XmlEnum(Name = "0")]
		NONE          = 0,				// NO this Device

		[XmlEnum(Name = "1")]
		K3706A        = 1,

		[XmlEnum(Name = "2")]
		MFB           = 2,
	}

	#endregion

    public enum EDAQModel : int
    {
        [XmlEnum(Name = "0")]
        NONE = 0,				// NO this Device

        [XmlEnum(Name = "1")]
        DAQ9527 = 1,

        [XmlEnum(Name = "2")]
        PCI9222 = 2,

        [XmlEnum(Name = "3")]
        PCI9111HR = 3,

        [XmlEnum(Name = "4")]
        DAQ2213 = 4,

		[XmlEnum(Name = "5")]
		NI4461 = 5,

		[XmlEnum(Name = "6")]
		NI6280 = 6,

        [XmlEnum(Name = "7")]
        PCI1756 = 1,
    }


    //public enum EIOPin : int
    //{
    //    Pass        = 2,
    //    Fail        = 3,
    //    Anode_P     = 4,
    //    Cathode_N   = 5,
    //    EOT         = 6,
    //    REOT        = 7,
    //    BIN0        = 8,
    //    BIN1        = 9,
    //    BIN2        = 10,
    //    BIN3        = 11,
    //    BIN4        = 12,
    //    BIN5        = 13,
    //    BIN6        = 14,
    //    BIN7        = 15,
    //}

	public enum EDevErrorNumber : int
    {
        Device_NO_Error						= 0,

        //------------------------------------------------------------
        // SpectroMeter Device Error Number
		//------------------------------------------------------------
		SpectrometerDevice_Init_Err			= 201,
		SpectrometerSN_Err					= 202,
		SphereSN_Err						= 203,
		NoSpectrometerParamSettingData		= 204,
		OPRS_AutoCountSetting_Err			= 205,

		OPRS_Reconnect_Fail					= 206,
		OPRS_Calculation_Err				= 207,
		OPRS_Trigger_Err					= 208,
		OPRS_GetIntensity_Fail				= 209,
		OPRS_								= 210,

		GetWavelength_Fail					= 211,
		NoGetDarkArray						= 212,
		LoadCalibrationFileFail				= 213,
		SPAMDriver_Init_Err					= 214,
        Spectrometer_CalcPara_Err   	    = 215,
        Spectrometer_Measurement_Err        = 216,
        SpectrometerLoadWavelengthPara_Err  = 217,

		//------------------------------------------------------------
        // Source Meter Device Error Number
		//------------------------------------------------------------
		SourceMeterDevice_Init_Err			= 301,
		NoSourceMeterParamSettingData		= 302,
		SourceMeterIndexSetting_Err			= 303,
		CalcSweepForceValue_Err				= 304,
		PMUSetting_Err						= 305,

		ForceOutput_Ctrl_Err				= 306,
		SourceMeterDevice_HW_Err			= 307,
		NoMatchRangeIndex					= 308,
		ClampValueSetting_Err				= 309,
		CalcSweepForceRangeIndex_Err		= 310,
        SweepPointsSetting_Err              = 311,
        ParameterSetting_Err                = 312,
        DutyRate_Err                        = 313,
        ParameterLengthExcessBufferSize     = 314,
        MeterOutput_Ctrl_Err                = 315,
        OpenShortParameterSetting_Err       = 316,
        SourceMeterDevice_Disconnect_Err    = 317,
		SourceMeterAcquireDataTimeout_Err	= 319,
        SourceMeterNotDetectedSMUB_Err      = 320,
        MeterOutput_Interlock_Err           = 321,

        //------------------------------------------------------------
        // ESD Device Error Number
        //------------------------------------------------------------
        EsdHWInitFail                       		= 401,		
		EsdWriteHardwareInfoFile_Err				= 402,
        EsdHWReadInfo_Fail 							= 403,
        EsdRead_CalibrationFile_Fail 				= 404,
        EsdRead_CalibrationFile_SerialNumber_Fail 	= 405,
        EsdRead_CalibrationFile_MechineType_Fail 	= 406,
        EsdRead_CalibrationFile_Ver_Fail 			= 407,
        EsdRead_CalibrationFile_Number_Fail 		= 408,
        EsdSetValue_Err                             = 409,
        EsdSetParameterLength_Err                   = 410,
        EsdNoSettingParameter_Err                   = 411,
        EsdParameterCalibrated_Err                  = 412,
        EsdPreChargeTimeOut_Err                     = 413,
        EsdHWInitFail_IOCard_Err                    = 414,
        EsdHWInitFail_DACard_Err                    = 415,

        //------------------------------------------------------------
        // Switch System Device Error Number
        //------------------------------------------------------------
		SwitchHWInitFail                            = 501,
        SwitchNoCardInstall                         = 502,
        SwitchConfigDataMissing                     = 503,
        SwitchChannelSetting_Err                    = 504,

        //------------------------------------------------------------
        // DAQ Device Error Number
        //------------------------------------------------------------
        DAQDevice_Init_Err          = 601,
        DAQAcquireDataTimeout_Err   = 602,
		//------------------------------------------------------------
		// LCR Device Error Number
		//------------------------------------------------------------
		LCRInitFail = 701,
		LCRParameterSetting_Err = 702,
		LCRAcquireDataTimeout_Err = 703,

        //------------------------------------------------------------
        // OSA Device Error Number
        //------------------------------------------------------------
        OSA_Init_Err = 801,
        OSA_Connection_Failed_Err = 802,
        OSA_Parameter_Setting_Err = 805,
        OSA_Parameter_ExceedBufferSize_Err = 806,
        OSA_Trigger_Failed_Err = 810,
        OSA_Trigger_Timeout_Err = 811,
        OSA_Acquire_Results_Err = 815,
        OSA_Acquire_Spectrum_Err = 816,

        //------------------------------------------------------------
        // LASER Device Error Number
        //------------------------------------------------------------
        LASER_Source_Init_Err = 901,
        LASER_Attenuator_Init_Err = 902,
        LASER_Attenuator_ParaSet_Err = 903,
        LASER_Attenuator_Slot_Not_Exist_Err = 904,

        LASER_OpticalSwitch_Init_Err = 911,
        LASER_OpticalSwitch_Set_Err = 912,
        LASER_OpticalSwitch_Slot_Not_Exist_Err = 913,

        LASER_PowerMeter_Init_Err = 921,
        LASER_PowerMeter_Set_Err = 922,
        LASER_PowerMeter_Slot_Not_Exist_Err = 923,
        LASER_PowerMeter_CheckPower_Fail_Err = 924,

        LASER_AutoSetAttenuator_Fail_Err = 931,
    }

    public enum EPDSensingMode : int
    {
        [XmlEnum(Name = "0")]
        NONE          = 0,		   // NO Device

        [XmlEnum(Name = "1")]
        SrcMeter_SMUB  = 1,	  // Via SrcMeter SMU-B

        [XmlEnum(Name = "2")]
        SrcMeter_2nd   = 2,	  // Via 2nd SrcMeter

        [XmlEnum(Name = "3")]
        DAQ            = 3,	  // Via DAQ

        [XmlEnum(Name = "4")]
        DMM_7510            = 4,	  // Via DMM
    }

    public enum EDmmModel : int
    {
        [XmlEnum(Name = "0")]
        NONE = 0,				// NO this Device

        [XmlEnum(Name = "1")]
        K7510 = 1,              // Keithley DMM 7510

    }

    #region >>> LCR Enum <<<

    public enum ELCRDCBiasType : int
    {
        [XmlEnum(Name = "0")]
        Internal = 0,		   // NO Device

        [XmlEnum(Name = "1")]
        Ext_Master = 1,

        [XmlEnum(Name = "2")]
        Ext_Other = 2,

		[XmlEnum(Name = "3")]
		Master = 3,

		[XmlEnum(Name = "4")]
		Other = 4,
    }

	public enum ELCRDCBiasSource : int
	{
		[XmlEnum(Name = "0")]
		K2600 = 0,
		[XmlEnum(Name = "1")]
		N5700 = 1,

	}
    #endregion
    public enum ETerminalName : int
    {
        [XmlEnum(Name = "None")]
        None =  -1,
        
        [XmlEnum(Name = "Drain")]
        Drain  = 0,

		[XmlEnum(Name = "Source")]
		Source = 1,

		[XmlEnum(Name = "Gate")]
        Gate   = 2,

		[XmlEnum(Name = "Bluk")]
        Bluk   = 3,
    }

    public enum ETermianlFuncType : int
    {
        Bias = 1,
        Sweep = 2,
    }

	public enum ELCRTestType : int
	{
		[XmlEnum(Name = "0")]
		CPD = 0,

		[XmlEnum(Name = "1")]
		CPG = 1,

		[XmlEnum(Name = "2")]
		CPQ = 2,

		[XmlEnum(Name = "3")]
		CPRP = 3,

		[XmlEnum(Name = "4")]
		CSD = 4,

		[XmlEnum(Name = "5")]
		CSQ = 5,

		[XmlEnum(Name = "6")]
		CSRS = 6,

		[XmlEnum(Name = "7")]
		LPD = 7,

		[XmlEnum(Name = "8")]
		LPG = 8,

		[XmlEnum(Name = "9")]
		LPQ = 9,

		[XmlEnum(Name = "10")]
		LPRD = 10,

		[XmlEnum(Name = "11")]
		LPRP = 11,

		[XmlEnum(Name = "12")]
		LSD = 12,

		[XmlEnum(Name = "13")]
		LSQ = 13,

		[XmlEnum(Name = "14")]
		LSRD = 14,

		[XmlEnum(Name = "15")]
		LSRS = 15,

		[XmlEnum(Name = "16")]
		GB = 16,

		[XmlEnum(Name = "17")]
		RX = 17,

		[XmlEnum(Name = "18")]
		VDID = 18,

		[XmlEnum(Name = "19")]
		YTD = 19,

		[XmlEnum(Name = "20")]
		YTR = 20,

		[XmlEnum(Name = "21")]
		ZTD = 21,

		[XmlEnum(Name = "22")]
		ZTR = 22,
	}

	public enum ELCRModel : int
	{
		[XmlEnum(Name = "0")]
		NONE = 0,				// NO this Device

		[XmlEnum(Name = "1")]
		LCR4284A = 1,

		[XmlEnum(Name = "2")]
		E4980A = 2,

		[XmlEnum(Name = "3")]
		WK4100 = 3,

		[XmlEnum(Name = "4")]
		WK6500 = 4,

		[XmlEnum(Name = "5")]
		eWK6101 = 5,

		[XmlEnum(Name = "6")]
		IM3536 = 6,

        [XmlEnum(Name = "7")]
        _3506_10 = 7,

        [XmlEnum(Name = "8")]
        HP4278A = 8,
	}

	public enum ELCRMsrtSpeed : int
	{
		[XmlEnum(Name = "0")]
		Long = 0,

		[XmlEnum(Name = "1")]
		Medium = 1,

		[XmlEnum(Name = "2")]
		Short = 2,

		[XmlEnum(Name = "3")]
		Max = 3,
	}

	public enum ELCRSignalMode : int
	{
		[XmlEnum(Name = "0")]
		Voltage = 0,

		[XmlEnum(Name = "1")]
		Current = 1,
	}

    public enum ELCRCaliMode : int
    {
        [XmlEnum(Name = "1")]
        Open = 1,

        [XmlEnum(Name = "2")]
        Short = 2,

        [XmlEnum(Name = "3")]
        Load = 3,

        [XmlEnum(Name = "4")]
        Set = 4,

        [XmlEnum(Name = "5")]
        DataCollect = 5,
    }

    public enum ELCRDCBiasMode : int
	{
		[XmlEnum(Name = "0")]
		Voltage = 0,

		[XmlEnum(Name = "1")]
		Current = 1,
	}

    public enum EIOTrig_Mode : int
    {
        [XmlEnum(Name = "TRIG_BYPASS")]//O
        TRIG_BYPASS = 0,

        [XmlEnum(Name = "TRIG_FALLING")]//I
        TRIG_FALLING = 1,

        [XmlEnum(Name = "TRIG_RISING")]
        TRIG_RISING = 2,

        [XmlEnum(Name = "TRIG_EITHER")]
        TRIG_EITHER = 3,

        [XmlEnum(Name = "TRIG_SYNCHRONOUSA")]
        TRIG_SYNCHRONOUSA = 4,

        [XmlEnum(Name = "TRIG_SYNCHRONOUS")]
        TRIG_SYNCHRONOUS = 5,

        [XmlEnum(Name = "TRIG_SYNCHRONOUSM")]
        TRIG_SYNCHRONOUSM = 6,

        [XmlEnum(Name = "TRIG_RISINGA")]
        TRIG_RISINGA = 7,

        [XmlEnum(Name = "TRIG_RISINGM")]
        TRIG_RISINGM = 8,

        [XmlEnum(Name = "READ")]
        READ = 9,

    }

    public enum EIOState : int
    {
        [XmlEnum(Name = "LOW")]
        LOW = 0,
        [XmlEnum(Name = "HIGH")]
        HIGH = 1,
        [XmlEnum(Name = "NONE")]
        NONE = -1,
        [XmlEnum(Name = "ASSERT")]
        ASSERT = 2,
        [XmlEnum(Name = "WAIT")]
        WAIT = 3,
    }

    public enum EIOAct : int
    {
        [XmlEnum(Name = "NONE")]
        NONE = 0,
        [XmlEnum(Name = "LEVEL_HIGH")]
        LEVEL_HIGH = 1,
        [XmlEnum(Name = "HOLD_HIGH")]
        HOLD_HIGH = 2,
        [XmlEnum(Name = "RISING")]
        RISING = 3,

        [XmlEnum(Name = "LEVEL_LOW")]
        LEVEL_LOW = 4,
        [XmlEnum(Name = "HOLD_LOW")]
        HOLD_LOW = 5,
        [XmlEnum(Name = "FALLING")]
        FALLING = 6,

        
    }

    public enum ELaserSourceSysAction : int
    {
        [XmlEnum(Name = "NONE")]
        NONE = 0,
        LASER_SOURCE_MSRT = 1,
        LASER_SOURCE_SET = 2,
        ATTENUATOR_MSRT = 11,
        ATTENUATOR_SET = 12,
        //MoniterPD_MSRT = 21,
    }

    public enum ELaserSourceModel : int
    {
        [XmlEnum(Name = "0")]
        NONE = 0,		   // NO Device
    }

    public enum ELaserAttenuatorModel : int
    {
        [XmlEnum(Name = "0")]
        NONE = 0,		   // NO Device

        [XmlEnum(Name = "1")]
        N7760A = 1,

        [XmlEnum(Name = "11")]
        FTBx_3500 = 11,     //EXFO


        [XmlEnum(Name = "999")]
        SimuAtt = 999,
    }

    public enum EOpticalSwitchModel:int
    {
        [XmlEnum(Name = "0")]
        NONE = 0,		   // NO Device
        [XmlEnum(Name = "1")]
        OSW12 = 1,		   // Thor Lab

        [XmlEnum(Name = "3")]
        OSW1xN = 3,		   // Thor Lab
        //[XmlEnum(Name = "2")]
        //OSW22 = 2,		   // Thor Lab
        //[XmlEnum(Name = "3")]
        //OSW14 = 3,		   // Thor Lab
        //[XmlEnum(Name = "4")]
        //OSW18 = 4,         // Thor Lab


        [XmlEnum(Name = "11")]
        FTBx_9160 = 11,	   //EXFO

        [XmlEnum(Name = "999")]
        SimuOS = 999,
    }

    public enum EPowerMeter : int
    {
        [XmlEnum(Name = "0")]
        NONE = 0,		   // NO Device
        [XmlEnum(Name = "1")]
        K2600 = 1,		   //
        [XmlEnum(Name = "2")]
        FTBx_1750  = 2,
        [XmlEnum(Name = "3")]
        PM400 = 3,
        [XmlEnum(Name = "4")]
        PM101 = 4,
        [XmlEnum(Name = "999")]
        SimuPowerMeter = 999,
    }

}