using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdSetTriggerOutDuration : RegCmdBase
    {
		#region >>> Public Properties <<<

        public ETrigSetting TriggerOutIndex
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        public double PeriodTime
		{
            set 
            { 
                lock (this._lockObj)
                {
                    this.SetParamData(1, (ushort)TSEConvert.TimeMSB(value));
                    this.SetParamData(2, (ushort)TSEConvert.TimeLSB(value)); 
                } 
            }	
		}

		#endregion

        #region >>> Public Method <<<

        public CmdSetTriggerOutDuration() : base(Convert.ToUInt16(EMeterCmdID.SetTriggerOutDuration), 3, 0)
        { 
        }

        public CmdSetTriggerOutDuration(int CmdID) : base(Convert.ToUInt16(CmdID), 3, 0)
        {
        }

        #endregion
    }
}
