using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security;

namespace MPI.Tester.Device.SwitchSystem
{
    public class PCI1756Wrapper
    {
        private object _lockObj;

        private const string _deviceDescription = "PCI-1756,BID#8";
      
        private int _deviceNumber;
        private int _devPortsCnt;
        private IntPtr _deviceHandle;
        private IntPtr _moduleHandle;

        public PCI1756Wrapper()
        { 
            this._lockObj = new object();

            this._deviceNumber = 0;
            this._deviceHandle = IntPtr.Zero;
        }

        #region >>> DLL import <<<

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceGetLinkageInfo(int deviceParent, int index, out int deviceNumber, StringBuilder description, out int subDeviceCount);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceOpen(int number, AccessMode accessMode, out IntPtr deviceHandle);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceClose(IntPtr deviceHandle);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceReset(IntPtr deviceHandle, int state);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceGetModuleHandle(IntPtr deviceHandle, ModuleType moduleType, int index, out IntPtr moduleHandle);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxPropertyRead(IntPtr module, PropertyId id, int bufferSize, void* buffer, int* dataLength, int* attribute);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDiReadPorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDoWritePorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDoReadBackPorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

        #endregion

        #region >>> Private Methods <<<

        private void SearchCard()
        { 
            bool isFindCard = false;
            int deviceNumber = 0;
            int subDeviceCount = 0;
            StringBuilder description = new StringBuilder(100);
            ErrorCode pci1756Err;
            ErrorCode moduleErr;
            IntPtr deviceHandle = IntPtr.Zero;
            IntPtr moduleHandle = IntPtr.Zero;

            if (this._deviceHandle != IntPtr.Zero)
                return;

            for ( int index = 0; index <= 15 ;index++ )
            {
                pci1756Err = AdxDeviceGetLinkageInfo(-1, index, out deviceNumber, description, out subDeviceCount);
                if (pci1756Err != ErrorCode.Success)
                {
                    this._deviceHandle = IntPtr.Zero; 
                }

                if (description.ToString() == _deviceDescription)
                {
                    isFindCard = true;
                    break;
                }
            }

            if (isFindCard == false)
            {
                this._deviceHandle = IntPtr.Zero; 
            }

            pci1756Err = AdxDeviceOpen(deviceNumber, AccessMode.ModeWrite, out deviceHandle);

            if (pci1756Err == ErrorCode.Success)
            {
                this._deviceHandle = deviceHandle;
            }
            else
            {
                this._deviceHandle = IntPtr.Zero;
            }

            moduleErr = AdxDeviceGetModuleHandle(deviceHandle, ModuleType.DaqDio, 0, out moduleHandle);

            if (moduleErr == ErrorCode.Success)
            {
                this._moduleHandle = moduleHandle;
                GetProperty(PropertyId.CFG_FeaturePortsCount, out this._devPortsCnt);
            }
            else
            {
                this._moduleHandle = IntPtr.Zero;
            }
        }

        private unsafe ErrorCode DoWrite(int port, byte data)
        {
            return AdxDoWritePorts(this._moduleHandle, port, 1, &data);
        }

        private unsafe ErrorCode DoWrite(int portStart, int portCount, byte[] data)
        {                              
            fixed (byte* p = data)
            {
                return AdxDoWritePorts(this._moduleHandle, portStart, portCount, p);
            }
        }

        private unsafe ErrorCode DoRead(int portStart, int portCount, byte[] data)
        {
            fixed (byte* p = data)
            {
                return AdxDiReadPorts(this._moduleHandle, portStart, portCount, p);
            }
        }

        private unsafe ErrorCode DoRead(int port, out byte data)
        {          
            fixed (byte* p = &data)
            {
                return AdxDoReadBackPorts(this._moduleHandle, port, 1, p);
            }
        }

        private unsafe ErrorCode DiRead(int portStart, int portCount, byte[] data)
        {
            fixed (byte* p = data)
            {
                return AdxDiReadPorts(this._moduleHandle, portStart, portCount, p);
            }
        }

        private unsafe ErrorCode DiRead(int port, out byte data)
        {
            fixed (byte* p = &data)
            {
                return AdxDiReadPorts(this._moduleHandle, port, 1, p);
            }
        }

        private unsafe ErrorCode GetProperty(PropertyId id, out int data)
        {
            fixed (void* p = &data)
            {
                return AdxPropertyRead(this._moduleHandle, id, sizeof(int), p, null, null);
            }
        }

        #endregion

        #region >>> Public Methods <<<

        public bool Init()
        {
            if (System.IO.File.Exists(@"C:\MPI\LEDTester\Driver\BioDaq.dll") == false)
            {
                return false;
            }
                      
            this.SearchCard();

            if (this._deviceHandle == IntPtr.Zero)
            {
                return false;
            }
            else
            {
                byte[] data = new byte[this._devPortsCnt]; 
                this.DoWrite(0, this._devPortsCnt - 1, data);
  
                return true;
            }
        }
        
        public void Close()
        {
            AdxDeviceClose(this._deviceHandle);
        }

        public bool WriteDO(int port, byte data)
        {
            if (port >= this._devPortsCnt)
                return false;

            if (this.DoWrite(port, data) != ErrorCode.Success)
                return false;

            return true;         
        }

        public bool WriteDO(int port, ushort data)
        {
            byte[] byteArray = BitConverter.GetBytes(data);

            if (port >= this._devPortsCnt || port + byteArray.Length > this._devPortsCnt)
                return false;

            if (this.DoWrite(port, byteArray.Length, byteArray) != ErrorCode.Success)
                return false;

            return true;
        }

        public bool ReadDI(int port, out byte data)
        {
            if (port >= this._devPortsCnt)
            {
                data = 0x00;
                return false;
            }

            if (this.DiRead(port, out data) != ErrorCode.Success)
                return false;

            return true;
        }

        #endregion

        #region >>> Enumeration <<<

        private enum ModuleType
        {
            DaqAny = -1,
            DaqGroup = 1,
            DaqDevice,
            DaqAi,
            DaqAo,
            DaqDio,
            DaqCounter,
        }

        private enum AccessMode
        {
            ModeRead = 0,
            ModeWrite,
            ModeWriteWithReset,
        }

        private enum PropertyId
        {
            /*-----------------------------------------------------------------
            * common property
            *-----------------------------------------------------------------*/
            CFG_Number,
            CFG_ComponentType,
            CFG_Description,
            CFG_Parent,
            CFG_ChildList,

            /*-----------------------------------------------------------------
            * component specified Property IDs -- group
            *-----------------------------------------------------------------*/
            CFG_DevicesNumber,
            CFG_DevicesHandle,

            /*-----------------------------------------------------------------
            * component specified Property IDs -- device
            *-----------------------------------------------------------------*/
            CFG_DeviceGroupNumber,
            CFG_DeviceProductID,
            CFG_DeviceBoardID,
            CFG_DeviceBoardVersion,
            CFG_DeviceDriverVersion,
            CFG_DeviceDllVersion,
            CFG_DeviceLocation,                       /* Reserved for later using */
            CFG_DeviceBaseAddresses,                  /* Reserved for later using */
            CFG_DeviceInterrupts,                     /* Reserved for later using */
            CFG_DeviceSupportedTerminalBoardTypes,    /* Reserved for later using */
            CFG_DeviceTerminalBoardType,              /* Reserved for later using */
            CFG_DeviceSupportedEvents,
            CFG_DeviceHotResetPreventable,            /* Reserved for later using */
            CFG_DeviceLoadingTimeInit,                /* Reserved for later using */
            CFG_DeviceWaitingForReconnect,
            CFG_DeviceWaitingForSleep,

            /*-----------------------------------------------------------------
            * component specified Property IDs -- AI, AO...
            *-----------------------------------------------------------------*/
            CFG_FeatureResolutionInBit,
            CFG_FeatureDataSize,
            CFG_FeatureDataMask,
            CFG_FeatureChannelNumberMax,
            CFG_FeatureChannelConnectionType,
            CFG_FeatureBurnDetectedReturnTypes,
            CFG_FeatureBurnoutDetectionChannels,
            CFG_FeatureOverallVrgType,
            CFG_FeatureVrgTypes,
            CFG_FeatureExtRefRange,
            CFG_FeatureExtRefAntiPolar,
            CFG_FeatureCjcChannels,
            CFG_FeatureChannelScanMethod,
            CFG_FeatureScanChannelStartBase,
            CFG_FeatureScanChannelCountBase,
            CFG_FeatureConvertClockSources,
            CFG_FeatureConvertClockRateRange,       /* Reserved for later using */
            CFG_FeatureScanClockSources,
            CFG_FeatureScanClockRateRange,         /* Reserved for later using */
            CFG_FeatureScanCountMax,               /* Reserved for later using */
            CFG_FeatureTriggersCount,
            CFG_FeatureTriggerSources,
            CFG_FeatureTriggerActions,
            CFG_FeatureTriggerDelayCountRange,
            CFG_FeatureTriggerSources1,            /* Reserved for later using */
            CFG_FeatureTriggerActions1,            /* Reserved for later using */
            CFG_FeatureTriggerDelayCountRange1,    /* Reserved for later using */

            CFG_ChannelCount,
            CFG_ConnectionTypeOfChannels,
            CFG_VrgTypeOfChannels,
            CFG_BurnDetectedReturnTypeOfChannels,
            CFG_BurnoutReturnValueOfChannels,
            CFG_ExtRefValueForUnipolar,         /* Reserved for later using */
            CFG_ExtRefValueForBipolar,          /* Reserved for later using */

            CFG_CjcChannel,
            CFG_CjcUpdateFrequency,             /* Reserved for later using */
            CFG_CjcValue,

            CFG_SectionDataCount,
            CFG_ConvertClockSource,
            CFG_ConvertClockRatePerChannel,
            CFG_ScanChannelStart,
            CFG_ScanChannelCount,
            CFG_ScanClockSource,                /* Reserved for later using */
            CFG_ScanClockRate,                  /* Reserved for later using */
            CFG_ScanCount,                      /* Reserved for later using */
            CFG_TriggerSource,
            CFG_TriggerSourceEdge,
            CFG_TriggerSourceLevel,
            CFG_TriggerDelayCount,
            CFG_TriggerAction,
            CFG_TriggerSource1,                 /* Reserved for later using */
            CFG_TriggerSourceEdge1,             /* Reserved for later using */
            CFG_TriggerSourceLevel1,            /* Reserved for later using */
            CFG_TriggerDelayCount1,             /* Reserved for later using */
            CFG_TriggerAction1,                 /* Reserved for later using */
            CFG_ParentSignalConnectionChannel,
            CFG_ParentCjcConnectionChannel,
            CFG_ParentControlPort,

            /*-----------------------------------------------------------------
            * component specified Property IDs -- DIO
            *-----------------------------------------------------------------*/
            CFG_FeaturePortsCount,
            CFG_FeaturePortsType,
            CFG_FeatureDiNoiseFilterOfChannels,
            CFG_FeatureDiNoiseFilterBlockTimeRange,     /* Reserved for later using */
            CFG_FeatureDiintTriggerEdges,
            CFG_FeatureDiintOfChannels,
            CFG_FeatureDiintGateOfChannels,
            CFG_FeatureDiCosintOfChannels,
            CFG_FeatureDiPmintOfChannels,
            CFG_FeatureDiSnapEventSources,
            CFG_FeatureDoFreezeSignalSources,            /* Reserved for later using */
            CFG_FeatureDoReflectWdtFeedIntervalRange,    /* Reserved for later using */

            CFG_FeatureDiPortScanMethod,                 /* Reserved for later using */
            CFG_FeatureDiConvertClockSources,            /* Reserved for later using */
            CFG_FeatureDiConvertClockRateRange,          /* Reserved for later using */
            CFG_FeatureDiScanClockSources,
            CFG_FeatureDiScanClockRateRange,             /* Reserved for later using */
            CFG_FeatureDiScanCountMax,
            CFG_FeatureDiTriggersCount,
            CFG_FeatureDiTriggerSources,
            CFG_FeatureDiTriggerActions,
            CFG_FeatureDiTriggerDelayCountRange,
            CFG_FeatureDiTriggerSources1,
            CFG_FeatureDiTriggerActions1,
            CFG_FeatureDiTriggerDelayCountRange1,

            CFG_FeatureDoPortScanMethod,                 /* Reserved for later using */
            CFG_FeatureDoConvertClockSources,            /* Reserved for later using */
            CFG_FeatureDoConvertClockRateRange,          /* Reserved for later using */
            CFG_FeatureDoScanClockSources,
            CFG_FeatureDoScanClockRateRange,             /* Reserved for later using */
            CFG_FeatureDoScanCountMax,
            CFG_FeatureDoTriggersCount,
            CFG_FeatureDoTriggerSources,
            CFG_FeatureDoTriggerActions,
            CFG_FeatureDoTriggerDelayCountRange,
            CFG_FeatureDoTriggerSources1,
            CFG_FeatureDoTriggerActions1,
            CFG_FeatureDoTriggerDelayCountRange1,

            CFG_DirectionOfPorts,
            CFG_DiDataMaskOfPorts,
            CFG_DoDataMaskOfPorts,

            CFG_DiNoiseFilterOverallBlockTime,              /* Reserved for later using */
            CFG_DiNoiseFilterEnabledChannels,
            CFG_DiintTriggerEdgeOfChannels,
            CFG_DiintGateEnabledChannels,
            CFG_DiCosintEnabledChannels,
            CFG_DiPmintEnabledChannels,
            CFG_DiPmintValueOfPorts,
            CFG_DoInitialStateOfPorts,                   /* Reserved for later using */
            CFG_DoFreezeEnabled,                         /* Reserved for later using */
            CFG_DoFreezeSignalState,                     /* Reserved for later using */
            CFG_DoReflectWdtFeedInterval,                /* Reserved for later using */
            CFG_DoReflectWdtLockValue,                   /* Reserved for later using */
            CFG_DiSectionDataCount,
            CFG_DiConvertClockSource,
            CFG_DiConvertClockRatePerPort,
            CFG_DiScanPortStart,
            CFG_DiScanPortCount,
            CFG_DiScanClockSource,
            CFG_DiScanClockRate,
            CFG_DiScanCount,
            CFG_DiTriggerAction,
            CFG_DiTriggerSource,
            CFG_DiTriggerSourceEdge,
            CFG_DiTriggerSourceLevel,                    /* Reserved for later using */
            CFG_DiTriggerDelayCount,
            CFG_DiTriggerAction1,
            CFG_DiTriggerSource1,
            CFG_DiTriggerSourceEdge1,
            CFG_DiTriggerSourceLevel1,                   /* Reserved for later using */
            CFG_DiTriggerDelayCount1,

            CFG_DoSectionDataCount,
            CFG_DoConvertClockSource,
            CFG_DoConvertClockRatePerPort,
            CFG_DoScanPortStart,
            CFG_DoScanPortCount,
            CFG_DoScanClockSource,
            CFG_DoScanClockRate,
            CFG_DoScanCount,
            CFG_DoTriggerAction,
            CFG_DoTriggerSource,
            CFG_DoTriggerSourceEdge,
            CFG_DoTriggerSourceLevel,                    /* Reserved for later using */
            CFG_DoTriggerDelayCount,
            CFG_DoTriggerAction1,
            CFG_DoTriggerSource1,
            CFG_DoTriggerSourceEdge1,
            CFG_DoTriggerSourceLevel1,                   /* Reserved for later using */
            CFG_DoTriggerDelayCount1,

            /*-----------------------------------------------------------------
            * component specified Property IDs -- Counter/Timer
            *-----------------------------------------------------------------*/
            /*common feature*/
            CFG_FeatureClkPolarities,
            CFG_FeatureGatePolarities,

            CFG_FeatureCapabilitiesOfCounter0 = CFG_FeatureGatePolarities + 3,
            CFG_FeatureCapabilitiesOfCounter1,
            CFG_FeatureCapabilitiesOfCounter2,
            CFG_FeatureCapabilitiesOfCounter3,
            CFG_FeatureCapabilitiesOfCounter4,
            CFG_FeatureCapabilitiesOfCounter5,
            CFG_FeatureCapabilitiesOfCounter6,
            CFG_FeatureCapabilitiesOfCounter7,

            /*primal counter features*/
            CFG_FeatureChipOperationModes = CFG_FeatureCapabilitiesOfCounter7 + 25,
            CFG_FeatureChipSignalCountingTypes,

            /*event counting features*/
            CFG_FeatureEcSignalCountingTypes,
            CFG_FeatureEcOverCompareIntCounters,
            CFG_FeatureEcUnderCompareIntCounters,

            /*timer/pulse features*/
            CFG_FeatureTmrCascadeGroups,

            /*frequency measurement features*/
            CFG_FeatureFmCascadeGroups,
            CFG_FeatureFmMethods,

            /*Primal counter properties */
            CFG_ChipOperationModeOfCounters = CFG_FeatureFmMethods + 7,
            CFG_ChipSignalCountingTypeOfCounters,
            CFG_ChipLoadValueOfCounters,
            CFG_ChipHoldValueOfCounters,
            CFG_ChipOverCompareValueOfCounters,
            CFG_ChipUnderCompareValueOfCounters,
            CFG_ChipOverCompareEnabledCounters,
            CFG_ChipUnderCompareEnabledCounters,

            /*Event counting properties*/
            CFG_EcOverCompareValueOfCounters,
            CFG_EcUnderCompareValueOfCounters,
            CFG_EcSignalCountingTypeOfCounters,

            /*frequency measurement properties*/
            CFG_FmMethodOfCounters,
            CFG_FmCollectionPeriodOfCounters,

            //##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            // v1.1
            //##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            CFG_DevicePrivateRegionLength,
            CFG_SaiAutoConvertClockRate,
            CFG_SaiAutoConvertChannelStart,
            CFG_SaiAutoConvertChannelCount,
            CFG_ExtPauseSignalEnabled,
            CFG_ExtPauseSignalPolarity,
            CFG_OrderOfChannels,
            CFG_InitialStateOfChannels,

            //##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            // v1.2: new features & properties of counter
            //##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            /*common features*/
            CFG_FeatureOutSignalTypes,

            /*primal counter features*/
            CFG_FeatureChipClkSourceOfCounter0,
            CFG_FeatureChipClkSourceOfCounter1,
            CFG_FeatureChipClkSourceOfCounter2,
            CFG_FeatureChipClkSourceOfCounter3,
            CFG_FeatureChipClkSourceOfCounter4,
            CFG_FeatureChipClkSourceOfCounter5,
            CFG_FeatureChipClkSourceOfCounter6,
            CFG_FeatureChipClkSourceOfCounter7,

            CFG_FeatureChipGateSourceOfCounter0,
            CFG_FeatureChipGateSourceOfCounter1,
            CFG_FeatureChipGateSourceOfCounter2,
            CFG_FeatureChipGateSourceOfCounter3,
            CFG_FeatureChipGateSourceOfCounter4,
            CFG_FeatureChipGateSourceOfCounter5,
            CFG_FeatureChipGateSourceOfCounter6,
            CFG_FeatureChipGateSourceOfCounter7,

            CFG_FeatureChipValueRegisters,

            /*one-shot features*/
            CFG_FeatureOsClkSourceOfCounter0,
            CFG_FeatureOsClkSourceOfCounter1,
            CFG_FeatureOsClkSourceOfCounter2,
            CFG_FeatureOsClkSourceOfCounter3,
            CFG_FeatureOsClkSourceOfCounter4,
            CFG_FeatureOsClkSourceOfCounter5,
            CFG_FeatureOsClkSourceOfCounter6,
            CFG_FeatureOsClkSourceOfCounter7,

            CFG_FeatureOsGateSourceOfCounter0,
            CFG_FeatureOsGateSourceOfCounter1,
            CFG_FeatureOsGateSourceOfCounter2,
            CFG_FeatureOsGateSourceOfCounter3,
            CFG_FeatureOsGateSourceOfCounter4,
            CFG_FeatureOsGateSourceOfCounter5,
            CFG_FeatureOsGateSourceOfCounter6,
            CFG_FeatureOsGateSourceOfCounter7,

            /*Pulse width measurement features*/
            CFG_FeaturePiCascadeGroups,

            /*common properties*/
            CFG_ClkPolarityOfCounters,
            CFG_GatePolarityOfCounters,
            CFG_OutSignalTypeOfCounters,

            /*Primal counter properties */
            CFG_ChipClkSourceOfCounters,
            CFG_ChipGateSourceOfCounters,

            /*one-shot properties*/
            CFG_OsClkSourceOfCounters,
            CFG_OsGateSourceOfCounters,
            CFG_OsDelayCountOfCounters,

            /*Timer pulse properties*/
            CFG_TmrFrequencyOfCounters,

            /*Pulse width modulation properties*/
            CFG_PoHiPeriodOfCounters,
            CFG_PoLoPeriodOfCounters,
        }

        private enum ErrorCode
        {
            /// <summary>
            /// The operation is completed successfully. 
            /// </summary>
            Success = 0,

            ///************************************************************************
            /// warning                                                              
            ///************************************************************************
            /// <summary>
            /// The interrupt resource is not available. 
            /// </summary>
            WarningIntrNotAvailable = unchecked((int)0xA0000000),

            /// <summary>
            /// The parameter is out of the range. 
            /// </summary>
            WarningParamOutOfRange = unchecked((int)0xA0000001),

            /// <summary>
            /// The property value is out of range. 
            /// </summary>
            WarningPropValueOutOfRange = unchecked((int)0xA0000002),

            /// <summary>
            /// The property value is not supported. 
            /// </summary>
            WarningPropValueNotSpted = unchecked((int)0xA0000003),

            /// <summary>
            /// The property value conflicts with the current state.
            /// </summary>
            WarningPropValueConflict = unchecked((int)0xA0000004),

            ///***********************************************************************
            /// error                                                                
            ///***********************************************************************
            /// <summary>
            /// The handle is NULL or its type doesn't match the required operation. 
            /// </summary>
            ErrorHandleNotValid = unchecked((int)0xE0000000),

            /// <summary>
            /// The parameter value is out of range.
            /// </summary>
            ErrorParamOutOfRange = unchecked((int)0xE0000001),

            /// <summary>
            /// The parameter value is not supported.
            /// </summary>
            ErrorParamNotSpted = unchecked((int)0xE0000002),

            /// <summary>
            /// The parameter value format is not the expected. 
            /// </summary>
            ErrorParamFmtUnexpted = unchecked((int)0xE0000003),

            /// <summary>
            /// Not enough memory is available to complete the operation. 
            /// </summary>
            ErrorMemoryNotEnough = unchecked((int)0xE0000004),

            /// <summary>
            /// The data buffer is null. 
            /// </summary>
            ErrorBufferIsNull = unchecked((int)0xE0000005),

            /// <summary>
            /// The data buffer is too small for the operation. 
            /// </summary>
            ErrorBufferTooSmall = unchecked((int)0xE0000006),

            /// <summary>
            /// The data length exceeded the limitation. 
            /// </summary>
            ErrorDataLenExceedLimit = unchecked((int)0xE0000007),

            /// <summary>
            /// The required function is not supported. 
            /// </summary>
            ErrorFuncNotSpted = unchecked((int)0xE0000008),

            /// <summary>
            /// The required event is not supported. 
            /// </summary>
            ErrorEventNotSpted = unchecked((int)0xE0000009),

            /// <summary>
            /// The required property is not supported. 
            /// </summary>
            ErrorPropNotSpted = unchecked((int)0xE000000A),

            /// <summary>
            /// The required property is read-only. 
            /// </summary>
            ErrorPropReadOnly = unchecked((int)0xE000000B),

            /// <summary>
            /// The specified property value conflicts with the current state.
            /// </summary>
            ErrorPropValueConflict = unchecked((int)0xE000000C),

            /// <summary>
            /// The specified property value is out of range.
            /// </summary>
            ErrorPropValueOutOfRange = unchecked((int)0xE000000D),

            /// <summary>
            /// The specified property value is not supported. 
            /// </summary>
            ErrorPropValueNotSpted = unchecked((int)0xE000000E),

            /// <summary>
            /// The handle hasn't own the privilege of the operation the user wanted. 
            /// </summary>
            ErrorPrivilegeNotHeld = unchecked((int)0xE000000F),

            /// <summary>
            /// The required privilege is not available because someone else had own it. 
            /// </summary>
            ErrorPrivilegeNotAvailable = unchecked((int)0xE0000010),

            /// <summary>
            /// The driver of specified device was not found. 
            /// </summary>
            ErrorDriverNotFound = unchecked((int)0xE0000011),

            /// <summary>
            /// The driver version of the specified device mismatched. 
            /// </summary>
            ErrorDriverVerMismatch = unchecked((int)0xE0000012),

            /// <summary>
            /// The loaded driver count exceeded the limitation. 
            /// </summary>
            ErrorDriverCountExceedLimit = unchecked((int)0xE0000013),

            /// <summary>
            /// The device is not opened. 
            /// </summary>
            ErrorDeviceNotOpened = unchecked((int)0xE0000014),

            /// <summary>
            /// The required device does not exist. 
            /// </summary>
            ErrorDeviceNotExist = unchecked((int)0xE0000015),

            /// <summary>
            /// The required device is unrecognized by driver. 
            /// </summary>
            ErrorDeviceUnrecognized = unchecked((int)0xE0000016),

            /// <summary>
            /// The configuration data of the specified device is lost or unavailable. 
            /// </summary>
            ErrorConfigDataLost = unchecked((int)0xE0000017),

            /// <summary>
            /// The function is not initialized and can't be started. 
            /// </summary>
            ErrorFuncNotInited = unchecked((int)0xE0000018),

            /// <summary>
            /// The function is busy. 
            /// </summary>
            ErrorFuncBusy = unchecked((int)0xE0000019),

            /// <summary>
            /// The interrupt resource is not available. 
            /// </summary>
            ErrorIntrNotAvailable = unchecked((int)0xE000001A),

            /// <summary>
            /// The DMA channel is not available. 
            /// </summary>
            ErrorDmaNotAvailable = unchecked((int)0xE000001B),

            /// <summary>
            /// Time out when reading/writing the device. 
            /// </summary>
            ErrorDeviceIoTimeOut = unchecked((int)0xE000001C),

            /// <summary>
            /// The given signature does not match with the device current one.
            /// </summary>
            ErrorSignatureNotMatch = unchecked((int)0xE000001D),

            /// <summary>
            /// Undefined error 
            /// </summary>
            ErrorUndefined = unchecked((int)0xE000FFFF),
        };

        #endregion

    }
}
