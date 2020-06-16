using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdResetSystem : RegCmdBase
    {
         #region >>> Public Method <<<

        public CmdResetSystem() : base(Convert.ToUInt16(EMeterCmdID.ResetSystem), 0, 0)
        { 
        }

        public CmdResetSystem(int CmdID) : base(Convert.ToUInt16(CmdID), 0, 0)
        { 
        }

        #endregion
    }
}
