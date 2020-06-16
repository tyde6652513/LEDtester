using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
	[Serializable]
    public class ElectSettingData : ICloneable
    {
        public const uint   MIN_SWEEP_COUNT = 1;
        public const uint   MAX_SWEEP_COUNT = 2000;

        private object _lockObj;

        private string _id;
        private string _keyName;
        private string _name;

        private uint _order;

        private EMsrtType _msrtType;

        private double _forceDelayTime;
        private double _forceValue;
		private double _forceValueFrequency;
        private double _forceTime;			
		private double _forceTimeVR;
		private double _forceTimeExt;
        private double _forceRange;
        private int _forceRangeIndex;
        private double _turnOffTime;

        private double _msrtProtection;		
        private double[] _msrtValue;
        private double _msrtRange;			
        private int _msrtRangeIndex;
        private int _msrtFilterCount;
        private double _msrtNPLC;
        private int _thySGFilterCount;
		private int _thyMovingAverageWindow;

        private string _forceUnit;
        private string _msrtUnit;
        private string _forceTimeUnit;

        private ESweepMode _sweepMode;
        private double _sweepHoldTime;
        private double[] _sweepCustomValue;
        private double _sweepStart;
        private double _sweepStop;
        private double _sweepStep;
		private double _sweepTurnOffTime;
        private uint _sweepRiseCount;
		private uint _sweepContCount;
        private bool _isSweepAutoMsrtRange;

        private uint _pmuIndex;
        private bool _isAutoTurnOff;

        private bool _isAutoForceRange;

        private bool _isBipolarOutput;

        private double _polarThresholdVoltage;

		private double _rthImforceTime;
		private double _rthIm2forceTime;
        private double _rthIhforceValue;
        private double _rthIhforceTime;

        private bool _isEnableK26RevisedScript;

        private double _safetyClampGain;

	    private bool _isTrigDetector;
        private double _detectorBiasValue;
        private double _detectorMsrtRange;
        private double _detectorNPLC;
        private bool _isDetectorAutoMsrtRange;

        private bool _isTrigDetector2;
        private double _detectorBiasValue2;
        private double _detectorMsrtRange2;
        private double _detectorNPLC2;
        private bool _isDetectorAutoMsrtRange2;

		private bool _isSweepFirstElec;

        private bool _isPulseMode;

        private uint _pulseCount;

        private double _duty;

		private ElecTerminalSetting[] _elecTerminalSetting;

        private bool _isEnableFloatForceValue;

		private EContactCheckSpeed _conatctCheckSpeed;

		private double _msrtRTestItemRange;

		private ERTestItemMsrtSpeed _msrtRTestItemSpeed;

        private bool _isNextIsESDTestItem;

        private bool _isEnableMsrtForceValue;

        private ESourceFunc _srcFunc;

        private uint _calcMsrtFromPulseIndex;

        private IOSetting _ioSetting;

		private double _forceTrigDelayTime;

		private bool _isAutoMsrtRange;

        private bool _isTrigCamera;

        public ElectSettingData()
        {
            this._lockObj = new object();

            this._id = string.Empty;
            this._keyName = string.Empty;
            this._name = string.Empty;

            this._order = 0;

            this._msrtType = EMsrtType.FV;

            this._forceDelayTime = 0.0d;
            this._forceValue = 0.0d;
			this._forceValueFrequency = 0.0d;
            this._forceTime = 0.0d;
			this._forceTimeVR = 0.0d;
			this._forceTimeExt = 0.0d;
            this._forceRange = 0.0d;
            this._forceRangeIndex = 0;
            this._turnOffTime = 0.0d;

			this._msrtProtection = 0.0;              
            this._msrtValue = new double[1];
            this._msrtRange = 0.0;
            this._msrtRangeIndex = 0;
            this._msrtFilterCount = 3;
            this._msrtNPLC = 0.01;

            this._forceUnit = string.Empty;
            this._msrtUnit = string.Empty;
            this._forceTimeUnit = string.Empty;


            this._sweepMode = ESweepMode.Linear;
            this._sweepHoldTime = 0.0d;
            this._sweepCustomValue = new double[] { 0.0d, 0.0d };
            this._sweepStart = 0.0d;
            this._sweepStop = 1.0d;
            this._sweepStep = 1.0d;
			this._sweepTurnOffTime = 0.0d;
            IsSweepFirstElec = true;
            this.ResetCount();

            this._pmuIndex = 0;
            this._isAutoTurnOff = true;

            this._isAutoForceRange = true;
            this._isSweepAutoMsrtRange = true;
            this._isBipolarOutput = false;

			this._polarThresholdVoltage = 4.0d;
			this._thySGFilterCount = 0;
			this._thyMovingAverageWindow = 5;

			this._rthImforceTime = 0.0d;
			this._rthIm2forceTime = 0.0d;
            this._rthIhforceValue = 0.0d;
            this._rthIhforceTime = 0.0d;

			this._safetyClampGain = 1.0d;

            this._isEnableK26RevisedScript = false;

            this._isSweepFirstElec = true;

            this._isPulseMode = false;

            this._pulseCount = 1;

			this._elecTerminalSetting = new ElecTerminalSetting[4]{new ElecTerminalSetting(), 
															new ElecTerminalSetting(),
															new ElecTerminalSetting(),
															new ElecTerminalSetting()};

            this._isEnableFloatForceValue = false;

			this._conatctCheckSpeed = EContactCheckSpeed.FAST;

			this._msrtRTestItemRange = 1000.0d;

			this._msrtRTestItemSpeed = ERTestItemMsrtSpeed.FAST;

            this.IsTrigDetector = false;

            this._detectorBiasValue = 0.0d;

            this._detectorMsrtRange = 1;

            this._detectorNPLC = 0.01;

            this._isDetectorAutoMsrtRange = false;

            this.IsTrigDetector2 = false;

            this._detectorBiasValue2 = 0.0d;

            this._detectorMsrtRange2 = 1;

            this._detectorNPLC2 = 0.01;

			this._isDetectorAutoMsrtRange2 = false;

			this._isNextIsESDTestItem = false;

            this._isEnableMsrtForceValue = false;

            this._srcFunc = ESourceFunc.CW;

            this._duty = 100.0d;

            this._calcMsrtFromPulseIndex = 1;

            this._isTrigCamera = false;

            this._forceTrigDelayTime = 0.0d;

			this._isAutoMsrtRange = false;

            IsSweepEnd = true;

        }

        public ElectSettingData(string forceUnit, string msrtUnit, string foreTimeUnit) : this()
        {
            this._forceUnit = forceUnit;
			this._msrtUnit = msrtUnit;
			this._forceTimeUnit = ForceTimeUnit ; //  (EAmpVoltUnit)Enum.Parse(typeof(EAmpVoltUnit), foreTimeUnit, true); 
        }

        #region >>> Public Property <<<
        /// <summary>
        /// ID number of Electrical Setting Item
        /// </summary>
        public string ID
        {
            get { return this._id; }
            set { lock (this._lockObj) { this._id = value; } }
        }

        /// <summary>
        /// KeyName of Electrical Setting Item 
        /// </summary>
        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        /// <summary>
        /// Name of Electrical Setting Item
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }

        public uint Order
        {
            get { return this._order; }
            set { lock (this._lockObj) { this._order = value; } }
        }

        /// <summary>
        /// MsrtType of Electrical Setting Item for Device Operation
        /// </summary>
        public EMsrtType MsrtType
        {
            get { return this._msrtType; }
            set { lock (this._lockObj) { this._msrtType = value; } }
        }

        /// <summary>
        /// Force delay / Source delay
        /// </summary>
        public double ForceDelayTime
        {
            get { return this._forceDelayTime; }
            set { lock (this._lockObj) { this._forceDelayTime = value; } }
        }

        /// <summary>
        /// Force value / Source value
        /// </summary>
        public double ForceValue
        {
            get { return this._forceValue; }
            set { lock (this._lockObj) { this._forceValue = value; } }
        }

        /// <summary>
		/// Force value frequency/ Source value frequency
		/// </summary>
		public double ForceValueFrequency
		{
			get { return this._forceValueFrequency; }
			set { lock (this._lockObj) { this._forceValueFrequency = value; } }
		}

        /// <summary>
        /// Force Time
        /// </summary>
        public double ForceTime
        {
            get { return this._forceTime; }
            set { lock (this._lockObj) { this._forceTime = value; } }
        }

		/// <summary>
		/// Force Time
		/// </summary>
		public double ForceTimeVR
		{
			get { return this._forceTimeVR; }
			set { lock (this._lockObj) { this._forceTimeVR = value; } }
		}

		/// <summary>
		/// Extended Force Time 
		/// </summary>
		public double ForceTimeExt
		{
			get { return this._forceTimeExt; }
			set { lock (this._lockObj) { this._forceTimeExt = value; } }
		}

        /// <summary>
        /// Force Range
        /// </summary>
        public double ForceRange
        {
            get { return this._forceRange; }
            set { lock (this._lockObj) { this._forceRange = value; } }
        }

        /// <summary>
        /// Force Range Index
        /// </summary>
        public int ForceRangeIndex
        {
            get { return this._forceRangeIndex; }
            set { lock (this._lockObj) { this._forceRangeIndex = value; } }
        }

        /// <summary>
        /// Turn Off Time
        /// </summary>
        public double TurnOffTime
        {
            get { return this._turnOffTime; }
            set { lock (this._lockObj) { this._turnOffTime = value; } }
        }

        /// <summary>
        /// Measurement protection / Sense protection / Sense compliance
        /// </summary>
        public double MsrtProtection
        {
            get { return this._msrtProtection; }
            set { lock (this._lockObj) { this._msrtProtection = value; } }
        }

        /// <summary>
        /// Measured value
        /// </summary>
        public double[] MsrtValue
        {
            get { return this._msrtValue; }
            set { lock (this._lockObj) { this._msrtValue = value; } }
        }

        /// <summary>
        /// Measurement range / Sense range
        /// </summary>
        public double MsrtRange
        {
            get { return this._msrtRange; }
            set { lock (this._lockObj) { this._msrtRange = value; } }
        }

        /// <summary>
        /// Measurement range index / Sense range index
        /// </summary>
        public int MsrtRangeIndex
        {
            get { return this._msrtRangeIndex; }
            set { lock (this._lockObj) { this._msrtRangeIndex = value; } }
        }

        /// <summary>
        /// Count of filter count
        /// </summary>
        public int MsrtFilterCount
        {
            get { return this._msrtFilterCount; }
            set { lock (this._lockObj) { this._msrtFilterCount = value; } }
        }

        /// <summary>
        /// The Number of Power Line Cycle
        /// </summary>
        public double MsrtNPLC
        {
            get { return this._msrtNPLC; }
            set { lock (this._lockObj) { this._msrtNPLC = value; } }
        }

        /// <summary>
        /// Unit of force value
        /// </summary>
        public string ForceUnit
        {
            get { return this._forceUnit; }
            set { lock (this._lockObj) { this._forceUnit = value; } }
        }

        /// <summary>
        ///  Unit of measured value
        /// </summary>
        public string MsrtUnit
        {
            get { return this._msrtUnit; }
            set { lock (this._lockObj) { this._msrtUnit = value; } }
        }

		/// <summary>
		/// Unit of force time
		/// </summary>
		public string ForceTimeUnit
		{
			get { return this._forceTimeUnit; }
			set { lock (this._lockObj) { this._forceTimeUnit = value; } }
		}

        public ESweepMode SweepMode
        {
            get { return this._sweepMode; }
            set { lock (this._lockObj) { this._sweepMode = value; } }
        }

        /// <summary>
        /// Sweep output value
        /// </summary>
        public double[] SweepCustomValue
        {
            get { return this._sweepCustomValue; }
            set
            {
                lock (this._lockObj)
                {
                    if (value == null)
                    {
                        this._sweepCustomValue = null;
                        this._msrtValue = null;
                    }
                    else if (value.Length < MIN_SWEEP_COUNT)
                    {
                        return;
                    }
                    else
                    {
                        this._sweepCustomValue = value;
                        this._msrtValue = new double[this._sweepCustomValue.Length];
                    }
                }
            }
        }

        public double SweepStart
        {
            get { return this._sweepStart; }
            set { lock (this._lockObj) { this._sweepStart = value; } }
        }

        public double SweepStop
        {
            get { return this._sweepStop; }
            set { lock (this._lockObj) { this._sweepStop = value; } }
        }

        public double SweepStep
        {
            get { return this._sweepStep; }
            set { lock (this._lockObj) { this._sweepStep = value; } }
        }

		public double SweepTurnOffTime
		{
			get { return this._sweepTurnOffTime; }
			set { lock (this._lockObj) { this._sweepTurnOffTime = value; } }
		}

        public uint SweepRiseCount
        {
            get { return this._sweepRiseCount; }
            set { lock (this._lockObj) { this._sweepRiseCount = value; } }
        }

		public uint SweepContCount
		{
			get { return this._sweepContCount; }
			set { lock (this._lockObj) { this._sweepContCount = value; } }
		}

        /// <summary>
        /// Index of PMU ( Power Measurement Unit )
        /// </summary>
        public uint PUMIndex
        {
            get { return this._pmuIndex; }
            set { lock (this._lockObj) { this._pmuIndex = value; } }
        }

        /// <summary>
        /// Is Enable Auto Turn-Off
        /// </summary>
        public bool IsAutoTurnOff
        {
            get { return this._isAutoTurnOff; }
            set { lock (this._lockObj) { this._isAutoTurnOff = value; } }
        }

        /// <summary>
        /// Is Enable Auto Set Force Range
        /// </summary>
        public bool IsAutoForceRange
        {
            get { return this._isAutoForceRange; }
            set { lock (this._lockObj) { this._isAutoForceRange = value; } }
        }

        /// <summary>
        /// Is Enable Bipolar-Output Voltage and Current
        /// </summary>
        public bool IsBipolarOutput
        {
            get { return this._isBipolarOutput; }
            set { lock (this._lockObj) { this._isBipolarOutput = value; } }
        }

        public double PolarThresholdVoltage
        {
			get { return this._polarThresholdVoltage; }
			set { lock (this._lockObj) { this._polarThresholdVoltage = value; } }
        }

        public int ThySGFilterCount
        {
            get { return this._thySGFilterCount; }
            set { lock (this._lockObj) { this._thySGFilterCount = value; } }
        }

		public int ThyMovingAverageWindow
        {
			get { return this._thyMovingAverageWindow; }
			set { lock (this._lockObj) { this._thyMovingAverageWindow = value; } }
        }

		public double RTHImForceTime
		{
			get { return this._rthImforceTime; }
			set { lock (this._lockObj) { this._rthImforceTime = value; } }
		}

		public double RTHIm2ForceTime
		{
			get { return this._rthIm2forceTime; }
			set { lock (this._lockObj) { this._rthIm2forceTime = value; } }
		}

        public double RTHIhForceValue
        {
            get { return this._rthIhforceValue; }
            set { lock (this._lockObj) { this._rthIhforceValue = value; } }
        }

        public double RTHIhForceTime
        {
            get { return this._rthIhforceTime; }
            set { lock (this._lockObj) { this._rthIhforceTime = value; } }
        }

        public bool IsEnableK26RevisedScript
        {
            get { return this._isEnableK26RevisedScript; }
            set { lock (this._lockObj) { this._isEnableK26RevisedScript = value; } }
        }

        public double SafetyClampGain
        {
            get { return this._safetyClampGain; }
            set { lock (this._lockObj) { this._safetyClampGain = value; } }
        }

        public bool IsTrigDetector
        {
            get { return this._isTrigDetector; }
            set { lock (this._lockObj) { this._isTrigDetector = value; } }
        }

        public double DetectorBiasValue
        {
            get { return this._detectorBiasValue; }
            set { lock (this._lockObj) { this._detectorBiasValue = value; } }
        }

        public double DetectorMsrtNPLC
        {
            get { return this._detectorNPLC; }
            set { lock (this._lockObj) { this._detectorNPLC = value; } }
        }

		public double DetectorMsrtRange
        {
            get { return this._detectorMsrtRange; }
            set { lock (this._lockObj) { this._detectorMsrtRange = value; } }
        }

        public bool IsDetectorAutoMsrtRange
        {
            get { return this._isDetectorAutoMsrtRange; }
            set { lock (this._lockObj) { this._isDetectorAutoMsrtRange = value; } }
        }

        public bool IsTrigDetector2
        {
            get { return this._isTrigDetector2; }
            set { lock (this._lockObj) { this._isTrigDetector2 = value; } }
        }

        public double DetectorBiasValue2
        {
            get { return this._detectorBiasValue2; }
            set { lock (this._lockObj) { this._detectorBiasValue2 = value; } }
        }

        public double DetectorMsrtNPLC2
        {
            get { return this._detectorNPLC2; }
            set { lock (this._lockObj) { this._detectorNPLC2 = value; } }
        }

        public double DetectorMsrtRange2
        {
            get { return this._detectorMsrtRange2; }
            set { lock (this._lockObj) { this._detectorMsrtRange2 = value; } }
        }

        public bool IsSweepFirstElec
        {
            get { return this._isSweepFirstElec; }
            set { lock (this._lockObj) { this._isSweepFirstElec = value; } }
        }

        public bool IsSweepAutoMsrtRange
        {
            get { return this._isSweepAutoMsrtRange; }
            set { lock (this._lockObj) { this._isSweepAutoMsrtRange = value; } }
        }

 		public bool IsDetectorAutoMsrtRange2
        {
            get { return this._isDetectorAutoMsrtRange2; }
            set { lock (this._lockObj) { this._isDetectorAutoMsrtRange2 = value; } }
        }

        public bool IsPulseMode
        {
            get { return this._isPulseMode; }
            set { lock (this._lockObj) { this._isPulseMode = value; } }
        }

        public uint PulseCount
        {
            get { return this._pulseCount; }
            set { lock (this._lockObj) { this._pulseCount = value; } }
        }

		public ElecTerminalSetting[] ElecTerminalSetting
		{
			get { return this._elecTerminalSetting; }
			set { lock (this._lockObj) { this._elecTerminalSetting = value; } }
		}

        public bool IsFloatForceValue
        {
            get { return this._isEnableFloatForceValue; }
            set { lock (this._lockObj) { this._isEnableFloatForceValue = value; } }
        }

        public double SweepHoldTime
        {
            get { return this._sweepHoldTime; }
            set { lock (this._lockObj) { this._sweepHoldTime = value; } }
        }

		public EContactCheckSpeed ConatctCheckSpeed
		{
			get { return this._conatctCheckSpeed; }
			set { lock (this._lockObj) { this._conatctCheckSpeed = value; } }
		}

		public double MsrtRTestItemRange
		{
			get { return this._msrtRTestItemRange; }
			set { lock (this._lockObj) { this._msrtRTestItemRange = value; } }
		}

		public ERTestItemMsrtSpeed MsrtRTestItemSpeed
		{
			get { return this._msrtRTestItemSpeed; }
			set { lock (this._lockObj) { this._msrtRTestItemSpeed = value; } }
		}

		public bool IsNextIsESDTestItem
		{
			get { return this._isNextIsESDTestItem; }
			set { lock (this._lockObj) { this._isNextIsESDTestItem = value; } }
		}

        public bool IsEnableMsrtForceValue
        {
            get { return this._isEnableMsrtForceValue; }
            set { lock (this._lockObj) { this._isEnableMsrtForceValue = value; } }
        }

        public ESourceFunc SourceFunction
        {
            get { return this._srcFunc; }
            set { lock (this._lockObj) { this._srcFunc = value; } }
        }

        public double Duty
        {
            get { return this._duty; }
            set { lock (this._lockObj) { this._duty = value; } }
        }

        public uint CalcMsrtFromPulseIndex
        {
            get { return this._calcMsrtFromPulseIndex; }
            set { lock (this._lockObj) { this._calcMsrtFromPulseIndex = value; } }
        }

        public IOSetting IOSetting
        {
            get { return this._ioSetting; }
            set { lock (this._lockObj) { this._ioSetting = value; } }
        }
		public bool IsAutoMsrtRange
        {
            get { return this._isAutoMsrtRange; }
            set { lock (this._lockObj) { this._isAutoMsrtRange = value; } }
        }

        public bool IsTrigCamera
        {
            get { return this._isTrigCamera; }
            set { lock (this._lockObj) { this._isTrigCamera = value; } }
        }

        /// <summary>
        /// Force Trigger Delay Time
        /// </summary>
        public double ForceTriggerDelayTime
        {
            get { return this._forceTrigDelayTime; }
            set { lock (this._lockObj) { this._forceTrigDelayTime = value; } }
        }

        public bool IsSweepEnd { get; set; }
        #endregion

        #region >>> Private Method <<<

        private void ResetCount()
        {
            if ( this._sweepStep < 0.0d )
            {
                return ;
            }
				else if ( this._sweepStep < ( Double.Epsilon * 1000 ) )   // sweep count = 0.0d 
            {
                this._sweepRiseCount = MAX_SWEEP_COUNT;
                this._sweepContCount = 0;
            }
            else
            {
                if( this._sweepStart <= this._sweepStop )
                {
                    this._sweepRiseCount = (uint)( ( this._sweepStop - this._sweepStart ) / this._sweepStep ) + 1;
                }
                else
                {
						 this._sweepRiseCount = (uint)( ( this._sweepStart - this._sweepStop ) / this._sweepStep ) + 1;
                }

					 if ( this._sweepRiseCount < MIN_SWEEP_COUNT )
                {
						 this._sweepRiseCount = MIN_SWEEP_COUNT;
                }
            }
        }

		private double UnitConvertFactor(string fromUnit, string toUnit)
		{
			double scale = 0.0d;
			double order = 1.0d;

			if (Enum.IsDefined(typeof(EAmpUnit), fromUnit) && Enum.IsDefined(typeof(EAmpUnit), toUnit))
			{
				order = (double)((EAmpUnit)Enum.Parse(typeof(EAmpUnit), fromUnit, false) - (EAmpUnit)Enum.Parse(typeof(EAmpUnit), toUnit, false));
				scale = Math.Pow(10.0d, order);
			}
			else if (Enum.IsDefined(typeof(EVoltUnit), fromUnit) && Enum.IsDefined(typeof(EVoltUnit), toUnit))
			{
				order = (double)((EVoltUnit)Enum.Parse(typeof(EVoltUnit), fromUnit, false) - (EVoltUnit)Enum.Parse(typeof(EVoltUnit), toUnit, false));
				scale = Math.Pow(10.0d, order);
			}
            else if (Enum.IsDefined(typeof(ETimeUnit), fromUnit) && Enum.IsDefined(typeof(ETimeUnit), toUnit))
            {
                order = (double)((ETimeUnit)Enum.Parse(typeof(ETimeUnit), fromUnit, false) - (ETimeUnit)Enum.Parse(typeof(ETimeUnit), toUnit, false));
                scale = Math.Pow(10.0d, order);
            }
			else
			{
				scale = 1.0d;
			}

			return scale;
		}

        #endregion

        #region >>> Public Methods <<<

        public object Clone()
        {
			ElectSettingData data = this.MemberwiseClone() as ElectSettingData;
            //if (this._elecTerminalSetting != null)
            //{
            //    data._elecTerminalSetting = new ElecTerminalSetting[this._elecTerminalSetting.Length];

            //    for (int i = 0; i < data._elecTerminalSetting.Length; i++)
            //    {
            //        if (this._elecTerminalSetting[i] != null)
            //        {
            //            data._elecTerminalSetting[i] = this._elecTerminalSetting[i].Clone() as ElecTerminalSetting;
            //        }
            //    }
            //}

			return data;
        }

		public ElectSettingData ConvertUnitTo(string ampUnit = "A", string voltUnit = "V", double polarity = 1,string timeUnit = "s")
		{ 
			ElectSettingData data = (ElectSettingData)this.Clone();
			double factor = 1.0d;

			if ( Enum.IsDefined(typeof(EAmpUnit), ampUnit) == false || Enum.IsDefined(typeof(EVoltUnit), voltUnit) == false)
				return data;

            // ElecSetting Unit
			switch (data.MsrtType)
            {
                //---------------------------------------------------------------------------------------------------------------------
                // Current Source
                //---------------------------------------------------------------------------------------------------------------------
                case EMsrtType.FIMV:
                case EMsrtType.FI:
                case EMsrtType.FIMVSWEEP:
                case EMsrtType.THY:
                case EMsrtType.MV:
                case EMsrtType.POLAR:
                case EMsrtType.RTH:
                case EMsrtType.VLR:
                case EMsrtType.LIV:
                case EMsrtType.FIMVLOP:
                case EMsrtType.PIV:
                    {
                        factor = this.UnitConvertFactor(data.ForceUnit, ampUnit);
                        data.ForceValue = Math.Round(data.ForceValue * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.RTHIhForceValue = Math.Round(data.RTHIhForceValue * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.ForceRange = Math.Round(data.ForceRange * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.SweepStart = Math.Round(data.SweepStart * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.SweepStep = Math.Round(data.SweepStep * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.SweepStop = Math.Round(data.SweepStop * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        
                        data.ForceUnit = ampUnit;

                        factor = this.UnitConvertFactor(data.MsrtUnit, voltUnit);
                        data.MsrtProtection = Math.Round(data.MsrtProtection * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.MsrtRange = Math.Round(data.MsrtRange * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.MsrtUnit = voltUnit;

                        //現有機制並未重新修計算Force Time因此先繞過這段
                        //factor = this.UnitConvertFactor(data.ForceTimeUnit, timeUnit);
                        //data.ForceTime = Math.Round(data.ForceTime * factor, 15, MidpointRounding.AwayFromZero);
                        //data.SweepTurnOffTime = Math.Round(data.SweepTurnOffTime * factor, 15, MidpointRounding.AwayFromZero);
                        //data.ForceTimeUnit = timeUnit;
                        ////if (data.ElecTerminalSetting != null && data.ElecTerminalSetting.Length > 0)
                        //{
                        //    for (int i = 0; i < data.ElecTerminalSetting.Length; ++i)
                        //    {
                        //        ElecTerminalSetting ets = data.ElecTerminalSetting[i];
                        //        factor = this.UnitConvertFactor(ets.ForceUnit, ampUnit);
                        //        ets.ForceValue = Math.Round(ets.ForceValue * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.ForceRange = Math.Round(ets.ForceRange * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.SweepStart = Math.Round(ets.SweepStart * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.SweepStep = Math.Round(ets.SweepStep * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.SweepStop = Math.Round(ets.SweepStop * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.ForceUnit = ampUnit;

                        //        factor = this.UnitConvertFactor(ets.MsrtUnit, voltUnit);
                        //        ets.MsrtProtection = Math.Round(ets.MsrtProtection * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.MsrtRange = Math.Round(ets.MsrtRange * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.MsrtUnit = voltUnit;

                        //        factor = this.UnitConvertFactor(ets.ForceTimeUnit, timeUnit);
                        //        ets.ForceTime = Math.Round(ets.ForceTime * factor, 15, MidpointRounding.AwayFromZero);
                        //        ets.SweepTurnOffTime = Math.Round(ets.SweepTurnOffTime * factor, 15, MidpointRounding.AwayFromZero);
                        //        ets.ForceTimeUnit = timeUnit;
                        //    }
                        //}
                        break;
                    }
                //---------------------------------------------------------------------------------------------------------------------
                // Voltage Source
                //---------------------------------------------------------------------------------------------------------------------
                case EMsrtType.FVMI:
                case EMsrtType.FV:
                case EMsrtType.FVMISWEEP:
                case EMsrtType.MI:
                case EMsrtType.FVMILOP:
                case EMsrtType.LVI:
                case EMsrtType.FVMISCAN:
                    {
                        factor = this.UnitConvertFactor(data.ForceUnit, voltUnit);
                        data.ForceValue = Math.Round(data.ForceValue * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.ForceRange = Math.Round(data.ForceRange * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.SweepStart = Math.Round(data.SweepStart * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.SweepStep = Math.Round(data.SweepStep * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.SweepStop = Math.Round(data.SweepStop * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.ForceUnit = voltUnit;

                        factor = this.UnitConvertFactor(data.MsrtUnit, ampUnit);
                        data.MsrtProtection = Math.Round(data.MsrtProtection * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.MsrtRange = Math.Round(data.MsrtRange * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        data.MsrtUnit = ampUnit;

                        //factor = this.UnitConvertFactor(data.ForceTimeUnit, timeUnit);
                        //data.ForceTime = Math.Round(data.ForceTime * factor, 15, MidpointRounding.AwayFromZero);
                        //data.SweepTurnOffTime = Math.Round(data.SweepTurnOffTime * factor, 15, MidpointRounding.AwayFromZero);
                        //data.ForceTimeUnit = timeUnit;

                        //if (data.ElecTerminalSetting != null && data.ElecTerminalSetting.Length > 0)
                        //{
                        //    for (int i = 0; i < data.ElecTerminalSetting.Length; ++i)
                        //    {
                        //        ElecTerminalSetting ets = data.ElecTerminalSetting[i];
                        //        factor = this.UnitConvertFactor(ets.ForceUnit, voltUnit);
                        //        ets.ForceValue = Math.Round(ets.ForceValue * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.ForceValue = Math.Round(ets.ForceValue * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.ForceRange = Math.Round(ets.ForceRange * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.SweepStart = Math.Round(ets.SweepStart * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.SweepStep = Math.Round(ets.SweepStep * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.SweepStop = Math.Round(ets.SweepStop * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.ForceUnit = voltUnit;

                        //        factor = this.UnitConvertFactor(ets.MsrtUnit, ampUnit);
                        //        ets.MsrtProtection = Math.Round(ets.MsrtProtection * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.MsrtRange = Math.Round(ets.MsrtRange * factor * polarity, 15, MidpointRounding.AwayFromZero);
                        //        ets.MsrtUnit = ampUnit;

                        //        factor = this.UnitConvertFactor(ets.ForceTimeUnit, timeUnit);
                        //        ets.ForceTime = Math.Round(ets.ForceTime * factor, 15, MidpointRounding.AwayFromZero);
                        //        ets.SweepTurnOffTime = Math.Round(ets.SweepTurnOffTime * factor, 15, MidpointRounding.AwayFromZero);
                        //        ets.ForceTimeUnit = timeUnit;
                        //    }
                        //}

                        break;
                    }
                //-----------------------------------------------------------------------------
                
                //-----------------------------------------------------------------------------
                default:
                    break;
            }


            //改在UI層直接做掉，讓RDFunc可設定UI上的unit
            //未新增變數改至此處處理的原因是要避免 ElectSettingData修改後出現相容性問題

            // Detector Unit
            //factor = this.UnitConvertFactor("mA", "A");

            //data.DetectorMsrtRange = Math.Round(data.DetectorMsrtRange * factor, 15, MidpointRounding.AwayFromZero);
            //data.DetectorMsrtRange2 = Math.Round(data.DetectorMsrtRange2 * factor, 15, MidpointRounding.AwayFromZero);

			return data;
		}

		public ESMU GetTerminalSMUByOrder(int order)
		{
			if (order >= 0)
			{
				foreach (var data in this._elecTerminalSetting)
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
