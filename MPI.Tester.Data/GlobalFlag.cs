using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	public class GlobalFlag
	{
		private static object _slockObj = new Object();

        private static bool _is64bit;

		private static bool _isSuccessLoadBin;
		private static bool _isSuccessLoadProduct;
		private static bool _isEnableShowMap;
		private static int _seqStep;
		private static int _optimumStatus;
        private static bool _isReSingleTest;
        private static bool _isSuccessLoadMESData;
        private static bool _isFinishPrintBarcode;
        private static bool _isSuccessCheckFilterWheel;
        private static bool _isTestResultFileEmpty;
        private static bool _isDailyCheckFail;
        private static bool _isSuccessCheckChannelConfig;
		private static bool _isSourceMeterDisconnect;
        private static bool _isPreSamplingCheckSuccess;
		private static bool _isDataRecoveryPushSuccess;
		private static bool _isContinueMode;
        private static bool _isPassRateCheckSuccess;
        private static bool _isProductChannelConditionNotMatch;
        private static bool _isDeviceVerifyTest;
		private static EOutputReportState _outputReportState;
		private static bool _isOperatorEmpty;

		private static bool _isStopPushData;
        private static bool _isEnableSendBarcodeToProbe;

        private static bool _isCameraLiveMode;

        private static bool _isEnoughFreeHDSpace;
        private static bool _isNftSaturation;

        private static bool _isSystemReady;

        private static ETesterTestMode _testerTestMode;

        private static bool _isAbleEndTest = true;

        private static EOutputReportState _proberAssignMode;

		#region >>> Public Property <<<

        public static bool Is64bit
        {
            get { return _is64bit; }
            set { lock (_slockObj) { _is64bit = value; } }
        }

		public static bool IsSuccessLoadProduct
		{
			get { return _isSuccessLoadProduct; }
			set { lock (_slockObj) { _isSuccessLoadProduct = value; } }
		}

		public static bool IsSuccessLoadBin
		{
			get { return _isSuccessLoadBin; }
			set { lock (_slockObj) { _isSuccessLoadBin = value; } }
		}

		public static bool IsEnableShowMap
		{
			get { return _isEnableShowMap; }
			set { lock (_slockObj) { _isEnableShowMap = value; } }
		}

		public static int SeqStep
		{
			get { return _seqStep; }
			set { lock (_slockObj) { _seqStep = value; } }
		}

		public static int OptimumStatus
		{
			get { return _optimumStatus; }
			set { lock (_slockObj) { _optimumStatus = value; } }
		}

        public static bool IsReSingleTestMode
        {
            get { return _isReSingleTest; }
            set { lock (_slockObj) { _isReSingleTest = value; } }
        }

        public static bool IsSuccessLoadMESData
        {
            get { return _isSuccessLoadMESData; }
            set { lock (_slockObj) { _isSuccessLoadMESData = value; } }
        }

        public static bool IsSuccessCheckFilterWheel
        {
            get { return _isSuccessCheckFilterWheel; }
            set { lock (_slockObj) { _isSuccessCheckFilterWheel = value; } }
        }

        public static bool IsFinishPrintBarcode
        {
            get { return _isFinishPrintBarcode; }
            set { lock (_slockObj) {_isFinishPrintBarcode = value; } }
        }

        public static bool IsTestResultFileEmpty
        {
            get { return _isTestResultFileEmpty; }
            set { lock (_slockObj) { _isTestResultFileEmpty = value; } }
        }

        public static bool IsDailyCheckFail
        {
            get { return _isDailyCheckFail; }
            set { lock (_slockObj) { _isDailyCheckFail = value; } }
        }

        public static bool IsSuccessCheckChannelConfig
        {
            get { return _isSuccessCheckChannelConfig; }
            set { lock (_slockObj) { _isSuccessCheckChannelConfig = value; } }
        }

		public static bool IsSourceMeterDisconnect
		{
			get { return _isSourceMeterDisconnect; }
			set { lock (_slockObj) { _isSourceMeterDisconnect = value; } }
		}

        public static bool IsPreSamplingCheckSuccess
        {
            get { return _isPreSamplingCheckSuccess; }
            set { lock (_slockObj) { _isPreSamplingCheckSuccess = value; } }
        }

		public static bool IsDataRecoveryPushSuccess
        {
			get { return _isDataRecoveryPushSuccess; }
			set { lock (_slockObj) { _isDataRecoveryPushSuccess = value; } }
        }

		public static bool IsContinueMode
		{
			get { return _isContinueMode; }
			set { lock (_slockObj) { _isContinueMode = value; } }
        }

        public static bool IsPassRateCheckSuccess
        {
            get { return _isPassRateCheckSuccess; }
            set { lock (_slockObj) { _isPassRateCheckSuccess = value; } }
        }

        public static bool IsProductChannelConditionNotMatch
        {
            get { return _isProductChannelConditionNotMatch; }
            set { lock (_slockObj) { _isProductChannelConditionNotMatch = value; } }
        }
		
        public static bool IsDeviceVerifyMode
        {
            get { return _isDeviceVerifyTest; }
            set { lock (_slockObj) { _isDeviceVerifyTest = value; } }
        }
		
		public static EOutputReportState OutputReportState
		{
			get { return _outputReportState; }
			set { lock (_slockObj) { _outputReportState = value; } }
		}

		public static bool IsStopPushData
		{
			get { return _isStopPushData; }
			set { lock (_slockObj) { _isStopPushData = value; } }
		}

		public static bool IsOperatorEmpty
		{
			get { return _isOperatorEmpty; }
			set { lock (_slockObj) { _isOperatorEmpty = value; } }
		}

        public static bool IsEnableSendBarcodeToProbe
        {
            get { return _isEnableSendBarcodeToProbe; }
            set { lock (_slockObj) { _isEnableSendBarcodeToProbe = value; } }
        }

        public static bool IsCameraLiveMode
        {
            get { return _isCameraLiveMode; }
            set { lock (_slockObj) { _isCameraLiveMode = value; } }
        }

        public static bool IsEnoughFreeHDSpace
        {
            get { return _isEnoughFreeHDSpace; }
            set { lock (_slockObj) { _isEnoughFreeHDSpace = value; } }
        }

        public static bool IsNftSaturation
        {
            get { return _isNftSaturation; }
            set { lock (_slockObj) { _isNftSaturation = value; } }
        }

        public static bool IsSystemReady
        {
            get { return _isSystemReady; }
            set { lock (_slockObj) { _isSystemReady = value; } }
        }

        public static ETesterTestMode TestMode
        {
            get { return _testerTestMode; }
            set { lock (_slockObj) { _testerTestMode = value; } }
        }

        public static bool IsEnableEndTest
        {
            get { return _isAbleEndTest; }
            set { lock (_slockObj) { _isAbleEndTest = value; } }
        }

        public static EOutputReportState ProberAssignMode
        {
            get { return _proberAssignMode; }
            set { lock (_slockObj) { _proberAssignMode = value; } }
        }

		#endregion
	}

	public class GlobalData
	{
		private static object _slockObj = new Object();

		private static string _proberRecipeName;

        private static string _toProbeWaferNumber;

        private static string _toProbeLotNumber;

        private static string _toProbeBarcode;

        private static string _toProbeOperator;

        private static int _hwFilterWheelPos;

		private static int _continueModeCol;

		private static int _continueModeRow;

        private static string _proberAligenKeyFileName;

        private static int _proberProductionMode; // Prober生產模式

        private static double _proberTemperature;

		#region >>> Public Property <<<

		public static string ProberRecipeName
		{
			get { return _proberRecipeName; }
			set { lock (_slockObj) { _proberRecipeName = value; } }
		}

        public static string ToProbeWaferNumber
        {
            get { return _toProbeWaferNumber; }
            set { lock (_slockObj) { _toProbeWaferNumber = value; } }
        }

        public static string ToProbeLotNumber
        {
            get { return _toProbeLotNumber; }
            set { lock (_slockObj) { _toProbeLotNumber = value; } }
        }

        public static string ToProbeBarcode
        {
            get { return _toProbeBarcode; }
            set { lock (_slockObj) { _toProbeBarcode = value; } }
        }

        public static string ToProbeOperator
        {
            get { return _toProbeOperator; }
            set { lock (_slockObj) { _toProbeOperator = value; } }
        }

        public static int HwFilterWheelPos
        {
            get { return _hwFilterWheelPos; }
            set { lock (_slockObj) { _hwFilterWheelPos = value; } }
        }

		public static int ContinueModeCol
		{
			get { return _continueModeCol; }
			set { lock (_slockObj) { _continueModeCol = value; } }
		}

		public static int ContinueModeRow
		{
			get { return _continueModeRow; }
			set { lock (_slockObj) { _continueModeRow = value; } }
		}

        public static string ProberAligenKeyFileName
        {
            get { return _proberAligenKeyFileName; }
            set { lock (_slockObj) { _proberAligenKeyFileName = value; } }
	}

        public static int ProberProductionMode
	{
            get { return _proberProductionMode; }
            set { lock (_slockObj) { _proberProductionMode = value; } }
        }

        public static double ProberTemperature
        {
            get { return _proberTemperature; }
            set { lock (_slockObj) { _proberTemperature = value; } }
        }

	   #endregion  
	}

    public enum EOutputReportState : int
    {
        None = 0,

        FileNameIsEmpty = 2,

        CanOverwrite = 3,

        CanAppend = 4,

        CanNotAppend = 5,

        CanRetest = 6,

        CanAppendAndRetest = 7,

        FileNameExist = 8,
    }

    public enum ETesterTestMode : int
    {
        Normal = 1,
        Overload = 2,
    }
}
