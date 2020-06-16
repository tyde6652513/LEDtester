using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.Tester.GuiComponent.Unit
{
    public static class UnitMath
    {
        public static double ToSIUnit(string unitName)
        {
            double scale = 0.0d;
            if (Enum.IsDefined(typeof(EVoltUnit), unitName))
            {
                EVoltUnit tmp = (EVoltUnit)Enum.Parse(typeof(EVoltUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp - EVoltUnit.V);
            }
            else if (Enum.IsDefined(typeof(EAmpUnit), unitName))
            {
                EAmpUnit tmp = (EAmpUnit)Enum.Parse(typeof(EAmpUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp - EAmpUnit.A);
            }
            else if (Enum.IsDefined(typeof(EWattUnit), unitName))
            {
                EWattUnit tmp = (EWattUnit)Enum.Parse(typeof(EWattUnit), unitName, false);

                scale = Math.Pow(10.0d, tmp - EWattUnit.W);
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

    }


    public enum EVoltUnit : int
    {
        [XmlEnum(Name = "kV")]
        kV = 3,

        [XmlEnum(Name = "V")]
        V = 0,

        [XmlEnum(Name = "mV")]
        mV = -3,

        [XmlEnum(Name = "uV")]
        uV = -6,

        [XmlEnum(Name = "nV")]
        nV = -9,

        [XmlEnum(Name = "pV")]
        pV = -12,
    }

    public enum EAmpUnit : int
    {
        [XmlEnum(Name = "kA")]
        kA = 3,

        [XmlEnum(Name = "A")]
        A = 0,

        [XmlEnum(Name = "mA")]
        mA = -3,

        [XmlEnum(Name = "uA")]
        uA = -6,

        [XmlEnum(Name = "nA")]
        nA = -9,

        [XmlEnum(Name = "pA")]
        pA = -12,
    }

    public enum EWattUnit : int
    {
        [XmlEnum(Name = "W")]
        W = 0,

        [XmlEnum(Name = "mW")]
        mW = -3,

        [XmlEnum(Name = "uW")]
        uW = -6,

        [XmlEnum(Name = "nW")]
        nW = -9,

        [XmlEnum(Name = "pW")]
        pW = -12,
    }

    public enum ETimeUnit : int
    {
        [XmlEnum(Name = "s")]
        s = 0,

        [XmlEnum(Name = "ms")]
        ms = -3,

        [XmlEnum(Name = "us")]
        us = -6,
    }

    public enum ECapUnit : int
    {
        [XmlEnum(Name = "kF")]
        kF = 3,

        [XmlEnum(Name = "F")]
        F = 0,

        [XmlEnum(Name = "mF")]
        mF = -3,

        [XmlEnum(Name = "uF")]
        uF = -6,

        [XmlEnum(Name = "nF")]
        nF = -9,

        [XmlEnum(Name = "pF")]
        pF = -12,

        [XmlEnum(Name = "fF")]
        fF = -15,
    }

    public enum EIndUnit : int
    {
        [XmlEnum(Name = "kH")]
        kH = 3,

        [XmlEnum(Name = "H")]
        H = 0,

        [XmlEnum(Name = "mH")]
        mH = -3,

        [XmlEnum(Name = "uH")]
        uH = -6,

        [XmlEnum(Name = "nH")]
        nH = -9,

        [XmlEnum(Name = "pH")]
        pH = -12,
    }

    public enum EOhmUnit : int
    {
        [XmlEnum(Name = "GOhm")]
        GOhm = 6,

        [XmlEnum(Name = "MOhm")]
        MOhm = 6,

        [XmlEnum(Name = "kOhm")]
        kOhm = 3,

        [XmlEnum(Name = "Ohm")]
        Ohm = 0,

        [XmlEnum(Name = "mOhm")]
        mOhm = -3,

        [XmlEnum(Name = "uOhm")]
        uOhm = -6,

        [XmlEnum(Name = "nOhm")]
        nOhm = -9,

        [XmlEnum(Name = "pOhm")]
        pOhm = -12,
    }

    public enum ESieUnit : int
    {
        [XmlEnum(Name = "GS")]
        GS = 6,

        [XmlEnum(Name = "MS")]
        MS = 6,

        [XmlEnum(Name = "kS")]
        kS = 3,

        [XmlEnum(Name = "S")]
        S = 0,

        [XmlEnum(Name = "mS")]
        mS = -3,

        [XmlEnum(Name = "uS")]
        uS = -6,

        [XmlEnum(Name = "nS")]
        nS = -9,

        [XmlEnum(Name = "pS")]
        pS = -12,
    }

    public enum EFreqUnit : int
    {
        [XmlEnum(Name = "Hz")]
        Hz = 0,

        [XmlEnum(Name = "KHz")]
        KHz = 3,

        [XmlEnum(Name = "MHz")]
        MHz = 6,
    }
}
