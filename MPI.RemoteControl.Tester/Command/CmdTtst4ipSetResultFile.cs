using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipSetResultFile(8)
	/// 
	///	struct Ttst4ipSetResultFile
	///	{
	///		string ResultFileName			// char[256]
	///	}
	/// </summary>
	public class CmdTtst4ipSetResultFile : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_SET_RESULT_FILE;

		// Data Length
		public const Int32 DATA_LENGTH = 256;	// AdditionalResults(256 Bytes)

		// Position 
		public const Int32 RESULT_FILE_NAME_POS = 0;

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipSetResultFile()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}

		/// <summary>
		/// Result File Name
		/// </summary>
		public char[] ResultFileName
		{
			get { return this.GetCharData(RESULT_FILE_NAME_POS, MAX_STRING_LENGTH); }
			set { this.SetData(RESULT_FILE_NAME_POS, MAX_STRING_LENGTH, value); }
		}
	}
}
