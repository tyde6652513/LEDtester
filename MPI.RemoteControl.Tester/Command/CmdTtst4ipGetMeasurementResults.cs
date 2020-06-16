using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipGetMeasurementResults(9)
	/// 
	///	struct Ttst4ipGetMeasurementResults
	///	{
	///	}
	/// </summary>
	public class CmdTtst4ipGetMeasurementResults : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_GET_MEASUREMENT_RESULT;

		// Data Length
		public const Int32 DATA_LENGTH = 0;

		// Position 

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipGetMeasurementResults()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}
	}
}
