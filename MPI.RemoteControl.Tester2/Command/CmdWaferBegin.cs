using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
    /// <summary>
    /// Wafer Begin Command.
    /// 
    /// typedef struct _PBSORTER_WAFER_BEGIN_COMMAND
    ///	{
    ///		char szWaferNo[MAX_ITEM_NAME];                      	// Wafer No
    ///	} 
    /// </summary>
    public class CmdWaferBegin : CmdMPIBased
    {
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

		// CommandID.
        public const int COMMAND_ID = (int)ETSECommand.ID_WAFER_BEGIN;

		// Length		// MAX_DATA_LENGTH
		public static int DATA_LEN = Const.MAX_DATA_LENGTH;
       
        // Position
        public static int WAFER_NO_POS = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdWaferBegin()
            : base(COMMAND_ID, DATA_LEN)
        {
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
    }
}
