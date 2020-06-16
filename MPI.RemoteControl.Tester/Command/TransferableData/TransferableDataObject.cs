using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPI.RemoteControl.Tester
{
    public enum ETransferableCommonObject : int
    {
        None = -1,

        D76Data = 100001,
        MC300Data = 100002,
        TesterInformation = 100003,
        LaserBarProberInfo = 100004,
        ProcessInformation = 100005,

        SubRecipeInformation = 100101,  // P10T		
        PreSOTInformationForDP = 100102,  // LDP80V
        PreWaferINInformation = 100103,  // AWSC
        PreSOTInformationForProber = 100104,  // AWSC

        PreOverloadTestInfo = 100201,  // Emcore
        ChuckTemperatureInfo = 100202,

        PreWaferInForAMS = 100301,//AMS
        TesterOutputRelativePath = 100302,

        CheckLaserPower = 100401,
    }

	[Serializable]
	public class DP76Data : TransferableCommonObjectBase
	{
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

		[XmlIgnore]
		private string _productLine;
		[XmlIgnore]
		private string _caliFileLine;
		[XmlIgnore]
		private KeyNameTable _testResultList;
		[XmlIgnore]
		private ItemSpecTable _itemSpecTable;

        [XmlIgnore]
        private bool _isEnableIntensityFactor;
        [XmlIgnore]
        private double _maxCountPercen;
        [XmlIgnore]
        private double _minCountPercen;
        [XmlIgnore]
        private double _maxPoGain;
        [XmlIgnore]
        private double _minPoGain;
        [XmlIgnore]
        private double _maxIvGain;
        [XmlIgnore]
        private double _minIvGain;

		public DP76Data()
			: base()
		{
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.Initialize();
		}

		private void Initialize()
		{
			this._productLine = string.Empty;

			this._caliFileLine = string.Empty;

			this._testResultList = new KeyNameTable();

			this._itemSpecTable = new ItemSpecTable();

            this._isEnableIntensityFactor = false;

            this._maxCountPercen = 0;

            this._minCountPercen = 0;

            this._maxPoGain = 1;

            this._minPoGain = 1;

            this._maxIvGain = 1;

            this._minIvGain = 1;
        }

		[XmlIgnore]
		public string ProductLine
		{
			get { return this._productLine; }
			set { this._productLine = value; }
		}

		[XmlElement("ProductLine")]
		public XmlCDataSection XmlProductLine
		{
            get { return s_document.CreateCDataSection(_productLine); }
			set { _productLine = (value == null) ? String.Empty : value.Data; }
		}

		[XmlIgnore]
		public string CaliFileLine
		{
			get { return this._caliFileLine; }
			set { this._caliFileLine = value; }
		}

		[XmlElement("CaliFileLine")]
		public XmlCDataSection XmlCaliFileLine
		{
            get { return s_document.CreateCDataSection(_caliFileLine); }
			set { _caliFileLine = (value == null) ? String.Empty : value.Data; }
		}

		[XmlElement]
		public KeyNameTable TestResultList
		{
			get { return this._testResultList; }
			set { this._testResultList = value; }
		}

		[XmlElement("ItemSpecTable")]
		public ItemSpecTable SpecTable
		{
			get { return this._itemSpecTable; }
			set { this._itemSpecTable = value; }
		}

        [XmlElement]
        public bool IsEnableIntensityFactor
        {
            get { return this._isEnableIntensityFactor; }
            set { this._isEnableIntensityFactor = value; }
        }

        [XmlElement]
        public double MaxCountPercen
        {
            get { return this._maxCountPercen; }
            set { this._maxCountPercen = value; }
        }
        [XmlElement]
        public double MinCountPercen
        {
            get { return this._minCountPercen; }
            set { this._minCountPercen = value; }
        }
        [XmlElement]
        public double MaxPoGain
        {
            get { return this._maxPoGain; }
            set { this._maxPoGain = value; }
        }
        [XmlElement]
        public double MinPoGain
        {
            get { return this._minPoGain; }
            set { this._minPoGain = value; }
        }
        [XmlElement]
        public double MaxIvGain
        {
            get { return this._maxIvGain; }
            set { this._maxIvGain = value; }
        }
        [XmlElement]
        public double MinIvGain
        {
            get { return this._minIvGain; }
            set { this._minIvGain = value; }
        }

		public DP76Data Clone()
		{
			DP76Data data = new DP76Data();

			data._productLine = this._productLine;

			data._caliFileLine = this._caliFileLine;

			this._testResultList.Reset();
			while (_testResultList.MoveNext())
			{
				data._testResultList.Add(_testResultList.CurrentPair.Key, _testResultList.CurrentPair.Value);
			}

            data.IsEnableIntensityFactor = this._isEnableIntensityFactor;
            data.MaxCountPercen = this._maxCountPercen;
            data.MinCountPercen = this._minCountPercen;
            data.MaxPoGain = this._maxPoGain;
            data.MinPoGain = this._minPoGain;
            data.MaxIvGain = this._maxIvGain;
            data.MinIvGain = this._minIvGain;

			return data;
		}

		protected override bool DeserializeCommonObject(string context)
		{
			using (StringReader sr = new StringReader(context))
			{
				try
				{
                    DP76Data data = s_serializer.Deserialize(sr) as DP76Data;
					if (data != null)
					{
						Initialize();

						this._caliFileLine = data.CaliFileLine;
						this._productLine = data.ProductLine;
						this._testResultList = data.TestResultList;
						this._itemSpecTable = data.SpecTable;
                        this._isEnableIntensityFactor = data.IsEnableIntensityFactor;
                        this._maxCountPercen = data.MaxCountPercen;
                        this._minCountPercen = data.MinCountPercen;
                        this._maxPoGain = data.MaxPoGain;
                        this._minPoGain = data.MinPoGain;
                        this._maxIvGain = data.MaxIvGain;
                        this._minIvGain = data.MinIvGain;
						return true;
					}
					else
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
			}
		}

		protected override string SerializeCommonObject()
		{
			StringBuilder sb = new StringBuilder();
			using (StringWriter sw = new StringWriter(sb))
			{
                s_serializer.Serialize(sw, this);

				return sw.ToString();
			}
		}

		protected override int ObjectIdentifiedID
		{
			get
			{
				return (int)ETransferableCommonObject.D76Data;
			}
		}
	}

    [Serializable]
    public class SubRecipeInformation : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        private string _mainRecipeName;
        private string _subRecipeName;
        private int _subRecipeIndex;
        private double _temperature;
        private int _slotNumber;

        public SubRecipeInformation()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.InitializeData();
        }

        private void InitializeData()
        {
            _mainRecipeName = String.Empty;
            _subRecipeName = String.Empty;
            _subRecipeIndex = 0;
            _temperature = 0;
            _slotNumber = 0;
        }

        [XmlIgnore]
        public string MainRecipeName
        {
            get { return _mainRecipeName; }
            set { _mainRecipeName = value; }
        }

        [XmlIgnore]
        public string SubRecipeName
        {
            get { return _subRecipeName; }
            set { _subRecipeName = value; }
        }

        [XmlElement("MainRecipeName")]
        public XmlCDataSection _xmlMainRecipeName
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_mainRecipeName) ? String.Empty : _mainRecipeName); }
            set { _mainRecipeName = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("SubRecipeName")]
        public XmlCDataSection _xmlSubRecipeName
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_subRecipeName) ? String.Empty : _subRecipeName); }
            set { _subRecipeName = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("SubRecipeIndex")]
        public int SubRecipeIndex
        {
            get { return _subRecipeIndex; }
            set { _subRecipeIndex = value; }
        }

        [XmlElement("Temperature")]
        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }

        [XmlElement("SlotNumber")]
        public int SlotNumber
        {
            get { return _slotNumber; }
            set { _slotNumber = value; }
        }

        public SubRecipeInformation Clone()
        {
            SubRecipeInformation info = new SubRecipeInformation();

            return this.CopyTo(info) ? info : null;
        }

        public bool CopyTo(SubRecipeInformation info)
        {
            if (info == null) return false;

            info.MainRecipeName = this.MainRecipeName;
            info.SubRecipeName = this.SubRecipeName;
            info.SubRecipeIndex = this.SubRecipeIndex;
            info.Temperature = this.Temperature;
            info.SlotNumber = this.SlotNumber;

            return true;
        }

        public bool CopyFrom(SubRecipeInformation info)
        {
            if (info == null) return false;

            this.MainRecipeName = info.MainRecipeName;
            this.SubRecipeName = info.SubRecipeName;
            this.SubRecipeIndex = info.SubRecipeIndex;
            this.Temperature = info.Temperature;
            this.SlotNumber = info.SlotNumber;

            return true;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            try
            {
                using (StringReader sr = new StringReader(context))
                {
                    SubRecipeInformation info = s_serializer.Deserialize(sr) as SubRecipeInformation;

                    if (info != null)
                    {
                        this.InitializeData();

                        this.CopyFrom(info);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        protected override string SerializeCommonObject()
        {
            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                s_serializer.Serialize(sw, this);

                return sw.ToString();
            }
        }

        protected override int ObjectIdentifiedID
        {
            get { return (int)ETransferableCommonObject.SubRecipeInformation; }
        }
    }

    [Serializable]
    public class InstrumentInfoBase
    {
        protected static XmlDocument s_doc = new XmlDocument();

        public InstrumentInfoBase()
        {
        }
    }

    public enum SourceMeterManufactory
    {
        MPI = 0,
        Keithely,
    }

    public enum SpectrumMeterManufactory
    {
        MPI = 0,
        Ocean,
        IS,
    }

    [Serializable]
    public class SourceMeterInfo : InstrumentInfoBase
    {
        private SourceMeterManufactory _manufactory;
        private string _model;
        private string _serialNumber;
        private string _firmwareVersion;

        [XmlElement("Brand")]
        public SourceMeterManufactory Brand
        {
            get { return _manufactory; }
            set { _manufactory = value; }
        }

        [XmlIgnore]
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        [XmlElement("Model")]
        public XmlCDataSection XmlModel
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_model) ? String.Empty : _model); }
            set { _model = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlIgnore]
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }

        [XmlElement("SerialNumber")]
        public XmlCDataSection XmlSerialNumber
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_serialNumber) ? String.Empty : _serialNumber); }
            set { _serialNumber = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlIgnore]
        public string FirmwareVersion
        {
            get { return _firmwareVersion; }
            set { _firmwareVersion = value; }
        }

        [XmlElement("FirmwareVersion")]
        public XmlCDataSection XmlFirmwareVersion
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_firmwareVersion) ? String.Empty : _firmwareVersion); }
            set { _firmwareVersion = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        public SourceMeterInfo()
            : base()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            _manufactory = SourceMeterManufactory.MPI;
            _model = String.Empty;
            _firmwareVersion = String.Empty;
            _serialNumber = String.Empty;
        }
    }

    [Serializable]
    public class SpectrumMeterInfo : InstrumentInfoBase
    {
        private SpectrumMeterManufactory _manufactory;
        private string _model;
        private string _serialNumber;


        [XmlElement("Brand")]
        public SpectrumMeterManufactory Brand
        {
            get { return _manufactory; }
            set { _manufactory = value; }
        }

        [XmlIgnore]
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        [XmlElement("Model")]
        public XmlCDataSection XmlModel
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_model) ? String.Empty : _model); }
            set { _model = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlIgnore]
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }

        [XmlElement("SerialNumber")]
        public XmlCDataSection XmlSerialNumber
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_serialNumber) ? String.Empty : _serialNumber); }
            set { _serialNumber = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        public SpectrumMeterInfo()
            : base()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            _manufactory = SpectrumMeterManufactory.MPI;
            _model = String.Empty;
            _serialNumber = String.Empty;

        }
    }

    [Serializable]
    public class SourceMeterCollector : InstrumentInformationCollector<SourceMeterInfo>
    {
        public SourceMeterCollector()
            : base()
        {
        }
    }

    [Serializable]
    public class SpectrumMeterCollector : InstrumentInformationCollector<SpectrumMeterInfo>
    {
        public SpectrumMeterCollector()
            : base()
        {
        }
    }

    [Serializable]
    public class InstrumentInformationCollector<TInstrument>
        where TInstrument : class
    {
        private object _lock;
        private List<TInstrument> _instrumentInformations;

        [XmlElement]
        public List<TInstrument> Instruments
        {
            get { return _instrumentInformations; }
            set { _instrumentInformations = value; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return (_instrumentInformations == null) ? 0 : _instrumentInformations.Count; }
        }

        public InstrumentInformationCollector()
        {
            _lock = new object();

            _instrumentInformations = new List<TInstrument>();
        }

        public void Add(TInstrument instrument)
        {
            if (_instrumentInformations == null)
                _instrumentInformations = new List<TInstrument>();

            _instrumentInformations.Add(instrument);
        }

        public int DoAction(Action<TInstrument> ActionForEachInstrument)
        {
            if (_instrumentInformations == null || _instrumentInformations.Count <= 0) return 0;

            List<TInstrument>.Enumerator itr = _instrumentInformations.GetEnumerator();

            int nDoActionCount = 0;

            while (itr.MoveNext())
            {
                ActionForEachInstrument((TInstrument)itr.Current);
                nDoActionCount++;
            }

            return nDoActionCount;
        }

        public void Clear()
        {
            if (_instrumentInformations == null)
                _instrumentInformations = new List<TInstrument>();

            _instrumentInformations.Clear();
        }

    }

    [Serializable]
    public class TesterInformation : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        private string _calibrationFileName;
        private SourceMeterCollector _sourcemeterCollector;
        private SpectrumMeterCollector _spectrumCollector;

        [XmlIgnore]
        public string CalibrationFileName
        {
            get { return _calibrationFileName; }
            set { _calibrationFileName = value; }
        }

        [XmlElement("CalibrationFileName")]
        public XmlCDataSection XmlCalibrationFileName
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_calibrationFileName) ? String.Empty : _calibrationFileName); }
            set { _calibrationFileName = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlElement]
        public SourceMeterCollector SourceMeters
        {
            get { return _sourcemeterCollector; }
            set { _sourcemeterCollector = value; }
        }

        [XmlElement]
        public SpectrumMeterCollector SpetrumMeters
        {
            get { return _spectrumCollector; }
            set { _spectrumCollector = value; }
        }

        public TesterInformation()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.InitializeData();
        }

        private void InitializeData()
        {
            this._sourcemeterCollector = new SourceMeterCollector();
            this._spectrumCollector = new SpectrumMeterCollector();
            _calibrationFileName = String.Empty;

        }

        public TesterInformation Clone()
        {
            TesterInformation info = new TesterInformation();

            return this.CopyTo(info) ? info : null;
        }

        public bool CopyTo(TesterInformation info)
        {
            if (info == null) return false;

            info.SourceMeters = this.SourceMeters;
            info.SpetrumMeters = this.SpetrumMeters;
            info.CalibrationFileName = this.CalibrationFileName;

            return true;
        }

        public bool CopyFrom(TesterInformation info)
        {
            if (info == null) return false;

            this.SourceMeters = info.SourceMeters;
            this.SpetrumMeters = info.SpetrumMeters;
            this.CalibrationFileName = info.CalibrationFileName;

            return true;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    TesterInformation data = s_serializer.Deserialize(sr) as TesterInformation;
                    if (data != null)
                    {
                        InitializeData();

                        this.CopyFrom(data);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        protected override string SerializeCommonObject()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                s_serializer.Serialize(sw, this);

                return sw.ToString();
            }
        }

        protected override int ObjectIdentifiedID
        {
            get
            {
                return (int)ETransferableCommonObject.TesterInformation;
            }
        }
    }

    [Serializable]
    public class PreSOTInformationForDP : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        [XmlIgnore]
        private int _testStageCount;
        [XmlIgnore]
        private StageItemTable _stageItemTable;

        public PreSOTInformationForDP()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.Initialize();
        }

        private void Initialize()
        {
            _testStageCount = 1;

            _stageItemTable = new StageItemTable();
        }


        [XmlElement]
        public int TestStageCount
        {
            get { return this._testStageCount; }
            set { this._testStageCount = value; }
        }

        [XmlElement("StageItemTable")]
        public StageItemTable StageItemTable
        {
            get { return this._stageItemTable; }
            set { this._stageItemTable = value; }
        }


        public PreSOTInformationForDP Clone()
        {
            PreSOTInformationForDP data = new PreSOTInformationForDP();

            data.TestStageCount = this._testStageCount;
            data.StageItemTable = this._stageItemTable;
            return data;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    PreSOTInformationForDP data = s_serializer.Deserialize(sr) as PreSOTInformationForDP;
                    if (data != null)
                    {
                        Initialize();

                        this._testStageCount = data.TestStageCount;
                        this._stageItemTable = data.StageItemTable;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        protected override string SerializeCommonObject()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                s_serializer.Serialize(sw, this);

                return sw.ToString();
            }
        }

        protected override int ObjectIdentifiedID
        {
            get
            {
                return (int)ETransferableCommonObject.PreSOTInformationForDP;
            }
        }
    }

    [Serializable]
    public class PreWaferINInformation : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        [XmlIgnore]
        private int _testStageCount;
        [XmlIgnore]
        //private List<int> _stageTestItemIndexList;
        private StageItemTable _stageItemTable;
        [XmlIgnore]
        private string _productType;
        [XmlIgnore]
        private string _proberRecipe;
        [XmlIgnore]
        private double _temperature;

        public PreWaferINInformation()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.Initialize();
        }

        private void Initialize()
        {
            _testStageCount = 1;

            //_stageTestItemIndexList = new List<int>();
            _stageItemTable = new StageItemTable();

            _productType = String.Empty;

            _proberRecipe = String.Empty;

            _temperature = 0.0d;
        }


        [XmlElement]
        public int TestStageCount
        {
            get { return this._testStageCount; }
            set { this._testStageCount = value; }
        }

        //[XmlElement("StageTestItemIndexList")]
        //public List<int> StageTestItemIndexList
        //{
        //    get { return this._stageTestItemIndexList; }
        //    set { this._stageTestItemIndexList = value; }
        //}

        [XmlElement("StageItemTable")]
        public StageItemTable StageItemTable
        {
            get { return this._stageItemTable; }
            set { this._stageItemTable = value; }
        }

        [XmlIgnore]
        public string ProductType
        {
            get { return _productType; }
            set { _productType = value; }
        }

        [XmlElement("ProductType")]
        public XmlCDataSection XmlProductType
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_productType) ? String.Empty : _productType); }
            set { _productType = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlIgnore]
        public string ProberRecipe
        {
            get { return _proberRecipe; }
            set { _proberRecipe = value; }
        }

        [XmlElement("ProberRecipe")]
        public XmlCDataSection XmlProberRecipe
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_proberRecipe) ? String.Empty : _proberRecipe); }
            set { _proberRecipe = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

         [XmlElement]
        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }

        //[XmlElement("Temperature")]
        //public XmlCDataSection XmlTemperature
        //{
        //    get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_temperature) ? String.Empty : _temperature); }
        //    set { _temperature = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        //}


        public PreWaferINInformation Clone()
        {
            PreWaferINInformation data = new PreWaferINInformation();

            data.TestStageCount = this._testStageCount;
            //data.StageTestItemIndexList = this._stageTestItemIndexList;
            data.StageItemTable = this._stageItemTable;

            data.ProductType = this._productType;
            data.ProberRecipe = this._proberRecipe;
            data.Temperature = this._temperature;
            return data;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    PreWaferINInformation data = s_serializer.Deserialize(sr) as PreWaferINInformation;
                    if (data != null)
                    {
                        Initialize();

                        this._testStageCount = data.TestStageCount;
                        //this._stageTestItemIndexList = data.StageTestItemIndexList;
                        this._stageItemTable = data.StageItemTable;

                        this._productType = data.ProductType;
                        this._proberRecipe = data.ProberRecipe;
                        this._temperature = data.Temperature;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        protected override string SerializeCommonObject()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                s_serializer.Serialize(sw, this);

                return sw.ToString();
            }
        }

        protected override int ObjectIdentifiedID
        {
            get
            {
                return (int)ETransferableCommonObject.PreWaferINInformation;
            }
        }
    }

    [Serializable]
    public class PreSOTInformationForProber : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        [XmlIgnore]
        private int _reticleX;
        [XmlIgnore]
        private int _reticleY;
        [XmlIgnore]
        private int _dutNumber;
        [XmlIgnore]
        private string _dutID;
        [XmlIgnore]
        private double _dutOffset;
        [XmlIgnore]
        private double _pzPosition;
        [XmlIgnore]
        private double _dzPosition;

        public PreSOTInformationForProber()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.Initialize();
        }

        private void Initialize()
        {
            this._reticleX = 0;
            this._reticleY = 0;
            this._dutNumber = 0;
            this._dutID = String.Empty;
            this._dutOffset = 0;
            this._pzPosition = 0;
            this._dzPosition = 0;
        }

        [XmlElement]
        public int ReticleX
        {
            get { return this._reticleX; }
            set { this._reticleX = value; }
        }

        [XmlElement]
        public int ReticleY
        {
            get { return this._reticleY; }
            set { this._reticleY = value; }
        }

       [XmlElement]
        public int DUTNumber
        {
            get { return _dutNumber; }
            set { _dutNumber = value; }
        }

        //[XmlElement("DUTNumber")]
        //public XmlCDataSection XmlDUTNumber
        //{
        //    get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_dutNumber) ? String.Empty : _dutNumber); }
        //    set { _dutNumber = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        //}

        [XmlIgnore]
        public string DUTID
        {
            get { return _dutID; }
            set { _dutID = value; }
        }

        [XmlElement("DUTID")]
        public XmlCDataSection XmlDUTID
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_dutID) ? String.Empty : _dutID); }
            set { _dutID = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlElement]
        public double DUTOffset
        {
            get { return this._dutOffset; }
            set { this._dutOffset = value; }
        }

        [XmlElement]
        public double PZPosition
        {
            get { return this._pzPosition; }
            set { this._pzPosition = value; }
        }

        [XmlElement]
        public double DZPosition
        {
            get { return this._dzPosition; }
            set { this._dzPosition = value; }
        }

        public PreSOTInformationForProber Clone()
        {
            PreSOTInformationForProber data = new PreSOTInformationForProber();

            data.ReticleX = this._reticleX;
            data.ReticleY = this._reticleY;
            data.DUTNumber = this._dutNumber;
            data.DUTID = this._dutID;
            data.DUTOffset = this._dutOffset;
            data.PZPosition = this._pzPosition;
            data.DZPosition = this._dzPosition;

            return data;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    PreSOTInformationForProber data = s_serializer.Deserialize(sr) as PreSOTInformationForProber;
                    if (data != null)
                    {
                        Initialize();

                        this._reticleX = data.ReticleX;
                        this._reticleY = data.ReticleY;
                        this._dutNumber = data.DUTNumber;
                        this._dutID = data.DUTID;
                        this._dutOffset = data.DUTOffset;
                        this._pzPosition = data.PZPosition;
                        this._dzPosition = data.DZPosition;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        protected override string SerializeCommonObject()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                s_serializer.Serialize(sw, this);

                return sw.ToString();
            }
        }

        protected override int ObjectIdentifiedID
        {
            get
            {
                return (int)ETransferableCommonObject.PreSOTInformationForProber;
            }
        }
    }


    //[Serializable]
    //public class PreOverloadTestInfo : TransferableCommonObjectBase
    //{
    //    private static XmlDocument s_document = new XmlDocument();
    //    private static XmlSerializer s_serializer = null;

    //    [XmlIgnore]
    //    private RefPointsTable _refTable;
    //    [XmlIgnore]
    //    private EOverloadTestMode _testMode;

    //    public PreOverloadTestInfo()
    //        : base()
    //    {
    //        if (s_serializer == null)
    //            s_serializer = new XmlSerializer(this.GetType());

    //        this.Initialize();
    //    }

    //    private void Initialize()
    //    {
    //        _refTable = new RefPointsTable();
    //        _testMode = EOverloadTestMode.Normal;
    //    }


    //    [XmlElement]
    //    public RefPointsTable RefTable
    //    {
    //        get { return this._refTable; }
    //        set { this._refTable = value; }
    //    }

    //    [XmlElement]
    //    public EOverloadTestMode TestMode
    //    {
    //        get { return this._testMode; }
    //        set { this._testMode = value; }
    //    }

    //    public PreOverloadTestInfo Clone()
    //    {
    //        PreOverloadTestInfo data = new PreOverloadTestInfo();

    //        data.RefTable.Table.AddRange(this.RefTable.Table.ToArray());//deep copy

    //        data.TestMode = this.TestMode;

    //        return data;
    //    }

    //    protected override bool DeserializeCommonObject(string context)
    //    {
    //        using (StringReader sr = new StringReader(context))
    //        {
    //            try
    //            {
    //                PreOverloadTestInfo data = s_serializer.Deserialize(sr) as PreOverloadTestInfo;
    //                if (data != null)
    //                {
    //                    Initialize();

    //                    this.TestMode = data.TestMode;

    //                    this.RefTable.Table.AddRange(data.RefTable.Table.ToArray());//deep copy

    //                    return true;
    //                }
    //                else
    //                {
    //                    return false;
    //                }
    //            }
    //            catch
    //            {
    //                return false;
    //            }
    //        }
    //    }

    //    protected override string SerializeCommonObject()
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        using (StringWriter sw = new StringWriter(sb))
    //        {
    //            s_serializer.Serialize(sw, this);

    //            return sw.ToString();
    //        }
    //    }

    //    protected override int ObjectIdentifiedID
    //    {
    //        get
    //        {
    //            return (int)ETransferableCommonObject.PreOverloadTestInfo;
    //        }
    //    }
    //}

    //public enum EOverloadTestMode
    //{
    //    Normal = 1,
    //    Overload = 2,
    //}

}
