using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
    public class VFTestItem : TestItemData
    {
        private bool _isEnableRetest;
        private uint _retestCount;
        private double _retestThresholdV;
        
        public VFTestItem()
			: base()
        {
            this._lockObj = new object();

            this._type = ETestType.VF;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("V", "mA", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FVMI;

            // Tested Result Setting
			this._msrtResult = new TestResultData[] { new TestResultData("mA", "0.000"), new TestResultData("V", "0.000") };
			this._msrtResult[0].MinLimitValue = 0.0d;
			this._msrtResult[0].MaxLimitValue = 100.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 100.0d;

            this._msrtResult[1].MinLimitValue = 0.0d;
            this._msrtResult[1].MaxLimitValue = 8.0d;
            this._msrtResult[1].MinLimitValue2 = 0.0d;
            this._msrtResult[1].MaxLimitValue2 = 8.0d;

            // Gain Offset Setting
            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset), new GainOffsetData(true, EGainOffsetType.None) };

            this._isEnableRetest = false;
            this._retestCount = 1;
            this._retestThresholdV = 8.0d;

            this.ResetKeyName();
        }

        #region >>> Public Property <<<

        public bool IsEnableRetest
        {
            get { return this._isEnableRetest; }
            set { lock (this._lockObj) { this._isEnableRetest = value; } }
        }

        public uint RetestCount
        {
            get
            {
                if (this._retestCount == 0)
                {
                    this._retestCount = 1;
                }
                
                return this._retestCount; 
            }
            set { lock (this._lockObj) { this._retestCount = value; } }
        }

        public double RetestThresholdV
        {
            get { return this._retestThresholdV; }
            set { lock (this._lockObj) { this._retestThresholdV = value; } }
        }

        #endregion

        #region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            // Reset Electrical Setting KeyName
            this._elecSetting[0].KeyName = this.KeyName;

            // Reset Tested Result KeyName
            this._msrtResult[0].KeyName = "MIF_" + num.ToString();
			this._msrtResult[0].Name = "MIF" + num.ToString("D2");

            this._msrtResult[1].KeyName = "MIFSV_" + num.ToString();
            this._msrtResult[1].Name = "MIFSV" + num.ToString("D2");
            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName
			this.GainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
			this.GainOffsetSetting[0].Name = this._msrtResult[0].Name;

            this.GainOffsetSetting[1].KeyName = this._msrtResult[1].KeyName;
            this.GainOffsetSetting[1].Name = this._msrtResult[1].Name;
        }


        #endregion

    }
}
