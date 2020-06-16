using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security;

namespace MPI.Tester.Device.ESD
{
    internal class PCI1756Wrapper
    {
        private object _lockObj;

        private const string _deviceDescription = "PCI-1756,BID#12";

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

        private int _deviceNumber;
        private IntPtr _deviceHandle;
        private IntPtr _moduleHandle;
        private byte[] _outByteArray;


        public PCI1756Wrapper()
        { 
            this._lockObj = new object();

            this._deviceNumber = 0;
            this._deviceHandle = IntPtr.Zero;

            this.SearchCard();
            this._outByteArray = new byte[4];
        }

        #region >>> DLL import <<<

        [DllImport("BioDaq.dll", CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceGetLinkageInfo(int deviceParent, int index, out int deviceNumber, StringBuilder description, out int subDeviceCount);
        
        [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceOpen(int number, AccessMode accessMode, out IntPtr deviceHandle);

        [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceClose(IntPtr deviceHandle);

        [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceReset(IntPtr deviceHandle, int state);

        [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private static extern ErrorCode AdxDeviceGetModuleHandle(IntPtr deviceHandle, ModuleType moduleType, int index, out IntPtr moduleHandle);
        

        [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDiReadPorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

        [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        private unsafe static extern ErrorCode AdxDoWritePorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

        [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
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
            
            for ( int index = 0; index<=15 ;index++ )
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
            }
            else
            {
                this._moduleHandle = IntPtr.Zero;
            }
        }

        private unsafe ErrorCode DoWrite(int port, byte data)
        {
            if ( port >= this._outByteArray.Length )
                return ErrorCode.ErrorUndefined;

            this._outByteArray[port] = data;
            return AdxDoWritePorts(this._moduleHandle, port, 1, &data);
        }

        private unsafe ErrorCode DoRead(int portStart, int portCount, byte[] data)
        {
            fixed (byte* p = data)
            {
                return AdxDoReadBackPorts(this._moduleHandle, portStart, portCount, p);
            }
        }

        #endregion


        #region >>> Public Methods <<<

        public bool Open()
        {
            if (this._deviceHandle == IntPtr.Zero)
            {
                return false;
            }
            else
            {
                //this.DoWrite(0, 0xFF);
                //this.DoWrite(1, 0xFF);
                //this.DoWrite(2, 0xFF);
                //this.DoWrite(3, 0xFF);

                this.DoWrite(0, 0x00);
                this.DoWrite(1, 0x00);
                this.DoWrite(2, 0x00);
                this.DoWrite(3, 0x00);

                return true;
            }
        }
        
        public void Close()
        {
            AdxDeviceClose(this._deviceHandle);
        }

        public void BitOutput(int bitPos, int active)
        {
            int rem;
            int portNum = Math.DivRem( bitPos , 8, out rem);  // 0-base

            if (portNum >= this._outByteArray.Length)
                return;

            if ( active > 0 )
            {
                this._outByteArray[portNum] = (byte)(this._outByteArray[portNum] | (0x01 << rem));
            }
            else
            {
                this._outByteArray[portNum] = (byte)(((uint)this._outByteArray[portNum]) & (~(0x01 << rem)));
            }

            this.DoWrite(portNum, this._outByteArray[portNum]);
        }

        #endregion

    }
}
