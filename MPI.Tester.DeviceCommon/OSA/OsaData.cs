using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class OsaData : ICloneable
    {

        private object _lockObj;

        private double _meanWl;  // WLC

        private double _peakWl;  // WLP
        private double _peakLevel;
        private double _peakWl2;
        private double _peakLevel2;

        private double _fwhm;

        private double _watt;

        private double _smsr; // delta level
        private double _deltaLamda;

        private double _totalPow;
        private double _stdev;

        private string _traceID;
        private List<double> _wavelength;
        private List<double> _spectrum;

        private double _rms;

        /// <summary>
        /// Constructor.
        /// </summary>
        public OsaData()
        {
            this._lockObj = new object();

            this._wavelength = new List<double>();
            this._spectrum = new List<double>();

            this.Clear();
        }

        public OsaData(string traceID) : this()
        {
            this._traceID = traceID;
        }

        #region >>> Public Property <<<

        /// <summary>
        /// Active Trace
        /// </summary>
        public string TraceID
        {
            get { return this._traceID; }
            set { lock (this._lockObj) { this._traceID = value; } }
        }

        /// <summary>
        /// Centroid Wavelength at power spectrum
        /// </summary>
        public double MeanWL
        {
            get { return this._meanWl; }
            set { lock (this._lockObj) { this._meanWl = value; } }
        }

        /// <summary>
        /// Peak wavelength
        /// </summary>
        public double PeakWL
        {
            get { return this._peakWl; }
            set { lock (this._lockObj) { this._peakWl = value; } }
        }

        /// <summary>
        /// Peak Level
        /// </summary>
        public double PeakLevel
        {
            get { return this._peakLevel; }
            set { lock (this._lockObj) { this._peakLevel = value; } }
        }

        /// <summary>
        /// Seconde Peak wavelength
        /// </summary>
        public double PeakWL2
        {
            get { return this._peakWl2; }
            set { lock (this._lockObj) { this._peakWl2 = value; } }
        }

        /// <summary>
        /// 2nd Peak Level
        /// </summary>
        public double PeakLevel2
        {
            get { return this._peakLevel2; }
            set { lock (this._lockObj) { this._peakLevel2 = value; } }
        }

        /// <summary>
        /// Full width at half maximum
        /// </summary>
        public double FWHM
        {
            get { return this._fwhm; }
            set { lock (this._lockObj) { this._fwhm = value; } }
        }

        /// <summary>
        /// Radiant flux, SI unit : Watt (W)
        /// </summary>
        public double Watt
        {
            get { return this._watt; }
            set { lock (this._lockObj) { this._watt = value; } }
        }

        /// <summary>
        /// SMSR Ratio (delta level)
        /// </summary>
        public double SMSR
        {
            get { return this._smsr; }
            set { lock (this._lockObj) { this._smsr = value; } }
        }

        /// <summary>
        /// SMSR Different Wavelength
        /// </summary>
        public double DeltaLamda
        {
            get { return this._deltaLamda; }
            set { lock (this._lockObj) { this._deltaLamda = value; } }
        }

        public double TotalPower
        {
            get { return this._totalPow; }
            set { lock (this._lockObj) { this._totalPow = value; } }
        }

        public List<double> Wavelength
        {
            get { return this._wavelength; }
            set { lock (this._lockObj) { this._wavelength = value; } }
        }

        public List<double> Spectrum
        {
            get { return this._spectrum; }
            set { lock (this._lockObj) { this._spectrum = value; } }
        }

        public double  Stdev
        {
            get { return this._stdev; }
            set { lock (this._lockObj) { this._stdev = value; } }
        }

        public double RMS
        {
            get { return this._rms; }
            set { lock (this._lockObj) { this._rms = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public void Clear()
        {
            this._wavelength.Clear();
            this._spectrum.Clear();

            this._traceID = string.Empty;

            this._meanWl = 0.0d;
            
            this._peakWl = 0.0d;
            this._peakLevel = 0.0d;
            this._peakWl2 = 0.0d;
            this._peakLevel2 = 0.0d;
    
            this._fwhm = 0.0d;

            this._watt = 0.0d;

            this._smsr = 0.0d;
            this._deltaLamda = 0.0d;

            this._totalPow = 0.0d;
            this._stdev = 0.0d;
            this._rms = 0.0d;
        }

        public object Clone()
        {
            OsaData cloneObj = this.MemberwiseClone() as OsaData;

            foreach(var wl in this._wavelength)
            {
                cloneObj.Wavelength.Add(wl);
            }

            foreach (var spt in this._spectrum)
            {
                cloneObj.Spectrum.Add(spt);
            }

            return cloneObj;
        }

        #endregion
    }
}
