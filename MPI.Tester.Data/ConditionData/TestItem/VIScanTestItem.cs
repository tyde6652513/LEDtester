using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class VIScanTestItem : TestItemData
    {
        public VIScanTestItem() : base()
        {
            this._lockObj = new object();

            this._type = ETestType.VISCAN;

            this._elecSetting = new ElectSettingData[] { new ElectSettingData("V", "uA", "ms") };

            this._elecSetting[0].MsrtType = EMsrtType.FVMISCAN;

            // Tested Result Setting
            this._msrtResult = new TestResultData[] {   new TestResultData("uA", "0.000000000"),
														new TestResultData("uA", "0.000000000"),
														new TestResultData("uA", "0.000000000"),
                                                        new TestResultData("uA", "0.000000000"),
													};

            // Gain Offset Setting
            //this._gainOffsetSetting = null;

            this._gainOffsetSetting = new GainOffsetData[] {  new GainOffsetData(false, EGainOffsetType.None), //MSCANIPEAK_
                                                            new GainOffsetData(false, EGainOffsetType.None),//MSCANISTABLE_
                                                            new GainOffsetData(false, EGainOffsetType.None),  // MSCANIDIFF_
                                                            new GainOffsetData(false, EGainOffsetType.None)};   // MSETTLING_

            // Then reset those keyname of this._msrtResult[] Array
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
            this._msrtResult[0].KeyName = "MSCANIPEAK_" + num.ToString();
            this._msrtResult[0].Name = "IPeak" + num.ToString("D2");

            this._msrtResult[1].KeyName = "MSCANISTABLE_" + num.ToString();
            this._msrtResult[1].Name = "IStable" + num.ToString("D2");

            this._msrtResult[2].KeyName = "MSCANIDIFF_" + num.ToString();
            this._msrtResult[2].Name = "IDiff" + num.ToString("D2");

            this._msrtResult[3].KeyName = "MSETTLING_" + num.ToString();
            this._msrtResult[3].Name = "Tstable" + num.ToString("D2");

            for (int i = 0; i < this._msrtResult.Length; ++i)
            {
                this.GainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;
                this.GainOffsetSetting[i].Name = this._msrtResult[i].Name;
            }
        }

        #endregion
    }
}
