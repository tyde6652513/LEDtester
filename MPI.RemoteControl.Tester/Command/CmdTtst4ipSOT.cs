using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipSOT(3)
	/// 
	///	struct Ttst4ipSOT
	///	{
	///	}
	/// </summary>
	public class CmdTtst4ipSOT : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_SOT;

		// Data Length
		public const Int32 DATA_LENGTH = 0;

		// Position 

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipSOT()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}
	}
}
