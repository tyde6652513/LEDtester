using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdSetGainOffset : RegCmdBase
    {
        private int _mode;
		private double _ivRange;
		
		#region >>> Public Properties <<<

		public EPMU PMU
		{ 
			set{ lock (this._lockObj ) { this.SetParamData(0, Convert.ToUInt16(value)); } }					
		}

        public double VoltRange
        {
            set { lock (this._lockObj) { this.SetParamData(1, Convert.ToUInt16(value)); } }
        }

        public double CurrRange
        {
            set { lock (this._lockObj) { this.SetParamData(2, Convert.ToUInt16(value)); } }
        }

		public double GainOffsetSelect
		{
			set { lock (this._lockObj) { this.SetParamData(3, Convert.ToUInt16(value)); } }					
		}

        public double Gain
        {
            set { lock (this._lockObj) { this.SetParamData(4, Convert.ToUInt16(value)); } }
        }

        public double Offset
        {
            set { lock (this._lockObj) { this.SetParamData(5, Convert.ToUInt16(value)); } }
        }

		#endregion

        #region >>> Public Method <<<

        public CmdSetGainOffset() : base(Convert.ToUInt16(EMeterCmdID.SetGainOffset), 2, 0)
        { 
        }

        public CmdSetGainOffset(int CmdID) : base(Convert.ToUInt16(CmdID), 2, 0)
        {
        }

        #endregion
    }
}
