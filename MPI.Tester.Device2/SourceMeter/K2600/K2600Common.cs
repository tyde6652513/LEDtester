using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.KeithleySMU
{
    public static class K2600Const
    {
        public const string NAME_SMUA = "A";
        public const string NAME_SMUB = "B";

        public const int INDEX_TERMINAL = 999998;
        public const int INDEX_TERMINAL_PD = 999999;

        public const int ID_SMUA = 0;
        public const int ID_SMUB = 1;
        public const double IO_DEFUALT_PULSE_WIDTH = 0.005;


        #region >>> I/O Ping Define <<<

        // Enable二極體:		PIN_RTH_EANBLE = 0     
        public const uint IO_SPT_TRIG_OUT = 1;
        public const uint IO_PM_RELAY1 = 2;
        public const uint IO_PM_RELAY2 = 3;
        public const uint IO_SMU_ABORT_IN = 4;
        public const uint IO_SMU_TRIG_IN = 11;
        public const uint IO_FRAME_GROUND = 5;
        public const uint IO_DAQ_ENABLE = 6;
        public const uint IO_RTH_EANBLE = 7;
        public const uint IO_CAP_SW = 12;
        public const uint IO_DAQ_TRIG_OUT = 13;
        public const uint IO_POLAR_SW = 14;

        public const uint IO_AutoShiftFilter = 4;
        
        #endregion
    }
    
    public static class K2600Spec
    {
        #region >>> DC Range <<<

        public static double[][] DC_V_RANGE_K2601 = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.1d,   1.0d,   6.0d },
                                                    new double[] { 40.0d },
												};

        public static double[][] DC_I_RANGE_K2601 = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d, 1.0d, 3.0d },  
                                                    new double[] { 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d, 1.0d },
												};

        public static double[][] DC_V_RANGE_K2611 = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.20d,   2.0d,   20.0d },
                                                    new double[] { 200.0d },
												};

        public static double[][] DC_I_RANGE_K2611 = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d, 1.0d, 1.5d },  
                                                    new double[] { 100e-9d, 1e-6d, 10e-6d, 100e-6d, 1e-3d, 10e-3d, 100e-3d,  },
												};

        public static double[][] DC_V_RANGE_K2635 = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.20d,   2.0d,   20.0d },
                                                    new double[] { 200.0d },
												};

        public static double[][] DC_I_RANGE_K2635 = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-12d, 1e-9d,  10e-9d,  100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  1.5d },  
                                                    new double[] { 100e-12d, 1e-9d,  10e-9d,  100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  },
												};

        public static double[][] DC_V_RANGE_K2651 = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.10d,   1.0d,   10.0d },
                                                    new double[] { 20.0d },
                                                    new double[] { 40.0d },
												};

        public static double[][] DC_I_RANGE_K2651 = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  5.0d,   10.0d,   20.0d, }, 
                                                    new double[] { 100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  5.0d,   10.0d, },
                                                    new double[] { 100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  5.0d, }, 
												};


        #endregion

        #region >>> Pulse Range <<<

        public static double[] PULSE_V_RANGE_K2611 = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													5.0d,
                                                    180.0d,
													200.0d 
												};

        public static double[] PULSE_I_RANGE_K2611 = new double[]	// [Index ], unit = I
												{	
													10.0d,
                                                    1.0d,
													1.0d
												};

        public static double[] MAX_PULSE_WIDTH_K2611 = new double[]	// [Index ], unit = s
												{	
													0.0010d,	//1.0ms
                                                    0.00850d,	//8.5ms
													0.00220d	//2.2ms
												};

        public static double[] MAX_PULSE_DUTY_K2611 = new double[]	// [Index ], unit =
												{	
													0.0220d,	//2.2%
                                                    0.010d,		//1.0%
													0.010d		//1.0%
												};

        public static double[] PULSE_V_RANGE_K2635 = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
                                                    5.0d,
                                                    180.0d,
													200.0d 
												};

        public static double[] PULSE_I_RANGE_K2635 = new double[]	// [Index ], unit = I
												{	
                                                    10.0d,
                                                    1.0d,
													1.0d
												};

        public static double[] MAX_PULSE_WIDTH_K2635 = new double[]	// [Index ], unit = s
												{	
                                                    0.0010d,	//1.0ms
                                                    0.00850d,	//8.5ms
													0.00220d	//2.2ms
												};

        public static double[] MAX_PULSE_DUTY_K2635 = new double[]	// [Index ], unit =
												{	
                                                    0.0220d,	//2.2%
                                                    0.010d,		//1.0%
													0.010d		//1.0%
												};

        public static double[] PULSE_V_RANGE_K2651 = new double[]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													10.0d,
                                                    20.0d,
													40.0d,
 													10.0d,
                                                    20.0d,
													40.0d
												};

        public static double[] PULSE_I_RANGE_K2651 = new double[]	// [Index ], unit = I
												{	
													30.0d,
                                                    20.0d,
													10.0d,
													50.0d,
                                                    50.0d,
													50.0d
												};

        public static double[] MAX_PULSE_WIDTH_K2651 = new double[]	// [Index ], unit = s
												{	
													0.0010d,	//1.0ms
                                                    0.00150d,	//1.5ms
													0.00150d,	//1.5ms
													0.0010d,	//1.0ms
                                                    0.000330d,	//330us
													0.000330d	//330us
												};

        public static double[] MAX_PULSE_DUTY_K2651 = new double[]	// [Index ], unit =
												{	
													0.50d,	//50%
                                                    0.50d,	//50%
													0.40d,	//40%
													0.350d,	//35%
                                                    0.10d,	//10%
													0.010d	// 1%
												};

        #endregion
    }

    public class K2600ScriptSetting : ICloneable
    {
        public uint Index = 0;
        public string KeyName = string.Empty;
       
        public K2600SmuSetting SMUA = new K2600SmuSetting();
        public K2600SmuSetting SMUB = new K2600SmuSetting();

        public bool IsTurnOffToDefaultRange = false;
        public bool IsTurnOffToZeroVolt = true;

        // Special Setting
        public bool IsSpReverseCurrentRange = false;
        public double SpReverseCurrentApplyRange = 0.01;
        public bool IsSpFwCalcResult = false;

        public bool IsEnableBriefScript = false;

        // IO
        public EK2600ProtectionModuleState SetProtectionModuleStatus = EK2600ProtectionModuleState.KeepTheLast;
        public EK2600ProtectionModule ProtectionModuleSN = EK2600ProtectionModule.NONE;
        public EK2600ProtectionModuleResistance PMResistance = EK2600ProtectionModuleResistance.SHORT;

        public K2600ScriptSetting()
        {
      
        }

        public void Reset()
        {
            Index = 0;
            KeyName = string.Empty;
        
            SMUA.Reset();
            SMUB.Reset();

            //--------------------------------------------------------------------
            IsTurnOffToDefaultRange = false;
            IsTurnOffToZeroVolt = true;

            // Special Setting
            IsSpReverseCurrentRange = false;
            SpReverseCurrentApplyRange = 0.01;
            IsSpFwCalcResult = false;
        }
        public void ResetState()
        {
            SMUA.ResetState();
            SMUB.ResetState();
 
        }

        public object Clone()
        {
            K2600ScriptSetting obj = this.MemberwiseClone() as K2600ScriptSetting;

            if (this.SMUA != null)
            {
                obj.SMUA = null;
                obj.SMUA = this.SMUA.Clone() as K2600SmuSetting;

                obj.SMUB = null;
                obj.SMUB = this.SMUB.Clone() as K2600SmuSetting;

            }

            return obj;
        }
    }

    public class K2600SmuSetting:ICloneable
    {
        public bool IsEnableSmu = true;

        public EK2600ContactCheckSpeed ContactCheckSpeed = EK2600ContactCheckSpeed.MEDIUM;

        public double WaitTime = 0.0d;

        // Source
        public EK2600SrcMode SrcMode = EK2600SrcMode.I_Source;
        public double SrcRange = 0.0d;
        public double SrcLevel = 0.0d;

        // Delay
        public double srcTime = 0.0d;

        // Measure
        public double MsrtRange = 0.0d;
        public double MsrtClamp = 0.0d;
        public double MsrtNPLC = 0.0d;

        public double MsrtBoundryLimit;
        public bool IsAutoMsrtRange = false;

        public bool IsEnableMsrt = true;
        public bool IsEnableMsrtSrcLevel = false;

        // TurnOFF
        public bool IsAutoTurnOff = true;
        public double TurnOffTime = 0.0d;

        //------------------------------------------------
        // Sweep

        public double SweepStartHoldTime = 0.0d;
        public EK2600SweepMode SweepMode = EK2600SweepMode.Linear;
        public double SweepStart = 0.0d;
        public double SweepStep = 0.0d;
        public double SweepStop = 0.0d;
        public bool IsSweepFirstElec = true;
        public bool IsSweepEnd = true;
        //-----------------------------------------------------------
        // State
        public double SrcCurrRangeSt = 0.0d;
        public double SrcVoltRangeSt = 0.0d;
        public double MsrtCurrRangeSt = 0.0d;
        public double MsrtVoltRangeSt = 0.0d;
        public double MsrtCurrClampSt = 0.0d;
        public double MsrtVoltClampSt = 0.0d;
        public double NplcSt = 0.0d;
        public int SrcModeSt = -1;


        public List<double> SweepCustomList;

        public double SweepEndPulseTurnOffTime = 0.0d;
        public EK2600EndPulseAction SweepEndPulseAction = EK2600EndPulseAction.SOURCE_IDEL;

        public uint SweepPoints = 0;

        public double Duty = 0;

        public K2600SmuSetting()
        {
            this.SweepCustomList = new List<double>();
        }

        public void Reset()
        {
            IsEnableSmu = true;

            ContactCheckSpeed = EK2600ContactCheckSpeed.MEDIUM;

            SrcMode = EK2600SrcMode.I_Source;
            SrcRange = 0.0d;
            SrcLevel = 0.0d;

            srcTime = 0.0d;

            MsrtRange = 0.0d;
            MsrtClamp = 0.0d;
            MsrtNPLC = 0.010d;

            MsrtBoundryLimit = 0;
            IsAutoMsrtRange = false;

            IsEnableMsrt = true;
            IsEnableMsrtSrcLevel = false;

            IsAutoTurnOff = true;
            TurnOffTime = 0.0d;

            SweepStartHoldTime = 0.0d;
            SweepMode = EK2600SweepMode.Linear;
            SweepStart = 0.0d;
            SweepStep = 0.0d;
            SweepStop = 0.0d;

            SweepCustomList.Clear();

            SweepEndPulseTurnOffTime = 0.0d;
            SweepEndPulseAction = EK2600EndPulseAction.SOURCE_IDEL;

            SweepPoints = 0;

            Duty = 0.0d;
        }

        public void ResetState()
        {
            SrcCurrRangeSt = 0.0d;
            SrcVoltRangeSt = 0.0d;
            MsrtCurrRangeSt = 0.0d;
            MsrtVoltRangeSt = 0.0d;
            MsrtCurrClampSt = 0.0d;
            MsrtVoltClampSt = 0.0d;
            NplcSt = 0.0d;
            SrcModeSt = -1;
        }

        public object Clone()
        {
            double[] valArr = null;
            if (this.SweepCustomList != null)
            {valArr = this.SweepCustomList.ToArray();}
            K2600SmuSetting obj = this.MemberwiseClone() as K2600SmuSetting;
            if (this.SweepCustomList != null)
            {
                this.SweepCustomList = null;                
                this.SweepCustomList = new List<double>();
                this.SweepCustomList.AddRange(valArr);

                obj.SweepCustomList = null;
                obj.SweepCustomList = new List<double>();
                obj.SweepCustomList.AddRange(valArr);
            }
            return obj;
        }
    }

    #region >>> Enumeration Define <<<

    public enum EK2600SrcMode : int
    {
        I_Source = 0,
        V_Source = 1,
    }

    public enum EK2600SrcSettling : int
    {
        SMOOTH = 0,
        FAST_RANGE = 1,
        FAST_POLAR = 2,
        DIRECT_IRANGE = 3,
        SMOOTH_100NA = 4,
        FAST_ALL = 128,
    }

    public enum EK2600SenseMode : int
    {
        LOCAL = 0,
        REMOTE = 1,
        CAL = 3,
    }

    public enum EK2600ContactCheckSpeed : uint
    {
        FAST = 0,
        MEDIUM = 1,
        SLOW = 2,
    }

    public enum EK2600SweepMode : uint
    {
        Linear = 0,
        Log    = 1,
        Custom = 2,
    }

    public enum EK2600EndPulseAction : uint
    {
        SOURCE_IDEL = 0,
        SOURCE_HOLD = 1,
    }

    public enum EK2600IOTriggerSynMode : int
    {
        Single = 0,
        Master = 1,
        Slave = 2,
    }

    public enum EK2600ProtectionModuleState : int
    {
        KeepTheLast = 0,
        ON = 1,
        OFF = 2,
    }

    public enum EK2600ProtectionModule : int
    {
        NONE = 0,
        ATV_PM2 = 1,
        MPI_KPM = 2,
    }

    public enum EK2600ProtectionModuleResistance : int
    {
        SHORT = 0,
        R1 = 1,
        R2 = 2,
        R3 =3,
    }

    #endregion

}
