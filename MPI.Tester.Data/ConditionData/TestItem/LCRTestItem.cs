using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class LCRTestItem : TestItemData
	{
		private LCRSettingData _lcrSetting;

		public LCRTestItem()
			: base()
        {
            this._lockObj = new object();
          
            this._type = ETestType.LCR;

            // LCR Setting 
			this._lcrSetting = new LCRSettingData();

			this._elecSetting = new ElectSettingData[] { new ElectSettingData("V", "V", "ms") };
			this._elecSetting[0].MsrtType = EMsrtType.LCR;

            // Tested Result Setting
			int msrtLen = Enum.GetNames(typeof(ELCRMsrtType)).Length;
			this._msrtResult = new TestResultData[msrtLen];

			this._msrtResult[(int)ELCRMsrtType.LCRCP] = new TestResultData("pF", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRCP].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRCP].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRCP].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRCP].MaxLimitValue2 = 9999999.0d;

			this._msrtResult[(int)ELCRMsrtType.LCRCS] = new TestResultData("pF", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRCS].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRCS].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRCS].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRCS].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRCS].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRLP] = new TestResultData("nH", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRLP].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRLP].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRLP].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRLP].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRLP].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRLS] = new TestResultData("nH", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRLS].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRLS].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRLS].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRLS].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRLS].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRD] = new TestResultData("", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRD].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRD].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRD].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRD].MaxLimitValue2 = 9999999.0d;

			this._msrtResult[(int)ELCRMsrtType.LCRQ] = new TestResultData("", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRQ].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRQ].MaxLimitValue = 1000000.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRQ].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRQ].MaxLimitValue2 = 1000000.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRQ].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRG] = new TestResultData("S", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRG].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRG].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRG].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRG].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRG].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRRP] = new TestResultData("mOhm", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRRP].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRP].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRP].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRP].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRRP].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRRS] = new TestResultData("mOhm", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRRS].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRS].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRS].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRS].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRRS].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRRDC] = new TestResultData("mOhm", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRRDC].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRDC].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRDC].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRRDC].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRRDC].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRR] = new TestResultData("mOhm", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRR].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRR].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRR].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRR].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRR].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRX] = new TestResultData("mOhm", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRX].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRX].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRX].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRX].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRX].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRZ] = new TestResultData("mOhm", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRZ].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRZ].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRZ].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRZ].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRZ].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRY] = new TestResultData("S", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRY].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRY].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRY].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRY].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRY].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRTD] = new TestResultData("deg", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRTD].MinLimitValue = -360;
			this._msrtResult[(int)ELCRMsrtType.LCRTD].MaxLimitValue = 360;
			this._msrtResult[(int)ELCRMsrtType.LCRTD].MinLimitValue2 = -360;
			this._msrtResult[(int)ELCRMsrtType.LCRTD].MaxLimitValue2 = 360;
            this._msrtResult[(int)ELCRMsrtType.LCRTD].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRTR] = new TestResultData("rad", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRTR].MinLimitValue = -6.284;
			this._msrtResult[(int)ELCRMsrtType.LCRTR].MaxLimitValue = 6.284;
			this._msrtResult[(int)ELCRMsrtType.LCRTR].MinLimitValue2 = -6.284;
			this._msrtResult[(int)ELCRMsrtType.LCRTR].MaxLimitValue2 = 6.284;
            this._msrtResult[(int)ELCRMsrtType.LCRTR].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRB] = new TestResultData("S", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRB].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRB].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRB].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRB].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRB].IsEnable = false;

			this._msrtResult[(int)ELCRMsrtType.LCRVDC] = new TestResultData("V", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRVDC].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRVDC].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRVDC].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRVDC].MaxLimitValue2 = 9999999.0d;

			this._msrtResult[(int)ELCRMsrtType.LCRIDC] = new TestResultData("mA", "0.000");
			this._msrtResult[(int)ELCRMsrtType.LCRIDC].MinLimitValue = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRIDC].MaxLimitValue = 9999999.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRIDC].MinLimitValue2 = 0.0d;
			this._msrtResult[(int)ELCRMsrtType.LCRIDC].MaxLimitValue2 = 9999999.0d;
            this._msrtResult[(int)ELCRMsrtType.LCRIDC].IsEnable = false;

            SetMsrtNameAsKey();
			// Gain Offset Setting
			this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset) };

            this.ResetKeyName();
        }

        #region >>> Protected Methods <<<

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            this._elecSetting[0].KeyName = this.KeyName;

            // Reset Tested Result KeyName
            this._msrtResult[0].KeyName = ELCRMsrtType.LCRCP.ToString() + "_" + num.ToString();
			this._msrtResult[0].Name = "CP" + num.ToString("D2");

			this._msrtResult[1].KeyName = ELCRMsrtType.LCRCS.ToString() + "_" + num.ToString();
			this._msrtResult[1].Name = "CS" + num.ToString("D2");

			this._msrtResult[2].KeyName = ELCRMsrtType.LCRLP.ToString() + "_" + num.ToString();
			this._msrtResult[2].Name = "LP" + num.ToString("D2");

			this._msrtResult[3].KeyName = ELCRMsrtType.LCRLS.ToString() + "_" + num.ToString();
			this._msrtResult[3].Name = "LS" + num.ToString("D2");

			this._msrtResult[4].KeyName = ELCRMsrtType.LCRD.ToString() + "_" + num.ToString();
			this._msrtResult[4].Name = "D" + num.ToString("D2");

			this._msrtResult[5].KeyName = ELCRMsrtType.LCRQ.ToString() + "_" + num.ToString();
			this._msrtResult[5].Name = "Q" + num.ToString("D2");

			this._msrtResult[6].KeyName = ELCRMsrtType.LCRG.ToString() + "_" + num.ToString();
			this._msrtResult[6].Name = "G" + num.ToString("D2");

			this._msrtResult[7].KeyName = ELCRMsrtType.LCRRP.ToString() + "_" + num.ToString();
			this._msrtResult[7].Name = "RP" + num.ToString("D2");

			this._msrtResult[8].KeyName = ELCRMsrtType.LCRRS.ToString() + "_" + num.ToString();
			this._msrtResult[8].Name = "RS" + num.ToString("D2");

			this._msrtResult[9].KeyName = ELCRMsrtType.LCRRDC.ToString() + "_" + num.ToString();
			this._msrtResult[9].Name = "Rdc" + num.ToString("D2");

			this._msrtResult[10].KeyName = ELCRMsrtType.LCRR.ToString() + "_" + num.ToString();
			this._msrtResult[10].Name = "R" + num.ToString("D2");

			this._msrtResult[11].KeyName = ELCRMsrtType.LCRX.ToString() + "_" + num.ToString();
			this._msrtResult[11].Name = "X" + num.ToString("D2");

			this._msrtResult[12].KeyName = ELCRMsrtType.LCRZ.ToString() + "_" + num.ToString();
			this._msrtResult[12].Name = "Z" + num.ToString("D2");

			this._msrtResult[13].KeyName = ELCRMsrtType.LCRY.ToString() + "_" + num.ToString();
			this._msrtResult[13].Name = "Y" + num.ToString("D2");

			this._msrtResult[14].KeyName = ELCRMsrtType.LCRTD.ToString() + "_" + num.ToString();
			this._msrtResult[14].Name = "Theta_d" + num.ToString("D2");

			this._msrtResult[15].KeyName = ELCRMsrtType.LCRTR.ToString() + "_" + num.ToString();
			this._msrtResult[15].Name = "Theta_r" + num.ToString("D2");

			this._msrtResult[16].KeyName = ELCRMsrtType.LCRB.ToString() + "_" + num.ToString();
			this._msrtResult[16].Name = "B" + num.ToString("D2");

			this._msrtResult[17].KeyName = ELCRMsrtType.LCRVDC.ToString() + "_" + num.ToString();
			this._msrtResult[17].Name = "Vdc" + num.ToString("D2");

			this._msrtResult[18].KeyName = ELCRMsrtType.LCRIDC.ToString() + "_" + num.ToString();
			this._msrtResult[18].Name = "Idc" + num.ToString("D2");

            // Reset Gain Offset Seeting KeyName
			int msrtLen = Enum.GetNames(typeof(ELCRMsrtType)).Length;
			this._gainOffsetSetting = new GainOffsetData[msrtLen];

			for (int i = 0; i < msrtLen; i++)
			{
				this._gainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.Offset);
				this._gainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;
				this._gainOffsetSetting[i].Name = this._msrtResult[i].Name;
			}
        }

        protected override object GetTestItemForceSetting()//LCR,Calc等記得要覆寫掉
        {
            Dictionary<string, object> kObj = new Dictionary<string, object>();

            kObj.Add("MsrtType", EMsrtType.LCR.ToString());
            kObj.Add("ForceValue", LCRSetting.DCBiasV.ToString());
            kObj.Add("ForceUnit", "V");
            kObj.Add("SignalLevel", LCRSetting.SignalLevelV.ToString());
            kObj.Add("LCRMsrtType", LCRSetting.LCRMsrtType.ToString());
            kObj.Add("Frequency", LCRSetting.Frequency.ToString());

            //Frequency

            return kObj;
        }

        #endregion

		#region >>> Public Property <<<

		public LCRSettingData LCRSetting
		{
			get { return this._lcrSetting; }
			set { lock (this._lockObj) { this._lcrSetting = value; } }
		}

		#endregion
	}
}
