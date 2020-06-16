using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SpectroMeter
{
    //-------------------------------------------------------------------------------
    //  Ver. 1.00 , Update Date = 20110424
    //-------------------------------------------------------------------------------

	/// <summary>
	/// USB2000-Plus driver wrapper class.
	/// </summary>
    public class SeaBreezeWrapper
    {
        private int _errorCode;
        private const int CCD_PIXEL_LENGTH_2048 = 2048;
        private const int CCD_PIXEL_LENGTH_3684 = 3648;
        private double nlCoeff0 = 1;
        private double nlCoeff1 = 1;
        private double nlCoeff2 = 1;
        private double nlCoeff3 = 1;
        private double nlCoeff4 = 1;
        private double nlCoeff5 = 1;
        private double nlCoeff6 = 1;
        private double nlCoeff7 = 1;

        private double[][] rowSpectrum;
        private bool _isSetCorrectForNonlinearity;
		private int _boxcarWidth;
		private object _lockObj;

		private double[] spectrumArray;
		private int[] _toDevIntegrationTime;
        private bool _isInitOK;

        private int _ccdPixelLength = CCD_PIXEL_LENGTH_2048;

        private ISeaBreeze _seaBreeze;

        public SeaBreezeWrapper(ESpectrometerModel model = ESpectrometerModel.USB2000P)
        {
			this._lockObj = new object();

            //this.nlCoeff0 = 0.862276;
            //this.nlCoeff1 = 8.013E-06;
            //this.nlCoeff2 = -2.00042E-10;
            //this.nlCoeff3 = 3.26229E-15;
            //this.nlCoeff4 = -4.80686E-20;
            //this.nlCoeff5 = 4.75802E-25;
            //this.nlCoeff6 = 7.21702E-30;
            //this.nlCoeff7 = -1.729992E-34;
  

            switch(model)
            {
                case ESpectrometerModel.HR2000P:
                    {
                        this._ccdPixelLength = CCD_PIXEL_LENGTH_2048;
                        this._seaBreeze = new SeaBreezeDLL_HR2000P();
                        break;
                    }
                case ESpectrometerModel.HR4000:
                    {
                        this._ccdPixelLength = CCD_PIXEL_LENGTH_3684;
                        this._seaBreeze = new SeaBreezeDLL_HR2000P();
                        break;
                    }
                default:  // USB2000+
                    {
                        this._ccdPixelLength = CCD_PIXEL_LENGTH_2048;
                        this._seaBreeze = new SeaBreezeDLL_USB2000P();
                        break;
                    }
            }

            this._isSetCorrectForNonlinearity = false;
            this._boxcarWidth = 0;
            this.spectrumArray = new double[this._ccdPixelLength];
            this._toDevIntegrationTime = new int[1] { 10 };

            this._isInitOK = false;

        }

       // #region >>> DLL Declaration( SeaBreezeWin32 ) <<<

       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern int seabreeze_open_spectrometer(int spectrometerIndex, int errorMsrCode);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern int seabreeze_close_spectrometer(int spectrometerIndex, int errorMsrCode);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern void seabreeze_set_trigger_mode(int spectrometerIndex, int errorMsrCode, int mode);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern string seabreeze_get_error_string(int spectrometerIndex, int errorMsrCode, int length);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern void seabreeze_set_integration_time(int spectrometerIndex, int errorMsrCode, int microseconds);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern int seabreeze_get_formatted_spectrum(int spectrometerIndex, int errorMsrCode, double[] array, int length);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern int seabreeze_get_formatted_spectrum_length(int spectrometerIndex, int errorMsrCode);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern int seabreeze_get_wavelengths(int spectrometerIndex, int errorMsrCode, double[] array, int length);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern string seabreeze_get_serial_number(int spectrometerIndex, int errorMsrCode);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern int seabreeze_read_eeprom_slot(int spectrometerIndex, int errorMsrCode, int slotNumber, byte[] buffer, int length);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern int seabreeze_get_unformatted_spectrum_length(int spectrometerIndex, int errorMsrCode);
       // [DllImport(@"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll", CallingConvention = CallingConvention.Cdecl)]
       // private static extern int seabreeze_get_unformatted_spectrum(int spectrometerIndex, int errorMsrCode, byte[] array, int length);

        //#endregion

        #region >>> Private  Method <<<

        /// <summary>
        /// initialized SpectrumMeter CCD Pixel Length
        /// </summary>
        public int CCD_PIXEL_LENGTH
        {
            get { return this._ccdPixelLength; }
        }

        #endregion

        #region >>> Private  Method <<<

        public double[] DoBoxCar(double[] array, int numberOfPixelsOnEitherSideOfCenter)
         {
             double[] caculatedArray = new double[array.Length];

             for (int i = 0; i < array.Length; i++)
             {
                 List<double> data = new List<double>(2 * numberOfPixelsOnEitherSideOfCenter + 1);
                 data.Add(array[i]);
                 //decrease
                 for (int index = 0; index < numberOfPixelsOnEitherSideOfCenter; index++)
                 {
                     if ((i - numberOfPixelsOnEitherSideOfCenter) < 0)
                     {
                         data.Add(array[i]);
                     }
                     else
                     {
                         data.Add(array[i - index - 1]);
                     }
                 }
                 //increase
                 for (int index = 0; index < numberOfPixelsOnEitherSideOfCenter; index++)
                 {
                     if ((i + numberOfPixelsOnEitherSideOfCenter) >= array.Length)
                     {
                         data.Add(array[i]);
                     }
                     else
                     {
                         data.Add(array[i + index + 1]);
                     }
                 }

                 double sum = 0;

                 if (data.Count == (2 * numberOfPixelsOnEitherSideOfCenter + 1))
                 {
                     foreach (double d in data)
                     {
                         sum += d;
                     }
                 }
                 caculatedArray[i] = sum / data.Count;
             }
             return caculatedArray;
         }

        private double[] DoCorrectForNonlinearity(double[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                double factor = nlCoeff0;
                double accumulator = pixels[i];
                double pixel = pixels[i];
        
                factor += accumulator * nlCoeff1;
                accumulator *= pixel;

                factor += accumulator * nlCoeff2;
                accumulator *= pixel;

                factor += accumulator * nlCoeff3;
                accumulator *= pixel;

                factor += accumulator * nlCoeff4;
                accumulator *= pixel;

                factor += accumulator * nlCoeff5;
                accumulator *= pixel;

                factor += accumulator * nlCoeff6;
                accumulator *= pixel;

                factor += accumulator * nlCoeff7;
                pixels[i] = pixel / factor;
            }
            return pixels;
        }

        //------------------------------------
        // Get NI Coeff
        //------------------------------------       
        private void GetNonlinearityCoeff()
        {
            this.nlCoeff0 = GetCoefficientFormEEPROM(6);
            this.nlCoeff1 = GetCoefficientFormEEPROM(7);
            this.nlCoeff2 = GetCoefficientFormEEPROM(8);
            this.nlCoeff3 = GetCoefficientFormEEPROM(9);
            this.nlCoeff4 = GetCoefficientFormEEPROM(10);
            this.nlCoeff5 = GetCoefficientFormEEPROM(11);
            this.nlCoeff6 = GetCoefficientFormEEPROM(12);
            this.nlCoeff7 = GetCoefficientFormEEPROM(13);
        }

        private string GetSeriesNumber()
        {
            byte[] buffer = new byte[11];

            //seabreeze_read_eeprom_slot(0, this._errorCode, 0, buffer, 11);
            this._seaBreeze.ReadEepromSlot(0, this._errorCode, 0, buffer, 11);
            
            Encoding unicode = Encoding.ASCII;

            string str = unicode.GetString(buffer);

            if (str.Contains("\0"))
            {
                str = str.Replace("\0", "");
            }

            return str;
        }

        private double GetCoefficientFormEEPROM(int eppromIndex)
        {
            double coeffvalue = 0;
            byte[] epprom = new byte[17];

            //seabreeze_read_eeprom_slot(0, this._errorCode, eppromIndex, epprom, 17);
            this._seaBreeze.ReadEepromSlot(0, this._errorCode, eppromIndex, epprom, 17);

            Encoding unicode = Encoding.ASCII;
            string str = unicode.GetString(epprom);

            int idx = str.IndexOf("\0");

            if (idx > 0)
            {
                str = str.Substring(0, idx);
            }

            coeffvalue = double.Parse(str);
            return coeffvalue;
        }
 
        #endregion

        #region >>> Public  Method <<<

        public void CreateWrapper()
        {     
            // no support
        }

        public bool Init()
        {
            if (this._seaBreeze == null)
                return false;

            if (System.IO.File.Exists(this._seaBreeze.SeaBreezeImportDllPath) == false)
                return false;

            this._isInitOK = true;
            return true;
        }

        public void openAllSpectrometers()
        {
            //seabreeze_open_spectrometer(0, _errorCode);
            this._seaBreeze.Open(0, _errorCode);

            GetNonlinearityCoeff();    
        }

        public string[] GetEPPROMConfigData()
        {
            string[] EPPROMConfigData = new string[17];

            for (int i = 0; i < EPPROMConfigData.Length; i++)
            {
                byte[] epprom = new byte[17];
                //seabreeze_read_eeprom_slot(0, this._errorCode, i, epprom, 17);
                this._seaBreeze.ReadEepromSlot(0, this._errorCode, i, epprom, 17);
                Encoding unicode = Encoding.ASCII;
                if (epprom != null)
                {
                    string str = unicode.GetString(epprom);
                    str = str.Trim('\0');
                    EPPROMConfigData[i] = str;
                }
            }
            return EPPROMConfigData;
        }

        public double GetCompensatedMaxCount()
        {
            double[] data = new double[] { 65535 };
            data = DoCorrectForNonlinearity(data);
            return data[0];
        }

        public int getIntegrationTime()
        {
            return 0;
        }

        public string getSerialNumber()
        {
            return GetSeriesNumber();
        }

        public string getApiVersion()
        {
            return "no supoort";
        }

        public int getNumberOfSpectrometersFound()
        {
			//int sptMeterCount = seabreeze_get_formatted_spectrum_length(0, this._errorCode);
            int sptMeterCount = this._seaBreeze.GetFormattedSpectrumLength(0, this._errorCode);

			this._toDevIntegrationTime = new int[1];

            for (int i = 0; i < this._toDevIntegrationTime.Length ;  i++)
			{
				this._toDevIntegrationTime[i] = 10000;		// us
				this.setIntegrationTime(i, this._toDevIntegrationTime[i]);
			}
			return sptMeterCount;
        }

        public double[] getSpectrum(int spectrometerIndex)
        {
			lock (this._lockObj)
			{
               // seabreeze_get_error_string(spectrometerIndex, this._errorCode, 4);

				//seabreeze_get_formatted_spectrum(spectrometerIndex, this._errorCode, spectrumArray, CCD_PIXEL_LENGTH);
                this._seaBreeze.GetFormattedSpectrum(spectrometerIndex, this._errorCode, spectrumArray, this._ccdPixelLength);

				//System.Threading.Thread.Sleep(0);
				//Console.WriteLine("Get spectrum successfully");
				//Correct For Nonlinear
				if (this._isSetCorrectForNonlinearity == true)
				{
					spectrumArray = this.DoCorrectForNonlinearity(spectrumArray);
				}
                spectrumArray[2] = spectrumArray[3];
				return spectrumArray;
			}
        }

        public void ResetSpectrumArray()
        {
				Array.Clear(spectrumArray, 0, spectrumArray.Length);

				//for (int i = 0; i < 2048; i++)
				//{
				//    spectrumArray[i] = 0;
				//}
        }

        public double[] getWavelengths(int spectrometerIndex)
        {
            double[] wavelengthArray = new double[this._ccdPixelLength];

            //seabreeze_get_wavelengths(spectrometerIndex, this._errorCode, wavelengthArray, CCD_PIXEL_LENGTH);
            this._seaBreeze.GetWavelengths(spectrometerIndex, this._errorCode, wavelengthArray, this._ccdPixelLength);

            return wavelengthArray;
        }

        public void setIntegrationTime(int spectrometerIndex, int IntegratingTime)
        {
			if (this._toDevIntegrationTime[spectrometerIndex] != IntegratingTime)
			{
				//seabreeze_set_integration_time(spectrometerIndex, this._errorCode, IntegratingTime);
                this._seaBreeze.SetIntegrationTime(spectrometerIndex, this._errorCode, IntegratingTime);

				this._toDevIntegrationTime[spectrometerIndex] = IntegratingTime;
			}
        }

        public void setBoxcarWidth(int spectrometerIndex, int numberOfPixelsOnEitherSideOfCenter)
        {
            this._boxcarWidth = numberOfPixelsOnEitherSideOfCenter;
        }

        public void setScansToAverage(int spectrometerIndex, int numberOfScansToAverage)
        {
            // no support
        }

        public void setCorrectForDetectorNonlinearity(int spectrometerIndex, int enable)
        {
            if (enable == 1)
            {
                this._isSetCorrectForNonlinearity = true;
            }
            else
            {
                this._isSetCorrectForNonlinearity = false;
            }
        }

        public bool getCorrectForDetectorNonlinearity()
        {
            // no support
            return this._isSetCorrectForNonlinearity;
        }

        public bool isSaturation()
        {
            // no support
            return true;
        }

        public void setExternalTriggerMode(int spectrometerIndex, int mode)
        {
            //seabreeze_set_trigger_mode(0, this._errorCode, mode);
            this._seaBreeze.SetTriggerMode(0, this._errorCode, mode);
        }

        public void Close()
        {
            this._seaBreeze.Close(0, this._errorCode);
        }

        #endregion
    }
}
