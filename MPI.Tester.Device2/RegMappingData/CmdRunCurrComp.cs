using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunCurrComp : RegCmdBase
    {
        #region >>> Public Properties <<<

        public int TestItemIndex
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        #endregion

        #region >>> Public Method <<<

        public CmdRunCurrComp() : base(Convert.ToUInt16(EMeterCmdID.RunCurrComp), 1, 0)
        { 
        }

        public CmdRunCurrComp(int CmdID) : base(Convert.ToUInt16(CmdID), 1, 0)
        { 
        }

        #endregion
    }
}
