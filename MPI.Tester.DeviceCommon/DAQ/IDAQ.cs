using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public interface IDAQ
    {
        string SerialNumber { get; }
        string SoftwareVersion { get; }
        string HardwareVersion { get; }

        bool Init(DAQSettingData data, uint channelCount);
        bool SetParamToDAQ(ElectSettingData[] data);
        bool SetTrigger(uint settingIndex);
		double[] GetDataFromDAQ(uint channel, uint settingIndex);
		void Close();
    }
}
