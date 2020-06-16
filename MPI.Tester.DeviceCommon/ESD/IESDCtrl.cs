using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
	public interface IESDDevice
	{
        string SerialNumber { get; }
		string SoftwareVersion { get; }
		string HardwareVersion { get; }
        EDevErrorNumber ErrorNumber { get; }
		bool IsWorkingBusy { get; }
        int SetLength { get; }
        ESDHardwareInfo HardwareInfo { get; }

		bool Open();
		void Close();
        bool SetConfigToMeter(ESDDevSetting devSetting);
        bool SetParamToMeter(ESDSettingData[] paramSetting);
		bool Zap(uint[] activeCH, int index);
        bool PreCharge(int index);
        bool ResetToSafeStatus();
	}
}
