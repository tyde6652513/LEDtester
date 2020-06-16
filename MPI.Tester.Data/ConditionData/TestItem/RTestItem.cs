using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class RTestItem : TestItemData
    {      
        public RTestItem() : base()
        {
            this._lockObj = new object();
          
            this._type = ETestType.R;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.R;
            this._elecSetting[0].MsrtRange = 8;
            this._elecSetting[0].MsrtProtection = 8.0d;

            // Tested Result Setting
			this._msrtResult = new TestResultData[] { new TestResultData("Ohm", "0.000")};
			this._msrtResult[0].MinLimitValue = 0.0d;
			this._msrtResult[0].MaxLimitValue = 99999.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 99999.0d;

            // Gain Offset Setting
			this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset)};
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
            this._msrtResult[0].KeyName = "MR_" + num.ToString();
			this._msrtResult[0].Name = "R" + num.ToString("D2");
            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName
			this.GainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
			this.GainOffsetSetting[0].Name = this._msrtResult[0].Name;
        }

        #endregion
    }
}
