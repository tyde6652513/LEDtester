using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{

	static class UTest
	{
		[STAThread]
		static void Main()
		{
			double[] x = new double[] {	1.47,		1.5,	1.52,	1.55,	1.57,	
										1.6,		1.63,	1.65,	1.68,	1.7,
										1.73,		1.75,	1.78,	1.8,	1.83,	};

			double[] y = new double[] {	52.21,		53.12,	54.48,	55.84,	57.2,
										58.57,		59.93,	61.29,	63.11,	64.47,
										66.28,		68.1,	69.92,	72.19,	74.46,	};

			// Result
			// slop = 61.272, intercept = -39.062
			// R2 = 0.989196922


			double[] result = null;

			result = MiscFunction.SimpleLinearRegression( x, y );
			Console.WriteLine("Method 1");
			Console.WriteLine("Slop = {0} \t Intecept = {1}", result[0], result[1] );
			Console.WriteLine("Corr = {0} \t R2 = {1}", result[2], result[3]);

			result = MiscFunction.LeastSquaresFitLine2(x, y);
			Console.WriteLine("\nMethod 2");
			Console.WriteLine("Slop = {0} \t Intecept = {1}", result[0], result[1]);
			Console.WriteLine("Corr = {0} \t R2 = {1}", result[2], result[3]);

			SimpleLinearRegression slr = new SimpleLinearRegression();
			slr.Calculate( x, y );
			Console.WriteLine("\nMethod 3");
			Console.WriteLine("Slop = {0} \t Intecept = {1}", slr.Slop, slr.Intercept );
			Console.WriteLine("Corr = {0} \t R2 = {1}", slr.CorrCoef, slr.Rsquare);

			double[] temp = null;

			slr.GetResiduals(x, y , out temp);
			Console.WriteLine("\nMethod 4");
			Console.WriteLine("Slop = {0} \t Intecept = {1}", slr.Slop, slr.Intercept);
			Console.WriteLine("Corr = {0} \t R2 = {1}\n", slr.CorrCoef, slr.Rsquare);

			for( int i = 0; i < x.Length ; i++ )
			{
				Console.WriteLine(" x[{0}] = {1}, \ty[{2}] = {3}, \tResi = {4}",i,x[i],i,y[i],temp[i]);
			}


			double[] x2 = new double[] {	1.47,		1.5,	};
			double[] y2 = new double[] {	52.21,		53.12,	};

			slr.Calculate(x2, y2);
			Console.WriteLine("\n x2, y2 variable ");
			Console.WriteLine("Slop = {0} \t Intecept = {1}", slr.Slop, slr.Intercept);
			Console.WriteLine("SSR = {0} \t SSE = {1} \t SST = {2}", slr.SSR, slr.SSE, slr.SST);
			Console.WriteLine("Corr = {0} \t R2 = {1}", slr.CorrCoef, slr.Rsquare);

			double[] x3 = new double[] { 1.5,	1.5,	1.5,	};
			double[] y3 = new double[] { 52.0,	52.0,	52.0,	};

			slr.Calculate(x3, y3);
			Console.WriteLine("\n x3, y3 variable ");
			Console.WriteLine("Slop = {0} \t Intecept = {1}", slr.Slop, slr.Intercept);
			Console.WriteLine("SSR = {0} \t SSE = {1} \t SST = {2}", slr.SSR, slr.SSE, slr.SST);
			Console.WriteLine("Corr = {0} \t R2 = {1}", slr.CorrCoef, slr.Rsquare);

			double[] xFail = new double[] { 1.5, };
			double[] yFail = new double[] { 52.0, 52.0, 52.0, };

			bool rtn = slr.GetResiduals(xFail, yFail, out temp);
			Console.WriteLine("\n xFail, yFail variable ");
			Console.WriteLine("Slop = {0} \t Intecept = {1}", slr.Slop, slr.Intercept);
			Console.WriteLine("SSR = {0} \t SSE = {1} \t SST = {2}", slr.SSR, slr.SSE, slr.SST);
			Console.WriteLine("Corr = {0} \t R2 = {1}", slr.CorrCoef, slr.Rsquare);
			
			if ( rtn == false )
			{
				Console.WriteLine(" x, y Data is wrong data");	
			}
			else
			{
				for (int i = 0; i < x.Length; i++)
				{
					Console.WriteLine(" x[{0}] = {1}, \ty[{2}] = {3}, \tResi = {4}", i, xFail[i], i, yFail[i], temp[i]);
				}
			}

			Console.Read();

		}
	}
}
