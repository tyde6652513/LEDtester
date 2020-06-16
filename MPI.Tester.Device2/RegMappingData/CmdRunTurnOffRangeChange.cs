using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunTurnOffRangeChange : RegCmdBase
    {
        #region >>> Public Properties <<<

        public int TurnOffRange
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        #endregion

        #region >>> Public Method <<<

        public CmdRunTurnOffRangeChange() : base(Convert.ToUInt16(EMeterCmdID.RunTurnOffRangeChange), 1, 0)
        { 
        }

        public CmdRunTurnOffRangeChange(int CmdID) : base(Convert.ToUInt16(CmdID), 1, 0)
        { 
        }

        #endregion
    }
}
