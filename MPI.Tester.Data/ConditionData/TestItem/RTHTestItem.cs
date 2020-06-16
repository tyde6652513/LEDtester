using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class RTHTestItem : TestItemData
	{
		public RTHTestItem()
			: base()
        {
            this._lockObj = new object();

            this._type = ETestType.RTH;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };

            this._elecSetting[0].MsrtType = EMsrtType.RTH;

			this._elecSetting[0].IsAutoTurnOff = true;

            // Tested Result Setting
            this._msrtResult = new TestResultData[] { new TestResultData("V", "0.000000"), new TestResultData("V", "0.000000"), new TestResultData("V", "0.000000") };
            this._msrtResult[0].MinLimitValue = 0.0d;
            this._msrtResult[0].MaxLimitValue = 8.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 8.0d;

            this._msrtResult[1].MinLimitValue = 0.0d;
            this._msrtResult[1].MaxLimitValue = 8.0d;
            this._msrtResult[1].MinLimitValue2 = 0.0d;
            this._msrtResult[1].MaxLimitValue2 = 8.0d;

            this._msrtResult[2].MinLimitValue = 0.0d;
            this._msrtResult[2].MaxLimitValue = 8.0d;
            this._msrtResult[2].MinLimitValue2 = 0.0d;
            this._msrtResult[2].MaxLimitValue2 = 8.0d;

            // No Gain Offset Setting
            this._gainOffsetSetting = null;

            this.ResetKeyName();
        }

		#region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

			int num = this._subItemIndex + 1;                       // 0-base
			//int index = 1 + 3 * this._subItemIndex;

            // Reset Electrical Setting KeyName
            this._elecSetting[0].KeyName = "RTHIF_" + num.ToString();
			this._elecSetting[0].Name = "RTHIF" + num.ToString("D2");
   
            // Reset Tested Result KeyName
			this._msrtResult[0].KeyName = "MRTHA_" + num.ToString();
			this._msrtResult[0].Name = "MRTHA" + num.ToString("D2");

            this._msrtResult[1].KeyName = "MRTHB_" + num.ToString();
            this._msrtResult[1].Name = "MRTHB" + num.ToString("D2");

            this._msrtResult[2].KeyName = "MRTHC_" + num.ToString();
            this._msrtResult[2].Name = "MRTHC" + num.ToString("D2");

            SetMsrtNameAsKey();
        }

        #endregion
	}
}
