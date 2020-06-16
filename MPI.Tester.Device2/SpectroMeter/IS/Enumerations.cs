using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SpectroMeter.IS
{
	#region >>> Error Handling <<<

	/// <summary>
	/// Error code.
	/// </summary>
	public enum EISErrorCode : int
	{
		ErrorNoError = 0,
		ErrorUnknown = -1,
		ErrorTimeoutRWSNoData = -2,
		ErrorInvalidDeviceType = -3,
		ErrorAcquisition = -4,
		ErrorAccuDataStream = -5,
		ErrorPrivilege = -6,
		ErrorFIFOOverflow = -7,
		ErrorTimeoutEOSScan = -8, //ISA only
		ErrorCCDTemperatureFail = -13,
		ErrorAdrControl = -14,
		ErrorFloat = -15, //floating point error
		ErrorTriggerTimeout = -16,
		ErrorAbortWaitTrigger = -17,
		ErrorDarkArray = -18,
		ErrorNoCalibration = -19,
		ErrorCRI = -21,
		ErrorNoMultiTrack = -25,
		ErrorInvalidTrack = -26,
		ErrorDetectPixel = -31,
		ErrorSelectParamSet = -32,
		ErrorI2CInit = -35,
		ErrorI2CBusy = -36,
		ErrorI2CNotAck = -37,
		ErrorI2CRelease = -38,
		ErrorI2CTimeOut = -39,
		ErrorI2CTransmission = -40,
		ErrorI2CController = -41,
		ErrorDataNotAck = -42,
		ErrorNoExternalADC = -52,
		ErrorShutterPos = -53,
		ErrorFilterPos = -54,
		ErrorConfigSerialMismatch = -55,
		ErrorCalibSerialMismatch = -56,
		ErrorInvalidParameter = -57,
		ErrorGetFilterPos = -58,
		ErrorParamOutOfRange = -59,
	}

	/// <summary>
	/// CAS error code.
	/// </summary>
	public enum EISCASError : int
	{
		errCASOK = ( int ) EISErrorCode.ErrorNoError,

		errCASError = -1000,
		errCasNoConfig = errCASError - 3,
		errCASDriverMissing = errCASError - 6,		// driver stuff missing, returned in INITDevice
		errCasDeviceNotFound = errCASError - 10,	// invalid ADevice param
	}

	#endregion

	#region >>> Device Handling <<<

	/// <summary>
	/// Interface type.
	/// </summary>
	public enum EInterfaceType : int
	{
		InterfaceISA = 0,
		InterfacePCI = 1,
		InterfaceTest = 3,
		InterfaceUSB = 5,
		InterfaceNVISCluster = 7
	}

	/// <summary>
	/// Assignment constants.
	/// </summary>
	public enum EAssignConstant : int
	{
		aoAssignDevice = 0,
		aoAssignParameters = 1,
		aoAssignComplete = 2,
	}

	// Initialization
	public enum EInitializationPerform : int
	{
		InitOnce = 0,
		InitForced = 1,
		InitNoHardware = 2
	}

	#endregion

	#region >>> Instrument Property <<<

	/// <summary>
	/// Device parameters.
	/// </summary>
	public enum EDeviceParameter : int
	{
		dpidIntTimeMin = 101,
		dpidIntTimeMax = 102,
		dpidDeadPixels = 103,
		dpidVisiblePixels = 104,
		dpidPixels = 105,
		dpidParamSets = 106,
		dpidCurrentParamSet = 107,
		dpidADCRange = 108,
		dpidADCBits = 109,
		dpidSerialNo = 110,
		dpidTOPSerial = 111,
		dpidTransmissionFileName = 112,
		dpidConfigFileName = 113,
		dpidCalibFileName = 114,
		dpidCalibrationUnit = 115,
		dpidAccessorySerial = 116,
		dpidTriggerCapabilities = 118,
		dpidAveragesMax = 119,
		dpidFilterType = 120,
		dpidRelSaturationMin = 123,
		dpidRelSaturationMax = 124,
		dpidInterfaceVersion = 125,
		dpidTriggerDelayTimeMax = 126,
		dpidSpectrometerName = 127,
		dpidNeedDarkCurrent = 130,
		dpidNeedDensityFilterChange = 131,
		dpidSpectrometerModel = 132,
		dpidLine1FlipFlop = 133,
		dpidTimer = 134,
		dpidInterfaceType = 135,
		dpidInterfaceOption = 136,
		dpidInitialized = 137,
		dpidDCRemeasureReasons = 138,
	}

	/// <summary>
	/// Trigger capability.
	/// </summary>
	public enum ETriggerCapability : int
	{
		tcoCanTrigger = 0x0001,
		tcoTriggerDelay = 0x0002,
		tcoTriggerOnlyWhenReady = 0x0004,
		tcoAutoRangeTriggering = 0x0008,
		tcoShowBusyState = 0x0010,
		tcoShowACQState = 0x0020,
		tcoFlashOutput = 0x0040,
		tcoFlashHardware = 0x0080,
		tcoFlashForEachAverage = 0x0100,
		tcoFlashDelay = 0x0200,
		tcoFlashDelayNegative = 0x0400,
		tcoFlashSoftware = 0x0800,
		tcoGetFlipFlopState = 0x1000,
	}

	/// <summary>
	/// DC remeasure reasons.
	/// </summary>
	public enum EDCRemeasureReasons : int
	{
		todcrrNeedDarkCurrent = 0x0001,
		todcrrCCDTemperature = 0x0002,
	}

	/// <summary>
	/// Serial constants.
	/// </summary>
	public enum ESerialConstants : int
	{
		casSerialComplete = 0,
		casSerialAccessory = 1,
		casSerialExtInfo = 2,
		casSerialDevice = 3,
		casSerialTOP = 4,
	}

	/// <summary>
	/// Option constants
	/// </summary>
	public enum EOptionConstants : int
	{
		coShutter = 0x0001,
		coFilter = 0x0002,
		coGetShutter = 0x0004,
		coGetFilter = 0x0008,
		coGetAccessories = 0x0010,
		coGetTemperature = 0x0020,
		coUseDarkcurrentArray = 0x0040,
		coUseTransmission = 0x0080,
		coAutorangeMeasurement = 0x0100,
		coAutorangeFilter = 0x0200,
		coCheckCalibConfigSerials = 0x0400,
		coTOPHasFieldOfViewConfig = 0x0800,
		coAutoRemeasureDC = 0x1000,
		coCanMultiTrack = 0x8000,
	}
	
	#endregion

	#region >>> Measurement <<<

	/// <summary>
	/// Perform action.
	/// </summary>
	public enum EPerformAction : int
	{
		paPrepareMeasurement = 1,
		paLoadCalibration = 3,
		paCheckAccessories = 4,
	}

	/// <summary>
	/// Measurement parameters.
	/// </summary>
	public enum EMeasurementParameters : int
	{
		mpidIntegrationTime = 01,
		mpidAverages = 02,
		mpidTriggerDelayTime = 03,
		mpidTriggerTimeout = 04,
		mpidCheckStart = 05,
		mpidCheckStop = 06,
		mpidColormetricStart = 07,
		mpidColormetricStop = 08,
		mpidACQTime = 10,
		mpidMaxADCValue = 11,
		mpidMaxADCPixel = 12,
		mpidTriggerSource = 14,
		mpidAmpOffset = 15,
		mpidSkipLevel = 16,
		mpidSkipLevelEnabled = 17,
		mpidScanStartTime = 18,
		mpidAutoRangeMaxIntTime = 19,
		mpidAutoRangeLevel = 20,
		mpidDensityFilter = 21,
		mpidCurrentDensityFilter = 22,
		mpidNewDensityFilter = 23,
		mpidLastDCAge = 24,
		mpidRelSaturation = 25,
		mpidRemeasureDCInterval = 28,
		mpidFlashDelayTime = 29,
		mpidTOPAperture = 30,
		mpidTOPDistance = 31,
		mpidTOPSpotSize = 32,
		mpidTriggerOptions = 33,
		mpidForceFilter = 34,
		mpidFlashType = 35,
		mpidFlashOptions = 36,
		mpidACQStateLine = 37,
		mpidACQStateLinePolarity = 38,
		mpidBusyStateLine = 39,
		mpidBusyStateLinePolarity = 40,
		mpidAutoFlowTime = 41,
		mpidCRIMode = 42,
		mpidObserver = 43,
		mpidTOPFieldOfView = 44,
		mpidCurrentCCDTemperature = 46,
		mpidLastCCDTemperature = 47,
		mpidDCCCDTemperature = 48,
	}

	/// <summary>
	/// Trigger options.
	/// </summary>
	public enum ETriggerOptions : int
	{
		toAcceptOnlyWhenReady = 1,
		toForEachAutoRangeTrial = 2,
		toShowBusyState = 4,
		toShowACQState = 8,
	}

	/// <summary>
	/// Flash type.
	/// </summary>
	public enum EFlashType : int
	{
		ftNone = 0,
		ftHardware = 1,
		ftSoftware = 2,
	}

	/// <summary>
	/// Flash options.
	/// </summary>
	public enum EFlashOptions : int
	{
		foEveryAverage = 1,
	}

	/// <summary>
	/// Trigger source.
	/// </summary>
	public enum ETriggerSource : int
	{
		trgSoftware = 0,
		trgFlipFlop = 3,
	}

	/// <summary>
	/// CRI mode.
	/// </summary>
	public enum ECRIMode : int
	{
		criDIN6169 = 0,
		criCIE13_3_95 = 1,
	}

	/// <summary>
	/// Observer.
	/// </summary>
	public enum EObserver : int
	{
		cieObserver1931 = 0,
		cieObserver1964 = 1,
	}
	
	#endregion

	#region >>> Shutter and Filter <<<

	/// <summary>
	/// Shutter constants.
	/// </summary>
	public enum EShutterConstants : int
	{
		casShutterInvalid = -1,
		casShutterOpen = 0,
		casShutterClose = 1,
	}

	#endregion

	#region >>> Measurement Result <<<

	/// <summary>
	/// Lambda constants.
	/// </summary>
	public enum ELambdaConstants : int
	{
		cLambdaWidth = 0,
		cLambdaLow = 1,
		cLambdaMiddle = 2,
		cLambdaHigh = 3,
		cLambdaOuterWidth = 4,
		cLambdaOuterLow = 5,
		cLambdaOuterMiddle = 6,
		cLambdaOuterHigh = 7,
	}

	/// <summary>
	/// Extended color values.
	/// </summary>
	public enum EExtendedColorValues : int
	{
		ecvRedPart = 1,
		ecvVisualEffect = 2,
		ecvUVA = 3,
		ecvUVB = 4,
		ecvUVC = 5,
		ecvVIS = 6,
		ecvCRICCT = 7,
		ecvCDI = 8,
		ecvDistance = 9,
		ecvCalibMin = 10,
		ecvCalibMax = 11,
		ecvScotopicInt = 12,
	}

	#endregion

	#region >>> Utilities <<<

	/// <summary>
	/// External constants.
	/// </summary>
	public enum EExternalConstatns : int
	{
		extNoError = 0,
		extExternalError = 1,
		extFilterBlink = 2,
		extShutterBlink = 4,
	}

	#endregion
}
