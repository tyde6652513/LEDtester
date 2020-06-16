using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.LDT3ALib
{
	public enum E_VRange : int
	{
		_6V		= 0x40,
		_20V	= 0x60,
		_200V	= 0xA0,
	}

	public enum E_IRange : int
	{
		_1uA	= 0x001E,		// 30		//  數字累加遞增，Enum 的 Names 排列才會依序
		_10uA	= 0x011E,		// 286
		_100uA	= 0x021C,		// 540
		_1mA	= 0x031C,		// 796
		_10mA	= 0x041A,		// 1050
		_100mA	= 0x051A,		// 1306
		_800mA	= 0x0616,		// 1558
		_2A		= 0x070E,		// 1806
		_3A		= 0x0806,		// 2054
	}

	public enum EStatus : int
	{ 
		WIRE2_FAN_OFF	= 0x00,
		WIRE2_FAN_ON	= 0x02,
		WIRE4_FAN_OFF	= 0x04,
		WIRE4_FAN_ON	= 0x06,
	}

	public enum ERegCmd : byte
	{
		ReadAD0MA	= 0x30,
		ReadAD1MA	= 0x31,
		ReadAD2MA	= 0x32,
		ReadAD0		= 0x33,
		ReadAD1		= 0x34,
		ReadAD2		= 0x35,
		ReadADMA	= 0x36,
		ReadAD		= 0x37,
		ReadDI		= 0x38,

		ReadCurve		= 0x43,
		SetInternalIO	= 0x44,
		ReadAndWriteInternalIO		= 0x45,
		ReadAndWriteFlash_nMemory	= 0x46,

		SetCH2Gain	= 0x47,
		SetNPLC		= 0x4E,
		WriteDO		= 0x4F,
		SetSequence = 0x50,

		StopSequence	= 0x53,
		SetTimeBase		= 0x54,
		Read_sVersion	= 0x56,

		SetParameter	= 0x70,
		SetStatus		= 0x73,

		GetThyData		= 0x74,
	}

	public enum ERegRange : byte
	{
		_1uA	= 0x1E,
		_10uA	= 0x1E,
		_100uA	= 0x1C,
		_1mA	= 0x1C,
		_10mA	= 0x1A,
		_100mA	= 0x1A,
		_800mA	= 0x16,
		_2A		= 0x0E,
		_3A		= 0x06,

		_6V		= 0x40,
		_20V	= 0x60,
		_200V	= 0xA0,
	}

    public enum ERtnValue : int
    {
        IFN = 0,
        VSP = 1,
        VSN = 2,
        VDD = 3,
        VDB = 4,
    }

    public enum ERtnFwCalcThyValue : int
    {
        VDD_Peak   = 0,
        VDD_Stable = 1,
        VDD_Diff   = 2,
        VDD_SpecificPoint = 3,
    }

    public enum ERegADChannel : byte
    {
        IFN = 0,
        VSP = 1,
        VSN = 2,
    }
}
