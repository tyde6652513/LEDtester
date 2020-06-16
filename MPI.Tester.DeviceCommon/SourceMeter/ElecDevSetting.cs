using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public class ElecDevSetting : System.ICloneable
    {
        private object _lockObj;

        private EHWConnectType _hwConnectorType;
        private bool _isGetAllData;

        //T2001L Device Setting
        private int _devCalibrationMode;
        private bool _isDevPeakFilering;
        private bool _isFastPolar;

        private double _openShortThresholdVoltage;
        private double _openShortForceValue;
        private double _openShortMsrtRange;
        private uint _waitRiseCount;
		private double _lowLimitVoltage;
		private int _openShortPolar;

        private EDAQModel _daqModel;

		private bool _isEnableRTH;
		private string _rthDeviceIP;
        private int _daqSampleRate;
        private uint _daqCalibrationBufferID;

		private ESrcTurnOffType _srcTurnOffType;
		private double _rthTdTime;

		private bool _turnOffRangeIBackToDefault;
        private ESMUTriggerMode _smuTriggerMode;
      
        private List<SourceMeterAssignmentData> _assigmntTable;

        private ESrcSensingMode _sensingMode;

		private bool _isSettingReverseCurrentRange;

        private double _reverseCurrentApplyRange;

        private EPDSensingMode _detectorMsrtDev;

        private string _detectorDevIP;

		private bool _isDetectorHwTrig;

        private bool _isPDDualChannel;

        private bool _isEnableProtectionModule;

        private bool _isEnableDeviceLog;

        private ELCRDCBiasType _lcrDCBiasType;

		private ELCRDCBiasSource _lcrDCBiasSource;

		private string _lcrDCBiasSourceIP;

		private ESourceMeterModel _srcMeterModel;

        private IOConfigData _ioSetting;

        private EDmmModel _voltMsrtDev;
        private string _voltMsrtDevIP;

        private ESourceMeterModel _revSrcDevModel;
        private string _revSrcDevIp;

		private bool _isEnableBriefScript;

        public ElecDevSetting()
        {
            this._lockObj = new object();

            this._isGetAllData = false;
            this._devCalibrationMode = 1;
            this._isDevPeakFilering = false;
            
            this._isFastPolar = false;

            this._openShortThresholdVoltage = 5;
            this._openShortForceValue = 0.0001;
            this._openShortMsrtRange = 8;
            this._waitRiseCount = 0;
			this._lowLimitVoltage = 0.5;

			// Positive:0 , Negative:1
			this._openShortPolar = 0;

            this._daqModel = EDAQModel.NONE;

			this._isEnableRTH = false;

			this._rthDeviceIP = "192.168.50.99";

			this._daqSampleRate = 100000;

            this._daqCalibrationBufferID = 0;

			this._srcTurnOffType = ESrcTurnOffType.EachTestItem;

			this._rthTdTime = 1.6;
			
			this._turnOffRangeIBackToDefault = false;

            this._smuTriggerMode = ESMUTriggerMode.Single;

            this._assigmntTable = new List<SourceMeterAssignmentData>();

            this._sensingMode = ESrcSensingMode._4wire;

			this._detectorDevIP = "192.168.50.101";

            this._detectorMsrtDev = EPDSensingMode.NONE;

			this._isSettingReverseCurrentRange = false; 

            this._reverseCurrentApplyRange = 0.1; //unit=mA

			this._isDetectorHwTrig = false;

            this._isPDDualChannel = false;

            this._isEnableProtectionModule = false;

            this._isEnableDeviceLog = false;

            this._lcrDCBiasType = ELCRDCBiasType.Internal;

			this._lcrDCBiasSource = ELCRDCBiasSource.K2600;

			this._lcrDCBiasSourceIP = "192.168.50.111";

			this._srcMeterModel = ESourceMeterModel.NONE;

            this._ioSetting = new IOConfigData();

			this._voltMsrtDev = EDmmModel.NONE;

            this._voltMsrtDevIP = "192.168.50.100";

            this._revSrcDevModel = ESourceMeterModel.NONE;

            this._revSrcDevIp = "192.168.50.99";

			this._isEnableBriefScript = false;

            PDMonitorSMU = new SourceMeterAssignmentData();

            PDMonitorSMU.SerialNumber = "" ;
        }

        #region >>> Public Proberty <<<

		public EHWConnectType HWConnectorType
        {
			get { return this._hwConnectorType; }
			set { lock (this._lockObj) { this._hwConnectorType = value; } }
        }

        public bool IsGetAllData
        {
            get { return this._isGetAllData; }
            set { lock (this._lockObj) { this._isGetAllData = value; } }
        }

        public int DeviceCalibrationMode
        {
            get { return this._devCalibrationMode; }
            set { lock (this._lockObj) { this._devCalibrationMode = value; } }
        }

        public bool IsDevicePeakFiltering
        {
            get { return this._isDevPeakFilering; }
            set { lock (this._lockObj) { this._isDevPeakFilering = value; } }
        }

        public bool IsFastPolar
        {
            get { return this._isFastPolar; }
            set { lock (this._lockObj) { this._isFastPolar = value; } }
        }

        public double OpenShortThresholdVoltage
        {
            get { return this._openShortThresholdVoltage; }
            set { lock (this._lockObj) { this._openShortThresholdVoltage = value; } }
        }

        public double OpenShortForceValue
        {
            get { return this._openShortForceValue; }
            set { lock (this._lockObj) { this._openShortForceValue = value; } }
        }

        public double OpenShortMsrtRange
        {
            get { return this._openShortMsrtRange; }
            set { lock (this._lockObj) { this._openShortMsrtRange = value; } }
        }

        public uint WaitRiseCount
        {
            get { return this._waitRiseCount; }
            set { lock (this._lockObj) { this._waitRiseCount = value; } }
        }

		public double LowLimitVoltage
        {
			get { return this._lowLimitVoltage; }
			set { lock (this._lockObj) { this._lowLimitVoltage = value; } }
        }

		public int OpenShortPolar
		{
			get { return this._openShortPolar; }
			set { lock (this._lockObj) { this._openShortPolar = value; } }
		}

        public EDAQModel DAQModel
        {
            get { return this._daqModel; }
            set { lock (this._lockObj) { this._daqModel = value; } }
        }

		public bool IsEnableRTH
		{
			get { return this._isEnableRTH; }
			set { lock (this._lockObj) { this._isEnableRTH = value; } }
		}

		public string RTHDeviceIP
		{
			get { return this._rthDeviceIP; }
			set { lock (this._lockObj) { this._rthDeviceIP = value; } }
		}

        public int DAQSampleRate
        {
			get { return this._daqSampleRate; }
			set { lock (this._lockObj) { this._daqSampleRate = value; } }
        }

        public uint DAQCalibrationBufferID
        {
            get { return this._daqCalibrationBufferID; }
            set { lock (this._lockObj) { this._daqCalibrationBufferID = value; } }
        }

		public ESrcTurnOffType SrcTurnOffType
		{
			get { return this._srcTurnOffType; }
			set { lock (this._lockObj) { this._srcTurnOffType = value; } }
		}

		public double RTHTdTime
        {
			get { return this._rthTdTime; }
			set { lock (this._lockObj) { this._rthTdTime = value; } }
        }

        public ESMUTriggerMode SrcTriggerMode
        {
            get { return this._smuTriggerMode; }
            set { lock (this._lockObj) { this._smuTriggerMode = value; } }
        }

        public List<SourceMeterAssignmentData> Assignment
        {
            get { return this._assigmntTable; }
            set { lock (this._lockObj) { this._assigmntTable = value; } }
        }

        public bool TurnOffRangeIBackToDefault
		{
            get { return this._turnOffRangeIBackToDefault; }
            set { lock (this._lockObj) { this._turnOffRangeIBackToDefault = value; } }
        }

        public ESrcSensingMode SrcSensingMode
        {
            get { return this._sensingMode; }
            set { lock (this._lockObj) { this._sensingMode = value; } }
        }

		public bool IsSettingReverseCurrentRange
        {
            get { return this._isSettingReverseCurrentRange; }
            set { lock (this._lockObj) { this._isSettingReverseCurrentRange = value; } }
        }

        public double ReverseCurrentApplyRange
        {
            get { return this._reverseCurrentApplyRange; }
            set { lock (this._lockObj) { this._reverseCurrentApplyRange = value; } }
        }

        //public EPDSensingMode LopDeviceAssignment
        //{
        //    get { return this._lopDeviceAssignment; }
        //    set { lock (this._lockObj) { this._lopDeviceAssignment = value; } }
        //}

        //public string DetectorIP
        //{
        //    get { return this._detectorIP; }
        //    set { lock (this._lockObj) { this._detectorIP = value; } }
        //}


        public EPDSensingMode DetectorMsrtDevice
        {
            get { return this._detectorMsrtDev; }
            set { lock (this._lockObj) { this._detectorMsrtDev = value; } }
        }

        public string DetectorDeviceIP
        {
            get { return this._detectorDevIP; }
            set { lock (this._lockObj) { this._detectorDevIP = value; } }
        }

		public bool IsDetectorHwTrig
        {
            get { return this._isDetectorHwTrig; }
            set { lock (this._lockObj) { this._isDetectorHwTrig = value; } }
        }

        public bool IsPDDualChannel
        {
            get { return this._isPDDualChannel; }
            set { lock (this._lockObj) { this._isPDDualChannel = value; } }
        }

        public bool IsEnableProtectionModule
        {
            get { return this._isEnableProtectionModule; }
            set { lock (this._lockObj) { this._isEnableProtectionModule = value; } }
        }

        public bool IsEnableDeviceLog
        {
            get { return this._isEnableDeviceLog; }
            set { lock (this._lockObj) { this._isEnableDeviceLog = value; } }
        }

        public ELCRDCBiasType LCRDCBiasType
        {
            get { return this._lcrDCBiasType; }
            set { lock (this._lockObj) { this._lcrDCBiasType = value; } }
        }

		public ELCRDCBiasSource LCRDCBiasSource
		{
			get { return this._lcrDCBiasSource; }
			set { lock (this._lockObj) { this._lcrDCBiasSource = value; } }
		}

		public string LCRDCBiasSourceIP
		{
			get { return this._lcrDCBiasSourceIP; }
			set { lock (this._lockObj) { this._lcrDCBiasSourceIP = value; } }
		}

		public ESourceMeterModel SourceMeterModel
		{
			get { return this._srcMeterModel; }
			set { lock (this._lockObj) { this._srcMeterModel = value; } }
		}

        public IOConfigData IOSetting
        {
            get { return this._ioSetting; }
            set { lock (this._lockObj) { this._ioSetting = value; } }
        }
        public EDmmModel VoltMsrtDevice
        {
            get { return this._voltMsrtDev; }
            set { lock (this._lockObj) { this._voltMsrtDev = value; } }
        }

        public string VoltMsrtDeviceIP
        {
            get { return this._voltMsrtDevIP; }
            set { lock (this._lockObj) { this._voltMsrtDevIP = value; } }
        }

        public ESourceMeterModel ReverseSrcDevModel
        {
            get { return this._revSrcDevModel; }
            set { lock (this._lockObj) { this._revSrcDevModel = value; } }
        }

        public string ReverseSrcDevIP
        {
            get { return this._revSrcDevIp; }
            set { lock (this._lockObj) { this._revSrcDevIp = value; } }
        }

		public bool IsEnableBriefScript
        {
            get { return this._isEnableBriefScript; }
            set { lock (this._lockObj) { this._isEnableBriefScript = value; } }
        }

        public SourceMeterAssignmentData PDMonitorSMU { set; get; }
        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            ElecDevSetting cloneObj = this.MemberwiseClone() as ElecDevSetting;

            cloneObj._assigmntTable = new List<SourceMeterAssignmentData>();

            foreach (SourceMeterAssignmentData data in this._assigmntTable)
            {
                cloneObj._assigmntTable.Add(data.Clone() as SourceMeterAssignmentData);
            }

            return cloneObj;
        }

        #endregion

    }

    public class SourceMeterAssignmentData : System.ICloneable
    {
        private object _lockObj;

        private uint _srcChannel;
        private string _model;
        private string _connPort;
        private string _smu;
        private string _sn;

        private uint _relateSwitchSlot;

        public SourceMeterAssignmentData()
        {
            this._lockObj = new object();
        }

        #region >>> Public Proberty <<<

        public uint Channel
        {
            get { return this._srcChannel; }
            set { lock (this._lockObj) { this._srcChannel = value; } }
        }

        public string Model
        {
            get { return this._model; }
            set { lock (this._lockObj) { this._model = value; } }
        }

        public string ConnectionPort
        {
            get { return this._connPort; }
            set { lock (this._lockObj) { this._connPort = value; } }
        }

        public string SMU
        {
            get { return this._smu; }
            set { lock (this._lockObj) { this._smu = value; } }
        }

        public string SerialNumber
        {
            get { return this._sn; }
            set { lock (this._lockObj) { this._sn = value; } }
        }

        public uint RelateSwitchSlot
        {
            get { return this._relateSwitchSlot; }
            set { lock (this._lockObj) { this._relateSwitchSlot = value; } }
        }

        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }


    public class SourceMeterDioCtrl
    {
        private uint _srcChannel;
        private ushort _ioData;

        public SourceMeterDioCtrl(uint srcCh, ushort ioData)
        {
            this._srcChannel = srcCh;

            this._ioData = ioData;
        }

        public uint SrcChannel
        {
            get { return this._srcChannel; }
            set { this._srcChannel = value; }
        } 

        public ushort DioData
        {
            get { return this._ioData; }
            set { this._ioData = value; }
        }
    }
}
