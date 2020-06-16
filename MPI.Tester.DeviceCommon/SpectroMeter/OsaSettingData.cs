using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public class OsaSettingData
    {    
        private object _lockObj;

        private double _centerWavelength;

        private double _SpanOfWavelength;

        private double _resoluation;

        private double _referenceLevel;

        private double _logDiv;

        private EAQ6370DAnalysisMode _analysisMode = 0;

        private double _vidoeBW;

        public OsaSettingData()
        {
            this._lockObj = new object();

            this._centerWavelength = 1050;

            this._SpanOfWavelength = 50;

            this._resoluation = 0.1;

            _referenceLevel = -30;

            _logDiv = 10;

            _analysisMode = EAQ6370DAnalysisMode.SWTHresh;

            _vidoeBW = 1;
        }

        #region >>> Public Property <<<

        /// <summary>
        /// CenterWavelength
        /// </summary>
        public double CenterWavelength
        {
            get { return this._centerWavelength; }
            set
            {
                lock (this._lockObj)
                {
                    this._centerWavelength = value;
                }
            }
        }

        /// <summary>
        /// Span 
        /// </summary>
        public double SpanOfWavelength
        {
            get { return this._SpanOfWavelength; }
            set
            {
                lock (this._lockObj)
                {
                    this._SpanOfWavelength = value;
                }
            }
        }

        /// <summary>
        /// Resoluation 
        /// </summary>
        public double Resoluation
        {
            get { return this._resoluation; }
            set
            {
                lock (this._lockObj)
                {
                    this._resoluation = value;
                }
            }
        }

        /// <summary>
        /// Resoluation 
        /// </summary>
        public double ReferenceLevel
        {
            get { return this._referenceLevel; }
            set
            {
                lock (this._lockObj)
                {
                    this._referenceLevel = value;
                }
            }
        }

        /// <summary>
        /// Resoluation 
        /// </summary>
        public double LogDiv
        {
            get { return this._logDiv; }
            set
            {
                lock (this._lockObj)
                {
                    this._logDiv = value;
                }
            }
        }

        /// <summary>
        /// Resoluation 
        /// </summary>
        public double VideoBW
        {
            get { return this._vidoeBW; }
            set
            {
                lock (this._lockObj)
                {
                    this._vidoeBW = value;
                }
            }
        }


        /// <summary>
        /// AnalysisMode 
        /// </summary>
        public EAQ6370DAnalysisMode AnalysisMode
        {
            get { return this._analysisMode; }
            set
            {
                lock (this._lockObj)
                {
                    this._analysisMode = value;
                }
            }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
