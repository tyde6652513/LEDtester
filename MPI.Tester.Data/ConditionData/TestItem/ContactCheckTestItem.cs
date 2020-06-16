using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class ContactCheckTestItem : TestItemData
	{
		public ContactCheckTestItem()
			: base()
        {
            this._lockObj = new object();
          
            this._type = ETestType.ContactCheck;

			// Electrical Setting 
			this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
			this._elecSetting[0].MsrtType = EMsrtType.CONTACTCHECK;
			this._elecSetting[0].MsrtRange = 8;
			this._elecSetting[0].MsrtProtection = 8.0d;

            // Electrical Setting 
            // Tested Result Setting
			this._msrtResult = new TestResultData[] { new TestResultData("Ohm", "0.000"), new TestResultData("Ohm", "0.000"), new TestResultData("", "0") };
			this._msrtResult[0].MinLimitValue = 0.0d;
			this._msrtResult[0].MaxLimitValue = 99999.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 99999.0d;
            this._msrtResult[1].MinLimitValue = 0.0d;
            this._msrtResult[1].MaxLimitValue = 99999.0d;
            this._msrtResult[1].MinLimitValue2 = 0.0d;
            this._msrtResult[1].MaxLimitValue2 = 99999.0d;
            this._msrtResult[2].MinLimitValue = 1.0d;
            this._msrtResult[2].MaxLimitValue = 2.0d;
            this._msrtResult[2].MinLimitValue2 = 1.0d;
            this._msrtResult[2].MaxLimitValue2 = 2.0d;

            // Gain Offset Setting
            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset), new GainOffsetData(true, EGainOffsetType.Offset), new GainOffsetData(true, EGainOffsetType.Offset) };
            this.ResetKeyName();            
        }

        #region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            // Reset Tested Result KeyName
            this._msrtResult[0].KeyName = "MRH_" + num.ToString();
            this._msrtResult[0].Name = "Rh" + num.ToString("D2");

            this._msrtResult[1].KeyName = "MRL_" + num.ToString();
            this._msrtResult[1].Name = "MRl" + num.ToString("D2");

			this._msrtResult[2].KeyName = "CP_" + num.ToString();
			this._msrtResult[2].Name = "CP" + num.ToString("D2");

            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName
			this.GainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
			this.GainOffsetSetting[0].Name = this._msrtResult[0].Name;

            this.GainOffsetSetting[1].KeyName = this._msrtResult[1].KeyName;
            this.GainOffsetSetting[1].Name = this._msrtResult[1].Name;

            this.GainOffsetSetting[2].KeyName = this._msrtResult[2].KeyName;
            this.GainOffsetSetting[2].Name = this._msrtResult[2].Name;
        }

        #endregion
	}
}
