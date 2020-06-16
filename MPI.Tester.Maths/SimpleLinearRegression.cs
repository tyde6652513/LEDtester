using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
	//---------------------------------------------------------------------------------------------------------------------
	//
	// In statistics, simple linear regression is the least squares estimator of a linear regression model with 
	// a single predictor variable. In other words, simple linear regression fits a straight line through the set
	// of n points in such a way that makes the sum of squared residuals of the model (that is, vertical distances
	// between the points of the data set and the fitted line) as small as possible.
	//		
	// ~ by http://en.wikipedia.org/wiki/Simple_linear_regression
	//
	// yi = m * xi + b , only one independent variable
	//
	//---------------------------------------------------------------------------------------------------------------------
	public class SimpleLinearRegression
	{
		private object _lockObj;

		private double _mSlop;
		private double _bIntercept;
		private double _corrCoef;		// Correlation coefficiet
		private double _Rsquare;		// Coefficient of determination	
		private double _SSregress;		// ( SSR ) the sum of squares of regression, also called the explained sum of squares.
		private double _SSerror;		// ( SSE )  the sum of squares of residuals, also called the residual sum of squares. ( error )
		private double _SStotal;		// ( SST ) the total sum of squares (proportional to the sample variance);
		private double _xMean;
		private double _yMean;

		public SimpleLinearRegression()
		{
			this._lockObj = new object();

			this.ResetValue();			
		}

		#region >>> Public Property <<<

		public double Slop
		{
			get { return this._mSlop; }
		}

		public double Intercept
		{
			get { return this._bIntercept; }
		}

		public double CorrCoef
		{
			get { return this._corrCoef; }
		}
		
		public double Rsquare
		{
			get { return this._Rsquare; }
		}
		
		public double SSR
		{
			get { return this._SSregress; }
		}

		public double SSE
		{
			get { return this._SSerror; }
		}

		public double SST
		{
			get { return this._SStotal; }
		}

		#endregion

		#region >>> Private Method <<<

		private void ResetValue()
		{
			this._mSlop = 0.0d;
			this._bIntercept = 0.0d;
			this._corrCoef = 0.0d;
			this._Rsquare = 0.0d;
			this._SSregress = 0.0d;
			this._SSerror = 0.0d;
			this._SStotal = this._SSregress + this._SSerror;
			this._xMean = 0.0d;
			this._yMean = 0.0d;
		}

		#endregion

		#region >>> Public Method <<<

		public bool Calculate( double[] xIn, double[] yOut )
		{
			double n = 2.0d;
			double xSum = 0.0d;			// sum of xi
			double ySum = 0.0d;			// sum of yi
			double x2Sum = 0.0d;		// sum of square of xi
			double y2Sum = 0.0d;		// sum of square of yi
			double xySum = 0.0d;		// sum of product xi and yi

			double Sxx = 0.0d;
			double Syy = 0.0d;
			double Sxy = 0.0d;

			this.ResetValue();	
			if (xIn == null || yOut == null || xIn.Length < 2 || yOut.Length < 2 || ( xIn.Length != yOut.Length ) )
				return false;

			n = (double) xIn.Length;
			for ( int i = 0; i < xIn.Length; i++)
			{
				xSum += xIn[i];
				ySum += yOut[i];
				x2Sum += (xIn[i] * xIn[i]);
				y2Sum += (yOut[i] * yOut[i]);
				xySum += (xIn[i] * yOut[i]);
			}

			// n > 2.0d 
			this._xMean = xSum / n;
			this._yMean = ySum / n;

			Sxx = x2Sum - xSum * xSum / n;
			Syy = y2Sum - ySum * ySum / n;
			Sxy = xySum - xSum * ySum / n;

			this._SStotal = Syy;

			// estimate slop and SSR
			if ( Sxx != 0.0d )
			{
				this._mSlop = Sxy / Sxx;
				this._SSregress = Sxy * Sxy / Sxx;
			}
			else if ( Sxy == 0.0d )
			{
				this._mSlop = 0.0d;
				this._SSregress = 0.0d;
			}
			else
			{
				this._mSlop = double.MaxValue;
				this._SSregress = double.MaxValue;
			}

			// estimate intercept and SSE
			this._bIntercept = this._yMean - this._mSlop * this._xMean;			
			this._SSerror = this._SStotal - this._SSregress;

			// coefficient of determination
			if ( this._SStotal != 0.0d )
			{
				this._Rsquare = this._SSregress / this._SStotal;
			}
			else
			{
				this._Rsquare = 1.0d;
			}

			//-----------------------------------------------------------------------------------------------------------
			// Coefficient of determination = square of correlation coefficient,  at simple linear regression. 
			// yi = m * xi + b , only one independent variable
			//-----------------------------------------------------------------------------------------------------------
			// correlation coeff
			this._corrCoef = Math.Sqrt(this._Rsquare);

			return true;		
		}

		public bool GetResiduals( double[] xIn, double[] yOut, out double[] Residuals )
		{
			if ( this.Calculate( xIn , yOut ) == true )
			{
				Residuals = new double[ xIn.Length ];

				for( int i = 0; i< xIn.Length; i++ )
				{
					Residuals[i] = yOut[i] - ( this._mSlop * xIn[i] + this._bIntercept );
				}

				return true;
			}
			else
			{	
				Residuals = null;
				return false;
			}
		}

		#endregion
	}
}
