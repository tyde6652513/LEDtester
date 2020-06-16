using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class PIVDataSet : ICloneable, IEnumerable<PIVData>
    {    
        #region >>> Private Property <<<

        private object _lockObj;
        private List<PIVData> _dataList;
        private List<string> _keyNameList;

        #endregion

        #region >>> Constructor / Disposor <<<

        public PIVDataSet()
        {
            this._lockObj = new object();

            this._dataList = new List<PIVData>();
        }

        #endregion

        #region >>> Public Property <<<

        public PIVData this[string keyName]
        {
            get
            {
                foreach (var item in this._dataList)
                {
                    if (item.KeyName == keyName)
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
                            this._dataList[i] = value;
                        }
                    }
                }
            }
        }

        public PIVData this[int index]
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

        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(PIVData data)
        {
            this._dataList.Add(data);
        }

        public void Clear()
        {
            this._dataList.Clear();
        }

        public void Overwrite(PIVDataSet data)
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
                    if (this._dataList[i].PowerData.Length != data[i].PowerData.Length)
                    {
                        data[i].PowerData = new double[this._dataList[i].PowerData.Length];
                    }

                    Array.Copy(this._dataList[i].PowerData, data[i].PowerData, this._dataList[i].PowerData.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].CurrentData.Length != data[i].CurrentData.Length)
                    {
                        data[i].CurrentData = new double[this._dataList[i].CurrentData.Length];
                    }

                    Array.Copy(this._dataList[i].CurrentData, data[i].CurrentData, this._dataList[i].CurrentData.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].VoltageData.Length != data[i].VoltageData.Length)
                    {
                        data[i].VoltageData = new double[this._dataList[i].VoltageData.Length];
                    }

                    Array.Copy(this._dataList[i].VoltageData, data[i].VoltageData, this._dataList[i].VoltageData.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].SeData.Length != data[i].SeData.Length)
                    {
                        data[i].SeData = new double[this._dataList[i].SeData.Length];
                    }

                    Array.Copy(this._dataList[i].SeData, data[i].SeData, this._dataList[i].SeData.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].RsData.Length != data[i].RsData.Length)
                    {
                        data[i].RsData = new double[this._dataList[i].RsData.Length];
                    }

                    Array.Copy(this._dataList[i].RsData, data[i].RsData, this._dataList[i].RsData.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].PceData.Length != data[i].PceData.Length)
                    {
                        data[i].PceData = new double[this._dataList[i].PceData.Length];
                    }

                    Array.Copy(this._dataList[i].PceData, data[i].PceData, this._dataList[i].PceData.Length);
                }
            }
        }

        public object Clone()
        {
            PIVDataSet cloneObj = new PIVDataSet();

            foreach (var item in this._dataList)
            {
                cloneObj._dataList.Add(item.Clone() as PIVData);
            }

            return cloneObj;
        }

        #endregion

        #region >>> IEnumerator Interface <<<

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new PIVDataEnum(this._dataList));
        }

        IEnumerator<PIVData> IEnumerable<PIVData>.GetEnumerator()
        {
            return (IEnumerator<PIVData>)(new PIVDataEnum(this._dataList));
        }

        #endregion

        #region >>> PIVDataEnum Class <<<

        private class PIVDataEnum : IEnumerator<PIVData>
        {
            #region >>> Private Property <<<

            private int _position;
            private PIVData _data;
            private List<PIVData> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public PIVDataEnum(List<PIVData> dataList)
            {
                this._position = -1;

                this._data = default(PIVData);

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

            public PIVData Current
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
