using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MPI.Tester.Data;

namespace MPI.Tester.Tools
{
    [Serializable]
    public class DeviceVerifySpec : ICloneable
    {
        private object _lockObj;

        private List<DeviceVerifySpecData> _dataList;

        private DateTime _biasRegisterDate;

        public DeviceVerifySpec()
        {
            this._lockObj = new object();

            this._dataList = new List<DeviceVerifySpecData>();
        }

        #region >>> Public Property <<<

        public DateTime BiasRegisterDate
        {
            get { return this._biasRegisterDate; }
            set { lock (this._lockObj) { this._biasRegisterDate = value; } }
        }

        public List<DeviceVerifySpecData> Data
        {
            get { return this._dataList; }
            set { lock (this._lockObj) { this._dataList = value; } }
        }

        public DeviceVerifySpecData this[string keyName]
        {
            get 
            {
                if (this._dataList == null || this._dataList.Count == 0)
                {
                    return null;
                }
                
                foreach (DeviceVerifySpecData item in this._dataList)
                {
                    if (item.KeyName == keyName)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public DeviceVerifySpecData this[int index]
        {
            get
            {
                if (this._dataList == null || this._dataList.Count == 0 || index >= this._dataList.Count )
                {
                    return null;
                }

                return this._dataList[index];
            }
        }

        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            DeviceVerifySpec cloneObj = new DeviceVerifySpec();

            cloneObj._dataList = new List<DeviceVerifySpecData>();

            foreach (DeviceVerifySpecData item in this._dataList)
            {
                cloneObj._dataList.Add(item.Clone() as DeviceVerifySpecData);
            }

            return cloneObj;
        }

        #endregion
    }

    [Serializable]
    public class DeviceVerifySpecData : ICloneable
    {
        private object _lockObj;
        private bool _isEnable;
        private string _keyName;
        private string _name;
        private double _bias;
        private double _spec;
        private string _unit;
        private string _format;

        private double _avgValue;
        private double _maxValue;
        private double _minValue;
        private double _diff;
        private EGainOffsetType _type;
        private double _gain;
        private double _offset;
        private bool _isPass;

        public DeviceVerifySpecData()
        {
            this._lockObj = new object();
            this._isEnable = false;
            this._keyName = string.Empty;
            this._name = string.Empty;
            this._bias = 0.0d;
            this._spec = 0.0d;
            this._unit = string.Empty;

            this._format = "0.000";

            this._avgValue = 0.0d;
            this._type = EGainOffsetType.None;
            this._gain = 1.0d;
            this._offset = 0.0d;
            this._diff = 0.0d;
            this._isPass = false;

        }

        public DeviceVerifySpecData(string keyName, string name, string format, string unit) : this()
        {
            this._keyName = keyName;
            this._name = name;
            this._format = format;
            this._unit = unit;
        }

        #region >>> Public Property <<<

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }

        public double Spec
        {
            get { return this._spec; }
            set { lock (this._lockObj) { this._spec = value; } }
        }

        public double Bias
        {
            get { return this._bias; }
            set { lock (this._lockObj) { this._bias = value; } }
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

        [XmlIgnore]
        public double ResultAverageValue
        {
            get { return this._avgValue; }
            set { lock (this._lockObj) { this._avgValue = value; } }
        }

        [XmlIgnore]
        public double ResultMaxValue
        {
            get { return this._maxValue; }
            set { lock (this._lockObj) { this._maxValue = value; } }
        }

        [XmlIgnore]
        public double ResultMinValue
        {
            get { return this._minValue; }
            set { lock (this._lockObj) { this._minValue = value; } }
        }

        public EGainOffsetType Type
        {
            get { return this._type; }
            set { lock (this._lockObj) { this._type = value; } }
        }

        [XmlIgnore]
        public double Diff
        {
            get { return this._diff; }
            set { lock (this._lockObj) { this._diff = value; } }
        }

        public double Gain
        {
            get { return this._gain; }
            set { lock (this._lockObj) { this._gain = value; } }
        }

        public double Offset
        {
            get { return this._offset; }
            set { lock (this._lockObj) { this._offset = value; } }
        }

        [XmlIgnore]
        public bool IsPass
        {
            get { return this._isPass; }
            set { lock (this._lockObj) { this._isPass = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public bool Calculate()
        {
            this._diff = 0.0d;

            this._isPass = false;

            if (this._keyName.Contains(EOptiMsrtType.LM.ToString()) ||
                this._keyName.Contains(EOptiMsrtType.WATT.ToString()) ||
                this._keyName.Contains(EOptiMsrtType.LOP.ToString()))
            {
                this._diff =  (this._bias / this._avgValue) -1.0d;
            }
            else
            {
                this._diff = this._bias - this._avgValue;
            }

            if (Math.Abs(this._diff) <= this._spec)
            {
                this._isPass = true;
            }
            
            return true;
        }

        public void SetCoefficient()
        {
            if (this._keyName.Contains(EOptiMsrtType.LM.ToString()) ||
               this._keyName.Contains(EOptiMsrtType.WATT.ToString()) ||
               this._keyName.Contains(EOptiMsrtType.LOP.ToString()))
            {
                if (this._avgValue == 0.0d)
                {
                    this._gain = 1.0d;
                }
                else
                {
                    this._gain = this._bias / this._avgValue;
                }
            }
            else
            {
                this._offset = this._bias - this._avgValue;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone() as DeviceVerifySpecData;
        }

        #endregion
    }
}
