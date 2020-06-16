using System;
using System.Collections.Generic;
using System.Text;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    public class MachineInfoData
    {
        private object _lockObj;

        private string _testerModel;
        private string _testerSN;

        private bool _isSrcInitSuccess;
        private SourceMeterSpec _srcSpec;
        private string _srcMeterSN;
		private string _srcMeterHWVersion;
		private string _srcMeterSWVersion;

        private string _srcMeterSN2;
        private string _srcMeterHWVersion2;
        private string _srcMeterSWVersion2;

        private bool _isSptInitSuccess;
        private SpectrometerSpec _sptSpec;
        private string _sptMeterSN;
        private string _sphereSN;

        private bool _isEsdInitSuccess;
        private ESDSpec _esdSpec;
        private string _esdSN;

        private bool _isLcrInitSuccess;
		private LCRMeterSpec _lcrSpec;
        private string _lcrMeterSN;
        private string _lcrMeterHWVersion;
        private string _lcrMeterSWVersion;

		private SourceMeterSpec _dcBiasSpec;

        private bool _isSwitchInitSuccess;
		private string _switchSystemSN;

        private string[] _EPPROMConfigData;

        private bool _isOsaInitSuccess;
        private string _osaSN;

        private long _esdHbmRelayCount;
        private long _esdMmRelayCount;

        private long _esdHbmRelayCount2;
        private long _esdMmRelayCount2;

        private long _esdHbmRelayCount3;
        private long _esdMmRelayCount3;

        private long _esdHbmRelayCount4;
        private long _esdMmRelayCount4;

        private int _maxSwitchChannelCount;

        private AttenuatorSpec _attSpec;

        private Dictionary<int,LaserSourceSpec> _chLaserSysSpecDic;

        public MachineInfoData()
        {
            this._lockObj = new object();

            this._testerModel = "NONE";
            this._sphereSN = "NONE";
            this._srcMeterSN = "NONE";
            this._sptMeterSN = "NONE";
            this._sphereSN = "NONE";
            this._esdSN = "NONE";
            this._lcrMeterSN = "NONE";
            this._lcrMeterHWVersion= "NONE";
            this._lcrMeterSWVersion= "NONE";
			this._switchSystemSN = "NONE";

            this._osaSN = "NONE";
          //  this._EPPROMConfigData = new string[16];
            this._esdHbmRelayCount = 0;
            this._esdMmRelayCount = 0;

            this._esdHbmRelayCount2 = 0;
            this._esdMmRelayCount2 = 0;

            this._esdHbmRelayCount3 = 0;
            this._esdMmRelayCount3 = 0;

            this._esdHbmRelayCount4 = 0;
            this._esdMmRelayCount4 = 0;

            this._maxSwitchChannelCount = 0;

            this._isEsdInitSuccess = false;
            this._isLcrInitSuccess = false;
            this._isOsaInitSuccess = false;
            this._isSptInitSuccess = false;
            this._isSrcInitSuccess = false;
            this._isSwitchInitSuccess = false;
            _chLaserSysSpecDic = new Dictionary<int, LaserSourceSpec>();
            SNDeviceRelayDic = new Dictionary<string, object>();
        }

        #region >>> Public Property <<<

        public string TesterModel
        {
            get { return this._testerModel; }
            set { lock (this._lockObj) { this._testerModel = value; } }
        }

        public string TesterSN
        {
            get { return this._testerSN; }
            set { lock (this._lockObj) { this._testerSN = value; } }
        }

        public string SourceMeterSN
        {
            get { return this._srcMeterSN; }
            set { lock (this._lockObj) { this._srcMeterSN = value; } }
        }

		public string SourceMeterHWVersion
		{
			get { return this._srcMeterHWVersion; }
			set { lock (this._lockObj) { this._srcMeterHWVersion = value; } }
		}

		public string SourceMeterSWVersion
		{
			get { return this._srcMeterSWVersion; }
			set { lock (this._lockObj) { this._srcMeterSWVersion = value; } }
		}

        public string SourceMeterSN2
        {
            get { return this._srcMeterSN2; }
            set { lock (this._lockObj) { this._srcMeterSN2 = value; } }
        }

        public string SourceMeterHWVersion2
        {
            get { return this._srcMeterHWVersion2; }
            set { lock (this._lockObj) { this._srcMeterHWVersion2 = value; } }
        }

        public string SourceMeterSWVersion2
        {
            get { return this._srcMeterSWVersion2; }
            set { lock (this._lockObj) { this._srcMeterSWVersion2 = value; } }
        }

        public string SpectrometerSN
        {
            get { return this._sptMeterSN; }
            set { lock (this._lockObj) { this._sptMeterSN = value; } }
        }

        public string SphereSN
        {
            get { return this._sphereSN; }
            set { lock (this._lockObj) { this._sphereSN = value; } }
        }

        public string EsdSN
        {
            get { return this._esdSN; }
            set { lock (this._lockObj) { this._esdSN = value; } }
        }

        public string LCRMeterSN
        {
            get { return this._lcrMeterSN; }
            set { lock (this._lockObj) { this._lcrMeterSN = value; } }
        }
        public string LCRMeterHWVersion
        {
            get { return this._lcrMeterHWVersion; }
            set { lock (this._lockObj) { this._lcrMeterHWVersion = value; } }
        }
        public string LCRMeterSWVersion
        {
            get { return this._lcrMeterSWVersion; }
            set { lock (this._lockObj) { this._lcrMeterSWVersion = value; } }
        }

		public string SwitchSystemSN
		{
			get { return this._switchSystemSN; }
			set { lock (this._lockObj) { this._switchSystemSN = value; } }
		}

        public string[] EPPROMConfigData
        {
            get { return this._EPPROMConfigData; }
            set { lock (this._lockObj) { this._EPPROMConfigData = value; } }
        }

        public long EsdHbmRelayCount
        {
            get { return this._esdHbmRelayCount; }
            set { lock (this._lockObj) { this._esdHbmRelayCount = value; } }
        }

        public long EsdMmRelayCount
        {
            get { return this._esdMmRelayCount; }
            set { lock (this._lockObj) { this._esdMmRelayCount = value; } }
        }

        public long EsdHbmRelayCount2
        {
            get { return this._esdHbmRelayCount2; }
            set { lock (this._lockObj) { this._esdHbmRelayCount2 = value; } }
        }

        public long EsdMmRelayCount2
        {
            get { return this._esdMmRelayCount2; }
            set { lock (this._lockObj) { this._esdMmRelayCount2 = value; } }
        }

        public long EsdHbmRelayCount3
        {
            get { return this._esdHbmRelayCount3; }
            set { lock (this._lockObj) { this._esdHbmRelayCount3 = value; } }
        }

        public long EsdMmRelayCount3
        {
            get { return this._esdMmRelayCount3; }
            set { lock (this._lockObj) { this._esdMmRelayCount3 = value; } }
        }

        public long EsdHbmRelayCount4
        {
            get { return this._esdHbmRelayCount4; }
            set { lock (this._lockObj) { this._esdHbmRelayCount4 = value; } }
        }

        public long EsdMmRelayCount4
        {
            get { return this._esdMmRelayCount4; }
            set { lock (this._lockObj) { this._esdMmRelayCount4 = value; } }
        }

        public SourceMeterSpec SourceMeterSpec
        {
            get { return this._srcSpec; }
            set { lock (this._lockObj) { this._srcSpec = value; } }
        }

        public SpectrometerSpec SpectrometerSpec
        {
            get { return this._sptSpec; }
            set { lock (this._lockObj) { this._sptSpec = value; } }
        }

        public ESDSpec ESDSpec
        {
            get { return this._esdSpec; }
            set { lock (this._lockObj) { this._esdSpec = value; } }
        }

		public LCRMeterSpec LCRMeterSpec
		{
			get { return this._lcrSpec; }
			set { lock (this._lockObj) { this._lcrSpec = value; } }
		}

		public SourceMeterSpec DCBiasSpec
		{
			get { return this._dcBiasSpec; }
			set { lock (this._lockObj) { this._dcBiasSpec = value; } }
		}

        public AttenuatorSpec AttSpec
        {
            get { return this._attSpec; }
            set { lock (this._lockObj) { this._attSpec = value; } }
        }

        public Dictionary<int,LaserSourceSpec> ChLaserSysSpecDic
        {
            get { return this._chLaserSysSpecDic; }
            set { lock (this._lockObj) { this._chLaserSysSpecDic = value; } }
            
        }

        public string OsaSN
        {
            get { return this._osaSN; }
            set { lock (this._lockObj) { this._osaSN = value; } }
        }

        public int MaxSwitchingChannelCount
        {
            get { return this._maxSwitchChannelCount; }
            set { lock (this._lockObj) { this._maxSwitchChannelCount = value; } }
        }

        public bool IsSrcInitSuccess
        {
            get { return this._isSrcInitSuccess; }
            set { lock (this._lockObj) { this._isSrcInitSuccess = value; } }
        }

        public bool IsSptInitSuccess
        {
            get { return this._isSptInitSuccess; }
            set { lock (this._lockObj) { this._isSptInitSuccess = value; } }
        }

        public bool IsOsaInitSuccess
        {
            get { return this._isOsaInitSuccess; }
            set { lock (this._lockObj) { this._isOsaInitSuccess = value; } }
        }

        public bool IsLcrInitSuccess
        {
            get { return this._isLcrInitSuccess; }
            set { lock (this._lockObj) { this._isLcrInitSuccess = value; } }
        }

        public bool IsEsdInitSuccess
        {
            get { return this._isEsdInitSuccess; }
            set { lock (this._lockObj) { this._isEsdInitSuccess = value; } }
        }

        public bool IsSwitchInitSuccess
        {
            get { return this._isSwitchInitSuccess; }
            set { lock (this._lockObj) { this._isSwitchInitSuccess = value; } }
        }
        public Dictionary<string, object> SNDeviceRelayDic { get; set; }

        #endregion
    }
    
}
