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
    public class LaserSourceConfig : ICloneable
    {
        
        #region>>private property<<
        private ELaserSourceModel _laserSourceModel;
        private string _laserSourceSN;
        #endregion

        public LaserSourceConfig()
        {
            _laserSourceModel = ELaserSourceModel.NONE;
            _laserSourceSN = "";
        }

        #region >>publoic property<<
        
        [DisplayName("Laser Model")]
        public ELaserSourceModel LaserSourceModel
        {
            set { _laserSourceModel = value; }
            get { return _laserSourceModel; }
        }
        [DisplayName("Laser SN")]
        public string LaserSourceSN
        {
            set { _laserSourceSN = value; }
            get { return _laserSourceSN; }
        }
        #endregion

        #region >>public method<<


        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

    }


   
}
