using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.TestServer
{
    /// Define
    public enum EMPIDefine : int
    {
        Deactive				= 0,
        Active					= 1,

        Pass					= 0,
        Fail					= -1,
    }

    /// <summary>
    /// MPI test server command.
    /// </summary>
    public enum EMPITestServerCmd : int
    {
		TS_CMD_NONE				= -1,

		TS_CMD_TEST_START		= 1,
		TS_CMD_TEST_END			= 2,
		TS_CMD_TEST_ABORT		= 3,
		TS_CMD_SOT				= 4,
		TS_CMD_CALC				= 5,

		TS_CMD_SEND_BARCODE					= 6,
		TS_CMD_SEND_TEST_FILE_NAMES			= 7,
		TS_CMD_CHK_START					= 8,
		TS_CMD_GET_ITEM_NAME				= 9,
		TS_CMD_SHOW_TESTERUI				= 10,

		TS_CMD_REWRITE_TESTER_OUTPUT_FILE	= 11,
		TS_CMD_SHORT_TEST				    = 12,
		TS_CMD_OPEN_TEST				    = 13,
		TS_CMD_ABORT_OPEN_SHORT_TEST		= 14,
		TS_CMD_GET_BIN_RESULT				= 15,
		TS_CMD_PRINT_BARCODE				= 16,
		TS_CMD_REQUEST_RECIPE				= 17,
        TS_CMD_USER_LOGIN                   = 18,
        TS_CMD_USER_LOGOUT                  = 19,
        TS_CMD_APPEND_TESTER_OUTPUT_FILE    = 20,
        TS_CMD_RUN_DAILY_CHECK              = 21,

		TS_CMD_REQUEST_PRODUCTION_INFO      = 24,
        TS_CMD_CHECK_SPECTROMETER_STATE    = 25,

        TS_CMD_AUTO_CH_CALIB_START       =31,

        TS_CMD_AUTO_CH_CALIB_END        =32,

        TS_CMD_AUTO_CONTINUE_PROBING=33,

        TS_CMD_LOAD_CALIBRATION_DATA=101,
        TS_CMD_CHECK_RCONTACT_STATE_START =103,
        TS_CMD_CHECK_RCONTACT_STATE_END = 104,
    }

    public enum EMPITestServerErrCode : int
    {
        SUCCESS = 1,
        PB_ERROR_TESTER_IS_NOT_READY = 11,
        PB_ERROR_TESTER_GET_ITEM_NAME_ERROR = 12,
        PB_ERROR_SOT_ERROR = 13,
        PB_ERROR_EOT_ERROR = 14,
        PB_ERROR_OUTPUT_FILENAME_EXIST = 15,
        PB_ERROR_REWRITE_OUTPUT_FILENAME_FAIL = 16,
        PB_ERROR_LOAD_RECIPE_ERROR = 17,
        PB_ERROR_AUTO_LOAD_RECIPE_ERROR = 18,
        PB_ERROR_REMOTE_FILE_ERROR = 19,
        PB_ERROR_OUTPUT_FOLDER_IS_NOT_READY = 21,
        PB_ERROR_PRINT_BARCODE_ERROR = 22,
        PB_ERROR_FILTER_WHEEL_ERROR = 23,
        PB_ERROR_OUTPUT_FILENAME_EMPTY = 24,
        PB_ERROR_EOT_ADJACENT_ERROR = 25,
        PB_ERROR_EOT_TEST_DATA_ZERO = 26,
        PB_ERROR_NO_RUN_DAILY_CHECK = 27,
        PB_ERROR_NG_BIN_FRAME = 28,
        PB_ERROR_CONNECT_MESDB_ERROR = 29,
		PB_ERROR_CONTINUE_MODE = 30,
        PB_ERROR_EOT_FAILRATE_OUTSPEC =31,
        PB_ERROR_SPECTROMETER_ERROR =32,
        PB_ERROR_RCONTACT_RECIPE_ERROR = 33,
		PB_ERROR_INT_OVER_FLOW = 34,
        PB_ERROR_CH_CALIB_START_ERROR =35,
		PB_ERROR_AUTO_CONTINUES_PROBING = 37,
		PB_ERROR_APPEND_OUTPUT_FILENAME_FAIL = 38,
		PB_ERROR_OUTPUT_FILENAME_EXIST_FORMAT_ERR = 39,
        PB_ERROR_MULTIDIE_SETTING_NOT_READY=101,
    }

    public enum EMPIIOTestServerCmd : int
    {
        TS_CMD_NONE = -1,
        TS_CMD_TEST_START = 1,
        TS_CMD_TEST_END = 2,
        TS_CMD_TEST_ABORT = 3,
        TS_CMD_SOT = 4,
        TS_CMD_CALC = 5,
    }

    /// MPI test server command result.
    public enum EMPITestServerCmdResult : int
    {
        Success					= 0,
        Err_CommandUndefined,
    }
    
    /// MPI test server data array index.
    public enum EMPITestServerDataIndex : int
    {
        // System -> Test Server 
        Address_Col				= 0,			// Column(X) No. of channel 0
        Address_Row				= 1,			// Row(Y) No. of channel 0
        ProbeChannel			= 2,			// channel number	
        ChipStsCh0				= 3,			// channel 0 with chip or not
        ChipStsCh1				= 4,			// channel 1 with chip or not
        ChipStsCh2				= 5,			// channel 2 with chip or not

        ChipStsCh3				= 6,			// channel 3 with chip or not
        ChipStsCh4				= 7,			// channel 4 with chip or not
        ChipStsCh5				= 8,			// channel 5 with chip or not
        ChipStsCh6				= 9,			// channel 5 with chip or not
        ChipStsCh7				= 10,			// channel 5 with chip or not

        ChipStsCh8				= 11,			// channel 5 with chip or not
        ChipStsCh9				= 12,			// channel 5 with chip or not
		ChuckX					= 13,
		ChuckY					= 14,
		ChuckZ					= 15,

		ES01					= 16,
		ES02					= 17,
		ES03					= 18,
		ES04					= 19,

		OUT_TUBE_NUM			= 20,		
		OUT_TUBE_BIN_NUM		= 21,
		OUT_TUBE_COUNT			= 22,

        //-----------------------------------------------------------
        // Prober(System)  -> Tester Server for "TS_CMD_TEST_START" 
        //-----------------------------------------------------------
        CoordTransColX		= 23,          
        CoordTransRowY		= 24,       
 		TotalSacnCounts		= 25,
	    RowMinimum			= 26,
		RowMaximum			= 27,
		ColMinimum			= 28,
		ColMaximum			= 29,
		ChipXPictch			= 30,
		ChipYPictch			= 31,
		TotalProbingCounts	= 32,
        MoveMainAxis		= 33,
        SamplingMode        = 34,
        XInitDirection      = 35,
        YInitDirection      = 36,
        BinFrameMode     = 37,  // 0 : Disable , 1: 由測試機判斷NG BIN // 代表啟動P80C4抽測功能
        

        BinFrameSteeringAngle = 38, //  0 90,-90,180  BIN 轉向的角度
                                                        // 測試機和MESDB連線後，讀取Recipe後，告知P80C4轉向的角度 

        HighSpeedMode =39, // 0 : Disable,1: Enable High Speed Mode

        UserAuthorityLevel  = 40,    // User LOGIN
		AbortIsSaveFile     = 41,   // 0 : Save File  1 : Not Save File

		ProbingCount1		= 42,
		ProbingCount2		= 43,
		ProbingCount3		= 44,
		ProbingCount4		= 45,

        StartTemp           = 47,
        EndTemp             = 48,

		MLIResult			= 49,

        // Test Setver -> System 
        RESULT					= 50,
        REOT					= 51,
        EOT						= 52,
        PF0						= 53,
        PF1						= 54,
        PF2						= 55,
        PF3						= 56,
        PF4						= 57,
        PF5						= 58,
        PF6						= 59,

        PF7						= 60,
        PF8						= 61,
        PF9						= 62,
        UI_Display_ID			= 63,
		BIN_NUM					= 64,
        ReTestColX              = 65,
        ReTestRowY              = 66,

        OUT_TUBE_PULL_NUM       = 67,

		BoundarySampleTestMode	= 97,
		TestChipGroup			= 98,

        RefColumn				= 100,
        RefRow					= 101,

		ES01_START_COUNT = 111,
		ES02_START_COUNT = 112,
		ES03_START_COUNT = 113,
		ES04_START_COUNT = 114,
		ES05_START_COUNT = 115,
		ES06_START_COUNT = 116,
		ES07_START_COUNT = 117,
		ES08_START_COUNT = 118,

		ES01_END_COUNT = 119,
		ES02_END_COUNT = 120,
		ES03_END_COUNT = 121,
		ES04_END_COUNT = 122,
		ES05_END_COUNT = 123,
		ES06_END_COUNT = 124,
		ES07_END_COUNT = 125,
		ES08_END_COUNT = 126,

		IsPreSampling			= 127,

		SamplingDiePitchCol = 133,
		SamplingDiePitchRow = 134,


		XPitch1 = 137,
		YPitch1 = 138,
	    XPitch2 = 139,
		YPitch2 = 140,
		XPitch3 = 141,
		YPitch3 = 142,
		XPitch4 = 143,
		YPitch4 = 144,
		XPitch5 = 145,
		YPitch5 = 146,


        //PD200 Test Group
        IsSingleProbingInMultiDie = 158,    //還不確定定義，姑且先1=true 0=false
        //TestGroupIndex = 159,             //GroupTest
        //TestGroupStr = 160,               //GroupTest
        TestGroupStr = 160,                 //GroupTest
        TestTemperature = 176,              //Chuch Temperature

        //-----------------------------------------------------------
        // Prober(System)  -> Test Setver  for Multi-Die channel info
        //-----------------------------------------------------------
        ProberChannel_ColX       = 200,  // Roy, Multi-Die Testing
        ProberChannel_RowY      = 201,
        ProberChannel_Rotation  = 202,

		SubBin					= 650,	// 650~652, (3 X 8 = 24 Char) [SOT] Channel 上的Group, 以逗點區隔, ex: 1,1,2,2
		ProberBin				= 653,	// 653~655, (3 X 8 = 24 Char) [SOT] Channel 上的ProbeBin, 以逗點區隔, ex: 1,1,2,2
        XLineSubBinSampleCH     = 656,  // 2016.06.21 提供幾抽1的資訊
        YLineSubBinSampleCH     = 657,  // 2016.06.21 提供幾抽1的資訊

        //Roy, Multi-Die channel info
        ProberChannelStatus = 700,   // 700~715, (16 X 8 = 128 Char) [SOT] Channel 上有無Die 
        ChannelResultPF = 716,   // 716~732, (16 X 8 = 128 Char) [REOT] Chips pass/fail

		//-----------------------------------------------------------
		// Prober(System)  -> Test Setver  for file name
		//-----------------------------------------------------------
		// Prober Keyin Information and Machine Information
        
        CustomerKeyNumber		= 784,
        CassetteNumer           = 800,
        SoltNumber              = 816,
		ProductType				= 832,
		Substrate				= 848,
		LotNumber				= 864,
		CassetteID			    = 880,
		WaferNumber				= 896,
		OperatorName			= 912,
		ProberRecipeFileName	= 928,
		ProberMachineName		= 944,

		// Tester Information
		TesterRecipeFileName	= 960,
        CondFileName			= 976,		
        BinFileName				= 992,		
        BarCodeFileName			= 1008,		
		// 1008 + 16 = 1024 ( final bit )

    }
       

    public enum EServerQueryCmd : int
    {
        CMD_LOAD_ITEM_FILE		       = 1,
        CMD_OPEN_OUTPUT_FILE	       = 2,
        CMD_TESTER_START		       = 3,
        CMD_TESTER_END			       = 4,
		CMD_TESTER_ABORT		       = 5,
		
		CMD_SET_LOT_ID			       = 6,
        CMD_SET_WAFER_ID		       = 7,
        CMD_AUTOCONTACT			       = 8,
        CMD_AUTOCAL_END			       = 9,
		CMD_OVERWRITE_TESTER_OUTPUT_FILE	= 10,

		CMD_CALC					   = 11,
        CMD_UPDATE_UI_FORM			   = 12,		
		CMD_PRINT_BARCODE			   = 13,

		CMD_LOAD_RECIPE_FROM_PROBER	   = 14,
        CMD_APPEND_TESTER_OUTPUT_FILE  = 15,
        CMD_USER_LOGIN                 = 18,
        CMD_USER_LOGOUT                = 19,
        CMD_CHECK_RCONTACT_STATE       = 20,
        CMD_SEND_BARCODE_TO_PROBER     = 21,

        CMD_CHECK_AVALIABLE_MODE       = 22,
        CMD_LOAD_FILE_TO_TEMP          = 23,
        CMD_CREATE_MAP_FROM_TEMP       = 24,

        CMD_AUTO_CH_CALIB_START = 31,

        CMD_AUTO_CH_CALIB_END = 32,

        CMD_QUERY_PROCESS_INFO = 40,
        CMD_QUERY_DP76_DATA_INFO = 41,
        CMD_QUERY_MC300_DATA_INFO = 42,
        CMD_QUERY_SUB_RECIPE_INFO = 43,
        CMD_QUERY_TESTER_INFO = 44,  
        CMD_QUERY_PRE_SOT_DP_INFO = 45,
        CMD_QUERY_PRE_WAFER_IN_INFO = 46,
        CMD_QUERY_PRE_SOT_PROBER_INFO = 47,

        CMD_LASER_BAR_INFO = 48,
        CMD_SUB_RECIPE_INFO = 49,

		CMD_LOT_END=50,

        CMD_QUERY_PRE_OVERLOAD_TEST_INFO = 60,
        CMD_QUERY_PRE_CHUCK_TEMP_INFO = 61,
        CMD_QUERY_CHECK_LASER_POWER_INFO = 62,

        CMD_WAFER_BEGIN = 70,
        CMD_WAFER_FINISH = 71,

        CMD_SHOW_ERROR = 200,
    }
	
    public enum ETCPClientState : int
    { 
        NONE					= -1,
        CONNECTING				= 1,
        CONNECTED				= 2,
        SENDING					= 3,
        SENDED					= 4,
        READING					= 5,
        READED					= 6,
        ERROR					= 7,
    }
	
    public enum ETCPTestSeverState : int
    { 
        NOT_READY				= 0,
        READY					= 1,
        DISCONNECT				= 2,
        ERROR					= 3,
    }
}
