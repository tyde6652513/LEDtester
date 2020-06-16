using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    public class ChipInfo : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private int _colX;
        private int _rowY;
        private int _reTestColX;
        private int _reTestRowY;
        private int _testCount;
        private int _binGrade;
        private int _goodDieCount;
        private int _failDieCount;
        private int _chuckIndex;
        private int _transCOL;
        private int _transROW;
        private int _chipIndex;
        private bool _isPass;
        private bool _isPass2;
        private bool _isSortTestOK;
        private bool _isOpenTestOK;
        private bool _isDieTestStatePass;
        private double _goodRate;
        private double _testTime;
        
        private EPolarity _polarity;
        private EAdjacentResult _adjacentResult;

        private double _chuckTemp;
        private bool _isNewChuckData;

        private uint _channel;
        private string _groupName;
        private int _testGroupIndex;

        private DateTime _startTime;
        private DateTime _endTime;

        #endregion

        #region >>> Constructor / Disposor <<<

        public ChipInfo()
        {
            this._lockObj = new object();

            this._channel = 0;

            this._colX = 0;

            this._rowY = 0;

            this._reTestColX = 0;

            this._reTestRowY = 0;

            this._testCount = 0;

            this._binGrade = 0;

            this._goodDieCount = 0;

            this._failDieCount = 0;

            this._chuckIndex = 0;

            this._isPass = false;

            this._isPass2 = false;

            this._isSortTestOK = false;

            this._isOpenTestOK = false;

            this._isDieTestStatePass = false;

            this._goodRate = 0.0d;

            this._testTime = 0.0d;

            this._polarity = EPolarity.Anode_P;

            this._adjacentResult = EAdjacentResult.NONE;

            this._groupName = "";

            this._chuckTemp = 0;

            this._chipIndex = 0;

            this._isNewChuckData = false;

            this._startTime = DateTime.Now;

            this._endTime = DateTime.Now;
        }

        #endregion

        #region >>> Public Property <<<

        public int ColX
        {
            get { return this._colX; }
            set { lock (_lockObj) { this._colX = value; } }
        }

        public int RowY
        {
            get { return this._rowY; }
            set { lock (_lockObj) { this._rowY = value; } }
        }

        public uint Channel
        {
            get { return this._channel; }
            set { lock (_lockObj) { this._channel = value; } }
        }

        public int ReTestColX
        {
            get { return this._reTestColX; }
            set { lock (_lockObj) { this._reTestColX = value; } }
        }

        public int ReTestRowY
        {
            get { return this._reTestRowY; }
            set { lock (_lockObj) { this._reTestRowY = value; } }
        }

        public int TestCount
        {
            get { return this._testCount; }
            set { lock (_lockObj) { this._testCount = value; } }
        }

        public int BinGrade
        {
            get { return this._binGrade; }
            set { lock (_lockObj) { this._binGrade = value; } }
        }

        public int GoodDieCount
        {
            get { return this._goodDieCount; }
            set { lock (_lockObj) { this._goodDieCount = value; } }
        }

        public int FailDieCount
        {
            get { return this._failDieCount; }
            set { lock (_lockObj) { this._failDieCount = value; } }
        }

        public int ChuckIndex
        {
            get { return this._chuckIndex; }
            set { lock (_lockObj) { this._chuckIndex = value; } }
        }


        public int TransCol
        {
            get { return this._transCOL; }
            set { lock (_lockObj) { this._transCOL = value; } }
        }

        public int TransRow
        {
            get { return this._transROW; }
            set { lock (_lockObj) { this._transROW = value; } }
        }
        public int ChipIndex
        {
            get { return this._chipIndex; }
            set { lock (_lockObj) { this._chipIndex = value; } }
        }

        public bool IsPass
        {
            get { return this._isPass; }
            set { lock (_lockObj) { this._isPass = value; } }
        }

        public bool IsPass2
        {
            get { return this._isPass2; }
            set { lock (_lockObj) { this._isPass2 = value; } }
        }

        public bool IsSortTestOK
        {
            get { return this._isSortTestOK; }
            set { lock (_lockObj) { this._isSortTestOK = value; } }
        }

        public bool IsOpenTestOK
        {
            get { return this._isOpenTestOK; }
            set { lock (_lockObj) { this._isOpenTestOK = value; } }
        }

        public bool IsDieTestStatePass
        {
            get { return this._isDieTestStatePass; }
            set { lock (_lockObj) { this._isDieTestStatePass = value; } }
        }

        public double GoodRate
        {
            get { return this._goodRate; }
            set { lock (_lockObj) { this._goodRate = value; } }
        }

        public double TestTime
        {
            get { return this._testTime; }
            set { lock (_lockObj) { this._testTime = value; } }
        }

        public EPolarity Polarity
        {
            get { return this._polarity; }
            set { lock (_lockObj) { this._polarity = value; } }
        }

        public EAdjacentResult AdjacentResult
        {
            get { return this._adjacentResult; }
            set { lock (_lockObj) { this._adjacentResult = value; } }
        }
        public string GroupName
        {
            get { return this._groupName; }
            set { lock (_lockObj) { this._groupName = value; } }
        }

        public int GroupIndex
        {
            get { return this._testGroupIndex; }
            set { lock (_lockObj) { this._testGroupIndex = value; } }            
        }

        public double ChuckTemp
        {
            set
            {
                lock (_lockObj)
                {
                    this._chuckTemp = value;
                    _isNewChuckData = true;
                }
            }

            get
            {
                _isNewChuckData = false;
                return this._chuckTemp;
            }
        }

        public bool IsNewChuckData
        {
            get
            {
                return _isNewChuckData;
            }
        }

        public DateTime StartTime
        {
            set
            {
                lock (_lockObj)
                {
                    this._startTime = value;
                }
            }
            get
            {
                return _startTime;
            }
        }

        public DateTime EndTime
        {
            set
            {
                lock (_lockObj)
                {
                    this._endTime = value;
                }
            }
            get
            {
                return _endTime;
            }
        }

        public TimeSpan TimeSpan 
        {
            get { return EndTime - StartTime; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Overwrite(ChipInfo data)
        {
            data._colX = this._colX;

            data._rowY = this._rowY;

            data._channel = this._channel;

            data._reTestColX = this._reTestColX;

            data._reTestRowY = this._reTestRowY;

            data._testCount = this._testCount;

            data._binGrade = this._binGrade;

            data._goodDieCount = this._goodDieCount;

            data._failDieCount = this._failDieCount;

            data._chuckIndex = this._chuckIndex;

            data._isPass = this._isPass;

            data._isPass2 = this._isPass2;

            data._isSortTestOK = this._isSortTestOK;

            data._isOpenTestOK = this._isOpenTestOK;

            data._isDieTestStatePass = this._isDieTestStatePass;

            data._goodRate = this._goodRate;

            data._testTime = this._testTime;

            data._polarity = this._polarity;

            data._adjacentResult = this._adjacentResult;

            data._groupName = this._groupName;

            data._chipIndex = this._chipIndex;


            data._startTime = this._startTime;

            data._endTime = this._endTime;

            if (this._isDieTestStatePass)
            {
                data.ChuckTemp = this.ChuckTemp;//use proprety to change the state of IsNewChuckData
            }

        }

        public object Clone()
        {
            ChipInfo cloneObj = this.MemberwiseClone() as ChipInfo;

            return cloneObj;
        }

        #endregion

    }
}
