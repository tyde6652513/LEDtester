using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    public class SpectrumData : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private uint _channel;
        private string _name;
        private string _keyName;
        private bool _isEnable;
        private double[] _wavelength;
        private double[] _intensity;
        private double[] _absoluate;
        private double[] _dark;

        #endregion

        #region >>> Constructor / Disposor <<<

        public SpectrumData()
        {
            this._lockObj = new object();

            this._channel = 0;

            this._name = string.Empty;

            this._keyName = string.Empty;

            this._isEnable = false;

            this._wavelength = new double[] { 0.0d };

            this._intensity = new double[] { 0.0d };

            this._absoluate = new double[] { 0.0d };

            this._dark = new double[] { 0.0d };
        }

        public SpectrumData(TestItemData item, uint channel) : this()
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

        public double[] Wavelength
        {
            get { return this._wavelength; }
            set { lock (this._lockObj) { this._wavelength = value; } }
        }

        public double[] Intensity
        {
            get { return this._intensity; }
            set { lock (this._lockObj) { this._intensity = value; } }
        }

        public double[] Absoluate
        {
            get { return this._absoluate; }
            set { lock (this._lockObj) { this._absoluate = value; } }
        }

        public double[] Dark
        {
            get { return this._dark; }
            set { lock (this._lockObj) { this._dark = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            SpectrumData cloneObj = new SpectrumData();

            cloneObj._channel = this._channel;

            cloneObj._name = this._name;

            cloneObj._keyName = this._keyName;

            cloneObj._isEnable = this._isEnable;

            cloneObj._wavelength = this._wavelength.Clone() as double[];

            cloneObj._intensity = this._intensity.Clone() as double[];

            cloneObj._absoluate = this._absoluate.Clone() as double[];

            cloneObj._dark = this._dark.Clone() as double[];

            return cloneObj;
        }

        #endregion
    }
}
