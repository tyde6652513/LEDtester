using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SpectroMeter
{
    //-------------------------------------------------------------------------------
	// USB2000-Plus driver wrapper class which package the "OmniDriver32.dll" and
	// "common32.dll"
	//
    //  Ver. 1.00 , Update Date = 20110411
    //-------------------------------------------------------------------------------

	/// <summary>
	/// USB2000-Plus driver wrapper class by OmniDriver32.dll
	/// </summary>
    public class OceanWrapper
    {
        private const int CCD_PIXEL_LENGTH = 2048;
        private double[][] rowSpectrum;
        private double[] spectrumArray;
		private int _wrapperHandle;

        public OceanWrapper()
        {     
   
        }

        #region >>> DLL Declaration( Login, Set accesor and Configure ) <<<

        [DllImport("OmniDriver32.dll")]
        private static extern int Wrapper_Create();
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Wrapper_getBuildNumber(int wrapper_handle);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Wrapper_getNumberOfPixels(int wrapper_handle, int spectrometerIndex);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_getSerialNumber(int wrapper_handle, int spectrometerIndex, int jstring_handle);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Wrapper_openAllSpectrometers(int wrapper_handle);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_getSpectrum_1(int wrapper_handle, int spectrometerIndex, int channelIndex, int double_array_handle);     
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_setIntegrationTime_1(int wrapper_handle, int spectrometerIndex, int channelIndex, int microseconds);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_setBoxcarWidth(int wrapper_handle, int spectrometerIndex, int numberOfPixelsOnEitherSideOfCenter);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Wrapper_getBoxcarWidth(int wrapper_handle, int spectrometerIndex);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_setExternalTriggerMode(int wrapper_handle, int spectrometerIndex, int mode);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Wrapper_getNumberOfSpectrometersFound(int wrapper_handle );
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_getWavelengths(int wrapper_handle, int spectrometerIndex, int double_array_handle);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern  void Wrapper_setCorrectForDetectorNonlinearity(int wrapper_handle, int spectrometerIndex, int enable);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Wrapper_getIntegrationTime(int wrapper_handle, int spectrometerIndex);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_setScansToAverage(int wrapper_handle, int spectrometerIndex, int numberOfScansToAverage);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_getApiVersion(int wrapper_handle, int jstring_handle);
         [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
         private static extern bool Wrapper_getCorrectForDetectorNonlinearity(int wrapper_handle, int spectrometerIndex);
         [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
         private static extern void Wrapper_highSpdAcq_AllocateBuffer(int wrapper_handle, int spectrometerIndex, int numberOfSpectra);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
         private static extern void Wrapper_highSpdAcq_StartAcquisition(int wrapper_handle, int spectrometerIndex);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_highSpdAcq_StopAcquisition(int wrapper_handle);
        [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Wrapper_highSpdAcq_GetSpectrum(int wrapper_handle, int spectrumNumber, int double_array_handle);
         [DllImport("OmniDriver32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool Wrapper_isSaturated(int wrapper_handle, int spectrometerIndex);


        #endregion

        #region >>> DLL Declaration( Common32) <<<

        [DllImport("common32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int DoubleArray_Create();
        [DllImport("common32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DoubleArray_Destroy(int double_array_handle);
        [DllImport("common32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double DoubleArray_getElementAt(int double_array_handle, int index);
        [DllImport("common32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int JString_Create();
        [DllImport("common32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void JString_Destroy(int jstring_handle);
        [DllImport("common32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern string JString_getASCII(int jstring_handle);
			
        #endregion

        #region >>> Public Method 20110406 <<< 
      
        public void CreateWrapper()
        {
            this._wrapperHandle = Wrapper_Create();
        }

        public void openAllSpectrometers()
        {
            Wrapper_openAllSpectrometers(this._wrapperHandle);
        }

        public int getIntegrationTime()
        {
            return Wrapper_getIntegrationTime(this._wrapperHandle, 0);
		}        

        public string getSerialNumber()
        {
            string seriesNum = "";
            int jstring_handle;

            jstring_handle = JString_Create();
            Wrapper_getSerialNumber(this._wrapperHandle, 0, jstring_handle);
            seriesNum=JString_getASCII(jstring_handle);
            JString_Destroy(jstring_handle);

            return seriesNum;
        }

        public string getApiVersion()
        {
            string ApiVersion = "";
            int jstring_handle;

            jstring_handle = JString_Create();
            Wrapper_getApiVersion(this._wrapperHandle, jstring_handle);
            ApiVersion = JString_getASCII(jstring_handle);
            JString_Destroy(jstring_handle);

            return ApiVersion;
        }

        public int getNumberOfSpectrometersFound()
        {
            return Wrapper_getNumberOfSpectrometersFound(this._wrapperHandle);
        }

        public double[] getSpectrum(int spectrometerIndex)
        {
           // double[] spectrumArray = new double[CCD_PIXEL_LENGTH];
            int double_array_handle;        

            double_array_handle = DoubleArray_Create();
            Wrapper_getSpectrum_1(this._wrapperHandle, spectrometerIndex, 0, double_array_handle);
            for (int i = 0; i < CCD_PIXEL_LENGTH; i++)
            {
                this.spectrumArray[i] = DoubleArray_getElementAt(double_array_handle, i);
            }
            DoubleArray_Destroy(double_array_handle);

            return this.spectrumArray;
        }


        public double[] getWavelengths(int spectrometerIndex)
        {
            double[] wavelengthArray = new double[CCD_PIXEL_LENGTH];
            int wavelength_array_handle;

            wavelength_array_handle = DoubleArray_Create();
            Wrapper_getWavelengths(this._wrapperHandle, spectrometerIndex, wavelength_array_handle);
            for (int i = 0; i < CCD_PIXEL_LENGTH; i++)
            {
                wavelengthArray[i] = DoubleArray_getElementAt(wavelength_array_handle, i);
            }
            DoubleArray_Destroy(wavelength_array_handle);

            return wavelengthArray;
        }

        public void setIntegrationTime(int spectrometerIndex,int SetIntegratingTime)
        {
            Wrapper_setIntegrationTime_1(this._wrapperHandle, spectrometerIndex, 0, (int)SetIntegratingTime);
        }

        public void setBoxcarWidth(int spectrometerIndex, int numberOfPixelsOnEitherSideOfCenter)
        {
            Wrapper_setBoxcarWidth(this._wrapperHandle, spectrometerIndex, numberOfPixelsOnEitherSideOfCenter);
        }

		public void  setScansToAverage(int spectrometerIndex, int numberOfScansToAverage)
		{
			Wrapper_setScansToAverage(this._wrapperHandle,spectrometerIndex, numberOfScansToAverage);
		}

        public void setCorrectForDetectorNonlinearity(int spectrometerIndex,int enable)
        {
            Wrapper_setCorrectForDetectorNonlinearity(this._wrapperHandle, spectrometerIndex, enable);
        }

        public bool getCorrectForDetectorNonlinearity()
        {
            return Wrapper_getCorrectForDetectorNonlinearity(this._wrapperHandle, 0);
        }

        public bool isSaturation()
        {
            return Wrapper_isSaturated(this._wrapperHandle,0);
        }

        public void setExternalTriggerMode(int spectrometerIndex, int mode)
        {
            Wrapper_setExternalTriggerMode(this._wrapperHandle, spectrometerIndex, mode);
        }

        #endregion

    }
}
