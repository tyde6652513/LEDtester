using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SpectroMeter
{
	/// <summary>
	/// Base class for spectro meter.
	/// </summary>
	public abstract class SpectroMeterBase : ISpectroMeter
	{
		private object _lockObj;

		private int _ccdPixelLength;
		
		protected string _version;
		protected string _serialNumber;

		protected OptiDevSetting _devSetting;
		protected OptiMsrtSetting[] _msrtSetting;

		protected SpectroCaliData _spectroCaliData;
		
		protected OptiData[] _opticalData;

		protected EDevErrorNumber _errorNumber;

        protected double[] _darkIntensityArray;

		/// <summary>
		/// Constructor.
		/// </summary>
		public SpectroMeterBase( int CCDPixelLength )
		{
			this._lockObj = new object();

			this._ccdPixelLength = CCDPixelLength;

			this._devSetting = new OptiDevSetting();
			this._opticalData = new OptiData[ 1 ] { new OptiData() };

			this._errorNumber = EDevErrorNumber.Device_NO_Error;
		}

		#region >>> Public Property <<<

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

		/// <summary>
		/// Pixel array length.
		/// </summary>
		public abstract int PixelArrayLength { get; }

		#endregion

		#region >>> Public Method <<<

		public abstract bool Init( int deviceNum, string spectroMeterSN, string sphereSN );
		public abstract void Close();
	
		public abstract bool SetDeviceSetting( OptiDevSetting devSetting );
		public abstract bool SetMsrtSetting( OptiMsrtSetting[] msrtSetting );
		public abstract int Trigger( uint settingIndex );

		public abstract bool CalculateParameters( uint settingIndex );
		public abstract double[] GetDarkSample( uint Darkcount, uint intTime );
		public abstract string[] GetEPPROMConfigData();
		public abstract double[] GetXWavelength();
		public abstract double[] GetYAbsoluateSpectrum( uint index );
		public abstract double[][] GetYAbsoluateSpectrumAll();
		public abstract double[] GetYSpectrumIntensity( uint index );
		public abstract double[][] GetYSpectrumIntensityAll();

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

		#endregion
	}
}
