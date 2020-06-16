using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
    public class CommonNumericalMethods
    {
        private static int lastHuntValue = 0;

        public static double[] cubicSpline(double[] xIn, double[] yIn, double[] xOut)
        {
            double[] array = new double[xOut.Length];

            double[] dy = CommonNumericalMethods.nrSpline(xIn, yIn, 0.0, 0.0);

            for (int i = 0; i < xOut.Length; i++)
            {
                array[i] = CommonNumericalMethods.nrSplint(xIn, yIn, dy, xOut[i]);
            }
            return array;
        }

        public static int nrHunt(double[] x, double val)
        {
            int num = CommonNumericalMethods.lastHuntValue;
            int num2 = x.Length - 1;
            bool flag = x[num2] > x[1];
            int num3;
            int result;
            if (num <= 0 || num > num2)
            {
                num = 0;
                num3 = num2 + 1;
            }
            else
            {
                int num4 = 1;
                if (val >= x[num] == flag)
                {
                    if (num == num2)
                    {
                        result = num;
                        return result;
                    }
                    num3 = num + 1;
                    while (val >= x[num3] == flag)
                    {
                        num = num3;
                        num4 += num4;
                        num3 = num + num4;
                        if (num3 > num2)
                        {
                            num3 = num2 + 1;
                            break;
                        }
                    }
                }
                else
                {
                    if (num == 1)
                    {
                        num = 0;
                        result = num;
                        return result;
                    }
                    num3 = num--;
                    while (val < x[num] == flag)
                    {
                        num3 = num;
                        num4 <<= 1;
                        if (num4 >= num3)
                        {
                            num = 0;
                            break;
                        }
                        num = num3 - num4;
                    }
                }
            }
            while (num3 - num != 1)
            {
                int num5 = num3 + num >> 1;
                if (val > x[num5] == flag)
                {
                    num = num5;
                }
                else
                {
                    num3 = num5;
                }
            }
            result = num;
            return result;
        }

        public static double[] nrSpline(double[] x, double[] y, double dx0, double dxn)
        {
            int num = x.Length - 1;
            double[] array = new double[x.Length];
            double[] array2 = new double[x.Length];
            if (dx0 > 9.9E+29)
            {
                array2[0] = (array[0] = 0.0);
            }
            else
            {
                array2[0] = -0.5;
                array[0] = 3.0 / (x[1] - x[0]) * ((y[1] - y[0]) / (x[1] - x[0]) - dx0);
            }
            for (int i = 1; i < num - 1; i++)
            {
                double num2 = (x[i] - x[i - 1]) / (x[i + 1] - x[i - 1]);
                double num3 = num2 * array2[i - 1] + 2.0;
                array2[i] = (num2 - 1.0) / num3;
                array[i] = (y[i + 1] - y[i]) / (x[i + 1] - x[i]) - (y[i] - y[i - 1]) / (x[i] - x[i - 1]);
                array[i] = (6.0 * array[i] / (x[i + 1] - x[i - 1]) - num2 * array[i - 1]) / num3;
            }
            double num5;
            double num4;
            if (dxn > 9.9E+29)
            {
                num4 = (num5 = 0.0);
            }
            else
            {
                num5 = 0.5;
                num4 = 3.0 / (x[num] - x[num - 1]) * (dxn - (y[num] - y[num - 1]) / (x[num] - x[num - 1]));
            }
            array2[num] = (num4 - num5 * array[num - 1]) / (num5 * array2[num - 1] + 1.0);
            for (int j = num - 1; j >= 1; j--)
            {
                array2[j] = array2[j] * array2[j + 1] + array[j];
            }
            return array2;
        }

        public static double nrSplint(double[] x, double[] y, double[] dy, double xval)
        {
            int num = x.Length - 1;
            int num2 = CommonNumericalMethods.nrHunt(x, xval);
            double result;
            if (num2 >= x.Length - 1)
            {
                result = y[x.Length - 1];
            }
            else
            {
                int num3 = num2 + 1;
                double num4 = x[num3] - x[num2];
                if (num4 <= 1E-09)
                {
                    result = 0.0;
                }
                else
                {
                    double num5 = (x[num3] - xval) / num4;
                    double num6 = (xval - x[num2]) / num4;
                    double num7 = num5 * y[num2] + num6 * y[num3] + ((num5 * num5 * num5 - num5) * dy[num2] + (num6 * num6 * num6 - num6) * dy[num3]) * (num4 * num4) / 6.0;
                    result = num7;
                }
            }
            return result;
        }

        public static double average(double[] data)
        {
            return average(data, 0, data.Length - 1);
        }

        public static double average(double[] data, int start, int end)
        {
            double result;
            try
            {
                //  this.setErrorCode(0);
                double num = 0.0;
                if (end < start)
                {
                    int num2 = start;
                    start = end;
                    end = num2;
                }
                for (int i = start; i <= end; i++)
                {
                    num += data[i];
                }
                result = num / (double)(end - start + 1);
            }
            catch (NullReferenceException)
            {
                //this.setErrorCode(7);
                result = -1.0;
            }
            catch (IndexOutOfRangeException)
            {
                //this.setErrorCode(5, "Array Index Out of Bounds (Check Input Indices.  Are They Too Large?");
                result = -1.0;
            }
            catch (Exception ex)
            {
                //   this.setErrorCode(1, "WARNING: Unexpected Error: " + ex.Message);
                result = -1.0;
            }
            return result;
        }

        public static double average(double[] x, double[] data, double startX, double endX)
        {
            double result;
            try
            {
                //  this.setErrorCode(0);
                int start = CommonNumericalMethods.nrHunt(x, startX);
                int end = CommonNumericalMethods.nrHunt(x, endX);
                result = average(data, start, end);
            }
            catch (NullReferenceException)
            {
                //  this.setErrorCode(7);
                result = -1.0;
            }
            catch (IndexOutOfRangeException)
            {
                //  this.setErrorCode(5, "Array Index Out of Bounds (Check Input Indices.  Are They Too Large?");
                result = -1.0;
            }
            catch (Exception ex)
            {
                //this.setErrorCode(1, "WARNING: Unexpected Error: " + ex.Message);
                result = -1.0;
            }
            return result;
        }

        // MPI Spam
        public static void CalcualteStartAndEndPixel(double[] wavelength, double startWave, double endWave, out int startPixel, out int endPixel)
        {
            startPixel = 0;
            endPixel = wavelength.Length - 1;

            //--- Use the default value return
            if (endWave < startWave)
                return;

            //-- Search the pixel index in the every range
            for (int k = 0; k < wavelength.Length; k++)
            {
                if (wavelength[k] < startWave)
                {
                    startPixel = k;
                }

                if (wavelength[wavelength.Length - 1 - k] >= endWave)
                {
                    endPixel = wavelength.Length - 1 - k;
                }
            }
            //--- Not find index for the start wave
            if (startWave == wavelength[wavelength.Length - 1])
            {
                startPixel = wavelength.Length - 2;
            }
            //--- Not find index for the end wave
            if (endWave == wavelength[0])
            {
                endPixel = 1;
            }
            //--- When start value out of max range. ( default , startIndex = 0, out of min range )
            if (startWave > wavelength[wavelength.Length - 1])
            {
                startPixel = wavelength.Length - 1;
            }
            //--- When end value out of min range. ( default , endIndex = max Index, out of max range )
            if (endWave < wavelength[0])
            {
                endPixel = 0;
            }

        }

        public static int getMaxIndex(double[] yArray, int startIndex, double yLowBoundValue)
        {
            return getMaxIndex(yArray, startIndex, yArray.Length, yLowBoundValue);
        }

        public static int getMaxIndex(double[] yArray, int startIndex, int endIndex, double yLowBoundValue)
        {
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            if (endIndex > yArray.Length)
            {
                endIndex = yArray.Length;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                if (yArray[i] > yLowBoundValue)
                {
                    yLowBoundValue = yArray[i];
                    startIndex = i;
                }
            }
            return startIndex;
        }

        public static int findNearestElement(double[] data, double value_Renamed)
        {
            int result;

            int num = findNearestElementLessThan(data, value_Renamed);
            int num2 = num + 1;
            if (num2 >= data.Length)
            {
                result = num;
            }
            else
            {
                double num3 = data[num];
                double num4 = data[num2];
                double num5 = Math.Abs(num3 - value_Renamed);
                double num6 = Math.Abs(num4 - value_Renamed);
                if (num6 > num5)
                {
                    result = num;
                }
                else
                {
                    result = num2;
                }
            }

            return result;
        }

        public static int findNearestElementLessThan(double[] data, double value_Renamed)
        {
            int result;

            int num = 0;
            int num2 = data.Length - 1;
            if (num2 == num)
            {
                result = (int)value_Renamed;
            }
            else
            {
                bool flag = data[num2] > data[num];
                while (num2 - num != 1)
                {
                    int num3 = num2 + num >> 1;
                    if (value_Renamed > data[num3] == flag)
                    {
                        num = num3;
                    }
                    else
                    {
                        num2 = num3;
                    }
                }
                if (flag)
                {
                    result = num;
                }
                else
                {
                    result = num2;
                }
            }

            return result;
        }

        public static int findNearestElementGreaterThan(double[] data, double value_Renamed)
        {
            int result;

            int num = 0;
            int num2 = data.Length - 1;
            bool flag = data[num2] > data[num];
            while (num2 - num != 1)
            {
                int num3 = num2 + num >> 1;
                if (value_Renamed > data[num3] == flag)
                {
                    num = num3;
                }
                else
                {
                    num2 = num3;
                }
            }
            if (flag)
            {
                result = num2;
            }
            else
            {
                result = num;
            }
            return result;
        }

        private static double interpolateLagrange(double xOut, double[] xIn, double[] yIn, int inOffset, int order)
        {
            double result;
            try
            {
                if (xIn == null || null == yIn)
                {
                    result = 0.0;
                }
                else
                {
                    if (order >= xIn.Length)
                    {
                        result = 0.0;
                    }
                    else
                    {
                        if (order + inOffset >= xIn.Length)
                        {
                            result = 0.0;
                        }
                        else
                        {
                            double[] array = new double[order + 1];
                            double[] array2 = new double[order + 1];
                            for (int i = 0; i < order + 1; i++)
                            {
                                array[i] = 1.0;
                                array2[i] = xOut - xIn[i + inOffset];
                            }
                            for (int i = 0; i < order + 1; i++)
                            {
                                for (int j = 0; j < order + 1; j++)
                                {
                                    if (i != j)
                                    {
                                        array[i] *= array2[j] / (xIn[i + inOffset] - xIn[j + inOffset]);
                                    }
                                }
                            }
                            double num = 0.0;
                            for (int i = 0; i < order + 1; i++)
                            {
                                num += yIn[i + inOffset] * array[i];
                            }
                            result = num;
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                result = 0.0;
            }
            catch (Exception ex)
            {
                result = 0.0;
            }
            return result;
        }

        public static double[] interpolateSpectrumLagrange(double[] xIn, double[] yIn, double[] xOut, int order, bool trim)
        {
            double[] result;
            try
            {
                if (xIn == null || yIn == null || null == xOut)
                {

                    result = new double[0];
                }
                else
                {
                    if (xIn.Length != yIn.Length)
                    {

                        result = new double[0];
                    }
                    else
                    {
                        if (xIn.Length <= order + 1)
                        {

                            result = new double[0];
                        }
                        else
                        {
                            int num = findNearestElementLessThan(xIn, xOut[0]);
                            num -= order / 2;
                            if (num < 0)
                            {
                                num = 0;
                            }
                            double[] array = new double[xOut.Length];
                            for (int i = 0; i < xOut.Length; i++)
                            {
                                if (trim && (xOut[i] < xIn[0] || xOut[i] > xIn[xIn.Length - 1]))
                                {
                                    array[i] = 0.0;
                                }
                                else
                                {
                                    array[i] = interpolateLagrange(xOut[i], xIn, yIn, num, order);
                                }
                                if (i < xOut.Length - 1)
                                {
                                    int num2 = num;
                                    while (num2 + order - order / 2 - 1 < xIn.Length)
                                    {
                                        if (xIn[num2] > xOut[i + 1])
                                        {
                                            num2 -= order / 2 + 1;
                                            num = ((num2 < 0) ? 0 : num2);
                                            break;
                                        }
                                        num2++;
                                    }
                                }
                            }

                            result = array;
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                result = new double[0];
            }
            catch (Exception ex)
            {
                result = new double[0];
            }
            return result;
        }

    }
}
