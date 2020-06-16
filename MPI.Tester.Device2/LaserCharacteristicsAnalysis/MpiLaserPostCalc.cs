using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI;
using MPI.Tester;
using MPI.Tester.Data;
using MPI.Tester.Maths.LaserMath;

namespace MPI.Tester.Device.PostCalc
{
    public class MpiLaserPostCalc : ILaserPostCalc
    {
        private object _lockObj;

        private const string REF_KINK_FILENAME_AND_PATH = @"C:\MPI\LEDTester\Data\refKink.csv";

        private double _calPowFactor = 1.0d;

        private LaserCalcSetting _setting;
        private LaserCharacteristicsData _result;
        private LaserCurveData _curve;

        private List<double> _refIkinkCurve;
        private List<double> _refPkinkCurve;

        public MpiLaserPostCalc()
        {
            this._lockObj = new object();

            this._setting = new LaserCalcSetting();
            this._result = new LaserCharacteristicsData();
            this._curve = new LaserCurveData();

            this._refIkinkCurve = new List<double>();
            this._refPkinkCurve = new List<double>();

            //this.LoadKinkRefCurve(REF_KINK_FILENAME_AND_PATH);
        }

        #region >>> Public Property <<<

        public double CalibratedPowerFactor
        {
            get { return this._calPowFactor; }
            set 
            { 
                lock (this._lockObj) 
                {
                    if (value > 0.0d)
                    {
                        this._calPowFactor = value;
                    }
                    else
                    {
                        this._calPowFactor = 1.0d;
                    }
                } 
            }
        }

        public LaserCalcSetting SettingData
        {
            get { return this._setting; }
            set { lock (this._lockObj) { this._setting = value; } }
        }

        public LaserCharacteristicsData CharacteristicResults
        {
            get { return this._result; }
            set { lock (this._lockObj) { this._result = value; } }
        }

        public LaserCurveData Curve
        {
            get { return this._curve; }
        }

        #endregion

        #region >>> Private Method <<<

        private bool LoadKinkRefCurve(string path)
        {
            this._refIkinkCurve.Clear();
            this._refPkinkCurve.Clear();

            List<string[]> tempList = CSVUtil.ReadCSV(path, "Current(A)", false, true);

            if (tempList == null)
            {
                Console.WriteLine("[MpiLaserPostCalc], Load refKink Fail.");
                return false;
            }

            double curr = 0.0d;
            double pow = 0.0d;

            try
            {
                foreach (var row in tempList)
                {
                    if (!double.TryParse(row[0], out curr) || !double.TryParse(row[1], out pow))
                    {
                        Console.WriteLine("[MpiLaserPostCalc], Load refKink data Parse Fail.");
                        return false;
                    }

                    this._refIkinkCurve.Add(curr);
                    this._refPkinkCurve.Add(pow);

                }
            }
            catch
            {
                Console.WriteLine("[MpiLaserPostCalc], Load refKink Exception");
                return false;
            }

            return true;
        }

        private double CalcSlopeEfficiency(double[] currArray, double[] powArray, ELaserCalcMode mode, int startIdx, int endIdx)
        {
            double se = 0.0d;

            switch (mode)
            {
                case ELaserCalcMode.TwoPointsDifference:
                    {
                        se = LaserCharacteristicAnalysis.SlopeEfficiencyDiff(currArray, powArray, startIdx, endIdx);

                        break;
                    }
                case ELaserCalcMode.LinearRegression:
                    {
                        se = LaserCharacteristicAnalysis.SlopeEfficiencyLR(currArray, powArray, startIdx, endIdx);

                        break;
                    }
                case ELaserCalcMode.Average:
                    {
                        se = LaserCharacteristicAnalysis.SlopeEfficiencyAvg(currArray, powArray, startIdx, endIdx);

                        break;
                    }
            }

            return Math.Round(se, 9, MidpointRounding.AwayFromZero);
        }

        private double CalcThresholdCurrent(double[] currArray, double[] powArray, ELaserCalcMode mode, int startIdx, int endIdx, double thrValue, double thrValue2)
        {
            double ith = 0.0d;

            double slope = 0.0d;

            switch (mode)
            {
                case ELaserCalcMode.TwoPointsDifference:
                    {
                        slope = LaserCharacteristicAnalysis.SlopeEfficiencyDiff(currArray, powArray, startIdx, endIdx);

                        ith = LaserCharacteristicAnalysis.ThresholdCurrentDiff(currArray, powArray, slope, startIdx, endIdx);

                        break;
                    }
                case ELaserCalcMode.LinearRegression:
                    {
                        slope = LaserCharacteristicAnalysis.SlopeEfficiencyDiff(currArray, powArray, startIdx, endIdx);

                        ith = LaserCharacteristicAnalysis.ThresholdCurrentLR(currArray, powArray, slope, startIdx, endIdx);

                        break;
                    }
                case ELaserCalcMode.ThresholdValue:
                    {
                        ith = LaserCharacteristicAnalysis.ThresholdCurrentSe(currArray, powArray, thrValue, startIdx, endIdx);
                        
                        break;
                    }
                case ELaserCalcMode.ThresholdValue2:
                    {
                        // required power array (no moving average)
                        ith = LaserCharacteristicAnalysis.ThresholdCurrentPo(currArray, powArray, thrValue2, startIdx, endIdx);

                        break;
                    }
            }

            return Math.Round(ith, 9, MidpointRounding.AwayFromZero);
        }

        private double CalcResistance(double[] currArray, double[] voltArray, ELaserCalcMode mode, int startIdx, int endIdx)
        {
            double rs = 0.0d;

            switch (mode)
            {
                case ELaserCalcMode.TwoPointsDifference:
                    {
                        rs = LaserCharacteristicAnalysis.ResistanceDiff(currArray, voltArray, startIdx, endIdx);

                        break;
                    }
                case ELaserCalcMode.LinearRegression:
                    {
                        rs = LaserCharacteristicAnalysis.ResistanceLR(currArray, voltArray, startIdx, endIdx);

                        break;
                    }
                case ELaserCalcMode.Average:
                    {
                        rs = LaserCharacteristicAnalysis.ResistanceAvg(currArray, voltArray, startIdx, endIdx);

                        break;
                    }
            }

            return Math.Round(rs, 6, MidpointRounding.AwayFromZero);
        }

        private double CalcLinearity(double[] currArray, double[] powArray, double[] seArray, int startIdx, int endIdx)
        {
            return LaserCharacteristicAnalysis.Linearity(currArray, powArray, seArray, startIdx, endIdx);
        }

        private double CalcRollOver(double[] currArray, double[] powArray, double se, double ith, double iroll)
        {
            return LaserCharacteristicAnalysis.RollOver(currArray, powArray, se, ith, iroll);
        }

        private double[] CalcPceSeries(double[] currArray, double[] voltArray, double[] powArray)
        {
            double[] pceArray = new double[currArray.Length];

            for (int i = 0; i < currArray.Length; i++)
            {
                pceArray[i] = LaserCharacteristicAnalysis.PCE(currArray[i], voltArray[i], powArray[i]);
            }

            return pceArray;
        }

        private bool FindKink(ELaserKinkCalcMode mode, double[] currArray, double[] powArray, double[] seArray, double ratio, int startIdx, int endIdx, ref double kink, ref double Ppk, ref double Ipk)
        {
            bool rtn = false;
            
            switch (mode)
            {
                case ELaserKinkCalcMode.SEk:
                    {
                        rtn = LaserCharacteristicAnalysis.KinkBySEk(currArray, powArray, seArray, ratio, startIdx, endIdx, ref kink, ref Ppk, ref Ipk);

                        kink = kink * 100.0d;

                        break;
                    }
                case ELaserKinkCalcMode.RefCurve:
                    {
                        rtn = LaserCharacteristicAnalysis.KinkByRefCurve(currArray, powArray, this._refIkinkCurve.ToArray(), this._refPkinkCurve.ToArray(), startIdx, endIdx, ref kink, ref Ppk, ref Ipk);

                        kink = kink * 100.0d;

                        break;
                    }
                case ELaserKinkCalcMode.FittingLine:
                    {
                        rtn = LaserCharacteristicAnalysis.KinkByFittingLine(currArray, powArray, seArray, ratio, startIdx, endIdx, ref kink, ref Ppk, ref Ipk);

                        kink = kink * 100.0d;

                        break;
                    }
                case ELaserKinkCalcMode.DeltaPow:
                    {
                        rtn = LaserCharacteristicAnalysis.KinkByDeltaPow(currArray, powArray, ratio, startIdx, endIdx, ref kink, ref Ppk, ref Ipk);

                        kink = kink * 100.0d;

                        break;
                    }
                case ELaserKinkCalcMode.SecondOrder:
                    {
                        rtn = LaserCharacteristicAnalysis.KinkBySecondOrderDiff(currArray, powArray, seArray, startIdx, endIdx, ref kink, ref Ppk, ref Ipk);

                        break;
                    }
            }

            return rtn;  
        }

        //private double CalcPceByDefindPoint(double[] currArray, double[] voltArray, double[] powArray, double ipce)
        //{
        //    double pce = 0.0d;

        //    int index = 0;

        //    if (LaserCharacteristicAnalysis.SerachPointIndex(ipce, currArray, out index))
        //    {
        //        double po = 0.0d;
        //        double vo = 0.0d;

        //        po = powArray[index];
        //        vo = voltArray[index];

        //        if (ipce * vo > 0)
        //        {
        //            pce = (po / 1000.0d) / (ipce * vo) * 100.0d;
        //        }
        //    }

        //    return pce;
        //}

        //private double[] CalcKink(double[] currArray, double[] powArray, double ith, double se, double[] seArray, double ratioRange, int startIdx, int endIdx)
        //{
        //    // [0] powAtkink, [1] currArKink, [2] ratioAtKink,
        //    double[] kinkCalcResult = new double[3];

        //    double[] seFitting = LaserCharacteristicAnalysis.MovingAverage(seArray, seArray.Length / 5);

        //    double[] ratioSet = new double[powArray.Length];

        //    for (int i = 0; i < seArray.Length; i++)
        //    {
        //        if (seFitting[i] == 0)
        //        {
        //            ratioSet[i] = 1.0d;
        //        }
        //        else
        //        {
        //            ratioSet[i] = ((seArray[i] - seFitting[i]) / seArray[i]) + 1.0d;
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

        //    for (int i = 0; i < seArray.Length; i++)
        //    {
        //        if (seArray[i] > 0 && currArray[i] > ith)
        //        {
        //            seMaxIndex = i;
        //            break;
        //        }
        //    }

        //    double stepSE = seArray[seMaxIndex];
        //    double stepPow = powArray[seMaxIndex];
        //    double stepRatio = seArray[seMaxIndex] / se;
        //    double kOut = (ratioSet.Max() - 1.0d) * 100.0d;

        //    return kinkCalcResult;
        //}

        private void ApplyGainOffset(ref double[] powArray, ref double[] CurrArray, ref double[] voltArray)
        {
            if (powArray == null || CurrArray == null || voltArray == null)
            {
                return;
            }
            
            for (int i = 0; i < powArray.Length; i++)
            {
                if (this._setting.GainPower != 0.0d)
                {
                    powArray[i] = powArray[i] * this._setting.GainPower + this._setting.OffsetPower;
                }

                if (this._setting.GainCurrent != 0.0d)
                {
                    CurrArray[i] = CurrArray[i] * this._setting.GainCurrent + this._setting.OffsetCurrent;
                }

                if (this._setting.GainVoltage != 0.0d)
                {
                    voltArray[i] = voltArray[i] * this._setting.GainVoltage + this._setting.OffsetVoltage;
                }
            }
        }

        #endregion

        #region >>> Public Method <<<

        public bool CalcParameter(double[] powRawArray, double[] currRawArray, double[] voltRawArray)
        {
            double[] pArray;
            double[] vArray;
            double[] iArray;

            bool isFoundIndex;

            int seStartIdx = 0;
            int seEndIdx = 0;
            int se2StartIdx = 0;
            int se2EndIdx = 0;
            int sethStartIdx = 0;
            int sethEndIdx = 0;
            int rsStartIdx = 0;
            int rsEndIdx = 0;
            int lnStartIdx = 0;
            int lnEndIdx = 0;
            int kinkStartIdx = 0;
            int kinkEndIdx = 0;

            double pop = 0.0d;
            double po = 0.0d;
            double iop = 0.0d;
            double vop = 0.0d;

            double pth = 0.0d;
            double ith = 0.0d;
            double vth = 0.0d;

            double se = 0.0d;
            double se2 = 0.0d;
            double rs = 0.0d;

            double iroll = 0.0d;
           
            double kink = 0.0d;
            double pkink = 0.0d;
            double ikink = 0.0d;
            double kinkRatio = 0.0d;

            double icod = 0.0d;
            double pcod = 0.0d;

            double ifx = 0.0d;
            double vfx = 0.0d;
            double pfx = 0.0d;
            double rdx = 0.0d;

            pop = this._setting.Pop;

            iroll = this._setting.Iroll;

            this._result.Clear();

            if (pop <= 0)
            {
                return false;
            }

            try
            {
                this._result.Pop = pop;  // mW

                //-------------------------------------------------------------------------------------------------------
                // Apply Curve Gain / Offset
                this.ApplyGainOffset(ref powRawArray, ref currRawArray, ref voltRawArray);

                //-------------------------------------------------------------------------------------------------------
                // Moving Average
                pArray = LaserCharacteristicAnalysis.MovingAverage(powRawArray, this._setting.PowMovingAverageWindow);

                vArray = LaserCharacteristicAnalysis.MovingAverage(voltRawArray, this._setting.VoltMovingAverageWindow);

                iArray = currRawArray.Clone() as double[];

                //-------------------------------------------------------------------------------------------------------
                // Find Ppk (maximum in power array) & Ipk (the current at Ppk)
                this._result.Ppk = powRawArray.Max();  // mW

                int indexOfPpk = Array.IndexOf(powRawArray, this._result.Ppk);

                this._result.Ipk = iArray[indexOfPpk];
                this._result.Vpk = vArray[indexOfPpk];
                this._result.Impk = Math.Round((this._result.Ppk / 1000.0d / this._calPowFactor), 6, MidpointRounding.AwayFromZero); // (1) mW -> W  (2) Im = (W / Factor), unit: A


                double[] pceArray = this.CalcPceSeries(iArray, voltRawArray, powRawArray);   // PCE Curve

                this._result.Pcepk = pceArray.Max();  // Pce Max

                this._curve.AddPceData(pceArray);

                //-------------------------------------------------------------------------------------------------------
                // Laser Diode 特性, 指定區間
                // RS Section
                double[] rsArray = null;
                double[] rsCurve = null;

                isFoundIndex = true;

                if (this._setting.RsSearchMode == ELaserSearchMode.byPower)
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.RsSectionLowLimitP, this._setting.RsSectionUpperLimitP, pArray, out rsStartIdx, out rsEndIdx);
                }
                else
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.RsSectionLowLimitI, this._setting.RsSectionUpperLimitI, iArray, out rsStartIdx, out rsEndIdx);
                }

                // Resistance Series (特性計算--> 最小平方法, 圖表表示 --> 一次微分 + Moving Avg.)
                rsArray = LaserCharacteristicAnalysis.ForwardDifference(iArray, vArray);

                rsCurve = LaserCharacteristicAnalysis.MovingAverage(rsArray, this._setting.VoltMovingAverageWindow);

                this._curve.AddRsData(rsCurve);
                    
                if (isFoundIndex && rsStartIdx < rsEndIdx) 
                {
                    rs = this.CalcResistance(iArray, vArray, this._setting.RsCalcMode, rsStartIdx, rsEndIdx);
                }

                this._result.Rs = rs;
         
                //-------------------------------------------------------------------------------------------------------
                // find Vf, Pf, Pce form setting current 
                // At point "a"
                ifx = this._setting.IfA;
                LaserCharacteristicAnalysis.SearchPointByInterpolation(iArray, voltRawArray, powRawArray, rsArray, ifx, out pfx, out vfx, out rdx);
                this._result.VfA = vfx;
                this._result.PfA = pfx;
                this._result.RdA = rdx;
                this._result.PceA = LaserCharacteristicAnalysis.PCE(ifx, vfx, pfx);

                // At point "b"
                ifx = this._setting.IfB;
                LaserCharacteristicAnalysis.SearchPointByInterpolation(iArray, voltRawArray, powRawArray, rsArray, ifx, out pfx, out vfx, out rdx);
                this._result.VfB = vfx;
                this._result.PfB = pfx;
                this._result.RdB = rdx;
                this._result.PceB = LaserCharacteristicAnalysis.PCE(ifx, vfx, pfx);

                // At point "c"
                ifx = this._setting.IfC;
                LaserCharacteristicAnalysis.SearchPointByInterpolation(iArray, voltRawArray, powRawArray, rsArray, ifx, out pfx, out vfx, out rdx);
                this._result.VfC = vfx;
                this._result.PfC = pfx;
                this._result.RdC = rdx;
                this._result.PceC = LaserCharacteristicAnalysis.PCE(ifx, vfx, pfx);

                //-------------------------------------------------------------------------------------------------------
                // SE Curve
                double[] seArray = LaserCharacteristicAnalysis.ForwardDifference(iArray, pArray);

                //double[] seCurve = LaserCharacteristicAnalysis.MovingAverage(seArray, this._setting.PowMovingAverageWindow);  // mW/A

                double[] seCurve = new double[seArray.Length];

                for (int i = 0; i < seCurve.Length; i++)
                {
                    seCurve[i] = seArray[i] / 1000.0d; // W/A

                    seCurve[i] = seCurve[i] >= 0.0d ? seCurve[i] : 0.0d;
                }

                this._curve.AddSeData(seCurve);

                //-------------------------------------------------------------------------------------------------------
                // Slope Efficiency (特性計算--> 最小平方法, 圖表表示 --> 一次微分 + Moving Avg.)
                // SE Section
                isFoundIndex = true;

                if (this._setting.SeSearchMode == ELaserSearchMode.byPower)
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.SeSectionLowLimitP, this._setting.SeSectionUpperLimitP, pArray, out seStartIdx, out seEndIdx);
                }
                else
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.SeSectionLowLimitI, this._setting.SeSectionUpperLimitI, iArray, out seStartIdx, out seEndIdx);
                }

                if (isFoundIndex && seStartIdx < seEndIdx)
                {
                se = this.CalcSlopeEfficiency(iArray, pArray, this._setting.SeCalcMode, seStartIdx, seEndIdx);  // mW/A
                }

                this._result.SE = se / 1000.0d;   // W/A

                //-------------------------------------------------------------------------------------------------------
                // Slope Efficiency 2 (特性計算--> 最小平方法, 圖表表示 --> 一次微分 + Moving Avg.)
                // SE2 Section
                isFoundIndex = true;

                if (this._setting.Se2SearchMode == ELaserSearchMode.byPower)
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.Se2SectionLowLimitP, this._setting.Se2SectionUpperLimitP, pArray, out se2StartIdx, out se2EndIdx);
                }
                else
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.Se2SectionLowLimitI, this._setting.Se2SectionUpperLimitI, iArray, out se2StartIdx, out se2EndIdx);
                }

                if (isFoundIndex && se2StartIdx < se2EndIdx)
                {
                se2 = this.CalcSlopeEfficiency(iArray, pArray, this._setting.SeCalcMode, se2StartIdx, se2EndIdx);  // mW/A
                }

                this._result.SE2 = se2 / 1000.0d;   // W/A

                //-------------------------------------------------------------------------------------------------------
                // Threshold Current / Voltage / Power
                isFoundIndex = true;

                if (this._setting.ThresholdSearchMode == ELaserSearchMode.byPower)
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.ThresholdSectionLowLimitP, this._setting.ThresholdSectionUpperLimitP, pArray, out sethStartIdx, out sethEndIdx);
                }
                else
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.ThresholdSectionLowLimitI, this._setting.ThresholdSectionUpperLimitI, iArray, out sethStartIdx, out sethEndIdx);
                }

                if (isFoundIndex && sethStartIdx < sethEndIdx)
                {
                ith = this.CalcThresholdCurrent(iArray, pArray, this._setting.ThresholdCalcMode, sethStartIdx, sethEndIdx, this._setting.ThresholdSearchValue, this._setting.ThresholdSearchValue2);  // A
                }

                LaserCharacteristicAnalysis.ThresholdPointByInterpolation(iArray, voltRawArray, powRawArray, ith, out pth, out vth);

                this._result.Ith = ith;  // A
                this._result.Pth = pth;  // mW
                this._result.Vth = vth;  // V

                //-------------------------------------------------------------------------------------------------------
                // Calc Iop & Vop (Linear interpolation)
                if (this._setting.OperationPointSelection == ELaserPointSelectMode.ClosestPoint)
                {
                    LaserCharacteristicAnalysis.OperationPointByClosestPoint(iArray, vArray, pArray, pop, out iop, out vop, out po);
                }
                else if (this._setting.OperationPointSelection == ELaserPointSelectMode.OnFittingLine)
                {
                    int index = 0;
                    double v1 = 0.0d;
                    double i1 = this._setting.RsSectionLowLimitI;
                    
                    if (LaserCharacteristicAnalysis.SearchPointIndex(i1, iArray, out index))
                    {
                        v1 = vArray[index];

                        LaserCharacteristicAnalysis.OperationPointOnFittingLine(ith, se, rs, i1, v1, pop, out iop, out vop, out po);
                    }
                }
                else
                {
                    LaserCharacteristicAnalysis.OperationPointByInterpolation(iArray, vArray, pArray, pop, out iop, out vop, out po);
                }

                this._result.Iop = iop;
                this._result.Vop = vop;
                this._result.Imop = Math.Round((po / 1000.0d / this._calPowFactor), 6, MidpointRounding.AwayFromZero);  // (1) mW -> W  (2) Im = (W / Factor), unit: A
                this._result.Pceop = LaserCharacteristicAnalysis.PCE(iop, vop, pop);

                //-------------------------------------------------------------------------------------------------------
                // Rollover
                this._result.Rollover = this.CalcRollOver(iArray, pArray, se, ith, this._setting.Iroll); ; // %;

                //-------------------------------------------------------------------------------------------------------
                // Linearity (指定區間內, (SlopeMax - SlopeMin) / 線性迴歸算出的 Slope )
                isFoundIndex = true;

                if (this._setting.LnSearchMode == ELaserSearchMode.byPower)
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.LnSectionLowLimitP, this._setting.LnSectionUpperLimitP, pArray, out lnStartIdx, out lnEndIdx);
                }
                else
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(this._setting.LnSectionLowLimitI, this._setting.LnSectionUpperLimitI, iArray, out lnStartIdx, out lnEndIdx);
                }

                if (isFoundIndex && lnStartIdx < lnEndIdx)
                {
                    this._result.Linearity = this.CalcLinearity(iArray, pArray, seArray, lnStartIdx, lnEndIdx);  // %
                }

                if (se != 0.0d)
                {
                this._result.Linearity2 = (se2 / se) * 100.0d;
                }

                //-------------------------------------------------------------------------------------------------------
                // Icod & Pcod
                LaserCharacteristicAnalysis.COD(iArray, powRawArray, seArray, ith, ref icod, ref pcod);
                this._result.Icod = icod;
                this._result.Pcod = pcod;

                //-------------------------------------------------------------------------------------------------------
                // Kink  [0] powAtkink, [1] currArKink, [2] ratioAtKink,
                // double[] kink = this.CalcKink(iArray, pArray, ith, se, seSet, 0.05d, kinkStartIdx, kinkEndIdx);
                isFoundIndex = true;

                double kinkLowLimitI = ith + this._setting.KinkSectionLowLimitI;
                double kinkUpperLimitI = this._setting.KinkSectionUpperLimitI;

                if (kinkLowLimitI < kinkUpperLimitI)
                {
                    isFoundIndex &= LaserCharacteristicAnalysis.SearchPointIndex(kinkLowLimitI, kinkUpperLimitI, iArray, out kinkStartIdx, out kinkEndIdx);

                    kinkRatio = this._setting.KinkRatio;

                    if (isFoundIndex && kinkStartIdx < kinkEndIdx)
                    {
                    if (!this.FindKink(this._setting.KinkCalcMode, iArray, powRawArray, seArray, kinkRatio, kinkStartIdx, kinkEndIdx, ref kink, ref pkink, ref ikink))
                    {
                        // if Kink(%) < ratio (No Kink, Ikink = Ipk, Pkink = Ppk)
                        pkink = this._result.Ppk;
                        ikink = this._result.Ipk;
                        }
                    }
                }
                else
                {
                    kink = 100.0d;
                }

                this._result.Pkink = pkink;
                this._result.Ikink = ikink;
                this._result.Kink = kink;

                //-------------------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                this._result.Clear();

                Console.WriteLine(string.Format("[MpiLaserPostCalc], CalcParameter, Exception Msg. = {0}", ex.Message));

                return false;
            }

            return true;
        }

        #endregion
    }
}
