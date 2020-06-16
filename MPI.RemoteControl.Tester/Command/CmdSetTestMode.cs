using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{

    public class CmdSetTestMode : MPIDS7600Command
    {
        public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

        // CommandID.
        public const Int32 COMMAND_ID = (Int32)ETSECommand.ID_SET_TEST_MODE;//60029
        public static Int32 DATA_LEN = Const.MAX_DATA_LENGTH * 5;

        public enum ETestMode
        {
            UNDEFINED = -1,
            NORMAL = 0,
            NG_RETEST =  1,
            OVERWRITE = 2, // NG_OVERWRITE -> OVERWRITE
            CONTINOUS = 3,
            NG_SKIP = 4,  
        }

        // Position
        public static Int32 TEST_MODE_POS = 0;

        public CmdSetTestMode()
            : base(COMMAND_ID, DATA_LEN)
        {
        }

        public Int32 TesterTestMode
        {
            get
            {
                return this.GetInt32Data(TEST_MODE_POS);
            }
            set
            {
                this.SetInt32Data(TEST_MODE_POS, value);
            }
        }

        public ETestMode TestMode
        {
            get
            {
                ETestMode code = ETestMode.UNDEFINED;

                return Enum.TryParse<ETestMode>(TesterTestMode.ToString(), out code) ? code : ETestMode.UNDEFINED;
            }
            set
            {
                this.TesterTestMode = value.GetHashCode();
            }
        }
    }
}
