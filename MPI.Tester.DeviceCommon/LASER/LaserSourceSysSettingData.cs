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
    public class LaserSourceSysSettingData:ICloneable
    {
        private AttenuatorSettingData _attSetting;
        private PowerMeterSettingData _powerMeterSetting;
        private AutoTuneVOASettingData _atVOASetting;
        //private List<AttenuatorSettingData> _attSetList;
        ////private List<OpticalSwitchSettingData> _osSetList;
        //private List<PowerMeterSettingData> _pmSetList;

        public LaserSourceSysSettingData()
        {
            AttenuatorData = new AttenuatorSettingData();
            PowerMeterSetting = new PowerMeterSettingData();
            ChName = "";
            _atVOASetting = new AutoTuneVOASettingData();
            SysChannel = 0;
        }
        public LaserSourceSysSettingData(int sysCh):this()
        {
            SysChannel = sysCh;
            AttenuatorData.SysChannel = sysCh;
            PowerMeterSetting.SysChannel = sysCh;
        }
        #region >>public method<<

        #endregion

        #region >>public property<<
        public int SysChannel;
        public string ChName;

        public AttenuatorSettingData AttenuatorData
        {
            get {return _attSetting;}
            set { _attSetting = value;}
        }

        public PowerMeterSettingData PowerMeterSetting
        {
            get { return _powerMeterSetting; }
            set { _powerMeterSetting = value; }
        }
        public AutoTuneVOASettingData AutoTuneVOASetting
        {
            get { return _atVOASetting; }
            set { _atVOASetting = value; }
        }


        
        #endregion

        #region >>public method<<
        public object Clone()
        {
            LaserSourceSysSettingData obj = new LaserSourceSysSettingData();
            obj.SysChannel = SysChannel;
            if (AttenuatorData != null)
                obj.AttenuatorData =  AttenuatorData.Clone() as AttenuatorSettingData;
            if (PowerMeterSetting != null)
                obj.PowerMeterSetting =  PowerMeterSetting.Clone() as PowerMeterSettingData;
            if (AutoTuneVOASetting != null)
                obj.AutoTuneVOASetting = AutoTuneVOASetting.Clone() as AutoTuneVOASettingData;
            obj.ChName = ChName;
            return obj;

        }
        #endregion
    }
}
