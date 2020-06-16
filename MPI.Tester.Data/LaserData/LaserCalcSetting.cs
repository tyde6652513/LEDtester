using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    [Serializable]
    public class LaserCalcSetting
    {
        // PIV, unit: A, V, mW
        private object _lockObj;

        private double _pGain;
        private double _pOffset;
        private double _iGain;
        private double _iOffset;
        private double _vGain;
        private double _vOffset;

        private int _powMovingAvgWindow;
        private int _voltMovingAvgWindow;

        private double _pop;

        private ELaserPointSelectMode _pointSelect;

        private ELaserCalcMode _seCalcMode;

        private ELaserSearchMode _seSearch;
        private double _seSectionLowLimitP;
        private double _seSectionUpperLimitP;
        private double _seSectionLowLimitI;
        private double _seSectionUpperLimitI;

        private ELaserSearchMode _se2Search;
        private double _se2SectionLowLimitP;
        private double _se2SectionUpperLimitP;
        private double _se2SectionLowLimitI;
        private double _se2SectionUpperLimitI;

        private ELaserCalcMode _thresholdCalcMode;
        private ELaserSearchMode _thresholdSearch;
        private double _thresholdSectionLowLimitP;
        private double _thresholdSectionUpperLimitP;
        private double _thresholdSectionLowLimitI;
        private double _thresholdSectionUpperLimitI;
        private double _thresholdSearchValue;
        private double _thresholdSearchValue2;

        private ELaserCalcMode _rsCalcMode;
        private ELaserSearchMode _rsSearch;
        private double _rsSectionLowLimitP;
        private double _rsSectionUpperLimitP;
        private double _rsSectionLowLimitI;
        private double _rsSectionUpperLimitI;

        private ELaserSearchMode _lnSearch;
        private double _lnSectionLowLimitP;
        private double _lnSectionUpperLimitP;
        private double _lnSectionLowLimitI;
        private double _lnSectionUpperLimitI;

        private double _kinkSectionLowLimitI;
        private double _kinkSectionUpperLimitI;
        private double _kinkRatio;

        private double _iroll;

        private ELaserKinkCalcMode _kinkCalcMode;

        private double _ifa;
        private double _ifb;
        private double _ifc;

        public LaserCalcSetting()
        {
            this._lockObj = new object();

            this._pGain = 1.0d;
            this._pOffset = 0.0d;

            this._iGain = 1.0d;
            this._iOffset = 0.0d;

            this._vGain = 1.0d;
            this._vOffset = 0.0d;

            this._pointSelect = ELaserPointSelectMode.Interpolation;

            //------------------------------------------------------------------
            // SE
            this._seCalcMode = ELaserCalcMode.TwoPointsDifference;

            this._seSearch = ELaserSearchMode.byPower;
            this._seSectionLowLimitP = 0.5d;
            this._seSectionUpperLimitP = 1.5d;
            this._seSectionLowLimitI = 0.004d;
            this._seSectionUpperLimitI = 0.008d;

            this._se2Search = ELaserSearchMode.byPower;
            this._se2SectionLowLimitP = 0.5d;
            this._se2SectionUpperLimitP = 1.5d;
            this._se2SectionLowLimitI = 0.004d;
            this._se2SectionUpperLimitI = 0.008d;

            //------------------------------------------------------------------
            // Threshold
            this._thresholdCalcMode = ELaserCalcMode.TwoPointsDifference;
            this._thresholdSearch = ELaserSearchMode.byPower;
            this._thresholdSectionLowLimitP = 0.5d;
            this._thresholdSectionUpperLimitP = 1.5d;
            this._thresholdSectionLowLimitI = 0.004d;
            this._se2SectionUpperLimitI = 0.008d;
            this._thresholdSearchValue = 0.03;
            this._thresholdSearchValue2 = 0.02d;

            //------------------------------------------------------------------
            // Linearity, Ln
            this._lnSearch = ELaserSearchMode.byPower;
            this._lnSectionLowLimitP = 0.5d;
            this._lnSectionUpperLimitP = 1.5d;
            this._lnSectionLowLimitI = 0.004d;
            this._lnSectionUpperLimitI = 0.008d;

            //------------------------------------------------------------------
            // RS
            this._rsCalcMode = ELaserCalcMode.TwoPointsDifference;
            this._rsSearch = ELaserSearchMode.byCurrent;
            this._rsSectionLowLimitP = 0.5d;
            this._rsSectionUpperLimitP = 1.5d;
            this._rsSectionLowLimitI = 0.004d;
            this._rsSectionUpperLimitI = 0.008d;

            //------------------------------------------------------------------
            // Kink
            this._kinkRatio = 0.01d;
            this._kinkSectionLowLimitI = 0.004d;
            this._kinkSectionUpperLimitI = 0.008d;

            //------------------------------------------------------------------

            this._iroll = 0.0d;

            this._kinkCalcMode = ELaserKinkCalcMode.SEk;

            this._ifa = 0.005d;
            this._ifb = 0.010d;
            this._ifc = 0.015d;
        }

        #region >>> Public Property <<<

        /// <summary>
        /// Gain / Offset
        /// </summary> 
        public double GainPower
        {
            get { return this._pGain; }
            set { lock (this._lockObj) { this._pGain = value; } }
        }

        public double OffsetPower
        {
            get { return this._pOffset; }
            set { lock (this._lockObj) { this._pOffset = value; } }
        }

        public double GainCurrent
        {
            get { return this._iGain; }
            set { lock (this._lockObj) { this._iGain = value; } }
        }

        public double OffsetCurrent
        {
            get { return this._iOffset; }
            set { lock (this._lockObj) { this._iOffset = value; } }
        }

        public double GainVoltage
        {
            get { return this._vGain; }
            set { lock (this._lockObj) { this._vGain = value; } }
        }

        public double OffsetVoltage
        {
            get { return this._vOffset; }
            set { lock (this._lockObj) { this._vOffset = value; } }
        }

        /// <summary>
        /// Smoothing
        /// </summary> 
        public int VoltMovingAverageWindow
        {
            get { return this._voltMovingAvgWindow; }
            set { lock (this._lockObj) { this._voltMovingAvgWindow = value; } }
        }

        public int PowMovingAverageWindow
        {
            get { return this._powMovingAvgWindow; }
            set { lock (this._lockObj) { this._powMovingAvgWindow = value; } }
        }

        /// <summary>
        /// Operation Points
        /// </summary>
        public double Pop
        {
            get { return this._pop; }
            set { lock (this._lockObj) { this._pop = value; } }
        }

        public ELaserPointSelectMode OperationPointSelection
        {
            get { return this._pointSelect; }
            set { lock (this._lockObj) { this._pointSelect = value; } }
        }

        /// <summary>
        /// SE
        /// </summary>
        public ELaserCalcMode SeCalcMode
        {
            get { return this._seCalcMode; }
            set { lock (this._lockObj) { this._seCalcMode = value; } }
        }

        public ELaserSearchMode SeSearchMode
        {
            get { return this._seSearch; }
            set { lock (this._lockObj) { this._seSearch = value; } }
        }

        public double SeSectionLowLimitP
        {
            get { return this._seSectionLowLimitP; }
            set { lock (this._lockObj) { this._seSectionLowLimitP = value; } }
        }

        public double SeSectionUpperLimitP
        {
            get { return this._seSectionUpperLimitP; }
            set { lock (this._lockObj) { this._seSectionUpperLimitP = value; } }
        }

        public double SeSectionLowLimitI
        {
            get { return this._seSectionLowLimitI; }
            set { lock (this._lockObj) { this._seSectionLowLimitI = value; } }
        }

        public double SeSectionUpperLimitI
        {
            get { return this._seSectionUpperLimitI; }
            set { lock (this._lockObj) { this._seSectionUpperLimitI = value; } }
        }

        /// <summary>
        /// SE2
        /// </summary>        
        public ELaserSearchMode Se2SearchMode
        {
            get { return this._se2Search; }
            set { lock (this._lockObj) { this._se2Search = value; } }
        }

        public double Se2SectionLowLimitP
        {
            get { return this._se2SectionLowLimitP; }
            set { lock (this._lockObj) { this._se2SectionLowLimitP = value; } }
        }

        public double Se2SectionUpperLimitP
        {
            get { return this._se2SectionUpperLimitP; }
            set { lock (this._lockObj) { this._se2SectionUpperLimitP = value; } }
        }

        public double Se2SectionLowLimitI
        {
            get { return this._se2SectionLowLimitI; }
            set { lock (this._lockObj) { this._se2SectionLowLimitI = value; } }
        }

        public double Se2SectionUpperLimitI
        {
            get { return this._se2SectionUpperLimitI; }
            set { lock (this._lockObj) { this._se2SectionUpperLimitI = value; } }
        }

        /// <summary>
        /// Threshold
        /// </summary>     
        public ELaserCalcMode ThresholdCalcMode
        {
            get { return this._thresholdCalcMode; }
            set { lock (this._lockObj) { this._thresholdCalcMode = value; } }
        }

        public ELaserSearchMode ThresholdSearchMode
        {
            get { return this._thresholdSearch; }
            set { lock (this._lockObj) { this._thresholdSearch = value; } }
        }

        public double ThresholdSectionLowLimitP
        {
            get { return this._thresholdSectionLowLimitP; }
            set { lock (this._lockObj) { this._thresholdSectionLowLimitP = value; } }
        }

        public double ThresholdSectionUpperLimitP
        {
            get { return this._thresholdSectionUpperLimitP; }
            set { lock (this._lockObj) { this._thresholdSectionUpperLimitP = value; } }
        }

        public double ThresholdSectionLowLimitI
        {
            get { return this._thresholdSectionLowLimitI; }
            set { lock (this._lockObj) { this._thresholdSectionLowLimitI = value; } }
        }

        public double ThresholdSectionUpperLimitI
        {
            get { return this._thresholdSectionUpperLimitI; }
            set { lock (this._lockObj) { this._thresholdSectionUpperLimitI = value; } }
        }

        public double ThresholdSearchValue
        {
            get { return this._thresholdSearchValue; }
            set { lock (this._lockObj) { this._thresholdSearchValue = value; } }
        }

        public double ThresholdSearchValue2
        {
            get { return this._thresholdSearchValue2; }
            set { lock (this._lockObj) { this._thresholdSearchValue2 = value; } }
        }

        /// <summary>
        /// RS
        /// </summary>
        public ELaserCalcMode RsCalcMode
        {
            get { return this._rsCalcMode; }
            set { lock (this._lockObj) { this._rsCalcMode = value; } }
        }

        public ELaserSearchMode RsSearchMode
        {
            get { return this._rsSearch; }
            set { lock (this._lockObj) { this._rsSearch = value; } }
        }

        public double RsSectionLowLimitP
        {
            get { return this._rsSectionLowLimitP; }
            set { lock (this._lockObj) { this._rsSectionLowLimitP = value; } }
        }

        public double RsSectionUpperLimitP
        {
            get { return this._rsSectionUpperLimitP; }
            set { lock (this._lockObj) { this._rsSectionUpperLimitP = value; } }
        }

        public double RsSectionLowLimitI
        {
            get { return this._rsSectionLowLimitI; }
            set { lock (this._lockObj) { this._rsSectionLowLimitI = value; } }
        }

        public double RsSectionUpperLimitI
        {
            get { return this._rsSectionUpperLimitI; }
            set { lock (this._lockObj) { this._rsSectionUpperLimitI = value; } }
        }

        /// <summary>
        /// Kink
        /// </summary>
        public ELaserKinkCalcMode KinkCalcMode
        {
            get { return this._kinkCalcMode; }
            set { lock (this._lockObj) { this._kinkCalcMode = value; } }
        }

        public double KinkSectionLowLimitI
        {
            get { return this._kinkSectionLowLimitI; }
            set { lock (this._lockObj) { this._kinkSectionLowLimitI = value; } }
        }

        public double KinkSectionUpperLimitI
        {
            get { return this._kinkSectionUpperLimitI; }
            set { lock (this._lockObj) { this._kinkSectionUpperLimitI = value; } }
        }

        public double KinkRatio
        {
            get { return this._kinkRatio; }
            set { lock (this._lockObj) { this._kinkRatio = value; } }
        }

        /// <summary>
        /// Linearity, Ln
        /// </summary>
        public ELaserSearchMode LnSearchMode
        {
            get { return this._lnSearch; }
            set { lock (this._lockObj) { this._lnSearch = value; } }
        }

        public double LnSectionLowLimitP
        {
            get { return this._lnSectionLowLimitP; }
            set { lock (this._lockObj) { this._lnSectionLowLimitP = value; } }
        }

        public double LnSectionUpperLimitP
        {
            get { return this._lnSectionUpperLimitP; }
            set { lock (this._lockObj) { this._lnSectionUpperLimitP = value; } }
        }

        public double LnSectionLowLimitI
        {
            get { return this._lnSectionLowLimitI; }
            set { lock (this._lockObj) { this._lnSectionLowLimitI = value; } }
        }

        public double LnSectionUpperLimitI
        {
            get { return this._lnSectionUpperLimitI; }
            set { lock (this._lockObj) { this._lnSectionUpperLimitI = value; } }
        }

        /// <summary>
        /// Rollover
        /// </summary>
        public double Iroll
        {
            get { return this._iroll; }
            set { lock (this._lockObj) { this._iroll = value; } }
        }

        /// <summary>
        /// Specific Point
        /// </summary>
        public double IfA
        {
            get { return this._ifa; }
            set { lock (this._lockObj) { this._ifa = value; } }
        }

        public double IfB
        {
            get { return this._ifb; }
            set { lock (this._lockObj) { this._ifb = value; } }
        }

        public double IfC
        {
            get { return this._ifc; }
            set { lock (this._lockObj) { this._ifc = value; } }
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
