using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
	public static class LinearRegress
	{
        public static void Calculate(double[] x_MsrValue, double[] y_ExpectValue, out double slope, out double intercept, out double rsqure)
		{
            double Lxx = 0.0d;
            double Lxy = 0.0d;
            double Ly = 0.0d;
            double Lx = 0.0d;
            double Lyy = 0.0d;

            double Syy = 0.0d;
            double Sxx = 0.0d;
            double Sxy = 0.0d;
            double SSR = 0.0d;

			int m = x_MsrValue.Length;

			for (int i = 0; i < m; i++)
			{
				Lx += x_MsrValue[i];	//sum of x

				Ly += y_ExpectValue[i];

				Lxx += Math.Pow(x_MsrValue[i], 2);

				Lxy += x_MsrValue[i] * y_ExpectValue[i];

				Lyy += Math.Pow(y_ExpectValue[i], 2);
			}

            slope = (m * Lxy - Lx * Ly) / (m * Lxx - Math.Pow(Lx, 2));
            intercept = (Lxx * Ly - Lxy * Lx) / (m * Lxx - Math.Pow(Lx, 2));

			Syy = Lyy - (Math.Pow(Ly, 2) / m);
			Sxx = Lxx - (Math.Pow(Lx, 2) / m);
			Sxy = Lxy - ((Lx * Ly) / m);

			SSR = Math.Pow(Sxy, 2) / Sxx;

			rsqure = SSR / Syy;
		}
	}
}
