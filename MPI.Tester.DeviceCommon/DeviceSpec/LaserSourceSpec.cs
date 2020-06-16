using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class LaserSourceSpec
    {
        public AttenuatorSpec AttSpec { set; get; }
        public List<OpticalSwitchSpec> OpticalSwitchSpec { set; get; }
        public PowerMeterSpec PowerMeterSpec { set; get; }

        public LaserSourceSpec()
        {
            AttSpec = null;
            OpticalSwitchSpec = new List<OpticalSwitchSpec>();
            PowerMeterSpec = null;
        }
    }



}
