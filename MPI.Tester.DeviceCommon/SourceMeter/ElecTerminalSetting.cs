using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
	[Serializable]
	public class ElecTerminalSetting : ICloneable
	{
		#region >>> Private Property <<<

		private object _lockObj;

		private ESMU _smu;
        private ETerminalName _termName; 
		private EMsrtType _msrtType;
        private ETermianlFuncType _funcType;

        private int _seqOrder;

		private bool _isAutoTurnOff;

		private double _forceDelayTime;
		private double _forceValue;
		private double _forceTime;
		private double _forceRange;
        private string _forceUnit;
		private string _forceTimeUnit;

		private int _msrtFilterCount;
		private double _msrtProtection;
		private double _msrtRange;
		private double _msrtNPLC;
        private string _msrtUnit;
        private bool _msrtAutoRange;

        private double _turnOffTime;

        private ESweepMode _sweepMode;
		private uint _sweepRiseCount;
		private uint _sweepContCount;
		private double _sweepStart;
		private double _sweepStop;
		private double _sweepStep;
		private double _sweepTurnOffTime;
		private List<double> _sweepCustomList;
        private List<double> _sweepLogList;

        private string _description;

		#endregion

		#region >>> Constructor / Disposor <<<

        public ElecTerminalSetting()
		{
			this._lockObj = new object();

			this._smu = ESMU.None;
			this._msrtType = EMsrtType.FVMI;
            this._funcType = ETermianlFuncType.Bias;

            this._seqOrder = 0;

			this._isAutoTurnOff = false;

			this._forceDelayTime = 0.0d;
			this._forceValue = 0.0d;
			this._forceTime = 0.0d;
			this._forceRange = 0.0d;
            this._forceUnit = "V";
            this._forceTimeUnit = "ms";

			this._msrtFilterCount = 0;
			this._msrtProtection = 100.0d;
			this._msrtRange = 100.0d;
			this._msrtNPLC = 0.01d;
            this._msrtUnit = "mA";

            this._sweepMode = ESweepMode.Linear;
			this._sweepRiseCount = 0;
			this._sweepContCount = 1;
			this._sweepStart = 0.0d;
			this._sweepStop = 1.0d;
			this._sweepStep = 1.0d;
			this._sweepTurnOffTime = 0.0d;
            this._sweepCustomList = new List<double>();
            this._sweepLogList = new List<double>();

			this._description = "Bias Voltage";

            this._msrtAutoRange = false;
		}

        public ElecTerminalSetting(ETerminalName name, ESMU smu) : this()
        {
            this._termName = name;
            this._smu = smu;
            this._seqOrder = (int)name;
        }

		#endregion

		#region >>> public Property <<<

		public EMsrtType MsrtType
		{
			get { return this._msrtType; }
			set { lock (this._lockObj) { this._msrtType = value; } }
		}

		public ESMU SMU
		{
			get { return this._smu; }
			set { lock (this._lockObj) { this._smu = value; } }
		}

        public ETerminalName TerminalName
        {
            get { return this._termName; }
            set { lock (this._lockObj) { this._termName = value; } }
        }

        public ETermianlFuncType TermianlFuncType
        {
            get { return this._funcType; }
            set { lock (this._lockObj) { this._funcType = value; } }
        }

        public int SequenceOrder
        {
            get { return this._seqOrder; }
            set { lock (this._lockObj) { this._seqOrder = value; } }
        }

		public bool IsAutoTurnOff
		{
			get { return this._isAutoTurnOff; }
			set { lock (this._lockObj) { this._isAutoTurnOff = value; } }
		}

		public double ForceDelayTime
		{
			get { return this._forceDelayTime; }
			set { lock (this._lockObj) { this._forceDelayTime = value; } }
		}

		public double ForceValue
		{
			get { return this._forceValue; }
			set { lock (this._lockObj) { this._forceValue = value; } }
		}

		public double ForceTime
		{
			get { return this._forceTime; }
			set { lock (this._lockObj) { this._forceTime = value; } }
		}

		public double ForceRange
		{
			get { return this._forceRange; }
			set { lock (this._lockObj) { this._forceRange = value; } }
		}

		public string ForceUnit
		{
			get { return this._forceUnit; }
			set { lock (this._lockObj) { this._forceUnit = value; } }
		}

		public string ForceTimeUnit
		{
			get { return this._forceTimeUnit; }
			set { lock (this._lockObj) { this._forceTimeUnit = value; } }
		}

		public int MsrtFilterCount
		{
			get { return this._msrtFilterCount; }
			set { lock (this._lockObj) { this._msrtFilterCount = value; } }
		}

		public double MsrtProtection
		{
			get { return this._msrtProtection; }
			set { lock (this._lockObj) { this._msrtProtection = value; } }
		}

		public double MsrtRange
		{
			get { return this._msrtRange; }
			set { lock (this._lockObj) { this._msrtRange = value; } }
		}

		public double MsrtNPLC
		{
			get { return this._msrtNPLC; }
			set { lock (this._lockObj) { this._msrtNPLC = value; } }
		}

		public string MsrtUnit
		{
			get { return this._msrtUnit; }
			set { lock (this._lockObj) { this._msrtUnit = value; } }
		}
        public bool MsrtAutoRange
        {
            get { return this._msrtAutoRange; }
            set { lock (this._lockObj) { this._msrtAutoRange = value; } }
        }
        //_msrtAutoRange

        public double TurnOffTime
        {
            get { return this._turnOffTime; }
            set { lock (this._lockObj) { this._turnOffTime = value; } }
        }

        public ESweepMode SweepMode
        {
            get { return this._sweepMode; }
            set { lock (this._lockObj) { this._sweepMode = value; } }
        }

		public List<double> SweepCustomList
		{
			get { return this._sweepCustomList; }
			set { lock (this._lockObj) { this._sweepCustomList = value; } }
		}

        public List<double> SweepLogList
        {
            get { return this._sweepLogList; }
            set { lock (this._lockObj) { this._sweepLogList = value; } }
        }

		public double SweepStart
		{
			get { return this._sweepStart; }
			set { lock (this._lockObj) { this._sweepStart = value; } }
		}

		public double SweepStop
		{
			get { return this._sweepStop; }
			set { lock (this._lockObj) { this._sweepStop = value; } }
		}

		public double SweepStep
		{
			get { return this._sweepStep; }
			set { lock (this._lockObj) { this._sweepStep = value; } }
		}

		public double SweepTurnOffTime
		{
			get { return this._sweepTurnOffTime; }
			set { lock (this._lockObj) { this._sweepTurnOffTime = value; } }
		}

		public uint SweepRiseCount
		{
			get { return this._sweepRiseCount; }
			set { lock (this._lockObj) { this._sweepRiseCount = value; } }
		}

		public uint SweepContCount
		{
			get { return this._sweepContCount; }
			set { lock (this._lockObj) { this._sweepContCount = value; } }
		}

        public string Description
        {
            get { return this._description; }
            set { lock (this._lockObj) { this._description = value; } }
        }

		#endregion

		#region >>> Public Methods <<<

		public object Clone()
		{
            ElecTerminalSetting cloneObj = this.MemberwiseClone() as ElecTerminalSetting;
            
            cloneObj._sweepCustomList = new List<double>();

            foreach (var value in this._sweepCustomList)
            {
                cloneObj._sweepCustomList.Add(value);
            }

            return cloneObj;
		}

		#endregion
	}
}
