using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
    public class MPIFilter
    {
        private static SavitzkyGolayFilter _sgFilter = new SavitzkyGolayFilter();

        public static double[] DoSavitzkyGolay(double[] data, uint points, uint order)
        {
            return _sgFilter.filter(data, points, order);
        }

        public static double[] DoBoxCar(double[] array, int numberOfPixelsOnEitherSideOfCenter)
        {
            double sum = 0;
            int moveIndex = 0;
            double[] caculatedArray = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                sum = array[i];
                for (int index = 1; index <= numberOfPixelsOnEitherSideOfCenter; index++)
                {
                    //decrease
                    moveIndex = i - index;
                    if (moveIndex < 0)
                    {
                        sum += array[0];
                    }
                    else
                    {
                        sum += array[moveIndex];
                    }

                    //increase
                    moveIndex = i + index;
                    if (moveIndex >= array.Length)
                    {
                        sum += array[array.Length - 1];
                    }
                    else
                    {
                        sum += array[moveIndex];
                    }
                }
                caculatedArray[i] = sum / (2 * numberOfPixelsOnEitherSideOfCenter + 1);
            }

            return caculatedArray;
        }

        private double[] BoxcarAverage(double[] pixels, int boxcarWidth)
        {
            double[] array = new double[pixels.Length];
            for (int i = 0; i < pixels.Length; i++)
            {
                int num = i - boxcarWidth;
                if (num < 0)
                {
                    num = 0;
                }
                int num2 = i + boxcarWidth;
                if (num2 >= pixels.Length)
                {
                    num2 = pixels.Length - 1;
                }
                double num3 = 0.0;
                for (int j = num; j <= num2; j++)
                {
                    num3 += pixels[j];
                }
                array[i] = num3 / (double)(num2 - num + 1);
            }
            return array;
        }

        /// <summary>
        /// filtered data can advance repeatibilty of DUT , particularly in red LED product 
        /// </summary>
        public static double[] FilterBaseLineNoise(double[] wavelength, double cutOffPoint, double[] data, double cutValue)
        {
            int indexPixel = 0;
            int UpindexPixel = 0;
            int LowindexPixel = 0;
            double filterValue = 0.0d;
            double[] outputData = new double[data.Length];

            if (((int)cutValue) == 0)
            {
                cutValue = 1.0;
            }

            indexPixel = CommonNumericalMethods.getMaxIndex(data, 0, 0.0d);

            if (wavelength[indexPixel] < cutOffPoint)
            {
                return data;
            }

            filterValue = data[indexPixel] / cutValue;

            for (int i = indexPixel; i < data.Length; i++)
            {
                if (data[i] < filterValue)
                {
                    UpindexPixel = i;
                    break;
                }
            }

            for (int i = indexPixel; i >= 0; i--)
            {
                if (data[i] < filterValue)
                {
                    LowindexPixel = i;
                    break;
                }
            }

            for (int i = 0; i < outputData.Length; i++)
            {
                if (i > LowindexPixel && i < UpindexPixel)
                {
                    outputData[i] = data[i];
                }
                else
                {
                    outputData[i] = 0;
                }
            }

            return outputData;
        }

    }

    internal class SavitzkyGolayFilter
    {
        private double[] _coefSmoothing;

        public SavitzkyGolayFilter()
        {
        }

        #region >>> Public Method <<<

        public double[] filter(double[] data, uint points, uint order)
        {
            if (points % 2 == 0 || points < 5 || points > 25)
            {
                return data;
            }
            double[] tempSum = new double[data.Length];
            int halfWindow;
            this._coefSmoothing = new double[points];
            this.SGcoef(points, order);

            halfWindow = ((int)points - 1) / 2;

            for (int i = halfWindow; i < (data.Length - halfWindow); i++)
            {

                for (int j = 0; j < points; j++)
                {
                    tempSum[i] += this._coefSmoothing[j] * data[i - halfWindow + j];
                }

            }
            return tempSum;
        }

        #endregion

        #region >>> Private Method <<<

        private void SGcoef(uint points, uint order)
        {
            double[] tempCoef = null;
            int norm = 0;

            switch (points)
            {
                case 5:
                    tempCoef = new double[] { -3.0d, 12.0d, 17.0d, 12.0d, -3.0d };
                    norm = 35;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 7:
                    tempCoef = new double[] { -2.0d, 3.0d, 6.0d, 7.0d, 6.0d, 3.0d, -2.0d };
                    norm = 21;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 9:
                    tempCoef = new double[] { -21.0d, 14.0d, 39.0d, 54.0d, 59.0d, 54.0d, 39.0d, 14.0d, -21.0d };
                    norm = 231;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 11:
                    tempCoef = new double[] {-36.0d, 9.0d, 44.0d, 69.0d, 84.0d, 89.0d, 
                                              84.0d, 69.0d, 44.0d, 9.0d, -36.0d };
                    norm = 429;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 13:
                    tempCoef = new double[] {-11.0d, 0.0d, 9.0d, 16.0d, 21.0d, 24.0d, 25.0d,
                                              24.0d, 21.0d, 16.0d, 9.0d, 0.0d, -11.0d };
                    norm = 143;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 15:
                    tempCoef = new double[] {-78.0d, -13.0d, 42.0d, 87.0d, 122.0d, 147.0d, 162.0d, 167.0d,
                                             162.0d, 147.0d, 122.0d, 87.0d, 42.0d, -13.0d, -78.0d };
                    norm = 1105;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 17:
                    tempCoef = new double[] {-21.0d, -6.0d, 7.0d, 18.0d, 27.0d, 34.0d, 39.0d, 42.0d, 43.0d,
                                              42.0d, 39.0d, 34.0d, 27.0d, 18.0d, 7.0d, -6.0d, -21.0d };
                    norm = 323;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 19:
                    tempCoef = new double[] {-136.0d, -51.0d, 24.0d, 89.0d, 144.0d, 189.0d, 224.0d, 249.0d, 264.0d, 269.0d,
                                              264.0d, 249.0d, 224.0d, 189.0d, 144.0d, 89.0d, 24.0d, -51.0d, -136.0d };
                    norm = 2261;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 21:
                    tempCoef = new double[] {-171.0d, -76.0d, 9.0d, 84.0d, 149.0d, 204.0d, 249.0d, 284.0d, 309.0d, 324.0d, 329.0d,
                                             324.0d, 309.0d, 284.0d, 249.0d, 204.0d, 149.0d, 84.0d, 9.0d, -76.0d, -171.0d };
                    norm = 3059;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 23:
                    tempCoef = new double[] {-42.0d, -21.0d, -2.0d, 15.0d, 30.0d, 43.0d, 54.0d, 63.0d, 70.0d, 75.0d, 78.0d, 79.0d,
                                              78.0d, 75.0d, 70.0d, 63.0d, 54.0d, 43.0d, 30.0d, 15.0d, -2.0d, -21.0d, -42.0d };
                    norm = 805;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                case 25:
                    tempCoef = new double[] {-253.0d, -138.0d, -33.0d, 62.0d, 147.0d, 222.0d, 
                                              287.0d, 343.0d, 387.0d, 422.0d, 447.0d, 462.0d, 467.0d,
                                              462.0d, 447.0d, 422.0d, 387.0d, 343.0d, 287.0d, 
                                              222.0d, 147.0d, 62.0d, -33.0d, -138.0d, -253.0d };
                    norm = 5175;
                    this.GenerateCoef(tempCoef, norm);
                    break;
                //------------------------------------------------------------------------------------------
                default:
                    tempCoef = new double[] {-36.0d, 9.0d, 44.0d, 69.0d, 84.0d, 89.0d, 
                                              84.0d, 69.0d, 44.0d, 9.0d, -36.0d };
                    norm = 429;
                    this.GenerateCoef(tempCoef, norm);
                    break;
            }
        }

        private void GenerateCoef(double[] tempCoef, int norm)
        {
            for (int i = 0; i < tempCoef.Length; i++)
            {
                this._coefSmoothing[i] = tempCoef[i] / norm;
            }
        }

        #endregion

    }
}
