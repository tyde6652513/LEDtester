using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
	[Serializable]
    public class OptiSettingData : System.ICloneable
    {		 
        private object _lockObj;

        private ESensingMode _sensingMode;
        private double _fixIntegralTime;
        private double _limitIntegralTime;
        private double _trigDelayTime;
        private double _densityFilterPostion;

        private uint _order;

        public OptiSettingData()
        {
            this._lockObj = new object();

            this._sensingMode = ESensingMode.Fixed;
            this._fixIntegralTime = 10.0d;
            this._limitIntegralTime = 50.0d;
            this._trigDelayTime = 0.0d;
            this._densityFilterPostion = 0.0d;

            this._order = 0;
        }

        #region >>> Public Property <<<

        public ESensingMode SensingMode
        {
            get { return this._sensingMode; }
            set { lock (this._lockObj) { this._sensingMode = value; } }
        }

        public double FixIntegralTime
        {
            get { return this._fixIntegralTime; }
            set { lock (this._lockObj) { this._fixIntegralTime = value; } }
        }

        public double LimitIntegralTime
        {
            get { return this._limitIntegralTime; }
            set { lock (this._lockObj) { this._limitIntegralTime = value; } }
        }

        public double TrigDelayTime
        {
            get { return this._trigDelayTime; }
            set { lock (this._lockObj) { this._trigDelayTime = value; } }
        }

        public double DensityFilterPostion
        {
            get
            {
                return this._densityFilterPostion;
            }
            set
            {
                lock (this._lockObj)
                {
                    this._densityFilterPostion = value;
                }
            }
        }

        public uint Order
        {
            get { return this._order; }
            set { lock (this._lockObj) { this._order = value; } }
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
