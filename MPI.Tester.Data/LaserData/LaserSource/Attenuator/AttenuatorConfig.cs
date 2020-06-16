using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data.LaserData.LaserSource
{
    [Serializable]
    public class AttenuatorConfig : ICloneable
    {
        #region>>private property<<
        private ELaserAttenuatorModel _attenuatorModel;
        private string _attenuatorSN;
        #endregion

        public AttenuatorConfig()
        {
            _attenuatorModel = ELaserAttenuatorModel.NONE;
            _attenuatorSN = "";
            LaserSysChannel = 0;
            AttChannel = 0;
            Enable = true;
            Slot = 0;
        }

        #region
        [DisplayName("Model")]
        public ELaserAttenuatorModel AttenuatorModel
        {
            set { _attenuatorModel = value; }
            get { return _attenuatorModel; }
        }
        //[DisplayName("Attenuator SN")]
        [BrowsableAttribute(false)]
        public string AttenuatorSN
        {
            set { _attenuatorSN = value; }
            get { return _attenuatorSN; }
        }

        [DisplayName("Address")]
        public string Address
        {
            set { _attenuatorSN = value; }
            get { return _attenuatorSN; }
        }

        [ReadOnly(true)]
        public int LaserSysChannel { set; get; }
        [DisplayName("Channel")]
        public int AttChannel { set; get; }
        public int Slot { set; get; }
        public bool Enable { set; get; }
        #endregion

        #region >>public method<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }


}

