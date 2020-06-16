using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using MPI.Tester.DeviceCommon;

using MPI.Tester.Data.LaserData.LaserSource;

namespace MPI.Tester.Data
{
	public class MachineConfig : ICloneable
    {
        private object _lockObj;

        private string _testerModel;

		private string _testerSN;

		private ESpectrometerModel _sptMeterModel;

		private ESourceMeterModel _srcMeterModel;

		private EESDModel _esdModel;

		private FuncEnable _funcEnable;

		private string _sourceMeterSN;
        private string _spectrometerSN;
        private string _sphereSN;
		private string _lcrMeterSN;

		private ETesterCommMode _testerCommMode;
        private EEqModel _eqModle;
        private EActiveState _activeState;
		private string _ipAddr01;
		private int _netPort01;
		private string _ipAddr02;
		private int _netPort02;

		private EWheelMsrtSource _wheelMsrtSource;

        //For T2001L, 啟用快速功能會無法得知當前待測極性
        private bool _isFastPolar;

		private ESwitchSystemModel _switchSystemModel;

        private string _switchSystemSN;

        private SpectrometerHWSetting _spetometerHwSetting;

        private ESpectrometerInterfaceType _sptInterfaceType;

        private ESpectrometerCalibDataMode _sptCalibMode;

		private EDAQModel _daqModel;

		private ELCRModel _lcrModel;

		private EPDSensingMode _pdSensingMode;
        private ELCRDCBiasType _lcrDCBiasType;
        private ELCRDCBiasSource _lcrDCBiasSource;
        private int _daqSampleRate;
        private uint _daqCalibrationBufferID;

		private ESrcTurnOffType _srcTurnOffType;

        private ETesterFunctionType _testerFuncType;

        private ChannelConfig _channelConfig;

        private ESrcSensingMode _srcSensingMode;

        private string _pdDetectorSN;

		private string _lcrDCBiasSourceSN;

        private bool _isPDDetectorHwTrig;

        private EOSAModel _osaModel;

        private string _osaSN;

        private IOConfigData _ioSetting;

        private LaserSrcSysConfig _laserSrcSysConfig;

		private EDmmModel _dmmModel;
        private string _dmmSN;

        private ESourceMeterModel _srcMeterModel2;
        private string _sourceMeterSN2;
        /// <summary>
        /// Construnctor
        /// </summary>
        public MachineConfig()
        {
            this._lockObj = new object();

            this._testerModel = "T180";
			this._testerSN = "T180SNxxxx01";
			this._sptMeterModel = ESpectrometerModel.NONE;
			this._srcMeterModel = ESourceMeterModel.NONE;
			this._esdModel = EESDModel.NONE;
            this._switchSystemModel = ESwitchSystemModel.NONE;

			this._funcEnable = new FuncEnable();

			this._sourceMeterSN = "SourceMeterSN";

            this._switchSystemSN = "SwitchSystemSN";

            this._spectrometerSN = "RS-OP123456";
            this._sphereSN = "0000000000";

			this._lcrMeterSN = "LCRMeterSN";

			this._testerCommMode = ETesterCommMode.BySoftware;
			this._ipAddr01 = "127.0.0.1";
			this._netPort01 = 998;
			this._ipAddr02 = "127.0.0.2";
			this._netPort02 = 998;

            this._eqModle = EEqModel.Prober;
            this._activeState = EActiveState.ActiveHigh;

			this._wheelMsrtSource = EWheelMsrtSource.SourceMeterIO;

            this._isFastPolar = false;

            this._spetometerHwSetting = new SpectrometerHWSetting();
         
            this._daqModel = EDAQModel.NONE;

			this._lcrModel = ELCRModel.NONE;

			this._daqSampleRate = 100000;

            this._daqCalibrationBufferID = 0;

			this._pdSensingMode = EPDSensingMode.NONE;

			this._srcTurnOffType = ESrcTurnOffType.TestEnd;

            this._channelConfig = new ChannelConfig();

            this._srcSensingMode = ESrcSensingMode._4wire;

            this._pdDetectorSN = "PD Detector SN";

			this._lcrDCBiasSourceSN = "LCR DC Bias Source SN";

            this._isPDDetectorHwTrig = false;

            this._osaModel = EOSAModel.NONE;

            this._osaSN = "OSA SN";

            this._ioSetting = new IOConfigData();

            this._laserSrcSysConfig = new LaserSrcSysConfig();


			this._dmmModel = EDmmModel.NONE;

            this._dmmSN = "DMM SN";

            this._srcMeterModel2 = ESourceMeterModel.NONE;
            this._sourceMeterSN = "SourceMeterSN2";
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

		public ESpectrometerModel SpectrometerModel
		{
			get { return this._sptMeterModel; }
			set { lock (this._lockObj) { this._sptMeterModel = value; } }
		}

		public ESourceMeterModel SourceMeterModel
		{
			get { return this._srcMeterModel; }
			set { lock (this._lockObj) { this._srcMeterModel = value; } }
		}

		public EESDModel ESDModel
		{
			get { return this._esdModel; }
			set { lock (this._lockObj) { this._esdModel = value; } }
		}

		public FuncEnable Enable
		{
			get { return this._funcEnable; }
			set { lock (this._lockObj) { this._funcEnable = value; } }
		}

		public string SourceMeterSN
		{
			get { return this._sourceMeterSN; }
			set { lock (this._lockObj) { this._sourceMeterSN = value; } }
		}

        public string SpectrometerSN
        {
            get { return this._spectrometerSN; }
            set { lock (this._lockObj) { this._spectrometerSN = value; } }
        }

		public string LCRMeterSN
		{
			get { return this._lcrMeterSN; }
			set { lock (this._lockObj) { this._lcrMeterSN = value; } }
		}

        public string SphereSN
        {
            get { return this._sphereSN; }
            set { lock (this._lockObj) { this._sphereSN = value; } }
        }
		
		public ETesterCommMode TesterCommMode
		{
			get { return this._testerCommMode; }
			set { lock (this._lockObj) { this._testerCommMode = value; } }
		}

		public string IPAddr01
		{
			get { return this._ipAddr01; }
			set { lock (this._lockObj) { this._ipAddr01 = value; } }
		}

		public int NetPort01
		{
			get { return this._netPort01; }
			set { lock (this._lockObj) { this._netPort01 = value; } }
		}

		public string IPAddr02
		{
			get { return this._ipAddr02; }
			set { lock (this._lockObj) { this._ipAddr02 = value; } }
		}
		
		public int NetPort02
		{
			get { return this._netPort02; }
			set { lock (this._lockObj) { this._netPort02 = value; } }
		}

		public EWheelMsrtSource WheelMsrtSource
		{
			get { return this._wheelMsrtSource; }
			set { lock (this._lockObj) { this._wheelMsrtSource = value; } }
		}

        public bool IsFastPolar
        {
            get { return this._isFastPolar; }
            set { lock (this._lockObj) { this._isFastPolar = value; } }
        }

        public ESwitchSystemModel SwitchSystemModel
		{
			get { return this._switchSystemModel; }
			set { lock (this._lockObj) { this._switchSystemModel = value; } }
		}

        public string SwitchSystemSN
        {
            get { return this._switchSystemSN; }
            set { lock (this._lockObj) { this._switchSystemSN = value; } }
        }

        public SpectrometerHWSetting spetometerHWSetting
        {
            get { return this._spetometerHwSetting; }
            set { lock (this._lockObj) { this._spetometerHwSetting = value; } }
        }

        public EDAQModel DAQModel
        {
            get { return this._daqModel; }
            set { lock (this._lockObj) { this._daqModel = value; } }
        }

		public ELCRModel LCRModel
		{
			get { return this._lcrModel; }
			set { lock (this._lockObj) { this._lcrModel = value; } }
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

        public EEqModel EQModle
        {
            get { return this._eqModle; }
            set { lock (this._lockObj) { this._eqModle = value; } }
        }

        public EActiveState ActiveState
        {
            get { return this._activeState; }
            set { lock (this._lockObj) { this._activeState = value; } }
        }

		public ESrcTurnOffType SrcTurnOffType
		{
			get { return this._srcTurnOffType; }
			set { lock (this._lockObj) { this._srcTurnOffType = value; } }
		}

        public ETesterFunctionType TesterFunctionType
        {
            get { return this._testerFuncType; }
            set { lock (this._lockObj) { this._testerFuncType = value; } }
        }

        public ChannelConfig ChannelConfig
        {
            get { return this._channelConfig; }
            set { lock (this._lockObj) { this._channelConfig = value; } }
        }

		public EPDSensingMode PDSensingMode
		{
            get { return this._pdSensingMode; }
            set { lock (this._lockObj) { this._pdSensingMode = value; } }
        }

		public ELCRDCBiasSource LCRDCBiasSource
		{
			get { return this._lcrDCBiasSource; }
			set { lock (this._lockObj) { this._lcrDCBiasSource = value; } }
		}

        public ELCRDCBiasType LCRDCBiasType
        {
            get { return this._lcrDCBiasType; }
            set { lock (this._lockObj) { this._lcrDCBiasType = value; } }
        }

        public ESrcSensingMode SrcSensingMode
        {
            get { return this._srcSensingMode; }
            set { lock (this._lockObj) { this._srcSensingMode = value; } }
        }

        public string PDDetectorSN
        {
            get { return this._pdDetectorSN; }
            set { lock (this._lockObj) { this._pdDetectorSN = value; } }
        }

		public string LCRDCBiasSourceSN
		{
			get { return this._lcrDCBiasSourceSN; }
			set { lock (this._lockObj) { this._lcrDCBiasSourceSN = value; } }
		}

        public bool IsPDDetectorHwTrig
        {
            get { return this._isPDDetectorHwTrig; }
            set { lock (this._lockObj) { this._isPDDetectorHwTrig = value; } }
        }

        public EOSAModel OSAModel
        {
            get { return this._osaModel; }
            set { lock (this._lockObj) { this._osaModel = value; } }
        }

        public string OSASN
        {
            get { return this._osaSN; }
            set { lock (this._lockObj) { this._osaSN = value; } }
        }
        public IOConfigData IOConfig
        {
            get { return this._ioSetting; }
            set { lock (this._lockObj) { this._ioSetting = value; } }
        }


        public LaserSrcSysConfig LaserSrcSysConfig
        {

            set{
                _laserSrcSysConfig = value;
            }
            get{
                return _laserSrcSysConfig;
            }

        }
        public EDmmModel DmmModel
        {
            get { return this._dmmModel; }
            set { lock (this._lockObj) { this._dmmModel = value; } }
        }

        public string DmmSN
        {
            get { return this._dmmSN; }
            set { lock (this._lockObj) { this._dmmSN = value; } }
        }

        public ESourceMeterModel SourceMeterModel2
        {
            get { return this._srcMeterModel2; }
            set { lock (this._lockObj) { this._srcMeterModel2 = value; } }
        }

        public string SourceMeterSN2
        {
            get { return this._sourceMeterSN2; }
            set { lock (this._lockObj) { this._sourceMeterSN2 = value; } }
        }

        #endregion

		# region >>> Public Method <<<

		public object Clone()
		{
			MachineConfig obj = this.MemberwiseClone() as MachineConfig;

			obj._lockObj = new object();
			return (object)obj;
		}

		#endregion

    }
}
