using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace MPI.Tester.Data
{
    public enum EWMTestMode : int
    {
        FullyTest				= 0,
        SampleTest			= 1,
        ESDTest	            = 2,
        EngineerTest			= 3,
    }


    public class WMData : ICloneable
    {
        private object _lockObj;

		private string _outputFileName;
		private string _keyInFileName;

		private string _productName;
		private string _deviceNumber;
		private string _specification;
		private string _specificationRemark;

		private string _sampleBins;
		private string _sampleStandard;
		private string _sampleLevel;
		private string _totalTested;
		private string _samples;

        private string _remark01;
        private string _remark02;
        private string _remark03;
        private string _remark04;

		private string _customerID;
		private string _customer;
        private string _customerNote01;
        private string _customerNote02;
        private string _customerNote03;
		private string _customerNote04;
        private string _customerRemark01;

		private string _lotNumber;
		private string _classNumber;
		private string _codeNumber;
		private string _serialNumber;

        private int _wmTestMode;

		private string _wm_MES_Path01;
		private string _wm_MES_Path02;
		private string _wm_MES_FileExt01;
		private string _wm_MES_FileExt02;
		private string _wm_MES_Char01;
		private string _wm_MES_Char02;

        private bool _isAutoResetData;

        /// <summary>
        /// Construnctor
        /// </summary>
        public WMData()
        {
            this._lockObj = new object();

			this._outputFileName = "WM_OutputFileName";
			this._deviceNumber = string.Empty;
			this._specification = string.Empty;
			this._specificationRemark = string.Empty;

			this._sampleBins = "ALL";
			this._sampleStandard = "WEIMIN";
			this._sampleLevel = "STANDARD";
			this._totalTested = "999999";
			this._samples = "999999";

            this._remark01 = string.Empty;
            this._remark02 = string.Empty;
            this._remark03 = string.Empty;
            this._remark04 = string.Empty;

			this._customerID = string.Empty;
			this._customer = string.Empty;
            this._customerNote01 = string.Empty;
            this._customerNote02 = string.Empty;
            this._customerNote03 = string.Empty;
			this._customerNote04 = string.Empty;
            this._customerRemark01 = string.Empty;

			this._classNumber = string.Empty;
			this._codeNumber = string.Empty;
			this._serialNumber = string.Empty;

            this._wmTestMode = 0;

			this._wm_MES_Path01 = @"C:\";
			this._wm_MES_Path02 = @"C:\";
			this._wm_MES_FileExt01 = "txt";
			this._wm_MES_FileExt02 = "csv";
			this._wm_MES_Char01 = "-";
			this._wm_MES_Char02 = "-";

            this._isAutoResetData = false;

        }

        #region >>> Public Property <<<

		public string OutputFileName
		{
			get { return this._outputFileName; }
			set { lock (this._lockObj) { this._outputFileName = value; } }
		}

		public string KeyInFileName
		{
			get { return this._keyInFileName; }
			set { lock (this._lockObj) { this._keyInFileName = value; } }
		}

		public string ProductName
		{
			get { return this._productName; }
			set { lock (this._lockObj) { this._productName = value; } }
		}

		public string DeviceNumber
		{
			get { return this._deviceNumber; }
			set { lock (this._lockObj) { this._deviceNumber = value; } }
		}

		public string Specification
		{
			get { return this._specification; }
			set { lock (this._lockObj) { this._specification = value; } }
		}

		public string SpecificationRemark
		{
			get { return this._specificationRemark; }
			set { lock (this._lockObj) { this._specificationRemark = value; } }
		}

		public string SampleBins
		{
			get { return this._sampleBins; }
			set { lock (this._lockObj) { this._sampleBins = value; } }
		}

		public string SampleStandard
		{
			get { return this._sampleStandard; }
			set { lock (this._lockObj) { this._sampleStandard = value; } }
		}

		public string SampleLevel
		{
			get { return this._sampleLevel; }
			set { lock (this._lockObj) { this._sampleLevel = value; } }
		}

		public string TotalTested
		{
			get { return this._totalTested; }
			set { lock (this._lockObj) { this._totalTested = value; } }
		}

		public string Samples
		{
			get { return this._samples; }
			set { lock (this._lockObj) { this._samples = value; } }
		}

        public string Remark01
        {
            get { return this._remark01; }
            set { lock (this._lockObj) { this._remark01 = value; } }
        }

        public string Remark02
        {
            get { return this._remark02; }
            set { lock (this._lockObj) { this._remark02 = value; } }
        }

        public string Remark03
        {
            get { return this._remark03; }
            set { lock (this._lockObj) { this._remark03 = value; } }
        }

        public string Remark04
        {
            get { return this._remark04; }
            set { lock (this._lockObj) { this._remark04 = value; } }
        }

        public string CustomerID
        {
            get { return this._customerID; }
			set { lock (this._lockObj) { this._customerID = value; } }
        }

		public string Customer
        {
            get { return this._customer; }
			set { lock (this._lockObj) { this._customer = value; } }
        }

        public string CustomerNote01
        {
            get { return this._customerNote01; }
            set { lock (this._lockObj) { this._customerNote01 = value; } }
        }

        public string CustomerNote02
        {
            get { return this._customerNote02; }
            set { lock (this._lockObj) { this._customerNote02 = value; } }
        }

        public string CustomerNote03
        {
            get { return this._customerNote03; }
            set { lock (this._lockObj) { this._customerNote03 = value; } }
        }

		public string CustomerNote04
		{
			get { return this._customerNote04; }
			set { lock (this._lockObj) { this._customerNote04 = value; } }
		}

        public string CustomerRemark01
        {
            get { return this._customerRemark01; }
            set { lock (this._lockObj) { this._customerRemark01 = value; } }
        }

		public string LotNumber
		{
			get { return this._lotNumber; }
			set { lock (this._lockObj) { this._lotNumber = value; } }
		}

		public string ClassNumber
		{
			get { return this._classNumber; }
			set { lock (this._lockObj) { this._classNumber = value; } }
		}

		public string CodeNumber
		{
			get { return this._codeNumber; }
			set { lock (this._lockObj) { this._codeNumber = value; } }
		}

		public string SerialNumber
		{
			get { return this._serialNumber; }
			set { lock (this._lockObj) { this._serialNumber = value; } }
		}		

        public int WMTestMode
        {
            get { return this._wmTestMode; }
            set { lock (this._lockObj) { this._wmTestMode = value; } }
        }

		public string WM_MES_Path01
		{
			get { return this._wm_MES_Path01; }
			set { lock (this._lockObj) { this._wm_MES_Path01 = value; } }
		}

		public string WM_MES_Path02
		{
			get { return this._wm_MES_Path02; }
			set { lock (this._lockObj) { this._wm_MES_Path02 = value; } }
		}

		public string WM_MES_FileExt01
		{
			get { return this._wm_MES_FileExt01; }
			set { lock (this._lockObj) { this._wm_MES_FileExt01 = value; } }
		}

		public string WM_MES_FileExt02
		{
			get { return this._wm_MES_FileExt02; }
			set { lock (this._lockObj) { this._wm_MES_FileExt02 = value; } }
		}

		public string WM_MES_Char01
		{
			get { return this._wm_MES_Char01; }
			set { lock (this._lockObj) { this._wm_MES_Char01 = value; } }
		}

		public string WM_MES_Char02
		{
			get { return this._wm_MES_Char02; }
			set { lock (this._lockObj) { this._wm_MES_Char02 = value; } }
		}

        public bool IsAutoResetData
        {
            get { return this._isAutoResetData; }
            set { lock (this._lockObj) { this._isAutoResetData = value; } }
        }

        #endregion  

		#region >>> Public Method <<<

		public object Clone()
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, this);
			ms.Position = 0;
			object obj = bf.Deserialize(ms);
			ms.Close();

			return obj;
		}

		public void ResetAllData()
		{
            if (_isAutoResetData)
            {
                this._keyInFileName = "";
                this._outputFileName = "";

                this._productName = "";
                this._deviceNumber = "";
                this._specification = "";
                this._specificationRemark = "";

                //this._sampleBins;
                //this._sampleStandard;
                //this._sampleLevel;
                //this._totalTested;
                //this._samples;

                this._remark01 = "";
                this._remark02 = "";
                this._remark03 = "";
                this._remark04 = "";

                this._customerID = "";
                this._customer = "";
                this._customerNote01 = "" ;
                this._customerNote02 = "";
                this._customerNote03 = "";
				this._customerNote04 = "";
                //this._customerRemark01 = "";

                this._lotNumber = "";
                //	this._classNumber = "";
                //	this._codeNumber = "";
                //	this._serialNumber ="" ;
            }
		}

		#endregion
    }

    public class CustomizeDeifneOutputPath
    {
        private object _lockObj;

        public bool  IsEanble;

        public bool IsEnableCheckMinYieldRate;

        public bool IsEnableCheckMinProbingChips;

        public bool IsEnableCheckContinousNGChips;

        public int MinYieldRate;

        public int MinProbingChips;

        public int ContinousNGChips;

        public string PassOutputPath;

        public string NGOutputPath;

        private bool _isPass;

        private string _targetOutputPath;

        public CustomizeDeifneOutputPath()
        {
            this._lockObj = new object();

            IsEanble = false;

            IsEnableCheckMinYieldRate = false;

            IsEnableCheckMinProbingChips = false;

            IsEnableCheckContinousNGChips = false;

            MinYieldRate = 0;

            MinProbingChips = 10000;

            ContinousNGChips = 50;

            PassOutputPath = Constants.Paths.LEDTESTER_TEMP_DIR;

            NGOutputPath = Constants.Paths.LEDTESTER_TEMP_DIR;

            this._isPass = true;

        }


        public void Reset()
        {
            this._isPass = true;
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsPass
        {
            get { return this._isPass; }
            set { lock (this._lockObj) { this._isPass = value; } }
        }

        public string TargetOutputPath
        {
            get
            {
                if (this.IsEanble & this._isPass)
                {
                    return PassOutputPath;
                }
                else
                {
                    return NGOutputPath;

                }
            }
        }

        public void CheckContinousNGChips(int NGChips)
        {
            if (this.IsEanble && this.IsEnableCheckContinousNGChips)
            {
                if (NGChips >= ContinousNGChips)
                {
                    this._isPass = false;
                }
            }
        }

        public void CheckYieldRate(double yieldRate)
        {
            if (this.IsEanble && this.IsEnableCheckMinYieldRate)
            {
                if (yieldRate < MinYieldRate)
                {
                    this._isPass = false;
                }
            }
        }

        public void CheckMinProbingChips(int chipCounts)
        {
            if (this.IsEanble && this.IsEnableCheckMinProbingChips)
            {
                if (chipCounts < MinProbingChips)
                {
                    this._isPass = false;
                }
            }
        }

    }

    public class LIVCurveDrawSetting
    {
        private object _lockObj;

        private string _chartAxAxisName;

        private string _chartAyAxisName;

        private string _chartBxAxisName;

        private string _chartByAxisName;

        public LIVCurveDrawSetting()
        {
            _lockObj=new object();

            _chartAxAxisName = string.Empty;

            _chartAyAxisName = string.Empty;

            _chartBxAxisName = string.Empty;

            _chartByAxisName = string.Empty;
        }


        public string ChartA_xAxisName 
        {
            get { return this._chartAxAxisName; }
            set { lock (this._lockObj) { this._chartAxAxisName = value; } }
        }

        public string ChartA_yAxisName
        {
            get { return this._chartAyAxisName; }
            set { lock (this._lockObj) { this._chartAyAxisName = value; } }
        }

        public string ChartB_xAxisName
        {
            get { return this._chartBxAxisName; }
            set { lock (this._lockObj) { this._chartBxAxisName = value; } }
        }

        public string ChartB_yAxisName
        {
            get { return this._chartByAxisName; }
            set { lock (this._lockObj) { this._chartByAxisName = value; } }
        }
           
    }

}
