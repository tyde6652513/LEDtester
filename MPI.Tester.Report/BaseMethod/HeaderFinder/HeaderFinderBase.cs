using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Report.BaseMethod.HeaderFinder
{
    public class HeaderFinderBase:ICloneable
    {
        protected string _tarStr = "";
        protected int _shiftRow = 0;
        protected int _oriShiftRow = 0;

        protected bool startCount = false;

        public HeaderFinderBase(string tarStr, int shift)
        {
            _tarStr = tarStr;
            _oriShiftRow = shift;
            _shiftRow = _oriShiftRow;
        }

        public virtual bool CheckIfRowData(string str)//表頭使用這個來確認，包含ShiftRow功能
        {
            if (str == _tarStr)
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

        public virtual bool IsFitTarStr(string str)//純粹確認輸入的字串與TarStr
        { return str == _tarStr; }

        public object Clone()
        {
            HeaderFinderBase obj = new HeaderFinderBase(_tarStr, _shiftRow);
            return obj;
        }
        public void Reset()
        {
            _shiftRow = _oriShiftRow;
            startCount = false;
        }

        public enum EHFType:int
        {
            Base = 1,
            ByEndStr =2,
            ByStartStr = 3

        }

    }
}
