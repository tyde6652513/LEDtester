using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class PowerMeterSettingData : ICloneable
    {
        public PowerMeterSettingData()
        {
            PDGain = 1;
            SysGain = 1;
            TarPower = 1E-6;
            CheckInAutoProcess = false;

            ForceValue = 0;
            ForceUnit = "V";
            ForceTime = 1;
            TimeUnit = "ms";
            Clamp = 10;
            MsrtUnit = "uA";
            WaveLength = 1300;

            Tolerence = 20;
            RecordPower = false;
            TuneVOALimit = 5;//%
            TuneVOAtolerence = 4;//%
        }
        #region >>public property<<
        public int SysChannel;
        public bool CheckInAutoProcess;
        public bool RecordPower;//驗證光機強度用
        public double PDGain;//W/A
        public double SysGain;//對校用
        public double TarPower;//W
        public double WaveLength;//nm
        public double Tolerence;//±%

        public double ForceValue =0;//V
        public string ForceUnit = "V";
        public double ForceTime = 1;//s
        public string TimeUnit = "ms";
        public double Clamp = 0.001;//A
        public string MsrtUnit = "mA";

        public double TuneVOALimit = 5;//%
        public double TuneVOAtolerence = 4;//%

        #endregion

        #region >>public method<<

        public object Clone()
        {
            PowerMeterSettingData obj = this.MemberwiseClone() as PowerMeterSettingData;

            return obj;
        }
        #endregion
    }
}
