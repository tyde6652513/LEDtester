using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SpectroMeter
{
	public class HR2000P : ISpectroMeter
	{
		[DllImport("inpout32.dll", EntryPoint = "Out32")]
		public static extern void Output(int adress, int value);
		[DllImport("inpout32.dll", EntryPoint = "Inp32")]
		public static extern void Input(int adress);

		private const int		CCD2048_PIXEL_LENGTH		= 2048;
		private const int		MIN_INTEGRATION_TIME		= 3;
		private const int		MAX_INTEGRATION_TIME		= 650;
		private const int		DARK_COUNT					= 2000;
		private const int		MAX_COUNT					= 16000;
		private const int		MIN_WAVELENGTH				= 350;
		private const int		MAX_WAVELENGTH				= 1000;
        private const int       MAX_ADC_COUNT                = 16383;

		private object _lockObj;

        private SeaBreezeWrapper _oceanWrapper;
		private string _version;
		private string _serialNum;
		private EDevErrorNumber _errorNum;

		private OptiDevSetting _devSetting;
		private OptiSettingData[] _paramSetting;
		private OptiData[] _opticalData;
		private OceanSPAM _SPAM;
        private MpiSPAM2 _mpiSPAM2;

		private double[] _recordIntTime;
		private double[] _calculatetIntTime;
		private double[][] _recordIntensityArray;
		private double[][] _absoluteSepctrum;

		private double[] _wavelengthArray;
		private double[] _currentIntensityArray;
		private double[] _darkIntensityArray;
		private double[] _caliSpectrum;
        private double[] _calibWeightArray;

        private PerformanceTimer _pt;
		private SpectroCaliData _spectroCaliData;

		private double _triggerMaxCount;
		private double _triggerSpanTime;
        private double _preTriggerSpanTime;

		private int _startIntegratePixel;
		private int _endIntegratePixel;

		private bool _isCorrectForNonlinearity = false;
		private bool _isDarkExistence = false;

		private int[] _indexPixel;
		private int _maxCountIndex;
		private double _modifyNonlinearCountRate = 1.0d;
        private bool[] _isFirstTest;
        private double _currentMcdCoeff = 1.0d;
        private double _currentMWCoeff = 1.0d;
       
		//------------------------------------------------------
		// Hardware trigger
        //------------------------------------------------------
		private bool _isHardwareTriggerMode = false;
		private Thread AcquisitionThread;
		private AutoResetEvent _eventToSM;
		private AutoResetEvent _eventToDriver;
		private MPI.Tester.Maths.Statistic _darkStat;
		private double _currentDark;
        private bool _isFileterAbsSpectrum = true;
        private bool _isSuccessLimTrigger = true;

        //------------------------------------------------------
        // Smart trigger
        //------------------------------------------------------
        private const double SMART_MODE_MIN_COUNT = 25000;
        private const double SMART_MODE_MAX_COUNT = 60000;
        private double _smartModeINTTime = 0.0d;
        private bool _smartModeisReTest = false;

        private SpectrometerHWSetting _hardwareSetting;

        private double _currentDarkCount = 0.0d;

        public HR2000P(SpectrometerHWSetting HWSetting)
		{
			this._lockObj = new object();

			this._devSetting = new OptiDevSetting();
            this._oceanWrapper = new SeaBreezeWrapper(ESpectrometerModel.HR2000P);
			this._SPAM = new OceanSPAM();
			this._opticalData = new OptiData[1] { new OptiData() }; 
            this._pt = new PerformanceTimer();

			this._version = "NONE";
			this._serialNum = "NONE";
			this._errorNum = EDevErrorNumber.Device_NO_Error;
			this._wavelengthArray = new double[CCD2048_PIXEL_LENGTH];
			this._recordIntensityArray = new double[1][] { new double[CCD2048_PIXEL_LENGTH] };
			this._currentIntensityArray = new double[CCD2048_PIXEL_LENGTH];
            this._darkIntensityArray = new double[CCD2048_PIXEL_LENGTH];
            this._recordIntTime = new double[1] { 6.0d };           
            this._calculatetIntTime = new double[1] { 6.0d };
            this._isFirstTest = new bool[1]{true};
			this._triggerMaxCount = 0.0d;
            _preTriggerSpanTime = 0.0d;
			this._startIntegratePixel = 106;
			this._endIntegratePixel = 1242;	
			//this._indexPixel = 20	;
			this._maxCountIndex = 20;
			this._darkStat = new Maths.Statistic();
			this._currentDark = 0.0d;

            this._hardwareSetting = HWSetting;

			this.Init(0, "A", "B");

            this._mpiSPAM2 = new MpiSPAM2();
            this._mpiSPAM2.StartAndInitial();
            this._mpiSPAM2.SetCIEParameter();
		}

		#region >>> Public Property <<<

		/// <summary>
		/// Spectrometer software version 
		/// </summary>
		public string Version
		{
			get { return this._version; }
		}

		/// <summary>
		/// Spectrometer serial number
		/// </summary>
		public string SerialNumber
		{
			get { return this._serialNum; }
		}

		/// <summary>
		///  Error number of spectrometer
		/// </summary>
		public EDevErrorNumber ErrorNumber
		{
			get { return this._errorNum; }
		}

		/// <summary>
		/// Photometry and colormetry data of spectrometer
		/// </summary>
		public OptiData[] Data
		{
			get { return this._opticalData; }
		}

		public double[] CaliSpectrum
		{
			get { return this._caliSpectrum; }
			set { this._caliSpectrum = value; }
		}

        public double[] DarkIntensityArray
        {
            get { return this._darkIntensityArray; }
        }

		#endregion

		#region >>> Private Method <<<

		private void ResetData(uint index, uint status)
		{
			if (index < 0 || index >= this._opticalData.Length)
				return;

			if (status == 0)
			{
				this._opticalData[index].CIE1931X = 0.0d;
				this._opticalData[index].CIE1931Y = 0.0d;
				this._opticalData[index].CIE1931Z = 0.0d;
				this._opticalData[index].CIE1931x = 0.0d;
				this._opticalData[index].CIE1931y = 0.0d;
				this._opticalData[index].CIE1931z = 1.0d;


				this._opticalData[index].Purity = 0.0d;
				this._opticalData[index].CCT = 0.0d;

				this._opticalData[index].WLP = 0.0d;
				this._opticalData[index].WLP2 = 0.0d;
				//this._opticalData[index].WLPNIR = 0.0d;
				this._opticalData[index].WLD = 0.0d;
				this._opticalData[index].WLCv = 0.0d;
				this._opticalData[index].WLCp = 0.0d;
				this._opticalData[index].FWHM = 0.0d;

				this._opticalData[index].Watt = 0.0d;
				this._opticalData[index].Lm = 0.0d;
				this._opticalData[index].Lx = 0.0d;
				this._opticalData[index].GeneralCRI = 0.0d;
			}
			else if (status == 1)
			{
				this._opticalData[index].CIE1931X = 9999.999d;
				this._opticalData[index].CIE1931Y = 9999.999d;
				this._opticalData[index].CIE1931Z = 9999.999d;
				this._opticalData[index].CIE1931x = 9999.999d;
				this._opticalData[index].CIE1931y = 9999.999d;
				this._opticalData[index].CIE1931z = 9999.999d;
				this._opticalData[index].Purity = 9999.999d;
				this._opticalData[index].CCT = 9999.999d;

				this._opticalData[index].WLP = 9999.999d;
				this._opticalData[index].WLP2 = 9999.999d;
			//	this._opticalData[index].WLPNIR = 9999.999d;
				this._opticalData[index].WLD = 9999.999d;
				this._opticalData[index].WLCv = 9999.999d;
				this._opticalData[index].WLCp = 9999.999d;
				this._opticalData[index].FWHM = 9999.999d;

				this._opticalData[index].Watt = 9999.999d;
				this._opticalData[index].Lm = 9999.999d;
				this._opticalData[index].Lx = 9999.999d;

				this._opticalData[index].GeneralCRI = 9999.999d;
			}
			else
			{
				this._opticalData[index].CIE1931X = -9999.999d;
				this._opticalData[index].CIE1931Y = -9999.999d;
				this._opticalData[index].CIE1931Z = -9999.999d;
				this._opticalData[index].CIE1931x = -9999.999d;
				this._opticalData[index].CIE1931y = -9999.999d;
				this._opticalData[index].CIE1931z = -9999.999d;
				this._opticalData[index].Purity = -9999.999d;
				this._opticalData[index].CCT = -9999.999d;

				this._opticalData[index].WLP = -9999.999d;
				this._opticalData[index].WLP2 = -9999.999d;
			//	this._opticalData[index].WLPNIR = -9999.999d;
				this._opticalData[index].WLD = -9999.999d;
				this._opticalData[index].WLCv = -9999.999d;
				this._opticalData[index].WLCp = -9999.999d;
				this._opticalData[index].FWHM = -9999.999d;

				this._opticalData[index].Watt = -9999.999d;
				this._opticalData[index].Lm = -9999.999d;
				this._opticalData[index].Lx = -9999.999d;

				this._opticalData[index].GeneralCRI = -9999.999d;
			}

            //	=============================================
            //  Paul, 20140207
            //  Clear SpecialCRI Data , u_prime ,v_prime
            //  =============================================

            for (int i = 0; i < this._opticalData[index].SpecialCRI.Length; i++)
            {
                this._opticalData[index].SpecialCRI[i] = 0;
            }

            this._opticalData[index].u_prime = 0.0d;
            this._opticalData[index].v_prime = 0.0d;
            this._opticalData[index].ColorDelta = 0.0d;
		}

		private void SetTriggerMode(int mode)
		{
			switch (mode)
			{
				case 0:
					this._oceanWrapper.setExternalTriggerMode(0, 0x83);			//Self-Hardware Trigger mode
					break;
				//------------------------------------------------------------------------------
				case 1:
					this._oceanWrapper.setExternalTriggerMode(0, 0x03);
					this._isHardwareTriggerMode = true;
					System.Threading.Thread.Sleep(100);
					this._eventToSM = new AutoResetEvent(false);
					this._eventToDriver = new AutoResetEvent(false);

					if (this.AcquisitionThread != null)
					{
						Console.Write(this.AcquisitionThread.ThreadState.ToString());
					}
					else
					{
						AcquisitionThread = new Thread(new ThreadStart(doWork));
						AcquisitionThread.Start();
						this._eventToSM.Set();
					}
					break;
				//------------------------------------------------------------------------------
				default:
					break;
			}
		}

		private double CalcNextTime(double startTime, double upBoundTime, double currentCount, double targetCount)
		{

			double nextTime = startTime;      
			double countIncreasRate = 1.0d;

			if (startTime < ((double)MIN_INTEGRATION_TIME))
			{
				startTime = (double)MIN_INTEGRATION_TIME;
			}

			if (upBoundTime > ((double)MAX_INTEGRATION_TIME))
			{
				upBoundTime = (double)MAX_INTEGRATION_TIME;
			}

			countIncreasRate = currentCount / startTime;

			nextTime = targetCount / countIncreasRate;

			if (nextTime >= upBoundTime)
			{
				nextTime = upBoundTime;
			}
			else if (nextTime <= MIN_INTEGRATION_TIME)
			{
				nextTime = (double)MIN_INTEGRATION_TIME;
			}
			else
			{
				if (nextTime >= 1)
				{
					nextTime = Math.Floor(nextTime);		//  2.1 => 2  ; 2.0 => 2  ;  1.9  => 1
				}
				else
				{
					nextTime = Math.Ceiling(nextTime);		//	0.9 => 1  ;  1.0 => 1  ;  1.2 => 2
				}
			}

			return nextTime;
		}		

        /// <summary>
        /// Calc Next Trigger Time Considering DarkCount 
        /// </summary>
        private double CalcNextTime(double startTime, double upBoundTime, double currentCount, double targetCount,double darkCount)
        {
            double nextTime = startTime;

            double countIncreasRate = 1.0d;

            if (startTime < this._devSetting.Limit02MinSTTime)
            {
				startTime = this._devSetting.Limit02MinSTTime;
            }

            if (upBoundTime > ((double)MAX_INTEGRATION_TIME))
            {
                upBoundTime = (double)MAX_INTEGRATION_TIME;
            }

            countIncreasRate = (currentCount-darkCount) / startTime;

            nextTime = (targetCount - darkCount) / countIncreasRate;

            if (nextTime >= upBoundTime)
            {
                nextTime = upBoundTime;
            }
            else if (nextTime <= MIN_INTEGRATION_TIME)
            {
                nextTime = (double)MIN_INTEGRATION_TIME;
            }
            else
            {
                if (nextTime >= 1)
                {
                    nextTime = Math.Round(nextTime);

                   // nextTime = Math.Floor(nextTime);		//  2.1 => 2  ; 2.0 => 2  ;  1.9  => 1
                }
                else
                {
                    nextTime = Math.Ceiling(nextTime);		//	0.9 => 1  ;  1.0 => 1  ;  1.2 => 2
                }
            }

            return nextTime;
        }		

		//private void CalcualteStartAndEndPixel()
		//{
		//    double startWave = (double)this._devSetting.StartWavelength;
		//    double endWave = (double)this._devSetting.EndWavelength;

		//    for (int i = 0; i < this._wavelengthArray.Length; i++)
		//    {
		//        if (startWave >= this._wavelengthArray[i] )
		//        {
		//            this._startIntegratePixel = i + 1;                  
		//        }
		//        if (endWave >= this._wavelengthArray[i] )
		//        {
		//            this._endIntegratePixel = i ;
		//        }
		//    }

		//    if (this._startIntegratePixel == this._wavelengthArray.Length)
		//    {
		//        this._startIntegratePixel = this._startIntegratePixel - 1;
		//    }
		//}

		private int CalcMaxCount(double[] array,double lowerboundValue)
		{
			int index = 11;
			//double data = this._devSetting.MinCatchPeakCount;

			for (int i = 10; i < array.Length; i++)
			{
                if (array[i] > lowerboundValue)
				{
                    lowerboundValue = array[i];
					index = i;
				}
			}
			return index;
		}

        private double CalcSpecialWLP(int peakIndex, double[] xWave, double[] yCount)
        {
            double percentage = this._devSetting.CalcSpecialWLPPlace * 0.01;

            double threshold = yCount[peakIndex] * percentage;

            double leftPoint = -1.0d;

            double rightPoint = -1.0d;

            for ( int i = peakIndex - 1 ; i >= 0; i--)
            {
                if (yCount[i] < threshold)
                {
                    leftPoint = yCount[i];

                    break;
                }
            }

            for (int i = peakIndex + 1; i < yCount.Length; i++)
            {
                if (yCount[i] < threshold)
                {
                    rightPoint = yCount[i];

                    break;
                }
            }

            if (leftPoint > 0 && rightPoint > 0)
            {
                return (leftPoint + rightPoint) / 2;
            }
            else
            {
                return 0.0d;
            }
        }

        private bool LoadBinaryCaliFile(string seriesNum)
        {
            string CaliFilePath = @"C:\MPI\LEDTester\Spectrometer\";

            string pathAndFile = Path.Combine(CaliFilePath, "CaliData_" + seriesNum + ".dat");

            if (this._hardwareSetting.SpectometerCalibMode == ESpectrometerCalibDataMode.McdModule)
            {
                pathAndFile = Path.Combine(CaliFilePath, "CaliData_" + seriesNum + "-mcd.dat");
            }
               
            if (File.Exists(pathAndFile))
            {
                try
                {
                    IFormatter binFmt = new BinaryFormatter();
                    using (Stream s = File.Open(pathAndFile, FileMode.Open))
                    {
                        this._spectroCaliData = (SpectroCaliData)binFmt.Deserialize(s);
                        s.Close();
                    }
                }
                catch
                {
                    this._spectroCaliData = null;
                }

                if (this._spectroCaliData != null)
                {
                    if (this._spectroCaliData.SeriesNum == seriesNum)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    this._spectroCaliData = new SpectroCaliData();
                    return false;
                }

            }
            else
            {
                this._spectroCaliData = new SpectroCaliData();
                return false;
            }
        }

        private bool LoadCaliFile(string seriesNum)
        {
            string CaliFilePath = @"C:\MPI\LEDTester\Spectrometer";
			string pathAndFile = Path.Combine(CaliFilePath, "CaliData_" + seriesNum + ".xml");

            if (File.Exists(pathAndFile))
            {
                this._spectroCaliData = MPI.Xml.XmlFileSerializer.Deserialize(typeof(SpectroCaliData), pathAndFile) as SpectroCaliData;

				if (this._spectroCaliData != null)
				{
                    if (this._spectroCaliData.SeriesNum == seriesNum)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }	
				}
				else
				{
                    this._spectroCaliData = new SpectroCaliData();
                    return false;
				}
            }
            else
            {
                this._spectroCaliData = new SpectroCaliData();
                return false;
            }
        }	
   
		private int LimitTrigger(uint index)
		{
			bool meterRtn = false;
			double secondIntTime = 0.0d;
            _isSuccessLimTrigger = false;

			//----------------------------------------------------------
			// (1) Firt, Trigger 
			//----------------------------------------------------------

			meterRtn = this.MeterTrigger(this._devSetting.LimitStartTime);

            _preTriggerSpanTime = this._triggerSpanTime - this._devSetting.LimitStartTime;

			if (meterRtn == false)	// MeterTrigger has error, the subfunction will set the error code
			{
				this.ResetData(index, 0);
				return -21;
			}

           // 當第二次的delta trigger Time > settig 
           // 保留是第一次trigger count

            if (_preTriggerSpanTime > 25)
            {
                _isSuccessLimTrigger = false;
                this.ResetData(index, 0);
                return -23;
            }
            else
            {
                _isSuccessLimTrigger = true;
            }

           


			if (this._devSetting.LimitStartTime != 0.0d)
			{
				this._opticalData[index].Ratio = (uint)(this._triggerMaxCount / this._devSetting.LimitStartTime);		// Count / ms
			}
			else
			{
				this._opticalData[index].Ratio = double.MaxValue;
			}

#if ( !DebugVer )
            // this._devSetting.LimitTargetCount = (int)(this._devSetting.LimitLowCount * 0.25 + this._devSetting.LimitHighCount * 0.75); // 30000 * 0.25 + 64000 * 0.75 = 55500 , 55500 / 64000 = 0.867
			this._devSetting.LimitTargetCount = ( MAX_COUNT * 8 / 10) ;
#endif
			//----------------------------------------------------------
			// (2) Second Trigger 
			//-----------------------------------------------------------
			secondIntTime = this.CalcNextTime(this._devSetting.LimitStartTime,
																	this._paramSetting[index].LimitIntegralTime,
																	this._triggerMaxCount,
																	this._devSetting.LimitTargetCount);

            this._smartModeINTTime = secondIntTime;

			meterRtn = this.MeterTrigger(secondIntTime);
			this._opticalData[index].IntegralTime = secondIntTime;
			this._calculatetIntTime[index] = secondIntTime;
			if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
			{
				this.ResetData(index, 0);
				return -22;
			}

			if (this._triggerMaxCount < this._devSetting.LimitLowCount)
			{
				return 21;
			}
			else if (this._triggerMaxCount > this._devSetting.LimitHighCount)
			{
				return 22;
			}
			else
			{
				return 20;		// OK
			}
		}

        private int LimitTrigger02(uint index)
        {
            bool meterRtn = false;
            double secondIntTime = 0.0d;
            _isSuccessLimTrigger = false;

            //----------------------------------------------------------
            // (1) First, Trigger 
            //----------------------------------------------------------
            //System.Threading.Thread.Sleep(5);

            meterRtn = this.MeterTrigger(this._devSetting.LimitStartTime);

            _preTriggerSpanTime = this._triggerSpanTime - this._devSetting.LimitStartTime;

            if (meterRtn == false)	// MeterTrigger has error, the subfunction will set the error code
            {
                this.ResetData(index, 0);
                return -21;
            }

            // 當第二次的delta trigger Time > settig 
            // 保留是第一次trigger count

            if (_preTriggerSpanTime > 25)
            {
                _isSuccessLimTrigger = false;
                this.ResetData(index, 0);
                return -23;
            }
            else
            {
                _isSuccessLimTrigger = true;
            }


            if (this._devSetting.LimitStartTime != 0.0d)
            {
                this._opticalData[index].Ratio = (uint)(this._triggerMaxCount / this._devSetting.LimitStartTime);		// Count / ms
            }
            else
            {
                this._opticalData[index].Ratio = double.MaxValue;
            }

#if ( !DebugVer )
            // this._devSetting.LimitTargetCount = (int)(this._devSetting.LimitLowCount * 0.25 + this._devSetting.LimitHighCount * 0.75); // 30000 * 0.25 + 64000 * 0.75 = 55500 , 55500 / 64000 = 0.867
           // this._devSetting.LimitTargetCount = (MAX_COUNT * 80 / 100);

            if (this._devSetting.Limit02PeakPercent > 20 && this._devSetting.Limit02PeakPercent <95)
            {
                this._devSetting.LimitTargetCount = (MAX_COUNT * this._devSetting.Limit02PeakPercent / 100);
            }
            else
            {
                this._devSetting.LimitTargetCount = (MAX_COUNT * 80 / 100);
            }


#endif
            //----------------------------------------------------------
            // (2) Second Trigger 
            //-----------------------------------------------------------
            secondIntTime = this.CalcNextTime(this._devSetting.LimitStartTime,
                                                                    this._paramSetting[index].LimitIntegralTime,
                                                                    this._triggerMaxCount,
                                                                    this._devSetting.LimitTargetCount,this._currentDarkCount);

           // System.Threading.Thread.Sleep(8);

            if (secondIntTime == this._paramSetting[index].LimitIntegralTime)
            {
                secondIntTime = this._smartModeINTTime;
            }
            else
            {
                this._smartModeINTTime = secondIntTime;
            }

           // this._smartModeINTTime = secondIntTime;

            meterRtn = this.MeterTrigger(secondIntTime);
            this._opticalData[index].IntegralTime = secondIntTime;
            this._calculatetIntTime[index] = secondIntTime;

            //return -23;	

            if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
            {
                this.ResetData(index, 0);
                return -22;
            }

            if (this._triggerMaxCount < this._devSetting.LimitLowCount)
            {
                return 21;
            }
            else if (this._triggerMaxCount > this._devSetting.LimitHighCount)
            {
                return 22;
            }
            else
            {
                return 20;		// OK
            }
        }

       	private int SmartTrigger(uint index)
		{
            if (this._smartModeisReTest)
            {
                this._smartModeisReTest = false;

                int rtn = this.LimitTrigger02(index);

                return -23;
            }

            bool meterRtn = this.MeterTrigger(this._smartModeINTTime);

			if (this._triggerMaxCount - this._currentDarkCount < 1000)
			{
				//Bab Die
				return 311;
			}
			else if (this._triggerMaxCount > SMART_MODE_MAX_COUNT && this._smartModeINTTime == this._devSetting.Limit02MinSTTime)
			{
				//Over Light
				this.ResetData(index, 0);

				return 312;
			}
            else if (this._triggerMaxCount < SMART_MODE_MIN_COUNT || this._triggerMaxCount > SMART_MODE_MAX_COUNT)
            {
                this._smartModeisReTest = true;

                this.ResetData(index, 0);

                return -23;
            }

            this._opticalData[index].IntegralTime = this._smartModeINTTime;

            this._calculatetIntTime[index] = this._smartModeINTTime;

            return 20;
        }

       	private int SmartTrigger2(uint index)
		{
			bool meterRtn = true;
			int rtnNum = -1;
			bool isAdjustTime = false;
			double countIncreasRate = 1.0d;
			double triggerTime = (double)MIN_INTEGRATION_TIME;

			// (A) First, Trigger by record integration time
			triggerTime = this._recordIntTime[index];

            if (triggerTime >= (this._paramSetting[index].LimitIntegralTime - 2.0d))
            {
                triggerTime = this._devSetting.LimitStartTime;
            }
			meterRtn = this.MeterTrigger(triggerTime);
			this._opticalData[index].IntegralTime = triggerTime;
			this._calculatetIntTime[index] = triggerTime;

			if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
			{
                this._triggerMaxCount = 0;
				return -301;
			}

			if ( triggerTime != 0.0d )
			{
				this._opticalData[index].Ratio = (uint)(this._triggerMaxCount / triggerTime);		// Count / ms
			}
			else
			{
				this._opticalData[index].Ratio = double.MaxValue;
			}

			//-----------------------------------------------------------------------------------------------------------------------
			// [ state = A1 ]  ,  Count01 < Dark  , It is bad chip
			//-----------------------------------------------------------------------------------------------------------------------
			if (this._triggerMaxCount <= DARK_COUNT)
			{
				rtnNum = 311;
                return rtnNum;
			}

			//-----------------------------------------------------------------------------------------------------------------------
			// [ state = A2 ]  ,  Dark < Count01 <= Low  ,  Calculate the NEXT Integration Time
			//-----------------------------------------------------------------------------------------------------------------------
			if (this._triggerMaxCount > DARK_COUNT && this._triggerMaxCount <= this._devSetting.LimitLowCount)
			{
#if ( !DebugVer )
                this._devSetting.LimitTargetCount = (int)(this._devSetting.LimitLowCount + this._devSetting.LimitHighCount) / 2;
#endif
				// (B) Re-calculate integration time, increase the time and "Second" trigger spectrometer
				triggerTime = this.CalcNextTime(triggerTime,
																	this._paramSetting[index].LimitIntegralTime,
																	this._triggerMaxCount,
																	this._devSetting.LimitTargetCount);

				meterRtn = this.MeterTrigger(triggerTime);
				this._opticalData[index].IntegralTime = triggerTime;
				this._calculatetIntTime[index] = triggerTime;
				if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
				{
                    this._triggerMaxCount = 0;
					return -302;
				}

				//==================================================
				// [ state = A2B0 ]  ,  Low < Count02 < High	,  It is OK
				//==================================================
				if (this._triggerMaxCount > this._devSetting.LimitLowCount && this._triggerMaxCount < this._devSetting.LimitHighCount)
				{
                    isAdjustTime = true;

                    if (triggerTime < (this._recordIntTime[index] * 1.6))
                    {
                        this._recordIntTime[index] = triggerTime;
                        rtnNum = 320;
                    }
                    else
                    {
                        rtnNum = 323;
                    }
				}
				//==================================================
				// [ state = A2B1 ]  ,  Count02 <= Low  ,  It is still low count, but the optical data will calculate by spectrum
				//==================================================
				else if (this._triggerMaxCount <= this._devSetting.LimitLowCount)
				{
					rtnNum = 321;
                    this._triggerMaxCount = 0;      // The chip is bad, force the this._triggerMaxCount = 0, 
                                                    // The CalculateParam() function will reset OptiData(index,0);
				}
				//==================================================
				// [ state = A2B2 ]  ,  Count02 >= High  , It jumps to saturation ??
				//==================================================
				else
				{
                    isAdjustTime = true;
                    this._recordIntTime[index] = triggerTime;
					rtnNum = 322;
				}
			}


			//-----------------------------------------------------------------------------------------------------------------------
			// [ state = A3 ]   ,  Low < Count01 < High  ,  It is OK
			//-----------------------------------------------------------------------------------------------------------------------
			else if (this._triggerMaxCount > this._devSetting.LimitLowCount && this._triggerMaxCount < this._devSetting.LimitHighCount)
			{
                isAdjustTime = true;                    
				this._recordIntTime[index] = triggerTime;
                rtnNum = 330;
			}

			//-----------------------------------------------------------------------------------------------------------------------
			// [ state = A4 ]  ,  Count01 >= High  ,  Saturation , Calculate the NEXT integration time
			//-----------------------------------------------------------------------------------------------------------------------
            else if (this._triggerMaxCount >= this._devSetting.LimitHighCount)
			{
				// (C) Re-calculate integration time, decrese the time and "Second" trigger spectrometer
				triggerTime = this.CalcNextTime(	triggerTime,
																	this._paramSetting[index].LimitIntegralTime,
																	this._triggerMaxCount,
																	//this._devSetting.LimitTargetCount);
																	(this._devSetting.LimitHighCount * 0.25 + this._devSetting.LimitLowCount *0.75 ) );

				meterRtn = this.MeterTrigger(triggerTime);
				this._opticalData[index].IntegralTime = triggerTime;
				this._calculatetIntTime[index] = triggerTime;
				if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
				{
                    this._triggerMaxCount = 0;
					return -304;
				}

				//==================================================
				// [ state = A4C0 ]  ,  Low < Count03 <= High , It is OK
				//==================================================
				if (this._triggerMaxCount > this._devSetting.LimitLowCount && this._triggerMaxCount < this._devSetting.LimitHighCount)
				{
					isAdjustTime = true;					
					this._recordIntTime[index] = triggerTime;
                    rtnNum = 340;
				}
				//==================================================
				// [ state = A4C1 ]  ,  Count03 <= Low , Jump the Low count section ??
				//==================================================
				else if (this._triggerMaxCount <= this._devSetting.LimitLowCount)
				{
					rtnNum = 341;
                    this._triggerMaxCount = 0;      // The chip is bad, force the this._triggerMaxCount = 0, 
                                                    // The CalculateParam() function will reset OptiData(index,0);
				}
				//==================================================
				// [ state = A4C2 ]  ,  Count03 >= High , It is still saturation ???
				//==================================================
				else
				{
                    isAdjustTime = true;										
                    this._recordIntTime[index] = triggerTime;
                    rtnNum = 342;
				}
			}

			// Record the current integration time, before it be adjusted.
			this._opticalData[index].IntegralTime = this._calculatetIntTime[index];

			if (isAdjustTime == true)
			{
				//-----------------------------------------------------------------------------
				//  isAdjuectTime = true => It is OK state, then
				//  LowCount < this._triggerMaxCounr <= HighCount
				//------------------------------------------------------------------------------
				countIncreasRate = this._triggerMaxCount / triggerTime;

				double deltaCount = this._triggerMaxCount - this._devSetting.LimitLowCount;

				if (deltaCount < countIncreasRate)
				{
					this._recordIntTime[index] += 2.0d;
                    rtnNum += 2000;
				}
				else if ((deltaCount >= countIncreasRate) && (deltaCount < (2.0d * countIncreasRate)))
				{
					this._recordIntTime[index] += 1.0d;
                    rtnNum += 1000;
				}

				deltaCount = this._devSetting.LimitHighCount - this._triggerMaxCount;
				if (deltaCount < countIncreasRate)
				{
					this._recordIntTime[index] -= 2.0d;
                    rtnNum += 4000;
				}
				else if (deltaCount >= countIncreasRate && deltaCount < (2.0d * countIncreasRate))
				{
					this._recordIntTime[index] -= 1.0d;
                    rtnNum += 3000;
				}

                if (this._recordIntTime[index] >= this._paramSetting[index].LimitIntegralTime)
                {
                    this._recordIntTime[index] = this._paramSetting[index].LimitIntegralTime;
                }
			}

			return rtnNum;
		}
	
		/// <summary>
		///  Trigger the device to get one relative intensity
		/// </summary>
		private bool MeterTrigger(double IntegralTime)
		{
			if (this._devSetting.IsEnableTrigger == false)
				return true;

			if (IntegralTime < (double)MIN_INTEGRATION_TIME)
			{
				IntegralTime = (double)MIN_INTEGRATION_TIME;
			}

			if (IntegralTime > (double)MAX_INTEGRATION_TIME)
			{
				IntegralTime = (double)MAX_INTEGRATION_TIME;
			}

			if ( this._wavelengthArray == null )
			{
				this._wavelengthArray = (double[])this._oceanWrapper.getWavelengths(0);
			}

            this._pt.Start();

            lock (this)
            {
                //-----------------------------------------------------------------------------------
                // (1) Set integration time to USB2000P
                //-----------------------------------------------------------------------------------
                this._oceanWrapper.setIntegrationTime(0, (int)(1000 * IntegralTime));
                //-----------------------------------------------------------------------------------
                // (2) Get  intensity array from USB2000P , CCD response
                //-----------------------------------------------------------------------------------

                if (this._isHardwareTriggerMode == true)
                {
                   // Output(0x378, 0);
                    System.Threading.Thread.Sleep(0);
                   // Output(0x378, 0xE7);
                    this._eventToDriver.WaitOne();
                    this._eventToSM.Set();
                }
                else
                {
                    this._currentIntensityArray = (double[])this._oceanWrapper.getSpectrum(0);
                }

                this._currentIntensityArray = MpiSPAM.DoBoxCar(this._currentIntensityArray, (int)this._devSetting.BoxCar);

                //-----------------------------------------------------------------------------------
                // (4) Calculatet max count  
                //-----------------------------------------------------------------------------------
                this._maxCountIndex = this.CalcMaxCount(this._currentIntensityArray,100);
                //if (this._devSetting.IsEnableCorrectForDark == true)
                //{
                //    this._currentDark = MpiSPAM.DoCorrectForDarkIntensity(ref this._currentIntensityArray, this._darkIntensityArray);
                //}
                //else
                //{
                //    this._currentDark = 99.99d;
                //}

                if (this._isCorrectForNonlinearity == true)
                {
                    if (this._currentIntensityArray[this._maxCountIndex] > 0)
                    {
                        this._triggerMaxCount = (int)(this._currentIntensityArray[this._maxCountIndex] * this._modifyNonlinearCountRate);

                        if (this._triggerMaxCount > MAX_ADC_COUNT)
                        {
                            this._triggerMaxCount = MAX_ADC_COUNT;
                        }

                        if (this._triggerMaxCount >= MAX_COUNT)
                        {
                            this._triggerMaxCount += 1;
                        }
                    }
                    else
                    {
                        this._triggerMaxCount = 0;
                    }

                }
                else
                {
                    this._triggerMaxCount = (int)(this._currentIntensityArray[this._maxCountIndex]);
                }

                this._pt.Stop();
            }

            this._triggerSpanTime = this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond);


			return true;
		}

		#endregion

		#region >>> Public Methods <<<

		/// <summary>
		///  Initialize the spectrometer 
		/// </summary>
		public bool Init(int deviceNum, string spectrometerSN, string sphereSN)
		{
            Console.WriteLine("[HR2000+], Init() , Start Initial");

			int numSpectrometer = 0;
			try
			{
				//---------------------------------------------------------------------------
				// Create OmniDriver Omni32  SeaBrezzer32
				//---------------------------------------------------------------------------
                //============================
                // (1) Create the wrapper object, first
                //============================
				this._oceanWrapper.CreateWrapper();

                //============================
                // (2) Initialize the spectrometer
                //============================
                if (this._oceanWrapper.Init() == false)
                    return false;

                //============================
                // (3) Open the spectrometers in system
                //============================
				this._oceanWrapper.openAllSpectrometers();

                this._serialNum = this._oceanWrapper.getSerialNumber();

                if (this._serialNum == string.Empty)
                {
                    this._errorNum = EDevErrorNumber.SpectrometerDevice_Init_Err;
                    Console.WriteLine("[HR2000+], Init() , EDevErrorNumber.SpectrometerDevice_Init_Err, SN = Empty");
                    
                    return false;
                }

                if (!this._serialNum.Contains("HR+"))
                {
                    Console.WriteLine("[HR2000+], Init() , Fail! Wrong device model, SN = " + this._serialNum);
                    this._errorNum = EDevErrorNumber.SpectrometerDevice_Init_Err;
                    return false;
                }

				numSpectrometer = this._oceanWrapper.getNumberOfSpectrometersFound();

                Console.WriteLine("[HR2000+], Init() , Success, SN = " + this._serialNum);
                Console.WriteLine("[HR2000+], Init() ,getNumberOfSpectrometersFound =" + numSpectrometer.ToString());

				this._wavelengthArray = this._oceanWrapper.getWavelengths(0);
				this._version = this._oceanWrapper.getApiVersion();
				this._wavelengthArray = (double[])this._oceanWrapper.getWavelengths(0);
			}
			catch
			{
				this._errorNum = EDevErrorNumber.SpectrometerDevice_Init_Err;
                Console.WriteLine("[HR2000+], Init() , EDevErrorNumber.SpectrometerDevice_Init_Err");
				return false;
			}

			if ( numSpectrometer == 0)
			{
				this._errorNum = EDevErrorNumber.SpectrometerDevice_Init_Err;
                Console.WriteLine("[HR2000+], Init() , EDevErrorNumber.SpectrometerDevice_Init_Err");
				return false;
			}

            if (this.LoadBinaryCaliFile(this._serialNum) == false)
			{
				this._errorNum = EDevErrorNumber.LoadCalibrationFileFail;
                Console.WriteLine("[HR2000+], Init() , EDevErrorNumber.LoadCalibrationFileFail");
				return false;
			}

			//---------------------------------------------------------------------------
			// Init SPAM Calculation Class
			//---------------------------------------------------------------------------
			if (this._SPAM.Init() == false)
			{
				this._errorNum = EDevErrorNumber.SPAMDriver_Init_Err;
                Console.WriteLine("[HR2000+], Init() , EDevErrorNumber.SPAMDriver_Init_Err");
				return false;
			}

			this._errorNum = EDevErrorNumber.Device_NO_Error;
           // Console.WriteLine("[HR2000+], Init() , EDevErrorNumber.Success");
			return true;
		}

        public string[] GetEPPROMConfigData()
        {
            return this._oceanWrapper.GetEPPROMConfigData();
        }

		public bool SetConfigToMeter(OptiDevSetting devSetting)
		{
           // Console.WriteLine("[HR2000+], SetConfigToMeter() ");

            if (this._errorNum != EDevErrorNumber.Device_NO_Error)
            {
                return false;
            }
           
            devSetting.CieObserver = ECIEObserver.Ob_1931;
            devSetting.CieIlluminant = ECIEilluminant.E;
            //devSetting.MinCatchPeakCount = 5000;
            devSetting.IsCorrectForNonlinearity = true;
            devSetting.IsUseSphere = true;
            devSetting.SurfaceAreaCmSquared = 1;
            devSetting.IsAutoGetDark = true;
			devSetting.LimitHighCount = MAX_COUNT;
		//	devSetting.LimitStartTime = 6;			// 6ms
			devSetting.IsEnableCorrectForDark = false;
			devSetting.IsEnableHardwareTrigger = false;
        //    devSetting.BoxCar = 6;
            devSetting.ScanAverage = 1;

			this._devSetting = devSetting;
			this._oceanWrapper.setBoxcarWidth(0, (int) this._devSetting.BoxCar);
			this._oceanWrapper.setScansToAverage(0, (int) this._devSetting.ScanAverage);

			if (this._devSetting.IsEnableHardwareTrigger == true)
			{
				this.SetTriggerMode(1);
			}
			else
			{
				this.SetTriggerMode(0);
			}
			//---------------------------------------------------------------
			// Enable Correct for Nonlinearity
			//---------------------------------------------------------------
			if (this._devSetting.IsCorrectForNonlinearity == true)
			{
				this._oceanWrapper.setCorrectForDetectorNonlinearity(0, 1);
			}
			else
			{
				this._oceanWrapper.setCorrectForDetectorNonlinearity(0, 0);
			}
			this._isCorrectForNonlinearity = this._oceanWrapper.getCorrectForDetectorNonlinearity();
			//-----------------------------------------------------------------------------------
			// Search  integration range index form start pixel  and  end pixel
			//-----------------------------------------------------------------------------------
            if (this._wavelengthArray != null)
            {
                MpiSPAM.CalcualteStartAndEndPixel(this._wavelengthArray,
                                                                                (double)this._devSetting.StartWavelength,
                                                                                (double)this._devSetting.EndWavelength,
                                                                                out this._startIntegratePixel,
                                                                                out this._endIntegratePixel);

                Console.WriteLine("[HR2000P],SPT CalcPixel(), wl start=" + this._devSetting.StartWavelength.ToString() + ", End=" + this._devSetting.EndWavelength.ToString());
            }
            else
            {
                Console.WriteLine("[HR2000P], SPT CalcPixel(), wl = null");
            }
			//-----------------------------------------------------------------------------------
			//  Set CIE constant to SPAM class.
			//-----------------------------------------------------------------------------------
			this._SPAM.SetCIEConstant(0, 7);			// 0 = 1931  //  7 = Std. illuminance E
			//this._cieConstants.CreateCIEConstants();
			//this._cieObserver = this._cieConstants.getCIEObserverByIndex(0);        // 0 = 1931 , 1 = 1964
			//this._illuminant = this._cieConstants.getIlluminantByIndex(7);					// Std. illuminance E

			//---------------------------------------------------------------
			// Cali Array And Dark Array 
			//---------------------------------------------------------------

            if (this._spectroCaliData != null)
            {
                if (this._devSetting.IsUseNDFilterRatio == true && this._spectroCaliData.BaseCalibSpectrum!=null)
                {
                    double[] baseSpectrum = this._spectroCaliData.BaseCalibSpectrum;
                    int baseNDIndex = this._spectroCaliData.BaseCalibIndex;
                    int userSettingNDIndex = (int)this._devSetting.AttenuatorPos;
                    double[] ratio = new double[CCD2048_PIXEL_LENGTH];
                    this._caliSpectrum = new double[CCD2048_PIXEL_LENGTH];

                    for (int i = 0; i < CCD2048_PIXEL_LENGTH; i++)
                    {
                        if (this._spectroCaliData.CaliSpectrumArray[userSettingNDIndex][i] != 0 && this._spectroCaliData.CaliSpectrumArray[baseNDIndex][i] != 0)
                            ratio[i] = this._spectroCaliData.CaliSpectrumArray[userSettingNDIndex][i] / this._spectroCaliData.CaliSpectrumArray[baseNDIndex][i];      
                          // calculate ratio //
                          this._caliSpectrum[i] = 1000 * baseSpectrum[i] * ratio[i];                 
                     }
                   // this._caliSpectrum = MpiSPAM.SGFilter(this._caliSpectrum, 25, 3);
                }
                else
                {
                    this._caliSpectrum = this._spectroCaliData.CaliSpectrumArray[this._devSetting.AttenuatorPos];
                    //----------------------------------------------------------------------------------------------
                    // The coef. of calibration spectrum file is saved by "mWatt / nm" unit.
                    // Transfer the unit of "this._caliSpectrum" to uWatt / nm.
                    //----------------------------------------------------------------------------------------------
                    for (int i = 0; i < CCD2048_PIXEL_LENGTH; i++)
                    {
                       // this._caliSpectrum[i] = 1000 * this._caliSpectrum[i];
                    }
                    this._currentMcdCoeff = this._spectroCaliData.MCDCoeff;
                    this._currentMWCoeff = this._spectroCaliData.MWCoeff;
                }
            }
            else
            {
                this._caliSpectrum = new double[CCD2048_PIXEL_LENGTH];
            }
            //----------------------------------------------------------------------------------------------
            // Light Power Factor 
            // After using Halgeon Lamp to do spectral calibration,
            // the factor calculated by using a reference LED // (Refer lm/Msrt lm)=Light Power Factor
            //----------------------------------------------------------------------------------------------
            if (this._spectroCaliData.LightPowerFactor == 0.0d)
            {
                this._spectroCaliData.LightPowerFactor = 1.0d;
            }

            if (this._devSetting.IsUseAbsCorrection)
            {
                for (int i = 0; i < CCD2048_PIXEL_LENGTH; i++)
                {
                    this._caliSpectrum[i] = this._caliSpectrum[i] * this._spectroCaliData.LightPowerFactor;
                }
            }
            // Set Scan Range
            for (int i = 0; i < CCD2048_PIXEL_LENGTH; i++)
            {
                if (i > (this._startIntegratePixel + 3) && i < (this._endIntegratePixel))
                {

                }
                else
                {
                    this._caliSpectrum[i] = 0;
                }
            }

            //paul 20120228-PM1735  By Product Calibration

            if (this._devSetting.IsUseProductXaxisCoeff == true)
            {
                if (this._devSetting.SptXaxisCoefficientByProduct != null)
                {
                    double[] tempWavelength = new double[CCD2048_PIXEL_LENGTH];
                    double intercept = this._devSetting.SptXaxisCoefficientByProduct[(int)ESpectrometerXaxisCoeff.Intercept];
                    double firstCoefficient = this._devSetting.SptXaxisCoefficientByProduct[(int)ESpectrometerXaxisCoeff.FirstCoefficient];
                    double secondCoefficient = this._devSetting.SptXaxisCoefficientByProduct[(int)ESpectrometerXaxisCoeff.SecondCoefficient];
                    double thirdCoefficient = this._devSetting.SptXaxisCoefficientByProduct[(int)ESpectrometerXaxisCoeff.ThirdCoefficient];

                    for (int p = 0; p < CCD2048_PIXEL_LENGTH; p++)
                    {
                        tempWavelength[p] = intercept + (firstCoefficient * p) + secondCoefficient * Math.Pow(p, 2) + thirdCoefficient * Math.Pow(p, 3);
                    }
                    this._wavelengthArray = tempWavelength;                
                }
            }
            else
            {
                this._wavelengthArray = this._oceanWrapper.getWavelengths(0);
            }

            if (this._devSetting.IsUseProductYaxisCalib == true)
            {
                if (this._devSetting.SptYaxisCalibArrayByProduct != null)
                {
                    //-----------------------------------------------------------------------------
                    // the unit of "this._caliSpectrum" to uWatt / nm.
                    //-----------------------------------------------------------------------------
                    this._caliSpectrum = this._devSetting.SptYaxisCalibArrayByProduct;
                }
                else
                {
                    this._caliSpectrum = new double[CCD2048_PIXEL_LENGTH];
                }
            }

            if (this._devSetting.isUseProductYaxisWeight == true)
            {
                if (this._devSetting.SptYaxisCalibArrayByProduct != null)
                {
                    this._calibWeightArray = this._devSetting.SptYaxisWeightArrayByProduct;
                }
            }
            else
            {
                this._calibWeightArray = new double[CCD2048_PIXEL_LENGTH];

                for (int i = 0; i < CCD2048_PIXEL_LENGTH; i++)
                {
                    this._calibWeightArray[i] = 1;
                }
            }

            //

            this.GetDarkSample(5, 8000);

			for (int i = 0; i < this._recordIntTime.Length; i++)
			{
                this._calculatetIntTime[i] = this._devSetting.LimitStartTime;
                this._recordIntTime[i] = this._devSetting.LimitStartTime;
			}

			if (this._devSetting.LimitStartTime < MIN_INTEGRATION_TIME)
			{
				this._devSetting.LimitStartTime = MIN_INTEGRATION_TIME;
			}

            if (this._devSetting.SptFilterMode == ESptFilterMode.Filter)
            {
                this._isFileterAbsSpectrum = true;
            }
            else
            {
                this._isFileterAbsSpectrum = false;
            }
			
            this._smartModeisReTest = true;

			return true;
		}

		public bool SetParamToMeter(OptiSettingData[] paramSetting)
		{
            //Console.WriteLine("[HR2000+], SetParamToMeter() ");
			int settingCount = 1;

			if (paramSetting == null)
			{
				settingCount = 1;
			}
			else
			{
				this._paramSetting = paramSetting;
				settingCount = this._paramSetting.Length;
			}

			this._opticalData = new OptiData[settingCount];
			this._recordIntTime = new double[settingCount];
			this._calculatetIntTime = new double[settingCount];

			this._recordIntensityArray = new double[settingCount][];
			this._absoluteSepctrum = new double[settingCount][];
			this._isFirstTest = new bool[settingCount];
			this._indexPixel = new int[settingCount];

			for (int i = 0; i < settingCount; i++)
			{
                Console.WriteLine("[USB2000P], SetParamToMeter(), LOPWL_" + i.ToString() + " - " + paramSetting[i].SensingMode + " Fix=" + paramSetting[i].FixIntegralTime + ", Limit=" + paramSetting[i].LimitIntegralTime);
				this._opticalData[i] = new OptiData();
				this._recordIntTime[i] = this._devSetting.LimitStartTime;
				this._calculatetIntTime[i] = this._devSetting.LimitStartTime;

				this._recordIntensityArray[i] = new double[CCD2048_PIXEL_LENGTH];
				this._absoluteSepctrum[i] = new double[CCD2048_PIXEL_LENGTH];
				this._isFirstTest[i] = true;
				this._indexPixel[i] = 20;
			}	

			if (paramSetting == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Re-conncect to the spectrometer
		/// </summary>
		public bool ReConnect()
		{
			return true;
		}

		/// <summary>
		/// Get wavelength data of the spectrometer 
		/// </summary>
		public double[] GetXWavelength()
		{
			if (this._errorNum != EDevErrorNumber.Device_NO_Error)    // Trigger function is not OK or spectrometer is in ERROR Status
			{
				this._errorNum = EDevErrorNumber.GetWavelength_Fail;
				this._wavelengthArray = new double[CCD2048_PIXEL_LENGTH];
			}
			return this._wavelengthArray;
		}

		/// <summary>
		/// Get intensity data of the device 
		/// </summary>
		public double[] GetYSpectrumIntensity(uint index)
		{
			if (index < 0 || index >= this._recordIntensityArray.Length)
			{
				index = 0;
			}
			return this._recordIntensityArray[index];
		}

		public double[][] GetYSpectrumIntensityAll()
		{
			return this._recordIntensityArray;
		}

		public double[] GetYAbsoluateSpectrum(uint index)
		{
			if (index < 0 || index >= this._absoluteSepctrum.Length)
			{
				index = 0;
			}
			return this._absoluteSepctrum[index];
		}

		public double[][] GetYAbsoluateSpectrumAll()
		{
			return this._absoluteSepctrum;
		}

		public int Trigger(uint index)
		{
			int rtnNum = 0;

			if (this._paramSetting == null)
			{
				this._errorNum = EDevErrorNumber.NoSpectrometerParamSettingData;
				return -12345;
			}

			if (this._devSetting.IsEnableTrigger == false)
				return 12345;

			lock (this._lockObj)
			{
				switch (this._paramSetting[index].SensingMode)
				{
					case ESensingMode.Limit:
						rtnNum = this.LimitTrigger(index);
						// rtnNum = -21, first trigger error
						// rtnNum = -22, sencond trigger error
                        // rtnNum = -23, 光譜卡通訊時間過長,當下重新測試
						// rtnNum = 21, this. _triggerMaxCount < LimitLowCount , 
						// rtnNum = 22, this. _triggerMaxCount >LimitHighCount
						// rtnNum = 20, LimitLowCount <= this. _triggerMaxCount <= LimitHighCount
						break;
					//-----------------------------------------------------------------------------------------------------
					case ESensingMode.Fixed:
						if (this.MeterTrigger(this._paramSetting[index].FixIntegralTime))
						{
							rtnNum = 10;
						}
						else
						{
							rtnNum = -10;
						}
						this._calculatetIntTime[index] = this._paramSetting[index].FixIntegralTime;
						this._opticalData[index].IntegralTime = this._paramSetting[index].FixIntegralTime;
						this._opticalData[index].Ratio = 0.0d;
						break;
					//-----------------------------------------------------------------------------------------------------
					case ESensingMode.Limit02:
						rtnNum = this.SmartTrigger(index);
						// rtnNum = -301, -302, -304, spectrometer trigger error
						// rtnNum = 311,  < DARK Count, It is Bad chip
						// rtnNum = 312,  < Over light
						// rtnNum = 321, 341, this. _triggerMaxCount < LimitLowCount , 
						// rtnNum = 322, 342, this. _triggerMaxCount >LimitHighCount
						// rtnNum = 320, 330, 340, LimitLowCount <= this. _triggerMaxCount <= LimitHighCount
						break;
					//-----------------------------------------------------------------------------------------------------
					default:
						break;
				}

				if (rtnNum == 311 || rtnNum == 312)
				{
					this._opticalData[index].MaxCount = (uint)this._triggerMaxCount;
					this._opticalData[index].CountPercent = 100.0d * (this._triggerMaxCount / (double)MAX_COUNT);
					this._opticalData[index].TriggerStatus = rtnNum;

					Array.Clear(this._currentIntensityArray, 0, this._currentIntensityArray.Length);

					Array.Copy(this._currentIntensityArray, this._recordIntensityArray[index], this._currentIntensityArray.Length);

					this._oceanWrapper.ResetSpectrumArray();
				}
				else
				{
                //this._opticalData[index].IntegralTime = this._triggerSpanTime;
                this._opticalData[index].MaxCount = (uint)this._triggerMaxCount;
                this._opticalData[index].CountPercent = 100.0d * (this._triggerMaxCount / (double)MAX_COUNT);
                this._opticalData[index].TriggerStatus = rtnNum;

                this._opticalData[index].DarkAvg = (int)this._darkStat.Mean;
                this._opticalData[index].DarkSTDev = (int)this._darkStat.Variance;
                //this._opticalData[index].ChipDarkAvg = (int) MathMethod.DoCorrectForDarkIntensity_4(this._currentIntensityArray, this._darkIntensityArray);
                this._opticalData[index].ChipDarkAvg = (int)this._currentDarkCount;
                this._opticalData[index].TriggerTime = this._triggerSpanTime;
                this._opticalData[index].WLPNIR = this._preTriggerSpanTime;
                this._indexPixel[index] = this._maxCountIndex;
                Array.Copy(this._currentIntensityArray, this._recordIntensityArray[index], this._currentIntensityArray.Length);

                this._oceanWrapper.ResetSpectrumArray();
				}
			}

			return rtnNum;
		}

		public bool CalculateParameters(uint index)
		{
			if (this._isDarkExistence == false)
			{
				this._errorNum = EDevErrorNumber.NoGetDarkArray;
				return false;
			}

			ResetData(index, 0);

            if (this._isSuccessLimTrigger == false)
            {
                this._absoluteSepctrum[index] = new double[CCD2048_PIXEL_LENGTH];
                return true;
            }


            if (this._opticalData[index].MaxCount <= this._devSetting.MinCatchPeakCount || this._opticalData[index].MaxCount >= MAX_COUNT)
            {
                this._absoluteSepctrum[index] = new double[CCD2048_PIXEL_LENGTH];
                return true;
            }

			if (this._opticalData[index].TriggerStatus == 311 || this._opticalData[index].TriggerStatus == 312)
			{
				Array.Clear( this._absoluteSepctrum[index], 0, this._absoluteSepctrum[index].Length);
				return true;
			}

			//-------------------------------------------------------------------------------------------------------------------------------------	
			// (1) Reset all calculated parameters of SPAM
			//-------------------------------------------------------------------------------------------------------------------------------------	
			this._SPAM.ResetAllParam();

			//---------------------------------------------------------------------------------------------------------------------------------------	
			// (2) Compute absolute spectrum ( Unit = uWatt/cm2/nm ) from relative intensity (count / pixel ), 
			//		wavelength ( nm ), calibration spectrum ( Unit = uWatt/ pixel ), integration time and others parameters
			//---------------------------------------------------------------------------------------------------------------------------------------	

            if (this._devSetting.IsUseMPISpam2)
            {
                this._absoluteSepctrum[index] = this._mpiSPAM2.ComputeAbsSpectrum(this._darkIntensityArray,
                                                                                this._recordIntensityArray[index],
                                                                                this._wavelengthArray,
                                                                                this._caliSpectrum,
                                                                                this._calculatetIntTime[index],
                                                                                this._devSetting.SurfaceAreaCmSquared,
                                                                                this._devSetting.IsUseSphere);
            }
            else
            {
                this._absoluteSepctrum[index] = this._SPAM.ComputeAbsSpectrum(this._darkIntensityArray,
                                                                            this._recordIntensityArray[index],
                                                                            this._wavelengthArray,
                                                                            this._caliSpectrum,
                                                                            this._calculatetIntTime[index],
                                                                            this._devSetting.SurfaceAreaCmSquared,
                                                                            this._devSetting.IsUseSphere);
            }



            // S-G Filter11 points ,cublic Spline =3
             this._absoluteSepctrum[index] = MpiSPAM.SGFilter(this._absoluteSepctrum[index], (uint)11, (uint)3);

            // Yaxis Weight Calibration Abs To Abs
            if (this._devSetting.isUseProductYaxisWeight)
            {
                for (int i = 0; i < CCD2048_PIXEL_LENGTH; i++)
                {
                    this._absoluteSepctrum[index][i] = this._absoluteSepctrum[index][i] * this._calibWeightArray[i];
                }
            }

            if (this._isFileterAbsSpectrum)
            {
                this._absoluteSepctrum[index] = MpiSPAM.FilterData(this._wavelengthArray, 555, this._absoluteSepctrum[index], 1000.0d);
            }

            //if (this._isFileterAbsSpectrum)
            //{
            //    // Filter Base Noise , User can set this parameter
            //    // cut Value= 100/BaseNoise
            //    // Base Noise =1 , Cut Value=100 , indicate cut 1%
            //    // Base Noise =0.1 , Cut Value=1000 , indicate cut 0.1%
            //    // Base Noise =2 , Cut Value=50 , indicate cut 2%

            //    if (this._devSetting.BaseNoise != 0.0d)
            //    {
            //        double cutValue = 100 / this._devSetting.BaseNoise;

            //        this._absoluteSepctrum[index] = MpiSPAM.FilterData(this._wavelengthArray, 380, this._absoluteSepctrum[index], cutValue);
            //    }      
            //}
            //else
            //{	
            //    //  Filter the absolute spectrum
            //    //  Default Base Noise =0.1 , Cut Value=1000 , indicate cut 0.1%

            //    this._absoluteSepctrum[index] = MpiSPAM.FilterData(this._wavelengthArray, 555, this._absoluteSepctrum[index], 1000.0d);
            //}



            if ( !this._devSetting.IsUseMPISpam2 )
            {
			    //---------------------------------------------------------------------------------------------------------------------------------------	
			    // (3) Compute peak wavelength ( WLP ) , centriod wavelength ( WLC ) and FHWM by absolute spectrum
			    //---------------------------------------------------------------------------------------------------------------------------------------	

			    // [3-a] Smooth for the peak curve data again, and find the index
                this._indexPixel[index] = MpiSPAM.CalcMaxCount(this._absoluteSepctrum[index], 0, 0);

			    // [3-b] Calculate FHWM, WLP, WLC.	
			    this._SPAM.ComputeSpectrumData(this._indexPixel[index], this._wavelengthArray, this._absoluteSepctrum[index]);

			    // [3-c] Caculate the WLP by curve fiting
			    double peakWave = 0.0d;

                if (this._devSetting.IsCalcSpecialWLP)
                {
                    peakWave = this.CalcSpecialWLP(this._indexPixel[index], this._wavelengthArray, this._absoluteSepctrum[index]);
                }
                else
                {
		            //peakWave = MpiSPAM.CaculatePeakWavelength(this._indexPixel[index], this._wavelengthArray, this._absoluteSepctrum[index], 15);

                    // 直接由相對頻譜找 Peak
                    int peakIndex = MpiSPAM.CalcMaxCount(this._recordIntensityArray[index], 0, 0);

                    peakWave = this._wavelengthArray[peakIndex];
                }

			    //---------------------------------------------------------------------------------------------------------------------------------------	
			    // (4) Compute CIE x, CIE y, purity, dominant Wavelength, CCT and CRI by absolute spectrum
			    //---------------------------------------------------------------------------------------------------------------------------------------	
			    this._SPAM.ComputeColorPurityWLD_CCT_CRI(this._wavelengthArray, this._absoluteSepctrum[index], this._devSetting.IsCalcCRIData);

			    //---------------------------------------------------------------------------------------------------------------------------------------	
			    // (5) Compute absoluate watt (uWatt ) by integrate absolute spectrum ( Unit = uWatt/cm2/nm )
			    //---------------------------------------------------------------------------------------------------------------------------------------	
			    double microWattBySPAM = 0.0d;

			    microWattBySPAM = this._SPAM.NumericalIntgrateB(		this._wavelengthArray,
																									    this._absoluteSepctrum[index],
																									    this._startIntegratePixel,
																									    this._endIntegratePixel,
																									    EIntegrateMode.SIMPSONS);

			    //microWattBySPAM = this._SPAM.ComputeWattB(		this._wavelengthArray,
			    //																						this._absoluteSepctrum[index],
			    //																						 this._startIntegratePixel,
			    //																						this._endIntegratePixel,
			    //																						EIntegrateMode.SIMPSONS);

			    //microWatt = MathMethod.NumericalIntgrateA(	this._wavelengthArray,
			    //                                                                           this._absoluteSepctrum[index],
			    //                                                                           this._startIntegratePixel,
			    //                                                                           this._endIntegratePixel,
			    //                                                                           EIntegrateMode.SIMPSONS,
			    //                                                                           out microWattPerPixel);

			    //---------------------------------------------------------------------------------------------------------------------------------------	
			    // (6) Compute luminous flux ( Unit = lm) by absolute spectrum ( Unit = uWatt/cm2/nm )
			    //---------------------------------------------------------------------------------------------------------------------------------------	
			    double lumens = 0.0d;
			    double[] weighting;
			    double[] lmPerWave;
			    double[] lmPerPixel;

			    //==============================================================		
			    // The parameter "this._absoluteSepctrum[index]" Unit = uWatt/cm2/nm
			    // The out parameter value "lmPerWave" Unit = u-lm/cm2/nm
			    //==============================================================	

                if (this._devSetting.VLamdaType == 0)
                {
                    MpiSPAM.ComputeLuminousFluxLumenC(this._wavelengthArray, this._absoluteSepctrum[index], out weighting, out lmPerWave);
                }
                else
                {
                    MpiSPAM.ComputeLuminousFluxLumenE(this._wavelengthArray, this._absoluteSepctrum[index], out weighting, out lmPerWave);
                }
                //else
                //{
                //    MpiSPAM.ComputeLuminousFluxLumenE(this._wavelengthArray, this._absoluteSepctrum[index], out weighting, out lmPerWave);
                //}
		
			    lumens = MpiSPAM.NumericalIntgrateA(  this._wavelengthArray,
																				    lmPerWave,
																				    this._startIntegratePixel,
																				    this._endIntegratePixel,
																				    EIntegrateMode.TRAPEZOID,
																				    out lmPerPixel);

			    lumens = lumens * 0.000001;		// u-lm => lm;
			    //---------------------------------------------------------------------------------------------------------------------------------------	
			    // (7) Compute illuminance, in Lux, given the amount of visible light emitted, as well as 
			    //		the area in square meters that is being illuminated.
			    //
			    //  Return : the total luminous flux incident on a surface, per unit area in lux (lx = lm/m²).
			    //---------------------------------------------------------------------------------------------------------------------------------------
			    double illuminanceLux = lumens / 0.002827433;		// ( lm / const. )
		
			    //---------------------------------------------------------------------------------------------------------------------------------------
			    // (8) Set the calculated parameters result
			    //---------------------------------------------------------------------------------------------------------------------------------------
			    this._opticalData[index].WLP = peakWave;            
			    //this._opticalData[index].WLPNIR = 0.0d;
			    this._opticalData[index].WLCv = this._SPAM.WLC;
                this._opticalData[index].WLCp = this._SPAM.WLC;
			    this._opticalData[index].FWHM = this._SPAM.FWHM;

                this._opticalData[index].u_prime = this._SPAM.CIE1976u;
                this._opticalData[index].v_prime = this._SPAM.CIE1976v; 
			    this._opticalData[index].CIE1931x = this._SPAM.CIEx;
			    this._opticalData[index].CIE1931y = this._SPAM.CIEy;
			    this._opticalData[index].Purity = this._SPAM.Purity;
			    this._opticalData[index].WLD = this._SPAM.WLD;
			    this._opticalData[index].CCT = this._SPAM.CCT;
			    this._opticalData[index].GeneralCRI = this._SPAM.CRI;
                this._opticalData[index].ColorDelta = this._SPAM.ColorDelta;
                this._opticalData[index].SpecialCRI = this._SPAM.SpecialCRI;

			    this._opticalData[index].Watt = microWattBySPAM * 0.001;			// micro-Watt  => mWatt
			    this._opticalData[index].Lm = lumens;											// lm
			    this._opticalData[index].Lx = illuminanceLux;									// lm / sr

                if (this._opticalData[index].Purity >= 1.0d)
                {
                    this._opticalData[index].Purity = 1.0d;
                }
            }
            else
            {
                OpticalDataPackage optDataPackage = new OpticalDataPackage();

                this._mpiSPAM2.SetIsCaculateCCTandCRI(this._devSetting.IsCalcCRIData);

                this._mpiSPAM2.CaculateParameter(ref optDataPackage, this._wavelengthArray, this._absoluteSepctrum[index], this._startIntegratePixel, this._endIntegratePixel);

                double peakwave=0.0d;

                //---------------------------------------------------------------------------------------------------------------------------------------
                // (8) Set the calculated parameters result
                //---------------------------------------------------------------------------------------------------------------------------------------
                if (this._devSetting.IsCalcSpecialWLP)
                {
                    this._opticalData[index].WLP = this.CalcSpecialWLP(MpiSPAM.CalcMaxCount(this._absoluteSepctrum[index], 0, 0), this._wavelengthArray, this._absoluteSepctrum[index]);
                }
                else
                {
                   //this._opticalData[index].WLP = optDataPackage._WLP;
                    int peakIndex2 = MpiSPAM.CalcMaxCount(this._absoluteSepctrum[index], 0, 0);

                    peakwave = this._wavelengthArray[peakIndex2];

                    this._opticalData[index].WLP = peakwave;
                }

                //this._opticalData[index].WLPNIR = 0.0d;
                this._opticalData[index].WLCv = optDataPackage._WLCv;
                this._opticalData[index].WLCp = optDataPackage._WLCp;
                this._opticalData[index].FWHM = optDataPackage._FWHM;

                this._opticalData[index].u_prime = optDataPackage._CIE1976u;
                this._opticalData[index].v_prime = optDataPackage._CIE1976v;
                this._opticalData[index].CIE1931x = optDataPackage._CIE1931x;
                this._opticalData[index].CIE1931y = optDataPackage._CIE1931y;
                this._opticalData[index].Purity = optDataPackage._purity;
                this._opticalData[index].WLD = optDataPackage._WLD;
                this._opticalData[index].CCT = optDataPackage._CCT;
                this._opticalData[index].GeneralCRI = optDataPackage._generalCRI;
                this._opticalData[index].ColorDelta = optDataPackage._colorDelta;
                this._opticalData[index].SpecialCRI = new double[optDataPackage.criSampleCount];
                unsafe
                {
                    for (int i = 0; i < optDataPackage.criSampleCount; i++)
                    {
                        this._opticalData[index].SpecialCRI[i] = optDataPackage._specialCRI[i];
                    }
                }

                this._opticalData[index].Watt = optDataPackage._watt;       // mW
                this._opticalData[index].Lm = optDataPackage._Lm;       	// lm
                this._opticalData[index].Lx = optDataPackage._Lx;			// lm / sr

                if (this._opticalData[index].Purity >= 1.0d)
                {
                    this._opticalData[index].Purity = 1.0d;
                }
            }
            //---------------------------------------------------------------------------------------------------------------------------------------	
            // Paul 2013.06.25
            // NIR Product or UV Product , WLD =0.0d is normal, so only WLP can set low boundary. 
            //---------------------------------------------------------------------------------------------------------------------------------------	

            if (this._opticalData[index].WLP < 1.0d)
            {
                ResetData(index, 0);
                return false;
            }

            //if (this._opticalData[index].WLP < 1.0d || this._opticalData[index].WLD < 1.0d || this._opticalData[index].WLCv < 1.0d)
            //{
            //    ResetData(index, 0);
            //    return false;
            //}

			return true;
		}

		public void doWork()
		{
			while (true)
			{
				this._eventToSM.WaitOne();
				this._currentIntensityArray = (double[])this._oceanWrapper.getSpectrum(0);
				//Output(0x378, 0);
				this._eventToDriver.Set();
			}
		}

		public double[] GetDarkSample(uint samplingDarkCount,uint IntTime)
		{
			this._oceanWrapper.setIntegrationTime(0, (int)IntTime);
            double[] darkSpectrumSum = new double[2048];

			if (this._isHardwareTriggerMode == true)
			{
				//Output(0x378, 0);
				System.Threading.Thread.Sleep(5);

				//Output(0x378, 0xE7);
				this._eventToDriver.WaitOne();
				this._eventToSM.Set();
				this._darkIntensityArray = this._currentIntensityArray;
				this._isDarkExistence = true;
			}
			else
			{
                for (int i = 0; i < samplingDarkCount; i++)
                {
                    double[] array= (double[])this._oceanWrapper.getSpectrum(0);
                    for (int pix = 0; pix < CCD2048_PIXEL_LENGTH; pix++)
                    {
                        darkSpectrumSum[pix] += array[pix];
                    }
                    //this._darkIntensityArray = (double[])this._oceanWrapper.getSpectrum(0);
                }              
				this._isDarkExistence = true;
			}
			
			// Average of dark array 
            for (int i = 0; i < CCD2048_PIXEL_LENGTH; i++)
            {
                this._darkIntensityArray[i] = darkSpectrumSum[i] / samplingDarkCount;
            }

            this._darkIntensityArray = MpiSPAM.DoBoxCar(this._darkIntensityArray, this._devSetting.BoxCar);

			this._darkStat.Clear();

			for (int i = 0; i < this._darkIntensityArray.Length; i++)
			{
				if (this._wavelengthArray[i] > MIN_WAVELENGTH)
				{
					this._darkStat.Push(this._darkIntensityArray[i]);
				}
			}

            this._currentDarkCount = this._darkStat.Mean * this._modifyNonlinearCountRate;

			return _darkIntensityArray;
		}

		public void Close()
		{
            this._oceanWrapper.Close();
		}

		#endregion

		#region >>> Get Calibration <<<

		public bool GetIrradianceCalibrationArray(double[] referLampWave, double[] referLampPower)
		{
			if (this._darkIntensityArray == null)
			{
				return false;
			}
			//----------------------------------------------------------------------------
			// Get Refer Spectrum (Halogen lamp) 
			//----------------------------------------------------------------------------

			// SPAM.CCoNumericalMethods numMethod = new SPAM.CCoNumericalMethods();
			// numMethod.CreateNumericalMethods();
			//if (this._numericalMethod == null)
			//{
			//    this._numericalMethod = new SPAM.CCoNumericalMethods();
			//}
			//double[] yout = (double[])this._numericalMethod.cubicSpline(referLampWave, referLampPower, this._wavelengthArray);

			double[] yout = this._SPAM.NumCublicSpline(referLampWave, referLampPower, this._wavelengthArray);

			//this._caliSpectrum = (double[])this._irradianceCalibration.processSpectrum(this._darkIntensityArray,
			//                                                                                this._recordIntensityArray[0],
			//                                                                                this._wavelengthArray,
			//                                                                                yout,
			//                                                                                this._calculatetIntTime[0],
			//                                                                                this._devSetting.SurfaceAreaCmSquared,
			//                                                                                this._devSetting.IsUseSphere);

			this._caliSpectrum = this._SPAM.IrradianceCalibrate(this._darkIntensityArray,
																				this._recordIntensityArray[0],
																				this._wavelengthArray,
																				yout,
																				this._calculatetIntTime[0],
																				this._devSetting.SurfaceAreaCmSquared,
																				this._devSetting.IsUseSphere);
			
			//----------------------------------------------------------------------------------------------
			// Transfer the unit of "this._caliSpectrum" to mWatt / nm,
			// The coef. of calibration spectrum file will saved by "mWatt / nm" unit
			//----------------------------------------------------------------------------------------------
			for (int i = 0; i < CCD2048_PIXEL_LENGTH;i++ )
			{
				this._caliSpectrum[i] = 0.001 * this._caliSpectrum[i];
				if (this._wavelengthArray[i] < referLampWave[0] || this._wavelengthArray[i] > referLampWave[referLampWave.Length-1])
				{
					this._caliSpectrum[i] = 0;
				}
			}
			return true;
		}
		
		#endregion
	}
}
