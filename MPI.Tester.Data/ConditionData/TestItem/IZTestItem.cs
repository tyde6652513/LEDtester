using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
    public class IZTestItem : TestItemData
    {
        private bool _isUseIrAsForceValue;
        private string _refIrKeyName;
        private string _refIrName;
        private double _factor;
        private double _offset;

        public IZTestItem() : base()
        {
            this._lockObj = new object();

            this._type = ETestType.IZ;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("uA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FIMV;

            this._elecSetting[0].MsrtProtection = 40;
            this._elecSetting[0].MsrtRange = 50;      

            // Tested Result Setting
            this._msrtResult = new TestResultData[] { new TestResultData("V", "0.000"), new TestResultData("uA", "0.000") };

			this._msrtResult[0].MinLimitValue = 0.0d;
			this._msrtResult[0].MaxLimitValue = 50.0d;
            this._msrtResult[0].MinLimitValue2 = 0.0d;
            this._msrtResult[0].MaxLimitValue2 = 50.0d;

            this._msrtResult[1].MinLimitValue = 0.0d;
            this._msrtResult[1].MaxLimitValue = 100.0d;
            this._msrtResult[1].MinLimitValue2 = 0.0d;
            this._msrtResult[1].MaxLimitValue2 = 100.0d;

            // Gain Offset Setting
            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset), new GainOffsetData(true, EGainOffsetType.None) };
            this.ResetKeyName();
        }
        #region >>> Public Property <<<

        public bool IsUseIrAsForceValue
        {
            get { return this._isUseIrAsForceValue; }
            set { lock (this._lockObj) { this._isUseIrAsForceValue = value; } }
        }

        public string RefIrKeyName
        {
            get { return this._refIrKeyName; }
            set { lock (this._lockObj) { this._refIrKeyName = value; } }
        }

        public string RefIrName
        {
            get { return this._refIrName; }
            set { lock (this._lockObj) { this._refIrName = value; } }
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
            this._msrtResult[0].KeyName = "MVZ_" + num.ToString();
			this._msrtResult[0].Name = "MVZ" + num.ToString("D2");

            this._msrtResult[1].KeyName = "MVZSI_" + num.ToString();
            this._msrtResult[1].Name = "MVZSI" + num.ToString("D2");

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
