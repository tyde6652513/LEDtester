using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
    /// <summary>
    /// SOT Command.
    /// 
    /// struct _PBSORTER_SOT_PACKET
    ///	{
    ///		BOOL bExistChip;			  // Test Chip exist Judgment
    ///		long lWaferPositionX;   	  // Wafer Position X
    ///		long lWaferPositionY;    	  // Wafer Position Y
    ///		BOOL bAutoSOT;           	  // TRUE: AUTO Test, FALSE: Manual Test 
    ///		BOOL bNewChip;           	  // TRUE: New Chip, FALSE: Re-Test Chip
    ///		int  nProbeIndex;        	  // Probe Index value
    ///		int  nTestingConditionIndex;  // Testing condition index.
    ///		int  nChipIndex;              // Probing chip index.
    ///	}
    /// </summary>
    public class CmdSOT : CmdMPIBased
    {
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;
		
		// CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_SOT;

        // Length
        public static Int32 DATA_LEN = 8 * sizeof(Int32);

        // Position
		public static Int32 EXIST_CHIP_POS = 0;
		public static Int32 WAFER_POSITION_X_POS = EXIST_CHIP_POS + sizeof(Int32);
		public static Int32 WAFER_POSITION_Y_POS = WAFER_POSITION_X_POS + sizeof(Int32);
		public static Int32 AUTO_SOT_POS = WAFER_POSITION_Y_POS + sizeof(Int32);
		public static Int32 NEW_CHIP_POS = AUTO_SOT_POS + sizeof(Int32);
		public static Int32 PROBE_INDEX_POS = NEW_CHIP_POS + sizeof(Int32);
		public static Int32 TESTING_CONDITION_INDEX_POS = PROBE_INDEX_POS + sizeof(Int32);
        public static Int32 PROBING_CHIP_INDEX_POS = TESTING_CONDITION_INDEX_POS + sizeof(Int32);

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdSOT()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

        /// <summary>
        /// Exist Chip.
        /// </summary>
        public Boolean ExistChip
        {
            get
            {
                return this.GetBoolData(EXIST_CHIP_POS);
            }
            set
            {
                this.SetBoolData(EXIST_CHIP_POS, value);
            }
        }

        /// <summary>
        /// Wafer Position X.
        /// </summary>
        public Int32 WaferPositionX
        {
            get
            {
                return this.GetInt32Data(WAFER_POSITION_X_POS);
            }
            set
            {
                this.SetInt32Data(WAFER_POSITION_X_POS, value);
            }
        }

        /// <summary>
        /// Wafer Position Y.
        /// </summary>
        public Int32 WaferPositionY
        {
            get
            {
                return this.GetInt32Data(WAFER_POSITION_Y_POS);
            }
            set
            {
                this.SetInt32Data(WAFER_POSITION_Y_POS, value);
            }
        }

        /// <summary>
        /// Auto SOT.
        /// </summary>
        public Boolean AutoSOT
        {
            get
            {
                return this.GetBoolData(AUTO_SOT_POS);
            }
            set
            {
                this.SetBoolData(AUTO_SOT_POS, value);
            }
        }

        /// <summary>
        /// New Chip.
        /// </summary>
        public Boolean NewChip
        {
            get
            {
                return this.GetBoolData(NEW_CHIP_POS);
            }
            set
            {
                this.SetBoolData(NEW_CHIP_POS, value);
            }
        }

        /// <summary>
        /// Probe Index.
        /// </summary>
        public Int32 ProbeIndex
        {
            get
            {
                return this.GetInt32Data(PROBE_INDEX_POS);
            }
            set
            {
                this.SetInt32Data(PROBE_INDEX_POS, value);
            }
        }

        /// <summary>
        /// Testing condition index
        /// </summary>
        public Int32 TestingConditionIndex
        {
            get { return this.GetInt32Data(TESTING_CONDITION_INDEX_POS); }
            set { this.SetInt32Data(TESTING_CONDITION_INDEX_POS, value); }
        }

        /// <summary>
        /// Probing chip index.
        /// </summary>
        public Int32 ProbingChipIndex
        {
            get { return this.GetInt32Data(PROBING_CHIP_INDEX_POS); }
            set { this.SetInt32Data(PROBING_CHIP_INDEX_POS, value); }
        }
    }
}
