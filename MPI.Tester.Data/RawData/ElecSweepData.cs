using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    public class ElecSweepData : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private uint _channel;
        private string _name;
        private string _keyName;
        private bool _isEnable;
        private double[] _timeChain;
        private double[] _applyData;
        private double[] _sweepData;
        private double[] _derivative;

        #endregion

        #region >>> Constructor / Disposor <<<

        public ElecSweepData()
        {
            this._lockObj = new object();

            this._channel = 0;

            this._name = string.Empty;

            this._keyName = string.Empty;

            this._isEnable = false;

            this._timeChain = new double[] { 0.0d };

            this._applyData = new double[] { 0.0d };

            this._sweepData = new double[] { 0.0d };

            this._derivative = new double[] { 0.0d };
        }

        public ElecSweepData(TestItemData item, uint channel) : this()
        {
            this._channel = channel;

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

        public double[] TimeChain
        {
            get { return this._timeChain; }
            set { lock (this._lockObj) { this._timeChain = value; } }
        }

        public double[] ApplyData
        {
            get { return this._applyData; }
            set { lock (this._lockObj) { this._applyData = value; } }
        }

        public double[] SweepData
        {
            get { return this._sweepData; }
            set { lock (this._lockObj) { this._sweepData = value; } }
        }

        public double[] Derivative
        {
            get { return this._derivative; }
            set { lock (this._lockObj) { this._derivative = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            ElecSweepData cloneObj = new ElecSweepData();

            cloneObj._channel = this._channel;

            cloneObj._name = this._name;

            cloneObj._keyName = this._keyName;

            cloneObj._isEnable = this._isEnable;

            cloneObj._timeChain = this._timeChain.Clone() as double[];

            cloneObj._applyData = this._applyData.Clone() as double[];

            cloneObj._sweepData = this._sweepData.Clone() as double[];

            cloneObj.Derivative = this.Derivative.Clone() as double[];

            return cloneObj;
        }

        #endregion
    }
}
