using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class ESDSettingData : ICloneable
    {
		private object _lockObj;

		private bool _isEnable;
		private string _keyName;
		private EESDMode _mode;
		
		private int _intervalTime;
		private int _count;
        private double _gainVolt;
        private double _offsetVolt;

        private EESDPolarity _polarity;     
        private int _voltage;

        private double _chargeDelayTime;
        private uint _order;

        public ESDSettingData()
        {
            this._lockObj = new object();

			this._isEnable = true;
			this._mode = EESDMode.HBM;

            this._intervalTime = 0;
            this._count = 1;

            this._polarity = EESDPolarity.N;
			this._voltage = 100;

            this._gainVolt = 1.0;
            this._offsetVolt = 0.0d;

            this._chargeDelayTime = 0.0d;

            this._order = 0;
        }

        #region >>> Public Property <<<
            
		public bool IsEnable
            {
			get { return this._isEnable; }
			set { lock (this._lockObj) { this._isEnable = value; } }
            }

		public string KeyName
		{
			get { return this._keyName; }
			set { lock (this._lockObj) { this._keyName = value; } }
		}

		public EESDMode Mode
		{
			get { return this._mode; }
			set { lock (this._lockObj) { this._mode = value; } }
		}

        public double GainVolt
        {
            get { return this._gainVolt; }
            set { lock (this._lockObj) { this._gainVolt = value; } }
        }

        public double OffsetVolt
        {
            get { return this._offsetVolt; }
            set { lock (this._lockObj) { this._offsetVolt = value; } }
        }

		public int Count
		{
			get { return this._count; }
			set { lock (this._lockObj) { this._count = value; } }
		}

        public int IntervalTime
        {
            get { return this._intervalTime; }
            set { lock (this._lockObj) { this._intervalTime = value; } }
        }

        public EESDPolarity Polarity
        {
            get { return this._polarity; }
            set { lock (this._lockObj) { this._polarity = value; } }
        }

        public int ZapVoltage
		{
            get { return this._voltage; }
            set { lock (this._lockObj) { this._voltage = value; } }
        }

        public double ChargeDelayTime
        {
            get { return this._chargeDelayTime; }
            set { lock (this._lockObj) { this._chargeDelayTime = value; } }
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
			// return this.MemberwiseClone();

			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, this);
			ms.Position = 0;
			object obj = bf.Deserialize(ms);
			ms.Close();
			return obj;
		}

		#endregion
        
    }
}
