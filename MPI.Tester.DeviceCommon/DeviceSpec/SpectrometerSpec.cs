using System;

namespace MPI.Tester.DeviceCommon
{
    public class SpectrometerSpec : ICloneable
    {
        private object _lockObj;

        private uint _minTime;
        private uint _maxTime;
        private double _minWavelength;
        private double _maxWavelength;

		public SpectrometerSpec()
        {
            this._lockObj = new object();

            this._minTime = 5;
            this._maxTime = 500;
            this._minWavelength = 350.0d;
            this._maxWavelength = 1000.0d;
        }

        #region >>> Public Property <<<

        public uint MinTime
        {
            get { return this._minTime; }
            set { lock (this._lockObj) { this._minTime = value; } }            
        }

        public uint MaxTime
        {
            get { return this._maxTime; }
            set { lock (this._lockObj) { this._maxTime = value; } }
        }

        public double MinWavelength
        {
            get { return this._minWavelength; }
            set { lock (this._lockObj) { this._minWavelength = value; } }            
        }

        public double MaxWavelength
        {
            get { return this._maxWavelength; }
            set { lock (this._lockObj) { this._maxWavelength = value; } }
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
