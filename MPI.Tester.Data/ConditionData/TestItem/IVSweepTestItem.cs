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
    public class IVSweepTestItem : TestItemData
    {
        private List<SweepInfo> _swList = new List<SweepInfo>();

        public IVSweepTestItem() : base()
        {
            this._lockObj = new object();
       
            this._type = ETestType.IVSWEEP;

            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FIMVSWEEP;
            this._elecSetting[0].IsAutoTurnOff = true;

            this._gainOffsetSetting = null;           
            
            // Tested Result Setting
            this._msrtResult = new TestResultData[] {  new TestResultData("V", "0.000"),
                                                       new TestResultData("V", "0.000"),
                                                       new TestResultData("V", "0.000")};                                                        

            // Then reset those keyname of this._msrtResult[] Array
            this.ResetKeyName();
        }
        
        #region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            this._elecSetting[0].KeyName = this.KeyName;

			this._msrtResult[0].KeyName = "VPEAK" + "_" + num.ToString();
			this._msrtResult[1].KeyName = "VSTABLE" + "_" + num.ToString();
			this._msrtResult[2].KeyName = "VDIFF" + "_" + num.ToString();

            SetMsrtNameAsKey();            
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
            set
            { //_swList = value;
                SweepInfoList = new List<SweepInfo>();
                if (value != null)
                {
                    SweepInfoList.AddRange(value);
                }
            }
            get { return SweepInfoList.ToArray(); }
        }

        public bool IsCustomerizeSweepMode { set; get; }

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
                    ets.MsrtType = EMsrtType.FIMVSWEEP;
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
}
