using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using MPI.Tester.DeviceCommon;
using System.ComponentModel;


namespace MPI.Tester.Data.LaserData.LaserSource
{
    [Serializable]
    public class LaserSrcSysConfig : ICloneable
    {
        #region >>private <<
        private object _lockObj;
        
        private AttenuatorConfig _attenuatorConfig;//相容舊版用

        private SourceMeterAssignmentData _moniterPDSmu;

        private List< LaserSrcChConfig> _chConfigList;
        
        #endregion

        public LaserSrcSysConfig()
        {
            _lockObj = new object();
            _attenuatorConfig = new AttenuatorConfig();
            _moniterPDSmu = null;
            _chConfigList = new List<LaserSrcChConfig>();
            //IsAutoAttInPreheat =false;
            AutoAttPerCntCheck = 1;
            OSDelayInms = 0;
        }


        #region >>public Property<<
        [BrowsableAttribute(false)]
        [XmlIgnore]
        public List<AttenuatorConfig> AttList
        {
            get
            {
                List<AttenuatorConfig> aList = new List<AttenuatorConfig>();
                if (_chConfigList != null)
                {
                    foreach (var ch in _chConfigList)
                    {
                        if (ch.AttConfig != null && ch.AttConfig.AttenuatorModel != ELaserAttenuatorModel.NONE)
                        {
                            aList.Add(ch.AttConfig);
                        }
                    }
                }
                return aList;
            }

        }

        [BrowsableAttribute(false)]
        [XmlIgnore]
        public List<OpticalSwitchConfig> OSList
        {
            get
            {
                List<OpticalSwitchConfig> aList = new List<OpticalSwitchConfig>();
                if (_chConfigList != null)
                {
                    foreach (var ch in _chConfigList)
                    {
                        
                        if (ch.OpticalSwitchList != null )
                        {
                            foreach(var os in ch.OpticalSwitchList)
                            {
                                if (os != null && os.OpticalSwitchModel != EOpticalSwitchModel.NONE)
                                { aList.Add(os); }
                            }                           

                        }
                    }
                }

                return aList;
            }

        }
        [DisplayName("Laser Attenuator")]
        [BrowsableAttribute(true)]
        [TypeConverter(typeof(MyEditor.Converters.LaserSourceConverter))]
        public AttenuatorConfig Attenuator
        {
            set
            {
                lock (_lockObj)
                {
                    _attenuatorConfig = value;
                }
            }
            get
            { return _attenuatorConfig; }

            //set
            //{
            //    lock (_lockObj)
            //    {
            //        if (AttList == null || AttList.Count < 1)
            //        {
            //            AttList = new List<AttenuatorConfig>();
            //            AttList.Add(new AttenuatorConfig());
            //        }
            //        AttList[0] = value;
            //    }
            //}
            //get
            //{
            //    if (AttList != null && AttList.Count > 0)
            //    {
            //        return AttList[0];
            //    }
            //    return null;
            //}
        }

        [XmlIgnore]
        [BrowsableAttribute(false)]
        public List<LaserSrcChConfig> ChConfigList
        {
            set
            {
                lock (_lockObj)
                {
                    _chConfigList = value;
                }
            }
            get
            { return _chConfigList; }
        }

        [BrowsableAttribute(false)]
        public LaserSrcChConfig[] ChConfigArr
        {
            get
            {
                if (_chConfigList == null)
                { _chConfigList = new List<LaserSrcChConfig>(); }
                return _chConfigList.ToArray();
            }
            set
            {
                lock (_lockObj)
                {
                    _chConfigList = new List<LaserSrcChConfig>();
                    _chConfigList.AddRange(value);
                }
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SourceMeterAssignmentData MoniterPDSMU
        {
            set
            {
                lock (_lockObj)
                {
                    _moniterPDSmu = value;
                }
            }
            get
            {
                _moniterPDSmu = null;
                if (_chConfigList != null)
                {
                    foreach (var ch in _chConfigList)
                    {
                        if (ch.PowerMeterConfig != null && ch.PowerMeterConfig.PowerMeterModel == EPowerMeter.K2600)
                        {
                            _moniterPDSmu = new SourceMeterAssignmentData();
                            _moniterPDSmu.ConnectionPort = ch.PowerMeterConfig.Address;
                            _moniterPDSmu.SMU = (ch.PowerMeterConfig.Slot < 2)?"A":"B";                         
                        }
                    }
                }
                return _moniterPDSmu;
            }

        }


        [BrowsableAttribute(false)]
        [XmlIgnore]
        public List<PowerMeterConfig> PowerMeterList
        {
            get
            {
                List<PowerMeterConfig> pList = new List<PowerMeterConfig>();
                if (_chConfigList != null)
                {
                    foreach (var ch in _chConfigList)
                    {
                        if (ch.PowerMeterConfig != null && ch.PowerMeterConfig.PowerMeterModel != EPowerMeter.NONE)
                        {
                            pList.Add(ch.PowerMeterConfig);
                        }
                    }
                }
                return pList;
            }

        }

        //public bool IsAutoAttInPreheat { get; set; }

        /// <summary>
        /// Auto tune Att when "laser check times" % AutoAttPerCheck == 0
        /// </summary>
        public int AutoAttPerCntCheck { get; set; }

        [XmlIgnore]
        public double OSDelayInms { get; set; }

        
        #endregion

        #region >>public method<<
        public object Clone()
        {
            //return this.MemberwiseClone();
            LaserSrcSysConfig obj = new LaserSrcSysConfig();
            //obj.AttArr = AttList.ToArray();
            //obj._laserSrcConfig = this._laserSrcConfig.Clone() as LaserSourceConfig;
            obj.ChConfigList = new List<LaserSrcChConfig>();
            obj.ChConfigList.AddRange(ChConfigArr);

            obj.Attenuator = Attenuator.Clone() as AttenuatorConfig;
            if (_moniterPDSmu != null)
            {
                obj._moniterPDSmu = this._moniterPDSmu.Clone() as SourceMeterAssignmentData;
            }
            //obj.IsAutoAttInPreheat = this.IsAutoAttInPreheat;
            obj.AutoAttPerCntCheck = this.AutoAttPerCntCheck;
            obj.OSDelayInms = this.OSDelayInms;
            return obj;
        }

        public void ModifyDevSetCh()
        {
            if (_chConfigList != null)
            {
                foreach (var chData in _chConfigList)
                {
                    if (chData != null)
                    {
                        int sysCh = chData.SysChannel;
                        if (chData.PowerMeterConfig != null)
                        {
                            chData.PowerMeterConfig.LaserSysChannel = sysCh;
                        }
                        if (chData.AttConfig != null)
                        {
                            chData.AttConfig.LaserSysChannel = sysCh;
                        }
                        if (chData.OpticalSwitchList != null)
                        {
                            foreach (var os in chData.OpticalSwitchList)
                            {
                                os.LaserSysChannel = sysCh;
                            }
                        }
                    }
                }
            }
        }


        public virtual List<Dictionary<string, object>> GetLaserInfoList()
        {
            List<Dictionary<string, object>> lsInfoList = new List<Dictionary<string, object>>();
            if (ChConfigList != null && ChConfigList.Count > 0)
            {
                for (int i = 0; i < ChConfigList.Count; ++i)
                {
                    lsInfoList.Add(ChConfigList[i].GetLaserSrcChInfoList());
                }
            }

            return lsInfoList;
        }

      
        #endregion

        #region >>private method <<
        //private void SetChListToConfig()
        //{
        //    _chConfigList = new List<LaserSrcChConfig>();
        //    if (ChConfigList != null)
        //    {
                
        //    }
        //}
        #endregion

    }

    [Serializable]
    public class LaserSrcChConfig : ICloneable
    {
        int _sysCh = -1;
        bool _enable = true;
        List<OpticalSwitchConfig> _osList;
        //public bool IsDefauleChannel;

        public LaserSrcChConfig()
        {
            IsDefauleChannel = true;
            
            ChannelName = "NONE";
            AttConfig = new AttenuatorConfig();
            OpticalSwitchList = new List<OpticalSwitchConfig>();
            Enable = true;
            SysChannel = -1;
            _osList = new List<OpticalSwitchConfig>();
            PowerMeterConfig = new PowerMeterConfig();
        }

        #region
        public int SysChannel
        {
            get { return _sysCh; }
            set
            {
                _sysCh = value;
                if (AttConfig != null) { AttConfig.LaserSysChannel = _sysCh; }
                if (OpticalSwitchList != null)
                {
                    foreach (var ch in OpticalSwitchList)
                    {
                        ch.LaserSysChannel = _sysCh;
                    }
                }
                if (PowerMeterConfig != null) { PowerMeterConfig.LaserSysChannel = _sysCh; }
            }
        }
        [DisplayName("1.Name")]
        public string ChannelName { set; get; }
        [DisplayName("Enable")]
        [Browsable(false)]
        //public bool Enable { set; get; }
        //_enable
        public bool Enable
         {
            get { return _enable; }
            set
            {
                _enable = value;
                if (AttConfig != null) { AttConfig.Enable &= _enable; }
                if (OpticalSwitchList != null)
                {
                    foreach (var ch in OpticalSwitchList)
                    {
                        ch.Enable &= _enable;
                    }
                }
                if (PowerMeterConfig != null) { PowerMeterConfig.Enable &= _enable; }
            }
        }

        [Browsable(false)]
        public bool IsDefauleChannel { set; get; }//預設狀態
        [DisplayName("2.Attenuator")]
        [TypeConverter(typeof(MyEditor.Converters.LaserSourceConverter))]
        public AttenuatorConfig AttConfig { set; get; }

        [XmlIgnore]
        [DisplayName("3.Optical Switch")]
        [TypeConverter(typeof(MyEditor.Converters.LaserSourceConverter))]
        public List<OpticalSwitchConfig> OpticalSwitchList//有可能是串接
        {
            get { return _osList; }
            set { _osList = value; }
        }

        [DisplayName("4.Power Meter")]
        [TypeConverter(typeof(MyEditor.Converters.LaserSourceConverter))]
        public PowerMeterConfig PowerMeterConfig { set; get; }

       
        [Browsable(false)]
        public OpticalSwitchConfig[] OpticalSwitchArr
        {
            get
            {
                if (_osList != null)
                { return _osList.ToArray(); }
                return null;
            }
            set
            {
                if (value != null)
                {
                    _osList = new List<OpticalSwitchConfig>();
                }
                _osList.AddRange(value);
            }
        }

        #endregion


        public virtual Dictionary<string, object>  GetLaserSrcChInfoList()
        {
            Dictionary<string, object> LaserInfoDic = new Dictionary<string, object>();
            LaserInfoDic.Add("ChannelName", ChannelName);
            LaserInfoDic.Add("SysChannel", SysChannel.ToString());
            LaserInfoDic.Add("Enable", Enable.ToString());
            LaserInfoDic.Add("VOAConfig", AttConfig);
            LaserInfoDic.Add("PowerMeterConfig", PowerMeterConfig);
            LaserInfoDic.Add("OSList", _osList);


            return LaserInfoDic;
        }
        public object Clone()
        {
            LaserSrcChConfig obj = new LaserSrcChConfig();
            obj.IsDefauleChannel = IsDefauleChannel;
            obj.SysChannel = SysChannel;
            obj.ChannelName = ChannelName;
            obj.Enable = Enable;
            obj.AttConfig = AttConfig.Clone() as AttenuatorConfig;
            obj.OpticalSwitchList = new List<OpticalSwitchConfig>();
            obj.OpticalSwitchList.AddRange(OpticalSwitchArr);
            obj.PowerMeterConfig = PowerMeterConfig.Clone() as PowerMeterConfig;
            return obj;
        }
 
    }
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

}
