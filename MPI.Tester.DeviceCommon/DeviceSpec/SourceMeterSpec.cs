using System;
using System.Collections.Generic;
using System.Linq;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class SourceMeterSpec : ICloneable
    {
        private object _lockObj;
        private const double DEFAULT_MAX_FORCE_TIME = 10000.0d;
        private List<double[]> _lstVoltageRange;
        private List<double[]> _lstCurrentRange;
        private double _maxForceTime;

        private double _maxDcCurrentRange;
        private double _maxDcVoltageRange;
        private double _minDcCurrentRange;
        private double _minDcVoltageRange;

        private bool _isAutoForceRange;
        private bool _isAutoMsrtRange;
        private bool _isSupportedNPLC;
        private bool _isSupportedMsrtFilter;

        private bool _isSupportedDualDetectorCH;

        private SmuPulseSpec _pulseSpec;

        private int _ioQty;

        public SourceMeterSpec()
        {
            this._lockObj = new object();
            this._lstVoltageRange = new List<double[]>();
            this._lstCurrentRange = new List<double[]>();
            this._maxForceTime = DEFAULT_MAX_FORCE_TIME; // ms
            this._isAutoForceRange = false;
            this._isAutoMsrtRange = true;
            this._isSupportedNPLC = false;
            this._isSupportedMsrtFilter = false;

            this._isSupportedDualDetectorCH = false;

            this._pulseSpec = new SmuPulseSpec();
            this._ioQty = 1;
        }

        public SourceMeterSpec(double[][] dcCurrentRange, double[][] dcVoltageRange) : this()
        {
            this.UpdateDcRangeToSpec(dcCurrentRange, dcVoltageRange);
        }

        public SourceMeterSpec(double[][] dcCurrentRange, double[][] dcVoltageRange, double[] pulseCurrentRange, double[] PulseVoltageRange, double[] pulseWidth, double[] pulseDuty) : this()
        {
            this.UpdateDcRangeToSpec(dcCurrentRange, dcVoltageRange);

            this.UpdatePulseRangeToSpec(pulseCurrentRange, PulseVoltageRange, pulseWidth, pulseDuty); 
        }        

        #region >>> Public Property <<<

        public double MaxForceTime
        {
            get
            {
                if (this._maxForceTime == 0.0d)
                {
                    return DEFAULT_MAX_FORCE_TIME;
                }

                return this._maxForceTime;
            }
            set { lock (this._lockObj) { this._maxForceTime = value; } }
        }

        public List<double[]> VoltageRange
        {
            get { return this._lstVoltageRange; }
            set { lock (this._lockObj) { this._lstVoltageRange = value; } }
        }

        public List<double[]> CurrentRange
        {
            get { return this._lstCurrentRange; }
            set { lock (this._lockObj) { this._lstCurrentRange = value; } }
        }

        public double MaxCurrentRange
        {
            get { return this._maxDcCurrentRange; }      
        }

        public double MaxVoltageRange   
        {
            get { return this._maxDcVoltageRange; }     
        }

        public double MinCurrentRange   
        {
            get { return this._minDcCurrentRange; } 
        }

        public double MinVoltageRange
        {
            get { return this._minDcVoltageRange; }
        }

        public bool IsAutoForceRange
        {
            get { return this._isAutoForceRange; }
            set { lock (this._lockObj) { this._isAutoForceRange = value; } }
        }

        public bool IsAutoMsrtRange
        {
            get { return this._isAutoMsrtRange; }
            set { lock (this._lockObj) { this._isAutoMsrtRange = value; } }
        }

        public bool IsSupportedNPLC
        {
            get { return this._isSupportedNPLC; }
            set { lock (this._lockObj) { this._isSupportedNPLC = value; } }
        }

        public bool IsSupportedMsrtFilter
        {
            get { return this._isSupportedMsrtFilter; }
            set { lock (this._lockObj) { this._isSupportedMsrtFilter = value; } }
        }

        public bool IsSupportedDualDetectorCH
        {
            get { return this._isSupportedDualDetectorCH; }
            set { lock (this._lockObj) { this._isSupportedDualDetectorCH = value; } }
        }

        public SmuPulseSpec PulseSpec
        {
            get { return this._pulseSpec; }
        }

        public int IOQty
        {
            get { return this._ioQty; }
            set { lock (this._lockObj) { this._ioQty = value; } }
        }

        

        #endregion

        #region >>> Private Method <<<

        private void UpdateDcRangeToSpec(double[][] dcCurrentRange, double[][] dcVoltageRange)
        {
            double maxValue;
            double minValue;

            if (dcCurrentRange != null)
            {
                maxValue = 0.0d;
                minValue = 99999.0d;

                foreach (var pmu in dcCurrentRange)
                {
                    maxValue = Math.Max(maxValue, pmu.Max());
                    minValue = Math.Min(minValue, pmu.Min());

                    this._lstCurrentRange.Add(pmu);
                }

                this._maxDcCurrentRange = maxValue;
                this._minDcCurrentRange = minValue;
            }

            if (dcVoltageRange != null)
            {
                maxValue = 0.0d;
                minValue = 99999.0d;

                foreach (var pmu in dcVoltageRange)
                {
                    maxValue = Math.Max(maxValue, pmu.Max());
                    minValue = Math.Min(minValue, pmu.Min());

                    this._lstVoltageRange.Add(pmu);
                }

                this._maxDcVoltageRange = maxValue;
                this._minDcVoltageRange = minValue;
            }
        }

        private void UpdatePulseRangeToSpec(double[] pulseCurrentRange, double[] PulseVoltageRange, double[] pulseWidth, double[] pulseDuty)
        {
            if (pulseCurrentRange == null || PulseVoltageRange == null || pulseWidth == null || pulseDuty == null)
            {
                return;
            }

            this._pulseSpec.Clear();
            
            if (pulseCurrentRange.Length == PulseVoltageRange.Length && pulseCurrentRange.Length == pulseWidth.Length && pulseCurrentRange.Length == pulseDuty.Length)
            {
                for (int i = 0; i < pulseCurrentRange.Length; i++)
                {
                    this._pulseSpec.Add(pulseCurrentRange[i], PulseVoltageRange[i], pulseWidth[i], pulseDuty[i]);
                }
            }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            SourceMeterSpec cloneObj = this.MemberwiseClone() as SourceMeterSpec;

            cloneObj._lstVoltageRange = new List<double[]>();

            cloneObj._lstCurrentRange = new List<double[]>();

            foreach (var data in this._lstVoltageRange)
            {
                double[] vRange = data.Clone() as double[];

                cloneObj._lstVoltageRange.Add(vRange);
            }

            foreach (var data in this._lstCurrentRange)
            {
                double[] iRange = data.Clone() as double[];

                cloneObj._lstCurrentRange.Add(iRange);
            }

            //-----------------------------------------------------------
            // pulse mode
            cloneObj._pulseSpec = new SmuPulseSpec();

            cloneObj._pulseSpec = this._pulseSpec.Clone() as SmuPulseSpec;

            return cloneObj;
        }

        #endregion
    }

    [Serializable]
    public class SmuPulseSpec : ICloneable
    {
        private List<PulseRegion> _region;
        
        public SmuPulseSpec()
        {
            this._region = new List<PulseRegion>();
        }

        #region >>> Public Property <<<

        public List<PulseRegion> Region
        {
            get { return this._region; }
        }

        public int Count
        {
            get { return this._region.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(double pulseCurrentRange, double PulseVoltageRange, double pulseWidth, double pulseDuty)
        {
            PulseRegion region = new PulseRegion(pulseCurrentRange, PulseVoltageRange, pulseWidth, pulseDuty);

            this._region.Add(region);
        }

        public void Clear()
        {
            this._region.Clear();
        }

        public object Clone()
        {
            SmuPulseSpec cloneObj = this.MemberwiseClone() as SmuPulseSpec;

            cloneObj._region = new List<PulseRegion>();

            foreach (var data in this._region)
            {
                cloneObj._region.Add(data.Clone() as PulseRegion);
            }

            return cloneObj;
        }

        #endregion
    }

    [Serializable]
    public class PulseRegion : ICloneable
    {
        private double _pulseCurrentRange; // uint: A
        private double _pulseVoltageRange; // unit: V
        private double _pulseWidth;  // unit: ms
        private double _duty; // unit: %

        public PulseRegion()
        {
            this._pulseCurrentRange = 10.0d;
            this._pulseVoltageRange = 5.0d;
            this._pulseWidth = 1.0d;
            this._duty = 100.0d;
        }

        public PulseRegion(double pulseCurrentRange, double PulseVoltageRange, double pulseWidth, double pulseDuty) : this()
        {
            this._pulseCurrentRange = pulseCurrentRange;
            this._pulseVoltageRange = PulseVoltageRange;
            this._pulseWidth = pulseWidth;
            this._duty = pulseDuty;
        }

        #region >>> Public Property <<<

        public double PulseCurrentRange
        {
            set { this._pulseCurrentRange = value; }
            get { return this._pulseCurrentRange; }
        }

        public double PulseVoltageRange
        {
            set { this._pulseVoltageRange = value; }
            get { return this._pulseVoltageRange; }
        }

        public double PulseWidth
        {
            set { this._pulseWidth = value; }
            get { return this._pulseWidth; }
        }

        public double PulseDuty
        {
            set { this._duty = value; }
            get { return this._duty; }
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
