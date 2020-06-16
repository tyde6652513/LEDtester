using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
    public class IFTestItem : TestItemData
    {
        public IFTestItem() : base()
        {
            this._lockObj = new object();
          
            this._type = ETestType.IF;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FIMV;
            this._elecSetting[0].MsrtRange = 8;
            this._elecSetting[0].MsrtProtection = 8.0d;
            // Tested Result Setting
            this._msrtResult = new TestResultData[] { new TestResultData("V", "0.000"), new TestResultData("mA", "0.000") };
			this._msrtResult[0].MinLimitValue = 0.0d;
			this._msrtResult[0].MaxLimitValue = 8.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 8.0d;

            this._msrtResult[1].MinLimitValue = 0.0d;
            this._msrtResult[1].MaxLimitValue = 100.0d;
            this._msrtResult[1].MinLimitValue2 = 0.0d;
            this._msrtResult[1].MaxLimitValue2 = 100.0d;

            // Gain Offset Setting
            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset), new GainOffsetData(true, EGainOffsetType.None) };
            this.ResetKeyName();
        }

        #region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            // Reset Electrical Setting KeyName
            this._elecSetting[0].KeyName = this.KeyName;

            // Reset Tested Result KeyName
            this._msrtResult[0].KeyName = "MVF_" + num.ToString();
            this._msrtResult[0].Name = this._msrtResult[0].KeyName;

            this._msrtResult[1].KeyName = "MVFSI_" + num.ToString();
            this._msrtResult[1].Name = this._msrtResult[0].KeyName;

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
