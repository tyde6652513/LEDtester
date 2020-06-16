using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunTestItem : RegCmdBase
    {
        #region >>> Public Properties <<<

        public int TestItemIndex
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        #endregion

        #region >>> Public Method <<<

        public CmdRunTestItem() : base(Convert.ToUInt16(EMeterCmdID.RunTestItem), 1, 0)
        { 
        }

        public CmdRunTestItem(int CmdID) : base(Convert.ToUInt16(CmdID), 1, 0)
        { 
        }

        #endregion
    }
}
