using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SpectroMeter.IS
{
	/// <summary>
	/// DLL wrapper for CAS4.
	/// </summary>
	internal class CAS4Wrapper
	{
		private const string ModuleName = "CAS4.DLL";

		private const int MessageStringLength = 256;

		/// <summary>
		/// Error handling.
		/// </summary>
		public class ErrorHandling
		{
			#region >>> DLL Import <<<

			[DllImport( ModuleName, EntryPoint = "casGetError" )]
			public static extern int GetError( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casGetErrorMessage", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern int casGetErrorMessage( int AError, StringBuilder ADest, int ASize );

			#endregion

			#region >>> Public Static Method <<<

			public static int GetErrorMessage( int AError, out string msg )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				int ret = casGetErrorMessage( AError, sb, sb.Capacity );

				msg = sb.ToString();

				return ret;
			}

			#endregion
		}

		/// <summary>
		/// Device handling.
		/// </summary>
		public class DeviceHandling
		{
			#region >>> DLL Import <<<

			// Device Handles and Interfaces
			[DllImport( ModuleName, EntryPoint = "casCreateDevice" )]
			public static extern int CreateDevice();

			[DllImport( ModuleName, EntryPoint = "casCreateDeviceEx" )]
			public static extern int CreateDeviceEx( EInterfaceType AInterfaceType, int AInterfaceOption );

			[DllImport( ModuleName, EntryPoint = "casChangeDevice" )]
			public static extern int ChangeDevice( int ADevice, EInterfaceType AInterfaceType, int AInterfaceOption );

			[DllImport( ModuleName, EntryPoint = "casDoneDevice" )]
			public static extern int DoneDevice( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casAssignDeviceEx" )]
			public static extern int AssignDeviceEx( int ASourceDevice, int ADestDevice, int AOption );

			[DllImport( ModuleName, EntryPoint = "casGetDeviceTypes" )]
			public static extern int GetDeviceTypes();

			[DllImport( ModuleName, EntryPoint = "casGetDeviceTypeName", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern int casGetDeviceTypeName( EInterfaceType AInterfaceType, StringBuilder Dest, int ASize );

			[DllImport( ModuleName, EntryPoint = "casGetDeviceTypeOptions" )]
			public static extern int GetDeviceTypeOptions( EInterfaceType AInterfaceType );

			[DllImport( ModuleName, EntryPoint = "casGetDeviceTypeOption" )]
			public static extern int GetDeviceTypeOption( EInterfaceType AInterfaceType, int AIndex );

			[DllImport( ModuleName, EntryPoint = "casGetDeviceTypeOptionName", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern int casGetDeviceTypeOptionName( EInterfaceType AInterfaceType, int AInterfaceOption, StringBuilder Dest, int ASize );

			[DllImport( ModuleName, EntryPoint = "casInitialize" )]
			public static extern int Initialize( int ADevice, EInitializationPerform Perform );

			#endregion

			#region >>> Public Static Method <<<

			public static int GetDeviceTypeName( EInterfaceType AInterfaceType, out string msg )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				int ret = casGetDeviceTypeName( AInterfaceType, sb, sb.Capacity );

				msg = sb.ToString();

				return ret;
			}

			public static int GetDeviceTypeOptionName( EInterfaceType AInterfaceType, int AInterfaceOption, out string msg )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				int ret = casGetDeviceTypeOptionName( AInterfaceType, AInterfaceOption, sb, sb.Capacity );

				msg = sb.ToString();

				return ret;
			}

			#endregion
		}

		/// <summary>
		/// Instrument property.
		/// </summary>
		public class InstrumentProperty
		{
			#region >>> DLL Import <<<

			[DllImport( ModuleName, EntryPoint = "casGetOptions" )]
			public static extern int GetOptions( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casSetOptionsOnOff" )]
			public static extern void SetOptionsOnOff( int ADevice, int AOptions, int AOnOff );

			[DllImport( ModuleName, EntryPoint = "casSetOptions" )]
			public static extern void SetOptions( int ADevice, int AOptions );

			[DllImport( ModuleName, EntryPoint = "casGetDeviceParameter" )]
			public static extern double GetDeviceParameter( int ADevice, EDeviceParameter AWhat );

			[DllImport( ModuleName, EntryPoint = "casSetDeviceParameter" )]
			public static extern int SetDeviceParameter( int ADevice, EDeviceParameter AWhat, double AValue );

			[DllImport( ModuleName, EntryPoint = "casGetDeviceParameterString", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern int casGetDeviceParameterString( int ADevice, EDeviceParameter AWhat, StringBuilder ADest, int ADestSize );

			[DllImport( ModuleName, EntryPoint = "casSetDeviceParameterString", CharSet = CharSet.Ansi, ExactSpelling = true )]
			public static extern int SetDeviceParameterString( int ADevice, EDeviceParameter AWhat, string AValue );

			[DllImport( ModuleName, EntryPoint = "casGetSerialNumberEx", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern int casGetSerialNumberEx( int ADevice, int AWhat, StringBuilder Dest, int ASize );

			#endregion

			#region >>> Public Static Method <<<

			public static int GetDeviceParameterString( int ADevice, EDeviceParameter AWhat, out string msg )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				int ret = casGetDeviceParameterString( ADevice, AWhat, sb, sb.Capacity );

				msg = sb.ToString();

				return ret;
			}

			public static int GetSerialNumberEx( int ADevice, int AWhat, out string msg )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				int ret = casGetSerialNumberEx( ADevice, AWhat, sb, sb.Capacity );

				msg = sb.ToString();

				return ret;
			}

			#endregion
		}

		/// <summary>
		/// Measuement.
		/// </summary>
		public class Measurement
		{
			#region >>> DLL Import <<<

			// Measurement commands
			[DllImport( ModuleName, EntryPoint = "casMeasure" )]
			public static extern int Measure( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casStart" )]
			public static extern int Start( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casFIFOHasData" )]
			public static extern int FIFOHasData( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casGetFIFOData" )]
			public static extern int GetFIFOData( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casMeasureDarkCurrent" )]
			public static extern int MeasureDarkCurrent( int ADevice );

			// Measurement Parameter
			[DllImport( ModuleName, EntryPoint = "casPerformAction" )]
			public static extern int PerformAction( int ADevice, EPerformAction AID );

			[DllImport( ModuleName, EntryPoint = "casGetMeasurementParameter" )]
			public static extern double GetMeasurementParameter( int ADevice, EMeasurementParameters AWhat );

			[DllImport( ModuleName, EntryPoint = "casSetMeasurementParameter" )]
			public static extern int SetMeasurementParameter( int ADevice, EMeasurementParameters AWhat, double AValue );

			[DllImport( ModuleName, EntryPoint = "casClearDarkCurrent" )]
			public static extern int ClearDarkCurrent( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casDeleteParamSet" )]
			public static extern int DeleteParamSet( int ADevice, int AParamSet );

			#endregion
		}

		/// <summary>
		/// Shutter and Filter.
		/// </summary>
		public class ShutterAndFilter
		{
			#region >>> DLL Import <<<

			[DllImport( ModuleName, EntryPoint = "casGetShutter" )]
			public static extern int GetShutter( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casSetShutter" )]
			public static extern void SetShutter( int ADevice, int OnOff );

			[DllImport( ModuleName, EntryPoint = "casGetFilterName", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern int casGetFilterName( int ADevice, int AFilter, StringBuilder Dest, int ASize );

			[DllImport( ModuleName, EntryPoint = "casGetDigitalOut" )]
			public static extern int GetDigitalOut( int ADevice, int APort );

			[DllImport( ModuleName, EntryPoint = "casSetDigitalOut" )]
			public static extern void SetDigitalOut( int ADevice, int APort, int OnOff );

			[DllImport( ModuleName, EntryPoint = "casGetDigitalIn" )]
			public static extern int GetDigitalIn( int ADevice, int APort );

			#endregion

			#region >>> Public Static Method <<<

			public static int GetFilterName( int ADevice, int AFilter, out string msg )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				int ret = casGetFilterName( ADevice, AFilter, sb, sb.Capacity );

				msg = sb.ToString();

				return ret;
			}

			#endregion
		}

		/// <summary>
		/// Calibration class.
		/// </summary>
		public class Calibration
		{
			#region >>> DLL Import <<<

			// Calibration and Configuration Commands
			[DllImport( ModuleName, EntryPoint = "casCalculateCorrectedData" )]
			public static extern void CalculateCorrectedData( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casConvoluteTransmission" )]
			public static extern void ConvoluteTransmission( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casGetCalibrationFactors" )]
			public static extern double GetCalibrationFactors( int ADevice, int What, int Index, int Extra );

			[DllImport( ModuleName, EntryPoint = "casSetCalibrationFactors" )]
			public static extern void SetCalibrationFactors( int ADevice, int What, int Index, int Extra, double Value );

			[DllImport( ModuleName, EntryPoint = "casUpdateCalibrations" )]
			public static extern void UpdateCalibrations( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casSaveCalibration", CharSet = CharSet.Ansi, ExactSpelling = true )]
			public static extern void SaveCalibration( int ADevice, string AFileName );

			[DllImport( ModuleName, EntryPoint = "casClearCalibration" )]
			public static extern void ClearCalibration( int ADevice, int What );

			#endregion

			public const int gcfDensityFunction = 0;
			public const int gcfSensitivityFunction = 1;
			public const int gcfTransmissionFunction = 2;
			public const int gcfDensityFactor = 3;
			public const int gcfTOPApertureFactor = 4;
			public const int gcfTOPDistanceFactor = 5;

			public const int gcfTDCount = -1;
			public const int gcfTDExtraDistance = 1;
			public const int gcfTDExtraFactor = 2;
			
			public const int gcfWLCalibrationChannel = 6;
			public const int gcfWLCalibPointCount = -1;
			public const int gcfWLExtraCalibrationNone = 0;
			public const int gcfWLExtraCalibrationDelete = 1;
			public const int gcfWLExtraCalibrationDeleteAll = 2;
			public const int gcfWLCalibrationAlias = 7;
			public const int gcfWLCalibrationSave = 8;
			
			public const int gcfDarkArrayValues = 9;
			public const int gcfDarkArrayDepth = -1;  //Extra
			public const int gcfDarkArrayIntTime = -2;  //Extra
			
			public const int gcfTOPParameter = 11;
			public const int gcfTOPApertureSize = 0; //Extra
			public const int gcfTOPSpotSizeDenominator = 1;
			public const int gcfTOPSpotSizeOffset = 2;
			
			public const int gcfLinearityFunction = 12;
			public const int gcfLinearityCounts = 0;
			public const int gcfLinearityFactor = 1;
		}

		/// <summary>
		/// Measurement result.
		/// </summary>
		public class MeasurementResult
		{
			#region >>> DLL Import <<<
			
			// Measurement Results
			[DllImport( ModuleName, EntryPoint = "casGetData" )]
			public static extern double GetData( int ADevice, int AIndex );

			[DllImport( ModuleName, EntryPoint = "casGetXArray" )]
			public static extern double GetXArray( int ADevice, int AIndex );

			[DllImport( ModuleName, EntryPoint = "casGetDarkCurrent" )]
			public static extern double GetDarkCurrent( int ADevice, int AIndex );

			[DllImport( ModuleName, EntryPoint = "casGetPhotInt", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern void casGetPhotInt( int ADevice, out double APhotInt, StringBuilder AUnit, int AUnitLen );

			[DllImport( ModuleName, EntryPoint = "casGetRadInt", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern void casGetRadInt( int ADevice, out double ARadInt, StringBuilder AUnit, int AUnitLen );

			[DllImport( ModuleName, EntryPoint = "casGetCentroid" )]
			public static extern double GetCentroid( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casGetPeak" )]
			public static extern void GetPeak( int ADevice, out double x, out double y );

			[DllImport( ModuleName, EntryPoint = "casGetWidth" )]
			public static extern double GetWidth( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casGetWidthEx" )]
			public static extern double GetWidthEx( int ADevice, int What );

			[DllImport( ModuleName, EntryPoint = "casGetColorCoordinates" )]
			public static extern void GetColorCoordinates( int ADevice, ref double x, ref double y, ref double z, ref double u, ref double v1976, ref double v1960 );

			[DllImport( ModuleName, EntryPoint = "casGetCCT" )]
			public static extern double GetCCT( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casGetCRI" )]
			public static extern double GetCRI( int ADevice, int Index );

			[DllImport( ModuleName, EntryPoint = "casGetTriStimulus" )]
			public static extern void GetTriStimulus( int ADevice, ref double X, ref double Y, ref double Z );

			[DllImport( ModuleName, EntryPoint = "casGetExtendedColorValues" )]
			public static extern double GetExtendedColorValues( int ADevice, EExtendedColorValues What );

			#endregion

			#region >>> Public Static Method <<<

			public static void GetPhotInt( int ADevice, out double APhotInt, out string AUnit )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				casGetPhotInt( ADevice, out APhotInt, sb, sb.Capacity );

				AUnit = sb.ToString();
			}

			public static void GetRadInt( int ADevice, out double ARadInt, out string AUnit )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				casGetRadInt( ADevice, out ARadInt, sb, sb.Capacity );

				AUnit = sb.ToString();
			}

            public static void GetPeakWavelength( int ADevice,double startWave, double endWave, out double peakLambda, out double peakSpectrum)
            {
                peakLambda = 0;

                peakSpectrum = 0;

                //double start = CommonNumericalMethods.findNearestElementGreaterThan(this._wavelengthArray, startWave);

                //double end = CommonNumericalMethods.findNearestElementLessThan(this._wavelengthArray, endWave);

                CAS4Wrapper.Measurement.SetMeasurementParameter(ADevice, EMeasurementParameters.mpidColormetricStart, startWave);

                CAS4Wrapper.Measurement.SetMeasurementParameter(ADevice, EMeasurementParameters.mpidColormetricStop, endWave);

                CAS4Wrapper.ClormetricCalculation.ColorMetric(ADevice);

                CAS4Wrapper.MeasurementResult.GetPeak(ADevice, out peakLambda, out peakSpectrum);
            }



			#endregion
		}

		/// <summary>
		/// Clormetric calculation.
		/// </summary>
		public class ClormetricCalculation
		{
			#region >>> DLL Import <<<

			//Colormetric Calculation
			[DllImport( ModuleName, EntryPoint = "casColorMetric" )]
			public static extern int ColorMetric( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casCalculateCRI" )]
			public static extern int CalculateCRI( int ADevice );

			[DllImport( ModuleName, EntryPoint = "cmXYToDominantWavelength" )]
			public static extern int cmXYToDominantWavelength( double x, double y, double IllX, double IllY, ref double LambdaDom, ref double Purity );

			#endregion
		}

		/// <summary>
		/// Utilities.
		/// </summary>
		public class Utilities
		{
			#region >>> DLL Import <<<

			// Utilities
			[DllImport( ModuleName, EntryPoint = "casGetDLLFileName", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern int casGetDLLFileName( StringBuilder Dest, int ASize );

			[DllImport( ModuleName, EntryPoint = "casGetDLLVersionNumber", CharSet = CharSet.Ansi, ExactSpelling = true )]
			private static extern int casGetDLLVersionNumber( StringBuilder Dest, int ASize );

			[DllImport( ModuleName, EntryPoint = "casSaveSpectrum", CharSet = CharSet.Ansi, ExactSpelling = true )]
			public static extern int SaveSpectrum( int ADevice, string AFileName );

			[DllImport( ModuleName, EntryPoint = "casGetExternalADCValue" )]
			public static extern double GetExternalADCValue( int ADevice, int AIndex );

			[DllImport( ModuleName, EntryPoint = "casSetStatusLED" )]
			public static extern void SetStatusLED( int ADevice, int AWhat );

			[DllImport( ModuleName, EntryPoint = "casStopTime" )]
			public static extern int StopTime( int ADevice, int ARefTime );

			[DllImport( ModuleName, EntryPoint = "casNmToPixel" )]
			public static extern int NmToPixel( int ADevice, double nm );

			[DllImport( ModuleName, EntryPoint = "casPixelToNm" )]
			public static extern double PixelToNm( int ADevice, int APixel );

			[DllImport( ModuleName, EntryPoint = "casCalculateTOPParameter" )]
			public static extern int CalculateTOPParameter( int ADevice, int AAperture, double ADistance, ref double ASpotSize, ref double AFieldOfView );

			#endregion

			#region >>> Public Static Method <<<

			public static int GetDLLFileName( out string text )
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				int ret = casGetDLLFileName( sb, sb.Capacity );

				text = sb.ToString();

				return ret;
			}

			public static int GetDLLVersionNumber(out string text)
			{
				StringBuilder sb = new StringBuilder( MessageStringLength );

				int ret = casGetDLLVersionNumber( sb, sb.Capacity );

				text = sb.ToString();
			
				return ret;
			}

			#endregion
		}

		/// <summary>
		/// Multiple track.
		/// </summary>
		public class MultiTrack
		{
			#region >>> DLL Import <<<
			
			//MultiTrack
			[DllImport( ModuleName, EntryPoint = "casMultiTrackInit" )]
			public static extern int MultiTrackInit( int ADevice, int ATracks );

			[DllImport( ModuleName, EntryPoint = "casMultiTrackDone" )]
			public static extern int MultiTrackDone( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casMultiTrackCount" )]
			public static extern int MultiTrackCount( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casMultiTrackCopySet" )]
			public static extern void MultiTrackCopySet( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casMultiTrackReadData" )]
			public static extern int MultiTrackReadData( int ADevice, int ATrack );

			[DllImport( ModuleName, EntryPoint = "casMultiTrackCopyData" )]
			public static extern int MultiTrackCopyData( int ADevice, int ATrack );

			[DllImport( ModuleName, EntryPoint = "casMultiTrackSaveData", CharSet = CharSet.Ansi, ExactSpelling = true )]
			public static extern int MultiTrackSaveData( int ADevice, string AFileName );

			[DllImport( ModuleName, EntryPoint = "casMultiTrackLoadData", CharSet = CharSet.Ansi, ExactSpelling = true )]
			public static extern int MultiTrackLoadData( int ADevice, string AFileName );
		
			#endregion
		}

		/// <summary>
		/// Spectrum manipulation.
		/// </summary>
		public class SpectrumManipulation
		{
			#region >>> DLL Import <<<
			
			//Spectrum Manipulation
			[DllImport( ModuleName, EntryPoint = "casSetData" )]
			public static extern void SetData( int ADevice, int AIndex, double Value );

			[DllImport( ModuleName, EntryPoint = "casSetXArray" )]
			public static extern void SetXArray( int ADevice, int AIndex, double Value );

			[DllImport( ModuleName, EntryPoint = "casSetDarkCurrent" )]
			public static extern void SetDarkCurrent( int ADevice, int AIndex, double Value );

			[DllImport( ModuleName, EntryPoint = "casGetDataPtr" )]
			public static extern IntPtr GetDataPtr( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casGetXPtr" )]
			public static extern IntPtr GetXPtr( int ADevice );

			[DllImport( ModuleName, EntryPoint = "casLoadTestData", CharSet = CharSet.Ansi, ExactSpelling = true )]
			public static extern void LoadTestData( int ADevice, string AFileName );

			#endregion
		}
	}
}
