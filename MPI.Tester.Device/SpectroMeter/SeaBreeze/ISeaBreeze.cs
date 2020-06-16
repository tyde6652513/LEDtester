using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.Device.SpectroMeter
{
    interface ISeaBreeze
    {
        string SeaBreezeImportDllPath { get; }
        
        int Open(int spectrometerIndex, int errorMsrCode);
        int Close(int spectrometerIndex, int errorMsrCode);
      
        int ReadEepromSlot(int spectrometerIndex, int errorMsrCode, int slotNumber, byte[] buffer, int length);

        void SetTriggerMode(int spectrometerIndex, int errorMsrCode, int mode);
        void SetIntegrationTime(int spectrometerIndex, int errorMsrCode, int microseconds);

        string GetSerialNumber(int spectrometerIndex, int errorMsrCode);
        string GetError(int spectrometerIndex, int errorMsrCode, int length);

        int GetWavelengths(int spectrometerIndex, int errorMsrCode, double[] array, int length);

        int GetFormattedSpectrum(int spectrometerIndex, int errorMsrCode, double[] array, int length);
        int GetFormattedSpectrumLength(int spectrometerIndex, int errorMsrCode);

        int GetUnformattedSpectrum(int spectrometerIndex, int errorMsrCode, byte[] array, int length);
        int GetUnformattedSpectrumLength(int spectrometerIndex, int errorMsrCode);
    }
}
