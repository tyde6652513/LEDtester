using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunDioOutput : RegCmdBase
    {
        #region >>> Public Properties <<<

        public int PinIndex
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        #endregion

        #region >>> Public Method <<<

        public CmdRunDioOutput() : base(Convert.ToUInt16(EMeterCmdID.RunDioOutput), 1, 0)
        { 
        }

        public CmdRunDioOutput(int CmdID) : base(Convert.ToUInt16(CmdID), 1, 0)
        { 
        }

        #endregion
    }
}
