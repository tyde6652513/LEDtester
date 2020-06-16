using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class PolarTestItem : TestItemData
	{
        private double _thresholdVoltage;

		public PolarTestItem()
			: base()
        {
            this._lockObj = new object();
          
            this._type = ETestType.POLAR;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.POLAR;
            this._elecSetting[0].MsrtRange = 8;
            this._elecSetting[0].MsrtProtection = 8.0d;
            // Tested Result Setting
            this._msrtResult = new TestResultData[] { new TestResultData("V", "0.000")};
            this._msrtResult[0].MinLimitValue = 0.0d;
            this._msrtResult[0].MaxLimitValue = 8.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 8.0d;

            this._thresholdVoltage = 0.0d;

            // Gain Offset Setting
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
            this._msrtResult[0].KeyName = "PLMVF_" + num.ToString();
            this._msrtResult[0].Name = "PLMVF" + num.ToString("D2");

            SetMsrtNameAsKey();
            this._msrtResult[0].IsEnable = true;
            this._msrtResult[0].IsVision = true;
        }

        #endregion
	}
}
