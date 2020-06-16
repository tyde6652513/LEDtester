using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class OsaSettingData : ICloneable
    {    
        private object _lockObj;

        private bool _isTrigger;

        private string _traceID;

        private double _centerWavelength;

        private double _SpanOfWavelength;

        private double _resoluation;

        private double _referenceLevel;

        private double _logDiv;

       // private EAQ6370DAnalysisMode _analysisMode;

        private double _vidoeBW;

        private uint _samplingPoints;

        private EMS9740AnalysisMode _msAnalysisMode;

        // MS9740 AP-FP
        private double _fpSliceLevel;

        // MS9740 AP-DFB
        private EMS9740DfbSideMode _msDfbSideMode;
        private double _dfbSliceLevel;
        private double _dfbStdevFactor;
        private double _dfbSearchResoluation;

        public OsaSettingData()
        {
            this._lockObj = new object();

            this._isTrigger = false;

            this._traceID = "A";

            this._msAnalysisMode = EMS9740AnalysisMode.DFB_LD;

            this._centerWavelength = 1350.0d;
            this._SpanOfWavelength = 500.0d;
            this._resoluation = 0.1d;
            this._referenceLevel = -30.0d;
            this._logDiv = 10.0d;
            this._vidoeBW = 1.0d;

            this._fpSliceLevel = 3.0d;

            this._msDfbSideMode = EMS9740DfbSideMode.Second_Peak;
            this._dfbSliceLevel = 20.0d;
            this._dfbStdevFactor = 6.07d;
            this._dfbSearchResoluation = 0.1d;

            this._samplingPoints = 501;
        }

        #region >>> Public Property <<<

        public bool IsTrigger
        {
            get { return this._isTrigger; }
            set { lock (this._lockObj) { this._isTrigger = value; } }
        }

        public string TraceID
        {
            get { return this._traceID; }
            set { lock (this._lockObj) { this._traceID = value; } }
        }

        /// <summary>
        /// CenterWavelength
        /// </summary>
        public double CenterWavelength
        {
            get { return this._centerWavelength; }
            set { lock (this._lockObj) { this._centerWavelength = value; } }
        }

        /// <summary>
        /// Span 
        /// </summary>
        public double SpanOfWavelength
        {
            get { return this._SpanOfWavelength; }
            set { lock (this._lockObj) { this._SpanOfWavelength = value; } }
        }

        /// <summary>
        /// Resoluation 
        /// </summary>
        public double Resoluation
        {
            get { return this._resoluation; }
            set { lock (this._lockObj) { this._resoluation = value; } }
        }

        /// <summary>
        /// Resoluation 
        /// </summary>
        public double ReferenceLevel
        {
            get { return this._referenceLevel; }
            set { lock (this._lockObj) { this._referenceLevel = value; } }
        }

        /// <summary>
        /// Resoluation 
        /// </summary>
        public double LogDiv
        {
            get { return this._logDiv; }
            set { lock (this._lockObj) { this._logDiv = value; } }
        }

        /// <summary>
        /// Resoluation 
        /// </summary>
        public double VideoBW
        {
            get { return this._vidoeBW; }
            set { lock (this._lockObj) { this._vidoeBW = value; } }
        }

        ///// <summary>
        ///// AnalysisMode 
        ///// </summary>
        //[XmlIgnore]
        //public EAQ6370DAnalysisMode AnalysisMode
        //{
        //    get { return this._analysisMode; }
        //    set
        //    {
        //        lock (this._lockObj)
        //        {
        //            this._analysisMode = value;
        //        }
        //    }
        //}


        /// <summary>
        /// AnalysisMode 
        /// </summary>
        public EMS9740AnalysisMode MS9740AnalysisMode
        {
            get { return this._msAnalysisMode; }
            set { lock (this._lockObj) { this._msAnalysisMode = value; } }
        }

        public double FpSliceLevel
        {
            get { return this._fpSliceLevel; }
            set { lock (this._lockObj) { this._fpSliceLevel = value; } }
        }

        public EMS9740DfbSideMode MS9740DfbSideMode
        {
            get { return this._msDfbSideMode; }
            set { lock (this._lockObj) { this._msDfbSideMode = value; } }
        }

        public double DfbSliceLevel
        {
            get { return this._dfbSliceLevel; }
            set { lock (this._lockObj) { this._dfbSliceLevel = value; } }
        }

        public double DfbStdevFactor
        {
            get { return this._dfbStdevFactor; }
            set { lock (this._lockObj) { this._dfbStdevFactor = value; } }
        }

        public double DfbSearchResolution
        {
            get { return this._dfbSearchResoluation; }
            set { lock (this._lockObj) { this._dfbSearchResoluation = value; } }
        }

        public uint SamplingPoints
        {
            get { return this._samplingPoints; }
            set { lock (this._lockObj) { this._samplingPoints = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public void Reset()
        {
            this._isTrigger = false;
            this._traceID = "A";
            this._msAnalysisMode = EMS9740AnalysisMode.NONE;

            this._centerWavelength = 0.0d;
            this._SpanOfWavelength = 0.0d;
            this._resoluation = 0.0d;
            this._referenceLevel = 0.0d;
            this._logDiv = 0.0d;
            this._vidoeBW = 0.0d;

            this._fpSliceLevel = 0.0d;

            this._msDfbSideMode = EMS9740DfbSideMode.Left;
            this._dfbSliceLevel = 0.0d;
            this._dfbStdevFactor = 0.0d;
            this._dfbSearchResoluation = 0.0d;

            this._samplingPoints = 1;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
