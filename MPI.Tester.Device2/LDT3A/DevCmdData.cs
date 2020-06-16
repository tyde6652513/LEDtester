using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.LDT3ALib
{
	public class LDevConst
	{
		public const float OP6VRangeOut = 10.0f;
		public const float OP20VRangeOut = 24.0f;
		public const float OP200VRangeOut = 216.0f;
		public const float CMP_ex_RefVolt = 3.0f;

		public const uint MAX_CURVE_POINTS = 2000;
		public const uint MAX_MEMORY_LENGTH = 525 + 282 + 1;  // old + new + 1
		public const uint MAX_SEQUENCE_LENGTH = 50;

        public const double FIRMWARE_TIMER_RESOLUTION = 0.01d; // unit: ms

		public static readonly UInt16[] memTableVFP = new UInt16[]		{ 0,	23,		24,		46,		94,		117,	118,	140,	188,	211,	 212,	234 };	// +6V, -6V, +20V, -20V, +200V, -200V : 24 points (Left Point)
		public static readonly UInt16[] memTableVSP = new UInt16[]		{ 47,	70,		71,		93,		141,	164,	165,	187,	235,	258,	259,	281 };	// +6V, -6V, +20V, -20V, +200V, -200V : 24 points
		public static readonly UInt16[] memTableVSN = new UInt16[]		{ 336,	339,	340,	343 };		// +VSN, -VSN : 4 points
		public static readonly UInt16[] memTableVZero = new UInt16[]	{ 288,	289 };						// +VZero, -VZero : 2 point   for VSP

		public static readonly UInt16[] memTableIFN = new UInt16[]		{ 384,		389,	392,	397,	400,	405,	408,	413,	416,	421,	424,	429,	// +1uA,	-1uA,	+10uA,	-10uA,	+100uA,		-100uA
																		  432,		437,	440,	445,	448,	453,	456,	461,	464,	469,	472,	477,	// +1mA,	-1mA,	+10mA,	-10mA,	+100mA,		-100mA
																		  480,		485,	488,	493,	496,	501,	504,	509,	512,	517,	520,	525};	// +800mA,	-800mA, +2A,		-2A,		+3A,		-3A
		public static readonly UInt16[] memTableVDB = new UInt16[]		{ 344,		352,	353,	361,	362,	368,	369,	375,	296,	302,	303,	309 };	// +6V, -6V : 9 points, -20V, +20V : 7 points, +200V, -200V : 7 points

		public static readonly double[] CalVN_Value = new double[]	{ 0.003,	0.3,	1.5,	3 };
		public static readonly double[] IFN_SegScale = new double[] { 0.0,		0.001,	0.01,	0.1,	0.5,	1.0 };

		public static readonly double[] IRANGE_SegValue = new double[]	{ 1,	1E1,	1E2,	1E3,	1E4,	1E5,	8E5,	2E6,	3E6 };   // uA
		public static readonly double[] VRANGE_SegValue = new double[]	{ 6,	20,		200};
		public static readonly double[] IRANGE_AutoValue = new double[] { 1,	1E1,	1E2,	1E3,	1E4,	1E5,	4E5,	1E6,	3E6 };		// 自動切換選擇的判斷值，不全然等於硬體的範圍

		public const int CMP_CENTER = 0x1980;
		public const int CMP_MAX = 0x30AF;
		public const int CMP_MIN = 0x024F;

		public const int A_REG_CENTER = 0x1900;
		public const int A_REG_MAX = 0x3000;
		public const int A_REG_MIN = 0x0200;
		public const int A_REG_STEP = 0x0100;

		public const int HR_REG_CENTER = 0x80;

        public const int HR_REG_MAX = 0xAF;
        public const int HR_REG_MIN = 0x4F;
		public const int HR_REG_STEP = 0x01;
		public const int HR_REG_LENGTH = 97;		// HR_REG_MAX - HR_REG_MIN + 1 = 0x60 + 1 = 97
		public const int HR_REG_R_COUNT = 0x30;		// 0x30 = 48, 0x30 = 0xAF - 0x80 + 0x01
		public const int HR_REG_L_COUNT = 0x31;		// 0x31 = 49, 0x31 = 0x7F - 0x4F + 0x01 
		public const int HR_REG_DIFF = 0x9F;		// Next 0f Max, 0x19AF + 1 = 0x19B0 + DIFF (0x9F) = 0x1A4F  ==> 0x1A4F + 0x9F = 0x1BAF

        public const uint MEM_VF_INDEX_SHIFT = 526;

                                                                    // 0 (+6V)   2 (-6V)    4 (+20V)   6 (-20V)   8 (+200V)   10 (-200V)
        public static readonly UInt16[] memTableVFP2 = new UInt16[] { 526, 572,  573, 619,  620, 666,  667, 713,  714, 760,   761, 807 };	// +6V, -6V, +20V, -20V, +200V, -200V

	}

	public class CmdData : ICloneable 
	{
		private object _lockObj;

		private byte _bExecMode;
		private byte _bTimeBase;
		private byte _bCMPA;
		private byte _bCMPAHR;

		private bool _isAutoTurnOff;
		private bool _isCalcCMP_ex;

		public CmdData()
		{
			this._lockObj = new object();

			this._bExecMode = 0x65;
			this._bTimeBase = 100;
			this._bCMPA = (byte)((LDevConst.CMP_CENTER & 0xFF ) >> 8 );
			this._bCMPAHR = (byte)(LDevConst.HR_REG_CENTER);

			this._isAutoTurnOff = true;
			this._isCalcCMP_ex = false;
		}

		#region >>> Public Property <<<

		public byte ExecMode
		{
			get { return this._bExecMode; }
			set { this._bExecMode = value; }
		}

		public byte TimeBase
		{
			get { return this._bTimeBase; }
			set { this._bTimeBase = value; }
		}

		public byte CMPA
		{
			get { return this._bCMPA; }
			set { this._bCMPA = value; }
		}

		public byte CMPAHR
		{
			get { return this._bCMPAHR; }
			set { this._bCMPAHR = value; }
		}

		public bool IsAutoTurnOff
		{
			get { return this._isAutoTurnOff; }
			set
			{
				lock (this._lockObj)
				{
					this._isAutoTurnOff = value;
				}
			}
		}

		#endregion

		# region >>> Public Method <<<

		public object Clone()
		{
			CmdData obj = this.MemberwiseClone() as CmdData;

			obj._lockObj = new object();
			return (object)obj;
		}

		#endregion

	}
}
