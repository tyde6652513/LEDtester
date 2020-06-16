using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MPI.Tester.Data
{

    /// <summary>
    ///  The type of test item,
    ///  The declaration for item DO NOT has "_" char, 
    /// </summary>
    //[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ETestType : int
    {
        IF				= 0,
        IZ				= 1,
        VF				= 2,
        VR				= 3,
        IFH				= 4,
        DVF				= 5,
        LOP				= 6,
        LOPWL			= 7,
		IVSWEEP			= 8,
        VISWEEP			= 9,      
		THY				= 10,
        CALC			= 11,
        DIB             = 12, // disturb 
		POLAR			= 13,
		VAC				= 14,
        R               = 15,
        RTH             = 16,
		LIV             = 17,
        VLR             = 18,
        ESD             = 19,
        VISCAN          = 20,
        PIV             = 21,
        TRANSISTOR      = 22,
        OSA             = 23,
		LCR				= 24,
        ContactCheck    = 25,
        LCRSWEEP        = 26,//20170904 David
        IO              = 27,//20170904 David
		FFP             = 28,
        NFP             = 29,
        NFPBlinking     = 30,
        LaserSource     = 31,
    }
	
    public enum EGainOffsetType : int 
    { 
        None			= 0,
		Offset			= 1,
        Gain			= 2,
        GainAndOffest	= 3,
		Square			= 4,
    }
	 
    public enum ESweepType : int
    {
        None			= 0,
        Linear			= 1,
        Log				= 2,
        Custom			= 3,
    }
	
    public enum EPolarity : int
    {
        Anode_P			= 0,        // Positive
        Cathode_N       = 1,       // Negative
        Auto			= 2,
    }

    public enum ETesterCommMode : int
    {
        [XmlEnum(Name = "0")]
        BySoftware		= 0,

        [XmlEnum(Name = "1")]
        TCPIP			= 1,   

        [XmlEnum(Name = "2")]
        IO = 2,
    }

    public enum EOptiMsrtType : int
    {
		MVFLA			= 0,		// 0-base
		MVFLB			= 1,
        ConLOP          = 2,
		LOP				= 3,
		WATT			= 4,

		LM				= 5,
		WLP				= 6,
		WLD				= 7,
		WLC				= 8,
		WLCP			= 9,

		HW				= 10,		  
		DWDWP			= 11,
		PURITY			= 12,      
		CIEx			= 13,
		CIEy			= 14,
		CIEz			= 15,
		CCT				= 16,
		CRI				= 17,
		ST				= 18,
		INT				= 19,

		INTP			= 20,
		STR				= 21,
		INTSS			= 22,
		DARKA			= 23,
		DARKB			= 24,
		DARKC			= 25,

		R01				= 26,
		R02				= 27,
		R03				= 28,
		R04				= 29,
		R05				= 30,

		R06				= 31,
		R07				= 32,
		R08				= 33,
		R09				= 34,
		R10				= 35,

		R11				= 36,
		R12				= 37,
		R13				= 38,
		R14				= 39,
		R15				= 40,
        Uprime			= 41,
        Vprime			= 42,

        MFILA           = 43,
        MFILB           = 44,
        EWATT           = 45,
        LE              = 46,
        WPE             = 47,
        Duv             = 48,

		//AC Item
		ACMIFL			= 49,
		ACPOWERL	    = 50,
		ACAPPARENTL	    = 51,
		ACPFL			= 52,
		ACFREQUENCYL    = 53,
		ACPEAKL		    = 54,
		ACPEAKMAXL		= 55,

		//SDCM

		ANSISDCM	    = 56,
		ANSINEARCCT		= 57,
		ANSINEARSDCM	= 58,
		GBSDCM			= 59,
		GBNEARCCT		= 60,
		GBNEARSDCM		= 61,

        MVFLD           = 62,

        MFVLA           = 63,
        MIFLA           = 64,

        // PD Detector
        LOPWLPDCURRENT = 65,
        LOPWLPDCOUNT     = 66,

        WLCrms = 67,
        STDEV = 68,
        RMS = 69,
        D86 = 70,
    }

    public enum ECalMode : int
    {      
  		GainOffset		= 0,
		LookTable		= 1,
    }

    public enum ECalBaseWave : int
    {
		By_WLD			= 0,
        By_WLP			= 1,
        By_WLC			= 2,
    }

	public enum ELOPSaveItem : int
	{
		mcd			= 0,		// 1 item
		watt		= 1,		// 1 item
		lm			= 2,		// 1 item
		mcd_watt	= 3,		// 2 items
		mcd_lm		= 4,		// 2 items
		watt_lm		= 5,		// 2 items
		mcd_watt_lm	= 6,
	}
	
    public enum ECalLookupWave : int
    {
        Original		= 0,
        Corrected		= 1,
    }
		
    public enum ECoefTableItem : int    // 0-base
    { 
        Wave			= 0,
        WLP				= 1,
        WLD				= 2,
        WLC				= 3,
        LOP				= 4,
        WATT			= 5,
        LM				= 6,
        HW				= 7,
    }

    public enum ECalcType : int    // 0-base
    {
        Add				= 0,
        Subtract		= 1,
        Multiple		= 2,
        DivideBy		= 3,
        Min             = 4,
        Max             = 5,
        DeltaR          = 6,
    }

    public enum EControlTaskSetting : int    // 0-base
    {
        NONE			= 0,
        NEW				= 1,
        SAVEAS			= 2,
        UPDATE			= 3,
        DELETE			= 4,
		TSMC		    = 5,
        EPILED        	= 6,
    }

	public enum ETesterResultCreatFolderType : int
	{
        
		[XmlEnum(Name = "0")]
		None = 0,

		[XmlEnum(Name = "1")]
		ByLotNumber = 1,

		[XmlEnum(Name = "2")]
		ByMachineName = 2,

		[XmlEnum(Name = "3")]
		ByDataTime = 3,

        [XmlEnum(Name = "4")]
        ByBarcode = 4,

		[XmlEnum(Name = "5")]
        ByWaferID = 5,

        [XmlEnum(Name = "6")]
        ByLotNumber_WaferID = 6,

        [XmlEnum(Name = "7")]
        ByLot_WaferID_Times = 7,
	}

    public enum ERefTpye : int
    {
        [XmlEnum(Name = "0")]
        None = 0,

        [XmlEnum(Name = "1")]
        AsOutputPath1 = 1,

        [XmlEnum(Name = "2")]
        Specified_Path = 2,
	}

    public enum EMapGrowthDirection
    {
        Downward = 0,
        Upward = 1,
        Rightward = 2,
        Leftward = 3,
    }

    public enum EProbderMoveDirection
    {
        Xaxis = 0,
        Yaxis = 1,
    }

	public enum ECalibrationUIMode
	{
		T200	= 0,
		T100	= 1,
		Both	= 2,
	}

	public enum EStatisticType
	{
		All01 = 0,
		All02 = 1,
		All03 = 2,
		Single01 = 4,
		Single02 = 5,
		Single03 = 6,
		GoodRate01 = 7,
		GoodRate02 = 8,
		GoodRate03 = 9,
        System = 10,
	}

    public enum EEqModel
    {
        [XmlEnum(Name = "0")]
        Prober = 0,

        [XmlEnum(Name = "1")]
        Handler = 1,

        [XmlEnum(Name = "2")]
        Taping = 2,
    }

    public enum EActiveState
    {
        [XmlEnum(Name = "0")]
        ActiveHigh = 0,

        [XmlEnum(Name = "1")]
        ActiveLow = 1,
    }

	public enum ELIVOptiMsrtType : int
    {
		LIVSETVALUE = 0,

		LIVTIMEA = 1,		//Sampling Time
		LIVTIMEB = 2,		//Process Time

		LIVLOP = 3,
		LIVWATT = 4,

		LIVLM = 5,
		LIVWLP = 6,
		LIVWLD = 7,
		LIVWLC = 8,
		LIVWLCP = 9,

		LIVHW = 10,
		LIVDWDWP = 11,
		LIVPURITY = 12,
		LIVCIEx = 13,
		LIVCIEy = 14,
		LIVCIEz = 15,
		LIVCCT = 16,
		LIVCRI = 17,
		LIVST = 18,
		LIVINT = 19,

		LIVINTP = 20,
		LIVSTR = 21,
		LIVINTSS = 22,
		LIVDARKA = 23,
		LIVDARKB = 24,
		LIVDARKC = 25,

		LIVR01 = 26,
		LIVR02 = 27,
		LIVR03 = 28,
		LIVR04 = 29,
		LIVR05 = 30,

		LIVR06 = 31,
		LIVR07 = 32,
		LIVR08 = 33,
		LIVR09 = 34,
		LIVR10 = 35,

		LIVR11 = 36,
		LIVR12 = 37,
		LIVR13 = 38,
		LIVR14 = 39,
		LIVR15 = 40,
		LIVUprime = 41,
		LIVVprime = 42,

		LIVEWATT = 43,
		LIVLE = 44,
		LIVWPE = 45,
		LIVDuv = 46,		

		LIVWATTTD = 47,
		LIVLMTD = 48,

        LIVPDCURRENT = 49,
        LIVPDWATT = 50,
  
        LIVMsrtV = 51,
        LIVMsrtI = 52,
    }

	public enum ETransistorOptiMsrtType : int
	{
		TRTIME = 0,		//Process Time

		TRLOP = 1,
		TRWATT = 2,
		TRLM = 3,

		TRWLP = 4,
		TRWLD = 5,
		TRWLC = 6,
		TRWLCP = 7,

		TRHW = 8,
		TRDWDWP = 9,
		TRPURITY = 10,
		TRCIEx = 11,
		TRCIEy = 12,
		TRCIEz = 13,
		TRCCT = 14,
		TRCRI = 15,
		TRST = 16,
		TRINT = 17,

		TRINTP = 18,
		TRSTR = 19,
		TRINTSS = 20,
		TRDARKA = 21,
		TRDARKB = 22,
		TRDARKC = 23,

		TRR01 = 24,
		TRR02 = 25,
		TRR03 = 26,
		TRR04 = 27,
		TRR05 = 28,
		TRR06 = 29,
		TRR07 = 30,
		TRR08 = 31,
		TRR09 = 32,
		TRR10 = 33,
		TRR11 = 34,
		TRR12 = 35,
		TRR13 = 36,
		TRR14 = 37,
		TRR15 = 38,
		
		TRUprime = 39,
		TRVprime = 40,

		TRDrainEWATT = 41,
		TRLE = 42,
		TRWPE = 43,
		TRDuv = 44,

		TRWATTTD = 45,
		TRLMTD = 46,

		TRPDCURRENT = 47,
		TRPDWATT = 48,

		TRMsrtDrainV = 49,
		TRMsrtSourceV = 50,
		TRMsrtGateV = 51,
		TRMsrtBlukV = 52,

		TRMsrtDrainI = 53,
		TRMsrtSourceI = 54,
		TRMsrtGateI = 55,
		TRMsrtBlukI = 56,
	}

    public enum ELaserMsrtType : int
    {
        //-----------------------------------------------------
        // 操作功率下的量測值
        Pop         = 0,  // 0-base
        Iop         = 1,
        Vop         = 2,
        Imop        = 3,
        Pceop       = 4,    // power conversion efficiency
        //-----------------------------------------------------
        // Maximum I, P , ipd
        Ipk         = 5,
        Ppk         = 6,
        Vpk         = 7,
        Impk        = 8,
        Pcepk       = 9,
        //-----------------------------------------------------
        Pth         = 10,
        Ith         = 11,
        Vth         = 12,
        SE          = 13,
        SE2         = 14,
        RS          = 15,
        //-----------------------------------------------------
        Kink        = 16,
        Ikink       = 17,
        Pkink       = 18,
        //-----------------------------------------------------
        Linearity   = 19,
        Linearity2  = 20,
        Rollover    = 21,
        //-----------------------------------------------------
        Icod        = 22,
        Pcod        = 23,
        //-----------------------------------------------------
        // 指定電流下的量測值
        PfA         = 24,
        VfA         = 25,
        RdA         = 26,
        PceA        = 27,

        PfB         = 28,
        VfB         = 29,
        RdB         = 30,
        PceB        = 31,

        PfC         = 32,
        VfC         = 33,
        RdC         = 34,
        PceC        = 35,
        //-----------------------------------------------------
        RSquaredSE  = 36,
        RSquaredSE2 = 37,
        RSquaredRS  = 38,

        Iop2 =39,
        Vop2=40,
        V0   = 41,

    }

    public enum EOsaOptiMsrtType : int  // 0-base
    {
        OSAMsrtVs = 0,		//  msrt volt at start point
        OSAMsrtVe = 1,      //  msrt volt at end point
        OSAMeanWl = 2,
        OSAPeakWl = 3,
        OSAPeakLvl = 4,
        OSA2ndPeak = 5,
        OSA2ndPeakLvl = 6,
        OSAFWHMrms = 7,
        OSATotalPower = 8,
        OSASMSR = 9,
        OSADeltaLamda = 10,
		OSAStdev      = 11,
        OSARMS        = 12,
    }

    public enum EFfpMsrtType
    {
        FfpVolt = 0,
        FfpAngleAvg = 1,
        FfpAngleXMajor = 2,
        FfpAngleYMinor = 3,
        FfpAngleXMajor2 = 4,
        FfpAngleYMinor2 = 5,
        FfpDIP = 6,
        FfpCentroidX = 7,
        FfpCentroidY = 8,
        FfpD4SigmaDiameter = 9,
        FfpD4SigmaMajor = 10,
        FfpD4SigmaMinor = 11,
        FfpDsigmaPeak = 12,
        FfpDsigmaPeakMajor = 13,
        FfpDsigmamPeakMinor = 14, 
        FfpOrientation=15,
        FfpAngleAvgFWHM = 16,
        FfpAngleXMajorFWHM = 17,
        FfpAngleYMinorFWHM = 18,
        FfpAngleXMajor2FWHM = 19,
        FfpAngleYMinor2FWHM = 20,
        FfpUniformityMax = 21,
        FfpUniformityMin = 22,
        FfpUniA = 23,
        FfpUniB = 24,
        FfpUniC = 25,
        FfpUniD = 26,
        FfpUniE = 27,
        FfpUniF = 28,
        FfpUniG = 29,
        FfpUniH = 30,
        FfpUniI = 31,
        FfpRatialUniA = 32,
        FfpRatialUniB = 33,
        FfpRatialUniC = 34,
        FfpRatialUniD = 35,
        FfpRatialUniE = 36,
        FfpRatialUniF = 37,
        FfpRatialUniG = 38,
        FfpRatialUniH = 39,
        FfpRatialUniI = 40,
        FfpEyeSafetyRatio =41,
    }

    public enum ENfpMsrtType
    {
        NfpVolt = 0,
        DieCount=1,
        DeadDieCount = 2,
        BadDieCount= 3,
        BadDieCount2 = 4,
        
        IntensityMedian = 5,
        IntensityMax=6,
        IntensityMin=7,
        IntensityIQR = 8,
        RatioIntMinDivMedian = 9,
        RatioIntMaxDivMedian = 10,
        RatioIntIQRDivMedian = 11,
        RatioIntDeltaRangeDivMedian = 12,

        DiameterMajorMedian = 13,
        DiameterMajorMax= 14,
        DiameterMajorMin = 15,
        DiameterMajorIQR=16,
        RatioDiameterMajorIqrDivMedian = 17,
        DiameterMajorStdev=18,

        DiameterMinorMedian = 19,
        DiameterMinorMax = 20,
        DiameterMinorMin = 21,
        DiameterMinorIQR = 22,
        RatioDiameterMinorIqrDivMedian = 23,
        DiameterMinorStdev=24,

        PeakCountMedian = 25,
        PeakCountMax = 26,
        PeakCountMin = 27,
        PeakCountIQR = 28,
        RatioPeakCountIqrDivMedian = 29,
        PeakCountStdev=30,

        IntensityStdev = 31,
  
    }

    public enum ENfpBlinkingMsrtType
    {
        BkgVolt = 0,
        BkgDieCount = 1,
        BkgDeadDieCount = 2,
        BkgBadDieCount = 3,
        BkgBlinkingCount = 4,
    }

    public enum EAdjacentResult
    {
        NONE = 0,
        PASS = 1,
        RETEST = 2,
        ERROR = 3
    }

    public enum ESamplingMointorMode : int
    {
        ValueType = 0,
        GradientType = 1,
    }

	public enum ESysResultItem : int
	{
		TEST = 0,		// 0-base
		BIN = 1,
		POLAR = 2,
		CHANNEL = 3,
		CHUCKINDEX = 4,
		ISALLPASS = 5,
		ISFAIL = 6,
		ISALLPASS02 = 7,
		DIETESTSTATE = 8,
		SEQUENCETIME = 9,		// 0-base
        BINSN = 10,		// 0-base
		ISALLPASS03 = 11,
        IS_MOVE_DZ_AXIS = 12,
        DZ_MOVE_DISTANCE = 13,
        CHIP_INDEX = 14,
        TEST_START_TIME = 15,
        TEST_END_TIME = 16,
        TIME_SPAN = 17,
	}

	public enum EProberDataIndex : uint
	{
		ROW = 0,        //  0-base
		COL = 1,
		CHUCKX = 2,
		CHUCKY = 3,
		CHUCKZ = 4,

		ES01 = 5,
		ES02 = 6,
		ES03 = 7,
		ES04 = 8,
		PROBE_INDEX = 9,
		TransROW = 10,        //  
		TransCOL = 11,
		MLIResult = 12,
		TestChipGroup = 13,
        
        ReticleX = 14,
        ReticleY = 15,
        DutNumber = 16,
        DutOffset = 17,
        DZPosition = 18,
        PZPosition = 19,
		IsSingleProbingInMultiDie = 50, //還不確定定義，姑且先1=true 0=false

	}

    public enum EProberStrDataIndex : uint
    {
        ChannelHasDie = 0,
        ConditionGroup = 1,
        ProbeBin = 2,
        DutID = 3,
	}

    public enum EVLROptiMsrtType : int
    {
        VLRVA = 0,
        VLRVB = 1,
        VLRVC = 2,
        VLRVD = 3,

        VLRDVA = 4,
        VLRDVB = 5,
        VLRDVC = 6,

        VLRVAS = 7,
        VLRVBS = 8,
        VLRVCS = 9,
        VLRVDS = 10,

        VLRLOP = 11,
        VLRWATT = 12,

        VLRLM = 13,
        VLRWLP = 14,
        VLRWLD = 15,
        VLRWLC = 16,
        VLRWLCP = 17,

        VLRHW = 18,
        VLRDWDWP = 19,
        VLRPURITY = 20,
        VLRCIEx = 21,
        VLRCIEy = 22,
        VLRCIEz = 23,
        VLRCCT = 24,
        VLRCRI = 25,
        VLRST = 26,
        VLRINT = 27,

        VLRINTP = 28,
        VLRSTR = 29,
        VLRINTSS = 30,
        VLRDARKA = 31,
        VLRDARKB = 32,
        VLRDARKC = 33,

        VLRR01 = 34,
        VLRR02 = 35,
        VLRR03 = 36,
        VLRR04 = 37,
        VLRR05 = 38,

        VLRR06 = 39,
        VLRR07 = 40,
        VLRR08 = 41,
        VLRR09 = 42,
        VLRR10 = 43,

        VLRR11 = 44,
        VLRR12 = 45,
        VLRR13 = 46,
        VLRR14 = 47,
        VLRR15 = 48,
        VLRUprime = 49,
        VLRVprime = 50,

        VLRMFILA = 51,
        VLRMFILB = 52,
        VLREWATT = 53,
        VLRLE = 54,
        VLRWPE = 55,
        VLRDuv = 56,
    }

	public enum EANSI376 : int
	{
		ANSI_2700 = 2700,
		ANSI_3000 = 3000,
		ANSI_3500 = 3500,
		ANSI_4000 = 4000,
		ANSI_5000 = 5000,
		ANSI_6500 = 6500,
	}

	public enum EGB10682 : int
	{
		GB_2700 = 2700,
		GB_3000 = 3000,
		GB_3500 = 3500,
		GB_4000 = 4000,
		GB_5000 = 5000,
		GB_6500 = 6500,
	}

    public enum EPassRateCheckNGMode : int
    {
        STOP_TEST=0,
        
        STOP_ZAP_ESD=1,
    }

	public enum EBinningType : int
	{
		NONE = 0,
		IN_BIN = 1,
		NG_BIN = 2,
		SIDE_BIN = 3,
	}
    public enum EGroupBinRule : int
    {
        [XmlEnum(Name = "MAX")]
        MAX = 0,
        [XmlEnum(Name = "MIN")]
        MIN = 1,
        [XmlEnum(Name = "SAME")]
        SAME = 2,
    }
    public enum EBinBoundaryRule : int
    {
        [XmlEnum(Name = "0")]//≦ Item <"
        LeValL = 0,
        [XmlEnum(Name = "1")]//≦ Item ≦
        LeValLe = 1,
        [XmlEnum(Name = "2")]//< Item ≦
        LValLe = 2,
        [XmlEnum(Name = "3")]//< Item <
        LValL = 3,
        [XmlEnum(Name = "4")]//Various
        Various = 4,
    }
    public enum ECreateBinMode : int
    {
        ByBoundary = 0,
        ByBinCount = 1,
    }

	public enum EEqModelName
	{
		[XmlEnum(Name = "0")]
		LEDA_3GS = 0,

		[XmlEnum(Name = "1")]
		P7202 = 1,

		[XmlEnum(Name = "2")]
		P80C4_WaferFrame = 2,

		[XmlEnum(Name = "3")]
		P80C4_BinFrame = 3,

		[XmlEnum(Name = "4")]
        P80C8 = 4,

		[XmlEnum(Name = "5")]
		P6602 = 5,

		[XmlEnum(Name = "6")]
		P7602 = 6,
	}

    public enum ECoordinateRotation : int
    {
        None  = 0,
        CW90  = 1,
        CW180 = 2,
        CW270 = 3,
    }

    public enum EItemDescription : int
    {
        // SrcMeter (SMU 1)
        WaitTime    = 1,
        ForceValue  = 2,
        ForceTime   = 3,
        ForceRange  = 4,
        MsrtRange   = 5,
        MsrtClamp   = 6,
        FilterCount = 7,
        NPLC        = 8,

        SweepStart     = 10,
        SweepStep      = 11,
        SweepEnd       = 12,
        SweepRiseCount = 13,
        SweepFlatCount = 14,
        SweepTurnOffTime = 15,

        SGFilterCount       = 16,
        MovingAvgWindowSize = 17,

        IsPulseMode = 18,
		RTestItemRange = 19,

        // AC Power Supply
        ACFrequency = 20,

        // SptMeter
        IntTimeFix   = 30,
        IntTimeLimit = 31,
        SweepAdvanceMode= 32,

        // PD Detector (SMU 2)
        DetectorMsrtRange  = 40,
        DetectorBiasValue = 41,

        // ESD
        HBMVolt     = 50,
        MMVolt      = 51,
        ESDInterval = 52,
        ESDCount    = 53,

        // (LD) Laser Calc
        OperationMethod = 70,
        SeMethod        = 71,
        RsMethod        = 72,
        ThresholdMethod = 73,
        KinkMethod      = 74,

        // (Transistor) 4-Terminal

        // Special Setting
        EnableFloatForceValue = 101,  // (PD)
        FloatFactor = 102,
        FloatOffset = 103,

        // Pulse Mode
        PulseValue = 121,
        PulseWidth = 122,
        PulseDuty = 123,
        PulseMsrtRange = 124,

		//LCR
		LCR_IsProvideSignalLevelV = 150,
		LCR_IsProvideSignalLevelI = 151,
		LCR_IsProvideDCBiasV = 152,
		LCR_IsProvideDCBiasI = 153,
		LCR_SignalLevelV = 154,
		LCR_SignalLevelI = 155,
		LCR_Frequency = 156,
		LCR_DCBiasV = 157,
		LCR_DCBiasI = 158,
		LCR_MsrtSpeed = 159,
		LCR_TestType = 160,
        LCR_CaliDataQty = 161,
        LCR_CableLength = 162,
        LCR_CaliType = 163,

        //IO
        IO_Qty = 164,
        IO_State = 165,
        IO_Mode = 166,
        IO_Inverse = 167,

        //Calc
        CALC_GAIN = 168,
        CALC_ADV = 169,

        //LaserSource
        ATT_Attenuat_Range = 201,
        ATT_Power_Range = 202,
        ATT_AvgTime = 203,
        ATT_TransitionSpeed = 204,
        ATT_WavelengthRange = 205,

        //LCR2前面的將錯就錯，新增的LCR相關選項從300開始
        LCR_Cali_UI_Ver = 300,
        LCR_Enable_Open_Cali = 301,
        LCR_Enable_Short_Cali = 302,
        LCR_Enable_Load_Cali = 303,

    }

    public enum ELaserSearchMode : int
    {
        byCurrent = 0,
        byPower = 1,
    }

    public enum ELaserCalcMode : int
    {
        TwoPointsDifference = 0,
        LinearRegression = 1,
        ThresholdValue   = 2,
        Average          = 3,
        ThresholdValue2  = 4,
    }

    public enum ELaserPointSelectMode : int
    {
        ClosestPoint = 0,
        Interpolation = 1,
        OnFittingLine = 2,
    }


	public enum ELCRMsrtType : int
	{
		LCRCP = 0,			// Capacitance value measured with parallel-equivalent circuit model
		LCRCS = 1,			// Capacitance value measured with series-equivalent circuit model
		LCRLP = 2,			// Inductance value measured with parallel-equivalent circuit model
		LCRLS = 3,			// Inductance value measured with series-equivalent circuit model
		LCRD = 4,			// Dissipation factor
		LCRQ = 5,			// Quality factor (inverse of D)
		LCRG = 6,			// Equivalent parallel conductance measured with parallel-equivalent circuit model
		LCRRP = 7,			// Equivalent parallel resistance measured with parallel-equivalent circuit model
		LCRRS = 8,			// Equivalent series resistance measured with series-equivalent circuit model
		LCRRDC = 9,		// Direct-current resistance
		LCRR = 10,			// Resistance
		LCRX = 11,			// Reactance
		LCRZ = 12,			// Impedance
		LCRY = 13,			// Admittance
		LCRTD = 14,	// Phase angle of impedance/admittance (degree)
		LCRTR = 15,	// Phase angle of impedance/admittance (radian)
		LCRB = 16,			// Susceptance
		LCRVDC = 17,		// Direct-current oltagve
		LCRIDC = 18,		// Direct-current lectreicity
	}

    public enum ELCRSweepMsrtType : int
    {
        LCRSCP = 0,			// Capacitance value measured with parallel-equivalent circuit model
        LCRSCS = 1,			// Capacitance value measured with series-equivalent circuit model
        LCRSLP = 2,			// Inductance value measured with parallel-equivalent circuit model
        LCRSLS = 3,			// Inductance value measured with series-equivalent circuit model
        LCRSD = 4,			// Dissipation factor
        LCRSQ = 5,			// Quality factor (inverse of D)
        LCRSG = 6,			// Equivalent parallel conductance measured with parallel-equivalent circuit model
        LCRSRP = 7,			// Equivalent parallel resistance measured with parallel-equivalent circuit model
        LCRSRS = 8,			// Equivalent series resistance measured with series-equivalent circuit model
        LCRSRDC = 9,		// Direct-current resistance
        LCRSR = 10,			// Resistance
        LCRSX = 11,			// Reactance
        LCRSZ = 12,			// Impedance
        LCRSY = 13,			// Admittance
        LCRSTD = 14,	// Phase angle of impedance/admittance (degree)
        LCRSTR = 15,	// Phase angle of impedance/admittance (radian)
        LCRSB = 16,			// Susceptance
        LCRSVDC = 17,		// Direct-current oltagve
        LCRSIDC = 18,		// Direct-current lectreicity
    }

    public enum ELaserKinkCalcMode : int
    {
        SEk = 0,
        RefCurve = 1,
        FittingLine = 2,
        DeltaPow = 3,
        SecondOrder = 4,
    }

    public enum ETesterConfigType : uint
    {
        LEDTester = 0,
        LDTester = 1,
        PDTester = 2,
    }

	public enum ERISpecMode : int
	{
		STD = 0,
		Range = 1,
	}

	public enum ERIReCalcMode : int
	{
		Average = 0,
		Median = 1,
	}

    public enum ERILoadReportMode : int
    {
        None = 0,
        LoadOptiReport = 1,
        LoadElecReport = 2,
    }

    public enum ETestStage : int
    {
        LIV = 0,
        NFT = 1,
        FFT = 2,
    }


    public enum EFPOutputMode
    {
        ALL = 0,
        NG_DIE_ONLY = 1,
        ALL_AND_NG_DIE=2,
    }
    
    public enum ENfpUserJudgmentRule
    {
        SystemDefault = 0,
        Defined01 = 1,
        Defined02 = 2,
    }
}
