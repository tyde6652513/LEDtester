using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
    /// <summary>
    /// Bin Grade Command.
    /// 
    /// typedef struct _PBSORTER_CMD_BIN_GRADE_NAME
    ///	{
    ///		char szItemName[MAX_ITEM_NAME];					    // Bin Grade Name
    ///	} 
    /// 
    /// typedef struct _PBSORTER_BIN_GRADE_PACKET
    ///	{
    ///		int iBinGradeCount;									// Bin Grade Count
    ///		_PBSORTER_CMD_BIN_GRADE_NAME BinGradeList[MAX_BIN_GRADE_LIST];
    ///	}
    /// </summary>
    public class CmdTestAbort : CmdMPIBased
    {
        public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

        // CommandID.
        public const int COMMAND_ID = (int)ETSECommand.ID_TEST_ABORT;

        // Length		// sizeof(Int32) + MAX_BIN_GRADE_NAME * MAX_BIN_GRADE_LIST
        public static int DATA_LEN = sizeof(Int32) ;

        // Position
                // Position
        public static int ABORT_SAVE_FILE_POS = 0;
        
        public CmdTestAbort()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

        /// <summary>
        /// Item Count.
        /// </summary>
        public Int32 ABORT_SAVE_FILE
        {
            get
            {
                return this.GetInt32Data(ABORT_SAVE_FILE_POS);
            }
            set
            {
                this.SetInt32Data(ABORT_SAVE_FILE_POS, value);
            }
        }

       
    }
}
