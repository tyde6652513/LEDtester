using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
    public class IFHTestItem : TestItemData
    {
        public IFHTestItem() : base()
        {
            this._lockObj = new object();
           
            this._type = ETestType.IFH;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FI;

            // No Tested Result Setting and No Gain Offset setting
            this._msrtResult = null; 
            this._gainOffsetSetting = null;           

            this.ResetKeyName();
        }

        #region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            // Reset Electrical Setting KeyName
            this._elecSetting[0].KeyName = this.KeyName;
        }

        #endregion 
    }
}
