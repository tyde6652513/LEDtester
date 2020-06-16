using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
	[Serializable]
	public class LCRSettingData : ICloneable
	{
        private EMsrtType _msrtType;
		private ELCRTestType _lcrMsrtType;
		private ELCRMsrtSpeed _msrtSpeed;
		private ELCRSignalMode _signalMode;
		private ELCRDCBiasMode _dcBiasMode;
		private double _msrtDelayTime;
		private double _frequency;
		private double _dcBiasV;
		private double _dcBiasI;
		private double _signalLevelV;
		private double _signalLevelI;

        //for lcr sweep 

        private double _signalLevel;

        private double _dcBiasStart;
        private double _dcBiasEnd;

        private double _biasDelay;
        private int _points;
        
        //private int
        ///

		private int _range;
        private bool _hiAccMode;
		private object _lockObj;

        

		public LCRSettingData()
		{
            this._msrtType = EMsrtType.LCR;

            this._lcrMsrtType = ELCRTestType.ZTD;

			this._msrtSpeed = ELCRMsrtSpeed.Medium;

			this._signalMode = ELCRSignalMode.Voltage;

			this._dcBiasMode = ELCRDCBiasMode.Voltage;

			this._msrtDelayTime = 0.0d;

			this._frequency = 100000.0d;

			this._dcBiasV = 0.0d;

			this._dcBiasI = 0.0d;

			this._signalLevelV = 0.5;

			this._signalLevelI = 0.001;

			this._range = 0;

            this._signalLevel = 0;

            this._dcBiasStart=0 ;

            this._dcBiasEnd = 0;

            this._biasDelay = 0;

            this._points = 1;

			this._lockObj = new object();
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

        public EMsrtType MsrtType
		{
            get { return this._msrtType; }
            set { lock (this._lockObj) { this._msrtType = value; } }
		}

        

		public ELCRTestType LCRMsrtType
		{
            get { return this._lcrMsrtType; }
            set { lock (this._lockObj) { this._lcrMsrtType = value; } }
		}

		public ELCRMsrtSpeed MsrtSpeed
		{
			get { return this._msrtSpeed; }
			set { lock (this._lockObj) { this._msrtSpeed = value; } }
		}

		public ELCRSignalMode SignalMode
		{
			get { return this._signalMode; }
			set { lock (this._lockObj) { this._signalMode = value; } }
		}

		public ELCRDCBiasMode DCBiasMode
		{
			get { return this._dcBiasMode; }
			set { lock (this._lockObj) { this._dcBiasMode = value; } }
		}

		public double MsrtDelayTime
		{
			get { return this._msrtDelayTime; }
			set { lock (this._lockObj) { this._msrtDelayTime = value; } }
		}

		public double Frequency
		{
			get { return this._frequency; }
			set { lock (this._lockObj) { this._frequency = value; } }
		}

		public double DCBiasV
		{
			get { return this._dcBiasV; }
			set { lock (this._lockObj) { this._dcBiasV = value; } }
		}

		public double DCBiasI
		{
			get { return this._dcBiasI; }
			set { lock (this._lockObj) { this._dcBiasI = value; } }
		}

		public double SignalLevelV
		{
			get { return this._signalLevelV; }
			set { lock (this._lockObj) { this._signalLevelV = value; } }
		}

		public double SignalLevelI
		{
			get { return this._signalLevelI; }
			set { lock (this._lockObj) { this._signalLevelI = value; } }
		}

        public double SignalLevel
        {
            get { return this._signalLevel; }
            set { lock (this._lockObj) { this._signalLevel = value; } }
        }


        public double DCBiasStart
        {
            get { return this._dcBiasStart; }
            set { lock (this._lockObj) { this._dcBiasStart = value; } }
        }
        public double DCBiasEnd
        {
            get { return this._dcBiasEnd; }
            set { lock (this._lockObj) { this._dcBiasEnd = value; } }
        }

        public double DCBiasStep
        {
            get
            {
                if (this.Point > 1)
                {
                    return (this._dcBiasEnd - this._dcBiasStart) / (this.Point - 1);
                }
                else
                {
                    return 0;
                }
            }
        }

        public double BiasDelay
        {
            get { return this._biasDelay; }
            set { lock (this._lockObj) { this._biasDelay = value; } }
        }
        public int Point
        {
            get { return this._points; }
            set { lock (this._lockObj) { this._points = value; } }
        }

		public int Range
		{
			get { return this._range; }
			set { lock (this._lockObj) { this._range = value; } }
		}

        public bool HighAcc
        {
            get { return _hiAccMode; }
            set { lock (this._lockObj) { _hiAccMode = value; } }
        }
	}
}
