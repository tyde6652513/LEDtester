using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public enum EDmmMeasureFunc
    {
        DC_VOLTAGE,
        DC_CURRENT,
        DIGITIZE_CURRENT,
        DIGITIZE_VOLTAGE,
    }

    public enum EDmmDcIntegrationUnit
    {
        NPLC,
        Aperture,
    }

    public enum EDmmDioTriggerOut : int
    {
        NONE = 0,
        PIN1_FFP = 1,
        PIN2_NFP = 2,
        PIN3_SPT = 3,
    }

}
