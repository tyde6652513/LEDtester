using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class BaseData : ICloneable, IEnumerable<BaseResultData>
    {
        #region >>> Private Property <<<

        protected object _lockObj;
        protected string _name;
        protected string _keyName;
        protected bool _isEnable;
        protected string _type;
        protected uint _chennel;//0 base
        protected List<BaseResultData> _baseResultItemDataList;

        #endregion

        #region >>> Constructor / Disposor <<<

        public BaseData()
        {
            this._lockObj = new object();

            this._name = string.Empty;

            this._keyName = string.Empty;

            this._isEnable = false;

            this._type = "Base";

            this._chennel = 0;

            this._baseResultItemDataList = new List<BaseResultData>();
        }

        public BaseData(TestItemData item, int sweepLength, uint ch = 0)//考量到 TestItemData並無原生的變數紀錄掃描點數，因此強迫使用者寫入
            : this()
        {

            this._name = item.Name;

            this._keyName = item.KeyName;

            this._isEnable = item.IsEnable;

            this._type = item.Type.ToString();

            this._chennel = ch;

            foreach (var resultItem in item.MsrtResult)
            {

                BaseResultData data = new BaseResultData(resultItem, sweepLength);

                data.IsEnable = resultItem.IsEnable;

                this._baseResultItemDataList.Add(data);
            }

            

        }

        #endregion

        #region >>> Public Property <<<

        virtual public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }

        virtual public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        virtual public string Type
        {
            get { return this._type; }
            set { lock (this._lockObj) { this._type = value; } }
        }

        virtual public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        virtual public int Count
        {
            get { return this._baseResultItemDataList.Count; }
        }

        virtual public uint Channel
        {
            get { return this._chennel; }
            set { lock (this._lockObj) { this._chennel = value; } }
        }        

        virtual public BaseResultData this[string ResultItemKeyName]
        {
            get
            {
                foreach (var item in this._baseResultItemDataList)
                {
                    if (item.KeyName.Contains(ResultItemKeyName))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        virtual public BaseResultData this[int index]
        {
            get
            {
                if (index >= 0 && index < this._baseResultItemDataList.Count)
                {
                    return this._baseResultItemDataList[index];
                }

                return null;
            }
        }

        virtual public List<BaseResultData> ResultList
        {
            get { return _baseResultItemDataList; }
 
        }

        #endregion

        #region >>> Public Method <<<

        virtual public void Overwrite(BaseData data)
        {
            for (int i = 0; i < this._baseResultItemDataList.Count; i++)
            {
                if (this._baseResultItemDataList[i].IsEnable)
                {
                    for (int j = 0; j < this._baseResultItemDataList[i].DataArray.Length; j++)
                    {
                        data._baseResultItemDataList[i].DataArray[j] = this._baseResultItemDataList[i].DataArray[j];
                    }
                }
            }
        }

        virtual public object Clone()
        {
            BaseData cloneObj = new BaseData();

            cloneObj._name = this._name;

            cloneObj._keyName = this._keyName;

            cloneObj._isEnable = this._isEnable;

            cloneObj._type = this._type;

            foreach (var item in this._baseResultItemDataList)
            {
                cloneObj._baseResultItemDataList.Add(item.Clone() as BaseResultData);
            }



            return cloneObj;
        }

        #endregion

        #region >>> IEnumerator Interface <<<



        IEnumerator<BaseResultData> IEnumerable<BaseResultData>.GetEnumerator()
        {
            return (IEnumerator<BaseResultData>)(new BaseResultDataEnum(this._baseResultItemDataList));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new BaseResultDataEnum(this._baseResultItemDataList));
        }

        #endregion

        #region >>> ElecSweepDataEnum Class <<<

        private class BaseResultDataEnum : IEnumerator<BaseResultData>
        {
            #region >>> Private Property <<<

            private int _position;
            private BaseResultData _data;
            private List<BaseResultData> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public BaseResultDataEnum(List<BaseResultData> dataList)
            {
                this._position = -1;

                this._data = default(BaseResultData);

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

            public BaseResultData Current
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


    public class BaseResultData : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private string _name;
        private string _keyName;
        private bool _isEnable;
        private string _unit;
        private string _formate;
        private int _dataLength;
        private float[] _dataArray;

        #endregion

        #region >>> Constructor / Disposor <<<

        public BaseResultData()
        {
            this._lockObj = new object();

            this._name = string.Empty;

            this._keyName = string.Empty;

            this._isEnable = false;

            this._formate = "0.00000";
        }

        public BaseResultData(TestResultData item, int dataLength)
            : this()
        {
            this._name = item.Name;

            this._keyName = item.KeyName;

            this._isEnable = item.IsEnable;

            this._unit = item.Unit;

            this._formate = item.Formate;

            this._dataLength = dataLength;

            this._dataArray = new float[dataLength];
        }

        #endregion

        #region >>> Public Property <<<

        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }

        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        public string Unit
        {
            get { return this._unit; }
            set { lock (this._lockObj) { this._unit = value; } }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public string Formate
        {
            get { return this._formate; }
            set { lock ((this._lockObj)) { this._formate = value; } }
        }

        public float[] DataArray
        {
            get { return this._dataArray; }
            set { lock (this._lockObj) { this._dataArray = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            BaseResultData cloneObj = new BaseResultData();

            cloneObj._name = this._name;

            cloneObj._keyName = this._keyName;

            cloneObj._isEnable = this._isEnable;

            cloneObj._unit = this._unit;

            cloneObj._formate = this._formate;

            cloneObj._dataLength = this._dataLength;

            cloneObj._dataArray = this._dataArray.Clone() as float[];

            return cloneObj;
        }

        #endregion
    }
}
