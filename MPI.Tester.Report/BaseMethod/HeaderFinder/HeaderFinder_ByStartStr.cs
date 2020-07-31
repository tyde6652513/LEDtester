using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Report.BaseMethod.HeaderFinder
{
    public class HeaderFinder_ByStartStr: HeaderFinderBase
    {
        bool startCount = false;

        public HeaderFinder_ByStartStr(string tarStr, int shift)
            : base(tarStr, shift)
        {
            TarStr = tarStr;
            ShiftRow = shift;
        }

        public override bool CheckIfRowData(string str)
        {
            if (str.StartsWith(TarStr))
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

        public override bool IsFitTarStr(string str)//純粹確認輸入的字串與TarStr
        { return str.StartsWith(TarStr); }
    }
}
