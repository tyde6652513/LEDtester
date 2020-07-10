using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class VISweepTestItem : TestItemData
	{
		private const uint MIN_SWEEP_COUNT = 2;

        private List<SweepInfo> _swList = new List<SweepInfo>();

		public VISweepTestItem() : base()
		{
            this._lockObj = new object();
       
            this._type = ETestType.VISWEEP;

            this._elecSetting = new ElectSettingData[] { new ElectSettingData("V", "uA", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FVMISWEEP;
            this._elecSetting[0].IsAutoTurnOff = true;

            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.GainAndOffest)};      
            
			// Tested Result Setting
			this._msrtResult = new TestResultData[] { new TestResultData("V", "E4")};//求PD光電流的轉折電壓
            IsCustomerizeSweepMode = false;
            IsCalcVp = false;
            dIp = 0.1;
        }

        #region >>> Protected Methods <<<

        protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            this._elecSetting[0].KeyName = this.KeyName;

            if (this._msrtResult == null || this._msrtResult.Length < 4)
            {
                this._msrtResult = new TestResultData[] { new TestResultData("V", "E4") };
            }
            
            this._msrtResult[0].KeyName = "VISWVP" + "_" + num.ToString();
            
            SetMsrtNameAsKey();

            this.GainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
            this.GainOffsetSetting[0].Name = this._msrtResult[0].Name;
        }

        #endregion

        #region>>public property<<

        [XmlIgnore]
        public List<SweepInfo> SweepInfoList
        {
            set
            {
                _swList = value;
                
            }
            get { return _swList; }
        }

        public SweepInfo[] SweepInfoArray
        {
            set { //_swList = value;
                SweepInfoList = new List<SweepInfo>();
                if (value != null)
                {
                    SweepInfoList.AddRange(value);
                }
            }
            get { return SweepInfoList.ToArray(); }
        }

        public bool IsCustomerizeSweepMode { set; get; }

        public bool IsCalcVp { get; set; }

        public double dIp { get; set; }//in uA
        #endregion

        #region >>public method<<
        public void RefreshElecSetting()
        {
            if (_swList != null && _swList.Count > 0)
            {
                ElecSetting[0].ElecTerminalSetting = null;
                //List<ElecTerminalSetting> etsList = new List<ElecTerminalSetting>();
                List<ElectSettingData> eleList = new List<ElectSettingData>();

                foreach (var sinfo in _swList)
                {
                    ElectSettingData ets = new ElectSettingData();
                    ets.MsrtType = EMsrtType.FVMISWEEP;
                    ets.SweepMode = sinfo.Mode;

                    ets.SweepStart = sinfo.StartValue;
                    ets.SweepStop = sinfo.EndValue;
                    ets.SweepRiseCount = (uint)sinfo.Cnt;
                    ets.ForceUnit = sinfo.ForceUnit;
                    ets.MsrtProtection = sinfo.Clamp;
                    ets.MsrtRange = sinfo.Clamp;
                    ets.MsrtNPLC = sinfo.NPLC;
                    ets.ForceTime = sinfo.ForceTime;
                    ets.SweepTurnOffTime = sinfo.OffTime;
                    ets.SweepContCount = 0;
                    ets.IsAutoMsrtRange = sinfo.AutoMsrtRange;
                    ets.IsSweepAutoMsrtRange = sinfo.AutoMsrtRange;
                    ets.MsrtUnit = sinfo.MsrtUnit;
                    ets.ForceTimeUnit = sinfo.TimeUnit;
                    ets.ForceValue = ets.SweepStart;

                    double asVal = Math.Abs(ets.SweepStart);
                    double aeVal = Math.Abs(ets.SweepStop);
                    double srange = Math.Max(asVal, aeVal);
                    ets.ForceRange = srange;
                    ets.IsSweepFirstElec = false;
                    ets.IsSweepEnd = false;

                    double uFactor = 1 / MPI.Tester.Maths.UnitMath.UnitConvertFactor(EVoltUnit.V, ets.ForceUnit);

                    switch (ets.SweepMode)
                    {
                        case ESweepMode.Linear:
                            {
                                ets.SweepCustomValue = MPI.Tester.Maths.CoordTransf.LinearScale(ets.SweepStart * uFactor
                                    , ets.SweepStop * uFactor, ets.SweepRiseCount).ToArray();
                                break;
                            }
                        case ESweepMode.Log:
                            {
                                ets.SweepCustomValue = MPI.Tester.Maths.CoordTransf.LogScale(ets.SweepStart * uFactor
                                    , ets.SweepStop * uFactor, ets.SweepRiseCount).ToArray();
                                break;
                            }
                    }
                    ets.IsSweepFirstElec = true;

                    eleList.Add(ets);
                }
                if (eleList.Count > 0)
                {
                    eleList[0].IsSweepFirstElec = true;

                    eleList[eleList.Count - 1].IsSweepEnd = true;
                }
                _elecSetting = eleList.ToArray();
            }
        }
        #endregion
    }
    [Serializable]
    public class SweepInfo
    {
        public SweepInfo()
        {
            StartValue = 0;
            EndValue = 0;
            Cnt = 2;
            NPLC = 0.01;
            Clamp = 1;
            ForceTime = 1;
            OffTime = 0;
            AutoMsrtRange = false;
            Mode = ESweepMode.Linear;
            ForceUnit = "V";
            MsrtUnit = "mA";
            ForceDelayTime = 0;

        }
        public double StartValue { set; get; }
        public double EndValue { set; get; }
        public int Cnt { set; get; }
        public double NPLC { set; get; }
        public double Clamp { set; get; }
        public double ForceTime { set; get; }
        public double OffTime { set; get; }
        public bool AutoMsrtRange { set; get; }

        public ESweepMode Mode { set; get; }

        public string ForceUnit { set;get; }
        public string MsrtUnit { set; get; }
        public string TimeUnit { set; get; }
        public double ForceDelayTime { set; get; }
    }
}
