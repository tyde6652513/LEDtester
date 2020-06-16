using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;

namespace MPI.Tester.Device.SourceMeter.Keithley
{
	public partial class K2600Wrapper
	{
		#region >>> Keithley 2600 Voltage and Current Range <<<

		private static double[][] _k2601VoltRange = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.1d,   1.0d,   6.0d },
                                                    new double[] { 40.0d },
												};

		private static double[][] _k2601CurrRange = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d, 1.0d, 3.0d },  
                                                    new double[] { 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d, 1.0d },
												};

        private static double[] _k2601PulseVoltRange = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													20.0d,
                                                    35.0d,
													40.0d 
												};

        private static double[] _k2601PulseCurrRange = new double[]	// [Index ], unit = I
												{	
													10.0d,
                                                    5.0d,
													1.5d
												};

        private static double[] _k2601PulseMaxTime = new double[]	// [Index ], unit = s
												{	
													0.0018d,	//  1.8ms
                                                    0.0040d,	//  4.0ms
													0.0100d	    //100.0ms
												};

        private static double[] _k2601PulseDuty = new double[]	// [Index ], unit =
												{	
													0.010d,	// 1.0%
                                                    0.040d, // 4.0%
													0.250d	//25.0%
												};

		private static double[][] _k2611VoltRange = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.20d,   2.0d,   20.0d },
                                                    new double[] { 200.0d },
												};

		private static double[][] _k2611CurrRange = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d, 1.0d, 1.5d },  
                                                    new double[] { 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d,  },
												};

        private static double[] _k2611PulseVoltRange = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													5.0d,
                                                    180.0d,
													200.0d 
												};

        private static double[] _k2611PulseCurrRange = new double[]	// [Index ], unit = I
												{	
													10.0d,
                                                    1.0d,
													1.0d
												};

        private static double[] _k2611PulseMaxTime = new double[]	// [Index ], unit = s
												{	
													0.0010d,	//1.0ms
                                                    0.00850d,	//8.5ms
													0.00220d	//2.2ms
												};

        private static double[] _k2611PulseDuty = new double[]	// [Index ], unit =
												{	
													0.0220d,	//2.2%
                                                    0.010d,		//1.0%
													0.010d		//1.0%
												};

        private static double[][] _k2635VoltRange = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.20d,   2.0d,   20.0d },
                                                    new double[] { 200.0d },
												};

        private static double[][] _k2635CurrRange = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-12d, 1e-9d,  10e-9d,  100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  1.5d },  
                                                    new double[] { 100e-12d, 1e-9d,  10e-9d,  100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  },
												};


        private static double[] _k2635PulseVoltRange = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													5.0d,
                                                    180.0d,
													200.0d 
												};

        private static double[] _k2635PulseCurrRange = new double[]	// [Index ], unit = I
												{	
													10.0d,
                                                    1.0d,
													1.0d
												};

        private static double[] _k2635PulseMaxTime = new double[]	// [Index ], unit = s
												{	
													0.0010d,	//1.0ms
                                                    0.00850d,	//8.5ms
													0.00220d	//2.2ms
												};

        private static double[] _k2635PulseDuty = new double[]	// [Index ], unit =
												{	
													0.0220d,	//2.2%
                                                    0.010d,		//1.0%
													0.010d		//1.0%
												};

        private static double[][] _k2651VoltRange = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.10d,   1.0d,   10.0d },
                                                    new double[] { 20.0d },
                                                    new double[] { 40.0d },
												};

        private static double[][] _k2651CurrRange = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  5.0d,   10.0d,   20.0d, }, 
                                                    new double[] { 100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  5.0d,   10.0d, },
                                                    new double[] { 100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  5.0d, }, 
												};


        private static double[] _k2651PulseVoltRange = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													10.0d,
                                                    20.0d,
													40.0d,
 													10.0d,
                                                    20.0d,
													40.0d
												};

        private static double[] _k2651PulseCurrRange = new double[]	// [Index ], unit = I
												{	
													30.0d,
                                                    20.0d,
													10.0d,
													50.0d,
                                                    50.0d,
													50.0d
												};

        private static double[] _k2651PulseMaxTime = new double[]	// [Index ], unit = s
												{	
													0.0010d,	//1.0ms
                                                    0.00150d,	//1.5ms
													0.00150d,	//1.5ms
													0.0010d,	//1.0ms
                                                    0.000330d,	//330us
													0.000330d	//330us
												};

        private static double[] _k2651PulseDuty = new double[]	// [Index ], unit =
												{	
													0.50d,	//50%
                                                    0.50d,	//50%
													0.40d,	//40%
													0.350d,	//35%
                                                    0.10d,	//10%
													0.010d	// 1%
												};

		#endregion
         
		#region >>> I/O Ping Define <<<

		// Enable二極體:		_pinRTHEnable = 0  
		// Bypass 二極體:		_pinRTHEnable = 1
		// P:				_pinDAQEnable = 1, _pinPolarSW = 0
		// N:				_pinDAQEnable = 0, _pinPolarSW = 1
		// Open:			_pinDAQEnable = 0, _pinPolarSW = 0
		//小電容:				_pinCapSW = 0
		//大電容:				_pinCapSW = 1

        /*private const uint PIN_SPT_TRIG_OUT  = 1;

        private const uint PIN_SMU_TRIG_OUT  = 2;
        private const uint PIN_SMU_ABORT_OUT = 3;
        private const uint PIN_SMU_ABORT_IN  = 4;
        private const uint PIN_SMU_TRIG_IN   = 11;
       
        private const uint PIN_DAQ_ENABLE   = 6;
        private const uint PIN_RTH_EANBLE   = 7;
		private const uint PIN_CAP_SW       = 12;
		private const uint PIN_DAQ_TRIG_OUT = 13;
		private const uint PIN_POLAR_SW     = 14;

        private const double _ioPulseWidth = 0.005;*/


 		public static uint PIN_SPT_TRIG_OUT  = 1;
        public static uint PIN_SMU_TRIG_OUT = 2;
        public static uint PIN_SMU_SYNC = 3;

        public static uint PIN_SMU_ABORT_IN = 4;  // no used
        public static uint PIN_SMU_TRIG_IN = 11; // no used

        public static uint PIN_FrameGround = 5;

        public static uint PIN_DAQ_ENABLE   = 6;
        public static uint PIN_CAMERA_TRIG_OUT = 7;
		public static uint PIN_CAP_SW       = 12;
		public static uint PIN_DAQ_TRIG_OUT = 13;
		public static uint PIN_POLAR_SW     = 14;

        public static double _ioPulseWidth = 0.0001;

		#endregion

		#region >>> Private Proberty <<<

        private double _lineFreq = 60.0d;

		private const double MAX_DELAY_TIME = 999;	// Unit: s
		private const double MIN_DELAY_TIME = 0.0;	// Unit: s

        private string _name;
		private IConnect _conn;
		private int _srcSettling;
		private string _hwVersion;
		private string _swVersion;
		private string _serialNum;
		private double[][] _voltRange;
		private double[][] _currRange;
		private ElecDevSetting _elecDevSetting;
        private LANSettingData _lanSetting;

        private List<double> _sweepList;
        private List<double> _sweepTimeSpan;

        private double[] _voltPulseRange;
        private double[] _currPulseRange;
        private double[] _pulseMaxTime;
        private double[] _pulseDuty;

        private bool _isDualSMU;

		#endregion

		#region >>> Constructor / Disposor <<<

		private K2600Wrapper()
		{
            this._name = string.Empty;		

			this._srcSettling = (int)ESrcSettling.FAST_ALL;

            this._sweepList = new List<double>();
            this._sweepTimeSpan = new List<double>();
		}

        public K2600Wrapper(string name, ElecDevSetting setting, string ipAddress) : this()
		{
            this._name = name;

			this._elecDevSetting = setting;

            this._lanSetting = new LANSettingData();

            this._lanSetting.IPAddress = ipAddress;

            this._conn = new LANConnect(this._lanSetting);
		}

		#endregion

		#region >>> Private Method Keithley 2600 Script <<<

		private bool GetDeviceInfomation()
		{
            this._isDualSMU = false;
            
            this._conn.SendCommand("reset()");

			this._conn.SendCommand("print(localnode.model, localnode.serialno, localnode.revision)");

			string result = string.Empty;

            string[] info = this.GetDevicePrintValueToArray('\t');

			if (info == null || info.Length < 3)
			{
				return false;
			}

            string deviceModel = info[0];

            this._serialNum = info[1];

            this._swVersion = info[2];

			this._hwVersion = deviceModel;

            if (deviceModel.Contains("2601") || deviceModel.Contains("2602"))
            {
                this._voltRange = _k2601VoltRange;

                this._currRange = _k2601CurrRange;

                this._voltPulseRange = _k2601PulseVoltRange;

                this._currPulseRange = _k2601PulseCurrRange;

                this._pulseMaxTime = _k2601PulseMaxTime;

                this._pulseDuty = _k2601PulseDuty;
            }
            else if (deviceModel.Contains("2611") || deviceModel.Contains("2612"))
            {
                //----------------------------------------------------------------------------------------------                
                if (deviceModel.Contains("L"))
                {
                    // K2600-L Remove 100 nA Range 
                    this._currRange = new double[2][];
                    this._currRange[0] = new double[_k2611CurrRange[0].Length - 1];
                    this._currRange[1] = new double[_k2611CurrRange[1].Length - 1];

                    Array.Copy(_k2611CurrRange[0], 1, this._currRange[0], 0, _k2611CurrRange[0].Length - 1);
                    Array.Copy(_k2611CurrRange[1], 1, this._currRange[1], 0, _k2611CurrRange[1].Length - 1);
                }
                else
                {
                    this._currRange = _k2611CurrRange;
                }
                //----------------------------------------------------------------------------------------------

                this._voltRange = _k2611VoltRange;

                this._voltPulseRange = _k2611PulseVoltRange;

                this._currPulseRange = _k2611PulseCurrRange;

                this._pulseMaxTime = _k2611PulseMaxTime;

                this._pulseDuty = _k2611PulseDuty;
            }
            else if (deviceModel.Contains("2635") || deviceModel.Contains("2636"))
            {
                this._voltRange = _k2635VoltRange;

                this._currRange = _k2635CurrRange;

                this._voltPulseRange = _k2635PulseVoltRange;

                this._currPulseRange = _k2635PulseCurrRange;

                this._pulseMaxTime = _k2635PulseMaxTime;

                this._pulseDuty = _k2635PulseDuty;
            }
            else if (deviceModel.Contains("2651"))
            {
                this._voltRange = _k2651VoltRange;

                this._currRange = _k2651CurrRange;

                this._voltPulseRange = _k2651PulseVoltRange;

                this._currPulseRange = _k2651PulseCurrRange;

                this._pulseMaxTime = _k2651PulseMaxTime;

                this._pulseDuty = _k2651PulseDuty;
            }
            else
			{
                return false;
			}

            if (deviceModel.Contains("2612") || deviceModel.Contains("2602") || deviceModel.Contains("2636"))
            {
                this._isDualSMU = true;
            }

            //-------------------------------------------------------------------------------------
            // Get SMU power line frequency 
            this._conn.SendCommand("print(localnode.linefreq)");

            string[] linefreq = this.GetDevicePrintValueToArray('\n');

            double.TryParse(linefreq[0], out this._lineFreq);

            if (this._lineFreq > 60.0d || this._lineFreq < 50.0d)
            {
                this._lineFreq = 60.0d;
            }

            Console.WriteLine("[K2600Wrapper], GetDeviceInfomation(), linefreq(Hz) = " + this._lineFreq.ToString());

			return true;
		}

		private bool SetConfig()
		{
            if (this._name == "SLAVE_PD" && this._elecDevSetting.IsDetectorHwTrig)
            {
                // Slave Config (K2611 * 2, H/W Trigger)
                this.SetSlavePDHwTrigConfigScript();
            }
            else
            {
                // Master Config (K2611 * 2, S/W Trigger & K2612 SMUB)
                this.SetMasterConfigScript();
            }

            this.SetScriptSetterConfig();

            this.SetFunctionToDevice();

			if (this.GetErrorMsg())
			{
				this.Close();

				return false;
			}

			return true;
		}

        private void SetScriptSetterConfig()
        {
            string cmd = string.Empty;

            //---------------------------------------------------------------------------------------------------------------------------
            cmd += "setLimitV = makesetter(smua.source, \"limitv\")\n";

            cmd += "setLimitI = makesetter(smua.source, \"limiti\")\n";

            cmd += "setSorceRangeV = makesetter(smua.source, \"rangev\")\n";

            cmd += "setSorceRangeI = makesetter(smua.source, \"rangei\")\n";

            cmd += "setLevelV = makesetter(smua.source, \"levelv\")\n";

            cmd += "setLevelI = makesetter(smua.source, \"leveli\")\n";

            cmd += "getFunc = makegetter(smua.source, \"func\")\n";

            cmd += "setFunc = makesetter(smua.source, \"func\")\n";

            cmd += "setMeasureRangeV = makesetter(smua.measure, \"rangev\")\n";

            cmd += "setMeasureRangeI = makesetter(smua.measure, \"rangei\")\n";

            cmd += "setMeasureDelay = makesetter(smua.measure, \"delay\")\n";

            cmd += "mrtA = smua.measure\n";

            cmd += "mrtIsyncA = smua.measure.overlappedi\n";

            cmd += "mrtVsyncA = smua.measure.overlappedv\n";

            cmd += "mrtIVsyncA = smua.measure.overlappediv\n";

            cmd += "setNPLC = makesetter(smua.measure, \"nplc\")\n";

            cmd += "setMsrtCount = makesetter(smua.measure, \"count\")";

            cmd += "setMsrtInterval = makesetter(smua.measure, \"interval\")\n";

            cmd += "setBufTimestamps = makesetter(smua.nvbuffer1, \"collecttimestamps\")\n";

            cmd += "bufA1 = smua.nvbuffer1\n";

            cmd += "bufA2 = smua.nvbuffer2\n";

            cmd += "setOutput = makesetter(smua.source, \"output\")\n";

            cmd += "getOutput = makegetter(smua.source, \"output\")\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // Pulse Mode Command
            // Trigger / Arm Layer Setter
            cmd += "T1_Count = makesetter(trigger.timer[1], \"count\")\n";

            cmd += "T1_Delay = makesetter(trigger.timer[1], \"delay\")\n";

            cmd += "T1_Passthrough = makesetter(trigger.timer[1], \"passthrough\")\n";

            cmd += "T1_Stimulus = makesetter(trigger.timer[1], \"stimulus\")\n";

            cmd += "T2_Count = makesetter(trigger.timer[2], \"count\")\n";

            cmd += "T2_Delay = makesetter(trigger.timer[2], \"delay\")\n";

            cmd += "T2_Passthrough = makesetter(trigger.timer[2], \"passthrough\")\n";

            cmd += "T2_Stimulus = makesetter(trigger.timer[2], \"stimulus\")\n";

            cmd += "T3_Count = makesetter(trigger.timer[3], \"count\")\n";

            cmd += "T3_Delay = makesetter(trigger.timer[3], \"delay\")\n";

            cmd += "T3_Passthrough = makesetter(trigger.timer[3], \"passthrough\")\n";

            cmd += "T3_Stimulus = makesetter(trigger.timer[3], \"stimulus\")\n";

            cmd += "T4_Count = makesetter(trigger.timer[4], \"count\")\n";

            cmd += "T4_Delay = makesetter(trigger.timer[4], \"delay\")\n";

            cmd += "T4_Passthrough = makesetter(trigger.timer[4], \"passthrough\")\n";

            cmd += "T4_Stimulus = makesetter(trigger.timer[4], \"stimulus\")\n";

            cmd += "Trig_T1 = trigger.timer[1]\n";

            cmd += "Trig_T2 = trigger.timer[2]\n";

            cmd += "Trig_T3 = trigger.timer[3]\n";

            cmd += "Trig_T4 = trigger.timer[4]\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // smua setter
            cmd += "TrigA_LimitV = makesetter(smua.trigger.source, \"limitv\")\n";

            cmd += "TrigA_LimitI = makesetter(smua.trigger.source, \"limiti\")\n";

            cmd += "TrigA_Action_Src = makesetter(smua.trigger.source, \"action\")\n";

            cmd += "TrigA_Action_Msrt = makesetter(smua.trigger.measure, \"action\")\n";

            cmd += "TrigA_Action_EndPulse = makesetter(smua.trigger.endpulse, \"action\")\n";

            cmd += "TrigA_Action_EndSweep = makesetter(smua.trigger.endsweep, \"action\")\n";

            cmd += "TrigA_Stimulus_Arm = makesetter(smua.trigger.arm, \"stimulus\")\n";

            cmd += "TrigA_Stimulus_Src = makesetter(smua.trigger.source, \"stimulus\")\n";

            cmd += "TrigA_Stimulus_Msrt = makesetter(smua.trigger.measure, \"stimulus\")\n";

            cmd += "TrigA_Stimulus_EndPulse = makesetter(smua.trigger.endpulse, \"stimulus\")\n";

            cmd += "TrigA_Count = makesetter(smua.trigger, \"count\")\n";

            cmd += "TrigA = smua.trigger\n";

            cmd += "TrigA_Src = smua.trigger.source\n";

            cmd += "TrigA_Msrt = smua.trigger.measure\n";

            //---------------------------------------------------------------------------------------------------------------------------

            if (this._isDualSMU)
            {
                cmd += "setBLimitV = makesetter(smub.source, \"limitv\")\n";

                cmd += "setBLimitI = makesetter(smub.source, \"limiti\")\n";

                cmd += "setBSorceRangeV = makesetter(smub.source, \"rangev\")\n";

                cmd += "setBSorceRangeI = makesetter(smub.source, \"rangei\")\n";

                cmd += "setBLevelV = makesetter(smub.source, \"levelv\")\n";

                cmd += "setBLevelI = makesetter(smub.source, \"leveli\")\n";

                cmd += "getBFunc = makegetter(smub.source, \"func\")\n";

                cmd += "setBFunc = makesetter(smub.source, \"func\")\n";

                cmd += "setBMeasureRangeV = makesetter(smub.measure, \"rangev\")\n";

                cmd += "setBMeasureRangeI = makesetter(smub.measure, \"rangei\")\n";

                cmd += "setBMeasureDelay = makesetter(smub.measure, \"delay\")\n";

                cmd += "mrtB = smub.measure\n";

                cmd += "mrtIsyncB = smub.measure.overlappedi\n";

                cmd += "mrtVsyncB = smub.measure.overlappedv\n";

                cmd += "mrtIVsyncB = smub.measure.overlappediv\n";

                cmd += "setBNPLC = makesetter(smub.measure, \"nplc\")\n";

                cmd += "setBMsrtCount = makesetter(smub.measure, \"count\")";

                cmd += "setBMsrtInterval = makesetter(smub.measure, \"interval\")\n";

                cmd += "setBBufTimestamps = makesetter(smub.nvbuffer1, \"collecttimestamps\")\n";

                cmd += "bufB1 = smub.nvbuffer1\n";

                cmd += "bufB2 = smub.nvbuffer2\n";

                cmd += "setBOutput = makesetter(smub.source, \"output\")\n";

                cmd += "getBOutput = makegetter(smub.source, \"output\")\n";

                //---------------------------------------------------------------------------------------------------------------------------
                // smub setter
                cmd += "TrigB_LimitV = makesetter(smub.trigger.source, \"limitv\")\n";

                cmd += "TrigB_LimitI = makesetter(smub.trigger.source, \"limiti\")\n";

                cmd += "TrigB_Action_Src = makesetter(smub.trigger.source, \"action\")\n";

                cmd += "TrigB_Action_Msrt = makesetter(smub.trigger.measure, \"action\")\n";

                cmd += "TrigB_Action_EndPulse = makesetter(smub.trigger.endpulse, \"action\")\n";

                cmd += "TrigB_Action_EndSweep = makesetter(smub.trigger.endsweep, \"action\")\n";

                cmd += "TrigB_Stimulus_Arm = makesetter(smub.trigger.arm, \"stimulus\")\n";

                cmd += "TrigB_Stimulus_Src = makesetter(smub.trigger.source, \"stimulus\")\n";

                cmd += "TrigB_Stimulus_Msrt = makesetter(smub.trigger.measure, \"stimulus\")\n";

                cmd += "TrigB_Stimulus_EndPulse = makesetter(smub.trigger.endpulse, \"stimulus\")\n";

                cmd += "TrigB_Count = makesetter(smub.trigger, \"count\")\n";

                cmd += "TrigB = smub.trigger\n";

                cmd += "TrigB_Src = smub.trigger.source\n";

                cmd += "TrigB_Msrt = smub.trigger.measure\n";
            }

            this._conn.SendCommand(cmd);
        }

        private void SetMasterConfigScript()
        {
            string cmd = string.Empty;

            // turn the beeper off
            cmd += "beeper.enable = 0\n";

            cmd += "display.clear()\n";

            cmd += "display.setcursor(1, 3)\n";

            cmd += "display.settext(\"MPI CORPORATION\")\n";

            cmd += "display.setcursor(2, 4)\n";

            cmd += "display.settext(\"" + this._lanSetting.IPAddress + "\")\n";

            // Disable automatic display of errors - leave  error messages in queue and enable  Error Prompt.
            cmd += "localnode.showerrors = 0\n";

            cmd += "localnode.prompts = 0\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // Autozero disabled: 0 = disabled, 1 = once, 2 = enabled
            cmd += "smua.measure.autozero = 0\n";

            // Mode: [0] 2-wire; [1] 4-wire; [3] calibration sense mode
            if (this._elecDevSetting.SrcSensingMode == ESrcSensingMode._2wire)
            {
                cmd += "smua.sense = " + ((int)ESenseMode.LOCAL).ToString() + "\n";   // 2-wire, 0
            }
            else
            {
                cmd += "smua.sense = " + ((int)ESenseMode.REMOTE).ToString() + "\n";  // 4-wire, 1
            }

            // Disables the filter
            cmd += "smua.measure.filter.enable = 0\n";

            cmd += "smua.source.settling = " + this._srcSettling.ToString() + "\n";

            cmd += "smua.measure.autorangev = 0\n";

            cmd += "smua.measure.autorangei = 0\n";

            cmd += "smua.source.autorangei = 0\n";

            cmd += "smua.source.autorangev = 0\n";

            cmd += "smua.source.delay = 0\n";

            cmd += "smua.measure.delay = 0\n";

            cmd += "smua.source.offmode	= 0\n";

            cmd += "format.data = 1\n";

            cmd += "format.asciiprecision = 6\n";

            cmd += "smua.source.levelv = 0\n";

            cmd += "smua.source.leveli = 0\n";

            cmd += "smua.source.rangei = 0.01\n";

            cmd += "smua.source.rangev = 0.01\n";

            cmd += "smua.source.limitv = 10\n";

            if (this._isDualSMU)
            {
                // Autozero disabled: 0 = disabled, 1 = once, 2 = enabled
                cmd += "smub.measure.autozero = 0\n";

                // Mode: [0] 2-wire; [1] 4-wire; [3] calibration sense mode
                if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB)
                {
                    cmd += "smub.sense = " + ((int)ESenseMode.LOCAL).ToString() + "\n";  // PD量測 : 2-wire
                }
                else
                {
                    cmd += "smub.sense = " + ((int)ESenseMode.REMOTE).ToString() + "\n";  // Default: 4-wire
                }

                // Disables the filter
                cmd += "smub.measure.filter.enable = 0\n";

                cmd += "smub.source.settling = " + this._srcSettling.ToString() + "\n";

                cmd += "smub.measure.autorangev = 0\n";

                cmd += "smub.measure.autorangei = 0\n";

                cmd += "smub.source.autorangei = 0\n";

                cmd += "smub.source.autorangev = 0\n";

                cmd += "smub.source.delay = 0\n";

                cmd += "smub.measure.delay = 0\n";

                cmd += "smub.source.offmode	= 0\n";

                cmd += "smub.source.levelv = 0\n";

                cmd += "smub.source.leveli = 0\n";

                cmd += "smub.source.rangei = 0.01\n";

                cmd += "smub.source.rangev = 0.01\n";

                cmd += "smub.source.limitv = 10\n";
            }

            //--------------------------------------------------------------------------------------------------------------------------


            cmd += SetDefaultIOScript();

            // Roy H/W Trig
            //double trigPw = 0.0001;

            //cmd += SetDefaultIOScript((int)PIN_SMU_TRIG_IN, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, trigPw);

            //cmd += SetDefaultIOScript((int)PIN_SMU_TRIG_OUT, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, trigPw);

            //cmd += SetDefaultIOScript((int)PIN_SMU_ABORT_IN, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, 0.0005);

            //cmd += SetDefaultIOScript((int)PIN_SMU_ABORT_OUT, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, 0.0005);
            // cmd += "digio.writebit(" + PIN_SMU_TRIG_IN.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].pulsewidth = " + trigPw.ToString() + "\n";

          //  cmd += "digio.writebit(" + PIN_SMU_TRIG_OUT.ToString() + ", 0)\n";
            
            //cmd += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].pulsewidth = " + trigPw.ToString() + "\n";

            //cmd += "digio.writebit(" + PIN_SMU_ABORT.ToString() + ", 0)\n";            

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_IN.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_IN.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_IN.ToString() + "].pulsewidth = 0.0005\n";            

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_OUT.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_OUT.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_OUT.ToString() + "].pulsewidth = 0.0005\n";

            //---------------------------------------------------------------------------------------------------------------------------
            //Active hight

            //cmd += SetDefaultIOScript((int)PIN_DAQ_ENABLE, EIOTrig_Mode.TRIG_RISING, EIOState.NONE, _ioPulseWidth);

            //cmd += SetDefaultIOScript((int)PIN_RTH_EANBLE, EIOTrig_Mode.TRIG_RISING, EIOState.NONE, _ioPulseWidth);

            //cmd += SetDefaultIOScript((int)PIN_DAQ_TRIG_OUT, EIOTrig_Mode.TRIG_RISING, EIOState.NONE, _ioPulseWidth);

            //cmd += SetDefaultIOScript((int)PIN_POLAR_SW, EIOTrig_Mode.TRIG_RISING, EIOState.NONE, 0.0001);

            //cmd += "digio.writebit(" + PIN_DAQ_ENABLE.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + PIN_DAQ_ENABLE.ToString() + "].pulsewidth = " + _ioPulseWidth.ToString() + "\n";

            //cmd += "digio.trigger[" + PIN_DAQ_ENABLE.ToString() + "].mode = digio.TRIG_RISING\n";

            //cmd += "digio.writebit(" + PIN_RTH_EANBLE.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + PIN_RTH_EANBLE.ToString() + "].pulsewidth = " + _ioPulseWidth.ToString() + "\n";

            //cmd += "digio.trigger[" + PIN_RTH_EANBLE.ToString() + "].mode = digio.TRIG_RISING\n";

            //cmd += "digio.writebit(" + PIN_DAQ_TRIG_OUT.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + PIN_DAQ_TRIG_OUT.ToString() + "].pulsewidth = " + _ioPulseWidth.ToString() + "\n";

            //cmd += "digio.trigger[" + PIN_DAQ_TRIG_OUT.ToString() + "].mode = digio.TRIG_RISING\n";

            //cmd += "digio.writebit(" + PIN_POLAR_SW.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + PIN_POLAR_SW.ToString() + "].pulsewidth = 0.0001\n";

            //cmd += "digio.trigger[" + PIN_POLAR_SW.ToString() + "].mode = digio.TRIG_RISING\n";

            //---------------------------------------------------------------------------------------------------------------------------

            this._conn.SendCommand(cmd);
        }

        private void SetSlavePDHwTrigConfigScript()
        {
            string cmd = string.Empty;

            // turn the beeper off
            cmd += "beeper.enable = 0\n";

            cmd += "display.clear()\n";

            cmd += "display.setcursor(1, 3)\n";

            cmd += "display.settext(\"SLAVE - LISTENING\")\n";

            cmd += "display.setcursor(2, 4)\n";

            cmd += "display.settext(\"" + this._lanSetting.IPAddress + "\")\n";

            // Disable automatic display of errors - leave  error messages in queue and enable  Error Prompt.
            cmd += "localnode.showerrors = 0\n";

            cmd += "localnode.prompts = 0\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // Autozero disabled: 0 = disabled, 1 = once, 2 = enabled
            cmd += "smua.measure.autozero = 0\n";

            // Mode: [0] 2-wire; [1] 4-wire; [3] calibration sense mode
            if (this._elecDevSetting.SrcSensingMode == ESrcSensingMode._2wire)
            {
                cmd += "smua.sense = " + ((int)ESenseMode.LOCAL).ToString() + "\n";
            }
            else
            {
                cmd += "smua.sense = " + ((int)ESenseMode.REMOTE).ToString() + "\n";
            }

            // Disables the filter
            cmd += "smua.measure.filter.enable = 0\n";

            cmd += "smua.source.settling = " + this._srcSettling.ToString() + "\n";

            cmd += "smua.measure.autorangev = 0\n";

            cmd += "smua.measure.autorangei = 0\n";

            cmd += "smua.source.autorangei = 0\n";

            cmd += "smua.source.autorangev = 0\n";

            cmd += "smua.source.delay = 0\n";

            cmd += "smua.measure.delay = 0\n";

            cmd += "smua.source.offmode	= 0\n";

            cmd += "format.data = 1\n";

            cmd += "format.asciiprecision = 6\n";

            cmd += "smua.source.levelv = 0\n";

            cmd += "smua.source.leveli = 0\n";

            cmd += "smua.source.rangei = 0.01\n";

            cmd += "smua.source.rangev = 2\n";

            cmd += "smua.source.limitv = 10\n";

            //---------------------------------------------------------------------------------------------------------------------------
            //SetDefaultIOScript();
            //foreach (IOData ioData in _elecDevSetting.IOSetting.IOList)
            //{
            //    cmd += SetDefaultIOScript(ioData.PinNum, EIOTrig_Mode.TRIG_BYPASS, EIOState.LOW, 0);
            //}
            //// Roy H/W Trig (Active High)

            double trigPw = 0.0001;//由於Slave理論上不會進行IO控制，因此直接寫死

            cmd += SetDefaultIOScript((int)PIN_SMU_TRIG_IN, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, trigPw);

            cmd += SetDefaultIOScript((int)PIN_SMU_TRIG_OUT, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, trigPw);

            cmd += SetDefaultIOScript((int)PIN_SMU_ABORT_IN, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, 0.0005);

            cmd += SetDefaultIOScript((int)PIN_SMU_SYNC, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, 0.0005);

            // cmd += "digio.writebit(" + PIN_SMU_TRIG_IN.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].pulsewidth = " + trigPw.ToString() + "\n";

            ////  cmd += "digio.writebit(" + PIN_SMU_TRIG_OUT.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].pulsewidth = " + trigPw.ToString() + "\n";

            ////  cmd += "digio.writebit(" + PIN_SMU_ABORT_IN.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_IN.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_IN.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_IN.ToString() + "].pulsewidth = 0.0005\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_OUT.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_OUT.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + PIN_SMU_ABORT_OUT.ToString() + "].pulsewidth = 0.0005\n";
            //---------------------------------------------------------------------------------------------------------------------------

            this._conn.SendCommand(cmd);
        }

        private void SetFunctionToDevice()
        {
            string func = string.Empty;

            #region >>> FVMI Func <<<

            func = "function funcFVMI(waitTime, srcValue, srcTime, msrtRange, nplc) ";

            func += "setNPLC(nplc) ";

            func += "if getFunc() ~= 1 then setFunc(1) end ";  // [0] I Source;  [1] V Source

            func += "setMeasureRangeI(msrtRange) ";

            func += "if getOutput() ~= 1 then setOutput(1) end ";

            func += "setLimitI(msrtRange) ";

            func += "setSorceRangeV(srcValue) ";

            func += "delay(waitTime) ";

            func += "setLevelV(srcValue) ";

            func += "delay(srcTime) ";

            func += "print(mrtA.i()) ";

            func += "setLevelV(0) ";

            func += "end";

            this._conn.SendCommand(func);


            #endregion

            #region >>> FI Func <<<

            func = "function funcFI(waitTime, srcValue, srcTime, msrtRange, nplc) ";

            func += "setNPLC(nplc) ";

            func += "if getFunc() ~= 0 then setFunc(0) end ";  // [0] I Source;  [1] V Source

            func += "setMeasureRangeV(msrtRange) ";

            func += "if getOutput() ~= 1 then setOutput(1) end ";

            func += "setLimitV(msrtRange) ";

            func += "setSorceRangeI(srcValue) ";

            func += "delay(waitTime) ";

            func += "setLevelI(srcValue) ";

            func += "delay(srcTime) ";

            //func += "print(mrtA.i()) ";

            //func += "setLevelV(0) ";

            func += "end";

            this._conn.SendCommand(func);


            #endregion

            #region >>> FIMV Func <<<

            func = "function funcFIMV(waitTime, srcValue, srcTime, msrtRange, nplc, isTurnOff) ";

            func += "setNPLC(nplc) ";

            func += "if getFunc() ~= 0 then setFunc(0) end ";  // [0] I Source;  [1] V Source

            func += "setMeasureRangeV(msrtRange) ";

            func += "if getOutput() ~= 1 then setOutput(1) end ";

            func += "setLimitV(msrtRange) ";

            func += "setSorceRangeI(srcValue) ";

            func += "delay(waitTime) ";

            func += "setLevelI(srcValue) ";

            func += "delay(srcTime) ";

            func += "print(mrtA.v()) ";

            func += "if isTurnOff ~= 0 then setLevelI(0) end ";

            func += "end";

            this._conn.SendCommand(func);


            #endregion

            #region >>> PulseI MsrtV Func <<<

            // Set Timer
            func = "function funcSetTimer(pulseWidth, period, pulseCnt) ";

            func += "T1_Count(pulseCnt) ";
            func += "T1_Delay(period) ";
            func += "T1_Passthrough(true) ";
            func += "T1_Stimulus(TrigA.ARMED_EVENT_ID) ";

            func += "T2_Count(1) ";
            func += "T2_Delay(pulseWidth - (1/localnode.linefreq)*0.003 - 60e-6) ";
            func += "T2_Passthrough(false) ";
            func += "T2_Stimulus(Trig_T1.EVENT_ID) ";

            func += "T3_Count(1) ";
            func += "T3_Delay(pulseWidth) ";
            func += "T3_Passthrough(false) ";
            func += "T3_Stimulus(Trig_T1.EVENT_ID) ";

            func += "end";

            this._conn.SendCommand(func);


            // Set Trigger / Arm, Pulse

            func = "function funcPulseIMsrtV(pulseLevel, pulseWidth, period, pulseCnt, msrtRange) ";

            func += "if getFunc() ~= 0 then setFunc(0) end ";

            func += "setSorceRangeI(pulseLevel) ";
            //func += "setLevelI(0) ";
            //func += "setLimitV(1) ";

            func += "setMeasureRangeV(msrtRange) ";
            func += "setNPLC(0.003) ";

            func += "funcSetTimer(pulseWidth, period, pulseCnt) ";

            func += "TrigA_Src.lineari(pulseLevel , pulseLevel, pulseCnt) ";
            func += "TrigA_LimitV(msrtRange) ";
            func += "TrigA_Action_Msrt(smua.ENABLE) ";
            func += "TrigA_Msrt.v(bufA1) ";
            func += "TrigA_Action_EndPulse(smua.SOURCE_IDLE) ";
            func += "TrigA_Count(pulseCnt) ";
            func += "TrigA_Stimulus_Arm(0) ";

            func += "TrigA_Stimulus_Src(Trig_T1.EVENT_ID) ";   // control period (Ton + Toff); stimulus by Arm
            func += "TrigA_Stimulus_Msrt(Trig_T2.EVENT_ID) ";  // control Measure timeing;     stimulus by timer[1]
            func += "TrigA_Stimulus_EndPulse(Trig_T3.EVENT_ID) "; // control Ton;                 stimulus by timer[1]

            func += "TrigA_Action_Src(smua.ENABLE) ";

            func += "if getOutput() ~= 1 then setOutput(1) end ";

            func += "TrigA.initiate() ";

            func += "waitcomplete() ";
            func += "setOutput(0) ";

            func += "printbuffer(1, bufA1.n, bufA1) ";
            func += "Trig_T1.reset() ";
            func += "Trig_T2.reset() ";
            func += "Trig_T3.reset() ";
            func += "bufA1.clear() ";

            func += "if pulseLevel >= 0.1 then setSorceRangeI(0.01) end ";

            func += "end";

            this._conn.SendCommand(func);

            #endregion
        }

        private bool GetErrorMsg()
        {
            string cmd = string.Empty;

            cmd += "errorCode, message, severity, errorNode = errorqueue.next()\n";

            cmd += "print(message)";

            this._conn.SendCommand(cmd);

            string msg = string.Empty;

            this._conn.WaitAndGetData(out msg);

            if (msg == "Queue Is Empty\n")
            {
                return false;
            }
				
            Console.WriteLine("[K2600Wrapper], GetErrorMsg()," + msg);

            return true;
        }

        private void SetDCMsrtItemScript(uint index, ElectSettingData item)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double complLimit = 0.0d;

            // [0] I Source;  [1] V Source
            int srcMode;

            if (item.MsrtType == EMsrtType.FIMV || item.MsrtType == EMsrtType.POLAR || item.MsrtType == EMsrtType.LIV || item.MsrtType == EMsrtType.FI || item.MsrtType == EMsrtType.FIMVLOP)
            {
                srcFunc = "i";

                msrtFunc = "v";

                srcMode = 0;

                complLimit = this._voltRange[0][this._voltRange[0].Length - 1];
            }
            else
            {
                srcFunc = "v";

                msrtFunc = "i";

                srcMode = 1;

				complLimit = this._currRange[1][this._currRange[1].Length - 1];
            }

			double forceRange = item.ForceRange;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFunc == "i")
            {
                if (forceRange < 0.000001)
                {
                    forceRange = 0.000001;
                }
            }
            else
            {
                if (forceRange < 2.0d)
                {
                    forceRange = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================

            if (item.MsrtNPLC == 0.01d && item.MsrtProtection < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            // IZ Clamp > 50.1，使用100uA檔位進行測試

            if (item.KeyName.Contains("IZ"))
            {
                if (item.MsrtProtection > 50.1)
                {
                    if (forceRange < 0.0001)
                    {
                        forceRange = 0.0001;
                    }
                }

                if (this._elecDevSetting.IsSettingReverseCurrentRange)
                {
                    double applyforceRange = this._elecDevSetting.ReverseCurrentApplyRange * 0.001;

                    forceRange = applyforceRange;
                }
            }


            if (item.MsrtProtection <= complLimit)
            {
                if (msrtFunc == "v")
                {
					srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

					srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";

					srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

					srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + index.ToString() + "\n";

            script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";

                //script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {

                script += "setMeasureRangeI(" + item.MsrtRange.ToString() + ")\n";

                //script += "setMeasureRangeV(" + item.ForceRange.ToString() + ")\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            //======================================
            // 切完Range後，在Set Range & Compliance
            //=======================================
            if (item.ForceDelayTime > 0)
            {
                script += "delay(" + item.ForceDelayTime.ToString() + ")\n";
            }

            if (srcFunc == "v")
            {
                script += "setLevelV(" + item.ForceValue.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + item.ForceValue.ToString() + ")\n";
            }

            script += "delay(" + item.ForceTime.ToString() + ")\n";


            if (item.IsTrigCamera)
            {
                script += IOAssert(PIN_CAMERA_TRIG_OUT );
                //"digio.trigger[" + PIN_CAMERA_TRIG_OUT.ToString() + "].assert()\n";
            }

            //======================================
            // 回傳量測值
            //=======================================
            if (item.MsrtType != EMsrtType.FI && item.MsrtType != EMsrtType.FV && item.MsrtType != EMsrtType.RTH)
            {
                if (item.IsEnableMsrtForceValue)
                {
                    script += "mrtIVsyncA(bufA1, bufA2)\n";  // overlappediv

                    script += "waitcomplete()\n";

                    script += "printbuffer(1, 1, bufA1, bufA2)\n";  // print i and v
                }
                else
                {
                    script += "print(mrtA." + msrtFunc + "())\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
			if (item.MsrtProtection > 20 && srcFunc == "i" && item.IsAutoTurnOff)
            {
                script += "setLimitV(8)\n";
            }

            if (item.IsAutoTurnOff)
            {
                if (this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd ||
                    this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.EOT)
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        if (this._elecDevSetting.TurnOffRangeIBackToDefault)
                        {
                            script += "setSorceRangeI(0.01)\n";
                        }
                        else
                        {
                            if (forceRange >= 0.1)
                            {
                                // 防止切換 1A Range 時, 產生 Overshoot
                                script += "setSorceRangeI(0.01)\n";
                            }
                        }
                    }
                }
                else  // ESrcTurnOffType.EachItem ==> FI=0,FV=0
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";
                       // script += "setOutput(0)\n";
                       // 

                        if (this._elecDevSetting.TurnOffRangeIBackToDefault)
                        {
                            script += "setSorceRangeI(0.01)\n";
                        }
                        else
                        {
                            if (forceRange >= 0.1)
                            {
                                // 防止切換 1A Range 時, 產生 Overshoot
                                script += "setSorceRangeI(0.01)\n";
                            }
                        }
                    }
                }
            }

            if (item.IsEnableMsrtForceValue)
            {
                // Clear Buffer
                script += "bufA1.clear()\n";
                script += "bufA2.clear()\n";
            }


            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            this._conn.SendCommand(script);
        }

		private void SetRMsrtItemScript(uint index, ElectSettingData item)
		{
			string script = string.Empty;

			string srcRngAndComplScript = "";

			double complLimit = this._voltRange[0][this._voltRange[0].Length - 1];

			//-----------------------------------------------------------------------------------------
			// Force Range and Compliance Setting
			//-----------------------------------------------------------------------------------------
			if (item.MsrtProtection <= complLimit)
			{
				srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

				srcRngAndComplScript += "setSorceRangeI(" + item.ForceRange.ToString() + ")\n";
			}
			else
			{
				if (Math.Abs(item.ForceRange) < 0.000001)
				{
					srcRngAndComplScript += "setSorceRangeI(0.000001)\n";
				}
				else
				{
					srcRngAndComplScript += "setSorceRangeI(" + item.ForceRange.ToString() + ")\n";
				}

				srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
			}

			//-----------------------------------------------------------------------------------------
			// Test Sequence
			//-----------------------------------------------------------------------------------------
			script = "loadscript num_" + index.ToString() + "\n";

			script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";

			script += "if getFunc() ~= 0 then setFunc(0) end\n";

			script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";

			script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";

			script += "if getOutput() ~= 1 then setOutput(1) end\n";

			script += srcRngAndComplScript;

			if (item.ForceDelayTime > 0)
			{
				script += "delay(" + item.ForceDelayTime.ToString() + ")" + "\n";
			}

			script += "setLevelI(" + item.ForceValue.ToString() + ")\n";

			script += "delay(" + item.ForceTime.ToString() + ")" + "\n";

			script += "print(mrtA.v())\n";

			//-----------------------------------------------------------------------------------------
			// 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
			//-----------------------------------------------------------------------------------------
			if (Math.Abs(item.MsrtProtection) > 20)
			{
				script += "setLimitV(8)\n";
			}

			if (item.IsAutoTurnOff)
			{
				if (this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd ||
					this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.EOT)
				{
					script += "setLevelI(0)\n";
				}
				else
				{
					script += "setOutput(0)\n";
				}
			}

			script += "endscript";

			this._conn.SendCommand(script);

		}

		private void SetContactCheckItemScript(uint index, ElectSettingData item)
		{
			string script = "";

			script += "loadscript num_" + index.ToString() + "\n";

			script += "if getFunc() ~= 0 then setFunc(0) end\n";

			script += "setSorceRangeI(0.001)\n";

			// 0: smuX.CONTACT_FAST, 1: smuX.CONTACT_MEDIUM, 2: smuX.CONTACT_SLOW
			if (item.ConatctCheckSpeed == EContactCheckSpeed.SLOW)
			{
				script += "smua.contact.speed = 2\n"; 
			}
			else if (item.ConatctCheckSpeed == EContactCheckSpeed.FAST)
			{
				script += "smua.contact.speed = 0\n"; 
			}
			else
			{
				script += "smua.contact.speed = 1\n"; 
			}

			script += "rhi, rlo = smua.contact.r()\n";

			script += "local Outp = string.format(\"%.3f, %.3f\", rhi, rlo)\n";

			script += "print(Outp)\n";

			script += "endscript\n";

			script += "num_" + index.ToString() + ".source = nil";

			this._conn.SendCommand(script);
		}

        private void SetSweepMsrtItemScript(uint index, ElectSettingData item)
        {
            string script = string.Empty;
            string srcRngAndComplScript = string.Empty;
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            double complLimit = 0.0d;
            int srcMode;     // [0] I Source;  [1] V Source

            string listName = string.Format("List_{0}", index);

            if (item.MsrtType == EMsrtType.FIMVSWEEP)
            {
                srcFunc = "i";
                msrtFunc = "v";
                srcMode = 0;
                complLimit = this._voltRange[0][this._voltRange[0].Length - 1];
            }
            else
            {
                srcFunc = "v";
                msrtFunc = "i";
                srcMode = 1;
                complLimit = this._currRange[1][this._currRange[1].Length - 1];
            }

            int sweepCnt = this._sweepList.Count;

            //-----------------------------------------------------------------------------------------
            // Sweep Sequence
            //-----------------------------------------------------------------------------------------
            script += "loadscript num_" + index.ToString() + "\n"         // set the script name for the i-th parameter: num_i; i starts from 0
                    + "smua.source.func = " + srcMode.ToString() + "\n"
                    + "smua.source.level" + srcFunc + " = 0\n";            // set the source level in the idle status

            //script += "smua.measure.lowrange" + msrtFunc + " = 10e-6\n";

            // Force Range and Compliance Setting
            if (item.MsrtProtection <= complLimit)
            {
                srcRngAndComplScript += "smua.source.limit" + msrtFunc + " = " + item.MsrtProtection.ToString() + "\n"        // set the normal compliance
                                      + "smua.source.range" + srcFunc + " = " + item.ForceRange.ToString() + "\n";            // Selects the range for the specified source
            }
            else
            {
                srcRngAndComplScript += "smua.source.range" + srcFunc + " = " + item.ForceRange.ToString() + "\n"              // Selects the range for the specified source
                                      + "smua.source.limit" + msrtFunc + " = " + item.MsrtProtection.ToString() + "\n";        // set the normal compliance
            }

            if (item.IsSweepAutoMsrtRange)
            {
                script += "smua.measure.autorangei = smua.AUTORANGE_ON\n";
                script += "smua.measure.autorangev = smua.AUTORANGE_ON\n";
            }
            else
            {
                if (msrtFunc == "v")
                {
                    script += "setMeasureRangeV(" + item.MsrtProtection.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    script += "setMeasureRangeI(" + item.MsrtProtection.ToString() + ")\n";
                }
            }

            script += srcRngAndComplScript
                    + "smua.trigger.source.limit" + msrtFunc + " = smua.LIMIT_AUTO\n"
                    + "smua.trigger.measure." + msrtFunc + "(smua.nvbuffer1)\n"
                    + "smua.trigger.measure.action = smua.ENABLE\n"
                    + "smua.measure.count = 1\n";

            script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";



            if (item.SweepTurnOffTime == 0.0d)
            {
                // Continous DC Sweep
                script += "smua.trigger.arm.stimulus = 0\n" // Reset trigger model stimuli
                       + "smua.trigger.source.stimulus = 0\n"
                       + "smua.trigger.source.list" + srcFunc + "(" + listName + ")\n" // configure the source action
                       + "smua.trigger.source.action = smua.ENABLE\n"
                       + "smua.trigger.endpulse.action = smua.SOURCE_HOLD\n" // configure the end pluse action 
                       + "trigger.timer[1].reset()\n"                        // configure the timer triggering
                       + "trigger.timer[1].stimulus = smua.trigger.SOURCE_COMPLETE_EVENT_ID\n"
                       + "trigger.timer[1].delay = " + item.ForceTime.ToString() + "\n"
                       + "trigger.timer[1].count = 1\n"
                       + "smua.trigger.measure.stimulus = trigger.timer[1].EVENT_ID\n"
                       + "smua.trigger.endpulse.stimulus = smua.trigger.MEASURE_COMPLETE_EVENT_ID\n"
                       + "smua.trigger.arm.count = 1\n"                      // configure the trigger count
                       + "smua.trigger.count = " + sweepCnt.ToString() + "\n";
            }
            else
            {
                // Pulsed DC Sweep
                script += "smua.trigger.arm.stimulus = 0\n"                                         // Reset trigger model stimulus         
                       + "smua.trigger.source.list" + srcFunc + "(" + listName + ")\n" // configure the source action
                       + "smua.trigger.source.action = smua.ENABLE\n"
                       + "smua.trigger.endpulse.action = smua.SOURCE_IDLE\n" // configure the end pulse action
                       + "trigger.timer[1].reset()\n"                        // timer[1] is for the pulse output and the measurement
                       + "trigger.timer[1].stimulus = smua.trigger.SOURCE_COMPLETE_EVENT_ID\n"
                       + "trigger.timer[1].delay = " + item.ForceTime.ToString() + "\n"
                       + "trigger.timer[1].count = 1\n"
                       + "smua.trigger.measure.stimulus = trigger.timer[1].EVENT_ID\n"
                       + "smua.trigger.endpulse.stimulus = smua.trigger.MEASURE_COMPLETE_EVENT_ID\n"
                       + "trigger.timer[2].reset()\n"
                       + "trigger.timer[2].delay = " + item.SweepTurnOffTime.ToString() + "\n"
                       + "trigger.timer[2].count = 1\n"
                       + "trigger.timer[2].stimulus = smua.trigger.PULSE_COMPLETE_EVENT_ID\n"
                       + "smua.trigger.source.stimulus = trigger.timer[2].EVENT_ID\n"         // the timer[2] is for stimulating the pulses after the 1st pulse and defining the interval between two consecutive pulses
                       + "smua.trigger.arm.count = 1\n"                                       // configure the trigger count
                       + "smua.trigger.count = " + sweepCnt.ToString() + "\n";
            }

            //------------------------ Hold Time --------------------------------------
            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            if (item.ForceDelayTime > 0)  // waiting time 
            {
                script += "delay(" + item.ForceDelayTime.ToString() + ")\n";
            }

            //if (item.SweepHoldTime > 0.0d)
            //{
            //    script += "smua.source.level" + srcFunc + " = " + listName + "[1]\n";
            //    script += "delay(" + item.SweepHoldTime.ToString() + ")\n";
            //}

            //-------------------------------------------------------------------------
            script += "smua.trigger.initiate()\n";  // range 都已經設定完成
            script += "smua.trigger.source.set()\n";
            script += "waitcomplete()\n";

            script += "setOutput(0)\n";
            // disable the measurement autorage after the sweep operation
            script += "smua.measure.autorangei = smua.AUTORANGE_OFF\n";
            script += "smua.measure.autorangev = smua.AUTORANGE_OFF\n";


            script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1)\n";
            script += "smua.nvbuffer1.clear()\n";     // clear the reading buffers

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            this._conn.SendCommand(script);
        }

        private void SetTHYItemAndMsrtScript(uint index, ElectSettingData item)
        {
            item.SweepContCount = (uint)Math.Round((double)item.SweepContCount / 100.0d) * 100;

            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double complLimit = 0.0d;

            // [0] I Source;  [1] V Source
            int srcMode;

            if (item.MsrtType == EMsrtType.THY)
            {
                srcFunc = "i";

                msrtFunc = "v";

                srcMode = 0;

                complLimit = this._voltRange[0][this._voltRange[0].Length - 1];
            }
            else
            {
                // For THY02
                srcFunc = "v";

                msrtFunc = "i";

                srcMode = 1;

                complLimit = this._currRange[1][this._voltRange[1].Length - 1];

                return;
            }

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (item.MsrtProtection <= complLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + item.ForceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + item.ForceRange.ToString() + ")\n";
                }
            }
            else
            {
                double forceRange = item.ForceRange;

                if (msrtFunc == "v")
                {
					if (Math.Abs(item.ForceRange) < 0.000001)
					{
						forceRange = 0.000001;
					}

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";
                }
            }

            script = "loadscript num_" + index.ToString() + "\n";

            script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";

            uint measureCount = (item.SweepContCount) / 2;

			if (measureCount < 100)
			{
				measureCount = 100;
			}

            script += "setMsrtCount(" + measureCount.ToString() + ")\n";

            script += "setMsrtInterval(0.000001)\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";

                script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + item.MsrtRange.ToString() + ")\n";

                script += "setMeasureRangeV(" + item.ForceRange.ToString() + ")\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (item.ForceDelayTime > 0)
            {
                script += "delay(" + item.ForceDelayTime.ToString() + ")" + "\n";
            }

            if (srcFunc == "v")
            {
                script += "setLevelV(" + item.ForceValue.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + item.ForceValue.ToString() + ")\n";
            }

            script += "mrtA." + msrtFunc + "(bufA1)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(item.MsrtProtection) > 20 && srcFunc == "i")
            {
                script += "setLimitV(8)\n";
            }

            if (item.IsAutoTurnOff)
            {
                if (this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.EOT && !item.IsNextIsESDTestItem)
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";
                    }
                }
                else
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";
                    }
                }
            }

            if (this._elecDevSetting.IsDevicePeakFiltering)
            {
                script += "local maxPeak = 0\n";

                script += "local stableSum = 0\n";

                script += "for i = 1, " + measureCount.ToString() + " do\n";

                script += "if bufA1.readings[i] > maxPeak then\n";

                script += "maxPeak = bufA1.readings[i]\n";

                script += "end\n";

                script += "if " + measureCount.ToString() + " - i < 20 then\n";

                script += "stableSum = stableSum + bufA1.readings[i]\n";

                script += "end\n";

                script += "end\n";

                script += "local stableValue = stableSum / 20\n";

                script += "local maxToStable = maxPeak - stableValue\n";

                script += "local Outp = string.format(\"%.3f, %.3f\", maxPeak, stableValue)\n";

                script += "print(Outp)\n";

                //script += "statistics = smua.buffer.getstats(smua.nvbuffer1)\n";

                //script += "local Outp = string.format(\"%.3f, %.3f\", statistics.max.reading, statistics.mean)\n";

                //script += "print(Outp)\n";
            }
            else
            {
                script += "printbuffer(1, " + measureCount.ToString() + ", bufA1)\n";
            }

            script += "setMsrtCount(1)\n";

            script += "bufA1.clear()\n";

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            this._conn.SendCommand(script);
        }

        private void SetTHYItemScript(uint index, ElectSettingData item)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            double complLimit = this._voltRange[0][this._voltRange[0].Length - 1];

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (item.MsrtProtection <= complLimit)
            {
                srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(" + item.ForceRange.ToString() + ")\n";
            }
            else
            {
                double forceRange = item.ForceRange;

                if (Math.Abs(item.ForceRange) < 0.000001)
                {
                    forceRange = 0.000001;
                }

                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + index.ToString() + "\n";


            //----------------------------------------------
            // 打THY支前，碰一下，維持3ms
            //----------------------------------------------

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += IOWritebit(PIN_FrameGround, EIOState.HIGH);
            //script += "digio.writebit(" + PIN_FrameGround.ToString() + ", 1)\n";

            script += "delay(0.003)\n";

            script += IOWritebit(PIN_FrameGround, EIOState.LOW);
            //script += "digio.writebit(" + PIN_FrameGround.ToString() + ", 0)\n";


            // Enable二極體:		_pinRTHEnable = 0  
            // Bypass 二極體:		_pinRTHEnable = 1
            // P:				_pinDAQEnable = 1, _pinPolarSW = 0
            // N:				_pinDAQEnable = 0, _pinPolarSW = 1
            // Open:			_pinDAQEnable = 0, _pinPolarSW = 0
            //小電容:				_pinCapSW = 0
            //大電容:				_pinCapSW = 1

            // Bypass 二極體
            //script += "digio.writebit(" + _pinRTHEnable.ToString() + ", 1)\n";

            // P極

            script += IOWritebit(PIN_DAQ_ENABLE, EIOState.HIGH);
            script += IOWritebit(PIN_POLAR_SW, EIOState.LOW);
            //script += "digio.writebit(" + PIN_DAQ_ENABLE.ToString() + ", 1)\n";

            //script += "digio.writebit(" + PIN_POLAR_SW.ToString() + ", 0)\n";

            //切換大小電容
            if (item.ForceValue >= 0.00001)
            {
                script += IOWritebit(PIN_CAP_SW, EIOState.HIGH);
                //script += "digio.writebit(" + PIN_CAP_SW.ToString() + ", 1)\n";
            }
            else
            {
                script += IOWritebit(PIN_CAP_SW, EIOState.LOW);
                //script += "digio.writebit(" + PIN_CAP_SW.ToString() + ", 0)\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (item.ForceDelayTime > 0)
            {
                script += "delay(" + item.ForceDelayTime.ToString() + ")" + "\n";
            }

            script += IOAssert(PIN_DAQ_TRIG_OUT);
            //script += "digio.trigger[" + PIN_DAQ_TRIG_OUT.ToString() + "].assert()\n";

            script += "delay(0.0002)\n";

            script += "setLevelI(" + item.ForceValue.ToString() + ")\n";
			
            script += "delay(" + item.ForceTime.ToString() + ")" + "\n";

            script += "print(0)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(item.MsrtProtection) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (item.IsAutoTurnOff)
            {
                if (this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.EOT && !item.IsNextIsESDTestItem)
                {
                    script += "setLevelI(0)\n";
                }
                else
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

			        script += "setLevelV(0)\n";
                }
            }

            //關閉迴路
            //script += "digio.writebit(" + _pinRTHEnable.ToString() + ", 1)\n";
            script += IOWritebit(PIN_DAQ_ENABLE, EIOState.LOW);
            //script += "digio.writebit(" + PIN_DAQ_ENABLE.ToString() + ", 0)\n";

            script += IOWritebit(PIN_POLAR_SW, EIOState.LOW);
            //script += "digio.writebit(" + PIN_POLAR_SW.ToString() + ", 0)\n";

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            this._conn.SendCommand(script);

            this.SetDAQTriggerScritp(index, item);
        }

		private void SetDAQTriggerScritp(uint index, ElectSettingData item)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            double complLimit = this._voltRange[0][this._voltRange[0].Length - 1];

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (item.MsrtProtection <= complLimit)
            {
                srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(" + item.ForceRange.ToString() + ")\n";
            }
            else
            {
                double forceRange = item.ForceRange;

                if (Math.Abs(item.ForceRange) < 0.000001)
                {
                    forceRange = 0.000001;
                }

                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + index.ToString() + "_DAQTrigger\n";


            //----------------------------------------------
            // 打THY支前，碰一下，維持3ms
            //----------------------------------------------

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += IOWritebit(PIN_FrameGround, EIOState.HIGH);

            //script += "digio.writebit(" + PIN_FrameGround .ToString() + ", 1)\n";

            script += "delay(0.003)\n";

            script += IOWritebit(PIN_FrameGround, EIOState.LOW);
            //script += "digio.writebit(" + PIN_FrameGround.ToString() + ", 0)\n";


			// Enable二極體:		_pinRTHEnable = 0  
			// Bypass 二極體:		_pinRTHEnable = 1
			// P:				_pinDAQEnable = 1, _pinPolarSW = 0
			// N:				_pinDAQEnable = 0, _pinPolarSW = 1
			// Open:			_pinDAQEnable = 0, _pinPolarSW = 0
			//小電容:				_pinCapSW = 0
			//大電容:				_pinCapSW = 1

            // Bypass 二極體
            //script += "digio.writebit(" + _pinRTHEnable.ToString() + ", 1)\n";

            // P極
            script += IOWritebit(PIN_DAQ_ENABLE, EIOState.HIGH);
            script += IOWritebit(PIN_POLAR_SW, EIOState.LOW);
            //script += "digio.writebit(" + PIN_DAQ_ENABLE.ToString() + ", 1)\n";

            //script += "digio.writebit(" + PIN_POLAR_SW.ToString() + ", 0)\n";

			//切換大小電容
			if (item.ForceValue >= 0.00001)
			{
                script += IOWritebit(PIN_CAP_SW, EIOState.HIGH);
                //script += "digio.writebit(" + PIN_CAP_SW.ToString() + ", 1)\n";
			}
			else
			{
                script += IOWritebit(PIN_CAP_SW, EIOState.LOW);
                //script += "digio.writebit(" + PIN_CAP_SW.ToString() + ", 0)\n";
			}

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (item.ForceDelayTime > 0)
            {
                script += "delay(" + item.ForceDelayTime.ToString() + ")" + "\n";
            }

            script += "delay(0.0003)\n";

            script += IOAssert(PIN_DAQ_TRIG_OUT);
            //script += "digio.trigger[" + PIN_DAQ_TRIG_OUT.ToString() + "].assert()\n";

            script += "delay(0.0002)\n";

            //script += "setLevelI(" + item.ForceValue.ToString() + ")\n";

            script += "delay(" + item.ForceTime.ToString() + ")" + "\n";

            //script += "print(0)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(item.MsrtProtection) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (item.IsAutoTurnOff)
            {
                if (this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.EOT && !item.IsNextIsESDTestItem)
                {
                    script += "setLevelI(0)\n";
                }
                else
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";
                }
            }

			//關閉迴路
            //script += "digio.writebit(" + _pinRTHEnable.ToString() + ", 1)\n";

            script += IOWritebit(PIN_DAQ_ENABLE, EIOState.LOW);
            script += IOWritebit(PIN_POLAR_SW, EIOState.LOW);
            //script += "digio.writebit(" + PIN_DAQ_ENABLE.ToString() + ", 0)\n";

            //script += "digio.writebit(" + PIN_POLAR_SW.ToString() + ", 0)\n";

            script += "endscript\n";

            script += "num_" + index.ToString() + "_DAQTrigger.source = nil";

            this._conn.SendCommand(script);
        }

		private void SetRTHHeatItemScript(uint index, ElectSettingData item)
		{
			string script = string.Empty; ;

			string srcRngAndComplScript = string.Empty;

            double complLimit = this._voltRange[0][this._voltRange[0].Length - 1];

			//-----------------------------------------------------------------------------------------
			// Force Range and Compliance Setting
			//-----------------------------------------------------------------------------------------
			if (item.MsrtProtection <= complLimit)
			{
                srcRngAndComplScript += "setSorceRangeI(" + (item.ForceRange + Math.Abs(item.RTHIhForceValue)).ToString() + ")\n";
			}
			else
			{
				double forceRange = item.ForceRange + item.RTHIhForceValue;

                if (Math.Abs(forceRange) < 0.000001)
				{
					forceRange = 0.000001;
				}

				srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
			}

			//-----------------------------------------------------------------------------------------
			// Test Sequence
			//-----------------------------------------------------------------------------------------
			script = "loadscript num_" + index.ToString() + "\n";

			script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += "setLevelI(0)\n";

			script += srcRngAndComplScript;

			if (item.ForceDelayTime > 0)
			{
				script += "delay(" + item.ForceDelayTime.ToString() + ")\n";
			}

            if (item.RTHImForceTime > 0)
			{
                script += "delay(" + item.RTHImForceTime.ToString() + ")\n";
			}

            script += "setLevelI(" + item.RTHIhForceValue.ToString() + ")\n";
			
			script += "delay(" + item.RTHIhForceTime.ToString() + ")\n";

			//-----------------------------------------------------------------------------------------
			// 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
			//-----------------------------------------------------------------------------------------
			if (Math.Abs(item.MsrtProtection) > 20)
			{
				script += "setLimitV(8)\n";
			}

			if (item.IsAutoTurnOff)
			{
                script += "setLevelI(0)\n";

                script += "setLevelV(0)\n";    

				script += "setOutput(0)\n";
			}

			script += "endscript\n";

			script += "num_" + index.ToString() + ".source = nil";

			this._conn.SendCommand(script);
		}

        private void SetScanItemAndMsrtScript(uint index, ElectSettingData item)
        {
            bool isDeviceMsrtTimeStamps = true;

            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double complLimit = 0.0d;

            // [0] I Source;  [1] V Source
            int srcMode;

            if (item.MsrtType == EMsrtType.FVMISCAN)
            {
                srcFunc = "v";

                msrtFunc = "i";

                srcMode = 1;

                complLimit = this._currRange[1][this._currRange[1].Length - 1];
            }
            else
            {
                srcFunc = "i";

                msrtFunc = "v";

                srcMode = 0;

                complLimit = this._voltRange[0][this._voltRange[0].Length - 1];
            }

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (item.MsrtProtection <= complLimit)
            {
                double forceRange = item.ForceRange;

                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + item.ForceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    if (Math.Abs(item.ForceRange) < 0.000001)
                    {
                        forceRange = 2.0d;
                    }

                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                double forceRange = item.ForceRange;

                if (msrtFunc == "v")
                {
                    if (Math.Abs(item.ForceRange) < 0.000001)
                    {
                        forceRange = 0.000001;
                    }

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    if (Math.Abs(item.ForceRange) < 0.000001)
                    {
                        forceRange = 2.0d;
                    }

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";
                }
            }

            script = "loadscript num_" + index.ToString() + "\n";

            script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";

            script += "setMsrtCount(" + item.SweepContCount.ToString() + ")\n";

            script += "setMsrtInterval(0.000001)\n";

            if (isDeviceMsrtTimeStamps)
            {
                script += "setBufTimestamps(1)\n";
            }

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";

                script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + item.MsrtRange.ToString() + ")\n";

                double forceRange = item.ForceRange;

                if (Math.Abs(item.ForceRange) < 0.000001)
                {
                    forceRange = 2.0d;
                }

                script += "setMeasureRangeV(" + forceRange.ToString() + ")\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (item.ForceDelayTime > 0)
            {
                script += "delay(" + item.ForceDelayTime.ToString() + ")" + "\n";
            }

            if (srcFunc == "v")
            {
                script += "setLevelV(" + item.ForceValue.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + item.ForceValue.ToString() + ")\n";
            }

            script += "mrtA." + msrtFunc + "(bufA1)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(item.MsrtProtection) > 20 && srcFunc == "i")
            {
                script += "setLimitV(8)\n";
            }

            if (item.IsAutoTurnOff)
            {
                if (this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd ||
                    this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.EOT)
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";
                    }
                }
                else
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";
                    }
                }
            }

            if (isDeviceMsrtTimeStamps)
            {
                script += "printbuffer(1, " + item.SweepContCount.ToString() + ", bufA1, bufA1.timestamps)\n";

                script += "bufA1.clear()\n";

                script += "setBufTimestamps(0)\n";
            }
            else
            {
                script += "printbuffer(1, " + item.SweepContCount.ToString() + ", bufA1)\n";

                script += "bufA1.clear()\n";
            }

            script += "setMsrtCount(1)\n";

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            this._conn.SendCommand(script);

            //this.ExportScriptToTxt(script, "Script_VISCAN");
        }

        private void SetLOPMsrtItemScript(uint index, ElectSettingData item)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double complLimit = 0.0d;

            // [0] I Source;  [1] V Source
            int srcMode;

            if (item.MsrtType == EMsrtType.FIMV || item.MsrtType == EMsrtType.POLAR || item.MsrtType == EMsrtType.LIV || item.MsrtType == EMsrtType.FI || item.MsrtType == EMsrtType.FIMVLOP)
            {
                srcFunc = "i";

                msrtFunc = "v";

                srcMode = 0;

                complLimit = this._voltRange[0][this._voltRange[0].Length - 1];
            }
            else
            {
                srcFunc = "v";

                msrtFunc = "i";

                srcMode = 1;

                complLimit = this._currRange[1][this._currRange[1].Length - 1];
            }

            double forceRange = item.ForceRange;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFunc == "i")
            {
                if (forceRange < 0.000001)
                {
                    forceRange = 0.000001;
                }
            }
            else
            {
                if (forceRange < 2.0d)
                {
                    forceRange = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================

            if (item.MsrtNPLC == 0.01d && item.MsrtProtection < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            if (item.MsrtProtection <= complLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + index.ToString() + "\n";

            if (item.SourceFunction == ESourceFunc.CW)
            {
                script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";
            }
            else
            {
                script += "setNPLC(0.01)\n";
            }

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";

                //script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + item.MsrtRange.ToString() + ")\n";

                //script += "setMeasureRangeV(" + item.ForceRange.ToString() + ")\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB && this._isDualSMU)
            {
                double detectorForceRange = Math.Abs(item.DetectorBiasValue) < 2.0d ? 6.0d : item.DetectorBiasValue;
                
                script += "if getBFunc() ~= 1 then setBFunc(1) end\n";

                if (item.SourceFunction == ESourceFunc.CW)
                {
                    script += "setBNPLC(" + item.DetectorMsrtNPLC.ToString() + ")\n";
                }
                else
                {
                    script += "setBNPLC(0.01)\n";
                }

                script += "setBLimitI(" + item.DetectorMsrtRange.ToString() + ")\n";

                script += "setBSorceRangeV(" + detectorForceRange.ToString() + ")\n";

                script += "setBMeasureRangeI(" + item.DetectorMsrtRange.ToString() + ")\n";

                script += "setBLevelV(" + item.DetectorBiasValue.ToString() + ")\n";

                script += "setBOutput(1)\n";
            }
            else if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd && this._elecDevSetting.IsDetectorHwTrig)
            {
                script +=IOWait(PIN_SMU_SYNC, 0.2);
                //script += "digio.trigger[" + PIN_SMU_SYNC.ToString() + "].wait(0.2)\n";  // Time Out 200ms
            }

            //======================================
            // 切完Range後，在Set Range & Compliance
            //=======================================
            if (item.ForceDelayTime > 0)
            {
                script += "delay(" + item.ForceDelayTime.ToString() + ")\n";
            }

            if (srcFunc == "v")
            {
                script += "setLevelV(" + item.ForceValue.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + item.ForceValue.ToString() + ")\n";
            }

            //======================================
            // 回傳量測值
            //=======================================
            double calcForceTime = item.ForceTime;

            if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB && this._isDualSMU)
            {
                if (item.SourceFunction != ESourceFunc.CW)
            {
                calcForceTime = item.ForceTime - 0.0007d;

                calcForceTime = calcForceTime < 0 ? 0.0d : calcForceTime;
            }

                script += "delay(" + calcForceTime.ToString() + ")\n";

                script += "mrtVsyncA(bufA1)\n";  // overlappedv

                script += "mrtIsyncB(bufB1)\n";  // overlappedi

                script += "waitcomplete()\n";
            }
            else
            {
 				if (item.SourceFunction != ESourceFunc.CW)
                {
                    calcForceTime = Math.Round((item.ForceTime - (1/60.0d) * item.MsrtNPLC - 350e-6), 6, MidpointRounding.AwayFromZero);// item.ForceTime - 0.0007d;

                   // calcForceTime = item.ForceTime - 0.0007d;

                    calcForceTime = calcForceTime < 0 ? 0.0d : calcForceTime;
                }

                script += "delay(" + calcForceTime.ToString() + ")\n";
                
                if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd && this._elecDevSetting.IsDetectorHwTrig)
                {

                    script += IOAssert(PIN_SMU_TRIG_OUT);
                    /*//script += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].assert()\n";

                    //script += "print(mrtA." + msrtFunc + "())\n";

                    //script += "local reading = mrtA." + msrtFunc + "()\n";

                    //script += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].wait(5)\n";  // Time Out 100ms*/
                }
                
                script += "local reading = mrtA." + msrtFunc + "()\n";
                
            }

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (item.MsrtProtection > 20 && srcFunc == "i" && item.IsAutoTurnOff)
            {
                script += "setLimitV(8)\n";
            }

            bool isForceDischarge = true;

            if (item.IsAutoTurnOff)
            {
                if (!isForceDischarge)
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        if (this._elecDevSetting.TurnOffRangeIBackToDefault)
                        {
                            script += "setSorceRangeI(0.01)\n";
                        }
                        else
                        {
                            if (forceRange >= 0.1)
                            {
                                // 防止切換 1A Range 時, 產生 Overshoot
                                script += "setSorceRangeI(0.01)\n";
                            }
                        }
                    }
                }
                else
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";

                        if (this._elecDevSetting.TurnOffRangeIBackToDefault)
                        {
                            script += "setSorceRangeI(0.01)\n";
                        }
                        else
                        {
                            if (forceRange >= 0.1)
                            {
                                // 防止切換 1A Range 時, 產生 Overshoot
                                script += "setSorceRangeI(0.01)\n";
                            }
                        }
                    }
                }
            }

            if (this._isDualSMU)
            {
                script += "setBOutput(0)\n";
                script += "printbuffer(1, bufA1.n, bufA1, bufB1)\n";
                script += "bufA1.clear()\n";
                script += "bufA2.clear()\n";
                script += "bufB1.clear()\n";
            }
            else
            {
                script += "print(reading)\n";
            }

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

           // this.ExportScriptToTxt(script, "LOP");

            this._conn.SendCommand(script);
        }

        private void SetSlaveLOPMsrtItemScript(uint index, ElectSettingData item, bool isHwTrigger)
        {
            string script = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            // [0] I Source;  [1] V Source
            int srcMode;

            srcFunc = "v";

            msrtFunc = "i";

            srcMode = 1;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            double forceRange = Math.Abs(item.DetectorBiasValue);

            double msrtRange = Math.Abs(item.DetectorMsrtRange);

            if (forceRange < 2.0d)
            {
                forceRange = 2.0d;
            }

            double forceRange2 = Math.Abs(item.DetectorBiasValue2);

            double msrtRange2 = Math.Abs(item.DetectorMsrtRange2);

            if (forceRange2 < 2.0d)
            {
                forceRange2 = 2.0d;
            }

            //-----------------------------------------------------------------------------------------------------------------------------
            if (isHwTrigger)
            {
                #region >>> H/W Trig <<<

                script = "loadscript num_" + index.ToString() + "\n";

                if (item.IsTrigDetector && item.IsTrigDetector2 && this._isDualSMU)
                {
                    script += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                    script += "setBSorceRangeV(" + forceRange2.ToString() + ")\n";

                    script += "setLimitI(" + msrtRange.ToString() + ")\n";
                    script += "setBLimitI(" + msrtRange2.ToString() + ")\n";

                    script += "setNPLC(" + item.DetectorMsrtNPLC.ToString() + ")\n";
                    script += "setBNPLC(" + item.DetectorMsrtNPLC2.ToString() + ")\n";

                    script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
                    script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

                    script += "setMeasureRangeI(" + msrtRange.ToString() + ")\n";
                    script += "setBMeasureRangeI(" + msrtRange2.ToString() + ")\n";

                    script += "if getOutput() ~= 1 then setOutput(1) end\n";
                    script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

                    script += IOAssert(PIN_SMU_SYNC);
                    //script += "digio.trigger[" + PIN_SMU_SYNC.ToString() + "].assert()\n";  // 為了同步 先用 Trigger In 訊號讓兩個 Script 同步

                    script += "setLevelV(" + item.DetectorBiasValue.ToString() + ")\n";
                    script += "setBLevelV(" + item.DetectorBiasValue2.ToString() + ")\n";

                    script += IOWait(PIN_SMU_TRIG_OUT, 1);// 
                    //script += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].wait(5)\n";

                    //-------------------------------------------------------------------------------------------------
                    // SMU Msrt
                    script += "mrtIsyncA(bufA1)\n";  // overlappedi
                    script += "mrtIsyncB(bufB1)\n";  // overlappedi
                    script += "waitcomplete()\n";
                    //-------------------------------------------------------------------------------------------------

                    script += "printbuffer(1, 1, bufA1, bufB1)\n";

                    script += "setOutput(0)\n";
                    script += "setBOutput(0)\n";

                    script += "bufA1.clear()\n";
                    script += "bufB1.clear()\n";
                }
                else if (item.IsTrigDetector2 && this._isDualSMU)
                {
                    script += "setBSorceRangeV(" + forceRange2.ToString() + ")\n";

                    script += "setBLimitI(" + msrtRange2.ToString() + ")\n";

                    script += "setBNPLC(" + item.DetectorMsrtNPLC2.ToString() + ")\n";

                    script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

                    script += "setBMeasureRangeI(" + msrtRange2.ToString() + ")\n";

                    script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

                    script += IOAssert(PIN_SMU_SYNC);
                    //script += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].assert()\n";  // 為了同步 先用 Trigger In 訊號讓兩個 Script 同步

                    script += "setBLevelV(" + item.DetectorBiasValue2.ToString() + ")\n";

					script += IOWait(PIN_SMU_TRIG_OUT, 1);// 

                    //script += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].wait(5)\n";

                    //-------------------------------------------------------------------------------------------------
                    // SMU Msrt
                    script += "local reading = mrtB." + msrtFunc + "()\n";

                    //-------------------------------------------------------------------------------------------------

                    script += "setBOutput(0)\n";

                    script += "print(reading)\n";
                }
                else if (item.IsTrigDetector)
                {
                    script += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    script += "setLimitI(" + msrtRange.ToString() + ")\n";

                    script += "setNPLC(" + item.DetectorMsrtNPLC.ToString() + ")\n";

                    script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

                    script += "setMeasureRangeI(" + msrtRange.ToString() + ")\n";

                    script += "if getOutput() ~= 1 then setOutput(1) end\n";

                    script += IOAssert(PIN_SMU_SYNC);
                    //script += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].assert()\n";  // 為了同步 先用 Trigger In 訊號讓兩個 Script 同步

                    script += "setLevelV(" + item.DetectorBiasValue.ToString() + ")\n";

                    script += IOWait(PIN_SMU_TRIG_OUT,1);
                    //script += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].wait(1)\n";

                    //-------------------------------------------------------------------------------------------------
                    // SMU Msrt
                    script += "local reading = mrtA." + msrtFunc + "()\n";

                    //-------------------------------------------------------------------------------------------------

                    script += "setOutput(0)\n";

                    script += "print(reading)\n";
                }
              
                script += "endscript\n";

                this._conn.SendCommand(script);

                #endregion
            }
            else
            {
                #region >>> S/W Trig <<<

                script = "loadscript num_" + index.ToString() + "\n";

                if (item.IsTrigDetector && item.IsTrigDetector2 && this._isDualSMU)
                {
                    script += "setNPLC(" + item.DetectorMsrtNPLC.ToString() + ")\n";
                    script += "setBNPLC(" + item.DetectorMsrtNPLC2.ToString() + ")\n";

                    script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
                    script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

                    script += "setMeasureRangeI(" + msrtRange.ToString() + ")\n";
                    script += "setBMeasureRangeI(" + msrtRange2.ToString() + ")\n";

                    //=============================
                    // 切完Range後，在Set Range & Compliance
                    //=============================
                    script += "if getOutput() ~= 1 then setOutput(1) end\n";
                    script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

                    script += "setLimitI(" + msrtRange.ToString() + ")\n";
                    script += "setBLimitI(" + msrtRange2.ToString() + ")\n";

                    script += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                    script += "setBSorceRangeV(" + forceRange2.ToString() + ")\n";

                    script += "setLevelV(" + item.DetectorBiasValue.ToString() + ")\n";
                    script += "setBLevelV(" + item.DetectorBiasValue2.ToString() + ")\n";
                    //script += "delay(" + item.ForceTime.ToString() + ")\n";

                    script += "mrtIsyncA(bufA1)\n";  // overlappedi
                    script += "mrtIsyncB(bufB1)\n";  // overlappedi

                    script += "waitcomplete()\n";

                    // script += "setLevelV(0)\n";
                    // script += "setBLevelV(0)\n";
                    
                    script += "printbuffer(1, 1, bufA1, bufB1)\n";

                    script += "setOutput(0)\n";
                    script += "setBOutput(0)\n";
                 
                    script += "bufA1.clear()\n";
                    script += "bufB1.clear()\n";
                }
                else if (item.IsTrigDetector2 && this._isDualSMU)
                {
                    script += "setBNPLC(" + item.DetectorMsrtNPLC2.ToString() + ")\n";

                    script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

                    script += "setBMeasureRangeI(" + msrtRange2.ToString() + ")\n";

                    //=============================
                    // 切完Range後，在Set Range & Compliance
                    //=============================
                    script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

                    script += "setBLimitI(" + msrtRange2.ToString() + ")\n";

                    script += "setBSorceRangeV(" + forceRange2.ToString() + ")\n";

                    script += "setBLevelV(" + item.DetectorBiasValue2.ToString() + ")\n";

                    //script += "delay(" + item.ForceTime.ToString() + ")\n";

                    script += "print(mrtB." + msrtFunc + "())\n";

                    // script += "setLevelV(0)\n";

                    script += "setBOutput(0)\n";
                }
                else if (item.IsTrigDetector)
                {
                    script += "setNPLC(" + item.DetectorMsrtNPLC.ToString() + ")\n";

                    script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

                    script += "setMeasureRangeI(" + msrtRange.ToString() + ")\n";

                    //=============================
                    // 切完Range後，在Set Range & Compliance
                    //=============================
                    script += "if getOutput() ~= 1 then setOutput(1) end\n";

                    script += "setLimitI(" + msrtRange.ToString() + ")\n";

                    script += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    script += "setLevelV(" + item.DetectorBiasValue.ToString() + ")\n";

                    //script += "delay(" + item.ForceTime.ToString() + ")\n";

                    script += "print(mrtA." + msrtFunc + "())\n";

                    // script += "setLevelV(0)\n";

                    script += "setOutput(0)\n";
                }


                script += "endscript\n";

                script += "num_" + index.ToString() + ".source = nil";

                this._conn.SendCommand(script);

                //this.ExportScriptToTxt(script, "SlavePD");

                #endregion
            }
        }

        private void SetPIVMsrtItemScript(uint index, ElectSettingData item)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double complLimit = 0.0d;

            double calcForceTime = 0.0d;

            double turnOffTime = 0.0d;

            //-----------------------------------------------------------------------------------------
            // Sweep List
            //-----------------------------------------------------------------------------------------
            string listName = string.Format("List_{0}", index);

            int sweepCnt = this._sweepList.Count;

            //---------------------------------------------------------------------------------------
            // [0] I Source;  [1] V Source
            int srcMode;

            srcFunc = "i";

            msrtFunc = "v";

            srcMode = 0;

            complLimit = this._voltRange[0][this._voltRange[0].Length - 1];

            double forceRange = Math.Abs(item.SweepStop);

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (forceRange < 0.000001)
            {
                forceRange = 0.000001;
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================
            if (item.MsrtNPLC == 0.01d && item.MsrtProtection < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            if (item.MsrtProtection <= complLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + index.ToString() + "\n";

            script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";

                //script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + item.MsrtRange.ToString() + ")\n";

                //script += "setMeasureRangeV(" + item.ForceRange.ToString() + ")\n";
            }

            if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB && this._isDualSMU)
            {
                #region >>> PIV For KEITHLEY 2612B <<<

                double detectorForceRange = Math.Abs(item.DetectorBiasValue) < 2.0d ? 6.0d : item.DetectorBiasValue;

                script += "if getBFunc() ~= 1 then setBFunc(1) end\n";

                script += "setBNPLC(" + item.DetectorMsrtNPLC.ToString() + ")\n";

                script += "setBLimitI(" + item.DetectorMsrtRange.ToString() + ")\n";

                script += "setBSorceRangeV(" + detectorForceRange.ToString() + ")\n";

                script += "setBMeasureRangeI(" + item.DetectorMsrtRange.ToString() + ")\n";

                script += "setBLevelV(" + item.DetectorBiasValue.ToString() + ")\n";

                script += "setBOutput(1)\n";

                script += srcRngAndComplScript;
                //======================================
                // 切完Range後，在Set Range & Compliance
                //=======================================
                if (item.ForceDelayTime > 0)
                {
                    script += "delay(" + item.ForceDelayTime.ToString() + ")\n";
                }

                script += "if getOutput() ~= 1 then setOutput(1) end\n";

                script += "for i = 1, " + sweepCnt.ToString() + " do\n";

                script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

                script += "setLevelI(" + listName + "[i])\n";

                if (item.SourceFunction != ESourceFunc.CW)
                {
                    calcForceTime = item.ForceTime - 0.0005d;

                    calcForceTime = calcForceTime < 0 ? 0.0d : calcForceTime;
                }
                else
                {
                    calcForceTime = item.ForceTime;
                }

                script += "delay(" + calcForceTime.ToString() + ")\n";

                script += "mrtIVsyncA(bufA1, bufA2)\n";  // overlappediv

                script += "mrtIsyncB(bufB1)\n";  // overlappedi

                script += "waitcomplete()\n";

                if (item.SweepTurnOffTime > 0)
                {
                    script += "setLevelI(0)\n";

                    //script += "setFunc(1)\n";

                    //script += "setLevelV(0)\n";

                    //if (item.SweepTurnOffTime >= 0.001d)  // time unit = s
                    //{
                    //    turnOffTime = item.SweepTurnOffTime - 0.0009d;
                    //}

                    turnOffTime = item.SweepTurnOffTime - 0.0001d;

                    turnOffTime = turnOffTime < 0 ? 0.0d : turnOffTime;

                    script += "delay(" + turnOffTime.ToString() + ")\n";
                }

                script += "end\n";  // Trigger Loop End

                script += "setBOutput(0)\n";

                if (item.SweepTurnOffTime == 0.0d)  // time unit = s
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";

                    if (forceRange >= 0.1)
                    {
                        // 防止切換 1A Range 時, 產生 Overshoot
                        script += "setSorceRangeI(0.01)\n";
                    }
                }

                //script += "setOutput(0)\n";

                script += "printbuffer(1, " + this._sweepList.Count.ToString() + ", bufA1, bufA2, bufB1)\n";

                script += "bufA1.clear()\n";
                script += "bufA2.clear()\n";
                script += "bufB1.clear()\n";

                #endregion
            }
            else
            {
                #region >>> PIV For KEITHLEY 2611B + 2611B / 2651A + 2611B <<<

                script += srcRngAndComplScript;
                //======================================
                // 切完Range後，在Set Range & Compliance
                //=======================================
                if (item.ForceDelayTime > 0)
                {
                    script += "delay(" + item.ForceDelayTime.ToString() + ")\n";
                }

                script += "if getOutput() ~= 1 then setOutput(1) end\n";

                script += IOWait(PIN_SMU_SYNC, 0.2);
                //script += "digio.trigger[" + PIN_SMU_SYNC.ToString() + "].wait(0.2)\n";  // Time Out 200ms, Sync for SMU#2

                script += "for i = 1, " + sweepCnt.ToString() + " do\n";

                script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

                script += "setLevelI(" + listName + "[i])\n";

                if (item.SourceFunction != ESourceFunc.CW)
                {
                    calcForceTime = item.ForceTime - 0.00062d;

                    calcForceTime = calcForceTime < 0 ? 0.0d : calcForceTime;
                }
                else
                {
                    calcForceTime = item.ForceTime;
                }

				script += IOAssert(PIN_SMU_TRIG_OUT);

                script += "delay(" + calcForceTime.ToString() + ")\n";

                script += "mrtIVsyncA(bufA1, bufA2)\n";

                script += "waitcomplete()\n";

                if (item.SweepTurnOffTime > 0)
                {
                    script += "setLevelI(0)\n";

                    //script += "setFunc(1)\n";

                    //script += "setLevelV(0)\n";

                    //if (item.SweepTurnOffTime >= 0.001d)  // time unit = s
                    //{
                    //    turnOffTime = item.SweepTurnOffTime - 0.0009d;
                    //}

                    turnOffTime = item.SweepTurnOffTime - 0.0001d;

                    turnOffTime = turnOffTime < 0 ? 0.0d : turnOffTime;

                    script += "delay(" + turnOffTime.ToString() + ")\n";
                }

                script += "end\n";

                if (item.SweepTurnOffTime == 0.0d)  // time unit = s
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";

                    if (forceRange >= 0.1)
                    {
                        // 防止切換 1A Range 時, 產生 Overshoot
                        script += "setSorceRangeI(0.01)\n";
                    }
                }

                script += "printbuffer(1, bufA1.n, bufA1, bufA2)\n";

                script += "bufA1.clear()\n";

                script += "bufA2.clear()\n";

                #endregion
            }

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            //ExportScriptToTxt(script, "NewPiv");

            this._conn.SendCommand(script);
        }

        private void SetSlavePIVMsrtItemScript(uint index, ElectSettingData item)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double complLimit = 0.0d;

            // [0] I Source;  [1] V Source
            int srcMode;

            srcFunc = "v";

            msrtFunc = "i";

            srcMode = 1;

            complLimit = this._currRange[1][this._currRange[1].Length - 1];

            double detectorForceRange = Math.Abs(item.DetectorBiasValue) < 2.0d ? 6.0d : item.DetectorBiasValue;

            script = "loadscript num_" + index.ToString() + "\n";

            script += "setSorceRangeV(" + detectorForceRange.ToString() + ")\n";

            script += "setLimitI(" + item.DetectorMsrtRange.ToString() + ")\n";

            script += "setNPLC(" + item.DetectorMsrtNPLC.ToString() + ")\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            script += "setMeasureRangeI(" + item.DetectorMsrtRange.ToString() + ")\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += IOAssert(PIN_SMU_TRIG_IN);
            //script += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].assert()\n";  // 為了同步 先用 Trigger In 訊號讓兩個 Script 同步

            script += "local TrigIN = false\n";

            script += "local ioBreak\n";

            script += "setLevelV(" + item.DetectorBiasValue.ToString() + ")\n";

            script += "while true do\n";

            script += "ioBreak =" + IOWait(PIN_SMU_SYNC, 0.000001);
            //script += "ioBreak = digio.trigger[" + PIN_SMU_SYNC.ToString() + "].wait(0.000001)\n";

            script += "if ioBreak then break end\n";

            script += "TrigIN =  " + IOWait(PIN_SMU_TRIG_OUT, 0.000001);
            //script += "TrigIN = digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].wait(0.000001)\n";

            //-------------------------------------------------------------------------------------------------
            // SMU Msrt
            script += "if TrigIN then\n";

            // script += "delay(" + item.ForceTime.ToString() + ")\n";

            script += "mrtA." + msrtFunc + "(bufA1)\n";

            script += IOAssert(PIN_SMU_TRIG_IN);
            //script += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].assert()\n";

            script += "end\n";
            //-------------------------------------------------------------------------------------------------

            script += "end\n";

            script += "setOutput(0)\n";

            script += "printbuffer(1, bufA1.n, bufA1)\n";

            script += "bufA1.clear()\n";

            //  script += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].release()\n";

            script += "endscript\n";

            this._conn.SendCommand(script);
        }

        private void SetPulsePIVMsrtItemScript_Daul(uint index, ElectSettingData item, int region)
        {
            uint trigCount = 1;
            double pulseWidth = item.ForceTime;
            double turnOffTime = item.SweepTurnOffTime;

            double pulsePeriod = Math.Round((pulseWidth + turnOffTime), 6, MidpointRounding.AwayFromZero);
        
            double start = item.SweepStart;
            double stop = item.SweepStop;

            double limitV = item.MsrtProtection;
            double nplc = item.MsrtNPLC;

            double detectorBiasValue = item.DetectorBiasValue;
            double detectorMsrtRange = item.DetectorMsrtRange;
            double detectorNplc = item.DetectorMsrtNPLC;

            double forceRange = Math.Abs(item.SweepStop);

            double detectorForceRange = Math.Abs(item.DetectorBiasValue) < 2.0d ? 6.0d : item.DetectorBiasValue;

            double msrtDelay = 0.0d;

            msrtDelay = Math.Round((pulseWidth - (1.0d / this._lineFreq) * nplc - 60e-6), 6, MidpointRounding.AwayFromZero);

            msrtDelay = msrtDelay >= 0.0d ? msrtDelay : 60e-6d;

            if (pulseWidth < 200e-6d)
            {
                nplc = 0.002d;
                detectorNplc = 0.002d;
            }
            else if (pulseWidth < 300e-6d)
            {
                msrtDelay = 75e-6;
                nplc = 0.005d;
                detectorNplc = 0.005d;
            }
            else
            {
                nplc = 0.01d;
                detectorNplc = 0.01d;
            }

            if (item.SourceFunction == ESourceFunc.QCW)
            {
                trigCount = item.SweepRiseCount * item.PulseCount;
            }
            else
            {
                trigCount = item.SweepRiseCount;
            }

            string listName = string.Format("List_{0}", index);

            string script = string.Empty;

            script += "loadscript num_" + index.ToString() + "\n";

            //--------------------------------------------------------------
            // smua Config
            script += "setFunc(0)\n";    // [0] I Source;  [1] V Source
            script += "setSorceRangeI(" + forceRange.ToString() + ")\n";
            script += "setLevelI(0)\n";
            script += "setLimitV(" + limitV.ToString() + ")\n";
            script += "setMeasureRangeV(" + limitV.ToString() + ")\n";
            script += "setNPLC(" + nplc.ToString() + ")\n";
            script += "setMeasureDelay(0)\n";

            script += "bufA1.clear()\n";
            script += "bufA1.collecttimestamps = 0\n";
            script += "bufA2.clear()\n";
            script += "bufA2.collecttimestamps = 0\n";
            //--------------------------------------------------------------
            // smub Config
            script += "setBFunc(1)\n";    // [0] I Source;  [1] V Source
            script += "setBSorceRangeV(" + detectorForceRange.ToString() + ")\n";
            script += "setBLevelV(0)\n";
            script += "setBLimitI(" + detectorMsrtRange.ToString() + ")\n";
            script += "setBMeasureRangeI(" + detectorMsrtRange.ToString() + ")\n";
            script += "setBNPLC(" + detectorNplc.ToString() + ")\n";
            script += "setBMeasureDelay(0)\n";

            script += "bufB1.clear()\n";
            script += "bufB1.collecttimestamps = 0\n";
            script += "bufB2.clear()\n";
            script += "bufB2.collecttimestamps = 0\n";

            //------------------------------------------------------------------------------------------------------------
            // Control Period Timer
            script += string.Format("T1_Count({0})\n", trigCount);
            script += string.Format("T1_Delay({0})\n", pulsePeriod);
            script += string.Format("T1_Passthrough({0})\n", "true");
            script += string.Format("T1_Stimulus({0})\n", "TrigA.ARMED_EVENT_ID");

            // Control Measure Delay Timer
            script += string.Format("T2_Count({0})\n", 1);

            //script += string.Format("T2_Delay({0}-(1/localnode.linefreq)*{1}- 60e-6)\n", pulseWidth, nplc);
            script += string.Format("T2_Delay({0})\n", msrtDelay);
            script += string.Format("T2_Passthrough({0})\n", "false");
            script += string.Format("T2_Stimulus({0})\n", "Trig_T1.EVENT_ID");

            // Control Pulse Width Timer
            script += string.Format("T3_Count({0})\n", 1);
            script += string.Format("T3_Delay({0})\n", pulseWidth);
            script += string.Format("T3_Passthrough({0})\n", "false");
            script += string.Format("T3_Stimulus({0})\n", "Trig_T1.EVENT_ID");

            //------------------------------------------------------------------------------------------------------------
            // smua
            script += "TrigA_Src.listi(" + listName + ")\n";  // configure the source action

            script += "TrigA_LimitV(" + limitV.ToString() + ")\n";
            script += "TrigA_Action_Msrt(smua.ENABLE)\n";
            script += "TrigA_Msrt.iv(bufA1, bufA2)\n";
    
            script += "TrigA_Action_EndPulse(smua.SOURCE_IDLE)\n";
            script += "TrigA_Action_EndSweep(smua.SOURCE_IDLE)\n";

            script += "TrigA_Count(" + trigCount.ToString() + ")\n";

            script += "TrigA_Stimulus_Arm(0)\n";
            script += "TrigA_Stimulus_Src(Trig_T1.EVENT_ID)\n";      // control period (Ton + Toff); stimulus by Arm
            script += "TrigA_Stimulus_Msrt(Trig_T2.EVENT_ID)\n";     // control Measure timeing;     stimulus by timer[1]
            script += "TrigA_Stimulus_EndPulse(Trig_T3.EVENT_ID)\n"; // control Ton;                 stimulus by timer[1]
            script += "TrigA_Action_Src(smua.ENABLE)\n";

            //--------------------------------------------------------------
            //smub
           // script += "TrigB_Src.linearv(" + detectorBiasValue.ToString() + "," + detectorBiasValue.ToString() + "," + trigCount.ToString() + ")\n";

            script += "TrigA_Stimulus_Arm(0)\n";

            ////--------------------------------------------------------------
            //script += "TrigA_LimitI(" + detectorMsrtRange.ToString() + ")\n";
            //script += "TrigA_Action_Msrt(smua.ENABLE)\n";
            //script += "TrigA_Msrt.i(bufA1)\n";
            //script += "TrigA_Count(" + trigCount.ToString() + ")\n";
            //script += "TrigA_Stimulus_Msrt(digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].EVENT_ID)\n";


            script += "TrigB_LimitI(" + detectorMsrtRange.ToString() + ")\n";
            script += "TrigB_Action_Msrt(smub.ENABLE)\n";     
            script += "TrigB_Msrt.i(bufB1)\n";

        //    script += "TrigB_Action_EndPulse(smub.SOURCE_HOLD)\n";
         //   script += "TrigB_Action_EndSweep(smub.SOURCE_IDLE)\n";

            script += "TrigB_Count(" + trigCount.ToString() + ")\n";
           
            script += "TrigB_Stimulus_Arm(0)\n";
            //script += "TrigB_Stimulus_Src(Trig_T1.EVENT_ID)\n";      // control period (Ton + Toff); stimulus by Arm
            script += "TrigB_Stimulus_Msrt(Trig_T2.EVENT_ID)\n";     // control Measure timeing;     stimulus by timer[1]
          //  script += "TrigB_Stimulus_EndPulse(Trig_T3.EVENT_ID)\n"; // control Ton;                 stimulus by timer[1]
           // script += "TrigB_Action_Src(smub.ENABLE)\n";

            //--------------------------------------------------------------
            // smua & smub Trigger
            script += "setBOutput(1)\n";
            script += "setOutput(1)\n";

            script += "setBLevelV(" + item.DetectorBiasValue.ToString() + ")\n";
            script += "TrigB.initiate()\n";
            script += "TrigA.initiate()\n";
          
            script += "waitcomplete()\n";

            script += "setOutput(0)\n";
            script += "setBLevelV(0)\n";
            script += "setBOutput(0)\n";

            if (forceRange >= 0.1)
            {
                // 防止切換 1A Range 時, 產生 Overshoot
                script += "setSorceRangeI(0.01)\n";
            }

            //--------------------------------------------------------------
            // smua & smub print result
            double calcSweepEndTurnOffTime = Math.Round((turnOffTime * 0.95d - 10e-3), 6, MidpointRounding.AwayFromZero);

            if (calcSweepEndTurnOffTime > 0.0d)
            {
                script += "delay(" + calcSweepEndTurnOffTime.ToString() + ")\n";
            }

            script += "printbuffer(1, " + trigCount + ", bufA1, bufA2, bufB1)\n";
            script += "Trig_T1.reset()\n";
            script += "Trig_T2.reset()\n";
            script += "Trig_T3.reset()\n";
            script += "bufA1.clear()\n";
            script += "bufA2.clear()\n";
            script += "bufB1.clear()\n";

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil\n";

            this._conn.SendCommand(script);
        }

        private void SetPulsePIVMsrtItemScript(uint index, ElectSettingData item, int region)
        {
            uint trigCount = 1;

            double pulseWidth = item.ForceTime;
            double turnOffTime = item.SweepTurnOffTime;

            double pulsePeriod = Math.Round((pulseWidth + turnOffTime), 6, MidpointRounding.AwayFromZero);

            double start = item.SweepStart;
            double stop = item.SweepStop;

            double limitV = item.MsrtProtection;
            double nplc = item.MsrtNPLC;

            //double detectorBiasValue = item.DetectorBiasValue;
            //double detectorMsrtRange = item.DetectorMsrtRange;

            double forceRange = Math.Abs(item.SweepStop);

            //double detectorForceRange = Math.Abs(item.DetectorBiasValue) < 2.0d ? 6.0d : item.DetectorBiasValue;

            string script = string.Empty;

            if (item.SourceFunction == ESourceFunc.QCW)
            {
                trigCount = item.SweepRiseCount * item.PulseCount;
            }
            else
            {
                trigCount = item.SweepRiseCount;
            }

            string listName = string.Format("List_{0}", index);

            script += "loadscript num_" + index.ToString() + "\n";

            //--------------------------------------------------------------
            // smua Config
            script += "setNPLC(" + nplc.ToString() + ")\n";

            script += "setFunc(0)\n";    // [0] I Source;  [1] V Source
            script += "setLevelI(0)\n";

            script += "setMeasureDelay(0)\n";
            script += "setLimitV(" + limitV.ToString() + ")\n";
            script += "setMeasureRangeV(" + limitV.ToString() + ")\n";

            script += "setSorceRangeI(" + forceRange.ToString() + ")\n";

            script += "bufA1.clear()\n";
            script += "bufA1.collecttimestamps = 0\n";
            script += "bufA2.clear()\n";
            script += "bufA2.collecttimestamps = 0\n";

            //------------------------------------------------------------------------------------------------------------
            // Control Period Timer
            script += string.Format("T1_Count({0})\n", trigCount);
            script += string.Format("T1_Delay({0})\n", pulsePeriod);
            script += string.Format("T1_Passthrough({0})\n", "true");
            script += string.Format("T1_Stimulus({0})\n", "TrigA.ARMED_EVENT_ID");

            // Control Measure Delay Timer
            script += string.Format("T2_Count({0})\n", 1);

            double msrtDelay = 0.0d;

            msrtDelay = Math.Round((pulseWidth - (1.0d / this._lineFreq) * nplc - 60e-6), 6, MidpointRounding.AwayFromZero);

            msrtDelay = msrtDelay >= 0.0d ? msrtDelay : 10e-6d;

            //script += string.Format("T2_Delay({0}-(1/localnode.linefreq)*{1}- 60e-6)\n", pulseWidth, nplc);
            script += string.Format("T2_Delay({0})\n", msrtDelay);
            script += string.Format("T2_Passthrough({0})\n", "false");
            script += string.Format("T2_Stimulus({0})\n", "Trig_T1.EVENT_ID");

            // Control Pulse Width Timer
            script += string.Format("T3_Count({0})\n", 1);
            script += string.Format("T3_Delay({0})\n", pulseWidth);
            script += string.Format("T3_Passthrough({0})\n", "false");
            script += string.Format("T3_Stimulus({0})\n", "Trig_T1.EVENT_ID");

            //------------------------------------------------------------------------------------------------------------
            script += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].stimulus = smua.trigger.SOURCE_COMPLETE_EVENT_ID\n";

            //--------------------------------------------------------------
            // smua
            script += "TrigA_Src.listi(" + listName + ")\n";  // configure the source action
            script += "TrigA_LimitV(" + limitV.ToString() + ")\n";
            script += "TrigA_Action_Msrt(smua.ENABLE)\n";
            script += "TrigA_Msrt.iv(bufA1, bufA2)\n";

            script += "TrigA_Action_EndPulse(smua.SOURCE_IDLE)\n";
            script += "TrigA_Action_EndSweep(smua.SOURCE_IDLE)\n";

            script += "TrigA_Count(" + trigCount.ToString() + ")\n";
            script += "TrigA_Stimulus_Arm(0)\n";

            script += "TrigA_Stimulus_Src(Trig_T1.EVENT_ID)\n";   // control period (Ton + Toff); stimulus by Arm
            script += "TrigA_Stimulus_Msrt(Trig_T2.EVENT_ID)\n";  // control Measure timeing;     stimulus by timer[1]
            script += "TrigA_Stimulus_EndPulse(Trig_T3.EVENT_ID)\n"; // control Ton;                 stimulus by timer[1]
            script += "TrigA_Action_Src(smua.ENABLE)\n";

       
            //--------------------------------------------------------------
            // smua Trigger

            script += "setOutput(1)\n";

            script += "digio.trigger[" + PIN_SMU_SYNC.ToString() + "].wait(0.2)\n";  // Slave Sync; Time out = 200ms

            script += "TrigA.initiate()\n";

            script += "waitcomplete()\n";

            script += "setOutput(0)\n";

            if (forceRange >= 0.1)
            {
                // 防止切換 1A Range 時, 產生 Overshoot
                script += "setSorceRangeI(0.01)\n";
            }

            //--------------------------------------------------------------
            // smua print result
            double calcSweepEndTurnOffTime = Math.Round((turnOffTime * 0.95d - 10e-3), 6, MidpointRounding.AwayFromZero);

            if (calcSweepEndTurnOffTime > 0.0d)
            {
                script += "delay(" + calcSweepEndTurnOffTime.ToString() + ")\n";
            }

            script += "printbuffer(1, " + trigCount + ", bufA1, bufA2)\n";
            script += "Trig_T1.reset()\n";
            script += "Trig_T2.reset()\n";
            script += "Trig_T3.reset()\n";
            script += "bufA1.clear()\n";
            script += "bufA2.clear()\n";;

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil\n";

            this._conn.SendCommand(script);
        }

        private void SetSlavePulsePIVMsrtItemScript(uint index, ElectSettingData item)
        {
            uint trigCount = 1;
            double pulsePeriod = item.ForceTime + item.SweepTurnOffTime;
            double pulseWidth = item.ForceTime;
 
            double detectorBiasValue = item.DetectorBiasValue;
            double detectorMsrtRange = item.DetectorMsrtRange;
            double nplc = item.DetectorMsrtNPLC;

            double detectorForceRange = Math.Abs(detectorBiasValue) < 2.0d ? 6.0d : detectorBiasValue;

            string script = string.Empty;

            if (item.SourceFunction == ESourceFunc.QCW)
            {
                trigCount = item.SweepRiseCount * item.PulseCount;
            }
            else
            {
                trigCount = item.SweepRiseCount;
            }


            script += "loadscript num_" + index.ToString() + "\n";

            script += "bufA1.clear()\n";
            script += "bufA1.collecttimestamps = 0\n";

            script += "setFunc(1)\n";    // [0] I Source;  [1] V Source
            script += "setSorceRangeV(" + detectorForceRange.ToString() + ")\n";
            script += "setLimitI(" + detectorMsrtRange.ToString() + ")\n";
            script += "setMeasureRangeI(" + detectorMsrtRange.ToString() + ")\n";
            script += "setNPLC(" + nplc.ToString() + ")\n";

            double msrtDelay = 0.0d;

            msrtDelay = Math.Round((pulseWidth - (1.0d / this._lineFreq) * nplc - 60e-6), 6, MidpointRounding.AwayFromZero);

            msrtDelay = msrtDelay >= 0.0d ? msrtDelay : 10e-6d;

            script += string.Format("setMeasureDelay({0})\n", msrtDelay);
            //script += string.Format("setMeasureDelay({0}-(1/localnode.linefreq)*{1}- 60e-6)\n", pulseWidth, nplc);

            script += "TrigA_Stimulus_Arm(0)\n";

            //--------------------------------------------------------------
            script += "TrigA_LimitI(" + detectorMsrtRange.ToString() + ")\n";
            script += "TrigA_Action_Msrt(smua.ENABLE)\n";
            script += "TrigA_Msrt.i(bufA1)\n";
            script += "TrigA_Count(" + trigCount.ToString() + ")\n";
            script += "TrigA_Stimulus_Msrt(digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].EVENT_ID)\n";
           // script += "digio.trigger[" + PIN_SMU_TRIG_IN.ToString() + "].stimulus = smua.trigger.MEASURE_COMPLETE_EVENT_ID\n";  // Debug for measuring timing

            //--------------------------------------------------------------
            // smua Trigger
            script += "setOutput(1)\n";

            script += "setLevelV(" + item.DetectorBiasValue.ToString() + ")\n";

            script += "digio.trigger[" + PIN_SMU_SYNC.ToString() + "].assert()\n";

            script += "TrigA.initiate()\n";

            script += "waitcomplete()\n";

            script += "setLevelV(0)\n";
            script += "setMeasureDelay(0)\n";
            script += "setOutput(0)\n";

            //--------------------------------------------------------------
            // smua print result
            script += "printbuffer(1, bufA1.n, bufA1)\n";

            script += "bufA1.clear()\n";

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil\n";

            this._conn.SendCommand(script);
        }

        private void SetPulseIMsrtItemScript(uint index, ElectSettingData item, int region)
        {
            //region < 0, DC Mode
            double pulseTrigDelay = 0.0d;

            double nplc = 0.003;
            double pulseLevel = item.ForceValue;
            double pulseWidth = item.ForceTime;
            //double duty = region < 0 ? 1.0d : this._pulseDuty[region];
            double duty = item.Duty;
            double limitV = item.MsrtProtection;

            double pulsePeriod = pulseWidth / duty;

            uint trigCount = item.PulseCount;
            string script = string.Empty;

            double msrtDelay = 0.0d;
          
            double complLimit = this._voltRange[0][this._voltRange[0].Length - 1];

            if (pulseWidth < 200e-6d)
            {
                nplc = 0.001d;

                msrtDelay = 50e-6;
            }
            else if (pulseWidth < 300e-6d)
            {
                nplc = 0.005d;

                msrtDelay = 75e-6;
            }
            else
            {
                nplc = 0.01d;

                msrtDelay = Math.Round((pulseWidth - (1.0d / this._lineFreq) * nplc - 60e-6), 6, MidpointRounding.AwayFromZero);

                msrtDelay = msrtDelay >= 0.0d ? msrtDelay : 60e-6d;
            }


            pulseTrigDelay = pulseTrigDelay - 10e-6d;

            pulseTrigDelay = pulseTrigDelay >= 0.0d ? pulseTrigDelay : 0.0d;

            pulsePeriod = Math.Round(pulsePeriod, 6, MidpointRounding.AwayFromZero);

            script += "loadscript num_" + index.ToString() + "\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";
            // script += "setSorceAotoRangeI(smua.AUTORANGE_OFF)\n";
        
            script += "setLevelI(0)\n";
            script += "setLimitV(1)\n";

            script += "setNPLC(" + nplc.ToString() + ")\n";

            if (item.MsrtProtection <= complLimit)
            {
                script += "setMeasureRangeV(" + limitV.ToString() + ")\n";
                script += "setSorceRangeI(" + pulseLevel.ToString() + ")\n";
               
            }
            else
            {
                script += "setSorceRangeI(" + pulseLevel.ToString() + ")\n";
            script += "setMeasureRangeV(" + limitV.ToString() + ")\n";
            }


            script += "setMeasureDelay(0)\n";

            if (pulseTrigDelay != 0.0d)
            {
                script += "T4_Count(1)\n";
                script += "T4_Delay(" + pulseTrigDelay.ToString() + ")\n";
                script += "T4_Passthrough(false)\n";                           // timer passthrough the trigger, 
                script += "T4_Stimulus(TrigA.ARMED_EVENT_ID)\n";

                script += "T1_Count(" + trigCount.ToString() + ")\n";
                script += "T1_Delay(" + pulsePeriod.ToString() + ")\n";
                script += "T1_Passthrough(true)\n";
                script += "T1_Stimulus(Trig_T4.EVENT_ID)\n";
            }
            else
            {
                script += "T1_Count(" + trigCount.ToString() + ")\n";
                script += "T1_Delay(" + pulsePeriod.ToString() + ")\n";
                script += "T1_Passthrough(true)\n";
                script += "T1_Stimulus(TrigA.ARMED_EVENT_ID)\n";
            }


            script += "T2_Count(1)\n";

            //script += string.Format("T2_Delay({0}-(1/localnode.linefreq)*{1}- 60e-6)\n", pulseWidth, nplc);
            script += string.Format("T2_Delay({0})\n", msrtDelay);

           // script += "T2_Delay(" + pulseWidth.ToString() + " - (1/localnode.linefreq)*" + nplc.ToString() + " - 60e-6)\n";
            script += "T2_Passthrough(false)\n";
            script += "T2_Stimulus(Trig_T1.EVENT_ID)\n";

            script += "T3_Count(1)\n";
            script += "T3_Delay(" + pulseWidth.ToString() + ")\n";
            script += "T3_Passthrough(false)\n";
            script += "T3_Stimulus(Trig_T1.EVENT_ID)\n";

            if (item.IsTrigCamera)
            {
                script += "digio.trigger[" + PIN_CAMERA_TRIG_OUT.ToString() + "].stimulus = TrigA.ARMED_EVENT_ID\n";
               // script += "digio.trigger[" + PIN_SMU_TRIG_OUT.ToString() + "].stimulus = Trig_T1.EVENT_ID\n";
            }

            //script += "TrigA_Src.listi({" + value.ToString() + "})\n";
            script += "TrigA_Src.lineari(" + pulseLevel.ToString() + "," + pulseLevel.ToString() + "," + trigCount.ToString() + ")\n";
            script += "TrigA_LimitV(" + limitV.ToString() + ")\n";
            script += "TrigA_Action_Msrt(smua.ENABLE)\n";
            script += "TrigA_Msrt.v(bufA1)\n";
            script += "TrigA_Action_EndPulse(smua.SOURCE_IDLE)\n";
            script += "TrigA_Count(" + trigCount.ToString() + ")\n";
            script += "TrigA_Stimulus_Arm(0)\n";

            script += "TrigA_Stimulus_Src(Trig_T1.EVENT_ID)\n";   // control period (Ton + Toff); stimulus by Arm
            script += "TrigA_Stimulus_Msrt(Trig_T2.EVENT_ID)\n";  // control Measure timeing;     stimulus by timer[1]
            script += "TrigA_Stimulus_EndPulse(Trig_T3.EVENT_ID)\n"; // control Ton;                 stimulus by timer[1]

            script += "TrigA_Action_Src(smua.ENABLE)\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            if (item.ForceDelayTime > 0)
            {
                script += "delay(" + item.ForceDelayTime.ToString() + ")\n";
            }

            script += "TrigA.initiate()\n";

            script += "waitcomplete()\n";
            script += "setOutput(0)\n";

            script += "printbuffer(1, bufA1.n, bufA1)\n";

            script += "Trig_T1.reset()\n";
            script += "Trig_T2.reset()\n";
            script += "Trig_T3.reset()\n";
            script += "Trig_T4.reset()\n";
            script += "bufA1.clear()\n";

            if (pulseLevel >= 0.1)
            {
                // 防止切換 1A Range 時, 產生 Overshoot
                script += "setSorceRangeI(0.01)\n";
            }

            script += "endscript\n";
            script += "num_" + index.ToString() + ".source = nil\n";

            this._conn.SendCommand(script);
        }

        private void SetPulseIMsrtItemScript(uint index, ElectSettingData item)
        {
            //region < 0, DC Mode
            double nplc = 0.01;

           // double limitV = item.MsrtProtection;
            double tOn = item.ForceTime - (1 / 60.0d * nplc) - 450e-6;
            double tOff = (item.TurnOffTime - 0.1d)/ 1000.0d;
            uint pulseCnt = item.PulseCount;
            //double duty = region < 0 ? 1.0d : this._pulseDuty[region];
            //double pulsePeriod = region < 0 ? 0 : pulseWidth * (1 - duty) / duty;

            tOn = tOn >= 0 ? tOn : 0.1e-3;
            tOff = tOff >= 0 ? tOff : 0.0d;

            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double complLimit = 0.0d;

            // [0] I Source;  [1] V Source
            int srcMode;

            if (item.MsrtType == EMsrtType.FIMV || item.MsrtType == EMsrtType.POLAR || item.MsrtType == EMsrtType.LIV || item.MsrtType == EMsrtType.FI || item.MsrtType == EMsrtType.FIMVLOP)
            {
                srcFunc = "i";

                msrtFunc = "v";

                srcMode = 0;

                complLimit = this._voltRange[0][this._voltRange[0].Length - 1];
            }
            else
            {
                srcFunc = "v";

                msrtFunc = "i";

                srcMode = 1;

                complLimit = this._currRange[1][this._currRange[1].Length - 1];
            }

            double forceRange = item.ForceRange;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFunc == "i")
            {
                if (forceRange < 0.000001)
                {
                    forceRange = 0.000001;
                }
            }
            else
            {
                if (forceRange < 2.0d)
                {
                    forceRange = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================

            if (item.MsrtNPLC == 0.01d && item.MsrtProtection < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            if (item.MsrtProtection <= complLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + index.ToString() + "\n";

            script += "setNPLC(" + nplc.ToString() + ")\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + item.MsrtRange.ToString() + ")\n";
            }

            script += srcRngAndComplScript;

            //======================================
            // 切完Range後，在Set Range & Compliance
            //=======================================

            script += "delay(0.001)\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += "for i = 1, " + pulseCnt.ToString() + " do\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            script += "if i == 1 then digio.trigger[" + PIN_CAMERA_TRIG_OUT.ToString() + "].assert() end\n";

            if (srcFunc == "v")
            {
                script += "setLevelV(" + item.ForceValue.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + item.ForceValue.ToString() + ")\n";
            }

            script += "delay(" + tOn.ToString() + ")\n";

            script += "mrtA." + msrtFunc + "(bufA1)\n";

            script += "setLevelI(0)\n";

            //script += "setFunc(1)\n";

            //script += "setLevelV(0)\n";

            if (tOff != 0.0d)
            {
                script += "delay(" + tOff.ToString() + ")\n";
            }

            script += "end\n";  // for loop end

            script += "printbuffer(1, " + pulseCnt.ToString() + ", bufA1)\n";

            script += "bufA1.clear()\n";

            if (forceRange >= 0.1)
            {
                // 防止切換 1A Range 時, 產生 Overshoot
                script += "setSorceRangeI(0.01)\n";
            }

            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            this._conn.SendCommand(script);
        }

        private bool SetSweepListScript(uint index, ElectSettingData item)
        {
            this._sweepList.Clear();

            this._sweepTimeSpan.Clear();

            string script = string.Empty;

            double start = item.SweepStart;

            double stop = item.SweepStop;

            double step = 0;

            uint points = 0;

            uint pulseCount =1;

            string listName = string.Format("List_{0}", index);

            script += "loadandrunscript makeSweepList_" + index.ToString() + "\n";
             
            switch (item.MsrtType)
            {
                case EMsrtType.FIMVSWEEP:
                case EMsrtType.FVMISWEEP:
                    {
                        #region >>> Sweep Create List Script <<<

                        switch (item.SweepMode)
                        {
                            case ESweepMode.Linear:
                                {
                                    points = item.SweepRiseCount;

                                    step = (stop - start) / (points - 1);

                                    // Set value into the array
                                    script += listName + " = {}\n"
                                            + "for i = 1," + points.ToString() + " do\n"
                                            + listName + "[i] = " + start.ToString() + " + " + step.ToString() + " * (i - 1)\n"
                                            + "end\n";

                                    break;
                                }
                            case ESweepMode.Log:
                                {
                                    points = item.SweepRiseCount;

                                    if (start == 0.0d)
                                    {
                                        start = 1e-12;
                                    }

                                    if (stop == 0.0d)
                                    {
                                        stop = 1e-12;
                                    }

                                    int polar = 1;

                                    if (start < 0 || stop < 0)
                                    {
                                        polar = -1;
                                    }

                                    start = Math.Abs(start);

                                    stop = Math.Abs(stop);

                                    double logStepSize = (Math.Log10(stop) - Math.Log10(start)) / (points - 1);

                                    script += "local start =" + start.ToString() + "\n"
                                            + "local polar =" + polar.ToString() + "\n"
                                            + "local logStepSize =" + logStepSize.ToString() + "\n";

                                    // Set value into the array
                                    script += listName + " = {}\n"
                                            + "for i = 1," + points.ToString() + " do\n"
                                            + "logStep = logStepSize * (i - 1)\n"
                                            + listName + "[i] = math.pow(10, logStep) * start * polar\n"
                                            + "end\n";

                                    break;
                                }
                            default:
                                return false;
                        }

                        #endregion

                        break;
                    }
                case EMsrtType.PIV:
                    {
                        #region >>> PIV Create List Script <<<

                        //points = (uint)(Math.Ceiling((item.SweepStop - item.SweepStart) / item.SweepStep)) + 1;

                        points = (uint)(Math.Round(((item.SweepStop - item.SweepStart) / item.SweepStep), 6, MidpointRounding.AwayFromZero)) + 1;
               
                        step = item.SweepStep;

                        if (item.SourceFunction == ESourceFunc.QCW)
                        {
                            pulseCount = item.PulseCount;
                            
                            // Sweep List For QCW Mode
                            script += listName + " = {}\n"
                                 + "local cnt = 1\n"
                                 + "for i = 1," + points.ToString() + " do\n" // outer loop, Rising Step
                                 + "for j = 1," + pulseCount.ToString() + " do\n" // inner loop, Step Pulse Count
                                 + listName + "[cnt] = " + start.ToString() + " + " + step.ToString() + " * (i - 1)\n"
                                 + "if math.abs(" + listName + "[cnt]) > math.abs(" + stop.ToString() + ") then " + listName + "[cnt] =" + stop.ToString() + " end\n"
                                 + "cnt = cnt + 1\n"
                                 + "end\n"  // end of inner loop
                                 + "end\n"; // end of outer loop
                        }
                        else
                        {
                            // Sweep List For Pulse / CW Mode
                            script += listName + " = {}\n"
                                    + "for i = 1," + points.ToString() + " do\n"
                                    + listName + "[i] = " + start.ToString() + " + " + step.ToString() + " * (i - 1)\n"
                                    + "if math.abs(" + listName + "[i]) > math.abs(" + stop.ToString() + ") then " + listName + "[i] =" + stop.ToString() + " end\n"
                                    + "end\n";
                        }

                        #endregion

                        break;
                    }
                default:
                    return false;
            }

            // print Sweep List 
            script += "print(table.concat(" + listName + ", \",\"))\n"
                    + "endscript";

            this._conn.SendCommand(script);

            //---------------------------------------------------------------------------------------------------
            // Get F/W Created Sweep List
            string[] scriptCreateList = this.GetDevicePrintValueToArray(',');

            double tempValue = 0.0d;

            for (int i = 0; i < scriptCreateList.Length; i++)
            {
                Double.TryParse(scriptCreateList[i], out tempValue);

                this._sweepList.Add(tempValue);

                this._sweepTimeSpan.Add(item.ForceTime * i * 1000.0d); // s -> ms
            }

            return !this.GetErrorMsg();
        }

		private void SetLCRItemScript(uint index, ElectSettingData item)
		{
			string script = string.Empty;

			string srcRngAndComplScript = string.Empty;

			double complLimit = this._currRange[1][this._currRange[1].Length - 1];

			double forceRange = item.ForceRange;

			if (forceRange < 2.0d)
			{
				forceRange = 2.0d;
			}

			if (item.MsrtProtection <= complLimit)
			{
				srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";

				srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
			}
			else
			{
				srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

				srcRngAndComplScript += "setLimitI(" + item.MsrtProtection.ToString() + ")\n";
			}

			script = "loadscript num_" + index.ToString() + "\n";

			script += "if getFunc() ~= 1 then setFunc(1) end\n";

			script += "if getOutput() ~= 1 then setOutput(1) end\n";

			script += srcRngAndComplScript;

			script += "setLevelV(" + item.ForceValue.ToString() + ")\n";

			script += "endscript\n";

			script += "num_" + index.ToString() + ".source = nil";

			this._conn.SendCommand(script);
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

		#endregion

		#region >>> Private Method <<<

        private bool CheckDCModeElectSetting(ElectSettingData item)
        {
            bool rtn = true;

            rtn &= this.TimeUnitConvert(item);

            rtn &= this.FindSrcAndMsrtRange(item);

            rtn &= this.CheckProtectionModuleSpec(item);

            return rtn;
        }

        private bool CheckPulseModeElectSetting(ElectSettingData item, out int region)
        {
            region = -1;

            if (!this.TimeUnitConvert(item))
            {
                return false;
            }

            if (this.FindSrcAndMsrtRange(item))
            {
                //DC範圍
                return true;
            }
            else
            {
                //Pulse Mode範圍
                return this.FindSrcAndMsrtPulseRange(item, out region);
            }

            //需確認Protection Module 能否承受 Pulse Mode
            //rtn &= this.CheckProtectionModuleSpec(item);
        }

        private bool TimeUnitConvert(ElectSettingData item)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Time Unit Convert
            ////////////////////////////////////////////////////////////////////////////////////////
            item.ForceTime = Math.Round((item.ForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            item.RTHImForceTime = Math.Round((item.RTHImForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            item.RTHIhForceTime = Math.Round((item.RTHIhForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            item.RTHIm2ForceTime = Math.Round((item.RTHIm2ForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            item.ForceDelayTime = Math.Round((item.ForceDelayTime / 1000.0d), 6, MidpointRounding.AwayFromZero); //Unit: Second

            item.SweepTurnOffTime = Math.Round((item.SweepTurnOffTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            item.SweepHoldTime = Math.Round((item.SweepHoldTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            ////////////////////////////////////////////////////////////////////////////////////////
            // (2) Check Time Limit
            ////////////////////////////////////////////////////////////////////////////////////////
            if (item.ForceTime > MAX_DELAY_TIME || item.ForceTime < MIN_DELAY_TIME)
            {
                return false;
            }

            if (item.ForceDelayTime > MAX_DELAY_TIME || item.ForceDelayTime < MIN_DELAY_TIME)
            {
                return false;
            }

            return true;
        }

        //private bool FindSrcAndMsrtRange(ElectSettingData item)
        //{
        //    double tempValue = 0.0d;

        //    switch (item.MsrtType)
        //    {
        //        case EMsrtType.FIMV:
        //             if (Math.Abs(item.ForceValue) <= this._currRange[0][this._currRange[0].Length - 1])
        //            {
        //                if (item.IsAutoForceRange)
        //                {
        //                    item.ForceRange = Math.Abs(item.ForceValue);
        //                }
        //            }
        //            else
        //            {
        //                item.ForceRange = this._currRange[0][this._currRange[0].Length - 1];

        //                return false;
        //            }

        //            // Measurement Range
        //            if (Math.Abs(item.ForceValue) > this._currRange[1][this._currRange[1].Length - 1] && Math.Abs(item.MsrtRange) > this._voltRange[0][this._voltRange[0].Length - 1])
        //            {
        //                item.MsrtRange = this._voltRange[0][this._voltRange[0].Length - 1];

        //                item.MsrtProtection = Math.Abs(item.MsrtRange);

        //                return false;
        //            }
        //            else
        //            {
        //                if (Math.Abs(item.MsrtRange) > this._voltRange[1][this._voltRange[1].Length - 1])
        //                {
        //                    item.MsrtRange = this._voltRange[0][this._voltRange[0].Length - 1];
        //                    item.MsrtProtection = Math.Abs(item.MsrtRange);
        //                    return false;
        //                }
        //            }
        //            break;
        //        //-----------------------------------------------------------------------------//
        //        case EMsrtType.FI:
        //        case EMsrtType.THY:
        //        case EMsrtType.POLAR:
        //        case EMsrtType.R:
        //        case EMsrtType.RTH:
        //        case EMsrtType.VLR:
        //        case EMsrtType.LIV:
        //        case EMsrtType.FIMVLOP:
        //            // Force Range
        //            if (Math.Abs(item.ForceValue) <= this._currRange[0][this._currRange[0].Length - 1])
        //            {
        //                if (item.MsrtType != EMsrtType.LIV)
        //                {
        //                    item.ForceRange = Math.Abs(item.ForceValue);
        //                }
        //            }
        //            else
        //            {
        //                item.ForceRange = this._currRange[0][this._currRange[0].Length - 1];

        //                return false;
        //            }

        //            // Measurement Range
        //            if (Math.Abs(item.ForceValue) > this._currRange[1][this._currRange[1].Length - 1] && Math.Abs(item.MsrtRange) > this._voltRange[0][this._voltRange[0].Length - 1])
        //            {
        //                item.MsrtRange = this._voltRange[0][this._voltRange[0].Length - 1];

        //                item.MsrtProtection = Math.Abs(item.MsrtRange);

        //                return false;
        //            }
        //            else
        //            {
        //                if (Math.Abs(item.MsrtRange) > this._voltRange[1][this._voltRange[1].Length - 1])
        //                {
        //                    item.MsrtRange = this._voltRange[0][this._voltRange[0].Length - 1];
        //                    item.MsrtProtection = Math.Abs(item.MsrtRange);
        //                    return false;
        //                }
        //            }
        //            break;
        //        //-----------------------------------------------------------------------------//
        //        case EMsrtType.FVMI:
        //        case EMsrtType.FV:
        //        case EMsrtType.FVMILOP:
        //        case EMsrtType.LVI:
        //        case EMsrtType.FVMISCAN:
        //            // Force Range
        //            if (Math.Abs(item.ForceValue) <= this._voltRange[1][this._voltRange[1].Length - 1])
        //            {
        //                item.ForceRange = Math.Abs(item.ForceValue);
        //            }
        //            else
        //            {
        //                item.ForceRange = this._voltRange[1][this._voltRange[1].Length - 1];

        //                return false;
        //            }

        //            // Measurement Range
        //            if (Math.Abs(item.ForceValue) > this._voltRange[0][this._voltRange[0].Length - 1] && Math.Abs(item.MsrtRange) > this._currRange[1][this._currRange[1].Length - 1])
        //            {
        //                item.MsrtRange = this._currRange[1][this._currRange[1].Length - 1];

        //                item.MsrtProtection = Math.Abs(item.MsrtRange);

        //                return false;
        //            }
        //            else
        //            {
        //                if (Math.Abs(item.MsrtRange) > this._currRange[0][this._currRange[0].Length - 1])
        //                {
        //                    item.MsrtRange = this._currRange[1][this._currRange[1].Length - 1];

        //                    item.MsrtProtection = Math.Abs(item.MsrtRange);

        //                    return false;
        //                }
        //            }
        //            break;
        //        //-----------------------------------------------------------------------------//
        //        case EMsrtType.FIMVSWEEP:

        //            tempValue = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

        //            // Force Range
        //            if (tempValue <= this._currRange[0][this._currRange[0].Length - 1])
        //            {
        //                item.ForceRange = tempValue;
        //            }
        //            else
        //            {
        //                item.ForceRange = this._currRange[0][this._currRange[0].Length - 1];

        //                return false;
        //            }

        //            // Measurement Range
        //            if (tempValue > this._currRange[1][this._currRange[1].Length - 1] && Math.Abs(item.MsrtRange) > this._voltRange[0][this._voltRange[0].Length - 1])
        //            {
        //                item.MsrtRange = this._voltRange[0][this._voltRange[0].Length - 1];

        //                item.MsrtProtection = Math.Abs(item.MsrtRange);

        //                return false;
        //            }
        //            else
        //            {
        //                if (Math.Abs(item.MsrtRange) > this._voltRange[1][this._voltRange[1].Length - 1])
        //                {
        //                    item.MsrtRange = this._voltRange[0][this._voltRange[0].Length - 1];

        //                    item.MsrtProtection = Math.Abs(item.MsrtRange);

        //                    return false;
        //                }
        //            }
        //            break;
        //        //-----------------------------------------------------------------------------//
        //        case EMsrtType.FVMISWEEP:

        //            tempValue = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

        //            // Force Range
        //            if (tempValue <= this._voltRange[1][this._voltRange[1].Length - 1])
        //            {
        //                item.ForceRange = tempValue;
        //            }
        //            else
        //            {
        //                item.ForceRange = this._voltRange[1][this._voltRange[1].Length - 1];

        //                return false;
        //            }

        //            // Measurement Range
        //            if (tempValue > this._voltRange[0][this._voltRange[0].Length - 1] && Math.Abs(item.MsrtRange) > this._currRange[1][this._currRange[1].Length - 1])
        //            {
        //                item.MsrtRange = this._currRange[1][this._currRange[1].Length - 1];

        //                item.MsrtProtection = Math.Abs(item.MsrtRange);

        //                return false;
        //            }
        //            else
        //            {
        //                if (Math.Abs(item.MsrtRange) > this._currRange[0][this._currRange[0].Length - 1])
        //                {
        //                    item.MsrtRange = this._currRange[1][this._currRange[1].Length - 1];

        //                    item.MsrtProtection = Math.Abs(item.MsrtRange);

        //                    return false;
        //                }
        //            }
        //            break;
        //        //-----------------------------------------------------------------------------//
        //        default:
        //            return false;
        //    }

        //    return true;
        //}

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

                            if (item.ForceTime > this._pulseMaxTime[i])
                            {
                                continue;
                            }

                            item.ForceRange = Math.Abs(item.ForceValue);

                            item.MsrtProtection = Math.Abs(item.MsrtRange);

                            if (item.MsrtProtection > this._voltPulseRange[i])
                            {
                                return false;
                            }

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

		private void ExportScriptToTxt(string script, string fileName)
		{
			string path = System.IO.Path.Combine(@"C:\", fileName + ".txt");

			using (System.IO.StreamWriter sWriter = new System.IO.StreamWriter(path))
			{
				int rowIdx = 0;

				StringBuilder strBuilder = new StringBuilder();

				string[] strArry = script.Split('\n');

				foreach (string str in strArry)
				{
					strBuilder.Append(str).AppendLine();

					rowIdx++;
				}

				sWriter.Write(strBuilder.ToString());
			}
		}

        //private bool SetAutoContactItemScript()
        //{
        //    //----------------------------------------------------------------------------------------------------
        //    // (1) Creat Auto Contact Item
        //    //----------------------------------------------------------------------------------------------------
        //    ElectSettingData data = new ElectSettingData();

        //    data = new ElectSettingData("mA", "V", "ms");

        //    data.MsrtType = EMsrtType.FIMV;

        //    data.ForceValue = this._elecDevSetting.OpenShortForceValue;

        //    data.ForceTime = 1000;

        //    data.ForceDelayTime = 0;

        //    data.MsrtRange = this._elecDevSetting.OpenShortMsrtRange;

        //    data.IsAutoForceRange = true;

        //    data.IsAutoTurnOff = true;

        //    data.ForceRange = data.ForceValue;

        //    data.MsrtProtection = data.MsrtRange;

        //    //----------------------------------------------------------------------------------------------------
        //    // (2) Find the PMUIndex, Force Range Index, Msrt Range Index
        //    //----------------------------------------------------------------------------------------------------
        //    if (!this.FindSrcAndMsrtRange(data))
        //    {
        //        return false;
        //    }

        //    //----------------------------------------------------------------------------------------------------
        //    // (3) Check Time Limit
        //    //----------------------------------------------------------------------------------------------------
        //    data.ForceTime = Math.Round((data.ForceTime), 6, MidpointRounding.AwayFromZero);

        //    if (data.ForceTime > MAX_DELAY_TIME * 1000 || data.ForceTime < MIN_DELAY_TIME * 1000)
        //    {
        //        return false;
        //    }

        //    data.ForceDelayTime = Math.Round((data.ForceDelayTime), 6, MidpointRounding.AwayFromZero);

        //    if (data.ForceDelayTime > MAX_DELAY_TIME * 1000 || data.ForceDelayTime < MIN_DELAY_TIME * 1000)
        //    {
        //        return false;
        //    }

        //    //----------------------------------------------------------------------------------------------------
        //    // (4) Set Open Short Parameter into K2600 Device
        //    //----------------------------------------------------------------------------------------------------
        //    this.SetAutoContactScript(data);

        //    return !this.GetErrorMsg();
        //}

        private bool CheckProtectionModuleSpec(ElectSettingData item)
        {
            if (!this._elecDevSetting.IsEnableProtectionModule)
            {
                return true;
            }

            if (item.KeyName.Contains("IF"))
            {
                if (item.ForceValue > 1.0d)
                {
                    return false;
                }

                if (item.ForceValue > 0.50d && item.ForceTime > 0.0050d)
                {
                    return false;
                }
            }

            if (item.KeyName.Contains("IFWLA"))
            {
                if (item.ForceValue > 1.0d)
                {
                    return false;
                }

                if (item.ForceValue > 0.50d && (item.ForceTime + item.ForceTimeExt) > 0.030d)
                {
                    return false;
                }
            }

            return true;
        }

		#endregion

        #region >>> Public Method <<<

        public bool Init()
		{
			string connInfo = string.Empty;

            this._conn.Close();

			if (!this._conn.Open(out connInfo))
			{
                if (!this._conn.Open(out connInfo))
                {
                    Console.WriteLine("[K2600Device], Open(), Open Fail");

                    return false;
                }
			}

			if (!this.GetDeviceInfomation())
			{
				Console.WriteLine("[K2600Device], QueryIDN(), QueryIDN Fail");

				return false;
			}

            this.ClearBuffer();

			if (!this.SetConfig())
			{
				Console.WriteLine("[K2600Device], ConfigDevice(), ConfigDevice Fail");

				return false;
			}

            //if (!this.SetAutoContactItemScript())
            //{
            //    Console.WriteLine("[K2600Device], SetAutoContactItemScript(), SetAutoContactItemScript Fail");

            //    return false;
            //}

			return true;
		}

		public void Close()
		{
			string cmd = string.Empty;

			cmd = "setOutput(0)\n";

            // Local
			cmd += "display.sendkey(75)";

			this._conn.SendCommand(cmd);

			this._conn.Close();
		}

		public void Reset()
		{
			string cmd = string.Empty;

			cmd += "status.reset()\n";

			cmd += "opc()\n";

			this._conn.SendCommand(cmd);

			this.ClearBuffer();
		}

		public void TurnOff()
		{
			string cmd = string.Empty;

            if (_elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd)
            {
                cmd += "setLevelI(0)\n";

                cmd += "setLevelV(0)";
            }
            else
            {
                cmd += "if getFunc() ~= 1 then setFunc(1) end\n";

                cmd += "setLevelV(0)\n";

                cmd += "setOutput(0)";
			}

			this._conn.SendCommand(cmd);
		}

        public void TurnOff(double delay, bool isOpenRelay)
		{
			string cmd = string.Empty;

            if (_elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd)
            {
                cmd += "setLevelI(0)\n";

                cmd += "setLevelV(0)\n";
            }
            else
            {
                cmd += "if getFunc() ~= 1 then setFunc(1) end\n";

                cmd += "setLevelV(0)\n";

                if (isOpenRelay)
                {
                    cmd += "setOutput(0)\n";
                }
			}

			cmd += "delay(" + (delay / 1000.0d).ToString() + ")";

			this._conn.SendCommand(cmd);
		}

		public byte InputB(uint point)
		{
			byte result = 0x00;

			string script = string.Empty;

			string readStr = string.Empty;

			script = "print(digio.readport())";

			this._conn.SendCommand(script);

			if (this._conn.WaitAndGetData(out readStr))
			{
				if (readStr != string.Empty)
				{
					uint tempData = (uint)Convert.ToDouble(readStr);

					result = (byte)((tempData >> 7) & 0x07);
				}
			}

			return result;
		}

        public void MeterOutput(uint index)
		{
			string script = string.Empty;

			script += "num_" + index.ToString() + ".run()";

			this._conn.SendCommand(script);
		}

        public string[] AcquireMsrtData()
        {
            return GetDevicePrintValueToArray(',');
        }

        public string[] AcquireMsrtDataAtOnce(EMsrtType type)
        {
            string msrtFunc;

            if (type == EMsrtType.MI)
            {
                msrtFunc = "i";
            }
            else
            {
                msrtFunc = "v";
            }

            string script = "print(mrtA." + msrtFunc + "())";

            this._conn.SendCommand(script);

            return GetDevicePrintValueToArray(',');
        }

		public double ContactTest()
		{
			string cmd = "";

			cmd = "ShortTest.run()";

			this._conn.SendCommand(cmd);

			string msg = string.Empty;

			if (this._conn.WaitAndGetData(out msg))
			{
				double data = 0.0d;

				Double.TryParse(msg, out data);

				return data;
			}
			else
			{
				return 0.0d;
			}
		}

        public void ClearBuffer()
        {
            string cmd = string.Empty;

            cmd += "errorqueue.clear()\n";

            cmd += "smua.nvbuffer1.clear()\n";

            cmd += "smua.nvbuffer2.clear()\n";

            cmd += "smua.nvbuffer1.appendmode = 1\n";

            cmd += "smua.nvbuffer2.appendmode = 1";

            cmd += "smua.nvbuffer1.collecttimestamps = 0\n";

            cmd += "smua.nvbuffer2.collecttimestamps = 0\n";

            cmd += "smua.nvbuffer1.collectsourcevalues = 0\n";

            cmd += "smua.nvbuffer2.collectsourcevalues = 0";

            if (this._isDualSMU)
            {
                cmd += "\n smub.nvbuffer1.clear()\n";

                cmd += "smub.nvbuffer2.clear()\n";

                cmd += "smub.nvbuffer1.appendmode = 1\n";

                cmd += "smub.nvbuffer2.appendmode = 1";

                cmd += "smub.nvbuffer1.collecttimestamps = 0\n";

                cmd += "smub.nvbuffer2.collecttimestamps = 0\n";

                cmd += "smub.nvbuffer1.collectsourcevalues = 0\n";

                cmd += "smub.nvbuffer2.collectsourcevalues = 0";
            }

            this._conn.SendCommand(cmd);
        }

        public bool SetDCItem(uint index, ElectSettingData item)
        {
            if (!this.CheckDCModeElectSetting(item))
            {
                Console.WriteLine("[K2600Device], SetDCItem(), CheckDCModeElectSetting Fail");
                return false;
            }

			this.SetDCMsrtItemScript(index, item);

            return !this.GetErrorMsg();
        }

        public bool SetPulseItem(uint index, ElectSettingData item)
        {
            int region = -1;

            if (!this.CheckPulseModeElectSetting(item, out region))
            {
                Console.WriteLine("[K2600Device], SetPulseItem(), CheckPulseModeElectSetting Fail");
                return false;
            }

            //if (region == -1)
            //{
            //    this.SetPulseIMsrtItemScript(index, item);
            //}
            //else
            //{
            //    this.SetPulseIMsrtItemScript(index, item, region);
            //}
  
            this.SetPulseIMsrtItemScript(index, item, region);

            return !this.GetErrorMsg();
            }

        public bool SetArmPulseItem(uint index, ElectSettingData item)
        {
            int region = -1;

            if (!this.CheckPulseModeElectSetting(item, out region))
            {
                Console.WriteLine("[K2600Device], SetPulseItem(), CheckPulseModeElectSetting Fail");
                return false;
            }

            this.SetPulseIMsrtItemScript(index, item, region);

            return !this.GetErrorMsg();
        }

        public bool SetRItem(uint index, ElectSettingData item)
        {
            if (!this.CheckDCModeElectSetting(item))
            {
                return false;
            }

            this.SetRMsrtItemScript(index, item);

            return !this.GetErrorMsg();
        }

		public bool SetContactCheckItem(uint index, ElectSettingData item)
		{
			this.SetContactCheckItemScript(index, item);

			return !this.GetErrorMsg();
		}


        public bool SetSweepItem(uint index, ElectSettingData item)
        {
            if (!this.CheckDCModeElectSetting(item))
            {
                return false;
            }

            if (!this.SetSweepListScript(index, item))
            {
                return false;
            }

            this.SetSweepMsrtItemScript(index, item);

            return !this.GetErrorMsg();
        }

        public bool SetPIVItem(uint index, ElectSettingData item)
        {
            if (!this.CheckDCModeElectSetting(item))
            {
                return false;
            }

            if (!this.SetSweepListScript(index, item))
            {
                return false;
            }

            this.SetPIVMsrtItemScript(index, item);

            return !this.GetErrorMsg();
        }

        public bool SetPulsePIVItem(uint index, ElectSettingData item)
        {
            int region = -1;

            if (!this.CheckPulseModeElectSetting(item, out region))
            {
                return false;
            }

            if (!this.SetSweepListScript(index, item))
            {
                return false;
            }

            if (this._isDualSMU)
            {
                this.SetPulsePIVMsrtItemScript_Daul(index, item, region);
            }
            else
            {
            this.SetPulsePIVMsrtItemScript(index, item, region);
            }

            return !this.GetErrorMsg();
        }

        public bool SetTHYItem(uint index, ElectSettingData item, bool isMsrt)
        {
            if (!this.CheckDCModeElectSetting(item))
            {
                return false;
            }

            if (isMsrt)
            {
                this.SetTHYItemAndMsrtScript(index, item);
            }
            else
            {
                this.SetTHYItemScript(index, item);
            }

            return !this.GetErrorMsg();
        }

    //    public bool SetRTHItem(uint index, ElectSettingData item, bool isMsrt)
    //    {
    //        if (!this.CheckDCModeElectSetting(item))
    //        {
    //            return false;
    //        }

    //        if (isMsrt)
    //        {
    //            this.SetRTHMsrtItemAndMsrtScript(index, item);
    //        }
    //        else
    //        {
				//this.SetRTHMsrtItemScript(index, item);
    //        }

    //        return !this.GetErrorMsg();
    //    }

		public bool SetRTHItem(uint index, ElectSettingData item)
		{
            if (!this.CheckDCModeElectSetting(item))
			{
				return false;
			}

			this.SetRTHHeatItemScript(index, item);

			return !this.GetErrorMsg();
		}

        //public bool SetVLRItem(uint index, ElectSettingData item)
        //{
        //    if (!this.CheckDCModeElectSetting(item))
        //    {
        //        return false;
        //    }

        //    this.SetVLRMsrtItemScript(index, item);

        //    return !this.GetErrorMsg();
        //}

        public bool SetScanItem(uint index, ElectSettingData item)
        {
            if (!this.CheckDCModeElectSetting(item))
            {
                return false;
            }

            this.SetScanItemAndMsrtScript(index, item);

            return !this.GetErrorMsg();
        }

        public bool SetLopItem(uint index, ElectSettingData item)
        {
            if (!this._elecDevSetting.IsDetectorHwTrig)
            {
                item.IsAutoTurnOff = false;
            }

            if (item.MsrtType != EMsrtType.LIV && item.MsrtType != EMsrtType.LVI)
            {
            if (this._isDualSMU)
            {
                item.IsAutoTurnOff = true;
            }
            }

            if (!this.CheckDCModeElectSetting(item))
            {
                return false;
            }

            this.SetLOPMsrtItemScript(index, item);

            return !this.GetErrorMsg();
        }

		public bool SetLCRItem(uint index, ElectSettingData item)
		{
			if (!this.CheckDCModeElectSetting(item))
			{
				return false;
			}

			this.SetLCRItemScript(index, item);

			return !this.GetErrorMsg();
		}

        public bool SetLopItemToSlaveDevice(uint index, ElectSettingData item)
        {
            this.SetSlaveLOPMsrtItemScript(index, item, this._elecDevSetting.IsDetectorHwTrig);

            return !this.GetErrorMsg();
        }

        public bool SetPIVItemToSlaveDevice(uint index, ElectSettingData item)
        {
            //if (!this.CheckDCModeElectSetting(item))
            //{
            //    return false;
            //}

            this.SetSlavePIVMsrtItemScript(index, item);

            return !this.GetErrorMsg();
        }

        public bool SetPulsePIVItemToSlaveDevice(uint index, ElectSettingData item)
        {
            //if (!this.CheckDCModeElectSetting(item))
            //{
            //    return false;
            //}

            this.SetSlavePulsePIVMsrtItemScript(index, item);

            return !this.GetErrorMsg();
        }

        public string[] GetTHYTimestanps(uint index, ElectSettingData item)
        {
            string script = "";

            string srcRngAndComplScript = "";

            double complLimit = 0.0d;

            if (item.MsrtType == EMsrtType.THY)
            {
                complLimit = this._voltRange[0][this._voltRange[0].Length - 1];
            }

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (item.MsrtProtection <= complLimit)
            {
                srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(0.000001)\n";
            }
            else
            {
                srcRngAndComplScript += "setSorceRangeI(0.000001)\n";

                srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
            }

            script = "loadscript num_" + index.ToString() + "Timestamps\n";

            script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";

            int measureCount = ((int)item.SweepContCount + 1) / 2;

            if (measureCount < 100)
            {
                measureCount = 100;
            }

            script += "setMsrtCount(" + measureCount.ToString() + ")\n";

            script += "setMsrtInterval(0.000001)\n";

            script += "setBufTimestamps(1)\n";

            script += "if getFunc ~= 0 then setFunc(0) end\n";

            script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";

            script += "setMeasureRangeI(0.000001)\n";

            script += srcRngAndComplScript;

            script += "setLevelI(0.00000)\n";

            script += "mrtA.v(bufA1)\n";

            script += "for i=1, " + measureCount.ToString() + " do\n";

            script += "print(bufA1.timestamps[i])\n";

            script += "end\n";

            script += "setNPLC(0.01)\n";

            script += "setMsrtCount(1)\n";

            script += "bufA1.clear()\n";

            script += "setBufTimestamps(0)\n";

            script += "endscript\n";

            script += "num_" + index.ToString() + "Timestamps.source = nil";

            this._conn.SendCommand(script);

            this._conn.SendCommand("num_" + index.ToString() + "Timestamps.run()");

            string returnString = string.Empty;

            string[] returnStrData = null;

            if ((this._conn as LANConnect).WaitAndGetData(out returnString, measureCount))
            {
                returnString = returnString.TrimEnd('\n');

                returnStrData = returnString.Split('\n');
            }

            return returnStrData;
        }

        public void OutputOn()
        {
			//if (_elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd)
			//{
			//    this._conn.SendCommand("setOutput(1)");
			//}
        }

        public void OutputOff()
        {
            this._conn.SendCommand("setOutput(0)");
        }

        public bool ReadPortStatus(uint pin)
        {
            // True --> High
            // False --> Low
            byte result = 0x00;

            string script = string.Empty;

            string readStr = string.Empty;

            script = "print(digio.readport())";

            this._conn.SendCommand(script);

            if (this._conn.WaitAndGetData(out readStr))
            {
                if (readStr != string.Empty)
                {
                    uint tempData = (uint)Convert.ToDouble(readStr);

                    result = (byte)((tempData >> (int)pin) & 0x01);
                }
            }

            if (result > 0)
            {
                return true;
            }

            return false;
        }

        public void Abort()
        {
            string cmd = "smua.abort()";

            this._conn.SendCommand(cmd);
        }

        public void TriggerDAQ(uint index)
        {
            this._conn.SendCommand("num_" + index.ToString() + "_DAQTrigger.run()");
        }

        public void CollectGarbage()
        {
            string cmd = "collectgarbage()";

            this._conn.SendCommand(cmd);
        }

        public void RunFunctionFVMI(double waitTime, double forceValue, double forceTime, double msrtRange, double nplc)
        {
            string func = string.Empty;

            func = string.Format("funcFVMI({0}, {1}, {2}, {3}, {4})", waitTime, forceValue, forceTime, msrtRange, nplc);

            this._conn.SendCommand(func);
        }

        public void RunFunctionFI(double waitTime, double forceValue, double forceTime, double msrtRange, double nplc)
        {
            string func = string.Empty;

            func = string.Format("funcFI({0}, {1}, {2}, {3}, {4})", waitTime, forceValue, forceTime, msrtRange, nplc);

            this._conn.SendCommand(func);
        }

        public void RunFunctionFIMV(double waitTime, double forceValue, double forceTime, double msrtRange, double nplc, bool isTurnOff)
        {
            string func = string.Empty;

            int turnOff = isTurnOff ? 1 : 0;

            func = string.Format("funcFIMV({0}, {1}, {2}, {3}, {4}, {5})", waitTime, forceValue, forceTime, msrtRange, nplc, turnOff);

            this._conn.SendCommand(func);
        }

        public void RunFunctionPulseIMsrtV(double pulseLevel, double pulseWidth, double period, uint pulseCnt, double msrtRange, double nplc)
        {
            string func = string.Empty;

            func = string.Format("funcPulseIMsrtV({0}, {1}, {2}, {3}, {4})", pulseLevel, pulseWidth, period, pulseCnt, msrtRange);

            this._conn.SendCommand(func);
        }

		#endregion

		#region >>> Public Proberty <<<

		public string SerialNumber
		{
            get { return this._serialNum; }
		}

		public string SoftwareVersion
		{
            get { return this._swVersion; }
		}

		public string HardwareVersion
		{
            get { return this._hwVersion; }
		}

        public string Name
        {
            get { return this._name; }
        }

        public double[] SweepPoints
        {
            get { return this._sweepList.ToArray(); }
        }

        public double[][] CurrentRange
        {
            get { return this._currRange; }
        }

        public double[][] VoltageRange
        {
            get { return this._voltRange; }
        }

        public bool IsDualChannel
        {
            get { return this._isDualSMU; }
        }

        public double[] CurrentPulseRange
        {
            get { return this._currPulseRange; }
        }

        public double[] VoltagePulseRange
        {
            get { return this._voltPulseRange; }
        }

        public double[] PulseWidth
        {
            get 
            {
                if( this._pulseMaxTime == null)
                {
                    return null;
                }
                
                double[] convertWidth = this._pulseMaxTime.Clone() as double[];

                for (int i = 0; i < convertWidth.Length; i++)
                {
                    convertWidth[i] *= 1000.0d; // s -> ms
                }

                return convertWidth; 
            }
        }

        public double[] PulseDuty
        {
            get
            {
                if (this._pulseDuty == null)
                {
                    return null;
                }

                double[] convertDuty = this._pulseDuty.Clone() as double[];

                for (int i = 0; i < convertDuty.Length; i++)
                {
                    double duty = convertDuty[i] * 100.0d;

                    duty = Math.Round(duty, 2, MidpointRounding.AwayFromZero);

                    convertDuty[i] = duty; //  -> %
                }

                return convertDuty; 
            }
        }

		#endregion

		#region >>> Enumeration Define <<<

		private enum ESenseMode : int
		{
			LOCAL = 0,

			REMOTE = 1,

			CAL = 3,
		}

		private enum ESrcSettling : int
		{
			SMOOTH = 0,

			FAST_RANGE = 1,

			FAST_POLAR = 2,

			DIRECT_IRANGE = 3,

			SMOOTH_100NA = 4,

			FAST_ALL = 128,
		}

		#endregion
    }
}
