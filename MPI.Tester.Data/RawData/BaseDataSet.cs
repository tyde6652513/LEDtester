using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class BaseDataSet<T> : ICloneable, IEnumerable<T>  where T:BaseData 
    {
         #region >>> Private Property <<<

        protected object _lockObj;
        protected List<T> _dataList;

        #endregion

        #region >>> Constructor / Disposor <<<

         public BaseDataSet()
        {
            this._lockObj = new object();

            this._dataList = new List<T>();
        }

        #endregion

        #region >>> Public Property <<<

        virtual public T this[string keyName]
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

        virtual public T this[int index]
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

        virtual public string[] KeyNames
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

        virtual public string[] Names
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

        virtual public string[] MsrtItemNames
        {
            get
            {
                List<string> nameList = new List<string>();
                foreach (T testData in _dataList)
                {
                    foreach (BaseResultData resultData in testData.ResultList)
                    {
                        nameList.Add(resultData.KeyName);
                    } 
                }
                return nameList.ToArray();
            }
        }

        virtual public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        virtual public void Add(T data)
        {
            this._dataList.Add(data);
        }

        virtual public void Clear()
        {
            this._dataList.Clear();
        }

        virtual public void Overwrite(BaseDataSet<T> data)
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

        virtual public object Clone()
        {
            BaseDataSet<T> cloneObj = new BaseDataSet<T>();

            foreach (T item in this._dataList)
            {
                cloneObj._dataList.Add(item.Clone() as T);
            }

            return cloneObj;
        }

        #endregion

        #region >>> IEnumerator Interface <<<

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new BaseDataEnum(this._dataList));
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (IEnumerator<T>)(new BaseDataEnum(this._dataList));
        }

        #endregion

        #region >>> ElecSweepDataEnum Class <<<

         private class BaseDataEnum : IEnumerator<T>
        {
            #region >>> Private Property <<<

            private int _position;
            private T _data;
            private List<T> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public BaseDataEnum(List<T> dataList)
            {
                this._position = -1;

                this._data = default(T);

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

            public T Current
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
