using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{

    public interface IAttenuator
    {
        string SerialNumber { get;}
        string SoftwareVersion { get; }
        string HardwareVersion { get;}
        string Address { get; }
        int Slot { get; }
        EDevErrorNumber ErrorNumber { get;}
        IConnect Connect { get; }

        AttenuatorSpec Spec { get;}
        AttenuatorSettingData AttSetting { get;}//目前假設不會在同一Recipe中切換衰減器設定

        Dictionary<int, int> SysDevChDic { set; get; }

        bool Init( string sourceMeterSN,IConnect connect);
        void TurnOff();
        //void TurnOff(uint index);
        void Close();
        bool SetParamToAttenuator(List<AttenuatorSettingData> paramSetting);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysCh"></param>
        /// <returns></returns>
        double  GetMsrtPower(int sysCh = 1,ELaserPowerUnit unit = ELaserPowerUnit.W);

        MinMaxValuePair<double> GetOutputPowerRangeIndBm(int ch);
    }
}
