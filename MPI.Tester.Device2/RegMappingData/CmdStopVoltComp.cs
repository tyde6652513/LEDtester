using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdStopVoltComp : RegCmdBase
    {
        #region >>> Public Method <<<

        public CmdStopVoltComp() : base(Convert.ToUInt16(EMeterCmdID.StopVoltComp), 0, 0)
        { 
        }

        public CmdStopVoltComp(int CmdID) : base(Convert.ToUInt16(CmdID), 0, 0)
        { 
        }
        #endregion
    }
}
