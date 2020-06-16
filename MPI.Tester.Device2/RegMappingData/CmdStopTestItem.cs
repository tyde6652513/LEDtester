using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdStopTestItem : RegCmdBase
    {
        #region >>> Public Method <<<

        public CmdStopTestItem() : base(Convert.ToUInt16(EMeterCmdID.StopTestItem))
        { 
        }

        public CmdStopTestItem(int CmdID) : base(Convert.ToUInt16(CmdID))
        { 
        }

        #endregion
    }
}
