using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MPI.Tester.DeviceCommon;
using System.IO;

namespace MPI.Tester.Device.SpectroMeter.IS
{
    //-------------------------------------------------------------------------------
    //  Ver. 1.00 , Update Date = 20120807
    //-------------------------------------------------------------------------------
	/// <summary>
	/// IS CAS4 driver class.
	/// </summary>
	public class CAS4 : ISpectroMeter
	{
        private const bool FIRE_DLL_EXCEPTION = true;
        private const string CAS_FILE_PATH = @"C:\MPI\LEDTester\Spectrometer";


        private object _lockObj;

        private uint _ArraySensorPixelLength;

        protected string _version;
        protected string _serialNumber;

        protected OptiDevSetting _devSetting;
        protected OptiSettingData[] _msrtSetting;

        protected SpectroCaliData _spectroCaliData;

        protected OptiData[] _opticalData;

        protected EDevErrorNumber _errorNumber;

		private int _casID;

        private double[] _spectrumMax;

		private int _visibleNum;
		private int _deadNum;

		private int _maxADCValue;

		private EInterfaceType _interface;
		private int _interfaceOption;

		private string _configFile;
		private string _calibFile;

        private bool _isSaturated;

		private string _calibrationUnit;
		private string _radIntUnit;
		private string _photIntUnit;
		private double _darkCurrentAge;
		private int _criMode;
		private double _commonCRI;
		private double[] _specialCRI;

        private bool _initSuccessful;

        protected uint _intTimeMin;		// in ms
        protected uint _intTimeMax;

        private double[] _wavelengthArray;
        private double[] _currentIntensityArray;
        private double[] _darkIntensityArray;
        private double[][] _recordIntensityArray;
        private double[][] _absoluteSepctrum;

        private double[] _densityFilterPreviousPosition;
        private double[] _densityFilterCurrentPosition;
        private string[] _densityFilterPreviousName;
        private string[] _densityFilterCurrentName;
        private bool _canCheckDensityFilter;
        private bool _canCheckShutter;
        private double[][] _normalizedSpectrum;
        private double[] _normalization;

        double a = 0;

        private SpectrometerHWSetting _hardwareSetting;


		/// <summary>
		/// Constructor.
		/// </summary>
		public CAS4(SpectrometerHWSetting HWSetting)
		{
            _casID = -1;

            //_spectrumMax = 0;
            _visibleNum = 0;
            _deadNum = 0;

            _intTimeMin = 0;
            _intTimeMax = 0;

            _maxADCValue = -1;

            _isSaturated = false;

            _interface = EInterfaceType.InterfacePCI;
            _interfaceOption = 0;

            _calibrationUnit = "None";
            _radIntUnit = "None";
            _photIntUnit = "None";
            _darkCurrentAge = 0.0;

            _criMode = 1;
            _commonCRI = 0;
            _specialCRI = new double[16];

            _configFile = "None";
            _calibFile = "None";

            _serialNumber = "None";

            //_secondCentralMoment = 0;
            //_thirdCentralMoment = 0;

            _initSuccessful = false;
            //_performSpectrumMeasurement = -1;
            //_getSpectrumSuccessful = false;
            //_getResultsSuccessful = false;

            _densityFilterPreviousPosition = null;
            _densityFilterCurrentPosition = null;
            _densityFilterPreviousName = null;
            _densityFilterCurrentName = null;

            _canCheckDensityFilter = false;
            _canCheckShutter = false;

            _normalizedSpectrum = null;

            _spectrumMax = null;
            _normalization = null;

            _hardwareSetting = HWSetting;

		}

		#region >>> Public Property  <<<

        public string SerialNumber
		{
            get { return this._serialNumber; }
		}

        /// <summary>
        /// Version.
        /// </summary>
        public string Version
		{
            get { return this._version; }
		}

        /// <summary>
        /// Error number.
        /// </summary>
        public EDevErrorNumber ErrorNumber
		{
            get { return this._errorNumber; }
		}

        /// <summary>
        /// Data.
        /// </summary>
        public OptiData[] Data
		{
            get { return this._opticalData; }
		}

        /// <summary>
        /// Pixel array length.
        /// </summary>
        public uint ArraySensorPixelLength
		{
            get { return this._ArraySensorPixelLength; }
		}

        public double[] DarkIntensityArray
		{
            get { return this._darkIntensityArray; }
		}


		#endregion

		#region >>> Private Property <<<

		#endregion

		#region >>> Public  Method <<<
        //----------------------------------------------------------------------------
        // the functions inherited from the ISpectrometer
        //----------------------------------------------------------------------------
        public  bool Init(int deviceNum, string spectroMeterSN, string sphereSN)
		{
            //2012.10.26 Weiting
            if (_initSuccessful)
                return true;

            _initSuccessful = false;

            this.Close();

            _calibFile = Path.Combine(CAS_FILE_PATH, sphereSN + ".isc");
            _configFile = Path.Combine(CAS_FILE_PATH, spectroMeterSN + ".ini");

            if (!File.Exists(this._calibFile) || !File.Exists(this._configFile))
		    {
                _errorNumber = EDevErrorNumber.SpectrometerSN_Err;
                return false;
		    }

            // Set Spectometer Interface

              _interface = EInterfaceType.InterfaceUSB;

            if (this._hardwareSetting != null)
            {
                _interface = (EInterfaceType)Enum.Parse(typeof(ESpectrometerInterfaceType), this._hardwareSetting.SptInterfaceType.ToString(), true);	
            }

            //_calibFile = @"C:\MPI\LEDTester\Data\CAS 140BCT Calibration Files\2355142i1.isc";
            //_configFile = @"C:\MPI\LEDTester\Data\CAS 140BCT Calibration Files\2355142i1.";
           // _interface = EInterfaceType.InterfaceUSB;
            //_interfaceOption = 235514212;
            _interfaceOption = CAS4Wrapper.DeviceHandling.GetDeviceTypeOption(_interface, 0);

            try
		{
                // create device handle
                _casID = CAS4Wrapper.DeviceHandling.CreateDeviceEx(_interface, _interfaceOption);
                if (!this.CheckCASError(_casID))
		{
                    _errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
                    return false;
		}

                // setup config, calib, and initialize
                if (!CheckCASError(CAS4Wrapper.InstrumentProperty.SetDeviceParameterString(_casID, EDeviceParameter.dpidConfigFileName, _configFile)))
		{
                    _errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
                    return false;
		}

                if (!CheckCASError(CAS4Wrapper.InstrumentProperty.SetDeviceParameterString(_casID, EDeviceParameter.dpidCalibFileName, _calibFile)))
		{
                    _errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
                    return false;
		}

                if (!CheckCASError(CAS4Wrapper.DeviceHandling.Initialize(_casID, EInitializationPerform.InitForced)))
		{
                    _errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
                    return false;
		}

                // retrieve the max integration time and the min integration time
                _intTimeMin = (uint)Math.Round(CAS4Wrapper.InstrumentProperty.GetDeviceParameter(_casID, EDeviceParameter.dpidIntTimeMin));
                _intTimeMax = (uint)Math.Round(CAS4Wrapper.InstrumentProperty.GetDeviceParameter(_casID, EDeviceParameter.dpidIntTimeMax));

                // retrieve the serial number
                string text;

                if (CheckCASError(CAS4Wrapper.InstrumentProperty.GetDeviceParameterString(_casID, EDeviceParameter.dpidSerialNo, out text)))
		{
                    _serialNumber = text;
		}
                else
		{
                    _errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
                    return false;
		}

                // retrieve the number of visible pixels
                _visibleNum = (int)Math.Round(CAS4Wrapper.InstrumentProperty.GetDeviceParameter(_casID, EDeviceParameter.dpidVisiblePixels));

                // retrieve the number of dead pixels
                _deadNum = (int)Math.Round(CAS4Wrapper.InstrumentProperty.GetDeviceParameter(_casID, EDeviceParameter.dpidDeadPixels));

                this.SetArraySensorPixelLength((uint)(_visibleNum + _deadNum));

                // initial data array
                this.InitDataArray(1);

                // retrieve the wavelength
                this.GetWavelengthFromMeter();
		
                // check whether we can check the density filter positon and the shutter position
                _canCheckDensityFilter = CheckDeviceOption(EOptionConstants.coGetFilter);
                _canCheckShutter = CheckDeviceOption(EOptionConstants.coGetShutter);

                // specify the initialization status
                _initSuccessful = true;
            }
            catch (Exception ex)
            {
                this.Close();

                _errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
            }

            return _initSuccessful;
        }

        public  void Close()
        {
            if (_casID >= 0)
                this.CheckCASError(CAS4Wrapper.DeviceHandling.DoneDevice(_casID));

            _casID = -1;
		}

        public  bool SetConfigToMeter(OptiDevSetting devSetting)
		{
            _devSetting = devSetting;

           // this.SetDensityFilter(_msrtSetting[settingIndex].DensityFilterPostion);
		
            this.SetDensityFilter((double)this._devSetting.AttenuatorPos);
            //this.GetDarkSample(1, 10000);
            return true;
		}

        public  bool SetParamToMeter(OptiSettingData[] msrtSetting)
		{
            if (msrtSetting == null)
                return false;

            _msrtSetting = msrtSetting;
		
            this.InitDataArray(_msrtSetting.Length);
            this.GetWavelengthFromMeter();
            return true;
		}

        public  int Trigger(uint settingIndex)
		{
            if (_msrtSetting == null)
                return -1;
		
            if (settingIndex >= _msrtSetting.Length)
                return -1;

            if (_msrtSetting[settingIndex].SensingMode == ESensingMode.Fixed)
            {
                // check the previous density filter position
              //  this.SetDensityFilter(_msrtSetting[settingIndex].DensityFilterPostion);
		
                //_densityFilterPreviousPosition[settingIndex] = CAS4Wrapper.Measurement.GetMeasurementParameter( _casID, EMeasurementParameters.mpidDensityFilter );
                //CheckCASError();
                //CAS4Wrapper.ShutterAndFilter.GetFilterName( _casID, ( int ) _densityFilterPreviousPosition[settingIndex], out _densityFilterPreviousName[settingIndex] );
                //CheckCASError();

                uint intTime = (uint)(_msrtSetting[settingIndex].FixIntegralTime);
		
                if (!this.MeasureSpectrum(settingIndex, intTime))
                    return -1;

                //// check the density filter position after the measurement
                //_densityFilterCurrentPosition[settingIndex] = CAS4Wrapper.Measurement.GetMeasurementParameter( _casID, EMeasurementParameters.mpidCurrentDensityFilter );
                //CheckCASError();
                //CAS4Wrapper.ShutterAndFilter.GetFilterName( _casID, ( int ) _densityFilterCurrentPosition[settingIndex], out _densityFilterCurrentName[settingIndex] );
                //CheckCASError();

		}
            else if (_msrtSetting[settingIndex].SensingMode == ESensingMode.Limit)
		{
		}


            // get spectrum from meter
            if (!this.GetSpectrumFromMeter(settingIndex))
                return -1;

            // calculate result
            if (!this.CalculateMeasureResults(settingIndex))
                return -1;

            return 10;
		}

		public bool CalculateParameters( uint settingIndex )
		{


				return true;
		}

		public double[] GetXWavelength()
		{
            return _wavelengthArray;
		}

		public double[] GetYSpectrumIntensity( uint index )
		{
            if (index < _recordIntensityArray.Length)
                return _recordIntensityArray[index];
            else
                return null;
		}

		public double[][] GetYSpectrumIntensityAll()
		{
            return _recordIntensityArray;
		}

		public double[] GetYAbsoluateSpectrum( uint index )
		{
            if (index < _absoluteSepctrum.Length)
                return _absoluteSepctrum[index];
            else
                return null;
		}

		public double[][] GetYAbsoluateSpectrumAll()
		{
            return _absoluteSepctrum;
		}

		public double[] GetDarkSample( uint Darkcount, uint intTime )
		{
            Darkcount = 1;
            double[] averageDarkCurrent = new double[_darkIntensityArray.Length];
			double[] tempDarkCurrent;

            intTime = (uint)(intTime * 0.001);

            if (intTime > _intTimeMax)
            {
                intTime = _intTimeMax;
            }
            else if (intTime < _intTimeMin)
            {
                intTime = _intTimeMin;
            }

			// initialize averageDarkCurrent
            for (int i = 0; i < averageDarkCurrent.Length; i++)
				averageDarkCurrent[ i ] = 0;

            // set integral time
            this.SetIntTime(intTime);

			for ( int i = 0; i < ( int ) Darkcount; i++ )
			{
                //2012.10.26 Weiting
                // measure dark current but not use shutter
                this.MeasureDarkCurrent(true); //ture = use shelter, false = keep shelter open

                // get dark current array
                tempDarkCurrent = this.GetDarkCurrentFromMeter();

                for (int j = 0; j < averageDarkCurrent.Length; j++)
                    averageDarkCurrent[j] += tempDarkCurrent[j];
			}

            for (int i = 0; i < averageDarkCurrent.Length; i++)
				averageDarkCurrent[ i ] = averageDarkCurrent[ i ] / Darkcount;

			return averageDarkCurrent;
		}

		public string[] GetEPPROMConfigData()
		{
            return new string[0];
		}


		#endregion

		#region >>> Private  Method <<<


        private void SetArraySensorPixelLength(uint length)
			{
            this._ArraySensorPixelLength = length;
		}

        private double[] CreatePixelArray()
		{
            return new double[this.ArraySensorPixelLength];
		}

        private void InitDataArray(int settingCount)
		{
            // wave length array
            _wavelengthArray = CreatePixelArray();

            // current intensity array
            _currentIntensityArray = CreatePixelArray();

            // dark intensity array
            _darkIntensityArray = CreatePixelArray();

            // intensity array
            _recordIntensityArray = new double[settingCount][];

            for (int i = 0; i < _recordIntensityArray.Length; i++)
                _recordIntensityArray[i] = CreatePixelArray();

            // absolute spectrum array
            _absoluteSepctrum = new double[settingCount][];

            for (int i = 0; i < _absoluteSepctrum.Length; i++)
                _absoluteSepctrum[i] = CreatePixelArray();

            // optical data array
            _opticalData = new OptiData[settingCount];

            for (int i = 0; i < _opticalData.Length; i++)
                _opticalData[i] = new OptiData();

            // max spectrum arrary
            _spectrumMax = new double[settingCount];

            // previous density filter positions
            _densityFilterPreviousPosition = new double[settingCount];
            _densityFilterPreviousName = new string[settingCount];
            // current density filter positions
            _densityFilterCurrentPosition = new double[settingCount];
            _densityFilterCurrentName = new string[settingCount];

            // normalization array
            _normalization = new double[settingCount];

            // normalized spectrum array
            _normalizedSpectrum = new double[settingCount][];
            for (int i = 0; i < _normalizedSpectrum.Length; i++)
                _normalizedSpectrum[i] = CreatePixelArray();
		}

        private bool CheckCASError(int AError, out string msg)
			{
            msg = null;

            if (AError < (int)EISCASError.errCASOK)
		{
                string message;

                CAS4Wrapper.ErrorHandling.GetErrorMessage(AError, out message);

                msg = message;

                if (FIRE_DLL_EXCEPTION)
                    this.FireCASErrorException(AError, msg);

                return false;
            }

            return true;
		}

        private bool CheckCASError(int AError)
        {
            if (AError < (int)EISCASError.errCASOK)
		{
                if (FIRE_DLL_EXCEPTION)
                    this.FireCASErrorException(AError);

                return false;
		}

            return true;
		}

        private bool CheckCASError()
		{
            return CheckCASError(CAS4Wrapper.ErrorHandling.GetError(_casID));
		}

        private void FireCASErrorException(int AError)
		{
            string msg;

            CAS4Wrapper.ErrorHandling.GetErrorMessage(AError, out msg);

            this.FireCASErrorException(AError, msg);
		}

        private void FireCASErrorException(int AError, string msg)
		{
            Console.Error.WriteLine("[CAS4] DLL error ({0}): {1}", AError, msg);
            //throw new Exception( string.Format( "CAS DLL error ({0}): {1}", AError, msg ) );
        }
			
        /// <summary>
        /// Set integration time ( ms ).
        /// </summary>
        private void SetIntTime(uint time)
        {
            CheckCASError(CAS4Wrapper.Measurement.SetMeasurementParameter(_casID, EMeasurementParameters.mpidIntegrationTime, (double)time));
		}

        private void ShutterClose()
		{
            CAS4Wrapper.ShutterAndFilter.SetShutter(_casID, 1);
            CheckCASError();
        }

        private void ShutterOpen()
			{
            CAS4Wrapper.ShutterAndFilter.SetShutter(_casID, 0);
            CheckCASError();
        }

        private void GetWavelengthFromMeter()
			{
            if (_wavelengthArray == null)
                _wavelengthArray = this.CreatePixelArray();

            for (int i = 0; i < _wavelengthArray.Length; i++)
                _wavelengthArray[i] = CAS4Wrapper.MeasurementResult.GetXArray(_casID, i);
		}

        private void MeasureDarkCurrent(bool isCloseShutter)
		{
            if (isCloseShutter)
                this.ShutterClose();

			try
			{
                int ret = CAS4Wrapper.Measurement.MeasureDarkCurrent(_casID);
                Console.WriteLine("[CAS4::MeasureDarkCurrent] MeasureDarkCurrent " + ret);
				CheckCASError();
			}
			finally
			{
                if (isCloseShutter)
                    this.ShutterOpen();
			}
		}

        private double[] GetDarkCurrentFromMeter()
		{
            double[] darkArray = this.CreatePixelArray();

			try
			{
                for (int i = 0; i < darkArray.Length; i++) // VisibleNum and DeadNum are set in Initialize() 
				{
                    darkArray[i] = CAS4Wrapper.MeasurementResult.GetDarkCurrent(_casID, i);
					CheckCASError();
				}

                return darkArray;
			}
            catch (Exception ex)
			{
		}

            return null;
		}

		private void CheckDensityFilter()
		{
            if ((int)Math.Round(CAS4Wrapper.InstrumentProperty.GetDeviceParameter(_casID, EDeviceParameter.dpidNeedDensityFilterChange)) != 0)
			{
                CAS4Wrapper.Measurement.SetMeasurementParameter(_casID, EMeasurementParameters.mpidDensityFilter, CAS4Wrapper.Measurement.GetMeasurementParameter(_casID, EMeasurementParameters.mpidNewDensityFilter));
				CheckCASError();
			}
		}

        private void CheckDarkCurrentMeasurement(uint intTime)
		{
            if ((int)Math.Round(CAS4Wrapper.InstrumentProperty.GetDeviceParameter(_casID, EDeviceParameter.dpidNeedDarkCurrent)) != 0)
			{
                this.SetIntTime(intTime);
                this.MeasureDarkCurrent(true);
			}
		}

        private void GetPeakWavelength(double startWave, double endWave,out double peakLambda,out double peakSpectrum)
		{
             peakLambda = 0;

             peakSpectrum = 0;

            //double start = CommonNumericalMethods.findNearestElementGreaterThan(this._wavelengthArray, startWave);

            //double end = CommonNumericalMethods.findNearestElementLessThan(this._wavelengthArray, endWave);

            CAS4Wrapper.Measurement.SetMeasurementParameter(_casID, EMeasurementParameters.mpidColormetricStart, startWave);

            CAS4Wrapper.Measurement.SetMeasurementParameter(_casID, EMeasurementParameters.mpidColormetricStop, endWave);

            CheckCASError(CAS4Wrapper.ClormetricCalculation.ColorMetric(_casID));

            CAS4Wrapper.MeasurementResult.GetPeak(_casID, out peakLambda, out peakSpectrum);
			}

        private bool CheckSaturation()
			{
            _maxADCValue = (int)Math.Round(CAS4Wrapper.Measurement.GetMeasurementParameter(_casID, EMeasurementParameters.mpidMaxADCValue));

            if (_maxADCValue > (int)Math.Round(CAS4Wrapper.InstrumentProperty.GetDeviceParameter(_casID, EDeviceParameter.dpidADCRange)))
                return _isSaturated = true;
            else
                return _isSaturated = false;
		}

        private bool MeasureSpectrum(uint settingIndex, uint intTime)
		{
            if (intTime > _intTimeMax)
			{
                intTime = _intTimeMax;
			}
            else if (intTime < _intTimeMin)
			{
                intTime = _intTimeMin;
			}

            // set the integration time 
            this.SetIntTime(intTime);
            this._opticalData[settingIndex].IntegralTime = intTime;
            // check the density filter
            //this.CheckDensityFilter();

            // check the dark current
            //this.CheckDarkCurrentMeasurement( intTime );

            // measure the spectrum
            CAS4Wrapper.Measurement.Measure(_casID);
            CheckCASError();

            return true;
		}

        private bool GetSpectrumFromMeter(uint settingIndex)
		{
            bool ret = false;

            _spectrumMax[settingIndex] = -1E37;

			// check for saturation
            this.CheckSaturation();

			try
			{
                for (int i = 0; i < _absoluteSepctrum[settingIndex].Length; i++) // VisibleNum and DeadNum are set in Initialize() 
				{
                    double value = CAS4Wrapper.MeasurementResult.GetData(_casID, i);
                    _absoluteSepctrum[settingIndex][i] = value * 1000000; // we need to reprensent this data in uW
					CheckCASError();
                    _spectrumMax[settingIndex] = Math.Max(_spectrumMax[settingIndex], _absoluteSepctrum[settingIndex][i]);

                    _normalization[settingIndex] = Math.Max(Math.Abs(_spectrumMax[settingIndex]), Math.Abs(_absoluteSepctrum[settingIndex][i]));
				}

				// specify the status of the spectrum retrival
                ret = true;
			}
            catch (Exception ex)
			{
			}

            return ret;
		}

        private bool CalculateMeasureResults(uint settingIndex)
        {
            if (_msrtSetting == null)
                return false;

            if (settingIndex >= _msrtSetting.Length)
                return false;

            try
            {
                string text;
                _opticalData[settingIndex].MaxCount = (uint)Math.Round(CAS4Wrapper.Measurement.GetMeasurementParameter(_casID, EMeasurementParameters.mpidMaxADCValue));
                // calibration unit
                CheckCASError(CAS4Wrapper.InstrumentProperty.GetDeviceParameterString(_casID, EDeviceParameter.dpidCalibrationUnit, out text));
                _calibrationUnit = text;

                // dark current age, unit = millisecond
                _darkCurrentAge = Math.Round(CAS4Wrapper.Measurement.GetMeasurementParameter(_casID, EMeasurementParameters.mpidLastDCAge));
                CheckCASError();

                // calculates colormetric results for the previously measured spectrum of the given device
                // to get the actual calculated results, we need to call casColorMetric

                // WLP

                double peakLambda = 0;

                double peakSpectrum = 0;

                // CAS4Wrapper.MeasurementResult.GetPeak(_casID, out peakLambda, out peakSpectrum);

                CAS4Wrapper.MeasurementResult.GetPeakWavelength(_casID, 0, 0, out peakLambda, out peakSpectrum);

                CheckCASError();

                _opticalData[settingIndex].WLP = peakLambda;


              //  CheckCASError(CAS4Wrapper.ClormetricCalculation.ColorMetric(_casID));

                double tempValue = 0;

                // radiometric integral
                CAS4Wrapper.MeasurementResult.GetRadInt(_casID, out tempValue, out text);
                CheckCASError();
                _opticalData[settingIndex].Watt = 1000 * tempValue;  // we need to represent the power in mW
                _radIntUnit = text;

                 tempValue = 0;

                // photometric integral
                CAS4Wrapper.MeasurementResult.GetPhotInt(_casID, out tempValue, out text);
                CheckCASError();

                if (text == "lm")
                {
                    _opticalData[settingIndex].Lm = tempValue;
                    _opticalData[settingIndex].Lx = 0.0d;
                    //_photIntUnit = text;

                }
                else
                {
                    _opticalData[settingIndex].Lx = tempValue * 1000;
                    _opticalData[settingIndex].Lm = 0.0d;
                    _photIntUnit = text;
                }


                // radiometric integral
                //CAS4Wrapper.MeasurementResult.get(_casID, out tempValue, out text);
                //CheckCASError();
                // _opticalData[settingIndex].Lx = tempValue / 0.002827433;  // we need to represent the power in mW
                //_radIntUnit = text;

                // centroid
                double meanLambda = 0;
                meanLambda = CAS4Wrapper.MeasurementResult.GetCentroid(_casID);
                CheckCASError();
                _opticalData[settingIndex].WLCp = meanLambda;
                _opticalData[settingIndex].WLCv = meanLambda;


                // retrieve the tristimulus
                double X = 0;
                double Y = 0;
                double Z = 0;
                CAS4Wrapper.MeasurementResult.GetTriStimulus(_casID, ref X, ref Y, ref Z);
                CheckCASError();
                _opticalData[settingIndex].CIE1931X = X;
                _opticalData[settingIndex].CIE1931Y = Y;
                _opticalData[settingIndex].CIE1931Z = Z;

                // retrieve the CIE color coordinates
                // The x and y color coordinates are necessary to calculate the dominant wavelength using cmXYToDominantWavelength
                double x = 0;
                double y = 0;
                double z = 0;
                double u = 0;
                double v1976 = 0;
                double v1960 = 0;
                CAS4Wrapper.MeasurementResult.GetColorCoordinates(_casID, ref x, ref y, ref z, ref u, ref v1976, ref v1960);
                CheckCASError();
                _opticalData[settingIndex].CIE1931x = x;
                _opticalData[settingIndex].CIE1931y = y;
                _opticalData[settingIndex].CIE1931z = z;
                _opticalData[settingIndex].u_prime = u;
                _opticalData[settingIndex].v_prime = v1976;

                // WLD and purity
                double lambdaDom = 0;
                double purity = 0;
                double illuminantRefX = 0.3333;
                double illuminantRefY = 0.3333;
                CAS4Wrapper.ClormetricCalculation.cmXYToDominantWavelength(x, y, illuminantRefX, illuminantRefY, ref lambdaDom, ref purity);
                CheckCASError();
                _opticalData[settingIndex].WLD = lambdaDom;
                _opticalData[settingIndex].Purity = purity;

                // CCT, it is mandatory to call casGetCCT before calling casCalculateCRI
                _opticalData[settingIndex].CCT = CAS4Wrapper.MeasurementResult.GetCCT(_casID);
                CheckCASError();

                #region >>> CCT and CRI test <<<
                //// retrieve the additional colormetric results
                //double CCT_additional;
                //double criCCT;
                //double ChromaticityDistance;
                //CCT_additional = CAS4Wrapper.MeasurementResult.GetCCT( _casID );
                //criCCT = CAS4Wrapper.MeasurementResult.GetExtendedColorValues( _casID, EExtendedColorValues.ecvCRICCT );
                //CheckCASError();
                //ChromaticityDistance = CAS4Wrapper.MeasurementResult.GetExtendedColorValues( _casID, EExtendedColorValues.ecvDistance );
                //CheckCASError();

                //// check the CCT choice used in chromaticity distance calculation
                //double CCT1 = _opticalData[ settingIndex ].CCT;
                //double DC1, DC2;

                //double uT1, vT1;
                //PlanckianLocusUV( CCT1, out uT1, out vT1 );
                //double uT2, vT2;
                //PlanckianLocusUV( criCCT, out uT2, out vT2 );

                //double u0 = u;
                //double v0 = v1960;

                //DC1 = Math.Pow( ( ( u0 - uT1 ) * ( u0 - uT1 ) + ( v0 - vT1 ) * ( v0 - vT1 ) ), 0.5 );
                //DC2 = Math.Pow( ( ( u0 - uT2 ) * ( u0 - uT2 ) + ( v0 - vT2 ) * ( v0 - vT2 ) ), 0.5 );

                //Console.WriteLine( "CCT1(from GetCCT()) = {0}", CCT1 );
                //Console.WriteLine( "CCT_additional(from GetCCT) = {0}", CCT_additional );
                //Console.WriteLine( "criCCT = {0}", criCCT );
                //Console.WriteLine( "u0 = {0}", u0 );
                //Console.WriteLine( "uT1 = {0}", uT1 );
                //Console.WriteLine( "uT2 = {0}", uT2 );
                //Console.WriteLine( "v0 = {0}", v0 );
                //Console.WriteLine( "vT1 = {0}", vT1 );
                //Console.WriteLine( "vT2 = {0}", vT2 );
                //Console.WriteLine( "ecvDistance = {0}", ChromaticityDistance );
                //Console.WriteLine( "DC1 = {0}", DC1 );
                //Console.WriteLine( "DC2 = {0}", DC2 );
                //Console.WriteLine();
                #endregion

                // set CRI calculation mode first, consider CRI Calculation according to "CIE 13.3 - 95" in our case
                CheckCASError(CAS4Wrapper.Measurement.SetMeasurementParameter(_casID, EMeasurementParameters.mpidCRIMode, (double)_criMode));

                if (_devSetting.IsCalcCRIData)
                {
                    // calculate CRI
                    CheckCASError(CAS4Wrapper.ClormetricCalculation.CalculateCRI(_casID));

                    // get common CRI
                    _commonCRI = CAS4Wrapper.MeasurementResult.GetCRI(_casID, 0);
                    CheckCASError();
                    _opticalData[settingIndex].GeneralCRI = _commonCRI;

                    // get special CRI
                    for (int i = 0; i < _specialCRI.Length; i++)
                    {
                        _specialCRI[i] = CAS4Wrapper.MeasurementResult.GetCRI(_casID, i + 1);
                        CheckCASError();
                    }

                    for (int i = 0; i < _opticalData[settingIndex].SpecialCRI.Length; i++)
                        _opticalData[settingIndex].SpecialCRI[i] = _specialCRI[i];

                }
          
                // FWHM
                _opticalData[settingIndex].FWHM = CAS4Wrapper.MeasurementResult.GetWidth(_casID);
                CheckCASError();
         
                // LP1

                //double peakLambda1 = 0;

                //double peakSpectrum1 = 0;

                //CAS4Wrapper.MeasurementResult.GetPeakWavelength(_casID,430,490, out peakLambda1, out peakSpectrum1);

                //_opticalData[settingIndex].LP1 = peakLambda1;

                //// LP2

                //double peakLambda2 = 0;

                //double peakSpectrum2 = 0;

                //CAS4Wrapper.MeasurementResult.GetPeakWavelength(_casID, 490, 540, out peakLambda2, out peakSpectrum2);

                //_opticalData[settingIndex].LP2 = peakLambda2;

                //// LP3

                //double peakLambda3 = 0;
                //double peakSpectrum3 = 0;

                //CAS4Wrapper.MeasurementResult.GetPeakWavelength(_casID, 540, 830, out peakLambda3, out peakSpectrum3);

                //_opticalData[settingIndex].LP3 = peakLambda3;

                //// 2nd central moment
                //_secondCentralMoment = this.CalculateCentralMoment( 2, meanLambda );
                //// 3rd central moment
                //_thirdCentralMoment = this.CalculateCentralMoment( 3, meanLambda );
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        //private double CalculateCentralMoment( int momentNumber, double mean, double[] spectrumArray )
        //{
        //    double normalization = 0;
        //    double momentInt = 0;
        //    double deltaLambda = 0;

        //    for ( int i = 0; i < spectrumArray.Length; i++ )
        //    {
        //        //calculate delta-lambda; for last pixel use difference to previous
        //        //for all others, use difference to next pixel
        //        //so called DeltaD integration as used in CAS DLL
        //        if ( i == spectrumArray.Length - 1 )
        //            deltaLambda = _wavelengthArray[ i ] - _wavelengthArray[ i - 1 ];
        //        else
        //            deltaLambda = _wavelengthArray[ i + 1 ] - _wavelengthArray[ i ];

        //        normalization += spectrumArray[ i ] * deltaLambda;
        //        momentInt += spectrumArray[ i ] * Math.Pow( _wavelengthArray[ i ] - mean, momentNumber ) * deltaLambda;
        //    }

        //    if ( !normalization.Equals( 0 ) ) // is it appropriate to compare double with int ? this syntex is from the sample code
        //        return ( momentInt / normalization );
        //    else
        //        return 0;
        //}

        private bool CheckDeviceOption(EOptionConstants option)
        {
            int optionsRetrieved = CAS4Wrapper.InstrumentProperty.GetOptions(_casID);
            int optionTarget = (int)option;

            if ((optionsRetrieved & optionTarget) > 0)
                return true;
            else
                return false;
			}

        private void CalculateNormalizedSpectrum()
        {
            for (int i = 0; i < _absoluteSepctrum.Length; i++)
                for (int j = 0; j < _absoluteSepctrum[i].Length; j++)
                    _normalizedSpectrum[i][j] = (_absoluteSepctrum[i][j] / _normalization[i]);
        }

        private void PlanckianLocusUV(double T, out double uT, out double vT)
			{
            uT = (0.860117757 + 1.54118254 * 0.0001 * T + 1.28641212 * 0.0000001 * T * T) / (1 + 8.42420235 * 0.0001 * T + 7.08145163 * 0.0000001 * T * T);

            vT = (0.317398726 + 4.22806245 * 0.00001 * T + 4.20481691 * 0.00000001 * T * T) / (1 - 2.89741816 * 0.00001 * T + 1.61456053 * 0.0000001 * T * T);
			}

        private void SetDensityFilter(double filterPosition)
        {
            //double currentDensityFilter = (CAS4Wrapper.Measurement.GetMeasurementParameter(_casID, EMeasurementParameters.mpidCurrentDensityFilter));

            //if (filterPosition != currentDensityFilter)
            //{
            //    CAS4Wrapper.Measurement.SetMeasurementParameter(_casID, EMeasurementParameters.mpidDensityFilter, filterPosition);
            //}
            //CheckCASError();

            CAS4Wrapper.Measurement.SetMeasurementParameter(_casID, EMeasurementParameters.mpidDensityFilter, filterPosition);
		}


		#endregion
	}
}
