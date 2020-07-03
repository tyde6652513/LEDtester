using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class AttenuatorSettingData : ICloneable
    {
        public AttenuatorSettingData()
        {
            SysChannel = -1;
            PowerContorll = false;
            Output = true;

            WaveLength = 900;
            AvgTime = 1.0;
            Speed = 1;

            APMode = EAPMode.Attenuator;
            APowerOn = true;
            IsRecordPower = false;

            PowerUnit = ELaserPowerUnit.dBm;
            Attenuate = new ValuePara();
            Attenuate.Offset = 0;
            Attenuate.Set = 0;
            Attenuate.Unit = "dB";
            Attenuate.Mode = "Attenuator";

            Power = new ValuePara();
            Power.Offset = 0;
            Power.Set = 0.001;
            Power.Unit = ELaserPowerUnit.W.ToString() ;
            Power.Mode = "Power";
        }

        #region >>publoic property<<

        public int SysChannel;//不使用getter,省的propertygrid還要處理隱藏
        public bool IsRecordPower;
        
        [DisplayName("Auto Power Contorl")]
        [Description("\"True\" to Adjust output power as power set value automatically.\n\"False\" to set output power as set value once.")]
        public bool PowerContorll { get; set; }

        
        [DisplayName("Enable Output")]
        [Description("Enable laser output from attenuator")]
        public bool Output { get; set; }

        //output at power on => always true


        [DisplayName("Wave Length")]
        [Description("Working wave length (nm)")]
        public double WaveLength { get; set; }//nm
        [DisplayName("Average Time")]
        [Description("Time of power measureing (sec)")]
        public double AvgTime { get; set; }// s

        [DisplayName("Changing Speed")]
        [Description("Outpot power changing speed (dB/sec)")]
        public double Speed { get; set; }//dB/s

        [DisplayName("Unit of Power")]
        [Description("W or dBm")]
        public ELaserPowerUnit PowerUnit { get; set; }

        [DisplayName("Output at power on ")]
        [Description("Determines whether optical output or not at next power-on")]
        public bool APowerOn { get; set; }

        [DisplayName("Attenuat/Power")]
        [Description("Select controll in attenuate or power")]
        public EAPMode APMode { get; set; }

        [DisplayName("Attenuate")]
        [Description("Attenuate factor(dB)")]
        [TypeConverter(typeof(MyEditor.Converters.LaserSourceConverter))]
        public ValuePara Attenuate { get; set; }

        [DisplayName("Power")]
        [Description("Power")]
        [TypeConverter(typeof(MyEditor.Converters.LaserSourceConverter))]
        public ValuePara Power { get; set; }



        #endregion

        #region >>public method<<

        public void PowdBm2W()
        {           
            double setdBm = Power.Set;
            double setW = dBm2WConverter.dBm2W(setdBm);
            Power.Set = setW;
        }

        public void PowW2dBm()
        {
            double setW = Power.Set;
            double setdBm = dBm2WConverter.W2dBm(setW);
            Power.Set = setdBm;
        }

        public object Clone()
        {
            ValuePara att = this.Attenuate.Clone() as ValuePara;
            ValuePara pow = this.Power.Clone() as ValuePara;
            this.Attenuate = null;
            this.Power = null;
            AttenuatorSettingData obj = this.MemberwiseClone() as AttenuatorSettingData;

            this.Attenuate = att.Clone() as ValuePara;
            obj.Attenuate = att.Clone() as ValuePara;
            this.Power = pow.Clone() as ValuePara;
            obj.Power = pow.Clone() as ValuePara;
            return obj;
            //return obj;
        }
        #endregion

    }

    [Serializable]
    public class ValuePara : ICloneable
    {
        [ReadOnly(true)]
        [DisplayName("1.Mode")]
        public string Mode { get; set; }
        [DisplayName("2.Unit")]
        [ReadOnly(true)]
        public string Unit { get; set; }


        [BrowsableAttribute(true)]
        [DisplayName("3.Set Value")]
        public double Set { get; set; }

        [DisplayName("4.Offset Value")]
        [Description("(dB)")]
        public double Offset { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
            //return obj;
        }

    }

    [Serializable]
    public enum ELaserPowerUnit : int
    {      
        dBm = 0, 
        W =1,
    }

    [Serializable]
    public enum EAPMode : int
    {
        Attenuator = 0,
        Pwoer = 1,
        
    }


    namespace MyEditor.Converters
    {
        public class LaserSourceConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(
                ITypeDescriptorContext context,
                System.Globalization.CultureInfo culture,
                object value, Type destinationType)
            {

                if (destinationType == typeof(double) || destinationType == typeof(float))
                    return ((int)value).ToString("0.000000");


                else if (destinationType == typeof(string))
                    return "";

                return base.ConvertTo(context, culture, value, destinationType);

            }

            public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override object ConvertFrom(
                ITypeDescriptorContext context,
                System.Globalization.CultureInfo culture, object value)
            {
                if (value is string)
                {
                    string s = (string)value;
                    return Int32.Parse(s, System.Globalization.NumberStyles.AllowThousands, culture);
                }

                return base.ConvertFrom(context, culture, value);
            }


        }


        [System.AttributeUsage(System.AttributeTargets.Class |  
                       System.AttributeTargets.Struct) ]  
        public class Range:System.Attribute
        {
            private double _max;
            private double _min;

            public Range(double max,double min)
            {
                _max = max;
                _min = min;
            }
        }

//    public class Author : System.Attribute  
//{  
//    private string name;  
//    public double version;  

//    public Author(string name)  
//    {  
//        this.name = name;  
//        version = 1.0;  
//    }  
    }  

}
