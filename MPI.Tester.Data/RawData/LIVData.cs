using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class LIVData : ICloneable, IEnumerable<LIVResultItemData>
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private string _name;
        private string _keyName;
        private bool _isEnable;
        private string _type;
        private List<LIVResultItemData> _livResultItemData;
		private List<SpectrumData> _livSpectrumDataData;

        #endregion

        #region >>> Constructor / Disposor <<<

        public LIVData()
        {
            this._lockObj = new object();

            this._name = string.Empty;

            this._keyName = string.Empty;

            this._isEnable = false;

            this._type = "LIV";

            this._livResultItemData = new List<LIVResultItemData>();

			this._livSpectrumDataData = new List<SpectrumData>();

        }

        public LIVData(TestItemData item) : this()
        {
            if (item is LIVTestItem)
            {
                LIVTestItem livItem = (LIVTestItem)item;

                this._name = livItem.Name;

                this._keyName = livItem.KeyName;

                this._isEnable = livItem.IsEnable;

                this._type = "LIV";

                foreach (var resultItem in item.MsrtResult)
                {
                    if (resultItem.KeyName.Contains(ELIVOptiMsrtType.LIVLMTD.ToString()))
                    {
                        continue;
                    }

                    if (resultItem.KeyName.Contains(ELIVOptiMsrtType.LIVWATTTD.ToString()))
                    {
                        continue;
                    }

                    LIVResultItemData data = new LIVResultItemData(resultItem, livItem.DataLength);

                    data.IsEnable = resultItem.IsEnable;

                    this._livResultItemData.Add(data);
                }

				for (int i = 0; i < livItem.DataLength; i++)
				{
					this._livSpectrumDataData.Add(new SpectrumData(new LOPWLTestItem(), 0));
				}
            }
            else if (item is TransistorTestItem)
            {
                 TransistorTestItem trItem = (TransistorTestItem)item;

                this._name = trItem.Name;

                this._keyName = trItem.KeyName;

                this._isEnable = trItem.IsEnable;

                this._type = "TRANSISTOR";

                foreach (var resultItem in item.MsrtResult)
                {
                    //if (resultItem.KeyName.Contains(ETransistorOptiMsrtType.TRLMTD.ToString()))
                    //{
                    //    continue;
                    //}

                    //if (resultItem.KeyName.Contains(ETransistorOptiMsrtType.TRWATTTD.ToString()))
                    //{
                    //    continue;
                    //}

                    LIVResultItemData data = new LIVResultItemData(resultItem, trItem.DataLength);

                    data.IsEnable = resultItem.IsEnable;

                    this._livResultItemData.Add(data);
                }

                for (int i = 0; i < trItem.DataLength; i++)
				{
					this._livSpectrumDataData.Add(new SpectrumData(new LOPWLTestItem(), 0));
				}

            }
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

        public string Type
        {
            get { return this._type; }
            set { lock (this._lockObj) { this._type = value; } }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public int Count
        {
            get { return this._livResultItemData.Count; }
        }

        public LIVResultItemData this[string livResultItemKeyName]
        {
            get
            {
                foreach (var item in this._livResultItemData)
                {
                    if (item.KeyName.Contains(livResultItemKeyName))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public LIVResultItemData this[ELIVOptiMsrtType eLIVOptiMsrtType]
        {
            get
            {
                foreach (var item in this._livResultItemData)
                {
                    if (item.KeyName.Contains(eLIVOptiMsrtType.ToString()))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

		public LIVResultItemData this[ETransistorOptiMsrtType eTROptiMsrtType]
		{
			get
			{
				foreach (var item in this._livResultItemData)
				{
					if (item.KeyName.Contains(eTROptiMsrtType.ToString()))
					{
						return item;
					}
				}

				return null;
			}
		}

        public LIVResultItemData this[int index]
        {
            get
            {
                if (index >= 0 && index < this._livResultItemData.Count)
                {
                    return this._livResultItemData[index];
                }

                return null;
            }
        }

		public List<SpectrumData> SpectrumDataData
		{
			get { return this._livSpectrumDataData; }
		}

        #endregion

        #region >>> Public Method <<<

        public void Overwrite(LIVData data)
        {
            //data._channel = this._channel;
            for (int i = 0; i < this._livResultItemData.Count; i++)
            {
                if (this._livResultItemData[i].IsEnable)
                {
                    for (int j = 0; j < this._livResultItemData[i].DataArray.Length; j++)
                    {
                        data._livResultItemData[i].DataArray[j] = this._livResultItemData[i].DataArray[j];
                    }
                }
            }

			for (int i = 0; i < this._livSpectrumDataData.Count; i++)
			{
                data._livSpectrumDataData[i].Dark = this._livSpectrumDataData[i].Dark.ToArray();

                data._livSpectrumDataData[i].Absoluate = this._livSpectrumDataData[i].Absoluate.ToArray();

                data._livSpectrumDataData[i].Intensity = this._livSpectrumDataData[i].Intensity.ToArray();

                data._livSpectrumDataData[i].Wavelength = this._livSpectrumDataData[i].Wavelength.ToArray();		
			}
        }

        public object Clone()
        {
            LIVData cloneObj = new LIVData();

            cloneObj._name = this._name;

            cloneObj._keyName = this._keyName;

            cloneObj._isEnable = this._isEnable;

            cloneObj._type = this._type;

            foreach (var item in this._livResultItemData)
            {
                cloneObj._livResultItemData.Add(item.Clone() as LIVResultItemData);
            }

			foreach (var item in this._livSpectrumDataData)
			{
				cloneObj._livSpectrumDataData.Add(item.Clone() as SpectrumData);
			}

            return cloneObj;
        }

        #endregion
        
        #region >>> IEnumerator Interface <<<

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new LIVResultItemDataEnum(this._livResultItemData));
        }

        IEnumerator<LIVResultItemData> IEnumerable<LIVResultItemData>.GetEnumerator()
        {
            return (IEnumerator<LIVResultItemData>)(new LIVResultItemDataEnum(this._livResultItemData));
        }

        #endregion

        #region >>> ElecSweepDataEnum Class <<<

        private class LIVResultItemDataEnum : IEnumerator<LIVResultItemData>
        {
            #region >>> Private Property <<<

            private int _position;
            private LIVResultItemData _data;
            private List<LIVResultItemData> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public LIVResultItemDataEnum(List<LIVResultItemData> dataList)
            {
                this._position = -1;

                this._data = default(LIVResultItemData);

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

            public LIVResultItemData Current
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

    public class LIVResultItemData : ICloneable
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

        public LIVResultItemData()
        {
            this._lockObj = new object();

            this._name = string.Empty;

            this._keyName = string.Empty;

            this._isEnable = false;

            this._formate = "0.00000";
        }

        public LIVResultItemData(TestResultData item, int dataLength) : this()
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
            LIVResultItemData cloneObj = new LIVResultItemData();

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
