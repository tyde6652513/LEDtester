using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
	//---------------------------------------------------------------------------------------------------
	// This better way of computing variance goes back to a 1962 paper by B. P. Welford 
	// and is presented in Donald Knuth's Art of Computer Programming, Vol 2, page 232, 3rd edition.
	//---------------------------------------------------------------------------------------------------
	public class Statistic
	{
		private object _lockObj;
		private int _count;
		private double _oldMean;
		private double _newMean;
		private double _oldSTDEV;
		private double _newSTDEV;
		private double _maxValue;
		private double _minValue;
		private double _sum;

		public Statistic()
		{
			this._lockObj = new object();

			this._count = 0;
		}


		#region >>> Public Property <<<
			
		public int Count
		{
			get { return this._count; }
		}

		public double Mean
		{
			get { return  ( ( this._count > 0) ? this._newMean : 0.0d ); }
		}

		public double Variance
		{
			get { return ((this._count > 1) ? ( this._newSTDEV / (double) ( this._count - 1)) : 0.0d); }
		}

		public double STDEV
		{
			get { return  Math.Sqrt( this.Variance); }
		}

		public double Max
		{
			get { return this._maxValue; }
		}

		public double Min
		{
			get { return this._minValue; }
		}

		public double Sum
		{
			get { return this._sum; }
		}

		public double Range
		{
			get { return (this._maxValue - this._minValue ); }
		}

		#endregion

		#region >>> Public Method <<<

		public static double CalcMean( double[] valueArray )
		{
			if (valueArray.Length == 0)
				return 0.0d;

			List<double> data = new List<double>(valueArray);
			return data.Average();
		}

		public static double CalcMax(double[] valueArray)
		{
			if (valueArray.Length == 0)
				return 0.0d;

			List<double> data = new List<double>(valueArray);
			return data.Max();
		}

		public static double CalcMin(double[] valueArray)
		{
			if (valueArray.Length == 0)
				return 0.0d;

			List<double> data = new List<double>(valueArray);
			return data.Min();
		}

		public void Clear()
		{
			this._count = 0;
			this._sum = 0.0d;
		}

		public void Push(double value)
		{
			this._count++;

			// See Knuth TAOCP vol 2, 3rd edition, page 232     
			if ( this._count == 1 )
			{
				this._oldMean = value;
				this._newMean = value;
				this._oldSTDEV = 0.0;

				this._minValue = value;
				this._maxValue = value;

				this._sum = value;
			}
			else
			{
				this._newMean = this._oldMean + ( value - this._oldMean ) / this._count;
				this._newSTDEV = this._oldSTDEV + ( value - this._oldMean ) * ( value - this._newMean );
				
				//-------------------------------------
				// set up for next iteration 
				//-------------------------------------
				this._oldMean = this._newMean;
				this._oldSTDEV = this._newSTDEV;           
				
				if ( value > this._maxValue )
				{
					this._maxValue  = value;
				}
					
				if ( value < this._minValue )
				{
					this._minValue = value;
				}    

				this._sum += value;
			}
		}

		public void PushAll(double[] valueArray)
		{
			if (valueArray == null)
				return;
			this.Clear();

			for (int i = 0; i < valueArray.Length; i++)
			{
				this.Push(valueArray[i]);
			}
		}

		#endregion
	}
}
