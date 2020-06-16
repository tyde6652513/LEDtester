using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdRunWait : RegCmdBase
    {
        #region >>> Public Properties <<<

        public double TimeOut
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(0, (ushort)TSEConvert.TimeMSB(value));
                    this.SetParamData(1, (ushort)TSEConvert.TimeLSB(value));
                }
            }
        }
        #endregion

        #region >>> Public Method <<<

        public CmdRunWait() : base(Convert.ToUInt16(EMeterCmdID.RunWait), 2, 0)
        {
        }

        public CmdRunWait(int CmdID) : base(Convert.ToUInt16(CmdID), 2, 0)
        {
        }

        #endregion
    }
      

      
}
