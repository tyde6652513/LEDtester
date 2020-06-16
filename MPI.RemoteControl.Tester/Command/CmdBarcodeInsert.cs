using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
    /// <summary>
    /// Barcode Insert Command.
    /// 
    /// typedef struct _PBSORTER_BARCODE_INSERT_PACKET
    ///	{
    ///		int nErrorCode;  						// 0: Success, 1: Not exist Test File, 2: Not exist Bin File, 
    ///												// 3: Test File and Bin File not match
    ///		char szTestItemName[MAX_ITEM_NAME];		// Lot No
    ///		char szBinFileName[MAX_ITEM_NAME];		// Wafer No
    ///	}
    /// </summary>
    public class CmdBarcodeInsert : MPIDS7600Command
    {
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

        // CommandID.
        public const int COMMAND_ID = (int)ETSECommand.ID_BARCODE_INSERT;

		// Length		// MAX_DATA_LENGTH + MAX_DATA_LENGTH
		public static int DATA_LEN = Const.MAX_DATA_LENGTH + Const.MAX_DATA_LENGTH;

        // Position
		public static int TEST_ITEM_NAME_POS = 0;
		public static int BIN_FILE_NAME_POS = TEST_ITEM_NAME_POS + Const.MAX_DATA_LENGTH;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdBarcodeInsert()
            : base(COMMAND_ID, DATA_LEN)
        {
        }

        /// <summary>
        /// Test Item Name.
        /// </summary>
        public char[] TestItemName
        {
            get
            {
				return this.GetData(Const.MAX_DATA_LENGTH, TEST_ITEM_NAME_POS);
            }
            set
            {
				this.SetData(Const.MAX_DATA_LENGTH, TEST_ITEM_NAME_POS, value);
            }
        }

        /// <summary>
        /// Bin File Name.
        /// </summary>
        public Char[] BinFileName
        {
            get
            {
				return this.GetData(Const.MAX_DATA_LENGTH, BIN_FILE_NAME_POS);
            }
            set
            {
				this.SetData(Const.MAX_DATA_LENGTH, BIN_FILE_NAME_POS, value);
            }
        }
    }
}
