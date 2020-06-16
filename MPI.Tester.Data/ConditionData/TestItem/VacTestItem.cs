using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class VacTestItem : TestItemData
	{
		public VacTestItem()
			: base()
        {
            this._lockObj = new object();

            this._type = ETestType.VAC;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("V", "V", "ms") };

            this._elecSetting[0].MsrtType = EMsrtType.FVMI;

			// Tested Result Setting
			this._msrtResult = new TestResultData[] { new TestResultData("A", "0.000"),		// Current
														new TestResultData("W", "0.000"),	// Power
														new TestResultData("VA", "0.000"),	// Apparent
														new TestResultData("", "0.000"),	// Power Factor
														new TestResultData("Hz", "0.000"),	// Frequency
														new TestResultData("A", "0.000"),	// Current Peak 
														new TestResultData("A", "0.000"),	// Current Peak Max
														};
			// Gain Offset Setting
			this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset), 
																new GainOffsetData(true, EGainOffsetType.Offset),
																new GainOffsetData(true, EGainOffsetType.Offset),
																new GainOffsetData(true, EGainOffsetType.Offset),
																new GainOffsetData(true, EGainOffsetType.Offset),
																new GainOffsetData(true, EGainOffsetType.Offset),
																new GainOffsetData(true, EGainOffsetType.Offset)
																};

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
			this._msrtResult[0].KeyName = "ACMIF_" + num.ToString();

            this._msrtResult[0].Name = "ACMIF" + num.ToString("D2");

			this._msrtResult[0].MinLimitValue = 0.0d;

			this._msrtResult[0].MaxLimitValue = double.MaxValue;

			this._msrtResult[1].KeyName = "ACPOWER_" + num.ToString();

			this._msrtResult[1].Name = "POWER" + num.ToString("D2");

			this._msrtResult[0].MinLimitValue = 0.0d;

			this._msrtResult[0].MaxLimitValue = double.MaxValue;

			this._msrtResult[2].KeyName = "ACAPPARENT_" + num.ToString();

			this._msrtResult[2].Name = "APPARENT" + num.ToString("D2");

			this._msrtResult[0].MinLimitValue = 0.0d;

			this._msrtResult[0].MaxLimitValue = double.MaxValue;

			this._msrtResult[3].KeyName = "ACPF_" + num.ToString();

			this._msrtResult[3].Name = "PF" + num.ToString("D2");

			this._msrtResult[0].MinLimitValue = -double.MaxValue;

			this._msrtResult[0].MaxLimitValue = double.MaxValue;

			this._msrtResult[4].KeyName = "ACFREQUENCY_" + num.ToString();

			this._msrtResult[4].Name = "FREQUENCY" + num.ToString("D2");

			this._msrtResult[0].MinLimitValue = 0.0d;

			this._msrtResult[0].MaxLimitValue = double.MaxValue;

			this._msrtResult[5].KeyName = "ACPEAK_" + num.ToString();

			this._msrtResult[5].Name = "PEAK" + num.ToString("D2");

			this._msrtResult[0].MinLimitValue = 0.0d;

			this._msrtResult[0].MaxLimitValue = double.MaxValue;

			this._msrtResult[6].KeyName = "ACPEAKMAX_" + num.ToString();

			this._msrtResult[6].Name = "PEAKMAX" + num.ToString("D2");

			this._msrtResult[0].MinLimitValue = 0.0d;

			this._msrtResult[0].MaxLimitValue = double.MaxValue;

            SetMsrtNameAsKey();

			// Reset Tested Result KeyName and Gain Offset Seeting KeyName
			for (int i = 0; i < this._msrtResult.Length; i++)
			{
				if (this._msrtResult[i] == null)
				{
					break;
				}

				this._gainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;
				
				this._gainOffsetSetting[i].Name = this._msrtResult[i].Name;
			}
        }


        #endregion
	}
}
