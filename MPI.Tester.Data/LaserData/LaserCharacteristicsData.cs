using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    public class LaserCharacteristicsData
    {
        private object _lockObj;

        private double _Pop;
        private double _Iop;
        private double _Vop;
        private double _Imop;
        private double _Pceop;

        private double _Ipk;
        private double _Ppk;
        private double _Vpk;
        private double _Impk;
        private double _Pcepk;

        private double _Ith;
        private double _Pth;
        private double _Vth;

        private double _SE;
        private double _SE2;
        private double _Rs;

        private double _kink;
        private double _Ikink;
        private double _Pkink;

        private double _linearity;
        private double _linearity2;
        private double _rollover;

        private double _Icod;
        private double _Pcod;

        private double _PfA;
        private double _VfA;
        private double _PceA;
        private double _RdA;

        private double _PfB;
        private double _VfB;
        private double _PceB;
        private double _Rdb;

        private double _PfC;
        private double _VfC;
        private double _PceC;
        private double _RdC;

        public LaserCharacteristicsData()
        {
            this._lockObj = new object();

            this.Clear();  // Clear All Parameter
        }

        #region >>> Public Property <<<

        /// <summary>
        /// Output Optical Power
        /// </summary>
        public double Pop
        {
            get { return this._Pop; }
            set { lock (this._lockObj) { this._Pop = value; } }
        }

        /// <summary>
        /// Current at Operating Point
        /// </summary>
        public double Iop
        {
            get { return this._Iop; }
            set { lock (this._lockObj) { this._Iop = value; } }
        }

        /// <summary>
        /// Voltage at Operating Point
        /// </summary>
        public double Vop
        {
            get { return this._Vop; }
            set { lock (this._lockObj) { this._Vop = value; } }
        }

        /// <summary>
        /// Measured Photodiode Current at Operating Point
        /// </summary>
        public double Imop
        {
            get { return this._Imop; }
            set { lock (this._lockObj) { this._Imop = value; } }
        }

        /// <summary>
        /// Power Conversion Efficiency at Operating Point
        /// </summary>
        public double Pceop
        {
            get { return this._Pceop; }
            set { lock (this._lockObj) { this._Pceop = value; } }
        }

        /// <summary>
        /// Apply Current at Maximun Output Optical Power
        /// </summary>
        public double Ipk
        {
            get { return this._Ipk; }
            set { lock (this._lockObj) { this._Ipk = value; } }
        }

        /// <summary>
        /// Maximum Output Optical Power
        /// </summary>
        public double Ppk
        {
            get { return this._Ppk; }
            set { lock (this._lockObj) { this._Ppk = value; } }
        }

        /// <summary>
        /// Msasure Voltage at Maximun Output Optical Power
        /// </summary>
        public double Vpk
        {
            get { return this._Vpk; }
            set { lock (this._lockObj) { this._Vpk = value; } }
        }

        /// <summary>
        /// Maximum Measured Photodiode Current
        /// </summary>
        public double Impk
        {
            get { return this._Impk; }
            set { lock (this._lockObj) { this._Impk = value; } }
        }

        /// <summary>
        /// Maximum PCE
        /// </summary>
        public double Pcepk
        {
            get { return this._Pcepk; }
            set { lock (this._lockObj) { this._Pcepk = value; } }
        }

        /// <summary>
        /// Power at Threshold Current 
        /// </summary>
        public double Pth
        {
            get { return this._Pth; }
            set { lock (this._lockObj) { this._Pth = value; } }
        }

        /// <summary>
        /// Threshold Current
        /// </summary>
        public double Ith
        {
            get { return this._Ith; }
            set { lock (this._lockObj) { this._Ith = value; } }
        }

        /// <summary>
        /// Threshold Voltage
        /// </summary>
        public double Vth
        {
            get { return this._Vth; }
            set { lock (this._lockObj) { this._Vth = value; } }
        }

        /// <summary>
        /// Slope Efficiency
        /// </summary>
        public double SE
        {
            get { return this._SE; }
            set { lock (this._lockObj) { this._SE = value; } }
        }

        /// <summary>
        /// Slope Efficiency
        /// </summary>
        public double SE2
        {
            get { return this._SE2; }
            set { lock (this._lockObj) { this._SE2 = value; } }
        }

        /// <summary>
        /// Resistance (Ohm)
        /// </summary>
        public double Rs
        {
            get { return this._Rs; }
            set { lock (this._lockObj) { this._Rs = value; } }
        }

        /// <summary>
        /// Kink Ratio (%)
        /// </summary>
        public double Kink
        {
            get { return this._kink; }
            set { lock (this._lockObj) { this._kink = value; } }
        }

        /// <summary>
        /// Kink Current
        /// </summary>
        public double Ikink
        {
            get { return this._Ikink; }
            set { lock (this._lockObj) { this._Ikink = value; } }
        }

        /// <summary>
        /// Kink Power
        /// </summary>
        public double Pkink
        {
            get { return this._Pkink; }
            set { lock (this._lockObj) { this._Pkink = value; } }
        }

        /// <summary>
        /// Linearity, Lin 
        /// </summary>
        public double Linearity
        {
            get { return this._linearity; }
            set { lock (this._lockObj) { this._linearity = value; } }
        }

        /// <summary>
        /// Linearity, Lin2 
        /// </summary>
        public double Linearity2
        {
            get { return this._linearity2; }
            set { lock (this._lockObj) { this._linearity2 = value; } }
        }

        /// <summary>
        /// Rollover
        /// </summary>
        public double Rollover
        {
            get { return this._rollover; }
            set { lock (this._lockObj) { this._rollover = value; } }
        }

        /// <summary>
        /// Icod
        /// </summary>
        public double Icod
        {
            get { return this._Icod; }
            set { lock (this._lockObj) { this._Icod = value; } }
        }

        /// <summary>
        /// Pcod
        /// </summary>
        public double Pcod
        {
            get { return this._Pcod; }
            set { lock (this._lockObj) { this._Pcod = value; } }
        }

        /// <summary>
        /// Power at specific point "A"
        /// </summary>
        public double PfA
        {
            get { return this._PfA; }
            set { lock (this._lockObj) { this._PfA = value; } }
        }

        /// <summary>
        /// Voltage at specific point "A"
        /// </summary>
        public double VfA
        {
            get { return this._VfA; }
            set { lock (this._lockObj) { this._VfA = value; } }
        }

        /// <summary>
        /// Resistance at specific point "A"
        /// </summary>
        public double RdA
        {
            get { return this._RdA; }
            set { lock (this._lockObj) { this._RdA = value; } }
        }

        /// <summary>
        /// Power Conversion Efficiency at specific point "A"
        /// </summary>
        public double PceA
        {
            get { return this._PceA; }
            set { lock (this._lockObj) { this._PceA = value; } }
        }

        /// <summary>
        /// Power at specific point "B"
        /// </summary>
        public double PfB
        {
            get { return this._PfB; }
            set { lock (this._lockObj) { this._PfB = value; } }
        }

        /// <summary>
        /// Voltage at specific point "B"
        /// </summary>
        public double VfB
        {
            get { return this._VfB; }
            set { lock (this._lockObj) { this._VfB = value; } }
        }

        /// <summary>
        /// Resistance at specific point "B"
        /// </summary>
        public double RdB
        {
            get { return this._Rdb; }
            set { lock (this._lockObj) { this._Rdb = value; } }
        }

        /// <summary>
        /// Power Conversion Efficiency at specific point "B"
        /// </summary>
        public double PceB
        {
            get { return this._PceB; }
            set { lock (this._lockObj) { this._PceB = value; } }
        }

        /// <summary>
        /// Power at specific point "C"
        /// </summary>
        public double PfC
        {
            get { return this._PfC; }
            set { lock (this._lockObj) { this._PfC = value; } }
        }

        /// <summary>
        /// Voltage at specific point "C"
        /// </summary>
        public double VfC
        {
            get { return this._VfC; }
            set { lock (this._lockObj) { this._VfC = value; } }
        }

        /// <summary>
        /// Resistance at specific point "C"
        /// </summary>
        public double RdC
        {
            get { return this._RdC; }
            set { lock (this._lockObj) { this._RdC = value; } }
        }

        /// <summary>
        /// Power Conversion Efficiency at specific point "C"
        /// </summary>
        public double PceC
        {
            get { return this._PceC; }
            set { lock (this._lockObj) { this._PceC = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public void Clear()
        {
            this._Pop = 0.0d; 
            this._Iop = 0.0d; 
            this._Vop = 0.0d; 
            this._Imop = 0.0d; 
            this._Pceop = 0.0d; 

            this._Ipk = 0.0d; 
            this._Ppk = 0.0d;
            this._Vpk = 0.0d;
            this._Impk = 0.0d; 

            this._Ith = 0.0d; 
            this._Pth = 0.0d; 
            this._Vth = 0.0d; 

            this._SE = 0.0d;
            this._SE2 = 0.0d; 
            this._Rs = 0.0d; 

            this._kink = 0.0d; 
            this._Ikink = 0.0d; 
            this._Pkink = 0.0d; 

            this._linearity = 0.0d;
            this._linearity2 = 0.0d; 
            this._rollover = 0.0d;

            this._Icod = 0.0d;
            this._Pcod = 0.0d;

            this._PfA = 0.0d; 
            this._VfA = 0.0d; 
            this._PceA = 0.0d; 
            this._RdA = 0.0d; 

            this._PfB = 0.0d; 
            this._VfB = 0.0d; 
            this._PceB = 0.0d; 
            this._Rdb = 0.0d; 

            this._PfC = 0.0d; 
            this._VfC = 0.0d; 
            this._PceC = 0.0d; 
            this._RdC = 0.0d; 
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
