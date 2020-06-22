using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.MPI.Command
{
    /// <summary>
    /// Barcode Insert Command.
    /// 
	/// typedef struct _PBSORTER_QUERY_VERSION_PACKET
    ///	{
    ///		int nErrorCode;  						// 0: Success, 1: Not exist Test File, 2: Not exist Bin File, 
    ///												// 3: Test File and Bin File not match
    ///		char szTestItemName[MAX_ITEM_NAME];		// Lot No
    ///		char szBinFileName[MAX_ITEM_NAME];		// Wafer No
    ///	}
    /// </summary>
    public class CmdQueryVersion : MPIDS7600Command
    {
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

        // CommandID.
		public const int COMMAND_ID = (int)ETSECommand.ID_QUERY_VERSION;

		// Length		// MAX_DATA_LENGTH + MAX_DATA_LENGTH
		public static int DATA_LEN = Const.MAX_DATA_LENGTH + Const.MAX_DATA_LENGTH;

		// Position
		public static int EQUIPMENT_NAME_POS = 0;
		public static int VERSION_POS = EQUIPMENT_NAME_POS + Const.MAX_DATA_LENGTH;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdQueryVersion()
            : base(COMMAND_ID, DATA_LEN)
        {
        }

		/// <summary>
		/// Equipment Name.
		/// </summary>
		public char[] EquipmentName
		{
			get
			{
				return this.GetData(Const.MAX_DATA_LENGTH, EQUIPMENT_NAME_POS);
			}
			set
			{
				this.SetData(Const.MAX_DATA_LENGTH, EQUIPMENT_NAME_POS, value);
			}
		}

		/// <summary>
		/// Version.
		/// </summary>
		public char[] Version
		{
			get
			{
				return this.GetData(Const.MAX_DATA_LENGTH, VERSION_POS);
			}
			set
			{
				this.SetData(Const.MAX_DATA_LENGTH, VERSION_POS, value);
			}
		}
    }
}
