using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    public interface ILaserPostCalc
    {
        double CalibratedPowerFactor { get; set; }
        LaserCalcSetting SettingData { get; set; }
        LaserCharacteristicsData CharacteristicResults { get; set; }
        LaserCurveData Curve { get; }

        bool CalcParameter(double[] powRawArray, double[] currRawArray, double[] voltRawArray);
    }
}
