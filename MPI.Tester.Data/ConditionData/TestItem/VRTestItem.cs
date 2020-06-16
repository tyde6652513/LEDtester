using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
    public class VRTestItem : TestItemData
    {
        private bool _isUseVzAsForceValue;
        private string _refVzKeyName;
        private string _refVzName;
        private double _factor;
        private double _offset;

        public VRTestItem() : base()
        {
            this._lockObj = new object();

            this._type = ETestType.VR;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("V", "uA", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FVMI;
            this._elecSetting[0].MsrtProtection = 4.0d;
            // Tested Result Setting
			this._msrtResult = new TestResultData[] { new TestResultData("uA", "0.000"), new TestResultData("V", "0.000")  };
			this._msrtResult[0].MinLimitValue = 0.0d;
			this._msrtResult[0].MaxLimitValue = 40.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 40.0d;

            this._msrtResult[1].MinLimitValue = 0.0d;
            this._msrtResult[1].MaxLimitValue = 10.0d;
            this._msrtResult[1].MinLimitValue2 = 0.0d;
            this._msrtResult[1].MaxLimitValue2 = 10.0d;

            // Gain Offset Setting
            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset), new GainOffsetData(true, EGainOffsetType.None) };

            this._factor = 1.0d;
            this._offset = 0.0d;
            this.ResetKeyName();
        }

        #region >>> Public Property <<<

        public bool IsUseVzAsForceValue
        {
            get { return this._isUseVzAsForceValue; }
            set { lock (this._lockObj) { this._isUseVzAsForceValue = value; } }
        }

        public string RefVzKeyName
        {
            get { return this._refVzKeyName; }
            set { lock (this._lockObj) { this._refVzKeyName = value; } }
        }

        public string RefVzName
        {
            get { return this._refVzName; }
            set { lock (this._lockObj) { this._refVzName = value; } }
        }

        public double Factor
        {
            get { return this._factor; }
            set { lock (this._lockObj) { this._factor = value; } }
        }

        public double Offset
        {
            get { return this._offset; }
            set { lock (this._lockObj) { this._offset = value; } }
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
            this._msrtResult[0].KeyName = "MIR_" + num.ToString();
			this._msrtResult[0].Name = "MIR" + num.ToString("D2");

            this._msrtResult[1].KeyName = "MIRSV_" + num.ToString();
            this._msrtResult[1].Name = "MIRSV" + num.ToString("D2");
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
