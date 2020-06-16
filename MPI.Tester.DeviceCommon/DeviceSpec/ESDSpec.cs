using System;

namespace MPI.Tester.DeviceCommon
{
    public class ESDSpec : ICloneable
    {
        private object _lockObj;

        private double _maxHBMVolt;
        private double _minHBMVolt;

        private double _maxMMVolt;
        private double _minMMVolt;

        private double _maxZapInverval;        // ms
        private double _minZapInverval;        // ms

        private int _maxZapCount;

		public ESDSpec()
        {
            this._lockObj = new object();

            this._maxHBMVolt = 2000.0d;
            this._minHBMVolt = 100.0d;

            this._maxMMVolt = 200.0d;
            this._minMMVolt = 100.0d;

            this._maxZapInverval = 1000.0d;		// ms
            this._minZapInverval = 100.0d;		// ms

            this._maxZapCount = 20;

        }

        #region >>> Public Property <<<

        public double HBMVoltMaxValue
        {
            get { return this._maxHBMVolt; }
            set { lock (this._lockObj) { this._maxHBMVolt = value; } }       
        }

        public double HBMVoltMinValue
        {
            get { return this._minHBMVolt; }
            set { lock (this._lockObj) { this._minHBMVolt = value; } }
        }

        public double MMVoltMaxValue
        {
            get { return this._maxMMVolt; }
            set { lock (this._lockObj) { this._maxMMVolt = value; } }
        }

        public double MMVoltMinValue
        {
            get { return this._minMMVolt; }
            set { lock (this._lockObj) { this._minMMVolt = value; } }
        }

        public double ZapInvervalMaxValue
        {
            get { return this._maxZapInverval; }
            set { lock (this._lockObj) { this._maxZapInverval = value; } }
        }

        public double ZapInvervalMinValue
        {
            get { return this._minZapInverval; }
            set { lock (this._lockObj) { this._minZapInverval = value; } }
        }

        public int ZapCountMaxValue
        {
            get { return this._maxZapCount; }
            set { lock (this._lockObj) { this._maxZapCount = value; } }
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
