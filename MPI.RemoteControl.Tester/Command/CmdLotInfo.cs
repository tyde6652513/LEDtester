using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
	public class CmdLotInfo : MPIDS7600Command
	{
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_LOT_INFO;

		// Length		// BinningSequence(20)+BinNameCount(4)+WaferCount(4)+BinNameList(4000)+WaferIDList(4000) = 8028
		public static Int32 DATA_LEN = Const.MAX_DATA + sizeof(Int32) + sizeof(Int32) + Const.MAX_DATA_LENGTH * Const.MAX_LIST_COUNT + Const.MAX_DATA_LENGTH * Const.MAX_LIST_COUNT;

		// Position
		public static int BINNING_SEQUENCE_POS = 0;
		public static int BIN_NAME_COUNT_POS = BINNING_SEQUENCE_POS + Const.MAX_DATA_LENGTH;
		public static int WAFER_COUNT_POS = BIN_NAME_COUNT_POS + sizeof(Int32);
		public static int BIN_NAME_LIST_POS = WAFER_COUNT_POS + sizeof(Int32);
		public static int WAFER_ID_LIST_POS = BIN_NAME_LIST_POS + Const.MAX_DATA_LENGTH * Const.MAX_LIST_COUNT;

        /// <summary>
        /// Constructor.
        /// </summary>
		public CmdLotInfo()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

		/// <summary>
		/// Binning Sequence
		/// </summary>
		public char[] BinningSequence
		{
			get
			{
				return this.GetData(Const.MAX_DATA_LENGTH, BINNING_SEQUENCE_POS);
			}
			set
			{
				this.SetData(Const.MAX_DATA_LENGTH, BINNING_SEQUENCE_POS, value);
			}
		}

		/// <summary>
		/// Bin Name Count
		/// </summary>
		public Int32 BinNameCount
		{
			get
			{
				return this.GetInt32Data(BIN_NAME_COUNT_POS);
			}
			set
			{
				this.SetInt32Data(BIN_NAME_COUNT_POS, value);
			}
		}

		/// <summary>
		/// Wafer Count
		/// </summary>
		public Int32 WaferCount
		{
			get
			{
				return this.GetInt32Data(WAFER_COUNT_POS);
			}
			set
			{
				this.SetInt32Data(WAFER_COUNT_POS, value);
			}
		}

		/// <summary>
		/// Get Bin Name.
		/// </summary>
		public char[] GetBinName(int index)
		{
			if (index < 0 || index >= Const.MAX_LIST_COUNT) return null;

			return this.GetData(Const.MAX_DATA_LENGTH, BIN_NAME_LIST_POS + (index * Const.MAX_DATA_LENGTH));
		}

		/// <summary>
		/// Get Bin Name String.
		/// </summary>
		public string GetBinNameString(int index)
		{
			return new string(GetBinName(index));
		}

		/// <summary>
		/// Set Bin Name.
		/// </summary>
		public void SetBinName(int i, char[] binName)
		{
			if (i < 0 || i >= Const.MAX_LIST_COUNT) return;
			if (binName.Length > Const.MAX_DATA_LENGTH) return;

			int startPos = BIN_NAME_LIST_POS + (i * Const.MAX_DATA_LENGTH);
			Array.Clear(this.Data, startPos, Const.MAX_DATA_LENGTH);
			this.SetData(binName.Length, startPos, binName);
		}

		/// <summary>
		/// Set Bin Name String.
		/// </summary>
		public void SetBinName(int i, string binName)
		{
			SetBinName(i, binName.ToCharArray());
		}

		/// <summary>
		/// Get Wafer ID.
		/// </summary>
		public char[] GetWaferID(int index)
		{
			if (index < 0 || index >= Const.MAX_LIST_COUNT) return null;

			return this.GetData(Const.MAX_DATA_LENGTH, WAFER_ID_LIST_POS + (index * Const.MAX_DATA_LENGTH));
		}

		/// <summary>
		/// Get Wafer ID String.
		/// </summary>
		public string GetWaferIDString(int index)
		{
			return new string(GetWaferID(index));
		}

		/// <summary>
		/// Set Wafer ID.
		/// </summary>
		public void SetWaferID(int i, char[] waferID)
		{
			if (i < 0 || i >= Const.MAX_LIST_COUNT) return;
			if (waferID.Length > Const.MAX_DATA_LENGTH) return;

			int startPos = WAFER_ID_LIST_POS + (i * Const.MAX_DATA_LENGTH);
			Array.Clear(this.Data, startPos, Const.MAX_DATA_LENGTH);
			this.SetData(waferID.Length, startPos, waferID);
		}

		/// <summary>
		/// Set Wafer ID String.
		/// </summary>
		public void SetWaferID(int i, string waferID)
		{
			SetWaferID(i, waferID.ToCharArray());
		}
	}
}
