using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipEOTResults(4)
	/// 
	///	struct Ttst4ipEOTResults
	///	{
	///		Int16 Result
	///		Int32 Bin
	///	}
	/// </summary>
	public class CmdTtst4ipEOTResults : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_EOT;

		// Data Length
		public const Int32 DATA_LENGTH = 8;

		// Position 
		public const Int32 RESULT_POS = 0;	// 4 bytes
		public const Int32 BIN_POS = RESULT_POS + sizeof(Int32);	// 4 bytes

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipEOTResults()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}

		/// <summary>
		/// Bin Number
		/// </summary>
        public Int32 Bin
        {
            get { return this.GetInt32Data(BIN_POS); }
            set { this.SetInt32Data(BIN_POS, value); }
        }

		/// <summary>
		/// EOT Result Code
		/// </summary>
		public Int32 ResultCode
		{
			get { return this.GetInt32Data(RESULT_POS); }
			set { this.SetInt32Data(RESULT_POS, value); }
		}
	}
}
