using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Report.BaseMethod.HeaderFinder
{
    public class HeaderFinder_ByEndStr : HeaderFinderBase
    {
        public HeaderFinder_ByEndStr(string tarStr, int shift)
            : base(tarStr, shift)
        {
        }

        public override bool CheckIfRowData(string str)
        {
            if (str.EndsWith(_tarStr))
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
        { return str.EndsWith(_tarStr); }
    }
}
