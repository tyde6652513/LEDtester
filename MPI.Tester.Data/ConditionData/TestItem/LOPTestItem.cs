using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
    public class LOPTestItem : TestItemData
    {
        private uint _ampGainPow;

        public LOPTestItem() : base()
        {
            this._lockObj = new object();
                   
            this._type = ETestType.LOP;

            this._ampGainPow = 0;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };

            this._elecSetting[0].MsrtType = EMsrtType.FIMVLOP;
            this._elecSetting[0].IsAutoTurnOff = true;


            // Tested Result Setting
            this._msrtResult = new TestResultData[] { new TestResultData("V", "0.000"), 
                                                      new TestResultData("mA", "0.0000"),
                                                      // Detector01
                                                      new TestResultData("uA", "0.0000"),
                                                      new TestResultData("cnt", "0"),
													  new TestResultData("mcd", "0.0000"),
													  new TestResultData("mW", "0.0000"), 
                                                       // Detector02
                                                      new TestResultData("uA", "0.0000"),
                                                      new TestResultData("cnt", "0"),
													  new TestResultData("mcd", "0.0000"),
													  new TestResultData("mW", "0.0000")};

            this._msrtResult[0].MinLimitValue = 0.0d;
            this._msrtResult[0].MaxLimitValue = 8.0d;

            this._msrtResult[1].MinLimitValue = 0.0d;
            this._msrtResult[1].MaxLimitValue = 100.0d;

            // Detector01
            this._msrtResult[2].MinLimitValue = 0.0d;
            this._msrtResult[2].MaxLimitValue = 100.0d;

            this._msrtResult[3].MinLimitValue = 0.0d;
            this._msrtResult[3].MaxLimitValue = 1.000d;

            this._msrtResult[4].MinLimitValue = 0.0d;
            this._msrtResult[4].MaxLimitValue = 9999.999d;

            this._msrtResult[5].MinLimitValue = 0.0d;
            this._msrtResult[5].MaxLimitValue = 9999.999d;

            // Detector02
            this._msrtResult[6].MinLimitValue = 0.0d;
            this._msrtResult[6].MaxLimitValue = 100.0d;

            this._msrtResult[7].MinLimitValue = 0.0d;
            this._msrtResult[7].MaxLimitValue = 1.000d;

            this._msrtResult[8].MinLimitValue = 0.0d;
            this._msrtResult[8].MaxLimitValue = 9999.999d;

            this._msrtResult[9].MinLimitValue = 0.0d;
            this._msrtResult[9].MaxLimitValue = 9999.999d;

            // Gain Offset Setting
           this._gainOffsetSetting= new GainOffsetData[] {  new GainOffsetData(true, EGainOffsetType.Offset), 
                                                            new GainOffsetData(true, EGainOffsetType.Offset),
                                                            // Detector01
                                                            new GainOffsetData(true, EGainOffsetType.Offset),  // PDCURRENT
                                                            new GainOffsetData(false, EGainOffsetType.None),   // PDCNT
															new GainOffsetData(true, EGainOffsetType.Gain),    // PDMCD
														    new GainOffsetData(true, EGainOffsetType.Gain),    // PDWATT
                                                            // Detector02
                                                            new GainOffsetData(true, EGainOffsetType.Offset),  // PDCURRENTb
                                                            new GainOffsetData(false, EGainOffsetType.None),   // PDCNTb
															new GainOffsetData(true, EGainOffsetType.Gain),    // PDMCDb
														    new GainOffsetData(true, EGainOffsetType.Gain)};   // PDWATTb
         
            this.ResetKeyName();
        }

        #region >>> Public Property <<<

        public uint AmpGainPower
        { 
            get { return this._ampGainPow; }
            set { lock (this._lockObj) { this._ampGainPow = value; } }      
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
            this._msrtResult[0].KeyName = "PDMVF_" + num.ToString();
            this._msrtResult[0].Name = "PDMVF" + num.ToString("D2");

            this._msrtResult[1].KeyName = "PDMIF_" + num.ToString();
            this._msrtResult[1].Name = "PDMIF" + num.ToString("D2");

            //----------------------------------------------------------------------------------------
            // Detector01
            this._msrtResult[2].KeyName = "PDCURRENT_" + num.ToString();
            this._msrtResult[2].Name = "PDCURRENT" + num.ToString("D2");

            this._msrtResult[3].KeyName = "PDCNT_" + num.ToString();
            this._msrtResult[3].Name = "PDCNT" + num.ToString("D2");

            this._msrtResult[4].KeyName = "PDMCD_" + num.ToString();
            this._msrtResult[4].Name = "PDMCD" + num.ToString("D2");

            this._msrtResult[5].KeyName = "PDWATT_" + num.ToString();
            this._msrtResult[5].Name = "PDWATT" + num.ToString("D2");

            //----------------------------------------------------------------------------------------
            // Detector02
            this._msrtResult[6].KeyName = "PDCURRENTb_" + num.ToString();
            this._msrtResult[6].Name = "PDCURRENTb" + num.ToString("D2");

            this._msrtResult[7].KeyName = "PDCNTb_" + num.ToString();
            this._msrtResult[7].Name = "PDCNTb" + num.ToString("D2");

            this._msrtResult[8].KeyName = "PDMCDb_" + num.ToString();
            this._msrtResult[8].Name = "PDMCDb" + num.ToString("D2");

            this._msrtResult[9].KeyName = "PDWATTb_" + num.ToString();
            this._msrtResult[9].Name = "PDWATTb" + num.ToString("D2");

            SetMsrtNameAsKey();
            //----------------------------------------------------------------------------------------
            // Reset Gain Offset Seeting KeyName
            for (int i = 0; i < this._msrtResult.Length; i++ )
            {
                this.GainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;
				this.GainOffsetSetting[i].Name = this._msrtResult[i].Name;
            }
        }

        #endregion
    }
}
