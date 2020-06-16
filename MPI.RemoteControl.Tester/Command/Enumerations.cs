using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester
{
    /// <summary>
    /// Command set for communication with TSE Tester.
    /// </summary>
    public enum ETSECommand : int
    {
        ID_LOT_IN = 50000,   // P  -->  T
        ID_LOT_INFO = 50001,   // P  <--  T
        ID_WAFER_END = 50002,   // P  <->  T
        ID_WAFER_IN = 60000,   // P  -->  T
        ID_TEST_ITEM = 60001,   // P  <--  T
        ID_WAFER_SCAN_END = 60002,   // P  -->  T
        ID_SOT = 60003,   // P  -->  T
        ID_EOT = 60004,   // P  <--  T
        ID_RETEST = 60006,   // P  <--  T
        ID_AUTOCONTACT = 60007,   // P  -->  T
        ID_LOT_END = 60009,   // P  <->  T
        ID_AUTOSTATUS_STOP = 60010,   // P  <->  T
        ID_ATTOSTATUS_START = 60011,   // P  <->  T
        ID_BARCODE_INSERT = 60012,   // P  <->  T
        ID_REOT = 60013,   // P  <--  T
        ID_BARCODE_PRINT = 60014,   // P  ---  T  // ???
        ID_OVERRIDE_TESTER_REPORT = 60015,   // P  <->  T
        ID_QUERY_VERSION = 60016,   // P  ---  T  // ???
        ID_AUTOCAL_START = 60020,   // P  -->  T
        ID_AUTOCAL_END = 60021,   // P  -->  T
        ID_AUTOCAL_SOT = 60022,   // P  -->  T
        ID_AUTOCAL_EOT = 60023,   // P  <--  T
        ID_AUTICAL_FAIL = 60024,   // P  <--  T 
        ID_QUERY_INFORMATION = 60025,   // p  <->  T
        ID_WAFER_BEGIN = 60026,   // P  <->  T
        ID_WAFER_FINISH = 60027,   // P  <->  T

        ID_QUERY_ABLE_MODE = 60028,   // P  -->  T  // 
        ID_SET_TEST_MODE = 60029,   // P  -->  T  // 
        ID_TEST_ABORT = 60030,

        ID_ERROR_LOT_NOT_SETTING = 60100,   // P  <--  T
        ID_ERROR_NO_TEST_ITEM_FIlE = 60101,   // P  <--  T
        ID_ERROR_NO_BIN_FILE = 60102,   // P  <--  T
        ID_ERROR_NOT_EQUAL_ITEM = 60103,   // P  <--  T
        ID_BIN_GRADE = 60200,   // P  <--  T
        ID_WAFER_IN_INFO = 60300,   // P  -->  T  // like ID_WAFER_IN, but with more information, For Weimin tester only

        ID_MUTIL_DIE_SOT = 60400,
        ID_MUTIL_DIE_EOT = 60401,

        ID_ERROR = 70000,   // P  <--  T
    }

	/// <summary>
	/// Command set for communication with IS Tester.
	/// </summary>
	public enum EISCommand : int
	{
		ID_TTST4IP_RESULT = 1,
		ID_TTST4IP_SOJ = 2,
		ID_TTST4IP_SOT = 3,
		ID_TTST4IP_EOT = 4,
		ID_TTST4IP_JOB_INFO = 5,
		ID_TTST4IP_ADDITIONAL_HEADER = 6,
		ID_TTST4IP_ADDITIONAL_RESULT = 7,
		ID_TTST4IP_SET_RESULT_FILE = 8,
		ID_TTST4IP_GET_MEASUREMENT_RESULT = 9,
		ID_TTST4IP_MEASUREMENT_RESULT = 10,
	}

	/// <summary>
	/// Command Return Error Code for IS Tester
	/// </summary>
	public enum EISCmdErrorCode : int
	{
		//---------------------------------------------------------
		// Error Code Base Offset for distinguish IS Command & MPIDS7600 Command
		ID_ERR_IS_BASE_OFFSET = -10000,
		//---------------------------------------------------------
		// IS Standard Error Code
		ID_ERR_NO_ERROR = 0,
		ID_ERR_UNEXPECTED_ERROR = -1,
		ID_ERR_JOB_IS_RUNNING = -2,
		ID_ERR_JOB_FILE_NOT_FOUND = -3,
		ID_ERR_NO_REMOTE_HANDLER = -4,
		//---------------------------------------------------------
		// IS Command Extended Error Code
		ID_ERR_GET_MEASUREMENT_RESULT_FAIL = -1001,
		ID_ERR_PARSE_RESULT_CODE_FAIL = -1002,
		ID_ERR_UNEXPECTED_ECHO_COMMAND = -1003,
		ID_ERR_ISCMD_SOJ_START_TIMEOUT = -1004,
		ID_ERR_ISCMD_SOJ_STOP_TIMEOUT = -1005,
		ID_ERR_ISCMD_SOT_TIMEOUT = -1006,
		ID_ERR_ISCMD_GET_MEASUREMENT_TIMEOUT = -1007,
		ID_ERR_ISCMD_SET_RESULT_FILE_TIMEOUT = -1008,
		ID_ERR_ISCMD_ADDITIONAL_HEADER_TIMEOUT = -1009,
		ID_ERR_ISCMD_ADDITIONAL_RESULT_TIMEOUT = -1010,
		ID_ERR_ISCMD_JOB_INFO_TIMEOUT = -1011,
	}

	public enum EAtvCmdErrorCode : int
	{
		//---------------------------------------------------------
		// Error Code Base Offset for distinguish ATV Command & MPIDS7600 Command
		ID_ERR_ATV_BASE_OFFSET = -20000,
		//---------------------------------------------------------
		// ATV Standard Error Code
		ID_ERR_ATV_NO_ERROR = 0,
		ID_ERR_ATV_LOAD_MEASUREMENT_PROGRAM_FAIL = -1,
		ID_ERR_ATV_START_CURRENTLY_LOADED_PROGRAM_FAIL = -2,
		ID_ERR_ATV_STOP_CURRENTLY_LOADED_PROGRAM_FAIL = -3,
		ID_ERR_ATV_START_A_MEASUREMENT_CYCLE_FAIL = -4,
		ID_ERR_ATV_QUERY_MEASUREMENT_RESULT_FAIL = -5,
		ID_ERR_ATV_QUERY_MEASUREMENT_HEAD_FAIL = -6,
		ID_ERR_ATV_QUERY_VARIABLE_VALUE_FAIL = -7,
		ID_ERR_ATV_QUERY_BIN_TABLE_FAIL = -8,
		ID_ERR_ATV_QUERY_STATISTIC_DATA_FAIL = -9,
		ID_ERR_ATV_SET_VARIABLE_FAIL = -10,
		//---------------------------------------------------------
		// ATV Timeout Error Code
		ID_ERR_ATV_LOAD_MEASUREMENT_PROGRAM_TIMEOUT = -1001,
		ID_ERR_ATV_START_CURRENTLY_LOADED_PROGRAM_TIMEOUT = -1002,
		ID_ERR_ATV_STOP_CURRENTLY_LOADED_PROGRAM_TIMEOUT = -1003,
		ID_ERR_ATV_START_A_MEASUREMENT_CYCLE_TIMEOUT = -1004,
		ID_ERR_ATV_QUERY_MEASUREMENT_RESULT_TIMEOUT = -1005,
		ID_ERR_ATV_QUERY_MEASUREMENT_HEAD_TIMEOUT = -1006,
		ID_ERR_ATV_QUERY_VARIABLE_VALUE_TIMEOUT = -1007,
		ID_ERR_ATV_QUERY_BIN_TABLE_TIMEOUT = -1008,
		ID_ERR_ATV_QUERY_STATISTIC_DATA_TIMEOUT = -1009,
		ID_ERR_ATV_SET_VARIABLE_TIMEOUT = -1010,
		//---------------------------------------------------------
		// ATV Extended Error Code
		ID_ERR_ATV_TESTER_RECIPE_SETTING_IS_EMPTY = -2001,
		ID_ERR_ATV_TESTER_RECIPE_FILE_NOT_EXIST = -2002,
		ID_ERR_ATV_TESTER_RECIPE_LOAD_FAIL = -2003,
	}

	public enum ETesterID
	{
		TSE = 0,
		GTester,
		Epistar,
		Weimin,
		IS,
		ATV,
	}
}
