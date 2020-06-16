using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class ESDTestItem : TestItemData
    {
        private ESDSettingData _esdSetting;
        private bool _isEnableJudgeItem;

		public ESDTestItem()
			: base()
        {    
            this._lockObj = new object();
           
            this._type = ETestType.ESD;

			// Electrical Setting 
			this._elecSetting = new ElectSettingData[] { new ElectSettingData("V", "uA", "ms") };
			this._elecSetting[0].MsrtType = EMsrtType.FVMI;
			this._elecSetting[0].MsrtProtection = 4.0d;
            // Tested Result Setting
            this._msrtResult = new TestResultData[] { new TestResultData("uA", "0.000"),  // Judge IR
                                                      new TestResultData("V", "0"),       // ESD
                                                      new TestResultData("V", "0") };     // Final

			this._msrtResult[0].MinLimitValue = 0.0d;
			this._msrtResult[0].MaxLimitValue = 40.0d;
			this._msrtResult[0].MinLimitValue2 = 0.0d;
			this._msrtResult[0].MaxLimitValue2 = 40.0d;

            // Gain Offset Setting      
			this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset) };

			// ESD Setting
			this._esdSetting = new ESDSettingData();
                         
            this._isEnableJudgeItem = false;
        }

		#region >>> Public Property <<<

		public ESDSettingData EsdSetting
		{
			get { return this._esdSetting; }
			set { lock (this._lockObj) { this._esdSetting = value; } }
		}

        public bool IsEnableJudgeItem
                    {
            get { return this._isEnableJudgeItem; }
            set { lock (this._lockObj) { this._isEnableJudgeItem = value; } }
        }

		#endregion

		#region >>> Proteced Method <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            // Reset Electrical Setting KeyName
            this._esdSetting.KeyName = this.KeyName;

            // Reset Tested Result KeyName
            // Judge IR
			this._msrtResult[0].KeyName = "MIRJ_" + num.ToString();
            this._msrtResult[0].Name = this._msrtResult[0].KeyName;
            // ESD
            this._msrtResult[1].KeyName = "MESD_" + num.ToString();
            this._msrtResult[1].Name = this._msrtResult[1].KeyName;
            // Final
            this._msrtResult[2].KeyName = "MESDF_" + num.ToString();
            this._msrtResult[2].Name = this._msrtResult[2].KeyName;

            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName
            // Judge IR
            this.GainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
            this.GainOffsetSetting[0].Name = this._msrtResult[0].Name;
        }

        #endregion
    }
}
