using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Report.BaseMethod.HeaderFinder
{
    public class HeaderFinder_ByEndStr : HeaderFinder
    {
        bool startCount = false;

        public HeaderFinder_ByEndStr(string tarStr, int shift)
            : base(tarStr, shift)
        {
            TarStr = tarStr;
            ShiftRow = shift;
        }

        public override bool CheckIfRowData(string str)
        {
            if (str.EndsWith(TarStr))
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
        { return str.EndsWith(TarStr); }
    }
}
