using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.Keysight
{
    public static class CPD
    {
        public static string PIN_SPT_TRIG_OUT = "EXT1";
        public static string PIN_SMU_TRIG_OUT = "EXT2";
        public static string PIN_SMU_SYNC = "EXT3";

        public static string PIN_CAMERA_NFP_TRIG_OUT = "EXT6";
        public static string PIN_CAMERA_FFP_TRIG_OUT = "EXT7";

        public static string FUNC_VOLT = "VOLT";
        public static string FUNC_CURR = "CURR";

        public static string MODE_SWEEP = "SWE";
        public static string MODE_FIX = "FIX";
        public static string MODE_LIST = "LIST";

        public static string SHAPE_DC = "DC";
        public static string SHAPE_PULSE = "PULS";

        public static string TRIG_INTERNAL = "AINT";
        public static string TRIG_TIMER = "TIM";
      
        public static string TRIG_BUS = "BUS";

        public static string OUTP_NORMAL = "NORM";
        public static string OUTP_ZERO = "ZERO";
        public static string OUTP_HIZ = "HIZ";
    }
    
    
    public class SCPI
    {
        #region >>> Public Static Method - Common Commend <<<

        public static string RESET = "*RST\n";
        public static string CLEAR_STATUS = "*CLS\n";
        public static string ABORT = ":ABOR\n";
        public static string TRIGGER = "*TRG\n";


        public static string QUERY_DEV_INFO = "*IDN?\n";
        public static string QUERY_SYS_ERR = ":SYST:ERR?\n";
        public static string QUERY_STAT_OPER = ":STAT:OPER:COND?";
        public static string QUERY_COMPLETE = "*OPC?\n";

        public static string QUERY_DIO_DATA = ":SOUR:DIG:DATA?\n";

        public static string QUERY_SYS_INTERLOCK = ":SYST:INT:TRIP?\n";  // Returns if the interlock circuit is [0] close or [1] open.

        public static string FLAG_COMPLETE = "1";

        public static string FORM_CURR_VOLT_CH1 = ":FORM:ELEM:SENS1 CURR,VOLT\n";  //"FORM:ELEM:SENS VOLT,CURR,TIME,STAT,SOUR"
        public static string FORM_CURR_VOLT_CH2 = ":FORM:ELEM:SENS2 CURR,VOLT\n";  //"FORM:ELEM:SENS VOLT,CURR,TIME,STAT,SOUR"

        public static ushort BITS_STAT_READY = 0x0492;
        public static ushort BITS_03_MASK_WAIT_TRANS_ARM = 0x0008;

        #endregion

        #region >>> Private Properties Status <<<
        
        // Channel-1
        public bool IS_TRIG_CH1 = false;
        private string sSrcFuncIV1 = string.Empty;
        private string sSrcMode1 = string.Empty;

        private string sSrcShap1 = string.Empty;
        private double sSrcRange1 = 999.0d;
        private double sSrcPulseLvl1 = 999.0d;
        private double sSrcBiasLvl1 = 999.0d;

        private double sSrcSweepStart = 999.0d;
        private double sSrcSweepStop = 999.0d;
        private uint sSrcSweepPoints = 9999;

        private double sArmDelay1 = 999.0d;
        private double sTrigPulseDelay1 = 999.0d;
        private double sTrigPulseWidth1 = 999.0d; //s
        private double sTrigPeriod1 = 999.0d;
        private double sTrigTranDelay1 = 999.0d;
        private double sTrigAcqDelay1 = 999.0d;

        //private bool sIsArmHwTriggerOutput1 = true;
        private string sArmSign1 = string.Empty;

        private string sTrigTranSign1 = string.Empty;
        private string sTrigAcqSign1 = string.Empty;

        private string sMsrtMode1 = string.Empty;
        private double sMsrtRange1 = 999.0d;
        private double sMsrtClamp1 = 999.0d;
        private double sNplc1 = 0.0d;
        private double sMsrtApertureTime1 = 999.0d;

        private uint sTrigCount1 = 0;

        private bool sOutput1 = false;

        private bool sIsArmExtTrig = false;
        private string sArmExtPinNum = string.Empty;

        // Channel-2
        public bool IS_TRIG_CH2 = false;
        private string sSrcFuncIV2 = string.Empty;
        private string sSrcMode2 = string.Empty;

        private string sSrcShap2 = string.Empty;
        private double sSrcRange2 = 999.0d;
        private double sSrcPulseLvl2 = 999.0d;
        private double sSrcBiasLvl2 = 999.0d;

        private double sArmDelay2 = 999.0d;
        private double sTrigPulseDelay2 = 999.0d;
        private double sTrigPulseWidth2 = 999.0d; //s
        private double sTrigPeriod2 = 0.0d;
        private double sTrigTranDelay2 = 999.0d;
        private double sTrigAcqDelay2 = 999.0d;

        private string sArmSign2 = string.Empty;

        private string sTrigTranSign2 = string.Empty;
        private string sTrigAcqSign2 = string.Empty;

        private string sMsrtMode2 = string.Empty;
        private double sMsrtRange2 = 999.0d;
        private double sMsrtClamp2 = 999.0d;
        private double sNplc2 = 999.0d;
        private double sMsrtApertureTime2 = 999.0d;

        private uint sTrigCount2 = 0;

        private bool sOutput2 = false;

        #endregion

        #region >>> Public Method - Source Commend <<<

        /// <summary>
        ///  isOn = true [Output ON] | false [Output OFF];
        /// </summary>
        public string SrcOutput_CH1(bool isOn)
        {
            if (isOn == sOutput1)
            {
                return string.Empty;
            }

            sOutput1 = isOn;

            if (sOutput1)
            {
                return ":OUTP1 1\n";
            }
            else
            {
                return ":OUTP1 0\n";
            }
        }

        public string SrcOutput_CH2(bool isOn)
        {
            if (isOn == sOutput2)
            {
                return string.Empty;
            }

            sOutput2 = isOn;

            if (sOutput2)
            {
                return ":OUTP2 1\n";
            }
            else
            {
                return ":OUTP2 0\n";
            }
        }

        /// <summary>
        /// Set the filter ON to obtain clean source output without spikes and overshooting.
        /// Note, however, that using a filter may increase the SMU settling time.
        /// </summary>
        public string SrcOutputFilter_CH1(bool isOn)
        {
            string cmd = string.Empty;

            if (isOn)
            {
                cmd += ":OUTP1:FILT ON\n";
               // cmd += ":OUTP1:FILT:AUTO ON\n";
            }
            else
            {
                cmd += ":OUTP1:FILT OFF\n";
               // cmd += ":OUTP1:FILT:AUTO OFF\n";
              //  cmd += string.Format(":OUTP1:FILT:FREQ {0}\n", freq);
            }

            return cmd;
        }

        public string SrcOutputFilter_CH2(bool isOn)
        {
            string cmd = string.Empty;

            if (isOn)
            {
                cmd += ":OUTP2:FILT ON\n";
              //  cmd += ":OUTP2:FILT:AUTO ON\n";
            }
            else
            {
                cmd += ":OUTP2:FILT OFF\n";
               // cmd += ":OUTP2:FILT:AUTO OFF\n";
            }

            return cmd;
        }

        /// <summary>
        /// Selects the state of the low terminal. Before executing this command, the source
        /// output must be disabled by the :OUTPut[:STATe] command. Or else, an error
        /// occurs.
        /// FLOat|GROund (default)
        /// </summary>
        public string SrcOutputLowState_CH1(bool isGrounding)
        {
            if (isGrounding)
            {
                return ":OUTP1:LOW GRO\n";
            }
            else
            {
                return ":OUTP1:LOW FLO\n";
            }
        }

        public string SrcOutputLowState_CH2(bool isGrounding)
        {
            if (isGrounding)
            {
                return ":OUTP2:LOW GRO\n";
            }
            else
            {
                return ":OUTP2:LOW FLO\n";
            }
        }

        /// <summary>
        /// Enables or disables the automatic output when the :INITiate or :READ command is sent.
        /// </summary>
        public string SrcOutputAutoON_CH1(bool isOn)
        {
            if (isOn)
            {
                return ":OUTP1:ON:AUTO ON\n";
            }
            else
            {
                return ":OUTP1:ON:AUTO OFF\n";
            }
        }

        public string SrcOutputAutoON_CH2(bool isOn)
        {
            if (isOn)
            {
                return ":OUTP2:ON:AUTO ON\n";
            }
            else
            {
                return ":OUTP2:ON:AUTO OFF\n";
            }
        }

        /// <summary>
        /// mode = ZERO | HIZ | NORMal (default)
        /// </summary>
        public string SrcOutputOffMode_CH1(string mode)
        {
            return string.Format(":OUTP1:OFF:MODE {0}\n", mode);
        }

        /// <summary>
        /// mode = ZERO | HIZ | NORMal (default)
        /// </summary>
        public string SrcOutputOffMode_CH2(string mode)
        {
            return string.Format(":OUTP2:OFF:MODE {0}\n", mode);
        }

        /// <summary>
        /// [ON] wait time = gain * initial wait time + offset; [OFF] wait time = offset
        /// </summary>
        public string SrcWaitTime_CH1(bool isOn)
        {
            if (isOn)
            {
                return ":SOUR1:WAIT 1\n";
            }
            else
            {
                return ":SOUR1:WAIT 0\n";
            }
        }

        public string SrcWaitTime_CH2(bool isOn)
        {
            if (isOn)
            {
                return ":SOUR2:WAIT 1\n";
            }
            else
            {
                return ":SOUR2:WAIT 0\n";
            }
        }

        /// <summary>
        ///  func = CURR | VOLT;
        ///  mode = SWE | LIST | FIX
        ///  shape = PULS | DC 
        /// </summary>
        public string SrcConfig_CH1(string func, string mode, string shape)
        {
            if (func == sSrcFuncIV1 && mode == sSrcMode1 && shape == sSrcShap1)
            {
                return string.Empty;
            }

            sSrcFuncIV1 = func;
            sSrcMode1 = mode;
            sSrcShap1 = shape;

            string cmd = string.Empty;

            cmd += string.Format(":SOUR1:FUNC:MODE {0}\n", sSrcFuncIV1);
            cmd += string.Format(":SOUR1:{0}:MODE {1}\n", sSrcFuncIV1, sSrcMode1);
            cmd += string.Format(":SOUR1:FUNC:SHAP {0}\n", sSrcShap1);

            return cmd;
        }

        public string SrcConfig_CH2(string func, string mode, string shape)
        {
            if (func == sSrcFuncIV2 && mode == sSrcMode2 && shape == sSrcShap2)
            {
                return string.Empty;
            }

            sSrcFuncIV2 = func;
            sSrcMode2 = mode;
            sSrcShap2 = shape;

            string cmd = string.Empty;

            cmd += string.Format(":SOUR2:FUNC:MODE {0}\n", sSrcFuncIV2);
            cmd += string.Format(":SOUR2:{0}:MODE {1}\n", sSrcFuncIV2, sSrcMode2);
            cmd += string.Format(":SOUR2:FUNC:SHAP {0}\n", sSrcShap2);

            return cmd;
        }

        public string SrcFuncMode_CH1(string func, double range)
        {
            if (func == sSrcFuncIV1)
            {
                return string.Empty;
            }

            sSrcFuncIV1 = func;
            sSrcRange1 = range;

            string cmd = string.Empty;

            cmd += string.Format(":SOUR1:FUNC:MODE {0}\n", sSrcFuncIV1);
            cmd += string.Format(":SOUR1:{0}:RANG {1}\n", sSrcFuncIV1, sSrcRange1);

            return cmd;
        }

        /// <summary>
        ///  range = Current / Voltage Source Range
        /// </summary>
        public string SrcRange_CH1(double range)
        {
            if (range == sSrcRange1)
            {
                return string.Empty;
            }

            sSrcRange1 = range;

            return string.Format(":SOUR1:{0}:RANG {1}\n", sSrcFuncIV1, sSrcRange1);
        }

        public string SrcRange_CH2(double range)
        {
            if (range == sSrcRange2)
            {
                return string.Empty;
            }

            sSrcRange2 = range;

            return string.Format(":SOUR2:{0}:RANG {1}\n", sSrcFuncIV2, sSrcRange2);
        }

        /// <summary>
        ///  level = Output Base Level;
        ///  using SrcConfig_CH2 to change Voltage / Current output
        /// </summary>
        public string SrcBaseLevel_CH1(double level)
        {
            if (level == sSrcBiasLvl1)
            {
                return string.Empty;
            }

            sSrcBiasLvl1 = level;

            return string.Format(":SOUR1:{0} {1}\n", sSrcFuncIV1, sSrcBiasLvl1);
        }

        public string SrcBaseLevel_CH2(double level)
        {
            if (level == sSrcBiasLvl2)
            {
                return string.Empty;
            }

            sSrcBiasLvl2 = level;

            return string.Format(":SOUR2:{0} {1}\n", sSrcFuncIV2, sSrcBiasLvl2);
        }

        /// <summary>
        ///  level = Output Pulse Level;
        ///  using SrcConfig_CH1 to change Voltage / Current output
        /// </summary>
        public string SrcPulseLevel_CH1(double level)
        {
            if (level == sSrcPulseLvl1)
            {
                return string.Empty;
            }

            sSrcPulseLvl1 = level;

            return string.Format(":SOUR1:{0}:TRIG {1}\n", sSrcFuncIV1, sSrcPulseLvl1);
        }

        public string SrcPulseLevel_CH2(double level)
        {
            if (level == sSrcPulseLvl2)
            {
                return string.Empty;
            }

            sSrcPulseLvl2 = level;

            return string.Format(":SOUR2:{0}:TRIG {1}\n", sSrcFuncIV2, sSrcPulseLvl2);
        }

        /// <summary>
        ///  start = start value; stop = stop value; points = sweep points
        ///  Staircase Sweep Function
        /// </summary>
        public string SrcSweep_CH1(double start, double stop, uint points)
        {
            string cmd = string.Empty;

            if (sSrcSweepStart != start)
            {
                cmd += string.Format(":SOUR1:{0}:STAR {1}\n", sSrcFuncIV1, start);
            }

            sSrcSweepStart = start;

            if (sSrcSweepStop != stop)
            {
                cmd += string.Format(":SOUR1:{0}:STOP {1}\n", sSrcFuncIV1, stop);
            }

            sSrcSweepStop = stop;

            if (sSrcSweepPoints != points)
            {
                cmd += string.Format(":SOUR1:{0}:POIN {1}\n", sSrcFuncIV1, points);
            }

            sSrcSweepPoints = points;

             return cmd;
        }

        public string SrcListSweep_CH1(string[] strArray)
        {
            string strList = string.Empty;

            for (int i = 0; i < strArray.Length; i++)
            {
                if (i == strArray.Length - 1)
                {
                    strList += strArray[i];
                }
                else
                {
                    strList += strArray[i] + ",";
                }
            }

            return string.Format(":SOUR1:LIST:{0} {1}\n", sSrcFuncIV1, strList);
        }

        public string SrcListSweep_CH2(string[] strArray)
        {
            string strList = string.Empty;

            for (int i = 0; i < strArray.Length; i++)
            {
                if (i == strArray.Length - 1)
                {
                    strList += strArray[i];
                }
                else
                {
                    strList += strArray[i] + ",";
                }
            }

            return string.Format(":SOUR2:LIST:{0} {1}\n", sSrcFuncIV2, strList);
        }

        public string SrcAutoRange_CH1(bool isAuto)
        {
            string cmd = string.Empty;

            if (isAuto)
            {
                cmd += ":SOUR1:CURR:RANGE:AUTO ON\n";
                cmd += ":SOUR1:VOLT:RANGE:AUTO ON\n";
            }
            else
            {
                cmd += ":SOUR1:CURR:RANGE:AUTO OFF\n";
                cmd += ":SOUR1:VOLT:RANGE:AUTO OFF\n";
            }

            return cmd;
        }

        public string SrcAutoRange_CH2(bool isAuto)
        {
            string cmd = string.Empty;

            if (isAuto)
            {
                cmd += ":SOUR2:CURR:RANGE:AUTO ON\n";
                cmd += ":SOUR2:VOLT:RANGE:AUTO ON\n";
            }
            else
            {
                cmd += ":SOUR2:CURR:RANGE:AUTO OFF\n";
                cmd += ":SOUR2:VOLT:RANGE:AUTO OFF\n";
            }

            return cmd;
        }

        #endregion

        #region >>> Public  Method - Measure Commend <<<

        /// <summary>
        /// Is Enable Kelvin sensing
        /// </summary>
        public string MsrtRemoteSensing_CH1(bool is4Wired)
        {
            if (is4Wired)
            {
                return ":SENS1:REM ON\n";
            }
            else
            {
                return ":SENS1:REM OFF\n";
            }
        }

        public string MsrtRemoteSensing_CH2(bool is4Wired)
        {
            if (is4Wired)
            {
                return ":SENS2:REM ON\n";
            }
            else
            {
                return ":SENS2:REM OFF\n";
            }
        }

        /// <summary>
        /// [ON] wait time = gain * initial wait time + offset; [OFF] wait time = offset
        /// </summary>
        public string MsrtWaitTime_CH1(bool isOn)
        {
            if (isOn)
            {
                return ":SENS1:WAIT 1\n";
            }
            else
            {
                return ":SENS1:WAIT 0\n";
            }
        }

        public string MsrtWaitTime_CH2(bool isOn)
        {
            if (isOn)
            {
                return ":SENS2:WAIT 1\n";
            }
            else
            {
                return ":SENS2:WAIT 0\n";
            }
        }

        /// <summary>
        ///  func = CURR | VOLT;
        /// </summary>
        public string MsrtConfig_CH1(string func)
        {
            if (func == sMsrtMode1)
            {
                return string.Empty;
            }

            sMsrtMode1 = func;

            return string.Format(":SENS1:FUNC \"{0}\"\n", sMsrtMode1);
        }

        public string MsrtConfig_CH2(string func)
        {
            if (func == sMsrtMode2)
            {
                return string.Empty;
            }

            sMsrtMode2 = func;

            return string.Format(":SENS2:FUNC \"{0}\"\n", sMsrtMode2);
        }

        /// <summary>
        ///  range = Current / Voltage Measure Range;
        /// </summary>
        public string MsrtRange_CH1(double range)
        {
            if (range == sMsrtRange1)
            {
                return string.Empty;
            }

            sMsrtRange1 = range;

            return string.Format(":SENS1:{0}:RANG {1}\n", sMsrtMode1, sMsrtRange1);
        }

        public string MsrtRange_CH2(double range)
        {
            if (range == sMsrtRange2)
            {
                return string.Empty;
            }

            sMsrtRange2 = range;

            return string.Format(":SENS2:{0}:RANG {1}\n", sMsrtMode2, sMsrtRange2);
        }

        /// <summary>
        ///  clamp = Current / Voltage Clamp;
        /// </summary>
        public string MsrtClamp_CH1(double clamp)
        {
            if (clamp == sMsrtClamp1)
            {
                return string.Empty;
            }

            sMsrtClamp1 = clamp;

            return string.Format(":SENS1:{0}:PROT {1}\n", sMsrtMode1, sMsrtClamp1);
        }

        public string MsrtClamp_CH2(double clamp)
        {
            if (clamp == sMsrtClamp2)
            {
                return string.Empty;
            }

            sMsrtClamp2 = clamp;

            return string.Format(":SENS2:{0}:PROT {1}\n", sMsrtMode2, sMsrtClamp2);
        }

        /// <summary>
        ///  NPLC = 0.00048 ~ 120 for 60 Hz
        /// </summary>
        public string MsrtNPLC_CH1(double nplc)
        {
            if (nplc == sNplc1)
            {
                return string.Empty;
            }

            sNplc1 = nplc;

            return string.Format(":SENS1:{0}:NPLC {1}\n", sMsrtMode1, sNplc1);
        }

        public string MsrtNPLC_CH2(double nplc)
        {
            if (nplc == sNplc2)
            {
                return string.Empty;
            }

            sNplc2 = nplc;

            return string.Format(":SENS2:{0}:NPLC {1}\n", sMsrtMode2, sNplc2);
        }

        public string MsrtAutoRange_CH1(bool isAuto)
        {
            string cmd = string.Empty;

            if (isAuto)
            {
                cmd += ":SENS1:CURR:RANGE:AUTO ON\n";
                cmd += ":SENS1:VOLT:RANGE:AUTO ON\n";
            }
            else
            {
                cmd += ":SENS1:CURR:RANGE:AUTO OFF\n";
                cmd += ":SENS1:VOLT:RANGE:AUTO OFF\n";
            }

            return cmd;
        }

        public string MsrtAutoRange_CH2(bool isAuto)
        {
            string cmd = string.Empty;

            if (isAuto)
            {
                cmd += ":SENS2:CURR:RANGE:AUTO ON\n";
                cmd += ":SENS2:VOLT:RANGE:AUTO ON\n";
            }
            else
            {
                cmd += ":SENS2:CURR:RANGE:AUTO OFF\n";
                cmd += ":SENS2:VOLT:RANGE:AUTO OFF\n";
            }

            return cmd;
        }

        /// <summary>
        /// offset = CURRent | STATt | 0 to maximum
        /// </summary>
        public string QueryMsrtData_CH1()
        {
            return ":SENS1:DATA? 0";
        }

        public string QueryMsrtData_CH2()
        {
            return ":SENS2:DATA? 0";
        }


        /// <summary>
        /// Sets the integration time for one point measurement.
        /// time = nplc / power line frequency
        /// value (+8E-6 to +2 seconds)|MINimum|MAXimum|DEFault (default is 0.1 PLC, =0.1/power line frequency).
        /// </summary>
        public string MsrtApertureTime_CH1(double time)
        {
            if (time == sMsrtApertureTime1)
            {
                return string.Empty;
            }

            time = time >= 8e-6 ? time : 8e-6;

            sMsrtApertureTime1 = time;

            return string.Format(":SENS1:{0}:APER {1}\n", sMsrtMode1, sMsrtApertureTime1);
        }

        public string MsrtApertureTime_CH2(double time)
        {
            if (time == sMsrtApertureTime2)
            {
                return string.Empty;
            }

            time = time >= 8e-6 ? time : 8e-6;

            sMsrtApertureTime2 = time;

            return string.Format(":SENS2:{0}:APER {1}\n", sMsrtMode2, sMsrtApertureTime2);
        }

        /// <summary>
        /// Enables or disables the automatic aperture function.
        /// mode=0 or OFF disables the automatic aperture function.
        /// mode=1 or ON (Default) enables the automatic aperture function. If this function is enabled, 
        ///              the instrument automatically sets the integration time (NPLC value) suitable for the measurement range.
        /// 
        /// </summary>
        public string MsrtAutoAperture_CH1(bool isAuto)
        {
            string cmd = string.Empty;

            if (isAuto)
            {
                cmd += ":SENS1:CURR:APER:AUTO 1\n";
                cmd += ":SENS1:VOLT:APER:AUTO 1\n";
            }
            else
            {
                cmd += ":SENS1:CURR:APER:AUTO 0\n";
                cmd += ":SENS1:VOLT:APER:AUTO 0\n";
            }

            return cmd;
        }

        public string MsrtAutoAperture_CH2(bool isAuto)
        {
            string cmd = string.Empty;

            if (isAuto)
            {
                cmd += ":SENS2:CURR:APER:AUTO 1\n";
                cmd += ":SENS2:VOLT:APER:AUTO 1\n";
            }
            else
            {
                cmd += ":SENS2:CURR:APER:AUTO 0\n";
                cmd += ":SENS2:VOLT:APER:AUTO 0\n";
            }

            return cmd;
        }

        #endregion

        #region >>> Public Method - Arm/Trig Commend <<<

        /// <summary>
        ///  Trigger init for Ch1 | Ch2 | both
        /// </summary>
        public string TrigInit(bool isTrigCh1, bool isTrigCh2)
        {
            IS_TRIG_CH1 = isTrigCh1;
            IS_TRIG_CH2 = isTrigCh2;
            
            if (isTrigCh1 && isTrigCh2)
            {
                return ":INIT (@1,2)\n";
            }
            else if (isTrigCh1)
            {
                return ":INIT (@1)\n";
            }
            else if (isTrigCh2)
            {
                return ":INIT (@2)\n";
            }

            return string.Empty;
        }

        /// <summary>
        ///  delay = Trigger Pulse delay;
        /// </summary>
        public string TrigPulseDelay_CH1(double delay)
        {
            if (delay == sTrigPulseDelay1)
            {
                return string.Empty;
            }

            sTrigPulseDelay1 = delay;

            return string.Format(":SOUR1:PULS:DEL {0}\n", sTrigPulseDelay1);
        }

        public string TrigPulseDelay_CH2(double delay)
        {
            if (delay == sTrigPulseDelay2)
            {
                return string.Empty;
            }

            sTrigPulseDelay2 = delay;

            return string.Format(":SOUR2:PULS:DEL {0}\n", sTrigPulseDelay2);
        }

        /// <summary>
        ///  delay = Trigger Pulse Width;
        /// </summary>
        public string TrigPulseWidth_CH1(double pw)
        {
            if (pw == sTrigPulseWidth1)
            {
                return string.Empty;
            }

            sTrigPulseWidth1 = pw;

            return string.Format(":SOUR1:PULS:WIDT {0}\n", sTrigPulseWidth1);
        }

        public string TrigPulseWidth_CH2(double pw)
        {
            if (pw == sTrigPulseWidth2)
            {
                return string.Empty;
            }

            sTrigPulseWidth2 = pw;

            return string.Format(":SOUR2:PULS:WIDT {0}\n", sTrigPulseWidth2);
        }

        /// <summary>
        ///  peroid = Trigger Pulse Width;
        /// </summary>
        public string TrigPeroid_CH1(double peroid)
        {
            if (peroid == sTrigPeriod1)
            {
                return string.Empty;
            }

            sTrigPeriod1 = peroid;

            return string.Format(":TRIG1:TIM {0}\n", sTrigPeriod1);
        }

        public string TrigPeroid_CH2(double peroid)
        {
            if (peroid == sTrigPeriod2)
            {
                return string.Empty;
            }

            sTrigPeriod2 = peroid;

            return string.Format(":TRIG2:TIM {0}\n", sTrigPeriod2);
        }

        /// <summary>
        ///  delay = Trigger Source Delay;
        /// </summary>
        public string TrigSrcDelay_CH1(double delay)
        {
            if (delay == sTrigTranDelay1)
            {
                return string.Empty;
            }

            sTrigTranDelay1 = delay;

            return string.Format(":TRIG1:TRAN:DEL {0}\n", sTrigTranDelay1);
        }

        public string TrigSrcDelay_CH2(double delay)
        {
            if (delay == sTrigTranDelay2)
            {
                return string.Empty;
            }

            sTrigTranDelay2 = delay;

            return string.Format(":TRIG2:TRAN:DEL {0}\n", sTrigTranDelay2);
        }

        /// <summary>
        ///  delay = Trigger Acquire Delay;
        /// </summary>
        public string TrigAcqDelay_CH1(double delay)
        {
            if (delay == sTrigAcqDelay1)
            {
                return string.Empty;
            }

            sTrigAcqDelay1 = delay;

            return string.Format(":TRIG1:ACQ:DEL {0}\n", sTrigAcqDelay1);
        }

        public string TrigAcqDelay_CH2(double delay)
        {
            if (delay == sTrigAcqDelay2)
            {
                return string.Empty;
            }

            sTrigAcqDelay2 = delay;

            return string.Format(":TRIG2:ACQ:DEL {0}\n", sTrigAcqDelay2);
        }

        /// <summary>
        ///  mode = AINT | TIM | INT1 | INT2 | LAN | EXT1~14 (IO)
        /// </summary>
        public string TrigTranSignal_CH1(string mode)
        {
            if (mode == sTrigTranSign1)
            {
                return string.Empty;
            }

            sTrigTranSign1 = mode;

            return string.Format(":TRIG1:TRAN:SOUR {0}\n", sTrigTranSign1);
        }

        public string TrigTranSignal_CH2(string mode)
        {
            if (mode == sTrigTranSign2)
            {
                return string.Empty;
            }

            sTrigTranSign2 = mode;

            return string.Format(":TRIG2:TRAN:SOUR {0}\n", sTrigTranSign2);
        }

        /// <summary>
        ///  mode = AINT | TIM | INT1 | INT2 | LAN | EXT1~14 (IO)
        /// </summary>
        public string TrigAcqSignal_CH1(string mode)
        {
            if (mode == sTrigAcqSign1)
            {
                return string.Empty;
            }

            sTrigAcqSign1 = mode;

            return string.Format(":TRIG1:ACQ:SOUR {0}\n", sTrigAcqSign1);
        }

        public string TrigAcqSignal_CH2(string mode)
        {
            if (mode == sTrigAcqSign2)
            {
                return string.Empty;
            }

            sTrigAcqSign2 = mode;

            return string.Format(":TRIG2:ACQ:SOUR {0}\n", sTrigAcqSign2);
        }

        /// <summary>
        ///  count = Trigger Count;
        /// </summary>
        public string TrigCount_CH1(uint count)
        {
            if (count == sTrigCount1)
            {
                return string.Empty;
            }

            sTrigCount1 = count;

            return string.Format(":TRIG1:COUN {0}\n", sTrigCount1);
        }

        public string TrigCount_CH2(uint count)
        {
            if (count == sTrigCount2)
            {
                return string.Empty;
            }

            sTrigCount2 = count;

            return string.Format(":TRIG2:COUN {0}\n", sTrigCount2);
        }

        /// <summary>
        /// Arm Stimulus Source
        /// mode = AINT | BUS | TIMer | INT1 | INT2 | LAN | EXT1~EXT14
        /// </summary>
        public string ArmStimulus_CH1(string mode)
        {
            if (mode == sArmSign1)
            {
                return string.Empty;
            }

            sArmSign1 = mode;

            return string.Format(":ARM1:ALL:SOUR {0}\n", sArmSign1);
        }

        public string ArmStimulus_CH2(string mode)
        {
            if (mode == sArmSign2)
            {
                return string.Empty;
            }

            sArmSign2 = mode;

            return string.Format(":ARM2:ALL:SOUR {0}\n", sArmSign2);
        }

        /// <summary>
        /// delay = Arm Delay
        /// </summary>
        public string ArmDelay_CH1(double delay)
        {
            if (delay == sArmDelay1)
            {
                return string.Empty;
            }

            sArmDelay1 = delay;

            return string.Format(":ARM:DEL {0}\n", sArmDelay1);
        }

        public string ArmDelay_CH2(double delay)
        {
            if (delay == sArmDelay2)
            {
                return string.Empty;
            }

            sArmDelay2 = delay;

            return string.Format(":ARM2:DEL {0}\n", sArmDelay2);
        }

        /// <summary>
        /// EXT I/O Trigger
        /// isTrigAssert = true; Trigger Assert
        /// extIO = EXT Pin Num; if extIO = "", Keep the setting
        /// </summary>
        public string ArmTriggerAssert_CH1(bool isTrigAssert, string extIO = "")
        {
            string cmd = string.Empty;


            cmd += string.Format(":ARM1:TOUT {0}\n", (isTrigAssert ? "1" : "0"));
            //if (isTrigAssert != sIsArmExtTrig)
            //{
            //    sIsArmExtTrig = isTrigAssert;

            //    cmd += string.Format(":ARM1:TOUT {0}\n", (sIsArmExtTrig? "1" : "0"));
            //}

            if (extIO != string.Empty)
            {
                if (extIO != sArmExtPinNum)
                {
                    sArmExtPinNum = extIO;

                    cmd += string.Format(":ARM1:TOUT:SIGN {0}\n", sArmExtPinNum);
                }
            }

            return cmd;
        }

        public string ExtTriggerAssert(int dio)
        {
            string cmd = string.Empty;

            cmd += string.Format(":DIG:DATA {0}\n", dio);
            cmd += ":DIG:DATA 0";

            return cmd;
        }

        #endregion

        #region >>> Public Method - Common Commend <<<

        public string SysLineFreq()
        {
            return ":SYST:LFR 60\n";
        }

        public string SysBeeper(bool isOn)
        {
            if (isOn)
            {
                return ":SYST:BEEP:STAT ON\n";
            }
            else
            {
                return ":SYST:BEEP:STAT OFF\n";
            }
        }

        public string SysDisplay(bool isOn)
        {
            if (isOn)
            {
                return ":DISP:ENAB ON\n";
            }
            else
            {
                return ":DISP:ENAB OFF\n";
            }
        }

        public string SysStatusReset_CH1()
        {
            string cmd = string.Empty;

            cmd += ":SOUR1:FUNC:TRIG:CONT OFF\n";
            cmd += ":ARM1:ALL:COUN 1\n";
            cmd += ":ARM1:ACQ:DEL 0\n";
            cmd += ":ARM1:TRAN:DEL 0\n";
            cmd += ":ARM1:ALL:SOUR AINT\n";
            cmd += ":ARM1:ALL:TIM MIN\n";

            // Reset Digital IO Signal 
            cmd += ":SOUR:DIG:DATA 0\n";
        
            //---------------------------------------------------------
            // Arm Trigger In   
           // cmd += ":SOUR:DIG:EXT1:FUNC TINP\n";
            //cmd += ":SOUR:DIG:EXT1:POL POS\n";
            //---------------------------------------------------------

            // Trigger Out Pin_1 Triggering SpectrumMeter
            cmd += string.Format(":SOUR:DIG:{0}:FUNC TOUT\n", CPD.PIN_SPT_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:POL POS\n", CPD.PIN_SPT_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:TOUT:POS BEF\n", CPD.PIN_SPT_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:TOUT:WIDT 50E-6\n", CPD.PIN_SPT_TRIG_OUT);

            //cmd += ":ARM1:TOUT:SIGN EXT1\n";
            //cmd += ":ARM:TOUT 1\n";

            //---------------------------------------------------------
            // Trigger Out Pin_6 Triggering NFP Camera
            cmd += string.Format(":SOUR:DIG:{0}:FUNC TOUT\n", CPD.PIN_CAMERA_NFP_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:POL POS\n", CPD.PIN_CAMERA_NFP_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:TOUT:POS BEF\n", CPD.PIN_CAMERA_NFP_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:TOUT:WIDT 50E-6\n", CPD.PIN_CAMERA_NFP_TRIG_OUT);

            // Trigger Out Pin_7 Triggering FFP Camera
            cmd += string.Format(":SOUR:DIG:{0}:FUNC TOUT\n", CPD.PIN_CAMERA_FFP_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:POL POS\n", CPD.PIN_CAMERA_FFP_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:TOUT:POS BEF\n", CPD.PIN_CAMERA_FFP_TRIG_OUT);
            cmd += string.Format(":SOUR:DIG:{0}:TOUT:WIDT 50E-6\n", CPD.PIN_CAMERA_FFP_TRIG_OUT);
            //cmd += ":ARM1:TOUT:SIGN EXT7\n";
            //cmd += ":ARM1:TOUT 1\n";

            //---------------------------------------------------------

            cmd += ":SOUR:DIG:EXT8:FUNC DINP\n";
            cmd += ":SOUR:DIG:EXT8:POL POS\n";

            cmd += ":SOUR:DIG:EXT9:FUNC DINP\n";
            cmd += ":SOUR:DIG:EXT9:POL POS\n";

            cmd += ":SOUR:DIG:EXT10:FUNC DINP\n";
            cmd += ":SOUR:DIG:EXT10:POL POS\n";

            cmd += ":SOUR:DIG:EXT13:FUNC DIO\n";
            cmd += ":SOUR:DIG:EXT13:POL NEG\n";
            cmd += ":SOUR:DIG:DATA 4096\n";

            //cmd += ":OUPT1:PROT OFF\n";
            cmd += ":ARM:TOUT 0\n";

            sArmSign1 = CPD.TRIG_INTERNAL;

            return cmd;
        }

        public string SysStatusReset_CH2()
        {
            string cmd = string.Empty;

            cmd += ":SOUR2:FUNC:TRIG:CONT OFF\n";
            cmd += ":ARM2:ALL:COUN 1\n";
            cmd += ":ARM2:ACQ:DEL 0\n";
            cmd += ":ARM2:TRAN:DEL 0\n";
            cmd += ":ARM2:ALL:SOUR AINT\n";
            cmd += ":ARM2:ALL:TIM MIN\n";

            sArmSign2 = CPD.TRIG_INTERNAL;

            return cmd;
        }

        #endregion
    }

    public static class B2900ASpec
    {
        public static double PROG_MIN_APPLY_TIME = 0.05e-3d; // ms
        public static double PROG_MAX_APPLY_TIME = 9999.99e-3d; // ms
        
        public static double[][] B2911_DC_VOLT_RANGE = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.2d,   2.0d,   6.0d },
                                                    new double[] { 20.0d },
                                                    new double[] { 200.0d },
												};

        public static double[][] B2911_DC_CURR_RANGE = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 10e-9d, 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d, 1.0d, 1.5d, 3.0d },  
                                                    new double[] { 10e-9d, 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d, 1.0d, 1.5d  },
                                                    new double[] { 10e-9d, 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d },
												};

        public static double[] B2911_PULSE_VOLT_RANGE = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													6.0d,
                                                    180.0d,
													200.0d
												};

        public static double[] B2911_PULSE_CURR_RANGE = new double[]	// [Index ], unit = I
												{	
													10.0d,
                                                    1.0d,
                                                    1.5d,
												};

        public static double[] B2911_MAX_PULSE_DUTY = new double[]
												{	
													2.5e-2d,  // 2.5%
                                                    2.5e-2d,  // 2.5%
                                                    2.5e-2d,  // 2.5%
												};

        public static double[] B2911_MAX_PULSE_WIDTH = new double[]	//unit = s
												{	
													2.5e-3d,
                                                    10.0e-3d,
                                                    1.0e-3d,
												};
    }
}
