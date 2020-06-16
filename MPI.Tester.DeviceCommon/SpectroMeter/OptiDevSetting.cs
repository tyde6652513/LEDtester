using System;
using System.Collections.Generic;
using System.Text;


namespace MPI.Tester.DeviceCommon
{
	[Serializable]
    public class OptiDevSetting : System.ICloneable
    {   
		private const int CCD_PIXEL_LENGTH = 2048;  
		private object _lockObj;

		private ESpectrometerOpMode _operationMode;

		//private int _simulationMode;
		//private int _simulationLevel;

		private int _autoLowCount;
		private int _autoHighCount;

		private int _limitLowCount;
		private int _limitHighCount;
		private int _limitTargetCount;
        
		private double _limitStartTime;

		private int _yAxisResolution;           // 2^16 = 65536
		private int _xAxisResolution;           // 2^11 = 2048

		private bool _isGetRawData;
		private bool _isFitWeiminData;
		private bool _isModifyWave;

		private bool _isCalcCRIData;
		private int _intensityDataMode;

		private uint _attenuatorPos;

		private bool _isEnableTrigger;
		private bool _isEnableCalc;

		//---------------------------------------------------------------------------
		// Extend device setting parameters for USB2000P 
		//---------------------------------------------------------------------------
		private bool _isCorrectForNonlinearity;
		private int _boxCar;
		private int _scanAverage;
		private double _minCatcPeakCount;
		private ECIEObserver _cieObserver;
		private ECIEilluminant _cieIlluminant;
		private bool _isUseSphere;
		private double _surfaceAreaCmSquared;		
		private double[] _darkIntensity;
		private double _lumemCoeff;
		private double _uWattCoeff;
		private uint _startWavelength;
		private uint _endWavelength;

		//private int _integratingTime;
        private bool _isEnableAutoGetDark;
		private bool _isEnableHardwareTrigger;
		private bool _isEnableCorrectForDark;
        private bool _isFilterAbsSpectrum;

		private ESptFilterMode _sptFilterMode;
		private EDarkCorrectMode _darkCorrectMode;

        private bool _isUseProductXaxisCoeff;
        private bool _isUseProductYaxisCoeff;
        private bool _isUseProductYaxisWeight;
        private double[] _sptXaxisCoefficient;
        private double[] _SptYaxisCalibArray;
        private double[] _SptYaxisWeightArray;
        private bool _isUseNDFilterRatio;
        private bool _isUseAbsCorrection;
		private bool _isCalcCCTByCaliCIExy;
        private int _vLamdaType;
        private double _baseNoise;
        private bool _isUseShelter;	  

        private bool _isUseMPISpam2;
        private bool _isCalcSpecialWLP;
        private double _calcWpecialWLPPlace;

        private ESpectrometerInterfaceType _sptInterfaceType;

        private ESpectrometerCalibDataMode _sptCalibMode;

		private int _limit02MinSTTime;

        private int _limit02PeakPercent;

        private uint _limit02TurnOffTime;

		private bool _isEnableLoadWavelenghtCoefficent;
        /// <summary>
        /// Constructor
        /// </summary>
        public OptiDevSetting()
        {
            this._lockObj = new object();

            this._operationMode = ESpectrometerOpMode.Filter;

			//this._simulationMode = 0;
			//this._simulationLevel = 0;

            this._autoLowCount = 30000;
            this._autoHighCount = 60000;

            this._limitLowCount = 30000;
            this._limitHighCount = 60000; 
            this._limitTargetCount = ( this._limitLowCount + this._limitHighCount ) / 2 ;
            
            this._limitStartTime = 10.0d;
            this._yAxisResolution = 65536;
            this._xAxisResolution = 2048;

            this._isFitWeiminData = false;
            this._isGetRawData = false;

            this._isCalcCRIData = false;
            this._intensityDataMode = 1;    // FOR SMUSB spectometer // 0 : from trigger; 1 : raw data; 2 : multiply gain; 3 : filter process

            this._attenuatorPos = 0;

			this._isEnableTrigger = true;
			this._isEnableCalc = true;

			//---------------------------------------------------------------------------------
			// Extend device setting parameters for USB2000P 
			//---------------------------------------------------------------------------------
		    this._isCorrectForNonlinearity = true;
            this._boxCar = 5;
			this._scanAverage = 1;
			this._minCatcPeakCount = 5000;
			this._surfaceAreaCmSquared = 1;     
			this._isUseSphere = false;
			this._darkIntensity = new double[CCD_PIXEL_LENGTH];
            this._lumemCoeff = 1;
            this._uWattCoeff = 1;
            this._startWavelength = 380;
            this._endWavelength = 780;
            this._isEnableAutoGetDark = true;
			this._isEnableHardwareTrigger = false;

			this._cieObserver = ECIEObserver.Ob_1931;
			this._cieIlluminant = ECIEilluminant.E;
			this._isEnableCorrectForDark = false;
            this._isFilterAbsSpectrum = false;

			this._sptFilterMode = ESptFilterMode.Normal;
			this._darkCorrectMode = EDarkCorrectMode.Normal;

            this._isUseProductXaxisCoeff = false;
            this._isUseProductYaxisCoeff = false;
            this._isUseProductYaxisWeight = false;
            this._sptXaxisCoefficient = new double[4];
            this._SptYaxisCalibArray = new double[CCD_PIXEL_LENGTH];
            this._SptYaxisWeightArray = new double[CCD_PIXEL_LENGTH];
            this._isUseNDFilterRatio = false;
            this._isUseAbsCorrection = false;
            this._vLamdaType = 0;
            this._baseNoise = 0.1d;
 			this._isUseShelter = true;

            this._isUseMPISpam2 = false;

            this._isCalcSpecialWLP = false;

            this._calcWpecialWLPPlace = 90.0d;

            this._sptInterfaceType = ESpectrometerInterfaceType.InterfaceUSB;

            this._sptCalibMode = ESpectrometerCalibDataMode.IntegratingSphere;

			this._limit02MinSTTime = 5;

            this._limit02PeakPercent = 80;

            this._limit02TurnOffTime = 100;

			this._isEnableLoadWavelenghtCoefficent = false;
        }

        #region >>> Public Property <<<

        public ESpectrometerOpMode OperationMode
        {
            get { return this._operationMode; }
            set { lock (this._lockObj) { this._operationMode = value; } }
        }
        
        public int AutoLowCount
        {
            get { return this._autoLowCount; }
			set
			{
				lock (this._lockObj)
				{
					if (value > (this._autoHighCount - 1000))
					{
						this._autoLowCount = this._autoHighCount - 1000;
					}
					this._autoLowCount = value;
				}
			}
			
        }
        
        public int AutoHighCount
        {
            get { return this._autoHighCount; }
			set
			{
				lock (this._lockObj)
				{
					if (value < (this._autoLowCount + 1000 ))
					{
						this._autoHighCount = this._autoLowCount + 1000;
					}
					else
					{
						this._autoHighCount = value;
					}
				}
			}
			
        }

        public double LimitStartTime
        {
            get { return this._limitStartTime; }
            set { lock (this._lockObj) { this._limitStartTime = value; } }
        }

        //public double LimitIntegralTime
        //{
        //    get { return this._limitIntegralTime; }
        //    set { lock (this._lockObj) { this._limitIntegralTime = value; } }
        //}

        public int LimitLowCount
        {
            get { return this._limitLowCount; }
			set
			{
				lock (this._lockObj)
				{
					if (value > (this._limitHighCount - 1000))
					{
						this._limitLowCount = this._limitHighCount - 1000;
					}
					this._limitLowCount = value;
				}
			}
        }
        
        public int LimitHighCount
        {
            get { return this._limitHighCount; }
			set
			{
				lock (this._lockObj)
				{
					if (value < (this._limitLowCount + 1000))
					{
						this._limitHighCount = this._limitLowCount + 1000;
					}
					else
					{
						this._limitHighCount = value;
					}
				}
			}
        }

        public int LimitTargetCount
        {
            get { return this._limitTargetCount; }
            set { lock (this._lockObj) { this._limitTargetCount = value; } }
        }

        public int XAxisResolution
        {
            get { return this._xAxisResolution; }
            set { lock (this._lockObj) { this._xAxisResolution = value; } }            
        }

        public int YAxisResolution
        {
            get { return this._yAxisResolution; }
            set { lock (this._lockObj) { this._yAxisResolution = value; } }
        }

        public bool IsGetRawData
        {
            get { return this._isGetRawData; }
            set { lock (this._lockObj) { this._isGetRawData = value; } }
        }

        public bool IsFitWeiminData
        {
            get { return this._isFitWeiminData; }
            set { lock (this._lockObj) { this._isFitWeiminData = value; } }
        }

        public bool IsModifyWave
        {
            get { return this._isModifyWave; }
            set { lock (this._lockObj) { this._isModifyWave = value; } }
        }

        public bool IsCalcCRIData
        {
            get { return this._isCalcCRIData; }
            set { lock (this._lockObj) { this._isCalcCRIData = value; } }
        }

        public int IntensityDataMode
        {
            get { return this._intensityDataMode; }
            set { lock (this._lockObj) { this._intensityDataMode = value; } }
        }


         public uint AttenuatorPos
         {
             get { return this._attenuatorPos; }
             set { lock (this._lockObj) { this._attenuatorPos = value; } }
         }

         public bool IsEnableTrigger
         {
             get { return this._isEnableTrigger; }
             set { lock (this._lockObj) { this._isEnableTrigger = value; } }
         }

         public bool IsEnableCalc
         {
             get { return this._isEnableCalc; }
             set { lock (this._lockObj) { this._isEnableCalc = value; } }
         }


		 //-------------------------------------------------
		 // Extend device setting parameters for USB2000P 
		 //-------------------------------------------------

        public int BoxCar
        {
            get { return this._boxCar; }
			set
			{
				lock (this._lockObj)
				{
					if (value <= 0)
					{
						this._boxCar = 0;
					}
					else
					{
						this._boxCar = value;
					}
				}
			}				 
        }

        public int ScanAverage
        {
            get { return this._scanAverage; }
			set
			{
				lock (this._lockObj)
				{
					if (value <= 1)
					{
						this._scanAverage = 1;
					}
					else
					{
						this._scanAverage = value;
					}
				}
			}
        }

        public double MinCatchPeakCount
        {
            get { return this._minCatcPeakCount; }
            set { lock (this._lockObj) { this._minCatcPeakCount = value; } }
        }

        public ECIEObserver CieObserver
        {
            get { return this._cieObserver; }
            set { lock (this._lockObj) { this._cieObserver = value; } }
        }

		public ECIEilluminant CieIlluminant
        {
            get { return this._cieIlluminant; }
            set { lock (this._lockObj) { this._cieIlluminant = value; } }
        }

		 public bool IsCorrectForNonlinearity
		 {
			 get { return this._isCorrectForNonlinearity; }
			 set { lock (this._lockObj) { this._isCorrectForNonlinearity = value; } }
		 }

		 public double SurfaceAreaCmSquared
		 {
			 get { return this._surfaceAreaCmSquared; }
			 set { lock (this._lockObj) { this._surfaceAreaCmSquared = value; } }
		 }

		 public bool IsUseSphere
		 {
			 get { return this._isUseSphere; }
			 set { lock (this._lockObj) { this._isUseSphere = value; } }
		 }

		 public double[] DarkArray
		 {
			 get { return this._darkIntensity; }
			 set { lock (this._lockObj) { this._darkIntensity = value; } }
		 }

         public double LumensCoeff
         {
             get { return this._lumemCoeff; }
             set { lock (this._lockObj) { this._lumemCoeff = value; } }
         }

         public double WattCoeff
         {
             get { return this._uWattCoeff; }
             set { lock (this._lockObj) { this._uWattCoeff = value; } }
         }

         public uint StartWavelength
         {
             get { return this._startWavelength; }
             set { lock (this._lockObj) { this._startWavelength = value; } }
         }

         public uint EndWavelength
         {
             get { return this._endWavelength; }
             set { lock (this._lockObj) { this._endWavelength = value; } }
         }

         public bool IsAutoGetDark
         {
             get { return this._isEnableAutoGetDark; }
             set { lock (this._lockObj) { this._isEnableAutoGetDark = value; } }
         }

         public bool IsExportSpectrum
         {
             get { return this._isGetRawData; }
             set { lock (this._lockObj) { this._isGetRawData = value; } }
         }

		 public bool IsEnableHardwareTrigger
		 {
			 get { return this._isEnableHardwareTrigger; }
			 set { lock (this._lockObj) { this._isEnableHardwareTrigger = value; } }
		 }

		public bool IsEnableCorrectForDark
		 {
			 get { return this._isEnableCorrectForDark; }
			 set { lock (this._lockObj) { this._isEnableCorrectForDark = value; } }
		 }

        public bool IsFilterAbsSpectrum
        {
            get { return this._isFilterAbsSpectrum; }
            set { lock (this._lockObj) { this._isFilterAbsSpectrum = value; } }
        }

		public ESptFilterMode SptFilterMode
		{
			get { return this._sptFilterMode; }
			set { lock (this._lockObj) { this._sptFilterMode = value; } }
		}

		public EDarkCorrectMode DarkCorrectMode
		{
			get { return this._darkCorrectMode; }
			set { lock (this._lockObj) { this._darkCorrectMode = value; } }
		}

        public bool IsUseProductXaxisCoeff
        {
            get { return this._isUseProductXaxisCoeff; }
            set { lock (this._lockObj) { this._isUseProductXaxisCoeff = value; } }
        }

        public bool IsUseProductYaxisCalib
        {
            get { return this._isUseProductYaxisCoeff; }
            set { lock (this._lockObj) { this._isUseProductYaxisCoeff = value; } }
        }

        public bool isUseProductYaxisWeight
        {
            get { return this._isUseProductYaxisWeight; }
            set { lock (this._lockObj) { this._isUseProductYaxisWeight = value; } }
        }

        public double[] SptXaxisCoefficientByProduct
        {
            get { return this._sptXaxisCoefficient; }
            set { lock (this._lockObj) { this._sptXaxisCoefficient = value; } }
        }

        public double[] SptYaxisCalibArrayByProduct
        {
            get { return this._SptYaxisCalibArray; }
            set { lock (this._lockObj) { this._SptYaxisCalibArray = value; } }
        }

        public double[] SptYaxisWeightArrayByProduct
        {
            get { return this._SptYaxisWeightArray; }
            set { lock (this._lockObj) { this._SptYaxisWeightArray = value; } }
        }

        public bool IsUseNDFilterRatio
        {
            get { return this._isUseNDFilterRatio; }
            set { lock (this._lockObj) { this._isUseNDFilterRatio = value; } }
        }

        public bool IsUseAbsCorrection
        {
            get { return this._isUseAbsCorrection; }
            set { lock (this._lockObj) { this._isUseAbsCorrection = value; } }
        }

		public bool IsCalcCCTByCaliCIExy
		{
			get { return this._isCalcCCTByCaliCIExy; }
			set { lock (this._lockObj) { this._isCalcCCTByCaliCIExy = value; } }
		}

        public int VLamdaType
        {
            get { return this._vLamdaType; }
            set { lock (this._lockObj) { this._vLamdaType = value; } }
        }

        public double BaseNoise
        {
            get { return this._baseNoise; }
            set { lock (this._lockObj) { this._baseNoise = value; } }
        }

         public bool IsUseShelter
		  {
			  get { return this._isUseShelter; }
			  set { lock (this._lockObj) { this._isUseShelter = value; } }
		  }

        public bool IsUseMPISpam2
        {
            get { return this._isUseMPISpam2; }
            set { lock (this._lockObj) { this._isUseMPISpam2 = value; } }
        }

        public bool IsCalcSpecialWLP
        {
            get { return this._isCalcSpecialWLP; }
            set { lock (this._lockObj) { this._isCalcSpecialWLP = value; } }
        }

        public double CalcSpecialWLPPlace
        {
            get { return this._calcWpecialWLPPlace; }
            set { lock (this._lockObj) { this._calcWpecialWLPPlace = value; } }
        }

        public ESpectrometerInterfaceType SptInterfaceType
        {
            get { return this._sptInterfaceType; }
            set { lock (this._lockObj) { this._sptInterfaceType = value; } }
        }

        public ESpectrometerCalibDataMode SpectometerCalibMode
        {
            get { return this._sptCalibMode; }
            set { lock (this._lockObj) { this._sptCalibMode = value; } }
        }

		public int Limit02MinSTTime
		{
			get { return this._limit02MinSTTime; }
			set { lock (this._lockObj) { _limit02MinSTTime = value; } }
		}

        public int Limit02PeakPercent
        {
            get { return this._limit02PeakPercent; }
            set { lock (this._lockObj) { this._limit02PeakPercent = value; } }
        }

        public uint Limit02TurnOffTime
        {
            get { return this._limit02TurnOffTime; }
            set { lock (this._lockObj) { this._limit02TurnOffTime = value; } }
        }
		
			public bool IsEnableLoadWavelenghtCoefficent
		{
			get { return this._isEnableLoadWavelenghtCoefficent; }
			set { lock (this._lockObj) { this._isEnableLoadWavelenghtCoefficent = value; } }
		}

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

}
