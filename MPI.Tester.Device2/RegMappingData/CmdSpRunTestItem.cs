using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdSpRunTestItem : RegCmdBase
    {
        private ushort _testItemIdex;

        #region >>> Public Properties <<<

        public int TestItemIndex
        {
            set 
            { 
                lock (this._lockObj) 
                {
                    
                    this._testItemIdex = (ushort)(((ushort)EMeterCmdID.SpRunTestItem & 0xFF00) | (ushort)value & 0x00FF);
                    base.CmdID = this._testItemIdex;
                } 
            }
        }

        #endregion
        
        #region >>> Public Method <<<

        public CmdSpRunTestItem()
        {            
        }

        public CmdSpRunTestItem(int CmdID) : base(Convert.ToUInt16(CmdID), 0, 0)
        { 
        }

        #endregion
    }
}
