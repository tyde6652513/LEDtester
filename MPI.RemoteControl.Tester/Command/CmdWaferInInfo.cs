using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
    /// <summary>
    /// Wafer In Info Command.
    /// 
    /// typedef struct _PBSORTER_CMD_INFO
    ///	{
    ///		char szInfo[MAX_INFO];					// Wafer In Info
    ///	} 
    /// typedef struct _PBSORTER_WAFER_IN_INFO_COMMAND
    ///	{
    ///		char szLotNo[MAX_ITEM_NAME];                         	// Lot No
    ///		char szWaferNo[MAX_ITEM_NAME];                      	// Wafer No
    ///		char szOperatorName[MAX_ITEM_NAME];                		// Operator Name
	///		int iInfoCount;								            // WaferIn Info Count
	///		PBSORTER_CMD_INFO InfoList[MAX_INFO_LIST];		        // WaferInfoList
    ///	} 
    /// </summary>
    public class CmdWaferInInfo : MPIDS7600Command
    {
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
        public const int COMMAND_ID = (int)ETSECommand.ID_WAFER_IN_INFO;

		// Length		// MAX_ITEM_NAME + MAX_ITEM_NAME + MAX_ITEM_NAME +  MAX_INFO * MAX_INFO_LIST = 2060 bytes
		public static int DATA_LEN = Const.MAX_ITEM_NAME + Const.MAX_ITEM_NAME + Const.MAX_ITEM_NAME + Const.MAX_INFO * Const.MAX_INFO_LIST;
       
        // Position
		public static int LOT_NO_POS = 0;
		public static int WAFER_NO_POS = LOT_NO_POS + Const.MAX_ITEM_NAME;
		public static int OPERATOR_POS = WAFER_NO_POS + Const.MAX_ITEM_NAME;
		public static int WAFER_IN_INFO_LIST_POS = OPERATOR_POS + Const.MAX_ITEM_NAME;

        /// <summary>
        /// Constructor.
        /// </summary>
		public CmdWaferInInfo()
            : base(COMMAND_ID, DATA_LEN)
        {
        }

        /// <summary>
        /// Lot Number.
        /// </summary>
        public char[] LotNo
        {
            get
            {
				return this.GetData(Const.MAX_ITEM_NAME, LOT_NO_POS);
            }
            set
            {
				this.SetData(Const.MAX_ITEM_NAME, LOT_NO_POS, value);
            }
        }

        /// <summary>
        /// Wafer Number.
        /// </summary>
        public char[] WaferNo
        {
            get
            {
				return this.GetData(Const.MAX_ITEM_NAME, WAFER_NO_POS);
            }
            set
            {
				this.SetData(Const.MAX_ITEM_NAME, WAFER_NO_POS, value);
            }
        }

		/// <summary>
		/// OP Name
		/// </summary>
        public char[] OperatorName
        {
            get
            {
				return this.GetData(Const.MAX_ITEM_NAME, OPERATOR_POS);
            }
            set
            {
				this.SetData(Const.MAX_ITEM_NAME, OPERATOR_POS, value);
            }
        }

		/// </summary>
		/// WaferIn Info Count.
		/// </summary>
		//public Int32 InfoCount
		//{
		//    get
		//    {
		//        return this.GetInt32Data(WAFER_IN_INFO_COUNT_POS);
		//    }
		//    set
		//    {
		//        this.SetInt32Data(WAFER_IN_INFO_COUNT_POS, value);
		//    }
		//}

		/// <summary>
		/// Get WaferIn Info.
		/// </summary>
		public char[] GetInfo(int i)
		{
			if (i < 0 || i >= Const.MAX_INFO_LIST) return null;

			return this.GetData(Const.MAX_INFO, WAFER_IN_INFO_LIST_POS + (i * Const.MAX_INFO));
		}

		/// <summary>
		/// Get WaferIn Info.
		/// </summary>
		public string GetInfoString(int i)
		{
			return new string(GetInfo(i));
		}
		
		/// <summary>
		/// Set Info.
		/// </summary>
		public void SetInfo(int i, char[] info)
		{
			if (i < 0 || i >= Const.MAX_INFO_LIST) return;
			if (info.Length > Const.MAX_INFO) return;

			int startPos = WAFER_IN_INFO_LIST_POS + (i * Const.MAX_INFO);
			Array.Clear(this.Data, startPos, Const.MAX_INFO);
			this.SetData(info.Length, startPos, info);
		}

		/// <summary>
		/// Set Info.
		/// </summary>
		public void SetInfoString(int i, string info)
		{
			SetInfo(i, info.ToCharArray());
		}

    }
}
