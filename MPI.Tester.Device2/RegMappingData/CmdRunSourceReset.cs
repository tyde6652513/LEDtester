using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunSourceReset : RegCmdBase
    {
        #region >>> Public Method <<<

        public CmdRunSourceReset() : base(Convert.ToUInt16(EMeterCmdID.RunSourceReset), 0, 0)
        { 
        }

        public CmdRunSourceReset(int CmdID) : base(Convert.ToUInt16(CmdID), 0, 0)
        { 
        }

        #endregion
    }
}
