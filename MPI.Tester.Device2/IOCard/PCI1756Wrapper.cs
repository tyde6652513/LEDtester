using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.IOCard
{
    public class PCI1756Wrapper : IIOCard
    {
        private object _lockObj;
        private const string _deviceDescription = "PCI-1756,BID#8";
        private int _devPortsCnt;
        private IntPtr _deviceHandle;
        private IntPtr _moduleHandle;
        private bool _isInitSuccess;
        private IOCommand _ioCommand;
        private bool[][] _doState;
        private bool _activeState;
        private DI _di;
        private DO _do;

        public PCI1756Wrapper()
        { 
            this._lockObj = new object();

            this._deviceHandle = IntPtr.Zero;

            this._isInitSuccess = false;

            this._do = new DO(32, this.DOCallBack);

            this._di = new DI(32, this.DICallBack);

            //Active High: true, Active Low: false 
            this._activeState = true;
        }

        public PCI1756Wrapper(IOCommand ioCommand, bool activeState)
            : this()
        {
            this._ioCommand = ioCommand;

            this._activeState = activeState;
        }

        #region >>> Public Property <<<

        public EDevErrorNumber ErrorNumber
        {
            get { return EDevErrorNumber.Device_NO_Error; }
        }

        public string HardwareVersion
        {
            get { return "NONE"; }
        }

        public string SerialNumber
        {
            get { return "NONE"; }
        }

        public string SoftwareVersion
        {
            get { return "NONE"; }
        }

        public bool IsInitSuccess
        {
            get { return this._isInitSuccess; }
        }

        public DI DI
        {
            get { return this._di; }
        }

        public DO DO
        {
            get { return this._do; }
        }

        #endregion

        #region >>> Private Methods <<<

        private bool SearchCard()
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
            {
                return false;
            }

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

                this._doState = new bool[this._devPortsCnt][];

                for (int i = 0; i < this._doState.Length; i++)
                {
                    this._doState[i] = new bool[8];
                }

                return true;
            }
            else
            {
                this._moduleHandle = IntPtr.Zero;

                return false;
            }
        }

        private void SetInterrupt(EventId id)
        {
            ErrorCode errorCode = ErrorCode.Success;

            /////////////////////////////////////////////////////////////
            // Get Interrupt Event Handle
            /////////////////////////////////////////////////////////////
            WaitHandle InterruptEventHandle = null;

            errorCode = this.GetHandle(id, out InterruptEventHandle);

            if (errorCode != ErrorCode.Success)
            {
                return;
            }

            /////////////////////////////////////////////////////////////
            // Reg Snap
            /////////////////////////////////////////////////////////////
            IntPtr bufferForSnap = IntPtr.Zero;

            errorCode = this.DiSnapStart(id, 0, 1, out bufferForSnap);

            if (errorCode != ErrorCode.Success)
            {
                return;
            }

            /////////////////////////////////////////////////////////////
            // Wait Interrupt
            /////////////////////////////////////////////////////////////
            Thread thread = new Thread(() => 
            {
                if (this._activeState)
                {
                    while (this._isInitSuccess)
                    {
                        if (InterruptEventHandle.WaitOne(10, false))
                        {
                            this.ClearFlag(id, 0, 0);

                            this.SendEvent(id);
                        }
                    }
                }
                else
                {
                    bool oldState = true;

                    bool state = true;

                    while (this._isInitSuccess)
                    {
                        state = this._di[0];

                        //When Active Low, Send Event
                        if (oldState == true && state == false)
                        {
                            this.SendEvent(EventId.EvtDiintChannel000);
                        }

                        oldState = state;

                        System.Threading.Thread.Sleep(1);
                    }
                }
            });

            thread.Priority = ThreadPriority.BelowNormal;

            thread.Name = id.ToString() + "_WatchInterruptThread";

            thread.Start();
        }

        private void SendEvent(EventId id)
        {
            if (this._ioCommand == null)
            {
                return;
            }

            if (id == EventId.EvtDiintChannel000)
            {
                if (this._ioCommand.StartOfTest != null)
                {
                    this._ioCommand.StartOfTest();
                }
            }
            else if (id == EventId.EvtDiintChannel016)
            {
                if (this._ioCommand.Calculate != null)
                {
                    this._ioCommand.Calculate();
                }
            }
        }

        private bool DICallBack(int pin)
        {
            if (!this._isInitSuccess)
            {
                return false;
            }

            ///////////////////////////////////////////////////////////
            // Find Port
            ///////////////////////////////////////////////////////////
            int port = 0;

            if (pin < 16)
            {
                port = 0;
            }
            else if (pin < 32)
            {
                port = 1;
            }
            else
            {
                return false;
            }

            byte byteData = 0x00;

            this.DiRead(port, out byteData);

            ///////////////////////////////////////////////////////////
            // Convert to Binary
            ///////////////////////////////////////////////////////////
            string str = Convert.ToString(byteData, 2);

            str = str[pin].ToString();

            if (str == "1")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        private void DOCallBack(int pin, bool state)
        {
            if (!this._isInitSuccess)
            {
                return;
            }

            ///////////////////////////////////////////////////////////
            // Find Port
            ///////////////////////////////////////////////////////////
            int port = 0;

            if (pin < 8)
            {
                port = 0;
            }
            else if (pin < 16)
            {
                port = 1;
            }
            else if (pin < 24)
            {
                port = 2;
            }
            else if (pin < 32)
            {
                port = 3;
            }
            else
            {
                return;
            }

            this._doState[port][pin] = state;

            ///////////////////////////////////////////////////////////
            // Convert to Binary
            ///////////////////////////////////////////////////////////
            byte data = 0x00;

            for (int i = 0; i < this._doState[port].Length; i++)
            {
                if (this._doState[port][i])
                {
                    data |= (byte)(0x01 << (byte)(i));
                }
            }

            ///////////////////////////////////////////////////////////
            // Write DO
            ///////////////////////////////////////////////////////////
            this.DoWrite(port, data);
        }

        #endregion

        #region >>> Public Methods <<<

        public bool Init()
        {
            if (System.IO.File.Exists(@"C:\MPI\LEDTester\Driver\BioDaq.dll") == false)
            {
                return false;
            }
                      
            if (this.SearchCard())
            {
                this._isInitSuccess = true;

                this.SetInterrupt(EventId.EvtDiintChannel000);

                //this.SetInterrupt(EventId.EvtDiintChannel016);

                //ErrorCode errorCode = this.SetProperty(PropertyId.CFG_DiTriggerSourceEdge, (int)ActiveSignal.FallingEdge);

                //if (errorCode != ErrorCode.Success)
                //{
                //    return false;
                //}

                return true;
            }
            else
            {
                return false;
            }
        }
        
        public void Close()
        {
            if (this._isInitSuccess)
            {
                AdxDeviceClose(this._deviceHandle);
            }

            this._isInitSuccess = false;
        }

        public void WriteDO(int[] pins, bool state)
        {
            foreach (var pin in pins)
            { 
                ///////////////////////////////////////////////////////////
                // Find Port
                ///////////////////////////////////////////////////////////
                int port = 0;

                if (pin < 8)
                {
                    port = 0;
                }
                else if (pin < 16)
                {
                    port = 1;
                }
                else if (pin < 24)
                {
                    port = 2;
                }
                else if (pin < 32)
                {
                    port = 3;
                }
                else
                {
                    return;
                }

                this._doState[port][pin % 8] = state;
            }

            ///////////////////////////////////////////////////////////
            // Convert to Binary
            ///////////////////////////////////////////////////////////
            byte[] data = new byte[this._doState.Length];

            for (int i = 0; i < this._doState.Length; i++)
            {
                data[i] = 0x00;

                for (int j = 0; j < this._doState[i].Length; j++)
                {
                    if (this._doState[i][j])
                    {
                        data[i] |= (byte)(0x01 << (byte)(j));
                    }
                }
            }

            ///////////////////////////////////////////////////////////
            // Write DO
            ///////////////////////////////////////////////////////////
            this.DoWrite(0, this._doState.Length, data);
        }

        #endregion

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
        private unsafe static extern ErrorCode AdxPropertyWrite(IntPtr module, PropertyId id, int dataLength, void* buffer, int notifyNow);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDiReadPorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDoWritePorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDoReadBackPorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxEventGetHandle(IntPtr module, EventId id, out IntPtr eventHandle);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxEventClearFlag(IntPtr module, EventId id, int flagLParam, int flagRParam);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDiSnapStart(IntPtr dioHandle, EventId id, int portStart, int portCount, out IntPtr buffer);

        [DllImport(@"C:\MPI\LEDTester\Driver\BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDiSnapStop(IntPtr dioHandle, EventId id);

        #endregion

        #region >>> BioDaq API <<<

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

        private unsafe ErrorCode SetProperty(PropertyId id, int data)
        {
            return AdxPropertyWrite(this._moduleHandle, id, sizeof(int), (void*)&data, 0);
        }

        private unsafe ErrorCode GetProperty(PropertyId id, int count, int[] data)
        {
            fixed (void* p = &data[0])
            {
                return AdxPropertyRead(this._moduleHandle, id, count * sizeof(int), p, null, null);
            }
        }

        private unsafe ErrorCode GetLength(PropertyId id, out int data)
        {
            fixed (int* p = &data)
            {
                ErrorCode rtn = AdxPropertyRead(this._moduleHandle, id, 0, null, p, null);

                data = data / sizeof(int);

                return rtn;
            }


        }

        private unsafe ErrorCode GetHandle(EventId id, out WaitHandle eventHandle)
        {
            eventHandle = null;

            IntPtr w32Handle;

            ErrorCode ret = AdxEventGetHandle(this._moduleHandle, id, out w32Handle);

            if (ret == ErrorCode.Success)
            {
                eventHandle = new AutoResetEvent(false);

                eventHandle.SafeWaitHandle = new SafeWaitHandle(w32Handle, false);
            }

            return ret;
        }

        private unsafe ErrorCode DiSnapStart(EventId id, int portStart, int portCount, out IntPtr buffer)
        {
            return AdxDiSnapStart(this._moduleHandle, id, portStart, portCount, out buffer);
        }

        private unsafe ErrorCode DiSnapStop(EventId id, int portStart)
        {
            return AdxDiSnapStop(this._moduleHandle, id);
        }

        private unsafe ErrorCode ClearFlag(EventId id, int flagLParam, int flagRParam)
        {
            return AdxEventClearFlag(this._moduleHandle, id, flagLParam, flagRParam);
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
        }

        public enum EventId
        {
            EvtDeviceRemoved = 0,  /* The device was removed from system */
            EvtDeviceReconnected,  /* The device is reconnected */
            EvtPropertyChanged,    /* Some properties of the device were changed */
            /*-----------------------------------------------------------------
            * AI events
            *-----------------------------------------------------------------*/
            EvtBufferedAiDataReady,
            EvtBufferedAiOverrun,
            EvtBufferedAiCacheOverflow,
            EvtBufferedAiStopped,

            /*-----------------------------------------------------------------
            * AO event IDs
            *-----------------------------------------------------------------*/
            EvtBufferedAoDataTransmitted,
            EvtBufferedAoUnderrun,
            EvtBufferedAoCacheEmptied,
            EvtBufferedAoTransStopped,
            EvtBufferedAoStopped,

            /*-----------------------------------------------------------------
            * DIO event IDs
            *-----------------------------------------------------------------*/
            EvtDiintChannel000, EvtDiintChannel001, EvtDiintChannel002, EvtDiintChannel003,
            EvtDiintChannel004, EvtDiintChannel005, EvtDiintChannel006, EvtDiintChannel007,
            EvtDiintChannel008, EvtDiintChannel009, EvtDiintChannel010, EvtDiintChannel011,
            EvtDiintChannel012, EvtDiintChannel013, EvtDiintChannel014, EvtDiintChannel015,
            EvtDiintChannel016, EvtDiintChannel017, EvtDiintChannel018, EvtDiintChannel019,
            EvtDiintChannel020, EvtDiintChannel021, EvtDiintChannel022, EvtDiintChannel023,
            EvtDiintChannel024, EvtDiintChannel025, EvtDiintChannel026, EvtDiintChannel027,
            EvtDiintChannel028, EvtDiintChannel029, EvtDiintChannel030, EvtDiintChannel031,
            EvtDiintChannel032, EvtDiintChannel033, EvtDiintChannel034, EvtDiintChannel035,
            EvtDiintChannel036, EvtDiintChannel037, EvtDiintChannel038, EvtDiintChannel039,
            EvtDiintChannel040, EvtDiintChannel041, EvtDiintChannel042, EvtDiintChannel043,
            EvtDiintChannel044, EvtDiintChannel045, EvtDiintChannel046, EvtDiintChannel047,
            EvtDiintChannel048, EvtDiintChannel049, EvtDiintChannel050, EvtDiintChannel051,
            EvtDiintChannel052, EvtDiintChannel053, EvtDiintChannel054, EvtDiintChannel055,
            EvtDiintChannel056, EvtDiintChannel057, EvtDiintChannel058, EvtDiintChannel059,
            EvtDiintChannel060, EvtDiintChannel061, EvtDiintChannel062, EvtDiintChannel063,
            EvtDiintChannel064, EvtDiintChannel065, EvtDiintChannel066, EvtDiintChannel067,
            EvtDiintChannel068, EvtDiintChannel069, EvtDiintChannel070, EvtDiintChannel071,
            EvtDiintChannel072, EvtDiintChannel073, EvtDiintChannel074, EvtDiintChannel075,
            EvtDiintChannel076, EvtDiintChannel077, EvtDiintChannel078, EvtDiintChannel079,
            EvtDiintChannel080, EvtDiintChannel081, EvtDiintChannel082, EvtDiintChannel083,
            EvtDiintChannel084, EvtDiintChannel085, EvtDiintChannel086, EvtDiintChannel087,
            EvtDiintChannel088, EvtDiintChannel089, EvtDiintChannel090, EvtDiintChannel091,
            EvtDiintChannel092, EvtDiintChannel093, EvtDiintChannel094, EvtDiintChannel095,
            EvtDiintChannel096, EvtDiintChannel097, EvtDiintChannel098, EvtDiintChannel099,
            EvtDiintChannel100, EvtDiintChannel101, EvtDiintChannel102, EvtDiintChannel103,
            EvtDiintChannel104, EvtDiintChannel105, EvtDiintChannel106, EvtDiintChannel107,
            EvtDiintChannel108, EvtDiintChannel109, EvtDiintChannel110, EvtDiintChannel111,
            EvtDiintChannel112, EvtDiintChannel113, EvtDiintChannel114, EvtDiintChannel115,
            EvtDiintChannel116, EvtDiintChannel117, EvtDiintChannel118, EvtDiintChannel119,
            EvtDiintChannel120, EvtDiintChannel121, EvtDiintChannel122, EvtDiintChannel123,
            EvtDiintChannel124, EvtDiintChannel125, EvtDiintChannel126, EvtDiintChannel127,
            EvtDiintChannel128, EvtDiintChannel129, EvtDiintChannel130, EvtDiintChannel131,
            EvtDiintChannel132, EvtDiintChannel133, EvtDiintChannel134, EvtDiintChannel135,
            EvtDiintChannel136, EvtDiintChannel137, EvtDiintChannel138, EvtDiintChannel139,
            EvtDiintChannel140, EvtDiintChannel141, EvtDiintChannel142, EvtDiintChannel143,
            EvtDiintChannel144, EvtDiintChannel145, EvtDiintChannel146, EvtDiintChannel147,
            EvtDiintChannel148, EvtDiintChannel149, EvtDiintChannel150, EvtDiintChannel151,
            EvtDiintChannel152, EvtDiintChannel153, EvtDiintChannel154, EvtDiintChannel155,
            EvtDiintChannel156, EvtDiintChannel157, EvtDiintChannel158, EvtDiintChannel159,
            EvtDiintChannel160, EvtDiintChannel161, EvtDiintChannel162, EvtDiintChannel163,
            EvtDiintChannel164, EvtDiintChannel165, EvtDiintChannel166, EvtDiintChannel167,
            EvtDiintChannel168, EvtDiintChannel169, EvtDiintChannel170, EvtDiintChannel171,
            EvtDiintChannel172, EvtDiintChannel173, EvtDiintChannel174, EvtDiintChannel175,
            EvtDiintChannel176, EvtDiintChannel177, EvtDiintChannel178, EvtDiintChannel179,
            EvtDiintChannel180, EvtDiintChannel181, EvtDiintChannel182, EvtDiintChannel183,
            EvtDiintChannel184, EvtDiintChannel185, EvtDiintChannel186, EvtDiintChannel187,
            EvtDiintChannel188, EvtDiintChannel189, EvtDiintChannel190, EvtDiintChannel191,
            EvtDiintChannel192, EvtDiintChannel193, EvtDiintChannel194, EvtDiintChannel195,
            EvtDiintChannel196, EvtDiintChannel197, EvtDiintChannel198, EvtDiintChannel199,
            EvtDiintChannel200, EvtDiintChannel201, EvtDiintChannel202, EvtDiintChannel203,
            EvtDiintChannel204, EvtDiintChannel205, EvtDiintChannel206, EvtDiintChannel207,
            EvtDiintChannel208, EvtDiintChannel209, EvtDiintChannel210, EvtDiintChannel211,
            EvtDiintChannel212, EvtDiintChannel213, EvtDiintChannel214, EvtDiintChannel215,
            EvtDiintChannel216, EvtDiintChannel217, EvtDiintChannel218, EvtDiintChannel219,
            EvtDiintChannel220, EvtDiintChannel221, EvtDiintChannel222, EvtDiintChannel223,
            EvtDiintChannel224, EvtDiintChannel225, EvtDiintChannel226, EvtDiintChannel227,
            EvtDiintChannel228, EvtDiintChannel229, EvtDiintChannel230, EvtDiintChannel231,
            EvtDiintChannel232, EvtDiintChannel233, EvtDiintChannel234, EvtDiintChannel235,
            EvtDiintChannel236, EvtDiintChannel237, EvtDiintChannel238, EvtDiintChannel239,
            EvtDiintChannel240, EvtDiintChannel241, EvtDiintChannel242, EvtDiintChannel243,
            EvtDiintChannel244, EvtDiintChannel245, EvtDiintChannel246, EvtDiintChannel247,
            EvtDiintChannel248, EvtDiintChannel249, EvtDiintChannel250, EvtDiintChannel251,
            EvtDiintChannel252, EvtDiintChannel253, EvtDiintChannel254, EvtDiintChannel255,

            EvtDiCosintPort000, EvtDiCosintPort001, EvtDiCosintPort002, EvtDiCosintPort003,
            EvtDiCosintPort004, EvtDiCosintPort005, EvtDiCosintPort006, EvtDiCosintPort007,
            EvtDiCosintPort008, EvtDiCosintPort009, EvtDiCosintPort010, EvtDiCosintPort011,
            EvtDiCosintPort012, EvtDiCosintPort013, EvtDiCosintPort014, EvtDiCosintPort015,
            EvtDiCosintPort016, EvtDiCosintPort017, EvtDiCosintPort018, EvtDiCosintPort019,
            EvtDiCosintPort020, EvtDiCosintPort021, EvtDiCosintPort022, EvtDiCosintPort023,
            EvtDiCosintPort024, EvtDiCosintPort025, EvtDiCosintPort026, EvtDiCosintPort027,
            EvtDiCosintPort028, EvtDiCosintPort029, EvtDiCosintPort030, EvtDiCosintPort031,

            EvtDiPmintPort000, EvtDiPmintPort001, EvtDiPmintPort002, EvtDiPmintPort003,
            EvtDiPmintPort004, EvtDiPmintPort005, EvtDiPmintPort006, EvtDiPmintPort007,
            EvtDiPmintPort008, EvtDiPmintPort009, EvtDiPmintPort010, EvtDiPmintPort011,
            EvtDiPmintPort012, EvtDiPmintPort013, EvtDiPmintPort014, EvtDiPmintPort015,
            EvtDiPmintPort016, EvtDiPmintPort017, EvtDiPmintPort018, EvtDiPmintPort019,
            EvtDiPmintPort020, EvtDiPmintPort021, EvtDiPmintPort022, EvtDiPmintPort023,
            EvtDiPmintPort024, EvtDiPmintPort025, EvtDiPmintPort026, EvtDiPmintPort027,
            EvtDiPmintPort028, EvtDiPmintPort029, EvtDiPmintPort030, EvtDiPmintPort031,

            EvtBufferedDiDataReady,
            EvtBufferedDiOverrun,
            EvtBufferedDiCacheOverflow,
            EvtBufferedDiStopped,

            EvtBufferedDoDataTransmitted,
            EvtBufferedDoUnderrun,
            EvtBufferedDoCacheEmptied,
            EvtBufferedDoTransStopped,
            EvtBufferedDoStopped,

            EvtReflectWdtOccured,
            /*-----------------------------------------------------------------
            * Counter/Timer event IDs
            *-----------------------------------------------------------------*/
            EvtCntTerminalCount0, EvtCntTerminalCount1, EvtCntTerminalCount2, EvtCntTerminalCount3,
            EvtCntTerminalCount4, EvtCntTerminalCount5, EvtCntTerminalCount6, EvtCntTerminalCount7,

            EvtCntOverCompare0, EvtCntOverCompare1, EvtCntOverCompare2, EvtCntOverCompare3,
            EvtCntOverCompare4, EvtCntOverCompare5, EvtCntOverCompare6, EvtCntOverCompare7,

            EvtCntUnderCompare0, EvtCntUnderCompare1, EvtCntUnderCompare2, EvtCntUnderCompare3,
            EvtCntUnderCompare4, EvtCntUnderCompare5, EvtCntUnderCompare6, EvtCntUnderCompare7,

            EvtCntEcOverCompare0, EvtCntEcOverCompare1, EvtCntEcOverCompare2, EvtCntEcOverCompare3,
            EvtCntEcOverCompare4, EvtCntEcOverCompare5, EvtCntEcOverCompare6, EvtCntEcOverCompare7,

            EvtCntEcUnderCompare0, EvtCntEcUnderCompare1, EvtCntEcUnderCompare2, EvtCntEcUnderCompare3,
            EvtCntEcUnderCompare4, EvtCntEcUnderCompare5, EvtCntEcUnderCompare6, EvtCntEcUnderCompare7,

            EvtCntOneShot0, EvtCntOneShot1, EvtCntOneShot2, EvtCntOneShot3,
            EvtCntOneShot4, EvtCntOneShot5, EvtCntOneShot6, EvtCntOneShot7,

            EvtCntTimer0, EvtCntTimer1, EvtCntTimer2, EvtCntTimer3,
            EvtCntTimer4, EvtCntTimer5, EvtCntTimer6, EvtCntTimer7,

            EvtCntPwmInOverflow0, EvtCntPwmInOverflow1, EvtCntPwmInOverflow2, EvtCntPwmInOverflow3,
            EvtCntPwmInOverflow4, EvtCntPwmInOverflow5, EvtCntPwmInOverflow6, EvtCntPwmInOverflow7,
        }

        public enum ActiveSignal
        {
            ActiveNone = 0,
            RisingEdge,
            FallingEdge,
            BothEdge,
            HighLevel,
            LowLevel,
        }

        #endregion

    }
}
