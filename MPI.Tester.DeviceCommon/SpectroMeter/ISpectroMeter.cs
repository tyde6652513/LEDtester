using System;
using System.Collections.Generic;
using System.Text;


namespace MPI.Tester.DeviceCommon
{
	/// <summary>
	/// Interface for spectro meter.
	/// </summary>
    public interface ISpectroMeter
    {
		/// <summary>
		/// Serial number.
		/// </summary>
		string SerialNumber
		{
			get;
		}

		/// <summary>
		/// API or driver version.
		/// </summary>
		string Version
		{
			get;
		}

		/// <summary>
		/// Error number.
		/// </summary>
		EDevErrorNumber ErrorNumber
    {
			get;
		}

		/// <summary>
		/// Spectrum data.
		/// </summary>
		OptiData[] Data
		{
			get;
		}

		double[] DarkIntensityArray
		{
			get;
		}

        #region >>> Method <<<

		/// <summary>
		/// Initialize device.
		/// </summary>
        bool Init(int deviceNum, string spectroMeterSN, string sphereSN);

		/// <summary>
		/// Close device.
		/// </summary>
        void Close();

		/// <summary>
		/// Set device setting.
		/// </summary>
		bool SetConfigToMeter(OptiDevSetting devSetting);

		/// <summary>
		/// Set measurement setting.
		/// </summary>
		bool SetParamToMeter( OptiSettingData[] msrtSetting );

		/// <summary>
		/// Trigger measurment function.
		/// </summary>
		int Trigger(uint settingIndex);

		/// <summary>
		/// Calculate parameters.
		/// </summary>
		bool CalculateParameters(uint settingIndex);

		/// <summary>
		/// Get all values of x coordinate in spectrum. ( Wave Length Unit : nm )
		/// </summary>
        double[] GetXWavelength();

		/// <summary>
		/// Get all values of y coordinate in spectrum. ( Intensity Unit : ADC Count )
		/// </summary>
        double[] GetYSpectrumIntensity(uint index);

		/// <summary>
		/// Get all values of y coordinate in spectrum on all measure settings.
		/// [ MsrtSettingIndex ][ XCoordinateIndex ] ( Intensity Unit : ADC Count )
		/// </summary>
		double[][] GetYSpectrumIntensityAll();

		/// <summary>
		/// Get all values of y coordinate that after compensated in spectrum. ( Intensity Unit : uW )
		/// </summary>
        double[] GetYAbsoluateSpectrum(uint index);

		/// <summary>
		/// Get all values of y coordinate that after compensated in spectrum on all measure settings.
		/// [ MsrtSettingIndex ][ XCoordinateIndex ] ( Intensity Unit : uW )
		/// </summary>
		double[][] GetYAbsoluateSpectrumAll();

		/// <summary>
		/// Get dark spectrum.
		/// </summary>
		double[] GetDarkSample(uint Darkcount, uint intTime);

		/// <summary>
		/// Get EPPROM configuration.
		/// </summary>
        string[] GetEPPROMConfigData();

        #endregion
    }
}
