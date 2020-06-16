using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class THYTestItem : TestItemData
	{

		public THYTestItem() : base()
		{
			this._lockObj = new object();
       
			this._type = ETestType.THY;

			this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
			this._elecSetting[0].MsrtType = EMsrtType.THY;            
                        
			// Tested Result Setting
			this._msrtResult = new TestResultData[] {   new TestResultData("V", "0.000"),
														new TestResultData("V", "0.000"),
														new TestResultData("V", "0.000"),

														new TestResultData("V", "0.000"),
														new TestResultData("V", "0.000"),

														//new TestResultData("cnt", "0"),
														//new TestResultData("m", "0.000"),
														//new TestResultData("b", "0.000"), 
														};

            // Gain Offset Setting
            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset),
                                                             new GainOffsetData(true, EGainOffsetType.Offset),
                                                             new GainOffsetData(true, EGainOffsetType.Offset),
                                                             new GainOffsetData(true, EGainOffsetType.Offset),
                                                             new GainOffsetData(true, EGainOffsetType.Offset)};

			// Then reset those keyname of this._msrtResult[] Array
			this.ResetKeyName();
		}
        
        #region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

			// Reset Electrical Setting KeyName
			this._elecSetting[0].KeyName = "THYIF_" + num.ToString();
			this._elecSetting[0].Name = "THYIF" + num.ToString();

			// Reset Tested Result KeyName
			this._msrtResult[0].KeyName = "MTHYVP_" + num.ToString();
			this._msrtResult[0].Name = "MTHYVP" + num.ToString("D2");

			this._msrtResult[1].KeyName = "MTHYVS_" + num.ToString();
			this._msrtResult[1].Name = "MTHYVS" + num.ToString("D2");

			this._msrtResult[2].KeyName = "MTHYVD_" + num.ToString();
			this._msrtResult[2].Name = "MTHYVD" +  num.ToString("D2");

			this._msrtResult[3].KeyName = "MTHYVDA_" + num.ToString();
			this._msrtResult[3].Name = "MTHYVDA" + num.ToString("D2");

			this._msrtResult[4].KeyName = "MTHYVDB_" + num.ToString();
			this._msrtResult[4].Name = "MTHYVDB" + num.ToString("D2");

			//this._msrtResult[3].KeyName = "THYVCP" + "_" + num.ToString();
			//this._msrtResult[4].KeyName = "THYVCL" + "_" + num.ToString();
			//this._msrtResult[5].KeyName = "THYVCD" + "_" + num.ToString();

			//this._msrtResult[6].KeyName = "THYINDEX" + "_" + num.ToString();
			//this._msrtResult[7].KeyName = "THYM" + "_" + num.ToString();
			//this._msrtResult[8].KeyName = "THYB" + "_" + num.ToString();	
            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName
            for (int i = 0; i < this._msrtResult.Length; i++)
            {
                this.GainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;
                this.GainOffsetSetting[i].Name = this._msrtResult[i].Name;
            }


        }

        #endregion

	}
}
