using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class IOTestItem : TestItemData
    {
        //private IOSettingData _ioSetting;

        public IOTestItem()
            : base()
        {
            this._lockObj = new object();

            this._type = ETestType.IO;

            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.IO;
            this._elecSetting[0].MsrtRange = 8;
            this._elecSetting[0].MsrtProtection = 8.0d;
            this._msrtResult = new TestResultData[] { new TestResultData("V", "0.000") };//, new TestResultData("mA", "0.000") 
            this._msrtResult[0].MinLimitValue = 0.0d;
            this._msrtResult[0].MaxLimitValue = 8.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 8.0d;
            this._msrtResult[0].IsEnable = false;
            this._msrtResult[0].IsVision = false;

            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset) };

            this.ResetKeyName();

            this._elecSetting[0].IOSetting = new IOSetting();
        }

        #region

        public IOSetting IOSetData
        {
            get { return this._elecSetting[0].IOSetting; }
            set { lock (_lockObj) { this._elecSetting[0].IOSetting = value; } }		
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
            this._msrtResult[0].KeyName = "MIOVF_" + num.ToString();
            this._msrtResult[0].Name = this._msrtResult[0].KeyName;

            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName
            this.GainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
            this.GainOffsetSetting[0].Name = this._msrtResult[0].Name;

        }

        #endregion



    }
}