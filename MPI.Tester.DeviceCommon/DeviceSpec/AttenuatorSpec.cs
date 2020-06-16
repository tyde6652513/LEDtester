using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class AttenuatorSpec:ICloneable
    {
        #region >>public property<<       
        /// <summary>
        /// dBm
        /// </summary>
        public MinMaxValuePair<double> PowerRange  { get; set; }//dBm
        /// <summary>
        /// dB
        /// </summary>
        public MinMaxValuePair<double> AttenuationRange { set; get; }//dB
        /// <summary>
        /// nm
        /// </summary>
        public MinMaxValuePair<double> WavelengthRange { set; get; }//nm
        /// <summary>
        /// dB/sec
        /// </summary>
        public MinMaxValuePair<double> TransitionSpeed { set; get; }//dB/sec
        /// <summary>
        /// Sec
        /// </summary>
        public MinMaxValuePair<double> AveragingTime { set; get; }//S

        public double MaxInputPower { set; get; }//dBm
        public double SettlingTime  { set; get; }//Sec
        public MinMaxValuePair<int> Channel { get; set; }
        public bool HavePowerControlMode { get; set; }

        #endregion
        public AttenuatorSpec()
        {
            PowerRange = new MinMaxValuePair<double>(-50, 20);
            AttenuationRange = new MinMaxValuePair<double>(0, 40);

            WavelengthRange = new MinMaxValuePair<double>(100, 2000);
            TransitionSpeed = new MinMaxValuePair<double>(0.1, 1000);
            AveragingTime = new MinMaxValuePair<double>(0.002, 1);
            Channel = new MinMaxValuePair<int>(1, 1);
            MaxInputPower = 18;
            SettlingTime = 0.1;
            HavePowerControlMode = false;
        }


        #region >>public mehtod<<

        public object Clone()
        {
            AttenuatorSpec obj = this.MemberwiseClone() as AttenuatorSpec;
            return obj;
        }
        #endregion
        #region

        #endregion
    }

    public static class dBm2WConverter
    {
        public static MinMaxValuePair<double> GetPowerInW(MinMaxValuePair<double> valPair)
        {
            double Min = dBm2W(valPair.Min);
            double Max = dBm2W(valPair.Max);
            MinMaxValuePair<double> wRange = new MinMaxValuePair<double>(Min, Max);
            return wRange;
        }

        public static MinMaxValuePair<double> SetPowerRangeInW(this MinMaxValuePair<double> valPair)
        {
            double Min = W2dBm(valPair.Min);
            double Max = W2dBm(valPair.Max);

            MinMaxValuePair<double> WValuePair = new MinMaxValuePair<double>(Min, Max);

            return WValuePair;
        }

        public static MinMaxValuePair<double> SetPowerRangeInW(ref  MinMaxValuePair<double> valPair)
        {
            double Min = W2dBm(valPair.Min);
            double Max = W2dBm(valPair.Max);

            valPair = new MinMaxValuePair<double>(Min, Max);

            return valPair;
        }

        public static double dBm2W(double dBm)
        {
            double wVal = 0.001 * Math.Pow(10, dBm/10);
            return wVal;
        }

        public static double W2dBm(double W)
        {
            double dBmVal = 10 * Math.Log10(W / 0.001);
            return dBmVal;
        }
    }
}
