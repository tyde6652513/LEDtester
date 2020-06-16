using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public class ESDDevSetting
    {
        private object _lockObj;

		private int _waitTime;			// ms
		private int _safeTime;			// ms
		//private int _timeOut;			// ms
		private string _comPortName;

        private int _chargeTime;
        private int _dischargeTime;

        private bool _isHighSpeedMode;
        private double _highSpeedDelayTime;

        private double _estimateDelayTime;

        private double _prechargeWaitTime;

        private List<ChannelAssignmentData> _assigmntTable;

        public ESDDevSetting()
        {
            this._lockObj = new object();
  
			this._waitTime = 5;
			this._safeTime = 5;
			//this._timeOut = 5000;	// ms
			this._comPortName = "COM1";

            this._chargeTime = 12;      // ms
            this._dischargeTime = 3;    // ms

            this._isHighSpeedMode = false;

            this._highSpeedDelayTime = 0.0d;

            this._estimateDelayTime = 0.0d;

            this._prechargeWaitTime = 0.0d;

            this._assigmntTable = new List<ChannelAssignmentData>();
        }
                
        #region >>> Public Property <<<

		public int WaitTime
		{
			get { return this._waitTime; }
			set { lock (this._lockObj) { this._waitTime = value; } }
		}

		public int SafeTime
		{
			get { return this._safeTime; }
			set { lock (this._lockObj) { this._safeTime = value; } }
		}

		public string ComPortName
		{
			get { return this._comPortName; }
			set { lock (this._lockObj) { this._comPortName = value; } }
		}
                
        public int ChargeTime
        {
            get { return this._chargeTime; }
            set { lock (this._lockObj) { this._chargeTime = value; } }
        }

        public int DischargeTime
        {
            get { return this._dischargeTime; }
            set { lock (this._lockObj) { this._dischargeTime = value; } }
        }

        public bool IsHighSpeedMode
        {
            get { return this._isHighSpeedMode; }
            set { lock (this._lockObj) { this._isHighSpeedMode = value; } }
        }

        public double EstimateDelayTime
        {
            get { return this._estimateDelayTime; }
            set { lock (this._lockObj) { this._estimateDelayTime = value; } }
        }

        public double HighSpeedDelayTime
        {
            get { return this._highSpeedDelayTime; }
            set { lock (this._lockObj) { this._highSpeedDelayTime = value; } }
        }

        public double PrechargeWaitTime
        {
            get { return this._prechargeWaitTime; }
            set { lock (this._lockObj) { this._prechargeWaitTime = value; } }
        }

        public List<ChannelAssignmentData> ChannelAssignment
        {
            get { return this._assigmntTable; }
            set { lock (this._lockObj) { this._assigmntTable = value; } }
        }

		#endregion
	}
}
