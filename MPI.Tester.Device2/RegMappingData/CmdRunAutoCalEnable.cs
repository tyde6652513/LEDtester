using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunAutoCalEnable : RegCmdBase
    {
        #region >>> Public Properties <<<

        public ERegAutoCalApply AutoCalDataRead
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        #endregion

        #region >>> Public Method <<<

        public CmdRunAutoCalEnable() : base(Convert.ToUInt16(EMeterCmdID.RunAutoCalEnable), 1, 0)
        { 
        }

        public CmdRunAutoCalEnable(int CmdID) : base(Convert.ToUInt16(CmdID), 1, 0)
        { 
        }

        #endregion
    }
}
