using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;

using MPI.Tester.Device.SourceMeter.Keysight;

namespace MPI.Tester.Device.SourceMeter
{
    public class KeysightB2900A : ISourceMeter
    {
        private SCPI _keysightSCPI = new SCPI();
        
        private bool _isDeviceLog = false;
        
        private IConnect _conn;

        private string _devSN;
        private string _devHwVer;
        private string _devSwVer;

        private MPI.PerformanceTimer _pt;

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

        private bool _isDualSMU;

        private bool _isSyneAcqMsrtData;

        public KeysightB2900A()
        {
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this._pt = new PerformanceTimer();

            this._acquireData = new List<double[][]>();

            this._timeChain = new List<double[][]>();

            this._sweepResult = new List<double[][]>();

            this._applyData = new List<double[][]>();

            this._strSweepTrigList = new List<string[][]>();

            this._isDualSMU = false;

            this._isSyneAcqMsrtData = true;
        }

        public KeysightB2900A(ElecDevSetting devConfig) : this()
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

        private void SetConfig()
        {
            string cmd = string.Empty;

            //---------------------------------------------------------
            // Reset Device
            this._conn.SendCommand(SCPI.RESET);
            this._conn.SendCommand(SCPI.CLEAR_STATUS);

            //---------------------------------------------------------
            cmd += this._keysightSCPI.SysBeeper(false);
            cmd += this._keysightSCPI.SysDisplay(false);
            cmd += this._keysightSCPI.SysLineFreq();
            cmd += this._keysightSCPI.SrcAutoRange_CH1(false);
            cmd += this._keysightSCPI.MsrtAutoRange_CH1(false);
            cmd += this._keysightSCPI.MsrtAutoAperture_CH1(false);

            if (this._devConfig.SrcSensingMode == ESrcSensingMode._2wire)
            {
                cmd += this._keysightSCPI.MsrtRemoteSensing_CH1(false);
            }
            else
            {
                cmd += this._keysightSCPI.MsrtRemoteSensing_CH1(true);
            }

            bool isOutputFilter = false;

            cmd += SCPI.FORM_CURR_VOLT_CH1;

            cmd += this._keysightSCPI.SrcWaitTime_CH1(false);
            cmd += this._keysightSCPI.MsrtWaitTime_CH1(false);
            cmd += this._keysightSCPI.SrcOutputFilter_CH1(isOutputFilter);
            cmd += this._keysightSCPI.SrcOutputOffMode_CH1(CPD.OUTP_ZERO);
            cmd += this._keysightSCPI.SrcOutputAutoON_CH1(false);
            cmd += this._keysightSCPI.SrcOutputLowState_CH1(true);

            if (this._isDualSMU)
            {
                cmd += this._keysightSCPI.SrcAutoRange_CH2(false);
                cmd += this._keysightSCPI.MsrtAutoRange_CH2(false);

                cmd += this._keysightSCPI.MsrtRemoteSensing_CH2(false);
                cmd += SCPI.FORM_CURR_VOLT_CH2;
          
                cmd += this._keysightSCPI.SrcWaitTime_CH2(false);
                cmd += this._keysightSCPI.MsrtWaitTime_CH2(false);
                cmd += this._keysightSCPI.SrcOutputFilter_CH2(isOutputFilter);
                cmd += this._keysightSCPI.SrcOutputOffMode_CH2(CPD.OUTP_ZERO);
                cmd += this._keysightSCPI.SrcOutputAutoON_CH2(false);
                cmd += this._keysightSCPI.SrcOutputLowState_CH2(false);
                cmd += this._keysightSCPI.MsrtAutoAperture_CH2(false);
            }

           // cmd += ":SOUR:SWE:RANG FIX\n";
   
            this._conn.SendCommand(cmd);

            string msg = string.Empty;

            this.GetErrorMsg(out msg);
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
            if (item.ForceTime > B2900ASpec.PROG_MAX_APPLY_TIME || item.ForceTime < B2900ASpec.PROG_MIN_APPLY_TIME)
            {
                return false;
            }

            if (item.ForceDelayTime > B2900ASpec.PROG_MAX_APPLY_TIME || item.ForceDelayTime < 0.0d)
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
            
            string rtnStr = string.Empty;

            //-----------------------------------------------------------------------------------------------
            this._conn.SendCommand(SCPI.QUERY_COMPLETE);

            do
            {
                rtnStr = string.Empty;
                
                this._conn.WaitAndGetData(out rtnStr);

                System.Threading.Thread.Sleep(0);
            }
            while (rtnStr.Trim() != SCPI.FLAG_COMPLETE);

            uint pulseCount = 1;
            string[] strBuf1;
            string[] strBuf2;

            switch (item.MsrtType)
            {
                case EMsrtType.FIMV:
                    {
                        //-----------------------------------------------------------------------------------------------
                        if (!item.KeyName.Contains("IFWLA"))
                        {
                            if (item.SourceFunction != ESourceFunc.CW)
                            {
                                this._conn.SendCommand(this._keysightSCPI.SrcBaseLevel_CH1(0.0d));
                                this._conn.SendCommand(this._keysightSCPI.SrcFuncMode_CH1(CPD.FUNC_VOLT, 20));
                            }
                        }

                        //-----------------------------------------------------------------------------------------------
   
                        double dutI = 0.0d;
                        double dutV = 0.0d;

                        // Format (VOLT,CURR)
                        this._conn.SendCommand(":SENS1:DATA? 0");
                        strBuf1 = this.GetDevicePrintValueToArray(',');

                        Double.TryParse(strBuf1[0], out dutV);

                        this._acquireData[(int)id][index][0] = dutV;

                        break;
                    }
                case EMsrtType.FVMI:
                    {
                        //-----------------------------------------------------------------------------------------------
                        this._conn.SendCommand(this._keysightSCPI.SrcFuncMode_CH1(CPD.FUNC_VOLT, 20));
                        //-----------------------------------------------------------------------------------------------
                        
                        double dutI = 0.0d;
                        double dutV = 0.0d;

                        // Format (VOLT,CURR)
                        this._conn.SendCommand(":SENS1:DATA? 0");
                        strBuf1 = this.GetDevicePrintValueToArray(',');

                        Double.TryParse(strBuf1[1], out dutI);

                        this._acquireData[(int)id][index][0] = dutI;

                        break;
                    }
                case EMsrtType.FIMVLOP:
                    {
                        //-----------------------------------------------------------------------------------------------
                        this._conn.SendCommand(this._keysightSCPI.SrcBaseLevel_CH1(0.0d));
                        this._conn.SendCommand(this._keysightSCPI.SrcFuncMode_CH1(CPD.FUNC_VOLT, 20));
                        //-----------------------------------------------------------------------------------------------

                        double dutI = 0.0d;
                        double dutV = 0.0d;
                        double pdCurr = 0.0d;
                        double pdCnt = 0.0d;

                        // Format (VOLT,CURR)
                        this._conn.SendCommand(":SENS1:DATA? 0");
                        strBuf1 = this.GetDevicePrintValueToArray(',');

                        this._conn.SendCommand(":SENS2:DATA? 0");
                        strBuf2 = this.GetDevicePrintValueToArray(',');

                        if (item.SourceFunction == ESourceFunc.QCW)
                        {
                            pulseCount = item.PulseCount;

                            int count = 0;
                            uint avgCnt = pulseCount - item.CalcMsrtFromPulseIndex + 1;
                            double avgI = 0.0d;
                            double avgV = 0.0d;
                            double avgP = 0.0;
                            double tempI = 0.0d;
                            double tempV = 0.0d;
                            double tempP = 0.0d;

                            for (int i = 1; i <= pulseCount; i++)
                            {
                                if (i >= item.CalcMsrtFromPulseIndex)  // // CalcMsrtFromPulseIndex: base 1
                                {
                                    Double.TryParse(strBuf1[count + 1], out tempI);   // DUT_I
                                    Double.TryParse(strBuf1[count], out tempV);       // DUT_V
                                    Double.TryParse(strBuf2[count + 1], out tempP);   // PD_I

                                    avgI += tempI;
                                    avgV += tempV;
                                    avgP += tempP;
                                }

                                count += 2;
                            }

                            dutI = avgI / avgCnt;
                            dutV = avgV / avgCnt;
                            pdCurr = avgP / avgCnt;
                        }
                        else
                        {
                            Double.TryParse(strBuf1[0], out dutV);
                            Double.TryParse(strBuf1[1], out dutI);
                            Double.TryParse(strBuf2[1], out pdCurr);
                        }

                        this._applyData[(int)id][index][0] = dutI;

                        this._acquireData[(int)id][index][0] = dutV;
                        this._acquireData[(int)id][index][1] = pdCurr;
                        this._acquireData[(int)id][index][2] = pdCnt;
                        this._acquireData[(int)id][index][3] = 0.0d;
                        this._acquireData[(int)id][index][4] = 0.0d;

                        //this._conn.SendCommand(SCPI.SrcBaseLevel_CH2(0.0d));

                        break;
                    }
                case EMsrtType.PIV:
                    {
                        #region >>> PIV Reading Buffer <<<

                        //-----------------------------------------------------------------------------------------------
                        this._conn.SendCommand(this._keysightSCPI.SrcBaseLevel_CH1(0.0d));
                        this._conn.SendCommand(this._keysightSCPI.SrcFuncMode_CH1(CPD.FUNC_VOLT, 20));
                        //-----------------------------------------------------------------------------------------------

                        double[] iArray = null;
                        double[] vArray = null;
                        double[] pArray = null;

                        uint stepRisingCnt = item.SweepRiseCount;
                        pulseCount = 1;

                        // Format (VOLT,CURR)
                        this._conn.SendCommand(":SENS1:DATA? 0");
                        strBuf1 = this.GetDevicePrintValueToArray(',');
                  
                        this._conn.SendCommand(":SENS2:DATA? 0");
                        strBuf2 = this.GetDevicePrintValueToArray(',');

                        iArray = new double[stepRisingCnt];
                        vArray = new double[stepRisingCnt];
                        pArray = new double[stepRisingCnt];

                        if (item.SourceFunction == ESourceFunc.QCW)
                        {
                            pulseCount = item.PulseCount;

                            int count = 0;
                            uint avgCnt = pulseCount - item.CalcMsrtFromPulseIndex + 1;
                            double avgI = 0.0d;
                            double avgV = 0.0d;
                            double avgP = 0.0;
                            double tempI = 0.0d;
                            double tempV = 0.0d;
                            double tempP = 0.0d;

                            if (strBuf1.Length != (stepRisingCnt * pulseCount * 2))
                            {
                                return false;
                            }

                            for (int i = 0; i < stepRisingCnt; i++)
                            {
                                avgI = 0.0d;
                                avgV = 0.0d;
                                avgP = 0.0d;
                                
                                for (int j = 1; j <= pulseCount; j++)
                                {
                                    if (j >= item.CalcMsrtFromPulseIndex)  // CalcMsrtFromPulseIndex: base 1
                                    {
                                        Double.TryParse(strBuf1[count + 1], out tempI);   // DUT_I
                                        Double.TryParse(strBuf1[count], out tempV);       // DUT_V
                                        Double.TryParse(strBuf2[count + 1], out tempP);   // PD_I

                                        avgI += tempI;
                                        avgV += tempV;
                                        avgP += tempP;
                                    }

                                    count += 2;
                                }

                                avgI = avgI / avgCnt;
                                avgV = avgV / avgCnt;
                                avgP = avgP / avgCnt;

                                iArray[i] = avgI;
                                vArray[i] = avgV;
                                pArray[i] = avgP;
                            }

                        }
                        else
                        {
                            if (strBuf1.Length != (stepRisingCnt * 2))
                            {
                                string errMsg = string.Empty;

                                this.GetErrorMsg(out errMsg);

                                Console.WriteLine("[B2900Device], AcquireMsrtData(), PIV Reading Buffer Error: " + errMsg);
                                
                                return false;
                            }
                            
                            for (int i = 0; i < stepRisingCnt; i++)   
                            {
                                Double.TryParse(strBuf1[i * 2 + 1], out iArray[i]);   // DUT_I
                                Double.TryParse(strBuf1[i * 2], out vArray[i]);       // DUT_V
                                Double.TryParse(strBuf2[i * 2 + 1], out pArray[i]);   // PD_I
                            }
                        }

                        if (this._isDeviceLog)
                        {
                            //---------------------------------------------------------------------------------------------------------------------------------------
                            // Log Buffer Reading Raw Data
                            string fileNameAndPath = string.Format(@"C:\_Keysight_Log\kst_piv_{0}.csv", DateTime.Now.ToString("MMdd_HH_mm_ss"));
                            List<string[]> writeData = new List<string[]>();
                            string[] row = new string[6];
                            int currentIdx = 0;
                            writeData.Add(new[] { "Rc", "Pc", "I_1", "V_1", "I_2", "V_2" });

                            for (int i = 0; i < stepRisingCnt; i++)
                            {
                                for (int j = 0; j < pulseCount; j++)
                                {
                                    row[0] = (i + 1).ToString();
                                    row[1] = (j + 1).ToString();
                                    row[2] = strBuf1[currentIdx + 1].ToString();  // I_1
                                    row[3] = strBuf1[currentIdx].ToString();      // V_1
                                    row[4] = strBuf2[currentIdx + 1].ToString();  // I_2
                                    row[5] = strBuf2[currentIdx].ToString();      // V_2

                                    writeData.Add(row.Clone() as string[]);

                                    currentIdx += 2;
                                }

                            }

                            CSVUtil.WriteCSV(fileNameAndPath, writeData);
                            //---------------------------------------------------------------------------------------------------------------------------------------
                        }
                        //this._elcSetting[index].SweepCustomValue = iArray;
                        this._acquireData[(int)id][index] = vArray;
                        this._sweepResult[(int)id][index] = pArray;

                        //this._conn.SendCommand(SCPI.SrcBaseLevel_CH2(0.0d));

                        #endregion

                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            return true;
        }

        private bool GetErrorMsg(out string msg)
        {
            msg = string.Empty;

            this._conn.SendCommand(SCPI.QUERY_SYS_ERR);

            this._conn.WaitAndGetData(out msg);

            if (msg.Contains("No error"))
            {
                return false;
            }

            Console.WriteLine("[B2900Device], GetErrorMsg()," + msg);

            return true;
        }

        private bool GetOperStatus(out ushort uStatus)
        {
            string status = string.Empty;

            this._conn.SendCommand(SCPI.QUERY_STAT_OPER);

            this._conn.WaitAndGetData(out status);

            status = status.Trim();

            uStatus = 0;

            ushort.TryParse(status, out uStatus);

            if (uStatus == SCPI.BITS_STAT_READY)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region >>> B2900A Program <<<

        private void ResetStatus()
        {
            string cmd = string.Empty;

            cmd += SCPI.CLEAR_STATUS;
            cmd += this._keysightSCPI.SysStatusReset_CH1();
            cmd += this._keysightSCPI.SysStatusReset_CH2();

            this._conn.SendCommand(cmd);
        }

        private bool RunTestItem(uint deviceID, uint index)
        {
            double avoidTimeOutDelay = 0.0d;

            this._isSyneAcqMsrtData = true;

            ElectSettingData item = this._elcSetting[index];

            #region >>> Proberty <<<

            double armDelay = 0.0d;

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
            double msrtAperTime1 = 0.0d;

            // Channel-2
            string srcIV2 = string.Empty;
            string srcMode2 = string.Empty;

            string srcShap2 = string.Empty;
            double srcRange2 = 0.0d;
            double srcPulseLvl2 = 0.0d;
            double srcBiasLvl2 = 0.0d;

            double pulseWidth2 = 0.0d; //s
            double period2 = 0.0d;

            string msrtMode2 = string.Empty;
            double msrtAcqDelay2 = 0.0d;
            double msrtRange2 = 0.0d;
            double msrtClamp2 = 0.0d;
            double nplc2 = 0.0d;
            double msrtAperTime2 = 0.0d;

            uint trigCount = 1;

            #endregion

            string cmd = string.Empty;

            // Start SrcMeter Test Sequence
            switch (item.MsrtType)
            {
                case EMsrtType.FIMV:
                    {
                        #region >>> FIMV Cmd <<<

                        if (item.KeyName.Contains("IFWLB"))
                        {
                            if (item.SourceFunction == ESourceFunc.CW)
                            {
                                cmd = this._keysightSCPI.SrcBaseLevel_CH1(0.0d);
                                cmd += this._keysightSCPI.SrcFuncMode_CH1(CPD.FUNC_VOLT, 20);
                                this._conn.SendCommand(cmd);
                            }

                            break;
                        }

                        string sysArmSignal = string.Empty;

                        // Channel-1
                        srcIV1 = CPD.FUNC_CURR;

                        switch (item.SourceFunction)
                        {
                            case ESourceFunc.CW:
                                {
                                    srcMode1 = CPD.MODE_FIX;
                                    srcShap1 = CPD.SHAPE_DC;  // PULS / DC

                                    if (item.IsAutoTurnOff)
                                    {
                                        // i.e. IZ, IF items
                                        srcBiasLvl1 = 0.0d;   // 在Pulse Mode下, 一定要設0
                                        srcPulseLvl1 = item.ForceValue;   // via pulse sweep List, 
                                        pulseWidth1 = item.ForceTime + (0.016666 * item.MsrtNPLC) * 1.2d;
                                        msrtAcqDelay1 = item.ForceTime;
                                        period1 = pulseWidth1;
                                    }
                                    else
                                    {
                                        // i.e. LOPWL item
                                        srcBiasLvl1 = item.ForceValue;
                                        srcPulseLvl1 = item.ForceValue;   // via pulse sweep List, 
                                        pulseWidth1 = item.ForceTime;
                                        msrtAcqDelay1 = pulseWidth1 * 0.7;  // Pulse width 的後 30% 做量測
                                        period1 = item.ForceTime;
                                    }

                                    armDelay = 0.0d;
                                    trigCount = 1;
                                    sysArmSignal = CPD.TRIG_INTERNAL;
                                    break;
                                }
                            case ESourceFunc.PULSE:
                                {
                                    this._isSyneAcqMsrtData = false;

                                    srcMode1 = CPD.MODE_FIX;
                                    srcShap1 = CPD.SHAPE_PULSE;  // PULS / DC

                                    srcBiasLvl1 = 0.0d;   // 在Pulse Mode下, 一定要設0
                                    srcPulseLvl1 = item.ForceValue;   // via pulse sweep List, 
                                    pulseWidth1 = item.ForceTime;
                                    msrtAcqDelay1 = pulseWidth1 * 0.7;  // Pulse width 的後 30% 做量測

                                    if (pulseWidth1 <= 400e-6d)
                                    {
                                        srcBiasLvl1 = 0.001d;   // 在Pulse Mode下, 一定要設0
                                    }

                                    if (item.IsPulseMode)
                                    {
                                        period1 = Math.Round((item.ForceTime + item.TurnOffTime), 6, MidpointRounding.AwayFromZero);
                                        period1 = period1 * 0.75;  // 估算下次Triger的通訊時間, 當作Duty休息, 爭取時間
                                    }
                                    else
                                    {
                                        period1 = item.ForceTime;
                                    }

                                    armDelay = 0.0d;
                                    trigCount = 1;
                       
                                    if (item.KeyName.Contains("IFWLA"))
                                    {
                                        sysArmSignal = CPD.TRIG_BUS;
                                    }
                                    else
                                    {
                                        sysArmSignal = CPD.TRIG_INTERNAL;
                                    }
                                  
                                    break;
                                }
                            case ESourceFunc.QCW:
                                {
                                    this._isSyneAcqMsrtData = false;

                                    srcMode1 = CPD.MODE_FIX;
                                    srcShap1 = CPD.SHAPE_PULSE;  // PULS / DC

                                    srcBiasLvl1 = 0.0d;   // 在Pulse Mode下, 一定要設0
                                    srcPulseLvl1 = item.ForceValue;   // via pulse sweep List, 
                                    pulseWidth1 = item.ForceTime;
                                    msrtAcqDelay1 = pulseWidth1 * 0.7;  // Pulse width 的後 30% 做量測

                                    if (pulseWidth1 <= 400e-6d)
                                    {
                                        srcBiasLvl1 = 0.001d;   // 在Pulse Mode下, 一定要設0
                                    }

                                    period1 = Math.Round((item.ForceTime + item.TurnOffTime), 6, MidpointRounding.AwayFromZero);

                                    armDelay = 0.0d;
                                    trigCount = item.PulseCount;

                                    if (item.KeyName.Contains("IFWLA"))
                                    {
                                        sysArmSignal = CPD.TRIG_BUS;
                                    }
                                    else
                                    {
                                        sysArmSignal = CPD.TRIG_INTERNAL;
                                    }

                                    break;
                                }
                        }

                        if (armDelay != 0.0d)
                        {
                            armDelay = armDelay - 5e-6;
                            armDelay = armDelay >= 0 ? armDelay : 0.0d;
                        }

                        srcRange1 = item.ForceRange;

                        msrtMode1 = CPD.FUNC_VOLT;
                        msrtRange1 = item.MsrtRange;
                        msrtClamp1 = item.MsrtProtection;

                        nplc1 = item.MsrtNPLC;

                        //----------------------------------------------------------------------------------------
                        // Ext Trigger Control
                        if (item.KeyName.Contains("IFWLA"))
                        {
                            cmd += this._keysightSCPI.ArmTriggerAssert_CH1(true, CPD.PIN_SPT_TRIG_OUT);
                        }
                        else
                        {
                            cmd += this._keysightSCPI.ArmTriggerAssert_CH1(false, string.Empty); 
                        }
                        //----------------------------------------------------------------------------------------

                        cmd += this._keysightSCPI.SrcConfig_CH1(srcIV1, srcMode1, srcShap1);

                        cmd += this._keysightSCPI.SrcRange_CH1(srcRange1);

                        cmd += this._keysightSCPI.SrcBaseLevel_CH1(srcBiasLvl1);

                        cmd += this._keysightSCPI.SrcPulseLevel_CH1(srcPulseLvl1);

                        cmd += this._keysightSCPI.TrigPulseDelay_CH1(0.0d);

                        cmd += this._keysightSCPI.TrigPulseWidth_CH1(pulseWidth1);

                        cmd += this._keysightSCPI.MsrtConfig_CH1(msrtMode1);

                        cmd += this._keysightSCPI.MsrtRange_CH1(msrtRange1);

                        cmd += this._keysightSCPI.MsrtClamp_CH1(msrtClamp1);

                        cmd += this._keysightSCPI.MsrtNPLC_CH1(nplc1);

                        //---------------------------------------------------------------------------------------------
                        // using triger layer for continous pulse
                        //---------------------------------------------------------------------------------------------
                        cmd += this._keysightSCPI.ArmStimulus_CH1(sysArmSignal);

                        cmd += this._keysightSCPI.ArmDelay_CH1(armDelay);

                        cmd += this._keysightSCPI.TrigCount_CH1(trigCount);

                        cmd += this._keysightSCPI.TrigSrcDelay_CH1(0.0d);

                        cmd += this._keysightSCPI.TrigTranSignal_CH1(CPD.TRIG_TIMER);

                        cmd += this._keysightSCPI.TrigPeroid_CH1(period1);

                        cmd += this._keysightSCPI.TrigAcqSignal_CH1(CPD.TRIG_TIMER);

                        cmd += this._keysightSCPI.TrigAcqDelay_CH1(msrtAcqDelay1);

                        cmd += this._keysightSCPI.SrcOutput_CH1(true);

                        cmd += this._keysightSCPI.TrigInit(true, false);

                        this._conn.SendCommand(cmd);

                        if (sysArmSignal == CPD.TRIG_BUS)
                        {
                            // Query Status
                            ushort uStatus = 0;
                            int waitTransArm = -1;

                            do
                            {
                                this.GetOperStatus(out uStatus);

                                waitTransArm = (uStatus & SCPI.BITS_03_MASK_WAIT_TRANS_ARM) >> 3;

                                System.Threading.Thread.Sleep(0);
                                //Console.WriteLine("[B2900Device], RunTestItem(), Wait for Trams Arm =" + waitTransArm.ToString());

                            } while (waitTransArm != 1);

                            this._conn.SendCommand(SCPI.TRIGGER);
                        }

                        #endregion

                        break;
                    }
                case EMsrtType.FVMI:
                    {
                        #region >>> FVMI Cmd <<<
                        //----------------------------------------------------------------------------------
                        // Channel-1
                        srcIV1 = CPD.FUNC_VOLT;
                        srcMode1 = CPD.MODE_FIX;
                        srcShap1 = CPD.SHAPE_PULSE;  // PULS / DC

                        srcRange1 = item.ForceRange;
                        srcPulseLvl1 = item.ForceValue;   // via pulse sweep List, 
                        srcBiasLvl1 = 0.0d;   // 在Pulse Mode下, 一定要設0

                        //pulseWidth1 = item.ForceTime;
                        pulseWidth1 = item.ForceTime + (0.016666 * item.MsrtNPLC) * 1.2d;
                        period1 = pulseWidth1;

                        msrtMode1 = CPD.FUNC_CURR;
                        msrtRange1 = item.MsrtRange;
                        msrtClamp1 = item.MsrtProtection;

                        nplc1 = item.MsrtNPLC;

                        msrtAcqDelay1 = item.ForceTime;  
                       // msrtAcqDelay1 = pulseWidth1 * 0.7;
                        trigCount = 1;

                        cmd += this._keysightSCPI.ArmTriggerAssert_CH1(false, string.Empty); 

                        cmd += this._keysightSCPI.SrcConfig_CH1(srcIV1, srcMode1, srcShap1);
              
                        cmd += this._keysightSCPI.SrcRange_CH1(srcRange1);
               
                        cmd += this._keysightSCPI.SrcBaseLevel_CH1(srcBiasLvl1);
                    
                        cmd += this._keysightSCPI.SrcPulseLevel_CH1(srcPulseLvl1);
                      
                        cmd += this._keysightSCPI.TrigPulseDelay_CH1(0.0d);
                     
                        cmd += this._keysightSCPI.TrigPulseWidth_CH1(pulseWidth1);
                      
                        cmd += this._keysightSCPI.MsrtConfig_CH1(msrtMode1);
                     
                        cmd += this._keysightSCPI.MsrtRange_CH1(msrtRange1);
                    
                        cmd += this._keysightSCPI.MsrtClamp_CH1(msrtClamp1);
                       
                        cmd += this._keysightSCPI.MsrtNPLC_CH1(nplc1);
                      
                        //---------------------------------------------------------------------------------------------
                        // using triger layer for continous pulse
                        //---------------------------------------------------------------------------------------------
                        cmd += this._keysightSCPI.ArmStimulus_CH1(CPD.TRIG_INTERNAL);

                        cmd += this._keysightSCPI.TrigCount_CH1(trigCount);
                
                        cmd += this._keysightSCPI.TrigSrcDelay_CH1(0.0d);
                     
                        cmd += this._keysightSCPI.TrigTranSignal_CH1(CPD.TRIG_TIMER);
                     
                        cmd += this._keysightSCPI.TrigPeroid_CH1(period1);

                        cmd += this._keysightSCPI.TrigAcqSignal_CH1(CPD.TRIG_TIMER);
                      
                        cmd += this._keysightSCPI.TrigAcqDelay_CH1(msrtAcqDelay1);
                     
                        cmd += this._keysightSCPI.SrcOutput_CH1(true);
                     
                        cmd += this._keysightSCPI.TrigInit(true, false);

                        this._conn.SendCommand(cmd);

                        #endregion

                        break;
                    }
                case EMsrtType.FIMVLOP:
                    {
                        #region >>> FIMVLOP Cmd <<<

                        //----------------------------------------------------------------------------------
                        // Channel-1
                        srcIV1 = CPD.FUNC_CURR;
                        srcMode1 = CPD.MODE_FIX;
                        srcShap1 = CPD.SHAPE_PULSE;  // PULS / DC

                        srcRange1 = item.ForceRange;
                        srcPulseLvl1 = item.ForceValue;   // via pulse sweep List, 
                        srcBiasLvl1 = 0.0d;   // 在Pulse Mode下, 一定要設0

                        pulseWidth1 = item.ForceTime;

                        msrtMode1 = CPD.FUNC_VOLT;
                        msrtRange1 = item.MsrtRange;
                        msrtClamp1 = item.MsrtProtection;

                        nplc1 = item.MsrtNPLC;

                        msrtAcqDelay1 = pulseWidth1 * 0.7;  // Pulse width 的後 30% 做量測

                        if (pulseWidth1 <= 400e-6d)
                        {
                            srcBiasLvl1 = 0.001d;   // 在Pulse Mode下, 一定要設0
                        }

                        //----------------------------------------------------------------------------------
                        // Channel-2
                        srcIV2 = CPD.FUNC_VOLT;
                        srcMode2 = CPD.MODE_FIX;
                        srcShap2 = CPD.SHAPE_DC;  // PULS / DC

                        srcRange2 = item.DetectorBiasValue;
                        srcPulseLvl2 = item.DetectorBiasValue;
                        srcBiasLvl2 = item.DetectorBiasValue;

                        pulseWidth2 = pulseWidth1;
                 

                        msrtMode2 = CPD.FUNC_CURR;
                        msrtRange2 = item.DetectorMsrtRange;
                        msrtClamp2 = item.DetectorMsrtRange;

                        nplc2 = nplc1;

                        msrtAcqDelay2 = msrtAcqDelay1;

                        switch (item.SourceFunction)
                        {
                            case ESourceFunc.CW:
                                {
                                    trigCount = 1;
                                    period1 = item.ForceTime;
                                    break;
                                }
                            case ESourceFunc.PULSE:
                                {
                                    trigCount = 1;

                                    if (item.IsPulseMode)
                                    {
                                        period1 = Math.Round((item.ForceTime + item.TurnOffTime), 6, MidpointRounding.AwayFromZero);
                                        period1 = period1 * 0.75;  // 估算下次Triger的通訊時間, 當作Duty休息, 爭取時間
                                    }
                                    else
                                    {
                                        period1 = item.ForceTime;
                                    }
                                    break;
                                }
                            case ESourceFunc.QCW:
                                {
                                    period1 = Math.Round((item.ForceTime + item.TurnOffTime), 6, MidpointRounding.AwayFromZero);
                                    trigCount = item.PulseCount;
                                    break;
                                }
                        }

                        if (item.SourceFunction == ESourceFunc.QCW)
                        {
                            period1 = Math.Round((item.ForceTime + item.TurnOffTime), 6, MidpointRounding.AwayFromZero);
                            trigCount = item.PulseCount;
                        }

                        period2 = period1;

              
                      //  cmd += this._keysightSCPI.ArmTriggerAssert_CH1(false, string.Empty); 
                        ///////////////////////////////////////////////////////////////////////////////////////////
                        cmd += this._keysightSCPI.ArmTriggerAssert_CH1(true, CPD.PIN_SPT_TRIG_OUT);   // for debug
                       // armDelay = 0.003d;
                        ////////////////////////////////////////////////////////////////////////////////////////////

                        cmd += this._keysightSCPI.SrcConfig_CH1(srcIV1, srcMode1, srcShap1);
                        cmd += this._keysightSCPI.SrcConfig_CH2(srcIV2, srcMode2, srcShap2);

                        cmd += this._keysightSCPI.SrcRange_CH1(srcRange1);
                        cmd += this._keysightSCPI.SrcRange_CH2(srcRange2);

                        cmd += this._keysightSCPI.SrcBaseLevel_CH1(srcBiasLvl1);
                        cmd += this._keysightSCPI.SrcBaseLevel_CH2(srcBiasLvl2);

                        cmd += this._keysightSCPI.SrcPulseLevel_CH1(srcPulseLvl1);
                        cmd += this._keysightSCPI.SrcPulseLevel_CH2(srcPulseLvl2);

                        cmd += this._keysightSCPI.TrigPulseDelay_CH1(0.0d);
                        cmd += this._keysightSCPI.TrigPulseDelay_CH2(0.0d);

                        cmd += this._keysightSCPI.TrigPulseWidth_CH1(pulseWidth1);
                        cmd += this._keysightSCPI.TrigPulseWidth_CH2(pulseWidth2);

                        cmd += this._keysightSCPI.MsrtConfig_CH1(msrtMode1);
                        cmd += this._keysightSCPI.MsrtConfig_CH2(msrtMode2);

                        cmd += this._keysightSCPI.MsrtRange_CH1(msrtRange1);
                        cmd += this._keysightSCPI.MsrtRange_CH2(msrtRange2);

                        cmd += this._keysightSCPI.MsrtClamp_CH1(msrtClamp1);
                        cmd += this._keysightSCPI.MsrtClamp_CH2(msrtClamp2);

                        cmd += this._keysightSCPI.MsrtNPLC_CH1(nplc1);
                        cmd += this._keysightSCPI.MsrtNPLC_CH2(nplc2);

                        //---------------------------------------------------------------------------------------------
                        // using triger layer for continous pulse
                        //---------------------------------------------------------------------------------------------
                        cmd += this._keysightSCPI.ArmStimulus_CH1(CPD.TRIG_INTERNAL);
                        cmd += this._keysightSCPI.ArmStimulus_CH2(CPD.TRIG_INTERNAL);

                        cmd += this._keysightSCPI.ArmDelay_CH1(armDelay);
						cmd += this._keysightSCPI.ArmDelay_CH2(armDelay);

                        cmd += this._keysightSCPI.TrigCount_CH1(trigCount);
                        cmd += this._keysightSCPI.TrigCount_CH2(trigCount);

                        cmd += this._keysightSCPI.TrigSrcDelay_CH1(0.0d);
                        cmd += this._keysightSCPI.TrigSrcDelay_CH2(0.0d);

                        cmd += this._keysightSCPI.TrigTranSignal_CH1(CPD.TRIG_TIMER);
                        cmd += this._keysightSCPI.TrigTranSignal_CH2(CPD.TRIG_TIMER);

                        cmd += this._keysightSCPI.TrigPeroid_CH1(period1);
                        cmd += this._keysightSCPI.TrigPeroid_CH2(period2);

                        cmd += this._keysightSCPI.TrigAcqSignal_CH1(CPD.TRIG_TIMER);
                        cmd += this._keysightSCPI.TrigAcqSignal_CH2(CPD.TRIG_TIMER);

                        cmd += this._keysightSCPI.TrigAcqDelay_CH1(msrtAcqDelay1);
                        cmd += this._keysightSCPI.TrigAcqDelay_CH2(msrtAcqDelay1);

                        cmd += this._keysightSCPI.SrcOutput_CH1(true);
                        cmd += this._keysightSCPI.SrcOutput_CH2(true);

                        cmd += this._keysightSCPI.TrigInit(true, true);

                        this._conn.SendCommand(cmd);

                        #endregion

                        break;
                    }
                case EMsrtType.PIV:
                    {
                        #region >>> Pulse List Sweep Cmd <<<

                        double start = item.SweepStart;
                        double end = item.SweepStop;
                        uint points = item.SweepRiseCount;

                        armDelay = 0.0;

                        //----------------------------------------------------------------------------------
                        // Channel-1
                        srcIV1 = CPD.FUNC_CURR;

                        if (item.SourceFunction == ESourceFunc.CW)
                        {
                            srcMode1 = CPD.MODE_SWEEP;
                            srcShap1 = CPD.SHAPE_DC;  // PULS / DC

                            trigCount = points;
                        }
                        else
                        {
                            srcMode1 = CPD.MODE_LIST;
                            srcShap1 = CPD.SHAPE_PULSE;  // PULS / DC

                            trigCount = (uint)this._strSweepTrigList[(int)deviceID][index].Length;
                        }

                        srcRange1 = item.ForceRange;

                        //srcRange1 = 3.0d;

                        srcPulseLvl1 = 0.0;   // via pulse sweep List, 
                        srcBiasLvl1 = 0.0d;

                        pulseWidth1 = item.ForceTime;
                        period1 = Math.Round((item.ForceTime + item.SweepTurnOffTime), 6, MidpointRounding.AwayFromZero);
                        
                        msrtMode1 = CPD.FUNC_VOLT;
                        msrtRange1 = item.MsrtRange;
                        msrtClamp1 = item.MsrtProtection;

                        nplc1 = item.MsrtNPLC;

                        if (srcShap1 == CPD.SHAPE_PULSE && pulseWidth1 <= 400e-6d)
                        {
                            srcBiasLvl1 = 0.001d;   // 在Pulse Mode下, 一定要設0
                        }

                       // nplc1 = 0.003;
                        msrtAcqDelay1 = Math.Round((pulseWidth1 * 0.7), 6, MidpointRounding.AwayFromZero);  // Pulse width 的後 30% 做量測

                        //----------------------------------------------------------------------------------
                        // Channel-2
                        srcIV2 = CPD.FUNC_VOLT;
                        srcMode2 = CPD.MODE_FIX;
                        srcShap2 = CPD.SHAPE_DC;  // PULS / DC

                        srcRange2 = item.DetectorBiasValue;
                        srcPulseLvl2 = item.DetectorBiasValue;
                        srcBiasLvl2 = item.DetectorBiasValue;

                        pulseWidth2 = pulseWidth1;
                        period2 = period1;

                        msrtMode2 = CPD.FUNC_CURR;
                        msrtRange2 = item.DetectorMsrtRange;
                        msrtClamp2 = item.DetectorMsrtRange;

                        nplc2 = nplc1;

                        msrtAperTime1 = Math.Round((pulseWidth1 - msrtAcqDelay1), 6, MidpointRounding.AwayFromZero);

                        msrtAcqDelay2 = msrtAcqDelay1;
                        msrtAperTime2 = msrtAperTime1;

                        //---------------------------------------------------------------------------------
                        cmd += this._keysightSCPI.ArmTriggerAssert_CH1(false, string.Empty); 

                        cmd += this._keysightSCPI.SrcConfig_CH1(srcIV1, srcMode1, srcShap1);
                        cmd += this._keysightSCPI.SrcConfig_CH2(srcIV2, srcMode2, srcShap2);

                        cmd += this._keysightSCPI.SrcRange_CH1(srcRange1);
                        cmd += this._keysightSCPI.SrcRange_CH2(srcRange2);

                        cmd += this._keysightSCPI.SrcBaseLevel_CH1(srcBiasLvl1);
                        cmd += this._keysightSCPI.SrcBaseLevel_CH2(srcBiasLvl2);

                        // cmd += SCPI.SrcPulseLevel_CH1(srcPulseLvl1);
                        if (item.SourceFunction == ESourceFunc.CW)
                        {
                            cmd += this._keysightSCPI.SrcSweep_CH1(start, end, points);
                        }
                        else
                        {
                            cmd += this._keysightSCPI.SrcListSweep_CH1(this._strSweepTrigList[(int)deviceID][index]);
                        }
               
                        cmd += this._keysightSCPI.SrcPulseLevel_CH2(srcPulseLvl2);

                        cmd += this._keysightSCPI.ArmDelay_CH1(armDelay);
                        cmd += this._keysightSCPI.ArmDelay_CH2(armDelay);

                        cmd += this._keysightSCPI.TrigPulseDelay_CH1(0.0d);
                        cmd += this._keysightSCPI.TrigPulseDelay_CH2(0.0d);

                        cmd += this._keysightSCPI.TrigPulseWidth_CH1(pulseWidth1);
                        cmd += this._keysightSCPI.TrigPulseWidth_CH2(pulseWidth2);

                        cmd += this._keysightSCPI.MsrtConfig_CH1(msrtMode1);
                        cmd += this._keysightSCPI.MsrtConfig_CH2(msrtMode2);

                        cmd += this._keysightSCPI.MsrtRange_CH1(msrtRange1);
                        cmd += this._keysightSCPI.MsrtRange_CH2(msrtRange2);

                        cmd += this._keysightSCPI.MsrtClamp_CH1(msrtClamp1);
                        cmd += this._keysightSCPI.MsrtClamp_CH2(msrtClamp2);

                       // cmd += this._keysightSCPI.MsrtNPLC_CH1(nplc1);
                       // cmd += this._keysightSCPI.MsrtNPLC_CH2(nplc2);

                        cmd += this._keysightSCPI.MsrtApertureTime_CH1(msrtAperTime1);
                        cmd += this._keysightSCPI.MsrtApertureTime_CH2(msrtAperTime2);
                        //---------------------------------------------------------------------------------------------
                        // using triger layer for continous pulse
                        //---------------------------------------------------------------------------------------------
                        cmd += this._keysightSCPI.ArmStimulus_CH1(CPD.TRIG_INTERNAL);
                        cmd += this._keysightSCPI.ArmStimulus_CH2(CPD.TRIG_INTERNAL);

                        cmd += this._keysightSCPI.TrigCount_CH1(trigCount);
                        cmd += this._keysightSCPI.TrigCount_CH2(trigCount);

                        cmd += this._keysightSCPI.TrigSrcDelay_CH1(0.0d);
                        cmd += this._keysightSCPI.TrigSrcDelay_CH2(0.0d);

                        cmd += this._keysightSCPI.TrigTranSignal_CH1(CPD.TRIG_TIMER);
                        cmd += this._keysightSCPI.TrigTranSignal_CH2(CPD.TRIG_TIMER);

                        cmd += this._keysightSCPI.TrigPeroid_CH1(period1);
                        cmd += this._keysightSCPI.TrigPeroid_CH2(period2);

                        cmd += this._keysightSCPI.TrigAcqSignal_CH1(CPD.TRIG_TIMER);
                        cmd += this._keysightSCPI.TrigAcqSignal_CH2(CPD.TRIG_TIMER);

                        cmd += this._keysightSCPI.TrigAcqDelay_CH1(msrtAcqDelay1);
                        cmd += this._keysightSCPI.TrigAcqDelay_CH2(msrtAcqDelay1);

                        cmd += this._keysightSCPI.SrcOutput_CH1(true);
                        cmd += this._keysightSCPI.SrcOutput_CH2(true);

                        cmd += this._keysightSCPI.TrigInit(true, true);

                        avoidTimeOutDelay = period1 * 1000.0d * trigCount ;   // unit = ms 

                        // 若整段預估時間超過1秒的話, 先Sleep預估時間的 60%, 再進行 Query Buffer, 避免Timeout發生 (Receive Timeout = 5s)
                        avoidTimeOutDelay = avoidTimeOutDelay > 1000.0d ? avoidTimeOutDelay * 0.6d : 0.0d;

                        this._conn.SendCommand(cmd);

                        #endregion

                        break;
                    }
                default:
                    {
                        return false;
                    }
            }


            if (avoidTimeOutDelay != 0.0d)
            {
                System.Threading.Thread.Sleep((int)avoidTimeOutDelay);
            }

            return true;
        }

        #endregion

        #region >>> Public Method <<<

        public bool Init(int deviceNum, string sourceMeterSN)
        {
            string resourceName = string.Format("TCPIP0::{0}::INSTR", sourceMeterSN);

            //this._conn = new IVIConnect(resourceName);

            LANSettingData lanSetting = new LANSettingData();

            lanSetting.IPAddress = sourceMeterSN;
        
            this._conn = new LANConnect(lanSetting);

            string rtnStr = string.Empty;

            if (!this._conn.Open(out rtnStr))
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                Console.WriteLine("[B2900Device], Init(), Connection Open Fail");
                return false;
            }

            //--------------------------------------------------------------------
            // Query Device Information
            this._conn.SendCommand(SCPI.QUERY_DEV_INFO);

            this._conn.WaitAndGetData(out rtnStr);

            if (rtnStr == string.Empty)
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                Console.WriteLine("[B2900Device], Init(), QueryIDN Fail");
                return false;
            }

            string[] devinfo = rtnStr.Trim().Split(',');

            if (devinfo.Length != 4)
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                Console.WriteLine("[B2900Device], Init(), QueryIDN Fail");
                return false;
            }

            string manufacturer = devinfo[0];

            if (!manufacturer.Contains("Keysight") && !manufacturer.Contains("Agilent"))
            {
                 this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                 Console.WriteLine("[B2900Device], Init(), Model Not Match, Query=" + manufacturer);
                return false;
            }

            string model = devinfo[1];
            string sn = devinfo[2];
            string hwVer = devinfo[3];

            this._devSN = sn;
            this._devSwVer = hwVer;
            this._devHwVer = "Keysight " + model;

            //--------------------------------------------------------------------------------------------------------
            if (model.Contains("2902") || model.Contains("2912"))
            {
                this._isDualSMU = true;
            }


            this._voltRange = B2900ASpec.B2911_DC_VOLT_RANGE;
            this._currRange = B2900ASpec.B2911_DC_CURR_RANGE;

            this._currPulseRange = B2900ASpec.B2911_PULSE_CURR_RANGE;
            this._voltPulseRange = B2900ASpec.B2911_PULSE_VOLT_RANGE;

            this._maxPulseWidth = B2900ASpec.B2911_MAX_PULSE_WIDTH;
            this._maxPulseDuty = B2900ASpec.B2911_MAX_PULSE_DUTY;

            if(model.Contains("2901") || model.Contains("2902"))
            {
                // B2901A & B2902A remove 10nA current range
                this._currRange = new double[3][];
                this._currRange[0] = new double[B2900ASpec.B2911_DC_CURR_RANGE[0].Length - 1];
                this._currRange[1] = new double[B2900ASpec.B2911_DC_CURR_RANGE[1].Length - 1];
                this._currRange[2] = new double[B2900ASpec.B2911_DC_CURR_RANGE[2].Length - 1];

                Array.Copy(B2900ASpec.B2911_DC_CURR_RANGE[0], 1, this._currRange[0], 0, (B2900ASpec.B2911_DC_CURR_RANGE[0].Length - 1));
                Array.Copy(B2900ASpec.B2911_DC_CURR_RANGE[1], 1, this._currRange[1], 0, (B2900ASpec.B2911_DC_CURR_RANGE[1].Length - 1));
                Array.Copy(B2900ASpec.B2911_DC_CURR_RANGE[2], 1, this._currRange[2], 0, (B2900ASpec.B2911_DC_CURR_RANGE[2].Length - 1));
            }

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
            this.SetConfig();

            //--------------------------------------------------------------------------------------------------------
            // Init Status
            ushort sStatus = 0;

            this.GetOperStatus(out sStatus);

            Console.WriteLine("[B2900Device], Init(), initial status = " + sStatus.ToString());

            return true;
        }

        public void Close()
        {
            this._conn.Close();
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

            //----------------------------------------------------------------------------------------
            this.ResetStatus();

            ////////////////////////////////////////////////////////////////////////////////////////
            // (3) Set Test Item to Meter
            ////////////////////////////////////////////////////////////////////////////////////////
            uint deviceID = 0;
            int region;

            for (uint index = 0; index < this._elcSetting.Length; index++)
            {
                this._applyData[(int)deviceID][index] = new double[1] { this._elcSetting[index].ForceValue };

                ElectSettingData item = this._elcSetting[index];

                if (!this.TimeUnitConvert(item))
                {
                    Console.WriteLine("[B2900Device], SetParamToMeter(), Time settings conflict;");
                    this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                    return false;
                }

                if (!this.FindSrcAndMsrtRange(item))
                {
                    if (!this.FindSrcAndMsrtPulseRange(item, out region))
                    {
                        Console.WriteLine("[B2900Device], SetParamToMeter(), Paramter out of boundry");
                        this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                        return false;
                    }
                }

                //-----------------------------------------------------------------------------------------
                // Paul 2014.03.14 取絕對值，IF>120mA IZ=10uA，會出現 Error Code 5007 
                // Force Range and Compliance Setting
                //-----------------------------------------------------------------------------------------
                item.ForceRange = Math.Abs(item.ForceRange);

                item.MsrtRange = Math.Abs(item.MsrtRange);

                item.MsrtProtection = Math.Abs(item.MsrtProtection);

                item.DetectorMsrtRange = Math.Abs(item.DetectorMsrtRange);

                switch (item.MsrtType)
                {
                    case EMsrtType.FIMV:
                    case EMsrtType.FVMI:
                        {
                            this._acquireData[(int)deviceID][index] = new double[1];
                            break;
                        }
                    case EMsrtType.FIMVLOP:
                        {
                            // DUT_Volt, Ipd-CHA, Icnt-CHA, Ipd-CHB, Ipd-CHB
                            this._acquireData[(int)deviceID][index] = new double[5];
                            
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

                            break;
                        }
                    default:
                        {
                            Console.WriteLine("[B2900Device], SetParamToMeter(), Not Suppoerted Test item: " + item.MsrtType.ToString());
                            this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                            return false;
                        }
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
            double forceDelay = this._elcSetting[index].ForceDelayTime * 1000.0d;

            if (forceDelay != 0.0d)
            {
                this.DelayTime(forceDelay);
            }

            //-----------------------------------------------------------------------
            // RunTestItem
            uint deviceID = 0;

            if (!this.RunTestItem(deviceID, index))
            {
                string msg = string.Empty;
                this.GetErrorMsg(out msg);

                Console.WriteLine("[B2900Device], MeterOuput_Err(), Trigger Error: " + msg);
                this._errorNum = EDevErrorNumber.MeterOutput_Ctrl_Err;

                return false;
            }

            bool rtn = true;

            if (this._isSyneAcqMsrtData)
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

            if (!this._isSyneAcqMsrtData)
            {
                this.AcquireMsrtData(null, index);
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

            cmd += this._keysightSCPI.SrcOutput_CH1(false);

            if (this._isDualSMU)
            {
                cmd += this._keysightSCPI.SrcOutput_CH2(false);
            }

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

            this._conn.SendCommand(SCPI.QUERY_DIO_DATA);

            string rtn = string.Empty;

            this._conn.WaitAndGetData(out rtn);

            rtn = rtn.Trim();

            ushort uDio = 0;

            ushort.TryParse(rtn, out uDio);

            result = (byte)((uDio >> 7) & 0x07);

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
