using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SourceMeter
{
	public class DSA_DASK
	{
		//ADLink DSA Card Type
		public const ushort PCI_9527 = 1;

		public const ushort MAX_CARD = 32;

		//Error Number
		public const short NoError = 0;
		public const short ErrorUnknownCardType = -1;
		public const short ErrorInvalidCardNumber = -2;
		public const short ErrorTooManyCardRegistered = -3;
		public const short ErrorCardNotRegistered = -4;
		public const short ErrorFuncNotSupport = -5;
		public const short ErrorInvalidIoChannel = -6;
		public const short ErrorInvalidAdRange = -7;
		public const short ErrorContIoNotAllowed = -8;
		public const short ErrorDiffRangeNotSupport = -9;
		public const short ErrorLastChannelNotZero = -10;
		public const short ErrorChannelNotDescending = -11;
		public const short ErrorChannelNotAscending = -12;
		public const short ErrorOpenDriverFailed = -13;
		public const short ErrorOpenEventFailed = -14;
		public const short ErrorTransferCountTooLarge = -15;
		public const short ErrorNotDoubleBufferMode = -16;
		public const short ErrorInvalidSampleRate = -17;
		public const short ErrorInvalidCounterMode = -18;
		public const short ErrorInvalidCounter = -19;
		public const short ErrorInvalidCounterState = -20;
		public const short ErrorInvalidBinBcdParam = -21;
		public const short ErrorBadCardType = -22;
		public const short ErrorInvalidDaRefVoltage = -23;
		public const short ErrorAdTimeOut = -24;
		public const short ErrorNoAsyncAI = -25;
		public const short ErrorNoAsyncAO = -26;
		public const short ErrorNoAsyncDI = -27;
		public const short ErrorNoAsyncDO = -28;
		public const short ErrorNotInputPort = -29;
		public const short ErrorNotOutputPort = -30;
		public const short ErrorInvalidDioPort = -31;
		public const short ErrorInvalidDioLine = -32;
		public const short ErrorContIoActive = -33;
		public const short ErrorDblBufModeNotAllowed = -34;
		public const short ErrorConfigFailed = -35;
		public const short ErrorInvalidPortDirection = -36;
		public const short ErrorBeginThreadError = -37;
		public const short ErrorInvalidPortWidth = -38;
		public const short ErrorInvalidCtrSource = -39;
		public const short ErrorOpenFile = -40;
		public const short ErrorAllocateMemory = -41;
		public const short ErrorDaVoltageOutOfRange = -42;
		public const short ErrorDaExtRefNotAllowed = -43;
		public const short ErrorDIODataWidthError = -44;
		public const short ErrorTaskCodeError = -45;
		public const short ErrortriggercountError = -46;
		public const short ErrorInvalidTriggerMode = -47;
		public const short ErrorInvalidTriggerType = -48;
		public const short ErrorInvalidCounterValue = -50;
		public const short ErrorInvalidEventHandle = -60;
		public const short ErrorNoMessageAvailable = -61;
		public const short ErrorEventMessgaeNotAdded = -62;
		public const short ErrorCalibrationTimeOut = -63;
		public const short ErrorUndefinedParameter = -64;
		public const short ErrorInvalidBufferID = -65;
		public const short ErrorInvalidSampledClock = -66;
		public const short ErrorInvalisOperationMode = -67;
		public const short ErrorInvalidDDSFrequency = -80;
		public const short ErrorInvalidDDSPhase = -81;
		public const short ErrorInvalidSPDMode = -82;
		public const short ErrorInvalidPGAGain = -83;
		public const short ErrorInvalidParmPointer = -84;
		public const short ErrorIoChannelNotCreated = -85;
		public const short ErrorInvalidAOParameter = -86;
		//Error number for driver API
		public const short ErrorConfigIoctl = -201;
		public const short ErrorAsyncSetIoctl = -202;
		public const short ErrorDBSetIoctl = -203;
		public const short ErrorDBHalfReadyIoctl = -204;
		public const short ErrorContOPIoctl = -205;
		public const short ErrorContStatusIoctl = -206;
		public const short ErrorPIOIoctl = -207;
		public const short ErrorDIntSetIoctl = -208;
		public const short ErrorWaitEvtIoctl = -209;
		public const short ErrorOpenEvtIoctl = -210;
		public const short ErrorCOSIntSetIoctl = -211;
		public const short ErrorMemMapIoctl = -212;
		public const short ErrorMemUMapSetIoctl = -213;
		public const short ErrorCTRIoctl = -214;
		public const short ErrorGetResIoctl = -215;
		public const short ErrorCalIoctl = -216;
		public const short ErrorPMIntSetIoctl = -217;

		//AD Range
		public const ushort AD_B_10_V = 1; //PCI-9527 AI,AO
		public const ushort AD_B_5_V = 2;
		public const ushort AD_B_2_5_V = 3;
		public const ushort AD_B_1_25_V = 4;
		public const ushort AD_B_0_625_V = 5;
		public const ushort AD_B_0_3125_V = 6;
		public const ushort AD_B_0_5_V = 7;
		public const ushort AD_B_0_05_V = 8;
		public const ushort AD_B_0_005_V = 9;
		public const ushort AD_B_1_V = 10; //PCI-9527 AI,AO
		public const ushort AD_B_0_1_V = 11; //PCI-9527 AO
		public const ushort AD_B_0_01_V = 12;
		public const ushort AD_B_0_001_V = 13;
		public const ushort AD_U_20_V = 14;
		public const ushort AD_U_10_V = 15;
		public const ushort AD_U_5_V = 16;
		public const ushort AD_U_2_5_V = 17;
		public const ushort AD_U_1_25_V = 18;
		public const ushort AD_U_1_V = 19;
		public const ushort AD_U_0_1_V = 20;
		public const ushort AD_U_0_01_V = 21;
		public const ushort AD_U_0_001_V = 22;
		public const ushort AD_B_2_V = 23;
		public const ushort AD_B_0_25_V = 24;
		public const ushort AD_B_0_2_V = 25;
		public const ushort AD_U_4_V = 26;
		public const ushort AD_U_2_V = 27;
		public const ushort AD_U_0_5_V = 28;
		public const ushort AD_U_0_4_V = 29;
		public const ushort AD_B_1_5_V = 30;
		public const ushort AD_B_0_2125_V = 31;
		public const ushort AD_B_40_V = 32; //PCI-9527 AI
		public const ushort AD_B_3_16_V = 33; //PCI-9527 AI
		public const ushort AD_B_0_316_V = 34; //PCI-9527 AI

		//T or F
		public const ushort TRUE = 1;
		public const ushort FALSE = 0;

		//Synchronous Mode
		public const ushort SYNCH_OP = 1;
		public const ushort ASYNCH_OP = 2;

		//Clock Mode
		public const ushort TRIG_SOFTWARE = 0;
		public const ushort TRIG_INT_PACER = 1;
		public const ushort TRIG_EXT_STROBE = 2;
		public const ushort TRIG_HANDSHAKE = 3;
		public const ushort TRIG_CLK_10MHZ = 4;  //PCI-7300A
		public const ushort TRIG_CLK_20MHZ = 5;  //PCI-7300A
		public const ushort TRIG_DO_CLK_TIMER_ACK = 6;  //PCI-7300A Rev. B
		public const ushort TRIG_DO_CLK_10M_ACK = 7;  //PCI-7300A Rev. B
		public const ushort TRIG_DO_CLK_20M_ACK = 8;  //PCI-7300A Rev. B

		//DAQ Event type for the event message
		public const ushort AIEnd = 0;
		public const ushort AOEnd = 0;
		public const ushort DIEnd = 0;
		public const ushort DOEnd = 0;
		public const ushort DBEvent = 1;
		public const ushort TrigEvent = 2;

		//Type Constants
		public const ushort DAQ_AI = 0;
		public const ushort DAQ_AO = 1;
		public const ushort DAQ_DI = 2;
		public const ushort DAQ_DO = 3;

		//EEPROM
		public const ushort EEPROM_DEFAULT_BANK = 0;
		public const ushort EEPROM_USER_BANK1 = 1;
		public const ushort EEPROM_USER_BANK2 = 2;

		/*------------------------*/
		/* Constants for PCI-9527 */
		/*------------------------*/
		//DDS Constants
		public const uint P9527_AI_MaxDDSFreq = 432000;
		public const ushort P9527_AI_MinDDSFreq = 2000;
		public const uint P9527_AO_MaxDDSFreq = 216000;
		public const ushort P9527_AO_MinDDSFreq = 1000;

		//AI Constants
		//AI Select Channel
		public const ushort P9527_AI_CH_0 = 0;
		public const ushort P9527_AI_CH_1 = 1;
		public const ushort P9527_AI_CH_DUAL = 2;
		//Input Type
		public const ushort P9527_AI_Differential = 0x00;
		public const ushort P9527_AI_PseudoDifferential = 0x01;
		//Input Coupling
		public const ushort P9527_AI_Coupling_DC = 0x00;
		public const ushort P9527_AI_Coupling_AC = 0x10;
		public const ushort P9527_AI_EnableIEPE = 0x20;

		//AO Constants
		//AO Select Channel
		public const ushort P9527_AO_CH_0 = 0;
		public const ushort P9527_AO_CH_1 = 1;
		public const ushort P9527_AO_CH_DUAL = 2;
		//Output Type
		public const ushort P9527_AO_Differential = 0x00;
		public const ushort P9527_AO_PseudoDifferential = 0x01;
		public const ushort P9527_AO_BalancedOutput = 0x02;

		//Trigger Constants
		//Trigger Mode
		public const ushort P9527_TRG_MODE_POST = 0x00;
		public const ushort P9527_TRG_MODE_DELAY = 0x01;
		//Trigger Target
		public const ushort P9527_TRG_NONE = 0x0;
		public const ushort P9527_TRG_AI = 0x1;
		public const ushort P9527_TRG_AO = 0x2;
		public const ushort P9527_TRG_ALL = 0x3;
		//Trigger Source
		public const ushort P9527_TRG_SRC_SOFT = 0x00;
		public const ushort P9527_TRG_SRC_EXTD = 0x10;
		public const ushort P9527_TRG_SRC_ANALOG = 0x20;
		public const ushort P9527_TRG_SRC_SSI = 0x30;
		public const ushort P9527_TRG_NOWAIT = 0x40;
		//Trigger Polarity
		public const ushort P9527_TRG_Negative = 0x000;
		public const ushort P9527_TRG_Positive = 0x100;
		//ReTrigger
		public const ushort P9527_TRG_EnReTigger = 0x200;
		//Analog Trigger Source
		public const ushort P9527_TRG_Analog_CH0 = 0;
		public const ushort P9527_TRG_Analog_CH1 = 1;
		//Analog Trigger Source
		public const ushort P9527_TRG_Analog_Above_threshold = 0;
		public const ushort P9527_TRG_Analog_Below_threshold = 1;

		//SSI signal code
		public const ushort P9527_SSI_AO_TRIG = 0x40;

		/*----------------------------------------------------------------------------*/
		/* DSA-DASK Function prototype                                               */
		/*----------------------------------------------------------------------------*/
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_Register_Card(ushort CardType, ushort card_num);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_Release_Card(ushort CardNumber);
		/*---------------------------------------------------------------------------*/
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_9527_ConfigChannel(ushort CardNumber, ushort Channel, ushort AdRange, ushort ConfigCtrl, bool AutoResetBuf);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_9527_ConfigSampleRate(ushort CardNumber, double SetDemandRate, out double GetActualRate);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_AsyncCheck(ushort CardNumber, out byte Stopped, out uint AccessCnt);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_AsyncClear(ushort CardNumber, out uint AccessCnt);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_AsyncDblBufferHalfReady(ushort CardNumber, out byte HalfReady, out byte StopFlag);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_AsyncDblBufferMode(ushort CardNumber, bool Enable);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_AsyncDblBufferOverrun(ushort CardNumber, ushort op, out ushort overrunFlag);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_AsyncDblBufferHandled(ushort CardNumber);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_AsyncDblBufferToFile(ushort CardNumber);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_AsyncReTrigNextReady(ushort CardNumber, out bool Ready, out bool StopFlag, out ushort RdyTrigCnt);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_ContBufferSetup(ushort CardNumber, uint[] Buffer, uint ReadCount, out ushort BufferId);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_ContBufferSetup(ushort CardNumber, IntPtr Buffer, uint ReadCount, out ushort BufferId);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_ContBufferReset(ushort CardNumber);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public unsafe static extern short DSA_AI_ContReadChannel(ushort CardNumber, ushort Channel, ushort AdRange, ushort* Buffer, uint ReadCount, double SampleRate, ushort SyncMode);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public unsafe static extern short DSA_AI_ContReadChannel(ushort CardNumber, ushort Channel, ushort AdRange, IntPtr Buffer, uint ReadCount, double SampleRate, ushort SyncMode);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_ContReadChannelToFile(ushort CardNumber, ushort Channel, ushort AdRange, string FileName, uint ReadCount, double SampleRate, ushort SyncMode);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_ContStatus(ushort CardNumber, out ushort Status);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_ContVScale(ushort CardNumber, ushort adRange, IntPtr readingArray, double[] voltageArray, int count);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_DataScaler(ushort cardType, ushort adRange, uint[] readingArray, double[] voltageArray, int count);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_EventCallBack(ushort CardNumber, ushort mode, ushort EventType, MulticastDelegate callbackAddr);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_InitialMemoryAllocated(ushort CardNumber, out uint MemSize);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_SetTimeOut(ushort CardNumber, uint TimeOut);
		/*---------------------------------------------------------------------------*/
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_9527_ConfigChannel(ushort CardNumber, ushort Channel, ushort AdRange, ushort ConfigCtrl, bool AutoResetBuf);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_9527_ConfigSampleRate(ushort CardNumber, double SetDemandRate, out double GetActualRate);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_AsyncCheck(ushort CardNumber, out bool Stopped, out uint AccessCnt);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_AsyncClear(ushort CardNumber, out uint AccessCnt, ushort stop_mode);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_AsyncDblBufferMode(ushort CardNumber, bool Enable);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_AsyncDblBufferHalfReady(ushort CardNumber, out bool bHalfReady);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_ContBufferReset(ushort CardNumber);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_ContBufferSetup(ushort CardNumber, uint[] Buffer, uint WriteCount, out ushort BufferId);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_ContStatus(ushort CardNumber, out ushort Status);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_ContWriteChannel(ushort CardNumber, ushort Channel, ushort BufId, uint WriteCount, uint Iterations, uint dwInterval, ushort definite, ushort SyncMode);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_EventCallBack(ushort CardNumber, ushort mode, ushort EventType, MulticastDelegate callbackAddr);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_InitialMemoryAllocated(ushort CardNumber, out uint MemSize);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_SetTimeOut(ushort CardNumber, uint TimeOut);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_VoltScale(ushort CardNumber, ushort Channel, double Voltage, out long binValue);
		/*---------------------------------------------------------------------------*/
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_TRG_Config(ushort CardNumber, ushort FuncSel, ushort TrigCtrl, uint ReTriggerCnt, uint TriggerDelay);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_TRG_ConfigAnalogTrigger(ushort CardNumber, uint ATrigSrc, uint ATrigMode, double Threshold);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_TRG_SoftTriggerGen(ushort CardNumber);
		/*---------------------------------------------------------------------------*/
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AI_GetEvent(ushort CardNumber, out long hEvent);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_AO_GetEvent(ushort CardNumber, out long hEvent);
		/*---------------------------------------------------------------------------*/
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_GetActualRate(ushort CardNumber, double fSampleRate, out double fActualRate);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_GetBaseAddr(ushort CardNumber, out uint BaseAddr, out uint BaseAddr2);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_GetLCRAddr(ushort CardNumber, out uint LcrAddr);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_GetFPGAVersion(ushort CardNumber, out uint FPGAVersion);
		/*---------------------------------------------------------------------------*/
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_Auto_Calibration_ALL(ushort CardNumber);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_CAL_LoadFromBank(ushort CardNumber, ushort bank);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_CAL_SaveToUserBank(ushort CardNumber, ushort bank);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_CAL_SetDefaultBank(ushort CardNumber, ushort bank);
		/*----------------------------------------------------------------------------*/
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_SSI_SourceConn(ushort CardNumber, ushort sigCode);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_SSI_SourceDisConn(ushort CardNumber, ushort sigCode);
		[DllImport(@"C:\MPI\LEDTester\Driver\DSA-Dask.dll")]
		public static extern short DSA_SSI_SourceClear(ushort CardNumber);
		/*----------------------------------------------------------------------------*/
	}
}
