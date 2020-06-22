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
    ///		int nProbeIndex;        	  // Probe Index value
    ///		int nTestingConditionIndex;  // Testing condition index.
    ///		int nChipIndex;              // Probing chip index.
	///		int PZPosition;
	///		int DZPosition;
	///		int M2ZPosition;
	///		int NeedleForce1;
	///		int NeedleForce2;
	///		int NeedleForce3;
	///		int NeedleForce4;
	///		int DXIndex;
	///		int DYIndex;
	///		int DXPosition;
	///		int DYPosition;
	///		int SubDieIndex;
	///		int SubDiePositionX;
	///		int SubDiePositionY;
	///		int GroupPositionX;
	///		int GroupPositionY;
	///		int TestingTemperature;
    ///	}
    /// </summary>
    public class CmdSOT1 : CmdMPIBased
    {
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;
		
		// CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_SOT;

        // Length
        public static Int32 DATA_LEN = 25 * sizeof(Int32); // 2019.05.14 saber add

        // Position
		public static Int32 EXIST_CHIP_POS = 0;
		public static Int32 WAFER_POSITION_X_POS = EXIST_CHIP_POS + sizeof(Int32);
		public static Int32 WAFER_POSITION_Y_POS = WAFER_POSITION_X_POS + sizeof(Int32);
		public static Int32 AUTO_SOT_POS = WAFER_POSITION_Y_POS + sizeof(Int32);
		public static Int32 NEW_CHIP_POS = AUTO_SOT_POS + sizeof(Int32);
		public static Int32 PROBE_INDEX_POS = NEW_CHIP_POS + sizeof(Int32);

		public static Int32 TESTING_CONDITION_INDEX_POS = PROBE_INDEX_POS + sizeof(Int32);
        public static Int32 PROBING_CHIP_INDEX_POS = TESTING_CONDITION_INDEX_POS + sizeof(Int32);
        //
        // 2018.11.25 saber add
        public static Int32 PZ_POSITION = PROBING_CHIP_INDEX_POS + sizeof(Int32);
        public static Int32 DZ_POSITION = PZ_POSITION + sizeof(Int32);
        public static Int32 M2Z_POSITION = DZ_POSITION + sizeof(Int32);
        public static Int32 NEEDLE_FORCE_1 = M2Z_POSITION + sizeof(Int32);
        public static Int32 NEEDLE_FORCE_2 = NEEDLE_FORCE_1 + sizeof(Int32);
        public static Int32 NEEDLE_FORCE_3 = NEEDLE_FORCE_2 + sizeof(Int32);
        public static Int32 NEEDLE_FORCE_4 = NEEDLE_FORCE_3 + sizeof(Int32);
        // 2019.05.14 saber add
        public static Int32 DX_INDEX = NEEDLE_FORCE_4 + sizeof(Int32);
        public static Int32 DY_INDEX = DX_INDEX + sizeof(Int32);
        public static Int32 DX_POSITION = DY_INDEX + sizeof(Int32);
        public static Int32 DY_POSITION = DX_POSITION + sizeof(Int32);
        // 2019.06.19 saber add
		public static Int32 SUBDIE_POSITION_INDEX = DY_POSITION + sizeof(Int32);
		public static Int32 SUBDIE_POSITION_X_POS = SUBDIE_POSITION_INDEX + sizeof(Int32);
        public static Int32 SUBDIE_POSITION_Y_POS = SUBDIE_POSITION_X_POS + sizeof(Int32);
        public static Int32 GROUP_POSITION_X_POS = SUBDIE_POSITION_Y_POS + sizeof(Int32);
        public static Int32 GROUP_POSITION_Y_POS = GROUP_POSITION_X_POS + sizeof(Int32);
        public static Int32 TESTING_TEMPERATURE = GROUP_POSITION_Y_POS + sizeof(Int32);


        /// <summary>
        /// Constructor.
        /// </summary>
		public CmdSOT1()
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

        /// <summary>
        ///PZ Position // 2018.11.25 saber add
        /// </summary>
        public Int32 PZPosition
        {
            get { return this.GetInt32Data(PZ_POSITION); }
            set { this.SetInt32Data(PZ_POSITION, value); }
        }

        /// <summary>
        ///DZ Position // 2018.11.25 saber add
        /// </summary>
        public Int32 DZPosition
        {
            get { return this.GetInt32Data(DZ_POSITION); }
            set { this.SetInt32Data(DZ_POSITION, value); }
        }

        /// <summary>
        ///M2Z Position // 2018.11.25 saber add
        /// </summary>
        public Int32 M2ZPosition
        {
            get { return this.GetInt32Data(M2Z_POSITION); }
            set { this.SetInt32Data(M2Z_POSITION, value); }
        }

        /// <summary>
        ///Needle Force1 // 2018.11.25 saber add
        /// </summary>
        public Int32 NeedleForce1
        {
            get { return this.GetInt32Data(NEEDLE_FORCE_1); }
            set { this.SetInt32Data(NEEDLE_FORCE_1, value); }
        }

        /// <summary>
        ///Needle Force2 // 2018.11.25 saber add
        /// </summary>
        public Int32 NeedleForce2
        {
            get { return this.GetInt32Data(NEEDLE_FORCE_2); }
            set { this.SetInt32Data(NEEDLE_FORCE_2, value); }
        }

        /// <summary>
        ///Needle Force3 // 2018.11.25 saber add
        /// </summary>
        public Int32 NeedleForce3
        {
            get { return this.GetInt32Data(NEEDLE_FORCE_3); }
            set { this.SetInt32Data(NEEDLE_FORCE_3, value); }
        }

        /// <summary>
        ///Needle Force4 // 2018.11.25 saber add
        /// </summary>
        public Int32 NeedleForce4
        {
            get { return this.GetInt32Data(NEEDLE_FORCE_4); }
            set { this.SetInt32Data(NEEDLE_FORCE_4, value); }
        }

        /// <summary>
        ///DX Index
        /// </summary>
        public Int32 DXIndex // 2019.05.14 saber add
        {
            get { return this.GetInt32Data(DX_INDEX); }
            set { this.SetInt32Data(DX_INDEX, value); }
        }

        /// <summary>
        ///DY Index
        /// </summary>
        public Int32 DYIndex // 2019.05.14 saber add
        {
            get { return this.GetInt32Data(DY_INDEX); }
            set { this.SetInt32Data(DY_INDEX, value); }
        }

        /// <summary>
        ///DX Position
        /// </summary>
        public Int32 DXPosition // 2019.05.14 saber add
        {
            get { return this.GetInt32Data(DX_POSITION); }
            set { this.SetInt32Data(DX_POSITION, value); }
        }

        /// <summary>
        ///DY Position
        /// </summary>
        public Int32 DYPosition // 2019.05.14 saber add
        {
            get { return this.GetInt32Data(DY_POSITION); }
            set { this.SetInt32Data(DY_POSITION, value); }
        }

		/// <summary>
		///Probing Pad index.
		/// </summary>
		public Int32 SubDieIndex // 2019.06.19 saber add
		{
			get { return this.GetInt32Data(SUBDIE_POSITION_INDEX); }
			set { this.SetInt32Data(SUBDIE_POSITION_INDEX, value); }
		}

        /// <summary>
        ///Chip Position X
        /// </summary>
        public Int32 SubDiePositionX // 2019.06.19 saber add
        {
            get { return this.GetInt32Data(SUBDIE_POSITION_X_POS); }
            set { this.SetInt32Data(SUBDIE_POSITION_X_POS, value); }
        }

        /// <summary>
        ///Chip Position Y
        /// </summary>
        public Int32 SubDiePositionY // 2019.06.19 saber add
        {
            get { return this.GetInt32Data(SUBDIE_POSITION_Y_POS); }
            set { this.SetInt32Data(SUBDIE_POSITION_Y_POS, value); }
        }

        /// <summary>
        ///Group Position X
        /// </summary>
        public Int32 GroupPositionX // 2019.06.19 saber add
        {
            get { return this.GetInt32Data(GROUP_POSITION_X_POS); }
            set { this.SetInt32Data(GROUP_POSITION_X_POS, value); }
        }

        /// <summary>
        ///Group Position Y
        /// </summary>
        public Int32 GroupPositionY // 2019.06.19 saber add
        {
            get { return this.GetInt32Data(GROUP_POSITION_Y_POS); }
            set { this.SetInt32Data(GROUP_POSITION_Y_POS, value); }
        }

        /// <summary>
        ///Testing Temperature
        /// </summary>
        public Int32 TestingTemperature // 2019.06.19 saber add
        {
            get { return this.GetInt32Data(TESTING_TEMPERATURE); }
            set { this.SetInt32Data(TESTING_TEMPERATURE, value); }
        }
    }
}
