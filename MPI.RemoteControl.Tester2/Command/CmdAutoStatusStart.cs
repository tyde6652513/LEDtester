using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
    /// <summary>
    /// CmdAutoStatusStart Command.
    /// </summary>
    public class CmdAutoStatusStart : CmdMPIBased
    {
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

		// CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_ATTOSTATUS_START;

        // Length
        public static Int32 DATA_LEN = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdAutoStatusStart()
            : base(COMMAND_ID, DATA_LEN)
        {

        }
    }
}
