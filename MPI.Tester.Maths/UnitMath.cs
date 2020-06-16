using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Maths
{
    public static class  UnitMath
    {
        public static double ToSIUnit(string unitName)
        {
            double scale = 0.0d;
            if (Enum.IsDefined(typeof(EVoltUnit), unitName))
            {
                EVoltUnit tmp = (EVoltUnit)Enum.Parse(typeof(EVoltUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp -  EVoltUnit.V);
            }
            else if (Enum.IsDefined(typeof(EAmpUnit), unitName))
            {
                EAmpUnit tmp = (EAmpUnit)Enum.Parse(typeof(EAmpUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp - EAmpUnit.A);
            }
            else if (Enum.IsDefined(typeof(EWattUnit), unitName))
            {
                EWattUnit tmp = (EWattUnit)Enum.Parse(typeof(EWattUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp -  EWattUnit.W);
            }
            else if (Enum.IsDefined(typeof(ECapUnit), unitName))
            {
                ECapUnit tmp = (ECapUnit)Enum.Parse(typeof(ECapUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp - ECapUnit.F);
            }
            else if (Enum.IsDefined(typeof(EIndUnit), unitName))
            {
                EIndUnit tmp = (EIndUnit)Enum.Parse(typeof(EIndUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp - EIndUnit.H);
            }
            else if (Enum.IsDefined(typeof(ESieUnit), unitName))
            {
                ESieUnit tmp = (ESieUnit)Enum.Parse(typeof(ESieUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp - ESieUnit.S);
            }
            return scale;
        }


        public static double UnitConvertFactor(EVoltUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EVoltUnit), toUnit))
            {
                EVoltUnit tmp = (EVoltUnit)Enum.Parse(typeof(EVoltUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public static double UnitConvertFactor(EAmpUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EAmpUnit), toUnit))
            {
                EAmpUnit tmp = (EAmpUnit)Enum.Parse(typeof(EAmpUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public static double UnitConvertFactor(EWattUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EWattUnit), toUnit))
            {
                EWattUnit tmp = (EWattUnit)Enum.Parse(typeof(EWattUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public static double UnitConvertFactor(ECapUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(ECapUnit), toUnit))
            {
                ECapUnit tmp = (ECapUnit)Enum.Parse(typeof(ECapUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public static double UnitConvertFactor(EIndUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EIndUnit), toUnit))
            {
                EIndUnit tmp = (EIndUnit)Enum.Parse(typeof(EIndUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public static double UnitConvertFactor(EOhmUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EOhmUnit), toUnit))
            {
                EOhmUnit tmp = (EOhmUnit)Enum.Parse(typeof(EOhmUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public static double UnitConvertFactor(ESieUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(ESieUnit), toUnit))
            {
                ESieUnit tmp = (ESieUnit)Enum.Parse(typeof(ESieUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public static double UnitConvertFactor(ETimeUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(ETimeUnit), toUnit))
            {
                ETimeUnit tmp = (ETimeUnit)Enum.Parse(typeof(ETimeUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public static List<string> GetUnitList(string unitName)
        {
            List<string> unitList = new List<string>();
            unitList.Clear();

            if (Enum.IsDefined(typeof(EVoltUnit), unitName))
            {
                unitList.Add(EVoltUnit.V.ToString());
                unitList.Add(EVoltUnit.mV.ToString());
            }
            else if (Enum.IsDefined(typeof(EAmpUnit), unitName))
            {
                unitList.Add(EAmpUnit.A.ToString());
                unitList.Add(EAmpUnit.mA.ToString());
                unitList.Add(EAmpUnit.uA.ToString());
                unitList.Add(EAmpUnit.nA.ToString());
            }
            else if (Enum.IsDefined(typeof(EWattUnit), unitName))
            {
                unitList.Add(EWattUnit.W.ToString());
                unitList.Add(EWattUnit.mW.ToString());
                unitList.Add(EWattUnit.uW.ToString());
                unitList.Add(EWattUnit.nW.ToString());
                unitList.Add(EWattUnit.pW.ToString());
            }
            else if (Enum.IsDefined(typeof(ETimeUnit), unitName))
            {
                unitList.Add(ETimeUnit.s.ToString());
                unitList.Add(ETimeUnit.ms.ToString());
                unitList.Add(ETimeUnit.us.ToString());
            }
            else if (Enum.IsDefined(typeof(EOhmUnit), unitName))
            {
                unitList.Add(EOhmUnit.MOhm.ToString());
                unitList.Add(EOhmUnit.kOhm.ToString());
                unitList.Add(EOhmUnit.Ohm.ToString());
                unitList.Add(EOhmUnit.mOhm.ToString());
                unitList.Add(EOhmUnit.uOhm.ToString());
            }
            else if (Enum.IsDefined(typeof(ECapUnit), unitName))
            {
                unitList.Add(ECapUnit.F.ToString());
                unitList.Add(ECapUnit.mF.ToString());
                unitList.Add(ECapUnit.uF.ToString());
                unitList.Add(ECapUnit.nF.ToString());
                unitList.Add(ECapUnit.pF.ToString());
                unitList.Add(ECapUnit.fF.ToString());
            }
            else if (Enum.IsDefined(typeof(ESieUnit), unitName))
            {
                unitList.Add(ESieUnit.S.ToString());
                unitList.Add(ESieUnit.mS.ToString());
                unitList.Add(ESieUnit.uS.ToString());
                unitList.Add(ESieUnit.nS.ToString());
                unitList.Add(ESieUnit.pS.ToString());
                
            }
            else
            {
                unitList.Add(unitName);
            }
            return unitList;
        }

        public static double dBm2W(double dBm)
        {
            //double wVal = 0.001 * Math.Pow(10, dBm/10);
            double wVal = dB2Decimal(dBm) * 0.001;
            return wVal;
        }

        public static double W2dBm(double W)
        {
            //double dBmVal = 10*Math.Log10(W / 0.001);
            double dBmVal = Decimal2dB(W) +30;
            return dBmVal;
        }

        public static double dB2Decimal(double dBVal)
        {
            double dVal =  Math.Pow(10, dBVal / 10);
            return dVal;
        }
        public static double Decimal2dB(double dVal)
        {
            double dBVal = 10 * Math.Log10(dVal);
            return dBVal;
        }
    }
}
