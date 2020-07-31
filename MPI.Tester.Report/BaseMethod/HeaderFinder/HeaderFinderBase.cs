using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Report.BaseMethod.HeaderFinder
{
    public class HeaderFinderBase:ICloneable
    {
        public string TarStr = "";
        public int ShiftRow = 0;
        bool startCount = false;

        public HeaderFinderBase(string tarStr, int shift)
        {
            TarStr = tarStr;
            ShiftRow = shift;
        }

        public virtual bool CheckIfRowData(string str)//表頭使用這個來確認，包含ShiftRow功能
        {
            if (str == TarStr)
            {
                startCount = true;
            }

            if (startCount)
            {
                ShiftRow--;
                if (ShiftRow <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsFitTarStr(string str)//純粹確認輸入的字串與TarStr
        { return str == TarStr; }

        public object Clone()
        {
            HeaderFinderBase obj = new HeaderFinderBase(TarStr, ShiftRow);
            return obj;

        }
    }
}
