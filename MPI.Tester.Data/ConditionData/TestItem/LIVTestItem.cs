using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class LIVTestItem : TestItemData
    {
        #region >>> private Property <<<

        private List<OptiSettingData> _optiSettingList;
        private List<ElectSettingData> _elecSettingList;
        private bool _livIsLifeTest;
        private int _livSamplimgTime;
        private bool _livIsLogScale;
        private int _livLogScaleTime;
        private List<double> _livLogScale;
        private bool _livIsLimitModeFixedSIT;//Sampling Interval Time
        private double _livForceDelayTime;
        private double _livStartValue;
        private double _livStepValue;
        private double _livStopValue;
        private uint _livSweepPoints;

        private uint _livPUMIndex;
        private double _livForceRange;
        private double _livForceTime;
        private double _livTurnOffTime;
        private double _livMsrtRange;
        private double _livMsrtProtection;
        private int _livMsrtFilterCount;
        private ESensingMode _livSensingMode;
        private double _livFixIntegralTime;
        private double _livLimitIntegralTime;
        private double _livTrigDelayTime;
        private string _livForceTimeUnit;
        private string _livMsrtUnit;
        private string _livForceUnit;
        private double _livProcessDelayTime;
        private EMsrtType _livMsrtType;

        private double _livMsrtNPLC;

        private bool _livIsTestOptical;

        private bool _livIsEnableDetector;
        private double _livDetectorMsrtRange;
        private double _livDetectorMsrtNplc;
        private double _livDetectorBiasVolt;

        private ESweepMode _livSweepMode;

        [NonSerialized]
        private PerformanceTimer _livLimitModeFixedSITTimer;
        [NonSerialized]
        private PerformanceTimer _livFixedSITTimer;
        [NonSerialized]
        private PerformanceTimer _livProcessTimer;
        [NonSerialized]
        private List<double[]> _relativeSpectrumAarryList;
        [NonSerialized]
        private List<double[]> _absoluteSpectrumAarryList;
        [NonSerialized]
        private double[] _wavelength;

        #endregion

        public LIVTestItem()
            : base()
        {
            this._lockObj = new object();

            this._type = ETestType.LIV;

            this._livIsLifeTest = false;
            this._livSamplimgTime = 1000;
            this._livIsLogScale = false;
            this._livLogScaleTime = 1000;
            this._livLogScale = new List<double>();
            this._livLimitModeFixedSITTimer = new PerformanceTimer();
            this._livProcessTimer = new PerformanceTimer();
            this._livFixedSITTimer = new PerformanceTimer();
            this._absoluteSpectrumAarryList = new List<double[]>();
            this._relativeSpectrumAarryList = new List<double[]>();

            /////////////////////////////////////////////////
            // Electrical Setting 
            /////////////////////////////////////////////////
            this._elecSettingList = new List<ElectSettingData>();
            this._livIsLimitModeFixedSIT = false;
            this._livForceDelayTime = 0.0d;
            this._livStartValue = 0.0d;
            this._livStepValue = 1.0d;
            this._livStopValue = 1.0d;

            this._livSweepPoints = 2;

            this._livPUMIndex = 0;
            this._livForceRange = 0.0d;
            this._livForceTime = 1.0d;
            this._livTurnOffTime = 0.0d;
            this._livMsrtRange = 0.0d;
            this._livMsrtProtection = 0.0d;
            this._livMsrtFilterCount = 3;
            this._livMsrtNPLC = 0.01d;

            this._livForceUnit = EAmpUnit.mA.ToString();
            this._livForceTimeUnit = ETimeUnit.ms.ToString();
            this._livMsrtUnit = EVoltUnit.V.ToString();

            this._livProcessDelayTime = 0.0d;

            this._livMsrtType = EMsrtType.LIV;
            /////////////////////////////////////////////////
            // optical setting
            /////////////////////////////////////////////////
            this._optiSettingList = new List<OptiSettingData>();
            this._livSensingMode = ESensingMode.Fixed;
            this._livFixIntegralTime = 10.0d;
            this._livLimitIntegralTime = 50.0d;
            this._livTrigDelayTime = 0.0d;

            this._livIsTestOptical = true;

            this._livIsEnableDetector = false;
            this._livDetectorMsrtRange = 0.001d;
            this._livDetectorMsrtNplc = 0.01d;
            this._livDetectorBiasVolt = 0;

            this._livSweepMode = ESweepMode.Linear;
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

        public bool LIVIsLifeTest
        {
            get { return this._livIsLifeTest; }
            set { lock (this._lockObj) { this._livIsLifeTest = value; } }
        }

        public int LIVSamplimgTime
        {
            get { return this._livSamplimgTime; }
            set { lock (this._lockObj) { this._livSamplimgTime = value; } }
        }

        public bool LIVIsLogScale
        {
            get { return this._livIsLogScale; }
            set { lock (this._lockObj) { this._livIsLogScale = value; } }
        }

        public List<double> LIVLogScale
        {
            get { return this._livLogScale; }
            set { lock (this._lockObj) { this._livLogScale = value; } }
        }

        public int LIVLogScaleTime
        {
            get { return this._livLogScaleTime; }
            set { lock (this._lockObj) { this._livLogScaleTime = value; } }
        }

        /// <summary>
        /// Sampling Interval Time
        /// </summary>
        public bool LIVIsLimitModeFixedSIT
        {
            get { return this._livIsLimitModeFixedSIT; }
            set { lock (this._lockObj) { this._livIsLimitModeFixedSIT = value; } }
        }

        public EMsrtType LIVMsrtType
        {
            get { return this._livMsrtType; }
            set { lock (this._lockObj) { this._livMsrtType = value; } }
        }

        public double LIVForceDelayTime
        {
            get { return this._livForceDelayTime; }
            set { lock (this._lockObj) { this._livForceDelayTime = value; } }
        }

        public double LIVStartValue
        {
            get { return this._livStartValue; }
            set { lock (this._lockObj) { this._livStartValue = value; } }
        }

        public double LIVStepValue
        {
            get { return this._livStepValue; }
            set { lock (this._lockObj) { this._livStepValue = value; } }
        }

        public double LIVStopValue
        {
            get { return this._livStopValue; }
            set { lock (this._lockObj) { this._livStopValue = value; } }
        }

        public uint LIVSweepPoints
        {
            get { return this._livSweepPoints; }
            set { lock (this._lockObj) { this._livSweepPoints = value; } }
        }

        public uint LIVPUMIndex
        {
            get { return this._livPUMIndex; }
            set { lock (this._lockObj) { this._livPUMIndex = value; } }
        }

        public double LIVForceRange
        {
            get { return this._livForceRange; }
            set { lock (this._lockObj) { this._livForceRange = value; } }
        }

        public double LIVForceTime
        {
            get { return this._livForceTime; }
            set { lock (this._lockObj) { this._livForceTime = value; } }
        }

        public double LIVTurnOffTime
        {
            get { return this._livTurnOffTime; }
            set { lock (this._lockObj) { this._livTurnOffTime = value; } }
        }

        public double LIVMsrtRange
        {
            get { return this._livMsrtRange; }
            set { lock (this._lockObj) { this._livMsrtRange = value; } }
        }

        public double LIVMsrtProtection
        {
            get { return this._livMsrtProtection; }
            set { lock (this._lockObj) { this._livMsrtProtection = value; } }
        }

        public int LIVMsrtFilterCount
        {
            get { return this._livMsrtFilterCount; }
            set { lock (this._lockObj) { this._livMsrtFilterCount = value; } }
        }

        public ESensingMode LIVSensingMode
        {
            get { return this._livSensingMode; }
            set { lock (this._lockObj) { this._livSensingMode = value; } }
        }

        public double LIVFixIntegralTime
        {
            get { return this._livFixIntegralTime; }
            set { lock (this._lockObj) { this._livFixIntegralTime = value; } }
        }

        public double LIVLimitIntegralTime
        {
            get { return this._livLimitIntegralTime; }
            set { lock (this._lockObj) { this._livLimitIntegralTime = value; } }
        }

        public double LIVTrigDelayTime
        {
            get { return this._livTrigDelayTime; }
            set { lock (this._lockObj) { this._livTrigDelayTime = value; } }
        }

        public string LIVForceUnit
        {
            get { return this._livForceUnit; }
            set { lock (this._lockObj) { this._livForceUnit = value; } }
        }

        public string LIVForceTimeUnit
        {
            get { return this._livForceTimeUnit; }
            set { lock (this._lockObj) { this._livForceTimeUnit = value; } }
        }

        public string LIVMsrtUnit
        {
            get { return this._livMsrtUnit; }
            set { lock (this._lockObj) { this._livMsrtUnit = value; } }
        }

        public double LIVMsrtNPLC
        {
            get { return this._livMsrtNPLC; }
            set { lock (this._lockObj) { this._livMsrtNPLC = value; } }
        }

        public int DataLength
        {
            get { return (int)(this._livSweepPoints); }
        }

        public List<double[]> AbsoluteSpectrumAarryList
        {
            get { return this._absoluteSpectrumAarryList; }
            set { lock (this._lockObj) { this._absoluteSpectrumAarryList = value; } }
        }

        public List<double[]> RelativeSpectrumAarryList
        {
            get { return this._relativeSpectrumAarryList; }
            set { lock (this._lockObj) { this._relativeSpectrumAarryList = value; } }
        }

        public double[] Wavelength
        {
            get { return this._wavelength; }
            set { lock (this._lockObj) { this._wavelength = value; } }
        }

        public double LIVProcessDelayTime
        {
            get { return this._livProcessDelayTime; }
            set { lock (this._lockObj) { this._livProcessDelayTime = value; } }
        }

        public bool LIVIsEnableDetector
        {
            get { return this._livIsEnableDetector; }
            set { lock (this._lockObj) { this._livIsEnableDetector = value; } }
        }

        public double LIVDetectorMsrtRange
        {
            get { return this._livDetectorMsrtRange; }
            set { lock (this._lockObj) { this._livDetectorMsrtRange = value; } }
        }

        public double LIVDetectorNPLC
        {
            get { return this._livDetectorMsrtNplc; }
            set { lock (this._lockObj) { this._livDetectorMsrtNplc = value; } }
        }

        public double LIVDetectorBiasVolt
        {
            get { return this._livDetectorBiasVolt; }
            set { lock (this._lockObj) { this._livDetectorBiasVolt = value; } }
        }

        public bool LIVIsTestOptical
        {
            get { return this._livIsTestOptical; }
            set { lock (this._lockObj) { this._livIsTestOptical = value; } }
        }

        public ESweepMode LIVSweepMode
        {
            get { return this._livSweepMode; }
            set { lock (this._lockObj) { this._livSweepMode = value; } }
        }

        #endregion

        #region >>> Protected Method <<<

        protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            string[] str = Enum.GetNames(typeof(ELIVOptiMsrtType));

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
                
				//this._msrtResult[i].Name = str[i] + num.ToString("D2");

                this._msrtResult[i].Name = this._msrtResult[i].KeyName;
                //SetMsrtNameAsKey();

                this._gainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;

                this._gainOffsetSetting[i].Name = this._msrtResult[i].Name;
            }
        }

        private void CreateGainAndMsrtItem()
        {
            // New the MsrtResult Data and GainOffsetSetting Data
            this._msrtResult = new TestResultData[Enum.GetNames(typeof(ELIVOptiMsrtType)).Length];

            this._gainOffsetSetting = new GainOffsetData[Enum.GetNames(typeof(ELIVOptiMsrtType)).Length];

            for (int i = 0; i < this._msrtResult.Length; i++)
            {
                this._msrtResult[i] = new TestResultData();

                this._msrtResult[i].IsVision = false;

                this._msrtResult[i].IsEnable = true;

                if (i == ((int)ELIVOptiMsrtType.LIVLOP) || i == ((int)ELIVOptiMsrtType.LIVWATT) || i == ((int)ELIVOptiMsrtType.LIVLM) || i == ((int)ELIVOptiMsrtType.LIVPDWATT))
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
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLOP].Unit = "mcd";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLOP].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLOP].MaxLimitValue = 999.999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLOP].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLOP].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLOP].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVLOP].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVLOP].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVWATT].Unit = "mW";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWATT].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWATT].MaxLimitValue = 9999.999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWATT].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWATT].MaxLimitValue2 = 9999.999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWATT].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVWATT].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVWATT].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVLM].Unit = "lm";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLM].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLM].MaxLimitValue = 999.999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLM].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLM].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLM].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVLM].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVLM].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLP].Unit = "nm";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLP].Formate = "0.00";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLP].MaxLimitValue = 780.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLP].MinLimitValue = 380.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVWLP].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVWLP].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLD].Unit = "nm";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLD].Formate = "0.00";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLD].MaxLimitValue = 780.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLD].MinLimitValue = 380.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVWLD].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVWLD].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLC].Unit = "nm";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLC].Formate = "0.00";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLC].MaxLimitValue = 780.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLC].MinLimitValue = 380.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVWLC].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVWLC].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVHW].Unit = "nm";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVHW].Formate = "0.00";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVHW].MaxLimitValue = 780.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVHW].MinLimitValue = 380.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVPURITY].Unit = "";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPURITY].Formate = "0.00";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPURITY].MaxLimitValue = 1.00d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPURITY].MinLimitValue = 0.00d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEx].Unit = "";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEx].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEx].MaxLimitValue = 1.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEx].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEy].Unit = "";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEy].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEy].MaxLimitValue = 1.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEy].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVUprime].Unit = "";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVUprime].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVUprime].MaxLimitValue = 1.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVUprime].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVVprime].Unit = "";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVVprime].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVVprime].MaxLimitValue = 1.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVVprime].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEz].Unit = "";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEz].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEz].MaxLimitValue = 1.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCIEz].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVCCT].Unit = "K";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCCT].Formate = "0";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCCT].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCCT].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVCRI].Unit = "";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCRI].Formate = "0";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCRI].MaxLimitValue = 100.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVCRI].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVST].Unit = "ms";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVST].Formate = "0.0";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVST].MaxLimitValue = 99999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVST].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVINT].Unit = "cnt";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINT].Formate = "0";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINT].MaxLimitValue = 999999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINT].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVINTP].Unit = "%";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINTP].Formate = "0.00";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINTP].MaxLimitValue = 100.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINTP].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLCP].Unit = "nm";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLCP].Formate = "0.00";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLCP].MaxLimitValue = 780.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWLCP].MinLimitValue = 380.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVSTR].Unit = "cnt";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVSTR].Formate = "0.0";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVSTR].MaxLimitValue = 999999d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVSTR].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVDWDWP].Unit = "nm";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDWDWP].Formate = "0.00";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDWDWP].MaxLimitValue = 780.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDWDWP].MinLimitValue = 380.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKA].Unit = "cnt";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKA].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKA].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKA].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKB].Unit = "cnt";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKB].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKB].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKB].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKB].Unit = "cnt";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKB].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKB].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDARKB].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVINTSS].Unit = "ms";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINTSS].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINTSS].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVINTSS].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVEWATT].Unit = "W";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVEWATT].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVEWATT].MaxLimitValue = 10.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVEWATT].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVLE].Unit = "lm/W";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLE].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLE].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVLE].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVWPE].Unit = "%";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWPE].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWPE].MaxLimitValue = 100.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVWPE].MinLimitValue = 0.0d;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVDuv].Unit = "";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDuv].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDuv].MaxLimitValue = 100.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVDuv].MinLimitValue = 0.0d;

			this._msrtResult[(int)ELIVOptiMsrtType.LIVTIMEA].Unit = "ms";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVTIMEA].Formate = "0.000";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVTIMEA].MaxLimitValue = double.MaxValue;
			this._msrtResult[(int)ELIVOptiMsrtType.LIVTIMEA].MinLimitValue = 0.0d;

			this._msrtResult[(int)ELIVOptiMsrtType.LIVTIMEB].Unit = "ms";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVTIMEB].Formate = "0.000";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVTIMEB].MaxLimitValue = double.MaxValue;
			this._msrtResult[(int)ELIVOptiMsrtType.LIVTIMEB].MinLimitValue = 0.0d;

			this._msrtResult[(int)ELIVOptiMsrtType.LIVSETVALUE].Unit = "mA";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVSETVALUE].Formate = "0.000";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVSETVALUE].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ELIVOptiMsrtType.LIVSETVALUE].MinLimitValue = 0.0d;

			this._msrtResult[(int)ELIVOptiMsrtType.LIVWATTTD].Unit = "mW";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVWATTTD].Formate = "0.000";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVWATTTD].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ELIVOptiMsrtType.LIVWATTTD].MinLimitValue = 0.0d;
            //this._msrtResult[(int)ELIVOptiMsrtType.LIVWATTTD].IsVision = true;
            //this._msrtResult[(int)ELIVOptiMsrtType.LIVWATTTD].IsEnable = true;

			this._msrtResult[(int)ELIVOptiMsrtType.LIVLMTD].Unit = "lm";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVLMTD].Formate = "0.000";
			this._msrtResult[(int)ELIVOptiMsrtType.LIVLMTD].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)ELIVOptiMsrtType.LIVLMTD].MinLimitValue = 0.0d;
            //this._msrtResult[(int)ELIVOptiMsrtType.LIVLMTD].IsVision = true;
            //this._msrtResult[(int)ELIVOptiMsrtType.LIVLMTD].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVPDCURRENT].Unit = "uA";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPDCURRENT].Formate = "0.000000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPDCURRENT].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPDCURRENT].MinLimitValue = 0.0d;


            this._msrtResult[(int)ELIVOptiMsrtType.LIVPDWATT].Unit = "mW";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPDWATT].Formate = "0.000000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPDWATT].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVPDWATT].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVPDWATT].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVPDWATT].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVMsrtV].Unit = "V";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVMsrtV].Formate = "0.0000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVMsrtV].MaxLimitValue = 8.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVMsrtV].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVMsrtV].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVMsrtV].IsEnable = true;

            this._msrtResult[(int)ELIVOptiMsrtType.LIVMsrtI].Unit = "mA";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVMsrtI].Formate = "0.000";
            this._msrtResult[(int)ELIVOptiMsrtType.LIVMsrtI].MaxLimitValue = 2000.0d;
            this._msrtResult[(int)ELIVOptiMsrtType.LIVMsrtI].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVMsrtI].IsVision = true;
            this._gainOffsetSetting[(int)ELIVOptiMsrtType.LIVMsrtI].IsEnable = true;

            for (int k = (int)ELIVOptiMsrtType.LIVR01; k < (int)ELIVOptiMsrtType.LIVR15; k++)
            {
                this._msrtResult[k].Unit = "";
                this._msrtResult[k].Formate = "0.0";
                this._msrtResult[k].MaxLimitValue = 100.0d;
                this._msrtResult[k].MinLimitValue = 0.0d;
            }
        }

        #endregion

        #region >>> public Method <<<

        public void LIVApplyParameter()
        {
            if (this._livSweepPoints <= 0)
            {
                return;
            }

            if (this._livLimitModeFixedSITTimer == null)
            {
                this._livLimitModeFixedSITTimer = new PerformanceTimer();
            }

            if (this._livProcessTimer == null)
            {
                this._livProcessTimer = new PerformanceTimer();
            }

            if (this._livFixedSITTimer == null)
            {
                this._livFixedSITTimer = new PerformanceTimer();
            }

            if (this._absoluteSpectrumAarryList == null)
            {
                this._absoluteSpectrumAarryList = new List<double[]>();
            }

            if (this._relativeSpectrumAarryList == null)
            {
                this._relativeSpectrumAarryList = new List<double[]>();
            }

            //==============================================================
            // Sourcemeter setting, electric setting
            //==============================================================
            uint samplingCount = this.LIVSweepPoints;

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

            double[] forceValueArray = null;

            switch (this._livSweepMode)
            {
                case ESweepMode.Linear:
                    {
                        forceValueArray = MPI.Tester.Maths.CoordTransf.LinearScale(this._livStartValue, this._livStopValue, samplingCount).ToArray();
                        break;
                    }
                case ESweepMode.Log:
                    {
                        forceValueArray = MPI.Tester.Maths.CoordTransf.LogScale(this._livStartValue, this._livStopValue, samplingCount).ToArray();
                        break;
                    }
            }

            for (int i = 0; i < samplingCount; i++)
            {
                if (this._livMsrtType == EMsrtType.LIV)
                {
                    this._elecSettingList[i].MsrtType = EMsrtType.LIV;

                    this._elecSettingList[i].ForceUnit = EAmpUnit.mA.ToString();

                    this._elecSettingList[i].MsrtUnit = EVoltUnit.V.ToString();
                }
                else
                {
                    this._elecSettingList[i].MsrtType = EMsrtType.LVI;

                    this._elecSettingList[i].ForceUnit = EVoltUnit.V.ToString();

                    this._elecSettingList[i].MsrtUnit = EAmpUnit.mA.ToString();
                }
                
                this._elecSettingList[i].IsAutoForceRange = false;

                this._elecSettingList[i].ForceRange = this._livForceRange;

                if (i == 0)
                {
                    this._elecSettingList[i].ForceDelayTime = this.LIVForceDelayTime;
                }
                else
                {
                    this._elecSettingList[i].ForceDelayTime = 0;
                }

                this._elecSettingList[i].ForceTime = this._livForceTime;

                this._elecSettingList[i].ForceTimeUnit = ETimeUnit.ms.ToString();

                this._elecSettingList[i].TurnOffTime = this._livTurnOffTime;

                this._elecSettingList[i].ForceValue = forceValueArray[i];

                this._livMsrtRange = this._livMsrtProtection;

                this._elecSettingList[i].SweepStart = this._elecSettingList[0].ForceValue;

                this._elecSettingList[i].SweepStop = this._livForceRange;

                this._elecSettingList[i].MsrtRange = this._livMsrtRange;

                this._elecSettingList[i].MsrtProtection = this._livMsrtProtection;

                this._elecSettingList[i].MsrtFilterCount = this._livMsrtFilterCount;

                this._elecSettingList[i].MsrtNPLC = this._livMsrtNPLC;

                this._elecSettingList[i].IsAutoTurnOff = false;

                if (this._livSensingMode == ESensingMode.Fixed)
                {
                    this._elecSettingList[i].ForceTimeExt = this._livFixIntegralTime;
                }
                else if (this._livSensingMode == ESensingMode.Limit)
                {
                    this._elecSettingList[i].ForceTimeExt = this._livLimitIntegralTime;
                }

                this._elecSettingList[i].IsTrigDetector = this._livIsEnableDetector;

                this._elecSettingList[i].DetectorBiasValue = this._livDetectorBiasVolt;

                this._elecSettingList[i].DetectorMsrtRange = this._livDetectorMsrtRange;

                this._elecSettingList[i].DetectorMsrtNPLC = this._livDetectorMsrtNplc;

				
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
                this._optiSettingList[i].SensingMode = this._livSensingMode;

                this._optiSettingList[i].FixIntegralTime = this._livFixIntegralTime;

                this._optiSettingList[i].LimitIntegralTime = this._livLimitIntegralTime;

                this._optiSettingList[i].TrigDelayTime = this._livTrigDelayTime;
            }

            if (this._livIsLogScale)
            {
                this._livLogScale = MPI.Tester.Maths.CoordTransf.LogScale(this._livLogScaleTime, samplingCount);
            }
        }

        public void LIVLimitModeFixedSITTimerStart()
        {
            if (this._livIsLimitModeFixedSIT)
            {
                this._livLimitModeFixedSITTimer.Start();
            }
        }

        public void LIVActiveLimitModeFixedSIT(int optiSettingIndex)
        {
            if (this._livSensingMode != ESensingMode.Limit)
            {
                return;
            }

            if (!this._livIsLimitModeFixedSIT || optiSettingIndex < 0 || optiSettingIndex >= this._optiSettingList.Count)
            {
                this._livLimitModeFixedSITTimer.Stop();

                return;
            }

            int sleep = (int)(this._optiSettingList[optiSettingIndex].LimitIntegralTime - this._livLimitModeFixedSITTimer.PeekTimeSpan(ETimeSpanUnit.MilliSecond));

            if (sleep > 0)
            {
                System.Threading.Thread.Sleep(sleep);
            }

            this._livLimitModeFixedSITTimer.Stop();
        }

        public void LIVFixedSITTimerStart()
        {
            if (this._livIsLifeTest)
            {
                this._livFixedSITTimer.Start();
            }
        }

        public void LIVActiveFixedSIT(int optiSettingIndex)
        {
            if (!this._livIsLifeTest || optiSettingIndex < 0 || optiSettingIndex >= this._optiSettingList.Count)
            {
                this._livFixedSITTimer.Stop();

                return;
            }

            int sleep = 0;

            if (this._livIsLogScale)
            {
                double def = 0.0d;

                if (optiSettingIndex > 0)
                {
                    def = this._livLogScale[optiSettingIndex] - this._livLogScale[optiSettingIndex - 1];
                }

                sleep = (int)(def - this._livFixedSITTimer.PeekTimeSpan(ETimeSpanUnit.MilliSecond));
            }
            else
            {
                sleep = (int)((double)this._livSamplimgTime - this._livFixedSITTimer.PeekTimeSpan(ETimeSpanUnit.MilliSecond));
            }

            if (sleep > 0)
            {
                System.Threading.Thread.Sleep(sleep);
            }

            this._livFixedSITTimer.Stop();
        }

        public void LIVProcessStart()
        {
            this._livProcessTimer.Start();
        }

        public void LIVProcessEnd()
        {
            this._livProcessTimer.Stop();
        }

        public double LIVProcessTime()
        {
            return this._livProcessTimer.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
        }

        public void ClearDataList()
        {
            foreach (var item in this.MsrtResult)
            {
                item.DataList.Clear();
            }

            this._absoluteSpectrumAarryList.Clear();

            this._relativeSpectrumAarryList.Clear();
        }

        #endregion
    }
}
