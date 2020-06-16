using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
	public class CmdWaferEnd : MPIDS7600Command
	{
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_WAFER_END;

		// Length		// MAX_DATA_LENGTH
		public static Int32 DATA_LEN = Const.MAX_DATA_LENGTH;

		// Position
		public static int WAFER_NO_POS = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
		public CmdWaferEnd()
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
