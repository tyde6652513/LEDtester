using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    
    public interface ISourceMeter
    {
        string SerialNumber { get; }
		string SoftwareVersion { get; }
		string HardwareVersion { get; }
        EDevErrorNumber ErrorNumber { get; }
        ElectSettingData[] ElecSetting { get; }
        SourceMeterSpec Spec { get;}

		bool Init(int deviceNum, string sourceMeterSN);
        void Close();
        void Reset();

        bool SetConfigToMeter(ElecDevSetting devSetting);
        bool SetParamToMeter(ElectSettingData[] paramSetting);

        bool MeterOutput(uint[] activateChannels, uint settingIndex);			// [ Setting Item Index ]
        bool MeterOutput(uint[] activateChannels, uint settingIndex, double applyValue);			// [ Setting Item Index ]
        double[] GetDataFromMeter(uint channel, uint settingIndex);				// [ Setting Item Index ]
        double[] GetApplyDataFromMeter(uint channel, uint settingIndex);		// [ Setting Item Index ]
		double[] GetSweepPointFromMeter(uint channel, uint settingIndex);		// [ Setting Item Index ]
	    double[] GetSweepResultFromMeter(uint channel, uint itemIndex);			// [ Setting Item Index ]
		double[] GetTimeChainFromMeter(uint channel, uint settingIndex);	    // [ Setting Item Index ]

        //double[] MeterOutput(ElectSettingData set);
		void TurnOff();
        void TurnOff(double delay, bool isOpenRelay);

		void Output(uint point, bool active);
		byte InputB(uint point);
		byte Input(uint point);

        double GetPDDarkSample(int count);

        bool CheckInterLock();
    }

}
