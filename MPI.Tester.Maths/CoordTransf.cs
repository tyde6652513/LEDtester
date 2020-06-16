using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
    public static class CoordTransf
    {
        public static List<double> LogScale(double endValue, uint scaleCount)
        {
            if (endValue <= 0 || scaleCount <= 1)
            {
                return null;
            }

            double step = Math.Log10(endValue) / (scaleCount - 1);

            List<double> logScale = new List<double>();

            for (int i = 0; i < scaleCount; i++)
            {
                logScale.Add(Math.Pow(10, step * i));
            }

            return logScale;
        }

		public static List<double> LogScale(double startValue, double stopValue, uint point)
		{
			// Vi = startValue * Bi
			// stepSize = [log(stopValue) - log(startValue)] / (point -1)
			// Bi = pow10(stepSize)

            List<double> logScaleData = new List<double>();

            if (point < 2)
            {
                logScaleData.Add(startValue);

                return logScaleData;
            }

            if (startValue == stopValue)
            {
                for (int i = 0; i < point; i++)
                {
                    logScaleData.Add(startValue);
                }

                return logScaleData;
            }

            bool isShift = startValue <= 0;

            double shift = 0;

            if (isShift)
            {
                shift = 1 - startValue;

                startValue += shift;

                stopValue += shift;
            }

            double stepSize = (Math.Log10(stopValue) - Math.Log10(startValue)) / (point - 1);

			for (int i = 0; i < point; i++)
			{
				double bi = Math.Pow(10, stepSize * i);

				double vi = startValue * bi;

                logScaleData.Add(vi - shift);
			}

            return logScaleData;
		}

        public static List<double> LinearScale(double startValue, double stopValue, uint point)
        {
            List<double> linearScaleData = new List<double>();

            if (point < 2)
            {
                linearScaleData.Add(startValue);

                return linearScaleData;
            }

            if (startValue == stopValue)
            {
                for (int i = 0; i < point; i++)
                {
                    linearScaleData.Add(startValue);
                }

                return linearScaleData;
            }

            double stepValue = (stopValue - startValue) / ((int)point - 1);

            for (int i = 0; i < point; i++)
            {
                linearScaleData.Add(startValue + stepValue * i);
            }

            return linearScaleData;
        }
    }
}
