using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
	public class CmdError : CmdMPIBased
	{
        public enum EErrorCode  // DS_ERROR_CODE
        {
            Undefined = 1,
            Unknown = -1,                    // DS_ERROR_UNKNOW_ERROR = -1,
            LotInFailed = -2,                // DS_ERROR_LOT_NO_FAIL = -2,
            WaferInFailed = -3,              // DS_ERROR_WAFER_ID_FAIL = -3,
            SOTFailed = -4,                  // DS_ERROR_SOT_FAIL = -4,
            WaferEndFailed = -5,             // DS_ERROR_WAFER_END_FAIL = -5,
            LotEndFailed = -6,               // DS_ERROR_LOT_END_FAIL = -6,
            WaferScanFailed = -7,            // DS_ERROR_WAFER_SCAN_END_FAIL = -7,
            BarcodeInsertFailed = -8,        // DS_ERROR_BARCODE_INSERT_FAIL = -8,
            WaferIDExisted = -9,             // DS_ERROR_TESTER_WAFER_ID_EXIST = -9,
            PrintBarcodeFailed = -10,        // DS_ERROR_PRINT_BARCODE_FAIL = -10,
            OverrideReportFailed = -11,      // DS_ERROR_TESTER_OVERRIDE_REPORT_FAIL = -11,
            SourceMeterDisconnect = -12,     // DS_ERROR_TESTER_SOURCEMETER_DISCONNECT = -12,
            TesterCommandLineWaitting = -13, // DS_ERROR_TESTER_COMMAND_LINE_WAITTING = -13,
            CheckChannelFail = -14,          // DS_ERROR_CheckChannelFail = -14,
        }
        
        public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

		// CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_ERROR;

		// Length		// sizeof(Int32)
		public static Int32 DATA_LEN = sizeof(Int32) + Const.MAX_DATA_LENGTH;

		// Position
		public static int ERROR_CODE_POS = 0;
		public static int ERROR_TEXT_POS = ERROR_CODE_POS + sizeof(Int32);

        /// <summary>
        /// Constructor.
        /// </summary>
		public CmdError()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

		/// <summary>
		/// Error Code
		/// </summary>
		public Int32 nErrorCode
		{
			get
			{
				return this.GetInt32Data(ERROR_CODE_POS);
			}
			set
			{
				this.SetInt32Data(ERROR_CODE_POS, value);
			}
		}

        /// <summary>
        /// Error Code.
        /// </summary>
        public EErrorCode ErrorCode
        {
            get
            {
                EErrorCode code = EErrorCode.Undefined;

                return Enum.TryParse<EErrorCode>(nErrorCode.ToString(), out code) ? code : EErrorCode.Undefined;
            }
            set
            {
                this.nErrorCode = value.GetHashCode();
            }
        }

		/// <summary>
		/// Error text charactors.
		/// </summary>
		public char[] ErrorTextChars
		{
			get
			{
				return this.GetData(Const.MAX_DATA_LENGTH, ERROR_TEXT_POS);
			}
			set
			{
				this.SetData(Const.MAX_DATA_LENGTH, ERROR_TEXT_POS, value);
			}
		}

        /// <summary>
        /// Error string
        /// </summary>
        public string ErrorText
        {
            get
            {
                string temp = new String(ErrorTextChars);

                return temp.TrimEnd("\0".ToCharArray());
            }
        }

	}
}
