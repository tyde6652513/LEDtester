
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class OpticalSwitchSpec:ICloneable
    {
        #region >>public property<<       
        public MinMaxValuePair<int> InputCh  { get; set; }
        public MinMaxValuePair<int> OutputCh { set; get; }

        #endregion
        public OpticalSwitchSpec()
        {
            InputCh = new MinMaxValuePair<int>(1, 1);
            OutputCh = new MinMaxValuePair<int>(1, 1);
        }

        #region >>public mehtod<<

        public object Clone()
        {
            OpticalSwitchSpec obj = this.MemberwiseClone() as OpticalSwitchSpec;
            return obj;
        }
        #endregion

        #region
        
        #endregion
    }

}
