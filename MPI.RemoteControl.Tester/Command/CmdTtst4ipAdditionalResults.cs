using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipAdditionalResults(7)
	/// 
	///	struct Ttst4ipAdditionalResults
	///	{
	///		string AdditionalResults			// char[256]
	///	}
	/// </summary>
	public class CmdTtst4ipAdditionalResults : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_ADDITIONAL_RESULT;

		// Data Length
		public const Int32 DATA_LENGTH = 256;	// AdditionalResults(256 Bytes)

		// Position 
		public const Int32 ADDITIONAL_RESULTS_POS = 0;

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipAdditionalResults()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}

		/// <summary>
		/// Additional Results
		/// </summary>
		public char[] AdditionalResults
		{
			get { return this.GetCharData(ADDITIONAL_RESULTS_POS, MAX_STRING_LENGTH); }
			set { this.SetData(ADDITIONAL_RESULTS_POS, MAX_STRING_LENGTH, value); }
		}
	}
}
