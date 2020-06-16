using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
	public enum DS_ERROR_CODE
	{
		DS_ERROR_UNKNOW_ERROR = -1,
		DS_ERROR_LOT_NO_FAIL = -2,
		DS_ERROR_WAFER_ID_FAIL = -3,
		DS_ERROR_SOT_FAIL = -4,
		DS_ERROR_WAFER_END_FAIL = -5,
		DS_ERROR_LOT_END_FAIL = -6,
		DS_ERROR_WAFER_SCAN_END_FAIL = -7,
		DS_ERROR_BARCODE_INSERT_FAIL =  -8,
        DS_ERROR_TESTER_OUTPUT_FILENAME_EXIST = -9,
        DS_ERROR_PRINT_BARCODE_FAIL = -10,
		DS_ERROR_TESTER_OVERRIDE_REPORT_FAIL = -11,
		DS_ERROR_TESTER_SOURCEMETER_DISCONNECT = -12,
        DS_ERROR_TESTER_NOT_ENOUGH_HD_SPACE = -13,
        DS_ERROR_TESTER_NFT_SATURATION = -14,

        DS_ERROR_TESTER_COMMAND_LINE_WAITTING = -101,
	}

	public class CmdError : MPIDS7600Command
	{
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

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
		public Int32 ErrorCode
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
		/// Error Text
		/// </summary>
		public char[] ErrorText
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

	}
}
