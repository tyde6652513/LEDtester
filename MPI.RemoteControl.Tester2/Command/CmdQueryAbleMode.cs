using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;
using MPI.RemoteControl2.Tester.Mpi.Command.Base;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{

    public class CmdQueryAbleMode : CmdMPIBased
    {
        public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

        // CommandID.
        public const Int32 COMMAND_ID = (Int32)ETSECommand.ID_QUERY_ABLE_MODE;//60028

        // Length		// MAX_DATA_LENGTH + MAX_DATA_LENGTH + MAX_DATA_LENGTH = 60 bytes
        public static Int32 DATA_LEN = Const.MAX_DATA_LENGTH * 10;//9

        // Position
        public static Int32 LOT_NO_POS = 0;
        public static Int32 WAFER_NO_POS = LOT_NO_POS + Const.MAX_DATA_LENGTH;// P->T
        public static Int32 OPERATOR_POS = WAFER_NO_POS + Const.MAX_DATA_LENGTH;// P->T
        public static Int32 PROBER_ITEM_NAME_POS = OPERATOR_POS + Const.MAX_DATA_LENGTH;// P->T
        public static Int32 TEST_ITEM_NAME_POS = PROBER_ITEM_NAME_POS + +Const.MAX_DATA_LENGTH;// P->T
        public static Int32 BIN_FILE_NAME_POS = TEST_ITEM_NAME_POS + Const.MAX_DATA_LENGTH;

        public static Int32 FILE_NAME_REPEAT_POS = BIN_FILE_NAME_POS + Const.MAX_DATA_LENGTH;// T-> P
        public static Int32 FILE_NAME_APPENDABLE_POS = FILE_NAME_REPEAT_POS + Const.MAX_DATA_LENGTH;// T-> P
        public static Int32 TESTER_OUTPUT_FILE_NAME_RELATIVE_PATH_POS = FILE_NAME_APPENDABLE_POS + Const.MAX_DATA_LENGTH;// T-> P
        public static Int32 TESTER_OUTPUT_FILE_COORD = TESTER_OUTPUT_FILE_NAME_RELATIVE_PATH_POS + Const.MAX_DATA_LENGTH;// T-> P

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdQueryAbleMode()
            : base(COMMAND_ID, DATA_LEN)
        {
        }

        /// <summary>
        /// Lot Number.
        /// </summary>
        public char[] LotNo
        {
            get
            {
                return this.GetData(Const.MAX_DATA_LENGTH, LOT_NO_POS);
            }
            set
            {
                this.SetData(Const.MAX_DATA_LENGTH, LOT_NO_POS, value);
            }
        }

        /// <summary>
        /// Wafer Number.
        /// </summary>
        public char[] WaferNo
        {
            get
            {
                return this.GetData(Const.MAX_DATA_LENGTH, WAFER_NO_POS);
            }
            set
            {
                this.SetData(Const.MAX_DATA_LENGTH, WAFER_NO_POS, value);
            }
        }

        public char[] OperatorName
        {
            get
            {
                return this.GetData(Const.MAX_DATA_LENGTH, OPERATOR_POS);
            }
            set
            {
                this.SetData(Const.MAX_DATA_LENGTH, OPERATOR_POS, value);
            }
        }

        public char[] ProberMainRecipe
        {
            get
            {
                return this.GetData(Const.MAX_DATA_LENGTH, PROBER_ITEM_NAME_POS);
            }
            set
            {
                this.SetData(Const.MAX_DATA_LENGTH, PROBER_ITEM_NAME_POS, value);
            }
        }

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


        public char[] BinFileName
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

        public Boolean FileNameRepeat//exist file with same file Name =>  retestable
        {
            get
            {
                return this.GetBoolData(FILE_NAME_REPEAT_POS);
            }
            set
            {
                this.SetBoolData(FILE_NAME_REPEAT_POS, value);
            }
        }

        public Boolean FileAppendable//exist temp file with same file Name =>  appendable
        {
            get
            {
                return this.GetBoolData(FILE_NAME_APPENDABLE_POS);
            }
            set
            {
                this.SetBoolData(FILE_NAME_APPENDABLE_POS, value);
            }
        }

        public char[] TesterOutputFileNameRelativePath
        {
            get
            {
                return this.GetData(Const.MAX_DATA_LENGTH, TESTER_OUTPUT_FILE_NAME_RELATIVE_PATH_POS);
            }
            set
            {
                this.SetData(Const.MAX_DATA_LENGTH, TESTER_OUTPUT_FILE_NAME_RELATIVE_PATH_POS, value);
            }
        }

        public int TesterCoord
        {
            get
            {
                return this.GetInt32Data(TESTER_OUTPUT_FILE_COORD);
            }
            set
            {
                this.SetInt32Data(TESTER_OUTPUT_FILE_COORD, value);
            }
        }

    }
}
