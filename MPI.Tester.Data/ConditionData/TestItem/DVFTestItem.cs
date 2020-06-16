using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class DVFTestItem : TestItemData
	{
		private bool _isAutoTurnOff = true;

		public DVFTestItem() : base()
        {
            this._lockObj = new object();

            this._type = ETestType.DVF;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] {   new ElectSettingData("mA" , "V", "ms" ),
																				new ElectSettingData("mA" , "V", "ms" ),
																				new ElectSettingData("mA" , "V", "ms" ),	};

            for (int i = 0; i < this._elecSetting.Length; i++ )
            {
                this._elecSetting[i].MsrtType = EMsrtType.FIMV;
            }
                     
            // Tested Result Setting
            this._msrtResult = new TestResultData[] {		new TestResultData("V", "0.000"),
																				new TestResultData("V", "0.000"),
																				new TestResultData("V", "0.000"),
																				new TestResultData("V", "0.000"),	 };


            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset),
                                                                                                   new GainOffsetData(true, EGainOffsetType.Offset) ,
                                                                                                   new GainOffsetData(true, EGainOffsetType.Offset) ,
                                                                                                   new GainOffsetData(true, EGainOffsetType.Offset) };

            // No Gain Offset Setting
            //this._gainOffsetSetting = null;

            this.ResetKeyName();
        }

		#region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;                       // 0-base
            int index = 1 + 3 * this._subItemIndex;

            // Reset Electrical Setting KeyName
            this._elecSetting[0].KeyName = "IFMA_" + num.ToString();
			this._elecSetting[0].Name = "IFMA" + num.ToString("D2"); 
            this._elecSetting[1].KeyName = "IFMB_" + num.ToString();
			this._elecSetting[1].Name = "IFMB" + num.ToString("D2");    
            this._elecSetting[2].KeyName = "IFMC_" + num.ToString();
			this._elecSetting[2].Name = "IFMC" + num.ToString("D2");
   
            // Reset Tested Result KeyName
			this._msrtResult[0].KeyName = "MVFMA_" + num.ToString();
            this._msrtResult[0].Name = this._msrtResult[0].Name;
			this._msrtResult[1].KeyName = "MVFMB_" + num.ToString();
            this._msrtResult[1].Name = this._msrtResult[1].Name;
			this._msrtResult[2].KeyName = "MVFMC_" + num.ToString();
            this._msrtResult[2].Name = this._msrtResult[0].Name;
			this._msrtResult[3].KeyName = "MVFMD_" + num.ToString();
            this._msrtResult[3].Name = this._msrtResult[0].Name;


            SetMsrtNameAsKey();

            for (int i = 0; i < 4; ++i)
            {
                this._gainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
                this._gainOffsetSetting[0].Name = this._msrtResult[0].Name;
            }   
        }

        #endregion

		#region >>> Public Methods <<<

		public bool IsAutoTurnOff
		{
			get { return this._isAutoTurnOff; }
			set { this._isAutoTurnOff = value; }
		}

		#endregion
    }
}

