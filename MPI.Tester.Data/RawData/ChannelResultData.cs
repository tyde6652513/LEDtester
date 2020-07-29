using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class ChannelResultData : ICloneable, IEnumerable<TestResultData>
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private bool _isTested;
        private uint _channel;
        private int _row = 0;
        private int _col = 0;
        private int _subRow = 0;
        private int _subCol = 0;
        private int _groupRow = 0;
        private int _groupCol = 0;
        private bool _isPass;
        private int _binGrade;

        private string _binGradeName;

        private List<TestResultData> _channelResultData;

        #endregion

        #region >>> Constructor / Disposor <<<

        public ChannelResultData()
        {
            this._lockObj = new object();

            this._isTested = false;

            this._channel = 0;  // 0-base

            this._row = 0;

            this._col = 0;

            this._isPass = false;

            this._binGradeName = "";

            this._channelResultData = new List<TestResultData>();
        }

        #endregion

        #region >>> Public Property <<<

        public bool IsTested
        {
            get { return this._isTested; }
            set { lock (this._lockObj) { this._isTested = value; } }
        }

        public uint Channel
        {
            get { return this._channel; }
            set 
            { 
                lock (this._lockObj) 
                { 
                    this._channel = value;

                    PushValueByKey("CHANNEL", value);
                } 
            }
        }

        public int Row
        {
            get { return this._row; }
            set 
            { 
                lock (this._lockObj) 
                { 
                    this._row = value;

                    PushValueByKey("ROW", value);
                } 
            }
        }

        public int Col
        {
            get { return this._col; }
            set 
            { 
                lock (this._lockObj) 
                { 
                    this._col = value;

                    PushValueByKey("COL", value);
                } 
            }
        }

        public int SubRow
        {
            get { return this._subRow; }
            set
            {
                lock (this._lockObj)
                {
                    this._subRow = value;
                    PushValueByKey("SubROW", value);

                }
            }
        }

        public int SubCol
        {
            get { return this._subCol; }
            set
            {
                lock (this._lockObj)
                {
                    this._subCol = value;
                    PushValueByKey("SubCOL", value);

                }
            }
        }


        public int GroupRow
        {
            get { return this._groupRow; }
            set
            {
                lock (this._lockObj)
                {
                    this._groupRow = value;
                    PushValueByKey("GroupROW", value);
                }
            }
        }

        public int GroupCol
        {
            get { return this._groupCol; }
            set
            {
                lock (this._lockObj)
                {
                    this._groupCol = value;
                    PushValueByKey("GroupCOL", value);
                }
            }
        }

        public int Count
        {
            get { return this._channelResultData.Count; }
        }

        public bool IsPass
        {
            get { return this._isPass; }
            set { lock (this._lockObj) { this._isPass = value; } }
        }

        public int BinGrade
        {
            get { return this._binGrade; }
            set
            {
                lock (this._lockObj)
                {
                    this._binGrade = value;
                    PushValueByKey("BIN", value);
                }
            }
        }

        public string BinGradeName
        {
            get { return this._binGradeName; }
            set { lock (this._lockObj) { this._binGradeName = value;  } }
        }


        public TestResultData this[string keyName]
        {
            get
            {
                foreach (var item in this._channelResultData)
                {
                    if (item.KeyName.Contains(keyName))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public TestResultData this[int index]
        {
            get
            {
                if (index < this._channelResultData.Count)
                {
                    return this._channelResultData[index];
                }
                
                return null;
            }
        }

        #endregion

        #region >>> private method<<<

        private void PushValueByKey(string key, double value)
        {
            if (this._channelResultData != null)
            {
                foreach (var item in this._channelResultData)
                {
                    if (item.KeyName == key)
                    {
                        item.Value = value;
                    }
                }
            }
        }
        #endregion

        #region >>> Public Method <<<

        public void Add(TestResultData data)
        {
            this._channelResultData.Add(data);
        }

        public void AddRange(TestResultData[] dataArray)
        {
            TestResultData tempData = new TestResultData();

            foreach (TestResultData data in dataArray)
            {
                tempData.IsEnable = data.IsEnable;

                tempData.KeyName = data.KeyName;

                tempData.Name = data.Name;

                this._channelResultData.Add(tempData.Clone() as TestResultData);
            }

            tempData = null;
        }

        public void Overwrite(ChannelResultData data)
        {
            for (int i = 0; i < this._channelResultData.Count; i++)
            {
                if (this._channelResultData[i].IsEnable)
                {

                    data._channelResultData[i].RawValue = this._channelResultData[i].RawValue;

                    data._channelResultData[i].Value = this._channelResultData[i].Value;

                    data._channelResultData[i].IsTested = this._channelResultData[i].IsTested;
                }
            }
        }

        public object Clone()
        {
            ChannelResultData cloneObj = new ChannelResultData();

            cloneObj._isTested = this._isTested;

            cloneObj._channel = this._channel;

            cloneObj._row = this._row;

            cloneObj._col = this._col;

            cloneObj._isPass = this._isPass;

            cloneObj._binGrade = this._binGrade;

            cloneObj._binGradeName = this._binGradeName.ToString();


            foreach (var item in this._channelResultData)
            {
                cloneObj._channelResultData.Add(item.Clone() as TestResultData);
            }

            return cloneObj;
        }

        #endregion

        #region >>> IEnumerator Interface <<<

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new ChannelResultItemDataEnum(this._channelResultData));
        }

        IEnumerator<TestResultData> IEnumerable<TestResultData>.GetEnumerator()
        {
            return (IEnumerator<TestResultData>)(new ChannelResultItemDataEnum(this._channelResultData));
        }

        #endregion

        #region >>> ElecSweepDataEnum Class <<<

        private class ChannelResultItemDataEnum : IEnumerator<TestResultData>
        {
            #region >>> Private Property <<<

            private int _position;
            private TestResultData _data;
            private List<TestResultData> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public ChannelResultItemDataEnum(List<TestResultData> dataList)
            {
                this._position = -1;

                this._data = default(TestResultData);

                this._dataList = dataList;
            }

            #endregion

            #region >>> Interface Property <<<

            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion

            #region >>> Public Method <<<

            public TestResultData Current
            {
                get { return this._data; }
            }

            public bool MoveNext()
            {
                if (++this._position >= this._dataList.Count)
                {
                    return false;
                }
                else
                {
                    this._data = this._dataList[this._position];
                }

                return true;
            }

            public void Reset()
            {
                this._position = -1;
            }

            public void Dispose()
            {
            }

            #endregion

        }

        #endregion
    }
}
