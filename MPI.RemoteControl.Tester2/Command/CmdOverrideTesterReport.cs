using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
    /// <summary>
    /// Override Tester Report Command
    /// 
    /// typedef struct _PBSORTER_OVERRIDE_REPORT_COMMAND
    ///	{
    ///		char szOverrideName[MAX_DATA_LENGTH];      // Override name
    ///		int nOverrideType                          // EOverrideType - Not test yet in P10T case.
    ///	} 
    public class CmdOverrideTesterReport : CmdMPIBased
	{
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

		// CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_OVERRIDE_TESTER_REPORT;

		// Length		// MAX_DATA_LENGTH + 1 * sizeof(Int32)
		public static Int32 DATA_LEN = Const.MAX_DATA_LENGTH + sizeof(Int32);

		// Position
		public static int OVERRIDE_NAME_POS = 0;
        public static int OVERRIDE_TYPE_POS = OVERRIDE_NAME_POS + Const.MAX_DATA_LENGTH;

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdOverrideTesterReport()
			: base(COMMAND_ID, DATA_LEN)
		{
		}

		/// <summary>
		/// Override Name
		/// </summary>
		public char[] OverrideName
		{
			get
			{
				return this.GetData(Const.MAX_DATA_LENGTH, OVERRIDE_NAME_POS);
			}
			set
			{
				this.SetData(Const.MAX_DATA_LENGTH, OVERRIDE_NAME_POS, value);
			}
		}

        public EOverrideType OveriddeType
        {
            get
            {
                int nValue =  GetInt32Data(OVERRIDE_TYPE_POS);
                EOverrideType overrideType;
                return (Enum.TryParse<EOverrideType>(nValue.ToString(), out overrideType)) ? overrideType : EOverrideType.Overwrite;
            }

            set
            {
                SetInt32Data(OVERRIDE_TYPE_POS, value.GetHashCode());
            }
        }

        public enum EOverrideType
        {
            Overwrite = 0,
            Append = 1,
        }
	}
}
