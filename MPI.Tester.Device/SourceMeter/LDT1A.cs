using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SourceMeter
{
    public class LDT1A : ISourceMeter
    {
        public const uint PMU_COUNT = 2;
        public const uint PULSE_COUNT = 2;

        private const uint MAX_SWEEP_COUNT = 100001;
        private const uint MIN_SWEEP_COUNT = 2;
        private const double DEFAULT_TURN_OFF_DELAY = 0.5;          // ms 0.25
        private const double DEFAULT_MSRT_DELAY = 0.25;             // ms 0.5

        private object _lockObj;

        private static double[][] _voltRange = new double[][]	// [ PMU Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 4.0d,        8.0d }, 
													new double[] { 50.0d } 
												};

        private static double[][] _currRange = new double[][]  // [ PMU Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 0.000004d,   0.00004d,       0.0004d,    0.004d,    0.04d,    0.4d,     0.8d },    
													new double[] { 0.00004d } 
												};

        private WheWrapper2 _ldt1a;

        private SourceMeterConfigData _meterConfig;

        private string _hwVersion;
        private string _swVersion;
        private string _serialNum;
        private EDevErrorNumber _errorNum;

        private double _turnOffDelay;
        private double _msrtDelay;

        private ElectSettingData[] _setting;			//  [ Setting Item Index ]
        //private double[][] _sweepOutput;			    //  [ Setting Item Index ] [ sweep point ]  
        private double[][] _applyData;				//  [ Setting Item Index ] [ raw data or sweep data length ] 
        private double[][] _acquireData;				//  [ Setting Item Index ] [ raw data or sweep data length ] 
        private double[][] _sweepResult;			    //  [ Setting Item Index ] [ sweep result item ] , sweep mode , point = 2 ~ *
        private double[][] _timeChain;
        private int[][] _sweepForceRangeIndex;			//  [ Setting Item Index ] [ range index or range indexs of sweep ] , sweep mode , point = 2 ~ *

        private MPI.PerformanceTimer _pt;
        private MeterStatus _status;
        private List<double> _msrtDataList;

        private SourceMeterSpec _spec;

        public LDT1A()
        {
            this._lockObj = new object();

            this._ldt1a = new WheWrapper2();

            this._hwVersion = "HW NONE";
            this._swVersion = "SW NONE";
            this._serialNum = "SN NONE";
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this._turnOffDelay = 0.5; // mike1221
            this._msrtDelay = 0.0d;

            this._setting = null;
            this._acquireData = null;
            this._sweepForceRangeIndex = null;
            this._applyData = null;

            this._pt = new PerformanceTimer();
            this._status = new MeterStatus();
            this._msrtDataList = new List<double>(WheWrapper2.MAX_MSRT_AVG_COUNT);

            this._spec = new SourceMeterSpec();
        }

        public LDT1A(SourceMeterConfigData config) : this()
        {
            this._meterConfig = config;
        }

        #region >>> Public Property <<<

        public string SerialNumber
        {
            get { return ("1A-" + this._serialNum); }
        }

        public string HardwareVersion
        {
            get { return this._hwVersion; }
        }

        public string SoftwareVersion
        {
            get { return this._swVersion; }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return this._errorNum; }
        }

        public double TurnOffDelay
        {
            get { return this._turnOffDelay; }
            set
            {
                // unit in mSec, Resolution 200nS
                lock (this._lockObj)
                {
                    if (value <= DEFAULT_TURN_OFF_DELAY)
                    {
                        this._turnOffDelay = DEFAULT_TURN_OFF_DELAY;
                    }
                    else
                    {
                        this._turnOffDelay = value;
                    }
                }
            }
        }


        public ElectSettingData[] ElecSetting
        {
            get
            {
                if (this._setting == null)
                {
                    return null;
                }

                ElectSettingData[] data = new ElectSettingData[this._setting.Length];

                for (int i = 0; i < this._setting.Length; i++)
                {
                    data[i] = this._setting[i].Clone() as ElectSettingData;
                }

                return data;
            }
        }

        public SourceMeterSpec Spec
        {
            get { return this._spec; }
        }

        #endregion

        #region >>> Private Method <<<

        private int FindCurrentIndex(uint pmuIndex, double current)
        {
            int index = 0;
            double deltaValue = 0.0d;

            if (pmuIndex >= _currRange.Length)
                return -1;

            for (index = 0; index < _currRange[pmuIndex].Length; index++)
            {
                deltaValue = Math.Abs(current) - _currRange[pmuIndex][index];
                if (deltaValue < 0.0d || Math.Abs(deltaValue) <= Double.Epsilon)
                    break;
            }

            if (index == _currRange[pmuIndex].Length)
            {
                return -1;
            }
            else
            {
                return index;
            }
        }

        private int FindVoltageIndex(uint pmuIndex, double voltage)
        {
            int index = 0;
            double deltaValue = 0.0d;

            if (pmuIndex >= _voltRange.Length)
                return -1;

            for (index = 0; index < _voltRange[pmuIndex].Length; index++)
            {
                deltaValue = Math.Abs(voltage) - _voltRange[pmuIndex][index];

                if (deltaValue < 0.0d || Math.Abs(deltaValue) <= Double.Epsilon)
                    break;
            }

            if (index == _voltRange[pmuIndex].Length)
            {
                return -1;
            }
            else
            {
                return index;
            }
        }

        private bool FindPMUIndex(ElectSettingData setting)
        {
            bool[] forcePMUArray = new bool[PMU_COUNT];
            bool[] msrtPMUArray = new bool[PMU_COUNT];
            int[] forceIndexArray = new int[PMU_COUNT];
            int[] msrtIndexArray = new int[PMU_COUNT];

            for (uint pmuIndex = 0; pmuIndex < PMU_COUNT; pmuIndex++)
            {
                switch (setting.MsrtType)
                {
                    case EMsrtType.MV:
                        forcePMUArray[pmuIndex] = true;
                        msrtIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------
                    case EMsrtType.FI:
                    case EMsrtType.FIMV:
                    case EMsrtType.POLAR:
                    case EMsrtType.LIV:
                        forceIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.ForceValue);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------
                    case EMsrtType.MI:
                        forceIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.ForceValue);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------
                    case EMsrtType.FV:
                    case EMsrtType.FVMI:
                        forceIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.ForceValue);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------
                    case EMsrtType.THY:
                    case EMsrtType.FIMVSWEEP:
                        forceIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.SweepStop);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------
                    case EMsrtType.FVMISWEEP:
                        forceIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.SweepStop);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------
                    default:
                        forcePMUArray[pmuIndex] = false;
                        msrtPMUArray[pmuIndex] = false;
                        forceIndexArray[pmuIndex] = -1;
                        msrtIndexArray[pmuIndex] = -1;
                        break;
                }
            }

            uint i = 0;
            for (i = 0; i < PMU_COUNT; i++)
            {
                if (forcePMUArray[i] == true && msrtPMUArray[i] == true)
                    break;
            }

            if (i == PMU_COUNT)
            {
                return false;
            }
            else
            {
                setting.PUMIndex = i;
                setting.ForceRangeIndex = forceIndexArray[i];
                setting.MsrtRangeIndex = msrtIndexArray[i];
                return true;
            }
        }

        private bool CheckClampAndMsrtRange(ElectSettingData setting)
        {
            if (Math.Abs(setting.MsrtProtection) > Math.Abs(setting.MsrtRange))
                return false;

            return true;
        }

        //private int[] FindSweepForceRangIndex(ElectSettingData setting, double[] sweepPoint)
        //{
        //    double[] forceRangeArray = null;
        //    int[] forceRangeIndex;				// for single ElecSettingData   
        //    double difference;
        //    //-------------------------------------------------------------------------------------------------------
        //    // Get the force range of the meter hardware setting ( voltage or current )
        //    //-------------------------------------------------------------------------------------------------------
        //    if (setting.MsrtType == EMsrtType.FI || setting.MsrtType == EMsrtType.FIMV ||
        //            setting.MsrtType == EMsrtType.FIMVSWEEP || setting.MsrtType == EMsrtType.THY)
        //    {
        //        if (setting.PUMIndex > _currRange.Length)
        //        {
        //            this._errorNum = EDevErrorNumber.PMUSetting_Err;
        //            return null;
        //        }
        //        else
        //        {
        //            forceRangeArray = _currRange[setting.PUMIndex];
        //        }
        //    }
        //    else if (setting.MsrtType == EMsrtType.FV || setting.MsrtType == EMsrtType.FVMI ||
        //                  setting.MsrtType == EMsrtType.FVMISWEEP)
        //    {
        //        if (setting.PUMIndex > _voltRange.Length)
        //        {
        //            this._errorNum = EDevErrorNumber.PMUSetting_Err;
        //            return null;
        //        }
        //        else
        //        {
        //            forceRangeArray = _voltRange[setting.PUMIndex];
        //        }
        //    }

        //    //-------------------------------------------------------------------------------------
        //    // calculate the range index setting for the  "ElecSettingData"
        //    //-------------------------------------------------------------------------------------
        //    if (setting.MsrtType == EMsrtType.FIMVSWEEP || setting.MsrtType == EMsrtType.FVMISWEEP || setting.MsrtType == EMsrtType.THY)
        //    {
        //        forceRangeIndex = new int[setting.SweepRiseCount + setting.SweepContCount + 1];			// include the sweep start point
        //        for (int i = 0; i < forceRangeIndex.Length; i++)             // sweep force point
        //        {
        //            for (int j = 0; j < forceRangeArray.Length; j++)       // range of meter
        //            {
        //                difference = (Math.Abs(sweepPoint[i]) - forceRangeArray[j]);
        //                if (difference < 0.0d || Math.Abs(difference) < Double.Epsilon)
        //                {
        //                    forceRangeIndex[i] = j;  // forece range for each sweep point 
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else if (setting.MsrtType == EMsrtType.FI || setting.MsrtType == EMsrtType.FIMV || setting.MsrtType == EMsrtType.FV || setting.MsrtType == EMsrtType.FVMI)
        //    {
        //        forceRangeIndex = new int[1];
        //        for (int j = 0; j < forceRangeArray.Length; j++)             // range of meter
        //        {
        //            difference = Math.Abs(setting.ForceValue) - forceRangeArray[j];
        //            if (difference < 0.0d || Math.Abs(difference) < Double.Epsilon)
        //            {
        //                forceRangeIndex[0] = j;      // single range setting for force value
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        forceRangeIndex = new int[1];
        //    }

        //    return forceRangeIndex;
        //}

        private bool CalcSweepForceValue(ElectSettingData setting)
        {
            double[] sweepPoint = null;
            uint totalCount = setting.SweepRiseCount + setting.SweepContCount + 1;		// include the sweep start point

            if (totalCount > MAX_SWEEP_COUNT)
            {
                return false;
            }

            if (setting.MsrtType == EMsrtType.FIMVSWEEP || setting.MsrtType == EMsrtType.FVMISWEEP || setting.MsrtType == EMsrtType.THY)
            {
                switch (setting.SweepMode)
                {
                    case ESweepMode.Linear:
                        sweepPoint = new double[totalCount];
                        for (uint i = 0; i <= setting.SweepRiseCount; i++)			// include sweep start point and rise count
                        {
                            sweepPoint[i] = setting.SweepStart + setting.SweepStep * i;
                        }

                        for (uint j = setting.SweepRiseCount + 1; j < totalCount; j++)
                        {
                            sweepPoint[j] = sweepPoint[setting.SweepRiseCount];
                        }
                        break;
                    //------------------------------------------------------------------------------------
                    case ESweepMode.Log:
                        {
                        }
                        break;
                    //------------------------------------------------------------------------------------
                    case ESweepMode.Custom:
                        break;
                    //------------------------------------------------------------------------------------
                    default:
                        break;
                }
            }
            else
            {
                return true;
            }

            setting.SweepCustomValue = sweepPoint;
            return true;
        }

        private int[] CalcSweepForceRangIndex(ElectSettingData setting)
        {
            double[] forceRangeArray = null;
            int[] forceRangeIndex;				// for single ElecSettingData   
            int rangeIndex = 0;
            double deltaValue;
            //-------------------------------------------------------------------------------------------------------
            // Get the force range of the meter hardware setting ( voltage or current )
            //-------------------------------------------------------------------------------------------------------
            if (setting.MsrtType == EMsrtType.FIMVSWEEP || setting.MsrtType == EMsrtType.THY)
            {
                if (setting.PUMIndex > _currRange.Length)
                {
                    this._errorNum = EDevErrorNumber.PMUSetting_Err;
                    return null;
                }
                else
                {
                    forceRangeArray = _currRange[setting.PUMIndex];
                }
            }
            else if (setting.MsrtType == EMsrtType.FVMISWEEP)
            {
                if (setting.PUMIndex > _voltRange.Length)
                {
                    this._errorNum = EDevErrorNumber.PMUSetting_Err;
                    return null;
                }
                else
                {
                    forceRangeArray = _voltRange[setting.PUMIndex];
                }
            }
            else
            {
                return null;
            }

            //-------------------------------------------------------------------------------------
            // calculate the range index setting for the  "ElecSettingData"
            //-------------------------------------------------------------------------------------
            forceRangeIndex = new int[setting.SweepRiseCount + setting.SweepContCount + 1];			// include the sweep start point
            for (int i = 0; i < forceRangeIndex.Length; i++)             // sweep force point
            {
                for (rangeIndex = 0; rangeIndex < forceRangeArray.Length; rangeIndex++)       // range of meter
                {
                    deltaValue = (Math.Abs(setting.SweepCustomValue[i]) - forceRangeArray[rangeIndex]);
                    if (deltaValue < 0.0d || Math.Abs(deltaValue) < Double.Epsilon)
                    {
                        forceRangeIndex[i] = rangeIndex;  // forece range for each sweep point 
                        break;
                    }
                }

                if (rangeIndex == forceRangeArray.Length)
                    return null;
            }

            return forceRangeIndex;
        }

        private void FIMVSweepMsrt(uint itemIndex)
        {
            int pmuPolrity = 0;
            double outputValue = 0.0d;
            bool rtn = true;

            this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] = 0.0d;
            this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak] = 0.0d;

            for (int i = 0; i < this._setting[itemIndex].SweepCustomValue.Length && rtn; i++)
            {
                if (this._setting[itemIndex].IsBipolarOutput == true)
                {
                    outputValue = this._setting[itemIndex].SweepCustomValue[i];
                }
                else
                {
                    outputValue = Math.Abs(this._setting[itemIndex].SweepCustomValue[i]);

                    if (this._setting[itemIndex].SweepCustomValue[i] < 0)
                    {
                        pmuPolrity = 1;   // POSITIVE = 0; NEGATIVE = 1
                    }
                    else
                    {
                        pmuPolrity = 0;
                    }
                }

                if (this._setting[itemIndex].IsAutoForceRange)
                {
                    rtn = this._ldt1a.ForceI((int)this._setting[itemIndex].PUMIndex, pmuPolrity,
                                                             outputValue,
                                                             this._setting[itemIndex].MsrtRangeIndex,		// voltRangeIndex
                                                             this._sweepForceRangeIndex[itemIndex][i],		// currentRangeIndex
                                                             this._setting[itemIndex].ForceTime);
                }
                else
                {
                    rtn = this._ldt1a.ForceI((int)this._setting[itemIndex].PUMIndex, pmuPolrity,
                                                            outputValue,
                                                            this._setting[itemIndex].MsrtRangeIndex,		// voltRangeIndex
                                                            this._setting[itemIndex].ForceRangeIndex,		// currentRangeIndex
                                                            this._setting[itemIndex].ForceTime);
                }

                this._acquireData[itemIndex][i] = this._ldt1a.MeasureV((int)this._setting[itemIndex].PUMIndex, this._msrtDelay + this._setting[itemIndex].ForceTime);

                if (i == 0)
                {
                    this._pt.Start();
                    this._timeChain[itemIndex][i] = 0.0d;
                }
                else
                {
                    this._timeChain[itemIndex][i] = this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                }

                if (((int)this._setting[itemIndex].SweepTurnOffTime) != 0)
                {
                    this._ldt1a.TurnOff(this._turnOffDelay + this._setting[itemIndex].SweepTurnOffTime);
                }

                // Record the Max Peak Value
                if (Math.Abs(this._acquireData[itemIndex][i]) > this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak])
                {
                    this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] = Math.Abs(this._acquireData[itemIndex][i]);
                }

                // Record the Min Peak Value
                if (Math.Abs(this._acquireData[itemIndex][i]) < this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak])
                {
                    this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak] = Math.Abs(this._acquireData[itemIndex][i]);
                }
            }

            this._pt.Stop();
        
        }        

        private void THYIVSweepMsrt(uint itemIndex)
        {
            int pmuPolrity = 0;
            double outputValue = 0.0d;
            bool rtn = true;

            this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] = 0.0d;
            this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak] = 0.0d;
         
            // 21121126 Mike
            // THY 功能,最後已經由 FV -> FI 切換,接下來的操作都是在 FIMV 上.
            // HCFI(0)
            
            /////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < this._setting[itemIndex].SweepCustomValue.Length && rtn; i++)
            {
                if (this._setting[itemIndex].IsBipolarOutput == true)
                {
                    outputValue = this._setting[itemIndex].SweepCustomValue[i];
                }
                else
                {
                    outputValue = Math.Abs(this._setting[itemIndex].SweepCustomValue[i]);

                    if (this._setting[itemIndex].SweepCustomValue[i] < 0)
                    {
                        pmuPolrity = 1;   // POSITIVE = 0; NEGATIVE = 1
                    }
                    else
                    {
                        pmuPolrity = 0;
                    }
                }


                if (this._setting[itemIndex].IsAutoForceRange)
                {
                    rtn = this._ldt1a.ForceI((int)this._setting[itemIndex].PUMIndex, pmuPolrity,
                                                             outputValue,
                                                             this._setting[itemIndex].MsrtRangeIndex,		// voltRangeIndex
                                                             this._sweepForceRangeIndex[itemIndex][i],		// currentRangeIndex
                                                             this._setting[itemIndex].ForceTime);
                }
                else
                {
                    rtn = this._ldt1a.ForceI((int)this._setting[itemIndex].PUMIndex, pmuPolrity,
                                                            outputValue,
                                                            this._setting[itemIndex].MsrtRangeIndex,		// voltRangeIndex
                                                            this._setting[itemIndex].ForceRangeIndex,		// currentRangeIndex
                                                            this._setting[itemIndex].ForceTime);
                }

                this._acquireData[itemIndex][i] = this._ldt1a.MeasureV((int)this._setting[itemIndex].PUMIndex, this._msrtDelay + this._setting[itemIndex].ForceTime);

                if (i == 0)
                {
                    this._pt.Start();
                    this._timeChain[itemIndex][i] = 0.0d;
                }
                else
                {
                    this._timeChain[itemIndex][i] = this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                }

                if (((int)this._setting[itemIndex].SweepTurnOffTime) != 0)
                {
                    this._ldt1a.TurnOff(this._turnOffDelay + this._setting[itemIndex].SweepTurnOffTime);
                }

                // Record the Max Peak Value
                if (Math.Abs(this._acquireData[itemIndex][i]) > this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak])
                {
                    this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] = Math.Abs(this._acquireData[itemIndex][i]);
                }

                // Record the Min Peak Value
                if (Math.Abs(this._acquireData[itemIndex][i]) < this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak])
                {
                    this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak] = Math.Abs(this._acquireData[itemIndex][i]);
                }
            }

            this._pt.Stop();
        }

        private void FindMsrtRangeIndex()
        {
            int i = 0;

            foreach (ElectSettingData setting in this._setting)
            {
                if (setting.MsrtType == EMsrtType.MV || setting.MsrtType == EMsrtType.FIMV || setting.MsrtType == EMsrtType.FIMVSWEEP || setting.MsrtType == EMsrtType.POLAR || setting.MsrtType == EMsrtType.LIV)
                {
                    for (i = 0; i < _voltRange[setting.PUMIndex].Length; i++)
                    {
                        if (((Math.Abs(setting.MsrtRange) - _voltRange[setting.PUMIndex][i]) <= 0.0d) ||
                             (Math.Abs(Math.Abs(setting.MsrtRange) - _voltRange[setting.PUMIndex][i]) <= Double.Epsilon))
                            break;
                    }

                    if (i < _voltRange[setting.PUMIndex].Length)
                    {
                        setting.MsrtRangeIndex = i;
                    }
                    else
                    {
                        setting.MsrtRangeIndex = _voltRange[setting.PUMIndex].Length - 1;
                    }
                }
                else if (setting.MsrtType == EMsrtType.MI || setting.MsrtType == EMsrtType.FVMI || setting.MsrtType == EMsrtType.FVMISWEEP)
                {
                    for (i = 0; i < _currRange[setting.PUMIndex].Length; i++)
                    {
                        if (((Math.Abs(setting.MsrtRange) - _currRange[setting.PUMIndex][i]) <= 0.0d) ||
                             (Math.Abs(Math.Abs(setting.MsrtRange) - _currRange[setting.PUMIndex][i]) <= Double.Epsilon))
                            break;
                    }

                    if (i < _voltRange[setting.PUMIndex].Length)
                    {
                        setting.MsrtRangeIndex = i;
                    }
                    else
                    {
                        setting.MsrtRangeIndex = _voltRange[setting.PUMIndex].Length - 1;
                    }
                }
            }
        }

        private void THYCalculateByTime(uint itemIndex)
        {
            // Calculate the stable value
            int startIndex = this._acquireData[itemIndex].Length - 1;
            int endIndex = startIndex - 20;
            int index = 0;
            double sumStable = 0.0d;

            for (index = startIndex; index >= 0 && index > endIndex; index--)
            {
                sumStable += this._acquireData[itemIndex][index];
            }

            if (index != 0)
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue] = Math.Abs(sumStable) / 20.0d;
            }
            else if (index == 0 && this._acquireData[itemIndex].Length >= 1)
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue] = Math.Abs(sumStable) / ((double)this._acquireData[itemIndex].Length);
            }

            this._sweepResult[itemIndex][(int)ETHYResultItem.MaxToStable] = this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue];
            if (this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] > this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue])
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.OverShoot] = this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue];
            }
            else
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.OverShoot] = 0.0d;
            }
        }

        private void CalcMsrtData(uint itemIndex)
        {

            int index = this._status.MsrtCount - this._status.PulseCount;
            //double avg01 = this._msrtDataList.Average();


            //this._msrtDataList.Sort();

            //if (index < this._msrtDataList.Count && (index + this._status.PulseCount) <= this._msrtDataList.Count)
            //{
            //    this._msrtDataList.RemoveRange(this._status.MsrtCount - this._status.PulseCount, this._status.PulseCount);
            //}

            //if (this._status.PulseCount < this._msrtDataList.Count)
            //{
            //    this._msrtDataList.RemoveRange(0, this._status.PulseCount);
            //}


            for (int i = 0; i < this._status.PulseCount; i++)
            {
                this._msrtDataList.Remove(this._msrtDataList.Max());
                this._msrtDataList.Remove(this._msrtDataList.Min());
            }


            this._msrtDataList.CopyTo(0, this._acquireData[itemIndex], 1, this._acquireData[itemIndex].Length - 1);


            double avg02 = this._msrtDataList.Average();
            //double maxValue = this._msrtDataList.Max();
            //double minValue = this._msrtDataList.Min();
        }

        //private void THYTestCalcultate(uint itemIndex)
        //{
        //    double temp = 0.0d;
        //    int dataCount = this._acquireData[itemIndex].Length;

        //    for (int index = 0; ((index < 10) && (dataCount - index) >= 0); index++)
        //    {
        //        temp += this._acquireData[itemIndex][dataCount - index - 1];
        //    }

        //    this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue] = this._acquireData[itemIndex][this._acquireData[itemIndex].Length - 1];
        //    //this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue] = temp / 10.0d;

        //    this._sweepResult[itemIndex][(int)ETHYResultItem.MaxToStable] = this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue];

        //    if (this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] > this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue])
        //    {
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.OverShoot] = this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue];
        //    }
        //    else
        //    {
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.OverShoot] = 0.0d;
        //    }

        //    int count = this._sweepOutput[itemIndex].Length / 4;

        //    if (count > 2)
        //    {
        //        double[] y = new double[count];
        //        double[] x = new double[count];
        //        double maxValue = 0.0d;

        //        Array.Copy(this._sweepOutput[itemIndex], this._sweepOutput[itemIndex].Length - count, y, 0, count);
        //        Array.Copy(this._acquireData[itemIndex], this._acquireData[itemIndex].Length - count, x, 0, count);

        //        this._simpleLR.Calculate(x, y);

        //        List<double> listData = new List<double>(this._acquireData[itemIndex].Length);

        //        double predictValue = 0.0d;
        //        int maxIndex = 0;
        //        if (this._simpleLR.Slop != 0.0d)
        //        {
        //            for (int i = 0; i < this._acquireData[itemIndex].Length; i++)
        //            {

        //                listData.Add(this._acquireData[itemIndex][i] - (this._sweepOutput[itemIndex][i] - this._simpleLR.Intercept) / this._simpleLR.Slop);
        //                if (listData[i] > maxValue)
        //                {
        //                    maxValue = listData[i];
        //                    maxIndex = i;
        //                }
        //            }

        //            predictValue = (this._sweepOutput[itemIndex][maxIndex] - this._simpleLR.Intercept) / this._simpleLR.Slop;
        //        }
        //        else
        //        {
        //            for (int i = 0; i < this._acquireData[itemIndex].Length; i++)
        //            {
        //                listData.Add(this._acquireData[itemIndex][i] - (this._sweepOutput[itemIndex][i] - this._simpleLR.Intercept));
        //                if (listData[i] > maxValue)
        //                {
        //                    maxValue = listData[i];
        //                    maxIndex = i;
        //                }
        //            }
        //            predictValue = this._sweepOutput[itemIndex][maxIndex] - this._simpleLR.Intercept;
        //        }

        //        this._sweepResult[itemIndex][(int)ETHYResultItem.LineSlop] = this._simpleLR.Slop;
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.LineIntercept] = this._simpleLR.Intercept;
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.ConvexIndex] = maxIndex;
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.VoltConvexPeak] = this._acquireData[itemIndex][maxIndex];
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.VoltConvexLine] = this._acquireData[itemIndex][maxIndex] - maxValue;
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.VoltConvexDiff] = this._acquireData[itemIndex][maxIndex] - maxValue;
        //    }
        //    else
        //    {
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.LineSlop] = 0.0d;
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.LineIntercept] = 0.0d;
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.ConvexIndex] = 0.0d;
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.VoltConvexPeak] = this._acquireData[itemIndex][0];
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.VoltConvexLine] = 0.0d;
        //        this._sweepResult[itemIndex][(int)ETHYResultItem.VoltConvexDiff] = -9999.999d;
        //    }
        //}

        private bool TriggerMeterOut(uint itemIndex)
        {
            int pmuIndex = 0;
            int pmuPolrity = 0;
            double outputValue = 0.0d;

            int voltRangeIndex = 0;
            int currentRangeIndex = 0;
            bool rtn = true;

            // 2012 1220 Mike parameter

            this._status = this._ldt1a.GetMeterStatus();
                        
            if (this._setting[itemIndex].IsBipolarOutput == true)
            {
                outputValue = this._setting[itemIndex].ForceValue;
            }
            else
            {
                outputValue = Math.Abs(this._setting[itemIndex].ForceValue);

                if (this._setting[itemIndex].ForceValue < 0)
                {
                    pmuPolrity = 1;   // POSITIVE = 0; NEGATIVE = 1
                }
                else
                {
                    pmuPolrity = 0;
                }
            }

            pmuIndex = (int)this._setting[itemIndex].PUMIndex;
            
            this._ldt1a.HWTimerWait(this._setting[itemIndex].ForceDelayTime);



            switch (this._setting[itemIndex].MsrtType)
            {
                case EMsrtType.FI:

                    currentRangeIndex = this._setting[itemIndex].ForceRangeIndex;
                    voltRangeIndex = _voltRange[this._setting[itemIndex].PUMIndex].Length - 1;
            
                    // 2012 1113 Mike   2012  1220 Mike PMU
                    if (pmuIndex == 0) // PMU = HC
                    {
                        if (currentRangeIndex > 2) // 電流範圍 > 2 (400uA)
                        {
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity,0.0 ,voltRangeIndex ,2 , 0.1);                      //  (A) HCFV狀態, 切換電流檔位為 2=400uA ，  FV 值 = 0 V 
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.2);      //  (B) HCFV狀態, 切換電流檔位為輸入測試項目電流範圍，  FV 值 = 0 V 
                        }
                        else  // 電流範圍 < 2 (400uA)
                        {
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.2);      //  (B) HCFV狀態, 切換電流檔位為輸入測試項目電流範圍，  FV 值 = 0 V 
                        }
                    }
                    else  // PMU = HV
                    {
                        this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.2);          //  (B) HVFV狀態, 切換電流檔位為輸入測試項目電流範圍，  FV 值 = 0 V 
                    }
                    
                    //-----------------------------------------------
                    // (C) FV -> FI 切換, for HC & HV 
                    this._ldt1a.ForceI(pmuIndex, pmuPolrity, 0.0,  voltRangeIndex, currentRangeIndex, 0.2);   
                    // (D) FI = x.xxA 輸出電流, for HC & HV 
                    if (((int)this._setting[itemIndex].ForceTime) <= 10)
                    {
                        rtn = this._ldt1a.ForceI(pmuIndex, pmuPolrity, outputValue, voltRangeIndex, currentRangeIndex, this._setting[itemIndex].ForceTime);
                    }
                    else
                    {
                        rtn = this._ldt1a.ForceI(pmuIndex, pmuPolrity, outputValue, voltRangeIndex, currentRangeIndex, 1.0);
                        System.Threading.Thread.Sleep((int)(this._setting[itemIndex].ForceTime - 1.0));
                    }
                    break;
                //---------------------------------------------------------------------------------------------------------
                case EMsrtType.FV:
                    voltRangeIndex = this._setting[itemIndex].ForceRangeIndex;
                    currentRangeIndex = _currRange[this._setting[itemIndex].PUMIndex].Length - 1;
                    rtn = this._ldt1a.ForceV(pmuIndex, pmuPolrity, outputValue, voltRangeIndex, currentRangeIndex, this._setting[itemIndex].ForceTime);
                    break;
                //---------------------------------------------------------------------------------------------------------
                case EMsrtType.MI:
                    if (this._setting[itemIndex].MsrtFilterCount <= 0)
                    {
                        this._acquireData[itemIndex][0] = this._ldt1a.MeasureI(pmuIndex, this._msrtDelay);
                    }
                    else
                    {
                        this._acquireData[itemIndex][0] = this._ldt1a.MeasureAvgI(pmuIndex, this._setting[itemIndex].MsrtFilterCount, this._msrtDelay);
                    }
                    break;
                //---------------------------------------------------------------------------------------------------------
                case EMsrtType.MV:
                    if (this._setting[itemIndex].MsrtFilterCount <= 0)
                    {
                        this._acquireData[itemIndex][0] = this._ldt1a.MeasureV(pmuIndex, this._msrtDelay);
                    }
                    else
                    {
                        this._acquireData[itemIndex][0] = this._ldt1a.MeasureAvgV(pmuIndex, this._setting[itemIndex].MsrtFilterCount, this._msrtDelay);
                    }
                    break;
                //---------------------------------------------------------------------------------------------------------
                case EMsrtType.FIMV:
                case EMsrtType.POLAR:
                case EMsrtType.LIV:
                    currentRangeIndex = this._setting[itemIndex].ForceRangeIndex;
                    voltRangeIndex = this._setting[itemIndex].MsrtRangeIndex;

                    // 20121113 Mike    2012 1220 Mike
                    if (pmuIndex == 0) // PMU = HC
                    {
                        if (currentRangeIndex > 2)  // 電流檔位 > 2 (400uA) 
                        {
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, 2, 0.1);                          // (A) HCFV狀態, 切換電流檔位為 2=400uA ，  FV 值 = 0 V
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.6);          // (B) HCFV狀態, 切換電流檔位為輸入測試項目電流範圍  ， FV 值 = 0 V 
                        }
                        else  
                        {
                            if (currentRangeIndex == 0 && outputValue <= 0.000002)
                            {
                                this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex + 2, 0.6);  // (B) HCFV狀態，切換電流檔位為 輸入測試項目電流範圍(0) + 2  ，FV 值 = 0V   (目的  切換到大電流檔位，接下來輸入大電流 FI 值 ，幫助測量 IF1=1uA )  
                            }
                            else
                            {
                                this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.6);      // (B) HCFV狀態, 切換電流檔位為輸入測試項目電流範圍，  FV 值 = 0 V  
                            }
                        }   
                    }
                    else // PMU = HV
                    {
                        this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 1);                // (B) HVFV狀態, 切換電流檔位為輸入測試項目電流範圍，  FV 值 = 0 V            
                    }

                    //--------------------------------------------------------
                    // PMU = HC , for 4uA I Range, FI < 2uA special case.
                    if (pmuIndex == 0 && currentRangeIndex == 0 && outputValue <= 0.000002)
                    {
                        this._ldt1a.ForceI(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex + 2, 0);            // (C) FV - FI 切換, for HC, 2uA 小電流
                        this._ldt1a.ForceI(pmuIndex, pmuPolrity, 0.00004, voltRangeIndex, currentRangeIndex + 2, 0.1);      // (D) FI = 40uA (電流範圍十分之一  400uA /10 = 40uA), for HC, 2uA 小電流 
                        this._ldt1a.ForceI(pmuIndex, pmuPolrity, 0.000004, voltRangeIndex, currentRangeIndex + 1, 0.2);   // (D) FI = 4uA (電流範圍十分之一  40uA / 10 = 4uA), for HC, 2uA 小電流 
                        //this._ldt1a.ForceI(pmuIndex, pmuPolrity, 0.00002, voltRangeIndex, currentRangeIndex + 1, 0.2);    // (D) FI = 20uA (電流範圍十分之一  40uA / 2 = 20uA), for HC, 2uA 小電流 
                    }
                    else
                    {
                        this._ldt1a.ForceI(pmuIndex, pmuPolrity, 0, voltRangeIndex, currentRangeIndex, 0);                  // (C) FV -> FI 切換,  for HC & HV 
                    }

                    // (D) FI = x.xxA 輸出電流, for HC & HV 
                    if ( ((int)this._setting[itemIndex].ForceTime) <= 40)
                    {
                        rtn = this._ldt1a.ForceI(pmuIndex, pmuPolrity, outputValue, voltRangeIndex, currentRangeIndex, this._setting[itemIndex].ForceTime);
                    }
                    else
                    {
                        rtn = this._ldt1a.ForceI(pmuIndex, pmuPolrity, outputValue, voltRangeIndex, currentRangeIndex, 1.0);
                        System.Threading.Thread.Sleep((int)(this._setting[itemIndex].ForceTime - 1.0));
                    }

                    // (E) MV = x.xxV 量測電壓,  for HC & HV 

                    this._applyData[itemIndex][0] = outputValue;

                    if (this._setting[itemIndex].MsrtFilterCount <= 0)
                    {
                        this._acquireData[itemIndex][0] = this._ldt1a.MeasureV(pmuIndex, this._msrtDelay + DEFAULT_MSRT_DELAY);
                    }
                    else
                    {
                        this._acquireData[itemIndex][0] = this._ldt1a.MeasureAvgV02(pmuIndex, this._setting[itemIndex].MsrtFilterCount, (int)(this._setting[itemIndex].MsrtFilterCount*0.5), this._msrtDelay + DEFAULT_MSRT_DELAY);
                        this._msrtDataList = this._ldt1a.GetMsrtRowData();
                        this._status = this._ldt1a.GetMeterStatus();
                        this.CalcMsrtData(itemIndex);
                        
                    }
                    break;
                //---------------------------------------------------------------------------------------------------------
                case EMsrtType.FVMI:
                    voltRangeIndex = this._setting[itemIndex].ForceRangeIndex;
                    currentRangeIndex = this._setting[itemIndex].MsrtRangeIndex;

                    ///// 21121113 Mike    20121220  Mike  add PMU 
                    if (pmuIndex == 0)  // PMU = HC
                    {
                        if (currentRangeIndex > 2)   // 電流檔位 > 2(400uA) 
                        {
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, 2, 0.1);                      // (A) HCFV狀態, 切換電流檔位為 2=400uA ，  FV 值 = 0 V
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.1);      // (B) HCFV狀態, 切換電流檔位為輸入測試項目電流範圍  ， FV 值 = 0 V 
                        }
                        else
                        {
                          this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.1);        // (B) HCFV狀態, 切換電流檔位為輸入測試項目電流範圍  ， FV 值 = 0 V 
                         }
                    }
                    else // PMU = HV
                    {
                        this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.1);          // (B) HVFV狀態, 切換電流檔位為輸入測試項目電流範圍  ， FV 值 = 0 V 
                    }

                    //-----------------------------------------------
                    // (D) FV = x.xxA 輸出電壓
                    if (((int)this._setting[itemIndex].ForceTime) <= 40)
                    {
                        rtn = this._ldt1a.ForceV(pmuIndex, pmuPolrity, outputValue, voltRangeIndex, currentRangeIndex, this._setting[itemIndex].ForceTime);
                    }
                    else
                    {
                        rtn = this._ldt1a.ForceV(pmuIndex, pmuPolrity, outputValue, voltRangeIndex, currentRangeIndex, 1.0);
                        System.Threading.Thread.Sleep((int)(this._setting[itemIndex].ForceTime - 1.0));
                    }

                    // (D) MI = x.xxV  量測電流

                    this._applyData[itemIndex][0] = outputValue;

                    if (this._setting[itemIndex].MsrtFilterCount <= 0)
                    {
                        this._acquireData[itemIndex][0] = this._ldt1a.MeasureI(pmuIndex, this._msrtDelay + DEFAULT_MSRT_DELAY);
                    }
                    else
                    {
                        this._acquireData[itemIndex][0] = this._ldt1a.MeasureAvgI02(pmuIndex, this._setting[itemIndex].MsrtFilterCount,(int)(this._setting[itemIndex].MsrtFilterCount * 0.5), this._msrtDelay + DEFAULT_MSRT_DELAY);
                       /// this._acquireData[itemIndex][0] = this._ldt1a.MeasureAvgI(pmuIndex, this._setting[itemIndex].MsrtFilterCount, this._msrtDelay + DEFAULT_MSRT_DELAY);
                        this._msrtDataList = this._ldt1a.GetMsrtRowData();
                        this._status = this._ldt1a.GetMeterStatus();
                        this.CalcMsrtData(itemIndex);
                    }
                    break;
                //---------------------------------------------------------------------------------------------------------
                case EMsrtType.THY:

                    // 20120714 Gilbert
					// Force Voltage = 0.0 V / 5ms 
                    // Reset the output voltage =  0 V

					voltRangeIndex = this._setting[itemIndex].MsrtRangeIndex;		// THY_MsrtRangeIndex
					currentRangeIndex = this._setting[itemIndex].ForceRangeIndex;	// THY_ForceRangeIndex

                    // 21121113 Mike
                    if (pmuIndex == 0) // PMU = HC
                    {
                        if (currentRangeIndex > 2)  
                        {
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, 2, 0.1);                      // (A) HCFV狀態, 切換電流檔位為 2=400uA ，  FV 值 = 0 V  
                            this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.2);      // (B) HCFV狀態, 切換電流檔位為輸入測試項目電流範圍  ， FV 值 = 0 V 
                        }
                        else
                        {
                            if (currentRangeIndex == 0)
                            {
                                this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex + 2, 0.1);    // Why ? Gilbert
                            }
                            else
                            {
                                this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.1);      // (B) HCFV狀態, 切換電流檔位為輸入測試項目電流範圍  ， FV 值 = 0 V 
                            }
                        }
                    }
                    else // PMU = HV
                    {
                        this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.1);              // (B) HVFV狀態, 切換電流檔位為輸入測試項目電流範圍  ， FV 值 = 0 V 
                    }
                    //-----------------------------------------------
                    // (D) FV = x.xxA 輸出電壓
                    if (pmuIndex == 0 && currentRangeIndex == 0)
                    {
                        rtn = this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex + 2, 1.0);      // Why ? Gilbert
                    }
                    else
                    {
                        rtn = this._ldt1a.ForceV(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 5.0);        // FV = 0 V @ 5ms , default time = 5ms
                    }

                    // (D) MI = x.xxV  量測電流
					this._acquireData[itemIndex][0] = this._ldt1a.MeasureAvgI(pmuIndex, 5, this._msrtDelay + DEFAULT_MSRT_DELAY);

                    // (C) FV -> FI 切換,  for HC & HV     
                    rtn = this._ldt1a.ForceI(pmuIndex, pmuPolrity, 0.0, voltRangeIndex, currentRangeIndex, 0.2);
                    // I-V Sweep, Multi-FIMV                     
                    lock (this)
                    {
                        this.THYIVSweepMsrt(itemIndex);  // (D) 多點的 FI = x.xxA 輸出電流 (E) MV = x.xxV  量測電壓
                    }
                    this.THYCalculateByTime(itemIndex);
                    break;
                //---------------------------------------------------------------------------------------------------------
                case EMsrtType.FIMVSWEEP:
                    lock (this)
                    {
                        this.FIMVSweepMsrt(itemIndex);
                    }
                    break;
                //---------------------------------------------------------------------------------------------------------
                case EMsrtType.FVMISWEEP:
                    break;
                //---------------------------------------------------------------------------------------------------------
                default:
                    break;
            }

            if (this._setting[itemIndex].IsAutoTurnOff == true)
            {
                this._ldt1a.TurnOff(this._turnOffDelay); /////mike1025
            }

            if (rtn == false)
            {
                this._errorNum = EDevErrorNumber.ForceOutput_Ctrl_Err;
            }

            return rtn;
        }

        #endregion

        # region >>> Public Method <<<

        public bool Init(int devNum, string sourceMeterSN)
        {
            try
            {
                if (0 == this._ldt1a.Init())
                {
                    this._ldt1a.Reset();
                    this._serialNum = this._ldt1a.GetSerialNum().ToString("D8");
                    this._hwVersion = this._ldt1a.GetHardwareVersion().ToString("X4");
                    this._swVersion = this._ldt1a.GetSoftwareVersion().ToString("D4");
                    return true;
                }
                else
                {
                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                    return false;
                }
            }
            catch
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_HW_Err;
                return false;
            }

        }

        public void Close()
        {
            this._ldt1a.Close();
        }

        public void Reset()
        {
            this._ldt1a.Reset();
            this._status = this._ldt1a.GetMeterStatus();
        }

        public bool SetConfigToMeter(ElecDevSetting cfg)
        {
            return true;
        }

        public bool SetParamToMeter(ElectSettingData[] setting)
        {
            int itemIndex = 0;

            this._errorNum = EDevErrorNumber.Device_NO_Error;
            this._setting = null;
            if (setting == null || setting.Length == 0)
            {
                return true;
            }

            this._acquireData = new double[setting.Length][];
            this._sweepForceRangeIndex = new int[setting.Length][];
            this._sweepResult = new double[setting.Length][];
            this._timeChain = new double[setting.Length][];
            this._applyData = new double[setting.Length][];

            foreach (ElectSettingData setItem in setting)
            {
                if (setItem == null)
                {
                    this._errorNum = EDevErrorNumber.NoSourceMeterParamSettingData;
                    return false;
                }

                //----------------------------------------------------------------------------------------------------
                // (1) Find the PMUIndex, Force Range Index, Msrt Range Index
                //----------------------------------------------------------------------------------------------------
                if (this.FindPMUIndex(setItem) == false)
                {
                    this._errorNum = EDevErrorNumber.NoMatchRangeIndex;
                    return false;
                }

                //----------------------------------------------------------------------------------------------------
                // (2) Check clamp value 
                //----------------------------------------------------------------------------------------------------
                if (this.CheckClampAndMsrtRange(setItem) == false)
                {
                    this._errorNum = EDevErrorNumber.ClampValueSetting_Err;
                    return false;
                }
                //----------------------------------------------------------------------------------------------------
                // (3) Calculate the sweep output value (points)
                //----------------------------------------------------------------------------------------------------
                if (this.CalcSweepForceValue(setItem) == false)
                {
                    this._errorNum = EDevErrorNumber.CalcSweepForceValue_Err;
                    return false;
                }

                //----------------------------------------------------------------------------------------------------
                // (4) Find the sweep force range  index by each sweep force value
                //----------------------------------------------------------------------------------------------------
                //this._sweepForceRangeIndex[itemIndex] = this.FindSweepForceRangIndex(setItem, this._sweepOutput[itemIndex]);
                this._sweepForceRangeIndex[itemIndex] = this.CalcSweepForceRangIndex(setItem);
                if (this._sweepForceRangeIndex == null)
                {
                    this._errorNum = EDevErrorNumber.CalcSweepForceRangeIndex_Err;
                    return false;
                }

                //this.FindMsrtRangeIndex();

                //----------------------------------------------------------------------------------------------------
                // (5) Create the _acquireData, _sweepResult, _timeChain object
                //----------------------------------------------------------------------------------------------------	       
                if (setItem.MsrtType == EMsrtType.FI || setItem.MsrtType == EMsrtType.FIMV || setItem.MsrtType == EMsrtType.MI || setItem.MsrtType == EMsrtType.POLAR ||
                        setItem.MsrtType == EMsrtType.FV || setItem.MsrtType == EMsrtType.FVMI || setItem.MsrtType == EMsrtType.MV || setItem.MsrtType == EMsrtType.LIV)
                {
                    this._acquireData[itemIndex] = new double[setItem.MsrtFilterCount + 1 ];
                    this._applyData[itemIndex] = new double[1];
                }
                else if (setItem.MsrtType == EMsrtType.FIMVSWEEP || setItem.MsrtType == EMsrtType.FVMISWEEP || setItem.MsrtType == EMsrtType.THY)
                {
                    this._acquireData[itemIndex] = new double[setItem.SweepRiseCount + setItem.SweepContCount + 1];		// include sweep start point
                    this._applyData[itemIndex] = new double[1];
                }

                this._sweepResult[itemIndex] = new double[Enum.GetNames(typeof(ETHYResultItem)).Length];
                this._timeChain[itemIndex] = new double[setItem.SweepRiseCount + setItem.SweepContCount + 1];		// include sweep start point

                itemIndex++;
            }

            this._setting = setting;
            return true;
        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex)
        {
            if (this._setting == null)
            {
                this._errorNum = EDevErrorNumber.NoSourceMeterParamSettingData;
                return false;
            }

            if (itemIndex > this._setting.Length)
            {
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return false;
            }

            return this.TriggerMeterOut(itemIndex);

        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex, double applyValue)
        {
            return true;
        }

        public double[] MeterOutput(ElectSettingData setting)
        {
            this._acquireData = new double[1][];
            //this._sweepOutput = new double[1][];
            this._sweepForceRangeIndex = new int[1][];

            if (setting == null)
            {
                return this._acquireData[0];
            }

            ////----------------------------------------------------------------------------------------------------
            //// (1) Calculate the sweep Points
            ////----------------------------------------------------------------------------------------------------
            //this._sweepOutput[0] = this.CalcSweepPoint(setting);
            //if (this._sweepOutput[0] == null)
            //{
            //    return this._acquireData[0];
            //}

            ////----------------------------------------------------------------------------------------------------
            //// (2) Find the sweep force range  index by sweep output value
            ////----------------------------------------------------------------------------------------------------
            //this._sweepForceRangeIndex[0] = this.FindSweepForceRangIndex(setting, this._sweepOutput[0]);
            //if (this._sweepForceRangeIndex[0] == null)
            //{
            //    return this._acquireData[0];
            //}

            //this.FindMsrtRangeIndex();

            ////----------------------------------------------------------------------------------------------------
            //// (3) Create the _acquireData, _sweepResult, _timeChain object
            ////----------------------------------------------------------------------------------------------------
            //this._setting[0] = setting;
            //if (setting.MsrtType == EMsrtType.FIMVSWEEP || setting.MsrtType == EMsrtType.FVMISWEEP || setting.MsrtType == EMsrtType.THY)
            //{
            //    this._acquireData[0] = new double[setting.SweepRiseCount + setting.SweepContCount + 1];		// include sweep start point
            //}
            //else
            //{
            //    this._acquireData[0] = new double[1];
            //}
            //this._sweepResult[0] = new double[Enum.GetNames(typeof(ETHYResultItem)).Length];
            //this._timeChain[0] = new double[this._acquireData[0].Length];

            ////---------------------------------------------------------------------
            //// (4) Trigger sourceMeter output 
            ////---------------------------------------------------------------------
            //if (this.TriggerMeterOut(0) == true)
            //{
            //    this._setting = null;
            //    return this._acquireData[0];
            //}
            //else
            //{
            //    this._setting = null;
            return this._acquireData[0];
            //}

        }

        public double[] GetDataFromMeter(uint channel, uint itemIndex)
        {
            if (this._setting == null || itemIndex > this._setting.Length)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }

            return this._acquireData[itemIndex];
        }

        public double[] GetApplyDataFromMeter(uint channel, uint itemIndex)
        {
            if (this._setting == null || itemIndex > this._setting.Length)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }

            return this._applyData[itemIndex];
        }

        public double[] GetSweepPointFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._setting.Length)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }

            //return this._sweepOutput[itemIndex];
            return this._setting[itemIndex].SweepCustomValue;
        }

        public double[] GetSweepResultFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._setting.Length)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }

            this._sweepResult[itemIndex][(int)ETHYResultItem.OverShoot] = this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue];
            return this._sweepResult[itemIndex];
        }

        public double[] GetTimeChainFromMeter(uint channel, uint itemIndex)
        {
            return this._timeChain[itemIndex];
        }

        public void TurnOff()  //mike1025
        {
            // this._ldt1a.TurnOff( this._turnOffDelay + DEFAULT_TURN_OFF_DELAY );
            //this._ldt1a.TurnOff(DEFAULT_TURN_OFF_DELAY);

            //this._status = this._ldt1a.GetMeterStatus();
            this._ldt1a.TurnOff(DEFAULT_TURN_OFF_DELAY);
            this._ldt1a.Dissconnect();
            this._ldt1a.HWTimerWait(0.5);
            //this._status = this._ldt1a.GetMeterStatus();

        }

        public void TurnOff(double delay, bool isOpenRelay)
        {
            if ( delay < DEFAULT_TURN_OFF_DELAY )
            {
                delay = DEFAULT_TURN_OFF_DELAY;
            }

            if (isOpenRelay)
            {

                this._ldt1a.TurnOff(delay);
                this._ldt1a.Dissconnect();
                this._ldt1a.HWTimerWait(0.5);
            }
            else
            {
                this._ldt1a.TurnOff(delay);
            }

        }

        public void Output(uint point, bool state)
        {
            this._ldt1a.TriggerOut(point, state);
        }

        public byte InputB(uint point)
        {
            if (point >= 3)
                return 0;

            uint inData = this._ldt1a.TriggerIn();

            return ((byte)(inData & 0x07));
        }

        public byte Input(uint point)
        {
            if (point >= 3)
                return 0;

            uint inData = this._ldt1a.TriggerIn();
            int shiftBit = (int)point;

            if ((inData & (uint)(0x01 << shiftBit)) == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public double GetPDDarkSample(int count)
        {
            return 0.0d;
        }

        public bool CheckInterLock()
        {
            return true;
        }

        #endregion
    }
}
