using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdSetEEPROMSave : RegCmdBase
    {
       #region >>> Public Properties <<<

        public int Address
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        public int Data
        {
            set { lock (this._lockObj) { this.SetParamData(1, Convert.ToUInt16(value)); } }
        }

        #endregion

        #region >>> Public Method <<<

        public CmdSetEEPROMSave() : base(Convert.ToUInt16(EMeterCmdID.SetEEPROMSave), 2, 0)
        { 
        }

        public CmdSetEEPROMSave(int CmdID) : base(Convert.ToUInt16(CmdID), 2, 0)
        { 
        }

        #endregion
    }
}
