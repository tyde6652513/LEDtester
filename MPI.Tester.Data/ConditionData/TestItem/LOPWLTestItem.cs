using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class LOPWLTestItem : TestItemData
	{
		private const int COEF_COUNT = 10;
		private double _coefWLResolution;
		private double _coefStartWL;
		private double _coefEndWL;
		private double[][] _coefTable;
		private OptiSettingData _optiSetting;
        private bool _isTestOptical;
		private bool _isACSourceMeter;
		private bool _isOnlyTestMVFLA;
		private bool _isTestElecDontTestLOPWL;
        private bool _isPDSensing;

        private bool _isUseMsrtAsForceValue;
        private string _refMsrtKeyName;
        private string _refMsrtName;
        private double _factor;
        private double _offset;
        private double _maxProtectForceValue;

		public LOPWLTestItem()	: base()
		{
			this._lockObj = new object();

			this.Type = ETestType.LOPWL;

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
			this._coefStartWL = 200.0d;
			this._coefEndWL = 1200.0d;
			this._coefWLResolution = 1.0d;
            this._isTestOptical = true; 

			this.CreateTable();

			this._optiSetting = new OptiSettingData();

			this.ResetKeyName();

			this._isACSourceMeter = false;

            this._isPDSensing = false;

			this._isOnlyTestMVFLA = false;

			this._isTestElecDontTestLOPWL = false;

            this._factor = 1.0d;
            this._offset = 0.0d;

            this._maxProtectForceValue = 0.1d;
		}

		public LOPWLTestItem( double startWL, double endWL, double WLResolution )	: this()
		{
			this._coefStartWL = startWL;
			this._coefEndWL = endWL;
			this._coefWLResolution = WLResolution;
			this.CreateTable();

			this.ResetKeyName();
		}

		#region >>> Public Property <<<

        public OptiSettingData OptiSetting
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

		public bool IsACSourceMeter
		{
			get
			{
				return this._isACSourceMeter;
			}
			set
			{
				lock (this._lockObj) { this._isACSourceMeter = value; }
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

        public bool EnablePDSensing
        {
            get { return this._isPDSensing; }
            set { lock (this._lockObj) { this._isPDSensing = value; } }
        }

		public bool IsOnlyTestMVFLA
		{
			get { return this._isOnlyTestMVFLA; }
			set { lock (this._lockObj) { this._isOnlyTestMVFLA = value; } }
		}

		public bool IsTestElecDontTestLOPWL
		{
			get { return this._isTestElecDontTestLOPWL; }
			set { lock (this._lockObj) { this._isTestElecDontTestLOPWL = value; } }
		}
		
        public bool IsUseMsrtAsForceValue
        {
            get { return this._isUseMsrtAsForceValue; }
            set { lock (this._lockObj) { this._isUseMsrtAsForceValue = value; } }
        }

        public string RefMsrtKeyName
        {
            get { return this._refMsrtKeyName; }
            set { lock (this._lockObj) { this._refMsrtKeyName = value; } }
        }

        public string RefMsrtName
        {
            get { return this._refMsrtName; }
            set { lock (this._lockObj) { this._refMsrtName = value; } }
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

        public double MaxProtectForceValue
        {
            get { return this._maxProtectForceValue; }
            set { lock (this._lockObj) { this._maxProtectForceValue = value; } }
        }

		#endregion

		#region >>> Private Method <<<

		private void CreateGainAndMsrtItem()
		{
			// New the MsrtResult Data and GainOffsetSetting Data
			this._msrtResult = new TestResultData[Enum.GetNames( typeof( EOptiMsrtType ) ).Length];
			this._gainOffsetSetting = new GainOffsetData[Enum.GetNames(typeof(EOptiMsrtType)).Length];
			for ( int i = 0; i < this._msrtResult.Length; i++ )
			{
				this._msrtResult[i] = new TestResultData();
				if (i == ((int)EOptiMsrtType.LOP) || i == ((int)EOptiMsrtType.WATT) || i == ((int)EOptiMsrtType.LM))
				{
					this._gainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.Gain);
				}
				else
				{
					this._gainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.Offset);
				}
			}
			

			// Set Tested Result Items and Gain Offset Setting
			this._msrtResult[( int ) EOptiMsrtType.LOP].Unit = "mcd";
			this._msrtResult[( int ) EOptiMsrtType.LOP].Formate = "0.0000";
			this._msrtResult[( int ) EOptiMsrtType.LOP].MaxLimitValue = 999.999d;
			this._msrtResult[( int ) EOptiMsrtType.LOP].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOptiMsrtType.LOP].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)EOptiMsrtType.LOP].MinLimitValue2 = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.LOP].IsEnable = true;

			this._msrtResult[( int ) EOptiMsrtType.WATT].Unit = "mW";
			this._msrtResult[( int ) EOptiMsrtType.WATT].Formate = "0.0000";
			this._msrtResult[( int ) EOptiMsrtType.WATT].MaxLimitValue = 9999.999d;
			this._msrtResult[( int ) EOptiMsrtType.WATT].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOptiMsrtType.WATT].MaxLimitValue2 = 9999.999d;
            this._msrtResult[(int)EOptiMsrtType.WATT].MinLimitValue2 = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.WATT].IsEnable = true;

			this._msrtResult[( int ) EOptiMsrtType.LM].Unit = "lm";
			this._msrtResult[( int ) EOptiMsrtType.LM].Formate = "0.0000";
			this._msrtResult[( int ) EOptiMsrtType.LM].MaxLimitValue = 999.999d;
			this._msrtResult[( int ) EOptiMsrtType.LM].MinLimitValue = 0.0d;
            this._msrtResult[(int)EOptiMsrtType.LM].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)EOptiMsrtType.LM].MinLimitValue2 = 0.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.LM].IsEnable = true;

			this._msrtResult[( int ) EOptiMsrtType.WLP].Unit = "nm";
			this._msrtResult[( int ) EOptiMsrtType.WLP].Formate = "0.00";
			this._msrtResult[( int ) EOptiMsrtType.WLP].MaxLimitValue = 780.0d;
			this._msrtResult[( int ) EOptiMsrtType.WLP].MinLimitValue = 380.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.WLP].IsEnable = true;

			this._msrtResult[( int ) EOptiMsrtType.WLD].Unit = "nm";
			this._msrtResult[( int ) EOptiMsrtType.WLD].Formate = "0.00";
			this._msrtResult[( int ) EOptiMsrtType.WLD].MaxLimitValue = 780.0d;
			this._msrtResult[( int ) EOptiMsrtType.WLD].MinLimitValue = 380.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.WLD].IsEnable = true;

			this._msrtResult[( int ) EOptiMsrtType.WLC].Unit = "nm";
			this._msrtResult[( int ) EOptiMsrtType.WLC].Formate = "0.00";
			this._msrtResult[( int ) EOptiMsrtType.WLC].MaxLimitValue = 780.0d;
			this._msrtResult[( int ) EOptiMsrtType.WLC].MinLimitValue = 380.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.WLC].IsEnable = true;

			this._msrtResult[( int ) EOptiMsrtType.HW].Unit = "nm";
			this._msrtResult[( int ) EOptiMsrtType.HW].Formate = "0.00";
			this._msrtResult[( int ) EOptiMsrtType.HW].MaxLimitValue = 780.0d;
			this._msrtResult[( int ) EOptiMsrtType.HW].MinLimitValue = 380.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.HW].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.PURITY].Unit = "";
			this._msrtResult[(int)EOptiMsrtType.PURITY].Formate = "0.00";
			this._msrtResult[(int)EOptiMsrtType.PURITY].MaxLimitValue = 1.00d;
			this._msrtResult[(int)EOptiMsrtType.PURITY].MinLimitValue = 0.00d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.PURITY].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.CIEx].Unit = "";
			this._msrtResult[(int)EOptiMsrtType.CIEx].Formate = "0.0000";
			this._msrtResult[(int)EOptiMsrtType.CIEx].MaxLimitValue = 1.0d;
			this._msrtResult[(int)EOptiMsrtType.CIEx].MinLimitValue = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.CIEx].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.CIEy].Unit = "";
			this._msrtResult[(int)EOptiMsrtType.CIEy].Formate = "0.0000";
			this._msrtResult[(int)EOptiMsrtType.CIEy].MaxLimitValue = 1.0d;
			this._msrtResult[(int)EOptiMsrtType.CIEy].MinLimitValue = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.CIEy].IsEnable = true;

			this._msrtResult[(int) EOptiMsrtType.CIEz].Unit = "";
			this._msrtResult[(int) EOptiMsrtType.CIEz].Formate = "0.0000";
			this._msrtResult[(int)EOptiMsrtType.CIEz].MaxLimitValue = 1.0d;
			this._msrtResult[(int)EOptiMsrtType.CIEz].MinLimitValue = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.CIEz].IsEnable = false;

			this._msrtResult[(int) EOptiMsrtType.CCT].Unit = "K";
			this._msrtResult[(int) EOptiMsrtType.CCT].Formate = "0";
			this._msrtResult[(int)EOptiMsrtType.CCT].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)EOptiMsrtType.CCT].MinLimitValue = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.CCT].IsEnable = true;

			this._msrtResult[( int ) EOptiMsrtType.ST].Unit = "ms";
			this._msrtResult[( int ) EOptiMsrtType.ST].Formate = "0.0";
			this._msrtResult[(int)EOptiMsrtType.ST].MaxLimitValue = 99999d;
			this._msrtResult[(int)EOptiMsrtType.ST].MinLimitValue = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.ST].IsEnable = false;

			this._msrtResult[(int)EOptiMsrtType.INT].Unit = "cnt";
			this._msrtResult[(int)EOptiMsrtType.INT].Formate = "0";
			this._msrtResult[(int)EOptiMsrtType.INT].MaxLimitValue = 999999d;
			this._msrtResult[(int)EOptiMsrtType.INT].MinLimitValue = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.INT].IsEnable = false;

			this._msrtResult[(int)EOptiMsrtType.INTP].Unit = "%";
			this._msrtResult[(int)EOptiMsrtType.INTP].Formate = "0.00";
			this._msrtResult[(int)EOptiMsrtType.INTP].MaxLimitValue = 100.0d;
			this._msrtResult[(int)EOptiMsrtType.INTP].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.INTP].IsEnable = false;

			this._msrtResult[( int ) EOptiMsrtType.MVFLA].Unit = "V";
			this._msrtResult[( int ) EOptiMsrtType.MVFLA].Formate = "0.0000";
			this._msrtResult[( int ) EOptiMsrtType.MVFLA].MaxLimitValue = 8.0d;
			this._msrtResult[( int ) EOptiMsrtType.MVFLA].MinLimitValue = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.MVFLA].IsEnable = true;

			this._msrtResult[( int ) EOptiMsrtType.MVFLB].Unit = "V";
			this._msrtResult[( int ) EOptiMsrtType.MVFLB].Formate = "0.0000";
			this._msrtResult[( int ) EOptiMsrtType.MVFLB].MaxLimitValue = 8.0d;
			this._msrtResult[( int ) EOptiMsrtType.MVFLB].MinLimitValue = 0.0d;
			this._gainOffsetSetting[( int ) EOptiMsrtType.MVFLB].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.MVFLD].Unit = "V";
			this._msrtResult[(int)EOptiMsrtType.MVFLD].Formate = "0.0000";
			this._msrtResult[(int)EOptiMsrtType.MVFLD].MaxLimitValue = 8.0d;
			this._msrtResult[(int)EOptiMsrtType.MVFLD].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.MVFLD].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.WLCP].Unit = "nm";
			this._msrtResult[(int)EOptiMsrtType.WLCP].Formate = "0.00";
			this._msrtResult[(int)EOptiMsrtType.WLCP].MaxLimitValue = 780.0d;
			this._msrtResult[(int)EOptiMsrtType.WLCP].MinLimitValue = 380.0d;
			this._msrtResult[(int)EOptiMsrtType.WLCP].IsEnable = false;
			this._msrtResult[(int)EOptiMsrtType.WLCP].IsVision = false;
			this._gainOffsetSetting[(int)EOptiMsrtType.WLCP].IsEnable = false;

			this._msrtResult[(int)EOptiMsrtType.STR].Unit = "cnt";
			this._msrtResult[(int)EOptiMsrtType.STR].Formate = "0.0";
			this._msrtResult[(int)EOptiMsrtType.STR].MaxLimitValue = 999999d;
			this._msrtResult[(int)EOptiMsrtType.STR].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.STR].IsEnable = false;

			this._msrtResult[(int)EOptiMsrtType.DWDWP].Unit = "nm";
			this._msrtResult[(int)EOptiMsrtType.DWDWP].Formate = "0.00";
			this._msrtResult[(int)EOptiMsrtType.DWDWP].MaxLimitValue = 780.0d;
			this._msrtResult[(int)EOptiMsrtType.DWDWP].MinLimitValue = 380.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.DWDWP].IsEnable = false;

			this._msrtResult[(int)EOptiMsrtType.DARKA].Unit = "cnt";
			this._msrtResult[(int)EOptiMsrtType.DARKA].Formate = "0.000";
			this._msrtResult[(int)EOptiMsrtType.DARKA].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)EOptiMsrtType.DARKA].MinLimitValue = 0.0d;

			this._msrtResult[(int)EOptiMsrtType.DARKB].Unit = "cnt";
			this._msrtResult[(int)EOptiMsrtType.DARKB].Formate = "0.000";
			this._msrtResult[(int)EOptiMsrtType.DARKB].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)EOptiMsrtType.DARKB].MinLimitValue = 0.0d;

			this._msrtResult[(int)EOptiMsrtType.DARKB].Unit = "cnt";
			this._msrtResult[(int)EOptiMsrtType.DARKB].Formate = "0.000";
			this._msrtResult[(int)EOptiMsrtType.DARKB].MaxLimitValue = 99999.0d;
			this._msrtResult[(int)EOptiMsrtType.DARKB].MinLimitValue = 0.0d;

            this._msrtResult[(int)EOptiMsrtType.INTSS].Unit = "ms";
            this._msrtResult[(int)EOptiMsrtType.INTSS].Formate = "0.000";
            this._msrtResult[(int)EOptiMsrtType.INTSS].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)EOptiMsrtType.INTSS].MinLimitValue = 0.0d;


            this._msrtResult[(int)EOptiMsrtType.MFILA].Unit = "mA";
            this._msrtResult[(int)EOptiMsrtType.MFILA].Formate = "0.000";
            this._msrtResult[(int)EOptiMsrtType.MFILA].MaxLimitValue = 2000.0d;
            this._msrtResult[(int)EOptiMsrtType.MFILA].MinLimitValue = 0.0d;

            this._msrtResult[(int)EOptiMsrtType.EWATT].Unit = "W";
            this._msrtResult[(int)EOptiMsrtType.EWATT].Formate = "0.000";
            this._msrtResult[(int)EOptiMsrtType.EWATT].MaxLimitValue = 10.0d;
            this._msrtResult[(int)EOptiMsrtType.EWATT].MinLimitValue = 0.0d;

            this._msrtResult[(int)EOptiMsrtType.LE].Unit = "lm/W";
            this._msrtResult[(int)EOptiMsrtType.LE].Formate = "0.000";
            this._msrtResult[(int)EOptiMsrtType.LE].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)EOptiMsrtType.LE].MinLimitValue = 0.0d;

            this._msrtResult[(int)EOptiMsrtType.WPE].Unit = "%";
            this._msrtResult[(int)EOptiMsrtType.WPE].Formate = "0.000";
            this._msrtResult[(int)EOptiMsrtType.WPE].MaxLimitValue = 100.0d;
            this._msrtResult[(int)EOptiMsrtType.WPE].MinLimitValue = 0.0d;

            this._msrtResult[(int)EOptiMsrtType.Duv].Unit = "";
            this._msrtResult[(int)EOptiMsrtType.Duv].Formate = "0.000";
            this._msrtResult[(int)EOptiMsrtType.Duv].MaxLimitValue = 100.0d;
            this._msrtResult[(int)EOptiMsrtType.Duv].MinLimitValue = 0.0d;

			for (int k = (int)EOptiMsrtType.R01; k < (int)EOptiMsrtType.R15; k++)
			{
				this._msrtResult[k].Unit = "";
				this._msrtResult[k].Formate = "0.0";
				this._msrtResult[k].MaxLimitValue = 100.0d;
				this._msrtResult[k].MinLimitValue = 0.0d;
			}

			//AC Item
			this._msrtResult[(int)EOptiMsrtType.ACMIFL].Unit = "V";
            this._msrtResult[(int)EOptiMsrtType.ACMIFL].Formate = "0.0";
            this._msrtResult[(int)EOptiMsrtType.ACMIFL].MaxLimitValue = 999.0d;
            this._msrtResult[(int)EOptiMsrtType.ACMIFL].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EOptiMsrtType.ACMIFL].IsEnable = true;

            this._msrtResult[(int)EOptiMsrtType.ACPOWERL].Unit = "W";
            this._msrtResult[(int)EOptiMsrtType.ACPOWERL].Formate = "0.0";
            this._msrtResult[(int)EOptiMsrtType.ACPOWERL].MaxLimitValue = 9999.0d;
            this._msrtResult[(int)EOptiMsrtType.ACPOWERL].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EOptiMsrtType.ACPOWERL].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.ACAPPARENTL].Unit = "VA";
            this._msrtResult[(int)EOptiMsrtType.ACAPPARENTL].Formate = "0.0";
            this._msrtResult[(int)EOptiMsrtType.ACAPPARENTL].MaxLimitValue = 9999.0d;
            this._msrtResult[(int)EOptiMsrtType.ACAPPARENTL].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EOptiMsrtType.ACAPPARENTL].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.ACPFL].Unit = "";
            this._msrtResult[(int)EOptiMsrtType.ACPFL].Formate = "0.0";
            this._msrtResult[(int)EOptiMsrtType.ACPFL].MaxLimitValue = 1.0d;
            this._msrtResult[(int)EOptiMsrtType.ACPFL].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EOptiMsrtType.ACPFL].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.ACFREQUENCYL].Unit = "hz";
            this._msrtResult[(int)EOptiMsrtType.ACFREQUENCYL].Formate = "0.0";
            this._msrtResult[(int)EOptiMsrtType.ACFREQUENCYL].MaxLimitValue = 1000.0d;
            this._msrtResult[(int)EOptiMsrtType.ACFREQUENCYL].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EOptiMsrtType.ACFREQUENCYL].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.ACPEAKL].Unit = "mA";
            this._msrtResult[(int)EOptiMsrtType.ACPEAKL].Formate = "0.0";
            this._msrtResult[(int)EOptiMsrtType.ACPEAKL].MaxLimitValue = 9999.0d;
            this._msrtResult[(int)EOptiMsrtType.ACPEAKL].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EOptiMsrtType.ACPEAKL].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.ACPEAKMAXL].Unit = "mA";
            this._msrtResult[(int)EOptiMsrtType.ACPEAKMAXL].Formate = "0.0";
            this._msrtResult[(int)EOptiMsrtType.ACPEAKMAXL].MaxLimitValue = 9999.0d;
            this._msrtResult[(int)EOptiMsrtType.ACPEAKMAXL].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EOptiMsrtType.ACPEAKMAXL].IsEnable = true;

			//SDCM
			this._msrtResult[(int)EOptiMsrtType.ANSISDCM].Unit = "";
			this._msrtResult[(int)EOptiMsrtType.ANSISDCM].Formate = "0";
			this._msrtResult[(int)EOptiMsrtType.ANSISDCM].MaxLimitValue = 999.0d;
			this._msrtResult[(int)EOptiMsrtType.ANSISDCM].MinLimitValue = 1.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.ANSISDCM].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.ANSINEARCCT].Unit = "K";
			this._msrtResult[(int)EOptiMsrtType.ANSINEARCCT].Formate = "0";
			this._msrtResult[(int)EOptiMsrtType.ANSINEARCCT].MaxLimitValue = 9999.0d;
			this._msrtResult[(int)EOptiMsrtType.ANSINEARCCT].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.ANSINEARCCT].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.ANSINEARSDCM].Unit = "";
			this._msrtResult[(int)EOptiMsrtType.ANSINEARSDCM].Formate = "0";
			this._msrtResult[(int)EOptiMsrtType.ANSINEARSDCM].MaxLimitValue = 999.0d;
			this._msrtResult[(int)EOptiMsrtType.ANSINEARSDCM].MinLimitValue = 1.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.ANSINEARSDCM].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.GBSDCM].Unit = "";
			this._msrtResult[(int)EOptiMsrtType.GBSDCM].Formate = "0";
			this._msrtResult[(int)EOptiMsrtType.GBSDCM].MaxLimitValue = 999.0d;
			this._msrtResult[(int)EOptiMsrtType.GBSDCM].MinLimitValue = 1.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.GBSDCM].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.GBNEARCCT].Unit = "K";
			this._msrtResult[(int)EOptiMsrtType.GBNEARCCT].Formate = "0";
			this._msrtResult[(int)EOptiMsrtType.GBNEARCCT].MaxLimitValue = 9999.0d;
			this._msrtResult[(int)EOptiMsrtType.GBNEARCCT].MinLimitValue = 0.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.GBNEARCCT].IsEnable = true;

			this._msrtResult[(int)EOptiMsrtType.GBNEARSDCM].Unit = "";
			this._msrtResult[(int)EOptiMsrtType.GBNEARSDCM].Formate = "0";
			this._msrtResult[(int)EOptiMsrtType.GBNEARSDCM].MaxLimitValue = 999.0d;
			this._msrtResult[(int)EOptiMsrtType.GBNEARSDCM].MinLimitValue = 1.0d;
			this._gainOffsetSetting[(int)EOptiMsrtType.GBNEARSDCM].IsEnable = true;
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

			string[] str = Enum.GetNames( typeof( EOptiMsrtType ) );

			// Reset Electrical Setting KeyName
			this._elecSetting[0].KeyName = "IFWLA_" + num.ToString(); 
			this._elecSetting[1].KeyName = "IFWLB_" + num.ToString();

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
