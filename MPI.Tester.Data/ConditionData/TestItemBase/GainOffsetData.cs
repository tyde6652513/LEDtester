using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.Data
{
    [Serializable]
    public class GainOffsetData : ICloneable
    {
        private object _lockObj;

        private string _id;
        private string _keyName;
        private string _name;

        private EGainOffsetType _type;
		private EGainOffsetType _type2;
		private EGainOffsetType _type3;
		private EGainOffsetType _type4;

        private bool _isEnable;
		private bool _isVision;

        private double _square;
        private double _gain;
        private double _offset;

        private double _square2;
        private double _gain2;
        private double _offset2;

        private double _square3;
        private double _gain3;
        private double _offset3;

		private double _square4;
		private double _gain4;
		private double _offset4;

        private double _param01;
        private double _param02;

        public GainOffsetData()
        {
            this._lockObj = new object();

            this._id = string.Empty;
            this._keyName = string.Empty;
            this._name = string.Empty;
            
            this._isEnable = true;
			this._isVision = true;

            this._type = EGainOffsetType.None;
            this._square = 0.0d;
            this._gain = 1.0d;
            this._offset = 0.0d;

            this._type2 = EGainOffsetType.None;
            this._square2 = 0.0d;
            this._gain2 = 1.0d;
            this._offset2 = 0.0d;

            this._type3 = EGainOffsetType.None;
            this._square3 = 0.0d;
            this._gain3 = 1.0d;
            this._offset3= 0.0d;

			this._type4 = EGainOffsetType.None;
			this._square4 = 0.0d;
			this._gain4 = 1.0d;
			this._offset4 = 0.0d;

            this._param01 = 0.0d;
            this._param02 = 0.0d;
        }

        public GainOffsetData(bool isEnable, EGainOffsetType type) : this()
        {
            this._isEnable = isEnable;
            this._type = type;
        }

        #region >>> Public Property <<<

        public string ID
        {
            get { return this._id; }
            set { lock (this._lockObj) { this._id = value; } }          
        }
        
        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }
        
        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }           
        }

        public EGainOffsetType Type
        {
            get { return this._type; }
            set { lock (this._lockObj) { this._type = value; } }       
        }

		public EGainOffsetType Type2
		{
			get { return this._type2; }
			set { lock (this._lockObj) { this._type2 = value; } }
		}

		public EGainOffsetType Type3
		{
			get { return this._type3; }
			set { lock (this._lockObj) { this._type3 = value; } }
		}

		public EGainOffsetType Type4
		{
			get { return this._type4; }
			set { lock (this._lockObj) { this._type4 = value; } }
		}

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }         
        }

		public bool IsVision
		{
			get { return this._isVision; }
			set { lock (this._lockObj) { this._isVision = value; } }
		}

        public double Square
        {
            get { return this._square; }
            set { lock (this._lockObj) { this._square = value; } }              
        }

        public double Gain
        {
            get { return this._gain; }
            set { lock (this._lockObj) { this._gain = value; } }       
        }

        public double Offset
        {
            get { return this._offset; }
            set { lock (this._lockObj) { this._offset = value; } }       
        }

        public double Square2
        {
            get { return this._square2; }
            set { lock (this._lockObj) { this._square2 = value; } }
        }

        public double Gain2
        {
            get { return this._gain2; }
            set { lock (this._lockObj) { this._gain2 = value; } }
        }

        public double Offset2
        {
            get { return this._offset2; }
            set { lock (this._lockObj) { this._offset2 = value; } }
        }

        public double Square3
        {
            get { return this._square3; }
            set { lock (this._lockObj) { this._square3 = value; } }
        }

        public double Gain3
        {
            get { return this._gain3; }
            set { lock (this._lockObj) { this._gain3 = value; } }
        }

        public double Offset3
        {
            get { return this._offset3; }
            set { lock (this._lockObj) { this._offset3 = value; } }
        }

		public double Square4
		{
			get { return this._square4; }
			set { lock (this._lockObj) { this._square4 = value; } }
		}

		public double Gain4
		{
			get { return this._gain4; }
			set { lock (this._lockObj) { this._gain4 = value; } }
		}

		public double Offset4
		{
			get { return this._offset4; }
			set { lock (this._lockObj) { this._offset4 = value; } }
		}

        public double Param01
        {
            get { return this._param01; }
            set { lock (this._lockObj) { this._param01 = value; } }            
        }
        
        public double Param02
        {
            get { return this._param02; }
            set { lock (this._lockObj) { this._param02 = value; } }
        }

        #endregion

        #region >>> Public Methods <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    public class FactorBoundary
    {
        private object _lockObj;

        private string _keyName;

        private string _name;

        private EGainOffsetType _type;

        private bool _isEnable;

        private double _gainLow;

        private double _gainHigh;

        public FactorBoundary(string name,string keyname,bool isEnable)
        {
            this._lockObj = new object();

            this._name = name;

            this._keyName = keyname;

            this._gainHigh = 0.0d;

            this._gainLow = 0.0d;

            this._type = EGainOffsetType.Offset;

            _isEnable = isEnable;
        }

        #region >>> Public Property <<<

        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public double Low
        {
            get { return this._gainLow; }
            set { lock (this._lockObj) { this._gainLow = value; } }
        }

        public double High
        {
            get { return this._gainHigh; }
            set { lock (this._lockObj) { this._gainHigh = value; } }
        }

         public EGainOffsetType Type
        {
            get { return this._type; }
            set { lock (this._lockObj) { this._type = value; } }
        }

        #endregion
    }

}
