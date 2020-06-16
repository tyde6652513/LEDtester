using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.Tools
{
    public class DailyGainOffset : CalcGainOffset
    {
        private EDailyWacthState _state;
        private double _dailyWatchSpec;
        private double _reCalibSpec;
        private bool _isAuoCalibDailyData;
        private bool _isEnable;
        private double _daltaMax = 0;
        private double _daltaAvg = 0;
        private double _daltaMin = 0;
        private double _everDieLowSpec=0;
        private double _everDieHighSpec = 0;
        private double[] _delta;

        public DailyGainOffset()
        {
            this._lockObj = new object();
            this._xIn = null;
            this._yOut = null;
            this._state = EDailyWacthState.PASS;
            this._isAuoCalibDailyData = false;
            this._isEnable = false;
            this._delta = null;
        }

        public DailyGainOffset(string keyName, string name)
            : this()
        {
            this._keyName = keyName;
            this._name = name;
        }

        public DailyGainOffset(string keyName, string name, double[] xIn, double[] yOut, double dailyWacthSpec, double reCalibSpec)
            : this(keyName, name)
        {
            this._xIn = xIn;
            this._yOut = yOut;
            this._dailyWatchSpec = dailyWacthSpec;
            this._reCalibSpec = reCalibSpec;
        }

        public DailyGainOffset(string keyName, string name, double[] xIn, double[] yOut, double dailyWacthSpec, double reCalibSpec,double everDieLowSpec,double everDieHighSpec)
            : this(keyName, name)
        {
            this._xIn = xIn;
            this._yOut = yOut;
            this._dailyWatchSpec = dailyWacthSpec;
            this._reCalibSpec = reCalibSpec;
            this._everDieLowSpec = everDieLowSpec;
            this._everDieHighSpec = everDieHighSpec;
        }

        #region >>> Public Property <<<

        public EDailyWacthState EState
        {
            get { return this._state; }
            set { lock (this._lockObj) { this._state = value; } }
        }

        public double[] XArray
        {
            get { return this._xIn; }
            set { lock (this._lockObj) { this._xIn = value; } }
        }
        public double[] YArray
        {
            get { return this._yOut; }
            set { lock (this._lockObj) { this._yOut = value; } }
        }

        public double[] DeltaArray
        {
            get { return this._delta; }
            set { lock (this._lockObj) { this._delta = value; } }
        }

        public bool ISAutoCalibData
        {
            get { return this._isAuoCalibDailyData; }
            set { lock (this._lockObj) { this._isAuoCalibDailyData = value; } }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public double DeltaMax
        {
            get { return this._daltaMax; }
            set { lock (this._lockObj) { this._daltaMax = value; } }
        }

        public double DeltaMin
        {
            get { return this._daltaMin; }
            set { lock (this._lockObj) { this._daltaMin = value; } }
        }
        public double DeltaAvg
        {
            get { return this._daltaAvg; }
            set { lock (this._lockObj) { this._daltaAvg = value; } }
        }

        public double EverDieHighSpec
        {
            get { return this._everDieHighSpec; }
            set { lock (this._lockObj) { this._everDieHighSpec = value; } }
        }
        public double EverDieLowSpec
        {
            get { return this._everDieLowSpec; }
            set { lock (this._lockObj) { this._everDieLowSpec = value; } }
        }


        #endregion

        #region >>> Public Method <<<

        public void RunCalcAndGetState(EGainOffsetType gainOffsetType)
        {
            this.RunCalcAndGetState(this._xIn, this._yOut, gainOffsetType);
        }
        #endregion

        #region >>> Private Method <<<

        private void RunCalcAndGetState(double[] xIn, double[] yOut, EGainOffsetType type)
        {
            //this.CalcType = type;

            if (xIn == null || yOut == null)
                return;

            this._xIn = xIn;
            this._yOut = yOut;

            this._delta = new double[xIn.Length];

            for (int i = 0; i < xIn.Length; i++)
            {
                this._delta[i] = yOut[i] - xIn[i];
            }

            if (this._delta == null || this._delta.Length == 0)
                return;

            // Value Range

            this._daltaAvg = MPI.Maths.Statistic.Average(this._delta);
            this._daltaMin = MPI.Maths.Statistic.Min(this._delta);
            this._daltaMax = MPI.Maths.Statistic.Max(this._delta);

            switch (CalcType)
            {
                case EGainOffsetType.None:

                    this._offset = Math.Round(MPI.Maths.Statistic.Average(this._delta), 3);
                    this._gain = 1.0d;
                    this._square = 0.0d;

                    if (this._keyName.IndexOf("_") >= 0)
                    {
                        this._isAuoCalibDailyData = true;

                        if (this._keyName.Remove(this._keyName.IndexOf("_")) == "LOP" ||
                                this._keyName.Remove(this._keyName.IndexOf("_")) == "WATT" ||
                                this._keyName.Remove(this._keyName.IndexOf("_")) == "LM")
                        {
                            for (int i = 0; i < xIn.Length; i++)
                            {
                                if (yOut[i] == 0.0d)
                                {
                                    this._delta[i] = 0.0d;
                                }
                                else if (xIn[i] == 0.0d)
                                {
                                    this._delta[i] = double.MaxValue;
                                }
                                else
                                {
                                    this._delta[i] = yOut[i] / xIn[i];
                                }
                            }

                            if (Math.Round(MPI.Maths.Statistic.Average(this._delta), 3) == 0.0d)
                            {
                                this._offset = 0.0d;
                                this._gain = 1.0d;
                                this._square = 0.0d;
                            }

                            else
                            {
                                this._offset = 0.0d;
                                this._gain = Math.Round(MPI.Maths.Statistic.Average(this._delta), 3);
                                this._square = 0.0d;
                            }

                            if (this._gain == double.MaxValue)
                            {
                                this._gain = 999.0d;
                            }

                            if (Math.Abs(this._gain - 1) > (0.01 * this._reCalibSpec))
                            {
                                this._state = EDailyWacthState.ReCalibration;
                            }
                            else if (Math.Abs(this._gain - 1) > (0.01 * this._dailyWatchSpec) && Math.Abs(this._gain - 1) < (0.01 * this._reCalibSpec))
                            {
                                this._state = EDailyWacthState.OverDailyWacth;
                            }
                            else
                            {
                                this._state = EDailyWacthState.PASS;
                            }
                        }
                        else
                        {
                            if (Math.Abs(this._offset) > (this._reCalibSpec))
                            {
                                this._state = EDailyWacthState.ReCalibration;
                            }
                            else if (Math.Abs(this._offset) > (this._dailyWatchSpec) && Math.Abs(this._offset) < (this._reCalibSpec))
                            {
                                this._state = EDailyWacthState.OverDailyWacth;
                            }
                            else
                            {
                                this._state = EDailyWacthState.PASS;
                            }

                        }
           
                 }


                    break;

                case EGainOffsetType.Gain:

                    for (int i = 0; i < xIn.Length; i++)
                    {
                        if (yOut[i] == 0.0d)
                        {
                            this._delta[i] = 0.0d;
                        }
                        else if (xIn[i] == 0.0d)
                        {
                            this._delta[i] = double.MaxValue;
                        }
                        else
                        {
                            this._delta[i] = yOut[i] / xIn[i];
                        }
                    }

                    if (Math.Round(MPI.Maths.Statistic.Average(this._delta), 3) == 0.0d)
                    {
                        this._offset = 0.0d;
                        this._gain = 1.0d;
                        this._square = 0.0d;
                    }
                    else
                    {
                        this._offset = 0.0d;
                        this._gain = Math.Round(MPI.Maths.Statistic.Average(this._delta), 3);
                        this._square = 0.0d;
                    }

                    break;

                case EGainOffsetType.Offset:

                    this._offset = Math.Round(MPI.Maths.Statistic.Average(this._delta), 3);
                    this._gain = 1.0d;
                    this._square = 0.0d;
                    break;

                case EGainOffsetType.GainAndOffest:

                    this._offset = Math.Round(MPI.Maths.Statistic.Average(this._delta), 3);
                    this._gain = 1.0d;
                    this._square = 0.0d;

                    if (this._keyName.IndexOf("_") >= 0)
                    {
                        if (this._keyName.Remove(this._keyName.IndexOf("_")) == "LOP" ||
                                this._keyName.Remove(this._keyName.IndexOf("_")) == "WATT" ||
                                this._keyName.Remove(this._keyName.IndexOf("_")) == "LM")
                        {
                            double rSqure = 0;
                            Maths.LinearRegress.Calculate(xIn, yOut, out this._gain, out this._offset, out rSqure);
                            this._square = 0.0d;
                        }
                    }

                    break;

            }
        }

        #endregion


    }
}
