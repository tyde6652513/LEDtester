using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
	public class CmdSetVoltComp : RegCmdBase
	{
        private int _voltRangeIndex;		
		
		#region >>> Public Properties <<<

        public ECompareType CompareType
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        public uint PMU
        {
            set { lock (this._lockObj) { this.SetParamData(1, Convert.ToUInt16(value)); } }
        }

        public int VoltRangeIndex
        {
            set
            {
                lock (this._lockObj)
                {
                    this._voltRangeIndex = value;
                    this.SetParamData(2, Convert.ToUInt16(value));
                }
            }
        }

        public double CompareValue
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(3, TSEConvert.ValueToUInt16(value, (EVRange)this._voltRangeIndex));
                }
            }
        }

        public double TimeOut
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(4, 0);
                    this.SetParamData(5, Convert.ToUInt16(value));
                }
            }
        }

		#endregion

        #region >>> Public Method <<<

        public CmdSetVoltComp() : base(Convert.ToUInt16(EMeterCmdID.SetVoltComp), 6, 0)
        { 
        }

        public CmdSetVoltComp(int CmdID) : base(Convert.ToUInt16(CmdID), 6, 0)
        { 
        }

        #endregion

    }
}
