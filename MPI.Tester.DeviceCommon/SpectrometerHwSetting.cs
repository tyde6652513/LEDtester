using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class SpectrometerHWSetting
    {
        private ESpectrometerInterfaceType _sptInterfaceType;

        private ESpectrometerCalibDataMode _sptCalibMode;

        private object _lockObj;

        private bool _isMultiSpectrometer;

        public SpectrometerHWSetting()
        {
            this._lockObj = new object();
            
            this._sptInterfaceType = ESpectrometerInterfaceType.InterfaceUSB;

            this._sptCalibMode = ESpectrometerCalibDataMode.IntegratingSphere;

            this._isMultiSpectrometer = false;
        }

         public ESpectrometerInterfaceType SptInterfaceType
        {
            get { return this._sptInterfaceType; }
            set { lock (this._lockObj) { this._sptInterfaceType = value; } }
        }

        public ESpectrometerCalibDataMode SpectometerCalibMode
        {
            get { return this._sptCalibMode; }
            set { lock (this._lockObj) { this._sptCalibMode = value; } }
        }

        public bool IsMultiSpectrometer
        {
            get { return this._isMultiSpectrometer; }
            set { lock (this._lockObj) { this._isMultiSpectrometer = value; } }
        }


        #region >>> Public Method <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

    }
}
