using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipSOJ(2)
	/// 
	///	struct Ttst4ipSOJ
	///	{
	///		Int32 StartStopJob
	///	}
	/// </summary>
	public class CmdTtst4ipSOJ : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_SOJ;

		// Data Length
		public const Int32 DATA_LENGTH = 4;

		// Position 
		public const Int32 START_STOP_JOB_POS = 0;	// 4 Bytes (Int32)

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipSOJ()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}

		/// <summary>
		/// Get Job Status, Start:1, Stop:0
		/// </summary>
		private Int32 StartStopJob
		{
			get { return this.GetInt32Data(START_STOP_JOB_POS); }
			set { this.SetInt32Data(START_STOP_JOB_POS, value); }
		}

		/// <summary>
		/// Start or Stop a Job
		/// </summary>
		/// <param name="bEnable"></param>
		public void EnableJob(bool bEnable)
		{
			int nSetValue = bEnable ? 1 : 0;

			this.SetInt32Data(START_STOP_JOB_POS, nSetValue);
		}
	}
}
