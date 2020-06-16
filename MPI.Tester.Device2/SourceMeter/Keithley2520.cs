using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Device.SourceMeter.Keithley;

namespace MPI.Tester.Device.Pulser
{
    public class Keithley2520 : ISourceMeter
    {
        private object _lockObj;

        private static int REGISTER_ENABLE_OPERATION = 128;

        private const double MAX_PULSE_DELAY = 500e-3d; // unit: s, max = 500 ms
        private const double MIN_PULSE_DELAY = 20e-6d;  // unit: s, min = 20 us

        private const double MAX_PULSE_WIDTH = 5e-3d; // unit: s, max = 5 ms
        private const double MIN_PULSE_WIDTH = 500e-9d;  // unit: s, min = 500 ns

        private const double MAX_DUTY = 0.04d;

        private int _bufferSize = 280;

        private string _hwVersion;
        private string _swVersion;
        private string _serialNum;

        private ElectSettingData[] _elcSetting;
        private ElecDevSetting _devSetting;
        private EDevErrorNumber _errorNum;

        private SourceMeterSpec _spec;
        private IConnect _conn;

        private List<double[]> _acquireData;
        private List<double[]> _sweepResult1;
        private List<double[]> _sweepResult2;
        private List<double[]> _timeChain;
        private List<double[]> _applyData;

        public Keithley2520()
        {
            this._lockObj = new object();

            this._errorNum = EDevErrorNumber.Device_NO_Error;
            this._hwVersion = "HW NONE";
            this._swVersion = "SW NONE";
            this._serialNum = "SN NONE";

            this._spec = new SourceMeterSpec();

            this._acquireData = new List<double[]>();
            this._sweepResult1 = new List<double[]>();
            this._sweepResult2 = new List<double[]>();
            this._timeChain = new List<double[]>();
            this._applyData = new List<double[]>();
        }

        public Keithley2520(ElecDevSetting config)
            : this()
        {
            this._devSetting = config;
        }

        #region >>> Public Property <<<

        public string SerialNumber
        {
            get { return this._serialNum; }
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

        public ElectSettingData[] ElecSetting
        {
            get { return this._elcSetting; }
        }

        public SourceMeterSpec Spec
        {
            get { return this._spec; }
        }

        #endregion

        #region >>> Private Method <<<

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

            uint risingPoints = (uint)(Math.Round(((end - start) / step), 6, MidpointRounding.AwayFromZero)) + 1;

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

        private void DeviceConfigPIV(double start, double end, uint point,double pulseWidth, double pulseDelay, double pdBias1, double pdRange1, double pdBias2, double pdRange2)
        {
            this._conn.SendCommand("*RST");
            this._conn.SendCommand("*CLS");
  
            this._conn.SendCommand("*SRE 128");
            this._conn.SendCommand("STAT:OPER:ENAB 1024");  // idle

            this._conn.SendCommand("SOUR:FUNC PULSE");
            this._conn.SendCommand("SOUR:PULS:DELAY " + pulseDelay.ToString());
            this._conn.SendCommand("SOUR:PULS:WIDTH " + pulseWidth.ToString());

            this._conn.SendCommand("SOUR:CURR:RANG " + end.ToString());

            this._conn.SendCommand("SOUR:CURR:START " + start.ToString());
            this._conn.SendCommand("SOUR:CURR:STOP " + end.ToString());
            this._conn.SendCommand("SOUR:SWE:SPAC LIN");
            this._conn.SendCommand("SOUR:SWE:POIN " + point.ToString());
            this._conn.SendCommand("SOUR:CURR:MODE SWE");

            this._conn.SendCommand("SENS:VOLT:RANG 5");
            this._conn.SendCommand("SOUR:VOLT:PROT 4.5");

            this._conn.SendCommand(":SENS2:CURR:POL POS");
            this._conn.SendCommand(":SENS2:CURR:RANG " + pdRange1.ToString());
            this._conn.SendCommand(":SOUR2:VOLT " + pdBias1.ToString());

            this._conn.SendCommand(":SENS3:CURR:POL NEG");   // POS
            this._conn.SendCommand(":SENS3:CURR:RANG " + pdRange2.ToString());
            this._conn.SendCommand(":SOUR3:VOLT " + pdBias2.ToString());

            this._conn.SendCommand("TRIG:OUTPUT TRIG");
            //this._conn.SendCommand("TRIG:OLINE 2");
            //this._conn.SendCommand("DISP:ENAB OFF");

            this._conn.SendCommand("OUTPUT ON");
        }

        private void DeviceConfigSinglePulse(double pulseValue, double pulseWidth, double pulseDelay, double pdBias1, double pdRange1, double pdBias2, double pdRange2)
        {
            this._conn.SendCommand("*RST");
           // this._conn.SendCommand(":DISP:ENAB ON");

            this._conn.SendCommand("SOUR:CURR:MODE FIX");
            this._conn.SendCommand("SOUR:FUNC PULS");
            this._conn.SendCommand("SOUR:PULS:DELAY " + pulseDelay.ToString());
            this._conn.SendCommand("SOUR:PULS:WIDTH " + pulseWidth.ToString());
            this._conn.SendCommand("SOUR:CURR:RANG " + pulseValue.ToString());
            this._conn.SendCommand("SOUR:CURR " + pulseValue.ToString());
            this._conn.SendCommand("SENS:VOLT:RANG 5");
            this._conn.SendCommand("SOUR:VOLT:PROT 4.5");
            this._conn.SendCommand("FORM:ELEM VOLT,CURR2,CURR3");

            //----------------------------------------------------



            //----------------------------------------------------

            this._conn.SendCommand(":SENS2:CURR:POL POS");
            this._conn.SendCommand(":SENS2:CURR:RANG " + pdRange1.ToString());
            this._conn.SendCommand(":SOUR2:VOLT " + pdBias1.ToString());

            this._conn.SendCommand(":SENS3:CURR:POL NEG");   // POS
            this._conn.SendCommand(":SENS3:CURR:RANG " + pdRange2.ToString());
            this._conn.SendCommand(":SOUR3:VOLT " + pdBias2.ToString());

            this._conn.SendCommand("TRIG:COUN 1");
            this._conn.SendCommand("OUTPUT ON");
        }

        private void DeviceConfigContinuousPulse(double pulseValue, double pulseWidth, double pulseDelay)
        {
            this._conn.SendCommand("*RST");
            this._conn.SendCommand(":DISP:ENAB ON");

            this._conn.SendCommand("SOUR:CURR:MODE FIX");
            this._conn.SendCommand(":SOUR:CURR:Range 5");
            this._conn.SendCommand(":SOUR:CURR:LEV:IMM 0");
            this._conn.SendCommand("SOUR:PULS:DELAY " + pulseDelay.ToString());
            this._conn.SendCommand("SOUR:PULS:WIDTH " + pulseWidth.ToString());
            this._conn.SendCommand("SENS:VOLT:RANG 7");
            this._conn.SendCommand(":SENS2:CURR:POL NEG");
            this._conn.SendCommand(":SENS2:CURR:RANG 0.1");
            this._conn.SendCommand(":SOUR2:VOLT 0");
            
            this._conn.SendCommand("TRIG:COUN CONTINUOUS");
            this._conn.SendCommand("FORM:ELEM:TRACE CURR2");
            this._conn.SendCommand("OUTPUT ON");
        }

        private bool DeviceCheckErrorState(out string msg)
        {
            msg = "No Error";

            this._conn.SendCommand(":SYST:ERR?");

            this._conn.WaitAndGetData(out msg);

            string[] status = msg.Split(',');

            if (status[0] != "0")
            {
                msg = status[1];

                Console.WriteLine(string.Format("[OSLaser], Keithley 2520 Error: {0}", msg));

                return false;
            }

            return true;
        }

        private int DeviceRequestStatus()
        {
            int status = 0;

            string tempStrResult = string.Empty;

            this._conn.SendCommand("*STB?");

            if (this._conn.WaitAndGetData(out tempStrResult))
            {
                int.TryParse(tempStrResult, out status);
            }

            return status;
        }

        private bool DeviceRunProcess(uint index, ElectSettingData setting)
        {
            this._errorNum = EDevErrorNumber.Device_NO_Error;
            
            string msg = string.Empty;

            switch (setting.MsrtType)
            {
                case EMsrtType.FIMV:      // for LOPWL
                case EMsrtType.FIMVLOP:   // for LOP
                    {
                        //-------------------------------------------------------
                        // Continuous Pulse
                        //-------------------------------------------------------
                        if (setting.KeyName.Contains("IFWLA"))
                        {
                            double pulseValue = setting.ForceValue;

                            double ampVolt = 0.0d;

                            double pulseWitdth = setting.ForceTime;
                            double pulseDelay = setting.ForceDelayTime;

                            this.DeviceConfigContinuousPulse(pulseValue, pulseWitdth, pulseDelay);

                            if (!this.DeviceCheckErrorState(out msg))
                            {
                                this._conn.SendCommand("OUTPUT OFF");
                                return false;
                            }

                            this._conn.SendCommand("INIT");
                            this._conn.SendCommand("SYST:KEY 20");  // Key 20 = TRIG

                        }
                        else if (setting.KeyName.Contains("IFWLB"))
                        {
                            this._conn.SendCommand(":ABORT");

                            string rtnStr = string.Empty;

                            do
                            {
                                this._conn.SendCommand("*OPC?");
                                this._conn.WaitAndGetData(out rtnStr);
                            }
                            while (rtnStr != "1");


                            this._conn.SendCommand("OUTPUT OFF");
                        }
                        else
                        {
                            //Single Pluse
                            double pulseValue = setting.ForceValue;
                            double ampVolt = 0.0d;

                            double pulseWitdth = setting.ForceTime;
                            double pulseDelay = setting.ForceDelayTime;

                            double pdBias1 = 0.0d;
                            double pdRange1 = 0.0d;
                            double pdBias2 = 0.0d;
                            double pdRange2 = 0.0d;

                            pulseDelay = 0.0005d;

                            pdBias1 = setting.DetectorBiasValue;
                            pdRange1 = setting.DetectorMsrtRange;

                            pdBias2 = setting.DetectorBiasValue;
                            pdRange2 = setting.DetectorMsrtRange;

                            this.DeviceConfigSinglePulse(pulseValue, pulseWitdth, pulseDelay, pdBias1, pdRange1, pdBias2, pdRange2);

                            System.Threading.Thread.Sleep(30);

                            if (!this.DeviceCheckErrorState(out msg))
                            {
                                this._conn.SendCommand("OUTPUT OFF");
                                return false;
                            }


                            System.Threading.Thread.Sleep(10);
                           

                            //this.DeviceConfigSinglePulse(pulseWitdth, pulseDelay);

                            //if (!this.DeviceCheckErrorState(out msg))
                            //{
                            //    this._errorNum = EDevErrorNumber.SMU_OutputBlockedByInterlock;
                            //    this._k2400.TurnOff();
                            //    this._avTech.TurnOff();
                            //    this._conn.SendCommand("OUTPUT OFF");
                            //    return false;
                            //}

                            this._conn.SendCommand("INIT");
                            //this._conn.SendCommand("SYST:KEY 20");  // Key 20 = TRIG

                            this._conn.SendCommand("FETCH?");

                            string tempStrResult = "";

                            double volt = 0.0d;
                            double pdCurr1 = 0.0d;
                            double pdCurr2 = 0.0d;
                        

                            if (this._conn.WaitAndGetData(out tempStrResult))
                            {
                                string[] rtnString = tempStrResult.Split(',');
                                
                                double.TryParse(rtnString[0], out volt);
                                double.TryParse(rtnString[1], out pdCurr1);
                                double.TryParse(rtnString[2], out pdCurr2);
                               
                                
                            }

                            this._conn.SendCommand("OUTPUT OFF");

                         //   curr = (curr * 1000.0d) / 0.1;  // 1.631

                            this._acquireData[(int)index][0] = volt;   // 0.00007620594     0.00007451233
                            this._acquireData[(int)index][1] = pdCurr1;
                            this._acquireData[(int)index][3] = pdCurr2;
                            
                            
                        }

                        break;
                    }   
                case EMsrtType.PIV:
                    {
                        //-------------------------------------------------------
                        // Pulse Sweep
                        //-------------------------------------------------------
                        int status = 0;

                        double strartValue = 0.0d;
                        double endValue = 0.0d;
                        uint point = 0;

                        double pulseWitdth = 0.0d;
                        double pulseDelay = 0.0d;

                        string tempStrResult = "";

                        double pdBias1 = 0.0d;
                        double pdRange1 = 0.0d;
                        double pdBias2 = 0.0d;
                        double pdRange2 = 0.0d;

                        strartValue = setting.SweepStart;
                        endValue = setting.SweepStop;
                        point = setting.SweepRiseCount;

                        pulseWitdth = setting.ForceTime;
                        pulseDelay = setting.SweepTurnOffTime;

                        pdBias1 = setting.DetectorBiasValue;
                        pdRange1 = setting.DetectorMsrtRange;
                        pdBias2 = setting.DetectorBiasValue;
                        pdRange2 = setting.DetectorMsrtRange;
                        //strartValue = 2;
                        //endValue = 6;
                        //point = 5;
                        //pulseWitdth = 1e-6;
                        //pulseDelay = 1e-3;

                        this.DeviceConfigPIV(strartValue, endValue, point, pulseWitdth, pulseDelay, pdBias1, pdRange1, pdBias2, pdRange2);

                        if (!this.DeviceCheckErrorState(out msg))
                        {
                            this._conn.SendCommand("OUTPUT OFF");
                            return false;
                        }

                        System.Threading.Thread.Sleep(50);

                        this._conn.SendCommand("INIT");

                        status = 0;

                        do
                        {
                            status = this.DeviceRequestStatus();
                        }
                        while (status == 0);

                        this._conn.SendCommand("OUTPUT OFF");

                        this._conn.SendCommand("FETCH?");

                        double volt = 0.0d;
                        double pdCurr1 = 0.0d;
                        double pdCurr2 = 0.0d;

                       // if(this._conn.WaitAndGetData(out tempStrResult))
                        if ((this._conn as GPIBConnect).WaitAndGetData(3 * (int)point * 14 - 1, out tempStrResult))   // format +0.000000E-07, (14),  -1: the last data didn't have ","
                        {
                            string[] rtnString = tempStrResult.Split(',');

                            for (int i = 0; i < point; i++)
                            {
                                double.TryParse(rtnString[i * 3], out volt);
                                double.TryParse(rtnString[i * 3 + 1], out pdCurr1);
                                double.TryParse(rtnString[i * 3 + 2], out pdCurr2);

                               // pow = pow * -1000.0d;
                                volt = volt <= 10.0d ? volt : 10.0d;
                               // curr = (curr * 1000.0d) / 0.1;  // 1.631
                                if (pdCurr2 > 9999999999.0d)
                                {
                                    pdCurr2 = pdRange2;  // for overflow case
                                }

                                this._acquireData[(int)index][i] = volt;
                                this._sweepResult1[(int)index][i] = pdCurr1;
                                this._sweepResult2[(int)index][i] = pdCurr2;

                            }
                        }

                        //this._conn.SendCommand("*CLS");

                        this._conn.WaitAndGetData(out msg);

                        break;
                    }
                //----------------------------------------------------------------------------------
                default:
                    {
                        return false;
                    }
            }
            
            return true;
        }

        private bool RunTestItem(uint deviceID, uint index)
        {
            double estimatedSequnceTime = 0.0d;

            this._errorNum = EDevErrorNumber.Device_NO_Error;

            ElectSettingData setting = this._elcSetting[index];

            string cmd = string.Empty;
            string msg = string.Empty;

            switch (setting.MsrtType)
            {
                case EMsrtType.FIMV:      // for LOPWL
                case EMsrtType.FIMVLOP:   // for LOP
                    {
                        //-------------------------------------------------------
                        // Continuous Pulse
                        //-------------------------------------------------------
                        if (setting.KeyName.Contains("IFWLA"))
                        {
                            double pulseValue = setting.ForceValue;

                            double pulseWitdth = setting.ForceTime;
                            double pulseDelay = setting.ForceDelayTime;

                            double msrtRange = setting.MsrtProtection;

                            pulseDelay = pulseDelay >= MIN_PULSE_DELAY ? pulseDelay : MIN_PULSE_DELAY;

                            pulseDelay = 0.001;

                            cmd = string.Empty;

                            cmd += SCPI.SrcFuncMode(CPD.SHAPE_PULSE);
                            cmd += SCPI.SrcCurrMode(CPD.MODE_FIX);
                            cmd += SCPI.MsrtVoltClamp(msrtRange);
                            cmd += SCPI.MsrtVoltRange(msrtRange);
                            cmd += SCPI.SrcCurrRange(pulseValue);
                            cmd += SCPI.SrcPulseLevel(pulseValue);

                            cmd += SCPI.SrcPulseDelay(pulseDelay);
                            cmd += SCPI.SrcPulseWidth(pulseWitdth);

                            cmd += SCPI.SrcTrigCount(9999);

                            cmd += SCPI.SrcOutput(true);
                            cmd += SCPI.TrigInit();
                            //cmd += SCPI.SrcBeginsContinuousPulsing();
 
                            this._conn.SendCommand(cmd);

                        }
                        else if (setting.KeyName.Contains("IFWLB"))
                        {
                            cmd = string.Empty;

                            cmd += SCPI.SrcStopContinuousPulsing();
                           // cmd += SCPI.SrcOutput(false);

                            this._conn.SendCommand(cmd);
                        }
                        else
                        {
                            //Single Pluse
                            double pulseValue = setting.ForceValue;
                         
                            double pulseWitdth = setting.ForceTime;
                            double pulseDelay = setting.ForceDelayTime;

                            double msrtRange = setting.MsrtProtection;

                            double pdBias1 = 0.0d;
                            double pdRange1 = 0.0d;
                            double pdBias2 = 0.0d;
                            double pdRange2 = 0.0d;

                            uint pulseCnt = setting.PulseCount;

                            if (setting.SourceFunction != ESourceFunc.QCW)
                            {
                                pulseCnt = 1;
                            }

                            pdBias1 = setting.DetectorBiasValue;
                            pdRange1 = setting.DetectorMsrtRange;

                            pdBias2 = setting.DetectorBiasValue;
                            pdRange2 = setting.DetectorMsrtRange;

                            if (pulseValue > 1.0d)
                            {
                                pulseDelay = Math.Round(((pulseWitdth / 0.04d) - pulseWitdth), 7, MidpointRounding.AwayFromZero);
                            }
                            
                            pulseDelay = pulseDelay >= MIN_PULSE_DELAY ? pulseDelay : MIN_PULSE_DELAY;

                            //pulseDelay = MIN_PULSE_DELAY;



                            cmd = string.Empty;

                            if (pulseWitdth < 6e-6d)
                            {
                                cmd += SCPI.SrcTransitionCtrl(false);
                            }
                            else
                            {
                                cmd += SCPI.SrcTransitionCtrl(true);
                            }

                            cmd += SCPI.SrcFuncMode(CPD.SHAPE_PULSE);
                            cmd += SCPI.SrcCurrMode(CPD.MODE_FIX);
                            cmd += SCPI.MsrtVoltClamp(msrtRange);
                            cmd += SCPI.MsrtVoltRange(msrtRange);
                            cmd += SCPI.SrcCurrRange(pulseValue);
                            cmd += SCPI.SrcPulseLevel(pulseValue);

                            cmd += SCPI.SrcPulseDelay(0.1d);
                            cmd += SCPI.SrcPulseWidth(10e-6d);
                            cmd += SCPI.SrcPulseDelay(pulseDelay);
                            cmd += SCPI.SrcPulseWidth(pulseWitdth);

                        
                  
                           // cmd += SCPI.SrcBiasLevel(0.01d);

                            cmd += SCPI.SrcTrigCount(pulseCnt);
                          //  cmd += SCPI.MsrtCurrPolar_DET1(CPD.POLAR_ANODE);
                            cmd += SCPI.MsrtCurrPolar_DET1(CPD.POLAR_CATHODE);

                            cmd += SCPI.MsrtCurrRange_DET1(pdRange1);
                            cmd += SCPI.SrcBiasVolt_DET1(pdBias1);

                            cmd += SCPI.MsrtCurrPolar_DET2(CPD.POLAR_CATHODE);
                            cmd += SCPI.MsrtCurrRange_DET2(pdRange2);
                            cmd += SCPI.SrcBiasVolt_DET2(pdBias2);

                            this._conn.SendCommand(cmd);
                            cmd = string.Empty;


                            //cmd += "TRIG:OUTPUT TRIG\n";
                            //cmd += "TRIG:OLINE 2\n";
                           // cmd += "TRIG:SOUR IMM\n";
                            cmd += SCPI.SrcOutput(true);

                                
                            cmd += SCPI.DioOutput(1);

                            cmd += SCPI.TrigInit();
  
                            cmd += SCPI.DioOutput(0);
                           // cmd += SCPI.SrcBiasLevel(0.0d);
                            cmd += SCPI.QUERY_FETCH;

                            this._conn.SendCommand(cmd);
                           
                            string tempStrResult = "";

                            double volt = 0.0d;
                            double pdCurr1 = 0.0d;
                            double pdCurr2 = 0.0d;

                            if (this._conn.WaitAndGetData(out tempStrResult))
                            {
                                string[] rtnString = tempStrResult.Split(',');

                                double.TryParse(rtnString[0], out volt);
                                double.TryParse(rtnString[1], out pdCurr1);
                                double.TryParse(rtnString[2], out pdCurr2);
                            }

                            this._acquireData[(int)index][0] = volt;   // 0.00007620594     0.00007451233
                            this._acquireData[(int)index][1] = pdCurr1;
                            this._acquireData[(int)index][3] = pdCurr2;
                        }

                        break;
                    }
                case EMsrtType.PIV:
                    {
                        //-------------------------------------------------------
                        // Pulse Sweep
                        //-------------------------------------------------------
               
                        double startValue = 0.0d;
                        double endValue = 0.0d;
                        uint point = 0;
                        double stepValue = 0.0d;

                        double[] lstCurrList;

                        double pulseWitdth = 0.0d;
                        double pulseDelay = 0.0d;

                        double msrtRange = 0.0d;

                        double pdBias1 = 0.0d;
                        double pdRange1 = 0.0d;
                        double pdBias2 = 0.0d;
                        double pdRange2 = 0.0d;

                        uint count = 0;

                        startValue = setting.SweepStart;
                        endValue = setting.SweepStop;
                        point = setting.SweepRiseCount;
                        stepValue = setting.SweepStep;

                        pulseWitdth = setting.ForceTime;
                        pulseDelay = setting.SweepTurnOffTime;

                        msrtRange = setting.MsrtProtection;

                        pdBias1 = setting.DetectorBiasValue;
                        pdRange1 = setting.DetectorMsrtRange;
                        pdBias2 = setting.DetectorBiasValue;
                        pdRange2 = setting.DetectorMsrtRange;

                        count = setting.PulseCount;

                        if (setting.SourceFunction == ESourceFunc.CW)
                        {
                            pulseDelay = 20e-6;
                        }

                        if (setting.SourceFunction != ESourceFunc.QCW)
                        {
                            count = 1;
                        }

                        cmd = string.Empty;

                        if (pulseWitdth < 6e-6d)
                        {
                            cmd += SCPI.SrcTransitionCtrl(false);
                        }
                        else
                        {
                            cmd += SCPI.SrcTransitionCtrl(true);
                        }

                        switch (setting.SourceFunction)
                        {
                            case ESourceFunc.CW:
                                {
                                    cmd += SCPI.SrcFuncMode(CPD.SHAPE_DC);
                        			cmd += SCPI.SrcCurrMode(CPD.MODE_SWEEP);
                        			cmd += SCPI.SrcSweepSpacingType(CPD.SWEEP_TYPE_LINEAR);

                                    //cmd += "SOUR:PULS:DELAY 0.001\n";
                                    //cmd += "SOUR:PULS:WIDTH MIN\n";
                                    cmd += SCPI.SrcPulseDelay(pulseDelay);
                                    cmd += SCPI.SrcPulseWidth(pulseWitdth);

                                    cmd += SCPI.SrcSweepStartLevel(startValue);
                                    cmd += SCPI.SrcSweepStopLevel(endValue);
                                    cmd += SCPI.SrcSweepPoints(point);
                                    break;
                                }
                            case ESourceFunc.PULSE:
                                {
                                    cmd += SCPI.SrcFuncMode(CPD.SHAPE_PULSE);
                                    cmd += SCPI.SrcCurrMode(CPD.MODE_SWEEP);
                                    cmd += SCPI.SrcSweepSpacingType(CPD.SWEEP_TYPE_LINEAR);
                       
                       				cmd += SCPI.SrcPulseDelay(pulseDelay);
                        			cmd += SCPI.SrcPulseWidth(pulseWitdth);

                                    cmd += SCPI.SrcSweepStartLevel(startValue);
                        			cmd += SCPI.SrcSweepStopLevel(endValue);
                        			cmd += SCPI.SrcSweepPoints(point);

                                    break;
                                }
                            case ESourceFunc.QCW:
                                {
                                    cmd += SCPI.SrcFuncMode(CPD.SHAPE_PULSE);
                                    cmd += SCPI.SrcCurrMode(CPD.MODE_LIST);
                                    lstCurrList = this.MakeLinearSweepList(startValue, stepValue, endValue, count);
                                    if (lstCurrList.Length <= 100)
                                    {
                                        cmd += SCPI.SrcCurrList(lstCurrList);
                                        cmd += SCPI.SrcDelayList(pulseDelay, point, count);
                                        cmd += SCPI.SrcWidthList(pulseWitdth, point, count);
                                    }
                                    else
                                    {
                                        this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                                        return false;                                    
                                    }
                                    break;
                                }
                        }

                        cmd += SCPI.MsrtVoltClamp(msrtRange);
                        cmd += SCPI.MsrtVoltRange(msrtRange);

                        cmd += SCPI.SrcCurrRange(endValue);

                        cmd += SCPI.SrcTrigCount(1);

                        cmd += SCPI.MsrtCurrPolar_DET1(CPD.POLAR_CATHODE);
                        cmd += SCPI.MsrtCurrRange_DET1(pdRange1);
                        cmd += SCPI.SrcBiasVolt_DET1(pdBias1);

                        cmd += SCPI.MsrtCurrPolar_DET2(CPD.POLAR_CATHODE);
                        cmd += SCPI.MsrtCurrRange_DET2(pdRange2);
                        cmd += SCPI.SrcBiasVolt_DET2(pdBias2);

                        cmd += SCPI.SrcOutput(true);
                  
                        cmd += SCPI.TrigInit();
                      //  cmd += SCPI.SrcOutput(false);
                        cmd += SCPI.QUERY_FETCH;
                        this._conn.SendCommand(cmd);
                       
                        //---------------------------------------------------------------------------------------
                        // Reading Data
                        string tempStrResult = "";

                        double volt = 0.0d;
                        double pdCurr1 = 0.0d;
                        double pdCurr2 = 0.0d;

                        // if(this._conn.WaitAndGetData(out tempStrResult))
                        if ((this._conn as GPIBConnect).WaitAndGetData(3 * (int)point * (int)count * 14 - 1, out tempStrResult))   // format +0.000000E-07, (14),  -1: the last data didn't have ","
                        //if (GetDataFromDevice(point, count, out tempStrResult))  // buffer size v.s. FETCH time
                        {
                            string[] rtnString = tempStrResult.Split(',');

                            for (int i = 0; i < point; i++)
                            {
                                double.TryParse(rtnString[i * 3], out volt);
                                double.TryParse(rtnString[i * 3 + 1], out pdCurr1);
                                double.TryParse(rtnString[i * 3 + 2], out pdCurr2);

                                // pow = pow * -1000.0d;
                                volt = volt <= 10.0d ? volt : 10.0d;
                               
                                // curr = (curr * 1000.0d) / 0.1;  // 1.631
                                if (pdCurr2 > 9999999999.0d)
                                {
                                    pdCurr2 = pdRange2;  // for overflow case
                                }

                                this._acquireData[(int)index][i] = volt;
                                this._sweepResult1[(int)index][i] = Math.Abs(pdCurr1);
                                this._sweepResult2[(int)index][i] = pdCurr2;

                            }
                        }

                        this._conn.WaitAndGetData(out msg);
                        
                        //this._conn.SendCommand("*CLS");

                        break;
                    }
                //----------------------------------------------------------------------------------
                default:
                    {
                        return false;
                    }
            }

            return true;
        }

        private bool GetDataFromDevice(uint point, uint count, out string OutResult)
        {
            OutResult = string.Empty;

            if (this._conn == null)
            {
                return false;            
            }
            
            int ReadingCount = 3 * (int)point * (int)count * 14 - 1;

            string Temp = string.Empty;

            int Remainder;

            int loop = Math.DivRem(ReadingCount, this._bufferSize, out Remainder);

            for (int i = 0; i <= loop; i++)
            {
                if (i == loop - 1)
                {
                    (this._conn as GPIBConnect).WaitAndGetData(Remainder, out Temp);

                    OutResult += Temp;
                }
                else
                {
                    (this._conn as GPIBConnect).WaitAndGetData(this._bufferSize, out Temp);

                    OutResult += Temp;
                }
            }

            return true;        
        }

        #endregion

        #region >>> Public Method <<<

        public bool Init(int deviceNum, string sourceMeterSN)
        {
            string info = "";
            string deviceModel = "";

            //-------------------------------------------------------------------
            // K2520 Init
            GPIBSettingData gpibSetting = new GPIBSettingData();
            gpibSetting.PrimaryAddress = 25;
            gpibSetting.IOTimeOut = 13;

            this._conn = new GPIBConnect(gpibSetting);

            if (this._conn.Open(out info) && info != "")
            {
                string[] deviceInfo = info.Split(',');
                deviceModel = deviceInfo[1];
                this._serialNum = deviceInfo[2];
                this._swVersion = "1.0";
                this._hwVersion = "Keithley " + deviceModel.Replace("MODEL ", "");
            }
            else
            {
                Console.WriteLine("[Device], Keithley 2520 Init Fail");

                this._conn = null;
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;

                return false;
            }

            this._conn.SendCommand(SCPI.RESET);

            this._conn.SendCommand(SCPI.FORM_ACSII);

            this._conn.SendCommand(SCPI.FORM_VOLT_CURR_CURR);

            this._conn.SendCommand(SCPI.SysDisplay(false));

            this._conn.SendCommand(SCPI.DioOutput(0));
     
            return true;
        }

        public void Reset()
        {
            
        }

        public bool SetConfigToMeter(ElecDevSetting devSetting)
        {
            return true;
        }

        public bool SetParamToMeter(ElectSettingData[] paramSetting)
        {
            int count = 0;

            this._errorNum = EDevErrorNumber.Device_NO_Error;

            if (paramSetting == null || paramSetting.Length == 0)
            {
                return true;
            }

            this._elcSetting = paramSetting.Clone() as ElectSettingData[];

            this._acquireData.Clear();  // Msrt Volt
            this._applyData.Clear();    // Apply Current
            this._sweepResult1.Clear();  // Pow1
            this._sweepResult2.Clear();  // Pow2

            foreach (var item in this._elcSetting)
            {
                this._acquireData.Add(new double[1]);
                this._applyData.Add(new double[1]);
                this._sweepResult1.Add(new double[1]);
                this._sweepResult2.Add(new double[1]);
                
                switch (item.MsrtType)
                {
                    case EMsrtType.FIMV:
                    case EMsrtType.FIMVLOP:
                        {
                            item.ForceTime = Math.Round((item.ForceTime / 1000.0d), 7, MidpointRounding.AwayFromZero);      // Pulse Width.
                            item.ForceDelayTime = Math.Round((item.ForceDelayTime / 1000.0d), 7, MidpointRounding.AwayFromZero);      // Pulse Width
                            this._acquireData[count] = new double[5];
                            break;
                        }
                    case EMsrtType.PIV:
                        {
                            if (item.SweepStep != 0.0d)
                            {
                                item.SweepRiseCount = (uint)(Math.Ceiling((item.SweepStop - item.SweepStart) / item.SweepStep)) + 1;
                            }
                            else
                            {
                                item.SweepRiseCount = 1; // StartValue and EndValue
                            }
                            
                            item.ForceTime = Math.Round((item.ForceTime / 1000.0d), 7, MidpointRounding.AwayFromZero);      // Pulse Width
                            //item.SweepTurnOffTime = Math.Round((item.SweepTurnOffTime / 1000.0d), 7, MidpointRounding.AwayFromZero);  // Pulse Delay

                           

                           double offtime = Math.Round(((item.ForceTime / item.Duty) - item.ForceTime), 7, MidpointRounding.AwayFromZero);

                           item.SweepTurnOffTime = offtime;

                            this._applyData[count] = new double[item.SweepRiseCount];
                            this._acquireData[count] = new double[item.SweepRiseCount];
                            this._sweepResult1[count] = new double[item.SweepRiseCount];
                            this._sweepResult2[count] = new double[item.SweepRiseCount];

                            //-------------------------------------------------------------------------------------------------
                            // Default Apply Data
                            double start = item.SweepStart;
                            double step = item.SweepStep;
                            double end = item.SweepStop;
                            uint pulseCnt = item.PulseCount;

                            double[] lstRisingStep = this.MakeLinearSweepList(start, step, end); // Rising step, 用來計算的Apply Data

                            this._elcSetting[count].SweepCustomValue = lstRisingStep;

                            //for (int i = 0; i < this._applyData[count].Length; i++)
                            //{
                            //    this._applyData[count][i] = item.SweepStart + i * item.SweepStep;

                            //    if (this._applyData[count][i] > item.SweepStop)
                            //    {
                            //        this._applyData[count][i] = item.SweepStop;
                            //    }
                            //}
                                
                            //-------------------------------------------------------------------------------------------------
                            // if (item.ForceTime > MAX_DELAY_TIME || item.ForceTime < MIN_DELAY_TIME)
                            if (item.ForceTime > MAX_PULSE_WIDTH || item.ForceTime < MIN_PULSE_WIDTH)
                            {
                                this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                                return false;
                            }

                            if (item.SweepTurnOffTime > MAX_PULSE_DELAY || item.SweepTurnOffTime < MIN_PULSE_DELAY)
                            {
                                this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                                return false;
                            }

                            break;
                        }
                    default:
                        {
                            this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                            return false;
                        }
                }

                count++;
            }

            return true;
        }

        public bool MeterOutput(uint[] activateChannels, uint index)
        {
            if (this._elcSetting == null || this._elcSetting.Length == 0 || index > this._elcSetting.Length - 1)
            {
                return false;
            }

            //return this.DeviceRunProcess(index, this._elcSetting[index]);

            return this.RunTestItem(0, index);
        }

        public bool MeterOutput(uint[] activateChannels, uint index, double forceValue)
        {
            if (this._elcSetting == null || this._elcSetting.Length == 0 || index > this._elcSetting.Length - 1)
            {
                return false;
            }

            ElectSettingData oneSetting = this._elcSetting[index].Clone() as ElectSettingData;

            oneSetting.ForceValue = forceValue;

            return this.DeviceRunProcess(index, oneSetting);
        }

        public double[] GetDataFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            return this._acquireData[(int)itemIndex];
        }

        public double[] GetApplyDataFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            return this._applyData[(int)itemIndex];
        }

        public double[] GetSweepPointFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            return this._elcSetting[itemIndex].SweepCustomValue;
        }

        public double[] GetSweepResultFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            return this._sweepResult1[(int)itemIndex]; // Channel = 0, SweepReuslt = Pow1
            //if (channel == 1)
            //{
            //    return this._sweepResult2[(int)itemIndex];  // Channel = 1, SweepReuslt = Pow2
            //}
            //else
            //{
            //    return this._sweepResult1[(int)itemIndex]; // Channel = 0, SweepReuslt = Pow1
            //}
        }

        public double[] GetTimeChainFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            return this._timeChain[(int)itemIndex];
        }

        public void TurnOff(double delay, bool isOpenRelay)
        {
            if (isOpenRelay)
            {
                this._conn.SendCommand(":ABORT");

                string rtnStr = string.Empty;

                do
                {
                    this._conn.SendCommand("*OPC?");
                    this._conn.WaitAndGetData(out rtnStr);
                }
                while (rtnStr != "1");

                this._conn.SendCommand("OUTPUT OFF");
            }
        }

        public void TurnOff()
        {
            //this._conn.SendCommand(":ABORT");
            //this._conn.SendCommand("OUTPUT OFF");

            //this._k2400.TurnOff();
            //this._avTech.TurnOff();

            this._conn.SendCommand(SCPI.SrcOutput(false));
        }

        public void Close()
        {
            this._conn.SendCommand(SCPI.SYS_OUT_OF_REMOTE);
            this._conn.Close();
        }

        public void Output(uint point, bool active)
        {
            
        }

        public byte Input(uint point)
        {
            return 0x00;
        }

        public byte InputB(uint point)
        {
            return 0x00;
        }

        public double GetPDDarkSample(int count)
        {
            return 0.0d;
        }

        public void StopOutput()
        {
            string cmd = string.Empty;

            cmd += SCPI.SrcStopContinuousPulsing();
           // cmd += SCPI.SrcOutput(false);

            this._conn.SendCommand(cmd); 
        }

        public bool CheckInterLock()
        {
            string msg = string.Empty;

            // Check Interlock (door open)
            this._conn.SendCommand(SCPI.STAT_INTERLOCK);

            this._conn.WaitAndGetData(out msg);

            if (msg != CPD.STAT_INTERLOCKED)  // 0 -> ok; 1 -> door open
            {
                this._errorNum = EDevErrorNumber.MeterOutput_Interlock_Err;
                return false;
            }

            return true;
        }

        #endregion
    }
}
