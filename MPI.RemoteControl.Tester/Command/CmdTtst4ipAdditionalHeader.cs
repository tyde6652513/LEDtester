using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipAdditionalHeader(6)
	/// 
	///	struct Ttst4ipAdditionalHeader
	///	{
	///		string AdditionalHeader			// char[256]
	///	}
	/// </summary>
	public class CmdTtst4ipAdditionalHeader : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_ADDITIONAL_HEADER;

		// Data Length
		public const Int32 DATA_LENGTH = 256;	// AdditionalHeader(256 Bytes)

		// Position 
		public const Int32 ADDITIONAL_HEADER_POS = 0;

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipAdditionalHeader()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}

		/// <summary>
		/// Additional Header
		/// </summary>
		public char[] AdditionalHeader
		{
			get { return this.GetCharData(ADDITIONAL_HEADER_POS, MAX_STRING_LENGTH); }
			set { this.SetData(ADDITIONAL_HEADER_POS, MAX_STRING_LENGTH, value); }
		}
	}
}
