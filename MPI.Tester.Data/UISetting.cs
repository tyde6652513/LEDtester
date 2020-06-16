using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace MPI.Tester.Data
{
    public class UISetting : OutputPathManager, ICloneable
	{
		public delegate void CheckIdleTimeEventHandler();

        #region >> private data<<
		private object _lockObj;
        //private OutputPathManager _pathManager;//理論上應該要另外開一個Class來儲存輸出路徑
        //但由於動用路徑的地方實在太多，因此直接以繼承的方式來實現另開class來儲存路徑的需求

		private string _productFileName;
		private string _binDataFileName;
		private string _taskSheetFileName;
		private string _testResultFileName;
		private string _mapDataFileName;
		private string _importBinFileName;
		private string _importCalibrateFileName;
		private string _importCalibSptDataFileName;

        //private string _productPath;
        //private string _productPath02;
        //private string _coefTableExportPath;

		private string _machineName;
		private EUserID _userID;
		private UserData _userDefinedData;
		private string _formatName;
		private uint _formatIndex;

		private int _multiLanguage;

		private string _operatorName;
		private string _reporterName;
		private string _productName;
		private string _lotNumber;
		private string _cassetteNumber;
		private string _waferNumber;
		private string _barcode;
		private string _substrate;
		private string _loginID;
		private string _slotNumber;
		private string _keyNumber;
        private string _probRecipeName;
        private double _chuckTemperature;

		private bool _isExtResultItem;

		private int _mainFormLeftPos;
		private int _mainFormTopPos;

		private string[] _userIDList;

		private WMData _weiminUIData;
		private int _uiDisplayType;
		private int _uiOperateMode;

		private EAuthority _authorityLevel;

		private bool _isSetIFMsrtRange;
		private bool _isSetIZMsrtRange;
		private bool _isSetVFMsrtRange;
		private bool _isSetVRMsrtRange;

		private double _defaultIFMsrtRange;
		private double _defaultIZMsrtRange;
		private double _defaultVFMsrtRange;
		private double _defaultVRMsrtRange;

		private int _displayUIForm;
		// UI Set Task Sheet Control
		private string _sendTaskFileName;
		private EControlTaskSetting _controlTaskSettingUI;
		// MES
		private string _mesPath;
		private string _mesPath2;
		private string _mesbackupPath;

       private string _mesPath3;
        private string _mesPath4;

		private string _mesOpenFileName;
		private bool _isEnableMesSystem;
		private string _productType;
		private string _autoRm2Path;
		private string _rm2FileName;
		private string _proberRecipeName;
		private string _mesConditinFileName;
		private string _mapGradeFileName;
		private bool _isDeliverProberRecipe;
        private bool _isSendBarcodeToProbe;
		// Wafer map List Keyname
		private string[] _wafermapList;
		// 
		private int _waferPcs;

		//Wafer Mpa Boundary
		private bool _waferMapAutoBoundery;
		private int _waferMapLift;
		private int _waferMapTop;
		private int _waferMapRight;
		private int _waferMapBottom;
		private string _showMapKeyName;
		private int _waferMapGrowthDirection;
		// 
		private bool _isRunDailyCheckMode;
		private bool _isManualRunMode;
		private bool _isRunReTestSortPageMode;
		private bool _isConverterTasksheet;
		private bool _isShowDailyCheck;
		private string _dailyCheckLogFileName;
		private List<int> _daliycheckTime;
		private bool _isEnableCheckDailyDataIsValid;
		private bool _isEnableMapFormTopMost;
		private bool _isEnableAutoClearMapAndCIEChart;

		private bool _isEnableSaveErrSpectrum;
		private bool _isEnableSaveErrMsrt;
		private bool _isClearHistoryWhenStartTest;
        //private bool _isEnableSaveBackupCoef;
        //private string _backupCoefPath;
		private int _reProbingCounts;

		private uint _probingCount1;
		private uint _probingCount2;
		private uint _probingCount3;
		private uint _probingCount4;

		private uint _totalSacnCounts;

		private bool _isEnableSaveBinTable;
		private string _binTablePath;
		private bool _isWriteStatisticsDataToXmlHead;

		private ECalibrationUIMode _calibrationUIMode;

		private bool _isTCPIPSendEnableResultItem;

		private bool _isShowNPLCAndSGFilterSetting;

		private bool _isCheckStandardRecipe;

		private string _standRecipePath;

		private bool _isEnableDuplicateStdRecipe;



		// Daily Checking Verify
		private bool _isEnableJudgeRunDailyCheck;

		private string _runDailyCheckingKeyWord;

		// MES (Auto Recipe Setting)
		// DailyChecking Monitor

		private bool _isCheckDailyVerify;

		private string _dailyCheckingMonitorPath;

		private bool _isCheckDailyCheckingOverDue;

		private int _dailyCheckingOverDueHours;

		private bool _isEnableFloatReport;

		private bool _isWriteReportDirectly;

		// Pre Sampling Data

		private bool _isEnableMonitorPreSamplingData;

		private string _preSamplingDataPath;

		private bool _isManualSettingContiousProbingRowCol;

		private int _contiousProbingPosX;

		private int _contiousProbingPosY;

		private bool _isEnableConditionFormEnCheckBox;

        //Group Map Data

        private bool _isEnableLoadGroupData;

        private string _groupDataPath;

		// Load User ID
		private CustomizeDeifneOutputPath _customerDefineOutputPath;

		private bool _isEnableLoadUserTable;

		private string _txtLoadUserTablePath;

		private EEqModelName _eqModelName;

		//for 隆達
		private string _wO;
		private string _probeMachineName;
		private string _probeScanTime;
		private string _oPNO;
		private string _recipeID;
		private string _specID;

		private bool _isEnableMergeAOIFile;

		private bool _isDeleteAOISourceFile;

		private double _startTemp;

		private double _endTemp;

		private LIVCurveDrawSetting _LIVCurveDrawSetting;

		private int _probingValidCounts;

		private bool _isShowReportCommentsUI;

		private string _reportComments;

		private bool _isAbortSaveFile;

		private event CheckIdleTimeEventHandler CheckIdleTimeEvent;

		private string _uploadTaskSheetFilePath;

        private string _uploadProductFilePath;

		private string _uploadBinFilePath;

        private string _uploadMapFilePath;

		private bool _isDeleteMESFile;

		private int _boundarySampleTestMode;

		private double _xPitch1;

		private double _yPitch1;

		private double _xPitch2;

		private double _yPitch2;

		private double _xPitch3;

		private double _yPitch3;

		private double _xPitch4;

		private double _yPitch4;

		private double _xPitch5;

		private double _yPitch5;

	    private bool _isShowESDJudgeIR;

		private int _samplingDiePitchCol;

		private int _samplingDiePitchRow;
		
        private bool _isAbortSaveReport;

        private bool _isAutoLoadCalibData;

        private bool _isEnableContinousProbeInOPMode;

        private uint _dataKeepDays;

        private bool _isEnableAutoDeleteMESFile;

        private bool _isDownRecipeFromServer;

        private bool _isLoadProductFactor;

        private bool _isOpenRecipeOnServer;

        private string _recipePathOnServer;

        private bool _isReworkMode;

        private bool _isOutputFormatByRecipe;

		private int[] _startCountOfEdgeSensor;

		private int[] _endCountOfEdgeSensor;

		private bool _isEnableRIBackupFilePath;

		private string _riBackupFilePath;

		private ERILoadReportMode _riLoadReportMode;

		private string _riLoadReportFilePath;

		private string _riReportSpecFile;

		private string _riSimulatorOptiReport;

		private string _riSimulatorElecReport;

        private string _riSingleSimulatorReport;

        private int _testerCoord;

        private ETesterResultCreatFolderType _byRecipeResultCreateFolderType;
        #endregion

		public UISetting()
		{
			this._lockObj = new object();

            

			this._productFileName = Constants.Files.DEFAULT_FILENAME;
			this._binDataFileName = Constants.Files.DEFAULT_FILENAME;
			this._taskSheetFileName = Constants.Files.DEFAULT_FILENAME;
			this._testResultFileName = Constants.Files.DEFAULT_FILENAME;
			this._mapDataFileName = Constants.Files.DEFAULT_FILENAME;
			this._importCalibrateFileName = string.Empty;
			this._importBinFileName = string.Empty;
			this._importCalibSptDataFileName = string.Empty;

            //this._productPath = Constants.Paths.PRODUCT_FILE;
            //this._productPath02 = Constants.Paths.PRODUCT_FILE02;
            //this._coefTableExportPath = Constants.Paths.MPI_TEMP_DIR;

			this._machineName = "Machine Name";
			this._userID = EUserID.MPI;
			this._userDefinedData = new UserData();
			this._formatName = string.Empty;
			this._formatIndex = 0;

			this._multiLanguage = (int)EMultiLanguage.ENG;



			this._operatorName = "Operator Name";
			this._reporterName = "Reporter Name";
			this._productName = "Product Name";
			this._lotNumber = "Lot22";
			this._cassetteNumber = "Cassette55";
			this._waferNumber = "Wafer33";
			this._barcode = "Barcode66";
			this._substrate = "substrate";
			this._loginID = "Login ID";
			this._slotNumber = "SlotID";
            this._chuckTemperature = 0;

			this._isExtResultItem = false;

			this._mainFormLeftPos = 0;
			this._mainFormTopPos = 0;
			this._userIDList = null;

			this._weiminUIData = new WMData();
			this._uiDisplayType = (int)EUIDisplayType.MPIStartUI;
			this._uiOperateMode = (int)EUIOperateMode.Idle;

			this._mesPath = Constants.Paths.MPI_TEMP_DIR;
			this._mesPath2 = Constants.Paths.MPI_TEMP_DIR;
			this._mesbackupPath = Constants.Paths.MPI_TEMP_DIR;

			this._isSetIFMsrtRange = false;
			this._isSetIZMsrtRange = false;
			this._isSetVFMsrtRange = true;
			this._isSetVRMsrtRange = true;

			this._defaultIFMsrtRange = 8.0d;			// 8.0V
			this._defaultIZMsrtRange = 50.0d;			// 50V
			this._defaultVRMsrtRange = 40.0d;		// 40uA
			this._defaultVFMsrtRange = 20.0d;		// 20mA
			this._displayUIForm = 0;
			this._sendTaskFileName = "";
			this._controlTaskSettingUI = EControlTaskSetting.NONE;
			this._mesOpenFileName = "";
			this._isEnableMesSystem = false;
			this._productType = "";
			this._autoRm2Path = "";
			this._rm2FileName = "";
			this._proberRecipeName = "";
			this._mesConditinFileName = "";
			this._mapGradeFileName = string.Empty;
			this._isDeliverProberRecipe = false;
            this._isSendBarcodeToProbe = false;

			this._wafermapList = new string[4] { "", "", "", "" };
			this._waferPcs = 0;

			this._waferMapAutoBoundery = true;
			this._waferMapLift = 0;
			this._waferMapTop = 0;
			this._waferMapRight = 100;
			this._waferMapBottom = 100;
			this._waferMapGrowthDirection = 0;
			this._isRunDailyCheckMode = false;
			this._isManualRunMode = false;
			this._isRunReTestSortPageMode = false;

			this._isConverterTasksheet = false;
			this._isShowDailyCheck = false;
			this._showMapKeyName = string.Empty;
			this._dailyCheckLogFileName = string.Empty;
			this._daliycheckTime = new List<int>();
			this._isEnableCheckDailyDataIsValid = false;
			this._isEnableMapFormTopMost = false;
			this._isEnableAutoClearMapAndCIEChart = false;

			this._isEnableSaveErrSpectrum = false;
			this._isEnableSaveErrMsrt = false;
			this._isClearHistoryWhenStartTest = false;
            //this._isEnableSaveBackupCoef = false;
            //this._backupCoefPath = Constants.Paths.PRODUCT_FILE02;
			this._reProbingCounts = 0;

			this._probingCount1 = 0;
			this._probingCount2 = 0;
			this._probingCount3 = 0;
			this._probingCount4 = 0;

			this._totalSacnCounts = 0;
			this._isEnableSaveBinTable = false;
			this._binTablePath = Constants.Paths.MPI_SHARE_DIR;

			this._calibrationUIMode = ECalibrationUIMode.T200;
			this._isWriteStatisticsDataToXmlHead = false;

			this._isTCPIPSendEnableResultItem = false;

			this._isShowNPLCAndSGFilterSetting = false;

			this._isCheckStandardRecipe = false;

			this._standRecipePath = Constants.Paths.MPI_TEMP_DIR;

			this._isEnableDuplicateStdRecipe = false;

			this._isCheckDailyVerify = false;

			this._isEnableJudgeRunDailyCheck = false;

			this._runDailyCheckingKeyWord = string.Empty;

			this._isCheckDailyCheckingOverDue = false;

			this._dailyCheckingOverDueHours = -1;

			this._dailyCheckingMonitorPath = Constants.Paths.MPI_TEMP_DIR;

			this._isEnableFloatReport = false;

			this._isWriteReportDirectly = false;

			this._isEnableMonitorPreSamplingData = false;

			this._preSamplingDataPath = Constants.Paths.MPI_TEMP_DIR;

			this._isManualSettingContiousProbingRowCol = false;

			this._contiousProbingPosX = 0;

			this._contiousProbingPosY = 0;

			this._isEnableConditionFormEnCheckBox = true;

			this._wO = string.Empty;

			this._probeMachineName = string.Empty;

			this._probeScanTime = string.Empty;

			this._oPNO = string.Empty;

			this._recipeID = string.Empty;

			this._specID = string.Empty;

			this._isEnableMergeAOIFile = false;

			this._isDeleteAOISourceFile = false;

			this._startTemp = 0.0d;

			this._endTemp = 0.0d;

			this._customerDefineOutputPath = new CustomizeDeifneOutputPath();

			this._isEnableLoadUserTable = false;

			this._txtLoadUserTablePath = Constants.Paths.DATA_FILE;

			this._eqModelName = EEqModelName.LEDA_3GS;

			this._LIVCurveDrawSetting = new LIVCurveDrawSetting();

			this._probingValidCounts = 0;

			this._isShowReportCommentsUI = false;

			this._reportComments = string.Empty;

			this._isAbortSaveFile = false;

			this._uploadTaskSheetFilePath = Constants.Paths.MPI_TEMP_DIR;

            this._uploadProductFilePath = Constants.Paths.MPI_TEMP_DIR;

			this._uploadBinFilePath = Constants.Paths.MPI_TEMP_DIR;

            this._uploadMapFilePath = Constants.Paths.MPI_TEMP_DIR;

			this._isDeleteMESFile = false;

			this._xPitch1 = 0.0d;

			this._yPitch1 = 0.0d;

			this._xPitch2 = 0.0d;

			this._yPitch2 = 0.0d;

			this._xPitch3 = 0.0d;

			this._yPitch3 = 0.0d;

			this._xPitch4 = 0.0d;

			this._yPitch4 = 0.0d;

			this._xPitch5 = 0.0d;

			this._yPitch5 = 0.0d;

    		this._isShowESDJudgeIR = false;

			this._boundarySampleTestMode = 0;

			this._samplingDiePitchCol = 1;

			this._samplingDiePitchRow = 1;

			this._isAutoLoadCalibData = false;

            this._isEnableContinousProbeInOPMode = false;

            this._dataKeepDays = 30;

            this._isEnableAutoDeleteMESFile = false;

            this._isDownRecipeFromServer = false;

            this._isLoadProductFactor = false;

            this._isOpenRecipeOnServer = false;

            this._recipePathOnServer = string.Empty;

            this._isReworkMode = false;

            this._isOutputFormatByRecipe = false;

			this._startCountOfEdgeSensor = new int[8];

			this._endCountOfEdgeSensor = new int[8];

			this._isEnableRIBackupFilePath = false;

			this._riBackupFilePath = Constants.Paths.MPI_TEMP_DIR;

			this._riLoadReportMode = ERILoadReportMode.None;

			this._riLoadReportFilePath = Constants.Paths.MPI_TEMP_DIR;

			this._riReportSpecFile = string.Empty;

			this._riSimulatorOptiReport = string.Empty;

			this._riSimulatorElecReport = string.Empty;

            this._riSingleSimulatorReport = string.Empty;

            this._testerCoord = 4;//傳Group資訊用

            this._probRecipeName = string.Empty;

            this._byRecipeResultCreateFolderType = ETesterResultCreatFolderType.None;

            IsTestResultPathByTaskSheet = false;

            FileInProcessList = new List<string>();

            ProberSubRecipe = "";

            TestTimes = "";//重測次數(一般點測為1,第一次重測為2)
            EdgeSensorName = "";
            SubPiece = "Q00";//預設為聯穎全片的格式

            IsRetest = false;
            IsAppend = false;
            IsAppendForWaferBegine = false;

            PrefixStr = "@";
            ConditionKeyNames = new List<string>();
            //AttenuatorInfo = "";

		}

		#region >> Private Method <<<

		private void CheckIdleTime()
		{
			if (CheckIdleTimeEvent != null)
			{
				CheckIdleTimeEvent();
			}
		}

		#endregion

		#region >> Public Property <<

		public string SendTaskFileName
		{
			get { return this._sendTaskFileName; }
			set { lock (this._lockObj) { this._sendTaskFileName = value; } }
		}

		public string ProductFileName
		{
			get { return this._productFileName; }
			set { lock (this._lockObj) { this._productFileName = value; } }
		}

		public string MapDataFileName
		{
			get { return this._mapDataFileName; }
			set { lock (this._lockObj) { this._mapDataFileName = value; } }
		}

		public string ConditionFileName
		{
			get { return this._productFileName; }
			set { lock (this._lockObj) { this._productFileName = value; } }
		}

		public string BinDataFileName
		{
			get { return this._binDataFileName; }
			set { lock (this._lockObj) { this._binDataFileName = value; } }
		}

		public string TaskSheetFileName
		{
			get { return this._taskSheetFileName; }
			set { lock (this._lockObj) { this._taskSheetFileName = value; } }
		}

		public string TestResultFileName
		{
			get { return this._testResultFileName; }
			set { lock (this._lockObj) { this._testResultFileName = value; } }
		}

		public string ImportBinFileName
		{
			get { return this._importBinFileName; }
			set { lock (this._lockObj) { this._importBinFileName = value; } }
		}

		public string ImportCalibrateFileName
		{
			get { return this._importCalibrateFileName; }
			set { lock (this._lockObj) { this._importCalibrateFileName = value; } }
		}

		public string ImportCalibSptDataFileName
		{
			get { return this._importCalibSptDataFileName; }
			set { lock (this._lockObj) { this._importCalibSptDataFileName = value; } }
		}

		public string ProductPath
		{
            get { return this._productPathInfo.TestResultPath; }
            set { lock (this._lockObj) { this._productPathInfo.TestResultPath = value; } }
		}

		public string ProductPath02
		{
            get { return this._productPathInfo2.TestResultPath; }
            set { lock (this._lockObj) { this._productPathInfo2.TestResultPath = value; } }
		}

		public string CoefTableExportPath
		{
            get { return this._coefTablePathInfo.TestResultPath; }
            set { lock (this._lockObj) { this._coefTablePathInfo.TestResultPath = value; } }
		}


		public string MachineName
		{
			get { return this._machineName; }
			set { lock (this._lockObj) { this._machineName = value; } }
		}

		[XmlIgnore]
		public EUserID UserID
		{
			get { return this._userID; }
			set { lock (this._lockObj) { this._userID = value; } }
		}

		public int UserIDNumber
		{
			get { return (int)this._userID; }
			set
			{
				lock (this._lockObj)
				{
					if (Enum.IsDefined(typeof(EUserID), value))
					{
						this._userID = (EUserID)value;
					}
					else
					{
						this._userID = EUserID.MPI;
					}
				}
			}
		}
		public UserData UserDefinedData
		{
			get { return this._userDefinedData; }
		}

		public string FormatName
		{
			get { return this._formatName; }
			set { lock (this._lockObj) { this._formatName = value; } }
		}

		public uint FormatIndex
		{
			get { return this._formatIndex; }
			set { lock (this._lockObj) { this._formatIndex = value; } }
		}

		public int MultiLanguage
		{
			get { return this._multiLanguage; }
			set { lock (this._lockObj) { this._multiLanguage = value; } }
		}

		public string OperatorName
		{
			get { return this._operatorName; }
			set { lock (this._lockObj) { this._operatorName = value; } }
		}

		public string LoginID
		{
			get { return this._loginID; }
			set { lock (this._lockObj) { this._loginID = value; } }
		}

		public string ReporterName
		{
			get { return this._reporterName; }
			set { lock (this._lockObj) { this._reporterName = value; } }
		}

		public string ProductName
		{
			get { return this._productName; }
			set { lock (this._lockObj) { this._productName = value; } }
		}

		public string ProductType
		{
			get { return this._productType; }
			set { lock (this._lockObj) { this._productType = value; } }
		}

		public string LotNumber
		{
			get { return this._lotNumber; }
			set { lock (this._lockObj) { this._lotNumber = value; } }
		}
        public double ChuckTemprature
        {
            get { return this._chuckTemperature; }
            set { lock (this._lockObj) { this._chuckTemperature = value; } }
        }


		public string CassetteNumber
		{
			get { return this._cassetteNumber; }
			set { lock (this._lockObj) { this._cassetteNumber = value; } }
		}

		public string WaferNumber
		{
			get { return this._waferNumber; }
			set { lock (this._lockObj) { this._waferNumber = value; } }
		}

		public string Barcode
		{
			get { return this._barcode; }
			set { lock (this._lockObj) { this._barcode = value; } }
		}


        //public string ProbRecipeName
        //{
        //    get { return this._probRecipeName; }
        //    set { lock (this._lockObj) { this._probRecipeName = value; } }
        //}
        public string Substrate
		{
			get { return this._substrate; }
			set { lock (this._lockObj) { this._substrate = value; } }
		}

		public string KeyNumber
		{
			get { return this._keyNumber; }
			set { lock (this._lockObj) { this._keyNumber = value; } }
		}

		public bool IsExtResultItem
		{
			get { return this._isExtResultItem; }
			set { lock (this._lockObj) { this._isExtResultItem = value; } }
		}

		public int MainFormLeftPos
		{
			get { return this._mainFormLeftPos; }
			set
			{
				lock (this._lockObj)
				{
					if (value >= 0)
					{
						this._mainFormLeftPos = value;
					}
					else
					{
						this._mainFormLeftPos = 0;
					}
				}
			}
		}

		public int MainFormTopPos
		{
			get { return this._mainFormTopPos; }
			set
			{
				lock (this._lockObj)
				{
					if (value >= 0)
					{
						this._mainFormTopPos = value;
					}
					else
					{
						this._mainFormTopPos = 0;
					}
				}
			}
		}

		public string[] UserIDList
		{
			get { return this._userIDList; }
			set { lock (this._lockObj) { this._userIDList = value; } }
		}

		public WMData WeiminUIData
		{
			get { return this._weiminUIData; }
			set { lock (this._lockObj) { this._weiminUIData = value; } }
		}

		public int UIDisplayType
		{
			get { return this._uiDisplayType; }
			set { lock (this._lockObj) { this._uiDisplayType = value; } }
		}

		public int UIOperateMode
		{
			get { return this._uiOperateMode; }
			set { this._uiOperateMode = value; }
		}

		public EAuthority AuthorityLevel
		{
			get { return this._authorityLevel; }
			set 
			{
				lock (this._lockObj) 
				{
					bool isChange = this._authorityLevel != value;

					this._authorityLevel = value;

					if (isChange)
					{
						this.CheckIdleTime();
					}
				} 
			}
		}

		public bool IsSetIFMsrtRange
		{
			get { return this._isSetIFMsrtRange; }
			set { lock (this._lockObj) { this._isSetIFMsrtRange = value; } }
		}

		public bool IsSetIZMsrtRange
		{
			get { return this._isSetIZMsrtRange; }
			set { lock (this._lockObj) { this._isSetIZMsrtRange = value; } }
		}

		public bool IsSetVFMsrtRange
		{
			get { return this._isSetVFMsrtRange; }
			set { lock (this._lockObj) { this._isSetVFMsrtRange = value; } }
		}

		public bool IsSetVRMsrtRange
		{
			get { return this._isSetVRMsrtRange; }
			set { lock (this._lockObj) { this._isSetVRMsrtRange = value; } }
		}

		public double DefaultIFMsrtRange
		{
			get { return this._defaultIFMsrtRange; }
			set { lock (this._lockObj) { this._defaultIFMsrtRange = value; } }
		}

		public double DefaultIZMsrtRange
		{
			get { return this._defaultIZMsrtRange; }
			set { lock (this._lockObj) { this._defaultIZMsrtRange = value; } }
		}

		public double DefaultVFMsrtRange
		{
			get { return this._defaultVFMsrtRange; }
			set { lock (this._lockObj) { this._defaultVFMsrtRange = value; } }
		}

		public double DefaultVRMsrtRange
		{
			get { return this._defaultVRMsrtRange; }
			set { lock (this._lockObj) { this._defaultVRMsrtRange = value; } }
		}

		public int TopDisplayUIForm
		{
			get { return this._displayUIForm; }
			set { lock (this._lockObj) { this._displayUIForm = value; } }
		}

		public EControlTaskSetting ControlTaskSettingUI
		{
			get { return this._controlTaskSettingUI; }
			set { lock (this._lockObj) { this._controlTaskSettingUI = value; } }
		}

		public string MESOpenFileName
		{
			get { return this._mesOpenFileName; }
			set { lock (this._lockObj) { this._mesOpenFileName = value; } }
		}

		public string MESPath
		{
			get { return this._mesPath; }
			set { lock (this._lockObj) { this._mesPath = value; } }
		}

		public string MESPath2
		{
			get { return this._mesPath2; }
			set { lock (this._lockObj) { this._mesPath2 = value; } }
		}

		public string MESBackupPath
		{
			get { return this._mesbackupPath; }
			set { lock (this._lockObj) { this._mesbackupPath = value; } }
		}

		public bool IsEnableRunMesSystem
		{
			get { return this._isEnableMesSystem; }
			set { lock (this._lockObj) { this._isEnableMesSystem = value; } }
		}

		public bool IsDeliverProberRecipe
		{
			get { return this._isDeliverProberRecipe; }
			set { lock (this._lockObj) { this._isDeliverProberRecipe = value; } }
		}

        public bool IsSendBarcodeToProbe
        {
            get { return this._isSendBarcodeToProbe; }
            set { lock (this._lockObj) { this._isSendBarcodeToProbe = value; } }
        }

		public string[] WaferMapList
		{
			get { return this._wafermapList; }
			set { lock (this._lockObj) { this._wafermapList = value; } }
		}

		public int WaferMapGrowthDirection
		{
			get { return this._waferMapGrowthDirection; }
			set { lock (this._lockObj) { this._waferMapGrowthDirection = value; } }
		}

		public string AutoRm2Path
		{
			get { return this._autoRm2Path; }
			set { lock (this._lockObj) { this._autoRm2Path = value; } }
		}

		public string ProberRecipeName
		{
			get { return this._proberRecipeName; }
			set { lock (this._lockObj) { this._proberRecipeName = value; } }
		}

		public string RM2FileName
		{
			get { return this._rm2FileName; }
			set { lock (this._lockObj) { this._rm2FileName = value; } }
		}

		public string MapGradeFileName
		{
			get { return this._mapGradeFileName; }
			set { lock (this._lockObj) { this._mapGradeFileName = value; } }
		}

		public int WaferPcs
		{
			get { return this._waferPcs; }
			set { lock (this._lockObj) { this._waferPcs = value; } }
		}

		public string MESConditinFileName
		{
			get { return this._mesConditinFileName; }
			set { lock (this._lockObj) { this._mesConditinFileName = value; } }
		}

		public bool WaferMapAutoBoundery
		{
			get { return this._waferMapAutoBoundery; }
			set { lock (this._lockObj) { this._waferMapAutoBoundery = value; } }
		}

		public int WaferMapLeft
		{
			get { return this._waferMapLift; }
			set { lock (this._lockObj) { this._waferMapLift = value; } }
		}

		public int WaferMapTop
		{
			get { return this._waferMapTop; }
			set { lock (this._lockObj) { this._waferMapTop = value; } }
		}

		public int WaferMapRight
		{
			get { return this._waferMapRight; }
			set { lock (this._lockObj) { this._waferMapRight = value; } }
		}

		public int WaferMapBottom
		{
			get { return this._waferMapBottom; }
			set { lock (this._lockObj) { this._waferMapBottom = value; } }
		}

		public bool IsRunDailyCheckMode
		{
			get { return this._isRunDailyCheckMode; }
			set { lock (this._lockObj) { this._isRunDailyCheckMode = value; } }
		}

		public bool IsManualRunMode
		{
			get { return this._isManualRunMode; }
			set { lock (this._lockObj) { this._isManualRunMode = value; } }
		}

		public bool IsRunReTestSortPageMode
		{
			get { return this._isRunReTestSortPageMode; }
			set { lock (this._lockObj) { this._isRunReTestSortPageMode = value; } }
		}

		public bool IsConverterTasksheet
		{
			get { return this._isConverterTasksheet; }
			set { lock (this._lockObj) { this._isConverterTasksheet = value; } }
		}

		public bool IsShowDailyCheckUI
		{
			get { return this._isShowDailyCheck; }
			set { lock (this._lockObj) { this._isShowDailyCheck = value; } }
		}

		public string ShowMapKeyName
		{
			get { return this._showMapKeyName; }
			set { lock (this._lockObj) { this._showMapKeyName = value; } }
		}

		public string DailyCheckLogFileName
		{
			get { return this._dailyCheckLogFileName; }
			set { lock (this._lockObj) { this._dailyCheckLogFileName = value; } }
		}

		public List<int> DailyCheckTime
		{
			get { return this._daliycheckTime; }
			set { lock (this._lockObj) { this._daliycheckTime = value; } }
		}

		public bool IsEnableCheckDailyDataIsValid
		{
			get { return this._isEnableCheckDailyDataIsValid; }
			set { lock (this._lockObj) { this._isEnableCheckDailyDataIsValid = value; } }
		}

		public bool IsEnableMapFormTopMost
		{
			get { return this._isEnableMapFormTopMost; }
			set { lock (this._lockObj) { this._isEnableMapFormTopMost = value; } }
		}

		public bool IsEnableAutoClearMapAndCIEChart
		{
			get { return this._isEnableAutoClearMapAndCIEChart; }
			set { lock (this._lockObj) { this._isEnableAutoClearMapAndCIEChart = value; } }
		}

		public bool IsEnableSaveErrSpectrum
		{
			get { return this._isEnableSaveErrSpectrum; }
			set { lock (this._lockObj) { this._isEnableSaveErrSpectrum = value; } }
		}

		public bool IsEnableSaveErrMsrt
		{
			get { return this._isEnableSaveErrMsrt; }
			set { lock (this._lockObj) { this._isEnableSaveErrMsrt = value; } }
		}

		public bool IsEnableClearHistoryWhenStartTest
		{
			get { return this._isClearHistoryWhenStartTest; }
			set { lock (this._lockObj) { this._isClearHistoryWhenStartTest = value; } }
		}

		public bool IsEnableSaveBackupCoef
		{
			get { return this._coefBackupPathInfo.EnablePath; }
			set { lock (this._lockObj) { this._coefBackupPathInfo.EnablePath = value; } }
		}

		public string BackupCoefPath
		{
            get { return this._coefBackupPathInfo.TestResultPath; }
            set { lock (this._lockObj) { this._coefBackupPathInfo.TestResultPath = value; } }
		}

		public int ReProbingCounts
		{
			get { return this._reProbingCounts; }
			set { lock (this._lockObj) { this._reProbingCounts = value; } }
		}

		public uint ProbingCount1
		{
			get { return this._probingCount1; }
			set { lock (this._lockObj) { this._probingCount1 = value; } }
		}

		public uint ProbingCount2
		{
			get { return this._probingCount2; }
			set { lock (this._lockObj) { this._probingCount2 = value; } }
		}

		public uint ProbingCount3
		{
			get { return this._probingCount3; }
			set { lock (this._lockObj) { this._probingCount3 = value; } }
		}

		public uint ProbingCount4
		{
			get { return this._probingCount4; }
			set { lock (this._lockObj) { this._probingCount4 = value; } }
		}

		public uint TotalSacnCounts
		{
			get { return this._totalSacnCounts; }
			set { lock (this._lockObj) { this._totalSacnCounts = value; } }
		}

		public bool IsEnableSaveBinTable
		{
			get { return this._isEnableSaveBinTable; }
			set { lock (this._lockObj) { this._isEnableSaveBinTable = value; } }
		}

		public string BinTablePath
		{
			get { return this._binTablePath; }
			set { lock (this._lockObj) { this._binTablePath = value; } }
		}

		public ECalibrationUIMode CalibrationUIMode
		{
			get { return this._calibrationUIMode; }
			set { lock (this._lockObj) { this._calibrationUIMode = value; } }
		}

		public bool IsWriteStatisticsDataToXmlHead
		{
			get { return this._isWriteStatisticsDataToXmlHead; }
			set { lock (this._lockObj) { this._isWriteStatisticsDataToXmlHead = value; } }
		}

		public bool IsTCPIPSendEnableResultItem
		{
			get { return this._isTCPIPSendEnableResultItem; }
			set { lock (this._lockObj) { this._isTCPIPSendEnableResultItem = value; } }
		}

		public bool IsShowNPLCAndSGFilterSetting
		{
			get { return this._isShowNPLCAndSGFilterSetting; }
			set { lock (this._lockObj) { this._isShowNPLCAndSGFilterSetting = value; } }
		}

		public bool IsCheckStandardRecipe
		{
			get { return this._isCheckStandardRecipe; }
			set { lock (this._lockObj) { this._isCheckStandardRecipe = value; } }
		}

		public string StandRecipePath
		{
			get { return this._standRecipePath; }
			set { lock (this._lockObj) { this._standRecipePath = value; } }
		}

		public bool IsEnableDuplicateStdRecipe
		{
			get { return this._isEnableDuplicateStdRecipe; }
			set { lock (this._lockObj) { this._isEnableDuplicateStdRecipe = value; } }
		}

		public bool IsCheckDailyVerifyResult
		{
			get { return this._isCheckDailyVerify; }
			set { lock (this._lockObj) { this._isCheckDailyVerify = value; } }
		}

		public bool IsEnableJudgeRunDailyCheck
		{
			get { return this._isEnableJudgeRunDailyCheck; }
			set { lock (this._lockObj) { this._isEnableJudgeRunDailyCheck = value; } }
		}

		public string RunDailyCheckingKeyWord
		{
			get { return this._runDailyCheckingKeyWord; }
			set { lock (this._lockObj) { this._runDailyCheckingKeyWord = value; } }
		}

		public bool IsCheckDailyCheckingOverDue
		{
			get { return this._isCheckDailyCheckingOverDue; }
			set { lock (this._lockObj) { this._isCheckDailyCheckingOverDue = value; } }
		}

		public int DailyCheckingOverDueHours
		{
			get { return this._dailyCheckingOverDueHours; }
			set { lock (this._lockObj) { this._dailyCheckingOverDueHours = value; } }
		}

		public string DailyCheckingMonitorPath
		{
			get { return this._dailyCheckingMonitorPath; }
			set { lock (this._lockObj) { this._dailyCheckingMonitorPath = value; } }
		}

		public bool IsEnableFloatReport
		{
			get { return this._isEnableFloatReport; }
			set { lock (this._lockObj) { this._isEnableFloatReport = value; } }
		}

		public bool IsWriteReportDirectly
		{
			get { return this._isWriteReportDirectly; }
			set { lock (this._lockObj) { this._isWriteReportDirectly = value; } }
		}

		public bool IsEnableMonitorPreSamplingData
		{
			get { return this._isEnableMonitorPreSamplingData; }
			set { lock (this._lockObj) { this._isEnableMonitorPreSamplingData = value; } }
		}

		public string PreSamplingDataPath
		{
			get { return this._preSamplingDataPath; }
			set { lock (this._lockObj) { this._preSamplingDataPath = value; } }
		}
        public bool IsEnableLaodGroupData
        {
            get { return this._isEnableLoadGroupData; }
            set { lock (this._lockObj) { this._isEnableLoadGroupData = value; } }
        }
        public string GroupDataPath
        {
            get { return this._groupDataPath; }
            set { lock (this._lockObj) { this._groupDataPath = value; } }
        }
		public bool IsManualSettingContiousProbingRowCol
		{
			get { return this._isManualSettingContiousProbingRowCol; }
			set { lock (this._lockObj) { this._isManualSettingContiousProbingRowCol = value; } }
		}

		public int ContiousProbingPosX
		{
			get { return this._contiousProbingPosX; }
			set { lock (this._lockObj) { this._contiousProbingPosX = value; } }
		}

		public int ContiousProbingPosY
		{
			get { return this._contiousProbingPosY; }
			set { lock (this._lockObj) { this._contiousProbingPosY = value; } }
		}

		public bool IsEnableConditionFormEnCheckBox
		{
			get { return this._isEnableConditionFormEnCheckBox; }
			set { lock (this._lockObj) { this._isEnableConditionFormEnCheckBox = value; } }
		}

		public CustomizeDeifneOutputPath customizeDefineOutputPath
		{
			get { return this._customerDefineOutputPath; }
			set { lock (this._lockObj) { this._customerDefineOutputPath = value; } }
		}

		public string WO
		{
			get { return this._wO; }
			set { lock (this._lockObj) { this._wO = value; } }
		}

		public string ProbeMachineName
		{
			get { return this._probeMachineName; }
			set { lock (this._lockObj) { this._probeMachineName = value; } }
		}

		public string OPNO
		{
			get { return this._oPNO; }
			set { lock (this._lockObj) { this._oPNO = value; } }
		}

		public string ProbeScanTime
		{
			get { return this._probeScanTime; }
			set { lock (this._lockObj) { this._probeScanTime = value; } }
		}

		public string RecipeID
		{
			get { return this._recipeID; }
			set { lock (this._lockObj) { this._recipeID = value; } }
		}

		public string SpecID
		{
			get { return this._specID; }
			set { lock (this._lockObj) { this._specID = value; } }
		}

		public string SlotNumber
		{
			get { return this._slotNumber; }
			set { lock (this._lockObj) { this._slotNumber = value; } }
		}

		public bool IsEnableMergeAOIFile
		{
			get { return this._isEnableMergeAOIFile; }
			set { lock (this._lockObj) { this._isEnableMergeAOIFile = value; } }
		}

		public bool IsDeletePBAOISourceFile
		{
			get { return this._isDeleteAOISourceFile; }
			set { lock (this._lockObj) { this._isDeleteAOISourceFile = value; } }
		}

		public double StartTemp
		{
			get { return this._startTemp; }
			set { lock (this._lockObj) { this._startTemp = value; } }
		}

		public double EndTemp
		{
			get { return this._endTemp; }
			set { lock (this._lockObj) { this._endTemp = value; } }
		}

		public bool IsEnableLoadUserTable
		{
			get { return this._isEnableLoadUserTable; }
			set { lock (this._lockObj) { this._isEnableLoadUserTable = value; } }
		}

		public string LoadUserTablePath
		{
			get { return this._txtLoadUserTablePath; }
			set { lock (this._lockObj) { this._txtLoadUserTablePath = value; } }
		}

		public EEqModelName MESMachineName
		{
			get { return this._eqModelName; }
			set { lock (this._lockObj) { this._eqModelName = value; } }
		}

		public LIVCurveDrawSetting LIVDrawSetting
		{
			get { return this._LIVCurveDrawSetting; }
			set { lock (this._lockObj) { this._LIVCurveDrawSetting = value; } }
		}

		public int ProbingValidCounts
		{
			get { return this._probingValidCounts; }
			set { lock (this._lockObj) { this._probingValidCounts = value; } }
		}

		public bool IsShowReportCommentsUI
		{
			get { return this._isShowReportCommentsUI; }
			set { lock (this._lockObj) { this._isShowReportCommentsUI = value; } }
		}

		public string ReportComments
		{
			get { return this._reportComments; }
			set { lock (this._lockObj) { this._reportComments = value; } }
		}

        //public bool IsEnableSaveSweepData
        //{
        //    get { return base._isEnableSaveSweepData; }
        //    set { lock (this._lockObj) { base._isEnableSaveSweepData = value; } }
        //}

        //public string SweepDataSavePath
        //{
        //    get { return base._sweepDataSavePath; }
        //    set { lock (this._lockObj) { base._sweepDataSavePath = value; } }
        //}


		public bool IsAbortSaveFile
		{
			get { return this._isAbortSaveFile; }
			set { lock (this._lockObj) { this._isAbortSaveFile = value; } }
		}

        public string UploadTaskSheetFilePath
        {
            get { return this._uploadTaskSheetFilePath; }
			set { lock (this._lockObj) { this._uploadTaskSheetFilePath = value; } }
        }

		public string UploadProductFilePath
		{
			get { return this._uploadProductFilePath; }
			set { lock (this._lockObj) { this._uploadProductFilePath = value; } }
		}

		public string UploadBinFilePath
		{
			get { return this._uploadBinFilePath; }
			set { lock (this._lockObj) { this._uploadBinFilePath = value; } }
		}

        public string UploadMapFilePath
        {
            get { return this._uploadMapFilePath; }
            set { lock (this._lockObj) { this._uploadMapFilePath = value; } }
        }

		public bool IsDeleteMESFile
		{
			get { return this._isDeleteMESFile; }
			set { lock (this._lockObj) { this._isDeleteMESFile = value; } }
		}

		public int BoundarySampleTestMode
		{
			get { return this._boundarySampleTestMode; }
			set { lock (this._lockObj) { this._boundarySampleTestMode = value; } }
		}

		public double XPitch1
		{
			get { return this._xPitch1; }
			set { lock (this._lockObj) { this._xPitch1 = value; } }
		}

		public double YPitch1
		{
			get { return this._yPitch1; }
			set { lock (this._lockObj) { this._yPitch1 = value; } }
		}

		public double XPitch2
		{
			get { return this._xPitch2; }
			set { lock (this._lockObj) { this._xPitch2 = value; } }
		}

		public double YPitch2
		{
			get { return this._yPitch2; }
			set { lock (this._lockObj) { this._yPitch2 = value; } }
		}

		public double XPitch3
		{
			get { return this._xPitch3; }
			set { lock (this._lockObj) { this._xPitch3 = value; } }
		}

		public double YPitch3
		{
			get { return this._yPitch3; }
			set { lock (this._lockObj) { this._yPitch3 = value; } }
		}

		public double XPitch4
		{
			get { return this._xPitch4; }
			set { lock (this._lockObj) { this._xPitch4 = value; } }
		}

		public double YPitch4
		{
			get { return this._yPitch4; }
			set { lock (this._lockObj) { this._yPitch4 = value; } }
		}

		public double XPitch5
		{
			get { return this._xPitch5; }
			set { lock (this._lockObj) { this._xPitch5 = value; } }
		}

		public double YPitch5
		{
			get { return this._yPitch5; }
			set { lock (this._lockObj) { this._yPitch5 = value; } }
		}

		   public bool IsShowESDJudgeIR
 		 {
            get { return _isShowESDJudgeIR; }
            set { lock (this._lockObj) { this._isShowESDJudgeIR = value; } }
        }

		public int SamplingDiePitchCol
		{
			get { return this._samplingDiePitchCol; }
			set { lock (this._lockObj) { this._samplingDiePitchCol = value; } }
		}

		public int SamplingDiePitchRow
		{
			get { return this._samplingDiePitchRow; }
			set { lock (this._lockObj) { this._samplingDiePitchRow = value; } }
		}        

		public bool IsAbortSaveReport
        {
            get { return this._isAbortSaveReport; }
            set { lock (this._lockObj) { this._isAbortSaveReport = value; } }
        }

        public bool IsAutoLoadCalibData
        {
            get { return this._isAutoLoadCalibData; }
            set { lock (this._lockObj) { this._isAutoLoadCalibData = value; } }
        }

        public bool IsEnableContinousProbeInOPMode
        {
            get { return this._isEnableContinousProbeInOPMode; }
            set { lock (this._lockObj) { this._isEnableContinousProbeInOPMode = value; } }
        }

        public uint DataKeepDays
        {
            get { return this._dataKeepDays; }
            set { lock (this._lockObj) { this._dataKeepDays = value; } }
        }

        public bool IsEnableAutoDeleteMESFile
        {
            get { return this._isEnableAutoDeleteMESFile; }
            set { lock (this._lockObj) { this._isEnableAutoDeleteMESFile = value; } }
        }

        public string MESPath3
        {
            get { return this._mesPath3; }
            set { lock (this._lockObj) { this._mesPath3 = value; } }
        }

        public string MESPath4
        {
            get { return this._mesPath4; }
            set { lock (this._lockObj) { this._mesPath4 = value; } }
        }

        public bool IsDownRecipeFromServer
        {
            get { return this._isDownRecipeFromServer; }
            set { lock (_lockObj) { this._isDownRecipeFromServer = value; } }
        }

        public bool IsLoadProductFactor
        {
            get { return this._isLoadProductFactor; }
            set { lock (this._lockObj) { this._isLoadProductFactor = value; } }
        }

        public string RecipePathOnServer
        {
            get { return this._recipePathOnServer; }
            set { lock (this._lockObj) { this._recipePathOnServer = value; } }
        }

        public bool IsOpenRecipeOnServer
        {
            get { return this._isOpenRecipeOnServer; }
            set { lock (this._lockObj) { this._isOpenRecipeOnServer = value; } }
        }

        public bool IsReworkMode
        {
            get { return this._isReworkMode; }
            set { lock (this._lockObj) { this._isReworkMode = value; } }
        }

        public bool IsOutputFormatByRecipe
        {
            get { return this._isOutputFormatByRecipe; }
            set { lock (this._lockObj) { this._isOutputFormatByRecipe = value; } }
        }

		public int[] StartCountOfEdgeSensor
		{
			get { return this._startCountOfEdgeSensor; }
			set { lock (this._lockObj) { this._startCountOfEdgeSensor = value; } }
		}

		public int[] EndCountOfEdgeSensor
		{
			get { return this._endCountOfEdgeSensor; }
			set { lock (this._lockObj) { this._endCountOfEdgeSensor = value; } }
		}

		public bool IsEnableRIBackupFilePath
		{
			get { return this._isEnableRIBackupFilePath; }
			set { lock (this._lockObj) { this._isEnableRIBackupFilePath = value; } }
		}

		public string RIBackupFilePath
		{
			get { return this._riBackupFilePath; }
			set { lock (this._lockObj) { this._riBackupFilePath = value; } }
		}

		public ERILoadReportMode RILoadReportMode
		{
			get { return this._riLoadReportMode; }
			set { lock (this._lockObj) { this._riLoadReportMode = value; } }
		}

		public string RILoadReportFilePath
		{
			get { return this._riLoadReportFilePath; }
			set { lock (this._lockObj) { this._riLoadReportFilePath = value; } }
		}

		public string RIReportSpecFile
		{
			get { return this._riReportSpecFile; }
			set { lock (this._lockObj) { this._riReportSpecFile = value; } }
		}

		public string RISimulatorOptiReport
		{
			get { return this._riSimulatorOptiReport; }
			set { lock (this._lockObj) { this._riSimulatorOptiReport = value; } }
		}

		public string RISimulatorElecReport
		{
			get { return this._riSimulatorElecReport; }
			set { lock (this._lockObj) { this._riSimulatorElecReport = value; } }
		}

        public string RISingleSimulatorReport
        {
            get { return this._riSingleSimulatorReport; }
            set { lock (this._lockObj) { this._riSingleSimulatorReport = value; } }
        }

        public int TesterCoord
        {
            get { return this._testerCoord; }
            set { lock (this._lockObj) { this._testerCoord = value; } }
        }

        public ETesterResultCreatFolderType ByRecipeResultCreateFolderType
        {
            get { return this._byRecipeResultCreateFolderType; }
            set { lock (this._lockObj) { this._byRecipeResultCreateFolderType = value; } }
        }

        public bool IsTestResultPathByTaskSheet { get; set; }

        public string PrefixStr { get; set; }

        public List<string> ConditionKeyNames { get; set; }

        [XmlIgnore]
        public string SoftwareVersoin
        { get;set;}
		[XmlIgnore]
        public string ProberSubRecipe
        { set; get; }


        [XmlIgnore]
        public string SubPiece//破片/全片
        {get; set; }
        [XmlIgnore]
        public string TestTimes//重測次數(一般點測為1,第一次重測為2)
        { set; get; }
        [XmlIgnore]
        public string EdgeSensorName
        { set; get; }
        [XmlIgnore]
        public DateTime WaferBeginTime
        { set; get; }
        [XmlIgnore]
        public bool IsRetest { set; get; }
        [XmlIgnore]
        public bool IsAppend { set; get; }
        [XmlIgnore]
        public bool IsAppendForWaferBegine { set; get; }//聯穎的毛

        [XmlIgnore]
        public List<string> FileInProcessList { set; get; }

        public string Remark { set; get; }

        //[XmlIgnore]
        //public string AttenuatorInfo { set; get; }

		#endregion

		#region >> Public Method <<<
        public virtual string GetPathWithFolder(PathInfo pInfo)
        {
            string folderName = "";
            string outPath = pInfo.TestResultPath;
            switch (pInfo.FolderType)
            {
                case ETesterResultCreatFolderType.ByDataTime:
                    folderName = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByLotNumber:
                    folderName = this.LotNumber;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByMachineName:
                    folderName = this.MachineName;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByWaferID:
                    folderName = this.WaferNumber;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByLotNumber_WaferID:
                    folderName = Path.Combine(this.LotNumber, this.WaferNumber);
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByBarcode:
                    folderName = this.Barcode;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                  case ETesterResultCreatFolderType.ByLot_WaferID_Times:
                    string tfolderName = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                    folderName = Path.Combine(this.LotNumber, this.WaferNumber, tfolderName);
                    outPath = Path.Combine(outPath, folderName);
                    break;  
            }

            return outPath;
        }

		public object Clone()
		{
			UISetting obj = this.MemberwiseClone() as UISetting;

			obj._lockObj = new object();
			return (object)obj;
		}

		#endregion
	}


}
