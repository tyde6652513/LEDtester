using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class TransistorTestItem : TestItemData
	{
		#region >>> private Property <<<

		private List<OptiSettingData> _optiSettingList;
		private List<ElectSettingData> _elecSettingList;

        private EMsrtType _msrtType;
        private double _processDelayTime;

        private bool _isPulseMode;

		private double _forceTime;
		private double _turnOffTime;
        private string _forceTimeUnit;

		private double _msrtNPLC;
        
        private uint _sweepPoints;
        private ElecTerminalSetting[] _terminalSettingDescription;

		private bool _isTestOptical;
        private ESensingMode _sensingMode;
        private double _fixIntegralTime;
        private double _limitIntegralTime;
        private double _trigDelayTime;

		private bool _isEnableDetector;
        private bool _isEnableDetector2;

        private bool _isFixedDetectorMsrtRange;
		private double _detectorMsrtRange;
        private List<double> _detectorMsrtRangeList;
		private double _detectorMsrtNplc;
		private double _detectorBiasVolt;

        private bool _isFixedDetectorMsrtRange2;
        private double _detectorMsrtRange2;
        private List<double> _detectorMsrtRangeList2;
        private double _detectorMsrtNplc2;
        private double _detectorBiasVolt2;

		[NonSerialized]
		private PerformanceTimer _fixedSITTimer;
		[NonSerialized]
		private PerformanceTimer _processTimer;

		#endregion

        public TransistorTestItem(): base()
		{
			this._lockObj = new object();

			this._type = ETestType.TRANSISTOR;

			this._processTimer = new PerformanceTimer();
			this._fixedSITTimer = new PerformanceTimer();

			/////////////////////////////////////////////////
			// Electrical Setting 
			/////////////////////////////////////////////////
			this._elecSettingList = new List<ElectSettingData>();

			this._sweepPoints = 2;

			this._forceTime = 1.0d;
			this._turnOffTime = 0.0d;

            this._forceTimeUnit = "ms";

			this._msrtNPLC = 0.01d;

			this._processDelayTime = 0.0d;

			this._msrtType = EMsrtType.TRANSISTOR;

            this._terminalSettingDescription = new ElecTerminalSetting[4] { new ElecTerminalSetting(ETerminalName.Drain, ESMU.SMU1), 
															                new ElecTerminalSetting(ETerminalName.Source, ESMU.SMU2),
															                new ElecTerminalSetting(ETerminalName.Gate, ESMU.SMU3),
															                new ElecTerminalSetting(ETerminalName.Bluk, ESMU.SMU4) };

			/////////////////////////////////////////////////
			// optical setting
			/////////////////////////////////////////////////
			this._optiSettingList = new List<OptiSettingData>();
			this._sensingMode = ESensingMode.Fixed;
			this._fixIntegralTime = 10.0d;
			this._limitIntegralTime = 50.0d;
			this._trigDelayTime = 0.0d;

			this._isTestOptical = true;

			this._isEnableDetector = false;
            this._isFixedDetectorMsrtRange = false;
            this._detectorMsrtRange = 0.0d;
            this._detectorMsrtRangeList = new List<double>();
			this._detectorMsrtNplc = 0.01d;
			this._detectorBiasVolt = 0;

            this._isEnableDetector2 = false;
            this._isFixedDetectorMsrtRange2 = false;
            this._detectorMsrtRange2 = 0.0d;
            this._detectorMsrtRangeList2 = new List<double>();
            this._detectorMsrtNplc2 = 0.01d;
            this._detectorBiasVolt2 = 0;

			/////////////////////////////////////////////////
			// Tested Result Setting
			/////////////////////////////////////////////////
			this.CreateGainAndMsrtItem();

			this.ResetKeyName();
		}

		#region >>> Public Property <<<

		public List<OptiSettingData> OptiSettingList
		{
			get { return this._optiSettingList; }
			set { lock (this._lockObj) { this._optiSettingList = value; } }
		}

		public EMsrtType TRMsrtType
		{
			get { return this._msrtType; }
			set { lock (this._lockObj) { this._msrtType = value; } }
		}

        public int DataLength
        {
            get { return (int)(this._sweepPoints); }
        } 

		public uint TRSweepPoints
		{
            get { return this._sweepPoints; }
			set { lock (this._lockObj){ this._sweepPoints = value; } }
		}

		public double TRForceTime
		{
			get { return this._forceTime; }
			set { lock (this._lockObj) { this._forceTime = value; } }
		}

		public double TRTurnOffTime
		{
			get { return this._turnOffTime; }
			set { lock (this._lockObj) { this._turnOffTime = value; } }
		}

        public string TRForceTimeUnit
        {
            get { return this._forceTimeUnit; }
            set { lock (this._lockObj) { this._forceTimeUnit = value; } }
        }

		public ESensingMode TRSensingMode
		{
			get { return this._sensingMode; }
			set { lock (this._lockObj) { this._sensingMode = value; } }
		}

		public double TRFixIntegralTime
		{
			get { return this._fixIntegralTime; }
			set { lock (this._lockObj) { this._fixIntegralTime = value; } }
		}

		public double TRLimitIntegralTime
		{
			get { return this._limitIntegralTime; }
			set { lock (this._lockObj) { this._limitIntegralTime = value; } }
		}

		public double TRTrigDelayTime
		{
			get { return this._trigDelayTime; }
			set { lock (this._lockObj) { this._trigDelayTime = value; } }
		}

		public double TRMsrtNPLC
		{
			get { return this._msrtNPLC; }
			set { lock (this._lockObj) { this._msrtNPLC = value; } }
		}

		public double TRProcessDelayTime
		{
			get { return this._processDelayTime; }
			set { lock (this._lockObj) { this._processDelayTime = value; } }
		}

		public bool TRIsEnableDetector
		{
			get { return this._isEnableDetector; }
			set { lock (this._lockObj) { this._isEnableDetector = value; } }
		}

        public bool TRIsFixedDetectorMsrtRange
        {
            get { return this._isFixedDetectorMsrtRange; }
            set { lock (this._lockObj) { this._isFixedDetectorMsrtRange = value; } }
        }

		public double TRDetectorMsrtRange
		{
			get { return this._detectorMsrtRange; }
			set { lock (this._lockObj) { this._detectorMsrtRange = value; } }
		}

        public List<double> TRDetectorMsrtRangeList
        {
            get { return this._detectorMsrtRangeList; }
            set { lock (this._lockObj) { this._detectorMsrtRangeList = value; } }
        }

		public double TRDetectorNPLC
		{
			get { return this._detectorMsrtNplc; }
			set { lock (this._lockObj) { this._detectorMsrtNplc = value; } }
		}

		public double TRDetectorBiasVolt
		{
			get { return this._detectorBiasVolt; }
			set { lock (this._lockObj) { this._detectorBiasVolt = value; } }
		}

        public bool TRIsEnableDetector2
        {
            get { return this._isEnableDetector2; }
            set { lock (this._lockObj) { this._isEnableDetector2 = value; } }
        }

        public bool TRIsFixedDetectorMsrtRange2
        {
            get { return this._isFixedDetectorMsrtRange2; }
            set { lock (this._lockObj) { this._isFixedDetectorMsrtRange2 = value; } }
        }

        public double TRDetectorMsrtRange2
        {
            get { return this._detectorMsrtRange2; }
            set { lock (this._lockObj) { this._detectorMsrtRange2 = value; } }
        }

        public List<double> TRDetectorMsrtRangeList2
        {
            get { return this._detectorMsrtRangeList2; }
            set { lock (this._lockObj) { this._detectorMsrtRangeList2 = value; } }
        }

        public double TRDetectorNPLC2
        {
            get { return this._detectorMsrtNplc2; }
            set { lock (this._lockObj) { this._detectorMsrtNplc2 = value; } }
        }

        public double TRDetectorBiasVolt2
        {
            get { return this._detectorBiasVolt2; }
            set { lock (this._lockObj) { this._detectorBiasVolt2 = value; } }
        }

		public bool TRIsTestOptical
		{
			get { return this._isTestOptical; }
			set { lock (this._lockObj) { this._isTestOptical = value; } }
		}

        public ElecTerminalSetting[] TRTerminalDescription
        {
            get { return this._terminalSettingDescription; }
            set { lock (this._lockObj) { this._terminalSettingDescription = value; } }
        }

		#endregion

		#region >>> Protected Method <<<

		protected override void ResetKeyName()
		{
			base.ResetKeyName();

			int num = this._subItemIndex + 1;     // 0-base

			string[] str = Enum.GetNames(typeof(ETransistorOptiMsrtType));

			// Reset Electrical Setting KeyName
			//this._elecSetting[0].KeyName = "IFLIVA_" + num.ToString();
			//this._elecSetting[1].KeyName = "IFLIVB_" + num.ToString();

			// Reset Tested Result KeyName and Gain Offset Seeting KeyName
			for (int i = 0; i < this._msrtResult.Length; i++)
			{
				if (this._msrtResult[i] == null)
				{
					break;
				}

				this._msrtResult[i].KeyName = str[i] + "_" + num.ToString();

                //this._msrtResult[i].Name = str[i]  + num.ToString("D2");
                this._msrtResult[i].Name = this._msrtResult[i].KeyName;
                //SetMsrtNameAsKey();

				this._gainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;

				this._gainOffsetSetting[i].Name = this._msrtResult[i].Name;
			}
		}

		private void CreateGainAndMsrtItem()
		{
			// New the MsrtResult Data and GainOffsetSetting Data
			this._msrtResult = new TestResultData[Enum.GetNames(typeof(ETransistorOptiMsrtType)).Length];

			this._gainOffsetSetting = new GainOffsetData[Enum.GetNames(typeof(ETransistorOptiMsrtType)).Length];

			for (int i = 0; i < this._msrtResult.Length; i++)
			{
				this._msrtResult[i] = new TestResultData();

                this._msrtResult[i].IsVision = true;

				this._msrtResult[i].IsEnable = true;

				if (i == ((int)ETransistorOptiMsrtType.TRLOP) || i == ((int)ETransistorOptiMsrtType.TRWATT) || i == ((int)ETransistorOptiMsrtType.TRLM) || i == ((int)ETransistorOptiMsrtType.TRPDWATT))
				{
					this._gainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.Gain);
				}
				else
				{
					this._gainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.Offset);
				}

				this._gainOffsetSetting[i].IsVision = false;

				this._gainOffsetSetting[i].IsEnable = false;
			}


			// Set Tested Result Items and Gain Offset Setting
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLOP].Unit = "mcd";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLOP].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLOP].MaxLimitValue = 999.999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLOP].MinLimitValue = 0.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLOP].MaxLimitValue2 = 999.999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLOP].MinLimitValue2 = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRLOP].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRLOP].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATT].Unit = "mW";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATT].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATT].MaxLimitValue = 9999.999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATT].MinLimitValue = 0.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATT].MaxLimitValue2 = 9999.999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATT].MinLimitValue2 = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRWATT].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRWATT].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRLM].Unit = "lm";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLM].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLM].MaxLimitValue = 999.999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLM].MinLimitValue = 0.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLM].MaxLimitValue2 = 999.999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLM].MinLimitValue2 = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRLM].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRLM].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLP].Unit = "nm";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLP].Formate = "0.00";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLP].MaxLimitValue = 780.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLP].MinLimitValue = 380.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRWLP].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRWLP].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLD].Unit = "nm";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLD].Formate = "0.00";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLD].MaxLimitValue = 780.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLD].MinLimitValue = 380.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRWLD].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRWLD].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLC].Unit = "nm";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLC].Formate = "0.00";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLC].MaxLimitValue = 780.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLC].MinLimitValue = 380.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRWLC].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRWLC].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRHW].Unit = "nm";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRHW].Formate = "0.00";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRHW].MaxLimitValue = 780.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRHW].MinLimitValue = 380.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRPURITY].Unit = "";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPURITY].Formate = "0.00";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPURITY].MaxLimitValue = 1.00d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPURITY].MinLimitValue = 0.00d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEx].Unit = "";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEx].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEx].MaxLimitValue = 1.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEx].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEy].Unit = "";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEy].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEy].MaxLimitValue = 1.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEy].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRUprime].Unit = "";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRUprime].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRUprime].MaxLimitValue = 1.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRUprime].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRVprime].Unit = "";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRVprime].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRVprime].MaxLimitValue = 1.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRVprime].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEz].Unit = "";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEz].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEz].MaxLimitValue = 1.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCIEz].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRCCT].Unit = "K";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCCT].Formate = "0";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCCT].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCCT].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRCRI].Unit = "";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCRI].Formate = "0";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCRI].MaxLimitValue = 100.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRCRI].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRST].Unit = "ms";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRST].Formate = "0.0";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRST].MaxLimitValue = 99999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRST].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRINT].Unit = "cnt";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINT].Formate = "0";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINT].MaxLimitValue = 999999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINT].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRINTP].Unit = "%";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINTP].Formate = "0.00";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINTP].MaxLimitValue = 100.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINTP].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLCP].Unit = "nm";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLCP].Formate = "0.00";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLCP].MaxLimitValue = 780.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWLCP].MinLimitValue = 380.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRSTR].Unit = "cnt";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRSTR].Formate = "0.0";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRSTR].MaxLimitValue = 999999d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRSTR].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRDWDWP].Unit = "nm";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDWDWP].Formate = "0.00";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDWDWP].MaxLimitValue = 780.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDWDWP].MinLimitValue = 380.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKA].Unit = "cnt";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKA].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKA].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKA].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKB].Unit = "cnt";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKB].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKB].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKB].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKB].Unit = "cnt";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKB].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKB].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDARKB].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRINTSS].Unit = "ms";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINTSS].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINTSS].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRINTSS].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRDrainEWATT].Unit = "W";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDrainEWATT].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDrainEWATT].MaxLimitValue = 10.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDrainEWATT].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRLE].Unit = "lm/W";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLE].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLE].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLE].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRWPE].Unit = "%";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWPE].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWPE].MaxLimitValue = 100.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWPE].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRDuv].Unit = "";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDuv].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDuv].MaxLimitValue = 100.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRDuv].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRTIME].Unit = "ms";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRTIME].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRTIME].MaxLimitValue = double.MaxValue;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRTIME].MinLimitValue = 0.0d;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATTTD].Unit = "mW";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATTTD].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATTTD].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRWATTTD].MinLimitValue = 0.0d;
			//this._msrtResult[(int)ETransistorOptiMsrtType.TRWATTTD].IsVision = true;
			//this._msrtResult[(int)ETransistorOptiMsrtType.TRWATTTD].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRLMTD].Unit = "lm";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLMTD].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLMTD].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRLMTD].MinLimitValue = 0.0d;
			//this._msrtResult[(int)ETransistorOptiMsrtType.TRLMTD].IsVision = true;
			//this._msrtResult[(int)ETransistorOptiMsrtType.TRLMTD].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRPDCURRENT].Unit = "uA";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPDCURRENT].Formate = "0.000000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPDCURRENT].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPDCURRENT].MinLimitValue = 0.0d;


			this._msrtResult[(int)ETransistorOptiMsrtType.TRPDWATT].Unit = "mW";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPDWATT].Formate = "0.000000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPDWATT].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRPDWATT].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRPDWATT].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRPDWATT].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainV].Unit = "V";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainV].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainV].MaxLimitValue = 8.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainV].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtDrainV].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtDrainV].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceV].Unit = "V";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceV].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceV].MaxLimitValue = 8.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceV].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtSourceV].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtSourceV].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateV].Unit = "V";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateV].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateV].MaxLimitValue = 8.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateV].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtGateV].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtGateV].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukV].Unit = "V";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukV].Formate = "0.0000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukV].MaxLimitValue = 8.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukV].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtBlukV].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtBlukV].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainI].Unit = "mA";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainI].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainI].MaxLimitValue = 2000.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainI].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtDrainI].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtDrainI].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceI].Unit = "mA";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceI].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceI].MaxLimitValue = 2000.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceI].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtSourceI].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtSourceI].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateI].Unit = "mA";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateI].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateI].MaxLimitValue = 2000.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateI].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtGateI].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtGateI].IsEnable = true;

			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukI].Unit = "mA";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukI].Formate = "0.000";
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukI].MaxLimitValue = 2000.0d;
			this._msrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukI].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtBlukI].IsVision = true;
			this._gainOffsetSetting[(int)ETransistorOptiMsrtType.TRMsrtBlukI].IsEnable = true;

			for (int k = (int)ETransistorOptiMsrtType.TRR01; k < (int)ETransistorOptiMsrtType.TRR15; k++)
			{
				this._msrtResult[k].Unit = "";
				this._msrtResult[k].Formate = "0.0";
				this._msrtResult[k].MaxLimitValue = 100.0d;
				this._msrtResult[k].MinLimitValue = 0.0d;
			}
		}

		#endregion

		#region >>> public Method <<<

		public void TRApplyParameter()
		{
			if (this._sweepPoints <= 0)
			{
				return;
			}

			if (this._processTimer == null)
			{
				this._processTimer = new PerformanceTimer();
			}

			if (this._fixedSITTimer == null)
			{
				this._fixedSITTimer = new PerformanceTimer();
			}

			//==============================================================
			// Sourcemeter setting, electric setting
			//==============================================================
			uint samplingCount = this._sweepPoints;

			for (int i = 0; samplingCount != this._elecSettingList.Count; i++)
			{
				if (samplingCount < this._elecSettingList.Count)
				{
					this._elecSettingList.RemoveAt(0);
				}
				else
				{
					this._elecSettingList.Add(new ElectSettingData());
				}
			}

			this._elecSettingList[0].IsSweepFirstElec = true;

			for (int i = 0; i < samplingCount; i++)
			{
                this._elecSettingList[i].MsrtType = EMsrtType.TRANSISTOR;

				this._elecSettingList[i].IsAutoForceRange = false;

				this._elecSettingList[i].TurnOffTime = this._turnOffTime;

                if (i == 0)
                {
                    this._elecSettingList[i].IsSweepFirstElec = true;
                }

                //-------------------------------------------------------------------------------------------
                // Terminal Setting
                int subIndex = 0;

                foreach (var subItem in this._elecSettingList[i].ElecTerminalSetting)
                {
                    ElecTerminalSetting trDescp = this._terminalSettingDescription[subIndex];

                    subItem.SMU = trDescp.SMU;
                    subItem.TerminalName = trDescp.TerminalName;
                    subItem.Description = trDescp.Description;
					subItem.SequenceOrder = trDescp.SequenceOrder;
                    subItem.MsrtType = trDescp.MsrtType;

                    if (i == 0)
                    {
                        subItem.ForceDelayTime = this._processDelayTime;
                    }
                    else
                    {
                        subItem.ForceDelayTime = 0.0d;
                    }

                    subItem.ForceTime = this._forceTime;
                    subItem.ForceTimeUnit = "ms";

                    switch (trDescp.MsrtType)
                    {
                        case EMsrtType.FIMVSWEEP:
                            {
                                subItem.MsrtType = EMsrtType.FIMV;
                                subItem.TermianlFuncType = ETermianlFuncType.Sweep;
                                subItem.SweepMode = trDescp.SweepMode;
                                subItem.TurnOffTime = this._turnOffTime;

                                if (subItem.SweepMode == ESweepMode.Custom)
                                {
                                    subItem.ForceValue = trDescp.SweepCustomList[i];
                                }
                                else if (subItem.SweepMode == ESweepMode.Log)
                                {
                                    subItem.ForceValue = trDescp.SweepLogList[i];
                                }
                                else
                                {
                                    subItem.ForceValue = trDescp.SweepStart + trDescp.SweepStep * i;
                                }

                                break;
                            }
                        case EMsrtType.FVMISWEEP:
                            {
                                subItem.MsrtType = EMsrtType.FVMI;
                                subItem.TermianlFuncType = ETermianlFuncType.Sweep;
                                subItem.SweepMode = trDescp.SweepMode;
                                subItem.TurnOffTime = this._turnOffTime;

                                if (subItem.SweepMode == ESweepMode.Custom)
                                {
                                    subItem.ForceValue = trDescp.SweepCustomList[i];
                                }
                                else
                                {
                                    subItem.ForceValue = trDescp.SweepStart + trDescp.SweepStep * i;
                                }

                                break;
                            }
                        case EMsrtType.FIMV:
                        case EMsrtType.FVMI:
                            {
                                subItem.MsrtType = trDescp.MsrtType;
                                subItem.TermianlFuncType = ETermianlFuncType.Bias;
                                subItem.ForceValue = trDescp.ForceValue;

                                subItem.TurnOffTime = 0.0d;
                                break;
                            }
                        default:
                            break;
                    }

                    subItem.ForceUnit = trDescp.ForceUnit;

                    subItem.IsAutoTurnOff = false;

                    subItem.MsrtRange = trDescp.MsrtProtection;
                    subItem.MsrtProtection = trDescp.MsrtProtection;
                    subItem.MsrtUnit = trDescp.MsrtUnit;
                    subItem.MsrtNPLC = this._msrtNPLC;

                    subIndex++;
                }

                //-------------------------------------------------------------------------------------------
                // SpectroMeter Setting
				if (this._sensingMode == ESensingMode.Fixed)
				{
					this._elecSettingList[i].ForceTimeExt = this._fixIntegralTime;
				}
				else if (this._sensingMode == ESensingMode.Limit)
				{
					this._elecSettingList[i].ForceTimeExt = this._limitIntegralTime;
				}

                //-------------------------------------------------------------------------------------------
                // PD Detector Setting
				this._elecSettingList[i].IsTrigDetector = this._isEnableDetector;

				this._elecSettingList[i].DetectorBiasValue = this._detectorBiasVolt;

                if (this._isFixedDetectorMsrtRange)
                {
				this._elecSettingList[i].DetectorMsrtRange = this._detectorMsrtRange;
                }
                else
                {
                    this._elecSettingList[i].DetectorMsrtRange = this._detectorMsrtRangeList[i];
                }

				this._elecSettingList[i].DetectorMsrtNPLC = this._detectorMsrtNplc;
			}

			this._elecSetting = this._elecSettingList.ToArray();

			//==============================================================
			// Spectrometer setting, optical setting
			//==============================================================          
			for (int i = 0; samplingCount != this._optiSettingList.Count; i++)
			{
				if (samplingCount < this._optiSettingList.Count)
				{
					this._optiSettingList.RemoveAt(0);
				}
				else
				{
					this._optiSettingList.Add(new OptiSettingData());
				}
			}

			for (int i = 0; i < samplingCount; i++)
			{
                this._optiSettingList[i].SensingMode = this._sensingMode;

				this._optiSettingList[i].FixIntegralTime = this._fixIntegralTime;

				this._optiSettingList[i].LimitIntegralTime = this._limitIntegralTime;

				this._optiSettingList[i].TrigDelayTime = this._trigDelayTime;
			}

		}

		public void TRProcessStart()
		{
			this._processTimer.Start();
		}

		public void TRProcessEnd()
		{
			this._processTimer.Stop();
		}

		public double TRProcessTime()
		{
			return this._processTimer.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
		}

		public void ClearDataList()
		{
			foreach (var item in this.MsrtResult)
			{
				item.DataList.Clear();
			}
		}

        public bool CheckTerminalSMU(ETerminalName name, ESMU smu)
        {
            if (smu == ESMU.None)
            {
                return true;
            }

            foreach (var data in this._terminalSettingDescription)
            {
                if (name == data.TerminalName)
                {
                    continue;
                }
                
                if (data.SMU == smu)
                {
                    return false;
                }
            }

            return true;
        }

        public ETerminalName GetTerminalNameByOrder(int order)
        {
            if (order >= 0)
            {
                foreach (var data in this._terminalSettingDescription)
                {
                    if (data.SequenceOrder == order)
                    {
                        return data.TerminalName;
                    }
                }
            }

            return ETerminalName.None;
        }

        public ESMU GetSmuByOrder(int order)
        {
            if (order >= 0)
            {
                foreach (var data in this._terminalSettingDescription)
                {
                    if (data.SequenceOrder == order)
                    {
                        return data.SMU;
                    }
                }
            }

            return ESMU.None;
        }

		#endregion
	}
}
