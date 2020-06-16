using System;

namespace MPI.Tester.DeviceCommon
{
    public class OptiData : ICloneable
    {
        public const int CRI_SAMPLES_COUNT = 15;

        private object _lockObj;

        private double _CIE1931X;
        private double _CIE1931Y;
        private double _CIE1931Z;
        private double _CIE1931x;
        private double _CIE1931y;
        private double _CIE1931z;
        private double _u_prime;
        private double _v_prime;
        private double _purity;
        private double _CCT;

        private double _WLP;
        private double _WLP2;
        private double _WLPNIR;
        private double _WLD;
        private double _WLCv;
        private double _WLCp;
        private double _FWHM;

        private double _watt;
        private double _Lm;
        private double _Lx;

        private double _generalCRI;
        private double[] _specialCRI;
        private double _colorDelta;

        private double _IntegralTime;
        private uint _maxCount;

        private double _ratio;
        private double _countPercent;
		private int _triggerStatus;
		private int _darkAvg;
		private int _darkSTDev;
		private int _chipDarkAvg;
        private double _deltaWLP;
        private double _triggerTime;
		////-------------------------------------------------
		//// Extend measured data for USB2000P 
		////-------------------------------------------------
 
		//private int _boxCar;
		//private int _scanAverage;

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptiData()
        {
            this._lockObj = new object();

            this._CIE1931X = 0.0d;
            this._CIE1931Y = 0.0d;
            this._CIE1931Z = 0.0d;
            this._CIE1931x = 0.0d;
            this._CIE1931y = 0.0d;
            this._CIE1931z = 0.0d;
            this._u_prime = 0.0d;
            this._v_prime = 0.0d;
            this._purity = 0.0d;
            this._CCT = 0.0d;

            this._WLP = 0.0d;
            this._WLP2 = 0.0d;
            this._WLPNIR = 0.0d;
            this._WLD = 0.0d;
            this._WLCv = 0.0d;
            this._WLCp = 0.0d;
            this._FWHM = 0.0d;

            this._watt = 0.0d;
            this._Lx = 0.0d;
            this._Lm = 0.0d;

            this._generalCRI = 0.0d;
            this._specialCRI = new double[CRI_SAMPLES_COUNT];
            this._colorDelta = 0.0d;

            this._IntegralTime = 0.0d;
            this._maxCount = 0;
            this._countPercent = 0.0d;
            this._deltaWLP = 0.0d;
            this._triggerTime = 0.0d;
        }

        #region >>> Public Property <<<

        /// <summary>
        /// Chromaticity X by CIE 1931
        /// </summary>
        public double CIE1931X
        {
            get { return this._CIE1931X; }
            set { lock (this._lockObj) { this._CIE1931X = value; } }
        }

        /// <summary>
        /// Chromaticity Y by CIE 1931
        /// </summary>
        public double CIE1931Y
        {
            get { return this._CIE1931Y; }
            set { lock (this._lockObj) { this._CIE1931Y = value; } }
        }

        /// <summary>
        /// Chromaticity Z by CIE 1931
        /// </summary>
        public double CIE1931Z
        {
            get { return this._CIE1931Z; }
            set { lock (this._lockObj) { this._CIE1931Z = value; } }
        }
        /// <summary>
        /// Chromaticity x by CIE 1931
        /// </summary>
        public double CIE1931x
        {
            get { return this._CIE1931x; }
            set { lock (this._lockObj) { this._CIE1931x = value; } }
        }

        /// <summary>
        /// Chromaticity y by CIE 1931
        /// </summary>
        public double CIE1931y
        {
            get { return this._CIE1931y; }
            set { lock (this._lockObj) { this._CIE1931y = value; } }
        }

        /// <summary>
        /// Chromaticity z by CIE 1931
        /// </summary>
        public double CIE1931z
        {
            get { return ( 1.0d - this.CIE1931x - this.CIE1931y); }
            set { lock (this._lockObj) { this._CIE1931z = value; } }
        }

        /// <summary>
        /// u_prime
        /// </summary>
        public double u_prime
        {
            get { return (this._u_prime); }
            set { lock (this._lockObj) { this._u_prime = value; } }
        }

        /// <summary>
        /// v_prime
        /// </summary>
        public double v_prime
        {
            get { return (this._v_prime); }
            set { lock (this._lockObj) { this._v_prime = value; } }
        }

        /// <summary>
        /// Purity
        /// </summary>
        public double Purity
        {
            get { return this._purity; }
            set { lock (this._lockObj) { this._purity = value; } }
        }

        /// <summary>
        /// Correlated Color Temperature
        /// </summary>
        public double CCT
        {
            get { return this._CCT; }
            set { lock (this._lockObj) { this._CCT = value; } }
        }

        /// <summary>
        /// Peak wavelength
        /// </summary>
        public double WLP
        {
            get { return this._WLP; }
            set { lock (this._lockObj) { this._WLP = value; } }
        }

        /// <summary>
        /// Seconde Peak wavelength
        /// </summary>
        public double WLP2
        {
            get { return this._WLP2; }
            set { lock (this._lockObj) { this._WLP2 = value; } }
        }

        /// <summary>
        /// Peak wavelength at NIR
        /// </summary>
        public double WLPNIR
        {
            get { return this._WLPNIR; }
            set { lock (this._lockObj) { this._WLPNIR = value; } }
        }

        /// <summary>
        /// Dominant wavelength
        /// </summary>
        public double WLD
        {
            get { return this._WLD; }
            set { lock (this._lockObj) { this._WLD = value; } }
        }

        /// <summary>
        /// Centroid Wavelength at vision spectrum
        /// </summary>
        public double WLCv
        {
            get { return this._WLCv; }
            set { lock (this._lockObj) { this._WLCv = value; } }
        }

        /// <summary>
        /// Centroid Wavelength at power spectrum
        /// </summary>
        public double WLCp
        {
            get { return this._WLCp; }
            set { lock (this._lockObj) { this._WLCp = value; } }
        }

        /// <summary>
        /// Full width at half maximum
        /// </summary>
        public double FWHM
        {
            get { return this._FWHM; }
            set { lock (this._lockObj) { this._FWHM = value; } }
        }

        /// <summary>
        /// Radiant flux, SI unit : Watt (W)
        /// </summary>
        public double Watt
        {
            get { return this._watt; }
            set { lock (this._lockObj) { this._watt = value; } }
        }

        /// <summary>
        /// Luminous flux, SI unit : Lumen (lm)
        /// </summary>
        public double Lm
        {
            get { return this._Lm; }
            set { lock (this._lockObj) { this._Lm = value; } }
        }

        /// <summary>
        /// Illuminance, SI uint : lux, (lx ) , (lm/m^2) 
        /// </summary>
        public double Lx
        {
            get { return this._Lx; }
            set { lock (this._lockObj) { this._Lx = value; } }
        }

        /// <summary>
        /// General Color Rendering Index ( Ra ), 1 ~ 8 test samples
        /// </summary>
        public double GeneralCRI
        {
            get { return this._generalCRI; }
            set { lock (this._lockObj) { this._generalCRI = value; } }
        }

        /// <summary>
        /// Special Color Rendering Index ( Ri ), 14 test samples
        /// </summary>
        public double[] SpecialCRI
        {
            get { return this._specialCRI; }
            set { lock (this._lockObj) { this._specialCRI = value; } }
        }

        /// <summary>
        /// Color Delta >0.0054  CRI=0
        /// </summary>
        public double ColorDelta
        {
            get { return this._colorDelta; }
            set { lock (this._lockObj) { this._colorDelta = value; } }
        }

        /// <summary>
        /// Integration time for specrtometer setting
        /// </summary>
        public double IntegralTime
        {
            get { return this._IntegralTime; }
            set { lock (this._lockObj) { this._IntegralTime = value; } }
        }

        /// <summary>
        /// Max. Count of spectrum data 
        /// </summary>
        public uint MaxCount
        {
            get { return this._maxCount; }
            set { lock (this._lockObj) { this._maxCount = value; } }
        }

		/// <summary>
		/// Ratio of count and integration time 
		/// </summary>
		public double Ratio
		{
			get { return this._ratio; }
			set { lock (this._lockObj) { this._ratio = value; } }
		}


		/// <summary>
		/// Ratio of count and integration time 
		/// </summary>
		public double CountPercent
		{
			get { return this._countPercent; }
			set { lock (this._lockObj) { this._countPercent = value; } }
		}

		/// <summary>
		/// Status of Spectrometer Trigger
		/// </summary>
		public int TriggerStatus
		{
			get { return this._triggerStatus; }
			set { lock (this._lockObj) { this._triggerStatus = value; } }
		}

		/// <summary>
		/// Average of Dark Intensity Array
		/// </summary>
		public int DarkAvg
		{
			get { return this._darkAvg; }
			set { lock (this._lockObj) { this._darkAvg = value; } }
		}

		/// <summary>
		/// Standard Deviation of Dark Intensity Array
		/// </summary>
		public int DarkSTDev
		{
			get { return this._darkSTDev; }
			set { lock (this._lockObj) { this._darkSTDev = value; } }
		}

		public int ChipDarkAvg
		{
			get { return this._chipDarkAvg; }
			set { lock (this._lockObj) { this._chipDarkAvg = value; } }
		}

        public double DeltaWLP
        {
            get { return this._deltaWLP; }
            set { lock (this._lockObj) { this._deltaWLP = value; } }
        }

        public double TriggerTime
        {
            get { return this._triggerTime; }
            set { lock (this._lockObj) { this._triggerTime = value; } }
        }

		////-------------------------------------------------
		//// Extend measured data for USB2000P 
		////-------------------------------------------------

		//public int BoxCar
		//{
		//    get { return this._boxCar; }
		//    set { lock (this._lockObj) { this._boxCar = value; } }
		//}

		//public int ScanAverage
		//{
		//    get { return this._scanAverage; }
		//    set { lock (this._lockObj) { this._scanAverage = value; } }
		//}

		#endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


    }
}
