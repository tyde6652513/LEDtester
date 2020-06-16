using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunTriggerIn1 : RegCmdBase
    {
        #region >>> Public Properties <<<

        public int PinIndex
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        #endregion

        #region >>> Public Method <<<

        public CmdRunTriggerIn1() : base(Convert.ToUInt16(EMeterCmdID.RunTriggerIn_1), 1, 0)
        { 
        }

        public CmdRunTriggerIn1(int CmdID) : base(Convert.ToUInt16(CmdID), 1, 0)
        { 
        }

        #endregion
    }
}
