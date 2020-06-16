using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MPI.Tester.Data
{
    [Serializable]
    public class TestResultData : ICloneable
    {
        private object _lockObj;

        private string _id;
        private string _keyName;
        private string _name;

        private double _value;
        private double _rawValue;
        private List<float> _dataList;
        private double[] _rawValueArray;

        private string _formate;
        private string _unit;

        private bool _isEnable;
		private bool _isVision;
		private bool _isVerify;
		private bool _isSkip;
		private bool _isTested;
        private bool _isGroupSkip;

        private double _maxLimitValue;
        private double _minLimitValue;
		private double _maxLimitValue02;
		private double _minLimitValue02;
		private double _maxLimitValue03;
		private double _minLimitValue03;

        private int _binGrade;

		private double _tempValue;

		private bool _enableColorOutOfRange;

        private bool _enableAdjacent;

        private double _adjacentRange;

        private int _adjacentType;

        private bool _isEnableMapItem;
        private bool _isSystemItem;
		
        private bool _enableSamplingMonitor;

        private double _samplingMonitorRange;

        private int _samplingMonitorType;

        private int _minPassRatePercent;

        private bool _isEnablePassRateCheck;

        private int _index;

		private bool _isOnlyTestMVFLA;

        private bool _volatile;

        public TestResultData()
        {
            this._lockObj = new object();

            this._id = string.Empty;
            this._keyName = string.Empty;
            this._name = string.Empty;

            this._value = 0.0d;
            this._rawValue = 0.0d;
            this._dataList = new List<float>();
            this._rawValueArray = null;
			this._formate = "0.00000";
			this._unit = string.Empty;

			this._isEnable = true;
			this._isVision = true;
			this._isVerify = false;
			this._isSkip = false;
			this._isTested = false;
            this._isGroupSkip = false;

			this._binGrade = -1;

			this._maxLimitValue = 9999.9999d;
			this._minLimitValue = -9999.9999d;

			this._maxLimitValue02 = 9999.9999d;
			this._minLimitValue02 = -9999.9999d;

			this._maxLimitValue03 = 9999.9999d;
			this._minLimitValue03 = -9999.9999d;

			this._enableColorOutOfRange = true;

            this._adjacentRange = 0.0d;
            this._enableAdjacent = false;
            this._adjacentType = 0;

            this._isEnableMapItem = true;
            this._isSystemItem = false;
			this._enableSamplingMonitor = false;

            this._samplingMonitorRange = 0.0d;

            this._samplingMonitorType = 0;

            _isEnablePassRateCheck = false;

            _minPassRatePercent = 0;

            _index = 0;

			this._isOnlyTestMVFLA = false;

            BoundaryRule = EBinBoundaryRule.LeValL;

            IsThisItemTested = true;

            Volatile = true;
        }
        
		public TestResultData(string unit, string formatStr)
			: this()
        {
            this._unit = unit;
            this._formate = formatStr;
        }

		public TestResultData(string keyName, string name, string unit, string formatStr)
			: this(unit, formatStr)
        {
            this._keyName = keyName;
            this._name = name;
        }

        #region >>> Public Property <<<

        /// <summary>
        /// ID number of test result
        /// </summary>
        public string ID
        {
            get { return this._id; }
            set { lock (this._lockObj) { this._id = value; } }
        }

        public string KeyName
        {
            get { return this._keyName; }
            set { lock ((this._lockObj)) { this._keyName = value; } }
        }

        public string Name
        { 
            get { return this._name; }
            set { lock ((this._lockObj)) { this._name = value; } } 
        }

        public double Value
        {
            get { return this._value; }
            set { lock ((this._lockObj)) { this._value = value; } } 
        }
        
        public double RawValue
        {
            get { return this._rawValue; }
            set { lock ((this._lockObj)) { this._rawValue = value; } }
        }

        public List<float> DataList
        {
            get { return this._dataList; }
            set { lock ((this._lockObj)) { this._dataList = value; } }
        }

        public double[] RawValueArray
        {
            get { return this._rawValueArray; }
            set { lock ((this._lockObj)) { this._rawValueArray = value; } }
        }

        public string Formate
        {
            get { return this._formate; }
            set { lock ((this._lockObj)) { this._formate = value; } }
        }

        public string Unit
        {
            get { return this._unit; }
            set { lock ((this._lockObj)) { this._unit = value; } }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock ((this._lockObj)) { this._isEnable = value; } }
        }

		public bool IsVision
		{
			get { return this._isVision; }
			set { lock ((this._lockObj)) { this._isVision = value; } }
		}

		public bool IsVerify
		  {
			  get { return this._isVerify; }
			  set { lock ((this._lockObj)) { this._isVerify = value; } }
		  }

		public bool IsSkip
		  {
			  get { return this._isSkip; }
			  set { lock ((this._lockObj)) { this._isSkip = value; } }
		  }

		public bool IsTested
		{
			get { return this._isTested; }
			set { lock ((this._lockObj)) { this._isTested = value; } }
		}

        public bool IsGroupSkip
        {
            get { return this._isGroupSkip; }
            set { lock ((this._lockObj)) { this._isGroupSkip = value; } }
        }

        public bool IsPassAndIsTested
        {
			get
			{
				if (this._isVerify == false || this._isTested == false)
					{
						return true;
					}
					else
					{
						this._tempValue = Convert.ToDouble(this._value.ToString(this._formate));
						if (this._tempValue >= this._minLimitValue && this._tempValue <= this._maxLimitValue)
						{				    
							return true;
						}
						else
						{
							return false;
						}
					}
				}
        }

		public bool IsPass
		{
			get
			{
               // if (this._isVerify == false || this._isTested == false)

				if (this._isVerify == false)
				{
					return true;
				}
				else if (this._isOnlyTestMVFLA && !this._keyName.Contains(EOptiMsrtType.MVFLA.ToString()))
				{
					return true;
				}
				else
				{
                    this._tempValue = Convert.ToDouble(this._value.ToString(this._formate));
                    return IsInSpec(_tempValue);

					
                    //if (this._tempValue >= this._minLimitValue && this._tempValue <= this._maxLimitValue)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
				}
			}
		}

        public bool IsPass02
        {
            get
            {
				if (this._isVerify == false || this._isTested == false)
                {
                    return true;
                }
				else if (this._isOnlyTestMVFLA && !this._keyName.Contains(EOptiMsrtType.MVFLA.ToString()))
				{
					return true;
				}
                else
                {
                    this._tempValue = Convert.ToDouble(this._value.ToString(this._formate));
                    if (this._tempValue >= this._minLimitValue02 && this._tempValue <= this._maxLimitValue02)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

		public bool IsPass03
		{
			get
			{
				if (this._isVerify == false || this._isTested == false)
				{
					return true;
				}
				else if (this._isOnlyTestMVFLA && !this._keyName.Contains(EOptiMsrtType.MVFLA.ToString()))
				{
					return true;
				}
				else
				{
					this._tempValue = Convert.ToDouble(this._value.ToString(this._formate));
					if (this._tempValue >= this._minLimitValue03 && this._tempValue <= this._maxLimitValue03)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}

        public int BinGrade
        {
            get { return this._binGrade; }
            set { lock ((this._lockObj)) { this._binGrade = value; } }       
        }

        public double MaxLimitValue 
        {
            get { return this._maxLimitValue; }
            set { lock ((this._lockObj)) { this._maxLimitValue = value; } }
        }

        public double MinLimitValue
        {
            get { return this._minLimitValue; }
            set { lock ((this._lockObj)) { this._minLimitValue = value; } }
        }

        public double MaxLimitValue2
        {
            get { return this._maxLimitValue02; }
            set { lock ((this._lockObj)) { this._maxLimitValue02 = value; } }
        }

        public double MinLimitValue2
        {
            get { return this._minLimitValue02; }
            set { lock ((this._lockObj)) { this._minLimitValue02 = value; } }
        }

		public double MaxLimitValue3
		{
			get { return this._maxLimitValue03; }
			set { lock ((this._lockObj)) { this._maxLimitValue03 = value; } }
		}

		public double MinLimitValue3
		{
			get { return this._minLimitValue03; }
			set { lock ((this._lockObj)) { this._minLimitValue03 = value; } }
		}

		public bool EnableColorOutOfRange
		{
			get { return this._enableColorOutOfRange; }
			set { lock (this._lockObj) { this._enableColorOutOfRange = value; } }
		}

        public bool EnableAdjacent
        {
            get { return this._enableAdjacent; }
            set { lock (this._lockObj) { this._enableAdjacent = value; } }
        }

        public double AdjacentRange
        {
            get { return this._adjacentRange; }
            set { lock (this._lockObj) { this._adjacentRange = value; } }
        }

        public int AdjacentType
        {
            get { return this._adjacentType; }
            set { lock (this._lockObj) { this._adjacentType = value; } }
        }

        public bool IsEnableMapItem
        {
            get { return this._isEnableMapItem; }
            set { lock (this._lockObj) { this._isEnableMapItem = value; } }
        }

        public bool IsSystemItem
        {
            get { return this._isSystemItem; }
            set { lock (this._lockObj) { this._isSystemItem = value; } }
        }

        public bool EnableSamplingMonitor
        {
            get { return this._enableSamplingMonitor; }
            set { lock (this._lockObj) { this._enableSamplingMonitor = value; } }
        }

        public double SamplingMonitorRange
        {
            get { return this._samplingMonitorRange; }
            set { lock (this._lockObj) { this._samplingMonitorRange = value; } }
        }

        public int SamplingMonitorType
        {
            get { return this._samplingMonitorType; }
            set { lock (this._lockObj) { this._samplingMonitorType = value; } }
        }

        public bool IsEnablePassRateCheck
        {
            get { return this._isEnablePassRateCheck; }
            set { lock (this._lockObj) { this._isEnablePassRateCheck = value; } }
        }

        public int MinPassRatePercent
        {
            get { return this._minPassRatePercent; }
            set { lock (this._lockObj) { this._minPassRatePercent = value; } }
        }

        public int Index
        {
            get { return this._index; }
            set { lock (this._lockObj) { this._index = value; } }
        }

		public bool IsOnlyTestMVFLA
		{
			get { return this._isOnlyTestMVFLA; }
			set { lock (this._lockObj) { this._isOnlyTestMVFLA = value; } }
		}

        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public EBinBoundaryRule BoundaryRule { set; get; } //20181210 David NG Bin Boundary Rule follow by TestResultItem

        [System.Xml.Serialization.XmlIgnore]
        public bool IsThisItemTested
        { get; set; }
        /// <summary>
        /// 定義下一次點測前是否要原始數值歸0，用來記錄LaserPower監控值等與SOT啟動頻率不同得的測試項目用
        /// </summary>
        public bool Volatile
        {
            get { return this._volatile; }
            set { lock (this._lockObj) { this._volatile = value; } }
        }

        #endregion

        #region >>> Public Method <<<
        public void ResetResult(double val = 0.0)
        {
            this.IsTested = false;
            if (Volatile)
            {
                this.RawValue = val;
                this.Value = val;

                if (RawValueArray != null)
                {
                    for (int i = 0; i < RawValueArray.Length; ++i)
                    {
                        RawValueArray[i] = val;
                    }
                    
                }
                
            }
        }

        public bool IsInSpec(double val)
        {
            return IsInSpec(val.ToString(this._formate));
        }

        public bool IsInSpec(string valStr)
        {
            double val = Convert.ToDouble(valStr);
            if (this._isVerify )
            {
                switch(BoundaryRule)
                {
                    default:
                    case EBinBoundaryRule.LeValL:
                        {
                            if (this._minLimitValue  <= val && val < this._maxLimitValue)
                            {
                                return true;
                            }
                        }
                        break;
                    case EBinBoundaryRule.LeValLe:
                        {
                            if (this._minLimitValue <= val && val <= this._maxLimitValue)
                            {
                                return true;
                            }
                        }
                        break;
                    case EBinBoundaryRule.LValL:
                        {
                            if (this._minLimitValue < val && val < this._maxLimitValue)
                            {
                                return true;
                            }
                        }
                        break;
                    case EBinBoundaryRule.LValLe:
                        {
                            if (this._minLimitValue < val && val <= this._maxLimitValue)
                            {
                                return true;
                            }
                        }
                        break;
                }
                return false;
                
            }
            else
            {
                return true;
            }
            

        }

        public Dictionary<string, object> GetTestResultDataInfo()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("ID", ID);
            dic.Add("KeyName", KeyName);
            dic.Add("Name", Name);
            dic.Add("Formate", Formate);
            dic.Add("Unit", Unit);
            dic.Add("IsEnable", IsEnable);
            dic.Add("IsVision", IsVision);
            dic.Add("IsVerify", IsVerify);
            dic.Add("IsSkip", IsSkip);
            dic.Add("MaxLimitValue", MaxLimitValue);
            dic.Add("MinLimitValue", MinLimitValue);
            dic.Add("IIsSystemItemD", IsSystemItem);
            dic.Add("Index", Index);

            dic.Add("BoundaryRule", BoundaryRule.ToString());
            dic.Add("Volatile", Volatile);


            return dic;

        }

        public object Clone()
        {
			//--------------------------------
			// All field is value type
			//--------------------------------
            
			TestResultData cloneObj = (TestResultData)this.MemberwiseClone();

            if (this._rawValueArray != null)
            {
                Array.Copy(this._rawValueArray, cloneObj._rawValueArray, this._rawValueArray.Length);
            }

			return cloneObj;
        }

        #endregion
    }
}
