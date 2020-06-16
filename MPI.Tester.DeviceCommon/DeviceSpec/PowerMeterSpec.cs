using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public class PowerMeterSpec
    {
        #region >>public property<<       
        public MinMaxValuePair<double> PowerRange { get; set; }//dBm
        public MinMaxValuePair<double> WavelengthRange { set; get; }//nm
        public MinMaxValuePair<int> Channel { get; set; }

        public SourceMeterSpec SMUSpec { get; set; }
        #endregion
        public PowerMeterSpec()
        {
            PowerRange = new MinMaxValuePair<double>(-90, 10);
            WavelengthRange = new MinMaxValuePair<double>(100, 2000);
            Channel = new MinMaxValuePair<int>(1, 1);
            SMUSpec = null;
        }


        #region >>public mehtod<<

        public object Clone()
        {
            PowerMeterSpec obj = this.MemberwiseClone() as PowerMeterSpec;
            return obj;
        }
        #endregion
        #region

        #endregion
    }
}
