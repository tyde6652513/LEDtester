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
    public class AutoTuneVOASettingData
    {
        public double TuneVOATolerence { get; set; }//%
        public double TuneVOATriggerLimit { get; set; }//%
        public AutoTuneVOASettingData()
        {
            TuneVOATolerence = 5;
            TuneVOATriggerLimit = 4;
        }

        #region
        public object Clone()
        {
            AutoTuneVOASettingData obj = this.MemberwiseClone() as AutoTuneVOASettingData;
            return obj;
        }
        #endregion
    }
}
