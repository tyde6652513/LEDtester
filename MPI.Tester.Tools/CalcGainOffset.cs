using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MPI.Tester.Data;
using MPI.Tester.Maths;

namespace MPI.Tester.Tools
{
	//public enum EGainOffsetType : int
	//{
	//    None = 0,
	//    Gain = 1,
	//    Offset = 2,
	//    GainAndOffest = 3,
	//    Square = 4,
	//}

	public class CalcGainOffset
	{
        protected object _lockObj;

        protected string _keyName;
        protected string _name;

        protected double _square;
        protected double _gain;
        protected double _offset;
		protected double _r2square;

        protected EGainOffsetType _calcType;
        protected double[] _xIn;
        protected double[] _yOut;

        protected double[] _delta;

		protected string _format;
		protected SimpleLinearRegression _lstRegress;

        protected uint _channel;

		int _extGainDigit;
		int _extOffsetDigit;

		public CalcGainOffset() 
		{
			this._lockObj = new object();

			this._square = 1.0d;
			this._gain = 1.0d;
			this._offset = 0.0d;
			this._r2square = 0.0d;

			this._calcType = EGainOffsetType.None;
			this._xIn = null;
			this._yOut = null;

			this._lstRegress = new SimpleLinearRegression();
			this._format = "0.0";
			this._extGainDigit = 0;
			this._extOffsetDigit = 0;

            this._channel = 0;
		}

		public CalcGainOffset( string keyName, string name) : this()
		{
			this._keyName = keyName;
			this._name = name;
		}

		public CalcGainOffset(string keyName, string name, double[] xIn, double[] yOut) : this ( keyName, name )
		{
			this._xIn = xIn;
			this._yOut = yOut;
		}

		#region >>> Public Property <<<

		public string KeyName
		{
			get{ return this._keyName; }
		}

		public string KeyNameLetter
		{
			get
			{
				if (this._keyName.IndexOf("_") >= 0)
				{
					return this._keyName.Remove(this._keyName.IndexOf("_"));
				}
				else
				{
					return this._keyName;
				}
			}
		}

		public string Name
		{
			get{ return this._name; }
		}

		public int Digit
		{
			get
			{
				int pointPos = this._format.IndexOf('.');

				if (pointPos < 0)
				{
					return 0;
				}
				else
				{
					return (this._format.Length - pointPos - 1);
				}
			}
		}

        public double[] Delta
        {
            get 
            {
                double[] delta = new double[this._xIn.Length];

                for (int i = 0; i < delta.Length; i++)
                {
                    delta[i] = this._yOut[i] - (this._gain*_xIn[i]+this._offset);
                }

                return delta;
            }
        }

        public double[] YCalibration
        {
            get
            {
                double[] data = new double[this._xIn.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = this._gain * _xIn[i] + this._offset;
                }

                return data;
            }
        }

        public double[] Xin
        {
            get { return this._xIn; }
        }

        public double[] Yout
        {
            get { return this._yOut; }
        }

		public string Format
		{
			get { return this._format; }
			set { this._format = value; }
		}

		public double Sqaure
		{
			get{ return this._square; }
		}

		public double Gain
		{
			get{ return this._gain; }
            set { this._gain = value; }
		}

		public double Offset
		{
			get{ return this._offset; }
            set { this._offset = value; }
		}

		public double R2Square
		{
			get { return this._r2square; }
		}

		public EGainOffsetType CalcType
		{
			get{ return this._calcType; }
			set{ lock ( this._lockObj ) { this._calcType = value; } }
		}

		public int ExtGainDigit
		{
			get { return this._extGainDigit; }
			set { lock (this._lockObj) { this._extGainDigit = value; } }
		}

		public int ExtOffsetDigit
		{
			get { return this._extOffsetDigit; }
			set { lock (this._lockObj) { this._extOffsetDigit = value; } }
		}

        public int DataCounts
        {
            get { return this._xIn.Length; }
        }

		#endregion		

		public void RunCalculate(double[] xIn, double[] yOut) 
		{
			this._xIn = xIn;
			this._yOut = yOut;

            this.RunCalculate();
		}

		public void RunCalculate(double[] xIn, double[] yOut, EGainOffsetType calcType)
		{
			this._xIn = xIn;
			this._yOut = yOut;
			this._calcType = calcType;

			this.RunCalculate();
		}

		public void RunCalculate()
		{
			if (this._xIn == null || this._yOut == null || 
				this._xIn.Length == 0 || this._yOut.Length == 0 ||
				this._xIn.Length != this._yOut.Length )
				return;

			double[] delta = new double[this._xIn.Length];
			double[] scale = new double[this._yOut.Length];

			MPI.Tester.Maths.Statistic stsDelta = new Maths.Statistic();
			MPI.Tester.Maths.Statistic stsScale = new Maths.Statistic();
			stsDelta.Clear();
			stsScale.Clear();

			for (int i = 0; i < this._xIn.Length; i++)
			{
				//=========================================================
				// "stsDelta" => calculation for average of DC Offset 
				//=========================================================
				stsDelta.Push(this._yOut[i] - this._xIn[i]);

				//=========================================================
				// "stsScale" => calculation for average of DC Gain // Dc Gain don't concern about zero points
				//=========================================================
				if (this._yOut[i] == 0.0d)
				{
					//stsScale.Push(0.0d);
				}
				else if (this._xIn[i] == 0.0d)
				{
					//stsScale.Push(double.MaxValue);
				}
				else
				{
					stsScale.Push(this._yOut[i] / this._xIn[i]);
				}
			}

			string str = this._keyName;

    		switch ( this._calcType )
			{
				case EGainOffsetType.None:
					this._square = 0.0;
					this._gain = 1.0;
					this._offset = 0.0;
					break;
				//----------------------------------------------------------------------------------
				case EGainOffsetType.Gain:
					this._square = 0.0d;
					this._offset = 0.0d;

					if (Math.Round(stsScale.Mean, this.Digit + this.ExtGainDigit, MidpointRounding.AwayFromZero) == 0.0d)
					{
						this._gain = 1.0d;
					}
					else
					{
                        this._gain = Math.Round(stsScale.Mean, this.Digit + this.ExtGainDigit, MidpointRounding.AwayFromZero);
					}
					break;
				//----------------------------------------------------------------------------------
				case EGainOffsetType.Offset:
					this._square = 0.0d;
					this._gain = 1.0d;
                    this._offset = Math.Round(stsDelta.Mean, this.Digit + this.ExtOffsetDigit, MidpointRounding.AwayFromZero);
					break;
				//----------------------------------------------------------------------------------
				case EGainOffsetType.GainAndOffest:
					//=========================================================
					// "_lstRegress" => calculation for linear regresssion
					//=========================================================
					this._square = 0.0d;
					if (this._lstRegress.Calculate(this._xIn, this._yOut))
					{
                        this._gain = Math.Round(this._lstRegress.Slop, this.Digit + this.ExtGainDigit, MidpointRounding.AwayFromZero);
                        this._offset = Math.Round(this._lstRegress.Intercept, this.Digit + this.ExtOffsetDigit, MidpointRounding.AwayFromZero);
                        this._r2square = Math.Round(this._lstRegress.Rsquare, this.Digit + this.ExtOffsetDigit, MidpointRounding.AwayFromZero);
					}
					else
					{
						this._gain = 1.0d;
						this._offset = 0.0d;
						this._r2square = 0.0d;
					}
					break;
				//----------------------------------------------------------------------------------
				default:
					break;
			}
		}

	}
}
