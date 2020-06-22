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
    public class CmdBinGrade : CmdMPIBased
    {
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

        // CommandID.
        public const int COMMAND_ID = (int)ETSECommand.ID_BIN_GRADE;

		// Length		// sizeof(Int32) + MAX_BIN_GRADE_NAME * MAX_BIN_GRADE_LIST
		public static int DATA_LEN = sizeof(Int32) + Const.MAX_BIN_GRADE_NAME * Const.MAX_BIN_GRADE_LIST;

        // Position
		public static int BINGRADE_COUNT_POS = 0;
        public static int BINGRADE_LIST_POS = BINGRADE_COUNT_POS + sizeof(Int32);

        public CmdBinGrade()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

        /// <summary>
        /// Item Count.
        /// </summary>
        public Int32 BinGradeCount
        {
            get
            {
                return this.GetInt32Data(BINGRADE_COUNT_POS);
            }
            set
            {
                this.SetInt32Data(BINGRADE_COUNT_POS, value);
            }
        }

        /// <summary>
        /// Get Bin Grade Name.
        /// </summary>
        public char[] GetBinGradeName(int i)
        {
			if (i < 0 || i >= Const.MAX_BIN_GRADE_LIST) return null;

			return this.GetData(Const.MAX_BIN_GRADE_NAME, BINGRADE_LIST_POS + (i * Const.MAX_BIN_GRADE_NAME));
        }

        /// <summary>
        /// Get Bin Grade Name.
        /// </summary>
        public string GetBinGradeNameString(int i)
        {
            return new string(GetBinGradeName(i));
        }

        /// <summary>
        /// Set Bin Grade Name Name.
        /// </summary>
        public void SetBinGradeName(int i, char[] binGradeName)
        {
			if (i < 0 || i >= Const.MAX_BIN_GRADE_LIST) return;
			if (binGradeName.Length > Const.MAX_BIN_GRADE_NAME) return;

			int startPos = BINGRADE_LIST_POS + (i * Const.MAX_BIN_GRADE_NAME);
			Array.Clear(this.Data, startPos, Const.MAX_BIN_GRADE_NAME);
            this.SetData(binGradeName.Length, startPos, binGradeName);
        }

        /// <summary>
        /// Set Bin Grade Item Name.
        /// </summary>
        public void SetBinGradeName(int i, string binGradeName)
        {
            SetBinGradeName(i, binGradeName.ToCharArray());
        }
    }
}
