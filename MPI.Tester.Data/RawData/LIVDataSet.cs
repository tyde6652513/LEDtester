using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class LIVDataSet : ICloneable, IEnumerable<LIVData>
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private List<LIVData> _dataList;

        #endregion

        #region >>> Constructor / Disposor <<<

        public LIVDataSet()
        {
            this._lockObj = new object();

            this._dataList = new List<LIVData>();
        }

        #endregion

        #region >>> Public Property <<<

        public LIVData this[string keyName]
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

        public LIVData this[int index]
        {
            get 
            {
                if (this._dataList.Count == 0 || index < 0 || index >= this._dataList.Count)
                {
                    return null;
                }
 
                return this._dataList[index]; 
            }
        }

        public string[] KeyNames
        {
            get
            {
                string[] keys = new string[this._dataList.Count];

                for (int i = 0; i < this._dataList.Count; i++)
                {
                    keys[i] = this._dataList[i].KeyName;
                }

                return keys;
            }
        }

        public string[] Names
        {
            get
            {
                string[] names = new string[this._dataList.Count];

                for (int i = 0; i < this._dataList.Count; i++)
                {
                    names[i] = this._dataList[i].Name;
                }

                return names;
            }
        }

        public string[] MsrtItemNames
        {
            get
            {
                string[] names = new string[Enum.GetNames(typeof(ELIVOptiMsrtType)).Length];

                // init the names
                int count = 0;

                foreach (string str in Enum.GetNames(typeof(ELIVOptiMsrtType)))
                {
                    names[count] = str;
                    count++;
                }

                if (this._dataList.Count != 0)
                {
                    count = 0;
                    
                    foreach (ELIVOptiMsrtType e in Enum.GetValues(typeof(ELIVOptiMsrtType)))
                    {
                        var resultItem = this._dataList[0][e];

                        if (resultItem != null)
                        {
                            names[count] = resultItem.Name;
                        }

                        count++;
                    }
                }

                return names;
            }
        }

        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(LIVData data)
        {
            this._dataList.Add(data);
        }

        public void Clear()
        {
            this._dataList.Clear();
        }

        public void Overwrite(LIVDataSet data)
        {
            foreach (var item in this._dataList)
            {
                if (item.IsEnable)
                {
                    data[item.KeyName].Name = item.Name;

                    data[item.KeyName].KeyName = item.KeyName;

                    data[item.KeyName].IsEnable = item.IsEnable;

                    item.Overwrite(data[item.KeyName]);
                }
            }
        }

        public object Clone()
        {
            LIVDataSet cloneObj = new LIVDataSet();

            foreach (var item in this._dataList)
            {
                cloneObj._dataList.Add(item.Clone() as LIVData);
            }

            return cloneObj;
        }

        #endregion

        #region >>> IEnumerator Interface <<<

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new LIVDataEnum(this._dataList));
        }

        IEnumerator<LIVData> IEnumerable<LIVData>.GetEnumerator()
        {
            return (IEnumerator<LIVData>)(new LIVDataEnum(this._dataList));
        }

        #endregion

        #region >>> ElecSweepDataEnum Class <<<

        private class LIVDataEnum : IEnumerator<LIVData>
        {
            #region >>> Private Property <<<

            private int _position;
            private LIVData _data;
            private List<LIVData> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public LIVDataEnum(List<LIVData> dataList)
            {
                this._position = -1;

                this._data = default(LIVData);

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

            public LIVData Current
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
