using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunMemoryClear : RegCmdBase
    {
        #region >>> Public Properties <<<

        public int ItemIndex
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(0, Convert.ToUInt16(value));
                }
            }
        }
        #endregion

        #region >>> Public Method <<<

        public CmdRunMemoryClear() : base(Convert.ToUInt16(EMeterCmdID.RunMemoryClear), 0, 0)
        {
        }

        public CmdRunMemoryClear(int CmdID) : base(Convert.ToUInt16(CmdID), 0, 0)
        {
        }

        #endregion
    }
}
