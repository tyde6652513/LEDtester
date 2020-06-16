using System;

namespace MPI.Tester.DeviceCommon
{
    public class RangeSpec : ICloneable
    {
        private object _lockObj;

        private int _index;
        private double _minRange;
        private double _maxRange;
        private double _resolution;
        private string _unit;

		public RangeSpec()
        {
            this._lockObj = new object();

			this._index = 0;
            this._minRange = 0.0;
            this._maxRange = 0.0;
            this._resolution = 0.0;
            this._unit = "mV";
        }

		public RangeSpec(string unitStr) : this()
		{
			this._unit = unitStr;
		}

        #region >>> Public Property <<<

        public int Index
        {
			get { return this._index; }
			set { lock (this._lockObj) { this._index = value; } }             
        }

        public double MinRange
        {
            get { return this._minRange; }
            set { lock (this._lockObj) { this._minRange = value; } }              
        }

        public double MaxRange
        {
            get { return this._maxRange; }
            set { lock (this._lockObj) { this._maxRange = value; } }              
        }

        public double Resolution
        {
            get { return this._resolution; }
            set { lock (this._lockObj) { this._resolution = value; } }      
        }

        public string Unit
        {
            get { return this._unit; }
            set { lock (this._lockObj) { this._unit = value; } }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string Descrip
        {
            get
            {
                return (this._minRange.ToString() + " ~ " + this._maxRange.ToString() + " " + this._unit);
            }
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
