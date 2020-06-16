using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class ElecSweepDataSet : ICloneable, IEnumerable<ElecSweepData>
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private List<ElecSweepData> _dataList;

        private int _channelCount;

        #endregion

        #region >>> Constructor / Disposor <<<

        public ElecSweepDataSet()
        {
            this._lockObj = new object();

            this._dataList = new List<ElecSweepData>();

            this._channelCount = 1;
        }

        #endregion

        #region >>> Public Property <<<

        public ElecSweepData this[uint channel, string keyName]
        {
            get
            {
                foreach (var item in this._dataList)
                {
                    if (item.KeyName == keyName && item.Channel == channel)
                    {
                        return item;
                    }
                }

                return null;
            }

            set
            {
                lock (this._lockObj)
                {
                    for (int i = 0; i < this._dataList.Count; i++)
                    {
                        if (this._dataList[i].KeyName == keyName)
                        {
                            this._dataList[i].Channel = channel;
                            
                            this._dataList[i] = value;
                        }
                    }
                }
            }
        }

        public ElecSweepData this[int index]
        {
            get { return this._dataList[index]; }
        }

        public string[] KeyNames
        {
            get
            {
                if (this._dataList == null || this._dataList.Count == 0)
                {
                    return null;
                }

                List<string> lstKeys = new List<string>();

                foreach (var item in this._dataList)
                {
                    if (!lstKeys.Contains(item.KeyName))
                    {
                        lstKeys.Add(item.KeyName);
                    }
                }

                return lstKeys.ToArray();
            }
        }

        public int ChannelCount
        {
            get { return this._channelCount; }
            set { lock (_lockObj) { this._channelCount = value; } }
        }
        
        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(ElecSweepData data)
        {
            this._dataList.Add(data);
        }

        public void Clear()
        {
            this._dataList.Clear();
        }

        public void Overwrite(ElecSweepDataSet data)
        {
            for (int i = 0; i < this._dataList.Count; i++)
            {
                if (this._dataList[i].IsEnable)
                {
                    data[i].Channel = this._dataList[i].Channel;

                    data[i].IsEnable = this._dataList[i].IsEnable;

                    data[i].KeyName = this._dataList[i].KeyName;

                    data[i].Name = this._dataList[i].Name;

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].TimeChain.Length != data[i].TimeChain.Length)
                    {
                        data[i].TimeChain = new double[this._dataList[i].TimeChain.Length];
                    }

                    Array.Copy(this._dataList[i].TimeChain, data[i].TimeChain, this._dataList[i].TimeChain.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].ApplyData.Length != data[i].ApplyData.Length)
                    {
                        data[i].ApplyData = new double[this._dataList[i].ApplyData.Length];
                    }

                    Array.Copy(this._dataList[i].ApplyData, data[i].ApplyData, this._dataList[i].ApplyData.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].SweepData.Length != data[i].SweepData.Length)
                    {
                        data[i].SweepData = new double[this._dataList[i].SweepData.Length];
                    }

                    Array.Copy(this._dataList[i].SweepData, data[i].SweepData, this._dataList[i].SweepData.Length);
                }
            }
        }

        public bool ContainKeyName(string keyName)
        {
            if (keyName == string.Empty || this._dataList == null || this._dataList.Count == 0)
            {
                return false;
            }

            foreach (var item in this._dataList)
            {
                if (item.KeyName == keyName)
                {
                    return true;
                }
            }

            return false;
        }

        public object Clone()
        {
            ElecSweepDataSet cloneObj = new ElecSweepDataSet();

            cloneObj._channelCount = this._channelCount;

            foreach (var item in this._dataList)
            {
                cloneObj._dataList.Add(item.Clone() as ElecSweepData);
            }

            return cloneObj;
        }

        #endregion

        #region >>> IEnumerator Interface <<<

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new ElecSweepDataEnum(this._dataList));
        }

        IEnumerator<ElecSweepData> IEnumerable<ElecSweepData>.GetEnumerator()
        {
            return (IEnumerator<ElecSweepData>)(new ElecSweepDataEnum(this._dataList));
        }

        #endregion

        #region >>> ElecSweepDataEnum Class <<<

        private class ElecSweepDataEnum : IEnumerator<ElecSweepData>
        {
            #region >>> Private Property <<<

            private int _position;
            private ElecSweepData _data;
            private List<ElecSweepData> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public ElecSweepDataEnum(List<ElecSweepData> dataList)
            {
                this._position = -1;

                this._data = default(ElecSweepData);

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

            public ElecSweepData Current
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
