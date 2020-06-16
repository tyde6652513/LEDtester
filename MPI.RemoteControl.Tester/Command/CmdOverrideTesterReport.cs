using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// Override Tester Report Command
	/// </summary>
	public class CmdOverrideTesterReport : MPIDS7600Command
	{
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_OVERRIDE_TESTER_REPORT;

		// Length		// MAX_DATA_LENGTH
		public static Int32 DATA_LEN = Const.MAX_DATA_LENGTH;

		// Position
		public static int OVERRIDE_NAME_POS = 0;

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdOverrideTesterReport()
			: base(COMMAND_ID, DATA_LEN)
		{
		}

		/// <summary>
		/// Override Name
		/// </summary>
		public char[] OverrideName
		{
			get
			{
				return this.GetData(Const.MAX_DATA_LENGTH, OVERRIDE_NAME_POS);
			}
			set
			{
				this.SetData(Const.MAX_DATA_LENGTH, OVERRIDE_NAME_POS, value);
			}
		}
	}
}
