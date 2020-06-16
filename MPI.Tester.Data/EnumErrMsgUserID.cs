using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	public delegate void ErrorCodeEventHandler(ErrorCodeEventArgs e);

	public class ErrorCodeEventArgs : EventArgs
	{
		public readonly EErrorCode ErrorCode;
		public readonly string ErrorMsg;

		public ErrorCodeEventArgs(EErrorCode errorCode, string errorMsg = "")
		{
			this.ErrorCode = errorCode;
			this.ErrorMsg = errorMsg;
		}
	}

	public enum EErrorCode : int
	{
		NONE							= 0,
		UndefinedErrorCode				= 1,

		//--------------------------------------------------------------------------
		// The Error Code ID = 100 ~ 999
		// The Range is for Device Error Number is Defined in
		// MPI.Tester.DeviceCommon namespace
		//--------------------------------------------------------------------------

		//------------------------------------------------------------
		// SpectroMeter Device Error Number
		//------------------------------------------------------------
		SpectrometerDevice_Init_Err				= 201,
		SpectrometerSN_Err						= 202,
		SphereSN_Err							= 203,
		NoSpectromSettingData					= 204,
		OPRS_AutoCountSetting_Err				= 205,

		OPRS_Reconnect_Fail						= 206,
		OPRS_Calculation_Err					= 207,
		OPRS_Trigger_Err						= 208,
		OPRS_GetIntensity_Fail					= 209,
		OPRS_									= 210,

		GetWavelength_Fail						= 211,
		NoGetDarkArray							= 212,
		LoadCalibrationFileFail					= 213,
		SPAMDriver_Init_Err						= 214,
        SpectrometerDevice_Connect_Error		= 215,
        Spectrometer_Measurement_Err			= 216,
        Spectrometer_SetWavelength_Err			= 217,
        Spectrometer_SetCalibrationSpectrum_Err = 218,

		//------------------------------------------------------------
		// Source Meter Device Error Number
		//------------------------------------------------------------
		SourceMeterDevice_Init_Err			= 301,
		NoSourceMeterParamSettingData		= 302,
		SourceMeterIndexSetting_Err			= 303,
		SweepSetting_Err					= 304,
		PMUSetting_Err						= 305,

		ForceOutput_Ctrl_Err				= 306,
		SourceMeterDevice_HW_Err			= 307,
		NoMatchRangeIndex					= 308,
		ClampValueSetting_Err				= 309,
		CalcSweepForceRangeIndex_Err		= 310,
		SweepPointsSetting_Err				= 311,
		ParameterSetting_Err				= 312,
		DutyRate_Err						= 313,
		ParameterLengthExcessBufferSize		= 314,
		MeterOutput_Ctrl_Err				= 315,
        OpenShortParameterSetting_Err		= 316,
        SourceMeterDevice_Disconnect_Err	= 317,
        SourceMeterDllMissing_Err			= 318,
		SourceMeterAcquireDataTimeout_Err	= 319,
        SourceMeterNotDetectedSMUB_Err      = 320,
        MeterOutput_Interlock_Err           = 321,

        //------------------------------------------------------------
        // ESD Device Error Number
        //------------------------------------------------------------
        EsdHWInitFail                             = 401,
        EsdWriteHardwareInfoFile_Err              = 402,
        EsdHWReadInfo_Fail                        = 403,
        EsdRead_CalibrationFile_Fail              = 404,
        EsdRead_CalibrationFile_SerialNumber_Fail = 405,
        EsdRead_CalibrationFile_MechineType_Fail  = 406,
        EsdRead_CalibrationFile_Ver_Fail          = 407,
        EsdRead_CalibrationFile_Number_Fail       = 408,
        EsdSetValue_Err                           = 409,
        EsdSetParameterLength_Err                 = 410,
        EsdNoSettingParameter_Err                 = 411,
        EsdParameterCalibrated_Err                = 412,

        //------------------------------------------------------------
        // Switch System Device Error Number
        //------------------------------------------------------------
        SwitchHWInitFail				= 501,
        SwitchNoCardInstall				= 502,
        SwitchConfigDataMissing			= 503,
        SwitchChannelSetting_Err        = 504,
        //------------------------------------------------------------
        // DAQ Device Error Number
        //------------------------------------------------------------
        DAQDevice_Init_Err				= 601,
        DAQAcquireDataTimeout_Err		= 602,

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
		//------------------------------------------------------------
		// System Error Code
		//------------------------------------------------------------
		System_Not_Ready				= 1001,
		NotFinishMoveTestedData			= 1002,
		FilterWheelSettingErr			= 1003,

        SystemChannelConfigNotMatch		= 1101,

        //------------------------------------------------------------
        // TCPIP Error Code
        //------------------------------------------------------------
        TCPIP_Err = 1500,
        TCPIP_CommandFactory_Err = 1501,
        TCPIP_CheckPacket_Err = 1502,
        TCPIP_TransferableItem_Err = 1503,

        TCPIP2_Err = 1600,
        TCPIP2_CommandFactory_Err = 1601,
        TCPIP2_CheckPacket_Err = 1602,
        TCPIP2_TransferableItem_Err = 1603,
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		//------------------------------------------------------------
		// UI Error Code
		//------------------------------------------------------------
		// LoadMachine						= 10001,
		// NoFileExisted					= 10002,
		LoadUISettingFail					= 10003,
		LoadTesterSettingFail				= 10004,
		LoadProductFileFail					= 10005,

		LoadBinDataFileFail					= 10006,
		LoadUserDataFail					= 10007,
		LoadTaskSheetFail					= 10008,
		ResetUserDataFail					= 10009,
		NoMatchUserFormat					= 10010,

		TaskSheetFileExisted				= 10011,
		TestResultFileExisted				= 10012,
		MachineCfgSettingErr				= 10013,
		NoMatchDeviceSpec					= 10014,
		Reserved_01							= 10015,

		LOPSaveItemNotMatch					= 10016,
		SaveFileFail						= 10017,
		SaveFilePathSetingFail				= 10018,
		NoSaveFileName						= 10019,

		BinTransfer_FindTitleIndexFail		= 10020,
		BinTransfer_FindBinIndexFail		= 10021,
		BinTransfer_Fail					= 10022,
		BinTransfer_TargetPathSettingFail	= 10023,
		BinTransfer_LoadTestDataFail		= 10024,

		BinTransfer_SaveTransferDataFail	= 10025,
		ImportCalibrateDatafFail			= 10026,
        ImportCalibrateNotMathSystem        = 10126,
        ImportChuckCalibDataFile            = 10127,
		ExportCalibrateDataFail				= 10027,
		ExportPathSettingFail				= 10028,
		ImportBinTableFail					= 10029,
		ExportBinTableFail					= 10030,

		AddMsrtItem_SelectedItemsError		= 10031,
		ImportBin_RepeatBinItem				= 10032,
		ImportBin_TitleFormatError			= 10033,
		ImportBin_BinItemNotExist			= 10034,
		ImportBin_DataValueError			= 10035,
		
		NoUserDataInFolder					= 10036,
        ImportSpectrometerCalibDataFail		= 10037,
        ResultFolderIsNotReady				= 10038,
        SaveWatchCoefficientFileFail        = 10039,

        NotEnoughMemoryToSaveReportFile     = 10040,

		LoadMapDataFileFail					= 10041,
		CommunicationModeError				= 10042,

        OutputExtFilenameRepeat             = 10043,

		TCPIPConnectTimeout					= 10044,

        ByRecipeOutputFileFormatError       = 10050,

        DataRecoveryTitleNotMatch           = 10051,
        LoadUserTableFail                   = 10052,

		OutputFilePathRepeat				= 10053,

        //------------------------------------------------------------
        // MutliDie Error Msg
        //------------------------------------------------------------
        ProductChannelConditionNotMatch     = 10060,
        ChannelOrderSettingErr              = 10061,

		//------------------------------------------------------------
		// Weimin sequence , UI Error Code	10101 ~ 10200
		//------------------------------------------------------------
		WM_MES_FullyModeLoadMKFileFail		= 10101,
		WM_MES_MKFileContentErr				= 10102,
		WM_MES_NotMatchLotNumber			= 10103,

		WM_MES_CaliValueNotMatch			= 10104,
		WM_MES_SamplingModeLoadMKFileFail	= 10105,
		WM_MES_NotMacthWaferNumber			= 10106,
        //------------------------------------------------------------
        // Tools Error , UI Error Code	20001 ~ 30000
        //------------------------------------------------------------
        Tools_LoadStdDataFail					= 20001,
        Tools_LoadMsrtDataFail					= 20002,
        Tools_NoCompareData						= 20003,
        Tools_CompareDataFail					= 20004,
        Tools_UserDataStdTitleIsNotMatch		= 20005,
        Tools_UserDataMsrtTitleIsNotMatch		= 20006,
        Tools_UserDataNotDefineRowCol			= 20007,
        Tools_UserDataNotDefineSeq				= 20008,
        DailyCheck_LoadUserFormatError			= 20100,
        DailyCheck_LoadSpecByRecipeFail			= 20101,
        DailyCheck_LoadSpecByFileNamFail		= 20102,
        DailyCheck_LoadStdFileFail				= 20103,
        DailyCheck_LoadMsrtFileFail             = 20103,
        DailyCheck_NoCompareData                = 20104,
        DailyCheck_SaveCompFileFail             = 20105,
        DailyCheck_SaveResultLogFail            = 20106,
        DailyCheck_SpeFileIsNotExist			= 20107,
        DailyCheck_NotFindMacthRecipe			= 20108,
        DailyCheck_ParseUserFormatError			= 20109,
        DailyCheck_NotFinishingTheWork			= 20110,
        DailyCheck_AvgBiasOutSpec				= 20111,
        DailyCheck_BoundaryOutSpec				= 20112,
        DailyCheck_LessThanMinAcceptDies		= 20113,
        DailyCheck_MsrtFileIsOverdue			= 20114,

		PassRateCheckFail						= 20115,

        //------------------------------------------------------------
        // Customer MES UI Error Code	30001 ~ 40000
        //------------------------------------------------------------
        MES_CondDataNotExist					= 30000,
        MES_LoadTaskError						= 30001,
        MES_LoadCalibFileError					= 30002,
        MES_LoadOutputPathError					= 30003,
		MES_BarcodeError						= 30004,
        MES_ServerConnectFail					= 30005,
        MES_OpenFileError						= 30006,
        MES_TargetRecipeNoExist					= 30007,
        MES_NotMatchRecipe						= 30008,
        MES_LoadDataFromUserFormatError			= 30009,
        MES_ParseFormatError					= 30010,
        MES_SaveRecipeToFileError				= 30011,
  		MES_LoadBinTableFileError				= 30012,
        MES_ReferenceDataNotExist				= 30013,  
        MES_NoWaferIDDescribeData				= 30014,
        MES_RM2FileNotExist						= 30015,
        MES_DailyCheckIsNotReady				= 30016,
        MES_RecipeContentNotMatch				= 30017,
        MES_UndefineMesProcess					= 30018,
        //------------------------------------------------------------
        // Barcode Print Error Code	40001 ~ 50000
        //------------------------------------------------------------
        PRINT_PrintProgramNotExist				= 40001,
        PRINT_SourceDataFileNotExist			= 40002,
        PRINT_XsltFileNotExist					= 40003,
        PRINT_XslTransformFail					= 40004,
        PRINT_PrintFail							= 40005,
        //------------------------------------------------------------
        // Report Error Code	50001 ~ 60000
        //------------------------------------------------------------
        REPORT_ReplaceDataFail            		= 50001,
		REPORT_Merge_ReadReportHeadFail         = 50002,
        REPORT_Merge_GetReportHeadKeyFail       = 50003,
        REPORT_Merge_ReadReportCondition        = 50004,
        REPORT_MergeFileReadReportFail          = 50005,
        REPORT_MergeFileNumberNotMatch          = 50006,
        REPORT_Merge_FilePathError              = 50007,
        REPORT_Merge_BackupFileFail             = 50008,
        REPORT_Merge_OuputFileFail              = 50009,
        REPORT_Merge_FileNameError              = 50010,
        REPORT_Merge_FileNumberNotMatch         = 50011,
        REPORT_File_To_Temp_Fail                = 50012,
        REPORT_Temp_To_Map_Fail                 = 50013,
		REPORT_AddReportCodeFail				= 50014,
        REPORT_NF_Fail                          = 50015,
        REPORT_FF_Fail                          = 50015,
        REPORT_NFB_Fail                         = 50016,
		REPORT_MergeFileFail                    = 50017,

        REPORT_ConvertFail                      = 50020,

        REPORT_File1_Create_Fail                = 50021,
        REPORT_File2_Create_Fail                = 50022,
        REPORT_File3_Create_Fail                = 50023,

        REPORT_Customize_File_Create_Fail        = 50024,
	}

    public enum EMessageCode : int
    {
        NONE = 0,
        CheckResetGainOffsetCoeff                = 001,
        CheckResetTableCoeff                     = 002,
        CheckResetChuckGainOffset                = 003,
        CheckIsOverWriteTestOutputFile           = 004,
        CheckIsReStartSystem                     = 005,
        LogOut                                   = 006,
        ResetSystemState                         = 007,
        DataFormatError                          = 008,
        DeleteTaskSheet                          = 009,
        InsrtTestItem                            = 010,
        DeleteTestItem                           = 011,
        RunSingleReTest                          = 012,
 		linkupProbing                            = 013,
        CheckIsResetESDSystemParameter           = 014,
        ExitApplication                          = 015,
        CheckNormalizedByChannel                 = 050,

        Tools_DeleteTooMuchChips                 = 101,
        Tools_ConfirmCoverGainOffset             = 102,
        Tools_ConfirmSetTableCoeff               = 103,
        Tools_CombineThisTestItem                = 104,
        Tools_CombineAllTestItem                 = 105,
        Tools_BinCountTooMuch                    = 107,
        Tools_CombineSysCoef                     = 108,

        Tools_RunDeviceVerifySquence       = 111,
        Tools_RunDeviceVerifyBiasRegister  = 112,

        Tools_ConfirmOverrideCoefTable     = 204,

        

        CheckIzClampVoltage = 301,

    }
}
