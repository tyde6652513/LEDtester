using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class TesterSetting : ICloneable
	{
		private object _lockObj;

		private UInt32 _dieRepeatTestCount;
		private UInt32 _dieRepeatTestDelay;
		private UInt32 _SOTDelay;

		private int _proberCoord;
		private int _testerCoord;

		private TesterSpecCtrl _specCtrl;

		private OptiDevSetting _optiDevSetting;
		private ESDDevSetting _esdDevSetting;

        private OsaDevSetting _osaDevSetting;

		private DateTime _startTestTime;
		private DateTime _endTestTime;

		private bool _isAutoSelectForceRange;

		private int _IfPmuIndex;
		private int _VfPmuIndex;
		private int _IzPmuIndex;
		private int _VrPmuIndex;

		private double _coefTableStartWL;
		private double _coefTableEndWL;
		private double _coefTableResolution;

		private bool _isEnableDarkCorrect;

		private bool _isCheckRowCol;

		private bool _isCheckSpec2;

		private bool _isCountPassFailByBinGrade;

        private EBinBoundaryRule _binSortingRule;

		private bool _isBinSortingIncludeMinMax;

		private bool _isEnableAdjacentError;

		private bool _isEnableErrStateReTest;

		private bool _isEnableSaveDetailLog;

		private bool _iseEnableQAFailAutoReTest;

		private bool _isEnalbeCalcBigFactorBeforeSmall;

		private bool _isEnableLDT1ASoftwareClamp;

		private bool _isEnableSettingDefaultBinGrade;

		private int _defaultBinGrade;

		private ECCTCaculationType _CCTcaculationType;

		private string _preSamplingMonitorInfo;

		private bool _isAutoPopFourMapForm;

		private ContactCheckCFG _contactCheckCFG;

		private bool _isCalcANSIAndGB;

		private EANSI376 _ansi376;

		private EGB10682 _gb10682;

		private bool _isEnableHighSpeedMode;

		private bool _isEnablePassRateCheck;

		private EPassRateCheckNGMode _passRateCheckMode;

		private int _minCountOfRunningPassRateCheck;

		private bool _isMultiDieOpticalSamplingTest;

		private bool _isOnlySkipIZItem;

		private bool _isTakeFirstItemAsOpenShort;

		private bool _isPresampling;

        private uint _totalProbingCount;

		private bool _isEnableTestGroup;

        private bool _isJudgeFailKeepOpticsResult;

        private EPDDarkCorrectMode _pdDarkCorrectMode;

        private double _detectorSysFactor;

		private string _riSpecRecipeDir;

		private string _riSimulatorReportDir;

		private ERIReCalcMode _riReCalcMode;

        private bool _isEnableDeviceQcMode;	

        private bool _isEnableReportInterpolation;

        private bool _isEnableSrcMeterMsrtForceValue;

        private bool _isEnableRetestTestItem;

        private EGroupBinRule _groupBinRule;

		public TesterSetting()
		{
			this._lockObj = new object();

			this._dieRepeatTestCount = 1;
			this._dieRepeatTestDelay = 0;
			this._SOTDelay = 0;

			this._proberCoord = 1;
			this._testerCoord = 1;

			this._specCtrl = new TesterSpecCtrl();

			this._optiDevSetting = new OptiDevSetting();
			this._esdDevSetting = new ESDDevSetting();

            this._osaDevSetting = new OsaDevSetting();

			this._startTestTime = new DateTime();
			this._endTestTime = new DateTime();

			this._isAutoSelectForceRange = true;

			this._IfPmuIndex = 0;
			this._VfPmuIndex = 0;
			this._IzPmuIndex = 1;
			this._VrPmuIndex = 0;

			this._coefTableStartWL = 350.0d;
			this._coefTableEndWL = 850.0d;
			this._coefTableResolution = 1.0d;
			this._isEnableDarkCorrect = false;
			this._isCheckRowCol = true;
			this._isCheckSpec2 = false;
			this._isCountPassFailByBinGrade = false;
			this._binSortingRule = 0;
			this._isBinSortingIncludeMinMax = false;
			this._isEnableAdjacentError = false;
			this._isEnableErrStateReTest = true;
			this._isEnableSaveDetailLog = false;
			this._iseEnableQAFailAutoReTest = false;
			this._isEnalbeCalcBigFactorBeforeSmall = false;
			this._isEnableLDT1ASoftwareClamp = false;

			this._isEnableSettingDefaultBinGrade = false;

			this._defaultBinGrade = -1;

			this._CCTcaculationType = ECCTCaculationType.McCamy;

			this._preSamplingMonitorInfo = string.Empty;

			this._isAutoPopFourMapForm = false;

			this._contactCheckCFG = new ContactCheckCFG();

			this._isCalcANSIAndGB = false;

			this._ansi376 = EANSI376.ANSI_2700;

			this._gb10682 = EGB10682.GB_2700;

			this._isEnableHighSpeedMode = false;

			this._isEnablePassRateCheck = false;

			this._passRateCheckMode = EPassRateCheckNGMode.STOP_TEST;

			this._minCountOfRunningPassRateCheck = 99;

			this._isMultiDieOpticalSamplingTest = false;

			this._isOnlySkipIZItem = false;

			this._isTakeFirstItemAsOpenShort = false;

			this._isPresampling = false;

            this._totalProbingCount = 0;

			this._isEnableTestGroup = false;

            this._isJudgeFailKeepOpticsResult = false;

            this._pdDarkCorrectMode = EPDDarkCorrectMode.None;

            this._detectorSysFactor = 1.0d;

			this._riSpecRecipeDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

			this._riSimulatorReportDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

			this._riReCalcMode = ERIReCalcMode.Average;

            this._isEnableDeviceQcMode = false;	

            this._isEnableReportInterpolation = false;

            this._isEnableSrcMeterMsrtForceValue = false;

            this._isEnableRetestTestItem = false;

            this._groupBinRule = EGroupBinRule.MAX  ;

            this.IsSpecBinTableSync = false;
		}

		#region >>> Public Property <<<

		public UInt32 DieRepeatTestCount
		{
			get { return this._dieRepeatTestCount; }
			set
			{
				lock (this._lockObj)
				{
					if (value < 1)
					{
						this._dieRepeatTestCount = 1;
					}
					else
					{
						this._dieRepeatTestCount = value;
					}

				}
			}
		}

		public UInt32 DieRepeatTestDelay
		{
			get { return this._dieRepeatTestDelay; }
			set { lock (this._lockObj) { this._dieRepeatTestDelay = value; } }
		}

		public UInt32 SOTDelay
		{
			get { return this._SOTDelay; }
			set { lock (this._lockObj) { this._SOTDelay = value; } }
		}

		public int ProberCoord
		{
			get { return this._proberCoord; }
			set { lock (this._lockObj) { this._proberCoord = value; } }
		}

		public int TesterCoord
		{
			get { return this._testerCoord; }
			set { lock (this._lockObj) { this._testerCoord = value; } }
		}

		public OptiDevSetting OptiDevSetting
		{
			get { return this._optiDevSetting; }
			set { lock (this._lockObj) { this._optiDevSetting = value; } }
		}

		public ESDDevSetting EsdDevSetting
		{
			get { return this._esdDevSetting; }
			set { lock (this._lockObj) { this._esdDevSetting = value; } }
		}

        public OsaDevSetting OsaDevSetting
        {
            get { return this._osaDevSetting; }
            set { lock (this._lockObj) { this._osaDevSetting = value; } }
        }

		public DateTime StartTestTime
		{
			get { return this._startTestTime; }
			set { lock (this._lockObj) { this._startTestTime = value; } }
		}

		public DateTime EndTestTime
		{
			get { return this._endTestTime; }
			set { lock (this._lockObj) { this._endTestTime = value; } }
		}

		public bool IsAutoSelectForceRange
		{
			get { return this._isAutoSelectForceRange; }
			set { lock (this._lockObj) { this._isAutoSelectForceRange = value; } }
		}

		public int IfPmuIndex
		{
			get { return this._IfPmuIndex; }
			set { lock (this._lockObj) { this._IfPmuIndex = value; } }
		}

		public int VfPmuIndex
		{
			get { return this._VfPmuIndex; }
			set { lock (this._lockObj) { this._VfPmuIndex = value; } }
		}

		public int IzPmuIndex
		{
			get { return this._IzPmuIndex; }
			set { lock (this._lockObj) { this._IzPmuIndex = value; } }
		}

		public int VrPmuIndex
		{
			get { return this._VrPmuIndex; }
			set { lock (this._lockObj) { this._VrPmuIndex = value; } }
		}

		public double CoefTableStartWL
		{
			get { return this._coefTableStartWL; }
			set { lock (this._lockObj) { this._coefTableStartWL = value; } }
		}

		public double CoefTableEndWL
		{
			get { return this._coefTableEndWL; }
			set { lock (this._lockObj) { this._coefTableEndWL = value; } }
		}

		public double CoefTableResolution
		{
			get { return this._coefTableResolution; }
			set
			{
				lock (this._lockObj)
				{
					if (value <= 0.001d)
					{
						this._coefTableResolution = 0.001d;
					}
					else
					{
						this._coefTableResolution = value;
					}
				}
			}
		}

		public bool IsEnableDarkCorrect
		{
			get { return this._isEnableDarkCorrect; }
			set { lock (this._lockObj) { this._isEnableDarkCorrect = value; } }
		}

		public bool IsCheckRowCol
		{
			get { return this._isCheckRowCol; }
			set { lock (this._lockObj) { this._isCheckRowCol = value; } }
		}

		public bool IsCheckSpec2
		{
			get { return this._isCheckSpec2; }
			set { lock (this._lockObj) { this._isCheckSpec2 = value; } }
		}

		public bool IsCountPassFailByBinGrade
		{
			get { return this._isCountPassFailByBinGrade; }
			set { lock (this._lockObj) { this._isCountPassFailByBinGrade = value; } }
		}

        public EBinBoundaryRule BinSortingRule
		{
			get { return this._binSortingRule; }
			set { lock (this._lockObj) { this._binSortingRule = value; } }
		}

		public bool IsBinSortingIncludeMinMax
		{
			get { return this._isBinSortingIncludeMinMax; }
			set { lock (this._lockObj) { this._isBinSortingIncludeMinMax = value; } }
		}

		public bool IsEnableAdjacentError
		{
			get { return this._isEnableAdjacentError; }
			set { lock (this._lockObj) { this._isEnableAdjacentError = value; } }
		}

		public bool IsEnableErrStateReTest
		{
			get { return this._isEnableErrStateReTest; }
			set { lock (this._lockObj) { this._isEnableErrStateReTest = value; } }
		}

		public bool IsEnableSaveDetailLog
		{
			get { return this._isEnableSaveDetailLog; }
			set { lock (this._lockObj) { this._isEnableSaveDetailLog = value; } }
		}

		public bool IseEnableQAFailAutoReTest
		{
			get { return this._iseEnableQAFailAutoReTest; }
			set { lock (this._lockObj) { this._iseEnableQAFailAutoReTest = value; } }
		}

		public bool IsEnalbeCalcBigFactorBeforeSmall
		{
			get { return this._isEnalbeCalcBigFactorBeforeSmall; }
			set { lock (this._lockObj) { this._isEnalbeCalcBigFactorBeforeSmall = value; } }
		}

		public bool IsEnableLDT1ASoftwareClamp
		{
			get { return this._isEnableLDT1ASoftwareClamp; }
			set { lock (this._lockObj) { this._isEnableLDT1ASoftwareClamp = value; } }
		}

		public bool IsEnableSettingDefaultBinGrade
		{
			get { return this._isEnableSettingDefaultBinGrade; }
			set { lock (this._lockObj) { this._isEnableSettingDefaultBinGrade = value; } }
		}

		public int DefaultBinGrade
		{
			get { return this._defaultBinGrade; }
			set { lock (this._lockObj) { this._defaultBinGrade = value; } }
		}

		public ECCTCaculationType CCTcaculationType
		{
			get { return this._CCTcaculationType; }
			set { lock (this._lockObj) { this._CCTcaculationType = value; } }
		}

		public string PreSamplingMonitorInfo
		{
			get { return this._preSamplingMonitorInfo; }
			set { lock (this._lockObj) { this._preSamplingMonitorInfo = value; } }
		}

		public bool IsAutoPopFourMapForm
		{
			get { return this._isAutoPopFourMapForm; }
			set { lock (this._lockObj) { this._isAutoPopFourMapForm = value; } }
		}

		public ContactCheckCFG contactCheckCFG
		{
			get { return this._contactCheckCFG; }
			set { lock (this._lockObj) { this._contactCheckCFG = value; } }
		}

		public bool IsCalcANSIAndGB
		{
			get { return this._isCalcANSIAndGB; }
			set { lock (this._lockObj) { this._isCalcANSIAndGB = value; } }
		}

		public EANSI376 ANSI376
		{
			get { return this._ansi376; }
			set { lock (this._lockObj) { this._ansi376 = value; } }
		}

		public EGB10682 GB10682
		{
			get { return this._gb10682; }
			set { lock (this._lockObj) { this._gb10682 = value; } }
		}

		public bool IsEnablePassRateCheck
		{
			get { return this._isEnablePassRateCheck; }
			set { lock (this._lockObj) { this._isEnablePassRateCheck = value; } }
		}

		public EPassRateCheckNGMode PassRateCheckMode
		{
			get { return this._passRateCheckMode; }
			set { lock (this._lockObj) { this._passRateCheckMode = value; } }
		}

		public int MinCountOfRunningPassRateCheck
		{
			get { return this._minCountOfRunningPassRateCheck; }
			set { lock (this._lockObj) { this._minCountOfRunningPassRateCheck = value; } }
		}

		[XmlIgnore]
		public bool IsEnableHighSpeedMode
		{
			get { return this._isEnableHighSpeedMode; }
			set { lock (this._lockObj) { this._isEnableHighSpeedMode = value; } }
		}

		public bool IsEnableMultiDieOpticalSamplingTest
		{
			get { return this._isMultiDieOpticalSamplingTest; }
			set { lock (this._lockObj) { this._isMultiDieOpticalSamplingTest = value; } }
		}

		public TesterSpecCtrl SpecCtrl
		{
			get { return this._specCtrl; }
			set { lock (this._lockObj) { this._specCtrl = value; } }
		}

		public bool IsOnlySkipIZItem
		{
			get { return this._isOnlySkipIZItem; }
			set { lock (this._lockObj) { this._isOnlySkipIZItem = value; } }
		}

		public bool IsTakeFirstItemAsOpenShort
		{
			get { return this._isTakeFirstItemAsOpenShort; }
			set { lock (this._lockObj) { this._isTakeFirstItemAsOpenShort = value; } }
		}

		public bool IsPresampling
		{
			get { return this._isPresampling; }
			set { lock (this._lockObj) { this._isPresampling = value; } }
		}

        public uint TotalProbingCount
        {
            get { return this._totalProbingCount; }
            set { lock (this._lockObj) { this._totalProbingCount = value; } }
        }

		public bool IsEnableTestGroup
		{
			get { return this._isEnableTestGroup; }
			set { lock (this._lockObj) { this._isEnableTestGroup = value; } }
		}

        public bool IsJudgeFailKeepOpticsResult
        {
            get { return this._isJudgeFailKeepOpticsResult; }
            set { lock (this._lockObj) { this._isJudgeFailKeepOpticsResult = value; } }
        }

        public EPDDarkCorrectMode PDDarkCorrectMode
        {
            get { return this._pdDarkCorrectMode; }
            set { lock (this._lockObj) { this._pdDarkCorrectMode = value; } }
        }

        public double DetectorSysFactor
        {
            get { return this._detectorSysFactor; }
            set { lock (this._lockObj) { this._detectorSysFactor = value; } }
        }

		public string RISpecRecipeDir
		{
			get { return this._riSpecRecipeDir; }
			set { this._riSpecRecipeDir = value; }
		}

		public string RISimulatorReportDir
		{
			get { return this._riSimulatorReportDir; }
			set { this._riSimulatorReportDir = value; }
		}

		public ERIReCalcMode RIReCalcMode
		{
			get { return this._riReCalcMode; }
			set { this._riReCalcMode = value; }
		}

        public bool IsEnableDeviceQcMode
        {
            get { return this._isEnableDeviceQcMode; }
            set { this._isEnableDeviceQcMode = value; }
        }

        public bool IsEnableReportInterpolation
		{
			get { return this._isEnableReportInterpolation; }
			set { this._isEnableReportInterpolation = value; }
		}

        public bool IsEnableSrcMeterMsrtForceValue
        {
            get { return this._isEnableSrcMeterMsrtForceValue; }
            set { this._isEnableSrcMeterMsrtForceValue = value; }
        }

        public bool IsEnableRetestTestItem
        {
            get { return this._isEnableRetestTestItem; }
            set { this._isEnableRetestTestItem = value; }
        }
         public EGroupBinRule GroupBinRule
        {
            get { return this._groupBinRule; }
            set { this._groupBinRule = value; }
        }

         public bool IsSpecBinTableSync { get; set; }

		#endregion

		# region >>> Public Method <<<

		public object Clone()
		{
			TesterSetting obj = this.MemberwiseClone() as TesterSetting;

			obj._lockObj = new object();
			return (object)obj;
		}

		#endregion

	}

	public class ContactCheckCFG
	{
		public bool _isEnableContactCheck;

		public double _contactApplyCurrentValue;

		public double _contactApplyForceTime;

		public double _contactSpecMax;

		public double _contactSpecMin;

		public bool _isDisableCheckAtPosX;

		public bool _isEnableVzFillRandomValue;

		public bool _isEsdItemContactCheck;

		public ContactCheckCFG()
		{
			_isEnableContactCheck = false;

			_contactApplyCurrentValue = 10; // (uA)

			_contactApplyForceTime = 1;

			_contactSpecMax = 3;

			_contactSpecMin = 1.9;

			_isDisableCheckAtPosX = false;

			_isEnableVzFillRandomValue = false;

			_isEsdItemContactCheck = false;
		}
	}
}
