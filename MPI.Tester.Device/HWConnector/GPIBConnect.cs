using System;
using System.Collections.Generic;
using System.Text;

using NationalInstruments.NI4882;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device
{
    public class GPIBConnect : IConnect
    {
        private object _lockObj;

        private GPIBSettingData _setting;
        private NationalInstruments.NI4882.Device _device;
        private EGPIBErrorCode _lastErrorNum;

        private bool _isQueried;
        private string _readBuffer;
        private string _lastErrorStr;
        private int _transferCount;
        private string _lastQueryCmd;

        public GPIBConnect()
        {
            this._lockObj = new object();

            this._setting = new GPIBSettingData();
            this._device = null;
            this._lastErrorNum = EGPIBErrorCode.ERR_NONE;
            this._lastErrorStr = "NONE";
            this._isQueried = false;
            this._transferCount = 0;
            this._lastQueryCmd = "";
            this._readBuffer = "";
        }

        public GPIBConnect(GPIBSettingData setting) : this()
        {
            this._setting = setting;
        }

        #region >>> Public Property <<<

        public int LastErrorNum
        {
            get { return (int)this._lastErrorNum; }
        }

        public string LastErrorStr
        {
            get { return this._lastErrorStr; }
        }

        public string BufferData
        {
            get { return this._readBuffer; }
        }

        public int TransferCount
        {
            get { return this._transferCount; }
        }

        #endregion

        #region >>> Public Method <<<

        public bool Open(out string info)
        {
            info = "";
            try
            {
                if (this._device == null)
                {
                    this._device = new NationalInstruments.NI4882.Device(this._setting.DeviceNumber, (byte)this._setting.PrimaryAddress, (byte)this._setting.SecondAddress);
                    this._device.DefaultBufferSize = _setting.BufferSize;
                    this._device.IOTimeout = (TimeoutValue)this._setting.IOTimeOut;

                    this._device.Clear();   // This resets the device’s internal functions to the default state
                }

                if (this.SendCommand("*IDN?"))
                {
                    return WaitAndGetData(out info);
                }
                else
                {
                    this._device.SerialPoll();
                    return false;
                }          
            }
            catch (Exception ex)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_OPEN;
                this._lastErrorStr = ex.Message;

                info = ex.Message;
                return false;
            }
        }

        public bool SendCommand(string cmd)
        {
            if (this._device == null)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_NO_DEVICE;
                return false;
            }

            if (cmd != "*RST" && this._lastErrorNum != EGPIBErrorCode.ERR_NONE)
            {
                return false;
            }

            if (cmd != "*RST" && this._isQueried == true)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_QUERY_INTERRUPT;
                return false;
            }

            if (cmd == "*RST")
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_NONE;
                this._isQueried = false;
            }

            try
            {
                this._device.Write(cmd + "\n");

            }
            catch (Exception ex)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_SEND_CMD;
                this._lastErrorStr = ex.Message;

                return false;
            }

            return CheckGPIBStatus();
        }

        public bool QueryCommand(string command)
        {
            if (this._device == null)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_NO_DEVICE;
                return false;
            }

            if (this._lastErrorNum != EGPIBErrorCode.ERR_NONE)
            {
                return false;
            }

            this._readBuffer = "";

            if (this._isQueried == false)
            {
                this._lastQueryCmd = command;

                if (this.SendCommand(command))
                {
                    this._isQueried = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_QUERY_INTERRUPT;
                return false;
            }
        }

        public bool WaitAndGetData(out string result)
        {
            result = "";

            if (this._device == null)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_NO_DEVICE;
                return false;
            }

            if (this._lastErrorNum != EGPIBErrorCode.ERR_NONE)
            {
                return false;
            }

            //if (this._isQueried == false)
            //{
            //    this._lastErrorNum = EGPIBErrorCode.ERR_NO_QUERY_CMD;
            //    return false;
            //}

            try
            {
                this._readBuffer = _device.ReadString().TrimEnd('\n');
                result = this._readBuffer;
            }
            catch (Exception ex)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_GET_DATA;
                this._lastErrorStr = ex.Message;
                return false;
            }

            this._isQueried = false;
            return CheckGPIBStatus();
        }

        public bool WaitAndGetData(int count, out string result)
        {
            result = "";

            if (this._device == null)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_NO_DEVICE;
                return false;
            }

            if (this._lastErrorNum != EGPIBErrorCode.ERR_NONE)
            {
                return false;
            }

            //if (this._isQueried == false)
            //{
            //    this._lastErrorNum = EGPIBErrorCode.ERR_NO_QUERY_CMD;
            //    return false;
            //}

            try
            {
                this._readBuffer = _device.ReadString(count);

                result = this._readBuffer;
            }
            catch (Exception ex)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_GET_DATA;
                this._lastErrorStr = ex.Message;
                return false;
            }

            this._isQueried = false;
            return CheckGPIBStatus();
        }

        public void Close()
        {
            try
            {
                this._device.Reset();
                this._device.Dispose();
            }
            catch (Exception ex)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_CLOSE;
                this._lastErrorStr = ex.Message;
            }
        }
        #endregion

        #region >>> Private Methods <<<

        private bool CheckGPIBStatus()
        {
            if ((((int)this._device.LastStatus) & ((int)ibsta_bits.ERR)) > 0)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_GPIB_CONN;
                this._lastErrorStr = "GPIB Error";
                return false;
            }
            if ((((int)this._device.LastStatus) & ((int)ibsta_bits.TIMO)) > 0)
            {
                this._lastErrorNum = EGPIBErrorCode.ERR_GPIB_TIMEOUT;
                this._lastErrorStr = "GPIB TimeOut";
                return false;
            }

            return true;
        }

        #endregion

        public enum ibsta_bits : int
        {
            DCAS = (1 << 0),	/* device clear state */
            DTAS = (1 << 1),	/* device trigger state */
            LACS = (1 << 2),	/* GPIB interface is addressed as Listener */
            TACS = (1 << 3),	/* GPIB interface is addressed as Talker */
            ATN = (1 << 4),	    /* Attention is asserted */
            CIC = (1 << 5),	    /* GPIB interface is Controller-in-Charge */
            REM = (1 << 6),	    /* remote state */
            LOK = (1 << 7),	    /* lockout state */
            CMPL = (1 << 8),	/* I/O is complete  */
            EVENT = (1 << 9),	/* DCAS, DTAS, or IFC has occurred */
            SPOLL = (1 << 10),	/* board serial polled by busmaster */
            RQS = (1 << 11),	/* Device requesting service  */
            SRQI = (1 << 12),	/* SRQ is asserted */
            END = (1 << 13),	/* EOI or EOS encountered */
            TIMO = (1 << 14),	/* Time limit on I/O or wait function exceeded */
            ERR = (1 << 15)	    /* Function call terminated on error */
        }

        public enum EGPIBErrorCode : int
        {
            ERR_NONE = 0,
            ERR_NO_DEVICE = 50,
            ERR_OPEN = 51,
            ERR_CLOSE = 52,
            ERR_SEND_CMD = 53,
            ERR_GET_DATA = 54,
            ERR_GPIB_CONN = 55,
            ERR_GPIB_TIMEOUT = 56,
            ERR_WRITE = 57,
            ERR_READ = 58,
            ERR_QUERY_INTERRUPT = 59,
            ERR_NO_QUERY_CMD = 60,
        }
    }
}
