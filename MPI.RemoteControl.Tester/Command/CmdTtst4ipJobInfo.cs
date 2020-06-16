using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipJobInfo(5)
	/// 
	///	struct Ttst4ipJobInfo
	///	{
	///		string Operator					// char[256]
	///		string JobFileName			// char[256]
	///		string TestID					// char[256]
	///		string TestComment			// char[256]
	///		string CustomResultPath		// char[256], End with a backslash(\).
	///		string CustomSpectraPath	// char[256], End with a backslash(\).
	///	}
	/// </summary>
	public class CmdTtst4ipJobInfo : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_JOB_INFO;

		// Data Length
		public const Int32 DATA_LENGTH = 1536;	// Operator(256) + JobFileName(256) + TestID(256) + TestComment(256) + CustomResultPath(256) + CustomSpectraPath(256) = 1536 Bytes

		// Position 
		public const Int32 OPERATOR_POS = 0;
		public const Int32 JOB_FILE_NAME_POS = OPERATOR_POS + MAX_STRING_LENGTH;
		public const Int32 TEST_ID_POS = JOB_FILE_NAME_POS + MAX_STRING_LENGTH;
		public const Int32 TEST_COMMENT_POS = TEST_ID_POS + MAX_STRING_LENGTH;
		public const Int32 CUSTOM_RESULT_PATH_POS = TEST_COMMENT_POS + MAX_STRING_LENGTH;
		public const Int32 CUSTOM_SPECTRA_PATH_POS = CUSTOM_RESULT_PATH_POS + MAX_STRING_LENGTH;

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipJobInfo()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}

		/// <summary>
		/// Operator Name
		/// </summary>
		public char[] Operator
		{
			get { return this.GetCharData(OPERATOR_POS, MAX_STRING_LENGTH); }
			set { this.SetData(OPERATOR_POS, MAX_STRING_LENGTH, value); }
		}

		/// <summary>
		/// Job File Name
		/// </summary>
		public char[] JobFileName
		{
			get { return this.GetCharData(JOB_FILE_NAME_POS, MAX_STRING_LENGTH); }
			set { this.SetData(JOB_FILE_NAME_POS, MAX_STRING_LENGTH, value); }
		}

		/// <summary>
		/// Test ID
		/// </summary>
		public char[] TestID
		{
			get { return this.GetCharData(TEST_ID_POS, MAX_STRING_LENGTH); }
			set { this.SetData(TEST_ID_POS, MAX_STRING_LENGTH, value); }
		}

		/// <summary>
		/// Test Comment
		/// </summary>
		public char[] TestComment
		{
			get { return this.GetCharData(TEST_COMMENT_POS, MAX_STRING_LENGTH); }
			set { this.SetData(TEST_COMMENT_POS, MAX_STRING_LENGTH, value); }
		}

		/// <summary>
		/// Custom Result Path
		/// </summary>
		public char[] CustomResultPath
		{
			get { return this.GetCharData(CUSTOM_RESULT_PATH_POS, MAX_STRING_LENGTH); }
			set { this.SetData(CUSTOM_RESULT_PATH_POS, MAX_STRING_LENGTH, value); }
		}

		/// <summary>
		/// Custom Spectra Path
		/// </summary>
		public char[] CustomSpectraPath
		{
			get { return this.GetCharData(CUSTOM_SPECTRA_PATH_POS, MAX_STRING_LENGTH); }
			set { this.SetData(CUSTOM_SPECTRA_PATH_POS, MAX_STRING_LENGTH, value); }
		}
	}

}
