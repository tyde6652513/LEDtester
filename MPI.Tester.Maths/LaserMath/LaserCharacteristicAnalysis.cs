using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths.LaserMath
{
    public static class LaserCharacteristicAnalysis
    {
        #region >>> Common Math <<<

        public static bool SearchPointIndex(double value, double[] valueArray, out int index)
        {
            index = 0;

            for (int i = 0; i < valueArray.Length; i++)
            {
                if (valueArray[i] >= value)
                {
                    index = i;

                    return true;
                }
            }

            return false;
        }

        public static bool SearchPointIndex(double value1, double value2, double[] valueArray, out int index1, out int index2)
        {
            bool isFoundIndex1 = false;

            bool isFoundIndex2 = false;

            index1 = 0;

            index2 = 0;

            for (int i = 0; i < valueArray.Length; i++)
            {
                if (valueArray[i] >= value1 && !isFoundIndex1)
                {
                    index1 = i;

                    isFoundIndex1 = true;
                }

                if (valueArray[i] >= value2 & !isFoundIndex2)
                {
                    index2 = i;

                    isFoundIndex2 = true;
                }

                if (isFoundIndex1 & isFoundIndex2)
                {
                    return true;
                }
            }

            return false;
        }

        public static double[] MovingAverage(double[] data, int windows)
        {
            if (data == null || data.Length == 0 || windows <= 0)
            {
                return data.Clone() as double[];
            }

            double[] afterFilter = new double[data.Length];

            int i = 0;

            while (i < data.Length)
            {
                double Element = 0.0;

                if (i > windows / 2 & i < data.Length - windows / 2)
                {
                    for (int j = 0; j < windows; j++)
                    {
                        Element += data[i + j - windows / 2];
                    }

                    Element /= (double)windows;
                }
                else
                {
                    Element = data[i];
                }

                afterFilter[i] = Element;

                i++;
            }
            return afterFilter;
        }

        public static double Average(double[] array, int startIndex, int endIndex)
        {
            if (endIndex - startIndex == 0)
            {
                return 0.0d;
            }

            double sum = 0.0d;

            for (int i = startIndex; i <= endIndex; i++)
            {
                sum += array[i];
            }

            return sum / (double)(endIndex - startIndex + 1);
        }

        public static double SlopeByLinearRegression(double[] x, double[] y, int startIndex, int endIndex)
        {
            //--------------------------------------------------------------------------------------------
            //  Roy Huang, 20150512
            //  最小平方法 -> 斜率公式
            //            SUM [(Xi - Xavg) * (Yi - Yavg)]
            //  Slope =  -------------------------------------
            //            SUM [(Xi - Xavg) ^ 2]
            //--------------------------------------------------------------------------------------------
            double sumH = 0.0d;

            double sumL = 0.0d;

            double xAvg = 0.0d;

            double yAvg = 0.0d;

            if (endIndex - startIndex == 1)
            {
                sumH = y[endIndex] - y[startIndex];
                sumL = x[endIndex] - x[startIndex];
            }
            else
            {
                xAvg = Average(x, startIndex, endIndex);

                yAvg = Average(y, startIndex, endIndex);

                for (int i = startIndex; i < endIndex; i++)
                {
                    sumH += (x[i] - xAvg) * (y[i] - yAvg);

                    sumL += (x[i] - xAvg) * (x[i] - xAvg);
                }
            }

            return sumH / sumL;
        }

        public static double[] ForwardDifference(double[] x, double[] y)
        {
            if ((x == null || x.Length == 0) && (y == null || y.Length == 0))
            {
                return null;
            }

            double[] result = new double[x.Length];

            double tempValue = 0.0d;

            for (int i = 1; i < x.Length; i++)
            {
                if (i < x.Length - 1)
                {
                    tempValue = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
                }

                result[i] = Math.Round(tempValue, 9, MidpointRounding.AwayFromZero);
            }

            return result;
        }

        public static double[] CentralDifference(double[] x, double[] y)
        {
            if ((x == null || x.Length == 0) && (y == null || y.Length == 0))
            {
                return null;
            }

            double[] result = new double[x.Length];

            double tempValue = 0.0d;

            for (int i = 0; i < x.Length; i++)
            {
                if (i > 0 && i < x.Length - 1)
                {
                    tempValue = (y[i - 1] - y[i + 1]) / (x[i - 1] - x[i + 1]);
                }
                else
                {
                    if (i == x.Length - 1)
                    {
                        tempValue = (y[i - 2] - y[i]) / (x[i - 2] - x[i]);
                    }
                }

                result[i] = tempValue;
            }

            return result;
        }

        public static double Interpolate(double x1, double y1, double x2, double y2, double x)
        {
            double slop = 0.0d;

            if ((x2 - x1) != 0)
            {
                slop = (y2 - y1) / (x2 - x1);
                return slop * (x - x1) + y1;
            }

            return 0.0d;
        }

        public static void LinearRegression(double[] x, double[] y, int startIndex, int endIndex, out double slope, out double intercept, out double rsqure)
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

            int m = 0;

            for (int i = startIndex; i <= endIndex; i++)
            {
                Lx += x[i];	//sum of x

                Ly += y[i];

                Lxx += Math.Pow(x[i], 2);

                Lxy += x[i] * y[i];

                Lyy += Math.Pow(y[i], 2);

                m++;
            }

            slope = (m * Lxy - Lx * Ly) / (m * Lxx - Math.Pow(Lx, 2));
            intercept = (Lxx * Ly - Lxy * Lx) / (m * Lxx - Math.Pow(Lx, 2));

            Syy = Lyy - (Math.Pow(Ly, 2) / m);
            Sxx = Lxx - (Math.Pow(Lx, 2) / m);
            Sxy = Lxy - ((Lx * Ly) / m);

            SSR = Math.Pow(Sxy, 2) / Sxx;

            rsqure = SSR / Syy;
        }

        #endregion

        #region >>> Laser Characteristic <<<

        /// <summary>
        /// 
        /// </summary>
        public static void ThresholdPointByInterpolation(double[] currArray, double[] voltArray, double[] powArray, double ith, out double pth, out double vth)
        {
            int ithIndex = 0;

            pth = 0.0d;
            vth = 0.0d;

            if (!SearchPointIndex(ith, currArray, out ithIndex))
            {
                return;
            }

            if (ithIndex == 0)
            {
                return;
            }

            double factor = (currArray[ithIndex] - ith) / (currArray[ithIndex] - currArray[ithIndex - 1]);

            pth = powArray[ithIndex] - (powArray[ithIndex] - powArray[ithIndex - 1]) * factor;

            vth = voltArray[ithIndex] - (voltArray[ithIndex] - voltArray[ithIndex - 1]) * factor;
        }

        /// <summary>
        /// Operation Point (Iop [A], Vop [V])
        /// Interpolation
        /// <\summary>
        public static void OperationPointByInterpolation(double[] currArray, double[] voltArray, double[] powArray, double pop, out double iop, out double vop, out double po)
        {
            int popIndex = 0;
            
            iop = 0.0d;
            vop = 0.0d;
            po = 0.0d;

            if (!SearchPointIndex(pop, powArray, out popIndex))
            {
                return;
            }

            if (popIndex == 0)
            {
                return;
            }

            double factor = (powArray[popIndex] - pop) / (powArray[popIndex] - powArray[popIndex - 1]);

            iop = currArray[popIndex] - (currArray[popIndex] - currArray[popIndex - 1]) * factor;

            vop = voltArray[popIndex] - (voltArray[popIndex] - voltArray[popIndex - 1]) * factor;

            po = pop;
        }

        /// <summary>
        /// Operation Point (Iop [A], Vop [V])
        /// Closest Point
        /// <\summary>
        public static void OperationPointByClosestPoint(double[] currArray, double[] voltArray, double[] powArray, double pop, out double iop, out double vop, out double po)
        {
            int popIndex = 0;

            iop = 0.0d;
            vop = 0.0d;
            po = 0.0d;

            if (!SearchPointIndex(pop, powArray, out popIndex))
            {
                return;
            }

            iop = currArray[popIndex];

            vop = voltArray[popIndex];

            po = powArray[popIndex];
        }

        /// <summary>
        /// Operation Point (Iop [A], Vop [V])
        /// (AMS) On Fitting Line
        /// <\summary>
        public static void OperationPointOnFittingLine(double ith, double se, double rs,  double i1, double v1, double pop, out double iop, out double vop, out double po)
        {
            iop = 0.0d;
            vop = 0.0d;
            po = 0.0d;

            if (se == 0.0d)
            {
                return;
            }

            iop = ith + pop / se;

            vop = v1 + rs * (iop - i1);

            po = pop;
        }

        /// <summary>
        /// Slope Efficiency (SE [mW/A])
        /// TwoPointDifferent
        /// <\summary>
        public static double SlopeEfficiencyDiff(double[] currArray, double[] powArray, int startIdx, int endIdx)
        {
            return (powArray[endIdx] - powArray[startIdx]) / (currArray[endIdx] - currArray[startIdx]);
        }

        /// <summary>
        /// Slope Efficiency (SE [mW/A])
        /// Linear Regression
        /// <\summary>
        public static double SlopeEfficiencyLR(double[] currArray, double[] powArray, int startIdx, int endIdx)
        {
            return SlopeByLinearRegression(currArray, powArray, startIdx, endIdx);
        }

        /// <summary>
        /// Slope Efficiency (SE [mW/A])
        /// Average the SE data in the section
        /// <\summary>
        public static double SlopeEfficiencyAvg(double[] currArray, double[] powArray, int startIdx, int endIdx)
        {
            double[] seArray = ForwardDifference(currArray, powArray);

            double sum = 0.0d;

            int count = 0;

            double se = 0.0d;

            for (int i = startIdx; i <= endIdx; i++)
            {
                sum += seArray[i];

                count++;
            }

            if (count > 0)
            {
                se = sum / count;
            }

            return se;
        }

        /// <summary>
        /// Threshold Current (Ith [A])
        /// TwoPointDifferent
        /// <\summary>
        public static double ThresholdCurrentDiff(double[] currArray, double[] powArray, double slope, int startIdx, int endIdx)
        {
            if (slope <= 0.01d)
            {
                return 0.0d;
            }
            
            return currArray[startIdx] - (powArray[startIdx] / slope);
        }

        /// <summary>
        /// Threshold Current (Ith [A])
        /// Linear Regression
        /// <\summary>
        public static double ThresholdCurrentLR(double[] currArray, double[] powArray, double slope, int startIdx, int endIdx)
        {
            if (slope <= 0.01d)
            {
                return 0.0d;
            }

            if (startIdx == endIdx)
            {
                return 0.0d;
            }
            //--------------------------------------------------------------------------------------------
            //  Roy Huang, 20150512
            //  最小平方法 -> 截距公式
            //  b = Yavg - m * Xavg
            //  Ith -->  y = mx + b, which y = 0 
            //  Xo (Ith) = -b / m = -(Yavg - m * Xavg) / m = Xavg - (Yavg / m);
            //--------------------------------------------------------------------------------------------
            double ith = 0.0d;

            double sum = 0.0d;

            for (int i = startIdx; i < endIdx; i++)
            {
                sum += currArray[i] - powArray[i] / slope;
            }

            ith = sum / (double)(endIdx - startIdx);

            return ith;
        }

        public static double ThresholdCurrentSe(double[] currArray, double[] powArray, double thrValue, int startIdx, int endIdx)
        {
            double ith = 0.0d;

            //--------------------------------------------------------------------------------------------
            //  Roy Huang, 20171128
            //  閥值法: dP/dI > ThrValue (mW/mA) 的第一個點, 為Threshold Current
            //--------------------------------------------------------------------------------------------
            double[] se = ForwardDifference(currArray, powArray);  // mW/A

            double se1 = 0.0d;
            double se2 = 0.0d;

            for (int i = startIdx; i < endIdx; i++)
            {
                se1 = se[i] / 1000.0d; // W/A
                se2 = se[i + 1] / 1000.0d; // W/A

                if (se1 >= thrValue && se2 >= thrValue)
                {
                    ith = currArray[i];

                    break;
                }
            }

            return ith;
        }

        public static double ThresholdCurrentPo(double[] currArray, double[] powArray, double thrValue, int startIdx, int endIdx)
        {
            double ith = 0.0d;

            //--------------------------------------------------------------------------------------------
            //  Roy Huang, 20171213
            //  閥值法: Pi > ThrValue (mW) 的第一個點, 為Threshold Current
            //--------------------------------------------------------------------------------------------
            double pi = 0.0d;

            if (startIdx > 1)
            {
                startIdx = startIdx - 1;
            }

            for (int i = startIdx; i < endIdx; i++)
            {
                pi = powArray[i];

                if (pi >= thrValue)
                {
                    ith = currArray[i];

                    break;
                }
            }

            return ith;
        }

        /// <summary>
        /// Resistance Series (RS [ohm])
        /// TwoPointDifferent
        /// <\summary>
        public static double ResistanceDiff(double[] currArray, double[] voltArray, int startIdx, int endIdx)
        {
            return (voltArray[endIdx] - voltArray[startIdx]) / (currArray[endIdx] - currArray[startIdx]);
        }

        /// <summary>
        /// Resistance Series (RS [ohm])
        /// Linear Regression
        /// <\summary>
        public static double ResistanceLR(double[] currArray, double[] voltArray, int startIdx, int endIdx)
        {
            double rs = 0.0d;

            double sumH = 0.0d;

            double sumL = 0.0d;

            double xAvg = 0.0d;

            double yAvg = 0.0d;

            xAvg = Average(currArray, startIdx, endIdx);

            yAvg = Average(voltArray, startIdx, endIdx);

            for (int i = startIdx; i < endIdx; i++)
            {
                sumH += (currArray[i] - xAvg) * (voltArray[i] - yAvg);

                sumL += (currArray[i] - xAvg) * (currArray[i] - xAvg);
            }

            rs = sumH / sumL;

            return rs;
        }

        public static double ResistanceAvg(double[] currArray, double[] voltArray, int startIdx, int endIdx)
        {
            double[] rsArray = ForwardDifference(currArray, voltArray);

            double sum = 0.0d;

            int count = 0;

            double rs = 0.0d;

            for (int i = startIdx; i <= endIdx; i++)
            {
                sum += rsArray[i];

                count++;
            }

            if (count > 0)
            {
                rs = sum / count;
            }

            return rs;
        }

        /// <summary>
        /// Linearity
        /// Ln = (SEmax - SEmin) / SE_LR [宏綱算法]
        /// <\summary>
        public static double Linearity(double[] currArray, double[] powArray, double[] SEArray, int startIdx, int endIdx)
        {
            if (startIdx >= endIdx)
            {
                return 0.0d;
            }

            double seMax = 0.0d;

            double seMin = 9999.0d;

            double seLR = SlopeByLinearRegression(currArray, powArray, startIdx, endIdx);

            for (int i = startIdx; i <= endIdx; i++)
            {
                if (seMax < SEArray[i])
                {
                    seMax = SEArray[i];
                }

                if (seMin > SEArray[i])
                {
                    seMin = SEArray[i];
                }
            }

            if (seMax == seMin)
            {
                seMax = SEArray[endIdx];
            }

            return ((seMax - seMin) / seLR) * 100.0d;
        }

        /// <summary>
        /// RollOver
        /// Rollover = (Pideal - Preal) / Pideal;  where Pidal = m * (X - (-b/m)) = m * (X - Ith)
        /// <\summary>
        public static double RollOver(double[] currArray, double[] powArray, double se, double ith, double iroll)
        {
            int rollIdx = 0;

            double Pideal = (iroll - ith) * se;   // Iroll, Ith unit: A; se unit: mW/A

            if (Pideal == 0.0d)
            {
                return 0.0d;
            }

            if (!SearchPointIndex(iroll, currArray, out rollIdx))
            {
                return 0.0d;
            }

            if (rollIdx == 0)
            {
                return 0.0d;
            }

            double Preal = powArray[rollIdx - 1] + (iroll - currArray[rollIdx - 1]) * (powArray[rollIdx] - powArray[rollIdx - 1]) / (currArray[rollIdx] - currArray[rollIdx - 1]);

            return (Pideal - Preal) / Pideal * 100.0d;
        }

        /// <summary>
        /// Kink: FittingLine
        /// <\summary>
        //public static double[] Kink(double[] currArray, double[] powArray, double Ith, double SE, double[] SEArray, double ratioRange, int startIdx, int endIdx)
        //{
        //    // [0] powAtkink, [1] currArKink, [2] ratioAtKink,
        //    double[] kinkCalcResult = new double[3];

        //    double[] seFitting = MovingAverage(SEArray, SEArray.Length / 5);

        //    double[] ratioSet = new double[powArray.Length];

        //    for (int i = 0; i < SEArray.Length; i++)
        //    {
        //        if (seFitting[i] == 0)
        //        {
        //            ratioSet[i] = 1.0d;
        //        }
        //        else
        //        {
        //            ratioSet[i] = ((SEArray[i] - seFitting[i]) / SEArray[i]) + 1.0d;
        //        }
        //    }

        //    bool isFoundKink = false;

        //    double maxRatio = 1.0d + ratioRange;

        //    double minRatio = 1.0d - ratioRange;

        //    double tempRatio = 0.0d;

        //    for (int i = 0; i < ratioSet.Length; i++)
        //    {
        //        if (i < startIdx || i > endIdx)
        //        {
        //            ratioSet[i] = 1.0d;
        //        }
        //        else
        //        {
        //            // 指定區間內, 搜尋 Kink
        //            if (!isFoundKink)
        //            {
        //                tempRatio = ratioSet[i];

        //                if (tempRatio > maxRatio || tempRatio < minRatio)
        //                {
        //                    kinkCalcResult[0] = powArray[i];

        //                    kinkCalcResult[1] = currArray[i];

        //                    kinkCalcResult[2] = tempRatio - 1.0d;

        //                    isFoundKink = true;
        //                }
        //            }
        //        }
        //    }

        //    if (!isFoundKink)
        //    {
        //        kinkCalcResult[0] = powArray[powArray.Length - 1];

        //        kinkCalcResult[1] = currArray[powArray.Length - 1];
        //    }

        //    //----------------------------------------------------------------------------
        //    int seMaxIndex = 0;

        //    for (int i = 0; i < SEArray.Length; i++)
        //    {
        //        if (SEArray[i] > 0 && currArray[i] > Ith)
        //        {
        //            seMaxIndex = i;
        //            break;
        //        }
        //    }

        //    double stepSE = SEArray[seMaxIndex];
        //    double stepPow = powArray[seMaxIndex];
        //    double stepRatio = SEArray[seMaxIndex] / SE;
        //    double kOut = (ratioSet.Max() - 1.0d) * 100.0d;

        //    return kinkCalcResult;
        //}

        /// <summary>
        /// Kink: SEk (UOC)
        /// <\summary>
        public static bool KinkBySEk(double[] currArray, double[] powArray, double[] seArray, double ratio, int startIdx, int endIdx, ref double kink, ref double pkink, ref double ikink)
        {
            pkink = 0.0d;
            ikink = 0.0d;
            kink = 0.0d;

            double seMax = seArray.Max();
            double sek = 0.0d;

            if (seMax == 0.0d)
            {
                return false;  
            }

            for (int i = startIdx; i < endIdx; i++)
            {
                sek = Math.Abs((seArray[i] - seMax) / seMax);

                if (sek >= ratio)
                {
                    pkink = powArray[i];
                    ikink = currArray[i];
                    kink = sek;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Kink: RefKinkCurve (HLJ)
        /// <\summary>
        public static bool KinkByRefCurve(double[] currArray, double[] powArray, double[] refCurrArray, double[] refPowerArray, int startIdx, int endIdx, ref double kink, ref double pkink, ref double ikink)
        {
            pkink = 0.0d;
            ikink = 0.0d;
            kink = 0.0d;
            
            if (refCurrArray.Length < 2 || refPowerArray.Length < 2)
            {
                return false;
            }

            double curr = 0.0d;
            double pow = 0.0d;

            double refPow = 0.0d;
            double deltaPow = 0.0d;

            int index = -1;

            for (int i = startIdx; i < endIdx; i++)
            {
                curr = currArray[i];
                pow = powArray[i];

                if (SearchPointIndex(curr, refCurrArray, out index))
                {
                    if (index > 0)
                    {
                        refPow = Interpolate(refCurrArray[i], refPowerArray[i], refCurrArray[i - 1], refPowerArray[i - 1], curr);

                        if (refPow != 0.0d)
                        {
                            deltaPow = (pow / refPow) - 1.0d;

                            if (deltaPow >= kink)
                            {
                                kink = deltaPow;
                                ikink = curr;
                                pkink = pow;
                            }
                        }
                    }
                }
            }

            if (kink == 0.0d)
            {
                pkink = 0.0d;
                ikink = 0.0d;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kink: FittingLine
        /// 
        ///                (dP / dI) - FittingLine(I) 
        /// Kink = max [ ----------------------------- ]
        ///                     FittingLine(I) 
        /// 
        /// <\summary>
        public static bool KinkByFittingLine(double[] currArray, double[] powArray, double[] seArray, double ratio, int startIdx, int endIdx, ref double kink, ref double pkink, ref double ikink)
        {
            pkink = 0.0d;
            ikink = 0.0d;
            kink = 0.0d;

            double slope = 0.0d;
            double intercept = 0.0d;
            double rsqure = 0.0d;

            double[] tempSE = new double[seArray.Length];

            for (int i = 0; i < tempSE.Length; i++ )
            {
                tempSE[i] = seArray[i] / 1000.0d;
            }

            tempSE = seArray;

            LinearRegression(currArray, tempSE, startIdx, endIdx, out slope, out intercept, out rsqure);  //

            double current = 0.0d;
            double power = 0.0d;
            double se = 0.0d;
            double onFittingLine = 0.0d;
            double tempKink = 0.0d;

            for (int i = startIdx; i <= endIdx; i++)
            {
                current = currArray[i];
                power = powArray[i];
                se = tempSE[i];
                onFittingLine = Math.Round((slope * current + intercept), 6, MidpointRounding.AwayFromZero);

                tempKink = Math.Abs((se - onFittingLine) / onFittingLine);

                if (tempKink > kink)
                {
                    kink = tempKink;
                    ikink = current;
                    pkink = power;
                }
            }

            if (kink >= ratio)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Kink = [P(i) - Pmin] / Pmin
        /// <\summary>
        public static bool KinkByDeltaPow(double[] currArray, double[] powArray,  double ratio, int startIdx, int endIdx, ref double kink, ref double pkink, ref double ikink)
        {
            pkink = 0.0d;
            ikink = 0.0d;
            kink = 0.0d;

            double tempKink = 0.0d;
            double current = 0.0d;
            double power1 = 0.0d;
            double power2 = 0.0d;
         
            for (int i = startIdx + 1; i < endIdx; i++)
            {
                current = currArray[i];
                power1 = powArray[i - 1];
                power2 = powArray[i];

                if (power1 == 0.0d)
                {
                    continue;
                }

                tempKink = Math.Abs(power2 - power1) / power1;

                if (tempKink > kink)
                {
                    kink = tempKink;
                    ikink = current;
                    pkink = power2;
                }
            }

            if (kink >= ratio)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Kink  二次微分法
        /// <\summary>
        public static bool KinkBySecondOrderDiff(double[] currArray, double[] powArray, double[] seArray, int startIdx, int endIdx, ref double kink, ref double pkink, ref double ikink)
        {
            pkink = 0.0d;
            ikink = 0.0d;
            kink = 0.0d;

            double tempKink = 0.0d;
            double current = 0.0d;
            double power = 0.0d;

            double[] tempSE = new double[seArray.Length];
            double[] tempI = new double[currArray.Length];

            for(int i = 0; i < tempSE.Length; i++)
            {
                tempSE[i] = seArray[i] / 1000.0d;
                tempI[i] = currArray[i] * 1000.0d;
            }

            double[] secOrderSE = ForwardDifference(tempI, tempSE);

            for (int i = startIdx; i <= endIdx; i++)
            {
                tempKink = Math.Abs(secOrderSE[i]);

                current = currArray[i];
                power = powArray[i];

                if (tempKink > kink)
                {
                    kink = tempKink;
                    ikink = current;
                    pkink = power;
                }
            }

            return true;
        }

        /// <summary>
        /// power conversion efficiency (PCE)
        /// PCE(%) = Pop / (Iop * Pop)
        /// <\summary>
        public static double PCE(double i, double v, double p)
        {
            double pce = 0.0d;

            if (i * v > 0)
            {
                pce = (p / 1000.0d) / (i * v) * 100.0d; // W / (A * V), unit: %
            }

            return pce;    
        }

        /// <summary>
        /// power conversion efficiency (PCE)
        /// PCE(%) = P@i / (i * V@i), where i = indicate current point
        /// <\summary>
        public static double PCE(double[] currArray, double[] voltArray, double[] powArray, double indicateCurrnetPoint)
        {
            double pce = 0.0d;

            int index = 0;

            if (SearchPointIndex(indicateCurrnetPoint, currArray, out index))
            {
                double po = 0.0d;
                double vo = 0.0d;

                po = powArray[index];
                vo = voltArray[index];

                if (indicateCurrnetPoint * vo > 0)
                {
                    pce = (po / 1000.0d) / (indicateCurrnetPoint * vo) * 100.0d; // W / (A * V), unit: %
                }
            }

            return pce;
        }

        public static void SearchPointByInterpolation(double[] currArray, double[] voltArray, double[] powArray, double[] rsArray, double i, out double p, out double v, out double rd)
        {
            int idx = 0;

            p = 0.0d;
            v = 0.0d;
            rd = 0.0d;

            if (!SearchPointIndex(i, currArray, out idx))
            {
                return;
            }

            if (idx == 0)
            {
                p = powArray[idx];
                v = voltArray[idx];
                rd = 0.0d;
                return;
            }

            double factor = (currArray[idx] - i) / (currArray[idx] - currArray[idx - 1]);

            p = powArray[idx] - (powArray[idx] - powArray[idx - 1]) * factor;

            v = voltArray[idx] - (voltArray[idx] - voltArray[idx - 1]) * factor;

            rd = rsArray[idx] - (rsArray[idx] - rsArray[idx - 1]) * factor;
        }

        public static void COD(double[] currArray, double[] powArray, double[] seArray, double Ith, ref double Icod, ref double Pcod)
        {
            Icod = 0.0d;
            Pcod = 0.0d;
            
            double current = 0.0d;
            double pow = 0.0d;
            double se = 0.0d;

            for (int i = 0; i < seArray.Length; i++)
            {
                current = currArray[i];
                pow = powArray[i];
                se = seArray[i];

                if (current >= Ith)
                {
                    if (se < 0)
                    {
                        Icod = current;
                        Pcod = pow;
                        return;
                    }
                }
            }
        }

        #endregion
    }
}
