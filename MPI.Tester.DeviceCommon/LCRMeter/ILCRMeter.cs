using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using MPI.Tester.Data;

namespace MPI.Tester.DeviceCommon
{
	public interface ILCRMeter
	{
		string SerialNumber { get; }
		string SoftwareVersion { get; }
		string HardwareVersion { get; }
		EDevErrorNumber ErrorNumber { get; }
		LCRSettingData[] LCRSetting { get; }
		LCRMeterSpec Spec { get; }
        LCRCaliData CaliData { get; }

		bool Init(int deviceNum, string sourceMeterSN);
		void TurnOff();
        void TurnOff(uint index);
		void Close();

		bool SetConfigToMeter(LCRDevSetting devSetting);
		bool SetParamToMeter(LCRSettingData[] paramSetting);
        bool PreSettingParamToMeter(uint settingIndex);
        bool PreSetBiasListToMeter(uint settingIndex);

        bool SetCaliData(LCRCaliData caliData);
        bool LCRCali(ELCRCaliMode caliMode);

		bool MeterOutput(uint[] activateChannels, uint settingIndex);
		double[] GetDataFromMeter(uint channel, uint settingIndex);
        List<List<float>> GetDataFromMeter(uint settingIndex);

        void GetRawDataOfMeterCali();
        //void GetDataFromMeter(LIVData livData, uint settingIndex);
	}
}
