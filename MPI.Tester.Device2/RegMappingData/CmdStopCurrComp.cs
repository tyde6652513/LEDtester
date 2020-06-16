using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdStopCurrComp : RegCmdBase
    {
        #region >>> Public Method <<<

        public CmdStopCurrComp() : base(Convert.ToUInt16(EMeterCmdID.StopCurrComp), 0, 0)
        { 
        }

        public CmdStopCurrComp(int CmdID) : base(Convert.ToUInt16(CmdID), 0, 0)
        { 
        }

        #endregion
    }
}
