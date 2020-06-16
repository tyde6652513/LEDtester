using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
    /// <summary>
    /// Auto Contact Command.
    /// </summary>
    public class CmdAutoContact : MPIDS7600Command
    {
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_AUTOCONTACT;

        // Length
        public static Int32 DATA_LEN = 4;

        // Position
        public static Int32 CONTACT_POS = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdAutoContact()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

        /// <summary>
        /// Contact.
        /// </summary>
        public Int32 Contact
        {
            get
            {
                return this.GetInt32Data(CONTACT_POS);
            }
            set
            {
                this.SetInt32Data(CONTACT_POS, value);
            }
        }
    }
}
