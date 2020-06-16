using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunTriggerIn : RegCmdBase
    {
        #region >>> Public Properties <<<

        public int PinIndex
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        public ETriggerInLatchType LatchType
        {
            set { lock (this._lockObj) { this.SetParamData(1, Convert.ToUInt16(value)); } }
        }

        #endregion

        #region >>> Public Method <<<

        public CmdRunTriggerIn() : base(Convert.ToUInt16(EMeterCmdID.RunTriggerIn), 2, 0)
        { 
        }

        public CmdRunTriggerIn(int CmdID) : base(Convert.ToUInt16(CmdID), 2, 0)
        { 
        }

        #endregion
    }
}
