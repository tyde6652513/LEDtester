using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: tstipResult(1)
	/// 
	///	struct tstipResult
	///	{
	///		Int16 Result
	///	}
	/// </summary>
	public class CmdTtst4ipResult : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_RESULT;

		// Data Length
		public const Int32 DATA_LENGTH = 2;

		// Position 
		public const Int32 RESULT_POS = 0;	// 2 Bytes (Int16)

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipResult()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}

		/// <summary>
		/// Result ID
		/// </summary>
		public Int16 Result
		{
			get { return this.GetInt16Data(RESULT_POS); }
			set { this.SetInt16Data(RESULT_POS, value); }
		}
	}
}
