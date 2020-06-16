using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// Barcode Print Command.
	///typedef struct _PBSORTER_BARCODE_PRINT_COMMAND
	///    {
	///           int nOutTubenum;     // OUT_TUBE_NUM
	///           int nOutTubeBinnum; // OUT_TUBE_BIN_NUM
	///           int nOutTubeBinCount;  //OUT_TUBE_COUNT
	///    }PBSORTER_BARCODE_PRINT_COMMAND,*LPBSORTER_BARCODE_PRINT_COMMAND;
	/// </summary>
	public class CmdBarcodePrint : MPIDS7600Command
	{
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
		public const int COMMAND_ID = (int)ETSECommand.ID_BARCODE_PRINT;

		// Length
		public static int DATA_LEN = 5 * sizeof(Int32) + Const.MAX_DATA_LENGTH;

		// Position
		public static int TUBE_NUMBER = 0;
		public static int TUBE_BIN = TUBE_NUMBER + sizeof(Int32);
		public static int TUBE_COUNT = TUBE_BIN + sizeof(Int32);
        public static int TUBE_ID = TUBE_COUNT + sizeof(Int32);
		public static int PART_ID = TUBE_ID + Const.MAX_DATA_LENGTH;
		public static int TUBE_PULLNUMBER = PART_ID + sizeof(Int32);

		public CmdBarcodePrint()
			: base(COMMAND_ID, DATA_LEN)
		{

		}

		/// <summary>
		/// Get Tube Number
		/// </summary>
		public Int32 TubeNumber
		{
			get
			{
				return this.GetInt32Data(TUBE_NUMBER);
			}
			//set
			//{
			//    this.SetInt32Data(TUBE_NUMBER, value);
			//}
		}


		/// <summary>
		/// Get Tube Bin
		/// </summary>
		public Int32 TubeBin
		{
			get
			{
				return this.GetInt32Data(TUBE_BIN);
			}
			//set
			//{
			//    this.SetInt32Data(TUBE_BIN, value);
			//}
		}


		/// <summary>
		/// Get Tube Count
		/// </summary>
		public Int32 TubeCount
		{
			get
			{
				return this.GetInt32Data(TUBE_COUNT);
			}
			//set
			//{
			//    this.SetInt32Data(TUBE_COUNT, value);
			//}
		}


		/// <summary>
		/// OutTube ID Name.
		/// </summary>
		public char[] TubeID
		{
			get
			{
				return this.GetData(Const.MAX_DATA_LENGTH, TUBE_ID);
			}
			set
			{
				this.SetData(Const.MAX_DATA_LENGTH, TUBE_ID, value);
			}
		}

		/// <summary>
		/// Part ID Name.
		/// </summary>
		public char[] PartID
		{
			get
			{
				return this.GetData(Const.MAX_DATA_LENGTH, PART_ID);
			}
			set
			{
				this.SetData(Const.MAX_DATA_LENGTH, PART_ID, value);
			}
		}

		/// <summary>
		/// TUBE PULL NUMBER
		/// </summary>
		public Int32 TubePullNumber
		{
			get
			{
				return this.GetInt32Data(TUBE_PULLNUMBER);
			}
		}
	}
}
