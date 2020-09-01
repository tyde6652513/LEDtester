using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

using MPI.Tester.Data.CalibrateData;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class ProductData : ICloneable
	{
		private object _lockObj;

		private bool _isSingleLOPItem;
		private ELOPSaveItem _lopSaveItem;
		private int _lopSaveItemNumber;
		private ConditionData _testCondition;

		private double _dispCoefStartWL;
		private double _dispCoefEndWL;

		private uint _productFilterWheelPos;

		private GainOffsetData[][]  _chuckLOPCorrectArray;
		private GainOffsetData[] _productCorrectArray;
		private GainOffsetData[] _customerCorrectArray;

        private ByCustomerSetting _ByCustomerSetting;

        private string _sptCalibPathAndFile;  //
		private double[] _productSptXaxisCoef;  // 4 parameter
        private double[] _productSptYintCoef;    // Relative ==> Abs
        private double[] _productSptYweight;   // Abs ==> Abs
        private double _resistance;
        private double[] _chuckResistanceCorrectArray;
        private uint _adjacentConsecutiveErrorCount;
		private string _barcodePrintFormat;
        private uint _LOPWLSkipCount;
        private string _testResultPathByTaskSheet;

        private int _adjacentStartingCount;

        private int _adjacentStopCount;

        private bool _isTVSProduct;

        private string _outputFileFormat;

		private bool _adjacentAllItemPassCheck;

		private uint _samplingMonitorConsecutiveErrCount;

		private uint _samplingMonitorMode;

		private CalibrateChipValue _caliChipValue;

        private bool _isApplySysCoef;

		private bool _isJudgeFailSkipESDItem;

        private double _pdDectorFactor;

        private RISpec _riSpec;
        private int _xLineSubBinSampleCH;
        private int _yLineSubBinSampleCH;

        private LaserSourceSysSettingData _laserSrcSetting;

        private string _productName;

        



		public ProductData()
		{
			this._lockObj = new object();
			
			this._isSingleLOPItem = false;
			this._lopSaveItemNumber = 0;
			this._testCondition = new ConditionData();
			this._dispCoefStartWL = 400.0d;
			this._dispCoefEndWL = 600.0d;
			this._productFilterWheelPos = 0;

            this._adjacentStartingCount = 500;

            this._adjacentStopCount = 500;

			this._chuckLOPCorrectArray = new GainOffsetData[20][];

            this._ByCustomerSetting = new ByCustomerSetting();

            for (int m = 0; m < this._chuckLOPCorrectArray.Length; m++)
            {
                this._chuckLOPCorrectArray[m] = new GainOffsetData[3];
                for (int n = 0; n < this._chuckLOPCorrectArray[0].Length; n++)
                {
                    this._chuckLOPCorrectArray[m][n] = new GainOffsetData(true, EGainOffsetType.GainAndOffest);
                    if (n == 0)
                    {
                        this._chuckLOPCorrectArray[m][n].KeyName = "CKLOPGAIN_" + (n + 1).ToString();
                    }
					else if (n == 1)
                    {
                        this._chuckLOPCorrectArray[m][n].KeyName = "CKWATTGAIN_" + (n + 1).ToString();
                    }
                    else if (n == 2)
                    {
                        this._chuckLOPCorrectArray[m][n].KeyName = "CKLMGAIN_" + (n + 1).ToString();
                    }


                    this._chuckLOPCorrectArray[m][n].Name = "Chuck" + (n + 1).ToString("00");
                }
            }

            //  Spt Calib By Product
            this._sptCalibPathAndFile = "";
            this._productSptXaxisCoef = new double[4] { 0, 0, 0, 0 };
            this._productSptYintCoef = new double[2048];
            this._productSptYweight = new double[2048];


            for (int i = 0; i < _productSptYintCoef.Length; i++)
            {
                this._productSptYintCoef[i] = 0;
                this._productSptYweight[i] = 0;
            }

            this._resistance = 0;
            this._chuckResistanceCorrectArray = new double[20];
            this._adjacentConsecutiveErrorCount = 20;
			this._barcodePrintFormat = string.Empty;
            this._LOPWLSkipCount = 0;

            this._testResultPathByTaskSheet = Constants.Paths.MPI_TEMP_DIR;

            this._isTVSProduct = false;

            this._outputFileFormat = string.Empty;

			this._adjacentAllItemPassCheck = false;

            this._samplingMonitorConsecutiveErrCount = 0;

            this._samplingMonitorMode = 0;

			this._caliChipValue = new CalibrateChipValue();

            this._isApplySysCoef = false;

			this._isJudgeFailSkipESDItem = false;

            this._pdDectorFactor = 1.0d;
			this._riSpec = new RISpec();

            this._xLineSubBinSampleCH = 1;
            this._yLineSubBinSampleCH = 1;

            _laserSrcSetting = new LaserSourceSysSettingData();

            _productName = "";

            PathManager = null;

            
		}

        #region >>> Public property <<<

        public ConditionData TestCondition
		{
			get { return this._testCondition; }
			set { lock (this._lockObj) { this._testCondition = value; } }	
		}

		public bool IsSingleLOPItem
		{
			get { return this._isSingleLOPItem; }
			//set { lock (this._lockObj) { this._isSingleLOPItem = value; } }
		}

		[XmlIgnore]
		public ELOPSaveItem LOPSaveItem
        {
            get
            {
                if (Enum.IsDefined(typeof(ELOPSaveItem), this._lopSaveItemNumber))
                {
                    return ((ELOPSaveItem)this._lopSaveItemNumber);
                }
                else
                {
                    return ELOPSaveItem.watt;
                }
            }

			set
			{
					lock (this._lockObj) 
					{ 
						this._lopSaveItem = value;
						this._lopSaveItemNumber = (int)this._lopSaveItem;
					} 
				}	
		}

		public int LOPSaveItemNumber
		{
			get { return this._lopSaveItemNumber; }
			set
			{
					lock (this._lockObj)
					{
                        this._lopSaveItemNumber = value;

                        if (Enum.IsDefined(typeof(ELOPSaveItem), value))
                        {
                            this._lopSaveItem = (ELOPSaveItem)value;
                        }
                        else
                        {
                            this._lopSaveItem = ELOPSaveItem.watt;
                        }
					}
				}
		}

		public double DispCoefStartWL
		{
			get { return this._dispCoefStartWL; }
			set { lock (this._lockObj) { this._dispCoefStartWL = value; } }
		}

		public double DispCoefEndWL
		{
			get { return this._dispCoefEndWL; }
			set { lock (this._lockObj) { this._dispCoefEndWL = value; } }
		}

		public uint ProductFilterWheelPos
		{
			get { return this._productFilterWheelPos; }
			set { lock (this._lockObj) { this._productFilterWheelPos = value; } }
		}

		public GainOffsetData[][] ChuckLOPCorrectArray
		{
			get { return this._chuckLOPCorrectArray; }
            set
            {
                lock (this._lockObj)
                {
                    if (value.Length != this._chuckLOPCorrectArray.Length)
                    {
                        return;
                    }
                    else
                    {
                        this._chuckLOPCorrectArray = value;
                    }
                }
            }
		}

        public string SptCalibPathAndFile
        {
            get { return this._sptCalibPathAndFile; }
            set { lock (this._lockObj) { this._sptCalibPathAndFile = value; } }
        }

        public double[] ProductSptXwaveCoef
        {
            get { return this._productSptXaxisCoef; }
            set { lock (this._lockObj) { this._productSptXaxisCoef = value; } }
        }

        public double[] ProductSptYintCoef
        {
            get { return this._productSptYintCoef; }
            set { lock (this._lockObj) { this._productSptYintCoef = value; } }
        }

        public double[] ProductSptYweight
        {
            get { return this._productSptYweight; }
            set { lock (this._lockObj) { this._productSptYweight = value; } }
        }
		
        public double Resistance
        {
            get { return this._resistance; }
            set { lock (this._lockObj) { this._resistance = value; } }
        }

        public double[] ChuckResistanceCorrectArray
        {
            get { return this._chuckResistanceCorrectArray; }
            set { lock (this._lockObj) { this._chuckResistanceCorrectArray = value; } }
        }

        public uint AdjacentConsecutiveErrorCount
        {
            get { return this._adjacentConsecutiveErrorCount; }
            set { lock (this._lockObj) { this._adjacentConsecutiveErrorCount = value; } }
        }

		public string BarcodePrintFormat
		{
			get { return this._barcodePrintFormat; }
			set { lock (this._lockObj) { this._barcodePrintFormat = value; } }
		}

        public uint LOPWLSkipCount
        {
            get { return this._LOPWLSkipCount; }
            set { lock (this._lockObj) { this._LOPWLSkipCount = value; } }
        }

        public string TestResultPathByTaskSheet
        {
            get { return this._testResultPathByTaskSheet; }
            set { lock (this._lockObj) { this._testResultPathByTaskSheet = value; } }
        }
  
        public bool IsTVSProduct
        {
            get { return this._isTVSProduct; }
            set { lock (this._lockObj) { this._isTVSProduct = value; } }
        }

        public string OutputFileFormat
        {
            get { return this._outputFileFormat; }
            set { lock (this._lockObj) { this._outputFileFormat = value; } }
        }

		public bool IsAdjacentAllItemPassCheck
		{
			get { return this._adjacentAllItemPassCheck; }
			set { lock (this._lockObj) { this._adjacentAllItemPassCheck = value; } }
		}

		public uint SamplingMonitorConsecutiveErrCount
		{
			get { return this._samplingMonitorConsecutiveErrCount; }
			set { lock (this._lockObj) { this._samplingMonitorConsecutiveErrCount = value; } }
		}

		public uint SamplingMonitorMode
		{
			get { return this._samplingMonitorMode; }
			set { lock (this._lockObj) { this._samplingMonitorMode = value; } }
		}

		public CalibrateChipValue CaliChipValue
		{
			get { return this._caliChipValue; }
			set { lock (this._lockObj) { this._caliChipValue = value; } }
		}

        public bool IsApplySystemCoef
        {
            get { return this._isApplySysCoef; }
            set { lock (this._lockObj) { this._isApplySysCoef = value; } }
        }

        public int AdjacentStartingCount
        {
            get { return this._adjacentStartingCount; }
            set { lock (this._lockObj) { this._adjacentStartingCount = value; } }
        }

        public int AdjacentStopCount
        {
            get { return this._adjacentStopCount; }
            set { lock (this._lockObj) { this._adjacentStopCount = value; } }
        }

        public bool IsJudgeFailSkipESDItem
		{
            get { return this._isJudgeFailSkipESDItem; }
			set { lock (this._lockObj) { this._isJudgeFailSkipESDItem = value; } }
		}

        public double PdDetectorFactor
        {
            get { return this._pdDectorFactor; }
            set { lock (this._lockObj) { this._pdDectorFactor = value; } }
        }

		public RISpec RISpec
		{
			get { return this._riSpec; }
			set { lock (this._lockObj) { this._riSpec = value; } }
		}

        public int XLineSubBinSampleCH
        {
            get { return this._xLineSubBinSampleCH; }
            set { this._xLineSubBinSampleCH = value; }
        }

        public int YLineSubBinSampleCH
        {
            get { return this._yLineSubBinSampleCH; }
            set { this._yLineSubBinSampleCH = value; }
        }
        public ByCustomerSetting CustomerizedSetting
        {
            get { return this._ByCustomerSetting; }
            set { lock (this._lockObj) { this._ByCustomerSetting = value; } }
        }

        public LaserSourceSysSettingData LaserSrcSetting
        {
            get { return this._laserSrcSetting; }
            set { lock (this._lockObj) { this._laserSrcSetting = value; } }            
        }

        public string ProductName
        {
            get { return this._productName; }
            set { lock (this._lockObj) { this._productName = value; } }            
        }

        public OutputPathManager PathManager;


        #endregion

        #region >>> Public Method <<<

        public object Clone()
		{
			//ConditionData obj = this.MemberwiseClone() as ConditionData;
			//obj._lockObj = new object();
			//return (object) obj;

			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, this);
			ms.Position = 0;
			object obj = bf.Deserialize(ms);
			ms.Close();

			return obj;
		}		

		#endregion
		
	}

    [Serializable]
    public class ByCustomerSetting : ICloneable
    {
        private object _lockObj;

        private string _testResultPath01;
        private string _testResultPath02;
        private string _testResultPath03;

        private string _WAFOutputPath01;
        private string _WAFOutputPath02;
        private string _WAFOutputPath03;

        private bool _isEnableTestGroup;

        private bool _isMergeReport;

        //[XmlIgnore]
        //private Dictionary<string, CustomerMsrtNameInfo> _cusmreNameDic;

        //private List<CustomerMsrtNameInfo> _cusmreNameList;

        public ByCustomerSetting()
        {
            this._lockObj = new object();

            this._testResultPath01 = Constants.Paths.MPI_TEMP_DIR;
            this._testResultPath02 = Constants.Paths.MPI_TEMP_DIR;
            this._testResultPath03 = Constants.Paths.MPI_TEMP_DIR;

            this._WAFOutputPath01 = Constants.Paths.MPI_TEMP_DIR;
            this._WAFOutputPath02 = Constants.Paths.MPI_TEMP_DIR;
            this._WAFOutputPath03 = Constants.Paths.MPI_TEMP_DIR;

            //this._cusmreNameList = new List<CustomerMsrtNameInfo>();
            //_cusmreNameDic = null;
            this._isEnableTestGroup = false;
            this._isMergeReport = false;
        }


        public bool IsEnableTestGroup
        {
            get { return this._isEnableTestGroup; }
            set { lock (this._lockObj) { this._isEnableTestGroup = value; } }
        }

        // Test Result Path 

        public string TestResultPath01
        {
            get { return this._testResultPath01; }
            set { lock (this._lockObj) { this._testResultPath01 = value; } }
        }

        public string TestResultPath02
        {
            get { return this._testResultPath02; }
            set { lock (this._lockObj) { this._testResultPath02 = value; } }
        }

        public string TestResultPath03
        {
            get { return this._testResultPath03; }
            set { lock (this._lockObj) { this._testResultPath03 = value; } }
        }

        // WAF Setting

        public string WAFOutputPath01
        {
            get { return this._WAFOutputPath01; }
            set { lock (this._lockObj) { this._WAFOutputPath01 = value; } }
        }

        public string WAFOutputPath02
        {
            get { return this._WAFOutputPath02; }
            set { lock (this._lockObj) { this._WAFOutputPath02 = value; } }
        }

        public string WAFOutputPath03
        {
            get { return this._WAFOutputPath03; }
            set { lock (this._lockObj) { this._WAFOutputPath03 = value; } }
        }
        
        
        public bool IsMergeReport
        {
            get { return this._isMergeReport; }
            set { lock (this._lockObj) { this._isMergeReport = value; } }
        }


        //public List<CustomerMsrtNameInfo> CustomerNameInfo { // 沒辦法 Dictionary 序列化會有問題
        //    get {
        //        _cusmreNameDic = null;
        //        return _cusmreNameList; }
        //    set {
        //        _cusmreNameDic = null;
        //        _cusmreNameList = value; 
        //    }
        //}

        //[XmlIgnore]
        //public Dictionary<string, CustomerMsrtNameInfo> CustomerNameDic
        //{
        //    get
        //    {
        //        if (_cusmreNameDic == null)
        //        {
        //            _cusmreNameDic = new Dictionary<string, CustomerMsrtNameInfo>();
        //            foreach (var cData in _cusmreNameList)//不要用 getter，不然會陷入死循環
        //            {
        //                if (!_cusmreNameDic.ContainsKey(cData.Key))
        //                {
        //                    _cusmreNameDic.Add(cData.Key, cData);
        //                }
        //                else
        //                {
        //                    _cusmreNameDic[cData.Key] = cData;
        //                }
        //            }
        //        }
        //        return _cusmreNameDic;
        //    }
        //}

        public object Clone()
        {
            //ConditionData obj = this.MemberwiseClone() as ConditionData;
            //obj._lockObj = new object();
            //return (object) obj;

            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            ms.Position = 0;
            object obj = bf.Deserialize(ms);
            ms.Close();

            return obj;
        }


    }

    //[Serializable]
    //public class CustomerMsrtNameInfo
    //{

    //    public List<string> NameList { get; set; }
    //    public string Key { get; set; }
        
    //    public string Name { get; set; }

    //    public CustomerMsrtNameInfo()
    //        : base()
    //    {
    //        NameList = new List<string>();
    //        Key = "";
    //        Name = "";
    //    }

    //    #region >>public property<<
    //    public string this[int index]
    //    {
    //        get { return NameList[index]; }
    //    }

    //    public int Count
    //    {
    //        get { return NameList.Count; }
    //    }

    //    #endregion

    //    #region
    //    public void Add(string str)
    //    {
    //        NameList.Add(str);
    //    }

    //    public void Clear()
    //    {
    //        NameList.Clear();
    //    }
    //    #endregion
        
    //}
}
