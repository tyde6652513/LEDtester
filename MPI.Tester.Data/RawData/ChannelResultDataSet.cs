using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class ChannelResultDataSet : ICloneable, IEnumerable<ChannelResultData>
    {
        #region >>> Private Property <<<

        private object _lockObj;

        private List<ChannelResultData> _dataList;

        #endregion

        #region >>> Constructor / Disposor <<<

        public ChannelResultDataSet()
        {
            this._lockObj = new object();

            this._dataList = new List<ChannelResultData>();
        }

        #endregion

        #region >>> Public Property <<<

        public ChannelResultData this[uint channel]
        {
            get
            {
                if (this._dataList.Count == 0 || channel >= this._dataList.Count)
                {
                    return null;
                }

                return this._dataList[(int)channel];
            }
        }

        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(ChannelResultData data)
        {
            this._dataList.Add(data);
        }

        public void Clear()
        {
            this._dataList.Clear();
        }

        public void Overwrite(ChannelResultDataSet data)
        {
            uint channel = 0;

            foreach (var item in this._dataList)
            {
                data[channel].IsTested = item.IsTested;

                data[channel].Channel = channel;  // need to comfirm

                data[channel].Row = item.Row;

                data[channel].Col = item.Col;

                data[channel].SubRow = item.SubRow;

                data[channel].SubCol = item.SubCol;

                data[channel].GroupRow = item.GroupRow;

                data[channel].GroupCol = item.GroupCol;
                
                if (item.IsTested)
                {
                    //data[channel].IsTested = item.IsTested;

                    //data[channel].Channel = channel;  // need to comfirm

                    //data[channel].Row = item.Row;

                    //data[channel].Col = item.Col;

                    item.Overwrite(data[channel]);
                }

                channel++;
            }
        }

        public object Clone()
        {
            ChannelResultDataSet cloneObj = new ChannelResultDataSet();

            foreach (var item in this._dataList)
            {
                cloneObj._dataList.Add(item.Clone() as ChannelResultData);
            }

            return cloneObj;
        }

        #endregion

        #region >>> IEnumerator Interface <<<

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new ChannelResultDataEnum(this._dataList));
        }

        IEnumerator<ChannelResultData> IEnumerable<ChannelResultData>.GetEnumerator()
        {
            return (IEnumerator<ChannelResultData>)(new ChannelResultDataEnum(this._dataList));
        }

        #endregion

        #region >>> ElecSweepDataEnum Class <<<

        private class ChannelResultDataEnum : IEnumerator<ChannelResultData>
        {
            #region >>> Private Property <<<

            private int _position;
            private ChannelResultData _data;
            private List<ChannelResultData> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public ChannelResultDataEnum(List<ChannelResultData> dataList)
            {
                this._position = -1;

                this._data = default(ChannelResultData);

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

            public ChannelResultData Current
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
