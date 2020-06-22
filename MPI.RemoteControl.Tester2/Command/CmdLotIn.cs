using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
	public class CmdLotIn : CmdMPIBased
	{
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

		// CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_LOT_IN;

		// Length		// MAX_DATA_LENGTH
		public static Int32 DATA_LEN = Const.MAX_DATA_LENGTH;

		// Position
		public static int LOT_NO_POS = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
		public CmdLotIn()
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

	}
}
