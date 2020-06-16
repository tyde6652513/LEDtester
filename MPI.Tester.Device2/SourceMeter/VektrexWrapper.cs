using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.Pulser.Vektrex
{
    public class CPD
    {
        public static string MODE_SWEEP = "SWE";
        public static string MODE_FIX = "FIX";
        public static string MODE_LIST = "LIST";

        public static string SHAPE_DC = "DC";
        public static string SHAPE_SINGLE_PULSE = "SINGLEPULSE";
        public static string SHAPE_PULSE_SWEEP = "PULSEDSWEEP";
        public static string SHAPE_MULTIPULSE = "MULTIPULSE";

        public static string RANGE_HIGH = "HIGH";
        public static string RANGE_LOW = "LOW";

        public static string TRIG_POL_RISING = "RISING";
        public static string TRIG_POL_FALLING = "FALLING";

        public static string PULSE_HOLD_WIDTH = "WID";
        public static string PULSE_HOLD_DUTY = "DCYC";
        public static string PULSE_HOLD_PERIOD = "PER";

        public static string RAMP_SPEED_DEFAULT = "DEFAULT";
        public static string RAMP_SPEED_FAST = "FAST";
        public static string RAMP_SPEED_SLOW = "SLOW";

        //---------------------------------------------------------------------------------
        // Channel Event
        public static string STATUS_EVENT_NO_ERROR = "OK";
        public static string STATUS_EVENT_CHANNEL_READY = "100";
        public static string STATUS_EVENT_CHANNEL_COMPLETE = "109";
        public static string STATUS_EVENT_CHANNEL_SHUT_DOWN_SWEEP = "110";

        //---------------------------------------------------------------------------------
        // Load Fault
        public static string STATUS_LOAD_OVER_VOLTAGE_SWEEP = "200";
        public static string STATUS_LOAD_HIGH_SIDE_OVER_CURRENT = "201";
        public static string STATUS_LOAD_LOW_SIDE_OVER_CURRENT = "202";
                                                                      // 203 Not Define
        public static string STATUS_LOAD_VOLT_RAMP = "204";
        public static string STATUS_LOAD_LEAKAGE_FROM_EXT_SOURCE = "205";
        public static string STATUS_LOAD_OVER_VOLTAGE = "206";
        public static string STATUS_LOAD_CURRENT_LEAKAGE = "207";
        public static string STATUS_LOAD_EXCESSIVE_INT_VOLTAGE = "208";

        //---------------------------------------------------------------------------------
        // Instrument Error
        public static string STATUS_ERROR_CHANNEL_NOT_READY = "351";


        //---------------------------------------------------------------------------------
        public static string STATUS_OPC_PULSE_END_TRUE = "TRUE";
        public static string STATUS_OPC_PULSE_END_FLASE = "FALSE";
        public static string STATUS_OPC_PULSE_END_ERR = "ERROR";

        public static string TRIG_EXTERNAL = "EXT";
        public static string TRIG_INTERNAL = "INT";

        public enum ECableCompFactor : uint
        {
            High    = 1,
            Medium  = 2,
            Low     = 3,
            VeryLow = 4,
        }

        public enum ERiseTimeCompFactor : uint
        {
            Fast     = 1,
            Medium   = 2,
            Slow     = 3,
            VerySlow = 4,
        }
    }

    public class SCPI
    {
        #region >>> Public Static Method - Common Commend <<<

        public static string RESET = "*RST";
        public static string CLEAR_STATUS = "*CLS";

        public static string QUERY_DEV_INFO = "*IDN?";
        public static string QUERY_SYS_EVENT_QUEUE = "SYST:ERR?";
        public static string QUERY_SYS_EVENT_QUEUE_CNT = "SYST:ERR:COUN?";
        public static string QUERY_COMPLETE = "SOUR1:PULS:END?";


        #endregion

        #region >>> Private Properties Status <<<
        // Channel-1
  
        private static string sSrcFuncIV1 = string.Empty;
        private static string sSrcMode1 = string.Empty;

        private static CPD.ECableCompFactor sSrcConfigCCom = CPD.ECableCompFactor.Medium;
        private static CPD.ERiseTimeCompFactor sSrcConfigRCom = CPD.ERiseTimeCompFactor.Medium;

        private static string sSrcRampSpeed = string.Empty;
        private static string sSrcShap1 = string.Empty;
        private static string sSrcRange1 = string.Empty;
        private static double sSrcPulseLvl1 = 999.0d;
        private static double sSrcBiasLvl1 = 999.0d;
        private static double sSrcVoltClamp1 = 999.0d;
        private static uint sSrcPulseCnt1 = 999;
        private static string sSrcPulseHold1 = string.Empty;

        private static double sTrigPulseDelay1 = 999.0d;
        private static double sTrigPulseOnTime1 = 999.0d; //s
        private static double sTrigPulseOffTime1 = 999.0d; //s

        private static double sTrigInputDelay1 = 0.0d;

        private static double sSrcSweepStart = 999.0d;
        private static double sSrcSweepStop = 999.0d;
        private static uint sSrcSweepPoints = 0;

        private static bool sOutput1 = false;

        private static string sSrcTrigMode = CPD.TRIG_INTERNAL;

        #endregion

        #region >>> Public Static Method <<<

        public string SrcShape1
        {
            get { return sSrcShap1; }
        }

        #endregion

        #region >>> Public Static Method <<<

        /// <summary>
        ///  isOn = true [Output ON] | false [Output OFF];
        /// </summary>
        public static string SrcOutput_CH1(bool isOn)
        {
            if (isOn == sOutput1)
            {
                return string.Empty;
            }

            sOutput1 = isOn;

            if (sOutput1)
            {
                return "OUTP1 1\n";
            }
            else
            {
                return "OUTP1 0\n";
            }
        }

        /// <summary>
        ///  shape = SINGLEPULSE | MULTIPULSE | PULSEDSWEEP 
        /// </summary>
        public static string SrcConfig_CH1(string shape)
        {
            if (shape == sSrcShap1)
            {
                return string.Empty;
            }

            sSrcShap1 = shape;

            string cmd = string.Empty;

            cmd += string.Format("SOUR1:FUNC:SHAP {0}\n", sSrcShap1);

            return cmd;
        }

        /// <summary>
        ///  shape = DEFAULT | FAST | SLOW
        /// </summary>
        public static string SrcRampSpeed_CH1(string speed)
        {
            if (speed == sSrcRampSpeed)
            {
                return string.Empty;
            }

            sSrcRampSpeed = speed;

            string cmd = string.Empty;

            cmd += string.Format("OUTP1:RAMP {0}\n", sSrcRampSpeed);

            return cmd;
        }

        /// <summary>
        ///  Channel Cable Comp Factor 
        /// </summary>
        public static string SrcConfigCcom_CH1(CPD.ECableCompFactor ccom)
        {
            if (ccom == sSrcConfigCCom)
            {
                return string.Empty;
            }

            sSrcConfigCCom = ccom;

            string cmd = string.Empty;

            cmd += string.Format("SOUR1:PULS:CCOM {0}\n", (uint)sSrcConfigCCom);

            return cmd;
        }

        /// <summary>
        ///  Channel Rise Time Comp Factor 
        /// </summary>
        public static string SrcConfigRcom_CH1(CPD.ERiseTimeCompFactor rcom)
        {
            if (rcom == sSrcConfigRCom)
            {
                return string.Empty;
            }

            sSrcConfigRCom = rcom;

            string cmd = string.Empty;

            cmd += string.Format("SOUR1:PULS:RCOM {0}\n", (uint)sSrcConfigRCom);

            return cmd;
        }

        /// <summary>
        ///  range = Current / Voltage Source Range
        /// </summary>
        public static string SrcAutoRange_CH1(bool isAuto)
        {
            if (isAuto)
            {
                return "SOUR1:CURR:RANG:AUTO 1\n";
            }
            else
            {
                return "SOUR1:CURR:RANG:AUTO 0\n";
            }
        }

        /// <summary>
        ///  range = Current / Voltage Source Range
        /// </summary>
        public static string SrcRange_CH1(string range)
        {
            if (range == sSrcRange1)
            {
                return string.Empty;
            }

            sSrcRange1 = range;

            return string.Format("SOUR1:CURR:RANG {0}\n", sSrcRange1);
        }

        /// <summary>
        ///  cnt = Output Pulse count;
        /// </summary>
        public static string SrcPulseHold_CH1(string hold)
        {
            if (hold == sSrcPulseHold1)
            {
                return string.Empty;
            }

            sSrcPulseHold1 = hold;

            return string.Format("SOUR1:PULS:HOLD {0}\n", sSrcPulseHold1);
        }

        /// <summary>
        ///  cnt = Output Pulse count;
        /// </summary>
        public static string SrcPulseCount_CH1(uint cnt)
        {
            if (cnt == sSrcPulseCnt1)
            {
                return string.Empty;
            }

            sSrcPulseCnt1 = cnt;

            return string.Format("SOUR1:PULS:COUN {0}\n", sSrcPulseCnt1);
        }

        /// <summary>
        ///  level = Output Pulse Level;
        /// </summary>
        public static string SrcPulseLevel_CH1(double level)
        {
            if (level == sSrcPulseLvl1)
            {
                return string.Empty;
            }

            sSrcPulseLvl1 = level;

            return string.Format("SOUR1:CURR {0}\n", sSrcPulseLvl1);
        }

        /// <summary>
        ///  delay = Trigger Pulse delay;
        /// </summary>
        public static string SrcPulseTrigDelay_CH1(double delay)
        {
            if (delay == sTrigPulseDelay1)
            {
                return string.Empty;
            }

            sTrigPulseDelay1 = delay;

            return string.Format("SOUR1:PULS:TRIG:DEL {0}\n", sTrigPulseDelay1);
        }

        /// <summary>
        ///  pw = Pulse On Time;
        /// </summary>
        public static string SrcPulseOnTime_CH1(double tOn)
        {
            if (tOn == sTrigPulseOnTime1)
            {
                return string.Empty;
            }

            sTrigPulseOnTime1 = tOn;

            return string.Format("SOUR1:PULS:TON {0}\n", sTrigPulseOnTime1);
        }

        /// <summary>
        ///  delay = Trigger Pulse Width;
        /// </summary>
        public static string SrcPulseOffTime_CH1(double tOff)
        {
            if (tOff == sTrigPulseOffTime1)
            {
                return string.Empty;
            }

            sTrigPulseOffTime1 = tOff;

            return string.Format("SOUR1:PULS:TOFF {0}\n", sTrigPulseOffTime1);
        }

        /// <summary>
        ///  delay = Trigger Pulse Width;
        /// </summary>
        public static string SrcPulseDuty_CH1(double duty)
        {
            if (duty == sTrigPulseOnTime1)
            {
                return string.Empty;
            }

            sTrigPulseOnTime1 = duty;

            return string.Format("SOUR1:PULS:TON {0}\n", sTrigPulseOnTime1);
        }

        /// <summary>
        ///  delay = Trigger Pulse Width;
        /// </summary>
        public static string SrcVoltageClamp_CH1(double clamp)
        {
            if (clamp == sSrcVoltClamp1)
            {
                return string.Empty;
            }

            sSrcVoltClamp1 = clamp;

            return string.Format("SOUR1:VOLT {0}\n", sSrcVoltClamp1);
        }

        /// <summary>
        ///  value = Sweep Start Value;
        /// </summary>
        public static string SrcSweepStart_CH1(double value)
        {
            if (value == sSrcSweepStart)
            {
                return string.Empty;
            }

            sSrcSweepStart = value;

            return string.Format("SOUR1:CURR:STAR {0}\n", sSrcSweepStart);
        }

        /// <summary>
        ///  value = Sweep Stop Value;
        /// </summary>
        public static string SrcSweepStop_CH1(double value)
        {
            if (value == sSrcSweepStop)
            {
                return string.Empty;
            }

            sSrcSweepStop = value;

            return string.Format("SOUR1:CURR:STOP {0}\n", sSrcSweepStop);
        }

        /// <summary>
        ///  points = Sweep Points;
        /// </summary>
        public static string SrcSweepPoints_CH1(uint points)
        {
            if (points == sSrcSweepPoints)
            {
                return string.Empty;
            }

            sSrcSweepPoints = points;

            return string.Format("SOUR1:CURR:STEP {0}\n", sSrcSweepPoints);
        }

        /// <summary>
        ///  Trigger init for Ch1
        /// </summary>
        public static string TrigInit()
        {
            return "OUTP1:TRIG\n";
        }

        /// <summary>
        ///  Source Trigger Mode: EXT (External) | INT (Internal) 
        /// </summary>
        public static string SourceTrggerMode1(string mode)
        {
            if (mode == sSrcTrigMode)
            {
                return string.Empty;
            }

            sSrcTrigMode = mode;

            return string.Format("OUTP1:TRIG:SOUR {0}\n", sSrcTrigMode);
        }

        /// <summary>
        ///  Input trigger delay for ch1
        /// </summary>
        public static string TrigInputDelay1(double delay)
        {
            if (delay == sTrigInputDelay1)
            {
                return string.Empty;
            }

            sTrigInputDelay1 = delay;

            return string.Format("OUTP1:TRIG:DEL {0}\n", sTrigInputDelay1);
        }

        /// <summary>
        ///  Input trigger delay for ch1
        /// </summary>
        public static string TrigInputPolarity1(string polar)
        {
            return string.Format("OUTP1:TRIG:POL {0}\n", polar);
        }

        #endregion
    }

    public static class SS400Spec
    {
        public static double PROG_MIN_APPLY_TIME = 1e-6d; // us
        public static double PROG_MAX_APPLY_TIME = 9999.99e-3d; // ms
        public static double PROG_MIN_OFF_TIME = 300e-6d; // us

        public static double[][] SS400_DC_VOLT_RANGE = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 10.0d, 30.0d },
                                                
												};

        public static double[][] SS400_DC_CURR_RANGE = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 0.4d, 10.0d },  
                                                
												};

        public static double[] SS400_PULSE_VOLT_RANGE = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													10.0d, 
                                                    30.0d,
												};

        public static double[] SS400_PULSE_CURR_RANGE = new double[]	// [Index ], unit = I
												{	
													0.4d, 
                                                    10.0d,
												};

        public static double[] SS400_MAX_PULSE_DUTY = new double[]
												{	
													  33e-2d,  // 33%  
                                                      33e-2d,  // 33% 
												};

        public static double[] SS400_MAX_PULSE_WIDTH = new double[]	//unit = s
												{	
													100e-3d,
                                                    100e-3d,
												};
    }
}
