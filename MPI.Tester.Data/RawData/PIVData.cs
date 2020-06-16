using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    public class PIVData : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private uint _channel;
        private string _name;
        private string _keyName;
        private bool _isEnable;
        private double[] _powArray;
        private double[] _currArray;
        private double[] _voltArray;
        private double[] _seArray;
        private double[] _rsArray;
        private double[] _pceArray;

        #endregion

        #region >>> Constructor / Disposor <<<

        public PIVData()
        {
            this._lockObj = new object();

            this._channel = 0;

            this._name = string.Empty;

            this._keyName = string.Empty;

            this._isEnable = false;

            this._powArray = new double[] { 0.0d };

            this._currArray = new double[] { 0.0d };

            this._voltArray = new double[] { 0.0d };

            this._seArray = new double[] { 0.0d };

            this._rsArray = new double[] { 0.0d };

            this._pceArray = new double[] { 0.0d };
        }

        public PIVData(TestItemData item) : this()
        {
            this._name = item.Name;

            this._keyName = item.KeyName;

            this._isEnable = item.IsEnable;
        }

        #endregion

        #region >>> Public Property <<<

        public uint Channel
        {
            get { return this._channel; }
            set { lock (this._lockObj) { this._channel = value; } }
        }

        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }

        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public double[] PowerData
        {
            get { return this._powArray; }
            set { lock (this._lockObj) { this._powArray = value; } }
        }

        public double[] CurrentData
        {
            get { return this._currArray; }
            set { lock (this._lockObj) { this._currArray = value; } }
        }

        public double[] VoltageData
        {
            get { return this._voltArray; }
            set { lock (this._lockObj) { this._voltArray = value; } }
        }

        public double[] SeData
        {
            get { return this._seArray; }
            set { lock (this._lockObj) { this._seArray = value; } }
        }

        public double[] RsData
        {
            get { return this._rsArray; }
            set { lock (this._lockObj) { this._rsArray = value; } }
        }

        public double[] PceData
        {
            get { return this._pceArray; }
            set { lock (this._lockObj) { this._pceArray = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            PIVData cloneObj = new PIVData();

            cloneObj._channel = this._channel;

            cloneObj._name = this._name;

            cloneObj._keyName = this._keyName;

            cloneObj._isEnable = this._isEnable;

            cloneObj._powArray = this._powArray.Clone() as double[];

            cloneObj._currArray = this._currArray.Clone() as double[];

            cloneObj._voltArray = this._voltArray.Clone() as double[];

            cloneObj._seArray = this._seArray.Clone() as double[];

            cloneObj._rsArray = this._rsArray.Clone() as double[];

            cloneObj._pceArray = this._pceArray.Clone() as double[];

            return cloneObj;
        }

        #endregion
    }
}
