using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class LaserSourceTestItem : TestItemData
    {
        public LaserSourceTestItem()
            : base()
        {
            this._lockObj = new object();
            this._type = ETestType.LaserSource;
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("V", "uA", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FVMI;
            this._elecSetting[0].MsrtProtection = 4.0d;

            //// Tested Result Setting
            this._msrtResult = new TestResultData[] { new TestResultData("A", "E6"), new TestResultData("W", "E6"), new TestResultData("dBm", "0.0") };
            this._msrtResult[0].IsVision = true;
            this._msrtResult[0].IsEnable = true;
            //// Gain Offset Setting
            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.GainAndOffest), new GainOffsetData(true, EGainOffsetType.GainAndOffest),
                new GainOffsetData(true, EGainOffsetType.GainAndOffest) };
            this._gainOffsetSetting[0].IsEnable = true;
            this.ResetKeyName();
            LaserSourceSet = new LaserSourceSysSettingData();

        }
        #region >>public property<<
        public LaserSourceSysSettingData LaserSourceSet{get;set;}
        #endregion

        #region  >>protected method<<
        protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            // Reset Electrical Setting KeyName
            this._elecSetting[0].KeyName = this.KeyName;

            // Reset Tested Result KeyName
            this._msrtResult[0].KeyName = "MPDIr_" + num.ToString();
            this._msrtResult[0].Name = "PDIr" + num.ToString("D2");
            this._msrtResult[0].Volatile = false;

            this._msrtResult[1].KeyName = "MPDPow_" + num.ToString();
            this._msrtResult[1].Name = "PDPow" + num.ToString("D2");
            this._msrtResult[1].Volatile = false;

            this._msrtResult[2].KeyName = "AttPower_" + num.ToString();
            this._msrtResult[2].Name = "AttPower" + num.ToString("D2");
            this._msrtResult[2].Volatile = false;
            this._msrtResult[2].Unit = "dBm";
            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName

            if (GainOffsetSetting == null || GainOffsetSetting.Length != _msrtResult.Length)
            {
                GainOffsetSetting = new GainOffsetData[_msrtResult.Length];
            }

            for (int i = 0; i < _msrtResult.Length; ++i)
            {
                if (GainOffsetSetting[i] == null)
                {
                    GainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.GainAndOffest);
                }
                this.GainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;
                this.GainOffsetSetting[i].Name = this._msrtResult[i].Name;
            }
        }
        #endregion
        #region  >>public method<<
        public override object Clone()
        {
            LaserSourceSysSettingData lsd = LaserSourceSet.Clone() as LaserSourceSysSettingData;
            LaserSourceSet = null;

            LaserSourceTestItem obj = this.MemberwiseClone() as LaserSourceTestItem;

            LaserSourceSet = lsd.Clone() as LaserSourceSysSettingData;
            obj.LaserSourceSet = lsd.Clone() as LaserSourceSysSettingData;
            
            return obj;
        }
        #endregion
    }
}
