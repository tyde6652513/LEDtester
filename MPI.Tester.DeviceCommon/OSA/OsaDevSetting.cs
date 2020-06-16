using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class OsaDevSetting :  ICloneable
    {
        private object _lockObj;

        private int _autoLowCount;
        private int _autoHighCount;

        private int _limitLowCount;
        private int _limitHighCount;
        private int _limitTargetCount;
        private bool _isSaveRawData;

        public OsaDevSetting()
        {
            //this._simulationLevel = 0;
            this._lockObj = new object();
            this._autoLowCount = 30000;
            this._autoHighCount = 60000;

            this._limitLowCount = 30000;
            this._limitHighCount = 60000;
            this._isSaveRawData = false;
        }

        #region >>> Public Property <<<

        public int AutoHighCount
        {
            get { return this._autoHighCount; }
            set
            {
                lock (this._lockObj)
                {
                    this._autoHighCount = value;
                }
            }

        }

        public bool IsSaveRawData
        {
            get { return this._isSaveRawData; }
            set { lock (this._lockObj) {this._isSaveRawData = value; } }
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
