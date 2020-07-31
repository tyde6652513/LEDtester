using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
    /// <summary>
    /// Command set for communication with TSE/MPI Tester.
    /// </summary>
    public enum ETSECommand : int
    {
        ID_LOT_IN                  = 50000,   // P  -->  T
        ID_LOT_INFO                = 50001,   // P  <--  T
        ID_WAFER_END               = 50002,   // P  <->  T
        ID_WAFER_IN                = 60000,   // P  -->  T
        ID_TEST_ITEM               = 60001,   // P  <--  T
        ID_WAFER_SCAN_END          = 60002,   // P  -->  T
        ID_SOT                     = 60003,   // P  -->  T
        ID_EOT                     = 60004,   // P  <--  T
        ID_RETEST                  = 60006,   // P  <--  T
        ID_AUTOCONTACT             = 60007,   // P  -->  T
        ID_LOT_END                 = 60009,   // P  <->  T
        ID_AUTOSTATUS_STOP         = 60010,   // P  <->  T
        ID_ATTOSTATUS_START        = 60011,   // P  <->  T
        ID_BARCODE_INSERT          = 60012,   // P  <->  T
        ID_REOT                    = 60013,   // P  <--  T
        ID_BARCODE_PRINT           = 60014,   // P  ---  T  // ???
        ID_OVERRIDE_TESTER_REPORT  = 60015,   // P  <->  T
        ID_QUERY_VERSION           = 60016,   // P  ---  T  // ???
        ID_AUTOCAL_START           = 60020,   // P  -->  T
        ID_AUTOCAL_END             = 60021,   // P  -->  T
        ID_AUTOCAL_SOT             = 60022,   // P  -->  T
        ID_AUTOCAL_EOT             = 60023,   // P  <--  T
        ID_AUTICAL_FAIL            = 60024,   // P  <--  T 
        ID_QUERY_INFORMATION       = 60025,   // p  <->  T
        ID_WAFER_BEGIN             = 60026,   // P  <->  T
        ID_WAFER_FINISH            = 60027,   // P  <->  T
        ID_QUERY_ABLE_MODE = 60028,
        ID_SET_TEST_MODE = 60029,
        ID_TEST_ABORT              = 60030,
        ID_ERROR_LOT_NOT_SETTING   = 60100,   // P  <--  T
        ID_ERROR_NO_TEST_ITEM_FIlE = 60101,   // P  <--  T
        ID_ERROR_NO_BIN_FILE       = 60102,   // P  <--  T
        ID_ERROR_NOT_EQUAL_ITEM    = 60103,   // P  <--  T
        ID_BIN_GRADE               = 60200,   // P  <--  T
        ID_WAFER_IN_INFO           = 60300,   // P  -->  T  // like ID_WAFER_IN, but with more information, For Weimin tester only
		ID_MUTIL_DIE_SOT           = 60400,   // P  -->  T
		ID_MUTIL_DIE_EOT           = 60401,   // P  <--  T
		ID_SOT2                    = 60402,   // P  -->  T
		ID_EOT2                    = 60403,   // P  <--  T

        ID_ERROR                   = 70000,   // P  <--  T
    }
}
