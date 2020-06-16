using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.TestKernel
{
	public static class SDCMFunc
	{
		private static SmartEllipse GetEllipse(EANSI376 type)
		{
			SmartEllipse ellipse = new SmartEllipse(SmartEllipse.ECIEType.CIE1931);

			switch (type)
			{
				case EANSI376.ANSI_2700:
					{
						ellipse.X = 0.459;

						ellipse.Y = 0.412;

						ellipse.Theta = 53.42;

						ellipse.a = 0.0027;

						ellipse.b = 0.0014;

						break;
					}
				case EANSI376.ANSI_3000:
					{
						ellipse.X = 0.44;

						ellipse.Y = 0.403;

						ellipse.Theta = 53.13;

						ellipse.a = 0.00278;

						ellipse.b = 0.00136;

						break;
					}
				case EANSI376.ANSI_3500:
					{
						ellipse.X = 0.411;

						ellipse.Y = 0.393;

						ellipse.Theta = 54;

						ellipse.a = 0.00309;

						ellipse.b = 0.00138;

						break;
					}
				case EANSI376.ANSI_4000:
					{
						ellipse.X = 0.38;

						ellipse.Y = 0.38;

						ellipse.Theta = 53.43;

						ellipse.a = 0.00313;

						ellipse.b = 0.00134;

						break;
					}
				case EANSI376.ANSI_5000:
					{
						ellipse.X = 0.346;

						ellipse.Y = 0.359;

						ellipse.Theta = 59.37;

						ellipse.a = 0.00274;

						ellipse.b = 0.00118;

						break;
					}
				case EANSI376.ANSI_6500:
					{
						ellipse.X = 0.313;

						ellipse.Y = 0.337;

						ellipse.Theta = 58.34;

						ellipse.a = 0.00223;

						ellipse.b = 0.00095;

						break;
					}
				default:
					break;
			}

			return ellipse;
		}

		private static SmartEllipse GetEllipse(EGB10682 type)
		{
			SmartEllipse ellipse = new SmartEllipse(SmartEllipse.ECIEType.CIE1931);

			switch (type)
			{
				case EGB10682.GB_2700:
					{
						ellipse.X = 0.463;

						ellipse.Y = 0.420;

						ellipse.Theta = 57.17;

						ellipse.a = 0.00258;

						ellipse.b = 0.00137;

						break;
					}
				case EGB10682.GB_3000:
					{
						ellipse.X = 0.44;

						ellipse.Y = 0.403;

						ellipse.Theta = 53.1;

						ellipse.a = 0.00278;

						ellipse.b = 0.00136;

						break;
					}
				case EGB10682.GB_3500:
					{
						ellipse.X = 0.409;

						ellipse.Y = 0.394;

						ellipse.Theta = 52.58;

						ellipse.a = 0.00316;

						ellipse.b = 0.00139;

						break;
					}
				case EGB10682.GB_4000:
					{
						ellipse.X = 0.38;

						ellipse.Y = 0.38;

						ellipse.Theta = 54;

						ellipse.a = 0.00313;

						ellipse.b = 0.00134;

						break;
					}
				case EGB10682.GB_5000:
					{
						ellipse.X = 0.346;

						ellipse.Y = 0.359;

						ellipse.Theta = 59.37;

						ellipse.a = 0.00274;

						ellipse.b = 0.00118;

						break;
					}
				case EGB10682.GB_6500:
					{
						ellipse.X = 0.313;

						ellipse.Y = 0.337;

						ellipse.Theta = 58.23;

						ellipse.a = 0.00223;

						ellipse.b = 0.00095;

						break;
					}
				default:
					break;
			}

			return ellipse;
		}

		private static bool IsInArea(double coordX, double coordY, double a, double b, double theta, double pX, double pY)
		{
			double pointXT, pointYT;

			double thetaT = (theta / 180 - (int)(theta / 180)) * Math.PI;

			// coordinate transformation
			pointXT = pX - coordX;

			pointYT = pY - coordY;

			double tempX = pointXT;

			pointXT = pointXT * Math.Cos(thetaT) + pointYT * Math.Sin(thetaT);

			pointYT = pointYT * Math.Cos(thetaT) - tempX * Math.Sin(thetaT);

			// Judge
			double judgeValue = Math.Pow(pointXT, 2.0) / Math.Pow(a, 2.0) + Math.Pow(pointYT, 2.0) / Math.Pow(b, 2.0);

			if (judgeValue > 1)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public static int ANSI376(double pX, double pY, EANSI376 type)
		{
			int SDCM = 0;

			double a = 0.0d;

			double b = 0.0d;

			bool meet = false;

			SmartEllipse ellipse = SDCMFunc.GetEllipse(type);

			while (meet == false)
			{
				SDCM++;

				a = SDCM * ellipse.a;

				b = SDCM * ellipse.b;

				meet = SDCMFunc.IsInArea(ellipse.X, ellipse.Y, a, b, ellipse.Theta, pX, pY);
			}

			return SDCM;
		}

		public static EANSI376 ANSI376(double pX, double pY, out int SDCM)
		{
			string[] items = Enum.GetNames(typeof(EANSI376));

			int[] sdcms = new int[items.Length];

			for (int i = 0; i < items.Length; i++)
			{
				sdcms[i] = 0;

				double a = 0.0d;

				double b = 0.0d;

				EANSI376 range = (EANSI376)Enum.Parse(typeof(EANSI376), items[i]);

				SmartEllipse ellipse = SDCMFunc.GetEllipse(range);

				bool meet = false;

				while (meet == false)
				{
					sdcms[i]++;

					a = sdcms[i] * ellipse.a;

					b = sdcms[i] * ellipse.b;

					meet = SDCMFunc.IsInArea(ellipse.X, ellipse.Y, a, b, ellipse.Theta, pX, pY);
				}
			}

			SDCM = int.MaxValue;

			EANSI376 type = EANSI376.ANSI_2700;

			for (int i = 0; i < sdcms.Length; i++)
			{
				if (SDCM > sdcms[i])
				{
					SDCM = sdcms[i];

					type = (EANSI376)Enum.Parse(typeof(EANSI376), items[i]);
				}
			}

			return type;
		}

		public static int GB10682(double pX, double pY, EGB10682 type)
		{
			int SDCM = 0;

			double a = 0.0d;

			double b = 0.0d;

			bool meet = false;

			SmartEllipse ellipse = SDCMFunc.GetEllipse(type);

			while (meet == false)
			{
				SDCM++;

				a = SDCM * ellipse.a;

				b = SDCM * ellipse.b;

				meet = SDCMFunc.IsInArea(ellipse.X, ellipse.Y, a, b, ellipse.Theta, pX, pY);
			}

			return SDCM;
		}

		public static EGB10682 GB10682(double pX, double pY, out int SDCM)
		{
			string[] items = Enum.GetNames(typeof(EGB10682));

			int[] sdcms = new int[items.Length];

			for (int i = 0; i < items.Length; i++)
			{
				sdcms[i] = 0;

				double a = 0.0d;

				double b = 0.0d;

				EGB10682 range = (EGB10682)Enum.Parse(typeof(EGB10682), items[i]);

				SmartEllipse ellipse = SDCMFunc.GetEllipse(range);

				bool meet = false;

				while (meet == false)
				{
					sdcms[i]++;

					a = sdcms[i] * ellipse.a;

					b = sdcms[i] * ellipse.b;

					meet = SDCMFunc.IsInArea(ellipse.X, ellipse.Y, a, b, ellipse.Theta, pX, pY);
				}
			}

			SDCM = int.MaxValue;

			EGB10682 type = EGB10682.GB_2700;

			for (int i = 0; i < sdcms.Length; i++)
			{
				if (SDCM > sdcms[i])
				{
					SDCM = sdcms[i];

					type = (EGB10682)Enum.Parse(typeof(EGB10682), items[i]);
				}
			}

			return type;
		}
	}
}
