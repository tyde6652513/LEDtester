using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public interface ISwitch
    {
        string SerialNumber { get; }
        string SoftwareVersion { get; }
        string HardwareVersion { get; }
        EDevErrorNumber ErrorNumber { get; }
        int MaxSwitchingChannelCount { get; }

        bool Init(string switchSystemSN);
        void Reset();
        bool EnableCH(uint index);
        bool DisableCH();
        void Close();
    }
}
