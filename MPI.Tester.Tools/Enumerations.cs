using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Tools
{
    public enum EDailyWacthState : int
    {
        PASS = 0,
        OverDailyWacth = 1,
        ReCalibration = 2,
        FAIL = -1,
    }

    public enum EDailyCheckState : int
    {
        PASS = 0,
        AutoCalibration = 1,
        FAIL = -1,
    }

    public enum EDailyCheckSpecBy : int
    {
        RECIPE =0,
        TestFileName =1,
    }

    public enum EDailyCheckResult
    {
        PASS = 0,
        AvgBiasOutSpec = 1,
        BoundayOutSpec = 2,
        LessThanMinAcceptDies = 3,
        NoCompareData = 4,
    }
}
