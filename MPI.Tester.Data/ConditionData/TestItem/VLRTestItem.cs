using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class VLRTestItem : TestItemData
    {
        private const int COEF_COUNT = 10;
        private double _coefWLResolution;
        private double _coefStartWL;
        private double _coefEndWL;
        private double[][] _coefTable;
        private OptiSettingData _optiSetting;
        private bool _isTestOptical;
        private bool _isACSourceMeter;

        public VLRTestItem()
            : base()
        {
            this._lockObj = new object();

            this.Type = ETestType.VLR;

            // Electrical Setting 
            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.VLR;

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
        }

        public VLRTestItem(double startWL, double endWL, double WLResolution)
            : this()
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
                lock (this._lockObj)
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
                lock (this._lockObj)
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
                lock (this._lockObj)
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
                lock (this._lockObj)
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

        #endregion

        #region >>> Private Method <<<

        private void CreateGainAndMsrtItem()
        {
            // New the MsrtResult Data and GainOffsetSetting Data
            this._msrtResult = new TestResultData[Enum.GetNames(typeof(EVLROptiMsrtType)).Length];
            this._gainOffsetSetting = new GainOffsetData[Enum.GetNames(typeof(EVLROptiMsrtType)).Length];
            for (int i = 0; i < this._msrtResult.Length; i++)
            {
                this._msrtResult[i] = new TestResultData();
                if (i == ((int)EVLROptiMsrtType.VLRLOP) || i == ((int)EVLROptiMsrtType.VLRWATT) || i == ((int)EVLROptiMsrtType.VLRLM))
                {
                    this._gainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.Gain);
                }
                else
                {
                    this._gainOffsetSetting[i] = new GainOffsetData(true, EGainOffsetType.Offset);
                }
            }

            // Set Tested Result Items and Gain Offset Setting
            this._msrtResult[(int)EVLROptiMsrtType.VLRVA].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVA].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVA].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRVA].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRVA].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRVB].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVB].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVB].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRVB].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRVB].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRVC].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVC].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVC].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRVC].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRVC].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRVD].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVD].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVD].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRVD].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRVD].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRDVA].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVA].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVA].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVA].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRDVA].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRDVB].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVB].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVB].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVB].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRDVB].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRDVC].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVC].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVC].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRDVC].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRDVC].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRVAS].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVAS].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVAS].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRVAS].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRVA].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRVBS].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVBS].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVBS].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRVBS].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRVB].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRVCS].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVCS].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVCS].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRVCS].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRVC].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRVDS].Unit = "V";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVDS].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRVDS].MaxLimitValue = 8.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRVDS].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRVD].IsEnable = true;
            
            this._msrtResult[(int)EVLROptiMsrtType.VLRLOP].Unit = "mcd";
            this._msrtResult[(int)EVLROptiMsrtType.VLRLOP].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRLOP].MaxLimitValue = 999.999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRLOP].MinLimitValue = 0.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRLOP].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRLOP].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRLOP].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRWATT].Unit = "mW";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWATT].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWATT].MaxLimitValue = 9999.999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWATT].MinLimitValue = 0.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWATT].MaxLimitValue2 = 9999.999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWATT].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRWATT].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRLM].Unit = "lm";
            this._msrtResult[(int)EVLROptiMsrtType.VLRLM].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRLM].MaxLimitValue = 999.999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRLM].MinLimitValue = 0.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRLM].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRLM].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRLM].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRWLP].Unit = "nm";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLP].Formate = "0.00";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLP].MaxLimitValue = 780.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLP].MinLimitValue = 380.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRWLP].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRWLD].Unit = "nm";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLD].Formate = "0.00";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLD].MaxLimitValue = 780.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLD].MinLimitValue = 380.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRWLD].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRWLC].Unit = "nm";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLC].Formate = "0.00";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLC].MaxLimitValue = 780.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLC].MinLimitValue = 380.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRWLC].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRHW].Unit = "nm";
            this._msrtResult[(int)EVLROptiMsrtType.VLRHW].Formate = "0.00";
            this._msrtResult[(int)EVLROptiMsrtType.VLRHW].MaxLimitValue = 780.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRHW].MinLimitValue = 380.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRHW].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRPURITY].Unit = "";
            this._msrtResult[(int)EVLROptiMsrtType.VLRPURITY].Formate = "0.00";
            this._msrtResult[(int)EVLROptiMsrtType.VLRPURITY].MaxLimitValue = 1.00d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRPURITY].MinLimitValue = 0.00d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRPURITY].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEx].Unit = "";
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEx].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEx].MaxLimitValue = 1.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEx].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRCIEx].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEy].Unit = "";
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEy].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEy].MaxLimitValue = 1.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEy].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRCIEy].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEz].Unit = "";
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEz].Formate = "0.0000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEz].MaxLimitValue = 1.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRCIEz].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRCIEz].IsEnable = false;

            this._msrtResult[(int)EVLROptiMsrtType.VLRCCT].Unit = "K";
            this._msrtResult[(int)EVLROptiMsrtType.VLRCCT].Formate = "0";
            this._msrtResult[(int)EVLROptiMsrtType.VLRCCT].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRCCT].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRCCT].IsEnable = true;

            this._msrtResult[(int)EVLROptiMsrtType.VLRST].Unit = "ms";
            this._msrtResult[(int)EVLROptiMsrtType.VLRST].Formate = "0.0";
            this._msrtResult[(int)EVLROptiMsrtType.VLRST].MaxLimitValue = 99999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRST].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRST].IsEnable = false;

            this._msrtResult[(int)EVLROptiMsrtType.VLRINT].Unit = "cnt";
            this._msrtResult[(int)EVLROptiMsrtType.VLRINT].Formate = "0";
            this._msrtResult[(int)EVLROptiMsrtType.VLRINT].MaxLimitValue = 999999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRINT].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRINT].IsEnable = false;

            this._msrtResult[(int)EVLROptiMsrtType.VLRINTP].Unit = "%";
            this._msrtResult[(int)EVLROptiMsrtType.VLRINTP].Formate = "0.00";
            this._msrtResult[(int)EVLROptiMsrtType.VLRINTP].MaxLimitValue = 100.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRINTP].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRINTP].IsEnable = false;

            this._msrtResult[(int)EVLROptiMsrtType.VLRWLCP].Unit = "nm";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLCP].Formate = "0.00";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLCP].MaxLimitValue = 780.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLCP].MinLimitValue = 380.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLCP].IsEnable = false;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWLCP].IsVision = false;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRWLCP].IsEnable = false;

            this._msrtResult[(int)EVLROptiMsrtType.VLRSTR].Unit = "cnt";
            this._msrtResult[(int)EVLROptiMsrtType.VLRSTR].Formate = "0.0";
            this._msrtResult[(int)EVLROptiMsrtType.VLRSTR].MaxLimitValue = 999999d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRSTR].MinLimitValue = 0.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRSTR].IsEnable = false;

            this._msrtResult[(int)EVLROptiMsrtType.VLRDWDWP].Unit = "nm";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDWDWP].Formate = "0.00";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDWDWP].MaxLimitValue = 780.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRDWDWP].MinLimitValue = 380.0d;
            this._gainOffsetSetting[(int)EVLROptiMsrtType.VLRDWDWP].IsEnable = false;

            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKA].Unit = "cnt";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKA].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKA].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKA].MinLimitValue = 0.0d;

            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKB].Unit = "cnt";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKB].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKB].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKB].MinLimitValue = 0.0d;

            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKB].Unit = "cnt";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKB].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKB].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRDARKB].MinLimitValue = 0.0d;

            this._msrtResult[(int)EVLROptiMsrtType.VLRINTSS].Unit = "ms";
            this._msrtResult[(int)EVLROptiMsrtType.VLRINTSS].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRINTSS].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRINTSS].MinLimitValue = 0.0d;


            this._msrtResult[(int)EVLROptiMsrtType.VLRMFILA].Unit = "mA";
            this._msrtResult[(int)EVLROptiMsrtType.VLRMFILA].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRMFILA].MaxLimitValue = 2000.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRMFILA].MinLimitValue = 0.0d;

            this._msrtResult[(int)EVLROptiMsrtType.VLREWATT].Unit = "W";
            this._msrtResult[(int)EVLROptiMsrtType.VLREWATT].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLREWATT].MaxLimitValue = 10.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLREWATT].MinLimitValue = 0.0d;

            this._msrtResult[(int)EVLROptiMsrtType.VLRLE].Unit = "lm/W";
            this._msrtResult[(int)EVLROptiMsrtType.VLRLE].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRLE].MaxLimitValue = 99999.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRLE].MinLimitValue = 0.0d;

            this._msrtResult[(int)EVLROptiMsrtType.VLRWPE].Unit = "%";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWPE].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRWPE].MaxLimitValue = 100.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRWPE].MinLimitValue = 0.0d;

            this._msrtResult[(int)EVLROptiMsrtType.VLRDuv].Unit = "";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDuv].Formate = "0.000";
            this._msrtResult[(int)EVLROptiMsrtType.VLRDuv].MaxLimitValue = 100.0d;
            this._msrtResult[(int)EVLROptiMsrtType.VLRDuv].MinLimitValue = 0.0d;

            for (int k = (int)EVLROptiMsrtType.VLRR01; k < (int)EVLROptiMsrtType.VLRR15; k++)
            {
                this._msrtResult[k].Unit = "";
                this._msrtResult[k].Formate = "0.0";
                this._msrtResult[k].MaxLimitValue = 100.0d;
                this._msrtResult[k].MinLimitValue = 0.0d;
            }
        }

        private void CreateTable()
        {
            double value = 0.0d;

            if (this._coefStartWL >= this._coefEndWL || this._coefStartWL < 0 || this._coefEndWL < 0)
            {
                return;
            }

            if (this._coefWLResolution < 0)
            {
                return;
            }

            UInt32 items = (UInt32)Math.Floor((this._coefEndWL - this._coefStartWL) / this._coefWLResolution) + 1;

            this._coefTable = new double[items][];

            for (int i = 0; i < items; i++)
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

            string[] str = Enum.GetNames(typeof(EVLROptiMsrtType));

            // Reset Electrical Setting KeyName
            this._elecSetting[0].KeyName = "IFVLR_" + num.ToString();

            // Reset Tested Result KeyName and Gain Offset Seeting KeyName
            for (int i = 0; i < this._msrtResult.Length; i++)
            {
                if (this._msrtResult[i] == null)
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

        public void CreateCoefTable(double startWL, double endWL, double resolution)
        {
            this._coefStartWL = startWL;
            this._coefEndWL = endWL;
            this._coefWLResolution = resolution;
            this.CreateTable();
        }

        #endregion
    }
}
