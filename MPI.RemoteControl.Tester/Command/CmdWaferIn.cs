using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
    /// <summary>
    /// Wafer In Command.
    /// 
    /// typedef struct _PBSORTER_WAFER_IN_COMMAND
    ///	{
    ///		char szLotNo[MAX_ITEM_NAME];                         	// Lot No
    ///		char szWaferNo[MAX_ITEM_NAME];                      	// Wafer No
    ///		char szOperatorName[MAX_ITEM_NAME];                		// Operator Name
    ///	} 
    /// </summary>
    public class CmdWaferIn : MPIDS7600Command
    {
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
        public const int COMMAND_ID = (int)ETSECommand.ID_WAFER_IN;

		// Length		// MAX_DATA_LENGTH + MAX_DATA_LENGTH + MAX_DATA_LENGTH = 60 bytes
		public static int DATA_LEN = Const.MAX_DATA_LENGTH + Const.MAX_DATA_LENGTH + Const.MAX_DATA_LENGTH;
       
        // Position
		public static int LOT_NO_POS = 0;
		public static int WAFER_NO_POS = LOT_NO_POS + Const.MAX_DATA_LENGTH;
		public static int OPERATOR_POS = WAFER_NO_POS + Const.MAX_DATA_LENGTH;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdWaferIn()
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
    }
}
