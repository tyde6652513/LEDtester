using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MPI.Tester.Data;

namespace MPI.Tester.Tools
{
    public class DailyResultData
    {
        private string _keyName = string.Empty;
        private string _name = "";
        private double[] _data;
        private Boolean[] _isOutSpec;
        private double _everyDieHighSpec = 0;
        private double _everyDieLowSpec = 0;
        private int _overSpecCounts = 0;
        private double _max = 0;
        private double _avg = 0;
        private double _min = 0;
        private double _std = 0;
        private object _lockObj = new object();
        private EGainOffsetType _type = EGainOffsetType.Offset;
        private bool _isCaculate = false;
        private bool _isPass = false;
        private bool _isEnable = false;
        private double _avgLowSpec = 0;
        private double _avgHighSpec = 0;
        private string _deltaUnit="";
        private DailyWatchSpec _spec;
        private int[] _state;
        private bool _isEveryDiePass=true;

        public DailyResultData()
        {

        }

        #region >>> Public Method <<<

        public void Caculate(bool isEnable, int calcType)
        {
            this._isPass = false;
            this._isOutSpec = new Boolean[this._data.Length];
            this._state = new int[this._data.Length];
            this._isCaculate = isEnable;

            if (!isEnable || this._data.Length == 0)
                return;

            this._max = MPI.Maths.Statistic.Max(this._data);
            this._min = MPI.Maths.Statistic.Min(this._data);
            this._avg = MPI.Maths.Statistic.Average(this._data);
            this._std = MPI.Maths.Statistic.StandardDeviation(this._data);

            double autoTuneHigh = _spec.DailyWatchSpec;
            double autoTuneLow = (-1.0d)*_spec.DailyWatchSpec;
          

            //===============================
            //Check Average data in Spec
            //===============================
            switch (calcType)
            {
                case 0:
                    this._isPass = true;
                    this._deltaUnit = "";
                    break;

                case 1:
                    this._type = EGainOffsetType.Offset;
                    this._deltaUnit = "";

                    if (this._avg > this._avgHighSpec || this._avg < this._avgLowSpec)
                    {
                        this._isPass = false;
                    }
                    else
                    {
                        this._isPass = true;
                    }
                    break;

                case 2:
                    this._type = EGainOffsetType.Gain;  // Gain Percent
                    this._deltaUnit = "%";
                    if (this._avg > this._avgHighSpec || this._avg < this._avgLowSpec)
                    {
                        this._isPass = false;
                    }
                    else
                    {
                        this._isPass = true;
                    }
                    break;
            }
            //===============================
            // Check Every Die if in Spec
            //===============================
            this._overSpecCounts = 0;
            double high = this._everyDieHighSpec; ;
            double low = this._everyDieLowSpec;
 
            //if (this._type == EGainOffsetType.Gain) // Offset
            //{
            //    //high = 1 + (this._everyDieHighSpec * 0.01);
            //    //low = 1 + (this._everyDieLowSpec * 0.01);
            //}

            for (int i = 0; i < _data.Length; i++)
            {
                double value = Math.Round(this._data[i], 4);

                if (value > high || value < low)
                {
                    this._overSpecCounts++; // accumulate over Spec 
                    this._isOutSpec[i] = true;
                    this._state[i] = (int)EDailyCheckState.FAIL;
                }
                else
                {
                    this._state[i] = (int)EDailyCheckState.PASS;
                }
            }
        }

        #endregion

        #region >>> Public Property <<<

        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }

        public Boolean[] IsOutSpec
        {
            get { return this._isOutSpec; }
        }

        public double[] DataArray
        {
            get { return this._data; }
            set { lock (this._lockObj) { this._data = value; } }
        }

        public double EverDieHighSpec
        {
            get { return this._everyDieHighSpec; }
            set { lock (this._lockObj) { this._everyDieHighSpec = value; } }
        }
        public double EverDieLowerSpec
        {
            get { return this._everyDieLowSpec; }
            set { lock (this._lockObj) { this._everyDieLowSpec = value; } }
        }

        public int OverSpecCounts
        {
            get { return this._overSpecCounts; }
        }
        public double Max
        {
            get { return this._max; }
        }
        public double Min
        {
            get { return this._min; }
        }
        public double Avg
        {
            get { return this._avg; }
        }

        public int DataCount
        {
            get { return this._data.Length; }
        }

        public bool IsRunCalculate
        {
            get { return this._isCaculate; }
            set { lock (this._lockObj) { this._isCaculate = value; } }
        }

        public bool IsPASS
        {
            get
            {
                return this._isPass;
            }
        }

        public EGainOffsetType Type
        {
            get { return this._type; }
        }

        public double AvgLowSpec
        {
            get { return this._avgLowSpec; }
            set { lock (this._lockObj) { this._avgLowSpec = value; } }
        }

        public double AvgHighSpec
        {
            get { return this._avgHighSpec; }
            set { lock (this._lockObj) { this._avgHighSpec = value; } }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public string DeltaUnit
        {
            get { return this._deltaUnit; }
            set { lock (this._lockObj) { this._deltaUnit = value; } }
        }

        public DailyWatchSpec Spec
        {
            get { return this._spec; }
            set { lock (this._lockObj) { this._spec = value; } }
        }

        public bool IsEveryDiePass
        {
            get { return this._isEveryDiePass; }
            set { lock (this._lockObj) { this._isEveryDiePass = value; } }
        }

        #endregion

    }
}
