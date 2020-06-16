using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class DmmSettingData : ICloneable
    {
        private object _lockObj;

        private EDmmMeasureFunc _msrtFunc;

        private EDmmDcIntegrationUnit _msrtIntegrationUnit;

        private double _msrtRange;

        private double _msrtNplc;

        private double _msrtApertureTime;

        private double _trigInputDelay;

        private double _trigOutputDelay;

        private uint _trigCount;

        private EDmmDioTriggerOut _eTriggerOutMode;
        
        public DmmSettingData()
        {
            this._lockObj = new object();

            this._msrtFunc = EDmmMeasureFunc.DC_VOLTAGE;

            this._msrtIntegrationUnit = EDmmDcIntegrationUnit.Aperture;

            this._msrtRange = 10.0d;

            this._msrtNplc = 0.01;

            this._msrtApertureTime = 0.0d;

            this._trigInputDelay = 0.0d;

            this._trigOutputDelay = 0.0d;

            this._trigCount = 1;

            this._eTriggerOutMode = EDmmDioTriggerOut.NONE;
        }

        #region >>> Public Property <<<

        /// <summary>
        /// Measure Function, Define by  EDmmMeasureFunc
        /// </summary>
        public EDmmMeasureFunc MeasureFunction
        {
            get { return this._msrtFunc; }
            set { lock (this._lockObj) { this._msrtFunc = value; } }
        }

        /// <summary>
        /// Measure Function, Define by  EDmmMeasureFunc
        /// </summary>
        public EDmmDcIntegrationUnit MeasureIntegrationUnit
        {
            get { return this._msrtIntegrationUnit; }
            set { lock (this._lockObj) { this._msrtIntegrationUnit = value; } }
        }

        /// <summary>
        /// Measure Range
        /// </summary>
        public double MeasureRange
        {
            get { return this._msrtRange; }
            set { lock (this._lockObj) { this._msrtRange = value; } }
        }

        /// <summary>
        /// Measure NPLC, reflect by Measure ApertureTime
        /// </summary>
        public double MeasureNPLC
        {
            get { return this._msrtNplc; }
            set { lock (this._lockObj) { this._msrtNplc = value; } }
        }

        /// <summary>
        /// Measure Aperture Time, reflect by NPLC
        /// DC mode, min. sample time is 8.33us
        /// Digitize mode, min. sample time is 1us
        /// </summary>
        public double MeasureApertureTime
        {
            get { return this._msrtApertureTime; }
            set 
            { 
                lock (this._lockObj) 
                {
                    double aperture = value;

                    aperture = Math.Round(aperture, 6, MidpointRounding.AwayFromZero);

                    this._msrtApertureTime = aperture >= 1e-6? aperture : 1e-6; 
                } 
            }
        }

        /// <summary>
        /// Trigger Intput Delay,  Ext.TriggerIN -> Delay -> DMM Action
        /// </summary>
        public double TriggerInputDelay
        {
            get { return this._trigInputDelay; }
            set { lock (this._lockObj) { this._trigInputDelay = value; } }
        }

        /// <summary>
        /// Trigger Output Delay,  DMM TriggerOut -> Delay -> Ext.Action
        /// </summary>
        public double TriggerOutDelay
        {
            get { return this._trigOutputDelay; }
            set { lock (this._lockObj) { this._trigOutputDelay = value; } }
        }

        /// <summary>
        /// Trigger Count
        /// </summary>
        public uint TriggerCount
        {
            get { return this._trigCount; }
            set { lock (this._lockObj) { this._trigCount = value; } }
        }

        /// <summary>
        /// Trigger Out Mode, NONE | PIN1_FFP | PIN2_NFP
        /// </summary>
        public EDmmDioTriggerOut TriggerOutMode
        {
            get { return this._eTriggerOutMode; }
            set { lock (this._lockObj) { this._eTriggerOutMode = value; } }
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
