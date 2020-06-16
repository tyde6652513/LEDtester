using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdSetCurrComp : RegCmdBase
    {

		private int _currRangeIndex;

		#region >>> Public Properties <<<

		public ECompareType CompareType
		{ 
			set{ lock (this._lockObj ) { this.SetParamData(0, Convert.ToUInt16(value)); } }					
		}

		public uint PMU
		{
			set { lock (this._lockObj) { this.SetParamData(1, Convert.ToUInt16(value)); } }					
		}

		public int CurrRangeIndex
		{
			set { 
                    lock (this._lockObj) 
                     {
                         this._currRangeIndex = value;
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
                    this.SetParamData(3, TSEConvert.ValueToUInt16(value, (EIRange)this._currRangeIndex));
                    
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

        public CmdSetCurrComp() : base(Convert.ToUInt16(EMeterCmdID.SetCurrComp), 6, 0)
        { 
        }

        public CmdSetCurrComp(int CmdID) : base(Convert.ToUInt16(CmdID), 6, 0)
        { 
        }

        #endregion

    }
}
