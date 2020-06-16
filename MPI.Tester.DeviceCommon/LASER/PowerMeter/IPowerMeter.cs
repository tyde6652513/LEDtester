using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public interface IPowerMeter
    {
        string SerialNumber { get; }
        string SoftwareVersion { get; }
        string HardwareVersion { get; }
        string Address { get; }
        int Slot { get; }
        EDevErrorNumber ErrorNumber { get; }
        IConnect Connect { get; }

        PowerMeterSpec Spec { get; }

        Dictionary<int, int> SysChDevDic { set; get; }//跟別人不一樣，會出現多個sys channel 接同一個Dev channel

        bool Init( IConnect connect);
        void TurnOff();
        void Close();
        /// <summary>
        /// 要一起檢查輸入的LaserSyschannel以及DeviceChannel是否都在規格內
        /// </summary>
        /// <param name="sysCh"></param>
        /// <param name="devCh"></param>
        /// <returns></returns>
        bool Push(int sysCh, int devCh);
        double GetMsrtPower(PowerMeterSettingData pData, int Polarity);

    }
}
