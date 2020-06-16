using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.DeviceCommon;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;


namespace MPI.Tester.Device.SpectroMeter.LE5400
{
    public class LE5400 : ISpectroMeter
    {
        private const bool FIRE_DLL_EXCEPTION = true;
        private const uint DEFAULT_INT_TIME = 10;
        private const int MIN_INTEGRATION_TIME = 2;
        private const int MAX_INTEGRATION_TIME = 2000;
        private const double LIMIT_TARGET = 0.75;
        public const string ROOT = @"C:\MPI\Spectrometer";        

        private MMC_Cond _msrtCond;
        private DARKDATA _darkData;
        private MLAR_RESULT _msrtResult;
        private MCPD_MEASE_SPECTRUM _msrtSpectrum;
        private MLAC_COND _calcCond;
        private CORENGINFO _correctEngInfo;
        private AUTOADJUST_RESULT _autoAdjustST;
        private MCPD_MEASE_SPECTRUM[] _recordSpectrum;
        private MCPD_MEASE_SPECTRUM[] _absSpectrum;


        private object _lockObj;
        private int _ccdPixelLength;

        private string _version;
        private string _serialNumber;

        private OptiDevSetting _devSetting;
        private OptiSettingData[] _msrtSetting;
        private OptiData[] _opticalData;
        private EDevErrorNumber _errorNumber;
        private double[] _darkIntensityArray;

        OceanSPAM _spam;
        SpectroCaliData _spectroCaliData;

        private int _conncetID;
        private int _numbersOfPixel;
        private bool _isSaturated;
        private bool _initSuccessful;

        protected uint _intTimeMin;		// in ms
        protected uint _intTimeMax;

        private double[] _wavelengthArray;
        private double[] _currentIntensityArray;
        private double[][] _recordIntensityArray;
        private double[][] _absoluteSepctrum;
        private uint[] _recordST;
        private double _correctEngFactor;
        private int _startIntegratePixel;
        private int _endIntegratePixel;
        private double[] _calibrationArray;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LE5400()
        {
            this._msrtCond = new MMC_Cond((int)EConnectMode.Highspeed);
            this._msrtResult = new MLAR_RESULT();
            this._msrtSpectrum = new MCPD_MEASE_SPECTRUM();       
            this._calcCond = new MLAC_COND(true);
            this._correctEngInfo = new CORENGINFO();
            this._autoAdjustST = new AUTOADJUST_RESULT();

            this._conncetID = -1;
            this._numbersOfPixel = LE5400Wrapper.MEAS_DATA_MAX;

            this._intTimeMin = 2;
            this._intTimeMax = 20000;

            this._isSaturated = false;
            this._spam = new OceanSPAM();
            this._spam.Init();
            this._correctEngFactor = 1;
            this._serialNumber = "None";
            this._initSuccessful = false;
            this._calibrationArray = new double[LE5400Wrapper.MEAS_DATA_MAX];
        }

        #region >>> Public Property  <<<

        /// <summary>
        /// Pixel array length.
        /// </summary>
   


        /// <summary>
        /// Serial number.
        /// </summary>
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

        public double[] DarkIntensityArray
        {
            get { return this._darkIntensityArray; }
        }
        #endregion

        #region >>> Public  Method <<<
        //----------------------------------------------------------------------------
        // the functions inherited from the ISpectrometer
        //----------------------------------------------------------------------------
        public  bool Init(int deviceNum, string spectroMeterSN, string sphereSN)
        {
            int rtn=-1;
            this._initSuccessful = false;
            int isEnableLog = 0;

            if(this._conncetID > 0)
            {
                if(LE5400Wrapper.DeviceHandling.CloseMcpd(this._conncetID) == (int)EMcpdApiErrorCode.SUCCESS)
                {
                    this._conncetID = 0;
                }  
                LE5400Wrapper.ApiHandling.McpdApiExit(ref isEnableLog);
            }

             int numsDeviceOfConnected  = -1;
             LE5400Wrapper.DeviceHandling.SetConnectMode((int)EConnectMode.Highspeed, 0);

             int[] dwMachine = new int[10];
            // Multi-Spectrometer 
             numsDeviceOfConnected = LE5400Wrapper.DeviceHandling.EnumMcpd(ref dwMachine[0], 10);

             if (numsDeviceOfConnected < 1)
             {
                 this._initSuccessful = false;
             }
             //LE5400Wrapper.ApiHandling.McpdApiExit(ref isEnableLog);
             rtn=LE5400Wrapper.ApiHandling.McpdApiInit(ref isEnableLog); // Delete Error Log

             if (rtn == 0)
             {
                 this._initSuccessful = false;
                 return this._initSuccessful;
             }

             int conncetType = 0;
             this._conncetID = LE5400Wrapper.DeviceHandling.OpenMcpd(conncetType);

             if(this._conncetID > 0)
             {
                 this._initSuccessful = true;
                 this._serialNumber = LE5400Wrapper.DeviceHandling.GetMcpdMachineCode(this._conncetID).ToString();

                 Console.WriteLine("Intial LE5400"+  this._serialNumber );

                 LE5400Wrapper.DeviceHandling.GetCorEngInfo(this._msrtCond.CorrectEng, 0, ref this._correctEngInfo);
                  //this = new string(this._correctEngInfo.CorEngName).Trim('\0');
                 //if(this.LoadCaliFile("LE5400-" + this._serialNumber)==false)
                 //{
                 //    this._initSuccessful = false;
                 //    this._errorNumber = EDevErrorNumber.LoadCalibrationFileFail;
                 //    return this._initSuccessful;
                 //}
               
                 this.InitDataArray(1);
                 this.GetDarkSample(1, 1000 * DEFAULT_INT_TIME);
             }
             else
             {
                 this._initSuccessful = false;
                 this._errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
             }    
             return this._initSuccessful;
        }

        public  void Close()
        {
            if (this._conncetID <= 0)
            {
                return;
            }

            if (LE5400Wrapper.DeviceHandling.CloseMcpd(this._conncetID) == (int)EMcpdApiErrorCode.SUCCESS)
            {
                this._conncetID = 0;
            }
            int isEnableLog = 0;
            LE5400Wrapper.ApiHandling.McpdApiExit(ref isEnableLog);
        }

        public bool SetConfigToMeter(OptiDevSetting devSetting)
        {
            this._calcCond.VisionAngle = 0;
            //LE5400Wrapper.InstrumentProperty.SyncMode((int)EConnectMode.Highspeed, 1);
            //this._calcCond.WaveLo = 380;
            //this._calcCond.WaveHi = 780;
            this._calcCond.IntegPeak[0] = 1;
            this._calcCond.IntegPeakLo[0] = 380;
            this._calcCond.IntegPeakHi[0] = 780;
            this._calcCond.IntegPeakLo[1] = 380;
            this._calcCond.IntegPeakHi[1] = 780;
            this._calcCond.PeakDependLo = 380;
            this._calcCond.PeakDependHi = 780;
            this._devSetting = devSetting;

            if(this._wavelengthArray != null)
            {
                MpiSPAM.CalcualteStartAndEndPixel(this._wavelengthArray,
                                                                                (double)this._devSetting.StartWavelength,
                                                                                (double)this._devSetting.EndWavelength,
                                                                                out this._startIntegratePixel,
                                                                                out this._endIntegratePixel);
            }
            // No Create ND Filter
            this._devSetting.AttenuatorPos = 0;

            if(this._spectroCaliData != null)
            {
                this._calibrationArray = this._spectroCaliData.CaliSpectrumArray[this._devSetting.AttenuatorPos];

                if(this._devSetting.AttenuatorPos > 1)
                {
                    for(int i = 0; i < this._numbersOfPixel; i++)
                    {
                        this._calibrationArray[i] = 1;
                    }
                }
            }
            else
            {
                this._calibrationArray = new double[LE5400Wrapper.MEAS_DATA_MAX];

                for(int i = 0; i < this._numbersOfPixel; i++)
                {
                    this._calibrationArray[i] = 1;
                }
            }

            if(this._devSetting.IsUseAbsCorrection && this._spectroCaliData != null)
            {
                for(int i = 0; i < _numbersOfPixel; i++)
                {
                    this._calibrationArray[i] = this._calibrationArray[i] * this._spectroCaliData.LightPowerFactor;
                }
            }
            return true;
        }

        public bool SetParamToMeter(OptiSettingData[] msrtSetting)
        {
            if (msrtSetting == null)
                return false;

            this._msrtSetting = msrtSetting;
            this.InitDataArray(this._msrtSetting.Length);
            Console.WriteLine("Spectrometer , Setting Mseasure Para ");
            return true;
        }

        public  int Trigger(uint settingIndex)
        {
            if (this._msrtSetting == null)
                return -1;

            if (settingIndex >= this._msrtSetting.Length)
                return -1;

            this._msrtSpectrum = new MCPD_MEASE_SPECTRUM();
            uint intTime = 0;

            if (this._msrtSetting[settingIndex].SensingMode == ESensingMode.Fixed)
            {
                // us to ms
                intTime = (uint)(this._msrtSetting[settingIndex].FixIntegralTime);
                this.MeterTrigger(intTime);
                this._recordSpectrum[settingIndex] = this._msrtSpectrum;
                this._recordST[settingIndex] = intTime;
            }
            else if (this._msrtSetting[settingIndex].SensingMode == ESensingMode.Limit)
            {
                this.LimitTrigger(settingIndex);        
            }

            if(this._currentIntensityArray == null)
            {
                return -1;
            }

            this._wavelengthArray = this._msrtSpectrum.pdWave;
            this._opticalData[settingIndex].IntegralTime = this._recordST[settingIndex];
            int indexPixel = MpiSPAM.CalcMaxCount(this._recordSpectrum[settingIndex].pdValue, 10, 0);
            this._opticalData[settingIndex].MaxCount = (uint)(100 * this._recordSpectrum[settingIndex].pdValue[indexPixel]);
            Array.Copy(this._currentIntensityArray, this._recordIntensityArray[settingIndex], this._currentIntensityArray.Length);
            return 10;
        }

        public  bool CalculateParameters(uint settingIndex)
        {
            // get spectrum from meter
            if (!this.GetSpectrumFromMeter())
                return false;

            // calculate result
            if(!this.CalculateMeasureResults2(settingIndex))
            {
                return false;
            }
            return true;
        }

        public  double[] GetXWavelength()
        {
            return this._wavelengthArray;
        }

        public  double[] GetYSpectrumIntensity(uint index)
        {
            if (index < 0 || index >= this._recordIntensityArray.Length)
            {
                index = 0;
            }
            return this._recordIntensityArray[index];
        }

        public  double[][] GetYSpectrumIntensityAll()
        {
            return this._recordIntensityArray;
        }

        public  double[] GetYAbsoluateSpectrum(uint index)
        {
            if (index < this._absoluteSepctrum.Length)
                return this._absoluteSepctrum[index];
            else
                return null;
        }

        public  double[][] GetYAbsoluateSpectrumAll()
        {
            return this._absoluteSepctrum;
        }

        public  double[] GetDarkSample(uint Darkcount, uint intTime)
        {
            Darkcount = 1;
            double[] averageDarkCurrent = new double[this._darkIntensityArray.Length];
            double[] tempDarkCurrent;

            // initialize averageDarkCurrent
            for (int i = 0; i < averageDarkCurrent.Length; i++)
                averageDarkCurrent[i] = 0;

            intTime = (uint)(intTime * 0.001);

            // set integral time
            this.SetIntTime(intTime);

            for (int i = 0; i < (int)Darkcount; i++)
            {
                // measure dark current but not use shutter
                this.MeasureDarkCurrent(true);

                // get dark current array
                tempDarkCurrent = this.GetDarkCurrentFromMeter();

                for (int j = 0; j < averageDarkCurrent.Length; j++)
                    averageDarkCurrent[j] += tempDarkCurrent[j];
            }

            for (int i = 0; i < averageDarkCurrent.Length; i++)
                averageDarkCurrent[i] = averageDarkCurrent[i] / Darkcount;

            this._darkIntensityArray = averageDarkCurrent;
            return this._darkIntensityArray;
        }

        public  string[] GetEPPROMConfigData()
        {
            return new string[15];
        }

        #endregion

        #region >>> Private  Method <<<

        private double[] CreatePixelArray()
        {
            return new double[LE5400Wrapper.MEAS_DATA_MAX];
        }

        private void InitDataArray(int settingCount)
        {
            // wave length array
            this._wavelengthArray = CreatePixelArray();

            // current intensity array
            this._currentIntensityArray = CreatePixelArray();

            // dark intensity array
            this._darkIntensityArray = CreatePixelArray();

            // intensity array
            this._recordIntensityArray = new double[settingCount][];

            for (int i = 0; i < this._recordIntensityArray.Length; i++)
                this._recordIntensityArray[i] = CreatePixelArray();

            // absolute spectrum array
            this._absoluteSepctrum = new double[settingCount][];

            for (int i = 0; i < this._absoluteSepctrum.Length; i++)
                this._absoluteSepctrum[i] = CreatePixelArray();

            this._recordSpectrum = new MCPD_MEASE_SPECTRUM[settingCount];
            for (int i = 0; i < this._recordSpectrum.Length; i++)
            this._recordSpectrum[i] = new MCPD_MEASE_SPECTRUM();

            this._absSpectrum = new MCPD_MEASE_SPECTRUM[settingCount];
            for(int i = 0; i < this._absoluteSepctrum.Length; i++)
                this._absSpectrum[i] = new MCPD_MEASE_SPECTRUM();


            for (int i = 0; i < this._absoluteSepctrum.Length; i++)
                this._absoluteSepctrum[i] = CreatePixelArray();

            this._recordST = new uint[settingCount];
            // optical data array
            this._opticalData = new OptiData[settingCount];
            for (int i = 0; i < this._opticalData.Length; i++)
                this._opticalData[i] = new OptiData();
        }

        /// <summary>
        /// Set integration time ( ms ).
        /// </summary>
        private bool LoadCaliFile(string seriesNum)
        {
            string pathAndFile = Path.Combine(ROOT, "CaliData_" + seriesNum + ".xml");

            if(File.Exists(pathAndFile))
            {
                this._spectroCaliData = MPI.Xml.XmlFileSerializer.Deserialize(typeof(SpectroCaliData), pathAndFile) as SpectroCaliData;

                if(this._spectroCaliData != null)
                {
                    return true;
                }
                else
                {
                    this._spectroCaliData = null;
                    return false;
                }
            }
            else
            {
                this._spectroCaliData = null;
                return false;
            }
        }

        /// <summary>
        /// Reset Data
        /// </summary>
        public virtual void ResetData(uint index, uint status)
        {
            if(index < 0 || index >= this._opticalData.Length)
                return;

            if(status == 0)
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
                this._opticalData[index].WLPNIR = 0.0d;
                this._opticalData[index].WLD = 0.0d;
                this._opticalData[index].WLCv = 0.0d;
                this._opticalData[index].WLCp = 0.0d;
                this._opticalData[index].FWHM = 0.0d;

                this._opticalData[index].Watt = 0.0d;
                this._opticalData[index].Lm = 0.0d;
                this._opticalData[index].Lx = 0.0d;

                this._opticalData[index].GeneralCRI = 0.0d;
            }
            else if(status == 1)
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
                this._opticalData[index].WLPNIR = 9999.999d;
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
                this._opticalData[index].WLPNIR = -9999.999d;
                this._opticalData[index].WLD = -9999.999d;
                this._opticalData[index].WLCv = -9999.999d;
                this._opticalData[index].WLCp = -9999.999d;
                this._opticalData[index].FWHM = -9999.999d;

                this._opticalData[index].Watt = -9999.999d;
                this._opticalData[index].Lm = -9999.999d;
                this._opticalData[index].Lx = -9999.999d;

                this._opticalData[index].GeneralCRI = -9999.999d;
            }

        }

        /// <summary>
        /// Set integration time ( ms ).
        /// </summary>
        private void SetIntTime(uint time)
        {         
            if (time > this._intTimeMax)
            {
                time = this._intTimeMax;
            }
            else if (time < this._intTimeMin)
            {
                time = this._intTimeMin;
            }

            this._msrtCond.GateTime = (int)time;
            this._msrtCond.DarkCloseSkipCount = (int)(500 / time) + 2;
            this._msrtCond.SyncSt = 1;
            // LE 5400 SkipCount Design
            // this._msrtCond.DarkOpenSkipCount = 2;
            this._msrtCond.CorrectEng = 1;
            this._msrtCond.SigMode = 0;
            LE5400Wrapper.MeasurementResult.SetMeasCondAll(this._conncetID, ref this._msrtCond);
        }

        private void MeasureDarkCurrent(bool isCloseShutter)
        {           
            int shutterMode=1;

            if(isCloseShutter)
            {
                shutterMode=0;
            }
            else
            {
                shutterMode=1;
            }
            //Shutter Mode(1:OPEN 0:CLOSE)
            LE5400Wrapper.MeasurementResult.AQUDark(this._conncetID, shutterMode);
            LE5400Wrapper.MeasurementResult.GetSpectrum(this._conncetID, 0, ref  this._msrtSpectrum);
            this._wavelengthArray = this._msrtSpectrum.pdWave;      
        }

        private double[] GetDarkCurrentFromMeter()
        {
            double[] darkArray = this.CreatePixelArray();
            LE5400Wrapper.MeasurementResult.GetDark(this._conncetID, out this._darkData);

            try
            {
                for (int i = 0; i < darkArray.Length; i++) // VisibleNum and DeadNum are set in Initialize() 
                {
                    darkArray[i] = this._darkData.Data[i];
                }
                return darkArray;
            }
            catch
            {

            }
            return null;
        }

        private bool CheckSaturation()
        {
            return this._isSaturated = false;
        }

        private bool MeterTrigger(uint intTime)
        {
            // set the integration time 
            this.SetIntTime(intTime);

            if (LE5400Wrapper.MeasurementResult.AQUMeas(this._conncetID) != (int)EMcpdApiErrorCode.SUCCESS)
            {
                return false;
            }

            LE5400Wrapper.MeasurementResult.GetSpectrum(this._conncetID, 0, ref  this._msrtSpectrum); // get Relative Spectrum
            this._currentIntensityArray = this._msrtSpectrum.pdValue ;
            //return this._msrtSpectrum.pdValue;m.pdValue;
            return true;
        }

        private bool GetSpectrumFromMeter()
        {
            return true;
        }

        private bool CalculateMeasureResults2(uint settingIndex)
        {
            if(this._msrtSetting == null)
                return false;

            if(settingIndex >= this._msrtSetting.Length)
                return false;

            if(settingIndex >= this._recordST.Length)
                return false;

            EMcpdApiErrorCode rtn = EMcpdApiErrorCode.SUCCESS;

            try
            {
                this._msrtCond.GateTime = (int)this._recordST[settingIndex];
                LE5400Wrapper.MeasurementResult.SetMeasCondAll(this._conncetID, ref this._msrtCond);

                rtn = (EMcpdApiErrorCode)LE5400Wrapper.MeasurementResult.Calc(this._conncetID, ref this._calcCond, ref this._recordSpectrum[settingIndex], ref this._msrtResult);

                if(rtn != EMcpdApiErrorCode.SUCCESS)
                {
                    return false;
                }
                MCPD_MEASE_SPECTRUM absSpectrum = new MCPD_MEASE_SPECTRUM();

                LE5400Wrapper.MeasurementResult.GetSpectrum(this._conncetID, 1, ref  absSpectrum);

                this._absoluteSepctrum[settingIndex] = absSpectrum.pdValue;
                this._wavelengthArray = absSpectrum.pdWave;

                this._spam.ResetAllParam();

               // this._spam.ComputeColorPurityWLD_CCT_CRI(absSpectrum.pdWave, this._absoluteSepctrum[settingIndex], true);
               // this._opticalData[settingIndex].Watt = 0.001 * this._spam.ComputeWattA(absSpectrum.pdWave, this._absoluteSepctrum[settingIndex], this._devSetting.StartWavelength, this._devSetting.EndWavelength, EIntegrateMode.SIMPSONS);
               // this._opticalData[settingIndex].Lm = 0.001 * 0.001 * this._spam.ComputeLuminousFluxLumen(absSpectrum.pdWave, this._absoluteSepctrum[settingIndex]);

                //// radiometric integral
                this._opticalData[settingIndex].Watt = this._msrtResult.Sum[0] * 1000;
                //// photometric integral
                this._opticalData[settingIndex].Lm = this._msrtResult.LY;
                //// centroid
                this._opticalData[settingIndex].WLCp = this._msrtResult.CentroidWave[0];
                //this._opticalData[settingIndex].WLCv = this._msrtResult.CentroidWave[0];
                //this._opticalData[settingIndex].Watt = this._msrtResult.Sum[0];
                //// retrieve the tristimulus
                //double X = 0;
                //double Y = 0;
                //double Z = 0;
                this._opticalData[settingIndex].CIE1931X = this._msrtResult.LX;
                this._opticalData[settingIndex].CIE1931Y = this._msrtResult.LY;
                this._opticalData[settingIndex].CIE1931Z = this._msrtResult.LZ;
                //this._opticalData[settingIndex].CIE1931X = this._spam.TristimulusX;
                //this._opticalData[settingIndex].CIE1931Y = this._spam.TristimulusY;
                //this._opticalData[settingIndex].CIE1931Z = this._spam.TristimulusX;
                //// retrieve the CIE color coordinates
                this._opticalData[settingIndex].CIE1931x = this._msrtResult.X;
                this._opticalData[settingIndex].CIE1931y = this._msrtResult.Y;
                this._opticalData[settingIndex].CIE1931z = (1 - this._msrtResult.X - this._msrtResult.Y);
                //double illuminantRefX = 0.3333;
                //double illuminantRefY = 0.3333;
                this._opticalData[settingIndex].WLD = this._msrtResult.Dominant;
                this._opticalData[settingIndex].Purity = this._msrtResult.Purity;

                //// CCT, 
                this._opticalData[settingIndex].CCT = this._msrtResult.Tc;
                //double cri = 0;
                //for(int i = 0; i < 8; i++)
                //{
                //    cri = cri + this._msrtResult.R[i];
                //}
                //this._opticalData[settingIndex].GeneralCRI = cri / 8;
                this._opticalData[settingIndex].GeneralCRI = this._msrtResult.R[0];

                //// get special CRI
                for(int i = 0; i < this._opticalData[settingIndex].SpecialCRI.Length; i++)
                {
                    this._opticalData[settingIndex].SpecialCRI[i] = this._msrtResult.R[i+1];
                }

                //// WLP
                //int indexPixel = MpiSPAM.CalcMaxCount(this._absoluteSepctrum[settingIndex], 0, 0);
                //double peakWave = 0.0d;
                //this._spam.ComputeSpectrumData(indexPixel, this._wavelengthArray, this._absoluteSepctrum[settingIndex]);
                //peakWave = MpiSPAM.CaculatePeakWavelength(indexPixel, this._wavelengthArray, this._absoluteSepctrum[settingIndex], 2);

                //this._opticalData[settingIndex].WLP = peakWave;
                //this._opticalData[settingIndex].WLP2 = this._msrtResult.PeakWave[0];
                //this._opticalData[settingIndex].FWHM = this._spam.FWHM;

                this._opticalData[settingIndex].WLP = this._msrtResult.PeakWave[0];
                this._opticalData[settingIndex].WLP2 = this._msrtResult.PeakWave[1];
                this._opticalData[settingIndex].FWHM = this._msrtResult.PeakHalfWidth[0];

                //// FWHM
                //this._opticalData[settingIndex].FWHM = CAS4Wrapper.MeasurementResult.GetWidth(this._casID);
                //CheckCASError();

                //// 2nd central moment
                //this._secondCentralMoment = this.CalculateCentralMoment( 2, meanLambda );
                //// 3rd central moment
                //this._thirdCentralMoment = this.CalculateCentralMoment( 3, meanLambda );
            }
            catch
            {
                this._errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
                return false;
            }

            return true;
        }

        private bool CalculateMeasureResults(uint settingIndex)
        {
            if (this._msrtSetting == null)
                return false;

            if (settingIndex >= this._msrtSetting.Length)
                return false;

            if (settingIndex >= this._recordST.Length)
                return false;

            EMcpdApiErrorCode rtn = EMcpdApiErrorCode.SUCCESS;

            try
            {
                this._msrtCond.GateTime = (int)this._recordST[settingIndex];
                LE5400Wrapper.MeasurementResult.SetMeasCondAll(this._conncetID, ref this._msrtCond);

                rtn = (EMcpdApiErrorCode)LE5400Wrapper.MeasurementResult.Calc(this._conncetID, ref this._calcCond, ref this._recordSpectrum[settingIndex], ref this._msrtResult);

                if(rtn != EMcpdApiErrorCode.SUCCESS)
                {
                    return false;
                }
                MCPD_MEASE_SPECTRUM absSpectrum = new MCPD_MEASE_SPECTRUM();

                LE5400Wrapper.MeasurementResult.GetSpectrum(this._conncetID, 1, ref  absSpectrum);

                this._absoluteSepctrum[settingIndex] = this.CalcAbsSpectrum(absSpectrum.pdValue);
                this._wavelengthArray = absSpectrum.pdWave;

                this._spam.ResetAllParam();





                this._spam.ComputeColorPurityWLD_CCT_CRI(absSpectrum.pdWave, this._absoluteSepctrum[settingIndex], true);
                this._opticalData[settingIndex].Watt = 0.001*this._spam.ComputeWattA(absSpectrum.pdWave, this._absoluteSepctrum[settingIndex], this._devSetting.StartWavelength, this._devSetting.EndWavelength, EIntegrateMode.SIMPSONS);
                this._opticalData[settingIndex].Lm = 0.001*0.001*this._spam.ComputeLuminousFluxLumen(absSpectrum.pdWave, this._absoluteSepctrum[settingIndex]);
      
                //// radiometric integral
                //  this._opticalData[settingIndex].Watt = this._msrtResult.Sum[0];
                //// photometric integral

                //// centroid
                this._opticalData[settingIndex].WLCp = this._msrtResult.CentroidWave[0];
               //this._opticalData[settingIndex].WLCv = this._msrtResult.CentroidWave[0];
               //this._opticalData[settingIndex].Watt = this._msrtResult.Sum[0];
                //// retrieve the tristimulus
                //double X = 0;
                //double Y = 0;
                //double Z = 0;
                this._opticalData[settingIndex].CIE1931X = this._msrtResult.LX;
                this._opticalData[settingIndex].CIE1931Y = this._msrtResult.LY;
                this._opticalData[settingIndex].CIE1931Z = this._msrtResult.LZ;

                this._opticalData[settingIndex].CIE1931X = this._spam.TristimulusX;
                this._opticalData[settingIndex].CIE1931Y = this._spam.TristimulusY;
                this._opticalData[settingIndex].CIE1931Z = this._spam.TristimulusX;
                //// retrieve the CIE color coordinates
                this._opticalData[settingIndex].CIE1931x = this._spam.CIEx;
                this._opticalData[settingIndex].CIE1931y = this._spam.CIEy;
                this._opticalData[settingIndex].CIE1931z = (1 - this._msrtResult.X - this._msrtResult.Y);
                //double illuminantRefX = 0.3333;
                //double illuminantRefY = 0.3333;
                this._opticalData[settingIndex].WLD = this._msrtResult.Dominant;
                this._opticalData[settingIndex].Purity = this._msrtResult.Purity;

                //// CCT, 
                this._opticalData[settingIndex].CCT = this._spam.CCT;
                double cri = 0;
                for (int i = 0; i < 8; i++)
                {
                    cri = cri + this._msrtResult.R[i];
                }
              //  this._opticalData[settingIndex].GeneralCRI = cri / 8;
                this._opticalData[settingIndex].GeneralCRI = this._spam.CRI;

                //// get special CRI
                for(int i = 0; i < this._opticalData[settingIndex].SpecialCRI.Length; i++)
                {
                    this._opticalData[settingIndex].SpecialCRI[i] = this._spam.SpecialCRI[i];
                }            

                //// WLP
                int indexPixel = MpiSPAM.CalcMaxCount(this._absoluteSepctrum[settingIndex], 0, 0);
                double peakWave = 0.0d;
                this._spam.ComputeSpectrumData(indexPixel, this._wavelengthArray, this._absoluteSepctrum[settingIndex]);
                peakWave = MpiSPAM.CaculatePeakWavelength(indexPixel, this._wavelengthArray, this._absoluteSepctrum[settingIndex], 2);

                this._opticalData[settingIndex].WLP =peakWave;
                this._opticalData[settingIndex].WLP2 = this._msrtResult.PeakWave[0];
                this._opticalData[settingIndex].FWHM = this._spam.FWHM;

                //this._opticalData[settingIndex].WLP = this._msrtResult.PeakWave[0];
                //this._opticalData[settingIndex].WLP2 = this._msrtResult.PeakWave[1];
                //this._opticalData[settingIndex].FWHM = this._msrtResult.PeakHalfWidth[0];
                
                //// FWHM
                //this._opticalData[settingIndex].FWHM = CAS4Wrapper.MeasurementResult.GetWidth(this._casID);
                //CheckCASError();

                //// 2nd central moment
                //this._secondCentralMoment = this.CalculateCentralMoment( 2, meanLambda );
                //// 3rd central moment
                //this._thirdCentralMoment = this.CalculateCentralMoment( 3, meanLambda );
            }
            catch
            {
                this._errorNumber = EDevErrorNumber.SpectrometerDevice_Init_Err;
                return false;
            }

            return true;
        }

        private int LimitTrigger(uint index)
        {
            bool meterRtn = false;
            uint secondIntTime = 0;

            //----------------------------------------------------------
            // (1) Firt, Trigger 
            //----------------------------------------------------------

           meterRtn= this.MeterTrigger((uint)this._devSetting.LimitStartTime);

            if(meterRtn == false)	// MeterTrigger has error, the subfunction will set the error code
            {
                this.ResetData(index, 0);
                return -21;
            }

            //----------------------------------------------------------
            // (2) Second Trigger 
            //-----------------------------------------------------------

            int maxPixelIndex = MpiSPAM.CalcMaxCount(this._currentIntensityArray, 10, 0.01);
            double currentCount = this._currentIntensityArray[maxPixelIndex];
            secondIntTime = (uint)this.CalcNextTime(this._devSetting.LimitStartTime, this._msrtSetting[index].LimitIntegralTime, currentCount, LIMIT_TARGET);

            meterRtn = this.MeterTrigger(secondIntTime);
            this._opticalData[index].IntegralTime = secondIntTime;
            this._recordST[index] = secondIntTime;
            this._recordSpectrum[index] = this._msrtSpectrum;

            if(meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
            {
                this.ResetData(index, 0);
                return -22;
            }
            else
            {
                return 20;		// OK
            }
        }

        private double CalcNextTime(double startTime, double upBoundTime, double currentCount, double targetCount)
        {
            double nextTime = startTime;
            double countIncreasRate = 1.0d;

            if(startTime < ((double)MIN_INTEGRATION_TIME))
            {
                startTime = (double)MIN_INTEGRATION_TIME;
            }

            if(upBoundTime > ((double)MAX_INTEGRATION_TIME))
            {
                upBoundTime = (double)MAX_INTEGRATION_TIME;
            }

            countIncreasRate = currentCount / startTime;
            nextTime = targetCount / countIncreasRate;

            if(nextTime >= upBoundTime)
            {
                nextTime = upBoundTime;
            }
            else if(nextTime <= MIN_INTEGRATION_TIME)
            {
                nextTime = (double)MIN_INTEGRATION_TIME;
            }
            else
            {
                if(nextTime >= startTime)
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

        private double[] CalcAbsSpectrum(double[] array)
        {
            double[] rtnArray = new double[array.Length];

            if(this._spectroCaliData != null || this._calibrationArray.Length < LE5400Wrapper.MEAS_DATA_MAX)
            {
                for(int i = 0; i < array.Length; i++)
                {
                    rtnArray[i] = array[i] * this._calibrationArray[i] * 1000;
                }
            }

            else
            {
                for(int i = 0; i < array.Length; i++)
                {
                    rtnArray[i] = array[i] * 1000;
                }
            }
            return rtnArray;   
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
        //            deltaLambda = this._wavelengthArray[ i ] - this._wavelengthArray[ i - 1 ];
        //        else
        //            deltaLambda = this._wavelengthArray[ i + 1 ] - this._wavelengthArray[ i ];

        //        normalization += spectrumArray[ i ] * deltaLambda;
        //        momentInt += spectrumArray[ i ] * Math.Pow( this._wavelengthArray[ i ] - mean, momentNumber ) * deltaLambda;
        //    }

        //    if ( !normalization.Equals( 0 ) ) // is it appropriate to compare double with int ? this syntex is from the sample code
        //        return ( momentInt / normalization );
        //    else
        //        return 0;
        //}

        #endregion
    }
}
