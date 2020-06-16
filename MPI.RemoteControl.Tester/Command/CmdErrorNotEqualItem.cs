using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
    /// <summary>
    /// Error : Not Equal Item.
    /// </summary>
    public class CmdErrorNotEqualItem : MPIDS7600Command
    {
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_ERROR_NOT_EQUAL_ITEM;

        // Length
        public static Int32 DATA_LEN = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdErrorNotEqualItem()
            : base(COMMAND_ID, DATA_LEN)
        {

        }
    }
}
