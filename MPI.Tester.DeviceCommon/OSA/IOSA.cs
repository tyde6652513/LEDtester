using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public interface IOSA
    {
        string SerialNumber { get; }

        OsaData[] Data { get; }
        EDevErrorNumber ErrorNumber { get; }

        bool Init(string ipAddress);
        bool SetConfigToMeter(OsaDevSetting cfg);
        bool SetParaToMeter(OsaSettingData[] parameter);

        bool Trigger(uint index);
        bool CalculateMeasureResultData(uint index);
        void Close();
        void Reset();
    }
}
