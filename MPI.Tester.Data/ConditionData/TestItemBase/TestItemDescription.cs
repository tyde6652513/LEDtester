using System;
using System.Collections.Generic;
using System.Collections;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class TestItemDescriptionCollections : ICloneable/*, IEnumerable<TestItemDescription>*/
    {
        private object _lockObj;

        private List<TestItemDescription> _dataList;

        public TestItemDescriptionCollections()
        {
            this._lockObj = new object();

            this._dataList = new List<TestItemDescription>();
        }

        #region >>> Public Property <<<

        public TestItemDescription this[string testTypeKeyName]
        {
            get
            {
                foreach (var data in this._dataList)
                {
                    if (data.KeyName == testTypeKeyName)
                    {
                        return data;
                    }
                }

                return null;
            }
        }

        public List<TestItemDescription> Items
        {
            get { return this._dataList; }
            set { lock (this._lockObj) { this._dataList = value; } }
        }

        public string[] TestTypeKeyNames
        {
            get
            {
                List<string> lstKeyNames = new List<string>();
                
                if (this._dataList.Count != 0)
                {
                    foreach (var data in this._dataList)
                    {
                        lstKeyNames.Add(data.KeyName);
                    }
                }

                return lstKeyNames.ToArray();
            }
        }

        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(TestItemDescription data)
        {
            this._dataList.Add(data);
        }

        public void AddRange(TestItemDescription data)
        {
            this._dataList.Add(data);
        }

        public void OverWrite(string testTypeKeyName, TestItemDescription data)
        {
            if (data == null || data.Property.Count == 0)
            {
                return;
            }
            
            TestItemDescription desc = this[testTypeKeyName];

            if (desc != null)
            {
                foreach (var prop in data.Property)
                {
                    if (desc.ContainsKeyName(prop.PropertyKeyName))
                    {
                        desc[prop.PropertyKeyName].OverWrite(prop);
                    }
                    else
                    {
                        desc.Property.Add(prop.Clone() as ItemDescriptionBase);
                    }
                }
            }
        }

        public bool ContainsKeyName(string testTypeKeyName)
        {
            if (this._dataList.Count == 0)
            {
                return false;
            }

            foreach (var data in this._dataList)
            {
                if (data.KeyName == testTypeKeyName)
                {
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            this._dataList.Clear();
        }

        public object Clone()
        {
            TestItemDescriptionCollections cloneObj = new TestItemDescriptionCollections();

            foreach (var data in this._dataList)
            {
                cloneObj._dataList.Add(data.Clone() as TestItemDescription);
            }

            return cloneObj;
        }

        #endregion

        //#region >>> IEnumerator Interface <<<

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return (IEnumerator)(new TestItemDescriptionCollectionsEnum(this._dataList));
        //}

        //IEnumerator<TestItemDescription> IEnumerable<TestItemDescription>.GetEnumerator()
        //{
        //    return (IEnumerator<TestItemDescription>)(new TestItemDescriptionCollectionsEnum(this._dataList));
        //}

        //#endregion

        //#region >>> TestItemDescriptionCollectionsEnum Class <<<

        //private class TestItemDescriptionCollectionsEnum : IEnumerator<TestItemDescription>
        //{
        //    #region >>> Private Property <<<

        //    private int _position;
        //    private TestItemDescription _data;
        //    private List<TestItemDescription> _dataList;

        //    #endregion

        //    #region >>> Constructor / Disposor <<<

        //    public TestItemDescriptionCollectionsEnum(List<TestItemDescription> dataList)
        //    {
        //        this._position = -1;

        //        this._data = default(TestItemDescription);

        //        this._dataList = dataList;
        //    }

        //    #endregion

        //    #region >>> Interface Property <<<

        //    object IEnumerator.Current
        //    {
        //        get { return Current; }
        //    }

        //    #endregion

        //    #region >>> Public Method <<<

        //    public TestItemDescription Current
        //    {
        //        get { return this._data; }
        //    }

        //    public bool MoveNext()
        //    {
        //        if (++this._position >= this._dataList.Count)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            this._data = this._dataList[this._position];
        //        }

        //        return true;
        //    }

        //    public void Reset()
        //    {
        //        this._position = -1;
        //    }

        //    public void Dispose()
        //    {
        //    }

        //    #endregion

        //}

        //#endregion
    }

    [Serializable]
    public class TestItemDescription : ICloneable/*, IEnumerable<ItemDescriptionBase>*/
    {
        private object _lockObj;

        private string _keyName;

        private List<ItemDescriptionBase> _dataList;

        public TestItemDescription()
        {
            this._lockObj = new object();

            this._dataList = new List<ItemDescriptionBase>();
        }

        public TestItemDescription(string itemKeyName) : this()
        {
            this._keyName = itemKeyName;
        }

        #region >>> Public Property <<<

        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        public ItemDescriptionBase this[string propertyKeyName]
        {
            get 
            {
                foreach (var data in this._dataList)
                {
                    if (data.PropertyKeyName == propertyKeyName)
                    {
                        return data;
                    }
                }

                return null;
            }
        }

        public List<ItemDescriptionBase> Property
        {
            get { return this._dataList; }
            set { lock (this._lockObj) { this._dataList = value; } }
        }

        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(ItemDescriptionBase data)
        {
            this._dataList.Add(data);
        }

        public bool ContainsKeyName(string propertyKeyName)
        {
            if (this._dataList.Count == 0)
            {
                return false;
            }

            foreach (var data in this._dataList)
            {
                if (data.PropertyKeyName == propertyKeyName)
                {
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            this._dataList.Clear();
        }

        public object Clone()
        {
            TestItemDescription cloneObj = new TestItemDescription();

            cloneObj._keyName = this._keyName;

            foreach(var data in this._dataList)
            {
                cloneObj._dataList.Add(data.Clone() as ItemDescriptionBase);
            }

            return cloneObj;
        }

        #endregion

        
    }

    [Serializable]
    public class ItemDescriptionBase : ICloneable
    {
        private object _lockObj;

        private bool _isEnable;
        private bool _isVisible;
        private string _keyName;
        private double _minValue;
        private double _maxValue;
        private double _defaultValue;

        private string _format;
        private string _unit;

        public ItemDescriptionBase()
        {
            this._lockObj = new object();

            this._isEnable = false;
            this._isVisible = false;
            this._minValue = 0.0;
            this._maxValue = 0.0;
            this._defaultValue = 0.0;
            this._format = "0";
        }

        public ItemDescriptionBase(string propertyKeyName, double minValue, double maxValue, double defaultValue, string unit, string format) : this()
        {
            this._isEnable = true;
            this._isVisible = true;
            this._keyName = propertyKeyName;
            this._minValue = minValue;
            this._maxValue = maxValue;
            this._defaultValue = defaultValue;
            this._unit = unit;
            this._format = format;
        }

        #region >>> Public Property <<<

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public bool IsVisible
        {
            get { return this._isVisible; }
            set { lock (this._lockObj) { this._isVisible = value; } }
        }

        public string PropertyKeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }             
        }

        public double MinValue
        {
            get { return this._minValue; }
            set { lock (this._lockObj) { this._minValue = value; } }              
        }

        public double MaxValue
        {
            get { return this._maxValue; }
            set { lock (this._lockObj) { this._maxValue = value; } }              
        }

        public double DefaultValue
        {
            get { return this._defaultValue; }
            set { lock (this._lockObj) { this._defaultValue = value; } }
        }

        public string Format
        {
            get { return this._format; }
            set { lock (this._lockObj) { this._format = value; } }      
        }

        public string Unit
        {
            get { return this._unit; }
            set { lock (this._lockObj) { this._unit = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        private double ChenckUpDownBoundary(double value, double lowValue, double highValue)
        {
            if (value <= lowValue)
            {
                return lowValue;
            }

            if (value > highValue)
            {
                return highValue;
            }

            return value;
        }

        private void CheckBoundaryLimit(double deviceLowerLimit, double deviceUpperLimit, bool isByPassMax)
        {
			this._minValue = Math.Max(this._minValue, deviceLowerLimit);

            if (isByPassMax)
            {
                this._maxValue = deviceUpperLimit;
            }
            else
            {
                this._maxValue = Math.Min(this._maxValue, deviceUpperLimit);
            }

            this._defaultValue = this.ChenckUpDownBoundary(this._defaultValue, this._minValue, this._maxValue);
        }

        private double UnitConvertFactor(EVoltUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EVoltUnit), toUnit))
            {
                EVoltUnit tmp = (EVoltUnit)Enum.Parse(typeof(EVoltUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        private double UnitConvertFactor(EAmpUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EAmpUnit), toUnit))
            {
                EAmpUnit tmp = (EAmpUnit)Enum.Parse(typeof(EAmpUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

		private double UnitConvertFactor(EFreqUnit fromUnit, string toUnit)
		{
			double scale = 0.0d;

			if (Enum.IsDefined(typeof(EFreqUnit), toUnit))
			{
				EFreqUnit tmp = (EFreqUnit)Enum.Parse(typeof(EFreqUnit), toUnit, false);

				scale = Math.Pow(10.0d, fromUnit - tmp);
			}

			return scale;
		}

        #endregion

        #region >>> Public Method <<<

        public bool OverWrite(ItemDescriptionBase data)
        {
			this._isVisible = data.IsVisible;

            if (data.MaxValue <= 0 || data.MaxValue < data.MinValue)
            {
                return false;
            }

            //this._isEnable = data.IsEnable;

            this._maxValue = data.MaxValue;

            this._minValue = data.MinValue;

            this._defaultValue = data.DefaultValue;

            this._unit = data.Unit;

            this._format = data.Format;

            this._defaultValue = this.ChenckUpDownBoundary(this._defaultValue, this._minValue, this._maxValue);

            return true;
        }

        public void LimitBoundary(double deviceLowerLimit, double deviceUpperLimit, EVoltUnit fromUnit, bool isByPassMax = false)
        {
            deviceLowerLimit *= this.UnitConvertFactor(fromUnit, this._unit);

            deviceUpperLimit *= this.UnitConvertFactor(fromUnit, this._unit);

            this.CheckBoundaryLimit(deviceLowerLimit, deviceUpperLimit, isByPassMax);
        }

        public void LimitBoundary(double deviceLowerLimit, double deviceUpperLimit, EAmpUnit fromUnit, bool isByPassMax = false)
        {
            deviceLowerLimit *= this.UnitConvertFactor(fromUnit, this._unit);

            deviceUpperLimit *= this.UnitConvertFactor(fromUnit, this._unit);

            this.CheckBoundaryLimit(deviceLowerLimit, deviceUpperLimit, isByPassMax);
        }

        public void LimitBoundary(double deviceLowerLimit, double deviceUpperLimit, EFreqUnit fromUnit, bool isByPassMax = false)
		{
			deviceLowerLimit *= this.UnitConvertFactor(fromUnit, this._unit);

			deviceUpperLimit *= this.UnitConvertFactor(fromUnit, this._unit);

            this.CheckBoundaryLimit(deviceLowerLimit, deviceUpperLimit, isByPassMax);
		}

        public void LimitBoundary(double deviceLowerLimit, double deviceUpperLimit, bool isByPassMax = false)
        {
            this.CheckBoundaryLimit(deviceLowerLimit, deviceUpperLimit, isByPassMax);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    
	[Serializable]
	public class LCRItemDescription : ItemDescriptionBase
	{
		private List<ELCRTestType> _supportMserItemList;

        private List<ELCRTestType> _caliItemList;

		private List<ELCRMsrtSpeed> _supportMsrtSpeed;

        //private List<LCRCaliData> _caliDataList;

        private List<string> _cableLengthList;

        private uint _caliDataQty;

		public LCRItemDescription()
			: base()
		{
			this._supportMserItemList = new List<ELCRTestType>();

			this._supportMsrtSpeed = new List<ELCRMsrtSpeed>();

            this._cableLengthList = new List<string> ();

            this._caliDataQty = 1;
		}

		public LCRItemDescription(string propertyKeyName, double minValue, double maxValue, double defaultValue, string unit, string format)
			: base(propertyKeyName, minValue, maxValue, defaultValue, unit, format)
		{
			this._supportMserItemList = new List<ELCRTestType>();

            this._caliItemList = new List<ELCRTestType>();

			this._supportMsrtSpeed = new List<ELCRMsrtSpeed>();

            

            this._cableLengthList = new List<string>();

            this._caliDataQty = 1;
		}

		public List<ELCRTestType> SupportTestItemList
		{
			get { return this._supportMserItemList; }
		}

        public List<ELCRTestType> CaliTypeList
        {
            get { return this._caliItemList; }
        }

		public List<ELCRMsrtSpeed> SupportMsrtSpeed
		{
			get { return this._supportMsrtSpeed; }
		}
        public List<string> CableLengthList
        {
            get { return this._cableLengthList; }
        }
        public uint CaliDataQty
        {
            get { return _caliDataQty; }
            set { this._caliDataQty = value; }
        }

        //public List<LCRCaliData> CaliDataList
        //{
        //    get { return this._caliDataList; }
        //}

	}



    [Serializable]
    public class IOItemDescription : ItemDescriptionBase
    {
        private List<EIOTrig_Mode> _supportMdoe;

        private List<EIOState> _supportState;

        private int _ioQty;


        private uint _caliDataQty;

        public IOItemDescription()
            : base()
        {
            this._supportMdoe = new List<EIOTrig_Mode>();

            this._supportState = new List<EIOState>();

            _ioQty = 14;//K2611

            foreach (string name in Enum.GetNames(typeof(EIOTrig_Mode)))
            {
                EIOTrig_Mode mode = (EIOTrig_Mode)Enum.Parse(typeof(EIOTrig_Mode), name, false);
                _supportMdoe.Add(mode);
            }
            foreach (string name in Enum.GetNames(typeof(EIOState)))
            {
                EIOState state = (EIOState)Enum.Parse(typeof(EIOState), name, false);
                _supportState.Add(state);
            }
        }

        public IOItemDescription(string propertyKeyName, double minValue, double maxValue, double defaultValue, string unit, string format)
            : base(propertyKeyName, minValue, maxValue, defaultValue, unit, format)
        {
            this._supportMdoe = new List<EIOTrig_Mode>();

            this._supportState = new List<EIOState>();
            _ioQty = 14;//K2611

            foreach (string name in Enum.GetNames(typeof(EIOTrig_Mode)))
            {
                EIOTrig_Mode mode = (EIOTrig_Mode)Enum.Parse(typeof(EIOTrig_Mode), name, false);
                _supportMdoe.Add(mode);
            }
            foreach (string name in Enum.GetNames(typeof(EIOState)))
            {
                EIOState state = (EIOState)Enum.Parse(typeof(EIOState), name, false);
                _supportState.Add(state);
            }

            
        }

        public List<EIOTrig_Mode> ModeList
        {
            get { return this._supportMdoe; }
        }

        public List<EIOState> StateList
        {
            get { return this._supportState; }
        }

        public int IOQty
        {
            get { return _ioQty; }
            set { this._ioQty = value; }
        }

        //public List<LCRCaliData> CaliDataList
        //{
        //    get { return this._caliDataList; }
        //}

    }
}
