using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.GuiComponent.Unit;

namespace MPI.Tester.GuiComponent
{
    public class UnitAmpData:ICloneable
    {
        public UnitAmpData()
        {
            
        }

        public EAmpUnit UnitEA { get; set; }

        public double Value { get; set; }

        public double Maximum { get; set; }

        public double Minimum { get; set; }

        public double Increment { get; set; }

        public string DisplayFormat { get; set; }

        public object Clone()
        {
            object obj = new UnitAmpData();

            obj = this.MemberwiseClone();
            return obj;
        }
    }

    public class UnitVoltData : ICloneable
    {
        public UnitVoltData()
        {

        }

        public EVoltUnit UnitEA { get; set; }

        public double Value { get; set; }

        public double Maximum { get; set; }

        public double Minimum { get; set; }

        public double Increment { get; set; }

        public string DisplayFormat { get; set; }

        public object Clone()
        {
            object obj = new UnitVoltData();

            obj = this.MemberwiseClone();
            return obj;
        }
    }

    public class UnitValueData
        : ICloneable
    {
        public UnitValueData()
        {
            Type = EForceType.Current;
            Unit = "mA";
            Value = 1;
            Maximum = 2000;
            Minimum = 0;
            DisplayFormat = "0.000###";
        }

        public UnitValueData(EForceType type)
        {
            Type = type;
            switch (Type)
            {
                case EForceType.Current:
                    {
                        Unit = "mA";
                        Value = 1;
                        Maximum = 2000;
                        Minimum = 0;
                        DisplayFormat = "0.000###";
                    }
                    break;
                case EForceType.Voltage:
                    {
                        Unit = "V";
                        Value = 1;
                        Maximum = 200;
                        Minimum = 0;
                        DisplayFormat = "0.000###";
                    }
                    break;
            }

        }

        public EForceType Type { set; get; }//

        public string Unit { get; set; }

        public double Value { get; set; }

        public double Maximum { get; set; }

        public double Minimum { get; set; }

        public double Increment { get; set; }

        public string DisplayFormat { get; set; }

        public object Clone()
        {
            object obj = new UnitVoltData();

            obj = this.MemberwiseClone();
            return obj;
        }
    }

    public enum EForceType
    {
        Current = 0,
        Voltage = 1,
    }
}
