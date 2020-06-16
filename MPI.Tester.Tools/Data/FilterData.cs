using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.Tools
{
	public class FilterData
	{
		private object _lockObj;

		private string _keyName;
		private string _name;
		private bool _isEnable;
		private double _min;
		private double _max;
		private string _unit;
        private int _stdFilterCounts;
        private int _msrtFilterCounts;

		public FilterData()
		{
			this._lockObj = new object();

			this._keyName = string.Empty;
			this._name = string.Empty;
			this._isEnable = false;
			this._min = 0.0d;
			this._max = 1.0d;
			this._unit = string.Empty;
		}

		public FilterData(string keyName, string name) : this()
		{
			this._keyName = keyName;
			this._name = name;
		}

		#region >>> Public Property <<<

		public string KeyName
		{
			get{ return this._keyName; }
			set{ lock( this._lockObj) { this._keyName = value; } }
		}

		public string Name
		{
			get{ return this._name; }
			set{ lock( this._lockObj) { this._name = value; } }
		}

		public bool IsEnable
		{
			get{ return this._isEnable; }
			set{ lock (this._lockObj) { this._isEnable = value; } }
		}

		public double Min
		{
			get { return this._min; }
			set { lock (this._lockObj) { this._min = value; } }
		}

		public double Max
		{
			get{ return this._max; }
			set{ lock( this._lockObj) { this._max = value; } }
		}

		public string Unit
		{
			get { return this._unit; }
			set { lock (this._lockObj) { this._unit = value; } }
		}

        public int StdFilterCounts
        {
            get { return this._stdFilterCounts; }
            set { lock (this._lockObj) { this._stdFilterCounts = value; } }
        }

        public int MsrtFilterCounts
        {
            get { return this._msrtFilterCounts; }
            set { lock (this._lockObj) { this._msrtFilterCounts = value; } }
        }

		#endregion
		
	}

    public class GainOffset
    {
        private object _lockObj;

        private string _keyName;
        private string _name;
        private bool _isEnable;
        private double _square;
        private double _gain;
        private double _offset;
        private double _rSqure;
		private EGainOffsetType _calcType;

        private double _value;

        public GainOffset()
        {
            this._lockObj = new object();
            this._keyName = string.Empty;
            this._name = string.Empty;
            this._isEnable = false;
            this._square = 0.0d;
            this._gain = 1.0d;
            this._offset = 0.0d;
            this._rSqure = 0.0d;
            this._value = 0.0d;

        }

        public GainOffset(string keyName, string name)
            : this()
        {
            this._keyName = keyName;
            this._name = name;
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

        public double RSqure
        {
            get { return this._rSqure; }
            set { lock (this._lockObj) { this._rSqure = value; } }
        }

        public double Value
        {
            get { return this._value; }
            set { lock (this._lockObj) { this._value = value; } }
        }

		public EGainOffsetType CalcType
		{
			get { return this._calcType; }
			set { lock (this._lockObj) { this._calcType = value; } }
		}

        #endregion

    }
}
