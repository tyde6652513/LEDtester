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
    public  class OpticalSwitchSettingData : ICloneable
    {
        public OpticalSwitchSettingData()
        {
            SysChannel = 1;

        }

        #region >>publoic property<<

        public int SysChannel;

        #endregion

        #region >>public method<<


        public object Clone()
        {
            OpticalSwitchSettingData obj = this.MemberwiseClone() as OpticalSwitchSettingData;

            return obj;
        }
        #endregion
    }
}
