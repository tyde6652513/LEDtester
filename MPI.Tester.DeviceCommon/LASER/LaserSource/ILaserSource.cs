using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public interface ILaserSource
    {
        string SerialNumber { get; }
        string SoftwareVersion { get; }
        string HardwareVersion { get; }

        EDevErrorNumber ErrorNumber { get; }

        //LCRSettingData[] LCRSetting { get; }
        //LCRMeterSpec Spec { get; }
        //LCRCaliData CaliData { get; }


        bool Init(int deviceNum, string sourceMeterSN);
        void TurnOff();
        void TurnOff(uint index);
        void Close();

    }
}
