using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdClearTestItem : RegCmdBase
    {
        #region >>> Public Method <<<

        public CmdClearTestItem() : base(Convert.ToUInt16(EMeterCmdID.ClearTestItem), 0, 0)
        { 
        }

        public CmdClearTestItem(int CmdID) : base(Convert.ToUInt16(CmdID), 0, 0)
        { 
        }

        #endregion
    }
}
