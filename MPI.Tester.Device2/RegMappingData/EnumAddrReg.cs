using System;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public enum EAddr : uint
	{
		//----------------------------------------------------------------
		// Address mapping for write data to resigtor 
		//----------------------------------------------------------------
        CMD_ID					= 0x0010,
        PARAMETER				= 0x0011,
        SWEEP_TABLE				= 0x0060,     

		//----------------------------------------------------------------
		// Address mapping for read data from resigtor 
		//----------------------------------------------------------------
        DEVICE_ID				  = 0x1000,
        FIRMWARE_VERSION          = 0x114A,
       
        DEVICE_STATUS			  = 0x1200,
        ERROR_CODE                = 0x1201,
        COMPARED_STATE			  = 0x1202,
        TRIGGER_IN_DATA			  = 0x1203,
        DIO_INPUT				  = 0x1204,
        TRIGGER_IN_DATA_1         = 0x1205,

		DC_RESULT_HEAD			  = 0x1210,
		DC_RESULT_DATA			  = 0x1218,
		RESULT_SWEEP_TABLE	      = 0x1384,

        SWEEP_RESULT_DATA         = 0x14E0,

        SRAM_TEST_ITEMS_SETTING   = 0x6000,
        GAIN_OFFSET_START_ADDRESS = 0x9000,
	}

    public enum EMeterCmdID : ushort
    {
        SetTestItem           = 0x2710,
        SetTriggerOutDuration = 0x2711,
        SetVoltComp           = 0x2712,
        SetCurrComp           = 0x2713,
        SetGainOffset         = 0x2714,
        SetVoltTestItem       = 0x2715,
        SetCurrTestItem       = 0x2716,
        SetEEPROMSave         = 0x2717,

        RunWait               = 0x4E23,
        RunTestItem           = 0x4E24,
        RunVoltComp           = 0x4E25,
        RunCurrComp           = 0x4E26,
        RunTriggerOut         = 0x4E27,
        RunTriggerIn          = 0x4E28,
        RunTriggerIn_1        = 0x9761,
        RunDioOutput          = 0x4E29,

        RunAutoCalEnable      = 0x9766,
        RunSourceReset        = 0x9765,
        RunTurnOffRangeChange = 0x9764,

        ResetSystem           = 0x4E21,
        ClearTestItem         = 0x4E22,

        StopVoltComp          = 0x4E2A,
        StopCurrComp          = 0x4E2B,
        StopTestItem          = 0x4E2C,
        RunDioInput           = 0x4E2D,
        RunMemoryClear        = 0x4E2E,
        SpRunTestItem         = 0x5E01,
    }

    public enum EPMU : ushort
    {
        Normal_Mode = 0x0001,
        HV_Mode     = 0x0002,
    }

    public enum ERegMsrtType : ushort
    {
        FVMI      = 0x0003,
        FIMV      = 0x0004,
        THY       = 0x0005,
        FIMVSWEEP = 0x0006,
        FVMISWEEP = 0x0007,
        FVMV      = 0x000B,
        FIMI      = 0x000C,
    }   

    public enum EIRange : ushort
    {
        _5uA			= 0x0000,
        _10uA			= 0x0001,
        _100uA			= 0x0002,
        _1mA			= 0x0003,
        _10mA			= 0x0004,
        _100mA			= 0x0005,
        _500mA			= 0x0006,
        _1A				= 0x0007,
        _2A				= 0x0008,
    }

    public enum EVRange : ushort
    {
        _5V				= 0x0020,
        _10V			= 0x0021,
        _20V			= 0x0022,
        _50V			= 0x0023,
        _80V			= 0x0024,   //  F*65535 / 100 (uint: V)
    }

    public enum ECompareType : ushort
    {
        Small			= 1,
        Bigger			= 2,
    }  

    public enum EPolarity : ushort
    {
        Positive		= 0x0001,
        Negitive		= 0x0002,
    }

    public enum ESourceSpeed : ushort
    {
        High			= 0x0000,
        Normal			= 0x0001,
        Slow			= 0x0002,
        VerySlow		= 0x0003,
    }

    public enum ERegSweepMode : ushort
    {
        None				= 0x0000,
        LinearIncrease_1	= 0x0001,
        LinearIncrease_2	= 0x0002,
        THY_1				= 0x0003,
        THY_2				= 0x0004,
        Sweep_1				= 0x0005,
        Sweep_2				= 0x0006,
    }

    public enum ENplcCount : ushort
    {
        _60Hz_00		= 0x1000,

        _60Hz_01		= 0x1001,
        _60Hz_02		= 0x1002,
        _60Hz_03		= 0x1003,
        _60Hz_04		= 0x1004,
        _60Hz_05		= 0x1005,

        _60Hz_06		= 0x1006,
        _60Hz_07		= 0x1007,
        _60Hz_08		= 0x1008,
        _60Hz_09		= 0x1009,
        _60Hz_10		= 0x100A,

        _50Hz_00		= 0x2000,

		_50Hz_01		= 0x2001,
        _50Hz_02		= 0x2002,
        _50Hz_03		= 0x2003,
        _50Hz_04		= 0x2004,
        _50Hz_05		= 0x2005,

        _50Hz_06		= 0x2006,
        _50Hz_07		= 0x2007,
        _50Hz_08		= 0x2008,
        _50Hz_09		= 0x2009,
        _50Hz_10		= 0x200A,
    }

    public enum ERegAutoCalApply : ushort
    {
        Disable			= 0x0000,
        EEPROMcopy		= 0x0001,
        bySW		    = 0x0002,
        byFW		    = 0x0003,
    }

    public enum ETriggerInLatchType : ushort
    {
        Rising_Edge_Trigger		= 0x0001,
        Level_Trigger			= 0x0002,
        Falling_Edge_Trigger	= 0x0003,
    }

    public enum ETriggerOutIndex : ushort
    {
        All_Low     = 0x0000,
        All_High    = 0x0100,
        Default     = 0x000F,
        Pin01       = 0x0001,   // VoltComp
        Pin02       = 0x0002,   // CurrComp
        Pin03       = 0x0003,  // Spectrometer
        Pin04       = 0x0004,  // ExtTrigger
    }

    public enum ETrigSetting : ushort
    {
        None            = 0x0000,
        Pin01_Low		= 0x0001,
        Pin02_Low		= 0x0002,
        Pin03_Low		= 0x0003,
        Pin04_Low		= 0x0004,

        Pin01_High		= 0x0101,
        Pin02_High		= 0x0102,
        Pin03_High		= 0x0103,
        Pin04_High		= 0x0104,
    }

    public enum ETurnOffRange : ushort
    {
        Auto    = 0x0000,
        Fix     = 0x0001,   // for AutoCal
        Default = 0x0002,   // for ESD  
    }

}
