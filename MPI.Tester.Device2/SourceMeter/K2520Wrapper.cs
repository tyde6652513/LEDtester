using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.Keithley
{
    public class CPD
    {
        public static string MODE_SWEEP = "SWE";
        public static string MODE_FIX = "FIX";
        public static string MODE_LIST = "LIST";

        public static string SWEEP_TYPE_LINEAR = "LIN";
        public static string SWEEP_TYPE_LOG = "LOG";

        public static string SHAPE_DC = "DC";
        public static string SHAPE_PULSE = "PULS";

        public static string POLAR_ANODE = "POS";
        public static string POLAR_CATHODE = "NEG";  // cathode

        public static string STAT_INTERLOCKED = "0";
    }

    public class SCPI
    {
        #region >>> Public Static Method - Common Commend <<<

        public static string RESET = "*RST";
        public static string CLEAR_STATUS = "*CLS";

        public static string QUERY_DEV_INFO = "*IDN?";
        public static string QUERY_SYS_ERR = "SYST:ERR?";
        public static string QUERY_COMPLETE = "*OPC?";

        public static string QUERY_FETCH = "FETCh?";

        public static string FORM_ACSII = "FORM ASC";
        public static string FORM_VOLT_CURR_CURR = "FORM:ELEM VOLT,CURR2,CURR3";

        public static string STAT_OPERATION = "STAT:OPER:COND?";
        public static string STAT_MEASUREMENT = "STAT:MEAS:COND?";
        public static string STAT_INTERLOCK = ":OUTP:INT:TRIP?";

        //public static string SYS_ABORT = ":ABORT";
        public static string SYS_OUT_OF_REMOTE = ":SYST:LOC";

        #endregion

        #region >>> Private Properties Status <<<

        private static bool sSrcOutput = false;
        private static string sSrcFuncMode = string.Empty; 
        private static bool sDISPlay = true;
        private static string sSrcCurrMode = string.Empty;

        private static bool sSrcTranState = false;
        private static string sSrcCurrPolar = CPD.POLAR_ANODE;
        private static double sSrcCurrRange = 999.0d;
        private static double sSrcCurrLevel = 999.0d;
        private static double sSrcCurrDelay = 999.0d;
        private static double sSrcCurrWidth = 999.0d;

        private static double sSrcBiasLevel = 999.0d;

        private static string sDataFormat = string.Empty;

        private static double sSenVoltRange = 999.0d;

        private static string sSen2CurrPol = string.Empty;
        private static double sSen2CurrRange = 999.0d;
        private static double sSrc2VoltBias = 999.0d;
        private static string sSen3CurrPol = string.Empty;
        private static double sSen3CurrRange = 999.0d;
        private static double sSrc3VoltBias = 999.0d;

        private static uint sSrcPulseCnt = 99999;
        private static double sSrcVoltClamp = 999.0d;

        private static string sSrcSweepSpac = string.Empty;
        private static double sSrcSweepStart = 999.0d;
        private static double sSrcSweepStop = 999.0d;
        private static uint sSrcSweepPoints = 0;
 
        private static string sSrcCurrList = string.Empty;
        private static string sSrcDelayList = string.Empty;
        private static string sSrcWidthList = string.Empty;
 
        #endregion

        #region >>> Public Static Method - Source Commend <<<

        /// <summary>
        /// isOn = true [Output ON] | false [Output OFF];
        /// </summary>
        public static string SrcOutput(bool isOn)
        {
            if (isOn == sSrcOutput)
            {
                return string.Empty;
            }

            sSrcOutput = isOn;

            if (sSrcOutput)
            {
                return "OUTPUT ON\n";
            }
            else
            {
                return "OUTPUT OFF\n";
            }
        }

        /// <summary>
        ///  Trigger INIT
        /// </summary>
        public static string TrigInit()
        {
            return "INIT\n";
        }

        /// <summary>
        /// Set Function Mode : DC | PULS
        /// </summary>>
        public static string SrcFuncMode(string mode)
        {
        if (mode == sSrcFuncMode)
        {
        return string.Empty;
        }

        sSrcFuncMode = mode;

        return string.Format("SOUR:FUNC {0}\n", sSrcFuncMode);
        }

        /// <summary>
        /// Source Current Mode : FIX | SWE | LIST
        /// </summary>>
        public static string SrcCurrPolarity(string polar)
        {
            if (sSrcCurrPolar == polar)
            {
                return string.Empty;
            }

            sSrcCurrPolar = polar;

            return string.Format("SOUR:CURR:POL {0}\n", sSrcCurrPolar);
        }

        /// <summary>
        /// Source Current Mode : FIX | SWE | LIST
        /// </summary>>
        public static string SrcCurrMode(string state)
        {
            if (state == sSrcCurrMode)
            {
                return string.Empty;
            }

            sSrcCurrMode = state;

            return string.Format("SOUR:CURR:MODE {0}\n", sSrcCurrMode);
        }

        /// <summary>
        /// Source Current Range(A) : 0.5 | 5 
        /// </summary>>
        public static string SrcCurrRange(double range)
        {
            if (range == sSrcCurrRange)
            {
                return string.Empty;
            }

            sSrcCurrRange = range;

            return string.Format("SOUR1:CURR:RANG {0}\n", sSrcCurrRange);        
        }

        /// <summary>
        /// Set Pulse Level : Max = 5A
        /// </summary>
        public static string SrcPulseLevel(double pulseamplitude)
        {
            if (pulseamplitude == sSrcCurrLevel)
            {
                return string.Empty;
            }

            sSrcCurrLevel = pulseamplitude;

            return string.Format("SOUR1:CURR {0}\n", sSrcCurrLevel);
        }

        /// <summary>
        /// Set Pulse Bias Level : Source Range 5A, bias 0 to 0.150A
        /// </summary>
        public static string SrcBiasLevel(double bias)
        {
            if (bias == sSrcBiasLevel)
            {
                return string.Empty;
            }

            sSrcBiasLevel = bias;

            return string.Format("SOUR1:CURR:LOW {0}\n", sSrcBiasLevel);
        }

        /// <summary>
        /// Set Pulse Delay Time : Min = 20e-6 s | Max = 0.5 s
        /// </summary>>
        public static string SrcPulseDelay(double delaytime)
        {
            if (delaytime == sSrcCurrDelay)
            {
                return string.Empty;
            }

            sSrcCurrDelay = delaytime;

            return string.Format("SOUR:PULS:DELAY {0}\n", sSrcCurrDelay);
        }
        
        /// <summary>
        /// Set Pulse Width : Min = 500e-9 s | Max = 5e-3 s
        /// </summary>>
        public static string SrcPulseWidth(double pulsewidth)
        {
            if (pulsewidth == sSrcCurrWidth)
            {
                return string.Empty;
            }

            sSrcCurrWidth = pulsewidth;

            return string.Format("SOUR:PULS:WIDTH {0}\n", sSrcCurrWidth);
        }

        /// <summary>
        /// Set Detector #1 Voltage Bias(V) : DEFault = 0
        ///                                   MINimum = -20
        ///                                   MAXimum = 20
        /// </summary>>
        public static string SrcBiasVolt_DET1(double bias)
        {
            if (bias == sSrc2VoltBias)
            {
                return string.Empty;
            }

            sSrc2VoltBias = bias;

            return string.Format("SOUR2:VOLT {0}\n", sSrc2VoltBias);
        }

        /// <summary>
        /// Set Detector #2 Voltage Bias(V) : DEFault = 0
        ///                                   MINimum = -20
        ///                                   MAXimum = 20
        /// </summary>>
        public static string SrcBiasVolt_DET2(double bias)
        {
            if (bias == sSrc3VoltBias)
            {
                return string.Empty;
            }

            sSrc3VoltBias = bias;

            return string.Format("SOUR3:VOLT {0}\n", sSrc3VoltBias);
        }
                
        /// <summary>
        /// Set Keithley 2520 Trigger Count : 1 ~ 5000
        /// </summary>
        public static string SrcTrigCount(uint cnt)
        {
            if (cnt == sSrcPulseCnt)
            {
                return string.Empty;
            }

            sSrcPulseCnt = cnt;

            if (cnt > 999)
            {
                return string.Format("TRIG:COUN INF\n");
            }
            else
            {
                return string.Format("TRIG:COUN {0}\n", sSrcPulseCnt);
            }
        }

        /// <summary>
        /// Set Sweep Spacing Type : LIN | LOG
        /// </summary>
        public static string SrcSweepSpacingType(string type)
        {
            if (type == sSrcSweepSpac)
            {
                return string.Empty;
            }
        
            sSrcSweepSpac = type;

            return string.Format("SOUR:SWE:SPAC {0}\n", sSrcSweepSpac);
        }

        /// <summary>
        /// Set Sweep Pulse Start Level
        /// </summary>>
        public static string SrcSweepStartLevel(double startlevel)
        {
            if (startlevel == sSrcSweepStart)
            {
                return string.Empty;
            }

            sSrcSweepStart = startlevel;

            return string.Format("SOUR1:CURR:STAR {0}\n", sSrcSweepStart);
        }

        /// <summary>
        /// Set Sweep Pulse Stop Level
        /// </summary>>
        public static string SrcSweepStopLevel(double stoplevel)
        {
            if (stoplevel == sSrcSweepStop)
            {
                return string.Empty;
            }

            sSrcSweepStop = stoplevel;

            return string.Format("SOUR1:CURR:STOP {0}\n", sSrcSweepStop);
        }

        /// <summary>
        /// Set Sweep Points(cnt) : 2 ~ 1000
        /// </summary>>
        public static string SrcSweepPoints(uint count)
        {
            if (count == sSrcSweepPoints)
            {
                return string.Empty;
            }

            sSrcSweepPoints = count;

            return string.Format("SOUR1:SWE:POIN {0}\n", sSrcSweepPoints);
        }

        public static string SrcBeginsContinuousPulsing()
        {
            return ":SYST:KEY 20\n";
        }

        public static string SrcStopContinuousPulsing()
        {
//            return ":SDC\n";

            return ":ABORT\n";
        }

        /// <summary>
        /// isOn = true [Enable] | false [Disable];
        /// This command enables or disables pulse transition (rise time) control.
        /// When on (1), the turn on is intentionally slowed down to a fixed setting
        /// of 5e-6. (The output will not fully settle to its final value for 5us.)
        /// </summary>
        public static string SrcTransitionCtrl(bool isOn)
        {
            if (isOn == sSrcTranState)
            {
                return string.Empty;
            }
            sSrcTranState = isOn;

            if (sSrcTranState)
            {
                return "SOUR:PULS:TRAN:STAT ON\n";
            }
            else
            {
                return "SOUR:PULS:TRAN:STAT OFF\n";
            }
        }


        public static string SrcCurrList(double[] list)
        { 
            
            string strList = string.Empty;

            foreach (var curr in list)
            {
                strList += " " + curr.ToString() + ",";
            }

            strList = strList.TrimEnd(',');

            if (strList == sSrcCurrList)
            {

                return string.Empty;

            }

            sSrcCurrList = strList;

            return string.Format("SOUR1:LIST:CURR{0}\n", strList);        
        }

        public static string SrcDelayList(double delay,uint points,uint cnt)
        { 
            string list = string.Empty;
            
            for (int i = 0; i < points * cnt; i++)
            {

                list += " " + delay.ToString() + ",";
            
            }

            list = list.TrimEnd(',');

            if (list == sSrcDelayList)
            {

                return string.Empty;

            }

            sSrcDelayList = list;

            return string.Format("SOUR1:LIST:DEL{0}\n", sSrcDelayList);
        
        }

        public static string SrcWidthList(double width, uint points, uint cnt)
        {
            string list = string.Empty;

            for (int i = 0; i < points * cnt; i++)
            {

                list += " " + width.ToString() + ",";

            }

            list = list.TrimEnd(',');

            if (list == sSrcWidthList)
            {

                return string.Empty;

            }

            sSrcWidthList = list;

            return string.Format("SOUR1:LIST:WIDT{0}\n", sSrcWidthList);

        }


        #endregion

        #region >>> Public Static Method - Measure Commend <<<

        /// <summary>
        /// Set Voltage Measure Range(V) : 5 | 10 
        /// </summary>>
        public static string MsrtVoltRange(double range)
        {
            if (range == sSenVoltRange)
            {
                return string.Empty;
            }

            sSenVoltRange = range;

            return string.Format("SENS:VOLT:RANG {0}\n", sSenVoltRange);
        }

        /// <summary>
        /// Set Voltage Clamp(V) : 3 ~ 10.5
        /// </summary>>
        public static string MsrtVoltClamp(double clamp)
        {
            if (clamp == sSrcVoltClamp)
            {
                return string.Empty;
            }

            sSrcVoltClamp = clamp;

            return string.Format("SOUR:VOLT:PROT {0}\n", sSrcVoltClamp);
        }

        /// <summary>
        /// Set Detector #1 measure polarity : POS | NEG
        /// </summary>>
        public static string MsrtCurrPolar_DET1(string polar)
        {
            if (polar == sSen2CurrPol)
            {
                return string.Empty;
            }

            sSen2CurrPol = polar;

            return string.Format("SENS2:CURR:POL {0}\n", sSen2CurrPol);
        }

        /// <summary>
        /// Set Photodiode #1 Current Measure Range(A) : 0.01 | 0.02 | 0.05 | 0.1
        /// </summary>>
        public static string MsrtCurrRange_DET1(double range)
        {
            if (range == sSen2CurrRange)
            {
                return string.Empty;
            }

            sSen2CurrRange = range;

            return string.Format("SENS2:CURR:RANG {0}\n", sSen2CurrRange);
        }

        /// <summary>
        /// Set Detector #2 measure polarity : POS | NEG
        /// </summary>>
        public static string MsrtCurrPolar_DET2(string polar)
        {
            if (polar == sSen3CurrPol)
            {
                return string.Empty;
            }

            sSen3CurrPol = polar;

            return string.Format("SENS3:CURR:POL {0}\n", sSen3CurrPol);
        }

        /// <summary>
        /// Set Photodiode #2 Current Measure Range(A) : 0.01 | 0.02 | 0.05 | 0.1
        /// </summary>>
        public static string MsrtCurrRange_DET2(double range)
        {
            if (range == sSen3CurrRange)
            {
                return string.Empty;
            }

            sSen3CurrRange = range;

            return string.Format("SENS3:CURR:RANG {0}\n", sSen3CurrRange);
        }

        #endregion

        #region >>> Public Static Method - Common Commend <<<

        /// <summary>
        /// isOn = true [DISPlay ON] | false [DISPlay OFF]
        /// </summary>>
        public static string SysDisplay(bool isOn)
        {
            if (isOn == sDISPlay)
            {
                return string.Empty;
            }

            sDISPlay = isOn;

            if (sDISPlay)
            {
                return "DISP:ENAB ON\n";
            }
            else
            {
                return "DISP:ENAB OFF\n";
            }
        }

        public static string DioOutput(uint io)
        {
            return string.Format("SOUR4:TTL {0}\n", io);
        }

        #endregion
    }


}
