using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
    /// <summary>
    /// Mutil Die SOT Command.
    /// 
    /// struct _PBSORTER_SOT_PACKET
    ///	{
    ///		int  Col;   	                        // Col
    ///		int  Row;    	                        // Row
    ///		char ChannelStatus[128];                // Channel State for die
    ///     char SubBin[24];
    ///	}
    /// </summary>
    public class CmdMutiDieSOT : MPIDS7600Command
    {
        public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

        // Const
        public const Int32 MAX_CH_NUM = 128;
        public const Int32 MAX_SUB_BIN_NUM = 24;
		
		// CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_MUTIL_DIE_SOT;

        // Length		// 2 * sizeof(Int32)
        public static Int32 DATA_LEN = 2 * sizeof(Int32) + MAX_CH_NUM + MAX_SUB_BIN_NUM;
        

        // Position
		public static Int32 COL_POS = 0;
		public static Int32 ROW_POS = COL_POS + sizeof(Int32);
		public static Int32 CHANNEL_STATUS_POS = ROW_POS + sizeof(Int32);
        public static Int32 SUB_BIN_POS = CHANNEL_STATUS_POS + MAX_CH_NUM;


        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdMutiDieSOT()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

        /// <summary>
        /// Chip Col
        /// </summary>
        public Int32 Col
        {
            get
            {
                return this.GetInt32Data(COL_POS);
            }
            set
            {
                this.SetInt32Data(COL_POS, value);
            }
        }

        /// <summary>
        /// Chip Row
        /// </summary>
        public Int32 Row
        {
            get
            {
                return this.GetInt32Data(ROW_POS);
            }
            set
            {
                this.SetInt32Data(ROW_POS, value);
            }
        }

        /// <summary>
        /// Channel Status.
        /// </summary>
        public char[] ChannelStatus
        {
            get
            {
                return this.GetData(MAX_CH_NUM, CHANNEL_STATUS_POS);
            }
            set
            {
                this.SetData(MAX_CH_NUM, CHANNEL_STATUS_POS, value);
            }
        }

        /// <summary>
        /// Sub Bin.
        /// </summary>
        public char[] SubBin
        {
            get
            {
                return this.GetData(MAX_SUB_BIN_NUM, SUB_BIN_POS);
            }
            set
            {
                this.SetData(MAX_SUB_BIN_NUM, SUB_BIN_POS, value);
            }
        }
    }
}
