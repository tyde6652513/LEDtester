using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;

using MPI.Tester.DeviceCommon;


namespace MPI.Tester.Data.LaserData.LaserSource
{
    [Serializable]
    public class PowerMeterConfig:ICloneable
    {
        public PowerMeterConfig()
        {
            PowerMeterModel = EPowerMeter.NONE;

            LaserSysChannel = 0;
            Enable = false;
            Address = "";
            Slot = 0;
            PowerMeterChannel = 0;
    }
        #region >>public property<<
        [DisplayName("Model")]
        public EPowerMeter PowerMeterModel { set; get; }

        [ReadOnly(true)]
        public int LaserSysChannel { set; get; }
        public bool Enable { set; get; }
        [DisplayName("Address")]
        public string Address { set; get; }
        [Description("For K2600 ,select 1 as channel A,2 as channel B")]
        public int Slot { set; get; }
        [DisplayName("Channel")]
        public int PowerMeterChannel { set; get; }
        #endregion


        #region >>public method<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
