using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Report.BaseMethod.HeaderFinder
{
    public class HeaderFinder_ByStartStr: HeaderFinderBase
    {
        public HeaderFinder_ByStartStr(string tarStr, int shift)
            : base(tarStr, shift)
        {
        }

        public override bool CheckIfRowData(string str)
        {
            if (str.StartsWith(_tarStr))
            {
                startCount = true;
            }

            if (startCount)
            {
                _shiftRow--;
                if (_shiftRow <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool IsFitTarStr(string str)//純粹確認輸入的字串與TarStr
        { return str.StartsWith(_tarStr); }
    }
}
