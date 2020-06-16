using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class SwitchSettingData : ICloneable
    {
        private object _lockObj;

        private ESrcSensingMode _senseMode;

        public SwitchSettingData()
        {
            this._lockObj = new object();

            this._senseMode = ESrcSensingMode._4wire;
        }

        #region >>> Public Property <<<

        public ESrcSensingMode MsrtSensingMode
        {
            get { return this._senseMode; }
            set { lock (this._lockObj) { this._senseMode = value; } }
        }

        #endregion
	
        #region >>> Public Method <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }
            
        #endregion
    }
}
