using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SpectroMeter
{
    public class SeaBreezeDLL_USB2000P : ISeaBreeze
    {
        private const string DLL_PATH = @"C:\MPI\LEDTester\Driver\SeaBreezeWin32.dll";

        public SeaBreezeDLL_USB2000P()
        {
 
        }

        #region >>> DLL Declaration (SeaBreezeWin32) <<<

        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern int seabreeze_open_spectrometer(int spectrometerIndex, int errorMsrCode);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern int seabreeze_close_spectrometer(int spectrometerIndex, int errorMsrCode);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern void seabreeze_set_trigger_mode(int spectrometerIndex, int errorMsrCode, int mode);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern string seabreeze_get_error_string(int spectrometerIndex, int errorMsrCode, int length);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern void seabreeze_set_integration_time(int spectrometerIndex, int errorMsrCode, int microseconds);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern int seabreeze_get_formatted_spectrum(int spectrometerIndex, int errorMsrCode, double[] array, int length);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern int seabreeze_get_formatted_spectrum_length(int spectrometerIndex, int errorMsrCode);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern int seabreeze_get_wavelengths(int spectrometerIndex, int errorMsrCode, double[] array, int length);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern string seabreeze_get_serial_number(int spectrometerIndex, int errorMsrCode);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern int seabreeze_read_eeprom_slot(int spectrometerIndex, int errorMsrCode, int slotNumber, byte[] buffer, int length);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern int seabreeze_get_unformatted_spectrum_length(int spectrometerIndex, int errorMsrCode);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        private static extern int seabreeze_get_unformatted_spectrum(int spectrometerIndex, int errorMsrCode, byte[] array, int length);


        #endregion

        #region >>> Public Property <<<

        public string SeaBreezeImportDllPath
        {
            get { return DLL_PATH; }
        }

        #endregion

        #region >>> Public Method <<<

        public int Open(int spectrometerIndex, int errorMsrCode)
        {
            return seabreeze_open_spectrometer(spectrometerIndex, errorMsrCode);
        }

        public int Close(int spectrometerIndex, int errorMsrCode)
        {
            return seabreeze_close_spectrometer(spectrometerIndex, errorMsrCode);
        }

        public int ReadEepromSlot(int spectrometerIndex, int errorMsrCode, int slotNumber, byte[] buffer, int length)
        {
            return seabreeze_read_eeprom_slot(spectrometerIndex, errorMsrCode, slotNumber, buffer, length);
        }

        public void SetTriggerMode(int spectrometerIndex, int errorMsrCode, int mode)
        {
            seabreeze_set_trigger_mode(spectrometerIndex, errorMsrCode, mode);
        }

        public void SetIntegrationTime(int spectrometerIndex, int errorMsrCode, int microseconds)
        {
            seabreeze_set_integration_time(spectrometerIndex, errorMsrCode, microseconds);
        }

        public string GetSerialNumber(int spectrometerIndex, int errorMsrCode)
        {
            return seabreeze_get_serial_number(spectrometerIndex, errorMsrCode);
        }

        public string GetError(int spectrometerIndex, int errorMsrCode, int length)
        {
            return seabreeze_get_error_string(spectrometerIndex, errorMsrCode, length);
        }

        public int GetWavelengths(int spectrometerIndex, int errorMsrCode, double[] array, int length)
        {
            return seabreeze_get_wavelengths(spectrometerIndex, errorMsrCode, array, length);
        }

        public int GetFormattedSpectrum(int spectrometerIndex, int errorMsrCode, double[] array, int length)
        {
            return seabreeze_get_formatted_spectrum(spectrometerIndex, errorMsrCode, array, length);
        }

        public int GetFormattedSpectrumLength(int spectrometerIndex, int errorMsrCode)
        {
            return seabreeze_get_formatted_spectrum_length(spectrometerIndex, errorMsrCode);
        }

        public int GetUnformattedSpectrum(int spectrometerIndex, int errorMsrCode, byte[] array, int length)
        {
            return seabreeze_get_unformatted_spectrum(spectrometerIndex, errorMsrCode, array, length);
        }

        public int GetUnformattedSpectrumLength(int spectrometerIndex, int errorMsrCode)
        {
            return seabreeze_get_unformatted_spectrum_length(spectrometerIndex, errorMsrCode);
        }

        #endregion
    }
}
