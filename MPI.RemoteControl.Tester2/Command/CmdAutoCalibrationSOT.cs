using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
    /// <summary>
    /// Auto Calibration SOT. 
    /// </summary>
    public class CmdAutoCalibrationSOT : CmdMPIBased
    {
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

		// CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_AUTOCAL_SOT;

        // Length		// sizeof(Int32)
		public static Int32 DATA_LEN = sizeof(Int32);

        // Position
        public static Int32 PROBER_POS = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdAutoCalibrationSOT()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

        /// <summary>
        /// Lot Number.
        /// </summary>
        public int ProberIndex
        {
            get
            {
                return this.GetInt32Data(PROBER_POS);
            }
            set
            {
                this.SetInt32Data(PROBER_POS, value);
            }
        }
    }
}
