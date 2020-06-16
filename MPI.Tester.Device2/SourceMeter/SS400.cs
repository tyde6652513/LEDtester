using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Device.Pulser.Vektrex;
using MPI.Tester.Device.DMM.Keithley;

namespace MPI.Tester.Device.Pulser
{
    public class SS400 : ISourceMeter
    {
        private bool _isDeviceLog = false;
        private bool _isSequenceTimeLog = false;
        
        private IConnect _conn;

        private K7510 _dmmVoltMsrt;
        private K7510 _dmmPhotoCurrentMsrt;

        private ISourceMeter _iRevSrcDev;

        private string _devSN;
        private string _devHwVer;
        private string _devSwVer;

        private MPI.PerformanceTimer _pt;
        private MPI.PerformanceTimer _ptTimeOut;

        private MPI.PerformanceTimer _ptSequenceLog;

        private ElecDevSetting _devConfig;
        private ElectSettingData[] _elcSetting;
        private EDevErrorNumber _errorNum;
        private SourceMeterSpec _spec;

        private List<double[][]> _applyData;
        private List<double[][]> _acquireData;
        private List<double[][]> _timeChain;
        private List<double[][]> _sweepResult;

        private List<string[][]> _strSweepTrigList;

        private double[][] _voltRange;
        private double[][] _currRange;

        private double[] _voltPulseRange;
        private double[] _currPulseRange;
        private double[] _maxPulseWidth;
        private double[] _maxPulseDuty;

        private bool _isSyncAcqMsrtData;
        private double _estimatedSequnceTime;

        private bool _isContactFailSkip;

        private List<string> _tempStatusQueue;

        public SS400()
        {
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this._pt = new PerformanceTimer();

            this._ptTimeOut = new PerformanceTimer();

            this._ptSequenceLog = new PerformanceTimer();

            this._acquireData = new List<double[][]>();

            this._timeChain = new List<double[][]>();

            this._sweepResult = new List<double[][]>();

            this._applyData = new List<double[][]>();

            this._strSweepTrigList = new List<string[][]>();

            this._isSyncAcqMsrtData = true;

            this._isContactFailSkip = false;

            this._tempStatusQueue = new List<string>();
        }

        public SS400(ElecDevSetting devConfig) : this()
        {
            this._devConfig = devConfig;
        }

        #region >>> Public Proberty <<<

        public string SerialNumber
        {
            get { return this._devSN; }
        }

        public string SoftwareVersion
        {
            get { return this._devSwVer; }
        }

        public string HardwareVersion
        {
            get { return this._devHwVer; }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return _errorNum; }
        }

        public ElectSettingData[] ElecSetting
        {
            get
            {
                if (this._elcSetting == null)
                {
                    return null;
                }

                ElectSettingData[] data = new ElectSettingData[this._elcSetting.Length];

                for (int i = 0; i < this._elcSetting.Length; i++)
                {
                    data[i] = this._elcSetting[i].Clone() as ElectSettingData;
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

        private void DelayTime(double delayTime, bool isThreadSleep = false)
        {
            if (delayTime > 0.0d)
            {
                if (delayTime >= 30.0d)
                {
                    System.Threading.Thread.Sleep((int)delayTime);
                }
                else
                {
                    this._pt.Start();

                    do
                    {
                        if (isThreadSleep)
                        {
                            System.Threading.Thread.Sleep(0);
                        }

                        if (this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond) >= delayTime)
                        {
                            this._pt.Stop();
                            this._pt.Reset();
                            return;
                        }
                        System.Threading.Thread.Sleep(0);
                    } while (this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond) < delayTime);
                    this._pt.Stop();
                    this._pt.Reset();
                }
            }
        }

        private bool TimeUnitConvert(ElectSettingData item)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Time Unit Convert
            ////////////////////////////////////////////////////////////////////////////////////////
            item.ForceTime = Math.Round((item.ForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            item.ForceDelayTime = Math.Round((item.ForceDelayTime / 1000.0d), 6, MidpointRounding.AwayFromZero); //Unit: Second

            item.SweepTurnOffTime = Math.Round((item.SweepTurnOffTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            item.SweepHoldTime = Math.Round((item.SweepHoldTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            item.TurnOffTime = Math.Round((item.TurnOffTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            ////////////////////////////////////////////////////////////////////////////////////////
            // (2) Check Time Limit
            ////////////////////////////////////////////////////////////////////////////////////////
            if (item.ForceTime > SS400Spec.PROG_MAX_APPLY_TIME || item.ForceTime < SS400Spec.PROG_MIN_APPLY_TIME)
            {
                return false;
            }

            if (item.ForceDelayTime > SS400Spec.PROG_MAX_APPLY_TIME || item.ForceDelayTime < 0.0d)
            {
                return false;
            }

            return true;
        }

        private bool FindSrcAndMsrtRange(ElectSettingData item)
        {
            bool isCurrDrive = true;

            double setCurrRange = 0.0d;

            double setVoltRange = 0.0d;

            switch (item.MsrtType)
            {
                case EMsrtType.FI:
                case EMsrtType.FIMV:
                case EMsrtType.THY:
                case EMsrtType.POLAR:
                case EMsrtType.R:
                case EMsrtType.RTH:
                case EMsrtType.VLR:
                case EMsrtType.FIMVLOP:
                    {
                        setCurrRange = Math.Abs(item.ForceValue);

                        setVoltRange = Math.Abs(item.MsrtRange);

                        isCurrDrive = true;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.LIV:
                    {
                        setCurrRange = Math.Abs(item.SweepStop);

                        setVoltRange = Math.Abs(item.MsrtRange);

                        isCurrDrive = true;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.FVMI:
                case EMsrtType.FV:
                case EMsrtType.FVMILOP:
                case EMsrtType.FVMISCAN:
                case EMsrtType.LCR:
                    {
                        setVoltRange = Math.Abs(item.ForceValue);

                        setCurrRange = Math.Abs(item.MsrtRange);

                        isCurrDrive = false;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.LVI:
                    {
                        setVoltRange = Math.Abs(item.SweepStop);

                        setCurrRange = Math.Abs(item.MsrtRange);

                        isCurrDrive = false;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.FIMVSWEEP:
                case EMsrtType.PIV:
                    {
                        setCurrRange = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

                        setVoltRange = Math.Abs(item.MsrtRange);

                        isCurrDrive = true;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.FVMISWEEP:
                    {
                        setVoltRange = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

                        setCurrRange = Math.Abs(item.MsrtRange);

                        isCurrDrive = false;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                default:
                    return false;
            }

            int index;

            // Current Range Check
            for (index = this._currRange.Length - 1; index >= 0; index--)
            {
                if (setCurrRange <= this._currRange[index][this._currRange[index].Length - 1]) // Force Value <= Device Max Current
                {
                    if (isCurrDrive)
                    {
                        item.ForceRange = setCurrRange;
                    }
                    else
                    {
                        item.MsrtRange = setCurrRange;
                    }
                    break;
                }
            }

            // Voltage Range Check
            if (index >= 0)
            {
                if (setVoltRange <= this._voltRange[index][this._voltRange[index].Length - 1])
                {
                    if (isCurrDrive)
                    {
                        item.MsrtRange = setVoltRange;
                    }
                    else
                    {
                        item.ForceRange = setVoltRange;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool FindSrcAndMsrtPulseRange(ElectSettingData item, out int region)
        {
            region = -1;

            switch (item.MsrtType)
            {
                case EMsrtType.FIMV:
                case EMsrtType.PIV:
                case EMsrtType.FIMVLOP:
                    {
                        for (int i = 0; i < this._currPulseRange.Length; i++)
                        {
                            if (Math.Abs(item.ForceValue) > this._currPulseRange[i] || Math.Abs(item.MsrtRange) > this._voltPulseRange[i])
                            {
                                continue;
                            }

                            if (item.ForceTime > this._maxPulseWidth[i])
                            {
                                continue;
                            }

                            item.ForceRange = this._currPulseRange[i];

                            item.MsrtProtection = Math.Abs(item.MsrtRange);

                            region = i;

                            return true;
                        }

                        return false;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        /// start = sweep start value
        /// end   = sweep end value
        /// step = sweep step Value
        /// </summary>
        private double[] MakeLinearSweepList(double start, double step, double end, uint lvlCount = 1)
        {
            // lvlCount  = repeat count for each step; (min = 1)
            // i.e. start = 1, end = 2, step = 0.5, lvlCount = 4
            //      the sweep list will be "1,1,1,1, 1.5, 1.5,1.5,1.5  2,2,2,2" for QCW Sweep
            // Note: In CW mode / Pulse Mode, set the lvlCcount = 1
 
            if (step == 0)
            {
                return null;
            }

            lvlCount = lvlCount > 0 ? lvlCount : 1;

           //uint risingPoints = (uint)((end - start) / step) + 1;

            uint risingPoints  = (uint)(Math.Round(((end - start) / step), 6, MidpointRounding.AwayFromZero)) + 1;

            double[] list = new double[risingPoints * lvlCount];

            uint count = 0;

            for (int i = 0; i < risingPoints; i++)
            {
                for (int j = 0; j < lvlCount; j++)
                {
                    list[count] = Math.Round((start + step * i), 6, MidpointRounding.AwayFromZero);

                    if (Math.Abs(list[count]) > Math.Abs(end))
                    {
                        list[count] = end;
                    }

                    count++;
                }
            }

            return list;
        }

        private string[] GetDevicePrintValueToArray(char splitSymbol)
        {
            string rawStrData = string.Empty;

            if (this._conn.WaitAndGetData(out rawStrData))
            {
                rawStrData = rawStrData.TrimEnd('\n');

                rawStrData = rawStrData.Replace(" ", "");

                string[] rawStrDataArray = rawStrData.Split(splitSymbol);

                return rawStrDataArray;
            }
            else
            {
                return null;
            }
        }

        private bool AcquireMsrtData(uint[] deviceIDs, uint index)
        {
            ElectSettingData item = this._elcSetting[index];

            if (item.KeyName.Contains("IFWLB"))
            {
                return true;
            }
            
            uint id = 0;

            bool isPulserDelay = true;

            string rtnStr = string.Empty;

            //-----------------------------------------------------------------------------------------------
           // this._conn.SendCommand(SCPI.QUERY_COMPLETE);

            //do
            //{
            //    rtnStr = string.Empty;
                
            //    this._conn.WaitAndGetData(out rtnStr);

            //    System.Threading.Thread.Sleep(0);
            //}
            //while (rtnStr.Trim() != SCPI.FLAG_COMPLETE);

            uint triggerCnt = 1;
            double[] tempBuffer;

            switch (item.MsrtType)
            {
                case EMsrtType.FIMV:
                    {
                        if (item.KeyName.Contains("IZ"))
                        {
                            isPulserDelay = false;

                            if (this._iRevSrcDev != null)
                            {
                                double[] tempRevData = this._iRevSrcDev.GetDataFromMeter(0, this._elcSetting[index].Order);

                                this._acquireData[(int)id][index][0] = tempRevData[0];
                            }
                        }
                        else
                        {
                            double dutI = 0.0d;
                            double dutV = 0.0d;

                            if (!this._isContactFailSkip)
                            {
                                if (this._dmmVoltMsrt != null)
                                {
                                    tempBuffer = this._dmmVoltMsrt.ReadDataFromBuffer(triggerCnt);

                                    dutV = tempBuffer[0];
                                }

                                if (dutV > item.MsrtProtection)
                                {
                                    dutV = item.MsrtProtection;
                                }
                            }
                            else
                            {
                                dutV = item.MsrtProtection;
                            }

                            this._acquireData[(int)id][index][0] = dutV;
                        }
                        break;
                    }
                case EMsrtType.FVMI:
                    {
                        if (item.KeyName.Contains("VR"))
                        {
                            isPulserDelay = false;
                            
                            if (this._iRevSrcDev != null)
                            {
                                double[] tempRevData = this._iRevSrcDev.GetDataFromMeter(0, this._elcSetting[index].Order);

                                this._acquireData[(int)id][index][0] = tempRevData[0];
                            }
                        }

                        break;
                    }
                case EMsrtType.FIMVLOP:
                    {
                        //-----------------------------------------------------------------------------------------------
                       // this._conn.SendCommand(SCPI.SrcFuncMode_CH1(CPD.FUNC_VOLT, 20));
                        //-----------------------------------------------------------------------------------------------
                        double dutI = 0.0d;
                        double dutV = item.MsrtProtection;
                        double pdCurr = 0.0d;
                        double pdCnt = 0.0d;

                        if (!this._isContactFailSkip)
                        {
                            if (this._dmmVoltMsrt != null)
                            {
                                tempBuffer = this._dmmVoltMsrt.ReadDataFromBuffer(triggerCnt);

                                dutV = tempBuffer[0];
                            }

                            if (this._dmmPhotoCurrentMsrt != null)
                            {
                                tempBuffer = this._dmmPhotoCurrentMsrt.ReadDataFromBuffer(triggerCnt);

                                pdCurr = tempBuffer[0];
                            }
                        }

                        this._applyData[(int)id][index][0] = dutI;

                        this._acquireData[(int)id][index][0] = dutV;
                        this._acquireData[(int)id][index][1] = pdCurr;
                        this._acquireData[(int)id][index][2] = pdCnt;
                        this._acquireData[(int)id][index][3] = 0.0d;
                        this._acquireData[(int)id][index][4] = 0.0d;

                        break;
                    }
                case EMsrtType.PIV:
                    {
                        #region >>> PIV Reading Buffer <<<

                        double[] iArray = null;
                        double[] vArray = null;
                        double[] pArray = null;

                        triggerCnt = item.SweepRiseCount;

                        iArray = new double[triggerCnt];
                        vArray = new double[triggerCnt];
                        pArray = new double[triggerCnt];

                        if (!this._isContactFailSkip)
                        {
                            this.LogTimerStart();
                            
                            if (this._dmmVoltMsrt != null)
                            {
                                vArray = this._dmmVoltMsrt.ReadDataFromBuffer(triggerCnt);
                            }

                            this.LogTimerStop("DMM_V AcquireData");

                            this.LogTimerStart();

                            if (this._dmmPhotoCurrentMsrt != null)
                            {
                                pArray = this._dmmPhotoCurrentMsrt.ReadDataFromBuffer(triggerCnt);
                            }

                            this.LogTimerStop("DMM_I AcquireData");
                        }
                        else
                        {
                            for (int i = 0; i < vArray.Length; i++)
                            {
                                vArray[i] = item.MsrtProtection;
                            }
                        }

                        //this._elcSetting[index].SweepCustomValue = iArray;
                        this._acquireData[(int)id][index] = vArray;
                        this._sweepResult[(int)id][index] = pArray;

                        #endregion

                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            if (isPulserDelay)
            {
                this.DelayTime(20.0d);  // 
            }

            return true;
        }

        private void GetStatusMsgQueue()
        {
            string msg = string.Empty;

            this._tempStatusQueue.Clear();

            do
            {
                this._conn.SendCommand(SCPI.QUERY_SYS_EVENT_QUEUE);

                this._conn.WaitAndGetData(out msg);

                this._tempStatusQueue.Add(msg.Trim());

                System.Threading.Thread.Sleep(0);

            } while (!msg.Contains(CPD.STATUS_EVENT_NO_ERROR));
        }

        //private bool GetErrorMsg(out string msg)
        //{
        //    this.GetStatusMsg(out msg);

        //    if (msg.Contains("OK"))
        //    {
        //        return false;
        //    }

        //    Console.WriteLine("[SS400Device], GetErrorMsg()," + msg);
            
        //    return true;
        //}

        private void LogTimerStart()
        {
            if (!this._isSequenceTimeLog)
                return;

            this._ptSequenceLog.Reset();
 
            this._ptSequenceLog.Start();
        }

        private void LogTimerStop(string msg)
        {
            if (!this._isSequenceTimeLog)
                return;

            this._ptSequenceLog.Stop();

            double time = this._ptSequenceLog.GetTimeSpan(ETimeSpanUnit.MilliSecond);

            Console.WriteLine(string.Format("[SS400] Sequence Timer Log, {0} = {1}ms", msg, time.ToString("0.0000")));
        }

        #endregion

        #region >>> SS400 Program <<<

        private void SetConfig(ElecDevSetting devSetting)
        {
            string cmd = string.Empty;


            //this._conn.SendCommand(cmd);
        }

        private string SetPulseSweep(ElectSettingData item)
        {
            double pulsWidth = item.ForceTime;

            double pulsVoltClamp = 20.0d;

            double duty = item.Duty;
            double start = item.SweepStart;
            double stop = item.SweepStop;
            uint points = item.SweepRiseCount;

            string pulsRange = CPD.RANGE_HIGH;

            if (stop <= SS400Spec.SS400_DC_CURR_RANGE[0][0])
            {
                pulsRange = CPD.RANGE_LOW;
            }

            //double start = item.SweepStart;
            //double end = item.SweepStop;
            //uint points = item.SweepRiseCount;
            pulsWidth = Math.Round(pulsWidth, 6, MidpointRounding.AwayFromZero);  // us -> s

            double offTime = Math.Round(((pulsWidth / duty) - pulsWidth), 6, MidpointRounding.AwayFromZero);


            string cmd = string.Empty;

            cmd += SCPI.SrcConfig_CH1(CPD.SHAPE_PULSE_SWEEP);

            cmd += SCPI.SrcPulseOnTime_CH1(pulsWidth);

            cmd += SCPI.SrcPulseOffTime_CH1(offTime);

            cmd += SCPI.SrcRange_CH1(pulsRange);
            cmd += SCPI.SrcSweepStart_CH1(start);
            cmd += SCPI.SrcSweepStop_CH1(stop);
            cmd += SCPI.SrcSweepPoints_CH1(points);

          
            cmd += SCPI.SrcVoltageClamp_CH1(pulsVoltClamp);

            return cmd;
        }

        private string SetDcSweep(ElectSettingData item)
        {
            double pulsWidth = item.ForceTime;

            double pulsVoltClamp = 20.0d;

            double duty = item.Duty;
            double start = item.SweepStart;
            double stop = item.SweepStop;
            uint points = item.SweepRiseCount;

            string pulsRange = CPD.RANGE_HIGH;

            if (stop <= SS400Spec.SS400_DC_CURR_RANGE[0][0])
            {
                pulsRange = CPD.RANGE_LOW;
            }

            //double start = item.SweepStart;
            //double end = item.SweepStop;
            //uint points = item.SweepRiseCount;
            pulsWidth = Math.Round(pulsWidth, 6, MidpointRounding.AwayFromZero);  // us -> s

            //double offTime = Math.Round(((pulsWidth / duty) - pulsWidth), 6, MidpointRounding.AwayFromZero);

            double offTime = 300e-6d;

            string cmd = string.Empty;

            cmd += SCPI.SrcConfig_CH1(CPD.SHAPE_PULSE_SWEEP);

            cmd += SCPI.SrcPulseOnTime_CH1(pulsWidth);

            cmd += SCPI.SrcPulseOffTime_CH1(offTime);

            cmd += SCPI.SrcRange_CH1(pulsRange);
            cmd += SCPI.SrcSweepStart_CH1(start);
            cmd += SCPI.SrcSweepStop_CH1(stop);
            cmd += SCPI.SrcSweepPoints_CH1(points);


            cmd += SCPI.SrcVoltageClamp_CH1(pulsVoltClamp);

            return cmd;
        }

        private string SetMultiPulse(ElectSettingData item)
        {
            double pulsVoltClamp = 20.0d;

            double pulsLvl = item.ForceValue;
            double pulsWidth = item.ForceTime;
            double duty = item.Duty;
            uint pulseCnt = item.PulseCount;

            string pulsRange = CPD.RANGE_HIGH;

            if (pulsLvl <= SS400Spec.SS400_DC_CURR_RANGE[0][0])
            {
                pulsRange = CPD.RANGE_LOW;
            }

            pulsWidth = Math.Round(pulsWidth, 6, MidpointRounding.AwayFromZero);  // us -> s

            double offTime = SS400Spec.PROG_MIN_OFF_TIME;

            string cmd = string.Empty;

            cmd += SCPI.SrcConfig_CH1(CPD.SHAPE_MULTIPULSE);

            cmd += SCPI.SrcPulseLevel_CH1(pulsLvl);

            cmd += SCPI.SrcPulseOnTime_CH1(pulsWidth);

            cmd += SCPI.SrcPulseOffTime_CH1(offTime);

            cmd += SCPI.SrcPulseCount_CH1(pulseCnt);
            cmd += SCPI.SrcRange_CH1(pulsRange);
            cmd += SCPI.SrcVoltageClamp_CH1(pulsVoltClamp);

            return cmd;
        }

        private bool RunTestItem(uint deviceID, uint index)
        {
            double estimatedSequnceTime = 0.0d;

            this._isContactFailSkip = false;

            this._isSyncAcqMsrtData = true;

            ElectSettingData item = this._elcSetting[index];

            #region >>> Proberty <<<
            // Channel-1
            string srcIV1 = string.Empty;
            string srcMode1 = string.Empty;

            string srcShap1 = string.Empty;
            double srcRange1 = 0.0d;
            double srcPulseLvl1 = 0.0d;
            double srcBiasLvl1 = 0.0d;

            double pulseWidth1 = 0.0d; //s
            double period1 = 0.0d;

            string msrtMode1 = string.Empty;
            double msrtAcqDelay1 = 0.0d;
            double msrtRange1 = 0.0d;
            double msrtClamp1 = 0.0d;
            double nplc1 = 0.01d;

            uint trigCount = 1;

            #endregion

            string status = string.Empty;

            string cmd = string.Empty;

            // Start SrcMeter Test Sequence
            switch (item.MsrtType)
            {
                case EMsrtType.FIMV:
                    {
                        #region >>> FIMV Cmd <<<

                        period1 = Math.Round((item.ForceTime / item.Duty), 6, MidpointRounding.AwayFromZero);

                        estimatedSequnceTime = (item.ForceTime + SS400Spec.PROG_MIN_OFF_TIME) * 1000.0d;
                        estimatedSequnceTime = estimatedSequnceTime * 2.0d;

                        cmd = string.Empty;

                        if (item.KeyName.Contains("IFWLB"))
                        {
                            return true;
                        }
                        else if (item.KeyName.Contains("IFWLA"))
                        {
                            if (this._isDeviceLog)
                            {
                                Console.WriteLine("[SS400Device], RunTestItem(), FIMV_SPT");
                            }
                            
                            this._isSyncAcqMsrtData = false;

                            cmd = string.Empty;

                            cmd += this.SetMultiPulse(item);

                            cmd += SCPI.SourceTrggerMode1(CPD.TRIG_INTERNAL);

                            cmd += SCPI.SrcOutput_CH1(true);

                            this._conn.SendCommand(cmd);

                            ///////////////////////////////////////////////////////////////////////////////////////////////////
                            // DMM
                            if (this._dmmVoltMsrt != null)
                            {
                                this._dmmVoltMsrt.Trigger(index);
                            }

                            if (this._dmmVoltMsrt != null)
                            {
                                this._dmmVoltMsrt.WaitTriggerReady();
                            }

                            ///////////////////////////////////////////////////////////////////////////////////////////////////
                            if (!this.WaitTriggerReady(out status))
                            {
                                if (this._isDeviceLog)
                                {
                                    Console.WriteLine("[SS400Device], RunTestItem(), FIMV_SPT, WaitTriggerReady = false");
                                }
                                
                                cmd = SCPI.SrcOutput_CH1(false);

                                this._conn.SendCommand(cmd);

                                if (this._dmmVoltMsrt != null)
                                {
                                    this._dmmVoltMsrt.AbortTrigger();
                                }

                                if (this.IsLoadFaultStatusEvent(status))
                                {
                                    this._isContactFailSkip = true;

                                    return true;
                                }

                                return false;
                            }

                            this._conn.SendCommand(SCPI.TrigInit());
                        }
                        else
                        {
                            
                            if (item.IsTrigCamera)
                            {
                                if (this._isDeviceLog)
                                {
                                    Console.WriteLine("[SS400Device], RunTestItem(), FIMV_CT");
                                }
                                
                                // Single / Multi Pulse with Trigger
                                
                                if (this._dmmVoltMsrt == null)
                                {
                                    return false;
                                }

                                cmd += this.SetMultiPulse(item);

                                cmd += SCPI.SourceTrggerMode1(CPD.TRIG_EXTERNAL);

                                cmd += SCPI.SrcOutput_CH1(true);

                                this._conn.SendCommand(cmd);

                                if (!this.WaitTriggerReady(out status))
                                {
                                    if (this._isDeviceLog)
                                    {
                                        Console.WriteLine("[SS400Device], RunTestItem(), FIMV_CT, WaitTriggerReady = false");
                                    }
                                    
                                    cmd = SCPI.SrcOutput_CH1(false);

                                    this._conn.SendCommand(cmd);

                                    this._dmmVoltMsrt.AbortTrigger();

                                    if (this.IsLoadFaultStatusEvent(status))
                                    {
                                        this._isContactFailSkip = true;

                                        return true;
                                    }

                                    return false;
                                }

                               // this._conn.SendCommand(SCPI.TrigInit());

                                //-------------------------------------------------
                                this._dmmVoltMsrt.Trigger(index);

                                this._dmmVoltMsrt.WaitTriggerIdle();

                                this._conn.SendCommand(SCPI.SrcOutput_CH1(false));
                            }
                            else
                            {
                               // Single / Multi Pulse without Trigger
                                
                                cmd = string.Empty;

                                cmd += this.SetMultiPulse(item);

                                cmd += SCPI.SourceTrggerMode1(CPD.TRIG_INTERNAL);

                                cmd += SCPI.SrcOutput_CH1(true);

                                this._conn.SendCommand(cmd);

                                // DMM
                                if (this._dmmVoltMsrt != null)
                                {
                                    this._dmmVoltMsrt.Trigger(index);
                                }

                                if (this._dmmVoltMsrt != null)
                                {
                                    this._dmmVoltMsrt.WaitTriggerReady();
                                }

                                ///////////////////////////////////////////////////////////////////////////////////////////////////
                                if (!this.WaitTriggerReady(out status))
                                {
                                    cmd = SCPI.SrcOutput_CH1(false);

                                    this._conn.SendCommand(cmd);

                                    if (this._dmmVoltMsrt != null)
                                    {
                                        this._dmmVoltMsrt.AbortTrigger();
                                    }

                                    if (this._dmmPhotoCurrentMsrt != null)
                                    {
                                        this._dmmPhotoCurrentMsrt.AbortTrigger();
                                    }

                                    if (this.IsLoadFaultStatusEvent(status))
                                    {
                                        this._isContactFailSkip = true;

                                        return true;
                                    }

                                    return false;
                                }

                                this._conn.SendCommand(SCPI.TrigInit());

                                if (!this.WaitTriggerComplete(estimatedSequnceTime, out status))
                                {
                                    cmd = SCPI.SrcOutput_CH1(false);

                                    this._conn.SendCommand(cmd);

                                    if (this._dmmVoltMsrt != null)
                                    {
                                        this._dmmVoltMsrt.AbortTrigger();
                                    }

                                    if (this.IsLoadFaultStatusEvent(status))
                                    {
                                        this._isContactFailSkip = true;

                                        return true;
                                    }

                                    return false;
                                }

                                //if (this._dmmVoltMsrt != null)
                                //{
                                //    this._dmmVoltMsrt.WaitTriggerIdle();
                                //}

                                this._conn.SendCommand(SCPI.SrcOutput_CH1(false));
                            }
                        }


                        #endregion

                        break;
                    }
                case EMsrtType.FIMVLOP:
                    {
                        #region >>> FIMVLOP Cmd <<<

                        if (this._isDeviceLog)
                        {
                            Console.WriteLine("[SS400Device], RunTestItem(), FIMVLOP");
                        }

                        ////----------------------------------------------------------------------------------
                        period1 = Math.Round((item.ForceTime / item.Duty), 6, MidpointRounding.AwayFromZero);

                        estimatedSequnceTime = (item.ForceTime + SS400Spec.PROG_MIN_OFF_TIME) * 1000.0d;
                        estimatedSequnceTime = estimatedSequnceTime * 2.0d;

                        cmd = string.Empty;

                        cmd += this.SetMultiPulse(item);

                        cmd += SCPI.SourceTrggerMode1(CPD.TRIG_INTERNAL);

                        cmd += SCPI.SrcOutput_CH1(true);

                        this._conn.SendCommand(cmd);

                        ///////////////////////////////////////////////////////////////////////////////////////////////////
                        // DMM
                        if (this._dmmVoltMsrt != null)
                        {
                            this._dmmVoltMsrt.Trigger(index);
                        }

                        if (this._dmmPhotoCurrentMsrt != null)
                        {
                            this._dmmPhotoCurrentMsrt.Trigger(index);
                        }

                        if (this._dmmVoltMsrt != null)
                        {
                            this._dmmVoltMsrt.WaitTriggerReady();
                        }

                        if (this._dmmPhotoCurrentMsrt != null)
                        {
                            this._dmmPhotoCurrentMsrt.WaitTriggerReady();
                        }

                        ///////////////////////////////////////////////////////////////////////////////////////////////////
                        if (!this.WaitTriggerReady(out status))
                        {
                            if (this._isDeviceLog)
                            {
                                Console.WriteLine("[SS400Device], RunTestItem(), FIMVLOP, WaitTriggerReady = false");
                            }
                            
                            cmd = SCPI.SrcOutput_CH1(false);

                            this._conn.SendCommand(cmd);

                            if (this._dmmVoltMsrt != null)
                            {
                                this._dmmVoltMsrt.AbortTrigger();
                            }

                            if (this._dmmPhotoCurrentMsrt != null)
                            {
                                this._dmmPhotoCurrentMsrt.AbortTrigger();
                            }

                            if (this.IsLoadFaultStatusEvent(status))
                            {
                                this._isContactFailSkip = true;

                                return true;
                            }

                            return false;
                        }

                        this._conn.SendCommand(SCPI.TrigInit());

                        if (!this.WaitTriggerComplete(estimatedSequnceTime, out status))
                        {
                            cmd = SCPI.SrcOutput_CH1(false);

                            this._conn.SendCommand(cmd);

                            if (this._dmmVoltMsrt != null)
                            {
                                this._dmmVoltMsrt.AbortTrigger();
                            }

                            if (this._dmmPhotoCurrentMsrt != null)
                            {
                                this._dmmPhotoCurrentMsrt.AbortTrigger();
                            }

                            if (this.IsLoadFaultStatusEvent(status))
                            {
                                this._isContactFailSkip = true;

                                return true;
                            }

                            return false;
                        }

                        if (this._dmmVoltMsrt != null)
                        {
                            this._dmmVoltMsrt.WaitTriggerIdle();
                        }

                        if (this._dmmPhotoCurrentMsrt != null)
                        {
                            this._dmmPhotoCurrentMsrt.WaitTriggerIdle();
                        }

                        this._conn.SendCommand(SCPI.SrcOutput_CH1(false));

                        #endregion

                        break;
                    }
                case EMsrtType.PIV:
                    {
                        #region >>> Pulse Sweep Cmd <<<

                        if (this._isDeviceLog)
                        {
                            Console.WriteLine("[SS400Device], RunTestItem(), PIV");
                        }

                        //---------------------------------------------------------------------------------------------
                        // Output Channel-Pulser
                        double start = item.SweepStart;
                        double end = item.SweepStop;
                        uint points = item.SweepRiseCount;

                        period1 = Math.Round((item.ForceTime / item.Duty), 6, MidpointRounding.AwayFromZero);

                        trigCount = points;

                        estimatedSequnceTime = period1 * 1000.0d * trigCount;   // unit = ms 
                        estimatedSequnceTime = estimatedSequnceTime * 2.0d;  // sequence time 2 倍時間

                        this.LogTimerStart();

                        cmd = string.Empty;

                        if (item.SourceFunction == ESourceFunc.CW)
                        {
                            cmd += this.SetDcSweep(item);
                        }
                        else
                        {
                            cmd += this.SetPulseSweep(item);
                        }

                        cmd += SCPI.SourceTrggerMode1(CPD.TRIG_INTERNAL);

                        cmd += SCPI.SrcOutput_CH1(true);
  
                        this._conn.SendCommand(cmd);

                        ///////////////////////////////////////////////////////////////////////////////////////////////////
                        // DMM
                        if (this._dmmVoltMsrt != null)
                        {
                            this._dmmVoltMsrt.Trigger(index);
                        }

                        if (this._dmmPhotoCurrentMsrt != null)
                        {
                            this._dmmPhotoCurrentMsrt.Trigger(index);
                        }

                        this.LogTimerStop("SS400/DMM SendCmd");

                        this.LogTimerStart();

                        if (this._dmmVoltMsrt != null)
                        {
                            this._dmmVoltMsrt.WaitTriggerReady();
                        }

                        this.LogTimerStop("DMM_V WaitTriggerReady");

                        this.LogTimerStart();

                        if (this._dmmPhotoCurrentMsrt != null)
                        {
                            this._dmmPhotoCurrentMsrt.WaitTriggerReady();
                        }

                        this.LogTimerStop("DMM_I WaitTriggerReady");

                        this.LogTimerStart();

                        ///////////////////////////////////////////////////////////////////////////////////////////////////
                        if (!this.WaitTriggerReady(out status))
                        {
                            if (this._isDeviceLog)
                            {
                                Console.WriteLine("[SS400Device], RunTestItem(), PIV, WaitTriggerReady = false");
                            }
                            
                            cmd = SCPI.SrcOutput_CH1(false);

                            this._conn.SendCommand(cmd);

                            if (this._dmmVoltMsrt != null)
                            {
                                this._dmmVoltMsrt.AbortTrigger();
                            }

                            if (this._dmmPhotoCurrentMsrt != null)
                            {
                                this._dmmPhotoCurrentMsrt.AbortTrigger();
                            }

                            if (this.IsLoadFaultStatusEvent(status))
                            {
                                this._isContactFailSkip = true;

                                this.LogTimerStop("SS400 WaitTriggerReady, LoadFaultSkip");

                                return true;
                            }

                            return false;
                        }

                        this.LogTimerStop("SS400 WaitTriggerReady");

                        this.LogTimerStart();

                        this._conn.SendCommand(SCPI.TrigInit());

                        if (!this.WaitTriggerComplete(estimatedSequnceTime, out status))
                        {
                            cmd = SCPI.SrcOutput_CH1(false);

                            this._conn.SendCommand(cmd);

                            if (this._dmmVoltMsrt != null)
                            {
                                this._dmmVoltMsrt.AbortTrigger();
                            }

                            if (this._dmmPhotoCurrentMsrt != null)
                            {
                                this._dmmPhotoCurrentMsrt.AbortTrigger();
                            }

                            if (this.IsLoadFaultStatusEvent(status))
                            {
                                this._isContactFailSkip = true;
                                this.LogTimerStop("SS400 WaitTriggerComplete, LoadFaultSkip");
                                return true;
                            }

                            return false;
                        }

                        this.LogTimerStop("SS400 WaitTriggerComplete");

                        this._conn.SendCommand(SCPI.SrcOutput_CH1(false));
                        
                        #endregion

                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            this._estimatedSequnceTime = estimatedSequnceTime;

            return true;
        }

        private bool CheckStatusEvent(out string status)
        {
            bool rtn = true;
            string rtnMsg = string.Empty;

            status = string.Empty;

            for (int cnt = 0; cnt < this._tempStatusQueue.Count; cnt++)
            {
                rtnMsg = this._tempStatusQueue[cnt];

                //---------------------------------------------------------------------------------------------------------------
                // Load - Fail
                // 200
                if (rtnMsg.Contains(CPD.STATUS_LOAD_OVER_VOLTAGE_SWEEP))
                {
                    status = CPD.STATUS_LOAD_OVER_VOLTAGE_SWEEP;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_OVER_VOLTAGE");
                    }

                    rtn &= false;
                    return rtn;
                }

                // 201
                if (rtnMsg.Contains(CPD.STATUS_LOAD_HIGH_SIDE_OVER_CURRENT))
                {
                    status = CPD.STATUS_LOAD_HIGH_SIDE_OVER_CURRENT;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_HIGH_SIDE_OVER_CURRENT");
                    }

                    rtn &= false;
                    return rtn;
                }

                // 202
                if (rtnMsg.Contains(CPD.STATUS_LOAD_LOW_SIDE_OVER_CURRENT))
                {
                    status = CPD.STATUS_LOAD_LOW_SIDE_OVER_CURRENT;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_LOW_SIDE_OVER_CURRENT");
                    }

                    rtn &= false;
                    return rtn;
                }

                // 204
                if (rtnMsg.Contains(CPD.STATUS_LOAD_VOLT_RAMP))
                {
                    status = CPD.STATUS_LOAD_VOLT_RAMP;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_LOW_SIDE_OVER_CURRENT");
                    }

                    rtn &= false;
                    return rtn;
                }

                // 205
                if (rtnMsg.Contains(CPD.STATUS_LOAD_LEAKAGE_FROM_EXT_SOURCE))
                {
                    status = CPD.STATUS_LOAD_LEAKAGE_FROM_EXT_SOURCE;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_LEAKAGE_FROM_SOURCE");
                    }

                    rtn &= false;
                    return rtn;
                }

                // 206
                if (rtnMsg.Contains(CPD.STATUS_LOAD_OVER_VOLTAGE))
                {
                    status = CPD.STATUS_LOAD_OVER_VOLTAGE;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_OVER_VOLTAGE");
                    }

                    rtn &= false;
                    return rtn;
                }

                // 207
                if (rtnMsg.Contains(CPD.STATUS_LOAD_CURRENT_LEAKAGE))
                {
                    status = CPD.STATUS_LOAD_CURRENT_LEAKAGE;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_OVER_VOLTAGE");
                    }

                    rtn &= false;
                    return rtn;
                }

                // 208
                if (rtnMsg.Contains(CPD.STATUS_LOAD_EXCESSIVE_INT_VOLTAGE))
                {
                    status = CPD.STATUS_LOAD_EXCESSIVE_INT_VOLTAGE;
                    
                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_OVER_VOLTAGE");
                    }

                    rtn &= false;
                    return rtn;
                }

                //---------------------------------------------------------------------------------------------------------------
                if (rtnMsg.Contains(CPD.STATUS_EVENT_CHANNEL_SHUT_DOWN_SWEEP))
                {
                    status = CPD.STATUS_EVENT_CHANNEL_SHUT_DOWN_SWEEP;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerComplete(), STATUS_CHANNEL_SHUT_DOWN");
                    }

                    rtn &= false;
                    return rtn;
                }

                //---------------------------------------------------------------------------------------------------------------

                if (rtnMsg.Contains(CPD.STATUS_ERROR_CHANNEL_NOT_READY))
                {
                    status = CPD.STATUS_ERROR_CHANNEL_NOT_READY;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_CHANNEL_NOT_READY");
                    }

                    rtn &= false;
                    continue;
                }

                if (rtnMsg.Contains(CPD.STATUS_EVENT_CHANNEL_SHUT_DOWN_SWEEP))
                {
                    status = CPD.STATUS_EVENT_CHANNEL_SHUT_DOWN_SWEEP;

                    if (this._isDeviceLog)
                    {
                        Console.WriteLine("[SS400Device], WaitTriggerReady(), STATUS_EVENT_CHANNEL_SHUT_DOWN_SWEEP");
                    }

                    rtn &= false;
                    return rtn;
                }

                //---------------------------------------------------------------------------------------------------------------
                if (rtnMsg.Contains(CPD.STATUS_EVENT_CHANNEL_READY))
                {
                    status = CPD.STATUS_EVENT_CHANNEL_READY;
                    rtn &= true;
                    continue;
                }

                if (rtnMsg.Contains(CPD.STATUS_EVENT_CHANNEL_COMPLETE))
                {
                    status = CPD.STATUS_EVENT_CHANNEL_COMPLETE;
                    rtn &= true;
                    return rtn;
                }
                //---------------------------------------------------------------------------------------------------------------
            }

            return rtn;
        }

        private bool IsLoadFaultStatusEvent(string statusID)
        {
            if (statusID == CPD.STATUS_LOAD_OVER_VOLTAGE_SWEEP ||
                statusID == CPD.STATUS_LOAD_HIGH_SIDE_OVER_CURRENT ||
                statusID == CPD.STATUS_LOAD_LOW_SIDE_OVER_CURRENT ||
                statusID == CPD.STATUS_LOAD_VOLT_RAMP ||
                statusID == CPD.STATUS_LOAD_LEAKAGE_FROM_EXT_SOURCE ||
                statusID == CPD.STATUS_LOAD_OVER_VOLTAGE ||
                statusID == CPD.STATUS_LOAD_CURRENT_LEAKAGE ||
                statusID == CPD.STATUS_LOAD_EXCESSIVE_INT_VOLTAGE)
            {
                if (this._isDeviceLog)
                {
                    Console.WriteLine("[SS400Device], IsLoadFaultStatusEvent(), ByPass");
                }
                
                return true;
            }

            return false;
        }

        private bool WaitTriggerReady(out string status)
        {
            status = string.Empty;
            
            string rtnMsg = string.Empty;

            do
            {
                this.GetStatusMsgQueue();

            } while (this._tempStatusQueue.Count == 1);


            bool rtn = this.CheckStatusEvent(out status);

            return rtn;
        }

        private bool WaitTriggerComplete(double estimatedSequnceTime, out string status)
        {
            bool rtn = true;
            
            status = string.Empty;
            
            string rtnMsg = string.Empty;
            string errMsg = string.Empty;
            double nowTime = 0.0d;
           
           // this._ptTimeOut.Start();

            do
            {
                this._conn.SendCommand(SCPI.QUERY_COMPLETE);

                this._conn.WaitAndGetData(out rtnMsg);

                rtnMsg = rtnMsg.Trim();

                this.GetStatusMsgQueue();

                if (rtnMsg == CPD.STATUS_OPC_PULSE_END_TRUE)
                {
                    rtn = this.CheckStatusEvent(out status);

                    status = CPD.STATUS_OPC_PULSE_END_TRUE;

                    return true;
                }

                //if (rtnMsg == CPD.STATUS_OPC_PULSE_END_ERR)
                //{
                //    status = CPD.STATUS_OPC_PULSE_END_TRUE;
                    
                //    rtn = this.CheckStatusEvent(out status);

                //    //status = CPD.STATUS_OPC_PULSE_END_ERR;

                //    //for (int cnt = 0; cnt < this._tempStatusQueue.Count; cnt++)
                //    //{
                //    //    errMsg = this._tempStatusQueue[cnt];

                //    //    Console.WriteLine("[SS400Device], WaitTriggerComplete(), STATUS_OPC_PULSE_END_ERR, ErrMsg = " + errMsg);
                //    //}

                //    return false;
                //}

                if (!this.CheckStatusEvent(out status))
                {
                    if (this._isDeviceLog)
                    {
                        for (int cnt = 0; cnt < this._tempStatusQueue.Count; cnt++)
                        {
                            errMsg = this._tempStatusQueue[cnt];

                            Console.WriteLine("[SS400Device], WaitTriggerComplete(), CheckStatusEvent = false, ErrMsg = " + errMsg);
                        }
                    }

                    return false;
                }
                //------------------------------------------------------------------------------------------------------------------------
                // rtnMsg == CPD.STATUS_OPC_PULSE_END_FLASE, Check the status during the test
              


                //nowTime = this._ptTimeOut.PeekTimeSpan(ETimeSpanUnit.MilliSecond);

                //if (nowTime >= estimatedSequnceTime)
                //{
                //    Console.WriteLine("[SS400Device], WaitTriggerComplete(), Time Out!");
                //    this._ptTimeOut.Stop();
                //    this._ptTimeOut.Reset();
                //    return true;
                //}

                System.Threading.Thread.Sleep(0);

            } while (rtnMsg == CPD.STATUS_OPC_PULSE_END_FLASE);

            //this._ptTimeOut.Stop();

            //this._ptTimeOut.Reset();

            return true;
        }

        #endregion

        #region >>> Public Method <<<

        public bool Init(int deviceNum, string sourceMeterSN)
        {
            #region >>> Output Channel Unit (Pulser) <<<

            LANSettingData lanSetting = new LANSettingData();

            lanSetting.IPAddress = sourceMeterSN;
            lanSetting.Port = 8282;

            this._conn = new LANConnect(lanSetting);

            string rtnStr = string.Empty;

            if (!this._conn.Open(out rtnStr))
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                Console.WriteLine("[SS400Device], Init(), Connection Open Fail");
                return false;
            }

            //--------------------------------------------------------------------
            // Query Device Information
            this._conn.SendCommand(SCPI.RESET);
            this._conn.SendCommand(SCPI.QUERY_DEV_INFO);

            this._conn.WaitAndGetData(out rtnStr);

            if (rtnStr == string.Empty)
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                Console.WriteLine("[SS400Device], Init(), QueryIDN Fail");
                return false;
            }

            string[] devinfo = rtnStr.Trim().Split(',');

            if (devinfo.Length != 15)
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                Console.WriteLine("[SS400Device], Init(), QueryIDN Fail");
                return false;
            }


            string manufacturer = devinfo[0];
            string model = devinfo[14];
            string sn = devinfo[12];
            string hwVer = devinfo[13];

            this._devSN = sn;
            this._devSwVer = hwVer;
            this._devHwVer = "Vektrex " + model.Replace("Model: ", "");

            this._conn.SendCommand(SCPI.TrigInputPolarity1(CPD.TRIG_POL_RISING));
            this._conn.SendCommand(SCPI.SrcAutoRange_CH1(false));
            this._conn.SendCommand(SCPI.SrcRange_CH1(CPD.RANGE_HIGH));

            this.GetStatusMsgQueue();

            #endregion

            #region >>> Voltage Measurement Unit (DMM) <<<

            if (this._devConfig.VoltMsrtDevice == EDmmModel.K7510)
            {
                this._dmmVoltMsrt = new K7510();
                
                if (!this._dmmVoltMsrt.Init(this._devConfig.VoltMsrtDeviceIP, EDmmMeasureFunc.DC_VOLTAGE))
                {
                    this._dmmVoltMsrt = null;

                    return false;
                }

                this._dmmVoltMsrt.ReadingBufferSize = 60;
            }

            #endregion

            #region >>> Photo-Current Measurement Unit (DMM) <<<

            if (this._devConfig.DetectorMsrtDevice == EPDSensingMode.DMM_7510)
            {
                this._dmmPhotoCurrentMsrt = new K7510();
                
                if (!this._dmmPhotoCurrentMsrt.Init(this._devConfig.DetectorDeviceIP, EDmmMeasureFunc.DC_CURRENT))
                {
                    this._dmmPhotoCurrentMsrt = null;

                    return false;
                }

                this._dmmPhotoCurrentMsrt.ReadingBufferSize = 40;
            }

            #endregion

            #region >>> Reverse SMU <<<

            if (this._devConfig.ReverseSrcDevModel != ESourceMeterModel.NONE)
            {
                ElecDevSetting devSetting = new ElecDevSetting();

                devSetting.SrcTurnOffType = ESrcTurnOffType.EachTestItem;

                devSetting.SrcSensingMode = ESrcSensingMode._4wire;

                devSetting.SourceMeterModel = this._devConfig.ReverseSrcDevModel;

                devSetting.SrcTriggerMode = ESMUTriggerMode.Single;

                switch (this._devConfig.ReverseSrcDevModel)
                {
                    case ESourceMeterModel.K2600:
                        {
                            this._iRevSrcDev = new MPI.Tester.Device.SourceMeter.Keithley2600(devSetting);
                            break;
                        }
                    case ESourceMeterModel.B2900A:
                        {
                            this._iRevSrcDev = new MPI.Tester.Device.SourceMeter.KeysightB2900A(devSetting);
                            break;
                        }
                    default:
                        return false;
                }

                if (this._iRevSrcDev != null)
                {
                    if (!this._iRevSrcDev.Init(0, this._devConfig.ReverseSrcDevIP))
                    {
                        this._iRevSrcDev = null;
                        return false;
                    }
                }
            }

            #endregion

            //--------------------------------------------------------------------------------------------------------
            if (this._iRevSrcDev != null)
            {
                this._voltRange = this._iRevSrcDev.Spec.VoltageRange.ToArray();
                this._currRange = this._iRevSrcDev.Spec.CurrentRange.ToArray();
            }
            else
            {
                this._voltRange = SS400Spec.SS400_DC_VOLT_RANGE;
                this._currRange = SS400Spec.SS400_DC_CURR_RANGE;
            }

            this._currPulseRange = SS400Spec.SS400_PULSE_VOLT_RANGE;
            this._voltPulseRange = SS400Spec.SS400_PULSE_VOLT_RANGE;

            this._maxPulseWidth = SS400Spec.SS400_MAX_PULSE_WIDTH;
            this._maxPulseDuty = SS400Spec.SS400_MAX_PULSE_DUTY;

            double[] maxPulseWidth_ms = this._maxPulseWidth.Clone() as double[];
            double[] maxDuty_percent = this._maxPulseDuty.Clone() as double[];

            for (int i = 0; i < maxPulseWidth_ms.Length; i++)
            {
                maxPulseWidth_ms[i] = maxPulseWidth_ms[i] * 1000.0d;
            }

            for (int i = 0; i < maxDuty_percent.Length; i++)
            {
                maxDuty_percent[i] = maxDuty_percent[i] * 100.0d;
            }

            this._spec = new SourceMeterSpec(this._currRange, this._voltRange,
                                 this._currPulseRange, this._voltPulseRange, maxPulseWidth_ms, maxDuty_percent);

            this._spec.IsAutoForceRange = true;
            this._spec.IsSupportedNPLC = true;

            //--------------------------------------------------------------------------------------------------------
           // this.SetConfig();

            return true;
        }

        public void Close()
        {
            this._conn.Close();

            if (this._dmmVoltMsrt != null)
            {
                this._dmmVoltMsrt.Close();
            }

            if (this._dmmPhotoCurrentMsrt != null)
            {
                this._dmmPhotoCurrentMsrt.Close();
            }

            if (this._iRevSrcDev != null)
            {
                this._iRevSrcDev.Close();
            }
        }

        public void Reset()
        {
           
        }

        public bool SetConfigToMeter(ElecDevSetting devSetting)
        {
         
            
            return true;
        }

        public bool SetParamToMeter(ElectSettingData[] eleSetting)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Reset Error Code
            ////////////////////////////////////////////////////////////////////////////////////////
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            if (eleSetting == null || eleSetting.Length == 0)
            { 
                return true;
            }

            this._elcSetting = eleSetting;

            ////////////////////////////////////////////////////////////////////////////////////////
            // (2) Create Buffer
            ////////////////////////////////////////////////////////////////////////////////////////
            this._applyData.Clear();

            this._acquireData.Clear();

            this._sweepResult.Clear();

            this._timeChain.Clear();

            this._strSweepTrigList.Clear();

            this._applyData.Add(new double[eleSetting.Length][]);

            this._acquireData.Add(new double[eleSetting.Length][]);

            this._sweepResult.Add(new double[eleSetting.Length][]);

            this._timeChain.Add(new double[eleSetting.Length][]);

            this._strSweepTrigList.Add(new string[eleSetting.Length][]);

            ////////////////////////////////////////////////////////////////////////////////////////
            // (4) Set Config to Pulser
            ////////////////////////////////////////////////////////////////////////////////////////

            CPD.ECableCompFactor ccom = CPD.ECableCompFactor.Low;
            CPD.ERiseTimeCompFactor rcom = CPD.ERiseTimeCompFactor.Medium;

            string cmd = string.Empty;
            cmd += SCPI.SrcConfigCcom_CH1(ccom);
            cmd += SCPI.SrcConfigRcom_CH1(rcom);
            cmd += SCPI.SrcRampSpeed_CH1(CPD.RAMP_SPEED_FAST);

            this._conn.SendCommand(cmd);
            ////////////////////////////////////////////////////////////////////////////////////////
            // (4) Set Test Item to Meter
            ////////////////////////////////////////////////////////////////////////////////////////
            uint deviceID = 0;
            int region;

            List<ElectSettingData> revSetting = new List<ElectSettingData>();
            DmmSettingData dmmVoltSetting;
            DmmSettingData dmmDetectorCurrSetting;

            if (this._dmmVoltMsrt != null)
            {
                this._dmmVoltMsrt.ClearStatus();
            }

            if (this._dmmPhotoCurrentMsrt != null)
            {
                this._dmmPhotoCurrentMsrt.ClearStatus();
            }

            uint subIndex = 0;

            for (uint index = 0; index < this._elcSetting.Length; index++)
            {
                this._applyData[(int)deviceID][index] = new double[1] { this._elcSetting[index].ForceValue };

                ElectSettingData item = this._elcSetting[index];

                ElectSettingData revItem = this._elcSetting[index].Clone() as ElectSettingData;

                if (!this.TimeUnitConvert(item))
                {
                    Console.WriteLine("[SS400Device], SetParamToMeter(), Time settings conflict;");
                    this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                    return false;
                }

                //if (!this.FindSrcAndMsrtRange(item))
                //{
                //    if (!this.FindSrcAndMsrtPulseRange(item, out region))
                //    {
                //        Console.WriteLine("[SS400Device], SetParamToMeter(), Paramter out of boundry");
                //        this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                //        return false;
                //    }
                //}

                //-----------------------------------------------------------------------------------------
                // Paul 2014.03.14 取絕對值，IF>120mA IZ=10uA，會出現 Error Code 5007 
                // Force Range and Compliance Setting
                //-----------------------------------------------------------------------------------------
                item.ForceRange = Math.Abs(item.ForceRange);

                item.MsrtRange = Math.Abs(item.MsrtRange);

                item.MsrtProtection = Math.Abs(item.MsrtProtection);

                item.DetectorMsrtRange = Math.Abs(item.DetectorMsrtRange);

                dmmVoltSetting = null;

                dmmDetectorCurrSetting = null;

                switch (item.MsrtType)
                {
                    case EMsrtType.FIMV:   // Vf, Vz
                        {
                            this._acquireData[(int)deviceID][index] = new double[1];

                            if (item.KeyName.Contains("IZ"))
                            {
                                item.Order = subIndex;

                                revSetting.Add(revItem);

                                subIndex++;
                            }
                            else
                            {
                                //-------------------------------------------------------------------------------------------
                                // DMM Voltage Msrt
                                dmmVoltSetting = new DmmSettingData();

                                item.IsTrigCamera = true;

                                if (item.IsTrigCamera)
                                {
                                    dmmVoltSetting.TriggerOutMode = EDmmDioTriggerOut.PIN1_FFP;
                                    dmmVoltSetting.TriggerOutDelay = 50e-6;
                                }

                                dmmVoltSetting.MeasureFunction = EDmmMeasureFunc.DC_VOLTAGE;
                                dmmVoltSetting.MeasureIntegrationUnit = EDmmDcIntegrationUnit.Aperture;
                                dmmVoltSetting.MeasureRange = item.MsrtRange;
                                dmmVoltSetting.MeasureApertureTime = item.ForceTime * 0.7;

                                dmmVoltSetting.TriggerCount = 1;
                                dmmVoltSetting.TriggerInputDelay = 10e-6;
                            }

                            break;
                        }
                    case EMsrtType.FVMI:   // If, Ir
                        {
                            this._acquireData[(int)deviceID][index] = new double[1];

                            if (item.KeyName.Contains("VR"))
                            {
                                item.Order = subIndex;

                                revSetting.Add(revItem);

                                subIndex++;
                            }

                            break;
                        }
                    case EMsrtType.FIMVLOP:
                        {
                            // DUT_Volt, Ipd-CHA, Icnt-CHA, Ipd-CHB, Ipd-CHB
                            this._acquireData[(int)deviceID][index] = new double[5];

                            item.PulseCount = 1;

                            //-------------------------------------------------------------------------------------------
                            // DMM Voltage Msrt
                            dmmVoltSetting = new DmmSettingData();

                            dmmVoltSetting.MeasureFunction = EDmmMeasureFunc.DC_VOLTAGE;
                            dmmVoltSetting.MeasureIntegrationUnit = EDmmDcIntegrationUnit.Aperture;
                            dmmVoltSetting.MeasureRange = item.MsrtRange;
                            dmmVoltSetting.MeasureApertureTime = item.ForceTime * 0.7;

                            dmmVoltSetting.TriggerCount = item.PulseCount;
                            dmmVoltSetting.TriggerInputDelay = 10e-6;

                            //-------------------------------------------------------------------------------------------
                            // DMM detector curr Msrt
                            dmmDetectorCurrSetting = new DmmSettingData();

                            dmmDetectorCurrSetting.MeasureFunction = EDmmMeasureFunc.DC_CURRENT;
                            dmmDetectorCurrSetting.MeasureIntegrationUnit = EDmmDcIntegrationUnit.Aperture;
                            dmmDetectorCurrSetting.MeasureRange = item.DetectorMsrtRange;
                            dmmDetectorCurrSetting.MeasureApertureTime = item.ForceTime * 0.7;

                            dmmDetectorCurrSetting.TriggerCount = item.PulseCount;
                            dmmDetectorCurrSetting.TriggerInputDelay = 10e-6;

                            break;
                        }
                    case EMsrtType.PIV:
                        {
                            double start = item.SweepStart;
                            double step = item.SweepStep;
                            double end = item.SweepStop;
                            uint pulseCnt = item.PulseCount;
                          
                            double[] lstRisingStep = this.MakeLinearSweepList(start, step, end); // Rising step, 用來計算的Apply Data
                            double[] lstTrigList;

                            if(item.SourceFunction == ESourceFunc.QCW)
                            {
                                lstTrigList = this.MakeLinearSweepList(start, step, end, pulseCnt);
                            }
                            else
                            {
                                lstTrigList = lstRisingStep;
                            }

                            this._strSweepTrigList[(int)deviceID][index] = new string[lstTrigList.Length];

                            for (int i = 0; i < lstTrigList.Length; i++)
                            {
                                this._strSweepTrigList[(int)deviceID][index][i] = lstTrigList[i].ToString();
                            }

                            this._elcSetting[index].SweepCustomValue = lstRisingStep;

                            //-------------------------------------------------------------------------------------------
                            // DMM Voltage Msrt
                            dmmVoltSetting = new DmmSettingData();

                            dmmVoltSetting.MeasureFunction = EDmmMeasureFunc.DC_VOLTAGE;
                            dmmVoltSetting.MeasureIntegrationUnit = EDmmDcIntegrationUnit.Aperture;
                            dmmVoltSetting.MeasureRange = item.MsrtRange;
                            dmmVoltSetting.MeasureApertureTime = item.ForceTime * 0.7;

                            dmmVoltSetting.TriggerCount = item.SweepRiseCount;
                            dmmVoltSetting.TriggerInputDelay = 10e-6;

                            //-------------------------------------------------------------------------------------------
                            // DMM detector curr Msrt
                            dmmDetectorCurrSetting = new DmmSettingData();

                            dmmDetectorCurrSetting.MeasureFunction = EDmmMeasureFunc.DC_CURRENT;
                            dmmDetectorCurrSetting.MeasureIntegrationUnit = EDmmDcIntegrationUnit.Aperture;
                            dmmDetectorCurrSetting.MeasureRange = item.DetectorMsrtRange;
                            dmmDetectorCurrSetting.MeasureApertureTime = item.ForceTime * 0.7;

                            dmmDetectorCurrSetting.TriggerCount = item.SweepRiseCount;
                            dmmDetectorCurrSetting.TriggerInputDelay = 10e-6;

                            break;
                        }
                    default:
                        {
                            Console.WriteLine("[SS400Device], SetParamToMeter(), Not Suppoerted Test item: " + item.MsrtType.ToString());
                            this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                            return false;
                        }
                } // end case

                if (this._dmmVoltMsrt != null && dmmVoltSetting != null)
                {
                    if (!this._dmmVoltMsrt.SetParameterToDMM(index, dmmVoltSetting))
                    {
                        Console.WriteLine("[SS400Device], SetParamToMeter(), DMM_Volt Err msg: " + this._dmmVoltMsrt.ErrorMsg);
                        
                        return false;
                    }
                }

                if (this._dmmPhotoCurrentMsrt != null && dmmDetectorCurrSetting != null)
                {
                    if (!this._dmmPhotoCurrentMsrt.SetParameterToDMM(index, dmmDetectorCurrSetting))
                    {
                        Console.WriteLine("[SS400Device], SetParamToMeter(), DMM_Detector Err msg: " + this._dmmPhotoCurrentMsrt.ErrorMsg);
                        
                        return false;
                    }
                }

            } // end for-loop


            if (this._iRevSrcDev != null)
            {
                if (!this._iRevSrcDev.SetParamToMeter(revSetting.ToArray()))
                {
                    return false;
                }
            }

            return true;
        }

        public bool MeterOutput(uint[] activateChannels, uint index)
        {
            if (this._errorNum != EDevErrorNumber.Device_NO_Error)
            {
                return false;
            }

            if (this._elcSetting == null || this._elcSetting.Length == 0)
            {
                this._errorNum = EDevErrorNumber.NoSourceMeterParamSettingData;
                return false;
            }

            if (index > this._elcSetting.Length)
            {
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return false;
            }

            //-----------------------------------------------------------------------
            // Trigger Delay Time
            double forceDelay = this._elcSetting[index].ForceDelayTime;

            if (forceDelay != 0.0d)
            {
                this.DelayTime(forceDelay);
            }

            //-----------------------------------------------------------------------
            // RunTestItem
            uint deviceID = 0;

            if (this._elcSetting[index].KeyName.Contains("IZ") || this._elcSetting[index].KeyName.Contains("VR"))
            {
                if (this._iRevSrcDev != null)
                {
                    if(!this._iRevSrcDev.MeterOutput(new uint[]{0}, this._elcSetting[index].Order))
                    {
                        return false;
                    }

                    this._iRevSrcDev.TurnOff();
                }
            }
            else
            {
                if (!this.RunTestItem(deviceID, index))
                {
                    string errMsg = string.Empty;

                    for (int cnt = 0; cnt < this._tempStatusQueue.Count; cnt++)
                    {
                        errMsg = this._tempStatusQueue[cnt];

                        Console.WriteLine("[SS400Device], MeterOutput(), RunTestItem = false, ErrMsg = " + errMsg);
                    }
  
                    return false;
                }
            }

            bool rtn = true;

            if (this._isSyncAcqMsrtData)
            {
                rtn = this.AcquireMsrtData(null, index);
            }

            return rtn;
        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex, double applyValue)
        {
            return true;
        }

        public double[] GetDataFromMeter(uint channel, uint index)
        {
            if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            if (!this._isSyncAcqMsrtData)
            {
                if (this._isDeviceLog)
                {
                    Console.WriteLine("[SS400Device], GetDataFromMeter(), Async Acq MsrtData");
                }
                
                string status = string.Empty;

                if (!this._isContactFailSkip)
                {
                if (!this.WaitTriggerComplete(this._estimatedSequnceTime, out status))
                {
                    this._conn.SendCommand(SCPI.SrcOutput_CH1(false));

                    if (this._dmmVoltMsrt != null)
                    {
                        this._dmmVoltMsrt.AbortTrigger();
                    }

                    if (this._dmmPhotoCurrentMsrt != null)
                    {
                        this._dmmPhotoCurrentMsrt.AbortTrigger();
                    }

                    if (this.IsLoadFaultStatusEvent(status))
                    {
                        this._isContactFailSkip = true;
                    }
                }

                if (this._dmmVoltMsrt != null)
                {
                    this._dmmVoltMsrt.WaitTriggerIdle();
                }

                if (this._dmmPhotoCurrentMsrt != null)
                {
                    this._dmmPhotoCurrentMsrt.WaitTriggerIdle();
                }
                }

                this.AcquireMsrtData(null, index);

                this._conn.SendCommand(SCPI.SrcOutput_CH1(false));
            }

            return this._acquireData[(int)channel][index];
        }

        public double[] GetApplyDataFromMeter(uint channel, uint index)
        {
            if (this._elcSetting == null || index > this._elcSetting.Length)
            {
                double[] data = new double[0];

                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;

                return data;
            }

            return this._applyData[(int)channel][index];
        }

        public double[] GetSweepPointFromMeter(uint channel, uint index)
        {
            if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            return this._elcSetting[index].SweepCustomValue;
        }

        public double[] GetSweepResultFromMeter(uint channel, uint index)
        {
            if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;

                return null;
            }

            return this._sweepResult[(int)channel][index];
        }

        public double[] GetTimeChainFromMeter(uint channel, uint index)
        {
            if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;

                return null;
            }

            return this._timeChain[(int)channel][index];
        }

        public void TurnOff(double delay, bool isOpenRelay)
        {
            this.TurnOff();

            this.DelayTime(delay);
        }

        public void TurnOff()
        {
            string cmd = string.Empty;

            cmd += SCPI.SrcOutput_CH1(false);

            this._conn.SendCommand(cmd);
        }

        public void Output(uint point, bool active)
        {
            
        }

        public byte Input(uint point)
        {
            return 0;
        }

        public byte InputB(uint point)
        {
            byte result = 0x00;

            return result;
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
