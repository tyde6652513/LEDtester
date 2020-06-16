using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class OSATestItem : TestItemData
	{
		private const int COEF_COUNT = 10;
		private double _coefWLResolution;
		private double _coefStartWL;
		private double _coefEndWL;
		private double[][] _coefTable;
		private OsaSettingData _optiSetting;
        private bool _isTestOptical;

		public OSATestItem() : base()
		{
			this._lockObj = new object();

            this.Type = ETestType.OSA;

			// Electrical Setting 
			this._elecSetting = new ElectSettingData[2] { new ElectSettingData( "mA", "V", "ms" ), new ElectSettingData( "mA", "V", "ms" ) };
			this._elecSetting[0].MsrtType = EMsrtType.FIMV;
			this._elecSetting[0].IsAutoTurnOff = false;
            this._elecSetting[0].MsrtRange = 8;
            this._elecSetting[0].MsrtProtection = 8.0d;


			this._elecSetting[1].MsrtType = EMsrtType.MV;
			this._elecSetting[1].IsAutoTurnOff = true;
            this._elecSetting[1].MsrtRange = 8;
            this._elecSetting[1].MsrtProtection = 8.0d;
			// Tested Result Setting and Gain Offset Setting
			this.CreateGainAndMsrtItem();

			// Create Coef. Table memory, it must set start WL, End WL and WL resolution  first
			this._coefStartWL = 600.0d;
			this._coefEndWL = 1750.0d;
			this._coefWLResolution = 1.0d;
            this._isTestOptical = true; 

			this.CreateTable();

            this._optiSetting = new OsaSettingData();

			this.ResetKeyName();
		}

        public OSATestItem(double startWL, double endWL, double WLResolution) : this()
		{
			this._coefStartWL = startWL;
			this._coefEndWL = endWL;
			this._coefWLResolution = WLResolution;
			this.CreateTable();

			this.ResetKeyName();
		}

		#region >>> Public Property <<<

        public OsaSettingData OsaSettingData
        {
            get { return this._optiSetting; }
            set { lock (this._lockObj) { this._optiSetting = value; } }          
        }

		public double[][] CoefTable
		{
			get
			{
				return this._coefTable;
			}
			set
			{
				lock ( this._lockObj )
				{
					this._coefTable = value;
				}
			}
		}

		public double CoefStartWL
		{
			get
			{
				return this._coefStartWL;
			}
			set
			{
				lock ( this._lockObj )
				{
					this._coefStartWL = value;
				}
			}
		}

		public double CoefEndWL
		{
			get
			{
				return this._coefEndWL;
			}
			set
			{
				lock ( this._lockObj )
				{
					this._coefEndWL = value;
				}
			}
		}

		public double CoefWLResolution
		{
			get
			{
				return this._coefWLResolution;
			}
			set
			{
				lock ( this._lockObj )
				{
					this._coefWLResolution = value;
				}
			}
		}

        public bool IsTestOptical
        {
            get
            {
                return this._isTestOptical;
            }
            set
            {
                lock (this._lockObj) { this._isTestOptical = value; }
            }
        }

        public bool IsAlreadyCorrected
        {
            get
            {
                if (this._coefTable == null)
                {
                    return true;
                }

                foreach (double[] array in _coefTable)
                {
                    for (int i = 1; i < array.Length; i++)
                    {
                        if (!array[i].Equals(0.0d) && !array[i].Equals(1.0d))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

		#endregion

		#region >>> Private Method <<<

		private void CreateGainAndMsrtItem()
		{
			// New the MsrtResult Data and GainOffsetSetting Data
            this._msrtResult = new TestResultData[Enum.GetNames(typeof(EOsaOptiMsrtType)).Length];

            this._gainOffsetSetting = new GainOffsetData[Enum.GetNames(typeof(EOsaOptiMsrtType)).Length];

			for ( int i = 0; i < this._msrtResult.Length; i++ )
			{
				this._msrtResult[i] = new TestResultData();

                this._gainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.Offset);
			}

            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVs].Unit = "V";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVs].Formate = "0.000";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVs].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVs].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVs].MaxLimitValue2 = 8.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVs].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSAMsrtVs].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVe].Unit = "V";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVe].Formate = "0.000";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVe].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVe].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVe].MaxLimitValue2 = 8.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMsrtVe].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSAMsrtVe].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSAMeanWl].Unit = "nm";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMeanWl].Formate = "0.000";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMeanWl].MaxLimitValue = 1750.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMeanWl].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMeanWl].MaxLimitValue2 = 1750.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAMeanWl].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSAMeanWl].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakWl].Unit = "nm";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakWl].Formate = "0.000";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakWl].MaxLimitValue = 1750.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakWl].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakWl].MaxLimitValue2 = 1750.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakWl].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSAPeakWl].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakLvl].Unit = "dBm";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakLvl].Formate = "0.00";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakLvl].MaxLimitValue = 30.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakLvl].MinLimitValue = -90.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakLvl].MaxLimitValue2 = 30.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAPeakLvl].MinLimitValue2 = -90.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSAPeakLvl].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeak].Unit = "nm";
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeak].Formate = "0.000";
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeak].MaxLimitValue = 1750.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeak].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeak].MaxLimitValue2 = 1750.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeak].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSA2ndPeak].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeakLvl].Unit = "dBm";
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeakLvl].Formate = "0.00";
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeakLvl].MaxLimitValue = 30.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeakLvl].MinLimitValue = -90.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeakLvl].MaxLimitValue2 = 30.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSA2ndPeakLvl].MinLimitValue2 = -90.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSA2ndPeakLvl].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSAFWHMrms].Unit = "nm";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAFWHMrms].Formate = "0.000";
            this._msrtResult[(int)EOsaOptiMsrtType.OSAFWHMrms].MaxLimitValue = 100.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAFWHMrms].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAFWHMrms].MaxLimitValue2 = 100.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSAFWHMrms].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSAFWHMrms].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSATotalPower].Unit = "dBm";
            this._msrtResult[(int)EOsaOptiMsrtType.OSATotalPower].Formate = "0.00";
            this._msrtResult[(int)EOsaOptiMsrtType.OSATotalPower].MaxLimitValue = 30.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSATotalPower].MinLimitValue = -90.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSATotalPower].MaxLimitValue2 = 30.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSATotalPower].MinLimitValue2 = -90.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSATotalPower].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSASMSR].Unit = "dB";
            this._msrtResult[(int)EOsaOptiMsrtType.OSASMSR].Formate = "0.00";
            this._msrtResult[(int)EOsaOptiMsrtType.OSASMSR].MaxLimitValue = 30.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSASMSR].MinLimitValue = -90.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSASMSR].MaxLimitValue2 = 30.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSASMSR].MinLimitValue2 = -90.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSASMSR].IsEnable = true;

            this._msrtResult[(int)EOsaOptiMsrtType.OSADeltaLamda].Unit = "nm";
            this._msrtResult[(int)EOsaOptiMsrtType.OSADeltaLamda].Formate = "0.000";
            this._msrtResult[(int)EOsaOptiMsrtType.OSADeltaLamda].MaxLimitValue = 100.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSADeltaLamda].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSADeltaLamda].MaxLimitValue2 = 100.0d;
            this._msrtResult[(int)EOsaOptiMsrtType.OSADeltaLamda].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EOsaOptiMsrtType.OSADeltaLamda].IsEnable = true;
		}

		private void CreateTable()
		{
			double value = 0.0d;

			if ( this._coefStartWL >= this._coefEndWL || this._coefStartWL < 0 || this._coefEndWL < 0 )
			{
				return;
			}

			if ( this._coefWLResolution < 0 )
			{
				return;
			}

			UInt32 items = ( UInt32 ) Math.Floor( ( this._coefEndWL - this._coefStartWL ) / this._coefWLResolution ) + 1;

			this._coefTable = new double[items][];

			for ( int i = 0; i < items; i++ )
			{
				value = this._coefStartWL + i * this._coefWLResolution;
				this._coefTable[i] = new double[COEF_COUNT] { value, 0.0d, 0.0d, 0.0d, 1.0d, 1.0d, 1.0d, 0.0d, 0.0d, 0.0d };
			}
		}
		
		#endregion

		#region >>> Protected Method <<<

		protected override void ResetKeyName()
		{
			base.ResetKeyName();

			int num = this._subItemIndex + 1;     // 0-base

            string[] str = Enum.GetNames(typeof(EOsaOptiMsrtType));

			// Reset Tested Result KeyName and Gain Offset Seeting KeyName
			for ( int i = 0; i < this._msrtResult.Length; i++ )
			{
				if ( this._msrtResult[i] == null )
					break;

				this._msrtResult[i].KeyName = str[i] + "_" + num.ToString();
				//this._msrtResult[i].Name = str[i]  + num.ToString("D2");
                this._msrtResult[i].Name = this._msrtResult[i].KeyName;
                //SetMsrtNameAsKey();
				this._gainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;
				this._gainOffsetSetting[i].Name = this._msrtResult[i].Name;
			}
		}

		#endregion

		#region >>> Public Method <<<	

		public void CreateCoefTable( double startWL, double endWL, double resolution )
		{
			this._coefStartWL = startWL;
			this._coefEndWL = endWL;
			this._coefWLResolution = resolution;
			this.CreateTable();		
		}

		#endregion
	}
}
